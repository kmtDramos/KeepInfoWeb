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

public partial class CSolicitudPago
{
	//Propiedades Privadas
	private int idSolicitudPago;
	private int idUsuario;
	private int idProveedor;
	private decimal monto;
	private DateTime fechaCreacion;
	private DateTime fechaRequerida;
	private int idOportunidad;
	private bool pagada;
	private DateTime fechaPago;
	private bool baja;
	
	//Propiedades
	public int IdSolicitudPago
	{
		get { return idSolicitudPago; }
		set
		{
			idSolicitudPago = value;
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
	
	public int IdProveedor
	{
		get { return idProveedor; }
		set
		{
			idProveedor = value;
		}
	}
	
	public decimal Monto
	{
		get { return monto; }
		set
		{
			monto = value;
		}
	}
	
	public DateTime FechaCreacion
	{
		get { return fechaCreacion; }
		set { fechaCreacion = value; }
	}
	
	public DateTime FechaRequerida
	{
		get { return fechaRequerida; }
		set { fechaRequerida = value; }
	}
	
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
		}
	}
	
	public bool Pagada
	{
		get { return pagada; }
		set { pagada = value; }
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
	public CSolicitudPago()
	{
		idSolicitudPago = 0;
		idUsuario = 0;
		idProveedor = 0;
		monto = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		fechaRequerida = new DateTime(1, 1, 1);
		idOportunidad = 0;
		pagada = false;
		fechaPago = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CSolicitudPago(int pIdSolicitudPago)
	{
		idSolicitudPago = pIdSolicitudPago;
		idUsuario = 0;
		idProveedor = 0;
		monto = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		fechaRequerida = new DateTime(1, 1, 1);
		idOportunidad = 0;
		pagada = false;
		fechaPago = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudPago>(typeof(CSolicitudPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSolicitudPago>(typeof(CSolicitudPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudPago", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudPago>(typeof(CSolicitudPago), pConexion);
		foreach (CSolicitudPago O in Obten.ListaRegistros)
		{
			idSolicitudPago = O.IdSolicitudPago;
			idUsuario = O.IdUsuario;
			idProveedor = O.IdProveedor;
			monto = O.Monto;
			fechaCreacion = O.FechaCreacion;
			fechaRequerida = O.FechaRequerida;
			idOportunidad = O.IdOportunidad;
			pagada = O.Pagada;
			fechaPago = O.FechaPago;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudPago_ConsultarFiltros";
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
		Obten.Llena<CSolicitudPago>(typeof(CSolicitudPago), pConexion);
		foreach (CSolicitudPago O in Obten.ListaRegistros)
		{
			idSolicitudPago = O.IdSolicitudPago;
			idUsuario = O.IdUsuario;
			idProveedor = O.IdProveedor;
			monto = O.Monto;
			fechaCreacion = O.FechaCreacion;
			fechaRequerida = O.FechaRequerida;
			idOportunidad = O.IdOportunidad;
			pagada = O.Pagada;
			fechaPago = O.FechaPago;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudPago_ConsultarFiltros";
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
		Obten.Llena<CSolicitudPago>(typeof(CSolicitudPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SolicitudPago_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudPago", 0);
		Agregar.StoredProcedure.Parameters["@pIdSolicitudPago"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		if(fechaCreacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		if(fechaRequerida.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRequerida", fechaRequerida);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPagada", pagada);
		if(fechaPago.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSolicitudPago= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSolicitudPago"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SolicitudPago_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudPago", idSolicitudPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
		if(fechaCreacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		if(fechaRequerida.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRequerida", fechaRequerida);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPagada", pagada);
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
		Eliminar.StoredProcedure.CommandText = "spb_SolicitudPago_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudPago", idSolicitudPago);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
