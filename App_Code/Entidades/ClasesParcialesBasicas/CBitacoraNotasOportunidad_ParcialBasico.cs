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

public partial class CBitacoraNotasOportunidad
{
	//Propiedades Privadas
	private int idBitacoraNotasOportunidad;
	private string nota;
	private int idUsuario;
	private DateTime fechaCreacion;
	private int idOportunidad;
	private int area;
	
	//Propiedades
	public int IdBitacoraNotasOportunidad
	{
		get { return idBitacoraNotasOportunidad; }
		set
		{
			idBitacoraNotasOportunidad = value;
		}
	}
	
	public string Nota
	{
		get { return nota; }
		set
		{
			nota = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public DateTime FechaCreacion
	{
		get { return fechaCreacion; }
		set { fechaCreacion = value; }
	}
	
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
		}
	}
	
	public int Area
	{
		get { return area; }
		set
		{
			area = value;
		}
	}
	
	//Constructores
	public CBitacoraNotasOportunidad()
	{
		idBitacoraNotasOportunidad = 0;
		nota = "";
		idUsuario = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		idOportunidad = 0;
		area = 0;
	}
	
	public CBitacoraNotasOportunidad(int pIdBitacoraNotasOportunidad)
	{
		idBitacoraNotasOportunidad = pIdBitacoraNotasOportunidad;
		nota = "";
		idUsuario = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		idOportunidad = 0;
		area = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", pIdentificador);
		Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
		foreach (CBitacoraNotasOportunidad O in Obten.ListaRegistros)
		{
			idBitacoraNotasOportunidad = O.IdBitacoraNotasOportunidad;
			nota = O.Nota;
			idUsuario = O.IdUsuario;
			fechaCreacion = O.FechaCreacion;
			idOportunidad = O.IdOportunidad;
			area = O.Area;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_ConsultarFiltros";
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
		Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
		foreach (CBitacoraNotasOportunidad O in Obten.ListaRegistros)
		{
			idBitacoraNotasOportunidad = O.IdBitacoraNotasOportunidad;
			nota = O.Nota;
			idUsuario = O.IdUsuario;
			fechaCreacion = O.FechaCreacion;
			idOportunidad = O.IdOportunidad;
			area = O.Area;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_ConsultarFiltros";
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
		Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", 0);
		Agregar.StoredProcedure.Parameters["@pIdBitacoraNotasOportunidad"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		if(fechaCreacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pArea", area);
		Agregar.Insert(pConexion);
		idBitacoraNotasOportunidad= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdBitacoraNotasOportunidad"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", idBitacoraNotasOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		if(fechaCreacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pArea", area);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", idBitacoraNotasOportunidad);
		Eliminar.Delete(pConexion);
	}
}
