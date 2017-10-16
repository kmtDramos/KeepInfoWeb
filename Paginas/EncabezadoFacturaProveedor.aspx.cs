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

public partial class EncabezadoFacturaProveedor : System.Web.UI.Page
{
    public static int puedeAgregarEncabezadoFacturaProveedor = 0;
    public static int puedeEditarEncabezadoFacturaProveedor = 0;
    public static int puedeEliminarEncabezadoFacturaProveedor = 0;
    public static int puedeRevisarFacturaProveedor = 0;

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

        puedeAgregarEncabezadoFacturaProveedor = Usuario.TienePermisos(new string[] { "puedeAgregarEncabezadoFacturaProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarEncabezadoFacturaProveedor = Usuario.TienePermisos(new string[] { "puedeEditarEncabezadoFacturaProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarEncabezadoFacturaProveedor = Usuario.TienePermisos(new string[] { "puedeEliminarEncabezadoFacturaProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeRevisarFacturaProveedor = Usuario.TienePermisos(new string[] { "puedeRevisarFacturaProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;

        //GridEncabezadoFacturaProveedor
        CJQGrid GridEncabezadoFacturaProveedor = new CJQGrid();
        GridEncabezadoFacturaProveedor.NombreTabla = "grdEncabezadoFacturaProveedor";
        GridEncabezadoFacturaProveedor.CampoIdentificador = "IdEncabezadoFacturaProveedor";
        GridEncabezadoFacturaProveedor.ColumnaOrdenacion = "NumeroFactura";
        GridEncabezadoFacturaProveedor.Metodo = "ObtenerEncabezadoFacturaProveedor";
        GridEncabezadoFacturaProveedor.TituloTabla = "Facturas de proveedor";
        GridEncabezadoFacturaProveedor.GenerarFuncionFiltro = false;

        //IdEncabezadoFacturaProveedor
        CJQColumn ColIdEncabezadoFacturaProveedor = new CJQColumn();
        ColIdEncabezadoFacturaProveedor.Nombre = "IdEncabezadoFacturaProveedor";
        ColIdEncabezadoFacturaProveedor.Oculto = "true";
        ColIdEncabezadoFacturaProveedor.Encabezado = "IdEncabezadoFacturaProveedor";
        ColIdEncabezadoFacturaProveedor.Buscador = "false";
        ColIdEncabezadoFacturaProveedor.Ancho = "5";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColIdEncabezadoFacturaProveedor);

        //NumeroFactura
        CJQColumn ColNumeroFactura = new CJQColumn();
        ColNumeroFactura.Nombre = "NumeroFactura";
        ColNumeroFactura.Encabezado = "No. factura";
        ColNumeroFactura.Ancho = "40";
        ColNumeroFactura.Alineacion = "left";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColNumeroFactura);

        //Proveedor
        CJQColumn ColNombreProveedor = new CJQColumn();
        ColNombreProveedor.Nombre = "RazonSocial";
        ColNombreProveedor.Encabezado = "Razón social";
        ColNombreProveedor.Ancho = "120";
        ColNombreProveedor.Alineacion = "left";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColNombreProveedor);

        //FechaFactura
        CJQColumn ColFechaFactura = new CJQColumn();
        ColFechaFactura.Nombre = "Fecha";
        ColFechaFactura.Encabezado = "Fecha de factura";
        ColFechaFactura.Ancho = "40";
        ColFechaFactura.Alineacion = "left";
        ColFechaFactura.Buscador = "false";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColFechaFactura);


