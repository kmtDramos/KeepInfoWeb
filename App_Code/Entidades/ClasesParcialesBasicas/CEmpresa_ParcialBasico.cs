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

public partial class CEmpresa
{
	//Propiedades Privadas
	private int idEmpresa;
	private string empresa;
	private string razonSocial;
	private string clave;
	private string rFC;
	private string calle;
	private string numeroExterior;
	private string colonia;
	private string codigoPostal;
	private string telefono;
	private string correo;
	private string logo;
	private string dominio;
	private int idMunicipio;
	private string numeroInterior;
	private string referencia;
	private int idLocalidad;
	private string regimenFiscal;
	private string token;
	private int idTimbrado;
	private bool baja;
	
	//Propiedades
	public int IdEmpresa
	{
		get { return idEmpresa; }
		set
		{
			idEmpresa = value;
		}
	}
	
	public string Empresa
	{
		get { return empresa; }
		set
		{
			empresa = value;
		}
	}
	
	public string RazonSocial
	{
		get { return razonSocial; }
		set
		{
			razonSocial = value;
		}
	}
	
	public string Clave
	{
		get { return clave; }
		set
		{
			clave = value;
		}
	}
	
	public string RFC
	{
		get { return rFC; }
		set
		{
			rFC = value;
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
	
	public string CodigoPostal
	{
		get { return codigoPostal; }
		set
		{
			codigoPostal = value;
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
	
	public string Correo
	{
		get { return correo; }
		set
		{
			correo = value;
		}
	}
	
	public string Logo
	{
		get { return logo; }
		set
		{
			logo = value;
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
	
	public int IdLocalidad
	{
		get { return idLocalidad; }
		set
		{
			idLocalidad = value;
		}
	}
	
	public string RegimenFiscal
	{
		get { return regimenFiscal; }
		set
		{
			regimenFiscal = value;
		}
	}
	
	public string Token
	{
		get { return token; }
		set
		{
			token = value;
		}
	}
	
	public int IdTimbrado
	{
		get { return idTimbrado; }
		set
		{
			idTimbrado = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CEmpresa()
	{
		idEmpresa = 0;
		empresa = "";
		razonSocial = "";
		clave = "";
		rFC = "";
		calle = "";
		numeroExterior = "";
		colonia = "";
		codigoPostal = "";
		telefono = "";
		correo = "";
		logo = "";
		dominio = "";
		idMunicipio = 0;
		numeroInterior = "";
		referencia = "";
		idLocalidad = 0;
		regimenFiscal = "";
		token = "";
		idTimbrado = 0;
		baja = false;
	}
	
	public CEmpresa(int pIdEmpresa)
	{
		idEmpresa = pIdEmpresa;
		empresa = "";
		razonSocial = "";
		clave = "";
		rFC = "";
		calle = "";
		numeroExterior = "";
		colonia = "";
		codigoPostal = "";
		telefono = "";
		correo = "";
		logo = "";
		dominio = "";
		idMunicipio = 0;
		numeroInterior = "";
		referencia = "";
		idLocalidad = 0;
		regimenFiscal = "";
		token = "";
		idTimbrado = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Empresa_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEmpresa>(typeof(CEmpresa), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Empresa_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CEmpresa>(typeof(CEmpresa), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Empresa_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CEmpresa>(typeof(CEmpresa), pConexion);
		foreach (CEmpresa O in Obten.ListaRegistros)
		{
			idEmpresa = O.IdEmpresa;
			empresa = O.Empresa;
			razonSocial = O.RazonSocial;
			clave = O.Clave;
			rFC = O.RFC;
			calle = O.Calle;
			numeroExterior = O.NumeroExterior;
			colonia = O.Colonia;
			codigoPostal = O.CodigoPostal;
			telefono = O.Telefono;
			correo = O.Correo;
			logo = O.Logo;
			dominio = O.Dominio;
			idMunicipio = O.IdMunicipio;
			numeroInterior = O.NumeroInterior;
			referencia = O.Referencia;
			idLocalidad = O.IdLocalidad;
			regimenFiscal = O.RegimenFiscal;
			token = O.Token;
			idTimbrado = O.IdTimbrado;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Empresa_ConsultarFiltros";
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
		Obten.Llena<CEmpresa>(typeof(CEmpresa), pConexion);
		foreach (CEmpresa O in Obten.ListaRegistros)
		{
			idEmpresa = O.IdEmpresa;
			empresa = O.Empresa;
			razonSocial = O.RazonSocial;
			clave = O.Clave;
			rFC = O.RFC;
			calle = O.Calle;
			numeroExterior = O.NumeroExterior;
			colonia = O.Colonia;
			codigoPostal = O.CodigoPostal;
			telefono = O.Telefono;
			correo = O.Correo;
			logo = O.Logo;
			dominio = O.Dominio;
			idMunicipio = O.IdMunicipio;
			numeroInterior = O.NumeroInterior;
			referencia = O.Referencia;
			idLocalidad = O.IdLocalidad;
			regimenFiscal = O.RegimenFiscal;
			token = O.Token;
			idTimbrado = O.IdTimbrado;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_Empresa_ConsultarFiltros";
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
		Obten.Llena<CEmpresa>(typeof(CEmpresa), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_Empresa_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", 0);
		Agregar.StoredProcedure.Parameters["@pIdEmpresa"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pEmpresa", empresa);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", razonSocial);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRFC", rFC);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pLogo", logo);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pDominio", dominio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRegimenFiscal", regimenFiscal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pToken", token);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTimbrado", idTimbrado);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idEmpresa= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEmpresa"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_Empresa_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Editar.StoredProcedure.Parameters.AddWithValue("@pEmpresa", empresa);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", razonSocial);
		Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRFC", rFC);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCalle", calle);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExterior", numeroExterior);
		Editar.StoredProcedure.Parameters.AddWithValue("@pColonia", colonia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostal", codigoPostal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pTelefono", telefono);
		Editar.StoredProcedure.Parameters.AddWithValue("@pCorreo", correo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pLogo", logo);
		Editar.StoredProcedure.Parameters.AddWithValue("@pDominio", dominio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipio", idMunicipio);
		Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInterior", numeroInterior);
		Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidad", idLocalidad);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRegimenFiscal", regimenFiscal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pToken", token);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdTimbrado", idTimbrado);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_Empresa_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}
