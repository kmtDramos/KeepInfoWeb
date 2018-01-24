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

public partial class Dashboard : System.Web.UI.Page
{

	private static int GridOportunidadesClienteAgenteCreado;
	private static int GridReporteVentasAgentes;

	protected void Page_Load(object sender, EventArgs e)
	{

		GridOportunidadesClienteAgenteCreado = 0;
		GridReporteVentasAgentes = 0;

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			CSucursalAsignada SucursalActual = new CSucursalAsignada();
			Dictionary<string, object> pParametros = new Dictionary<string, object>();
			pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
			pParametros.Add("IdSucursal", UsuarioSesion.IdSucursalActual);

			SucursalActual.LlenaObjetoFiltros(pParametros, pConexion);

			InitCargasIniciales(SucursalActual.IdPerfil, pConexion, this, ClientScript);
		});
		
	}

	[WebMethod]
	public static string ObtenerControles()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
				pParametros.Add("IdSucursal", UsuarioSesion.IdSucursalActual);
				pParametros.Add("Baja", 0);
				SucursalAsignada.LlenaObjetoFiltros(pParametros, pConexion);

				CControlDashboardUsuario ControlesUsuario = new CControlDashboardUsuario();
				pParametros.Clear();
				pParametros.Add("IdPerfil", SucursalAsignada.IdPerfil);
				pParametros.Add("Baja", 0);

				JArray Controles = new JArray();

				foreach (CControlDashboardUsuario ControlUsuario in ControlesUsuario.LlenaObjetosFiltros(pParametros, pConexion))
				{
					JObject Control = new JObject();
					Control.Add("Identificador", ControlUsuario.Identificador);
					Control.Add("Metodo", ControlUsuario.MetodoControl);
					Control.Add("Nombre", ControlUsuario.NombreControl);
					Control.Add("Template", ControlUsuario.TemplateControl);
					Control.Add("Orden", ControlUsuario.Orden);
					Controles.Add(Control);
				}

				Modelo.Add("Controles", Controles);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	private static void InitCargasIniciales(int IdPerfil, CConexion pConexion, Page Page, ClientScriptManager ClientScript)
	{
		CControlDashboardUsuario Controles = new CControlDashboardUsuario();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdPerfil", IdPerfil);
		pParametros.Add("Baja", 0);

		foreach (CControlDashboardUsuario Control in Controles.LlenaObjetosFiltros(pParametros, pConexion))
		{
			switch (Control.IdControl)
			{
				case 1:
					GenerarGridOportunidadesClienteAgente(pConexion, Page, ClientScript);
					break;
				case 2:
					GenerarGridOportunidadesClienteAgente(pConexion, Page, ClientScript);
					GenerarGridVentasAgentes(pConexion, Page, ClientScript);
					break;
			}
		}
	}

	/*********************************************************************
	 * Pantalla de Analisi de venta de agente
	 ********************************************************************/
	[WebMethod]
	public static string ObtenerAnalisisVentasAgente()
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				decimal Meta = 0;
				int EficienciaMesAnterior = 0;
				decimal OportunidadBaja = 0;
				decimal OportunidadMedia = 0;
				decimal OportunidadAlta = 0;
				decimal PronosticoVenta = 0;
				decimal BolsaNecesaria = 0;
				decimal TiempoTranscurrido = 0;
				decimal TiempoRestante = 0;
				decimal VentasReales = 0;
				decimal VentasMeta = 0;
				decimal Comision = 0;
				decimal ComisionAlcanzada = 0;
				decimal ComisionAlcanzar = 0;
				int ClientesAA = 0;
				int ClientesA = 0;
				int ClientesB = 0;
				int ClientesC = 0;
				int ClientesMostrador = 0;
				int TotalClientesAA = 0;
				int TotalClientesA = 0;
				int TotalClientesB = 0;
				int TotalClientesC = 0;
				int TotalClientesMostrador = 0;

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				CSelectEspecifico Select = new CSelectEspecifico();
				Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
				Select.StoredProcedure.CommandText = "sp_Dashboard_AnalisisVentaAgente";
				Select.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = UsuarioSesion.IdUsuario;
				Select.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

				Select.Llena(pConexion);

				if (Select.Registros.Read())
				{
					Meta = Convert.ToInt32(Select.Registros["Meta"]);
					EficienciaMesAnterior = Convert.ToInt32(Select.Registros["Efectividad"]);
					OportunidadBaja = Convert.ToInt32(Select.Registros["OportunidadBaja"]);
					OportunidadMedia = Convert.ToInt32(Select.Registros["OportunidadMedia"]);
					OportunidadAlta = Convert.ToInt32(Select.Registros["OportunidadAlta"]);
					PronosticoVenta = Convert.ToInt32(Select.Registros["Pronostico"]);
					BolsaNecesaria = Convert.ToInt32(Select.Registros["Bolsa"]);
					TiempoTranscurrido = Convert.ToInt32(Select.Registros["TiempoTranscurrido"]);
					TiempoRestante = Convert.ToInt32(Select.Registros["TiempoRestante"]);
					VentasReales = Convert.ToInt32(Select.Registros["VentasActuales"]);
					VentasMeta = Convert.ToInt32(Select.Registros["VentaPendiente"]);
					Comision = Convert.ToInt32(Select.Registros["PorcentajeComision"]);
					ComisionAlcanzada = Convert.ToInt32(Select.Registros["ComisionAlcanzada"]);
					ComisionAlcanzar = Convert.ToInt32(Select.Registros["ComisionPendiente"]);
					ClientesAA = Convert.ToInt32(Select.Registros["ClientesAA"]);
					ClientesA = Convert.ToInt32(Select.Registros["ClientesA"]);
					ClientesB = Convert.ToInt32(Select.Registros["ClientesB"]);
					ClientesC = Convert.ToInt32(Select.Registros["ClientesC"]);
					ClientesMostrador = Convert.ToInt32(Select.Registros["ClientesMostrador"]);
					TotalClientesAA = Convert.ToInt32(Select.Registros["TotalClientesAA"]);
					TotalClientesA = Convert.ToInt32(Select.Registros["TotalClientesA"]);
					TotalClientesB = Convert.ToInt32(Select.Registros["TotalClientesB"]);
					TotalClientesC = Convert.ToInt32(Select.Registros["TotalClientesC"]);
					TotalClientesMostrador = Convert.ToInt32(Select.Registros["TotalClientesMostrador"]);
				}

				Select.CerrarConsulta();

				Modelo.Add("IdUsuario", UsuarioSesion.IdUsuario);
				Modelo.Add("Agente", UsuarioSesion.Nombre + " " + UsuarioSesion.ApellidoPaterno + " " + UsuarioSesion.ApellidoMaterno);
				Modelo.Add("Meta", Meta.ToString("C"));
				Modelo.Add("Eficiencia", EficienciaMesAnterior);
				Modelo.Add("OportunidadBaja", OportunidadBaja.ToString("C"));
				Modelo.Add("OportunidadMedia", OportunidadMedia.ToString("C"));
				Modelo.Add("OportunidadAlta", OportunidadAlta.ToString("C"));
				Modelo.Add("PronósticoVenta", PronosticoVenta.ToString("C"));
				Modelo.Add("BolsaNecesaria", BolsaNecesaria.ToString("C"));
				Modelo.Add("DiasTranscurridos", TiempoTranscurrido);
				Modelo.Add("DiasRestantes", TiempoRestante);
				Modelo.Add("TotalVentas", VentasReales.ToString("C"));
				Modelo.Add("MontoMeta", VentasMeta.ToString("C"));
				Modelo.Add("PorcentajeComisión", Comision.ToString("C"));
				Modelo.Add("ComisionAlcanzada", ComisionAlcanzada.ToString("C"));
				Modelo.Add("ComisionAlcanzar", ComisionAlcanzar.ToString("C"));
				Modelo.Add("ClientesAA", ClientesAA);
				Modelo.Add("ClientesA", ClientesA);
				Modelo.Add("ClientesB", ClientesB);
				Modelo.Add("ClientesC", ClientesC);
				Modelo.Add("ClientesMostrador", ClientesMostrador);
				Modelo.Add("TotalClientesAA", TotalClientesAA);
				Modelo.Add("TotalClientesA", TotalClientesA);
				Modelo.Add("TotalClientesB", TotalClientesB);
				Modelo.Add("TotalClientesC", TotalClientesC);
				Modelo.Add("TotalClientesMostrador", TotalClientesMostrador);
				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerAnalisisVentasAgenteEspecifico(int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			CUsuario Usuario = new CUsuario();
			Usuario.LlenaObjeto(IdUsuario, pConexion);
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				decimal Meta = 0;
				int EficienciaMesAnterior = 0;
				decimal OportunidadBaja = 0;
				decimal OportunidadMedia = 0;
				decimal OportunidadAlta = 0;
				decimal PronosticoVenta = 0;
				decimal BolsaNecesaria = 0;
				decimal TiempoTranscurrido = 0;
				decimal TiempoRestante = 0;
				decimal VentasReales = 0;
				decimal VentasMeta = 0;
				decimal Comision = 0;
				decimal ComisionAlcanzada = 0;
				decimal ComisionAlcanzar = 0;
				int ClientesAA = 0;
				int ClientesA = 0;
				int ClientesB = 0;
				int ClientesC = 0;
				int ClientesMostrador = 0;
				int TotalClientesAA = 0;
				int TotalClientesA = 0;
				int TotalClientesB = 0;
				int TotalClientesC = 0;
				int TotalClientesMostrador = 0;

				CSucursal Sucursal = new CSucursal();
				Sucursal.LlenaObjeto(Usuario.IdSucursalPredeterminada, pConexion);
				int IdEmpresa = Sucursal.IdEmpresa;

				CSelectEspecifico Select = new CSelectEspecifico();
				Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
				Select.StoredProcedure.CommandText = "sp_Dashboard_AnalisisVentaAgente";
				Select.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = Usuario.IdUsuario;
				Select.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

				Select.Llena(pConexion);

				if (Select.Registros.Read())
				{
					Meta = Convert.ToInt32(Select.Registros["Meta"]);
					EficienciaMesAnterior = Convert.ToInt32(Select.Registros["Efectividad"]);
					OportunidadBaja = Convert.ToInt32(Select.Registros["OportunidadBaja"]);
					OportunidadMedia = Convert.ToInt32(Select.Registros["OportunidadMedia"]);
					OportunidadAlta = Convert.ToInt32(Select.Registros["OportunidadAlta"]);
					PronosticoVenta = Convert.ToInt32(Select.Registros["Pronostico"]);
					BolsaNecesaria = Convert.ToInt32(Select.Registros["Bolsa"]);
					TiempoTranscurrido = Convert.ToInt32(Select.Registros["TiempoTranscurrido"]);
					TiempoRestante = Convert.ToInt32(Select.Registros["TiempoRestante"]);
					VentasReales = Convert.ToInt32(Select.Registros["VentasActuales"]);
					VentasMeta = Convert.ToInt32(Select.Registros["VentaPendiente"]);
					Comision = Convert.ToInt32(Select.Registros["PorcentajeComision"]);
					ComisionAlcanzada = Convert.ToInt32(Select.Registros["ComisionAlcanzada"]);
					ComisionAlcanzar = Convert.ToInt32(Select.Registros["ComisionPendiente"]);
					ClientesAA = Convert.ToInt32(Select.Registros["ClientesAA"]);
					ClientesA = Convert.ToInt32(Select.Registros["ClientesA"]);
					ClientesB = Convert.ToInt32(Select.Registros["ClientesB"]);
					ClientesC = Convert.ToInt32(Select.Registros["ClientesC"]);
					ClientesMostrador = Convert.ToInt32(Select.Registros["ClientesMostrador"]);
					TotalClientesAA = Convert.ToInt32(Select.Registros["TotalClientesAA"]);
					TotalClientesA = Convert.ToInt32(Select.Registros["TotalClientesA"]);
					TotalClientesB = Convert.ToInt32(Select.Registros["TotalClientesB"]);
					TotalClientesC = Convert.ToInt32(Select.Registros["TotalClientesC"]);
					TotalClientesMostrador = Convert.ToInt32(Select.Registros["TotalClientesMostrador"]);
				}

				Select.CerrarConsulta();


				JArray FamiliasOportunidades = new JArray();
				decimal TotalAlta = 0;
				decimal TotalMedia = 0;
				decimal TotalBaja = 0;

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_TablaFamiliaOportunidadAgente";
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = Usuario.IdUsuario;
				Consulta.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					JObject FamiliaOportunidad = new JObject();
					string Familia = Convert.ToString(Consulta.Registros["Familia"]);
					decimal Alta = Convert.ToDecimal(Consulta.Registros["Alta"]);
					decimal Media = Convert.ToDecimal(Consulta.Registros["Media"]);
					decimal Baja = Convert.ToDecimal(Consulta.Registros["Baja"]);
					decimal Total = Convert.ToDecimal(Consulta.Registros["Total"]);
					TotalAlta += Alta;
					TotalMedia += Media;
					TotalBaja += Baja;
					FamiliaOportunidad.Add("Familia", Familia);
					FamiliaOportunidad.Add("Alta", Alta.ToString("C"));
					FamiliaOportunidad.Add("Media", Media.ToString("C"));
					FamiliaOportunidad.Add("Baja", Baja.ToString("C"));
					FamiliaOportunidad.Add("Total", Total.ToString("C"));
					FamiliasOportunidades.Add(FamiliaOportunidad);
				}

				Consulta.CerrarConsulta();
				
				Modelo.Add("IdUsuario", Usuario.IdUsuario);
				Modelo.Add("Agente", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
				Modelo.Add("Meta", Meta.ToString("C"));
				Modelo.Add("Eficiencia", EficienciaMesAnterior);
				Modelo.Add("OportunidadBaja", OportunidadBaja.ToString("C"));
				Modelo.Add("OportunidadMedia", OportunidadMedia.ToString("C"));
				Modelo.Add("OportunidadAlta", OportunidadAlta.ToString("C"));
				Modelo.Add("PronósticoVenta", PronosticoVenta.ToString("C"));
				Modelo.Add("BolsaNecesaria", BolsaNecesaria.ToString("C"));
				Modelo.Add("DiasTranscurridos", TiempoTranscurrido);
				Modelo.Add("DiasRestantes", TiempoRestante);
				Modelo.Add("TotalVentas", VentasReales.ToString("C"));
				Modelo.Add("MontoMeta", VentasMeta.ToString("C"));
				Modelo.Add("PorcentajeComisión", Comision.ToString("C"));
				Modelo.Add("ComisionAlcanzada", ComisionAlcanzada.ToString("C"));
				Modelo.Add("ComisionAlcanzar", ComisionAlcanzar.ToString("C"));
				Modelo.Add("ClientesAA", ClientesAA);
				Modelo.Add("ClientesA", ClientesA);
				Modelo.Add("ClientesB", ClientesB);
				Modelo.Add("ClientesC", ClientesC);
				Modelo.Add("ClientesMostrador", ClientesMostrador);
				Modelo.Add("TotalClientesAA", TotalClientesAA);
				Modelo.Add("TotalClientesA", TotalClientesA);
				Modelo.Add("TotalClientesB", TotalClientesB);
				Modelo.Add("TotalClientesC", TotalClientesC);
				Modelo.Add("TotalClientesMostrador", TotalClientesMostrador);
				Modelo.Add("FamiliasOportunidades", FamiliasOportunidades);
				Modelo.Add("TotalAlta", TotalAlta);
				Modelo.Add("TotalMedia", TotalMedia);
				Modelo.Add("TotalBaja", TotalBaja);
				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	private static void GenerarGridOportunidadesClienteAgente(CConexion pConexion, Page Page, ClientScriptManager ClientScript) {
		if (GridOportunidadesClienteAgenteCreado == 0)
		{
			//GridOportunidad
			CJQGrid GridVentasAgente = new CJQGrid();
			GridVentasAgente.NombreTabla = "grdVentasAgente";
			GridVentasAgente.CampoIdentificador = "IdOportunidad";
			GridVentasAgente.ColumnaOrdenacion = "IdOportunidad";
			GridVentasAgente.TipoOrdenacion = "DESC";
			GridVentasAgente.Metodo = "ObtenerOportunidadesClienteAgente";
			GridVentasAgente.TituloTabla = "Oportunidades de Cliente";
			GridVentasAgente.GenerarGridCargaInicial = false;
			GridVentasAgente.GenerarFuncionFiltro = false;
			GridVentasAgente.GenerarFuncionTerminado = false;
			GridVentasAgente.NumeroFila = false;
			GridVentasAgente.Altura = 220;
			GridVentasAgente.Ancho = 1050;
			GridVentasAgente.NumeroRegistros = 50;
			GridVentasAgente.RangoNumeroRegistros = "10,25,50";

			//IdOportunidad
			CJQColumn ColIdOpotunidad = new CJQColumn();
			ColIdOpotunidad.Nombre = "IdOportunidad";
			ColIdOpotunidad.Encabezado = "No. Opt";
			ColIdOpotunidad.Ordenable = "false";
			ColIdOpotunidad.Oculto = "false";
			ColIdOpotunidad.Ancho = "50";
			GridVentasAgente.Columnas.Add(ColIdOpotunidad);

			//IdEmpresa
			CJQColumn ColCliente = new CJQColumn();
			ColCliente.Nombre = "Cliente";
			ColCliente.Encabezado = "Cliente";
			ColCliente.Ancho = "150";
			ColCliente.Ordenable = "false";
			ColCliente.Alineacion = "Left";
			GridVentasAgente.Columnas.Add(ColCliente);

			CJQColumn ColTipoCliente = new CJQColumn();
			ColTipoCliente.Nombre = "TipoCliente";
			ColTipoCliente.Encabezado = "Tipo cliente";
			ColTipoCliente.Ancho = "80";
			ColTipoCliente.Ordenable = "false";
			ColTipoCliente.Buscador = "true";
			ColTipoCliente.TipoBuscador = "Combo";
			ColTipoCliente.StoredProcedure.CommandText = "sp_Cliente_Consultar_TipoCliente";
			GridVentasAgente.Columnas.Add(ColTipoCliente);

			//MedioContacto
			CJQColumn ColFormaContacto = new CJQColumn();
			ColFormaContacto.Nombre = "FormaContacto";
			ColFormaContacto.Encabezado = "Forma de Contacto";
			ColFormaContacto.Ancho = "80";
			ColFormaContacto.Alineacion = "Left";
			ColFormaContacto.Ordenable = "false";
			ColFormaContacto.Buscador = "false";
			GridVentasAgente.Columnas.Add(ColFormaContacto);

			//Division
			CJQColumn ColDivision = new CJQColumn();
			ColDivision.Nombre = "Division";
			ColDivision.Encabezado = "División";
			ColDivision.Ancho = "100";
			ColDivision.Alineacion = "Left";
			ColDivision.Buscador = "true";
			ColDivision.TipoBuscador = "Combo";
			ColDivision.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Division";
			GridVentasAgente.Columnas.Add(ColDivision);

			//Oportunidad
			CJQColumn ColOportunidad = new CJQColumn();
			ColOportunidad.Nombre = "Oportunidad";
			ColOportunidad.Encabezado = "Oportunidad";
			ColOportunidad.Ancho = "100";
			ColOportunidad.Ordenable = "false";
			ColOportunidad.Alineacion = "Left";
			GridVentasAgente.Columnas.Add(ColOportunidad);

			//OportunidadAlta
			CJQColumn ColOptAlta = new CJQColumn();
			ColOptAlta.Nombre = "OportunidadAlta";
			ColOptAlta.Encabezado = "Alta";
			ColOptAlta.Ancho = "90";
			ColOptAlta.Alineacion = "Right";
			ColOptAlta.Ordenable = "false";
			ColOptAlta.Buscador = "false";
			GridVentasAgente.Columnas.Add(ColOptAlta);

			//OportunidadMedia
			CJQColumn ColOptMedia = new CJQColumn();
			ColOptMedia.Nombre = "OportunidadMedia";
			ColOptMedia.Encabezado = "Media";
			ColOptMedia.Ancho = "90";
			ColOptMedia.Ordenable = "false";
			ColOptMedia.Alineacion = "Right";
			ColOptMedia.Buscador = "false";
			GridVentasAgente.Columnas.Add(ColOptMedia);

			//OportunidadBaja
			CJQColumn ColOptBaja = new CJQColumn();
			ColOptBaja.Nombre = "OportunidadBaja";
			ColOptBaja.Encabezado = "Baja";
			ColOptBaja.Ancho = "90";
			ColOptBaja.Alineacion = "Right";
			ColOptBaja.Ordenable = "false";
			ColOptBaja.Buscador = "false";
			GridVentasAgente.Columnas.Add(ColOptBaja);

			//Ganado
			CJQColumn ColFacturado = new CJQColumn();
			ColFacturado.Nombre = "Facturado";
			ColFacturado.Encabezado = "Facturado";
			ColFacturado.Ancho = "90";
			ColFacturado.Alineacion = "Right";
			ColFacturado.Ordenable = "false";
			ColFacturado.Buscador = "false";
			GridVentasAgente.Columnas.Add(ColFacturado);

			//Apertura
			CJQColumn ColFechaCreacion = new CJQColumn();
			ColFechaCreacion.Nombre = "FechaCreacion";
			ColFechaCreacion.Encabezado = "Dias";
			ColFechaCreacion.Ancho = "80";
			ColFechaCreacion.Alineacion = "Center";
			ColFechaCreacion.Ordenable = "false";
			ColFechaCreacion.Buscador = "false";
			GridVentasAgente.Columnas.Add(ColFechaCreacion);

			//Cierre
			CJQColumn ColFechaCierre = new CJQColumn();
			ColFechaCierre.Nombre = "FechaCierre";
			ColFechaCierre.Encabezado = "Cierre";
			ColFechaCierre.Ancho = "80";
			ColFechaCierre.Alineacion = "Center";
			ColFechaCierre.Ordenable = "false";
			ColFechaCierre.Buscador = "false";
			ColFechaCierre.Oculto = "true";
			GridVentasAgente.Columnas.Add(ColFechaCierre);

			// ActividadesAFuturo
			CJQColumn ColActividadesAFuturo = new CJQColumn();
			ColActividadesAFuturo.Nombre = "ActividadesAFuturo";
			ColActividadesAFuturo.Encabezado = "Seguimientos";
			ColActividadesAFuturo.Ancho = "50";
			ColActividadesAFuturo.Alineacion = "Center";
			ColActividadesAFuturo.TipoBuscador = "Combo";
			ColActividadesAFuturo.StoredProcedure.CommandText = "sp_Oportunidad_FiltroProyecto";
			GridVentasAgente.Columnas.Add(ColActividadesAFuturo);

			ClientScript.RegisterStartupScript(Page.GetType(), "grdVentasAgente", GridVentasAgente.GeneraGrid(), true);

			GridOportunidadesClienteAgenteCreado = 1;
		}
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
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerOportunidadesClienteAgente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCliente, string pOportunidad, int IdUsuario, int IdTipoCliente, int IdDivision, int Seguimiento)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdVentasAgente", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pOportunidad", SqlDbType.VarChar, 255).Value = Convert.ToString(pOportunidad);
		Stored.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = Convert.ToString(pCliente);
		Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = IdUsuario;
		Stored.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
		Stored.Parameters.Add("pIdTipoCliente", SqlDbType.Int).Value = Convert.ToInt32(IdTipoCliente);
		Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = Convert.ToInt32(IdDivision);
		Stored.Parameters.Add("pSeguimiento", SqlDbType.Int).Value = Convert.ToInt32(Seguimiento);

		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

	/*********************************************************************
	 * Dashboard de Ventas por Agente
	 ********************************************************************/
	private static void GenerarGridVentasAgentes(CConexion pConexion, Page Page, ClientScriptManager ClientScript)
	{
		if (GridReporteVentasAgentes == 0)
		{
			//GridOportunidad
			CJQGrid GridVentasAgentes = new CJQGrid();
			GridVentasAgentes.NombreTabla = "grdReporteVentasAgentes";
			GridVentasAgentes.CampoIdentificador = "IdUsuario";
			GridVentasAgentes.ColumnaOrdenacion = "Agente";
			GridVentasAgentes.TipoOrdenacion = "ASC";
			GridVentasAgentes.Metodo = "ObtenerVentasAgentes";
			GridVentasAgentes.TituloTabla = "Ventas de agentes";
			GridVentasAgentes.GenerarGridCargaInicial = false;
			GridVentasAgentes.GenerarFuncionFiltro = false;
			GridVentasAgentes.GenerarFuncionTerminado = false;
			GridVentasAgentes.NumeroFila = false;
			GridVentasAgentes.Altura = 350;
			GridVentasAgentes.Ancho = 900;
			GridVentasAgentes.NumeroRegistros = 25;
			GridVentasAgentes.RangoNumeroRegistros = "25,40,100";

			CJQColumn ColIdUsuario = new CJQColumn();
			ColIdUsuario.Nombre = "IdUsuario";
			ColIdUsuario.Etiquetado = "IdUsuario";
			ColIdUsuario.Buscador = "false";
			ColIdUsuario.Oculto = "true";
			GridVentasAgentes.Columnas.Add(ColIdUsuario);

			CJQColumn ColAgente = new CJQColumn();
			ColAgente.Nombre = "Agente";
			ColAgente.Encabezado = "Agente";
			ColAgente.Alineacion = "Left";
			ColAgente.Ancho = "140";
			GridVentasAgentes.Columnas.Add(ColAgente);

			CJQColumn ColSucursal = new CJQColumn();
			ColSucursal.Nombre = "Sucursal";
			ColSucursal.Encabezado = "Sucurusal";
			ColSucursal.Ancho = "110";
			ColSucursal.TipoBuscador = "Combo";
			ColSucursal.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Sucursal";
			ColSucursal.StoredProcedure.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
			GridVentasAgentes.Columnas.Add(ColSucursal);

			CJQColumn ColOptsBaja = new CJQColumn();
			ColOptsBaja.Nombre = "OportunidadBaja";
			ColOptsBaja.Encabezado = "Opts B 2%";
			ColOptsBaja.Alineacion = "Right";
			ColOptsBaja.Ancho = "90";
			ColOptsBaja.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColOptsBaja);

			CJQColumn ColOptsMedia = new CJQColumn();
			ColOptsMedia.Nombre = "OportunidadMedia";
			ColOptsMedia.Encabezado = "Opts M 20%";
			ColOptsMedia.Alineacion = "Right";
			ColOptsMedia.Ancho = "90";
			ColOptsMedia.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColOptsMedia);

			CJQColumn ColOptsAlta = new CJQColumn();
			ColOptsAlta.Nombre = "OportunidadAlta";
			ColOptsAlta.Encabezado = "Opts A 75%";
			ColOptsAlta.Alineacion = "Right";
			ColOptsAlta.Ancho = "90";
			ColOptsAlta.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColOptsAlta);

			CJQColumn ColClientes = new CJQColumn();
			ColClientes.Nombre = "ClientesAtendidos";
			ColClientes.Encabezado = "Clientes";
			ColClientes.Ancho = "50";
			ColClientes.Buscador = "false";
			ColClientes.Alineacion = "Center";
			GridVentasAgentes.Columnas.Add(ColClientes);

			CJQColumn ColTotalOportunidades = new CJQColumn();
			ColTotalOportunidades.Nombre = "TotalOportunidades";
			ColTotalOportunidades.Encabezado = "Pron global";
			ColTotalOportunidades.Alineacion = "Right";
			ColTotalOportunidades.Ancho = "90";
			ColTotalOportunidades.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColTotalOportunidades);

			CJQColumn ColActividades = new CJQColumn();
			ColActividades.Nombre = "Actividades";
			ColActividades.Encabezado = "Segs.";
			ColActividades.Ancho = "50";
			ColActividades.Buscador = "false";
			ColActividades.Alineacion = "Center";
			GridVentasAgentes.Columnas.Add(ColActividades);

			CJQColumn ColProspeciones = new CJQColumn();
			ColProspeciones.Nombre = "Prospeciones";
			ColProspeciones.Encabezado = "Pros.";
			ColProspeciones.Ancho = "50";
			ColProspeciones.Buscador = "false";
			ColProspeciones.Alineacion = "Center";
			GridVentasAgentes.Columnas.Add(ColProspeciones);

			CJQColumn ColMeta = new CJQColumn();
			ColMeta.Nombre = "Meta";
			ColMeta.Encabezado = "Meta mes act";
			ColMeta.Alineacion = "Right";
			ColMeta.Ancho = "90";
			ColMeta.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColMeta);

			CJQColumn ColVentasMesAnterior = new CJQColumn();
			ColVentasMesAnterior.Nombre = "VentasMesAnterior";
			ColVentasMesAnterior.Encabezado = "Vta mes ant";
			ColVentasMesAnterior.Buscador = "false";
			ColVentasMesAnterior.Ancho = "90";
			ColVentasMesAnterior.Oculto = "true";
			ColVentasMesAnterior.Alineacion = "Right";
			//GridVentasAgentes.Columnas.Add(ColVentasMesAnterior);

			CJQColumn ColVentas = new CJQColumn();
			ColVentas.Nombre = "Ventas";
			ColVentas.Encabezado = "Vta mes act";
			ColVentas.Alineacion = "Right";
			ColVentas.Ancho = "90";
			ColVentas.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColVentas);

			CJQColumn ColPronostico = new CJQColumn();
			ColPronostico.Nombre = "Pronostico";
			ColPronostico.Encabezado = "X vend mes act";
			ColPronostico.Alineacion = "Right";
			ColPronostico.Ancho = "90";
			ColPronostico.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColPronostico);

			/*CJQColumn ColOportunidadesMesAnterior = new CJQColumn();
			ColOportunidadesMesAnterior.Nombre = "OportunidadesMesAnterior";
			ColOportunidadesMesAnterior.Encabezado = "Opts Mes Ante.";
			ColOportunidadesMesAnterior.Buscador = "false";
			ColOportunidadesMesAnterior.Oculto = "true";
			ColOportunidadesMesAnterior.Ancho = "90";
			ColOportunidadesMesAnterior.Alineacion = "Right";
			GridVentasAgentes.Columnas.Add(ColOportunidadesMesAnterior);*/

			/*CJQColumn ColDiasTranscurridos = new CJQColumn();
			ColDiasTranscurridos.Nombre = "DiasTranscurridos";
			ColDiasTranscurridos.Encabezado = "Días Transcurridos";
			ColDiasTranscurridos.Ancho = "60";
			ColDiasTranscurridos.Buscador = "false";
			ColDiasTranscurridos.Oculto = "true";
			//GridVentasAgentes.Columnas.Add(ColDiasTranscurridos);*/

			/*CJQColumn ColEfectividad = new CJQColumn();
			ColEfectividad.Nombre = "Efectividad";
			ColEfectividad.Encabezado = "Efectividad";
			ColEfectividad.Ancho = "80";
			ColEfectividad.Buscador = "false";
			ColEfectividad.Oculto = "true";
			//GridVentasAgentes.Columnas.Add(ColEfectividad);*/

			/*CJQColumn ColBolsa = new CJQColumn();
			ColBolsa.Nombre = "Bolsa";
			ColBolsa.Encabezado = "Bolsa";
			ColBolsa.Alineacion = "Right";
			ColBolsa.Oculto = "true";
			ColBolsa.Ancho = "90";
			ColBolsa.Buscador = "false";
			//GridVentasAgentes.Columnas.Add(ColBolsa);*/

			/*CJQColumn ColComision = new CJQColumn();
			ColComision.Nombre = "Comision";
			ColComision.Encabezado = "Comisión";
			ColComision.Ancho = "90";
			ColComision.Buscador = "false";
			ColComision.Oculto = "true";
			//GridVentasAgentes.Columnas.Add(ColComision);*/

			CJQColumn ColAvanceMeta = new CJQColumn();
			ColAvanceMeta.Nombre = "AvanceMeta";
			ColAvanceMeta.Encabezado = "Logro";
			ColAvanceMeta.Alineacion = "Center";
			ColAvanceMeta.Ancho = "50";
			ColAvanceMeta.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColAvanceMeta);

			CJQColumn ColAvanceTiempo = new CJQColumn();
			ColAvanceTiempo.Nombre = "AvanceTiempo";
			ColAvanceTiempo.Encabezado = "% Tiempo";
			ColAvanceTiempo.Buscador = "false";
			ColAvanceTiempo.Ancho = "50";
			//GridVentasAgentes.Columnas.Add(ColAvanceTiempo);

			CJQColumn ColDiasRestantes = new CJQColumn();
			ColDiasRestantes.Nombre = "DiasRestantes";
			ColDiasRestantes.Encabezado = "Días restantes";
			ColDiasRestantes.Ancho = "50";
			ColDiasRestantes.Buscador = "false";
			GridVentasAgentes.Columnas.Add(ColDiasRestantes);

			CJQColumn ColGrafica = new CJQColumn();
			ColGrafica.Nombre = "Grafica";
			ColGrafica.Encabezado = "Grafica";
			ColGrafica.Buscador = "false";
			ColGrafica.Ordenable = "false";
			ColGrafica.Ancho = "25";
			//GridVentasAgentes.Columnas.Add(ColGrafica);

			CJQColumn ColConsultar = new CJQColumn();
			ColConsultar.Nombre = "Consultar";
			ColConsultar.Encabezado = "Ver";
			ColConsultar.Etiquetado = "ImagenConsultar";
			ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarOportunidad";
			ColConsultar.Buscador = "false";
			ColConsultar.Ordenable = "false";
			ColConsultar.Ancho = "25";
			GridVentasAgentes.Columnas.Add(ColConsultar);

			ClientScript.RegisterStartupScript(Page.GetType(), "grdVentasAgentes", GridVentasAgentes.GeneraGrid(), true);

			GridReporteVentasAgentes = 1;
		}
	}

	[WebMethod]
	public static string ObtenerControlReporteVentasAgentes()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();
				Modelo.Add("IdEmpresa", Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]));
				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
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
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerVentasAgentes(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pAgente, int pIdSucursal)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdReporteVentasAgentes", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
		Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 4).Value = pAgente;
		Stored.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

	[WebMethod]
	public static string ObtenerTotales(string Agente, int IdSucursal)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				decimal Meta = 0;
				decimal Pronostico = 0;
				decimal Diferencia = 0;
				decimal Venta = 0;

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_TotalesResultadosFunnel";
				Consulta.StoredProcedure.Parameters.Add("@Agente", SqlDbType.VarChar, 255).Value = Agente;
				Consulta.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal; ;

				Consulta.Llena(pConexion);

				if (Consulta.Registros.Read())
				{
					Meta = Convert.ToDecimal(Consulta.Registros["Meta"]);
					Diferencia = Convert.ToDecimal(Consulta.Registros["Diferencia"]);
					Pronostico = Convert.ToDecimal(Consulta.Registros["Pronostico"]);
					Venta = Convert.ToDecimal(Consulta.Registros["Venta"]);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Meta", Meta.ToString("C"));
				Modelo.Add("Diferencia", Diferencia.ToString("C"));
				Modelo.Add("Pronostico", Pronostico.ToString("C"));
				Modelo.Add("Venta", Venta.ToString("C"));

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string TablaActividadesAFuturoAgente(int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//###########################################################################################################

				JArray Actividades = new JArray();
				int Seguimientos = 0;
				int Prospeccion = 0;
				int Total = 0;

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_TablaActividadesAFuturoAgente";
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;

				Consulta.Llena(pConexion);
				
				while (Consulta.Registros.Read())
				{
					string cliente = Convert.ToString(Consulta.Registros["Cliente"]);
					string agente = Convert.ToString(Consulta.Registros["Agente"]);
					string tipoActividad = Convert.ToString(Consulta.Registros["TipoActividad"]);
					string conOportunidad = Convert.ToString(Consulta.Registros["ConOportunidad"]);
					int IdOportunidad = Convert.ToInt32(Consulta.Registros["IdOportunidad"]);
					string actividad = Convert.ToString(Consulta.Registros["Actividad"]);
					string fechaActividad = Convert.ToString(Consulta.Registros["FechaActividad"]);
					int idActividad = Convert.ToInt32(Consulta.Registros["IdActividad"]);

					JObject Actividad = new JObject();
					Actividad.Add("Cliente", cliente);
					Actividad.Add("Agente", agente);
					Actividad.Add("TipoActividad", tipoActividad);
					Actividad.Add("ConOportunidad", conOportunidad);
					Actividad.Add("IdOportunidad", IdOportunidad);
					Actividad.Add("Actividad", actividad);
					Actividad.Add("FechaActividad", fechaActividad);
					Actividad.Add("IdActividad", idActividad);

					if (conOportunidad == "SI")
						Seguimientos++;
					else
						Prospeccion++;

					Actividades.Add(Actividad);
					Total++;
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Actividades", Actividades);
				Modelo.Add("Total", Total);
				Modelo.Add("Seguimientos", Seguimientos);
				Modelo.Add("Prospeccion", Prospeccion);

				//###########################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

}