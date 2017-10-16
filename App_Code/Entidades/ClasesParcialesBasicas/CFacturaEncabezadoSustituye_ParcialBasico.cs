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

public partial class CFacturaEncabezadoSustituye
{
	//Propiedades Privadas
	private int idFacturaEncabezadoSustituye;
	private int idFactura;
	private int idFacturaSustituye;
	private bool baja;
	
	//Propiedades
	public int IdFacturaEncabezadoSustituye
	{
		get { return idFacturaEncabezadoSustituye; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idFacturaEncabezadoSustituye = value;
		}
	}
	
	public int IdFactura
	{
		get { return idFactura; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idFactura = value;
		}
	}
	
	public int IdFacturaSustituye
	{
		get { return idFacturaSustituye; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idFacturaSustituye = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CFacturaEncabezadoSustituye()
	{
		idFacturaEncabezadoSustituye = 0;
		idFactura = 0;
		idFacturaSustituye = 0;
		baja = false;
	}
	
	public CFacturaEncabezadoSustituye(int pIdFacturaEncabezadoSustituye)
	{
		idFacturaEncabezadoSustituye = pIdFacturaEncabezadoSustituye;
		idFactura = 0;
		idFacturaSustituye = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFacturaEncabezadoSustituye>(typeof(CFacturaEncabezadoSustituye), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CFacturaEncabezadoSustituye>(typeof(CFacturaEncabezadoSustituye), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSustituye", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFacturaEncabezadoSustituye>(typeof(CFacturaEncabezadoSustituye), pConexion);
		foreach (CFacturaEncabezadoSustituye O in Obten.ListaRegistros)
		{
			idFacturaEncabezadoSustituye = O.IdFacturaEncabezadoSustituye;
			idFactura = O.IdFactura;
			idFacturaSustituye = O.IdFacturaSustituye;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_ConsultarFiltros";
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
		Obten.Llena<CFacturaEncabezadoSustituye>(typeof(CFacturaEncabezadoSustituye), pConexion);
		foreach (CFacturaEncabezadoSustituye O in Obten.ListaRegistros)
		{
			idFacturaEncabezadoSustituye = O.IdFacturaEncabezadoSustituye;
			idFactura = O.IdFactura;
			idFacturaSustituye = O.IdFacturaSustituye;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_ConsultarFiltros";
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
		Obten.Llena<CFacturaEncabezadoSustituye>(typeof(CFacturaEncabezadoSustituye), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSustituye", 0);
		Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezadoSustituye"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFactura", idFactura);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaSustituye", idFacturaSustituye);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idFacturaEncabezadoSustituye= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezadoSustituye"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSustituye", idFacturaEncabezadoSustituye);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFactura", idFactura);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaSustituye", idFacturaSustituye);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_FacturaEncabezadoSustituye_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSustituye", idFacturaEncabezadoSustituye);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}