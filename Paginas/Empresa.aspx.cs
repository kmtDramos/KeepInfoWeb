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

public partial class Empresa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridEmpresa
        CJQGrid GridEmpresa = new CJQGrid();
        GridEmpresa.NombreTabla = "grdEmpresa";
        GridEmpresa.CampoIdentificador = "IdEmpresa";
        GridEmpresa.ColumnaOrdenacion = "Empresa";
        GridEmpresa.Metodo = "ObtenerEmpresa";
        GridEmpresa.TituloTabla = "Catálogo de empresas";

        //IdEmpresa
        CJQColumn ColIdEmpresa = new CJQColumn();
        ColIdEmpresa.Nombre = "IdEmpresa";
        ColIdEmpresa.Oculto = "true";
        ColIdEmpresa.Encabezado = "IdEmpresa";
        ColIdEmpresa.Buscador = "false";
        GridEmpresa.Columnas.Add(ColIdEmpresa);

        //Empresa
        CJQColumn ColEmpresa = new CJQColumn();
        ColEmpresa.Nombre = "Empresa";
        ColEmpresa.Encabezado = "Empresa";
        ColEmpresa.Ancho = "400";
        ColEmpresa.Alineacion = "Left";
        GridEmpresa.Columnas.Add(ColEmpresa);

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
        GridEmpresa.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarEmpresa";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridEmpresa.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdEmpresa", GridEmpresa.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarEmpresa" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerEmpresa(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pEmpresa, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEmpresa", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pEmpresa", SqlDbType.VarChar, 250).Value = Convert.ToString(pEmpresa);
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
    public static string AgregarEmpresa(Dictionary<string, object> pEmpresa)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEmpresa Empresa = new CEmpresa();
            Empresa.IdEmpresa = 0;
            Empresa.RazonSocial = Convert.ToString(pEmpresa["RazonSocial"]);
            Empresa.Empresa = Convert.ToString(pEmpresa["Empresa"]);
            Empresa.RFC = Convert.ToString(pEmpresa["RFC"]);
            Empresa.Telefono = Convert.ToString(pEmpresa["Telefono"]);
            Empresa.Correo = Convert.ToString(pEmpresa["Correo"]);
            Empresa.RegimenFiscal = Convert.ToString(pEmpresa["RegimenFiscal"]);
            Empresa.Dominio = Convert.ToString(pEmpresa["Dominio"]);
            Empresa.Calle = Convert.ToString(pEmpresa["Calle"]);
            Empresa.NumeroExterior = Convert.ToString(pEmpresa["NumeroExterior"]);
            Empresa.NumeroInterior = Convert.ToString(pEmpresa["NumeroInterior"]);
            Empresa.Colonia = Convert.ToString(pEmpresa["Colonia"]);
            Empresa.IdLocalidad = Convert.ToInt32(pEmpresa["IdLocalidad"]);
            Empresa.CodigoPostal = Convert.ToString(pEmpresa["CodigoPostal"]);
            Empresa.IdMunicipio = Convert.ToInt32(pEmpresa["IdMunicipio"]);
            Empresa.IdLocalidad = Convert.ToInt32(pEmpresa["IdLocalidad"]);
            Empresa.Referencia = Convert.ToString(pEmpresa["Referencia"]);
            Empresa.Logo = Convert.ToString(pEmpresa["Logo"]);

            string validacion = ValidarEmpresa(Empresa, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Empresa.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarEmpresa()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(ConexionBaseDatos));
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
    public static string ObtenerListaEstados(int pIdPais)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonEstados(pIdPais, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaMunicipios(int pIdEstado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonMunicipios(pIdEstado, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaLocalidades(int pIdMunicipio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonLocalidades(pIdMunicipio, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerFormaEmpresa(int pIdEmpresa)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEmpresa = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEmpresa" }, ConexionBaseDatos) == "")
        {
            puedeEditarEmpresa = 1;
        }
        oPermisos.Add("puedeEditarEmpresa", puedeEditarEmpresa);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonEmpresa(Modelo, pIdEmpresa, ConexionBaseDatos);

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
    public static string ObtenerFormaEditarEmpresa(int pIdEmpresa)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEmpresa = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEmpresa" }, ConexionBaseDatos) == "")
        {
            puedeEditarEmpresa = 1;
        }
        oPermisos.Add("puedeEditarEmpresa", puedeEditarEmpresa);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(pIdEmpresa, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonEmpresa(Modelo, pIdEmpresa, ConexionBaseDatos);
            Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));
            Modelo.Add("Permisos", oPermisos);
            oRespuesta.Add("Error", 0);
            oRespuesta.Add("Modelo", Modelo);
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
    public static string EditarEmpresa(Dictionary<string, object> pEmpresa)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CEmpresa Empresa = new CEmpresa();
        Empresa.IdEmpresa = Convert.ToInt32(pEmpresa["IdEmpresa"]); ;
        Empresa.RazonSocial = Convert.ToString(pEmpresa["RazonSocial"]);
        Empresa.Empresa = Convert.ToString(pEmpresa["Empresa"]);
        Empresa.RFC = Convert.ToString(pEmpresa["RFC"]);
        Empresa.Telefono = Convert.ToString(pEmpresa["Telefono"]);
        Empresa.Correo = Convert.ToString(pEmpresa["Correo"]);
        Empresa.RegimenFiscal = Convert.ToString(pEmpresa["RegimenFiscal"]);
        Empresa.Dominio = Convert.ToString(pEmpresa["Dominio"]);
        Empresa.Calle = Convert.ToString(pEmpresa["Calle"]);
        Empresa.NumeroExterior = Convert.ToString(pEmpresa["NumeroExterior"]);
        Empresa.NumeroInterior = Convert.ToString(pEmpresa["NumeroInterior"]);
        Empresa.Colonia = Convert.ToString(pEmpresa["Colonia"]);
        Empresa.IdLocalidad = Convert.ToInt32(pEmpresa["IdLocalidad"]);
        Empresa.CodigoPostal = Convert.ToString(pEmpresa["CodigoPostal"]);
        Empresa.IdMunicipio = Convert.ToInt32(pEmpresa["IdMunicipio"]);
        Empresa.Referencia = Convert.ToString(pEmpresa["Referencia"]);
        Empresa.Logo = Convert.ToString(pEmpresa["Logo"]);

        string validacion = ValidarEmpresa(Empresa, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Empresa.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdEmpresa, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEmpresa Empresa = new CEmpresa();
            Empresa.IdEmpresa = pIdEmpresa;
            Empresa.Baja = pBaja;
            Empresa.Eliminar(ConexionBaseDatos);
            respuesta = "0|EmpresaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Metodos Privados
    private static string ValidarEmpresa(CEmpresa pEmpresa, CConexion pConexion)
    {
        string errores = "";
        if (pEmpresa.RazonSocial == "")
        { errores = errores + "<span>*</span> El campo razón social esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.Empresa == "")
        { errores = errores + "<span>*</span> El campo nombre comercial esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.RFC == "")
        { errores = errores + "<span>*</span> El campo RFC esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.RegimenFiscal == "")
        { errores = errores + "<span>*</span> El campo Régimen fiscal esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.Telefono == "")
        { errores = errores + "<span>*</span> El campo teléfono esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.Calle == "")
        { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.NumeroExterior == "")
        { errores = errores + "<span>*</span> El campo numero esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.Colonia == "")
        { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.CodigoPostal == "")
        { errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }
        if (pEmpresa.IdMunicipio == 0)
        { errores = errores + "<span>*</span> El campo municipio no fue capturado, favor de capturarlo.<br />"; }

        if (pEmpresa.Correo != "")
        {
            CValidacion ValidarCorreo = new CValidacion();
            if (ValidarCorreo.ValidarCorreo(pEmpresa.Correo))
            { errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
        }

        CEmpresa Empresa = new CEmpresa();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("RFC", pEmpresa.RFC);
        Empresa.LlenaObjetoFiltros(Parametros, pConexion);

        if (pEmpresa.RFC == Empresa.RFC && pEmpresa.IdEmpresa != Empresa.IdEmpresa)
        {
            errores = errores + "<span>*</span> El RFC ya existe, favor de verificarlo.<br />";
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}