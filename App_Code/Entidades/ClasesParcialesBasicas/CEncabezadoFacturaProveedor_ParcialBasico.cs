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

public partial class CEncabezadoFacturaProveedor
{
	//Propiedades Privadas
	private int idEncabezadoFacturaProveedor;
	private string serie;
	private int folio;
	private decimal total;
	private decimal saldo;
	private int idProveedor;
	private DateTime fecha;
	private int idEstatusEncabezadoFacturaProveedor;
	private int idTipoMoneda;
	private string numeroFactura;
	private DateTime fechaPago;
	private decimal subtotal;
	private decimal iVA;
	private string cheque;
	private decimal descuento;
	private string cuentaBancaria;
	private int idDivision;
	private int idUsuario;
	private decimal tipoCambio;
	private DateTime fechaCaptura;
	private string numeroGuia;
	private string totalLetra;
	private int idAlmacen;
	private bool seGeneroAsiento;
	private int idCondicionPago;
	private DateTime fechaRevision;
	private int idUsuarioRevision;
	private bool baja;
	
	//Propiedades
	public int IdEncabezadoFacturaProveedor
	{
		get { return idEncabezadoFacturaProveedor; }
		set
		{
			idEncabezadoFacturaProveedor = value;
		}
	}
	
	public string Serie
	{
		get { return serie; }
		set
		{
			serie = value;
		}
	}
	
	public int Folio
	{
		get { return folio; }
		set
		{
			folio = value;
		}
	}
	
	public decimal Total
	{
		get { return total; }
		set
		{
			total = value;
		}
	}
	
	public decimal Saldo
	{
		get { return saldo; }
		set
		{
			saldo = value;
		}
	}
	
