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

public partial class CSerieComplementoPago
{
	//Propiedades Privadas
	private int idSerieComplementoPago;
	private string serieComplementoPago;
	private int idSucursal;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private bool baja;
	
	//Propiedades
	public int IdSerieComplementoPago
	{
		get { return idSerieComplementoPago; }
		set
		{
			idSerieComplementoPago = value;
		}
	}
	
	public string SerieComplementoPago
	{
		get { return serieComplementoPago; }
		set
		{
			serieComplementoPago = value;
		}
	}
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			idSucursal = value;
		}
	}
	
	public int IdUsuarioAlta
	{
		get { return idUsuarioAlta; }
		set
		{
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
	public CSerieComplementoPago()
	{
		idSerieComplementoPago = 0;
		serieComplementoPago = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CSerieComplementoPago(int pIdSerieComplementoPago)
	{
		idSerieComplementoPago = pIdSerieComplementoPago;
		serieComplementoPago = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieComplementoPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSerieComplementoPago>(typeof(CSerieComplementoPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieComplementoPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSerieComplementoPago>(typeof(CSerieComplementoPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieComplementoPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSerieComplementoPago", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSerieComplementoPago>(typeof(CSerieComplementoPago), pConexion);
		foreach (CSerieComplementoPago O in Obten.ListaRegistros)
		{
			idSerieComplementoPago = O.IdSerieComplementoPago;
			serieComplementoPago = O.SerieComplementoPago;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieComplementoPago_ConsultarFiltros";
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
		Obten.Llena<CSerieComplementoPago>(typeof(CSerieComplementoPago), pConexion);
		foreach (CSerieComplementoPago O in Obten.ListaRegistros)
		{
			idSerieComplementoPago = O.IdSerieComplementoPago;
			serieComplementoPago = O.SerieComplementoPago;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieComplementoPago_ConsultarFiltros";
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
		Obten.Llena<CSerieComplementoPago>(typeof(CSerieComplementoPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SerieComplementoPago_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSerieComplementoPago", 0);
		Agregar.StoredProcedure.Parameters["@pIdSerieComplementoPago"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSerieComplementoPago", serieComplementoPago);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSerieComplementoPago= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSerieComplementoPago"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SerieComplementoPago_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSerieComplementoPago", idSerieComplementoPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSerieComplementoPago", serieComplementoPago);
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
		Eliminar.StoredProcedure.CommandText = "spb_SerieComplementoPago_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSerieComplementoPago", idSerieComplementoPago);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
