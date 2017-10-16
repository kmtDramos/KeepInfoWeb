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

public partial class FormaConsultarPerfil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idPerfil = Convert.ToInt32(this.Request.Params["IdPerfil"]);
            CPerfil Perfil = new CPerfil();
            Perfil.LlenaObjeto(idPerfil, ConexionBaseDatos);
            lblPerfilConsulta.Text = Perfil.Perfil;
            chkEsPerfilSucursal.Checked = Perfil.EsPerfilSucursal;

            CPagina Pagina = new CPagina();
            Pagina.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);
            lblPaginaConsulta.Text = Pagina.Titulo;

            //Agreagar valores a etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idPerfil", Convert.ToString(Perfil.IdPerfil));
            divFormulario.Attributes.Add("perfil", Perfil.Perfil);

            HtmlInputControl btnEditar = Page.FindControl("btnEditar") as HtmlInputControl;
            string onclickFormaEditarPerfil = "javascript:SetFormaEditarPerfil(" + Convert.ToString(Perfil.IdPerfil) + ")";
            btnEditar.Attributes.Add("onclick", onclickFormaEditarPerfil);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}