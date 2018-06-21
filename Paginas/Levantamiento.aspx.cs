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

public partial class Levantamiento : System.Web.UI.Page
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
        CJQGrid GridLevantamiento = new CJQGrid();
        GridLevantamiento.NombreTabla = "grdLevantamiento";
        GridLevantamiento.CampoIdentificador = "IdLevantamiento";
        GridLevantamiento.TipoOrdenacion = "DESC";
        GridLevantamiento.ColumnaOrdenacion = "IdLevantamiento";
        GridLevantamiento.Metodo = "ObtenerLevantamiento";
        GridLevantamiento.TituloTabla = "Catálogo de Levantamientos";
        GridLevantamiento.GenerarFuncionFiltro = false;

        //IdLevantamiento
        CJQColumn ColIdLevantamiento = new CJQColumn();
        ColIdLevantamiento.Nombre = "IdLevantamiento";
        ColIdLevantamiento.Oculto = "false";
        ColIdLevantamiento.Encabezado = "Folio";
        ColIdLevantamiento.Buscador = "false";
        ColIdLevantamiento.Ancho = "50";
        GridLevantamiento.Columnas.Add(ColIdLevantamiento);
        /*
        //NoFolio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        ColFolio.Ancho = "50";
        GridLevantamiento.Columnas.Add(ColFolio);
        */
        //Razon Social
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "200";
        GridLevantamiento.Columnas.Add(ColRazonSocial);

        //Oportunidad
        CJQColumn ColOportunidad = new CJQColumn();
        ColOportunidad.Nombre = "IdOportunidad";
        ColOportunidad.Encabezado = "Oportunidad";
        ColOportunidad.Buscador = "true";
        ColOportunidad.Alineacion = "left";
        ColOportunidad.Ancho = "50";
        GridLevantamiento.Columnas.Add(ColOportunidad);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "FechaInicio";
        ColFecha.Encabezado = "Alta";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridLevantamiento.Columnas.Add(ColFecha);

        //Valido Hasta
        CJQColumn ColValidoHasta = new CJQColumn();
        ColValidoHasta.Nombre = "FechaEstimada";
        ColValidoHasta.Encabezado = "Vencimiento";
        ColValidoHasta.Buscador = "false";
        ColValidoHasta.Alineacion = "left";
        ColValidoHasta.Ancho = "80";
        GridLevantamiento.Columnas.Add(ColValidoHasta);

        //Descripcion
        CJQColumn ColDesripcion = new CJQColumn();
        ColDesripcion.Nombre = "Descripcion";
        ColDesripcion.Encabezado = "Descripcion";
        ColDesripcion.Buscador = "false";
        ColDesripcion.Alineacion = "left";
        ColDesripcion.Ancho = "170";
        GridLevantamiento.Columnas.Add(ColDesripcion);

        //Etapa
        CJQColumn ColEtapa = new CJQColumn();
        ColEtapa.Nombre = "Descripcion";
        ColEtapa.Encabezado = "Estatus";
        ColEtapa.Buscador = "false";
        ColEtapa.Alineacion = "left";
        ColEtapa.Ancho = "170";
        GridLevantamiento.Columnas.Add(ColEtapa);

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
        ColBaja.Oculto = "false";
        GridLevantamiento.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarLevantamiento";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridLevantamiento.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdLevantamiento", GridLevantamiento.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerLevantamiento(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, string pIdOportunidad, int pIdEstatusLevantamiento, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdLevantamiento", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdOportunidad);
        Stored.Parameters.Add("pIdEstatusLevantamiento", SqlDbType.Int).Value = pIdEstatusLevantamiento;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerTotalesEstatusLevantamiento(string pFechaInicial, string pFechaFinal, int pPorFecha, int pFolio, string pRazonSocial, int pAI, int pIdEstatusLevantamiento)
    {
        JObject Resultado = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_EstatusLevantamiento_Consultar_ObtenerTotalesSinFiltro";
                Consulta.StoredProcedure.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
                Consulta.StoredProcedure.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
                Consulta.StoredProcedure.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
                Consulta.StoredProcedure.Parameters.Add("pFolio", SqlDbType.Int).Value = pFolio;
                Consulta.StoredProcedure.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
                Consulta.StoredProcedure.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
                Consulta.StoredProcedure.Parameters.Add("pIdEstatusLevantamiento", SqlDbType.Int).Value = pIdEstatusLevantamiento;

                Modelo.Add("TotalesEstatusLevantamiento", CUtilerias.ObtenerConsulta(Consulta, pConexion));

                Resultado.Add("Modelo", Modelo);
            }
            Resultado.Add("Error", Error);
            Resultado.Add("Descripcion", DescripcionError);
        });

        return Resultado.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaFiltroLevantamiento()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;

        Modelo.Add("FechaInicial", Convert.ToString(Fecha.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(Fecha.ToShortDateString()));

        JArray Estatus = new JArray();
        CSelectEspecifico Consulta = new CSelectEspecifico();
        Consulta.StoredProcedure.CommandText = "sp_Levantamiento_Filtro_grdLevantamiento_Estatus";
        Consulta.Llena(ConexionBaseDatos);

        while (Consulta.Registros.Read())
        {
            JObject oEstatus = new JObject();
            oEstatus.Add("Valor", Convert.ToInt32(Consulta.Registros["IdEstatusLevantamiento"]));
            oEstatus.Add("Descripcion", Convert.ToString(Consulta.Registros["EstatusLevantamiento"]));
            Estatus.Add(oEstatus);
        }

        Consulta.CerrarConsulta();

        Modelo.Add("Estatus", Estatus);

        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarLevantamiento()
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

    [WebMethod]
    public static string ObtenerDivisionOportunidad(int IdOportunidad) {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
       
        JObject Modelo = new JObject();
        
        if (respuesta == "Conexion Establecida")
        {
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            
            COportunidad oportunidad = new COportunidad();
            Parametros.Add("IdOportunidad", IdOportunidad);
            oportunidad.LlenaObjetoFiltros(Parametros,ConexionBaseDatos);

            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CDivision.ObtenerJsonDivisionesActivas(oportunidad.IdDivision, ConexionBaseDatos));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string BuscarRazonSocial(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        COrganizacion jsonRazonSocial = new COrganizacion();
        jsonRazonSocial.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosGrid";
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Usuario.IdSucursalActual);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        return jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string BuscarSolLevantamiento(string pIdSolicitud)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        COrganizacion jsonRazonSocial = new COrganizacion();
        jsonRazonSocial.StoredProcedure.CommandText = "sp_Solicitud_Levantamiento_Consultar";
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSolicitud", pIdSolicitud);
        respuesta = jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerListaOportunidad(int pIdCliente, int pIdOportunidad)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", COportunidad.ObtenerOportunidadProyecto(pIdCliente, Usuario.IdUsuario, pIdOportunidad, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string AgregarLevantamiento(Dictionary<string,object> Checks, int IdLevantamiento, int IdCliente, string Nota, string ValidoHasta, int IdDivision, int IdOportunidad, int IdEstatusLevantamiento, int IdSolLevantamiento) {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                

                CLevantamiento levantamiento = new CLevantamiento();
                levantamiento.IdSolicitudLevantamiento = IdSolLevantamiento;
                levantamiento.IdCliente = IdCliente;
                levantamiento.IdOportunidad = IdOportunidad;
                levantamiento.IdDivision = IdDivision;
                levantamiento.IdProyecto = 0;
                levantamiento.IdCotizacion = 0;
                levantamiento.IdEstatusLevantamiento = IdEstatusLevantamiento;
                levantamiento.IdUsuario = UsuarioSesion.IdUsuario;
                levantamiento.FechaInicio = DateTime.Now;
                levantamiento.FechaFin = DateTime.Now;
                levantamiento.FechaEstimada = Convert.ToDateTime(ValidoHasta);
                levantamiento.Descripcion = Nota;
                levantamiento.IdSucursal = UsuarioSesion.IdSucursalActual;

                levantamiento.Agregar(pConexion);

                if ((UsuarioSesion.IdUsuario == 95 || UsuarioSesion.IdUsuario == 215 || UsuarioSesion.IdUsuario == 26 || UsuarioSesion.IdUsuario == 93 || UsuarioSesion.IdUsuario == 202))
                {
                    COportunidad oportunidad = new COportunidad();
                    oportunidad.LlenaObjeto(IdOportunidad, pConexion);
                    oportunidad.CompromisoPreventa = Convert.ToDateTime(ValidoHasta);
                    oportunidad.Editar(pConexion);
                }

                agregarChecks(Checks, pConexion, levantamiento.IdLevantamiento);

                CSolicitudLevantamiento solLevantamiento = new CSolicitudLevantamiento();
                solLevantamiento.LlenaObjeto(IdSolLevantamiento, pConexion);
                solLevantamiento.LevantamientoCreado = Convert.ToBoolean(1);
                solLevantamiento.Editar(pConexion);

                Error = 0;
                DescripcionError = "Se ha guardado con éxito.";

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    private static void agregarChecks(Dictionary<string, object> Checks, CConexion pConexion, int IdLevantamiento) {

        CLevantamientoCheck check = new CLevantamientoCheck();
        CLevantamientoChecklist checklist = new CLevantamientoChecklist();
        CLevantamientoChecklistOp checklistop = new CLevantamientoChecklistOp();
  
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        
        for (int index = 1; index <= 160; index++)
        {
            //Parametros.Clear();
            //Parametros.Add("IdLevantamientoChecklistOp",index);
            checklistop.LlenaObjeto(index, pConexion);

            check.IdLevantamiento = IdLevantamiento;
            check.IdLevantamientoChecklist = checklistop.IdLevantamientoChecklist;
            check.IdLevantamientoChecklistOp = index;
            check.Cantidad = Convert.ToInt32(Checks["cantidad" + index]);
            check.Observaciones = Convert.ToString(Checks["Observacion" + index]);
            check.SINO = Convert.ToBoolean(Checks["sino" + index]);

            check.Agregar(pConexion);
            checklistop = new CLevantamientoChecklistOp();
        }


    }

    [WebMethod]
    public static string CambiarEstatus(int pIdLevantamiento, bool pBaja, int pIdEstatusLevantamiento)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion){
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CLevantamiento Levantamiento = new CLevantamiento();
                Levantamiento.LlenaObjeto(pIdLevantamiento, pConexion);

                bool validacion = ValidarBaja(Levantamiento, pConexion);

                if (validacion)
                {
                    Levantamiento.Baja = !Levantamiento.Baja;
                    Levantamiento.IdEstatusLevantamiento = (Convert.ToInt32(Levantamiento.Baja) == 0)?1:3;
                    Levantamiento.Editar(pConexion);
                }
                else {
                    Error = 1;
                    DescripcionError = "<span>*</span> El documento ya está ligado a una Cotización, no se puede dar de baja <br />";
                }

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    private static bool ValidarBaja(CLevantamiento Levantamiento, CConexion pConexion)
    {
        bool DocumentoLigado = false;
        bool flag = true;

        DocumentoLigado = (Levantamiento.IdCotizacion == 0)? false: true;
        if (DocumentoLigado == true)
        {
           flag = false;
        }

        return flag;
    }

    [WebMethod]
    public static string ObtenerFormaLevantamiento(int pIdLevantamiento)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();
                Dictionary<string, object> Parametros = new Dictionary<string, object>();

                CLevantamiento Levantamiento = new CLevantamiento();
                Levantamiento.LlenaObjeto(pIdLevantamiento, pConexion);
                
                Modelo.Add("Folio", Levantamiento.IdLevantamiento);
                Modelo.Add("IdLevantamiento", Levantamiento.IdLevantamiento);

                Modelo.Add("idSolLevantamiento", Levantamiento.IdSolicitudLevantamiento);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(Levantamiento.IdCliente, pConexion);
                Modelo.Add("IdCliente", Levantamiento.IdCliente);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);
                Modelo.Add("RazonSocial", organizacion.RazonSocial);

                COportunidad oportunidad = new COportunidad();
                Modelo.Add("IdOportunidad", Levantamiento.IdLevantamiento);
                oportunidad.LlenaObjeto(Levantamiento.IdOportunidad, pConexion);
                Modelo.Add("Oportunidad", oportunidad.Oportunidad);

                CDivision division = new CDivision();
                division.LlenaObjeto(oportunidad.IdDivision, pConexion);
                Modelo.Add("Division",division.Division);

                Modelo.Add("IdEstatusLevantamiento", Levantamiento.IdEstatusLevantamiento);

                Modelo.Add("FechaInicio", Levantamiento.FechaInicio.ToShortDateString());
                Modelo.Add("FechaEstimada", Levantamiento.FechaEstimada.ToShortDateString());
                Modelo.Add("Descripcion", Levantamiento.Descripcion);

                //Energia UPS
                Modelo.Add("EnergiaUPS", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 1, pConexion));

                //Comunicaciones Video Proyeccion
                Modelo.Add("ComunicacionesVideoProyeccion", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 2, pConexion));

                //Comunicaciones Audio
                Modelo.Add("ComunicacionesAudio", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 3, pConexion));

                //Comunicaciones Conmutador
                Modelo.Add("ComunicacionesConmutador", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 4, pConexion));

                //Comunicaciones Enlaces de Mircoonda
                Modelo.Add("ComunicacionesEnlacesMircoonda", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 5, pConexion));

                //Infraestructura Cableado Voz y Datos
                Modelo.Add("InfraestructuraCableadoVozDatos", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 6, pConexion));

                //Infraestructura Canalizaciones
                Modelo.Add("InfraestructuraCanalizaciones", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 7, pConexion));

                //Infraesructura Proteccion
                Modelo.Add("InfraestructuraProteccion", ObtenerJsonChecksLevantamiento(pIdLevantamiento, 8, pConexion));

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    public static JArray ObtenerJsonChecksLevantamiento(int pIdLevantamiento, int pIdCheckEncabezado, CConexion pConexion)
    {
        JArray JAChecks = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();

        Parametros.Add("IdLevantamiento", pIdLevantamiento);
        Parametros.Add("IdLevantamientoChecklist", pIdCheckEncabezado);

        CLevantamientoCheck checkOp = new CLevantamientoCheck();
        CLevantamientoChecklistOp checklistOp = new CLevantamientoChecklistOp();
        foreach (CLevantamientoCheck oCheckOp in checkOp.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCheckOp = new JObject();

            Parametros.Clear();
            checklistOp.LlenaObjeto(oCheckOp.IdLevantamientoChecklistOp, pConexion);

            JCheckOp.Add("IdCheck", checklistOp.IdLevantamientoChecklistOp);
            JCheckOp.Add("Check", oCheckOp.SINO);
            JCheckOp.Add("Descripcion",checklistOp.Descripcion);
            JCheckOp.Add("Cantidad", oCheckOp.Cantidad);
            JCheckOp.Add("Observacion", oCheckOp.Observaciones);
            JAChecks.Add(JCheckOp);
        }

        return JAChecks;
    }

    [WebMethod]
    public static string ObtenerFormaEditarLevantamiento(int IdLevantamiento)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();
                Dictionary<string, object> Parametros = new Dictionary<string, object>();

                CLevantamiento Levantamiento = new CLevantamiento();
                Levantamiento.LlenaObjeto(IdLevantamiento, pConexion);

                Modelo.Add("Folio", Levantamiento.IdLevantamiento);
                Modelo.Add("idLevantamiento", Levantamiento.IdLevantamiento);

                Modelo.Add("idSolLevantamiento", Levantamiento.IdSolicitudLevantamiento);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(Levantamiento.IdCliente, pConexion);
                Modelo.Add("IdCliente", cliente.IdCliente);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);
                Modelo.Add("RazonSocial", organizacion.RazonSocial);

                COportunidad oportunidad = new COportunidad();
                Modelo.Add("IdOportunidad", Levantamiento.IdOportunidad);
                oportunidad.LlenaObjeto(Levantamiento.IdOportunidad, pConexion);
                Modelo.Add("Oportunidad", oportunidad.Oportunidad);
                Modelo.Add("Oportunidades", COportunidad.ObtenerOportunidadProyecto(Levantamiento.IdCliente, UsuarioSesion.IdUsuario, Levantamiento.IdOportunidad, pConexion));

                Modelo.Add("IdEstatusLevantamiento", Levantamiento.IdEstatusLevantamiento);

                Modelo.Add("FechaInicio", Levantamiento.FechaInicio.ToShortDateString());
                Modelo.Add("FechaEstimada", Levantamiento.FechaEstimada.ToShortDateString());
                Modelo.Add("Descripcion", Levantamiento.Descripcion);

                //Energia UPS
                Modelo.Add("EnergiaUPS", ObtenerJsonChecksLevantamiento(IdLevantamiento, 1, pConexion));

                //Comunicaciones Video Proyeccion
                Modelo.Add("ComunicacionesVideoProyeccion", ObtenerJsonChecksLevantamiento(IdLevantamiento, 2, pConexion));

                //Comunicaciones Audio
                Modelo.Add("ComunicacionesAudio", ObtenerJsonChecksLevantamiento(IdLevantamiento, 3, pConexion));

                //Comunicaciones Conmutador
                Modelo.Add("ComunicacionesConmutador", ObtenerJsonChecksLevantamiento(IdLevantamiento, 4, pConexion));

                //Comunicaciones Enlaces de Mircoonda
                Modelo.Add("ComunicacionesEnlacesMircoonda", ObtenerJsonChecksLevantamiento(IdLevantamiento, 5, pConexion));

                //Infraestructura Cableado Voz y Datos
                Modelo.Add("InfraestructuraCableadoVozDatos", ObtenerJsonChecksLevantamiento(IdLevantamiento, 6, pConexion));

                //Infraestructura Canalizaciones
                Modelo.Add("InfraestructuraCanalizaciones", ObtenerJsonChecksLevantamiento(IdLevantamiento, 7, pConexion));

                //Infraesructura Proteccion
                Modelo.Add("InfraestructuraProteccion", ObtenerJsonChecksLevantamiento(IdLevantamiento, 8, pConexion));

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarLevantamiento(Dictionary<string,object> Checks, int IdSolLevantamiento, int IdLevantamiento, int IdCliente, string Nota, string ValidoHasta, int IdDivision, int IdOportunidad)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CLevantamiento Levantamiento = new CLevantamiento();
                Levantamiento.LlenaObjeto(IdLevantamiento, pConexion);

                Levantamiento.IdSolicitudLevantamiento = IdSolLevantamiento;
                Levantamiento.IdCliente = IdCliente;
                Levantamiento.Descripcion = Nota;
                Levantamiento.FechaEstimada = Convert.ToDateTime(ValidoHasta);
                Levantamiento.IdDivision = IdDivision;
                Levantamiento.IdOportunidad = IdOportunidad;

                Levantamiento.Editar(pConexion);

                editarChecks(Checks, pConexion, Levantamiento.IdLevantamiento);

                CSolicitudLevantamiento solLevantamiento = new CSolicitudLevantamiento();
                solLevantamiento.LlenaObjeto(IdSolLevantamiento, pConexion);
                solLevantamiento.LevantamientoCreado = Convert.ToBoolean(1);
                solLevantamiento.Editar(pConexion);

                if ((UsuarioSesion.IdUsuario == 95 || UsuarioSesion.IdUsuario == 215 || UsuarioSesion.IdUsuario == 26 || UsuarioSesion.IdUsuario == 93 || UsuarioSesion.IdUsuario == 202))
                {
                    COportunidad oportunidad = new COportunidad();
                    oportunidad.LlenaObjeto(IdOportunidad, pConexion);
                    oportunidad.CompromisoPreventa = Convert.ToDateTime(ValidoHasta);
                    oportunidad.Editar(pConexion);
                }

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    private static void editarChecks(Dictionary<string, object> Checks, CConexion pConexion, int IdLevantamiento)
    {

        CLevantamientoCheck check = new CLevantamientoCheck();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();

        for (int index = 1; index <= 160; index++)
        {
            Parametros.Clear();
            Parametros.Add("IdLevantamiento", IdLevantamiento);
            Parametros.Add("IdLevantamientoChecklistOp", index);
            check.LlenaObjetoFiltros(Parametros, pConexion);

            check.Cantidad = Convert.ToInt32(Checks["cantidad" + index]);
            check.Observaciones = Convert.ToString(Checks["Observacion" + index]);
            check.SINO = Convert.ToBoolean(Checks["sino" + index]);

            check.Editar(pConexion);
        }
        check = new CLevantamientoCheck();

    }

}