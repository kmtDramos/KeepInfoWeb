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

public partial class UnidadCompraVenta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridUnidadCompraVenta
        CJQGrid GridUnidadCompraVenta = new CJQGrid();
        GridUnidadCompraVenta.NombreTabla = "grdUnidadCompraVenta";
        GridUnidadCompraVenta.CampoIdentificador = "IdUnidadCompraVenta";
        GridUnidadCompraVenta.ColumnaOrdenacion = "UnidadCompraVenta";
        GridUnidadCompraVenta.Metodo = "ObtenerUnidadCompraVenta";
        GridUnidadCompraVenta.TituloTabla = "Catálogo de unidad de compra venta";

        //IdUnidadCompraVenta
        CJQColumn ColIdUnidadCompraVenta = new CJQColumn();
        ColIdUnidadCompraVenta.Nombre = "IdUnidadCompraVenta";
        ColIdUnidadCompraVenta.Oculto = "true";
        ColIdUnidadCompraVenta.Encabezado = "IdUnidadCompraVenta";
        ColIdUnidadCompraVenta.Buscador = "false";
        GridUnidadCompraVenta.Columnas.Add(ColIdUnidadCompraVenta);

        //UnidadCompraVenta
        CJQColumn ColUnidadCompraVenta = new CJQColumn();
        ColUnidadCompraVenta.Nombre = "UnidadCompraVenta";
        ColUnidadCompraVenta.Encabezado = "Unidad de compra venta";
        ColUnidadCompraVenta.Ancho = "400";
        ColUnidadCompraVenta.Alineacion = "left";
        GridUnidadCompraVenta.Columnas.Add(ColUnidadCompraVenta);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridUnidadCompraVenta.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarUnidadCompraVenta";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridUnidadCompraVenta.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdUnidadCompraVenta", GridUnidadCompraVenta.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerUnidadCompraVenta(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pUnidadCompraVenta, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdUnidadCompraVenta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pUnidadCompraVenta", SqlDbType.VarChar, 250).Value = Convert.ToString(pUnidadCompraVenta);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarUnidadCompraVenta(string pUnidadCompraVenta)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonUnidadCompraVenta = new CJson();
        jsonUnidadCompraVenta.StoredProcedure.CommandText = "sp_UnidadCompraVenta_Consultar_FiltroPorUnidadCompraVenta";
        jsonUnidadCompraVenta.StoredProcedure.Parameters.AddWithValue("@pUnidadCompraVenta", pUnidadCompraVenta);
        return jsonUnidadCompraVenta.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarUnidadCompraVenta(Dictionary<string, object> pUnidadCompraVenta)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            UnidadCompraVenta.UnidadCompraVenta = Convert.ToString(pUnidadCompraVenta["UnidadCompraVenta"]);

            string validacion = ValidarUnidadCompraVenta(UnidadCompraVenta, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                UnidadCompraVenta.Agregar(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFormaUnidadCompraVenta(int pIdUnidadCompraVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarUnidadCompraVenta = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarUnidadCompraVenta" }, ConexionBaseDatos) == "")
        {
            puedeEditarUnidadCompraVenta = 1;
        }
        oPermisos.Add("puedeEditarUnidadCompraVenta", puedeEditarUnidadCompraVenta);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            UnidadCompraVenta.LlenaObjeto(pIdUnidadCompraVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdUnidadCompraVenta", UnidadCompraVenta.IdUnidadCompraVenta));
            Modelo.Add(new JProperty("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarUnidadCompraVenta(int IdUnidadCompraVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarUnidadCompraVenta = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarUnidadCompraVenta" }, ConexionBaseDatos) == "")
        {
            puedeEditarUnidadCompraVenta = 1;
        }
        oPermisos.Add("puedeEditarUnidadCompraVenta", puedeEditarUnidadCompraVenta);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            UnidadCompraVenta.LlenaObjeto(IdUnidadCompraVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdUnidadCompraVenta", UnidadCompraVenta.IdUnidadCompraVenta));
            Modelo.Add(new JProperty("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarUnidadCompraVenta(Dictionary<string, object> pUnidadCompraVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        UnidadCompraVenta.IdUnidadCompraVenta = Convert.ToInt32(pUnidadCompraVenta["IdUnidadCompraVenta"]); ;
        UnidadCompraVenta.UnidadCompraVenta = Convert.ToString(pUnidadCompraVenta["UnidadCompraVenta"]);

        string validacion = ValidarUnidadCompraVenta(UnidadCompraVenta, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            UnidadCompraVenta.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdUnidadCompraVenta, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            UnidadCompraVenta.IdUnidadCompraVenta = pIdUnidadCompraVenta;
            UnidadCompraVenta.Baja = pBaja;
            UnidadCompraVenta.Eliminar(ConexionBaseDatos);
            respuesta = "0|UnidadCompraVentaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarUnidadCompraVenta(CUnidadCompraVenta pUnidadCompraVenta, CConexion pConexion)
    {
        string errores = "";
        if (pUnidadCompraVenta.UnidadCompraVenta == "")
        { errores = errores + "<span>*</span> El campo unidad de compra venta esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}