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

public partial class ImpresionDocumento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridImpresionDocumento
        CJQGrid GridImpresionDocumento = new CJQGrid();
        GridImpresionDocumento.NombreTabla = "grdImpresionDocumento";
        GridImpresionDocumento.CampoIdentificador = "IdImpresionDocumento";
        GridImpresionDocumento.ColumnaOrdenacion = "ImpresionDocumento";
        GridImpresionDocumento.Metodo = "ObtenerImpresionDocumento";
        GridImpresionDocumento.TituloTabla = "Catálogo de documentos de impresión";

        //IdImpresionDocumento
        CJQColumn ColIdImpresionDocumento = new CJQColumn();
        ColIdImpresionDocumento.Nombre = "IdImpresionDocumento";
        ColIdImpresionDocumento.Oculto = "true";
        ColIdImpresionDocumento.Encabezado = "IdImpresionDocumento";
        ColIdImpresionDocumento.Buscador = "false";
        GridImpresionDocumento.Columnas.Add(ColIdImpresionDocumento);

        //ImpresionDocumento
        CJQColumn ColImpresionDocumento = new CJQColumn();
        ColImpresionDocumento.Nombre = "ImpresionDocumento";
        ColImpresionDocumento.Encabezado = "ImpresionDocumento";
        ColImpresionDocumento.Ancho = "200";
        ColImpresionDocumento.Alineacion = "left";
        GridImpresionDocumento.Columnas.Add(ColImpresionDocumento);

        //ClaveCuentaContable
        CJQColumn ColClaveCuentaContable = new CJQColumn();
        ColClaveCuentaContable.Nombre = "Procedimiento";
        ColClaveCuentaContable.Encabezado = "Procedimiento";
        ColClaveCuentaContable.Ancho = "200";
        ColClaveCuentaContable.Alineacion = "left";
        ColClaveCuentaContable.Buscador = "false";
        GridImpresionDocumento.Columnas.Add(ColClaveCuentaContable);

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
        GridImpresionDocumento.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarImpresionDocumento";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridImpresionDocumento.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdImpresionDocumento", GridImpresionDocumento.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerImpresionDocumento(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pImpresionDocumento, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdImpresionDocumento", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pImpresionDocumento", SqlDbType.VarChar, 250).Value = Convert.ToString(pImpresionDocumento);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarImpresionDocumento(string pImpresionDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonImpresionDocumento = new CJson();
        jsonImpresionDocumento.StoredProcedure.CommandText = "sp_ImpresionDocumento_Consultar_FiltroPorImpresionDocumento";
        jsonImpresionDocumento.StoredProcedure.Parameters.AddWithValue("@pImpresionDocumento", pImpresionDocumento);
        return jsonImpresionDocumento.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarImpresionDocumento(Dictionary<string, object> pImpresionDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
            ImpresionDocumento.ImpresionDocumento = Convert.ToString(pImpresionDocumento["ImpresionDocumento"]);
            ImpresionDocumento.Procedimiento = Convert.ToString(pImpresionDocumento["Procedimiento"]);
            string validacion = ValidarImpresionDocumento(ImpresionDocumento, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                ImpresionDocumento.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaImpresionDocumento(int pIdImpresionDocumento)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarImpresionDocumento = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarImpresionDocumento" }, ConexionBaseDatos) == "")
        {
            puedeEditarImpresionDocumento = 1;
        }
        oPermisos.Add("puedeEditarImpresionDocumento", puedeEditarImpresionDocumento);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CImpresionDocumento.ObtenerImpresionDocumento(Modelo, pIdImpresionDocumento, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarImpresionDocumento(int IdImpresionDocumento)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarImpresionDocumento = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarImpresionDocumento" }, ConexionBaseDatos) == "")
        {
            puedeEditarImpresionDocumento = 1;
        }
        oPermisos.Add("puedeEditarImpresionDocumento", puedeEditarImpresionDocumento);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CImpresionDocumento.ObtenerImpresionDocumento(Modelo, IdImpresionDocumento, ConexionBaseDatos);
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
    public static string EditarImpresionDocumento(Dictionary<string, object> pImpresionDocumento)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
        ImpresionDocumento.LlenaObjeto(Convert.ToInt32(pImpresionDocumento["IdImpresionDocumento"]), ConexionBaseDatos);
        ImpresionDocumento.ImpresionDocumento = Convert.ToString(pImpresionDocumento["ImpresionDocumento"]);
        ImpresionDocumento.Procedimiento = Convert.ToString(pImpresionDocumento["Procedimiento"]);

        string validacion = ValidarImpresionDocumento(ImpresionDocumento, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            ImpresionDocumento.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdImpresionDocumento, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
            ImpresionDocumento.IdImpresionDocumento = pIdImpresionDocumento;
            ImpresionDocumento.Baja = pBaja;
            ImpresionDocumento.Eliminar(ConexionBaseDatos);
            respuesta = "0|ImpresionDocumentoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarImpresionDocumento(CImpresionDocumento pImpresionDocumento, CConexion pConexion)
    {
        string errores = "";
        if (pImpresionDocumento.ImpresionDocumento == "")
        { errores = errores + "<span>*</span> El campo tipo de compra esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pImpresionDocumento.Procedimiento == "")
        { errores = errores + "<span>*</span> El campo de procedimiento esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pImpresionDocumento.IdImpresionDocumento == 0)
        {
            if (pImpresionDocumento.ExisteImpresionDocumento(pImpresionDocumento.ImpresionDocumento, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de impresión " + pImpresionDocumento.ImpresionDocumento + " ya esta dada de alta.<br />";
            }
        }
        else
        {
            if (pImpresionDocumento.ExisteImpresionDocumentoEditar(pImpresionDocumento.ImpresionDocumento, pImpresionDocumento.IdImpresionDocumento, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de impresión " + pImpresionDocumento.ImpresionDocumento + " ya esta dada de alta.<br />";
            }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}