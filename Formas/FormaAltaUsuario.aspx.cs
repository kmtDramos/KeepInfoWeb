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

public partial class FormaAltaUsuario : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime FechaActual = DateTime.Now;
        FechaActual = FechaActual.ToUniversalTime();
        txtFechaNacimiento.Text = Convert.ToString(FechaActual.ToShortDateString());
    }

    public string ComboPerfil()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
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
                ComboPerfil.ValorInicio = "Eligir una opción...";
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
                ComboPerfil.ValorInicio = "Eligir una opción...";
                respuesta = ComboPerfil.GeneraCombo(ConexionBaseDatos);
            }
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }
}