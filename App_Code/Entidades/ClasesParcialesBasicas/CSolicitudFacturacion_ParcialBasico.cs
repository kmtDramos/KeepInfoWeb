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

public partial class CSolicitudFacturacion
{
	//Propiedades Privadas
	private int idSolicitudFacturacion;
	private int ordenFactura;
	private int idCliente;
	private int idProyecto;
	private int idEstatusSolicitudFacturacion;
	private string solicitud;
	private bool baja;
	
	//Propiedades
	public int IdSolicitudFacturacion
	{
		get { return idSolicitudFacturacion; }
		set
		{
			idSolicitudFacturacion = value;
		}
	}
	
	public int OrdenFactura
	{
		get { return ordenFactura; }
		set
		{
			ordenFactura = value;
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
	
	public int IdProyecto
	{
		get { return idProyecto; }
		set
		{
			idProyecto = value;
		}
	}
	
	public int IdEstatusSolicitudFacturacion
	{
		get { return idEstatusSolicitudFacturacion; }
		set
		{
			idEstatusSolicitudFacturacion = value;
		}
	}
	
	public string Solicitud
	{
		get { return solicitud; }
		set
		{
			solicitud = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSolicitudFacturacion()
	{
		idSolicitudFacturacion = 0;
		ordenFactura = 0;
		idCliente = 0;
		idProyecto = 0;
		idEstatusSolicitudFacturacion = 0;
		solicitud = "";
		baja = false;
	}
	
	public CSolicitudFacturacion(int pIdSolicitudFacturacion)
	{
		idSolicitudFacturacion = pIdSolicitudFacturacion;
		ordenFactura = 0;
		idCliente = 0;
		idProyecto = 0;
		idEstatusSolicitudFacturacion = 0;
		solicitud = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudFacturacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudFacturacion>(typeof(CSolicitudFacturacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudFacturacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSolicitudFacturacion>(typeof(CSolicitudFacturacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudFacturacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudFacturacion>(typeof(CSolicitudFacturacion), pConexion);
		foreach (CSolicitudFacturacion O in Obten.ListaRegistros)
		{
			idSolicitudFacturacion = O.IdSolicitudFacturacion;
			ordenFactura = O.OrdenFactura;
			idCliente = O.IdCliente;
			idProyecto = O.IdProyecto;
			idEstatusSolicitudFacturacion = O.IdEstatusSolicitudFacturacion;
			solicitud = O.Solicitud;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudFacturacion_ConsultarFiltros";
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
		Obten.Llena<CSolicitudFacturacion>(typeof(CSolicitudFacturacion), pConexion);
		foreach (CSolicitudFacturacion O in Obten.ListaRegistros)
		{
			idSolicitudFacturacion = O.IdSolicitudFacturacion;
			ordenFactura = O.OrdenFactura;
			idCliente = O.IdCliente;
			idProyecto = O.IdProyecto;
			idEstatusSolicitudFacturacion = O.IdEstatusSolicitudFacturacion;
			solicitud = O.Solicitud;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudFacturacion_ConsultarFiltros";
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
		Obten.Llena<CSolicitudFacturacion>(typeof(CSolicitudFacturacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SolicitudFacturacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdSolicitudFacturacion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrdenFactura", ordenFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusSolicitudFacturacion", idEstatusSolicitudFacturacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSolicitud", solicitud);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSolicitudFacturacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSolicitudFacturacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SolicitudFacturacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", idSolicitudFacturacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenFactura", ordenFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusSolicitudFacturacion", idEstatusSolicitudFacturacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSolicitud", solicitud);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SolicitudFacturacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", idSolicitudFacturacion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
