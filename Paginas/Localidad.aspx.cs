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

public partial class Localidad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //GridLocalidad
        CJQGrid GridLocalidad = new CJQGrid();
        GridLocalidad.NombreTabla = "grdLocalidad";
        GridLocalidad.CampoIdentificador = "IdLocalidad";
        GridLocalidad.ColumnaOrdenacion = "Localidad";
        GridLocalidad.Metodo = "ObtenerLocalidad";
        GridLocalidad.TituloTabla = "Catálogo de estados";

        //IdLocalidad
        CJQColumn ColIdLocalidad = new CJQColumn();
        ColIdLocalidad.Nombre = "IdLocalidad";
        ColIdLocalidad.Encabezado = "IdLocalidad";
        ColIdLocalidad.Oculto = "true";
        ColIdLocalidad.Buscador = "false";
        GridLocalidad.Columnas.Add(ColIdLocalidad);

        //Localidad
        CJQColumn ColLocalidad = new CJQColumn();
        ColLocalidad.Nombre = "Localidad";
        ColLocalidad.Encabezado = "Localidad";
        ColLocalidad.TipoBuscador = "";
        ColLocalidad.Buscador = "true";
        ColLocalidad.Alineacion = "left";
        ColLocalidad.Ancho = "75";
        GridLocalidad.Columnas.Add(ColLocalidad);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Buscador = "false";
        ColClave.Alineacion = "left";
        ColClave.Ancho = "50";
        GridLocalidad.Columnas.Add(ColClave);

        //Municipio
        CJQColumn ColMunicipio = new CJQColumn();
        ColMunicipio.Nombre = "Municipio";
        ColMunicipio.Encabezado = "Municipio";
        ColMunicipio.Alineacion = "left";
        ColMunicipio.TipoBuscador = "";
        ColMunicipio.Buscador = "true";
        ColMunicipio.Ancho = "50";
        GridLocalidad.Columnas.Add(ColMunicipio);

        //Estado
        CJQColumn ColEstado = new CJQColumn();
        ColEstado.Nombre = "Estado";
        ColEstado.Encabezado = "Estado";
        ColEstado.Alineacion = "left";
        ColEstado.TipoBuscador = "";
        ColEstado.Buscador = "true";
        ColEstado.Ancho = "50";
        GridLocalidad.Columnas.Add(ColEstado);

        //Pais
        CJQColumn ColPais = new CJQColumn();
        ColPais.Nombre = "Pais";
        ColPais.Encabezado = "Pais";
        ColPais.Alineacion = "left";
        ColPais.TipoBuscador = "";
        ColPais.Buscador = "true";
        ColPais.Ancho = "50";
        GridLocalidad.Columnas.Add(ColPais);

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
        GridLocalidad.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarLocalidad";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridLocalidad.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdLocalidad", GridLocalidad.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerLocalidad(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pLocalidad, string pMunicipio, string pEstado, string pPais, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdLocalidad", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pLocalidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pLocalidad);
        Stored.Parameters.Add("pMunicipio", SqlDbType.VarChar, 250).Value = Convert.ToString(pMunicipio);
        Stored.Parameters.Add("pEstado", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstado);
        Stored.Parameters.Add("pPais", SqlDbType.VarChar, 250).Value = Convert.ToString(pLocalidad);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarLocalidad()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add(new JProperty("Paises", CPais.ObtenerJsonPaises(ConexionBaseDatos)));
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

    [WebMethod]//
    public static string ObtenerFormaLocalidad(int pIdLocalidad)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarLocalidad = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CLocalidad Localidad = new CLocalidad();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarLocalidad" }, ConexionBaseDatos) == "")
        {
            puedeEditarLocalidad = 1;
        }
        oPermisos.Add("puedeEditarLocalidad", puedeEditarLocalidad);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Localidad.LlenaObjeto(pIdLocalidad, ConexionBaseDatos);
            CMunicipio Municipio = new CMunicipio();
            Municipio.LlenaObjeto(Localidad.IdMunicipio, ConexionBaseDatos);
            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
            CPais Pais = new CPais();
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);

            Modelo.Add("IdLocalidad", Localidad.IdLocalidad);
            Modelo.Add("Localidad", Localidad.Localidad);
            Modelo.Add("Clave", Localidad.Clave);

            Modelo.Add("Municipio", Municipio.Municipio);
            Modelo.Add("Estado", Estado.Estado);
            Modelo.Add("Pais", Pais.Pais);

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
    public static string ObtenerFormaEditarLocalidad(int IdLocalidad)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarLocalidad = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CLocalidad Localidad = new CLocalidad();
        if (Usuario.TienePermisos(new string[] { "puedeEditarLocalidad" }, ConexionBaseDatos) == "")
        {
            puedeEditarLocalidad = 1;
        }
        oPermisos.Add("puedeEditarLocalidad", puedeEditarLocalidad);

        if (respuesta == "Conexion Establecida")
        {
            Localidad.LlenaObjeto(IdLocalidad, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("IdLocalidad", Localidad.IdLocalidad));
            Modelo.Add(new JProperty("Localidad", Localidad.Localidad));
            Modelo.Add(new JProperty("Clave", Localidad.Clave));

            CMunicipio Municipio = new CMunicipio();
            Municipio.LlenaObjeto(Localidad.IdMunicipio, ConexionBaseDatos);

            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);

            Modelo.Add("Municipios", CMunicipio.ObtenerJsonMunicipios(Municipio.IdEstado, Localidad.IdMunicipio, ConexionBaseDatos));
            Modelo.Add("Estados", CEstado.ObtenerJsonEstados(Estado.IdPais, Municipio.IdEstado, ConexionBaseDatos));
            Modelo.Add("Paises", CPais.ObtenerJsonPaises(Estado.IdPais, ConexionBaseDatos));

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
            Modelo.Add("Opciones", CEstado.ObtenerJsonEstados(pIdPais, ConexionBaseDatos));
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
            Modelo.Add("Opciones", CMunicipio.ObtenerJsonMunicipios(pIdEstado, ConexionBaseDatos));
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
    public static string AgregarLocalidad(Dictionary<string, object> pLocalidad)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            CLocalidad Localidad = new CLocalidad();
            Localidad.Localidad = pLocalidad["Localidad"].ToString();
            Localidad.Clave = pLocalidad["Clave"].ToString();
            Localidad.IdMunicipio = Convert.ToInt32(pLocalidad["IdMunicipio"]);
            Localidad.Latitud = "";
            Localidad.Longitud = "";
            Localidad.Altitud = "";

            string validacion = ValidarLocalidad(Localidad);
            if (validacion == "")
            {
                Localidad.Agregar(ConexionBaseDatos);
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
    public static string EditarLocalidad(Dictionary<string, object> pLocalidad)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CLocalidad Localidad = new CLocalidad();
        Localidad.LlenaObjeto(Convert.ToInt32(pLocalidad["IdLocalidad"]), ConexionBaseDatos);
        Localidad.Localidad = pLocalidad["Localidad"].ToString();
        Localidad.Clave = pLocalidad["Clave"].ToString();
        Localidad.IdMunicipio = Convert.ToInt32(pLocalidad["IdMunicipio"]);


        string validacion = ValidarLocalidad(Localidad);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Localidad.Editar(ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdLocalidad, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CLocalidad Localidad = new CLocalidad();
            Localidad.IdLocalidad = pIdLocalidad;
            Localidad.Baja = pBaja;
            Localidad.Eliminar(ConexionBaseDatos);
            respuesta = "0|LocalidadEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarLocalidad(CLocalidad pLocalidad)
    {
        string errores = "";
        if (pLocalidad.Localidad == "")
        { errores = errores + "<span>*</span> El campo estado esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pLocalidad.Clave == "")
        { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pLocalidad.IdMunicipio == 0)
        { errores = errores + "<span>*</span> El campo municipio esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}