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

public partial class CExistenciaReal
{
	//Propiedades Privadas
	private int idExistenciaReal;
	private int idProducto;
	private DateTime fecha;
	private int cantidadInicial;
	private int cantidadFinal;
	private int idUsuario;
	private decimal costo;
	private int idAlmacen;
	private int idSucursal;
	
	//Propiedades
	public int IdExistenciaReal
	{
		get { return idExistenciaReal; }
		set
		{
			idExistenciaReal = value;
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
	
	public DateTime Fecha
	{
		get { return fecha; }
		set { fecha = value; }
	}
	
	public int CantidadInicial
	{
		get { return cantidadInicial; }
		set
		{
			cantidadInicial = value;
		}
	}
	
	public int CantidadFinal
	{
		get { return cantidadFinal; }
		set
		{
			cantidadFinal = value;
		}
	}
	
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public decimal Costo
	{
		get { return costo; }
		set
		{
			costo = value;
		}
	}
	
	public int IdAlmacen
	{
		get { return idAlmacen; }
		set
		{
			idAlmacen = value;
		}
	}
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			idSucursal = value;
		}
	}
	
	//Constructores
	public CExistenciaReal()
	{
		idExistenciaReal = 0;
		idProducto = 0;
		fecha = new DateTime(1, 1, 1);
		cantidadInicial = 0;
		cantidadFinal = 0;
		idUsuario = 0;
		costo = 0;
		idAlmacen = 0;
		idSucursal = 0;
	}
	
	public CExistenciaReal(int pIdExistenciaReal)
	{
		idExistenciaReal = pIdExistenciaReal;
		idProducto = 0;
		fecha = new DateTime(1, 1, 1);
		cantidadInicial = 0;
		cantidadFinal = 0;
		idUsuario = 0;
		costo = 0;
		idAlmacen = 0;
		idSucursal = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaReal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CExistenciaReal>(typeof(CExistenciaReal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaReal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CExistenciaReal>(typeof(CExistenciaReal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaReal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaReal", pIdentificador);
		Obten.Llena<CExistenciaReal>(typeof(CExistenciaReal), pConexion);
		foreach (CExistenciaReal O in Obten.ListaRegistros)
		{
			idExistenciaReal = O.IdExistenciaReal;
			idProducto = O.IdProducto;
			fecha = O.Fecha;
			cantidadInicial = O.CantidadInicial;
			cantidadFinal = O.CantidadFinal;
			idUsuario = O.IdUsuario;
			costo = O.Costo;
			idAlmacen = O.IdAlmacen;
			idSucursal = O.IdSucursal;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaReal_ConsultarFiltros";
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
		Obten.Llena<CExistenciaReal>(typeof(CExistenciaReal), pConexion);
		foreach (CExistenciaReal O in Obten.ListaRegistros)
		{
			idExistenciaReal = O.IdExistenciaReal;
			idProducto = O.IdProducto;
			fecha = O.Fecha;
			cantidadInicial = O.CantidadInicial;
			cantidadFinal = O.CantidadFinal;
			idUsuario = O.IdUsuario;
			costo = O.Costo;
			idAlmacen = O.IdAlmacen;
			idSucursal = O.IdSucursal;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ExistenciaReal_ConsultarFiltros";
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
		Obten.Llena<CExistenciaReal>(typeof(CExistenciaReal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ExistenciaReal_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaReal", 0);
		Agregar.StoredProcedure.Parameters["@pIdExistenciaReal"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		if(fecha.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadInicial", cantidadInicial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadFinal", cantidadFinal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.Insert(pConexion);
		idExistenciaReal= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdExistenciaReal"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ExistenciaReal_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaReal", idExistenciaReal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
		if(fecha.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadInicial", cantidadInicial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidadFinal", cantidadFinal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ExistenciaReal_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdExistenciaReal", idExistenciaReal);
		Eliminar.Delete(pConexion);
	}
}
