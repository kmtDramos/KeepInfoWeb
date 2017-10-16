using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
//using Oracle.DataAccess.Client;
using System.Web;


public class CConexion
{
    //Atributos
    private string servidor;
    private string baseDatos;
    private string usuario;
    private string contrasena;
    private OleDbConnection conexionBaseDatosAccess = new OleDbConnection();
    private string directorioRaiz = HttpContext.Current.Server.MapPath("~");
    private SqlConnection conexionBaseDatosSqlServer;
    //private OracleConnection conexionBaseDatosOracle;

    //Propiedades
    public string Servidor
    {
        get { return servidor; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            servidor = value;
        }
    }

    public string BaseDatos
    {
        get { return baseDatos; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            baseDatos = value;
        }
    }

    public string Usuario
    {
        get { return Usuario; }
        set
        {   
               if (value.Length == 0)
            {
                return;
            }
            usuario = value;
        }
    }

    public string Contrasena
    {
        get { return contrasena; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            contrasena = value;
        }
    }

    public OleDbConnection ConexionBaseDatosAccess
    {
        get { return conexionBaseDatosAccess; }
    }

    public SqlConnection ConexionBaseDatosSqlServer
    {
        get { return conexionBaseDatosSqlServer; }
    }

    //Metodos
    public string ConectarBaseDatos()
    {
        string stringConexion;
        stringConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + directorioRaiz + "/BaseDatos/BaseDatosArqNet.mdb;";
        conexionBaseDatosAccess.ConnectionString = stringConexion;
        try
        {
            conexionBaseDatosAccess.Open();
            return "Conexion Establecida";
        }
        catch (OleDbException exception)
        {
            string erroresConexion = "Hubo un error en la conexi&oacute;, intentelo de nuevo.";
            foreach (OleDbError error in exception.Errors)
            {
                erroresConexion = erroresConexion + error.Message + "<br />";
            }
            return "1|Hubo un error en la conexi&oacute;, intentelo de nuevo.";
        }
    }

    public string ConectarBaseDatos(string paramRutaBaseDatos)
    {
        string stringConexion;
        stringConexion = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + directorioRaiz + paramRutaBaseDatos;
        conexionBaseDatosAccess.ConnectionString = stringConexion;
        try
        {
            conexionBaseDatosAccess.Open();
            return "Conexion Establecida";
        }
        catch (OleDbException exception)
        {
            string erroresConexion = "Imposible conectar con la fuente de datos.<br />";
            foreach (OleDbError error in exception.Errors)
            {
                erroresConexion = erroresConexion + error.Message + "<br />";
            }
            return "1|Hubo un error al tratar de conectarse al servidor, intentelo de nuevo. Si la falla persiste contacte al administrador.";
        }
    }

    public string ConectarBaseDatosSqlServer()
    {
        conexionBaseDatosSqlServer = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        try
        {
            conexionBaseDatosSqlServer.Open();
            return "Conexion Establecida";
        }
        catch (SqlException exception)
        {
            string erroresConexion = "1|Hubo un error en la conexi&oacute;n, int&eacute;ntelo de nuevo.";
            foreach (SqlError error in exception.Errors)
            {
                erroresConexion = erroresConexion + error.Message + "<br />";
            }
            return erroresConexion;
        }
    }

    //public string ConectarBaseDatosOracle()
    //{
    //    conexionBaseDatosOracle = new OracleConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
    //    try
    //    {
    //        conexionBaseDatosSqlServer.Open();
    //        return "Conexion Establecida";
    //    }
    //    catch (OracleException exception)
    //    {
    //        string erroresConexion = "1|Hubo un error en la conexi&oacute;n, int&eacute;ntelo de nuevo.";
    //        foreach (OracleError error in exception.Errors)
    //        {
    //            erroresConexion = erroresConexion + error.Message + "<br />";
    //        }
    //        return erroresConexion;
    //    }
    //}


    public void CerrarBaseDatos()
    {
        conexionBaseDatosSqlServer.Close();
        conexionBaseDatosAccess.Close();
    }

    public void CerrarBaseDatosSqlServer()
    {
        conexionBaseDatosSqlServer.Close();
        conexionBaseDatosAccess.Close();
    }
}