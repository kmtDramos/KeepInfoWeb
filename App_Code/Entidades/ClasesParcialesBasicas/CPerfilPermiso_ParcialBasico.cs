using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class CPerfilPermiso
{
	//Propiedades Privadas
	private int idPerfilPermiso;
	private int idPerfil;
	private int idOpcion;
	private bool baja;
	
	//Propiedades
	public int IdPerfilPermiso
	{
		get { return idPerfilPermiso; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPerfilPermiso = value;
		}
	}
	
	public int IdPerfil
	{
		get { return idPerfil; }
		set
		{
			if (value < 0)
			{
				return;
			}
			idPerfil = value;
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
	public CPerfilPermiso()
	{
		IdPerfilPermiso = 0;
		idPerfil = 1;
		idOpcion = 1;
		baja = false;
	}
	
	public CPerfilPermiso(int pIdPerfilPermiso)
	{
		idPerfilPermiso = pIdPerfilPermiso;
		idPerfil = 1;
		idOpcion = 1;
		baja = false;
	}
	
	//Metodos Basicos
	public List<object> LlenaObjetos(CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PerfilPermiso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPerfilPermiso>(typeof(CPerfilPermiso), pConexion);
		return Obten.ListaRegistros;
	}
	
	public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PerfilPermiso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Columnas = new string[pColumnas.Length];
		Obten.Columnas = pColumnas;
		Obten.Llena<CPerfilPermiso>(typeof(CPerfilPermiso), pConexion);
		return Obten.ListaRegistros;
	}
	
	public void LlenaObjeto(int pIdentificador, CConexion pConexion)
	{
		CSelect Obten = new CSelect();
		Obten.StoredProcedure.CommandText = "spb_PerfilPermiso_Consultar";
		Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Obten.StoredProcedure.Parameters.AddWithValue("@pIdPerfilPermiso", pIdentificador);
		Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		Obten.Llena<CPerfilPermiso>(typeof(CPerfilPermiso), pConexion);
		foreach (CPerfilPermiso O in Obten.ListaRegistros)
		{
			idPerfilPermiso = O.IdPerfilPermiso;
			idPerfil = O.IdPerfil;
			idOpcion = O.IdOpcion;
			baja = O.Baja;
		}
	}
	
	public void Agregar(CConexion pConexion)
	{
		CConsultaAccion Agregar = new CConsultaAccion();
		Agregar.StoredProcedure.CommandText = "spb_PerfilPermiso_Agregar";
		Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Agregar.Insert(pConexion);
	}
	
	public void Editar(CConexion pConexion)
	{
		CConsultaAccion Editar = new CConsultaAccion();
		Editar.StoredProcedure.CommandText = "spb_PerfilPermiso_Editar";
		Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfilPermiso", idPerfilPermiso);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
		Editar.StoredProcedure.Parameters.AddWithValue("@pIdOpcion", idOpcion);
		Editar.Update(pConexion);
	}
	
	public void Eliminar(CConexion pConexion)
	{
		CConsultaAccion Eliminar = new CConsultaAccion();
		Eliminar.StoredProcedure.CommandText = "spb_PerfilPermiso_Eliminar";
		Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdPerfilPermiso", idPerfilPermiso);
		Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
		Eliminar.Delete(pConexion);
	}
}