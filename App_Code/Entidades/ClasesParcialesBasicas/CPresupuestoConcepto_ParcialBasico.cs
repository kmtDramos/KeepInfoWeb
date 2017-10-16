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

public partial class CPresupuestoConcepto
{
	//Propiedades Privadas
	private int idPresupuestoConcepto;
	private int idDivision;
	private int idPresupuesto;
	private int idProducto;
	private int idServicio;
	private int idTipoMoneda;
	private int idUsuarioAgente;
	private string clave;
	private string descripcion;
	private decimal costo;
	private string proveedor;
	private decimal precioUnitario;
	private decimal cantidad;
	private decimal total;
	private decimal tipoCambio;
	private decimal utilidad;
	private decimal orden;
	private decimal descuento;
	private decimal manoObra;
	private decimal iVA;
	private decimal margen;
	private decimal comision;
	private decimal facturacionCantidad;
	private bool baja;
	
	//Propiedades
	public int IdPresupuestoConcepto
	{
		get { return idPresupuestoConcepto; }
		set
		{
			idPresupuestoConcepto = value;
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
	
	public int IdPresupuesto
	{
		get { return idPresupuesto; }
		set
		{
			idPresupuesto = value;
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
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			idTipoMoneda = value;
		}
	}
	
	public int IdUsuarioAgente
	{
		get { return idUsuarioAgente; }
		set
		{
			idUsuarioAgente = value;
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
	
	public decimal Costo
	{
		get { return costo; }
		set
		{
			costo = value;
		}
	}
	
	public string Proveedor
	{
		get { return proveedor; }
		set
		{
			proveedor = value;
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
	
	public decimal Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
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
	
	public decimal TipoCambio
	{
		get { return tipoCambio; }
		set
		{
			tipoCambio = value;
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
	
	public decimal Orden
	{
		get { return orden; }
		set
		{
			orden = value;
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
	
	public decimal ManoObra
	{
		get { return manoObra; }
		set
		{
			manoObra = value;
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
	
	public decimal Margen
	{
		get { return margen; }
		set
		{
			margen = value;
		}
	}
	
	public decimal Comision
	{
		get { return comision; }
		set
		{
			comision = value;
		}
	}
	
	public decimal FacturacionCantidad
	{
		get { return facturacionCantidad; }
		set
		{
			facturacionCantidad = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CPresupuestoConcepto()
	{
		idPresupuestoConcepto = 0;
		idDivision = 0;
		idPresupuesto = 0;
		idProducto = 0;
		idServicio = 0;
		idTipoMoneda = 0;
		idUsuarioAgente = 0;
		clave = "";
		descripcion = "";
		costo = 0;
		proveedor = "";
		precioUnitario = 0;
		cantidad = 0;
		total = 0;
		tipoCambio = 0;
		utilidad = 0;
		orden = 0;
		descuento = 0;
		manoObra = 0;
		iVA = 0;
		margen = 0;
		comision = 0;
		facturacionCantidad = 0;
		baja = false;
	}
	
	public CPresupuestoConcepto(int pIdPresupuestoConcepto)
	{
		idPresupuestoConcepto = pIdPresupuestoConcepto;
		idDivision = 0;
		idPresupuesto = 0;
		idProducto = 0;
		idServicio = 0;
		idTipoMoneda = 0;
		idUsuarioAgente = 0;
		clave = "";
		descripcion = "";
		costo = 0;
		proveedor = "";
		precioUnitario = 0;
		cantidad = 0;
		total = 0;
		tipoCambio = 0;
		utilidad = 0;
		orden = 0;
		descuento = 0;
		manoObra = 0;
		iVA = 0;
		margen = 0;
		comision = 0;
		facturacionCantidad = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoConcepto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPresupuestoConcepto>(typeof(CPresupuestoConcepto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoConcepto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPresupuestoConcepto>(typeof(CPresupuestoConcepto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoConcepto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPresupuestoConcepto>(typeof(CPresupuestoConcepto), pConexion);
		foreach (CPresupuestoConcepto O in Obten.ListaRegistros)
		{
			idPresupuestoConcepto = O.IdPresupuestoConcepto;
			idDivision = O.IdDivision;
			idPresupuesto = O.IdPresupuesto;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idTipoMoneda = O.IdTipoMoneda;
			idUsuarioAgente = O.IdUsuarioAgente;
			clave = O.Clave;
			descripcion = O.Descripcion;
			costo = O.Costo;
			proveedor = O.Proveedor;
			precioUnitario = O.PrecioUnitario;
			cantidad = O.Cantidad;
			total = O.Total;
			tipoCambio = O.TipoCambio;
			utilidad = O.Utilidad;
			orden = O.Orden;
			descuento = O.Descuento;
			manoObra = O.ManoObra;
			iVA = O.IVA;
			margen = O.Margen;
			comision = O.Comision;
			facturacionCantidad = O.FacturacionCantidad;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoConcepto_ConsultarFiltros";
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
		Obten.Llena<CPresupuestoConcepto>(typeof(CPresupuestoConcepto), pConexion);
		foreach (CPresupuestoConcepto O in Obten.ListaRegistros)
		{
			idPresupuestoConcepto = O.IdPresupuestoConcepto;
			idDivision = O.IdDivision;
			idPresupuesto = O.IdPresupuesto;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idTipoMoneda = O.IdTipoMoneda;
			idUsuarioAgente = O.IdUsuarioAgente;
			clave = O.Clave;
			descripcion = O.Descripcion;
			costo = O.Costo;
			proveedor = O.Proveedor;
			precioUnitario = O.PrecioUnitario;
			cantidad = O.Cantidad;
			total = O.Total;
			tipoCambio = O.TipoCambio;
			utilidad = O.Utilidad;
			orden = O.Orden;
			descuento = O.Descuento;
			manoObra = O.ManoObra;
			iVA = O.IVA;
			margen = O.Margen;
			comision = O.Comision;
			facturacionCantidad = O.FacturacionCantidad;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoConcepto_ConsultarFiltros";
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
		Obten.Llena<CPresupuestoConcepto>(typeof(CPresupuestoConcepto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_PresupuestoConcepto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", 0);
		Agregar.StoredProcedure.Parameters["@pIdPresupuestoConcepto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProveedor", proveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pManoObra", manoObra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargen", margen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComision", comision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturacionCantidad", facturacionCantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idPresupuestoConcepto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdPresupuestoConcepto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_PresupuestoConcepto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", idPresupuestoConcepto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProveedor", proveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pManoObra", manoObra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMargen", margen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComision", comision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFacturacionCantidad", facturacionCantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_PresupuestoConcepto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", idPresupuestoConcepto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
