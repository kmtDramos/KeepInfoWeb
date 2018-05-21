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

public partial class CRespuesta
{
	//Propiedades Privadas
	private int idRespuesta;
	private int idUsuario;
	private int idOpcion;
	private int idPregunta;
	private int idFormulario;
	private string valor;
	private int idOportunidad;
	private bool baja;
	
	//Propiedades
	public int IdRespuesta
	{
		get { return idRespuesta; }
		set
		{
			idRespuesta = value;
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
	
	public int IdOpcion
	{
		get { return idOpcion; }
		set
		{
			idOpcion = value;
		}
	}
	
	public int IdPregunta
	{
		get { return idPregunta; }
		set
		{
			idPregunta = value;
		}
	}
	
	public int IdFormulario
	{
		get { return idFormulario; }
		set
		{
			idFormulario = value;
		}
	}
	
	public string Valor
	{
		get { return valor; }
		set
		{
			valor = value;
		}
	}
	
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CRespuesta()
	{
		idRespuesta = 0;
		idUsuario = 0;
		idOpcion = 0;
		idPregunta = 0;
		idFormulario = 0;
		valor = "";
		idOportunidad = 0;
		baja = false;
	}
	
	public CRespuesta(int pIdRespuesta)
	{
		idRespuesta = pIdRespuesta;
		idUsuario = 0;
		idOpcion = 0;
		idPregunta = 0;
		idFormulario = 0;
		valor = "";
		idOportunidad = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Respuesta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CRespuesta>(typeof(CRespuesta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Respuesta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CRespuesta>(typeof(CRespuesta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Respuesta_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdRespuesta", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CRespuesta>(typeof(CRespuesta), pConexion);
		foreach (CRespuesta O in Obten.ListaRegistros)
		{
			idRespuesta = O.IdRespuesta;
			idUsuario = O.IdUsuario;
			idOpcion = O.IdOpcion;
			idPregunta = O.IdPregunta;
			idFormulario = O.IdFormulario;
			valor = O.Valor;
			idOportunidad = O.IdOportunidad;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Respuesta_ConsultarFiltros";
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
		Obten.Llena<CRespuesta>(typeof(CRespuesta), pConexion);
		foreach (CRespuesta O in Obten.ListaRegistros)
		{
			idRespuesta = O.IdRespuesta;
			idUsuario = O.IdUsuario;
			idOpcion = O.IdOpcion;
			idPregunta = O.IdPregunta;
			idFormulario = O.IdFormulario;
			valor = O.Valor;
			idOportunidad = O.IdOportunidad;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Respuesta_ConsultarFiltros";
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
		Obten.Llena<CRespuesta>(typeof(CRespuesta), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Respuesta_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdRespuesta", 0);
		Agregar.StoredProcedure.Parameters["@pIdRespuesta"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPregunta", idPregunta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFormulario", idFormulario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idRespuesta= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdRespuesta"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Respuesta_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdRespuesta", idRespuesta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPregunta", idPregunta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFormulario", idFormulario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Respuesta_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdRespuesta", idRespuesta);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
