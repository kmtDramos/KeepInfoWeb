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

public partial class FormaConsultarOpcion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            //Llena etiquetas con la informacion solicitada
            int idOpcion = Convert.ToInt32(this.Request.Params["IdOpcion"]);
            COpcion Opcion = new COpcion();
            Opcion.LlenaObjeto(idOpcion, ConexionBaseDatos);
            lblOpcionConsulta.Text = Opcion.Opcion;
            lblComandoConsulta.Text = Opcion.Comando;

            //Agreagar atributos en etiquetas
            HtmlGenericControl divFormulario = Page.FindControl("divFormulario") as HtmlGenericControl;
            divFormulario.Attributes.Add("idOpcion", Convert.ToString(Opcion.IdOpcion));
            divFormulario.Attributes.Add("opcion", Opcion.Opcion);

            HtmlInputControl btnEditar = Page.FindControl("btnEditar") as HtmlInputControl;
            string onclickFormaEditarOpcion = "javascript:SetFormaEditarOpcion(" + Convert.ToString(Opcion.IdOpcion) + ")";
            btnEditar.Attributes.Add("onclick", onclickFormaEditarOpcion);
        }
        ConexionBaseDatos.CerrarBaseDatos();
    }
}