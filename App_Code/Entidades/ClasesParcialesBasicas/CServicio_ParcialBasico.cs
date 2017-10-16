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

	public partial class CServicio
	{
		//Propiedades Privadas
		private int idServicio;
		private string servicio;
		private int idUnidadCompraVenta;
		private decimal precio;
		private int idTipoServicio;
		private string clave;
		private int idTipoMoneda;
		private int idTipoVenta;
		private bool consumo;
		private int idUsuario;
		private DateTime fecha;
		private int idTipoIVA;
		private int idDivision;
		private bool baja;
		
		//Propiedades
		public int IdServicio
		{
			get { return idServicio; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idServicio = value;
			}
		}
		
		public string Servicio
		{
			get { return servicio; }
			set
			{
				servicio = value;
			}
		}
		
		public int IdUnidadCompraVenta
		{
			get { return idUnidadCompraVenta; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idUnidadCompraVenta = value;
			}
		}
		
		public decimal Precio
		{
			get { return precio; }
			set
			{
				if (value < 0)
				{
					return;
				}
				precio = value;
			}
		}
		
		public int IdTipoServicio
		{
			get { return idTipoServicio; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idTipoServicio = value;
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
		
		public int IdTipoMoneda
		{
			get { return idTipoMoneda; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idTipoMoneda = value;
			}
		}
		
		public int IdTipoVenta
		{
			get { return idTipoVenta; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idTipoVenta = value;
			}
		}
		
		public bool Consumo
		{
			get { return consumo; }
			set { consumo = value; }
		}
		
		public int IdUsuario
		{
			get { return idUsuario; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idUsuario = value;
			}
		}
		
		public DateTime Fecha
		{
			get { return fecha; }
			set { fecha = value; }
		}
		
		public int IdTipoIVA
		{
			get { return idTipoIVA; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idTipoIVA = value;
			}
		}
		
		public int IdDivision
		{
			get { return idDivision; }
			set
			{
				if (value < 0)
				{
					return;
				}
				idDivision = value;
			}
		}
		
		public bool Baja
		{
			get { return baja; }
			set { baja = value; }
		}
		
		//Constructores
		public CServicio()
		{
			idServicio = 0;
			servicio = "";
			idUnidadCompraVenta = 0;
			precio = 0;
			idTipoServicio = 0;
			clave = "";
			idTipoMoneda = 0;
			idTipoVenta = 0;
			consumo = false;
			idUsuario = 0;
			fecha = new DateTime(1, 1, 1);
			idTipoIVA = 0;
			idDivision = 0;
			baja = false;
		}
		
		public CServicio(int pIdServicio)
		{
			idServicio = pIdServicio;
			servicio = "";
			idUnidadCompraVenta = 0;
			precio = 0;
			idTipoServicio = 0;
			clave = "";
			idTipoMoneda = 0;
			idTipoVenta = 0;
			consumo = false;
			idUsuario = 0;
			fecha = new DateTime(1, 1, 1);
			idTipoIVA = 0;
			idDivision = 0;
			baja = false;
		}
		
		//Metodos Basicos
		public List<object> LlenaObjetos(CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_Servicio_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Llena<CServicio>(typeof(CServicio), pConexion);
			return Obten.ListaRegistros;
		}
		
		public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_Servicio_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Columnas = new string[pColumnas.Length];
			Obten.Columnas = pColumnas;
			Obten.Llena<CServicio>(typeof(CServicio), pConexion);
			return Obten.ListaRegistros;
		}
		
		public void LlenaObjeto(int pIdentificador, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_Servicio_Consultar";
			Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
			Obten.StoredProcedure.Parameters.AddWithValue("@pIdServicio", pIdentificador);
			Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
			Obten.Llena<CServicio>(typeof(CServicio), pConexion);
			foreach (CServicio O in Obten.ListaRegistros)
			{
				idServicio = O.IdServicio;
				servicio = O.Servicio;
				idUnidadCompraVenta = O.IdUnidadCompraVenta;
				precio = O.Precio;
				idTipoServicio = O.IdTipoServicio;
				clave = O.Clave;
				idTipoMoneda = O.IdTipoMoneda;
				idTipoVenta = O.IdTipoVenta;
				consumo = O.Consumo;
				idUsuario = O.IdUsuario;
				fecha = O.Fecha;
				idTipoIVA = O.IdTipoIVA;
				idDivision = O.IdDivision;
				baja = O.Baja;
			}
		}
		
		public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_Servicio_ConsultarFiltros";
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
			Obten.Llena<CServicio>(typeof(CServicio), pConexion);
			foreach (CServicio O in Obten.ListaRegistros)
			{
				idServicio = O.IdServicio;
				servicio = O.Servicio;
				idUnidadCompraVenta = O.IdUnidadCompraVenta;
				precio = O.Precio;
				idTipoServicio = O.IdTipoServicio;
				clave = O.Clave;
				idTipoMoneda = O.IdTipoMoneda;
				idTipoVenta = O.IdTipoVenta;
				consumo = O.Consumo;
				idUsuario = O.IdUsuario;
				fecha = O.Fecha;
				idTipoIVA = O.IdTipoIVA;
				idDivision = O.IdDivision;
				baja = O.Baja;
			}
		}
		
		public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
		{
			CSelect Obten = new CSelect();
			Obten.StoredProcedure.CommandText = "spb_Servicio_ConsultarFiltros";
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
			Obten.Llena<CServicio>(typeof(CServicio), pConexion);
			return Obten.ListaRegistros;
		}
		
		public void Agregar(CConexion pConexion)
		{
			CConsultaAccion Agregar = new CConsultaAccion();
			Agregar.StoredProcedure.CommandText = "spb_Servicio_Agregar";
			Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", 0);
			Agregar.StoredProcedure.Parameters["@pIdServicio"].Direction = ParameterDirection.Output;
			Agregar.StoredProcedure.Parameters.AddWithValue("@pServicio", servicio);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecio", precio);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoServicio", idTipoServicio);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoVenta", idTipoVenta);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pConsumo", consumo);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
			if(fecha.Year != 1)
			{
				Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
			}
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
			Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Agregar.Insert(pConexion);
			idServicio= Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdServicio"].Value);
		}
		
		public void Editar(CConexion pConexion)
		{
			CConsultaAccion Editar = new CConsultaAccion();
			Editar.StoredProcedure.CommandText = "spb_Servicio_Editar";
			Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
			Editar.StoredProcedure.Parameters.AddWithValue("@pServicio", servicio);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
			Editar.StoredProcedure.Parameters.AddWithValue("@pPrecio", precio);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoServicio", idTipoServicio);
			Editar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoVenta", idTipoVenta);
			Editar.StoredProcedure.Parameters.AddWithValue("@pConsumo", consumo);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
			if(fecha.Year != 1)
			{
				Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
			}
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
			Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
			Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Editar.Update(pConexion);
		}
		
		public void Eliminar(CConexion pConexion)
		{
			CConsultaAccion Eliminar = new CConsultaAccion();
			Eliminar.StoredProcedure.CommandText = "spb_Servicio_Eliminar";
			Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
			Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
			Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
			Eliminar.Delete(pConexion);
		}
	}