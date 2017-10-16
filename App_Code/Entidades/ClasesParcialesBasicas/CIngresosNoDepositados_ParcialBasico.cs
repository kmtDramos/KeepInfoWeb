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

public partial class CIngresosNoDepositados
{
	//Propiedades Privadas
	private int idIngresosNoDepositados;
	private DateTime fechaMovimiento;
	private int idTipoDcocumento;
	private int idCuentaBancaria;
	private decimal importe;
	private int idTipoMoneda;
	private int idCliente;
	private int idUsuarioAlta;
	private DateTime fechaDeposito;
	private DateTime fechaEmision;
	private string referencia;
	private string conceptoGeneral;
	private bool depositado;
	private bool asociado;
	private int folio;
	private int idMetodoPago;
	private decimal tipoCambio;
	private int idDeposito;
	private decimal tipoCambioDeposito;
	private DateTime fechaPago;
	private bool baja;
	
	//Propiedades
	public int IdIngresosNoDepositados
	{
		get { return idIngresosNoDepositados; }
		set
		{
			idIngresosNoDepositados = value;
		}
	}
	
	public DateTime FechaMovimiento
	{
		get { return fechaMovimiento; }
		set { fechaMovimiento = value; }
	}
	
	public int IdTipoDcocumento
	{
		get { return idTipoDcocumento; }
		set
		{
			idTipoDcocumento = value;
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
	
	public decimal Importe
	{
		get { return importe; }
		set
		{
			importe = value;
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
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			idCliente = value;
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
	
	public DateTime FechaDeposito
	{
		get { return fechaDeposito; }
		set { fechaDeposito = value; }
	}
	
	public DateTime FechaEmision
	{
		get { return fechaEmision; }
		set { fechaEmision = value; }
	}
	
	public string Referencia
	{
		get { return referencia; }
		set
		{
			referencia = value;
		}
	}
	
	public string ConceptoGeneral
	{
		get { return conceptoGeneral; }
		set
		{
			conceptoGeneral = value;
		}
	}
	
	public bool Depositado
	{
		get { return depositado; }
		set { depositado = value; }
	}
	
	public bool Asociado
	{
		get { return asociado; }
		set { asociado = value; }
	}
	
	public int Folio
	{
		get { return folio; }
		set
		{
			folio = value;
		}
	}
	
	public int IdMetodoPago
	{
		get { return idMetodoPago; }
		set
		{
			idMetodoPago = value;
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
	
	public int IdDeposito
	{
		get { return idDeposito; }
		set
		{
			idDeposito = value;
		}
	}
	
	public decimal TipoCambioDeposito
	{
		get { return tipoCambioDeposito; }
		set
		{
			tipoCambioDeposito = value;
		}
	}
	
	public DateTime FechaPago
	{
		get { return fechaPago; }
		set { fechaPago = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CIngresosNoDepositados()
	{
		idIngresosNoDepositados = 0;
		fechaMovimiento = new DateTime(1, 1, 1);
		idTipoDcocumento = 0;
		idCuentaBancaria = 0;
		importe = 0;
		idTipoMoneda = 0;
		idCliente = 0;
		idUsuarioAlta = 0;
		fechaDeposito = new DateTime(1, 1, 1);
		fechaEmision = new DateTime(1, 1, 1);
		referencia = "";
		conceptoGeneral = "";
		depositado = false;
		asociado = false;
		folio = 0;
		idMetodoPago = 0;
		tipoCambio = 0;
		idDeposito = 0;
		tipoCambioDeposito = 0;
		fechaPago = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CIngresosNoDepositados(int pIdIngresosNoDepositados)
	{
		idIngresosNoDepositados = pIdIngresosNoDepositados;
		fechaMovimiento = new DateTime(1, 1, 1);
		idTipoDcocumento = 0;
		idCuentaBancaria = 0;
		importe = 0;
		idTipoMoneda = 0;
		idCliente = 0;
		idUsuarioAlta = 0;
		fechaDeposito = new DateTime(1, 1, 1);
		fechaEmision = new DateTime(1, 1, 1);
		referencia = "";
		conceptoGeneral = "";
		depositado = false;
		asociado = false;
		folio = 0;
		idMetodoPago = 0;
		tipoCambio = 0;
		idDeposito = 0;
		tipoCambioDeposito = 0;
		fechaPago = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IngresosNoDepositados_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CIngresosNoDepositados>(typeof(CIngresosNoDepositados), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IngresosNoDepositados_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CIngresosNoDepositados>(typeof(CIngresosNoDepositados), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IngresosNoDepositados_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CIngresosNoDepositados>(typeof(CIngresosNoDepositados), pConexion);
		foreach (CIngresosNoDepositados O in Obten.ListaRegistros)
		{
			idIngresosNoDepositados = O.IdIngresosNoDepositados;
			fechaMovimiento = O.FechaMovimiento;
			idTipoDcocumento = O.IdTipoDcocumento;
			idCuentaBancaria = O.IdCuentaBancaria;
			importe = O.Importe;
			idTipoMoneda = O.IdTipoMoneda;
			idCliente = O.IdCliente;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaDeposito = O.FechaDeposito;
			fechaEmision = O.FechaEmision;
			referencia = O.Referencia;
			conceptoGeneral = O.ConceptoGeneral;
			depositado = O.Depositado;
			asociado = O.Asociado;
			folio = O.Folio;
			idMetodoPago = O.IdMetodoPago;
			tipoCambio = O.TipoCambio;
			idDeposito = O.IdDeposito;
			tipoCambioDeposito = O.TipoCambioDeposito;
			fechaPago = O.FechaPago;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IngresosNoDepositados_ConsultarFiltros";
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
		Obten.Llena<CIngresosNoDepositados>(typeof(CIngresosNoDepositados), pConexion);
		foreach (CIngresosNoDepositados O in Obten.ListaRegistros)
		{
			idIngresosNoDepositados = O.IdIngresosNoDepositados;
			fechaMovimiento = O.FechaMovimiento;
			idTipoDcocumento = O.IdTipoDcocumento;
			idCuentaBancaria = O.IdCuentaBancaria;
			importe = O.Importe;
			idTipoMoneda = O.IdTipoMoneda;
			idCliente = O.IdCliente;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaDeposito = O.FechaDeposito;
			fechaEmision = O.FechaEmision;
			referencia = O.Referencia;
			conceptoGeneral = O.ConceptoGeneral;
			depositado = O.Depositado;
			asociado = O.Asociado;
			folio = O.Folio;
			idMetodoPago = O.IdMetodoPago;
			tipoCambio = O.TipoCambio;
			idDeposito = O.IdDeposito;
			tipoCambioDeposito = O.TipoCambioDeposito;
			fechaPago = O.FechaPago;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IngresosNoDepositados_ConsultarFiltros";
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
		Obten.Llena<CIngresosNoDepositados>(typeof(CIngresosNoDepositados), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_IngresosNoDepositados_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", 0);
		Agregar.StoredProcedure.Parameters["@pIdIngresosNoDepositados"].Direction = ParameterDirection.Output;
		if(fechaMovimiento.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDcocumento", idTipoDcocumento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pImporte", importe);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaDeposito.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaDeposito", fechaDeposito);
		}
		if(fechaEmision.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDepositado", depositado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDeposito", idDeposito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDeposito", tipoCambioDeposito);
		if(fechaPago.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idIngresosNoDepositados= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdIngresosNoDepositados"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_IngresosNoDepositados_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", idIngresosNoDepositados);
		if(fechaMovimiento.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDcocumento", idTipoDcocumento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pImporte", importe);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaDeposito.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaDeposito", fechaDeposito);
		}
		if(fechaEmision.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDepositado", depositado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDeposito", idDeposito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDeposito", tipoCambioDeposito);
		if(fechaPago.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_IngresosNoDepositados_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", idIngresosNoDepositados);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
