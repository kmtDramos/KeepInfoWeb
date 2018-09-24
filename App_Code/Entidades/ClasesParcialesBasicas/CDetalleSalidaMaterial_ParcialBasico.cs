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

public partial class CDetalleSalidaMaterial
{
	//Propiedades Privadas
	private int idDetalleSalidaMaterial;
	private string descripcion;
	private decimal cantidad;
	private int idSalidaMaterial;
	private int idProducto;
	private int idServicio;
	private DateTime fechaAlta;
	private bool baja;
	
	//Propiedades
	public int IdDetalleSalidaMaterial
	{
		get { return idDetalleSalidaMaterial; }
		set
		{
			idDetalleSalidaMaterial = value;
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
	
	public decimal Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
		}
	}
	
	public int IdSalidaMaterial
	{
		get { return idSalidaMaterial; }
		set
		{
			idSalidaMaterial = value;
		}
	}
	
	public int IdProducto
	{
		get { return idProducto; }
		set
		{
			idProducto = value;
		}
	}
	
	public int IdServicio
	{
		get { return idServicio; }
		set
		{
			idServicio = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CDetalleSalidaMaterial()
	{
		idDetalleSalidaMaterial = 0;
		descripcion = "";
		cantidad = 0;
		idSalidaMaterial = 0;
		idProducto = 0;
		idServicio = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CDetalleSalidaMaterial(int pIdDetalleSalidaMaterial)
	{
		idDetalleSalidaMaterial = pIdDetalleSalidaMaterial;
		descripcion = "";
		cantidad = 0;
		idSalidaMaterial = 0;
		idProducto = 0;
		idServicio = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleSalidaMaterial>(typeof(CDetalleSalidaMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDetalleSalidaMaterial>(typeof(CDetalleSalidaMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDetalleSalidaMaterial", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleSalidaMaterial>(typeof(CDetalleSalidaMaterial), pConexion);
		foreach (CDetalleSalidaMaterial O in Obten.ListaRegistros)
		{
			idDetalleSalidaMaterial = O.IdDetalleSalidaMaterial;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			idSalidaMaterial = O.IdSalidaMaterial;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_ConsultarFiltros";
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
		Obten.Llena<CDetalleSalidaMaterial>(typeof(CDetalleSalidaMaterial), pConexion);
		foreach (CDetalleSalidaMaterial O in Obten.ListaRegistros)
		{
			idDetalleSalidaMaterial = O.IdDetalleSalidaMaterial;
			descripcion = O.Descripcion;
			cantidad = O.Cantidad;
			idSalidaMaterial = O.IdSalidaMaterial;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_ConsultarFiltros";
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
		Obten.Llena<CDetalleSalidaMaterial>(typeof(CDetalleSalidaMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleSalidaMaterial", 0);
		Agregar.StoredProcedure.Parameters["@pIdDetalleSalidaMaterial"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSalidaMaterial", idSalidaMaterial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDetalleSalidaMaterial= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDetalleSalidaMaterial"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleSalidaMaterial", idDetalleSalidaMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSalidaMaterial", idSalidaMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_DetalleSalidaMaterial_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleSalidaMaterial", idDetalleSalidaMaterial);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
