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

public partial class ReporteInventarioActualAlmacenProducto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    }

    [WebMethod]
    public static string ImprimirInventarioActualAlmacenProducto(string pTemplate, string pFechaInicial, string pFechaFinal, int pIdAlmacen, int pTipoImpresion)
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

        JObject datos = CReportesKeep.obtenerDatosImpresionInventarioActualAlmacenProducto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pFechaInicial, pFechaFinal, pIdAlmacen);

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "InventarioActualAlmacenProducto_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
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

            Modelo.Add(new JProperty("Archivo", Util.ReportePDFTemplateConceptosInventario(rutaPDF, rutaTemplate, rutaCSS, imagenLogo, ImpresionTemplate.IdImpresionTemplate, datos, ConexionBaseDatos, pTipoImpresion)));
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
    public static string ObtenerFormaFiltroReporteInventarioActualAlmacenProducto()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CSucursal SucursalActual = new CSucursal();
        SucursalActual.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        JObject oPermisos = new JObject();
        int puedeVerAlmacenes = 0;

        if (Usuario.TienePermisos(new string[] { "puedeVerAlmacenes" }, ConexionBaseDatos) == "")
        {
            puedeVerAlmacenes = 1;
        }
        oPermisos.Add("puedeVerAlmacenes", puedeVerAlmacenes);

        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;
        DateTime FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime FechaFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        Modelo.Add("FechaInicial", Convert.ToString(FechaInicial.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(FechaFinal.ToShortDateString()));

        CAlmacen AlmacenActual = new CAlmacen();

        JArray JAAlmacenes = new JArray();
        foreach (CAlmacen oAlmacen in AlmacenActual.ObtenerAlmacenesAsignadas(Usuario.IdUsuario, ConexionBaseDatos))
        {
            JObject JAlmacen = new JObject();
            JAlmacen.Add("IdAlmacen", oAlmacen.IdAlmacen);
            JAlmacen.Add("Almacen", oAlmacen.Almacen);
            if (AlmacenActual.IdAlmacen == oAlmacen.IdAlmacen)
            {
                JAlmacen.Add("Selected", 1);
            }
            else
            {
                JAlmacen.Add("Selected", 0);
            }
            JAAlmacenes.Add(JAlmacen);
        }
        Modelo.Add("Almacenes", JAAlmacenes);

        Modelo.Add(new JProperty("Permisos", oPermisos));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneInventarioActualAlmacenProducto(int pIdAlmacen, string pFechaInicial, string pFechaFinal)
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

            CSelectEspecifico ConsultaReporteInventarioActualAlmacenProducto = new CSelectEspecifico();
            ConsultaReporteInventarioActualAlmacenProducto.StoredProcedure.CommandText = "SP_Impresion_InventarioActualAlmacenProducto";
            ConsultaReporteInventarioActualAlmacenProducto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteInventarioActualAlmacenProducto.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
            ConsultaReporteInventarioActualAlmacenProducto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteInventarioActualAlmacenProducto.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteInventarioActualAlmacenProducto.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
            ConsultaReporteInventarioActualAlmacenProducto.Llena(ConexionBaseDatos);

            if (ConsultaReporteInventarioActualAlmacenProducto.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["RazonSocialReceptor"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Fecha"]));
                Modelo.Add("FECHAINICIAL", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["FechaInicial"]));
                Modelo.Add("FECHAFINAL", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["FechaFinal"]));
                Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["TipoMoneda"]));
                Modelo.Add("REPORTE", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Reporte"]));
                Modelo.Add("ALMACEN", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Almacen"]));

            }

            if (ConsultaReporteInventarioActualAlmacenProducto.Registros.NextResult())
            {
                JArray JAMovimientos = new JArray();
                while (ConsultaReporteInventarioActualAlmacenProducto.Registros.Read())
                {
                    JObject JMovomiento = new JObject();
                    JMovomiento.Add("CLAVE", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Clave"]));
                    JMovomiento.Add("PRODUCTO", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Producto"]));
                    JMovomiento.Add("COSTEO", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Costeo"]));
                    JMovomiento.Add("UNIDADCOMPRAVENTA", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["UnidadCompraVenta"]));
                    JMovomiento.Add("INVENTARIOINICIAL", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["InventarioInicial"]));
                    JMovomiento.Add("ENTRADAS", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Entradas"]));
                    JMovomiento.Add("SALIDAS", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Salidas"]));
                    JMovomiento.Add("EXISTENCIA", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["Existencia"]));
                    JMovomiento.Add("INVENTARIOINICIALIMPORTE", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["InventarioInicialImporte"]));
                    JMovomiento.Add("ENTRADASIMPORTE", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["EntradasImporte"]));
                    JMovomiento.Add("SALIDASIMPORTE", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["SalidasImporte"]));
                    JMovomiento.Add("EXISTENCIAIMPORTE", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["ExistenciaImporte"]));
                    JMovomiento.Add("ULTIMOCOSTO", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["UltimoCosto"]));
                    JMovomiento.Add("TOTALULTIMOCOSTO", Convert.ToString(ConsultaReporteInventarioActualAlmacenProducto.Registros["TotalUltimoCosto"]));
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