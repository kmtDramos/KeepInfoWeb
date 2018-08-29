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

public partial class COrdenCompraEncabezado
{
	//Propiedades Privadas
	private int idOrdenCompraEncabezado;
	private DateTime fechaRequerida;
	private DateTime fechaRealEntrega;
	private decimal subtotal;
	private decimal iVA;
	private decimal total;
	private decimal saldo;
	private int idUsuario;
	private int idProveedor;
	private int idTipoMoneda;
	private DateTime fechaAlta;
	private string direccionEntrega;
	private string nota;
	private decimal tipoCambio;
	private bool consolidado;
	private bool sinPedido;
	private int idDivision;
	private int idCliente;
	private string cantidadTotalLetra;
	private int idPedidoEncabezado;
	private int idProyecto;
	private string clienteProyecto;
	private int folio;
	private DateTime fechaRecepcion;
	private int idOportunidad;
	private int idPresupuesto;
	private bool baja;
	
	//Propiedades
	public int IdOrdenCompraEncabezado
	{
		get { return idOrdenCompraEncabezado; }
		set
		{
			idOrdenCompraEncabezado = value;
		}
	}
	
	public DateTime FechaRequerida
	{
		get { return fechaRequerida; }
		set { fechaRequerida = value; }
	}
	
	public DateTime FechaRealEntrega
	{
		get { return fechaRealEntrega; }
		set { fechaRealEntrega = value; }
	}
	
	public decimal Subtotal
	{
		get { return subtotal; }
		set
		{
			subtotal = value;
		}
	}
	
	public decimal IVA
	{
		get { return iVA; }
		set
		{
			iVA = value;
		}
	}
	
	public decimal Total
	{
		get { return total; }
		set
		{
			total = value;
		}
	}
	
	public decimal Saldo
	{
		get { return saldo; }
		set
		{
			saldo = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public int IdProveedor
	{
		get { return idProveedor; }
		set
		{
			idProveedor = value;
		}
	}
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			idTipoMoneda = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public string DireccionEntrega
	{
		get { return direccionEntrega; }
		set
		{
			direccionEntrega = value;
		}
	}
	
	public string Nota
	{
		get { return nota; }
		set
		{
			nota = value;
		}
	}
	
	public decimal TipoCambio
	{
		get { return tipoCambio; }
		set
		{
			tipoCambio = value;
		}
	}
	
	public bool Consolidado
	{
		get { return consolidado; }
		set { consolidado = value; }
	}
	
	public bool SinPedido
	{
		get { return sinPedido; }
		set { sinPedido = value; }
	}
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
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
	
	public string CantidadTotalLetra
	{
		get { return cantidadTotalLetra; }
		set
		{
			cantidadTotalLetra = value;
		}
	}
	
	public int IdPedidoEncabezado
	{
		get { return idPedidoEncabezado; }
		set
		{
			idPedidoEncabezado = value;
		}
	}
	
	public int IdProyecto
	{
		get { return idProyecto; }
		set
		{
			idProyecto = value;
		}
	}
	
	public string ClienteProyecto
	{
		get { return clienteProyecto; }
		set
		{
			clienteProyecto = value;
		}
	}
	
	public int Folio
	{
		get { return folio; }
		set
		{
			folio = value;
		}
	}
	
	public DateTime FechaRecepcion
	{
		get { return fechaRecepcion; }
		set { fechaRecepcion = value; }
	}
	
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
		}
	}
	
