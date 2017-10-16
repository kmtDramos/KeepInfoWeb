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

public partial class IVA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridIVA
        CJQGrid GridIVA = new CJQGrid();
        GridIVA.NombreTabla = "grdIVA";
        GridIVA.CampoIdentificador = "IdIVA";
        GridIVA.ColumnaOrdenacion = "IVA";
        GridIVA.Metodo = "ObtenerIVA";
        GridIVA.TituloTabla = "Catálogo de IVA";

        //IdIVA
        CJQColumn ColIdIVA = new CJQColumn();
        ColIdIVA.Nombre = "IdIVA";
        ColIdIVA.Oculto = "true";
        ColIdIVA.Encabezado = "IdIVA";
        ColIdIVA.Buscador = "false";
        GridIVA.Columnas.Add(ColIdIVA);

        //IVA
        CJQColumn ColIVA = new CJQColumn();
        ColIVA.Nombre = "IVA";
        ColIVA.Encabezado = "IVA";
        ColIVA.Ancho = "50";
        ColIVA.Alineacion = "right";
        ColIVA.Formato = "FormatoPorciento";
        GridIVA.Columnas.Add(ColIVA);

        //ClaveCuentaContable
        CJQColumn ColClaveCuentaContable = new CJQColumn();
        ColClaveCuentaContable.Nombre = "ClaveCuentaContable";
        ColClaveCuentaContable.Encabezado = "Clave de cuenta contable";
        ColClaveCuentaContable.Ancho = "500";
        ColClaveCuentaContable.Alineacion = "center";
        ColClaveCuentaContable.Buscador = "false";
        GridIVA.Columnas.Add(ColClaveCuentaContable);

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
        GridIVA.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarIVA";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridIVA.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdIVA", GridIVA.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerIVA(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIVA, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIVA", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIVA", SqlDbType.VarChar, 250).Value = Convert.ToString(pIVA);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarIVA(string pIVA)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonIVA = new CJson();
        jsonIVA.StoredProcedure.CommandText = "sp_IVA_Consultar_FiltroPorIVA";
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pIVA", pIVA);
        return jsonIVA.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarIVA(Dictionary<string, object> pIVA)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CIVA IVA = new CIVA();
            IVA.IVA = Convert.ToDecimal(pIVA["IVA"]);
            IVA.DescripcionIVA = Convert.ToString(pIVA["DescripcionIVA"]);
            IVA.ClaveCuentaContable = Convert.ToString(pIVA["ClaveCuentaContable"]);
            IVA.CuentaContableTrasladado = Convert.ToString(pIVA["CuentaContableTrasladado"]);
            IVA.CCAcreditablePagado = Convert.ToString(pIVA["CuentaContableAcreditablePagado"]);
            IVA.CCTrasladadoPagado = Convert.ToString(pIVA["CuentaContableTrasladadoPagado"]);
            string validacion = ValidarIVA(IVA, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                IVA.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaIVA(int pIdIVA)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarIVA = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarIVA" }, ConexionBaseDatos) == "")
        {
            puedeEditarIVA = 1;
        }
        oPermisos.Add("puedeEditarIVA", puedeEditarIVA);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CIVA.ObtenerIVA(Modelo, pIdIVA, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarIVA(int IdIVA)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarIVA = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarIVA" }, ConexionBaseDatos) == "")
        {
            puedeEditarIVA = 1;
        }
        oPermisos.Add("puedeEditarIVA", puedeEditarIVA);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CIVA.ObtenerIVA(Modelo, IdIVA, ConexionBaseDatos);
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
    public static string EditarIVA(Dictionary<string, object> pIVA)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CIVA IVA = new CIVA();
        IVA.IdIVA = Convert.ToInt32(pIVA["IdIVA"]); ;
        IVA.IVA = Convert.ToDecimal(pIVA["IVA"]);
        IVA.DescripcionIVA = Convert.ToString(pIVA["DescripcionIVA"]);
        IVA.ClaveCuentaContable = Convert.ToString(pIVA["ClaveCuentaContable"]);
        IVA.CuentaContableTrasladado = Convert.ToString(pIVA["CuentaContableTrasladado"]);
        IVA.CCAcreditablePagado = Convert.ToString(pIVA["CuentaContableAcreditablePagado"]);
        IVA.CCTrasladadoPagado = Convert.ToString(pIVA["CuentaContableTrasladadoPagado"]);

        string validacion = ValidarIVA(IVA, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            IVA.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdIVA, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CIVA IVA = new CIVA();
            IVA.IdIVA = pIdIVA;
            IVA.Baja = pBaja;
            IVA.Eliminar(ConexionBaseDatos);
            respuesta = "0|IVAEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarIVA(CIVA pIVA, CConexion pConexion)
    {
        string errores = "";
        if (pIVA.IVA == 0)
        { errores = errores + "<span>*</span> El campo IVA esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pIVA.DescripcionIVA == "")
        { errores = errores + "<span>*</span> El campo Descripción de IVA esta vacío, favor de capturarlo.<br/>"; }

        if (pIVA.ClaveCuentaContable == "")
        { errores = errores + "<span>*</span> El campo de la clave de cuenta contable esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pIVA.IdIVA == 0)
        {
            if (pIVA.ExisteClaveCuentaContable(pIVA.ClaveCuentaContable, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pIVA.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }
        else
        {
            if (pIVA.ExisteClaveCuentaContableEditar(pIVA.ClaveCuentaContable, pIVA.IdIVA, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pIVA.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}