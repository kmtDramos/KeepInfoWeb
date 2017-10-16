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

public partial class CFacturaEncabezadoSucursal
{
	//Propiedades Privadas
	private int idFacturaEncabezadoSucursal;
	private DateTime fechaAlta;
	private DateTime fechaUltimaModificacion;
	private int idFacturaEncabezado;
	private int idSucursal;
	private int idUsuarioAlta;
	private int idUsuarioModifico;
	private bool baja;
	
	//Propiedades
	public int IdFacturaEncabezadoSucursal
	{
		get { return idFacturaEncabezadoSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idFacturaEncabezadoSucursal = value;
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
	
	public int IdFacturaEncabezado
	{
		get { return idFacturaEncabezado; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idFacturaEncabezado = value;
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
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CFacturaEncabezadoSucursal()
	{
		idFacturaEncabezadoSucursal = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idFacturaEncabezado = 0;
		idSucursal = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		baja = false;
	}
	
	public CFacturaEncabezadoSucursal(int pIdFacturaEncabezadoSucursal)
	{
		idFacturaEncabezadoSucursal = pIdFacturaEncabezadoSucursal;
		fechaAlta = new DateTime(1, 1, 1);
		fechaUltimaModificacion = new DateTime(1, 1, 1);
		idFacturaEncabezado = 0;
		idSucursal = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFacturaEncabezadoSucursal>(typeof(CFacturaEncabezadoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CFacturaEncabezadoSucursal>(typeof(CFacturaEncabezadoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSucursal", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CFacturaEncabezadoSucursal>(typeof(CFacturaEncabezadoSucursal), pConexion);
		foreach (CFacturaEncabezadoSucursal O in Obten.ListaRegistros)
		{
			idFacturaEncabezadoSucursal = O.IdFacturaEncabezadoSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idFacturaEncabezado = O.IdFacturaEncabezado;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_ConsultarFiltros";
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
		Obten.Llena<CFacturaEncabezadoSucursal>(typeof(CFacturaEncabezadoSucursal), pConexion);
		foreach (CFacturaEncabezadoSucursal O in Obten.ListaRegistros)
		{
			idFacturaEncabezadoSucursal = O.IdFacturaEncabezadoSucursal;
			fechaAlta = O.FechaAlta;
			fechaUltimaModificacion = O.FechaUltimaModificacion;
			idFacturaEncabezado = O.IdFacturaEncabezado;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_ConsultarFiltros";
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
		Obten.Llena<CFacturaEncabezadoSucursal>(typeof(CFacturaEncabezadoSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSucursal", 0);
		Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezadoSucursal"].Direction = ParameterDirection.Output;
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idFacturaEncabezadoSucursal= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezadoSucursal"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSucursal", idFacturaEncabezadoSucursal);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaUltimaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_FacturaEncabezadoSucursal_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezadoSucursal", idFacturaEncabezadoSucursal);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}