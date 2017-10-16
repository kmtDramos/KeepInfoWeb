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

public partial class FormaAltaAsignarPermisosPerfil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int idPerfil = Convert.ToInt32(this.Request.Params["IdPerfil"]);
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idPerfil", idPerfil.ToString());

            HtmlGenericControl ulPermisosAsignados = Page.FindControl("ulPermisosAsignados") as HtmlGenericControl;
            string liPermisosAsignados = "";
            CPerfilPermiso PermisosAsignados = new CPerfilPermiso();
            foreach (CPerfilPermiso CPP in PermisosAsignados.PermisosAsignados(idPerfil, ConexionBaseDatos))
            {
                COpcion Opcion = new COpcion();
                Opcion.LlenaObjeto(CPP.IdOpcion, ConexionBaseDatos);
                liPermisosAsignados = liPermisosAsignados + "<li title='" + Opcion.Comando + "' opcion='" + Opcion.IdOpcion + "'>" + Opcion.Opcion + "</li>";
            }
            ulPermisosAsignados.InnerHtml = liPermisosAsignados;

            HtmlGenericControl ulPermisosDisponibles = Page.FindControl("ulPermisosDisponibles") as HtmlGenericControl;
            string liPermisosDisponibles = "";
            COpcion PermisosDisponibles = new COpcion();
            foreach (COpcion Opcion in PermisosDisponibles.PerfilPermisosDisponibles(idPerfil, ConexionBaseDatos))
            {
                liPermisosDisponibles = liPermisosDisponibles + "<li title='" + Opcion.Comando + "' opcion='" + Opcion.IdOpcion + "'>" + Opcion.Opcion + "</li>";
            }
            ulPermisosDisponibles.InnerHtml = liPermisosDisponibles;
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}