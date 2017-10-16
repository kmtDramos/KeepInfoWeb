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

public partial class Perfiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //Metodos Ajax
    [WebMethod]
    public static string AgregarPerfil(string pPerfil, int pIdPagina, bool pEsPerfilSucursal)
    {
        string validacion = ValidaPerfil(pPerfil, pIdPagina);
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
                CPerfil Perfil = new CPerfil();
                Perfil.Perfil = pPerfil;
                Perfil.IdPagina = pIdPagina;
                Perfil.EsPerfilSucursal = pEsPerfilSucursal;
                Perfil.Agregar(ConexionBaseDatos);
                string JsonPerfil = Perfil.ObtenerJsonArbol(ConexionBaseDatos);
                respuesta = "0|" + JsonPerfil;
            }

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return respuesta;
        }
    }

    [WebMethod]
    public static string EditarPerfil(int pIdPerfil, string pPerfil, int pIdPagina, bool pEsPerfilSucursal)
    {
        string validacion = ValidaPerfil(pPerfil, pIdPagina);
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
                CPerfil Perfil = new CPerfil();
                Perfil.IdPerfil = pIdPerfil;
                Perfil.Perfil = pPerfil;
                Perfil.IdPagina = pIdPagina;
                Perfil.EsPerfilSucursal = pEsPerfilSucursal;
                Perfil.Editar(ConexionBaseDatos);
                string JsonPerfil = Perfil.ObtenerJsonArbol(ConexionBaseDatos);
                respuesta = "0|" + JsonPerfil;
            }

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return respuesta;
        }
    }

    [WebMethod]
    public static string EliminarPerfil(int pIdPerfil)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPerfil Perfil = new CPerfil();
            Perfil.IdPerfil = pIdPerfil;
            Perfil.Eliminar(ConexionBaseDatos);
            string JsonPerfil = Perfil.ObtenerJsonArbol(ConexionBaseDatos);
            respuesta = "0|" + JsonPerfil;
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerJsonArbolPerfiles()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPerfil Perfil = new CPerfil();
            string JsonPerfil = Perfil.ObtenerJsonArbol(ConexionBaseDatos);
            respuesta = JsonPerfil;
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidaPerfil(string pPerfil, int pIdPagina)
    {
        string errores = "";

        if (pPerfil == "")
        { errores = errores + "<span>*</span> El campo perfil esta vac&iacute;o, favor de capturarlo.<br />"; }
        if (pIdPagina == 0)
        { errores = errores + "<span>*</span> Es necesario eligir una pagina, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}