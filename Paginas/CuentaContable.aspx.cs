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

public partial class CuentaContable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridCuentaContable
        CJQGrid GridCuentaContable = new CJQGrid();
        GridCuentaContable.NombreTabla = "grdCuentaContable";
        GridCuentaContable.CampoIdentificador = "IdCuentaContable";
        GridCuentaContable.ColumnaOrdenacion = "CuentaContable";
        GridCuentaContable.Metodo = "ObtenerCuentaContable";
        GridCuentaContable.TituloTabla = "Catálogo de cuentas contables";

        //IdCuentaContable
        CJQColumn ColIdCuentaContable = new CJQColumn();
        ColIdCuentaContable.Nombre = "IdCuentaContable";
        ColIdCuentaContable.Oculto = "true";
        ColIdCuentaContable.Encabezado = "IdCuentaContable";
        ColIdCuentaContable.Buscador = "false";
        GridCuentaContable.Columnas.Add(ColIdCuentaContable);

        //CuentaContable
        CJQColumn ColCuentaContable = new CJQColumn();
        ColCuentaContable.Nombre = "CuentaContable";
        ColCuentaContable.Encabezado = "Cuentas contables";
        ColCuentaContable.Ancho = "100";
        ColCuentaContable.Alineacion = "left";
        GridCuentaContable.Columnas.Add(ColCuentaContable);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "100";
        ColSucursal.Buscador = "false";
        ColSucursal.Alineacion = "left";
        GridCuentaContable.Columnas.Add(ColSucursal);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Division";
        ColDivision.Ancho = "100";
        ColDivision.Buscador = "false";
        ColDivision.Alineacion = "left";
        GridCuentaContable.Columnas.Add(ColDivision);

        //TipoCompra
        CJQColumn ColTipoCompra = new CJQColumn();
        ColTipoCompra.Nombre = "TipoCompra";
        ColTipoCompra.Encabezado = "Tipo de compra";
        ColTipoCompra.Ancho = "100";
        ColTipoCompra.Buscador = "false";
        ColTipoCompra.Alineacion = "left";
        GridCuentaContable.Columnas.Add(ColTipoCompra);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Ancho = "100";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        GridCuentaContable.Columnas.Add(ColDescripcion);

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
        GridCuentaContable.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCuentaContable";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCuentaContable.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCuentaContable", GridCuentaContable.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCuentaContable(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCuentaContable, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentaContable", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pCuentaContable", SqlDbType.VarChar, 250).Value = Convert.ToString(pCuentaContable);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarCuentaContable(string pCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonCuentaContable = new CJson();
        jsonCuentaContable.StoredProcedure.CommandText = "sp_CuentaContable_Consultar_FiltroPorCuentaContable";
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", pCuentaContable);
        string jsonCuentaContableString = jsonCuentaContable.ObtenerJsonString(ConexionBaseDatos);
        return jsonCuentaContable.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarCuentaContable(Dictionary<string, object> pCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CCuentaContable CuentaContable = new CCuentaContable();
            CuentaContable.CuentaContable = Convert.ToString(pCuentaContable["CuentaContable"]);
            CuentaContable.IdSucursal = Convert.ToInt32(pCuentaContable["IdSucursal"]);
            CuentaContable.IdDivision = Convert.ToInt32(pCuentaContable["IdDivision"]);
            CuentaContable.IdTipoCompra = Convert.ToInt32(pCuentaContable["IdTipoCompra"]);
            CuentaContable.IdTipoCuentaContable = 1;
            CuentaContable.Descripcion = Convert.ToString(pCuentaContable["Descripcion"]);
            string validacion = ValidarCuentaContable(CuentaContable, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                CuentaContable.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaCuentaContable(int pIdCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarCuentaContable", puedeEditarCuentaContable);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CCuentaContable.ObtenerCuentaContable(Modelo, pIdCuentaContable, ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarCuentaContable()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        string CuentaContableGenerada = "";
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarCuentaContable", puedeEditarCuentaContable);

        if (respuesta == "Conexion Establecida")
        {
            CCuentaContable CuentaContable = new CCuentaContable();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CuentaContableGenerada = CuentaContable.ObtenerCuentaContableGenerada(Usuario.IdSucursalActual, 0, 0, 0, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(ConexionBaseDatos));
            Modelo.Add("TipoCompras", CJson.ObtenerJsonTipoCompra(ConexionBaseDatos));
            Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(ConexionBaseDatos));
            Modelo.Add(new JProperty("CuentaContable", CuentaContableGenerada));
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
    public static string ObtenerCuentaContableGenerada(Dictionary<string, object> pCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        Dictionary<string, object> CuentaContableGenerada = new Dictionary<string, object>();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            CCuentaContable CuentaContable = new CCuentaContable();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CuentaContableGenerada = CuentaContable.ObtenerCuentaContableGeneradaSegmentos(Convert.ToInt32(pCuentaContable["IdSucursal"]), Convert.ToInt32(pCuentaContable["IdDivision"]), Convert.ToInt32(pCuentaContable["IdTipoCompra"]), 0, ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("SegmentoSucursal", CuentaContableGenerada["SegmentoSucursal"].ToString());
            Modelo.Add("SegmentoDivision", CuentaContableGenerada["SegmentoDivision"].ToString());
            Modelo.Add("SegmentoTipoCompra", CuentaContableGenerada["SegmentoTipoCompra"].ToString());
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexión con la base de datos."));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string ObtenerFormaEditarCuentaContable(int IdCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarCuentaContable", puedeEditarCuentaContable);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CCuentaContable.ObtenerCuentaContable(Modelo, IdCuentaContable, ConexionBaseDatos);
            Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(Convert.ToInt32(Modelo["IdDivision"].ToString()), ConexionBaseDatos));
            Modelo.Add("TipoCompras", CJson.ObtenerJsonTipoCompra(Convert.ToInt32(Modelo["IdTipoCompra"].ToString()), ConexionBaseDatos));
            Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(Convert.ToInt32(Modelo["IdSucursal"].ToString()), ConexionBaseDatos));
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
    public static string EditarCuentaContable(Dictionary<string, object> pCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CCuentaContable CuentaContable = new CCuentaContable();
        CuentaContable.LlenaObjeto(Convert.ToInt32(pCuentaContable["IdCuentaContable"]), ConexionBaseDatos);
        CuentaContable.IdCuentaContable = Convert.ToInt32(pCuentaContable["IdCuentaContable"]);
        CuentaContable.CuentaContable = Convert.ToString(pCuentaContable["CuentaContable"]);
        CuentaContable.IdSucursal = Convert.ToInt32(pCuentaContable["IdSucursal"]);
        CuentaContable.IdDivision = Convert.ToInt32(pCuentaContable["IdDivision"]);
        CuentaContable.IdTipoCompra = Convert.ToInt32(pCuentaContable["IdTipoCompra"]);
        CuentaContable.Descripcion = Convert.ToString(pCuentaContable["Descripcion"]);

        string validacion = ValidarCuentaContable(CuentaContable, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CuentaContable.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdCuentaContable, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCuentaContable CuentaContable = new CCuentaContable();
            CuentaContable.IdCuentaContable = pIdCuentaContable;
            CuentaContable.Baja = pBaja;
            CuentaContable.Eliminar(ConexionBaseDatos);
            respuesta = "0|CuentaContableEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerFormaTipoCuentaContable()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("DescripcionDefalut", "Seleccionar...");
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("Opciones", CTipoCuentaContable.ObtenerTiposCuentasContables(ConexionBaseDatos));
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
    public static string ObtenerFormaAgregarCuentaContableComplementos(int pIdTipoCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarCuentaContable", puedeEditarCuentaContable);

        if (respuesta == "Conexion Establecida")
        {
            CCuentaContable CuentaContable = new CCuentaContable();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdTipoCuentaContable", pIdTipoCuentaContable);
            CuentaContable.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

            CTipoCuentaContable TipoCuentaContable = new CTipoCuentaContable();
            TipoCuentaContable.LlenaObjeto(pIdTipoCuentaContable, ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("IdTipoCuentaContable", pIdTipoCuentaContable);
            Modelo.Add("TipoCuentaContable", TipoCuentaContable.TipoCuentaContable);
            Modelo.Add("CuentaClienteComplemento", CuentaContable.CuentaContable);
            Modelo.Add("DescripcionCuentaClienteComplemento", CuentaContable.Descripcion);
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
    public static string AgregarCuentaContableComplementos(Dictionary<string, object> pCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CCuentaContable CuentaContable = new CCuentaContable();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdTipoCuentaContable", Convert.ToInt32(pCuentaContable["IdTipoCuentaContable"]));
            CuentaContable.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (CuentaContable.IdCuentaContable == 0)
            {
                CuentaContable.IdTipoCuentaContable = Convert.ToInt32(pCuentaContable["IdTipoCuentaContable"]);
                CuentaContable.CuentaContable = pCuentaContable["CuentaClienteComplemento"].ToString();
                CuentaContable.Descripcion = pCuentaContable["DescripcionCuentaClienteComplemento"].ToString();
                CuentaContable.Agregar(ConexionBaseDatos);
            }
            else
            {
                CuentaContable.IdTipoCuentaContable = Convert.ToInt32(pCuentaContable["IdTipoCuentaContable"]);
                CuentaContable.CuentaContable = pCuentaContable["CuentaClienteComplemento"].ToString();
                CuentaContable.Descripcion = pCuentaContable["DescripcionCuentaClienteComplemento"].ToString();
                CuentaContable.Editar(ConexionBaseDatos);
            }

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    //Validaciones
    private static string ValidarCuentaContable(CCuentaContable pCuentaContable, CConexion pConexion)
    {
        string errores = "";
        if (pCuentaContable.CuentaContable == "")
        { errores = errores + "<span>*</span> El campo de la cuenta contable esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pCuentaContable.IdDivision == 0)
        { errores = errores + "<span>*</span> El campo división esta vacío, favor de seleccionarlo.<br />"; }

        if (pCuentaContable.IdTipoCompra == 0)
        { errores = errores + "<span>*</span> El campo tipo de compra esta vacío, favor de seleccionarlo.<br />"; }

        if (pCuentaContable.IdCuentaContable == 0)
        {
            if (pCuentaContable.ExisteCuentaContable(pCuentaContable.CuentaContable, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta cuenta contable " + pCuentaContable.CuentaContable + " ya esta dada de alta.<br />";
            }
        }
        else
        {
            if (pCuentaContable.ExisteCuentaContableEditar(pCuentaContable.CuentaContable, pCuentaContable.IdCuentaContable, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta cuenta contable " + pCuentaContable.CuentaContable + " ya esta dada de alta.<br />";
            }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}