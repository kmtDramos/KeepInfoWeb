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

public partial class CuentaBancaria : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //GridCuentaBancaria
        CJQGrid GridCuentaBancaria = new CJQGrid();
        GridCuentaBancaria.NombreTabla = "grdCuentaBancaria";
        GridCuentaBancaria.CampoIdentificador = "IdCuentaBancaria";
        GridCuentaBancaria.ColumnaOrdenacion = "CuentaBancaria";
        GridCuentaBancaria.Metodo = "ObtenerCuentaBancaria";
        GridCuentaBancaria.TituloTabla = "Cuenta bancaria";

        //IdCuentaBancaria
        CJQColumn ColIdCuentaBancaria = new CJQColumn();
        ColIdCuentaBancaria.Nombre = "IdCuentaBancaria";
        ColIdCuentaBancaria.Oculto = "true";
        ColIdCuentaBancaria.Encabezado = "IdCuentaBancaria";
        ColIdCuentaBancaria.Buscador = "false";
        GridCuentaBancaria.Columnas.Add(ColIdCuentaBancaria);

        //Banco
        CJQColumn ColBanco = new CJQColumn();
        ColBanco.Nombre = "Banco";
        ColBanco.Encabezado = "Banco";
        ColBanco.Buscador = "false";
        ColBanco.Ancho = "80";
        ColBanco.Alineacion = "right";
        GridCuentaBancaria.Columnas.Add(ColBanco);

        //CuentaBancaria
        CJQColumn ColCuentaBancaria = new CJQColumn();
        ColCuentaBancaria.Nombre = "CuentaBancaria";
        ColCuentaBancaria.Encabezado = "Cuenta bancaria";
        ColCuentaBancaria.Buscador = "true";
        ColCuentaBancaria.Ancho = "80";
        ColCuentaBancaria.Alineacion = "right";
        GridCuentaBancaria.Columnas.Add(ColCuentaBancaria);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Ancho = "80";
        ColDescripcion.Alineacion = "right";
        GridCuentaBancaria.Columnas.Add(ColDescripcion);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Ancho = "80";
        ColTipoMoneda.Alineacion = "right";
        GridCuentaBancaria.Columnas.Add(ColTipoMoneda);

        //CuentaBancariaAsignada
        CJQColumn ColCuentaBancariaAsignada = new CJQColumn();
        ColCuentaBancariaAsignada.Nombre = "CuentaBancariaAsignada";
        ColCuentaBancariaAsignada.Encabezado = "Cuenta bancaria";
        ColCuentaBancariaAsignada.Etiquetado = "Imagen";
        ColCuentaBancariaAsignada.Imagen = "refrescar.png";
        ColCuentaBancariaAsignada.Estilo = "divImagenConsultar imgFormaCuentaBancariaAsignada";
        ColCuentaBancariaAsignada.Buscador = "false";
        ColCuentaBancariaAsignada.Ordenable = "false";
        ColCuentaBancariaAsignada.Ancho = "50";
        GridCuentaBancaria.Columnas.Add(ColCuentaBancariaAsignada);

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
        GridCuentaBancaria.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCuentaBancaria";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCuentaBancaria.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCuentaBancaria", GridCuentaBancaria.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCuentaBancaria(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCuentaBancaria, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentaBancaria", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("CuentaBancaria", SqlDbType.VarChar, 250).Value = Convert.ToString(pCuentaBancaria);
        Stored.Parameters.Add("Baja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarCuentaBancaria(string pCuentaBancaria)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonCuentaBancaria = new CJson();
        jsonCuentaBancaria.StoredProcedure.CommandText = "sp_CuentaBancaria_Consultar_FiltroPorCuentaBancaria";
        jsonCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", pCuentaBancaria);
        return jsonCuentaBancaria.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarCuentaBancaria(Dictionary<string, object> pCuentaBancaria)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        CuentaBancaria.Saldo = Convert.ToDecimal(pCuentaBancaria["Saldo"]);
        CuentaBancaria.CuentaBancaria = Convert.ToString(pCuentaBancaria["CuentaBancaria"]);
        CuentaBancaria.Descripcion = Convert.ToString(pCuentaBancaria["Descripcion"]);
        CuentaBancaria.IdBanco = Convert.ToInt32(pCuentaBancaria["IdBanco"]);
        CuentaBancaria.IdTipoMoneda = Convert.ToInt32(pCuentaBancaria["IdTipoMoneda"]);
        CuentaBancaria.CuentaContable = Convert.ToString(pCuentaBancaria["CuentaContable"]);
        CuentaBancaria.CuentaContableComplemento = Convert.ToString(pCuentaBancaria["CuentaContableComplemento"]);

        Dictionary<string, object> ParametrosCB = new Dictionary<string, object>();
        ParametrosCB.Add("CuentaBancaria", CuentaBancaria.CuentaBancaria);
        JObject oRespuesta = new JObject();
        if (CuentaBancaria.RevisarExisteRegistro(ParametrosCB, ConexionBaseDatos) == false)
        {
            CuentaBancaria.Agregar(ConexionBaseDatos);
            respuesta = "CuentaBancariaAgregada";
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "Ya existe la cuenta bancaria"));
        }

        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string ObtenerFormaAgregarCuentaBancaria()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarCuentaBancaria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarCuentaBancaria" }, ConexionBaseDatos) == "")
        {
            puedeAgregarCuentaBancaria = 1;
        }
        oPermisos.Add("puedeAgregarCuentaBancaria", puedeAgregarCuentaBancaria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(ConexionBaseDatos));
            Modelo.Add("TipoBancos", CBanco.ObtenerJsonBanco(ConexionBaseDatos));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarCuentaBancaria(int pIdCuentaBancaria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentaBancaria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentaBancaria" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentaBancaria = 1;
        }
        oPermisos.Add("puedeEditarCuentaBancaria", puedeEditarCuentaBancaria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CCuentaBancaria.ObtenerCuentaBancaria(Modelo, pIdCuentaBancaria, ConexionBaseDatos);
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
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaCuentaBancariaAsignadaUsuario(int pIdCuentaBancaria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        CCuentaBancaria CuentaBancaria = new CCuentaBancaria(); 

        if (respuesta == "Conexion Establecida")
        {
            CuentaBancaria.LlenaObjeto(pIdCuentaBancaria, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add("IdCuentaBancaria", pIdCuentaBancaria);
            Modelo.Add("UsuariosDisponibles", CuentaBancaria.ObtenerJsonUsuariosDisponibles(pIdCuentaBancaria, ConexionBaseDatos));
            Modelo.Add("UsuariosAsignados", CuentaBancaria.ObtenerJsonUsuariosAsignados(pIdCuentaBancaria, ConexionBaseDatos));
            Modelo.Add("CuentaBancaria", CuentaBancaria.CuentaBancaria);
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
    public static string ObtenerFormaCuentaBancariaAsignadaUsuarioCuentaContable(int pIdCuentaBancaria, int IdUsuario, string Usuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();

        if (respuesta == "Conexion Establecida")
        {
            CuentaBancaria.LlenaObjeto(pIdCuentaBancaria, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add("UsuariosAsignados", CuentaBancaria.ObtenerJsonUsuariosAsignados(pIdCuentaBancaria, ConexionBaseDatos));
            Modelo.Add("Usuario", Usuario);
            Modelo.Add("IdUsuario", IdUsuario);
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
    public static string ObtenerFormaAgregarTodosUsuarios(Dictionary<string, object> pUsuariosEnrolar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add("Usuarios", CuentaBancaria.ObtenerJsonUsuariosTodos(Convert.ToInt32(pUsuariosEnrolar["IdCuentaBancaria"]), ConexionBaseDatos));
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
    public static string ObtenerFormaEditarCuentaBancaria(int IdCuentaBancaria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentaBancaria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentaBancaria" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentaBancaria = 1;
        }
        oPermisos.Add("puedeEditarCuentaBancaria", puedeEditarCuentaBancaria);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CCuentaBancaria.ObtenerCuentaBancaria(Modelo, IdCuentaBancaria, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("TipoBancos", CBanco.ObtenerJsonBanco(Convert.ToInt32(Modelo["IdBanco"].ToString()), ConexionBaseDatos));
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
    public static string AgregarCuentaBancariaUsuario(Dictionary<string, object> pUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            try
            {
                JObject Modelo = new JObject();
                CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
                UsuarioCuentaBancaria.IdCuentaBancaria = Convert.ToInt32(pUsuario["IdCuentaBancaria"]);
                UsuarioCuentaBancaria.BajaCuentaBancariaUsuario(ConexionBaseDatos);

                foreach (Dictionary<string, object> oUsuario in (Array)pUsuario["Usuarios"])
                {
                    UsuarioCuentaBancaria.IdUsuario = Convert.ToInt32(oUsuario["IdUsuario"]);
                    UsuarioCuentaBancaria.PuedeVerSaldo = Convert.ToBoolean(oUsuario["PuedeVerSaldo"]);
                    UsuarioCuentaBancaria.EnrolarCuentaBancariaUsuario(ConexionBaseDatos);
                }

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            catch (Exception ex)
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", ex.Message));
            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarCuentaBancaria(Dictionary<string, object> pCuentaBancaria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        CuentaBancaria.LlenaObjeto(Convert.ToInt32(pCuentaBancaria["IdCuentaBancaria"]), ConexionBaseDatos);
        CuentaBancaria.IdCuentaBancaria = Convert.ToInt32(pCuentaBancaria["IdCuentaBancaria"]);
        CuentaBancaria.Saldo = Convert.ToDecimal(pCuentaBancaria["Saldo"]);
        CuentaBancaria.CuentaBancaria = Convert.ToString(pCuentaBancaria["CuentaBancaria"]);
        CuentaBancaria.Descripcion = Convert.ToString(pCuentaBancaria["Descripcion"]);
        CuentaBancaria.IdBanco = Convert.ToInt32(pCuentaBancaria["IdBanco"]);
        CuentaBancaria.IdTipoMoneda = Convert.ToInt32(pCuentaBancaria["IdTipoMoneda"]);
        CuentaBancaria.CuentaContable = Convert.ToString(pCuentaBancaria["CuentaContable"]);
        CuentaBancaria.CuentaContableComplemento = Convert.ToString(pCuentaBancaria["CuentaContableComplemento"]);

        string validacion = ValidarCuentaBancaria(CuentaBancaria, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CuentaBancaria.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdCuentaBancaria, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
            CuentaBancaria.IdCuentaBancaria = pIdCuentaBancaria;
            CuentaBancaria.Baja = pBaja;
            CuentaBancaria.Eliminar(ConexionBaseDatos);
            respuesta = "0|CuentaBancariaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarCuentaBancaria(CCuentaBancaria pCuentaBancaria, CConexion pConexion)
    {
        string errores = "";

        if (pCuentaBancaria.CuentaBancaria == "")
        { errores = errores + "<span>*</span> La cuenta bancaria esta vacía, favor de capturarla.<br />"; }

        if (pCuentaBancaria.IdBanco == 0)
        { errores = errores + "<span>*</span> El banco esta vacio, favor de capturarlo.<br />"; }

        if (pCuentaBancaria.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> La moneda esta vacia, favor de capturarla.<br />"; }

        if (pCuentaBancaria.Descripcion == "")
        { errores = errores + "<span>*</span> La descripción esta vacia, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}