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

public partial class FormaEditarUsuario : System.Web.UI.Page
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
            txtNombre.Text = Usuario.Nombre;
            txtApellidoPaterno.Text = Usuario.ApellidoPaterno;
            txtApellidoMaterno.Text = Usuario.ApellidoMaterno;
            txtFechaNacimiento.Text = Convert.ToString(Usuario.FechaNacimiento.ToShortDateString());
            txtUsuario.Text = Usuario.Usuario;
            txtCorreo.Text = Usuario.Correo;
            chkEsAgente.Checked = Usuario.EsAgente;
			chkEsVendedor.Checked = Usuario.EsVendedor;
            txtAlcance1.Text = Convert.ToString(Usuario.Alcance1);
            txtAlcance2.Text = Convert.ToString(Usuario.Alcance2);
            txtMeta.Text = Convert.ToString(Usuario.Meta);
            txtClientesNuevos.Text = Convert.ToString(Usuario.ClientesNuevos);

            //Agreagar atributos en etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idUsuario", Convert.ToString(Usuario.IdUsuario));
            divFormulario.Attributes.Add("Usuario", Usuario.Usuario);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }

    public string ComboPerfil()
    {
        //Llena etiquetas con la informacion solicitada
        int idUsuario = Convert.ToInt32(HttpContext.Current.Request["IdUsuario"]);

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario PerfilUsuario = new CUsuario();
            PerfilUsuario.LlenaObjeto(idUsuario, ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (Usuario.IdPerfil == 1)
            {
                CComboBox ComboPerfil = new CComboBox();
                ComboPerfil.StoredProcedure.CommandText = "sp_Perfil_Consulta";
                ComboPerfil.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
                ComboPerfil.Columnas = new string[] { "IdPerfil", "Perfil" };
                ComboPerfil.CssClase = "cmbComboBox";
                ComboPerfil.Nombre = "cmbPerfil";
                ComboPerfil.ValorInicio = "Elegir una opción...";
                ComboPerfil.TabIndex = "7";
                ComboPerfil.OpcionSeleccionada = PerfilUsuario.IdPerfil;
                respuesta = ComboPerfil.GeneraCombo(ConexionBaseDatos);
            }
            else
            {
                CComboBox ComboPerfil = new CComboBox();
                ComboPerfil.StoredProcedure.CommandText = "sp_Perfil_Consulta";
                ComboPerfil.StoredProcedure.Parameters.AddWithValue("@Opcion", 6);
                ComboPerfil.Columnas = new string[] { "IdPerfil", "Perfil" };
                ComboPerfil.CssClase = "cmbComboBox";
                ComboPerfil.Nombre = "cmbPerfil";
                ComboPerfil.ValorInicio = "Elegir una opción...";
                ComboPerfil.TabIndex = "7";
                ComboPerfil.OpcionSeleccionada = PerfilUsuario.IdPerfil;
                respuesta = ComboPerfil.GeneraCombo(ConexionBaseDatos);
            }
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }
}