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

public partial class CTraspaso
{
	//Propiedades Privadas
	private int idTraspaso;
	private DateTime fecha;
	private int cantidad;
	private int idUsuario;
	private int idExistenciaDistribuidaOrigen;
	private int idExistenciaDistribuidaDestino;
	
	//Propiedades
	public int IdTraspaso
	{
		get { return idTraspaso; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTraspaso = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int Cantidad
	{
		get { return cantidad; }
		set
		{
			if (value < 0)
			{
				return;
			}
			cantidad = value;
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
	
	public int IdExistenciaDistribuidaOrigen
	{
		get { return idExistenciaDistribuidaOrigen; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idExistenciaDistribuidaOrigen = value;
		}
	}
	
	public int IdExistenciaDistribuidaDestino
	{
		get { return idExistenciaDistribuidaDestino; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idExistenciaDistribuidaDestino = value;
		}
	}
	
	//Constructores
	public CTraspaso()
	{
		idTraspaso = 0;
		fecha = new DateTime(1, 1, 1);
		cantidad = 0;
		idUsuario = 0;
		idExistenciaDistribuidaOrigen = 0;
		idExistenciaDistribuidaDestino = 0;
	}
	
	public CTraspaso(int pIdTraspaso)
	{
		idTraspaso = pIdTraspaso;
		fecha = new DateTime(1, 1, 1);
		cantidad = 0;
		idUsuario = 0;
		idExistenciaDistribuidaOrigen = 0;
		idExistenciaDistribuidaDestino = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Traspaso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CTraspaso>(typeof(CTraspaso), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Traspaso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTraspaso>(typeof(CTraspaso), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Traspaso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTraspaso", pIdentificador);
		Obten.Llena<CTraspaso>(typeof(CTraspaso), pConexion);
		foreach (CTraspaso O in Obten.ListaRegistros)
		{
			idTraspaso = O.IdTraspaso;
			fecha = O.Fecha;
			cantidad = O.Cantidad;
			idUsuario = O.IdUsuario;
			idExistenciaDistribuidaOrigen = O.IdExistenciaDistribuidaOrigen;
			idExistenciaDistribuidaDestino = O.IdExistenciaDistribuidaDestino;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Traspaso_ConsultarFiltros";
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
		Obten.Llena<CTraspaso>(typeof(CTraspaso), pConexion);
		foreach (CTraspaso O in Obten.ListaRegistros)
		{
			idTraspaso = O.IdTraspaso;
			fecha = O.Fecha;
			cantidad = O.Cantidad;
			idUsuario = O.IdUsuario;
			idExistenciaDistribuidaOrigen = O.IdExistenciaDistribuidaOrigen;
			idExistenciaDistribuidaDestino = O.IdExistenciaDistribuidaDestino;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Traspaso_ConsultarFiltros";
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
		Obten.Llena<CTraspaso>(typeof(CTraspaso), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Traspaso_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTraspaso", 0);
		Agregar.StoredProcedure.Parameters["@pIdTraspaso"].Direction = ParameterDirection.Output;
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuidaOrigen", idExistenciaDistribuidaOrigen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuidaDestino", idExistenciaDistribuidaDestino);
		Agregar.Insert(pConexion);
		idTraspaso= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTraspaso"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Traspaso_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTraspaso", idTraspaso);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuidaOrigen", idExistenciaDistribuidaOrigen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaDistribuidaDestino", idExistenciaDistribuidaDestino);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Traspaso_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTraspaso", idTraspaso);
		Eliminar.Delete(pConexion);
	}
}