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

public partial class ReporteInventarioActualGlobalUnidades : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    }

    [WebMethod]
    public static string ImprimirInventarioActualAlmacenProducto(string pTemplate, string pFechaInicial, string pFechaFinal, int pIdAlmacen, int pTipoImpresion, string pFechaIni, string pFechaF)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CImpresora Impresora = new CImpresora();

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

        JObject datos = CReportesKeep.obtenerDatosImpresionInventarioActualGlobalUnidades(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pFechaInicial, pFechaFinal, pIdAlmacen, pFechaIni, pFechaF, ConexionBaseDatos);

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "InventarioActualGlobalEnUnidades_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
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

            Modelo.Add(new JProperty("Archivo", Impresora.ReportePDFTemplateArreglos(rutaPDF, rutaTemplate, rutaCSS, imagenLogo, ImpresionTemplate.IdImpresionTemplate, datos, ConexionBaseDatos, pTipoImpresion)));
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
    public static string ObtieneInventarioGlobalUnidades(int pIdAlmacen, string pFechaInicial, string pFechaFinal, string pFechaIni, string pFechaF)
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

            CSelectEspecifico ConsultaReporteInventarioActualGlobalUnidades = new CSelectEspecifico();
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.CommandText = "SP_Impresion_InventarioGlobalUnidades";
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
            ConsultaReporteInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaFin", pFechaF);
            ConsultaReporteInventarioActualGlobalUnidades.Llena(ConexionBaseDatos);

            if (ConsultaReporteInventarioActualGlobalUnidades.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["RazonSocialReceptor"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Fecha"]));
                Modelo.Add("FECHAINICIAL", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["FechaInicial"]));
                Modelo.Add("FECHAFINAL", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["FechaFinal"]));
                Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["TipoMoneda"]));
                Modelo.Add("REPORTE", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Reporte"]));
                Modelo.Add("ALMACEN1", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Almacen"]));
                Modelo.Add("TOTALINVINICIAL", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["TotalInventarioInicial"]));
                Modelo.Add("TOTALENTRADAS", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["TotalEntradas"]));
                Modelo.Add("TOTALSALIDAS", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["TotalSalidas"]));
                Modelo.Add("TOTALEXISTENCIA", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["TotalExistencias"]));

            }

            if (ConsultaReporteInventarioActualGlobalUnidades.Registros.NextResult())
            {
                JArray JAAlmacen = new JArray();
                string Almacen = "";
                JObject JMAlmacen = new JObject();

                JArray JAMovimientos = new JArray();
                while (ConsultaReporteInventarioActualGlobalUnidades.Registros.Read())
                {

                    //Creamos list proveedor
                    if (Almacen != Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Descripcion"]))
                    {
                        if (JAMovimientos.Count > 0)
                        {
                            JMAlmacen.Add("Movimientos", JAMovimientos);
                            JAAlmacen.Add(JMAlmacen);
                            JMAlmacen = new JObject();
                            JAMovimientos = new JArray();
                        }
                        Almacen = Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Descripcion"]);
                        JMAlmacen.Add("Almacen", Almacen);
                        JObject JMovimiento = new JObject();
                        JMovimiento.Add("ALMACEN", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Descripcion"]));
                        JMovimiento.Add("CLAVE", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Clave"]));
                        JMovimiento.Add("PRODUCTO", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Producto"]));
                        JMovimiento.Add("COSTEO", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Costeo"]));
                        JMovimiento.Add("UNIDADCOMPRAVENTA", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                        JMovimiento.Add("INVENTARIOINICIAL", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["InventarioInicial"]));
                        JMovimiento.Add("ENTRADAS", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Entradas"]));
                        JMovimiento.Add("SALIDAS", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Salidas"]));
                        JMovimiento.Add("EXISTENCIA", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Existencia"]));
                        JMovimiento.Add("ROTACION", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Rotacion"]));
                        JAMovimientos.Add(JMovimiento);
                    }
                    else
                    {
                        JObject JMovimiento = new JObject();
                        JMovimiento.Add("ALMACEN", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Descripcion"]));
                        JMovimiento.Add("CLAVE", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Clave"]));
                        JMovimiento.Add("PRODUCTO", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Producto"]));
                        JMovimiento.Add("COSTEO", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Costeo"]));
                        JMovimiento.Add("UNIDADCOMPRAVENTA", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                        JMovimiento.Add("INVENTARIOINICIAL", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["InventarioInicial"]));
                        JMovimiento.Add("ENTRADAS", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Entradas"]));
                        JMovimiento.Add("SALIDAS", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Salidas"]));
                        JMovimiento.Add("EXISTENCIA", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Existencia"]));
                        JMovimiento.Add("ROTACION", Convert.ToString(ConsultaReporteInventarioActualGlobalUnidades.Registros["Rotacion"]));
                        JAMovimientos.Add(JMovimiento);
                    }
                }
                JMAlmacen.Add("Movimientos", JAMovimientos);
                JAAlmacen.Add(JMAlmacen);
                Modelo.Add("ALMACEN", JAAlmacen);
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


}