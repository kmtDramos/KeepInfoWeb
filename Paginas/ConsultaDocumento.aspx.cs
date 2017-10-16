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

public partial class ConsultaDocumento : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    private static int idEmpresa;
    public static int puedeConsultarConsultaDocumento = 0;
    public static string FechaInicial = "";
    public static string FechaFinal = "";

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


        if (Usuario.TienePermisos(new string[] { "puedeConsultarConsultaDocumento" }, ConexionBaseDatos) == "") { puedeConsultarConsultaDocumento = 1; }
        else { puedeConsultarConsultaDocumento = 0; }

        DateTime Hoy = DateTime.Now;
        FechaFinal = Hoy.ToShortDateString();

        //grdFactura
        CJQGrid grdFactura = new CJQGrid();
        grdFactura.NombreTabla = "grdFactura";
        grdFactura.CampoIdentificador = "IdFacturaEncabezado";
        grdFactura.ColumnaOrdenacion = "NumeroFactura";
        grdFactura.Metodo = "ObtenerConsultaDocumentoFacturaCliente";
        grdFactura.TituloTabla = "Facturas del cliente";
        grdFactura.GenerarGridCargaInicial = false;
        grdFactura.GenerarFuncionFiltro = false;
        grdFactura.GenerarFuncionTerminado = false;
        grdFactura.NumeroRegistros = 10;
        grdFactura.RangoNumeroRegistros = "10,30,100";
        grdFactura.Altura = 150;
        grdFactura.EventoRegistroSeleccionado = "FacturaClienteSeleccionado";

        //IdFactura
        CJQColumn ColIdFacturaEncabezado = new CJQColumn();
        ColIdFacturaEncabezado.Nombre = "IdFacturaEncabezado";
        ColIdFacturaEncabezado.Oculto = "true";
        ColIdFacturaEncabezado.Encabezado = "IdFacturaEncabezado";
        ColIdFacturaEncabezado.Buscador = "true";
        grdFactura.Columnas.Add(ColIdFacturaEncabezado);

        //Serie
        CJQColumn ColSerie = new CJQColumn();
        ColSerie.Nombre = "SerieFactura";
        ColSerie.Encabezado = "Serie";
        ColSerie.Ancho = "65";
        ColSerie.Alineacion = "left";
        grdFactura.Columnas.Add(ColSerie);

        //NumeroFactura
        CJQColumn ColNoFactura = new CJQColumn();
        ColNoFactura.Nombre = "NumeroFactura";
        ColNoFactura.Encabezado = "Factura";
        ColNoFactura.Ancho = "65";
        ColNoFactura.Alineacion = "left";
        grdFactura.Columnas.Add(ColNoFactura);

        //Fecha
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Fecha";
        ColFechaEmision.Ancho = "65";
        ColFechaEmision.Alineacion = "left";
        ColFechaEmision.Buscador = "false";
        grdFactura.Columnas.Add(ColFechaEmision);

        //Estatus
        CJQColumn ColEstatus = new CJQColumn();
        ColEstatus.Nombre = "EstatusFacturaEncabezado";
        ColEstatus.Encabezado = "Estatus";
        ColEstatus.Ancho = "47";
        ColEstatus.Alineacion = "left";
        ColEstatus.Buscador = "false";
        grdFactura.Columnas.Add(ColEstatus);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Ancho = "65";
        ColTotal.Alineacion = "right";
        ColTotal.Buscador = "false";
        grdFactura.Columnas.Add(ColTotal);


        //Saldo
        CJQColumn ColSaldo = new CJQColumn();
        ColSaldo.Nombre = "SaldoFactura";
        ColSaldo.Encabezado = "Saldo";
        ColSaldo.Ancho = "65";
        ColSaldo.Alineacion = "right";
        ColSaldo.Buscador = "false";
        grdFactura.Columnas.Add(ColSaldo);

        //Condiciones 
        CJQColumn ColCondiciones = new CJQColumn();
        ColCondiciones.Nombre = "CondicionPago";
        ColCondiciones.Encabezado = "Condiciones";
        ColCondiciones.Ancho = "83";
        ColCondiciones.Alineacion = "left";
        ColCondiciones.Buscador = "false";
        grdFactura.Columnas.Add(ColCondiciones);


        //FOR
        CJQColumn ColFOR = new CJQColumn();
        ColFOR.Nombre = "Tipo";
        ColFOR.Encabezado = "FOR";
        ColFOR.Ancho = "65";
        ColFOR.Alineacion = "left";
        ColFOR.Buscador = "false";
        grdFactura.Columnas.Add(ColFOR);


        //Moneda
        CJQColumn ColMoneda = new CJQColumn();
        ColMoneda.Nombre = "TipoMoneda";
        ColMoneda.Encabezado = "Moneda";
        ColMoneda.Ancho = "47";
        ColMoneda.Alineacion = "left";
        ColMoneda.Buscador = "false";
        grdFactura.Columnas.Add(ColMoneda);

        //Estado
        CJQColumn ColEstado = new CJQColumn();
        ColEstado.Nombre = "EstadoDocumentacion";
        ColEstado.Encabezado = "Estado";
        ColEstado.Ancho = "65";
        ColEstado.Alineacion = "left";
        ColEstado.Buscador = "false";
        grdFactura.Columnas.Add(ColEstado);

        //Cliente
        CJQColumn ColCliente = new CJQColumn();
        ColCliente.Nombre = "RazonSocial";
        ColCliente.Encabezado = "Cliente";
        ColCliente.Ancho = "83";
        ColCliente.Alineacion = "left";
        ColCliente.Buscador = "false";
        grdFactura.Columnas.Add(ColCliente);

        //Agente
        CJQColumn ColAgente = new CJQColumn();
        ColAgente.Nombre = "Agente";
        ColAgente.Encabezado = "Agente";
        ColAgente.Ancho = "65";
        ColAgente.Alineacion = "left";
        ColAgente.Buscador = "false";
        grdFactura.Columnas.Add(ColAgente);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Division";
        ColDivision.Ancho = "65";
        ColDivision.Alineacion = "left";
        ColDivision.Buscador = "false";
        grdFactura.Columnas.Add(ColDivision);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFactura", grdFactura.GeneraGrid(), true);

        //grdFacturaDetalleCliente
        CJQGrid grdFacturaDetalleCliente = new CJQGrid();
        grdFacturaDetalleCliente.NombreTabla = "grdFacturaDetalleCliente";
        grdFacturaDetalleCliente.CampoIdentificador = "IdFacturaEncabezado";
        grdFacturaDetalleCliente.ColumnaOrdenacion = "IdFacturaDetalle";
        grdFacturaDetalleCliente.Metodo = "ObtenerConsultaDocumentoFacturaDetalleCliente";
        grdFacturaDetalleCliente.TituloTabla = "Detalle de facturas de clientes";
        grdFacturaDetalleCliente.GenerarGridCargaInicial = false;
        grdFacturaDetalleCliente.GenerarFuncionFiltro = false;
        grdFacturaDetalleCliente.GenerarFuncionTerminado = false;
        grdFacturaDetalleCliente.NumeroRegistros = 10;
        grdFacturaDetalleCliente.RangoNumeroRegistros = "10,30,100";
        grdFacturaDetalleCliente.Altura = 150;
        grdFacturaDetalleCliente.EventoRegistroSeleccionado = "DetalleFacturaClienteSeleccionado";

        //IdFactura
        CJQColumn ColIdFacturaDetalle = new CJQColumn();
        ColIdFacturaDetalle.Nombre = "IdFacturaDetalle";
        ColIdFacturaDetalle.Oculto = "true";
        ColIdFacturaDetalle.Encabezado = "IdFacturaDetalle";
        ColIdFacturaDetalle.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColIdFacturaDetalle);

        //Serie
        CJQColumn ColSerieFactura = new CJQColumn();
        ColSerieFactura.Nombre = "SerieFactura";
        ColSerieFactura.Encabezado = "Serie";
        ColSerieFactura.Ancho = "50";
        ColSerieFactura.Alineacion = "left";
        ColSerieFactura.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColSerieFactura);

        //NumeroFactura
        CJQColumn ColNumeroFactura = new CJQColumn();
        ColNumeroFactura.Nombre = "NumeroFactura";
        ColNumeroFactura.Encabezado = "Factura";
        ColNumeroFactura.Ancho = "50";
        ColNumeroFactura.Alineacion = "left";
        ColNumeroFactura.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColNumeroFactura);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Ancho = "116";
        ColClave.Alineacion = "left";
        ColClave.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColClave);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Ancho = "116";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Ancho = "65";
        ColCantidad.Alineacion = "left";
        ColCantidad.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColCantidad);

        //Precio
        CJQColumn ColPrecio = new CJQColumn();
        ColPrecio.Nombre = "PrecioUnitario";
        ColPrecio.Encabezado = "Precio";
        ColPrecio.Ancho = "80";
        ColPrecio.Alineacion = "right";
        ColPrecio.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColPrecio);

        //Total
        CJQColumn ColTotalDetalle = new CJQColumn();
        ColTotalDetalle.Nombre = "Total";
        ColTotalDetalle.Encabezado = "Total";
        ColTotalDetalle.Ancho = "80";
        ColTotalDetalle.Alineacion = "right";
        ColTotalDetalle.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColTotalDetalle);

        //ColPS
        CJQColumn ColPS = new CJQColumn();
        ColPS.Nombre = "PS";
        ColPS.Encabezado = "PS";
        ColPS.Ancho = "60";
        ColPS.Alineacion = "left";
        ColPS.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColPS);

        //Costo
        CJQColumn ColCosto = new CJQColumn();
        ColCosto.Nombre = "Costo";
        ColCosto.Encabezado = "Costo";
        ColCosto.Ancho = "65";
        ColCosto.Alineacion = "right";
        ColCosto.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColCosto);

        //Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "Descuento";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Ancho = "65";
        ColDescuento.Alineacion = "right";
        ColDescuento.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColDescuento);

        //Pedido
        CJQColumn ColPedido = new CJQColumn();
        ColPedido.Nombre = "IdPedido";
        ColPedido.Encabezado = "Pedido";
        ColPedido.Ancho = "45";
        ColPedido.Alineacion = "left";
        ColPedido.Buscador = "false";
        grdFacturaDetalleCliente.Columnas.Add(ColPedido);

        //IdPedidoDetalle
        CJQColumn ColPedidoDetalle = new CJQColumn();
        ColPedidoDetalle.Nombre = "IdPedidoDetalle";
        ColPedidoDetalle.Encabezado = "PedidoDetalle";
        ColPedidoDetalle.Ancho = "45";
        ColPedidoDetalle.Alineacion = "left";
        ColPedidoDetalle.Buscador = "false";
        ColPedidoDetalle.Oculto = "true";
        grdFacturaDetalleCliente.Columnas.Add(ColPedidoDetalle);

        //IdEncabezadoRemision
        CJQColumn ColIdERemision = new CJQColumn();
        ColIdERemision.Nombre = "IdEncabezadoRemision";
        ColIdERemision.Encabezado = "IdEncabezadoRemision";
        ColIdERemision.Ancho = "65";
        ColIdERemision.Alineacion = "left";
        ColIdERemision.Buscador = "false";
        ColIdERemision.Oculto = "true";
        grdFacturaDetalleCliente.Columnas.Add(ColIdERemision);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturaDetalleCliente", grdFacturaDetalleCliente.GeneraGrid(), true);

        //grdRemision
        CJQGrid grdRemision = new CJQGrid();
        grdRemision.NombreTabla = "grdRemision";
        grdRemision.CampoIdentificador = "IdEncabezadoRemision";
        grdRemision.ColumnaOrdenacion = "Folio";
        grdRemision.Metodo = "ObtenerConsultaDocumentoRemision";
        grdRemision.TituloTabla = "Remisiones";
        grdRemision.GenerarGridCargaInicial = false;
        grdRemision.GenerarFuncionFiltro = false;
        grdRemision.GenerarFuncionTerminado = false;
        grdRemision.NumeroRegistros = 10;
        grdRemision.RangoNumeroRegistros = "10,30,100";
        grdRemision.Altura = 150;


        //IdEncabezadoRemision
        CJQColumn ColIdEncabezadoRemision = new CJQColumn();
        ColIdEncabezadoRemision.Nombre = "IdEncabezadoRemision";
        ColIdEncabezadoRemision.Oculto = "true";
        ColIdEncabezadoRemision.Encabezado = "IdEncabezadoRemision";
        ColIdEncabezadoRemision.Buscador = "false";
        grdRemision.Columnas.Add(ColIdEncabezadoRemision);

        //Folio
        CJQColumn ColNumeroRemision = new CJQColumn();
        ColNumeroRemision.Nombre = "Folio";
        ColNumeroRemision.Encabezado = "No.Remision";
        ColNumeroRemision.Ancho = "65";
        ColNumeroRemision.Alineacion = "left";
        ColNumeroRemision.Buscador = "false";
        grdRemision.Columnas.Add(ColNumeroRemision);

        //Producto
        CJQColumn ColProductoRemision = new CJQColumn();
        ColProductoRemision.Nombre = "Producto";
        ColProductoRemision.Encabezado = "Producto";
        ColProductoRemision.Ancho = "321";
        ColProductoRemision.Alineacion = "left";
        ColProductoRemision.Buscador = "false";
        grdRemision.Columnas.Add(ColProductoRemision);

        //Cantidad
        CJQColumn ColCantidadRemision = new CJQColumn();
        ColCantidadRemision.Nombre = "Cantidad";
        ColCantidadRemision.Encabezado = "Cantidad";
        ColCantidadRemision.Ancho = "70";
        ColCantidadRemision.Alineacion = "left";
        ColCantidadRemision.Buscador = "false";
        grdRemision.Columnas.Add(ColCantidadRemision);

        //PrecioUnitario
        CJQColumn ColPrecioRemision = new CJQColumn();
        ColPrecioRemision.Nombre = "PrecioUnitario";
        ColPrecioRemision.Encabezado = "Precio";
        ColPrecioRemision.Ancho = "152";
        ColPrecioRemision.Alineacion = "right";
        ColPrecioRemision.Buscador = "false";
        grdRemision.Columnas.Add(ColPrecioRemision);

        //Monto
        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Ancho = "152";
        ColMonto.Alineacion = "left";
        ColMonto.Buscador = "false";
        grdRemision.Columnas.Add(ColMonto);

        //Pedido
        CJQColumn ColPedidoRemision = new CJQColumn();
        ColPedidoRemision.Nombre = "IdEncabezadoPedido";
        ColPedidoRemision.Encabezado = "Pedido";
        ColPedidoRemision.Ancho = "152";
        ColPedidoRemision.Alineacion = "left";
        ColPedidoRemision.Buscador = "false";
        grdRemision.Columnas.Add(ColPedidoRemision);

        ClientScript.RegisterStartupScript(this.GetType(), "grdRemision", grdRemision.GeneraGrid(), true);

        //grdFacturaProveedor
        CJQGrid grdFacturaProveedor = new CJQGrid();
        grdFacturaProveedor.NombreTabla = "grdFacturaProveedor";
        grdFacturaProveedor.CampoIdentificador = "IdEncabezadoFacturaProveedor";
        grdFacturaProveedor.ColumnaOrdenacion = "ClienteProyecto";
        grdFacturaProveedor.Metodo = "ObtenerConsultaDocumentoFacturaProveedor";
        grdFacturaProveedor.TituloTabla = "Factura del proveedor";
        grdFacturaProveedor.GenerarGridCargaInicial = false;
        grdFacturaProveedor.GenerarFuncionFiltro = false;
        grdFacturaProveedor.GenerarFuncionTerminado = false;
        grdFacturaProveedor.NumeroRegistros = 10;
        grdFacturaProveedor.RangoNumeroRegistros = "10,30,100";
        grdFacturaProveedor.Altura = 150;
        //grdFacturaProveedor.EventoRegistroSeleccionado = "DetalleFacturaProveedorSeleccionado";

        //IdEncabezadoFacturaProveedor
        CJQColumn ColIdEncabezadoFacturaProveedor = new CJQColumn();
        ColIdEncabezadoFacturaProveedor.Nombre = "IdEncabezadoFacturaProveedor";
        ColIdEncabezadoFacturaProveedor.Oculto = "true";
        ColIdEncabezadoFacturaProveedor.Encabezado = "IdEncabezadoFacturaProveedor";
        ColIdEncabezadoFacturaProveedor.Buscador = "false";
        grdFacturaProveedor.Columnas.Add(ColIdEncabezadoFacturaProveedor);


        //NumeroFacturaProveedor
        CJQColumn ColNumeroFacturaProveedor = new CJQColumn();
        ColNumeroFacturaProveedor.Nombre = "NumeroFacturaProveedor";
        ColNumeroFacturaProveedor.Encabezado = "Factura";
        ColNumeroFacturaProveedor.Ancho = "91";
        ColNumeroFacturaProveedor.Alineacion = "left";
        ColNumeroFacturaProveedor.Buscador = "true";
        grdFacturaProveedor.Columnas.Add(ColNumeroFacturaProveedor);

        //Almacen
        CJQColumn ColAlmacenFP = new CJQColumn();
        ColAlmacenFP.Nombre = "Almacen";
        ColAlmacenFP.Encabezado = "Almacen";
        ColAlmacenFP.Ancho = "91";
        ColAlmacenFP.Alineacion = "left";
        ColAlmacenFP.Buscador = "false";
        grdFacturaProveedor.Columnas.Add(ColAlmacenFP);

        //IdOrdenCompraDetalle
        CJQColumn ColIdOrdenCompraDetalle = new CJQColumn();
        ColIdOrdenCompraDetalle.Nombre = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Encabezado = "OrdenCompra";
        ColIdOrdenCompraDetalle.Ancho = "91";
        ColIdOrdenCompraDetalle.Alineacion = "left";
        ColIdOrdenCompraDetalle.Buscador = "false";
        grdFacturaProveedor.Columnas.Add(ColIdOrdenCompraDetalle);

        //Total
        CJQColumn ColTotalProveedor = new CJQColumn();
        ColTotalProveedor.Nombre = "Total";
        ColTotalProveedor.Encabezado = "Total";
        ColTotalProveedor.Ancho = "91";
        ColTotalProveedor.Alineacion = "left";
        ColTotalProveedor.Buscador = "false";
        grdFacturaProveedor.Columnas.Add(ColTotalProveedor);

        //Saldo
        CJQColumn ColSaldoProveedor = new CJQColumn();
        ColSaldoProveedor.Nombre = "Saldo";
        ColSaldoProveedor.Encabezado = "Saldo";
        ColSaldoProveedor.Ancho = "91";
        ColSaldoProveedor.Alineacion = "left";
        ColSaldoProveedor.Buscador = "false";
        grdFacturaProveedor.Columnas.Add(ColSaldoProveedor);

        //Cheques
        CJQColumn ColCheques = new CJQColumn();
        ColCheques.Nombre = "Cheques";
        ColCheques.Encabezado = "Cheques";
        ColCheques.Buscador = "false";
        ColCheques.Ancho = "50";
        grdFacturaProveedor.Columnas.Add(ColCheques);

        //ColClienteProyecto
        CJQColumn ColClienteProyecto = new CJQColumn();
        ColClienteProyecto.Nombre = "ClienteProyecto";
        ColClienteProyecto.Encabezado = "ClienteProyecto";
        ColClienteProyecto.Ancho = "91";
        ColClienteProyecto.Alineacion = "left";
        ColClienteProyecto.Buscador = "false";
        grdFacturaProveedor.Columnas.Add(ColClienteProyecto);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturaProveedor", grdFacturaProveedor.GeneraGrid(), true);

        //grdDetalleFactura
        CJQGrid grdDetalleFactura = new CJQGrid();
        grdDetalleFactura.NombreTabla = "grdDetalleFactura";
        grdDetalleFactura.CampoIdentificador = "IdDetalleFactura";
        grdDetalleFactura.ColumnaOrdenacion = "DetalleFactura";
        grdDetalleFactura.Metodo = "ObtenerConsultaDocumentoDetalleFactura";
        grdDetalleFactura.TituloTabla = "Detalle de Factura";
        grdDetalleFactura.GenerarGridCargaInicial = false;
        grdDetalleFactura.GenerarFuncionFiltro = false;
        grdDetalleFactura.GenerarFuncionTerminado = false;
        grdDetalleFactura.NumeroRegistros = 10;
        grdDetalleFactura.RangoNumeroRegistros = "10,30,100";
        grdDetalleFactura.Altura = 150;

        //IdFacturaDetalle
        CJQColumn ColIdIdFacturaDetalle = new CJQColumn();
        ColIdIdFacturaDetalle.Nombre = "IdIdFacturaDetalle";
        ColIdIdFacturaDetalle.Oculto = "true";
        ColIdIdFacturaDetalle.Encabezado = "IdIdFacturaDetalle";
        ColIdIdFacturaDetalle.Buscador = "false";
        grdDetalleFactura.Columnas.Add(ColIdIdFacturaDetalle);


        //Clave
        CJQColumn ColClaves = new CJQColumn();
        ColClaves.Nombre = "Clave";
        ColClaves.Encabezado = "Clave";
        ColClaves.Ancho = "91";
        ColClaves.Alineacion = "left";
        ColClaves.Buscador = "false";
        grdDetalleFactura.Columnas.Add(ColClaves);

        //Descripcion
        CJQColumn ColDescripcionn = new CJQColumn();
        ColDescripcionn.Nombre = "Descripcion";
        ColDescripcionn.Encabezado = "Descripcion";
        ColDescripcionn.Ancho = "91";
        ColDescripcionn.Alineacion = "left";
        ColDescripcionn.Buscador = "false";
        grdDetalleFactura.Columnas.Add(ColDescripcionn);

        //Moneda
        CJQColumn ColMonedas = new CJQColumn();
        ColMonedas.Nombre = "Moneda";
        ColMonedas.Encabezado = "Moneda";
        ColMonedas.Ancho = "91";
        ColMonedas.Alineacion = "left";
        ColMonedas.Buscador = "false";
        grdDetalleFactura.Columnas.Add(ColMonedas);

        //Costo
        CJQColumn ColCostos = new CJQColumn();
        ColCostos.Nombre = "Costo";
        ColCostos.Encabezado = "Costo";
        ColCostos.Ancho = "91";
        ColCostos.Alineacion = "left";
        ColCostos.Buscador = "false";
        grdDetalleFactura.Columnas.Add(ColCostos);

        //Cantidad
        CJQColumn ColCantidadDetalle = new CJQColumn();
        ColCantidadDetalle.Nombre = "Cantidad";
        ColCantidadDetalle.Encabezado = "Cantidad";
        ColCantidadDetalle.Buscador = "false";
        ColCantidadDetalle.Ancho = "50";
        grdDetalleFactura.Columnas.Add(ColCantidadDetalle);
        
        //IVA
        CJQColumn ColIVA = new CJQColumn();
        ColIVA.Nombre = "IVA";
        ColIVA.Encabezado = "IVA %";
        ColIVA.Ancho = "91";
        ColIVA.Alineacion = "left";
        ColIVA.Buscador = "false";
        grdDetalleFactura.Columnas.Add(ColIVA);

        //NumeroSerie
        CJQColumn ColNumeroSerie = new CJQColumn();
        ColNumeroSerie.Nombre = "NumeroSerie";
        ColNumeroSerie.Encabezado = "Numero de Serie";
        ColNumeroSerie.Ancho = "91";
        ColNumeroSerie.Alineacion = "left";
        ColNumeroSerie.Buscador = "false";
        grdDetalleFactura.Columnas.Add(ColNumeroSerie);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleFactura", grdDetalleFactura.GeneraGrid(), true);

        //grdPedido
        CJQGrid grdPedido = new CJQGrid();
        grdPedido.NombreTabla = "grdPedido";
        grdPedido.CampoIdentificador = "IdPedido";
        grdPedido.ColumnaOrdenacion = "Descripcion";
        grdPedido.Metodo = "ObtenerConsultaDocumentoPedido";
        grdPedido.TituloTabla = "Pedidos";
        grdPedido.GenerarGridCargaInicial = false;
        grdPedido.GenerarFuncionFiltro = false;
        grdPedido.GenerarFuncionTerminado = false;
        grdPedido.NumeroRegistros = 10;
        grdPedido.RangoNumeroRegistros = "10,30,100";
        grdPedido.Altura = 150;

        //IdPedido
        CJQColumn ColIdPedido = new CJQColumn();
        ColIdPedido.Nombre = "IdPedido";
        ColIdPedido.Oculto = "true";
        ColIdPedido.Encabezado = "IdPedido";
        ColIdPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColIdPedido);

        //Pedido
        CJQColumn ColFolioPedido = new CJQColumn();
        ColFolioPedido.Nombre = "Pedido";
        ColFolioPedido.Encabezado = "Pedido";
        ColFolioPedido.Ancho = "65";
        ColFolioPedido.Alineacion = "left";
        ColFolioPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColFolioPedido);

        //Clave
        CJQColumn ColClavePedido = new CJQColumn();
        ColClavePedido.Nombre = "Clave";
        ColClavePedido.Encabezado = "Clave";
        ColClavePedido.Ancho = "65";
        ColClavePedido.Alineacion = "left";
        ColClavePedido.Buscador = "false";
        grdPedido.Columnas.Add(ColClavePedido);

        //Descripcion
        CJQColumn ColDescripcionPedido = new CJQColumn();
        ColDescripcionPedido.Nombre = "Descripcion";
        ColDescripcionPedido.Encabezado = "Descripcion";
        ColDescripcionPedido.Ancho = "321";
        ColDescripcionPedido.Alineacion = "left";
        ColDescripcionPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColDescripcionPedido);

        //Cantidad
        CJQColumn ColCantidadPedido = new CJQColumn();
        ColCantidadPedido.Nombre = "Cantidad";
        ColCantidadPedido.Encabezado = "Cantidad";
        ColCantidadPedido.Ancho = "70";
        ColCantidadPedido.Alineacion = "left";
        ColCantidadPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColCantidadPedido);

        //PrecioUnitario
        CJQColumn ColPrecioPedido = new CJQColumn();
        ColPrecioPedido.Nombre = "PrecioUnitario";
        ColPrecioPedido.Encabezado = "Precio";
        ColPrecioPedido.Ancho = "152";
        ColPrecioPedido.Alineacion = "right";
        ColPrecioPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColPrecioPedido);

        //Total
        CJQColumn ColTotalPedido = new CJQColumn();
        ColTotalPedido.Nombre = "Total";
        ColTotalPedido.Encabezado = "Total";
        ColTotalPedido.Ancho = "152";
        ColTotalPedido.Alineacion = "left";
        ColTotalPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColTotalPedido);

        //Cotizador
        CJQColumn ColUsuarioCotizador = new CJQColumn();
        ColUsuarioCotizador.Nombre = "Cotizador";
        ColUsuarioCotizador.Encabezado = "Cotizador";
        ColUsuarioCotizador.Ancho = "300";
        ColUsuarioCotizador.Alineacion = "left";
        ColUsuarioCotizador.Buscador = "false";
        grdPedido.Columnas.Add(ColUsuarioCotizador);

        //Agente
        CJQColumn ColUsuarioAgente = new CJQColumn();
        ColUsuarioAgente.Nombre = "Agente";
        ColUsuarioAgente.Encabezado = "Agente";
        ColUsuarioAgente.Ancho = "300";
        ColUsuarioAgente.Alineacion = "left";
        ColUsuarioAgente.Buscador = "false";
        grdPedido.Columnas.Add(ColUsuarioAgente);

        //PS
        CJQColumn ColPSPedido = new CJQColumn();
        ColPSPedido.Nombre = "PS";
        ColPSPedido.Encabezado = "PS";
        ColPSPedido.Ancho = "152";
        ColPSPedido.Alineacion = "left";
        ColPSPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColPSPedido);

        //Saldo
        CJQColumn ColSaldoPedido = new CJQColumn();
        ColSaldoPedido.Nombre = "Saldo";
        ColSaldoPedido.Encabezado = "Saldo";
        ColSaldoPedido.Ancho = "152";
        ColSaldoPedido.Alineacion = "left";
        ColSaldoPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColSaldoPedido);

        //Estado
        CJQColumn ColEstadoPedido = new CJQColumn();
        ColEstadoPedido.Nombre = "EstatusCotizacion";
        ColEstadoPedido.Encabezado = "Estado";
        ColEstadoPedido.Ancho = "152";
        ColEstadoPedido.Alineacion = "left";
        ColEstadoPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColEstadoPedido);

        //Descuento
        CJQColumn ColDescuentoPedido = new CJQColumn();
        ColDescuentoPedido.Nombre = "Descuento";
        ColDescuentoPedido.Encabezado = "Descuento";
        ColDescuentoPedido.Ancho = "152";
        ColDescuentoPedido.Alineacion = "left";
        ColDescuentoPedido.Buscador = "false";
        grdPedido.Columnas.Add(ColDescuentoPedido);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPedido", grdPedido.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerConsultaDocumentoFacturaCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerieFactura, string pNumeroFactura, string pFechaInicio, string pFechaFin)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdConsultaDocumentoFacturaCliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSerieFactura", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieFactura);
        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = idSucursal;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = 0;
        Stored.Parameters.Add("pFechaInicio", SqlDbType.VarChar, 10).Value = pFechaInicio;
        Stored.Parameters.Add("pFechaFin", SqlDbType.VarChar, 10).Value = pFechaFin;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerConsultaDocumentoFacturaDetalleCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdFacturaEncabezado, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdConsultaDocumentoFacturaDetalleCliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdFacturaEncabezado", SqlDbType.Int).Value = pIdFacturaEncabezado;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerConsultaDocumentoRemision(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPedidoDetalle, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdConsultaDocumentoRemision", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdPedidoDetalle", SqlDbType.Int).Value = pIdPedidoDetalle;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerConsultaDetalleFactura(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPedidoDetalle)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_ConsultaDocumento_FacturaDetalle", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdPedidoDetalle", SqlDbType.Int).Value = pIdPedidoDetalle;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerConsultaDocumentoFacturaProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPedidoDetalle, string pNumeroFacturaProveedor, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdConsultaDocumentoFacturaProveedor", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdPedidoDetalle", SqlDbType.Int).Value = pIdPedidoDetalle;
        Stored.Parameters.Add("pNumeroFacturaProveedor", SqlDbType.VarChar, 20).Value = pNumeroFacturaProveedor;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerConsultaDocumentoPedido(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPedidoDetalle, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdConsultaDocumentoPedido", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdPedidoDetalle", SqlDbType.Int).Value = pIdPedidoDetalle;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarSerieFactura(string pSerieFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(idUsuario, ConexionBaseDatos);

        CJson jsonSerieFactura = new CJson();
        jsonSerieFactura.StoredProcedure.CommandText = "sp_ConsultaDocumento_ConsultarFiltrosGridFacturaCliente";
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("@pSerieFactura", pSerieFactura);
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        jsonSerieFactura.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        string oRespuesta = jsonSerieFactura.ObtenerJsonString(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta;
    }

    [WebMethod]
    public static string BuscarNumeroFactura(string pNumeroFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(idUsuario, ConexionBaseDatos);

        CJson jsonNumeroFactura = new CJson();
        jsonNumeroFactura.StoredProcedure.CommandText = "sp_ConsultaDocumento_ConsultarFiltrosGridFacturaCliente";
        jsonNumeroFactura.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonNumeroFactura.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", pNumeroFactura);
        jsonNumeroFactura.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        jsonNumeroFactura.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        return jsonNumeroFactura.ObtenerJsonString(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string BuscarNumeroFacturaProveedor(string pNumeroFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(idUsuario, ConexionBaseDatos);

        CJson jsonNumeroFacturaProveedor = new CJson();
        jsonNumeroFacturaProveedor.StoredProcedure.CommandText = "sp_ConsultaDocumento_ConsultarFiltrosGridFacturaProveeedor";
        jsonNumeroFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonNumeroFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pNumeroFacturaProveedor", pNumeroFacturaProveedor);
        jsonNumeroFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        jsonNumeroFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        return jsonNumeroFacturaProveedor.ObtenerJsonString(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }
}