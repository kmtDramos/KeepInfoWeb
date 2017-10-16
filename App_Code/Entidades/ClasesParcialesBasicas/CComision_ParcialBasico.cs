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

public partial class CComision
{
	//Propiedades Privadas
	private int idComision;
	private int idUsuario;
	private decimal venta;
	private decimal costo;
	private decimal utilidad;
	private decimal margen;
	private decimal comision;
	private DateTime fechaCreacion;
	private int idUsuarioCreacion;
	private int idUsuarioAprovacion;
	private bool pagado;
	private DateTime fechaPagado;
	private int idPresupuesto;
	
	//Propiedades
	public int IdComision
	{
		get { return idComision; }
		set
		{
			idComision = value;
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
	
	public decimal Venta
	{
		get { return venta; }
		set
		{
			venta = value;
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
	
	public decimal Utilidad
	{
		get { return utilidad; }
		set
		{
			utilidad = value;
		}
	}
	
	public decimal Margen
	{
		get { return margen; }
		set
		{
			margen = value;
		}
	}
	
	public decimal Comision
	{
		get { return comision; }
		set
		{
			comision = value;
		}
	}
	
	public DateTime FechaCreacion
	{
		get { return fechaCreacion; }
		set { fechaCreacion = value; }
	}
	
	public int IdUsuarioCreacion
	{
		get { return idUsuarioCreacion; }
		set
		{
			idUsuarioCreacion = value;
		}
	}
	
	public int IdUsuarioAprovacion
	{
		get { return idUsuarioAprovacion; }
		set
		{
			idUsuarioAprovacion = value;
		}
	}
	
	public bool Pagado
	{
		get { return pagado; }
		set { pagado = value; }
	}
	
	public DateTime FechaPagado
	{
		get { return fechaPagado; }
		set { fechaPagado = value; }
	}
	
	public int IdPresupuesto
	{
		get { return idPresupuesto; }
		set
		{
			idPresupuesto = value;
		}
	}
	
	//Constructores
	public CComision()
	{
		idComision = 0;
		idUsuario = 0;
		venta = 0;
		costo = 0;
		utilidad = 0;
		margen = 0;
		comision = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		idUsuarioCreacion = 0;
		idUsuarioAprovacion = 0;
		pagado = false;
		fechaPagado = new DateTime(1, 1, 1);
		idPresupuesto = 0;
	}
	
	public CComision(int pIdComision)
	{
		idComision = pIdComision;
		idUsuario = 0;
		venta = 0;
		costo = 0;
		utilidad = 0;
		margen = 0;
		comision = 0;
		fechaCreacion = new DateTime(1, 1, 1);
		idUsuarioCreacion = 0;
		idUsuarioAprovacion = 0;
		pagado = false;
		fechaPagado = new DateTime(1, 1, 1);
		idPresupuesto = 0;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CComision>(typeof(CComision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CComision>(typeof(CComision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comision_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdComision", pIdentificador);
		Obten.Llena<CComision>(typeof(CComision), pConexion);
		foreach (CComision O in Obten.ListaRegistros)
		{
			idComision = O.IdComision;
			idUsuario = O.IdUsuario;
			venta = O.Venta;
			costo = O.Costo;
			utilidad = O.Utilidad;
			margen = O.Margen;
			comision = O.Comision;
			fechaCreacion = O.FechaCreacion;
			idUsuarioCreacion = O.IdUsuarioCreacion;
			idUsuarioAprovacion = O.IdUsuarioAprovacion;
			pagado = O.Pagado;
			fechaPagado = O.FechaPagado;
			idPresupuesto = O.IdPresupuesto;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comision_ConsultarFiltros";
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
		Obten.Llena<CComision>(typeof(CComision), pConexion);
		foreach (CComision O in Obten.ListaRegistros)
		{
			idComision = O.IdComision;
			idUsuario = O.IdUsuario;
			venta = O.Venta;
			costo = O.Costo;
			utilidad = O.Utilidad;
			margen = O.Margen;
			comision = O.Comision;
			fechaCreacion = O.FechaCreacion;
			idUsuarioCreacion = O.IdUsuarioCreacion;
			idUsuarioAprovacion = O.IdUsuarioAprovacion;
			pagado = O.Pagado;
			fechaPagado = O.FechaPagado;
			idPresupuesto = O.IdPresupuesto;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Comision_ConsultarFiltros";
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
		Obten.Llena<CComision>(typeof(CComision), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Comision_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdComision", 0);
		Agregar.StoredProcedure.Parameters["@pIdComision"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pVenta", venta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMargen", margen);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComision", comision);
		if(fechaCreacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreacion", idUsuarioCreacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAprovacion", idUsuarioAprovacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPagado", pagado);
		if(fechaPagado.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPagado", fechaPagado);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Agregar.Insert(pConexion);
		idComision= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdComision"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Comision_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdComision", idComision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pVenta", venta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUtilidad", utilidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMargen", margen);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComision", comision);
		if(fechaCreacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreacion", idUsuarioCreacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAprovacion", idUsuarioAprovacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPagado", pagado);
		if(fechaPagado.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPagado", fechaPagado);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Comision_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdComision", idComision);
		Eliminar.Delete(pConexion);
	}
}
