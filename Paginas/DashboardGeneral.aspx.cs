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
public partial class Paginas_DashboardGeneral : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ObtenerReporteDashboard()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_DashboardGeneralReporte";

				Consulta.Llena(pConexion);

				JArray Usuarios = new JArray();

				while (Consulta.Registros.Read())
				{
					JObject Usuario = new JObject();
					Usuario.Add("Sucursal", Convert.ToString(Consulta.Registros["Sucursal"]));
					Usuario.Add("Vendedor", Convert.ToString(Consulta.Registros["Vendedor"]));
					Usuario.Add("CantidadBaja", Convert.ToString(Consulta.Registros["CantidadBaja"]));
					Usuario.Add("CantidadMedia", Convert.ToString(Consulta.Registros["CantidadMedia"]));
					Usuario.Add("CantidadAlta", Convert.ToString(Consulta.Registros["CantidadAlta"]));
					Usuario.Add("CantidadNuevos", Convert.ToString(Consulta.Registros["CantidadNuevos"]));
					Usuario.Add("CantidadActual", Convert.ToString(Consulta.Registros["CantidadActual"]));
					Usuario.Add("CantidadTotal", Convert.ToString(Consulta.Registros["CantidadTotal"]));
					Usuario.Add("MontoBaja", Convert.ToString(Consulta.Registros["MontoBaja"]));
					Usuario.Add("MontoMedia", Convert.ToString(Consulta.Registros["MontoMedia"]));
					Usuario.Add("MontoAlta", Convert.ToString(Consulta.Registros["MontoAlta"]));
					Usuario.Add("MontoNuevos", Convert.ToString(Consulta.Registros["MontoNuevos"]));
					Usuario.Add("MontoActual", Convert.ToString(Consulta.Registros["MontoActual"]));
					Usuario.Add("MontoTotal", Convert.ToString(Consulta.Registros["MontoTotal"]));
					Usuario.Add("Colocado", Convert.ToString(Consulta.Registros["Colocado"]));
					Usuario.Add("Detenido", Convert.ToString(Consulta.Registros["Detenido"]));
					Usuario.Add("ComprasTotal", Convert.ToString(Consulta.Registros["ComprasTotal"]));
					Usuario.Add("Recibido", Convert.ToString(Consulta.Registros["Recibido"]));
					Usuario.Add("Pendiente", Convert.ToString(Consulta.Registros["Pendiente"]));
					Usuario.Add("AlmacenTotal", Convert.ToString(Consulta.Registros["AlmacenTotal"]));
					Usuario.Add("EnTiempo", Convert.ToString(Consulta.Registros["EnTiempo"]));
					Usuario.Add("FueraTiempo", Convert.ToString(Consulta.Registros["FueraTiempo"]));
					Usuario.Add("ProyectosTotal", Convert.ToString(Consulta.Registros["ProyectosTotal"]));
					Usuario.Add("Cobranza0", Convert.ToString(Consulta.Registros["Cobranza0"]));
					Usuario.Add("Cobranza30", Convert.ToString(Consulta.Registros["Cobranza30"]));
					Usuario.Add("Cobranza60", Convert.ToString(Consulta.Registros["Cobranza60"]));
					Usuario.Add("Cobranza90", Convert.ToString(Consulta.Registros["Cobranza90"]));
					Usuario.Add("CobranzaMas", Convert.ToString(Consulta.Registros["CobranzaMas"]));
					Usuario.Add("CobranzaTotal", Convert.ToString(Consulta.Registros["CobranzaTotal"]));
					Usuarios.Add(Usuario);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Usuarios", Usuarios);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});

		return Respuesta.ToString();
	}

}