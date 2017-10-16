using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.Services;
using System.Web.Script.Services;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Xml;
using arquitecturaNet.ConversorTipoCambio;
using arquitecturaNet.ConversorMonedas;

public partial class InicioSesion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtUsuario.Attributes.Add("onkeypress", "javascript:return IniciarSesionEnter(event);");
        txtContrasena.Attributes.Add("onkeypress", "javascript:return IniciarSesionEnter(event);");
        HttpContext.Current.Session["IdUsuario"] = "";
    }

    //Metodos Ajax
    [WebMethod]
    public static string IniciarSesion(string pUsuario, string pContrasena)
    {
        string validacion = ValidaUsuario(pUsuario, pContrasena);

        if (validacion != "")
        {
            return "1|" + validacion;
        }
        else
        {
            //Abrir Conexion
            CConexion ConexionBaseDatos = new CConexion();
            string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

            //¿La conexion se establecio?
            if (respuesta == "Conexion Establecida")
            {
                CUsuario Usuario = new CUsuario();
                Usuario.IniciarSesion(pUsuario, pContrasena, ConexionBaseDatos);
                if (Usuario.Usuario == "" || Usuario.Usuario == null || Usuario.Baja == true)
                {
                    respuesta = "1|El usuario o contraseña son incorrectos, intentalo de nuevo.";
                }
                else
                {
                    HttpContext.Current.Session["IdUsuario"] = Usuario.IdUsuario;
                    HttpContext.Current.Session["IdSucursal"] = Usuario.IdSucursalPredeterminada;
                    HttpCookie sesion = new HttpCookie("IdUsuario");
                    sesion.Value = Usuario.IdUsuario.ToString();
                    CSucursal Sucursal = new CSucursal();
                    Sucursal.LlenaObjeto(Usuario.IdSucursalPredeterminada, ConexionBaseDatos);
                    HttpContext.Current.Session["IdEmpresa"] = Sucursal.IdEmpresa;
                    if (Usuario.IdPerfil != 0 && (Usuario.TienePermisos(new string[] { "controlTotal" }, ConexionBaseDatos) == "" || Usuario.TienePermisos(new string[] { "perfilConfigurador" }, ConexionBaseDatos) == ""))
                    {
                        Usuario.LlenaObjeto(Usuario.IdUsuario, ConexionBaseDatos);
                        Usuario.IdSucursalActual = Usuario.IdSucursalPredeterminada;
                        Usuario.Editar(ConexionBaseDatos);
                        CPerfil Perfil = new CPerfil();
                        Perfil.IdPerfil = Usuario.IdPerfil;
                        Perfil.LlenaObjeto(Usuario.IdPerfil, ConexionBaseDatos);
                        CPagina PaginaInicio = new CPagina();
                        PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);
                        CMenu Menu = new CMenu();
                        Menu.LlenaObjeto(PaginaInicio.IdMenu, ConexionBaseDatos);
                        respuesta = "0|Paginas/" + PaginaInicio.Pagina;
                    }
                    else if (Usuario.TieneSucursalAsignada(ConexionBaseDatos) == true)
                    {
                        Usuario.LlenaObjeto(Usuario.IdUsuario, ConexionBaseDatos);
                        Usuario.IdSucursalActual = Usuario.IdSucursalPredeterminada;
                        Usuario.Editar(ConexionBaseDatos);

                        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
                        Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
                        ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
                        ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);
                        SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, ConexionBaseDatos);

                        CPerfil Perfil = new CPerfil();
                        Perfil.LlenaObjeto(SucursalAsignada.IdPerfil, ConexionBaseDatos);
                        CPagina PaginaInicio = new CPagina();
                        PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);
                        CMenu Menu = new CMenu();
                        Menu.LlenaObjeto(PaginaInicio.IdMenu, ConexionBaseDatos);
                        respuesta = "0|Paginas/" + PaginaInicio.Pagina;
                    }
                    else
                    {
                        HttpContext.Current.Session["IdUsuario"] = 0;
                        sesion.Value = "0";
                        respuesta = "1|No tienes ninguna sucursal asignada, favor de avisar al administrador.";
                    }
                    HttpContext.Current.Response.SetCookie(sesion);


                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "El usuario " + Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno + " ingreso al sistema";
                    HistorialGenerico.AgregarHistorialGenerico("Usuario", ConexionBaseDatos);


                }
            }
            return respuesta;
        }
    }

    [WebMethod]
    public static string CambiarContrasena(int pIdUsuario, string pContrasena, string pConfirmar)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(pIdUsuario, ConexionBaseDatos);
            Usuario.Contrasena = pContrasena;
            Usuario.Editar(ConexionBaseDatos);
            HttpContext.Current.Session["IdUsuario"] = Usuario.IdUsuario;
            CPerfil Perfil = new CPerfil();
            Perfil.IdPerfil = Usuario.IdPerfil;
            Perfil.LlenaObjeto(Usuario.IdPerfil, ConexionBaseDatos);
            CPagina PaginaInicio = new CPagina();
            PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);
            CMenu Menu = new CMenu();
            Menu.LlenaObjeto(PaginaInicio.IdMenu, ConexionBaseDatos);
            respuesta = "0|Paginas/" + PaginaInicio.Pagina;
        }
        return respuesta;
    }

    //Validaciones
    private static string ValidaUsuario(string pUsuario, string pContrasena)
    {
        string errores = "";

        if (pUsuario == "")
        { errores = errores + "<span>*</span> El campo usuario esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pContrasena == "")
        { errores = errores + "<span>*</span> El campo contrase&ntilde;a esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }
        return errores;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string MantenerSesion()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string conexion = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        string respuesta = "";
        if (conexion == "Conexion Establecida")
        {
            int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            IdUsuario = (IdUsuario == 0) ? Convert.ToInt32(HttpContext.Current.Request.Cookies["IdUsuario"]) : IdUsuario;
            if (IdUsuario != 0)
            {
                CUsuario Usuario = new CUsuario();
                HttpCookie miSesion = new HttpCookie("IdUsuario");
                Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);
                HttpContext.Current.Session["IdUsuario"] = Usuario.IdUsuario;
                HttpContext.Current.Session["IdSucursal"] = Usuario.IdSucursalActual;
                miSesion.Value = Usuario.IdUsuario.ToString();
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
                HttpContext.Current.Session["IdEmpresa"] = Sucursal.IdEmpresa;
                respuesta = "MantieneSesion";
            }
            else
            {
                respuesta = "Sesión expirada";
            }
        }
        else {
            respuesta = "Sin coneccion";
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerMenuPredeterminado(string pPaginaActual)
    {
        JObject JRespuesta = new JObject();
        if (pPaginaActual != "InicioSesion.aspx")
        {
            CConexion ConexionBaseDatos = new CConexion();
            string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
            if (respuesta == "Conexion Establecida")
            {
                JObject Modelo = new JObject();
                string carpeta = "";
                CPagina PaginaActual = new CPagina();
                PaginaActual.FiltrarPagina(pPaginaActual, ConexionBaseDatos);
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
                CPerfil Perfil = new CPerfil();
                Perfil.IdPerfil = Usuario.IdPerfil;
                Perfil.LlenaObjeto(Usuario.IdPerfil, ConexionBaseDatos);
                CPagina PaginaInicio = new CPagina();
                PaginaInicio.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);
                CMenu MenuInicio = new CMenu();
                MenuInicio.LlenaObjeto(PaginaInicio.IdMenu, ConexionBaseDatos);
                CMenu MenuPrincipal = new CMenu();

                if (MenuInicio.IdMenu != PaginaActual.IdMenu)
                { carpeta = "../" + MenuInicio.Menu + "/"; }
                Modelo.Add(new JProperty("PaginaInicio", PaginaInicio.Pagina));
                JArray JMenu = new JArray();
                foreach (CMenu Menu in MenuPrincipal.ObtenerMenuPrincipal(Usuario.IdPerfil, "Seguridad", ConexionBaseDatos))
                {
                    CPagina Pagina = new CPagina();
                    JObject JMenuPrincipal = new JObject();
                    JMenuPrincipal.Add("MenuPrincipal", Menu.Menu);
                    JArray JMenusSecundarios = new JArray();
                    int noMenusSecundarios = 0;
                    foreach (CPagina MenuSecundario in Pagina.ObtenerMenuSecundario(Menu.IdMenu, Usuario.IdPerfil, "Seguridad", ConexionBaseDatos))
                    {
                        noMenusSecundarios = noMenusSecundarios + 1;
                        JObject JMenuSecundario = new JObject();
                        JMenuSecundario.Add(new JProperty("Pagina", MenuSecundario.Pagina));
                        JMenuSecundario.Add(new JProperty("MenuSecundario", MenuSecundario.NombreMenu));
                        JMenusSecundarios.Add(JMenuSecundario);
                    }
                    JMenuPrincipal.Add("NoMenusSecundarios", noMenusSecundarios);
                    JMenuPrincipal.Add("MenusSecundarios", JMenusSecundarios);
                    JMenu.Add(JMenuPrincipal);
                }
                Modelo.Add("Menu", JMenu);
                JRespuesta.Add(new JProperty("Error", 0));
                JRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                JRespuesta.Add(new JProperty("Error", 1));
                JRespuesta.Add(new JProperty("Descripcion", "Esto fue un error"));
            }
        }
        else
        {
            JRespuesta.Add(new JProperty("Error", 1));
            JRespuesta.Add(new JProperty("Descripcion", "No hay menu porque se encuentra en el login."));
        }
        return JRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerPanelUbicacion(int IdTipoMonedaOrigen, int IdTipoMonedaDestino)
    {
        JObject oRespuesta = new JObject();
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
            Modelo.Add("Sucursal", Sucursal.Sucursal);

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);
            Modelo.Add("Empresa", Empresa.Empresa);

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(IdTipoMonedaOrigen));
            Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(IdTipoMonedaDestino));
            Parametros.Add("Fecha", DateTime.Today);

            CTipoCambio TipoCambio = new CTipoCambio();
            TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
            Modelo.Add("TipoCambio", TipoCambio.TipoCambio);

            oRespuesta.Add("Modelo", Modelo);
            oRespuesta.Add("Error", 0);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaCambiarSucursal()
    {
        JObject oRespuesta = new JObject();
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CSucursal SucursalActual = new CSucursal();
            SucursalActual.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

            JArray JAEmpresas = new JArray();
            CEmpresa EmpresaActual = new CEmpresa();
            EmpresaActual.LlenaObjeto(SucursalActual.IdEmpresa, ConexionBaseDatos);
            foreach (CEmpresa oEmpresa in EmpresaActual.ObtenerEmpresasAsignadas(Usuario.IdUsuario, ConexionBaseDatos))
            {
                JObject JEmpresa = new JObject();
                JEmpresa.Add("Valor", oEmpresa.IdEmpresa);
                JEmpresa.Add("Descripcion", oEmpresa.Empresa);
                if (SucursalActual.IdEmpresa == oEmpresa.IdEmpresa)
                {
                    JEmpresa.Add("Selected", 1);
                }
                else
                {
                    JEmpresa.Add("Selected", 0);
                }
                JAEmpresas.Add(JEmpresa);
            }
            Modelo.Add("Empresas", JAEmpresas);

            JArray JASucursales = new JArray();
            foreach (CSucursal oSucursal in SucursalActual.ObtenerSucursalesAsignadas(Usuario.IdUsuario, SucursalActual.IdEmpresa, ConexionBaseDatos))
            {
                JObject JSucursal = new JObject();
                JSucursal.Add("Valor", oSucursal.IdSucursal);
                JSucursal.Add("Descripcion", oSucursal.Sucursal);
                if (SucursalActual.IdSucursal == oSucursal.IdSucursal)
                {
                    JSucursal.Add("Selected", 1);
                }
                else
                {
                    JSucursal.Add("Selected", 0);
                }
                JASucursales.Add(JSucursal);
            }
            Modelo.Add("Sucursales", JASucursales);

            oRespuesta.Add("Modelo", Modelo);
            oRespuesta.Add("Error", 0);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerSucursalesAsignadas(int pIdEmpresa)
    {
        JObject oRespuesta = new JObject();
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CSucursal Sucursal = new CSucursal();

            JArray JASucursales = new JArray();
            foreach (CSucursal oSucursal in Sucursal.ObtenerSucursalesAsignadas(Usuario.IdUsuario, pIdEmpresa, ConexionBaseDatos))
            {
                JObject JSucursal = new JObject();
                JSucursal.Add("Valor", oSucursal.IdSucursal);
                JSucursal.Add("Descripcion", oSucursal.Sucursal);
                if (Usuario.IdSucursalActual == oSucursal.IdSucursal)
                {
                    JSucursal.Add("Selected", 1);
                }
                else
                {
                    JSucursal.Add("Selected", 0);
                }
                JASucursales.Add(JSucursal);
            }
            Modelo.Add("Opciones", JASucursales);
            oRespuesta.Add("Modelo", Modelo);
            oRespuesta.Add("Error", 0);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarSucursal(int pIdSucursal)
    {
        JObject oRespuesta = new JObject();
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Usuario.IdSucursalActual = pIdSucursal;
            Usuario.Editar(ConexionBaseDatos);
            HttpContext.Current.Session["IdSucursal"] = pIdSucursal;
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            HttpContext.Current.Session["IdEmpresa"] = Sucursal.IdEmpresa;

            oRespuesta.Add("Error", 0);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        return oRespuesta.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string DatosZopim()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string conexion = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        string respuesta = "";
        if (conexion == "Conexion Establecida")
        {
            int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            if (IdUsuario != 0)
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);
                HttpContext.Current.Session["IdUsuario"] = Usuario.IdUsuario;
                HttpContext.Current.Session["Correo"] = Usuario.Correo;
                HttpContext.Current.Session["IdSucursal"] = Usuario.IdSucursalActual;
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
                HttpContext.Current.Session["IdEmpresa"] = Sucursal.IdEmpresa;
                respuesta = "MantieneSesion";
            }
            else
            {
                respuesta = "Sesión expirada";
            }
        }
        else
        {
            respuesta = "Sin coneccion";
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //[WebMethod]
    //public static string ActualizaTipoCambioWS()
    //{
    //    string resultadoTC = string.Empty;
    //    decimal tc = 0;
    //    CConexion ConexionBaseDatos = new CConexion();
    //    CTipoMoneda TipoMoneda = new CTipoMoneda();
    //    CTipoMoneda TipoMonedaDest = new CTipoMoneda();
    //    CTipoCambio clsTipoCambio = new CTipoCambio();
    //    JObject oRespuesta = new JObject();

    //    try
    //    {
    //        string respuestaConexion = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //        ConversorTipoCambio.CurrencyConvertor wsTC = new ConversorTipoCambio.CurrencyConvertor();

    //        Dictionary<string, object> Monedas = new Dictionary<string, object>();
    //        Monedas.Add("Pesos", Currency.MXN);
    //        Monedas.Add("Dolares", Currency.USD);
    //        Monedas.Add("Euros", Currency.EUR);
    //        Monedas.Add("Yenes", Currency.JPY);

    //        foreach (KeyValuePair<string, object> MonedaOrigen in Monedas)
    //        {
    //            foreach (KeyValuePair<string, object> MonedaDestino in Monedas)
    //            {
    //                resultadoTC = wsTC.ConversionRate((Currency)MonedaOrigen.Value, (Currency)MonedaDestino.Value).ToString();
    //                tc = Convert.ToDecimal(resultadoTC);

    //                if (respuestaConexion == "Conexion Establecida")
    //                {
    //                    Dictionary<string, object> Parametros = new Dictionary<string, object>();
    //                    Parametros.Add("TipoMoneda", MonedaOrigen.Key);
    //                    TipoMoneda.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

    //                    Dictionary<string, object> ParametrosD = new Dictionary<string, object>();
    //                    ParametrosD.Add("TipoMoneda", MonedaDestino.Key);
    //                    TipoMonedaDest.LlenaObjetoFiltros(ParametrosD, ConexionBaseDatos);

    //                    clsTipoCambio.IdTipoMonedaOrigen = TipoMoneda.IdTipoMoneda;
    //                    clsTipoCambio.IdTipoMonedaDestino = TipoMonedaDest.IdTipoMoneda;
    //                    clsTipoCambio.TipoCambio = tc;
    //                    clsTipoCambio.Fecha = DateTime.Today;

    //                    if (!clsTipoCambio.ExisteTipoCambio(clsTipoCambio.IdTipoMonedaOrigen, clsTipoCambio.IdTipoMonedaDestino, clsTipoCambio.Fecha, ConexionBaseDatos))
    //                    {
    //                        clsTipoCambio.Agregar(ConexionBaseDatos);

    //                        CTipoCambioDiarioOficial TipoCambioDiarioOficial = new CTipoCambioDiarioOficial();
    //                        TipoCambioDiarioOficial.IdTipoMonedaOrigen = TipoMoneda.IdTipoMoneda;
    //                        TipoCambioDiarioOficial.IdTipoMonedaDestino = TipoMonedaDest.IdTipoMoneda;
    //                        TipoCambioDiarioOficial.TipoCambioDiarioOficial = tc;
    //                        TipoCambioDiarioOficial.Fecha = DateTime.Today;
    //                        TipoCambioDiarioOficial.Agregar(ConexionBaseDatos);
    //                    }
    //                    // else
    //                    //{
    //                    //  clsTipoCambio.Editar(ConexionBaseDatos);
    //                    //}
    //                }
    //            }
    //        }

    //        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //        oRespuesta.Add("Error", 0);
    //    }
    //    catch (Exception ex)
    //    {
    //        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //        oRespuesta = ActualizaTipoCambioWSOpcional();
    //    }

    //    return oRespuesta.ToString();
    //}

    //public static JObject ActualizaTipoCambioWSOpcional()
    //{
    //    decimal tc = 0;
    //    CConexion ConexionBaseDatos = new CConexion();
    //    CTipoMoneda TipoMoneda = new CTipoMoneda();
    //    CTipoMoneda TipoMonedaDest = new CTipoMoneda();
    //    CTipoCambio clsTipoCambio = new CTipoCambio();
    //    JObject oRespuesta = new JObject();

    //    try
    //    {
    //        string respuestaConexion = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //        ConversorMonedas.Converter wsTC = new ConversorMonedas.Converter();

    //        Dictionary<string, object> Monedas = new Dictionary<string, object>();
    //        Monedas.Add("Pesos", "MXN");
    //        Monedas.Add("Dolares", "USD");
    //        Monedas.Add("Euros", "EUR");
    //        Monedas.Add("Yenes", "JPY");

    //        foreach (KeyValuePair<string, object> MonedaOrigen in Monedas)
    //        {
    //            foreach (KeyValuePair<string, object> MonedaDestino in Monedas)
    //            {
    //                tc = wsTC.GetConversionRate((string)MonedaOrigen.Value, (string)MonedaDestino.Value, wsTC.GetLastUpdateDate());

    //                if (respuestaConexion == "Conexion Establecida")
    //                {
    //                    Dictionary<string, object> Parametros = new Dictionary<string, object>();
    //                    Parametros.Add("TipoMoneda", MonedaOrigen.Key);
    //                    TipoMoneda.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

    //                    Dictionary<string, object> ParametrosD = new Dictionary<string, object>();
    //                    ParametrosD.Add("TipoMoneda", MonedaDestino.Key);
    //                    TipoMonedaDest.LlenaObjetoFiltros(ParametrosD, ConexionBaseDatos);

    //                    clsTipoCambio.IdTipoMonedaOrigen = TipoMoneda.IdTipoMoneda;
    //                    clsTipoCambio.IdTipoMonedaDestino = TipoMonedaDest.IdTipoMoneda;
    //                    clsTipoCambio.TipoCambio = tc;
    //                    clsTipoCambio.Fecha = DateTime.Today;

    //                    if (!clsTipoCambio.ExisteTipoCambio(clsTipoCambio.IdTipoMonedaOrigen, clsTipoCambio.IdTipoMonedaDestino, clsTipoCambio.Fecha, ConexionBaseDatos))
    //                    {
    //                        clsTipoCambio.Agregar(ConexionBaseDatos);
    //                    }
    //                    else
    //                    {
    //                        clsTipoCambio.Editar(ConexionBaseDatos);
    //                    }
    //                }
    //            }
    //        }

    //        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //        oRespuesta.Add("Error", 0);
    //    }
    //    catch (Exception ex)
    //    {
    //        oRespuesta.Add(new JProperty("Error", 1));
    //        oRespuesta.Add(new JProperty("Descripcion", "Ocurrió un error al actualizar el tipo de cambio. Favor de entrar al catálogo para su modificación."));
    //    }

    //    return oRespuesta;
    //}
}