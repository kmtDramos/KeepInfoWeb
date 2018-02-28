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

public partial class CComparativa
{
	//Propiedades Privadas
	private int idComparativa;
	private int idOrdenCompraEncabezado;
	private string proveedor1;
	private string proveedor2;
	private string proveedor3;
	private int idProveedor1;
	private int idProveedor2;
	private int idProveedor3;
	private decimal costo1;
	private decimal costo2;
	private decimal costo3;
	private int diasCredito1;
	private int diasCredito2;
	private int diasCredito3;
	private int diasEntrega1;
	private int diasEntrega2;
	private int diasEntrega3;
	private decimal margen1;
	private decimal margen2;
	private decimal margen3;
	private int idUsuarioModificacion;
	private DateTime fechaModificacion;
	
	//Propiedades
	public int IdComparativa
	{
		get { return idComparativa; }
		set
		{
			idComparativa = value;
		}
	}
	
	public int IdOrdenCompraEncabezado
	{
		get { return idOrdenCompraEncabezado; }
		set
		{
			idOrdenCompraEncabezado = value;
		}
	}
	
	public string Proveedor1
	{
		get { return proveedor1; }
		set
		{
			proveedor1 = value;
		}
	}
	
	public string Proveedor2
	{
		get { return proveedor2; }
		set
		{
			proveedor2 = value;
		}
	}
	
	public string Proveedor3
	{
		get { return proveedor3; }
		set
		{
			proveedor3 = value;
		}
	}
	
	public int IdProveedor1
	{
		get { return idProveedor1; }
		set
		{
			idProveedor1 = value;
		}
	}
	
	public int IdProveedor2
	{
		get { return idProveedor2; }
		set
		{
			idProveedor2 = value;
		}
	}
	
	public int IdProveedor3
	{
		get { return idProveedor3; }
		set
		{
			idProveedor3 = value;
		}
	}
	
	public decimal Costo1
	{
		get { return costo1; }
		set
		{
			costo1 = value;
		}
	}
	
	public decimal Costo2
	{
		get { return costo2; }
		set
		{
			costo2 = value;
		}
	}
	
	public decimal Costo3
	{
		get { return costo3; }
		set
		{
			costo3 = value;
		}
	}
	
	public int DiasCredito1
	{
		get { return diasCredito1; }
		set
		{
			diasCredito1 = value;
		}
	}
	
	public int DiasCredito2
	{
		get { return diasCredito2; }
		set
		{
			diasCredito2 = value;
		}
	}
	
	public int DiasCredito3
	{
		get { return diasCredito3; }
		set
		{
			diasCredito3 = value;
		}
	}
	
	public int DiasEntrega1
	{
		get { return diasEntrega1; }
		set
		{
			diasEntrega1 = value;
		}
	}
	
	public int DiasEntrega2
	{
		get { return diasEntrega2; }
		set
		{
			diasEntrega2 = value;
		}
	}
	
	public int DiasEntrega3
	{
		get { return diasEntrega3; }
		set
		{
			diasEntrega3 = value;
		}
	}
	
	public decimal Margen1
	{
		get { return margen1; }
		set
		{
			margen1 = value;
		}
	}
	
	public decimal Margen2
	{
		get { return margen2; }
		set
		{
			margen2 = value;
		}
	}
	
	public decimal Margen3
	{
		get { return margen3; }
		set
		{
			margen3 = value;
		}
	}
	
	public int IdUsuarioModificacion
	{
		get { return idUsuarioModificacion; }
		set
		{
			idUsuarioModificacion = value;
		}
	}
	
	public DateTime FechaModificacion
	{
		get { return fechaModificacion; }
		set { fechaModificacion = value; }
	}
	
	//Constructores
	public CComparativa()
	{
		idComparativa = 0;
		idOrdenCompraEncabezado = 0;
		proveedor1 = "";
		proveedor2 = "";
		proveedor3 = "";
		idProveedor1 = 0;
		idProveedor2 = 0;
		idProveedor3 = 0;
		costo1 = 0;
		costo2 = 0;
		costo3 = 0;
		diasCredito1 = 0;
		diasCredito2 = 0;
		diasCredito3 = 0;
		diasEntrega1 = 0;
		diasEntrega2 = 0;
		diasEntrega3 = 0;
		margen1 = 0;
		margen2 = 0;
		margen3 = 0;
		idUsuarioModificacion = 0;
		fechaModificacion = new DateTime(1, 1, 1);
	}
	
