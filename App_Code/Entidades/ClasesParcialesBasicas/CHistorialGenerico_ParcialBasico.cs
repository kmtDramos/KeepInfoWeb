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

public partial class CHistorialGenerico
{
	//Propiedades Privadas
	private int idHistorialGenerico;
	private int idClaseGenerador;
	private int idGenerico;
	private int idUsuario;
	private DateTime fecha;
	private string comentario;
	
	//Propiedades
	public int IdHistorialGenerico
	{
		get { return idHistorialGenerico; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idHistorialGenerico = value;
		}
	}
	
	public int IdClaseGenerador
	{
		get { return idClaseGenerador; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idClaseGenerador = value;
		}
	}
	
	public int IdGenerico
	{
		get { return idGenerico; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idGenerico = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuario = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public string Comentario
	{
		get { return comentario; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			comentario = value;
		}
	}
	
	//Constructores
	public CHistorialGenerico()
	{
		idHistorialGenerico = 0;
		idClaseGenerador = 0;
		idGenerico = 0;
		idUsuario = 0;
		fecha = new DateTime(1, 1, 1);
		comentario = "";
	}
	
	public CHistorialGenerico(int pIdHistorialGenerico)
	{
		idHistorialGenerico = pIdHistorialGenerico;
		idClaseGenerador = 0;
		idGenerico = 0;
		idUsuario = 0;
		fecha = new DateTime(1, 1, 1);
		comentario = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistorialGenerico_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CHistorialGenerico>(typeof(CHistorialGenerico), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistorialGenerico_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CHistorialGenerico>(typeof(CHistorialGenerico), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistorialGenerico_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdHistorialGenerico", pIdentificador);
		Obten.Llena<CHistorialGenerico>(typeof(CHistorialGenerico), pConexion);
		foreach (CHistorialGenerico O in Obten.ListaRegistros)
		{
			idHistorialGenerico = O.IdHistorialGenerico;
			idClaseGenerador = O.IdClaseGenerador;
			idGenerico = O.IdGenerico;
			idUsuario = O.IdUsuario;
			fecha = O.Fecha;
			comentario = O.Comentario;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistorialGenerico_ConsultarFiltros";
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
		Obten.Llena<CHistorialGenerico>(typeof(CHistorialGenerico), pConexion);
		foreach (CHistorialGenerico O in Obten.ListaRegistros)
		{
			idHistorialGenerico = O.IdHistorialGenerico;
			idClaseGenerador = O.IdClaseGenerador;
			idGenerico = O.IdGenerico;
			idUsuario = O.IdUsuario;
			fecha = O.Fecha;
			comentario = O.Comentario;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistorialGenerico_ConsultarFiltros";
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
		Obten.Llena<CHistorialGenerico>(typeof(CHistorialGenerico), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_HistorialGenerico_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdHistorialGenerico", 0);
		Agregar.StoredProcedure.Parameters["@pIdHistorialGenerico"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGenerico", idGenerico);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComentario", comentario);
		Agregar.Insert(pConexion);
		idHistorialGenerico= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdHistorialGenerico"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_HistorialGenerico_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdHistorialGenerico", idHistorialGenerico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdGenerico", idGenerico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pComentario", comentario);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_HistorialGenerico_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdHistorialGenerico", idHistorialGenerico);
		Eliminar.Delete(pConexion);
	}
}