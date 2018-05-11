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

public partial class ExportarExcel : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] separador = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('/');
        string pagina = separador[separador.Length - 1];

        string pRazonSocial = "";
        string pNumeroFactura = "";
        string pFolio = "";
        int pIdEstatusRecepcion = 0;
        string pDivision = "";
        int pNotaCredito = 0;
        int pAI = 0;
        string pFechaInicial = "";
        string pFechaFinal = "";
        int pPorFecha = 0;
        int pIdDivision = 0;
        int pAgente = 0;
        string pAgenteCliente = "";
        string pNumeroSerie = "";
        string pNumeroPedido = "";
        int pIdEstatusRemision = 0;
        string pSerieFactura = "";
        int pFiltroTimbrado = 0;
        string pSerieNotaCredito = "";
        string pFolioNotaCredito = "";
        int pIdEstatusCotizacion = 0;
        int pIdSucursal = 0;
        int pIdReportesKeep = 0;
        int pIdFuncionalidad = 0;
        int pIdCliente = 0;
        int pEsCliente = -1;
        string pCliente = "";
		int pIdTipoCliente = -1;
		int pIdTipoIndustria = -1;
		string pRFC = "";
        int pEstatusProyecto = 0;
        int pIdSucursalReporteCrediticio = 0;
        string pEstatusFacturaEncabezado = "";
        int pIdTipoArchivo = 0;


        switch (pagina)
        {
            case "GestionCobranza.aspx":
                int pIdGestionCobranza = 0;
                pIdGestionCobranza = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdGestionCobranza"]);
                ExportarSeguimientosGestionCobranza(pIdGestionCobranza);
                break;

            case "EncabezadoFacturaProveedor.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pNumeroFactura = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroFactura"]);
                pIdEstatusRecepcion = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdEstatusRecepcion"]);
                pDivision = Convert.ToString(HttpContext.Current.Request.QueryString["pDivision"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                pNumeroSerie = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroSerie"]);
                pNumeroPedido = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroPedido"]);
                pIdTipoArchivo = Convert.ToInt16(HttpContext.Current.Request.QueryString["pIdTipoArchivo"]);
                ObtenerEncabezadoFacturaProveedorExportar(pNumeroFactura, pRazonSocial, pIdEstatusRecepcion, pDivision, pAI, pFechaInicial, pFechaFinal, pPorFecha, pNumeroSerie, pNumeroPedido, pIdTipoArchivo);
                break;

            case "OrdenCompra.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pIdEstatusRecepcion = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdEstatusRecepcion"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                pNumeroPedido = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroPedido"]);
                pIdTipoArchivo = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdTipoArchivo"]);
                ObtenerOrdenCompraExportar(pFolio, pRazonSocial, pIdEstatusRecepcion, pAI, pFechaInicial, pFechaFinal, pPorFecha, pNumeroPedido, pIdTipoArchivo);
                break;

            case "Remision.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pIdEstatusRemision = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdEstatusRemision"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                pNumeroSerie = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroSerie"]);
                pNumeroPedido = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroPedido"]);
                ObtenerRemisionExportar(pFolio, pRazonSocial, pIdEstatusRemision, pAI, pFechaInicial, pFechaFinal, pPorFecha, pNumeroSerie, pNumeroPedido);
                break;

            case "FacturaCliente.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pNumeroFactura = Convert.ToString(HttpContext.Current.Request.QueryString["pNumeroFactura"]);
                pSerieFactura = Convert.ToString(HttpContext.Current.Request.QueryString["pSerieFactura"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                pFiltroTimbrado = Convert.ToInt32(HttpContext.Current.Request.QueryString["pFiltroTimbrado"]);
                pEstatusFacturaEncabezado = Convert.ToString(HttpContext.Current.Request.QueryString["pEstatusFacturaEncabezado"]);
                ObtenerFacturasExportar(pSerieFactura, pNumeroFactura, pFechaInicial, pFechaFinal, pPorFecha, pAI, pFiltroTimbrado, pRazonSocial, pEstatusFacturaEncabezado);
                break;

            case "Oportunidad.aspx":
                pIdSucursal = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdSucursal"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicio"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pIdDivision = Convert.ToInt32(HttpContext.Current.Request.QueryString["pDivision"]);
                pNotaCredito = Convert.ToInt32(HttpContext.Current.Request.QueryString["pNotaCredito"]);
                pAgente = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAgente"]);
                ObtenerReporteComisionesExportar(pIdSucursal, pFechaInicial, pFechaFinal, pIdDivision, pNotaCredito, pAgente);
                break;

            case "NotaCredito.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolioNotaCredito = Convert.ToString(HttpContext.Current.Request.QueryString["pFolioNotaCredito"]);
                pSerieNotaCredito = Convert.ToString(HttpContext.Current.Request.QueryString["pSerieNotaCredito"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                pFiltroTimbrado = Convert.ToInt32(HttpContext.Current.Request.QueryString["pFiltroTimbrado"]);
                ObtenerNotaCreditoExportar(pSerieNotaCredito, pFolioNotaCredito, pFechaInicial, pFechaFinal, pPorFecha, pAI, pFiltroTimbrado, pRazonSocial);
                break;

            case "Cotizacion.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pIdEstatusCotizacion = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdEstatusCotizacion"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                ObtenerCotizacionExportar(pRazonSocial, pFolio, pIdEstatusCotizacion, pAI, pFechaInicial, pFechaFinal, pPorFecha);
                break;

            case "CuentasPorCobrar.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                ObtenerCuentasPorCobrarExportar(pRazonSocial, pFolio, pAI, pFechaInicial, pFechaFinal, pPorFecha);
                break;

            case "Egresos.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                ObtenerEgresosExportar(pRazonSocial, pFolio, pAI, pFechaInicial, pFechaFinal, pPorFecha);
                break;

            case "IngresosNoDepositados.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                ObtenerIngresosNoDepositados(pRazonSocial, pFolio, pAI, pFechaInicial, pFechaFinal, pPorFecha);
                break;

            case "Depositos.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                ObtenerDepositosExportar(pRazonSocial, pFolio, pAI, pFechaInicial, pFechaFinal, pPorFecha);
                break;

            case "Cheques.aspx":
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pFolio = Convert.ToString(HttpContext.Current.Request.QueryString["pFolio"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                pPorFecha = Convert.ToInt32(HttpContext.Current.Request.QueryString["pPorFecha"]);
                ObtenerChequesExportar(pRazonSocial, pFolio, pAI, pFechaInicial, pFechaFinal, pPorFecha);
                break;

            case "ReportesKeep.aspx":
                pIdReportesKeep = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdReportesKeep"]);
                pIdSucursal = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdSucursal"]);
                pFechaInicial = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaInicial"]);
                pFechaFinal = Convert.ToString(HttpContext.Current.Request.QueryString["pFechaFinal"]);
                ObtenerReporteExportar(pIdReportesKeep, pFechaInicial, pFechaFinal, pIdSucursal);
                break;

            case "ReporteEstadoCrediticioClientes.aspx":
                pIdFuncionalidad = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdFuncionalidad"]);
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pIdCliente = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdCliente"]);
                pIdSucursalReporteCrediticio = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdSucursal"]);
                switch(pIdFuncionalidad){
                    case 1:
						pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
						pIdCliente = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdCliente"]);
						pIdSucursalReporteCrediticio = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdSucursal"]);
						ObtenerReporteCrediticioClientePagos(pRazonSocial, pIdCliente, pIdSucursalReporteCrediticio);
                        break;
                    default:
						pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
						pIdCliente = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdCliente"]);
						pIdSucursalReporteCrediticio = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdSucursal"]);
						pEstatusProyecto = Convert.ToInt32(HttpContext.Current.Request.QueryString["pEstatusProyecto"]);
						ObtenerReporteCrediticioCliente(pIdFuncionalidad, pRazonSocial, pIdCliente, pEstatusProyecto, pIdSucursalReporteCrediticio);
                        break;
                }
                break;

            case "Cliente.aspx":
                pIdTipoCliente = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdTipoCliente"]);
				pIdTipoIndustria = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdTipoIndustria"]);
				pCliente = Convert.ToString(HttpContext.Current.Request.QueryString["pNombreCliente"]);
                pRazonSocial = Convert.ToString(HttpContext.Current.Request.QueryString["pRazonSocial"]);
                pRFC = Convert.ToString(HttpContext.Current.Request.QueryString["pRFC"]);
                pAgenteCliente = Convert.ToString(HttpContext.Current.Request.QueryString["pAgente"]);
                pEsCliente = Convert.ToInt32(HttpContext.Current.Request.QueryString["pCliente"]);
                pAI = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAI"]);
                ObtenerReporteClientesExportar(pIdTipoCliente, pIdTipoIndustria, pCliente, pRazonSocial, pRFC, pAgenteCliente, pEsCliente, pAI);
                break;
        }
    }

    private void ObtenerReporteClientesExportar(int pIdTipoCliente, int pIdTipoIndustria, string pCliente, string pRazonSocial, string pRFC, string pAgenteCliente, int pEsCliente, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand sp_GestionClientes = new SqlCommand("sp_Cliente_Consultar_ExportarClientes", sqlCon);
        sp_GestionClientes.CommandType = CommandType.StoredProcedure;
		sp_GestionClientes.Parameters.AddWithValue("@pIdTipoCliente", pIdTipoCliente);
		sp_GestionClientes.Parameters.AddWithValue("@pIdTipoIndustria", pIdTipoIndustria);
		sp_GestionClientes.Parameters.AddWithValue("@pCliente", pCliente);
        sp_GestionClientes.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        sp_GestionClientes.Parameters.AddWithValue("@pRFC", pRFC);
        sp_GestionClientes.Parameters.AddWithValue("@pAgente", pAgenteCliente);
        sp_GestionClientes.Parameters.AddWithValue("@pEsCliente", pEsCliente);
        sp_GestionClientes.Parameters.AddWithValue("@pAI", pAI);

        DataTable tablaClientes = new DataTable();
        SqlDataAdapter obtenerTabla = new SqlDataAdapter(sp_GestionClientes);
        obtenerTabla.Fill(tablaClientes);
        tblDatos.DataSource = tablaClientes;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionClientes.xls");
        Response.ContentType = "application/excel";
    }

    private void ExportarSeguimientosGestionCobranza(int pIdGestionCobranza)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand sp_GestionCobranza = new SqlCommand("sp_GestionCobranzaDetalle_Consultar_ExportarSeguimientosGestionCobranza", sqlCon);
        sp_GestionCobranza.CommandType = CommandType.StoredProcedure;
        sp_GestionCobranza.Parameters.AddWithValue("@pIdGestionCobranza", pIdGestionCobranza);

        DataTable tablaSeguimientosGestionCobranza = new DataTable();
        SqlDataAdapter obtenerTabla = new SqlDataAdapter(sp_GestionCobranza);
        obtenerTabla.Fill(tablaSeguimientosGestionCobranza);
        tblDatos.DataSource = tablaSeguimientosGestionCobranza;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionSeguimientosGestionCobranza.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerEncabezadoFacturaProveedorExportar(string pNumeroFactura, string pRazonSocial, int pIdEstatusRecepcion, string pDivision, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pNumeroSerie, string pNumeroPedido, int pIdTipoArchivo)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEncabezadoFacturaProveedorExcel", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;

        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pDivision", SqlDbType.VarChar, 250).Value = Convert.ToString(pDivision);
        Stored.Parameters.Add("pIdEstatusRecepcion", SqlDbType.Int).Value = pIdEstatusRecepcion;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pNumeroSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroSerie);
        Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
        Stored.Parameters.Add("pIdTipoArchivo", SqlDbType.Int).Value = pIdTipoArchivo;
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionRecepcion.xls");
        Response.ContentType = "application/excel";

    }

    private void ObtenerOrdenCompraExportar(string pFolio, string pRazonSocial, int pIdEstatusRecepcion, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pNumeroPedido, int pIdTipoArchivo)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdOrdenCompraExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("RazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("Folio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdEstatusRecepcion", SqlDbType.Int).Value = pIdEstatusRecepcion;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
        Stored.Parameters.Add("pIdTipoArchivo", SqlDbType.Int).Value = pIdTipoArchivo;
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionOrdenCompra.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerRemisionExportar(string pFolio, string pRazonSocial, int pIdEstatusRemision, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pNumeroSerie, string pNumeroPedido)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEncabezadoRemisionExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;

        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdEstatusRemision", SqlDbType.Int).Value = pIdEstatusRemision;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pNumeroSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroSerie);
        Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionRemision.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerFacturasExportar(string pSerieFactura, string pNumeroFactura, string pFechaInicial, string pFechaFinal, int pPorFecha, int pAI, int pFiltroTimbrado, string pRazonSocial, string pEstatusFacturaEncabezado)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturasExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieFactura);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pFiltroTimbrado", SqlDbType.Int).Value = pFiltroTimbrado;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pEstatusFacturaEncabezado", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstatusFacturaEncabezado);
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionFacturas.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerReporteComisionesExportar(int pIdSucursal, string pFechaInicio, string pFechaFinal, int pDivision, int pNotaCredito, int pAgente)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdReporteComisionesExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pFechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Stored.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
        Stored.Parameters.Add("pNotaCredito", SqlDbType.Int).Value = pNotaCredito;
        Stored.Parameters.Add("pAgente", SqlDbType.Int).Value = pAgente;
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionReporteComision.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerNotaCreditoExportar(string pSerieNotaCredito, string pFolioNotaCredito, string pFechaInicial, string pFechaFinal, int pPorFecha, int pAI, int pFiltroTimbrado, string pRazonSocial)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieNotaCredito);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolioNotaCredito);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pFiltroTimbrado", SqlDbType.Int).Value = pFiltroTimbrado;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionNotasCredito.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerCotizacionExportar(string pRazonSocial, string pFolio, int pIdEstatusCotizacion, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCotizacionExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdEstatusCotizacion", SqlDbType.Int).Value = pIdEstatusCotizacion;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = Usuario.IdSucursalActual;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionCotizaciones.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerCuentasPorCobrarExportar(string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionIngresos.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerEgresosExportar(string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEgresosExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionEgresos.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerIngresosNoDepositados(string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositadosExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionIngresosNoDepositados.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerDepositosExportar(string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDepositosExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionDepositos.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerChequesExportar(string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdChequesExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=ExportacionCheques.xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerReporteExportar(int pIdReportesKeep, string pFechaInicial, string pFechaFinal, int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CReportesKeep ReportesKeep = new CReportesKeep();
        ReportesKeep.LlenaObjeto(Convert.ToInt32(pIdReportesKeep), ConexionBaseDatos);
        string Reporte = "";
        Reporte = ReportesKeep.NombreProcedimiento;

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand(Reporte, sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = 10;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = 1;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = "";
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = "asc";
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = Convert.ToInt32(pIdSucursal);
        Stored.Parameters.Add("pExportar", SqlDbType.Int).Value = Convert.ToInt32(1);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);

        tblDatos.DataSource = dataSet.Tables[1];
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=" + ReportesKeep.Descripcion + ".xls");
        Response.ContentType = "application/excel";
    }

    private void ObtenerReporteCrediticioCliente(int pIdFuncionalidad, string pRazonSocial, int pIdCliente, int pEstatusProyecto, int pSucursal)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        string Store = "";
        string NombreArchivo = "";
        if (pIdFuncionalidad == 1)
        {
            Store = "spg_grdReporteCrediticioConsultaPagosExportar";
            NombreArchivo = "Pagos_" + pRazonSocial;
        }
        if (pIdFuncionalidad == 2)
        {
            Store = "spg_grdReporteCrediticioConsultaFacturasPendientesExportar";
            NombreArchivo = "FacturasPendientes_" + pRazonSocial;
        }

        if (pIdFuncionalidad == 3)
        {
            Store = "spg_grdReporteCrediticioConsultaProyectosExportar";
            NombreArchivo = "Proyectos_" + pRazonSocial;
        }

        if (pIdFuncionalidad == 4)
        {
            Store = "spg_grdReporteCrediticioConsultaFacturacionExportar";
            NombreArchivo = "Facturacion_" + pRazonSocial;
        }

        SqlCommand Stored = new SqlCommand(Store, sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;

        if (pIdFuncionalidad == 3)
        {
            Stored.Parameters.Add("pEstatusProyecto", SqlDbType.Int).Value = pEstatusProyecto;
        }
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pSucursal;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);

        string tab1 = "<td style='font-family:Calibri; background-color:#0404B4; padding:2px 2px 2px 12px; font-size:12px; font-weight:bold; color:White;'>";
        string endTab1 = "</td>";
        string tab2 = "<td>";
        string endTab2 = "</td>";
        string Columns = dataSet.Columns.Count.ToString();
        StringBuilder writer = new StringBuilder();

        writer.Append(" <table  border='1'>");
        writer.Append("<tr><td colspan=" + Columns + " align='center'>" + pRazonSocial + " </td></tr>");
        writer.Append(" <tr>");
        foreach (DataColumn dc in dataSet.Columns)
        {

            writer.Append(tab1 + "<b>" + dc.ColumnName + "</b>" + endTab1);
        }
        writer.Append(" </tr>");



        int k = 0;

        while (k < dataSet.Rows.Count)
        {
            writer.Append(" <tr>");
            foreach (DataColumn dc in dataSet.Columns)
            {

                writer.Append(tab2 + dataSet.Rows[k][dc.ColumnName].ToString() + endTab2);
            }


            k++;
            writer.Append(" </tr>");
        }

        writer.Append(" </table>");

        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("content-disposition", "attachment; filename=ReporteEstadoCrediticio " + NombreArchivo + ".xls");
        Response.BinaryWrite(System.Text.Encoding.UTF8.GetBytes(writer.ToString()));
        Response.End();
        Response.Flush();

    }

    private void ObtenerReporteCrediticioClientePagos(string pRazonSocial, int pIdCliente, int pIdSucursal)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdReporteCrediticioConsultaPagosExportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=Pagos_"+ pRazonSocial.Replace(" ","_") +".xls");
        Response.ContentType = "application/excel";
    }

}