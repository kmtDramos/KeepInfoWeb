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
using System.IO;

public partial class Oportunidad : System.Web.UI.Page
{
	public static int puedeAgregarOportunidad = 0;
	public static int ocultarBarrasResultadosOportunidades = 0;
	public static int puedeConsultarReporteComisiones = 0;
	public static int puedeVerGraficasOportunidades = 1;
	public static string FechaInicio = "";
	public static string FechaFinal = "";
	public static string FechaInicio2 = "";
	public static string FechaFinal2 = "";
	public static string FechaInicio3 = "";
	public static string FechaFinal3 = "";
	public static string mesActual = "";
	public static string mesAnterior = "";
	public static string ticks = "";

	protected void Page_Load(object sender, EventArgs e)
	{

		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		CUsuario Usuario = new CUsuario();
		Usuario.ValidarSession();
		Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

		if (Usuario.TienePermisos(new string[] { "puedeAgregarOportunidad" }, ConexionBaseDatos) == "")
		{
			puedeAgregarOportunidad = 1;
		}
		else
		{
			puedeAgregarOportunidad = 0;
		}

		if (Usuario.TienePermisos(new string[] { "ocultarBarrasResultadosOportunidades" }, ConexionBaseDatos) == "")
		{
			ocultarBarrasResultadosOportunidades = 1;
		}
		else
		{
			ocultarBarrasResultadosOportunidades = 0;
		}

		if (Usuario.TienePermisos(new string[] { "puedeConsultarReporteComisiones" }, ConexionBaseDatos) == "")
		{
			puedeConsultarReporteComisiones = 1;
		}
		else
		{
			puedeConsultarReporteComisiones = 0;
		}

		if (Usuario.TienePermisos(new string[] { "puedeVerGraficasOportunidades" }, ConexionBaseDatos) == "")
		{
			puedeVerGraficasOportunidades = 1;
		}

		CUtilerias Utilerias = new CUtilerias();

		mesActual = Utilerias.ObtenerMes(DateTime.Today.Month);
		mesAnterior = Utilerias.ObtenerMes(DateTime.Today.Month - 1);

		FechaInicio = DateTime.Now.ToString("yyyyMM") + "01";
		FechaFinal = DateTime.Now.ToString("yyyyMMdd");
		FechaInicio2 = DateTime.Now.ToString("yyyyMM") + "01";
		FechaFinal2 = DateTime.Now.ToString("yyyyMMdd");
		FechaInicio3 = DateTime.Now.AddMonths(-1).ToString("yyyyMM") + "01";
		DateTime hoy = DateTime.Now;
		DateTime anteriorMes = DateTime.Now.AddDays(-hoy.Day);
		FechaFinal3 = anteriorMes.ToString("yyyyMMdd");

		ticks = DateTime.Now.Ticks.ToString();

		GenerarGridOportunidades(ConexionBaseDatos);
		GenerarGridReporteComisiones(ConexionBaseDatos);
		GenerarGridClientesOportunidades(ConexionBaseDatos);
		CActividad.GenerarGridActividadesClienteOportunidad("Actividades de oportunidad", this, ClientScript);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();

	}

	private void GenerarGridClientesOportunidades(CConexion pConexion)
	{

		// Grid ClientesOportunidades por Agente
		CJQGrid GridClientesOportunidades = new CJQGrid();
		GridClientesOportunidades.NombreTabla = "grdClientesOportunidades";
		GridClientesOportunidades.CampoIdentificador = "IdFacturaEncabezado";
		GridClientesOportunidades.ColumnaOrdenacion = "RazonSocial";
		GridClientesOportunidades.TipoOrdenacion = "ASC";
		GridClientesOportunidades.Metodo = "ObtenerClientesOportunidades";
		GridClientesOportunidades.TituloTabla = "Clientes por agente";
		GridClientesOportunidades.GenerarGridCargaInicial = false;
		GridClientesOportunidades.GenerarFuncionFiltro = false;
		GridClientesOportunidades.GenerarFuncionTerminado = false;
		GridClientesOportunidades.NumeroFila = false;
		GridClientesOportunidades.Altura = 350;
		GridClientesOportunidades.Ancho = 700;
		GridClientesOportunidades.NumeroRegistros = 30;
		GridClientesOportunidades.RangoNumeroRegistros = "30,50,100";

		// RazonSocial
		CJQColumn ColRazonSocial = new CJQColumn();
		ColRazonSocial.Nombre = "RazonSocial";
		ColRazonSocial.Encabezado = "Razón Social";
		ColRazonSocial.Buscador = "false";
		ColRazonSocial.Alineacion = "left";
		ColRazonSocial.Ancho = "150";
		GridClientesOportunidades.Columnas.Add(ColRazonSocial);

		// Agentes
		CJQColumn ColAgentes = new CJQColumn();
		ColAgentes.Nombre = "Agentes";
		ColAgentes.Encabezado = "Agentes";
		ColAgentes.Buscador = "false";
		ColAgentes.Alineacion = "left";
		ColAgentes.Ancho = "150";
		GridClientesOportunidades.Columnas.Add(ColAgentes);

		// Oportunidades
		CJQColumn ColOportunidades = new CJQColumn();
		ColOportunidades.Nombre = "Oportunidades";
		ColOportunidades.Encabezado = "Oportunidades";
		ColOportunidades.Buscador = "false";
		ColOportunidades.Alineacion = "left";
		ColOportunidades.Ancho = "50";
		GridClientesOportunidades.Columnas.Add(ColOportunidades);

		// Cotizaciones
		CJQColumn ColCotizaciones = new CJQColumn();
		ColCotizaciones.Nombre = "Cotizaciones";
		ColCotizaciones.Encabezado = "Cotizaciones";
		ColCotizaciones.Buscador = "false";
		ColCotizaciones.Alineacion = "left";
		ColCotizaciones.Ancho = "50";
		GridClientesOportunidades.Columnas.Add(ColCotizaciones);

		// Pedidos
		CJQColumn ColPedidos = new CJQColumn();
		ColPedidos.Nombre = "Pedidos";
		ColPedidos.Encabezado = "Pedidos";
		ColPedidos.Buscador = "false";
		ColPedidos.Alineacion = "left";
		ColPedidos.Ancho = "50";
		GridClientesOportunidades.Columnas.Add(ColPedidos);

		// Proyectos
		CJQColumn ColProyectos = new CJQColumn();
		ColProyectos.Nombre = "Proyectos";
		ColProyectos.Encabezado = "Proyectos";
		ColProyectos.Buscador = "false";
		ColProyectos.Alineacion = "left";
		ColProyectos.Ancho = "50";
		GridClientesOportunidades.Columnas.Add(ColProyectos);

		ClientScript.RegisterStartupScript(this.GetType(), "grdReporteComision", GridClientesOportunidades.GeneraGrid(), true);

	}

	private void GenerarGridReporteComisiones(CConexion pConexion)
	{

		//############################# Drid Reporte Comisiones ###########################

		//GridReporteComision
		CJQGrid GridReporteComision = new CJQGrid();
		GridReporteComision.NombreTabla = "grdReporteComision";
		GridReporteComision.CampoIdentificador = "IdFacturaEncabezado";
		GridReporteComision.ColumnaOrdenacion = "IdFacturaEncabezado";
		GridReporteComision.TipoOrdenacion = "ASC";
		GridReporteComision.Metodo = "ObtenerReporteComisiones";
		GridReporteComision.TituloTabla = "Reporte de comisiones";
		GridReporteComision.GenerarGridCargaInicial = false;
		GridReporteComision.GenerarFuncionFiltro = false;
		GridReporteComision.GenerarFuncionTerminado = false;
		GridReporteComision.NumeroFila = false;
		GridReporteComision.Altura = 150;
		GridReporteComision.Ancho = 980;
		GridReporteComision.NumeroRegistros = 10;
		GridReporteComision.RangoNumeroRegistros = "10,20,30";

		//IdReporteComision
		CJQColumn ColIdFacturaEncabezado = new CJQColumn();
		ColIdFacturaEncabezado.Nombre = "IdFacturaEncabezado";
		ColIdFacturaEncabezado.Oculto = "true";
		ColIdFacturaEncabezado.Encabezado = "IdFacturaEncabezado";
		ColIdFacturaEncabezado.Buscador = "false";
		GridReporteComision.Columnas.Add(ColIdFacturaEncabezado);

		//Serie
		CJQColumn ColSerie = new CJQColumn();
		ColSerie.Nombre = "Serie";
		ColSerie.Encabezado = "Serie";
		ColSerie.Buscador = "false";
		ColSerie.Alineacion = "left";
		ColSerie.Ancho = "50";
		GridReporteComision.Columnas.Add(ColSerie);

		//NumeroFactura
		CJQColumn ColNumeroFactura = new CJQColumn();
		ColNumeroFactura.Nombre = "NumeroFactura";
		ColNumeroFactura.Encabezado = "No Factura";
		ColNumeroFactura.Ancho = "50";
		ColNumeroFactura.Buscador = "false";
		ColNumeroFactura.Alineacion = "left";
		GridReporteComision.Columnas.Add(ColNumeroFactura);

		//RazonSocialCliente
		CJQColumn ColCliente = new CJQColumn();
		ColCliente.Nombre = "RazonSocial";
		ColCliente.Encabezado = "Cliente";
		ColCliente.Buscador = "false";
		ColCliente.Alineacion = "left";
		ColCliente.Ancho = "200";
		GridReporteComision.Columnas.Add(ColCliente);

		//FechaEmision
		CJQColumn ColFechaEmision = new CJQColumn();
		ColFechaEmision.Nombre = "FechaEmision";
		ColFechaEmision.Encabezado = "Fecha Emision";
		ColFechaEmision.Buscador = "false";
		ColFechaEmision.Alineacion = "left";
		ColFechaEmision.Ancho = "100";
		GridReporteComision.Columnas.Add(ColFechaEmision);

		//Agente
		CJQColumn ColAgente = new CJQColumn();
		ColAgente.Nombre = "Agente";
		ColAgente.Encabezado = "Agente";
		ColAgente.Buscador = "false";
		ColAgente.Alineacion = "left";
		ColAgente.Ancho = "200";
		GridReporteComision.Columnas.Add(ColAgente);

		//EstatusFacturaEncabezado
		CJQColumn ColEstatusFacturaEncabezado = new CJQColumn();
		ColEstatusFacturaEncabezado.Nombre = "EstatusFacturaEncabezado";
		ColEstatusFacturaEncabezado.Encabezado = "Estatus";
		ColEstatusFacturaEncabezado.Buscador = "false";
		ColEstatusFacturaEncabezado.Alineacion = "left";
		ColEstatusFacturaEncabezado.Ancho = "100";
		GridReporteComision.Columnas.Add(ColEstatusFacturaEncabezado);

		//Dias vencidos 
		CJQColumn ColDiasVencidos = new CJQColumn();
		ColDiasVencidos.Nombre = "DiasVencidos";
		ColDiasVencidos.Encabezado = "Dias Vencidos";
		ColDiasVencidos.Buscador = "false";
		ColDiasVencidos.Alineacion = "left";
		ColDiasVencidos.Ancho = "50";
		GridReporteComision.Columnas.Add(ColDiasVencidos);

		//Nota Credito
		CJQColumn ColNotaCredito = new CJQColumn();
		ColNotaCredito.Nombre = "NotaCredito";
		ColNotaCredito.Encabezado = "Nota de credito";
		ColNotaCredito.Buscador = "false";
		ColNotaCredito.Alineacion = "left";
		ColNotaCredito.Ancho = "100";
		GridReporteComision.Columnas.Add(ColNotaCredito);

		//Total Nota Credito
		CJQColumn ColTotalNotaCredito = new CJQColumn();
		ColTotalNotaCredito.Nombre = "TotalNotaCredito";
		ColTotalNotaCredito.Encabezado = "Total nota credito";
		ColTotalNotaCredito.Buscador = "false";
		ColTotalNotaCredito.Alineacion = "left";
		ColTotalNotaCredito.Ancho = "80";
		GridReporteComision.Columnas.Add(ColTotalNotaCredito);

		////Folio Nota Credito
		//CJQColumn ColFolioNotaCredito = new CJQColumn();
		//ColFolioNotaCredito.Nombre = "FolioNotaCredito";
		//ColFolioNotaCredito.Encabezado = "Folio nota credito";
		//ColFolioNotaCredito.Buscador = "false";
		//ColFolioNotaCredito.Alineacion = "left";
		//ColFolioNotaCredito.Ancho = "50";
		//GridReporteComision.Columnas.Add(ColFolioNotaCredito);

		//SubTotal
		CJQColumn ColSubTotal = new CJQColumn();
		ColSubTotal.Nombre = "SubTotal";
		ColSubTotal.Encabezado = "Subtotal";
		ColSubTotal.Buscador = "false";
		ColSubTotal.Alineacion = "left";
		ColSubTotal.Ancho = "80";
		GridReporteComision.Columnas.Add(ColSubTotal);

		//Total
		CJQColumn ColTotal = new CJQColumn();
		ColTotal.Nombre = "Total";
		ColTotal.Encabezado = "Total";
		ColTotal.Buscador = "false";
		ColTotal.Alineacion = "left";
		ColTotal.Ancho = "80";
		GridReporteComision.Columnas.Add(ColTotal);


		//TipoCambio
		CJQColumn ColTipoCambio = new CJQColumn();
		ColTipoCambio.Nombre = "TipoCambio";
		ColTipoCambio.Encabezado = "Tipo de cambio";
		ColTipoCambio.Buscador = "false";
		ColTipoCambio.Alineacion = "left";
		ColTipoCambio.Ancho = "80";
		GridReporteComision.Columnas.Add(ColTipoCambio);

		//Total en Pesos
		CJQColumn ColTotalPesos = new CJQColumn();
		ColTotalPesos.Nombre = "TotalPesos";
		ColTotalPesos.Encabezado = "Total en pesos";
		ColTotalPesos.Buscador = "false";
		ColTotalPesos.Alineacion = "left";
		ColTotalPesos.Ancho = "80";
		GridReporteComision.Columnas.Add(ColTotalPesos);

		//Moneda
		CJQColumn ColMoneda = new CJQColumn();
		ColMoneda.Nombre = "Moneda";
		ColMoneda.Encabezado = "Moneda";
		ColMoneda.Buscador = "false";
		ColMoneda.Alineacion = "left";
		ColMoneda.Ancho = "80";
		GridReporteComision.Columnas.Add(ColMoneda);

		//Division
		CJQColumn ColDivision = new CJQColumn();
		ColDivision.Nombre = "Division";
		ColDivision.Encabezado = "División";
		ColDivision.Buscador = "false";
		ColDivision.Alineacion = "left";
		ColDivision.Ancho = "100";
		GridReporteComision.Columnas.Add(ColDivision);

		ClientScript.RegisterStartupScript(this.GetType(), "grdReporteComision", GridReporteComision.GeneraGrid(), true);

	}

