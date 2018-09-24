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

public partial class CSalidaMaterial
{
	//Propiedades Privadas
	private int idSalidaMaterial;
	private int idCliente;
	private int idProyecto;
	private int idMotivoSalida;
	private int fechaAltaERROR;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private bool baja;
	
	//Propiedades
	public int IdSalidaMaterial
	{
		get { return idSalidaMaterial; }
		set
		{
			idSalidaMaterial = value;
		}
	}
	
	public int IdCliente
	{
		get { return idCliente; }
		set
		{
			idCliente = value;
		}
	}
	
	public int IdProyecto
	{
		get { return idProyecto; }
		set
		{
			idProyecto = value;
		}
	}
	
	public int IdMotivoSalida
	{
		get { return idMotivoSalida; }
		set
		{
			idMotivoSalida = value;
		}
	}
	
	public int FechaAltaERROR
	{
		get { return fechaAltaERROR; }
		set
		{
			fechaAltaERROR = value;
		}
	}
	
	public int IdUsuarioAlta
	{
		get { return idUsuarioAlta; }
		set
		{
			idUsuarioAlta = value;
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
	public CSalidaMaterial()
	{
		idSalidaMaterial = 0;
		idCliente = 0;
		idProyecto = 0;
		idMotivoSalida = 0;
		fechaAltaERROR = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	public CSalidaMaterial(int pIdSalidaMaterial)
	{
		idSalidaMaterial = pIdSalidaMaterial;
		idCliente = 0;
		idProyecto = 0;
		idMotivoSalida = 0;
		fechaAltaERROR = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SalidaMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSalidaMaterial>(typeof(CSalidaMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SalidaMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSalidaMaterial>(typeof(CSalidaMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SalidaMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSalidaMaterial", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSalidaMaterial>(typeof(CSalidaMaterial), pConexion);
		foreach (CSalidaMaterial O in Obten.ListaRegistros)
		{
			idSalidaMaterial = O.IdSalidaMaterial;
			idCliente = O.IdCliente;
			idProyecto = O.IdProyecto;
			idMotivoSalida = O.IdMotivoSalida;
			fechaAltaERROR = O.FechaAltaERROR;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SalidaMaterial_ConsultarFiltros";
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
		Obten.Llena<CSalidaMaterial>(typeof(CSalidaMaterial), pConexion);
		foreach (CSalidaMaterial O in Obten.ListaRegistros)
		{
			idSalidaMaterial = O.IdSalidaMaterial;
			idCliente = O.IdCliente;
			idProyecto = O.IdProyecto;
			idMotivoSalida = O.IdMotivoSalida;
			fechaAltaERROR = O.FechaAltaERROR;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SalidaMaterial_ConsultarFiltros";
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
		Obten.Llena<CSalidaMaterial>(typeof(CSalidaMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SalidaMaterial_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSalidaMaterial", 0);
		Agregar.StoredProcedure.Parameters["@pIdSalidaMaterial"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMotivoSalida", idMotivoSalida);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAltaERROR", fechaAltaERROR);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSalidaMaterial= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSalidaMaterial"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SalidaMaterial_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSalidaMaterial", idSalidaMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMotivoSalida", idMotivoSalida);
		Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAltaERROR", fechaAltaERROR);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
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
		Eliminar.StoredProcedure.CommandText = "spb_SalidaMaterial_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSalidaMaterial", idSalidaMaterial);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
