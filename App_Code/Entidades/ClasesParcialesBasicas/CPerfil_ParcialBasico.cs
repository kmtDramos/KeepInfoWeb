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

public partial class CPerfil
{
	//Propiedades Privadas
	private int idPerfil;
	private string perfil;
	private int idPagina;
	private bool esPerfilSucursal;
	
	//Propiedades
	public int IdPerfil
	{
		get { return idPerfil; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPerfil = value;
		}
	}
	
	public string Perfil
	{
		get { return perfil; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			perfil = value;
		}
	}
	
	public int IdPagina
	{
		get { return idPagina; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPagina = value;
		}
	}
	
	public bool EsPerfilSucursal
	{
		get { return esPerfilSucursal; }
		set { esPerfilSucursal = value; }
	}
	
	//Constructores
	public CPerfil()
	{
		idPerfil = 0;
		perfil = "";
		idPagina = 0;
		esPerfilSucursal = false;
	}
	
	public CPerfil(int pIdPerfil)
	{
		idPerfil = pIdPerfil;
		perfil = "";
		idPagina = 0;
		esPerfilSucursal = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Perfil_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CPerfil>(typeof(CPerfil), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Perfil_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPerfil>(typeof(CPerfil), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Perfil_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", pIdentificador);
		Obten.Llena<CPerfil>(typeof(CPerfil), pConexion);
		foreach (CPerfil O in Obten.ListaRegistros)
		{
			idPerfil = O.IdPerfil;
			perfil = O.Perfil;
			idPagina = O.IdPagina;
			esPerfilSucursal = O.EsPerfilSucursal;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Perfil_ConsultarFiltros";
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
		Obten.Llena<CPerfil>(typeof(CPerfil), pConexion);
		foreach (CPerfil O in Obten.ListaRegistros)
		{
			idPerfil = O.IdPerfil;
			perfil = O.Perfil;
			idPagina = O.IdPagina;
			esPerfilSucursal = O.EsPerfilSucursal;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Perfil_ConsultarFiltros";
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
		Obten.Llena<CPerfil>(typeof(CPerfil), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Perfil_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", 0);
		Agregar.StoredProcedure.Parameters["@pIdPerfil"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPerfil", perfil);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", idPagina);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsPerfilSucursal", esPerfilSucursal);
		Agregar.Insert(pConexion);
		idPerfil= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdPerfil"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Perfil_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPerfil", perfil);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", idPagina);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsPerfilSucursal", esPerfilSucursal);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Perfil_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Eliminar.Delete(pConexion);
	}
}