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

public partial class FormaConsultarUsuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idUsuario = Convert.ToInt32(HttpContext.Current.Request["IdUsuario"]);
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(idUsuario, ConexionBaseDatos);
            lblNombreConsulta.Text = Usuario.Nombre;
            lblApellidoPaternoConsulta.Text = Usuario.ApellidoPaterno;
            lblApellidoMaternoConsulta.Text = Usuario.ApellidoMaterno;
            lblFechaNacimientoConsulta.Text = Convert.ToString(Usuario.FechaNacimiento.ToShortDateString());
            lblUsuarioConsulta.Text = Usuario.Usuario;
            lblContrasenaConsulta.Text = "******";
            lblCorreoConsulta.Text = Usuario.Correo;
            chkEsAgente.Checked = Usuario.EsAgente;
			chkEsVendedor.Checked = Usuario.EsVendedor;
            lblAlcance1Consulta.Text = Convert.ToString(Usuario.Alcance1);
            lblAlcance2Consulta.Text = Convert.ToString(Usuario.Alcance2);
            lblMetaConsulta.Text = Convert.ToString(Usuario.Meta);
            lblClientesNuevosConsulta.Text = Convert.ToString(Usuario.ClientesNuevos);

            CPerfil PerfilUsuario = new CPerfil();
            PerfilUsuario.LlenaObjeto(Usuario.IdPerfil, ConexionBaseDatos);
            lblPerfilConsulta.Text = PerfilUsuario.Perfil;

            //Agreagar atributos en etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idUsuario", Convert.ToString(Usuario.IdUsuario));
            divFormulario.Attributes.Add("usuario", Usuario.Usuario);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}