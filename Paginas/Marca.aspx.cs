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

public partial class Marca : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    private static int idEmpresa;
    public static int puedeAgregarMarca = 0;
    public static int puedeEditarMarca = 0;
    public static int puedeEliminarMarca = 0;
    public static int puedeConsultarMarca = 0;

    protected void Page_Load(object sender, EventArgs e)
    {


        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;
        idSucursal = Sucursal.IdSucursal;
        idEmpresa = Empresa.IdEmpresa;

        if (Usuario.TienePermisos(new string[] { "puedeAgregarMarca" }, ConexionBaseDatos) == "") { puedeAgregarMarca = 1; } else { puedeAgregarMarca = 0; }
        if (Usuario.TienePermisos(new string[] { "puedeConsultarMarca" }, ConexionBaseDatos) == "") { puedeConsultarMarca = 1; } else { puedeConsultarMarca = 0; }
        if (Usuario.TienePermisos(new string[] { "puedeEditarMarca" }, ConexionBaseDatos) == "") { puedeEditarMarca = 1; } else { puedeEditarMarca = 0; }
        if (Usuario.TienePermisos(new string[] { "puedeEliminarMarca" }, ConexionBaseDatos) == "") { puedeEliminarMarca = 1; } else { puedeEliminarMarca = 0; }

        //GridMarca
        CJQGrid GridMarca = new CJQGrid();
        GridMarca.NombreTabla = "grdMarca";
        GridMarca.CampoIdentificador = "IdMarca";
        GridMarca.ColumnaOrdenacion = "Marca";
        GridMarca.Metodo = "ObtenerMarca";
        GridMarca.TituloTabla = "Catálogo de marcas";

        //IdMarca
        CJQColumn ColIdMarca = new CJQColumn();
        ColIdMarca.Nombre = "IdMarca";
        ColIdMarca.Oculto = "true";
        ColIdMarca.Encabezado = "IdMarca";
        ColIdMarca.Buscador = "false";
        GridMarca.Columnas.Add(ColIdMarca);

        //Marca
        CJQColumn ColMarca = new CJQColumn();
        ColMarca.Nombre = "Marca";
        ColMarca.Encabezado = "Marca";
        ColMarca.Ancho = "570";
        ColMarca.Alineacion = "Left";
        GridMarca.Columnas.Add(ColMarca);

        //Cuota Compra
        CJQColumn ColCuotaCompra = new CJQColumn();
        ColCuotaCompra.Nombre = "CuotaCompra";
        ColCuotaCompra.Encabezado = "Cuota de compra";
        ColCuotaCompra.Ancho = "180";
        ColCuotaCompra.Buscador = "false";
        ColCuotaCompra.Formato = "FormatoMoneda";
        ColCuotaCompra.Alineacion = "right";
        GridMarca.Columnas.Add(ColCuotaCompra);

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
        GridMarca.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarMarca";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridMarca.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMarca", GridMarca.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarMarca" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMarca(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pMarca, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdMarca", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pMarca", SqlDbType.VarChar, 250).Value = Convert.ToString(pMarca);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarMarca(string pMarca)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonMarca = new CJson();
        jsonMarca.StoredProcedure.CommandText = "sp_Marca_Consultar_FiltroPorMarca";
        jsonMarca.StoredProcedure.Parameters.AddWithValue("@pMarca", pMarca);
        return jsonMarca.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarMarca(Dictionary<string, object> pMarca)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CMarca Marca = new CMarca();
            Marca.Marca = Convert.ToString(pMarca["Marca"]);
            Marca.CuotaCompra = Convert.ToDecimal(pMarca["CuotaCompra"].ToString());

            string validacion = ValidarMarca(Marca, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Marca.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaMarca(int pIdMarca)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeConsultarMarca", puedeConsultarMarca);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CMarca Marca = new CMarca();
            Marca.LlenaObjeto(pIdMarca, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdMarca", Marca.IdMarca));
            Modelo.Add(new JProperty("Marca", Marca.Marca));
            Modelo.Add(new JProperty("CuotaCompra", Marca.CuotaCompra));

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
    public static string ObtenerFormaEditarMarca(int IdMarca)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeEditarMarca", puedeEditarMarca);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CMarca Marca = new CMarca();
            Marca.LlenaObjeto(IdMarca, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdMarca", Marca.IdMarca));
            Modelo.Add(new JProperty("Marca", Marca.Marca));
            Modelo.Add(new JProperty("CuotaCompra", Marca.CuotaCompra));
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
    public static string EditarMarca(Dictionary<string, object> pMarca)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CMarca Marca = new CMarca();
        Marca.IdMarca = Convert.ToInt32(pMarca["IdMarca"]); ;
        Marca.Marca = Convert.ToString(pMarca["Marca"]);
        Marca.CuotaCompra = Convert.ToDecimal(pMarca["CuotaCompra"]);

        string validacion = ValidarMarca(Marca, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Marca.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdMarca, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CMarca Marca = new CMarca();
            Marca.IdMarca = pIdMarca;
            Marca.Baja = pBaja;
            Marca.Eliminar(ConexionBaseDatos);
            respuesta = "0|MarcaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarMarca(CMarca pMarca, CConexion pConexion)
    {
        string errores = "";
        if (pMarca.Marca == "")
        { errores = errores + "<span>*</span> El campo marca esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}