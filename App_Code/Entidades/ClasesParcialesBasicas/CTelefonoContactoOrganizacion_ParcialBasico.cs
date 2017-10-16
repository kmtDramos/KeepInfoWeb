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

public partial class CTelefonoContactoOrganizacion
{
	//Propiedades Privadas
	private int idTelefonoContactoOrganizacion;
	private string telefono;
	private int idContactoOrganizacion;
	private string descripcion;
	private bool baja;
	
	//Propiedades
	public int IdTelefonoContactoOrganizacion
	{
		get { return idTelefonoContactoOrganizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTelefonoContactoOrganizacion = value;
		}
	}
	
	public string Telefono
	{
		get { return telefono; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			telefono = value;
		}
	}
	
	public int IdContactoOrganizacion
	{
		get { return idContactoOrganizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idContactoOrganizacion = value;
		}
	}
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			descripcion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CTelefonoContactoOrganizacion()
	{
		idTelefonoContactoOrganizacion = 0;
		telefono = "";
		idContactoOrganizacion = 0;
		descripcion = "";
		baja = false;
	}
	
	public CTelefonoContactoOrganizacion(int pIdTelefonoContactoOrganizacion)
	{
		idTelefonoContactoOrganizacion = pIdTelefonoContactoOrganizacion;
		telefono = "";
		idContactoOrganizacion = 0;
		descripcion = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTelefonoContactoOrganizacion>(typeof(CTelefonoContactoOrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CTelefonoContactoOrganizacion>(typeof(CTelefonoContactoOrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdTelefonoContactoOrganizacion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CTelefonoContactoOrganizacion>(typeof(CTelefonoContactoOrganizacion), pConexion);
		foreach (CTelefonoContactoOrganizacion O in Obten.ListaRegistros)
		{
			idTelefonoContactoOrganizacion = O.IdTelefonoContactoOrganizacion;
			telefono = O.Telefono;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			descripcion = O.Descripcion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_ConsultarFiltros";
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
		Obten.Llena<CTelefonoContactoOrganizacion>(typeof(CTelefonoContactoOrganizacion), pConexion);
		foreach (CTelefonoContactoOrganizacion O in Obten.ListaRegistros)
		{
			idTelefonoContactoOrganizacion = O.IdTelefonoContactoOrganizacion;
			telefono = O.Telefono;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			descripcion = O.Descripcion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_ConsultarFiltros";
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
		Obten.Llena<CTelefonoContactoOrganizacion>(typeof(CTelefonoContactoOrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTelefonoContactoOrganizacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdTelefonoContactoOrganizacion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idTelefonoContactoOrganizacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdTelefonoContactoOrganizacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTelefonoContactoOrganizacion", idTelefonoContactoOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_TelefonoContactoOrganizacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdTelefonoContactoOrganizacion", idTelefonoContactoOrganizacion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}