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

public partial class FormaAltaPagina : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public string ComboMenu()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CComboBox ComboMenu = new CComboBox();
            ComboMenu.StoredProcedure.CommandText = "sp_Menu_Consulta";
            ComboMenu.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ComboMenu.Columnas = new string[] { "IdMenu", "Menu" };
            ComboMenu.CssClase = "cmbComboBox";
            ComboMenu.Nombre = "cmbMenu";
            ComboMenu.ValorInicio = "Eligir una opción...";
            ComboMenu.TabIndex = "4";
            respuesta = ComboMenu.GeneraCombo(ConexionBaseDatos);
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }
}