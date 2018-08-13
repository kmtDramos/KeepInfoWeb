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

public partial class CSolicitudMaterialConcepto
{
	//Propiedades Privadas
	private int idSolicitudMaterialConcepto;
	private int idPresupuesto;
	private int idSolicitudMaterial;
	private int idPresupuestoConcepto;
	private int cantidad;
	private bool baja;
	
	//Propiedades
	public int IdSolicitudMaterialConcepto
	{
		get { return idSolicitudMaterialConcepto; }
		set
		{
			idSolicitudMaterialConcepto = value;
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
	
	public int IdSolicitudMaterial
	{
		get { return idSolicitudMaterial; }
		set
		{
			idSolicitudMaterial = value;
		}
	}
	
	public int IdPresupuestoConcepto
	{
		get { return idPresupuestoConcepto; }
		set
		{
			idPresupuestoConcepto = value;
		}
	}
	
	public int Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSolicitudMaterialConcepto()
	{
		idSolicitudMaterialConcepto = 0;
		idPresupuesto = 0;
		idSolicitudMaterial = 0;
		idPresupuestoConcepto = 0;
		cantidad = 0;
		baja = false;
	}
	
	public CSolicitudMaterialConcepto(int pIdSolicitudMaterialConcepto)
	{
		idSolicitudMaterialConcepto = pIdSolicitudMaterialConcepto;
		idPresupuesto = 0;
		idSolicitudMaterial = 0;
		idPresupuestoConcepto = 0;
		cantidad = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudMaterialConcepto>(typeof(CSolicitudMaterialConcepto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSolicitudMaterialConcepto>(typeof(CSolicitudMaterialConcepto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterialConcepto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudMaterialConcepto>(typeof(CSolicitudMaterialConcepto), pConexion);
		foreach (CSolicitudMaterialConcepto O in Obten.ListaRegistros)
		{
			idSolicitudMaterialConcepto = O.IdSolicitudMaterialConcepto;
			idPresupuesto = O.IdPresupuesto;
			idSolicitudMaterial = O.IdSolicitudMaterial;
			idPresupuestoConcepto = O.IdPresupuestoConcepto;
			cantidad = O.Cantidad;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_ConsultarFiltros";
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
		Obten.Llena<CSolicitudMaterialConcepto>(typeof(CSolicitudMaterialConcepto), pConexion);
		foreach (CSolicitudMaterialConcepto O in Obten.ListaRegistros)
		{
			idSolicitudMaterialConcepto = O.IdSolicitudMaterialConcepto;
			idPresupuesto = O.IdPresupuesto;
			idSolicitudMaterial = O.IdSolicitudMaterial;
			idPresupuestoConcepto = O.IdPresupuestoConcepto;
			cantidad = O.Cantidad;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_ConsultarFiltros";
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
		Obten.Llena<CSolicitudMaterialConcepto>(typeof(CSolicitudMaterialConcepto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterialConcepto", 0);
		Agregar.StoredProcedure.Parameters["@pIdSolicitudMaterialConcepto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterial", idSolicitudMaterial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", idPresupuestoConcepto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSolicitudMaterialConcepto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSolicitudMaterialConcepto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterialConcepto", idSolicitudMaterialConcepto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterial", idSolicitudMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", idPresupuestoConcepto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SolicitudMaterialConcepto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterialConcepto", idSolicitudMaterialConcepto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
