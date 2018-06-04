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

public partial class CLevantamiento
{
	//Propiedades Privadas
	private int idLevantamiento;
	private int idCliente;
	private int idDivision;
	private int idOportunidad;
	private int idProyecto;
	private int idCotizacion;
	private int idEstatusLevantamiento;
	private int idUsuario;
	private DateTime fechaInicio;
	private DateTime fechaFin;
	private DateTime fechaEstimada;
	private string descripcion ;
	private string motivoCancelacion ;
	private bool baja;
	
	//Propiedades
	public int IdLevantamiento
	{
		get { return idLevantamiento; }
		set
		{
			idLevantamiento = value;
		}
	}
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			idCliente = value;
		}
	}
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
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
	
	public int IdProyecto
	{
		get { return idProyecto; }
		set
		{
			idProyecto = value;
		}
	}
	
	public int IdCotizacion
	{
		get { return idCotizacion; }
		set
		{
			idCotizacion = value;
		}
	}
	
	public int IdEstatusLevantamiento
	{
		get { return idEstatusLevantamiento; }
		set
		{
			idEstatusLevantamiento = value;
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
	
	public DateTime FechaEstimada
	{
		get { return fechaEstimada; }
		set { fechaEstimada = value; }
	}
	
	public string Descripcion 
	{
		get { return descripcion ; }
		set
		{
			descripcion  = value;
		}
	}
	
	public string MotivoCancelacion 
	{
		get { return motivoCancelacion ; }
		set
		{
			motivoCancelacion  = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CLevantamiento()
	{
		idLevantamiento = 0;
		idCliente = 0;
		idDivision = 0;
		idOportunidad = 0;
		idProyecto = 0;
		idCotizacion = 0;
		idEstatusLevantamiento = 0;
		idUsuario = 0;
		fechaInicio = new DateTime(1, 1, 1);
		fechaFin = new DateTime(1, 1, 1);
		fechaEstimada = new DateTime(1, 1, 1);
		descripcion  = "";
		motivoCancelacion  = "";
		baja = false;
	}
	
	public CLevantamiento(int pIdLevantamiento)
	{
		idLevantamiento = pIdLevantamiento;
		idCliente = 0;
		idDivision = 0;
		idOportunidad = 0;
		idProyecto = 0;
		idCotizacion = 0;
		idEstatusLevantamiento = 0;
		idUsuario = 0;
		fechaInicio = new DateTime(1, 1, 1);
		fechaFin = new DateTime(1, 1, 1);
		fechaEstimada = new DateTime(1, 1, 1);
		descripcion  = "";
		motivoCancelacion  = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Levantamiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CLevantamiento>(typeof(CLevantamiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Levantamiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CLevantamiento>(typeof(CLevantamiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Levantamiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CLevantamiento>(typeof(CLevantamiento), pConexion);
		foreach (CLevantamiento O in Obten.ListaRegistros)
		{
			idLevantamiento = O.IdLevantamiento;
			idCliente = O.IdCliente;
			idDivision = O.IdDivision;
			idOportunidad = O.IdOportunidad;
			idProyecto = O.IdProyecto;
			idCotizacion = O.IdCotizacion;
			idEstatusLevantamiento = O.IdEstatusLevantamiento;
			idUsuario = O.IdUsuario;
			fechaInicio = O.FechaInicio;
			fechaFin = O.FechaFin;
			fechaEstimada = O.FechaEstimada;
			descripcion  = O.Descripcion ;
			motivoCancelacion  = O.MotivoCancelacion ;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Levantamiento_ConsultarFiltros";
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
		Obten.Llena<CLevantamiento>(typeof(CLevantamiento), pConexion);
		foreach (CLevantamiento O in Obten.ListaRegistros)
		{
			idLevantamiento = O.IdLevantamiento;
			idCliente = O.IdCliente;
			idDivision = O.IdDivision;
			idOportunidad = O.IdOportunidad;
			idProyecto = O.IdProyecto;
			idCotizacion = O.IdCotizacion;
			idEstatusLevantamiento = O.IdEstatusLevantamiento;
			idUsuario = O.IdUsuario;
			fechaInicio = O.FechaInicio;
			fechaFin = O.FechaFin;
			fechaEstimada = O.FechaEstimada;
			descripcion  = O.Descripcion ;
			motivoCancelacion  = O.MotivoCancelacion ;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Levantamiento_ConsultarFiltros";
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
		Obten.Llena<CLevantamiento>(typeof(CLevantamiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Levantamiento_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", 0);
		Agregar.StoredProcedure.Parameters["@pIdLevantamiento"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusLevantamiento", idEstatusLevantamiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		if(fechaInicio.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		if(fechaFin.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		if(fechaEstimada.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEstimada", fechaEstimada);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion ", descripcion );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion ", motivoCancelacion );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idLevantamiento= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdLevantamiento"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Levantamiento_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", idLevantamiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusLevantamiento", idEstatusLevantamiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		if(fechaInicio.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		if(fechaFin.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		if(fechaEstimada.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEstimada", fechaEstimada);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion ", descripcion );
		Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion ", motivoCancelacion );
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Levantamiento_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", idLevantamiento);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
