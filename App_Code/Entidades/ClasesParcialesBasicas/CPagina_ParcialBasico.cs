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

public partial class CPagina
{
	//Propiedades Privadas
	private int idPagina;
	private string pagina;
	private string nombreMenu;
	private string titulo;
	private int orden;
	private int idMenu;
	private bool validarSucursal;
	
	//Propiedades
	public int IdPagina
	{
		get { return idPagina; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPagina = value;
		}
	}
	
	public string Pagina
	{
		get { return pagina; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			pagina = value;
		}
	}
	
	public string NombreMenu
	{
		get { return nombreMenu; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			nombreMenu = value;
		}
	}
	
	public string Titulo
	{
		get { return titulo; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			titulo = value;
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
	
	public bool ValidarSucursal
	{
		get { return validarSucursal; }
		set { validarSucursal = value; }
	}
	
	//Constructores
	public CPagina()
	{
		idPagina = 0;
		pagina = "";
		nombreMenu = "";
		titulo = "";
		orden = 0;
		idMenu = 0;
		validarSucursal = false;
	}
	
	public CPagina(int pIdPagina)
	{
		idPagina = pIdPagina;
		pagina = "";
		nombreMenu = "";
		titulo = "";
		orden = 0;
		idMenu = 0;
		validarSucursal = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Pagina_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CPagina>(typeof(CPagina), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Pagina_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPagina>(typeof(CPagina), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Pagina_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPagina", pIdentificador);
		Obten.Llena<CPagina>(typeof(CPagina), pConexion);
		foreach (CPagina O in Obten.ListaRegistros)
		{
			idPagina = O.IdPagina;
			pagina = O.Pagina;
			nombreMenu = O.NombreMenu;
			titulo = O.Titulo;
			orden = O.Orden;
			idMenu = O.IdMenu;
			validarSucursal = O.ValidarSucursal;
		}
	}
	
	public List<object> LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Pagina_ConsultarFiltros";
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
		Obten.Llena<CPagina>(typeof(CPagina), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Pagina_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", 0);
		Agregar.StoredProcedure.Parameters["@pIdPagina"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPagina", pagina);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNombreMenu", nombreMenu);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTitulo", titulo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMenu", idMenu);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pValidarSucursal", validarSucursal);
		Agregar.Insert(pConexion);
		idPagina= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdPagina"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Pagina_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", idPagina);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPagina", pagina);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNombreMenu", nombreMenu);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTitulo", titulo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMenu", idMenu);
		Editar.StoredProcedure.Parameters.AddWithValue("@pValidarSucursal", validarSucursal);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Pagina_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", idPagina);
		Eliminar.Delete(pConexion);
	}
}