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

public partial class CUsoCFDI
{
	//Propiedades Privadas
	private int idUsoCFDI;
	private string descricpion;
	private string claveUsoCFDI;
	private bool baja;
	
	//Propiedades
	public int IdUsoCFDI
	{
		get { return idUsoCFDI; }
		set
		{
			idUsoCFDI = value;
		}
	}
	
	public string Descricpion
	{
		get { return descricpion; }
		set
		{
			descricpion = value;
		}
	}
	
	public string ClaveUsoCFDI
	{
		get { return claveUsoCFDI; }
		set
		{
			claveUsoCFDI = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CUsoCFDI()
	{
		idUsoCFDI = 0;
		descricpion = "";
		claveUsoCFDI = "";
		baja = false;
	}
	
	public CUsoCFDI(int pIdUsoCFDI)
	{
		idUsoCFDI = pIdUsoCFDI;
		descricpion = "";
		claveUsoCFDI = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsoCFDI_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUsoCFDI>(typeof(CUsoCFDI), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsoCFDI_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CUsoCFDI>(typeof(CUsoCFDI), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsoCFDI_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsoCFDI", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUsoCFDI>(typeof(CUsoCFDI), pConexion);
		foreach (CUsoCFDI O in Obten.ListaRegistros)
		{
			idUsoCFDI = O.IdUsoCFDI;
			descricpion = O.Descricpion;
			claveUsoCFDI = O.ClaveUsoCFDI;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsoCFDI_ConsultarFiltros";
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
		Obten.Llena<CUsoCFDI>(typeof(CUsoCFDI), pConexion);
		foreach (CUsoCFDI O in Obten.ListaRegistros)
		{
			idUsoCFDI = O.IdUsoCFDI;
			descricpion = O.Descricpion;
			claveUsoCFDI = O.ClaveUsoCFDI;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsoCFDI_ConsultarFiltros";
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
		Obten.Llena<CUsoCFDI>(typeof(CUsoCFDI), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_UsoCFDI_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsoCFDI", 0);
		Agregar.StoredProcedure.Parameters["@pIdUsoCFDI"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescricpion", descricpion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveUsoCFDI", claveUsoCFDI);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idUsoCFDI= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdUsoCFDI"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_UsoCFDI_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsoCFDI", idUsoCFDI);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescricpion", descricpion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveUsoCFDI", claveUsoCFDI);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_UsoCFDI_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdUsoCFDI", idUsoCFDI);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
