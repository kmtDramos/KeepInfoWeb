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

public partial class CClienteRelacionEstatus
{
	//Propiedades Privadas
	private int idClienteRelacionEstatus;
	private string clienteRelacionEstatus;
	
	//Propiedades
	public int IdClienteRelacionEstatus
	{
		get { return idClienteRelacionEstatus; }
		set
		{
			idClienteRelacionEstatus = value;
		}
	}
	
	public string ClienteRelacionEstatus
	{
		get { return clienteRelacionEstatus; }
		set
		{
			clienteRelacionEstatus = value;
		}
	}
	
	//Constructores
	public CClienteRelacionEstatus()
	{
		idClienteRelacionEstatus = 0;
		clienteRelacionEstatus = "";
	}
	
	public CClienteRelacionEstatus(int pIdClienteRelacionEstatus)
	{
		idClienteRelacionEstatus = pIdClienteRelacionEstatus;
		clienteRelacionEstatus = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CClienteRelacionEstatus>(typeof(CClienteRelacionEstatus), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CClienteRelacionEstatus>(typeof(CClienteRelacionEstatus), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionEstatus", pIdentificador);
		Obten.Llena<CClienteRelacionEstatus>(typeof(CClienteRelacionEstatus), pConexion);
		foreach (CClienteRelacionEstatus O in Obten.ListaRegistros)
		{
			idClienteRelacionEstatus = O.IdClienteRelacionEstatus;
			clienteRelacionEstatus = O.ClienteRelacionEstatus;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_ConsultarFiltros";
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
		Obten.Llena<CClienteRelacionEstatus>(typeof(CClienteRelacionEstatus), pConexion);
		foreach (CClienteRelacionEstatus O in Obten.ListaRegistros)
		{
			idClienteRelacionEstatus = O.IdClienteRelacionEstatus;
			clienteRelacionEstatus = O.ClienteRelacionEstatus;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_ConsultarFiltros";
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
		Obten.Llena<CClienteRelacionEstatus>(typeof(CClienteRelacionEstatus), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionEstatus", 0);
		Agregar.StoredProcedure.Parameters["@pIdClienteRelacionEstatus"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClienteRelacionEstatus", clienteRelacionEstatus);
		Agregar.Insert(pConexion);
		idClienteRelacionEstatus= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdClienteRelacionEstatus"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionEstatus", idClienteRelacionEstatus);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClienteRelacionEstatus", clienteRelacionEstatus);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ClienteRelacionEstatus_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionEstatus", idClienteRelacionEstatus);
		Eliminar.Delete(pConexion);
	}
}
