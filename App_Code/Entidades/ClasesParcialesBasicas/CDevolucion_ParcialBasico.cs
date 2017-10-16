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

public partial class CDevolucion
{
	//Propiedades Privadas
	private int idDevolucion;
	private int idNotaCredito;
	private int idFacturaDetalle;
	private int idUsuarioEntrada;
	private DateTime fechaEntrada;
	private bool baja;
	
	//Propiedades
	public int IdDevolucion
	{
		get { return idDevolucion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDevolucion = value;
		}
	}
	
	public int IdNotaCredito
	{
		get { return idNotaCredito; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNotaCredito = value;
		}
	}
	
	public int IdFacturaDetalle
	{
		get { return idFacturaDetalle; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idFacturaDetalle = value;
		}
	}
	
	public int IdUsuarioEntrada
	{
		get { return idUsuarioEntrada; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioEntrada = value;
		}
	}
	
	public DateTime FechaEntrada
	{
		get { return fechaEntrada; }
		set { fechaEntrada = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CDevolucion()
	{
		idDevolucion = 0;
		idNotaCredito = 0;
		idFacturaDetalle = 0;
		idUsuarioEntrada = 0;
		fechaEntrada = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CDevolucion(int pIdDevolucion)
	{
		idDevolucion = pIdDevolucion;
		idNotaCredito = 0;
		idFacturaDetalle = 0;
		idUsuarioEntrada = 0;
		fechaEntrada = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Devolucion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDevolucion>(typeof(CDevolucion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Devolucion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDevolucion>(typeof(CDevolucion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Devolucion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDevolucion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDevolucion>(typeof(CDevolucion), pConexion);
		foreach (CDevolucion O in Obten.ListaRegistros)
		{
			idDevolucion = O.IdDevolucion;
			idNotaCredito = O.IdNotaCredito;
			idFacturaDetalle = O.IdFacturaDetalle;
			idUsuarioEntrada = O.IdUsuarioEntrada;
			fechaEntrada = O.FechaEntrada;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Devolucion_ConsultarFiltros";
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
		Obten.Llena<CDevolucion>(typeof(CDevolucion), pConexion);
		foreach (CDevolucion O in Obten.ListaRegistros)
		{
			idDevolucion = O.IdDevolucion;
			idNotaCredito = O.IdNotaCredito;
			idFacturaDetalle = O.IdFacturaDetalle;
			idUsuarioEntrada = O.IdUsuarioEntrada;
			fechaEntrada = O.FechaEntrada;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Devolucion_ConsultarFiltros";
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
		Obten.Llena<CDevolucion>(typeof(CDevolucion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Devolucion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucion", 0);
		Agregar.StoredProcedure.Parameters["@pIdDevolucion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", idFacturaDetalle);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", idUsuarioEntrada);
		if(fechaEntrada.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrada", fechaEntrada);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDevolucion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDevolucion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Devolucion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucion", idDevolucion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", idFacturaDetalle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", idUsuarioEntrada);
		if(fechaEntrada.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrada", fechaEntrada);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Devolucion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucion", idDevolucion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}        
}