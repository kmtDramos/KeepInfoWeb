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

public partial class CProcedencia
{
	//Propiedades Privadas
	private int idProcedencia;
	private int idExistenciaDistribuida;
	private int idExistenciaVendida;
	
	//Propiedades
	public int IdProcedencia
	{
		get { return idProcedencia; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProcedencia = value;
		}
	}
	
	public int IdExistenciaDistribuida
	{
		get { return idExistenciaDistribuida; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idExistenciaDistribuida = value;
		}
	}
	
	public int IdExistenciaVendida
	{
		get { return idExistenciaVendida; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idExistenciaVendida = value;
		}
	}
	
	//Constructores
	public CProcedencia()
	{
		idProcedencia = 0;
		idExistenciaDistribuida = 0;
		idExistenciaVendida = 0;
	}
	
	public CProcedencia(int pIdProcedencia)
	{
		idProcedencia = pIdProcedencia;
		idExistenciaDistribuida = 0;
		idExistenciaVendida = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Procedencia_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CProcedencia>(typeof(CProcedencia), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Procedencia_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CProcedencia>(typeof(CProcedencia), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Procedencia_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdProcedencia", pIdentificador);
		Obten.Llena<CProcedencia>(typeof(CProcedencia), pConexion);
		foreach (CProcedencia O in Obten.ListaRegistros)
		{
			idProcedencia = O.IdProcedencia;
			idExistenciaDistribuida = O.IdExistenciaDistribuida;
			idExistenciaVendida = O.IdExistenciaVendida;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Procedencia_ConsultarFiltros";
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
		Obten.Llena<CProcedencia>(typeof(CProcedencia), pConexion);
		foreach (CProcedencia O in Obten.ListaRegistros)
		{
			idProcedencia = O.IdProcedencia;
			idExistenciaDistribuida = O.IdExistenciaDistribuida;
			idExistenciaVendida = O.IdExistenciaVendida;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Procedencia_ConsultarFiltros";
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
		Obten.Llena<CProcedencia>(typeof(CProcedencia), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Procedencia_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProcedencia", 0);
		Agregar.StoredProcedure.Parameters["@pIdProcedencia"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", idExistenciaDistribuida);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaVendida", idExistenciaVendida);
		Agregar.Insert(pConexion);
		idProcedencia= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProcedencia"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Procedencia_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProcedencia", idProcedencia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuida", idExistenciaDistribuida);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaVendida", idExistenciaVendida);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Procedencia_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProcedencia", idProcedencia);
		Eliminar.Delete(pConexion);
	}
}