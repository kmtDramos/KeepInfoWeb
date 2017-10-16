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

public partial class CActividad
{
	//Propiedades Privadas
	private int idActividad;
	private int idEstatusActividad;
	private int idTipoActividad;
	private int idUsuario;
	private string actividad;
	private string tipoActividad;
	private DateTime fechaActividad;
	private int idCliente;
	private int idOportunidad;
	private int idTipoCliente;
	private DateTime fechaFin;
	private string cliente;
	
	//Propiedades
	public int IdActividad
	{
		get { return idActividad; }
		set
		{
			idActividad = value;
		}
	}
	
	public int IdEstatusActividad
	{
		get { return idEstatusActividad; }
		set
		{
			idEstatusActividad = value;
		}
	}
	
	public int IdTipoActividad
	{
		get { return idTipoActividad; }
		set
		{
			idTipoActividad = value;
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
	
	public string Actividad
	{
		get { return actividad; }
		set
		{
			actividad = value;
		}
	}
	
	public string TipoActividad
	{
		get { return tipoActividad; }
		set
		{
			tipoActividad = value;
		}
	}
	
	public DateTime FechaActividad
	{
		get { return fechaActividad; }
		set { fechaActividad = value; }
	}
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			idCliente = value;
		}
	}
	
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
		}
	}
	
	public int IdTipoCliente
	{
		get { return idTipoCliente; }
		set
		{
			idTipoCliente = value;
		}
	}
	
	public DateTime FechaFin
	{
		get { return fechaFin; }
		set { fechaFin = value; }
	}
	
	public string Cliente
	{
		get { return cliente; }
		set
		{
			cliente = value;
		}
	}
	
	//Constructores
	public CActividad()
	{
		idActividad = 0;
		idEstatusActividad = 0;
		idTipoActividad = 0;
		idUsuario = 0;
		actividad = "";
		tipoActividad = "";
		fechaActividad = new DateTime(1, 1, 1);
		idCliente = 0;
		idOportunidad = 0;
		idTipoCliente = 0;
		fechaFin = new DateTime(1, 1, 1);
		cliente = "";
	}
	
	public CActividad(int pIdActividad)
	{
		idActividad = pIdActividad;
		idEstatusActividad = 0;
		idTipoActividad = 0;
		idUsuario = 0;
		actividad = "";
		tipoActividad = "";
		fechaActividad = new DateTime(1, 1, 1);
		idCliente = 0;
		idOportunidad = 0;
		idTipoCliente = 0;
		fechaFin = new DateTime(1, 1, 1);
		cliente = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Actividad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CActividad>(typeof(CActividad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Actividad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CActividad>(typeof(CActividad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Actividad_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdActividad", pIdentificador);
		Obten.Llena<CActividad>(typeof(CActividad), pConexion);
		foreach (CActividad O in Obten.ListaRegistros)
		{
			idActividad = O.IdActividad;
			idEstatusActividad = O.IdEstatusActividad;
			idTipoActividad = O.IdTipoActividad;
			idUsuario = O.IdUsuario;
			actividad = O.Actividad;
			tipoActividad = O.TipoActividad;
			fechaActividad = O.FechaActividad;
			idCliente = O.IdCliente;
			idOportunidad = O.IdOportunidad;
			idTipoCliente = O.IdTipoCliente;
			fechaFin = O.FechaFin;
			cliente = O.Cliente;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Actividad_ConsultarFiltros";
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
		Obten.Llena<CActividad>(typeof(CActividad), pConexion);
		foreach (CActividad O in Obten.ListaRegistros)
		{
			idActividad = O.IdActividad;
			idEstatusActividad = O.IdEstatusActividad;
			idTipoActividad = O.IdTipoActividad;
			idUsuario = O.IdUsuario;
			actividad = O.Actividad;
			tipoActividad = O.TipoActividad;
			fechaActividad = O.FechaActividad;
			idCliente = O.IdCliente;
			idOportunidad = O.IdOportunidad;
			idTipoCliente = O.IdTipoCliente;
			fechaFin = O.FechaFin;
			cliente = O.Cliente;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Actividad_ConsultarFiltros";
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
		Obten.Llena<CActividad>(typeof(CActividad), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Actividad_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdActividad", 0);
		Agregar.StoredProcedure.Parameters["@pIdActividad"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusActividad", idEstatusActividad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoActividad", idTipoActividad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pActividad", actividad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoActividad", tipoActividad);
		if(fechaActividad.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaActividad", fechaActividad);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCliente", idTipoCliente);
		if(fechaFin.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCliente", cliente);
		Agregar.Insert(pConexion);
		idActividad= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdActividad"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Actividad_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdActividad", idActividad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusActividad", idEstatusActividad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoActividad", idTipoActividad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pActividad", actividad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoActividad", tipoActividad);
		if(fechaActividad.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaActividad", fechaActividad);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCliente", idTipoCliente);
		if(fechaFin.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCliente", cliente);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Actividad_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdActividad", idActividad);
		Eliminar.Delete(pConexion);
	}
}
