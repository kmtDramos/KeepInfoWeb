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

public partial class CTipoMovimientos
{
	//Propiedades Privadas
	private int idTipoMovimientos;
	private string tipoMovimiento;
	private int afectacion;
	private bool baja;
	
	//Propiedades
	public int IdTipoMovimientos
	{
		get { return idTipoMovimientos; }
		set
		{
			idTipoMovimientos = value;
		}
	}
	
	public string TipoMovimiento
	{
		get { return tipoMovimiento; }
		set
		{
			tipoMovimiento = value;
		}
	}
	
	public int Afectacion
	{
		get { return afectacion; }
		set
		{
			afectacion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CTipoMovimientos()
	{
		idTipoMovimientos = 0;
		tipoMovimiento = "";
		afectacion = 0;
		baja = false;
	}
	
	public CTipoMovimientos(int pIdTipoMovimientos)
	{
		idTipoMovimientos = pIdTipoMovimientos;
		tipoMovimiento = "";
		afectacion = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoMovimientos_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTipoMovimientos>(typeof(CTipoMovimientos), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoMovimientos_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTipoMovimientos>(typeof(CTipoMovimientos), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoMovimientos_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimientos", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTipoMovimientos>(typeof(CTipoMovimientos), pConexion);
		foreach (CTipoMovimientos O in Obten.ListaRegistros)
		{
			idTipoMovimientos = O.IdTipoMovimientos;
			tipoMovimiento = O.TipoMovimiento;
			afectacion = O.Afectacion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoMovimientos_ConsultarFiltros";
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
		Obten.Llena<CTipoMovimientos>(typeof(CTipoMovimientos), pConexion);
		foreach (CTipoMovimientos O in Obten.ListaRegistros)
		{
			idTipoMovimientos = O.IdTipoMovimientos;
			tipoMovimiento = O.TipoMovimiento;
			afectacion = O.Afectacion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TipoMovimientos_ConsultarFiltros";
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
		Obten.Llena<CTipoMovimientos>(typeof(CTipoMovimientos), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TipoMovimientos_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimientos", 0);
		Agregar.StoredProcedure.Parameters["@pIdTipoMovimientos"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoMovimiento", tipoMovimiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAfectacion", afectacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idTipoMovimientos= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTipoMovimientos"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TipoMovimientos_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimientos", idTipoMovimientos);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoMovimiento", tipoMovimiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAfectacion", afectacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TipoMovimientos_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMovimientos", idTipoMovimientos);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
