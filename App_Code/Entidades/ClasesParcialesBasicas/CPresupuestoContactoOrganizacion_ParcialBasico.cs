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

public partial class CPresupuestoContactoOrganizacion
{
	//Propiedades Privadas
	private int idPresupuestoContactoOrganizacion;
	private int idPresupuesto;
	private int idContactoOrganizacion;
	private int idUsuario;
	private bool baja;
	
	//Propiedades
	public int IdPresupuestoContactoOrganizacion
	{
		get { return idPresupuestoContactoOrganizacion; }
		set
		{
			idPresupuestoContactoOrganizacion = value;
		}
	}
	
	public int IdPresupuesto
	{
		get { return idPresupuesto; }
		set
		{
			idPresupuesto = value;
		}
	}
	
	public int IdContactoOrganizacion
	{
		get { return idContactoOrganizacion; }
		set
		{
			idContactoOrganizacion = value;
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
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CPresupuestoContactoOrganizacion()
	{
		idPresupuestoContactoOrganizacion = 0;
		idPresupuesto = 0;
		idContactoOrganizacion = 0;
		idUsuario = 0;
		baja = false;
	}
	
	public CPresupuestoContactoOrganizacion(int pIdPresupuestoContactoOrganizacion)
	{
		idPresupuestoContactoOrganizacion = pIdPresupuestoContactoOrganizacion;
		idPresupuesto = 0;
		idContactoOrganizacion = 0;
		idUsuario = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPresupuestoContactoOrganizacion>(typeof(CPresupuestoContactoOrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPresupuestoContactoOrganizacion>(typeof(CPresupuestoContactoOrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoContactoOrganizacion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPresupuestoContactoOrganizacion>(typeof(CPresupuestoContactoOrganizacion), pConexion);
		foreach (CPresupuestoContactoOrganizacion O in Obten.ListaRegistros)
		{
			idPresupuestoContactoOrganizacion = O.IdPresupuestoContactoOrganizacion;
			idPresupuesto = O.IdPresupuesto;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			idUsuario = O.IdUsuario;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_ConsultarFiltros";
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
		Obten.Llena<CPresupuestoContactoOrganizacion>(typeof(CPresupuestoContactoOrganizacion), pConexion);
		foreach (CPresupuestoContactoOrganizacion O in Obten.ListaRegistros)
		{
			idPresupuestoContactoOrganizacion = O.IdPresupuestoContactoOrganizacion;
			idPresupuesto = O.IdPresupuesto;
			idContactoOrganizacion = O.IdContactoOrganizacion;
			idUsuario = O.IdUsuario;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_ConsultarFiltros";
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
		Obten.Llena<CPresupuestoContactoOrganizacion>(typeof(CPresupuestoContactoOrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoContactoOrganizacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdPresupuestoContactoOrganizacion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idPresupuestoContactoOrganizacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdPresupuestoContactoOrganizacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoContactoOrganizacion", idPresupuestoContactoOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_PresupuestoContactoOrganizacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoContactoOrganizacion", idPresupuestoContactoOrganizacion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
