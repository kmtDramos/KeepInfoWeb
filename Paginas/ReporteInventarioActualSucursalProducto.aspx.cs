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

public partial class ReporteInventarioActualSucursalProducto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    }

    [WebMethod]
    public static string ImprimirInventarioActualSucursalProducto(string pTemplate, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdProducto, int pTipoImpresion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUtilerias Util = new CUtilerias();

        int idUsuario;
        int idSucursal;
        int idEmpresa;
        int idProducto;
        string logoEmpresa;

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        CProducto Producto = new CProducto();
        Producto.LlenaObjeto(pIdProducto, ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;
        idSucursal = Sucursal.IdSucursal;
        idProducto = Producto.IdProducto;
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

        JObject datos = CReportesKeep.obtenerDatosImpresionInventarioActualSucursalProducto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pFechaInicial, pFechaFinal, pIdSucursal, pIdProducto);

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "InventarioActualSucursalProducto_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
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
            try
            {
                JObject Modelo = new JObject();

                Modelo.Add(new JProperty("Archivo", Util.ReportePDFTemplateConceptos(rutaPDF, rutaTemplate, rutaCSS, imagenLogo, ImpresionTemplate.IdImpresionTemplate, datos, ConexionBaseDatos, pTipoImpresion)));
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));

                HttpContext.Current.Application.Set("PDFDescargar", Path.GetFileName(rutaPDF));
            }
            catch (Exception ex) { }
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
    public static string BuscarClave(string pClave)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoProyecto = new CJson();
        jsonTipoProyecto.StoredProcedure.CommandText = "sp_Producto_ConsultarFiltros";
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pClave));
        return jsonTipoProyecto.ObtenerJsonString(ConexionBaseDatos);
        //asegurarme de regresar lso tres vbalores q pido en js
    }

    [WebMethod]
    public static string ObtenerFormaFiltroReporteInventarioActualSucursalProducto()
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

        CSelectEspecifico ConsultaReporteSucursalProducto = new CSelectEspecifico();
        ConsultaReporteSucursalProducto.StoredProcedure.CommandText = "sp_Reportes_ConsultarSucursales";
        ConsultaReporteSucursalProducto.Llena(ConexionBaseDatos);

        JArray JASucursal = new JArray();
        while (ConsultaReporteSucursalProducto.Registros.Read())
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("IdSucursal", Convert.ToInt32(ConsultaReporteSucursalProducto.Registros[0]));
            JSucursal.Add("Sucursal", Convert.ToString(ConsultaReporteSucursalProducto.Registros[1]));
            JSucursal.Add("Empresa", Convert.ToString(ConsultaReporteSucursalProducto.Registros[2]));
            JASucursal.Add(JSucursal);
        }
        Modelo.Add("Sucursales", JASucursal);

        Modelo.Add(new JProperty("Permisos", oPermisos));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneInventarioActualSucursalProducto(int pIdSucursal, int pIdProducto, string pFechaInicial, string pFechaFinal)
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

            CSelectEspecifico ConsultaReporteInventarioActualSucursalProducto = new CSelectEspecifico();
            ConsultaReporteInventarioActualSucursalProducto.StoredProcedure.CommandText = "SSP_Impresion_InventarioActualSucursalProducto";
            //ConsultaReporteInventarioActualSucursalProducto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteInventarioActualSucursalProducto.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteInventarioActualSucursalProducto.StoredProcedure.Parameters.AddWithValue("@pIdProducto", pIdProducto);
            ConsultaReporteInventarioActualSucursalProducto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteInventarioActualSucursalProducto.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", "");
            ConsultaReporteInventarioActualSucursalProducto.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", "");
            ConsultaReporteInventarioActualSucursalProducto.Llena(ConexionBaseDatos);

            if (ConsultaReporteInventarioActualSucursalProducto.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["RazonSocialReceptor"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["Fecha"]));
                Modelo.Add("FECHAINICIAL", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["FechaInicial"]));
                Modelo.Add("FECHAFINAL", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["FechaFinal"]));
                Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["TipoMoneda"]));
                Modelo.Add("REPORTE", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["Reporte"]));
                Modelo.Add("SUCURSAL", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["SucursalEncabezado"]));
                Modelo.Add("EMPRESA", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["Empresa"]));
            }

            if (ConsultaReporteInventarioActualSucursalProducto.Registros.NextResult())
            {
                JArray JAMovimientos = new JArray();
                while (ConsultaReporteInventarioActualSucursalProducto.Registros.Read())
                {
                    JObject JMovomiento = new JObject();
                    JMovomiento.Add("IdProducto", Convert.ToInt32(ConsultaReporteInventarioActualSucursalProducto.Registros["IdProducto"]));
                    JMovomiento.Add("CLAVE", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["Codigo"]));
                    JMovomiento.Add("PRODUCTO", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["producto"]));
                    JMovomiento.Add("UNIDADCOMPRAVENTA", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["UnidadCompraVenta"]));
                    JMovomiento.Add("ALMACEN", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["Almacen"]));
                    JMovomiento.Add("SUCURSAL", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["Sucursal"]));
                    JMovomiento.Add("SALDOACTUAL", Convert.ToString(ConsultaReporteInventarioActualSucursalProducto.Registros["saldo"]));
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

    //[WebMethod]
    //public static string BuscarRazonSocial(string pRazonSocial, int pIdSucursal)
    //{
    //    //Abrir Conexion
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
    //    CUsuario Usuario = new CUsuario();
    //    Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

    //    COrganizacion jsonRazonSocial = new COrganizacion();
    //    jsonRazonSocial.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial_Reportes";
    //    jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
    //    jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
    //    jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal); 
    //    jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
    //    return jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);

    //    //Cerrar Conexion
    //    ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //    return respuesta;
    //}



}