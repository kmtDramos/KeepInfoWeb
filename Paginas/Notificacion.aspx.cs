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
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;

public partial class Notificacion : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ObtenerNotificaciones()
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				JArray Notificaciones = new JArray();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_Notificaciones";
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = UsuarioSesion.IdUsuario;
				Consulta.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					JObject Notificacion = new JObject();

					int IdActividad = Convert.ToInt32(Consulta.Registros["IdActividad"]);
					string Actividad = Convert.ToString(Consulta.Registros["Actividad"]);
					string Cliente = Convert.ToString(Consulta.Registros["Cliente"]);
					string FechaActividad = Convert.ToString(Consulta.Registros["FechaActividad"]);
					int IdOportunidad = Convert.ToInt32(Consulta.Registros["IdOportunidad"]);
					string Oportunidad = Convert.ToString(Consulta.Registros["Oportunidad"]);
					string TipoActividad = Convert.ToString(Consulta.Registros["TipoActividad"]);

					Notificacion.Add("IdActividad", IdActividad);
					Notificacion.Add("Actividad", Actividad);
					Notificacion.Add("Cliente", Cliente);
					Notificacion.Add("FechaActividad", FechaActividad);
					Notificacion.Add("IdOportunidad", IdOportunidad);
					Notificacion.Add("Oportunidad", Oportunidad);
					Notificacion.Add("TipoActividad", TipoActividad);

					Notificaciones.Add(Notificacion);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Notificaciones", Notificaciones);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

}