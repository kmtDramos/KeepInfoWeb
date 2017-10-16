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
using Newtonsoft.Json.Linq;

public partial class ExportacionesExcel_ExportarExcelReporteAtencionClientes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ExportarAtencionClientesExcel();
    }

    private void ExportarAtencionClientesExcel()
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_Reporte_AtencionClientes", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("IdEmpresa", SqlDbType.VarChar, 255).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ReporteAtencionClientes.xls");
        Response.ContentType = "application/excel";
    }
    //******************************************************
}
