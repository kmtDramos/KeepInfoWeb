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

public partial class Usuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridUsuarios
        CJQGrid GridUsuarios = new CJQGrid();
        GridUsuarios.NombreTabla = "grdUsuarios";
        GridUsuarios.CampoIdentificador = "IdUsuario";
        GridUsuarios.ColumnaOrdenacion = "Nombre";
        GridUsuarios.Metodo = "ObtenerUsuarios";
		GridUsuarios.GenerarFuncionTerminado = true;
        GridUsuarios.TituloTabla = "Catálogo de Usuarios";

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
        ColNombre.Encabezado = "Nombre";
        ColNombre.Ancho = "120";
        GridUsuarios.Columnas.Add(ColNombre);

        //ApellidoPaterno
        CJQColumn ColApellidoPaterno = new CJQColumn();
        ColApellidoPaterno.Nombre = "ApellidoPaterno";
        ColApellidoPaterno.Encabezado = "Apellido Paterno";
        ColApellidoPaterno.Ancho = "120";
        GridUsuarios.Columnas.Add(ColApellidoPaterno);

        //ApellidoMaterno
        CJQColumn ColApellidoMaterno = new CJQColumn();
        ColApellidoMaterno.Nombre = "ApellidoMaterno";
        ColApellidoMaterno.Encabezado = "ApellidoMaterno";
        ColApellidoMaterno.Ancho = "120";
        GridUsuarios.Columnas.Add(ColApellidoMaterno);

        //FechaNacimiento
        CJQColumn ColFechaNacimiento = new CJQColumn();
        ColFechaNacimiento.Nombre = "FechaNacimiento";
        ColFechaNacimiento.Encabezado = "Fecha de Nacimiento";
        ColFechaNacimiento.TipoBuscador = "Fecha";
        ColFechaNacimiento.Ancho = "100";
        GridUsuarios.Columnas.Add(ColFechaNacimiento);

        //Usuario
        CJQColumn ColUsuario = new CJQColumn();
        ColUsuario.Nombre = "Usuario";
        ColUsuario.Encabezado = "Usuario";
        ColUsuario.Ancho = "100";
        GridUsuarios.Columnas.Add(ColUsuario);

        //Perfil Asignado
        CJQColumn ColPerfil = new CJQColumn();
        ColPerfil.Nombre = "Perfil";
        ColPerfil.Encabezado = "Perfil";
        ColPerfil.Ancho = "100";
        ColPerfil.Buscador = "true";
        ColPerfil.TipoBuscador = "Combo";
        ColPerfil.StoredProcedure.CommandText = "spc_UsuarioPerfil_Consulta";
        GridUsuarios.Columnas.Add(ColPerfil);

        //Sucursal Actual
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "100";
        ColSucursal.Buscador = "true";
        ColSucursal.TipoBuscador = "Combo";
        ColSucursal.StoredProcedure.CommandText = "sp_Usuario_SucursalPrincipal";
        GridUsuarios.Columnas.Add(ColSucursal);

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
        GridUsuarios.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarUsuario";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridUsuarios.Columnas.Add(ColConsultar);

		// Metas
		CJQColumn ColMetas = new CJQColumn();
		ColMetas.Nombre = "Metas";
		ColMetas.Encabezado = "Metas";
		ColMetas.Buscador = "false";
		ColMetas.Ordenable = "false";
		ColMetas.Ancho = "25";
		GridUsuarios.Columnas.Add(ColMetas);

        //SucursalAsignada
        CJQColumn ColSucursalAsignada = new CJQColumn();
        ColSucursalAsignada.Nombre = "SucursalAsignada";
        ColSucursalAsignada.Encabezado = "Sucursales";
        ColSucursalAsignada.Etiquetado = "Imagen";
        ColSucursalAsignada.Imagen = "refrescar.png";
        ColSucursalAsignada.Estilo = "divImagenConsultar imgFormaSucursalAsignada";
        ColSucursalAsignada.Buscador = "false";
        ColSucursalAsignada.Ordenable = "false";
        ColSucursalAsignada.Ancho = "50";
        GridUsuarios.Columnas.Add(ColSucursalAsignada);

        ClientScript.RegisterStartupScript(this.GetType(), "grdUsuarios", GridUsuarios.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerUsuarios(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNombre, string pApellidoPaterno, string pApellidoMaterno, string pFechaNacimiento, string pUsuario, string pPerfil, int pSucursal, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdUsuarios", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNombre", SqlDbType.VarChar, 50).Value = Convert.ToString(pNombre);
        Stored.Parameters.Add("pApellidoPaterno", SqlDbType.VarChar, 50).Value = Convert.ToString(pApellidoPaterno);
        Stored.Parameters.Add("pApellidoMaterno", SqlDbType.VarChar, 50).Value = Convert.ToString(pApellidoMaterno);
        Stored.Parameters.Add("pFechaNacimiento", SqlDbType.VarChar, 8).Value = pFechaNacimiento;
        Stored.Parameters.Add("pUsuario", SqlDbType.VarChar, 15).Value = Convert.ToString(pUsuario);
        if (pPerfil == "")
        {
            pPerfil = "-1";
        }
        Stored.Parameters.Add("pIdPerfil", SqlDbType.Int).Value = Convert.ToInt32(pPerfil);
        Stored.Parameters.Add("pSucursal", SqlDbType.Int).Value = Convert.ToInt32(pSucursal);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    //Metodos Ajax
    [WebMethod]
    public static string BuscarNombre(string pNombre)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonUsuario = new CJson();
        jsonUsuario.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@Opcion", 10);
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@pNombre", pNombre);
        return jsonUsuario.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarApellidoPaterno(string pApellidoPaterno)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonUsuario = new CJson();
        jsonUsuario.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@Opcion", 11);
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@pApellidoPaterno", pApellidoPaterno);
        return jsonUsuario.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarApellidoMaterno(string pApellidoMaterno)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonUsuario = new CJson();
        jsonUsuario.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@Opcion", 12);
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@pApellidoMaterno", pApellidoMaterno);
        return jsonUsuario.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarUsuario(string pUsuario)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonUsuario = new CJson();
        jsonUsuario.StoredProcedure.CommandText = "sp_Usuario_Consulta";
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@Opcion", 13);
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@pUsuario", pUsuario);
        return jsonUsuario.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarUsuario(string pNombre, string pApellidoPaterno, string pApellidoMaterno, string pFechaNacimiento, string pUsuario, string pContrasena, string pCorreo, int pIdPerfil, bool pEsAgente, decimal pAlcance1, decimal pAlcance2, decimal pMeta, int pClientesNuevos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string validacion = ValidaUsuario(0, pNombre, pApellidoPaterno, pApellidoMaterno, pFechaNacimiento, pUsuario, pContrasena, pCorreo, pIdPerfil, ConexionBaseDatos);
            if (validacion != "")
            {
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "1|" + validacion;
            }
            else
            {
                CUsuario Usuario = new CUsuario();
                Usuario.Nombre = pNombre;
                Usuario.ApellidoPaterno = pApellidoPaterno;
                Usuario.ApellidoMaterno = pApellidoMaterno;
                Usuario.FechaNacimiento = Convert.ToDateTime(pFechaNacimiento);
                Usuario.Usuario = pUsuario;
                Usuario.Contrasena = pContrasena;
                Usuario.Correo = pCorreo;
                Usuario.IdPerfil = pIdPerfil;
                Usuario.FechaIngreso = DateTime.Now;
                Usuario.EsAgente = pEsAgente;
                Usuario.Alcance1 = pAlcance1;
                Usuario.Alcance2 = pAlcance2;
                Usuario.Meta = pMeta;
                Usuario.ClientesNuevos = pClientesNuevos;
                Usuario.Agregar(ConexionBaseDatos);
                respuesta = "UsuarioAgregado";

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "0|" + respuesta;
            }
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EditarUsuario(int pIdUsuario, string pNombre, string pApellidoPaterno, string pApellidoMaterno, string pFechaNacimiento, string pUsuario, string pContrasena, string pCorreo, int pIdPerfil, bool pEsAgente, bool pEsVendedor, decimal pAlcance1, decimal pAlcance2, decimal pMeta, int pClientesNuevos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string validacion = ValidaUsuario(pIdUsuario, pNombre, pApellidoPaterno, pApellidoMaterno, pFechaNacimiento, pUsuario, pContrasena, pCorreo, pIdPerfil, ConexionBaseDatos);
            if (validacion != "")
            {
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "1|" + validacion;
            }
            else
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(pIdUsuario, ConexionBaseDatos);
                Usuario.Nombre = pNombre;
                Usuario.ApellidoPaterno = pApellidoPaterno;
                Usuario.ApellidoMaterno = pApellidoMaterno;
                Usuario.FechaNacimiento = Convert.ToDateTime(pFechaNacimiento);
                Usuario.Usuario = pUsuario;
                Usuario.Contrasena = Usuario.Contrasena;
                Usuario.Correo = pCorreo;
                Usuario.IdPerfil = pIdPerfil;
                Usuario.EsAgente = pEsAgente;
				Usuario.EsVendedor = pEsVendedor;
                Usuario.Alcance1 = pAlcance1;
                Usuario.Alcance2 = pAlcance2;
                Usuario.Meta = pMeta;
                Usuario.ClientesNuevos = pClientesNuevos;
                Usuario.Editar(ConexionBaseDatos);
                respuesta = "UsuarioEditado";

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "0|" + respuesta;
            }
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EliminarUsuario(int pIdUsuario)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.IdUsuario = pIdUsuario;
            Usuario.Eliminar(ConexionBaseDatos);
            respuesta = "0|UsuarioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdUsuario, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.IdUsuario = pIdUsuario;
            Usuario.Baja = pBaja;
            Usuario.Eliminar(ConexionBaseDatos);
            respuesta = "0|UsuarioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string EditarDatosUsuario(string pNombre, string pApellidoPaterno, string pApellidoMaterno, string pUsuario, string pFechaNacimiento, string pCorreo)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            string validacion = ValidaDatosUsuario(idUsuario, pNombre, pApellidoPaterno, pApellidoMaterno, pUsuario, pFechaNacimiento, pCorreo, ConexionBaseDatos);
            if (validacion != "")
            {
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "1|" + validacion;
            }
            else
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(idUsuario, ConexionBaseDatos);
                Usuario.Nombre = pNombre;
                Usuario.ApellidoPaterno = pApellidoPaterno;
                Usuario.ApellidoMaterno = pApellidoMaterno;
                Usuario.FechaNacimiento = Convert.ToDateTime(pFechaNacimiento);
                Usuario.Usuario = pUsuario;
				Usuario.Contrasena = Usuario.Contrasena;
                Usuario.Correo = pCorreo;
                Usuario.Editar(ConexionBaseDatos);
                respuesta = Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno;

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "0|" + respuesta;
            }
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string CambiarContrasena(string pActual, string pNueva, string pConfirmar)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            string validacion = ValidaCambioContrasena(idUsuario, pActual, pNueva, pConfirmar, ConexionBaseDatos);
            if (validacion != "")
            {
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "1|" + validacion;
            }
            else
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(idUsuario, ConexionBaseDatos);
                Usuario.Contrasena = pNueva;
                Usuario.CambiarContrasena(ConexionBaseDatos);

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "0|" + respuesta;
            }
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EditarContrasena(int pIdUsuario, string pContrasenaAdministrador, string pContrasenaNueva)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            string validacion = ValidaEditarContrasena(idUsuario, pContrasenaAdministrador, ConexionBaseDatos);
            if (validacion != "")
            {
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "1|" + validacion;
            }
            else
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(pIdUsuario, ConexionBaseDatos);
                Usuario.Contrasena = pContrasenaNueva;
                Usuario.CambiarContrasena(ConexionBaseDatos);

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "0|" + respuesta;
            }
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFormaSucursalAsignada(int pIdUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject o = new JObject();
        JObject Modelo = new JObject();
        o.Add(new JProperty("Error", 0));

        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
        Dictionary<string, object> ParametrosAsignada = new Dictionary<string, object>();
        ParametrosAsignada.Add("Opcion", 3);
        ParametrosAsignada.Add("IdUsuario", pIdUsuario);
        ParametrosAsignada.Add("Baja", "false");
        JArray JSucursalesAsignadas = new JArray();

        foreach (CSucursalAsignada oSucursalAsignada in SucursalAsignada.LlenaObjetosFiltros(ParametrosAsignada, ConexionBaseDatos))
        {
            JObject JSucursal = new JObject();

            JSucursal.Add("IdSucursalAsignada", oSucursalAsignada.IdSucursalAsignada);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(oSucursalAsignada.IdSucursal, ConexionBaseDatos);

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

            JSucursal.Add("IdSucursal", Sucursal.IdSucursal);
            JSucursal.Add("Sucursal", Sucursal.Sucursal + " (" + Empresa.Empresa + ")");

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(pIdUsuario, ConexionBaseDatos);
            JSucursal.Add("IdSucursalPredeterminada", Usuario.IdSucursalPredeterminada);

            CPerfil Perfil = new CPerfil();
            JSucursalesAsignadas.Add(JSucursal);
            JArray JPerfiles = new JArray();

            Dictionary<string, object> ParametrosSA = new Dictionary<string, object>();
            ParametrosSA.Add("EsPerfilSucursal", true);

            foreach (CPerfil oPerfil in Perfil.LlenaObjetosFiltros(ParametrosSA, ConexionBaseDatos))
            {
                JObject JPerfil = new JObject();
                JPerfil.Add("Perfil", oPerfil.Perfil);
                JPerfil.Add("IdPerfil", oPerfil.IdPerfil);
                if (oPerfil.IdPerfil == oSucursalAsignada.IdPerfil)
                {
                    JPerfil.Add("Selected", 1);
                }
                else
                {
                    JPerfil.Add("Selected", 0);
                }
                JPerfiles.Add(JPerfil);
            }
            JSucursal.Add("Perfiles", JPerfiles);
        }

        JArray JSucursalesDisponibles = new JArray();
        foreach (CSucursal Sucursal in SucursalAsignada.ObtenerSucursalesDisponibles(pIdUsuario, ConexionBaseDatos))
        {
            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

            JObject JSucursal = new JObject();
            JSucursal.Add("IdSucursal", Sucursal.IdSucursal);
            JSucursal.Add("Sucursal", Sucursal.Sucursal + " (" + Empresa.Empresa + ")");
            JSucursalesDisponibles.Add(JSucursal);
        }

        Modelo.Add("IdUsuario", pIdUsuario);
        Modelo.Add("SucursalesDisponibles", JSucursalesDisponibles);
        Modelo.Add("SucursalesAsignadas", JSucursalesAsignadas);

        o.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return o.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarEnrolamientoSucursal(int pIdSucursal, int pIdUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add("IdSucursal", Sucursal.IdSucursal);
            Modelo.Add("Sucursal", Sucursal.Sucursal);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(pIdUsuario, ConexionBaseDatos);
            Modelo.Add("IdSucursalPredeterminada", Usuario.IdSucursalPredeterminada);


            CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
            Dictionary<string, object> ParametrosSA = new Dictionary<string, object>();
            ParametrosSA.Add("IdSucursal", pIdSucursal);
            ParametrosSA.Add("IdUsuario", pIdUsuario);

            JArray JPerfiles = new JArray();
            foreach (CSucursalAsignada oSucursalAsignada in SucursalAsignada.LlenaObjetosFiltros(ParametrosSA, ConexionBaseDatos))
            {
                CPerfil Perfil = new CPerfil();

                Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
                ParametrosP.Add("EsPerfilSucursal", true);

                foreach (CPerfil oPerfil in Perfil.LlenaObjetosFiltros(ParametrosP, ConexionBaseDatos))
                {
                    JObject JPerfil = new JObject();
                    JPerfil.Add("Perfil", oPerfil.Perfil);
                    JPerfil.Add("IdPerfil", oPerfil.IdPerfil);
                    if (oPerfil.IdPerfil == oSucursalAsignada.IdPerfil)
                    {
                        JPerfil.Add("Selected", 1);
                    }
                    else
                    {
                        JPerfil.Add("Selected", 0);
                    }
                    JPerfiles.Add(JPerfil);
                }
            }

            if (JPerfiles.Count == 0)
            {

                CPerfil Perfil = new CPerfil();
                Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
                ParametrosP.Add("EsPerfilSucursal", true);

                foreach (CPerfil oPerfil in Perfil.LlenaObjetosFiltros(ParametrosP, ConexionBaseDatos))
                {
                    JObject JPerfil = new JObject();
                    JPerfil.Add("Perfil", oPerfil.Perfil);
                    JPerfil.Add("IdPerfil", oPerfil.IdPerfil);
                    JPerfiles.Add(JPerfil);
                }
            }

            Modelo.Add("Perfiles", JPerfiles);
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
    public static string EditarSucursalEnrolada(Dictionary<string, object> pSucursalesEnrolar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();

        //Todo a baja
        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
        SucursalAsignada.IdUsuario = Convert.ToInt32(pSucursalesEnrolar["IdUsuario"]);
        SucursalAsignada.Baja = true;
        SucursalAsignada.EditarCampoBaja(ConexionBaseDatos);

        CUsuario Usuario = new CUsuario();
        Usuario.IdUsuario = Convert.ToInt32(pSucursalesEnrolar["IdUsuario"]);
        Usuario.IdSucursalPredeterminada = Convert.ToInt32(pSucursalesEnrolar["IdSucursalPredeterminada"]);
        Usuario.EditarSucursalPredeterminada(ConexionBaseDatos);
        respuesta = "SucursalPredeterminadaEditada";

        foreach (Dictionary<string, object> oEnrolamiento in (Array)pSucursalesEnrolar["Sucursales"])
        {


            Dictionary<string, object> ParametrosSA = new Dictionary<string, object>();
            ParametrosSA.Add("IdUsuario", Convert.ToInt32(oEnrolamiento["IdUsuario"]));
            ParametrosSA.Add("IdSucursal", Convert.ToInt32(oEnrolamiento["IdSucursal"]));

            SucursalAsignada.IdUsuario = Convert.ToInt32(oEnrolamiento["IdUsuario"]);
            SucursalAsignada.IdSucursal = Convert.ToInt32(oEnrolamiento["IdSucursal"]);
            SucursalAsignada.IdPerfil = Convert.ToInt32(oEnrolamiento["IdPerfil"]);
            SucursalAsignada.Baja = false;

            if (SucursalAsignada.RevisarExisteRegistro(ParametrosSA, ConexionBaseDatos) == true)
            {
                SucursalAsignada.EditarDatos(ConexionBaseDatos);
                respuesta = "SucursalAsignadaEditado";
            }
            else
            {
                SucursalAsignada.Agregar(ConexionBaseDatos);
                respuesta = "SucursalAsignadaAgregada";
            }
        }

        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string ObtenerFormaAgregarTodasSucursales(Dictionary<string, object> pSucursalesEnrolar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            JArray JSucursales = new JArray();
            foreach (Dictionary<string, object> oEnrolamiento in (Array)pSucursalesEnrolar["Sucursales"])
            {
                Dictionary<string, object> ParametrosSA = new Dictionary<string, object>();
                ParametrosSA.Add("IdSucursal", Convert.ToInt32(oEnrolamiento["IdSucursal"]));

                CSucursal Sucursal = new CSucursal();
                Sucursal.IdSucursal = Convert.ToInt32(oEnrolamiento["IdSucursal"]);

                JObject JSucursal = new JObject();
                Sucursal.LlenaObjeto(Sucursal.IdSucursal, ConexionBaseDatos);
                JSucursal.Add("IdSucursal", Sucursal.IdSucursal);
                JSucursal.Add("Sucursal", Sucursal.Sucursal);
                JSucursales.Add(JSucursal);
            }


            CPerfil Perfil = new CPerfil();
            JArray JPerfiles = new JArray();
            Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
            ParametrosP.Add("EsPerfilSucursal", true);

            foreach (CPerfil oPerfil in Perfil.LlenaObjetosFiltros(ParametrosP, ConexionBaseDatos))
            {
                JObject JPerfil = new JObject();
                JPerfil.Add(new JProperty("IdPerfil", oPerfil.IdPerfil));
                JPerfil.Add(new JProperty("Perfil", oPerfil.Perfil));
                JPerfiles.Add(JPerfil);
            }


            Modelo.Add(new JProperty("Perfiles", JPerfiles));
            Modelo.Add(new JProperty("Sucursales", JSucursales));
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
    public static string ObtenerFormaEliminarTodasSucursales(Dictionary<string, object> pSucursalesEnrolar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            JArray JSucursales = new JArray();
            foreach (Dictionary<string, object> oEnrolamiento in (Array)pSucursalesEnrolar["Sucursales"])
            {
                Dictionary<string, object> ParametrosSA = new Dictionary<string, object>();
                ParametrosSA.Add("IdSucursal", Convert.ToInt32(oEnrolamiento["IdSucursal"]));

                CSucursal Sucursal = new CSucursal();
                Sucursal.IdSucursal = Convert.ToInt32(oEnrolamiento["IdSucursal"]);

                JObject JSucursal = new JObject();
                Sucursal.LlenaObjeto(Sucursal.IdSucursal, ConexionBaseDatos);
                JSucursal.Add("IdSucursal", Sucursal.IdSucursal);
                JSucursal.Add("Sucursal", Sucursal.Sucursal);
                JSucursales.Add(JSucursal);
            }

            Modelo.Add(new JProperty("Sucursales", JSucursales));
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

    //Validaciones
    private static string ValidaUsuario(int pIdUsuario, string pNombre, string pApellidoPaterno, string pApellidoMaterno, string pFechaNacimiento, string pUsuario, string pContrasena, string pCorreo, int pPerfil, CConexion pConexion)
    {
        string errores = "";
        if (pNombre == "")
        { errores = errores + "<span>*</span> El campo nombre esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pApellidoPaterno == "")
        { errores = errores + "<span>*</span> El campo apellido paterno esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pApellidoMaterno == "")
        { errores = errores + "<span>*</span> El campo apellido materno esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pFechaNacimiento == "")
        { errores = errores + "<span>*</span> El campo fecha de nacimiento esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pUsuario == "")
        { errores = errores + "<span>*</span> El campo usuario esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pContrasena == "")
        { errores = errores + "<span>*</span> El campo contraseña esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pCorreo == "")
        { errores = errores + "<span>*</span> El campo correo esta vac&iacute;o, favor de capturarlo.<br />"; }

        //¿Correo no valido?
        if (pCorreo != "")
        {
            CValidacion ValidarCorreo = new CValidacion();
            if (ValidarCorreo.ValidarCorreo(pCorreo))
            { errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
        }

        CUsuario Usuario = new CUsuario();
        //¿Usuario o correo existen?
        if (pUsuario != "" && pCorreo != "")
        {
            if (Usuario.ExisteUsuario(pIdUsuario, pUsuario, pConexion))
            { errores = errores + "<span>*</span> El usuario que elegiste ya está en uso.<br />"; }
            if (Usuario.ExisteCorreo(pIdUsuario, pCorreo, pConexion))
            { errores = errores + "<span>*</span> El correo que elegiste ya está en uso.<br />"; }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidaDatosUsuario(int idUsuario, string pNombre, string pApellidoPaterno, string pApellidoMaterno, string pUsuario, string pFechaNacimiento, string pCorreo, CConexion pConexion)
    {
        string errores = "";
        if (pNombre == "")
        { errores = errores + "<span>*</span> El campo nombre esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pApellidoPaterno == "")
        { errores = errores + "<span>*</span> El campo apellido paterno esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pApellidoMaterno == "")
        { errores = errores + "<span>*</span> El campo apellido materno esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pUsuario == "")
        { errores = errores + "<span>*</span> El campo usuario esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pFechaNacimiento == "")
        { errores = errores + "<span>*</span> El campo fecha de nacimiento esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pCorreo == "")
        { errores = errores + "<span>*</span> El campo correo esta vac&iacute;o, favor de capturarlo.<br />"; }

        //¿Correo no valido?
        if (pCorreo != "")
        {
            CValidacion ValidarCorreo = new CValidacion();
            if (ValidarCorreo.ValidarCorreo(pCorreo))
            { errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
        }

        CUsuario Usuario = new CUsuario();
        //¿Usuario o correo existen?
        if (pUsuario != "" && pCorreo != "")
        {
            if (Usuario.ExisteUsuario(idUsuario, pUsuario, pConexion))
            { errores = errores + "<span>*</span> El usuario que elegiste ya está en uso.<br />"; }
            if (Usuario.ExisteCorreo(idUsuario, pCorreo, pConexion))
            { errores = errores + "<span>*</span> El correo que elegiste ya está en uso.<br />"; }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidaCambioContrasena(int pIdUsuario, string pActual, string pNueva, string pConfirmar, CConexion pConexion)
    {
        string errores = "";
        if (pActual == "")
        { errores = errores + "<span>*</span> El campo contraseña actual esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pNueva == "")
        { errores = errores + "<span>*</span> El campo nueva contraseña esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pConfirmar == "")
        { errores = errores + "<span>*</span> El campo confirmar contraseña esta vac&iacute;o, favor de capturarlo.<br />"; }


        if (pNueva != "" && pConfirmar != "")
        {
            if (pNueva != pConfirmar)
            {
                errores = errores + "<span>*</span> Los campos contraseña nueva y confirmar no coinciden, favor de capturarlos nuevamente.<br />";
            }
        }

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(pIdUsuario, pConexion);
        if (Usuario.Usuario == "" || Usuario.Usuario == null || Usuario.Baja == true || (Usuario.Contrasena != pActual))
        {
            errores = "<span>*</span> La contraseña actual es incorrecta, intentalo de nuevo.";
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidaEditarContrasena(int pIdUsuario, string pContrasenaAdministrador, CConexion pConexion)
    {
        string errores = "";

        CUsuario Usuario = new CUsuario(pIdUsuario);
        string validarPermiso = Usuario.TienePermisos(new string[] { "puedeCambiarContrasenas" }, pConexion);

        if (validarPermiso == "")
        {
            Usuario.LlenaObjeto(pIdUsuario, pConexion);
            if (Usuario.Usuario == "" || Usuario.Usuario == null || Usuario.Baja == true)
            {
                errores = "<span>*</span> No tiene los permisos requeridos.";
            }
            else if (Usuario.Contrasena != pContrasenaAdministrador)
            {
                errores = "<span>*</span> La contraseña del usuario en sesión es incorrecta, vuelva a intentarlo.";
            }
        }
        else
        {
            errores = "<span>*</span> No tiene los permisos requeridos.";
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

	[WebMethod]
	public static string ObtenerMetasUsuario (int IdUsuario)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_Usuario_MetasUsuario";
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;

				Consulta.Llena(pConexion);

				JArray Metas = new JArray();

				while (Consulta.Registros.Read())
				{
					JObject Fila = new JObject();
					Fila.Add("IdDivision", Convert.ToInt32(Consulta.Registros["IdDivision"]));
					Fila.Add("Division", Convert.ToString(Consulta.Registros["Division"]));
					Fila.Add("Meta", Convert.ToDecimal(Consulta.Registros["Meta"]));
					Metas.Add(Fila);
				}

				Modelo.Add("IdUsuario", IdUsuario);
				Modelo.Add("Metas", Metas);

				Consulta.CerrarConsulta();

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string GuardarMetasUsuario (int IdUsuario, Dictionary<string, object> Metas)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				object[] Divisiones = (object[]) Metas["Metas"];

				foreach (Dictionary<string, object> Division in Divisiones)
				{
					CUsuarioDivision UsuarioDivision = new CUsuarioDivision();
					Dictionary<string, object> pParametros = new Dictionary<string, object>();
					pParametros.Add("IdUsuario", IdUsuario);
					pParametros.Add("IdDivision", Convert.ToInt32(Division["IdDivision"]));
					UsuarioDivision.LlenaObjetoFiltros(pParametros, pConexion);

					UsuarioDivision.IdUsuario = IdUsuario;
					UsuarioDivision.IdDivision = Convert.ToInt32(Division["IdDivision"]);
					UsuarioDivision.Meta = Convert.ToDecimal(Division["Meta"]);

					if (UsuarioDivision.IdUsuarioDivision == 0)
					{
						UsuarioDivision.Agregar(pConexion);
					}
					else
					{
						UsuarioDivision.Editar(pConexion);
					}
				}

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

}