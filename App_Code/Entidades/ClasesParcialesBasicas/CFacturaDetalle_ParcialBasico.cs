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

public partial class CFacturaDetalle
{
	//Propiedades Privadas
	private int idFacturaDetalle;
	private string clave;
	private string descripcion;
	private decimal precioUnitario;
	private decimal iVA;
	private decimal total;
	private decimal descuento;
	private int idProducto;
	private int idServicio;
	private int idFacturaEncabezado;
	private decimal costo;
	private int idCotizacion;
	private int idCotizacionDetalle;
	private bool sinDocumentacion;
	private int idProyecto;
	private int idConceptoProyecto;
	private int idEncabezadoRemision;
	private int idDetalleRemision;
	private int idTipoIVA;
	private decimal cantidad;
	private string descripcionAgregada;
	private string claveProdServ;
	private bool baja;
	
	//Propiedades
	public int IdFacturaDetalle
	{
		get { return idFacturaDetalle; }
		set
		{
			idFacturaDetalle = value;
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
	
	public decimal PrecioUnitario
	{
		get { return precioUnitario; }
		set
		{
			precioUnitario = value;
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
	
	public decimal Descuento
	{
		get { return descuento; }
		set
		{
			descuento = value;
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
	
	public int IdFacturaEncabezado
	{
		get { return idFacturaEncabezado; }
		set
		{
			idFacturaEncabezado = value;
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
	
	public int IdCotizacion
	{
		get { return idCotizacion; }
		set
		{
			idCotizacion = value;
		}
	}
	
	public int IdCotizacionDetalle
	{
		get { return idCotizacionDetalle; }
		set
		{
			idCotizacionDetalle = value;
		}
	}
	
	public bool SinDocumentacion
	{
		get { return sinDocumentacion; }
		set { sinDocumentacion = value; }
	}
	
	public int IdProyecto
	{
		get { return idProyecto; }
		set
		{
			idProyecto = value;
		}
	}
	
	public int IdConceptoProyecto
	{
		get { return idConceptoProyecto; }
		set
		{
			idConceptoProyecto = value;
		}
	}
	
	public int IdEncabezadoRemision
	{
		get { return idEncabezadoRemision; }
		set
		{
			idEncabezadoRemision = value;
		}
	}
	
	public int IdDetalleRemision
	{
		get { return idDetalleRemision; }
		set
		{
			idDetalleRemision = value;
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
	
	public decimal Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
		}
	}
	
	public string DescripcionAgregada
	{
		get { return descripcionAgregada; }
		set
		{
			descripcionAgregada = value;
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
	public CFacturaDetalle()
	{
		idFacturaDetalle = 0;
		clave = "";
		descripcion = "";
		precioUnitario = 0;
		iVA = 0;
		total = 0;
		descuento = 0;
		idProducto = 0;
		idServicio = 0;
		idFacturaEncabezado = 0;
		costo = 0;
		idCotizacion = 0;
		idCotizacionDetalle = 0;
		sinDocumentacion = false;
		idProyecto = 0;
		idConceptoProyecto = 0;
		idEncabezadoRemision = 0;
		idDetalleRemision = 0;
		idTipoIVA = 0;
		cantidad = 0;
		descripcionAgregada = "";
		claveProdServ = "";
		baja = false;
	}
	
	public CFacturaDetalle(int pIdFacturaDetalle)
	{
		idFacturaDetalle = pIdFacturaDetalle;
		clave = "";
		descripcion = "";
		precioUnitario = 0;
		iVA = 0;
		total = 0;
		descuento = 0;
		idProducto = 0;
		idServicio = 0;
		idFacturaEncabezado = 0;
		costo = 0;
		idCotizacion = 0;
		idCotizacionDetalle = 0;
		sinDocumentacion = false;
		idProyecto = 0;
		idConceptoProyecto = 0;
		idEncabezadoRemision = 0;
		idDetalleRemision = 0;
		idTipoIVA = 0;
		cantidad = 0;
		descripcionAgregada = "";
		claveProdServ = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFacturaDetalle>(typeof(CFacturaDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CFacturaDetalle>(typeof(CFacturaDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaDetalle_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFacturaDetalle>(typeof(CFacturaDetalle), pConexion);
		foreach (CFacturaDetalle O in Obten.ListaRegistros)
		{
			idFacturaDetalle = O.IdFacturaDetalle;
			clave = O.Clave;
			descripcion = O.Descripcion;
			precioUnitario = O.PrecioUnitario;
			iVA = O.IVA;
			total = O.Total;
			descuento = O.Descuento;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idFacturaEncabezado = O.IdFacturaEncabezado;
			costo = O.Costo;
			idCotizacion = O.IdCotizacion;
			idCotizacionDetalle = O.IdCotizacionDetalle;
			sinDocumentacion = O.SinDocumentacion;
			idProyecto = O.IdProyecto;
			idConceptoProyecto = O.IdConceptoProyecto;
			idEncabezadoRemision = O.IdEncabezadoRemision;
			idDetalleRemision = O.IdDetalleRemision;
			idTipoIVA = O.IdTipoIVA;
			cantidad = O.Cantidad;
			descripcionAgregada = O.DescripcionAgregada;
			claveProdServ = O.ClaveProdServ;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaDetalle_ConsultarFiltros";
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
		Obten.Llena<CFacturaDetalle>(typeof(CFacturaDetalle), pConexion);
		foreach (CFacturaDetalle O in Obten.ListaRegistros)
		{
			idFacturaDetalle = O.IdFacturaDetalle;
			clave = O.Clave;
			descripcion = O.Descripcion;
			precioUnitario = O.PrecioUnitario;
			iVA = O.IVA;
			total = O.Total;
			descuento = O.Descuento;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			idFacturaEncabezado = O.IdFacturaEncabezado;
			costo = O.Costo;
			idCotizacion = O.IdCotizacion;
			idCotizacionDetalle = O.IdCotizacionDetalle;
			sinDocumentacion = O.SinDocumentacion;
			idProyecto = O.IdProyecto;
			idConceptoProyecto = O.IdConceptoProyecto;
			idEncabezadoRemision = O.IdEncabezadoRemision;
			idDetalleRemision = O.IdDetalleRemision;
			idTipoIVA = O.IdTipoIVA;
			cantidad = O.Cantidad;
			descripcionAgregada = O.DescripcionAgregada;
			claveProdServ = O.ClaveProdServ;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaDetalle_ConsultarFiltros";
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
		Obten.Llena<CFacturaDetalle>(typeof(CFacturaDetalle), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_FacturaDetalle_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", 0);
		Agregar.StoredProcedure.Parameters["@pIdFacturaDetalle"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", idCotizacionDetalle);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSinDocumentacion", sinDocumentacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", idConceptoProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", idDetalleRemision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcionAgregada", descripcionAgregada);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveProdServ", claveProdServ);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idFacturaDetalle= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFacturaDetalle"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_FacturaDetalle_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", idFacturaDetalle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", idCotizacionDetalle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSinDocumentacion", sinDocumentacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", idConceptoProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", idDetalleRemision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcionAgregada", descripcionAgregada);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveProdServ", claveProdServ);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_FacturaDetalle_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", idFacturaDetalle);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
