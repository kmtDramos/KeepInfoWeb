using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Services;

public partial class Paginas : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //Metodos Ajax
    [WebMethod]
    public static string AgregarPagina(string pPagina, string pTitulo, string pNombreMenu, int pIdMenu, bool pValidarSucursal)
    {
        string validacion = ValidaPagina(pPagina, pTitulo, pNombreMenu, pIdMenu);
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
                CPagina Pagina = new CPagina();
                Pagina.Pagina = pPagina;
                Pagina.Titulo = pTitulo;
                Pagina.NombreMenu = pNombreMenu;
                Pagina.IdMenu = pIdMenu;
                Pagina.ValidarSucursal = pValidarSucursal;
                Pagina.Agregar(ConexionBaseDatos);
                string JsonPaginas = Pagina.ObtenerJsonArbol(ConexionBaseDatos);
                respuesta = "0|" + JsonPaginas;
            }

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return respuesta;
        }
    }

    [WebMethod]
    public static string EditarPagina(int pIdPagina, string pPagina, string pTitulo, string pNombreMenu, int pIdMenu, bool pValidarSucursal)
    {
        string validacion = ValidaPagina(pPagina, pTitulo, pNombreMenu, pIdMenu);
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
                CPagina Pagina = new CPagina();
                Pagina.LlenaObjeto(pIdPagina, ConexionBaseDatos);
                Pagina.Pagina = pPagina;
                Pagina.Titulo = pTitulo;
                Pagina.NombreMenu = pNombreMenu;
                Pagina.IdMenu = pIdMenu;
                Pagina.ValidarSucursal = pValidarSucursal;
                Pagina.Editar(ConexionBaseDatos);
                string JsonPaginas = Pagina.ObtenerJsonArbol(ConexionBaseDatos);
                respuesta = "0|" + JsonPaginas;
            }

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return respuesta;
        }
    }

    [WebMethod]
    public static string EliminarPagina(int pIdPagina)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPagina Pagina = new CPagina();
            Pagina.IdPagina = pIdPagina;
            Pagina.Eliminar(ConexionBaseDatos);
            string JsonPaginas = Pagina.ObtenerJsonArbol(ConexionBaseDatos);
            respuesta = "0|" + JsonPaginas;
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerJsonArbolPaginas()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPagina Pagina = new CPagina();
            string JsonPaginas = Pagina.ObtenerJsonArbol(ConexionBaseDatos);
            respuesta = JsonPaginas.ToString();
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidaPagina(string pPagina, string pTitulo, string pNombreMenu, int pIdMenu)
    {
        string errores = "";
        if (pPagina == "")
        { errores = errores + "<span>*</span> El campo pagina esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pTitulo == "")
        { errores = errores + "<span>*</span> El campo titulo esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pNombreMenu == "")
        { errores = errores + "<span>*</span> El campo nombre men&uacute; esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}