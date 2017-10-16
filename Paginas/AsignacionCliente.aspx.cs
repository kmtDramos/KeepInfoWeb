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

public partial class AsignacionCliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GeneraGridAsignacionCliente();
        GeneraGridCliente();
    }

    private void GeneraGridAsignacionCliente()
    {
        //GridUsuarios
        CJQGrid GridUsuarios = new CJQGrid();
        GridUsuarios.NombreTabla = "grdAsignacionCliente";
        GridUsuarios.CampoIdentificador = "IdUsuario";
        GridUsuarios.ColumnaOrdenacion = "Nombre";
        GridUsuarios.Metodo = "ObtenerUsuarios";
        GridUsuarios.TituloTabla = "Asignación cliente";

        //IdUsuario
        CJQColumn ColIdUsuario = new CJQColumn();
        ColIdUsuario.Nombre = "IdUsuario";
        ColIdUsuario.Oculto = "true";
        ColIdUsuario.Encabezado = "IdUsuario";
        ColIdUsuario.Buscador = "false";
        GridUsuarios.Columnas.Add(ColIdUsuario);

        //Nombre
        CJQColumn ColNombre = new CJQColumn();
        ColNombre.Nombre = "Nombre";
        ColNombre.Encabezado = "Usuario";
        ColNombre.Alineacion = "left";
        ColNombre.Ancho = "200";
        GridUsuarios.Columnas.Add(ColNombre);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Alineacion = "left";
        ColSucursal.Ancho = "80";
        ColSucursal.Buscador = "false";
        GridUsuarios.Columnas.Add(ColSucursal);

        //Empresa
        CJQColumn ColEmpresa = new CJQColumn();
        ColEmpresa.Nombre = "Empresa";
        ColEmpresa.Encabezado = "Empresa";
        ColEmpresa.Alineacion = "left";
        ColEmpresa.Ancho = "80";
        ColEmpresa.Buscador = "false";
        GridUsuarios.Columnas.Add(ColEmpresa);

        //Clientes
        CJQColumn ColClientes = new CJQColumn();
        ColClientes.Nombre = "Clientes";
        ColClientes.Encabezado = "Clientes Asignados";
        ColClientes.Ancho = "40";
        ColClientes.Buscador = "false";
        GridUsuarios.Columnas.Add(ColClientes);

        //SucursalAsignada
        CJQColumn ColSucursalAsignada = new CJQColumn();
        ColSucursalAsignada.Nombre = "AsignarCliente";
        ColSucursalAsignada.Encabezado = "Asignar cliente";
        ColSucursalAsignada.Etiquetado = "Imagen";
        ColSucursalAsignada.Imagen = "refrescar.png";
        ColSucursalAsignada.Estilo = "divImagenConsultar imgFormaAsignadacionCliente";
        ColSucursalAsignada.Buscador = "false";
        ColSucursalAsignada.Ordenable = "false";
        ColSucursalAsignada.Ancho = "40";
        GridUsuarios.Columnas.Add(ColSucursalAsignada);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAsignacionCliente", GridUsuarios.GeneraGrid(), true);
    }

    private void GeneraGridCliente()
    {
        //GridUsuarios
        CJQGrid GridClientes = new CJQGrid();
        GridClientes.NombreTabla = "grdCliente";
        GridClientes.CampoIdentificador = "IdCliente";
        GridClientes.ColumnaOrdenacion = "Nombre";
        GridClientes.Metodo = "ObtenerClientes";
        GridClientes.Ancho = 500;
        GridClientes.GenerarFuncionFiltro = false;
        GridClientes.TituloTabla = "Clientes asignados";

        //IdUsuario
        CJQColumn ColIdAsignacionCliente = new CJQColumn();
        ColIdAsignacionCliente.Nombre = "IdAsignacionCliente";
        ColIdAsignacionCliente.Oculto = "true";
        ColIdAsignacionCliente.Encabezado = "IdAsignacionCliente";
        ColIdAsignacionCliente.Buscador = "true";
        GridClientes.Columnas.Add(ColIdAsignacionCliente);

        //IdUsuario
        CJQColumn ColIdUsuario = new CJQColumn();
        ColIdUsuario.Nombre = "IdUsuario";
        ColIdUsuario.Oculto = "true";
        ColIdUsuario.Encabezado = "IdUsuario";
        ColIdUsuario.Buscador = "true";
        GridClientes.Columnas.Add(ColIdUsuario);

        //IdCliente
        CJQColumn ColIdCliente = new CJQColumn();
        ColIdCliente.Nombre = "IdCliente";
        ColIdCliente.Oculto = "true";
        ColIdCliente.Encabezado = "IdCliente";
        GridClientes.Columnas.Add(ColIdCliente);

        //Nombre
        CJQColumn ColNombre = new CJQColumn();
        ColNombre.Nombre = "RazonSocial";
        ColNombre.Encabezado = "Razón social";
        ColNombre.Alineacion = "left";
        ColNombre.Ancho = "350";
        GridClientes.Columnas.Add(ColNombre);

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
        GridClientes.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCliente";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "75";
        GridClientes.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCliente", GridClientes.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerUsuarios(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNombre)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAsignacionCliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNombre", SqlDbType.VarChar, 255).Value = Convert.ToString(pNombre);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = Convert.ToInt32(0);
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerClientes(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pIdUsuario, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAsignacionCliente_PorUsuario", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.VarChar, 255).Value = pIdUsuario;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdAsignacion, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CAsignacionCliente Asigancion = new CAsignacionCliente();
            Asigancion.IdAsignacionCliente = pIdAsignacion;
            Asigancion.Baja = pBaja;
            Asigancion.Eliminar(ConexionBaseDatos);
            respuesta = "0|ProductoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string BuscarNombre(string pNombre)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonUsuario = new CJson();
        jsonUsuario.StoredProcedure.CommandText = "sp_Cliente_ConsultarPorNombre";
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@pNombre", pNombre);
        string jsonUsuarioString = jsonUsuario.ObtenerJsonString(ConexionBaseDatos);
        return jsonUsuarioString;
    }

    [WebMethod]
    public static string BuscarNombreComercial(string pNombreComercial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonCliente = new CJson();
        jsonCliente.StoredProcedure.CommandText = "sp_Cliente_ConsultarPorNombreComercial";
        jsonCliente.StoredProcedure.Parameters.AddWithValue("@pNombreComercial", pNombreComercial);
        string jsonClienteString = jsonCliente.ObtenerJsonString(ConexionBaseDatos);
        return jsonClienteString;
    }

    //[WebMethod]
    //public static string BuscarCliente(Dictionary<string, object> pCliente)
    //{
    //    //Abrir Conexion
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //    CAsignacionCliente Cliente = new CAsignacionCliente();
    //    string jsonCliente = Cliente.ObtenerJsonCliente(pCliente["Cliente"].ToString(), ConexionBaseDatos).ToString();
    //    return jsonCliente;
    //}

    [WebMethod]
    public static string BuscarCliente(string pRazonSocial)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }



    [WebMethod]
    public static string AsignarCliente(Dictionary<string, object> pCliente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CAsignacionCliente Asigancion = new CAsignacionCliente();
            Asigancion.IdUsuario = Convert.ToInt32(pCliente["IdUsuario"]);
            Asigancion.IdCliente = Convert.ToInt32(pCliente["IdCliente"]);

            if (!Asigancion.VerificaAsignacionCliente(ConexionBaseDatos))
            {
                Asigancion = new CAsignacionCliente();
                Asigancion.IdUsuario = Convert.ToInt32(pCliente["IdUsuario"]);
                Asigancion.IdCliente = Convert.ToInt32(pCliente["IdCliente"]);
                Asigancion.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                Asigancion.IdUsuarioModifico = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                Asigancion.FechaAlta = DateTime.Now;
                Asigancion.FechaModifico = DateTime.Now;
                Asigancion.Baja = false;
                Asigancion.Agregar(ConexionBaseDatos);
                respuesta = "0";
            }
            else
            {
                respuesta = "1";
            }
        }
        else
        {
            respuesta = "2";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;

    }

    [WebMethod]
    public static string ObtenerCliente(int pIdCliente, int pIdUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAsignacionCliente AsignacionCliente = new CAsignacionCliente();

            Modelo.Add("Cliente", AsignacionCliente.ObtenerJsonClientePorId(pIdCliente, pIdUsuario, ConexionBaseDatos));
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
    public static string ObtenerFormaAsignacionCliente(int pIdUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("IdUsuario", pIdUsuario);

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
    public static string ObtenerFormaConsultarCliente(int pIdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCliente = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCliente" }, ConexionBaseDatos) == "")
        {
            puedeEditarCliente = 1;
        }
        oPermisos.Add("puedeEditarCliente", puedeEditarCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonCliente(Modelo, pIdCliente, ConexionBaseDatos);
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

}