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

public partial class CControlDashboardUsuario
{
	//Propiedades Privadas
	private int idControlDashboardUsuario;
	private int idPerfil;
	private string templateControl;
	private string nombreControl;
	private string metodoControl;
	private int orden;
	private string identificador;
	private int idControl;
	private bool baja;
	
	//Propiedades
	public int IdControlDashboardUsuario
	{
		get { return idControlDashboardUsuario; }
		set
		{
			idControlDashboardUsuario = value;
		}
	}
	
	public int IdPerfil
	{
		get { return idPerfil; }
		set
		{
			idPerfil = value;
		}
	}
	
	public string TemplateControl
	{
		get { return templateControl; }
		set
		{
			templateControl = value;
		}
	}
	
	public string NombreControl
	{
		get { return nombreControl; }
		set
		{
			nombreControl = value;
		}
	}
	
	public string MetodoControl
	{
		get { return metodoControl; }
		set
		{
			metodoControl = value;
		}
	}
	
	public int Orden
	{
		get { return orden; }
		set
		{
			orden = value;
		}
	}
	
	public string Identificador
	{
		get { return identificador; }
		set
		{
			identificador = value;
		}
	}
	
	public int IdControl
	{
		get { return idControl; }
		set
		{
			idControl = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CControlDashboardUsuario()
	{
		idControlDashboardUsuario = 0;
		idPerfil = 0;
		templateControl = "";
		nombreControl = "";
		metodoControl = "";
		orden = 0;
		identificador = "";
		idControl = 0;
		baja = false;
	}
	
	public CControlDashboardUsuario(int pIdControlDashboardUsuario)
	{
		idControlDashboardUsuario = pIdControlDashboardUsuario;
		idPerfil = 0;
		templateControl = "";
		nombreControl = "";
		metodoControl = "";
		orden = 0;
		identificador = "";
		idControl = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CControlDashboardUsuario>(typeof(CControlDashboardUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CControlDashboardUsuario>(typeof(CControlDashboardUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdControlDashboardUsuario", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CControlDashboardUsuario>(typeof(CControlDashboardUsuario), pConexion);
		foreach (CControlDashboardUsuario O in Obten.ListaRegistros)
		{
			idControlDashboardUsuario = O.IdControlDashboardUsuario;
			idPerfil = O.IdPerfil;
			templateControl = O.TemplateControl;
			nombreControl = O.NombreControl;
			metodoControl = O.MetodoControl;
			orden = O.Orden;
			identificador = O.Identificador;
			idControl = O.IdControl;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_ConsultarFiltros";
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
		Obten.Llena<CControlDashboardUsuario>(typeof(CControlDashboardUsuario), pConexion);
		foreach (CControlDashboardUsuario O in Obten.ListaRegistros)
		{
			idControlDashboardUsuario = O.IdControlDashboardUsuario;
			idPerfil = O.IdPerfil;
			templateControl = O.TemplateControl;
			nombreControl = O.NombreControl;
			metodoControl = O.MetodoControl;
			orden = O.Orden;
			identificador = O.Identificador;
			idControl = O.IdControl;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_ConsultarFiltros";
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
		Obten.Llena<CControlDashboardUsuario>(typeof(CControlDashboardUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdControlDashboardUsuario", 0);
		Agregar.StoredProcedure.Parameters["@pIdControlDashboardUsuario"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTemplateControl", templateControl);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNombreControl", nombreControl);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMetodoControl", metodoControl);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdentificador", identificador);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdControl", idControl);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idControlDashboardUsuario= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdControlDashboardUsuario"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdControlDashboardUsuario", idControlDashboardUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTemplateControl", templateControl);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNombreControl", nombreControl);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMetodoControl", metodoControl);
		Editar.StoredProcedure.Parameters.AddWithValue("@pOrden", orden);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdentificador", identificador);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdControl", idControl);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ControlDashboardUsuario_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdControlDashboardUsuario", idControlDashboardUsuario);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
