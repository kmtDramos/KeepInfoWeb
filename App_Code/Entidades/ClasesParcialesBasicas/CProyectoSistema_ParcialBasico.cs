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

public partial class CProyectoSistema
{
	//Propiedades Privadas
	private int idProyectoSistema;
	private string proyectoSistema;
	private string comando;
	
	//Propiedades
	public int IdProyectoSistema
	{
		get { return idProyectoSistema; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProyectoSistema = value;
		}
	}
	
	public string ProyectoSistema
	{
		get { return proyectoSistema; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			proyectoSistema = value;
		}
	}
	
	public string Comando
	{
		get { return comando; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			comando = value;
		}
	}
	
	//Constructores
	public CProyectoSistema()
	{
		idProyectoSistema = 0;
		proyectoSistema = "";
		comando = "";
	}
	
	public CProyectoSistema(int pIdProyectoSistema)
	{
		idProyectoSistema = pIdProyectoSistema;
		proyectoSistema = "";
		comando = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProyectoSistema_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CProyectoSistema>(typeof(CProyectoSistema), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProyectoSistema_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CProyectoSistema>(typeof(CProyectoSistema), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProyectoSistema_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdProyectoSistema", pIdentificador);
		Obten.Llena<CProyectoSistema>(typeof(CProyectoSistema), pConexion);
		foreach (CProyectoSistema O in Obten.ListaRegistros)
		{
			idProyectoSistema = O.IdProyectoSistema;
			proyectoSistema = O.ProyectoSistema;
			comando = O.Comando;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProyectoSistema_ConsultarFiltros";
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
		Obten.Llena<CProyectoSistema>(typeof(CProyectoSistema), pConexion);
		foreach (CProyectoSistema O in Obten.ListaRegistros)
		{
			idProyectoSistema = O.IdProyectoSistema;
			proyectoSistema = O.ProyectoSistema;
			comando = O.Comando;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProyectoSistema_ConsultarFiltros";
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
		Obten.Llena<CProyectoSistema>(typeof(CProyectoSistema), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ProyectoSistema_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoSistema", 0);
		Agregar.StoredProcedure.Parameters["@pIdProyectoSistema"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProyectoSistema", proyectoSistema);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComando", comando);
		Agregar.Insert(pConexion);
		idProyectoSistema= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProyectoSistema"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ProyectoSistema_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoSistema", idProyectoSistema);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProyectoSistema", proyectoSistema);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComando", comando);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ProyectoSistema_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoSistema", idProyectoSistema);
		Eliminar.Delete(pConexion);
	}
}