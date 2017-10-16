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

public partial class CSerieFactura
{
	//Propiedades Privadas
	private int idSerieFactura;
	private string serieFactura;
	private int idSucursal;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private bool timbrado;
	private bool esParcialidad;
	private bool esVenta;
	private bool baja;
	
	//Propiedades
	public int IdSerieFactura
	{
		get { return idSerieFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSerieFactura = value;
		}
	}
	
	public string SerieFactura
	{
		get { return serieFactura; }
		set
		{
			serieFactura = value;
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
	
	public bool Timbrado
	{
		get { return timbrado; }
		set { timbrado = value; }
	}
	
	public bool EsParcialidad
	{
		get { return esParcialidad; }
		set { esParcialidad = value; }
	}
	
	public bool EsVenta
	{
		get { return esVenta; }
		set { esVenta = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSerieFactura()
	{
		idSerieFactura = 0;
		serieFactura = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		timbrado = false;
		esParcialidad = false;
		esVenta = false;
		baja = false;
	}
	
	public CSerieFactura(int pIdSerieFactura)
	{
		idSerieFactura = pIdSerieFactura;
		serieFactura = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		timbrado = false;
		esParcialidad = false;
		esVenta = false;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSerieFactura>(typeof(CSerieFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSerieFactura>(typeof(CSerieFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSerieFactura>(typeof(CSerieFactura), pConexion);
		foreach (CSerieFactura O in Obten.ListaRegistros)
		{
			idSerieFactura = O.IdSerieFactura;
			serieFactura = O.SerieFactura;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			timbrado = O.Timbrado;
			esParcialidad = O.EsParcialidad;
			esVenta = O.EsVenta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieFactura_ConsultarFiltros";
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
		Obten.Llena<CSerieFactura>(typeof(CSerieFactura), pConexion);
		foreach (CSerieFactura O in Obten.ListaRegistros)
		{
			idSerieFactura = O.IdSerieFactura;
			serieFactura = O.SerieFactura;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			timbrado = O.Timbrado;
			esParcialidad = O.EsParcialidad;
			esVenta = O.EsVenta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SerieFactura_ConsultarFiltros";
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
		Obten.Llena<CSerieFactura>(typeof(CSerieFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SerieFactura_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", 0);
		Agregar.StoredProcedure.Parameters["@pIdSerieFactura"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSerieFactura", serieFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTimbrado", timbrado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsParcialidad", esParcialidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsVenta", esVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSerieFactura= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSerieFactura"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SerieFactura_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", idSerieFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSerieFactura", serieFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pTimbrado", timbrado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsParcialidad", esParcialidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsVenta", esVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SerieFactura_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", idSerieFactura);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}