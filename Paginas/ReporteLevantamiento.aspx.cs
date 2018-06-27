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
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using arquitecturaNet;
using System.IO;
using System.Diagnostics;

public partial class ReporteLevantamiento : System.Web.UI.Page
{
    public string ticks = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ticks = DateTime.Now.Ticks.ToString();

        //GridLevantamiento
        CJQGrid GridReporteLevantamiento = new CJQGrid();
        GridReporteLevantamiento.NombreTabla = "grdReporteLevantamiento";
        GridReporteLevantamiento.CampoIdentificador = "IdLevantamiento";
        GridReporteLevantamiento.TipoOrdenacion = "DESC";
        GridReporteLevantamiento.ColumnaOrdenacion = "IdLevantamiento";
        GridReporteLevantamiento.Metodo = "ObtenerReporteLevantamiento";
        GridReporteLevantamiento.TituloTabla = "Catálogo de Levantamientos";
        GridReporteLevantamiento.GenerarFuncionFiltro = false;

        //IdLevantamiento
        CJQColumn ColIdLevantamiento = new CJQColumn();
        ColIdLevantamiento.Nombre = "IdLevantamiento";
        ColIdLevantamiento.Encabezado = "Folio";
        ColIdLevantamiento.Buscador = "false";
        ColIdLevantamiento.Ancho = "50";
        ColIdLevantamiento.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColIdLevantamiento);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Buscador = "false";
        ColSucursal.Ancho = "120";
        ColSucursal.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColSucursal);

        //Razon Social
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "false";
        ColRazonSocial.Ancho = "200";
        ColRazonSocial.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColRazonSocial);

        //Agente
        CJQColumn ColAgente = new CJQColumn();
        ColAgente.Nombre = "Agente";
        ColAgente.Encabezado = "Agente";
        ColAgente.Buscador = "false";
        ColAgente.Ancho = "150";
        ColAgente.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColAgente);

        //Oportunidad
        CJQColumn ColOportunidad = new CJQColumn();
        ColOportunidad.Nombre = "Oportunidad";
        ColOportunidad.Encabezado = "Oportunidad";
        ColOportunidad.Buscador = "false";
        ColOportunidad.Ancho = "80";
        ColOportunidad.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColOportunidad);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "División";
        ColDivision.Buscador = "false";
        ColDivision.Ancho = "200";
        ColDivision.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColDivision);

        //Estatus Levantamiento
        CJQColumn ColEstatus = new CJQColumn();
        ColEstatus.Nombre = "Estatus";
        ColEstatus.Encabezado = "Estatus";
        ColEstatus.Buscador = "false";
        ColEstatus.Ancho = "100";
        ColEstatus.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColEstatus);

        //Fecha Solicitada
        CJQColumn ColFechaSolicitada = new CJQColumn();
        ColFechaSolicitada.Nombre = "FechaSolicitada";
        ColFechaSolicitada.Encabezado = "Fecha Solicitada";
        ColFechaSolicitada.Buscador = "false";
        ColFechaSolicitada.Ancho = "100";
        ColFechaSolicitada.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColFechaSolicitada);

        //Nivel Servicio 1
        CJQColumn ColNivelServicio1 = new CJQColumn();
        ColNivelServicio1.Nombre = "NivelServicioAsignacion";
        ColNivelServicio1.Encabezado = "Nivel Asignacion";
        ColNivelServicio1.Buscador = "false";
        ColNivelServicio1.Ancho = "80";
        ColNivelServicio1.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColNivelServicio1);

        //Eficiencia 1
        CJQColumn ColEficiencia1 = new CJQColumn();
        ColEficiencia1.Nombre = "Eficiencia";
        ColEficiencia1.Encabezado = "Nivel Eficiencia";
        ColEficiencia1.Buscador = "false";
        ColEficiencia1.Ancho = "80";
        ColEficiencia1.Ordenable = "true";
        ColEficiencia1.Oculto = "true";
        GridReporteLevantamiento.Columnas.Add(ColEficiencia1);
        
        //Fecha Asginada
        CJQColumn ColFechaAsignacion = new CJQColumn();
        ColFechaAsignacion.Nombre = "FechaAsignacion";
        ColFechaAsignacion.Encabezado = "Fecha Asignada";
        ColFechaAsignacion.Buscador = "false";
        ColFechaAsignacion.Ancho = "100";
        ColFechaAsignacion.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColFechaAsignacion);

        //Asignado
        CJQColumn ColAsignado = new CJQColumn();
        ColAsignado.Nombre = "Asignado";
        ColAsignado.Encabezado = "Asignado";
        ColAsignado.Buscador = "false";
        ColAsignado.Ancho = "150";
        ColAsignado.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColAsignado);

        //Fecha Levantamiento
        CJQColumn ColFechaLevantamiento = new CJQColumn();
        ColFechaLevantamiento.Nombre = "FechaLevantamiento";
        ColFechaLevantamiento.Encabezado = "Fecha Levantamiento";
        ColFechaLevantamiento.Buscador = "false";
        ColFechaLevantamiento.Ancho = "100";
        ColFechaLevantamiento.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColFechaLevantamiento);

        //Nivel Servicio 2
        CJQColumn ColNivelServicio2 = new CJQColumn();
        ColNivelServicio2.Nombre = "NivelServicio2";
        ColNivelServicio2.Encabezado = "Nivel Asignacion";
        ColNivelServicio2.Buscador = "false";
        ColNivelServicio2.Ancho = "80";
        ColNivelServicio2.Ordenable = "true";
        GridReporteLevantamiento.Columnas.Add(ColNivelServicio2);

        //Eficiencia 2
        CJQColumn ColEficiencia2 = new CJQColumn();
        ColEficiencia2.Nombre = "Eficiencia2";
        ColEficiencia2.Encabezado = "Nivel Eficiencia";
        ColEficiencia2.Buscador = "false";
        ColEficiencia2.Ancho = "80";
        ColEficiencia2.Ordenable = "true";
        ColEficiencia2.Oculto = "true";
        GridReporteLevantamiento.Columnas.Add(ColEficiencia2);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "70";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBaja.Oculto = "true";
        GridReporteLevantamiento.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarLevantamiento";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        ColConsultar.Oculto = "true";
        GridReporteLevantamiento.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdReporteLevantamiento", GridReporteLevantamiento.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerReporteLevantamiento(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pAI, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdAgente, int pIdDivision, int pIdEstatusLevantamiento, int pIdCliente, int pIdOportunidad, int pIdAsignado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdLevantamiento_Reporte", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 30).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pIdAgente", SqlDbType.Int).Value = pIdAgente;
        Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
        Stored.Parameters.Add("pIdEstatusLevantamiento", SqlDbType.Int).Value = pIdEstatusLevantamiento;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
        Stored.Parameters.Add("pIdAsignado", SqlDbType.Int).Value = pIdAsignado;
        
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerSucursales()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSession) {
            if (Error == 0)
            {

                JObject Modelo = new JObject();
                Modelo.Add("Sucursales", CSucursal.ObtenerSucursalesEmpresa(pConexion));
                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerDivisiones()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSession) {
            if (Error == 0)
            {

                JObject Modelo = new JObject();
                Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(pConexion));
                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerEstatus()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSession) {
            if (Error == 0)
            {
                JArray JAEstatus = new JArray();
                CEstatusLevantamiento Estatus = new CEstatusLevantamiento();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("Baja", 0);
                foreach (CEstatusLevantamiento oEstatus in Estatus.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    if (oEstatus.IdEstatusLevantamiento == 1 || oEstatus.IdEstatusLevantamiento == 4)
                    {
                        JObject JEstatus = new JObject();
                        JEstatus.Add("Valor", oEstatus.IdEstatusLevantamiento);
                        JEstatus.Add("Descripcion", oEstatus.Descripcion);
                        JAEstatus.Add(JEstatus);
                    }
                }

                JObject Modelo = new JObject();
                Modelo.Add("Estatus", JAEstatus);
                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerUsuario(string Usuario)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_Agente";
                Consulta.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 50).Value = Usuario;

                Modelo.Add("Usuarios", CUtilerias.ObtenerConsulta(Consulta, pConexion));

                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string BuscarRazonSocialCliente(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

}