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

public partial class COrdenCompraDetalle
{
	//Propiedades Privadas
	private int idOrdenCompraDetalle;
	private string clave;
	private string descripcion;
	private int cantidad;
	private decimal costo;
	private decimal total;
	private decimal saldo;
	private int idPedidoEncabezado;
	private int idPedidoDetalle;
	private int descuento;
	private int idOrdenCompraEncabezado;
	private int idTipoCompra;
	private int idOrdenCompraEstatus;
	private int idProducto;
	private int idUnidadCompraVenta;
	private int idServicio;
	private DateTime fechaAlta;
	private int recepcionCantidad;
	private DateTime fechaRecepcion;
	private int idTipoIVA;
	private decimal iVA;
	private bool baja;
	
	//Propiedades
	public int IdOrdenCompraDetalle
	{
		get { return idOrdenCompraDetalle; }
		set
		{
			idOrdenCompraDetalle = value;
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
	
	public int Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
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
	
	public int IdPedidoEncabezado
	{
		get { return idPedidoEncabezado; }
		set
		{
			idPedidoEncabezado = value;
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
	
	public int Descuento
	{
		get { return descuento; }
		set
		{
			descuento = value;
		}
	}
	
	public int IdOrdenCompraEncabezado
	{
		get { return idOrdenCompraEncabezado; }
		set
		{
			idOrdenCompraEncabezado = value;
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
	
	public int IdOrdenCompraEstatus
	{
		get { return idOrdenCompraEstatus; }
		set
		{
			idOrdenCompraEstatus = value;
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
	
	public int IdUnidadCompraVenta
	{
		get { return idUnidadCompraVenta; }
		set
		{
			idUnidadCompraVenta = value;
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
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public int RecepcionCantidad
	{
		get { return recepcionCantidad; }
		set
		{
			recepcionCantidad = value;
		}
	}
	
	public DateTime FechaRecepcion
	{
		get { return fechaRecepcion; }
		set { fechaRecepcion = value; }
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
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public COrdenCompraDetalle()
	{
		idOrdenCompraDetalle = 0;
		clave = "";
		descripcion = "";
		cantidad = 0;
		costo = 0;
		total = 0;
		saldo = 0;
		idPedidoEncabezado = 0;
		idPedidoDetalle = 0;
		descuento = 0;
		idOrdenCompraEncabezado = 0;
		idTipoCompra = 0;
		idOrdenCompraEstatus = 0;
		idProducto = 0;
		idUnidadCompraVenta = 0;
		idServicio = 0;
		fechaAlta = new DateTime(1, 1, 1);
		recepcionCantidad = 0;
		fechaRecepcion = new DateTime(1, 1, 1);
		idTipoIVA = 0;
		iVA = 0;
		baja = false;
	}
	
	public COrdenCompraDetalle(int pIdOrdenCompraDetalle)
	{
		idOrdenCompraDetalle = pIdOrdenCompraDetalle;
		clave = "";
		descripcion = "";
		cantidad = 0;
		costo = 0;
		total = 0;
		saldo = 0;
		idPedidoEncabezado = 0;
		idPedidoDetalle = 0;
		descuento = 0;
		idOrdenCompraEncabezado = 0;
		idTipoCompra = 0;
		idOrdenCompraEstatus = 0;
		idProducto = 0;
		idUnidadCompraVenta = 0;
		idServicio = 0;
		fechaAlta = new DateTime(1, 1, 1);
		recepcionCantidad = 0;
		fechaRecepcion = new DateTime(1, 1, 1);
		idTipoIVA = 0;
		iVA = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrdenCompraDetalle>(typeof(COrdenCompraDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COrdenCompraDetalle>(typeof(COrdenCompraDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrdenCompraDetalle>(typeof(COrdenCompraDetalle), pConexion);
		foreach (COrdenCompraDetalle O in Obten.ListaRegistros)
		{
			idOrdenCompraDetalle = O.IdOrdenCompraDetalle;
			clave = O.Clave;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			costo = O.Costo;
			total = O.Total;
			saldo = O.Saldo;
			idPedidoEncabezado = O.IdPedidoEncabezado;
			idPedidoDetalle = O.IdPedidoDetalle;
			descuento = O.Descuento;
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			idTipoCompra = O.IdTipoCompra;
			idOrdenCompraEstatus = O.IdOrdenCompraEstatus;
			idProducto = O.IdProducto;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			idServicio = O.IdServicio;
			fechaAlta = O.FechaAlta;
			recepcionCantidad = O.RecepcionCantidad;
			fechaRecepcion = O.FechaRecepcion;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_ConsultarFiltros";
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
		Obten.Llena<COrdenCompraDetalle>(typeof(COrdenCompraDetalle), pConexion);
		foreach (COrdenCompraDetalle O in Obten.ListaRegistros)
		{
			idOrdenCompraDetalle = O.IdOrdenCompraDetalle;
			clave = O.Clave;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			costo = O.Costo;
			total = O.Total;
			saldo = O.Saldo;
			idPedidoEncabezado = O.IdPedidoEncabezado;
			idPedidoDetalle = O.IdPedidoDetalle;
			descuento = O.Descuento;
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			idTipoCompra = O.IdTipoCompra;
			idOrdenCompraEstatus = O.IdOrdenCompraEstatus;
			idProducto = O.IdProducto;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			idServicio = O.IdServicio;
			fechaAlta = O.FechaAlta;
			recepcionCantidad = O.RecepcionCantidad;
			fechaRecepcion = O.FechaRecepcion;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_ConsultarFiltros";
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
		Obten.Llena<COrdenCompraDetalle>(typeof(COrdenCompraDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", 0);
		Agregar.StoredProcedure.Parameters["@pIdOrdenCompraDetalle"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoEncabezado", idPedidoEncabezado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoDetalle", idPedidoDetalle);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEstatus", idOrdenCompraEstatus);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRecepcionCantidad", recepcionCantidad);
		if(fechaRecepcion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRecepcion", fechaRecepcion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idOrdenCompraDetalle= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrdenCompraDetalle"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", idOrdenCompraDetalle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoEncabezado", idPedidoEncabezado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoDetalle", idPedidoDetalle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEstatus", idOrdenCompraEstatus);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pRecepcionCantidad", recepcionCantidad);
		if(fechaRecepcion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRecepcion", fechaRecepcion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_OrdenCompraDetalle_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", idOrdenCompraDetalle);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
