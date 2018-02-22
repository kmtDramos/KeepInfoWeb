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

public partial class NotaCredito : System.Web.UI.Page
{
    public static int puedeAgregarNotaCredito = 0;
    public static int puedeEditarNotaCredito = 0;
    public static int puedeEliminarNotaCredito = 0;
    public static int puedeEliminarCobroNotaCredito = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CEmpresa Empresa = new CEmpresa();

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        puedeAgregarNotaCredito = Usuario.TienePermisos(new string[] { "puedeAgregarNotaCredito" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarNotaCredito = Usuario.TienePermisos(new string[] { "puedeEditarNotaCredito" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarNotaCredito = Usuario.TienePermisos(new string[] { "puedeEliminarNotaCredito" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarCobroNotaCredito = Usuario.TienePermisos(new string[] { "puedeEliminarCobroNotaCredito" }, ConexionBaseDatos) == "" ? 1 : 0;

        //GridNotaCredito
        CJQGrid GridNotaCredito = new CJQGrid();
        GridNotaCredito.NombreTabla = "grdNotaCredito";
        GridNotaCredito.CampoIdentificador = "IdNotaCredito";
        GridNotaCredito.ColumnaOrdenacion = "Descripcion";
        GridNotaCredito.Metodo = "ObtenerNotaCredito";
        GridNotaCredito.GenerarFuncionFiltro = false;
        GridNotaCredito.TituloTabla = "Notas de crédito";

        //IdNotaCredito
        CJQColumn ColIdNotaCredito = new CJQColumn();
        ColIdNotaCredito.Nombre = "IdNotaCredito";
        ColIdNotaCredito.Oculto = "true";
        ColIdNotaCredito.Encabezado = "IdNotaCredito";
        ColIdNotaCredito.Buscador = "false";
        GridNotaCredito.Columnas.Add(ColIdNotaCredito);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "true";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridNotaCredito.Columnas.Add(ColSucursal);

        //SerieNotaCredito
        CJQColumn ColSerieNotaCredito = new CJQColumn();
        ColSerieNotaCredito.Nombre = "SerieNotaCredito";
        ColSerieNotaCredito.Encabezado = "Serie";
        ColSerieNotaCredito.Ancho = "50";
        ColSerieNotaCredito.Buscador = "true";
        ColSerieNotaCredito.Alineacion = "left";
        GridNotaCredito.Columnas.Add(ColSerieNotaCredito);

        //FolioNotaCredito
        CJQColumn ColFolioNotaCredito = new CJQColumn();
        ColFolioNotaCredito.Nombre = "FolioNotaCredito";
        ColFolioNotaCredito.Encabezado = "Folio";
        ColFolioNotaCredito.Ancho = "50";
        ColFolioNotaCredito.Buscador = "true";
        ColFolioNotaCredito.Alineacion = "left";
        GridNotaCredito.Columnas.Add(ColFolioNotaCredito);

        //Importe
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "80";
        GridNotaCredito.Columnas.Add(ColTotal);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "80";
        GridNotaCredito.Columnas.Add(ColTipoMoneda);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "FechaNotaCredito";
        ColFecha.Encabezado = "FechaNotaCredito";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridNotaCredito.Columnas.Add(ColFecha);

        //Asociado
        CJQColumn ColAsociado = new CJQColumn();
        ColAsociado.Nombre = "Asociado";
        ColAsociado.Encabezado = "Asociado";
        ColAsociado.Buscador = "true";
        ColAsociado.Alineacion = "left";
        ColAsociado.Ancho = "40";
        GridNotaCredito.Columnas.Add(ColAsociado);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Ancho = "150";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Buscador = "false";
        GridNotaCredito.Columnas.Add(ColDescripcion);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "150";
        GridNotaCredito.Columnas.Add(ColRazonSocial);

        // Factura
        CJQColumn ColFactura = new CJQColumn();
        ColFactura.Nombre = "Factura";
        ColFactura.Encabezado = "Factura";
        ColFactura.Buscador = "false";
        ColFactura.Alineacion = "left";
        ColFactura.Ancho = "50";
        GridNotaCredito.Columnas.Add(ColFactura);

        //Agente
        CJQColumn ColAgente = new CJQColumn();
        ColAgente.Nombre = "Agente";
        ColAgente.Encabezado = "Agente";
        ColAgente.Buscador = "true";
        ColAgente.Alineacion = "left";
        ColAgente.Ancho = "150";
        GridNotaCredito.Columnas.Add(ColAgente);

        //Timbrado
        CJQColumn Timbrado = new CJQColumn();
        Timbrado.Nombre = "Timbrado";
        Timbrado.Encabezado = "Timbrado";
        Timbrado.Ordenable = "true";
        Timbrado.Ancho = "50";
        Timbrado.Etiquetado = "EstatusTimbradoFile";
        Timbrado.Buscador = "false";
        Timbrado.StoredProcedure.CommandText = "spc_ManejadorConvertirAPedido_Consulta";
        GridNotaCredito.Columnas.Add(Timbrado);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        //ColBaja.Oculto = puedeEliminarNotaCredito == 1 ? "false" : "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarNotaCredito == 1 ? "A/I" : "A/IDeshabilitado";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridNotaCredito.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarNotaCredito";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridNotaCredito.Columnas.Add(ColConsultar);

        //Formato
        CJQColumn ColFormato = new CJQColumn();
        ColFormato.Nombre = "Formato";
        ColFormato.Encabezado = "Imprimir";
        ColFormato.Etiquetado = "Imagen";
        ColFormato.Imagen = "imprimir.png";
        ColFormato.Estilo = "divImagenConsultar imgFormaConsultarFacturaFormato";
        ColFormato.Buscador = "false";
        ColFormato.Ordenable = "false";
        ColFormato.Ancho = "50";
        GridNotaCredito.Columnas.Add(ColFormato);

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
        GridNotaCredito.Columnas.Add(ColXML);

        ClientScript.RegisterStartupScript(this.GetType(), "grdNotaCredito", GridNotaCredito.GeneraGrid(), true);

        //GridFacturas
        CJQGrid GridFacturas = new CJQGrid();
        GridFacturas.NombreTabla = "grdFacturas";
        GridFacturas.CampoIdentificador = "IdEncabezadoFactura";
        GridFacturas.ColumnaOrdenacion = "IdFacturaEncabezado";
        GridFacturas.TipoOrdenacion = "DESC";
        GridFacturas.Metodo = "ObtenerFacturas";
        GridFacturas.TituloTabla = "Facturas";
        GridFacturas.GenerarFuncionFiltro = false;
        GridFacturas.GenerarFuncionTerminado = false;
        GridFacturas.Altura = 120;
        GridFacturas.Editable = true;
        GridFacturas.NumeroRegistros = 15;
        GridFacturas.RangoNumeroRegistros = "15,30,60";

        //IdFactura
        CJQColumn ColIdFactura = new CJQColumn();
        ColIdFactura.Nombre = "IdEncabezadoFactura";
        ColIdFactura.Oculto = "true";
        ColIdFactura.Encabezado = "IdEncabezadoFactura";
        ColIdFactura.Buscador = "false";
        GridFacturas.Columnas.Add(ColIdFactura);

        //Fecha factura
        CJQColumn ColFechaFactura = new CJQColumn();
        ColFechaFactura.Nombre = "Fecha";
        ColFechaFactura.Encabezado = "Fecha";
        ColFechaFactura.Buscador = "false";
        ColFechaFactura.Alineacion = "left";
        ColFechaFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColFechaFactura);

        //Serie factura
        CJQColumn ColSerie = new CJQColumn();
        ColSerie.Nombre = "Serie";
        ColSerie.Encabezado = "Serie";
        ColSerie.Buscador = "true";
        ColSerie.Alineacion = "left";
        ColSerie.Ancho = "80";
        GridFacturas.Columnas.Add(ColSerie);

        //Folio factura
        CJQColumn ColFolioFactura = new CJQColumn();
        ColFolioFactura.Nombre = "Folio";
        ColFolioFactura.Encabezado = "Folio";
        ColFolioFactura.Buscador = "false";
        ColFolioFactura.Alineacion = "left";
        ColFolioFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColFolioFactura);

        //Total factura
        CJQColumn ColTotalFactura = new CJQColumn();
        ColTotalFactura.Nombre = "Total";
        ColTotalFactura.Encabezado = "Total";
        ColTotalFactura.Buscador = "false";
        ColTotalFactura.Formato = "FormatoMoneda";
        ColTotalFactura.Alineacion = "right";
        ColTotalFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColTotalFactura);

        //Tipo moneda
        CJQColumn ColTipoMonedaFactura = new CJQColumn();
        ColTipoMonedaFactura.Nombre = "TipoMoneda";
        ColTipoMonedaFactura.Encabezado = "Tipo de moneda";
        ColTipoMonedaFactura.Buscador = "false";
        ColTipoMonedaFactura.Alineacion = "left";
        ColTipoMonedaFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColTipoMonedaFactura);

        //Tipo cambio
        CJQColumn ColTipoCambio = new CJQColumn();
        ColTipoCambio.Nombre = "TipoCambio";
        ColTipoCambio.Encabezado = "Tipo de cambio";
        ColTipoCambio.Buscador = "false";
        ColTipoCambio.Alineacion = "left";
        ColTipoCambio.Ancho = "80";
        GridFacturas.Columnas.Add(ColTipoCambio);

        //Abono
        CJQColumn ColAbonoFactura = new CJQColumn();
        ColAbonoFactura.Nombre = "Abono";
        ColAbonoFactura.Encabezado = "Introducir abono";
        ColAbonoFactura.Buscador = "false";
        ColAbonoFactura.Editable = "true";
        ColAbonoFactura.Alineacion = "right";
        ColAbonoFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColAbonoFactura);

        //Saldo factura
        CJQColumn ColSaldoFactura = new CJQColumn();
        ColSaldoFactura.Nombre = "Saldo";
        ColSaldoFactura.Encabezado = "Saldo";
        ColSaldoFactura.Buscador = "false";
        ColSaldoFactura.Formato = "FormatoMoneda";
        ColSaldoFactura.Alineacion = "right";
        ColSaldoFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColSaldoFactura);

        //Forma de pago
        CJQColumn ColFormaPago = new CJQColumn();
        ColFormaPago.Nombre = "FormaPago";
        ColFormaPago.Encabezado = "Forma de pago";
        ColFormaPago.Buscador = "false";
        ColFormaPago.Alineacion = "left";
        ColFormaPago.Ancho = "100";
        GridFacturas.Columnas.Add(ColFormaPago);

        //Numero de parcialidades
        CJQColumn ColNumeroParcialidades = new CJQColumn();
        ColNumeroParcialidades.Nombre = "Parcialidades";
        ColNumeroParcialidades.Encabezado = "Parcialidades";
        ColNumeroParcialidades.Buscador = "false";
        ColNumeroParcialidades.Alineacion = "left";
        ColNumeroParcialidades.Ancho = "60";
        GridFacturas.Columnas.Add(ColNumeroParcialidades);

        //Parcialidades
        CJQColumn ColEsParcialidad = new CJQColumn();
        ColEsParcialidad.Nombre = "EsParcialidad";
        ColEsParcialidad.Oculto = "true";
        ColEsParcialidad.Encabezado = "EsParcialidad";
        ColEsParcialidad.Buscador = "false";
        ColEsParcialidad.Ancho = "10";
        GridFacturas.Columnas.Add(ColEsParcialidad);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturas", GridFacturas.GeneraGrid(), true);

        //GridMovimientosCobros
        CJQGrid grdMovimientosCobros = new CJQGrid();
        grdMovimientosCobros.NombreTabla = "grdMovimientosCobros";
        grdMovimientosCobros.CampoIdentificador = "IdNotaCreditoEncabezadoFactura";
        grdMovimientosCobros.ColumnaOrdenacion = "IdNotaCreditoEncabezadoFactura";
        grdMovimientosCobros.TipoOrdenacion = "DESC";
        grdMovimientosCobros.Metodo = "ObtenerMovimientosCobros";
        grdMovimientosCobros.TituloTabla = "Movimientos de cobros";
        grdMovimientosCobros.GenerarFuncionFiltro = false;
        grdMovimientosCobros.GenerarFuncionTerminado = false;
        grdMovimientosCobros.Altura = 120;
        grdMovimientosCobros.NumeroRegistros = 15;
        grdMovimientosCobros.RangoNumeroRegistros = "15,30,60";

        //IdNotaCreditoEncabezadoFactura
        CJQColumn ColIdNotaCreditoEncabezadoFactura = new CJQColumn();
        ColIdNotaCreditoEncabezadoFactura.Nombre = "IdNotaCreditoEncabezadoFactura";
        ColIdNotaCreditoEncabezadoFactura.Oculto = "true";
        ColIdNotaCreditoEncabezadoFactura.Encabezado = "IdNotaCreditoEncabezadoFactura";
        ColIdNotaCreditoEncabezadoFactura.Buscador = "false";
        grdMovimientosCobros.Columnas.Add(ColIdNotaCreditoEncabezadoFactura);

