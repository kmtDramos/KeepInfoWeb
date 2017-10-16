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

public partial class CDetalleRemision
{
	//Propiedades Privadas
	private int idDetalleRemision;
	private int cantidad;
	private decimal precioUnitario;
	private decimal monto;
	private int idEncabezadoRemision;
	private int idProducto;
	private int idProveedor;
	private int idEncabezadoFacturaProveedor;
	private int idDetalleFacturaProveedor;
	private int idEncabezadoPedido;
	private int idDetallePedido;
	private int idProyecto;
	private int idAlmacen;
	private DateTime fechaAlta;
	private bool baja;
	
	//Propiedades
	public int IdDetalleRemision
	{
		get { return idDetalleRemision; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDetalleRemision = value;
		}
	}
	
	public int Cantidad
	{
		get { return cantidad; }
		set
		{
			if (value < 0)
			{
				return;
			}
			cantidad = value;
		}
	}
	
	public decimal PrecioUnitario
	{
		get { return precioUnitario; }
		set
		{
			if (value < 0)
			{
				return;
			}
			precioUnitario = value;
		}
	}
	
	public decimal Monto
	{
		get { return monto; }
		set
		{
			if (value < 0)
			{
				return;
			}
			monto = value;
		}
	}
	
	public int IdEncabezadoRemision
	{
		get { return idEncabezadoRemision; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEncabezadoRemision = value;
		}
	}
	
	public int IdProducto
	{
		get { return idProducto; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProducto = value;
		}
	}
	
	public int IdProveedor
	{
		get { return idProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProveedor = value;
		}
	}
	
	public int IdEncabezadoFacturaProveedor
	{
		get { return idEncabezadoFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEncabezadoFacturaProveedor = value;
		}
	}
	
	public int IdDetalleFacturaProveedor
	{
		get { return idDetalleFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDetalleFacturaProveedor = value;
		}
	}
	
	public int IdEncabezadoPedido
	{
		get { return idEncabezadoPedido; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEncabezadoPedido = value;
		}
	}
	
	public int IdDetallePedido
	{
		get { return idDetallePedido; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDetallePedido = value;
		}
	}
	
	public int IdProyecto
	{
		get { return idProyecto; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProyecto = value;
		}
	}
	
	public int IdAlmacen
	{
		get { return idAlmacen; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idAlmacen = value;
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
	public CDetalleRemision()
	{
		idDetalleRemision = 0;
		cantidad = 0;
		precioUnitario = 0;
		monto = 0;
		idEncabezadoRemision = 0;
		idProducto = 0;
		idProveedor = 0;
		idEncabezadoFacturaProveedor = 0;
		idDetalleFacturaProveedor = 0;
		idEncabezadoPedido = 0;
		idDetallePedido = 0;
		idProyecto = 0;
		idAlmacen = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CDetalleRemision(int pIdDetalleRemision)
	{
		idDetalleRemision = pIdDetalleRemision;
		cantidad = 0;
		precioUnitario = 0;
		monto = 0;
		idEncabezadoRemision = 0;
		idProducto = 0;
		idProveedor = 0;
		idEncabezadoFacturaProveedor = 0;
		idDetalleFacturaProveedor = 0;
		idEncabezadoPedido = 0;
		idDetallePedido = 0;
		idProyecto = 0;
		idAlmacen = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleRemision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleRemision>(typeof(CDetalleRemision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleRemision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDetalleRemision>(typeof(CDetalleRemision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleRemision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleRemision>(typeof(CDetalleRemision), pConexion);
		foreach (CDetalleRemision O in Obten.ListaRegistros)
		{
			idDetalleRemision = O.IdDetalleRemision;
			cantidad = O.Cantidad;
			precioUnitario = O.PrecioUnitario;
			monto = O.Monto;
			idEncabezadoRemision = O.IdEncabezadoRemision;
			idProducto = O.IdProducto;
			idProveedor = O.IdProveedor;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idEncabezadoPedido = O.IdEncabezadoPedido;
			idDetallePedido = O.IdDetallePedido;
			idProyecto = O.IdProyecto;
			idAlmacen = O.IdAlmacen;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleRemision_ConsultarFiltros";
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
		Obten.Llena<CDetalleRemision>(typeof(CDetalleRemision), pConexion);
		foreach (CDetalleRemision O in Obten.ListaRegistros)
		{
			idDetalleRemision = O.IdDetalleRemision;
			cantidad = O.Cantidad;
			precioUnitario = O.PrecioUnitario;
			monto = O.Monto;
			idEncabezadoRemision = O.IdEncabezadoRemision;
			idProducto = O.IdProducto;
			idProveedor = O.IdProveedor;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idEncabezadoPedido = O.IdEncabezadoPedido;
			idDetallePedido = O.IdDetallePedido;
			idProyecto = O.IdProyecto;
			idAlmacen = O.IdAlmacen;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleRemision_ConsultarFiltros";
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
		Obten.Llena<CDetalleRemision>(typeof(CDetalleRemision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_DetalleRemision_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", 0);
		Agregar.StoredProcedure.Parameters["@pIdDetalleRemision"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoPedido", idEncabezadoPedido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetallePedido", idDetallePedido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDetalleRemision= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDetalleRemision"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_DetalleRemision_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", idDetalleRemision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoPedido", idEncabezadoPedido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetallePedido", idDetallePedido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
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
		Eliminar.StoredProcedure.CommandText = "spb_DetalleRemision_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", idDetalleRemision);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}