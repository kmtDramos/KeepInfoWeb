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

public partial class ReporteEstadoCuentaClientes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    }

    [WebMethod]
    public static string ImprimirEstadoCuenta(int pIdCliente, string pTemplate, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdTipoCambio, int pTipoImpresion)
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

        JObject datos = CReportesKeep.obtenerDatosImpresionEstadoCuentaCliente(pIdCliente.ToString(), Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pFechaInicial, pFechaFinal, pIdSucursal, pIdTipoCambio);

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "EstadoCuentaCliente_" + pIdCliente.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
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
    public static string ObtenerFormaFiltroReporteEstadoCuentaClientes()
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
        Modelo.Add("FechaInicial", Convert.ToString(FechaInicial.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(FechaFinal.ToShortDateString()));

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
    public static string ObtieneEstadoCuentaCliente(int pIdCliente, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

            CSelectEspecifico ConsultaReporteEstadoCuentaCliente = new CSelectEspecifico();
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.CommandText = "SP_Impresion_EstadoCuentaCliente";
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pIdCliente", pIdCliente);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambio", pIdTipoCambio);
            ConsultaReporteEstadoCuentaCliente.Llena(ConexionBaseDatos);

            if (ConsultaReporteEstadoCuentaCliente.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["RazonSocialReceptor"]));
                Modelo.Add("SALDOINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoInicial"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Fecha"]));
                Modelo.Add("FECHAINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaInicial"]));
                Modelo.Add("FECHAFINAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaFinal"]));
                Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TipoMoneda"]));
                Modelo.Add("ESTADOCUENTA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["EstadoCuenta"]));
                Modelo.Add("CLIENTE", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Cliente"]));
                Modelo.Add("NOMBRE", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Nombre"]));
                Modelo.Add("SUMACARGOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SumaCargos"]));
                Modelo.Add("SUMAABONOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SumaAbonos"]));
                Modelo.Add("SALDOFINAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoFinal"]));

            }

            if (ConsultaReporteEstadoCuentaCliente.Registros.NextResult())
            {
                JArray JAMovimientos = new JArray();
                while (ConsultaReporteEstadoCuentaCliente.Registros.Read())
                {
                    JObject JMovomiento = new JObject();
                    JMovomiento.Add("FECHAMOV", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaMovimiento"]));
                    JMovomiento.Add("SERIE", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Serie"]));
                    JMovomiento.Add("FOLIO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Folio"]));
                    JMovomiento.Add("CONCEPTO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Concepto"]));
                    JMovomiento.Add("TIPONOTACREDITO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TipoNotaCredito"]));
                    JMovomiento.Add("CARGOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Cargos"]));
                    JMovomiento.Add("ABONOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Abonos"]));
                    JMovomiento.Add("SALDODOC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoDocumento"]));
                    JMovomiento.Add("FECHAVEN", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaVencimiento"]));
                    JMovomiento.Add("MONEDA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TM"]));
                    JMovomiento.Add("TC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TC"]));
                    JMovomiento.Add("REFERENCIA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["REF"]));
                    JMovomiento.Add("MONTOAPLICADO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["MontoAplicado"]));
                    JAMovimientos.Add(JMovomiento);
                }
                Modelo.Add("Movimientos", JAMovimientos);
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

    [WebMethod]
    public static string BuscarRazonSocial(string pRazonSocial, int pIdSucursal)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        COrganizacion jsonRazonSocial = new COrganizacion();
        jsonRazonSocial.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial_Reportes";
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }
}