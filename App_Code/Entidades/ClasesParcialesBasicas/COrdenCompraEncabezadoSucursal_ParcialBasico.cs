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

public partial class COrdenCompraEncabezadoSucursal
{
	//Propiedades Privadas
	private int idOrdenCompraEncabezadoSucursal;
	private int idOrdenCompraEncabezado;
	private int idSucursal;
	private DateTime fechaAlta;
	private DateTime fechaUltimaModificacion;
	private int idUsuarioAlta;
	private int idUsuarioModifico;
	
	//Propiedades
	public int IdOrdenCompraEncabezadoSucursal
	{
		get { return idOrdenCompraEncabezadoSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOrdenCompraEncabezadoSucursal = value;
		}
	}
	
	public int IdOrdenCompraEncabezado
	{
		get { return idOrdenCompraEncabezado; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOrdenCompraEncabezado = value;
		}
	}
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursal = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaUltimaModificacion
	{
		get { return fechaUltimaModificacion; }
		set { fechaUltimaModificacion = value; }
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
	
	//Constructores
	public COrdenCompraEncabezadoSucursal()
	{
		idOrdenCompraEncabezadoSucursal = 0;
		idOrdenCompraEncabezado = 0;
		idSucursal = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
	}
	
	public COrdenCompraEncabezadoSucursal(int pIdOrdenCompraEncabezadoSucursal)
	{
		idOrdenCompraEncabezadoSucursal = pIdOrdenCompraEncabezadoSucursal;
		idOrdenCompraEncabezado = 0;
		idSucursal = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<COrdenCompraEncabezadoSucursal>(typeof(COrdenCompraEncabezadoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COrdenCompraEncabezadoSucursal>(typeof(COrdenCompraEncabezadoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezadoSucursal", pIdentificador);
		Obten.Llena<COrdenCompraEncabezadoSucursal>(typeof(COrdenCompraEncabezadoSucursal), pConexion);
		foreach (COrdenCompraEncabezadoSucursal O in Obten.ListaRegistros)
		{
			idOrdenCompraEncabezadoSucursal = O.IdOrdenCompraEncabezadoSucursal;
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			idSucursal = O.IdSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_ConsultarFiltros";
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
		Obten.Llena<COrdenCompraEncabezadoSucursal>(typeof(COrdenCompraEncabezadoSucursal), pConexion);
		foreach (COrdenCompraEncabezadoSucursal O in Obten.ListaRegistros)
		{
			idOrdenCompraEncabezadoSucursal = O.IdOrdenCompraEncabezadoSucursal;
			idOrdenCompraEncabezado = O.IdOrdenCompraEncabezado;
			idSucursal = O.IdSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_ConsultarFiltros";
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
		Obten.Llena<COrdenCompraEncabezadoSucursal>(typeof(COrdenCompraEncabezadoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezadoSucursal", 0);
		Agregar.StoredProcedure.Parameters["@pIdOrdenCompraEncabezadoSucursal"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Agregar.Insert(pConexion);
		idOrdenCompraEncabezadoSucursal= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrdenCompraEncabezadoSucursal"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezadoSucursal", idOrdenCompraEncabezadoSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_OrdenCompraEncabezadoSucursal_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezadoSucursal", idOrdenCompraEncabezadoSucursal);
		Eliminar.Delete(pConexion);
	}
}