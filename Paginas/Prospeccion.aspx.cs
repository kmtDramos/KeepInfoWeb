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

public partial class Paginas_Prospeccion : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ObtenerTablaProspeccion()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico ConsultarEtapasProspeccion = new CSelectEspecifico();
                ConsultarEtapasProspeccion.StoredProcedure.CommandText = "sp_EtapaProspeccion";

                ConsultarEtapasProspeccion.Llena(pConexion);

                JArray EtapasProspeccion = new JArray();

                while (ConsultarEtapasProspeccion.Registros.Read())
                {
                    JObject EtapaProspeccion = new JObject();
                    EtapaProspeccion.Add("EtapaProspeccion", Convert.ToString(ConsultarEtapasProspeccion.Registros["EtapaProspeccion"]));
                    EtapaProspeccion.Add("Colspan", Convert.ToInt32(ConsultarEtapasProspeccion.Registros["Colspan"]));
                    EtapasProspeccion.Add(EtapaProspeccion);
                }

                ConsultarEtapasProspeccion.CerrarConsulta();

                Modelo.Add("EtapasProspeccion", EtapasProspeccion);

                CSelectEspecifico ConsultarEstatusProspeccion = new CSelectEspecifico();
                ConsultarEstatusProspeccion.StoredProcedure.CommandText = "sp_EstatusProspeccion";

                ConsultarEstatusProspeccion.Llena(pConexion);

                JArray EstatusProspeccion = new JArray();

                while (ConsultarEstatusProspeccion.Registros.Read())
                {
                    JObject Estatus = new JObject();
                    Estatus.Add("EstatusProspeccion", Convert.ToString(ConsultarEstatusProspeccion.Registros["EstatusProspeccion"]));
                    EstatusProspeccion.Add(Estatus);
                }

                ConsultarEstatusProspeccion.CerrarConsulta();

                Modelo.Add("EstatusProspeccion", EstatusProspeccion);

                JArray Filas = new JArray();
                CProspeccion Prospecciones = new CProspeccion();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("Baja", 0);
                pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);

                foreach (CProspeccion Prospeccion in Prospecciones.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Fila = new JObject();

                    Fila.Add("IdProspeccion", Prospeccion.IdProspeccion);
                    Fila.Add("Usuario", UsuarioSesion.Nombre + " " + UsuarioSesion.ApellidoPaterno + " " + UsuarioSesion.ApellidoMaterno.Substring(0, 4));
                    Fila.Add("Cliente", Prospeccion.Cliente);
                    Fila.Add("Correo", Prospeccion.Correo);
                    Fila.Add("Nombre", Prospeccion.Nombre);
                    Fila.Add("Telefono", Prospeccion.Telefono);

                    if (Convert.ToInt32(Prospeccion.IdEstatusProspeccion) == 10 || Convert.ToInt32(Prospeccion.IdEstatusProspeccion) == 11)
                    {
                        Fila.Add("Dias", Dias(Convert.ToString(Prospeccion.FechaAlta), Convert.ToString(Prospeccion.FechaModificacion)));
                    }
                    else
                    {
                        Fila.Add("Dias", 0);
                    }

                    JArray Checkboxes = new JArray();

                    CEstatusProspeccionUsuario EstatusProspeccionUsuario = new CEstatusProspeccionUsuario();
                    pParametros.Clear();
                    pParametros.Add("IdProspeccion", Prospeccion.IdProspeccion);
                    float avance = 0.0f;
                    float avancePorcentaje = 0.0f;
                    foreach (CEstatusProspeccionUsuario Estatus in EstatusProspeccionUsuario.LlenaObjetosFiltros(pParametros, pConexion))
                    {
                        JObject Checkbox = new JObject();
                        Checkbox.Add("IdEstatusProspeccion", Estatus.IdEstatusProspeccion);
                        Checkbox.Add("Baja", ((Estatus.Baja) ? 1 : 0));

                        if (!Estatus.Baja)
                        {
                            avance++;
                        }

                        Checkboxes.Add(Checkbox);
                    }
                    avancePorcentaje = (((avance <= (Checkboxes.Count - 1)) ? avance : avance - 1) / (Checkboxes.Count - 1)) * 100;
                    Fila.Add("AvancePorcentaje", avancePorcentaje.ToString("0"));
                    Fila.Add("EstatusProspeccion", Checkboxes);
                    Filas.Add(Fila);

                    avance = 0;
                    avancePorcentaje = 0;
                }

                Modelo.Add("Prospecciones", Filas);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTablaProspeccionPorUsuario(string IdUsuario, string FechaInicio, string FechaFin)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico ConsultarEtapasProspeccion = new CSelectEspecifico();
                ConsultarEtapasProspeccion.StoredProcedure.CommandText = "sp_EtapaProspeccion";

                ConsultarEtapasProspeccion.Llena(pConexion);

                JArray EtapasProspeccion = new JArray();

                while (ConsultarEtapasProspeccion.Registros.Read())
                {
                    JObject EtapaProspeccion = new JObject();
                    EtapaProspeccion.Add("EtapaProspeccion", Convert.ToString(ConsultarEtapasProspeccion.Registros["EtapaProspeccion"]));
                    EtapaProspeccion.Add("Colspan", Convert.ToInt32(ConsultarEtapasProspeccion.Registros["Colspan"]));
                    EtapasProspeccion.Add(EtapaProspeccion);
                }

                ConsultarEtapasProspeccion.CerrarConsulta();

                Modelo.Add("EtapasProspeccion", EtapasProspeccion);

                CSelectEspecifico ConsultarEstatusProspeccion = new CSelectEspecifico();
                ConsultarEstatusProspeccion.StoredProcedure.CommandText = "sp_EstatusProspeccion";

                ConsultarEstatusProspeccion.Llena(pConexion);

                JArray EstatusProspeccion = new JArray();

                while (ConsultarEstatusProspeccion.Registros.Read())
                {
                    JObject Estatus = new JObject();
                    Estatus.Add("EstatusProspeccion", Convert.ToString(ConsultarEstatusProspeccion.Registros["EstatusProspeccion"]));
                    EstatusProspeccion.Add(Estatus);
                }

                ConsultarEstatusProspeccion.CerrarConsulta();

                Modelo.Add("EstatusProspeccion", EstatusProspeccion);

                JArray Filas = new JArray();
				
				JArray Divisiones = CDivision.ObtenerJsonDivisionesActivas(0,pConexion);

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_Prospeccion_ObtenerProspecciones";
				Consulta.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 20).Value = FechaInicio;
				Consulta.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 20).Value = FechaFin;
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdUsuario;

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					JObject Fila = new JObject();

					Fila.Add("IdProspeccion", Consulta.Registros["IdProspeccion"].ToString());
					Fila.Add("Dias", Consulta.Registros["Dias"].ToString());
					Fila.Add("AvancePorcentaje", Consulta.Registros["Promedio"].ToString());
					Fila.Add("Usuario", Consulta.Registros["Agente"].ToString());
					Fila.Add("IdDivision", Consulta.Registros["IdDivision"].ToString());
					Fila.Add("IdNivelInteresProspeccion", Consulta.Registros["IdNivelInteresProspeccion"].ToString());
					Fila.Add("Divisiones", Divisiones);
					Fila.Add("Cliente", Consulta.Registros["Cliente"].ToString());
					Fila.Add("Correo", Consulta.Registros["Correo"].ToString());
					Fila.Add("Nombre", Consulta.Registros["Nombre"].ToString());
					Fila.Add("Telefono", Consulta.Registros["Telefono"].ToString());
					Fila.Add("Nota", Consulta.Registros["Nota"].ToString());

					JArray Checkboxes = new JArray();

					JObject Estatus1 = new JObject();
					Estatus1.Add("IdEstatusProspeccion", 1);
					Estatus1.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus1"]));

					JObject Estatus2 = new JObject();
					Estatus2.Add("IdEstatusProspeccion", 2);
					Estatus2.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus2"]));

					JObject Estatus3 = new JObject();
					Estatus3.Add("IdEstatusProspeccion", 3);
					Estatus3.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus3"]));

					JObject Estatus4 = new JObject();
					Estatus4.Add("IdEstatusProspeccion", 4);
					Estatus4.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus4"]));

					JObject Estatus5 = new JObject();
					Estatus5.Add("IdEstatusProspeccion", 5);
					Estatus5.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus5"]));

					JObject Estatus6 = new JObject();
					Estatus6.Add("IdEstatusProspeccion", 6);
					Estatus6.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus6"]));

					JObject Estatus7 = new JObject();
					Estatus7.Add("IdEstatusProspeccion", 7);
					Estatus7.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus7"]));

					JObject Estatus8 = new JObject();
					Estatus8.Add("IdEstatusProspeccion", 8);
					Estatus8.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus8"]));

					JObject Estatus9 = new JObject();
					Estatus9.Add("IdEstatusProspeccion", 9);
					Estatus9.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus9"]));

					JObject Estatus10 = new JObject();
					Estatus10.Add("IdEstatusProspeccion", 10);
					Estatus10.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus10"]));

					JObject Estatus11 = new JObject();
					Estatus11.Add("IdEstatusProspeccion", 11);
					Estatus11.Add("Baja", Convert.ToInt32(Consulta.Registros["Estatus11"]));

					Checkboxes.Add(Estatus1);
					Checkboxes.Add(Estatus2);
					Checkboxes.Add(Estatus3);
					Checkboxes.Add(Estatus4);
					Checkboxes.Add(Estatus5);
					Checkboxes.Add(Estatus6);
					//Checkboxes.Add(Estatus7);
					//Checkboxes.Add(Estatus8);
					//Checkboxes.Add(Estatus9);
					Checkboxes.Add(Estatus10);
					Checkboxes.Add(Estatus11);

					Fila.Add("EstatusProspeccion", Checkboxes);
					Filas.Add(Fila);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Prospecciones", Filas);

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

                CUsuario Usuarios = new CUsuario();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("Baja", 0);
                pParametros.Add("EsAgente", 1);

                JArray Opciones = new JArray();

                foreach (CUsuario Usuario in Usuarios.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Opcion = new JObject();
                    Opcion.Add("Valor", Usuario.IdUsuario);
                    Opcion.Add("Nombre", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
                    Opciones.Add(Opcion);
                }

                Modelo.Add("Usuarios", Opciones);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    private static string Dias(string fInicial, string fFinal)
    {
        string[] init = fInicial.Split(' ');
        string[] end = fFinal.Split(' ');

        DateTime initDate = DateTime.Parse(init[0]);
        DateTime endDate = DateTime.Parse(end[0]);

        return Convert.ToString((endDate - initDate).TotalDays);
    }

    [WebMethod]
    public static string ObtenerAgregarFilaProspeccion()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico ConsultarEstatusProspeccion = new CSelectEspecifico();
                ConsultarEstatusProspeccion.StoredProcedure.CommandText = "sp_EstatusProspeccion";

                ConsultarEstatusProspeccion.Llena(pConexion);

                JArray EstatusProspeccion = new JArray();

                while (ConsultarEstatusProspeccion.Registros.Read())
                {
                    JObject Estatus = new JObject();
                    Estatus.Add("IdEstatusProspeccion", Convert.ToInt32(ConsultarEstatusProspeccion.Registros["IdEstatusProspeccion"]));
                    Estatus.Add("EstatusProspeccion", Convert.ToString(ConsultarEstatusProspeccion.Registros["EstatusProspeccion"]));
                    EstatusProspeccion.Add(Estatus);
                }

                ConsultarEstatusProspeccion.CerrarConsulta();

				JArray Divisiones = CDivision.ObtenerJsonDivisionesActivas(0, pConexion);
				Modelo.Add("Divisiones", Divisiones);

                Modelo.Add("Usuario", UsuarioSesion.Nombre + ' ' + UsuarioSesion.ApellidoPaterno + ' ' + UsuarioSesion.ApellidoMaterno.Substring(0, 4));
                Modelo.Add("EstatusProspeccion", EstatusProspeccion);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string GuardarProspeccion(int IdProspeccion, string Cliente, string Correo, string Nombre, string Telefono, Object[] EstatusProspeccion, int IdDivision, int IdNivelInteresProspeccion)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CProspeccion Prospeccion = new CProspeccion();
                Prospeccion.LlenaObjeto(IdProspeccion, pConexion);
				Prospeccion.IdDivision = IdDivision;
				Prospeccion.IdNivelInteresProspeccion = IdNivelInteresProspeccion;
                Prospeccion.Cliente = Cliente;
                Prospeccion.Correo = Correo;
                Prospeccion.Nombre = Nombre;
                Prospeccion.Telefono = Telefono;

                if (Prospeccion.IdProspeccion == 0)
                {
                    Prospeccion.FechaAlta = DateTime.Now;
                    Prospeccion.IdUsuario = UsuarioSesion.IdUsuario;
                    Prospeccion.Agregar(pConexion);
                }
                else
                {
                    Prospeccion.FechaModificacion = DateTime.Now;
                    Prospeccion.Editar(pConexion);
                }

                foreach (Dictionary<string, object> Estatus in EstatusProspeccion)
                {

                    CEstatusProspeccionUsuario EstatusUsuario = new CEstatusProspeccionUsuario();

                    Dictionary<string, object> pParametros = new Dictionary<string, object>();
                    pParametros.Add("IdProspeccion", Prospeccion.IdProspeccion);
                    pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
                    pParametros.Add("IdEstatusProspeccion", Estatus["IdEstatusProspeccion"]);

                    EstatusUsuario.LlenaObjetoFiltros(pParametros, pConexion);

                    if (EstatusUsuario.IdEstatusProspeccionUsuario == 0)
                    {
                        EstatusUsuario.IdUsuario = UsuarioSesion.IdUsuario;
                        EstatusUsuario.IdEstatusProspeccion = Convert.ToInt32(Estatus["IdEstatusProspeccion"]);
                        EstatusUsuario.IdProspeccion = Prospeccion.IdProspeccion;
                        EstatusUsuario.FechaAlta = DateTime.Now;
                        EstatusUsuario.Baja = Convert.ToBoolean(Estatus["Baja"]);
                        EstatusUsuario.Agregar(pConexion);
                    }
                    else
                    {
                        if (EstatusUsuario.Baja != Convert.ToBoolean(Estatus["Baja"]))
                        {
                            EstatusUsuario.IdUsuario = UsuarioSesion.IdUsuario;
                            EstatusUsuario.Baja = Convert.ToBoolean(Estatus["Baja"]);
                            EstatusUsuario.FechaAlta = DateTime.Now;
                            EstatusUsuario.Editar(pConexion);
                        }
                    }
                    if (Convert.ToBoolean(Estatus["Baja"]) == false)
                    {
                        Prospeccion.IdEstatusProspeccion = Convert.ToInt32(Estatus["IdEstatusProspeccion"]);
                        Prospeccion.IdEstatusProspeccionUsuario = EstatusUsuario.IdEstatusProspeccionUsuario;
                        Prospeccion.Editar(pConexion);
                    }

                }

                Modelo.Add("IdProspeccion", Prospeccion.IdProspeccion);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EliminarProspeccion(int IdProspeccion)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                CProspeccion Prospeccion = new CProspeccion();
                Prospeccion.LlenaObjeto(IdProspeccion, pConexion);
                Prospeccion.Baja = !Prospeccion.Baja;
                Prospeccion.Editar(pConexion);
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
    public static string Totales(string IdUsuario, string FechaInicial, string FechaFinal)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                int IdUser = 0;
                CUsuario Usuario = new CUsuario();
                if (IdUsuario == "0")
                {
                    IdUser = UsuarioSesion.IdUsuario;
                }
                else if (IdUsuario == "-1")
                {
                    IdUser = 0;
                }
                else
                {
                    IdUser = Convert.ToInt32(IdUsuario);
                }

                CSelectEspecifico ConsultarProspeccionTotales = new CSelectEspecifico();
                ConsultarProspeccionTotales.StoredProcedure.CommandText = "sp_Prospeccion_Totales";
                ConsultarProspeccionTotales.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
                ConsultarProspeccionTotales.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;
                ConsultarProspeccionTotales.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.VarChar, 50).Value = IdUser;

                JArray Totales = CUtilerias.ObtenerConsulta(ConsultarProspeccionTotales, pConexion);

                Modelo.Add("Totales", Totales);

                Respuesta.Add("Modelo", Modelo);
                
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

	[WebMethod]
	public static string ObtenerNotasProspeccion(int IdProspeccion)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				JArray Notas = new JArray();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_Prospeccion_Notas";
				Consulta.StoredProcedure.Parameters.Add("IdProspeccion", SqlDbType.Int).Value = IdProspeccion;

				Consulta.Llena(pConexion);

				while (Consulta.Registros.Read())
				{
					JObject Nota = new JObject();

					Nota.Add("IdNotaProspeccion", Consulta.Registros["IdNotaProspeccion"].ToString());
					Nota.Add("Nota", Consulta.Registros["Nota"].ToString());
					Nota.Add("FechaAlta", Consulta.Registros["FechaAlta"].ToString());
					Nota.Add("Agente", Consulta.Registros["Agente"].ToString());

					Notas.Add(Nota);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("IdProspeccion", IdProspeccion);
				Modelo.Add("Notas", Notas);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	[WebMethod]
	public static string GuardarNota(int IdProspeccion, string Nota)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CProspeccion Prospeccion = new CProspeccion();
				Prospeccion.LlenaObjeto(IdProspeccion, pConexion);

				CNotaProspeccion NotaProspeccion = new CNotaProspeccion();
				NotaProspeccion.Nota = Nota;
				NotaProspeccion.IdProspeccion = IdProspeccion;
				NotaProspeccion.IdUsuario = UsuarioSesion.IdUsuario;
				NotaProspeccion.FechaAlta = DateTime.Now;
				NotaProspeccion.Agregar(pConexion);

				Prospeccion.Nota = Nota;
				Prospeccion.Editar(pConexion);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}
}