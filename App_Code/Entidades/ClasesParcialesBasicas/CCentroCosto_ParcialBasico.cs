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

	public partial class CCentroCosto
	{
		//Propiedades Privadas
		private int idCentroCosto;
		private string centroCosto;
		private decimal monto;
		private DateTime fechaAlta;
		private int idUsuarioAlta;
		private string descripcion;
		private int idCuentaContable;
		private string cuentaContable;
		private bool baja;
		
		//Propiedades
		public int IdCentroCosto
		{
			get { return idCentroCosto; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idCentroCosto = value;
			}
		}
		
		public string CentroCosto
		{
			get { return centroCosto; }
			set
			{
				if (value.Length == 0)
				{
					return;
				}
				centroCosto = value;
			}
		}
		
		public decimal Monto
		{
			get { return monto; }
			set
			{
				if (value < 0)
				{
					return;
				}
				monto = value;
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
				if (value < 0)
				{
					return;
				}
				idUsuarioAlta = value;
			}
		}
		
		public string Descripcion
		{
			get { return descripcion; }
			set
			{
				if (value.Length == 0)
				{
					return;
				}
				descripcion = value;
			}
		}
		
		public int IdCuentaContable
		{
			get { return idCuentaContable; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idCuentaContable = value;
			}
		}
		
		public string CuentaContable
		{
			get { return cuentaContable; }
			set
			{
				if (value.Length == 0)
				{
					return;
				}
				cuentaContable = value;
			}
		}
		
		public bool Baja
		{
			get { return baja; }
			set { baja = value; }
		}
		
		//Constructores
		public CCentroCosto()
		{
			idCentroCosto = 0;
			centroCosto = "";
			monto = 0;
			fechaAlta = new DateTime(1, 1, 1);
			idUsuarioAlta = 0;
			descripcion = "";
			idCuentaContable = 0;
			cuentaContable = "";
			baja = false;
		}
		
		public CCentroCosto(int pIdCentroCosto)
		{
			idCentroCosto = pIdCentroCosto;
			centroCosto = "";
			monto = 0;
			fechaAlta = new DateTime(1, 1, 1);
			idUsuarioAlta = 0;
			descripcion = "";
			idCuentaContable = 0;
			cuentaContable = "";
			baja = false;
		}
		
		//Metodos Basicos
		public List<object> LlenaObjetos(CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_CentroCosto_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Llena<CCentroCosto>(typeof(CCentroCosto), pConexion);
			return Obten.ListaRegistros;
		}
		
		public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_CentroCosto_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Columnas = new string[pColumnas.Length];
			Obten.Columnas = pColumnas;
			Obten.Llena<CCentroCosto>(typeof(CCentroCosto), pConexion);
			return Obten.ListaRegistros;
		}
		
		public void LlenaObjeto(int pIdentificador, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_CentroCosto_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
			Obten.StoredProcedure.Parameters.AddWithValue("@pIdCentroCosto", pIdentificador);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Llena<CCentroCosto>(typeof(CCentroCosto), pConexion);
			foreach (CCentroCosto O in Obten.ListaRegistros)
			{
				idCentroCosto = O.IdCentroCosto;
				centroCosto = O.CentroCosto;
				monto = O.Monto;
				fechaAlta = O.FechaAlta;
				idUsuarioAlta = O.IdUsuarioAlta;
				descripcion = O.Descripcion;
				idCuentaContable = O.IdCuentaContable;
				cuentaContable = O.CuentaContable;
				baja = O.Baja;
			}
		}
		
		public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_CentroCosto_ConsultarFiltros";
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
			Obten.Llena<CCentroCosto>(typeof(CCentroCosto), pConexion);
			foreach (CCentroCosto O in Obten.ListaRegistros)
			{
				idCentroCosto = O.IdCentroCosto;
				centroCosto = O.CentroCosto;
				monto = O.Monto;
				fechaAlta = O.FechaAlta;
				idUsuarioAlta = O.IdUsuarioAlta;
				descripcion = O.Descripcion;
				idCuentaContable = O.IdCuentaContable;
				cuentaContable = O.CuentaContable;
				baja = O.Baja;
			}
		}
		
		public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_CentroCosto_ConsultarFiltros";
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
			Obten.Llena<CCentroCosto>(typeof(CCentroCosto), pConexion);
			return Obten.ListaRegistros;
		}
		
		public void Agregar(CConexion pConexion)
		{
			CConsultaAccion Agregar = new CConsultaAccion();
			Agregar.StoredProcedure.CommandText = "spb_CentroCosto_Agregar";
			Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCentroCosto", 0);
			Agregar.StoredProcedure.Parameters["@pIdCentroCosto"].Direction = ParameterDirection.Output;
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCentroCosto", centroCosto);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
			if(fechaAlta.Year != 1)
			{
				Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
			}
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Agregar.Insert(pConexion);
			idCentroCosto= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCentroCosto"].Value);
		}
		
		public void Editar(CConexion pConexion)
		{
			CConsultaAccion Editar = new CConsultaAccion();
			Editar.StoredProcedure.CommandText = "spb_CentroCosto_Editar";
			Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdCentroCosto", idCentroCosto);
			Editar.StoredProcedure.Parameters.AddWithValue("@pCentroCosto", centroCosto);
			Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
			if(fechaAlta.Year != 1)
			{
				Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
			}
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
			Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
			Editar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
			Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Editar.Update(pConexion);
		}
		
		public void Eliminar(CConexion pConexion)
		{
			CConsultaAccion Eliminar = new CConsultaAccion();
			Eliminar.StoredProcedure.CommandText = "spb_CentroCosto_Eliminar";
			Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
			Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCentroCosto", idCentroCosto);
			Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Eliminar.Delete(pConexion);
		}
	}