	private void GenerarGridOportunidades(CConexion pConexion)
	{
		string puedeDesactivarOportunidad = "true";
		string puedeConsultarOportunidad = "true";

		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);

		if (Usuario.TienePermisos(new string[] { "puedeConsultarOportunidad" }, pConexion) == "")
		{
			puedeConsultarOportunidad = "false";
		}

		if (Usuario.TienePermisos(new string[] { "puedeDesactivarOportunidad" }, pConexion) == "")
		{
			puedeDesactivarOportunidad = "false";
		}

		//GridOportunidad
		CJQGrid GridOportunidad = new CJQGrid();
		GridOportunidad.NombreTabla = "grdOportunidad";
		GridOportunidad.CampoIdentificador = "IdOportunidad";
		GridOportunidad.ColumnaOrdenacion = "IdOportunidad";
		GridOportunidad.TipoOrdenacion = "DESC";
		GridOportunidad.Metodo = "ObtenerOportunidad";
		GridOportunidad.TituloTabla = "Catálogo de oportunidades";
		GridOportunidad.GenerarGridCargaInicial = false;
		GridOportunidad.GenerarFuncionFiltro = false;
		GridOportunidad.GenerarFuncionTerminado = true;
		GridOportunidad.NumeroFila = false;
		GridOportunidad.Altura = 230;
		GridOportunidad.NumeroRegistros = 10;
		GridOportunidad.RangoNumeroRegistros = "10,25,40";

		//IdOportunidad
		CJQColumn ColIdOpotunidad = new CJQColumn();
		ColIdOpotunidad.Nombre = "IdOportunidad";
		ColIdOpotunidad.Encabezado = "No.";
		ColIdOpotunidad.Oculto = "false";
		ColIdOpotunidad.Ancho = "50";
		GridOportunidad.Columnas.Add(ColIdOpotunidad);

		//Oportunidad
		CJQColumn ColOportunidad = new CJQColumn();
		ColOportunidad.Nombre = "Oportunidad";
		ColOportunidad.Encabezado = "Oportunidad";
		ColOportunidad.Ancho = "100";
		ColOportunidad.Alineacion = "Left";
		GridOportunidad.Columnas.Add(ColOportunidad);

		//Agente
		CJQColumn ColAgente = new CJQColumn();
		ColAgente.Nombre = "Agente";
		ColAgente.Encabezado = "Agente";
		ColAgente.Ancho = "100";
		ColAgente.Alineacion = "Left";
		GridOportunidad.Columnas.Add(ColAgente);

		//Cliente
		CJQColumn ColCliente = new CJQColumn();
		ColCliente.Nombre = "Cliente";
		ColCliente.Encabezado = "Cliente";
		ColCliente.Ancho = "100";
		ColCliente.Alineacion = "Left";
		ColCliente.Buscador = "true";
		GridOportunidad.Columnas.Add(ColCliente);

		//Interes
		CJQColumn ColInteres = new CJQColumn();
		ColInteres.Nombre = "NivelInteres";
		ColInteres.Encabezado = "Nivel Interes";
		ColInteres.Ancho = "60";
		ColInteres.Alineacion = "Center";
		ColInteres.Buscador = "true";
		ColInteres.TipoBuscador = "Combo";
		ColInteres.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad";
		GridOportunidad.Columnas.Add(ColInteres);

		//Sucursal
		CJQColumn ColSucursal = new CJQColumn();
		ColSucursal.Nombre = "Sucursal";
		ColSucursal.Encabezado = "Sucursal";
		ColSucursal.Ancho = "60";
		ColSucursal.Alineacion = "Center";
		ColSucursal.Buscador = "true";
		ColSucursal.TipoBuscador = "Combo";
		ColSucursal.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Sucursal";
		ColSucursal.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
		GridOportunidad.Columnas.Add(ColSucursal);

		//Fecha
		CJQColumn ColFecha = new CJQColumn();
		ColFecha.Nombre = "FechaCreacion";
		ColFecha.Encabezado = "Fecha";
		ColFecha.Ancho = "100";
		ColFecha.Alineacion = "Right";
		ColFecha.Buscador = "false";
		GridOportunidad.Columnas.Add(ColFecha);

		//Fecha
		CJQColumn ColFechaCierre = new CJQColumn();
		ColFechaCierre.Nombre = "FechaCierre";
		ColFechaCierre.Encabezado = "Fecha cierre";
		ColFechaCierre.Ancho = "100";
		ColFechaCierre.Alineacion = "Right";
		ColFechaCierre.Buscador = "false";
		GridOportunidad.Columnas.Add(ColFechaCierre);

		//Dias
		CJQColumn ColDias = new CJQColumn();
		ColDias.Nombre = "Dias";
		ColDias.Encabezado = "Días";
		ColDias.Ancho = "60";
		ColDias.Alineacion = "Center";
		ColDias.Buscador = "false";
		GridOportunidad.Columnas.Add(ColDias);

		//Monto
		CJQColumn ColMonto = new CJQColumn();
		ColMonto.Nombre = "Monto";
		ColMonto.Encabezado = "Monto";
		ColMonto.Ancho = "100";
		ColMonto.Alineacion = "Right";
		ColMonto.Formato = "FormatoMoneda";
		ColMonto.Buscador = "true";
		GridOportunidad.Columnas.Add(ColMonto);

		//Cotizaciones
		CJQColumn ColCotizaciones = new CJQColumn();
		ColCotizaciones.Nombre = "Cotizaciones";
		ColCotizaciones.Encabezado = "Cotizado";
		ColCotizaciones.Ancho = "100";
		ColCotizaciones.Alineacion = "Right";
		ColCotizaciones.Formato = "FormatoMoneda";
		ColCotizaciones.Buscador = "false";
		GridOportunidad.Columnas.Add(ColCotizaciones);

		//Pedidos
		CJQColumn ColPedidos = new CJQColumn();
		ColPedidos.Nombre = "Pedidos";
		ColPedidos.Encabezado = "Pedidos";
		ColPedidos.Ancho = "100";
		ColPedidos.Alineacion = "Right";
		ColPedidos.Formato = "FormatoMoneda";
		ColPedidos.Buscador = "false";
		GridOportunidad.Columnas.Add(ColPedidos);

		//Proyectos
		CJQColumn ColProyectos = new CJQColumn();
		ColProyectos.Nombre = "Proyectos";
		ColProyectos.Encabezado = "Proyectos";
		ColProyectos.Ancho = "100";
		ColProyectos.Alineacion = "Right";
		ColProyectos.Formato = "FormatoMoneda";
		ColProyectos.Buscador = "false";
		GridOportunidad.Columnas.Add(ColProyectos);

		//Nota de credito
		//CJQColumn ColNotaCredito = new CJQColumn();
		//ColNotaCredito.Nombre = "NotaCredito";
		//ColNotaCredito.Encabezado = "Nota de credito";
		//ColNotaCredito.Ancho = "100";
		//ColNotaCredito.Alineacion = "Right";
		//ColNotaCredito.Formato = "FormatoMoneda";
		//ColNotaCredito.Buscador = "false";
		//GridOportunidad.Columnas.Add(ColNotaCredito);

		//Facturas
		CJQColumn ColFacturas = new CJQColumn();
		ColFacturas.Nombre = "Facturas";
		ColFacturas.Encabezado = "Facturado";
		ColFacturas.Ancho = "100";
		ColFacturas.Alineacion = "Right";
		ColFacturas.Formato = "FormatoMoneda";
		ColFacturas.Buscador = "false";
		GridOportunidad.Columnas.Add(ColFacturas);

		//Clasificacion
		CJQColumn ColClasificacion = new CJQColumn();
		ColClasificacion.Nombre = "Clasificacion";
		ColClasificacion.Encabezado = "Clasificación";
		ColClasificacion.Ancho = "100";
		ColClasificacion.Alineacion = "Left";
		ColClasificacion.Buscador = "true";
		ColClasificacion.TipoBuscador = "Combo";
		ColClasificacion.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Clasificacion";
		GridOportunidad.Columnas.Add(ColClasificacion);

		//Campaña
		CJQColumn ColCampana = new CJQColumn();
		ColCampana.Nombre = "Campana";
		ColCampana.Encabezado = "Campaña";
		ColCampana.Ancho = "100";
		ColCampana.Alineacion = "Left";
		ColCampana.Buscador = "true";
		ColCampana.TipoBuscador = "Combo";
		ColCampana.StoredProcedure.CommandText = "sp_CampanaCombo";
		GridOportunidad.Columnas.Add(ColCampana);

		//Division
		CJQColumn ColDivision = new CJQColumn();
		ColDivision.Nombre = "Division";
		ColDivision.Encabezado = "División";
		ColDivision.Ancho = "100";
		ColDivision.Alineacion = "Left";
		ColDivision.Buscador = "true";
		ColDivision.TipoBuscador = "Combo";
		ColDivision.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Division";
		GridOportunidad.Columnas.Add(ColDivision);

		//Cerrado
		CJQColumn ColCerrado = new CJQColumn();
		ColCerrado.Nombre = "Cerrado";
		ColCerrado.Encabezado = "Cerrado";
		ColCerrado.Ancho = "80";
		ColCerrado.Alineacion = "Left";
		ColCerrado.Buscador = "true";
		ColCerrado.TipoBuscador = "Combo";
		ColCerrado.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Cerradas";
		GridOportunidad.Columnas.Add(ColCerrado);

		CJQColumn ColEsProyecto = new CJQColumn();
		ColEsProyecto.Nombre = "EsProyecto";
		ColEsProyecto.Encabezado = "Es proyecto";
		ColEsProyecto.Ancho = "50";
		ColEsProyecto.Alineacion = "Center";
		ColEsProyecto.Buscador = "true";
		ColEsProyecto.TipoBuscador = "Combo";
		ColEsProyecto.StoredProcedure.CommandText = "sp_Oportunidad_FiltroProyecto";
		GridOportunidad.Columnas.Add(ColEsProyecto);

		CJQColumn ColUrgente = new CJQColumn();
		ColUrgente.Nombre = "Urgente";
		ColUrgente.Encabezado = "Urgente";
		ColUrgente.Ancho = "50";
		ColUrgente.Alineacion = "Center";
		ColUrgente.Buscador = "true";
		ColUrgente.TipoBuscador = "Combo";
		ColUrgente.StoredProcedure.CommandText = "sp_Oportunidad_FiltroProyecto";
		GridOportunidad.Columnas.Add(ColUrgente);

		CJQColumn ColActividades = new CJQColumn();
		ColActividades.Nombre = "Actividades";
		ColActividades.Encabezado = "Acts";
		ColActividades.Ancho = "50";
		ColActividades.Alineacion = "Center";
		ColActividades.Buscador = "false";
		GridOportunidad.Columnas.Add(ColActividades);

