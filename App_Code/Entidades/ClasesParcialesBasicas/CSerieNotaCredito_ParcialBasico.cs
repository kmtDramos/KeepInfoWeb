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

public partial class CSerieNotaCredito
{
	//Propiedades Privadas
	private int idSerieNotaCredito;
	private string serieNotaCredito;
	private int idSucursal;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private bool baja;
	
	//Propiedades
	public int IdSerieNotaCredito
	{
		get { return idSerieNotaCredito; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSerieNotaCredito = value;
		}
	}
	
	public string SerieNotaCredito
	{
		get { return serieNotaCredito; }
		set
		{
			serieNotaCredito = value;
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
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSerieNotaCredito()
	{
		idSerieNotaCredito = 0;
		serieNotaCredito = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CSerieNotaCredito(int pIdSerieNotaCredito)
	{
		idSerieNotaCredito = pIdSerieNotaCredito;
		serieNotaCredito = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieNotaCredito_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSerieNotaCredito>(typeof(CSerieNotaCredito), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieNotaCredito_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSerieNotaCredito>(typeof(CSerieNotaCredito), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieNotaCredito_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSerieNotaCredito", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSerieNotaCredito>(typeof(CSerieNotaCredito), pConexion);
		foreach (CSerieNotaCredito O in Obten.ListaRegistros)
		{
			idSerieNotaCredito = O.IdSerieNotaCredito;
			serieNotaCredito = O.SerieNotaCredito;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieNotaCredito_ConsultarFiltros";
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
		Obten.Llena<CSerieNotaCredito>(typeof(CSerieNotaCredito), pConexion);
		foreach (CSerieNotaCredito O in Obten.ListaRegistros)
		{
			idSerieNotaCredito = O.IdSerieNotaCredito;
			serieNotaCredito = O.SerieNotaCredito;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieNotaCredito_ConsultarFiltros";
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
		Obten.Llena<CSerieNotaCredito>(typeof(CSerieNotaCredito), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SerieNotaCredito_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSerieNotaCredito", 0);
		Agregar.StoredProcedure.Parameters["@pIdSerieNotaCredito"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSerieNotaCredito= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSerieNotaCredito"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SerieNotaCredito_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSerieNotaCredito", idSerieNotaCredito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SerieNotaCredito_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSerieNotaCredito", idSerieNotaCredito);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}