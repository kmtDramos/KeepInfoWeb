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

public partial class CSubCuentaContable
{
	//Propiedades Privadas
	private int idSubCuentaContable;
	private string subCuentaContable;
	private string descripcion;
	private int idCuentaContable;
	private int idSubCuentaContablePadre;
	private string cuentaContable;
	private bool baja;
	
	//Propiedades
	public int IdSubCuentaContable
	{
		get { return idSubCuentaContable; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSubCuentaContable = value;
		}
	}
	
	public string SubCuentaContable
	{
		get { return subCuentaContable; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			subCuentaContable = value;
		}
	}
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			descripcion = value;
		}
	}
	
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
	
	public int IdSubCuentaContablePadre
	{
		get { return idSubCuentaContablePadre; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSubCuentaContablePadre = value;
		}
	}
	
	public string CuentaContable
	{
		get { return cuentaContable; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			cuentaContable = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSubCuentaContable()
	{
		idSubCuentaContable = 0;
		subCuentaContable = "";
		descripcion = "";
		idCuentaContable = 0;
		idSubCuentaContablePadre = 0;
		cuentaContable = "";
		baja = false;
	}
	
	public CSubCuentaContable(int pIdSubCuentaContable)
	{
		idSubCuentaContable = pIdSubCuentaContable;
		subCuentaContable = "";
		descripcion = "";
		idCuentaContable = 0;
		idSubCuentaContablePadre = 0;
		cuentaContable = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCuentaContable_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSubCuentaContable>(typeof(CSubCuentaContable), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCuentaContable_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSubCuentaContable>(typeof(CSubCuentaContable), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCuentaContable_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSubCuentaContable>(typeof(CSubCuentaContable), pConexion);
		foreach (CSubCuentaContable O in Obten.ListaRegistros)
		{
			idSubCuentaContable = O.IdSubCuentaContable;
			subCuentaContable = O.SubCuentaContable;
			descripcion = O.Descripcion;
			idCuentaContable = O.IdCuentaContable;
			idSubCuentaContablePadre = O.IdSubCuentaContablePadre;
			cuentaContable = O.CuentaContable;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCuentaContable_ConsultarFiltros";
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
		Obten.Llena<CSubCuentaContable>(typeof(CSubCuentaContable), pConexion);
		foreach (CSubCuentaContable O in Obten.ListaRegistros)
		{
			idSubCuentaContable = O.IdSubCuentaContable;
			subCuentaContable = O.SubCuentaContable;
			descripcion = O.Descripcion;
			idCuentaContable = O.IdCuentaContable;
			idSubCuentaContablePadre = O.IdSubCuentaContablePadre;
			cuentaContable = O.CuentaContable;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCuentaContable_ConsultarFiltros";
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
		Obten.Llena<CSubCuentaContable>(typeof(CSubCuentaContable), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SubCuentaContable_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", 0);
		Agregar.StoredProcedure.Parameters["@pIdSubCuentaContable"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSubCuentaContable", subCuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContablePadre", idSubCuentaContablePadre);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSubCuentaContable= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSubCuentaContable"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SubCuentaContable_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", idSubCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSubCuentaContable", subCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContablePadre", idSubCuentaContablePadre);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SubCuentaContable_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", idSubCuentaContable);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}