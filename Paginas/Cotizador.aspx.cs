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

public partial class Paginas_Cotizador : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {

            GenerarGridCotizador(this, ClientScript, pConexion);

        });

    }

    public static void GenerarGridCotizador(Page Page, ClientScriptManager ClientScript, CConexion pConexion)
    {
        CJQGrid GridCotizador = new CJQGrid();
        GridCotizador.NombreTabla = "grdCotizador";
        GridCotizador.CampoIdentificador = "IdPresupuesto";
        GridCotizador.ColumnaOrdenacion = "IdPresupuesto";
        GridCotizador.TipoOrdenacion = "DESC";
        GridCotizador.Metodo = "ObtenerPresupuesto";
        GridCotizador.TituloTabla = "Cotizaciones";
        GridCotizador.GenerarGridCargaInicial = false;
        GridCotizador.GenerarFuncionFiltro = false;
        GridCotizador.GenerarFuncionTerminado = false;
        GridCotizador.NumeroFila = false;
        GridCotizador.Altura = 350;
        GridCotizador.Ancho = 940;
        GridCotizador.NumeroRegistros = 25;
        GridCotizador.RangoNumeroRegistros = "25,40,100";

        CJQColumn ColIdPresupuesto = new CJQColumn();
        ColIdPresupuesto.Nombre = "IdPresupuesto";
        ColIdPresupuesto.Encabezado = "IdPresupuesto";
        ColIdPresupuesto.Oculto = "true";
        ColIdPresupuesto.Buscador = "false";
        GridCotizador.Columnas.Add(ColIdPresupuesto);

        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Ancho = "50";
        ColFolio.Alineacion = "left";
        GridCotizador.Columnas.Add(ColFolio);

        CJQColumn ColCliente = new CJQColumn();
        ColCliente.Nombre = "Cliente";
        ColCliente.Encabezado = "Cliente";
        ColCliente.Ancho = "170";
        ColCliente.Alineacion = "left";
        GridCotizador.Columnas.Add(ColCliente);

        CJQColumn ColIdOportunidad = new CJQColumn();
        ColIdOportunidad.Nombre = "IdOportunidad";
        ColIdOportunidad.Encabezado = "Oportunidad";
        ColIdOportunidad.Ancho = "50";
        ColIdOportunidad.Alineacion = "right";
        GridCotizador.Columnas.Add(ColIdOportunidad);

        CJQColumn ColAgente = new CJQColumn();
        ColAgente.Nombre = "Agente";
        ColAgente.Encabezado = "Agente";
        ColAgente.Ancho = "170";
        ColAgente.Alineacion = "left";
        GridCotizador.Columnas.Add(ColAgente);

        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Moneda";
        ColTipoMoneda.Ancho = "80";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Buscador = "true";
        ColTipoMoneda.TipoBuscador = "Combo";
        ColTipoMoneda.StoredProcedure.CommandText = "sp_TipoMoneda_Filtro";
        GridCotizador.Columnas.Add(ColTipoMoneda);

        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Ancho = "70";
		ColTotal.Formato = "FormatoMoneda";
		ColTotal.Alineacion = "right";
        ColTotal.Buscador = "false";
        GridCotizador.Columnas.Add(ColTotal);

        CJQColumn ColCosto = new CJQColumn();
        ColCosto.Nombre = "Costo";
        ColCosto.Encabezado = "Costo";
        ColCosto.Ancho = "70";
		ColCosto.Formato = "FormatoMoneda";
		ColCosto.Alineacion = "right";
        ColCosto.Buscador = "false";
        GridCotizador.Columnas.Add(ColCosto);

		CJQColumn ColManoObra = new CJQColumn();
		ColManoObra.Nombre = "ManoObra";
		ColManoObra.Encabezado = "Mano obra";
		ColManoObra.Ancho = "70";
		ColManoObra.Formato = "FormatoMoneda";
		ColManoObra.Alineacion = "right";
		ColManoObra.Buscador = "false";
		GridCotizador.Columnas.Add(ColManoObra);

        CJQColumn ColUtilidad = new CJQColumn();
        ColUtilidad.Nombre = "Utilidad";
        ColUtilidad.Encabezado = "Utilidad";
        ColUtilidad.Ancho = "70";
		ColUtilidad.Formato = "FormatoMoneda";
		ColUtilidad.Alineacion = "right";
        ColUtilidad.Buscador = "false";
        GridCotizador.Columnas.Add(ColUtilidad);

        CJQColumn ColEstatus = new CJQColumn();
        ColEstatus.Nombre = "Estatus";
        ColEstatus.Encabezado = "Estatus";
        ColEstatus.Ancho = "70";
        ColEstatus.Alineacion = "left";
        ColEstatus.Buscador = "true";
        ColEstatus.TipoBuscador = "Combo";
        ColEstatus.StoredProcedure.CommandText = "sp_Presupuesto_Estatus";
        GridCotizador.Columnas.Add(ColEstatus);

        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "60";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridCotizador.Columnas.Add(ColBaja);

        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCotizacion";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCotizador.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(Page.GetType(), "grdCotizador", GridCotizador.GeneraGrid(), true);

    }

    [WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerPresupuesto(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, int pFolio, string pTipoOrden, string pIdOportunidad, string pAgente, string pCliente, int pAI, int pIdTipoMoneda)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
		
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdCotizador", sqlCon);
		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pFolio", SqlDbType.Int).Value = pFolio;
		Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
        Stored.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
        Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
        Stored.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
		//Stored.Parameters.Add("", SqlDbType.).Value = ;

		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatos();
		return new CJQGridJsonResponse(dataSet);
	}
    
	[WebMethod]
	public static string ObtenerFormaPresupuesto(int IdPresupuesto)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {

			CTipoCambio ValidarTipoCambio = new CTipoCambio();
			Dictionary<string, object> pParametros = new Dictionary<string, object>();
			pParametros.Add("Fecha", DateTime.Today);

			if (ValidarTipoCambio.LlenaObjetosFiltros(pParametros, pConexion).Count == 0)
			{
				Error = 1;
				DescripcionError = "No hay tipo de cambio del día de hoy";
			}
			pParametros.Clear();

			if (Error == 0)
			{
				JObject Modelo = new JObject();

				// Variables a devolver con sus valores default
				int IdCliente = 0;
				string RazonSocial = "";
				string FechaExpiracion = DateTime.Today.AddDays(7).ToShortDateString();
				string Nota = "LIBRE A BORDO: MEXICO D.F./ MTY TODA CANCELACIÓN,  DEVOLUCIÓN DE PRODUCTO EN CONDICIONES DE VENTA  O CHEQUE DEVUELTO GENERA UN CARGO DEL 20% MAS IVA CONDICIONES DE PAGO. LOS PRECIOS PUEDEN SER SUJETOS A CAMBIOS SIN PREVIO AVISO.";
				decimal TipoCambio = 1;

				// Arreglos a devolver en la consulta
				JArray Conceptos = new JArray();
				JArray ListaOportunidades = new JArray();
				JArray ListaContactos = new JArray();
				JArray ListaDirecciones = new JArray();
				JArray ListaEstatus = new JArray();

				// Llenado del presupuesto con el que se va a trabajar
				CPresupuesto Presupuesto = new CPresupuesto();
				Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);

				if (Presupuesto.IdPresupuesto != 0)
				#region Editar presupuesto
				{
					CCliente Cliente = new CCliente();
					Cliente.LlenaObjeto(Presupuesto.IdCliente, pConexion);
					IdCliente = Cliente.IdCliente;

					COrganizacion Organizacion = new COrganizacion();
					Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

					RazonSocial = Organizacion.RazonSocial;
					FechaExpiracion = Presupuesto.FechaExpiracion.ToShortDateString();
					Nota = Presupuesto.Nota;
					TipoCambio = (Presupuesto.TipoCambio == 0) ? TipoCambio: Presupuesto.TipoCambio;

					CContactoOrganizacion Contactos = new CContactoOrganizacion();
					pParametros.Clear();
					pParametros.Add("IdOrganizacion", Organizacion.IdOrganizacion);
					pParametros.Add("Baja", 0);

					foreach (CContactoOrganizacion Contacto in Contactos.LlenaObjetosFiltros(pParametros, pConexion))
					#region Contactos
					{
						JObject Opcion = new JObject();
						Opcion.Add("IdContactoOrganizacion", Contacto.IdContactoOrganizacion);
						Opcion.Add("Nombre", Contacto.Nombre);
						Opcion.Add("Selected", (Presupuesto.IdContactoOrganizacion == Contacto.IdContactoOrganizacion) ? "selected" : "");
						ListaContactos.Add(Opcion);
					}
					#endregion

					CDireccionOrganizacion Direcciones = new CDireccionOrganizacion();
					pParametros.Clear();
					pParametros.Add("IdOrganizacion", Cliente.IdOrganizacion);
					pParametros.Add("Baja", 0);

					foreach (CDireccionOrganizacion Direccion in Direcciones.LlenaObjetosFiltros(pParametros, pConexion))
					#region Direcciones
					{
						JObject Opcion = new JObject();
						Opcion.Add("IdDireccionOrganizacion", Direccion.IdDireccionOrganizacion);
						Opcion.Add("Selected", (Presupuesto.IdDireccionOrganizacion == Direccion.IdDireccionOrganizacion) ? "selected" : "");
						Opcion.Add("Descripcion", Direccion.Descripcion);
						ListaDirecciones.Add(Opcion);
					}
					#endregion

					CPresupuestoConcepto PresupuestoConceptos = new CPresupuestoConcepto();
					pParametros.Clear();
					pParametros.Add("Baja", 0);
					pParametros.Add("IdPresupuesto", Presupuesto.IdPresupuesto);

					foreach (CPresupuestoConcepto Concepto in PresupuestoConceptos.LlenaObjetosFiltros(pParametros, pConexion))
					#region Conceptos
					{
						JObject jConcepto = new JObject();

						int IdPresupuestoConcepto = Concepto.IdPresupuestoConcepto;
						decimal Orden = Concepto.Orden;
						string Clave = Concepto.Clave;
						string Descripcion = Concepto.Descripcion;
						string Proveedor = Concepto.Proveedor;
						decimal CostoUnitario = Concepto.Costo;
						decimal ManoObra = Concepto.ManoObra;
						decimal Descuento = Concepto.Descuento;
                        int IdProdcuto = Concepto.IdProducto;
                        int IdServicio = Concepto.IdServicio;
						decimal Cantidad = Concepto.Cantidad;
						decimal PrecioUnitario = Concepto.PrecioUnitario;
						decimal CostoTotal = CostoUnitario * Cantidad;
						decimal Margen = Concepto.Margen;
						bool IVA = (Concepto.IVA > 0);
						decimal Total = Concepto.Total;
						decimal Utilidad = Concepto.Utilidad;

						jConcepto.Add("IdConcepto", Concepto.IdPresupuestoConcepto);
						jConcepto.Add("Orden", Concepto.Orden);
						jConcepto.Add("Clave", Concepto.Clave);
                        jConcepto.Add("IdProducto", Concepto.IdProducto);
                        jConcepto.Add("IdServicio", Concepto.IdServicio);
                        jConcepto.Add("Descripcion", Concepto.Descripcion);
						jConcepto.Add("Proveedor", Concepto.Proveedor);
                        jConcepto.Add("PrecioUnitario", Concepto.PrecioUnitario.ToString("C"));
						jConcepto.Add("CostoUnitario", Concepto.Costo.ToString("C"));
						jConcepto.Add("ManoObra", Concepto.ManoObra.ToString("C"));
						jConcepto.Add("Margen", Margen);
						jConcepto.Add("Descuento", Descuento);
						jConcepto.Add("Cantidad", Cantidad);
						jConcepto.Add("IVA", IVA);
						jConcepto.Add("Utilidad", Utilidad.ToString("C"));
						jConcepto.Add("IdDivision", Concepto.IdDivision);
						jConcepto.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(pConexion));

						Conceptos.Add(jConcepto);

					}
					#endregion

					COportunidad OportunidadesUsuario = new COportunidad();
					pParametros.Clear();
					pParametros.Add("IdCliente", Cliente.IdCliente);
					pParametros.Add("Baja", 0);
					pParametros.Add("Cerrado", 0);

					foreach(COportunidad Oportunidad in OportunidadesUsuario.LlenaObjetosFiltros(pParametros, pConexion))
					#region Oportunidades cliente
					{
						JObject Opcion = new JObject();

						Opcion.Add("IdOportunidad", Oportunidad.IdOportunidad);
						Opcion.Add("Selected", (Oportunidad.IdOportunidad == Presupuesto.IdOportunidad) ? "selected": "");
						Opcion.Add("Oportunidad", Oportunidad.Oportunidad + " (" + Oportunidad.IdOportunidad + ")");

						ListaOportunidades.Add(Opcion);

					}
					#endregion

				}
				else
				{
					Presupuesto.TipoCambio = TipoCambio;
				}
				#endregion

				CEstatusPresupuesto Estatus = new CEstatusPresupuesto();
				pParametros.Clear();
				pParametros.Add("Baja", 0);

				foreach (CEstatusPresupuesto estatus in Estatus.LlenaObjetosFiltros(pParametros, pConexion))
				#region Estatus
				{
					JObject Opcion = new JObject();
					Opcion.Add("IdEstatusPresupuesto", estatus.IdEstatusPresupuesto);
					Opcion.Add("Selected", (estatus.IdEstatusPresupuesto == Presupuesto.IdEstatusPresupuesto) ? "selected" : "");
					Opcion.Add("EstatusPresupuesto", estatus.EstatusPresupuesto);
					ListaEstatus.Add(Opcion);
				}
				#endregion

				CSelectEspecifico ConsultaLevantamientos = new CSelectEspecifico();
                ConsultaLevantamientos.StoredProcedure.CommandText = "sp_Presupuesto_LevantamientoOportunidad";
                ConsultaLevantamientos.StoredProcedure.Parameters.Add("IdOportunidad", SqlDbType.Int).Value = Presupuesto.IdOportunidad;

                ConsultaLevantamientos.Llena(pConexion);

                JArray Levantamientos = new JArray();

                while (ConsultaLevantamientos.Registros.Read())
                {
                    JObject Levantamiento = new JObject();
                    Levantamiento.Add("Valor", Convert.ToInt32(ConsultaLevantamientos.Registros["IdLevantamiento"]));
                    Levantamiento.Add("Descripcion", Convert.ToString(ConsultaLevantamientos.Registros["Descripcion"]));
                    Levantamientos.Add(Levantamiento);
                }

                ConsultaLevantamientos.CerrarConsulta();

				pParametros.Clear();
				pParametros.Add("IdOportunidad", Presupuesto.IdOportunidad);

				Modelo.Add("IdPresupuesto", IdPresupuesto);
				Modelo.Add("IdCliente", IdCliente);
				Modelo.Add("Contactos", ListaContactos);
				Modelo.Add("Direcciones", ListaDirecciones);
				Modelo.Add("Oportunidades", ListaOportunidades);
				Modelo.Add("TipoCambio", Presupuesto.TipoCambio);
				Modelo.Add("SelectedPesos", (Presupuesto.IdTipoMoneda == 1) ? "selected" : "");
				Modelo.Add("SelectedDolares", (Presupuesto.IdTipoMoneda == 2) ? "selected" : "");
                Modelo.Add("Levantamientos", Levantamientos);
				Modelo.Add("Estatus", ListaEstatus);
				Modelo.Add("RazonSocial", RazonSocial);
				Modelo.Add("FechaExpiracion", FechaExpiracion);
				Modelo.Add("Nota", Nota);
                Modelo.Add("Margen", Presupuesto.Margen);
				Modelo.Add("Conceptos", Conceptos);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
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
	public static string BuscarProveedor(string pProveedor)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {

			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_BuscarProveedor_Cotizador";
				Consulta.StoredProcedure.Parameters.Add("@Proveedor", SqlDbType.VarChar, 50).Value = pProveedor;

				Consulta.Llena(pConexion);

				JArray Proveedores = new JArray();

				while (Consulta.Registros.Read())
				{
					JObject Proveedor = new JObject();

					Proveedor.Add("IdProveedor", Convert.ToInt32(Consulta.Registros["IdProveedor"]));
					Proveedor.Add("Proveedor", Convert.ToString(Consulta.Registros["RazonSocial"]));

					Proveedores.Add(Proveedor);
				}

				Modelo.Add("Proveedores", Proveedores);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);

		});

		return Respuesta.ToString();
	}
	
	[WebMethod]
	public static string Imprimir(int IdPresupuesto)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CPresupuesto Presupuesto = new CPresupuesto();
				Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);

				CSucursal Sucursal = new CSucursal();
				Sucursal.LlenaObjeto(Presupuesto.IdSucursal, pConexion);

				CEmpresa Empresa = new CEmpresa();
				Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

				CMunicipio Municipio = new CMunicipio();
				Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);

				CEstado Estado = new CEstado();
				Estado.LlenaObjeto(Municipio.IdEstado, pConexion);

				CPais Pais = new CPais();
				Pais.LlenaObjeto(Estado.IdPais, pConexion);

				CPresupuestoConcepto Conceptos = new CPresupuestoConcepto();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdPresupuesto", Presupuesto.IdPresupuesto);
				pParametros.Add("Baja", 0);
				
				JArray Partidas = new JArray();
				foreach (CPresupuestoConcepto Concepto in Conceptos.LlenaObjetosFiltros(pParametros, pConexion))
				{
					JObject Partida = new JObject();
					Partida.Add("CANTIDADDETALLE", Concepto.Cantidad);
                    Partida.Add("CLAVE", Concepto.Clave);
					Partida.Add("DESCRIPCIONDETALLE", Concepto.Descripcion);
					Partida.Add("PRECIOUNITARIODETALLE", Concepto.PrecioUnitario.ToString("C"));
					Partida.Add("TOTALDETALLE", Concepto.Total.ToString("C"));
					Partidas.Add(Partida);
				}

				CCliente Cliente = new CCliente();
				Cliente.LlenaObjeto(Presupuesto.IdCliente, pConexion);

				COrganizacion Organizacion = new COrganizacion();
				Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

				CDireccionOrganizacion Direccion = new CDireccionOrganizacion();
				pParametros.Clear();
				pParametros.Add("IdOrganizacion", Organizacion.IdOrganizacion);
				pParametros.Add("IdTipoDireccion", 1);
				Direccion.LlenaObjetoFiltros(pParametros, pConexion);

				CMunicipio MunicipioCliente = new CMunicipio();
				MunicipioCliente.LlenaObjeto(Direccion.IdMunicipio, pConexion);

				CEstado EstadoCliente = new CEstado();
				EstadoCliente.LlenaObjeto(MunicipioCliente.IdEstado, pConexion);

				CPais PaisCliente = new CPais();
				PaisCliente.LlenaObjeto(EstadoCliente.IdPais, pConexion);

				CContactoOrganizacion Contacto = new CContactoOrganizacion();
				Contacto.LlenaObjeto(Presupuesto.IdContactoOrganizacion, pConexion);

				CCondicionPago CondicionPagon = new CCondicionPago();
				CondicionPagon.LlenaObjeto(Cliente.IdCondicionPago, pConexion);
				
				CTipoMoneda Moneda = new CTipoMoneda();
				Moneda.LlenaObjeto(Presupuesto.IdTipoMoneda, pConexion);

				CUsuario Agente = new CUsuario();
				Agente.LlenaObjeto(Presupuesto.IdUsuarioAgente, pConexion);

				// Datos
				Modelo.Add("FOLIO", Presupuesto.Folio);
                Modelo.Add("CONTACTO", Contacto.Nombre);
				Modelo.Add("RAZONSOCIALEMISOR", Empresa.Empresa);
				Modelo.Add("RFCEMISOR", Empresa.RFC);
				Modelo.Add("CALLEEMISIOR", Empresa.Calle);
				Modelo.Add("NUMEROEXTERIOREMISOR", Empresa.NumeroExterior);
				Modelo.Add("COLONIAEMISOR", Empresa.Colonia);
				Modelo.Add("CODIGOPOSTALEMISOR", Empresa.CodigoPostal);
				Modelo.Add("MUNICIPIOEMISOR", Municipio.Municipio);
				Modelo.Add("ESTADOEMISOR", Estado.Estado);
				Modelo.Add("IMAGEN_LOGO", Empresa.Logo);
				Modelo.Add("PROYECTO", 0);
				Modelo.Add("FECHAALTA", Presupuesto.FechaExpiracion);
				Modelo.Add("RFCRECEPTOR", Organizacion.RFC);
				Modelo.Add("RAZONSOCIALRECEPTOR", Organizacion.RazonSocial);
				Modelo.Add("CALLERECEPTOR", Direccion.Calle);
				Modelo.Add("NUMEROEXTERIORRECEPTOR", Direccion.NumeroExterior);
				Modelo.Add("REFERENCIARECEPTOR", Direccion.Referencia);
				Modelo.Add("COLONIARECEPTOR", Direccion.Colonia);
				Modelo.Add("CODIGOPOSTALRECEPTOR", Direccion.CodigoPostal);
				Modelo.Add("MUNICIPIORECEPTOR", MunicipioCliente.Municipio);
				Modelo.Add("ESTADORECEPTOR", EstadoCliente.Estado);
				Modelo.Add("PAISRECEPTOR", PaisCliente.Pais);
				Modelo.Add("TELEFONORECEPTOR", Direccion.ConmutadorTelefono);
				Modelo.Add("CONDICIONPAGO", CondicionPagon.CondicionPago);
				Modelo.Add("USUARIOSOLICITO", Agente.Nombre + " " + Agente.ApellidoPaterno + " " + Agente.ApellidoMaterno);
				Modelo.Add("TIPOMONEDA", Moneda.TipoMoneda);
				Modelo.Add("TIPOCAMBIO", Presupuesto.TipoCambio);
				Modelo.Add("Conceptos", Partidas);
				Modelo.Add("SUBTOTALCOTIZACION", Presupuesto.Subtotal.ToString("C"));
				Modelo.Add("PorcentajeIVACotizacion", ((Presupuesto.Total * 100 == 0) ? 0 : 16));
				Modelo.Add("IVACOTIZACION",Presupuesto.IVA.ToString("C"));
				Modelo.Add("TOTALCOTIZACION",Presupuesto.Total.ToString("C"));
				Modelo.Add("CANTIDADTOTALLETRA",Presupuesto.MontoLetra);
				Modelo.Add("NOTA",Presupuesto.Nota);


				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

    [WebMethod]
    public static string ImprimirRequisicion(int IdPresupuesto)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CPresupuesto Presupuesto = new CPresupuesto();
                Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);

                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Presupuesto.IdSucursal, pConexion);

                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

                CMunicipio Municipio = new CMunicipio();
                Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);

                CEstado Estado = new CEstado();
                Estado.LlenaObjeto(Municipio.IdEstado, pConexion);

                CPais Pais = new CPais();
                Pais.LlenaObjeto(Estado.IdPais, pConexion);

                CPresupuestoConcepto Conceptos = new CPresupuestoConcepto();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdPresupuesto", Presupuesto.IdPresupuesto);
                pParametros.Add("Baja", 0);

                JArray Partidas = new JArray();
                foreach (CPresupuestoConcepto Concepto in Conceptos.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Partida = new JObject();
                    Partida.Add("CANTIDADDETALLE", Concepto.Cantidad);
                    Partida.Add("DESCRIPCIONDETALLE", Concepto.Descripcion);
                    Partida.Add("COSTOUNITARIODETALLE", Concepto.Costo.ToString("C"));
                    Partida.Add("TOTALDETALLE", (Convert.ToDecimal(Concepto.Costo) * Convert.ToInt32(Concepto.Cantidad)).ToString("C"));
                    Partidas.Add(Partida);
                }

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(Presupuesto.IdCliente, pConexion);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

                CDireccionOrganizacion Direccion = new CDireccionOrganizacion();
                pParametros.Clear();
                pParametros.Add("IdOrganizacion", Organizacion.IdOrganizacion);
                pParametros.Add("IdTipoDireccion", 1);
                Direccion.LlenaObjetoFiltros(pParametros, pConexion);

                CMunicipio MunicipioCliente = new CMunicipio();
                MunicipioCliente.LlenaObjeto(Direccion.IdMunicipio, pConexion);

                CEstado EstadoCliente = new CEstado();
                EstadoCliente.LlenaObjeto(MunicipioCliente.IdEstado, pConexion);

                CPais PaisCliente = new CPais();
                PaisCliente.LlenaObjeto(EstadoCliente.IdPais, pConexion);

                CContactoOrganizacion Contacto = new CContactoOrganizacion();
                Contacto.LlenaObjeto(Presupuesto.IdContactoOrganizacion, pConexion);

                CCondicionPago CondicionPagon = new CCondicionPago();
                CondicionPagon.LlenaObjeto(Cliente.IdCondicionPago, pConexion);

                CTipoMoneda Moneda = new CTipoMoneda();
                Moneda.LlenaObjeto(Presupuesto.IdTipoMoneda, pConexion);

                CUsuario Agente = new CUsuario();
                Agente.LlenaObjeto(Presupuesto.IdUsuarioAgente, pConexion);

                CProyecto Proyecto = new CProyecto();
                pParametros.Clear();
                pParametros.Add("IdOportunidad", Presupuesto.IdOportunidad);
                Proyecto.LlenaObjetoFiltros(pParametros, pConexion);

                // Datos
                Modelo.Add("FOLIO", Presupuesto.Folio);
                Modelo.Add("RAZONSOCIALEMISOR", Empresa.Empresa);
                Modelo.Add("RFCEMISOR", Empresa.RFC);
                Modelo.Add("CALLEEMISIOR", Empresa.Calle);
                Modelo.Add("NUMEROEXTERIOREMISOR", Empresa.NumeroExterior);
                Modelo.Add("COLONIAEMISOR", Empresa.Colonia);
                Modelo.Add("CODIGOPOSTALEMISOR", Empresa.CodigoPostal);
                Modelo.Add("MUNICIPIOEMISOR", Municipio.Municipio);
                Modelo.Add("ESTADOEMISOR", Estado.Estado);
                Modelo.Add("IMAGEN_LOGO", Empresa.Logo);
                Modelo.Add("PROYECTO", Proyecto.IdProyecto);
                Modelo.Add("NAMEPROYECTO", Proyecto.NombreProyecto);
                Modelo.Add("OPORTUNIDAD", Presupuesto.IdOportunidad);
                Modelo.Add("FECHAALTA", Presupuesto.FechaExpiracion);
                Modelo.Add("RFCRECEPTOR", Organizacion.RFC);
                Modelo.Add("RAZONSOCIALRECEPTOR", Organizacion.RazonSocial);
                Modelo.Add("CALLERECEPTOR", Direccion.Calle);
                Modelo.Add("NUMEROEXTERIORRECEPTOR", Direccion.NumeroExterior);
                Modelo.Add("REFERENCIARECEPTOR", Direccion.Referencia);
                Modelo.Add("COLONIARECEPTOR", Direccion.Colonia);
                Modelo.Add("CODIGOPOSTALRECEPTOR", Direccion.CodigoPostal);
                Modelo.Add("MUNICIPIORECEPTOR", MunicipioCliente.Municipio);
                Modelo.Add("ESTADORECEPTOR", EstadoCliente.Estado);
                Modelo.Add("PAISRECEPTOR", PaisCliente.Pais);
                Modelo.Add("TELEFONORECEPTOR", Direccion.ConmutadorTelefono);
                Modelo.Add("CONDICIONPAGO", CondicionPagon.CondicionPago);
                Modelo.Add("USUARIOSOLICITO", Agente.Nombre + " " + Agente.ApellidoPaterno + " " + Agente.ApellidoMaterno);
                Modelo.Add("TIPOMONEDA", Moneda.TipoMoneda);
                Modelo.Add("TIPOCAMBIO", Presupuesto.TipoCambio);
                Modelo.Add("Conceptos", Partidas);
                Modelo.Add("TOTALREQUISICION", Presupuesto.Costo.ToString("C"));
                Modelo.Add("MARGEN", Presupuesto.Margen);
                Modelo.Add("NOTA", Presupuesto.Nota);


                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerOportunidadesCliente(int IdCliente)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                COportunidad Oportunidades = new COportunidad();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdCliente", IdCliente);
                pParametros.Add("Baja", 0);
                pParametros.Add("Cerrado", 0);

                JArray OportunidadesCliente = new JArray();

                foreach (COportunidad Oportunidad in Oportunidades.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject OportunidadCliente = new JObject();
                    OportunidadCliente.Add("IdOportunidad", Oportunidad.IdOportunidad);
                    OportunidadCliente.Add("Oportunidad", Oportunidad.Oportunidad.ToUpper() + " (" + Oportunidad.IdOportunidad + ")");
                    OportunidadCliente.Add("Margen", Convert.ToInt16(((Oportunidad.Monto - Oportunidad.Costo) / ((Oportunidad.Monto == 0) ? 1 : Oportunidad.Monto)) * 100));
                    OportunidadesCliente.Add(OportunidadCliente);
                }

                Modelo.Add("Oportunidades", OportunidadesCliente);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerDireccionesCliente(int IdCliente)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(IdCliente, pConexion);
                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
                CDireccionOrganizacion DireccionesOranizacion = new CDireccionOrganizacion();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("Baja", 0);
                pParametros.Add("IdOrganizacion", Organizacion.IdOrganizacion);

                JArray Direcciones = new JArray();

                foreach (CDireccionOrganizacion DireccionOrganizacion in DireccionesOranizacion.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Direccion = new JObject();
                    Direccion.Add("IdDireccionOrganizacion", DireccionOrganizacion.IdDireccionOrganizacion);
                    Direccion.Add("Descripcion", DireccionOrganizacion.Descripcion);
                    Direcciones.Add(Direccion);
                }

                Modelo.Add("Direcciones", Direcciones);

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

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
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
	public static string ObtenerContactosCliente(int IdCliente)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CCliente Cliente = new CCliente();
				Cliente.LlenaObjeto(IdCliente, pConexion);

				CContactoOrganizacion Contactos = new CContactoOrganizacion();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdOrganizacion", Cliente.IdOrganizacion);
				pParametros.Add("Baja", 0);

				JArray Lista = new JArray();

				foreach (CContactoOrganizacion Contacto in Contactos.LlenaObjetosFiltros(pParametros, pConexion))
				{
					JObject Opcion = new JObject();

					Opcion.Add("IdContactoOrganizacion", Contacto.IdContactoOrganizacion);
					Opcion.Add("Nombre", Contacto.Nombre);

					Lista.Add(Opcion);
				}

				Modelo.Add("Contactos", Lista);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerTipoCambio(int IdPresupuesto, int IdTipoMoneda)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				decimal tipoCambio = 1;

				CPresupuesto Presupuesto = new CPresupuesto();
				Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);

				CTipoCambio TipoCambio = new CTipoCambio();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("Fecha", DateTime.Today);
				pParametros.Add("IdTipoMonedaOrigen", IdTipoMoneda);
				pParametros.Add("IdTipoMonedaDestino", 1);
				TipoCambio.LlenaObjetoFiltros(pParametros, pConexion);

				tipoCambio = TipoCambio.TipoCambio;

				Modelo.Add("TipoCambio", tipoCambio);

				Respuesta.Add("Modelo", Modelo);

			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string CambiarEstatusPresupuesto(int IdPresupuesto, string Motivo)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				if (UsuarioSesion.TienePermisos(new string[]{ "cambiarEstatusCotizacion" }, pConexion) == "")
				{
					CPresupuesto Presupuesto = new CPresupuesto();
					Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);
					Presupuesto.Baja = !Presupuesto.Baja;
					Presupuesto.MotivoCancelacion = Motivo;
					Presupuesto.Editar(pConexion);
				}
				else
				{
					Error = 1;
					DescripcionError = "No cuenta con los permisos necesarios";
				}
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string GurardarPresupuesto(Dictionary<string, object> pPresupuesto)
	{
		JObject Respuesta = new JObject();

		var pre = new CPresupuestoConcepto().GetType();
		var prop = pre.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
		Respuesta.Add("Clase", JArray.FromObject(prop.Select(x => x.Name).ToArray()));

		///*
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				int IdPresupuesto = 0;
				int IdCliente = 0;
				int IdOportunidad = 0;
				int IdContactoOrganizacion = 0;
				int IdDireccionOrganizacino = 0;
				int IdEstatusPresupuesto = 0;
				int IdTipoMoneda = 0;
				string MontoLetra = "";
				string Nota = "";
				DateTime FechaExpiracion = new DateTime().AddDays(7);
				decimal Subtotal = 0;
				decimal Descuento = 0;
				decimal IVA = 0;
				decimal Total = 0;
				decimal Costo = 0;
				decimal ManoObra = 0;
				decimal Utilidad = 0;
				decimal TipoCambio = 1;

				try
				{
					IdPresupuesto = Convert.ToInt32(pPresupuesto["IdPresupuesto"]);
					IdCliente = Convert.ToInt32(pPresupuesto["IdCliente"]);
					IdOportunidad = Convert.ToInt32(pPresupuesto["IdOportunidad"]);
					IdContactoOrganizacion = Convert.ToInt32(pPresupuesto["IdContactoOrganizacion"]);
					IdDireccionOrganizacino = Convert.ToInt32(pPresupuesto["IdDireccionOrganizacion"]);
					IdEstatusPresupuesto = Convert.ToInt32(pPresupuesto["IdEstatusPresupuesto"]);
					IdTipoMoneda = Convert.ToInt32(pPresupuesto["IdTipoMoneda"]);
					MontoLetra = Convert.ToString(pPresupuesto["MontoLetra"]);
					Nota = Convert.ToString(pPresupuesto["Nota"]);
					string[] Fecha = Convert.ToString(pPresupuesto["FechaExpiracion"]).Split('/');
					FechaExpiracion = new DateTime(Convert.ToInt32(Fecha[2]),Convert.ToInt32(Fecha[1]),Convert.ToInt32(Fecha[0]));
					Subtotal = Convert.ToDecimal(pPresupuesto["Subtotal"]);
					Descuento = Convert.ToDecimal(pPresupuesto["Descuento"]);
					IVA = Convert.ToDecimal(pPresupuesto["IVA"]);
					Total = Convert.ToDecimal(pPresupuesto["Total"]);
					Costo = Convert.ToDecimal(pPresupuesto["Costo"]);
					ManoObra = Convert.ToDecimal(pPresupuesto["ManoObra"]);
					Utilidad = Convert.ToDecimal(pPresupuesto["Utilidad"]);
					TipoCambio = Convert.ToDecimal(pPresupuesto["TipoCambio"]);

				} catch (Exception e)
				{
					Error = 1;
					DescripcionError = e.Message + " - " + e.StackTrace;
				}

				if (Error == 0)
				{
					CPresupuesto Presupuesto = new CPresupuesto();
					Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);
					Presupuesto.Subtotal = Subtotal;
					Presupuesto.Descuento = Descuento;
					Presupuesto.IVA = IVA;
					Presupuesto.Total = Total;
					Presupuesto.Costo = Costo;
					Presupuesto.ManoObra = ManoObra;
					Presupuesto.Utilidad = Utilidad;
					Presupuesto.IdCliente = IdCliente;
					Presupuesto.IdOportunidad = IdOportunidad;
					Presupuesto.IdSucursal = UsuarioSesion.IdSucursalActual;
					Presupuesto.IdUsuarioAgente = (Presupuesto.IdUsuarioAgente == 0) ? UsuarioSesion.IdUsuario : Presupuesto.IdUsuarioAgente;
					Presupuesto.FechaExpiracion = FechaExpiracion;
					Presupuesto.IdTipoMoneda = IdTipoMoneda;
					Presupuesto.IdContactoOrganizacion = IdContactoOrganizacion;
					Presupuesto.IdDireccionOrganizacion = IdDireccionOrganizacino;
					Presupuesto.IdEstatusPresupuesto = IdEstatusPresupuesto;
					Presupuesto.TipoCambio = TipoCambio;
					Presupuesto.Nota = Nota;
					Presupuesto.MontoLetra = MontoLetra;
                    Presupuesto.Margen = 0;
					if (Presupuesto.IdPresupuesto == 0)
					{
						//Presupuesto.IdEstatusPresupuesto = 1;
						Presupuesto.Agregar(pConexion);
						CPresupuesto ObtenerFolio = new CPresupuesto();
						Presupuesto.TipoCambio = TipoCambio;
						Dictionary<string, object> pParametros = new Dictionary<string, object>();
						pParametros.Add("IdSucursal", UsuarioSesion.IdSucursalActual);
						Presupuesto.Folio = ObtenerFolio.LlenaObjetos(pConexion).Count;
						Presupuesto.Editar(pConexion);
						IdPresupuesto = Presupuesto.IdPresupuesto;
					}
					else
					{
						Presupuesto.FechaUltimaModificacion = DateTime.Now;
						Presupuesto.Editar(pConexion);
					}

					//
					GuardarTipoCambioPresupuesto(Presupuesto.IdPresupuesto, pConexion);

					// Guardar contacto (hay que cambiarlo despues si se requieren multiples contactos
					CPresupuestoContactoOrganizacion Contacto = new CPresupuestoContactoOrganizacion();
					Contacto.IdContactoOrganizacion = IdContactoOrganizacion;
					Contacto.IdPresupuesto = Presupuesto.IdPresupuesto;
					Contacto.Baja = false;
					Contacto.IdUsuario = UsuarioSesion.IdUsuario;
					
					Object[] Conceptos = (Object[])pPresupuesto["Conceptos"];

					if (Conceptos.Length > 0)
					{
						int orden = 1;
						foreach (Dictionary<string, object> Concepto in Conceptos)
						{
							CPresupuestoConcepto pConcepto = new CPresupuestoConcepto();
							pConcepto.IdPresupuestoConcepto = Convert.ToInt32(Concepto["IdPropuestaConcepto"]);

							
							pConcepto.IdPresupuesto = Presupuesto.IdPresupuesto;
							pConcepto.Cantidad = Convert.ToDecimal(Concepto["Cantidad"]);
                            pConcepto.FacturacionCantidad = Convert.ToDecimal(Concepto["Cantidad"]);
							pConcepto.Orden = orden;
							pConcepto.Clave = Convert.ToString(Concepto["Clave"]);
                            pConcepto.IdProducto = Convert.ToInt32(Concepto["IdProducto"]);
                            pConcepto.IdServicio = Convert.ToInt32(Concepto["IdServicio"]);
                            pConcepto.Proveedor = Convert.ToString(Concepto["Proveedor"]);
							pConcepto.Costo = Convert.ToDecimal(Concepto["CostoUnitario"]);
							pConcepto.Descuento = Convert.ToInt32(Concepto["Descuento"]);
							pConcepto.ManoObra = Convert.ToDecimal(Concepto["ManoObra"]);
							pConcepto.PrecioUnitario = Convert.ToDecimal(Concepto["PrecioUnitario"]);
							pConcepto.Descripcion = Convert.ToString(Concepto["Descripcion"]);
							pConcepto.IdDivision = Convert.ToInt32(Concepto["IdDivision"]);
							pConcepto.Margen = Convert.ToDecimal(Concepto["Margen"]);
							pConcepto.Descuento = Convert.ToDecimal(Concepto["Descuento"]);
							pConcepto.Total = Convert.ToDecimal(Concepto["Total"]);
							pConcepto.IVA = Convert.ToDecimal(Concepto["IVA"]);
                            Presupuesto.Margen += pConcepto.Margen;

                            if (pConcepto.IdPresupuestoConcepto != 0)
							{
								pConcepto.Editar(pConexion);
							}
							else
                            {
                                pConcepto.FacturacionCantidad = Convert.ToDecimal(Concepto["Cantidad"]);
                                pConcepto.Agregar(pConexion);
							}
							orden++;
						}
					}
                    CPresupuesto PresupuestoMargen = new CPresupuesto();
                    PresupuestoMargen.LlenaObjeto(IdPresupuesto, pConexion);
                    PresupuestoMargen.Margen = Presupuesto.Margen / Conceptos.Length;
                    PresupuestoMargen.Editar(pConexion);

                    Modelo.Add("IdPresupuesto", IdPresupuesto);

					Respuesta.Add("Modelo", Modelo);
				}
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		//*/
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string EliminarConcepto(int IdConcepto)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CPresupuestoConcepto Concepto = new CPresupuestoConcepto();
				Concepto.LlenaObjeto(IdConcepto, pConexion);

				Concepto.Baja = true;
				Concepto.Editar(pConexion);

				Modelo.Add("Mensaje", "El concepto ha sido eliminado");

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string EliminarConceptos(int IdPresupuesto)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CPresupuesto Presupuesto = new CPresupuesto();
				Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);

				CPresupuestoConcepto Conceptos = new CPresupuestoConcepto();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdPresupuesto", pParametros);
				
				foreach(CPresupuestoConcepto Concepto in Conceptos.LlenaObjetosFiltros(pParametros, pConexion))
				{
					Concepto.Baja = true;
					Concepto.Editar(pConexion);
				}

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	private static void GuardarTipoCambioPresupuesto(int IdPresupuesto, CConexion pConexion)
	{
		CPresupuesto Presupuesto = new CPresupuesto();
		Presupuesto.LlenaObjeto(IdPresupuesto, pConexion);

		CTipoCambioPresupuesto ValidarTipoCambio = new CTipoCambioPresupuesto();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdPresupuesto", IdPresupuesto);

		if (ValidarTipoCambio.LlenaObjetosFiltros(pParametros, pConexion).Count == 0)
		{
			pParametros.Clear();
			CTipoCambio TipoCambios = new CTipoCambio();
			pParametros.Add("Fecha", DateTime.Today);
			foreach (CTipoCambio TipoCambio in TipoCambios.LlenaObjetosFiltros(pParametros, pConexion))
			{
				CTipoCambioPresupuesto TipoCambioPresupuesto = new CTipoCambioPresupuesto();
				TipoCambioPresupuesto.IdPresupuesto = IdPresupuesto;
				TipoCambioPresupuesto.TipoCambio = TipoCambio.TipoCambio;
				TipoCambioPresupuesto.IdTipoMonedaOrigen = TipoCambio.IdTipoMonedaOrigen;
				TipoCambioPresupuesto.IdTipoMonedaDestino = TipoCambio.IdTipoMonedaDestino;
				TipoCambioPresupuesto.Fecha = TipoCambio.Fecha;
				TipoCambioPresupuesto.Agregar(pConexion);
			}

			CTipoCambioPresupuesto tcPresupuesto = new CTipoCambioPresupuesto();
			pParametros.Add("IdTipoMonedaOrigen", Presupuesto.IdTipoMoneda);
			pParametros.Add("IdTipoMonedaDestino", 1);
			tcPresupuesto.LlenaObjetoFiltros(pParametros, pConexion);

			Presupuesto.TipoCambio = tcPresupuesto.TipoCambio;
			Presupuesto.Editar(pConexion);
		}
	}

	private static Dictionary<string, object> AgregarConcepto(List<Dictionary<string,object>> Conceptos, CConexion pConexion)
	{
		Dictionary<string, object> completado = new Dictionary<string, object>();
		int Error = 1;
		string DescripcionError = "";
		foreach (Dictionary<string, object> Concepto in Conceptos)
		{
			Error = 1;
			try
			{
				int IdPropuestaConcepto = Convert.ToInt32(Concepto["IdPropuestaConcepto"]);
				int IdPresupuesto = Convert.ToInt32(Concepto["IdPresupuesto"]);
				int Orden = Convert.ToInt32(Concepto["Orden"]);
				string Clave = Convert.ToString(Concepto["Clave"]);
				string Descripcion = Convert.ToString(Concepto["Descripcion"]);
				string Proveedor = Convert.ToString(Concepto["Proveedor"]);
				decimal CostoUnitario = Convert.ToDecimal(Concepto["CostoUnitario"]);
				decimal ManoObra = Convert.ToDecimal(Concepto["ManoObra"]);
				decimal PrecioUnitario = Convert.ToDecimal(Concepto["PrecioUnitario"]);
				decimal Descuento = Convert.ToDecimal(Concepto["Descuento"]);
				decimal Cantidad = Convert.ToDecimal(Concepto["Cantidad"]);
				decimal Margen = Convert.ToDecimal(Concepto["Margen"]);
				decimal IVA = Convert.ToDecimal(Concepto["IVA"]);
				decimal Total = Convert.ToDecimal(Concepto["Total"]);
				decimal Utilidad = Convert.ToDecimal(Concepto["Utilidad"]);

				CPresupuestoConcepto PresupuestoConcepto = new CPresupuestoConcepto();
				PresupuestoConcepto.LlenaObjeto(IdPropuestaConcepto, pConexion);

				PresupuestoConcepto.IdPresupuesto = IdPresupuesto;
				PresupuestoConcepto.Orden = Orden;
				PresupuestoConcepto.Clave = Clave;
				PresupuestoConcepto.Descripcion = Descripcion;
				PresupuestoConcepto.Proveedor = Proveedor;
				PresupuestoConcepto.Costo = CostoUnitario;
				PresupuestoConcepto.ManoObra = ManoObra;
				PresupuestoConcepto.Margen = Margen;
				PresupuestoConcepto.PrecioUnitario = PrecioUnitario;
				PresupuestoConcepto.Descuento = Descuento;
				PresupuestoConcepto.Cantidad = Cantidad;
				PresupuestoConcepto.Total = Total;
				PresupuestoConcepto.Utilidad = Utilidad;

				if (PresupuestoConcepto.IdPresupuestoConcepto == 0)
				{
					PresupuestoConcepto.Agregar(pConexion);
				}
				else
				{
					PresupuestoConcepto.Editar(pConexion);
				}

				Error = 0;
			}
			catch (Exception e)
			{
				Error = 1;
				DescripcionError = e.Message + " - " + e.StackTrace;
			}
		}
		completado.Add("Error", Error);
		completado.Add("Descripcion", DescripcionError);
		return completado;
	}

	private static bool ValidarPresupuesto()
	{
		bool valido = false;



		return valido;
	}

	[WebMethod]
	public static string ObtenerSucursales()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSucursal SucursalActural = new CSucursal();
				SucursalActural.LlenaObjeto(UsuarioSesion.IdSucursalActual, pConexion);

				Modelo.Add("Sucursales", CSucursal.ObtenerSucursalesEmpresa(pConexion));

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerConceptoClave (string Clave, int IdTipoMoneda)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_ConceptClave";
				Consulta.StoredProcedure.Parameters.Add("Clave", SqlDbType.VarChar, 250).Value = Clave;
				Consulta.StoredProcedure.Parameters.Add("IdTipoMoneda", SqlDbType.Int).Value = IdTipoMoneda;
				Consulta.Llena(pConexion);

				JArray Conceptos = new JArray();

				while (Consulta.Registros.Read())
				{
					JObject Concepto = new JObject();
                    Concepto.Add("IdProducto", Convert.ToInt32(Consulta.Registros["IdProducto"]));
                    Concepto.Add("IdServicio", Convert.ToInt32(Consulta.Registros["IdServicio"]));
                    Concepto.Add("Clave", Convert.ToString(Consulta.Registros["Clave"]));
					Concepto.Add("Descripcion", Convert.ToString(Consulta.Registros["Descripcion"]));
					Concepto.Add("Costo", Convert.ToDecimal(Consulta.Registros["Costo"]).ToString("C"));
					Conceptos.Add(Concepto);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Conceptos", Conceptos);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});

		return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerConceptoDescripcion(string Descripcion, int IdTipoMoneda)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_ConceptDescripcion";
                Consulta.StoredProcedure.Parameters.Add("Descripcion", SqlDbType.VarChar, 250).Value = Descripcion;
                Consulta.StoredProcedure.Parameters.Add("IdTipoMoneda", SqlDbType.Int).Value = IdTipoMoneda;
                Consulta.Llena(pConexion);

                JArray Conceptos = new JArray();

                while (Consulta.Registros.Read())
                {
                    JObject Concepto = new JObject();
                    Concepto.Add("IdProducto", Convert.ToInt32(Consulta.Registros["IdProducto"]));
                    Concepto.Add("IdServicio", Convert.ToInt32(Consulta.Registros["IdServicio"]));
                    Concepto.Add("Clave", Convert.ToString(Consulta.Registros["Clave"]));
                    Concepto.Add("Descripcion", Convert.ToString(Consulta.Registros["Descripcion"]));
                    Concepto.Add("Costo", Convert.ToDecimal(Consulta.Registros["Costo"]).ToString("C"));
                    Conceptos.Add(Concepto);
                }

                Consulta.CerrarConsulta();

                Modelo.Add("Conceptos", Conceptos);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string BuscarOportunidad(string Oportunidad)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_Cotizador_BuscarOportunidad";
                Consulta.StoredProcedure.Parameters.Add("Oportunidad", SqlDbType.VarChar, 30).Value = Oportunidad;

                Consulta.Llena(pConexion);

                JArray Oportunidades = new JArray();

                while (Consulta.Registros.Read())
                {
                    JObject Registro = new JObject();
                    Registro.Add("IdOportunidad", Convert.ToInt32(Consulta.Registros["IdOportunidad"]));
                    Registro.Add("Oportunidad", Convert.ToString(Consulta.Registros["Oportunidad"]));
                    Registro.Add("IdCliente", Convert.ToInt32(Consulta.Registros["IdCliente"]));
                    Registro.Add("Cliente", Convert.ToString(Consulta.Registros["Cliente"]));
                    Oportunidades.Add(Registro);
                }

                Consulta.CerrarConsulta();

                Modelo.Add("Oportunidades", Oportunidades);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

}
