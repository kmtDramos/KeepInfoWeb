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

public partial class CCuentasPorCobrarEncabezadoFactura
{
	//Propiedades Privadas
	private int idCuentasPorCobrarEncabezadoFactura;
	private DateTime fechaPago;
	private decimal monto;
	private string nota;
	private int idEncabezadoFactura;
	private int idCuentasPorCobrar;
	private int idUsuario;
	private int idTipoMoneda;
	private decimal tipoCambio;
	private bool baja;
	
	//Propiedades
	public int IdCuentasPorCobrarEncabezadoFactura
	{
		get { return idCuentasPorCobrarEncabezadoFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCuentasPorCobrarEncabezadoFactura = value;
		}
	}
	
	public DateTime FechaPago
	{
		get { return fechaPago; }
		set { fechaPago = value; }
	}
	
	public decimal Monto
	{
		get { return monto; }
		set
		{
			if (value < 0)
			{
				return;
			}
			monto = value;
		}
	}
	
	public string Nota
	{
		get { return nota; }
		set
		{
			nota = value;
		}
	}
	
	public int IdEncabezadoFactura
	{
		get { return idEncabezadoFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEncabezadoFactura = value;
		}
	}
	
	public int IdCuentasPorCobrar
	{
		get { return idCuentasPorCobrar; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCuentasPorCobrar = value;
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
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMoneda = value;
		}
	}
	
	public decimal TipoCambio
	{
		get { return tipoCambio; }
		set
		{
			if (value < 0)
			{
				return;
			}
			tipoCambio = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CCuentasPorCobrarEncabezadoFactura()
	{
		idCuentasPorCobrarEncabezadoFactura = 0;
		fechaPago = new DateTime(1, 1, 1);
		monto = 0;
		nota = "";
		idEncabezadoFactura = 0;
		idCuentasPorCobrar = 0;
		idUsuario = 0;
		idTipoMoneda = 0;
		tipoCambio = 0;
		baja = false;
	}
	
	public CCuentasPorCobrarEncabezadoFactura(int pIdCuentasPorCobrarEncabezadoFactura)
	{
		idCuentasPorCobrarEncabezadoFactura = pIdCuentasPorCobrarEncabezadoFactura;
		fechaPago = new DateTime(1, 1, 1);
		monto = 0;
		nota = "";
		idEncabezadoFactura = 0;
		idCuentasPorCobrar = 0;
		idUsuario = 0;
		idTipoMoneda = 0;
		tipoCambio = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentasPorCobrarEncabezadoFactura>(typeof(CCuentasPorCobrarEncabezadoFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CCuentasPorCobrarEncabezadoFactura>(typeof(CCuentasPorCobrarEncabezadoFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrarEncabezadoFactura", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentasPorCobrarEncabezadoFactura>(typeof(CCuentasPorCobrarEncabezadoFactura), pConexion);
		foreach (CCuentasPorCobrarEncabezadoFactura O in Obten.ListaRegistros)
		{
			idCuentasPorCobrarEncabezadoFactura = O.IdCuentasPorCobrarEncabezadoFactura;
			fechaPago = O.FechaPago;
			monto = O.Monto;
			nota = O.Nota;
			idEncabezadoFactura = O.IdEncabezadoFactura;
			idCuentasPorCobrar = O.IdCuentasPorCobrar;
			idUsuario = O.IdUsuario;
			idTipoMoneda = O.IdTipoMoneda;
			tipoCambio = O.TipoCambio;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_ConsultarFiltros";
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
		Obten.Llena<CCuentasPorCobrarEncabezadoFactura>(typeof(CCuentasPorCobrarEncabezadoFactura), pConexion);
		foreach (CCuentasPorCobrarEncabezadoFactura O in Obten.ListaRegistros)
		{
			idCuentasPorCobrarEncabezadoFactura = O.IdCuentasPorCobrarEncabezadoFactura;
			fechaPago = O.FechaPago;
			monto = O.Monto;
			nota = O.Nota;
			idEncabezadoFactura = O.IdEncabezadoFactura;
			idCuentasPorCobrar = O.IdCuentasPorCobrar;
			idUsuario = O.IdUsuario;
			idTipoMoneda = O.IdTipoMoneda;
			tipoCambio = O.TipoCambio;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_ConsultarFiltros";
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
		Obten.Llena<CCuentasPorCobrarEncabezadoFactura>(typeof(CCuentasPorCobrarEncabezadoFactura), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrarEncabezadoFactura", 0);
		Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrarEncabezadoFactura"].Direction = ParameterDirection.Output;
		if(fechaPago.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", idCuentasPorCobrar);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idCuentasPorCobrarEncabezadoFactura= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrarEncabezadoFactura"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrarEncabezadoFactura", idCuentasPorCobrarEncabezadoFactura);
		if(fechaPago.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", idCuentasPorCobrar);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_CuentasPorCobrarEncabezadoFactura_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrarEncabezadoFactura", idCuentasPorCobrarEncabezadoFactura);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}