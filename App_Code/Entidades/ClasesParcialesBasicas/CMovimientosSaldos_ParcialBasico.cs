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

public partial class CMovimientosSaldos
{
	//Propiedades Privadas
	private int idMovimientosSaldos;
	private DateTime fecha;
	private string tipo;
	private int idIngresosEncabezadoFactura;
	private int idEgresosEncabezadoFacturaProveedor;
	private int idIngresosNoDepositadosEncabezadoFactura;
	private int idChequesEncabezadoFacturaProveedor;
	private decimal monto;
	private int idTipoMoneda;
	private int idCliente;
	private int idProveedor;
	private int idUsuario;
	private int idEncabezadoFacturaProveedor;
	private int idEncabezadoFactura;
	private string notas;
	private decimal tipoCambio;
	private int idNotaCreditoEncabezadoFactura;
	private bool baja;
	
	//Propiedades
	public int IdMovimientosSaldos
	{
		get { return idMovimientosSaldos; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idMovimientosSaldos = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public string Tipo
	{
		get { return tipo; }
		set
		{
			tipo = value;
		}
	}
	
	public int IdIngresosEncabezadoFactura
	{
		get { return idIngresosEncabezadoFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idIngresosEncabezadoFactura = value;
		}
	}
	
	public int IdEgresosEncabezadoFacturaProveedor
	{
		get { return idEgresosEncabezadoFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEgresosEncabezadoFacturaProveedor = value;
		}
	}
	
	public int IdIngresosNoDepositadosEncabezadoFactura
	{
		get { return idIngresosNoDepositadosEncabezadoFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idIngresosNoDepositadosEncabezadoFactura = value;
		}
	}
	
	public int IdChequesEncabezadoFacturaProveedor
	{
		get { return idChequesEncabezadoFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idChequesEncabezadoFacturaProveedor = value;
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
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCliente = value;
		}
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
	
	public int IdEncabezadoFacturaProveedor
	{
		get { return idEncabezadoFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEncabezadoFacturaProveedor = value;
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
	
	public string Notas
	{
		get { return notas; }
		set
		{
			notas = value;
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
	
	public int IdNotaCreditoEncabezadoFactura
	{
		get { return idNotaCreditoEncabezadoFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNotaCreditoEncabezadoFactura = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CMovimientosSaldos()
	{
		idMovimientosSaldos = 0;
		fecha = new DateTime(1, 1, 1);
		tipo = "";
		idIngresosEncabezadoFactura = 0;
		idEgresosEncabezadoFacturaProveedor = 0;
		idIngresosNoDepositadosEncabezadoFactura = 0;
		idChequesEncabezadoFacturaProveedor = 0;
		monto = 0;
		idTipoMoneda = 0;
		idCliente = 0;
		idProveedor = 0;
		idUsuario = 0;
		idEncabezadoFacturaProveedor = 0;
		idEncabezadoFactura = 0;
		notas = "";
		tipoCambio = 0;
		idNotaCreditoEncabezadoFactura = 0;
		baja = false;
	}
	
	public CMovimientosSaldos(int pIdMovimientosSaldos)
	{
		idMovimientosSaldos = pIdMovimientosSaldos;
		fecha = new DateTime(1, 1, 1);
		tipo = "";
		idIngresosEncabezadoFactura = 0;
		idEgresosEncabezadoFacturaProveedor = 0;
		idIngresosNoDepositadosEncabezadoFactura = 0;
		idChequesEncabezadoFacturaProveedor = 0;
		monto = 0;
		idTipoMoneda = 0;
		idCliente = 0;
		idProveedor = 0;
		idUsuario = 0;
		idEncabezadoFacturaProveedor = 0;
		idEncabezadoFactura = 0;
		notas = "";
		tipoCambio = 0;
		idNotaCreditoEncabezadoFactura = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosSaldos_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMovimientosSaldos>(typeof(CMovimientosSaldos), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosSaldos_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CMovimientosSaldos>(typeof(CMovimientosSaldos), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosSaldos_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosSaldos", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMovimientosSaldos>(typeof(CMovimientosSaldos), pConexion);
		foreach (CMovimientosSaldos O in Obten.ListaRegistros)
		{
			idMovimientosSaldos = O.IdMovimientosSaldos;
			fecha = O.Fecha;
			tipo = O.Tipo;
			idIngresosEncabezadoFactura = O.IdIngresosEncabezadoFactura;
			idEgresosEncabezadoFacturaProveedor = O.IdEgresosEncabezadoFacturaProveedor;
			idIngresosNoDepositadosEncabezadoFactura = O.IdIngresosNoDepositadosEncabezadoFactura;
			idChequesEncabezadoFacturaProveedor = O.IdChequesEncabezadoFacturaProveedor;
			monto = O.Monto;
			idTipoMoneda = O.IdTipoMoneda;
			idCliente = O.IdCliente;
			idProveedor = O.IdProveedor;
			idUsuario = O.IdUsuario;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			idEncabezadoFactura = O.IdEncabezadoFactura;
			notas = O.Notas;
			tipoCambio = O.TipoCambio;
			idNotaCreditoEncabezadoFactura = O.IdNotaCreditoEncabezadoFactura;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosSaldos_ConsultarFiltros";
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
		Obten.Llena<CMovimientosSaldos>(typeof(CMovimientosSaldos), pConexion);
		foreach (CMovimientosSaldos O in Obten.ListaRegistros)
		{
			idMovimientosSaldos = O.IdMovimientosSaldos;
			fecha = O.Fecha;
			tipo = O.Tipo;
			idIngresosEncabezadoFactura = O.IdIngresosEncabezadoFactura;
			idEgresosEncabezadoFacturaProveedor = O.IdEgresosEncabezadoFacturaProveedor;
			idIngresosNoDepositadosEncabezadoFactura = O.IdIngresosNoDepositadosEncabezadoFactura;
			idChequesEncabezadoFacturaProveedor = O.IdChequesEncabezadoFacturaProveedor;
			monto = O.Monto;
			idTipoMoneda = O.IdTipoMoneda;
			idCliente = O.IdCliente;
			idProveedor = O.IdProveedor;
			idUsuario = O.IdUsuario;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
			idEncabezadoFactura = O.IdEncabezadoFactura;
			notas = O.Notas;
			tipoCambio = O.TipoCambio;
			idNotaCreditoEncabezadoFactura = O.IdNotaCreditoEncabezadoFactura;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MovimientosSaldos_ConsultarFiltros";
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
		Obten.Llena<CMovimientosSaldos>(typeof(CMovimientosSaldos), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_MovimientosSaldos_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosSaldos", 0);
		Agregar.StoredProcedure.Parameters["@pIdMovimientosSaldos"].Direction = ParameterDirection.Output;
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipo", tipo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosEncabezadoFactura", idIngresosEncabezadoFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEgresosEncabezadoFacturaProveedor", idEgresosEncabezadoFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositadosEncabezadoFactura", idIngresosNoDepositadosEncabezadoFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdChequesEncabezadoFacturaProveedor", idChequesEncabezadoFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoEncabezadoFactura", idNotaCreditoEncabezadoFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idMovimientosSaldos= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdMovimientosSaldos"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_MovimientosSaldos_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosSaldos", idMovimientosSaldos);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipo", tipo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosEncabezadoFactura", idIngresosEncabezadoFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEgresosEncabezadoFacturaProveedor", idEgresosEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositadosEncabezadoFactura", idIngresosNoDepositadosEncabezadoFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdChequesEncabezadoFacturaProveedor", idChequesEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoEncabezadoFactura", idNotaCreditoEncabezadoFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_MovimientosSaldos_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdMovimientosSaldos", idMovimientosSaldos);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}