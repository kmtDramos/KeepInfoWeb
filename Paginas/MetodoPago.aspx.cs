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

public partial class MetodoPago : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridMetodoPago
        CJQGrid GridMetodoPago = new CJQGrid();
        GridMetodoPago.NombreTabla = "grdMetodoPago";
        GridMetodoPago.CampoIdentificador = "IdMetodoPago";
        GridMetodoPago.ColumnaOrdenacion = "MetodoPago";
        GridMetodoPago.Metodo = "ObtenerMetodoPago";
        GridMetodoPago.TituloTabla = "Catálogo de tipos metodos de pago";

        //IdMetodoPago
        CJQColumn ColIdMetodoPago = new CJQColumn();
        ColIdMetodoPago.Nombre = "IdMetodoPago";
        ColIdMetodoPago.Oculto = "true";
        ColIdMetodoPago.Encabezado = "IdMetodoPago";
        ColIdMetodoPago.Buscador = "false";
        GridMetodoPago.Columnas.Add(ColIdMetodoPago);

        //MetodoPago
        CJQColumn ColMetodoPago = new CJQColumn();
        ColMetodoPago.Nombre = "MetodoPago";
        ColMetodoPago.Encabezado = "Tipo metodo de pago";
        ColMetodoPago.Ancho = "350";
        ColMetodoPago.Alineacion = "Left";
        GridMetodoPago.Columnas.Add(ColMetodoPago);

        //Tipo de movimientos
        CJQColumn ColTipoMovimiento = new CJQColumn();
        ColTipoMovimiento.Nombre = "TipoMovimiento";
        ColTipoMovimiento.Encabezado = "Tipo de movimiento";
        ColTipoMovimiento.Ancho = "350";
        ColTipoMovimiento.Alineacion = "Left";
        ColTipoMovimiento.Buscador = "true";
        ColTipoMovimiento.TipoBuscador = "Combo";
        ColTipoMovimiento.StoredProcedure.CommandText = "sp_TipoMovimiento_Combo";
        GridMetodoPago.Columnas.Add(ColTipoMovimiento);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Ancho = "80";
        ColClave.Alineacion = "Left";
        ColClave.Buscador = "false";
        ColClave.Ordenable = "false";
        GridMetodoPago.Columnas.Add(ColClave);

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
        GridMetodoPago.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarMetodoPago";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridMetodoPago.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMetodoPago", GridMetodoPago.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarMetodoPago" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMetodoPago(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pMetodoPago, int pTipoMovimiento, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdMetodoPago", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pMetodoPago", SqlDbType.VarChar, 250).Value = Convert.ToString(pMetodoPago);
        Stored.Parameters.Add("pIdTipoMovimiento", SqlDbType.Int).Value = pTipoMovimiento;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarMetodoPago(string pMetodoPago)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonMetodoPago = new CJson();
        jsonMetodoPago.StoredProcedure.CommandText = "sp_MetodoPago_Consultar_FiltroPorMetodoPago";
        jsonMetodoPago.StoredProcedure.Parameters.AddWithValue("@pMetodoPago", pMetodoPago);
        return jsonMetodoPago.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarMetodoPago()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Baja", 0);
            CTipoMovimiento TipoMovimiento = new CTipoMovimiento();
            JArray JTipoMovimientos = new JArray();
            foreach (CTipoMovimiento oTipoMovimiento in TipoMovimiento.LlenaObjetosFiltros(ParametrosTS, ConexionBaseDatos))
            {
                JObject JTipoMovimiento = new JObject();
                JTipoMovimiento.Add(new JProperty("IdTipoMovimiento", oTipoMovimiento.IdTipoMovimiento));
                JTipoMovimiento.Add(new JProperty("TipoMovimiento", oTipoMovimiento.TipoMovimiento));
                JTipoMovimientos.Add(JTipoMovimiento);
            }
            Modelo.Add(new JProperty("TipoMovimientos", JTipoMovimientos));

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
    public static string AgregarMetodoPago(Dictionary<string, object> pMetodoPago)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CMetodoPago MetodoPago = new CMetodoPago();
            MetodoPago.MetodoPago = Convert.ToString(pMetodoPago["MetodoPago"]);
            MetodoPago.IdTipoMovimiento = Convert.ToInt32(pMetodoPago["IdTipoMovimiento"]);
            MetodoPago.Clave = Convert.ToString(pMetodoPago["Clave"]);

            string validacion = ValidarMetodoPago(MetodoPago, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                MetodoPago.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaMetodoPago(int pIdMetodoPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarMetodoPago = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarMetodoPago" }, ConexionBaseDatos) == "")
        {
            puedeEditarMetodoPago = 1;
        }
        oPermisos.Add("puedeEditarMetodoPago", puedeEditarMetodoPago);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CMetodoPago MetodoPago = new CMetodoPago();
            MetodoPago.LlenaObjeto(pIdMetodoPago, ConexionBaseDatos);
            Modelo.Add("IdMetodoPago", MetodoPago.IdMetodoPago);
            Modelo.Add("MetodoPago", MetodoPago.MetodoPago);


            CTipoMovimiento TipoMovimiento = new CTipoMovimiento();//aqui llena el campo de consulta
            TipoMovimiento.LlenaObjeto(MetodoPago.IdTipoMovimiento, ConexionBaseDatos);
            Modelo.Add("TipoMovimiento", TipoMovimiento.TipoMovimiento);

            Modelo.Add("Clave", MetodoPago.Clave);

            Modelo.Add("Permisos", oPermisos);
            oRespuesta.Add("Error", 0);
            oRespuesta.Add("Modelo", Modelo);
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", "No hay conexion a Base de Datos");
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarMetodoPago(int IdMetodoPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarMetodoPago = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarMetodoPago" }, ConexionBaseDatos) == "")
        {
            puedeEditarMetodoPago = 1;
        }
        oPermisos.Add("puedeEditarMetodoPago", puedeEditarMetodoPago);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CMetodoPago MetodoPago = new CMetodoPago();
            MetodoPago.LlenaObjeto(IdMetodoPago, ConexionBaseDatos);
            Modelo.Add("IdMetodoPago", MetodoPago.IdMetodoPago);
            Modelo.Add("MetodoPago", MetodoPago.MetodoPago);
            Modelo.Add("Clave", MetodoPago.Clave);


            CTipoMovimiento TipoMovimiento = new CTipoMovimiento();//aqui llena el combo para su edicion
            TipoMovimiento.LlenaObjeto(MetodoPago.IdTipoMovimiento, ConexionBaseDatos);
            Modelo.Add("IdTipoMovimiento", TipoMovimiento.IdTipoMovimiento);
            Modelo.Add("TipoMovimiento", TipoMovimiento.TipoMovimiento);
            JArray JTipoMovimientos = new JArray();
            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Baja", 0);
            foreach (CTipoMovimiento oTipoMovimiento in TipoMovimiento.LlenaObjetosFiltros(ParametrosTS, ConexionBaseDatos))
            {
                JObject JTipoMovimiento = new JObject();
                JTipoMovimiento.Add("IdTipoMovimiento", oTipoMovimiento.IdTipoMovimiento);
                JTipoMovimiento.Add("TipoMovimiento", oTipoMovimiento.TipoMovimiento);
                if (TipoMovimiento.IdTipoMovimiento == oTipoMovimiento.IdTipoMovimiento)
                {
                    JTipoMovimiento.Add("Selected", 1);
                }
                else
                {
                    JTipoMovimiento.Add("Selected", 0);
                }
                JTipoMovimientos.Add(JTipoMovimiento);
            }
            Modelo.Add("TipoMovimientos", JTipoMovimientos);

            Modelo.Add("Permisos", oPermisos);
            oRespuesta.Add("Error", 0);
            oRespuesta.Add("Modelo", Modelo);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", "No hay conexion a Base de Datos");
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarMetodoPago(Dictionary<string, object> pMetodoPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CMetodoPago MetodoPago = new CMetodoPago();
        MetodoPago.IdMetodoPago = Convert.ToInt32(pMetodoPago["IdMetodoPago"]);
        MetodoPago.MetodoPago = Convert.ToString(pMetodoPago["MetodoPago"]);
        MetodoPago.IdTipoMovimiento = Convert.ToInt32(pMetodoPago["IdTipoMovimiento"]);
        MetodoPago.Clave = Convert.ToString(pMetodoPago["Clave"]);

        string validacion = ValidarMetodoPago(MetodoPago, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            MetodoPago.Editar(ConexionBaseDatos);
            oRespuesta.Add("Error", 0);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", validacion);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdMetodoPago, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CMetodoPago MetodoPago = new CMetodoPago();
            MetodoPago.IdMetodoPago = pIdMetodoPago;
            MetodoPago.Baja = pBaja;
            MetodoPago.Eliminar(ConexionBaseDatos);
            respuesta = "0|MetodoPagoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarMetodoPago(CMetodoPago pMetodoPago, CConexion pConexion)
    {
        string errores = "";
        if (pMetodoPago.MetodoPago == "")
        { errores = errores + "<span>*</span> El campo MetodoPago esta vacío, favor de capturarlo.<br />"; }

        if (pMetodoPago.IdTipoMovimiento == 0)
        { errores = errores + "<span>*</span> El campo tipo de movimiento esta vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

}