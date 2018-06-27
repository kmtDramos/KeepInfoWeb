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

public partial class ExportacionesExcel_ExportarExcelPlanVentas : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string[] separador = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('/');
        string pagina = separador[separador.Length - 1];

        switch (pagina)
        {
            case "PlaneacionVentas.aspx":
				string pColumnaOrden = Convert.ToString(HttpContext.Current.Request.QueryString["pColumnaOrden"]);
				string pTipoOrden = Convert.ToString(HttpContext.Current.Request.QueryString["pTipoOrden"]);
				string pIdOportunidad = Convert.ToString(HttpContext.Current.Request.QueryString["pIdOportunidad"]);
                string pOportunidad = Convert.ToString(HttpContext.Current.Request.QueryString["pOportunidad"]);
                string pAgente = Convert.ToString(HttpContext.Current.Request.QueryString["pAgente"]);
                string pCliente = Convert.ToString(HttpContext.Current.Request.QueryString["pCliente"]);
				int pPreventaDetenico = Convert.ToInt16(HttpContext.Current.Request.QueryString["pPreventaDetenido"]);
                int pVentasDetenido = Convert.ToInt16(HttpContext.Current.Request.QueryString["pVentasDetenido"]);
                int pComprasDetenido = Convert.ToInt16(HttpContext.Current.Request.QueryString["pComprasDetenido"]);
                int pProyectosDetenido = Convert.ToInt16(HttpContext.Current.Request.QueryString["pProyectosDetenido"]);
                int pFinzanzasDetenido = Convert.ToInt16(HttpContext.Current.Request.QueryString["pFinzanzasDetenido"]);
                int pSinPlaneacion = Convert.ToInt16(HttpContext.Current.Request.QueryString["pSinPlaneacion"]);
				int planeacionMes1 = Convert.ToInt16(HttpContext.Current.Request.QueryString["planeacionMes1"]);
				int pNivelInteres = Convert.ToInt32(HttpContext.Current.Request.QueryString["pNivelInteres"]);
                int pIdSucursal = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdSucursal"]);
                int pIdDivision = Convert.ToInt32(HttpContext.Current.Request.QueryString["pDivision"]);
                int pEsProyecto = Convert.ToInt32(HttpContext.Current.Request.QueryString["pEsProyecto"]);
                int pAutorizado = Convert.ToInt32(HttpContext.Current.Request.QueryString["pAutorizado"]);
				int pIdEstatusCompras = Convert.ToInt32(HttpContext.Current.Request.QueryString["pIdEstatusCompras"]);
                ExportarOportunidades(pColumnaOrden, pTipoOrden, pIdOportunidad, pOportunidad, pAgente, pCliente, pNivelInteres, pIdSucursal, pIdDivision, 
                    pPreventaDetenico, pVentasDetenido, pComprasDetenido, pProyectosDetenido, pFinzanzasDetenido, pSinPlaneacion, planeacionMes1, pEsProyecto, pAutorizado,
					pIdEstatusCompras);
                break;
        }

    }

    private void ExportarOportunidades(string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente,
        int pNivelInteres, int pIdSucursal, int pIdDivision, int pPreventaDetenico, int pVentasDetenido, int pComprasDetenido, int pProyectosDetenido,
        int pFinzanzasDetenido, int pSinPlaneacion, int planeacionMes1, int pEsProyecto, int pAutorizado, int pIdEstatusCompras)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdPlanVentas_Exportar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 255).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 255).Value = pTipoOrden;
		Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
		Stored.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = pOportunidad;
        Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
        Stored.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
        Stored.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
		if (pIdSucursal == 0)
        {
            pIdSucursal = -1;
        }
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
		Stored.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = 1;
		
		Stored.Parameters.Add("pPreventaDetenido", SqlDbType.Int).Value = pPreventaDetenico;
        Stored.Parameters.Add("pVentasDetenido", SqlDbType.Int).Value = pVentasDetenido;
        Stored.Parameters.Add("pComprasDetenido", SqlDbType.Int).Value = pComprasDetenido;
        Stored.Parameters.Add("pProyectosDetenido", SqlDbType.Int).Value = pProyectosDetenido;
        Stored.Parameters.Add("pFinzanzasDetenido", SqlDbType.Int).Value = pFinzanzasDetenido;
        Stored.Parameters.Add("pSinPlaneacion", SqlDbType.Int).Value = pSinPlaneacion;
        Stored.Parameters.Add("pPlaneacionMes1", SqlDbType.Int).Value = planeacionMes1;
        Stored.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
        Stored.Parameters.Add("pAutorizado", SqlDbType.Int).Value = pAutorizado;
		Stored.Parameters.Add("pIdEstatusCompras", SqlDbType.Int).Value = pIdEstatusCompras;

        DataTable dataSet = new DataTable();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        tblDatos.DataSource = dataSet;
        tblDatos.DataBind();
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=PlanVentas"+ DateTime.Now.ToShortDateString() +".xls");
        Response.ContentType = "application/excel";
    }

}
