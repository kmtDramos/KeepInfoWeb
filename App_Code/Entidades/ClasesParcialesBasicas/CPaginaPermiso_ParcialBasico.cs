using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class CPaginaPermiso
{
	//Propiedades Privadas
	private int idPaginaPermiso;
	private int idPagina;
	private int idOpcion;
	private bool baja;
	
	//Propiedades
	public int IdPaginaPermiso
	{
		get { return idPaginaPermiso; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPaginaPermiso = value;
		}
	}
	
	public int IdPagina
	{
		get { return idPagina; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPagina = value;
		}
	}
	
	public int IdOpcion
	{
		get { return idOpcion; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idOpcion = value;
		}
	}
	
	public bool Baja
	{
		get { return baja; }
		set { baja = value; }
	}
	
	//Constructores
	public CPaginaPermiso()
	{
		IdPaginaPermiso = 0;
		idPagina = 1;
		idOpcion = 1;
		baja = false;
	}
	
	public CPaginaPermiso(int pIdPaginaPermiso)
	{
		idPaginaPermiso = pIdPaginaPermiso;
		idPagina = 1;
		idOpcion = 1;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PaginaPermiso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPaginaPermiso>(typeof(CPaginaPermiso), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PaginaPermiso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPaginaPermiso>(typeof(CPaginaPermiso), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PaginaPermiso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPaginaPermiso", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPaginaPermiso>(typeof(CPaginaPermiso), pConexion);
		foreach (CPaginaPermiso O in Obten.ListaRegistros)
		{
			idPaginaPermiso = O.IdPaginaPermiso;
			idPagina = O.IdPagina;
			idOpcion = O.IdOpcion;
			baja = O.Baja;
		}
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_PaginaPermiso_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", idPagina);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_PaginaPermiso_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPaginaPermiso", idPaginaPermiso);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPagina", idPagina);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_PaginaPermiso_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPaginaPermiso", idPaginaPermiso);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}