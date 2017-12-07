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

public partial class CCotizacionDetalle
{
	//Propiedades Privadas
	private int idCotizacionDetalle;
	private string clave;
	private string descripcion;
	private int cantidad;
	private decimal precioUnitario;
	private decimal total;
	private decimal descuento;
	private int ordenDeCompraCantidad;
	private int recepcionCantidad;
	private int remisionCantidad;
	private int facturacionCantidad;
	private int idCotizacion;
	private int idProducto;
	private int idServicio;
	private int idTiempoDeEntrega;
	private DateTime ordenDeCompra;
	private DateTime recepcion;
	private DateTime remision;
	private DateTime facturacion;
	private int ordenacion;
	private int cantidadPendienteFacturar;
	private int idTipoIVA;
	private decimal iVA;
	private bool partidaCompuesta;
	private int cantidadCompuesta;
	private int ordenCompraCantidadCompuesta;
	private int remisionCantidadCompuesta;
	private string claveProdServ;
	private bool baja;
	
	//Propiedades
	public int IdCotizacionDetalle
	{
		get { return idCotizacionDetalle; }
		set
		{
			idCotizacionDetalle = value;
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
	
	public decimal PrecioUnitario
	{
		get { return precioUnitario; }
		set
		{
			precioUnitario = value;
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
	
	public decimal Descuento
	{
		get { return descuento; }
		set
		{
			descuento = value;
		}
	}
	
	public int OrdenDeCompraCantidad
	{
		get { return ordenDeCompraCantidad; }
		set
		{
			ordenDeCompraCantidad = value;
		}
	}
	
	public int RecepcionCantidad
	{
		get { return recepcionCantidad; }
		set
		{
			recepcionCantidad = value;
		}
	}
	
	public int RemisionCantidad
	{
		get { return remisionCantidad; }
		set
		{
			remisionCantidad = value;
		}
	}
	
	public int FacturacionCantidad
	{
		get { return facturacionCantidad; }
		set
		{
			facturacionCantidad = value;
		}
	}
	
	public int IdCotizacion
	{
		get { return idCotizacion; }
		set
		{
			idCotizacion = value;
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
	
	public int IdTiempoDeEntrega
	{
		get { return idTiempoDeEntrega; }
		set
		{
			idTiempoDeEntrega = value;
		}
	}
	
	public DateTime OrdenDeCompra
	{
		get { return ordenDeCompra; }
		set { ordenDeCompra = value; }
	}
	
	public DateTime Recepcion
	{
		get { return recepcion; }
		set { recepcion = value; }
	}
	
	public DateTime Remision
	{
		get { return remision; }
		set { remision = value; }
	}
	
	public DateTime Facturacion
	{
		get { return facturacion; }
		set { facturacion = value; }
	}
	
	public int Ordenacion
	{
		get { return ordenacion; }
		set
		{
			ordenacion = value;
		}
	}
	
	public int CantidadPendienteFacturar
	{
		get { return cantidadPendienteFacturar; }
		set
		{
			cantidadPendienteFacturar = value;
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
	
	public bool PartidaCompuesta
	{
		get { return partidaCompuesta; }
		set { partidaCompuesta = value; }
	}
	
	public int CantidadCompuesta
	{
		get { return cantidadCompuesta; }
		set
		{
			cantidadCompuesta = value;
		}
	}
	
	public int OrdenCompraCantidadCompuesta
	{
		get { return ordenCompraCantidadCompuesta; }
		set
		{
			ordenCompraCantidadCompuesta = value;
		}
	}
	
	public int RemisionCantidadCompuesta
	{
		get { return remisionCantidadCompuesta; }
		set
		{
			remisionCantidadCompuesta = value;
		}
	}
	
	public string ClaveProdServ
	{
		get { return claveProdServ; }
		set
		{
			claveProdServ = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CCotizacionDetalle()
	{
		idCotizacionDetalle = 0;
		clave = "";
		descripcion = "";
		cantidad = 0;
		precioUnitario = 0;
		total = 0;
		descuento = 0;
		ordenDeCompraCantidad = 0;
		recepcionCantidad = 0;
		remisionCantidad = 0;
		facturacionCantidad = 0;
		idCotizacion = 0;
		idProducto = 0;
		idServicio = 0;
		idTiempoDeEntrega = 0;
		ordenDeCompra = new DateTime(1, 1, 1);
		recepcion = new DateTime(1, 1, 1);
		remision = new DateTime(1, 1, 1);
		facturacion = new DateTime(1, 1, 1);
		ordenacion = 0;
		cantidadPendienteFacturar = 0;
		idTipoIVA = 0;
		iVA = 0;
		partidaCompuesta = false;
		cantidadCompuesta = 0;
		ordenCompraCantidadCompuesta = 0;
		remisionCantidadCompuesta = 0;
		claveProdServ = "";
		baja = false;
	}
	
	public CCotizacionDetalle(int pIdCotizacionDetalle)
	{
		idCotizacionDetalle = pIdCotizacionDetalle;
		clave = "";
		descripcion = "";
		cantidad = 0;
		precioUnitario = 0;
		total = 0;
		descuento = 0;
		ordenDeCompraCantidad = 0;
		recepcionCantidad = 0;
		remisionCantidad = 0;
		facturacionCantidad = 0;
		idCotizacion = 0;
		idProducto = 0;
		idServicio = 0;
		idTiempoDeEntrega = 0;
		ordenDeCompra = new DateTime(1, 1, 1);
		recepcion = new DateTime(1, 1, 1);
		remision = new DateTime(1, 1, 1);
		facturacion = new DateTime(1, 1, 1);
		ordenacion = 0;
		cantidadPendienteFacturar = 0;
		idTipoIVA = 0;
		iVA = 0;
		partidaCompuesta = false;
		cantidadCompuesta = 0;
		ordenCompraCantidadCompuesta = 0;
		remisionCantidadCompuesta = 0;
		claveProdServ = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
		foreach (CCotizacionDetalle O in Obten.ListaRegistros)
		{
			idCotizacionDetalle = O.IdCotizacionDetalle;
			clave = O.Clave;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			precioUnitario = O.PrecioUnitario;
			total = O.Total;
			descuento = O.Descuento;
			ordenDeCompraCantidad = O.OrdenDeCompraCantidad;
			recepcionCantidad = O.RecepcionCantidad;
			remisionCantidad = O.RemisionCantidad;
			facturacionCantidad = O.FacturacionCantidad;
			idCotizacion = O.IdCotizacion;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idTiempoDeEntrega = O.IdTiempoDeEntrega;
			ordenDeCompra = O.OrdenDeCompra;
			recepcion = O.Recepcion;
			remision = O.Remision;
			facturacion = O.Facturacion;
			ordenacion = O.Ordenacion;
			cantidadPendienteFacturar = O.CantidadPendienteFacturar;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			partidaCompuesta = O.PartidaCompuesta;
			cantidadCompuesta = O.CantidadCompuesta;
			ordenCompraCantidadCompuesta = O.OrdenCompraCantidadCompuesta;
			remisionCantidadCompuesta = O.RemisionCantidadCompuesta;
			claveProdServ = O.ClaveProdServ;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionDetalle_ConsultarFiltros";
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
		Obten.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
		foreach (CCotizacionDetalle O in Obten.ListaRegistros)
		{
			idCotizacionDetalle = O.IdCotizacionDetalle;
			clave = O.Clave;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			precioUnitario = O.PrecioUnitario;
			total = O.Total;
			descuento = O.Descuento;
			ordenDeCompraCantidad = O.OrdenDeCompraCantidad;
			recepcionCantidad = O.RecepcionCantidad;
			remisionCantidad = O.RemisionCantidad;
			facturacionCantidad = O.FacturacionCantidad;
			idCotizacion = O.IdCotizacion;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idTiempoDeEntrega = O.IdTiempoDeEntrega;
			ordenDeCompra = O.OrdenDeCompra;
			recepcion = O.Recepcion;
			remision = O.Remision;
			facturacion = O.Facturacion;
			ordenacion = O.Ordenacion;
			cantidadPendienteFacturar = O.CantidadPendienteFacturar;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			partidaCompuesta = O.PartidaCompuesta;
			cantidadCompuesta = O.CantidadCompuesta;
			ordenCompraCantidadCompuesta = O.OrdenCompraCantidadCompuesta;
			remisionCantidadCompuesta = O.RemisionCantidadCompuesta;
			claveProdServ = O.ClaveProdServ;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionDetalle_ConsultarFiltros";
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
		Obten.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_CotizacionDetalle_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", 0);
		Agregar.StoredProcedure.Parameters["@pIdCotizacionDetalle"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenDeCompraCantidad", ordenDeCompraCantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRecepcionCantidad", recepcionCantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRemisionCantidad", remisionCantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturacionCantidad", facturacionCantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTiempoDeEntrega", idTiempoDeEntrega);
		if(ordenDeCompra.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenDeCompra", ordenDeCompra);
		}
		if(recepcion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pRecepcion", recepcion);
		}
		if(remision.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pRemision", remision);
		}
		if(facturacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturacion", facturacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenacion", ordenacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadPendienteFacturar", cantidadPendienteFacturar);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPartidaCompuesta", partidaCompuesta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadCompuesta", cantidadCompuesta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenCompraCantidadCompuesta", ordenCompraCantidadCompuesta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRemisionCantidadCompuesta", remisionCantidadCompuesta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveProdServ", claveProdServ);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idCotizacionDetalle= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCotizacionDetalle"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_CotizacionDetalle_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", idCotizacionDetalle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenDeCompraCantidad", ordenDeCompraCantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRecepcionCantidad", recepcionCantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRemisionCantidad", remisionCantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFacturacionCantidad", facturacionCantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTiempoDeEntrega", idTiempoDeEntrega);
		if(ordenDeCompra.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenDeCompra", ordenDeCompra);
		}
		if(recepcion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pRecepcion", recepcion);
		}
		if(remision.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pRemision", remision);
		}
		if(facturacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFacturacion", facturacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenacion", ordenacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadPendienteFacturar", cantidadPendienteFacturar);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPartidaCompuesta", partidaCompuesta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadCompuesta", cantidadCompuesta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenCompraCantidadCompuesta", ordenCompraCantidadCompuesta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRemisionCantidadCompuesta", remisionCantidadCompuesta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveProdServ", claveProdServ);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_CotizacionDetalle_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", idCotizacionDetalle);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
