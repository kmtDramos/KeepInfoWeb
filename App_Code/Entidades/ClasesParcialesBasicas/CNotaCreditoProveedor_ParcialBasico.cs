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

public partial class CNotaCreditoProveedor
{
	//Propiedades Privadas
	private int idNotaCreditoProveedor;
	private int folioNotaCredito;
	private string serieNotaCredito;
	private DateTime fecha;
	private int idProveedor;
	private decimal monto;
	private decimal iVA;
	private decimal total;
	private int idTipoMoneda;
	private decimal tipoCambio;
	private decimal saldoDocumento;
	private string referencia;
	private decimal porcentajeIVA;
	private string descripcion;
	private string totalLetra;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private DateTime fechaCancelacion;
	private string motivoCancelacion;
	private int idUsuarioCancelacion;
	private int idTipoNotaCredito;
	private string refid;
	private bool baja;
	
	//Propiedades
	public int IdNotaCreditoProveedor
	{
		get { return idNotaCreditoProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNotaCreditoProveedor = value;
		}
	}
	
	public int FolioNotaCredito
	{
		get { return folioNotaCredito; }
		set
		{
			if (value < 0)
			{
				return;
			}
			folioNotaCredito = value;
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
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int IdProveedor
	{
		get { return idProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProveedor = value;
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
	
	public decimal IVA
	{
		get { return iVA; }
		set
		{
			if (value < 0)
			{
				return;
			}
			iVA = value;
		}
	}
	
	public decimal Total
	{
		get { return total; }
		set
		{
			if (value < 0)
			{
				return;
			}
			total = value;
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
	
	public decimal SaldoDocumento
	{
		get { return saldoDocumento; }
		set
		{
			if (value < 0)
			{
				return;
			}
			saldoDocumento = value;
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
	
	public decimal PorcentajeIVA
	{
		get { return porcentajeIVA; }
		set
		{
			if (value < 0)
			{
				return;
			}
			porcentajeIVA = value;
		}
	}
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			descripcion = value;
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
	
	public DateTime FechaCancelacion
	{
		get { return fechaCancelacion; }
		set { fechaCancelacion = value; }
	}
	
	public string MotivoCancelacion
	{
		get { return motivoCancelacion; }
		set
		{
			motivoCancelacion = value;
		}
	}
	
	public int IdUsuarioCancelacion
	{
		get { return idUsuarioCancelacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioCancelacion = value;
		}
	}
	
	public int IdTipoNotaCredito
	{
		get { return idTipoNotaCredito; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoNotaCredito = value;
		}
	}
	
	public string Refid
	{
		get { return refid; }
		set
		{
			refid = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CNotaCreditoProveedor()
	{
		idNotaCreditoProveedor = 0;
		folioNotaCredito = 0;
		serieNotaCredito = "";
		fecha = new DateTime(1, 1, 1);
		idProveedor = 0;
		monto = 0;
		iVA = 0;
		total = 0;
		idTipoMoneda = 0;
		tipoCambio = 0;
		saldoDocumento = 0;
		referencia = "";
		porcentajeIVA = 0;
		descripcion = "";
		totalLetra = "";
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaCancelacion = new DateTime(1, 1, 1);
		motivoCancelacion = "";
		idUsuarioCancelacion = 0;
		idTipoNotaCredito = 0;
		refid = "";
		baja = false;
	}
	
	public CNotaCreditoProveedor(int pIdNotaCreditoProveedor)
	{
		idNotaCreditoProveedor = pIdNotaCreditoProveedor;
		folioNotaCredito = 0;
		serieNotaCredito = "";
		fecha = new DateTime(1, 1, 1);
		idProveedor = 0;
		monto = 0;
		iVA = 0;
		total = 0;
		idTipoMoneda = 0;
		tipoCambio = 0;
		saldoDocumento = 0;
		referencia = "";
		porcentajeIVA = 0;
		descripcion = "";
		totalLetra = "";
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaCancelacion = new DateTime(1, 1, 1);
		motivoCancelacion = "";
		idUsuarioCancelacion = 0;
		idTipoNotaCredito = 0;
		refid = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNotaCreditoProveedor>(typeof(CNotaCreditoProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CNotaCreditoProveedor>(typeof(CNotaCreditoProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNotaCreditoProveedor>(typeof(CNotaCreditoProveedor), pConexion);
		foreach (CNotaCreditoProveedor O in Obten.ListaRegistros)
		{
			idNotaCreditoProveedor = O.IdNotaCreditoProveedor;
			folioNotaCredito = O.FolioNotaCredito;
			serieNotaCredito = O.SerieNotaCredito;
			fecha = O.Fecha;
			idProveedor = O.IdProveedor;
			monto = O.Monto;
			iVA = O.IVA;
			total = O.Total;
			idTipoMoneda = O.IdTipoMoneda;
			tipoCambio = O.TipoCambio;
			saldoDocumento = O.SaldoDocumento;
			referencia = O.Referencia;
			porcentajeIVA = O.PorcentajeIVA;
			descripcion = O.Descripcion;
			totalLetra = O.TotalLetra;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			fechaCancelacion = O.FechaCancelacion;
			motivoCancelacion = O.MotivoCancelacion;
			idUsuarioCancelacion = O.IdUsuarioCancelacion;
			idTipoNotaCredito = O.IdTipoNotaCredito;
			refid = O.Refid;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_ConsultarFiltros";
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
		Obten.Llena<CNotaCreditoProveedor>(typeof(CNotaCreditoProveedor), pConexion);
		foreach (CNotaCreditoProveedor O in Obten.ListaRegistros)
		{
			idNotaCreditoProveedor = O.IdNotaCreditoProveedor;
			folioNotaCredito = O.FolioNotaCredito;
			serieNotaCredito = O.SerieNotaCredito;
			fecha = O.Fecha;
			idProveedor = O.IdProveedor;
			monto = O.Monto;
			iVA = O.IVA;
			total = O.Total;
			idTipoMoneda = O.IdTipoMoneda;
			tipoCambio = O.TipoCambio;
			saldoDocumento = O.SaldoDocumento;
			referencia = O.Referencia;
			porcentajeIVA = O.PorcentajeIVA;
			descripcion = O.Descripcion;
			totalLetra = O.TotalLetra;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			fechaCancelacion = O.FechaCancelacion;
			motivoCancelacion = O.MotivoCancelacion;
			idUsuarioCancelacion = O.IdUsuarioCancelacion;
			idTipoNotaCredito = O.IdTipoNotaCredito;
			refid = O.Refid;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_ConsultarFiltros";
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
		Obten.Llena<CNotaCreditoProveedor>(typeof(CNotaCreditoProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdNotaCreditoProveedor"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolioNotaCredito", folioNotaCredito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldoDocumento", saldoDocumento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeIVA", porcentajeIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaCancelacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoNotaCredito", idTipoNotaCredito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idNotaCreditoProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNotaCreditoProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolioNotaCredito", folioNotaCredito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoDocumento", saldoDocumento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeIVA", porcentajeIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaCancelacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoNotaCredito", idTipoNotaCredito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_NotaCreditoProveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}