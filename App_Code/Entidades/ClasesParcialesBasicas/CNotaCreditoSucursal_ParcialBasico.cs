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

public partial class CNotaCreditoSucursal
{
	//Propiedades Privadas
	private int idNotaCreditoSucursal;
	private DateTime fechaAlta;
	private DateTime fechaUltimaModificacion;
	private int idNotaCredito;
	private int idSucursal;
	private int idUsuarioAlta;
	private int idUsuarioModifico;
	private bool baja;
	
	//Propiedades
	public int IdNotaCreditoSucursal
	{
		get { return idNotaCreditoSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNotaCreditoSucursal = value;
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
	
	public int IdNotaCredito
	{
		get { return idNotaCredito; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNotaCredito = value;
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
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CNotaCreditoSucursal()
	{
		idNotaCreditoSucursal = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idNotaCredito = 0;
		idSucursal = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		baja = false;
	}
	
	public CNotaCreditoSucursal(int pIdNotaCreditoSucursal)
	{
		idNotaCreditoSucursal = pIdNotaCreditoSucursal;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idNotaCredito = 0;
		idSucursal = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNotaCreditoSucursal>(typeof(CNotaCreditoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CNotaCreditoSucursal>(typeof(CNotaCreditoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoSucursal", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNotaCreditoSucursal>(typeof(CNotaCreditoSucursal), pConexion);
		foreach (CNotaCreditoSucursal O in Obten.ListaRegistros)
		{
			idNotaCreditoSucursal = O.IdNotaCreditoSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idNotaCredito = O.IdNotaCredito;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_ConsultarFiltros";
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
		Obten.Llena<CNotaCreditoSucursal>(typeof(CNotaCreditoSucursal), pConexion);
		foreach (CNotaCreditoSucursal O in Obten.ListaRegistros)
		{
			idNotaCreditoSucursal = O.IdNotaCreditoSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idNotaCredito = O.IdNotaCredito;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_ConsultarFiltros";
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
		Obten.Llena<CNotaCreditoSucursal>(typeof(CNotaCreditoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoSucursal", 0);
		Agregar.StoredProcedure.Parameters["@pIdNotaCreditoSucursal"].Direction = ParameterDirection.Output;
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idNotaCreditoSucursal= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNotaCreditoSucursal"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoSucursal", idNotaCreditoSucursal);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_NotaCreditoSucursal_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoSucursal", idNotaCreditoSucursal);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}