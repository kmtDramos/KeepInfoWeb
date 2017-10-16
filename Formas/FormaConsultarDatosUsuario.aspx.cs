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

public partial class FormaConsultarDatosUsuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(idUsuario, ConexionBaseDatos);
            txtDatosNombre.Text = Usuario.Nombre;
            txtDatosApellidoPaterno.Text = Usuario.ApellidoPaterno;
            txtDatosApellidoMaterno.Text = Usuario.ApellidoMaterno;
            txtDatosUsuario.Text = Usuario.Usuario;
            txtDatosFechaNacimiento.Text = Usuario.FechaNacimiento.ToShortDateString();
            txtDatosCorreo.Text = Usuario.Correo;
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}