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

public partial class TipoCambioDiarioOficial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridTipoCambioDiarioOficial
        CJQGrid GridTipoCambioDiarioOficial = new CJQGrid();
        GridTipoCambioDiarioOficial.NombreTabla = "grdTipoCambioDiarioOficial";
        GridTipoCambioDiarioOficial.CampoIdentificador = "IdTipoCambioDiarioOficial";
        GridTipoCambioDiarioOficial.ColumnaOrdenacion = "TipoCambioDiarioOficial";
        GridTipoCambioDiarioOficial.Metodo = "ObtenerTipoCambioDiarioOficial";
        GridTipoCambioDiarioOficial.TituloTabla = "Catálogo de tipos de cambio";

        //IdTipoCambioDiarioOficial
        CJQColumn ColIdTipoCambioDiarioOficial = new CJQColumn();
        ColIdTipoCambioDiarioOficial.Nombre = "IdTipoCambioDiarioOficial";
        ColIdTipoCambioDiarioOficial.Oculto = "true";
        ColIdTipoCambioDiarioOficial.Encabezado = "IdTipoCambioDiarioOficial";
        ColIdTipoCambioDiarioOficial.Buscador = "false";
        GridTipoCambioDiarioOficial.Columnas.Add(ColIdTipoCambioDiarioOficial);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "Fecha";
        ColFecha.Encabezado = "Fecha";
        ColFecha.Buscador = "false";
        ColFecha.Ancho = "100";
        ColFecha.Alineacion = "Right";
        GridTipoCambioDiarioOficial.Columnas.Add(ColFecha);

        //TipoCambioDiarioOficial
        CJQColumn ColTipoCambioDiarioOficial = new CJQColumn();
        ColTipoCambioDiarioOficial.Nombre = "TipoCambioDiarioOficial";
        ColTipoCambioDiarioOficial.Encabezado = "Tipo de cambio diario oficial";
        ColTipoCambioDiarioOficial.Ancho = "200";
        ColTipoCambioDiarioOficial.Alineacion = "Right";
        ColTipoCambioDiarioOficial.Buscador = "false";
        GridTipoCambioDiarioOficial.Columnas.Add(ColTipoCambioDiarioOficial);

        //MonedaOrigen
        CJQColumn ColMonedaO = new CJQColumn();
        ColMonedaO.Nombre = "IdTipoMonedaOrigen";
        ColMonedaO.Encabezado = "Tipo moneda origen";
        ColMonedaO.Buscador = "false";
        ColMonedaO.Ancho = "200";
        ColMonedaO.Alineacion = "Left";
        GridTipoCambioDiarioOficial.Columnas.Add(ColMonedaO);

        //MonedaDestino
        CJQColumn ColMonedaD = new CJQColumn();
        ColMonedaD.Nombre = "IdTipoMonedaDestino";
        ColMonedaD.Encabezado = "Tipo moneda destino";
        ColMonedaD.Buscador = "false";
        ColMonedaD.Ancho = "200";
        ColMonedaD.Alineacion = "Left";
        GridTipoCambioDiarioOficial.Columnas.Add(ColMonedaD);

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
        GridTipoCambioDiarioOficial.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoCambioDiarioOficial";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoCambioDiarioOficial.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoCambioDiarioOficial", GridTipoCambioDiarioOficial.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarTipoCambioDiarioOficial" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoCambioDiarioOficial(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden,/* string pTipoCambioDiarioOficial,*/ int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoCambioDiarioOficial", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 30).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        //Stored.Parameters.Add("pTipoCambioDiarioOficial", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoCambioDiarioOficial);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoCambioDiarioOficial(string pTipoCambioDiarioOficial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoCambioDiarioOficial = new CJson();
        jsonTipoCambioDiarioOficial.StoredProcedure.CommandText = "sp_TipoCambioDiarioOficial_Consultar_FiltroPorTipoCambioDiarioOficial";
        jsonTipoCambioDiarioOficial.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDiarioOficial", pTipoCambioDiarioOficial);
        return jsonTipoCambioDiarioOficial.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarTipoCambioDiarioOficial()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("TiposMoneda", CTipoMoneda.ObtenerJsonTiposMoneda(ConexionBaseDatos));
            Modelo.Add("Fecha", DateTime.Now.ToString("dd/MM/yyyy"));

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
    public static string AgregarTipoCambioDiarioOficial(Dictionary<string, object> pTipoCambioDiarioOficial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoCambioDiarioOficial TipoCambioDiarioOficial = new CTipoCambioDiarioOficial();
            TipoCambioDiarioOficial.TipoCambioDiarioOficial = Convert.ToDecimal(pTipoCambioDiarioOficial["TipoCambioDiarioOficial"]);
            TipoCambioDiarioOficial.IdTipoMonedaOrigen = Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoMonedaOrigen"]);
            TipoCambioDiarioOficial.IdTipoMonedaDestino = Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoMonedaDestino"]);
            TipoCambioDiarioOficial.Fecha = Convert.ToDateTime(pTipoCambioDiarioOficial["Fecha"]);

            //Conversion inversa
            CTipoCambioDiarioOficial TipoCambioInversa = new CTipoCambioDiarioOficial();
            decimal inverso = 1 / Convert.ToDecimal(pTipoCambioDiarioOficial["TipoCambioDiarioOficial"]);
            TipoCambioInversa.TipoCambioDiarioOficial = Convert.ToDecimal(inverso);
            TipoCambioInversa.IdTipoMonedaOrigen = Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoMonedaDestino"]);
            TipoCambioInversa.IdTipoMonedaDestino = Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoMonedaOrigen"]);
            TipoCambioInversa.Fecha = Convert.ToDateTime(pTipoCambioDiarioOficial["Fecha"]);

            //Conversion pesos
            CTipoCambioDiarioOficial TipoCambioPesos = new CTipoCambioDiarioOficial();
            TipoCambioPesos.TipoCambioDiarioOficial = Convert.ToDecimal(1);
            TipoCambioPesos.IdTipoMonedaOrigen = 1;
            TipoCambioPesos.IdTipoMonedaDestino = 1;
            TipoCambioPesos.Fecha = Convert.ToDateTime(pTipoCambioDiarioOficial["Fecha"]);

            //Conversion dolares
            CTipoCambioDiarioOficial TipoCambioDolares = new CTipoCambioDiarioOficial();
            TipoCambioDolares.TipoCambioDiarioOficial = Convert.ToDecimal(1);
            TipoCambioDolares.IdTipoMonedaOrigen = 2;
            TipoCambioDolares.IdTipoMonedaDestino = 2;
            TipoCambioDolares.Fecha = Convert.ToDateTime(pTipoCambioDiarioOficial["Fecha"]);

            string validacion = ValidarTipoCambioDiarioOficial(TipoCambioDiarioOficial, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoCambioDiarioOficial.Agregar(ConexionBaseDatos);
                TipoCambioInversa.Agregar(ConexionBaseDatos);
                TipoCambioPesos.Agregar(ConexionBaseDatos);
                TipoCambioDolares.Agregar(ConexionBaseDatos);

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
    public static string ObtenerFormaTipoCambioDiarioOficial(int pIdTipoCambioDiarioOficial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCambioDiarioOficial = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioDiarioOficial" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioDiarioOficial = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioDiarioOficial", puedeEditarTipoCambioDiarioOficial);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCambioDiarioOficial TipoCambioDiarioOficial = new CTipoCambioDiarioOficial();
            TipoCambioDiarioOficial.LlenaObjeto(pIdTipoCambioDiarioOficial, ConexionBaseDatos);

            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(TipoCambioDiarioOficial.IdTipoMonedaOrigen, ConexionBaseDatos);

            CTipoMoneda TipoMonedaDes = new CTipoMoneda();
            TipoMonedaDes.LlenaObjeto(TipoCambioDiarioOficial.IdTipoMonedaDestino, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdTipoCambioDiarioOficial", TipoCambioDiarioOficial.IdTipoCambioDiarioOficial));
            Modelo.Add(new JProperty("TipoCambioDiarioOficial", TipoCambioDiarioOficial.TipoCambioDiarioOficial));
            Modelo.Add(new JProperty("TipoMonedaOrigen", TipoMoneda.TipoMoneda));
            Modelo.Add(new JProperty("TipoMonedaDestino", TipoMonedaDes.TipoMoneda));
            Modelo.Add(new JProperty("Fecha", TipoCambioDiarioOficial.Fecha.ToString("dd/MM/yyyy")));
            Modelo.Add(new JProperty("Simbolo", TipoMoneda.Simbolo));
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
    public static string ObtenerFormaEditarTipoCambioDiarioOficial(int IdTipoCambioDiarioOficial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCambioDiarioOficial = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioDiarioOficial" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioDiarioOficial = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioDiarioOficial", puedeEditarTipoCambioDiarioOficial);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCambioDiarioOficial TipoCambioDiarioOficial = new CTipoCambioDiarioOficial();
            TipoCambioDiarioOficial.LlenaObjeto(IdTipoCambioDiarioOficial, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdTipoCambioDiarioOficial", TipoCambioDiarioOficial.IdTipoCambioDiarioOficial));
            Modelo.Add(new JProperty("TipoCambioDiarioOficial", TipoCambioDiarioOficial.TipoCambioDiarioOficial));
            Modelo.Add(new JProperty("TiposMonedaOrigen", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(TipoCambioDiarioOficial.IdTipoMonedaOrigen), ConexionBaseDatos)));
            Modelo.Add(new JProperty("TiposMonedaDestino", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(TipoCambioDiarioOficial.IdTipoMonedaDestino), ConexionBaseDatos)));
            Modelo.Add(new JProperty("Fecha", TipoCambioDiarioOficial.Fecha.ToString("dd/MM/yyyy")));
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
    public static string EditarTipoCambioDiarioOficial(Dictionary<string, object> pTipoCambioDiarioOficial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoCambioDiarioOficial TipoCambioDiarioOficial = new CTipoCambioDiarioOficial();
        TipoCambioDiarioOficial.LlenaObjeto(Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoCambioDiarioOficial"]), ConexionBaseDatos);
        TipoCambioDiarioOficial.TipoCambioDiarioOficial = Convert.ToDecimal(pTipoCambioDiarioOficial["TipoCambioDiarioOficial"]);
        TipoCambioDiarioOficial.IdTipoMonedaOrigen = Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoMonedaOrigen"]);
        TipoCambioDiarioOficial.IdTipoMonedaDestino = Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoMonedaDestino"]);
        TipoCambioDiarioOficial.Fecha = Convert.ToDateTime(pTipoCambioDiarioOficial["Fecha"]);

        string validacion = ValidarTipoCambioDiarioOficial(TipoCambioDiarioOficial, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoCambioDiarioOficial.Editar(ConexionBaseDatos);

            //Tipo de cambio inverso
            CTipoCambioDiarioOficial TipoCambioEditado = new CTipoCambioDiarioOficial();
            TipoCambioEditado.LlenaObjeto(Convert.ToInt32(pTipoCambioDiarioOficial["IdTipoCambioDiarioOficial"]), ConexionBaseDatos);
            CTipoCambioDiarioOficial TipoCambioInverso = new CTipoCambioDiarioOficial();

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            decimal inverso = 1 / Convert.ToDecimal(pTipoCambioDiarioOficial["TipoCambioDiarioOficial"]);
            Parametros.Add("Fecha", TipoCambioEditado.Fecha);
            Parametros.Add("IdTipoMonedaOrigen", TipoCambioEditado.IdTipoMonedaDestino);
            Parametros.Add("IdTipoMonedaDestino", TipoCambioEditado.IdTipoMonedaOrigen);

            TipoCambioInverso.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
            TipoCambioInverso.IdTipoMonedaOrigen = TipoCambioEditado.IdTipoMonedaDestino;
            TipoCambioInverso.IdTipoMonedaDestino = TipoCambioEditado.IdTipoMonedaOrigen;
            TipoCambioInverso.TipoCambioDiarioOficial = inverso;
            TipoCambioInverso.Editar(ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdTipoCambioDiarioOficial, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCambioDiarioOficial TipoCambioDiarioOficial = new CTipoCambioDiarioOficial();
            TipoCambioDiarioOficial.IdTipoCambioDiarioOficial = pIdTipoCambioDiarioOficial;
            TipoCambioDiarioOficial.Baja = pBaja;
            TipoCambioDiarioOficial.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoCambioDiarioOficialEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoCambioDiarioOficial(CTipoCambioDiarioOficial pTipoCambioDiarioOficial, CConexion pConexion)
    {
        string errores = "";
        bool ExisteTipoCambioDiarioOficial = false;

        if (pTipoCambioDiarioOficial.IdTipoCambioDiarioOficial == 0)
        {
            ExisteTipoCambioDiarioOficial = pTipoCambioDiarioOficial.ExisteTipoCambioDiarioOficial(pTipoCambioDiarioOficial.IdTipoMonedaOrigen, pTipoCambioDiarioOficial.IdTipoMonedaDestino, pTipoCambioDiarioOficial.Fecha, pConexion);

            if (ExisteTipoCambioDiarioOficial)
            {
                errores = errores + "<span>*</span> El tipo de cambio ya se encuentra para esta fecha, favor de revisar su edición.<br />";
                return errores;
            }
        }

        if (pTipoCambioDiarioOficial.TipoCambioDiarioOficial.ToString() == "")
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacío, favor de capturarlo.<br />"; }

        if (pTipoCambioDiarioOficial.IdTipoMonedaOrigen.ToString() == "")
        { errores = errores + "<span>*</span> El campo tipo de moneda origen esta vacío, favor de seleccionarlo.<br />"; }

        if (pTipoCambioDiarioOficial.IdTipoMonedaDestino.ToString() == "")
        { errores = errores + "<span>*</span> El campo tipo de moneda destino esta vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}