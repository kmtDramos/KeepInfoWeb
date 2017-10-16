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

public partial class ReporteEstadoCuentaClientesPesosDolares : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    }

    [WebMethod]
    public static string ImprimirEstadoCuentaPesosDolares(int pIdCliente, string pTemplate, string pFechaInicial, int pIdSucursal, int pTipoImpresion)
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

        JObject datos = CReportesKeep.obtenerDatosImpresionEstadoCuentaClientePesosDolares(pIdCliente.ToString(), Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pFechaInicial, pIdSucursal);

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
    public static string ObtenerFormaFiltroReporteEstadoCuentaClientesPesosDolares()
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
    public static string ObtieneEstadoCuentaClientePesosDolares(int pIdCliente, string pFechaInicial, int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;

            CSelectEspecifico ConsultaReporteEstadoCuentaCliente = new CSelectEspecifico();
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.CommandText = "SP_Impresion_EstadoCuentaClientePesosDolares";
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pIdCliente", pIdCliente);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            //ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
            ConsultaReporteEstadoCuentaCliente.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
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
                Modelo.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["CorrientePesos"]));
                Modelo.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["UnoTreintaPesos"]));
                Modelo.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TreintaSesentaPesos"]));
                Modelo.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SesentaNoventaPesos"]));
                Modelo.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["MasNoventaPesos"]));
                Modelo.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TotalVencidoPesos"]));

                Modelo.Add("SALDOFINALD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoFinalD"]));
                Modelo.Add("CORRIENTED", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["CorrienteD"]));
                Modelo.Add("UNOTREINTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["UnoTreintaD"]));
                Modelo.Add("TREINTASESENTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TreintaSesentaD"]));
                Modelo.Add("SESENTANOVENTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SesentaNoventaD"]));
                Modelo.Add("MASNOVENTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["MasNoventaD"]));
                Modelo.Add("TOTALVENCIDOD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TotalVencidoD"]));

                Modelo.Add("MONTOPORAPLICAR", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["MontoPorAplicar"]));
                Modelo.Add("MONTOPORAPLICARDOLARES", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["MontoPorAplicarD"]));
            }

            if (ConsultaReporteEstadoCuentaCliente.Registros.NextResult())
            {
                JArray JAMovimientos = new JArray();
                while (ConsultaReporteEstadoCuentaCliente.Registros.Read())
                {
                    JObject JMovimiento = new JObject();
                    JMovimiento.Add("FECHAMOV", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaMovimiento"]));
                    JMovimiento.Add("SERIE", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Serie"]));
                    JMovimiento.Add("FOLIO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Folio"]));
                    JMovimiento.Add("CONCEPTO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Concepto"]));
                    JMovimiento.Add("CARGOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Cargos"]));
                    JMovimiento.Add("ABONOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Abonos"]));
                    JMovimiento.Add("SALDODOC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoDocumento"]));
                    JMovimiento.Add("FECHAVEN", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaVencimiento"]));
                    JMovimiento.Add("DIASVENC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["DiasVencidos"]));
                    JMovimiento.Add("MONEDA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TM"]));
                    JMovimiento.Add("TC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TC"]));
                    JMovimiento.Add("REFERENCIA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["REF"]));
                    JAMovimientos.Add(JMovimiento);
                }
                Modelo.Add("Movimientos", JAMovimientos);
            }

            if (ConsultaReporteEstadoCuentaCliente.Registros.NextResult())
            {
                JArray JAMovimientosD = new JArray();
                while (ConsultaReporteEstadoCuentaCliente.Registros.Read())
                {
                    JObject JMovimientoD = new JObject();
                    JMovimientoD.Add("FECHAMOVD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaMovimientoD"]));
                    JMovimientoD.Add("SERIED", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SerieD"]));
                    JMovimientoD.Add("FOLIOD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FolioD"]));
                    JMovimientoD.Add("CONCEPTOD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["ConceptoD"]));
                    JMovimientoD.Add("CARGOSD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["CargosD"]));
                    JMovimientoD.Add("ABONOSD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["AbonosD"]));
                    JMovimientoD.Add("SALDODOCD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoDocumentoD"]));
                    JMovimientoD.Add("FECHAVEND", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaVencimientoD"]));
                    JMovimientoD.Add("DIASVENCD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["DiasVencidosD"]));
                    JMovimientoD.Add("MONEDAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TMD"]));
                    JMovimientoD.Add("TCD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TCD"]));
                    JMovimientoD.Add("REFERENCIAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["REFD"]));
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