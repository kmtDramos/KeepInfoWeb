using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using System.Text;

public partial class ExportarSeguimientosCliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] separador = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('/');
        string pagina = separador[separador.Length - 1];
        switch (pagina)
        {
            case "GestionCobranza.aspx":
                int pIdCliente = 0;
                pIdCliente = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdCliente"]);

                ExportarSeguimientosGestionCobranzaCliente(pIdCliente);
                break;
        }
    }

    private void ExportarSeguimientosGestionCobranzaCliente(int pIdCliente)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand sp_GestionCobranzaDetalleCliente = new SqlCommand("sp_GestionCobranzaDetalle_Consultar_ExportarSeguimientosCliente", sqlCon);
        sp_GestionCobranzaDetalleCliente.CommandType = CommandType.StoredProcedure;
        sp_GestionCobranzaDetalleCliente.Parameters.AddWithValue("@pIdCliente", pIdCliente);

        DataTable tablaSeguimientosGestionCobranza = new DataTable();
        SqlDataAdapter obtenerTabla = new SqlDataAdapter(sp_GestionCobranzaDetalleCliente);
        obtenerTabla.Fill(tablaSeguimientosGestionCobranza);
        tblDatos.DataSource = tablaSeguimientosGestionCobranza;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionSeguimientosCliente.xls");
        Response.ContentType = "application/excel";
    }
}