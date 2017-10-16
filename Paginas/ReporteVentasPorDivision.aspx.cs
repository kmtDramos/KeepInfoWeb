using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class ReporteVentasPorDivision : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}
	
	[WebMethod]
	public static string ObtenerSucursales()
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray Sucursales = new JArray();

				CSucursalAsignada SucursalesDisponibles = new CSucursalAsignada();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
				pParametros.Add("Baja", 0);

				List<object> SucursalesAsignadas = SucursalesDisponibles.LlenaObjetosFiltros(pParametros, pConexion);

				if (SucursalesAsignadas.Count > 1)
				{
					JObject OpcionDefault = new JObject();
					OpcionDefault.Add("Valor", -1);
					OpcionDefault.Add("Descripcion", "-Todas-");
					Sucursales.Add(OpcionDefault);
				}

				foreach (CSucursalAsignada SucursalAsignada in SucursalesAsignadas)
				{
					CSucursal Sucursal = new CSucursal();
					pParametros.Clear();
					pParametros.Add("IdSucursal", SucursalAsignada.IdSucursal);
					pParametros.Add("IdEmpresa", Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]));
					Sucursal.LlenaObjetoFiltros(pParametros, pConexion);

					if (Sucursal.IdSucursal != 0)
					{
						JObject Opcion = new JObject();
						Opcion.Add("Valor", Sucursal.IdSucursal);
						Opcion.Add("Descripcion", Sucursal.Sucursal);

						Sucursales.Add(Opcion);
					}
				}

				Modelo.Add("Sucursales", Sucursales);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerAgentes(int IdSucursal)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray Agentes = new JArray();

				CUsuario AgentesSucursal = new CUsuario();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("Baja", 0);
				pParametros.Add("EsAgente", 1);
				pParametros.Add("EsVendedor", 1);
				if (IdSucursal != -1)
				{
					pParametros.Add("IdSucursalPredeterminada", IdSucursal);
				}

				JObject OpcionDefault = new JObject();
				OpcionDefault.Add("Valor", -1);
				OpcionDefault.Add("Descripcion", "-Todos-");
				Agentes.Add(OpcionDefault);

				foreach (CUsuario Agente in AgentesSucursal.LlenaObjetosFiltros(pParametros, pConexion))
				{
					JObject Opcion = new JObject();
					Opcion.Add("Valor", Agente.IdUsuario);
					Opcion.Add("Descripcion", Agente.Nombre + " " + Agente.ApellidoPaterno + " " + Agente.ApellidoMaterno);
					Agentes.Add(Opcion);
				}

				Modelo.Add("Agentes", Agentes);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerDivisiones()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CDivision Divisiones = new CDivision();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("Baja", 0);

				JArray Opciones = new JArray();

				foreach(CDivision Division in Divisiones.LlenaObjetosFiltros(pParametros, pConexion))
				{
					JObject Opcion = new JObject();
					Opcion.Add("Valor", Division.IdDivision);
					Opcion.Add("Descripcion", Division.Division);
					Opciones.Add(Opcion);
				}

				Modelo.Add("Divisiones", Opciones);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerReporteVentasPorDivision(string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario, int IdCliente, int IdDivision)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray VentasPorDivision = new JArray();
				decimal TotalVentas = 0;
				int TotalTransacciones = 0;

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				CSelectEspecifico VentasDivision = new CSelectEspecifico();
				VentasDivision.StoredProcedure.CommandTimeout = 120;
				VentasDivision.StoredProcedure.CommandText = "sp_Reporte_VentasPorDivision";
				VentasDivision.StoredProcedure.Parameters.Add("@FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
				VentasDivision.StoredProcedure.Parameters.Add("@FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
				VentasDivision.StoredProcedure.Parameters.Add("@IdSucursal", SqlDbType.Int).Value = IdSucursal;
				VentasDivision.StoredProcedure.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
				VentasDivision.StoredProcedure.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
				VentasDivision.StoredProcedure.Parameters.Add("@IdDivision", SqlDbType.Int).Value = IdDivision;
				VentasDivision.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

				VentasDivision.Llena(pConexion);

				while (VentasDivision.Registros.Read())
				{
					JObject VentaDivision = new JObject();
					decimal Venta = Convert.ToDecimal(VentasDivision.Registros["Ventas"]);
					VentaDivision.Add("Division", Convert.ToString(VentasDivision.Registros["Division"]));
					VentaDivision.Add("Ventas", Venta.ToString("C"));
					VentaDivision.Add("Transacciones", Convert.ToInt32(VentasDivision.Registros["Transacciones"]));
					VentaDivision.Add("Conversion", Convert.ToInt32(VentasDivision.Registros["Conversion"]));
					VentasPorDivision.Add(VentaDivision);
					TotalVentas += Venta;
					TotalTransacciones += Convert.ToInt32(VentasDivision.Registros["Transacciones"]);
				}

				VentasDivision.CerrarConsulta();

				Modelo.Add("VentasDivision", VentasPorDivision);
				Modelo.Add("TotalVentas", TotalVentas.ToString("C"));
				Modelo.Add("TotalTransacciones", TotalTransacciones);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerReporteVentasPorDivisionMes(string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario, int IdCliente, int IdDivision)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			
			if (Error == 0)
			{
				try
				{
					JObject Modelo = new JObject();

					JArray Divisiones = new JArray();
					JArray Columnas = new JArray();

					bool llendado = false;

					int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

					CSelectEspecifico Consulta = new CSelectEspecifico();
					Consulta.StoredProcedure.CommandTimeout = 120;
					Consulta.StoredProcedure.CommandText = "sp_ReporteVentasPorDivision_Meses";
					Consulta.StoredProcedure.Parameters.Add("@FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
					Consulta.StoredProcedure.Parameters.Add("@FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
					Consulta.StoredProcedure.Parameters.Add("@IdSucursal", SqlDbType.Int).Value = IdSucursal;
					Consulta.StoredProcedure.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
					Consulta.StoredProcedure.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
					Consulta.StoredProcedure.Parameters.Add("@IdDivision", SqlDbType.Int).Value = IdDivision;
					Consulta.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

					Consulta.Llena(pConexion);

					while (Consulta.Registros.Read())
					{
						JObject Division = new JObject();
						for (int i = 0; i < Consulta.Registros.FieldCount; i++)
						{
							if (!llendado)
							{
								Columnas.Add(Consulta.Registros.GetName(i));
							}
							Division.Add(Consulta.Registros.GetName(i), Consulta.Registros[i].ToString());
						}
						llendado = true;
						Divisiones.Add(Division);
					}

					Consulta.CerrarConsulta();

					Modelo.Add("Columnas", Columnas);
					Modelo.Add("Divisiones", Divisiones);

					Respuesta.Add("Modelo", Modelo);
				}
				catch (Exception e)
				{
					Error = 1;
					DescripcionError = e.Message +" in ("+ e.StackTrace +")";
				}
			}

			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);

		});

		return Respuesta.ToString();
	}

}