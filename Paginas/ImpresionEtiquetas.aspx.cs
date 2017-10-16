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

public partial class ImpresionEtiquetas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridImpresionEtiquetas
        CJQGrid GridImpresionEtiquetas = new CJQGrid();
        GridImpresionEtiquetas.NombreTabla = "grdImpresionEtiquetas";
        GridImpresionEtiquetas.CampoIdentificador = "IdImpresionEtiquetas";
        GridImpresionEtiquetas.ColumnaOrdenacion = "Empresa";
        GridImpresionEtiquetas.Metodo = "ObtenerImpresionEtiquetas";
        GridImpresionEtiquetas.TituloTabla = "Catálogo de impresión etiquetas";

        //IdImpresionEtiquetas
        CJQColumn ColIdImpresionEtiquetas = new CJQColumn();
        ColIdImpresionEtiquetas.Nombre = "IdImpresionEtiquetas";
        ColIdImpresionEtiquetas.Oculto = "true";
        ColIdImpresionEtiquetas.Encabezado = "IdImpresionEtiquetas";
        ColIdImpresionEtiquetas.Buscador = "false";
        GridImpresionEtiquetas.Columnas.Add(ColIdImpresionEtiquetas);

        //Empresa
        CJQColumn ColEmpresa = new CJQColumn();
        ColEmpresa.Nombre = "Empresa";
        ColEmpresa.Encabezado = "Empresa";
        ColEmpresa.Buscador = "true";
        ColEmpresa.Ancho = "200";
        ColEmpresa.Alineacion = "Left";
        GridImpresionEtiquetas.Columnas.Add(ColEmpresa);

        //ImpresionDocumento
        CJQColumn ColImpresionDocumento = new CJQColumn();
        ColImpresionDocumento.Nombre = "ImpresionDocumento";
        ColImpresionDocumento.Encabezado = "Impresión documento";
        ColImpresionDocumento.Buscador = "true";
        ColImpresionDocumento.Ancho = "200";
        ColImpresionDocumento.Alineacion = "Left";
        GridImpresionEtiquetas.Columnas.Add(ColImpresionDocumento);

        //RutaTemplate
        CJQColumn ColRutaTemplate = new CJQColumn();
        ColRutaTemplate.Nombre = "Campo";
        ColRutaTemplate.Encabezado = "Campo";
        ColRutaTemplate.Buscador = "false";
        ColRutaTemplate.Ancho = "200";
        ColRutaTemplate.Alineacion = "Left";
        GridImpresionEtiquetas.Columnas.Add(ColRutaTemplate);

        //MonedaDestino
        CJQColumn ColRutaCSS = new CJQColumn();
        ColRutaCSS.Nombre = "Etiqueta";
        ColRutaCSS.Encabezado = "Etiqueta";
        ColRutaCSS.Buscador = "false";
        ColRutaCSS.Ancho = "200";
        ColRutaCSS.Alineacion = "Left";
        GridImpresionEtiquetas.Columnas.Add(ColRutaCSS);

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
        GridImpresionEtiquetas.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarImpresionEtiquetas";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridImpresionEtiquetas.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdImpresionEtiquetas", GridImpresionEtiquetas.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarImpresionEtiquetas" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerImpresionEtiquetas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pEmpresa, string pImpresionDocumento, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdImpresionEtiquetas", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pEmpresa", SqlDbType.VarChar, 250).Value = Convert.ToString(pEmpresa);
        Stored.Parameters.Add("pImpresionDocumento", SqlDbType.VarChar, 250).Value = Convert.ToString(pImpresionDocumento);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarEmpresa(string pEmpresa)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonEmpresa = new CJson();
        jsonEmpresa.StoredProcedure.CommandText = "sp_Empresa_Consultar_FiltroPorEmpresa";
        jsonEmpresa.StoredProcedure.Parameters.AddWithValue("@pEmpresa", pEmpresa);
        return jsonEmpresa.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarImpresionDocumento(string pImpresionDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonImpresionEtiquetas = new CJson();
        jsonImpresionEtiquetas.StoredProcedure.CommandText = "sp_ImpresionDocumento_Consultar_FiltroPorImpresionDocumento";
        jsonImpresionEtiquetas.StoredProcedure.Parameters.AddWithValue("@pImpresionDocumento", pImpresionDocumento);
        return jsonImpresionEtiquetas.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarImpresionEtiquetas()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("Templates", CImpresionTemplate.ObtenerJsonImpresionTemplates(ConexionBaseDatos)));
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
    public static string AgregarImpresionEtiquetas(Dictionary<string, object> pImpresionEtiquetas)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();
            ImpresionEtiquetas.IdImpresionTemplate = Convert.ToInt32(pImpresionEtiquetas["IdImpresionTemplate"]);
            ImpresionEtiquetas.Campo = Convert.ToString(pImpresionEtiquetas["Campo"]);
            ImpresionEtiquetas.Etiqueta = Convert.ToString(pImpresionEtiquetas["Etiqueta"]);

            string validacion = ValidarImpresionEtiquetas(ImpresionEtiquetas, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                ImpresionEtiquetas.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaImpresionEtiquetas(int pIdImpresionEtiquetas)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarImpresionEtiquetas = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarImpresionEtiquetas" }, ConexionBaseDatos) == "")
        {
            puedeEditarImpresionEtiquetas = 1;
        }
        oPermisos.Add("puedeEditarImpresionEtiquetas", puedeEditarImpresionEtiquetas);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();
            ImpresionEtiquetas.LlenaObjeto(pIdImpresionEtiquetas, ConexionBaseDatos);

            CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
            ImpresionTemplate.LlenaObjeto(ImpresionEtiquetas.IdImpresionTemplate, ConexionBaseDatos);

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(ImpresionTemplate.IdEmpresa, ConexionBaseDatos);

            CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
            ImpresionDocumento.LlenaObjeto(ImpresionTemplate.IdImpresionDocumento, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdImpresionEtiquetas", ImpresionEtiquetas.IdImpresionEtiquetas));
            Modelo.Add(new JProperty("Template", Empresa.Empresa + " - " + ImpresionDocumento.ImpresionDocumento));
            Modelo.Add(new JProperty("Campo", ImpresionEtiquetas.Campo));
            Modelo.Add(new JProperty("Etiqueta", ImpresionEtiquetas.Etiqueta));

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
    public static string ObtenerFormaEditarImpresionEtiquetas(int IdImpresionEtiquetas)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarImpresionEtiquetas = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarImpresionEtiquetas" }, ConexionBaseDatos) == "")
        {
            puedeEditarImpresionEtiquetas = 1;
        }
        oPermisos.Add("puedeEditarImpresionEtiquetas", puedeEditarImpresionEtiquetas);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();
            ImpresionEtiquetas.LlenaObjeto(IdImpresionEtiquetas, ConexionBaseDatos);

            CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
            ImpresionTemplate.LlenaObjeto(ImpresionEtiquetas.IdImpresionTemplate, ConexionBaseDatos);

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(ImpresionTemplate.IdEmpresa, ConexionBaseDatos);

            CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
            ImpresionDocumento.LlenaObjeto(ImpresionTemplate.IdImpresionDocumento, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdImpresionEtiquetas", ImpresionEtiquetas.IdImpresionEtiquetas));
            Modelo.Add(new JProperty("Templates", CImpresionTemplate.ObtenerJsonImpresionTemplates(ImpresionEtiquetas.IdImpresionTemplate, ConexionBaseDatos)));
            Modelo.Add(new JProperty("Campo", ImpresionEtiquetas.Campo));
            Modelo.Add(new JProperty("Etiqueta", ImpresionEtiquetas.Etiqueta.Substring(1, ImpresionEtiquetas.Etiqueta.Length - 2)));

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
    public static string EditarImpresionEtiquetas(Dictionary<string, object> pImpresionEtiquetas)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();
        ImpresionEtiquetas.LlenaObjeto(Convert.ToInt32(pImpresionEtiquetas["IdImpresionEtiquetas"]), ConexionBaseDatos);
        ImpresionEtiquetas.IdImpresionTemplate = Convert.ToInt32(pImpresionEtiquetas["IdImpresionTemplate"]);
        ImpresionEtiquetas.Campo = Convert.ToString(pImpresionEtiquetas["Campo"]);
        ImpresionEtiquetas.Etiqueta = Convert.ToString(pImpresionEtiquetas["Etiqueta"]);

        string validacion = ValidarImpresionEtiquetas(ImpresionEtiquetas, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            ImpresionEtiquetas.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdImpresionEtiquetas, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CImpresionEtiquetas ImpresionEtiquetas = new CImpresionEtiquetas();
            ImpresionEtiquetas.IdImpresionEtiquetas = pIdImpresionEtiquetas;
            ImpresionEtiquetas.Baja = pBaja;
            ImpresionEtiquetas.Eliminar(ConexionBaseDatos);
            respuesta = "0|ImpresionEtiquetasEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarImpresionEtiquetas(CImpresionEtiquetas pImpresionEtiquetas, CConexion pConexion)
    {
        string errores = "";

        if (pImpresionEtiquetas.IdImpresionTemplate.ToString() == "0")
        { errores = errores + "<span>*</span> El campo template esta vacío, favor de capturarlo.<br />"; }

        if (pImpresionEtiquetas.Campo.ToString() == "")
        { errores = errores + "<span>*</span> El campo esta vacío, favor de seleccionarlo.<br />"; }

        if (pImpresionEtiquetas.Etiqueta.ToString() == "")
        { errores = errores + "<span>*</span> El campo etiqueta esta vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}