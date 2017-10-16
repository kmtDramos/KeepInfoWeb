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

public partial class CUsuarioCuentaBancaria
{
	//Propiedades Privadas
	private int idUsuarioCuentaBancaria;
	private int idUsuario;
	private int idCuentaBancaria;
	private bool puedeVerSaldo;
	private bool baja;
	
	//Propiedades
	public int IdUsuarioCuentaBancaria
	{
		get { return idUsuarioCuentaBancaria; }
		set
		{
			idUsuarioCuentaBancaria = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public int IdCuentaBancaria
	{
		get { return idCuentaBancaria; }
		set
		{
			idCuentaBancaria = value;
		}
	}
	
	public bool PuedeVerSaldo
	{
		get { return puedeVerSaldo; }
		set { puedeVerSaldo = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CUsuarioCuentaBancaria()
	{
		idUsuarioCuentaBancaria = 0;
		idUsuario = 0;
		idCuentaBancaria = 0;
		puedeVerSaldo = false;
		baja = false;
	}
	
	public CUsuarioCuentaBancaria(int pIdUsuarioCuentaBancaria)
	{
		idUsuarioCuentaBancaria = pIdUsuarioCuentaBancaria;
		idUsuario = 0;
		idCuentaBancaria = 0;
		puedeVerSaldo = false;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUsuarioCuentaBancaria>(typeof(CUsuarioCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CUsuarioCuentaBancaria>(typeof(CUsuarioCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCuentaBancaria", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUsuarioCuentaBancaria>(typeof(CUsuarioCuentaBancaria), pConexion);
		foreach (CUsuarioCuentaBancaria O in Obten.ListaRegistros)
		{
			idUsuarioCuentaBancaria = O.IdUsuarioCuentaBancaria;
			idUsuario = O.IdUsuario;
			idCuentaBancaria = O.IdCuentaBancaria;
			puedeVerSaldo = O.PuedeVerSaldo;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_ConsultarFiltros";
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
		Obten.Llena<CUsuarioCuentaBancaria>(typeof(CUsuarioCuentaBancaria), pConexion);
		foreach (CUsuarioCuentaBancaria O in Obten.ListaRegistros)
		{
			idUsuarioCuentaBancaria = O.IdUsuarioCuentaBancaria;
			idUsuario = O.IdUsuario;
			idCuentaBancaria = O.IdCuentaBancaria;
			puedeVerSaldo = O.PuedeVerSaldo;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_ConsultarFiltros";
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
		Obten.Llena<CUsuarioCuentaBancaria>(typeof(CUsuarioCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCuentaBancaria", 0);
		Agregar.StoredProcedure.Parameters["@pIdUsuarioCuentaBancaria"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPuedeVerSaldo", puedeVerSaldo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idUsuarioCuentaBancaria= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdUsuarioCuentaBancaria"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCuentaBancaria", idUsuarioCuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPuedeVerSaldo", puedeVerSaldo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_UsuarioCuentaBancaria_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCuentaBancaria", idUsuarioCuentaBancaria);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
