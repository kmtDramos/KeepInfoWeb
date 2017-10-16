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

public partial class CMovimientosCuenta
{
	//Propiedades Privadas
	private int idMovimientosCuenta;
	private DateTime fechaMovimiento;
	private decimal saldoInicial;
	private decimal monto;
	private decimal saldoActual;
	private string cuentaBancaria;
	private int idIngreso;
	private int idEgreso;
	private int idCheque;
	private int idDeposito;
	private string notas;
	private bool traspaso;
	private int idUsuarioAlta;
	private int idTipoMoneda;
	private decimal tipoCambio;
	private bool baja;
	
	//Propiedades
	public int IdMovimientosCuenta
	{
		get { return idMovimientosCuenta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idMovimientosCuenta = value;
		}
	}
	
	public DateTime FechaMovimiento
	{
		get { return fechaMovimiento; }
		set { fechaMovimiento = value; }
	}
	
	public decimal SaldoInicial
	{
		get { return saldoInicial; }
		set
		{
			if (value < 0)
			{
				return;
			}
			saldoInicial = value;
		}
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
	
	public decimal SaldoActual
	{
		get { return saldoActual; }
		set
		{
			if (value < 0)
			{
				return;
			}
			saldoActual = value;
		}
	}
	
	public string CuentaBancaria
	{
		get { return cuentaBancaria; }
		set
		{
			cuentaBancaria = value;
		}
	}
	
	public int IdIngreso
	{
		get { return idIngreso; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idIngreso = value;
		}
	}
	
	public int IdEgreso
	{
		get { return idEgreso; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEgreso = value;
		}
	}
	
	public int IdCheque
	{
		get { return idCheque; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCheque = value;
		}
	}
	
	public int IdDeposito
	{
		get { return idDeposito; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDeposito = value;
		}
	}
	
	public string Notas
	{
		get { return notas; }
		set
		{
			notas = value;
		}
	}
	
	public bool Traspaso
	{
		get { return traspaso; }
		set { traspaso = value; }
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
	public CMovimientosCuenta()
	{
		idMovimientosCuenta = 0;
		fechaMovimiento = new DateTime(1, 1, 1);
		saldoInicial = 0;
		monto = 0;
		saldoActual = 0;
		cuentaBancaria = "";
		idIngreso = 0;
		idEgreso = 0;
		idCheque = 0;
		idDeposito = 0;
		notas = "";
		traspaso = false;
		idUsuarioAlta = 0;
		idTipoMoneda = 0;
		tipoCambio = 0;
		baja = false;
	}
	
	public CMovimientosCuenta(int pIdMovimientosCuenta)
	{
		idMovimientosCuenta = pIdMovimientosCuenta;
		fechaMovimiento = new DateTime(1, 1, 1);
		saldoInicial = 0;
		monto = 0;
		saldoActual = 0;
		cuentaBancaria = "";
		idIngreso = 0;
		idEgreso = 0;
		idCheque = 0;
		idDeposito = 0;
		notas = "";
		traspaso = false;
		idUsuarioAlta = 0;
		idTipoMoneda = 0;
		tipoCambio = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosCuenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMovimientosCuenta>(typeof(CMovimientosCuenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosCuenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CMovimientosCuenta>(typeof(CMovimientosCuenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosCuenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosCuenta", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMovimientosCuenta>(typeof(CMovimientosCuenta), pConexion);
		foreach (CMovimientosCuenta O in Obten.ListaRegistros)
		{
			idMovimientosCuenta = O.IdMovimientosCuenta;
			fechaMovimiento = O.FechaMovimiento;
			saldoInicial = O.SaldoInicial;
			monto = O.Monto;
			saldoActual = O.SaldoActual;
			cuentaBancaria = O.CuentaBancaria;
			idIngreso = O.IdIngreso;
			idEgreso = O.IdEgreso;
			idCheque = O.IdCheque;
			idDeposito = O.IdDeposito;
			notas = O.Notas;
			traspaso = O.Traspaso;
			idUsuarioAlta = O.IdUsuarioAlta;
			idTipoMoneda = O.IdTipoMoneda;
			tipoCambio = O.TipoCambio;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosCuenta_ConsultarFiltros";
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
		Obten.Llena<CMovimientosCuenta>(typeof(CMovimientosCuenta), pConexion);
		foreach (CMovimientosCuenta O in Obten.ListaRegistros)
		{
			idMovimientosCuenta = O.IdMovimientosCuenta;
			fechaMovimiento = O.FechaMovimiento;
			saldoInicial = O.SaldoInicial;
			monto = O.Monto;
			saldoActual = O.SaldoActual;
			cuentaBancaria = O.CuentaBancaria;
			idIngreso = O.IdIngreso;
			idEgreso = O.IdEgreso;
			idCheque = O.IdCheque;
			idDeposito = O.IdDeposito;
			notas = O.Notas;
			traspaso = O.Traspaso;
			idUsuarioAlta = O.IdUsuarioAlta;
			idTipoMoneda = O.IdTipoMoneda;
			tipoCambio = O.TipoCambio;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosCuenta_ConsultarFiltros";
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
		Obten.Llena<CMovimientosCuenta>(typeof(CMovimientosCuenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_MovimientosCuenta_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosCuenta", 0);
		Agregar.StoredProcedure.Parameters["@pIdMovimientosCuenta"].Direction = ParameterDirection.Output;
		if(fechaMovimiento.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldoInicial", saldoInicial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldoActual", saldoActual);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", cuentaBancaria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngreso", idIngreso);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEgreso", idEgreso);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCheque", idCheque);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDeposito", idDeposito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTraspaso", traspaso);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idMovimientosCuenta= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdMovimientosCuenta"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_MovimientosCuenta_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosCuenta", idMovimientosCuenta);
		if(fechaMovimiento.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoInicial", saldoInicial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoActual", saldoActual);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", cuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdIngreso", idIngreso);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEgreso", idEgreso);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCheque", idCheque);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDeposito", idDeposito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTraspaso", traspaso);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_MovimientosCuenta_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosCuenta", idMovimientosCuenta);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}