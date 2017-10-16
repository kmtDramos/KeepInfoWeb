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

public partial class CClienteRelacionUsuario
{
	//Propiedades Privadas
	private int idClienteRelacionUsuario;
	private int idClienteRelacion;
	private int idUsuarioAgente;
	private DateTime idClienteRelacionEstatus;
	private DateTime fechaAlta;
	private DateTime fehcaEstatus;
	private bool baja;
	
	//Propiedades
	public int IdClienteRelacionUsuario
	{
		get { return idClienteRelacionUsuario; }
		set
		{
			idClienteRelacionUsuario = value;
		}
	}
	
	public int IdClienteRelacion
	{
		get { return idClienteRelacion; }
		set
		{
			idClienteRelacion = value;
		}
	}
	
	public int IdUsuarioAgente
	{
		get { return idUsuarioAgente; }
		set
		{
			idUsuarioAgente = value;
		}
	}
	
	public DateTime IdClienteRelacionEstatus
	{
		get { return idClienteRelacionEstatus; }
		set { idClienteRelacionEstatus = value; }
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FehcaEstatus
	{
		get { return fehcaEstatus; }
		set { fehcaEstatus = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CClienteRelacionUsuario()
	{
		idClienteRelacionUsuario = 0;
		idClienteRelacion = 0;
		idUsuarioAgente = 0;
		idClienteRelacionEstatus = new DateTime(1, 1, 1);
		fechaAlta = new DateTime(1, 1, 1);
		fehcaEstatus = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CClienteRelacionUsuario(int pIdClienteRelacionUsuario)
	{
		idClienteRelacionUsuario = pIdClienteRelacionUsuario;
		idClienteRelacion = 0;
		idUsuarioAgente = 0;
		idClienteRelacionEstatus = new DateTime(1, 1, 1);
		fechaAlta = new DateTime(1, 1, 1);
		fehcaEstatus = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CClienteRelacionUsuario>(typeof(CClienteRelacionUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CClienteRelacionUsuario>(typeof(CClienteRelacionUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionUsuario", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CClienteRelacionUsuario>(typeof(CClienteRelacionUsuario), pConexion);
		foreach (CClienteRelacionUsuario O in Obten.ListaRegistros)
		{
			idClienteRelacionUsuario = O.IdClienteRelacionUsuario;
			idClienteRelacion = O.IdClienteRelacion;
			idUsuarioAgente = O.IdUsuarioAgente;
			idClienteRelacionEstatus = O.IdClienteRelacionEstatus;
			fechaAlta = O.FechaAlta;
			fehcaEstatus = O.FehcaEstatus;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_ConsultarFiltros";
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
		Obten.Llena<CClienteRelacionUsuario>(typeof(CClienteRelacionUsuario), pConexion);
		foreach (CClienteRelacionUsuario O in Obten.ListaRegistros)
		{
			idClienteRelacionUsuario = O.IdClienteRelacionUsuario;
			idClienteRelacion = O.IdClienteRelacion;
			idUsuarioAgente = O.IdUsuarioAgente;
			idClienteRelacionEstatus = O.IdClienteRelacionEstatus;
			fechaAlta = O.FechaAlta;
			fehcaEstatus = O.FehcaEstatus;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_ConsultarFiltros";
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
		Obten.Llena<CClienteRelacionUsuario>(typeof(CClienteRelacionUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionUsuario", 0);
		Agregar.StoredProcedure.Parameters["@pIdClienteRelacionUsuario"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", idClienteRelacion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		if(idClienteRelacionEstatus.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionEstatus", idClienteRelacionEstatus);
		}
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fehcaEstatus.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFehcaEstatus", fehcaEstatus);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idClienteRelacionUsuario= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdClienteRelacionUsuario"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionUsuario", idClienteRelacionUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacion", idClienteRelacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
		if(idClienteRelacionEstatus.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionEstatus", idClienteRelacionEstatus);
		}
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fehcaEstatus.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFehcaEstatus", fehcaEstatus);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ClienteRelacionUsuario_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdClienteRelacionUsuario", idClienteRelacionUsuario);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
