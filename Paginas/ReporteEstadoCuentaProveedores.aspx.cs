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

public partial class ReporteEstadoCuentaProveedores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

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
    public static string ObtenerFormaFiltroReporteEstadoCuentaProveedores()
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
    public static string ObtieneEstadoCuentaProveedor(int pIdProveedor, string pFechaInicial, int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;

            CSelectEspecifico ConsultaReporteEstadoCuentaProveedor = new CSelectEspecifico();
            ConsultaReporteEstadoCuentaProveedor.StoredProcedure.CommandText = "SP_Impresion_EstadoCuentaProveedor";
            ConsultaReporteEstadoCuentaProveedor.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteEstadoCuentaProveedor.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", pIdProveedor);
            ConsultaReporteEstadoCuentaProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCuentaProveedor.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteEstadoCuentaProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCuentaProveedor.Llena(ConexionBaseDatos);

            if (ConsultaReporteEstadoCuentaProveedor.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["RazonSocialReceptor"]));
                Modelo.Add("SALDOINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SaldoInicial"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Fecha"]));
                Modelo.Add("FECHAINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["FechaInicial"]));
                Modelo.Add("FECHAFINAL", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["FechaFinal"]));
                Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TipoMoneda"]));
                Modelo.Add("ESTADOCUENTA", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["EstadoCuenta"]));
                Modelo.Add("PROVEEDOR", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Proveedor"]));
                Modelo.Add("NOMBRE", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Nombre"]));
                Modelo.Add("SUMACARGOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SumaCargos"]));
                Modelo.Add("SUMAABONOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SumaAbonos"]));
                Modelo.Add("SALDOFINAL", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SaldoFinal"]));
                Modelo.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["CorrientePesos"]));
                Modelo.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["UnoTreintaPesos"]));
                Modelo.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TreintaSesentaPesos"]));
                Modelo.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SesentaNoventaPesos"]));
                Modelo.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["MasNoventaPesos"]));
                Modelo.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TotalVencidoPesos"]));

                Modelo.Add("SALDOFINALD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SaldoFinalD"]));
                Modelo.Add("CORRIENTED", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["CorrienteD"]));
                Modelo.Add("UNOTREINTAD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["UnoTreintaD"]));
                Modelo.Add("TREINTASESENTAD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TreintaSesentaD"]));
                Modelo.Add("SESENTANOVENTAD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SesentaNoventaD"]));
                Modelo.Add("MASNOVENTAD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["MasNoventaD"]));
                Modelo.Add("TOTALVENCIDOD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TotalVencidoD"]));

                Modelo.Add("MONTOPORAPLICAR", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["MontoPorAplicar"]));
                Modelo.Add("MONTOPORAPLICARDOLARES", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["MontoPorAplicarD"]));
            }

            if (ConsultaReporteEstadoCuentaProveedor.Registros.NextResult())
            {
                JArray JAMovimientos = new JArray();
                while (ConsultaReporteEstadoCuentaProveedor.Registros.Read())
                {
                    JObject JMovimiento = new JObject();
                    JMovimiento.Add("FECHAMOV", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["FechaMovimiento"]));
                    JMovimiento.Add("SERIE", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Serie"]));
                    JMovimiento.Add("FOLIO", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Folio"]));
                    JMovimiento.Add("CONCEPTO", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Concepto"]));
                    JMovimiento.Add("CARGOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Cargos"]));
                    JMovimiento.Add("ABONOS", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["Abonos"]));
                    JMovimiento.Add("SALDODOC", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SaldoDocumento"]));
                    JMovimiento.Add("FECHAVEN", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["FechaVencimiento"]));
                    JMovimiento.Add("DIASVENC", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["DiasVencidos"]));
                    JMovimiento.Add("MONEDA", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TM"]));
                    JMovimiento.Add("TC", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TC"]));
                    JMovimiento.Add("REFERENCIA", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["REF"]));
                    JAMovimientos.Add(JMovimiento);
                }
                Modelo.Add("Movimientos", JAMovimientos);
            }

            if (ConsultaReporteEstadoCuentaProveedor.Registros.NextResult())
            {
                JArray JAMovimientosD = new JArray();
                while (ConsultaReporteEstadoCuentaProveedor.Registros.Read())
                {
                    JObject JMovimientoD = new JObject();
                    JMovimientoD.Add("FECHAMOVD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["FechaMovimientoD"]));
                    JMovimientoD.Add("SERIED", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SerieD"]));
                    JMovimientoD.Add("FOLIOD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["FolioD"]));
                    JMovimientoD.Add("CONCEPTOD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["ConceptoD"]));
                    JMovimientoD.Add("CARGOSD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["CargosD"]));
                    JMovimientoD.Add("ABONOSD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["AbonosD"]));
                    JMovimientoD.Add("SALDODOCD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["SaldoDocumentoD"]));
                    JMovimientoD.Add("FECHAVEND", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["FechaVencimientoD"]));
                    JMovimientoD.Add("DIASVENCD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["DiasVencidosD"]));
                    JMovimientoD.Add("MONEDAD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TMD"]));
                    JMovimientoD.Add("TCD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["TCD"]));
                    JMovimientoD.Add("REFERENCIAD", Convert.ToString(ConsultaReporteEstadoCuentaProveedor.Registros["REFD"]));
                    JAMovimientosD.Add(JMovimientoD);
                }
                Modelo.Add("MovimientosD", JAMovimientosD);
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