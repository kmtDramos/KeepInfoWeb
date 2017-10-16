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

public partial class CEstatusProspeccionUsuario
{
	//Propiedades Privadas
	private int idEstatusProspeccionUsuario;
	private int idUsuario;
	private int idEstatusProspeccion;
	private int idProspeccion;
	private DateTime fechaAlta;
	private bool baja;
	
	//Propiedades
	public int IdEstatusProspeccionUsuario
	{
		get { return idEstatusProspeccionUsuario; }
		set
		{
			idEstatusProspeccionUsuario = value;
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
	
	public int IdEstatusProspeccion
	{
		get { return idEstatusProspeccion; }
		set
		{
			idEstatusProspeccion = value;
		}
	}
	
	public int IdProspeccion
	{
		get { return idProspeccion; }
		set
		{
			idProspeccion = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CEstatusProspeccionUsuario()
	{
		idEstatusProspeccionUsuario = 0;
		idUsuario = 0;
		idEstatusProspeccion = 0;
		idProspeccion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CEstatusProspeccionUsuario(int pIdEstatusProspeccionUsuario)
	{
		idEstatusProspeccionUsuario = pIdEstatusProspeccionUsuario;
		idUsuario = 0;
		idEstatusProspeccion = 0;
		idProspeccion = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstatusProspeccionUsuario>(typeof(CEstatusProspeccionUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CEstatusProspeccionUsuario>(typeof(CEstatusProspeccionUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccionUsuario", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEstatusProspeccionUsuario>(typeof(CEstatusProspeccionUsuario), pConexion);
		foreach (CEstatusProspeccionUsuario O in Obten.ListaRegistros)
		{
			idEstatusProspeccionUsuario = O.IdEstatusProspeccionUsuario;
			idUsuario = O.IdUsuario;
			idEstatusProspeccion = O.IdEstatusProspeccion;
			idProspeccion = O.IdProspeccion;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_ConsultarFiltros";
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
		Obten.Llena<CEstatusProspeccionUsuario>(typeof(CEstatusProspeccionUsuario), pConexion);
		foreach (CEstatusProspeccionUsuario O in Obten.ListaRegistros)
		{
			idEstatusProspeccionUsuario = O.IdEstatusProspeccionUsuario;
			idUsuario = O.IdUsuario;
			idEstatusProspeccion = O.IdEstatusProspeccion;
			idProspeccion = O.IdProspeccion;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_ConsultarFiltros";
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
		Obten.Llena<CEstatusProspeccionUsuario>(typeof(CEstatusProspeccionUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccionUsuario", 0);
		Agregar.StoredProcedure.Parameters["@pIdEstatusProspeccionUsuario"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", idEstatusProspeccion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProspeccion", idProspeccion);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idEstatusProspeccionUsuario= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEstatusProspeccionUsuario"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccionUsuario", idEstatusProspeccionUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccion", idEstatusProspeccion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProspeccion", idProspeccion);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_EstatusProspeccionUsuario_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusProspeccionUsuario", idEstatusProspeccionUsuario);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
