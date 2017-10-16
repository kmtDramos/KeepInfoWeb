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

public partial class CNivelInteresCotizacion
{
	//Propiedades Privadas
	private int idNivelInteresCotizacion;
	private string nivelInteresCotizacion;
	private string abreviatura;
	private int orden;
	private bool baja;
	
	//Propiedades
	public int IdNivelInteresCotizacion
	{
		get { return idNivelInteresCotizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNivelInteresCotizacion = value;
		}
	}
	
	public string NivelInteresCotizacion
	{
		get { return nivelInteresCotizacion; }
		set
		{
			nivelInteresCotizacion = value;
		}
	}
	
	public string Abreviatura
	{
		get { return abreviatura; }
		set
		{
			abreviatura = value;
		}
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
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CNivelInteresCotizacion()
	{
		idNivelInteresCotizacion = 0;
		nivelInteresCotizacion = "";
		abreviatura = "";
		orden = 0;
		baja = false;
	}
	
	public CNivelInteresCotizacion(int pIdNivelInteresCotizacion)
	{
		idNivelInteresCotizacion = pIdNivelInteresCotizacion;
		nivelInteresCotizacion = "";
		abreviatura = "";
		orden = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNivelInteresCotizacion>(typeof(CNivelInteresCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CNivelInteresCotizacion>(typeof(CNivelInteresCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresCotizacion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNivelInteresCotizacion>(typeof(CNivelInteresCotizacion), pConexion);
		foreach (CNivelInteresCotizacion O in Obten.ListaRegistros)
		{
			idNivelInteresCotizacion = O.IdNivelInteresCotizacion;
			nivelInteresCotizacion = O.NivelInteresCotizacion;
			abreviatura = O.Abreviatura;
			orden = O.Orden;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_ConsultarFiltros";
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
		Obten.Llena<CNivelInteresCotizacion>(typeof(CNivelInteresCotizacion), pConexion);
		foreach (CNivelInteresCotizacion O in Obten.ListaRegistros)
		{
			idNivelInteresCotizacion = O.IdNivelInteresCotizacion;
			nivelInteresCotizacion = O.NivelInteresCotizacion;
			abreviatura = O.Abreviatura;
			orden = O.Orden;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_ConsultarFiltros";
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
		Obten.Llena<CNivelInteresCotizacion>(typeof(CNivelInteresCotizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresCotizacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdNivelInteresCotizacion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNivelInteresCotizacion", nivelInteresCotizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idNivelInteresCotizacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNivelInteresCotizacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresCotizacion", idNivelInteresCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNivelInteresCotizacion", nivelInteresCotizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAbreviatura", abreviatura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_NivelInteresCotizacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresCotizacion", idNivelInteresCotizacion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}