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

public partial class CEstructuraAddenda
{
	//Propiedades Privadas
	private int idEstructuraAddenda;
	private string estructuraAddenda;
	private int idAddenda;
	private bool esVisible;
	private int idTipoElemento;
	private int idPadre;
	private bool tieneHijos;
	private int orden;
	private string prefijo;
	private bool baja;
	
	//Propiedades
	public int IdEstructuraAddenda
	{
		get { return idEstructuraAddenda; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEstructuraAddenda = value;
		}
	}
	
	public string EstructuraAddenda
	{
		get { return estructuraAddenda; }
		set
		{
			estructuraAddenda = value;
		}
	}
	
	public int IdAddenda
	{
		get { return idAddenda; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idAddenda = value;
		}
	}
	
	public bool EsVisible
	{
		get { return esVisible; }
		set { esVisible = value; }
	}
	
	public int IdTipoElemento
	{
		get { return idTipoElemento; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoElemento = value;
		}
	}
	
	public int IdPadre
	{
		get { return idPadre; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPadre = value;
		}
	}
	
	public bool TieneHijos
	{
		get { return tieneHijos; }
		set { tieneHijos = value; }
	}
	
	public int Orden
	{
		get { return orden; }
		set
		{
			if (value < 0)
			{
				return;
			}
			orden = value;
		}
	}
	
	public string Prefijo
	{
		get { return prefijo; }
		set
		{
			prefijo = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CEstructuraAddenda()
	{
		idEstructuraAddenda = 0;
		estructuraAddenda = "";
		idAddenda = 0;
		esVisible = false;
		idTipoElemento = 0;
		idPadre = 0;
		tieneHijos = false;
		orden = 0;
		prefijo = "";
		baja = false;
	}
	
	public CEstructuraAddenda(int pIdEstructuraAddenda)
	{
		idEstructuraAddenda = pIdEstructuraAddenda;
		estructuraAddenda = "";
		idAddenda = 0;
		esVisible = false;
		idTipoElemento = 0;
		idPadre = 0;
		tieneHijos = false;
		orden = 0;
		prefijo = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstructuraAddenda_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstructuraAddenda>(typeof(CEstructuraAddenda), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstructuraAddenda_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CEstructuraAddenda>(typeof(CEstructuraAddenda), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstructuraAddenda_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstructuraAddenda>(typeof(CEstructuraAddenda), pConexion);
		foreach (CEstructuraAddenda O in Obten.ListaRegistros)
		{
			idEstructuraAddenda = O.IdEstructuraAddenda;
			estructuraAddenda = O.EstructuraAddenda;
			idAddenda = O.IdAddenda;
			esVisible = O.EsVisible;
			idTipoElemento = O.IdTipoElemento;
			idPadre = O.IdPadre;
			tieneHijos = O.TieneHijos;
			orden = O.Orden;
			prefijo = O.Prefijo;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstructuraAddenda_ConsultarFiltros";
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
		Obten.Llena<CEstructuraAddenda>(typeof(CEstructuraAddenda), pConexion);
		foreach (CEstructuraAddenda O in Obten.ListaRegistros)
		{
			idEstructuraAddenda = O.IdEstructuraAddenda;
			estructuraAddenda = O.EstructuraAddenda;
			idAddenda = O.IdAddenda;
			esVisible = O.EsVisible;
			idTipoElemento = O.IdTipoElemento;
			idPadre = O.IdPadre;
			tieneHijos = O.TieneHijos;
			orden = O.Orden;
			prefijo = O.Prefijo;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstructuraAddenda_ConsultarFiltros";
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
		Obten.Llena<CEstructuraAddenda>(typeof(CEstructuraAddenda), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_EstructuraAddenda_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", 0);
		Agregar.StoredProcedure.Parameters["@pIdEstructuraAddenda"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEstructuraAddenda", estructuraAddenda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAddenda", idAddenda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsVisible", esVisible);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoElemento", idTipoElemento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPadre", idPadre);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTieneHijos", tieneHijos);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPrefijo", prefijo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idEstructuraAddenda= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEstructuraAddenda"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_EstructuraAddenda_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", idEstructuraAddenda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEstructuraAddenda", estructuraAddenda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAddenda", idAddenda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsVisible", esVisible);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoElemento", idTipoElemento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPadre", idPadre);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTieneHijos", tieneHijos);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPrefijo", prefijo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_EstructuraAddenda_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", idEstructuraAddenda);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}