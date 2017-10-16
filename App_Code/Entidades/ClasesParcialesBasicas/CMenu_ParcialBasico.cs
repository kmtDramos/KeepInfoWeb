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

public partial class CMenu
{
	//Propiedades Privadas
	private int idMenu;
	private string menu;
	private int idProyectoSistema;
	private int orden;
	
	//Propiedades
	public int IdMenu
	{
		get { return idMenu; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idMenu = value;
		}
	}
	
	public string Menu
	{
		get { return menu; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			menu = value;
		}
	}
	
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
	
	public int Orden
	{
		get { return orden; }
		set
		{
			if (value < 0)
			{
				return;
			}
			orden = value;
		}
	}
	
	//Constructores
	public CMenu()
	{
		idMenu = 0;
		menu = "";
		idProyectoSistema = 0;
		orden = 0;
	}
	
	public CMenu(int pIdMenu)
	{
		idMenu = pIdMenu;
		menu = "";
		idProyectoSistema = 0;
		orden = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Menu_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CMenu>(typeof(CMenu), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Menu_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CMenu>(typeof(CMenu), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Menu_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdMenu", pIdentificador);
		Obten.Llena<CMenu>(typeof(CMenu), pConexion);
		foreach (CMenu O in Obten.ListaRegistros)
		{
			idMenu = O.IdMenu;
			menu = O.Menu;
			idProyectoSistema = O.IdProyectoSistema;
			orden = O.Orden;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Menu_ConsultarFiltros";
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
		Obten.Llena<CMenu>(typeof(CMenu), pConexion);
		foreach (CMenu O in Obten.ListaRegistros)
		{
			idMenu = O.IdMenu;
			menu = O.Menu;
			idProyectoSistema = O.IdProyectoSistema;
			orden = O.Orden;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Menu_ConsultarFiltros";
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
		Obten.Llena<CMenu>(typeof(CMenu), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Menu_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMenu", 0);
		Agregar.StoredProcedure.Parameters["@pIdMenu"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMenu", menu);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoSistema", idProyectoSistema);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Agregar.Insert(pConexion);
		idMenu= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdMenu"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Menu_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMenu", idMenu);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMenu", menu);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoSistema", idProyectoSistema);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Menu_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdMenu", idMenu);
		Eliminar.Delete(pConexion);
	}
}