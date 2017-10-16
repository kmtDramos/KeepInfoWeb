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

public partial class ExportacionesExcel_ExportarExcelFacturaDetalle : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{
		string pColumnaOrden = Convert.ToString(HttpContext.Current.Request.QueryString["pColumnaOrden"]);
		string pTipoOrden = Convert.ToString(HttpContext.Current.Request.QueryString["pTipoOrden"]);
		string pSerieFactura = Convert.ToString(HttpContext.Current.Request.QueryString["pSerieFactura"]);
		string pNumeroFactura = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroFactura"]);
		string pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
		string pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
		string pPorFecha = Convert.ToString(HttpContext.Current.Request.QueryString["pPorFecha"]);
		string pAI = Convert.ToString(HttpContext.Current.Request.QueryString["pAI"]);
		string pFiltroTimbrado = Convert.ToString(HttpContext.Current.Request.QueryString["pFiltroTimbrado"]);
		string pIdDivision = Convert.ToString(HttpContext.Current.Request.QueryString["pIdDivision"]);
		string pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
		string pAgente = Convert.ToString(HttpContext.Current.Request.QueryString["pAgente"]);
		string pBusquedaDocumento = Convert.ToString(HttpContext.Current.Request.QueryString["pBusquedaDocumento"]);
		string pNumeroPedido = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroPedido"]);
		string pEstatusFacturaEncabezado = Convert.ToString(HttpContext.Current.Request.QueryString["pEstatusFacturaEncabezado"]);
		string pDescripcion = Convert.ToString(HttpContext.Current.Request.QueryString["pDescripcion"]);
		string pClave = Convert.ToString(HttpContext.Current.Request.QueryString["pClave"]);

		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdFacturas_Detalle_Exportar", sqlCon);
		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieFactura);
		Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
		Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
		Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
		Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
		Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
		Stored.Parameters.Add("pFiltroTimbrado", SqlDbType.Int).Value = pFiltroTimbrado;
		Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
		Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
		Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
		Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
		Stored.Parameters.Add("pBusquedaDocumento", SqlDbType.Int).Value = pBusquedaDocumento;
		Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
		Stored.Parameters.Add("pEstatusFacturaEncabezado", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstatusFacturaEncabezado);
		Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 250).Value = pDescripcion;
		Stored.Parameters.Add("pClave", SqlDbType.VarChar, 250).Value = pClave;

		DataTable dataSet = new DataTable();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		tblDatos.DataSource = dataSet;
		tblDatos.DataBind();
		Response.ClearContent();
		Response.AddHeader("content-disposition", "attachment; filename=DetalleFacturas_" + DateTime.Now.ToShortDateString() + ".xls");
		Response.ContentType = "application/excel";


	}

}