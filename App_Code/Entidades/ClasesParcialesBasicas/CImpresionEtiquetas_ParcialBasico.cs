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

public partial class CImpresionEtiquetas
{
	//Propiedades Privadas
	private int idImpresionEtiquetas;
	private int idImpresionTemplate;
	private string campo;
	private string etiqueta;
	private bool baja;
	
	//Propiedades
	public int IdImpresionEtiquetas
	{
		get { return idImpresionEtiquetas; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idImpresionEtiquetas = value;
		}
	}
	
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
	
	public string Campo
	{
		get { return campo; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			campo = value;
		}
	}
	
	public string Etiqueta
	{
		get { return etiqueta; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			etiqueta = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CImpresionEtiquetas()
	{
		idImpresionEtiquetas = 0;
		idImpresionTemplate = 0;
		campo = "";
		etiqueta = "";
		baja = false;
	}
	
	public CImpresionEtiquetas(int pIdImpresionEtiquetas)
	{
		idImpresionEtiquetas = pIdImpresionEtiquetas;
		idImpresionTemplate = 0;
		campo = "";
		etiqueta = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CImpresionEtiquetas>(typeof(CImpresionEtiquetas), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CImpresionEtiquetas>(typeof(CImpresionEtiquetas), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdImpresionEtiquetas", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CImpresionEtiquetas>(typeof(CImpresionEtiquetas), pConexion);
		foreach (CImpresionEtiquetas O in Obten.ListaRegistros)
		{
			idImpresionEtiquetas = O.IdImpresionEtiquetas;
			idImpresionTemplate = O.IdImpresionTemplate;
			campo = O.Campo;
			etiqueta = O.Etiqueta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_ConsultarFiltros";
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
		Obten.Llena<CImpresionEtiquetas>(typeof(CImpresionEtiquetas), pConexion);
		foreach (CImpresionEtiquetas O in Obten.ListaRegistros)
		{
			idImpresionEtiquetas = O.IdImpresionEtiquetas;
			idImpresionTemplate = O.IdImpresionTemplate;
			campo = O.Campo;
			etiqueta = O.Etiqueta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_ConsultarFiltros";
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
		Obten.Llena<CImpresionEtiquetas>(typeof(CImpresionEtiquetas), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionEtiquetas", 0);
		Agregar.StoredProcedure.Parameters["@pIdImpresionEtiquetas"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionTemplate", idImpresionTemplate);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCampo", campo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEtiqueta", etiqueta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idImpresionEtiquetas= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdImpresionEtiquetas"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionEtiquetas", idImpresionEtiquetas);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionTemplate", idImpresionTemplate);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCampo", campo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEtiqueta", etiqueta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ImpresionEtiquetas_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdImpresionEtiquetas", idImpresionEtiquetas);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}