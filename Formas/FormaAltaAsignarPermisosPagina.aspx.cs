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

public partial class FormaAltaAsignarPermisosPagina : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int idPagina = Convert.ToInt32(this.Request.Params["IdPagina"]);
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idPagina", idPagina.ToString());

            HtmlGenericControl ulPermisosAsignados = Page.FindControl("ulPermisosAsignados") as HtmlGenericControl;
            string liPermisosAsignados = "";
            CPaginaPermiso PermisosAsignados = new CPaginaPermiso();
            foreach (CPaginaPermiso CPP in PermisosAsignados.PermisosAsignados(idPagina, ConexionBaseDatos))
            {
                COpcion Opcion = new COpcion();
                Opcion.LlenaObjeto(CPP.IdOpcion, ConexionBaseDatos);
                liPermisosAsignados = liPermisosAsignados + "<li title='" + Opcion.Comando + "' opcion='" + Opcion.IdOpcion + "'>" + Opcion.Opcion + "</li>";
            }
            ulPermisosAsignados.InnerHtml = liPermisosAsignados;

            HtmlGenericControl ulPermisosDisponibles = Page.FindControl("ulPermisosDisponibles") as HtmlGenericControl;
            string liPermisosDisponibles = "";
            COpcion PermisosDisponibles = new COpcion();
            foreach (COpcion Opcion in PermisosDisponibles.PaginaPermisosDisponibles(idPagina, ConexionBaseDatos))
            {
                liPermisosDisponibles = liPermisosDisponibles + "<li title='" + Opcion.Comando + "' opcion='" + Opcion.IdOpcion + "'>" + Opcion.Opcion + "</li>";
            }
            ulPermisosDisponibles.InnerHtml = liPermisosDisponibles;
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}