	public int IdProveedor
	{
		get { return idProveedor; }
		set
		{
			idProveedor = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int IdEstatusEncabezadoFacturaProveedor
	{
		get { return idEstatusEncabezadoFacturaProveedor; }
		set
		{
			idEstatusEncabezadoFacturaProveedor = value;
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
	
	public string NumeroFactura
	{
		get { return numeroFactura; }
		set
		{
			numeroFactura = value;
		}
	}
	
	public DateTime FechaPago
	{
		get { return fechaPago; }
		set { fechaPago = value; }
	}
	
	public decimal Subtotal
	{
		get { return subtotal; }
		set
		{
			subtotal = value;
		}
	}
	
	public decimal IVA
	{
		get { return iVA; }
		set
		{
			iVA = value;
		}
	}
	
	public string Cheque
	{
		get { return cheque; }
		set
		{
			cheque = value;
		}
	}
	
	public decimal Descuento
	{
		get { return descuento; }
		set
		{
			descuento = value;
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
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
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
	
	public DateTime FechaCaptura
	{
		get { return fechaCaptura; }
		set { fechaCaptura = value; }
	}
	
	public string NumeroGuia
	{
		get { return numeroGuia; }
		set
		{
			numeroGuia = value;
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
	
	public int IdAlmacen
	{
		get { return idAlmacen; }
		set
		{
			idAlmacen = value;
		}
	}
	
	public bool SeGeneroAsiento
	{
		get { return seGeneroAsiento; }
		set { seGeneroAsiento = value; }
	}
	
	public int IdCondicionPago
	{
		get { return idCondicionPago; }
		set
		{
			idCondicionPago = value;
		}
	}
	
	public DateTime FechaRevision
	{
		get { return fechaRevision; }
		set { fechaRevision = value; }
	}
	
	public int IdUsuarioRevision
	{
		get { return idUsuarioRevision; }
		set
		{
			idUsuarioRevision = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CEncabezadoFacturaProveedor()
	{
		idEncabezadoFacturaProveedor = 0;
		serie = "";
		folio = 0;
		total = 0;
		saldo = 0;
		idProveedor = 0;
		fecha = new DateTime(1, 1, 1);
		idEstatusEncabezadoFacturaProveedor = 0;
		idTipoMoneda = 0;
		numeroFactura = "";
		fechaPago = new DateTime(1, 1, 1);
		subtotal = 0;
		iVA = 0;
		cheque = "";
		descuento = 0;
		cuentaBancaria = "";
		idDivision = 0;
		idUsuario = 0;
		tipoCambio = 0;
		fechaCaptura = new DateTime(1, 1, 1);
		numeroGuia = "";
		totalLetra = "";
		idAlmacen = 0;
		seGeneroAsiento = false;
		idCondicionPago = 0;
		fechaRevision = new DateTime(1, 1, 1);
		idUsuarioRevision = 0;
		baja = false;
	}
	
	public CEncabezadoFacturaProveedor(int pIdEncabezadoFacturaProveedor)
	{
		idEncabezadoFacturaProveedor = pIdEncabezadoFacturaProveedor;
		serie = "";
		folio = 0;
		total = 0;
		saldo = 0;
		idProveedor = 0;
		fecha = new DateTime(1, 1, 1);
		idEstatusEncabezadoFacturaProveedor = 0;
		idTipoMoneda = 0;
		numeroFactura = "";
		fechaPago = new DateTime(1, 1, 1);
		subtotal = 0;
		iVA = 0;
		cheque = "";
		descuento = 0;
		cuentaBancaria = "";
		idDivision = 0;
		idUsuario = 0;
		tipoCambio = 0;
		fechaCaptura = new DateTime(1, 1, 1);
		numeroGuia = "";
		totalLetra = "";
		idAlmacen = 0;
		seGeneroAsiento = false;
		idCondicionPago = 0;
		fechaRevision = new DateTime(1, 1, 1);
		idUsuarioRevision = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEncabezadoFacturaProveedor>(typeof(CEncabezadoFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CEncabezadoFacturaProveedor>(typeof(CEncabezadoFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEncabezadoFacturaProveedor>(typeof(CEncabezadoFacturaProveedor), pConexion);
		foreach (CEncabezadoFacturaProveedor O in Obten.ListaRegistros)
		{
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			serie = O.Serie;
			folio = O.Folio;
			total = O.Total;
			saldo = O.Saldo;
			idProveedor = O.IdProveedor;
			fecha = O.Fecha;
			idEstatusEncabezadoFacturaProveedor = O.IdEstatusEncabezadoFacturaProveedor;
			idTipoMoneda = O.IdTipoMoneda;
			numeroFactura = O.NumeroFactura;
			fechaPago = O.FechaPago;
			subtotal = O.Subtotal;
			iVA = O.IVA;
			cheque = O.Cheque;
			descuento = O.Descuento;
			cuentaBancaria = O.CuentaBancaria;
			idDivision = O.IdDivision;
			idUsuario = O.IdUsuario;
			tipoCambio = O.TipoCambio;
			fechaCaptura = O.FechaCaptura;
			numeroGuia = O.NumeroGuia;
			totalLetra = O.TotalLetra;
			idAlmacen = O.IdAlmacen;
			seGeneroAsiento = O.SeGeneroAsiento;
			idCondicionPago = O.IdCondicionPago;
			fechaRevision = O.FechaRevision;
			idUsuarioRevision = O.IdUsuarioRevision;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CEncabezadoFacturaProveedor>(typeof(CEncabezadoFacturaProveedor), pConexion);
		foreach (CEncabezadoFacturaProveedor O in Obten.ListaRegistros)
		{
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			serie = O.Serie;
			folio = O.Folio;
			total = O.Total;
			saldo = O.Saldo;
			idProveedor = O.IdProveedor;
			fecha = O.Fecha;
			idEstatusEncabezadoFacturaProveedor = O.IdEstatusEncabezadoFacturaProveedor;
			idTipoMoneda = O.IdTipoMoneda;
			numeroFactura = O.NumeroFactura;
			fechaPago = O.FechaPago;
			subtotal = O.Subtotal;
			iVA = O.IVA;
			cheque = O.Cheque;
			descuento = O.Descuento;
			cuentaBancaria = O.CuentaBancaria;
			idDivision = O.IdDivision;
			idUsuario = O.IdUsuario;
			tipoCambio = O.TipoCambio;
			fechaCaptura = O.FechaCaptura;
			numeroGuia = O.NumeroGuia;
			totalLetra = O.TotalLetra;
			idAlmacen = O.IdAlmacen;
			seGeneroAsiento = O.SeGeneroAsiento;
			idCondicionPago = O.IdCondicionPago;
			fechaRevision = O.FechaRevision;
			idUsuarioRevision = O.IdUsuarioRevision;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CEncabezadoFacturaProveedor>(typeof(CEncabezadoFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdEncabezadoFacturaProveedor"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSerie", serie);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusEncabezadoFacturaProveedor", idEstatusEncabezadoFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", numeroFactura);
		if(fechaPago.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCheque", cheque);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", cuentaBancaria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fechaCaptura.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCaptura", fechaCaptura);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroGuia", numeroGuia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
		if(fechaRevision.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRevision", fechaRevision);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioRevision", idUsuarioRevision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idEncabezadoFacturaProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEncabezadoFacturaProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSerie", serie);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusEncabezadoFacturaProveedor", idEstatusEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", numeroFactura);
		if(fechaPago.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCheque", cheque);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", cuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fechaCaptura.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCaptura", fechaCaptura);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroGuia", numeroGuia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
		if(fechaRevision.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRevision", fechaRevision);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioRevision", idUsuarioRevision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_EncabezadoFacturaProveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
