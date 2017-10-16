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

public partial class CCotizacion
{
	//Propiedades Privadas
	private int idCotizacion;
	private DateTime fechaAlta;
	private decimal subTotal;
	private decimal total;
	private string nota;
	private DateTime validoHasta;
	private int idCliente;
	private int idContactoOrganizacion;
	private int idTipoMoneda;
	private int idEstatusCotizacion;
	private int idUsuarioAgente;
	private int idUsuarioCotizador;
	private int folio;
	private decimal iVA;
	private int idCampana;
	private DateTime fechaPedido;
	private decimal tipoCambio;
	private decimal autorizacionIVA;
	private string cantidadTotalLetra;
	private string proyecto;
	private int idSucursalEjecutaServicio;
	private string oportunidad;
	private int idNivelInteresCotizacion;
	private int idOportunidad;
	private int idDivision;
	private string motivoDeclinar;
	private int idUsuarioDeclinar;
	private DateTime fechaDeclinar;
	private bool baja;
	
	//Propiedades
	public int IdCotizacion
	{
		get { return idCotizacion; }
		set
		{
			idCotizacion = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public decimal SubTotal
	{
		get { return subTotal; }
		set
		{
			subTotal = value;
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
	
	public string Nota
	{
		get { return nota; }
		set
		{
			nota = value;
		}
	}
	
	public DateTime ValidoHasta
	{
		get { return validoHasta; }
		set { validoHasta = value; }
	}
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			idCliente = value;
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
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			idTipoMoneda = value;
		}
	}
	
	public int IdEstatusCotizacion
	{
		get { return idEstatusCotizacion; }
		set
		{
			idEstatusCotizacion = value;
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
	
	public int IdUsuarioCotizador
	{
		get { return idUsuarioCotizador; }
		set
		{
			idUsuarioCotizador = value;
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
	
	public decimal IVA
	{
		get { return iVA; }
		set
		{
			iVA = value;
		}
	}
	
	public int IdCampana
	{
		get { return idCampana; }
		set
		{
			idCampana = value;
		}
	}
	
	public DateTime FechaPedido
	{
		get { return fechaPedido; }
		set { fechaPedido = value; }
	}
	
	public decimal TipoCambio
	{
		get { return tipoCambio; }
		set
		{
			tipoCambio = value;
		}
	}
	
	public decimal AutorizacionIVA
	{
		get { return autorizacionIVA; }
		set
		{
			autorizacionIVA = value;
		}
	}
	
	public string CantidadTotalLetra
	{
		get { return cantidadTotalLetra; }
		set
		{
			cantidadTotalLetra = value;
		}
	}
	
	public string Proyecto
	{
		get { return proyecto; }
		set
		{
			proyecto = value;
		}
	}
	
	public int IdSucursalEjecutaServicio
	{
		get { return idSucursalEjecutaServicio; }
		set
		{
			idSucursalEjecutaServicio = value;
		}
	}
	
	public string Oportunidad
	{
		get { return oportunidad; }
		set
		{
			oportunidad = value;
		}
	}
	
	public int IdNivelInteresCotizacion
	{
		get { return idNivelInteresCotizacion; }
		set
		{
			idNivelInteresCotizacion = value;
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
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
		}
	}
	
	public string MotivoDeclinar
	{
		get { return motivoDeclinar; }
		set
		{
			motivoDeclinar = value;
		}
	}
	
	public int IdUsuarioDeclinar
	{
		get { return idUsuarioDeclinar; }
		set
		{
			idUsuarioDeclinar = value;
		}
	}
	
	public DateTime FechaDeclinar
	{
		get { return fechaDeclinar; }
		set { fechaDeclinar = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CCotizacion()
	{
		idCotizacion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		subTotal = 0;
		total = 0;
		nota = "";
		validoHasta = new DateTime(1, 1, 1);
		idCliente = 0;
		idContactoOrganizacion = 0;
		idTipoMoneda = 0;
		idEstatusCotizacion = 0;
		idUsuarioAgente = 0;
		idUsuarioCotizador = 0;
		folio = 0;
		iVA = 0;
		idCampana = 0;
		fechaPedido = new DateTime(1, 1, 1);
		tipoCambio = 0;
		autorizacionIVA = 0;
		cantidadTotalLetra = "";
		proyecto = "";
		idSucursalEjecutaServicio = 0;
		oportunidad = "";
		idNivelInteresCotizacion = 0;
		idOportunidad = 0;
		idDivision = 0;
		motivoDeclinar = "";
		idUsuarioDeclinar = 0;
		fechaDeclinar = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CCotizacion(int pIdCotizacion)
	{
		idCotizacion = pIdCotizacion;
		fechaAlta = new DateTime(1, 1, 1);
		subTotal = 0;
		total = 0;
		nota = "";
		validoHasta = new DateTime(1, 1, 1);
		idCliente = 0;
		idContactoOrganizacion = 0;
		idTipoMoneda = 0;
		idEstatusCotizacion = 0;
		idUsuarioAgente = 0;
		idUsuarioCotizador = 0;
		folio = 0;
		iVA = 0;
		idCampana = 0;
		fechaPedido = new DateTime(1, 1, 1);
		tipoCambio = 0;
		autorizacionIVA = 0;
		cantidadTotalLetra = "";
		proyecto = "";
		idSucursalEjecutaServicio = 0;
		oportunidad = "";
		idNivelInteresCotizacion = 0;
		idOportunidad = 0;
		idDivision = 0;
		motivoDeclinar = "";
		idUsuarioDeclinar = 0;
		fechaDeclinar = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Cotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCotizacion>(typeof(CCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Cotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CCotizacion>(typeof(CCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Cotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCotizacion>(typeof(CCotizacion), pConexion);
		foreach (CCotizacion O in Obten.ListaRegistros)
		{
			idCotizacion = O.IdCotizacion;
			fechaAlta = O.FechaAlta;
			subTotal = O.SubTotal;
			total = O.Total;
			nota = O.Nota;
			validoHasta = O.ValidoHasta;
			idCliente = O.IdCliente;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			idTipoMoneda = O.IdTipoMoneda;
			idEstatusCotizacion = O.IdEstatusCotizacion;
			idUsuarioAgente = O.IdUsuarioAgente;
			idUsuarioCotizador = O.IdUsuarioCotizador;
			folio = O.Folio;
			iVA = O.IVA;
			idCampana = O.IdCampana;
			fechaPedido = O.FechaPedido;
			tipoCambio = O.TipoCambio;
			autorizacionIVA = O.AutorizacionIVA;
			cantidadTotalLetra = O.CantidadTotalLetra;
			proyecto = O.Proyecto;
			idSucursalEjecutaServicio = O.IdSucursalEjecutaServicio;
			oportunidad = O.Oportunidad;
			idNivelInteresCotizacion = O.IdNivelInteresCotizacion;
			idOportunidad = O.IdOportunidad;
			idDivision = O.IdDivision;
			motivoDeclinar = O.MotivoDeclinar;
			idUsuarioDeclinar = O.IdUsuarioDeclinar;
			fechaDeclinar = O.FechaDeclinar;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Cotizacion_ConsultarFiltros";
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
		Obten.Llena<CCotizacion>(typeof(CCotizacion), pConexion);
		foreach (CCotizacion O in Obten.ListaRegistros)
		{
			idCotizacion = O.IdCotizacion;
			fechaAlta = O.FechaAlta;
			subTotal = O.SubTotal;
			total = O.Total;
			nota = O.Nota;
			validoHasta = O.ValidoHasta;
			idCliente = O.IdCliente;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			idTipoMoneda = O.IdTipoMoneda;
			idEstatusCotizacion = O.IdEstatusCotizacion;
			idUsuarioAgente = O.IdUsuarioAgente;
			idUsuarioCotizador = O.IdUsuarioCotizador;
			folio = O.Folio;
			iVA = O.IVA;
			idCampana = O.IdCampana;
			fechaPedido = O.FechaPedido;
			tipoCambio = O.TipoCambio;
			autorizacionIVA = O.AutorizacionIVA;
			cantidadTotalLetra = O.CantidadTotalLetra;
			proyecto = O.Proyecto;
			idSucursalEjecutaServicio = O.IdSucursalEjecutaServicio;
			oportunidad = O.Oportunidad;
			idNivelInteresCotizacion = O.IdNivelInteresCotizacion;
			idOportunidad = O.IdOportunidad;
			idDivision = O.IdDivision;
			motivoDeclinar = O.MotivoDeclinar;
			idUsuarioDeclinar = O.IdUsuarioDeclinar;
			fechaDeclinar = O.FechaDeclinar;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Cotizacion_ConsultarFiltros";
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
		Obten.Llena<CCotizacion>(typeof(CCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Cotizacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdCotizacion"].Direction = ParameterDirection.Output;
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSubTotal", subTotal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		if(validoHasta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pValidoHasta", validoHasta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusCotizacion", idEstatusCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCotizador", idUsuarioCotizador);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
		if(fechaPedido.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPedido", fechaPedido);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAutorizacionIVA", autorizacionIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadTotalLetra", cantidadTotalLetra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProyecto", proyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalEjecutaServicio", idSucursalEjecutaServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOportunidad", oportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresCotizacion", idNivelInteresCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMotivoDeclinar", motivoDeclinar);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioDeclinar", idUsuarioDeclinar);
		if(fechaDeclinar.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaDeclinar", fechaDeclinar);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idCotizacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCotizacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Cotizacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pSubTotal", subTotal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		if(validoHasta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pValidoHasta", validoHasta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusCotizacion", idEstatusCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCotizador", idUsuarioCotizador);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
		if(fechaPedido.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPedido", fechaPedido);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAutorizacionIVA", autorizacionIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadTotalLetra", cantidadTotalLetra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProyecto", proyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalEjecutaServicio", idSucursalEjecutaServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOportunidad", oportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresCotizacion", idNivelInteresCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoDeclinar", motivoDeclinar);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioDeclinar", idUsuarioDeclinar);
		if(fechaDeclinar.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaDeclinar", fechaDeclinar);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Cotizacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
