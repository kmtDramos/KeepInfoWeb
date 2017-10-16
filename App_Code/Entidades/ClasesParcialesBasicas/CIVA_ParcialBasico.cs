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

public partial class CIVA
{
	//Propiedades Privadas
	private int idIVA;
	private string claveCuentaContable;
	private decimal iVA;
	private string descripcionIVA;
	private string cuentaContableTrasladado;
	private string cCAcreditablePagado;
	private string cCTrasladadoPagado;
	private bool baja;
	
	//Propiedades
	public int IdIVA
	{
		get { return idIVA; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idIVA = value;
		}
	}
	
	public string ClaveCuentaContable
	{
		get { return claveCuentaContable; }
		set
		{
			claveCuentaContable = value;
		}
	}
	
	public decimal IVA
	{
		get { return iVA; }
		set
		{
			if (value < 0)
			{
				return;
			}
			iVA = value;
		}
	}
	
	public string DescripcionIVA
	{
		get { return descripcionIVA; }
		set
		{
			descripcionIVA = value;
		}
	}
	
	public string CuentaContableTrasladado
	{
		get { return cuentaContableTrasladado; }
		set
		{
			cuentaContableTrasladado = value;
		}
	}
	
	public string CCAcreditablePagado
	{
		get { return cCAcreditablePagado; }
		set
		{
			cCAcreditablePagado = value;
		}
	}
	
	public string CCTrasladadoPagado
	{
		get { return cCTrasladadoPagado; }
		set
		{
			cCTrasladadoPagado = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CIVA()
	{
		idIVA = 0;
		claveCuentaContable = "";
		iVA = 0;
		descripcionIVA = "";
		cuentaContableTrasladado = "";
		cCAcreditablePagado = "";
		cCTrasladadoPagado = "";
		baja = false;
	}
	
	public CIVA(int pIdIVA)
	{
		idIVA = pIdIVA;
		claveCuentaContable = "";
		iVA = 0;
		descripcionIVA = "";
		cuentaContableTrasladado = "";
		cCAcreditablePagado = "";
		cCTrasladadoPagado = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IVA_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CIVA>(typeof(CIVA), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IVA_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CIVA>(typeof(CIVA), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IVA_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdIVA", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CIVA>(typeof(CIVA), pConexion);
		foreach (CIVA O in Obten.ListaRegistros)
		{
			idIVA = O.IdIVA;
			claveCuentaContable = O.ClaveCuentaContable;
			iVA = O.IVA;
			descripcionIVA = O.DescripcionIVA;
			cuentaContableTrasladado = O.CuentaContableTrasladado;
			cCAcreditablePagado = O.CCAcreditablePagado;
			cCTrasladadoPagado = O.CCTrasladadoPagado;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IVA_ConsultarFiltros";
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
		Obten.Llena<CIVA>(typeof(CIVA), pConexion);
		foreach (CIVA O in Obten.ListaRegistros)
		{
			idIVA = O.IdIVA;
			claveCuentaContable = O.ClaveCuentaContable;
			iVA = O.IVA;
			descripcionIVA = O.DescripcionIVA;
			cuentaContableTrasladado = O.CuentaContableTrasladado;
			cCAcreditablePagado = O.CCAcreditablePagado;
			cCTrasladadoPagado = O.CCTrasladadoPagado;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_IVA_ConsultarFiltros";
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
		Obten.Llena<CIVA>(typeof(CIVA), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_IVA_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIVA", 0);
		Agregar.StoredProcedure.Parameters["@pIdIVA"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcionIVA", descripcionIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableTrasladado", cuentaContableTrasladado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCCAcreditablePagado", cCAcreditablePagado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCCTrasladadoPagado", cCTrasladadoPagado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idIVA= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdIVA"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_IVA_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdIVA", idIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcionIVA", descripcionIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableTrasladado", cuentaContableTrasladado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCCAcreditablePagado", cCAcreditablePagado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCCTrasladadoPagado", cCTrasladadoPagado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_IVA_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdIVA", idIVA);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}