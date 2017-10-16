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

public partial class CNivelInteresOportunidad
{
	//Propiedades Privadas
	private int idNivelInteresOportunidad;
	private string nivelInteresOportunidad;
	private int orden;
	private string abreviatura;
	private bool baja;
	
	//Propiedades
	public int IdNivelInteresOportunidad
	{
		get { return idNivelInteresOportunidad; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNivelInteresOportunidad = value;
		}
	}
	
	public string NivelInteresOportunidad
	{
		get { return nivelInteresOportunidad; }
		set
		{
			nivelInteresOportunidad = value;
		}
	}
	
	public int Orden
	{
		get { return orden; }
		set
		{
			if (value < 0)
			{
				return;
			}
			orden = value;
		}
	}
	
	public string Abreviatura
	{
		get { return abreviatura; }
		set
		{
			abreviatura = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CNivelInteresOportunidad()
	{
		idNivelInteresOportunidad = 0;
		nivelInteresOportunidad = "";
		orden = 0;
		abreviatura = "";
		baja = false;
	}
	
	public CNivelInteresOportunidad(int pIdNivelInteresOportunidad)
	{
		idNivelInteresOportunidad = pIdNivelInteresOportunidad;
		nivelInteresOportunidad = "";
		orden = 0;
		abreviatura = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNivelInteresOportunidad>(typeof(CNivelInteresOportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CNivelInteresOportunidad>(typeof(CNivelInteresOportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresOportunidad", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNivelInteresOportunidad>(typeof(CNivelInteresOportunidad), pConexion);
		foreach (CNivelInteresOportunidad O in Obten.ListaRegistros)
		{
			idNivelInteresOportunidad = O.IdNivelInteresOportunidad;
			nivelInteresOportunidad = O.NivelInteresOportunidad;
			orden = O.Orden;
			abreviatura = O.Abreviatura;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_ConsultarFiltros";
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
		Obten.Llena<CNivelInteresOportunidad>(typeof(CNivelInteresOportunidad), pConexion);
		foreach (CNivelInteresOportunidad O in Obten.ListaRegistros)
		{
			idNivelInteresOportunidad = O.IdNivelInteresOportunidad;
			nivelInteresOportunidad = O.NivelInteresOportunidad;
			orden = O.Orden;
			abreviatura = O.Abreviatura;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_ConsultarFiltros";
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
		Obten.Llena<CNivelInteresOportunidad>(typeof(CNivelInteresOportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresOportunidad", 0);
		Agregar.StoredProcedure.Parameters["@pIdNivelInteresOportunidad"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNivelInteresOportunidad", nivelInteresOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idNivelInteresOportunidad= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNivelInteresOportunidad"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresOportunidad", idNivelInteresOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNivelInteresOportunidad", nivelInteresOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_NivelInteresOportunidad_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresOportunidad", idNivelInteresOportunidad);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}