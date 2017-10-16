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

public partial class FormaEditarOpcion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        if (respuesta == "Conexion Establecida")
        {
            int idOpcion = Convert.ToInt32(this.Request.Params["IdOpcion"]);
            COpcion Opcion = new COpcion();
            Opcion.LlenaObjeto(idOpcion, ConexionBaseDatos);
            txtOpcion.Text = Opcion.Opcion;
            txtComando.Text = Opcion.Comando;
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idOpcion", Convert.ToString(Opcion.IdOpcion));
            divFormulario.Attributes.Add("opcion", Opcion.Opcion);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}