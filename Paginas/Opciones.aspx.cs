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

public partial class Opciones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    //Metodos Ajax
    [WebMethod]
    public static string AgregarOpcion(string pOpcion, string pComando)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string validacion = ValidaOpcion(0, pOpcion, pComando, ConexionBaseDatos);
            if (validacion != "")
            {
                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "1|" + validacion;
            }
            else
            {
                COpcion Opcion = new COpcion();
                Opcion.Opcion = pOpcion;
                Opcion.Comando = pComando;
                Opcion.Agregar(ConexionBaseDatos);
                string JsonOpcion = Opcion.ObtenerJsonArbol(ConexionBaseDatos);

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return respuesta = "0|" + JsonOpcion;
            }
        }
        else
        {
            return respuesta;
        }
    }

    [WebMethod]
    public static string EditarOpcion(int pIdOpcion, string pOpcion, string pComando)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string validacion = ValidaOpcion(pIdOpcion, pOpcion, pComando, ConexionBaseDatos);
            if (validacion != "")
            {
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return "1|" + validacion;
            }
            else
            {

                COpcion Opcion = new COpcion();
                Opcion.IdOpcion = pIdOpcion;
                Opcion.Opcion = pOpcion;
                Opcion.Comando = pComando;
                Opcion.Editar(ConexionBaseDatos);
                string JsonOpcion = Opcion.ObtenerJsonArbol(ConexionBaseDatos);

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return respuesta = "0|" + JsonOpcion;
            }
        }
        else
        {
            return respuesta;
        }
    }

    [WebMethod]
    public static string EliminarOpcion(int pIdOpcion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            COpcion Opcion = new COpcion();
            Opcion.IdOpcion = pIdOpcion;
            Opcion.Eliminar(ConexionBaseDatos);
            string JsonOpcion = Opcion.ObtenerJsonArbol(ConexionBaseDatos);
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            respuesta = "0|" + JsonOpcion;
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerJsonArbolOpciones()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            COpcion Opcion = new COpcion();
            string JsonOpcion = Opcion.ObtenerJsonArbol(ConexionBaseDatos);
            respuesta = JsonOpcion;
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidaOpcion(int pIdOpcion, string pOpcion, string pComando, CConexion pConexion)
    {
        string errores = "";

        if (pOpcion == "")
        { errores = errores + "<span>*</span> El campo opci&oacute;n esta vacío, favor de capturarlo.<br />"; }
        if (pComando == "")
        { errores = errores + "<span>*</span> El campo comando esta vacío, favor de capturarlo.<br />"; }
        if (COpcion.ExisteOpcion(pIdOpcion, pOpcion, pConexion))
        { errores = errores + "<span>*</span> El campo opción ya existe, favor de cambiar la descripción.<br />"; }
        if (COpcion.ExisteComando(pIdOpcion, pComando, pConexion))
        { errores = errores + "<span>*</span> El campo comando ya existe, favor de cambiar la descripción.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}