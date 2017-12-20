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
    public static string ObtenerTablaProspeccionPorUsuario(string IdUsuario, string fechaInicio, string fechaFin)
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

                int IdUser = 0;
                CUsuario Usuario = new CUsuario();
                if (IdUsuario == "0")
                {
                    IdUser = UsuarioSesion.IdUsuario;
                }
                else
                {
                    IdUser = Convert.ToInt32(IdUsuario);
                }
                pParametros.Add("IdUsuario", IdUser);

                foreach (CProspeccion Prospeccion in Prospecciones.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Fila = new JObject();

                    Usuario.LlenaObjeto(IdUser, pConexion);

                    Fila.Add("IdProspeccion", Prospeccion.IdProspeccion);
                    Fila.Add("Usuario", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno.Substring(0, 4));
                    Fila.Add("Cliente", Prospeccion.Cliente);
                    Fila.Add("Correo", Prospeccion.Correo);
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
                pParametros.Add("EsVendedor", 1);

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
    public static string GuardarProspeccion(int IdProspeccion, string Cliente, string Correo, string Telefono, Object[] EstatusProspeccion)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CProspeccion Prospeccion = new CProspeccion();
                Prospeccion.LlenaObjeto(IdProspeccion, pConexion);
                Prospeccion.Cliente = Cliente;
                Prospeccion.Correo = Correo;
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
    public static string Totales(string IdUsuario)
    {
        JObject Respuesta = new JObject();
        Respuesta.Add("TotalProspectos", "TotalProspectosR");
        Respuesta.Add("DiasPromedio", "DiasPromedioR");
        Respuesta.Add("PorcentajeGanadas", "PorcentajeGanadasR");
        Respuesta.Add("PorcentajePerdidas", "PorcentajePerdidasR");
        Respuesta.Add("Error", 0);

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

                int IdUser = 0;
                CUsuario Usuario = new CUsuario();
                if (IdUsuario == "0")
                {
                    IdUser = UsuarioSesion.IdUsuario;
                }
                else
                {
                    IdUser = Convert.ToInt32(IdUsuario);
                }
                pParametros.Add("IdUsuario", IdUser);

                foreach (CProspeccion Prospeccion in Prospecciones.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Fila = new JObject();

                    Usuario.LlenaObjeto(IdUser, pConexion);

                    Fila.Add("IdProspeccion", Prospeccion.IdProspeccion);
                    Fila.Add("Usuario", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno.Substring(0, 4));
                    Fila.Add("Cliente", Prospeccion.Cliente);
                    Fila.Add("Correo", Prospeccion.Correo);
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

}