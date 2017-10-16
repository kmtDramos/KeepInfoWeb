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

public partial class CSucursalAccesoAlmacen
{
	//Propiedades Privadas
	private int idSucursalAccesoAlmacen;
	private int idSucursal;
	private int idAlmacen;
	private bool baja;
	
	//Propiedades
	public int IdSucursalAccesoAlmacen
	{
		get { return idSucursalAccesoAlmacen; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursalAccesoAlmacen = value;
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
	
	public int IdAlmacen
	{
		get { return idAlmacen; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idAlmacen = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSucursalAccesoAlmacen()
	{
		idSucursalAccesoAlmacen = 0;
		idSucursal = 0;
		idAlmacen = 0;
		baja = false;
	}
	
	public CSucursalAccesoAlmacen(int pIdSucursalAccesoAlmacen)
	{
		idSucursalAccesoAlmacen = pIdSucursalAccesoAlmacen;
		idSucursal = 0;
		idAlmacen = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursalAccesoAlmacen>(typeof(CSucursalAccesoAlmacen), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSucursalAccesoAlmacen>(typeof(CSucursalAccesoAlmacen), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAccesoAlmacen", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursalAccesoAlmacen>(typeof(CSucursalAccesoAlmacen), pConexion);
		foreach (CSucursalAccesoAlmacen O in Obten.ListaRegistros)
		{
			idSucursalAccesoAlmacen = O.IdSucursalAccesoAlmacen;
			idSucursal = O.IdSucursal;
			idAlmacen = O.IdAlmacen;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_ConsultarFiltros";
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
		Obten.Llena<CSucursalAccesoAlmacen>(typeof(CSucursalAccesoAlmacen), pConexion);
		foreach (CSucursalAccesoAlmacen O in Obten.ListaRegistros)
		{
			idSucursalAccesoAlmacen = O.IdSucursalAccesoAlmacen;
			idSucursal = O.IdSucursal;
			idAlmacen = O.IdAlmacen;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_ConsultarFiltros";
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
		Obten.Llena<CSucursalAccesoAlmacen>(typeof(CSucursalAccesoAlmacen), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAccesoAlmacen", 0);
		Agregar.StoredProcedure.Parameters["@pIdSucursalAccesoAlmacen"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSucursalAccesoAlmacen= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSucursalAccesoAlmacen"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAccesoAlmacen", idSucursalAccesoAlmacen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SucursalAccesoAlmacen_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAccesoAlmacen", idSucursalAccesoAlmacen);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}