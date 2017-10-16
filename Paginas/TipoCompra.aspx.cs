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

public partial class TipoCompra : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridTipoCompra
        CJQGrid GridTipoCompra = new CJQGrid();
        GridTipoCompra.NombreTabla = "grdTipoCompra";
        GridTipoCompra.CampoIdentificador = "IdTipoCompra";
        GridTipoCompra.ColumnaOrdenacion = "TipoCompra";
        GridTipoCompra.Metodo = "ObtenerTipoCompra";
        GridTipoCompra.TituloTabla = "Catálogo de tipos de compra";

        //IdTipoCompra
        CJQColumn ColIdTipoCompra = new CJQColumn();
        ColIdTipoCompra.Nombre = "IdTipoCompra";
        ColIdTipoCompra.Oculto = "true";
        ColIdTipoCompra.Encabezado = "IdTipoCompra";
        ColIdTipoCompra.Buscador = "false";
        GridTipoCompra.Columnas.Add(ColIdTipoCompra);

        //TipoCompra
        CJQColumn ColTipoCompra = new CJQColumn();
        ColTipoCompra.Nombre = "TipoCompra";
        ColTipoCompra.Encabezado = "Tipo de compra";
        ColTipoCompra.Ancho = "200";
        ColTipoCompra.Alineacion = "left";
        GridTipoCompra.Columnas.Add(ColTipoCompra);

        //ClaveCuentaContable
        CJQColumn ColClaveCuentaContable = new CJQColumn();
        ColClaveCuentaContable.Nombre = "ClaveCuentaContable";
        ColClaveCuentaContable.Encabezado = "Clave de cuenta contable";
        ColClaveCuentaContable.Ancho = "200";
        ColClaveCuentaContable.Alineacion = "left";
        ColClaveCuentaContable.Buscador = "false";
        GridTipoCompra.Columnas.Add(ColClaveCuentaContable);

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
        GridTipoCompra.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoCompra";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoCompra.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoCompra", GridTipoCompra.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoCompra(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoCompra, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoCompra", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoCompra", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoCompra);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoCompra(string pTipoCompra)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonTipoCompra = new CJson();
        jsonTipoCompra.StoredProcedure.CommandText = "sp_TipoCompra_Consultar_FiltroPorTipoCompra";
        jsonTipoCompra.StoredProcedure.Parameters.AddWithValue("@pTipoCompra", pTipoCompra);
        return jsonTipoCompra.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoCompra(Dictionary<string, object> pTipoCompra)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoCompra TipoCompra = new CTipoCompra();
            TipoCompra.TipoCompra = Convert.ToString(pTipoCompra["TipoCompra"]);
            TipoCompra.ClaveCuentaContable = Convert.ToString(pTipoCompra["ClaveCuentaContable"]);
            string validacion = ValidarTipoCompra(TipoCompra, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoCompra.Agregar(ConexionBaseDatos);
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
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFormaTipoCompra(int pIdTipoCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCompra = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCompra" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCompra = 1;
        }
        oPermisos.Add("puedeEditarTipoCompra", puedeEditarTipoCompra);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CTipoCompra.ObtenerTipoCompra(Modelo, pIdTipoCompra, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarTipoCompra(int IdTipoCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCompra = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCompra" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCompra = 1;
        }
        oPermisos.Add("puedeEditarTipoCompra", puedeEditarTipoCompra);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CTipoCompra.ObtenerTipoCompra(Modelo, IdTipoCompra, ConexionBaseDatos);
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
    public static string EditarTipoCompra(Dictionary<string, object> pTipoCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoCompra TipoCompra = new CTipoCompra();
        TipoCompra.IdTipoCompra = Convert.ToInt32(pTipoCompra["IdTipoCompra"]); ;
        TipoCompra.TipoCompra = Convert.ToString(pTipoCompra["TipoCompra"]);
        TipoCompra.ClaveCuentaContable = Convert.ToString(pTipoCompra["ClaveCuentaContable"]);

        string validacion = ValidarTipoCompra(TipoCompra, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoCompra.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdTipoCompra, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCompra TipoCompra = new CTipoCompra();
            TipoCompra.IdTipoCompra = pIdTipoCompra;
            TipoCompra.Baja = pBaja;
            TipoCompra.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoCompraEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoCompra(CTipoCompra pTipoCompra, CConexion pConexion)
    {
        string errores = "";
        if (pTipoCompra.TipoCompra == "")
        { errores = errores + "<span>*</span> El campo tipo de compra esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pTipoCompra.ClaveCuentaContable == "")
        { errores = errores + "<span>*</span> El campo de la clave de cuenta contable esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pTipoCompra.IdTipoCompra == 0)
        {
            if (pTipoCompra.ExisteClaveCuentaContable(pTipoCompra.ClaveCuentaContable, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pTipoCompra.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }
        else
        {
            if (pTipoCompra.ExisteClaveCuentaContableEditar(pTipoCompra.ClaveCuentaContable, pTipoCompra.IdTipoCompra, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pTipoCompra.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}