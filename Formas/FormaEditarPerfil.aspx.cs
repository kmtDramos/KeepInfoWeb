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

public partial class FormaEditarPerfil : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtPerfil.Attributes.Add("onkeypress", "javascript:return DesactivarEnvio(event, this.value);");
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idPerfil = Convert.ToInt32(this.Request.Params["IdPerfil"]);
            CPerfil Perfil = new CPerfil();
            Perfil.LlenaObjeto(idPerfil, ConexionBaseDatos);
            txtPerfil.Text = Perfil.Perfil;
            chkEsPerfilSucursal.Checked = Perfil.EsPerfilSucursal;

            //Agreagar valores a etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idPerfil", Convert.ToString(Perfil.IdPerfil));
            divFormulario.Attributes.Add("perfil", Perfil.Perfil);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }

    public string ComboPagina()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int idPerfil = Convert.ToInt32(this.Request.Params["IdPerfil"]);
        CPerfil Perfil = new CPerfil();
        Perfil.LlenaObjeto(idPerfil, ConexionBaseDatos);
        CPagina Pagina = new CPagina();
        Pagina.LlenaObjeto(Perfil.IdPagina, ConexionBaseDatos);

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
            ComboPagina.OpcionSeleccionada = Pagina.IdPagina;
            respuesta = ComboPagina.GeneraCombo(ConexionBaseDatos);
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }
}