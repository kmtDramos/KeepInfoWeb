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

public partial class CEstatusProspeccion
{
	//Propiedades Privadas
	private int idEstatusProspeccion;
	private string estatusProspeccion;
	private int idEtapaProspeccion;
	private bool baja;
	
	//Propiedades
	public int IdEstatusProspeccion
	{
		get { return idEstatusProspeccion; }
		set
		{
			idEstatusProspeccion = value;
		}
	}
	
	public string EstatusProspeccion
	{
		get { return estatusProspeccion; }
		set
		{
			estatusProspeccion = value;
		}
	}
	
	public int IdEtapaProspeccion
	{
		get { return idEtapaProspeccion; }
		set
		{
			idEtapaProspeccion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CEstatusProspeccion()
	{
		idEstatusProspeccion = 0;
		estatusProspeccion = "";
		idEtapaProspeccion = 0;
		baja = false;
	}
	
	public CEstatusProspeccion(int pIdEstatusProspeccion)
	{
		idEstatusProspeccion = pIdEstatusProspeccion;
		estatusProspeccion = "";
		idEtapaProspeccion = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstatusProspeccion>(typeof(CEstatusProspeccion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CEstatusProspeccion>(typeof(CEstatusProspeccion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstatusProspeccion>(typeof(CEstatusProspeccion), pConexion);
		foreach (CEstatusProspeccion O in Obten.ListaRegistros)
		{
			idEstatusProspeccion = O.IdEstatusProspeccion;
			estatusProspeccion = O.EstatusProspeccion;
			idEtapaProspeccion = O.IdEtapaProspeccion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccion_ConsultarFiltros";
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
		Obten.Llena<CEstatusProspeccion>(typeof(CEstatusProspeccion), pConexion);
		foreach (CEstatusProspeccion O in Obten.ListaRegistros)
		{
			idEstatusProspeccion = O.IdEstatusProspeccion;
			estatusProspeccion = O.EstatusProspeccion;
			idEtapaProspeccion = O.IdEtapaProspeccion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccion_ConsultarFiltros";
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
		Obten.Llena<CEstatusProspeccion>(typeof(CEstatusProspeccion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_EstatusProspeccion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", 0);
		Agregar.StoredProcedure.Parameters["@pIdEstatusProspeccion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEstatusProspeccion", estatusProspeccion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEtapaProspeccion", idEtapaProspeccion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idEstatusProspeccion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEstatusProspeccion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_EstatusProspeccion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", idEstatusProspeccion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEstatusProspeccion", estatusProspeccion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEtapaProspeccion", idEtapaProspeccion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_EstatusProspeccion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", idEstatusProspeccion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
