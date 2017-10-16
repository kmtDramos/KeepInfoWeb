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

public partial class CCotizacionSucursal
{
	//Propiedades Privadas
	private int idCotizacionSucursal;
	private int idCotizacion;
	private int idSucursal;
	private DateTime fechaAlta;
	private DateTime fechaUltimaModificacion;
	private int idUsuarioAlta;
	private int idUsuarioModifico;
	
	//Propiedades
	public int IdCotizacionSucursal
	{
		get { return idCotizacionSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCotizacionSucursal = value;
		}
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
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursal = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaUltimaModificacion
	{
		get { return fechaUltimaModificacion; }
		set { fechaUltimaModificacion = value; }
	}
	
	public int IdUsuarioAlta
	{
		get { return idUsuarioAlta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioAlta = value;
		}
	}
	
	public int IdUsuarioModifico
	{
		get { return idUsuarioModifico; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioModifico = value;
		}
	}
	
	//Constructores
	public CCotizacionSucursal()
	{
		idCotizacionSucursal = 0;
		idCotizacion = 0;
		idSucursal = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
	}
	
	public CCotizacionSucursal(int pIdCotizacionSucursal)
	{
		idCotizacionSucursal = pIdCotizacionSucursal;
		idCotizacion = 0;
		idSucursal = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CCotizacionSucursal>(typeof(CCotizacionSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CCotizacionSucursal>(typeof(CCotizacionSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionSucursal", pIdentificador);
		Obten.Llena<CCotizacionSucursal>(typeof(CCotizacionSucursal), pConexion);
		foreach (CCotizacionSucursal O in Obten.ListaRegistros)
		{
			idCotizacionSucursal = O.IdCotizacionSucursal;
			idCotizacion = O.IdCotizacion;
			idSucursal = O.IdSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionSucursal_ConsultarFiltros";
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
		Obten.Llena<CCotizacionSucursal>(typeof(CCotizacionSucursal), pConexion);
		foreach (CCotizacionSucursal O in Obten.ListaRegistros)
		{
			idCotizacionSucursal = O.IdCotizacionSucursal;
			idCotizacion = O.IdCotizacion;
			idSucursal = O.IdSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CotizacionSucursal_ConsultarFiltros";
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
		Obten.Llena<CCotizacionSucursal>(typeof(CCotizacionSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_CotizacionSucursal_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionSucursal", 0);
		Agregar.StoredProcedure.Parameters["@pIdCotizacionSucursal"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Agregar.Insert(pConexion);
		idCotizacionSucursal= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCotizacionSucursal"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_CotizacionSucursal_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionSucursal", idCotizacionSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_CotizacionSucursal_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionSucursal", idCotizacionSucursal);
		Eliminar.Delete(pConexion);
	}
}