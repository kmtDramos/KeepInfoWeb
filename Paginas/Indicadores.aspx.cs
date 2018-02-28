using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;

public partial class Paginas_Indicadores : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ObtenerIndicadores(int IdSucursal, int IdDivision, int IdUsuario)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_KPIS";
				Consulta.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
				Consulta.StoredProcedure.Parameters.Add("IdDivision", SqlDbType.Int).Value = IdDivision;
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					Modelo.Add("TotalProspectos", Convert.ToInt32(Consulta.Registros["TotalProspectos"]));
					Modelo.Add("Presentacion", Convert.ToInt32(Consulta.Registros["Presentacion"]));
					Modelo.Add("Preventa", Convert.ToInt32(Consulta.Registros["Preventa"]));
					Modelo.Add("Planeado", Convert.ToDecimal(Consulta.Registros["Planeado"]));
					Modelo.Add("Meta", Convert.ToDecimal(Consulta.Registros["Meta"]));
					Modelo.Add("Ventas", Convert.ToDecimal(Consulta.Registros["Ventas"]));
				}

				Consulta.CerrarConsulta();

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerTabla(int IdSucursal, int IdDivision, int IdUsuario)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_TablaKPIs";
				Consulta.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
				Consulta.StoredProcedure.Parameters.Add("IdDivision", SqlDbType.Int).Value = IdDivision;
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;

				Consulta.Llena(pConexion);

				JArray Filas = new JArray();

				while (Consulta.Registros.Read())
				{
					JObject Fila = new JObject();

					Fila.Add("Descripcion", Convert.ToString(Consulta.Registros["Descripcion"]));
					Fila.Add("Preventa", Convert.ToString(Consulta.Registros["Preventa"]));
					Fila.Add("Ventas", Convert.ToString(Consulta.Registros["Ventas"]));
					Fila.Add("Compras", Convert.ToString(Consulta.Registros["Compras"]));
					Fila.Add("Proyectos", Convert.ToString(Consulta.Registros["Proyectos"]));
					Fila.Add("Finanzas", Convert.ToString(Consulta.Registros["Finanzas"]));

					Filas.Add(Fila);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Filas", Filas);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerUsuarios()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();


				JArray Agentes = new JArray();
				CSelectEspecifico ConsultaAgentes = new CSelectEspecifico();
				ConsultaAgentes.StoredProcedure.CommandText = "sp_ObtenerVendedores";
				ConsultaAgentes.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
				ConsultaAgentes.Llena(pConexion);

				while (ConsultaAgentes.Registros.Read())
				{
					JObject Agente = new JObject();

					Agente.Add("Nombre", ConsultaAgentes.Registros["NombreCompleto"].ToString());
					Agente.Add("Valor", ConsultaAgentes.Registros["IdUsuario"].ToString());

					Agentes.Add(Agente);
				}

				ConsultaAgentes.CerrarConsulta();

				Modelo.Add("Usuarios", Agentes);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerSucursal()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();


				JArray Agentes = new JArray();
				CSelectEspecifico ConsultaAgentes = new CSelectEspecifico();
				ConsultaAgentes.StoredProcedure.CommandText = "sp_ObtenerSucursales";
				ConsultaAgentes.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
				ConsultaAgentes.Llena(pConexion);

				while (ConsultaAgentes.Registros.Read())
				{
					JObject Agente = new JObject();

					Agente.Add("Nombre", ConsultaAgentes.Registros["Sucursal"].ToString());
					Agente.Add("Valor", ConsultaAgentes.Registros["IdSucursal"].ToString());

					Agentes.Add(Agente);
				}

				ConsultaAgentes.CerrarConsulta();

				Modelo.Add("Usuarios", Agentes);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerDivision()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();


				JArray Agentes = new JArray();
				CSelectEspecifico ConsultaAgentes = new CSelectEspecifico();
				ConsultaAgentes.StoredProcedure.CommandText = "sp_ObtenerDivisiones";
				ConsultaAgentes.Llena(pConexion);

				while (ConsultaAgentes.Registros.Read())
				{
					JObject Agente = new JObject();

					Agente.Add("Nombre", ConsultaAgentes.Registros["Division"].ToString());
					Agente.Add("Valor", ConsultaAgentes.Registros["IdDivision"].ToString());

					Agentes.Add(Agente);
				}

				ConsultaAgentes.CerrarConsulta();

				Modelo.Add("Usuarios", Agentes);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

}