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

public partial class CReingresoMaterial
{
	//Propiedades Privadas
	private int idReingresoMaterial;
	private int idCliente;
	private int idProyecto;
	private int idMotivoReingreso;
	private DateTime fechaAlta;
	private int idUsuarioAlta;
	private bool baja;
	
	//Propiedades
	public int IdReingresoMaterial
	{
		get { return idReingresoMaterial; }
		set
		{
			idReingresoMaterial = value;
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
	
	public int IdMotivoReingreso
	{
		get { return idMotivoReingreso; }
		set
		{
			idMotivoReingreso = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public int IdUsuarioAlta
	{
		get { return idUsuarioAlta; }
		set
		{
			idUsuarioAlta = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CReingresoMaterial()
	{
		idReingresoMaterial = 0;
		idCliente = 0;
		idProyecto = 0;
		idMotivoReingreso = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioAlta = 0;
		baja = false;
	}
	
	public CReingresoMaterial(int pIdReingresoMaterial)
	{
		idReingresoMaterial = pIdReingresoMaterial;
		idCliente = 0;
		idProyecto = 0;
		idMotivoReingreso = 0;
		fechaAlta = new DateTime(1, 1, 1);
		idUsuarioAlta = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ReingresoMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CReingresoMaterial>(typeof(CReingresoMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ReingresoMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CReingresoMaterial>(typeof(CReingresoMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ReingresoMaterial_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdReingresoMaterial", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CReingresoMaterial>(typeof(CReingresoMaterial), pConexion);
		foreach (CReingresoMaterial O in Obten.ListaRegistros)
		{
			idReingresoMaterial = O.IdReingresoMaterial;
			idCliente = O.IdCliente;
			idProyecto = O.IdProyecto;
			idMotivoReingreso = O.IdMotivoReingreso;
			fechaAlta = O.FechaAlta;
			idUsuarioAlta = O.IdUsuarioAlta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ReingresoMaterial_ConsultarFiltros";
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
		Obten.Llena<CReingresoMaterial>(typeof(CReingresoMaterial), pConexion);
		foreach (CReingresoMaterial O in Obten.ListaRegistros)
		{
			idReingresoMaterial = O.IdReingresoMaterial;
			idCliente = O.IdCliente;
			idProyecto = O.IdProyecto;
			idMotivoReingreso = O.IdMotivoReingreso;
			fechaAlta = O.FechaAlta;
			idUsuarioAlta = O.IdUsuarioAlta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_ReingresoMaterial_ConsultarFiltros";
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
		Obten.Llena<CReingresoMaterial>(typeof(CReingresoMaterial), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_ReingresoMaterial_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdReingresoMaterial", 0);
		Agregar.StoredProcedure.Parameters["@pIdReingresoMaterial"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMotivoReingreso", idMotivoReingreso);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idReingresoMaterial= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdReingresoMaterial"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_ReingresoMaterial_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdReingresoMaterial", idReingresoMaterial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMotivoReingreso", idMotivoReingreso);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_ReingresoMaterial_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdReingresoMaterial", idReingresoMaterial);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
