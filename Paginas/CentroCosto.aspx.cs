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

public partial class CentroCosto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridCentroCosto
        CJQGrid GridCentroCosto = new CJQGrid();
        GridCentroCosto.NombreTabla = "grdCentroCosto";
        GridCentroCosto.CampoIdentificador = "IdCentroCosto";
        GridCentroCosto.ColumnaOrdenacion = "CentroCosto";
        GridCentroCosto.Metodo = "ObtenerCentroCosto";
        GridCentroCosto.TituloTabla = "Catálogo de centro de costos";

        //IdCentroCosto
        CJQColumn ColIdCentroCosto = new CJQColumn();
        ColIdCentroCosto.Nombre = "IdCentroCosto";
        ColIdCentroCosto.Oculto = "true";
        ColIdCentroCosto.Encabezado = "IdCentroCosto";
        ColIdCentroCosto.Buscador = "false";
        GridCentroCosto.Columnas.Add(ColIdCentroCosto);

        //CentroCosto
        CJQColumn ColCentroCosto = new CJQColumn();
        ColCentroCosto.Nombre = "CentroCosto";
        ColCentroCosto.Encabezado = "Centro de costos";
        ColCentroCosto.Ancho = "100";
        ColCentroCosto.Alineacion = "left";
        GridCentroCosto.Columnas.Add(ColCentroCosto);

        //Monto
        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Buscador = "false";
        ColMonto.Formato = "FormatoMoneda";
        ColMonto.Alineacion = "right";
        ColMonto.Ancho = "60";
        GridCentroCosto.Columnas.Add(ColMonto);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Ancho = "170";
        ColDescripcion.Buscador = "false";
        GridCentroCosto.Columnas.Add(ColDescripcion);

        //CuentaContable
        CJQColumn ColCuentaContable = new CJQColumn();
        ColCuentaContable.Nombre = "CuentaContable";
        ColCuentaContable.Encabezado = "CuentaContable";
        ColCuentaContable.Ancho = "100";
        ColCuentaContable.Buscador = "false";
        GridCentroCosto.Columnas.Add(ColCuentaContable);

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
        GridCentroCosto.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCentroCosto";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCentroCosto.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCentroCosto", GridCentroCosto.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCentroCosto(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCentroCosto, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCentroCosto", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pCentroCosto", SqlDbType.VarChar, 250).Value = Convert.ToString(pCentroCosto);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarCentroCosto(string pCentroCosto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonCentroCosto = new CJson();
        jsonCentroCosto.StoredProcedure.CommandText = "sp_CentroCosto_Consultar_FiltroPorCentroCosto";
        jsonCentroCosto.StoredProcedure.Parameters.AddWithValue("@pCentroCosto", pCentroCosto);
        string jsonCentroCostoString = jsonCentroCosto.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCentroCostoString;
    }

    [WebMethod]
    public static string AgregarCentroCosto(Dictionary<string, object> pCentroCosto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CCentroCosto CentroCosto = new CCentroCosto();
            CentroCosto.CentroCosto = Convert.ToString(pCentroCosto["CentroCosto"]);
            CentroCosto.Monto = Convert.ToDecimal(pCentroCosto["Monto"]);
            CentroCosto.Descripcion = Convert.ToString(pCentroCosto["Descripcion"]);
            CentroCosto.IdCuentaContable = Convert.ToInt32(pCentroCosto["IdCuentaContable"]);
            CentroCosto.CuentaContable = Convert.ToString(pCentroCosto["CuentaContable"]);
            CentroCosto.FechaAlta = Convert.ToDateTime(DateTime.Now);
            CentroCosto.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            string validacion = ValidarCentroCosto(CentroCosto, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                CentroCosto.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = CentroCosto.IdCentroCosto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un centro de costo";
                HistorialGenerico.AgregarHistorialGenerico("CentroCosto", ConexionBaseDatos);

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
    public static string ObtenerFormaCentroCosto(int pIdCentroCosto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCentroCosto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCentroCosto" }, ConexionBaseDatos) == "")
        {
            puedeEditarCentroCosto = 1;
        }
        oPermisos.Add("puedeEditarCentroCosto", puedeEditarCentroCosto);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CCentroCosto.ObtenerCentroCosto(Modelo, pIdCentroCosto, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarCentroCosto(int IdCentroCosto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCentroCosto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCentroCosto" }, ConexionBaseDatos) == "")
        {
            puedeEditarCentroCosto = 1;
        }
        oPermisos.Add("puedeEditarCentroCosto", puedeEditarCentroCosto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CCentroCosto.ObtenerCentroCosto(Modelo, IdCentroCosto, ConexionBaseDatos);
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
    public static string EditarCentroCosto(Dictionary<string, object> pCentroCosto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCentroCosto CentroCosto = new CCentroCosto();
        CentroCosto.LlenaObjeto(Convert.ToInt32(pCentroCosto["IdCentroCosto"]), ConexionBaseDatos);
        CentroCosto.IdCentroCosto = Convert.ToInt32(pCentroCosto["IdCentroCosto"]);
        CentroCosto.CentroCosto = Convert.ToString(pCentroCosto["CentroCosto"]);
        CentroCosto.Monto = Convert.ToDecimal(pCentroCosto["Monto"]);
        CentroCosto.Descripcion = Convert.ToString(pCentroCosto["Descripcion"]);
        CentroCosto.IdCuentaContable = Convert.ToInt32(pCentroCosto["IdCuentaContable"]);
        CentroCosto.CuentaContable = Convert.ToString(pCentroCosto["CuentaContable"]);
        string validacion = ValidarCentroCosto(CentroCosto, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CentroCosto.Editar(ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = CentroCosto.IdCentroCosto;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se edito un centro de costo";
            HistorialGenerico.AgregarHistorialGenerico("CentroCosto", ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdCentroCosto, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCentroCosto CentroCosto = new CCentroCosto();
            CentroCosto.IdCentroCosto = pIdCentroCosto;
            CentroCosto.Baja = pBaja;
            CentroCosto.Eliminar(ConexionBaseDatos);
            respuesta = "0|CentroCostoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string BuscarCuentaContable(string pCuentaContable)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCuentaContable jsonCuentaContable = new CCuentaContable();
        jsonCuentaContable.StoredProcedure.CommandText = "sp_Proveedor_ConsultarCuentaContable";
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", pCuentaContable);
        string jsonCuentaContableString = jsonCuentaContable.ObtenerJsonCuentaContable(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCuentaContableString;

    }

    //Validaciones
    private static string ValidarCentroCosto(CCentroCosto pCentroCosto, CConexion pConexion)
    {
        string errores = "";
        if (pCentroCosto.CentroCosto == "")
        { errores = errores + "<span>*</span> El campo centro de costos esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pCentroCosto.Monto == 0)
        { errores = errores + "<span>*</span> El monto esta vacío, favor de capturarlo.<br />"; }

        if (pCentroCosto.IdCuentaContable == 0)
        { errores = errores + "<span>*</span> La cuenta contable esta vacia, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}