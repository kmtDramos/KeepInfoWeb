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

public partial class GestionCobranza : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CrearGridGestionCobranza();
    }

    private void CrearGridGestionCobranza()
    {
        //GridGestionCobranza
        CJQGrid GridGestionCobranza = new CJQGrid();
        GridGestionCobranza.NombreTabla = "grdGestionCobranza";
        GridGestionCobranza.CampoIdentificador = "IdGestionCobranza";
        GridGestionCobranza.ColumnaOrdenacion = "IdGestionCobranza";
        GridGestionCobranza.TipoOrdenacion = "DESC";
        GridGestionCobranza.Metodo = "ObtenerGestionCobranza";
        GridGestionCobranza.TituloTabla = "Facturas pendientes";
        GridGestionCobranza.Altura = 290;
        GridGestionCobranza.GenerarFuncionFiltro = false;
        GridGestionCobranza.GenerarFuncionTerminado = true;
        GridGestionCobranza.NumeroRegistros = 25;
        GridGestionCobranza.RangoNumeroRegistros = "25,50,100";

        //IdGestionCobranza
        CJQColumn ColIdGestionCobranza = new CJQColumn();
        ColIdGestionCobranza.Nombre = "IdGestionCobranza";
        ColIdGestionCobranza.Oculto = "true";
        ColIdGestionCobranza.Encabezado = "IdGestionCobranza";
        ColIdGestionCobranza.Buscador = "false";
        GridGestionCobranza.Columnas.Add(ColIdGestionCobranza);

        //IdFactura
        CJQColumn ColIdFactura = new CJQColumn();
        ColIdFactura.Nombre = "IdFactura";
        ColIdFactura.Oculto = "true";
        ColIdFactura.Encabezado = "IdFactura";
        ColIdFactura.Buscador = "false";
        GridGestionCobranza.Columnas.Add(ColIdFactura);

        //Cliente
        CJQColumn ColCliente = new CJQColumn();
        ColCliente.Nombre = "RazonSocial";
        ColCliente.Encabezado = "Razón social";
        ColCliente.Buscador = "true";
        ColCliente.Alineacion = "left";
        ColCliente.Ancho = "240";
        GridGestionCobranza.Columnas.Add(ColCliente);

        //SerieFactura
        CJQColumn ColSerieFactura = new CJQColumn();
        ColSerieFactura.Nombre = "SerieFactura";
        ColSerieFactura.Encabezado = "Serie";
        ColSerieFactura.Buscador = "true";
        ColSerieFactura.Alineacion = "center";
        ColSerieFactura.Ancho = "100";
        GridGestionCobranza.Columnas.Add(ColSerieFactura);

        //NumeroFactura
        CJQColumn ColNumeroFactura = new CJQColumn();
        ColNumeroFactura.Nombre = "NumeroFactura";
        ColNumeroFactura.Encabezado = "Folio";
        ColNumeroFactura.Buscador = "true";
        ColNumeroFactura.Alineacion = "right";
        ColNumeroFactura.Ancho = "100";
        GridGestionCobranza.Columnas.Add(ColNumeroFactura);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "150";
        ColTotal.Formato = "FormatoMoneda";
        GridGestionCobranza.Columnas.Add(ColTotal);

        //Saldo
        CJQColumn ColSaldo = new CJQColumn();
        ColSaldo.Nombre = "SaldoFactura";
        ColSaldo.Encabezado = "Saldo";
        ColSaldo.Buscador = "false";
        ColSaldo.Alineacion = "right";
        ColSaldo.Ancho = "150";
        ColSaldo.Formato = "FormatoMoneda";
        GridGestionCobranza.Columnas.Add(ColSaldo);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "100";
        GridGestionCobranza.Columnas.Add(ColTipoMoneda);

        //FechaEmision
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Emisión";
        ColFechaEmision.Buscador = "false";
        ColFechaEmision.Ancho = "140";
        GridGestionCobranza.Columnas.Add(ColFechaEmision);

        //FechaVencimiento
        CJQColumn ColVencimiento = new CJQColumn();
        ColVencimiento.Nombre = "Vencimiento";
        ColVencimiento.Encabezado = "Vencimiento";
        ColVencimiento.Buscador = "false";
        ColVencimiento.Ancho = "140";
        GridGestionCobranza.Columnas.Add(ColVencimiento);

        //DiasVencidos
        CJQColumn ColDiasVencidos = new CJQColumn();
        ColDiasVencidos.Nombre = "DiasVencidos";
        ColDiasVencidos.Encabezado = "DV";
        ColDiasVencidos.Buscador = "false";
        ColDiasVencidos.Ancho = "65";
        GridGestionCobranza.Columnas.Add(ColDiasVencidos);

        //FechaProximaGestion
        CJQColumn ColFechaProximaGestion = new CJQColumn();
        ColFechaProximaGestion.Nombre = "ProximaGestion";
        ColFechaProximaGestion.Encabezado = "Próx. gestión";
        ColFechaProximaGestion.Etiquetado = "Calendario";
        ColFechaProximaGestion.Estilo = "divImagenConsultar imgFormaAgregarProximaGestion";
        ColFechaProximaGestion.Buscador = "false";
        ColFechaProximaGestion.Imagen = "calendario.png";
        ColFechaProximaGestion.Ancho = "160";
        GridGestionCobranza.Columnas.Add(ColFechaProximaGestion);

        //FechaPago
        CJQColumn ColFechaPago = new CJQColumn();
        ColFechaPago.Nombre = "FechaPago";
        ColFechaPago.Encabezado = "Fecha de pago";
        ColFechaPago.Etiquetado = "Calendario";
        ColFechaPago.Estilo = "divImagenConsultar imgFormaAgregarFechaPago";
        ColFechaPago.Buscador = "false";
        ColFechaPago.Imagen = "calendario.png";
        ColFechaPago.Ancho = "160";
        GridGestionCobranza.Columnas.Add(ColFechaPago);

        //Comentarios
        CJQColumn ColComentarios = new CJQColumn();
        ColComentarios.Nombre = "Comentario";
        ColComentarios.Encabezado = "C";
        ColComentarios.Buscador = "false";
        ColComentarios.Ordenable = "false";
        ColComentarios.Ancho = "50";
        GridGestionCobranza.Columnas.Add(ColComentarios);

        //SeleccionarVarios
        CJQColumn ColSeleccionarVarios = new CJQColumn();
        ColSeleccionarVarios.Nombre = "Sel";
        ColSeleccionarVarios.Encabezado = "Sel.";
        ColSeleccionarVarios.Buscador = "false";
        ColSeleccionarVarios.Ordenable = "false";
        ColSeleccionarVarios.Ancho = "50";
        ColSeleccionarVarios.Etiquetado = "CheckBox";
        ColSeleccionarVarios.Estilo = "checkAsignarVarios";
        GridGestionCobranza.Columnas.Add(ColSeleccionarVarios);

        ClientScript.RegisterStartupScript(this.GetType(), "grdGestionCobranza", GridGestionCobranza.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerGestionCobranza(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pSerieFactura, string pNumeroFactura, int pIdTipoGestion, int pFiltroFecha, string pFechaInicio, string pFechaCorte, int pIdFiltroClientesAsignados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdGestionCobranza", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pSerieFactura", SqlDbType.VarChar, 255).Value = pSerieFactura;
        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 255).Value = pNumeroFactura;
        Stored.Parameters.Add("pIdTipoGestion", SqlDbType.VarChar, 255).Value = pIdTipoGestion;
        Stored.Parameters.Add("pFiltroFecha", SqlDbType.VarChar, 255).Value = pFiltroFecha;
        Stored.Parameters.Add("pFechaInicio", SqlDbType.VarChar, 255).Value = pFechaInicio;
        Stored.Parameters.Add("pFechaCorte", SqlDbType.VarChar, 255).Value = pFechaCorte;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.VarChar, 255).Value = Usuario.IdSucursalActual;
        Stored.Parameters.Add("pIdFiltroClientesAsignados", SqlDbType.VarChar, 255).Value = pIdFiltroClientesAsignados;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarProximaGestion(int pIdGestionCobranza, int pIdFactura, string pFacturasSeleccionadas)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        Modelo.Add("Fecha", DateTime.Now.ToShortDateString());
        Modelo.Add("IdFactura", pIdFactura);
        Modelo.Add("IdGestionCobranza", pIdGestionCobranza);

        string[] facturasSeleccionadas = { };
        if (pFacturasSeleccionadas.Length > 0)
        {
            facturasSeleccionadas = pFacturasSeleccionadas.Split(',');
        }

        string foliosFacturas = "";
        foreach (string oIdFactura in facturasSeleccionadas)
        {
            int idFactura = Convert.ToInt32(oIdFactura);
            CFacturaEncabezado Factura = new CFacturaEncabezado();
            Factura.LlenaObjeto(idFactura, ConexionBaseDatos);

            CSerieFactura Serie = new CSerieFactura();
            Serie.LlenaObjeto(Factura.IdSerieFactura, ConexionBaseDatos);
            foliosFacturas = foliosFacturas + Serie.SerieFactura + "-" + Factura.NumeroFactura + ", ";
        }

        if (foliosFacturas.Length > 0)
        {
            foliosFacturas = foliosFacturas.Remove(foliosFacturas.Length - 2);
        }
        Modelo.Add("FoliosFacturas", foliosFacturas);
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarFechaPago(int pIdGestionCobranza, int pIdFactura, string pFacturasSeleccionadas)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        Modelo.Add("Fecha", DateTime.Now.ToShortDateString());
        Modelo.Add("IdFactura", pIdFactura);
        Modelo.Add("IdGestionCobranza", pIdGestionCobranza);

        string[] facturasSeleccionadas = { };
        if (pFacturasSeleccionadas.Length > 0)
        {
            facturasSeleccionadas = pFacturasSeleccionadas.Split(',');
        }

        string foliosFacturas = "";
        foreach (string oIdFactura in facturasSeleccionadas)
        {
            int idFactura = Convert.ToInt32(oIdFactura);
            CFacturaEncabezado Factura = new CFacturaEncabezado();
            Factura.LlenaObjeto(idFactura, ConexionBaseDatos);

            CSerieFactura Serie = new CSerieFactura();
            Serie.LlenaObjeto(Factura.IdSerieFactura, ConexionBaseDatos);
            foliosFacturas = foliosFacturas + Serie.SerieFactura + "-" + Factura.NumeroFactura + ", ";
        }

        if (foliosFacturas.Length > 0)
        {
            foliosFacturas = foliosFacturas.Remove(foliosFacturas.Length - 2);
        }
        Modelo.Add("FoliosFacturas", foliosFacturas);
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaFiltroGestionCobranza()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;

        CTipoGestion TipoGestion = new CTipoGestion();
        JArray JTiposGestion = new JArray();
        foreach (CTipoGestion oTipoGestion in TipoGestion.LlenaObjetos(ConexionBaseDatos))
        {
            JObject JTipoGestion = new JObject();
            JTipoGestion.Add(new JProperty("IdTipoGestion", oTipoGestion.IdTipoGestion));
            JTipoGestion.Add(new JProperty("TipoGestion", oTipoGestion.TipoGestion));
            JTiposGestion.Add(JTipoGestion);
        }
        Modelo.Add("TiposGestion", JTiposGestion);
        Modelo.Add("Fecha", Convert.ToString(Fecha.ToShortDateString()));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaGestionCobranzaSeguimientos(int pIdFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        Dictionary<string, object> ParametrosGestionCobranza = new Dictionary<string, object>();
        ParametrosGestionCobranza.Add("IdFactura", pIdFactura);

        CGestionCobranza GestionCobranza = new CGestionCobranza();
        GestionCobranza.LlenaObjetoFiltros(ParametrosGestionCobranza, ConexionBaseDatos);
        Modelo.Add("IdGestionCobranza", GestionCobranza.IdGestionCobranza);

        Dictionary<string, object> ParametrosGestionCobranzaDetalle = new Dictionary<string, object>();
        ParametrosGestionCobranzaDetalle.Add("IdGestionCobranza", GestionCobranza.IdGestionCobranza);

        CGestionCobranzaDetalle GestionCobranzaDetalle = new CGestionCobranzaDetalle();
        JArray JSeguimientos = new JArray();
        foreach (CGestionCobranzaDetalle oGestionCobranzaDetalle in GestionCobranzaDetalle.LlenaObjetosFiltrosOrdenarIdDesc(ParametrosGestionCobranzaDetalle, ConexionBaseDatos))
        {
            JObject JSeguimiento = new JObject();
            JSeguimiento.Add(new JProperty("FechaProgramada", oGestionCobranzaDetalle.FechaProgramada.ToShortDateString()));
            JSeguimiento.Add(new JProperty("FechaAlta", oGestionCobranzaDetalle.FechaAlta.ToShortDateString() + ' ' + oGestionCobranzaDetalle.FechaAlta.ToShortTimeString()));
            JSeguimiento.Add(new JProperty("Comentario", oGestionCobranzaDetalle.Comentario));

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(oGestionCobranzaDetalle.IdUsuarioAlta, ConexionBaseDatos);
            JSeguimiento.Add(new JProperty("Nombre", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno));

            CTipoGestion TipoGestion = new CTipoGestion();
            TipoGestion.LlenaObjeto(oGestionCobranzaDetalle.IdTipoGestion, ConexionBaseDatos);
            JSeguimiento.Add(new JProperty("TipoGestion", TipoGestion.TipoGestion));
            JSeguimientos.Add(JSeguimiento);
        }
        Modelo.Add("Seguimientos", JSeguimientos);
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string AgregarProximaGestion(int pIdGestionCobranza, int pIdFactura, string pFechaProximaGestion, string pComentarios, string pFacturasSeleccionadas)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            if (pFacturasSeleccionadas.Length > 0)
            {
                string[] facturasSeleccionadas = { };
                if (pFacturasSeleccionadas.Length > 0)
                {
                    facturasSeleccionadas = pFacturasSeleccionadas.Split(',');
                }

                foreach (string oIdFactura in facturasSeleccionadas)
                {
                    int idFactura = Convert.ToInt32(oIdFactura);
                    CFacturaEncabezado Factura = new CFacturaEncabezado();
                    Factura.LlenaObjeto(idFactura, ConexionBaseDatos);

                    Dictionary<string, object> Parametros = new Dictionary<string, object>();
                    Parametros.Add("IdFactura", Factura.IdFacturaEncabezado);
                    Parametros.Add("Baja", false);

                    CGestionCobranza GestionCobranzaValidar = new CGestionCobranza();
                    GestionCobranzaValidar.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

                    CGestionCobranza GestionCobranza = new CGestionCobranza();
                    GestionCobranza.IdFactura = Factura.IdFacturaEncabezado;
                    GestionCobranza.IdCliente = Factura.IdCliente;
                    GestionCobranza.FechaAlta = DateTime.Now;
                    GestionCobranza.FechaProgramada = Convert.ToDateTime(pFechaProximaGestion);
                    GestionCobranza.IdTipoGestion = 1;
                    GestionCobranza.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    GestionCobranza.Baja = false;

                    CGestionCobranzaDetalle GestionCobranzaDetalle = new CGestionCobranzaDetalle();
                    GestionCobranzaDetalle.Comentario = pComentarios;
                    GestionCobranzaDetalle.FechaProgramada = Convert.ToDateTime(pFechaProximaGestion);
                    GestionCobranzaDetalle.IdTipoGestion = 1;
                    GestionCobranzaDetalle.FechaAlta = DateTime.Now;
                    GestionCobranzaDetalle.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    GestionCobranzaDetalle.Gestionado = false;
                    GestionCobranzaDetalle.Baja = false;

                    if (GestionCobranzaValidar.IdGestionCobranza == 0)
                    {
                        GestionCobranza.Agregar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = GestionCobranza.IdGestionCobranza;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }
                    else
                    {
                        GestionCobranza.IdGestionCobranza = GestionCobranzaValidar.IdGestionCobranza;
                        GestionCobranza.Editar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = GestionCobranzaValidar.IdGestionCobranza; ;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }
                }
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                oRespuesta.Add(new JProperty("Error", 0));
                return oRespuesta.ToString();
            }
            else
            {
                CFacturaEncabezado Factura = new CFacturaEncabezado();
                Factura.LlenaObjeto(pIdFactura, ConexionBaseDatos);

                CGestionCobranza GestionCobranza = new CGestionCobranza();
                GestionCobranza.IdFactura = Factura.IdFacturaEncabezado;
                GestionCobranza.IdCliente = Factura.IdCliente;
                GestionCobranza.FechaAlta = DateTime.Now;
                GestionCobranza.FechaProgramada = Convert.ToDateTime(pFechaProximaGestion);
                GestionCobranza.IdTipoGestion = 1;
                GestionCobranza.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                GestionCobranza.Baja = false;

                CGestionCobranzaDetalle GestionCobranzaDetalle = new CGestionCobranzaDetalle();
                GestionCobranzaDetalle.Comentario = pComentarios;
                GestionCobranzaDetalle.FechaProgramada = Convert.ToDateTime(pFechaProximaGestion);
                GestionCobranzaDetalle.IdTipoGestion = 1;
                GestionCobranzaDetalle.FechaAlta = DateTime.Now;
                GestionCobranzaDetalle.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                GestionCobranzaDetalle.Gestionado = false;
                GestionCobranzaDetalle.Baja = false;

                string validacion = "";
                //string validacion = ValidarMarca(Marca, ConexionBaseDatos);
                if (validacion == "")
                {
                    if (pIdGestionCobranza == 0)
                    {
                        GestionCobranza.Agregar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = GestionCobranza.IdGestionCobranza;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }
                    else
                    {
                        GestionCobranza.IdGestionCobranza = pIdGestionCobranza;
                        GestionCobranza.Editar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = pIdGestionCobranza;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }

                    oRespuesta.Add(new JProperty("Error", 0));
                    ConexionBaseDatos.CerrarBaseDatosSqlServer();
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();
            }
        }
        else
        {

            return "1|" + respuesta;
        }
    }

    [WebMethod]
    public static string AgregarFechaPago(int pIdGestionCobranza, int pIdFactura, string pFechaPago, string pComentarios, string pFacturasSeleccionadas)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            if (pFacturasSeleccionadas.Length > 0)
            {
                string[] facturasSeleccionadas = { };
                if (pFacturasSeleccionadas.Length > 0)
                {
                    facturasSeleccionadas = pFacturasSeleccionadas.Split(',');
                }

                foreach (string oIdFactura in facturasSeleccionadas)
                {
                    int idFactura = Convert.ToInt32(oIdFactura);
                    CFacturaEncabezado Factura = new CFacturaEncabezado();
                    Factura.LlenaObjeto(idFactura, ConexionBaseDatos);

                    Dictionary<string, object> Parametros = new Dictionary<string, object>();
                    Parametros.Add("IdFactura", Factura.IdFacturaEncabezado);
                    Parametros.Add("Baja", false);

                    CGestionCobranza GestionCobranzaValidar = new CGestionCobranza();
                    GestionCobranzaValidar.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

                    CGestionCobranza GestionCobranza = new CGestionCobranza();
                    GestionCobranza.IdFactura = Factura.IdFacturaEncabezado;
                    GestionCobranza.IdCliente = Factura.IdCliente;
                    GestionCobranza.FechaAlta = DateTime.Now;
                    GestionCobranza.FechaProgramada = Convert.ToDateTime(pFechaPago);
                    GestionCobranza.IdTipoGestion = 2;
                    GestionCobranza.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    GestionCobranza.Baja = false;

                    CGestionCobranzaDetalle GestionCobranzaDetalle = new CGestionCobranzaDetalle();
                    GestionCobranzaDetalle.Comentario = pComentarios;
                    GestionCobranzaDetalle.FechaProgramada = Convert.ToDateTime(pFechaPago);
                    GestionCobranzaDetalle.IdTipoGestion = 2;
                    GestionCobranzaDetalle.FechaAlta = DateTime.Now;
                    GestionCobranzaDetalle.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    GestionCobranzaDetalle.Gestionado = false;
                    GestionCobranzaDetalle.Baja = false;

                    if (GestionCobranzaValidar.IdGestionCobranza == 0)
                    {
                        GestionCobranza.Agregar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = GestionCobranza.IdGestionCobranza;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }
                    else
                    {
                        GestionCobranza.IdGestionCobranza = GestionCobranzaValidar.IdGestionCobranza; ;
                        GestionCobranza.Editar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = GestionCobranzaValidar.IdGestionCobranza; ;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }
                }
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                oRespuesta.Add(new JProperty("Error", 0));
                return oRespuesta.ToString();
            }
            else
            {
                CFacturaEncabezado Factura = new CFacturaEncabezado();
                Factura.LlenaObjeto(pIdFactura, ConexionBaseDatos);

                CGestionCobranza GestionCobranza = new CGestionCobranza();
                GestionCobranza.IdFactura = Factura.IdFacturaEncabezado;
                GestionCobranza.IdCliente = Factura.IdCliente;
                GestionCobranza.FechaAlta = DateTime.Now;
                GestionCobranza.FechaProgramada = Convert.ToDateTime(pFechaPago);
                GestionCobranza.IdTipoGestion = 2;
                GestionCobranza.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                GestionCobranza.Baja = false;

                CGestionCobranzaDetalle GestionCobranzaDetalle = new CGestionCobranzaDetalle();
                GestionCobranzaDetalle.Comentario = pComentarios;
                GestionCobranzaDetalle.FechaProgramada = Convert.ToDateTime(pFechaPago);
                GestionCobranzaDetalle.IdTipoGestion = 2;
                GestionCobranzaDetalle.FechaAlta = DateTime.Now;
                GestionCobranzaDetalle.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                GestionCobranzaDetalle.Gestionado = false;
                GestionCobranzaDetalle.Baja = false;

                string validacion = "";
                //string validacion = ValidarMarca(Marca, ConexionBaseDatos);
                if (validacion == "")
                {
                    if (pIdGestionCobranza == 0)
                    {
                        GestionCobranza.Agregar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = GestionCobranza.IdGestionCobranza;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }
                    else
                    {
                        GestionCobranza.IdGestionCobranza = pIdGestionCobranza;
                        GestionCobranza.Editar(ConexionBaseDatos);
                        GestionCobranzaDetalle.IdGestionCobranza = pIdGestionCobranza;
                        GestionCobranzaDetalle.Agregar(ConexionBaseDatos);
                    }

                    oRespuesta.Add(new JProperty("Error", 0));
                    ConexionBaseDatos.CerrarBaseDatosSqlServer();
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();
            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string BuscarRazonSocialCliente(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarRazonSocial(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarSerieFactura(string pSerieFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_GestionCobranza_ConsultarSerieFactura";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pSerie", pSerieFactura);
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarNumeroFactura(string pNumeroFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_GestionCobranza_ConsultarNumeroFactura";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", pNumeroFactura);
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }
}