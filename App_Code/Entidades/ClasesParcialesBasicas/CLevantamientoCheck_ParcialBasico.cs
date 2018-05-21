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

public partial class CLevantamientoCheck
{
	//Propiedades Privadas
	private int idLevantamientoCheck;
	private int idLevantamiento;
	private int idLevantamientoChecklist;
	private int idLevantamientoChecklistOp;
	private int cantidad;
	private string observaciones;
	private bool sINO;
	
	//Propiedades
	public int IdLevantamientoCheck
	{
		get { return idLevantamientoCheck; }
		set
		{
			idLevantamientoCheck = value;
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
	
	public int IdLevantamientoChecklist
	{
		get { return idLevantamientoChecklist; }
		set
		{
			idLevantamientoChecklist = value;
		}
	}
	
	public int IdLevantamientoChecklistOp
	{
		get { return idLevantamientoChecklistOp; }
		set
		{
			idLevantamientoChecklistOp = value;
		}
	}
	
	public int Cantidad
	{
		get { return cantidad; }
		set
		{
			cantidad = value;
		}
	}
	
	public string Observaciones
	{
		get { return observaciones; }
		set
		{
			observaciones = value;
		}
	}
	
	public bool SINO
	{
		get { return sINO; }
		set { sINO = value; }
	}
	
	//Constructores
	public CLevantamientoCheck()
	{
		idLevantamientoCheck = 0;
		idLevantamiento = 0;
		idLevantamientoChecklist = 0;
		idLevantamientoChecklistOp = 0;
		cantidad = 0;
		observaciones = "";
		sINO = false;
	}
	
	public CLevantamientoCheck(int pIdLevantamientoCheck)
	{
		idLevantamientoCheck = pIdLevantamientoCheck;
		idLevantamiento = 0;
		idLevantamientoChecklist = 0;
		idLevantamientoChecklistOp = 0;
		cantidad = 0;
		observaciones = "";
		sINO = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoCheck_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Llena<CLevantamientoCheck>(typeof(CLevantamientoCheck), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoCheck_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CLevantamientoCheck>(typeof(CLevantamientoCheck), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoCheck_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoCheck", pIdentificador);
		Obten.Llena<CLevantamientoCheck>(typeof(CLevantamientoCheck), pConexion);
		foreach (CLevantamientoCheck O in Obten.ListaRegistros)
		{
			idLevantamientoCheck = O.IdLevantamientoCheck;
			idLevantamiento = O.IdLevantamiento;
			idLevantamientoChecklist = O.IdLevantamientoChecklist;
			idLevantamientoChecklistOp = O.IdLevantamientoChecklistOp;
			cantidad = O.Cantidad;
			observaciones = O.Observaciones;
			sINO = O.SINO;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoCheck_ConsultarFiltros";
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
		Obten.Llena<CLevantamientoCheck>(typeof(CLevantamientoCheck), pConexion);
		foreach (CLevantamientoCheck O in Obten.ListaRegistros)
		{
			idLevantamientoCheck = O.IdLevantamientoCheck;
			idLevantamiento = O.IdLevantamiento;
			idLevantamientoChecklist = O.IdLevantamientoChecklist;
			idLevantamientoChecklistOp = O.IdLevantamientoChecklistOp;
			cantidad = O.Cantidad;
			observaciones = O.Observaciones;
			sINO = O.SINO;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoCheck_ConsultarFiltros";
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
		Obten.Llena<CLevantamientoCheck>(typeof(CLevantamientoCheck), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_LevantamientoCheck_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoCheck", 0);
		Agregar.StoredProcedure.Parameters["@pIdLevantamientoCheck"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", idLevantamiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklist", idLevantamientoChecklist);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklistOp", idLevantamientoChecklistOp);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pObservaciones", observaciones);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSINO", sINO);
		Agregar.Insert(pConexion);
		idLevantamientoCheck= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdLevantamientoCheck"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_LevantamientoCheck_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoCheck", idLevantamientoCheck);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamiento", idLevantamiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklist", idLevantamientoChecklist);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklistOp", idLevantamientoChecklistOp);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pObservaciones", observaciones);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSINO", sINO);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_LevantamientoCheck_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoCheck", idLevantamientoCheck);
		Eliminar.Delete(pConexion);
	}
}
