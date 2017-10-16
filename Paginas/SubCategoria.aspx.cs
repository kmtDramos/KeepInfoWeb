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

public partial class SubCategoria : System.Web.UI.Page
{
    private static int idUsuario;
    public static int puedeAgregarSubCategoria = 0;
    public static int puedeConsultarSubCategoria = 0;
    public static int puedeEditarSubCategoria = 0;
    public static int puedeEliminarSubCategoria = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;

        if (Usuario.TienePermisos(new string[] { "puedeAgregarSubCategoria" }, ConexionBaseDatos) == "") { puedeAgregarSubCategoria = 1; }
        else { puedeAgregarSubCategoria = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeConsultarSubCategoria" }, ConexionBaseDatos) == "") { puedeConsultarSubCategoria = 1; }
        else { puedeConsultarSubCategoria = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarSubCategoria" }, ConexionBaseDatos) == "") { puedeEditarSubCategoria = 1; }
        else { puedeEditarSubCategoria = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEliminarSubCategoria" }, ConexionBaseDatos) == "") { puedeEliminarSubCategoria = 1; }
        else { puedeEliminarSubCategoria = 0; }


        //GridSubCategoria
        CJQGrid GridSubCategoria = new CJQGrid();
        GridSubCategoria.NombreTabla = "grdSubCategoria";
        GridSubCategoria.CampoIdentificador = "IdSubCategoria";
        GridSubCategoria.ColumnaOrdenacion = "SubCategoria";
        GridSubCategoria.Metodo = "ObtenerSubCategoria";
        GridSubCategoria.TituloTabla = "Catálogo de subgrupo";

        //IdSubCategoria
        CJQColumn ColIdSubCategoria = new CJQColumn();
        ColIdSubCategoria.Nombre = "IdSubCategoria";
        ColIdSubCategoria.Oculto = "true";
        ColIdSubCategoria.Encabezado = "IdSubCategoria";
        ColIdSubCategoria.Buscador = "false";
        GridSubCategoria.Columnas.Add(ColIdSubCategoria);

        //Categoria
        CJQColumn ColCategoria = new CJQColumn();
        ColCategoria.Nombre = "Categoria";
        ColCategoria.Encabezado = "Categoría";
        ColCategoria.Ancho = "400";
        ColCategoria.Alineacion = "Left";
        GridSubCategoria.Columnas.Add(ColCategoria);

        //SubCategoria
        CJQColumn ColSubCategoria = new CJQColumn();
        ColSubCategoria.Nombre = "SubCategoria";
        ColSubCategoria.Encabezado = "SubGrupo";
        ColSubCategoria.Ancho = "400";
        ColSubCategoria.Alineacion = "Left";
        GridSubCategoria.Columnas.Add(ColSubCategoria);

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
        ColBaja.Oculto = puedeEliminarSubCategoria == 1 ? "false" : "true";
        GridSubCategoria.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarSubCategoria";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        ColConsultar.Oculto = puedeConsultarSubCategoria == 1 ? "false" : "true";
        GridSubCategoria.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSubCategoria", GridSubCategoria.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSubCategoria(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSubCategoria, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSubCategoria", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSubCategoria", SqlDbType.VarChar, 250).Value = Convert.ToString(pSubCategoria);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    //[WebMethod]
    //public static string BuscarSubCategoria(string pSubCategoria)
    //{
    //    //Abrir Conexion
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //    CJson jsonSubCategoria = new CJson();
    //    jsonSubCategoria.StoredProcedure.CommandText = "spb_SubCategoria_ConsultarFiltros";
    //    jsonSubCategoria.StoredProcedure.Parameters.AddWithValue("@pSubCategoria", pSubCategoria);
    //    return jsonSubCategoria.ObtenerJsonString(ConexionBaseDatos);
    //}

    [WebMethod]
    public static string AgregarSubCategoria(Dictionary<string, object> pSubCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {

            CSubCategoria SubCategoria = new CSubCategoria();
            SubCategoria.SubCategoria = Convert.ToString(pSubCategoria["SubCategoria"]);
            SubCategoria.IdCategoria = Convert.ToInt32(pSubCategoria["IdCategoria"]);

            string validacion = ValidarSubCategoria(SubCategoria, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                SubCategoria.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarSubCategoria()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        oPermisos.Add("puedeAgregarSubCategoria", puedeAgregarSubCategoria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSubCategoria SubCategoria = new CSubCategoria();
            Modelo.Add("Categorias", CCategoria.ObtenerJsonCategorias(ConexionBaseDatos));

            Modelo.Add(new JProperty("IdSubCategoria", SubCategoria.IdSubCategoria));
            Modelo.Add(new JProperty("SubCategoria", SubCategoria.SubCategoria));

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
    public static string ObtenerFormaConsultarSubCategoria(int pIdSubCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        oPermisos.Add("puedeConsultarSubCategoria", puedeConsultarSubCategoria);
        oPermisos.Add("puedeEditarSubCategoria", puedeEditarSubCategoria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSubCategoria SubCategoria = new CSubCategoria();
            SubCategoria.LlenaObjeto(pIdSubCategoria, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSubCategoria", SubCategoria.IdSubCategoria));
            Modelo.Add(new JProperty("SubCategoria", SubCategoria.SubCategoria));

            CCategoria Categoria = new CCategoria();
            Categoria.LlenaObjeto(SubCategoria.IdCategoria, ConexionBaseDatos);
            Modelo.Add(new JProperty("Categoria", Categoria.Categoria));

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
    public static string ObtenerFormaEditarSubCategoria(int IdSubCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        oPermisos.Add("puedeEditarSubCategoria", puedeEditarSubCategoria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSubCategoria SubCategoria = new CSubCategoria();
            SubCategoria.LlenaObjeto(IdSubCategoria, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSubCategoria", SubCategoria.IdSubCategoria));
            Modelo.Add(new JProperty("SubCategoria", SubCategoria.SubCategoria));
            Modelo.Add("Categorias", CCategoria.ObtenerJsonCategorias(SubCategoria.IdCategoria, ConexionBaseDatos));

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
    public static string EditarSubCategoria(Dictionary<string, object> pSubCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CSubCategoria SubCategoria = new CSubCategoria();
        SubCategoria.IdSubCategoria = Convert.ToInt32(pSubCategoria["IdSubCategoria"]); ;
        SubCategoria.SubCategoria = Convert.ToString(pSubCategoria["SubCategoria"]);
        SubCategoria.IdCategoria = Convert.ToInt32(pSubCategoria["IdCategoria"]);

        string validacion = ValidarSubCategoria(SubCategoria, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            SubCategoria.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdSubCategoria, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            CSubCategoria SubCategoria = new CSubCategoria();
            SubCategoria.IdSubCategoria = pIdSubCategoria;
            SubCategoria.Baja = pBaja;
            SubCategoria.Eliminar(ConexionBaseDatos);
            respuesta = "0|SubCategoriaEliminado";
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarSubCategoria(CSubCategoria pSubCategoria, CConexion pConexion)
    {
        string errores = "";
        if (pSubCategoria.SubCategoria == "")
        { errores = errores + "<span>*</span> El campo SubCategoria esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}