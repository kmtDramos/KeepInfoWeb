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

public partial class COrganizacion
{
	//Propiedades Privadas
	private int idOrganizacion;
	private string nombreComercial;
	private DateTime fechaAlta;
	private DateTime fechaModificacion;
	private string rFC;
	private string notas;
	private string dominio;
	private int idTipoIndustria;
	private int idUsuarioAlta;
	private int idUsuarioModifico;
	private int idGrupoEmpresarial;
	private string razonSocial;
	private int idEmpresaAlta;
	private int idSegmentoMercado;
	private bool baja;
	
	//Propiedades
	public int IdOrganizacion
	{
		get { return idOrganizacion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOrganizacion = value;
		}
	}
	
	public string NombreComercial
	{
		get { return nombreComercial; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			nombreComercial = value;
		}
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaModificacion
	{
		get { return fechaModificacion; }
		set { fechaModificacion = value; }
	}
	
	public string RFC
	{
		get { return rFC; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			rFC = value;
		}
	}
	
	public string Notas
	{
		get { return notas; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			notas = value;
		}
	}
	
	public string Dominio
	{
		get { return dominio; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			dominio = value;
		}
	}
	
	public int IdTipoIndustria
	{
		get { return idTipoIndustria; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idTipoIndustria = value;
		}
	}
	
	public int IdUsuarioAlta
	{
		get { return idUsuarioAlta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioAlta = value;
		}
	}
	
	public int IdUsuarioModifico
	{
		get { return idUsuarioModifico; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idUsuarioModifico = value;
		}
	}
	
	public int IdGrupoEmpresarial
	{
		get { return idGrupoEmpresarial; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idGrupoEmpresarial = value;
		}
	}
	
	public string RazonSocial
	{
		get { return razonSocial; }
		set
		{
			if (value.Length == 0)
			{
				return;
			}
			razonSocial = value;
		}
	}
	
	public int IdEmpresaAlta
	{
		get { return idEmpresaAlta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idEmpresaAlta = value;
		}
	}
	
	public int IdSegmentoMercado
	{
		get { return idSegmentoMercado; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSegmentoMercado = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public COrganizacion()
	{
		idOrganizacion = 0;
		nombreComercial = "";
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		rFC = "";
		notas = "";
		dominio = "";
		idTipoIndustria = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		idGrupoEmpresarial = 0;
		razonSocial = "";
		idEmpresaAlta = 0;
		idSegmentoMercado = 0;
		baja = false;
	}
	
	public COrganizacion(int pIdOrganizacion)
	{
		idOrganizacion = pIdOrganizacion;
		nombreComercial = "";
		fechaAlta = new DateTime(1, 1, 1);
		fechaModificacion = new DateTime(1, 1, 1);
		rFC = "";
		notas = "";
		dominio = "";
		idTipoIndustria = 0;
		idUsuarioAlta = 0;
		idUsuarioModifico = 0;
		idGrupoEmpresarial = 0;
		razonSocial = "";
		idEmpresaAlta = 0;
		idSegmentoMercado = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Organizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrganizacion>(typeof(COrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Organizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<COrganizacion>(typeof(COrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Organizacion_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<COrganizacion>(typeof(COrganizacion), pConexion);
		foreach (COrganizacion O in Obten.ListaRegistros)
		{
			idOrganizacion = O.IdOrganizacion;
			nombreComercial = O.NombreComercial;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			rFC = O.RFC;
			notas = O.Notas;
			dominio = O.Dominio;
			idTipoIndustria = O.IdTipoIndustria;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			idGrupoEmpresarial = O.IdGrupoEmpresarial;
			razonSocial = O.RazonSocial;
			idEmpresaAlta = O.IdEmpresaAlta;
			idSegmentoMercado = O.IdSegmentoMercado;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Organizacion_ConsultarFiltros";
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
		Obten.Llena<COrganizacion>(typeof(COrganizacion), pConexion);
		foreach (COrganizacion O in Obten.ListaRegistros)
		{
			idOrganizacion = O.IdOrganizacion;
			nombreComercial = O.NombreComercial;
			fechaAlta = O.FechaAlta;
			fechaModificacion = O.FechaModificacion;
			rFC = O.RFC;
			notas = O.Notas;
			dominio = O.Dominio;
			idTipoIndustria = O.IdTipoIndustria;
			idUsuarioAlta = O.IdUsuarioAlta;
			idUsuarioModifico = O.IdUsuarioModifico;
			idGrupoEmpresarial = O.IdGrupoEmpresarial;
			razonSocial = O.RazonSocial;
			idEmpresaAlta = O.IdEmpresaAlta;
			idSegmentoMercado = O.IdSegmentoMercado;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Organizacion_ConsultarFiltros";
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
		Obten.Llena<COrganizacion>(typeof(COrganizacion), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Organizacion_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", 0);
		Agregar.StoredProcedure.Parameters["@pIdOrganizacion"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNombreComercial", nombreComercial);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRFC", rFC);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDominio", dominio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIndustria", idTipoIndustria);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGrupoEmpresarial", idGrupoEmpresarial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", razonSocial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresaAlta", idEmpresaAlta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSegmentoMercado", idSegmentoMercado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idOrganizacion= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrganizacion"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Organizacion_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNombreComercial", nombreComercial);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaModificacion.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaModificacion", fechaModificacion);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pRFC", rFC);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNotas", notas);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDominio", dominio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIndustria", idTipoIndustria);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdGrupoEmpresarial", idGrupoEmpresarial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", razonSocial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresaAlta", idEmpresaAlta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSegmentoMercado", idSegmentoMercado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Organizacion_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", idOrganizacion);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}