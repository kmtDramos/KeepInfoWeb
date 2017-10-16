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


public partial class FormaInicioAsignarPermisos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Agreagar atributos en etiquetas
        string mensaje = this.Request.Params["Mensaje"];
        HtmlGenericControl divMensaje = Page.FindControl("divMensaje") as HtmlGenericControl;
        divMensaje.InnerText = mensaje;
    }
}

