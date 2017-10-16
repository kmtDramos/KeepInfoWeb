using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using arquitecturaNet;
using System.IO;
using System.Diagnostics;

public partial class ReporteCuentasPorPagarPorFechaFactura : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    public static string ImprimirEstadoCuentaProveedor(int pIdProveedor, string pTemplate, string pFechaInicial, int pIdSucursal, int pTipoImpresion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUtilerias Util = new CUtilerias();

        int idUsuario;
        int idSucursal;
        int idEmpresa;
        string logoEmpresa;

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;
        idSucursal = Sucursal.IdSucursal;
        idEmpresa = Empresa.IdEmpresa;
        logoEmpresa = Empresa.Logo;

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("ImpresionDocumento", pTemplate);

        CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
        ImpresionDocumento.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTempl = new Dictionary<string, object>();
        //ParametrosTempl.Add("IdEmpresa", idEmpresa);
        ParametrosTempl.Add("Baja", 0);
        ParametrosTempl.Add("IdImpresionDocumento", ImpresionDocumento.IdImpresionDocumento);

        CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
        ImpresionTemplate.LlenaObjetoFiltros(ParametrosTempl, ConexionBaseDatos);

        JObject datos = CReportesKeep.obtenerDatosImpresionEstadoCuentaProveedor(pIdProveedor.ToString(), Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pFechaInicial, pIdSucursal);

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "EstadoCuentaProveedor_" + pIdProveedor.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
        string rutaTemplate = HttpContext.Current.Server.MapPath("~/Archivos/TemplatesImpresion/" + ImpresionTemplate.RutaTemplate);
        string rutaCSS = HttpContext.Current.Server.MapPath("~/Archivos/TemplatesImpresion/" + ImpresionTemplate.RutaCSS);
        string imagenLogo = HttpContext.Current.Server.MapPath("~/Archivos/EmpresaLogo/") + logoEmpresa;

        if (!File.Exists(rutaTemplate))
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay un template válido para esta empresa."));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add(new JProperty("Archivo", Util.ReportePDFTemplateConceptos(rutaPDF, rutaTemplate, rutaCSS, imagenLogo, ImpresionTemplate.IdImpresionTemplate, datos, ConexionBaseDatos, pTipoImpresion)));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));

            HttpContext.Current.Application.Set("PDFDescargar", Path.GetFileName(rutaPDF));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    protected void btnDescarga_Click(object sender, EventArgs e)
    {
        string PDFDescarga = HttpContext.Current.Application.Get("PDFDescargar").ToString();
        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + PDFDescarga);
        Response.WriteFile((HttpContext.Current.Server.MapPath("../Archivos/Impresiones/" + PDFDescarga)));
        Response.Flush();
        Response.End();
    }

    [WebMethod]
    public static string ObtenerFormaFiltroReporteCuentasPorPagarPorFechaFactura()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CSucursal SucursalActual = new CSucursal();
        SucursalActual.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        JObject oPermisos = new JObject();
        int puedeVerSucursales = 0;

        if (Usuario.TienePermisos(new string[] { "puedeVerSucursales" }, ConexionBaseDatos) == "")
        {
            puedeVerSucursales = 1;
        }
        oPermisos.Add("puedeVerSucursales", puedeVerSucursales);

        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;
        DateTime FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime FechaFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        Modelo.Add("FechaInicial", Convert.ToString(Fecha.ToShortDateString()));

        JArray JASucursales = new JArray();
        foreach (CSucursal oSucursal in SucursalActual.ObtenerSucursalesAsignadas(Usuario.IdUsuario, SucursalActual.IdEmpresa, ConexionBaseDatos))
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("Valor", oSucursal.IdSucursal);
            JSucursal.Add("Descripcion", oSucursal.Sucursal);
            if (SucursalActual.IdSucursal == oSucursal.IdSucursal)
            {
                JSucursal.Add("Selected", 1);
            }
            else
            {
                JSucursal.Add("Selected", 0);
            }
            JASucursales.Add(JSucursal);
        }
        Modelo.Add("Sucursales", JASucursales);
        Modelo.Add(new JProperty("Permisos", oPermisos));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneEstadoCuentasPorCobrar(int pIdProveedor, string pFechaInicial, int pIdSucursal, string pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;

            CSelectEspecifico ConsultaReporteCuentasPorPagarPorFechaFactura = new CSelectEspecifico();
            ConsultaReporteCuentasPorPagarPorFechaFactura.StoredProcedure.CommandText = "SP_Impresion_CuentasPorPagarPorFechaFactura";
            ConsultaReporteCuentasPorPagarPorFechaFactura.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteCuentasPorPagarPorFechaFactura.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", Convert.ToInt32(pIdProveedor));
            ConsultaReporteCuentasPorPagarPorFechaFactura.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteCuentasPorPagarPorFechaFactura.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteCuentasPorPagarPorFechaFactura.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteCuentasPorPagarPorFechaFactura.Llena(ConexionBaseDatos);

            if (ConsultaReporteCuentasPorPagarPorFechaFactura.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["RazonSocialReceptor"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Fecha"]));
                Modelo.Add("ESTADOCUENTA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Reporte"]));
                if (pIdSucursal != 0)
                {
                    Modelo.Add("SUCURSAL", Convert.ToString(pSucursal).ToUpper());
                }
                else
                {
                    Modelo.Add("SUCURSAL", "TODAS");
                }
            }

            if (ConsultaReporteCuentasPorPagarPorFechaFactura.Registros.NextResult())
            {
                JArray JAMovimientos = new JArray();
                JArray JAProveedor = new JArray();
                string Proveedor = "";
                decimal dTotales = 0;
                JObject JMProveedor = new JObject();


                while (ConsultaReporteCuentasPorPagarPorFechaFactura.Registros.Read())
                {
                    //Creamos list proveedor
                    if (Proveedor != Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Proveedor"]))
                    {
                        if (JAMovimientos.Count > 0)
                        {
                            JMProveedor.Add("Total", string.Format("{0:c}", dTotales));
                            dTotales = 0;
                            JMProveedor.Add("Movimientos", JAMovimientos);
                            JAProveedor.Add(JMProveedor);
                            JMProveedor = new JObject();
                            JAMovimientos = new JArray();
                        }

                        Proveedor = Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Proveedor"]);
                        JMProveedor.Add("PROVEEDOR", Proveedor);

                        JObject JMovimiento = new JObject();
                        JMovimiento.Add("FOLIO", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Folio"]));
                        JMovimiento.Add("PROVEEDOR", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Proveedor"]));
                        JMovimiento.Add("FECHA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Fecha"]));
                        JMovimiento.Add("FECHAPAGO", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["FechaPago"]));
                        JMovimiento.Add("SUBTOTAL", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Subtotal"]));
                        JMovimiento.Add("IVA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["IVA"]));
                        JMovimiento.Add("TOTAL", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Total"]));
                        JMovimiento.Add("SALDO", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Saldo"]));
                        JMovimiento.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["TipoMoneda"]));
                        JMovimiento.Add("SALDOENPESOS", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["SaldoEnPesos"]));
                        JAMovimientos.Add(JMovimiento);
                    }
                    else
                    {
                        JObject JMovimiento = new JObject();
                        JMovimiento.Add("FOLIO", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Folio"]));
                        JMovimiento.Add("PROVEEDOR", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Proveedor"]));
                        JMovimiento.Add("FECHA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Fecha"]));
                        JMovimiento.Add("FECHAPAGO", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["FechaPago"]));
                        JMovimiento.Add("SUBTOTAL", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Subtotal"]));
                        JMovimiento.Add("IVA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["IVA"]));
                        JMovimiento.Add("TOTAL", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Total"]));
                        JMovimiento.Add("SALDO", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["Saldo"]));
                        JMovimiento.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["TipoMoneda"]));
                        JMovimiento.Add("SALDOENPESOS", Convert.ToString(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["SaldoEnPesos"]));
                        JAMovimientos.Add(JMovimiento);
                    }
                    dTotales = dTotales + Convert.ToDecimal(ConsultaReporteCuentasPorPagarPorFechaFactura.Registros["SaldoEnPesos"].ToString().Replace("$", "").Replace(",", ""));

                }

                JMProveedor.Add("Total", string.Format("{0:c}", dTotales));
                JMProveedor.Add("Movimientos", JAMovimientos);
                JAProveedor.Add(JMProveedor);
                Modelo.Add("Proveedor", JAProveedor);
            }


            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));


        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a base de datos"));
        }


        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    //Busquedas
    [WebMethod]
    public static string BuscarRazonSocialProveedor(string pRazonSocial, int pIdSucursal)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proveedor_ConsultarRazonSocial_Reportes";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        string jsonOrganizacionString = jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacionString;

    }
}