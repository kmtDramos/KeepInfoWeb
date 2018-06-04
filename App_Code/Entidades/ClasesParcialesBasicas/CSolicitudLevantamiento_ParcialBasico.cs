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

public partial class CSolicitudLevantamiento
{
	//Propiedades Privadas
	private int idSolicitudLevantamiento;
	private int idUsuarioAsignado;
	private DateTime citaFechaHora;
	private string horaCliente;
	private bool confirmarSolicitud;
	private int diasOportunidadSolicitud;
	private bool levantamientoCreado;
	private DateTime fechaAlta;
	private DateTime fechaCita;
	private int idOportunidad;
	private int idCliente;
	private string contactoDirecto ;
	private string contactoEnSitio ;
	private string telefonos ;
	private bool esAsociado;
	private int idPuestoContactoDirecto;
	private int idPuestoContactoEnSitio;
	private string horaAtencionCliente ;
	private bool confirmado;
	private int idAgente;
	private bool permisoIngresarSitio;
	private bool equipoSeguridadIngresarSitio;
	private bool clienteCuentaEstacionamiento;
	private bool clienteCuentaPlanoLevantamiento;
	private string domicilio ;
	private int idDivision;
	private string descripcion ;
	private string notas ;
	private int idCreador;
	private bool baja;
	
	//Propiedades
	public int IdSolicitudLevantamiento
	{
		get { return idSolicitudLevantamiento; }
		set
		{
			idSolicitudLevantamiento = value;
		}
	}
	
	public int IdUsuarioAsignado
	{
		get { return idUsuarioAsignado; }
		set
		{
			idUsuarioAsignado = value;
		}
	}
	
	public DateTime CitaFechaHora
	{
		get { return citaFechaHora; }
		set { citaFechaHora = value; }
	}
	
	public string HoraCliente
	{
		get { return horaCliente; }
		set
		{
			horaCliente = value;
		}
	}
	
	public bool ConfirmarSolicitud
	{
		get { return confirmarSolicitud; }
		set { confirmarSolicitud = value; }
	}
	
	public int DiasOportunidadSolicitud
	{
		get { return diasOportunidadSolicitud; }
		set
		{
			diasOportunidadSolicitud = value;
		}
	}
	
	public bool LevantamientoCreado
	{
		get { return levantamientoCreado; }
		set { levantamientoCreado = value; }
	}
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public DateTime FechaCita
	{
		get { return fechaCita; }
		set { fechaCita = value; }
	}
	