		//FechaNota
		CJQColumn ColMensajeNota = new CJQColumn();
		ColMensajeNota.Nombre = "MensajeNota";
		ColMensajeNota.Encabezado = "Ultima Nota";
		ColMensajeNota.Ancho = "120";
		ColMensajeNota.Oculto = "true";
		ColMensajeNota.Alineacion = "Left";
		ColMensajeNota.Buscador = "false";
		GridOportunidad.Columnas.Add(ColMensajeNota);

		//FechaNota
		CJQColumn ColFechaNota = new CJQColumn();
		ColFechaNota.Nombre = "FechaNota";
		ColFechaNota.Encabezado = "Fecha Nota";
		ColFechaNota.Ancho = "120";
		ColFechaNota.Alineacion = "Left";
		ColFechaNota.Buscador = "false";
		GridOportunidad.Columnas.Add(ColFechaNota);

		//Baja
		CJQColumn ColBaja = new CJQColumn();
		ColBaja.Nombre = "AI";
		ColBaja.Encabezado = "A/I";
		ColBaja.Ordenable = "false";
		ColBaja.Etiquetado = "A/I";
		ColBaja.Ancho = "55";
		ColBaja.Buscador = "true";
		ColBaja.TipoBuscador = "Combo";
		ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
		ColBaja.Oculto = puedeDesactivarOportunidad;
		GridOportunidad.Columnas.Add(ColBaja);

		//Consultar
		CJQColumn ColConsultar = new CJQColumn();
		ColConsultar.Nombre = "Consultar";
		ColConsultar.Encabezado = "Ver";
		ColConsultar.Etiquetado = "ImagenConsultar";
		ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarOportunidad";
		ColConsultar.Buscador = "false";
		ColConsultar.Ordenable = "false";
		ColConsultar.Oculto = puedeConsultarOportunidad;
		ColConsultar.Ancho = "25";
		GridOportunidad.Columnas.Add(ColConsultar);

