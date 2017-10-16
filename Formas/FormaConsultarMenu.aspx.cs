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

public partial class FormaConsultarMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idMenu = Convert.ToInt32(this.Request.Params["IdMenu"]);
            CMenu Menu = new CMenu();
            Menu.LlenaObjeto(idMenu, ConexionBaseDatos);
            lblMenuConsulta.Text = Menu.Menu;

            CProyectoSistema ProyectoSistemaMenu = new CProyectoSistema();
            ProyectoSistemaMenu.LlenaObjeto(Menu.IdProyectoSistema, ConexionBaseDatos);
            lblProyectoSistemaConsulta.Text = ProyectoSistemaMenu.ProyectoSistema;

            //Agreagar atributos en etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idMenu", Convert.ToString(Menu.IdMenu));
            divFormulario.Attributes.Add("menu", Menu.Menu);

            HtmlInputControl btnEditar = Page.FindControl("btnEditar") as HtmlInputControl;
            string onclickFormaEditarMenu = "javascript:SetFormaEditarMenu(" + Convert.ToString(Menu.IdMenu) + ")";
            btnEditar.Attributes.Add("onclick", onclickFormaEditarMenu);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}