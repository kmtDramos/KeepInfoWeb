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

public partial class CTipoCambioFacturaProveedor
{
	//Propiedades Privadas
	private int idTipoCambioFacturaProveedor;
	private int idTipoMonedaOrigen;
	private int idTipoMonedaDestino;
	private decimal tipoCambio;
	private DateTime fecha;
	private int idEncabezadoFacturaProveedor;
	
	//Propiedades
	public int IdTipoCambioFacturaProveedor
	{
		get { return idTipoCambioFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoCambioFacturaProveedor = value;
		}
	}
	
	public int IdTipoMonedaOrigen
	{
		get { return idTipoMonedaOrigen; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMonedaOrigen = value;
		}
	}
	
	public int IdTipoMonedaDestino
	{
		get { return idTipoMonedaDestino; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMonedaDestino = value;
		}
	}
	
	public decimal TipoCambio
	{
		get { return tipoCambio; }
		set
		{
			if (value < 0)
			{
				return;
			}
			tipoCambio = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int IdEncabezadoFacturaProveedor
	{
		get { return idEncabezadoFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEncabezadoFacturaProveedor = value;
		}
	}
	
	//Constructores
	public CTipoCambioFacturaProveedor()
	{
		idTipoCambioFacturaProveedor = 0;
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		tipoCambio = 0;
		fecha = new DateTime(1, 1, 1);
		idEncabezadoFacturaProveedor = 0;
	}
	
	public CTipoCambioFacturaProveedor(int pIdTipoCambioFacturaProveedor)
	{
		idTipoCambioFacturaProveedor = pIdTipoCambioFacturaProveedor;
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		tipoCambio = 0;
		fecha = new DateTime(1, 1, 1);
		idEncabezadoFacturaProveedor = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CTipoCambioFacturaProveedor>(typeof(CTipoCambioFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTipoCambioFacturaProveedor>(typeof(CTipoCambioFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioFacturaProveedor", pIdentificador);
		Obten.Llena<CTipoCambioFacturaProveedor>(typeof(CTipoCambioFacturaProveedor), pConexion);
		foreach (CTipoCambioFacturaProveedor O in Obten.ListaRegistros)
		{
			idTipoCambioFacturaProveedor = O.IdTipoCambioFacturaProveedor;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			tipoCambio = O.TipoCambio;
			fecha = O.Fecha;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioFacturaProveedor>(typeof(CTipoCambioFacturaProveedor), pConexion);
		foreach (CTipoCambioFacturaProveedor O in Obten.ListaRegistros)
		{
			idTipoCambioFacturaProveedor = O.IdTipoCambioFacturaProveedor;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			tipoCambio = O.TipoCambio;
			fecha = O.Fecha;
			idEncabezadoFacturaProveedor = O.IdEncabezadoFacturaProveedor;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioFacturaProveedor>(typeof(CTipoCambioFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioFacturaProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdTipoCambioFacturaProveedor"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Agregar.Insert(pConexion);
		idTipoCambioFacturaProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoCambioFacturaProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioFacturaProveedor", idTipoCambioFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TipoCambioFacturaProveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioFacturaProveedor", idTipoCambioFacturaProveedor);
		Eliminar.Delete(pConexion);
	}
}