		ClientScript.RegisterStartupScript(this.GetType(), "grdOportunidad", GridOportunidad.GeneraGrid(), true);
	}

	private void GenerarGridComentarios(CConexion pConexion)
	{



	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerOportunidad(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pCampana, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdOportunidad", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
		Stored.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = Convert.ToString(pOportunidad);
		Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
		Stored.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
		Stored.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
		Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pSucursal;
		Stored.Parameters.Add("pMonto", SqlDbType.VarChar, 255).Value = (pMonto == "") ? "0" : Convert.ToString(pMonto);
		Stored.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
		Stored.Parameters.Add("pCampana", SqlDbType.Int).Value = pCampana;
		Stored.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
		Stored.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
		Stored.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
		Stored.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
		Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
		Stored.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
		Stored.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;

		int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

		if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidadesDeSucursalesAsignadas" }, ConexionBaseDatos) == "")
		{
			Stored.Parameters.Add("pPuedeVerSucursalesAsignadas", SqlDbType.Int).Value = 1;
		}
		else
		{
			Stored.Parameters.Add("pPuedeVerSucursalesAsignadas", SqlDbType.Int).Value = 0;
		}

		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

	[WebMethod]
	public static string ObtenerResultadoVentas(string pUsuario, int pIdSucursal, string pFechaInicio, string pFechaFinal)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

		if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) != "")
		{
			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidadesDeSucursalesAsignadas" }, ConexionBaseDatos) == "")
			{
				pIdSucursal = Usuario.IdSucursalActual;
			}
			else
			{
				pUsuario = Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno;
			}
		}
		DateTime hoy = DateTime.Now;

		COportunidad JsonResultado = new COportunidad();
		JsonResultado.StoredProcedure.CommandText = "sp_Oportunidad_ObtenerResultadoVentas";
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pUsuario", pUsuario);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pFechaIncial", pFechaInicio);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]));
		return JsonResultado.ObtenerJsonOportunidad(ConexionBaseDatos);

	}

	[WebMethod]
	public static string ObtenerResultadoVentasMesAnterior(string pUsuario, int pIdSucursal, string pFechaInicio, string pFechaFinal)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

		if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) != "")
		{
			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidadesDeSucursalesAsignadas" }, ConexionBaseDatos) == "")
			{
				pIdSucursal = Usuario.IdSucursalActual;
			}
			else
			{
				pUsuario = Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno;
			}
		}
		DateTime hoy = DateTime.Now;

		COportunidad JsonResultado = new COportunidad();
		JsonResultado.StoredProcedure.CommandText = "sp_Oportunidad_ObtenerResultadoVentasMesAnterior";
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pUsuario", pUsuario);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicio);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
		JsonResultado.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]));
		return JsonResultado.ObtenerJsonOportunidad(ConexionBaseDatos);

	}

	[WebMethod]
	public static string ObtenerUsuariosAsignar(string pUsuario)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		COportunidad JsonUsuarios = new COportunidad();
		JsonUsuarios.StoredProcedure.CommandText = "sp_Oportunidad_ObtenerUsuarioAsignado";
		JsonUsuarios.StoredProcedure.Parameters.AddWithValue("@pUsuario", pUsuario);
		string sJson = JsonUsuarios.ObtenerJsonOportunidad(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return sJson;

	}

	[WebMethod]
	public static string ObtenerMetricasUsuario(string pFechaInicio, string pFechaFin, string pUsuario)
	{
		CConexion ConexionBaseDatos = new CConexion();
		var respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		JObject oRespuesta = new JObject();

		if (respuesta == "Conexion Establecida")
		{
			CUsuario Usuario = new CUsuario();
			JObject Modelo = new JObject();
			Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

			decimal Meta = 0;

			if (Usuario.EsVendedor)
			{
				Meta = Usuario.Meta;
			}
			else
			{
				CSelectEspecifico ConsultarMeta = new CSelectEspecifico();
				ConsultarMeta.StoredProcedure.CommandText = "sp_Meta_ConsultarMetaDeVentas";
				ConsultarMeta.StoredProcedure.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Usuario.IdUsuario;
				ConsultarMeta.Llena(ConexionBaseDatos);

				if (ConsultarMeta.Registros.Read())
				{
					Meta = Convert.ToDecimal(ConsultarMeta.Registros["Meta"]);
				}

				ConsultarMeta.CerrarConsulta();
			}

			Modelo.Add(new JProperty("Alcance1", Usuario.Alcance1));
			Modelo.Add(new JProperty("Alcance2", Usuario.Alcance2));
			Modelo.Add(new JProperty("Meta", Meta));
			int pVerTodos = 0;
			int clientesNuevos = CCliente.ClientesNuevos(pFechaInicio, pFechaFin, "", pVerTodos, ConexionBaseDatos);

			Modelo.Add(new JProperty("ClienteNuevos", Usuario.ClientesNuevos + "/" + clientesNuevos));

			oRespuesta.Add(new JProperty("Error", 0));
			oRespuesta.Add(new JProperty("Modelo", Modelo));
		}
		else
		{
			oRespuesta.Add(new JProperty("Error", 1));
			oRespuesta.Add(new JProperty("Descripcion", "No se pudo establecer la conexión a la base de datos"));
		}

		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return oRespuesta.ToString();
	}

	[WebMethod]
	public static string BuscarMonto(string pMonto)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		COportunidad JsonMonto = new COportunidad();
		JsonMonto.StoredProcedure.CommandText = "sp_Oportunidad_ConsultarFiltrosMontoGrid";
		JsonMonto.StoredProcedure.Parameters.AddWithValue("@pMonto", pMonto);
		string sJson = JsonMonto.ObtenerJsonOportunidad(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return sJson;
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
	public static string AgregarActividad()
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{

			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string AgregarComentarioOportunidad(int pIdOportunidad, string pComentario)

    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CBitacoraNotasOportunidad Comentario = new CBitacoraNotasOportunidad();
                DateTime ahora = DateTime.Now;
                Comentario.IdOportunidad = pIdOportunidad;
                Comentario.Nota = pComentario;
                Comentario.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                Comentario.Area = 0;
                Comentario.FechaCreacion = ahora;

                COportunidad Oportunidad = new COportunidad();
                Oportunidad.LlenaObjeto(pIdOportunidad, pConexion);
                Oportunidad.FechaNota = ahora;
                Oportunidad.UltimaNota = pComentario;

                Comentario.Agregar(pConexion);
                Oportunidad.Editar(pConexion);

                Modelo.Add("Comentarios", CBitacoraNotasOportunidad.ObtenerComentariosOportunidadDesc(pIdOportunidad, pConexion));

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
	}

	[WebMethod]
	public static string AgregarOportunidad(Dictionary<string, object> pOportunidad)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = "";
		respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		if (respuesta == "Conexion Establecida")
		{
			JObject oRespuesta = new JObject();
			COportunidad Oportunidad = new COportunidad();
			int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			Oportunidad.Oportunidad = Convert.ToString(pOportunidad["pOportunidad"]);
			Oportunidad.IdNivelInteresOportunidad = Convert.ToInt32(pOportunidad["IdNivelInteresOportunidad"]);
			Oportunidad.IdUsuarioCreacion = IdUsuario;
			Oportunidad.IdSucursal = Usuario.IdSucursalActual;
			Oportunidad.IdCliente = Convert.ToInt32(pOportunidad["pIdCliente"]);
			Oportunidad.Monto = Convert.ToDecimal(pOportunidad["pMonto"]);
			Oportunidad.FechaCierre = Convert.ToDateTime(pOportunidad["pFechaCierre"]);
			Oportunidad.Clasificacion = Convert.ToBoolean(0);
			Oportunidad.FechaCreacion = Convert.ToDateTime(DateTime.Now.ToShortDateString());
			Oportunidad.IdDivision = Convert.ToInt32(pOportunidad["pIdDivision"]);
			Oportunidad.Cotizaciones = 0;
			Oportunidad.Pedidos = 0;
			Oportunidad.Proyectos = 0;
			Oportunidad.Facturas = 0;
			if (Convert.ToInt32(pOportunidad["pEsProyecto"]) == 0) { Oportunidad.EsProyecto = false; } else { Oportunidad.EsProyecto = true; }
			if (Convert.ToInt32(pOportunidad["pUrgente"]) == 0)
			{
				Oportunidad.Urgente = false;
			}
			else
			{
				if (Oportunidad.IdNivelInteresOportunidad == 1) { Oportunidad.Urgente = true; } else { Oportunidad.Urgente = false; }
			}
			//Oportunidad.NotaCredito = 0;
			//Oportunidad.Facturado = 0;
			//Oportunidad.Cerrado = 0;
			Oportunidad.IdCampana = Convert.ToInt32(pOportunidad["pIdCampana"]);
			Oportunidad.Proveedores = Convert.ToString(pOportunidad["pProveedores"]);
			Oportunidad.Utilidad = Convert.ToInt32(pOportunidad["pUtilidad"]);
			Oportunidad.Costo = Convert.ToDecimal(pOportunidad["pCosto"]);

			string validacion = ValidarOportunidad(Oportunidad, ConexionBaseDatos);
			if (validacion == "")
			{
				Oportunidad.Agregar(ConexionBaseDatos);
				oRespuesta.Add(new JProperty("Error", 0));
				respuesta = oRespuesta.ToString();
			}
			else
			{
				oRespuesta.Add(new JProperty("Error", 1));
				oRespuesta.Add(new JProperty("Descripcion", validacion));
				respuesta = oRespuesta.ToString();
			}
		}
		else
		{
			JObject oRespuesta = new JObject();
			oRespuesta.Add(new JProperty("Error", 1));
			oRespuesta.Add(new JProperty("Descripcion", respuesta));
			respuesta = oRespuesta.ToString();
		}

		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return respuesta;
	}

	[WebMethod]
	public static string EditarOportunidad(Dictionary<string, object> pOportunidad)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = "";
		respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		if (respuesta == "Conexion Establecida")
		{
			JObject oRespuesta = new JObject();
			COportunidad Oportunidad = new COportunidad();

			int IdUsuario = Convert.ToInt32(pOportunidad["pIdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			CCotizacion Cotizacion = new CCotizacion();
			CProyecto Proyecto = new CProyecto();
			Dictionary<string, object> Parametros = new Dictionary<string, object>();
			Parametros.Add("IdOportunidad", Convert.ToInt32(pOportunidad["pIdOportunidad"]));
			Parametros.Add("Baja", 0);

			Cotizacion.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
			Proyecto.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

			Oportunidad.LlenaObjeto(Convert.ToInt32(pOportunidad["pIdOportunidad"]), ConexionBaseDatos);
			Oportunidad.Oportunidad = Convert.ToString(pOportunidad["pOportunidad"]);
			Oportunidad.Monto = Convert.ToDecimal(pOportunidad["pMonto"]);
			Oportunidad.FechaCierre = Convert.ToDateTime(pOportunidad["pFechaCierre"]);
			if (Cotizacion.IdCotizacion == 0 || Proyecto.IdProyecto == 0)
			{
				Oportunidad.IdCliente = Convert.ToInt32(pOportunidad["pIdCliente"]);
			}
			Oportunidad.IdSucursal = Usuario.IdSucursalPredeterminada;
			Oportunidad.IdNivelInteresOportunidad = Convert.ToInt32(pOportunidad["IdNivelInteresOportunidad"]);
			Oportunidad.Clasificacion = Convert.ToBoolean(pOportunidad["pClasificacion"]);
			Oportunidad.IdDivision = Convert.ToInt32(pOportunidad["pDivision"]);
			Oportunidad.IdCampana = Convert.ToInt32(pOportunidad["pCampana"]);
			if (!Oportunidad.Cerrado && Convert.ToBoolean(pOportunidad["pCerrada"]))
			{
				Oportunidad.FechaCierre = DateTime.Now;
			}
			Oportunidad.Cerrado = Convert.ToBoolean(pOportunidad["pCerrada"]);
			if (Convert.ToInt32(pOportunidad["pEsProyecto"]) == 0) { Oportunidad.EsProyecto = false; } else { Oportunidad.EsProyecto = true; }
			if (Convert.ToInt32(pOportunidad["pUrgente"]) == 0) { Oportunidad.Urgente = false; } else { Oportunidad.Urgente = true; }

			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
			{
				Oportunidad.IdUsuarioCreacion = Convert.ToInt32(pOportunidad["pIdUsuario"]);
			}
			Oportunidad.Proveedores = Convert.ToString(pOportunidad["pProveedores"]);
			Oportunidad.Utilidad = Convert.ToInt32(pOportunidad["pUtilidad"]);
			Oportunidad.Costo = Convert.ToDecimal(pOportunidad["pCosto"]);

			string validacion = ValidarOportunidad(Oportunidad, ConexionBaseDatos);
			if (validacion == "")
			{
				Oportunidad.Editar(ConexionBaseDatos);
				oRespuesta.Add(new JProperty("Error", 0));
				respuesta = oRespuesta.ToString();
                /*
                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                Parametros.Clear();
                Parametros.Add("IdOportunidad", Oportunidad.IdOportunidad);
                solicitudLevantamiento.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                if (solicitudLevantamiento.IdSolicitudLevantamiento != 0)
                {
                    solicitudLevantamiento.IdDivision = Oportunidad.IdDivision;
                    solicitudLevantamiento.Editar(ConexionBaseDatos);
                }*/
            }
			else
			{
				oRespuesta.Add(new JProperty("Error", 1));
				oRespuesta.Add(new JProperty("Descripcion", validacion));
				respuesta = oRespuesta.ToString();
			}
		}
		else
		{
			JObject oRespuesta = new JObject();
			oRespuesta.Add(new JProperty("Error", 1));
			oRespuesta.Add(new JProperty("Descripcion", respuesta));
			respuesta = oRespuesta.ToString();
		}
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return respuesta;
	}

	[WebMethod]
	public static string CambiarEstatus(int pIdOportunidad, string pMotivoCancelacion, bool pBaja)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		//¿La conexion se establecio?
		if (respuesta == "Conexion Establecida")
		{
			COportunidad Oportunidad = new COportunidad();
			Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);
			Oportunidad.IdOportunidad = pIdOportunidad;
			Oportunidad.MotivoCancelacion = pMotivoCancelacion;
			Oportunidad.Baja = pBaja;

			CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
			HistorialGenerico.IdGenerico = Oportunidad.IdOportunidad;
			HistorialGenerico.IdUsuario = Convert.ToInt32(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
			HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
			HistorialGenerico.Comentario = "Se dio de baja la oportunidad";
			HistorialGenerico.AgregarHistorialGenerico("Oportunidad", ConexionBaseDatos);

			Oportunidad.Editar(ConexionBaseDatos);
			respuesta = "0|TiempoEntregaEliminado";
		}

		//Cerrar Conexion
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return respuesta;
	}

	[WebMethod]
	public static string ObtenerFormaOportunidad(int pIdOportunidad)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		JObject oRespuesta = new JObject();

		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			COportunidad Oportunidad = new COportunidad();
			Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);
			Modelo.Add(new JProperty("IdOportunidad", pIdOportunidad));
			Modelo.Add(new JProperty("Oportunidad", Oportunidad.Oportunidad));

			CNivelInteresOportunidad NivelInteresOportunidad = new CNivelInteresOportunidad();
			NivelInteresOportunidad.LlenaObjeto(Oportunidad.IdNivelInteresOportunidad, ConexionBaseDatos);
			Modelo.Add(new JProperty("NivelInteresOportunidad", NivelInteresOportunidad.NivelInteresOportunidad));

			Modelo.Add(new JProperty("Monto", Oportunidad.Monto));

			CCliente Cliente = new CCliente();
			Cliente.LlenaObjeto(Oportunidad.IdCliente, ConexionBaseDatos);
			COrganizacion Organizacion = new COrganizacion();
			Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);
			Modelo.Add(new JProperty("Cliente", Organizacion.RazonSocial));

			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(Oportunidad.IdUsuarioCreacion, ConexionBaseDatos);
			string Nombre = Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno;
			Modelo.Add(new JProperty("Usuario", Nombre));

			JObject Permisos = new JObject();
			Permisos.Add(new JProperty("puedeEditarOportunidad", 1));
			Modelo.Add(new JProperty("Permisos", Permisos));
			string Clasificacion = (Oportunidad.Clasificacion == false) ? "GA" : "Externo";
			Modelo.Add(new JProperty("Clasificacion", Clasificacion));
			Modelo.Add(new JProperty("Division", CDivision.ObtenerNombreDivision(Oportunidad.IdDivision, ConexionBaseDatos)));

			if (Oportunidad.Cerrado == true) { Modelo.Add("Cerrado", "Cerrado"); } else { Modelo.Add("Cerrado", "Abierto"); }
			if (Oportunidad.EsProyecto == true) { Modelo.Add("EsProyecto", "SI"); } else { Modelo.Add("EsProyecto", "NO"); }
			if (Oportunidad.Urgente == true) { Modelo.Add("Urgente", "SI"); } else { Modelo.Add("Urgente", "NO"); }

			CUsuario UsuarioOportunidad = new CUsuario();
			UsuarioOportunidad.LlenaObjeto(Oportunidad.IdUsuarioCreacion, ConexionBaseDatos);

			Modelo.Add("Correo", UsuarioOportunidad.Correo);
			Modelo.Add("DiasAntiguedad", (DateTime.Today - Oportunidad.FechaCreacion).Days);
			Modelo.Add(new JProperty("Baja", Convert.ToInt16(Oportunidad.Baja)));
			Modelo.Add(new JProperty("MotivoCancelacion", Oportunidad.MotivoCancelacion));

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

	//###################### Metodo obtener forma reporte comisiones ############################
	[WebMethod]
	public static string ObtenerFormaReporteComisiones()
	{

		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		JObject oRespuesta = new JObject();

		if (respuesta == "Conexion Establecida")
		{
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeConsultarReporteComisiones" }, ConexionBaseDatos) == "")
			{
				JObject Modelo = new JObject();
				Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(ConexionBaseDatos));
				Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos));
				Modelo.Add("FechaInicial", "01/" + DateTime.Now.ToString("MM/yyyy"));
				Modelo.Add("FechaFinal", DateTime.Now.ToString("dd/MM/yyyy"));

				oRespuesta.Add(new JProperty("Modelo", Modelo));
				oRespuesta.Add(new JProperty("Error", 0));
			}
			else
			{
				oRespuesta.Add(new JProperty("Error", 1));
				oRespuesta.Add(new JProperty("Descripcion", "No tienes privilegios para realizar esta acción"));
			}
		}
		else
		{
			oRespuesta.Add(new JProperty("Error", 1));
			oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
		}
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return oRespuesta.ToString();
	}

	//###################### Metodo obtener forma reporte comisiones ############################
	[WebMethod]
	public static string ObtenerFormaGridReporteComisiones(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSucursal, string pFechaInicio, string pFechaFinal, int pDivision, int pNotaCredito, int pAgente)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		JObject oRespuesta = new JObject();

		if (respuesta == "Conexion Establecida")
		{
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeConsultarReporteComisiones" }, ConexionBaseDatos) == "")
			{
				JObject Modelo = new JObject();

				Modelo.Add("TamanoPaginacion", pTamanoPaginacion);
				Modelo.Add("PaginaActual", pPaginaActual);
				Modelo.Add("ColumnaOrden", pColumnaOrden);
				Modelo.Add("TipoOrden", pTipoOrden);
				Modelo.Add("IdSucursal", pIdSucursal);
				Modelo.Add("FechaInicio", pFechaInicio);
				Modelo.Add("FechaFinal", pFechaFinal);
				Modelo.Add("IdDivision", pDivision);
				Modelo.Add("NotaCredito", pNotaCredito);
				Modelo.Add("IdAgente", pAgente);

				oRespuesta.Add(new JProperty("Modelo", Modelo));
				oRespuesta.Add(new JProperty("Error", 0));
			}
			else
			{
				oRespuesta.Add(new JProperty("Error", 1));
				oRespuesta.Add(new JProperty("Descripcion", "No tienes privilegios para realizar esta acción"));
			}
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
	public static string ObtenerFormaMotivoCancelacion()
	{
		JObject oRespuesta = new JObject();
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();

			oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add("Error", 0);
		}
		else
		{
			oRespuesta.Add("Error", 1);
			oRespuesta.Add("Descripcion", respuesta);
		}
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return oRespuesta.ToString();
	}

	//###################### Metodo obtener forma reporte comisiones ############################
	[WebMethod]
	public static CJQGridJsonResponse ObtenerReporteComisiones(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSucursal, string pFechaInicio, string pFechaFinal, int pDivision, int pNotaCredito, int pAgente)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();

		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

		if (Usuario.TienePermisos(new string[] { "puedeConsultarReporteComisiones" }, ConexionBaseDatos) == "")
		{
			SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
			SqlCommand Stored = new SqlCommand("spg_grdReporteComisiones", sqlCon);

			Stored.CommandType = CommandType.StoredProcedure;
			Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
			Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
			Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
			Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
			Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
			Stored.Parameters.Add("pFechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
			Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 10).Value = pFechaFinal;
			Stored.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
			Stored.Parameters.Add("pNotaCredito", SqlDbType.Int).Value = pNotaCredito;
			Stored.Parameters.Add("pAgente", SqlDbType.Int).Value = pAgente;

			DataSet dataSet = new DataSet();
			SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
			dataAdapter.Fill(dataSet);
			ConexionBaseDatos.CerrarBaseDatosSqlServer();
			return new CJQGridJsonResponse(dataSet);
		}
		else
		{
			return new CJQGridJsonResponse(new DataSet());
		}
	}

	public static CJQGridJsonResponse ObtenerClientesOportunidades(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdOportunidad", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
		Stored.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = Convert.ToString(pOportunidad);
		Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
		Stored.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
		Stored.Parameters.Add("pIdNivelInteres", SqlDbType.Int).Value = pNivelInteres;
		Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pSucursal;
		Stored.Parameters.Add("pMonto", SqlDbType.Decimal).Value = (pMonto == "") ? 0 : Convert.ToDecimal(pMonto);
		Stored.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
		Stored.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
		Stored.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
		Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

		int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

		if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
		{
			//Stored.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
		}
		else
		{
			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidadesDeSucursalesAsignadas" }, ConexionBaseDatos) == "")
			{
				//Stored.Parameters.Add("pPuedeVerSucursalesAsignadas", SqlDbType.Int).Value = 1;
			}
			else
			{
				//Stored.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
			}
		}

		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

	[WebMethod]
	public static string ObtenerFormaAgregarActividad()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerFormaAgregarMotivoCancelacionOportunidad(int pIdOportunidad)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();

		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();

			Modelo.Add(new JProperty("IdOportunidad", pIdOportunidad));

			oRespuesta.Add(new JProperty("Error", 1));
			oRespuesta.Add(new JProperty("Modelo", Modelo));

		}
		else
		{
			oRespuesta.Add(new JProperty("Error", 1));
			oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a base de datos"));
		}

		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return oRespuesta.ToString();
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
	public static string ObtenerFormaArchivoOportunidad(int pIdOportunidad)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();

		COportunidad Oportunidad = new COportunidad();
		Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);
		int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuarios"]);

		JObject Modelo = new JObject();
		Modelo.Add(new JProperty("IdOportunidad", Oportunidad.IdOportunidad));
		Modelo.Add(new JProperty("IdUsuario", IdUsuario));
		int ExisteArchivo = 0;
		if (Oportunidad.Archivo != "")
		{
			ExisteArchivo = 1;
		}
		Modelo.Add(new JProperty("ExisteArchivo", ExisteArchivo));
		Modelo.Add(new JProperty("Archivo", Oportunidad.Archivo));

		oRespuesta.Add(new JProperty("Modelo", Modelo));

		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return oRespuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerFormaComentariosOportunidad(int pIdOportunidad)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		JObject oRespuesta = new JObject();

		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			COportunidad Oportunidad = new COportunidad();
			Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);

			JObject Permisos = new JObject();
			Permisos.Add(new JProperty("puedeAgregarComentario", 1));

			Modelo.Add(new JProperty("IdOportunidad", Oportunidad.IdOportunidad));
			Modelo.Add("Comentarios", CBitacoraNotasOportunidad.ObtenerComentariosOportunidadDesc(Oportunidad.IdOportunidad, ConexionBaseDatos));

			Modelo.Add(new JProperty("Permisos", Permisos));

			oRespuesta.Add(new JProperty("Modelo", Modelo));
			oRespuesta.Add(new JProperty("Error", 0));

		}
		else
		{
			oRespuesta.Add(new JProperty("Error", 1));
			oRespuesta.Add(new JProperty("Descripcion", "No se establecio la conexion a la base de datos"));
		}

		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return oRespuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerFormaEditarOportunidad(int pIdOportunidad)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				COportunidad Oportunidad = new COportunidad();
				Oportunidad.LlenaObjeto(pIdOportunidad, pConexion);
				Modelo.Add(new JProperty("IdOportunidad", pIdOportunidad));
				Modelo.Add(new JProperty("Oportunidad", Oportunidad.Oportunidad));

				CCliente Cliente = new CCliente();
				Cliente.LlenaObjeto(Oportunidad.IdCliente, pConexion);
				Modelo.Add(new JProperty("IdCliente", Oportunidad.IdCliente));

				COrganizacion Organizacion = new COrganizacion();
				Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
				Modelo.Add(new JProperty("Cliente", Organizacion.RazonSocial));

                CCondicionPago condicionPago = new CCondicionPago();
                condicionPago.LlenaObjeto(Cliente.IdCondicionPago, pConexion);
                Modelo.Add(new JProperty("CondicionPago", condicionPago.CondicionPago));
                
                CSelectEspecifico Saldo = new CSelectEspecifico();
                Saldo.StoredProcedure.CommandText = "sp_Cliente_ConsultarFSaldo";
                Saldo.StoredProcedure.Parameters.Add("pIdCliente", SqlDbType.Int).Value = Cliente.IdCliente;

                Modelo.Add(new JProperty("Saldo", CUtilerias.ObtenerConsulta(Saldo, pConexion)));

                CUsuario Usuario = new CUsuario();
				Usuario.LlenaObjeto(Oportunidad.IdUsuarioCreacion, pConexion);
				string Nombre = Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno;
				Modelo.Add(new JProperty("IdUsuario", Oportunidad.IdUsuarioCreacion));
				Modelo.Add(new JProperty("UsuarioAsignado", Nombre));
				Modelo.Add(new JProperty("FechaCierre", Oportunidad.FechaCierre.ToShortDateString()));
				Modelo.Add(new JProperty("EsProyecto", (Oportunidad.EsProyecto) ? 1 : 0));
				Modelo.Add(new JProperty("Urgente", (Oportunidad.Urgente) ? 1 : 0));
				Modelo.Add(new JProperty("Proveedores", Oportunidad.Proveedores));
				Modelo.Add(new JProperty("Utilidad", Oportunidad.Utilidad));
				Modelo.Add(new JProperty("Costo", Oportunidad.Costo));

				CCotizacion Cotizacion = new CCotizacion();
				CProyecto Proyecto = new CProyecto();
				Dictionary<string, object> Parametros = new Dictionary<string, object>();
				Parametros.Add("IdOportunidad", pIdOportunidad);
				Parametros.Add("Baja", 0);

				Cotizacion.LlenaObjetoFiltros(Parametros, pConexion);
				Proyecto.LlenaObjetoFiltros(Parametros, pConexion);

				CSelectEspecifico Contactos = new CSelectEspecifico();
				Contactos.StoredProcedure.CommandText = "sp_ContactoOrganizacion_Datos";
				Contactos.StoredProcedure.Parameters.Add("IdOrganizacion", SqlDbType.Int).Value = Cliente.IdOrganizacion;

				Modelo.Add("Contactos", CUtilerias.ObtenerConsulta(Contactos, pConexion));

				JObject Permisos = new JObject();

				int PermisoEditarCliente = 0;

				if (Cotizacion.IdCotizacion != 0 || Proyecto.IdProyecto != 0 || Usuario.TienePermisos(new string[] { "puedeEditarCliente" }, pConexion) == "")
				{
					PermisoEditarCliente = 1;
				}

				Permisos.Add("puedeEditarCliente", PermisoEditarCliente);

				int PermisoVerTodasLasOportunidades = 0;

				if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, pConexion) == "")
				{
					PermisoVerTodasLasOportunidades = 1;
				}

				Permisos.Add(new JProperty("puedeAsignarUsuario", PermisoVerTodasLasOportunidades));

				Modelo.Add(new JProperty("Permisos", Permisos));
				Modelo.Add(new JProperty("Monto", Oportunidad.Monto));
				Modelo.Add(new JProperty("NivelInteresOportunidad", CNivelInteresOportunidad.ObtenerJsonNivelInteresOportunidad(Oportunidad.IdNivelInteresOportunidad, pConexion)));
				Modelo.Add(new JProperty("Clasificacion", COportunidad.ObtenerJsonOportunidadClasificacion(Convert.ToInt32(Oportunidad.Clasificacion), pConexion)));
				Modelo.Add(new JProperty("Division", CDivision.ObtenerJsonDivisionesActivas(Oportunidad.IdDivision, pConexion)));
				Modelo.Add(new JProperty("Campana", CCampana.ObtenerJsonCampana(Oportunidad.IdCampana, pConexion)));
				Modelo.Add(new JProperty("Cerrada", Oportunidad.Cerrado));

				Modelo.Add("Comentarios", CBitacoraNotasOportunidad.ObtenerComentariosOportunidadDesc(pIdOportunidad, pConexion));

                CSelectEspecifico Factura = new CSelectEspecifico();
                Factura.StoredProcedure.CommandText = "sp_Oportunidad_FacturaEncabezado";
                Factura.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = pIdOportunidad;

                Modelo.Add("Facturas", CUtilerias.ObtenerConsulta(Factura, pConexion));

                CSelectEspecifico FacturaTotal = new CSelectEspecifico();
                FacturaTotal.StoredProcedure.CommandText = "sp_Oportunidad_FacturaEncabezado_Total";
                FacturaTotal.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = pIdOportunidad;

                Modelo.Add("FacturaTotal", CUtilerias.ObtenerConsulta(FacturaTotal, pConexion));

				CSelectEspecifico OrdenCompra = new CSelectEspecifico();
				OrdenCompra.StoredProcedure.CommandText = "sp_OrdenCompra_Oportunidad";
				OrdenCompra.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = pIdOportunidad;

				Modelo.Add("OrdenCompra", CUtilerias.ObtenerConsulta(OrdenCompra, pConexion));

                CSelectEspecifico Compras = new CSelectEspecifico();
                Compras.StoredProcedure.CommandText = "sp_Oportunidad_Compras";
                Compras.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = pIdOportunidad;

                Modelo.Add("Compras", CUtilerias.ObtenerConsulta(Compras, pConexion));

                CSelectEspecifico ComprasTotal = new CSelectEspecifico();
                ComprasTotal.StoredProcedure.CommandText = "sp_Oportunidad_Compras_Total";
                ComprasTotal.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = pIdOportunidad;

                Modelo.Add("ComprasTotal", CUtilerias.ObtenerConsulta(ComprasTotal, pConexion));

                CSelectEspecifico Proyectos = new CSelectEspecifico();
				Proyectos.StoredProcedure.CommandText = "sp_Oportunidad_Proyectos";
				Proyectos.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = pIdOportunidad;

				Modelo.Add("Proyectos", CUtilerias.ObtenerConsulta(Proyectos, pConexion));

                CSelectEspecifico ProyectosTotal = new CSelectEspecifico();
                ProyectosTotal.StoredProcedure.CommandText = "sp_Oportunidad_Proyectos_Total";
                ProyectosTotal.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = pIdOportunidad;

                Modelo.Add("ProyectosTotal", CUtilerias.ObtenerConsulta(ProyectosTotal, pConexion));

                //Solicitud de Levantamiento
                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                Parametros.Clear();
                Parametros.Add("Baja", 0);
                Parametros.Add("IdOportunidad", pIdOportunidad);
                solicitudLevantamiento.LlenaObjetoFiltros(Parametros, pConexion);

                Modelo.Add(new JProperty("Agente", Nombre));
                CUsuario asignado = new CUsuario();
                CDivision division = new CDivision();

                string ExisteSolicitud = "";
                if(solicitudLevantamiento.IdSolicitudLevantamiento != 0)
                {
                    ExisteSolicitud = "1";
                    Modelo.Add(new JProperty("IdSolLevantamiento", solicitudLevantamiento.IdSolicitudLevantamiento));
                    Modelo.Add(new JProperty("FolioSolicitud", solicitudLevantamiento.IdSolicitudLevantamiento));

                    Modelo.Add(new JProperty("FechaAlta", solicitudLevantamiento.FechaAlta.ToShortDateString()));
                    
                    Modelo.Add(new JProperty("CitaFechaHora", solicitudLevantamiento.CitaFechaHora.ToShortDateString() + " " + solicitudLevantamiento.CitaFechaHora.ToShortTimeString().Replace(".", "").Replace("a m", "am").Replace("p m", "pm")));

                    division.LlenaObjeto(solicitudLevantamiento.IdDivision,pConexion);
                    Modelo.Add(new JProperty("Especialidad",division.Division));
                    asignado.LlenaObjeto(solicitudLevantamiento.IdUsuarioAsignado, pConexion);
                    Modelo.Add(new JProperty("Asignado", asignado.Nombre + " " + asignado.ApellidoPaterno + " " + asignado.ApellidoMaterno));
                    Modelo.Add(new JProperty("idUsuarioAsignado", solicitudLevantamiento.IdUsuarioAsignado));

                    Modelo.Add(new JProperty("ContactoDirecto", solicitudLevantamiento.ContactoDirecto));
                    Modelo.Add(new JProperty("IdContactoDirectoPuesto", solicitudLevantamiento.IdPuestoContactoDirecto));
                    Modelo.Add(new JProperty("ContactoDirectoPuesto", ObtenerPuestoContacto(pConexion)));

                    Modelo.Add(new JProperty("Externo", solicitudLevantamiento.Externo));

                    Modelo.Add(new JProperty("ContactoEnSitio", solicitudLevantamiento.ContactoEnSitio));
                    Modelo.Add(new JProperty("IdContactoSitioPuesto", solicitudLevantamiento.IdPuestoContactoEnSitio));
                    Modelo.Add(new JProperty("ContactoSitioPuesto", ObtenerPuestoContacto(pConexion)));

                    Modelo.Add(new JProperty("Telefonos", solicitudLevantamiento.Telefonos));
                    //Modelo.Add(new JProperty("HoraCliente", solicitudLevantamiento.HoraAtencionCliente));

                    Modelo.Add(new JProperty("PermisoIngresarSitio", solicitudLevantamiento.PermisoIngresarSitio));
                    Modelo.Add(new JProperty("EquipoSeguridadIngresarSitio", solicitudLevantamiento.EquipoSeguridadIngresarSitio));
                    Modelo.Add(new JProperty("ClienteCuentaEstacionamiento", solicitudLevantamiento.ClienteCuentaEstacionamiento));
                    Modelo.Add(new JProperty("ClienteCuentaPlanoLevantamiento", solicitudLevantamiento.ClienteCuentaPlanoLevantamiento));

                    Modelo.Add(new JProperty("Domicilio", solicitudLevantamiento.Domicilio));
                    Modelo.Add(new JProperty("Descripcion", solicitudLevantamiento.Descripcion));
                    Modelo.Add(new JProperty("Notas", solicitudLevantamiento.Notas));

                    Modelo.Add(new JProperty("ConfirmarSolicitud", solicitudLevantamiento.ConfirmarSolicitud));
                    Modelo.Add(new JProperty("LevantamientoCreado", solicitudLevantamiento.LevantamientoCreado));

                }
                else
                {
                    Modelo.Add(new JProperty("FolioSolicitud", solicitudLevantamiento.IdSolicitudLevantamiento));
                    Modelo.Add(new JProperty("FechaAlta", DateTime.Now.ToShortDateString()));
                    Modelo.Add(new JProperty("CitaFechaHora",""));

                    division.LlenaObjeto(Oportunidad.IdDivision, pConexion);
                    Modelo.Add(new JProperty("Especialidad", division.Division));
                    Modelo.Add(new JProperty("Asignado",""));
                    Modelo.Add(new JProperty("IdUsuarioAsignadoSolLevantamiento",""));

                    Modelo.Add(new JProperty("ContactoDirecto", ""));
                    Modelo.Add(new JProperty("IdContactoDirectoPuesto",""));
                    Modelo.Add(new JProperty("ContactoDirectoPuesto", ObtenerPuestoContacto(pConexion)));

                    Modelo.Add(new JProperty("Externo", "0"));

                    Modelo.Add(new JProperty("ContactoEnSitio", ""));
                    Modelo.Add(new JProperty("IdContactoSitioPuesto", ""));
                    Modelo.Add(new JProperty("ContactoSitioPuesto", ObtenerPuestoContacto(pConexion)));

                    Modelo.Add(new JProperty("Telefonos", ""));
                    //Modelo.Add(new JProperty("HoraCliente", ""));

                    Modelo.Add(new JProperty("PermisoIngresarSitio", "0"));
                    Modelo.Add(new JProperty("EquipoSeguridadIngresarSitio", "0"));
                    Modelo.Add(new JProperty("ClienteCuentaEstacionamiento", "0"));
                    Modelo.Add(new JProperty("ClienteCuentaPlanoLevantamiento", "0"));

                    Modelo.Add(new JProperty("Domicilio",""));
                    Modelo.Add(new JProperty("Descripcion", ""));
                    Modelo.Add(new JProperty("Notas",""));

                    Modelo.Add(new JProperty("ConfirmarSolicitud", "0"));
                    Modelo.Add(new JProperty("LevantamientoCreado", "0"));

                    ExisteSolicitud = "0";
                }
                Modelo.Add(new JProperty("SolicitudAsigando",solicitudLevantamiento.IdSolicitudLevantamiento));
                Modelo.Add(new JProperty("ExisteSolicitud", ExisteSolicitud));


                Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

    public static JArray ObtenerPuestoContacto(CConexion Conexion)
    {
        JArray JPuestosContactos = new JArray();
        CPuestoContacto puestoContacto = new CPuestoContacto();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CPuestoContacto oPuestoContacto in puestoContacto.LlenaObjetosFiltros(Parametros, Conexion))
        {
            JObject JPuestoContacto = new JObject();
            JPuestoContacto.Add("Valor", oPuestoContacto.IdPuestoContacto);
            JPuestoContacto.Add("Descripcion", oPuestoContacto.Descripcion);

            JPuestosContactos.Add(JPuestoContacto);
        }
        return JPuestosContactos;
    }

    [WebMethod]
	public static string ObtenerFormaGraficasOportunidades(string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();

			Modelo.Add("Cliente", pCliente);
			Modelo.Add("IdNivelInteres", pNivelInteres);
			Modelo.Add("IdSucursal", pSucursal);
			Modelo.Add("IdClasificacion", pClasificacion);
			Modelo.Add("IdDivision", pDivision);
			Modelo.Add("IdCerrada", pCerrado);
			Modelo.Add("Baja", pAI);

			int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

			Modelo.Add("Agentes", CUsuario.ObtenerJsonAgentes(IdEmpresa, ConexionBaseDatos));
			Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos));
			Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(ConexionBaseDatos));
			Modelo.Add("NivelesInteres", CNivelInteresOportunidad.ObtenerJsonNivelInteresOportunidad(ConexionBaseDatos));

			oRespuesta.Add("Error", 0);
			oRespuesta.Add("Modelo", Modelo);
		}
		else
		{
			oRespuesta.Add("Error", 1);
			oRespuesta.Add("Description", respuesta);
		}
		return oRespuesta.ToString();

	}

	[WebMethod]
	public static string ObtenerTotalesOportunidad(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();
				CSelectEspecifico Select = new CSelectEspecifico();
				Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
				Select.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_ObtenerTotalesOportunidad";
				Select.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
				Select.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pOportunidad);
				Select.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
				Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
				Select.StoredProcedure.Parameters.Add("pNivelInteres", SqlDbType.Int).Value = pNivelInteres;
				Select.StoredProcedure.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
				if (pMonto == "")
				{
					pMonto = "0";
				}
				Select.StoredProcedure.Parameters.Add("pMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(pMonto);
				Select.StoredProcedure.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
				Select.StoredProcedure.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
				Select.StoredProcedure.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
				Select.StoredProcedure.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
				Select.StoredProcedure.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
				Select.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
				Select.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
				Select.StoredProcedure.Parameters.Add("pOpcion", SqlDbType.Int).Value = 2;

				int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
				CUsuario Usuario = new CUsuario();
				Usuario.LlenaObjeto(IdUsuario, pConexion);

				if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, pConexion) == "")
				{
					Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
				}
				else
				{
					Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
				}

				Select.Llena(pConexion);
				if (Select.Registros.Read())
				{
					Modelo.Add("TotalClientes", Convert.ToDecimal(Select.Registros["TotalClientes"]));
					Modelo.Add("Actividades", Convert.ToInt32(Select.Registros["Actividades"]));
					Modelo.Add("MontoTotal", Convert.ToDecimal(Select.Registros["MontoTotal"]));
					Modelo.Add("MontoReal", Convert.ToDecimal(Select.Registros["MontoReal"]));
					Modelo.Add("Facturado", Convert.ToDecimal(Select.Registros["Facturado"]));
					Modelo.Add("TotalOportunidades", Convert.ToDecimal(Select.Registros["TotalOportunidades"]));
				}
				Respuesta.Add("Modelo", Modelo);

				Select.CerrarConsulta();

			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerTotalCotizaciones(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			CSelectEspecifico Select = new CSelectEspecifico();
			Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
			Select.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_ObtenerTotalCotizaciones";
			Select.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
			Select.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pOportunidad);
			Select.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
			Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
			Select.StoredProcedure.Parameters.Add("pNivelInteres", SqlDbType.Int).Value = pNivelInteres;
			Select.StoredProcedure.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
			if (pMonto == "") { pMonto = "0"; }
			Select.StoredProcedure.Parameters.Add("pMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(pMonto);
			Select.StoredProcedure.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
			Select.StoredProcedure.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
			Select.StoredProcedure.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
			Select.StoredProcedure.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
			Select.StoredProcedure.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
			Select.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
			Select.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
			Select.StoredProcedure.Parameters.Add("pOpcion", SqlDbType.Int).Value = 2;

			int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
			}
			else
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
			}

			Select.Llena(ConexionBaseDatos);
			if (Select.Registros.Read())
			{
				Modelo.Add("Cotizaciones", Convert.ToDecimal(Select.Registros["Cotizaciones"]));
			}
			oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add(new JProperty("Error", 0));

			Select.CerrarConsulta();
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
	public static string ObtenerDatosGrafica(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			CSelectEspecifico Select = new CSelectEspecifico();
			Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
			Select.StoredProcedure.CommandText = "sp_Oportunidad_DatosGrafica";
			Select.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
			Select.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pOportunidad);
			Select.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
			Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
			Select.StoredProcedure.Parameters.Add("pNivelInteres", SqlDbType.Int).Value = pNivelInteres;
			Select.StoredProcedure.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
			if (pMonto == "") { pMonto = "0"; }
			Select.StoredProcedure.Parameters.Add("pMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(pMonto);
			Select.StoredProcedure.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
			Select.StoredProcedure.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
			Select.StoredProcedure.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
			Select.StoredProcedure.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
			Select.StoredProcedure.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
			Select.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
			Select.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
			Select.StoredProcedure.Parameters.Add("pOpcion", SqlDbType.Int).Value = 2;

			int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
			}
			else
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
			}

			Select.Llena(ConexionBaseDatos);
			if (Select.Registros.Read())
			{
				Modelo.Add("Oportunidades", Convert.ToDecimal(Select.Registros["Oportunidades"]));
				Modelo.Add("Cotizaciones", Convert.ToDecimal(Select.Registros["Cotizaciones"]));
				Modelo.Add("Conversiones", Convert.ToDecimal(Select.Registros["Conversiones"]));
				Modelo.Add("Facturas", Convert.ToDecimal(Select.Registros["Facturas"]));
			}
			oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add(new JProperty("Error", 0));

			Select.CerrarConsulta();
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
	public static string ObtenerGraficaNivelInteres(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			CSelectEspecifico Select = new CSelectEspecifico();
			Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
			Select.StoredProcedure.CommandText = "sp_Oportunidad_GraficaNivelInteres";
			Select.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
			Select.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pOportunidad);
			Select.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
			Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
			Select.StoredProcedure.Parameters.Add("pNivelInteres", SqlDbType.Int).Value = pNivelInteres;
			Select.StoredProcedure.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
			if (pMonto == "") { pMonto = "0"; }
			Select.StoredProcedure.Parameters.Add("pMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(pMonto);
			Select.StoredProcedure.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
			Select.StoredProcedure.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
			Select.StoredProcedure.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
			Select.StoredProcedure.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
			Select.StoredProcedure.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
			Select.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
			Select.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
			Select.StoredProcedure.Parameters.Add("pOpcion", SqlDbType.Int).Value = 2;

			int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
			}
			else
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
			}

			Select.Llena(ConexionBaseDatos);
			if (Select.Registros.Read())
			{
				Modelo.Add("Alto", Convert.ToDecimal(Select.Registros["Alto"]));
				Modelo.Add("Alto2", Convert.ToDecimal(Select.Registros["Alto2"]));
				Modelo.Add("Medio", Convert.ToDecimal(Select.Registros["Medio"]));
				Modelo.Add("Medio2", Convert.ToDecimal(Select.Registros["Medio2"]));
				Modelo.Add("Bajo", Convert.ToDecimal(Select.Registros["Bajo"]));
				Modelo.Add("Bajo2", Convert.ToDecimal(Select.Registros["Bajo2"]));
			}
			oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add(new JProperty("Error", 0));

			Select.CerrarConsulta();
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
	public static string ObtenerDatosNivelInteres(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			CSelectEspecifico Select = new CSelectEspecifico();
			Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
			Select.StoredProcedure.CommandText = "sp_Oportunidad_DatosNivelInteres";
			Select.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
			Select.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pOportunidad);
			Select.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
			Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
			Select.StoredProcedure.Parameters.Add("pNivelInteres", SqlDbType.Int).Value = pNivelInteres;
			Select.StoredProcedure.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
			if (pMonto == "") { pMonto = "0"; }
			Select.StoredProcedure.Parameters.Add("pMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(pMonto);
			Select.StoredProcedure.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
			Select.StoredProcedure.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
			Select.StoredProcedure.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
			Select.StoredProcedure.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
			Select.StoredProcedure.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
			Select.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
			Select.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
			Select.StoredProcedure.Parameters.Add("pOpcion", SqlDbType.Int).Value = 2;

			int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
			}
			else
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
			}

			Select.Llena(ConexionBaseDatos);
			if (Select.Registros.Read())
			{
				Modelo.Add("Alto", Convert.ToDecimal(Select.Registros["Alto"]));
				Modelo.Add("Medio", Convert.ToDecimal(Select.Registros["Medio"]));
				Modelo.Add("Bajo", Convert.ToDecimal(Select.Registros["Bajo"]));
			}
			oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add(new JProperty("Error", 0));

			Select.CerrarConsulta();
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
	public static string ObtenerTotalPedidos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			CSelectEspecifico Select = new CSelectEspecifico();
			Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
			Select.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_ObtenerTotalPedidos";
			Select.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
			Select.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pOportunidad);
			Select.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
			Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
			Select.StoredProcedure.Parameters.Add("pNivelInteres", SqlDbType.Int).Value = pNivelInteres;
			Select.StoredProcedure.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
			if (pMonto == "") { pMonto = "0"; }
			Select.StoredProcedure.Parameters.Add("pMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(pMonto);
			Select.StoredProcedure.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
			Select.StoredProcedure.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
			Select.StoredProcedure.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
			Select.StoredProcedure.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
			Select.StoredProcedure.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
			Select.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
			Select.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
			Select.StoredProcedure.Parameters.Add("pOpcion", SqlDbType.Int).Value = 2;

			int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
			}
			else
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
			}

			Select.Llena(ConexionBaseDatos);
			if (Select.Registros.Read())
			{
				Modelo.Add("Pedidos", Convert.ToDecimal(Select.Registros["Pedidos"]));
			}
			oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add(new JProperty("Error", 0));

			Select.CerrarConsulta();
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
	public static string ObtenerTotalProyectos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdOportunidad, string pOportunidad, string pAgente, string pCliente, int pNivelInteres, int pSucursal, string pMonto, int pClasificacion, int pDivision, int pCerrado, int pEsProyecto, int pUrgente, int pAI)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			CSelectEspecifico Select = new CSelectEspecifico();
			Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
			Select.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_ObtenerTotalProyectos";
			Select.StoredProcedure.Parameters.Add("pIdOportunidad", SqlDbType.Int).Value = pIdOportunidad;
			Select.StoredProcedure.Parameters.Add("pOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pOportunidad);
			Select.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 250).Value = Convert.ToString(pAgente);
			Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
			Select.StoredProcedure.Parameters.Add("pNivelInteres", SqlDbType.Int).Value = pNivelInteres;
			Select.StoredProcedure.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
			if (pMonto == "") { pMonto = "0"; }
			Select.StoredProcedure.Parameters.Add("pMonto", SqlDbType.Decimal).Value = Convert.ToDecimal(pMonto);
			Select.StoredProcedure.Parameters.Add("pClasificacion", SqlDbType.Int).Value = pClasificacion;
			Select.StoredProcedure.Parameters.Add("pDivision", SqlDbType.Int).Value = pDivision;
			Select.StoredProcedure.Parameters.Add("pCerrado", SqlDbType.Int).Value = pCerrado;
			Select.StoredProcedure.Parameters.Add("pEsProyecto", SqlDbType.Int).Value = pEsProyecto;
			Select.StoredProcedure.Parameters.Add("pUrgente", SqlDbType.Int).Value = pUrgente;
			Select.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
			Select.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
			Select.StoredProcedure.Parameters.Add("pOpcion", SqlDbType.Int).Value = 2;

			int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, ConexionBaseDatos);

			if (Usuario.TienePermisos(new string[] { "puedeVerTodasLasOportunidades" }, ConexionBaseDatos) == "")
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = 0;
			}
			else
			{
				Select.StoredProcedure.Parameters.Add("pIdUsuarioCreacion", SqlDbType.Int).Value = IdUsuario;
			}

			Select.Llena(ConexionBaseDatos);
			if (Select.Registros.Read())
			{
				Modelo.Add("Proyectos", Convert.ToDecimal(Select.Registros["Proyectos"]));
			}
			oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add(new JProperty("Error", 0));

			Select.CerrarConsulta();
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
	private static string ValidarOportunidad(COportunidad pOportunidad, CConexion Conexion)
	{
		string errores = "";
		if (pOportunidad.Oportunidad == "")
		{
			errores = errores + "<span>*</span> El campo Oportunidad esta vacio, favor de completarlo.<br/>";
		}
		if (Convert.ToInt32(pOportunidad.IdNivelInteresOportunidad) <= 0)
		{
			errores = errores + "<span>*</span> Favor de selecionar el nivel de interés de la oportunidad.<br/>";
		}
		errores += (pOportunidad.IdDivision == 0) ? "<span>*</span> Favor de seleccionar la Division de la oportunidad.<br/>" : errores;
		if (Convert.ToInt32(pOportunidad.IdCliente) <= 0)
		{
			errores = errores + "<span>*</span> Favor de selecionar el cliente de la oportunidad.<br/>";
		}
		if (errores != "")
		{
			errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores;
		}
		return errores;
	}
	
	[WebMethod]
	public static string ObtenerGraficaResultadoVentaDivision(string pFechaInicio, string pFechaFinal)
	{
		JObject oRespuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion)
		{
			JObject Modelo = new JObject();
			Modelo.Add("Divisiones", GraficaDivisiones(pFechaInicio, pFechaFinal, pConexion));

			Modelo.Add("Sucursales", GraficaSucursal(pFechaInicio, pFechaFinal, pConexion));

			Modelo.Add("Agentes", GraficaAgentes(pFechaInicio, pFechaFinal, pConexion));

			Modelo.Add("Actividades", ReporteUsuarios(pFechaInicio, pFechaFinal, UsuarioSesion, pConexion));

			Modelo.Add("AvanceAgentes", ReporteAvanceMeta(pFechaInicio, pFechaFinal, pConexion));

			int PuedeExportar = (UsuarioSesion.TienePermisos(new string[] { "puedeExportarReporteVentasPorCliente" }, pConexion) == "") ? 1: 0;
			Modelo.Add("puedeExportarReporteVentasPorCliente", PuedeExportar);
            Modelo.Add("Sucursal", CSucursal.ObtenerSucursalesEmpresa(pConexion));
			Modelo.Add("VentasCliente", ReporteVentasPorCliente(pFechaInicio, pFechaFinal, pConexion));

			DateTime FechaInicio = new DateTime(DateTime.Now.Year,DateTime.Now.Month,1);

            Modelo.Add("FechaInicial", FechaInicio.ToShortDateString());
			Modelo.Add("FechaFinal", DateTime.Now.ToShortDateString());

            CSelectEspecifico AtencionClientes = new CSelectEspecifico();
            AtencionClientes.StoredProcedure.CommandText = "sp_Reporte_AtencionClientes";
            AtencionClientes.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

            AtencionClientes.Llena(pConexion);

            JArray AgentesClientes = new JArray();
			decimal TotalClientes = 0;
			decimal TotalContactos = 0;
			decimal TotalPorcentajeContactos = 0;
			decimal TotalActividades = 0;
			decimal TotalPorcentajeActividades = 0;
			decimal TotalConOportunidades = 0;
			decimal TotalPorcentajeConOportunidades = 0;
			decimal TotalSinOportunidades = 0;
			decimal TotalPorcentajeSinOportunidades = 0;

            try
            {
                while (AtencionClientes.Registros.Read())
                {
                    JObject AgenteCliente = new JObject();

					int Clientes = Convert.ToInt32(AtencionClientes.Registros["Clientes"]);
					int Contactos = Convert.ToInt32(AtencionClientes.Registros["Contactos"]);
					decimal PorcentajeContactos = Convert.ToDecimal(AtencionClientes.Registros["Por Contactos"]);
					int Actividades = Convert.ToInt32(AtencionClientes.Registros["Actividades"]);
					decimal PorcentajeActividades = Convert.ToDecimal(AtencionClientes.Registros["Por Actividades"]);
					int ConOportunidades = Convert.ToInt32(AtencionClientes.Registros["Cts con Opts"]);
					decimal PorcentajeConOportunidades = Convert.ToDecimal(AtencionClientes.Registros["Por Cts con Opts"]);
					int SinOportunidades = Convert.ToInt32(AtencionClientes.Registros["Cts sin Opts"]);
					decimal PorcentajeSinOportunidades = Convert.ToDecimal(AtencionClientes.Registros["Por Cts sin Opts"]);

					AgenteCliente.Add("Agente", Convert.ToString(AtencionClientes.Registros["Agente"]));
                    AgenteCliente.Add("Clientes", Clientes);
					AgenteCliente.Add("ConOportunidades", ConOportunidades);
					AgenteCliente.Add("PorcentajeConOportunidades", PorcentajeConOportunidades + " %");
					AgenteCliente.Add("SinOportunidades", SinOportunidades);
					AgenteCliente.Add("PorcentajeSinOportunidades", PorcentajeSinOportunidades + " %");
					AgenteCliente.Add("Actividades", Actividades);
					AgenteCliente.Add("PorcentajeActividades", PorcentajeActividades + " %");
					AgenteCliente.Add("Contactos", Contactos);
					AgenteCliente.Add("PorcentajeContactos", PorcentajeContactos + " %");

					TotalClientes += Clientes;
					TotalActividades += Actividades;
					TotalContactos += Contactos;
					TotalConOportunidades += ConOportunidades;
					TotalSinOportunidades += SinOportunidades;

                    AgentesClientes.Add(AgenteCliente);
                }

				TotalPorcentajeContactos = TotalContactos / TotalClientes;
				TotalPorcentajeActividades = TotalActividades / TotalClientes;
				TotalPorcentajeConOportunidades = TotalConOportunidades / TotalClientes;
				TotalPorcentajeSinOportunidades = TotalSinOportunidades / TotalClientes;

            }
            catch (Exception ex)
            {
                Descripcion = ex.Message + " - " + ex.StackTrace;
                Error = 1;
            }

            AtencionClientes.CerrarConsulta();

            int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

            Modelo.Add("Year", DateTime.Today.Year);
            Modelo.Add("Hoy", DateTime.Today.ToShortDateString());
            Modelo.Add("AgentesClientes", AgentesClientes);
            Modelo.Add("TotalClientes", TotalClientes);
            Modelo.Add("TotalContactos", TotalContactos);
            Modelo.Add("PorcentajeContactos", TotalPorcentajeContactos);
            Modelo.Add("TotalActividades", TotalActividades);
            Modelo.Add("PorcentajeActividades", TotalPorcentajeActividades);
            Modelo.Add("TotalConOportunidades", TotalConOportunidades);
            Modelo.Add("PorcentajeConOportunidades", TotalPorcentajeConOportunidades);
            Modelo.Add("TotalSinOportunidades", TotalSinOportunidades);
            Modelo.Add("PorcentajeSinOportunidades", TotalPorcentajeSinOportunidades);
            Modelo.Add("AgentesCombo", CUsuario.ObtenerJsonAgentes(IdEmpresa, pConexion));

            Modelo.Add("Oportunidades", GraficaOportunidadesTotales(pConexion));
            Modelo.Add("OportunidadesAlta", GraficaOportunidadesAlta(pConexion));
            Modelo.Add("OportunidadesMedia", GraficaOportunidadesMedia(pConexion));
            Modelo.Add("OportunidadesBaja", GraficaOportunidadesBaja(pConexion));

            oRespuesta.Add("Modelo", Modelo);
			oRespuesta.Add("Descripcion", Descripcion);
			oRespuesta.Add("Error", Error);
		});
		return oRespuesta.ToString();
	}

    private static JArray GraficaDivisiones(string pFechaInicio, string pFechaFinal, CConexion pConexion) {


        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
        Select.StoredProcedure.CommandText = "sp_Oportunidad_GraficaDivisiones";
        Select.StoredProcedure.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Select.StoredProcedure.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Select.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        Select.Llena(pConexion);
        JArray Divisiones = new JArray();
        while (Select.Registros.Read())
        {
            JArray Division = new JArray();
            Division.Add(Convert.ToString(Select.Registros["Division"]));
            Division.Add(Convert.ToDecimal(Select.Registros["Monto"]));
            Divisiones.Add(Division);
        }

        Select.CerrarConsulta();

        JArray Grafica = new JArray();
        Grafica.Add(Divisiones);

        return Grafica;
    }

    private static JArray GraficaSucursal(string pFechaInicio, string pFechaFinal, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
        Select.StoredProcedure.CommandText = "sp_Oportunidad_GraficaSucursal";
        Select.StoredProcedure.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Select.StoredProcedure.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Select.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        Select.Llena(pConexion);
        JArray Sucursales = new JArray();
        while (Select.Registros.Read())
        {
            JArray Sucursal = new JArray();
            Sucursal.Add(Convert.ToString(Select.Registros["Sucursal"]));
            Sucursal.Add(Convert.ToDecimal(Select.Registros["Monto"]));
            Sucursales.Add(Sucursal);
        }

        Select.CerrarConsulta();

        JArray Grafica = new JArray();
        Grafica.Add(Sucursales);
        return Grafica;
    }

    private static JArray GraficaAgentes(string pFechaInicio, string pFechaFinal, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
        Select.StoredProcedure.CommandText = "sp_Oportunidad_GraficaAgentes";
        Select.StoredProcedure.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Select.StoredProcedure.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Select.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        Select.Llena(pConexion);
        JArray Agentes = new JArray();
        while (Select.Registros.Read())
        {
            JArray Agente = new JArray();
            Agente.Add(Convert.ToString(Select.Registros["Agente"]));
            Agente.Add(Convert.ToDecimal(Select.Registros["Monto"]));
            Agentes.Add(Agente);
        }

        Select.CerrarConsulta();

        JArray Grafica = new JArray();
        Grafica.Add(Agentes);

        return Grafica;
    }

    private static JArray ReporteUsuarios(string pFechaInicio, string pFechaFinal, CUsuario UsuarioSesion, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
        Select.StoredProcedure.CommandText = "sp_Actividad_ReporteUsuarios";
        Select.StoredProcedure.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Select.StoredProcedure.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Select.StoredProcedure.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = UsuarioSesion.IdUsuario;
        Select.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        Select.Llena(pConexion);
        JArray AgentesActividades = new JArray();

        try
        {
            while (Select.Registros.Read())
            {
                JObject AgenteActividades = new JObject();
                for (int i = 0; i < Select.Registros.FieldCount; i++)
                {
                    string val = Select.Registros[Select.Registros.GetName(i)].ToString();
                    AgenteActividades.Add(Select.Registros.GetName(i), val);
                }
                AgentesActividades.Add(AgenteActividades);
            }
        }
        catch (Exception ex) { }

        Select.CerrarConsulta();

        return AgentesActividades;
    }

    private static JArray ReporteAvanceMeta(string pFechaInicio, string pFechaFinal, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
        Select.StoredProcedure.CommandText = "sp_Agentes_Avance_Meta";
        Select.StoredProcedure.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Select.StoredProcedure.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Select.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        Select.Llena(pConexion);
        JArray AvanceAgentes = new JArray();

        try
        {
            while (Select.Registros.Read())
            {
                JObject AvanceAgente = new JObject();
                AvanceAgente.Add("Agente", Convert.ToString(Select.Registros["Agente"]));
                AvanceAgente.Add("Meta", Convert.ToString(Select.Registros["Meta"]));
                AvanceAgente.Add("Monto", Convert.ToString(Select.Registros["Monto"]));
                AvanceAgente.Add("Avance Meta", Convert.ToString(Select.Registros["AvanceMeta"]));
                AvanceAgente.Add("Avance Tiempo", Convert.ToString(Select.Registros["AvanceTiempo"]));
                AvanceAgente.Add("EnMeta", Convert.ToString(Select.Registros["EnMeta"]));
                AvanceAgentes.Add(AvanceAgente);
            }
        }
        catch (Exception ex) { }

        Select.CerrarConsulta();

        return AvanceAgentes;
    }

    private static JArray ReporteVentasPorCliente(string pFechaInicio, string pFechaFinal, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
        Select.StoredProcedure.CommandText = "sp_Oportunidad_VentasPorCliente";
        Select.StoredProcedure.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Select.StoredProcedure.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Select.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

        Select.Llena(pConexion);
        JArray VentasCliente = new JArray();

        try
        {
            while (Select.Registros.Read())
            {
                JObject VentaCliente = new JObject();
                VentaCliente.Add("#", Convert.ToString(Select.Registros["Num"]));
                VentaCliente.Add("Clasificación", Convert.ToString(Select.Registros["TipoCliente"]));
                VentaCliente.Add("Cliente", Convert.ToString(Select.Registros["Cliente"]));
                VentaCliente.Add("Monto", Convert.ToDecimal(Select.Registros["Monto"]).ToString("C"));
                VentasCliente.Add(VentaCliente);
            }
        }
        catch (Exception ex) { }

        Select.CerrarConsulta();

        return VentasCliente;
    }

    private static JArray GraficaOportunidadesTotales(CConexion pConexion)
    {
        CSelectEspecifico GraficaOportunidades = new CSelectEspecifico();
        GraficaOportunidades.StoredProcedure.CommandText = "sp_Oportunidad_VentasPorDivision";

        GraficaOportunidades.Llena(pConexion);

        JArray JOportunidades = new JArray();

        while (GraficaOportunidades.Registros.Read())
        {
            JArray JDivision = new JArray();
            decimal Oportunidades = Convert.ToDecimal(GraficaOportunidades.Registros["Oportunidades"]);
            string Division = Convert.ToString(GraficaOportunidades.Registros["Division"]);
            JDivision.Add(Division);
            JDivision.Add(Oportunidades);
            JOportunidades.Add(JDivision);
        }

        GraficaOportunidades.CerrarConsulta();
        JArray GraficaOpt = new JArray();
        GraficaOpt.Add(JOportunidades);

        return GraficaOpt;
    }
    
    private static JArray GraficaOportunidadesAlta(CConexion pConexion)
    {
        CSelectEspecifico GraficaOportunidades = new CSelectEspecifico();
        GraficaOportunidades.StoredProcedure.CommandText = "sp_Oportunidad_VentasPorDivision_Alta";

        GraficaOportunidades.Llena(pConexion);

        JArray JOportunidades = new JArray();

        while (GraficaOportunidades.Registros.Read())
        {
            JArray JDivision = new JArray();
            decimal Oportunidades = Convert.ToDecimal(GraficaOportunidades.Registros["Oportunidades"]);
            string Division = Convert.ToString(GraficaOportunidades.Registros["Division"]);
            JDivision.Add(Division);
            JDivision.Add(Oportunidades);
            JOportunidades.Add(JDivision);
        }

        GraficaOportunidades.CerrarConsulta();
        JArray GraficaOpt = new JArray();
        GraficaOpt.Add(JOportunidades);

        return GraficaOpt;
    }

    private static JArray GraficaOportunidadesMedia(CConexion pConexion)
    {
        CSelectEspecifico GraficaOportunidades = new CSelectEspecifico();
        GraficaOportunidades.StoredProcedure.CommandText = "sp_Oportunidad_VentasPorDivision_Media";

        GraficaOportunidades.Llena(pConexion);

        JArray JOportunidades = new JArray();

        while (GraficaOportunidades.Registros.Read())
        {
            JArray JDivision = new JArray();
            decimal Oportunidades = Convert.ToDecimal(GraficaOportunidades.Registros["Oportunidades"]);
            string Division = Convert.ToString(GraficaOportunidades.Registros["Division"]);
            JDivision.Add(Division);
            JDivision.Add(Oportunidades);
            JOportunidades.Add(JDivision);
        }

        GraficaOportunidades.CerrarConsulta();
        JArray GraficaOpt = new JArray();
        GraficaOpt.Add(JOportunidades);

        return GraficaOpt;
    }

    private static JArray GraficaOportunidadesBaja(CConexion pConexion)
    {
        CSelectEspecifico GraficaOportunidades = new CSelectEspecifico();
        GraficaOportunidades.StoredProcedure.CommandText = "sp_Oportunidad_VentasPorDivision_Baja";

        GraficaOportunidades.Llena(pConexion);

        JArray JOportunidades = new JArray();

        while (GraficaOportunidades.Registros.Read())
        {
            JArray JDivision = new JArray();
            decimal Oportunidades = Convert.ToDecimal(GraficaOportunidades.Registros["Oportunidades"]);
            string Division = Convert.ToString(GraficaOportunidades.Registros["Division"]);
            JDivision.Add(Division);
            JDivision.Add(Oportunidades);
            JOportunidades.Add(JDivision);
        }

        GraficaOportunidades.CerrarConsulta();
        JArray GraficaOpt = new JArray();
        GraficaOpt.Add(JOportunidades);

        return GraficaOpt;
    }
    
    [WebMethod]
    public static string ObtenerSabanaClientes(string FechaInicial, string FechaFinal, int IdUsuario, int IdCliente, int Incidentes)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();
                int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

                JArray Sabana = SabanaClientes(FechaInicial, FechaFinal, IdEmpresa, IdUsuario, IdCliente, Incidentes, pConexion);

                Modelo.Add("Clientes", Sabana);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }
    
    private static JArray SabanaClientes(string FechaIncial, string FechaFinal, int IdEmpresa, int IdUsuario, int IdCliente, int Incidentes, CConexion pConexion)
	{
		JArray Sabana = new JArray();


		CSelectEspecifico SabanaClientes = new CSelectEspecifico();
		SabanaClientes.StoredProcedure.CommandText = "sp_Ventas_ClientePorMes";
		SabanaClientes.StoredProcedure.Parameters.Add("@FechaInicial", SqlDbType.VarChar, 10).Value = FechaIncial;
		SabanaClientes.StoredProcedure.Parameters.Add("@FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
		SabanaClientes.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = IdEmpresa;
        SabanaClientes.StoredProcedure.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
        SabanaClientes.StoredProcedure.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
        SabanaClientes.StoredProcedure.Parameters.Add("@pIncidentes", SqlDbType.Int).Value = Incidentes;

		SabanaClientes.Llena(pConexion);
        

		while (SabanaClientes.Registros.Read())
		{
			JObject Cliente = new JObject();

			for (var i = 0; i < SabanaClientes.Registros.FieldCount; i++)
			{
                Cliente.Add(SabanaClientes.Registros.GetName(i), SabanaClientes.Registros[i].ToString());
			}
			Sabana.Add(Cliente);
		}

        SabanaClientes.CerrarConsulta();

		return Sabana;
	}

	[WebMethod]
	public static string RecargarTablaVentasCliente(string FechaInicial, string FechaFinal, int IdSucursal)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Select6 = new CSelectEspecifico();
				Select6.StoredProcedure.CommandType = CommandType.StoredProcedure;
				Select6.StoredProcedure.CommandText = "sp_Oportunidad_VentasPorCliente";
				Select6.StoredProcedure.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = FechaInicial;
                Select6.StoredProcedure.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = FechaFinal;
                Select6.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
                Select6.StoredProcedure.Parameters.Add("@IdSucursal", SqlDbType.Int).Value = IdSucursal;

				Select6.Llena(pConexion);
				JArray VentasCliente = new JArray();

				try
				{
					while (Select6.Registros.Read())
					{
						JObject VentaCliente = new JObject();
						VentaCliente.Add("#", Convert.ToString(Select6.Registros["Num"]));
                        VentaCliente.Add("Clasificación", Convert.ToString(Select6.Registros["TipoCliente"]));
                        VentaCliente.Add("Cliente", Convert.ToString(Select6.Registros["Cliente"]));
						VentaCliente.Add("Monto", Convert.ToDecimal(Select6.Registros["Monto"]).ToString("C"));
						VentasCliente.Add(VentaCliente);
					}
				}
				catch (Exception ex)
				{
					DescripcionError = ex.Message + " - " + ex.StackTrace;
					Error = 1;
				}

				Select6.CerrarConsulta();
				Modelo.Add("VentasCliente", VentasCliente);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerMetasSucursales()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				decimal MetaMonterrey = 0;
				decimal MetaMexico = 0;
				decimal MetaGuadalajara = 0;
				decimal LogroMonterrey = 0;
				decimal LogroMexico = 0;
				decimal LogroGuadalajara = 0;

				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				CUsuario VendedoresMonterrey = new CUsuario();
				pParametros.Add("IdSucursalPredeterminada", 1);
				pParametros.Add("Baja", 0);
				pParametros.Add("EsVendedor", 1);
				pParametros.Add("EsAgente", 1);

				foreach (CUsuario Vendedor in VendedoresMonterrey.LlenaObjetosFiltros(pParametros, pConexion))
					MetaMonterrey += Vendedor.Meta;

				CUsuario VendedoresMexico = new CUsuario();
				pParametros["IdSucursalPredeterminada"] = 6;

				foreach (CUsuario Vendedor in VendedoresMexico.LlenaObjetosFiltros(pParametros, pConexion))
					MetaMexico += Vendedor.Meta;

				CUsuario VendedoresGuadalajara = new CUsuario();
				pParametros["IdSucursalPredeterminada"] = 7;

				foreach (CUsuario Vendedor in VendedoresMexico.LlenaObjetosFiltros(pParametros, pConexion))
					MetaGuadalajara += Vendedor.Meta;

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_LogroSucursales";

				Consulta.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 10).Value = FechaInicio;
				Consulta.StoredProcedure.Parameters.Add("FechaFin", SqlDbType.VarChar, 10).Value = FechaFinal;
				Consulta.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				Consulta.Llena(pConexion);

				if (Consulta.Registros.Read())
				{
					LogroMonterrey = Convert.ToDecimal(Consulta.Registros["Monterrey"]);
					LogroMexico = Convert.ToDecimal(Consulta.Registros["México"]);
					LogroGuadalajara = Convert.ToDecimal(Consulta.Registros["Guadalajara"]);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Monterrey", MetaMonterrey.ToString("C").Split('.')[0]);
				Modelo.Add("Mexico", MetaMexico.ToString("C").Split('.')[0]);
				Modelo.Add("Guadalajara", MetaGuadalajara.ToString("C").Split('.')[0]);
				Modelo.Add("LogroMty", LogroMonterrey.ToString("C").Split('.')[0]);
				Modelo.Add("LogroMex", LogroMexico.ToString("C").Split('.')[0]);
				Modelo.Add("LogroGdl", LogroGuadalajara.ToString("C").Split('.')[0]);
				
				string Hoy = DateTime.Today.ToShortDateString();


				CSelectEspecifico Consulta1 = new CSelectEspecifico();
				Consulta1.StoredProcedure.CommandText = "sp_LogroSucursales";

				Consulta1.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 10).Value = Hoy;
				Consulta1.StoredProcedure.Parameters.Add("FechaFin", SqlDbType.VarChar, 10).Value = Hoy;
				Consulta1.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				Consulta1.Llena(pConexion);

				decimal hoyMonterrey = 0;
				decimal hoyMexico = 0;
				decimal hoyGuadalajara = 0;

				if (Consulta1.Registros.Read() == true)
				{
					hoyMonterrey = Convert.ToDecimal(Consulta1.Registros["Monterrey"]);
					hoyMexico = Convert.ToDecimal(Consulta1.Registros["México"]);
					hoyGuadalajara = Convert.ToDecimal(Consulta1.Registros["Guadalajara"]);
				}
				Consulta1.CerrarConsulta();

				Modelo.Add("hoyMty", hoyMonterrey.ToString("C").Split('.')[0]);
				Modelo.Add("hoyMex", hoyMexico.ToString("C").Split('.')[0]);
				Modelo.Add("hoyGdl", hoyGuadalajara.ToString("C").Split('.')[0]);
				Modelo.Add("Fecha", Hoy);
				
				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerDescripcionDivision (int IdDivision)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CDivision Division = new CDivision();
				Division.LlenaObjeto(IdDivision, pConexion);

				Modelo.Add("Descripcion", Division.Descripcion);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

}