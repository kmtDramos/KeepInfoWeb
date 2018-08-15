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

public partial class CSolicitudMaterial
{
	//Propiedades Privadas
	private int idSolicitudMaterial;
	private int idOportunidad;
	private DateTime fechaAlta;
	private DateTime fechaEntrega;
	private int idPresupuesto;
	private int idUsuarioAprobar;
	private int idUsuarioCreador;
	private bool aprobar;
	private int idPresupuestoConcepto;
	private int cantidad;
	private string comentarios;
	private bool baja;
	
	//Propiedades
	public int IdSolicitudMaterial
	{
		get { return idSolicitudMaterial; }
		set
		{
			idSolicitudMaterial = value;
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
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaEntrega
	{
		get { return fechaEntrega; }
		set { fechaEntrega = value; }
	}
	
	public int IdPresupuesto
	{
		get { return idPresupuesto; }
		set
		{
			idPresupuesto = value;
		}
	}
	
	public int IdUsuarioAprobar
	{
		get { return idUsuarioAprobar; }
		set
		{
			idUsuarioAprobar = value;
		}
	}
	
	public int IdUsuarioCreador
	{
		get { return idUsuarioCreador; }
		set
		{
			idUsuarioCreador = value;
		}
	}
	
	public bool Aprobar
	{
		get { return aprobar; }
		set { aprobar = value; }
	}
	
	public int IdPresupuestoConcepto
	{
		get { return idPresupuestoConcepto; }
		set
		{
			idPresupuestoConcepto = value;
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
	
	public string Comentarios
	{
		get { return comentarios; }
		set
		{
			comentarios = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSolicitudMaterial()
	{
		idSolicitudMaterial = 0;
		idOportunidad = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaEntrega = new DateTime(1, 1, 1);
		idPresupuesto = 0;
		idUsuarioAprobar = 0;
		idUsuarioCreador = 0;
		aprobar = false;
		idPresupuestoConcepto = 0;
		cantidad = 0;
		comentarios = "";
		baja = false;
	}
	
	public CSolicitudMaterial(int pIdSolicitudMaterial)
	{
		idSolicitudMaterial = pIdSolicitudMaterial;
		idOportunidad = 0;
		fechaAlta = new DateTime(1, 1, 1);
		fechaEntrega = new DateTime(1, 1, 1);
		idPresupuesto = 0;
		idUsuarioAprobar = 0;
		idUsuarioCreador = 0;
		aprobar = false;
		idPresupuestoConcepto = 0;
		cantidad = 0;
		comentarios = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudMaterial>(typeof(CSolicitudMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSolicitudMaterial>(typeof(CSolicitudMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterial", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudMaterial>(typeof(CSolicitudMaterial), pConexion);
		foreach (CSolicitudMaterial O in Obten.ListaRegistros)
		{
			idSolicitudMaterial = O.IdSolicitudMaterial;
			idOportunidad = O.IdOportunidad;
			fechaAlta = O.FechaAlta;
			fechaEntrega = O.FechaEntrega;
			idPresupuesto = O.IdPresupuesto;
			idUsuarioAprobar = O.IdUsuarioAprobar;
			idUsuarioCreador = O.IdUsuarioCreador;
			aprobar = O.Aprobar;
			idPresupuestoConcepto = O.IdPresupuestoConcepto;
			cantidad = O.Cantidad;
			comentarios = O.Comentarios;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterial_ConsultarFiltros";
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
		Obten.Llena<CSolicitudMaterial>(typeof(CSolicitudMaterial), pConexion);
		foreach (CSolicitudMaterial O in Obten.ListaRegistros)
		{
			idSolicitudMaterial = O.IdSolicitudMaterial;
			idOportunidad = O.IdOportunidad;
			fechaAlta = O.FechaAlta;
			fechaEntrega = O.FechaEntrega;
			idPresupuesto = O.IdPresupuesto;
			idUsuarioAprobar = O.IdUsuarioAprobar;
			idUsuarioCreador = O.IdUsuarioCreador;
			aprobar = O.Aprobar;
			idPresupuestoConcepto = O.IdPresupuestoConcepto;
			cantidad = O.Cantidad;
			comentarios = O.Comentarios;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudMaterial_ConsultarFiltros";
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
		Obten.Llena<CSolicitudMaterial>(typeof(CSolicitudMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SolicitudMaterial_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterial", 0);
		Agregar.StoredProcedure.Parameters["@pIdSolicitudMaterial"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaEntrega.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrega", fechaEntrega);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAprobar", idUsuarioAprobar);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreador", idUsuarioCreador);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAprobar", aprobar);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", idPresupuestoConcepto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComentarios", comentarios);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSolicitudMaterial= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSolicitudMaterial"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SolicitudMaterial_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterial", idSolicitudMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaEntrega.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrega", fechaEntrega);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuesto", idPresupuesto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAprobar", idUsuarioAprobar);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreador", idUsuarioCreador);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAprobar", aprobar);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPresupuestoConcepto", idPresupuestoConcepto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComentarios", comentarios);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SolicitudMaterial_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudMaterial", idSolicitudMaterial);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
