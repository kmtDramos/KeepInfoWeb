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

public partial class FormaEditarMenu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtMenu.Attributes.Add("onkeypress", "javascript:return DesactivarEnvio(event, this.value);");
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idMenu = Convert.ToInt32(this.Request.Params["IdMenu"]);
            CMenu Menu = new CMenu();
            Menu.LlenaObjeto(idMenu, ConexionBaseDatos);
            txtMenu.Text = Menu.Menu;

            //Agreagar valores a etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idMenu", Convert.ToString(Menu.IdMenu));
            divFormulario.Attributes.Add("menu", Menu.Menu);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }

    public string ComboProyectoSistema()
    {
        //Llena etiquetas con la informacion solicitada
        int idMenu = Convert.ToInt32(this.Request.Params["IdMenu"]);

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CMenu Menu = new CMenu();
            Menu.LlenaObjeto(idMenu, ConexionBaseDatos);

            CComboBox ComboProyectoSistema = new CComboBox();
            ComboProyectoSistema.StoredProcedure.CommandText = "spb_ProyectoSistema_Consultar";
            ComboProyectoSistema.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ComboProyectoSistema.Columnas = new string[] { "IdProyectoSistema", "ProyectoSistema" };
            ComboProyectoSistema.CssClase = "cmbComboBox";
            ComboProyectoSistema.Nombre = "cmbProyectoSistema";
            ComboProyectoSistema.ValorInicio = "Eligir una opción...";
            ComboProyectoSistema.TabIndex = "3";
            ComboProyectoSistema.OpcionSeleccionada = Menu.IdProyectoSistema;
            respuesta = ComboProyectoSistema.GeneraCombo(ConexionBaseDatos);
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }
}