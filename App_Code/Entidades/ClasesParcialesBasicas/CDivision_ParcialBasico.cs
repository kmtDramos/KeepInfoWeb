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

public partial class CDivision
{
	//Propiedades Privadas
	private int idDivision;
	private string division;
	private string claveCuentaContable;
	private bool esVenta;
	private string clave;
	private string abreviatura;
	private decimal limiteMargen;
	private decimal limiteDescuento;
	private string descripcion;
	private bool baja;
	
	//Propiedades
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
		}
	}
	
	public string Division
	{
		get { return division; }
		set
		{
			division = value;
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
	
	public bool EsVenta
	{
		get { return esVenta; }
		set { esVenta = value; }
	}
	
	public string Clave
	{
		get { return clave; }
		set
		{
			clave = value;
		}
	}
	
	public string Abreviatura
	{
		get { return abreviatura; }
		set
		{
			abreviatura = value;
		}
	}
	
	public decimal LimiteMargen
	{
		get { return limiteMargen; }
		set
		{
			limiteMargen = value;
		}
	}
	
	public decimal LimiteDescuento
	{
		get { return limiteDescuento; }
		set
		{
			limiteDescuento = value;
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
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CDivision()
	{
		idDivision = 0;
		division = "";
		claveCuentaContable = "";
		esVenta = false;
		clave = "";
		abreviatura = "";
		limiteMargen = 0;
		limiteDescuento = 0;
		descripcion = "";
		baja = false;
	}
	
	public CDivision(int pIdDivision)
	{
		idDivision = pIdDivision;
		division = "";
		claveCuentaContable = "";
		esVenta = false;
		clave = "";
		abreviatura = "";
		limiteMargen = 0;
		limiteDescuento = 0;
		descripcion = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Division_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDivision>(typeof(CDivision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Division_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDivision>(typeof(CDivision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Division_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDivision", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDivision>(typeof(CDivision), pConexion);
		foreach (CDivision O in Obten.ListaRegistros)
		{
			idDivision = O.IdDivision;
			division = O.Division;
			claveCuentaContable = O.ClaveCuentaContable;
			esVenta = O.EsVenta;
			clave = O.Clave;
			abreviatura = O.Abreviatura;
			limiteMargen = O.LimiteMargen;
			limiteDescuento = O.LimiteDescuento;
			descripcion = O.Descripcion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Division_ConsultarFiltros";
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
		Obten.Llena<CDivision>(typeof(CDivision), pConexion);
		foreach (CDivision O in Obten.ListaRegistros)
		{
			idDivision = O.IdDivision;
			division = O.Division;
			claveCuentaContable = O.ClaveCuentaContable;
			esVenta = O.EsVenta;
			clave = O.Clave;
			abreviatura = O.Abreviatura;
			limiteMargen = O.LimiteMargen;
			limiteDescuento = O.LimiteDescuento;
			descripcion = O.Descripcion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Division_ConsultarFiltros";
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
		Obten.Llena<CDivision>(typeof(CDivision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Division_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", 0);
		Agregar.StoredProcedure.Parameters["@pIdDivision"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDivision", division);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsVenta", esVenta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLimiteMargen", limiteMargen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLimiteDescuento", limiteDescuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDivision= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDivision"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Division_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDivision", division);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsVenta", esVenta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLimiteMargen", limiteMargen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLimiteDescuento", limiteDescuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Division_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
