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
using System.IO;
using System.Xml;
using System.Globalization;
using System.Net;

public partial class FacturaCliente : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    private static int idEmpresa;
    private static string logoEmpresa;
    public static int puedeAgregarFacturaEncabezado = 0;
    public static int puedeEditarFacturaEncabezado = 0;
    public static int puedeEliminarFacturaEncabezado = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;
        idSucursal = Sucursal.IdSucursal;
        idEmpresa = Empresa.IdEmpresa;
        logoEmpresa = Empresa.Logo;

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        puedeAgregarFacturaEncabezado = Usuario.TienePermisos(new string[] { "puedeAgregarFacturaEncabezado" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarFacturaEncabezado = Usuario.TienePermisos(new string[] { "puedeEditarFacturaEncabezado" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarFacturaEncabezado = Usuario.TienePermisos(new string[] { "puedeEliminarFacturaEncabezado" }, ConexionBaseDatos) == "" ? 1 : 0;


        //grdFacturas
        CJQGrid GridFactura = new CJQGrid();
        GridFactura.NombreTabla = "grdFacturas";
        GridFactura.CampoIdentificador = "IdFacturaEncabezado";
        GridFactura.ColumnaOrdenacion = "NumeroFactura";
        GridFactura.Metodo = "ObtenerFacturas";
        GridFactura.GenerarFuncionFiltro = false;
        GridFactura.TituloTabla = "Facturas";
		GridFactura.ColumnaOrdenacion = "IdFacturaEncabezado";
		GridFactura.TipoOrdenacion = "DESC";

        //IdFactura
        CJQColumn ColIdFacturaEncabezado = new CJQColumn();
        ColIdFacturaEncabezado.Nombre = "IdFacturaEncabezado";
        ColIdFacturaEncabezado.Oculto = "true";
        ColIdFacturaEncabezado.Encabezado = "IdFacturaEncabezado";
        ColIdFacturaEncabezado.Buscador = "false";
        GridFactura.Columnas.Add(ColIdFacturaEncabezado);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "true";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridFactura.Columnas.Add(ColSucursal);

        //SerieFacturaEncabezado
        CJQColumn ColSerieFacturaEncabezado = new CJQColumn();
        ColSerieFacturaEncabezado.Nombre = "SerieFactura";
        ColSerieFacturaEncabezado.Encabezado = "Serie";
        ColSerieFacturaEncabezado.Ancho = "50";
        ColSerieFacturaEncabezado.Buscador = "true";
        ColSerieFacturaEncabezado.Alineacion = "left";
        GridFactura.Columnas.Add(ColSerieFacturaEncabezado);

        //FolioFacturaEncabezado
        CJQColumn ColFolioFacturaEncabezado = new CJQColumn();
        ColFolioFacturaEncabezado.Nombre = "NumeroFactura";
        ColFolioFacturaEncabezado.Encabezado = "Folio";
        ColFolioFacturaEncabezado.Ancho = "50";
        ColFolioFacturaEncabezado.Buscador = "true";
        ColFolioFacturaEncabezado.Alineacion = "left";
        GridFactura.Columnas.Add(ColFolioFacturaEncabezado);

        //Importe
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "80";
        GridFactura.Columnas.Add(ColTotal);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "80";
        GridFactura.Columnas.Add(ColTipoMoneda);

        //Fecha de emision
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Fecha de emisión";
        ColFechaEmision.Ancho = "80";
        ColFechaEmision.Alineacion = "Left";
        ColFechaEmision.Buscador = "false";
        GridFactura.Columnas.Add(ColFechaEmision);

        //Estatus
        CJQColumn ColEstatusFacturaEncabezado = new CJQColumn();
        ColEstatusFacturaEncabezado.Nombre = "EstatusFacturaEncabezado";
        ColEstatusFacturaEncabezado.Encabezado = "Estatus";
        ColEstatusFacturaEncabezado.Ancho = "80";
        ColEstatusFacturaEncabezado.Alineacion = "Left";
        ColEstatusFacturaEncabezado.Buscador = "true";
        GridFactura.Columnas.Add(ColEstatusFacturaEncabezado);

        //NotaCredito
        CJQColumn ColNotaCredito = new CJQColumn();
        ColNotaCredito.Nombre = "NotaCredito";
        ColNotaCredito.Encabezado = "Nota Credito";
        ColNotaCredito.Ancho = "50";
        ColNotaCredito.Buscador = "false";
        GridFactura.Columnas.Add(ColNotaCredito);

		CJQColumn ColDivision = new CJQColumn();
		ColDivision.Nombre = "IdDivision";
		ColDivision.Encabezado = "División";
		ColDivision.Ancho = "80";
		ColDivision.Buscador = "true";
		ColDivision.TipoBuscador = "Combo";
		ColDivision.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Division";
		GridFactura.Columnas.Add(ColDivision);

		CJQColumn ColProyecto = new CJQColumn();
		ColProyecto.Nombre = "IdProyecto";
		ColProyecto.Encabezado = "Proyecto";
		ColProyecto.Ancho = "80";
		//GridFactura.Columnas.Add(ColProyecto);

		CJQColumn ColPedido = new CJQColumn();
		ColPedido.Nombre = "Folio";
		ColPedido.Encabezado = "Pedido";
		ColPedido.Ancho = "80";
		//GridFactura.Columnas.Add(ColPedido);


        //Razon social
        CJQColumn ColCliente = new CJQColumn();
        ColCliente.Nombre = "RazonSocial";
        ColCliente.Encabezado = "Razón social";
        ColCliente.Ancho = "150";
        ColCliente.Alineacion = "Left";
        ColCliente.Buscador = "true";
        GridFactura.Columnas.Add(ColCliente);

        //Agente
        CJQColumn ColAgente = new CJQColumn();
        ColAgente.Nombre = "Agente";
        ColAgente.Encabezado = "Agente";
        ColAgente.Ancho = "150";
        ColAgente.Alineacion = "Left";
        ColAgente.Buscador = "true";
        GridFactura.Columnas.Add(ColAgente);

        //Timbrado
        CJQColumn Timbrado = new CJQColumn();
        Timbrado.Nombre = "Timbrado";
        Timbrado.Encabezado = "Timbrado";
        Timbrado.Ordenable = "true";
        Timbrado.Ancho = "50";
        Timbrado.Etiquetado = "EstatusTimbradoFile";
        Timbrado.Buscador = "false";
        Timbrado.StoredProcedure.CommandText = "spc_ManejadorConvertirAPedido_Consulta";
        GridFactura.Columnas.Add(Timbrado);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        //ColBaja.Oculto = puedeEliminarFacturaEncabezado == 1 ? "false" : "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarFacturaEncabezado == 1 ? "A/I" : "A/IDeshabilitado";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridFactura.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarFacturaEncabezado";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridFactura.Columnas.Add(ColConsultar);

        //Formato
        CJQColumn ColFormato = new CJQColumn();
        ColFormato.Nombre = "Formato";
        ColFormato.Encabezado = "Imprimir";
        ColFormato.Etiquetado = "Imagen";
        ColFormato.Imagen = "Imprimir.png";
        ColFormato.Estilo = "divImagenConsultar imgFormaConsultarFacturaFormato";
        ColFormato.Buscador = "false";
        ColFormato.Ordenable = "false";
        ColFormato.Ancho = "50";
        GridFactura.Columnas.Add(ColFormato);

        //XML
        CJQColumn ColXML = new CJQColumn();
        ColXML.Nombre = "XML";
        ColXML.Encabezado = "XML";
        ColXML.Etiquetado = "Imagen";
        ColXML.Imagen = "xml-file.png";
        ColXML.Estilo = "divImagenConsultar imgFormaConsultarFacturaXML";
        ColXML.Buscador = "false";
        ColXML.Ordenable = "false";
        ColXML.Ancho = "50";
        GridFactura.Columnas.Add(ColXML); 

        ////Addenda
        //CJQColumn ColAddenda = new CJQColumn();
        //ColAddenda.Nombre = "Addenda";
        //ColAddenda.Encabezado = "Addenda";
        //ColAddenda.Etiquetado = "Imagen";
        //ColAddenda.Imagen = "Imprimir.png";
        //ColAddenda.Estilo = "divImagenConsultar imgFormaConsultarFacturaAddenda";
        //ColAddenda.Buscador = "false";
        //ColAddenda.Ordenable = "false";
        //ColAddenda.Ancho = "50";
        //GridFactura.Columnas.Add(ColAddenda); 

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturas", GridFactura.GeneraGrid(), true);

		GridFacturasDetalle(ClientScript, this, ConexionBaseDatos);
        PintaGridDetallePedido();
        PintaGridConceptoProyecto();
        PintaGridDetalleFacturar();
        PintaGridDetalleFacturarConsultar();
        PintaGridDetalleFacturarEditar();
        PintaGridFacturasSustituye();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

	public static void GridFacturasDetalle(ClientScriptManager ClientScript, Page Pagina, CConexion pConexion)
	{

		CJQGrid GridFactura_Detalle = new CJQGrid();
		GridFactura_Detalle.NombreTabla = "grdFacturas_Detalle";
		GridFactura_Detalle.CampoIdentificador = "IdFacturaEncabezado";
		GridFactura_Detalle.ColumnaOrdenacion = "NumeroFactura";
		GridFactura_Detalle.Metodo = "ObtenerFacturas_Detalle";
		GridFactura_Detalle.GenerarFuncionFiltro = false;
		GridFactura_Detalle.GenerarFuncionTerminado = false;
		GridFactura_Detalle.GenerarGridCargaInicial = false;
		GridFactura_Detalle.TituloTabla = "Facturas Detalle";
		GridFactura_Detalle.ColumnaOrdenacion = "IdFacturaEncabezado";
		GridFactura_Detalle.TipoOrdenacion = "DESC";

		CJQColumn ColIdFacturaEncabezado = new CJQColumn();
		ColIdFacturaEncabezado.Nombre = "IdFacturaEncabezado";
		ColIdFacturaEncabezado.Encabezado = "IdFacturaEncabezado";
		ColIdFacturaEncabezado.Buscador = "false";
		ColIdFacturaEncabezado.Oculto = "true";
		GridFactura_Detalle.Columnas.Add(ColIdFacturaEncabezado);

		CJQColumn ColIdFacturaDetalle = new CJQColumn();
		ColIdFacturaDetalle.Nombre = "IdFacturaDetalle";
		ColIdFacturaDetalle.Encabezado = "IdFacturaDetalle";
		ColIdFacturaDetalle.Buscador = "false";
		ColIdFacturaDetalle.Oculto = "true";
		GridFactura_Detalle.Columnas.Add(ColIdFacturaDetalle);

		CJQColumn ColSerie = new CJQColumn();
		ColSerie.Nombre = "Serie";
		ColSerie.Encabezado = "Serie";
		ColSerie.Ancho = "80";
		ColSerie.Buscador = "false";
		GridFactura_Detalle.Columnas.Add(ColSerie);

		CJQColumn ColNumeroFactura = new CJQColumn();
		ColNumeroFactura.Nombre = "NumeroFactura";
		ColNumeroFactura.Encabezado = "No. Factura";
		ColNumeroFactura.Ancho = "80";
		ColNumeroFactura.Buscador = "false";
		GridFactura_Detalle.Columnas.Add(ColNumeroFactura);

		CJQColumn ColClave = new CJQColumn();
		ColClave.Nombre = "Clave";
		ColClave.Encabezado = "Clave";
		GridFactura_Detalle.Columnas.Add(ColClave);

		CJQColumn ColCantidad = new CJQColumn();
		ColCantidad.Nombre = "Cantidad";
		ColCantidad.Encabezado = "Cantidad";
		ColCantidad.Buscador = "false";
		GridFactura_Detalle.Columnas.Add(ColCantidad);

		CJQColumn ColDescripcion = new CJQColumn();
		ColDescripcion.Nombre = "Descripcion";
		ColDescripcion.Encabezado = "Descripcion";
		GridFactura_Detalle.Columnas.Add(ColDescripcion);

		CJQColumn ColPrecioUnitario = new CJQColumn();
		ColPrecioUnitario.Nombre = "PrecioUnitario";
		ColPrecioUnitario.Encabezado = "Precio Unitario";
		ColPrecioUnitario.Buscador = "false";
		ColPrecioUnitario.Ancho = "80";
		GridFactura_Detalle.Columnas.Add(ColPrecioUnitario);

		CJQColumn ColTotal = new CJQColumn();
		ColTotal.Nombre = "Total";
		ColTotal.Encabezado = "Total";
		ColTotal.Buscador = "false";
		ColTotal.Ancho = "80";
		GridFactura_Detalle.Columnas.Add(ColTotal);

		CJQColumn ColMoneda = new CJQColumn();
		ColMoneda.Nombre = "Moneda";
		ColMoneda.Encabezado = "Moneda";
		ColMoneda.Buscador = "false";
		ColMoneda.Ancho = "110";
		GridFactura_Detalle.Columnas.Add(ColMoneda);

		CJQColumn ColFechaEmision = new CJQColumn();
		ColFechaEmision.Nombre = "FechaEmision";
		ColFechaEmision.Encabezado = "Fecha";
		ColFechaEmision.Buscador = "false";
		ColFechaEmision.Ancho = "110";
		GridFactura_Detalle.Columnas.Add(ColFechaEmision);

		CJQColumn ColEstatus = new CJQColumn();
		ColEstatus.Nombre = "Estatus";
		ColEstatus.Encabezado = "Estatus";
		ColEstatus.Buscador = "false";
		ColEstatus.Ancho = "100";
		GridFactura_Detalle.Columnas.Add(ColEstatus);

		CJQColumn ColDivision = new CJQColumn();
		ColDivision.Nombre = "Division";
		ColDivision.Encabezado = "Division";
		ColDivision.Buscador = "false";
		GridFactura_Detalle.Columnas.Add(ColDivision);

		CJQColumn ColCliente = new CJQColumn();
		ColCliente.Nombre = "Cliente";
		ColCliente.Encabezado = "Cliente";
		ColCliente.Buscador = "false";
		GridFactura_Detalle.Columnas.Add(ColCliente);

		CJQColumn ColAgente = new CJQColumn();
		ColAgente.Nombre = "Agente";
		ColAgente.Encabezado = "Agente";
		ColAgente.Buscador = "false";
		GridFactura_Detalle.Columnas.Add(ColAgente);

		CJQColumn ColSucursal = new CJQColumn();
		ColSucursal.Nombre = "Sucursal";
		ColSucursal.Encabezado = "Sucursal";
		ColSucursal.Buscador = "false";
		GridFactura_Detalle.Columnas.Add(ColSucursal);

		ClientScript.RegisterStartupScript(Pagina.GetType(), "grdFacturas_Detalle", GridFactura_Detalle.GeneraGrid(), true);

	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerFacturas_Detalle(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerieFactura,
		string pNumeroFactura, string pFechaInicial, string pFechaFinal, int pPorFecha, int pAI, int pFiltroTimbrado, int pIdDivision, string pRazonSocial, string pAgente,
		string pNumeroPedido, int pBusquedaDocumento, string pEstatusFacturaEncabezado, int pFolio, int pIdProyecto, string pDescripcion, string pClave)
	{
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdFacturas_Detalle", sqlCon);
		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieFactura);
		Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
		Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
		Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
		Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
		Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
		Stored.Parameters.Add("pFiltroTimbrado", SqlDbType.Int).Value = pFiltroTimbrado;
		Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
		Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
		Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
		Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
		Stored.Parameters.Add("pBusquedaDocumento", SqlDbType.Int).Value = pBusquedaDocumento;
		Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
		Stored.Parameters.Add("pEstatusFacturaEncabezado", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstatusFacturaEncabezado);
		Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 250).Value = pDescripcion;
		Stored.Parameters.Add("pClave", SqlDbType.VarChar, 250).Value = pClave;

		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		return new CJQGridJsonResponse(dataSet);
	}

	[WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerieFactura, string pNumeroFactura, string pFechaInicial, string pFechaFinal, int pPorFecha, int pAI, int pFiltroTimbrado, int pIdDivision, string pRazonSocial, string pAgente, string pNumeroPedido, int pBusquedaDocumento, string pEstatusFacturaEncabezado, int pFolio, int pIdProyecto)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturas", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieFactura);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pFiltroTimbrado", SqlDbType.Int).Value = pFiltroTimbrado;
		Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
		Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pBusquedaDocumento", SqlDbType.Int).Value = pBusquedaDocumento;
        Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
        Stored.Parameters.Add("pEstatusFacturaEncabezado", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstatusFacturaEncabezado);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerPedidoDetalle(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPedido,
		int pIdTipoMonedaFactura, decimal pTipoCambioFactura, int pIdTipoMonedaPedido, decimal pTipoCambioPedido, int NuevoCotizador)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		string StoreProcedure = "spg_grdPedidoDetalle";
		if (NuevoCotizador == 1)
		{
			StoreProcedure = "spg_grdPedidoDetalle_NuevoCotizador";
		}
        SqlCommand Stored = new SqlCommand(StoreProcedure, sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdPedido", SqlDbType.Int).Value = pIdPedido;
        Stored.Parameters.Add("pIdTipoMonedaFactura", SqlDbType.Int).Value = pIdTipoMonedaFactura;
        Stored.Parameters.Add("pTipoCambioFactura", SqlDbType.Decimal).Value = pTipoCambioFactura;
        Stored.Parameters.Add("pIdTipoMonedaPedido", SqlDbType.Int).Value = pIdTipoMonedaPedido;
        Stored.Parameters.Add("pTipoCambioPedido", SqlDbType.Decimal).Value = pTipoCambioPedido;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerConceptoProyecto(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdProyecto, int pIdTipoMonedaFactura, decimal pTipoCambioFactura)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdConceptoProyectoFactura", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
        Stored.Parameters.Add("pIdTipoMonedaFactura", SqlDbType.Int).Value = pIdTipoMonedaFactura;
        Stored.Parameters.Add("pTipoCambioFactura", SqlDbType.Decimal).Value = pTipoCambioFactura;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturasSustituye(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdFacturaEncabezado)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturasSustituye", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdFacturaEncabezado", SqlDbType.Int).Value = pIdFacturaEncabezado;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturaDetalle(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdFacturaEncabezado)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaDetalle", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdFacturaEncabezado", SqlDbType.Int, 4).Value = pIdFacturaEncabezado;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturaDetalleConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdFacturaEncabezado)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaDetalle", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdFacturaEncabezado", SqlDbType.Int, 4).Value = pIdFacturaEncabezado;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturaDetalleEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdFacturaEncabezado)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaDetalle", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdFacturaEncabezado", SqlDbType.Int, 4).Value = pIdFacturaEncabezado;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerContactosOrganizacion(int pIdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CContactoOrganizacion.ObtenerJsonContactoOrganizacionFiltroIdCliente(pIdCliente, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");

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
    public static string BuscarNumeroFactura(string pNumeroFactura)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_FacturaCliente_ConsultarNumeroFactura";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", pNumeroFactura);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

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
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
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
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarEstatusFacturaEncabezado(string pEstatusFacturaEncabezado)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CSerieFactura jsonSerieFactura = new CSerieFactura();
        jsonSerieFactura.StoredProcedure.CommandText = "sp_FacturaEncabezado_EstatusFacturaEncabezadoConsultar";
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("pEstatusFacturaEncabezado", pEstatusFacturaEncabezado);
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("pNombreColumna", "EstatusFacturaEncabezado");
        string json = jsonSerieFactura.ObtenerJsonSerieFacturaConsulta(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return json;
    }

    [WebMethod]
    public static string BuscarAgente(string pAgente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario JsonAgente = new CUsuario();
        JsonAgente.StoredProcedure.CommandText = "sp_ConsultarFiltros_FacturaAgente";
        JsonAgente.StoredProcedure.Parameters.AddWithValue("@pAgente", pAgente);
        return JsonAgente.ObtenerJsonAgente(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarProyectoCliente(string pNombreProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CProyecto jsonProyecto = new CProyecto();
        jsonProyecto.StoredProcedure.CommandText = "sp_Proyecto_ConsultarNombreProyecto_Facturacion";
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@pNombreProyecto", pNombreProyecto);
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonProyecto.ObtenerJsonNombreProyecto(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarSerieFactura(string pSerieFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CSerieFactura jsonSerieFactura = new CSerieFactura();
        jsonSerieFactura.StoredProcedure.CommandText = "sp_FacturaEncabezado_SerieFacturaConsultar";
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("pSerieFactura", pSerieFactura);
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("pNombreColumna", "SerieFactura");
        string json = jsonSerieFactura.ObtenerJsonSerieFacturaConsulta(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return json;
    }

    [WebMethod]
    public static string AgregarDetalleFactura(Dictionary<string, object> pFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida" && HttpContext.Current.Session["IdUsuario"] != null)
        {
            CFacturaEncabezado FacturaCliente = new CFacturaEncabezado();
            CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CCliente Cliente = new CCliente();
            CProyecto Proyecto = new CProyecto();
            COrganizacion Organizacion = new COrganizacion();
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            CTipoCambio TipoCambio = new CTipoCambio();
            CCotizacion CotizacionFactura = new CCotizacion();

            FacturaCliente.LlenaObjeto(Convert.ToInt32(pFactura["IdFacturaEncabezado"]), ConexionBaseDatos);
            FacturaDetalle.IdFacturaEncabezado = Convert.ToInt32(pFactura["IdFacturaEncabezado"]);
            DireccionOrganizacion.LlenaObjeto(Convert.ToInt32(pFactura["IdDireccionOrganizacion"]), ConexionBaseDatos);

            CotizacionDetalle.LlenaObjeto(Convert.ToInt32(pFactura["IdCotizacionDetalle"]), ConexionBaseDatos);
            CotizacionFactura.LlenaObjeto(CotizacionDetalle.IdCotizacion, ConexionBaseDatos);
            
            Dictionary<string, object> ParametrosFTC = new Dictionary<string, object>();
            ParametrosFTC.Add("Fecha", DateTime.Today);
            ParametrosFTC.Add("IdTipoMonedaOrigen", CotizacionFactura.IdTipoMoneda);
            ParametrosFTC.Add("IdTipoMonedaDestino", Convert.ToInt32(pFactura["IdTipoMoneda"]));
            TipoCambio.LlenaObjetoFiltros(ParametrosFTC, ConexionBaseDatos);

			decimal Cantidad = Convert.ToDecimal(pFactura["CantidadFacturar"]);

			int NuevoCotizador = Convert.ToInt32(pFactura["NuevoCotizador"]);

            if (Convert.ToInt32(pFactura["IdCotizacionDetalle"]) != 0)
            {
                FacturaDetalle.IdCotizacion = CotizacionDetalle.IdCotizacion;
                FacturaDetalle.IdCotizacionDetalle = CotizacionDetalle.IdCotizacionDetalle;

                FacturaDetalle.Cantidad = Cantidad;
                FacturaDetalle.PrecioUnitario = Convert.ToDecimal(pFactura["PrecioUnitario"]);
                FacturaDetalle.Total = Convert.ToDecimal(pFactura["Total"]);
                
                FacturaDetalle.Descuento = CotizacionDetalle.Descuento;

                if (Convert.ToInt32(pFactura["IdProducto"]) != 0)
                {
                    Producto.LlenaObjeto(Convert.ToInt32(pFactura["IdProducto"]), ConexionBaseDatos);
                    FacturaDetalle.IdProducto = Producto.IdProducto;
                    FacturaDetalle.Clave = Convert.ToString(CotizacionDetalle.Clave);
                    FacturaDetalle.Descripcion = Convert.ToString(CotizacionDetalle.Descripcion);
                }
                else
                {
                    Servicio.LlenaObjeto(Convert.ToInt32(pFactura["IdServicio"]), ConexionBaseDatos);
                    FacturaDetalle.IdServicio = Servicio.IdServicio;
                    FacturaDetalle.Clave = Convert.ToString(CotizacionDetalle.Clave);
                    FacturaDetalle.Descripcion = Convert.ToString(CotizacionDetalle.Descripcion);
                }
            }

            FacturaDetalle.SinIVA = Convert.ToInt32(pFactura["SinIVA"]);

            string cadena = "";
            if (Convert.ToInt32(pFactura["IdProyecto"]) != 0)
            {
                foreach (string oIds in (Array)pFactura["IdsConceptosProyectos"])
                {
                    cadena = cadena + Convert.ToString(oIds) + ",";
                }
                cadena = cadena.Substring(0, cadena.Length - 1);
                FacturaDetalle.IdProyecto = Convert.ToInt32(pFactura["IdProyecto"]);

                FacturaDetalle.IdsConceptosProyectos = Convert.ToString(cadena);
            }
            else
            {
                FacturaDetalle.IdTipoIVA = Convert.ToInt32(pFactura["IdTipoIVA"]);
                FacturaDetalle.IVA = Convert.ToDecimal(pFactura["IVA"]);
            }

            FacturaDetalle.IdTipoMoneda = Convert.ToInt32(pFactura["IdTipoMoneda"]);
            FacturaDetalle.TipoCambio = Convert.ToDecimal(pFactura["TipoCambioFactura"]);

            FacturaDetalle.IdDescuentoCliente = Convert.ToInt32(pFactura["IdDescuentoCliente"]);
            FacturaDetalle.PorcentajeDescuento = Convert.ToDecimal(pFactura["PorcentajeDescuento"]);

            string validacion = ValidarDetalleFactura(FacturaDetalle, ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
				if (NuevoCotizador == 1)
				{
					CPresupuestoConcepto Concepto = new CPresupuestoConcepto();
					Concepto.LlenaObjeto(CotizacionDetalle.IdCotizacionDetalle, ConexionBaseDatos);
					FacturaDetalle.IdCotizacionDetalle = Concepto.IdPresupuestoConcepto;
					FacturaDetalle.IdCotizacion = Concepto.IdPresupuesto;
					FacturaDetalle.Cantidad = Cantidad;
					FacturaDetalle.PrecioUnitario = Convert.ToDecimal(pFactura["PrecioUnitario"]);
					FacturaDetalle.Total = Convert.ToDecimal(pFactura["Total"]);

					FacturaDetalle.Descuento = CotizacionDetalle.Descuento;

					FacturaDetalle.IdProducto = 0;
					FacturaDetalle.Clave = Concepto.Clave;
					FacturaDetalle.Descripcion = Concepto.Descripcion;

					FacturaDetalle.SinIVA = Convert.ToInt32(pFactura["SinIVA"]);

					if (Convert.ToInt32(pFactura["IdProyecto"]) != 0)
					{
						foreach (string oIds in (Array)pFactura["IdsConceptosProyectos"])
						{
							cadena = cadena + Convert.ToString(oIds) + ",";
						}
						cadena = cadena.Substring(0, cadena.Length - 1);
						FacturaDetalle.IdProyecto = Convert.ToInt32(pFactura["IdProyecto"]);

						FacturaDetalle.IdsConceptosProyectos = Convert.ToString(cadena);
					}
					else
					{
						FacturaDetalle.IdTipoIVA = Convert.ToInt32(pFactura["IdTipoIVA"]);
						FacturaDetalle.IVA = Convert.ToDecimal(pFactura["IVA"]);
					}

					FacturaDetalle.IdTipoMoneda = Convert.ToInt32(pFactura["IdTipoMoneda"]);
					FacturaDetalle.TipoCambio = Convert.ToDecimal(pFactura["TipoCambioFactura"]);

					FacturaDetalle.IdDescuentoCliente = Convert.ToInt32(pFactura["IdDescuentoCliente"]);
					FacturaDetalle.PorcentajeDescuento = Convert.ToDecimal(pFactura["PorcentajeDescuento"]);
					Concepto.FacturacionCantidad -= Cantidad;
					Concepto.Editar(ConexionBaseDatos);
				}

				if (FacturaDetalle.IdFacturaEncabezado != 0)
                {
                    CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
                    FacturaEncabezado.LlenaObjeto(FacturaDetalle.IdFacturaEncabezado, ConexionBaseDatos);
					
					FacturaDetalle.AgregarDetalleFactura(ConexionBaseDatos);

					int noPartidasPendientes = FacturaEncabezado.ExistenPartidasPendientesCotizacion(FacturaDetalle.IdFacturaEncabezado, ConexionBaseDatos);

                    CSerieFactura SerieFactura = new CSerieFactura();
                    SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, ConexionBaseDatos);

                    if (SerieFactura.Timbrado == false && noPartidasPendientes == 0)
                    {
                        FacturaEncabezado = new CFacturaEncabezado();
                        //FacturaEncabezado.ActualizarEstatusFacturadoCotizacion(FacturaDetalle.IdFacturaEncabezado, 6, ConexionBaseDatos);

                        Dictionary<string, object> ParametrosFD = new Dictionary<string, object>();
                        ParametrosFD.Add("IdFacturaEncabezado", FacturaDetalle.IdFacturaEncabezado);
                        foreach (CFacturaDetalle oFacturaDetalle in FacturaDetalle.LlenaObjetosFiltros(ParametrosFD, ConexionBaseDatos))
                        {
                            if (oFacturaDetalle.IdCotizacion != 0)
                            {
                                CCotizacion CotizacionOportunidad = new CCotizacion();
                                CotizacionOportunidad.LlenaObjeto(oFacturaDetalle.IdCotizacion, ConexionBaseDatos);
                                COportunidad.ActualizarTotalesOportunidad(CotizacionOportunidad.IdOportunidad, ConexionBaseDatos);
                            }
                            else
                            {
                                CProyecto ProyectoOportunidad = new CProyecto();
                                ProyectoOportunidad.LlenaObjeto(oFacturaDetalle.IdProyecto, ConexionBaseDatos);
                                COportunidad.ActualizarTotalesOportunidad(ProyectoOportunidad.IdOportunidad, ConexionBaseDatos);
                            }
                        }
                    }

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = FacturaDetalle.IdFacturaDetalle;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva partida de factura de cliente";
                    HistorialGenerico.AgregarHistorialGenerico("FacturaDetalle", ConexionBaseDatos);

                }
                else
                {
                    CSerieFactura SerieFactura = new CSerieFactura();
                    CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
                    FacturaEncabezado.RegimenFiscal = Convert.ToString(pFactura["RegimenFiscal"]);
                    FacturaEncabezado.LugarExpedicion = Convert.ToString(pFactura["LugarExpedicion"]);
                    FacturaEncabezado.NumeroFactura = Convert.ToInt32(0);
                    FacturaEncabezado.NumeroOrdenCompra = Convert.ToString(pFactura["NumeroOrdenCompra"]);
                    FacturaEncabezado.IdSerieFactura = Convert.ToInt32(pFactura["IdSerieFactura"]);
                    FacturaEncabezado.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    FacturaEncabezado.IdUsuarioAgente = Convert.ToInt32(pFactura["IdUsuarioAgente"]);
                    FacturaEncabezado.IdCliente = Convert.ToInt32(pFactura["IdCliente"]);
                    FacturaEncabezado.IdContactoCliente = Convert.ToInt32(pFactura["IdContactoCliente"]);

                    SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, ConexionBaseDatos);
                    FacturaEncabezado.Serie = SerieFactura.SerieFactura;

                    FacturaEncabezado.RazonSocial = Convert.ToString(pFactura["RazonSocial"]);
                    FacturaEncabezado.RFC = Convert.ToString(pFactura["RFC"]);
                    FacturaEncabezado.FechaRequeridaFacturacion = Convert.ToDateTime(pFactura["FechaFacturar"]);
                    FacturaEncabezado.IdMetodoPago = Convert.ToInt32(pFactura["IdMetodoPago"]);
                    FacturaEncabezado.MetodoPago = Convert.ToString(pFactura["MetodoPago"]);
                    FacturaEncabezado.IdCondicionPago = Convert.ToInt32(pFactura["IdCondicionPago"]);
                    FacturaEncabezado.CondicionPago = Convert.ToString(pFactura["CondicionPago"]);
                    FacturaEncabezado.EsRefactura = Convert.ToBoolean(pFactura["EsRefactura"]);
                    FacturaEncabezado.Parcialidades = Convert.ToBoolean(pFactura["Parcialidades"]);


                    if (FacturaEncabezado.Parcialidades == true)
                    {
                        FacturaEncabezado.NumeroParcialidades = Convert.ToInt32(pFactura["NoParcialidades"]);
                        FacturaEncabezado.NumeroParcialidadesPendientes = Convert.ToInt32(pFactura["NoParcialidades"]);
                    }
                    else
                    {
                        FacturaEncabezado.NumeroParcialidades = 1;
                        FacturaEncabezado.NumeroParcialidadesPendientes = 1;
                    }
                    FacturaEncabezado.Nota = Convert.ToString(pFactura["NotaFactura"]);
                    FacturaEncabezado.FechaEmision = Convert.ToDateTime(pFactura["FechaActual"]);
                    FacturaEncabezado.FechaPago = Convert.ToDateTime(pFactura["FechaPago"]);

                    FacturaEncabezado.IdNumeroCuenta = Convert.ToInt32(pFactura["IdNumeroCuenta"]);
					CCuentaBancariaCliente NumeroCuenta = new CCuentaBancariaCliente();
					NumeroCuenta.LlenaObjeto(FacturaEncabezado.IdNumeroCuenta, ConexionBaseDatos);
					FacturaEncabezado.NumeroCuenta = NumeroCuenta.CuentaBancariaCliente;

                    FacturaEncabezado.CalleFiscal = Convert.ToString(pFactura["CalleFiscal"]);
                    FacturaEncabezado.NumeroExteriorFiscal = Convert.ToString(pFactura["NumeroExteriorFiscal"]);
                    FacturaEncabezado.NumeroInteriorFiscal = Convert.ToString(pFactura["NumeroInteriorFiscal"]);
                    FacturaEncabezado.ColoniaFiscal = Convert.ToString(pFactura["ColoniaFiscal"]);
                    FacturaEncabezado.CodigoPostalFiscal = Convert.ToString(pFactura["CodigoPostalFiscal"]);
                    FacturaEncabezado.PaisFiscal = Convert.ToString(pFactura["PaisFiscal"]);
                    FacturaEncabezado.EstadoFiscal = Convert.ToString(pFactura["EstadoFiscal"]);
                    FacturaEncabezado.MunicipioFiscal = Convert.ToString(pFactura["MunicipioFiscal"]);
                    FacturaEncabezado.LocalidadFiscal = Convert.ToString(pFactura["LocalidadFiscal"]);
                    FacturaEncabezado.ReferenciaFiscal = Convert.ToString(pFactura["ReferenciaFiscal"]);
                    FacturaEncabezado.IdMunicipioFiscal = DireccionOrganizacion.IdMunicipio;
                    FacturaEncabezado.IdLocalidadFiscal = DireccionOrganizacion.IdLocalidad;

                    if (Convert.ToInt32(pFactura["IdDireccionEntrega"]) != 0)
                    {
                        DireccionOrganizacion.LlenaObjeto(Convert.ToInt32(pFactura["IdDireccionEntrega"]), ConexionBaseDatos);
                        FacturaEncabezado.CalleEntrega = DireccionOrganizacion.Calle;
                        FacturaEncabezado.NumeroExteriorEntrega = DireccionOrganizacion.NumeroExterior;
                        FacturaEncabezado.NumeroInteriorEntrega = DireccionOrganizacion.NumeroInterior;
                        FacturaEncabezado.ColoniaEntrega = DireccionOrganizacion.Colonia;
                        FacturaEncabezado.CodigoPostalEntrega = DireccionOrganizacion.CodigoPostal;
                        FacturaEncabezado.ReferenciaEntrega = DireccionOrganizacion.Referencia;
                        CLocalidad Localidad = new CLocalidad();
                        Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, ConexionBaseDatos);
                        CMunicipio Municipio = new CMunicipio();
                        Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, ConexionBaseDatos);
                        CEstado Estado = new CEstado();
                        Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
                        CPais Pais = new CPais();
                        Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
                        FacturaEncabezado.PaisEntrega = Pais.Pais;
                        FacturaEncabezado.EstadoEntrega = Estado.Estado;
                        FacturaEncabezado.MunicipioEntrega = Municipio.Municipio;
                        FacturaEncabezado.LocalidadEntrega = Localidad.Localidad;
                        FacturaEncabezado.IdMunicipioEntrega = DireccionOrganizacion.IdMunicipio;
                        FacturaEncabezado.IdLocalidadEntrega = DireccionOrganizacion.IdLocalidad;
                    }

                    FacturaEncabezado.IdDivision = Convert.ToInt32(pFactura["IdDivision"]);
                    FacturaEncabezado.IdTipoMoneda = Convert.ToInt32(pFactura["IdTipoMoneda"]);
                    FacturaEncabezado.IdEstatusFacturaEncabezado = 1;
                    FacturaEncabezado.TipoCambio = Convert.ToDecimal(pFactura["TipoCambioFactura"]);
                    FacturaEncabezado.AgregarFacturaEncabezado(ConexionBaseDatos);

                    CCotizacion Cotizacion = new CCotizacion();
                    Cotizacion.LlenaObjeto(FacturaDetalle.IdCotizacion, ConexionBaseDatos);

                    CFacturaEncabezadoSucursal FacturaEncabezadoSucursal = new CFacturaEncabezadoSucursal();
                    FacturaEncabezadoSucursal.IdFacturaEncabezado = FacturaEncabezado.IdFacturaEncabezado;
                    FacturaEncabezadoSucursal.IdSucursal = Usuario.IdSucursalActual;
                    FacturaEncabezadoSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    FacturaEncabezadoSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    FacturaEncabezadoSucursal.Agregar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = FacturaEncabezado.IdFacturaEncabezado;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva factura de cliente";
                    HistorialGenerico.AgregarHistorialGenerico("FacturaEncabezado", ConexionBaseDatos);

                    FacturaDetalle.IdFacturaEncabezado = FacturaEncabezado.IdFacturaEncabezado;
                    FacturaDetalle.AgregarDetalleFactura(ConexionBaseDatos);

                    FacturaDetalle.Total = Cotizacion.Total * FacturaEncabezado.TipoCambio;

                    HistorialGenerico.IdGenerico = FacturaDetalle.IdFacturaDetalle;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva partida de factura de cliente";
                    HistorialGenerico.AgregarHistorialGenerico("FacturaDetalle", ConexionBaseDatos);

                    int noPartidasPendientes = FacturaEncabezado.ExistenPartidasPendientesCotizacion(FacturaDetalle.IdFacturaEncabezado, ConexionBaseDatos);
                    if (SerieFactura.Timbrado == false && noPartidasPendientes == 0)
                    {
                        FacturaEncabezado = new CFacturaEncabezado();
                        FacturaEncabezado.ActualizarEstatusFacturadoCotizacion(FacturaDetalle.IdFacturaEncabezado, 6, ConexionBaseDatos);

                        Dictionary<string, object> ParametrosFD = new Dictionary<string, object>();
                        ParametrosFD.Add("IdFacturaEncabezado", FacturaDetalle.IdFacturaEncabezado);
                        foreach (CFacturaDetalle oFacturaDetalle in FacturaDetalle.LlenaObjetosFiltros(ParametrosFD, ConexionBaseDatos))
                        {
                            if (oFacturaDetalle.IdCotizacion != 0)
                            {
                                CCotizacion CotizacionOportunidad = new CCotizacion();
                                CotizacionOportunidad.LlenaObjeto(oFacturaDetalle.IdCotizacion, ConexionBaseDatos);
                                COportunidad.ActualizarTotalesOportunidad(CotizacionOportunidad.IdOportunidad, ConexionBaseDatos);
                            }
                            else
                            {
                                CProyecto ProyectoOportunidad = new CProyecto();
                                ProyectoOportunidad.LlenaObjeto(oFacturaDetalle.IdProyecto, ConexionBaseDatos);
                                COportunidad.ActualizarTotalesOportunidad(ProyectoOportunidad.IdOportunidad, ConexionBaseDatos);
                            }
                        }
                    }

                }

                string TotalLetras = "";

                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CFacturaEncabezado FacturaEncabezadoTotal = new CFacturaEncabezado();
                FacturaEncabezadoTotal.LlenaObjeto(Convert.ToInt32(FacturaDetalle.IdFacturaEncabezado), ConexionBaseDatos);

                decimal Descuento = 0;
                decimal Taza = 0;
                int Partidas = 0;

                CFacturaDetalle partidas = new CFacturaDetalle();

                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdFacturaEncabezado", FacturaDetalle.IdFacturaEncabezado);
                pParametros.Add("Baja", 0);

                foreach (CFacturaDetalle oDetalle in partidas.LlenaObjetosFiltros(pParametros, ConexionBaseDatos))
                {
                    Descuento = Descuento + ((oDetalle.Total * (oDetalle.Descuento / 100)) * TipoCambio.TipoCambio);
                    Taza += oDetalle.IVA;
                    Partidas++;
                }

                Taza = (Taza / Partidas) / 100;

                FacturaEncabezadoTotal.Descuento += Descuento;
                FacturaEncabezadoTotal.IVA = (FacturaEncabezadoTotal.Subtotal - FacturaEncabezadoTotal.Descuento) * Taza;
                FacturaEncabezadoTotal.Total = (FacturaEncabezadoTotal.Subtotal - FacturaEncabezadoTotal.Descuento) + FacturaEncabezadoTotal.IVA;
                
                FacturaEncabezadoTotal.PorcentajeDescuento = (FacturaEncabezadoTotal.Subtotal > 0) ? (FacturaEncabezadoTotal.Descuento / FacturaEncabezadoTotal.Subtotal) * 100 : 0;

                CSerieFactura SerieFacturaTotal = new CSerieFactura();
                SerieFacturaTotal.LlenaObjeto(FacturaEncabezadoTotal.IdSerieFactura, ConexionBaseDatos);

                TipoMoneda.LlenaObjeto(FacturaEncabezadoTotal.IdTipoMoneda, ConexionBaseDatos);
                string nomenclatura = "";
                if (FacturaEncabezadoTotal.IdTipoMoneda == 1)
                {
                    nomenclatura = " M.N.";
                }
                if (FacturaEncabezadoTotal.IdTipoMoneda == 2)
                {
                    nomenclatura = " USD";
                }
                TotalLetras = Utilerias.ConvertLetter(FacturaEncabezadoTotal.Total.ToString("C"), TipoMoneda.TipoMoneda.ToString());
                FacturaEncabezadoTotal.TotalLetra = TotalLetras + nomenclatura;
                FacturaEncabezadoTotal.Editar(ConexionBaseDatos);

                oRespuesta.Add("IdFacturaEncabezado", FacturaDetalle.IdFacturaEncabezado);
                oRespuesta.Add("SubtotalFactura", FacturaEncabezadoTotal.Subtotal);
                oRespuesta.Add("DescuentoFactura", FacturaEncabezadoTotal.Descuento);
                oRespuesta.Add("SubtotalDescuentoFactura", FacturaEncabezadoTotal.Subtotal - FacturaEncabezadoTotal.Descuento);
                oRespuesta.Add("IVAFactura", FacturaEncabezadoTotal.IVA);
                oRespuesta.Add("TotalFactura", FacturaEncabezadoTotal.Total);
                oRespuesta.Add("TotalLetraFactura", FacturaEncabezadoTotal.TotalLetra);
                oRespuesta.Add("NumeroFactura", FacturaEncabezadoTotal.NumeroFactura);

                if (SerieFacturaTotal.Timbrado == true)
                {
                    CCotizacion.ActualizarFacturado(FacturaDetalle.IdCotizacion, ConexionBaseDatos);
                    oRespuesta.Add("SerieTimbrado", 1);
                }
                else
                {
                    if (FacturaDetalle.IdCotizacion != 0 || FacturaDetalle.IdCotizacion != null)
                    {
                        CCotizacion Cotizacion = new CCotizacion();
                        Cotizacion.LlenaObjeto(FacturaDetalle.IdCotizacion, ConexionBaseDatos);

                        CCotizacionDetalle CotizacionPartidas = new CCotizacionDetalle();
                        Dictionary<string, object> pParametrosPartidas = new Dictionary<string, object>();
                        pParametrosPartidas.Add("IdCotizacion", Cotizacion.IdCotizacion);
                        pParametrosPartidas.Add("Baja", 0);

                        bool CotizacionFacturada = true;

                        foreach (CCotizacionDetalle Partida in CotizacionPartidas.LlenaObjetosFiltros(pParametrosPartidas, ConexionBaseDatos))
                        {
                            if (Partida.FacturacionCantidad != 0)
                            {
                                CotizacionFacturada = false;
                            }
                        }

                        if (CotizacionFacturada)
                        {
                            //Cotizacion.IdEstatusCotizacion = 6;
                        }
                        
                        Cotizacion.Editar(ConexionBaseDatos);
                    }
                    oRespuesta.Add("SerieTimbrado", 0);
                }

                // Actualiza Proyecto
                if (FacturaDetalle.IdProyecto != 0) {
                    CProyecto.ActualizarTotales(FacturaDetalle.IdProyecto, ConexionBaseDatos);
                }
                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                if (HttpContext.Current.Session["IdUsuario"] == null)
                {
                    oRespuesta.Add(new JProperty("Descripcion", "Ha expirado la sesión actual. Favor de iniciar sesión nuevamente."));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
                }
            }



            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarFacturaSustituye(Dictionary<string, object> pFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
            CFacturaEncabezadoSustituye FacturaEncabezadoSustituye = new CFacturaEncabezadoSustituye();

            FacturaEncabezadoSustituye.IdFactura = Convert.ToInt32(pFactura["IdFacturaEncabezado"]);
            FacturaEncabezado.IdSerieFactura = Convert.ToInt32(pFactura["IdSerieFactura"]);
            FacturaEncabezado.NumeroFactura = Convert.ToInt32(pFactura["NumeroFactura"]);

            string validacion = ValidarFacturaSustituye(FacturaEncabezado, ConexionBaseDatos);

            Dictionary<string, object> ParametrosFE = new Dictionary<string, object>();
            ParametrosFE.Add("IdSerieFactura", Convert.ToInt32(FacturaEncabezado.IdSerieFactura));
            ParametrosFE.Add("NumeroFactura", Convert.ToInt32(FacturaEncabezado.NumeroFactura));
            FacturaEncabezado.LlenaObjetoFiltros(ParametrosFE, ConexionBaseDatos);

            FacturaEncabezadoSustituye.IdFacturaSustituye = FacturaEncabezado.IdFacturaEncabezado;


            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                FacturaEncabezadoSustituye.Agregar(ConexionBaseDatos);

                CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
                Dictionary<string, object> ParametrosFD = new Dictionary<string, object>();
                ParametrosFD.Add("IdFacturaEncabezado", FacturaEncabezado.IdFacturaEncabezado);
                FacturaDetalle.LlenaObjetoFiltros(ParametrosFD, ConexionBaseDatos);

                CCotizacion Cotizacion = new CCotizacion();
                Cotizacion.LlenaObjeto(FacturaDetalle.IdCotizacion, ConexionBaseDatos);

                if (FacturaDetalle.IdCotizacion != 0)
                {
                    CCotizacion CotizacionOportunidad = new CCotizacion();
                    CotizacionOportunidad.LlenaObjeto(FacturaDetalle.IdCotizacion, ConexionBaseDatos);
                    COportunidad.ActualizarTotalesOportunidad(CotizacionOportunidad.IdOportunidad, ConexionBaseDatos);
                }
                else
                {
                    CProyecto ProyectoOportunidad = new CProyecto();
                    ProyectoOportunidad.LlenaObjeto(FacturaDetalle.IdProyecto, ConexionBaseDatos);
                    COportunidad.ActualizarTotalesOportunidad(ProyectoOportunidad.IdOportunidad, ConexionBaseDatos);
                }
                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EditarFacturaEncabezado(Dictionary<string, object> pFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CCliente Cliente = new CCliente();
            CProyecto Proyecto = new CProyecto();
            COrganizacion Organizacion = new COrganizacion();
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            DireccionOrganizacion.LlenaObjeto(Convert.ToInt32(pFactura["IdDireccionOrganizacion"]), ConexionBaseDatos);
            string validacion = "";

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                if (Convert.ToInt32(pFactura["IdFacturaEncabezado"]) != 0)
                {
                    CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
                    FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pFactura["IdFacturaEncabezado"]), ConexionBaseDatos);
                    FacturaEncabezado.RegimenFiscal = Convert.ToString(pFactura["RegimenFiscal"]);
                    FacturaEncabezado.LugarExpedicion = Convert.ToString(pFactura["LugarExpedicion"]);
                    FacturaEncabezado.NumeroFactura = Convert.ToInt32(pFactura["NumeroFactura"]);
                    FacturaEncabezado.NumeroOrdenCompra = Convert.ToString(pFactura["NumeroOrdenCompra"]);
                    FacturaEncabezado.IdSerieFactura = Convert.ToInt32(pFactura["IdSerieFactura"]);
                    FacturaEncabezado.IdContactoCliente = Convert.ToInt32(pFactura["IdContactoCliente"]);

                    CSerieFactura SerieFactura = new CSerieFactura();
                    SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, ConexionBaseDatos);
                    FacturaEncabezado.Serie = SerieFactura.SerieFactura;

                    FacturaEncabezado.IdUsuarioAgente = Convert.ToInt32(pFactura["IdUsuarioAgente"]);
                    FacturaEncabezado.IdCliente = Convert.ToInt32(pFactura["IdCliente"]);
                    FacturaEncabezado.RFC = Convert.ToString(pFactura["RFC"]);
                    FacturaEncabezado.FechaRequeridaFacturacion = Convert.ToDateTime(pFactura["FechaFacturar"]);
                    FacturaEncabezado.IdMetodoPago = Convert.ToInt32(pFactura["IdMetodoPago"]);
                    FacturaEncabezado.MetodoPago = Convert.ToString(pFactura["MetodoPago"]);
                    FacturaEncabezado.IdCondicionPago = Convert.ToInt32(pFactura["IdCondicionPago"]);
                    FacturaEncabezado.CondicionPago = Convert.ToString(pFactura["CondicionPago"]);
                    FacturaEncabezado.EsRefactura = Convert.ToBoolean(pFactura["EsRefactura"]);
                    FacturaEncabezado.Parcialidades = Convert.ToBoolean(pFactura["Parcialidades"]);

                    if (FacturaEncabezado.Parcialidades == true)
                    {
                        FacturaEncabezado.NumeroParcialidades = Convert.ToInt32(pFactura["NoParcialidades"]);
                    }
                    else
                    {
                        FacturaEncabezado.NumeroParcialidades = 1;
                    }
                    FacturaEncabezado.Nota = Convert.ToString(pFactura["NotaFactura"]);
                    FacturaEncabezado.FechaEmision = Convert.ToDateTime(pFactura["FechaActual"]);
                    FacturaEncabezado.FechaPago = Convert.ToDateTime(pFactura["FechaPago"]);

                    FacturaEncabezado.IdNumeroCuenta = Convert.ToInt32(pFactura["IdNumeroCuenta"]);
                    FacturaEncabezado.NumeroCuenta = Convert.ToString(pFactura["NumeroCuenta"]);

                    FacturaEncabezado.CalleFiscal = Convert.ToString(pFactura["CalleFiscal"]);
                    FacturaEncabezado.NumeroExteriorFiscal = Convert.ToString(pFactura["NumeroExteriorFiscal"]);
                    FacturaEncabezado.NumeroInteriorFiscal = Convert.ToString(pFactura["NumeroInteriorFiscal"]);
                    FacturaEncabezado.ColoniaFiscal = Convert.ToString(pFactura["ColoniaFiscal"]);
                    FacturaEncabezado.CodigoPostalFiscal = Convert.ToString(pFactura["CodigoPostalFiscal"]);
                    FacturaEncabezado.PaisFiscal = Convert.ToString(pFactura["PaisFiscal"]);
                    FacturaEncabezado.EstadoFiscal = Convert.ToString(pFactura["EstadoFiscal"]);
                    FacturaEncabezado.MunicipioFiscal = Convert.ToString(pFactura["MunicipioFiscal"]);
                    FacturaEncabezado.LocalidadFiscal = Convert.ToString(pFactura["LocalidadFiscal"]);
                    FacturaEncabezado.ReferenciaFiscal = Convert.ToString(pFactura["ReferenciaFiscal"]);
                    FacturaEncabezado.IdMunicipioFiscal = DireccionOrganizacion.IdMunicipio;
                    FacturaEncabezado.IdLocalidadFiscal = DireccionOrganizacion.IdLocalidad;

                    FacturaEncabezado.IdDescuentoCliente = Convert.ToInt32(pFactura["IdDescuentoCliente"]);
                    FacturaEncabezado.PorcentajeDescuento = Convert.ToDecimal(pFactura["PorcentajeDescuento"]);

                    CFacturaEncabezadoSucursal FacturaEncabezadoSucursal = new CFacturaEncabezadoSucursal();

                    Dictionary<string, object> ParametrosFES = new Dictionary<string, object>();
                    ParametrosFES.Add("IdFacturaEncabezado", Convert.ToInt32(FacturaEncabezado.IdFacturaEncabezado));
                    FacturaEncabezadoSucursal.LlenaObjetoFiltros(ParametrosFES, ConexionBaseDatos);
                    CSucursal Sucursal = new CSucursal();
                    Sucursal.LlenaObjeto(FacturaEncabezadoSucursal.IdSucursal, ConexionBaseDatos);

                    Decimal Descuento = 0;
                    Decimal IVA = 0;
                    Decimal Total = 0;
                    Descuento = ((FacturaEncabezado.Subtotal * FacturaEncabezado.PorcentajeDescuento)) / 100;

                    if (Convert.ToInt32(pFactura["SinIVA"]) == 1)
                    {
                        IVA = 0;
                        Total = (FacturaEncabezado.Subtotal - Descuento) + 0;
                    }
                    else
                    {
                        IVA = ((FacturaEncabezado.Subtotal - Descuento) * Sucursal.IVAActual) / 100;
                        Total = (FacturaEncabezado.Subtotal - Descuento) + IVA;
                    }

                    FacturaEncabezado.Descuento = Descuento;
                    FacturaEncabezado.IVA = IVA;
                    FacturaEncabezado.Total = Total;
                    FacturaEncabezado.SaldoFactura = Total;

                    if (Convert.ToInt32(pFactura["IdDireccionEntrega"]) != 0)
                    {
                        DireccionOrganizacion.LlenaObjeto(Convert.ToInt32(pFactura["IdDireccionEntrega"]), ConexionBaseDatos);
                        FacturaEncabezado.CalleEntrega = DireccionOrganizacion.Calle;
                        FacturaEncabezado.NumeroExteriorEntrega = DireccionOrganizacion.NumeroExterior;
                        FacturaEncabezado.NumeroInteriorEntrega = DireccionOrganizacion.NumeroInterior;
                        FacturaEncabezado.ColoniaEntrega = DireccionOrganizacion.Colonia;
                        FacturaEncabezado.CodigoPostalEntrega = DireccionOrganizacion.CodigoPostal;
                        FacturaEncabezado.ReferenciaEntrega = DireccionOrganizacion.Referencia;
                        CLocalidad Localidad = new CLocalidad();
                        Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, ConexionBaseDatos);
                        CMunicipio Municipio = new CMunicipio();
                        Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, ConexionBaseDatos);
                        CEstado Estado = new CEstado();
                        Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
                        CPais Pais = new CPais();
                        Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
                        FacturaEncabezado.PaisEntrega = Pais.Pais;
                        FacturaEncabezado.EstadoEntrega = Estado.Estado;
                        FacturaEncabezado.MunicipioEntrega = Municipio.Municipio;
                        FacturaEncabezado.LocalidadEntrega = Localidad.Localidad;
                        FacturaEncabezado.IdMunicipioEntrega = DireccionOrganizacion.IdMunicipio;
                        FacturaEncabezado.IdLocalidadEntrega = DireccionOrganizacion.IdLocalidad;
                    }
                    FacturaEncabezado.IdDivision = Convert.ToInt32(pFactura["IdDivision"]);
                    FacturaEncabezado.IdTipoMoneda = Convert.ToInt32(pFactura["IdTipoMoneda"]);
                    FacturaEncabezado.TipoCambio = Convert.ToDecimal(pFactura["TipoCambioFactura"]);
                    FacturaEncabezado.Editar(ConexionBaseDatos);

                    FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pFactura["IdFacturaEncabezado"]), ConexionBaseDatos);

                    string TotalLetras = "";

                    CUtilerias Utilerias = new CUtilerias();
                    CTipoMoneda TipoMoneda = new CTipoMoneda();

                    string terminacion = "";
                    if (FacturaEncabezado.IdTipoMoneda == 1)
                    {
                        terminacion = " M.N.";
                    }
                    else
                    {
                        terminacion = " USD";
                    }

                    TipoMoneda.LlenaObjeto(FacturaEncabezado.IdTipoMoneda, ConexionBaseDatos);
                    TotalLetras = Utilerias.ConvertLetter(FacturaEncabezado.Total.ToString(), TipoMoneda.TipoMoneda.ToString()) + terminacion;
                    FacturaEncabezado.TotalLetra = TotalLetras;

                    FacturaEncabezado.Editar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = FacturaEncabezado.IdFacturaEncabezado;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se edito la factura";
                    HistorialGenerico.AgregarHistorialGenerico("FacturaEncabezado", ConexionBaseDatos);

                    oRespuesta.Add(new JProperty("Error", 0));
                }
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string CancelarFacturaEncabezado(Dictionary<string, object> pFacturaEncabezado)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            if (FacturaEncabezado.ExisteFacturaEncabezadoMovimientos(Convert.ToInt32(pFacturaEncabezado["IdFacturaEncabezado"]), ConexionBaseDatos) == 0)
            {
                if (FacturaEncabezado.ExisteFacturaEncabezadoTimbrada(Convert.ToInt32(pFacturaEncabezado["IdFacturaEncabezado"]), ConexionBaseDatos) == 1)
                {
                    CUsuario Usuario = new CUsuario();
                    Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]),ConexionBaseDatos);
                    
                    int facturaMesAnterior = FacturaEncabezado.ValidarEsFacturaMesAnterior(Convert.ToInt32(pFacturaEncabezado["IdFacturaEncabezado"]), ConexionBaseDatos);
                    string cancelarFacturaMesAnterior = Usuario.TienePermisos(new string[] { "puedeCancelarFacturaMesAnterior" }, ConexionBaseDatos);

                    if (facturaMesAnterior == 0 || cancelarFacturaMesAnterior == "")
                    {
                        string validacion = CancelarArchivoBuzonFiscal(Convert.ToInt32(pFacturaEncabezado["IdFacturaEncabezado"]), ConexionBaseDatos);
                        if (validacion == "1")
                        {
                            CUtilerias Utilerias = new CUtilerias();
                            Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                            validacion = BuzonFiscalTimbradoCancelacion(Convert.ToInt32(pFacturaEncabezado["IdFacturaEncabezado"]), pFacturaEncabezado["MotivoCancelacion"].ToString(), ConexionBaseDatos);

                            CFacturaEncabezado FacturaEncabezadoEstatus = new CFacturaEncabezado();
                            FacturaEncabezadoEstatus.ActualizarEstatusFacturadoCotizacion(Convert.ToInt32(pFacturaEncabezado["IdFacturaEncabezado"]), 3, ConexionBaseDatos);

                            CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
                            Dictionary<string, object> ParametrosFD = new Dictionary<string, object>();
                            ParametrosFD.Add("IdFacturaEncabezado", Convert.ToInt32(pFacturaEncabezado["IdFacturaEncabezado"]));
                            foreach (CFacturaDetalle oFacturaDetalle in FacturaDetalle.LlenaObjetosFiltros(ParametrosFD, ConexionBaseDatos))
                            {
                                if (oFacturaDetalle.IdCotizacion != 0)
                                {
                                    CCotizacion CotizacionOportunidad = new CCotizacion();
                                    CotizacionOportunidad.LlenaObjeto(oFacturaDetalle.IdCotizacion, ConexionBaseDatos);
                                    COportunidad.ActualizarTotalesOportunidad(CotizacionOportunidad.IdOportunidad, ConexionBaseDatos);
                                    CotizacionOportunidad.IdEstatusCotizacion = 3;
                                    CotizacionOportunidad.Editar(ConexionBaseDatos);
                                }
                                else
                                {
                                    CProyecto ProyectoOportunidad = new CProyecto();
                                    ProyectoOportunidad.LlenaObjeto(oFacturaDetalle.IdProyecto, ConexionBaseDatos);
                                    COportunidad.ActualizarTotalesOportunidad(ProyectoOportunidad.IdOportunidad, ConexionBaseDatos);
                                }
                            }

                            oRespuesta.Add(new JProperty("Error", 0));
                            oRespuesta.Add(new JProperty("Descripcion", validacion));
                        }
                        else
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", validacion));
                        }
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                        oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar facturas de meses anteriores. Favor de cancelar por medio de nota de crédito."));
                    }
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta factura, ya que no esta timbrada"));
                }
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta factura, porque tiene movimientos de cobros asociados"));
            }

            CFacturaDetalle Detalle = new CFacturaDetalle();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdFacturaEncabezado", FacturaEncabezado.IdFacturaEncabezado);
            Parametros.Add("Baja", 0);

            foreach (CFacturaDetalle oDetalle in Detalle.LlenaObjetosFiltros(Parametros, ConexionBaseDatos))
            {
                // Actualiza Proyecto
                if (oDetalle.IdProyecto != 0)
                {
                    CProyecto.ActualizarTotales(oDetalle.IdProyecto, ConexionBaseDatos);
                }
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    private static string CancelarArchivoBuzonFiscal(int pIdFacturaEncabezado, CConexion pConexion)
    {
        string errores = "";
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CEmpresa Empresa = new CEmpresa();
        CTxtTimbradosFactura TxtTimbradosFacturaEncabezado = new CTxtTimbradosFactura();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CSerieFactura SerieFactura = new CSerieFactura();

        string NombreArchivo = "";

        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFacturaEncabezado), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

        Cliente.LlenaObjeto(FacturaEncabezado.IdCliente, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
        ParametrosTxt.Add("Folio", Convert.ToString(FacturaEncabezado.NumeroFactura));
        ParametrosTxt.Add("Serie", Convert.ToString(SerieFactura.SerieFactura));
        TxtTimbradosFacturaEncabezado.LlenaObjetoFiltros(ParametrosTxt, pConexion);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = "cancela_" + SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;

        if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
        {
            Encoding ANSI = Encoding.GetEncoding(1252);
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt", false, ANSI);
            file.WriteLine("cancela");
            file.WriteLine("########################################################################");
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("uuid|" + TxtTimbradosFacturaEncabezado.Uuid);
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("emRfc|" + Empresa.RFC);
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("reRfc|" + Organizacion.RFC);
            file.WriteLine("");
            file.WriteLine("#Opcional");
            file.WriteLine("refID|" + TxtTimbradosFacturaEncabezado.Refid);

            file.Close();

            errores = "1";
        }
        else
        {
            errores = "La ruta de la carpeta no es valida";
        }
        return errores;

    }

    private static string BuzonFiscalTimbradoCancelacion(int pIdFacturaEncabezado, string MotivoCancelacion, CConexion pConexion)
    {
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CSerieFactura SerieFactura = new CSerieFactura();
        CTxtTimbradosFactura TxtTimbradosFacturaEncabezado = new CTxtTimbradosFactura();
        string NombreArchivo = "";

        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFacturaEncabezado), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
        ParametrosTxt.Add("Folio", Convert.ToString(FacturaEncabezado.NumeroFactura));
        ParametrosTxt.Add("Serie", Convert.ToString(SerieFactura.SerieFactura));
        TxtTimbradosFacturaEncabezado.LlenaObjetoFiltros(ParametrosTxt, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = "cancela_" + SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;

        if (File.Exists(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt"))
        {

            StreamReader objReader = new StreamReader(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt");
            string sLine = "";
            ArrayList arrText = new ArrayList();
            string Campo = "";

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    string[] split = sLine.Split('|');
                    Campo = split[0];
                    switch (Campo)
                    {
                        case "fecha":
                            TxtTimbradosFacturaEncabezado.FechaCancelacion = Convert.ToString(split[1]);
                            string[] fechaFormateada = split[1].Split('T');
                            if (fechaFormateada[1].Length == 13)
                            {
                                fechaFormateada[1] = "0" + fechaFormateada[1];
                            }
                            FacturaEncabezado.FechaCancelacion = Convert.ToDateTime(fechaFormateada[0] + "T" + fechaFormateada[1]);
                            break;
                    }
                }
            }
            TxtTimbradosFacturaEncabezado.Editar(pConexion);
            FacturaEncabezado.Baja = true;
            FacturaEncabezado.IdEstatusFacturaEncabezado = 2;
            FacturaEncabezado.MotivoCancelacion = MotivoCancelacion;
            FacturaEncabezado.IdUsuarioCancelacion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            FacturaEncabezado.EditarFacturaEncabezado(pConexion);

            objReader.Close();

            NombreArchivo = "Factura cancelada correctamente";
        }
        else
        {
            NombreArchivo = "no existe el archivo";
        }

        return NombreArchivo;

    }

    [WebMethod]
    public static string EditarCliente(Dictionary<string, object> pCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        Cliente.LlenaObjeto(Convert.ToInt32(pCliente["IdCliente"]), ConexionBaseDatos);
        Cliente.IdCliente = Cliente.IdCliente;
        Cliente.FechaModificacion = Convert.ToDateTime(DateTime.Now);
        Cliente.IdUsuarioModifico = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);
        Organizacion.IdOrganizacion = Organizacion.IdOrganizacion;
        Organizacion.RazonSocial = Convert.ToString(pCliente["RazonSocial"]).ToUpper();
        Organizacion.NombreComercial = Convert.ToString(pCliente["NombreComercial"]).ToUpper();
        Organizacion.RFC = Convert.ToString(pCliente["RFC"]).ToUpper();
        Organizacion.FechaModificacion = Convert.ToDateTime(DateTime.Now);

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdTipoDireccion", 1);
        Parametros.Add("IdOrganizacion", Cliente.IdOrganizacion);
        DireccionOrganizacion.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
        DireccionOrganizacion.IdDireccionOrganizacion = DireccionOrganizacion.IdDireccionOrganizacion;
        DireccionOrganizacion.Calle = Convert.ToString(pCliente["Calle"]);
        DireccionOrganizacion.NumeroExterior = Convert.ToString(pCliente["NumeroExterior"]);
        DireccionOrganizacion.NumeroInterior = Convert.ToString(pCliente["NumeroInterior"]);
        DireccionOrganizacion.Colonia = Convert.ToString(pCliente["Colonia"]);
        DireccionOrganizacion.CodigoPostal = Convert.ToString(pCliente["CodigoPostal"]);
        DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pCliente["Conmutador"]);
        DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pCliente["IdMunicipio"]);
        DireccionOrganizacion.Referencia = Convert.ToString(pCliente["Referencia"].ToString());
        DireccionOrganizacion.IdTipoDireccion = 1;
        DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pCliente["IdLocalidad"]);

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        string validacion = "";
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Cliente.Editar(ConexionBaseDatos);
            Organizacion.Editar(ConexionBaseDatos);
            DireccionOrganizacion.Editar(ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Cliente.IdCliente;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico el Cliente.";
            HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);
            oRespuesta.Add("IdCliente", Cliente.IdCliente);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EliminarFacturaDetalle(Dictionary<string, object> pFacturaDetalle)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        int PuedeEliminarPartida = 0;

        CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
        FacturaDetalle.LlenaObjeto(Convert.ToInt32(pFacturaDetalle["pIdFacturaDetalle"]), ConexionBaseDatos);
        FacturaDetalle.IdFacturaDetalle = Convert.ToInt32(pFacturaDetalle["pIdFacturaDetalle"]);
        FacturaDetalle.Baja = true;

        PuedeEliminarPartida = FacturaDetalle.ValidaEliminarFacturaDetalle(Convert.ToInt32(pFacturaDetalle["pIdFacturaDetalle"]), ConexionBaseDatos);
        int existeIngresoAsociadoFactura = 0;
        existeIngresoAsociadoFactura = FacturaDetalle.ExisteIngresoAsociadoFactura(Convert.ToInt32(pFacturaDetalle["pIdFacturaDetalle"]), ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (PuedeEliminarPartida == 0 && existeIngresoAsociadoFactura == 0)
        {
            FacturaDetalle.EliminarFacturaDetalle(ConexionBaseDatos);

            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            CotizacionDetalle.LlenaObjeto(FacturaDetalle.IdCotizacionDetalle, ConexionBaseDatos);
            CotizacionDetalle.CantidadPendienteFacturar = Convert.ToInt32(FacturaDetalle.Cantidad);
            CotizacionDetalle.Editar(ConexionBaseDatos);

            CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
            FacturaEncabezado.LlenaObjeto(FacturaDetalle.IdFacturaEncabezado, ConexionBaseDatos);

            string TotalLetras = "";

            CUtilerias Utilerias = new CUtilerias();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            CFacturaEncabezado FacturaEncabezadoTotal = new CFacturaEncabezado();
            FacturaEncabezadoTotal.LlenaObjeto(Convert.ToInt32(FacturaDetalle.IdFacturaEncabezado), ConexionBaseDatos);

            string terminacion = "";
            if (FacturaEncabezadoTotal.IdTipoMoneda == 1)
            {
                terminacion = " M.N.";
            }
            else
            {
                terminacion = " USD";
            }

            TipoMoneda.LlenaObjeto(FacturaEncabezadoTotal.IdTipoMoneda, ConexionBaseDatos);
            TotalLetras = Utilerias.ConvertLetter(FacturaEncabezadoTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString()) + terminacion;
            FacturaEncabezadoTotal.TotalLetra = TotalLetras;
            FacturaEncabezadoTotal.Editar(ConexionBaseDatos);

            int noPartidasPendientes = FacturaEncabezado.ExistenPartidasPendientesCotizacion(FacturaDetalle.IdFacturaEncabezado, ConexionBaseDatos);
            CSerieFactura SerieFactura = new CSerieFactura();
            SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, ConexionBaseDatos);

            if (SerieFactura.Timbrado == false && noPartidasPendientes > 0)
            {
                FacturaEncabezado = new CFacturaEncabezado();
                FacturaEncabezado.ActualizarEstatusFacturadoCotizacion(FacturaDetalle.IdFacturaEncabezado, 3, ConexionBaseDatos);

                Dictionary<string, object> ParametrosFD = new Dictionary<string, object>();
                ParametrosFD.Add("IdFacturaEncabezado", FacturaDetalle.IdFacturaEncabezado);
                int partidas = 0;
                foreach (CFacturaDetalle oFacturaDetalle in FacturaDetalle.LlenaObjetosFiltros(ParametrosFD, ConexionBaseDatos))
                {
                    if (oFacturaDetalle.IdCotizacion != 0)
                    {
                        CCotizacion CotizacionOportunidad = new CCotizacion();
                        CotizacionOportunidad.LlenaObjeto(oFacturaDetalle.IdCotizacion, ConexionBaseDatos);
                        COportunidad.ActualizarTotalesOportunidad(CotizacionOportunidad.IdOportunidad, ConexionBaseDatos);
                    }
                    else
                    {
                        CProyecto ProyectoOportunidad = new CProyecto();
                        ProyectoOportunidad.LlenaObjeto(oFacturaDetalle.IdProyecto, ConexionBaseDatos);
                        COportunidad.ActualizarTotalesOportunidad(ProyectoOportunidad.IdOportunidad, ConexionBaseDatos);
                    }
                }
            }

            // Actualiza Proyecto
            if (FacturaDetalle.IdProyecto != 0)
            {
                CProyecto.ActualizarTotales(FacturaDetalle.IdProyecto, ConexionBaseDatos);
            }

            oRespuesta.Add("IdFacturaEncabezado", FacturaDetalle.IdFacturaEncabezado);
            oRespuesta.Add("SubtotalFactura", FacturaEncabezadoTotal.Subtotal);
            oRespuesta.Add("DescuentoFactura", FacturaEncabezadoTotal.Descuento);
            oRespuesta.Add("SubtotalDescuentoFactura", FacturaEncabezadoTotal.Subtotal - FacturaEncabezadoTotal.Descuento);
            oRespuesta.Add("IVAFactura", FacturaEncabezadoTotal.IVA);
            oRespuesta.Add("TotalFactura", FacturaEncabezadoTotal.Total);
            oRespuesta.Add("TotalLetraFactura", FacturaEncabezadoTotal.TotalLetra);

            oRespuesta.Add(new JProperty("Error", 0));
        }
        else
        {
            if (PuedeEliminarPartida > 0)
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede eliminar esta partida porque existen movimientos"));
            }
            if (existeIngresoAsociadoFactura > 0)
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede eliminar esta partida porque existen ingresos asociados a esta factura."));
            }
        }

        CFacturaDetalle DetalleActual = new CFacturaDetalle();
        Dictionary<string, object> FiltrosDetalle = new Dictionary<string, object>();
        FiltrosDetalle.Add("IdFacturaEncabezado", FacturaDetalle.IdFacturaEncabezado);
        FiltrosDetalle.Add("Baja", 0);

        int PartidasAgregadas = 0;
        foreach (CFacturaDetalle oDetalleActual in DetalleActual.LlenaObjetosFiltros(FiltrosDetalle, ConexionBaseDatos))
        {
            PartidasAgregadas++;
        }
        if (PartidasAgregadas == 0)
        {
            CCotizacion Pedido = new CCotizacion();
            Pedido.LlenaObjeto(FacturaDetalle.IdCotizacion, ConexionBaseDatos);
            Pedido.IdEstatusCotizacion = 3;
            Pedido.Editar(ConexionBaseDatos);
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EliminarFacturaEncabezadoSustituye(Dictionary<string, object> pFacturaEncabezadoSustituye)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        JObject oRespuesta = new JObject();
        CFacturaEncabezadoSustituye FacturaEncabezadoSustituye = new CFacturaEncabezadoSustituye();
        FacturaEncabezadoSustituye.LlenaObjeto(Convert.ToInt32(pFacturaEncabezadoSustituye["pIdFacturaEncabezadoSustituye"]), ConexionBaseDatos);
        FacturaEncabezadoSustituye.Baja = true;
        FacturaEncabezadoSustituye.Eliminar(ConexionBaseDatos);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string TimbrarFactura(Dictionary<string, object> pFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);

        int IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        CUsuario UsuarioSesion = new CUsuario();
        UsuarioSesion.LlenaObjeto(IdUsuario, ConexionBaseDatos);

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida" && UsuarioSesion.IdUsuario != 0)
        {

            ////AQUI VA A GENERAR EL FOLIO DE LA FACTURA/////////////////////
            CFacturaEncabezado FacturaEncabezadoFolio = new CFacturaEncabezado();
            FacturaEncabezadoFolio.LlenaObjeto(Convert.ToInt32(pFactura["IdFacturaEncabezado"]), ConexionBaseDatos);
            FacturaEncabezadoFolio.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            FacturaEncabezadoFolio.EditarFacturaEncabezadoFolio(ConexionBaseDatos);
            ////////////////////////////////////////////////////////////////

            JObject oRespuesta = new JObject();
            string validacion = CrearArchivoBuzonFiscal(Convert.ToInt32(pFactura["IdFacturaEncabezado"]), ConexionBaseDatos);
            if (validacion == "1")
            {
                CUtilerias Utilerias = new CUtilerias();
                Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                validacion = BuzonFiscalTimbrado(Convert.ToInt32(pFactura["IdFacturaEncabezado"]), ConexionBaseDatos);

                CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
                FacturaEncabezado.ActualizarEstatusFacturadoCotizacion(Convert.ToInt32(pFactura["IdFacturaEncabezado"]), 6, ConexionBaseDatos);

                CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
                Dictionary<string, object> ParametrosFD = new Dictionary<string, object>();
                ParametrosFD.Add("IdFacturaEncabezado", Convert.ToInt32(pFactura["IdFacturaEncabezado"]));
                foreach (CFacturaDetalle oFacturaDetalle in FacturaDetalle.LlenaObjetosFiltros(ParametrosFD, ConexionBaseDatos))
                {
                    if (oFacturaDetalle.IdCotizacion != 0)
                    {
                        CCotizacion CotizacionOportunidad = new CCotizacion();
                        CotizacionOportunidad.LlenaObjeto(oFacturaDetalle.IdCotizacion, ConexionBaseDatos);
                        COportunidad.ActualizarTotalesOportunidad(CotizacionOportunidad.IdOportunidad, ConexionBaseDatos);
                    }
                    else
                    {
                        CProyecto ProyectoOportunidad = new CProyecto();
                        ProyectoOportunidad.LlenaObjeto(oFacturaDetalle.IdProyecto, ConexionBaseDatos);
                        COportunidad.ActualizarTotalesOportunidad(ProyectoOportunidad.IdOportunidad, ConexionBaseDatos);
                    }
                }

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                if (HttpContext.Current.Session["IdUsuario"] == null)
                {
                    oRespuesta.Add(new JProperty("Descripcion", "Ha expirado la sesión actual."));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
                }
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    private static string CrearArchivoBuzonFiscal(int pIdFactura, CConexion pConexion)
    {
        string errores = "";
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CEmpresa Empresa = new CEmpresa();
        CLocalidad Localidad = new CLocalidad();
        CMunicipio Municipio = new CMunicipio();
        CEstado Estado = new CEstado();
        CPais Pais = new CPais();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CSerieFactura SerieFactura = new CSerieFactura();

        string NombreArchivo = "";

        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFactura), pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);
        Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
        Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        Cliente.LlenaObjeto(FacturaEncabezado.IdCliente, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        TipoMoneda.LlenaObjeto(FacturaEncabezado.IdTipoMoneda, pConexion);

        Dictionary<string, object> ParametrosDO = new Dictionary<string, object>();
        ParametrosDO.Add("IdOrganizacion", Convert.ToInt32(Organizacion.IdOrganizacion));
        ParametrosDO.Add("IdTipoDireccion", Convert.ToInt32(1));
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosDO, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;


        if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
        {
            Encoding ANSI = Encoding.GetEncoding(1252);
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt", false, ANSI);
            file.WriteLine("HOJA");
            file.WriteLine("########################################################################");
            file.WriteLine("[Datos Generales]");
            file.WriteLine("serie|" + SerieFactura.SerieFactura);
            file.WriteLine("folio|" + FacturaEncabezado.NumeroFactura);
            file.WriteLine("asignaFolio|FALSE");

            file.WriteLine("");

            file.WriteLine("[Datos del Emisor]");
            file.WriteLine("emRegimen|" + Convert.ToString(Empresa.RegimenFiscal));
            file.WriteLine("emRfc|" + Empresa.RFC);
            file.WriteLine("emNombre|" + Empresa.RazonSocial);
            file.WriteLine("emCalle|" + Empresa.Calle);
            file.WriteLine("emNoExterior|" + Empresa.NumeroExterior);
            file.WriteLine("emNoInterior|" + Empresa.NumeroInterior);
            file.WriteLine("emColonia|" + Empresa.Colonia);
            file.WriteLine("emLocalidad|" + Localidad.Localidad);
            file.WriteLine("emReferencia|" + Empresa.Referencia);
            file.WriteLine("emMunicipio|" + Municipio.Municipio);
            file.WriteLine("emEstado|" + Estado.Estado);
            file.WriteLine("emPais|" + Pais.Pais);
            file.WriteLine("emCodigoPostal|" + Empresa.CodigoPostal);
            file.WriteLine("emProveedor|");
            file.WriteLine("emGLN|");

            file.WriteLine("");

            file.WriteLine("[Datos de Expedicion]");

            if (Sucursal.DireccionFiscal == true)
            {
                file.WriteLine("exAlias|");
                file.WriteLine("exTelefono|");
                file.WriteLine("exCalle|");
                file.WriteLine("exNoExterior|");
                file.WriteLine("exNoInterior|");
                file.WriteLine("exColonia|");
                file.WriteLine("exLocalidad|");
                file.WriteLine("exReferencia|");
                file.WriteLine("exMunicipio|");
                file.WriteLine("exEstado|");
                file.WriteLine("exPais|");
                file.WriteLine("exCodigoPostal|");
            }
            else
            {
                Localidad.LlenaObjeto(Sucursal.IdLocalidad, pConexion);
                Municipio.LlenaObjeto(Sucursal.IdMunicipio, pConexion);
                Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
                Pais.LlenaObjeto(Estado.IdPais, pConexion);

                file.WriteLine("exAlias|" + Sucursal.Alias);
                file.WriteLine("exTelefono|" + Sucursal.Telefono);
                file.WriteLine("exCalle|" + Sucursal.Calle);
                file.WriteLine("exNoExterior|" + Sucursal.NumeroExterior);
                file.WriteLine("exNoInterior|" + Sucursal.NumeroInterior);
                file.WriteLine("exColonia|" + Sucursal.Colonia);
                file.WriteLine("exLocalidad|" + Localidad.Localidad);
                file.WriteLine("exReferencia|" + Sucursal.Referencia);
                file.WriteLine("exMunicipio|" + Municipio.Municipio);
                file.WriteLine("exEstado|" + Estado.Estado);
                file.WriteLine("exPais|" + Pais.Pais);
                file.WriteLine("exCodigoPostal|" + Sucursal.CodigoPostal);
            }

            file.WriteLine("");

            Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("[Datos del Receptor]");
            file.WriteLine("reRfc|" + FacturaEncabezado.RFC);
            file.WriteLine("reNombre|" + Organizacion.RazonSocial);
            file.WriteLine("reCalle|" + FacturaEncabezado.CalleFiscal);
            file.WriteLine("reNoExterior|" + FacturaEncabezado.NumeroExteriorFiscal);
            file.WriteLine("reNoInterior|" + FacturaEncabezado.NumeroInteriorFiscal);
            file.WriteLine("reColonia|" + FacturaEncabezado.ColoniaFiscal);
            file.WriteLine("reLocalidad|" + FacturaEncabezado.LocalidadFiscal);
            file.WriteLine("reReferencia|" + FacturaEncabezado.ReferenciaFiscal);
            file.WriteLine("reMunicipio|" + FacturaEncabezado.MunicipioFiscal);
            file.WriteLine("reEstado|" + FacturaEncabezado.EstadoFiscal);
            file.WriteLine("rePais|" + FacturaEncabezado.PaisFiscal);
            file.WriteLine("reCodigoPostal|" + FacturaEncabezado.CodigoPostalFiscal);
            file.WriteLine("reNoCliente|" + Cliente.IdCliente);
            file.WriteLine("reEmail|" + Cliente.Correo);
            file.WriteLine("reTelefono|");
            //file.WriteLine("reTelefono|" + DireccionOrganizacion.ConmutadorTelefono);
            file.WriteLine("reFax|");
            file.WriteLine("reComprador|");
            file.WriteLine("reNIM|");

            file.WriteLine("");

            Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("[Datos del Remitente]");

            file.WriteLine("remRfc|" + Empresa.RFC);
            file.WriteLine("remNombre|" + Empresa.RazonSocial);
            file.WriteLine("remClaveIdentificacion|");
            file.WriteLine("remCalle|" + Empresa.Calle);
            file.WriteLine("remNumero|" + Empresa.NumeroExterior);
            file.WriteLine("remReferencia|" + Empresa.Referencia);
            file.WriteLine("remColonia|" + Empresa.Colonia);
            file.WriteLine("remCiudad|" + Localidad.Localidad);
            file.WriteLine("remMunicipio|" + Municipio.Municipio);
            file.WriteLine("remEstado|" + Estado.Estado);
            file.WriteLine("remPais|" + Pais.Pais);
            file.WriteLine("remCodigoPostal|" + Empresa.CodigoPostal);

            file.WriteLine("");

            file.WriteLine("[Datos del Destino]");

            Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("desRfc|" + FacturaEncabezado.RFC);
            file.WriteLine("desNombre|" + Organizacion.RazonSocial);
            file.WriteLine("desClaveIdentificacion|");
            file.WriteLine("desCalle|" + DireccionOrganizacion.Calle);
            file.WriteLine("desNumero|" + DireccionOrganizacion.NumeroExterior);
            file.WriteLine("desReferencia|" + DireccionOrganizacion.Referencia);
            file.WriteLine("desColonia|" + DireccionOrganizacion.Colonia);
            file.WriteLine("desCiudad|" + Localidad.Localidad);
            file.WriteLine("desMunicipio|" + Municipio.Municipio);
            file.WriteLine("desEstado|" + Estado.Estado);
            file.WriteLine("desPais|" + Pais.Pais);
            file.WriteLine("desCodigoPostal|" + DireccionOrganizacion.CodigoPostal);

            file.WriteLine("");

            //aqui hace el ciclo de las partidas
            CFacturaDetalle ListaPartidas = new CFacturaDetalle();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();

            Dictionary<string, object> ParametrosOCD = new Dictionary<string, object>();
            ParametrosOCD.Add("Baja", 0);
            ParametrosOCD.Add("IdFacturaEncabezado", FacturaEncabezado.IdFacturaEncabezado);

            foreach (CFacturaDetalle oFacturaDetalle in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, pConexion))
            {
                if (oFacturaDetalle.IdProyecto == 0)
                {
                    if (oFacturaDetalle.IdProducto != 0)
                    {
                        Producto.LlenaObjeto(oFacturaDetalle.IdProducto, pConexion);
                        UnidadCompraVenta.LlenaObjeto(Producto.IdUnidadCompraVenta, pConexion);
                    }
                    else
                    {
                        Servicio.LlenaObjeto(oFacturaDetalle.IdServicio, pConexion);
                        UnidadCompraVenta.LlenaObjeto(Servicio.IdUnidadCompraVenta, pConexion);
                    }
                }
                else
                {
                    ConceptoProyecto.LlenaObjeto(oFacturaDetalle.IdConceptoProyecto, pConexion);
                    UnidadCompraVenta.LlenaObjeto(ConceptoProyecto.IdUnidadCompraVenta, pConexion);
                }
                file.WriteLine("[Datos de Conceptos]");
                file.WriteLine("cantidad|" + oFacturaDetalle.Cantidad);
                if (FacturaEncabezado.ParcialidadIndividual == true)
                {
                    file.WriteLine("unidad|" + "No aplica");
                }
                else
                {
                    file.WriteLine("unidad|" + ((UnidadCompraVenta.UnidadCompraVenta != null) ? UnidadCompraVenta.UnidadCompraVenta : "No aplica"));
                }
                file.WriteLine("numIdentificacion|" + oFacturaDetalle.Clave);
                string Descripcion = oFacturaDetalle.Descripcion.Replace('\n', ' ') + " " + oFacturaDetalle.DescripcionAgregada.Replace('\n', ' ');
                file.WriteLine("descripcion|" + Descripcion.Replace("|",""));
                file.WriteLine("valorUnitario|" + oFacturaDetalle.PrecioUnitario);
                file.WriteLine("importe|" + oFacturaDetalle.Total);
                file.WriteLine("#1  Cuenta Predial,");
                file.WriteLine("cpNumero|");
                file.WriteLine("#2  Informacion Aduanera");
                file.WriteLine("iaNumero|");
                file.WriteLine("iaFecha|");
                file.WriteLine("#3  Parte");
                file.WriteLine("parteCantidad|");
                file.WriteLine("parteUnidad|");
                file.WriteLine("parteNumIdentificacion|");
                file.WriteLine("parteDescripcion|");
                file.WriteLine("parteValorUnitario|");
                file.WriteLine("parteImporte|");
                file.WriteLine("#Bloque de Datos opcionales para");
                file.WriteLine("# introducir la informacion aduanera");
                file.WriteLine("parteIaNumero|");
                file.WriteLine("parteIaFecha|");
                file.WriteLine("parteIaAduana|");
                file.WriteLine("");
                file.WriteLine("#4 Complemento Concepto");
                file.WriteLine("");
                file.WriteLine("[Datos Complementarios para especificar la venta de vehiculos]");
                file.WriteLine("");
                file.WriteLine("claveVehicular|");
                file.WriteLine("");
                file.WriteLine("vehiculoIaNumero|");
                file.WriteLine("");
                file.WriteLine("vehiculoIaFecha|");
                file.WriteLine("");
                file.WriteLine("vehiculoIaAduana|");
                file.WriteLine("");
                file.WriteLine("#PARTES");
                file.WriteLine("vehiculoparteCantidad|");
                file.WriteLine("vehiculoparteUnidad|");
                file.WriteLine("vehiculopartenoIdentificacion|");
                file.WriteLine("vehiculoparteDescripcion|");
                file.WriteLine("vehiculoparteValorUnitario|");
                file.WriteLine("vehiculoparteImporte|");
                file.WriteLine("vehiculoparteIaNumero|");
                file.WriteLine("vehiculoparteIaFecha|");
                file.WriteLine("vehiculoparteIaAduana|");
                file.WriteLine("");
                file.WriteLine("[Complemento Dutty Free]");
                file.WriteLine("");
                file.WriteLine("dutFreeVersion|");
                file.WriteLine("dutFreeFechaTran|");
                file.WriteLine("dutFreeTipoTran|");
                file.WriteLine("# Datos Transito");
                file.WriteLine("dutFreeDatVia|");
                file.WriteLine("dutFreeDatTipoID|");
                file.WriteLine("dutFreeDatNumeroId|");
                file.WriteLine("dutFreeDatNacio|");
                file.WriteLine("dutFreeDatTransporte|");
                file.WriteLine("dutFreeDatidTransporte|");
                file.WriteLine("");
                file.WriteLine("[Datos Extra Conceptos]");
                file.WriteLine("ConExReferencia1|");
                file.WriteLine("ConExReferencia2|");
                file.WriteLine("ConExIndicador|");
                file.WriteLine("ConExDescripcionIngles|");
                file.WriteLine("ConExNumRemision|0");
                file.WriteLine("ConExCargo|");
                file.WriteLine("ConExDescuento|0");
                file.WriteLine("ConExMensaje|");
                file.WriteLine("ConExTasaImpuesto|");
                file.WriteLine("ConExImpuesto|");
                file.WriteLine("ConExValorUnitarioMonedaExtranjera|");
                file.WriteLine("ConExImporteMonedaExtranjera|");
                file.WriteLine("ConExtunitarioBruto|");
                file.WriteLine("ConExtImporteBruto|");
                file.WriteLine("ConExCvDivisas|");
                file.WriteLine("ConExtItemIdAlterno|");
                file.WriteLine("ConExtItemAlternoClave|");
                file.WriteLine("ConExtItemAlternoValor|");
                file.WriteLine("ConExtVendorPack|");
                file.WriteLine("");

            }

            ///////////////////////////////////////////////////////////////////////////

            file.WriteLine("[Datos Complementarios del Comprobante a nivel global]");
            file.WriteLine("subtotalConceptos|" + FacturaEncabezado.Subtotal);

            if (FacturaEncabezado.Descuento > 0)
            {
                CDescuentoCliente DescuentoCliente = new CDescuentoCliente();
                DescuentoCliente.LlenaObjeto(FacturaEncabezado.IdDescuentoCliente, pConexion);
                file.WriteLine("descuentoPorcentaje|" + FacturaEncabezado.PorcentajeDescuento);
                file.WriteLine("descuentoMonto|" + FacturaEncabezado.Descuento);
                file.WriteLine("descuentoMotivo|" + DescuentoCliente.Descripcion);
            }
            else
            {
                file.WriteLine("descuentoPorcentaje|");
                file.WriteLine("descuentoMonto|");
                file.WriteLine("descuentoMotivo|");
            }

            file.WriteLine("cargos|");
            file.WriteLine("totalConceptos|" + FacturaEncabezado.Subtotal);
            file.WriteLine("");

            if (FacturaEncabezado.Parcialidades == true || FacturaEncabezado.ParcialidadIndividual == true)
            {
                if (FacturaEncabezado.Parcialidades == true)
                {
                    file.WriteLine("pagoForma|Parcialidades (" + FacturaEncabezado.NumeroParcialidades + ")");
                }
                else
                {
                    file.WriteLine("pagoForma|Pago en una sola exhibición");
                }
            }
            else
            {
                file.WriteLine("pagoForma|Pago en una sola exhibición");
            }

            file.WriteLine("pagoCondiciones|" + FacturaEncabezado.CondicionPago);
            file.WriteLine("pagoMetodo|" + FacturaEncabezado.MetodoPago);
            file.WriteLine("numCtaPago|" + FacturaEncabezado.NumeroCuenta);
            file.WriteLine("lugarExpedicion|" + FacturaEncabezado.LugarExpedicion);

            if (FacturaEncabezado.ParcialidadIndividual == true)
            {
                file.WriteLine("serieFolioFiscalOrig|" + FacturaEncabezado.SerieGlobal);
                file.WriteLine("folioFiscalOrig|" + FacturaEncabezado.FolioGlobal);
                file.WriteLine("montoFolioFiscalOrig|" + FacturaEncabezado.MontoGlobal);
                file.WriteLine("fechaFolioFiscalOrig|" + FacturaEncabezado.FechaGlobal);
            }

            file.WriteLine("");

            file.WriteLine("[Datos Complementarios del Comprobante a nivel global para casos de importaci—n o exportaci—n de bienes]");
            file.WriteLine("#Datos Globales de Aduana, el bloque es opcional y se constituye por los siguientes tres datos, el bloque se repite para cada aduana que aplique.");
            file.WriteLine("comiaNumero|");
            file.WriteLine("comiaFecha|");
            file.WriteLine("comiaAduana|");
            file.WriteLine("embarque|");
            file.WriteLine("fob|");
            file.WriteLine("");
            file.WriteLine("[Datos Comerciales del Comprobante a nivel global]  Datos adicionales de tipo comercial comœnmente usados.");
            if (FacturaEncabezado.Refid != "" && FacturaEncabezado.IdFacturaEncabezado != 0)
            {
                file.WriteLine("refID|" + FacturaEncabezado.Refid);
            }
            else
            {
                file.WriteLine("refID|" + FacturaEncabezado.IdFacturaEncabezado);
                //file.WriteLine("refID|" + "1" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
            }
            file.WriteLine("tipoDocumento|Factura");

            Usuario.LlenaObjeto(FacturaEncabezado.IdUsuarioAgente, pConexion);

            file.WriteLine("ordenCompra|" + FacturaEncabezado.NumeroOrdenCompra);
            file.WriteLine("agente|" + Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
            file.WriteLine("observaciones|" + FacturaEncabezado.Nota);
            file.WriteLine("nombreMoneda|" + TipoMoneda.TipoMoneda);
            file.WriteLine("tipoCambio|" + FacturaEncabezado.TipoCambio);
            file.WriteLine("");
            file.WriteLine("[Impuestos Trasladados]");
            file.WriteLine("trasladadoImpuesto|IVA");
            file.WriteLine("trasladadoImporte|" + FacturaEncabezado.IVA);
            file.WriteLine("trasladadoTasa|" + Sucursal.IVAActual);
            file.WriteLine("subtotalTrasladados|" + FacturaEncabezado.IVA);
            file.WriteLine("");
            file.WriteLine("[Impuestos Retenidos]");
            file.WriteLine("retenidoImpuesto|");
            file.WriteLine("retenidoImporte|");
            file.WriteLine("subtotalRetenidos|");
            file.WriteLine("");
            file.WriteLine("[IMPUESTOS LOCALES]");
            file.WriteLine("version|");
            file.WriteLine("totalTraslados|");
            file.WriteLine("totalRetenciones|");
            file.WriteLine("");
            file.WriteLine("[TRASLADOS LOCALES]");
            file.WriteLine("impLocTrasladado|");
            file.WriteLine("tasaDeTraslado|");
            file.WriteLine("importeTraslados|");
            file.WriteLine("");
            file.WriteLine("[RETENCIONES LOCALES]");
            file.WriteLine("impLocRetenido|");
            file.WriteLine("tasaDeRetencion|");
            file.WriteLine("importeRetenciones|");
            file.WriteLine("");
            file.WriteLine("[Datos Totales]");
            file.WriteLine("montoTotal|" + FacturaEncabezado.Total);
            file.WriteLine("montoTotalTexto|" + FacturaEncabezado.TotalLetra);
            file.WriteLine("");

            file.WriteLine("[Otros]");
            file.WriteLine("ClaveTransportista|");
            file.WriteLine("NoRelacionPemex|");
            file.WriteLine("NoConvenioPemex|");
            file.WriteLine("NoCedulaPemex|");
            file.WriteLine("AireacionYSecado|");
            file.WriteLine("ApoyoEducampo|");
            file.WriteLine("Sanidad|");
            file.WriteLine("");
            file.WriteLine("[Otros]");
            file.WriteLine("LeyendaEspecial1|");
            file.WriteLine("LeyendaEspecial2|");
            file.WriteLine("LeyendaEspecial3|");
            file.Close();

            errores = "1";

        }
        else
        {
            errores = "La ruta de la carpeta no es valida";
        }
        return errores;
    }

    private static string BuzonFiscalTimbrado(int pIdFactura, CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CSerieFactura SerieFactura = new CSerieFactura();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();

        string NombreArchivo = "";
        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFactura), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);
        NombreArchivo = SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;

        if (File.Exists(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt"))
        {

            StreamReader objReader = new StreamReader(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt");
            string sLine = "";
            ArrayList arrText = new ArrayList();
            string Campo = "";


            CTxtTimbradosFactura TxtTimbradosFactura = new CTxtTimbradosFactura();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    string[] split = sLine.Split('|');
                    Campo = split[0];
                    switch (Campo)
                    {
                        case "refId":
                            TxtTimbradosFactura.Refid = Convert.ToString(split[1]);
                            break;
                        case "noCertificadoSAT":
                            TxtTimbradosFactura.NoCertificadoSAT = Convert.ToString(split[1]);
                            break;
                        case "fechaTimbrado":
                            TxtTimbradosFactura.FechaTimbrado = Convert.ToString(split[1]);
                            break;
                        case "uuid":
                            TxtTimbradosFactura.Uuid = Convert.ToString(split[1]);
                            break;
                        case "noCertificado":
                            TxtTimbradosFactura.NoCertificado = Convert.ToString(split[1]);
                            break;
                        case "selloSAT":
                            TxtTimbradosFactura.SelloSAT = Convert.ToString(split[1]);
                            break;
                        case "sello":
                            TxtTimbradosFactura.Sello = Convert.ToString(split[1]);
                            break;
                        case "fecha":
                            TxtTimbradosFactura.Fecha = Convert.ToString(split[1]);
                            break;
                        case "folio":
                            TxtTimbradosFactura.Folio = Convert.ToString(split[1]);
                            break;
                        case "serie":
                            TxtTimbradosFactura.Serie = Convert.ToString(split[1]);
                            break;
                        case "leyendaImpresion":
                            TxtTimbradosFactura.LeyendaImpresion = Convert.ToString(split[1]);
                            break;
                        case "cadenaOriginal":
                            TxtTimbradosFactura.CadenaOriginal = Convert.ToString(split[1]);
                            break;
                        case "totalConLetra":
                            TxtTimbradosFactura.TotalConLetra = Convert.ToString(split[1]);
                            break;
                    }
                }
            }

			CTxtTimbradosFactura ValidarTimbrado = new CTxtTimbradosFactura();
			Dictionary<string, object> pParametros = new Dictionary<string, object>();
			pParametros.Add("Serie", TxtTimbradosFactura.Serie);
			pParametros.Add("Folio", TxtTimbradosFactura.Folio);
			
			if (ValidarTimbrado.LlenaObjetosFiltros(pParametros, pConexion).Count == 0) {
				TxtTimbradosFactura.Agregar(pConexion);
			}
            FacturaEncabezado.Refid = TxtTimbradosFactura.Refid;
            FacturaEncabezado.Editar(pConexion);
            objReader.Close();

            NombreArchivo = "Factura timbrada correctamente";
        }
        else
        {
            NombreArchivo = "no existe el archivo";
        }

        return NombreArchivo;
    }

    [WebMethod]
    public static string ObtenerListaEstados(int pIdPais)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonEstados(pIdPais, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaMunicipios(int pIdEstado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonMunicipios(pIdEstado, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaLocalidades(int pIdMunicipio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonLocalidades(pIdMunicipio, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerFormaFiltrofacturaCliente()
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
    public static string ObtenerFormaAgregarFacturaPedidoCliente()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CEmpresa Empresa = new CEmpresa();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);
        CTipoCambio TipoCambio = new CTipoCambio();
        DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        string validacion = ValidarExisteTipoCambio(TipoCambio, Sucursal, Fecha, ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                CMunicipio Municipio = new CMunicipio();
                CEstado Estado = new CEstado();
                CPais Pais = new CPais();

                if (Sucursal.DireccionFiscal == true)
                {
                    Municipio.LlenaObjeto(Empresa.IdMunicipio, ConexionBaseDatos);
                }
                else
                {
                    Municipio.LlenaObjeto(Sucursal.IdMunicipio, ConexionBaseDatos);
                }

                Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
                Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);

                Modelo.Add("LugarExpedicion", Estado.Estado + ", " + Pais.Pais);
                Modelo.Add("FechaActual", DateTime.Now.ToShortDateString());
                Modelo.Add("SeriesFactura", CJson.ObtenerJsonSerieFactura(Usuario.IdSucursalActual, ConexionBaseDatos));
                Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
                Modelo.Add("Agentes", CUsuario.ObtenerJsonUsuarioAgenteTodos(Usuario.IdUsuario, ConexionBaseDatos));
                Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos));
                Modelo.Add("CondicionesPago", CCondicionPago.ObtenerJsonCondicionesPago(ConexionBaseDatos));
                Modelo.Add("MetodosPago", CMetodoPago.ObtenerMetodoPagoIngresos(ConexionBaseDatos));
                Modelo.Add("RegimenFiscal", Empresa.RegimenFiscal);
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

    private static string ValidarExisteTipoCambio(CTipoCambio TipoCambio, CSucursal Sucursal, DateTime Fecha, CConexion pConexion)
    {
        string errores = "";
        bool ExisteTipoCambio = false;

        ExisteTipoCambio = TipoCambio.ExisteTipoCambioOrigen(Sucursal.IdTipoMoneda, Fecha, pConexion);
        if (ExisteTipoCambio == false)
        {
            errores = errores + "<span>*</span> No existe tipo cambio para hoy.<br />";
        }
        return errores;
    }

    [WebMethod]
    public static string ObtenerFormaConsultarFacturaEncabezado(int pIdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarFacturaEncabezado = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CContactoOrganizacion oContacto = new CContactoOrganizacion();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        if (Usuario.TienePermisos(new string[] { "puedeEditarFacturaEncabezado" }, ConexionBaseDatos) == "")
        {
            puedeEditarFacturaEncabezado = 1;
        }
        oPermisos.Add("puedeEditarFacturaEncabezado", puedeEditarFacturaEncabezado);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CFacturaEncabezado.ObtenerFacturaEncabezado(Modelo, pIdFacturaEncabezado, ConexionBaseDatos);

            FacturaEncabezado.LlenaObjeto(pIdFacturaEncabezado, ConexionBaseDatos);
            oContacto.LlenaObjeto(FacturaEncabezado.IdContactoCliente, ConexionBaseDatos);

            Modelo.Add(new JProperty("Permisos", oPermisos));
            Modelo.Add(new JProperty("ContactoOrganizacion", oContacto.Nombre));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            if (HttpContext.Current.Session["IdUsuario"] == null)
            {
                oRespuesta.Add(new JProperty("Descripcion", "Ha expirado la sesión actual."));
            }
            else
            {
                oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
            }
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarDetallePartida(int pIdFacturaDetalle)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
            FacturaDetalle.LlenaObjeto(pIdFacturaDetalle, ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("Descripcion", FacturaDetalle.Descripcion);
            Modelo.Add("IdFacturaDetalle", pIdFacturaDetalle);
            Modelo.Add("DescripcionAgregada", FacturaDetalle.DescripcionAgregada);
            
            oRespuesta.Add("Error", 0);
            oRespuesta.Add("Modelo", Modelo);
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarFacturaEncabezado(int IdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarFacturaEncabezado = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarFacturaEncabezado" }, ConexionBaseDatos) == "")
        {
            puedeEditarFacturaEncabezado = 1;
        }
        oPermisos.Add("puedeEditarFacturaEncabezado", puedeEditarFacturaEncabezado);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CFacturaEncabezado.ObtenerFacturaEncabezado(Modelo, IdFacturaEncabezado, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("CondicionesPago", CJson.ObtenerJsonCondicionPago(Convert.ToInt32(Modelo["IdCondicionPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("MetodosPago", CJson.ObtenerJsonMetodoPago(Convert.ToInt32(Modelo["IdMetodoPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("NumerosCuenta", CJson.ObtenerJsonNumeroCuentaCliente(Convert.ToInt32(Modelo["IdNumeroCuenta"].ToString()), Convert.ToInt32(Modelo["IdCliente"].ToString()), ConexionBaseDatos));
            Modelo.Add("DireccionesOrganizacion", CJson.ObtenerJsonDireccionOrganizacion(Convert.ToInt32(Modelo["IdOrganizacion"].ToString()), ConexionBaseDatos));
            Modelo.Add("SeriesFactura", CJson.ObtenerJsonSerieFactura(Convert.ToInt32(Usuario.IdSucursalActual), Convert.ToInt32(Modelo["IdSerieFactura"].ToString()), ConexionBaseDatos));
            Modelo.Add("Agentes", CUsuario.ObtenerJsonUsuarioAgente(Usuario.IdUsuario, Convert.ToInt32(Modelo["IdUsuarioAgente"].ToString()), ConexionBaseDatos));
            Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(Convert.ToInt32(Modelo["IdDivision"].ToString()), ConexionBaseDatos));
            //Modelo.Add("Pedidos", CCotizacion.ObtenerPedidosClienteFacturaConDocumentacion(Convert.ToInt32(Modelo["IdCliente"].ToString()), ConexionBaseDatos));
            Modelo.Add("Descuentos", CDescuentoCliente.ObtenerJsonDescuentosCliente(Convert.ToInt32(Modelo["IdCliente"].ToString()), Convert.ToInt32(Modelo["IdDescuentoCliente"].ToString()), ConexionBaseDatos));

            CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
            FacturaEncabezado.LlenaObjeto(IdFacturaEncabezado, ConexionBaseDatos);
            ContactoOrganizacion.LlenaObjeto(FacturaEncabezado.IdContactoCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("ListaContactosOrganizacion", CContactoOrganizacion.ObtenerJsonContactoOrganizacion(ContactoOrganizacion.IdOrganizacion, FacturaEncabezado.IdContactoCliente, ConexionBaseDatos)));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaDatosFiscalesCliente(int IdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(IdCliente, ConexionBaseDatos);
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Modelo.Add("IdSucursalActual", Usuario.IdSucursalActual);
            Modelo = CJson.ObtenerJsonCliente(Modelo, IdCliente, ConexionBaseDatos);
            Modelo.Add("TipoIndustrias", CJson.ObtenerJsonTipoIndustria(Convert.ToInt32(Modelo["IdTipoIndustria"].ToString()), ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(Convert.ToInt32(Modelo["IdCondicionPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("FormaContactos", CJson.ObtenerJsonFormaContactos(Convert.ToInt32(Modelo["IdFormaContacto"].ToString()), ConexionBaseDatos));
            Modelo.Add("UsuarioAgentes", CUsuario.ObtenerJsonUsuarioAgente(Usuario.IdUsuario, Cliente.IdUsuarioAgente, ConexionBaseDatos));

            Modelo.Add("TipoClientes", CJson.ObtenerJsonTipoClientes(Convert.ToInt32(Modelo["IdTipoCliente"].ToString()), ConexionBaseDatos));
            Modelo.Add("GrupoEmpresariales", CJson.ObtenerJsonGrupoEmpresariales(Convert.ToInt32(Modelo["IdGrupoEmpresarial"].ToString()), ConexionBaseDatos));
            Modelo.Add("Localidades", CLocalidad.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CMunicipio.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CEstado.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CPais.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));

            COrganizacion Organizacion = new COrganizacion();
            Cliente.LlenaObjeto(IdCliente, ConexionBaseDatos);
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

            Modelo.Add("SegmentoMercados", CSegmentoMercado.ObtenerJsonSegmentoMercado(Organizacion.IdSegmentoMercado, ConexionBaseDatos));
            Modelo.Add("TipoGarantias", CTipoGarantia.ObtenerJsonTipoGarantia(Convert.ToInt32(Modelo["IdTipoGarantia"].ToString()), ConexionBaseDatos));

            Dictionary<string, object> ParametrosSucursalAlta = new Dictionary<string, object>();
            ParametrosSucursalAlta.Add("IdUsuario", Organizacion.IdUsuarioAlta);
            Usuario.LlenaObjetoFiltros(ParametrosSucursalAlta, ConexionBaseDatos);

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTipoCambio(Dictionary<string, object> pTipoCambio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCambio TipoCambio = new CTipoCambio();

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Opcion", 1);
            ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(pTipoCambio["IdTipoCambioOrigen"]));
            ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(pTipoCambio["IdTipoCambioDestino"]));
            ParametrosTS.Add("Fecha", DateTime.Today);
            TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("TipoCambioActual", TipoCambio.TipoCambio);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFechaPago(Dictionary<string, object> pCondicionPago)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
            DateTime FechaPago;
            FechaPago = new DateTime(1, 1, 1);

            FechaPago = DetalleFacturaProveedor.ObtieneFechaPago(Convert.ToInt32(pCondicionPago["IdCondicionPago"]), Convert.ToDateTime(pCondicionPago["FechaFactura"]), ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("FechaPago", FechaPago.ToShortDateString());
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFormaDatosCliente(int pIdCliente, int IdTipoMonedaFactura, int PorFiltroTipoMoneda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(pIdCliente, ConexionBaseDatos);
            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

            Modelo.Add("IdCliente", pIdCliente);
            Modelo.Add("IdUsuarioAgente", Cliente.IdUsuarioAgente);
            Modelo.Add("RFC", Organizacion.RFC);
            Modelo.Add("RazonSocial", Organizacion.RazonSocial);
            Modelo.Add("NombreComercial", Organizacion.NombreComercial);

            #region DireccionesFiscales
            CSelectEspecifico ObtenerDireccionesFiscales = new CSelectEspecifico();
            ObtenerDireccionesFiscales.StoredProcedure.CommandText = "sp_DireccionOrganizacion_Consultar_ObtenerDireccionesFiscales";
            ObtenerDireccionesFiscales.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", Organizacion.IdOrganizacion);
            ObtenerDireccionesFiscales.Llena(ConexionBaseDatos);

            JArray JADireccionesFiscales = new JArray();
            while (ObtenerDireccionesFiscales.Registros.Read())
            {
                JObject DireccionFiscal = new JObject();
                DireccionFiscal.Add("Valor", Convert.ToInt32(ObtenerDireccionesFiscales.Registros["IdDireccionOrganizacion"]));
                DireccionFiscal.Add("Descripcion", Convert.ToString(ObtenerDireccionesFiscales.Registros["Descripcion"]));
                JADireccionesFiscales.Add(DireccionFiscal);
            }
            ObtenerDireccionesFiscales.Registros.Close();
            JObject JComboDireccionFiscal = new JObject();
            JComboDireccionFiscal.Add("DescripcionDefault", "Seleccionar...");
            JComboDireccionFiscal.Add("ValorDefault", "0");
            JComboDireccionFiscal.Add("Opciones", JADireccionesFiscales);
            Modelo.Add("DireccionesFiscales", JComboDireccionFiscal);
            #endregion

            #region DireccionesNoFiscales
            CSelectEspecifico ObtenerDireccionesNoFiscales = new CSelectEspecifico();
            ObtenerDireccionesNoFiscales.StoredProcedure.CommandText = "sp_DireccionOrganizacion_Consultar_ObtenerDireccionesNoFiscales";
            ObtenerDireccionesNoFiscales.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", Organizacion.IdOrganizacion);
            ObtenerDireccionesNoFiscales.Llena(ConexionBaseDatos);

            JArray JADireccionesEntrega = new JArray();
            while (ObtenerDireccionesNoFiscales.Registros.Read())
            {
                JObject DireccionEntrega = new JObject();
                DireccionEntrega.Add("Valor", Convert.ToInt32(ObtenerDireccionesNoFiscales.Registros["IdDireccionOrganizacion"]));
                DireccionEntrega.Add("Descripcion", Convert.ToString(ObtenerDireccionesNoFiscales.Registros["Calle"]));
                JADireccionesEntrega.Add(DireccionEntrega);
            }
            ObtenerDireccionesNoFiscales.Registros.Close();
            JObject JComboDireccionEntrega = new JObject();
            JComboDireccionEntrega.Add("DescripcionDefault", "Seleccionar...");
            JComboDireccionEntrega.Add("ValorDefault", "0");
            JComboDireccionEntrega.Add("Opciones", JADireccionesEntrega);
            Modelo.Add("DireccionesEntrega", JComboDireccionEntrega);
            #endregion

            JObject JComboDescuentoCliente = new JObject();
            JComboDescuentoCliente.Add("DescripcionDefault", "Seleccionar...");
            JComboDescuentoCliente.Add("ValorDefault", "0");
            JComboDescuentoCliente.Add("Opciones", CDescuentoCliente.ObtenerJsonDescuentosCliente(pIdCliente, ConexionBaseDatos));
            Modelo.Add("DescuentosCliente", JComboDescuentoCliente);

            JObject JComboPedidos = new JObject();
            JComboPedidos.Add("DescripcionDefault", "Seleccionar...");
            JComboPedidos.Add("ValorDefault", "0");
            JComboPedidos.Add("Opciones", CCotizacion.ObtenerPedidosClienteFacturaConDocumentacion(pIdCliente, IdTipoMonedaFactura, PorFiltroTipoMoneda, ConexionBaseDatos));
            Modelo.Add("Pedidos", JComboPedidos);


            Modelo.Add(new JProperty("Permisos", oPermisos));
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
    public static string LlenaMotivoCancelacionFactura(int IdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEliminarFacturaEncabezado = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (Usuario.TienePermisos(new string[] { "puedeEliminarFacturaEncabezado" }, ConexionBaseDatos) == "")
        {
            puedeEliminarFacturaEncabezado = 1;
        }
        oPermisos.Add("puedeEliminarFacturaEncabezado", puedeEliminarFacturaEncabezado);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CFacturaEncabezado.ObtenerFacturaEncabezado(Modelo, IdFacturaEncabezado, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string LlenaFacturasSustituye(int IdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CFacturaEncabezado.ObtenerFacturaEncabezado(Modelo, IdFacturaEncabezado, ConexionBaseDatos);
            Modelo.Add("SeriesFactura", CJson.ObtenerJsonSerieFactura(Usuario.IdSucursalActual, ConexionBaseDatos));
            Modelo.Add(new JProperty("Permisos", oPermisos));
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
    public static string ObtieneFacturaFormato(int IdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CSerieFactura SerieFactura = new CSerieFactura();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        string NombreArchivo = "";
        string Ruta = "";

        FacturaEncabezado.LlenaObjeto(IdFacturaEncabezado, ConexionBaseDatos);
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

        NombreArchivo = SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;
        Ruta = RutaCFDI.RutaCFDI + "/out/" + NombreArchivo + ".pdf";

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("Ruta", Ruta));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneFacturaAddenda(int IdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CSerieFactura SerieFactura = new CSerieFactura();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        string NombreArchivo = "";
        string Ruta = "";

        FacturaEncabezado.LlenaObjeto(IdFacturaEncabezado, ConexionBaseDatos);
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

        NombreArchivo = SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;
        Ruta = RutaCFDI.RutaCFDI + "/out/" + NombreArchivo + ".pdf";

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("Ruta", Ruta));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneFacturaXML(int IdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject Respuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            JObject oRespuesta = new JObject();
            JObject oPermisos = new JObject();
            CUsuario Usuario = new CUsuario();
            CSucursal Sucursal = new CSucursal();
            CRutaCFDI RutaCFDI = new CRutaCFDI();
            CSerieFactura SerieFactura = new CSerieFactura();
            CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
            string NombreArchivo = "";
            string Ruta = "";

            FacturaEncabezado.LlenaObjeto(IdFacturaEncabezado, ConexionBaseDatos);
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
            SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, ConexionBaseDatos);

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
            ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
            RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

            NombreArchivo = SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;
            Ruta = RutaCFDI.RutaCFDI + "/out/" + NombreArchivo + ".xml";

            WebClient ajax = new WebClient();
            ajax.Encoding = Encoding.UTF8;

            string xml = ajax.DownloadString(Ruta);

            byte[] archivo = new byte[xml.Length * sizeof(char)];
            System.Buffer.BlockCopy(xml.ToCharArray(), 0, archivo, 0, archivo.Length);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/xml";
            HttpContext.Current.Response.AddHeader("Content-disposition", "attachment;filename='"+ NombreArchivo +".xml'");
            HttpContext.Current.Response.BinaryWrite(archivo);

        }
        else
        {
            Respuesta.Add("Error", 1);
            Respuesta.Add("Descripcion", respuesta);
        }

        return Respuesta.ToString();

    }

    [WebMethod]
    public static string obtenerPedidosClienteSinDocumentacion(int IdCliente, int IdTipoMonedaFactura, int PorFiltroTipoMoneda, int NuevoCotizador)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
		if (respuesta == "Conexion Establecida")
		{
			JObject Modelo = new JObject();
			JArray Opciones = new JArray();
			if (NuevoCotizador == 0) {
				Opciones = CCotizacion.ObtenerPedidosClienteFacturaSinDocumentacion(Convert.ToInt32(IdCliente), IdTipoMonedaFactura, PorFiltroTipoMoneda, ConexionBaseDatos);
			}
			else
			{
				Opciones = ObtenerPedidosNuevoCotizador(IdCliente, ConexionBaseDatos);
			}
			Modelo.Add("Opciones", Opciones);
			
			Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir un pedido...");
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexión a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string obtenerPedidosClienteConDocumentacion(int IdCliente, int IdTipoMonedaFactura, int PorFiltroTipoMoneda, int NuevoCotizador)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

			JArray Opciones = new JArray();
			if (NuevoCotizador == 0)
			{
				Opciones = CCotizacion.ObtenerPedidosClienteFacturaConDocumentacion(Convert.ToInt32(IdCliente), IdTipoMonedaFactura, PorFiltroTipoMoneda, ConexionBaseDatos);
			}
			else
			{
				Opciones = ObtenerPedidosNuevoCotizador(IdCliente, ConexionBaseDatos);
			}
			Modelo.Add("Opciones", Opciones);
			Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir un pedido...");
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexión a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

	public static JArray ObtenerPedidosNuevoCotizador(int IdCliente, CConexion pConexion)
	{
		JArray Opciones = new JArray();

		CSelectEspecifico Consulta = new CSelectEspecifico();
		Consulta.StoredProcedure.CommandText = "sp_Cotizador_ListarPedidos";
		Consulta.StoredProcedure.Parameters.Add("IdCliente", SqlDbType.Int).Value = IdCliente;

		Consulta.Llena(pConexion);

		while (Consulta.Registros.Read())
		{
			JObject Opcion = new JObject();

			Opcion.Add("Valor", Convert.ToInt32(Consulta.Registros["IdPresupuesto"]));
			Opcion.Add("Descripcion", Convert.ToString(Consulta.Registros["Folio"]));
			Opciones.Add(Opcion);
		}

		Consulta.CerrarConsulta();

		return Opciones;
	}

    [WebMethod]
    public static string ObtenerNumerosCuenta(int pIdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCuentaBancariaCliente CuentaBancariaCliente = new CCuentaBancariaCliente();
            Dictionary<string, object> ParametrosNumeroCuenta = new Dictionary<string, object>();
            ParametrosNumeroCuenta.Add("IdCliente", Convert.ToInt32(pIdCliente));
            ParametrosNumeroCuenta.Add("Baja", false);
            JArray JANumerosCuenta = new JArray();
            foreach (CCuentaBancariaCliente oNumeroCuenta in CuentaBancariaCliente.LlenaObjetosFiltros(ParametrosNumeroCuenta, ConexionBaseDatos))
            {
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                TipoMoneda.LlenaObjeto(oNumeroCuenta.IdTipoMoneda, ConexionBaseDatos);
                JObject JNumeroCuenta = new JObject();
                JNumeroCuenta.Add("Valor", oNumeroCuenta.IdCuentaBancariaCliente);
                JNumeroCuenta.Add("Descripcion", oNumeroCuenta.CuentaBancariaCliente + " (" + TipoMoneda.TipoMoneda + ")");
                JANumerosCuenta.Add(JNumeroCuenta);
            }
            Modelo.Add("DescripcionDefault", "No identificado");
            Modelo.Add("Valor", "0");
            Modelo.Add("Opciones", JANumerosCuenta);

            Modelo.Add(new JProperty("Permisos", oPermisos));
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
    public static string ObtenerNumeroFactura(Dictionary<string, object> pFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSerieFactura SerieFactura = new CSerieFactura();
            SerieFactura.LlenaObjeto(Convert.ToInt32(pFactura["IdSerieFactura"]), ConexionBaseDatos);
            CFacturaEncabezado Factura = new CFacturaEncabezado();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            int NumeroFactura = 0;
            NumeroFactura = Factura.ObtieneNumeroFactura(SerieFactura.SerieFactura, Usuario.IdSucursalActual, ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("NumeroFactura", NumeroFactura);
                if (SerieFactura.Timbrado == true)
                {
                    Modelo.Add("SerieTimbrado", 1);
                }
                else
                {
                    Modelo.Add("SerieTimbrado", 0);
                }
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerTipoCambioPedido(Dictionary<string, object> pFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CCotizacion Cotizacion = new CCotizacion();
            Cotizacion.LlenaObjeto(Convert.ToInt32(pFactura["IdPedido"]), ConexionBaseDatos);
            JObject Modelo = new JObject();

            if (Cotizacion.IdTipoMoneda == 2)
            {
                //OBTIENE EL TIPO DE CAMBIO EN DOLARES QUE SE GUARDO EN EL MOMENTO DE GENERAR LA NOTA DE CRÉDITO
                CTipoCambioCotizacion TipoCambioCotizacion = new CTipoCambioCotizacion();
                Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
                ParametrosTS.Add("Opcion", 1);
                ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
                ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
                ParametrosTS.Add("IdCotizacion", Convert.ToInt32(pFactura["IdPedido"]));
                TipoCambioCotizacion.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);
                Modelo.Add("TipoCambio", TipoCambioCotizacion.TipoCambio);
                Modelo.Add("IdTipoMonedaPedido", Cotizacion.IdTipoMoneda);
            }
            else
            {
                Modelo.Add("TipoCambio", 1);
                Modelo.Add("IdTipoMonedaPedido", 1);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////
            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerPorcentaje(int pIdDescuentoCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CDescuentoCliente DescuentoCliente = new CDescuentoCliente();
            DescuentoCliente.LlenaObjeto(pIdDescuentoCliente, ConexionBaseDatos);
            Modelo.Add("Porcentaje", DescuentoCliente.DescuentoCliente.ToString());

            Modelo.Add(new JProperty("Permisos", oPermisos));
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
    public static string ObtenerDireccionCliente(int pIdDireccionCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CDireccionOrganizacion DireccionCliente = new CDireccionOrganizacion();
            DireccionCliente.LlenaObjeto(pIdDireccionCliente, ConexionBaseDatos);
            Modelo.Add("IdDireccionCliente", pIdDireccionCliente);
            Modelo.Add("Calle", DireccionCliente.Calle);
            Modelo.Add("NumeroExterior", DireccionCliente.NumeroExterior);
            Modelo.Add("NumeroInterior", DireccionCliente.NumeroInterior);
            Modelo.Add("Colonia", DireccionCliente.Colonia);
            Modelo.Add("CodigoPostal", DireccionCliente.CodigoPostal);
            Modelo.Add("ConmutadorTelefono", DireccionCliente.ConmutadorTelefono);
            Modelo.Add("Referencia", DireccionCliente.Referencia);

            CLocalidad Localidad = new CLocalidad();
            Localidad.LlenaObjeto(DireccionCliente.IdLocalidad, ConexionBaseDatos);
            Modelo.Add("Localidad", Localidad.Localidad);

            CMunicipio Municipio = new CMunicipio();
            Municipio.LlenaObjeto(DireccionCliente.IdMunicipio, ConexionBaseDatos);
            Modelo.Add("Municipio", Municipio.Municipio);

            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
            Modelo.Add("Estado", Estado.Estado);

            CPais Pais = new CPais();
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
            Modelo.Add("Pais", Pais.Pais);

            Modelo.Add(new JProperty("Permisos", oPermisos));
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
    public static string ObtenerDireccionFiscal(int pIdDireccionFiscal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CDireccionOrganizacion DireccionCliente = new CDireccionOrganizacion();
            DireccionCliente.LlenaObjeto(pIdDireccionFiscal, ConexionBaseDatos);
            Modelo.Add("IdDireccionFiscal", pIdDireccionFiscal);
            Modelo.Add("Calle", DireccionCliente.Calle);
            Modelo.Add("NumeroExterior", DireccionCliente.NumeroExterior);
            Modelo.Add("NumeroInterior", DireccionCliente.NumeroInterior);
            Modelo.Add("Colonia", DireccionCliente.Colonia);
            Modelo.Add("CodigoPostal", DireccionCliente.CodigoPostal);
            Modelo.Add("ConmutadorTelefono", DireccionCliente.ConmutadorTelefono);
            Modelo.Add("Referencia", DireccionCliente.Referencia);

            CLocalidad Localidad = new CLocalidad();
            Localidad.LlenaObjeto(DireccionCliente.IdLocalidad, ConexionBaseDatos);
            Modelo.Add("Localidad", Localidad.Localidad);

            CMunicipio Municipio = new CMunicipio();
            Municipio.LlenaObjeto(DireccionCliente.IdMunicipio, ConexionBaseDatos);
            Modelo.Add("Municipio", Municipio.Municipio);

            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
            Modelo.Add("Estado", Estado.Estado);

            CPais Pais = new CPais();
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
            Modelo.Add("Pais", Pais.Pais);

            Modelo.Add(new JProperty("Permisos", oPermisos));
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

    public void PintaGridDetallePedido()
    {
        //GridPedidoDetalle
        CJQGrid GridPedidoDetalle = new CJQGrid();
        GridPedidoDetalle.NombreTabla = "grdPedidoDetalle";
        GridPedidoDetalle.CampoIdentificador = "IdCotizacionDetalle";
        GridPedidoDetalle.ColumnaOrdenacion = "IdCotizacionDetalle";
        GridPedidoDetalle.Metodo = "ObtenerPedidoDetalle";
        GridPedidoDetalle.TituloTabla = "Detalle de pedido";
        GridPedidoDetalle.GenerarGridCargaInicial = false;
        GridPedidoDetalle.GenerarFuncionFiltro = false;
        GridPedidoDetalle.Ancho = 840;
        GridPedidoDetalle.Altura = 80;

        //IdPedido
        CJQColumn ColIdPedidoDetalle = new CJQColumn();
        ColIdPedidoDetalle.Nombre = "IdCotizacionDetalle";
        ColIdPedidoDetalle.Oculto = "true";
        ColIdPedidoDetalle.Encabezado = "IdCotizacionDetalle";
        ColIdPedidoDetalle.Buscador = "false";
        GridPedidoDetalle.Columnas.Add(ColIdPedidoDetalle);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Buscador = "false";
        ColProducto.Alineacion = "left";
        ColProducto.Ancho = "50";
        GridPedidoDetalle.Columnas.Add(ColProducto);

        //Descripción
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "150";
        GridPedidoDetalle.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "right";
        ColCantidad.Ancho = "50";
        GridPedidoDetalle.Columnas.Add(ColCantidad);

        //Cantidad pendiente
        CJQColumn ColCantidadPendiente = new CJQColumn();
        ColCantidadPendiente.Nombre = "CantidadPendiente";
        ColCantidadPendiente.Encabezado = "Cantidad pendiente";
        ColCantidadPendiente.Buscador = "false";
        ColCantidadPendiente.Alineacion = "right";
        ColCantidadPendiente.Ancho = "50";
        GridPedidoDetalle.Columnas.Add(ColCantidadPendiente);

        //Precio
        CJQColumn ColPrecio = new CJQColumn();
        ColPrecio.Nombre = "Precio";
        ColPrecio.Encabezado = "Precio";
        ColPrecio.Buscador = "false";
        ColPrecio.Alineacion = "right";
        ColPrecio.Formato = "FormatoMoneda";
        ColPrecio.Ancho = "70";
        GridPedidoDetalle.Columnas.Add(ColPrecio);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Alineacion = "right";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Ancho = "70";
        GridPedidoDetalle.Columnas.Add(ColTotal);

        //Existencia
        CJQColumn ColExistencia = new CJQColumn();
        ColExistencia.Nombre = "Existencia";
        ColExistencia.Encabezado = "Existencia";
        ColExistencia.Buscador = "false";
        ColExistencia.Alineacion = "left";
        ColExistencia.Ancho = "70";
        GridPedidoDetalle.Columnas.Add(ColExistencia);

        //PSF
        CJQColumn ColPSF = new CJQColumn();
        ColPSF.Nombre = "PSF";
        ColPSF.Encabezado = "PSF";
        ColPSF.Buscador = "false";
        ColPSF.Alineacion = "right";
        ColPSF.Ancho = "50";
        GridPedidoDetalle.Columnas.Add(ColPSF);

        //IdProducto
        CJQColumn ColIdProducto = new CJQColumn();
        ColIdProducto.Nombre = "IdProducto";
        ColIdProducto.Oculto = "true";
        ColIdProducto.Encabezado = "IdProducto";
        ColIdProducto.Buscador = "false";
        GridPedidoDetalle.Columnas.Add(ColIdProducto);

        //IdServicio
        CJQColumn ColIdServicio = new CJQColumn();
        ColIdServicio.Nombre = "IdServicio";
        ColIdServicio.Oculto = "true";
        ColIdServicio.Encabezado = "IdServicio";
        ColIdServicio.Buscador = "false";
        GridPedidoDetalle.Columnas.Add(ColIdServicio);

        //IdTipoIVA
        CJQColumn ColIdTipoIVA = new CJQColumn();
        ColIdTipoIVA.Nombre = "IdTipoIVA";
        ColIdTipoIVA.Oculto = "true";
        ColIdTipoIVA.Encabezado = "IdTipoIVA";
        ColIdTipoIVA.Buscador = "false";
        GridPedidoDetalle.Columnas.Add(ColIdTipoIVA);

        //IVA
        CJQColumn ColIVA = new CJQColumn();
        ColIVA.Nombre = "IVA";
        ColIVA.Oculto = "true";
        ColIVA.Encabezado = "IVA";
        ColIVA.Buscador = "false";
        ColIVA.Ancho = "50";
        GridPedidoDetalle.Columnas.Add(ColIVA);

        //Nota
        CJQColumn ColNota = new CJQColumn();
        ColNota.Nombre = "Nota";
        ColNota.Encabezado = "Nota";
        ColNota.Buscador = "false";
        ColNota.Alineacion = "left";
        ColNota.Oculto = "true";
        ColNota.Ancho = "150";
        GridPedidoDetalle.Columnas.Add(ColNota);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPedidoDetalle", GridPedidoDetalle.GeneraGrid(), true);

    }

    public void PintaGridConceptoProyecto()
    {
        //GridPedidoDetalle
        CJQGrid GridConceptoProyecto = new CJQGrid();
        GridConceptoProyecto.NombreTabla = "grdConceptoProyecto";
        GridConceptoProyecto.CampoIdentificador = "IdConceptoProyecto";
        GridConceptoProyecto.ColumnaOrdenacion = "OrdenConcepto";
        GridConceptoProyecto.Metodo = "ObtenerConceptoProyecto";
        GridConceptoProyecto.TituloTabla = "Conceptos del proyecto";
        GridConceptoProyecto.GenerarGridCargaInicial = false;
        GridConceptoProyecto.GenerarFuncionFiltro = false;
        GridConceptoProyecto.Ancho = 840;
        GridConceptoProyecto.Altura = 80;
        GridConceptoProyecto.RangoNumeroRegistros = "200, 400, 600";
        GridConceptoProyecto.NumeroRegistros = 200;

        //IdConceptoProyecto
        CJQColumn ColIdConceptoProyecto = new CJQColumn();
        ColIdConceptoProyecto.Nombre = "IdConceptoProyecto";
        ColIdConceptoProyecto.Oculto = "true";
        ColIdConceptoProyecto.Encabezado = "IdConceptoProyecto";
        ColIdConceptoProyecto.Buscador = "false";
        GridConceptoProyecto.Columnas.Add(ColIdConceptoProyecto);

        //Clave
        CJQColumn ColClaveConcepto = new CJQColumn();
        ColClaveConcepto.Nombre = "Clave";
        ColClaveConcepto.Encabezado = "Clave";
        ColClaveConcepto.Buscador = "false";
        ColClaveConcepto.Alineacion = "left";
        ColClaveConcepto.Ancho = "50";
        GridConceptoProyecto.Columnas.Add(ColClaveConcepto);


        //DescripciónConcepto
        CJQColumn ColDescripcionConcepto = new CJQColumn();
        ColDescripcionConcepto.Nombre = "Descripcion";
        ColDescripcionConcepto.Encabezado = "Descripción";
        ColDescripcionConcepto.Buscador = "false";
        ColDescripcionConcepto.Alineacion = "left";
        ColDescripcionConcepto.Ancho = "150";
        GridConceptoProyecto.Columnas.Add(ColDescripcionConcepto);

        //CantidadConcepto
        CJQColumn ColCantidadConcepto = new CJQColumn();
        ColCantidadConcepto.Nombre = "Cantidad";
        ColCantidadConcepto.Encabezado = "Cantidad";
        ColCantidadConcepto.Buscador = "false";
        ColCantidadConcepto.Alineacion = "right";
        ColCantidadConcepto.Ancho = "50";
        GridConceptoProyecto.Columnas.Add(ColCantidadConcepto);

        //Precio
        CJQColumn ColPrecioConcepto = new CJQColumn();
        ColPrecioConcepto.Nombre = "Precio";
        ColPrecioConcepto.Encabezado = "Precio";
        ColPrecioConcepto.Buscador = "false";
        ColPrecioConcepto.Alineacion = "right";
        ColPrecioConcepto.Formato = "FormatoMoneda";
        ColPrecioConcepto.Ancho = "70";
        GridConceptoProyecto.Columnas.Add(ColPrecioConcepto);

        //Total
        CJQColumn ColTotalConcepto = new CJQColumn();
        ColTotalConcepto.Nombre = "Total";
        ColTotalConcepto.Encabezado = "Total";
        ColTotalConcepto.Buscador = "false";
        ColTotalConcepto.Alineacion = "right";
        ColTotalConcepto.Formato = "FormatoMoneda";
        ColTotalConcepto.Ancho = "70";
        GridConceptoProyecto.Columnas.Add(ColTotalConcepto);

        //IVA
        CJQColumn ColIVAConcepto = new CJQColumn();
        ColIVAConcepto.Nombre = "IVA";
        ColIVAConcepto.Oculto = "false";
        ColIVAConcepto.Encabezado = "IVA";
        ColIVAConcepto.Buscador = "false";
        ColIVAConcepto.Ancho = "50";
        GridConceptoProyecto.Columnas.Add(ColIVAConcepto);

        //Elegir
        CJQColumn ColElegir = new CJQColumn();
        ColElegir.Nombre = "Elegir";
        ColElegir.Encabezado = "Elegir";
        ColElegir.Buscador = "false";
        ColElegir.Alineacion = "left";
        ColElegir.Etiquetado = "CheckBoxchecked";
        ColElegir.Id = "chkElegir";
        ColElegir.Oculto = "true";
        ColElegir.Estilo = "chkElegir";
        ColElegir.Ancho = "30";
        GridConceptoProyecto.Columnas.Add(ColElegir);

        ClientScript.RegisterStartupScript(this.GetType(), "grdConceptoProyecto", GridConceptoProyecto.GeneraGrid(), true);

    }

    public void PintaGridFacturasSustituye()
    {
        //GridPedidoDetalle
        CJQGrid GridFacturasSustituye = new CJQGrid();
        GridFacturasSustituye.NombreTabla = "grdFacturasSustituye";
        GridFacturasSustituye.CampoIdentificador = "IdFacturaEncabezadoSustituye";
        GridFacturasSustituye.ColumnaOrdenacion = "IdFacturaEncabezadoSustituye";
        GridFacturasSustituye.Metodo = "ObtenerFacturasSustituye";
        GridFacturasSustituye.TituloTabla = "Facturas que sustituye";
        GridFacturasSustituye.GenerarGridCargaInicial = false;
        GridFacturasSustituye.GenerarFuncionFiltro = false;
        GridFacturasSustituye.Ancho = 440;
        GridFacturasSustituye.Altura = 80;

        //IdFacturaEncabezadoSustituye
        CJQColumn ColIdFacturaEncabezadoSustituye = new CJQColumn();
        ColIdFacturaEncabezadoSustituye.Nombre = "IdFacturaEncabezadoSustituye";
        ColIdFacturaEncabezadoSustituye.Oculto = "true";
        ColIdFacturaEncabezadoSustituye.Encabezado = "IdFacturaEncabezadoSustituye";
        ColIdFacturaEncabezadoSustituye.Buscador = "false";
        GridFacturasSustituye.Columnas.Add(ColIdFacturaEncabezadoSustituye);

        //SerieFacturaEncabezado
        CJQColumn ColSerieFacturaEncabezadoSustituye = new CJQColumn();
        ColSerieFacturaEncabezadoSustituye.Nombre = "SerieFactura";
        ColSerieFacturaEncabezadoSustituye.Encabezado = "Serie";
        ColSerieFacturaEncabezadoSustituye.Ancho = "50";
        ColSerieFacturaEncabezadoSustituye.Buscador = "false";
        ColSerieFacturaEncabezadoSustituye.Alineacion = "left";
        GridFacturasSustituye.Columnas.Add(ColSerieFacturaEncabezadoSustituye);

        //FolioFacturaEncabezado
        CJQColumn ColFolioFacturaEncabezadoSustituye = new CJQColumn();
        ColFolioFacturaEncabezadoSustituye.Nombre = "NumeroFactura";
        ColFolioFacturaEncabezadoSustituye.Encabezado = "Folio";
        ColFolioFacturaEncabezadoSustituye.Ancho = "50";
        ColFolioFacturaEncabezadoSustituye.Buscador = "false";
        ColFolioFacturaEncabezadoSustituye.Alineacion = "left";
        GridFacturasSustituye.Columnas.Add(ColFolioFacturaEncabezadoSustituye);

        //Eliminar LA factura asignada
        CJQColumn ColEliminarFacturaEncabezadoSustituye = new CJQColumn();
        ColEliminarFacturaEncabezadoSustituye.Nombre = "Eliminar";
        ColEliminarFacturaEncabezadoSustituye.Encabezado = "Eliminar";
        ColEliminarFacturaEncabezadoSustituye.Etiquetado = "Imagen";
        ColEliminarFacturaEncabezadoSustituye.Imagen = "eliminar.png";
        ColEliminarFacturaEncabezadoSustituye.Estilo = "divImagenConsultar imgEliminarFacturaEncabezadoSustituye";
        ColEliminarFacturaEncabezadoSustituye.Buscador = "false";
        ColEliminarFacturaEncabezadoSustituye.Ordenable = "false";
        ColEliminarFacturaEncabezadoSustituye.Ancho = "40";
        GridFacturasSustituye.Columnas.Add(ColEliminarFacturaEncabezadoSustituye);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturasSustituye", GridFacturasSustituye.GeneraGrid(), true);

    }

    public void PintaGridDetalleFacturar()
    {
        //GridFacturaDetalle
        CJQGrid GridFacturaDetalle = new CJQGrid();
        GridFacturaDetalle.NombreTabla = "grdFacturaDetalle";
        GridFacturaDetalle.CampoIdentificador = "IdFacturaDetalle";
        GridFacturaDetalle.ColumnaOrdenacion = "IdFacturaDetalle";
        GridFacturaDetalle.Metodo = "ObtenerFacturaDetalle";
        GridFacturaDetalle.TituloTabla = "Detalle de factura";
        GridFacturaDetalle.GenerarGridCargaInicial = false;
        GridFacturaDetalle.GenerarFuncionFiltro = false;
        GridFacturaDetalle.Ancho = 870;
        GridFacturaDetalle.Altura = 80;

        //IdFactura
        CJQColumn ColIdFacturaDetalle = new CJQColumn();
        ColIdFacturaDetalle.Nombre = "IdFacturaDetalle";
        ColIdFacturaDetalle.Oculto = "true";
        ColIdFacturaDetalle.Encabezado = "IdFacturaDetalle";
        ColIdFacturaDetalle.Buscador = "false";
        GridFacturaDetalle.Columnas.Add(ColIdFacturaDetalle);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Buscador = "false";
        ColClave.Alineacion = "left";
        ColClave.Ancho = "50";
        GridFacturaDetalle.Columnas.Add(ColClave);

        //Proyecto
        CJQColumn ColProyectoConsultar = new CJQColumn();
        ColProyectoConsultar.Nombre = "Proyecto";
        ColProyectoConsultar.Encabezado = "Proyecto";
        ColProyectoConsultar.Buscador = "false";
        ColProyectoConsultar.Alineacion = "left";
        ColProyectoConsultar.Ancho = "50";
        GridFacturaDetalle.Columnas.Add(ColProyectoConsultar);

        //Cotizacion
        CJQColumn ColCotizacionConsultar = new CJQColumn();
        ColCotizacionConsultar.Nombre = "Cotizacion";
        ColCotizacionConsultar.Encabezado = "Pedido";
        ColCotizacionConsultar.Buscador = "false";
        ColCotizacionConsultar.Alineacion = "left";
        ColCotizacionConsultar.Ancho = "50";
        GridFacturaDetalle.Columnas.Add(ColCotizacionConsultar);

        //Descripción
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "150";
        GridFacturaDetalle.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "right";
        ColCantidad.Ancho = "50";
        GridFacturaDetalle.Columnas.Add(ColCantidad);

        //PrecioUnitario
        CJQColumn ColPrecioUnitario = new CJQColumn();
        ColPrecioUnitario.Nombre = "PrecioUnitario";
        ColPrecioUnitario.Encabezado = "Precio unitario";
        ColPrecioUnitario.Buscador = "false";
        ColPrecioUnitario.Alineacion = "right";
        ColPrecioUnitario.Formato = "FormatoMoneda";
        ColPrecioUnitario.Ancho = "70";
        GridFacturaDetalle.Columnas.Add(ColPrecioUnitario);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Alineacion = "right";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Ancho = "70";
        GridFacturaDetalle.Columnas.Add(ColTotal);

        //Almacen
        CJQColumn ColAlmacen = new CJQColumn();
        ColAlmacen.Nombre = "Almacen";
        ColAlmacen.Encabezado = "Almacen";
        ColAlmacen.Buscador = "false";
        ColAlmacen.Alineacion = "left";
        ColAlmacen.Ancho = "70";
        GridFacturaDetalle.Columnas.Add(ColAlmacen);

        //Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "Descuento";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Buscador = "false";
        ColDescuento.Alineacion = "right";
        ColDescuento.Formato = "FormatoMoneda";
        ColDescuento.Ancho = "50";
        GridFacturaDetalle.Columnas.Add(ColDescuento);

        //IVA
        CJQColumn ColTotalIVA = new CJQColumn();
        ColTotalIVA.Nombre = "IVA";
        ColTotalIVA.Encabezado = "IVA";
        ColTotalIVA.Buscador = "false";
        ColTotalIVA.Alineacion = "right";
        ColTotalIVA.Formato = "FormatoMoneda";
        ColTotalIVA.Ancho = "50";
        GridFacturaDetalle.Columnas.Add(ColTotalIVA);

        //Eliminar concepto factura de proveedor consultar
        CJQColumn ColEliminarConcepto = new CJQColumn();
        ColEliminarConcepto.Nombre = "Eliminar";
        ColEliminarConcepto.Encabezado = "Eliminar";
        ColEliminarConcepto.Etiquetado = "Imagen";
        ColEliminarConcepto.Imagen = "eliminar.png";
        ColEliminarConcepto.Estilo = "divImagenConsultar imgEliminarConcepto";
        ColEliminarConcepto.Buscador = "false";
        ColEliminarConcepto.Ordenable = "false";
        ColEliminarConcepto.Ancho = "40";
        GridFacturaDetalle.Columnas.Add(ColEliminarConcepto);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturaDetalle", GridFacturaDetalle.GeneraGrid(), true);
    }

    public void PintaGridDetalleFacturarConsultar()
    {
        //GridFacturaDetalle
        CJQGrid GridFacturaDetalleConsultar = new CJQGrid();
        GridFacturaDetalleConsultar.NombreTabla = "grdFacturaDetalleConsultar";
        GridFacturaDetalleConsultar.CampoIdentificador = "IdFacturaDetalle";
        GridFacturaDetalleConsultar.ColumnaOrdenacion = "IdFacturaDetalle";
        GridFacturaDetalleConsultar.Metodo = "ObtenerFacturaDetalleConsultar";
        GridFacturaDetalleConsultar.TituloTabla = "Detalle de factura";
        GridFacturaDetalleConsultar.GenerarGridCargaInicial = false;
        GridFacturaDetalleConsultar.GenerarFuncionFiltro = false;
        GridFacturaDetalleConsultar.Ancho = 870;
        GridFacturaDetalleConsultar.Altura = 80;

        //IdFactura
        CJQColumn ColIdFacturaDetalleConsultar = new CJQColumn();
        ColIdFacturaDetalleConsultar.Nombre = "IdFacturaDetalle";
        ColIdFacturaDetalleConsultar.Oculto = "true";
        ColIdFacturaDetalleConsultar.Encabezado = "IdFacturaDetalle";
        ColIdFacturaDetalleConsultar.Buscador = "false";
        GridFacturaDetalleConsultar.Columnas.Add(ColIdFacturaDetalleConsultar);

        //Clave
        CJQColumn ColClaveConsultar = new CJQColumn();
        ColClaveConsultar.Nombre = "Clave";
        ColClaveConsultar.Encabezado = "Clave";
        ColClaveConsultar.Buscador = "false";
        ColClaveConsultar.Alineacion = "left";
        ColClaveConsultar.Ancho = "50";
        GridFacturaDetalleConsultar.Columnas.Add(ColClaveConsultar);

        //Proyecto
        CJQColumn ColProyectoConsultar = new CJQColumn();
        ColProyectoConsultar.Nombre = "Proyecto";
        ColProyectoConsultar.Encabezado = "Proyecto";
        ColProyectoConsultar.Buscador = "false";
        ColProyectoConsultar.Alineacion = "left";
        ColProyectoConsultar.Ancho = "50";
        GridFacturaDetalleConsultar.Columnas.Add(ColProyectoConsultar);

        //Cotizacion
        CJQColumn ColCotizacionConsultar = new CJQColumn();
        ColCotizacionConsultar.Nombre = "Cotizacion";
        ColCotizacionConsultar.Encabezado = "Pedido";
        ColCotizacionConsultar.Buscador = "false";
        ColCotizacionConsultar.Alineacion = "left";
        ColCotizacionConsultar.Ancho = "50";
        GridFacturaDetalleConsultar.Columnas.Add(ColCotizacionConsultar);

        //Descripción
        CJQColumn ColDescripcionConsultar = new CJQColumn();
        ColDescripcionConsultar.Nombre = "Descripcion";
        ColDescripcionConsultar.Encabezado = "Descripción";
        ColDescripcionConsultar.Buscador = "false";
        ColDescripcionConsultar.Alineacion = "left";
        ColDescripcionConsultar.Ancho = "150";
        GridFacturaDetalleConsultar.Columnas.Add(ColDescripcionConsultar);

        //Cantidad
        CJQColumn ColCantidadConsultar = new CJQColumn();
        ColCantidadConsultar.Nombre = "Cantidad";
        ColCantidadConsultar.Encabezado = "Cantidad";
        ColCantidadConsultar.Buscador = "false";
        ColCantidadConsultar.Alineacion = "right";
        ColCantidadConsultar.Ancho = "50";
        GridFacturaDetalleConsultar.Columnas.Add(ColCantidadConsultar);

        //PrecioUnitario
        CJQColumn ColPrecioUnitarioConsultar = new CJQColumn();
        ColPrecioUnitarioConsultar.Nombre = "PrecioUnitario";
        ColPrecioUnitarioConsultar.Encabezado = "Precio unitario";
        ColPrecioUnitarioConsultar.Buscador = "false";
        ColPrecioUnitarioConsultar.Alineacion = "right";
        ColPrecioUnitarioConsultar.Formato = "FormatoMoneda";
        ColPrecioUnitarioConsultar.Ancho = "70";
        GridFacturaDetalleConsultar.Columnas.Add(ColPrecioUnitarioConsultar);

        //Total
        CJQColumn ColTotalConsultar = new CJQColumn();
        ColTotalConsultar.Nombre = "Total";
        ColTotalConsultar.Encabezado = "Total";
        ColTotalConsultar.Buscador = "false";
        ColTotalConsultar.Alineacion = "right";
        ColTotalConsultar.Formato = "FormatoMoneda";
        ColTotalConsultar.Ancho = "70";
        GridFacturaDetalleConsultar.Columnas.Add(ColTotalConsultar);

        //Almacen
        CJQColumn ColAlmacenConsultar = new CJQColumn();
        ColAlmacenConsultar.Nombre = "Almacen";
        ColAlmacenConsultar.Encabezado = "Almacen";
        ColAlmacenConsultar.Buscador = "false";
        ColAlmacenConsultar.Alineacion = "left";
        ColAlmacenConsultar.Ancho = "70";
        GridFacturaDetalleConsultar.Columnas.Add(ColAlmacenConsultar);

        //Descuento
        CJQColumn ColDescuentoConsultar = new CJQColumn();
        ColDescuentoConsultar.Nombre = "Descuento";
        ColDescuentoConsultar.Encabezado = "Descuento";
        ColDescuentoConsultar.Buscador = "false";
        ColDescuentoConsultar.Alineacion = "right";
        ColDescuentoConsultar.Formato = "FormatoMoneda";
        ColDescuentoConsultar.Ancho = "50";
        GridFacturaDetalleConsultar.Columnas.Add(ColDescuentoConsultar);

        //IVA
        CJQColumn ColTotalIVAConsultar = new CJQColumn();
        ColTotalIVAConsultar.Nombre = "IVA";
        ColTotalIVAConsultar.Encabezado = "IVA";
        ColTotalIVAConsultar.Buscador = "false";
        ColTotalIVAConsultar.Alineacion = "right";
        ColTotalIVAConsultar.Formato = "FormatoMoneda";
        ColTotalIVAConsultar.Ancho = "50";
        GridFacturaDetalleConsultar.Columnas.Add(ColTotalIVAConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturaDetalleConsultar", GridFacturaDetalleConsultar.GeneraGrid(), true);
    }

    public void PintaGridDetalleFacturarEditar()
    {
        //GridFacturaDetalle
        CJQGrid GridFacturaDetalleEditar = new CJQGrid();
        GridFacturaDetalleEditar.NombreTabla = "grdFacturaDetalleEditar";
        GridFacturaDetalleEditar.CampoIdentificador = "IdFacturaDetalle";
        GridFacturaDetalleEditar.ColumnaOrdenacion = "IdFacturaDetalle";
        GridFacturaDetalleEditar.Metodo = "ObtenerFacturaDetalleEditar";
        GridFacturaDetalleEditar.TituloTabla = "Detalle de factura";
        GridFacturaDetalleEditar.GenerarGridCargaInicial = false;
        GridFacturaDetalleEditar.GenerarFuncionFiltro = false;
        GridFacturaDetalleEditar.Ancho = 870;
        GridFacturaDetalleEditar.Altura = 80;

        //IdFactura
        CJQColumn ColIdFacturaDetalleEditar = new CJQColumn();
        ColIdFacturaDetalleEditar.Nombre = "IdFacturaDetalle";
        ColIdFacturaDetalleEditar.Oculto = "true";
        ColIdFacturaDetalleEditar.Encabezado = "IdFacturaDetalle";
        ColIdFacturaDetalleEditar.Buscador = "false";
        GridFacturaDetalleEditar.Columnas.Add(ColIdFacturaDetalleEditar);

        //Clave
        CJQColumn ColClaveEditar = new CJQColumn();
        ColClaveEditar.Nombre = "Clave";
        ColClaveEditar.Encabezado = "Clave";
        ColClaveEditar.Buscador = "false";
        ColClaveEditar.Alineacion = "left";
        ColClaveEditar.Ancho = "50";
        GridFacturaDetalleEditar.Columnas.Add(ColClaveEditar);

        //Proyecto
        CJQColumn ColProyectoConsultar = new CJQColumn();
        ColProyectoConsultar.Nombre = "Proyecto";
        ColProyectoConsultar.Encabezado = "Proyecto";
        ColProyectoConsultar.Buscador = "false";
        ColProyectoConsultar.Alineacion = "left";
        ColProyectoConsultar.Ancho = "50";
        GridFacturaDetalleEditar.Columnas.Add(ColProyectoConsultar);

        //Cotizacion
        CJQColumn ColCotizacionConsultar = new CJQColumn();
        ColCotizacionConsultar.Nombre = "Cotizacion";
        ColCotizacionConsultar.Encabezado = "Pedido";
        ColCotizacionConsultar.Buscador = "false";
        ColCotizacionConsultar.Alineacion = "left";
        ColCotizacionConsultar.Ancho = "50";
        GridFacturaDetalleEditar.Columnas.Add(ColCotizacionConsultar);

        //Descripción
        CJQColumn ColDescripcionEditar = new CJQColumn();
        ColDescripcionEditar.Nombre = "Descripcion";
        ColDescripcionEditar.Encabezado = "Descripción";
        ColDescripcionEditar.Buscador = "false";
        ColDescripcionEditar.Alineacion = "left";
        ColDescripcionEditar.Ancho = "150";
        GridFacturaDetalleEditar.Columnas.Add(ColDescripcionEditar);

        //Cantidad
        CJQColumn ColCantidadEditar = new CJQColumn();
        ColCantidadEditar.Nombre = "Cantidad";
        ColCantidadEditar.Encabezado = "Cantidad";
        ColCantidadEditar.Buscador = "false";
        ColCantidadEditar.Alineacion = "right";
        ColCantidadEditar.Ancho = "50";
        GridFacturaDetalleEditar.Columnas.Add(ColCantidadEditar);

        //PrecioUnitario
        CJQColumn ColPrecioUnitarioEditar = new CJQColumn();
        ColPrecioUnitarioEditar.Nombre = "PrecioUnitario";
        ColPrecioUnitarioEditar.Encabezado = "Precio unitario";
        ColPrecioUnitarioEditar.Buscador = "false";
        ColPrecioUnitarioEditar.Alineacion = "right";
        ColPrecioUnitarioEditar.Formato = "FormatoMoneda";
        ColPrecioUnitarioEditar.Ancho = "70";
        GridFacturaDetalleEditar.Columnas.Add(ColPrecioUnitarioEditar);

        //Total
        CJQColumn ColTotalEditar = new CJQColumn();
        ColTotalEditar.Nombre = "Total";
        ColTotalEditar.Encabezado = "Total";
        ColTotalEditar.Buscador = "false";
        ColTotalEditar.Alineacion = "right";
        ColTotalEditar.Formato = "FormatoMoneda";
        ColTotalEditar.Ancho = "70";
        GridFacturaDetalleEditar.Columnas.Add(ColTotalEditar);

        //Almacen
        CJQColumn ColAlmacenEditar = new CJQColumn();
        ColAlmacenEditar.Nombre = "Almacen";
        ColAlmacenEditar.Encabezado = "Almacen";
        ColAlmacenEditar.Buscador = "false";
        ColAlmacenEditar.Alineacion = "left";
        ColAlmacenEditar.Ancho = "70";
        GridFacturaDetalleEditar.Columnas.Add(ColAlmacenEditar);

        //Descuento
        CJQColumn ColDescuentoEditar = new CJQColumn();
        ColDescuentoEditar.Nombre = "Descuento";
        ColDescuentoEditar.Encabezado = "Descuento";
        ColDescuentoEditar.Buscador = "false";
        ColDescuentoEditar.Alineacion = "right";
        ColDescuentoEditar.Formato = "FormatoMoneda";
        ColDescuentoEditar.Ancho = "50";
        GridFacturaDetalleEditar.Columnas.Add(ColDescuentoEditar);

        //IVA
        CJQColumn ColTotalIVAEditar = new CJQColumn();
        ColTotalIVAEditar.Nombre = "IVA";
        ColTotalIVAEditar.Encabezado = "IVA";
        ColTotalIVAEditar.Buscador = "false";
        ColTotalIVAEditar.Alineacion = "right";
        ColTotalIVAEditar.Formato = "FormatoMoneda";
        ColTotalIVAEditar.Ancho = "50";
        GridFacturaDetalleEditar.Columnas.Add(ColTotalIVAEditar);

        //Eliminar concepto factura de proveedor consultar
        CJQColumn ColEliminarConceptoEditar = new CJQColumn();
        ColEliminarConceptoEditar.Nombre = "Eliminar";
        ColEliminarConceptoEditar.Encabezado = "Eliminar";
        ColEliminarConceptoEditar.Etiquetado = "Imagen";
        ColEliminarConceptoEditar.Imagen = "eliminar.png";
        ColEliminarConceptoEditar.Estilo = "divImagenConsultar imgEliminarConcepto";
        ColEliminarConceptoEditar.Buscador = "false";
        ColEliminarConceptoEditar.Ordenable = "false";
        ColEliminarConceptoEditar.Ancho = "40";
        GridFacturaDetalleEditar.Columnas.Add(ColEliminarConceptoEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturaDetalleEditar", GridFacturaDetalleEditar.GeneraGrid(), true);
    }

    [WebMethod]
    public static string ObtenerDatosFacturaXML(string pArchivo)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {

            //CREAR OL OBJETO DEL DOCUMENTO
            XmlDocument objXMLDoc = new XmlDocument();
            string serie = "";
            string folio = "";
            string cadena2 = "";
            objXMLDoc.Load(@"C:\inetpub\wwwroot\KeepInfoWeb\arquitecturaNet\Archivos\ArchivosAddendas\" + pArchivo);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(objXMLDoc.NameTable);
            nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            XmlNode nComprobante = objXMLDoc.SelectSingleNode("//cfdi:Comprobante", nsmgr);
            XmlNodeReader nr = new XmlNodeReader(nComprobante);
            JObject Modelo = new JObject();
            if (nr.Read())
            {
                Modelo.Add(new JProperty("folio", nr.GetAttribute("folio")));
                Modelo.Add(new JProperty("serie", nr.GetAttribute("serie")));
                Modelo.Add(new JProperty("tipoDeComprobante", nr.GetAttribute("tipoDeComprobante")));
                Modelo.Add(new JProperty("fecha", nr.GetAttribute("fecha")));
                Modelo.Add(new JProperty("subTotal", nr.GetAttribute("subTotal")));
                Modelo.Add(new JProperty("total", nr.GetAttribute("total")));
                Modelo.Add(new JProperty("Moneda", nr.GetAttribute("Moneda")));
                Modelo.Add(new JProperty("TipoCambio", nr.GetAttribute("TipoCambio")));

            }

            XmlNode nEmisior = objXMLDoc.SelectSingleNode("//cfdi:Comprobante", nsmgr).SelectSingleNode("//cfdi:Emisor", nsmgr);
            XmlNodeReader nr2 = new XmlNodeReader(nEmisior);
            if (nr2.Read())
            {
                Modelo.Add(new JProperty("nombreemisior", nr2.GetAttribute("nombre")));
                Modelo.Add(new JProperty("rfcemisior", nr2.GetAttribute("rfc")));
            }

            XmlNode nReceptor = objXMLDoc.SelectSingleNode("//cfdi:Comprobante", nsmgr).SelectSingleNode("//cfdi:Receptor", nsmgr);
            XmlNodeReader nr3 = new XmlNodeReader(nReceptor);
            if (nr3.Read())
            {
                Modelo.Add(new JProperty("nombrereceptor", nr3.GetAttribute("nombre")));
                Modelo.Add(new JProperty("rfcreceptor", nr3.GetAttribute("rfc")));

            }

            XmlNode nTraslado = objXMLDoc.SelectSingleNode("//cfdi:Comprobante", nsmgr).SelectSingleNode("//cfdi:Impuestos", nsmgr).SelectSingleNode("//cfdi:Traslados", nsmgr);
            XmlNodeReader nr4 = new XmlNodeReader(nTraslado);

            while (nr4.Read())
            {
                if (nr4.HasAttributes)
                {
                    while (nr4.MoveToNextAttribute())
                    {
                        if (nr4.Name == "impuesto" && nr4.Value.ToUpper() == "IVA")
                        {
                            nr4.MoveToFirstAttribute();
                            do
                            {
                                if (nr4.Name == "tasa")
                                {
                                    Modelo.Add(new JProperty("tasa", nr4.Value));
                                }
                                else if (nr4.Name == "importe")
                                {
                                    Modelo.Add(new JProperty("importe", nr4.Value));
                                }
                            }
                            while (nr4.MoveToNextAttribute());
                        }
                    }
                }
            }

            int pIdAddenda = 0;
            pIdAddenda = 1;

            SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
            SqlCommand Stored = new SqlCommand("spr_ObtieneConceptosAddenda", sqlCon);
            Stored.CommandType = CommandType.StoredProcedure;
            Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = 100;
            Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = 1;
            Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = "";
            Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = "";
            Stored.Parameters.Add("IdAddenda", SqlDbType.Int).Value = pIdAddenda;
            DataSet dataSet = new DataSet();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
            dataAdapter.Fill(dataSet);
            List<string> Columnas = new List<string>();
            JArray Datos = new JArray();
            foreach (DataRow row in dataSet.Tables[1].Rows)
            {
                for (int i = 0; i < dataSet.Tables[1].Rows.Count; i++)
                {
                    Columnas.Add(dataSet.Tables[1].Rows[i][0].ToString());
                }
                break;
            }

            Modelo.Add(new JProperty("Columnas", Columnas));
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    //public static CJQGridJsonResponseReporte ObtenerConceptos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdAddenda)
    public static CJQGridJsonResponseReporte ObtenerConceptos(Dictionary<string, object> Conceptos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        //CREAR OL OBJETO DEL DOCUMENTO
        XmlDocument objXMLDoc = new XmlDocument();
        string serie = "";
        string folio = "";
        string cadena2 = "";
        objXMLDoc.Load(@"C:\inetpub\wwwroot\KeepInfoWeb\arquitecturaNet\Archivos\ArchivosAddendas\AMT228.xml");

        XmlNamespaceManager nsmgr = new XmlNamespaceManager(objXMLDoc.NameTable);
        nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

        XmlNode nConcepto = objXMLDoc.SelectSingleNode("//cfdi:Comprobante", nsmgr).SelectSingleNode("//cfdi:Conceptos", nsmgr);
        XmlNodeReader nrc = new XmlNodeReader(nConcepto);

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();

        dt.Columns.Add("NoRegistros");
        dt.Columns.Add("NoPaginas");
        dt.Columns.Add("PaginaActual");

        object[] Encabezados;
        Encabezados = (object[])Conceptos["pEncabezados"];
        for (int e = 0; e < Encabezados.Length; e++)
        {
            string[] Valores = Encabezados[e].ToString().Split('|');
            dt1.Columns.Add(Valores[0].ToString());
        }
        int NoRegistros = 0;
        int ExisteRegistro = 0;

        DataRow dr1;
        while (nrc.Read())
        {
            if (nrc.HasAttributes)
            {
                dr1 = dt1.NewRow();
                NoRegistros++;
                for (int f = 0; f < Encabezados.Length; f++)
                {
                    string[] Valores = Encabezados[f].ToString().Split('|');

                    ExisteRegistro = 0;
                    nrc.MoveToFirstAttribute();
                    do
                    {
                        if (Valores[0].ToString() == "IdAtributoAddenda")
                        {
                            dr1[Valores[0].ToString()] = NoRegistros.ToString();
                            ExisteRegistro = 1;
                            break;
                        }
                        if (Valores[1].ToString() != "")
                        {
                            if (nrc.Name.ToString() == Valores[1].ToString())
                            {
                                dr1[Valores[0].ToString()] = nrc.GetAttribute(nrc.Name.ToString());
                                ExisteRegistro = 1;
                                break;
                            }
                        }
                        else
                        {
                            if (nrc.Name.ToString() == Valores[0].ToString())
                            {
                                dr1[Valores[0].ToString()] = nrc.GetAttribute(nrc.Name.ToString());
                                ExisteRegistro = 1;
                                break;
                            }
                        }

                    } while (nrc.MoveToNextAttribute());
                    if (ExisteRegistro == 0)
                    {
                        dr1[Valores[0].ToString()] = "";
                    }
                }
                dt1.Rows.Add(dr1);

            }
        }
        DataRow dr;
        dr = dt.NewRow();
        dr["NoRegistros"] = NoRegistros;
        dr["NoPaginas"] = "1";
        dr["PaginaActual"] = "1";
        dt.Rows.Add(dr);

        DataSet dataSet = new DataSet();
        dataSet.Tables.Add(dt);
        dataSet.Tables.Add(dt1);

        return new CJQGridJsonResponseReporte(dataSet);
    }

    [WebMethod]
    public static string GeneraAddenda(Dictionary<string, object> pAddenda)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string validacion = "";

            JObject oRespuesta = new JObject();
            string cadena = "";
            if (validacion == "")
            {

                ////////////////////////////////////////////////////////////////////////////////////////////////////
                //CREAR OL OBJETO DEL DOCUMENTO


                XmlElement nFactura;
                XmlElement nMoneda;
                XmlElement nProveedor;
                XmlElement nDestino;
                XmlElement nPart;
                XmlElement nReferences;
                XmlNode nNode;
                XmlElement nElemento;


                XmlDocument objXMLDoc = new XmlDocument();
                objXMLDoc.Load(@"c:\FTG\AMT228.xml");
                XmlNode root = objXMLDoc.DocumentElement;
                root.RemoveChild(root.LastChild);

                XmlNode nAddenda = objXMLDoc.DocumentElement;
                XmlNode nConceptos = objXMLDoc.DocumentElement;

                CAddenda Addenda = new CAddenda();
                Addenda.LlenaObjeto(Convert.ToInt32(pAddenda["IdAddenda"]), ConexionBaseDatos);
                CEstructuraAddenda EstructuraAddenda = new CEstructuraAddenda();
                EstructuraAddenda.LlenaObjeto(Addenda.IdAddenda, ConexionBaseDatos);
                CAtributoAddenda AtributoAddenda = new CAtributoAddenda();

                int IdEstructuraAddenda = 0;
                string EstructuraAddendaD = "";
                int IdTipoElemento = 0;
                int IdPadre = 0;
                Boolean TieneHijos;
                int Orden = 0;

                CSelect ObtenObjeto = new CSelect();
                ObtenObjeto.StoredProcedure.CommandText = "sp_Estructura_Addenda_Consulta";
                ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdAddenda", Addenda.IdAddenda);
                ObtenObjeto.Llena<CEstructuraAddenda>(typeof(CEstructuraAddenda), ConexionBaseDatos);

                foreach (CEstructuraAddenda oEstructuraAddenda in ObtenObjeto.ListaRegistros)
                {
                    IdEstructuraAddenda = Convert.ToInt32(oEstructuraAddenda.IdEstructuraAddenda);
                    EstructuraAddendaD = Convert.ToString(oEstructuraAddenda.EstructuraAddenda);
                    IdTipoElemento = Convert.ToInt32(oEstructuraAddenda.IdTipoElemento);
                    TieneHijos = Convert.ToBoolean(oEstructuraAddenda.TieneHijos);
                    IdPadre = Convert.ToInt32(oEstructuraAddenda.IdPadre);
                    switch (IdTipoElemento)
                    {
                        case 1:
                            if (oEstructuraAddenda.Prefijo.ToString() != "")
                            {
                                nAddenda = objXMLDoc.CreateElement(oEstructuraAddenda.Prefijo.ToString(), EstructuraAddendaD.ToString(), "http://www.sat.gob.mx/cfd/3");
                            }
                            else
                            {
                                nAddenda = objXMLDoc.CreateElement(EstructuraAddendaD.ToString());
                            }
                            root.InsertAfter(nAddenda, root.LastChild);
                            root.AppendChild(nAddenda);
                            break;
                        case 2:
                            if (oEstructuraAddenda.Prefijo.ToString() != "")
                            {
                                nElemento = objXMLDoc.CreateElement(oEstructuraAddenda.Prefijo.ToString(), EstructuraAddendaD.ToString(), "https://www.interfactura.com/");
                            }
                            else
                            {
                                nElemento = objXMLDoc.CreateElement(EstructuraAddendaD.ToString());
                            }

                            AtributoAddenda.LlenaObjeto(IdEstructuraAddenda, ConexionBaseDatos);
                            if (AtributoAddenda.IdAtributoAddenda != 0)
                            {
                                CSelect ObtenObjeto1 = new CSelect();
                                ObtenObjeto1.StoredProcedure.CommandText = "sp_Atributo_Addenda_Consulta";
                                ObtenObjeto1.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                                ObtenObjeto1.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", IdEstructuraAddenda);
                                ObtenObjeto1.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), ConexionBaseDatos);
                                foreach (CAtributoAddenda oAtributoAddenda in ObtenObjeto1.ListaRegistros)
                                {
                                    if (oAtributoAddenda.EsConstante == true)
                                    {
                                        if (oAtributoAddenda.Valor.ToString().ToUpper() == "DOLARES" || oAtributoAddenda.Valor.ToString().ToUpper() == "DOLAR")
                                        {
                                            oAtributoAddenda.Valor = "USD";
                                        }
                                        else if (oAtributoAddenda.Valor.ToString().ToUpper() == "PESOS" || oAtributoAddenda.Valor.ToString().ToUpper() == "PESO")
                                        {
                                            oAtributoAddenda.Valor = "MXN";
                                        }
                                        nElemento.SetAttribute(oAtributoAddenda.AtributoAddenda, oAtributoAddenda.Valor);
                                    }
                                    else if (oAtributoAddenda.AtributoReferencia.ToString() != "")
                                    {
                                        string[] Valores = oAtributoAddenda.AtributoReferencia.ToString().Split('|');
                                        string Valor = "";
                                        for (int s = 0; s < Valores.Length; s++)
                                        {
                                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(objXMLDoc.NameTable);
                                            nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                            XmlNode nComprobante = objXMLDoc.SelectSingleNode(oAtributoAddenda.NodoABuscar, nsmgr);
                                            XmlNodeReader nr = new XmlNodeReader(nComprobante);
                                            JObject Modelo = new JObject();

                                            if (nr.Read())
                                            {
                                                Valor = Valor + nr.GetAttribute(Valores[s].ToString()).ToString();
                                            }
                                        }

                                        if (Valor.ToString().ToUpper() == "DOLARES" || Valor.ToString().ToUpper() == "DOLAR")
                                        {
                                            Valor = "USD";
                                        }
                                        else if (Valor.ToString().ToUpper() == "PESOS" || Valor.ToString().ToUpper() == "PESO")
                                        {
                                            Valor = "MXN";
                                        }

                                        if (oAtributoAddenda.Formato.ToString() != "" && oAtributoAddenda.TipoDato.ToString() != "")
                                        {
                                            switch (oAtributoAddenda.TipoDato)
                                            {
                                                case "Fecha":
                                                    nElemento.SetAttribute(oAtributoAddenda.AtributoAddenda, String.Format(oAtributoAddenda.Formato.ToString(), Convert.ToDateTime(Valor.ToString())));
                                                    break;
                                                case "Decimal":
                                                    nElemento.SetAttribute(oAtributoAddenda.AtributoAddenda, String.Format(oAtributoAddenda.Formato.ToString(), Convert.ToDecimal(Valor.ToString())));
                                                    break;
                                            }

                                        }
                                        else
                                        {
                                            nElemento.SetAttribute(oAtributoAddenda.AtributoAddenda, Valor.ToString());
                                        }
                                    }
                                }

                            }
                            nAddenda.AppendChild(nElemento);
                            break;
                        case 3:
                            if (oEstructuraAddenda.Prefijo.ToString() != "")
                            {
                                nConceptos = objXMLDoc.CreateElement(oEstructuraAddenda.Prefijo.ToString(), EstructuraAddendaD.ToString(), "http://www.sat.gob.mx/cfd/3");
                            }
                            else
                            {
                                nConceptos = objXMLDoc.CreateElement(EstructuraAddendaD.ToString());
                            }
                            nAddenda.AppendChild(nConceptos);
                            break;
                        case 4:

                            object[] Encabezados;
                            Encabezados = (object[])pAddenda["pEncabezados"];
                            foreach (Dictionary<string, object> oPartidas in (Array)pAddenda["DetallePartidas"])
                            {
                                if (oEstructuraAddenda.Prefijo.ToString() != "")
                                {
                                    nPart = objXMLDoc.CreateElement(oEstructuraAddenda.Prefijo.ToString(), EstructuraAddendaD.ToString(), "http://www.sat.gob.mx/cfd/3");
                                }
                                else
                                {
                                    nPart = objXMLDoc.CreateElement(EstructuraAddendaD.ToString());
                                }

                                AtributoAddenda.LlenaObjeto(IdEstructuraAddenda, ConexionBaseDatos);
                                if (AtributoAddenda.IdAtributoAddenda != 0)
                                {
                                    CSelect ObtenObjeto1 = new CSelect();
                                    ObtenObjeto1.StoredProcedure.CommandText = "sp_Atributo_Addenda_Consulta";
                                    ObtenObjeto1.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                                    ObtenObjeto1.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", IdEstructuraAddenda);
                                    ObtenObjeto1.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), ConexionBaseDatos);
                                    foreach (CAtributoAddenda oAtributoAddenda in ObtenObjeto1.ListaRegistros)
                                    {
                                        for (int e = 0; e < Encabezados.Length; e++)
                                        {
                                            string[] Valores = Encabezados[e].ToString().Split('|');
                                            cadena = oPartidas[Encabezados[e].ToString()].ToString();
                                            if (oAtributoAddenda.AtributoAddenda.ToString() == Valores[0].ToString())
                                            {
                                                if (oAtributoAddenda.AtributoAddenda.ToString() != "IdAtributoAddenda")
                                                {
                                                    nPart.SetAttribute(Valores[0].ToString(), cadena.ToString());
                                                    break;
                                                }
                                            }
                                        }
                                    }

                                }
                                nConceptos.AppendChild(nPart);
                                if (TieneHijos)
                                {

                                    Dictionary<string, object> Parametros = new Dictionary<string, object>();
                                    Parametros.Add("IdPadre", IdEstructuraAddenda);
                                    CEstructuraAddenda EA = new CEstructuraAddenda();
                                    EA.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                                    if (oEstructuraAddenda.Prefijo.ToString() != "")
                                    {
                                        nReferences = objXMLDoc.CreateElement(EA.Prefijo.ToString(), EA.EstructuraAddenda.ToString(), "http://www.sat.gob.mx/cfd/3");
                                    }
                                    else
                                    {
                                        nReferences = objXMLDoc.CreateElement(EA.EstructuraAddenda.ToString());
                                    }

                                    AtributoAddenda.LlenaObjeto(EA.IdEstructuraAddenda, ConexionBaseDatos);

                                    if (AtributoAddenda.IdAtributoAddenda != 0)
                                    {
                                        CSelect ObtenObjeto1 = new CSelect();
                                        ObtenObjeto1.StoredProcedure.CommandText = "sp_Atributo_Addenda_Consulta";
                                        ObtenObjeto1.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                                        ObtenObjeto1.StoredProcedure.Parameters.AddWithValue("@pIdEstructuraAddenda", EA.IdEstructuraAddenda);
                                        ObtenObjeto1.Llena<CAtributoAddenda>(typeof(CAtributoAddenda), ConexionBaseDatos);
                                        foreach (CAtributoAddenda oAtributoAddenda in ObtenObjeto1.ListaRegistros)
                                        {
                                            if (oAtributoAddenda.AtributoReferencia == "")
                                            {
                                                for (int e = 0; e < Encabezados.Length; e++)
                                                {
                                                    string[] Valores = Encabezados[e].ToString().Split('|');
                                                    cadena = oPartidas[Encabezados[e].ToString()].ToString();
                                                    if (oAtributoAddenda.AtributoAddenda.ToString() == Valores[0].ToString())
                                                    {
                                                        if (oAtributoAddenda.AtributoAddenda.ToString() != "IdAtributoAddenda")
                                                        {
                                                            nReferences.SetAttribute(Valores[0].ToString(), cadena.ToString());
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                string[] Valores = oAtributoAddenda.AtributoReferencia.ToString().Split('|');
                                                string Valor = "";
                                                for (int s = 0; s < Valores.Length; s++)
                                                {
                                                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(objXMLDoc.NameTable);
                                                    nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                                    XmlNode nComprobante = objXMLDoc.SelectSingleNode(oAtributoAddenda.NodoABuscar, nsmgr);
                                                    XmlNodeReader nr = new XmlNodeReader(nComprobante);
                                                    JObject Modelo = new JObject();

                                                    if (nr.Read())
                                                    {
                                                        Valor = Valor + nr.GetAttribute(Valores[s].ToString()).ToString();
                                                    }
                                                }

                                                if (Valor.ToString().ToUpper() == "DOLARES" || Valor.ToString().ToUpper() == "DOLAR")
                                                {
                                                    Valor = "USD";
                                                }
                                                else if (Valor.ToString().ToUpper() == "PESOS" || Valor.ToString().ToUpper() == "PESO")
                                                {
                                                    Valor = "MXN";
                                                }

                                                if (oAtributoAddenda.Formato.ToString() != "" && oAtributoAddenda.TipoDato.ToString() != "")
                                                {
                                                    switch (oAtributoAddenda.TipoDato)
                                                    {
                                                        case "Fecha":
                                                            nReferences.SetAttribute(oAtributoAddenda.AtributoAddenda, String.Format(oAtributoAddenda.Formato.ToString(), Convert.ToDateTime(Valor.ToString())));
                                                            break;
                                                        case "Decimal":
                                                            nReferences.SetAttribute(oAtributoAddenda.AtributoAddenda, String.Format(oAtributoAddenda.Formato.ToString(), Convert.ToDecimal(Valor.ToString())));
                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    nReferences.SetAttribute(oAtributoAddenda.AtributoAddenda, Valor.ToString());
                                                }
                                            }
                                        }
                                    }
                                    nPart.PrependChild(nReferences);
                                }
                            }
                            break;
                        case 5:
                            break;
                    }
                }

                objXMLDoc.Save(@"c:\FTG\AMT228OLD.xml");
                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string GuardarFacturaDetalleDescripcionAgregada(int pIdFacturaDetalle, string pDescripcionAgregada)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();

        if(respuesta == "Conexion Establecida"){
            CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
            FacturaDetalle.LlenaObjeto(pIdFacturaDetalle, ConexionBaseDatos);
            FacturaDetalle.DescripcionAgregada = pDescripcionAgregada;
            FacturaDetalle.Editar(ConexionBaseDatos);

            oRespuesta.Add("Error", 0);
        }else{
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string Imprimir(int pIdFacturaEncabezado, string pTemplate)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUtilerias Util = new CUtilerias();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("ImpresionDocumento", pTemplate);

        CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
        ImpresionDocumento.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTempl = new Dictionary<string, object>();
        //ParametrosTempl.Add("IdEmpresa", idEmpresa);
        ParametrosTempl.Add("Baja", 0);
        ParametrosTempl.Add("IdImpresionDocumento", ImpresionDocumento.IdImpresionDocumento);

        CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
        ImpresionTemplate.LlenaObjetoFiltros(ParametrosTempl, ConexionBaseDatos);

        JArray datos = (JArray)CFacturaEncabezado.obtenerDatosImpresionNotaVenta(pIdFacturaEncabezado.ToString(), Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "notaVenta_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
        string rutaTemplate = HttpContext.Current.Server.MapPath("~/Archivos/TemplatesImpresion/" + ImpresionTemplate.RutaTemplate);
        string rutaCSS = HttpContext.Current.Server.MapPath("~/Archivos/TemplatesImpresion/" + ImpresionTemplate.RutaCSS);
        string imagenLogo = HttpContext.Current.Server.MapPath("~/Archivos/EmpresaLogo/") + logoEmpresa;

        if (!File.Exists(rutaTemplate))
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay un template válido para esta empresa."));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo = CFacturaEncabezado.ObtenerFacturaEncabezado(Modelo, pIdFacturaEncabezado, ConexionBaseDatos);
            Modelo.Add(new JProperty("Archivo", Util.ReportePDFTemplate(rutaPDF, rutaTemplate, rutaCSS, imagenLogo, ImpresionTemplate.IdImpresionTemplate, datos, ConexionBaseDatos)));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));

            HttpContext.Current.Application.Set("PDFDescargar", Path.GetFileName(rutaPDF));
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
	public static string ImprimirFacturaEncabezado(int IdFacturaEncabezado)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CFacturaEncabezado Factura = new CFacturaEncabezado();
				Factura.LlenaObjeto(IdFacturaEncabezado, pConexion);

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				CEmpresa Empresa = new CEmpresa();
				Empresa.LlenaObjeto(IdEmpresa, pConexion);

				CMunicipio Municipio = new CMunicipio();
				Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);

				CEstado Estado = new CEstado();
				Estado.LlenaObjeto(Municipio.IdEstado, pConexion);

				Modelo.Add("TIPODOCUMENTO", "Nota de venta");
				Modelo.Add("FOLIO", Factura.NumeroFactura);
				Modelo.Add("RAZONSOCIALEMISOR", Empresa.RazonSocial);
				Modelo.Add("RFCEMISOR", Empresa.RFC);
				Modelo.Add("CALLEEMISOR", Empresa.Calle);
				Modelo.Add("COLONIAEMISOR", Empresa.Colonia);
				Modelo.Add("CODIGOPOSTALEMISOR", Empresa.CodigoPostal);
				Modelo.Add("MUNICIPIOEMISOR", Municipio.Municipio);
				Modelo.Add("ESTADOEMISOR", Estado.Estado);

				CCliente Cliente = new CCliente();
				Cliente.LlenaObjeto(Factura.IdCliente, pConexion);

				COrganizacion Organizacion = new COrganizacion();
				Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

				CContactoOrganizacion Contacto = new CContactoOrganizacion();
				Contacto.LlenaObjeto(Factura.IdContactoCliente, pConexion);

				CTelefonoContactoOrganizacion Telefono = new CTelefonoContactoOrganizacion();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdContactoOrganizacion", Contacto.IdContactoOrganizacion);
				Telefono.LlenaObjetoFiltros(pParametros, pConexion);

				Modelo.Add("RAZONSOCIALRECEPTOR", Factura.RazonSocial);
				Modelo.Add("RFCRECEPTOR", Factura.RFC);
				Modelo.Add("CALLERECEPTOR", Factura.CalleFiscal);
				Modelo.Add("NUMEROEXTERIORRECEPTOR", Factura.NumeroExteriorFiscal);
				Modelo.Add("REFERENCIARECEPTOR", Factura.ReferenciaFiscal);
				Modelo.Add("COLONIARECEPTOR", Factura.ColoniaFiscal);
				Modelo.Add("CODIGOPOSTALRECEPTOR", Factura.CodigoPostalFiscal);
				Modelo.Add("MUNICIPIORECEPTOR", Factura.MunicipioFiscal);
				Modelo.Add("ESTADORECEPTOR", Factura.EstadoFiscal);
				Modelo.Add("PAISRECEPTOR", Factura.PaisFiscal);
				Modelo.Add("TELEFONORECEPTOR", Telefono.Telefono);
				Modelo.Add("CONDICIONPAGO", Factura.CondicionPago);
				Modelo.Add("SUBTOTALFACTURACLIENTE", Factura.Subtotal.ToString("C"));
				Modelo.Add("DESCUENTOFACTURACLIENTE", Factura.Descuento.ToString("C"));
				Modelo.Add("IVAFACTURACLIENTE", Factura.IVA.ToString("C"));
				Modelo.Add("TOTALFACTURACLIENTE", Factura.Total.ToString("C"));
				Modelo.Add("CANTIDADTOTALLETRA", Factura.TotalLetra);
				Modelo.Add("FECHAPAGO", Factura.FechaEmision.ToShortDateString());

				JArray Conceptos = new JArray();
				CFacturaDetalle Detalle = new CFacturaDetalle();
				pParametros.Clear();
				pParametros.Add("IdFacturaEncabezado", Factura.IdFacturaEncabezado);
				pParametros.Add("Baja", 0);

				foreach (CFacturaDetalle Partida in Detalle.LlenaObjetosFiltros(pParametros, pConexion))
				{
					JObject Concepto = new JObject();
					Concepto.Add("CANTIDADDETALLE", Partida.Cantidad);
					Concepto.Add("DESCRIPCIONDETALLE", Partida.Descripcion);
					Concepto.Add("PRECIOUNITARIODETALLE", Partida.PrecioUnitario.ToString("C"));
					Concepto.Add("TOTALDETALLE", Partida.Total.ToString("C"));
					Conceptos.Add(Concepto);
				}
				Modelo.Add("Conceptos", Conceptos);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

    protected void btnDescarga_Click(object sender, EventArgs e)
    {
        string PDFDescarga = HttpContext.Current.Application.Get("PDFDescargar").ToString();

        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + PDFDescarga);
        Response.WriteFile((HttpContext.Current.Server.MapPath("../Archivos/Impresiones/" + PDFDescarga)));
        Response.Flush();
        Response.End();
    }

    private static string ValidarDetalleFactura(CFacturaDetalle pFactura, CConexion pConexion)
    {
        string errores = "";


        //if (pFactura.Cantidad <= 0)
        //{ errores = errores + "<span>*</span> El campo cantidad debe de ser mayor a 0.<br />"; }

        //if (pFactura.Total == 0)
        //{ errores = errores + "<span>*</span> El campo total esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarFacturaSustituye(CFacturaEncabezado pFactura, CConexion pConexion)
    {
        string errores = "";

        if (pFactura.IdSerieFactura == 0)
        { errores = errores + "<span>*</span> No hay serie por asociar, favor de elegir alguno.<br />"; }

        if (pFactura.NumeroFactura == 0)
        { errores = errores + "<span>*</span> El campo número de factura esta vacío, favor de capturarlo.<br />"; }

        if (pFactura.ValidaFactura(pFactura.IdSerieFactura, pFactura.NumeroFactura, pConexion) == 0)
        {
            errores = errores + "<span>*</span> Solo se pueden asignar facturas que existan.<br />";
        }

        //if (pFactura.ValidaExisteFactura(pFactura.IdSerieFactura, pFactura.NumeroFactura, pConexion) == 1)
        //{
        //    errores = errores + "<span>*</span> Esta factura ya esta asignada.<br />";
        //}

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

}