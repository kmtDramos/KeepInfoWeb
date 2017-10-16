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

public partial class CRutaCFDI
{
	//Propiedades Privadas
	private int idRutaCFDI;
	private string rutaCFDI;
	private int idSucursal;
	private int idUsuarioAlta;
	private DateTime fechaAlta;
	private int tipoRuta;
	private bool baja;
	
	//Propiedades
	public int IdRutaCFDI
	{
		get { return idRutaCFDI; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idRutaCFDI = value;
		}
	}
	
	public string RutaCFDI
	{
		get { return rutaCFDI; }
		set
		{
			rutaCFDI = value;
		}
	}
	
	public int IdSucursal
	{
		get { return idSucursal; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idSucursal = value;
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
	
	public DateTime FechaAlta
	{
		get { return fechaAlta; }
		set { fechaAlta = value; }
	}
	
	public int TipoRuta
	{
		get { return tipoRuta; }
		set
		{
			if (value < 0)
			{
				return;
			}
			tipoRuta = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CRutaCFDI()
	{
		idRutaCFDI = 0;
		rutaCFDI = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		tipoRuta = 0;
		baja = false;
	}
	
	public CRutaCFDI(int pIdRutaCFDI)
	{
		idRutaCFDI = pIdRutaCFDI;
		rutaCFDI = "";
		idSucursal = 0;
		idUsuarioAlta = 0;
		fechaAlta = new DateTime(1, 1, 1);
		tipoRuta = 0;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_RutaCFDI_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CRutaCFDI>(typeof(CRutaCFDI), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_RutaCFDI_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CRutaCFDI>(typeof(CRutaCFDI), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_RutaCFDI_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdRutaCFDI", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CRutaCFDI>(typeof(CRutaCFDI), pConexion);
		foreach (CRutaCFDI O in Obten.ListaRegistros)
		{
			idRutaCFDI = O.IdRutaCFDI;
			rutaCFDI = O.RutaCFDI;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			tipoRuta = O.TipoRuta;
			baja = O.Baja;
		}
	}
	
	public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_RutaCFDI_ConsultarFiltros";
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
		Obten.Llena<CRutaCFDI>(typeof(CRutaCFDI), pConexion);
		foreach (CRutaCFDI O in Obten.ListaRegistros)
		{
			idRutaCFDI = O.IdRutaCFDI;
			rutaCFDI = O.RutaCFDI;
			idSucursal = O.IdSucursal;
			idUsuarioAlta = O.IdUsuarioAlta;
			fechaAlta = O.FechaAlta;
			tipoRuta = O.TipoRuta;
			baja = O.Baja;
		}
	}
	
	public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_RutaCFDI_ConsultarFiltros";
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
		Obten.Llena<CRutaCFDI>(typeof(CRutaCFDI), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_RutaCFDI_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdRutaCFDI", 0);
		Agregar.StoredProcedure.Parameters["@pIdRutaCFDI"].Direction = ParameterDirection.Output;
		Agregar.StoredProcedure.Parameters.AddWithValue("@pRutaCFDI", rutaCFDI);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoRuta", tipoRuta);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
		idRutaCFDI= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdRutaCFDI"].Value);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_RutaCFDI_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdRutaCFDI", idRutaCFDI);
		Editar.StoredProcedure.Parameters.AddWithValue("@pRutaCFDI", rutaCFDI);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
		if(fechaAlta.Year != 1)
		{
			Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
		}
		Editar.StoredProcedure.Parameters.AddWithValue("@pTipoRuta", tipoRuta);
		Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_RutaCFDI_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdRutaCFDI", idRutaCFDI);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}