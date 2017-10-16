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

public partial class CProductoEnrolado
{
	//Propiedades Privadas
	private int idProductoEnrolado;
	private int idProducto;
	private int idEmpresa;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private int idUsuarioModifico;
	private DateTime fechaUltimaModificacion;
	private bool baja;
	
	//Propiedades
	public int IdProductoEnrolado
	{
		get { return idProductoEnrolado; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProductoEnrolado = value;
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
	
	public int IdEmpresa
	{
		get { return idEmpresa; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEmpresa = value;
		}
	}
	
	public int IdUsuarioAlta
	{
		get { return idUsuarioAlta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioAlta = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public int IdUsuarioModifico
	{
		get { return idUsuarioModifico; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioModifico = value;
		}
	}
	
	public DateTime FechaUltimaModificacion
	{
		get { return fechaUltimaModificacion; }
		set { fechaUltimaModificacion = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CProductoEnrolado()
	{
		idProductoEnrolado = 0;
		idProducto = 0;
		idEmpresa = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioModifico = 0;
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CProductoEnrolado(int pIdProductoEnrolado)
	{
		idProductoEnrolado = pIdProductoEnrolado;
		idProducto = 0;
		idEmpresa = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioModifico = 0;
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProductoEnrolado_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProductoEnrolado>(typeof(CProductoEnrolado), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProductoEnrolado_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CProductoEnrolado>(typeof(CProductoEnrolado), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProductoEnrolado_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdProductoEnrolado", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProductoEnrolado>(typeof(CProductoEnrolado), pConexion);
		foreach (CProductoEnrolado O in Obten.ListaRegistros)
		{
			idProductoEnrolado = O.IdProductoEnrolado;
			idProducto = O.IdProducto;
			idEmpresa = O.IdEmpresa;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProductoEnrolado_ConsultarFiltros";
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
		Obten.Llena<CProductoEnrolado>(typeof(CProductoEnrolado), pConexion);
		foreach (CProductoEnrolado O in Obten.ListaRegistros)
		{
			idProductoEnrolado = O.IdProductoEnrolado;
			idProducto = O.IdProducto;
			idEmpresa = O.IdEmpresa;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ProductoEnrolado_ConsultarFiltros";
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
		Obten.Llena<CProductoEnrolado>(typeof(CProductoEnrolado), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ProductoEnrolado_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProductoEnrolado", 0);
		Agregar.StoredProcedure.Parameters["@pIdProductoEnrolado"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		if(fechaUltimaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idProductoEnrolado= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProductoEnrolado"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ProductoEnrolado_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProductoEnrolado", idProductoEnrolado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		if(fechaUltimaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ProductoEnrolado_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProductoEnrolado", idProductoEnrolado);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}