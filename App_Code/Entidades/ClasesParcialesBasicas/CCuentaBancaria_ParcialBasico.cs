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

public partial class CCuentaBancaria
{
	//Propiedades Privadas
	private int idCuentaBancaria;
	private string cuentaBancaria;
	private string descripcion;
	private int idBanco;
	private int idTipoMoneda;
	private decimal saldo;
	private string cuentaContable;
	private string cuentaContableComplemento;
	private bool baja;
	
	//Propiedades
	public int IdCuentaBancaria
	{
		get { return idCuentaBancaria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCuentaBancaria = value;
		}
	}
	
	public string CuentaBancaria
	{
		get { return cuentaBancaria; }
		set
		{
			cuentaBancaria = value;
		}
	}
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			descripcion = value;
		}
	}
	
	public int IdBanco
	{
		get { return idBanco; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idBanco = value;
		}
	}
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMoneda = value;
		}
	}
	
	public decimal Saldo
	{
		get { return saldo; }
		set
		{
			if (value < 0)
			{
				return;
			}
			saldo = value;
		}
	}
	
	public string CuentaContable
	{
		get { return cuentaContable; }
		set
		{
			cuentaContable = value;
		}
	}
	
	public string CuentaContableComplemento
	{
		get { return cuentaContableComplemento; }
		set
		{
			cuentaContableComplemento = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CCuentaBancaria()
	{
		idCuentaBancaria = 0;
		cuentaBancaria = "";
		descripcion = "";
		idBanco = 0;
		idTipoMoneda = 0;
		saldo = 0;
		cuentaContable = "";
		cuentaContableComplemento = "";
		baja = false;
	}
	
	public CCuentaBancaria(int pIdCuentaBancaria)
	{
		idCuentaBancaria = pIdCuentaBancaria;
		cuentaBancaria = "";
		descripcion = "";
		idBanco = 0;
		idTipoMoneda = 0;
		saldo = 0;
		cuentaContable = "";
		cuentaContableComplemento = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentaBancaria>(typeof(CCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CCuentaBancaria>(typeof(CCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaBancaria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentaBancaria>(typeof(CCuentaBancaria), pConexion);
		foreach (CCuentaBancaria O in Obten.ListaRegistros)
		{
			idCuentaBancaria = O.IdCuentaBancaria;
			cuentaBancaria = O.CuentaBancaria;
			descripcion = O.Descripcion;
			idBanco = O.IdBanco;
			idTipoMoneda = O.IdTipoMoneda;
			saldo = O.Saldo;
			cuentaContable = O.CuentaContable;
			cuentaContableComplemento = O.CuentaContableComplemento;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaBancaria_ConsultarFiltros";
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
		Obten.Llena<CCuentaBancaria>(typeof(CCuentaBancaria), pConexion);
		foreach (CCuentaBancaria O in Obten.ListaRegistros)
		{
			idCuentaBancaria = O.IdCuentaBancaria;
			cuentaBancaria = O.CuentaBancaria;
			descripcion = O.Descripcion;
			idBanco = O.IdBanco;
			idTipoMoneda = O.IdTipoMoneda;
			saldo = O.Saldo;
			cuentaContable = O.CuentaContable;
			cuentaContableComplemento = O.CuentaContableComplemento;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaBancaria_ConsultarFiltros";
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
		Obten.Llena<CCuentaBancaria>(typeof(CCuentaBancaria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_CuentaBancaria_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", 0);
		Agregar.StoredProcedure.Parameters["@pIdCuentaBancaria"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", cuentaBancaria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdBanco", idBanco);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableComplemento", cuentaContableComplemento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idCuentaBancaria= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCuentaBancaria"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_CuentaBancaria_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", cuentaBancaria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdBanco", idBanco);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableComplemento", cuentaContableComplemento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_CuentaBancaria_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}