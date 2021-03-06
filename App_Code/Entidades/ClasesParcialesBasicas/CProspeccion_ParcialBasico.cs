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
	private string correo;
	private string telefono;
	private string nota;
	private string nombre;
	private int idUsuario;
	private string cliente;
	private int idEstatusProspeccion;
	private DateTime fechaAlta;
	private DateTime fechaModificacion;
	private int idEstatusProspeccionUsuario;
	private int idDivision;
	private int idNivelInteresProspeccion;
	private int idLevantamiento;
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
	
	public string Nota
	{
		get { return nota; }
		set
		{
			nota = value;
		}
	}
	
	public string Nombre
	{
		get { return nombre; }
		set
		{
			nombre = value;
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
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
		}
	}
	
	public int IdNivelInteresProspeccion
	{
		get { return idNivelInteresProspeccion; }
		set
		{
			idNivelInteresProspeccion = value;
		}
	}
	
	public int IdLevantamiento
	{
		get { return idLevantamiento; }
		set
		{
			idLevantamiento = value;
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
		correo = "";
		telefono = "";
		nota = "";
		nombre = "";
		idUsuario = 0;
		cliente = "";
		idEstatusProspeccion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idEstatusProspeccionUsuario = 0;
		idDivision = 0;
		idNivelInteresProspeccion = 0;
		idLevantamiento = 0;
		baja = false;
	}
	
	public CProspeccion(int pIdProspeccion)
	{
		idProspeccion = pIdProspeccion;
		correo = "";
		telefono = "";
		nota = "";
		nombre = "";
		idUsuario = 0;
		cliente = "";
		idEstatusProspeccion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		idEstatusProspeccionUsuario = 0;
		idDivision = 0;
		idNivelInteresProspeccion = 0;
		idLevantamiento = 0;
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
			correo = O.Correo;
			telefono = O.Telefono;
			nota = O.Nota;
			nombre = O.Nombre;
			idUsuario = O.IdUsuario;
			cliente = O.Cliente;
			idEstatusProspeccion = O.IdEstatusProspeccion;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idEstatusProspeccionUsuario = O.IdEstatusProspeccionUsuario;
			idDivision = O.IdDivision;
			idNivelInteresProspeccion = O.IdNivelInteresProspeccion;
			idLevantamiento = O.IdLevantamiento;
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
			correo = O.Correo;
			telefono = O.Telefono;
			nota = O.Nota;
			nombre = O.Nombre;
			idUsuario = O.IdUsuario;
			cliente = O.Cliente;
			idEstatusProspeccion = O.IdEstatusProspeccion;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			idEstatusProspeccionUsuario = O.IdEstatusProspeccionUsuario;
			idDivision = O.IdDivision;
			idNivelInteresProspeccion = O.IdNivelInteresProspeccion;
			idLevantamiento = O.IdLevantamiento;
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
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
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
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresProspeccion", idNivelInteresProspeccion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", idLevantamiento);
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
		Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
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
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresProspeccion", idNivelInteresProspeccion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", idLevantamiento);
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
