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

public partial class CImpresionTemplate
{
	//Propiedades Privadas
	private int idImpresionTemplate;
	private int idImpresionDocumento;
	private string rutaTemplate;
	private string rutaCSS;
	private int idEmpresa;
	private bool baja;
	
	//Propiedades
	public int IdImpresionTemplate
	{
		get { return idImpresionTemplate; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idImpresionTemplate = value;
		}
	}
	
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
	
	public string RutaTemplate
	{
		get { return rutaTemplate; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			rutaTemplate = value;
		}
	}
	
	public string RutaCSS
	{
		get { return rutaCSS; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			rutaCSS = value;
		}
	}
	
	public int IdEmpresa
	{
		get { return idEmpresa; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEmpresa = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CImpresionTemplate()
	{
		idImpresionTemplate = 0;
		idImpresionDocumento = 0;
		rutaTemplate = "";
		rutaCSS = "";
		idEmpresa = 0;
		baja = false;
	}
	
	public CImpresionTemplate(int pIdImpresionTemplate)
	{
		idImpresionTemplate = pIdImpresionTemplate;
		idImpresionDocumento = 0;
		rutaTemplate = "";
		rutaCSS = "";
		idEmpresa = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionTemplate_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CImpresionTemplate>(typeof(CImpresionTemplate), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionTemplate_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CImpresionTemplate>(typeof(CImpresionTemplate), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionTemplate_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdImpresionTemplate", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CImpresionTemplate>(typeof(CImpresionTemplate), pConexion);
		foreach (CImpresionTemplate O in Obten.ListaRegistros)
		{
			idImpresionTemplate = O.IdImpresionTemplate;
			idImpresionDocumento = O.IdImpresionDocumento;
			rutaTemplate = O.RutaTemplate;
			rutaCSS = O.RutaCSS;
			idEmpresa = O.IdEmpresa;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionTemplate_ConsultarFiltros";
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
		Obten.Llena<CImpresionTemplate>(typeof(CImpresionTemplate), pConexion);
		foreach (CImpresionTemplate O in Obten.ListaRegistros)
		{
			idImpresionTemplate = O.IdImpresionTemplate;
			idImpresionDocumento = O.IdImpresionDocumento;
			rutaTemplate = O.RutaTemplate;
			rutaCSS = O.RutaCSS;
			idEmpresa = O.IdEmpresa;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionTemplate_ConsultarFiltros";
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
		Obten.Llena<CImpresionTemplate>(typeof(CImpresionTemplate), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ImpresionTemplate_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionTemplate", 0);
		Agregar.StoredProcedure.Parameters["@pIdImpresionTemplate"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionDocumento", idImpresionDocumento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRutaTemplate", rutaTemplate);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRutaCSS", rutaCSS);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idImpresionTemplate= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdImpresionTemplate"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ImpresionTemplate_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionTemplate", idImpresionTemplate);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionDocumento", idImpresionDocumento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRutaTemplate", rutaTemplate);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRutaCSS", rutaCSS);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ImpresionTemplate_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionTemplate", idImpresionTemplate);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}