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

public partial class CProveedor
{
	//Propiedades Privadas
	private int idProveedor;
	private DateTime fechaAlta;
	private DateTime fechaModificacion;
	private int idTipoMoneda;
	private int idCondicionPago;
	private int idOrganizacion;
	private int idUsuarioAlta;
	private int idUsuarioModifico;
	private decimal iVAActual;
	private string cuentaContable;
	private string correo;
	private string limiteCredito;
	private int idTipoGarantia;
	private string cuentaContableDolares;
	private bool baja;
	
	//Propiedades
	public int IdProveedor
	{
		get { return idProveedor; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idProveedor = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaModificacion
	{
		get { return fechaModificacion; }
		set { fechaModificacion = value; }
	}
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoMoneda = value;
		}
	}
	
	public int IdCondicionPago
	{
		get { return idCondicionPago; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCondicionPago = value;
		}
	}
	
	public int IdOrganizacion
	{
		get { return idOrganizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOrganizacion = value;
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
	
	public decimal IVAActual
	{
		get { return iVAActual; }
		set
		{
			if (value < 0)
			{
				return;
			}
			iVAActual = value;
		}
	}
	
	public string CuentaContable
	{
		get { return cuentaContable; }
		set
		{
			cuentaContable = value;
		}
	}
	
	public string Correo
	{
		get { return correo; }
		set
		{
			correo = value;
		}
	}
	
	public string LimiteCredito
	{
		get { return limiteCredito; }
		set
		{
			limiteCredito = value;
		}
	}
	
	public int IdTipoGarantia
	{
		get { return idTipoGarantia; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoGarantia = value;
		}
	}
	
	public string CuentaContableDolares
	{
		get { return cuentaContableDolares; }
		set
		{
			cuentaContableDolares = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CProveedor()
	{
		idProveedor = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idTipoMoneda = 0;
		idCondicionPago = 0;
		idOrganizacion = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		iVAActual = 0;
		cuentaContable = "";
		correo = "";
		limiteCredito = "";
		idTipoGarantia = 0;
		cuentaContableDolares = "";
		baja = false;
	}
	
	public CProveedor(int pIdProveedor)
	{
		idProveedor = pIdProveedor;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idTipoMoneda = 0;
		idCondicionPago = 0;
		idOrganizacion = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		iVAActual = 0;
		cuentaContable = "";
		correo = "";
		limiteCredito = "";
		idTipoGarantia = 0;
		cuentaContableDolares = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProveedor>(typeof(CProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CProveedor>(typeof(CProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proveedor_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProveedor>(typeof(CProveedor), pConexion);
		foreach (CProveedor O in Obten.ListaRegistros)
		{
			idProveedor = O.IdProveedor;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idTipoMoneda = O.IdTipoMoneda;
			idCondicionPago = O.IdCondicionPago;
			idOrganizacion = O.IdOrganizacion;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			iVAActual = O.IVAActual;
			cuentaContable = O.CuentaContable;
			correo = O.Correo;
			limiteCredito = O.LimiteCredito;
			idTipoGarantia = O.IdTipoGarantia;
			cuentaContableDolares = O.CuentaContableDolares;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proveedor_ConsultarFiltros";
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
		Obten.Llena<CProveedor>(typeof(CProveedor), pConexion);
		foreach (CProveedor O in Obten.ListaRegistros)
		{
			idProveedor = O.IdProveedor;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idTipoMoneda = O.IdTipoMoneda;
			idCondicionPago = O.IdCondicionPago;
			idOrganizacion = O.IdOrganizacion;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			iVAActual = O.IVAActual;
			cuentaContable = O.CuentaContable;
			correo = O.Correo;
			limiteCredito = O.LimiteCredito;
			idTipoGarantia = O.IdTipoGarantia;
			cuentaContableDolares = O.CuentaContableDolares;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Proveedor_ConsultarFiltros";
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
		Obten.Llena<CProveedor>(typeof(CProveedor), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Proveedor_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", 0);
		Agregar.StoredProcedure.Parameters["@pIdProveedor"].Direction = ParameterDirection.Output;
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVAActual", iVAActual);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLimiteCredito", limiteCredito);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGarantia", idTipoGarantia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableDolares", cuentaContableDolares);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idProveedor= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProveedor"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Proveedor_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVAActual", iVAActual);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLimiteCredito", limiteCredito);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGarantia", idTipoGarantia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContableDolares", cuentaContableDolares);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Proveedor_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}