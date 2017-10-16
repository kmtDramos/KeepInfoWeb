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

public partial class ReporteCuentasBancariasPorRangoFechas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    public static string ImprimirEstadoCuentaBancaria(int pIdCuentaBancaria, string pTemplate, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdTipoCuenta, string pFechaIni, string pFechaF, string pSucursal, int pTipoImpresion)
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

        JObject datos = CReportesKeep.obtenerDatosImpresionEstadoCuentaBancaria(pIdCuentaBancaria.ToString(), Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pFechaInicial, pFechaFinal, pIdSucursal, pIdTipoCuenta, pFechaIni, pFechaF, pSucursal);

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "EstadoCuentaBancaria_" + pIdCuentaBancaria.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
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
    public static string ObtenerFormaFiltroReporteCuentasBancarias()
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
    public static string ObtieneReporteCuentasBancarias(int pIdCuentaBancaria, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdTipoCuenta, string pFechaIni, string pFechaF, string pSucursal)
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

            CSelectEspecifico ConsultaReporteEstadoCuentaBancaria = new CSelectEspecifico();
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.CommandText = "SP_Impresion_CuentasBancarias";
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", pIdCuentaBancaria);
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pIdTipoCuenta", pIdTipoCuenta);
            if (pIdSucursal == 0)
            {
                ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pSucursal", "TODAS");
            }
            else
            {
                ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pSucursal", pSucursal.ToUpper());
            }
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
            ConsultaReporteEstadoCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaFin", pFechaF);
            ConsultaReporteEstadoCuentaBancaria.Llena(ConexionBaseDatos);




            if (ConsultaReporteEstadoCuentaBancaria.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["RazonSocialReceptor"]));
                Modelo.Add("SALDOINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["SaldoInicial"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Fecha"]));
                Modelo.Add("ESTADOCUENTA", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["EstadoCuenta"]));
                Modelo.Add("CUENTABANCARIA", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["CuentaBancaria"]).Replace(",",", "));
                Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["TipoMoneda"]));
                Modelo.Add("SUMACARGOS", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["SumaCargos"]));
                Modelo.Add("SUMAABONOS", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["SumaAbonos"]));
                Modelo.Add("SALDOFINAL", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["SaldoFinal"]));
                Modelo.Add("SUCURSAL", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Sucursal"]));
                Modelo.Add("FECHAINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["FormatoFechaIni"]));
                Modelo.Add("FECHAFINAL", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["FormatoFechaFin"]));
            }

            if (ConsultaReporteEstadoCuentaBancaria.Registros.NextResult())
            {
                JArray JAMovimientos = new JArray();
                while (ConsultaReporteEstadoCuentaBancaria.Registros.Read())
                {
                    JObject JMovomiento = new JObject();
                    JMovomiento.Add("FECHAMOV", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["FechaMovimiento"]));
                    JMovomiento.Add("TIPO", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Tipo"]));
                    JMovomiento.Add("FOLIO", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Folio"]));
                    JMovomiento.Add("BENEFICIARIO", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Beneficiario"]).Replace(",", ", "));
                    JMovomiento.Add("CONCEPTO", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Concepto"]).Replace(",", ", "));
                    JMovomiento.Add("REFERENCIA", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Referencia"]).Replace(",", ", "));
                    JMovomiento.Add("CARGOS", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Cargos"]));
                    JMovomiento.Add("ABONOS", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Abonos"]));
                    JMovomiento.Add("SALDODOC", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["SaldoDocumento"]));
                    JMovomiento.Add("CONCILIADOS", Convert.ToString(ConsultaReporteEstadoCuentaBancaria.Registros["Conciliados"]));
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
    public static string BuscarCuentaBancaria(string pCuentaBancaria, int pIdSucursal)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        COrganizacion jsonCuentaBancaria = new COrganizacion();
        jsonCuentaBancaria.StoredProcedure.CommandText = "sp_ConsultarCuentasBancarias_Reportes";
        jsonCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", pCuentaBancaria);
        jsonCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonCuentaBancaria.ObtenerJsonRazonSocial(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }
}