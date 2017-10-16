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

public partial class CDetalleFacturaProveedor
{
	//Propiedades Privadas
	private int idDetalleFacturaProveedor;
	private int idEncabezadoFacturaProveedor;
	private string tipo;
	private string clave;
	private string descripcion;
	private decimal cantidad;
	private decimal precio;
	private decimal total;
	private int idUnidadCompraVenta;
	private string nota;
	private string numeroSerie;
	private int idCliente;
	private string pedimento;
	private int idAlmacen;
	private int idOrdenCompraDetalle;
	private DateTime fechaFacturacion;
	private string clienteProyecto;
	private int idTipoCompra;
	private decimal descuento;
	private string referenciaEntrega;
	private int idPedido;
	private int idPedidoDetalle;
	private int idUsuarioSolicito;
	private int idProyecto;
	private int idProducto;
	private int idServicio;
	private int idTipoIVA;
	private decimal iVA;
	private int idSubCuentaContable;
	private DateTime fechaAlta;
	private bool baja;
	
	//Propiedades
	public int IdDetalleFacturaProveedor
	{
		get { return idDetalleFacturaProveedor; }
		set
		{
			idDetalleFacturaProveedor = value;
		}
	}
	
	public int IdEncabezadoFacturaProveedor
	{
		get { return idEncabezadoFacturaProveedor; }
		set
		{
			idEncabezadoFacturaProveedor = value;
		}
	}
	
	public string Tipo
	{
		get { return tipo; }
		set
		{
			tipo = value;
		}
	}
	
