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

public partial class CSucursalAsignada
{
	//Propiedades Privadas
	private int idSucursalAsignada;
	private int idUsuario;
	private int idSucursal;
	private int idPerfil;
	private bool baja;
	
	//Propiedades
	public int IdSucursalAsignada
	{
		get { return idSucursalAsignada; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursalAsignada = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuario = value;
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
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSucursalAsignada()
	{
		idSucursalAsignada = 0;
		idUsuario = 0;
		idSucursal = 0;
		idPerfil = 0;
		baja = false;
	}
	
	public CSucursalAsignada(int pIdSucursalAsignada)
	{
		idSucursalAsignada = pIdSucursalAsignada;
		idUsuario = 0;
		idSucursal = 0;
		idPerfil = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAsignada_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursalAsignada>(typeof(CSucursalAsignada), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAsignada_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSucursalAsignada>(typeof(CSucursalAsignada), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAsignada_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAsignada", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursalAsignada>(typeof(CSucursalAsignada), pConexion);
		foreach (CSucursalAsignada O in Obten.ListaRegistros)
		{
			idSucursalAsignada = O.IdSucursalAsignada;
			idUsuario = O.IdUsuario;
			idSucursal = O.IdSucursal;
			idPerfil = O.IdPerfil;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAsignada_ConsultarFiltros";
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
		Obten.Llena<CSucursalAsignada>(typeof(CSucursalAsignada), pConexion);
		foreach (CSucursalAsignada O in Obten.ListaRegistros)
		{
			idSucursalAsignada = O.IdSucursalAsignada;
			idUsuario = O.IdUsuario;
			idSucursal = O.IdSucursal;
			idPerfil = O.IdPerfil;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAsignada_ConsultarFiltros";
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
		Obten.Llena<CSucursalAsignada>(typeof(CSucursalAsignada), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SucursalAsignada_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAsignada", 0);
		Agregar.StoredProcedure.Parameters["@pIdSucursalAsignada"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSucursalAsignada= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSucursalAsignada"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SucursalAsignada_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAsignada", idSucursalAsignada);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SucursalAsignada_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAsignada", idSucursalAsignada);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}