        //FechaPago
        CJQColumn ColFechaPago = new CJQColumn();
        ColFechaPago.Nombre = "FechaPago";
        ColFechaPago.Encabezado = "Fecha de pago";
        ColFechaPago.Ancho = "40";
        ColFechaPago.Alineacion = "left";
        ColFechaPago.Buscador = "false";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColFechaPago);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Ancho = "40";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Buscador = "false";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColTipoMoneda);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "50";
        ColTotal.Formato = "FormatoMoneda";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColTotal);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "División";
        ColDivision.Ancho = "60";
        ColDivision.Alineacion = "left";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColDivision);

        //Cheques
        CJQColumn ColCheques = new CJQColumn();
        ColCheques.Nombre = "Cheques";
        ColCheques.Encabezado = "Cheques";
        ColCheques.Buscador = "false";
        ColCheques.Ancho = "50";
        ColCheques.Alineacion = "left";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColCheques);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "30";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.Oculto = puedeEliminarEncabezadoFacturaProveedor == 1 ? "false" : "true";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarEncabezadoFacturaProveedor";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridEncabezadoFacturaProveedor.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdEncabezadoFacturaProveedor", GridEncabezadoFacturaProveedor.GeneraGrid(), true);

        //GridDetalleFacturaProveedor
        CJQGrid grdDetalleFacturaProveedor = new CJQGrid();
        grdDetalleFacturaProveedor.NombreTabla = "grdDetalleFacturaProveedor";
        grdDetalleFacturaProveedor.CampoIdentificador = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedor.ColumnaOrdenacion = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedor.TipoOrdenacion = "DESC";
        grdDetalleFacturaProveedor.Metodo = "ObtenerDetalleFacturaProveedor";
        grdDetalleFacturaProveedor.TituloTabla = "DetalleFacturaProveedor";
        grdDetalleFacturaProveedor.GenerarGridCargaInicial = false;
        grdDetalleFacturaProveedor.GenerarFuncionFiltro = false;
        grdDetalleFacturaProveedor.GenerarFuncionTerminado = false;
        grdDetalleFacturaProveedor.Altura = 150;
        grdDetalleFacturaProveedor.Ancho = 870;
        grdDetalleFacturaProveedor.NumeroRegistros = 15;
        grdDetalleFacturaProveedor.RangoNumeroRegistros = "15,30,60";

        //IdDetalleFacturaProveedor
        CJQColumn ColIdDetalleFacturaProveedor = new CJQColumn();
        ColIdDetalleFacturaProveedor.Nombre = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Oculto = "true";
        ColIdDetalleFacturaProveedor.Encabezado = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Buscador = "false";
        grdDetalleFacturaProveedor.Columnas.Add(ColIdDetalleFacturaProveedor);

        //Clavedetalle
        CJQColumn ColClaveDetalle = new CJQColumn();
        ColClaveDetalle.Nombre = "Clave";
        ColClaveDetalle.Encabezado = "Clave";
        ColClaveDetalle.Buscador = "false";
        ColClaveDetalle.Alineacion = "left";
        ColClaveDetalle.Ancho = "30";
        grdDetalleFacturaProveedor.Columnas.Add(ColClaveDetalle);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "90";
        grdDetalleFacturaProveedor.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        ColCantidad.Ancho = "30";
        grdDetalleFacturaProveedor.Columnas.Add(ColCantidad);

        //UnidadCompraVenta
        CJQColumn ColUnidadCompraVenta = new CJQColumn();
        ColUnidadCompraVenta.Nombre = "UnidadCompraVenta";
        ColUnidadCompraVenta.Encabezado = "UCV";
        ColUnidadCompraVenta.Buscador = "false";
        ColUnidadCompraVenta.Alineacion = "left";
        ColUnidadCompraVenta.Ancho = "30";
        grdDetalleFacturaProveedor.Columnas.Add(ColUnidadCompraVenta);

        //Precio
        CJQColumn ColPrecio = new CJQColumn();
        ColPrecio.Nombre = "Precio";
        ColPrecio.Encabezado = "Precio";
        ColPrecio.Buscador = "false";
        ColPrecio.Formato = "FormatoMoneda";
        ColPrecio.Alineacion = "right";
        ColPrecio.Ancho = "30";
        grdDetalleFacturaProveedor.Columnas.Add(ColPrecio);

        //Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "Descuento";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Buscador = "false";
        ColDescuento.Formato = "FormatoMoneda";
        ColDescuento.Alineacion = "right";
        ColDescuento.Ancho = "30";
        grdDetalleFacturaProveedor.Columnas.Add(ColDescuento);


        //Total
        CJQColumn ColTotalFactura = new CJQColumn();
        ColTotalFactura.Nombre = "Total";
        ColTotalFactura.Encabezado = "Total";
        ColTotalFactura.Buscador = "false";
        ColTotalFactura.Formato = "FormatoMoneda";
        ColTotalFactura.Alineacion = "right";
        ColTotalFactura.Ancho = "30";
        grdDetalleFacturaProveedor.Columnas.Add(ColTotalFactura);

        //IVA
        CJQColumn ColIVA = new CJQColumn();
        ColIVA.Nombre = "IVA";
        ColIVA.Encabezado = "IVA";
        ColIVA.Buscador = "false";
        ColIVA.Formato = "FormatoMoneda";
        ColIVA.Alineacion = "right";
        ColIVA.Ancho = "30";
        grdDetalleFacturaProveedor.Columnas.Add(ColIVA);

        //ClienteProyecto
        CJQColumn ColClienteProyecto = new CJQColumn();
        ColClienteProyecto.Nombre = "ClienteProyecto";
        ColClienteProyecto.Encabezado = "Cliente/Proyecto";
        ColClienteProyecto.Buscador = "false";
        ColClienteProyecto.Alineacion = "left";
        ColClienteProyecto.Ancho = "60";
        grdDetalleFacturaProveedor.Columnas.Add(ColClienteProyecto);

        //NumeroSerie
        CJQColumn ColNumeroSerie = new CJQColumn();
        ColNumeroSerie.Nombre = "NumeroSerie";
        ColNumeroSerie.Encabezado = "Número de Serie";
        ColNumeroSerie.Buscador = "false";
        ColNumeroSerie.Alineacion = "left";
        ColNumeroSerie.Ancho = "60";
        ColNumeroSerie.Estilo = "txtYNumeroSerie";
        grdDetalleFacturaProveedor.Columnas.Add(ColNumeroSerie);

        //Pedido
        CJQColumn ColNumeroPedido = new CJQColumn();
        ColNumeroPedido.Nombre = "NumeroPedido";
        ColNumeroPedido.Encabezado = "Pedido";
        ColNumeroPedido.Buscador = "false";
        ColNumeroPedido.Alineacion = "left";
        ColNumeroPedido.Ancho = "40";
        grdDetalleFacturaProveedor.Columnas.Add(ColNumeroPedido);

        //Eliminar concepto factura de proveedor
        CJQColumn ColEliminarConcepto = new CJQColumn();
        ColEliminarConcepto.Nombre = "Eliminar";
        ColEliminarConcepto.Encabezado = "Eliminar";
        ColEliminarConcepto.Etiquetado = "Imagen";
        ColEliminarConcepto.Imagen = "eliminar.png";
        ColEliminarConcepto.Estilo = "divImagenConsultar imgEliminarConceptoEditar";
        ColEliminarConcepto.Buscador = "false";
        ColEliminarConcepto.Ordenable = "false";
        ColEliminarConcepto.Ancho = "60";
        grdDetalleFacturaProveedor.Columnas.Add(ColEliminarConcepto);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleFacturaProveedor", grdDetalleFacturaProveedor.GeneraGrid(), true);

        //GridDetalleFacturaProveedorConsultar
        CJQGrid grdDetalleFacturaProveedorConsultar = new CJQGrid();
        grdDetalleFacturaProveedorConsultar.NombreTabla = "grdDetalleFacturaProveedorConsultar";
        grdDetalleFacturaProveedorConsultar.CampoIdentificador = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedorConsultar.ColumnaOrdenacion = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedorConsultar.TipoOrdenacion = "DESC";
        grdDetalleFacturaProveedorConsultar.Metodo = "ObtenerDetalleFacturaProveedorConsultar";
        grdDetalleFacturaProveedorConsultar.TituloTabla = "DetalleFacturaProveedor";
        grdDetalleFacturaProveedorConsultar.GenerarGridCargaInicial = false;
        grdDetalleFacturaProveedorConsultar.GenerarFuncionFiltro = false;
        grdDetalleFacturaProveedorConsultar.GenerarFuncionTerminado = false;
        grdDetalleFacturaProveedorConsultar.Altura = 150;
        grdDetalleFacturaProveedorConsultar.Ancho = 1200;
        grdDetalleFacturaProveedorConsultar.NumeroRegistros = 15;
        grdDetalleFacturaProveedorConsultar.RangoNumeroRegistros = "15,30,60";

        //IdDetalleFacturaProveedor
        CJQColumn ColIdDetalleFacturaProveedorConsultar = new CJQColumn();
        ColIdDetalleFacturaProveedorConsultar.Nombre = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedorConsultar.Oculto = "true";
        ColIdDetalleFacturaProveedorConsultar.Encabezado = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedorConsultar.Buscador = "false";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColIdDetalleFacturaProveedorConsultar);

        //ClavedetalleConsultar
        CJQColumn ColClaveDetalleConsultar = new CJQColumn();
        ColClaveDetalleConsultar.Nombre = "Clave";
        ColClaveDetalleConsultar.Encabezado = "Clave";
        ColClaveDetalleConsultar.Buscador = "false";
        ColClaveDetalleConsultar.Alineacion = "left";
        ColClaveDetalleConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColClaveDetalleConsultar);

        //DescripcionConsultar
        CJQColumn ColDescripcionConsultar = new CJQColumn();
        ColDescripcionConsultar.Nombre = "Descripcion";
        ColDescripcionConsultar.Encabezado = "Descripción";
        ColDescripcionConsultar.Buscador = "false";
        ColDescripcionConsultar.Alineacion = "left";
        ColDescripcionConsultar.Ancho = "90";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColDescripcionConsultar);

        //CantidadConsultar
        CJQColumn ColCantidadConsultar = new CJQColumn();
        ColCantidadConsultar.Nombre = "Cantidad";
        ColCantidadConsultar.Encabezado = "Cantidad";
        ColCantidadConsultar.Buscador = "false";
        ColCantidadConsultar.Alineacion = "left";
        ColCantidadConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColCantidadConsultar);

        //UnidadCompraVentaConsultar
        CJQColumn ColUnidadCompraVentaConsultar = new CJQColumn();
        ColUnidadCompraVentaConsultar.Nombre = "UnidadCompraVenta";
        ColUnidadCompraVentaConsultar.Encabezado = "UCV";
        ColUnidadCompraVentaConsultar.Buscador = "false";
        ColUnidadCompraVentaConsultar.Alineacion = "left";
        ColUnidadCompraVentaConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColUnidadCompraVentaConsultar);

        //PrecioConsultar
        CJQColumn ColPrecioConsultar = new CJQColumn();
        ColPrecioConsultar.Nombre = "Precio";
        ColPrecioConsultar.Encabezado = "Precio";
        ColPrecioConsultar.Buscador = "false";
        ColPrecioConsultar.Formato = "FormatoMoneda";
        ColPrecioConsultar.Alineacion = "right";
        ColPrecioConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColPrecioConsultar);

        //DescuentoConsultar
        CJQColumn ColDescuentoConsultar = new CJQColumn();
        ColDescuentoConsultar.Nombre = "Descuento";
        ColDescuentoConsultar.Encabezado = "Descuento";
        ColDescuentoConsultar.Buscador = "false";
        ColDescuentoConsultar.Formato = "FormatoMoneda";
        ColDescuentoConsultar.Alineacion = "right";
        ColDescuentoConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColDescuentoConsultar);


        //TotalConsultar
        CJQColumn ColTotalConsultar = new CJQColumn();
        ColTotalConsultar.Nombre = "Total";
        ColTotalConsultar.Encabezado = "Total";
        ColTotalConsultar.Buscador = "false";
        ColTotalConsultar.Formato = "FormatoMoneda";
        ColTotalConsultar.Alineacion = "right";
        ColTotalConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColTotalConsultar);

        //IVAConsultar
        CJQColumn ColIVAConsultar = new CJQColumn();
        ColIVAConsultar.Nombre = "IVA";
        ColIVAConsultar.Encabezado = "IVA";
        ColIVAConsultar.Buscador = "false";
        ColIVAConsultar.Formato = "FormatoMoneda";
        ColIVAConsultar.Alineacion = "right";
        ColIVAConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColIVAConsultar);

        //ClienteProyectoConsultar
        CJQColumn ColClienteProyectoConsultar = new CJQColumn();
        ColClienteProyectoConsultar.Nombre = "ClienteProyecto";
        ColClienteProyectoConsultar.Encabezado = "Cliente/Proyecto";
        ColClienteProyectoConsultar.Buscador = "false";
        ColClienteProyectoConsultar.Alineacion = "left";
        ColClienteProyectoConsultar.Ancho = "60";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColClienteProyectoConsultar);

        //NumeroSerieConsultar
        CJQColumn ColNumeroSerieConsultar = new CJQColumn();
        ColNumeroSerieConsultar.Nombre = "NumeroSerie";
        ColNumeroSerieConsultar.Encabezado = "Número de Serie";
        ColNumeroSerieConsultar.Buscador = "false";
        ColNumeroSerieConsultar.Alineacion = "left";
        ColNumeroSerieConsultar.Ancho = "60";
        ColNumeroSerieConsultar.Estilo = "txtCNumeroSerie";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColNumeroSerieConsultar);

        //Pedido consultar
        CJQColumn ColNumeroPedidoConsultar = new CJQColumn();
        ColNumeroPedidoConsultar.Nombre = "NumeroPedido";
        ColNumeroPedidoConsultar.Encabezado = "Pedido";
        ColNumeroPedidoConsultar.Buscador = "false";
        ColNumeroPedidoConsultar.Alineacion = "left";
        ColNumeroPedidoConsultar.Ancho = "40";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColNumeroPedidoConsultar);

        //Tipo de compra
        CJQColumn ColTipoCompra = new CJQColumn();
        ColTipoCompra.Nombre = "TipoCompra";
        ColTipoCompra.Encabezado = "Tipo de compra";
        ColTipoCompra.Buscador = "false";
        ColTipoCompra.Alineacion = "left";
        ColTipoCompra.Ancho = "80";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColTipoCompra);

        //Usuario solicito
        CJQColumn ColUsuarioSolicito = new CJQColumn();
        ColUsuarioSolicito.Nombre = "UsuarioSolicito";
        ColUsuarioSolicito.Encabezado = "Usuario solicitante";
        ColUsuarioSolicito.Buscador = "false";
        ColUsuarioSolicito.Alineacion = "left";
        ColUsuarioSolicito.Ancho = "80";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColUsuarioSolicito);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleFacturaProveedorConsultar", grdDetalleFacturaProveedorConsultar.GeneraGrid(), true);

        //GridDetalleFacturaProveedorEditar
        CJQGrid grdDetalleFacturaProveedorEditar = new CJQGrid();
        grdDetalleFacturaProveedorEditar.NombreTabla = "grdDetalleFacturaProveedorEditar";
        grdDetalleFacturaProveedorEditar.CampoIdentificador = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedorEditar.ColumnaOrdenacion = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedorEditar.TipoOrdenacion = "DESC";
        grdDetalleFacturaProveedorEditar.Metodo = "ObtenerDetalleFacturaProveedorEditar";
        grdDetalleFacturaProveedorEditar.TituloTabla = "DetalleFacturaProveedor";
        grdDetalleFacturaProveedorEditar.GenerarGridCargaInicial = false;
        grdDetalleFacturaProveedorEditar.GenerarFuncionFiltro = false;
        grdDetalleFacturaProveedorEditar.GenerarFuncionTerminado = false;
        grdDetalleFacturaProveedorEditar.Altura = 150;
        grdDetalleFacturaProveedorEditar.Ancho = 870;
        grdDetalleFacturaProveedorEditar.NumeroRegistros = 15;
        grdDetalleFacturaProveedorEditar.RangoNumeroRegistros = "15,30,60";

        //IdDetalleFacturaProveedor
        CJQColumn ColIdDetalleFacturaProveedorEditar = new CJQColumn();
        ColIdDetalleFacturaProveedorEditar.Nombre = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedorEditar.Oculto = "true";
        ColIdDetalleFacturaProveedorEditar.Encabezado = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedorEditar.Buscador = "false";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColIdDetalleFacturaProveedorEditar);

        //ClavedetalleEditar
        CJQColumn ColClaveDetalleEditar = new CJQColumn();
        ColClaveDetalleEditar.Nombre = "Clave";
        ColClaveDetalleEditar.Encabezado = "Clave";
        ColClaveDetalleEditar.Buscador = "false";
        ColClaveDetalleEditar.Alineacion = "left";
        ColClaveDetalleEditar.Ancho = "30";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColClaveDetalleEditar);

        //DescripcionEditar
        CJQColumn ColDescripcionEditar = new CJQColumn();
        ColDescripcionEditar.Nombre = "Descripcion";
        ColDescripcionEditar.Encabezado = "Descripción";
        ColDescripcionEditar.Buscador = "false";
        ColDescripcionEditar.Alineacion = "left";
        ColDescripcionEditar.Ancho = "90";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColDescripcionEditar);

        //CantidadEditar
        CJQColumn ColCantidadEditar = new CJQColumn();
        ColCantidadEditar.Nombre = "Cantidad";
        ColCantidadEditar.Encabezado = "Cantidad";
        ColCantidadEditar.Buscador = "false";
        ColCantidadEditar.Alineacion = "left";
        ColCantidadEditar.Ancho = "30";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColCantidadEditar);

        //UnidadCompraVentaEditar
        CJQColumn ColUnidadCompraVentaEditar = new CJQColumn();
        ColUnidadCompraVentaEditar.Nombre = "UnidadCompraVenta";
        ColUnidadCompraVentaEditar.Encabezado = "UCV";
        ColUnidadCompraVentaEditar.Buscador = "false";
        ColUnidadCompraVentaEditar.Alineacion = "left";
        ColUnidadCompraVentaEditar.Ancho = "30";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColUnidadCompraVentaEditar);

        //PrecioEditar
        CJQColumn ColPrecioEditar = new CJQColumn();
        ColPrecioEditar.Nombre = "Precio";
        ColPrecioEditar.Encabezado = "Precio";
        ColPrecioEditar.Buscador = "false";
        ColPrecioEditar.Formato = "FormatoMoneda";
        ColPrecioEditar.Alineacion = "right";
        ColPrecioEditar.Ancho = "30";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColPrecioEditar);

        //DescuentoEditar
        CJQColumn ColDescuentoEditar = new CJQColumn();
        ColDescuentoEditar.Nombre = "Descuento";
        ColDescuentoEditar.Encabezado = "Descuento";
        ColDescuentoEditar.Buscador = "false";
        ColDescuentoEditar.Formato = "FormatoMoneda";
        ColDescuentoEditar.Alineacion = "right";
        ColDescuentoEditar.Ancho = "30";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColDescuentoEditar);

        //TotalEditar
        CJQColumn ColTotalEditar = new CJQColumn();
        ColTotalEditar.Nombre = "Total";
        ColTotalEditar.Encabezado = "Total";
        ColTotalEditar.Buscador = "false";
        ColTotalEditar.Formato = "FormatoMoneda";
        ColTotalEditar.Alineacion = "right";
        ColTotalEditar.Ancho = "30";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColTotalEditar);

        //IVAEditar
        CJQColumn ColIVAEditar = new CJQColumn();
        ColIVAEditar.Nombre = "IVA";
        ColIVAEditar.Encabezado = "IVA";
        ColIVAEditar.Buscador = "false";
        ColIVAEditar.Formato = "FormatoMoneda";
        ColIVAEditar.Alineacion = "right";
        ColIVAEditar.Ancho = "30";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColIVAEditar);

        //ClienteProyectoEditar
        CJQColumn ColClienteProyectoEditar = new CJQColumn();
        ColClienteProyectoEditar.Nombre = "ClienteProyecto";
        ColClienteProyectoEditar.Encabezado = "Cliente/Proyecto";
        ColClienteProyectoEditar.Buscador = "false";
        ColClienteProyectoEditar.Alineacion = "left";
        ColClienteProyectoEditar.Ancho = "60";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColClienteProyectoEditar);

        //NumeroSerieEditar
        CJQColumn ColNumeroSerieEditar = new CJQColumn();
        ColNumeroSerieEditar.Nombre = "NumeroSerie";
        ColNumeroSerieEditar.Encabezado = "NumeroSerie";
        ColNumeroSerieEditar.Buscador = "false";
        ColNumeroSerieEditar.Alineacion = "left";
        ColNumeroSerieEditar.Ancho = "50";
        ColNumeroSerieEditar.Estilo = "txtENumeroSerie";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColNumeroSerieEditar);

        //Pedido editar
        CJQColumn ColNumeroPedidoEditar = new CJQColumn();
        ColNumeroPedidoEditar.Nombre = "NumeroPedido";
        ColNumeroPedidoEditar.Encabezado = "Pedido";
        ColNumeroPedidoEditar.Buscador = "false";
        ColNumeroPedidoEditar.Alineacion = "left";
        ColNumeroPedidoEditar.Ancho = "40";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColNumeroPedidoEditar);

        //Eliminar concepto factura de proveedor consultar
        CJQColumn ColEliminarConceptoEditar = new CJQColumn();
        ColEliminarConceptoEditar.Nombre = "Eliminar";
        ColEliminarConceptoEditar.Encabezado = "Eliminar";
        ColEliminarConceptoEditar.Etiquetado = "Imagen";
        ColEliminarConceptoEditar.Imagen = "eliminar.png";
        ColEliminarConceptoEditar.Estilo = "divImagenConsultar imgEliminarConceptoEditar";
        ColEliminarConceptoEditar.Buscador = "false";
        ColEliminarConceptoEditar.Ordenable = "false";
        ColEliminarConceptoEditar.Ancho = "60";
        grdDetalleFacturaProveedorEditar.Columnas.Add(ColEliminarConceptoEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleFacturaProveedorEditar", grdDetalleFacturaProveedorEditar.GeneraGrid(), true);

        //GridProductoNumeroSerie
        CJQGrid GridProductoNumeroSerie = new CJQGrid();
        GridProductoNumeroSerie.NombreTabla = "grdProductoNumeroSerie";
        GridProductoNumeroSerie.CampoIdentificador = "IdProducto";
        GridProductoNumeroSerie.ColumnaOrdenacion = "Clave";
        GridProductoNumeroSerie.TipoOrdenacion = "DESC";
        GridProductoNumeroSerie.Metodo = "ObtenerProductoNumeroSerie";
        GridProductoNumeroSerie.TituloTabla = "Productos";
        GridProductoNumeroSerie.GenerarFuncionFiltro = false;
        GridProductoNumeroSerie.GenerarFuncionTerminado = false;
        GridProductoNumeroSerie.Editable = true;
        GridProductoNumeroSerie.Altura = 200;
        GridProductoNumeroSerie.Ancho = 900;
        GridProductoNumeroSerie.NumeroRegistros = 200;
        GridProductoNumeroSerie.RangoNumeroRegistros = "200,400,600";

        //IdProducto
        CJQColumn ColIdProducto = new CJQColumn();
        ColIdProducto.Nombre = "IdProducto";
        ColIdProducto.Oculto = "true";
        ColIdProducto.Encabezado = "IdProducto";
        ColIdProducto.Buscador = "false";
        GridProductoNumeroSerie.Columnas.Add(ColIdProducto);

        //Clave
        CJQColumn ColClaveProducto = new CJQColumn();
        ColClaveProducto.Nombre = "Clave";
        ColClaveProducto.Encabezado = "Clave";
        ColClaveProducto.Buscador = "false";
        ColClaveProducto.Alineacion = "left";
        ColClaveProducto.Ancho = "80";
        GridProductoNumeroSerie.Columnas.Add(ColClaveProducto);

        //DescripcionProducto
        CJQColumn ColDescripcionProducto = new CJQColumn();
        ColDescripcionProducto.Nombre = "Descripcion";
        ColDescripcionProducto.Encabezado = "Descripción";
        ColDescripcionProducto.Buscador = "false";
        ColDescripcionProducto.Alineacion = "left";
        ColDescripcionProducto.Ancho = "100";
        GridProductoNumeroSerie.Columnas.Add(ColDescripcionProducto);

        //CantidadProducto
        CJQColumn ColCantidadProducto = new CJQColumn();
        ColCantidadProducto.Nombre = "Cantidad";
        ColCantidadProducto.Encabezado = "Cantidad";
        ColCantidadProducto.Buscador = "false";
        ColCantidadProducto.Alineacion = "left";
        ColCantidadProducto.Ancho = "50";
        GridProductoNumeroSerie.Columnas.Add(ColCantidadProducto);

        //UnidadCompraVentaProducto
        CJQColumn ColUnidadCompraVentaProducto = new CJQColumn();
        ColUnidadCompraVentaProducto.Nombre = "UCV";
        ColUnidadCompraVentaProducto.Encabezado = "UCV";
        ColUnidadCompraVentaProducto.Buscador = "false";
        ColUnidadCompraVentaProducto.Alineacion = "left";
        ColUnidadCompraVentaProducto.Ancho = "50";
        GridProductoNumeroSerie.Columnas.Add(ColUnidadCompraVentaProducto);

        //PrecioProducto
        CJQColumn ColPrecioProducto = new CJQColumn();
        ColPrecioProducto.Nombre = "Precio";
        ColPrecioProducto.Encabezado = "Costo";
        ColPrecioProducto.Buscador = "false";
        ColPrecioProducto.Formato = "FormatoMoneda";
        ColPrecioProducto.Alineacion = "right";
        ColPrecioProducto.Ancho = "60";
        GridProductoNumeroSerie.Columnas.Add(ColPrecioProducto);

        //DescuentoProducto
        CJQColumn ColDescuentoProducto = new CJQColumn();
        ColDescuentoProducto.Nombre = "Descuento";
        ColDescuentoProducto.Encabezado = "Descuento";
        ColDescuentoProducto.Buscador = "false";
        //ColDescuentoProducto.Formato = "FormatoMoneda";
        ColDescuentoProducto.Alineacion = "right";
        ColDescuentoProducto.Ancho = "60";
        GridProductoNumeroSerie.Columnas.Add(ColDescuentoProducto);

        //TotalProducto
        CJQColumn ColTotalProducto = new CJQColumn();
        ColTotalProducto.Nombre = "Total";
        ColTotalProducto.Encabezado = "Total";
        ColTotalProducto.Buscador = "false";
        ColTotalProducto.Formato = "FormatoMoneda";
        ColTotalProducto.Alineacion = "right";
        ColTotalProducto.Ancho = "60";
        GridProductoNumeroSerie.Columnas.Add(ColTotalProducto);

        //NumeroSerie
        CJQColumn ColNumeroSerieProducto = new CJQColumn();
        ColNumeroSerieProducto.Nombre = "NumeroSerie";
        ColNumeroSerieProducto.Encabezado = "Número de serie";
        ColNumeroSerieProducto.Buscador = "false";
        ColNumeroSerieProducto.EditableTexto = "true";
        ColNumeroSerieProducto.Ancho = "90";
        ColNumeroSerieProducto.Estilo = "txtPNumeroSerie";
        GridProductoNumeroSerie.Columnas.Add(ColNumeroSerieProducto);

        //Elegir
        CJQColumn ColElegir = new CJQColumn();
        ColElegir.Nombre = "Elegir";
        ColElegir.Encabezado = "Elegir";
        ColElegir.Buscador = "false";
        ColElegir.Alineacion = "left";
        ColElegir.Etiquetado = "CheckBoxchecked";
        ColElegir.Id = "chkElegir";
        ColElegir.Estilo = "chkElegir";
        ColElegir.Ancho = "30";
        GridProductoNumeroSerie.Columnas.Add(ColElegir);
        ClientScript.RegisterStartupScript(this.GetType(), "grdProductoNumeroSerie", GridProductoNumeroSerie.GeneraGrid(), true);

        //GridDetallePedido
        CJQGrid GridDetallePedido = new CJQGrid();
        GridDetallePedido.NombreTabla = "grdDetallePedido";
        GridDetallePedido.CampoIdentificador = "IdCotizacionDetalle";
        GridDetallePedido.ColumnaOrdenacion = "Clave";
        GridDetallePedido.TipoOrdenacion = "DESC";
        GridDetallePedido.Metodo = "ObtenerDetallePedido";
        GridDetallePedido.TituloTabla = "Detalle de pedido pendientes de recepcionar";
        GridDetallePedido.GenerarFuncionFiltro = false;
        GridDetallePedido.GenerarFuncionTerminado = false;
        GridDetallePedido.Altura = 200;
        GridDetallePedido.Ancho = 850;
        GridDetallePedido.NumeroRegistros = 15;
        GridDetallePedido.RangoNumeroRegistros = "15,30,60";

        //IdCotizacionDetalle
        CJQColumn ColIdCotizacionDetalle = new CJQColumn();
        ColIdCotizacionDetalle.Nombre = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Oculto = "true";
        ColIdCotizacionDetalle.Encabezado = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Buscador = "false";
        GridDetallePedido.Columnas.Add(ColIdCotizacionDetalle);

        //IdProductoPedido
        CJQColumn ColIdProductoPedido = new CJQColumn();
        ColIdProductoPedido.Nombre = "IdProducto";
        ColIdProductoPedido.Oculto = "true";
        ColIdProductoPedido.Encabezado = "IdProducto";
        ColIdProductoPedido.Buscador = "false";
        GridDetallePedido.Columnas.Add(ColIdProductoPedido);

        //ClavePedido
        CJQColumn ColClaveProductoPedido = new CJQColumn();
        ColClaveProductoPedido.Nombre = "Clave";
        ColClaveProductoPedido.Encabezado = "Clave";
        ColClaveProductoPedido.Buscador = "false";
        ColClaveProductoPedido.Alineacion = "left";
        ColClaveProductoPedido.Ancho = "30";
        GridDetallePedido.Columnas.Add(ColClaveProductoPedido);

        //DescripcionPedido
        CJQColumn ColDescripcionProductoPedido = new CJQColumn();
        ColDescripcionProductoPedido.Nombre = "Descripcion";
        ColDescripcionProductoPedido.Encabezado = "Descripción";
        ColDescripcionProductoPedido.Alineacion = "left";
        ColDescripcionProductoPedido.Buscador = "false";
        ColDescripcionProductoPedido.Ancho = "80";
        GridDetallePedido.Columnas.Add(ColDescripcionProductoPedido);

        //CantidadProductoPedido
        CJQColumn ColCantidadProductoPedido = new CJQColumn();
        ColCantidadProductoPedido.Nombre = "Cantidad";
        ColCantidadProductoPedido.Encabezado = "Cantidad";
        ColCantidadProductoPedido.Buscador = "false";
        ColCantidadProductoPedido.Alineacion = "left";
        ColCantidadProductoPedido.Ancho = "20";
        GridDetallePedido.Columnas.Add(ColCantidadProductoPedido);

        //CantidadProductoPedidoArecepcionar
        CJQColumn ColCantidadProductoPedidoARecepcionar = new CJQColumn();
        ColCantidadProductoPedidoARecepcionar.Nombre = "RecepcionCantidad";
        ColCantidadProductoPedidoARecepcionar.Encabezado = "Pendientes de recepcionar";
        ColCantidadProductoPedidoARecepcionar.Buscador = "false";
        ColCantidadProductoPedidoARecepcionar.Alineacion = "left";
        ColCantidadProductoPedidoARecepcionar.Ancho = "50";
        GridDetallePedido.Columnas.Add(ColCantidadProductoPedidoARecepcionar);

        //PrecioProductoPedido
        CJQColumn ColPrecioProductoPedido = new CJQColumn();
        ColPrecioProductoPedido.Nombre = "PrecioUnitario";
        ColPrecioProductoPedido.Encabezado = "Precio";
        ColPrecioProductoPedido.Buscador = "false";
        ColPrecioProductoPedido.Formato = "FormatoMoneda";
        ColPrecioProductoPedido.Alineacion = "right";
        ColPrecioProductoPedido.Ancho = "30";
        GridDetallePedido.Columnas.Add(ColPrecioProductoPedido);

        //TotalProductoPedido
        CJQColumn ColTotalProductoPedido = new CJQColumn();
        ColTotalProductoPedido.Nombre = "Total";
        ColTotalProductoPedido.Encabezado = "Total";
        ColTotalProductoPedido.Buscador = "false";
        ColTotalProductoPedido.Formato = "FormatoMoneda";
        ColTotalProductoPedido.Alineacion = "right";
        ColTotalProductoPedido.Ancho = "30";
        GridDetallePedido.Columnas.Add(ColTotalProductoPedido);

        //TipoMoneda produto pedido
        CJQColumn ColTipoMonedaProductoPedido = new CJQColumn();
        ColTipoMonedaProductoPedido.Nombre = "TipoMoneda";
        ColTipoMonedaProductoPedido.Encabezado = "Tipo de moneda";
        ColTipoMonedaProductoPedido.Ancho = "40";
        ColTipoMonedaProductoPedido.Alineacion = "left";
        ColTipoMonedaProductoPedido.Buscador = "false";
        GridDetallePedido.Columnas.Add(ColTipoMonedaProductoPedido);

        //SeleccionarDetallePedido
        CJQColumn ColSeleccionarDetallePedido = new CJQColumn();
        ColSeleccionarDetallePedido.Nombre = "Seleccionar";
        ColSeleccionarDetallePedido.Encabezado = "Seleccionar";
        ColSeleccionarDetallePedido.Etiquetado = "Imagen";
        ColSeleccionarDetallePedido.Imagen = "select.png";
        ColSeleccionarDetallePedido.Estilo = "divImagenConsultar imgSeleccionarDetallePedido";
        ColSeleccionarDetallePedido.Buscador = "false";
        ColSeleccionarDetallePedido.Ordenable = "false";
        ColSeleccionarDetallePedido.Ancho = "25";
        GridDetallePedido.Columnas.Add(ColSeleccionarDetallePedido);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetallePedido", GridDetallePedido.GeneraGrid(), true);

        //GridDetalleOrdenCompra
        CJQGrid GridDetalleOrdenCompra = new CJQGrid();
        GridDetalleOrdenCompra.NombreTabla = "grdDetalleOrdenCompra";
        GridDetalleOrdenCompra.CampoIdentificador = "IdOrdenCompraDetalle";
        GridDetalleOrdenCompra.ColumnaOrdenacion = "Clave";
        GridDetalleOrdenCompra.TipoOrdenacion = "DESC";
        GridDetalleOrdenCompra.Metodo = "ObtenerDetalleOrdenCompra";
        GridDetalleOrdenCompra.TituloTabla = "Detalle de Orden de Compra";
        GridDetalleOrdenCompra.GenerarFuncionFiltro = false;
        GridDetalleOrdenCompra.GenerarFuncionTerminado = false;
        GridDetalleOrdenCompra.Altura = 200;
        GridDetalleOrdenCompra.Ancho = 800;
        GridDetalleOrdenCompra.NumeroRegistros = 15;
        GridDetalleOrdenCompra.RangoNumeroRegistros = "15,30,60";

        //IdOrdenCompraDetalle
        CJQColumn ColIdOrdenCompraDetalle = new CJQColumn();
        ColIdOrdenCompraDetalle.Nombre = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Oculto = "true";
        ColIdOrdenCompraDetalle.Encabezado = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Buscador = "false";
        GridDetalleOrdenCompra.Columnas.Add(ColIdOrdenCompraDetalle);

        //IdProductoOrdenCompra
        CJQColumn ColIdProductoOrdenCompra = new CJQColumn();
        ColIdProductoOrdenCompra.Nombre = "IdProducto";
        ColIdProductoOrdenCompra.Oculto = "true";
        ColIdProductoOrdenCompra.Encabezado = "IdProducto";
        ColIdProductoOrdenCompra.Buscador = "false";
        GridDetalleOrdenCompra.Columnas.Add(ColIdProductoOrdenCompra);

        //ClaveOrdenCompra
        CJQColumn ColClaveProductoOrdenCompra = new CJQColumn();
        ColClaveProductoOrdenCompra.Nombre = "Clave";
        ColClaveProductoOrdenCompra.Encabezado = "Clave";
        ColClaveProductoOrdenCompra.Buscador = "false";
        ColClaveProductoOrdenCompra.Alineacion = "left";
        ColClaveProductoOrdenCompra.Ancho = "30";
        GridDetalleOrdenCompra.Columnas.Add(ColClaveProductoOrdenCompra);

        //DescripcionOrdenCompra
        CJQColumn ColDescripcionProductoOrdenCompra = new CJQColumn();
        ColDescripcionProductoOrdenCompra.Nombre = "Descripcion";
        ColDescripcionProductoOrdenCompra.Encabezado = "Descripción";
        ColDescripcionProductoOrdenCompra.Alineacion = "left";
        ColDescripcionProductoOrdenCompra.Buscador = "false";
        ColDescripcionProductoOrdenCompra.Ancho = "80";
        GridDetalleOrdenCompra.Columnas.Add(ColDescripcionProductoOrdenCompra);

        //CantidadProductoOrdenCompra
        CJQColumn ColCantidadProductoOrdenCompra = new CJQColumn();
        ColCantidadProductoOrdenCompra.Nombre = "Cantidad";
        ColCantidadProductoOrdenCompra.Encabezado = "Cantidad";
        ColCantidadProductoOrdenCompra.Buscador = "false";
        ColCantidadProductoOrdenCompra.Alineacion = "left";
        ColCantidadProductoOrdenCompra.Ancho = "20";
        GridDetalleOrdenCompra.Columnas.Add(ColCantidadProductoOrdenCompra);

        //CantidadProductoPedidoArecepcionar
        CJQColumn ColCantidadProductoOCARecepcionar = new CJQColumn();
        ColCantidadProductoOCARecepcionar.Nombre = "RecepcionCantidad";
        ColCantidadProductoOCARecepcionar.Encabezado = "Pendientes de recepcionar";
        ColCantidadProductoOCARecepcionar.Buscador = "false";
        ColCantidadProductoOCARecepcionar.Alineacion = "left";
        ColCantidadProductoOCARecepcionar.Ancho = "50";
        GridDetalleOrdenCompra.Columnas.Add(ColCantidadProductoOCARecepcionar);

        //PrecioProductoOrdenCompra
        CJQColumn ColPrecioProductoOrdenCompra = new CJQColumn();
        ColPrecioProductoOrdenCompra.Nombre = "Costo";
        ColPrecioProductoOrdenCompra.Encabezado = "Precio";
        ColPrecioProductoOrdenCompra.Buscador = "false";
        ColPrecioProductoOrdenCompra.Formato = "FormatoMoneda";
        ColPrecioProductoOrdenCompra.Alineacion = "right";
        ColPrecioProductoOrdenCompra.Ancho = "30";
        GridDetalleOrdenCompra.Columnas.Add(ColPrecioProductoOrdenCompra);

        //TotalProductoOrdenCompra
        CJQColumn ColTotalProductoOrdenCompra = new CJQColumn();
        ColTotalProductoOrdenCompra.Nombre = "Total";
        ColTotalProductoOrdenCompra.Encabezado = "Total";
        ColTotalProductoOrdenCompra.Buscador = "false";
        ColTotalProductoOrdenCompra.Formato = "FormatoMoneda";
        ColTotalProductoOrdenCompra.Alineacion = "right";
        ColTotalProductoOrdenCompra.Ancho = "30";
        GridDetalleOrdenCompra.Columnas.Add(ColTotalProductoOrdenCompra);

        //TipoMoneda produto orden de compra
        CJQColumn ColTipoMonedaOrdenCompra = new CJQColumn();
        ColTipoMonedaOrdenCompra.Nombre = "TipoMoneda";
        ColTipoMonedaOrdenCompra.Encabezado = "Tipo de moneda";
        ColTipoMonedaOrdenCompra.Ancho = "40";
        ColTipoMonedaOrdenCompra.Alineacion = "left";
        ColTipoMonedaOrdenCompra.Buscador = "false";
        GridDetalleOrdenCompra.Columnas.Add(ColTipoMonedaOrdenCompra);

        //SeleccionarDetalleOrdenCompra
        CJQColumn ColSeleccionarDetalleOrdenCompra = new CJQColumn();
        ColSeleccionarDetalleOrdenCompra.Nombre = "Seleccionar";
        ColSeleccionarDetalleOrdenCompra.Encabezado = "Seleccionar";
        ColSeleccionarDetalleOrdenCompra.Etiquetado = "Imagen";
        ColSeleccionarDetalleOrdenCompra.Imagen = "select.png";
        ColSeleccionarDetalleOrdenCompra.Estilo = "divImagenConsultar imgSeleccionarDetalleOrdenCompra";
        ColSeleccionarDetalleOrdenCompra.Buscador = "false";
        ColSeleccionarDetalleOrdenCompra.Ordenable = "false";
        ColSeleccionarDetalleOrdenCompra.Ancho = "25";
        GridDetalleOrdenCompra.Columnas.Add(ColSeleccionarDetalleOrdenCompra);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleOrdenCompra", GridDetalleOrdenCompra.GeneraGrid(), true);

        GenerarGridFacturaProveedorPorValidar(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerEncabezadoFacturaProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNumeroFactura, string pRazonSocial, string pDivision, int pIdEstatusRecepcion, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pNumeroSerie, string pClave, string pNumeroPedido, int pBusquedaDocumento)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEncabezadoFacturaProveedor", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pDivision", SqlDbType.VarChar, 250).Value = Convert.ToString(pDivision);
        Stored.Parameters.Add("pIdEstatusRecepcion", SqlDbType.Int).Value = pIdEstatusRecepcion;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pNumeroSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroSerie);
		Stored.Parameters.Add("pClave", SqlDbType.VarChar, 250).Value = Convert.ToString(pClave);
        Stored.Parameters.Add("pBusquedaDocumento", SqlDbType.Int).Value = pBusquedaDocumento;
        Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturasPendientesPorValidar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, int pAI, string pNumeroFactura, string pDivision)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturasPendientesPorValidar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pDivision", SqlDbType.VarChar, 250).Value = Convert.ToString(pDivision);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleFacturaProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEncabezadoFacturaProveedor)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEncabezadoFacturaProveedor", SqlDbType.Int).Value = pIdEncabezadoFacturaProveedor;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleFacturaProveedorConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEncabezadoFacturaProveedor)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleFacturaProveedorConsultaConsultar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEncabezadoFacturaProveedor", SqlDbType.Int).Value = pIdEncabezadoFacturaProveedor;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleFacturaProveedorEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEncabezadoFacturaProveedor)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEncabezadoFacturaProveedor", SqlDbType.Int).Value = pIdEncabezadoFacturaProveedor;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProductoNumeroSerie(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdProducto, int pCantidad, decimal pCosto, decimal pDescuento, decimal pCostoDescuento, decimal pTipoCambioFactura, decimal pTipoCambioDetalle)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleFacturaProveedorConsultaProducto", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdProducto", SqlDbType.Int).Value = pIdProducto;
        Stored.Parameters.Add("pCantidad", SqlDbType.Int).Value = pCantidad;
        Stored.Parameters.Add("pCosto", SqlDbType.Decimal).Value = pCosto;
        Stored.Parameters.Add("pDescuento", SqlDbType.Decimal).Value = pDescuento;
        Stored.Parameters.Add("pCostoDescuento", SqlDbType.Decimal).Value = pCostoDescuento;
        Stored.Parameters.Add("pTipoCambioFactura", SqlDbType.Decimal).Value = pTipoCambioFactura;
        Stored.Parameters.Add("pTipoCambioDetalle", SqlDbType.Decimal).Value = pTipoCambioDetalle;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetallePedido(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPedido)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetallePedidoFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdPedido", SqlDbType.Int).Value = pIdPedido;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleOrdenCompra(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdOrdenCompra)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleOrdenCompraFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdOrdenCompra", SqlDbType.Int).Value = pIdOrdenCompra;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string BuscarEncabezadoFacturaProveedor(string pEncabezadoFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonEncabezadoFacturaProveedor = new CJson();
        jsonEncabezadoFacturaProveedor.StoredProcedure.CommandText = "sp_EncabezadoFacturaProveedor_Consultar_FiltroPorEncabezadoFacturaProveedor";
        jsonEncabezadoFacturaProveedor.StoredProcedure.Parameters.AddWithValue("@pEncabezadoFacturaProveedor", pEncabezadoFacturaProveedor);
        return jsonEncabezadoFacturaProveedor.ObtenerJsonString(ConexionBaseDatos);
    }

    private void GenerarGridFacturaProveedorPorValidar(CConexion pConexion)
    {

        //GridFacturasPendientesPorValidar
        CJQGrid GridFacturasPendientesPorValidar = new CJQGrid();
        GridFacturasPendientesPorValidar.NombreTabla = "grdFacturasPendientesPorValidar";
        GridFacturasPendientesPorValidar.CampoIdentificador = "IdEncabezadoFacturaProveedor";
        GridFacturasPendientesPorValidar.ColumnaOrdenacion = "NumeroFacturaValidar";
        GridFacturasPendientesPorValidar.Metodo = "ObtenerFacturasPendientesPorValidar";
        GridFacturasPendientesPorValidar.TituloTabla = "Facturas pendientes por validar";
        GridFacturasPendientesPorValidar.GenerarFuncionFiltro = false;
        GridFacturasPendientesPorValidar.GenerarFuncionTerminado = false;
        GridFacturasPendientesPorValidar.NumeroRegistros = 15;
        GridFacturasPendientesPorValidar.RangoNumeroRegistros = "15,30,60";

        //IdEncabezadoFacturaProveedor
        CJQColumn ColFPVIdEncabezadoFacturaProveedor = new CJQColumn();
        ColFPVIdEncabezadoFacturaProveedor.Nombre = "IdEncabezadoFacturaProveedor";
        ColFPVIdEncabezadoFacturaProveedor.Oculto = "true";
        ColFPVIdEncabezadoFacturaProveedor.Encabezado = "IdEncabezadoFacturaProveedor";
        ColFPVIdEncabezadoFacturaProveedor.Buscador = "false";
        ColFPVIdEncabezadoFacturaProveedor.Ancho = "5";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVIdEncabezadoFacturaProveedor);

        //NumeroFactura
        CJQColumn ColFPVNumeroFactura = new CJQColumn();
        ColFPVNumeroFactura.Nombre = "NumeroFacturaValidar";
        ColFPVNumeroFactura.Encabezado = "No. factura";
        ColFPVNumeroFactura.Ancho = "40";
        ColFPVNumeroFactura.Alineacion = "left";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVNumeroFactura);

        //Proveedor
        CJQColumn ColFPVNombreProveedor = new CJQColumn();
        ColFPVNombreProveedor.Nombre = "RazonSocialValidar";
        ColFPVNombreProveedor.Encabezado = "Razón social";
        ColFPVNombreProveedor.Ancho = "120";
        ColFPVNombreProveedor.Alineacion = "left";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVNombreProveedor);

        //FechaFactura
        CJQColumn ColFPVFechaFactura = new CJQColumn();
        ColFPVFechaFactura.Nombre = "Fecha";
        ColFPVFechaFactura.Encabezado = "Fecha de factura";
        ColFPVFechaFactura.Ancho = "40";
        ColFPVFechaFactura.Alineacion = "left";
        ColFPVFechaFactura.Buscador = "false";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVFechaFactura);


        //FechaPago
        CJQColumn ColFPVFechaPago = new CJQColumn();
        ColFPVFechaPago.Nombre = "FechaPago";
        ColFPVFechaPago.Encabezado = "Fecha de pago";
        ColFPVFechaPago.Ancho = "40";
        ColFPVFechaPago.Alineacion = "left";
        ColFPVFechaPago.Buscador = "false";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVFechaPago);

        //TipoMoneda
        CJQColumn ColFPVTipoMoneda = new CJQColumn();
        ColFPVTipoMoneda.Nombre = "TipoMoneda";
        ColFPVTipoMoneda.Encabezado = "Tipo de moneda";
        ColFPVTipoMoneda.Ancho = "40";
        ColFPVTipoMoneda.Alineacion = "left";
        ColFPVTipoMoneda.Buscador = "false";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVTipoMoneda);

        //Total
        CJQColumn ColFPVTotal = new CJQColumn();
        ColFPVTotal.Nombre = "Total";
        ColFPVTotal.Encabezado = "Total";
        ColFPVTotal.Buscador = "false";
        ColFPVTotal.Alineacion = "right";
        ColFPVTotal.Ancho = "50";
        ColFPVTotal.Formato = "FormatoMoneda";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVTotal);

        //Division
        CJQColumn ColFPVDivision = new CJQColumn();
        ColFPVDivision.Nombre = "DivisionValidar";
        ColFPVDivision.Encabezado = "División";
        ColFPVDivision.Ancho = "60";
        ColFPVDivision.Alineacion = "left";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVDivision);

        //Baja
        CJQColumn ColFPVBaja = new CJQColumn();
        ColFPVBaja.Nombre = "AIValidar";
        ColFPVBaja.Encabezado = "A/I";
        ColFPVBaja.Ordenable = "false";
        ColFPVBaja.Etiquetado = "A/I";
        ColFPVBaja.Ancho = "30";
        ColFPVBaja.Buscador = "true";
        ColFPVBaja.TipoBuscador = "Combo";
        ColFPVBaja.Oculto = puedeEliminarEncabezadoFacturaProveedor == 1 ? "false" : "true";
        ColFPVBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVBaja);

        //Revision
        CJQColumn ColFPVRevision = new CJQColumn();
        ColFPVRevision.Nombre = "R";
        ColFPVRevision.Encabezado = "R";
        ColFPVRevision.Etiquetado = "Imagen";
        ColFPVRevision.Imagen = "fileok.png";
        ColFPVRevision.Estilo = "divImagenRevisada imgFormaMarcarComoRevisada";
        ColFPVRevision.Buscador = "false";
        ColFPVRevision.Ordenable = "false";
        ColFPVRevision.Ancho = "25";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVRevision);

        //Consultar
        CJQColumn ColFPVConsultar = new CJQColumn();
        ColFPVConsultar.Nombre = "Consultar";
        ColFPVConsultar.Encabezado = "Ver";
        ColFPVConsultar.Etiquetado = "ImagenConsultarOC";
        ColFPVConsultar.Estilo = "divImagenConsultar imgFormaConsultarFacturasPendientesPorValidar";
        ColFPVConsultar.Buscador = "false";
        ColFPVConsultar.Ordenable = "false";
        ColFPVConsultar.Ancho = "25";
        GridFacturasPendientesPorValidar.Columnas.Add(ColFPVConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturasPendientesPorValidar", GridFacturasPendientesPorValidar.GeneraGrid(), true);
    }

    [WebMethod]
    public static string LlenaComboAlmacen()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        int IdSucursal = Usuario.IdSucursalActual;

        JObject almacen = new JObject();
        CJson jsonAlmacenAsignado = new CJson();
        jsonAlmacenAsignado.StoredProcedure.CommandText = "sp_EncabezadoFacturaProveedor_ConsultarAlmacenAsignado";
        jsonAlmacenAsignado.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", IdSucursal);
        almacen.Add("Opciones", jsonAlmacenAsignado.ObtenerJsonJObject(ConexionBaseDatos));

        oRespuesta.Add(new JProperty("Modelo", almacen));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string AgregarEncabezadoFacturaProveedor(Dictionary<string, object> pEncabezadoFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedor.NumeroFactura = Convert.ToString(pEncabezadoFacturaProveedor["NumeroFactura"]);
            EncabezadoFacturaProveedor.IdProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdProveedor"]);
            EncabezadoFacturaProveedor.IdTipoMoneda = Convert.ToInt32(pEncabezadoFacturaProveedor["IdCondicionPago"]);
            EncabezadoFacturaProveedor.IdDivision = Convert.ToInt32(pEncabezadoFacturaProveedor["IdDivision"]);
            EncabezadoFacturaProveedor.IdTipoMoneda = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoMoneda"]);
            EncabezadoFacturaProveedor.Fecha = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaFactura"]);
            EncabezadoFacturaProveedor.FechaPago = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaPago"]);
            EncabezadoFacturaProveedor.NumeroGuia = Convert.ToString(pEncabezadoFacturaProveedor["NumeroGuia"]);
            EncabezadoFacturaProveedor.TipoCambio = Convert.ToDecimal(pEncabezadoFacturaProveedor["TipoCambioFactura"]);
            EncabezadoFacturaProveedor.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            EncabezadoFacturaProveedor.FechaCaptura = Convert.ToDateTime(DateTime.Now);
            EncabezadoFacturaProveedor.IdAlmacen = Convert.ToInt32(pEncabezadoFacturaProveedor["IdAlmacen"]);
            EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor = 4;
            string validacion = ValidarEncabezadoFacturaProveedor(EncabezadoFacturaProveedor, ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                EncabezadoFacturaProveedor.Agregar(ConexionBaseDatos);

                CEncabezadoFacturaProveedorSucursal EncabezadoFacturaProveedorSucursal = new CEncabezadoFacturaProveedorSucursal();
                EncabezadoFacturaProveedorSucursal.IdEncabezadoFacturaProveedor = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;
                EncabezadoFacturaProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                EncabezadoFacturaProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                EncabezadoFacturaProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                EncabezadoFacturaProveedorSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nueva factura de proveedor";
                HistorialGenerico.AgregarHistorialGenerico("EncabezadoFacturaProveedor", ConexionBaseDatos);

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

                if (Convert.ToInt32(pTipoCambio["IdTipoCambioOrigen"]) != 1)
                {
                    Dictionary<string, object> Parametros = new Dictionary<string, object>();
                    Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(pTipoCambio["IdTipoCambioOrigen"]));
                    Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
                    Parametros.Add("Fecha", DateTime.Today);
                    TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
                    Modelo.Add("TipoCambioFactura", TipoCambio.TipoCambio);
                }
                else
                {
                    Modelo.Add("TipoCambioFactura", 1);
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
    public static string AgregarDetalleFacturaProveedorNormal(Dictionary<string, object> pEncabezadoFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CCliente Cliente = new CCliente();
            CProyecto Proyecto = new CProyecto();
            COrganizacion Organizacion = new COrganizacion();
            DetalleFacturaProveedor.IdEncabezadoFacturaProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdEncabezadoFacturaProveedor"]);
            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdProducto"]) != 0)
            {
                Producto.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdProducto"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdProducto = Producto.IdProducto;
                DetalleFacturaProveedor.Tipo = "Producto";
                DetalleFacturaProveedor.Clave = Convert.ToString(Producto.Clave);
                DetalleFacturaProveedor.Descripcion = Convert.ToString(Producto.Producto);
                DetalleFacturaProveedor.IdAlmacen = Convert.ToInt32(pEncabezadoFacturaProveedor["IdAlmacen"]);
            }
            else
            {
                Servicio.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdServicio"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdServicio = Servicio.IdServicio;
                DetalleFacturaProveedor.Tipo = "Servicio";
                DetalleFacturaProveedor.Clave = Convert.ToString(Servicio.Clave);
                DetalleFacturaProveedor.Descripcion = Convert.ToString(Servicio.Servicio);
                DetalleFacturaProveedor.IdAlmacen = 0;
            }
            DetalleFacturaProveedor.Cantidad = Convert.ToDecimal(pEncabezadoFacturaProveedor["Cantidad"]);
            DetalleFacturaProveedor.Descuento = Convert.ToDecimal(pEncabezadoFacturaProveedor["Descuento"]);
            DetalleFacturaProveedor.Precio = Convert.ToDecimal(pEncabezadoFacturaProveedor["Costo"]);
            DetalleFacturaProveedor.Total = Convert.ToDecimal(pEncabezadoFacturaProveedor["Total"]);
            DetalleFacturaProveedor.IdUnidadCompraVenta = Convert.ToInt32(pEncabezadoFacturaProveedor["IdUnidadCompraVenta"]);
            DetalleFacturaProveedor.Cantidad = Convert.ToDecimal(pEncabezadoFacturaProveedor["Cantidad"]);
            DetalleFacturaProveedor.NumeroSerie = Convert.ToString(pEncabezadoFacturaProveedor["NumeroSerie"]);
            COrdenCompraDetalle OrdenCompraDetalle = new COrdenCompraDetalle();
            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdCotizacionDetalle"]) != 0)
            {
                DetalleFacturaProveedor.IdPedidoDetalle = Convert.ToInt32(pEncabezadoFacturaProveedor["IdCotizacionDetalle"]);

                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdPedidoDetalle", Convert.ToInt32(pEncabezadoFacturaProveedor["IdCotizacionDetalle"]));
                OrdenCompraDetalle.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                DetalleFacturaProveedor.IdOrdenCompraDetalle = OrdenCompraDetalle.IdOrdenCompraDetalle;
            }

            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdOrdenCompraDetalle"]) != 0)
            {
                OrdenCompraDetalle.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdOrdenCompraDetalle"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdOrdenCompraDetalle = Convert.ToInt32(pEncabezadoFacturaProveedor["IdOrdenCompraDetalle"]);
                DetalleFacturaProveedor.IdPedidoDetalle = OrdenCompraDetalle.IdPedidoDetalle;
                DetalleFacturaProveedor.Descripcion = OrdenCompraDetalle.Descripcion;
            }

            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdCliente"]) != 0)
            {

                Cliente.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdCliente"]), ConexionBaseDatos);
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);
                DetalleFacturaProveedor.IdCliente = Convert.ToInt32(pEncabezadoFacturaProveedor["IdCliente"]);
                DetalleFacturaProveedor.ClienteProyecto = Convert.ToString(Organizacion.RazonSocial);
            }
            else
            {
                Proyecto.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdProyecto"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdProyecto = Convert.ToInt32(pEncabezadoFacturaProveedor["IdProyecto"]);
                DetalleFacturaProveedor.ClienteProyecto = Convert.ToString(Proyecto.NombreProyecto);
            }
            DetalleFacturaProveedor.FechaFacturacion = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaFactura"]);
            DetalleFacturaProveedor.FechaAlta = Convert.ToDateTime(DateTime.Now);
            DetalleFacturaProveedor.IdTipoCompra = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoCompra"]);
            DetalleFacturaProveedor.IdUsuarioSolicito = Convert.ToInt32(pEncabezadoFacturaProveedor["IdUsuarioSolicito"]);
            DetalleFacturaProveedor.IdTipoIVA = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoIVA"]);
            DetalleFacturaProveedor.IVA = Convert.ToDecimal(pEncabezadoFacturaProveedor["IVA"]);
            DetalleFacturaProveedor.IdSubCuentaContable = Convert.ToInt32(pEncabezadoFacturaProveedor["IdSubCuentaContable"]);

            string validacion = ValidarDetalleFacturaProveedor(DetalleFacturaProveedor, ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                if (DetalleFacturaProveedor.IdEncabezadoFacturaProveedor != 0)
                {
                    DetalleFacturaProveedor.AgregarDetalleFacturaProveedor(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = DetalleFacturaProveedor.IdDetalleFacturaProveedor;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva partida de factura de proveedor";
                    HistorialGenerico.AgregarHistorialGenerico("DetalleFacturaProveedor", ConexionBaseDatos);

                }
                else
                {
                    CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
                    EncabezadoFacturaProveedor.NumeroFactura = Convert.ToString(pEncabezadoFacturaProveedor["NumeroFactura"]);
                    EncabezadoFacturaProveedor.IdProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdProveedor"]);
                    EncabezadoFacturaProveedor.IdCondicionPago = Convert.ToInt32(pEncabezadoFacturaProveedor["IdCondicionPago"]);
                    EncabezadoFacturaProveedor.IdDivision = Convert.ToInt32(pEncabezadoFacturaProveedor["IdDivision"]);
                    EncabezadoFacturaProveedor.IdTipoMoneda = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoMoneda"]);
                    EncabezadoFacturaProveedor.Fecha = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaFactura"]);
                    EncabezadoFacturaProveedor.FechaPago = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaPago"]);
                    EncabezadoFacturaProveedor.NumeroGuia = Convert.ToString(pEncabezadoFacturaProveedor["NumeroGuia"]);
                    EncabezadoFacturaProveedor.TipoCambio = Convert.ToDecimal(pEncabezadoFacturaProveedor["TipoCambioFactura"]);
                    EncabezadoFacturaProveedor.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    EncabezadoFacturaProveedor.IdAlmacen = Convert.ToInt32(pEncabezadoFacturaProveedor["IdAlmacen"]);
                    EncabezadoFacturaProveedor.FechaCaptura = Convert.ToDateTime(DateTime.Now);
                    EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor = 4;
                    EncabezadoFacturaProveedor.Agregar(ConexionBaseDatos);

                    CEncabezadoFacturaProveedorSucursal EncabezadoFacturaProveedorSucursal = new CEncabezadoFacturaProveedorSucursal();
                    EncabezadoFacturaProveedorSucursal.IdEncabezadoFacturaProveedor = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;
                    EncabezadoFacturaProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                    EncabezadoFacturaProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    EncabezadoFacturaProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    EncabezadoFacturaProveedorSucursal.Agregar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva factura de proveedor";
                    HistorialGenerico.AgregarHistorialGenerico("EncabezadoFacturaProveedor", ConexionBaseDatos);

                    DetalleFacturaProveedor.IdEncabezadoFacturaProveedor = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;
                    DetalleFacturaProveedor.AgregarDetalleFacturaProveedor(ConexionBaseDatos);

                    HistorialGenerico.IdGenerico = DetalleFacturaProveedor.IdDetalleFacturaProveedor;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva partida de factura de proveedor";
                    HistorialGenerico.AgregarHistorialGenerico("DetalleFacturaProveedor", ConexionBaseDatos);
                }

                string TotalLetras = "";

                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CEncabezadoFacturaProveedor EncabezadoFacturaProveedorTotal = new CEncabezadoFacturaProveedor();
                EncabezadoFacturaProveedorTotal.LlenaObjeto(Convert.ToInt32(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(EncabezadoFacturaProveedorTotal.IdTipoMoneda, ConexionBaseDatos);
                TotalLetras = Utilerias.ConvertLetter(EncabezadoFacturaProveedorTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
                EncabezadoFacturaProveedorTotal.TotalLetra = TotalLetras;
                EncabezadoFacturaProveedorTotal.Editar(ConexionBaseDatos);

                // Actualiza Proyecto
                if (DetalleFacturaProveedor.IdProyecto != 0)
                {
                    CProyecto.ActualizarTotales(DetalleFacturaProveedor.IdProyecto, ConexionBaseDatos);
                }

                oRespuesta.Add("IdEncabezadoFacturaProveedor", DetalleFacturaProveedor.IdEncabezadoFacturaProveedor);
                oRespuesta.Add("SubtotalFactura", EncabezadoFacturaProveedorTotal.Subtotal);
                oRespuesta.Add("IVAProveedor", EncabezadoFacturaProveedorTotal.IVA);
                oRespuesta.Add("TotalIVA", EncabezadoFacturaProveedorTotal.IVA);
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
    public static string AgregarPartidasDetalleFacturaProveedor(Dictionary<string, object> pEncabezadoFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CCliente Cliente = new CCliente();
            CProyecto Proyecto = new CProyecto();
            COrganizacion Organizacion = new COrganizacion();
            DetalleFacturaProveedor.IdEncabezadoFacturaProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdEncabezadoFacturaProveedor"]);
            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdProducto"]) != 0)
            {
                Producto.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdProducto"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdProducto = Producto.IdProducto;
                DetalleFacturaProveedor.Tipo = "Producto";
                DetalleFacturaProveedor.Clave = Convert.ToString(Producto.Clave);
                DetalleFacturaProveedor.Descripcion = Convert.ToString(Producto.Producto);
                DetalleFacturaProveedor.IdAlmacen = Convert.ToInt32(pEncabezadoFacturaProveedor["IdAlmacen"]);
            }
            else
            {
                Servicio.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdServicio"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdServicio = Servicio.IdServicio;
                DetalleFacturaProveedor.Tipo = "Servicio";
                DetalleFacturaProveedor.Clave = Convert.ToString(Servicio.Clave);
                DetalleFacturaProveedor.Descripcion = Convert.ToString(Servicio.Servicio);
            }
            DetalleFacturaProveedor.Cantidad = Convert.ToDecimal(pEncabezadoFacturaProveedor["Cantidad"]);
            DetalleFacturaProveedor.Descuento = Convert.ToDecimal(pEncabezadoFacturaProveedor["Descuento"]);
            DetalleFacturaProveedor.Precio = Convert.ToDecimal(pEncabezadoFacturaProveedor["Costo"]);
            DetalleFacturaProveedor.Total = Convert.ToDecimal(pEncabezadoFacturaProveedor["Total"]);
            DetalleFacturaProveedor.IdUnidadCompraVenta = Convert.ToInt32(pEncabezadoFacturaProveedor["IdUnidadCompraVenta"]);
            DetalleFacturaProveedor.Cantidad = Convert.ToDecimal(pEncabezadoFacturaProveedor["Cantidad"]);
            DetalleFacturaProveedor.NumeroSerie = Convert.ToString(pEncabezadoFacturaProveedor["NumeroSerie"]);
            DetalleFacturaProveedor.IdTipoIVA = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoIVA"]);
            DetalleFacturaProveedor.IVA = Convert.ToDecimal(pEncabezadoFacturaProveedor["IVA"]);

            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdCotizacionDetalle"]) != 0)
            {
                DetalleFacturaProveedor.IdPedidoDetalle = Convert.ToInt32(pEncabezadoFacturaProveedor["IdCotizacionDetalle"]);
            }

            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdOrdenCompraDetalle"]) != 0)
            {
                COrdenCompraDetalle OrdenCompraDetalle = new COrdenCompraDetalle();
                OrdenCompraDetalle.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdOrdenCompraDetalle"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdOrdenCompraDetalle = Convert.ToInt32(pEncabezadoFacturaProveedor["IdOrdenCompraDetalle"]);
                DetalleFacturaProveedor.IdPedidoDetalle = OrdenCompraDetalle.IdPedidoDetalle;
            }

            if (Convert.ToInt32(pEncabezadoFacturaProveedor["IdCliente"]) != 0)
            {

                Cliente.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdCliente"]), ConexionBaseDatos);
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);
                DetalleFacturaProveedor.IdCliente = Convert.ToInt32(pEncabezadoFacturaProveedor["IdCliente"]);
                DetalleFacturaProveedor.ClienteProyecto = Convert.ToString(Organizacion.RazonSocial);
            }
            else
            {
                Proyecto.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdProyecto"]), ConexionBaseDatos);
                DetalleFacturaProveedor.IdProyecto = Convert.ToInt32(pEncabezadoFacturaProveedor["IdProyecto"]);
                DetalleFacturaProveedor.ClienteProyecto = Convert.ToString(Proyecto.NombreProyecto);
            }
            DetalleFacturaProveedor.FechaFacturacion = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaFactura"]);
            DetalleFacturaProveedor.FechaAlta = Convert.ToDateTime(DateTime.Now);
            DetalleFacturaProveedor.IdTipoCompra = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoCompra"]);
            DetalleFacturaProveedor.IdUsuarioSolicito = Convert.ToInt32(pEncabezadoFacturaProveedor["IdUsuarioSolicito"]);
            DetalleFacturaProveedor.IdSubCuentaContable = Convert.ToInt32(pEncabezadoFacturaProveedor["IdSubCuentaContable"]);

            string validacion = ValidarDetalleFacturaProveedor(DetalleFacturaProveedor, ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                if (DetalleFacturaProveedor.IdEncabezadoFacturaProveedor != 0)
                {

                    foreach (Dictionary<string, object> oPartidas in (Array)pEncabezadoFacturaProveedor["DetallePartidas"])
                    {

                        DetalleFacturaProveedor.Precio = Convert.ToDecimal(oPartidas["Precio"]);
                        DetalleFacturaProveedor.Descuento = Convert.ToDecimal(oPartidas["Descuento"]);
                        DetalleFacturaProveedor.Cantidad = 1;
                        DetalleFacturaProveedor.Total = Convert.ToDecimal(oPartidas["Total"]);
                        DetalleFacturaProveedor.NumeroSerie = Convert.ToString(oPartidas["NumeroSerie"]);

                        DetalleFacturaProveedor.AgregarDetalleFacturaProveedor(ConexionBaseDatos);
                        CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                        HistorialGenerico.IdGenerico = DetalleFacturaProveedor.IdDetalleFacturaProveedor;
                        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                        HistorialGenerico.Comentario = "Se inserto una nueva partida de factura de proveedor";
                        HistorialGenerico.AgregarHistorialGenerico("DetalleFacturaProveedor", ConexionBaseDatos);

                    }

                }
                else
                {
                    CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
                    EncabezadoFacturaProveedor.NumeroFactura = Convert.ToString(pEncabezadoFacturaProveedor["NumeroFactura"]);
                    EncabezadoFacturaProveedor.IdProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdProveedor"]);
                    EncabezadoFacturaProveedor.IdDivision = Convert.ToInt32(pEncabezadoFacturaProveedor["IdDivision"]);
                    EncabezadoFacturaProveedor.IdTipoMoneda = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoMoneda"]);
                    EncabezadoFacturaProveedor.Fecha = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaFactura"]);
                    EncabezadoFacturaProveedor.FechaPago = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaPago"]);
                    EncabezadoFacturaProveedor.NumeroGuia = Convert.ToString(pEncabezadoFacturaProveedor["NumeroGuia"]);
                    EncabezadoFacturaProveedor.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    EncabezadoFacturaProveedor.IdAlmacen = Convert.ToInt32(pEncabezadoFacturaProveedor["IdAlmacen"]);
                    EncabezadoFacturaProveedor.FechaCaptura = Convert.ToDateTime(DateTime.Now);
                    EncabezadoFacturaProveedor.TipoCambio = Convert.ToDecimal(pEncabezadoFacturaProveedor["TipoCambioFactura"]);
                    EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor = 4;
                    EncabezadoFacturaProveedor.Agregar(ConexionBaseDatos);

                    CEncabezadoFacturaProveedorSucursal EncabezadoFacturaProveedorSucursal = new CEncabezadoFacturaProveedorSucursal();
                    EncabezadoFacturaProveedorSucursal.IdEncabezadoFacturaProveedor = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;
                    EncabezadoFacturaProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                    EncabezadoFacturaProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    EncabezadoFacturaProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    EncabezadoFacturaProveedorSucursal.Agregar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva factura de proveedor";
                    HistorialGenerico.AgregarHistorialGenerico("EncabezadoFacturaProveedor", ConexionBaseDatos);

                    DetalleFacturaProveedor.IdEncabezadoFacturaProveedor = EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor;

                    foreach (Dictionary<string, object> oPartidas in (Array)pEncabezadoFacturaProveedor["DetallePartidas"])
                    {

                        DetalleFacturaProveedor.Precio = Convert.ToDecimal(oPartidas["Precio"]);
                        DetalleFacturaProveedor.Descuento = Convert.ToDecimal(oPartidas["Descuento"]);
                        DetalleFacturaProveedor.Cantidad = 1;
                        DetalleFacturaProveedor.Total = Convert.ToDecimal(oPartidas["Total"]);
                        DetalleFacturaProveedor.NumeroSerie = Convert.ToString(oPartidas["NumeroSerie"]);

                        DetalleFacturaProveedor.AgregarDetalleFacturaProveedor(ConexionBaseDatos);

                        HistorialGenerico.IdGenerico = DetalleFacturaProveedor.IdDetalleFacturaProveedor;
                        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                        HistorialGenerico.Comentario = "Se inserto una nueva partida de factura de proveedor";
                        HistorialGenerico.AgregarHistorialGenerico("DetalleFacturaProveedor", ConexionBaseDatos);

                    }
                }

                string TotalLetras = "";

                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CEncabezadoFacturaProveedor EncabezadoFacturaProveedorTotal = new CEncabezadoFacturaProveedor();
                EncabezadoFacturaProveedorTotal.LlenaObjeto(Convert.ToInt32(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(EncabezadoFacturaProveedorTotal.IdTipoMoneda, ConexionBaseDatos);
                TotalLetras = Utilerias.ConvertLetter(EncabezadoFacturaProveedorTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
                EncabezadoFacturaProveedorTotal.TotalLetra = TotalLetras;
                EncabezadoFacturaProveedorTotal.Editar(ConexionBaseDatos);
                oRespuesta.Add("IdEncabezadoFacturaProveedor", DetalleFacturaProveedor.IdEncabezadoFacturaProveedor);
                oRespuesta.Add("SubtotalFactura", EncabezadoFacturaProveedorTotal.Subtotal);
                oRespuesta.Add("IVAProveedor", EncabezadoFacturaProveedorTotal.IVA);
                oRespuesta.Add("TotalIVA", EncabezadoFacturaProveedorTotal.IVA);
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

    //[WebMethod]
    //public static string GenerarAsientoContable(Dictionary<string, object> pEncabezadoFacturaProveedor)
    //{
    //    //Abrir Conexion
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //    //¿La conexion se establecio?
    //    if (respuesta == "Conexion Establecida")
    //    {

    //        CAsiento Asiento = new CAsiento();
    //        Asiento.IdEncabezadoFacturaProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdEncabezadoFacturaProveedor"]);
    //        Asiento.CuentaContable = Convert.ToString(pEncabezadoFacturaProveedor["CuentaContable"]);
    //        Asiento.AgregarAsiento(ConexionBaseDatos);
    //        string validacion = "";

    //        JObject oRespuesta = new JObject();
    //        if (validacion == "")
    //        {
    //            oRespuesta.Add(new JProperty("Error", 0));
    //            oRespuesta.Add(new JProperty("Descripcion", "El Asiento se genero correctamente"));
    //        }
    //        else
    //        {
    //            oRespuesta.Add(new JProperty("Error", 1));
    //            oRespuesta.Add(new JProperty("Descripcion", "Error al procesar el asiento"));
    //        }
    //        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //        return oRespuesta.ToString();
    //    }
    //    else
    //    { return "1|" + respuesta; }
    //}

    [WebMethod]
    public static string CancelarFacturaProveedor(Dictionary<string, object> pEncabezadoFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int PuedeCancelarFactura = 0;
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdEncabezadoFacturaProveedor"]);
            PuedeCancelarFactura = EncabezadoFacturaProveedor.ValidaCancelarEncabezadoFacturaProveedor(Convert.ToInt32(pEncabezadoFacturaProveedor["IdEncabezadoFacturaProveedor"]), ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (PuedeCancelarFactura == 0)
            {
                EncabezadoFacturaProveedor.CancelarFacturaProveedor(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "La factura se cancelo correctamente"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta factura de proveedor porque existen movimientos con alguna de sus partidas"));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdEncabezadoFacturaProveedor, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        int PuedeCancelarFactura = 0;
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = pIdEncabezadoFacturaProveedor;
            EncabezadoFacturaProveedor.Baja = pBaja;
            PuedeCancelarFactura = EncabezadoFacturaProveedor.ValidaCancelarEncabezadoFacturaProveedor(Convert.ToInt32(pIdEncabezadoFacturaProveedor), ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (PuedeCancelarFactura == 0)
            {
                EncabezadoFacturaProveedor.CancelarFacturaProveedor(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "La factura se cancelo correctamente"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta factura de proveedor porque existen movimientos con alguna de sus partidas"));
            }
            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }

        else
        { return "1|" + respuesta; }

    }

    [WebMethod]
    public static string CambiarEstatusRevisada(int pIdEncabezadoFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedor.LlenaObjeto(pIdEncabezadoFacturaProveedor, ConexionBaseDatos);
            EncabezadoFacturaProveedor.FechaRevision = DateTime.Now;
            EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor = 5;
            EncabezadoFacturaProveedor.IdUsuarioRevision = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"].ToString());
            EncabezadoFacturaProveedor.Editar(ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Descripcion", ""));
            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }

        else
        { return "1|" + respuesta; }

    }

    [WebMethod]
    public static string ActivarEstatus(int pIdEncabezadoFacturaProveedor, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        bool reactiva = true;
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = pIdEncabezadoFacturaProveedor;
            EncabezadoFacturaProveedor.Baja = pBaja;

            CDetalleFacturaProveedor ListaPartidas = new CDetalleFacturaProveedor();

            Dictionary<string, object> ParametrosOCD = new Dictionary<string, object>();
            ParametrosOCD.Add("Baja", 0);
            ParametrosOCD.Add("IdEncabezadoFacturaProveedor", pIdEncabezadoFacturaProveedor);

            foreach (CDetalleFacturaProveedor oDetalleFacturaProveedor in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, ConexionBaseDatos))
            {
                if (Convert.ToInt32(oDetalleFacturaProveedor.IdPedidoDetalle) != 0)
                {
                    CCotizacionDetalle PedidoDetalle = new CCotizacionDetalle();
                    PedidoDetalle.LlenaObjeto(oDetalleFacturaProveedor.IdPedidoDetalle, ConexionBaseDatos);
                    int vCantidad = PedidoDetalle.RecepcionCantidad;
                    vCantidad -= Convert.ToInt32(oDetalleFacturaProveedor.Cantidad);
                    reactiva = !(vCantidad < 0);
                    if (!reactiva) break;

                }
                if (Convert.ToInt32(oDetalleFacturaProveedor.IdOrdenCompraDetalle) != 0)
                {
                    COrdenCompraDetalle OrdenCompraDetalle = new COrdenCompraDetalle();
                    OrdenCompraDetalle.LlenaObjeto(oDetalleFacturaProveedor.IdOrdenCompraDetalle, ConexionBaseDatos);
                    int vCantidad = OrdenCompraDetalle.RecepcionCantidad;
                    vCantidad -= Convert.ToInt32(oDetalleFacturaProveedor.Cantidad);
                    reactiva = !(vCantidad < 0);
                    if (!reactiva) break;
                }
            }


            JObject oRespuesta = new JObject();
            if (reactiva)
            {
                EncabezadoFacturaProveedor.ActivarFacturaProveedor(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "La factura se reactivo correctamente"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede reactivar esta factura de proveedor porque existen movimientos con alguna de sus partidas"));
            }
            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }

        else
        { return "1|" + respuesta; }

    }

    [WebMethod]
    public static string ObtenerFormaEncabezadoFacturaProveedor(int pIdEncabezadoFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEncabezadoFacturaProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarEncabezadoFacturaProveedor = 1;
        }
        oPermisos.Add("puedeEditarEncabezadoFacturaProveedor", puedeEditarEncabezadoFacturaProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEncabezadoFacturaProveedor.ObtenerEncabezadoFacturaProveedor(Modelo, pIdEncabezadoFacturaProveedor, ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarEncabezadoFacturaProveedor(int pIdAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CSucursal Sucursal = new CSucursal();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
            CTipoCambio TipoCambio = new CTipoCambio();
            DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            string validacion = ValidarExisteTipoCambio(TipoCambio, Sucursal, Fecha, ConexionBaseDatos);
            if (validacion == "")
            {
                string CuentaContableGenerada = "";

                JObject Modelo = new JObject();
                Modelo.Add("Divisiones", CSucursalDivision.ObtenerJsonSucursalDivision(Usuario.IdSucursalActual, ConexionBaseDatos));
                Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(Sucursal.IdTipoMoneda, ConexionBaseDatos));
                Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(ConexionBaseDatos));
                Modelo.Add("UnidadCompraVentas", CJson.ObtenerJsonUnidadCompraVenta(ConexionBaseDatos));
                Modelo.Add("TipoCompras", CJson.ObtenerJsonTipoCompra(ConexionBaseDatos));
                Modelo.Add("TipoMonedaConceptos", CTipoMoneda.ObtenerJsonTiposMoneda(Sucursal.IdTipoMoneda, ConexionBaseDatos));
                Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuarioAgenteTodos(Usuario.IdUsuario, ConexionBaseDatos));
                Modelo.Add("Almacenes", CSucursalAccesoAlmacen.ObtenerJsonSucursalAlmacen(Usuario.IdSucursalActual, ConexionBaseDatos));
                Modelo.Add(new JProperty("FechaFactura", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("IdAlmacen", Convert.ToInt32(pIdAlmacen)));

                CAlmacen Almacen = new CAlmacen();
                Almacen.LlenaObjeto(Convert.ToInt32(pIdAlmacen), ConexionBaseDatos);
                Modelo.Add(new JProperty("Almacen", Almacen.Almacen));

                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(1));
                Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
                Parametros.Add("Fecha", DateTime.Today);
                TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
                Modelo.Add("TipoCambioFactura", TipoCambio.TipoCambio);

                CCuentaContable CuentaContable = new CCuentaContable();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
                CuentaContableGenerada = CuentaContable.ObtenerCuentaContableGenerada(Usuario.IdSucursalActual, 0, 0, 0, ConexionBaseDatos);
                Modelo.Add(new JProperty("CuentaContable", CuentaContableGenerada));

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
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
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarEncabezadoFacturaProveedor(int IdEncabezadoFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEncabezadoFacturaProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEncabezadoFacturaProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarEncabezadoFacturaProveedor = 1;
        }
        oPermisos.Add("puedeEditarEncabezadoFacturaProveedor", puedeEditarEncabezadoFacturaProveedor);

        if (respuesta == "Conexion Establecida")
        {
            string CuentaContableGenerada = "";
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CEncabezadoFacturaProveedor.ObtenerEncabezadoFacturaProveedor(Modelo, IdEncabezadoFacturaProveedor, ConexionBaseDatos);
            Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(Convert.ToInt32(Modelo["IdDivision"].ToString()), ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(Convert.ToInt32(Modelo["IdCondicionPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("UnidadCompraVentas", CJson.ObtenerJsonUnidadCompraVenta(ConexionBaseDatos));
            Modelo.Add("TipoCompras", CJson.ObtenerJsonTipoCompra(ConexionBaseDatos));
            Modelo.Add("TipoMonedaConceptos", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuarioAgenteTodos(Usuario.IdUsuario, ConexionBaseDatos));
            Modelo.Add("Almacenes", CSucursalAccesoAlmacen.ObtenerJsonSucursalAlmacen(Usuario.IdSucursalActual, ConexionBaseDatos));

            CCuentaContable CuentaContable = new CCuentaContable();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CuentaContableGenerada = CuentaContable.ObtenerCuentaContableGenerada(Usuario.IdSucursalActual, Convert.ToInt32(Modelo["IdDivision"].ToString()), 0, 0, ConexionBaseDatos);
            Modelo.Add(new JProperty("CuentaContable", CuentaContableGenerada));


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

    //[WebMethod]
    //public static string ObtenerFormaFacturasPendientesPorValidar()
    //{
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
    //    JObject oRespuesta = new JObject();
    //    JObject oPermisos = new JObject();
    //    CUsuario Usuario = new CUsuario();

    //    if (respuesta == "Conexion Establecida")
    //    {
    //        JObject Modelo = new JObject();

    //        Modelo.Add(new JProperty("Permisos", oPermisos));
    //        oRespuesta.Add(new JProperty("Error", 0));
    //        oRespuesta.Add(new JProperty("Modelo", Modelo));
    //        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //    }
    //    else
    //    {
    //        oRespuesta.Add(new JProperty("Error", 1));
    //        oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
    //    }
    //    return oRespuesta.ToString();
    //}

    [WebMethod]
    public static string ObtenerDatosDetallePedido(int pIdDetallePedido)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEncabezadoFacturaProveedor.ObtenerDetallePedido(Modelo, pIdDetallePedido, ConexionBaseDatos);
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
    public static string ObtenerDatosDetalleFacturaProveedor(int pIdDetalleFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CDetalleFacturaProveedor.ObtenerDetalleFacturaProveedor(Modelo, pIdDetalleFacturaProveedor, ConexionBaseDatos);
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
    public static string ObtenerDatosDetalleOrdenCompra(int pIdDetalleOrdenCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEncabezadoFacturaProveedor.ObtenerDetalleOrdenCompra(Modelo, pIdDetalleOrdenCompra, ConexionBaseDatos);
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
    public static string ObtenerListaSubCuentaContable(int pIdCuentaContable, int pIdDivision, int pIdTipoCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonCuentaContable(pIdCuentaContable, Usuario.IdSucursalActual, pIdDivision, pIdTipoCompra, ConexionBaseDatos));
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
    public static string ObtenerCuentaContableGenerada(Dictionary<string, object> pCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            if (Convert.ToInt32(pCuentaContable["IdSubCuentaContable"]) == 0)
            {
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

                CDivision Division = new CDivision();
                Division.LlenaObjeto(Convert.ToInt32(pCuentaContable["IdDivision"]), ConexionBaseDatos);

                CTipoCompra TipoCompra = new CTipoCompra();
                TipoCompra.LlenaObjeto(Convert.ToInt32(pCuentaContable["IdTipoCompra"]), ConexionBaseDatos);

                Modelo.Add("CuentaContable", Sucursal.ClaveCuentaContable + "-" + Division.ClaveCuentaContable + "-" + TipoCompra.ClaveCuentaContable + "-000");
                Modelo.Add("IdCuentaContable", 0);
            }
            else
            {
                CCuentaContable CuentaContable = new CCuentaContable();
                CuentaContable.LlenaObjeto(Convert.ToInt32(pCuentaContable["IdSubCuentaContable"]), ConexionBaseDatos);
                Modelo.Add("CuentaContable", CuentaContable.CuentaContable);
                Modelo.Add("IdCuentaContable", CuentaContable.IdCuentaContable);
            }
            Modelo.Add("IdSucursal", Usuario.IdSucursalActual);
            Modelo.Add("IdDivision", Convert.ToInt32(pCuentaContable["IdDivision"]));
            Modelo.Add("IdTipoCompra", Convert.ToInt32(pCuentaContable["IdTipoCompra"]));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();

            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFormaFiltroEncabezadoFacturaProveedor()
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
    public static string EditarEncabezadoFacturaProveedor(Dictionary<string, object> pEncabezadoFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
        EncabezadoFacturaProveedor.LlenaObjeto(Convert.ToInt32(pEncabezadoFacturaProveedor["IdEncabezadoFacturaProveedor"]), ConexionBaseDatos);
        EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdEncabezadoFacturaProveedor"]);
        EncabezadoFacturaProveedor.IdProveedor = Convert.ToInt32(pEncabezadoFacturaProveedor["IdProveedor"]);
        EncabezadoFacturaProveedor.NumeroFactura = Convert.ToString(pEncabezadoFacturaProveedor["NumeroFactura"]);
        EncabezadoFacturaProveedor.IdCondicionPago = Convert.ToInt32(pEncabezadoFacturaProveedor["IdCondicionPago"]);
        EncabezadoFacturaProveedor.IdDivision = Convert.ToInt32(pEncabezadoFacturaProveedor["IdDivision"]);
        EncabezadoFacturaProveedor.IdTipoMoneda = Convert.ToInt32(pEncabezadoFacturaProveedor["IdTipoMoneda"]);
        EncabezadoFacturaProveedor.Fecha = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaFactura"]);
        EncabezadoFacturaProveedor.FechaPago = Convert.ToDateTime(pEncabezadoFacturaProveedor["FechaPago"]);
        EncabezadoFacturaProveedor.NumeroGuia = Convert.ToString(pEncabezadoFacturaProveedor["NumeroGuia"]);
        EncabezadoFacturaProveedor.TipoCambio = Convert.ToDecimal(pEncabezadoFacturaProveedor["TipoCambioFactura"]);

        string validacion = ValidarEncabezadoFacturaProveedor(EncabezadoFacturaProveedor, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            EncabezadoFacturaProveedor.Editar(ConexionBaseDatos);

            string TotalLetras = "";

            CUtilerias Utilerias = new CUtilerias();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedorTotal = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedorTotal.LlenaObjeto(Convert.ToInt32(EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor), ConexionBaseDatos);
            TipoMoneda.LlenaObjeto(EncabezadoFacturaProveedorTotal.IdTipoMoneda, ConexionBaseDatos);
            TotalLetras = Utilerias.ConvertLetter(EncabezadoFacturaProveedorTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
            EncabezadoFacturaProveedorTotal.TotalLetra = TotalLetras;
            EncabezadoFacturaProveedorTotal.Editar(ConexionBaseDatos);
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
    public static string EliminarDetalleFacturaProveedor(Dictionary<string, object> pDetalleFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        int PuedeEliminarPartida = 0;

        CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
        DetalleFacturaProveedor.LlenaObjeto(Convert.ToInt32(pDetalleFacturaProveedor["pIdDetalleFacturaProveedor"]), ConexionBaseDatos);
        DetalleFacturaProveedor.IdDetalleFacturaProveedor = Convert.ToInt32(pDetalleFacturaProveedor["pIdDetalleFacturaProveedor"]);
        DetalleFacturaProveedor.Baja = true;

        PuedeEliminarPartida = DetalleFacturaProveedor.ValidaEliminarDetalleFacturaProveedor(Convert.ToInt32(pDetalleFacturaProveedor["pIdDetalleFacturaProveedor"]), ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (PuedeEliminarPartida == 0)
        {
            DetalleFacturaProveedor.EliminarDetalleFacturaProveedor(ConexionBaseDatos);
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedor.LlenaObjeto(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor, ConexionBaseDatos);

            string TotalLetras = "";

            CUtilerias Utilerias = new CUtilerias();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedorTotal = new CEncabezadoFacturaProveedor();
            EncabezadoFacturaProveedorTotal.LlenaObjeto(Convert.ToInt32(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor), ConexionBaseDatos);
            TipoMoneda.LlenaObjeto(EncabezadoFacturaProveedorTotal.IdTipoMoneda, ConexionBaseDatos);
            TotalLetras = Utilerias.ConvertLetter(EncabezadoFacturaProveedorTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
            EncabezadoFacturaProveedorTotal.TotalLetra = TotalLetras;
            EncabezadoFacturaProveedorTotal.Editar(ConexionBaseDatos);

            // Actualiza Proyecto
            if (DetalleFacturaProveedor.IdProyecto != 0)
            {
                CProyecto.ActualizarTotales(DetalleFacturaProveedor.IdProyecto, ConexionBaseDatos);
            }

            oRespuesta.Add("SubtotalFactura", EncabezadoFacturaProveedorTotal.Subtotal);
            oRespuesta.Add("TotalIVA", EncabezadoFacturaProveedorTotal.IVA);
            oRespuesta.Add(new JProperty("Error", 0));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No se puede eliminar esta partida porque existen movimientos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    //Busquedas

    [WebMethod]
    public static string BuscarDivision(string pDivision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proveedor_ConsultarDivision";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pDivision", pDivision);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarDivisionValidar(string pDivisionValidar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proveedor_ConsultarDivision";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pDivision", pDivisionValidar);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarRazonSocialProveedor(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proveedor_ConsultarRazonSocial";
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
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    public static string BuscarRazonSocialValidar(string pRazonSocialValidar)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_FacturaProveedor_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocialValidar);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@NombreColumna", "RazonSocialValidar");
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
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    public static string BuscarProyectoCliente(string pNombreProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CProyecto jsonProyecto = new CProyecto();
        jsonProyecto.StoredProcedure.CommandText = "sp_Proyecto_ConsultarNombreProyecto";
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@pNombreProyecto", pNombreProyecto);
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonProyecto.ObtenerJsonNombreProyecto(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarProducto(Dictionary<string, object> pProducto)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CProducto jsonProducto = new CProducto();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_ConsultarFiltros";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);

        string Busqueda = Convert.ToString(pProducto["TipoBusqueda"]);

        if (Busqueda == Convert.ToString("P"))
        {
            jsonProducto.StoredProcedure.Parameters.AddWithValue("@pProducto", Convert.ToString(pProducto["Producto"]));
        }
        else
        {
            jsonProducto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pProducto["Producto"]));
        }

        jsonProducto.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        return jsonProducto.ObtenerJsonProducto(ConexionBaseDatos);

    }

    [WebMethod]
    public static string BuscarServicio(Dictionary<string, object> pServicio)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CServicio jsonServicio = new CServicio();
        jsonServicio.StoredProcedure.CommandText = "sp_Servicio_ConsultarFiltros";
        jsonServicio.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);

        string Busqueda = Convert.ToString(pServicio["TipoBusqueda"]);

        if (Busqueda == Convert.ToString("P"))
        {
            jsonServicio.StoredProcedure.Parameters.AddWithValue("@pServicio", Convert.ToString(pServicio["Servicio"]));
        }
        else
        {
            jsonServicio.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pServicio["Servicio"]));
        }

        jsonServicio.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        return jsonServicio.ObtenerJsonServicio(ConexionBaseDatos);

    }

    [WebMethod]
    public static string ObtenerTotalesEstatusRecepcion()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Dictionary<Int32, Int32> TotalesEstatusRecepcion = EncabezadoFacturaProveedor.ObtenerTotalesEstatusRecepcion(Usuario.IdSucursalActual, ConexionBaseDatos);
            int TotalRecepciones = 0;
            JArray JTotalesEstatusRecepcion = new JArray();
            foreach (var Valor in TotalesEstatusRecepcion)
            {
                JObject JTotales = new JObject();
                JTotales.Add(new JProperty("IdEstatusRecepcion", Valor.Key));
                JTotales.Add(new JProperty("Contador", Valor.Value));
                JTotalesEstatusRecepcion.Add(JTotales);
                TotalRecepciones = TotalRecepciones + Valor.Value;
            }

            JObject JTotal = new JObject();
            JTotal.Add(new JProperty("IdEstatusRecepcion", 4));
            JTotal.Add(new JProperty("Contador", TotalRecepciones));
            JTotalesEstatusRecepcion.Add(JTotal);

            Modelo.Add("TotalesEstatusRecepcion", JTotalesEstatusRecepcion);
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
    public static string ExisteNumeroFactura(Dictionary<string, object> pEncabezadoFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
            string errores = "";

            if (EncabezadoFacturaProveedor.ExisteNumeroFactura(Convert.ToString(pEncabezadoFactura["NumeroFactura"]), Convert.ToInt32(pEncabezadoFactura["IdProveedor"]), ConexionBaseDatos) == 1)
            {
                errores = errores + "<span>*</span> Este número de factura " + Convert.ToString(pEncabezadoFactura["NumeroFactura"]) + " ya esta dada de alta para este proveedor.<br />";
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", errores));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 0));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string obtenerProducto(int IdProducto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CProducto.ObtenerJsonProducto(Modelo, IdProducto, ConexionBaseDatos);
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
    public static string obtenerServicio(int IdServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CServicio.ObtenerJsonServicio(Modelo, IdServicio, ConexionBaseDatos);
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
    public static string obtenerPedidosCliente(int IdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CCotizacion.ObtenerPedidosClienteRecepcion(Convert.ToInt32(IdCliente), ConexionBaseDatos));
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
    public static string ObtenerFormaFacturasPendientesPorValidar()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

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
    public static string obtenerOrdenCompraProveedor(int IdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", COrdenCompraEncabezado.ObtenerOrdenCompraProveedorRecepcion(Convert.ToInt32(IdProveedor), ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir un pedido...");

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

    //Validaciones
    private static string ValidarEncabezadoFacturaProveedor(CEncabezadoFacturaProveedor pEncabezadoFacturaProveedor, CConexion pConexion)
    {
        string errores = "";
        if (pEncabezadoFacturaProveedor.IdProveedor == 0)
        { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

        if (pEncabezadoFacturaProveedor.IdDivision == 0)
        { errores = errores + "<span>*</span> No hay división asociada, favor de elegir alguna.<br />"; }

        if (pEncabezadoFacturaProveedor.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; }

        if (pEncabezadoFacturaProveedor.FechaPago == null)
        { errores = errores + "<span>*</span> No se recibio la fecha de pago.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarDetalleFacturaProveedor(CDetalleFacturaProveedor pEncabezadoFacturaProveedor, CConexion pConexion)
    {
        string errores = "";


        if (pEncabezadoFacturaProveedor.Cantidad <= 0)
        { errores = errores + "<span>*</span> El campo cantidad debe de ser mayor a 0.<br />"; }

        //if (pEncabezadoFacturaProveedor.Total == 0)
        //{ errores = errores + "<span>*</span> El campo total esta vacío, favor de capturarlo.<br />"; }

        if (pEncabezadoFacturaProveedor.IdUnidadCompraVenta == 0)
        { errores = errores + "<span>*</span> No hay unidad de compra venta asociado, favor de elegir alguno.<br />"; }

        if (pEncabezadoFacturaProveedor.IdTipoCompra == 0)
        { errores = errores + "<span>*</span> No hay tipo de compra asociado, favor de elegir alguno.<br />"; }


        if (pEncabezadoFacturaProveedor.IdUsuarioSolicito == 0)
        { errores = errores + "<span>*</span> No hay usuario solicitante asociado, favor de elegir alguno.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
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
    public static string ObtenerListaTipoCompra()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonTipoCompra(true, ConexionBaseDatos));
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

}