        //Serie
        CJQColumn ColSerieMovimiento = new CJQColumn();
        ColSerieMovimiento.Nombre = "Serie";
        ColSerieMovimiento.Encabezado = "Serie";
        ColSerieMovimiento.Buscador = "false";
        ColSerieMovimiento.Alineacion = "left";
        ColSerieMovimiento.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColSerieMovimiento);

        //Folio
        CJQColumn ColFolioMovimiento = new CJQColumn();
        ColFolioMovimiento.Nombre = "Folio";
        ColFolioMovimiento.Encabezado = "Folio";
        ColFolioMovimiento.Buscador = "false";
        ColFolioMovimiento.Alineacion = "left";
        ColFolioMovimiento.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColFolioMovimiento);

        //Fecha pago
        CJQColumn ColFechaPago = new CJQColumn();
        ColFechaPago.Nombre = "FechaPago";
        ColFechaPago.Encabezado = "FechaPago";
        ColFechaPago.Buscador = "false";
        ColFechaPago.Alineacion = "left";
        ColFechaPago.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColFechaPago);

        //Monto
        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Buscador = "false";
        ColMonto.Formato = "FormatoMoneda";
        ColMonto.Alineacion = "right";
        ColMonto.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColMonto);

        //Eliminar movimiento
        CJQColumn ColEliminarMovimiento = new CJQColumn();
        ColEliminarMovimiento.Nombre = "Eliminar";
        ColEliminarMovimiento.Encabezado = "Eliminar";
        ColEliminarMovimiento.Etiquetado = "Imagen";
        ColEliminarMovimiento.Imagen = "eliminar.png";
        ColEliminarMovimiento.Estilo = "divImagenConsultar imgEliminarMovimiento";
        ColEliminarMovimiento.Buscador = "false";
        ColEliminarMovimiento.Ordenable = "false";
        ColEliminarMovimiento.Oculto = puedeEliminarCobroNotaCredito == 1 ? "false" : "true";
        ColEliminarMovimiento.Ancho = "25";
        grdMovimientosCobros.Columnas.Add(ColEliminarMovimiento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobros", grdMovimientosCobros.GeneraGrid(), true);

        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdNotaCreditoEncabezadoFactura";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdNotaCreditoEncabezadoFactura";
        grdMovimientosCobrosConsultar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosConsultar.Metodo = "ObtenerMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.TituloTabla = "Movimientos de cobros";
        grdMovimientosCobrosConsultar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosConsultar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosConsultar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosConsultar.Altura = 120;
        grdMovimientosCobrosConsultar.Ancho = 670;
        grdMovimientosCobrosConsultar.NumeroRegistros = 15;
        grdMovimientosCobrosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdNotaCreditoEncabezadoFactura
        CJQColumn ColIdNotaCreditoEncabezadoFacturaConsultar = new CJQColumn();
        ColIdNotaCreditoEncabezadoFacturaConsultar.Nombre = "IdNotaCreditoEncabezadoFactura";
        ColIdNotaCreditoEncabezadoFacturaConsultar.Oculto = "true";
        ColIdNotaCreditoEncabezadoFacturaConsultar.Encabezado = "IdNotaCreditoEncabezadoFactura";
        ColIdNotaCreditoEncabezadoFacturaConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdNotaCreditoEncabezadoFacturaConsultar);

        //Serie
        CJQColumn ColSerieMovimientoConsultar = new CJQColumn();
        ColSerieMovimientoConsultar.Nombre = "Serie";
        ColSerieMovimientoConsultar.Encabezado = "Serie";
        ColSerieMovimientoConsultar.Buscador = "false";
        ColSerieMovimientoConsultar.Alineacion = "left";
        ColSerieMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColSerieMovimientoConsultar);

        //Folio
        CJQColumn ColFolioMovimientoConsultar = new CJQColumn();
        ColFolioMovimientoConsultar.Nombre = "Folio";
        ColFolioMovimientoConsultar.Encabezado = "Folio";
        ColFolioMovimientoConsultar.Buscador = "false";
        ColFolioMovimientoConsultar.Alineacion = "left";
        ColFolioMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFolioMovimientoConsultar);

        //Fecha pago
        CJQColumn ColFechaPagoConsultar = new CJQColumn();
        ColFechaPagoConsultar.Nombre = "FechaPago";
        ColFechaPagoConsultar.Encabezado = "FechaPago";
        ColFechaPagoConsultar.Buscador = "false";
        ColFechaPagoConsultar.Alineacion = "left";
        ColFechaPagoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFechaPagoConsultar);

        //Monto
        CJQColumn ColMontoConsultar = new CJQColumn();
        ColMontoConsultar.Nombre = "Monto";
        ColMontoConsultar.Encabezado = "Monto";
        ColMontoConsultar.Buscador = "false";
        ColMontoConsultar.Formato = "FormatoMoneda";
        ColMontoConsultar.Alineacion = "right";
        ColMontoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColMontoConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobrosConsultar", grdMovimientosCobrosConsultar.GeneraGrid(), true);

        //GridMovimientosCobrosEditar
        CJQGrid grdMovimientosCobrosEditar = new CJQGrid();
        grdMovimientosCobrosEditar.NombreTabla = "grdMovimientosCobrosEditar";
        grdMovimientosCobrosEditar.CampoIdentificador = "IdNotaCreditoEncabezadoFactura";
        grdMovimientosCobrosEditar.ColumnaOrdenacion = "IdNotaCreditoEncabezadoFactura";
        grdMovimientosCobrosEditar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosEditar.Metodo = "ObtenerMovimientosCobrosEditar";
        grdMovimientosCobrosEditar.TituloTabla = "Movimientos de cobros";
        grdMovimientosCobrosEditar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosEditar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosEditar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosEditar.Altura = 120;
        grdMovimientosCobrosEditar.Ancho = 670;
        grdMovimientosCobrosEditar.NumeroRegistros = 15;
        grdMovimientosCobrosEditar.RangoNumeroRegistros = "15,30,60";

        //IdNotaCreditoEncabezadoFactura
        CJQColumn ColIdNotaCreditoEncabezadoFacturaEditar = new CJQColumn();
        ColIdNotaCreditoEncabezadoFacturaEditar.Nombre = "IdNotaCreditoEncabezadoFactura";
        ColIdNotaCreditoEncabezadoFacturaEditar.Oculto = "true";
        ColIdNotaCreditoEncabezadoFacturaEditar.Encabezado = "IdNotaCreditoEncabezadoFactura";
        ColIdNotaCreditoEncabezadoFacturaEditar.Buscador = "false";
        grdMovimientosCobrosEditar.Columnas.Add(ColIdNotaCreditoEncabezadoFacturaEditar);

        //Serie
        CJQColumn ColSerieMovimientoEditar = new CJQColumn();
        ColSerieMovimientoEditar.Nombre = "Serie";
        ColSerieMovimientoEditar.Encabezado = "Serie";
        ColSerieMovimientoEditar.Buscador = "false";
        ColSerieMovimientoEditar.Alineacion = "left";
        ColSerieMovimientoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColSerieMovimientoEditar);

        //Folio
        CJQColumn ColFolioMovimientoEditar = new CJQColumn();
        ColFolioMovimientoEditar.Nombre = "Folio";
        ColFolioMovimientoEditar.Encabezado = "Folio";
        ColFolioMovimientoEditar.Buscador = "false";
        ColFolioMovimientoEditar.Alineacion = "left";
        ColFolioMovimientoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColFolioMovimientoEditar);

        //Fecha pago
        CJQColumn ColFechaPagoEditar = new CJQColumn();
        ColFechaPagoEditar.Nombre = "FechaPago";
        ColFechaPagoEditar.Encabezado = "FechaPago";
        ColFechaPagoEditar.Buscador = "false";
        ColFechaPagoEditar.Alineacion = "left";
        ColFechaPagoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColFechaPagoEditar);

        //Monto
        CJQColumn ColMontoEditar = new CJQColumn();
        ColMontoEditar.Nombre = "Monto";
        ColMontoEditar.Encabezado = "Monto";
        ColMontoEditar.Buscador = "false";
        ColMontoEditar.Formato = "FormatoMoneda";
        ColMontoEditar.Alineacion = "right";
        ColMontoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColMontoEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobrosEditar", grdMovimientosCobrosEditar.GeneraGrid(), true);
        CrearGridFacturasRegistradas();
        CrearGridProductosAsociados();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }


    private void CrearGridFacturasRegistradas()
    {
        CJQGrid GridProductosNotaCreditoDevolucionCancelacion = new CJQGrid();
        GridProductosNotaCreditoDevolucionCancelacion.NombreTabla = "grdProductosNotaCreditoDevolucionCancelacion";
        GridProductosNotaCreditoDevolucionCancelacion.CampoIdentificador = "IdFacturaDetalle";
        GridProductosNotaCreditoDevolucionCancelacion.ColumnaOrdenacion = "IdFacturaDetalle";
        GridProductosNotaCreditoDevolucionCancelacion.TipoOrdenacion = "DESC";
        GridProductosNotaCreditoDevolucionCancelacion.Metodo = "ObtenerProductosNotaCreditoDevolucionCancelacion";
        GridProductosNotaCreditoDevolucionCancelacion.TituloTabla = "Facturas pendientes";
        GridProductosNotaCreditoDevolucionCancelacion.Altura = 290;
        GridProductosNotaCreditoDevolucionCancelacion.GenerarFuncionFiltro = false;
        GridProductosNotaCreditoDevolucionCancelacion.GenerarFuncionTerminado = true;

        //IdFacturaDetalle
        CJQColumn ColIdFacturaDetalle = new CJQColumn();
        ColIdFacturaDetalle.Nombre = "IdFacturaDetalle";
        ColIdFacturaDetalle.Oculto = "true";
        ColIdFacturaDetalle.Encabezado = "IdFacturaDetalle";
        ColIdFacturaDetalle.Buscador = "false";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColIdFacturaDetalle);

        //Factura
        CJQColumn ColFactura = new CJQColumn();
        ColFactura.Nombre = "Factura";
        ColFactura.Oculto = "false";
        ColFactura.Encabezado = "Factura";
        ColFactura.Buscador = "false";
        ColFactura.Ancho = "80";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColFactura);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Descripcion";
        ColProducto.Encabezado = "Producto";
        ColProducto.Buscador = "true";
        ColProducto.Alineacion = "left";
        ColProducto.Ancho = "500";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColProducto);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "true";
        ColCantidad.Alineacion = "center";
        ColCantidad.Ancho = "70";
        ColCantidad.Buscador = "false";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColCantidad);

        //Disponible
        CJQColumn ColDisponible = new CJQColumn();
        ColDisponible.Nombre = "Disponible";
        ColDisponible.Encabezado = "Disponible";
        ColDisponible.Buscador = "true";
        ColDisponible.Alineacion = "center";
        ColDisponible.Ancho = "90";
        ColDisponible.Buscador = "false";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColDisponible);

        //TipoMoneda
        CJQColumn ColTipoMonedaProducto = new CJQColumn();
        ColTipoMonedaProducto.Nombre = "TipoMoneda";
        ColTipoMonedaProducto.Encabezado = "Moneda";
        ColTipoMonedaProducto.Buscador = "false";
        ColTipoMonedaProducto.Alineacion = "center";
        ColTipoMonedaProducto.Ancho = "85";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColTipoMonedaProducto);

        //PrecioUnitario
        CJQColumn ColPrecioUnitario = new CJQColumn();
        ColPrecioUnitario.Nombre = "PrecioUnitario";
        ColPrecioUnitario.Encabezado = "Precio unitario";
        ColPrecioUnitario.Buscador = "false";
        ColPrecioUnitario.Alineacion = "right";
        ColPrecioUnitario.Ancho = "105";
        ColPrecioUnitario.Formato = "FormatoMoneda";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColPrecioUnitario);

        //PrecioUnitarioIVA
        CJQColumn ColPrecioIVA = new CJQColumn();
        ColPrecioIVA.Nombre = "PrecioUnitarioIVA";
        ColPrecioIVA.Encabezado = "Total IVA";
        ColPrecioIVA.Buscador = "false";
        ColPrecioIVA.Alineacion = "right";
        ColPrecioIVA.Ancho = "125";
        ColPrecioIVA.Formato = "FormatoMoneda";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColPrecioIVA);


        //SeleccionarVarios
        CJQColumn ColSeleccionarVarios = new CJQColumn();
        ColSeleccionarVarios.Nombre = "Sel";
        ColSeleccionarVarios.Encabezado = "Sel.";
        ColSeleccionarVarios.Buscador = "false";
        ColSeleccionarVarios.Ordenable = "false";
        ColSeleccionarVarios.Ancho = "50";
        ColSeleccionarVarios.Etiquetado = "CheckBox";
        ColSeleccionarVarios.Estilo = "checkAsignarVarios";
        GridProductosNotaCreditoDevolucionCancelacion.Columnas.Add(ColSeleccionarVarios);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProductosNotaCreditoDevolucionCancelacion", GridProductosNotaCreditoDevolucionCancelacion.GeneraGrid(), true);
    }

    private void CrearGridProductosAsociados()
    {
        CJQGrid GridProductosAsociados = new CJQGrid();
        GridProductosAsociados.NombreTabla = "grdProductosAsociadosNotaCreditoDevolucionCancelacion";
        GridProductosAsociados.CampoIdentificador = "IdDevolucion";
        GridProductosAsociados.ColumnaOrdenacion = "IdDevolucion";
        GridProductosAsociados.TipoOrdenacion = "DESC";
        GridProductosAsociados.Metodo = "ObtenerProductosAsociadosNotaCreditoDevolucionCancelacion";
        GridProductosAsociados.TituloTabla = "Productos asociados";
        GridProductosAsociados.Altura = 290;
        GridProductosAsociados.GenerarFuncionFiltro = false;
        GridProductosAsociados.GenerarFuncionTerminado = true;

        //IdDevolucion
        CJQColumn ColIdDevolucion = new CJQColumn();
        ColIdDevolucion.Nombre = "IdDevolucion";
        ColIdDevolucion.Oculto = "true";
        ColIdDevolucion.Encabezado = "IdDevolucion";
        ColIdDevolucion.Buscador = "false";
        GridProductosAsociados.Columnas.Add(ColIdDevolucion);

        //Factura
        CJQColumn ColFactura = new CJQColumn();
        ColFactura.Nombre = "Factura";
        ColFactura.Oculto = "false";
        ColFactura.Encabezado = "Factura";
        ColFactura.Buscador = "false";
        GridProductosAsociados.Columnas.Add(ColFactura);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "DescripcionProducto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Buscador = "true";
        ColProducto.Alineacion = "left";
        ColProducto.Ancho = "550";
        GridProductosAsociados.Columnas.Add(ColProducto);

        //PrecioUnitario
        CJQColumn ColPrecioUnitario = new CJQColumn();
        ColPrecioUnitario.Nombre = "PrecioUnitario";
        ColPrecioUnitario.Encabezado = "Precio unitario";
        ColPrecioUnitario.Buscador = "false";
        ColPrecioUnitario.Alineacion = "right";
        ColPrecioUnitario.Ancho = "100";
        ColPrecioUnitario.Formato = "FormatoMoneda";
        GridProductosAsociados.Columnas.Add(ColPrecioUnitario);

        //PrecioUnitarioIVA
        CJQColumn ColPrecioIVA = new CJQColumn();
        ColPrecioIVA.Nombre = "PrecioUnitarioIVA";
        ColPrecioIVA.Encabezado = "Total";
        ColPrecioIVA.Buscador = "false";
        ColPrecioIVA.Alineacion = "right";
        ColPrecioIVA.Ancho = "125";
        ColPrecioIVA.Formato = "FormatoMoneda";
        GridProductosAsociados.Columnas.Add(ColPrecioIVA);


        //SeleccionarVarios
        CJQColumn ColSeleccionarVarios = new CJQColumn();
        ColSeleccionarVarios.Nombre = "SelProductos";
        ColSeleccionarVarios.Encabezado = "Sel.";
        ColSeleccionarVarios.Buscador = "false";
        ColSeleccionarVarios.Ordenable = "false";
        ColSeleccionarVarios.Ancho = "50";
        ColSeleccionarVarios.Etiquetado = "CheckBox";
        ColSeleccionarVarios.Estilo = "checkAsignarVarios";
        GridProductosAsociados.Columnas.Add(ColSeleccionarVarios);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProductosAsociadosNotaCreditoDevolucionCancelacion", GridProductosAsociados.GeneraGrid(), true);
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerNotaCredito(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerieNotaCredito, string pFolioNotaCredito, string pFechaInicial, string pFechaFinal, int pPorFecha, int pAI, int pFiltroTimbrado, string pRazonSocial, string pAsociado, int pIdPedido, int pIdFactura, int pIdOrdenCompra)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCredito", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieNotaCredito);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolioNotaCredito);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pFiltroTimbrado", SqlDbType.Int).Value = pFiltroTimbrado;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pAsociado", SqlDbType.VarChar, 250).Value = Convert.ToString(pAsociado);
        Stored.Parameters.Add("pIdPedido", SqlDbType.Int).Value = pIdPedido;
        Stored.Parameters.Add("pIdFactura", SqlDbType.Int).Value = pIdFactura;
        Stored.Parameters.Add("pIdOrdenCompra", SqlDbType.Int).Value = pIdOrdenCompra;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerie, int pIdCliente)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerie);
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobros(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCredito)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCredito", SqlDbType.Int).Value = pIdNotaCredito;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCredito)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCredito", SqlDbType.Int).Value = pIdNotaCredito;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCredito)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCredito", SqlDbType.Int).Value = pIdNotaCredito;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProductosNotaCreditoDevolucionCancelacion(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCredito, int pIdTipoNotaCredito, int pIdCliente, int pIdTipoMoneda, decimal pTipoCambio, string pDescripcion)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_grdNotaCreditoAsociarProductosDevolucionCancelacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCredito", SqlDbType.Int).Value = pIdNotaCredito;
        Stored.Parameters.Add("pIdTipoNotaCredito", SqlDbType.Int).Value = pIdTipoNotaCredito;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;
        Stored.Parameters.Add("pTipoCambio", SqlDbType.Decimal).Value = pTipoCambio;
        Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 200).Value = pDescripcion;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProductosAsociadosNotaCreditoDevolucionCancelacion(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCredito, int pIdTipoNotaCredito, int pIdCliente, string pDescripcion)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_grdNotaCreditoProductosDevolucion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCredito", SqlDbType.Int).Value = pIdNotaCredito;
        Stored.Parameters.Add("pIdTipoNotaCredito", SqlDbType.Int).Value = pIdTipoNotaCredito;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 200).Value = pDescripcion;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);   
    }

    [WebMethod]
    public static string TimbrarNotaCredito(Dictionary<string, object> pNotaCredito)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();
            string validacion = CrearArchivoBuzonFiscal(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), pNotaCredito["LugarExpedicion"].ToString(), pNotaCredito["MetodoPago"].ToString(), pNotaCredito["FormaPago"].ToString(), pNotaCredito["CondicionPago"].ToString(), pNotaCredito["CuentaBancariaCliente"].ToString(), pNotaCredito["Referencia"].ToString(), pNotaCredito["Observaciones"].ToString(), ConexionBaseDatos);
            if (validacion == "1")
            {
                CUtilerias Utilerias = new CUtilerias();
                Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                validacion = BuzonFiscalTimbrado(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
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
    public static string CancelarNotaCredito(Dictionary<string, object> pNotaCredito)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);
        CNotaCredito NotaCredito = new CNotaCredito();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            if (NotaCredito.ExisteNotaCreditoMovimientos(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), ConexionBaseDatos) == 0)
            {
                if (NotaCredito.ExisteNotaCreditoTimbrada(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), ConexionBaseDatos) == 1)
                {
                    string validacion = CancelarArchivoBuzonFiscal(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), ConexionBaseDatos);
                    if (validacion == "1")
                    {
                        CUtilerias Utilerias = new CUtilerias();
                        Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                        validacion = BuzonFiscalTimbradoCancelacion(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), pNotaCredito["MotivoCancelacion"].ToString(), ConexionBaseDatos);
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
                    oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta nota de crédito, ya que no esta timbrada"));

                }
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta nota de crédito, porque tiene movimientos de cobros asociados a facturas"));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarNotaCredito(Dictionary<string, object> pNotaCredito)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CNotaCredito NotaCredito = new CNotaCredito();
            NotaCredito.IdCliente = Convert.ToInt32(pNotaCredito["IdCliente"]);
            NotaCredito.SerieNotaCredito = Convert.ToString(pNotaCredito["SerieNotaCredito"]);
            NotaCredito.FolioNotaCredito = Convert.ToInt32(pNotaCredito["FolioNotaCredito"]);
            NotaCredito.Descripcion = Convert.ToString(pNotaCredito["Descripcion"]);
            NotaCredito.Fecha = Convert.ToDateTime(pNotaCredito["Fecha"]);
            NotaCredito.IdTipoMoneda = Convert.ToInt32(pNotaCredito["IdTipoMoneda"]);
            NotaCredito.TipoCambio = Convert.ToDecimal(pNotaCredito["TipoCambio"]);
            NotaCredito.Referencia = Convert.ToString(pNotaCredito["Referencia"]);
            NotaCredito.Monto = Convert.ToDecimal(pNotaCredito["Monto"]);
            NotaCredito.PorcentajeIVA = Convert.ToDecimal(pNotaCredito["PorcentajeIVA"]);
            NotaCredito.IVA = Convert.ToDecimal(pNotaCredito["IVA"]);
            NotaCredito.Total = Convert.ToDecimal(pNotaCredito["Total"]);
            NotaCredito.SaldoDocumento = Convert.ToDecimal(pNotaCredito["Total"]);
            NotaCredito.FechaAlta = Convert.ToDateTime(DateTime.Now);
            NotaCredito.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            NotaCredito.IdTipoNotaCredito = Convert.ToInt32(pNotaCredito["IdTipoNotaCredito"]);

            string validacion = ValidarNotaCredito(NotaCredito, ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                NotaCredito.AgregarNotaCredito(ConexionBaseDatos);

                CNotaCreditoSucursal NotaCreditoSucursal = new CNotaCreditoSucursal();
                NotaCreditoSucursal.IdNotaCredito = NotaCredito.IdNotaCredito;
                NotaCreditoSucursal.IdSucursal = Usuario.IdSucursalActual;
                NotaCreditoSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                NotaCreditoSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                NotaCreditoSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = NotaCredito.IdNotaCredito;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nota de credito";
                HistorialGenerico.AgregarHistorialGenerico("NotaCredito", ConexionBaseDatos);

                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CNotaCredito NotaCreditoTotal = new CNotaCredito();
                NotaCreditoTotal.LlenaObjeto(Convert.ToInt32(NotaCredito.IdNotaCredito), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCreditoTotal.IdTipoMoneda, ConexionBaseDatos);
                
                string nomenclatura = "";
                if (NotaCreditoTotal.IdTipoMoneda == 1)
                {
                    nomenclatura = "M.N.";
                }
                else
                {
                    nomenclatura = "USD";
                }
                
                TotalLetras = Utilerias.ConvertLetter(NotaCreditoTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString())+nomenclatura;
                NotaCreditoTotal.TotalLetra = TotalLetras;
                NotaCreditoTotal.Editar(ConexionBaseDatos);

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
    public static string AgregarNotaCreditoEdicion(Dictionary<string, object> pNotaCredito)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CNotaCredito NotaCredito = new CNotaCredito();
            NotaCredito.IdCliente = Convert.ToInt32(pNotaCredito["IdCliente"]);
            NotaCredito.SerieNotaCredito = Convert.ToString(pNotaCredito["SerieNotaCredito"]);
            NotaCredito.FolioNotaCredito = Convert.ToInt32(pNotaCredito["FolioNotaCredito"]);
            NotaCredito.Descripcion = Convert.ToString(pNotaCredito["Descripcion"]);
            NotaCredito.Fecha = Convert.ToDateTime(pNotaCredito["Fecha"]);
            NotaCredito.IdTipoMoneda = Convert.ToInt32(pNotaCredito["IdTipoMoneda"]);
            NotaCredito.TipoCambio = Convert.ToDecimal(pNotaCredito["TipoCambio"]);
            NotaCredito.Referencia = Convert.ToString(pNotaCredito["Referencia"]);
            NotaCredito.Monto = Convert.ToDecimal(pNotaCredito["Monto"]);
            NotaCredito.PorcentajeIVA = Convert.ToDecimal(pNotaCredito["PorcentajeIVA"]);
            NotaCredito.IVA = Convert.ToDecimal(pNotaCredito["IVA"]);
            NotaCredito.Total = Convert.ToDecimal(pNotaCredito["Total"]);
            NotaCredito.SaldoDocumento = Convert.ToDecimal(pNotaCredito["Total"]);
            NotaCredito.FechaAlta = Convert.ToDateTime(DateTime.Now);
            NotaCredito.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            NotaCredito.IdTipoNotaCredito = Convert.ToInt32(pNotaCredito["IdTipoNotaCredito"]);

            string validacion = "";
            if (Convert.ToInt32(pNotaCredito["IdTipoNotaCredito"]) == 1)
            {
                ValidarNotaCreditoDevolucionCancelacion(NotaCredito, ConexionBaseDatos);
            }
            else
            {
                validacion = ValidarNotaCredito(NotaCredito, ConexionBaseDatos);
            }

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                NotaCredito.AgregarNotaCredito(ConexionBaseDatos);
                int IDNotaCredito = NotaCredito.IdNotaCredito;

                if (Convert.ToInt32(pNotaCredito["IdTipoNotaCredito"]) == 1)
                {
                    int TotalNotaCreditoTimbrada = NotaCredito.ObtieneNotaTimbrada(Convert.ToInt32(IDNotaCredito), ConexionBaseDatos);
                    oRespuesta.Add(new JProperty("TotalNotaCreditoTimbrada", TotalNotaCreditoTimbrada));
                }
                else
                {
                    oRespuesta.Add(new JProperty("TotalNotaCreditoTimbrada", 0));
                }

                CNotaCreditoSucursal NotaCreditoSucursal = new CNotaCreditoSucursal();
                NotaCreditoSucursal.IdNotaCredito = NotaCredito.IdNotaCredito;
                NotaCreditoSucursal.IdSucursal = Usuario.IdSucursalActual;
                NotaCreditoSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                NotaCreditoSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                NotaCreditoSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = NotaCredito.IdNotaCredito;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nota de credito";
                HistorialGenerico.AgregarHistorialGenerico("NotaCredito", ConexionBaseDatos);
                oRespuesta.Add("IdNotaCredito", NotaCredito.IdNotaCredito);

                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CNotaCredito NotaCreditoTotal = new CNotaCredito();

                string nomenclatura = "";
                if (Convert.ToInt32(pNotaCredito["IdTipoMoneda"]) == 1)
                {
                    nomenclatura = " M.N.";
                }
                else
                {
                    nomenclatura = " USD";
                }

                NotaCreditoTotal.LlenaObjeto(Convert.ToInt32(NotaCredito.IdNotaCredito), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCreditoTotal.IdTipoMoneda, ConexionBaseDatos);
                TotalLetras = Utilerias.ConvertLetter(NotaCreditoTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString()) + nomenclatura;
                NotaCreditoTotal.TotalLetra = TotalLetras;
                NotaCreditoTotal.Editar(ConexionBaseDatos);


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
    public static string ObtenerFormaFiltroNotaCredito()
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
    public static string ObtieneFacturaFormato(int pIdNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CRutaCFDI RutaCFDIF = new CRutaCFDI();
        CNotaCredito NotaCredito = new CNotaCredito();
        string NombreArchivo = "";
        string Ruta = "";
        string RutaF = "";
        NotaCredito.LlenaObjeto(pIdNotaCredito, ConexionBaseDatos);
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
        ParametrosTS.Add("Baja", Convert.ToInt32(0));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

        ParametrosTS.Clear();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        ParametrosTS.Add("Baja", Convert.ToInt32(0));
        RutaCFDIF.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

        NombreArchivo = NotaCredito.SerieNotaCredito + NotaCredito.FolioNotaCredito;

        CCliente cliente = new CCliente();
        cliente.LlenaObjeto(NotaCredito.IdCliente, ConexionBaseDatos);
        COrganizacion organizacion = new COrganizacion();
        organizacion.LlenaObjeto(cliente.IdOrganizacion,ConexionBaseDatos);

        

        RutaF = RutaCFDIF.RutaCFDI + "\\NotaCredito\\out\\" + organizacion.RFC + "\\" + NombreArchivo + ".pdf";
        if (File.Exists(RutaF))
        {
            Ruta = RutaCFDI.RutaCFDI + "/NotaCredito/out/" + organizacion.RFC + "/" + NombreArchivo + ".pdf";
        }
        else
        {
            ParametrosTS.Clear();
            ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
            ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
            ParametrosTS.Add("Baja", Convert.ToInt32(1));
            RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

            Ruta = RutaCFDI.RutaCFDI + "/out/" + NombreArchivo + ".pdf";
        }

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
    public static string ObtieneFacturaXML(int pIdNotaCredito)
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
            CRutaCFDI RutaCFDIF = new CRutaCFDI();
            CSerieFactura SerieFactura = new CSerieFactura();
            CNotaCredito NotaCredito = new CNotaCredito();
            string NombreArchivo = "";
            string Ruta = "";
            string RutaF = "";

            NotaCredito.LlenaObjeto(pIdNotaCredito, ConexionBaseDatos);
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
            ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
            ParametrosTS.Add("Baja", Convert.ToInt32(0));
            RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

            ParametrosTS.Clear();
            ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
            ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
            ParametrosTS.Add("Baja", Convert.ToInt32(0));
            RutaCFDIF.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

            NombreArchivo = NotaCredito.SerieNotaCredito + NotaCredito.FolioNotaCredito;
            //Ruta = RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".xml";

            CCliente cliente = new CCliente();
            cliente.LlenaObjeto(NotaCredito.IdCliente, ConexionBaseDatos);
            COrganizacion organizacion = new COrganizacion();
            organizacion.LlenaObjeto(cliente.IdOrganizacion, ConexionBaseDatos);
            Ruta = RutaCFDI.RutaCFDI + "/NotaCredito/out/" + organizacion.RFC + "/" + NombreArchivo + ".xml";
            RutaF = RutaCFDIF.RutaCFDI + "\\NotaCredito\\out\\" + organizacion.RFC + "\\" + NombreArchivo + ".xml";

            if (File.Exists(RutaF))
            {
                Respuesta.Add("Error", 0);
                Respuesta.Add("xml", Ruta);
                Respuesta.Add("name", NombreArchivo);

            }
            else
            {
                Respuesta.Add("Error", 1);
                Respuesta.Add("Descripcion", "No se encontro el XML");
            }

        }
        else
        {
            Respuesta.Add("Error", 1);
            Respuesta.Add("Descripcion", respuesta);
        }

        return Respuesta.ToString();

    }


    [WebMethod]
    public static string ObtenerFormaAsociarDocumentos(Dictionary<string, object> NotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCredito = 1;
        }
        oPermisos.Add("puedeEditarNotaCredito", puedeEditarNotaCredito);

        if (respuesta == "Conexion Establecida")
        {
            CNotaCredito NotaCreditoCliente = new CNotaCredito();
            NotaCreditoCliente.LlenaObjeto(Convert.ToInt32(NotaCredito["pIdNotaCredito"]), ConexionBaseDatos);
            NotaCreditoCliente.IdCliente = Convert.ToInt32(NotaCredito["pIdCliente"]);
            NotaCreditoCliente.Editar(ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CNotaCredito.ObtenerNotaCreditoAsociarDocumentos(Modelo, Convert.ToInt32(NotaCredito["pIdNotaCredito"]), ConexionBaseDatos);


            if (Convert.ToInt32(NotaCredito["IdTipoNotaCredito"]) == 1)
            {
                int TotalNotaCreditoTimbrada = NotaCreditoCliente.ObtieneNotaTimbrada(Convert.ToInt32(NotaCredito["pIdNotaCredito"]), ConexionBaseDatos);
                oPermisos.Add(new JProperty("TotalNotaCreditoTimbrada", TotalNotaCreditoTimbrada));
            }
            else
            {
                oPermisos.Add(new JProperty("TotalNotaCreditoTimbrada", 0));
            }
            oPermisos.Add(new JProperty("IdTipoNotaCredito", Convert.ToInt32(NotaCredito["IdTipoNotaCredito"])));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No ObtenerFormaAsociarDocumentos conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAsociarProductosDevolucionCancelacion(Dictionary<string, object> NotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCredito = 1;
        }
        oPermisos.Add("puedeEditarNotaCredito", puedeEditarNotaCredito);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("IdCliente", Convert.ToInt32(NotaCredito["pIdNotaCredito"]));
            Modelo.Add("IdNotaCredito", Convert.ToInt32(NotaCredito["pIdCliente"]));
            Modelo.Add("IdTipoNotaCredito", Convert.ToInt32(NotaCredito["pIdTipoNotaCredito"]));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
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
    public static string ObtenerFormaNotaCredito(int pIdNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCredito = 1;
        }
        oPermisos.Add("puedeEditarNotaCredito", puedeEditarNotaCredito);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CNotaCredito.ObtenerNotaCredito(Modelo, pIdNotaCredito, ConexionBaseDatos);

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

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarNotaCredito(int pIdTipoNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("SerieNotaCreditos", CJson.ObtenerJsonSerieNotaCredito(Usuario.IdSucursalActual, ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add("PorcentajeIVA", Sucursal.IVAActual);
            Modelo.Add("Fecha", DateTime.Today.ToShortDateString());
            Modelo.Add("IdTipoNotaCredito", pIdTipoNotaCredito);
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

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarNotaCreditoDevolucionCancelacion(int pIdTipoNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("SerieNotaCreditos", CJson.ObtenerJsonSerieNotaCredito(Usuario.IdSucursalActual, ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add("PorcentajeIVA", Sucursal.IVAActual);
            Modelo.Add("Fecha", DateTime.Today.ToShortDateString());
            Modelo.Add("IdTipoNotaCredito", pIdTipoNotaCredito);
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
    public static string ObtenerNumeroNotaCredito(Dictionary<string, object> pNotaCredito)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSerieNotaCredito SerieNotaCredito = new CSerieNotaCredito();
            SerieNotaCredito.LlenaObjeto(Convert.ToInt32(pNotaCredito["IdSerieNotaCredito"]), ConexionBaseDatos);
            CNotaCredito NotaCredito = new CNotaCredito();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            int NumeroNotaCredito = 0;
            NumeroNotaCredito = NotaCredito.ObtieneNumeroNotaCredito(SerieNotaCredito.SerieNotaCredito, Usuario.IdSucursalActual, ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("NumeroNotaCredito", NumeroNotaCredito);
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
    public static string ObtenerFormaEditarNotaCredito(int IdNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCredito = 1;
        }
        oPermisos.Add("puedeEditarNotaCredito", puedeEditarNotaCredito);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CNotaCredito.ObtenerNotaCredito(Modelo, IdNotaCredito, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));

            string Baja = Convert.ToString(Modelo["Baja"]);
            if (Baja == "true" || Baja == "True")
            {
                Modelo.Add("EstatusNota", 1);
            }
            else
            {
                Modelo.Add("EstatusNota", 0);
            }

            CNotaCredito NC = new CNotaCredito();
            NC.LlenaObjeto(IdNotaCredito, ConexionBaseDatos);
            int pIdTipoNotaCredito = NC.IdTipoNotaCredito;
            Modelo.Add("IdTipoNotaCredito", pIdTipoNotaCredito);
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
    public static string EditarNotaCredito(Dictionary<string, object> pNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CNotaCredito NotaCredito = new CNotaCredito();
        NotaCredito.LlenaObjeto(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), ConexionBaseDatos);
        NotaCredito.IdNotaCredito = Convert.ToInt32(pNotaCredito["IdNotaCredito"]);
        NotaCredito.IdCliente = Convert.ToInt32(pNotaCredito["IdCliente"]);
        NotaCredito.SerieNotaCredito = Convert.ToString(pNotaCredito["SerieNotaCredito"]);
        NotaCredito.FolioNotaCredito = Convert.ToInt32(pNotaCredito["FolioNotaCredito"]);
        NotaCredito.Descripcion = Convert.ToString(pNotaCredito["Descripcion"]);
        NotaCredito.Fecha = Convert.ToDateTime(pNotaCredito["Fecha"]);
        NotaCredito.IdTipoMoneda = Convert.ToInt32(pNotaCredito["IdTipoMoneda"]);
        NotaCredito.TipoCambio = Convert.ToDecimal(pNotaCredito["TipoCambio"]);
        NotaCredito.Referencia = Convert.ToString(pNotaCredito["Referencia"]);
        NotaCredito.Monto = Convert.ToDecimal(pNotaCredito["Monto"]);
        NotaCredito.PorcentajeIVA = Convert.ToDecimal(pNotaCredito["PorcentajeIVA"]);
        NotaCredito.IVA = Convert.ToDecimal(pNotaCredito["IVA"]);
        NotaCredito.Total = Convert.ToDecimal(pNotaCredito["Total"]);
        NotaCredito.SaldoDocumento = Convert.ToDecimal(pNotaCredito["Total"]);
        NotaCredito.IdTipoNotaCredito = Convert.ToInt32(pNotaCredito["IdTipoNotaCredito"]);

        string validacion = ValidarNotaCredito(NotaCredito, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            NotaCredito.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string LlenaDatosFiscales(int IdNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CNotaCredito NotaCredito = new CNotaCredito();
        CSucursal Sucursal = new CSucursal();
        CEmpresa Empresa = new CEmpresa();
        CLocalidad Localidad = new CLocalidad();
        CMunicipio Municipio = new CMunicipio();
        CEstado Estado = new CEstado();
        CPais Pais = new CPais();
        CCliente Cliente = new CCliente();
        NotaCredito.LlenaObjeto(IdNotaCredito, ConexionBaseDatos);
        Cliente.LlenaObjeto(NotaCredito.IdCliente, ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCredito = 1;
        }
        oPermisos.Add("puedeEditarNotaCredito", puedeEditarNotaCredito);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CNotaCredito.ObtenerNotaCredito(Modelo, IdNotaCredito, ConexionBaseDatos);
            Usuario.LlenaObjeto(Convert.ToInt32(NotaCredito.IdUsuarioAlta), ConexionBaseDatos);
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);
            if (Sucursal.DireccionFiscal == true)
            {
                Localidad.LlenaObjeto(Empresa.IdLocalidad, ConexionBaseDatos);
                Municipio.LlenaObjeto(Empresa.IdMunicipio, ConexionBaseDatos);
            }
            else
            {
                Localidad.LlenaObjeto(Sucursal.IdLocalidad, ConexionBaseDatos);
                Municipio.LlenaObjeto(Sucursal.IdMunicipio, ConexionBaseDatos);
            }
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
            Modelo.Add(new JProperty("LugarExpedicion", Estado.Estado + ", " + Pais.Pais));
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(ConexionBaseDatos));
            Modelo.Add("FormaPagos", CJson.ObtenerJsonFormaPago(1, ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(Cliente.IdCondicionPago, ConexionBaseDatos));
            Modelo.Add("CuentaBancariaClientes", CJson.ObtenerJsonCuentaBancariaCliente(Cliente.IdCliente, ConexionBaseDatos));
            Modelo.Add("TipoRelacion", CJson.ObtenerJsonTipoRelacion(ConexionBaseDatos));
            Modelo.Add("UsoCFDI", CJson.ObtenerJsonUsoCFDI(ConexionBaseDatos));
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
    public static string LlenaMotivoCancelacion(int IdNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEliminarNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (Usuario.TienePermisos(new string[] { "puedeEliminarNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEliminarNotaCredito = 1;
        }
        oPermisos.Add("puedeEliminarNotaCredito", puedeEliminarNotaCredito);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CNotaCredito.ObtenerNotaCredito(Modelo, IdNotaCredito, ConexionBaseDatos);
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
    public static string EditarMontos(Dictionary<string, object> pNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();

        CUtilerias Utilerias = new CUtilerias();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pNotaCredito["IdEncabezadoFactura"]), ConexionBaseDatos);

        CNotaCreditoEncabezadoFactura NotaCreditoEncabezadoFactura = new CNotaCreditoEncabezadoFactura();
        NotaCreditoEncabezadoFactura.IdNotaCredito = Convert.ToInt32(pNotaCredito["IdNotaCredito"]);
        NotaCreditoEncabezadoFactura.IdEncabezadoFactura = Convert.ToInt32(pNotaCredito["IdEncabezadoFactura"]);
        NotaCreditoEncabezadoFactura.Monto = Convert.ToDecimal(pNotaCredito["Monto"]);
        NotaCreditoEncabezadoFactura.FechaPago = Convert.ToDateTime(DateTime.Now);
        NotaCreditoEncabezadoFactura.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        NotaCreditoEncabezadoFactura.IdTipoMoneda = Convert.ToInt32(pNotaCredito["IdTipoMoneda"]);
        NotaCreditoEncabezadoFactura.TipoCambio = Convert.ToDecimal(pNotaCredito["TipoCambio"]);
        NotaCreditoEncabezadoFactura.Nota = "pago de la factura";

        string validacion = ValidarMontos(NotaCreditoEncabezadoFactura, FacturaEncabezado, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            NotaCreditoEncabezadoFactura.AgregarNotaCreditoEncabezadoFactura(ConexionBaseDatos);
            oRespuesta.Add("Monto", Convert.ToDecimal(pNotaCredito["Monto"]));
            oRespuesta.Add("rowid", Convert.ToDecimal(pNotaCredito["rowid"]));
            oRespuesta.Add("TipoMoneda", Convert.ToString(pNotaCredito["TipoMoneda"]));
            oRespuesta.Add("AbonosNotaCredito", NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(Convert.ToInt32(pNotaCredito["IdNotaCredito"]), ConexionBaseDatos));


            if (Convert.ToInt32(pNotaCredito["EsParcialidad"]) == 1)
            {
                /*
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                int NumeroParcialidadActual = 0;
                NumeroParcialidadActual = (FacturaEncabezado.NumeroParcialidades - FacturaEncabezado.NumeroParcialidadesPendientes) + 1;
                string Descripcion = "";
                Descripcion = "Parcialidad " + NumeroParcialidadActual + " de " + FacturaEncabezado.NumeroParcialidades;
                FacturaEncabezado.AgregarFacturaIndividual(ConexionBaseDatos, Descripcion, Convert.ToDecimal(pNotaCredito["Monto"]));

                CFacturaEncabezadoSucursal FacturaEncabezadoSucursal = new CFacturaEncabezadoSucursal();
                FacturaEncabezadoSucursal.IdFacturaEncabezado = FacturaEncabezado.IdFacturaEncabezado;
                FacturaEncabezadoSucursal.IdSucursal = Usuario.IdSucursalActual;
                FacturaEncabezadoSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                FacturaEncabezadoSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                FacturaEncabezadoSucursal.Agregar(ConexionBaseDatos);

                string TotalLetras = "";

                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CFacturaEncabezado FacturaEncabezadoTotal = new CFacturaEncabezado();
                FacturaEncabezadoTotal.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdFacturaEncabezado), ConexionBaseDatos);


                string nomenclatura = "";
                if (FacturaEncabezadoTotal.IdTipoMoneda == 1)
                {
                    nomenclatura = "M.N.";
                }
                else
                {
                    nomenclatura = "USD";
                }

                TipoMoneda.LlenaObjeto(FacturaEncabezadoTotal.IdTipoMoneda, ConexionBaseDatos);
                TotalLetras = Utilerias.ConvertLetter(FacturaEncabezadoTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString()) + nomenclatura;
                FacturaEncabezadoTotal.TotalLetra = TotalLetras;
                FacturaEncabezadoTotal.Editar(ConexionBaseDatos);

                CFacturaEncabezado FacturaEncabezadoGlobal = new CFacturaEncabezado();
                FacturaEncabezadoGlobal.LlenaObjeto(FacturaEncabezadoTotal.IdFacturaGlobal, ConexionBaseDatos);
                FacturaEncabezadoGlobal.NumeroParcialidadesPendientes = FacturaEncabezadoGlobal.NumeroParcialidadesPendientes - 1;
                FacturaEncabezadoGlobal.Editar(ConexionBaseDatos);

                //Abrir Conexion

                CConfiguracion Configuracion = new CConfiguracion();
                Configuracion.LlenaObjeto(1, ConexionBaseDatos);
                //¿La conexion se establecio?

                string validacionFactura = CrearArchivoBuzonFiscalFactura(Convert.ToInt32(FacturaEncabezadoTotal.IdFacturaEncabezado), ConexionBaseDatos);
                if (validacionFactura == "1")
                {
                    Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                    validacionFactura = BuzonFiscalTimbradoFactura(Convert.ToInt32(FacturaEncabezadoTotal.IdFacturaEncabezado), ConexionBaseDatos);
                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", validacionFactura));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacionFactura));
                }*/

                oRespuesta.Add("EsParcialidad", 0);

            }
            else
            {
                oRespuesta.Add("EsParcialidad", 0);
                oRespuesta.Add(new JProperty("Error", 0));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    //timbrado de factura///////////////////////////////////////////////////////////////////////////////////        

    private static string CrearArchivoBuzonFiscalFactura(int pIdFactura, CConexion pConexion)
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
        CValidacion ValidacionRFC = new CValidacion();
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
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt");
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
            file.WriteLine("reRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("reNombre|" + Organizacion.RazonSocial);
            file.WriteLine("reCalle|" + DireccionOrganizacion.Calle);
            file.WriteLine("reNoExterior|" + DireccionOrganizacion.NumeroExterior);
            file.WriteLine("reNoInterior|" + DireccionOrganizacion.NumeroInterior);
            file.WriteLine("reColonia|" + DireccionOrganizacion.Colonia);
            file.WriteLine("reLocalidad|" + Localidad.Localidad);
            file.WriteLine("reReferencia|" + DireccionOrganizacion.Referencia);
            file.WriteLine("reMunicipio|" + Municipio.Municipio);
            file.WriteLine("reEstado|" + Estado.Estado);
            file.WriteLine("rePais|" + Pais.Pais);
            file.WriteLine("reCodigoPostal|" + DireccionOrganizacion.CodigoPostal);
            file.WriteLine("reNoCliente|" + Cliente.IdCliente);
            file.WriteLine("reEmail|" + Cliente.Correo);
            file.WriteLine("reTelefono|" + DireccionOrganizacion.ConmutadorTelefono);
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

            file.WriteLine("desRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
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
                    file.WriteLine("unidad|" + UnidadCompraVenta.UnidadCompraVenta);
                }
                file.WriteLine("numIdentificacion|" + oFacturaDetalle.Clave);
                file.WriteLine("descripcion|" + oFacturaDetalle.Descripcion.Replace('\n', ' '));
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
            file.WriteLine("descuentoPorcentaje|");
            file.WriteLine("descuentoMonto|");
            file.WriteLine("descuentoMotivo|");
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
            if (FacturaEncabezado.Refid != "")
            {
                file.WriteLine("refID|" + FacturaEncabezado.IdFacturaEncabezado);
            }
            else
            {
                file.WriteLine("refID|" + FacturaEncabezado.IdFacturaEncabezado );
            }

            file.WriteLine("tipoDocumento|Factura");

            Usuario.LlenaObjeto(FacturaEncabezado.IdUsuarioAgente, pConexion);

            file.WriteLine("ordenCompra|");
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

    private static string BuzonFiscalTimbradoFactura(int pIdFactura, CConexion pConexion)
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
            TxtTimbradosFactura.Agregar(pConexion);
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

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [WebMethod]
    public static string EliminarNotaCreditoEncabezadoFactura(Dictionary<string, object> pNotaCreditoEncabezadoFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        int IdFacturaEncabezadoParcial = 0;

        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);

        CNotaCreditoEncabezadoFactura NotaCreditoEncabezadoFactura = new CNotaCreditoEncabezadoFactura();
        NotaCreditoEncabezadoFactura.LlenaObjeto(Convert.ToInt32(pNotaCreditoEncabezadoFactura["pIdNotaCreditoEncabezadoFactura"]), ConexionBaseDatos);
        FacturaEncabezado.LlenaObjeto(NotaCreditoEncabezadoFactura.IdEncabezadoFactura, ConexionBaseDatos);

        JObject oRespuesta = new JObject();

        if (FacturaEncabezado.Parcialidades == true)
        {
            IdFacturaEncabezadoParcial = NotaCreditoEncabezadoFactura.ValidaEliminarCuentasPorCobrarDetalle(Convert.ToInt32(FacturaEncabezado.IdFacturaEncabezado), ConexionBaseDatos);
            if (IdFacturaEncabezadoParcial != 0)
            {
                string MotivoCancelacion = "Se cancela la factura parcial por eliminación de cobro";

                string validacion = CancelarArchivoBuzonFiscalFactura(Convert.ToInt32(IdFacturaEncabezadoParcial), ConexionBaseDatos);
                if (validacion == "1")
                {
                    CUtilerias Utilerias = new CUtilerias();
                    Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                    validacion = BuzonFiscalTimbradoCancelacionFactura(Convert.ToInt32(IdFacturaEncabezadoParcial), MotivoCancelacion, ConexionBaseDatos);
                    if (validacion == "Factura cancelada correctamente")
                    {
                        NotaCreditoEncabezadoFactura.IdNotaCreditoEncabezadoFactura = Convert.ToInt32(pNotaCreditoEncabezadoFactura["pIdNotaCreditoEncabezadoFactura"]);
                        NotaCreditoEncabezadoFactura.Baja = true;
                        NotaCreditoEncabezadoFactura.EliminarNotaCreditoEncabezadoFactura(ConexionBaseDatos);
                        oRespuesta.Add("AbonosNotaCredito", NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(NotaCreditoEncabezadoFactura.IdNotaCredito, ConexionBaseDatos));
                        oRespuesta.Add(new JProperty("Error", 0));
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                    }
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                oRespuesta.Add("EsParcialidad", 1);

            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede eliminar este movimiento porque la factura parcial no esta timbrada."));
            }
        }
        else
        {
            NotaCreditoEncabezadoFactura.IdNotaCreditoEncabezadoFactura = Convert.ToInt32(pNotaCreditoEncabezadoFactura["pIdNotaCreditoEncabezadoFactura"]);
            NotaCreditoEncabezadoFactura.Baja = true;
            NotaCreditoEncabezadoFactura.EliminarNotaCreditoEncabezadoFactura(ConexionBaseDatos);
            oRespuesta.Add("EsParcialidad", 0);
            oRespuesta.Add("AbonosNotaCredito", NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(NotaCreditoEncabezadoFactura.IdNotaCredito, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    private static string CancelarArchivoBuzonFiscalFactura(int pIdFacturaEncabezado, CConexion pConexion)
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
        CValidacion ValidacionRFC = new CValidacion();

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
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt");
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
            file.WriteLine("reRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
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

    private static string BuzonFiscalTimbradoCancelacionFactura(int pIdFacturaEncabezado, string MotivoCancelacion, CConexion pConexion)
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
    public static string CambiarEstatus(int pIdNotaCredito, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CNotaCredito NotaCredito = new CNotaCredito();
            NotaCredito.IdNotaCredito = pIdNotaCredito;
            NotaCredito.Baja = pBaja;
            NotaCredito.Eliminar(ConexionBaseDatos);
            respuesta = "0|NotaCreditoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Nota de credito devolucion
    [WebMethod]
    public static string ObtenerNumeroNotaCreditoDevolucion(Dictionary<string, object> pNotaCredito)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSerieNotaCredito SerieNotaCredito = new CSerieNotaCredito();
            SerieNotaCredito.LlenaObjeto(Convert.ToInt32(pNotaCredito["IdSerieNotaCredito"]), ConexionBaseDatos);
            CNotaCredito NotaCredito = new CNotaCredito();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            int NumeroNotaCredito = 0;
            NumeroNotaCredito = NotaCredito.ObtieneNumeroNotaCredito(SerieNotaCredito.SerieNotaCredito, Usuario.IdSucursalActual, ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("NumeroNotaCredito", NumeroNotaCredito);
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
    public static string ObtenerTipoCambioDevolucion(Dictionary<string, object> pTipoCambio)
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturasRegistradas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCredito, int pIdTipoNotaCredito, int pIdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_grdNotaCreditoAsociarProductosDevolucionCancelacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 30).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCredito", SqlDbType.Int).Value = pIdNotaCredito;
        Stored.Parameters.Add("pIdTipoNotaCredito", SqlDbType.Int).Value = pIdTipoNotaCredito;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return new CJQGridJsonResponse(dataSet);
    }

    //Busquedas
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
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        COrganizacion jsonRazonSocial = new COrganizacion();
        jsonRazonSocial.StoredProcedure.CommandText = "sp_Oportunidad_ConsultarFiltrosGrid";
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Usuario.IdSucursalActual);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        respuesta = jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string LlenaComboTipoNotaCredito()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        JObject notaCredito = new JObject();
        CJson jsonNotaCredito = new CJson();
        jsonNotaCredito.StoredProcedure.CommandText = "sp_TipoNotaCredito_ConsultarTipoNotaCredito";
        notaCredito.Add("Opciones", jsonNotaCredito.ObtenerJsonJObject(ConexionBaseDatos));

        oRespuesta.Add(new JProperty("Modelo", notaCredito));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }


    //agregar devoluciones
    [WebMethod]
    public static string AgregarDevoluciones(Dictionary<string, object> pRequest)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            if (pRequest["pDevoluciones"].ToString().Length > 0)
            {
                string[] DevolucionesSeleccionadas = { };
                if (pRequest["pDevoluciones"].ToString().Length > 0)
                {
                    DevolucionesSeleccionadas = pRequest["pDevoluciones"].ToString().Split(',');
                }

                decimal totalMonto = 0;
                decimal totalMontoProductos = 0;

                List<CDevolucion> Devolucion = new List<CDevolucion>();
                foreach (Dictionary<string, object> oProductos in (Array)pRequest["IdsFacturas"])
                {

                    CDevolucion Devoluciones = new CDevolucion();
                    Devoluciones.IdNotaCredito = Convert.ToInt32(pRequest["pIdNotaCredito"]);
                    Devoluciones.IdFacturaDetalle = Convert.ToInt32(oProductos["IdDetalleFactura"]);
                    Devoluciones.Cantidad = Convert.ToInt32(oProductos["Cantidad"]);
                    Devoluciones.AgregarDevolucion(ConexionBaseDatos);
                }
                //Sumar los montos de los productos activos
                CDevolucion DevolucionesProductosActivos = new CDevolucion();
                totalMontoProductos = DevolucionesProductosActivos.ObtieneMonto(Convert.ToInt32(pRequest["pIdNotaCredito"]), ConexionBaseDatos);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

                CIVA oIVA = new CIVA();
                oIVA.LlenaObjeto(Sucursal.IdIVA, ConexionBaseDatos);

                decimal IVA = 0;
                IVA = Decimal.Round((totalMontoProductos * oIVA.IVA) / 100, 2);

                decimal total = 0;
                total = Decimal.Round(totalMontoProductos + IVA, 2);

                CNotaCredito NotaCredito = new CNotaCredito();
                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                NotaCredito.LlenaObjeto(Convert.ToInt32(pRequest["pIdNotaCredito"]), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCredito.IdTipoMoneda, ConexionBaseDatos);
                NotaCredito.Monto = totalMontoProductos;
                NotaCredito.IVA = IVA;
                NotaCredito.Total = total;
                NotaCredito.SaldoDocumento = total;

                string nomenclatura = "";
                if (NotaCredito.IdTipoMoneda == 1)
                {
                    nomenclatura = "M.N.";
                }
                else
                {
                    nomenclatura = "USD";
                }

                TotalLetras = Utilerias.ConvertLetter(NotaCredito.Total.ToString(), TipoMoneda.TipoMoneda.ToString()) + nomenclatura;
                NotaCredito.TotalLetra = TotalLetras;
                NotaCredito.Editar(ConexionBaseDatos);

                oRespuesta.Add(new JProperty("TotalMonto", totalMontoProductos));
                oRespuesta.Add(new JProperty("IVA", IVA));
                oRespuesta.Add(new JProperty("Total", total));

                oRespuesta.Add(new JProperty("Error", 0));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();

            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se seleccionaron productos."));
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
    public static string EliminarProductosAsociados(string pIdDevoluciones, int pIdNotaCredito)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        decimal totalMontoProductos = 0;
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            if (pIdDevoluciones.Length > 0)
            {
                string[] productosSeleccionados = { };
                if (pIdDevoluciones.Length > 0)
                {
                    productosSeleccionados = pIdDevoluciones.Split(',');
                }

                foreach (string oIdDevolucion in productosSeleccionados)
                {
                    CDevolucion Devolucion = new CDevolucion();
                    Devolucion.IdDevolucion = Convert.ToInt32(oIdDevolucion);
                    Devolucion.Baja = true;
                    Devolucion.Eliminar(ConexionBaseDatos);
                }


                //Sumar los montos de los productos activos
                CDevolucion DevolucionesProductosActivos = new CDevolucion();
                totalMontoProductos = DevolucionesProductosActivos.ObtieneMonto(pIdNotaCredito, ConexionBaseDatos);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

                CIVA oIVA = new CIVA();
                oIVA.LlenaObjeto(Sucursal.IdIVA, ConexionBaseDatos);

                decimal IVA = 0;
                IVA = Decimal.Round((totalMontoProductos * oIVA.IVA) / 100, 2);

                decimal total = 0;
                total = Decimal.Round(totalMontoProductos + IVA, 2);

                CNotaCredito NotaCredito = new CNotaCredito();
                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                NotaCredito.LlenaObjeto(Convert.ToInt32(pIdNotaCredito), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCredito.IdTipoMoneda, ConexionBaseDatos);
                NotaCredito.Monto = totalMontoProductos;
                NotaCredito.IVA = IVA;
                NotaCredito.Total = total;
                NotaCredito.SaldoDocumento = total;

                string nomenclatura = "";
                if (NotaCredito.IdTipoMoneda == 1)
                {
                    nomenclatura = "M.N.";
                }
                else
                {
                    nomenclatura = "USD";
                }

                TotalLetras = Utilerias.ConvertLetter(NotaCredito.Total.ToString(), TipoMoneda.TipoMoneda.ToString()) + nomenclatura;
                NotaCredito.TotalLetra = TotalLetras;
                NotaCredito.Editar(ConexionBaseDatos);

                oRespuesta.Add(new JProperty("TotalMonto", totalMontoProductos));
                oRespuesta.Add(new JProperty("IVA", IVA));
                oRespuesta.Add(new JProperty("Total", total));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                oRespuesta.Add(new JProperty("Error", 0));
                return oRespuesta.ToString();
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "no hay productos seleccionados"));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();
            }
        }
        else
        { return "1|" + respuesta; }
    }
    //Validaciones
    private static string ValidarNotaCredito(CNotaCredito pNotaCredito, CConexion pConexion)
    {
        string errores = "";
        if (pNotaCredito.IdCliente == 0)
        { errores = errores + "<span>*</span> El campo cliente esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.SerieNotaCredito == "")
        { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.FolioNotaCredito == 0)
        { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.Descripcion == "")
        { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCredito.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.TipoCambio == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.Monto == 0)
        { errores = errores + "<span>*</span> El campo monto esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCredito.PorcentajeIVA == 0)
        { errores = errores + "<span>*</span> El campo porcentaje IVA esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCredito.IVA == 0)
        { errores = errores + "<span>*</span> El campo IVA esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCredito.Total == 0)
        { errores = errores + "<span>*</span> El campo total esta vacio, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarNotaCreditoDevolucionCancelacion(CNotaCredito pNotaCredito, CConexion pConexion)
    {
        string errores = "";
        if (pNotaCredito.IdCliente == 0)
        { errores = errores + "<span>*</span> El campo cliente esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.SerieNotaCredito == "")
        { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.FolioNotaCredito == 0)
        { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.Descripcion == "")
        { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCredito.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCredito.TipoCambio == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarMontos(CNotaCreditoEncabezadoFactura NotaCredito, CFacturaEncabezado FacturaEncabezado, CConexion pConexion)
    {
        string errores = "";

        if (FacturaEncabezado.Parcialidades == true)
        {
            if (FacturaEncabezado.NumeroParcialidadesPendientes == 1)
            {
                if (Convert.ToDecimal(NotaCredito.Monto) != Convert.ToDecimal(FacturaEncabezado.SaldoFactura))
                {
                    errores = errores + "<span>*</span> Es la ultima parcialidad, favor de pagar todo el saldo<br />";
                }
            }
        }

        if (NotaCredito.IdEncabezadoFactura == 0)
        { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

        if (NotaCredito.IdNotaCredito == 0)
        { errores = errores + "<span>*</span> No hay cuenta por cobrar seleccionada, favor de elegir alguna.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string CrearArchivoBuzonFiscal(int pIdNotaCredito, string LugarExpedicion, string MetodoPago, string FormaPago, string CondicionPago, string CuentaBancariaCliente, string Referencia, string Observaciones, CConexion pConexion)
    {
        string errores = "";
        CNotaCredito NotaCredito = new CNotaCredito();
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
        CValidacion LimpiarRFC = new CValidacion();

        string NombreArchivo = "";

        NotaCredito.LlenaObjeto(Convert.ToInt32(pIdNotaCredito), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(NotaCredito.IdUsuarioAlta), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);
        Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
        Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        Cliente.LlenaObjeto(NotaCredito.IdCliente, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        TipoMoneda.LlenaObjeto(NotaCredito.IdTipoMoneda, pConexion);

        Dictionary<string, object> ParametrosDO = new Dictionary<string, object>();
        ParametrosDO.Add("IdOrganizacion", Convert.ToInt32(Organizacion.IdOrganizacion));
        ParametrosDO.Add("IdTipoDireccion", Convert.ToInt32(1));
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosDO, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = NotaCredito.SerieNotaCredito + NotaCredito.FolioNotaCredito;

        if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
        {
            Encoding ANSI = Encoding.GetEncoding(1252);
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt", false, ANSI);
            file.WriteLine("HOJA");
            file.WriteLine("########################################################################");
            file.WriteLine("[Datos Generales]");
            file.WriteLine("serie|" + NotaCredito.SerieNotaCredito);
            file.WriteLine("folio|" + NotaCredito.FolioNotaCredito);
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
            file.WriteLine("reRfc|" + LimpiarRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("reNombre|" + Organizacion.RazonSocial);
            file.WriteLine("reCalle|" + DireccionOrganizacion.Calle);
            file.WriteLine("reNoExterior|" + DireccionOrganizacion.NumeroExterior);
            file.WriteLine("reNoInterior|" + DireccionOrganizacion.NumeroInterior);
            file.WriteLine("reColonia|" + DireccionOrganizacion.Colonia);
            file.WriteLine("reLocalidad|" + Localidad.Localidad);
            file.WriteLine("reReferencia|" + DireccionOrganizacion.Referencia);
            file.WriteLine("reMunicipio|" + Municipio.Municipio);
            file.WriteLine("reEstado|" + Estado.Estado);
            file.WriteLine("rePais|" + Pais.Pais);
            file.WriteLine("reCodigoPostal|" + DireccionOrganizacion.CodigoPostal);
            file.WriteLine("reNoCliente|" + Cliente.IdCliente);
            file.WriteLine("reEmail|" + Cliente.Correo);
            file.WriteLine("reTelefono|" + DireccionOrganizacion.ConmutadorTelefono);
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


            file.WriteLine("desRfc|" + LimpiarRFC.LimpiarRFC(Organizacion.RFC));
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

            file.WriteLine("[Datos de Conceptos]");
            file.WriteLine("cantidad|1");
            file.WriteLine("unidad|No Aplica");
            file.WriteLine("numIdentificacion|Nota de Credito");
            file.WriteLine("descripcion|" + NotaCredito.Descripcion);
            file.WriteLine("valorUnitario|" + NotaCredito.Monto);
            file.WriteLine("importe|" + NotaCredito.Monto);
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


            file.WriteLine("[Datos Complementarios del Comprobante a nivel global]");
            file.WriteLine("subtotalConceptos|" + NotaCredito.Monto);
            file.WriteLine("descuentoPorcentaje|");
            file.WriteLine("descuentoMonto|");
            file.WriteLine("descuentoMotivo|");
            file.WriteLine("cargos|");
            file.WriteLine("totalConceptos|" + NotaCredito.Monto);
            file.WriteLine("");
            file.WriteLine("pagoForma|" + FormaPago);
            file.WriteLine("pagoCondiciones|" + CondicionPago);
            file.WriteLine("pagoMetodo|" + MetodoPago);
            file.WriteLine("numCtaPago|" + CuentaBancariaCliente);
            file.WriteLine("lugarExpedicion|" + LugarExpedicion);
            file.WriteLine("serieFolioFiscalOrig|");
            file.WriteLine("folioFiscalOrig|");
            file.WriteLine("montoFolioFiscalOrig|");
            file.WriteLine("fechaFolioFiscalOrig|");

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
            if (NotaCredito.Refid != "")
            {
                file.WriteLine("refID|" + NotaCredito.IdNotaCredito);
            }
            else
            {
                file.WriteLine("refID|" + NotaCredito.IdNotaCredito);
            }
            file.WriteLine("tipoDocumento|Nota de Credito");

            file.WriteLine("ordenCompra|");
            file.WriteLine("agente|" + Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
            file.WriteLine("observaciones|" + Observaciones);
            file.WriteLine("nombreMoneda|" + TipoMoneda.TipoMoneda);
            file.WriteLine("tipoCambio|" + NotaCredito.TipoCambio);
            file.WriteLine("");
            file.WriteLine("[Impuestos Trasladados]");
            file.WriteLine("trasladadoImpuesto|IVA");
            file.WriteLine("trasladadoImporte|" + NotaCredito.IVA);
            file.WriteLine("trasladadoTasa|" + Sucursal.IVAActual);
            file.WriteLine("subtotalTrasladados|" + NotaCredito.IVA);
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
            file.WriteLine("montoTotal|" + NotaCredito.Total);
            file.WriteLine("montoTotalTexto|" + NotaCredito.TotalLetra);
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

    private static string CancelarArchivoBuzonFiscal(int pIdNotaCredito, CConexion pConexion)
    {
        string errores = "";
        CNotaCredito NotaCredito = new CNotaCredito();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CEmpresa Empresa = new CEmpresa();
        CTxtTimbradosNotaCredito TxtTimbradosNotaCredito = new CTxtTimbradosNotaCredito();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CValidacion ValidacionRFC = new CValidacion();

        string NombreArchivo = "";

        NotaCredito.LlenaObjeto(Convert.ToInt32(pIdNotaCredito), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(NotaCredito.IdUsuarioAlta), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

        Cliente.LlenaObjeto(NotaCredito.IdCliente, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

        Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
        ParametrosTxt.Add("Folio", NotaCredito.FolioNotaCredito);
        ParametrosTxt.Add("Serie", NotaCredito.SerieNotaCredito);
        TxtTimbradosNotaCredito.LlenaObjetoFiltros(ParametrosTxt, pConexion);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = "cancela_" + NotaCredito.SerieNotaCredito + NotaCredito.FolioNotaCredito;

        if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt");
            file.WriteLine("cancela");
            file.WriteLine("########################################################################");
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("uuid|" + TxtTimbradosNotaCredito.Uuid);
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("emRfc|" + Empresa.RFC);
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("reRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("");
            file.WriteLine("#Opcional");
            file.WriteLine("refID|" + TxtTimbradosNotaCredito.Refid);

            file.Close();

            errores = "1";
        }
        else
        {
            errores = "La ruta de la carpeta no es valida";
        }
        return errores;

    }

    private static string BuzonFiscalTimbrado(int pIdNotaCredito, CConexion pConexion)
    {
        CNotaCredito NotaCredito = new CNotaCredito();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();

        string NombreArchivo = "";

        NotaCredito.LlenaObjeto(Convert.ToInt32(pIdNotaCredito), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(NotaCredito.IdUsuarioAlta), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = NotaCredito.SerieNotaCredito + NotaCredito.FolioNotaCredito;

        if (File.Exists(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt"))
        {

            StreamReader objReader = new StreamReader(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt");
            string sLine = "";
            ArrayList arrText = new ArrayList();
            string Campo = "";
            CTxtTimbradosNotaCredito TxtTimbradosNotaCredito = new CTxtTimbradosNotaCredito();
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
                            TxtTimbradosNotaCredito.Refid = Convert.ToString(NotaCredito.IdNotaCredito);
                            break;
                        case "noCertificadoSAT":
                            TxtTimbradosNotaCredito.NoCertificadoSAT = Convert.ToString(split[1]);
                            break;
                        case "fechaTimbrado":
                            TxtTimbradosNotaCredito.FechaTimbrado = Convert.ToString(split[1]);
                            break;
                        case "uuid":
                            TxtTimbradosNotaCredito.Uuid = Convert.ToString(split[1]);
                            break;
                        case "noCertificado":
                            TxtTimbradosNotaCredito.NoCertificado = Convert.ToString(split[1]);
                            break;
                        case "selloSAT":
                            TxtTimbradosNotaCredito.SelloSAT = Convert.ToString(split[1]);
                            break;
                        case "sello":
                            TxtTimbradosNotaCredito.Sello = Convert.ToString(split[1]);
                            break;
                        case "fecha":
                            TxtTimbradosNotaCredito.Fecha = Convert.ToString(split[1]);
                            break;
                        case "folio":
                            TxtTimbradosNotaCredito.Folio = Convert.ToString(split[1]);
                            break;
                        case "serie":
                            TxtTimbradosNotaCredito.Serie = Convert.ToString(split[1]);
                            break;
                        case "leyendaImpresion":
                            TxtTimbradosNotaCredito.LeyendaImpresion = Convert.ToString(split[1]);
                            break;
                        case "cadenaOriginal":
                            TxtTimbradosNotaCredito.CadenaOriginal = Convert.ToString(split[1]);
                            break;
                        case "totalConLetra":
                            TxtTimbradosNotaCredito.TotalConLetra = Convert.ToString(split[1]);
                            break;
                    }
                }
            }
            TxtTimbradosNotaCredito.Agregar(pConexion);
            NotaCredito.Refid = TxtTimbradosNotaCredito.Refid;
            NotaCredito.Editar(pConexion);
            objReader.Close();

            NombreArchivo = "Nota de crédito timbrada correctamente";
        }
        else
        {
            NombreArchivo = "no existe el archivo";
        }

        return NombreArchivo;

    }

    private static string BuzonFiscalTimbradoCancelacion(int pIdNotaCredito, string MotivoCancelacion, CConexion pConexion)
    {
        CNotaCredito NotaCredito = new CNotaCredito();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CTxtTimbradosNotaCredito TxtTimbradosNotaCredito = new CTxtTimbradosNotaCredito();
        string NombreArchivo = "";

        NotaCredito.LlenaObjeto(Convert.ToInt32(pIdNotaCredito), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(NotaCredito.IdUsuarioAlta), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);


        Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
        ParametrosTxt.Add("Folio", Convert.ToString(NotaCredito.FolioNotaCredito));
        ParametrosTxt.Add("Serie", Convert.ToString(NotaCredito.SerieNotaCredito));
        TxtTimbradosNotaCredito.LlenaObjetoFiltros(ParametrosTxt, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = "cancela_" + NotaCredito.SerieNotaCredito + NotaCredito.FolioNotaCredito;

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
                            TxtTimbradosNotaCredito.FechaCancelacion = Convert.ToString(split[1]);
                            string[] fechaFormateada = split[1].Split('T');
                            if (fechaFormateada[1].Length == 13)
                            {
                                fechaFormateada[1] = "0" + fechaFormateada[1];
                            }
                            NotaCredito.FechaCancelacion = Convert.ToDateTime(fechaFormateada[0] + "T" + fechaFormateada[1]);
                            break;
                    }
                }
            }
            TxtTimbradosNotaCredito.Editar(pConexion);
            NotaCredito.Baja = true;
            NotaCredito.MotivoCancelacion = MotivoCancelacion;
            NotaCredito.IdUsuarioCancelacion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            NotaCredito.EditarNotaCreditoCancelacion(pConexion);

            objReader.Close();

            NombreArchivo = "Nota de crédito cancelada correctamente";
        }
        else
        {
            NombreArchivo = "no existe el archivo";
        }

        return NombreArchivo;

    }


    ////////////////////////  Nueva forma de guardar Notas de Credito /////////////////////////////////////

    /* Timbrar */
    [WebMethod]
    public static string ObtenerDatosNotaCredito(int IdNotaCredito, string IdMetodoPago, string IdFormaPago, string CondicionPago, string Observaciones, string IdTipoRelacion, string IdUsoCFDI)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Comprobante = new JObject();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();

                // Llenado de clases necesarias para la creación del comprobante
                CNotaCredito NotaCredito = new CNotaCredito();
                NotaCredito.LlenaObjeto(Convert.ToInt32(IdNotaCredito), pConexion);

                CUsuario usuario = new CUsuario();
                usuario.LlenaObjeto(NotaCredito.IdUsuarioAlta, pConexion);

                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(usuario.IdSucursalActual, pConexion);

                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(NotaCredito.IdCliente, pConexion);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

                CMetodoPago FormaPago = new CMetodoPago();
                FormaPago.LlenaObjeto(Convert.ToInt32(IdMetodoPago),pConexion);

                CRutaCFDI RutaCFDI = new CRutaCFDI();
                pParametros.Clear();
                pParametros.Add("IdSucursal", Convert.ToInt32(UsuarioSesion.IdSucursalActual));
                pParametros.Add("TipoRuta", Convert.ToInt32(1));
                pParametros.Add("Baja", Convert.ToInt32(0));
                RutaCFDI.LlenaObjetoFiltros(pParametros, pConexion);


                // datos del comprobante
                Comprobante.Add("Serie", NotaCredito.SerieNotaCredito);
                Comprobante.Add("Folio", NotaCredito.FolioNotaCredito);
                Comprobante.Add("Fecha", NotaCredito.FechaAlta);
                Comprobante.Add("LugarExpedicion", Empresa.CodigoPostal); // Catalogo SAT
                Comprobante.Add("Moneda", (NotaCredito.IdTipoMoneda == 1) ? "MXN" : "USD"); // Catalogo SAT
                Comprobante.Add("TipoCambio", NotaCredito.TipoCambio);
                Comprobante.Add("CondicionDePago", CondicionPago); //Factura.FechaPago.ToShortDateString());
                Comprobante.Add("FormaPago", FormaPago.Clave); // Catalogo SAT
                Comprobante.Add("MetodoPago", (IdFormaPago == "2") ? "PPD" : "PUE"); // Catalogo SAT
                Comprobante.Add("TipoDeComprobante", "E"); // Catalogo SAT
                Comprobante.Add("SubTotal", NotaCredito.Monto);
                Comprobante.Add("Total", NotaCredito.Total);
                Comprobante.Add("NoCertificado","20001000000300022755"); // NoCertificado Example // Sucursal.NoCertificado);
                Comprobante.Add("Certificado", ""); // Llenado por SAT
                Comprobante.Add("Sello", ""); // Llenado por SAT

                // datos del emisor
                JObject Emisor = new JObject();
                Emisor.Add("Nombre", ClearString(Empresa.RazonSocial));
                Emisor.Add("RFC","MAG041126GT8"); // RFC example // ClearString(Empresa.RFC)); 
                Emisor.Add("RegimenFiscal", "601"); // Catalogo SAT

                Comprobante.Add("Emisor", Emisor);

                // datos del receptor
                JObject Receptor = new JObject();
                Receptor.Add("Nombre", ClearString(Organizacion.RazonSocial));
                Receptor.Add("RFC", ClearString(Organizacion.RFC));
                CUsoCFDI usoCFDI = new CUsoCFDI();
                usoCFDI.LlenaObjeto(Convert.ToInt32(IdUsoCFDI), pConexion);
                Receptor.Add("UsoCFDI", usoCFDI.ClaveUsoCFDI);// Catalogo SAT

                Comprobante.Add("Receptor", Receptor);


                //CFDIs RELACIONADOS
                JArray CfdisRelacionados = new JArray();
                CTipoRelacion tipoRelacion = new CTipoRelacion();
                tipoRelacion.LlenaObjeto(Convert.ToInt32(IdTipoRelacion), pConexion);
                if (NotaCredito.IdTipoNotaCredito == 1) // Si es tipo devolucion
                {
                    CDevolucion devolucion = new CDevolucion();
                    pParametros.Clear();
                    pParametros.Add("IdNotaCredito",NotaCredito.IdNotaCredito);
                    pParametros.Add("Baja",0);
                    
                    foreach (CDevolucion oDevolucion in devolucion.LlenaObjetosFiltros(pParametros, pConexion))
                    {
                        JObject cfdiRelacionadoD = new JObject();
                        CFacturaDetalle facturaDetalle = new CFacturaDetalle();
                        pParametros.Clear();
                        pParametros.Add("IdFacturaDetalle", oDevolucion.IdFacturaDetalle);
                        facturaDetalle.LlenaObjetoFiltros(pParametros, pConexion);

                        CFacturaEncabezado facturaEncabezado = new CFacturaEncabezado();
                        facturaEncabezado.LlenaObjeto(facturaDetalle.IdFacturaEncabezado, pConexion);

                        cfdiRelacionadoD.Add("TipoRelacion", tipoRelacion.Clave);
                        cfdiRelacionadoD.Add("UUID",facturaEncabezado.UUIDGlobal);

                        CfdisRelacionados.Add(cfdiRelacionadoD);
                    }
                }
                else //si es tipo cancelacion o descuento
                {
                    CNotaCreditoEncabezadoFactura ncEncabezadoFactura = new CNotaCreditoEncabezadoFactura();
                    pParametros.Clear();
                    pParametros.Add("IdNotaCredito", NotaCredito.IdNotaCredito);
                    pParametros.Add("Baja", 0);
                    foreach (CNotaCreditoEncabezadoFactura oncEncabezadoFactura in ncEncabezadoFactura.LlenaObjetosFiltros(pParametros, pConexion))
                    {
                        JObject cfdiRelacionadoEF = new JObject();
                        CFacturaEncabezado facturaEncabezado = new CFacturaEncabezado();
                        facturaEncabezado.LlenaObjeto(oncEncabezadoFactura.IdEncabezadoFactura, pConexion);

                        CTxtTimbradosFactura timbrado = new CTxtTimbradosFactura();
                        pParametros.Clear();
                        pParametros.Add("Refid", facturaEncabezado.IdFacturaEncabezado);
                        timbrado.LlenaObjetoFiltros(pParametros, pConexion);

                        cfdiRelacionadoEF.Add("TipoRelacion", tipoRelacion.Clave);
                        cfdiRelacionadoEF.Add("UUID", timbrado.Uuid);

                        CfdisRelacionados.Add(cfdiRelacionadoEF);
                    }
                }

                Comprobante.Add("CFDISRelacionados", CfdisRelacionados);
                
                JArray Conceptos = new JArray();

                JObject Concepto = new JObject();

                Concepto.Add("ClaveProdServ", "84111506"); // Catalogo SAT
                Concepto.Add("Cantidad", 1);
                Concepto.Add("ClaveUnidad", "ACT"); // Catalogo SAT
                Concepto.Add("Descripcion", ClearString(NotaCredito.Descripcion));
                Concepto.Add("ValorUnitario", NotaCredito.Monto );
                Concepto.Add("Importe", NotaCredito.Monto);

                JObject Impuestos = new JObject();

                JArray Traslados = new JArray();

                JObject Traslado = new JObject();

                JObject TrasladoContenido = new JObject();

                decimal tasaCuota = NotaCredito.PorcentajeIVA / 100;
                decimal importeImpTras = NotaCredito.Monto * tasaCuota;
                importeImpTras = Math.Round(importeImpTras, 2);

                TrasladoContenido.Add("Base", NotaCredito.Monto);
                TrasladoContenido.Add("Impuesto", "002"); // Catalogo SAT
                TrasladoContenido.Add("TipoFactor", "Tasa"); // Catalogo SAT
                TrasladoContenido.Add("TasaOCuota", tasaCuota);
                TrasladoContenido.Add("Importe", importeImpTras);

                Traslado.Add("Traslado", TrasladoContenido);

                Traslados.Add(Traslado);

                Impuestos.Add("Traslados", Traslados);

                Concepto.Add("Impuestos", Impuestos);

                Conceptos.Add(Concepto);

                Comprobante.Add("Conceptos", Conceptos);
                

                // Llenado de impuestos de la factura
                JObject ImpuestosGlobal = new JObject();

                ImpuestosGlobal.Add("TotalImpuestosTrasladados", importeImpTras);

                JArray TrasladosGlobal = new JArray();

                JObject TrasladoGlobal = new JObject();

                JObject TrasladoGlobalContenido = new JObject();

                TrasladoGlobalContenido.Add("Impuesto", "002"); // Catalogo SAT
                TrasladoGlobalContenido.Add("TipoFactor", "Tasa"); // Catalogo SAT
                TrasladoGlobalContenido.Add("TasaOCuota", tasaCuota);
                TrasladoGlobalContenido.Add("Importe", importeImpTras);

                TrasladoGlobal.Add("Traslado", TrasladoGlobalContenido);

                TrasladosGlobal.Add(TrasladoGlobal);

                ImpuestosGlobal.Add("Traslados", TrasladosGlobal);

                Comprobante.Add("Impuestos", ImpuestosGlobal);
                
                //Ruta CFDI
                Respuesta.Add("RutaCFDI", RutaCFDI.RutaCFDI);

                // Envio de correos a emisor y receptor
                string Correos = "";

                Correos = "fespino@grupoasercom.com";
                
                // Terminado de datos de comprobate
                Respuesta.Add("Id", 94327); // Id example // Empresa.IdTimbrado);
                Respuesta.Add("Token", "$2b$12$pj0NTsT/brybD2cJrNa8iuRRE5KoxeEFHcm/yJooiSbiAdbiTGzIq"); // Token example // Empresa.Token);
                Respuesta.Add("Comprobante", Comprobante);
                Respuesta.Add("RFC", "MAG041126GT8"); // RFC example // Empresa.RFC); 
                Respuesta.Add("RefID", NotaCredito.IdNotaCredito);
                Respuesta.Add("NoCertificado", "20001000000300022755"); // NoCertificado example  // Sucursal.NoCertificado);
                Respuesta.Add("Formato", "zip"); // xml, pdf, zip
                Respuesta.Add("Correos", Correos);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }
    
    [WebMethod]
    public static string GuardarNotaCredito(string UUId, int RefId)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CNotaCredito NotaCredito = new CNotaCredito();
                NotaCredito.LlenaObjeto(RefId, pConexion);

                NotaCredito.Refid = RefId.ToString();

                CTxtTimbradosNotaCredito notaCredito = new CTxtTimbradosNotaCredito();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("Refid", NotaCredito.IdNotaCredito);
                notaCredito.LlenaObjetoFiltros(pParametros, pConexion);

                notaCredito.Uuid = UUId;
                notaCredito.Refid = RefId.ToString();
                notaCredito.Serie = NotaCredito.SerieNotaCredito;
                notaCredito.TotalConLetra = NotaCredito.TotalLetra;
                notaCredito.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                notaCredito.FechaTimbrado = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                notaCredito.Folio = NotaCredito.FolioNotaCredito.ToString();

                CTxtTimbradosNotaCredito ValidarTimbrado = new CTxtTimbradosNotaCredito();
                pParametros.Clear();
                pParametros.Add("Serie", notaCredito.Serie);
                pParametros.Add("Folio", notaCredito.Folio);

                if (ValidarTimbrado.LlenaObjetosFiltros(pParametros, pConexion).Count == 0)
                {
                    notaCredito.Agregar(pConexion);

                }
                else
                {
                    Error = 1;
                    DescripcionError = "Ya se habia emitido esta Nota de Crédito.";
                }
                NotaCredito.Refid = notaCredito.Refid;
                NotaCredito.Editar(pConexion);
                
                Error = 0;
                DescripcionError = "Se ha guardado con éxito la Nota de Crédito.";

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    /* Cancelar */
    [WebMethod]
    public static string ObtenerDatosCancelacion(int IdNotaCredito, string MotivoCancelacion)
    {
        JObject Respuesta = new JObject();
        CNotaCredito notaCredito = new CNotaCredito();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {

                JObject Comprobante = new JObject();
                
                if (notaCredito.ExisteNotaCreditoMovimientos(IdNotaCredito, pConexion) == 0)
                {
                    if (notaCredito.ExisteNotaCreditoTimbrada(IdNotaCredito, pConexion) == 1)
                    {

                        
                        Dictionary<string, object> pParametros = new Dictionary<string, object>();
                        pParametros.Add("RefID", IdNotaCredito);
                        notaCredito.LlenaObjetoFiltros(pParametros, pConexion);

                        CTxtTimbradosNotaCredito timbradoNC = new CTxtTimbradosNotaCredito();
                        pParametros.Clear();
                        pParametros.Add("Refid", notaCredito.IdNotaCredito);
                        timbradoNC.LlenaObjetoFiltros(pParametros,pConexion);

                        Comprobante.Add("UUID", timbradoNC.Uuid);
                        Comprobante.Add("ref_id", notaCredito.Refid);

                        CNotaCreditoSucursal ncSucursal = new CNotaCreditoSucursal();
                        pParametros.Clear();
                        pParametros.Add("IdNotaCredito", IdNotaCredito);
                        ncSucursal.LlenaObjetoFiltros(pParametros, pConexion);

                        CSucursal Sucursal = new CSucursal();
                        Sucursal.LlenaObjeto(ncSucursal.IdSucursal, pConexion);

                        CEmpresa Empresa = new CEmpresa();
                        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

                        // Terminado de datos de comprobate
                        Respuesta.Add("Id", Empresa.IdTimbrado);// 94327); // Id example // Empresa.IdTimbrado);
                        Respuesta.Add("Token", Empresa.Token); //"$2b$12$pj0NTsT/brybD2cJrNa8iuRRE5KoxeEFHcm/yJooiSbiAdbiTGzIq"); // Token example // Empresa.Token);
                        Respuesta.Add("Comprobante", Comprobante);
                        Respuesta.Add("RFC", Empresa.RFC); //"MAG041126GT8"); // RFC example // Empresa.RFC); 
                        Respuesta.Add("NoCertificado", Sucursal.NoCertificado); //"20001000000300022755"); // NoCertificado example  // Sucursal.NoCertificado);

                        Respuesta.Add("MotivoCancelacion", MotivoCancelacion);
                        
                    }
                    else
                    {
                        Error = 1;
                        DescripcionError = "No se puede cancelar esta nota de crédito, ya que no esta timbrada";
                    }

                }
                else
                {
                    Error = 1;
                    DescripcionError = "No se puede cancelar esta nota de crédito, porque tiene movimientos de cobros asociados a facturas";
                }
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();

    }

    [WebMethod]
    public static string EditarNotaCreditoCancelado(string Date, int RefId, string message, string MotivoCancelacion)
    {

        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Comprobante = new JObject();

                CNotaCredito NotaCredito = new CNotaCredito();
                CUsuario Usuario = new CUsuario();
                CSucursal Sucursal = new CSucursal();
                CTxtTimbradosNotaCredito TxtTimbradosNotaCredito = new CTxtTimbradosNotaCredito();

                NotaCredito.LlenaObjeto(Convert.ToInt32(RefId), pConexion);
                Usuario.LlenaObjeto(Convert.ToInt32(NotaCredito.IdUsuarioAlta), pConexion);
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);

                Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
                ParametrosTxt.Add("Folio", Convert.ToString(NotaCredito.FolioNotaCredito));
                ParametrosTxt.Add("Serie", Convert.ToString(NotaCredito.SerieNotaCredito));
                TxtTimbradosNotaCredito.LlenaObjetoFiltros(ParametrosTxt, pConexion);

                string date = Date;
                if (message != "")
                {
                    date = Convert.ToString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                }

                if (TxtTimbradosNotaCredito.FechaCancelacion == "")
                {
                    if (date != "")
                    {
                        TxtTimbradosNotaCredito.FechaCancelacion = Convert.ToString(date);

                        string[] fechaFormateada = date.Split('T');
                        if (fechaFormateada[1].Length == 13)
                        {
                            fechaFormateada[1] = "0" + fechaFormateada[1];
                        }
                        TxtTimbradosNotaCredito.FechaCancelacion = Convert.ToString(Convert.ToDateTime(fechaFormateada[0] + "T" + fechaFormateada[1]));
                    }
                    
                    TxtTimbradosNotaCredito.Editar(pConexion);
                    NotaCredito.Baja = true;
                    NotaCredito.MotivoCancelacion = MotivoCancelacion;
                    NotaCredito.IdUsuarioCancelacion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    NotaCredito.EditarNotaCreditoCancelacion(pConexion);
                    
                    Error = 0;
                    DescripcionError = "Se ha cancelado la Factura " + NotaCredito.FolioNotaCredito;
                }
                else
                {
                    Error = 1;
                    DescripcionError = "El documento ha sido cancelado previamente.";
                }

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();

    }

    /* Funciones para nuevo Timbrado */
    private static byte[] Decode(string Hash)
    {
        byte[] bytes = System.Convert.FromBase64String(Hash);
        return bytes;// System.Text.Encoding.UTF8.GetString(bytes);
    }

    private static string ClearString(string data)
    {
        string d = data.Replace("\"", "&quot;");
        d = d.Replace("“", "&quot;");
        d = d.Replace("&", "&amp;");
        d = d.Replace("'", "&apos;");
        d = d.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
        return d;
    }

}