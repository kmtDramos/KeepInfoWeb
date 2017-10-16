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

public partial class CUnidadCaracteristica
{
	//Propiedades Privadas
	private int idUnidadCaracteristica;
	private string unidadCaracteristica;
	private bool baja;
	
	//Propiedades
	public int IdUnidadCaracteristica
	{
		get { return idUnidadCaracteristica; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUnidadCaracteristica = value;
		}
	}
	
	public string UnidadCaracteristica
	{
		get { return unidadCaracteristica; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			unidadCaracteristica = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CUnidadCaracteristica()
	{
		idUnidadCaracteristica = 0;
		unidadCaracteristica = "";
		baja = false;
	}
	
	public CUnidadCaracteristica(int pIdUnidadCaracteristica)
	{
		idUnidadCaracteristica = pIdUnidadCaracteristica;
		unidadCaracteristica = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCaracteristica_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUnidadCaracteristica>(typeof(CUnidadCaracteristica), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCaracteristica_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CUnidadCaracteristica>(typeof(CUnidadCaracteristica), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCaracteristica_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCaracteristica", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUnidadCaracteristica>(typeof(CUnidadCaracteristica), pConexion);
		foreach (CUnidadCaracteristica O in Obten.ListaRegistros)
		{
			idUnidadCaracteristica = O.IdUnidadCaracteristica;
			unidadCaracteristica = O.UnidadCaracteristica;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCaracteristica_ConsultarFiltros";
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
		Obten.Llena<CUnidadCaracteristica>(typeof(CUnidadCaracteristica), pConexion);
		foreach (CUnidadCaracteristica O in Obten.ListaRegistros)
		{
			idUnidadCaracteristica = O.IdUnidadCaracteristica;
			unidadCaracteristica = O.UnidadCaracteristica;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UnidadCaracteristica_ConsultarFiltros";
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
		Obten.Llena<CUnidadCaracteristica>(typeof(CUnidadCaracteristica), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_UnidadCaracteristica_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCaracteristica", 0);
		Agregar.StoredProcedure.Parameters["@pIdUnidadCaracteristica"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUnidadCaracteristica", unidadCaracteristica);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idUnidadCaracteristica= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdUnidadCaracteristica"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_UnidadCaracteristica_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCaracteristica", idUnidadCaracteristica);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUnidadCaracteristica", unidadCaracteristica);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_UnidadCaracteristica_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCaracteristica", idUnidadCaracteristica);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}