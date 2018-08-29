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

public partial class CSolicitudProyecto
{
	//Propiedades Privadas
	private int idSolicitudProyecto;
	private int idOportunidad;
	private string proyecto;
	private bool cotizacionExcel;
	private bool cotizacionFirmada;
	private bool ordenCompra;
	private string numOrdenCompra;
	private bool contrato;
	private string numContrato;
	private bool autorizacionCorreo;
	private bool pagoDeAnticipo;
	private bool requiereFactura;
	private decimal procentaje;
	private string quienAutoriza;
	private string contacto;
	private string quienCotizo;
	private int solicitudCompra;
	private int avanzarCompras;
	private string comentarios;
	private string archivo;
	private int idCliente;
	private DateTime fechaAlta;
	private int idUsuario;
	private bool baja;
	
	//Propiedades
	public int IdSolicitudProyecto
	{
		get { return idSolicitudProyecto; }
		set
		{
			idSolicitudProyecto = value;
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
	
	public string Proyecto
	{
		get { return proyecto; }
		set
		{
			proyecto = value;
		}
	}
	
	public bool CotizacionExcel
	{
		get { return cotizacionExcel; }
		set { cotizacionExcel = value; }
	}
	
	public bool CotizacionFirmada
	{
		get { return cotizacionFirmada; }
		set { cotizacionFirmada = value; }
	}
	
	public bool OrdenCompra
	{
		get { return ordenCompra; }
		set { ordenCompra = value; }
	}
	
	public string NumOrdenCompra
	{
		get { return numOrdenCompra; }
		set
		{
			numOrdenCompra = value;
		}
	}
	
	public bool Contrato
	{
		get { return contrato; }
		set { contrato = value; }
	}
	
	public string NumContrato
	{
		get { return numContrato; }
		set
		{
			numContrato = value;
		}
	}
	
	public bool AutorizacionCorreo
	{
		get { return autorizacionCorreo; }
		set { autorizacionCorreo = value; }
	}
	
	public bool PagoDeAnticipo
	{
		get { return pagoDeAnticipo; }
		set { pagoDeAnticipo = value; }
	}
	
	public bool RequiereFactura
	{
		get { return requiereFactura; }
		set { requiereFactura = value; }
	}
	
	public decimal Procentaje
	{
		get { return procentaje; }
		set
		{
			procentaje = value;
		}
	}
	
	public string QuienAutoriza
	{
		get { return quienAutoriza; }
		set
		{
			quienAutoriza = value;
		}
	}
	
	public string Contacto
	{
		get { return contacto; }
		set
		{
			contacto = value;
		}
	}
	
	public string QuienCotizo
	{
		get { return quienCotizo; }
		set
		{
			quienCotizo = value;
		}
	}
	
	public int SolicitudCompra
	{
		get { return solicitudCompra; }
		set
		{
			solicitudCompra = value;
		}
	}
	
	public int AvanzarCompras
	{
		get { return avanzarCompras; }
		set
		{
			avanzarCompras = value;
		}
	}
	
	public string Comentarios
	{
		get { return comentarios; }
		set
		{
			comentarios = value;
		}
	}
	
	public string Archivo
	{
		get { return archivo; }
		set
		{
			archivo = value;
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
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSolicitudProyecto()
	{
		idSolicitudProyecto = 0;
		idOportunidad = 0;
		proyecto = "";
		cotizacionExcel = false;
		cotizacionFirmada = false;
		ordenCompra = false;
		numOrdenCompra = "";
		contrato = false;
		numContrato = "";
		autorizacionCorreo = false;
		pagoDeAnticipo = false;
		requiereFactura = false;
		procentaje = 0;
		quienAutoriza = "";
		contacto = "";
		quienCotizo = "";
		solicitudCompra = 0;
		avanzarCompras = 0;
		comentarios = "";
		archivo = "";
		idCliente = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuario = 0;
		baja = false;
	}
	
	public CSolicitudProyecto(int pIdSolicitudProyecto)
	{
		idSolicitudProyecto = pIdSolicitudProyecto;
		idOportunidad = 0;
		proyecto = "";
		cotizacionExcel = false;
		cotizacionFirmada = false;
		ordenCompra = false;
		numOrdenCompra = "";
		contrato = false;
		numContrato = "";
		autorizacionCorreo = false;
		pagoDeAnticipo = false;
		requiereFactura = false;
		procentaje = 0;
		quienAutoriza = "";
		contacto = "";
		quienCotizo = "";
		solicitudCompra = 0;
		avanzarCompras = 0;
		comentarios = "";
		archivo = "";
		idCliente = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuario = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudProyecto>(typeof(CSolicitudProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSolicitudProyecto>(typeof(CSolicitudProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudProyecto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudProyecto>(typeof(CSolicitudProyecto), pConexion);
		foreach (CSolicitudProyecto O in Obten.ListaRegistros)
		{
			idSolicitudProyecto = O.IdSolicitudProyecto;
			idOportunidad = O.IdOportunidad;
			proyecto = O.Proyecto;
			cotizacionExcel = O.CotizacionExcel;
			cotizacionFirmada = O.CotizacionFirmada;
			ordenCompra = O.OrdenCompra;
			numOrdenCompra = O.NumOrdenCompra;
			contrato = O.Contrato;
			numContrato = O.NumContrato;
			autorizacionCorreo = O.AutorizacionCorreo;
			pagoDeAnticipo = O.PagoDeAnticipo;
			requiereFactura = O.RequiereFactura;
			procentaje = O.Procentaje;
			quienAutoriza = O.QuienAutoriza;
			contacto = O.Contacto;
			quienCotizo = O.QuienCotizo;
			solicitudCompra = O.SolicitudCompra;
			avanzarCompras = O.AvanzarCompras;
			comentarios = O.Comentarios;
			archivo = O.Archivo;
			idCliente = O.IdCliente;
			fechaAlta = O.FechaAlta;
			idUsuario = O.IdUsuario;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudProyecto_ConsultarFiltros";
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
		Obten.Llena<CSolicitudProyecto>(typeof(CSolicitudProyecto), pConexion);
		foreach (CSolicitudProyecto O in Obten.ListaRegistros)
		{
			idSolicitudProyecto = O.IdSolicitudProyecto;
			idOportunidad = O.IdOportunidad;
			proyecto = O.Proyecto;
			cotizacionExcel = O.CotizacionExcel;
			cotizacionFirmada = O.CotizacionFirmada;
			ordenCompra = O.OrdenCompra;
			numOrdenCompra = O.NumOrdenCompra;
			contrato = O.Contrato;
			numContrato = O.NumContrato;
			autorizacionCorreo = O.AutorizacionCorreo;
			pagoDeAnticipo = O.PagoDeAnticipo;
			requiereFactura = O.RequiereFactura;
			procentaje = O.Procentaje;
			quienAutoriza = O.QuienAutoriza;
			contacto = O.Contacto;
			quienCotizo = O.QuienCotizo;
			solicitudCompra = O.SolicitudCompra;
			avanzarCompras = O.AvanzarCompras;
			comentarios = O.Comentarios;
			archivo = O.Archivo;
			idCliente = O.IdCliente;
			fechaAlta = O.FechaAlta;
			idUsuario = O.IdUsuario;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudProyecto_ConsultarFiltros";
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
		Obten.Llena<CSolicitudProyecto>(typeof(CSolicitudProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SolicitudProyecto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudProyecto", 0);
		Agregar.StoredProcedure.Parameters["@pIdSolicitudProyecto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProyecto", proyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCotizacionExcel", cotizacionExcel);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCotizacionFirmada", cotizacionFirmada);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenCompra", ordenCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumOrdenCompra", numOrdenCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pContrato", contrato);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumContrato", numContrato);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAutorizacionCorreo", autorizacionCorreo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPagoDeAnticipo", pagoDeAnticipo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRequiereFactura", requiereFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProcentaje", procentaje);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pQuienAutoriza", quienAutoriza);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pContacto", contacto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pQuienCotizo", quienCotizo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSolicitudCompra", solicitudCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAvanzarCompras", avanzarCompras);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComentarios", comentarios);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pArchivo", archivo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSolicitudProyecto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSolicitudProyecto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SolicitudProyecto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudProyecto", idSolicitudProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProyecto", proyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCotizacionExcel", cotizacionExcel);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCotizacionFirmada", cotizacionFirmada);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenCompra", ordenCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumOrdenCompra", numOrdenCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pContrato", contrato);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumContrato", numContrato);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAutorizacionCorreo", autorizacionCorreo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPagoDeAnticipo", pagoDeAnticipo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRequiereFactura", requiereFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProcentaje", procentaje);
		Editar.StoredProcedure.Parameters.AddWithValue("@pQuienAutoriza", quienAutoriza);
		Editar.StoredProcedure.Parameters.AddWithValue("@pContacto", contacto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pQuienCotizo", quienCotizo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSolicitudCompra", solicitudCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAvanzarCompras", avanzarCompras);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComentarios", comentarios);
		Editar.StoredProcedure.Parameters.AddWithValue("@pArchivo", archivo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SolicitudProyecto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudProyecto", idSolicitudProyecto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
