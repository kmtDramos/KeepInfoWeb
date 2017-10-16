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

public partial class GraficaAvanceVenta : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ObtenerSucursales()
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
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
	public static string ObtenerDatosGraficaVentasMeta(int MostrarPor, string FechaInicial, string FechaFinal, int IdSucursal, int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//#######################################################################################################

				string Rango = "";
				switch (MostrarPor) {
					case 1:
						Rango = "Días";
						break;
					case 2:
						Rango = "Semanas";
						break;
					case 3:
						Rango = "Meses";
						break;
				}

				string Titulo = "Ventas por "+ Rango;

				JArray Ventas = new JArray();
				JArray Metas = new JArray();
				JArray Ticks = new JArray();

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				CSelectEspecifico DatosGrafica = new CSelectEspecifico();
				DatosGrafica.StoredProcedure.CommandText = "sp_Graficas_GraficaVentasMeta";
				DatosGrafica.StoredProcedure.Parameters.Add("@MostrarPor", SqlDbType.Int).Value = MostrarPor;
				DatosGrafica.StoredProcedure.Parameters.Add("@FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
				DatosGrafica.StoredProcedure.Parameters.Add("@FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
				DatosGrafica.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = IdEmpresa;
				DatosGrafica.StoredProcedure.Parameters.Add("@IdSucursal", SqlDbType.Int).Value = IdSucursal;
				DatosGrafica.StoredProcedure.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;

				DatosGrafica.Llena(pConexion);
				
				while (DatosGrafica.Registros.Read()) {

					Ticks.Add(Convert.ToString(DatosGrafica.Registros["Rango"]));
					Ventas.Add(Convert.ToDecimal(DatosGrafica.Registros["Ventas"]));
					Metas.Add(Convert.ToDecimal(DatosGrafica.Registros["Meta"]));

				}

				DatosGrafica.CerrarConsulta();

				Modelo.Add("Ventas", Ventas);
				Modelo.Add("Metas", Metas);
				Modelo.Add("Ticks", Ticks);
				Modelo.Add("Titulo", Titulo);

				//#######################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();

	}

}