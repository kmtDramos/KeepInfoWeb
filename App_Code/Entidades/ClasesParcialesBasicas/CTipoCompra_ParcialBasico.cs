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

public partial class CTipoCompra
{
	//Propiedades Privadas
	private int idTipoCompra;
	private string tipoCompra;
	private string claveCuentaContable;
	private bool baja;
	
	//Propiedades
	public int IdTipoCompra
	{
		get { return idTipoCompra; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoCompra = value;
		}
	}
	
	public string TipoCompra
	{
		get { return tipoCompra; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			tipoCompra = value;
		}
	}
	
	public string ClaveCuentaContable
	{
		get { return claveCuentaContable; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			claveCuentaContable = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CTipoCompra()
	{
		idTipoCompra = 0;
		tipoCompra = "";
		claveCuentaContable = "";
		baja = false;
	}
	
	public CTipoCompra(int pIdTipoCompra)
	{
		idTipoCompra = pIdTipoCompra;
		tipoCompra = "";
		claveCuentaContable = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCompra_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTipoCompra>(typeof(CTipoCompra), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCompra_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTipoCompra>(typeof(CTipoCompra), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCompra_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTipoCompra>(typeof(CTipoCompra), pConexion);
		foreach (CTipoCompra O in Obten.ListaRegistros)
		{
			idTipoCompra = O.IdTipoCompra;
			tipoCompra = O.TipoCompra;
			claveCuentaContable = O.ClaveCuentaContable;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCompra_ConsultarFiltros";
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
		Obten.Llena<CTipoCompra>(typeof(CTipoCompra), pConexion);
		foreach (CTipoCompra O in Obten.ListaRegistros)
		{
			idTipoCompra = O.IdTipoCompra;
			tipoCompra = O.TipoCompra;
			claveCuentaContable = O.ClaveCuentaContable;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCompra_ConsultarFiltros";
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
		Obten.Llena<CTipoCompra>(typeof(CTipoCompra), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TipoCompra_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", 0);
		Agregar.StoredProcedure.Parameters["@pIdTipoCompra"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCompra", tipoCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idTipoCompra= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoCompra"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TipoCompra_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCompra", tipoCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TipoCompra_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}