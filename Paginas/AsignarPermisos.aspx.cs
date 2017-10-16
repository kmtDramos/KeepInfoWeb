using System;
using System.Collections;
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

public partial class AsignarPermisos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //Metodos Ajax
    [WebMethod]
    public static string JsonArbolPermisos()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            COpcion Opcion = new COpcion();
            string JsonPermisos = Opcion.ObtenerJsonArbolPermisos(ConexionBaseDatos);
            respuesta = JsonPermisos;
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string AgregarPermisosPerfil(string pOpciones, int pIdPerfil)
    {
        string respuesta = "";
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPerfilPermiso PerfilPermiso = new CPerfilPermiso();
            PerfilPermiso.IdPerfil = pIdPerfil;
            PerfilPermiso.DesactivarTodos(ConexionBaseDatos);
            if (pOpciones != "")
            {
                string[] arrOpciones = pOpciones.Split('|');
                ArrayList opciones = new ArrayList();
                foreach (string opcion in arrOpciones)
                {
                    opciones.Add(opcion);
                }
                pOpciones = pOpciones.Replace("|", ",");
                foreach (CPerfilPermiso CPP in PerfilPermiso.PermisosExistentes(pOpciones, pIdPerfil, ConexionBaseDatos))
                {
                    for (int i = 0; i <= opciones.Count - 1; i++)
                    {
                        if (CPP.IdOpcion == Convert.ToInt32(opciones[i]))
                        {
                            opciones.RemoveAt(i);
                            break;
                        }
                    }
                    CPP.Activar(ConexionBaseDatos);
                }

                for (int i = 0; i <= opciones.Count - 1; i++)
                {
                    PerfilPermiso.IdOpcion = Convert.ToInt32(opciones[i]);
                    PerfilPermiso.Agregar(ConexionBaseDatos);
                }
                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
        }
        return respuesta;
    }

    [WebMethod]
    public static string AgregarPermisosPagina(string pOpciones, int pPagina)
    {
        string respuesta = "";
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPaginaPermiso PaginaPermiso = new CPaginaPermiso();
            PaginaPermiso.IdPagina = pPagina;
            PaginaPermiso.DesactivarTodos(ConexionBaseDatos);
            if (pOpciones != "")
            {
                string[] arrOpciones = pOpciones.Split('|');
                ArrayList opciones = new ArrayList();
                foreach (string opcion in arrOpciones)
                {
                    opciones.Add(opcion);
                }
                pOpciones = pOpciones.Replace("|", ",");
                foreach (CPaginaPermiso CPP in PaginaPermiso.PermisosExistentes(pOpciones, ConexionBaseDatos))
                {
                    for (int i = 0; i <= opciones.Count - 1; i++)
                    {
                        if (CPP.IdOpcion == Convert.ToInt32(opciones[i]))
                        {
                            opciones.RemoveAt(i);
                            break;
                        }
                    }
                    CPP.Activar(ConexionBaseDatos);
                }

                for (int i = 0; i <= opciones.Count - 1; i++)
                {
                    PaginaPermiso.IdOpcion = Convert.ToInt32(opciones[i]);
                    PaginaPermiso.Agregar(ConexionBaseDatos);
                }
                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
        }
        return respuesta;
    }
}