	public int IdPresupuesto
	{
		get { return idPresupuesto; }
		set
		{
			idPresupuesto = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public COrdenCompraEncabezado()
	{
		idOrdenCompraEncabezado = 0;
		fechaRequerida = new DateTime(1, 1, 1);
		fechaRealEntrega = new DateTime(1, 1, 1);
		subtotal = 0;
		iVA = 0;
		total = 0;
		saldo = 0;
		idUsuario = 0;
		idProveedor = 0;
		idTipoMoneda = 0;
		fechaAlta = new DateTime(1, 1, 1);
		direccionEntrega = "";
		nota = "";
		tipoCambio = 0;
		consolidado = false;
		sinPedido = false;
		idDivision = 0;
		idCliente = 0;
		cantidadTotalLetra = "";
		idPedidoEncabezado = 0;
		idProyecto = 0;
		clienteProyecto = "";
		folio = 0;
		fechaRecepcion = new DateTime(1, 1, 1);
		idOportunidad = 0;
		idPresupuesto = 0;
		baja = false;
	}
	
	public COrdenCompraEncabezado(int pIdOrdenCompraEncabezado)
	{
		idOrdenCompraEncabezado = pIdOrdenCompraEncabezado;
		fechaRequerida = new DateTime(1, 1, 1);
		fechaRealEntrega = new DateTime(1, 1, 1);
		subtotal = 0;
		iVA = 0;
		total = 0;
		saldo = 0;
		idUsuario = 0;
		idProveedor = 0;
		idTipoMoneda = 0;
		fechaAlta = new DateTime(1, 1, 1);
		direccionEntrega = "";
		nota = "";
		tipoCambio = 0;
		consolidado = false;
		sinPedido = false;
		idDivision = 0;
		idCliente = 0;
		cantidadTotalLetra = "";
		idPedidoEncabezado = 0;
		idProyecto = 0;
		clienteProyecto = "";
		folio = 0;
		fechaRecepcion = new DateTime(1, 1, 1);
		idOportunidad = 0;
		idPresupuesto = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrdenCompraEncabezado>(typeof(COrdenCompraEncabezado), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COrdenCompraEncabezado>(typeof(COrdenCompraEncabezado), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrdenCompraEncabezado>(typeof(COrdenCompraEncabezado), pConexion);
		foreach (COrdenCompraEncabezado O in Obten.ListaRegistros)
		{
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			fechaRequerida = O.FechaRequerida;
			fechaRealEntrega = O.FechaRealEntrega;
			subtotal = O.Subtotal;
			iVA = O.IVA;
			total = O.Total;
			saldo = O.Saldo;
			idUsuario = O.IdUsuario;
			idProveedor = O.IdProveedor;
			idTipoMoneda = O.IdTipoMoneda;
			fechaAlta = O.FechaAlta;
			direccionEntrega = O.DireccionEntrega;
			nota = O.Nota;
			tipoCambio = O.TipoCambio;
			consolidado = O.Consolidado;
			sinPedido = O.SinPedido;
			idDivision = O.IdDivision;
			idCliente = O.IdCliente;
			cantidadTotalLetra = O.CantidadTotalLetra;
			idPedidoEncabezado = O.IdPedidoEncabezado;
			idProyecto = O.IdProyecto;
			clienteProyecto = O.ClienteProyecto;
			folio = O.Folio;
			fechaRecepcion = O.FechaRecepcion;
			idOportunidad = O.IdOportunidad;
			idPresupuesto = O.IdPresupuesto;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_ConsultarFiltros";
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
		Obten.Llena<COrdenCompraEncabezado>(typeof(COrdenCompraEncabezado), pConexion);
		foreach (COrdenCompraEncabezado O in Obten.ListaRegistros)
		{
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			fechaRequerida = O.FechaRequerida;
			fechaRealEntrega = O.FechaRealEntrega;
			subtotal = O.Subtotal;
			iVA = O.IVA;
			total = O.Total;
			saldo = O.Saldo;
			idUsuario = O.IdUsuario;
			idProveedor = O.IdProveedor;
			idTipoMoneda = O.IdTipoMoneda;
			fechaAlta = O.FechaAlta;
			direccionEntrega = O.DireccionEntrega;
			nota = O.Nota;
			tipoCambio = O.TipoCambio;
			consolidado = O.Consolidado;
			sinPedido = O.SinPedido;
			idDivision = O.IdDivision;
			idCliente = O.IdCliente;
			cantidadTotalLetra = O.CantidadTotalLetra;
			idPedidoEncabezado = O.IdPedidoEncabezado;
			idProyecto = O.IdProyecto;
			clienteProyecto = O.ClienteProyecto;
			folio = O.Folio;
			fechaRecepcion = O.FechaRecepcion;
			idOportunidad = O.IdOportunidad;
			idPresupuesto = O.IdPresupuesto;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_ConsultarFiltros";
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
		Obten.Llena<COrdenCompraEncabezado>(typeof(COrdenCompraEncabezado), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", 0);
		Agregar.StoredProcedure.Parameters["@pIdOrdenCompraEncabezado"].Direction = ParameterDirection.Output;
		if(fechaRequerida.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRequerida", fechaRequerida);
		}
		if(fechaRealEntrega.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRealEntrega", fechaRealEntrega);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDireccionEntrega", direccionEntrega);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pConsolidado", consolidado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSinPedido", sinPedido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadTotalLetra", cantidadTotalLetra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoEncabezado", idPedidoEncabezado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClienteProyecto", clienteProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		if(fechaRecepcion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRecepcion", fechaRecepcion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idOrdenCompraEncabezado= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrdenCompraEncabezado"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		if(fechaRequerida.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRequerida", fechaRequerida);
		}
		if(fechaRealEntrega.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRealEntrega", fechaRealEntrega);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pDireccionEntrega", direccionEntrega);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pConsolidado", consolidado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSinPedido", sinPedido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadTotalLetra", cantidadTotalLetra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoEncabezado", idPedidoEncabezado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClienteProyecto", clienteProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		if(fechaRecepcion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRecepcion", fechaRecepcion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_OrdenCompraEncabezado_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
