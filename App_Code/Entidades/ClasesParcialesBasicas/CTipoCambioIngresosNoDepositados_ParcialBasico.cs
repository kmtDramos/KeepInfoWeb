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

public partial class CTipoCambioIngresosNoDepositados
{
	//Propiedades Privadas
	private int idTipoCambioIngresosNoDepositados;
	private int idTipoMonedaOrigen;
	private int idTipoMonedaDestino;
	private decimal tipoCambio ;
	private DateTime fecha;
	private int idIngresosNoDepositados;
	
	//Propiedades
	public int IdTipoCambioIngresosNoDepositados
	{
		get { return idTipoCambioIngresosNoDepositados; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoCambioIngresosNoDepositados = value;
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
		get { return tipoCambio ; }
		set
		{
			if (value < 0)
			{
				return;
			}
			tipoCambio  = value;
		}
	}
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int IdIngresosNoDepositados
	{
		get { return idIngresosNoDepositados; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idIngresosNoDepositados = value;
		}
	}
	
	//Constructores
	public CTipoCambioIngresosNoDepositados()
	{
		idTipoCambioIngresosNoDepositados = 0;
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		tipoCambio  = 0;
		fecha = new DateTime(1, 1, 1);
		idIngresosNoDepositados = 0;
	}
	
	public CTipoCambioIngresosNoDepositados(int pIdTipoCambioIngresosNoDepositados)
	{
		idTipoCambioIngresosNoDepositados = pIdTipoCambioIngresosNoDepositados;
		idTipoMonedaOrigen = 0;
		idTipoMonedaDestino = 0;
		tipoCambio  = 0;
		fecha = new DateTime(1, 1, 1);
		idIngresosNoDepositados = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CTipoCambioIngresosNoDepositados>(typeof(CTipoCambioIngresosNoDepositados), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTipoCambioIngresosNoDepositados>(typeof(CTipoCambioIngresosNoDepositados), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioIngresosNoDepositados", pIdentificador);
		Obten.Llena<CTipoCambioIngresosNoDepositados>(typeof(CTipoCambioIngresosNoDepositados), pConexion);
		foreach (CTipoCambioIngresosNoDepositados O in Obten.ListaRegistros)
		{
			idTipoCambioIngresosNoDepositados = O.IdTipoCambioIngresosNoDepositados;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			tipoCambio  = O.TipoCambio ;
			fecha = O.Fecha;
			idIngresosNoDepositados = O.IdIngresosNoDepositados;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioIngresosNoDepositados>(typeof(CTipoCambioIngresosNoDepositados), pConexion);
		foreach (CTipoCambioIngresosNoDepositados O in Obten.ListaRegistros)
		{
			idTipoCambioIngresosNoDepositados = O.IdTipoCambioIngresosNoDepositados;
			idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
			idTipoMonedaDestino = O.IdTipoMonedaDestino;
			tipoCambio  = O.TipoCambio ;
			fecha = O.Fecha;
			idIngresosNoDepositados = O.IdIngresosNoDepositados;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_ConsultarFiltros";
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
		Obten.Llena<CTipoCambioIngresosNoDepositados>(typeof(CTipoCambioIngresosNoDepositados), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioIngresosNoDepositados", 0);
		Agregar.StoredProcedure.Parameters["@pIdTipoCambioIngresosNoDepositados"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio ", tipoCambio );
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", idIngresosNoDepositados);
		Agregar.Insert(pConexion);
		idTipoCambioIngresosNoDepositados= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoCambioIngresosNoDepositados"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioIngresosNoDepositados", idTipoCambioIngresosNoDepositados);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio ", tipoCambio );
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", idIngresosNoDepositados);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TipoCambioIngresosNoDepositados_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCambioIngresosNoDepositados", idTipoCambioIngresosNoDepositados);
		Eliminar.Delete(pConexion);
	}
}