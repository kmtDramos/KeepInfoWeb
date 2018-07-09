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
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using arquitecturaNet;
using System.IO;
using System.Diagnostics;

public partial class SolicitudLevantamiento : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    private static int idEmpresa;
    private static string logoEmpresa;
    public static int puedeAgregarCotizacion = 0;
    public static int puedeEditarCotizacion = 0;
    public static int puedeEliminarCotizacion = 0;
    public static int puedeConsultarCotizacion = 0;
    public static int puedeGenerarPedido = 0;
    public static int puedeEditarVigenciaCotizacion = 0;
    public static int puedePasarPedidoACotizado = 0;
    public static int puedeDarMantenimiento = 0;
    public string ticks = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ticks = DateTime.Now.Ticks.ToString();

        //GridLevantamiento
        CJQGrid GridSolicitudLevantamiento = new CJQGrid();
        GridSolicitudLevantamiento.NombreTabla = "grdSolicitudLevantamiento";
        GridSolicitudLevantamiento.CampoIdentificador = "IdSolicitudLevantamiento";
        GridSolicitudLevantamiento.TipoOrdenacion = "DESC";
        GridSolicitudLevantamiento.ColumnaOrdenacion = "IdSolicitudLevantamiento";
        GridSolicitudLevantamiento.Metodo = "ObtenerSolicitudLevantamiento";
        GridSolicitudLevantamiento.TituloTabla = "Catálogo de Solicitudes de Levantamientos";
        GridSolicitudLevantamiento.GenerarFuncionFiltro = false;

        //IdSolicitudLevantamiento
        CJQColumn ColIdSolicitudLevantamiento = new CJQColumn();
        ColIdSolicitudLevantamiento.Nombre = "IdSolicitudLevantamiento";
        ColIdSolicitudLevantamiento.Oculto = "false";
        ColIdSolicitudLevantamiento.Encabezado = "Folio";
        ColIdSolicitudLevantamiento.Buscador = "false";
        ColIdSolicitudLevantamiento.Ancho = "50";
        GridSolicitudLevantamiento.Columnas.Add(ColIdSolicitudLevantamiento);
        /*
        //NoFolio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        ColFolio.Ancho = "50";
        GridSolicitudLevantamiento.Columnas.Add(ColFolio);
        */
        //Razon Social
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "200";
        GridSolicitudLevantamiento.Columnas.Add(ColRazonSocial);

        //Oportunidad
        CJQColumn ColOportunidad = new CJQColumn();
        ColOportunidad.Nombre = "IdOportunidad";
        ColOportunidad.Encabezado = "Oportunidad";
        ColOportunidad.Buscador = "true";
        ColOportunidad.Alineacion = "left";
        ColOportunidad.Ancho = "50";
        GridSolicitudLevantamiento.Columnas.Add(ColOportunidad);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "FechaAlta";
        ColFecha.Encabezado = "Alta";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridSolicitudLevantamiento.Columnas.Add(ColFecha);

        //Asignado
        CJQColumn ColAsignado = new CJQColumn();
        ColAsignado.Nombre = "Asignado";
        ColAsignado.Encabezado = "Asignado";
        ColAsignado.Buscador = "true";
        ColAsignado.Alineacion = "left";
        ColAsignado.Ancho = "50";
        GridSolicitudLevantamiento.Columnas.Add(ColAsignado);

        //Fecha Cita
        CJQColumn ColFechaCita = new CJQColumn();
        ColFechaCita.Nombre = "CitaFechaHora";
        ColFechaCita.Encabezado = "Fecha Cita";
        ColFechaCita.Buscador = "false";
        ColFechaCita.Alineacion = "left";
        ColFechaCita.Ancho = "80";
        GridSolicitudLevantamiento.Columnas.Add(ColFechaCita);

        //Confirmado
        CJQColumn ColConfirmado = new CJQColumn();
        ColConfirmado.Nombre = "Confirmado";
        ColConfirmado.Encabezado = "Confirmado";
        ColConfirmado.Buscador = "false";
        ColConfirmado.Alineacion = "left";
        ColConfirmado.Ancho = "50";
        GridSolicitudLevantamiento.Columnas.Add(ColConfirmado);


        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "70";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBaja.Oculto = "true";
        GridSolicitudLevantamiento.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarSolicitudLevantamiento";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridSolicitudLevantamiento.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSolicitudLevantamiento", GridSolicitudLevantamiento.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudLevantamiento(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, int pAI, string pTipoOrden, string pRazonSocial, string pFolio, string pIdOportunidad, string pFechaInicial, string pFechaFinal, int pPorFecha, int pConfirmado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSolicitudLevantamiento", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdOportunidad);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pConfirmado", SqlDbType.Int).Value = pConfirmado;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaFiltroSolicitudLevantamiento()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;

        Modelo.Add("FechaInicial", Convert.ToString(Fecha.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(Fecha.ToShortDateString()));


        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaSoliciudLevantamiento(int pIdSolicitudLevantamiento)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();
                Dictionary<string, object> Parametros = new Dictionary<string, object>();

                //Solicitud de Levantamiento
                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                Parametros.Clear();
                Parametros.Add("Baja", 0);
                Parametros.Add("IdSolicitudLevantamiento", pIdSolicitudLevantamiento);
                solicitudLevantamiento.LlenaObjetoFiltros(Parametros, pConexion);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudLevantamiento.IdOportunidad, pConexion);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(oportunidad.IdUsuarioCreacion, pConexion);
                string Nombre = Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno;

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(solicitudLevantamiento.IdCliente, pConexion);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add(new JProperty("Agente", Nombre));
                CUsuario asignado = new CUsuario();
                CDivision division = new CDivision();

                Modelo.Add(new JProperty("IdSolLevantamiento", solicitudLevantamiento.IdSolicitudLevantamiento));
                Modelo.Add(new JProperty("FolioSolicitud", solicitudLevantamiento.IdSolicitudLevantamiento));

                Modelo.Add(new JProperty("FechaAlta", solicitudLevantamiento.FechaAlta.ToShortDateString()));

                Modelo.Add(new JProperty("CitaFechaHora", solicitudLevantamiento.CitaFechaHora.ToShortDateString() + " " + solicitudLevantamiento.CitaFechaHora.ToShortTimeString().Replace(".", "").Replace("a m", "am").Replace("p m", "pm")));

                Modelo.Add(new JProperty("Oportunidad", oportunidad.Oportunidad));
                Modelo.Add(new JProperty("Cliente", organizacion.RazonSocial));

                division.LlenaObjeto(solicitudLevantamiento.IdDivision, pConexion);
                Modelo.Add(new JProperty("Especialidad", division.Division));
                asignado.LlenaObjeto(solicitudLevantamiento.IdUsuarioAsignado, pConexion);
                Modelo.Add(new JProperty("Asignado", asignado.Nombre + " " + asignado.ApellidoPaterno + " " + asignado.ApellidoMaterno));
                Modelo.Add(new JProperty("idUsuarioAsignado", solicitudLevantamiento.IdUsuarioAsignado));

                Modelo.Add(new JProperty("ContactoDirecto", solicitudLevantamiento.ContactoDirecto));
                Modelo.Add(new JProperty("IdContactoDirectoPuesto", solicitudLevantamiento.IdPuestoContactoDirecto));
                Modelo.Add(new JProperty("ContactoDirectoPuesto", ObtenerPuestoContacto(pConexion)));

                Modelo.Add(new JProperty("Externo", solicitudLevantamiento.Externo));

                Modelo.Add(new JProperty("ContactoEnSitio", solicitudLevantamiento.ContactoEnSitio));
                Modelo.Add(new JProperty("IdContactoSitioPuesto", solicitudLevantamiento.IdPuestoContactoEnSitio));
                Modelo.Add(new JProperty("ContactoSitioPuesto", ObtenerPuestoContacto(pConexion)));
                
                Modelo.Add(new JProperty("Telefonos", solicitudLevantamiento.Telefonos));
                //Modelo.Add(new JProperty("HoraCliente", solicitudLevantamiento.HoraAtencionCliente));

                Modelo.Add(new JProperty("PermisoIngresarSitio", solicitudLevantamiento.PermisoIngresarSitio));
                Modelo.Add(new JProperty("EquipoSeguridadIngresarSitio", solicitudLevantamiento.EquipoSeguridadIngresarSitio));
                Modelo.Add(new JProperty("ClienteCuentaEstacionamiento", solicitudLevantamiento.ClienteCuentaEstacionamiento));
                Modelo.Add(new JProperty("ClienteCuentaPlanoLevantamiento", solicitudLevantamiento.ClienteCuentaPlanoLevantamiento));

                Modelo.Add(new JProperty("Domicilio", solicitudLevantamiento.Domicilio));
                Modelo.Add(new JProperty("Descripcion", solicitudLevantamiento.Descripcion));
                Modelo.Add(new JProperty("Notas", solicitudLevantamiento.Notas));

                Modelo.Add(new JProperty("ConfirmarSolicitud", solicitudLevantamiento.ConfirmarSolicitud));
                Modelo.Add(new JProperty("LevantamientoCreado", solicitudLevantamiento.LevantamientoCreado));

                Respuesta.Add("Modelo", Modelo);
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    public static JArray ObtenerPuestoContacto(CConexion Conexion)
    {
        JArray JPuestosContactos = new JArray();
        CPuestoContacto puestoContacto = new CPuestoContacto();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CPuestoContacto oPuestoContacto in puestoContacto.LlenaObjetosFiltros(Parametros, Conexion))
        {
            JObject JPuestoContacto = new JObject();
            JPuestoContacto.Add("Valor", oPuestoContacto.IdPuestoContacto);
            JPuestoContacto.Add("Descripcion", oPuestoContacto.Descripcion);

            JPuestosContactos.Add(JPuestoContacto);
        }
        return JPuestosContactos;
    }

    [WebMethod]
    public static string AgregarSolicitudLevantamiento(string FechaAlta, string CitaFechaHora, int IdOportunidad, int IdCliente, int IdAgente, int IdAsignado, string ContactoDirecto, int ContactoDirectoPuesto, int Externo, string ContactoEnSitio, int ContactoEnSitioPuesto, string Telefonos, int PermisoIngresarSitio, int EquipoSeguridadIngresarSitio, int ClienteCuentaEstacionamiento, int ClienteCuentaPlanoLevantamiento, string Domicilio, string Descripcion, string Notas, int Confirmacion)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(IdOportunidad, pConexion);

                solicitudLevantamiento.FechaAlta = Convert.ToDateTime(FechaAlta);
                //solicitudLevantamiento.FechaCita = Convert.ToDateTime(FechaCita);
                solicitudLevantamiento.CitaFechaHora = Convert.ToDateTime(CitaFechaHora);
                solicitudLevantamiento.IdOportunidad = IdOportunidad;
                solicitudLevantamiento.IdCliente = IdCliente;
                solicitudLevantamiento.IdAgente = IdAgente;
                solicitudLevantamiento.IdUsuarioAsignado = (Externo != 0) ? 0 : IdAsignado;
                solicitudLevantamiento.ContactoDirecto = ContactoDirecto;
                solicitudLevantamiento.IdPuestoContactoDirecto = ContactoDirectoPuesto;
                solicitudLevantamiento.Externo = Convert.ToBoolean(Externo);
                solicitudLevantamiento.ContactoEnSitio = ContactoEnSitio;
                solicitudLevantamiento.IdPuestoContactoEnSitio = ContactoEnSitioPuesto;
                solicitudLevantamiento.Telefonos = Telefonos;
                //solicitudLevantamiento.HoraAtencionCliente = HoraCliente;
                solicitudLevantamiento.PermisoIngresarSitio = Convert.ToBoolean(PermisoIngresarSitio);
                solicitudLevantamiento.EquipoSeguridadIngresarSitio = Convert.ToBoolean(EquipoSeguridadIngresarSitio);
                solicitudLevantamiento.ClienteCuentaEstacionamiento = Convert.ToBoolean(ClienteCuentaEstacionamiento);
                solicitudLevantamiento.ClienteCuentaPlanoLevantamiento = Convert.ToBoolean(ClienteCuentaPlanoLevantamiento);
                solicitudLevantamiento.Domicilio = Domicilio;
                solicitudLevantamiento.IdDivision = oportunidad.IdDivision;
                solicitudLevantamiento.Descripcion = Descripcion;
                solicitudLevantamiento.Notas = Notas;
                solicitudLevantamiento.IdCreador = UsuarioSesion.IdUsuario;
                solicitudLevantamiento.ConfirmarSolicitud = Convert.ToBoolean(Confirmacion);
                solicitudLevantamiento.Agregar(pConexion);

                Respuesta.Add("IdSolLevantamiento", solicitudLevantamiento.IdSolicitudLevantamiento);

                if ((UsuarioSesion.IdUsuario == 95 || UsuarioSesion.IdUsuario == 215 || UsuarioSesion.IdUsuario == 26 || UsuarioSesion.IdUsuario == 93 || UsuarioSesion.IdUsuario == 202))
                {

                    CSelectEspecifico disponible = new CSelectEspecifico();
                    disponible.StoredProcedure.CommandText = "sp_SolicitudLevantamiento_Disponibilidad";
                    disponible.StoredProcedure.Parameters.Add("Fecha", SqlDbType.Int).Value = Convert.ToDateTime(CitaFechaHora);
                    disponible.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdAsignado;
                    //Respuesta.Add("DISPONIBLE", CUtilerias.ObtenerConsulta(disponible, pConexion));

                    //if (CUtilerias.ObtenerConsulta(disponible, pConexion))
                    //{

                    if (Confirmacion != 0)
                        if (Externo == 0)
                            enviarCorreo(solicitudLevantamiento.IdSolicitudLevantamiento, pConexion);

                    Error = 0;
                    DescripcionError = "Se ha guardado con éxito.";
                    //}
                    //else
                    //{
                    //    Error = 1;
                    //    DescripcionError = "El Usuario Asignado ya cuenta con un levantamiento en esta hora aproximada.";
                    //}

                }
                else
                {
                    Error = 1;
                    DescripcionError = "Solo los administradores pueden confirmar la solicitud.";
                }

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarSolicitudLevantamiento(int IdSolLevantamiento, string CitaFechaHora, int IdOportunidad, int IdCliente, int IdAgente, int IdAsignado, string ContactoDirecto, int ContactoDirectoPuesto, int Externo, string ContactoEnSitio, int ContactoEnSitioPuesto, string Telefonos, int PermisoIngresarSitio, int EquipoSeguridadIngresarSitio, int ClienteCuentaEstacionamiento, int ClienteCuentaPlanoLevantamiento, string Domicilio, string Descripcion, string Notas, int Confirmacion)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {

                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                solicitudLevantamiento.LlenaObjeto(IdSolLevantamiento, pConexion);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(IdOportunidad, pConexion);

                //solicitudLevantamiento.FechaAlta = Convert.ToDateTime(FechaAlta);
                //solicitudLevantamiento.FechaCita = Convert.ToDateTime(FechaCita);
                solicitudLevantamiento.CitaFechaHora = Convert.ToDateTime(CitaFechaHora);
                solicitudLevantamiento.IdOportunidad = IdOportunidad;
                solicitudLevantamiento.IdCliente = IdCliente;
                solicitudLevantamiento.IdAgente = IdAgente;
                solicitudLevantamiento.IdUsuarioAsignado = (Externo != 0) ? 0 : IdAsignado;
                solicitudLevantamiento.ContactoDirecto = ContactoDirecto;
                solicitudLevantamiento.IdPuestoContactoDirecto = ContactoDirectoPuesto;
                solicitudLevantamiento.Externo = Convert.ToBoolean(Externo);
                solicitudLevantamiento.ContactoEnSitio = ContactoEnSitio;
                solicitudLevantamiento.IdPuestoContactoEnSitio = ContactoEnSitioPuesto;
                solicitudLevantamiento.Telefonos = Telefonos;
                //solicitudLevantamiento.HoraAtencionCliente = HoraCliente;
                solicitudLevantamiento.PermisoIngresarSitio = Convert.ToBoolean(PermisoIngresarSitio);
                solicitudLevantamiento.EquipoSeguridadIngresarSitio = Convert.ToBoolean(EquipoSeguridadIngresarSitio);
                solicitudLevantamiento.ClienteCuentaEstacionamiento = Convert.ToBoolean(ClienteCuentaEstacionamiento);
                solicitudLevantamiento.ClienteCuentaPlanoLevantamiento = Convert.ToBoolean(ClienteCuentaPlanoLevantamiento);
                solicitudLevantamiento.Domicilio = Domicilio;
                solicitudLevantamiento.IdDivision = oportunidad.IdDivision;
                solicitudLevantamiento.Descripcion = Descripcion;
                solicitudLevantamiento.Notas = Notas;
                solicitudLevantamiento.ConfirmarSolicitud = Convert.ToBoolean(Confirmacion);
                solicitudLevantamiento.Editar(pConexion);

                if ((UsuarioSesion.IdUsuario == 95 || UsuarioSesion.IdUsuario == 215 || UsuarioSesion.IdUsuario == 26 || UsuarioSesion.IdUsuario == 93 || UsuarioSesion.IdUsuario == 202))
                {
                    CSelectEspecifico disponible = new CSelectEspecifico();
                    disponible.StoredProcedure.CommandText = "sp_SolicitudLevantamiento_Disponibilidad";
                    disponible.StoredProcedure.Parameters.Add("Fecha", SqlDbType.DateTime).Value = Convert.ToDateTime(CitaFechaHora);
                    disponible.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = IdAsignado;
                    //Respuesta.Add("disponibilidad", CUtilerias.ObtenerConsulta(disponible, pConexion));
                    Respuesta.Add("fecha", Convert.ToDateTime(CitaFechaHora));

                    //if (CUtilerias.ObtenerConsulta(disponible, pConexion))
                    //{

                    if (Confirmacion != 0)
                        if (Externo == 0)
                            enviarCorreo(solicitudLevantamiento.IdSolicitudLevantamiento, pConexion);

                    Error = 0;
                    DescripcionError = "Se ha editado con éxito.";
                    //}
                    //else
                    //{
                    //    Error = 1;
                    //    DescripcionError = "El Usuario Asignado ya cuenta con un levantamiento en esta hora aproximada.";
                    //}
                }
                else
                {
                    Error = 1;
                    DescripcionError = "Solo los administradores pueden confirmar la solicitud.";
                }
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ImprimirSolLevantamiento(int IdSolLevantamiento)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]), pConexion);

                CMunicipio MunicipioE = new CMunicipio();
                MunicipioE.LlenaObjeto(Empresa.IdMunicipio, pConexion);

                CEstado EstadoE = new CEstado();
                EstadoE.LlenaObjeto(MunicipioE.IdEstado, pConexion);

                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                solicitudLevantamiento.LlenaObjeto(IdSolLevantamiento, pConexion);

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(solicitudLevantamiento.IdCliente, pConexion);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

                CUsuario Agente = new CUsuario();
                Agente.LlenaObjeto(solicitudLevantamiento.IdAgente, pConexion);

                CUsuario Asignado = new CUsuario();
                Asignado.LlenaObjeto(solicitudLevantamiento.IdUsuarioAsignado, pConexion);

                CPuestoContacto contactoDirectoPuesto = new CPuestoContacto();
                contactoDirectoPuesto.LlenaObjeto(solicitudLevantamiento.IdPuestoContactoDirecto, pConexion);

                CPuestoContacto contactoEnSitioPuesto = new CPuestoContacto();
                contactoEnSitioPuesto.LlenaObjeto(solicitudLevantamiento.IdPuestoContactoEnSitio, pConexion);

                CDivision division = new CDivision();
                division.LlenaObjeto(solicitudLevantamiento.IdDivision, pConexion);

                Modelo.Add("FOLIO", solicitudLevantamiento.IdSolicitudLevantamiento);

                Modelo.Add("RAZONSOCIALEMISOR", Empresa.RazonSocial);
                Modelo.Add("RFCEMISOR", Empresa.RFC);
                Modelo.Add("IMAGEN_LOGO", Empresa.Logo);
                Modelo.Add("CALLEEMISOR", Empresa.Calle);
                Modelo.Add("NUMEROEXTERIOREMISOR", Empresa.NumeroExterior);
                Modelo.Add("COLONIAEMISOR", Empresa.Colonia);
                Modelo.Add("CODIGOPOSTALEMISOR", Empresa.CodigoPostal);
                Modelo.Add("MUNICIPIOEMISOR", MunicipioE.Municipio);
                Modelo.Add("ESTADOEMISOR", EstadoE.Estado);

                Modelo.Add("FECHAALTA", solicitudLevantamiento.FechaAlta.ToShortDateString());
                Modelo.Add("IDOPORTUNIDAD", solicitudLevantamiento.IdOportunidad);
                Modelo.Add("FECHACITA", solicitudLevantamiento.CitaFechaHora);
                Modelo.Add("ESPECIALDIAD", division.Division);
                Modelo.Add("RAZONSOCIALRECEPTOR", Organizacion.RazonSocial);
                Modelo.Add("AGENTE", Agente.Nombre + " " + Agente.ApellidoPaterno + " " + Agente.ApellidoMaterno);
                Modelo.Add("ASIGNADO", Asignado.Nombre + " " + Asignado.ApellidoPaterno + " " + Asignado.ApellidoMaterno);
                Modelo.Add("CONTACTODIRECTO", solicitudLevantamiento.ContactoDirecto);
                Modelo.Add("CONTACTODIRECTOPUESTO", contactoDirectoPuesto.Descripcion);
                Modelo.Add("ESASOCIADO", (Convert.ToInt32(solicitudLevantamiento.Externo) == 0) ? "NO" : "SI");
                Modelo.Add("CONTACTOENSITIO", solicitudLevantamiento.ContactoEnSitio);
                Modelo.Add("CONTACTOENSITIOPUESTO", contactoEnSitioPuesto.Descripcion);
                Modelo.Add("TELEFONOS", solicitudLevantamiento.Telefonos);
                Modelo.Add("HORAATENCIONCLIENTE", solicitudLevantamiento.HoraAtencionCliente);

                Modelo.Add("PERMISOINGRESARSITIO", (Convert.ToInt32(solicitudLevantamiento.PermisoIngresarSitio) == 0) ? "NO" : "SI");
                Modelo.Add("EQUIPOSEGURIDADINGRESARSITIO", (Convert.ToInt32(solicitudLevantamiento.EquipoSeguridadIngresarSitio) == 0) ? "NO" : "SI");
                Modelo.Add("CLIENTECUENTAESTACIONAMIENTO", (Convert.ToInt32(solicitudLevantamiento.ClienteCuentaEstacionamiento) == 0) ? "NO" : "SI");
                Modelo.Add("CLIENTECUENTAPLANOSLEVANTAMIENTO", (Convert.ToInt32(solicitudLevantamiento.ClienteCuentaPlanoLevantamiento) == 0) ? "NO" : "SI");

                Modelo.Add("DOMICILIO", solicitudLevantamiento.Domicilio);
                Modelo.Add("DESCRIPCION", solicitudLevantamiento.Descripcion);
                Modelo.Add("NOTA", solicitudLevantamiento.Notas);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    public static void enviarCorreo(int IdSolLevantamiento, CConexion pConexion)
    {

        CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
        CUsuario creador = new CUsuario();
        CUsuario asignado = new CUsuario();
        solicitudLevantamiento.LlenaObjeto(IdSolLevantamiento, pConexion);
        creador.LlenaObjeto(solicitudLevantamiento.IdCreador, pConexion);
        asignado.LlenaObjeto(solicitudLevantamiento.IdUsuarioAsignado, pConexion);

        string msg = templateCorreoSolicitud(IdSolLevantamiento, pConexion);

        if (solicitudLevantamiento.IdUsuarioAsignado != 0)
            CUtilerias.EnviarCorreo(creador.Correo, asignado.Correo, "Asignación de Levantamiento - " + solicitudLevantamiento.IdSolicitudLevantamiento, msg);


    }

    public static string templateCorreoSolicitud(int IdSolLevantamiento, CConexion pConexion)
    {

        string msg = "";
        CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();

        solicitudLevantamiento.LlenaObjeto(IdSolLevantamiento, pConexion);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]), pConexion);

        CMunicipio MunicipioE = new CMunicipio();
        MunicipioE.LlenaObjeto(Empresa.IdMunicipio, pConexion);

        CEstado EstadoE = new CEstado();
        EstadoE.LlenaObjeto(MunicipioE.IdEstado, pConexion);

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(solicitudLevantamiento.IdCliente, pConexion);

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

        CUsuario Agente = new CUsuario();
        Agente.LlenaObjeto(solicitudLevantamiento.IdAgente, pConexion);

        CUsuario Asignado = new CUsuario();
        Asignado.LlenaObjeto(solicitudLevantamiento.IdUsuarioAsignado, pConexion);

        CPuestoContacto contactoDirectoPuesto = new CPuestoContacto();
        contactoDirectoPuesto.LlenaObjeto(solicitudLevantamiento.IdPuestoContactoDirecto, pConexion);

        CPuestoContacto contactoEnSitioPuesto = new CPuestoContacto();
        contactoEnSitioPuesto.LlenaObjeto(solicitudLevantamiento.IdPuestoContactoEnSitio, pConexion);

        CDivision division = new CDivision();
        division.LlenaObjeto(solicitudLevantamiento.IdDivision, pConexion);

        msg = CUtilerias.TextoArchivo(@"C:\inetpub\wwwroot\KeepInfoWeb\Templates\tmplImprimirSolLevantamiento.html");
        msg = msg.Replace("${FOLIO}", Convert.ToString(solicitudLevantamiento.IdSolicitudLevantamiento));
        msg = msg.Replace("${RAZONSOCIALEMISOR}", Empresa.RazonSocial);
        msg = msg.Replace("${RFCEMISOR}", Empresa.RFC);
        msg = msg.Replace("${IMAGEN_LOGO}", Empresa.Logo);
        msg = msg.Replace("${CALLEEMISOR}", Empresa.Calle);
        msg = msg.Replace("${NUMEROEXTERIOREMISOR}", Empresa.NumeroExterior);
        msg = msg.Replace("${COLONIAEMISOR}", Empresa.Colonia);
        msg = msg.Replace("${CODIGOPOSTALEMISOR}", Empresa.CodigoPostal);
        msg = msg.Replace("${MUNICIPIOEMISOR}", MunicipioE.Municipio);
        msg = msg.Replace("${ESTADOEMISOR}", EstadoE.Estado);

        msg = msg.Replace("${FECHAALTA}", solicitudLevantamiento.FechaAlta.ToShortDateString());
        msg = msg.Replace("${IDOPORTUNIDAD}", Convert.ToString(solicitudLevantamiento.IdOportunidad));
        msg = msg.Replace("${FECHACITA}", Convert.ToString(solicitudLevantamiento.CitaFechaHora));
        msg = msg.Replace("${ESPECIALDIAD}", division.Division);
        msg = msg.Replace("${RAZONSOCIALRECEPTOR}", Organizacion.RazonSocial);
        msg = msg.Replace("${AGENTE}", Agente.Nombre + " " + Agente.ApellidoPaterno + " " + Agente.ApellidoMaterno);
        msg = msg.Replace("${ASIGNADO}", Asignado.Nombre + " " + Asignado.ApellidoPaterno + " " + Asignado.ApellidoMaterno);
        msg = msg.Replace("${CONTACTODIRECTO}", solicitudLevantamiento.ContactoDirecto);
        msg = msg.Replace("${CONTACTODIRECTOPUESTO}", contactoDirectoPuesto.Descripcion);
        msg = msg.Replace("${ESASOCIADO}", (Convert.ToInt32(solicitudLevantamiento.Externo) == 0) ? "NO" : "SI");
        msg = msg.Replace("${CONTACTOENSITIO}", solicitudLevantamiento.ContactoEnSitio);
        msg = msg.Replace("${CONTACTOENSITIOPUESTO}", contactoEnSitioPuesto.Descripcion);
        msg = msg.Replace("${TELEFONOS}", solicitudLevantamiento.Telefonos);
        msg = msg.Replace("${HORAATENCIONCLIENTE}", solicitudLevantamiento.HoraAtencionCliente);

        msg = msg.Replace("${PERMISOINGRESARSITIO}", (Convert.ToInt32(solicitudLevantamiento.PermisoIngresarSitio) == 0) ? "NO" : "SI");
        msg = msg.Replace("${EQUIPOSEGURIDADINGRESARSITIO}", (Convert.ToInt32(solicitudLevantamiento.EquipoSeguridadIngresarSitio) == 0) ? "NO" : "SI");
        msg = msg.Replace("${CLIENTECUENTAESTACIONAMIENTO}", (Convert.ToInt32(solicitudLevantamiento.ClienteCuentaEstacionamiento) == 0) ? "NO" : "SI");
        msg = msg.Replace("${CLIENTECUENTAPLANOSLEVANTAMIENTO}", (Convert.ToInt32(solicitudLevantamiento.ClienteCuentaPlanoLevantamiento) == 0) ? "NO" : "SI");

        msg = msg.Replace("${DOMICILIO}", solicitudLevantamiento.Domicilio);
        msg = msg.Replace("${DESCRIPCION}", solicitudLevantamiento.Descripcion);
        msg = msg.Replace("${NOTA}", solicitudLevantamiento.Notas);

        return msg;
    }
    
    [WebMethod]
    public static string ObtenerDatos(int pIdSolLevantamiento){
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                solicitudLevantamiento.LlenaObjeto(pIdSolLevantamiento, pConexion);

                Modelo.Add("idSolLevantamiento", solicitudLevantamiento.IdSolicitudLevantamiento);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(solicitudLevantamiento.IdCliente, pConexion);

                Modelo.Add("IdCliente",cliente.IdCliente);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add("RazonSocial", organizacion.RazonSocial);

                //Combos
                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudLevantamiento.IdOportunidad, pConexion);

                Modelo.Add("IdOportunidad", oportunidad.IdOportunidad);

                Modelo.Add("Oportunidades", COportunidad.ObtenerOportunidadProyecto(cliente.IdCliente, UsuarioSesion.IdUsuario, oportunidad.IdOportunidad, pConexion));

                CDivision division = new CDivision();
                division.LlenaObjeto(solicitudLevantamiento.IdDivision,pConexion);

                Modelo.Add("IdDivision", division.IdDivision);

                Modelo.Add("Divisiones",CDivision.ObtenerJsonDivisionesActivas(-1, pConexion));



                Modelo.Add("FOLIO", solicitudLevantamiento.IdSolicitudLevantamiento);


                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarLevantamiento(int pIdSolicitudLevantamiento)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            JObject Modelo = new JObject();

            if (Error == 0)
            {
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Convert.ToInt32(UsuarioSesion.IdSucursalActual), pConexion);
                DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                Modelo.Add("FechaAlta", DateTime.Now.ToShortDateString());
                DateTime fechaValidoHasta = DateTime.Now.AddDays(3);
                Modelo.Add("ValidoHasta", fechaValidoHasta.ToShortDateString());
                Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuario(pConexion));
                Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(pConexion));
                Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(-1, pConexion));



                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                solicitudLevantamiento.LlenaObjeto(pIdSolicitudLevantamiento, pConexion);
                
                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(solicitudLevantamiento.IdCliente, pConexion);

                Modelo.Add("IdCliente", cliente.IdCliente);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add("RazonSocial", organizacion.RazonSocial);

                //Combos
                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudLevantamiento.IdOportunidad, pConexion);

                Modelo.Add("IdOportunidad", oportunidad.IdOportunidad);

                Modelo.Add("Oportunidades", COportunidad.ObtenerOportunidadProyecto(cliente.IdCliente, UsuarioSesion.IdUsuario, oportunidad.IdOportunidad, pConexion));

                CDivision division = new CDivision();
                division.LlenaObjeto(solicitudLevantamiento.IdDivision, pConexion);

                Modelo.Add("IdDivision", division.IdDivision);

                //Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(-1, pConexion));




                //Energia UPS
                Modelo.Add("EnergiaUPS", ObtenerJsonChecksActivas(1, pConexion));

                //Comunicaciones Video Proyeccion
                Modelo.Add("ComunicacionesVideoProyeccion", ObtenerJsonChecksActivas(2, pConexion));

                //Comunicaciones Audio
                Modelo.Add("ComunicacionesAudio", ObtenerJsonChecksActivas(3, pConexion));

                //Comunicaciones Conmutador
                Modelo.Add("ComunicacionesConmutador", ObtenerJsonChecksActivas(4, pConexion));

                //Comunicaciones Enlaces de Mircoonda
                Modelo.Add("ComunicacionesEnlacesMircoonda", ObtenerJsonChecksActivas(5, pConexion));

                //Infraestructura Cableado Voz y Datos
                Modelo.Add("InfraestructuraCableadoVozDatos", ObtenerJsonChecksActivas(6, pConexion));

                //Infraestructura Canalizaciones
                Modelo.Add("InfraestructuraCanalizaciones", ObtenerJsonChecksActivas(7, pConexion));

                //Infraesructura Proteccion
                Modelo.Add("InfraestructuraProteccion", ObtenerJsonChecksActivas(8, pConexion));

                //Checks General 
                Modelo.Add("ChecksGeneral", ObtenerJsonChecksActivas(9, pConexion));
            }

            Respuesta.Add("Modelo", Modelo);
        });


        return Respuesta.ToString();
    }

    public static JArray ObtenerJsonChecksActivas(int pIdCheckEncabezado, CConexion pConexion)
    {
        JArray JAChecks = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", false);
        Parametros.Add("IdLevantamientoChecklist", pIdCheckEncabezado);
        CLevantamientoChecklistOp checkOp = new CLevantamientoChecklistOp();
        foreach (CLevantamientoChecklistOp oCheckOp in checkOp.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCheckOp = new JObject();
            JCheckOp.Add("Descripcion", oCheckOp.Descripcion);
            JCheckOp.Add("IdCheck", oCheckOp.IdLevantamientoChecklistOp);
            JAChecks.Add(JCheckOp);
        }

        return JAChecks;
    }

}