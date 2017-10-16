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

public partial class CEstatusPresupuesto
{
	//Propiedades Privadas
	private int idEstatusPresupuesto;
	private string estatusPresupuesto;
	private bool baja;
	
	//Propiedades
	public int IdEstatusPresupuesto
	{
		get { return idEstatusPresupuesto; }
		set
		{
			idEstatusPresupuesto = value;
		}
	}
	
	public string EstatusPresupuesto
	{
		get { return estatusPresupuesto; }
		set
		{
			estatusPresupuesto = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CEstatusPresupuesto()
	{
		idEstatusPresupuesto = 0;
		estatusPresupuesto = "";
		baja = false;
	}
	
	public CEstatusPresupuesto(int pIdEstatusPresupuesto)
	{
		idEstatusPresupuesto = pIdEstatusPresupuesto;
		estatusPresupuesto = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusPresupuesto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstatusPresupuesto>(typeof(CEstatusPresupuesto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusPresupuesto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CEstatusPresupuesto>(typeof(CEstatusPresupuesto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusPresupuesto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdEstatusPresupuesto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstatusPresupuesto>(typeof(CEstatusPresupuesto), pConexion);
		foreach (CEstatusPresupuesto O in Obten.ListaRegistros)
		{
			idEstatusPresupuesto = O.IdEstatusPresupuesto;
			estatusPresupuesto = O.EstatusPresupuesto;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusPresupuesto_ConsultarFiltros";
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
		Obten.Llena<CEstatusPresupuesto>(typeof(CEstatusPresupuesto), pConexion);
		foreach (CEstatusPresupuesto O in Obten.ListaRegistros)
		{
			idEstatusPresupuesto = O.IdEstatusPresupuesto;
			estatusPresupuesto = O.EstatusPresupuesto;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusPresupuesto_ConsultarFiltros";
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
		Obten.Llena<CEstatusPresupuesto>(typeof(CEstatusPresupuesto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_EstatusPresupuesto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusPresupuesto", 0);
		Agregar.StoredProcedure.Parameters["@pIdEstatusPresupuesto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEstatusPresupuesto", estatusPresupuesto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idEstatusPresupuesto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEstatusPresupuesto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_EstatusPresupuesto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusPresupuesto", idEstatusPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEstatusPresupuesto", estatusPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_EstatusPresupuesto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusPresupuesto", idEstatusPresupuesto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
