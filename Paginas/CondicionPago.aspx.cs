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

public partial class CondicionPago : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridCondicionPago
        CJQGrid GridCondicionPago = new CJQGrid();
        GridCondicionPago.NombreTabla = "grdCondicionPago";
        GridCondicionPago.CampoIdentificador = "IdCondicionPago";
        GridCondicionPago.ColumnaOrdenacion = "CondicionPago";
        GridCondicionPago.Metodo = "ObtenerCondicionPago";
        GridCondicionPago.TituloTabla = "Catálogo de condiciones de pago";

        //IdCondicionPago
        CJQColumn ColIdCondicionPago = new CJQColumn();
        ColIdCondicionPago.Nombre = "IdCondicionPago";
        ColIdCondicionPago.Oculto = "true";
        ColIdCondicionPago.Encabezado = "IdCondicionPago";
        ColIdCondicionPago.Buscador = "false";
        GridCondicionPago.Columnas.Add(ColIdCondicionPago);

        //CondicionPago
        CJQColumn ColCondicionPago = new CJQColumn();
        ColCondicionPago.Nombre = "CondicionPago";
        ColCondicionPago.Encabezado = "Condición de pago";
        ColCondicionPago.Ancho = "400";
        ColCondicionPago.Alineacion = "left";
        GridCondicionPago.Columnas.Add(ColCondicionPago);

        //NumeroDias
        CJQColumn ColNumeroDias = new CJQColumn();
        ColNumeroDias.Nombre = "NumeroDias";
        ColNumeroDias.Encabezado = "Número de días";
        ColNumeroDias.Ancho = "55";
        ColNumeroDias.Alineacion = "right";
        ColNumeroDias.Buscador = "false";
        GridCondicionPago.Columnas.Add(ColNumeroDias);

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
        GridCondicionPago.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCondicionPago";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCondicionPago.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCondicionPago", GridCondicionPago.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCondicionPago(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCondicionPago, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCondicionPago", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pCondicionPago", SqlDbType.VarChar, 250).Value = Convert.ToString(pCondicionPago);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarCondicionPago(string pCondicionPago)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonCondicionPago = new CJson();
        jsonCondicionPago.StoredProcedure.CommandText = "sp_CondicionPago_Consultar_FiltroPorCondicionPago";
        jsonCondicionPago.StoredProcedure.Parameters.AddWithValue("@pCondicionPago", pCondicionPago);
        string jsonCodicionPagoString = jsonCondicionPago.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCodicionPagoString;
    }

    [WebMethod]
    public static string AgregarCondicionPago(Dictionary<string, object> pCondicionPago)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CCondicionPago CondicionPago = new CCondicionPago();
            CondicionPago.CondicionPago = Convert.ToString(pCondicionPago["CondicionPago"]);
            CondicionPago.NumeroDias = Convert.ToInt32(pCondicionPago["NumeroDias"]);
            string validacion = ValidarCondicionPago(CondicionPago, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                CondicionPago.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaCondicionPago(int pIdCondicionPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCondicionPago = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCondicionPago" }, ConexionBaseDatos) == "")
        {
            puedeEditarCondicionPago = 1;
        }
        oPermisos.Add("puedeEditarCondicionPago", puedeEditarCondicionPago);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCondicionPago CondicionPago = new CCondicionPago();
            CondicionPago.LlenaObjeto(pIdCondicionPago, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCondicionPago", CondicionPago.IdCondicionPago));
            Modelo.Add(new JProperty("CondicionPago", CondicionPago.CondicionPago));
            Modelo.Add(new JProperty("NumeroDias", CondicionPago.NumeroDias));

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
    public static string ObtenerFormaEditarCondicionPago(int IdCondicionPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCondicionPago = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCondicionPago" }, ConexionBaseDatos) == "")
        {
            puedeEditarCondicionPago = 1;
        }
        oPermisos.Add("puedeEditarCondicionPago", puedeEditarCondicionPago);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCondicionPago CondicionPago = new CCondicionPago();
            CondicionPago.LlenaObjeto(IdCondicionPago, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCondicionPago", CondicionPago.IdCondicionPago));
            Modelo.Add(new JProperty("CondicionPago", CondicionPago.CondicionPago));
            Modelo.Add(new JProperty("NumeroDias", CondicionPago.NumeroDias));
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
    public static string EditarCondicionPago(Dictionary<string, object> pCondicionPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCondicionPago CondicionPago = new CCondicionPago();
        CondicionPago.IdCondicionPago = Convert.ToInt32(pCondicionPago["IdCondicionPago"]); ;
        CondicionPago.CondicionPago = Convert.ToString(pCondicionPago["CondicionPago"]);
        CondicionPago.NumeroDias = Convert.ToInt32(pCondicionPago["NumeroDias"]);
        string validacion = ValidarCondicionPago(CondicionPago, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CondicionPago.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdCondicionPago, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCondicionPago CondicionPago = new CCondicionPago();
            CondicionPago.IdCondicionPago = pIdCondicionPago;
            CondicionPago.Baja = pBaja;
            CondicionPago.Eliminar(ConexionBaseDatos);
            respuesta = "0|CondicionPagoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarCondicionPago(CCondicionPago pCondicionPago, CConexion pConexion)
    {
        string errores = "";
        if (pCondicionPago.CondicionPago == "")
        { errores = errores + "<span>*</span> El campo de condición de pago esta vacio, favor de capturarlo.<br />"; }
        if (pCondicionPago.NumeroDias == 0)
        { errores = errores + "<span>*</span> El campo de número de dias esta vacio, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}