	public CComparativa(int pIdComparativa)
	{
		idComparativa = pIdComparativa;
		idOrdenCompraEncabezado = 0;
		proveedor1 = "";
		proveedor2 = "";
		proveedor3 = "";
		idProveedor1 = 0;
		idProveedor2 = 0;
		idProveedor3 = 0;
		costo1 = 0;
		costo2 = 0;
		costo3 = 0;
		diasCredito1 = 0;
		diasCredito2 = 0;
		diasCredito3 = 0;
		diasEntrega1 = 0;
		diasEntrega2 = 0;
		diasEntrega3 = 0;
		margen1 = 0;
		margen2 = 0;
		margen3 = 0;
		idUsuarioModificacion = 0;
		fechaModificacion = new DateTime(1, 1, 1);
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comparativa_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CComparativa>(typeof(CComparativa), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comparativa_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CComparativa>(typeof(CComparativa), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comparativa_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdComparativa", pIdentificador);
		Obten.Llena<CComparativa>(typeof(CComparativa), pConexion);
		foreach (CComparativa O in Obten.ListaRegistros)
		{
			idComparativa = O.IdComparativa;
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			proveedor1 = O.Proveedor1;
			proveedor2 = O.Proveedor2;
			proveedor3 = O.Proveedor3;
			idProveedor1 = O.IdProveedor1;
			idProveedor2 = O.IdProveedor2;
			idProveedor3 = O.IdProveedor3;
			costo1 = O.Costo1;
			costo2 = O.Costo2;
			costo3 = O.Costo3;
			diasCredito1 = O.DiasCredito1;
			diasCredito2 = O.DiasCredito2;
			diasCredito3 = O.DiasCredito3;
			diasEntrega1 = O.DiasEntrega1;
			diasEntrega2 = O.DiasEntrega2;
			diasEntrega3 = O.DiasEntrega3;
			margen1 = O.Margen1;
			margen2 = O.Margen2;
			margen3 = O.Margen3;
			idUsuarioModificacion = O.IdUsuarioModificacion;
			fechaModificacion = O.FechaModificacion;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comparativa_ConsultarFiltros";
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
		Obten.Llena<CComparativa>(typeof(CComparativa), pConexion);
		foreach (CComparativa O in Obten.ListaRegistros)
		{
			idComparativa = O.IdComparativa;
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			proveedor1 = O.Proveedor1;
			proveedor2 = O.Proveedor2;
			proveedor3 = O.Proveedor3;
			idProveedor1 = O.IdProveedor1;
			idProveedor2 = O.IdProveedor2;
			idProveedor3 = O.IdProveedor3;
			costo1 = O.Costo1;
			costo2 = O.Costo2;
			costo3 = O.Costo3;
			diasCredito1 = O.DiasCredito1;
			diasCredito2 = O.DiasCredito2;
			diasCredito3 = O.DiasCredito3;
			diasEntrega1 = O.DiasEntrega1;
			diasEntrega2 = O.DiasEntrega2;
			diasEntrega3 = O.DiasEntrega3;
			margen1 = O.Margen1;
			margen2 = O.Margen2;
			margen3 = O.Margen3;
			idUsuarioModificacion = O.IdUsuarioModificacion;
			fechaModificacion = O.FechaModificacion;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comparativa_ConsultarFiltros";
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
		Obten.Llena<CComparativa>(typeof(CComparativa), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Comparativa_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdComparativa", 0);
		Agregar.StoredProcedure.Parameters["@pIdComparativa"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProveedor1", proveedor1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProveedor2", proveedor2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pProveedor3", proveedor3);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor1", idProveedor1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor2", idProveedor2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor3", idProveedor3);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto1", costo1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto2", costo2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto3", costo3);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDiasCredito1", diasCredito1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDiasCredito2", diasCredito2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDiasCredito3", diasCredito3);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDiasEntrega1", diasEntrega1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDiasEntrega2", diasEntrega2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDiasEntrega3", diasEntrega3);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargen1", margen1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargen2", margen2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargen3", margen3);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModificacion", idUsuarioModificacion);
		if(fechaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Agregar.Insert(pConexion);
		idComparativa= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdComparativa"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Comparativa_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdComparativa", idComparativa);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProveedor1", proveedor1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProveedor2", proveedor2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pProveedor3", proveedor3);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor1", idProveedor1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor2", idProveedor2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor3", idProveedor3);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto1", costo1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto2", costo2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto3", costo3);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDiasCredito1", diasCredito1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDiasCredito2", diasCredito2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDiasCredito3", diasCredito3);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDiasEntrega1", diasEntrega1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDiasEntrega2", diasEntrega2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDiasEntrega3", diasEntrega3);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMargen1", margen1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMargen2", margen2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMargen3", margen3);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModificacion", idUsuarioModificacion);
		if(fechaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Comparativa_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdComparativa", idComparativa);
		Eliminar.Delete(pConexion);
	}
}
