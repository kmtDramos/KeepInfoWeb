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

public partial class Comisiones : System.Web.UI.Page
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
	public static string ObtenerUtilidades(string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray Utilidades = new JArray();

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
				decimal TotalVentas = 0;
				decimal TotalCostos = 0;
				decimal TotalUtilidad = 0;
				decimal TotalPorcentajeUtilidad = 0;

				#region try
				try
				{
					CSelectEspecifico ConsultaUtilidades = new CSelectEspecifico();
					ConsultaUtilidades.StoredProcedure.CommandText = "sp_Reporte_Comisiones";
					ConsultaUtilidades.StoredProcedure.CommandTimeout = 180;
					ConsultaUtilidades.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
					ConsultaUtilidades.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
					ConsultaUtilidades.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
					ConsultaUtilidades.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
					ConsultaUtilidades.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

					ConsultaUtilidades.Llena(pConexion);

					while (ConsultaUtilidades.Registros.Read())
					{
						string Agente = Convert.ToString(ConsultaUtilidades.Registros["Agente"]);
						decimal Ventas = Convert.ToDecimal(ConsultaUtilidades.Registros["Ventas"]);
						decimal Costos = Convert.ToDecimal(ConsultaUtilidades.Registros["Costo"]);
						decimal Utilidad = Convert.ToDecimal(ConsultaUtilidades.Registros["Utilidad"]);
						decimal PorcentajeUtilidad = Convert.ToDecimal(ConsultaUtilidades.Registros["PorcentajeUtilidad"]);

						JObject DatosUtilidad = new JObject();
						DatosUtilidad.Add("Agente", Agente);
						DatosUtilidad.Add("Ventas", Ventas.ToString("C"));
						DatosUtilidad.Add("Costos", Costos.ToString("C"));
						DatosUtilidad.Add("Utilidad", Utilidad.ToString("C"));
						DatosUtilidad.Add("PorcentajeUtilidad", PorcentajeUtilidad * 100);

						TotalVentas += Ventas;
						TotalCostos += Costos;
						TotalUtilidad += Utilidad;

						Utilidades.Add(DatosUtilidad);
					}

					ConsultaUtilidades.CerrarConsulta();
				}
				catch (Exception ex)
				{
					DescripcionError = "<div style='width:500px'>" + ex.Message + " - " + ex.StackTrace + "</div>";
					Error = 1;
				}
				#endregion try

				TotalPorcentajeUtilidad = TotalUtilidad / TotalVentas;

				Modelo.Add("Utilidades", Utilidades);
				Modelo.Add("TotalVentas", TotalVentas);
				Modelo.Add("TotalCostos", TotalCostos);
				Modelo.Add("TotalUtilidad", TotalUtilidad);
				Modelo.Add("TotalPorcentajeUtilidad", TotalPorcentajeUtilidad * 100);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerDetalleUtilidades(string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray DetalleUtilidades = new JArray();

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				try
				{
					CSelectEspecifico ConsultaDetalleUtilidades = new CSelectEspecifico();
					ConsultaDetalleUtilidades.StoredProcedure.CommandText = "sp_Reporte_Comisiones_Detalle";
					ConsultaDetalleUtilidades.StoredProcedure.CommandTimeout = 180;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

					ConsultaDetalleUtilidades.Llena(pConexion);

					while (ConsultaDetalleUtilidades.Registros.Read())
					{
						string Cliente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Cliente"]);
						string Agente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Agente"]);
						string Folio = Convert.ToString(ConsultaDetalleUtilidades.Registros["Folio"]);
						string Clave = Convert.ToString(ConsultaDetalleUtilidades.Registros["Clave"]);
						string Descripcion = Convert.ToString(ConsultaDetalleUtilidades.Registros["Descripcion"]);
						string Cantidad = Convert.ToString(ConsultaDetalleUtilidades.Registros["Cantidad"]);
						decimal Precio = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["PrecioUnitario"]);
						decimal Total = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Total"]);
						decimal Ventas = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Venta"]);
						decimal Costos = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Costo"]);
						decimal Utilidad = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Utilidad"]);
						decimal PorcentajeUtilidad = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["PorcentajeUtilidad"]);

						JObject DatosDetalle = new JObject();
						DatosDetalle.Add("Cliente", Cliente);
						DatosDetalle.Add("Agente", Agente);
						DatosDetalle.Add("Folio", Folio);
						DatosDetalle.Add("Clave", Clave);
						DatosDetalle.Add("Descripcion", Descripcion);
						DatosDetalle.Add("Cantidad", Cantidad);
						DatosDetalle.Add("Precio", Precio.ToString("C"));
						DatosDetalle.Add("Total", Total.ToString("C"));
						DatosDetalle.Add("Ventas", Ventas.ToString("C"));
						DatosDetalle.Add("Costos", Costos.ToString("C"));
						DatosDetalle.Add("Utilidad", Utilidad.ToString("C"));
						DatosDetalle.Add("PorcentajeUtilidad", PorcentajeUtilidad * 100);

						DetalleUtilidades.Add(DatosDetalle);
					}

					ConsultaDetalleUtilidades.CerrarConsulta();
				}
				catch (Exception ex)
				{
					DescripcionError = "<div style='width:500px'>" + ex.Message + " - " + ex.StackTrace + "</div>";
					Error = 1;
				}

				Modelo.Add("DetalleUtilidades", DetalleUtilidades);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerUtilidadesFacturas(string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray DetalleUtilidades = new JArray();

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				try
				{
					CSelectEspecifico ConsultaDetalleUtilidades = new CSelectEspecifico();
					ConsultaDetalleUtilidades.StoredProcedure.CommandText = "sp_Reporte_Utilidades_Facturas";
					ConsultaDetalleUtilidades.StoredProcedure.CommandTimeout = 180;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

					ConsultaDetalleUtilidades.Llena(pConexion);

					while (ConsultaDetalleUtilidades.Registros.Read())
					{
						string Cliente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Cliente"]);
						string Agente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Agente"]);
						string Folio = Convert.ToString(ConsultaDetalleUtilidades.Registros["Folio"]);
						decimal Ventas = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Ventas"]);
						decimal Costos = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Costos"]);
						decimal Utilidad = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Utilidad"]);
						decimal PorcentajeUtilidad = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["PorcentajeUtilidad"]);

						JObject DatosDetalle = new JObject();
						DatosDetalle.Add("Cliente", Cliente);
						DatosDetalle.Add("Agente", Agente);
						DatosDetalle.Add("Folio", Folio);
						DatosDetalle.Add("Ventas", Ventas.ToString("C"));
						DatosDetalle.Add("Costos", Costos.ToString("C"));
						DatosDetalle.Add("Utilidad", Utilidad.ToString("C"));
						DatosDetalle.Add("PorcentajeUtilidad", PorcentajeUtilidad * 100);

						DetalleUtilidades.Add(DatosDetalle);
					}

					ConsultaDetalleUtilidades.CerrarConsulta();
				}
				catch (Exception ex)
				{
					DescripcionError = "<div style='width:500px'>" + ex.Message + " - " + ex.StackTrace + "</div>";
					Error = 1;
				}

				Modelo.Add("Facturas", DetalleUtilidades);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerFacturasSinCosto(string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray DetalleUtilidades = new JArray();

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				try
				{
					CSelectEspecifico ConsultaDetalleUtilidades = new CSelectEspecifico();
					ConsultaDetalleUtilidades.StoredProcedure.CommandText = "sp_Reporte_Facturas_SinCosto";
					ConsultaDetalleUtilidades.StoredProcedure.CommandTimeout = 180;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

					ConsultaDetalleUtilidades.Llena(pConexion);

					while (ConsultaDetalleUtilidades.Registros.Read())
					{
						string Cliente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Cliente"]);
						string Agente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Agente"]);
						string Folio = Convert.ToString(ConsultaDetalleUtilidades.Registros["Folio"]);
						decimal Ventas = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Ventas"]);

						JObject DatosDetalle = new JObject();
						DatosDetalle.Add("Cliente", Cliente);
						DatosDetalle.Add("Agente", Agente);
						DatosDetalle.Add("Folio", Folio);
						DatosDetalle.Add("Ventas", Ventas.ToString("C"));

						DetalleUtilidades.Add(DatosDetalle);
					}

					ConsultaDetalleUtilidades.CerrarConsulta();
				}
				catch (Exception ex)
				{
					DescripcionError = "<div style='width:500px'>" + ex.Message + " - " + ex.StackTrace + "</div>";
					Error = 1;
				}

				Modelo.Add("Facturas", DetalleUtilidades);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerDetalleSinCosto(string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				JArray DetalleUtilidades = new JArray();

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				try
				{
					CSelectEspecifico ConsultaDetalleUtilidades = new CSelectEspecifico();
					ConsultaDetalleUtilidades.StoredProcedure.CommandText = "sp_Reporte_Detalle_SinCosto";
					ConsultaDetalleUtilidades.StoredProcedure.CommandTimeout = 180;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;
					ConsultaDetalleUtilidades.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = IdEmpresa;

					ConsultaDetalleUtilidades.Llena(pConexion);

					while (ConsultaDetalleUtilidades.Registros.Read())
					{
						string Cliente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Cliente"]);
						string Agente = Convert.ToString(ConsultaDetalleUtilidades.Registros["Agente"]);
						string Folio = Convert.ToString(ConsultaDetalleUtilidades.Registros["Folio"]);
						string Clave = Convert.ToString(ConsultaDetalleUtilidades.Registros["Clave"]);
						string Descripcion = Convert.ToString(ConsultaDetalleUtilidades.Registros["Descripcion"]);
						string Cantidad = Convert.ToString(ConsultaDetalleUtilidades.Registros["Cantidad"]);
						decimal Precio = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["PrecioUnitario"]);
						decimal Total = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Total"]);
						decimal Ventas = Convert.ToDecimal(ConsultaDetalleUtilidades.Registros["Venta"]);

						JObject DatosDetalle = new JObject();
						DatosDetalle.Add("Cliente", Cliente);
						DatosDetalle.Add("Agente", Agente);
						DatosDetalle.Add("Folio", Folio);
						DatosDetalle.Add("Clave", Clave);
						DatosDetalle.Add("Descripcion", Descripcion);
						DatosDetalle.Add("Cantidad", Cantidad);
						DatosDetalle.Add("Precio", Precio.ToString("C"));
						DatosDetalle.Add("Total", Total.ToString("C"));
						DatosDetalle.Add("Ventas", Ventas.ToString("C"));

						DetalleUtilidades.Add(DatosDetalle);
					}

					ConsultaDetalleUtilidades.CerrarConsulta();
				}
				catch (Exception ex)
				{
					DescripcionError = "<div style='width:500px'>" + ex.Message + " - " + ex.StackTrace + "</div>";
					Error = 1;
				}

				Modelo.Add("DetalleSinCosto", DetalleUtilidades);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

}