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

public partial class CUsuarioDivision
{
	//Propiedades Privadas
	private int idUsuarioDivision;
	private int idUsuario;
	private int idDivision;
	private decimal meta;
	
	//Propiedades
	public int IdUsuarioDivision
	{
		get { return idUsuarioDivision; }
		set
		{
			idUsuarioDivision = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
		}
	}
	
	public decimal Meta
	{
		get { return meta; }
		set
		{
			meta = value;
		}
	}
	
	//Constructores
	public CUsuarioDivision()
	{
		idUsuarioDivision = 0;
		idUsuario = 0;
		idDivision = 0;
		meta = 0;
	}
	
	public CUsuarioDivision(int pIdUsuarioDivision)
	{
		idUsuarioDivision = pIdUsuarioDivision;
		idUsuario = 0;
		idDivision = 0;
		meta = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioDivision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CUsuarioDivision>(typeof(CUsuarioDivision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioDivision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CUsuarioDivision>(typeof(CUsuarioDivision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioDivision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioDivision", pIdentificador);
		Obten.Llena<CUsuarioDivision>(typeof(CUsuarioDivision), pConexion);
		foreach (CUsuarioDivision O in Obten.ListaRegistros)
		{
			idUsuarioDivision = O.IdUsuarioDivision;
			idUsuario = O.IdUsuario;
			idDivision = O.IdDivision;
			meta = O.Meta;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioDivision_ConsultarFiltros";
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
		Obten.Llena<CUsuarioDivision>(typeof(CUsuarioDivision), pConexion);
		foreach (CUsuarioDivision O in Obten.ListaRegistros)
		{
			idUsuarioDivision = O.IdUsuarioDivision;
			idUsuario = O.IdUsuario;
			idDivision = O.IdDivision;
			meta = O.Meta;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_UsuarioDivision_ConsultarFiltros";
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
		Obten.Llena<CUsuarioDivision>(typeof(CUsuarioDivision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_UsuarioDivision_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioDivision", 0);
		Agregar.StoredProcedure.Parameters["@pIdUsuarioDivision"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMeta", meta);
		Agregar.Insert(pConexion);
		idUsuarioDivision= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdUsuarioDivision"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_UsuarioDivision_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioDivision", idUsuarioDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMeta", meta);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_UsuarioDivision_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioDivision", idUsuarioDivision);
		Eliminar.Delete(pConexion);
	}
}
