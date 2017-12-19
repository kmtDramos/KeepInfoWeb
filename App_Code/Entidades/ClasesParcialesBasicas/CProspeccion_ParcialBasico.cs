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

public partial class CProspeccion
{
	//Propiedades Privadas
	private int idProspeccion;
	private int idUsuario;
	private string cliente;
	private int idEstatusProspeccion;
	private DateTime fechaAlta;
	private DateTime fechaModificacion;
	private int idEstatusProspeccionUsuario;
	private string correo;
	private string telefono;
	private bool baja;
	
	//Propiedades
	public int IdProspeccion
	{
		get { return idProspeccion; }
		set
		{
			idProspeccion = value;
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
	
	public string Cliente
	{
		get { return cliente; }
		set
		{
			cliente = value;
		}
	}
	
	public int IdEstatusProspeccion
	{
		get { return idEstatusProspeccion; }
		set
		{
			idEstatusProspeccion = value;
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
	
	public int IdEstatusProspeccionUsuario
	{
		get { return idEstatusProspeccionUsuario; }
		set
		{
			idEstatusProspeccionUsuario = value;
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
	
	public string Telefono
	{
		get { return telefono; }
		set
		{
			telefono = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CProspeccion()
	{
		idProspeccion = 0;
		idUsuario = 0;
		cliente = "";
		idEstatusProspeccion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idEstatusProspeccionUsuario = 0;
		correo = "";
		telefono = "";
		baja = false;
	}
	
	public CProspeccion(int pIdProspeccion)
	{
		idProspeccion = pIdProspeccion;
		idUsuario = 0;
		cliente = "";
		idEstatusProspeccion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idEstatusProspeccionUsuario = 0;
		correo = "";
		telefono = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Prospeccion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProspeccion>(typeof(CProspeccion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Prospeccion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CProspeccion>(typeof(CProspeccion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Prospeccion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdProspeccion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CProspeccion>(typeof(CProspeccion), pConexion);
		foreach (CProspeccion O in Obten.ListaRegistros)
		{
			idProspeccion = O.IdProspeccion;
			idUsuario = O.IdUsuario;
			cliente = O.Cliente;
			idEstatusProspeccion = O.IdEstatusProspeccion;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idEstatusProspeccionUsuario = O.IdEstatusProspeccionUsuario;
			correo = O.Correo;
			telefono = O.Telefono;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Prospeccion_ConsultarFiltros";
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
		Obten.Llena<CProspeccion>(typeof(CProspeccion), pConexion);
		foreach (CProspeccion O in Obten.ListaRegistros)
		{
			idProspeccion = O.IdProspeccion;
			idUsuario = O.IdUsuario;
			cliente = O.Cliente;
			idEstatusProspeccion = O.IdEstatusProspeccion;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idEstatusProspeccionUsuario = O.IdEstatusProspeccionUsuario;
			correo = O.Correo;
			telefono = O.Telefono;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Prospeccion_ConsultarFiltros";
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
		Obten.Llena<CProspeccion>(typeof(CProspeccion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Prospeccion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProspeccion", 0);
		Agregar.StoredProcedure.Parameters["@pIdProspeccion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCliente", cliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", idEstatusProspeccion);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccionUsuario", idEstatusProspeccionUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idProspeccion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProspeccion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Prospeccion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProspeccion", idProspeccion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCliente", cliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", idEstatusProspeccion);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccionUsuario", idEstatusProspeccionUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Prospeccion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProspeccion", idProspeccion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
