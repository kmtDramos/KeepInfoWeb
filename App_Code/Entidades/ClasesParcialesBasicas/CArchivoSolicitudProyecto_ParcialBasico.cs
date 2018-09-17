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

public partial class CArchivoSolicitudProyecto
{
	//Propiedades Privadas
	private int idArchivoSolicitudProyecto;
	private int idSolicitudProyecto;
	private string archivoSolicitudProyecto;
	private DateTime fechaCreacion;
	private int idUsuarioCracion;
	private bool baja;
	
	//Propiedades
	public int IdArchivoSolicitudProyecto
	{
		get { return idArchivoSolicitudProyecto; }
		set
		{
			idArchivoSolicitudProyecto = value;
		}
	}
	
	public int IdSolicitudProyecto
	{
		get { return idSolicitudProyecto; }
		set
		{
			idSolicitudProyecto = value;
		}
	}
	
	public string ArchivoSolicitudProyecto
	{
		get { return archivoSolicitudProyecto; }
		set
		{
			archivoSolicitudProyecto = value;
		}
	}
	
	public DateTime FechaCreacion
	{
		get { return fechaCreacion; }
		set { fechaCreacion = value; }
	}
	
	public int IdUsuarioCracion
	{
		get { return idUsuarioCracion; }
		set
		{
			idUsuarioCracion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CArchivoSolicitudProyecto()
	{
		idArchivoSolicitudProyecto = 0;
		idSolicitudProyecto = 0;
		archivoSolicitudProyecto = "";
		fechaCreacion = new DateTime(1, 1, 1);
		idUsuarioCracion = 0;
		baja = false;
	}
	
	public CArchivoSolicitudProyecto(int pIdArchivoSolicitudProyecto)
	{
		idArchivoSolicitudProyecto = pIdArchivoSolicitudProyecto;
		idSolicitudProyecto = 0;
		archivoSolicitudProyecto = "";
		fechaCreacion = new DateTime(1, 1, 1);
		idUsuarioCracion = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CArchivoSolicitudProyecto>(typeof(CArchivoSolicitudProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CArchivoSolicitudProyecto>(typeof(CArchivoSolicitudProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdArchivoSolicitudProyecto", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CArchivoSolicitudProyecto>(typeof(CArchivoSolicitudProyecto), pConexion);
		foreach (CArchivoSolicitudProyecto O in Obten.ListaRegistros)
		{
			idArchivoSolicitudProyecto = O.IdArchivoSolicitudProyecto;
			idSolicitudProyecto = O.IdSolicitudProyecto;
			archivoSolicitudProyecto = O.ArchivoSolicitudProyecto;
			fechaCreacion = O.FechaCreacion;
			idUsuarioCracion = O.IdUsuarioCracion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_ConsultarFiltros";
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
		Obten.Llena<CArchivoSolicitudProyecto>(typeof(CArchivoSolicitudProyecto), pConexion);
		foreach (CArchivoSolicitudProyecto O in Obten.ListaRegistros)
		{
			idArchivoSolicitudProyecto = O.IdArchivoSolicitudProyecto;
			idSolicitudProyecto = O.IdSolicitudProyecto;
			archivoSolicitudProyecto = O.ArchivoSolicitudProyecto;
			fechaCreacion = O.FechaCreacion;
			idUsuarioCracion = O.IdUsuarioCracion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_ConsultarFiltros";
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
		Obten.Llena<CArchivoSolicitudProyecto>(typeof(CArchivoSolicitudProyecto), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdArchivoSolicitudProyecto", 0);
		Agregar.StoredProcedure.Parameters["@pIdArchivoSolicitudProyecto"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudProyecto", idSolicitudProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pArchivoSolicitudProyecto", archivoSolicitudProyecto);
		if(fechaCreacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCracion", idUsuarioCracion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idArchivoSolicitudProyecto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdArchivoSolicitudProyecto"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdArchivoSolicitudProyecto", idArchivoSolicitudProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudProyecto", idSolicitudProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pArchivoSolicitudProyecto", archivoSolicitudProyecto);
		if(fechaCreacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCracion", idUsuarioCracion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ArchivoSolicitudProyecto_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdArchivoSolicitudProyecto", idArchivoSolicitudProyecto);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
