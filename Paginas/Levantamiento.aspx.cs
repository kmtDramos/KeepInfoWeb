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
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        JObject Modelo = new JObject();

        oPermisos.Add("puedeAgregarCotizacion", puedeAgregarCotizacion);

        if (respuesta == "Conexion Establecida")
        {
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
            CTipoCambio TipoCambio = new CTipoCambio();
            DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            string validacion = "";//ValidarExisteTipoCambio(TipoCambio, Sucursal, Fecha, ConexionBaseDatos);
            CNivelInteresCotizacion NivelInteresCotizacion = new CNivelInteresCotizacion();
            if (validacion == "")
            {
                Modelo.Add("FechaAlta", DateTime.Now.ToShortDateString());
                DateTime fechaValidoHasta = DateTime.Now.AddDays(3);
                Modelo.Add("ValidoHasta", fechaValidoHasta.ToShortDateString());
                Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuario(ConexionBaseDatos));
                Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(ConexionBaseDatos));
                Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(-1, ConexionBaseDatos));

                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No hay tipo de cambio del dia"));
            }
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
    public static string AgregarLevantamiento(int IdLevantamiento, int IdCliente, string Nota, string ValidoHasta, int IdDivision, int IdOportunidad, int IdEstatusLevantamiento) {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                

                CLevantamiento levantamiento = new CLevantamiento();
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

                levantamiento.Agregar(pConexion);

                Error = 0;
                DescripcionError = "Se ha guardado con éxito.";

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
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


                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
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


                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarLevantamiento(int IdLevantamiento, int IdCliente, string Nota, string ValidoHasta, int IdDivision, int IdOportunidad)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CLevantamiento Levantamiento = new CLevantamiento();
                Levantamiento.LlenaObjeto(IdLevantamiento, pConexion);

                Levantamiento.IdCliente = IdCliente;
                Levantamiento.Descripcion = Nota;
                Levantamiento.FechaEstimada = Convert.ToDateTime(ValidoHasta);
                Levantamiento.IdDivision = IdDivision;
                Levantamiento.IdOportunidad = IdOportunidad;
                Levantamiento.Editar(pConexion);
                
                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarSolicitudLevantamiento(string FechaAlta, string FechaCita, int IdOportunidad, int IdCliente, int IdAgente, string ContactoDirecto, int ContactoDirectoPuesto, int EsAsociado, string ContactoEnSitio, int ContactoEnSitioPuesto, string Telefonos, string HoraCliente,int PermisoIngresarSitio, int EquipoSeguridadIngresarSitio, int ClienteCuentaEstacionamiento, int ClienteCuentaPlanoLevantamiento, string Domicilio, int Division, string Descripcion, string Notas)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();

                solicitudLevantamiento.FechaAlta = Convert.ToDateTime(FechaAlta);
                solicitudLevantamiento.FechaCita = Convert.ToDateTime(FechaCita);
                solicitudLevantamiento.IdOportunidad = IdOportunidad;
                solicitudLevantamiento.IdCliente = IdCliente;
                solicitudLevantamiento.IdAgente = IdAgente;
                solicitudLevantamiento.ContactoDirecto = ContactoDirecto;
                solicitudLevantamiento.IdPuestoContactoDirecto = ContactoDirectoPuesto;
                solicitudLevantamiento.EsAsociado = Convert.ToBoolean(EsAsociado);
                solicitudLevantamiento.ContactoEnSitio = ContactoEnSitio;
                solicitudLevantamiento.IdPuestoContactoEnSitio = ContactoEnSitioPuesto;
                solicitudLevantamiento.Telefonos = Telefonos;
                solicitudLevantamiento.HoraAtencionCliente = HoraCliente;
                solicitudLevantamiento.PermisoIngresarSitio = Convert.ToBoolean(PermisoIngresarSitio);
                solicitudLevantamiento.EquipoSeguridadIngresarSitio = Convert.ToBoolean(EquipoSeguridadIngresarSitio);
                solicitudLevantamiento.ClienteCuentaEstacionamiento = Convert.ToBoolean(ClienteCuentaEstacionamiento);
                solicitudLevantamiento.ClienteCuentaPlanoLevantamiento = Convert.ToBoolean(ClienteCuentaPlanoLevantamiento);
                solicitudLevantamiento.Domicilio = Domicilio;
                solicitudLevantamiento.IdDivision = Division;
                solicitudLevantamiento.Descripcion = Descripcion;
                solicitudLevantamiento.Notas = Notas;
                solicitudLevantamiento.Agregar(pConexion);

                Respuesta.Add("IdSolLevantamiento", solicitudLevantamiento.IdSolicitudLevantamiento);

                Error = 0;
                DescripcionError = "Se ha guardado con éxito.";

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarSolicitudLevantamiento(int IdSolLevantamiento, string FechaCita, int IdOportunidad, int IdCliente, int IdAgente, string ContactoDirecto, int ContactoDirectoPuesto, int EsAsociado, string ContactoEnSitio, int ContactoEnSitioPuesto, string Telefonos, string HoraCliente, int PermisoIngresarSitio, int EquipoSeguridadIngresarSitio, int ClienteCuentaEstacionamiento, int ClienteCuentaPlanoLevantamiento, string Domicilio, int Division, string Descripcion, string Notas)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {

                CSolicitudLevantamiento solicitudLevantamiento = new CSolicitudLevantamiento();
                solicitudLevantamiento.LlenaObjeto(IdSolLevantamiento, pConexion);

                //solicitudLevantamiento.FechaAlta = Convert.ToDateTime(FechaAlta);
                solicitudLevantamiento.FechaCita = Convert.ToDateTime(FechaCita);
                solicitudLevantamiento.IdOportunidad = IdOportunidad;
                solicitudLevantamiento.IdCliente = IdCliente;
                solicitudLevantamiento.IdAgente = IdAgente;
                solicitudLevantamiento.ContactoDirecto = ContactoDirecto;
                solicitudLevantamiento.IdPuestoContactoDirecto = ContactoDirectoPuesto;
                solicitudLevantamiento.EsAsociado = Convert.ToBoolean(EsAsociado);
                solicitudLevantamiento.ContactoEnSitio = ContactoEnSitio;
                solicitudLevantamiento.IdPuestoContactoEnSitio = ContactoEnSitioPuesto;
                solicitudLevantamiento.Telefonos = Telefonos;
                solicitudLevantamiento.HoraAtencionCliente = HoraCliente;
                solicitudLevantamiento.PermisoIngresarSitio = Convert.ToBoolean(PermisoIngresarSitio);
                solicitudLevantamiento.EquipoSeguridadIngresarSitio = Convert.ToBoolean(EquipoSeguridadIngresarSitio);
                solicitudLevantamiento.ClienteCuentaEstacionamiento = Convert.ToBoolean(ClienteCuentaEstacionamiento);
                solicitudLevantamiento.ClienteCuentaPlanoLevantamiento = Convert.ToBoolean(ClienteCuentaPlanoLevantamiento);
                solicitudLevantamiento.Domicilio = Domicilio;
                solicitudLevantamiento.IdDivision = Division;
                solicitudLevantamiento.Descripcion = Descripcion;
                solicitudLevantamiento.Notas = Notas;
                solicitudLevantamiento.Editar(pConexion);

                Error = 0;
                DescripcionError = "Se ha guardado con éxito.";

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ImpirmirSolLevantamiento(int IdCotizacion)
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

                CCotizacion Cotizacion = new CCotizacion();
                Cotizacion.LlenaObjeto(IdCotizacion, pConexion);

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(Cotizacion.IdCliente, pConexion);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdOrganizacion", Organizacion.IdOrganizacion);
                pParametros.Add("IdTipoDireccion", 1);

                CDireccionOrganizacion ValidarDireccion = new CDireccionOrganizacion();

                if (ValidarDireccion.LlenaObjetosFiltros(pParametros, pConexion).Count == 1)
                {

                    CDireccionOrganizacion Direccion = new CDireccionOrganizacion();
                    Direccion.LlenaObjetoFiltros(pParametros, pConexion);

                    CMunicipio MunicipioR = new CMunicipio();
                    MunicipioR.LlenaObjeto(Direccion.IdMunicipio, pConexion);

                    CEstado EstadoR = new CEstado();
                    EstadoR.LlenaObjeto(MunicipioR.IdEstado, pConexion);

                    CPais Pais = new CPais();
                    Pais.LlenaObjeto(EstadoR.IdPais, pConexion);

                    CCondicionPago CondicionPago = new CCondicionPago();
                    CondicionPago.LlenaObjeto(Cliente.IdCondicionPago, pConexion);

                    CUsuario Agente = new CUsuario();
                    Agente.LlenaObjeto(Cotizacion.IdUsuarioAgente, pConexion);

                    CTipoMoneda Moneda = new CTipoMoneda();
                    Moneda.LlenaObjeto(Cotizacion.IdTipoMoneda, pConexion);

                    CTipoCambioCotizacion TipoCambio = new CTipoCambioCotizacion();
                    pParametros.Clear();
                    pParametros.Add("IdCotizacion", Cotizacion.IdCotizacion);
                    pParametros.Add("IdTipoMonedaOrigen", Cotizacion.IdTipoMoneda);
                    pParametros.Add("IdTipoMonedaDestino", 1);
                    TipoCambio.LlenaObjetoFiltros(pParametros, pConexion);

                    CCotizacionDetalle Detalle = new CCotizacionDetalle();
                    pParametros.Clear();
                    pParametros.Add("IdCotizacion", Cotizacion.IdCotizacion);
                    pParametros.Add("Baja", 0);
                    JArray Conceptos = new JArray();

                    Cotizacion.SubTotal = 0;
                    foreach (CCotizacionDetalle Partida in Detalle.LlenaObjetosFiltros(pParametros, pConexion))
                    {
                        JObject Concepto = new JObject();
                        Concepto.Add("CANTIDADDETALLE", Partida.Cantidad);
                        Concepto.Add("DESCRIPCIONDETALLE", Partida.Descripcion);
                        Concepto.Add("PRECIOUNITARIODETALLE", Partida.PrecioUnitario.ToString("C"));
                        Cotizacion.SubTotal += Partida.Total;
                        Concepto.Add("TOTALDETALLE", Partida.Total.ToString("C"));
                        //Conceptos.Add(Concepto);
                    }

                    CSelectEspecifico Consulta = new CSelectEspecifico();
                    Consulta.StoredProcedure.CommandText = "sp_CotizacionDetalle_Imprimir";
                    Consulta.StoredProcedure.Parameters.Add("IdCotizacion", SqlDbType.Int).Value = IdCotizacion;

                    Consulta.Llena(pConexion);

                    while (Consulta.Registros.Read())
                    {
                        JObject Concepto = new JObject();
                        Concepto.Add("CANTIDADDETALLE", Convert.ToInt32(Consulta.Registros["Cantidad"]));
                        Concepto.Add("DESCRIPCIONDETALLE", Convert.ToString(Consulta.Registros["Descripcion"]));
                        Concepto.Add("PRECIOUNITARIODETALLE", Convert.ToDecimal(Consulta.Registros["PrecioUnitario"]).ToString("C"));
                        //Cotizacion.SubTotal += Convert.ToDecimal(Consulta.Registros["Total"]);
                        Concepto.Add("TOTALDETALLE", Convert.ToDecimal(Consulta.Registros["Total"]).ToString("C"));
                        Conceptos.Add(Concepto);
                    }

                    Consulta.CerrarConsulta();


                    Modelo.Add("Conceptos", Conceptos);

                    Modelo.Add("FOLIO", Cotizacion.Folio);
                    Modelo.Add("RAZONSOCIALEMISOR", Empresa.RazonSocial);
                    Modelo.Add("RFCEMISOR", Empresa.RFC);
                    Modelo.Add("IMAGEN_LOGO", Empresa.Logo);
                    Modelo.Add("CALLEEMISOR", Empresa.Calle);
                    Modelo.Add("NUMEROEXTERIOREMISOR", Empresa.NumeroExterior);
                    Modelo.Add("COLONIAEMISOR", Empresa.Colonia);
                    Modelo.Add("CODIGOPOSTALEMISOR", Empresa.CodigoPostal);
                    Modelo.Add("MUNICIPIOEMISOR", MunicipioE.Municipio);
                    Modelo.Add("ESTADOEMISOR", EstadoE.Estado);
                    Modelo.Add("FECHAALTA", Cotizacion.FechaAlta.ToShortDateString());
                    Modelo.Add("PROYECTO", Cotizacion.Proyecto);
                    Modelo.Add("RFCRECEPTOR", Organizacion.RFC);
                    Modelo.Add("RAZONSOCIALRECEPTOR", Organizacion.RazonSocial);
                    Modelo.Add("CALLERECEPTOR", Direccion.Calle);
                    Modelo.Add("NUMEROEXTERIORRECEPTOR", Direccion.NumeroExterior);
                    Modelo.Add("REFERENCIARECEPTOR", Direccion.Referencia);
                    Modelo.Add("COLONIARECEPTOR", Direccion.Colonia);
                    Modelo.Add("CODIGOPOSTALRECEPTOR", Direccion.CodigoPostal);
                    Modelo.Add("MUNICIPIORECEPTOR", MunicipioR.Municipio);
                    Modelo.Add("ESTADORECEPTOR", EstadoR.Estado);
                    Modelo.Add("PAISRECEPTOR", Pais.Pais);
                    Modelo.Add("TELEFONORECEPTOR", Direccion.ConmutadorTelefono);
                    Modelo.Add("CONDICIONPAGO", CondicionPago.CondicionPago);
                    Modelo.Add("USUARIOSOLICITO", Agente.Nombre + " " + Agente.ApellidoPaterno + " " + Agente.ApellidoMaterno);
                    Modelo.Add("TIPOMONEDA", Moneda.TipoMoneda);
                    Modelo.Add("TIPOCAMBIO", TipoCambio.TipoCambio);
                    Modelo.Add("SUBTOTALCOTIZACION", Cotizacion.SubTotal.ToString("C"));
                    Modelo.Add("PorcentajeIVACotizacion", (Cotizacion.IVA == 0) ? 0 : Math.Round(Cotizacion.IVA / Cotizacion.SubTotal * 100));
                    Modelo.Add("IVACOTIZACION", Cotizacion.IVA.ToString("C"));
                    Modelo.Add("TOTALCOTIZACION", Cotizacion.Total.ToString("C"));
                    Modelo.Add("CANTIDADTOTALLETRA", Cotizacion.CantidadTotalLetra);
                    Modelo.Add("NOTA", Cotizacion.Nota);

                }
                else
                {
                    Error = 1;
                    DescripcionError = "Favor de verificar la dirección fiscal del cliente";
                }

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }
}