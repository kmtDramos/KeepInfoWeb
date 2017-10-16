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

public partial class ImpresionTemplate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridImpresionTemplate
        CJQGrid GridImpresionTemplate = new CJQGrid();
        GridImpresionTemplate.NombreTabla = "grdImpresionTemplate";
        GridImpresionTemplate.CampoIdentificador = "IdImpresionTemplate";
        GridImpresionTemplate.ColumnaOrdenacion = "Empresa";
        GridImpresionTemplate.Metodo = "ObtenerImpresionTemplate";
        GridImpresionTemplate.TituloTabla = "Catálogo de templates de impresión";

        //IdImpresionTemplate
        CJQColumn ColIdImpresionTemplate = new CJQColumn();
        ColIdImpresionTemplate.Nombre = "IdImpresionTemplate";
        ColIdImpresionTemplate.Oculto = "true";
        ColIdImpresionTemplate.Encabezado = "IdImpresionTemplate";
        ColIdImpresionTemplate.Buscador = "false";
        GridImpresionTemplate.Columnas.Add(ColIdImpresionTemplate);

        //Empresa
        CJQColumn ColEmpresa = new CJQColumn();
        ColEmpresa.Nombre = "Empresa";
        ColEmpresa.Encabezado = "Empresa";
        ColEmpresa.Ancho = "200";
        ColEmpresa.Alineacion = "Left";
        ColEmpresa.Buscador = "false";
        GridImpresionTemplate.Columnas.Add(ColEmpresa);

        //ImpresionDocumento
        CJQColumn ColImpresionDocumento = new CJQColumn();
        ColImpresionDocumento.Nombre = "ImpresionDocumento";
        ColImpresionDocumento.Encabezado = "Impresión documento";
        ColImpresionDocumento.Buscador = "true";
        ColImpresionDocumento.Ancho = "200";
        ColImpresionDocumento.Alineacion = "Left";
        GridImpresionTemplate.Columnas.Add(ColImpresionDocumento);

        //RutaTemplate
        CJQColumn ColRutaTemplate = new CJQColumn();
        ColRutaTemplate.Nombre = "RutaTemplate";
        ColRutaTemplate.Encabezado = "Ruta template";
        ColRutaTemplate.Buscador = "false";
        ColRutaTemplate.Ancho = "200";
        ColRutaTemplate.Alineacion = "Left";
        GridImpresionTemplate.Columnas.Add(ColRutaTemplate);

        //MonedaDestino
        CJQColumn ColRutaCSS = new CJQColumn();
        ColRutaCSS.Nombre = "RutaCSS";
        ColRutaCSS.Encabezado = "Ruta CSS";
        ColRutaCSS.Buscador = "false";
        ColRutaCSS.Ancho = "200";
        ColRutaCSS.Alineacion = "Left";
        GridImpresionTemplate.Columnas.Add(ColRutaCSS);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "75";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridImpresionTemplate.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarImpresionTemplate";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridImpresionTemplate.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdImpresionTemplate", GridImpresionTemplate.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarImpresionTemplate" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerImpresionTemplate(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pImpresionDocumento, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdImpresionTemplate", sqlCon);
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

        CJson jsonImpresionTemplate = new CJson();
        jsonImpresionTemplate.StoredProcedure.CommandText = "sp_ImpresionDocumento_Consultar_FiltroPorImpresionDocumento";
        jsonImpresionTemplate.StoredProcedure.Parameters.AddWithValue("@pImpresionDocumento", pImpresionDocumento);
        return jsonImpresionTemplate.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarImpresionTemplate()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Empresas", CEmpresa.ObtenerJsonEmpresas(ConexionBaseDatos));
            Modelo.Add("ImpresionDocumentos", CImpresionDocumento.ObtenerJsonImpresionDocumentos(ConexionBaseDatos));

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
    public static string AgregarImpresionTemplate(Dictionary<string, object> pImpresionTemplate)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
            ImpresionTemplate.IdEmpresa = Convert.ToInt32(pImpresionTemplate["IdEmpresa"]);
            ImpresionTemplate.IdImpresionDocumento = Convert.ToInt32(pImpresionTemplate["IdImpresionDocumento"]);
            ImpresionTemplate.RutaTemplate = Convert.ToString(pImpresionTemplate["RutaTemplate"]);
            ImpresionTemplate.RutaCSS = Convert.ToString(pImpresionTemplate["RutaCSS"]);

            string validacion = ValidarImpresionTemplate(ImpresionTemplate, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                ImpresionTemplate.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaImpresionTemplate(int pIdImpresionTemplate)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarImpresionTemplate = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarImpresionTemplate" }, ConexionBaseDatos) == "")
        {
            puedeEditarImpresionTemplate = 1;
        }
        oPermisos.Add("puedeEditarImpresionTemplate", puedeEditarImpresionTemplate);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
            ImpresionTemplate.LlenaObjeto(pIdImpresionTemplate, ConexionBaseDatos);

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(ImpresionTemplate.IdEmpresa, ConexionBaseDatos);

            CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
            ImpresionDocumento.LlenaObjeto(ImpresionTemplate.IdImpresionDocumento, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdImpresionTemplate", ImpresionTemplate.IdImpresionTemplate));
            Modelo.Add(new JProperty("Empresa", Empresa.Empresa));
            Modelo.Add(new JProperty("ImpresionDocumento", ImpresionDocumento.ImpresionDocumento));
            Modelo.Add(new JProperty("RutaTemplate", ImpresionTemplate.RutaTemplate));
            Modelo.Add(new JProperty("RutaCSS", ImpresionTemplate.RutaCSS));
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
    public static string ObtenerFormaEditarImpresionTemplate(int IdImpresionTemplate)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarImpresionTemplate = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarImpresionTemplate" }, ConexionBaseDatos) == "")
        {
            puedeEditarImpresionTemplate = 1;
        }
        oPermisos.Add("puedeEditarImpresionTemplate", puedeEditarImpresionTemplate);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
            ImpresionTemplate.LlenaObjeto(IdImpresionTemplate, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdImpresionTemplate", ImpresionTemplate.IdImpresionTemplate));
            Modelo.Add(new JProperty("Empresas", CEmpresa.ObtenerJsonEmpresas(Convert.ToInt32(ImpresionTemplate.IdEmpresa), ConexionBaseDatos)));
            Modelo.Add(new JProperty("ImpresionDocumentos", CImpresionDocumento.ObtenerJsonImpresionDocumentos(Convert.ToInt32(ImpresionTemplate.IdImpresionDocumento), ConexionBaseDatos)));
            Modelo.Add(new JProperty("RutaTemplate", ImpresionTemplate.RutaTemplate));
            Modelo.Add(new JProperty("RutaCSS", ImpresionTemplate.RutaCSS));
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
    public static string EditarImpresionTemplate(Dictionary<string, object> pImpresionTemplate)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
        ImpresionTemplate.LlenaObjeto(Convert.ToInt32(pImpresionTemplate["IdImpresionTemplate"]), ConexionBaseDatos);
        ImpresionTemplate.IdEmpresa = Convert.ToInt32(pImpresionTemplate["IdEmpresa"]);
        ImpresionTemplate.IdImpresionDocumento = Convert.ToInt32(pImpresionTemplate["IdImpresionDocumento"]);
        ImpresionTemplate.RutaTemplate = Convert.ToString(pImpresionTemplate["RutaTemplate"]);
        ImpresionTemplate.RutaCSS = Convert.ToString(pImpresionTemplate["RutaCSS"]);

        string validacion = ValidarImpresionTemplate(ImpresionTemplate, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            ImpresionTemplate.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdImpresionTemplate, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
            ImpresionTemplate.IdImpresionTemplate = pIdImpresionTemplate;
            ImpresionTemplate.Baja = pBaja;
            ImpresionTemplate.Eliminar(ConexionBaseDatos);
            respuesta = "0|ImpresionTemplateEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarImpresionTemplate(CImpresionTemplate pImpresionTemplate, CConexion pConexion)
    {
        string errores = "";

        if (pImpresionTemplate.IdEmpresa.ToString() == "0")
        { errores = errores + "<span>*</span> El campo empresa esta vacío, favor de capturarlo.<br />"; }

        if (pImpresionTemplate.IdImpresionDocumento.ToString() == "")
        { errores = errores + "<span>*</span> El campo impresión documento esta vacío, favor de seleccionarlo.<br />"; }

        if (pImpresionTemplate.RutaTemplate.ToString() == "")
        { errores = errores + "<span>*</span> El campo ruta template esta vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}