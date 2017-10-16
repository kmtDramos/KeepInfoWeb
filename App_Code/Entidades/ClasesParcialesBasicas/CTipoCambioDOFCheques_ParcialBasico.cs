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

public partial class CTipoCambioDOFCheques
{
	//Propiedades Privadas
	private int idTipoCambioDOFCheques;
	private int idTipoMonedaOrigen;
	private int idTipoMonedaDestino;
	private decimal tipoCambio;
	private DateTime fecha;
	private int idCheques;
	
	//Propiedades
	public int IdTipoCambioDOFCheques
	{
		get { return idTipoCambioDOFCheques; }
		set
		{
			idTipoCambioDOFCheques = value;
		}
	}
	
	public int IdTipoMonedaOrigen
	{
		get { return idTipoMonedaOrigen; }
		set
		{
			idTipoMonedaOrigen = value;
		}
	}
	
	public int IdTipoMonedaDestino
	{
		get { return idTipoMonedaDestino; }
		set
		{
			idTipoMonedaDestino = value;
		}
	}
	
	public decimal TipoCambio
	{
		get { return tipoCambio; }
		set
		{
			tipoCambio = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int IdCheques
	{
		get { return idCheques; }
		set
		{
			idCheques = value;
		}
	}
	
	//Constructores
	public CTipoCambioDOFCheques()
	{
		idTipoCambioDOFCheques = 0;
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		tipoCambio = 0;
		fecha = new DateTime(1, 1, 1);
		idCheques = 0;
	}
	
	public CTipoCambioDOFCheques(int pIdTipoCambioDOFCheques)
	{
		idTipoCambioDOFCheques = pIdTipoCambioDOFCheques;
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		tipoCambio = 0;
		fecha = new DateTime(1, 1, 1);
		idCheques = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CTipoCambioDOFCheques>(typeof(CTipoCambioDOFCheques), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTipoCambioDOFCheques>(typeof(CTipoCambioDOFCheques), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDOFCheques", pIdentificador);
		Obten.Llena<CTipoCambioDOFCheques>(typeof(CTipoCambioDOFCheques), pConexion);
		foreach (CTipoCambioDOFCheques O in Obten.ListaRegistros)
		{
			idTipoCambioDOFCheques = O.IdTipoCambioDOFCheques;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			tipoCambio = O.TipoCambio;
			fecha = O.Fecha;
			idCheques = O.IdCheques;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioDOFCheques>(typeof(CTipoCambioDOFCheques), pConexion);
		foreach (CTipoCambioDOFCheques O in Obten.ListaRegistros)
		{
			idTipoCambioDOFCheques = O.IdTipoCambioDOFCheques;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			tipoCambio = O.TipoCambio;
			fecha = O.Fecha;
			idCheques = O.IdCheques;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioDOFCheques>(typeof(CTipoCambioDOFCheques), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDOFCheques", 0);
		Agregar.StoredProcedure.Parameters["@pIdTipoCambioDOFCheques"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCheques", idCheques);
		Agregar.Insert(pConexion);
		idTipoCambioDOFCheques= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoCambioDOFCheques"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDOFCheques", idTipoCambioDOFCheques);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCheques", idCheques);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TipoCambioDOFCheques_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioDOFCheques", idTipoCambioDOFCheques);
		Eliminar.Delete(pConexion);
	}
}
