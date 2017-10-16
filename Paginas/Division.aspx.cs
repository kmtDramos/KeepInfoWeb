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

public partial class Division : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridDivision
        CJQGrid GridDivision = new CJQGrid();
        GridDivision.NombreTabla = "grdDivision";
        GridDivision.CampoIdentificador = "IdDivision";
        GridDivision.ColumnaOrdenacion = "Division";
        GridDivision.Metodo = "ObtenerDivision";
        GridDivision.TituloTabla = "Catálogo de divisiones";

        //IdDivision
        CJQColumn ColIdDivision = new CJQColumn();
        ColIdDivision.Nombre = "IdDivision";
        ColIdDivision.Oculto = "true";
        ColIdDivision.Encabezado = "IdDivision";
        ColIdDivision.Buscador = "false";
        GridDivision.Columnas.Add(ColIdDivision);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "División";
        ColDivision.Ancho = "200";
        ColDivision.Alineacion = "left";
        GridDivision.Columnas.Add(ColDivision);

        //ClaveCuentaContable
        CJQColumn ColClaveCuentaContable = new CJQColumn();
        ColClaveCuentaContable.Nombre = "ClaveCuentaContable";
        ColClaveCuentaContable.Encabezado = "Clave de cuenta contable";
        ColClaveCuentaContable.Ancho = "200";
        ColClaveCuentaContable.Alineacion = "left";
        ColClaveCuentaContable.Buscador = "false";
        GridDivision.Columnas.Add(ColClaveCuentaContable);

		CJQColumn ColLimiteDescuento = new CJQColumn();
		ColLimiteDescuento.Nombre = "LimiteDescuento";
		ColLimiteDescuento.Encabezado = "Lim. Descuento";
		ColLimiteDescuento.Buscador = "false";
		ColLimiteDescuento.Ancho = "40";
		GridDivision.Columnas.Add(ColLimiteDescuento);

		CJQColumn ColLimiteMargen = new CJQColumn();
		ColLimiteMargen.Nombre = "LimiteMargen";
		ColLimiteMargen.Encabezado = "Lim. Margen";
		ColLimiteMargen.Buscador = "false";
		ColLimiteMargen.Ancho = "40";
		GridDivision.Columnas.Add(ColLimiteMargen);

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
        GridDivision.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarDivision";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridDivision.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDivision", GridDivision.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDivision(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pDivision, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDivision", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pDivision", SqlDbType.VarChar, 250).Value = Convert.ToString(pDivision);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarDivision(string pDivision)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonDivision = new CJson();
        jsonDivision.StoredProcedure.CommandText = "sp_Division_Consultar_FiltroPorDivision";
        jsonDivision.StoredProcedure.Parameters.AddWithValue("@pDivision", pDivision);
        return jsonDivision.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarDivision(Dictionary<string, object> pDivision)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CDivision Division = new CDivision();
            Division.Division = Convert.ToString(pDivision["Division"]);
            Division.ClaveCuentaContable = Convert.ToString(pDivision["ClaveCuentaContable"]);
            Division.EsVenta = Convert.ToBoolean(pDivision["EsVenta"]);
			Division.Descripcion = Convert.ToString(pDivision["Descripcion"]);
			string validacion = ValidarDivision(Division, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Division.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaDivision(int pIdDivision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDivision = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDivision" }, ConexionBaseDatos) == "")
        {
            puedeEditarDivision = 1;
        }
        oPermisos.Add("puedeEditarDivision", puedeEditarDivision);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CDivision.ObtenerDivision(Modelo, pIdDivision, ConexionBaseDatos);
			CDivision Division = new CDivision();
			Division.LlenaObjeto(pIdDivision, ConexionBaseDatos);
			Modelo.Add("LimiteDescuento", Division.LimiteDescuento);
			Modelo.Add("LimiteMargen", Division.LimiteMargen);
			Modelo.Add(new JProperty("Descripcion", Division.Descripcion));
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
    public static string ObtenerFormaEditarDivision(int IdDivision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDivision = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDivision" }, ConexionBaseDatos) == "")
        {
            puedeEditarDivision = 1;
        }
        oPermisos.Add("puedeEditarDivision", puedeEditarDivision);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CDivision.ObtenerDivision(Modelo, IdDivision, ConexionBaseDatos);
			CDivision Division = new CDivision();
			Division.LlenaObjeto(IdDivision, ConexionBaseDatos);
			Modelo.Add("LimiteDescuento", Division.LimiteDescuento);
			Modelo.Add("LimiteMargen", Division.LimiteMargen);
			Modelo.Add(new JProperty("Descripcion", Division.Descripcion));
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
    public static string EditarDivision(Dictionary<string, object> pDivision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CDivision Division = new CDivision();
        Division.IdDivision = Convert.ToInt32(pDivision["IdDivision"]); ;
        Division.Division = Convert.ToString(pDivision["Division"]);
        Division.ClaveCuentaContable = Convert.ToString(pDivision["ClaveCuentaContable"]);
        Division.EsVenta = Convert.ToBoolean(pDivision["EsVenta"]);
		Division.Descripcion = Convert.ToString(pDivision["Descripcion"]);
		Division.LimiteDescuento = Convert.ToDecimal(pDivision["LimiteDescuento"]);
		Division.LimiteMargen = Convert.ToDecimal(pDivision["LimiteMargen"]);

        string validacion = ValidarDivision(Division, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Division.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdDivision, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDivision Division = new CDivision();
            Division.IdDivision = pIdDivision;
            Division.Baja = pBaja;
            Division.Eliminar(ConexionBaseDatos);
            respuesta = "0|DivisionEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarDivision(CDivision pDivision, CConexion pConexion)
    {
        string errores = "";
        if (pDivision.Division == "")
        { errores = errores + "<span>*</span> El campo división esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pDivision.ClaveCuentaContable == "")
        { errores = errores + "<span>*</span> El campo de la clave de cuenta contable esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pDivision.IdDivision == 0)
        {
            if (pDivision.ExisteClaveCuentaContable(pDivision.ClaveCuentaContable, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pDivision.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }
        else
        {
            if (pDivision.ExisteClaveCuentaContableEditar(pDivision.ClaveCuentaContable, pDivision.IdDivision, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pDivision.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

}