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

public partial class CLocalidad
{
	//Propiedades Privadas
	private int idLocalidad;
	private string localidad;
	private string clave;
	private string latitud;
	private string longitud;
	private string altitud;
	private int idMunicipio;
	private bool baja;
	
	//Propiedades
	public int IdLocalidad
	{
		get { return idLocalidad; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idLocalidad = value;
		}
	}
	
	public string Localidad
	{
		get { return localidad; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			localidad = value;
		}
	}
	
	public string Clave
	{
		get { return clave; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			clave = value;
		}
	}
	
	public string Latitud
	{
		get { return latitud; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			latitud = value;
		}
	}
	
	public string Longitud
	{
		get { return longitud; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			longitud = value;
		}
	}
	
	public string Altitud
	{
		get { return altitud; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			altitud = value;
		}
	}
	
	public int IdMunicipio
	{
		get { return idMunicipio; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idMunicipio = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CLocalidad()
	{
		idLocalidad = 0;
		localidad = "";
		clave = "";
		latitud = "";
		longitud = "";
		altitud = "";
		idMunicipio = 0;
		baja = false;
	}
	
	public CLocalidad(int pIdLocalidad)
	{
		idLocalidad = pIdLocalidad;
		localidad = "";
		clave = "";
		latitud = "";
		longitud = "";
		altitud = "";
		idMunicipio = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Localidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CLocalidad>(typeof(CLocalidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Localidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CLocalidad>(typeof(CLocalidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Localidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CLocalidad>(typeof(CLocalidad), pConexion);
		foreach (CLocalidad O in Obten.ListaRegistros)
		{
			idLocalidad = O.IdLocalidad;
			localidad = O.Localidad;
			clave = O.Clave;
			latitud = O.Latitud;
			longitud = O.Longitud;
			altitud = O.Altitud;
			idMunicipio = O.IdMunicipio;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Localidad_ConsultarFiltros";
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
		Obten.Llena<CLocalidad>(typeof(CLocalidad), pConexion);
		foreach (CLocalidad O in Obten.ListaRegistros)
		{
			idLocalidad = O.IdLocalidad;
			localidad = O.Localidad;
			clave = O.Clave;
			latitud = O.Latitud;
			longitud = O.Longitud;
			altitud = O.Altitud;
			idMunicipio = O.IdMunicipio;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Localidad_ConsultarFiltros";
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
		Obten.Llena<CLocalidad>(typeof(CLocalidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Localidad_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", 0);
		Agregar.StoredProcedure.Parameters["@pIdLocalidad"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLocalidad", localidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLatitud", latitud);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLongitud", longitud);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAltitud", altitud);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idLocalidad= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdLocalidad"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Localidad_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLocalidad", localidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLatitud", latitud);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLongitud", longitud);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAltitud", altitud);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Localidad_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}