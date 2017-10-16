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

public partial class CGrupoEmpresarial
{
	//Propiedades Privadas
	private int idGrupoEmpresarial;
	private string grupoEmpresarial;
	private string clave;
	private bool baja;
	
	//Propiedades
	public int IdGrupoEmpresarial
	{
		get { return idGrupoEmpresarial; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idGrupoEmpresarial = value;
		}
	}
	
	public string GrupoEmpresarial
	{
		get { return grupoEmpresarial; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			grupoEmpresarial = value;
		}
	}
	
	public string Clave
	{
		get { return clave; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			clave = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CGrupoEmpresarial()
	{
		idGrupoEmpresarial = 0;
		grupoEmpresarial = "";
		clave = "";
		baja = false;
	}
	
	public CGrupoEmpresarial(int pIdGrupoEmpresarial)
	{
		idGrupoEmpresarial = pIdGrupoEmpresarial;
		grupoEmpresarial = "";
		clave = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_GrupoEmpresarial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CGrupoEmpresarial>(typeof(CGrupoEmpresarial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_GrupoEmpresarial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CGrupoEmpresarial>(typeof(CGrupoEmpresarial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_GrupoEmpresarial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdGrupoEmpresarial", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CGrupoEmpresarial>(typeof(CGrupoEmpresarial), pConexion);
		foreach (CGrupoEmpresarial O in Obten.ListaRegistros)
		{
			idGrupoEmpresarial = O.IdGrupoEmpresarial;
			grupoEmpresarial = O.GrupoEmpresarial;
			clave = O.Clave;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_GrupoEmpresarial_ConsultarFiltros";
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
		Obten.Llena<CGrupoEmpresarial>(typeof(CGrupoEmpresarial), pConexion);
		foreach (CGrupoEmpresarial O in Obten.ListaRegistros)
		{
			idGrupoEmpresarial = O.IdGrupoEmpresarial;
			grupoEmpresarial = O.GrupoEmpresarial;
			clave = O.Clave;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_GrupoEmpresarial_ConsultarFiltros";
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
		Obten.Llena<CGrupoEmpresarial>(typeof(CGrupoEmpresarial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_GrupoEmpresarial_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGrupoEmpresarial", 0);
		Agregar.StoredProcedure.Parameters["@pIdGrupoEmpresarial"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pGrupoEmpresarial", grupoEmpresarial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idGrupoEmpresarial= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdGrupoEmpresarial"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_GrupoEmpresarial_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdGrupoEmpresarial", idGrupoEmpresarial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pGrupoEmpresarial", grupoEmpresarial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_GrupoEmpresarial_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdGrupoEmpresarial", idGrupoEmpresarial);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}