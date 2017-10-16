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

public partial class CNecesidadVenta
{
	//Propiedades Privadas
	private int idNecesidadVenta;
	private int idClienteRelacion;
	private string necesidad;
	private DateTime fechaAlta;
	private int idUsuarioAgente;
	private DateTime fechaBaja;
	private bool baja;
	
	//Propiedades
	public int IdNecesidadVenta
	{
		get { return idNecesidadVenta; }
		set
		{
			idNecesidadVenta = value;
		}
	}
	
	public int IdClienteRelacion
	{
		get { return idClienteRelacion; }
		set
		{
			idClienteRelacion = value;
		}
	}
	
	public string Necesidad
	{
		get { return necesidad; }
		set
		{
			necesidad = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public int IdUsuarioAgente
	{
		get { return idUsuarioAgente; }
		set
		{
			idUsuarioAgente = value;
		}
	}
	
	public DateTime FechaBaja
	{
		get { return fechaBaja; }
		set { fechaBaja = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CNecesidadVenta()
	{
		idNecesidadVenta = 0;
		idClienteRelacion = 0;
		necesidad = "";
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioAgente = 0;
		fechaBaja = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CNecesidadVenta(int pIdNecesidadVenta)
	{
		idNecesidadVenta = pIdNecesidadVenta;
		idClienteRelacion = 0;
		necesidad = "";
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioAgente = 0;
		fechaBaja = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NecesidadVenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNecesidadVenta>(typeof(CNecesidadVenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NecesidadVenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CNecesidadVenta>(typeof(CNecesidadVenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NecesidadVenta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdNecesidadVenta", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNecesidadVenta>(typeof(CNecesidadVenta), pConexion);
		foreach (CNecesidadVenta O in Obten.ListaRegistros)
		{
			idNecesidadVenta = O.IdNecesidadVenta;
			idClienteRelacion = O.IdClienteRelacion;
			necesidad = O.Necesidad;
			fechaAlta = O.FechaAlta;
			idUsuarioAgente = O.IdUsuarioAgente;
			fechaBaja = O.FechaBaja;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NecesidadVenta_ConsultarFiltros";
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
		Obten.Llena<CNecesidadVenta>(typeof(CNecesidadVenta), pConexion);
		foreach (CNecesidadVenta O in Obten.ListaRegistros)
		{
			idNecesidadVenta = O.IdNecesidadVenta;
			idClienteRelacion = O.IdClienteRelacion;
			necesidad = O.Necesidad;
			fechaAlta = O.FechaAlta;
			idUsuarioAgente = O.IdUsuarioAgente;
			fechaBaja = O.FechaBaja;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NecesidadVenta_ConsultarFiltros";
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
		Obten.Llena<CNecesidadVenta>(typeof(CNecesidadVenta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_NecesidadVenta_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNecesidadVenta", 0);
		Agregar.StoredProcedure.Parameters["@pIdNecesidadVenta"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", idClienteRelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNecesidad", necesidad);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		if(fechaBaja.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaBaja", fechaBaja);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idNecesidadVenta= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNecesidadVenta"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_NecesidadVenta_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNecesidadVenta", idNecesidadVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", idClienteRelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNecesidad", necesidad);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		if(fechaBaja.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaBaja", fechaBaja);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_NecesidadVenta_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNecesidadVenta", idNecesidadVenta);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
