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

public partial class Paginas_Actividad : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string AgregarActividad(int IdTipoActividad, string FechaActividad, string FechaFin, int IdCliente, string Cliente, int IdOportunidad, string Actividad)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				CActividad NuevaActividad = new CActividad();
				NuevaActividad.IdUsuario = UsuarioSesion.IdUsuario;
				NuevaActividad.IdTipoActividad = IdTipoActividad;
				NuevaActividad.FechaActividad = Convert.ToDateTime(FechaActividad);
				NuevaActividad.FechaFin = Convert.ToDateTime(FechaFin);
				NuevaActividad.IdCliente = IdCliente;
				NuevaActividad.Cliente = Cliente;
				NuevaActividad.IdOportunidad = IdOportunidad;
				NuevaActividad.Actividad = Actividad;
				NuevaActividad.Agregar(pConexion);
				
				if (IdOportunidad != 0)
				{
					CBitacoraNotasOportunidad Nota = new CBitacoraNotasOportunidad();
					Nota.BitacoraNotaOportunidad = "Actividad: " + Actividad;
					Nota.IdOportunidad = IdOportunidad;
					Nota.IdUsuario = UsuarioSesion.IdUsuario;
					Nota.FechaCreacion = DateTime.Now;
					Nota.Agregar(pConexion);

					COportunidad Oportunidad = new COportunidad();
					Oportunidad.LlenaObjeto(IdOportunidad, pConexion);
					Oportunidad.UltimaNota = "Actividad: " + Actividad;
					Oportunidad.FechaNota = DateTime.Now;
					Oportunidad.Editar(pConexion);
				}

				CTipoActividad TipoActividad = new CTipoActividad();
				TipoActividad.LlenaObjeto(IdTipoActividad, pConexion);

				string Encabezado = TipoActividad.TipoActividad;
				DateTime Inicio = NuevaActividad.FechaActividad;
				long Duracion = NuevaActividad.FechaFin.Ticks - NuevaActividad.FechaActividad.Ticks;
				string NombreUsuario = UsuarioSesion.Nombre + " " + UsuarioSesion.ApellidoPaterno + " " + UsuarioSesion.ApellidoMaterno;
				string Descripcion = "<p>Buen dia "+ NombreUsuario + ":</p><p>Se ha creado una actividad de " + TipoActividad.TipoActividad + ":</p><p>" + Actividad + "</p>";
				Descripcion += "<p>Con el cliente/prospecto " + Cliente + ".</p>";
				string Location = "Asercom";
				bool TodoElDia = false;
				string From = UsuarioSesion.Correo;
				string To = UsuarioSesion.Correo;

				System.Net.Mail.Attachment Meeting = CrearMeeting(Encabezado, Inicio, Duracion, Actividad, Location, TodoElDia, From, To);

				string Subject = "Nueva actividad: "+ TipoActividad.TipoActividad;
				string path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\Paginas","");
				string Correo = CUtilerias.TextoArchivo(path + @"FormatoCorreo\CorreoActividad.html");
				Correo = Correo.Replace("[Titulo]", Encabezado);
				Correo = Correo.Replace("[Contenido]", Descripcion);

				CUtilerias.EnviarCorreoAdjunto(From, To, Subject, Correo, Meeting);

			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string BuscarCliente(string pCliente)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		CUsuario Usuario = new CUsuario();
		Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

		COrganizacion jsonRazonSocial = new COrganizacion();
		jsonRazonSocial.StoredProcedure.CommandText = "sp_Oportunidad_ConsultarFiltrosGrid";
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pCliente);
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Usuario.IdSucursalActual);
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
		respuesta = jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);

		//Cerrar Conexion
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return respuesta;
	}

	[WebMethod]
	public static string EditarActividad(int IdActividad, int IdTipoActividad, string FechaActividad, string FechaFin, int IdCliente, string Cliente, int IdOportunidad, string Actividad)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				CActividad cActividad = new CActividad();
				cActividad.LlenaObjeto(IdActividad, pConexion);
				cActividad.Actividad = Actividad;
				cActividad.Cliente = Cliente;
				cActividad.FechaActividad = Convert.ToDateTime(FechaActividad);
				cActividad.FechaFin = Convert.ToDateTime(FechaFin);
				cActividad.IdCliente = IdCliente;
				cActividad.IdOportunidad = IdOportunidad;
				cActividad.IdTipoActividad = IdTipoActividad;
				CTipoActividad TipoActividad = new CTipoActividad();
				TipoActividad.LlenaObjeto(IdTipoActividad, pConexion);
				cActividad.TipoActividad = TipoActividad.TipoActividad;
				cActividad.Editar(pConexion);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerFormaAgregarActividad(int IdCliente, int IdOportunidad)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				COportunidad Oportunidad = new COportunidad();
				Oportunidad.LlenaObjeto(IdOportunidad, pConexion);
				CCliente Cliente = new CCliente();
				IdCliente = (IdCliente == 0) ? Oportunidad.IdCliente : IdCliente;
				Cliente.LlenaObjeto(IdCliente, pConexion);
				COrganizacion Organizacion = new COrganizacion();
				Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

				Modelo.Add("IdCliente", IdCliente);
				Modelo.Add("Cliente", Organizacion.RazonSocial);
				Modelo.Add("Oportunidad", ListaOportunidades(IdCliente, IdOportunidad, pConexion));

				Dictionary<string, object> pParametros = new Dictionary<string, object>();

				CTipoActividad TiposActividad = new CTipoActividad();
				pParametros.Add("Baja", 0);

				JArray jTiposActividad = new JArray();

				foreach (CTipoActividad TipoActividad in TiposActividad.LlenaObjetosFiltros(pParametros, pConexion))
				{
					JObject jTipoActividad = new JObject();
					jTipoActividad.Add("Valor", TipoActividad.IdTipoActividad);
					jTipoActividad.Add("Descripcion", TipoActividad.TipoActividad);
					jTipoActividad.Add("Color", TipoActividad.Color);
					jTiposActividad.Add(jTipoActividad);
				}

				Modelo.Add("TipoActividad", jTiposActividad);
				DateTime f1 = DateTime.Now.AddMinutes(-DateTime.Now.Minute);
				DateTime f2 = DateTime.Now.AddMinutes(15 - DateTime.Now.Minute);
				Modelo.Add("FechaActividad", f1.ToShortDateString() + " " + f1.ToShortTimeString().Replace(".", "").Replace("a m","am"));
				Modelo.Add("FechaFin", f2.ToShortDateString() + " " + f2.ToShortTimeString().Replace(".", "").Replace("a m", "am"));

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerFormaEditarActividad(int IdActividad)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();
				CActividad Actividad = new CActividad();
				Actividad.LlenaObjeto(IdActividad, pConexion);

				string FechaActividad = Actividad.FechaActividad.ToShortDateString() + " " + Actividad.FechaActividad.ToShortTimeString().Replace(".", "");
				string FechaFin = Actividad.FechaFin.ToShortDateString() + " " + Actividad.FechaFin.ToShortTimeString().Replace(".", "");

				Modelo.Add("IdActividad", Actividad.IdActividad);
				Modelo.Add("FechaActividad", FechaActividad);
				Modelo.Add("FechaFin", FechaFin);
				Modelo.Add("Actividad", Actividad.Actividad);
				Modelo.Add("TipoActividad", ListaTiposActividades(Actividad.IdTipoActividad, pConexion));
				Modelo.Add("IdCliente", Actividad.IdCliente);
				Modelo.Add("Cliente", Actividad.Cliente);
				Modelo.Add("Oportunidad", ListaOportunidades(Actividad.IdCliente, Actividad.IdOportunidad, pConexion));
				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});
		return Respuesta.ToString();
	}

	private static JArray ListaTiposActividades(int IdTipoActividad, CConexion pConexion)
	{
		JArray TiposActividades = new JArray();

		CTipoActividad ListaTipoActividad = new CTipoActividad();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("Baja", 0);

		foreach (CTipoActividad oTipoActividad in ListaTipoActividad.LlenaObjetosFiltros(pParametros, pConexion))
		{
			JObject TipoActividad = new JObject();
			TipoActividad.Add("Descripcion", oTipoActividad.TipoActividad);
			TipoActividad.Add("Valor", oTipoActividad.IdTipoActividad);
			TipoActividad.Add("Selected", (oTipoActividad.IdTipoActividad == IdTipoActividad) ? " selected" : "");
			TipoActividad.Add("Color", oTipoActividad.Color);
			TiposActividades.Add(TipoActividad);
		}

		return TiposActividades;
	}

	private static JArray ListaOportunidades(int IdCliente, int IdOportunidad, CConexion pConexion)
	{
		JArray Oportunidades = new JArray();

		COportunidad ListaOportunidades = new COportunidad();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("Baja", 0);
		pParametros.Add("Cerrado", 0);
		pParametros.Add("IdCliente", IdCliente);

		foreach (COportunidad oOportunidad in ListaOportunidades.LlenaObjetosFiltros(pParametros, pConexion))
		{
			JObject Oportunidad = new JObject();
			Oportunidad.Add("Descripcion", oOportunidad.Oportunidad);
			Oportunidad.Add("Valor", oOportunidad.IdOportunidad);
			Oportunidad.Add("Selected", (oOportunidad.IdOportunidad == IdOportunidad) ? " selected" : "");
			Oportunidades.Add(Oportunidad);
		}

		return Oportunidades;
	}

	[WebMethod]
	public static string ObtenerActividadesCliente()
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerActividadesClienteOportunidad(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCliente, int pIdOportunidad, int pIdTipoActividad)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdActividadesClienteOportunidad", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pIdCliente", SqlDbType.VarChar, 255).Value = pIdCliente;
		Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 255).Value = pIdOportunidad;
		Stored.Parameters.Add("pIdTipoActividad", SqlDbType.VarChar, 255).Value = pIdTipoActividad;

		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

	[WebMethod]
	public static string ObtenerListaUsuarios()
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();
				CUsuario ListaUsuarios = new CUsuario();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("Baja", 0);
				pParametros.Add("EsAgente", 1);

				if (UsuarioSesion.TienePermisos(new String[] { "verActividadesAgentes" }, pConexion) != "")
				{
					pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
				}
				JArray Usuarios = new JArray();
				JObject MiUsuario = new JObject();
				MiUsuario.Add("Valor", UsuarioSesion.IdUsuario);
				MiUsuario.Add("Descripcion", UsuarioSesion.Nombre + " " + UsuarioSesion.ApellidoPaterno + " " + UsuarioSesion.ApellidoMaterno);
				MiUsuario.Add("Selected", " selected");
				Usuarios.Add(MiUsuario);

				foreach (CUsuario oUsuario in ListaUsuarios.LlenaObjetosFiltros(pParametros, pConexion))
				{
					if (oUsuario.IdUsuario != UsuarioSesion.IdUsuario)
					{
						JObject Usuario = new JObject();
						Usuario.Add("Valor", oUsuario.IdUsuario);
						Usuario.Add("Descripcion", oUsuario.Nombre + " " + oUsuario.ApellidoPaterno + " " + oUsuario.ApellidoMaterno);
						Usuario.Add("Selected", "");
						Usuarios.Add(Usuario);
					}
				}
				Modelo.Add("Usuarios", Usuarios);
				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerOportunidadesClienteActividad(int IdCliente)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();
				Modelo.Add("Oportunidades", COportunidad.ObtenerOportunidadProyecto(IdCliente, UsuarioSesion.IdUsuario, pConexion));
				Respuesta.Add(new JProperty("Modelo", Modelo));
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", Descripcion);
		});
		return Respuesta.ToString();
	}

	private static System.Net.Mail.Attachment CrearMeeting(string Encabezado, DateTime Inicio, long Duracion, string Descripcion, string Ubicacion, bool TodoElDia, string From, string To) {

		iCalendar iCal = new iCalendar { Method = "PUBLISH", Version = "2.0" };
		Event Meeting = iCal.Create<Event>();
		Meeting.Summary = Encabezado;
		Meeting.Start = new iCalDateTime(Inicio);
		Meeting.Duration = new TimeSpan(Duracion);
		Meeting.Description = Descripcion;
		Meeting.Location = Ubicacion;
		Meeting.IsAllDay = TodoElDia;
		Meeting.UID = Guid.NewGuid().ToString();
		Meeting.Organizer = new Organizer(From);

		iCalendarSerializer convertidor = new iCalendarSerializer();
		string Archivo = convertidor.SerializeToString(iCal);

		return System.Net.Mail.Attachment.CreateAttachmentFromString(Archivo, "Actividad.ics");

	}

}