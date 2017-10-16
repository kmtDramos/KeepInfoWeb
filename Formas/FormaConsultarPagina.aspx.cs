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

public partial class FormaConsultarPagina : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idPagina = Convert.ToInt32(this.Request.Params["IdPagina"]);
            CPagina Pagina = new CPagina();
            Pagina.LlenaObjeto(idPagina, ConexionBaseDatos);
            lblPaginaConsulta.Text = Pagina.Pagina;
            lblTituloConsulta.Text = Pagina.Titulo;
            lblNombreMenuConsulta.Text = Pagina.NombreMenu;
            chkValidarSucursal.Checked = Pagina.ValidarSucursal;

            CMenu MenuPadre = new CMenu();
            MenuPadre.LlenaObjeto(Pagina.IdMenu, ConexionBaseDatos);
            lblMenuPadreConsulta.Text = MenuPadre.Menu;

            //Agreagar atributos en etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idPagina", Convert.ToString(Pagina.IdPagina));
            divFormulario.Attributes.Add("pagina", Pagina.Pagina);

            HtmlInputControl btnEditar = Page.FindControl("btnEditar") as HtmlInputControl;
            string onclickFormaEditarPagina = "javascript:SetFormaEditarPagina(" + Convert.ToString(Pagina.IdPagina) + ")";
            btnEditar.Attributes.Add("onclick", onclickFormaEditarPagina);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}