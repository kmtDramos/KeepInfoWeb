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

public partial class CFlujoCaja
{
	//Propiedades Privadas
	private int idFlujoCaja;
	private string flujoCaja;
	private int afectacion;
	private bool baja;
	
	//Propiedades
	public int IdFlujoCaja
	{
		get { return idFlujoCaja; }
		set
		{
			idFlujoCaja = value;
		}
	}
	
	public string FlujoCaja
	{
		get { return flujoCaja; }
		set
		{
			flujoCaja = value;
		}
	}
	
	public int Afectacion
	{
		get { return afectacion; }
		set
		{
			afectacion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CFlujoCaja()
	{
		idFlujoCaja = 0;
		flujoCaja = "";
		afectacion = 0;
		baja = false;
	}
	
	public CFlujoCaja(int pIdFlujoCaja)
	{
		idFlujoCaja = pIdFlujoCaja;
		flujoCaja = "";
		afectacion = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FlujoCaja_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFlujoCaja>(typeof(CFlujoCaja), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FlujoCaja_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CFlujoCaja>(typeof(CFlujoCaja), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FlujoCaja_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdFlujoCaja", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFlujoCaja>(typeof(CFlujoCaja), pConexion);
		foreach (CFlujoCaja O in Obten.ListaRegistros)
		{
			idFlujoCaja = O.IdFlujoCaja;
			flujoCaja = O.FlujoCaja;
			afectacion = O.Afectacion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FlujoCaja_ConsultarFiltros";
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
		Obten.Llena<CFlujoCaja>(typeof(CFlujoCaja), pConexion);
		foreach (CFlujoCaja O in Obten.ListaRegistros)
		{
			idFlujoCaja = O.IdFlujoCaja;
			flujoCaja = O.FlujoCaja;
			afectacion = O.Afectacion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FlujoCaja_ConsultarFiltros";
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
		Obten.Llena<CFlujoCaja>(typeof(CFlujoCaja), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_FlujoCaja_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFlujoCaja", 0);
		Agregar.StoredProcedure.Parameters["@pIdFlujoCaja"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFlujoCaja", flujoCaja);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAfectacion", afectacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idFlujoCaja= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFlujoCaja"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_FlujoCaja_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFlujoCaja", idFlujoCaja);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFlujoCaja", flujoCaja);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAfectacion", afectacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_FlujoCaja_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdFlujoCaja", idFlujoCaja);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
