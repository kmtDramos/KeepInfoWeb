using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
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

public partial class Paginas_SolicitudesProyecto : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        CJQGrid GridSolicitudMaterial = new CJQGrid();
        GridSolicitudMaterial.NombreTabla = "grdEntregaMaterial";
        GridSolicitudMaterial.CampoIdentificador = "IdSolicitudMaterial";
        GridSolicitudMaterial.ColumnaOrdenacion = "IdSolicitudMaterial";
        GridSolicitudMaterial.TipoOrdenacion = "ASC";
        GridSolicitudMaterial.Metodo = "ObtenerSolicitudEntregaMaterial";
        GridSolicitudMaterial.TituloTabla = "Salida Material";
        GridSolicitudMaterial.GenerarFuncionFiltro = false;
        GridSolicitudMaterial.GenerarFuncionTerminado = false;
        GridSolicitudMaterial.Altura = 231;
        GridSolicitudMaterial.Ancho = 940;
        GridSolicitudMaterial.NumeroRegistros = 25;
        GridSolicitudMaterial.RangoNumeroRegistros = "25,50,100";

        #region Columnas

        //IdSolicitudMaterial
        CJQColumn ColIdSolicitudMaterial = new CJQColumn();
        ColIdSolicitudMaterial.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMaterial.Encabezado = "Solicitud Material";
        ColIdSolicitudMaterial.Oculto = "false";
        ColIdSolicitudMaterial.Buscador = "true";
        ColIdSolicitudMaterial.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColIdSolicitudMaterial);

        //Solicitante
        CJQColumn ColSolicitante = new CJQColumn();
        ColSolicitante.Nombre = "Solicitante";
        ColSolicitante.Encabezado = "Solicitante";
        ColSolicitante.Alineacion = "left";
        ColSolicitante.Buscador = "false";
        ColSolicitante.Ancho = "100";
        GridSolicitudMaterial.Columnas.Add(ColSolicitante);

        //Fecha Alta
        CJQColumn ColFechaAlta = new CJQColumn();
        ColFechaAlta.Nombre = "FechaAlta";
        ColFechaAlta.Encabezado = "Fecha Alta";
        ColFechaAlta.Alineacion = "left";
        ColFechaAlta.Buscador = "false";
        ColFechaAlta.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColFechaAlta);

        //Estatus
        CJQColumn ColEstatus = new CJQColumn();
        ColEstatus.Nombre = "Estatus";
        ColEstatus.Encabezado = "Estatus";
        ColEstatus.Alineacion = "left";
        ColEstatus.Buscador = "false";
        ColEstatus.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColEstatus);

        //Fecha Entrega
        CJQColumn ColFechaEntrega = new CJQColumn();
        ColFechaEntrega.Nombre = "FechaEntrega";
        ColFechaEntrega.Encabezado = "Fecha Entrega";
        ColFechaEntrega.Alineacion = "left";
        ColFechaEntrega.Buscador = "false";
        ColFechaEntrega.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColFechaEntrega);

        //Comentarios
        CJQColumn ColComentarios = new CJQColumn();
        ColComentarios.Nombre = "Comentarios";
        ColComentarios.Encabezado = "Comentarios";
        ColComentarios.Alineacion = "left";
        ColComentarios.Buscador = "false";
        ColComentarios.Ancho = "150";
        GridSolicitudMaterial.Columnas.Add(ColComentarios);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarSolicitudMaterial";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridSolicitudMaterial.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(Page.GetType(), "grdEntregaMaterial", GridSolicitudMaterial.GeneraGrid(), true);

        //Grids
        PintaGridConceptosConsultar();
        PintaGridConceptosEditar();

    }

    public void PintaGridConceptosConsultar()
    {
        //GridPartidasSolicitudMaterialConsultar
        CJQGrid grdPartidasSolicitudMaterialConsultar = new CJQGrid();
        grdPartidasSolicitudMaterialConsultar.NombreTabla = "grdPartidasSolicitudMaterialConsultar";
        grdPartidasSolicitudMaterialConsultar.CampoIdentificador = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialConsultar.ColumnaOrdenacion = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialConsultar.TipoOrdenacion = "DESC";
        grdPartidasSolicitudMaterialConsultar.Metodo = "ObtenerSolicitudEntregaMaterialConceptosConsultar";
        grdPartidasSolicitudMaterialConsultar.TituloTabla = "Conceptos";
        grdPartidasSolicitudMaterialConsultar.GenerarGridCargaInicial = false;
        grdPartidasSolicitudMaterialConsultar.GenerarFuncionFiltro = false;
        grdPartidasSolicitudMaterialConsultar.GenerarFuncionTerminado = false;
        grdPartidasSolicitudMaterialConsultar.Altura = 120;
        grdPartidasSolicitudMaterialConsultar.Ancho = 670;
        grdPartidasSolicitudMaterialConsultar.NumeroRegistros = 15;
        grdPartidasSolicitudMaterialConsultar.RangoNumeroRegistros = "15,30,60";

        //IdSolcitudMaterial
        CJQColumn ColIdSolicitudMateriall = new CJQColumn();
        ColIdSolicitudMateriall.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Oculto = "true";
        ColIdSolicitudMateriall.Encabezado = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Buscador = "false";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColIdSolicitudMateriall);

        //IdSolcitudMaterialConcepto
        CJQColumn ColIdSolicitudMaterialConcepto = new CJQColumn();
        ColIdSolicitudMaterialConcepto.Nombre = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Oculto = "true";
        ColIdSolicitudMaterialConcepto.Encabezado = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Buscador = "false";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColIdSolicitudMaterialConcepto);

        //Clave Interna
        CJQColumn ColClaveInterna = new CJQColumn();
        ColClaveInterna.Nombre = "ClaveInterna";
        ColClaveInterna.Encabezado = "Clave Interna";
        ColClaveInterna.Buscador = "false";
        ColClaveInterna.Alineacion = "left";
        ColClaveInterna.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColClaveInterna);

        //Numero Parte
        CJQColumn ColNumeroParte = new CJQColumn();
        ColNumeroParte.Nombre = "NumeroParte";
        ColNumeroParte.Encabezado = "Numero Parte";
        ColNumeroParte.Buscador = "false";
        ColNumeroParte.Alineacion = "left";
        ColNumeroParte.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColNumeroParte);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        ColCantidad.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColCantidad);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Division";
        ColDivision.Buscador = "false";
        ColDivision.Alineacion = "left";
        ColDivision.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDivision);

        //Clave SAT
        CJQColumn ColClaveProdServ = new CJQColumn();
        ColClaveProdServ.Nombre = "ClaveProdServ";
        ColClaveProdServ.Encabezado = "Clave [SAT]";
        ColClaveProdServ.Buscador = "false";
        ColClaveProdServ.Alineacion = "left";
        ColClaveProdServ.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColClaveProdServ);

        //Disponible Inventario
        CJQColumn ColDisponibleInventario = new CJQColumn();
        ColDisponibleInventario.Nombre = "DisponibleInventario";
        ColDisponibleInventario.Encabezado = "Inventario";
        ColDisponibleInventario.Buscador = "false";
        ColDisponibleInventario.Alineacion = "left";
        ColDisponibleInventario.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDisponibleInventario);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPartidasSolicitudMaterialConsultar", grdPartidasSolicitudMaterialConsultar.GeneraGrid(), true);
    }

    public void PintaGridConceptosEditar()
    {
        //GridPartidasSolicitudMaterialConsultar
        CJQGrid grdPartidasSolicitudMaterialEditar = new CJQGrid();
        grdPartidasSolicitudMaterialEditar.NombreTabla = "grdPartidasSolicitudMaterialEditar";
        grdPartidasSolicitudMaterialEditar.CampoIdentificador = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialEditar.ColumnaOrdenacion = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialEditar.TipoOrdenacion = "DESC";
        grdPartidasSolicitudMaterialEditar.Metodo = "ObtenerSolicitudEntregaMaterialConceptosEditar";
        grdPartidasSolicitudMaterialEditar.TituloTabla = "Conceptos";
        grdPartidasSolicitudMaterialEditar.GenerarGridCargaInicial = false;
        grdPartidasSolicitudMaterialEditar.GenerarFuncionFiltro = false;
        grdPartidasSolicitudMaterialEditar.GenerarFuncionTerminado = false;
        grdPartidasSolicitudMaterialEditar.Altura = 120;
        grdPartidasSolicitudMaterialEditar.Ancho = 670;
        grdPartidasSolicitudMaterialEditar.NumeroRegistros = 15;
        grdPartidasSolicitudMaterialEditar.RangoNumeroRegistros = "15,30,60";

        //IdSolcitudMaterial
        CJQColumn ColIdSolicitudMateriall = new CJQColumn();
        ColIdSolicitudMateriall.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Oculto = "true";
        ColIdSolicitudMateriall.Encabezado = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Buscador = "false";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColIdSolicitudMateriall);

        //IdSolcitudMaterialConcepto
        CJQColumn ColIdSolicitudMaterialConcepto = new CJQColumn();
        ColIdSolicitudMaterialConcepto.Nombre = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Oculto = "true";
        ColIdSolicitudMaterialConcepto.Encabezado = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Buscador = "false";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColIdSolicitudMaterialConcepto);

        //Clave Interna
        CJQColumn ColClaveInterna = new CJQColumn();
        ColClaveInterna.Nombre = "ClaveInterna";
        ColClaveInterna.Encabezado = "Clave Interna";
        ColClaveInterna.Buscador = "false";
        ColClaveInterna.Alineacion = "left";
        ColClaveInterna.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColClaveInterna);

        //Numero Parte
        CJQColumn ColNumeroParte = new CJQColumn();
        ColNumeroParte.Nombre = "NumeroParte";
        ColNumeroParte.Encabezado = "Numero Parte";
        ColNumeroParte.Buscador = "false";
        ColNumeroParte.Alineacion = "left";
        ColNumeroParte.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColNumeroParte);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        ColCantidad.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColCantidad);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Division";
        ColDivision.Buscador = "false";
        ColDivision.Alineacion = "left";
        ColDivision.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColDivision);

        //Clave SAT
        CJQColumn ColClaveProdServ = new CJQColumn();
        ColClaveProdServ.Nombre = "ClaveProdServ";
        ColClaveProdServ.Encabezado = "Clave [SAT]";
        ColClaveProdServ.Buscador = "false";
        ColClaveProdServ.Alineacion = "left";
        ColClaveProdServ.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColClaveProdServ);

        //Disponible Inventario
        CJQColumn ColDisponibleInventario = new CJQColumn();
        ColDisponibleInventario.Nombre = "DisponibleInventario";
        ColDisponibleInventario.Encabezado = "Inventario";
        ColDisponibleInventario.Buscador = "false";
        ColDisponibleInventario.Alineacion = "left";
        ColDisponibleInventario.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColDisponibleInventario);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPartidasSolicitudMaterialEditar", grdPartidasSolicitudMaterialEditar.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterial(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdSolicitudMaterial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial", sqlCon);

        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterialConceptosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdSolicitudMaterial)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial_Conceptos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterialConceptosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdSolicitudMaterial)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial_Conceptos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string ObtenerFormaSolicitudEntregaMaterial(int pIdSolicitudMaterial)
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarSalidaEntregaMaterial = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                //Permisos
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarSalidaEntregaMaterial" }, pConexion) == "")
                {
                    puedeEditarSalidaEntregaMaterial = 1;
                }
                oPermisos.Add("puedeEditarSalidaEntregaMaterial", puedeEditarSalidaEntregaMaterial);

                Modelo.Add("Permisos", oPermisos);


                CSolicitudMaterial solicitudMaterial = new CSolicitudMaterial();
                solicitudMaterial.LlenaObjeto(pIdSolicitudMaterial, pConexion);

                Modelo.Add("IdSolicitudMaterial", solicitudMaterial.IdSolicitudMaterial);
                Modelo.Add("FechaAlta", Convert.ToString(solicitudMaterial.FechaAlta.ToShortDateString()));


                Modelo.Add("Confirmado", solicitudMaterial.Aprobar);
                Modelo.Add("Comentarios", solicitudMaterial.Comentarios);

                CUsuario solicitante = new CUsuario();
                solicitante.LlenaObjeto(solicitudMaterial.IdUsuarioCreador, pConexion);
                Modelo.Add("Solicitante", solicitante.Nombre + " " + solicitante.ApellidoPaterno + " " + solicitante.ApellidoMaterno);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudMaterial.IdOportunidad, pConexion);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(oportunidad.IdCliente, pConexion);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add("RazonSocial", organizacion.RazonSocial);
                Modelo.Add("RFC", organizacion.RFC);

                Modelo.Add("Oportunidad", oportunidad.Oportunidad);

                CUsuario agente = new CUsuario();
                agente.LlenaObjeto(oportunidad.IdUsuarioCreacion, pConexion);

                Modelo.Add("Agente", agente.Nombre + " " + agente.ApellidoPaterno + " " + agente.ApellidoMaterno);

                CDivision division = new CDivision();
                division.LlenaObjeto(oportunidad.IdDivision, pConexion);

                Modelo.Add("Division", division.Division);


                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarSolicitudEntregaMaterial(int IdSolicitudMaterial)
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarSalidaEntregaMaterial = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                //Permisos
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarSalidaEntregaMaterial" }, pConexion) == "")
                {
                    puedeEditarSalidaEntregaMaterial = 1;
                }
                oPermisos.Add("puedeEditarSalidaEntregaMaterial", puedeEditarSalidaEntregaMaterial);

                Modelo.Add("Permisos", oPermisos);


                CSolicitudMaterial solicitudMaterial = new CSolicitudMaterial();
                solicitudMaterial.LlenaObjeto(IdSolicitudMaterial, pConexion);

                Modelo.Add("IdSolicitudMaterial", solicitudMaterial.IdSolicitudMaterial);
                Modelo.Add("FechaAlta", Convert.ToString(solicitudMaterial.FechaAlta.ToShortDateString()));
                Modelo.Add("Confirmado", solicitudMaterial.Aprobar);
                Modelo.Add("Comentarios", solicitudMaterial.Comentarios);
                CUsuario solicitante = new CUsuario();
                solicitante.LlenaObjeto(solicitudMaterial.IdUsuarioCreador, pConexion);
                Modelo.Add("Solicitante", solicitante.Nombre + " " + solicitante.ApellidoPaterno + " " + solicitante.ApellidoMaterno);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudMaterial.IdOportunidad, pConexion);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(oportunidad.IdCliente, pConexion);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add("RazonSocial", organizacion.RazonSocial);
                Modelo.Add("RFC", organizacion.RFC);

                Modelo.Add("Oportunidad", oportunidad.Oportunidad);

                CUsuario agente = new CUsuario();
                agente.LlenaObjeto(oportunidad.IdUsuarioCreacion, pConexion);

                Modelo.Add("Agente", agente.Nombre + " " + agente.ApellidoPaterno + " " + agente.ApellidoMaterno);

                CDivision division = new CDivision();
                division.LlenaObjeto(oportunidad.IdDivision, pConexion);

                Modelo.Add("Division", division.Division);


                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string AgregarSolicitudProyecto(int IdOportunidad, int IdCliente, string NombreProyecto, int CotExcel, int CotFirmada, int OrdenCompra, string NumeroOC, int Contrato, string NumeroContrato, int AutorizadoCorreo, int PagoAnticipo, int RequiereFactura, string Porcentaje, string QuienAutoriza, string ContactoSolicitudProyecto, string QuienRealizaCotizacion, int Avanzar, int Compra, string Comentarios)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSolicitudProyecto solicitudProyecto = new CSolicitudProyecto();
                
                solicitudProyecto.FechaAlta = DateTime.Now;
                solicitudProyecto.IdUsuario = UsuarioSesion.IdUsuario;
                solicitudProyecto.IdOportunidad = IdOportunidad;
                solicitudProyecto.Proyecto = NombreProyecto;
                solicitudProyecto.CotizacionExcel = Convert.ToBoolean(CotExcel);
                solicitudProyecto.CotizacionFirmada = Convert.ToBoolean(CotFirmada);
                solicitudProyecto.OrdenCompra = Convert.ToBoolean(OrdenCompra);
                solicitudProyecto.NumOrdenCompra = Convert.ToString(NumeroOC);
                solicitudProyecto.Contrato = Convert.ToBoolean(Contrato);
                solicitudProyecto.NumContrato = Convert.ToString(NumeroContrato);
                solicitudProyecto.AutorizacionCorreo = Convert.ToBoolean(AutorizadoCorreo);
                solicitudProyecto.PagoDeAnticipo = Convert.ToBoolean(PagoAnticipo);
                solicitudProyecto.RequiereFactura = Convert.ToBoolean(RequiereFactura);
                solicitudProyecto.Procentaje = Convert.ToDecimal(Porcentaje);
                solicitudProyecto.QuienAutoriza = Convert.ToString(QuienAutoriza);
                solicitudProyecto.Contacto = Convert.ToString(ContactoSolicitudProyecto);
                solicitudProyecto.QuienCotizo = Convert.ToString(QuienRealizaCotizacion);
                solicitudProyecto.SolicitudCompra = Convert.ToInt32(Compra);
                solicitudProyecto.AvanzarCompras = Convert.ToInt32(Avanzar);
                solicitudProyecto.Comentarios = Convert.ToString(Comentarios);

                solicitudProyecto.Agregar(pConexion);

                Respuesta.Add("IdSolLevantamiento", solicitudProyecto.IdSolicitudProyecto);

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarSolicitudProyecto(int IdSolLevantamiento, string CitaFechaHora, int IdOportunidad, int IdCliente, int IdAgente, int IdAsignado, string ContactoDirecto, int ContactoDirectoPuesto, int Externo, string ContactoEnSitio, int ContactoEnSitioPuesto, string Telefonos, int PermisoIngresarSitio, int EquipoSeguridadIngresarSitio, int ClienteCuentaEstacionamiento, int ClienteCuentaPlanoLevantamiento, string Domicilio, string Descripcion, string Notas, int Confirmacion)
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

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }
    #endregion
}