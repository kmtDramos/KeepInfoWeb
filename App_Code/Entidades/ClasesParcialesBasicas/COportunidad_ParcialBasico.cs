using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class COportunidad
{
	//Propiedades Privadas
	private int idOportunidad;
	private string oportunidad;
	private DateTime fechaCreacion;
	private int idUsuarioCreacion;
	private int idNivelInteresOportunidad;
	private decimal monto;
	private int idCliente;
	private string motivoCancelacion;
	private int idSucursal;
	private string archivo;
	private decimal cotizaciones;
	private decimal pedidos;
	private decimal proyectos;
	private decimal facturas;
	private decimal neto;
	private int idDivision;
	private bool clasificacion;
	private bool facturado;
	private bool cerrado;
	private string ultimaNota;
	private DateTime fechaNota;
	private bool esProyecto;
	private bool urgente;
	private DateTime fechaCierre;
	private string proveedores;
	private decimal mes1;
	private decimal mes2;
	private decimal mes3;
	private DateTime terminadoPreventa;
	private DateTime terminadoVentas;
	private DateTime terminadoCompras;
	private DateTime terminadoProyectos;
	private DateTime terminadoFinanzas;
	private int idCampana;
	private bool preventaDetenido;
	private bool ventasDetenido;
	private bool comprasDetenido;
	private bool proyectosDetenido;
	private bool finzanzasDetenido;
	private string estatusPlan;
	private int idUsuarioEstatusPlan;
	private DateTime fechaEstatusPlan;
	private decimal pagos;
	private decimal costo;
	private decimal ordenCompra;
	private decimal notaCreditoProveedor;
	private decimal utilidad;
	private DateTime compromisoPreventa;
	private DateTime compromisoVentas;
	private DateTime compromisoCompras;
	private DateTime compromisoProyectos;
	private DateTime compromisoFinanzas;
	private bool autorizado;
	private int idEstatusCompras;
	private bool baja;
	
	//Propiedades
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
		}
	}
	
	public string Oportunidad
	{
		get { return oportunidad; }
		set
		{
			oportunidad = value;
		}
	}
	
	public DateTime FechaCreacion
	{
		get { return fechaCreacion; }
		set { fechaCreacion = value; }
	}
	
	public int IdUsuarioCreacion
	{
		get { return idUsuarioCreacion; }
		set
		{
			idUsuarioCreacion = value;
		}
	}
	
	public int IdNivelInteresOportunidad
	{
		get { return idNivelInteresOportunidad; }
		set
		{
			idNivelInteresOportunidad = value;
		}
	}
	
	public decimal Monto
	{
		get { return monto; }
		set
		{
			monto = value;
		}
	}
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			idCliente = value;
		}
	}
	
	public string MotivoCancelacion
	{
		get { return motivoCancelacion; }
		set
		{
			motivoCancelacion = value;
		}
	}
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			idSucursal = value;
		}
	}
	
	public string Archivo
	{
		get { return archivo; }
		set
		{
			archivo = value;
		}
	}
	
	public decimal Cotizaciones
	{
		get { return cotizaciones; }
		set
		{
			cotizaciones = value;
		}
	}
	
	public decimal Pedidos
	{
		get { return pedidos; }
		set
		{
			pedidos = value;
		}
	}
	
	public decimal Proyectos
	{
		get { return proyectos; }
		set
		{
			proyectos = value;
		}
	}
	
	public decimal Facturas
	{
		get { return facturas; }
		set
		{
			facturas = value;
		}
	}
	
	public decimal Neto
	{
		get { return neto; }
		set
		{
			neto = value;
		}
	}
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
		}
	}
	
	public bool Clasificacion
	{
		get { return clasificacion; }
		set { clasificacion = value; }
	}
	
	public bool Facturado
	{
		get { return facturado; }
		set { facturado = value; }
	}
	
	public bool Cerrado
	{
		get { return cerrado; }
		set { cerrado = value; }
	}
	
	public string UltimaNota
	{
		get { return ultimaNota; }
		set
		{
			ultimaNota = value;
		}
	}
	
	public DateTime FechaNota
	{
		get { return fechaNota; }
		set { fechaNota = value; }
	}
	
	public bool EsProyecto
	{
		get { return esProyecto; }
		set { esProyecto = value; }
	}
	
	public bool Urgente
	{
		get { return urgente; }
		set { urgente = value; }
	}
	
	public DateTime FechaCierre
	{
		get { return fechaCierre; }
		set { fechaCierre = value; }
	}
	
	public string Proveedores
	{
		get { return proveedores; }
		set
		{
			proveedores = value;
		}
	}
	
	public decimal Mes1
	{
		get { return mes1; }
		set
		{
			mes1 = value;
		}
	}
	
	public decimal Mes2
	{
		get { return mes2; }
		set
		{
			mes2 = value;
		}
	}
	
	public decimal Mes3
	{
		get { return mes3; }
		set
		{
			mes3 = value;
		}
	}
	
	public DateTime TerminadoPreventa
	{
		get { return terminadoPreventa; }
		set { terminadoPreventa = value; }
	}
	
	public DateTime TerminadoVentas
	{
		get { return terminadoVentas; }
		set { terminadoVentas = value; }
	}
	
	public DateTime TerminadoCompras
	{
		get { return terminadoCompras; }
		set { terminadoCompras = value; }
	}
	
	public DateTime TerminadoProyectos
	{
		get { return terminadoProyectos; }
		set { terminadoProyectos = value; }
	}
	
	public DateTime TerminadoFinanzas
	{
		get { return terminadoFinanzas; }
		set { terminadoFinanzas = value; }
	}
	
	public int IdCampana
	{
		get { return idCampana; }
		set
		{
			idCampana = value;
		}
	}
	
	public bool PreventaDetenido
	{
		get { return preventaDetenido; }
		set { preventaDetenido = value; }
	}
	
	public bool VentasDetenido
	{
		get { return ventasDetenido; }
		set { ventasDetenido = value; }
	}
	
	public bool ComprasDetenido
	{
		get { return comprasDetenido; }
		set { comprasDetenido = value; }
	}
	
	public bool ProyectosDetenido
	{
		get { return proyectosDetenido; }
		set { proyectosDetenido = value; }
	}
	
	public bool FinzanzasDetenido
	{
		get { return finzanzasDetenido; }
		set { finzanzasDetenido = value; }
	}
	
	public string EstatusPlan
	{
		get { return estatusPlan; }
		set
		{
			estatusPlan = value;
		}
	}
	
	public int IdUsuarioEstatusPlan
	{
		get { return idUsuarioEstatusPlan; }
		set
		{
			idUsuarioEstatusPlan = value;
		}
	}
	
	public DateTime FechaEstatusPlan
	{
		get { return fechaEstatusPlan; }
		set { fechaEstatusPlan = value; }
	}
	
	public decimal Pagos
	{
		get { return pagos; }
		set
		{
			pagos = value;
		}
	}
	
	public decimal Costo
	{
		get { return costo; }
		set
		{
			costo = value;
		}
	}
	
	public decimal OrdenCompra
	{
		get { return ordenCompra; }
		set
		{
			ordenCompra = value;
		}
	}
	
	public decimal NotaCreditoProveedor
	{
		get { return notaCreditoProveedor; }
		set
		{
			notaCreditoProveedor = value;
		}
	}
	
	public decimal Utilidad
	{
		get { return utilidad; }
		set
		{
			utilidad = value;
		}
	}
	
	public DateTime CompromisoPreventa
	{
		get { return compromisoPreventa; }
		set { compromisoPreventa = value; }
	}
	
	public DateTime CompromisoVentas
	{
		get { return compromisoVentas; }
		set { compromisoVentas = value; }
	}
	
	public DateTime CompromisoCompras
	{
		get { return compromisoCompras; }
		set { compromisoCompras = value; }
	}
	
	public DateTime CompromisoProyectos
	{
		get { return compromisoProyectos; }
		set { compromisoProyectos = value; }
	}
	
	public DateTime CompromisoFinanzas
	{
		get { return compromisoFinanzas; }
		set { compromisoFinanzas = value; }
	}
	
	public bool Autorizado
	{
		get { return autorizado; }
		set { autorizado = value; }
	}
	
	public int IdEstatusCompras
	{
		get { return idEstatusCompras; }
		set
		{
			idEstatusCompras = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public COportunidad()
	{
		idOportunidad = 0;
		oportunidad = "";
		fechaCreacion = new DateTime(1, 1, 1);
		idUsuarioCreacion = 0;
		idNivelInteresOportunidad = 0;
		monto = 0;
		idCliente = 0;
		motivoCancelacion = "";
		idSucursal = 0;
		archivo = "";
		cotizaciones = 0;
		pedidos = 0;
		proyectos = 0;
		facturas = 0;
		neto = 0;
		idDivision = 0;
		clasificacion = false;
		facturado = false;
		cerrado = false;
		ultimaNota = "";
		fechaNota = new DateTime(1, 1, 1);
		esProyecto = false;
		urgente = false;
		fechaCierre = new DateTime(1, 1, 1);
		proveedores = "";
		mes1 = 0;
		mes2 = 0;
		mes3 = 0;
		terminadoPreventa = new DateTime(1, 1, 1);
		terminadoVentas = new DateTime(1, 1, 1);
		terminadoCompras = new DateTime(1, 1, 1);
		terminadoProyectos = new DateTime(1, 1, 1);
		terminadoFinanzas = new DateTime(1, 1, 1);
		idCampana = 0;
		preventaDetenido = false;
		ventasDetenido = false;
		comprasDetenido = false;
		proyectosDetenido = false;
		finzanzasDetenido = false;
		estatusPlan = "";
		idUsuarioEstatusPlan = 0;
		fechaEstatusPlan = new DateTime(1, 1, 1);
		pagos = 0;
		costo = 0;
		ordenCompra = 0;
		notaCreditoProveedor = 0;
		utilidad = 0;
		compromisoPreventa = new DateTime(1, 1, 1);
		compromisoVentas = new DateTime(1, 1, 1);
		compromisoCompras = new DateTime(1, 1, 1);
		compromisoProyectos = new DateTime(1, 1, 1);
		compromisoFinanzas = new DateTime(1, 1, 1);
		autorizado = false;
		idEstatusCompras = 0;
		baja = false;
	}
	
	public COportunidad(int pIdOportunidad)
	{
		idOportunidad = pIdOportunidad;
		oportunidad = "";
		fechaCreacion = new DateTime(1, 1, 1);
		idUsuarioCreacion = 0;
		idNivelInteresOportunidad = 0;
		monto = 0;
		idCliente = 0;
		motivoCancelacion = "";
		idSucursal = 0;
		archivo = "";
		cotizaciones = 0;
		pedidos = 0;
		proyectos = 0;
		facturas = 0;
		neto = 0;
		idDivision = 0;
		clasificacion = false;
		facturado = false;
		cerrado = false;
		ultimaNota = "";
		fechaNota = new DateTime(1, 1, 1);
		esProyecto = false;
		urgente = false;
		fechaCierre = new DateTime(1, 1, 1);
		proveedores = "";
		mes1 = 0;
		mes2 = 0;
		mes3 = 0;
		terminadoPreventa = new DateTime(1, 1, 1);
		terminadoVentas = new DateTime(1, 1, 1);
		terminadoCompras = new DateTime(1, 1, 1);
		terminadoProyectos = new DateTime(1, 1, 1);
		terminadoFinanzas = new DateTime(1, 1, 1);
		idCampana = 0;
		preventaDetenido = false;
		ventasDetenido = false;
		comprasDetenido = false;
		proyectosDetenido = false;
		finzanzasDetenido = false;
		estatusPlan = "";
		idUsuarioEstatusPlan = 0;
		fechaEstatusPlan = new DateTime(1, 1, 1);
		pagos = 0;
		costo = 0;
		ordenCompra = 0;
		notaCreditoProveedor = 0;
		utilidad = 0;
		compromisoPreventa = new DateTime(1, 1, 1);
		compromisoVentas = new DateTime(1, 1, 1);
		compromisoCompras = new DateTime(1, 1, 1);
		compromisoProyectos = new DateTime(1, 1, 1);
		compromisoFinanzas = new DateTime(1, 1, 1);
		autorizado = false;
		idEstatusCompras = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Oportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COportunidad>(typeof(COportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Oportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COportunidad>(typeof(COportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Oportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COportunidad>(typeof(COportunidad), pConexion);
		foreach (COportunidad O in Obten.ListaRegistros)
		{
			idOportunidad = O.IdOportunidad;
			oportunidad = O.Oportunidad;
			fechaCreacion = O.FechaCreacion;
			idUsuarioCreacion = O.IdUsuarioCreacion;
			idNivelInteresOportunidad = O.IdNivelInteresOportunidad;
			monto = O.Monto;
			idCliente = O.IdCliente;
			motivoCancelacion = O.MotivoCancelacion;
			idSucursal = O.IdSucursal;
			archivo = O.Archivo;
			cotizaciones = O.Cotizaciones;
			pedidos = O.Pedidos;
			proyectos = O.Proyectos;
			facturas = O.Facturas;
			neto = O.Neto;
			idDivision = O.IdDivision;
			clasificacion = O.Clasificacion;
			facturado = O.Facturado;
			cerrado = O.Cerrado;
			ultimaNota = O.UltimaNota;
			fechaNota = O.FechaNota;
			esProyecto = O.EsProyecto;
			urgente = O.Urgente;
			fechaCierre = O.FechaCierre;
			proveedores = O.Proveedores;
			mes1 = O.Mes1;
			mes2 = O.Mes2;
			mes3 = O.Mes3;
			terminadoPreventa = O.TerminadoPreventa;
			terminadoVentas = O.TerminadoVentas;
			terminadoCompras = O.TerminadoCompras;
			terminadoProyectos = O.TerminadoProyectos;
			terminadoFinanzas = O.TerminadoFinanzas;
			idCampana = O.IdCampana;
			preventaDetenido = O.PreventaDetenido;
			ventasDetenido = O.VentasDetenido;
			comprasDetenido = O.ComprasDetenido;
			proyectosDetenido = O.ProyectosDetenido;
			finzanzasDetenido = O.FinzanzasDetenido;
			estatusPlan = O.EstatusPlan;
			idUsuarioEstatusPlan = O.IdUsuarioEstatusPlan;
			fechaEstatusPlan = O.FechaEstatusPlan;
			pagos = O.Pagos;
			costo = O.Costo;
			ordenCompra = O.OrdenCompra;
			notaCreditoProveedor = O.NotaCreditoProveedor;
			utilidad = O.Utilidad;
			compromisoPreventa = O.CompromisoPreventa;
			compromisoVentas = O.CompromisoVentas;
			compromisoCompras = O.CompromisoCompras;
			compromisoProyectos = O.CompromisoProyectos;
			compromisoFinanzas = O.CompromisoFinanzas;
			autorizado = O.Autorizado;
			idEstatusCompras = O.IdEstatusCompras;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Oportunidad_ConsultarFiltros";
		foreach (KeyValuePair<string, object> parametro in pParametros)
		{
			if (parametro.Key == "Opcion")
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@"+parametro.Key, parametro.Value);
			}
			else
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@p"+parametro.Key, parametro.Value);
			}
		}
		Obten.Llena<COportunidad>(typeof(COportunidad), pConexion);
		foreach (COportunidad O in Obten.ListaRegistros)
		{
			idOportunidad = O.IdOportunidad;
			oportunidad = O.Oportunidad;
			fechaCreacion = O.FechaCreacion;
			idUsuarioCreacion = O.IdUsuarioCreacion;
			idNivelInteresOportunidad = O.IdNivelInteresOportunidad;
			monto = O.Monto;
			idCliente = O.IdCliente;
			motivoCancelacion = O.MotivoCancelacion;
			idSucursal = O.IdSucursal;
			archivo = O.Archivo;
			cotizaciones = O.Cotizaciones;
			pedidos = O.Pedidos;
			proyectos = O.Proyectos;
			facturas = O.Facturas;
			neto = O.Neto;
			idDivision = O.IdDivision;
			clasificacion = O.Clasificacion;
			facturado = O.Facturado;
			cerrado = O.Cerrado;
			ultimaNota = O.UltimaNota;
			fechaNota = O.FechaNota;
			esProyecto = O.EsProyecto;
			urgente = O.Urgente;
			fechaCierre = O.FechaCierre;
			proveedores = O.Proveedores;
			mes1 = O.Mes1;
			mes2 = O.Mes2;
			mes3 = O.Mes3;
			terminadoPreventa = O.TerminadoPreventa;
			terminadoVentas = O.TerminadoVentas;
			terminadoCompras = O.TerminadoCompras;
			terminadoProyectos = O.TerminadoProyectos;
			terminadoFinanzas = O.TerminadoFinanzas;
			idCampana = O.IdCampana;
			preventaDetenido = O.PreventaDetenido;
			ventasDetenido = O.VentasDetenido;
			comprasDetenido = O.ComprasDetenido;
			proyectosDetenido = O.ProyectosDetenido;
			finzanzasDetenido = O.FinzanzasDetenido;
			estatusPlan = O.EstatusPlan;
			idUsuarioEstatusPlan = O.IdUsuarioEstatusPlan;
			fechaEstatusPlan = O.FechaEstatusPlan;
			pagos = O.Pagos;
			costo = O.Costo;
			ordenCompra = O.OrdenCompra;
			notaCreditoProveedor = O.NotaCreditoProveedor;
			utilidad = O.Utilidad;
			compromisoPreventa = O.CompromisoPreventa;
			compromisoVentas = O.CompromisoVentas;
			compromisoCompras = O.CompromisoCompras;
			compromisoProyectos = O.CompromisoProyectos;
			compromisoFinanzas = O.CompromisoFinanzas;
			autorizado = O.Autorizado;
			idEstatusCompras = O.IdEstatusCompras;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Oportunidad_ConsultarFiltros";
		foreach (KeyValuePair<string, object> parametro in pParametros)
		{
			if (parametro.Key == "Opcion")
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@"+parametro.Key, parametro.Value);
			}
			else
			{
				Obten.StoredProcedure.Parameters.AddWithValue("@p"+parametro.Key, parametro.Value);
			}
		}
		Obten.Llena<COportunidad>(typeof(COportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Oportunidad_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", 0);
		Agregar.StoredProcedure.Parameters["@pIdOportunidad"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOportunidad", oportunidad);
		if(fechaCreacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreacion", idUsuarioCreacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresOportunidad", idNivelInteresOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pArchivo", archivo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCotizaciones", cotizaciones);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPedidos", pedidos);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProyectos", proyectos);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturas", facturas);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNeto", neto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClasificacion", clasificacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCerrado", cerrado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUltimaNota", ultimaNota);
		if(fechaNota.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaNota", fechaNota);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsProyecto", esProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUrgente", urgente);
		if(fechaCierre.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCierre", fechaCierre);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProveedores", proveedores);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMes1", mes1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMes2", mes2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMes3", mes3);
		if(terminadoPreventa.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pTerminadoPreventa", terminadoPreventa);
		}
		if(terminadoVentas.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pTerminadoVentas", terminadoVentas);
		}
		if(terminadoCompras.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pTerminadoCompras", terminadoCompras);
		}
		if(terminadoProyectos.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pTerminadoProyectos", terminadoProyectos);
		}
		if(terminadoFinanzas.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pTerminadoFinanzas", terminadoFinanzas);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPreventaDetenido", preventaDetenido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pVentasDetenido", ventasDetenido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComprasDetenido", comprasDetenido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProyectosDetenido", proyectosDetenido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFinzanzasDetenido", finzanzasDetenido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEstatusPlan", estatusPlan);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEstatusPlan", idUsuarioEstatusPlan);
		if(fechaEstatusPlan.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEstatusPlan", fechaEstatusPlan);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPagos", pagos);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenCompra", ordenCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNotaCreditoProveedor", notaCreditoProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		if(compromisoPreventa.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCompromisoPreventa", compromisoPreventa);
		}
		if(compromisoVentas.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCompromisoVentas", compromisoVentas);
		}
		if(compromisoCompras.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCompromisoCompras", compromisoCompras);
		}
		if(compromisoProyectos.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCompromisoProyectos", compromisoProyectos);
		}
		if(compromisoFinanzas.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCompromisoFinanzas", compromisoFinanzas);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAutorizado", autorizado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusCompras", idEstatusCompras);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idOportunidad= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOportunidad"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Oportunidad_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOportunidad", oportunidad);
		if(fechaCreacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreacion", idUsuarioCreacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresOportunidad", idNivelInteresOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pArchivo", archivo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCotizaciones", cotizaciones);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPedidos", pedidos);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProyectos", proyectos);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFacturas", facturas);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNeto", neto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClasificacion", clasificacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCerrado", cerrado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUltimaNota", ultimaNota);
		if(fechaNota.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaNota", fechaNota);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsProyecto", esProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUrgente", urgente);
		if(fechaCierre.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCierre", fechaCierre);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pProveedores", proveedores);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMes1", mes1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMes2", mes2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMes3", mes3);
		if(terminadoPreventa.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pTerminadoPreventa", terminadoPreventa);
		}
		if(terminadoVentas.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pTerminadoVentas", terminadoVentas);
		}
		if(terminadoCompras.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pTerminadoCompras", terminadoCompras);
		}
		if(terminadoProyectos.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pTerminadoProyectos", terminadoProyectos);
		}
		if(terminadoFinanzas.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pTerminadoFinanzas", terminadoFinanzas);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPreventaDetenido", preventaDetenido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pVentasDetenido", ventasDetenido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComprasDetenido", comprasDetenido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProyectosDetenido", proyectosDetenido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFinzanzasDetenido", finzanzasDetenido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEstatusPlan", estatusPlan);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEstatusPlan", idUsuarioEstatusPlan);
		if(fechaEstatusPlan.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEstatusPlan", fechaEstatusPlan);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pPagos", pagos);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenCompra", ordenCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNotaCreditoProveedor", notaCreditoProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		if(compromisoPreventa.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pCompromisoPreventa", compromisoPreventa);
		}
		if(compromisoVentas.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pCompromisoVentas", compromisoVentas);
		}
		if(compromisoCompras.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pCompromisoCompras", compromisoCompras);
		}
		if(compromisoProyectos.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pCompromisoProyectos", compromisoProyectos);
		}
		if(compromisoFinanzas.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pCompromisoFinanzas", compromisoFinanzas);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pAutorizado", autorizado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusCompras", idEstatusCompras);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Oportunidad_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
