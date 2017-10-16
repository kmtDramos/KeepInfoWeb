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

public partial class ExportacionesExcel_ExportarExcelOportunidades : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] separador = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('/');
        string pagina = separador[separador.Length - 1];

        switch (pagina)
        {
            case "Oportunidad.aspx":
                string pIdOportunidad = Convert.ToString(HttpContext.Current.Request.QueryString["pIdOportunidad"]);
                string pOportunidad = Convert.ToString(HttpContext.Current.Request.QueryString["pOportunidad"]);
                string pAgente = Convert.ToString(HttpContext.Current.Request.QueryString["pAgente"]);
                string pCliente = Convert.ToString(HttpContext.Current.Request.QueryString["pCliente"]);
                int pNivelInteres = Convert.ToInt32(HttpContext.Current.Request.QueryString["pNivelInteres"]);
                int pIdSucursal = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdSucursal"]);
                string pMonto = Convert.ToString(HttpContext.Current.Request.QueryString["pMonto"]);
                int pIdDivision = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdDivision"]);
                int pCerrado = Convert.ToInt32(HttpContext.Current.Request.QueryString["pCerrado"]);
                int pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                ExportarOportunidades(pIdOportunidad, pOportunidad, pAgente, pCliente, pNivelInteres, pIdSucursal, pMonto, pIdDivision, pCerrado, pAI);
                break;
        }

    }

    private void ExportarOportunidades(string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pIdSucursal, string pMonto, int pIdDivision, int pCerrado, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_ExportarOportunidad", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar,255).Value = pIdOportunidad;
        Stored.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = pOportunidad;
        Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
        Stored.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
        Stored.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pMonto", SqlDbType.VarChar, 255).Value = pMonto;
        Stored.Parameters.Add("pDivision", SqlDbType.Int).Value = pIdDivision;
        Stored.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=Oportunidades"+ DateTime.Now.ToShortDateString() +".xls");
        Response.ContentType = "application/excel";
    }
}
