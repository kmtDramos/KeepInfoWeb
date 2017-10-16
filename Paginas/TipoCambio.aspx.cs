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

public partial class TipoCambio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridTipoCambio
        CJQGrid GridTipoCambio = new CJQGrid();
        GridTipoCambio.NombreTabla = "grdTipoCambio";
        GridTipoCambio.CampoIdentificador = "IdTipoCambio";
        GridTipoCambio.ColumnaOrdenacion = "TipoCambio";
        GridTipoCambio.Metodo = "ObtenerTipoCambio";
        GridTipoCambio.TituloTabla = "Catálogo de tipos de cambio";

        //IdTipoCambio
        CJQColumn ColIdTipoCambio = new CJQColumn();
        ColIdTipoCambio.Nombre = "IdTipoCambio";
        ColIdTipoCambio.Oculto = "true";
        ColIdTipoCambio.Encabezado = "IdTipoCambio";
        ColIdTipoCambio.Buscador = "false";
        GridTipoCambio.Columnas.Add(ColIdTipoCambio);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "Fecha";
        ColFecha.Encabezado = "Fecha";
        ColFecha.Buscador = "false";
        ColFecha.Ancho = "100";
        ColFecha.Alineacion = "Right";
        GridTipoCambio.Columnas.Add(ColFecha);

        //TipoCambio
        CJQColumn ColTipoCambio = new CJQColumn();
        ColTipoCambio.Nombre = "TipoCambio";
        ColTipoCambio.Encabezado = "Tipo de cambio";
        ColTipoCambio.Ancho = "200";
        ColTipoCambio.Alineacion = "Right";
        ColTipoCambio.Buscador = "false";
        GridTipoCambio.Columnas.Add(ColTipoCambio);

        //MonedaOrigen
        CJQColumn ColMonedaO = new CJQColumn();
        ColMonedaO.Nombre = "IdTipoMonedaOrigen";
        ColMonedaO.Encabezado = "Tipo moneda origen";
        ColMonedaO.Buscador = "false";
        ColMonedaO.Ancho = "200";
        ColMonedaO.Alineacion = "Left";
        GridTipoCambio.Columnas.Add(ColMonedaO);

        //MonedaDestino
        CJQColumn ColMonedaD = new CJQColumn();
        ColMonedaD.Nombre = "IdTipoMonedaDestino";
        ColMonedaD.Encabezado = "Tipo moneda destino";
        ColMonedaD.Buscador = "false";
        ColMonedaD.Ancho = "200";
        ColMonedaD.Alineacion = "Left";
        GridTipoCambio.Columnas.Add(ColMonedaD);

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
        GridTipoCambio.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoCambio";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoCambio.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoCambio", GridTipoCambio.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarTipoCambio" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoCambio(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden,/* string pTipoCambio,*/ int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoCambio", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        //Stored.Parameters.Add("pTipoCambio", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoCambio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoCambio(string pTipoCambio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoCambio = new CJson();
        jsonTipoCambio.StoredProcedure.CommandText = "sp_TipoCambio_Consultar_FiltroPorTipoCambio";
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", pTipoCambio);
        return jsonTipoCambio.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarTipoCambio()
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
    public static string AgregarTipoCambio(Dictionary<string, object> pTipoCambio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCambio TipoCambio = new CTipoCambio();
            TipoCambio.TipoCambio = Convert.ToDecimal(pTipoCambio["TipoCambio"]);
            TipoCambio.IdTipoMonedaOrigen = Convert.ToInt32(pTipoCambio["IdTipoMonedaOrigen"]);
            TipoCambio.IdTipoMonedaDestino = Convert.ToInt32(pTipoCambio["IdTipoMonedaDestino"]);
            TipoCambio.Fecha = Convert.ToDateTime(pTipoCambio["Fecha"]);

            //Conversion inversa
            CTipoCambio TipoCambioInversa = new CTipoCambio();
            decimal inverso = 1 / Convert.ToDecimal(pTipoCambio["TipoCambio"]);
            TipoCambioInversa.TipoCambio = Convert.ToDecimal(inverso);
            TipoCambioInversa.IdTipoMonedaOrigen = Convert.ToInt32(pTipoCambio["IdTipoMonedaDestino"]);
            TipoCambioInversa.IdTipoMonedaDestino = Convert.ToInt32(pTipoCambio["IdTipoMonedaOrigen"]);
            TipoCambioInversa.Fecha = Convert.ToDateTime(pTipoCambio["Fecha"]);

            //Conversion pesos
            CTipoCambio TipoCambioPesos = new CTipoCambio();
            TipoCambioPesos.TipoCambio = Convert.ToDecimal(1);
            TipoCambioPesos.IdTipoMonedaOrigen = 1;
            TipoCambioPesos.IdTipoMonedaDestino = 1;
            TipoCambioPesos.Fecha = Convert.ToDateTime(pTipoCambio["Fecha"]);

            //Conversion dolares
            CTipoCambio TipoCambioDolares = new CTipoCambio();
            TipoCambioDolares.TipoCambio = Convert.ToDecimal(1);
            TipoCambioDolares.IdTipoMonedaOrigen = 2;
            TipoCambioDolares.IdTipoMonedaDestino = 2;
            TipoCambioDolares.Fecha = Convert.ToDateTime(pTipoCambio["Fecha"]);

            string validacion = ValidarTipoCambio(TipoCambio, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoCambio.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoCambio(int pIdTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCambio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambio" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambio = 1;
        }
        oPermisos.Add("puedeEditarTipoCambio", puedeEditarTipoCambio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCambio TipoCambio = new CTipoCambio();
            TipoCambio.LlenaObjeto(pIdTipoCambio, ConexionBaseDatos);

            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(TipoCambio.IdTipoMonedaOrigen, ConexionBaseDatos);

            CTipoMoneda TipoMonedaDes = new CTipoMoneda();
            TipoMonedaDes.LlenaObjeto(TipoCambio.IdTipoMonedaDestino, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdTipoCambio", TipoCambio.IdTipoCambio));
            Modelo.Add(new JProperty("TipoCambio", TipoCambio.TipoCambio));
            Modelo.Add(new JProperty("TipoMonedaOrigen", TipoMoneda.TipoMoneda));
            Modelo.Add(new JProperty("TipoMonedaDestino", TipoMonedaDes.TipoMoneda));
            Modelo.Add(new JProperty("Fecha", TipoCambio.Fecha.ToString("dd/MM/yyyy")));
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
    public static string ObtenerFormaEditarTipoCambio(int IdTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCambio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambio" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambio = 1;
        }
        oPermisos.Add("puedeEditarTipoCambio", puedeEditarTipoCambio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCambio TipoCambio = new CTipoCambio();
            TipoCambio.LlenaObjeto(IdTipoCambio, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdTipoCambio", TipoCambio.IdTipoCambio));
            Modelo.Add(new JProperty("TipoCambio", TipoCambio.TipoCambio));
            Modelo.Add(new JProperty("TiposMonedaOrigen", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(TipoCambio.IdTipoMonedaOrigen), ConexionBaseDatos)));
            Modelo.Add(new JProperty("TiposMonedaDestino", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(TipoCambio.IdTipoMonedaDestino), ConexionBaseDatos)));
            Modelo.Add(new JProperty("Fecha", TipoCambio.Fecha.ToString("dd/MM/yyyy")));
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
    public static string EditarTipoCambio(Dictionary<string, object> pTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoCambio TipoCambio = new CTipoCambio();
        TipoCambio.LlenaObjeto(Convert.ToInt32(pTipoCambio["IdTipoCambio"]), ConexionBaseDatos);
        TipoCambio.TipoCambio = Convert.ToDecimal(pTipoCambio["TipoCambio"]);
        TipoCambio.IdTipoMonedaOrigen = Convert.ToInt32(pTipoCambio["IdTipoMonedaOrigen"]);
        TipoCambio.IdTipoMonedaDestino = Convert.ToInt32(pTipoCambio["IdTipoMonedaDestino"]);
        TipoCambio.Fecha = Convert.ToDateTime(pTipoCambio["Fecha"]);

        string validacion = ValidarTipoCambio(TipoCambio, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoCambio.Editar(ConexionBaseDatos);

            //Tipo de cambio inverso
            CTipoCambio TipoCambioEditado = new CTipoCambio();
            TipoCambioEditado.LlenaObjeto(Convert.ToInt32(pTipoCambio["IdTipoCambio"]), ConexionBaseDatos);
            CTipoCambio TipoCambioInverso = new CTipoCambio();

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            decimal inverso = 1 / Convert.ToDecimal(pTipoCambio["TipoCambio"]);
            Parametros.Add("Fecha", TipoCambioEditado.Fecha);
            Parametros.Add("IdTipoMonedaOrigen", TipoCambioEditado.IdTipoMonedaDestino);
            Parametros.Add("IdTipoMonedaDestino", TipoCambioEditado.IdTipoMonedaOrigen);

            TipoCambioInverso.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
            TipoCambioInverso.IdTipoMonedaOrigen = TipoCambioEditado.IdTipoMonedaDestino;
            TipoCambioInverso.IdTipoMonedaDestino = TipoCambioEditado.IdTipoMonedaOrigen;
            TipoCambioInverso.TipoCambio = inverso;
            TipoCambioInverso.Editar(ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdTipoCambio, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCambio TipoCambio = new CTipoCambio();
            TipoCambio.IdTipoCambio = pIdTipoCambio;
            TipoCambio.Baja = pBaja;
            TipoCambio.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoCambioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoCambio(CTipoCambio pTipoCambio, CConexion pConexion)
    {
        string errores = "";
        bool ExisteTipoCambio = false;

        if (pTipoCambio.IdTipoCambio == 0)
        {
            ExisteTipoCambio = pTipoCambio.ExisteTipoCambio(pTipoCambio.IdTipoMonedaOrigen, pTipoCambio.IdTipoMonedaDestino, pTipoCambio.Fecha, pConexion);

            if (ExisteTipoCambio)
            {
                errores = errores + "<span>*</span> El tipo de cambio ya se encuentra para esta fecha, favor de revisar su edición.<br />";
                return errores;
            }
        }

        if (pTipoCambio.TipoCambio.ToString() == "")
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacío, favor de capturarlo.<br />"; }

        if (pTipoCambio.IdTipoMonedaOrigen.ToString() == "")
        { errores = errores + "<span>*</span> El campo tipo de moneda origen esta vacío, favor de seleccionarlo.<br />"; }

        if (pTipoCambio.IdTipoMonedaDestino.ToString() == "")
        { errores = errores + "<span>*</span> El campo tipo de moneda destino esta vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}