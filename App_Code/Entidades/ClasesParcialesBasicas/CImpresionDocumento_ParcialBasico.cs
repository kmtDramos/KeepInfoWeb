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

public partial class CImpresionDocumento
{
	//Propiedades Privadas
	private int idImpresionDocumento;
	private string impresionDocumento;
	private string procedimiento;
	private bool baja;
	
	//Propiedades
	public int IdImpresionDocumento
	{
		get { return idImpresionDocumento; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idImpresionDocumento = value;
		}
	}
	
	public string ImpresionDocumento
	{
		get { return impresionDocumento; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			impresionDocumento = value;
		}
	}
	
	public string Procedimiento
	{
		get { return procedimiento; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			procedimiento = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CImpresionDocumento()
	{
		idImpresionDocumento = 0;
		impresionDocumento = "";
		procedimiento = "";
		baja = false;
	}
	
	public CImpresionDocumento(int pIdImpresionDocumento)
	{
		idImpresionDocumento = pIdImpresionDocumento;
		impresionDocumento = "";
		procedimiento = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionDocumento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CImpresionDocumento>(typeof(CImpresionDocumento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionDocumento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CImpresionDocumento>(typeof(CImpresionDocumento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionDocumento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdImpresionDocumento", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CImpresionDocumento>(typeof(CImpresionDocumento), pConexion);
		foreach (CImpresionDocumento O in Obten.ListaRegistros)
		{
			idImpresionDocumento = O.IdImpresionDocumento;
			impresionDocumento = O.ImpresionDocumento;
			procedimiento = O.Procedimiento;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionDocumento_ConsultarFiltros";
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
		Obten.Llena<CImpresionDocumento>(typeof(CImpresionDocumento), pConexion);
		foreach (CImpresionDocumento O in Obten.ListaRegistros)
		{
			idImpresionDocumento = O.IdImpresionDocumento;
			impresionDocumento = O.ImpresionDocumento;
			procedimiento = O.Procedimiento;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionDocumento_ConsultarFiltros";
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
		Obten.Llena<CImpresionDocumento>(typeof(CImpresionDocumento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ImpresionDocumento_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionDocumento", 0);
		Agregar.StoredProcedure.Parameters["@pIdImpresionDocumento"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pImpresionDocumento", impresionDocumento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProcedimiento", procedimiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idImpresionDocumento= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdImpresionDocumento"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ImpresionDocumento_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionDocumento", idImpresionDocumento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pImpresionDocumento", impresionDocumento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProcedimiento", procedimiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ImpresionDocumento_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionDocumento", idImpresionDocumento);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}