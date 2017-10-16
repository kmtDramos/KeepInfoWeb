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

public partial class CMetodoPago
{
	//Propiedades Privadas
	private int idMetodoPago;
	private string metodoPago;
	private int idTipoMovimiento;
	private bool especificaNumeroCuenta;
	private string clave;
	private bool baja;
	
	//Propiedades
	public int IdMetodoPago
	{
		get { return idMetodoPago; }
		set
		{
			idMetodoPago = value;
		}
	}
	
	public string MetodoPago
	{
		get { return metodoPago; }
		set
		{
			metodoPago = value;
		}
	}
	
	public int IdTipoMovimiento
	{
		get { return idTipoMovimiento; }
		set
		{
			idTipoMovimiento = value;
		}
	}
	
	public bool EspecificaNumeroCuenta
	{
		get { return especificaNumeroCuenta; }
		set { especificaNumeroCuenta = value; }
	}
	
	public string Clave
	{
		get { return clave; }
		set
		{
			clave = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CMetodoPago()
	{
		idMetodoPago = 0;
		metodoPago = "";
		idTipoMovimiento = 0;
		especificaNumeroCuenta = false;
		clave = "";
		baja = false;
	}
	
	public CMetodoPago(int pIdMetodoPago)
	{
		idMetodoPago = pIdMetodoPago;
		metodoPago = "";
		idTipoMovimiento = 0;
		especificaNumeroCuenta = false;
		clave = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MetodoPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMetodoPago>(typeof(CMetodoPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MetodoPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CMetodoPago>(typeof(CMetodoPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MetodoPago_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CMetodoPago>(typeof(CMetodoPago), pConexion);
		foreach (CMetodoPago O in Obten.ListaRegistros)
		{
			idMetodoPago = O.IdMetodoPago;
			metodoPago = O.MetodoPago;
			idTipoMovimiento = O.IdTipoMovimiento;
			especificaNumeroCuenta = O.EspecificaNumeroCuenta;
			clave = O.Clave;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MetodoPago_ConsultarFiltros";
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
		Obten.Llena<CMetodoPago>(typeof(CMetodoPago), pConexion);
		foreach (CMetodoPago O in Obten.ListaRegistros)
		{
			idMetodoPago = O.IdMetodoPago;
			metodoPago = O.MetodoPago;
			idTipoMovimiento = O.IdTipoMovimiento;
			especificaNumeroCuenta = O.EspecificaNumeroCuenta;
			clave = O.Clave;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_MetodoPago_ConsultarFiltros";
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
		Obten.Llena<CMetodoPago>(typeof(CMetodoPago), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_MetodoPago_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", 0);
		Agregar.StoredProcedure.Parameters["@pIdMetodoPago"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMetodoPago", metodoPago);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimiento", idTipoMovimiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEspecificaNumeroCuenta", especificaNumeroCuenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idMetodoPago= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdMetodoPago"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_MetodoPago_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMetodoPago", metodoPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimiento", idTipoMovimiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEspecificaNumeroCuenta", especificaNumeroCuenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_MetodoPago_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
