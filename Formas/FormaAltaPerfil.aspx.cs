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
using System.Web.Services;
using System.Xml.Linq;

public partial class FormaAltaPerfil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtPerfil.Attributes.Add("onkeypress", "javascript:return DesactivarEnvio(event, this.value);");
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            //Genera cmbPagina

        }
        ConexionBaseDatos.CerrarBaseDatos();
    }

    public string ComboPagina()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CComboBox ComboPagina = new CComboBox();
            ComboPagina.StoredProcedure.CommandText = "sp_Pagina_Consulta";
            ComboPagina.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ComboPagina.Columnas = new string[] { "IdPagina", "Titulo" };
            ComboPagina.CssClase = "cmbComboBox";
            ComboPagina.Nombre = "cmbPagina";
            ComboPagina.ValorInicio = "Seleccione una opcion...";
            ComboPagina.TabIndex = "3";
            respuesta = ComboPagina.GeneraCombo(ConexionBaseDatos);
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }
}