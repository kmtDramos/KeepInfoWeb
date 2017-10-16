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

public partial class Pais : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //GridPais
        CJQGrid GridPais = new CJQGrid();
        GridPais.NombreTabla = "grdPais";
        GridPais.CampoIdentificador = "IdPais";
        GridPais.ColumnaOrdenacion = "Pais";
        GridPais.Metodo = "ObtenerPais";
        GridPais.TituloTabla = "Catálogo de paises";

        //IdPais
        CJQColumn ColIdPais = new CJQColumn();
        ColIdPais.Nombre = "IdPais";
        ColIdPais.Encabezado = "IdPais";
        ColIdPais.Oculto = "true";
        ColIdPais.Buscador = "false";
        GridPais.Columnas.Add(ColIdPais);

        //Pais
        CJQColumn ColPais = new CJQColumn();
        ColPais.Nombre = "Pais";
        ColPais.Encabezado = "Pais";
        ColPais.Buscador = "true";
        ColPais.Alineacion = "left";
        ColPais.Ancho = "150";
        GridPais.Columnas.Add(ColPais);

        //Nacionalidad
        CJQColumn ColNacionalidad = new CJQColumn();
        ColNacionalidad.Nombre = "Nacionalidad";
        ColNacionalidad.Encabezado = "Nacionalidad";
        ColNacionalidad.Buscador = "false";
        ColNacionalidad.Alineacion = "left";
        ColNacionalidad.Ancho = "150";
        GridPais.Columnas.Add(ColNacionalidad);

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
        GridPais.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarPais";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridPais.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPais", GridPais.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerPais(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pPais, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdPais", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pPais", SqlDbType.VarChar, 250).Value = Convert.ToString(pPais);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarPais()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

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

    [WebMethod]//en este metodo se obtiene los datos del servicio, y valores de los combos
    public static string ObtenerFormaPais(int pIdPais)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarPais = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CPais Pais = new CPais();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarPais" }, ConexionBaseDatos) == "")
        {
            puedeEditarPais = 1;
        }
        oPermisos.Add("puedeEditarPais", puedeEditarPais);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Pais.LlenaObjeto(pIdPais, ConexionBaseDatos);
            Modelo.Add("IdPais", Pais.IdPais);
            Modelo.Add("Pais", Pais.Pais);
            Modelo.Add("Nacionalidad", Pais.Nacionalidad);

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

    [WebMethod]// en este metodo obtien los datos para la pantalla de consulta
    public static string ObtenerFormaEditarPais(int IdPais)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarPais = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CPais Pais = new CPais();
        if (Usuario.TienePermisos(new string[] { "puedeEditarPais" }, ConexionBaseDatos) == "")
        {
            puedeEditarPais = 1;
        }
        oPermisos.Add("puedeEditarPais", puedeEditarPais);

        if (respuesta == "Conexion Establecida")
        {
            Pais.LlenaObjeto(IdPais, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("IdPais", Pais.IdPais));
            Modelo.Add(new JProperty("Pais", Pais.Pais));
            Modelo.Add(new JProperty("Nacionalidad", Pais.Nacionalidad));

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
    public static string AgregarPais(Dictionary<string, object> pPais)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            CPais Pais = new CPais();
            Pais.Pais = pPais["Pais"].ToString();
            Pais.Nacionalidad = pPais["Nacionalidad"].ToString();

            string validacion = ValidarPais(Pais);
            if (validacion == "")
            {
                Pais.Agregar(ConexionBaseDatos);
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

    [WebMethod]//metodo para la edicion en la BD
    public static string EditarPais(Dictionary<string, object> pPais)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CPais Pais = new CPais();
        Pais.LlenaObjeto(Convert.ToInt32(pPais["IdPais"]), ConexionBaseDatos);
        Pais.Pais = pPais["Pais"].ToString();
        Pais.Nacionalidad = pPais["Nacionalidad"].ToString();


        string validacion = ValidarPais(Pais);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Pais.Editar(ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdPais, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPais Pais = new CPais();
            Pais.IdPais = pIdPais;
            Pais.Baja = pBaja;
            Pais.Eliminar(ConexionBaseDatos);
            respuesta = "0|PaisEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarPais(CPais pPais)
    {
        string errores = "";
        if (pPais.Pais == "")
        { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pPais.Nacionalidad == "")
        { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}