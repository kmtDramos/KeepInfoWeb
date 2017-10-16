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

public partial class CConceptoProyecto
{
	//Propiedades Privadas
	private int idConceptoProyecto;
	private string descripcion;
	private int ordenConcepto;
	private int idProyecto;
	private int idTipoVenta;
	private int idTipoMoneda;
	private int idUnidadCompraVenta;
	private int idSolicitudFacturacion;
	private decimal monto;
	private int idTipoIVA;
	private decimal iVA;
	private DateTime facturado;
	private decimal cantidad;
	private bool baja;
	
	//Propiedades
	public int IdConceptoProyecto
	{
		get { return idConceptoProyecto; }
		set
		{
			idConceptoProyecto = value;
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
	
	public int OrdenConcepto
	{
		get { return ordenConcepto; }
		set
		{
			ordenConcepto = value;
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
	
	public int IdTipoVenta
	{
		get { return idTipoVenta; }
		set
		{
			idTipoVenta = value;
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
	
	public int IdUnidadCompraVenta
	{
		get { return idUnidadCompraVenta; }
		set
		{
			idUnidadCompraVenta = value;
		}
	}
	
	public int IdSolicitudFacturacion
	{
		get { return idSolicitudFacturacion; }
		set
		{
			idSolicitudFacturacion = value;
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
	
	public DateTime Facturado
	{
		get { return facturado; }
		set { facturado = value; }
	}
	
	public decimal Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CConceptoProyecto()
	{
		idConceptoProyecto = 0;
		descripcion = "";
		ordenConcepto = 0;
		idProyecto = 0;
		idTipoVenta = 0;
		idTipoMoneda = 0;
		idUnidadCompraVenta = 0;
		idSolicitudFacturacion = 0;
		monto = 0;
		idTipoIVA = 0;
		iVA = 0;
		facturado = new DateTime(1, 1, 1);
		cantidad = 0;
		baja = false;
	}
	
	public CConceptoProyecto(int pIdConceptoProyecto)
	{
		idConceptoProyecto = pIdConceptoProyecto;
		descripcion = "";
		ordenConcepto = 0;
		idProyecto = 0;
		idTipoVenta = 0;
		idTipoMoneda = 0;
		idUnidadCompraVenta = 0;
		idSolicitudFacturacion = 0;
		monto = 0;
		idTipoIVA = 0;
		iVA = 0;
		facturado = new DateTime(1, 1, 1);
		cantidad = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ConceptoProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CConceptoProyecto>(typeof(CConceptoProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ConceptoProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CConceptoProyecto>(typeof(CConceptoProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ConceptoProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CConceptoProyecto>(typeof(CConceptoProyecto), pConexion);
		foreach (CConceptoProyecto O in Obten.ListaRegistros)
		{
			idConceptoProyecto = O.IdConceptoProyecto;
			descripcion = O.Descripcion;
			ordenConcepto = O.OrdenConcepto;
			idProyecto = O.IdProyecto;
			idTipoVenta = O.IdTipoVenta;
			idTipoMoneda = O.IdTipoMoneda;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			idSolicitudFacturacion = O.IdSolicitudFacturacion;
			monto = O.Monto;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			facturado = O.Facturado;
			cantidad = O.Cantidad;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ConceptoProyecto_ConsultarFiltros";
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
		Obten.Llena<CConceptoProyecto>(typeof(CConceptoProyecto), pConexion);
		foreach (CConceptoProyecto O in Obten.ListaRegistros)
		{
			idConceptoProyecto = O.IdConceptoProyecto;
			descripcion = O.Descripcion;
			ordenConcepto = O.OrdenConcepto;
			idProyecto = O.IdProyecto;
			idTipoVenta = O.IdTipoVenta;
			idTipoMoneda = O.IdTipoMoneda;
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			idSolicitudFacturacion = O.IdSolicitudFacturacion;
			monto = O.Monto;
			idTipoIVA = O.IdTipoIVA;
			iVA = O.IVA;
			facturado = O.Facturado;
			cantidad = O.Cantidad;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ConceptoProyecto_ConsultarFiltros";
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
		Obten.Llena<CConceptoProyecto>(typeof(CConceptoProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ConceptoProyecto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", 0);
		Agregar.StoredProcedure.Parameters["@pIdConceptoProyecto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenConcepto", ordenConcepto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoVenta", idTipoVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", idSolicitudFacturacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		if(facturado.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idConceptoProyecto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdConceptoProyecto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ConceptoProyecto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", idConceptoProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenConcepto", ordenConcepto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoVenta", idTipoVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", idSolicitudFacturacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		if(facturado.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ConceptoProyecto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", idConceptoProyecto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
