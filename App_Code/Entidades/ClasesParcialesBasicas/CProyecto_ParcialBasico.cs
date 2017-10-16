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

public partial class CProyecto
{
	//Propiedades Privadas
	private int idProyecto;
	private DateTime fechaAlta;
	private string nombreProyecto;
	private DateTime fechaInicio;
	private DateTime fechaTermino;
	private decimal precioTeorico;
	private decimal costoTeorico;
	private string notas;
	private int idCliente;
	private int idEstatusProyecto;
	private int idTipoMoneda;
	private int idUsuario;
	private int idUsuarioResponsable;
	private decimal tipoCambio;
	private int idDivision;
	private int idNivelInteres;
	private int idOportunidad;
	private decimal programado;
	private decimal facturado;
	private decimal cobrado;
	private decimal porCobrar;
	private decimal costoReal;
	private decimal margen;
	private bool baja;
	
	//Propiedades
	public int IdProyecto
	{
		get { return idProyecto; }
		set
		{
			idProyecto = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public string NombreProyecto
	{
		get { return nombreProyecto; }
		set
		{
			nombreProyecto = value;
		}
	}
	
	public DateTime FechaInicio
	{
		get { return fechaInicio; }
		set { fechaInicio = value; }
	}
	
	public DateTime FechaTermino
	{
		get { return fechaTermino; }
		set { fechaTermino = value; }
	}
	
	public decimal PrecioTeorico
	{
		get { return precioTeorico; }
		set
		{
			precioTeorico = value;
		}
	}
	
	public decimal CostoTeorico
	{
		get { return costoTeorico; }
		set
		{
			costoTeorico = value;
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
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			idCliente = value;
		}
	}
	
	public int IdEstatusProyecto
	{
		get { return idEstatusProyecto; }
		set
		{
			idEstatusProyecto = value;
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
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public int IdUsuarioResponsable
	{
		get { return idUsuarioResponsable; }
		set
		{
			idUsuarioResponsable = value;
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
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
		}
	}
	
	public int IdNivelInteres
	{
		get { return idNivelInteres; }
		set
		{
			idNivelInteres = value;
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
	
	public decimal Programado
	{
		get { return programado; }
		set
		{
			programado = value;
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
	
	public decimal Cobrado
	{
		get { return cobrado; }
		set
		{
			cobrado = value;
		}
	}
	
	public decimal PorCobrar
	{
		get { return porCobrar; }
		set
		{
			porCobrar = value;
		}
	}
	
	public decimal CostoReal
	{
		get { return costoReal; }
		set
		{
			costoReal = value;
		}
	}
	
	public decimal Margen
	{
		get { return margen; }
		set
		{
			margen = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CProyecto()
	{
		idProyecto = 0;
		fechaAlta = new DateTime(1, 1, 1);
		nombreProyecto = "";
		fechaInicio = new DateTime(1, 1, 1);
		fechaTermino = new DateTime(1, 1, 1);
		precioTeorico = 0;
		costoTeorico = 0;
		notas = "";
		idCliente = 0;
		idEstatusProyecto = 0;
		idTipoMoneda = 0;
		idUsuario = 0;
		idUsuarioResponsable = 0;
		tipoCambio = 0;
		idDivision = 0;
		idNivelInteres = 0;
		idOportunidad = 0;
		programado = 0;
		facturado = 0;
		cobrado = 0;
		porCobrar = 0;
		costoReal = 0;
		margen = 0;
		baja = false;
	}
	
	public CProyecto(int pIdProyecto)
	{
		idProyecto = pIdProyecto;
		fechaAlta = new DateTime(1, 1, 1);
		nombreProyecto = "";
		fechaInicio = new DateTime(1, 1, 1);
		fechaTermino = new DateTime(1, 1, 1);
		precioTeorico = 0;
		costoTeorico = 0;
		notas = "";
		idCliente = 0;
		idEstatusProyecto = 0;
		idTipoMoneda = 0;
		idUsuario = 0;
		idUsuarioResponsable = 0;
		tipoCambio = 0;
		idDivision = 0;
		idNivelInteres = 0;
		idOportunidad = 0;
		programado = 0;
		facturado = 0;
		cobrado = 0;
		porCobrar = 0;
		costoReal = 0;
		margen = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProyecto>(typeof(CProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CProyecto>(typeof(CProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProyecto>(typeof(CProyecto), pConexion);
		foreach (CProyecto O in Obten.ListaRegistros)
		{
			idProyecto = O.IdProyecto;
			fechaAlta = O.FechaAlta;
			nombreProyecto = O.NombreProyecto;
			fechaInicio = O.FechaInicio;
			fechaTermino = O.FechaTermino;
			precioTeorico = O.PrecioTeorico;
			costoTeorico = O.CostoTeorico;
			notas = O.Notas;
			idCliente = O.IdCliente;
			idEstatusProyecto = O.IdEstatusProyecto;
			idTipoMoneda = O.IdTipoMoneda;
			idUsuario = O.IdUsuario;
			idUsuarioResponsable = O.IdUsuarioResponsable;
			tipoCambio = O.TipoCambio;
			idDivision = O.IdDivision;
			idNivelInteres = O.IdNivelInteres;
			idOportunidad = O.IdOportunidad;
			programado = O.Programado;
			facturado = O.Facturado;
			cobrado = O.Cobrado;
			porCobrar = O.PorCobrar;
			costoReal = O.CostoReal;
			margen = O.Margen;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proyecto_ConsultarFiltros";
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
		Obten.Llena<CProyecto>(typeof(CProyecto), pConexion);
		foreach (CProyecto O in Obten.ListaRegistros)
		{
			idProyecto = O.IdProyecto;
			fechaAlta = O.FechaAlta;
			nombreProyecto = O.NombreProyecto;
			fechaInicio = O.FechaInicio;
			fechaTermino = O.FechaTermino;
			precioTeorico = O.PrecioTeorico;
			costoTeorico = O.CostoTeorico;
			notas = O.Notas;
			idCliente = O.IdCliente;
			idEstatusProyecto = O.IdEstatusProyecto;
			idTipoMoneda = O.IdTipoMoneda;
			idUsuario = O.IdUsuario;
			idUsuarioResponsable = O.IdUsuarioResponsable;
			tipoCambio = O.TipoCambio;
			idDivision = O.IdDivision;
			idNivelInteres = O.IdNivelInteres;
			idOportunidad = O.IdOportunidad;
			programado = O.Programado;
			facturado = O.Facturado;
			cobrado = O.Cobrado;
			porCobrar = O.PorCobrar;
			costoReal = O.CostoReal;
			margen = O.Margen;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proyecto_ConsultarFiltros";
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
		Obten.Llena<CProyecto>(typeof(CProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Proyecto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", 0);
		Agregar.StoredProcedure.Parameters["@pIdProyecto"].Direction = ParameterDirection.Output;
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNombreProyecto", nombreProyecto);
		if(fechaInicio.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		if(fechaTermino.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaTermino", fechaTermino);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecioTeorico", precioTeorico);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCostoTeorico", costoTeorico);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProyecto", idEstatusProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioResponsable", idUsuarioResponsable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteres", idNivelInteres);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProgramado", programado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCobrado", cobrado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPorCobrar", porCobrar);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCostoReal", costoReal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargen", margen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idProyecto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProyecto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Proyecto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pNombreProyecto", nombreProyecto);
		if(fechaInicio.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		if(fechaTermino.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaTermino", fechaTermino);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrecioTeorico", precioTeorico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCostoTeorico", costoTeorico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProyecto", idEstatusProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioResponsable", idUsuarioResponsable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteres", idNivelInteres);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProgramado", programado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFacturado", facturado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCobrado", cobrado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPorCobrar", porCobrar);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCostoReal", costoReal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMargen", margen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Proyecto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