	public int IdOportunidad
	{
		get { return idOportunidad; }
		set
		{
			idOportunidad = value;
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
	
	public string ContactoDirecto 
	{
		get { return contactoDirecto ; }
		set
		{
			contactoDirecto  = value;
		}
	}
	
	public string ContactoEnSitio 
	{
		get { return contactoEnSitio ; }
		set
		{
			contactoEnSitio  = value;
		}
	}
	
	public string Telefonos 
	{
		get { return telefonos ; }
		set
		{
			telefonos  = value;
		}
	}
	
	public bool EsAsociado
	{
		get { return esAsociado; }
		set { esAsociado = value; }
	}
	
	public int IdPuestoContactoDirecto
	{
		get { return idPuestoContactoDirecto; }
		set
		{
			idPuestoContactoDirecto = value;
		}
	}
	
	public int IdPuestoContactoEnSitio
	{
		get { return idPuestoContactoEnSitio; }
		set
		{
			idPuestoContactoEnSitio = value;
		}
	}
	
	public string HoraAtencionCliente 
	{
		get { return horaAtencionCliente ; }
		set
		{
			horaAtencionCliente  = value;
		}
	}
	
	public bool Confirmado
	{
		get { return confirmado; }
		set { confirmado = value; }
	}
	
	public int IdAgente
	{
		get { return idAgente; }
		set
		{
			idAgente = value;
		}
	}
	
	public bool PermisoIngresarSitio
	{
		get { return permisoIngresarSitio; }
		set { permisoIngresarSitio = value; }
	}
	
	public bool EquipoSeguridadIngresarSitio
	{
		get { return equipoSeguridadIngresarSitio; }
		set { equipoSeguridadIngresarSitio = value; }
	}
	
	public bool ClienteCuentaEstacionamiento
	{
		get { return clienteCuentaEstacionamiento; }
		set { clienteCuentaEstacionamiento = value; }
	}
	
	public bool ClienteCuentaPlanoLevantamiento
	{
		get { return clienteCuentaPlanoLevantamiento; }
		set { clienteCuentaPlanoLevantamiento = value; }
	}
	
	public string Domicilio 
	{
		get { return domicilio ; }
		set
		{
			domicilio  = value;
		}
	}
	
	public int IdDivision
	{
		get { return idDivision; }
		set
		{
			idDivision = value;
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
	
	public string Notas 
	{
		get { return notas ; }
		set
		{
			notas  = value;
		}
	}
	
	public int IdCreador
	{
		get { return idCreador; }
		set
		{
			idCreador = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSolicitudLevantamiento()
	{
		idSolicitudLevantamiento = 0;
		idUsuarioAsignado = 0;
		citaFechaHora = new DateTime(1, 1, 1);
		horaCliente = "";
		confirmarSolicitud = false;
		diasOportunidadSolicitud = 0;
		levantamientoCreado = false;
		fechaAlta = new DateTime(1, 1, 1);
		fechaCita = new DateTime(1, 1, 1);
		idOportunidad = 0;
		idCliente = 0;
		contactoDirecto  = "";
		contactoEnSitio  = "";
		telefonos  = "";
		esAsociado = false;
		idPuestoContactoDirecto = 0;
		idPuestoContactoEnSitio = 0;
		horaAtencionCliente  = "";
		confirmado = false;
		idAgente = 0;
		permisoIngresarSitio = false;
		equipoSeguridadIngresarSitio = false;
		clienteCuentaEstacionamiento = false;
		clienteCuentaPlanoLevantamiento = false;
		domicilio  = "";
		idDivision = 0;
		descripcion  = "";
		notas  = "";
		idCreador = 0;
		baja = false;
	}
	
	public CSolicitudLevantamiento(int pIdSolicitudLevantamiento)
	{
		idSolicitudLevantamiento = pIdSolicitudLevantamiento;
		idUsuarioAsignado = 0;
		citaFechaHora = new DateTime(1, 1, 1);
		horaCliente = "";
		confirmarSolicitud = false;
		diasOportunidadSolicitud = 0;
		levantamientoCreado = false;
		fechaAlta = new DateTime(1, 1, 1);
		fechaCita = new DateTime(1, 1, 1);
		idOportunidad = 0;
		idCliente = 0;
		contactoDirecto  = "";
		contactoEnSitio  = "";
		telefonos  = "";
		esAsociado = false;
		idPuestoContactoDirecto = 0;
		idPuestoContactoEnSitio = 0;
		horaAtencionCliente  = "";
		confirmado = false;
		idAgente = 0;
		permisoIngresarSitio = false;
		equipoSeguridadIngresarSitio = false;
		clienteCuentaEstacionamiento = false;
		clienteCuentaPlanoLevantamiento = false;
		domicilio  = "";
		idDivision = 0;
		descripcion  = "";
		notas  = "";
		idCreador = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudLevantamiento>(typeof(CSolicitudLevantamiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSolicitudLevantamiento>(typeof(CSolicitudLevantamiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudLevantamiento", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSolicitudLevantamiento>(typeof(CSolicitudLevantamiento), pConexion);
		foreach (CSolicitudLevantamiento O in Obten.ListaRegistros)
		{
			idSolicitudLevantamiento = O.IdSolicitudLevantamiento;
			idUsuarioAsignado = O.IdUsuarioAsignado;
			citaFechaHora = O.CitaFechaHora;
			horaCliente = O.HoraCliente;
			confirmarSolicitud = O.ConfirmarSolicitud;
			diasOportunidadSolicitud = O.DiasOportunidadSolicitud;
			levantamientoCreado = O.LevantamientoCreado;
			fechaAlta = O.FechaAlta;
			fechaCita = O.FechaCita;
			idOportunidad = O.IdOportunidad;
			idCliente = O.IdCliente;
			contactoDirecto  = O.ContactoDirecto ;
			contactoEnSitio  = O.ContactoEnSitio ;
			telefonos  = O.Telefonos ;
			esAsociado = O.EsAsociado;
			idPuestoContactoDirecto = O.IdPuestoContactoDirecto;
			idPuestoContactoEnSitio = O.IdPuestoContactoEnSitio;
			horaAtencionCliente  = O.HoraAtencionCliente ;
			confirmado = O.Confirmado;
			idAgente = O.IdAgente;
			permisoIngresarSitio = O.PermisoIngresarSitio;
			equipoSeguridadIngresarSitio = O.EquipoSeguridadIngresarSitio;
			clienteCuentaEstacionamiento = O.ClienteCuentaEstacionamiento;
			clienteCuentaPlanoLevantamiento = O.ClienteCuentaPlanoLevantamiento;
			domicilio  = O.Domicilio ;
			idDivision = O.IdDivision;
			descripcion  = O.Descripcion ;
			notas  = O.Notas ;
			idCreador = O.IdCreador;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_ConsultarFiltros";
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
		Obten.Llena<CSolicitudLevantamiento>(typeof(CSolicitudLevantamiento), pConexion);
		foreach (CSolicitudLevantamiento O in Obten.ListaRegistros)
		{
			idSolicitudLevantamiento = O.IdSolicitudLevantamiento;
			idUsuarioAsignado = O.IdUsuarioAsignado;
			citaFechaHora = O.CitaFechaHora;
			horaCliente = O.HoraCliente;
			confirmarSolicitud = O.ConfirmarSolicitud;
			diasOportunidadSolicitud = O.DiasOportunidadSolicitud;
			levantamientoCreado = O.LevantamientoCreado;
			fechaAlta = O.FechaAlta;
			fechaCita = O.FechaCita;
			idOportunidad = O.IdOportunidad;
			idCliente = O.IdCliente;
			contactoDirecto  = O.ContactoDirecto ;
			contactoEnSitio  = O.ContactoEnSitio ;
			telefonos  = O.Telefonos ;
			esAsociado = O.EsAsociado;
			idPuestoContactoDirecto = O.IdPuestoContactoDirecto;
			idPuestoContactoEnSitio = O.IdPuestoContactoEnSitio;
			horaAtencionCliente  = O.HoraAtencionCliente ;
			confirmado = O.Confirmado;
			idAgente = O.IdAgente;
			permisoIngresarSitio = O.PermisoIngresarSitio;
			equipoSeguridadIngresarSitio = O.EquipoSeguridadIngresarSitio;
			clienteCuentaEstacionamiento = O.ClienteCuentaEstacionamiento;
			clienteCuentaPlanoLevantamiento = O.ClienteCuentaPlanoLevantamiento;
			domicilio  = O.Domicilio ;
			idDivision = O.IdDivision;
			descripcion  = O.Descripcion ;
			notas  = O.Notas ;
			idCreador = O.IdCreador;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_ConsultarFiltros";
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
		Obten.Llena<CSolicitudLevantamiento>(typeof(CSolicitudLevantamiento), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudLevantamiento", 0);
		Agregar.StoredProcedure.Parameters["@pIdSolicitudLevantamiento"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAsignado", idUsuarioAsignado);
		if(citaFechaHora.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCitaFechaHora", citaFechaHora);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pHoraCliente", horaCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pConfirmarSolicitud", confirmarSolicitud);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDiasOportunidadSolicitud", diasOportunidadSolicitud);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLevantamientoCreado", levantamientoCreado);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaCita.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCita", fechaCita);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pContactoDirecto ", contactoDirecto );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pContactoEnSitio ", contactoEnSitio );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTelefonos ", telefonos );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsAsociado", esAsociado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPuestoContactoDirecto", idPuestoContactoDirecto);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPuestoContactoEnSitio", idPuestoContactoEnSitio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pHoraAtencionCliente ", horaAtencionCliente );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pConfirmado", confirmado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAgente", idAgente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPermisoIngresarSitio", permisoIngresarSitio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEquipoSeguridadIngresarSitio", equipoSeguridadIngresarSitio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClienteCuentaEstacionamiento", clienteCuentaEstacionamiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClienteCuentaPlanoLevantamiento", clienteCuentaPlanoLevantamiento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDomicilio ", domicilio );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion ", descripcion );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNotas ", notas );
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCreador", idCreador);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSolicitudLevantamiento= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSolicitudLevantamiento"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudLevantamiento", idSolicitudLevantamiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAsignado", idUsuarioAsignado);
		if(citaFechaHora.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pCitaFechaHora", citaFechaHora);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pHoraCliente", horaCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pConfirmarSolicitud", confirmarSolicitud);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDiasOportunidadSolicitud", diasOportunidadSolicitud);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLevantamientoCreado", levantamientoCreado);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		if(fechaCita.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCita", fechaCita);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pContactoDirecto ", contactoDirecto );
		Editar.StoredProcedure.Parameters.AddWithValue("@pContactoEnSitio ", contactoEnSitio );
		Editar.StoredProcedure.Parameters.AddWithValue("@pTelefonos ", telefonos );
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsAsociado", esAsociado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPuestoContactoDirecto", idPuestoContactoDirecto);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPuestoContactoEnSitio", idPuestoContactoEnSitio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pHoraAtencionCliente ", horaAtencionCliente );
		Editar.StoredProcedure.Parameters.AddWithValue("@pConfirmado", confirmado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdAgente", idAgente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPermisoIngresarSitio", permisoIngresarSitio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEquipoSeguridadIngresarSitio", equipoSeguridadIngresarSitio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClienteCuentaEstacionamiento", clienteCuentaEstacionamiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClienteCuentaPlanoLevantamiento", clienteCuentaPlanoLevantamiento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDomicilio ", domicilio );
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion ", descripcion );
		Editar.StoredProcedure.Parameters.AddWithValue("@pNotas ", notas );
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdCreador", idCreador);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_SolicitudLevantamiento_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudLevantamiento", idSolicitudLevantamiento);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
