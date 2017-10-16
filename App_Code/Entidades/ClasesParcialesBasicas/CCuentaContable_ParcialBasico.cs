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

public partial class CCuentaContable
{
	//Propiedades Privadas
	private int idCuentaContable;
	private string cuentaContable;
	private string descripcion;
	private int idTipoCompra;
	private int idDivision;
	private int idSucursal;
	private int idTipoCuentaContable;
	private bool baja;
	
	//Propiedades
	public int IdCuentaContable
	{
		get { return idCuentaContable; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCuentaContable = value;
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
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			descripcion = value;
		}
	}
	
	public int IdTipoCompra
	{
		get { return idTipoCompra; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoCompra = value;
		}
	}
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDivision = value;
		}
	}
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursal = value;
		}
	}
	
	public int IdTipoCuentaContable
	{
		get { return idTipoCuentaContable; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoCuentaContable = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CCuentaContable()
	{
		idCuentaContable = 0;
		cuentaContable = "";
		descripcion = "";
		idTipoCompra = 0;
		idDivision = 0;
		idSucursal = 0;
		idTipoCuentaContable = 0;
		baja = false;
	}
	
	public CCuentaContable(int pIdCuentaContable)
	{
		idCuentaContable = pIdCuentaContable;
		cuentaContable = "";
		descripcion = "";
		idTipoCompra = 0;
		idDivision = 0;
		idSucursal = 0;
		idTipoCuentaContable = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaContable_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentaContable>(typeof(CCuentaContable), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaContable_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CCuentaContable>(typeof(CCuentaContable), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaContable_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CCuentaContable>(typeof(CCuentaContable), pConexion);
		foreach (CCuentaContable O in Obten.ListaRegistros)
		{
			idCuentaContable = O.IdCuentaContable;
			cuentaContable = O.CuentaContable;
			descripcion = O.Descripcion;
			idTipoCompra = O.IdTipoCompra;
			idDivision = O.IdDivision;
			idSucursal = O.IdSucursal;
			idTipoCuentaContable = O.IdTipoCuentaContable;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaContable_ConsultarFiltros";
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
		Obten.Llena<CCuentaContable>(typeof(CCuentaContable), pConexion);
		foreach (CCuentaContable O in Obten.ListaRegistros)
		{
			idCuentaContable = O.IdCuentaContable;
			cuentaContable = O.CuentaContable;
			descripcion = O.Descripcion;
			idTipoCompra = O.IdTipoCompra;
			idDivision = O.IdDivision;
			idSucursal = O.IdSucursal;
			idTipoCuentaContable = O.IdTipoCuentaContable;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_CuentaContable_ConsultarFiltros";
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
		Obten.Llena<CCuentaContable>(typeof(CCuentaContable), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_CuentaContable_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", 0);
		Agregar.StoredProcedure.Parameters["@pIdCuentaContable"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCuentaContable", idTipoCuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idCuentaContable= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCuentaContable"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_CuentaContable_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCuentaContable", idTipoCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_CuentaContable_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}