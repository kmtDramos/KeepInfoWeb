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

public partial class ExportacionesExcel_ExportarExcelReporteVentaCliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pFechaInicio = Convert.ToString(HttpContext.Current.Request["pFechaInicio"]);
        string pFechaFinal = Convert.ToString(HttpContext.Current.Request["pFechaFinal"]);
        int pIdSucursal = Convert.ToInt32(HttpContext.Current.Request["pIdSucursal"]);
        ExportarVentaClienteExcel(pFechaInicio, pFechaFinal, pIdSucursal);
    }

    private void ExportarVentaClienteExcel(string pFechaInicio, string pFechaFinal, int pIdSucursal)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_Oportunidad_VentasPorCliente_Excel", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("FechaInicio", SqlDbType.VarChar, 255).Value = pFechaInicio;
        Stored.Parameters.Add("FechaFin", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("IdEmpresa", SqlDbType.VarChar, 255).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ReporteVentasCliente.xls");
        Response.ContentType = "application/excel";
    }
    //******************************************************
}
