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

public partial class FormaEditarPagina : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        if (respuesta == "Conexion Establecida")
        {
            int idPagina = Convert.ToInt32(this.Request.Params["IdPagina"]);
            CPagina Pagina = new CPagina();
            Pagina.LlenaObjeto(idPagina, ConexionBaseDatos);
            txtPagina.Text = Pagina.Pagina;
            txtTitulo.Text = Pagina.Titulo;
            txtNombreMenu.Text = Pagina.NombreMenu;
            chkValidarSucursal.Checked = Pagina.ValidarSucursal;

            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idPagina", Convert.ToString(Pagina.IdPagina));
            divFormulario.Attributes.Add("pagina", Pagina.Pagina);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }

    public string ComboMenu()
    {
        //Llena etiquetas con la informacion solicitada
        int idPagina = Convert.ToInt32(this.Request.Params["IdPagina"]);

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CPagina Pagina = new CPagina();
            Pagina.LlenaObjeto(idPagina, ConexionBaseDatos);

            CComboBox ComboMenu = new CComboBox();
            ComboMenu.StoredProcedure.CommandText = "sp_Menu_Consulta";
            ComboMenu.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ComboMenu.Columnas = new string[] { "IdMenu", "Menu" };
            ComboMenu.CssClase = "cmbComboBox";
            ComboMenu.Nombre = "cmbMenu";
            ComboMenu.ValorInicio = "Eligir una opción...";
            ComboMenu.TabIndex = "4";
            ComboMenu.OpcionSeleccionada = Pagina.IdMenu;
            respuesta = ComboMenu.GeneraCombo(ConexionBaseDatos);
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }
}