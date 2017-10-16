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

public partial class SubCuentaContable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridSubCuentaContable
        CJQGrid GridSubCuentaContable = new CJQGrid();
        GridSubCuentaContable.NombreTabla = "grdSubCuentaContable";
        GridSubCuentaContable.CampoIdentificador = "IdSubCuentaContable";
        GridSubCuentaContable.ColumnaOrdenacion = "SubCuentaContable";
        GridSubCuentaContable.Metodo = "ObtenerSubCuentaContable";
        GridSubCuentaContable.TituloTabla = "Catálogo de subcuentas contables";

        //IdSubCuentaContable
        CJQColumn ColIdSubCuentaContable = new CJQColumn();
        ColIdSubCuentaContable.Nombre = "IdSubCuentaContable";
        ColIdSubCuentaContable.Oculto = "true";
        ColIdSubCuentaContable.Encabezado = "IdSubCuentaContable";
        ColIdSubCuentaContable.Buscador = "false";
        GridSubCuentaContable.Columnas.Add(ColIdSubCuentaContable);

        //SubCuentaContable
        CJQColumn ColSubCuentaContable = new CJQColumn();
        ColSubCuentaContable.Nombre = "SubCuentaContable";
        ColSubCuentaContable.Encabezado = "Subcuenta contable";
        ColSubCuentaContable.Ancho = "100";
        ColSubCuentaContable.Alineacion = "left";
        GridSubCuentaContable.Columnas.Add(ColSubCuentaContable);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Ancho = "170";
        ColDescripcion.Buscador = "false";
        GridSubCuentaContable.Columnas.Add(ColDescripcion);

        //CuentaContable
        CJQColumn ColCuentaContable = new CJQColumn();
        ColCuentaContable.Nombre = "CuentaContable";
        ColCuentaContable.Encabezado = "CuentaContable";
        ColCuentaContable.Ancho = "100";
        ColCuentaContable.Buscador = "false";
        GridSubCuentaContable.Columnas.Add(ColCuentaContable);

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
        GridSubCuentaContable.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarSubCuentaContable";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridSubCuentaContable.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSubCuentaContable", GridSubCuentaContable.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSubCuentaContable(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSubCuentaContable, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSubCuentaContable", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSubCuentaContable", SqlDbType.VarChar, 250).Value = Convert.ToString(pSubCuentaContable);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarSubCuentaContable(string pSubCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonSubCuentaContable = new CJson();
        jsonSubCuentaContable.StoredProcedure.CommandText = "sp_SubCuentaContable_Consultar_FiltroPorSubCuentaContable";
        jsonSubCuentaContable.StoredProcedure.Parameters.AddWithValue("@pSubCuentaContable", pSubCuentaContable);
        return jsonSubCuentaContable.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarSubCuentaContable(Dictionary<string, object> pSubCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CSubCuentaContable SubCuentaContable = new CSubCuentaContable();
            SubCuentaContable.SubCuentaContable = Convert.ToString(pSubCuentaContable["SubCuentaContable"]);
            SubCuentaContable.Descripcion = Convert.ToString(pSubCuentaContable["Descripcion"]);
            SubCuentaContable.IdCuentaContable = Convert.ToInt32(pSubCuentaContable["IdCuentaContable"]);
            SubCuentaContable.CuentaContable = Convert.ToString(pSubCuentaContable["CuentaContable"]);
            string validacion = ValidarSubCuentaContable(SubCuentaContable, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                SubCuentaContable.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = SubCuentaContable.IdSubCuentaContable;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una subcuenta contable";
                HistorialGenerico.AgregarHistorialGenerico("SubCuentaContable", ConexionBaseDatos);

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
    public static string ObtenerFormaSubCuentaContable(int pIdSubCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSubCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSubCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarSubCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarSubCuentaContable", puedeEditarSubCuentaContable);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CSubCuentaContable.ObtenerSubCuentaContable(Modelo, pIdSubCuentaContable, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarSubCuentaContable(int IdSubCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSubCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSubCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarSubCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarSubCuentaContable", puedeEditarSubCuentaContable);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CSubCuentaContable.ObtenerSubCuentaContable(Modelo, IdSubCuentaContable, ConexionBaseDatos);
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
    public static string EditarSubCuentaContable(Dictionary<string, object> pSubCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CSubCuentaContable SubCuentaContable = new CSubCuentaContable();
        SubCuentaContable.LlenaObjeto(Convert.ToInt32(pSubCuentaContable["IdSubCuentaContable"]), ConexionBaseDatos);
        SubCuentaContable.IdSubCuentaContable = Convert.ToInt32(pSubCuentaContable["IdSubCuentaContable"]);
        SubCuentaContable.SubCuentaContable = Convert.ToString(pSubCuentaContable["SubCuentaContable"]);
        SubCuentaContable.Descripcion = Convert.ToString(pSubCuentaContable["Descripcion"]);
        SubCuentaContable.IdCuentaContable = Convert.ToInt32(pSubCuentaContable["IdCuentaContable"]);
        SubCuentaContable.CuentaContable = Convert.ToString(pSubCuentaContable["CuentaContable"]);
        string validacion = ValidarSubCuentaContable(SubCuentaContable, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            SubCuentaContable.Editar(ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = SubCuentaContable.IdSubCuentaContable;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se edito una subcuenta contable";
            HistorialGenerico.AgregarHistorialGenerico("SubCuentaContable", ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdSubCuentaContable, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSubCuentaContable SubCuentaContable = new CSubCuentaContable();
            SubCuentaContable.IdSubCuentaContable = pIdSubCuentaContable;
            SubCuentaContable.Baja = pBaja;
            SubCuentaContable.Eliminar(ConexionBaseDatos);
            respuesta = "0|SubCuentaContableEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string BuscarCuentaContable(string pCuentaContable)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCuentaContable jsonCuentaContable = new CCuentaContable();
        jsonCuentaContable.StoredProcedure.CommandText = "sp_Proveedor_ConsultarCuentaContable";
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", pCuentaContable);
        return jsonCuentaContable.ObtenerJsonCuentaContable(ConexionBaseDatos);

    }

    //Validaciones
    private static string ValidarSubCuentaContable(CSubCuentaContable pSubCuentaContable, CConexion pConexion)
    {
        string errores = "";
        if (pSubCuentaContable.SubCuentaContable == "")
        { errores = errores + "<span>*</span> El campo subcuenta contable esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pSubCuentaContable.IdCuentaContable == 0)
        { errores = errores + "<span>*</span> La cuenta contable esta vacia, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}