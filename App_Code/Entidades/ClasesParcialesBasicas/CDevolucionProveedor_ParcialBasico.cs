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

public partial class CDevolucionProveedor
{
	//Propiedades Privadas
	private int idDevolucionProveedor;
	private int idNotaCreditoProveedor;
	private int idDetalleFacturaProveedor;
	private int idUsuarioEntrada;
	private DateTime fechaEntrada;
	private bool baja;
	
	//Propiedades
	public int IdDevolucionProveedor
	{
		get { return idDevolucionProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDevolucionProveedor = value;
		}
	}
	
	public int IdNotaCreditoProveedor
	{
		get { return idNotaCreditoProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNotaCreditoProveedor = value;
		}
	}
	
	public int IdDetalleFacturaProveedor
	{
		get { return idDetalleFacturaProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idDetalleFacturaProveedor = value;
		}
	}
	
	public int IdUsuarioEntrada
	{
		get { return idUsuarioEntrada; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioEntrada = value;
		}
	}
	
	public DateTime FechaEntrada
	{
		get { return fechaEntrada; }
		set { fechaEntrada = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CDevolucionProveedor()
	{
		idDevolucionProveedor = 0;
		idNotaCreditoProveedor = 0;
		idDetalleFacturaProveedor = 0;
		idUsuarioEntrada = 0;
		fechaEntrada = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CDevolucionProveedor(int pIdDevolucionProveedor)
	{
		idDevolucionProveedor = pIdDevolucionProveedor;
		idNotaCreditoProveedor = 0;
		idDetalleFacturaProveedor = 0;
		idUsuarioEntrada = 0;
		fechaEntrada = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DevolucionProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDevolucionProveedor>(typeof(CDevolucionProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DevolucionProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CDevolucionProveedor>(typeof(CDevolucionProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DevolucionProveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdDevolucionProveedor", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CDevolucionProveedor>(typeof(CDevolucionProveedor), pConexion);
		foreach (CDevolucionProveedor O in Obten.ListaRegistros)
		{
			idDevolucionProveedor = O.IdDevolucionProveedor;
			idNotaCreditoProveedor = O.IdNotaCreditoProveedor;
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idUsuarioEntrada = O.IdUsuarioEntrada;
			fechaEntrada = O.FechaEntrada;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DevolucionProveedor_ConsultarFiltros";
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
		Obten.Llena<CDevolucionProveedor>(typeof(CDevolucionProveedor), pConexion);
		foreach (CDevolucionProveedor O in Obten.ListaRegistros)
		{
			idDevolucionProveedor = O.IdDevolucionProveedor;
			idNotaCreditoProveedor = O.IdNotaCreditoProveedor;
			idDetalleFacturaProveedor = O.IdDetalleFacturaProveedor;
			idUsuarioEntrada = O.IdUsuarioEntrada;
			fechaEntrada = O.FechaEntrada;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_DevolucionProveedor_ConsultarFiltros";
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
		Obten.Llena<CDevolucionProveedor>(typeof(CDevolucionProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_DevolucionProveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucionProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdDevolucionProveedor"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", idUsuarioEntrada);
		if(fechaEntrada.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrada", fechaEntrada);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idDevolucionProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDevolucionProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_DevolucionProveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucionProveedor", idDevolucionProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", idUsuarioEntrada);
		if(fechaEntrada.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrada", fechaEntrada);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_DevolucionProveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucionProveedor", idDevolucionProveedor);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}