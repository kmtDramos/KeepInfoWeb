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

public partial class Municipio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //GridMunicipio
        CJQGrid GridMunicipio = new CJQGrid();
        GridMunicipio.NombreTabla = "grdMunicipio";
        GridMunicipio.CampoIdentificador = "IdMunicipio";
        GridMunicipio.ColumnaOrdenacion = "Municipio";
        GridMunicipio.Metodo = "ObtenerMunicipio";
        GridMunicipio.TituloTabla = "Catálogo de estados";

        //IdMunicipio
        CJQColumn ColIdMunicipio = new CJQColumn();
        ColIdMunicipio.Nombre = "IdMunicipio";
        ColIdMunicipio.Encabezado = "IdMunicipio";
        ColIdMunicipio.Oculto = "true";
        ColIdMunicipio.Buscador = "false";
        GridMunicipio.Columnas.Add(ColIdMunicipio);

        //Municipio
        CJQColumn ColMunicipio = new CJQColumn();
        ColMunicipio.Nombre = "Municipio";
        ColMunicipio.Encabezado = "Municipio";
        ColMunicipio.TipoBuscador = "";
        ColMunicipio.Buscador = "true";
        ColMunicipio.Alineacion = "left";
        ColMunicipio.Ancho = "75";
        GridMunicipio.Columnas.Add(ColMunicipio);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Buscador = "false";
        ColClave.Alineacion = "left";
        ColClave.Ancho = "50";
        GridMunicipio.Columnas.Add(ColClave);

        //Estado
        CJQColumn ColEstado = new CJQColumn();
        ColEstado.Nombre = "Estado";
        ColEstado.Encabezado = "Estado";
        ColEstado.Alineacion = "left";
        ColEstado.TipoBuscador = "";
        ColEstado.Buscador = "true";
        ColEstado.Ancho = "50";
        GridMunicipio.Columnas.Add(ColEstado);

        //Pais
        CJQColumn ColPais = new CJQColumn();
        ColPais.Nombre = "Pais";
        ColPais.Encabezado = "Pais";
        ColPais.Alineacion = "left";
        ColPais.TipoBuscador = "";
        ColPais.Buscador = "true";
        ColPais.Ancho = "50";
        GridMunicipio.Columnas.Add(ColPais);

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
        GridMunicipio.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarMunicipio";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridMunicipio.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMunicipio", GridMunicipio.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMunicipio(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pMunicipio, string pEstado, string pPais, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdMunicipio", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pMunicipio", SqlDbType.VarChar, 250).Value = Convert.ToString(pMunicipio);
        Stored.Parameters.Add("pEstado", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstado);
        Stored.Parameters.Add("pPais", SqlDbType.VarChar, 250).Value = Convert.ToString(pMunicipio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarMunicipio()
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
    public static string ObtenerFormaMunicipio(int pIdMunicipio)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarMunicipio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CMunicipio Municipio = new CMunicipio();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarMunicipio" }, ConexionBaseDatos) == "")
        {
            puedeEditarMunicipio = 1;
        }
        oPermisos.Add("puedeEditarMunicipio", puedeEditarMunicipio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Municipio.LlenaObjeto(pIdMunicipio, ConexionBaseDatos);
            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
            CPais Pais = new CPais();
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);

            Modelo.Add("IdMunicipio", Municipio.IdMunicipio);
            Modelo.Add("Municipio", Municipio.Municipio);
            Modelo.Add("Clave", Municipio.Clave);

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
    public static string ObtenerFormaEditarMunicipio(int IdMunicipio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarMunicipio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CMunicipio Municipio = new CMunicipio();
        if (Usuario.TienePermisos(new string[] { "puedeEditarMunicipio" }, ConexionBaseDatos) == "")
        {
            puedeEditarMunicipio = 1;
        }
        oPermisos.Add("puedeEditarMunicipio", puedeEditarMunicipio);

        if (respuesta == "Conexion Establecida")
        {
            Municipio.LlenaObjeto(IdMunicipio, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("IdMunicipio", Municipio.IdMunicipio));
            Modelo.Add(new JProperty("Municipio", Municipio.Municipio));
            Modelo.Add(new JProperty("Clave", Municipio.Clave));

            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);

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
    public static string AgregarMunicipio(Dictionary<string, object> pMunicipio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            CMunicipio Municipio = new CMunicipio();
            Municipio.Municipio = pMunicipio["Municipio"].ToString();
            Municipio.Clave = pMunicipio["Clave"].ToString();
            Municipio.IdEstado = Convert.ToInt32(pMunicipio["IdEstado"]);

            string validacion = ValidarMunicipio(Municipio);
            if (validacion == "")
            {
                Municipio.Agregar(ConexionBaseDatos);
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
    public static string EditarMunicipio(Dictionary<string, object> pMunicipio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CMunicipio Municipio = new CMunicipio();
        Municipio.LlenaObjeto(Convert.ToInt32(pMunicipio["IdMunicipio"]), ConexionBaseDatos);
        Municipio.Municipio = pMunicipio["Municipio"].ToString();
        Municipio.Clave = pMunicipio["Clave"].ToString();
        Municipio.IdEstado = Convert.ToInt32(pMunicipio["IdEstado"]);


        string validacion = ValidarMunicipio(Municipio);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Municipio.Editar(ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdMunicipio, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CMunicipio Municipio = new CMunicipio();
            Municipio.IdMunicipio = pIdMunicipio;
            Municipio.Baja = pBaja;
            Municipio.Eliminar(ConexionBaseDatos);
            respuesta = "0|MunicipioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarMunicipio(CMunicipio pMunicipio)
    {
        string errores = "";
        if (pMunicipio.Municipio == "")
        { errores = errores + "<span>*</span> El campo estado esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pMunicipio.Clave == "")
        { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pMunicipio.IdEstado == 0)
        { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}