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

public partial class CUsuario
{
	//Propiedades Privadas
	private int idUsuario;
	private string usuario;
	private string contrasena;
	private int idPerfil;
	private string nombre;
	private string apellidoPaterno;
	private string apellidoMaterno;
	private DateTime fechaNacimiento;
	private string correo;
	private DateTime fechaIngreso;
	private int idSucursalPredeterminada;
	private int idSucursalActual;
	private bool esAgente;
	private decimal alcance1;
	private decimal alcance2;
	private decimal meta;
	private int clientesNuevos;
	private decimal comision1;
	private decimal comision2;
	private decimal comision3;
	private bool esVendedor;
	private int idDepartamento;
	private decimal porcentajeComision;
	private bool baja;
	
	//Propiedades
	public int IdUsuario
	{
		get { return idUsuario; }
		set
		{
			idUsuario = value;
		}
	}
	
	public string Usuario
	{
		get { return usuario; }
		set
		{
			usuario = value;
		}
	}
	
	public string Contrasena
	{
		get { return contrasena; }
		set
		{
			contrasena = value;
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
	
	public string Nombre
	{
		get { return nombre; }
		set
		{
			nombre = value;
		}
	}
	
	public string ApellidoPaterno
	{
		get { return apellidoPaterno; }
		set
		{
			apellidoPaterno = value;
		}
	}
	
	public string ApellidoMaterno
	{
		get { return apellidoMaterno; }
		set
		{
			apellidoMaterno = value;
		}
	}
	
	public DateTime FechaNacimiento
	{
		get { return fechaNacimiento; }
		set { fechaNacimiento = value; }
	}
	
	public string Correo
	{
		get { return correo; }
		set
		{
			correo = value;
		}
	}
	
	public DateTime FechaIngreso
	{
		get { return fechaIngreso; }
		set { fechaIngreso = value; }
	}
	
	public int IdSucursalPredeterminada
	{
		get { return idSucursalPredeterminada; }
		set
		{
			idSucursalPredeterminada = value;
		}
	}
	
	public int IdSucursalActual
	{
		get { return idSucursalActual; }
		set
		{
			idSucursalActual = value;
		}
	}
	
	public bool EsAgente
	{
		get { return esAgente; }
		set { esAgente = value; }
	}
	
	public decimal Alcance1
	{
		get { return alcance1; }
		set
		{
			alcance1 = value;
		}
	}
	
	public decimal Alcance2
	{
		get { return alcance2; }
		set
		{
			alcance2 = value;
		}
	}
	
	public decimal Meta
	{
		get { return meta; }
		set
		{
			meta = value;
		}
	}
	
	public int ClientesNuevos
	{
		get { return clientesNuevos; }
		set
		{
			clientesNuevos = value;
		}
	}
	
	public decimal Comision1
	{
		get { return comision1; }
		set
		{
			comision1 = value;
		}
	}
	
	public decimal Comision2
	{
		get { return comision2; }
		set
		{
			comision2 = value;
		}
	}
	
	public decimal Comision3
	{
		get { return comision3; }
		set
		{
			comision3 = value;
		}
	}
	
	public bool EsVendedor
	{
		get { return esVendedor; }
		set { esVendedor = value; }
	}
	
	public int IdDepartamento
	{
		get { return idDepartamento; }
		set
		{
			idDepartamento = value;
		}
	}
	
	public decimal PorcentajeComision
	{
		get { return porcentajeComision; }
		set
		{
			porcentajeComision = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CUsuario()
	{
		idUsuario = 0;
		usuario = "";
		contrasena = "";
		idPerfil = 0;
		nombre = "";
		apellidoPaterno = "";
		apellidoMaterno = "";
		fechaNacimiento = new DateTime(1, 1, 1);
		correo = "";
		fechaIngreso = new DateTime(1, 1, 1);
		idSucursalPredeterminada = 0;
		idSucursalActual = 0;
		esAgente = false;
		alcance1 = 0;
		alcance2 = 0;
		meta = 0;
		clientesNuevos = 0;
		comision1 = 0;
		comision2 = 0;
		comision3 = 0;
		esVendedor = false;
		idDepartamento = 0;
		porcentajeComision = 0;
		baja = false;
	}
	
	public CUsuario(int pIdUsuario)
	{
		idUsuario = pIdUsuario;
		usuario = "";
		contrasena = "";
		idPerfil = 0;
		nombre = "";
		apellidoPaterno = "";
		apellidoMaterno = "";
		fechaNacimiento = new DateTime(1, 1, 1);
		correo = "";
		fechaIngreso = new DateTime(1, 1, 1);
		idSucursalPredeterminada = 0;
		idSucursalActual = 0;
		esAgente = false;
		alcance1 = 0;
		alcance2 = 0;
		meta = 0;
		clientesNuevos = 0;
		comision1 = 0;
		comision2 = 0;
		comision3 = 0;
		esVendedor = false;
		idDepartamento = 0;
		porcentajeComision = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Usuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Usuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Usuario_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
		foreach (CUsuario O in Obten.ListaRegistros)
		{
			idUsuario = O.IdUsuario;
			usuario = O.Usuario;
			contrasena = O.Contrasena;
			idPerfil = O.IdPerfil;
			nombre = O.Nombre;
			apellidoPaterno = O.ApellidoPaterno;
			apellidoMaterno = O.ApellidoMaterno;
			fechaNacimiento = O.FechaNacimiento;
			correo = O.Correo;
			fechaIngreso = O.FechaIngreso;
			idSucursalPredeterminada = O.IdSucursalPredeterminada;
			idSucursalActual = O.IdSucursalActual;
			esAgente = O.EsAgente;
			alcance1 = O.Alcance1;
			alcance2 = O.Alcance2;
			meta = O.Meta;
			clientesNuevos = O.ClientesNuevos;
			comision1 = O.Comision1;
			comision2 = O.Comision2;
			comision3 = O.Comision3;
			esVendedor = O.EsVendedor;
			idDepartamento = O.IdDepartamento;
			porcentajeComision = O.PorcentajeComision;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Usuario_ConsultarFiltros";
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
		Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
		foreach (CUsuario O in Obten.ListaRegistros)
		{
			idUsuario = O.IdUsuario;
			usuario = O.Usuario;
			contrasena = O.Contrasena;
			idPerfil = O.IdPerfil;
			nombre = O.Nombre;
			apellidoPaterno = O.ApellidoPaterno;
			apellidoMaterno = O.ApellidoMaterno;
			fechaNacimiento = O.FechaNacimiento;
			correo = O.Correo;
			fechaIngreso = O.FechaIngreso;
			idSucursalPredeterminada = O.IdSucursalPredeterminada;
			idSucursalActual = O.IdSucursalActual;
			esAgente = O.EsAgente;
			alcance1 = O.Alcance1;
			alcance2 = O.Alcance2;
			meta = O.Meta;
			clientesNuevos = O.ClientesNuevos;
			comision1 = O.Comision1;
			comision2 = O.Comision2;
			comision3 = O.Comision3;
			esVendedor = O.EsVendedor;
			idDepartamento = O.IdDepartamento;
			porcentajeComision = O.PorcentajeComision;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Usuario_ConsultarFiltros";
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
		Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Usuario_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", 0);
		Agregar.StoredProcedure.Parameters["@pIdUsuario"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUsuario", usuario);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pContrasena", contrasena);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pApellidoPaterno", apellidoPaterno);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pApellidoMaterno", apellidoMaterno);
		if(fechaNacimiento.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaNacimiento", fechaNacimiento);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		if(fechaIngreso.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaIngreso", fechaIngreso);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalPredeterminada", idSucursalPredeterminada);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalActual", idSucursalActual);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsAgente", esAgente);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAlcance1", alcance1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAlcance2", alcance2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pMeta", meta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClientesNuevos", clientesNuevos);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComision1", comision1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComision2", comision2);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pComision3", comision3);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEsVendedor", esVendedor);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDepartamento", idDepartamento);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeComision", porcentajeComision);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idUsuario= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdUsuario"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Usuario_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUsuario", usuario);
		Editar.StoredProcedure.Parameters.AddWithValue("@pContrasena", contrasena);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNombre", nombre);
		Editar.StoredProcedure.Parameters.AddWithValue("@pApellidoPaterno", apellidoPaterno);
		Editar.StoredProcedure.Parameters.AddWithValue("@pApellidoMaterno", apellidoMaterno);
		if(fechaNacimiento.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaNacimiento", fechaNacimiento);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		if(fechaIngreso.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaIngreso", fechaIngreso);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalPredeterminada", idSucursalPredeterminada);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalActual", idSucursalActual);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsAgente", esAgente);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAlcance1", alcance1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAlcance2", alcance2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pMeta", meta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClientesNuevos", clientesNuevos);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComision1", comision1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComision2", comision2);
		Editar.StoredProcedure.Parameters.AddWithValue("@pComision3", comision3);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEsVendedor", esVendedor);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdDepartamento", idDepartamento);
		Editar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeComision", porcentajeComision);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Usuario_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
