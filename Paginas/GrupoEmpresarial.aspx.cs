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

public partial class GrupoEmpresarial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridGrupoEmpresarial
        CJQGrid GridGrupoEmpresarial = new CJQGrid();
        GridGrupoEmpresarial.NombreTabla = "grdGrupoEmpresarial";
        GridGrupoEmpresarial.CampoIdentificador = "IdGrupoEmpresarial";
        GridGrupoEmpresarial.ColumnaOrdenacion = "GrupoEmpresarial";
        GridGrupoEmpresarial.Metodo = "ObtenerGrupoEmpresarial";
        GridGrupoEmpresarial.TituloTabla = "Catálogo de grupos empresariales";

        //IdGrupoEmpresarial
        CJQColumn ColIdGrupoEmpresarial = new CJQColumn();
        ColIdGrupoEmpresarial.Nombre = "IdGrupoEmpresarial";
        ColIdGrupoEmpresarial.Oculto = "true";
        ColIdGrupoEmpresarial.Encabezado = "IdGrupoEmpresarial";
        ColIdGrupoEmpresarial.Buscador = "false";
        GridGrupoEmpresarial.Columnas.Add(ColIdGrupoEmpresarial);

        //GrupoEmpresarial
        CJQColumn ColGrupoEmpresarial = new CJQColumn();
        ColGrupoEmpresarial.Nombre = "GrupoEmpresarial";
        ColGrupoEmpresarial.Encabezado = "Grupo empresarial";
        ColGrupoEmpresarial.Ancho = "200";
        ColGrupoEmpresarial.Alineacion = "left";
        GridGrupoEmpresarial.Columnas.Add(ColGrupoEmpresarial);

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
        GridGrupoEmpresarial.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarGrupoEmpresarial";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridGrupoEmpresarial.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdGrupoEmpresarial", GridGrupoEmpresarial.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerGrupoEmpresarial(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pGrupoEmpresarial, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdGrupoEmpresarial", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pGrupoEmpresarial", SqlDbType.VarChar, 250).Value = Convert.ToString(pGrupoEmpresarial);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarGrupoEmpresarial(string pGrupoEmpresarial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonGrupoEmpresarial = new CJson();
        jsonGrupoEmpresarial.StoredProcedure.CommandText = "sp_GrupoEmpresarial_Consultar_FiltroPorGrupoEmpresarial";
        jsonGrupoEmpresarial.StoredProcedure.Parameters.AddWithValue("@pGrupoEmpresarial", pGrupoEmpresarial);
        return jsonGrupoEmpresarial.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarGrupoEmpresarial(Dictionary<string, object> pGrupoEmpresarial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
            GrupoEmpresarial.GrupoEmpresarial = Convert.ToString(pGrupoEmpresarial["GrupoEmpresarial"]);
            string validacion = ValidarGrupoEmpresarial(GrupoEmpresarial, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                GrupoEmpresarial.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaGrupoEmpresarial(int pIdGrupoEmpresarial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarGrupoEmpresarial = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarGrupoEmpresarial" }, ConexionBaseDatos) == "")
        {
            puedeEditarGrupoEmpresarial = 1;
        }
        oPermisos.Add("puedeEditarGrupoEmpresarial", puedeEditarGrupoEmpresarial);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CGrupoEmpresarial.ObtenerGrupoEmpresarial(Modelo, pIdGrupoEmpresarial, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarGrupoEmpresarial(int IdGrupoEmpresarial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarGrupoEmpresarial = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarGrupoEmpresarial" }, ConexionBaseDatos) == "")
        {
            puedeEditarGrupoEmpresarial = 1;
        }
        oPermisos.Add("puedeEditarGrupoEmpresarial", puedeEditarGrupoEmpresarial);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CGrupoEmpresarial.ObtenerGrupoEmpresarial(Modelo, IdGrupoEmpresarial, ConexionBaseDatos);
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
    public static string EditarGrupoEmpresarial(Dictionary<string, object> pGrupoEmpresarial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        GrupoEmpresarial.IdGrupoEmpresarial = Convert.ToInt32(pGrupoEmpresarial["IdGrupoEmpresarial"]); ;
        GrupoEmpresarial.GrupoEmpresarial = Convert.ToString(pGrupoEmpresarial["GrupoEmpresarial"]);

        string validacion = ValidarGrupoEmpresarial(GrupoEmpresarial, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            GrupoEmpresarial.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdGrupoEmpresarial, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
            GrupoEmpresarial.IdGrupoEmpresarial = pIdGrupoEmpresarial;
            GrupoEmpresarial.Baja = pBaja;
            GrupoEmpresarial.Eliminar(ConexionBaseDatos);
            respuesta = "0|GrupoEmpresarialEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarGrupoEmpresarial(CGrupoEmpresarial pGrupoEmpresarial, CConexion pConexion)
    {
        string errores = "";
        if (pGrupoEmpresarial.GrupoEmpresarial == "")
        { errores = errores + "<span>*</span> El campo división esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}