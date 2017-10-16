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

public partial class FormaConsultarAsignarPermisosPagina : System.Web.UI.Page
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

            HtmlGenericControl ulPermisosAsignados = Page.FindControl("ulConsultaPermisosAsignados") as HtmlGenericControl;
            string liPermisosAsignados = "";
            CPaginaPermiso PermisosAsignados = new CPaginaPermiso();
            foreach (CPaginaPermiso CPP in PermisosAsignados.PermisosAsignados(idPagina, ConexionBaseDatos))
            {
                COpcion Opcion = new COpcion();
                Opcion.LlenaObjeto(CPP.IdOpcion, ConexionBaseDatos);
                liPermisosAsignados = liPermisosAsignados + "<li title='" + Opcion.Comando + "' opcion='" + Opcion.IdOpcion + "'>" + Opcion.Opcion + "</li>";
            }
            ulPermisosAsignados.InnerHtml = liPermisosAsignados;

            HtmlInputControl btnEditar = Page.FindControl("btnEditar") as HtmlInputControl;
            string onclickFormaAltaAsignarPermisosPagina = "javascript:SetFormaAltaAsignarPermisosPagina(" + idPagina + ")";
            btnEditar.Attributes.Add("onclick", onclickFormaAltaAsignarPermisosPagina);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}