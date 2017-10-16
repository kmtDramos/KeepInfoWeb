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
using Newtonsoft.Json.Linq;

public partial class Menu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //Metodos Ajax
    [WebMethod]
    public static string AgregarMenu(string pMenu, int pIdProyectoSistema)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            //Valida permisos
            CUsuario Usuario = new CUsuario();
            string validaPermisos = Usuario.TienePermisos(new string[] { "ConsultarMenus", "AgregarMenu" }, ConexionBaseDatos);
            if (validaPermisos != "")
            {
                return "1|" + validaPermisos;
            }

            //Valida campos
            string validacion = ValidaMenu(pMenu, pIdProyectoSistema);
            if (validacion != "")
            {
                return "1|" + validacion;
            }
            else
            {

                CMenu Menu = new CMenu();
                Menu.Menu = pMenu;
                Menu.IdProyectoSistema = pIdProyectoSistema;
                Menu.Agregar(ConexionBaseDatos);
                string JsonMenu = Menu.ObtenerJsonArbol(ConexionBaseDatos);
                respuesta = JsonMenu.ToString();
            }

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return respuesta;
        }
        else
        {
            return "1|" + respuesta;
        }
    }

    [WebMethod]
    public static string EditarMenu(int pIdMenu, string pMenu, int pIdProyectoSistema)
    {
        string validacion = ValidaMenu(pMenu, pIdProyectoSistema);
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
                CMenu Menu = new CMenu();
                Menu.IdMenu = pIdMenu;
                Menu.Menu = pMenu;
                Menu.IdProyectoSistema = pIdProyectoSistema;
                Menu.Editar(ConexionBaseDatos);
                string XMLMenu = Menu.ObtenerJsonArbol(ConexionBaseDatos);
                respuesta = "0|" + XMLMenu;
            }

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return respuesta;
        }
    }

    [WebMethod]
    public static string EliminarMenu(int pIdMenu)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CMenu Menu = new CMenu();
            Menu.IdMenu = pIdMenu;
            Menu.Eliminar(ConexionBaseDatos);
            string XMLMenu = Menu.ObtenerJsonArbol(ConexionBaseDatos);
            respuesta = "0|" + XMLMenu;
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerJsonArbolMenu()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CMenu Menu = new CMenu();
            string JsonMenu = Menu.ObtenerJsonArbol(ConexionBaseDatos);
            respuesta = JsonMenu.ToString();
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerSubmenus(int pIdMenu)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject OJson = new JObject();
            JObject Submenu;
            JArray ArregloObjetos = new JArray();

            CPagina Pagina = new CPagina();
            foreach (CPagina OP in Pagina.LlenaObjetos(pIdMenu, ConexionBaseDatos))
            {
                Submenu = new JObject();
                Submenu.Add(new JProperty("IdPagina", OP.IdPagina));
                Submenu.Add(new JProperty("Submenu", OP.NombreMenu));
                ArregloObjetos.Add(Submenu);
            }
            OJson.Add("Success", true);
            OJson.Add("ListaSubmenus", ArregloObjetos);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
        else
        {
            JObject OJson = new JObject();
            OJson.Add("Success", false);
            OJson.Add("Mensaje", respuesta);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
    }

    [WebMethod]
    public static string ObtenerMenus()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject OJson = new JObject();
            JObject JMenu;
            JArray JArregloObjetos = new JArray();

            CMenu Menu = new CMenu();
            foreach (CMenu OM in Menu.LlenaObjetos_OrdenadoPorOrden(ConexionBaseDatos))
            {
                JMenu = new JObject();
                JMenu.Add(new JProperty("IdMenu", OM.IdMenu));
                JMenu.Add(new JProperty("Menu", OM.Menu));
                JArregloObjetos.Add(JMenu);
            }
            OJson.Add("Success", true);
            OJson.Add("ListaMenus", JArregloObjetos);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
        else
        {
            JObject OJson = new JObject();
            OJson.Add("Success", false);
            OJson.Add("Mensaje", respuesta);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
    }

    [WebMethod]
    public static string OrdenarSubmenus(object pObjetoJSON)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPagina Pagina = new CPagina();
            foreach (Dictionary<string, object> OPagina in (Array)pObjetoJSON)
            {
                Pagina.IdPagina = Convert.ToInt32(OPagina["IdPagina"]);
                Pagina.Orden = Convert.ToInt32(OPagina["Orden"]);
                Pagina.OrdenarSubmenu(ConexionBaseDatos);
            }
            JObject OJson = new JObject();
            OJson.Add("Error", false);
            OJson.Add("Mensaje", "Los cambios se guardaron correctamente, actualiza la pagina para mostrar los cambios en el menú.");
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
        else
        {
            JObject OJson = new JObject();
            OJson.Add("Error", true);
            OJson.Add("Mensaje", respuesta);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
    }

    [WebMethod]
    public static string OrdenarMenus(object pObjetoJSON)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CMenu Menu = new CMenu();
            foreach (Dictionary<string, object> OMenu in (Array)pObjetoJSON)
            {
                Menu.IdMenu = Convert.ToInt32(OMenu["IdMenu"]);
                Menu.Orden = Convert.ToInt32(OMenu["Orden"]);
                Menu.OrdenarMenu(ConexionBaseDatos);
            }
            JObject OJson = new JObject();
            OJson.Add("Error", false);
            OJson.Add("Mensaje", "Los cambios se guardaron correctamente, actualiza la pagina para mostrar los cambios en el menú.");
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
        else
        {
            JObject OJson = new JObject();
            OJson.Add("Error", true);
            OJson.Add("Mensaje", respuesta);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return OJson.ToString();
        }
    }

    //Validaciones
    private static string ValidaMenu(string pMenu, int pIdProyectoSistema)
    {
        string errores = "";

        if (pMenu == "")
        { errores = errores + "<span>*</span> El campo men&uacute; esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pIdProyectoSistema == 0)
        { errores = errores + "<span>*</span> El campo ProyectoSistema no se ha seleccionado, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }
        return errores;
    }
}