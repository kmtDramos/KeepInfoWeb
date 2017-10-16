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

public partial class CSubCategoria
{
	//Propiedades Privadas
	private int idSubCategoria;
	private int idCategoria;
	private string subCategoria;
	private bool baja;
	
	//Propiedades
	public int IdSubCategoria
	{
		get { return idSubCategoria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSubCategoria = value;
		}
	}
	
	public int IdCategoria
	{
		get { return idCategoria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCategoria = value;
		}
	}
	
	public string SubCategoria
	{
		get { return subCategoria; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			subCategoria = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSubCategoria()
	{
		idSubCategoria = 0;
		idCategoria = 0;
		subCategoria = "";
		baja = false;
	}
	
	public CSubCategoria(int pIdSubCategoria)
	{
		idSubCategoria = pIdSubCategoria;
		idCategoria = 0;
		subCategoria = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCategoria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSubCategoria>(typeof(CSubCategoria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCategoria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSubCategoria>(typeof(CSubCategoria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCategoria_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSubCategoria", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSubCategoria>(typeof(CSubCategoria), pConexion);
		foreach (CSubCategoria O in Obten.ListaRegistros)
		{
			idSubCategoria = O.IdSubCategoria;
			idCategoria = O.IdCategoria;
			subCategoria = O.SubCategoria;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCategoria_ConsultarFiltros";
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
		Obten.Llena<CSubCategoria>(typeof(CSubCategoria), pConexion);
		foreach (CSubCategoria O in Obten.ListaRegistros)
		{
			idSubCategoria = O.IdSubCategoria;
			idCategoria = O.IdCategoria;
			subCategoria = O.SubCategoria;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SubCategoria_ConsultarFiltros";
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
		Obten.Llena<CSubCategoria>(typeof(CSubCategoria), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SubCategoria_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSubCategoria", 0);
		Agregar.StoredProcedure.Parameters["@pIdSubCategoria"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCategoria", idCategoria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSubCategoria", subCategoria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSubCategoria= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSubCategoria"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SubCategoria_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSubCategoria", idSubCategoria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCategoria", idCategoria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSubCategoria", subCategoria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SubCategoria_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSubCategoria", idSubCategoria);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}