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

public partial class CCuentasPorCobrar
{
	//Propiedades Privadas
	private int idCuentasPorCobrar;
	private DateTime fechaMovimiento;
	private int idTipoDcocumento;
	private int idCuentaBancaria;
	private decimal importe;
	private int idTipoMoneda;
	private int idCliente;
	private int idUsuarioAlta;
	private DateTime fechaAplicacion;
	private DateTime fechaEmision;
	private string referencia;
	private string conceptoGeneral;
	private bool conciliado;
	private bool asociado;
	private int folio;
	private int idMetodoPago;
	private decimal tipoCambio;
	private bool seGeneroAsiento;
	private DateTime fechaConciliacion;
	private string refid;
	private string totalLetra;
	private int idSeriePago;
	private bool baja;
	
	//Propiedades
	public int IdCuentasPorCobrar
	{
		get { return idCuentasPorCobrar; }
		set
		{
			idCuentasPorCobrar = value;
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
	
	public DateTime FechaAplicacion
	{
		get { return fechaAplicacion; }
		set { fechaAplicacion = value; }
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
	
	public bool Conciliado
	{
		get { return conciliado; }
		set { conciliado = value; }
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
	
	public bool SeGeneroAsiento
	{
		get { return seGeneroAsiento; }
		set { seGeneroAsiento = value; }
	}
	
	public DateTime FechaConciliacion
	{
		get { return fechaConciliacion; }
		set { fechaConciliacion = value; }
	}
	
	public string Refid
	{
		get { return refid; }
		set
		{
			refid = value;
		}
	}
	
	public string TotalLetra
	{
		get { return totalLetra; }
		set
		{
			totalLetra = value;
		}
	}
	
	public int IdSeriePago
	{
		get { return idSeriePago; }
		set
		{
			idSeriePago = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CCuentasPorCobrar()
	{
		idCuentasPorCobrar = 0;
		fechaMovimiento = new DateTime(1, 1, 1);
		idTipoDcocumento = 0;
		idCuentaBancaria = 0;
		importe = 0;
		idTipoMoneda = 0;
		idCliente = 0;
		idUsuarioAlta = 0;
		fechaAplicacion = new DateTime(1, 1, 1);
		fechaEmision = new DateTime(1, 1, 1);
		referencia = "";
		conceptoGeneral = "";
		conciliado = false;
		asociado = false;
		folio = 0;
		idMetodoPago = 0;
		tipoCambio = 0;
		seGeneroAsiento = false;
		fechaConciliacion = new DateTime(1, 1, 1);
		refid = "";
		totalLetra = "";
		idSeriePago = 0;
		baja = false;
	}
	
	public CCuentasPorCobrar(int pIdCuentasPorCobrar)
	{
		idCuentasPorCobrar = pIdCuentasPorCobrar;
		fechaMovimiento = new DateTime(1, 1, 1);
		idTipoDcocumento = 0;
		idCuentaBancaria = 0;
		importe = 0;
		idTipoMoneda = 0;
		idCliente = 0;
		idUsuarioAlta = 0;
		fechaAplicacion = new DateTime(1, 1, 1);
		fechaEmision = new DateTime(1, 1, 1);
		referencia = "";
		conceptoGeneral = "";
		conciliado = false;
		asociado = false;
		folio = 0;
		idMetodoPago = 0;
		tipoCambio = 0;
		seGeneroAsiento = false;
		fechaConciliacion = new DateTime(1, 1, 1);
		refid = "";
		totalLetra = "";
		idSeriePago = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrar_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentasPorCobrar>(typeof(CCuentasPorCobrar), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrar_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CCuentasPorCobrar>(typeof(CCuentasPorCobrar), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrar_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentasPorCobrar>(typeof(CCuentasPorCobrar), pConexion);
		foreach (CCuentasPorCobrar O in Obten.ListaRegistros)
		{
			idCuentasPorCobrar = O.IdCuentasPorCobrar;
			fechaMovimiento = O.FechaMovimiento;
			idTipoDcocumento = O.IdTipoDcocumento;
			idCuentaBancaria = O.IdCuentaBancaria;
			importe = O.Importe;
			idTipoMoneda = O.IdTipoMoneda;
			idCliente = O.IdCliente;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAplicacion = O.FechaAplicacion;
			fechaEmision = O.FechaEmision;
			referencia = O.Referencia;
			conceptoGeneral = O.ConceptoGeneral;
			conciliado = O.Conciliado;
			asociado = O.Asociado;
			folio = O.Folio;
			idMetodoPago = O.IdMetodoPago;
			tipoCambio = O.TipoCambio;
			seGeneroAsiento = O.SeGeneroAsiento;
			fechaConciliacion = O.FechaConciliacion;
			refid = O.Refid;
			totalLetra = O.TotalLetra;
			idSeriePago = O.IdSeriePago;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrar_ConsultarFiltros";
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
		Obten.Llena<CCuentasPorCobrar>(typeof(CCuentasPorCobrar), pConexion);
		foreach (CCuentasPorCobrar O in Obten.ListaRegistros)
		{
			idCuentasPorCobrar = O.IdCuentasPorCobrar;
			fechaMovimiento = O.FechaMovimiento;
			idTipoDcocumento = O.IdTipoDcocumento;
			idCuentaBancaria = O.IdCuentaBancaria;
			importe = O.Importe;
			idTipoMoneda = O.IdTipoMoneda;
			idCliente = O.IdCliente;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAplicacion = O.FechaAplicacion;
			fechaEmision = O.FechaEmision;
			referencia = O.Referencia;
			conceptoGeneral = O.ConceptoGeneral;
			conciliado = O.Conciliado;
			asociado = O.Asociado;
			folio = O.Folio;
			idMetodoPago = O.IdMetodoPago;
			tipoCambio = O.TipoCambio;
			seGeneroAsiento = O.SeGeneroAsiento;
			fechaConciliacion = O.FechaConciliacion;
			refid = O.Refid;
			totalLetra = O.TotalLetra;
			idSeriePago = O.IdSeriePago;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentasPorCobrar_ConsultarFiltros";
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
		Obten.Llena<CCuentasPorCobrar>(typeof(CCuentasPorCobrar), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_CuentasPorCobrar_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", 0);
		Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrar"].Direction = ParameterDirection.Output;
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
		if(fechaAplicacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAplicacion", fechaAplicacion);
		}
		if(fechaEmision.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pConciliado", conciliado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
		if(fechaConciliacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaConciliacion", fechaConciliacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSeriePago", idSeriePago);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idCuentasPorCobrar= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrar"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_CuentasPorCobrar_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", idCuentasPorCobrar);
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
		if(fechaAplicacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAplicacion", fechaAplicacion);
		}
		if(fechaEmision.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
		Editar.StoredProcedure.Parameters.AddWithValue("@pConciliado", conciliado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
		if(fechaConciliacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaConciliacion", fechaConciliacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSeriePago", idSeriePago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_CuentasPorCobrar_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", idCuentasPorCobrar);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
