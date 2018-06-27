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

public partial class CPresupuesto
{
	//Propiedades Privadas
	private int idPresupuesto;
	private int idCliente;
	private int idUsuarioAgente;
	private int idOportunidad;
	private int idSucursal;
	private int folio;
	private string nota;
	private string montoLetra;
	private decimal subtotal;
	private decimal descuento;
	private decimal iVA;
	private decimal costo;
	private decimal utilidad;
	private decimal facturado;
	private DateTime fechaCreacion;
	private DateTime fechaUltimaModificacion;
	private DateTime fechaExpiracion;
	private DateTime fechaCancelacion;
	private DateTime fechaPedido;
	private int idUsuarioCancelacion;
	private string motivoCancelacion;
	private int idUsuarioPedido;
	private int idTipoMoneda;
	private decimal total;
	private int idEstatusPresupuesto;
	private decimal tipoCambio;
	private decimal manoObra;
	private int idContactoOrganizacion;
	private int idDireccionOrganizacion;
	private decimal comision;
	private DateTime fechaEntrega;
	private bool baja;
	
	//Propiedades
	public int IdPresupuesto
	{
		get { return idPresupuesto; }
		set
		{
			idPresupuesto = value;
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
	
	public int IdUsuarioAgente
	{
		get { return idUsuarioAgente; }
		set
		{
			idUsuarioAgente = value;
		}
	}
	
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
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
	
	public int Folio
	{
		get { return folio; }
		set
		{
			folio = value;
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
	
	public string MontoLetra
	{
		get { return montoLetra; }
		set
		{
			montoLetra = value;
		}
	}
	
	public decimal Subtotal
	{
		get { return subtotal; }
		set
		{
			subtotal = value;
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
	
	public decimal IVA
	{
		get { return iVA; }
		set
		{
			iVA = value;
		}
	}
	
	public decimal Costo
	{
		get { return costo; }
		set
		{
			costo = value;
		}
	}
	
	public decimal Utilidad
	{
		get { return utilidad; }
		set
		{
			utilidad = value;
		}
	}
	
	public decimal Facturado
	{
		get { return facturado; }
		set
		{
			facturado = value;
		}
	}
	
	public DateTime FechaCreacion
	{
		get { return fechaCreacion; }
		set { fechaCreacion = value; }
	}
	
	public DateTime FechaUltimaModificacion
	{
		get { return fechaUltimaModificacion; }
		set { fechaUltimaModificacion = value; }
	}
	
	public DateTime FechaExpiracion
	{
		get { return fechaExpiracion; }
		set { fechaExpiracion = value; }
	}
	
	public DateTime FechaCancelacion
	{
		get { return fechaCancelacion; }
		set { fechaCancelacion = value; }
	}
	
	public DateTime FechaPedido
	{
		get { return fechaPedido; }
		set { fechaPedido = value; }
	}
	
	public int IdUsuarioCancelacion
	{
		get { return idUsuarioCancelacion; }
		set
		{
			idUsuarioCancelacion = value;
		}
	}
	
	public string MotivoCancelacion
	{
		get { return motivoCancelacion; }
		set
		{
			motivoCancelacion = value;
		}
	}
	
	public int IdUsuarioPedido
	{
		get { return idUsuarioPedido; }
		set
		{
			idUsuarioPedido = value;
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
	
	public decimal Total
	{
		get { return total; }
		set
		{
			total = value;
		}
	}
	
	public int IdEstatusPresupuesto
	{
		get { return idEstatusPresupuesto; }
		set
		{
			idEstatusPresupuesto = value;
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
	
	public decimal ManoObra
	{
		get { return manoObra; }
		set
		{
			manoObra = value;
		}
	}
	
	public int IdContactoOrganizacion
	{
		get { return idContactoOrganizacion; }
		set
		{
			idContactoOrganizacion = value;
		}
	}
	
	public int IdDireccionOrganizacion
	{
		get { return idDireccionOrganizacion; }
		set
		{
			idDireccionOrganizacion = value;
		}
	}
	
	public decimal Comision
	{
		get { return comision; }
		set
		{
			comision = value;
		}
	}
	
	public DateTime FechaEntrega
	{
		get { return fechaEntrega; }
		set { fechaEntrega = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CPresupuesto()
	{
		idPresupuesto = 0;
		idCliente = 0;
		idUsuarioAgente = 0;
		idOportunidad = 0;
		idSucursal = 0;
		folio = 0;
		nota = "";
		montoLetra = "";
		subtotal = 0;
		descuento = 0;
		iVA = 0;
		costo = 0;
		utilidad = 0;
		facturado = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		fechaExpiracion = new DateTime(1, 1, 1);
		fechaCancelacion = new DateTime(1, 1, 1);
		fechaPedido = new DateTime(1, 1, 1);
		idUsuarioCancelacion = 0;
		motivoCancelacion = "";
		idUsuarioPedido = 0;
		idTipoMoneda = 0;
		total = 0;
		idEstatusPresupuesto = 0;
		tipoCambio = 0;
		manoObra = 0;
		idContactoOrganizacion = 0;
		idDireccionOrganizacion = 0;
		comision = 0;
		fechaEntrega = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CPresupuesto(int pIdPresupuesto)
	{
		idPresupuesto = pIdPresupuesto;
		idCliente = 0;
		idUsuarioAgente = 0;
		idOportunidad = 0;
		idSucursal = 0;
		folio = 0;
		nota = "";
		montoLetra = "";
		subtotal = 0;
		descuento = 0;
		iVA = 0;
		costo = 0;
		utilidad = 0;
		facturado = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		fechaExpiracion = new DateTime(1, 1, 1);
		fechaCancelacion = new DateTime(1, 1, 1);
		fechaPedido = new DateTime(1, 1, 1);
		idUsuarioCancelacion = 0;
		motivoCancelacion = "";
		idUsuarioPedido = 0;
		idTipoMoneda = 0;
		total = 0;
		idEstatusPresupuesto = 0;
		tipoCambio = 0;
		manoObra = 0;
		idContactoOrganizacion = 0;
		idDireccionOrganizacion = 0;
		comision = 0;
		fechaEntrega = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Presupuesto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPresupuesto>(typeof(CPresupuesto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Presupuesto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPresupuesto>(typeof(CPresupuesto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Presupuesto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPresupuesto>(typeof(CPresupuesto), pConexion);
		foreach (CPresupuesto O in Obten.ListaRegistros)
		{
			idPresupuesto = O.IdPresupuesto;
			idCliente = O.IdCliente;
			idUsuarioAgente = O.IdUsuarioAgente;
			idOportunidad = O.IdOportunidad;
			idSucursal = O.IdSucursal;
			folio = O.Folio;
			nota = O.Nota;
			montoLetra = O.MontoLetra;
			subtotal = O.Subtotal;
			descuento = O.Descuento;
			iVA = O.IVA;
			costo = O.Costo;
			utilidad = O.Utilidad;
			facturado = O.Facturado;
			fechaCreacion = O.FechaCreacion;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			fechaExpiracion = O.FechaExpiracion;
			fechaCancelacion = O.FechaCancelacion;
			fechaPedido = O.FechaPedido;
			idUsuarioCancelacion = O.IdUsuarioCancelacion;
			motivoCancelacion = O.MotivoCancelacion;
			idUsuarioPedido = O.IdUsuarioPedido;
			idTipoMoneda = O.IdTipoMoneda;
			total = O.Total;
			idEstatusPresupuesto = O.IdEstatusPresupuesto;
			tipoCambio = O.TipoCambio;
			manoObra = O.ManoObra;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			idDireccionOrganizacion = O.IdDireccionOrganizacion;
			comision = O.Comision;
			fechaEntrega = O.FechaEntrega;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Presupuesto_ConsultarFiltros";
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
		Obten.Llena<CPresupuesto>(typeof(CPresupuesto), pConexion);
		foreach (CPresupuesto O in Obten.ListaRegistros)
		{
			idPresupuesto = O.IdPresupuesto;
			idCliente = O.IdCliente;
			idUsuarioAgente = O.IdUsuarioAgente;
			idOportunidad = O.IdOportunidad;
			idSucursal = O.IdSucursal;
			folio = O.Folio;
			nota = O.Nota;
			montoLetra = O.MontoLetra;
			subtotal = O.Subtotal;
			descuento = O.Descuento;
			iVA = O.IVA;
			costo = O.Costo;
			utilidad = O.Utilidad;
			facturado = O.Facturado;
			fechaCreacion = O.FechaCreacion;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			fechaExpiracion = O.FechaExpiracion;
			fechaCancelacion = O.FechaCancelacion;
			fechaPedido = O.FechaPedido;
			idUsuarioCancelacion = O.IdUsuarioCancelacion;
			motivoCancelacion = O.MotivoCancelacion;
			idUsuarioPedido = O.IdUsuarioPedido;
			idTipoMoneda = O.IdTipoMoneda;
			total = O.Total;
			idEstatusPresupuesto = O.IdEstatusPresupuesto;
			tipoCambio = O.TipoCambio;
			manoObra = O.ManoObra;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			idDireccionOrganizacion = O.IdDireccionOrganizacion;
			comision = O.Comision;
			fechaEntrega = O.FechaEntrega;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Presupuesto_ConsultarFiltros";
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
		Obten.Llena<CPresupuesto>(typeof(CPresupuesto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Presupuesto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", 0);
		Agregar.StoredProcedure.Parameters["@pIdPresupuesto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMontoLetra", montoLetra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		if(fechaCreacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		if(fechaExpiracion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaExpiracion", fechaExpiracion);
		}
		if(fechaCancelacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		}
		if(fechaPedido.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPedido", fechaPedido);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioPedido", idUsuarioPedido);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusPresupuesto", idEstatusPresupuesto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pManoObra", manoObra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDireccionOrganizacion", idDireccionOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComision", comision);
		if(fechaEntrega.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrega", fechaEntrega);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idPresupuesto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdPresupuesto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Presupuesto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMontoLetra", montoLetra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		if(fechaCreacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		if(fechaExpiracion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaExpiracion", fechaExpiracion);
		}
		if(fechaCancelacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
		}
		if(fechaPedido.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPedido", fechaPedido);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioPedido", idUsuarioPedido);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusPresupuesto", idEstatusPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pManoObra", manoObra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDireccionOrganizacion", idDireccionOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComision", comision);
		if(fechaEntrega.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrega", fechaEntrega);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Presupuesto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
