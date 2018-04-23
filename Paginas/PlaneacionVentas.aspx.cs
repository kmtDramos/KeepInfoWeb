using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
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
using System.IO;

public partial class Paginas_PlaneacionVentas : System.Web.UI.Page
{

	public static string Mes1 = "";
	public static string Mes2 = "";
	public static string Mes3 = "";

	protected void Page_Load(object sender, EventArgs e)
	{

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				COportunidad Oportunidades = new COportunidad();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("Cerrado", 0);
				pParametros.Add("Baja", 0);

				foreach (COportunidad Oportunidad in Oportunidades.LlenaObjetosFiltros(pParametros, pConexion))
				{
					if (Oportunidad.Monto - Oportunidad.Facturas == 0)
					{
						Oportunidad.Mes1 = 0;
						Oportunidad.Mes2 = 0;
						Oportunidad.Mes3 = 0;
						Oportunidad.PreventaDetenido = false;
						Oportunidad.VentasDetenido = false;
						Oportunidad.ComprasDetenido = false;
						Oportunidad.ProyectosDetenido = false;
						Oportunidad.FinzanzasDetenido = false;
						//Oportunidad.Editar(pConexion);
					}
				}
			}
		});

		//GridOportunidad
		CJQGrid GridPlanVentas = new CJQGrid();
		GridPlanVentas.NombreTabla = "grdPlanVentas";
		GridPlanVentas.CampoIdentificador = "IdOportunidad";
		GridPlanVentas.ColumnaOrdenacion = "Diferencia";
		GridPlanVentas.TipoOrdenacion = "DESC";
		GridPlanVentas.Metodo = "ObtenerPlanVentas";
		GridPlanVentas.TituloTabla = "Plan de ventas";
		GridPlanVentas.GenerarFuncionFiltro = false;
		GridPlanVentas.GenerarFuncionTerminado = true;
		GridPlanVentas.Altura = 231;
		GridPlanVentas.Ancho = 940;
		GridPlanVentas.NumeroRegistros = 10;
		GridPlanVentas.RangoNumeroRegistros = "10,25,50";

		#region columnas oportunidad
		//IdOportunidad
		CJQColumn ColIdOpotunidad = new CJQColumn();
		ColIdOpotunidad.Nombre = "IdOportunidad";
		ColIdOpotunidad.Encabezado = "No. Opt";
		ColIdOpotunidad.Oculto = "false";
		ColIdOpotunidad.Ancho = "50";
		GridPlanVentas.Columnas.Add(ColIdOpotunidad);

		//Proyectos
		CJQColumn ColProyecto = new CJQColumn();
		ColProyecto.Nombre = "Proyecto";
		ColProyecto.Encabezado = "Proyecto";
		ColProyecto.Ancho = "100";
		ColProyecto.Ordenable = "false";
		ColProyecto.Alineacion = "Left";
		GridPlanVentas.Columnas.Add(ColProyecto);

		CJQColumn ColPedidos = new CJQColumn();
		ColPedidos.Nombre = "Pedido";
		ColPedidos.Encabezado = "Pedidos";
		ColPedidos.Ancho = "100";
		ColPedidos.Ordenable = "false";
		ColPedidos.Alineacion = "Left";
		GridPlanVentas.Columnas.Add(ColPedidos);

		//Oportunidad
		CJQColumn ColOportunidad = new CJQColumn();
		ColOportunidad.Nombre = "Oportunidad";
		ColOportunidad.Encabezado = "Oportunidad";
		ColOportunidad.Ancho = "100";
		ColOportunidad.Ordenable = "false";
		ColOportunidad.Alineacion = "Left";
		GridPlanVentas.Columnas.Add(ColOportunidad);

		//IdEmpresa
		CJQColumn ColCliente = new CJQColumn();
		ColCliente.Nombre = "Cliente";
		ColCliente.Encabezado = "Cliente";
		ColCliente.Ancho = "120";
		ColCliente.Ordenable = "false";
		ColCliente.Alineacion = "Left";
		GridPlanVentas.Columnas.Add(ColCliente);

		CJQColumn ColCondicionPago = new CJQColumn();
		ColCondicionPago.Nombre = "CondicionPago";
		ColCondicionPago.Encabezado = "Condición de pago";
		ColCondicionPago.Ancho = "80";
		ColCondicionPago.TipoBuscador = "Combo";
		ColCondicionPago.StoredProcedure.CommandText = "sp_CondicionesDePago_Filtro";
		GridPlanVentas.Columnas.Add(ColCondicionPago);

		CJQColumn ColMargen = new CJQColumn();
		ColMargen.Nombre = "Margen";
		ColMargen.Encabezado = "Margen";
		ColMargen.Buscador = "false";
		ColMargen.Ancho = "50";
		GridPlanVentas.Columnas.Add(ColMargen);

		CJQColumn ColSucursal = new CJQColumn();
		ColSucursal.Nombre = "Sucursal";
		ColSucursal.Encabezado = "Sucursal";
		ColSucursal.Ancho = "80";
		ColSucursal.Buscador = "true";
		ColSucursal.TipoBuscador = "Combo";
		ColSucursal.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Sucursal";
		GridPlanVentas.Columnas.Add(ColSucursal);

		CJQColumn ColDivision = new CJQColumn();
		ColDivision.Nombre = "Division";
		ColDivision.Encabezado = "División";
		ColDivision.Ancho = "80";
		ColDivision.Buscador = "true";
		ColDivision.TipoBuscador = "Combo";
		ColDivision.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Division";
		GridPlanVentas.Columnas.Add(ColDivision);

		CJQColumn ColAgente = new CJQColumn();
		ColAgente.Nombre = "Agente";
		ColAgente.Encabezado = "Agente";
		ColAgente.Ancho = "120";
		ColAgente.Ordenable = "false";
		ColAgente.Alineacion = "left";
		GridPlanVentas.Columnas.Add(ColAgente);

		//Ganado
		CJQColumn ColNivelInteres = new CJQColumn();
		ColNivelInteres.Nombre = "NivelInteres";
		ColNivelInteres.Encabezado = "Nivel Interes";
		ColNivelInteres.Ancho = "80";
		ColNivelInteres.Alineacion = "Right";
		ColNivelInteres.Ordenable = "false";
		ColNivelInteres.Buscador = "true";
		ColNivelInteres.TipoBuscador = "Combo";
		ColNivelInteres.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad";
		GridPlanVentas.Columnas.Add(ColNivelInteres);

		CJQColumn ColDias = new CJQColumn();
		ColDias.Nombre = "Dias";
		ColDias.Encabezado = "Dias op";
		ColDias.Ancho = "70";
		ColDias.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColDias);

		// Monto
		CJQColumn ColMonto = new CJQColumn();
		ColMonto.Nombre = "Monto";
		ColMonto.Encabezado = "Monto";
		ColMonto.Ancho = "100";
		ColMonto.Alineacion = "right";
		ColMonto.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColMonto);

		CJQColumn ColFacturado = new CJQColumn();
		ColFacturado.Nombre = "Facturas";
		ColFacturado.Encabezado = "Facturado";
		ColFacturado.Ancho = "100";
		ColFacturado.Alineacion = "right";
		ColFacturado.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColFacturado);

		CJQColumn ColDiferencia = new CJQColumn();
		ColDiferencia.Nombre = "Diferencia";
		ColDiferencia.Encabezado = "Diferencia";
		ColDiferencia.Ancho = "100";
		ColDiferencia.Alineacion = "right";
		ColDiferencia.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColDiferencia);

		CUtilerias Meses = new CUtilerias();
		Mes1 = Meses.ObtenerMes(DateTime.Today.Month);
		Mes2 = Meses.ObtenerMes(DateTime.Today.Month + 1);
		Mes3 = Meses.ObtenerMes(DateTime.Today.Month + 2);

		CJQColumn ColMes1 = new CJQColumn();
		ColMes1.Nombre = "Mes1";
		ColMes1.Encabezado = Mes1;
		ColMes1.Ancho = "100";
		ColMes1.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColMes1);

		CJQColumn ColMes2 = new CJQColumn();
		ColMes2.Nombre = "Mes2";
		ColMes2.Encabezado = Mes2;
		ColMes2.Ancho = "100";
		ColMes2.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColMes2);

		CJQColumn ColMes3 = new CJQColumn();
		ColMes3.Nombre = "Mes3";
		ColMes3.Encabezado = Mes3;
		ColMes3.Ancho = "100";
		ColMes3.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColMes3);

        CJQColumn ColEsProyecto = new CJQColumn();
        ColEsProyecto.Nombre = "EsProyecto";
        ColEsProyecto.Encabezado = "Es proyecto";
        ColEsProyecto.Ancho = "50";
        ColEsProyecto.Alineacion = "Center";
        ColEsProyecto.Buscador = "true";
        ColEsProyecto.TipoBuscador = "Combo";
        ColEsProyecto.StoredProcedure.CommandText = "sp_Oportunidad_FiltroProyecto";
        GridPlanVentas.Columnas.Add(ColEsProyecto);

        CJQColumn ColAutorizado = new CJQColumn();
        ColAutorizado.Nombre = "Autorizado";
        ColAutorizado.Encabezado = "Autorizado";
        ColAutorizado.Ancho = "50";
        ColAutorizado.Alineacion = "Center";
        ColAutorizado.Buscador = "true";
        ColAutorizado.TipoBuscador = "Combo";
        ColAutorizado.StoredProcedure.CommandText = "sp_Oportunidad_FiltroProyecto";
        GridPlanVentas.Columnas.Add(ColAutorizado);

        CJQColumn ColPreventa = new CJQColumn();
		ColPreventa.Nombre = "PreventaDetenido";
		ColPreventa.Encabezado = "Preventa";
		ColPreventa.Ancho = "120";
		ColPreventa.Buscador = "true";
		ColPreventa.TipoBuscador = "Combo";
		ColPreventa.StoredProcedure.CommandText = "sp_FiltroBooleano";
		GridPlanVentas.Columnas.Add(ColPreventa);

		CJQColumn ColVentas = new CJQColumn();
		ColVentas.Nombre = "VentasDetenido";
		ColVentas.Encabezado = "Ventas";
		ColVentas.Ancho = "120";
		ColVentas.Buscador = "true";
		ColVentas.TipoBuscador = "Combo";
		ColVentas.StoredProcedure.CommandText = "sp_FiltroBooleano";
		GridPlanVentas.Columnas.Add(ColVentas);

		CJQColumn ColCompras = new CJQColumn();
		ColCompras.Nombre = "ComprasDetenido";
		ColCompras.Encabezado = "Compras";
		ColCompras.Ancho = "120";
		ColCompras.Buscador = "true";
		ColCompras.TipoBuscador = "Combo";
		ColCompras.StoredProcedure.CommandText = "sp_FiltroBooleano";
		GridPlanVentas.Columnas.Add(ColCompras);

		CJQColumn ColProyectos = new CJQColumn();
		ColProyectos.Nombre = "ProyectosDetenido";
		ColProyectos.Encabezado = "Proyectos";
		ColProyectos.Ancho = "120";
		ColProyectos.Buscador = "true";
		ColProyectos.TipoBuscador = "Combo";
		ColProyectos.StoredProcedure.CommandText = "sp_FiltroBooleano";
		GridPlanVentas.Columnas.Add(ColProyectos);

        CJQColumn ColFinanzas = new CJQColumn();
		ColFinanzas.Nombre = "FinzanzasDetenido";
		ColFinanzas.Encabezado = "Finanzas";
		ColFinanzas.Ancho = "120";
		ColFinanzas.Buscador = "true";
		ColFinanzas.TipoBuscador = "Combo";
		ColFinanzas.StoredProcedure.CommandText = "sp_FiltroBooleano";
		GridPlanVentas.Columnas.Add(ColFinanzas);

		CJQColumn ColEstatus = new CJQColumn();
		ColEstatus.Nombre = "Margen";
		ColEstatus.Encabezado = "Estatus";
		ColEstatus.Ancho = "80";
		ColEstatus.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColEstatus);

		CJQColumn ColBaja = new CJQColumn();
		ColBaja.Nombre = "Baja";
		ColBaja.Encabezado = "Inactivar";
		ColBaja.Buscador = "false";
		ColBaja.Ordenable = "false";
		ColBaja.Ancho = "50";
		GridPlanVentas.Columnas.Add(ColBaja);

		CJQColumn ColFecha1 = new CJQColumn();
		ColFecha1.Nombre = "CompromisoPreventa";
		ColFecha1.Encabezado = "Preventa";
		ColFecha1.Oculto = "true";
		ColFecha1.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColFecha1);

		CJQColumn ColFecha2 = new CJQColumn();
		ColFecha2.Nombre = "CompromisoVenta";
		ColFecha2.Encabezado = "Preventa";
		ColFecha2.Oculto = "true";
		ColFecha2.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColFecha2);

		CJQColumn ColFecha3 = new CJQColumn();
		ColFecha3.Nombre = "CompromisoCompras";
		ColFecha3.Encabezado = "Preventa";
		ColFecha3.Oculto = "true";
		ColFecha3.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColFecha3);

		CJQColumn ColFecha4 = new CJQColumn();
		ColFecha4.Nombre = "CompromisoProyectos";
		ColFecha4.Encabezado = "Preventa";
		ColFecha4.Oculto = "true";
		ColFecha4.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColFecha4);

		CJQColumn ColFecha5 = new CJQColumn();
		ColFecha5.Nombre = "CompromisoFinanzas";
		ColFecha5.Encabezado = "Preventa";
		ColFecha5.Oculto = "true";
		ColFecha5.Buscador = "false";
		GridPlanVentas.Columnas.Add(ColFecha5);
		#endregion

		ClientScript.RegisterStartupScript(Page.GetType(), "grdVentasAgente", GridPlanVentas.GeneraGrid(), true);
        

	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerPlanVentas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad,
		string pOportunidad, string pAgente, string pCliente, int pSucursal, int pNivelInteres, int pPreventaDetenido, int pVentasDetenido, int pComprasDetenido, int pProyectosDetenido,
		int pFinzanzasDetenido, int pSinPlaneacion, int planeacionMes1, int pDivision, int pEsProyecto, int pAutorizado, int pIdCondicionPago)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdPlanVentas", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
		Stored.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = pOportunidad;
		Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = pAgente;
		Stored.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
		Stored.Parameters.Add("pIdCondicionPago", SqlDbType.Int).Value = pIdCondicionPago;
		Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pSucursal;
		Stored.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
		Stored.Parameters.Add("pPreventaDetenido", SqlDbType.Int).Value = pPreventaDetenido;
		Stored.Parameters.Add("pVentasDetenido", SqlDbType.Int).Value = pVentasDetenido;
		Stored.Parameters.Add("pComprasDetenido", SqlDbType.Int).Value = pComprasDetenido;
		Stored.Parameters.Add("pProyectosDetenido", SqlDbType.Int).Value = pProyectosDetenido;
		Stored.Parameters.Add("pFinzanzasDetenido", SqlDbType.Int).Value = pFinzanzasDetenido;
		Stored.Parameters.Add("pSinPlaneacion", SqlDbType.Int).Value = pSinPlaneacion;
		Stored.Parameters.Add("pPlaneacionMes1", SqlDbType.Int).Value = planeacionMes1;
		Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pDivision;
        Stored.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
        Stored.Parameters.Add("pAutorizado", SqlDbType.Int).Value = pAutorizado;

        DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

    [WebMethod]
    public static string AutorizarOportunidad(int pIdOportunidad, int pAutorizado)
    {

        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(pIdOportunidad, pConexion);

				if (oportunidad.Mes1 + oportunidad.Mes2 + oportunidad.Mes3 > 0)
				{
					oportunidad.Autorizado = Convert.ToBoolean(pAutorizado);
					oportunidad.Editar(pConexion);
				}
				else
				{
					DescripcionError = "No se puede autorizar una oportunidad sin planeación.";
					Error = 1;
				}
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();

    }

    [WebMethod]
	public static string BuscarOportunidad(string pOportunidad)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		COportunidad JsonOportunidad = new COportunidad();
		JsonOportunidad.StoredProcedure.CommandText = "sp_Oportunidad_ConsultarFiltroOportunidadGrid";
		JsonOportunidad.StoredProcedure.Parameters.AddWithValue("@pOportunidad", pOportunidad);
		string sJson = JsonOportunidad.ObtenerJsonOportunidad(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return sJson;

	}

	[WebMethod]
	public static string BuscarIdOportunidad(string pIdOportunidad)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		COportunidad JsonOportunidad = new COportunidad();
		JsonOportunidad.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_IdOportunidad";
		JsonOportunidad.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", pIdOportunidad);
		string sJson = JsonOportunidad.ObtenerJsonOportunidad(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return sJson;
	}

	[WebMethod]
	public static string BuscarAgente(string pAgente)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string repuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		COportunidad JsonOportunidad = new COportunidad();
		JsonOportunidad.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_Agente";
		JsonOportunidad.StoredProcedure.Parameters.AddWithValue("@pAgente", pAgente);
		string sJson = JsonOportunidad.ObtenerJsonOportunidad(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return sJson;
	}

	[WebMethod]
	public static string BuscarCliente(string pCliente)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

		COrganizacion jsonRazonSocial = new COrganizacion();
		jsonRazonSocial.StoredProcedure.CommandText = "sp_Oportunidad_ConsultarFiltrosGrid";
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pCliente);
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Usuario.IdSucursalActual);
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		respuesta = jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);

		//Cerrar Conexion
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return respuesta;
	}
	
	[WebMethod]
	public static string GuardarPlanVentas(int IdOportunidad, decimal Mes1, decimal Mes2, decimal Mes3)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				COportunidad Oportunidad = new COportunidad();
				Oportunidad.LlenaObjeto(IdOportunidad, pConexion);

				if (Oportunidad.IdOportunidad != 0)
				{
					Oportunidad.Mes1 = Mes1;
					Oportunidad.Mes2 = Mes2;
					Oportunidad.Mes3 = Mes3;

					Oportunidad.Editar(pConexion);

				}

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerTotalMeses(string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pPreventaDetenido,
		int pVentasDetenido, int pComprasDetenido, int pProyectosDetenido, int pFinzanzasDetenido)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescriptionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_PlanVentas_Totales";

				Consulta.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
				Consulta.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = pOportunidad;
				Consulta.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
				Consulta.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
				Consulta.StoredProcedure.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
				Consulta.StoredProcedure.Parameters.Add("pPreventaDetenido", SqlDbType.Int).Value = pPreventaDetenido;
				Consulta.StoredProcedure.Parameters.Add("pVentasDetenido", SqlDbType.Int).Value = pVentasDetenido;
				Consulta.StoredProcedure.Parameters.Add("pComprasDetenido", SqlDbType.Int).Value = pComprasDetenido;
				Consulta.StoredProcedure.Parameters.Add("pProyectosDetenido", SqlDbType.Int).Value = pProyectosDetenido;
				Consulta.StoredProcedure.Parameters.Add("pFinzanzasDetenido", SqlDbType.Int).Value = pFinzanzasDetenido;

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					Modelo.Add("Facturado", Convert.ToDecimal(Consulta.Registros["Facturado"]));
					Modelo.Add("Mes1", Convert.ToDecimal(Consulta.Registros["Mes1"]));
					Modelo.Add("PlanCierre", Convert.ToDecimal(Consulta.Registros["PlanCierre"]));
					Modelo.Add("Mes2", Convert.ToDecimal(Consulta.Registros["Mes2"]));
					Modelo.Add("Mes3", Convert.ToDecimal(Consulta.Registros["Mes3"]));
				}

				Consulta.CerrarConsulta();

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescriptionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerTotalesSucursal(string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pPreventaDetenido,
		int pVentasDetenido, int pComprasDetenido, int pProyectosDetenido, int pFinzanzasDetenido)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescriptionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_PlanVentas_TotalesSucursal";

				Consulta.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
				Consulta.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = pOportunidad;
				Consulta.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
				Consulta.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
				Consulta.StoredProcedure.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
				Consulta.StoredProcedure.Parameters.Add("pPreventaDetenido", SqlDbType.Int).Value = pPreventaDetenido;
				Consulta.StoredProcedure.Parameters.Add("pVentasDetenido", SqlDbType.Int).Value = pVentasDetenido;
				Consulta.StoredProcedure.Parameters.Add("pComprasDetenido", SqlDbType.Int).Value = pComprasDetenido;
				Consulta.StoredProcedure.Parameters.Add("pProyectosDetenido", SqlDbType.Int).Value = pProyectosDetenido;
				Consulta.StoredProcedure.Parameters.Add("pFinzanzasDetenido", SqlDbType.Int).Value = pFinzanzasDetenido;

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					Modelo.Add("Monterrey", Convert.ToDecimal(Consulta.Registros["Monterrey"]));
					Modelo.Add("Mexico", Convert.ToDecimal(Consulta.Registros["Mexico"]));
					Modelo.Add("Guadalajara", Convert.ToDecimal(Consulta.Registros["Guadalajara"]));
				}

				Consulta.CerrarConsulta();

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescriptionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerTotalDepartamentos(string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pPreventaDetenido,
		int pVentasDetenido, int pComprasDetenido, int pProyectosDetenido, int pFinzanzasDetenido)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescriptionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_PlanVentas_TotalDepartamento";

				Consulta.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
				Consulta.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = pOportunidad;
				Consulta.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
				Consulta.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
				Consulta.StoredProcedure.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
				Consulta.StoredProcedure.Parameters.Add("pPreventaDetenido", SqlDbType.Int).Value = pPreventaDetenido;
				Consulta.StoredProcedure.Parameters.Add("pVentasDetenido", SqlDbType.Int).Value = pVentasDetenido;
				Consulta.StoredProcedure.Parameters.Add("pComprasDetenido", SqlDbType.Int).Value = pComprasDetenido;
				Consulta.StoredProcedure.Parameters.Add("pProyectosDetenido", SqlDbType.Int).Value = pProyectosDetenido;
				Consulta.StoredProcedure.Parameters.Add("pFinzanzasDetenido", SqlDbType.Int).Value = pFinzanzasDetenido;

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					Modelo.Add("Preventa", Convert.ToDecimal(Consulta.Registros["PreventaDetenido"]));
					Modelo.Add("TotalPreventa", Convert.ToDecimal(Consulta.Registros["TotalPreventaDetenido"]));
					Modelo.Add("Ventas", Convert.ToDecimal(Consulta.Registros["VentasDetenido"]));
					Modelo.Add("TotalVentas", Convert.ToDecimal(Consulta.Registros["TotalVentasDetenido"]));
					Modelo.Add("Compras", Convert.ToDecimal(Consulta.Registros["ComprasDetenido"]));
					Modelo.Add("TotalCompras", Convert.ToDecimal(Consulta.Registros["TotalComprasDetenido"]));
					Modelo.Add("Proyectos", Convert.ToDecimal(Consulta.Registros["ProyectosDetenido"]));
					Modelo.Add("TotalProyectos", Convert.ToDecimal(Consulta.Registros["TotalProyectosDetenido"]));
					Modelo.Add("Finanzas", Convert.ToDecimal(Consulta.Registros["FinzanzasDetenido"]));
					Modelo.Add("TotalFinanzas", Convert.ToDecimal(Consulta.Registros["TotalFinzanzasDetenido"]));
				}

				Consulta.CerrarConsulta();

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescriptionError);
		});

		return Respuesta.ToString();
	}
	
	[WebMethod]
	public static string ObtenerFormaAgregarOportunidad()
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		JObject oRespuesta = new JObject();

		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			Modelo.Add(new JProperty("NivelInteresOportunidad", CNivelInteresOportunidad.ObtenerJsonNivelInteresOportunidad(ConexionBaseDatos)));
			Modelo.Add(new JProperty("Division", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos)));
			Modelo.Add(new JProperty("Campana", CCampana.ObtenerJsonCampana(true, ConexionBaseDatos)));
			oRespuesta.Add(new JProperty("Modelo", Modelo));
			oRespuesta.Add(new JProperty("Error", 0));
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
	public static string EliminarOportunidad(int IdOportunidad, string MotivoCancelacion)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				COportunidad Oportunidad = new COportunidad();
				Oportunidad.LlenaObjeto(IdOportunidad, pConexion);

				Oportunidad.Baja = !Oportunidad.Baja;
				Oportunidad.MotivoCancelacion = MotivoCancelacion;
				Oportunidad.UltimaNota = MotivoCancelacion;
				Oportunidad.FechaNota = DateTime.Now;
				Oportunidad.Editar(pConexion);

				CBitacoraNotasOportunidad Bitacora = new CBitacoraNotasOportunidad();
				Bitacora.IdUsuario = UsuarioSesion.IdUsuario;
				Bitacora.IdOportunidad = IdOportunidad;
				Bitacora.Nota = MotivoCancelacion;
				Bitacora.FechaCreacion = DateTime.Now;
				Bitacora.Agregar(pConexion);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

    // VALIDA SI LA OPORTUNIDAD CUENTA CON COTIZACION O PROYECTO //
    [WebMethod]
    public static string validaOportunidad(int IdOportunidad)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {

                Respuesta.Add("Respuesta", false);
                
            }
            Respuesta.Add("Error",Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
           
        return Respuesta.ToString();
    }

    [WebMethod]
	public static string ObtenerFechaCumplimiento(int IdOportunidad, int Fecha)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesioin) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				COportunidad Oportunidad = new COportunidad();
				Oportunidad.LlenaObjeto(IdOportunidad, pConexion);

				string FechaCompromiso = "";
				string FechaTerminado = "";

				switch (Fecha)
				{
					case 1:
						FechaCompromiso = (Oportunidad.CompromisoPreventa.ToShortDateString() != "01/01/0001") ? Oportunidad.CompromisoPreventa.ToShortDateString() : "";
						FechaTerminado = (Oportunidad.TerminadoPreventa.ToShortDateString() != "01/01/0001") ? Oportunidad.TerminadoPreventa.ToShortDateString() : "";

						Modelo.Add("FechaCompromiso", FechaCompromiso);
						Modelo.Add("FechaTerminado", FechaTerminado);
						Modelo.Add("Detenido", Oportunidad.PreventaDetenido);
						Modelo.Add("TipoFecha", Fecha);
						break;
					case 2:
						FechaCompromiso = (Oportunidad.CompromisoVentas.ToShortDateString() != "01/01/0001") ? Oportunidad.CompromisoVentas.ToShortDateString() : "";
						FechaTerminado = (Oportunidad.TerminadoVentas.ToShortDateString() != "01/01/0001") ? Oportunidad.TerminadoVentas.ToShortDateString() : "";

						Modelo.Add("FechaCompromiso", FechaCompromiso);
						Modelo.Add("FechaTerminado", FechaTerminado);
						Modelo.Add("Detenido", Oportunidad.VentasDetenido);
						Modelo.Add("TipoFecha", Fecha);
						break;
					case 3:
						FechaCompromiso = (Oportunidad.CompromisoCompras.ToShortDateString() != "01/01/0001") ? Oportunidad.CompromisoCompras.ToShortDateString() : "";
						FechaTerminado = (Oportunidad.TerminadoCompras.ToShortDateString() != "01/01/0001") ? Oportunidad.TerminadoCompras.ToShortDateString() : "";

						Modelo.Add("FechaCompromiso", FechaCompromiso);
						Modelo.Add("FechaTerminado", FechaTerminado);
						Modelo.Add("Detenido", Oportunidad.ComprasDetenido);
						Modelo.Add("TipoFecha", Fecha);
						break;
					case 4:
						FechaCompromiso = (Oportunidad.CompromisoProyectos.ToShortDateString() != "01/01/0001") ? Oportunidad.CompromisoProyectos.ToShortDateString() : "";
						FechaTerminado = (Oportunidad.TerminadoProyectos.ToShortDateString() != "01/01/0001") ? Oportunidad.TerminadoProyectos.ToShortDateString() : "";

						Modelo.Add("FechaCompromiso", FechaCompromiso);
						Modelo.Add("FechaTerminado", FechaTerminado);
						Modelo.Add("Detenido", Oportunidad.ProyectosDetenido);
						Modelo.Add("TipoFecha", Fecha);
						break;
					case 5:
						FechaCompromiso = (Oportunidad.CompromisoFinanzas.ToShortDateString() != "01/01/0001") ? Oportunidad.CompromisoFinanzas.ToShortDateString() : "";
						FechaTerminado = (Oportunidad.TerminadoFinanzas.ToShortDateString() != "01/01/0001") ? Oportunidad.TerminadoFinanzas.ToShortDateString() : "";

						Modelo.Add("FechaCompromiso", FechaCompromiso);
						Modelo.Add("FechaTerminado", FechaTerminado);
						Modelo.Add("Detenido", Oportunidad.FinzanzasDetenido);
						Modelo.Add("TipoFecha", Fecha);
						break;
					default:
						Modelo.Add("FechaCompromiso", "");
						Modelo.Add("FechaTerminado", "");
						Modelo.Add("Detenido", false);
						Modelo.Add("TipoFecha", Fecha);
						break;
				}

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}
	
	[WebMethod]
	public static string GuardarFechasOportunidad (int IdOportunidad, string FechaCompromiso, string FechaTerminado, bool Detenido, int Fecha, string Comentario)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesioin) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				COportunidad Oportunidad = new COportunidad();
				Oportunidad.LlenaObjeto(IdOportunidad, pConexion);

				DateTime Fecha1 = (FechaCompromiso == "") ? default(DateTime) : DateTime.ParseExact(FechaCompromiso, "dd/MM/yyyy", CultureInfo.InvariantCulture);
				DateTime Fecha2 = (FechaTerminado == "") ? default(DateTime) : DateTime.ParseExact(FechaTerminado, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Boolean flag = false;
				switch(Fecha)
				{
					case 1:
                        if (Fecha1 <= Oportunidad.CompromisoVentas)
                        {
                            Oportunidad.CompromisoPreventa = Fecha1;
                            Oportunidad.TerminadoPreventa = Fecha2;
                            Oportunidad.PreventaDetenido = Detenido;
                        }
                        else
                        {
                            flag = true;
                        }
                        break;
					case 2:
                        if (Oportunidad.IdUsuarioCreacion == UsuarioSesioin.IdUsuario)
                        {
                            Oportunidad.CompromisoVentas = Fecha1;
                            Oportunidad.TerminadoVentas = Fecha2;
                            Oportunidad.VentasDetenido = Detenido;
                        }
                        else
                        {
                            flag = true;
                        }
						break;
					case 3:
                        if (Fecha1 <= Oportunidad.CompromisoVentas) {
                            Oportunidad.CompromisoCompras = Fecha1;
                            Oportunidad.TerminadoCompras = Fecha2;
                            Oportunidad.ComprasDetenido = Detenido;
                        }
                        else
                        {
                            flag = true;
                        }
                         break;
					case 4:
                        if (Fecha1 <= Oportunidad.CompromisoVentas)
                        {
                            Oportunidad.CompromisoProyectos = Fecha1;
                            Oportunidad.TerminadoProyectos = Fecha2;
                            Oportunidad.ProyectosDetenido = Detenido;
                        }
                        else
                        {
                            flag = true;
                        }
						break;
					case 5:
                        if (Fecha1 <= Oportunidad.CompromisoVentas)
                        {
                            Oportunidad.CompromisoFinanzas = Fecha1;
                            Oportunidad.TerminadoFinanzas = Fecha2;
                            Oportunidad.FinzanzasDetenido = Detenido;
                        }
                        else
                        {
                            flag = true;
                        }
						break;
				}

				CBitacoraNotasOportunidad Nota = new CBitacoraNotasOportunidad();
				Nota.IdOportunidad = IdOportunidad;
				Nota.IdUsuario = UsuarioSesioin.IdUsuario;
				Nota.FechaCreacion = DateTime.Now;
				Nota.Area = Fecha;
				Nota.Nota = Comentario;

				if (Comentario != "")
				Nota.Agregar(pConexion);

                if (flag)
                {
                    Error = 1;
                    if (Fecha == 2)
                    {
                        DescripcionError = "Solo el Agente de la Oportunidad puede establecer una fecha para Venta.";
                    }
                    else
                    {
                        DescripcionError = "La fecha no puede ser mayor a la fecha Venta.";
                    }
                }
                else
                {
                    Oportunidad.Editar(pConexion);
                }

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ProyectosPedidosAutorizados(string Agente, int IdSucursal)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSession) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_PlaneacionVenta_ReporteProyectoPedidoAutorizacion";
				Consulta.StoredProcedure.Parameters.Add("Agente", SqlDbType.VarChar, 50).Value = Agente;
				Consulta.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;

				Consulta.Llena(pConexion);
				
				while (Consulta.Registros.Read())
				{
					Modelo.Add("ProyectosAutorizados", Convert.ToString(Consulta.Registros["ProyectoAutorizado"]));
					Modelo.Add("ProyectosNoAutorizados", Convert.ToString(Consulta.Registros["ProyectoNoAutorizado"]));
					Modelo.Add("PedidosAutorizados", Convert.ToString(Consulta.Registros["PedidoAutorizado"]));
					Modelo.Add("PedidosNoAutorizados", Convert.ToString(Consulta.Registros["PedidoNoAutorizado"]));
					Modelo.Add("TotalAutorizados", Convert.ToString(Consulta.Registros["TotalAutorizado"]));
					Modelo.Add("TotalNoAutorizados", Convert.ToString(Consulta.Registros["TotalNoAutorizado"]));
				}

				Consulta.CerrarConsulta();

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string SabanaAutorizado()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_SabanaAutorizado";
				Consulta.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 20).Value = CUtilerias.PrimerDiaMes().ToString("yyyy-dd-MM");
				Consulta.StoredProcedure.Parameters.Add("FechaFin", SqlDbType.VarChar, 20).Value = DateTime.Today.ToString("yyyy-dd-MM");
				Consulta.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				JArray Resultado = CUtilerias.ObtenerConsulta(Consulta, pConexion);

				Modelo.Add("SabanaAutorizados", Resultado);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

}