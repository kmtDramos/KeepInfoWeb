using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using Newtonsoft.Json.Linq;

public partial class ReportesKeep : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //GridReportes
        CJQGrid GridReportes = new CJQGrid();
        GridReportes.NombreTabla = "grdReportesKeep";
        GridReportes.CampoIdentificador = "IdReportesKeep";
        GridReportes.ColumnaOrdenacion = "IdReportesKeep";
        GridReportes.Metodo = "ObtenerReportesKeep";
        GridReportes.TituloTabla = "Reportes";

        //IdReportesKeep
        CJQColumn ColIdReportesKeep = new CJQColumn();
        ColIdReportesKeep.Nombre = "IdReportesKeep";
        ColIdReportesKeep.Oculto = "true";
        ColIdReportesKeep.Encabezado = "IdReportesKeep";
        ColIdReportesKeep.Buscador = "false";
        GridReportes.Columnas.Add(ColIdReportesKeep);

        //Departamento
        CJQColumn ColDepartamento = new CJQColumn();
        ColDepartamento.Nombre = "Carpeta";
        ColDepartamento.Encabezado = "Departamento";
        ColDepartamento.Ancho = "200";
        ColDepartamento.Alineacion = "left";
        GridReportes.Columnas.Add(ColDepartamento);

        //Descripcion
        CJQColumn ColNombreReporte = new CJQColumn();
        ColNombreReporte.Nombre = "Descripcion";
        ColNombreReporte.Encabezado = "Nombre del reporte";
        ColNombreReporte.Ancho = "300";
        ColNombreReporte.Alineacion = "left";
        GridReportes.Columnas.Add(ColNombreReporte);

        ClientScript.RegisterStartupScript(this.GetType(), "grdReportesKeep", GridReportes.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerReportesKeep(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pDescripcion, string pCarpeta)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdReportesKeep", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 250).Value = Convert.ToString(pDescripcion);
        Stored.Parameters.Add("pCarpeta", SqlDbType.VarChar, 250).Value = Convert.ToString(pCarpeta);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaFiltrosReportesKeep(int pIdReportesKeep)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        CReportesKeep ReportesKeep = new CReportesKeep();
        ReportesKeep.LlenaObjeto(pIdReportesKeep, ConexionBaseDatos);
        DateTime Fecha = DateTime.Now;
        Modelo.Add("FechaInicial", Convert.ToString(Fecha.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(Fecha.ToShortDateString()));
        Modelo.Add("IdReportesKeep", ReportesKeep.IdReportesKeep);
        Modelo.Add("NombreReporte", ReportesKeep.Descripcion);
        Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(ConexionBaseDatos));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaReporteKeep(int pIdReportesKeep)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        CReportesKeep ReportesKeep = new CReportesKeep();
        ReportesKeep.LlenaObjeto(pIdReportesKeep, ConexionBaseDatos);
        Modelo.Add("IdReportesKeep", ReportesKeep.IdReportesKeep);
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerNombres(Dictionary<string, object> ReporteKeep)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            CReportesKeep ReportesKeep = new CReportesKeep();
            ReportesKeep.LlenaObjeto(Convert.ToInt32(ReporteKeep["pIdReportesKeep"]), ConexionBaseDatos);
            string Reporte = "";
            Reporte = ReportesKeep.NombreProcedimiento;

            JObject Modelo = new JObject();
            SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
            SqlCommand Stored = new SqlCommand(Reporte, sqlCon);
            Stored.CommandType = CommandType.StoredProcedure;
            Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = ReporteKeep["pTamanoPaginacion"];
            Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = ReporteKeep["pPaginaActual"];
            Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = ReporteKeep["pColumnaOrden"];
            Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = ReporteKeep["pTipoOrden"];
            Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = ReporteKeep["pFechaInicial"];
            Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = ReporteKeep["pFechaFinal"];
            Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = Convert.ToInt32(ReporteKeep["pIdSucursal"]);
            DataSet dataSet = new DataSet();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
            dataAdapter.Fill(dataSet);
            List<string> Columnas = new List<string>();
            JArray Datos = new JArray();
            foreach (DataRow row in dataSet.Tables[1].Rows)
            {
                for (int i = 0; i < dataSet.Tables[1].Columns.Count; i++)
                {
                    Columnas.Add(dataSet.Tables[1].Columns[i].ColumnName);

                }
                break;
            }
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Columnas", Columnas));

        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponseReporte ObtenerReporte(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdReportesKeep, string pFechaInicial, string pFechaFinal, int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        CReportesKeep ReportesKeep = new CReportesKeep();
        ReportesKeep.LlenaObjeto(Convert.ToInt32(pIdReportesKeep), ConexionBaseDatos);
        string Reporte = "";
        Reporte = ReportesKeep.NombreProcedimiento;

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand(Reporte, sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = Convert.ToInt32(pIdSucursal);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponseReporte(dataSet);
    }

}