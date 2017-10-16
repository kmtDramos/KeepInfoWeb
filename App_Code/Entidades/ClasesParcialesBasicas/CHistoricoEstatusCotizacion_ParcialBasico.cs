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

public partial class CHistoricoEstatusCotizacion
{
	//Propiedades Privadas
	private int idHistoricoEstatusCotizacion;
	private DateTime fechaInicio;
	private DateTime fechaFin;
	private int idCotizacion;
	private int idEstatusCotizacion;
	private int idUsuario;
	
	//Propiedades
	public int IdHistoricoEstatusCotizacion
	{
		get { return idHistoricoEstatusCotizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idHistoricoEstatusCotizacion = value;
		}
	}
	
	public DateTime FechaInicio
	{
		get { return fechaInicio; }
		set { fechaInicio = value; }
	}
	
	public DateTime FechaFin
	{
		get { return fechaFin; }
		set { fechaFin = value; }
	}
	
	public int IdCotizacion
	{
		get { return idCotizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCotizacion = value;
		}
	}
	
	public int IdEstatusCotizacion
	{
		get { return idEstatusCotizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEstatusCotizacion = value;
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
	
	//Constructores
	public CHistoricoEstatusCotizacion()
	{
		idHistoricoEstatusCotizacion = 0;
		fechaInicio = new DateTime(1, 1, 1);
		fechaFin = new DateTime(1, 1, 1);
		idCotizacion = 0;
		idEstatusCotizacion = 0;
		idUsuario = 0;
	}
	
	public CHistoricoEstatusCotizacion(int pIdHistoricoEstatusCotizacion)
	{
		idHistoricoEstatusCotizacion = pIdHistoricoEstatusCotizacion;
		fechaInicio = new DateTime(1, 1, 1);
		fechaFin = new DateTime(1, 1, 1);
		idCotizacion = 0;
		idEstatusCotizacion = 0;
		idUsuario = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CHistoricoEstatusCotizacion>(typeof(CHistoricoEstatusCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CHistoricoEstatusCotizacion>(typeof(CHistoricoEstatusCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdHistoricoEstatusCotizacion", pIdentificador);
		Obten.Llena<CHistoricoEstatusCotizacion>(typeof(CHistoricoEstatusCotizacion), pConexion);
		foreach (CHistoricoEstatusCotizacion O in Obten.ListaRegistros)
		{
			idHistoricoEstatusCotizacion = O.IdHistoricoEstatusCotizacion;
			fechaInicio = O.FechaInicio;
			fechaFin = O.FechaFin;
			idCotizacion = O.IdCotizacion;
			idEstatusCotizacion = O.IdEstatusCotizacion;
			idUsuario = O.IdUsuario;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_ConsultarFiltros";
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
		Obten.Llena<CHistoricoEstatusCotizacion>(typeof(CHistoricoEstatusCotizacion), pConexion);
		foreach (CHistoricoEstatusCotizacion O in Obten.ListaRegistros)
		{
			idHistoricoEstatusCotizacion = O.IdHistoricoEstatusCotizacion;
			fechaInicio = O.FechaInicio;
			fechaFin = O.FechaFin;
			idCotizacion = O.IdCotizacion;
			idEstatusCotizacion = O.IdEstatusCotizacion;
			idUsuario = O.IdUsuario;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_ConsultarFiltros";
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
		Obten.Llena<CHistoricoEstatusCotizacion>(typeof(CHistoricoEstatusCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdHistoricoEstatusCotizacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdHistoricoEstatusCotizacion"].Direction = ParameterDirection.Output;
		if(fechaInicio.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		if(fechaFin.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusCotizacion", idEstatusCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.Insert(pConexion);
		idHistoricoEstatusCotizacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdHistoricoEstatusCotizacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdHistoricoEstatusCotizacion", idHistoricoEstatusCotizacion);
		if(fechaInicio.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		if(fechaFin.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusCotizacion", idEstatusCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_HistoricoEstatusCotizacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdHistoricoEstatusCotizacion", idHistoricoEstatusCotizacion);
		Eliminar.Delete(pConexion);
	}
}