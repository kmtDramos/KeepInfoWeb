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

public partial class AutorizacionIVA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //grdAutorizacionIVA
        CJQGrid GridAutorizacionIVA = new CJQGrid();
        GridAutorizacionIVA.NombreTabla = "grdAutorizacionIVA";
        GridAutorizacionIVA.CampoIdentificador = "IdAutorizacionIVA";
        GridAutorizacionIVA.ColumnaOrdenacion = "IdAutorizacionIVA";
        GridAutorizacionIVA.Metodo = "ObtenerAutorizacionIVA";
        GridAutorizacionIVA.TituloTabla = "Administración de autorización de IVAs";

        //IdAutorizacionIVA
        CJQColumn ColIdAutorizacionIVA = new CJQColumn();
        ColIdAutorizacionIVA.Nombre = "IdAutorizacionIVA";
        ColIdAutorizacionIVA.Oculto = "true";
        ColIdAutorizacionIVA.Encabezado = "Id";
        ColIdAutorizacionIVA.Buscador = "false";
        GridAutorizacionIVA.Columnas.Add(ColIdAutorizacionIVA);

        //UsuarioAutorizo
        CJQColumn ColUsuarioAutorizo = new CJQColumn();
        ColUsuarioAutorizo.Nombre = "UsuarioAutorizo";
        ColUsuarioAutorizo.Encabezado = "Autorizó";
        ColUsuarioAutorizo.Ancho = "200";
        ColUsuarioAutorizo.Alineacion = "Left";
        ColUsuarioAutorizo.Buscador = "false";
        GridAutorizacionIVA.Columnas.Add(ColUsuarioAutorizo);

        //UsuarioSolicito
        CJQColumn ColUsuarioSolicito = new CJQColumn();
        ColUsuarioSolicito.Nombre = "UsuarioSolicito";
        ColUsuarioSolicito.Encabezado = "Solicitó";
        ColUsuarioSolicito.Ancho = "200";
        ColUsuarioSolicito.Alineacion = "Left";
        ColUsuarioSolicito.Buscador = "false";
        GridAutorizacionIVA.Columnas.Add(ColUsuarioSolicito);

        //IVA
        CJQColumn ColIVA = new CJQColumn();
        ColIVA.Nombre = "IVA";
        ColIVA.Encabezado = "IVA";
        ColIVA.Ancho = "100";
        ColIVA.Alineacion = "Left";
        ColIVA.Buscador = "false";
        GridAutorizacionIVA.Columnas.Add(ColIVA);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "Fecha";
        ColFecha.Encabezado = "Inicio";
        ColFecha.Ancho = "100";
        ColFecha.Alineacion = "Left";
        ColFecha.Buscador = "false";
        GridAutorizacionIVA.Columnas.Add(ColFecha);

        //FechaVigencia
        CJQColumn ColFechaVigencia = new CJQColumn();
        ColFechaVigencia.Nombre = "FechaVigencia";
        ColFechaVigencia.Encabezado = "Fin";
        ColFechaVigencia.Ancho = "100";
        ColFechaVigencia.Alineacion = "Left";
        ColFechaVigencia.Buscador = "false";
        GridAutorizacionIVA.Columnas.Add(ColFechaVigencia);

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
        GridAutorizacionIVA.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarAutorizacionIVA";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridAutorizacionIVA.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAutorizacionIVA", GridAutorizacionIVA.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarAutorizacionIVA" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAutorizacionIVA(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAutorizacionIVA", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 50).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarAutorizacionIVA()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarAutorizacionIVA = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (Usuario.TienePermisos(new string[] { "puedeAgregarAutorizacionIVA" }, ConexionBaseDatos) == "")
        {
            puedeAgregarAutorizacionIVA = 1;
        }
        oPermisos.Add("puedeAgregarAutorizacionIVA", puedeAgregarAutorizacionIVA);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuario(ConexionBaseDatos));
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
    public static string AgregarAutorizacionIVA(Dictionary<string, object> pAutorizacionIVA)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CAutorizacionIVA AutorizacionIVA = new CAutorizacionIVA();
            AutorizacionIVA.IdUsuarioAutorizo = Convert.ToInt32(pAutorizacionIVA["IdUsuarioAutorizo"]);
            AutorizacionIVA.IdUsuarioSolicito = Convert.ToInt32(pAutorizacionIVA["IdUsuarioSolicito"]);
            AutorizacionIVA.FechaVigencia = Convert.ToDateTime(pAutorizacionIVA["FechaVigencia"]);
            AutorizacionIVA.IVA = Convert.ToDecimal(pAutorizacionIVA["IVA"]);
            AutorizacionIVA.ClaveAutorizacion = Convert.ToString(pAutorizacionIVA["ClaveAutorizacion"]);
            AutorizacionIVA.Disponible = true;
            AutorizacionIVA.Fecha = DateTime.Today;
            AutorizacionIVA.TipoDocumento = Convert.ToString(pAutorizacionIVA["TipoDocumento"]);

            string validacion = ValidarAutorizacionIVA(AutorizacionIVA, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                AutorizacionIVA.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaAutorizacionIVA(int pIdAutorizacionIVA)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeConsultarAutorizacionIVA = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CTipoMoneda TipoMoneda = new CTipoMoneda();

        if (Usuario.TienePermisos(new string[] { "puedeConsultarAutorizacionIVA" }, ConexionBaseDatos) == "")
        {
            puedeConsultarAutorizacionIVA = 1;
        }
        oPermisos.Add("puedeConsultarAutorizacionIVA", puedeConsultarAutorizacionIVA);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAutorizacionIVA AutorizacionIVA = new CAutorizacionIVA();
            AutorizacionIVA.LlenaObjeto(pIdAutorizacionIVA, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAutorizacionIVA", AutorizacionIVA.IdAutorizacionIVA));

            Usuario.LlenaObjeto(AutorizacionIVA.IdUsuarioAutorizo, ConexionBaseDatos);
            Modelo.Add(new JProperty("UsuarioAutoriza", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno));

            Usuario.LlenaObjeto(AutorizacionIVA.IdUsuarioSolicito, ConexionBaseDatos);
            Modelo.Add(new JProperty("UsuarioSolicita", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno));

            Modelo.Add(new JProperty("FechaVigencia", AutorizacionIVA.FechaVigencia.ToShortDateString()));
            Modelo.Add(new JProperty("IVA", AutorizacionIVA.IVA));
            Modelo.Add(new JProperty("TipoDocumento", AutorizacionIVA.TipoDocumento));

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
    public static string ObtenerFormaEditarAutorizacionIVA(int pIdAutorizacionIVA)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarAutorizacionIVA = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CTipoMoneda TipoMoneda = new CTipoMoneda();

        if (Usuario.TienePermisos(new string[] { "puedeEditarAutorizacionIVA" }, ConexionBaseDatos) == "")
        {
            puedeEditarAutorizacionIVA = 1;
        }
        oPermisos.Add("puedeEditarAutorizacionIVA", puedeEditarAutorizacionIVA);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAutorizacionIVA AutorizacionIVA = new CAutorizacionIVA();
            AutorizacionIVA.LlenaObjeto(pIdAutorizacionIVA, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAutorizacionIVA", AutorizacionIVA.IdAutorizacionIVA));
            Modelo.Add(new JProperty("UsuariosAutoriza", CUsuario.ObtenerJsonUsuarioNombre(AutorizacionIVA.IdUsuarioAutorizo, ConexionBaseDatos)));
            Modelo.Add(new JProperty("UsuariosSolicita", CUsuario.ObtenerJsonUsuarioNombre(AutorizacionIVA.IdUsuarioSolicito, ConexionBaseDatos)));
            Modelo.Add(new JProperty("FechaVigencia", AutorizacionIVA.FechaVigencia.ToString("dd/MM/yyyy")));
            Modelo.Add(new JProperty("IVA", AutorizacionIVA.IVA));
            Modelo.Add(new JProperty("TipoDocumento", AutorizacionIVA.TipoDocumento));
            Modelo.Add(new JProperty("ClaveAutorizacion", AutorizacionIVA.ClaveAutorizacion));

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
    public static string EditarAutorizacionIVA(Dictionary<string, object> pAutorizacionIVA)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAutorizacionIVA AutorizacionIVA = new CAutorizacionIVA();
        AutorizacionIVA.IdAutorizacionIVA = Convert.ToInt32(pAutorizacionIVA["IdAutorizacionIVA"]);
        AutorizacionIVA.IdUsuarioAutorizo = Convert.ToInt32(pAutorizacionIVA["IdUsuarioAutorizo"]);
        AutorizacionIVA.IdUsuarioSolicito = Convert.ToInt32(pAutorizacionIVA["IdUsuarioSolicito"]);
        AutorizacionIVA.FechaVigencia = Convert.ToDateTime(pAutorizacionIVA["FechaVigencia"]);
        AutorizacionIVA.IVA = Convert.ToDecimal(pAutorizacionIVA["IVA"]);
        AutorizacionIVA.TipoDocumento = Convert.ToString(pAutorizacionIVA["TipoDocumento"]);
        AutorizacionIVA.ClaveAutorizacion = Convert.ToString(pAutorizacionIVA["ClaveAutorizacion"]);
        AutorizacionIVA.Disponible = true;
        AutorizacionIVA.Fecha = DateTime.Today;

        string validacion = ValidarAutorizacionIVA(AutorizacionIVA, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            AutorizacionIVA.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdAutorizacionIVA, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CAutorizacionIVA AutorizacionIVA = new CAutorizacionIVA();
            AutorizacionIVA.IdAutorizacionIVA = pIdAutorizacionIVA;
            AutorizacionIVA.Baja = pBaja;
            AutorizacionIVA.Eliminar(ConexionBaseDatos);
            respuesta = "0|AutorizacionIVAEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarAutorizacionIVA(CAutorizacionIVA pAutorizacionIVA, CConexion pConexion)
    {
        string errores = "";

        if (pAutorizacionIVA.IdUsuarioAutorizo == 0)
        { errores = errores + "<span>*</span> El campo usuario autorizó esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionIVA.IdUsuarioSolicito == 0)
        { errores = errores + "<span>*</span> El campo usuario solicitó esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionIVA.FechaVigencia.ToString() == "")
        { errores = errores + "<span>*</span> El campo fecha vigencia esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionIVA.ClaveAutorizacion == "")
        { errores = errores + "<span>*</span> El campo clave autorización esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionIVA.IVA == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}