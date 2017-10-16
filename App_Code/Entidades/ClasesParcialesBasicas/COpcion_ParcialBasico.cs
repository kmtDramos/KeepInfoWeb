using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class COpcion
{
	//Propiedades Privadas
	private int idOpcion;
	private string opcion;
	private string comando;
	
	//Propiedades
	public int IdOpcion
	{
		get { return idOpcion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOpcion = value;
		}
	}
	
	public string Opcion
	{
		get { return opcion; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			opcion = value;
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
	public COpcion()
	{
		IdOpcion = 0;
		opcion = "";
		comando = "";
	}
	
	public COpcion(int pIdOpcion)
	{
		idOpcion = pIdOpcion;
		opcion = "";
		comando = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opcion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<COpcion>(typeof(COpcion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opcion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COpcion>(typeof(COpcion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opcion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", pIdentificador);
		Obten.Llena<COpcion>(typeof(COpcion), pConexion);
		foreach (COpcion O in Obten.ListaRegistros)
		{
			idOpcion = O.IdOpcion;
			opcion = O.Opcion;
			comando = O.Comando;
		}
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Opcion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOpcion", opcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComando", comando);
		Agregar.Insert(pConexion);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Opcion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOpcion", opcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComando", comando);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Opcion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Eliminar.Delete(pConexion);
	}
}