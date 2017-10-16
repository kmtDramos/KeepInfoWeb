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

public partial class CNumeroCuentaCliente
{
	//Propiedades Privadas
	private int idNumeroCuentaCliente;
	private string numeroCuentaCliente;
	private int idCliente;
	private int idMetodoPago;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private int idTipoMoneda;
	private string descripcion;
	private bool baja;
	
	//Propiedades
	public int IdNumeroCuentaCliente
	{
		get { return idNumeroCuentaCliente; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idNumeroCuentaCliente = value;
		}
	}
	
	public string NumeroCuentaCliente
	{
		get { return numeroCuentaCliente; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			numeroCuentaCliente = value;
		}
	}
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idCliente = value;
		}
	}
	
	public int IdMetodoPago
	{
		get { return idMetodoPago; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idMetodoPago = value;
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
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			descripcion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CNumeroCuentaCliente()
	{
		idNumeroCuentaCliente = 0;
		numeroCuentaCliente = "";
		idCliente = 0;
		idMetodoPago = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idTipoMoneda = 0;
		descripcion = "";
		baja = false;
	}
	
	public CNumeroCuentaCliente(int pIdNumeroCuentaCliente)
	{
		idNumeroCuentaCliente = pIdNumeroCuentaCliente;
		numeroCuentaCliente = "";
		idCliente = 0;
		idMetodoPago = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idTipoMoneda = 0;
		descripcion = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNumeroCuentaCliente>(typeof(CNumeroCuentaCliente), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CNumeroCuentaCliente>(typeof(CNumeroCuentaCliente), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdNumeroCuentaCliente", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CNumeroCuentaCliente>(typeof(CNumeroCuentaCliente), pConexion);
		foreach (CNumeroCuentaCliente O in Obten.ListaRegistros)
		{
			idNumeroCuentaCliente = O.IdNumeroCuentaCliente;
			numeroCuentaCliente = O.NumeroCuentaCliente;
			idCliente = O.IdCliente;
			idMetodoPago = O.IdMetodoPago;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			idTipoMoneda = O.IdTipoMoneda;
			descripcion = O.Descripcion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_ConsultarFiltros";
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
		Obten.Llena<CNumeroCuentaCliente>(typeof(CNumeroCuentaCliente), pConexion);
		foreach (CNumeroCuentaCliente O in Obten.ListaRegistros)
		{
			idNumeroCuentaCliente = O.IdNumeroCuentaCliente;
			numeroCuentaCliente = O.NumeroCuentaCliente;
			idCliente = O.IdCliente;
			idMetodoPago = O.IdMetodoPago;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			idTipoMoneda = O.IdTipoMoneda;
			descripcion = O.Descripcion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_ConsultarFiltros";
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
		Obten.Llena<CNumeroCuentaCliente>(typeof(CNumeroCuentaCliente), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNumeroCuentaCliente", 0);
		Agregar.StoredProcedure.Parameters["@pIdNumeroCuentaCliente"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroCuentaCliente", numeroCuentaCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idNumeroCuentaCliente= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNumeroCuentaCliente"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNumeroCuentaCliente", idNumeroCuentaCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroCuentaCliente", numeroCuentaCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_NumeroCuentaCliente_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNumeroCuentaCliente", idNumeroCuentaCliente);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}