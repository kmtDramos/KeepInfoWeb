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

public partial class ExportacionesExcel_ExportarExcelReporteComprasPedido : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{
        string fechaInicio = HttpContext.Current.Request.QueryString["fechaInicio"].ToString();
        string fechaFin = HttpContext.Current.Request.QueryString["fechaFinal"].ToString();
        ExportarReporte(fechaInicio, fechaFin);
	}

	private void ExportarReporte(string Inicio, string Fin)
	{
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("sp_Exportar_OrdenCompraPedido", sqlCon);
        Stored.Parameters.Add("FechaInicial", SqlDbType.VarChar, 255).Value = Inicio;//"10/06/2018";// Convert.ToString(Inicio);
        Stored.Parameters.Add("FechaFinal", SqlDbType.VarChar, 255).Value = Fin;// "11/07/2018";// Convert.ToString(Fin);

        Stored.CommandType = CommandType.StoredProcedure;

		DataTable dataSet = new DataTable();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		tblDatos.DataSource = dataSet;
		tblDatos.DataBind();
		Response.ClearContent();
		Response.AddHeader("content-disposition", "attachment; filename=ReporteOrdenesCompraPedido.xls");
		Response.ContentType = "application/excel";
	}

}