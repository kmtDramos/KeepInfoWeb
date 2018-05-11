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

public partial class COpciones
{
	//Propiedades Privadas
	private int idOpciones;
	private int idPregunta;
	private int idFormulario;
	private string valor;
	private string descripcion;
	private int idTipoOpcion;
	private bool baja;
	
	//Propiedades
	public int IdOpciones
	{
		get { return idOpciones; }
		set
		{
			idOpciones = value;
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
	
	public string Descripcion
	{
		get { return descripcion; }
		set
		{
			descripcion = value;
		}
	}
	
	public int IdTipoOpcion
	{
		get { return idTipoOpcion; }
		set
		{
			idTipoOpcion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public COpciones()
	{
		idOpciones = 0;
		idPregunta = 0;
		idFormulario = 0;
		valor = "";
		descripcion = "";
		idTipoOpcion = 0;
		baja = false;
	}
	
	public COpciones(int pIdOpciones)
	{
		idOpciones = pIdOpciones;
		idPregunta = 0;
		idFormulario = 0;
		valor = "";
		descripcion = "";
		idTipoOpcion = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opciones_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COpciones>(typeof(COpciones), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opciones_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COpciones>(typeof(COpciones), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opciones_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOpciones", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COpciones>(typeof(COpciones), pConexion);
		foreach (COpciones O in Obten.ListaRegistros)
		{
			idOpciones = O.IdOpciones;
			idPregunta = O.IdPregunta;
			idFormulario = O.IdFormulario;
			valor = O.Valor;
			descripcion = O.Descripcion;
			idTipoOpcion = O.IdTipoOpcion;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opciones_ConsultarFiltros";
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
		Obten.Llena<COpciones>(typeof(COpciones), pConexion);
		foreach (COpciones O in Obten.ListaRegistros)
		{
			idOpciones = O.IdOpciones;
			idPregunta = O.IdPregunta;
			idFormulario = O.IdFormulario;
			valor = O.Valor;
			descripcion = O.Descripcion;
			idTipoOpcion = O.IdTipoOpcion;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Opciones_ConsultarFiltros";
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
		Obten.Llena<COpciones>(typeof(COpciones), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Opciones_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOpciones", 0);
		Agregar.StoredProcedure.Parameters["@pIdOpciones"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPregunta", idPregunta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFormulario", idFormulario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoOpcion", idTipoOpcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idOpciones= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOpciones"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Opciones_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOpciones", idOpciones);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPregunta", idPregunta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdFormulario", idFormulario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pValor", valor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoOpcion", idTipoOpcion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Opciones_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOpciones", idOpciones);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
