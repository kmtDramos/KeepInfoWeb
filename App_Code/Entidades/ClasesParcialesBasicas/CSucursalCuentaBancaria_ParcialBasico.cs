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

public partial class CSucursalCuentaBancaria
{
	//Propiedades Privadas
	private int idSucursalCuentaBancaria;
	private int idSucursal;
	private int idCuentaBancaria;
	private bool baja;
	
	//Propiedades
	public int IdSucursalCuentaBancaria
	{
		get { return idSucursalCuentaBancaria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursalCuentaBancaria = value;
		}
	}
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursal = value;
		}
	}
	
	public int IdCuentaBancaria
	{
		get { return idCuentaBancaria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCuentaBancaria = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSucursalCuentaBancaria()
	{
		idSucursalCuentaBancaria = 0;
		idSucursal = 0;
		idCuentaBancaria = 0;
		baja = false;
	}
	
	public CSucursalCuentaBancaria(int pIdSucursalCuentaBancaria)
	{
		idSucursalCuentaBancaria = pIdSucursalCuentaBancaria;
		idSucursal = 0;
		idCuentaBancaria = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursalCuentaBancaria>(typeof(CSucursalCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSucursalCuentaBancaria>(typeof(CSucursalCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSucursalCuentaBancaria", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursalCuentaBancaria>(typeof(CSucursalCuentaBancaria), pConexion);
		foreach (CSucursalCuentaBancaria O in Obten.ListaRegistros)
		{
			idSucursalCuentaBancaria = O.IdSucursalCuentaBancaria;
			idSucursal = O.IdSucursal;
			idCuentaBancaria = O.IdCuentaBancaria;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_ConsultarFiltros";
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
		Obten.Llena<CSucursalCuentaBancaria>(typeof(CSucursalCuentaBancaria), pConexion);
		foreach (CSucursalCuentaBancaria O in Obten.ListaRegistros)
		{
			idSucursalCuentaBancaria = O.IdSucursalCuentaBancaria;
			idSucursal = O.IdSucursal;
			idCuentaBancaria = O.IdCuentaBancaria;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_ConsultarFiltros";
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
		Obten.Llena<CSucursalCuentaBancaria>(typeof(CSucursalCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalCuentaBancaria", 0);
		Agregar.StoredProcedure.Parameters["@pIdSucursalCuentaBancaria"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSucursalCuentaBancaria= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSucursalCuentaBancaria"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalCuentaBancaria", idSucursalCuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SucursalCuentaBancaria_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalCuentaBancaria", idSucursalCuentaBancaria);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}