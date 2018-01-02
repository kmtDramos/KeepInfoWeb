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

public partial class CSucursal
{
	//Propiedades Privadas
	private int idSucursal;
	private string sucursal;
	private int idEmpresa;
	private string calle;
	private string numeroExterior;
	private string colonia;
	private string correo;
	private string dominio;
	private string telefono;
	private string codigoPostal;
	private int idMunicipio;
	private string numeroInterior;
	private string referencia;
	private bool direccionFiscal;
	private int idLocalidad;
	private int idTipoMoneda;
	private string claveCuentaContable;
	private decimal iVAActual;
	private int idIVA;
	private string alias;
	private string baseDatosContpaq;
	private string usuarioContpaq;
	private string contrasenaContpaq;
	private string noCertificado;
	private bool baja;
	
	//Propiedades
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			idSucursal = value;
		}
	}
	
	public string Sucursal
	{
		get { return sucursal; }
		set
		{
			sucursal = value;
		}
	}
	
	public int IdEmpresa
	{
		get { return idEmpresa; }
		set
		{
			idEmpresa = value;
		}
	}
	
	public string Calle
	{
		get { return calle; }
		set
		{
			calle = value;
		}
	}
	
	public string NumeroExterior
	{
		get { return numeroExterior; }
		set
		{
			numeroExterior = value;
		}
	}
	
	public string Colonia
	{
		get { return colonia; }
		set
		{
			colonia = value;
		}
	}
	
	public string Correo
	{
		get { return correo; }
		set
		{
			correo = value;
		}
	}
	
	public string Dominio
	{
		get { return dominio; }
		set
		{
			dominio = value;
		}
	}
	
	public string Telefono
	{
		get { return telefono; }
		set
		{
			telefono = value;
		}
	}
	
	public string CodigoPostal
	{
		get { return codigoPostal; }
		set
		{
			codigoPostal = value;
		}
	}
	
	public int IdMunicipio
	{
		get { return idMunicipio; }
		set
		{
			idMunicipio = value;
		}
	}
	
	public string NumeroInterior
	{
		get { return numeroInterior; }
		set
		{
			numeroInterior = value;
		}
	}
	
	public string Referencia
	{
		get { return referencia; }
		set
		{
			referencia = value;
		}
	}
	
	public bool DireccionFiscal
	{
		get { return direccionFiscal; }
		set { direccionFiscal = value; }
	}
	
	public int IdLocalidad
	{
		get { return idLocalidad; }
		set
		{
			idLocalidad = value;
		}
	}
	
	public int IdTipoMoneda
	{
		get { return idTipoMoneda; }
		set
		{
			idTipoMoneda = value;
		}
	}
	
	public string ClaveCuentaContable
	{
		get { return claveCuentaContable; }
		set
		{
			claveCuentaContable = value;
		}
	}
	
	public decimal IVAActual
	{
		get { return iVAActual; }
		set
		{
			iVAActual = value;
		}
	}
	
	public int IdIVA
	{
		get { return idIVA; }
		set
		{
			idIVA = value;
		}
	}
	
	public string Alias
	{
		get { return alias; }
		set
		{
			alias = value;
		}
	}
	
	public string BaseDatosContpaq
	{
		get { return baseDatosContpaq; }
		set
		{
			baseDatosContpaq = value;
		}
	}
	
	public string UsuarioContpaq
	{
		get { return usuarioContpaq; }
		set
		{
			usuarioContpaq = value;
		}
	}
	
	public string ContrasenaContpaq
	{
		get { return contrasenaContpaq; }
		set
		{
			contrasenaContpaq = value;
		}
	}
	
	public string NoCertificado
	{
		get { return noCertificado; }
		set
		{
			noCertificado = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CSucursal()
	{
		idSucursal = 0;
		sucursal = "";
		idEmpresa = 0;
		calle = "";
		numeroExterior = "";
		colonia = "";
		correo = "";
		dominio = "";
		telefono = "";
		codigoPostal = "";
		idMunicipio = 0;
		numeroInterior = "";
		referencia = "";
		direccionFiscal = false;
		idLocalidad = 0;
		idTipoMoneda = 0;
		claveCuentaContable = "";
		iVAActual = 0;
		idIVA = 0;
		alias = "";
		baseDatosContpaq = "";
		usuarioContpaq = "";
		contrasenaContpaq = "";
		noCertificado = "";
		baja = false;
	}
	
	public CSucursal(int pIdSucursal)
	{
		idSucursal = pIdSucursal;
		sucursal = "";
		idEmpresa = 0;
		calle = "";
		numeroExterior = "";
		colonia = "";
		correo = "";
		dominio = "";
		telefono = "";
		codigoPostal = "";
		idMunicipio = 0;
		numeroInterior = "";
		referencia = "";
		direccionFiscal = false;
		idLocalidad = 0;
		idTipoMoneda = 0;
		claveCuentaContable = "";
		iVAActual = 0;
		idIVA = 0;
		alias = "";
		baseDatosContpaq = "";
		usuarioContpaq = "";
		contrasenaContpaq = "";
		noCertificado = "";
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Sucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Sucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Sucursal_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
		foreach (CSucursal O in Obten.ListaRegistros)
		{
			idSucursal = O.IdSucursal;
			sucursal = O.Sucursal;
			idEmpresa = O.IdEmpresa;
			calle = O.Calle;
			numeroExterior = O.NumeroExterior;
			colonia = O.Colonia;
			correo = O.Correo;
			dominio = O.Dominio;
			telefono = O.Telefono;
			codigoPostal = O.CodigoPostal;
			idMunicipio = O.IdMunicipio;
			numeroInterior = O.NumeroInterior;
			referencia = O.Referencia;
			direccionFiscal = O.DireccionFiscal;
			idLocalidad = O.IdLocalidad;
			idTipoMoneda = O.IdTipoMoneda;
			claveCuentaContable = O.ClaveCuentaContable;
			iVAActual = O.IVAActual;
			idIVA = O.IdIVA;
			alias = O.Alias;
			baseDatosContpaq = O.BaseDatosContpaq;
			usuarioContpaq = O.UsuarioContpaq;
			contrasenaContpaq = O.ContrasenaContpaq;
			noCertificado = O.NoCertificado;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Sucursal_ConsultarFiltros";
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
		Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
		foreach (CSucursal O in Obten.ListaRegistros)
		{
			idSucursal = O.IdSucursal;
			sucursal = O.Sucursal;
			idEmpresa = O.IdEmpresa;
			calle = O.Calle;
			numeroExterior = O.NumeroExterior;
			colonia = O.Colonia;
			correo = O.Correo;
			dominio = O.Dominio;
			telefono = O.Telefono;
			codigoPostal = O.CodigoPostal;
			idMunicipio = O.IdMunicipio;
			numeroInterior = O.NumeroInterior;
			referencia = O.Referencia;
			direccionFiscal = O.DireccionFiscal;
			idLocalidad = O.IdLocalidad;
			idTipoMoneda = O.IdTipoMoneda;
			claveCuentaContable = O.ClaveCuentaContable;
			iVAActual = O.IVAActual;
			idIVA = O.IdIVA;
			alias = O.Alias;
			baseDatosContpaq = O.BaseDatosContpaq;
			usuarioContpaq = O.UsuarioContpaq;
			contrasenaContpaq = O.ContrasenaContpaq;
			noCertificado = O.NoCertificado;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Sucursal_ConsultarFiltros";
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
		Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Sucursal_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", 0);
		Agregar.StoredProcedure.Parameters["@pIdSucursal"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pSucursal", sucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDominio", dominio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDireccionFiscal", direccionFiscal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIVAActual", iVAActual);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIVA", idIVA);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pAlias", alias);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaseDatosContpaq", baseDatosContpaq);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pUsuarioContpaq", usuarioContpaq);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pContrasenaContpaq", contrasenaContpaq);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNoCertificado", noCertificado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idSucursal= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdSucursal"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Sucursal_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pSucursal", sucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
		Editar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDominio", dominio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
		Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDireccionFiscal", direccionFiscal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", claveCuentaContable);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIVAActual", iVAActual);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdIVA", idIVA);
		Editar.StoredProcedure.Parameters.AddWithValue("@pAlias", alias);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaseDatosContpaq", baseDatosContpaq);
		Editar.StoredProcedure.Parameters.AddWithValue("@pUsuarioContpaq", usuarioContpaq);
		Editar.StoredProcedure.Parameters.AddWithValue("@pContrasenaContpaq", contrasenaContpaq);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNoCertificado", noCertificado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Sucursal_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
