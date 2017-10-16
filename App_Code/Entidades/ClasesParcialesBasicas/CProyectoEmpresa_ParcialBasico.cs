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

	public partial class CProyectoEmpresa
	{
		//Propiedades Privadas
		private int idProyectoEmpresa;
		private DateTime fechaAlta;
		private DateTime fechaUltimaModificacion;
		private int idProyecto;
		private int idEmpresa;
		private int idUsuarioAlta;
		private int idUsuarioModifico;
		private int idSucursal;
		private bool baja;
		
		//Propiedades
		public int IdProyectoEmpresa
		{
			get { return idProyectoEmpresa; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idProyectoEmpresa = value;
			}
		}
		
		public DateTime FechaAlta
		{
			get { return fechaAlta; }
			set { fechaAlta = value; }
		}
		
		public DateTime FechaUltimaModificacion
		{
			get { return fechaUltimaModificacion; }
			set { fechaUltimaModificacion = value; }
		}
		
		public int IdProyecto
		{
			get { return idProyecto; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idProyecto = value;
			}
		}
		
		public int IdEmpresa
		{
			get { return idEmpresa; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idEmpresa = value;
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
		
		public bool Baja
		{
			get { return baja; }
			set { baja = value; }
		}
		
		//Constructores
		public CProyectoEmpresa()
		{
			idProyectoEmpresa = 0;
			fechaAlta = new DateTime(1, 1, 1);
			fechaUltimaModificacion = new DateTime(1, 1, 1);
			idProyecto = 0;
			idEmpresa = 0;
			idUsuarioAlta = 0;
			idUsuarioModifico = 0;
			idSucursal = 0;
			baja = false;
		}
		
		public CProyectoEmpresa(int pIdProyectoEmpresa)
		{
			idProyectoEmpresa = pIdProyectoEmpresa;
			fechaAlta = new DateTime(1, 1, 1);
			fechaUltimaModificacion = new DateTime(1, 1, 1);
			idProyecto = 0;
			idEmpresa = 0;
			idUsuarioAlta = 0;
			idUsuarioModifico = 0;
			idSucursal = 0;
			baja = false;
		}
		
		//Metodos Basicos
		public List<object> LlenaObjetos(CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_ProyectoEmpresa_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Llena<CProyectoEmpresa>(typeof(CProyectoEmpresa), pConexion);
			return Obten.ListaRegistros;
		}
		
		public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_ProyectoEmpresa_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Columnas = new string[pColumnas.Length];
			Obten.Columnas = pColumnas;
			Obten.Llena<CProyectoEmpresa>(typeof(CProyectoEmpresa), pConexion);
			return Obten.ListaRegistros;
		}
		
		public void LlenaObjeto(int pIdentificador, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_ProyectoEmpresa_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
			Obten.StoredProcedure.Parameters.AddWithValue("@pIdProyectoEmpresa", pIdentificador);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Llena<CProyectoEmpresa>(typeof(CProyectoEmpresa), pConexion);
			foreach (CProyectoEmpresa O in Obten.ListaRegistros)
			{
				idProyectoEmpresa = O.IdProyectoEmpresa;
				fechaAlta = O.FechaAlta;
				fechaUltimaModificacion = O.FechaUltimaModificacion;
				idProyecto = O.IdProyecto;
				idEmpresa = O.IdEmpresa;
				idUsuarioAlta = O.IdUsuarioAlta;
				idUsuarioModifico = O.IdUsuarioModifico;
				idSucursal = O.IdSucursal;
				baja = O.Baja;
			}
		}
		
		public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_ProyectoEmpresa_ConsultarFiltros";
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
			Obten.Llena<CProyectoEmpresa>(typeof(CProyectoEmpresa), pConexion);
			foreach (CProyectoEmpresa O in Obten.ListaRegistros)
			{
				idProyectoEmpresa = O.IdProyectoEmpresa;
				fechaAlta = O.FechaAlta;
				fechaUltimaModificacion = O.FechaUltimaModificacion;
				idProyecto = O.IdProyecto;
				idEmpresa = O.IdEmpresa;
				idUsuarioAlta = O.IdUsuarioAlta;
				idUsuarioModifico = O.IdUsuarioModifico;
				idSucursal = O.IdSucursal;
				baja = O.Baja;
			}
		}
		
		public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_ProyectoEmpresa_ConsultarFiltros";
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
			Obten.Llena<CProyectoEmpresa>(typeof(CProyectoEmpresa), pConexion);
			return Obten.ListaRegistros;
		}
		
		public void Agregar(CConexion pConexion)
		{
			CConsultaAccion Agregar = new CConsultaAccion();
			Agregar.StoredProcedure.CommandText = "spb_ProyectoEmpresa_Agregar";
			Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoEmpresa", 0);
			Agregar.StoredProcedure.Parameters["@pIdProyectoEmpresa"].Direction = ParameterDirection.Output;
			if(fechaAlta.Year != 1)
			{
				Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
			}
			if(fechaUltimaModificacion.Year != 1)
			{
				Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
			}
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Agregar.Insert(pConexion);
			idProyectoEmpresa= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdProyectoEmpresa"].Value);
		}
		
		public void Editar(CConexion pConexion)
		{
			CConsultaAccion Editar = new CConsultaAccion();
			Editar.StoredProcedure.CommandText = "spb_ProyectoEmpresa_Editar";
			Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoEmpresa", idProyectoEmpresa);
			if(fechaAlta.Year != 1)
			{
				Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
			}
			if(fechaUltimaModificacion.Year != 1)
			{
				Editar.StoredProcedure.Parameters.AddWithValue("@pFechaUltimaModificacion", fechaUltimaModificacion);
			}
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", idEmpresa);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
			Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Editar.Update(pConexion);
		}
		
		public void Eliminar(CConexion pConexion)
		{
			CConsultaAccion Eliminar = new CConsultaAccion();
			Eliminar.StoredProcedure.CommandText = "spb_ProyectoEmpresa_Eliminar";
			Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
			Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdProyectoEmpresa", idProyectoEmpresa);
			Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Eliminar.Delete(pConexion);
		}
	}