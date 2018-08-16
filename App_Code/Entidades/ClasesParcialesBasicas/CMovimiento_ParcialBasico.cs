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

public partial class CMovimiento
{
	//Propiedades Privadas
	private int idMovimiento;
	private string referencia;
	private int idCuentaBancaria;
	private int idTipoMovimiento;
	private int idTipoMoneda;
	private int idUsuarioAlta;
	private int idUsuarioCancelacion;
	private DateTime fechaAlta;
	private DateTime fechaMovimiento;
	private DateTime fechaCancelacion;
	private decimal monto;
	private decimal saldoInicial;
	private decimal saldoFinal;
	private decimal tipoCambio;
	private int idOrganizacion;
	private int idFlujoCaja;
	private bool baja;
	
	//Propiedades
	public int IdMovimiento
	{
		get { return idMovimiento; }
		set
		{
			idMovimiento = value;
		}
	}
	
	public string Referencia
	{
		get { return referencia; }
		set
		{
			referencia = value;
		}
	}
	
	public int IdCuentaBancaria
	{
		get { return idCuentaBancaria; }
		set
		{
			idCuentaBancaria = value;
		}
	}
	
	public int IdTipoMovimiento
	{
		get { return idTipoMovimiento; }
		set
		{
			idTipoMovimiento = value;
		}
	}
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			idTipoMoneda = value;
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
	
	public int IdUsuarioCancelacion
	{
		get { return idUsuarioCancelacion; }
		set
		{
			idUsuarioCancelacion = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaMovimiento
	{
		get { return fechaMovimiento; }
		set { fechaMovimiento = value; }
	}
	
	public DateTime FechaCancelacion
	{
		get { return fechaCancelacion; }
		set { fechaCancelacion = value; }
	}
	
	public decimal Monto
	{
		get { return monto; }
		set
		{
			monto = value;
		}
	}
	
	public decimal SaldoInicial
	{
		get { return saldoInicial; }
		set
		{
			saldoInicial = value;
		}
	}
	
	public decimal SaldoFinal
	{
		get { return saldoFinal; }
		set
		{
			saldoFinal = value;
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
	
	public int IdOrganizacion
	{
		get { return idOrganizacion; }
		set
		{
			idOrganizacion = value;
		}
	}
	
	public int IdFlujoCaja
	{
		get { return idFlujoCaja; }
		set
		{
			idFlujoCaja = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CMovimiento()
	{
		idMovimiento = 0;
		referencia = "";
		idCuentaBancaria = 0;
		idTipoMovimiento = 0;
		idTipoMoneda = 0;
		idUsuarioAlta = 0;
		idUsuarioCancelacion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaMovimiento = new DateTime(1, 1, 1);
		fechaCancelacion = new DateTime(1, 1, 1);
		monto = 0;
		saldoInicial = 0;
		saldoFinal = 0;
		tipoCambio = 0;
		idOrganizacion = 0;
		idFlujoCaja = 0;
		baja = false;
	}
	
	public CMovimiento(int pIdMovimiento)
	{
		idMovimiento = pIdMovimiento;
		referencia = "";
		idCuentaBancaria = 0;
		idTipoMovimiento = 0;
		idTipoMoneda = 0;
		idUsuarioAlta = 0;
		idUsuarioCancelacion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaMovimiento = new DateTime(1, 1, 1);
		fechaCancelacion = new DateTime(1, 1, 1);
		monto = 0;
		saldoInicial = 0;
		saldoFinal = 0;
		tipoCambio = 0;
		idOrganizacion = 0;
		idFlujoCaja = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Movimiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMovimiento>(typeof(CMovimiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Movimiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CMovimiento>(typeof(CMovimiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Movimiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdMovimiento", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMovimiento>(typeof(CMovimiento), pConexion);
		foreach (CMovimiento O in Obten.ListaRegistros)
		{
			idMovimiento = O.IdMovimiento;
			referencia = O.Referencia;
			idCuentaBancaria = O.IdCuentaBancaria;
			idTipoMovimiento = O.IdTipoMovimiento;
			idTipoMoneda = O.IdTipoMoneda;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioCancelacion = O.IdUsuarioCancelacion;
			fechaAlta = O.FechaAlta;
			fechaMovimiento = O.FechaMovimiento;
			fechaCancelacion = O.FechaCancelacion;
			monto = O.Monto;
			saldoInicial = O.SaldoInicial;
			saldoFinal = O.SaldoFinal;
			tipoCambio = O.TipoCambio;
			idOrganizacion = O.IdOrganizacion;
			idFlujoCaja = O.IdFlujoCaja;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Movimiento_ConsultarFiltros";
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
		Obten.Llena<CMovimiento>(typeof(CMovimiento), pConexion);
		foreach (CMovimiento O in Obten.ListaRegistros)
		{
			idMovimiento = O.IdMovimiento;
			referencia = O.Referencia;
			idCuentaBancaria = O.IdCuentaBancaria;
			idTipoMovimiento = O.IdTipoMovimiento;
			idTipoMoneda = O.IdTipoMoneda;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioCancelacion = O.IdUsuarioCancelacion;
			fechaAlta = O.FechaAlta;
			fechaMovimiento = O.FechaMovimiento;
			fechaCancelacion = O.FechaCancelacion;
			monto = O.Monto;
			saldoInicial = O.SaldoInicial;
			saldoFinal = O.SaldoFinal;
			tipoCambio = O.TipoCambio;
			idOrganizacion = O.IdOrganizacion;
			idFlujoCaja = O.IdFlujoCaja;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Movimiento_ConsultarFiltros";
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
		Obten.Llena<CMovimiento>(typeof(CMovimiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Movimiento_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMovimiento", 0);
		Agregar.StoredProcedure.Parameters["@pIdMovimiento"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimiento", idTipoMovimiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaMovimiento.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
		}
		if(fechaCancelacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldoInicial", saldoInicial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldoFinal", saldoFinal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFlujoCaja", idFlujoCaja);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idMovimiento= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdMovimiento"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Movimiento_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMovimiento", idMovimiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimiento", idTipoMovimiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaMovimiento.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
		}
		if(fechaCancelacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoInicial", saldoInicial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoFinal", saldoFinal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFlujoCaja", idFlujoCaja);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Movimiento_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdMovimiento", idMovimiento);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
