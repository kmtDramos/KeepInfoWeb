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

public partial class CDetalleReingresoMaterial
{
	//Propiedades Privadas
	private int idDetalleReingresoMaterial;
	private string descripcion;
	private int idReingresoMaterial;
	private int idProducto;
	private int idServicio;
	private int descripcionError;
	private int cantidadERROR;
	private DateTime fechaAlta;
	private decimal cantidad;
	private bool baja;
	
	//Propiedades
	public int IdDetalleReingresoMaterial
	{
		get { return idDetalleReingresoMaterial; }
		set
		{
			idDetalleReingresoMaterial = value;
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
	
	public int IdReingresoMaterial
	{
		get { return idReingresoMaterial; }
		set
		{
			idReingresoMaterial = value;
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
	
	public int DescripcionError
	{
		get { return descripcionError; }
		set
		{
			descripcionError = value;
		}
	}
	
	public int CantidadERROR
	{
		get { return cantidadERROR; }
		set
		{
			cantidadERROR = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public decimal Cantidad
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
	public CDetalleReingresoMaterial()
	{
		idDetalleReingresoMaterial = 0;
		descripcion = "";
		idReingresoMaterial = 0;
		idProducto = 0;
		idServicio = 0;
		descripcionError = 0;
		cantidadERROR = 0;
		fechaAlta = new DateTime(1, 1, 1);
		cantidad = 0;
		baja = false;
	}
	
	public CDetalleReingresoMaterial(int pIdDetalleReingresoMaterial)
	{
		idDetalleReingresoMaterial = pIdDetalleReingresoMaterial;
		descripcion = "";
		idReingresoMaterial = 0;
		idProducto = 0;
		idServicio = 0;
		descripcionError = 0;
		cantidadERROR = 0;
		fechaAlta = new DateTime(1, 1, 1);
		cantidad = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleReingresoMaterial>(typeof(CDetalleReingresoMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDetalleReingresoMaterial>(typeof(CDetalleReingresoMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDetalleReingresoMaterial", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDetalleReingresoMaterial>(typeof(CDetalleReingresoMaterial), pConexion);
		foreach (CDetalleReingresoMaterial O in Obten.ListaRegistros)
		{
			idDetalleReingresoMaterial = O.IdDetalleReingresoMaterial;
			descripcion = O.Descripcion;
			idReingresoMaterial = O.IdReingresoMaterial;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			descripcionError = O.DescripcionError;
			cantidadERROR = O.CantidadERROR;
			fechaAlta = O.FechaAlta;
			cantidad = O.Cantidad;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_ConsultarFiltros";
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
		Obten.Llena<CDetalleReingresoMaterial>(typeof(CDetalleReingresoMaterial), pConexion);
		foreach (CDetalleReingresoMaterial O in Obten.ListaRegistros)
		{
			idDetalleReingresoMaterial = O.IdDetalleReingresoMaterial;
			descripcion = O.Descripcion;
			idReingresoMaterial = O.IdReingresoMaterial;
			idProducto = O.IdProducto;
			idServicio = O.IdServicio;
			descripcionError = O.DescripcionError;
			cantidadERROR = O.CantidadERROR;
			fechaAlta = O.FechaAlta;
			cantidad = O.Cantidad;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_ConsultarFiltros";
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
		Obten.Llena<CDetalleReingresoMaterial>(typeof(CDetalleReingresoMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleReingresoMaterial", 0);
		Agregar.StoredProcedure.Parameters["@pIdDetalleReingresoMaterial"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdReingresoMaterial", idReingresoMaterial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcionError", descripcionError);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadERROR", cantidadERROR);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDetalleReingresoMaterial= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDetalleReingresoMaterial"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleReingresoMaterial", idDetalleReingresoMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdReingresoMaterial", idReingresoMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcionError", descripcionError);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadERROR", cantidadERROR);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_DetalleReingresoMaterial_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleReingresoMaterial", idDetalleReingresoMaterial);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
