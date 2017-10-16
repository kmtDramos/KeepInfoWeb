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

public partial class CEstatusEncabezadoFacturaProveedor
{
	//Propiedades Privadas
	private int idEstatusEncabezadoFacturaProveedor;
	private string descripcion;
	
	//Propiedades
	public int IdEstatusEncabezadoFacturaProveedor
	{
		get { return idEstatusEncabezadoFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEstatusEncabezadoFacturaProveedor = value;
		}
	}
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			descripcion = value;
		}
	}
	
	//Constructores
	public CEstatusEncabezadoFacturaProveedor()
	{
		idEstatusEncabezadoFacturaProveedor = 0;
		descripcion = "";
	}
	
	public CEstatusEncabezadoFacturaProveedor(int pIdEstatusEncabezadoFacturaProveedor)
	{
		idEstatusEncabezadoFacturaProveedor = pIdEstatusEncabezadoFacturaProveedor;
		descripcion = "";
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CEstatusEncabezadoFacturaProveedor>(typeof(CEstatusEncabezadoFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CEstatusEncabezadoFacturaProveedor>(typeof(CEstatusEncabezadoFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdEstatusEncabezadoFacturaProveedor", pIdentificador);
		Obten.Llena<CEstatusEncabezadoFacturaProveedor>(typeof(CEstatusEncabezadoFacturaProveedor), pConexion);
		foreach (CEstatusEncabezadoFacturaProveedor O in Obten.ListaRegistros)
		{
			idEstatusEncabezadoFacturaProveedor = O.IdEstatusEncabezadoFacturaProveedor;
			descripcion = O.Descripcion;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CEstatusEncabezadoFacturaProveedor>(typeof(CEstatusEncabezadoFacturaProveedor), pConexion);
		foreach (CEstatusEncabezadoFacturaProveedor O in Obten.ListaRegistros)
		{
			idEstatusEncabezadoFacturaProveedor = O.IdEstatusEncabezadoFacturaProveedor;
			descripcion = O.Descripcion;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_ConsultarFiltros";
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
		Obten.Llena<CEstatusEncabezadoFacturaProveedor>(typeof(CEstatusEncabezadoFacturaProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusEncabezadoFacturaProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdEstatusEncabezadoFacturaProveedor"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.Insert(pConexion);
		idEstatusEncabezadoFacturaProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEstatusEncabezadoFacturaProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusEncabezadoFacturaProveedor", idEstatusEncabezadoFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_EstatusEncabezadoFacturaProveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusEncabezadoFacturaProveedor", idEstatusEncabezadoFacturaProveedor);
		Eliminar.Delete(pConexion);
	}
}