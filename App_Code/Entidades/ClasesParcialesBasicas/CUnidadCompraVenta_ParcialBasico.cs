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

public partial class CUnidadCompraVenta
{
	//Propiedades Privadas
	private int idUnidadCompraVenta;
	private string unidadCompraVenta;
	private string claveUnidad;
	private bool baja;
	
	//Propiedades
	public int IdUnidadCompraVenta
	{
		get { return idUnidadCompraVenta; }
		set
		{
			idUnidadCompraVenta = value;
		}
	}
	
	public string UnidadCompraVenta
	{
		get { return unidadCompraVenta; }
		set
		{
			unidadCompraVenta = value;
		}
	}
	
	public string ClaveUnidad
	{
		get { return claveUnidad; }
		set
		{
			claveUnidad = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CUnidadCompraVenta()
	{
		idUnidadCompraVenta = 0;
		unidadCompraVenta = "";
		claveUnidad = "";
		baja = false;
	}
	
	public CUnidadCompraVenta(int pIdUnidadCompraVenta)
	{
		idUnidadCompraVenta = pIdUnidadCompraVenta;
		unidadCompraVenta = "";
		claveUnidad = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCompraVenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUnidadCompraVenta>(typeof(CUnidadCompraVenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCompraVenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CUnidadCompraVenta>(typeof(CUnidadCompraVenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCompraVenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUnidadCompraVenta>(typeof(CUnidadCompraVenta), pConexion);
		foreach (CUnidadCompraVenta O in Obten.ListaRegistros)
		{
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			unidadCompraVenta = O.UnidadCompraVenta;
			claveUnidad = O.ClaveUnidad;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCompraVenta_ConsultarFiltros";
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
		Obten.Llena<CUnidadCompraVenta>(typeof(CUnidadCompraVenta), pConexion);
		foreach (CUnidadCompraVenta O in Obten.ListaRegistros)
		{
			idUnidadCompraVenta = O.IdUnidadCompraVenta;
			unidadCompraVenta = O.UnidadCompraVenta;
			claveUnidad = O.ClaveUnidad;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCompraVenta_ConsultarFiltros";
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
		Obten.Llena<CUnidadCompraVenta>(typeof(CUnidadCompraVenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_UnidadCompraVenta_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", 0);
		Agregar.StoredProcedure.Parameters["@pIdUnidadCompraVenta"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUnidadCompraVenta", unidadCompraVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveUnidad", claveUnidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idUnidadCompraVenta= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdUnidadCompraVenta"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_UnidadCompraVenta_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUnidadCompraVenta", unidadCompraVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveUnidad", claveUnidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_UnidadCompraVenta_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
