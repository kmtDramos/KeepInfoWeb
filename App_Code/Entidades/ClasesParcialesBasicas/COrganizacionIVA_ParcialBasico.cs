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

public partial class COrganizacionIVA
{
	//Propiedades Privadas
	private int idOrganizacionIVA;
	private int iVA;
	private bool esPrincipal;
	private int idOrganizacion;
	private bool baja;
	
	//Propiedades
	public int IdOrganizacionIVA
	{
		get { return idOrganizacionIVA; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOrganizacionIVA = value;
		}
	}
	
	public int IVA
	{
		get { return iVA; }
		set
		{
			if (value < 0)
			{
				return;
			}
			iVA = value;
		}
	}
	
	public bool EsPrincipal
	{
		get { return esPrincipal; }
		set { esPrincipal = value; }
	}
	
	public int IdOrganizacion
	{
		get { return idOrganizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOrganizacion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public COrganizacionIVA()
	{
		idOrganizacionIVA = 0;
		iVA = 0;
		esPrincipal = false;
		idOrganizacion = 0;
		baja = false;
	}
	
	public COrganizacionIVA(int pIdOrganizacionIVA)
	{
		idOrganizacionIVA = pIdOrganizacionIVA;
		iVA = 0;
		esPrincipal = false;
		idOrganizacion = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrganizacionIVA_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrganizacionIVA>(typeof(COrganizacionIVA), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrganizacionIVA_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COrganizacionIVA>(typeof(COrganizacionIVA), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrganizacionIVA_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacionIVA", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrganizacionIVA>(typeof(COrganizacionIVA), pConexion);
		foreach (COrganizacionIVA O in Obten.ListaRegistros)
		{
			idOrganizacionIVA = O.IdOrganizacionIVA;
			iVA = O.IVA;
			esPrincipal = O.EsPrincipal;
			idOrganizacion = O.IdOrganizacion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrganizacionIVA_ConsultarFiltros";
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
		Obten.Llena<COrganizacionIVA>(typeof(COrganizacionIVA), pConexion);
		foreach (COrganizacionIVA O in Obten.ListaRegistros)
		{
			idOrganizacionIVA = O.IdOrganizacionIVA;
			iVA = O.IVA;
			esPrincipal = O.EsPrincipal;
			idOrganizacion = O.IdOrganizacion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrganizacionIVA_ConsultarFiltros";
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
		Obten.Llena<COrganizacionIVA>(typeof(COrganizacionIVA), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_OrganizacionIVA_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacionIVA", 0);
		Agregar.StoredProcedure.Parameters["@pIdOrganizacionIVA"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsPrincipal", esPrincipal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idOrganizacionIVA= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrganizacionIVA"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_OrganizacionIVA_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacionIVA", idOrganizacionIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsPrincipal", esPrincipal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_OrganizacionIVA_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacionIVA", idOrganizacionIVA);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}