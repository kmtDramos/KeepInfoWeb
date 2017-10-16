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

public partial class CTipoCambioDiarioOficial
{
	//Propiedades Privadas
	private int idTipoCambioDiarioOficial;
	private decimal tipoCambioDiarioOficial;
	private DateTime fecha;
	private int idTipoMonedaOrigen;
	private int idTipoMonedaDestino;
	private bool baja;
	
	//Propiedades
	public int IdTipoCambioDiarioOficial
	{
		get { return idTipoCambioDiarioOficial; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoCambioDiarioOficial = value;
		}
	}
	
	public decimal TipoCambioDiarioOficial
	{
		get { return tipoCambioDiarioOficial; }
		set
		{
			if (value < 0)
			{
				return;
			}
			tipoCambioDiarioOficial = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int IdTipoMonedaOrigen
	{
		get { return idTipoMonedaOrigen; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMonedaOrigen = value;
		}
	}
	
	public int IdTipoMonedaDestino
	{
		get { return idTipoMonedaDestino; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMonedaDestino = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CTipoCambioDiarioOficial()
	{
		idTipoCambioDiarioOficial = 0;
		tipoCambioDiarioOficial = 0;
		fecha = new DateTime(1, 1, 1);
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		baja = false;
	}
	
	public CTipoCambioDiarioOficial(int pIdTipoCambioDiarioOficial)
	{
		idTipoCambioDiarioOficial = pIdTipoCambioDiarioOficial;
		tipoCambioDiarioOficial = 0;
		fecha = new DateTime(1, 1, 1);
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTipoCambioDiarioOficial>(typeof(CTipoCambioDiarioOficial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTipoCambioDiarioOficial>(typeof(CTipoCambioDiarioOficial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDiarioOficial", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTipoCambioDiarioOficial>(typeof(CTipoCambioDiarioOficial), pConexion);
		foreach (CTipoCambioDiarioOficial O in Obten.ListaRegistros)
		{
			idTipoCambioDiarioOficial = O.IdTipoCambioDiarioOficial;
			tipoCambioDiarioOficial = O.TipoCambioDiarioOficial;
			fecha = O.Fecha;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioDiarioOficial>(typeof(CTipoCambioDiarioOficial), pConexion);
		foreach (CTipoCambioDiarioOficial O in Obten.ListaRegistros)
		{
			idTipoCambioDiarioOficial = O.IdTipoCambioDiarioOficial;
			tipoCambioDiarioOficial = O.TipoCambioDiarioOficial;
			fecha = O.Fecha;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioDiarioOficial>(typeof(CTipoCambioDiarioOficial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDiarioOficial", 0);
		Agregar.StoredProcedure.Parameters["@pIdTipoCambioDiarioOficial"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDiarioOficial", tipoCambioDiarioOficial);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idTipoCambioDiarioOficial= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoCambioDiarioOficial"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDiarioOficial", idTipoCambioDiarioOficial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDiarioOficial", tipoCambioDiarioOficial);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TipoCambioDiarioOficial_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDiarioOficial", idTipoCambioDiarioOficial);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}