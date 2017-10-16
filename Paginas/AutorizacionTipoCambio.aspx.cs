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

public partial class AutorizacionTipoCambio : System.Web.UI.Page
{
    public static int puedeAgregarAutorizacionTipoCambio = 0;
    public static int puedeEditarAutorizacionTipoCambio = 0;
    public static int puedeEliminarAutorizacionTipoCambio = 0;
    public static int puedeConsultarAutorizacionTipoCambio = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarAutorizacionTipoCambio" }, ConexionBaseDatos) == "") { puedeAgregarAutorizacionTipoCambio = 1; }
        else { puedeAgregarAutorizacionTipoCambio = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarAutorizacionTipoCambio" }, ConexionBaseDatos) == "") { puedeEditarAutorizacionTipoCambio = 1; }
        else { puedeEditarAutorizacionTipoCambio = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEliminarAutorizacionTipoCambio" }, ConexionBaseDatos) == "") { puedeEliminarAutorizacionTipoCambio = 1; }
        else { puedeEliminarAutorizacionTipoCambio = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeConsultarAutorizacionTipoCambio" }, ConexionBaseDatos) == "") { puedeConsultarAutorizacionTipoCambio = 1; }
        else { puedeConsultarAutorizacionTipoCambio = 0; }

        //grdAutorizacionTipoCambio
        CJQGrid GridAutorizacionTipoCambio = new CJQGrid();
        GridAutorizacionTipoCambio.NombreTabla = "grdAutorizacionTipoCambio";
        GridAutorizacionTipoCambio.CampoIdentificador = "IdAutorizacionTipoCambio";
        GridAutorizacionTipoCambio.ColumnaOrdenacion = "IdAutorizacionTipoCambio";
        GridAutorizacionTipoCambio.Metodo = "ObtenerAutorizacionTipoCambio";
        GridAutorizacionTipoCambio.TituloTabla = "Administración de tipos de cambio";

        //IdAutorizacionTipoCambio
        CJQColumn ColIdAutorizacionTipoCambio = new CJQColumn();
        ColIdAutorizacionTipoCambio.Nombre = "IdAutorizacionTipoCambio";
        ColIdAutorizacionTipoCambio.Oculto = "true";
        ColIdAutorizacionTipoCambio.Encabezado = "Id";
        ColIdAutorizacionTipoCambio.Buscador = "false";
        GridAutorizacionTipoCambio.Columnas.Add(ColIdAutorizacionTipoCambio);

        //UsuarioAutorizo
        CJQColumn ColUsuarioAutorizo = new CJQColumn();
        ColUsuarioAutorizo.Nombre = "U.Nombre";
        ColUsuarioAutorizo.Encabezado = "Autorizó";
        ColUsuarioAutorizo.Ancho = "200";
        ColUsuarioAutorizo.Alineacion = "Left";
        ColUsuarioAutorizo.Buscador = "false";
        GridAutorizacionTipoCambio.Columnas.Add(ColUsuarioAutorizo);

        //UsuarioSolicito
        CJQColumn ColUsuarioSolicito = new CJQColumn();
        ColUsuarioSolicito.Nombre = "Us.Nombre";
        ColUsuarioSolicito.Encabezado = "Solicitó";
        ColUsuarioSolicito.Ancho = "200";
        ColUsuarioSolicito.Alineacion = "Left";
        ColUsuarioSolicito.Buscador = "false";
        GridAutorizacionTipoCambio.Columnas.Add(ColUsuarioSolicito);

        //MonedaOrigen
        CJQColumn ColMonedaOrigen = new CJQColumn();
        ColMonedaOrigen.Nombre = "TM.TipoMoneda";
        ColMonedaOrigen.Encabezado = "Moneda origen";
        ColMonedaOrigen.Ancho = "100";
        ColMonedaOrigen.Alineacion = "Left";
        ColMonedaOrigen.Buscador = "false";
        GridAutorizacionTipoCambio.Columnas.Add(ColMonedaOrigen);

        //MonedaDestino
        CJQColumn ColMonedaDestino = new CJQColumn();
        ColMonedaDestino.Nombre = "TMD.TipoMoneda";
        ColMonedaDestino.Encabezado = "Moneda destino";
        ColMonedaDestino.Ancho = "100";
        ColMonedaDestino.Alineacion = "Left";
        ColMonedaDestino.Buscador = "false";
        GridAutorizacionTipoCambio.Columnas.Add(ColMonedaDestino);

        //TipoCambio
        CJQColumn ColTipoCambio = new CJQColumn();
        ColTipoCambio.Nombre = "TipoCambio";
        ColTipoCambio.Encabezado = "Tipo de cambio";
        ColTipoCambio.Ancho = "100";
        ColTipoCambio.Alineacion = "Left";
        ColTipoCambio.Buscador = "false";
        GridAutorizacionTipoCambio.Columnas.Add(ColTipoCambio);

        //FechaVigencia
        CJQColumn ColFechaVigencia = new CJQColumn();
        ColFechaVigencia.Nombre = "FechaVigencia";
        ColFechaVigencia.Encabezado = "Vigencia";
        ColFechaVigencia.Ancho = "100";
        ColFechaVigencia.Alineacion = "Left";
        ColFechaVigencia.Buscador = "false";
        GridAutorizacionTipoCambio.Columnas.Add(ColFechaVigencia);

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
        GridAutorizacionTipoCambio.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarAutorizacionTipoCambio";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridAutorizacionTipoCambio.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAutorizacionTipoCambio", GridAutorizacionTipoCambio.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarAutorizacionTipoCambio" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAutorizacionTipoCambio(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAutorizacionTipoCambio", sqlCon);
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
    public static string AgregarAutorizacionTipoCambio(Dictionary<string, object> pAutorizacionTipoCambio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CAutorizacionTipoCambio AutorizacionTipoCambio = new CAutorizacionTipoCambio();
            AutorizacionTipoCambio.IdUsuarioAutorizo = Convert.ToInt32(pAutorizacionTipoCambio["IdUsuarioAutorizo"]);
            AutorizacionTipoCambio.IdUsuarioSolicito = Convert.ToInt32(pAutorizacionTipoCambio["IdUsuarioSolicito"]);
            AutorizacionTipoCambio.IdTipoMonedaOrigen = Convert.ToInt32(pAutorizacionTipoCambio["IdTipoMonedaOrigen"]);
            AutorizacionTipoCambio.IdTipoMonedaDestino = Convert.ToInt32(pAutorizacionTipoCambio["IdTipoMonedaDestino"]);
            AutorizacionTipoCambio.FechaVigencia = Convert.ToDateTime(pAutorizacionTipoCambio["FechaVigencia"]);
            AutorizacionTipoCambio.TipoCambio = Convert.ToDecimal(pAutorizacionTipoCambio["TipoCambio"]);
            AutorizacionTipoCambio.ClaveAutorizacion = Convert.ToString(pAutorizacionTipoCambio["ClaveAutorizacion"]);
            AutorizacionTipoCambio.Disponible = true;
            AutorizacionTipoCambio.Fecha = DateTime.Today;

            CTipoDocumento TipoDocumento = new CTipoDocumento();
            TipoDocumento.LlenaObjeto(Convert.ToInt32(pAutorizacionTipoCambio["IdTipoDocumento"]), ConexionBaseDatos);
            AutorizacionTipoCambio.IdTipoDocumento = Convert.ToInt32(TipoDocumento.IdTipoDocumento);
            AutorizacionTipoCambio.TipoDocumento = Convert.ToString(TipoDocumento.Comando);

            string validacion = ValidarAutorizacionTipoCambio(AutorizacionTipoCambio, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                AutorizacionTipoCambio.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaAutorizacionTipoCambio(int pIdAutorizacionTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CTipoMoneda TipoMoneda = new CTipoMoneda();


        oPermisos.Add("puedeEditarAutorizacionTipoCambio", puedeEditarAutorizacionTipoCambio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAutorizacionTipoCambio AutorizacionTipoCambio = new CAutorizacionTipoCambio();
            AutorizacionTipoCambio.LlenaObjeto(pIdAutorizacionTipoCambio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAutorizacionTipoCambio", AutorizacionTipoCambio.IdAutorizacionTipoCambio));
            Modelo.Add(new JProperty("TipoCambio", AutorizacionTipoCambio.TipoCambio));

            Usuario.LlenaObjeto(AutorizacionTipoCambio.IdUsuarioAutorizo, ConexionBaseDatos);
            Modelo.Add(new JProperty("UsuarioAutoriza", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno));

            Usuario.LlenaObjeto(AutorizacionTipoCambio.IdUsuarioSolicito, ConexionBaseDatos);
            Modelo.Add(new JProperty("UsuarioSolicita", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno));

            TipoMoneda.LlenaObjeto(AutorizacionTipoCambio.IdTipoMonedaOrigen, ConexionBaseDatos);
            Modelo.Add(new JProperty("MonedaOrigen", TipoMoneda.TipoMoneda));

            TipoMoneda.LlenaObjeto(AutorizacionTipoCambio.IdTipoMonedaDestino, ConexionBaseDatos);
            Modelo.Add(new JProperty("MonedaDestino", TipoMoneda.TipoMoneda));

            Modelo.Add(new JProperty("FechaVigencia", AutorizacionTipoCambio.FechaVigencia.ToShortDateString()));

            CTipoDocumento TipoDocumento = new CTipoDocumento();
            TipoDocumento.LlenaObjeto(Convert.ToInt32(AutorizacionTipoCambio.IdTipoDocumento), ConexionBaseDatos);


            Modelo.Add(new JProperty("TipoDocumento", TipoDocumento.TipoDocumento));

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
    public static string ObtenerFormaAgregarAutorizacionTipoCambio()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeEditarAutorizacionTipoCambio", puedeEditarAutorizacionTipoCambio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add("TiposMoneda", CTipoMoneda.ObtenerJsonTiposMoneda(ConexionBaseDatos));
            Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuario(ConexionBaseDatos));
            Modelo.Add("TipoDocumentos", CTipoDocumento.ObtenerJsonTipoDocumento(ConexionBaseDatos));
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
    public static string ObtenerFormaEditarAutorizacionTipoCambio(int pIdAutorizacionTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CTipoMoneda TipoMoneda = new CTipoMoneda();

        oPermisos.Add("puedeEditarAutorizacionTipoCambio", puedeEditarAutorizacionTipoCambio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAutorizacionTipoCambio AutorizacionTipoCambio = new CAutorizacionTipoCambio();
            AutorizacionTipoCambio.LlenaObjeto(pIdAutorizacionTipoCambio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAutorizacionTipoCambio", AutorizacionTipoCambio.IdAutorizacionTipoCambio));
            Modelo.Add(new JProperty("UsuariosAutoriza", CUsuario.ObtenerJsonUsuarioNombre(AutorizacionTipoCambio.IdUsuarioAutorizo, ConexionBaseDatos)));
            Modelo.Add(new JProperty("UsuariosSolicita", CUsuario.ObtenerJsonUsuarioNombre(AutorizacionTipoCambio.IdUsuarioSolicito, ConexionBaseDatos)));
            Modelo.Add(new JProperty("TiposMonedaOrigen", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(AutorizacionTipoCambio.IdTipoMonedaOrigen), ConexionBaseDatos)));
            Modelo.Add(new JProperty("TiposMonedaDestino", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(AutorizacionTipoCambio.IdTipoMonedaDestino), ConexionBaseDatos)));
            Modelo.Add(new JProperty("FechaVigencia", AutorizacionTipoCambio.FechaVigencia.ToString("dd/MM/yyyy")));
            Modelo.Add(new JProperty("TipoCambio", AutorizacionTipoCambio.TipoCambio));
            Modelo.Add(new JProperty("TipoDocumento", AutorizacionTipoCambio.TipoDocumento));
            Modelo.Add(new JProperty("ClaveAutorizacion", AutorizacionTipoCambio.ClaveAutorizacion));
            Modelo.Add("TipoDocumentos", CTipoDocumento.ObtenerJsonTipoDocumento(Convert.ToInt32(AutorizacionTipoCambio.IdTipoDocumento), ConexionBaseDatos));

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
    public static string EditarAutorizacionTipoCambio(Dictionary<string, object> pAutorizacionTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAutorizacionTipoCambio AutorizacionTipoCambio = new CAutorizacionTipoCambio();
        AutorizacionTipoCambio.IdAutorizacionTipoCambio = Convert.ToInt32(pAutorizacionTipoCambio["IdAutorizacionTipoCambio"]);
        AutorizacionTipoCambio.IdUsuarioAutorizo = Convert.ToInt32(pAutorizacionTipoCambio["IdUsuarioAutorizo"]);
        AutorizacionTipoCambio.IdUsuarioSolicito = Convert.ToInt32(pAutorizacionTipoCambio["IdUsuarioSolicito"]);
        AutorizacionTipoCambio.IdTipoMonedaOrigen = Convert.ToInt32(pAutorizacionTipoCambio["IdTipoMonedaOrigen"]);
        AutorizacionTipoCambio.IdTipoMonedaDestino = Convert.ToInt32(pAutorizacionTipoCambio["IdTipoMonedaDestino"]);
        AutorizacionTipoCambio.FechaVigencia = Convert.ToDateTime(pAutorizacionTipoCambio["FechaVigencia"]);
        AutorizacionTipoCambio.TipoCambio = Convert.ToDecimal(pAutorizacionTipoCambio["TipoCambio"]);
        AutorizacionTipoCambio.ClaveAutorizacion = Convert.ToString(pAutorizacionTipoCambio["ClaveAutorizacion"]);
        AutorizacionTipoCambio.Disponible = true;
        AutorizacionTipoCambio.Fecha = DateTime.Today;

        CTipoDocumento TipoDocumento = new CTipoDocumento();
        TipoDocumento.LlenaObjeto(Convert.ToInt32(pAutorizacionTipoCambio["IdTipoDocumento"]), ConexionBaseDatos);
        AutorizacionTipoCambio.IdTipoDocumento = Convert.ToInt32(TipoDocumento.IdTipoDocumento);
        AutorizacionTipoCambio.TipoDocumento = Convert.ToString(TipoDocumento.Comando);

        string validacion = ValidarAutorizacionTipoCambio(AutorizacionTipoCambio, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            AutorizacionTipoCambio.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdAutorizacionTipoCambio, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CAutorizacionTipoCambio AutorizacionTipoCambio = new CAutorizacionTipoCambio();
            AutorizacionTipoCambio.IdAutorizacionTipoCambio = pIdAutorizacionTipoCambio;
            AutorizacionTipoCambio.Baja = pBaja;
            AutorizacionTipoCambio.Eliminar(ConexionBaseDatos);
            respuesta = "0|AutorizacionTipoCambioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarAutorizacionTipoCambio(CAutorizacionTipoCambio pAutorizacionTipoCambio, CConexion pConexion)
    {
        string errores = "";

        if (pAutorizacionTipoCambio.IdUsuarioAutorizo == 0)
        { errores = errores + "<span>*</span> El campo usuario autorizó esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionTipoCambio.IdUsuarioSolicito == 0)
        { errores = errores + "<span>*</span> El campo usuario solicitó esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionTipoCambio.IdTipoMonedaOrigen == 0)
        { errores = errores + "<span>*</span> El campo moneda origen esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionTipoCambio.IdTipoMonedaDestino == 0)
        { errores = errores + "<span>*</span> El campo moneda destino esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionTipoCambio.FechaVigencia.ToString() == "")
        { errores = errores + "<span>*</span> El campo fecha vigencia esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionTipoCambio.ClaveAutorizacion == "")
        { errores = errores + "<span>*</span> El campo clave autorización esta vacío, favor de capturarlo.<br />"; }

        if (pAutorizacionTipoCambio.TipoCambio == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}