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

public partial class CDescuentoServicio
{
	//Propiedades Privadas
	private int idDescuentoServicio;
	private int idServicio;
	private string descuentoServicio;
	private decimal descuento;
	private bool baja;
	
	//Propiedades
	public int IdDescuentoServicio
	{
		get { return idDescuentoServicio; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDescuentoServicio = value;
		}
	}
	
	public int IdServicio
	{
		get { return idServicio; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idServicio = value;
		}
	}
	
	public string DescuentoServicio
	{
		get { return descuentoServicio; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			descuentoServicio = value;
		}
	}
	
	public decimal Descuento
	{
		get { return descuento; }
		set
		{
			if (value < 0)
			{
				return;
			}
			descuento = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CDescuentoServicio()
	{
		idDescuentoServicio = 0;
		idServicio = 0;
		descuentoServicio = "";
		descuento = 0;
		baja = false;
	}
	
	public CDescuentoServicio(int pIdDescuentoServicio)
	{
		idDescuentoServicio = pIdDescuentoServicio;
		idServicio = 0;
		descuentoServicio = "";
		descuento = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DescuentoServicio_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDescuentoServicio>(typeof(CDescuentoServicio), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DescuentoServicio_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDescuentoServicio>(typeof(CDescuentoServicio), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DescuentoServicio_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoServicio", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDescuentoServicio>(typeof(CDescuentoServicio), pConexion);
		foreach (CDescuentoServicio O in Obten.ListaRegistros)
		{
			idDescuentoServicio = O.IdDescuentoServicio;
			idServicio = O.IdServicio;
			descuentoServicio = O.DescuentoServicio;
			descuento = O.Descuento;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DescuentoServicio_ConsultarFiltros";
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
		Obten.Llena<CDescuentoServicio>(typeof(CDescuentoServicio), pConexion);
		foreach (CDescuentoServicio O in Obten.ListaRegistros)
		{
			idDescuentoServicio = O.IdDescuentoServicio;
			idServicio = O.IdServicio;
			descuentoServicio = O.DescuentoServicio;
			descuento = O.Descuento;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DescuentoServicio_ConsultarFiltros";
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
		Obten.Llena<CDescuentoServicio>(typeof(CDescuentoServicio), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_DescuentoServicio_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoServicio", 0);
		Agregar.StoredProcedure.Parameters["@pIdDescuentoServicio"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuentoServicio", descuentoServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDescuentoServicio= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDescuentoServicio"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_DescuentoServicio_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoServicio", idDescuentoServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuentoServicio", descuentoServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_DescuentoServicio_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoServicio", idDescuentoServicio);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}