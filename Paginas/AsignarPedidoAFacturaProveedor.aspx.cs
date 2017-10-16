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

public partial class AsignarPedidoAFacturaProveedor : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    private static int idEmpresa;
    public static int accesoAsiganarPedidoAFacturaProveedor = 0;
    public static int puedeAgregarPedidoAFacturaProveedor = 0;
    public static int puedeEditarPedidoAFacturaProveedor = 0;
    public static int puedeEliminarPedidoAFacturaProveedor = 0;
    public static int puedeConsultarPedidoAFacturaProveedor = 0;

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

        if (Usuario.TienePermisos(new string[] { "puedeAgregarPedidoAFacturaProveedor" }, ConexionBaseDatos) == "") { puedeAgregarPedidoAFacturaProveedor = 1; }
        else { puedeAgregarPedidoAFacturaProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarPedidoAFacturaProveedor" }, ConexionBaseDatos) == "") { puedeEditarPedidoAFacturaProveedor = 1; }
        else { puedeEditarPedidoAFacturaProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEliminarPedidoAFacturaProveedor" }, ConexionBaseDatos) == "") { puedeEliminarPedidoAFacturaProveedor = 1; }
        else { puedeEliminarPedidoAFacturaProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeConsultarPedidoAFacturaProveedor" }, ConexionBaseDatos) == "") { puedeConsultarPedidoAFacturaProveedor = 1; }
        else { puedeConsultarPedidoAFacturaProveedor = 0; }

        //GridPedidoFacturaProveedor
        CJQGrid GridPedidoFacturaProveedor = new CJQGrid();
        GridPedidoFacturaProveedor.NombreTabla = "grdPedidoFacturaProveedor";
        GridPedidoFacturaProveedor.CampoIdentificador = "IdDetalleFacturaProveedor";
        GridPedidoFacturaProveedor.ColumnaOrdenacion = "NumeroFactura";
        GridPedidoFacturaProveedor.Metodo = "ObtenerPedidoFacturaProveedor";
        GridPedidoFacturaProveedor.TituloTabla = "Material recibido sin asignar";
        GridPedidoFacturaProveedor.NumeroFila = true;
        GridPedidoFacturaProveedor.NumeroRegistros = 15;
        GridPedidoFacturaProveedor.RangoNumeroRegistros = "15,30,60";

        //IdIdDetalleFacturaProveedor
        CJQColumn ColIdDetalleFacturaProveedor = new CJQColumn();
        ColIdDetalleFacturaProveedor.Nombre = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Oculto = "true";
        ColIdDetalleFacturaProveedor.Encabezado = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Buscador = "false";
        GridPedidoFacturaProveedor.Columnas.Add(ColIdDetalleFacturaProveedor);

        //NumeroFactura
        CJQColumn ColNumeroFactura = new CJQColumn();
        ColNumeroFactura.Nombre = "NumeroFactura";
        ColNumeroFactura.Encabezado = "NoFactura";
        ColNumeroFactura.Buscador = "true";
        ColNumeroFactura.Alineacion = "left";
        ColNumeroFactura.Ancho = "65";
        GridPedidoFacturaProveedor.Columnas.Add(ColNumeroFactura);

        //Tipo
        CJQColumn ColTipo = new CJQColumn();
        ColTipo.Nombre = "Tipo";
        ColTipo.Encabezado = "Tipo";
        ColTipo.Buscador = "false";
        ColTipo.Alineacion = "left";
        ColTipo.Ancho = "65";
        GridPedidoFacturaProveedor.Columnas.Add(ColTipo);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Buscador = "false";
        ColClave.Alineacion = "left";
        ColClave.Ancho = "65";
        GridPedidoFacturaProveedor.Columnas.Add(ColClave);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "100";
        GridPedidoFacturaProveedor.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        ColCantidad.Ancho = "60";
        GridPedidoFacturaProveedor.Columnas.Add(ColCantidad);

        //Precio
        CJQColumn ColPrecio = new CJQColumn();
        ColPrecio.Nombre = "Precio";
        ColPrecio.Encabezado = "Precio";
        ColPrecio.Buscador = "false";
        ColPrecio.Formato = "FormatoMoneda";
        ColPrecio.Alineacion = "right";
        ColPrecio.Ancho = "60";
        GridPedidoFacturaProveedor.Columnas.Add(ColPrecio);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "60";
        GridPedidoFacturaProveedor.Columnas.Add(ColTotal);

        //UCV
        CJQColumn ColUCV = new CJQColumn();
        ColUCV.Nombre = "UCV";
        ColUCV.Encabezado = "UCV";
        ColUCV.Buscador = "false";
        ColUCV.Alineacion = "left";
        ColUCV.Ancho = "60";
        GridPedidoFacturaProveedor.Columnas.Add(ColUCV);

        //Nota
        CJQColumn ColNota = new CJQColumn();
        ColNota.Nombre = "Nota";
        ColNota.Encabezado = "Nota";
        ColNota.Buscador = "false";
        ColNota.Alineacion = "left";
        ColNota.Ancho = "100";
        GridPedidoFacturaProveedor.Columnas.Add(ColNota);

        //NumeroSerie
        CJQColumn ColNumeroSerie = new CJQColumn();
        ColNumeroSerie.Nombre = "NumeroSerie";
        ColNumeroSerie.Encabezado = "NoSerie";
        ColNumeroSerie.Buscador = "false";
        ColNumeroSerie.Alineacion = "left";
        ColNumeroSerie.Ancho = "65";
        GridPedidoFacturaProveedor.Columnas.Add(ColNumeroSerie);

        //Cliente
        CJQColumn ColCliente = new CJQColumn();
        ColCliente.Nombre = "Cliente";
        ColCliente.Encabezado = "Cliente";
        ColCliente.Buscador = "false";
        ColCliente.Alineacion = "left";
        ColCliente.Ancho = "100";
        GridPedidoFacturaProveedor.Columnas.Add(ColCliente);

        //Almacen
        CJQColumn ColAlmacen = new CJQColumn();
        ColAlmacen.Nombre = "Almacen";
        ColAlmacen.Encabezado = "Almacen";
        ColAlmacen.Buscador = "false";
        ColAlmacen.Alineacion = "left";
        ColAlmacen.Ancho = "100";
        GridPedidoFacturaProveedor.Columnas.Add(ColAlmacen);

        //TipoCompra
        CJQColumn ColTipoCompra = new CJQColumn();
        ColTipoCompra.Nombre = "TipoCompra";
        ColTipoCompra.Encabezado = "Tipo Compra";
        ColTipoCompra.Buscador = "false";
        ColTipoCompra.Alineacion = "left";
        ColTipoCompra.Ancho = "65";
        GridPedidoFacturaProveedor.Columnas.Add(ColTipoCompra);

        //Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "Descuento";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Buscador = "false";
        ColDescuento.Alineacion = "left";
        ColDescuento.Ancho = "65";
        GridPedidoFacturaProveedor.Columnas.Add(ColDescuento);

        //ReferenciaEntrega
        CJQColumn ColReferenciaEntrega = new CJQColumn();
        ColReferenciaEntrega.Nombre = "ReferenciaEntrega";
        ColReferenciaEntrega.Encabezado = "Ref. Entrega";
        ColReferenciaEntrega.Buscador = "false";
        ColReferenciaEntrega.Alineacion = "left";
        ColReferenciaEntrega.Ancho = "100";
        GridPedidoFacturaProveedor.Columnas.Add(ColReferenciaEntrega);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "Fecha";
        ColFecha.Encabezado = "Fecha";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridPedidoFacturaProveedor.Columnas.Add(ColFecha);
        ClientScript.RegisterStartupScript(this.GetType(), "grdPedidoFacturaProveedor", GridPedidoFacturaProveedor.GeneraGrid(), true);

        //GridFacturaProveedorSinAsignacionPedido
        CJQGrid GridFacturaProveedorSinAsignacionPedido = new CJQGrid();
        GridFacturaProveedorSinAsignacionPedido.NombreTabla = "grdFacturaProveedorSinAsignacionPedido";
        GridFacturaProveedorSinAsignacionPedido.CampoIdentificador = "IdDetalleFacturaProveedor";
        GridFacturaProveedorSinAsignacionPedido.ColumnaOrdenacion = "NumeroFactura";
        GridFacturaProveedorSinAsignacionPedido.TipoOrdenacion = "ASC";
        GridFacturaProveedorSinAsignacionPedido.Metodo = "ObtenerFacturaProveedorSinAsignacionPedido";
        GridFacturaProveedorSinAsignacionPedido.TituloTabla = "Material recibido sin asignación de pedido";
        GridFacturaProveedorSinAsignacionPedido.GenerarGridCargaInicial = false;
        GridFacturaProveedorSinAsignacionPedido.GenerarFuncionFiltro = false;
        GridFacturaProveedorSinAsignacionPedido.GenerarFuncionTerminado = false;
        GridFacturaProveedorSinAsignacionPedido.NumeroFila = true;
        GridFacturaProveedorSinAsignacionPedido.Altura = 350;
        GridFacturaProveedorSinAsignacionPedido.Ancho = 1462;
        GridFacturaProveedorSinAsignacionPedido.NumeroRegistros = 15;
        GridFacturaProveedorSinAsignacionPedido.RangoNumeroRegistros = "15,30,60";

        //IdIdDetalleFacturaProveedor
        CJQColumn ColIdDetalleFacturaProveedorAsignar = new CJQColumn();
        ColIdDetalleFacturaProveedorAsignar.Nombre = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedorAsignar.Oculto = "true";
        ColIdDetalleFacturaProveedorAsignar.Encabezado = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedorAsignar.Buscador = "false";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColIdDetalleFacturaProveedorAsignar);

        //NumeroFactura
        CJQColumn ColNumeroFacturaAsignar = new CJQColumn();
        ColNumeroFacturaAsignar.Nombre = "NumeroFacturaAsignar";
        ColNumeroFacturaAsignar.Encabezado = "NoFactura";
        ColNumeroFacturaAsignar.Buscador = "true";
        ColNumeroFacturaAsignar.Alineacion = "left";
        ColNumeroFacturaAsignar.Ancho = "67";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColNumeroFacturaAsignar);

        //Tipo
        CJQColumn ColTipoAsignar = new CJQColumn();
        ColTipoAsignar.Nombre = "Tipo";
        ColTipoAsignar.Encabezado = "Tipo";
        ColTipoAsignar.Buscador = "false";
        ColTipoAsignar.Alineacion = "left";
        ColTipoAsignar.Ancho = "58";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColTipoAsignar);

        //Clave
        CJQColumn ColClaveAsignar = new CJQColumn();
        ColClaveAsignar.Nombre = "Clave";
        ColClaveAsignar.Encabezado = "Clave";
        ColClaveAsignar.Buscador = "false";
        ColClaveAsignar.Alineacion = "left";
        ColClaveAsignar.Ancho = "100";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColClaveAsignar);

        //Descripcion
        CJQColumn ColDescripcionAsignar = new CJQColumn();
        ColDescripcionAsignar.Nombre = "Descripcion";
        ColDescripcionAsignar.Encabezado = "Descripcion";
        ColDescripcionAsignar.Buscador = "false";
        ColDescripcionAsignar.Alineacion = "left";
        ColDescripcionAsignar.Ancho = "100";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColDescripcionAsignar);

        //Cantidad
        CJQColumn ColCantidadAsignar = new CJQColumn();
        ColCantidadAsignar.Nombre = "Cantidad";
        ColCantidadAsignar.Encabezado = "Cantidad";
        ColCantidadAsignar.Buscador = "false";
        ColCantidadAsignar.Alineacion = "left";
        ColCantidadAsignar.Ancho = "60";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColCantidadAsignar);

        //Precio
        CJQColumn ColPrecioAsignar = new CJQColumn();
        ColPrecioAsignar.Nombre = "Precio";
        ColPrecioAsignar.Encabezado = "Precio";
        ColPrecioAsignar.Buscador = "false";
        ColPrecioAsignar.Formato = "FormatoMoneda";
        ColPrecioAsignar.Alineacion = "right";
        ColPrecioAsignar.Ancho = "85";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColPrecioAsignar);

        //Total
        CJQColumn ColTotalAsignar = new CJQColumn();
        ColTotalAsignar.Nombre = "Total";
        ColTotalAsignar.Encabezado = "Total";
        ColTotalAsignar.Buscador = "false";
        ColTotalAsignar.Formato = "FormatoMoneda";
        ColTotalAsignar.Alineacion = "right";
        ColTotalAsignar.Ancho = "85";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColTotalAsignar);

        //UCV
        CJQColumn ColUCVAsignar = new CJQColumn();
        ColUCVAsignar.Nombre = "UCV";
        ColUCVAsignar.Encabezado = "UCV";
        ColUCVAsignar.Buscador = "false";
        ColUCVAsignar.Alineacion = "left";
        ColUCVAsignar.Ancho = "50";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColUCVAsignar);

        //Nota
        CJQColumn ColNotaAsignar = new CJQColumn();
        ColNotaAsignar.Nombre = "Nota";
        ColNotaAsignar.Encabezado = "Nota";
        ColNotaAsignar.Buscador = "false";
        ColNotaAsignar.Alineacion = "left";
        ColNotaAsignar.Ancho = "64";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColNotaAsignar);

        //NumeroSerie
        CJQColumn ColNumeroSerieAsignar = new CJQColumn();
        ColNumeroSerieAsignar.Nombre = "NumeroSerie";
        ColNumeroSerieAsignar.Encabezado = "NoSerie";
        ColNumeroSerieAsignar.Buscador = "false";
        ColNumeroSerieAsignar.Alineacion = "left";
        ColNumeroSerieAsignar.Ancho = "64";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColNumeroSerieAsignar);

        //Cliente
        CJQColumn ColClienteAsignar = new CJQColumn();
        ColClienteAsignar.Nombre = "Cliente";
        ColClienteAsignar.Encabezado = "Cliente";
        ColClienteAsignar.Buscador = "false";
        ColClienteAsignar.Alineacion = "left";
        ColClienteAsignar.Ancho = "100";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColClienteAsignar);

        //Almacen
        CJQColumn ColAlmacenAsignar = new CJQColumn();
        ColAlmacenAsignar.Nombre = "Almacen";
        ColAlmacenAsignar.Encabezado = "Almacen";
        ColAlmacenAsignar.Buscador = "false";
        ColAlmacenAsignar.Alineacion = "left";
        ColAlmacenAsignar.Ancho = "100";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColAlmacenAsignar);


        //Saldo
        CJQColumn ColSaldoAsignar = new CJQColumn();
        ColSaldoAsignar.Nombre = "Saldo";
        ColSaldoAsignar.Encabezado = "Saldo";
        ColSaldoAsignar.Buscador = "false";
        ColSaldoAsignar.Formato = "FormatoMoneda";
        ColSaldoAsignar.Alineacion = "right";
        ColSaldoAsignar.Ancho = "64";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColSaldoAsignar);

        //ClienteProyecto
        CJQColumn ColClienteProyectoAsignar = new CJQColumn();
        ColClienteProyectoAsignar.Nombre = "ClienteProyecto";
        ColClienteProyectoAsignar.Encabezado = "ClienteProyecto";
        ColClienteProyectoAsignar.Buscador = "false";
        ColClienteProyectoAsignar.Alineacion = "left";
        ColClienteProyectoAsignar.Ancho = "100";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColClienteProyectoAsignar);

        //TipoCompra
        CJQColumn ColTipoCompraAsignar = new CJQColumn();
        ColTipoCompraAsignar.Nombre = "TipoCompra";
        ColTipoCompraAsignar.Encabezado = "Tipo Compra";
        ColTipoCompraAsignar.Buscador = "false";
        ColTipoCompraAsignar.Alineacion = "left";
        ColTipoCompraAsignar.Ancho = "78";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColTipoCompraAsignar);

        //Descuento
        CJQColumn ColDescuentoAsignar = new CJQColumn();
        ColDescuentoAsignar.Nombre = "Descuento";
        ColDescuentoAsignar.Encabezado = "Descuento";
        ColDescuentoAsignar.Buscador = "false";
        ColDescuentoAsignar.Alineacion = "left";
        ColDescuentoAsignar.Ancho = "64";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColDescuentoAsignar);

        //ReferenciaEntrega
        CJQColumn ColReferenciaEntregaAsignar = new CJQColumn();
        ColReferenciaEntregaAsignar.Nombre = "ReferenciaEntrega";
        ColReferenciaEntregaAsignar.Encabezado = "Ref. Entrega";
        ColReferenciaEntregaAsignar.Buscador = "false";
        ColReferenciaEntregaAsignar.Alineacion = "left";
        ColReferenciaEntregaAsignar.Ancho = "85";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColReferenciaEntregaAsignar);

        //Fecha
        CJQColumn ColFechaAsignar = new CJQColumn();
        ColFechaAsignar.Nombre = "Fecha";
        ColFechaAsignar.Encabezado = "Fecha";
        ColFechaAsignar.Buscador = "false";
        ColFechaAsignar.Alineacion = "left";
        ColFechaAsignar.Ancho = "85";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColFechaAsignar);

        //IdPedidoDetalle
        CJQColumn ColIdPedidoDetalleAsignar = new CJQColumn();
        ColIdPedidoDetalleAsignar.Nombre = "IdPedidoDetalle";
        ColIdPedidoDetalleAsignar.Encabezado = "IdPedidoDetalle";
        ColIdPedidoDetalleAsignar.Buscador = "false";
        ColIdPedidoDetalleAsignar.Alineacion = "left";
        ColIdPedidoDetalleAsignar.Oculto = "true";
        ColIdPedidoDetalleAsignar.Ancho = "53";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColIdPedidoDetalleAsignar);

        //Número de pedido
        CJQColumn ColPedido = new CJQColumn();
        ColPedido.Nombre = "Pedido";
        ColPedido.Encabezado = "Pedido";
        ColPedido.Buscador = "false";
        ColPedido.Alineacion = "left";
        ColPedido.Ancho = "53";
        GridFacturaProveedorSinAsignacionPedido.Columnas.Add(ColPedido);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturaProveedorSinAsignacionPedido", GridFacturaProveedorSinAsignacionPedido.GeneraGrid(), true);

        //GridPedidoSinAsignar
        CJQGrid GridPedidoSinAsignar = new CJQGrid();
        GridPedidoSinAsignar.NombreTabla = "grdPedidoSinAsignar";
        GridPedidoSinAsignar.CampoIdentificador = "IdPedido";
        GridPedidoSinAsignar.ColumnaOrdenacion = "Folio";
        GridPedidoSinAsignar.TipoOrdenacion = "ASC";
        GridPedidoSinAsignar.Metodo = "ObtenerPedidoSinAsignar";
        GridPedidoSinAsignar.TituloTabla = "Pedidos sin asignar";
        GridPedidoSinAsignar.GenerarGridCargaInicial = false;
        GridPedidoSinAsignar.GenerarFuncionFiltro = false;
        GridPedidoSinAsignar.GenerarFuncionTerminado = false;
        GridPedidoSinAsignar.NumeroFila = true;
        GridPedidoSinAsignar.Altura = 350;
        GridPedidoSinAsignar.Ancho = 1024;
        GridPedidoSinAsignar.NumeroRegistros = 15;
        GridPedidoSinAsignar.RangoNumeroRegistros = "15,30,60";

        //IdDetallePedido
        CJQColumn ColIdPedidoDetalle = new CJQColumn();
        ColIdPedidoDetalle.Nombre = "IdPedidoDetalle";
        ColIdPedidoDetalle.Oculto = "true";
        ColIdPedidoDetalle.Encabezado = "IdPedidoDetalle";
        ColIdPedidoDetalle.Buscador = "false";
        GridPedidoSinAsignar.Columnas.Add(ColIdPedidoDetalle);

        //IdPedido
        CJQColumn ColIdPedido = new CJQColumn();
        ColIdPedido.Nombre = "IdPedido";
        ColIdPedido.Oculto = "true";
        ColIdPedido.Encabezado = "IdPedido";
        ColIdPedido.Buscador = "false";
        GridPedidoSinAsignar.Columnas.Add(ColIdPedido);

        //Folio
        CJQColumn ColFolioPedido = new CJQColumn();
        ColFolioPedido.Nombre = "Folio";
        ColFolioPedido.Encabezado = "Número";
        ColFolioPedido.Buscador = "true";
        ColFolioPedido.Alineacion = "left";
        ColFolioPedido.Ancho = "65";
        GridPedidoSinAsignar.Columnas.Add(ColFolioPedido);

        //Clave
        CJQColumn ColClavePedido = new CJQColumn();
        ColClavePedido.Nombre = "Clave";
        ColClavePedido.Encabezado = "Clave";
        ColClavePedido.Buscador = "true";
        ColClavePedido.Alineacion = "left";
        ColClavePedido.Ancho = "65";
        GridPedidoSinAsignar.Columnas.Add(ColClavePedido);

        //Descripcion
        CJQColumn ColDescripcionPedido = new CJQColumn();
        ColDescripcionPedido.Nombre = "Descripcion";
        ColDescripcionPedido.Encabezado = "Descripcion";
        ColDescripcionPedido.Buscador = "true";
        ColDescripcionPedido.Alineacion = "left";
        ColDescripcionPedido.Ancho = "100";
        GridPedidoSinAsignar.Columnas.Add(ColDescripcionPedido);

        //Sucursal
        CJQColumn ColSucursalPedido = new CJQColumn();
        ColSucursalPedido.Nombre = "Sucursal";
        ColSucursalPedido.Encabezado = "Sucursal";
        ColSucursalPedido.Alineacion = "left";
        ColSucursalPedido.Ancho = "100";
        ColSucursalPedido.Buscador = "true";
        ColSucursalPedido.TipoBuscador = "Combo";
        ColSucursalPedido.StoredProcedure.CommandText = "sp_ConsultarFiltros_Oportunidad_Sucursal";
        GridPedidoSinAsignar.Columnas.Add(ColSucursalPedido);

        //Cantidad
        CJQColumn ColCantidadPedido = new CJQColumn();
        ColCantidadPedido.Nombre = "Cantidad";
        ColCantidadPedido.Encabezado = "Cantidad";
        ColCantidadPedido.Buscador = "false";
        ColCantidadPedido.Alineacion = "left";
        ColCantidadPedido.Ancho = "60";
        GridPedidoSinAsignar.Columnas.Add(ColCantidadPedido);

        //Precio
        CJQColumn ColPrecioPedido = new CJQColumn();
        ColPrecioPedido.Nombre = "PrecioUnitario";
        ColPrecioPedido.Encabezado = "PrecioU";
        ColPrecioPedido.Buscador = "false";
        ColPrecioPedido.Formato = "FormatoMoneda";
        ColPrecioPedido.Alineacion = "right";
        ColPrecioPedido.Ancho = "60";
        GridPedidoSinAsignar.Columnas.Add(ColPrecioPedido);

        //Total
        CJQColumn ColTotalPedido = new CJQColumn();
        ColTotalPedido.Nombre = "Total";
        ColTotalPedido.Encabezado = "Total";
        ColTotalPedido.Buscador = "false";
        ColTotalPedido.Formato = "FormatoMoneda";
        ColTotalPedido.Alineacion = "right";
        ColTotalPedido.Ancho = "60";
        GridPedidoSinAsignar.Columnas.Add(ColTotalPedido);

        //Tipo
        CJQColumn ColTipoPedido = new CJQColumn();
        ColTipoPedido.Nombre = "Tipo";
        ColTipoPedido.Encabezado = "Tipo";
        ColTipoPedido.Buscador = "false";
        ColTipoPedido.Alineacion = "left";
        ColTipoPedido.Ancho = "65";
        GridPedidoSinAsignar.Columnas.Add(ColTipoPedido);

        //Descuento
        CJQColumn ColDescuentoPedido = new CJQColumn();
        ColDescuentoPedido.Nombre = "Descuento";
        ColDescuentoPedido.Encabezado = "Descuento";
        ColDescuentoPedido.Buscador = "false";
        ColDescuentoPedido.Alineacion = "left";
        ColDescuentoPedido.Ancho = "65";
        GridPedidoSinAsignar.Columnas.Add(ColDescuentoPedido);


        //FechaFacturacion
        CJQColumn ColFechaPedido = new CJQColumn();
        ColFechaPedido.Nombre = "FechaAlta";
        ColFechaPedido.Encabezado = "Fecha";
        ColFechaPedido.Buscador = "false";
        ColFechaPedido.Alineacion = "left";
        ColFechaPedido.Ancho = "80";
        GridPedidoSinAsignar.Columnas.Add(ColFechaPedido);
        ClientScript.RegisterStartupScript(this.GetType(), "grdPedidoSinAsignar", GridPedidoSinAsignar.GeneraGrid(), true);

        //GridProyectoSinAsignar
        CJQGrid GridProyectoSinAsignar = new CJQGrid();
        GridProyectoSinAsignar.NombreTabla = "grdProyectoSinAsignar";
        GridProyectoSinAsignar.CampoIdentificador = "IdProyecto";
        GridProyectoSinAsignar.ColumnaOrdenacion = "NombreProyecto";
        GridProyectoSinAsignar.TipoOrdenacion = "ASC";
        GridProyectoSinAsignar.Metodo = "ObtenerProyectoSinAsignar";
        GridProyectoSinAsignar.TituloTabla = "Proyectos sin asignar";
        GridProyectoSinAsignar.GenerarGridCargaInicial = false;
        GridProyectoSinAsignar.GenerarFuncionFiltro = false;
        GridProyectoSinAsignar.GenerarFuncionTerminado = false;
        GridProyectoSinAsignar.NumeroFila = true;
        GridProyectoSinAsignar.Altura = 350;
        GridProyectoSinAsignar.Ancho = 1024;
        GridProyectoSinAsignar.NumeroRegistros = 15;
        GridProyectoSinAsignar.RangoNumeroRegistros = "15,30,60";

        //IdProyecto
        CJQColumn ColIdProyecto = new CJQColumn();
        ColIdProyecto.Nombre = "IdProyecto";
        ColIdProyecto.Encabezado = "IdProyecto";
        ColIdProyecto.Buscador = "true";
        ColIdProyecto.Alineacion = "left";
        ColIdProyecto.Ancho = "40";
        GridProyectoSinAsignar.Columnas.Add(ColIdProyecto);

        //NombreProyecto
        CJQColumn ColNombreProyecto = new CJQColumn();
        ColNombreProyecto.Nombre = "NombreProyecto";
        ColNombreProyecto.Encabezado = "Nombre";
        ColNombreProyecto.Buscador = "true";
        ColNombreProyecto.Alineacion = "left";
        ColNombreProyecto.Ancho = "65";
        GridProyectoSinAsignar.Columnas.Add(ColNombreProyecto);

        //FechaInicio
        CJQColumn ColFechaInicio = new CJQColumn();
        ColFechaInicio.Nombre = "FechaInicio";
        ColFechaInicio.Encabezado = "FechaI";
        ColFechaInicio.Buscador = "false";
        ColFechaInicio.Alineacion = "left";
        ColFechaInicio.Ancho = "65";
        GridProyectoSinAsignar.Columnas.Add(ColFechaInicio);

        //FechaTermino
        CJQColumn ColFechaTermino = new CJQColumn();
        ColFechaTermino.Nombre = "FechaTermino";
        ColFechaTermino.Encabezado = "FechaT";
        ColFechaTermino.Buscador = "false";
        ColFechaTermino.Alineacion = "left";
        ColFechaTermino.Ancho = "65";
        GridProyectoSinAsignar.Columnas.Add(ColFechaTermino);

        //NombreComercial
        CJQColumn ColNombreComercial = new CJQColumn();
        ColNombreComercial.Nombre = "NombreComercial";
        ColNombreComercial.Encabezado = "Cliente";
        ColNombreComercial.Buscador = "false";
        ColNombreComercial.Alineacion = "left";
        ColNombreComercial.Ancho = "65";
        GridProyectoSinAsignar.Columnas.Add(ColNombreComercial);

        //UsuarioResponsable
        CJQColumn ColUsuarioResponsable = new CJQColumn();
        ColUsuarioResponsable.Nombre = "UsuarioResponsable";
        ColUsuarioResponsable.Encabezado = "Responsable";
        ColUsuarioResponsable.Buscador = "false";
        ColUsuarioResponsable.Alineacion = "left";
        ColUsuarioResponsable.Ancho = "65";
        GridProyectoSinAsignar.Columnas.Add(ColUsuarioResponsable);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProyectoSinAsignar", GridProyectoSinAsignar.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerPedidoFacturaProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNumeroFactura)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdPedidoFacturaProveedor", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = 0;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturaProveedorSinAsignacionPedido(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNumeroFacturaAsignar, string pFechaInicial, string pFechaFinal, int pIdPedidoDetalle, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaProveedorSinAsignacionPedido", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNumeroFacturaAsignar", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFacturaAsignar);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 10).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Stored.Parameters.Add("pIdPedidoDetalle", SqlDbType.Int).Value = pIdPedidoDetalle;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerPedidoSinAsignar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pFechaInicial, string pFechaFinal, string pTipoOrden, int pTodos, string pFolio, string pClave, string pDescripcion, int pSucursal, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdPedidosSinAsignar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 10).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Stored.Parameters.Add("pTodos", SqlDbType.Int).Value = pTodos;
        Stored.Parameters.Add("pFechaPedido", SqlDbType.Int).Value = 0;
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 255).Value = pFolio;
        Stored.Parameters.Add("pClave", SqlDbType.VarChar, 255).Value = pClave;
        Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 255).Value = pDescripcion;
        Stored.Parameters.Add("pSucursal", SqlDbType.Int).Value = pSucursal;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProyectoSinAsignar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pFechaInicial, string pFechaFinal, string pTipoOrden, int pIdProyecto, string pNombreProyecto, int pTodos, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdProyectoSinAsignar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 10).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 10).Value = pFechaFinal;
        Stored.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
        Stored.Parameters.Add("pNombreProyecto", SqlDbType.VarChar, 100).Value = pNombreProyecto;
        Stored.Parameters.Add("pTodos", SqlDbType.Int).Value = pTodos;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarClave(string pClave)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
        DetalleFacturaProveedor.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_ConsultarFiltrosGrid";
        DetalleFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pClave", pClave);
        return DetalleFacturaProveedor.JsonStoredProcedure(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarDescripcion(string pDescripcion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
        DetalleFacturaProveedor.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_Consultar_Descripcion";
        DetalleFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pDescripcion", pDescripcion);
        return DetalleFacturaProveedor.JsonStoredProcedure(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarFolio(string pFolio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
        DetalleFacturaProveedor.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_Consultar_Folio";
        DetalleFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pFolio", pFolio);
        return DetalleFacturaProveedor.JsonStoredProcedure(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarIdProyecto(string pIdProyecto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
        DetalleFacturaProveedor.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_Consultar_IdProyecto";
        DetalleFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", pIdProyecto);
        return DetalleFacturaProveedor.JsonStoredProcedure(ConexionBaseDatos);
    }
    [WebMethod]
    public static string BuscarNumeroFactura(string pNumeroFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonNumeroFactura = new CJson();
        jsonNumeroFactura.StoredProcedure.CommandText = "sp_PedidoFacturaProveedor_ConsultarFiltrosGrid";
        jsonNumeroFactura.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonNumeroFactura.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", pNumeroFactura);
        return jsonNumeroFactura.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarNombreProyecto(string pNombreProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonNumeroFactura = new CJson();
        jsonNumeroFactura.StoredProcedure.CommandText = "sp_ProyectoFacturaProveedor_Consultar_Nombre";
        jsonNumeroFactura.StoredProcedure.Parameters.AddWithValue("@pNombreProyecto", pNombreProyecto);
        return jsonNumeroFactura.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarNumeroFacturaAsignar(string pNumeroFacturaAsignar)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonNumeroFacturaAsignar = new CJson();
        jsonNumeroFacturaAsignar.StoredProcedure.CommandText = "sp_FacturaProveedorSinAsignacionPedido_ConsultarFiltrosGrid";
        jsonNumeroFacturaAsignar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonNumeroFacturaAsignar.StoredProcedure.Parameters.AddWithValue("@pNumeroFacturaAsignar", pNumeroFacturaAsignar);
        return jsonNumeroFacturaAsignar.ObtenerJsonString(ConexionBaseDatos);
    }


    [WebMethod]
    public static string AsignarAFacturaProveedor(Dictionary<string, object> pDatos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
            DetalleFacturaProveedor.LlenaObjeto(Convert.ToInt32(pDatos["IdDetalleFacturaProveedor"]), ConexionBaseDatos);

            if (Convert.ToInt32(pDatos["IdPedido"]) != 0)
            {
                DetalleFacturaProveedor.IdPedido = Convert.ToInt32(pDatos["IdPedido"]);
                DetalleFacturaProveedor.IdPedidoDetalle = Convert.ToInt32(pDatos["IdPedidoDetalle"]);
            }
            else
            {
                DetalleFacturaProveedor.IdProyecto = Convert.ToInt32(pDatos["IdProyecto"]);
            }

            DetalleFacturaProveedor.Editar(ConexionBaseDatos);

            //Agrega la fecha de recepcion a Cotizacion Detalle
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            CotizacionDetalle.LlenaObjeto(Convert.ToInt32(pDatos["IdPedidoDetalle"]), ConexionBaseDatos);
            CotizacionDetalle.Recepcion = Convert.ToDateTime(DateTime.Now);
            CotizacionDetalle.Editar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
			oRespuesta.Add(new JProperty("Proyecto", DetalleFacturaProveedor.IdProyecto));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

}