	public string Clave
	{
		get { return clave; }
		set
		{
			clave = value;
		}
	}
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			descripcion = value;
		}
	}
	
	public decimal Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
		}
	}
	
	public decimal Precio
	{
		get { return precio; }
		set
		{
			precio = value;
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
	
	public int IdUnidadCompraVenta
	{
		get { return idUnidadCompraVenta; }
		set
		{
			idUnidadCompraVenta = value;
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
	
	public string NumeroSerie
	{
		get { return numeroSerie; }
		set
		{
			numeroSerie = value;
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
	
	public string Pedimento
	{
		get { return pedimento; }
		set
		{
			pedimento = value;
		}
	}
	
	public int IdAlmacen
	{
		get { return idAlmacen; }
		set
		{
			idAlmacen = value;
		}
	}
	
	public int IdOrdenCompraDetalle
	{
		get { return idOrdenCompraDetalle; }
		set
		{
			idOrdenCompraDetalle = value;
		}
	}
	
	public DateTime FechaFacturacion
	{
		get { return fechaFacturacion; }
		set { fechaFacturacion = value; }
	}
	
	public string ClienteProyecto
	{
		get { return clienteProyecto; }
		set
		{
			clienteProyecto = value;
		}
	}
	
	public int IdTipoCompra
	{
		get { return idTipoCompra; }
		set
		{
			idTipoCompra = value;
		}
	}
	
	public decimal Descuento
	{
		get { return descuento; }
		set
		{
			descuento = value;
		}
	}
	
	public string ReferenciaEntrega
	{
		get { return referenciaEntrega; }
		set
		{
			referenciaEntrega = value;
		}
	}
	
	public int IdPedido
	{
		get { return idPedido; }
		set
		{
			idPedido = value;
		}
	}
	
	public int IdPedidoDetalle
	{
		get { return idPedidoDetalle; }
		set
		{
			idPedidoDetalle = value;
		}
	}
	
	public int IdUsuarioSolicito
	{
		get { return idUsuarioSolicito; }
		set
		{
			idUsuarioSolicito = value;
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
	
	public int IdProducto
	{
		get { return idProducto; }
		set
		{
			idProducto = value;
		}
	}
	
	public int IdServicio
	{
		get { return idServicio; }
		set
		{
			idServicio = value;
		}
	}
	
	public int IdTipoIVA
	{
		get { return idTipoIVA; }
		set
		{
			idTipoIVA = value;
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
	
	public int IdSubCuentaContable
	{
		get { return idSubCuentaContable; }
		set
		{
			idSubCuentaContable = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CDetalleFacturaProveedor()
	{
		idDetalleFacturaProveedor = 0;
		idEncabezadoFacturaProveedor = 0;
		tipo = "";
		clave = "";
		descripcion = "";
		cantidad = 0;
		precio = 0;
		total = 0;
		idUnidadCompraVenta = 0;
		nota = "";
		numeroSerie = "";
		idCliente = 0;
		pedimento = "";
		idAlmacen = 0;
		idOrdenCompraDetalle = 0;
		fechaFacturacion = new DateTime(1, 1, 1);
		clienteProyecto = "";
		idTipoCompra = 0;
		descuento = 0;
		referenciaEntrega = "";
		idPedido = 0;
		idPedidoDetalle = 0;
		idUsuarioSolicito = 0;
		idProyecto = 0;
		idProducto = 0;
		idServicio = 0;
		idTipoIVA = 0;
		iVA = 0;
		idSubCuentaContable = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CDetalleFacturaProveedor(int pIdDetalleFacturaProveedor)
	{
		idDetalleFacturaProveedor = pIdDetalleFacturaProveedor;
		idEncabezadoFacturaProveedor = 0;
		tipo = "";
		clave = "";
		descripcion = "";
		cantidad = 0;
		precio = 0;
		total = 0;
		idUnidadCompraVenta = 0;
		nota = "";
		numeroSerie = "";
		idCliente = 0;
		pedimento = "";
		idAlmacen = 0;
		idOrdenCompraDetalle = 0;
		fechaFacturacion = new DateTime(1, 1, 1);
		clienteProyecto = "";
		idTipoCompra = 0;
		descuento = 0;
		referenciaEntrega = "";
		idPedido = 0;
		idPedidoDetalle = 0;
		idUsuarioSolicito = 0;
		idProyecto = 0;
		idProducto = 0;
		idServicio = 0;
		idTipoIVA = 0;
		iVA = 0;
		idSubCuentaContable = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleFacturaProveedor>(typeof(CDetalleFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDetalleFacturaProveedor>(typeof(CDetalleFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleFacturaProveedor>(typeof(CDetalleFacturaProveedor), pConexion);
		foreach (CDetalleFacturaProveedor O in Obten.ListaRegistros)
		{
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			tipo = O.Tipo;
			clave = O.Clave;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			precio = O.Precio;
			total = O.Total;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			nota = O.Nota;
			numeroSerie = O.NumeroSerie;
			idCliente = O.IdCliente;
			pedimento = O.Pedimento;
			idAlmacen = O.IdAlmacen;
			idOrdenCompraDetalle = O.IdOrdenCompraDetalle;
			fechaFacturacion = O.FechaFacturacion;
			clienteProyecto = O.ClienteProyecto;
			idTipoCompra = O.IdTipoCompra;
			descuento = O.Descuento;
			referenciaEntrega = O.ReferenciaEntrega;
			idPedido = O.IdPedido;
			idPedidoDetalle = O.IdPedidoDetalle;
			idUsuarioSolicito = O.IdUsuarioSolicito;
			idProyecto = O.IdProyecto;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			idSubCuentaContable = O.IdSubCuentaContable;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CDetalleFacturaProveedor>(typeof(CDetalleFacturaProveedor), pConexion);
		foreach (CDetalleFacturaProveedor O in Obten.ListaRegistros)
		{
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			tipo = O.Tipo;
			clave = O.Clave;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			precio = O.Precio;
			total = O.Total;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			nota = O.Nota;
			numeroSerie = O.NumeroSerie;
			idCliente = O.IdCliente;
			pedimento = O.Pedimento;
			idAlmacen = O.IdAlmacen;
			idOrdenCompraDetalle = O.IdOrdenCompraDetalle;
			fechaFacturacion = O.FechaFacturacion;
			clienteProyecto = O.ClienteProyecto;
			idTipoCompra = O.IdTipoCompra;
			descuento = O.Descuento;
			referenciaEntrega = O.ReferenciaEntrega;
			idPedido = O.IdPedido;
			idPedidoDetalle = O.IdPedidoDetalle;
			idUsuarioSolicito = O.IdUsuarioSolicito;
			idProyecto = O.IdProyecto;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			idSubCuentaContable = O.IdSubCuentaContable;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CDetalleFacturaProveedor>(typeof(CDetalleFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdDetalleFacturaProveedor"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipo", tipo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecio", precio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroSerie", numeroSerie);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPedimento", pedimento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", idOrdenCompraDetalle);
		if(fechaFacturacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFacturacion", fechaFacturacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClienteProyecto", clienteProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pReferenciaEntrega", referenciaEntrega);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedido", idPedido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoDetalle", idPedidoDetalle);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", idUsuarioSolicito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", idSubCuentaContable);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDetalleFacturaProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDetalleFacturaProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipo", tipo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrecio", precio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroSerie", numeroSerie);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPedimento", pedimento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", idOrdenCompraDetalle);
		if(fechaFacturacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaFacturacion", fechaFacturacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pClienteProyecto", clienteProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pReferenciaEntrega", referenciaEntrega);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPedido", idPedido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoDetalle", idPedidoDetalle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", idUsuarioSolicito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", idSubCuentaContable);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_DetalleFacturaProveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
