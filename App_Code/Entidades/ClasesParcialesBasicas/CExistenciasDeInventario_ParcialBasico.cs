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

public partial class CExistenciasDeInventario
{
	//Propiedades Privadas
	private int idExistenciasDeInventario;
	private DateTime fechaInicio;
	private int cantidadExistencia;
	private DateTime fechaFin;
	private string comentario;
	private int idProducto;
	private int idAlmacen;
	
	//Propiedades
	public int IdExistenciasDeInventario
	{
		get { return idExistenciasDeInventario; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idExistenciasDeInventario = value;
		}
	}
	
	public DateTime FechaInicio
	{
		get { return fechaInicio; }
		set { fechaInicio = value; }
	}
	
	public int CantidadExistencia
	{
		get { return cantidadExistencia; }
		set
		{
			if (value < 0)
			{
				return;
			}
			cantidadExistencia = value;
		}
	}
	
	public DateTime FechaFin
	{
		get { return fechaFin; }
		set { fechaFin = value; }
	}
	
	public string Comentario
	{
		get { return comentario; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			comentario = value;
		}
	}
	
	public int IdProducto
	{
		get { return idProducto; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProducto = value;
		}
	}
	
	public int IdAlmacen
	{
		get { return idAlmacen; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idAlmacen = value;
		}
	}
	
	//Constructores
	public CExistenciasDeInventario()
	{
		idExistenciasDeInventario = 0;
		fechaInicio = new DateTime(1, 1, 1);
		cantidadExistencia = 0;
		fechaFin = new DateTime(1, 1, 1);
		comentario = "";
		idProducto = 0;
		idAlmacen = 0;
	}
	
	public CExistenciasDeInventario(int pIdExistenciasDeInventario)
	{
		idExistenciasDeInventario = pIdExistenciasDeInventario;
		fechaInicio = new DateTime(1, 1, 1);
		cantidadExistencia = 0;
		fechaFin = new DateTime(1, 1, 1);
		comentario = "";
		idProducto = 0;
		idAlmacen = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CExistenciasDeInventario>(typeof(CExistenciasDeInventario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CExistenciasDeInventario>(typeof(CExistenciasDeInventario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdExistenciasDeInventario", pIdentificador);
		Obten.Llena<CExistenciasDeInventario>(typeof(CExistenciasDeInventario), pConexion);
		foreach (CExistenciasDeInventario O in Obten.ListaRegistros)
		{
			idExistenciasDeInventario = O.IdExistenciasDeInventario;
			fechaInicio = O.FechaInicio;
			cantidadExistencia = O.CantidadExistencia;
			fechaFin = O.FechaFin;
			comentario = O.Comentario;
			idProducto = O.IdProducto;
			idAlmacen = O.IdAlmacen;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_ConsultarFiltros";
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
		Obten.Llena<CExistenciasDeInventario>(typeof(CExistenciasDeInventario), pConexion);
		foreach (CExistenciasDeInventario O in Obten.ListaRegistros)
		{
			idExistenciasDeInventario = O.IdExistenciasDeInventario;
			fechaInicio = O.FechaInicio;
			cantidadExistencia = O.CantidadExistencia;
			fechaFin = O.FechaFin;
			comentario = O.Comentario;
			idProducto = O.IdProducto;
			idAlmacen = O.IdAlmacen;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_ConsultarFiltros";
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
		Obten.Llena<CExistenciasDeInventario>(typeof(CExistenciasDeInventario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciasDeInventario", 0);
		Agregar.StoredProcedure.Parameters["@pIdExistenciasDeInventario"].Direction = ParameterDirection.Output;
		if(fechaInicio.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadExistencia", cantidadExistencia);
		if(fechaFin.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComentario", comentario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Agregar.Insert(pConexion);
		idExistenciasDeInventario= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdExistenciasDeInventario"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciasDeInventario", idExistenciasDeInventario);
		if(fechaInicio.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaInicio", fechaInicio);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadExistencia", cantidadExistencia);
		if(fechaFin.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaFin", fechaFin);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pComentario", comentario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ExistenciasDeInventario_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciasDeInventario", idExistenciasDeInventario);
		Eliminar.Delete(pConexion);
	}
}