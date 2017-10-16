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

public partial class CClienteRelacion
{
	//Propiedades Privadas
	private int idClienteRelacion;
	private int idCliente;
	private string cliente;
	private int idUsuarioAgente;
	private DateTime fechaAlta;
	private DateTime fechaModificacion;
	private int idEstatusClienteRelacion;
	private bool baja;
	
	//Propiedades
	public int IdClienteRelacion
	{
		get { return idClienteRelacion; }
		set
		{
			idClienteRelacion = value;
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
	
	public string Cliente
	{
		get { return cliente; }
		set
		{
			cliente = value;
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
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaModificacion
	{
		get { return fechaModificacion; }
		set { fechaModificacion = value; }
	}
	
	public int IdEstatusClienteRelacion
	{
		get { return idEstatusClienteRelacion; }
		set
		{
			idEstatusClienteRelacion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CClienteRelacion()
	{
		idClienteRelacion = 0;
		idCliente = 0;
		cliente = "";
		idUsuarioAgente = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idEstatusClienteRelacion = 0;
		baja = false;
	}
	
	public CClienteRelacion(int pIdClienteRelacion)
	{
		idClienteRelacion = pIdClienteRelacion;
		idCliente = 0;
		cliente = "";
		idUsuarioAgente = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idEstatusClienteRelacion = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CClienteRelacion>(typeof(CClienteRelacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CClienteRelacion>(typeof(CClienteRelacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CClienteRelacion>(typeof(CClienteRelacion), pConexion);
		foreach (CClienteRelacion O in Obten.ListaRegistros)
		{
			idClienteRelacion = O.IdClienteRelacion;
			idCliente = O.IdCliente;
			cliente = O.Cliente;
			idUsuarioAgente = O.IdUsuarioAgente;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idEstatusClienteRelacion = O.IdEstatusClienteRelacion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacion_ConsultarFiltros";
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
		Obten.Llena<CClienteRelacion>(typeof(CClienteRelacion), pConexion);
		foreach (CClienteRelacion O in Obten.ListaRegistros)
		{
			idClienteRelacion = O.IdClienteRelacion;
			idCliente = O.IdCliente;
			cliente = O.Cliente;
			idUsuarioAgente = O.IdUsuarioAgente;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idEstatusClienteRelacion = O.IdEstatusClienteRelacion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacion_ConsultarFiltros";
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
		Obten.Llena<CClienteRelacion>(typeof(CClienteRelacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ClienteRelacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdClienteRelacion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCliente", cliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusClienteRelacion", idEstatusClienteRelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idClienteRelacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdClienteRelacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ClienteRelacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", idClienteRelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCliente", cliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusClienteRelacion", idEstatusClienteRelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ClienteRelacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", idClienteRelacion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
