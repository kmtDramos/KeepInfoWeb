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

public partial class CTipoCambioNotaCreditoProveedor
{
	//Propiedades Privadas
	private int idTipoCambioNotaCreditoProveedor;
	private int idTipoMonedaOrigen;
	private int idTipoMonedaOrigenDestino;
	private decimal tipoCambio;
	private DateTime fecha;
	private int idNotaCreditoProveedor;
	private int idTipoMonedaDestino;
	
	//Propiedades
	public int IdTipoCambioNotaCreditoProveedor
	{
		get { return idTipoCambioNotaCreditoProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoCambioNotaCreditoProveedor = value;
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
	
	public int IdTipoMonedaOrigenDestino
	{
		get { return idTipoMonedaOrigenDestino; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMonedaOrigenDestino = value;
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
	
	public int IdNotaCreditoProveedor
	{
		get { return idNotaCreditoProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNotaCreditoProveedor = value;
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
	
	//Constructores
	public CTipoCambioNotaCreditoProveedor()
	{
		idTipoCambioNotaCreditoProveedor = 0;
		idTipoMonedaOrigen = 0;
		idTipoMonedaOrigenDestino = 0;
		tipoCambio = 0;
		fecha = new DateTime(1, 1, 1);
		idNotaCreditoProveedor = 0;
		idTipoMonedaDestino = 0;
	}
	
	public CTipoCambioNotaCreditoProveedor(int pIdTipoCambioNotaCreditoProveedor)
	{
		idTipoCambioNotaCreditoProveedor = pIdTipoCambioNotaCreditoProveedor;
		idTipoMonedaOrigen = 0;
		idTipoMonedaOrigenDestino = 0;
		tipoCambio = 0;
		fecha = new DateTime(1, 1, 1);
		idNotaCreditoProveedor = 0;
		idTipoMonedaDestino = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CTipoCambioNotaCreditoProveedor>(typeof(CTipoCambioNotaCreditoProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTipoCambioNotaCreditoProveedor>(typeof(CTipoCambioNotaCreditoProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCreditoProveedor", pIdentificador);
		Obten.Llena<CTipoCambioNotaCreditoProveedor>(typeof(CTipoCambioNotaCreditoProveedor), pConexion);
		foreach (CTipoCambioNotaCreditoProveedor O in Obten.ListaRegistros)
		{
			idTipoCambioNotaCreditoProveedor = O.IdTipoCambioNotaCreditoProveedor;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaOrigenDestino = O.IdTipoMonedaOrigenDestino;
			tipoCambio = O.TipoCambio;
			fecha = O.Fecha;
			idNotaCreditoProveedor = O.IdNotaCreditoProveedor;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioNotaCreditoProveedor>(typeof(CTipoCambioNotaCreditoProveedor), pConexion);
		foreach (CTipoCambioNotaCreditoProveedor O in Obten.ListaRegistros)
		{
			idTipoCambioNotaCreditoProveedor = O.IdTipoCambioNotaCreditoProveedor;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaOrigenDestino = O.IdTipoMonedaOrigenDestino;
			tipoCambio = O.TipoCambio;
			fecha = O.Fecha;
			idNotaCreditoProveedor = O.IdNotaCreditoProveedor;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioNotaCreditoProveedor>(typeof(CTipoCambioNotaCreditoProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCreditoProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdTipoCambioNotaCreditoProveedor"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigenDestino", idTipoMonedaOrigenDestino);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Agregar.Insert(pConexion);
		idTipoCambioNotaCreditoProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoCambioNotaCreditoProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCreditoProveedor", idTipoCambioNotaCreditoProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigenDestino", idTipoMonedaOrigenDestino);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TipoCambioNotaCreditoProveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioNotaCreditoProveedor", idTipoCambioNotaCreditoProveedor);
		Eliminar.Delete(pConexion);
	}
}