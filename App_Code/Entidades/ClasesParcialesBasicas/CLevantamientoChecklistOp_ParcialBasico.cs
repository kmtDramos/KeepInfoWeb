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

public partial class CLevantamientoChecklistOp
{
	//Propiedades Privadas
	private int idLevantamientoChecklistOp;
	private int idLevantamientoChecklist;
	private string descripcion ;
	private bool baja;
	
	//Propiedades
	public int IdLevantamientoChecklistOp
	{
		get { return idLevantamientoChecklistOp; }
		set
		{
			idLevantamientoChecklistOp = value;
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
	
	public string Descripcion 
	{
		get { return descripcion ; }
		set
		{
			descripcion  = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CLevantamientoChecklistOp()
	{
		idLevantamientoChecklistOp = 0;
		idLevantamientoChecklist = 0;
		descripcion  = "";
		baja = false;
	}
	
	public CLevantamientoChecklistOp(int pIdLevantamientoChecklistOp)
	{
		idLevantamientoChecklistOp = pIdLevantamientoChecklistOp;
		idLevantamientoChecklist = 0;
		descripcion  = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CLevantamientoChecklistOp>(typeof(CLevantamientoChecklistOp), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CLevantamientoChecklistOp>(typeof(CLevantamientoChecklistOp), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklistOp", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CLevantamientoChecklistOp>(typeof(CLevantamientoChecklistOp), pConexion);
		foreach (CLevantamientoChecklistOp O in Obten.ListaRegistros)
		{
			idLevantamientoChecklistOp = O.IdLevantamientoChecklistOp;
			idLevantamientoChecklist = O.IdLevantamientoChecklist;
			descripcion  = O.Descripcion ;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_ConsultarFiltros";
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
		Obten.Llena<CLevantamientoChecklistOp>(typeof(CLevantamientoChecklistOp), pConexion);
		foreach (CLevantamientoChecklistOp O in Obten.ListaRegistros)
		{
			idLevantamientoChecklistOp = O.IdLevantamientoChecklistOp;
			idLevantamientoChecklist = O.IdLevantamientoChecklist;
			descripcion  = O.Descripcion ;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_ConsultarFiltros";
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
		Obten.Llena<CLevantamientoChecklistOp>(typeof(CLevantamientoChecklistOp), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklistOp", 0);
		Agregar.StoredProcedure.Parameters["@pIdLevantamientoChecklistOp"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklist", idLevantamientoChecklist);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion ", descripcion );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idLevantamientoChecklistOp= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdLevantamientoChecklistOp"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklistOp", idLevantamientoChecklistOp);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklist", idLevantamientoChecklist);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion ", descripcion );
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_LevantamientoChecklistOp_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdLevantamientoChecklistOp", idLevantamientoChecklistOp);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
