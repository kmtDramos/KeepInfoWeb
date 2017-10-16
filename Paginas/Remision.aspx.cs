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

public partial class Remision : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    private static int idEmpresa;
    private static string logoEmpresa;

    public static int puedeAgregarEncabezadoRemision = 0;
    public static int puedeEditarEncabezadoRemision = 0;
    public static int puedeEliminarEncabezadoRemision = 0;
    public static int generaRemisionSinCliente = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        puedeAgregarEncabezadoRemision = Usuario.TienePermisos(new string[] { "puedeAgregarEncabezadoRemision" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarEncabezadoRemision = Usuario.TienePermisos(new string[] { "puedeEditarEncabezadoRemision" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarEncabezadoRemision = Usuario.TienePermisos(new string[] { "puedeEliminarEncabezadoRemision" }, ConexionBaseDatos) == "" ? 1 : 0;
        generaRemisionSinCliente = Usuario.TienePermisos(new string[] { "generaRemisionSinCliente" }, ConexionBaseDatos) == "" ? 1 : 0;

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;
        idSucursal = Sucursal.IdSucursal;
        idEmpresa = Empresa.IdEmpresa;
        logoEmpresa = Empresa.Logo;

        //GridRemision
        CJQGrid GridRemision = new CJQGrid();
        GridRemision.NombreTabla = "grdEncabezadoRemision";
        GridRemision.CampoIdentificador = "IdEncabezadoRemision";
        GridRemision.ColumnaOrdenacion = "FechaRemision";
        GridRemision.TipoOrdenacion = "Desc";
        GridRemision.Metodo = "ObtenerRemision";
        GridRemision.TituloTabla = "Remisiones";
        GridRemision.GenerarFuncionFiltro = false;

        //IdRemision
        CJQColumn ColIdRemision = new CJQColumn();
        ColIdRemision.Nombre = "IdEncabezadoRemision";
        ColIdRemision.Encabezado = "No. Remision";
        ColIdRemision.Ancho = "10";
        ColIdRemision.Oculto = "true";
        ColIdRemision.Buscador = "false";
        GridRemision.Columnas.Add(ColIdRemision);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "true";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridRemision.Columnas.Add(ColSucursal);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Ancho = "40";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        GridRemision.Columnas.Add(ColFolio);

        //Fecha
        CJQColumn ColFechaRemision = new CJQColumn();
        ColFechaRemision.Nombre = "FechaRemision";
        ColFechaRemision.Encabezado = "Fecha";
        ColFechaRemision.Ancho = "40";
        ColFechaRemision.Ordenable = "true";
        ColFechaRemision.Alineacion = "Left";
        ColFechaRemision.Buscador = "false";
        GridRemision.Columnas.Add(ColFechaRemision);

        //Razon social
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Ancho = "120";
        ColRazonSocial.Ordenable = "true";
        ColRazonSocial.Alineacion = "Left";
        ColRazonSocial.Buscador = "true";
        GridRemision.Columnas.Add(ColRazonSocial);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "50";
        ColTotal.Ordenable = "false";
        ColTotal.Buscador = "false";
        GridRemision.Columnas.Add(ColTotal);

        //Moneda
        CJQColumn ColMoneda = new CJQColumn();
        ColMoneda.Nombre = "TipoMoneda";
        ColMoneda.Encabezado = "Moneda";
        ColMoneda.Ancho = "40";
        ColMoneda.Ordenable = "false";
        ColMoneda.Alineacion = "Left";
        ColMoneda.Buscador = "false";
        GridRemision.Columnas.Add(ColMoneda);

        //Nota
        CJQColumn ColNota = new CJQColumn();
        ColNota.Nombre = "Nota";
        ColNota.Encabezado = "Nota";
        ColNota.Ancho = "120";
        ColNota.Ordenable = "false";
        ColNota.Alineacion = "Left";
        ColNota.Buscador = "false";
        GridRemision.Columnas.Add(ColNota);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "30";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.Oculto = puedeEliminarEncabezadoRemision == 1 ? "false" : "true";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridRemision.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarEncabezadoRemision";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridRemision.Columnas.Add(ColConsultar);


        ClientScript.RegisterStartupScript(this.GetType(), "grdEncabezadoRemision", GridRemision.GeneraGrid(), true);

        //Grid Detalle Producto            
        CJQGrid GridDetalleProducto = new CJQGrid();
        GridDetalleProducto.NombreTabla = "grdDetalleProducto";
        GridDetalleProducto.CampoIdentificador = "IdProducto";
        GridDetalleProducto.ColumnaOrdenacion = "IdProducto";
        GridDetalleProducto.Metodo = "ObtenerDetalleProducto";
        GridDetalleProducto.TituloTabla = "Detalle del Producto";
        GridDetalleProducto.GenerarGridCargaInicial = false;
        GridDetalleProducto.GenerarFuncionFiltro = false;
        GridDetalleProducto.GenerarFuncionTerminado = false;
        GridDetalleProducto.Altura = 150;
        GridDetalleProducto.Ancho = 870;
        GridDetalleProducto.NumeroRegistros = 15;
        GridDetalleProducto.RangoNumeroRegistros = "15,30,60";


        //Agregar A Detalle Remision
        CJQColumn ColAgregar = new CJQColumn();
        ColAgregar.Nombre = "Agregar";
        ColAgregar.Encabezado = "Elegir";
        ColAgregar.Etiquetado = "Imagen";
        ColAgregar.Ancho = "30";
        ColAgregar.Buscador = "false";
        ColAgregar.Ordenable = "false";
        ColAgregar.Imagen = "select.png";
        ColAgregar.Estilo = "divImagenConsultar imgRemisionarProducto";
        GridDetalleProducto.Columnas.Add(ColAgregar);

        //IdProducto
        CJQColumn ColIdProducto = new CJQColumn();
        ColIdProducto.Nombre = "IdProducto";
        ColIdProducto.Encabezado = "IdProducto";
        ColIdProducto.Oculto = "true";
        ColIdProducto.Buscador = "false";
        GridDetalleProducto.Columnas.Add(ColIdProducto);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Encabezado = "Clave";
        ColClave.Nombre = "Clave";
        ColClave.Ancho = "80";
        ColClave.Ordenable = "true";
        ColClave.Buscador = "true";
        ColSucursal.Oculto = "true";
        ColClave.Alineacion = "left";
        GridDetalleProducto.Columnas.Add(ColClave);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Ancho = "150";
        ColProducto.Alineacion = "Left";
        ColProducto.Ordenable = "false";
        ColProducto.Buscador = "false";
        GridDetalleProducto.Columnas.Add(ColProducto);

        //Existencia
        CJQColumn ColExistencia = new CJQColumn();
        ColExistencia.Nombre = "Existencia";
        ColExistencia.Encabezado = "Cantidad";
        ColExistencia.Ancho = "50";
        ColExistencia.Alineacion = "right";
        ColExistencia.Ordenable = "false";
        ColExistencia.Buscador = "false";
        GridDetalleProducto.Columnas.Add(ColExistencia);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Ancho = "150";
        ColDescripcion.Alineacion = "Left";
        ColDescripcion.Ordenable = "false";
        ColDescripcion.Buscador = "false";
        GridDetalleProducto.Columnas.Add(ColDescripcion);

        //No Serie
        CJQColumn ColNoSerie = new CJQColumn();
        ColNoSerie.Nombre = "NumeroSerie";
        ColNoSerie.Encabezado = "NumeroSerie";
        ColNoSerie.Ancho = "100";
        ColNoSerie.Alineacion = "Center";
        ColNoSerie.Ordenable = "false";
        ColNoSerie.Buscador = "true";
        GridDetalleProducto.Columnas.Add(ColNoSerie);

        //Costo
        CJQColumn ColCosto = new CJQColumn();
        ColCosto.Nombre = "Costo";
        ColCosto.Encabezado = "Costo";
        ColCosto.Ancho = "80";
        ColCosto.Alineacion = "right";
        ColCosto.Buscador = "false";
        ColCosto.Ordenable = "false";
        ColCosto.Formato = "FormatoMoneda";
        GridDetalleProducto.Columnas.Add(ColCosto);

        //Precio
        CJQColumn ColPrecio = new CJQColumn();
        ColPrecio.Nombre = "Precio";
        ColPrecio.Encabezado = "Precio";
        ColPrecio.Ancho = "80";
        ColPrecio.Alineacion = "right";
        ColPrecio.Ordenable = "false";
        ColPrecio.Buscador = "false";
        ColPrecio.Formato = "FormatoMoneda";
        GridDetalleProducto.Columnas.Add(ColPrecio);

        //No pedido
        CJQColumn ColNumeroPedido = new CJQColumn();
        ColNumeroPedido.Nombre = "NumeroPedido";
        ColNumeroPedido.Encabezado = "Pedido";
        ColNumeroPedido.Ancho = "50";
        ColNumeroPedido.Ordenable = "false";
        ColNumeroPedido.Alineacion = "right";
        ColNumeroPedido.Buscador = "true";
        GridDetalleProducto.Columnas.Add(ColNumeroPedido);

        //IdProveedor
        CJQColumn ColIdProveedor = new CJQColumn();
        ColIdProveedor.Nombre = "IdProveedor";
        ColIdProveedor.Encabezado = "IdProveedor";
        ColIdProveedor.Ancho = "5";
        ColIdProveedor.Alineacion = "Center";
        ColIdProveedor.Ordenable = "false";
        ColIdProveedor.Buscador = "false";
        ColIdProveedor.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdProveedor);

        //IdEncabezadoFacturaProveedor
        CJQColumn ColIdEncabezadoFacturaProveedor = new CJQColumn();
        ColIdEncabezadoFacturaProveedor.Nombre = "IdEncabezadoFacturaProveedor";
        ColIdEncabezadoFacturaProveedor.Encabezado = "IdEncabezadoFacturaProveedor";
        ColIdEncabezadoFacturaProveedor.Ancho = "5";
        ColIdEncabezadoFacturaProveedor.Alineacion = "Center";
        ColIdEncabezadoFacturaProveedor.Ordenable = "false";
        ColIdEncabezadoFacturaProveedor.Buscador = "false";
        ColIdEncabezadoFacturaProveedor.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdEncabezadoFacturaProveedor);

        //IdDetalleFacturaProveedor
        CJQColumn ColIdDetalleFacturaProveedor = new CJQColumn();
        ColIdDetalleFacturaProveedor.Nombre = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Encabezado = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Ancho = "5";
        ColIdDetalleFacturaProveedor.Alineacion = "Center";
        ColIdDetalleFacturaProveedor.Ordenable = "false";
        ColIdDetalleFacturaProveedor.Buscador = "false";
        ColIdDetalleFacturaProveedor.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdDetalleFacturaProveedor);

        //IdCotizacion
        CJQColumn ColIdCotizacion = new CJQColumn();
        ColIdCotizacion.Nombre = "IdCotizacion";
        ColIdCotizacion.Encabezado = "IdCotizacion";
        ColIdCotizacion.Ancho = "5";
        ColIdCotizacion.Alineacion = "Center";
        ColIdCotizacion.Ordenable = "false";
        ColIdCotizacion.Buscador = "false";
        ColIdCotizacion.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdCotizacion);

        //IdCotizacionDetalle
        CJQColumn ColIdCotizacionDetalle = new CJQColumn();
        ColIdCotizacionDetalle.Nombre = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Encabezado = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Ancho = "5";
        ColIdCotizacionDetalle.Alineacion = "Center";
        ColIdCotizacionDetalle.Ordenable = "false";
        ColIdCotizacionDetalle.Buscador = "false";
        ColIdCotizacionDetalle.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdCotizacionDetalle);

        //IdExistenciaDistribuida
        CJQColumn ColIdExistenciaDistribuida = new CJQColumn();
        ColIdExistenciaDistribuida.Nombre = "IdExistenciaDistribuida";
        ColIdExistenciaDistribuida.Encabezado = "IdExistenciaDistribuida";
        ColIdExistenciaDistribuida.Ancho = "5";
        ColIdExistenciaDistribuida.Alineacion = "Center";
        ColIdExistenciaDistribuida.Ordenable = "false";
        ColIdExistenciaDistribuida.Buscador = "false";
        ColIdExistenciaDistribuida.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdExistenciaDistribuida);

        //IdAlmacenProducto
        CJQColumn ColIdAlmacenProducto = new CJQColumn();
        ColIdAlmacenProducto.Nombre = "IdAlmacen";
        ColIdAlmacenProducto.Encabezado = "IdAlmacen";
        ColIdAlmacenProducto.Ancho = "5";
        ColIdAlmacenProducto.Alineacion = "Center";
        ColIdAlmacenProducto.Ordenable = "false";
        ColIdAlmacenProducto.Buscador = "false";
        ColIdAlmacenProducto.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdAlmacenProducto);

        //IdTipoMoneda
        CJQColumn ColIdTipoMoneda = new CJQColumn();
        ColIdTipoMoneda.Nombre = "IdTipoMoneda";
        ColIdTipoMoneda.Encabezado = "IdTipoMoneda";
        ColIdTipoMoneda.Ancho = "5";
        ColIdTipoMoneda.Alineacion = "Center";
        ColIdTipoMoneda.Ordenable = "false";
        ColIdTipoMoneda.Buscador = "false";
        ColIdTipoMoneda.Oculto = "true";
        GridDetalleProducto.Columnas.Add(ColIdTipoMoneda);

        //IdTipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Moneda";
        ColTipoMoneda.Ancho = "50";
        ColTipoMoneda.Alineacion = "Center";
        ColTipoMoneda.Ordenable = "false";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Oculto = "false";
        GridDetalleProducto.Columnas.Add(ColTipoMoneda);

        //Elegir
        CJQColumn ColElegir = new CJQColumn();
        ColElegir.Nombre = "Elegir";
        ColElegir.Encabezado = "Elegir";
        ColElegir.Buscador = "false";
        ColElegir.Alineacion = "left";
        ColElegir.Etiquetado = "CheckBoxchecked";
        ColElegir.Estilo = "chkElegir";
        ColElegir.Ancho = "40";
        GridDetalleProducto.Columnas.Add(ColElegir);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleProducto", GridDetalleProducto.GeneraGrid(), true);

        //Grid Detalle Remision 
        CJQGrid GridDetalleRemision = new CJQGrid();
        GridDetalleRemision.NombreTabla = "grdDetalleRemision";
        GridDetalleRemision.CampoIdentificador = "IdDetalleRemision";
        GridDetalleRemision.ColumnaOrdenacion = "IdDetalleRemision";
        GridDetalleRemision.Metodo = "ObtenerDetalleRemision";
        GridDetalleRemision.TituloTabla = "Detalle de Remisión";
        GridDetalleRemision.GenerarGridCargaInicial = false;
        GridDetalleRemision.GenerarFuncionFiltro = false;
        GridDetalleRemision.GenerarFuncionTerminado = false;
        GridDetalleRemision.Altura = 150;
        GridDetalleRemision.Ancho = 870;
        GridDetalleRemision.NumeroRegistros = 15;
        GridDetalleRemision.RangoNumeroRegistros = "15,30,60";

        //IdDetalleRemision
        CJQColumn ColIdDetalleRemision = new CJQColumn();
        ColIdDetalleRemision.Nombre = "IdDetalleRemision";
        ColIdDetalleRemision.Oculto = "true";
        ColIdDetalleRemision.Encabezado = "IdDetalleRemision";
        ColIdDetalleRemision.Buscador = "false";
        GridDetalleRemision.Columnas.Add(ColIdDetalleRemision);

        //Numero Remision
        CJQColumn ColNumeroRemision = new CJQColumn();
        ColNumeroRemision.Nombre = "NumeroRemision";
        ColNumeroRemision.Encabezado = "No Remisión";
        ColNumeroRemision.Ancho = "85";
        ColNumeroRemision.Buscador = "false";
        ColNumeroRemision.Alineacion = "Right";
        GridDetalleRemision.Columnas.Add(ColNumeroRemision);

        //Numero Producto
        CJQColumn ColIdProductoRemision = new CJQColumn();
        ColIdProductoRemision.Encabezado = "No Producto";
        ColIdProductoRemision.Nombre = "IdProducto";
        ColIdProductoRemision.Ancho = "10";
        ColIdProductoRemision.Ordenable = "false";
        ColIdProductoRemision.Buscador = "false";
        ColIdProductoRemision.Alineacion = "Center";
        ColIdProductoRemision.Oculto = "true";
        GridDetalleRemision.Columnas.Add(ColIdProductoRemision);

        //Descipcion Producto
        CJQColumn ColDescripcionRemision = new CJQColumn();
        ColDescripcionRemision.Encabezado = "Descripción";
        ColDescripcionRemision.Nombre = "Descripcion";
        ColDescripcionRemision.Ancho = "200";
        ColDescripcionRemision.Buscador = "false";
        ColDescripcionRemision.Alineacion = "Center";
        GridDetalleRemision.Columnas.Add(ColDescripcionRemision);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Ancho = "80";
        ColCantidad.Alineacion = "Right";
        ColCantidad.Buscador = "false";
        GridDetalleRemision.Columnas.Add(ColCantidad);

        //Precio Unitario
        CJQColumn ColPrecioU = new CJQColumn();
        ColPrecioU.Nombre = "PrecioUnitario";
        ColPrecioU.Encabezado = "Precio U";
        ColPrecioU.Formato = "FormatoMoneda";
        ColPrecioU.Ancho = "80";
        ColPrecioU.Alineacion = "right";
        ColPrecioU.Buscador = "false";
        GridDetalleRemision.Columnas.Add(ColPrecioU);

        //Monto
        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Formato = "FormatoMoneda";
        ColMonto.Ancho = "80";
        ColMonto.Alineacion = "right";
        ColMonto.Buscador = "false";
        GridDetalleRemision.Columnas.Add(ColMonto);

        //No Serie
        CJQColumn ColNoSerieRemision = new CJQColumn();
        ColNoSerieRemision.Nombre = "NumeroSerie";
        ColNoSerieRemision.Encabezado = "No Serie";
        ColNoSerieRemision.Ancho = "80";
        ColNoSerieRemision.Alineacion = "Center";
        ColNoSerieRemision.Buscador = "false";
        GridDetalleRemision.Columnas.Add(ColNoSerieRemision);

        //No pedido
        CJQColumn ColNoPedido = new CJQColumn();
        ColNoPedido.Nombre = "NumeroPedido";
        ColNoPedido.Encabezado = "No pedido";
        ColNoPedido.Ancho = "80";
        ColNoPedido.Alineacion = "right";
        ColNoPedido.Buscador = "false";
        GridDetalleRemision.Columnas.Add(ColNoPedido);

        //Eliminar concepto factura de proveedor consultar
        CJQColumn ColEliminarConcepto = new CJQColumn();
        ColEliminarConcepto.Nombre = "Eliminar";
        ColEliminarConcepto.Encabezado = "Eliminar";
        ColEliminarConcepto.Etiquetado = "Imagen";
        ColEliminarConcepto.Imagen = "eliminar.png";
        ColEliminarConcepto.Estilo = "divImagenConsultar imgEliminarConceptoEditar";
        ColEliminarConcepto.Buscador = "false";
        ColEliminarConcepto.Ordenable = "false";
        ColEliminarConcepto.Ancho = "60";
        GridDetalleRemision.Columnas.Add(ColEliminarConcepto);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleRemision", GridDetalleRemision.GeneraGrid(), true);

        //Grid Detalle RemisionConsultar 
        CJQGrid GridDetalleRemisionConsultar = new CJQGrid();
        GridDetalleRemisionConsultar.NombreTabla = "grdDetalleRemisionConsultar";
        GridDetalleRemisionConsultar.CampoIdentificador = "IdDetalleRemision";
        GridDetalleRemisionConsultar.ColumnaOrdenacion = "IdDetalleRemision";
        GridDetalleRemisionConsultar.Metodo = "ObtenerDetalleRemisionConsultar";
        GridDetalleRemisionConsultar.TituloTabla = "Detalle de Remisión";
        GridDetalleRemisionConsultar.GenerarGridCargaInicial = false;
        GridDetalleRemisionConsultar.GenerarFuncionFiltro = false;
        GridDetalleRemisionConsultar.GenerarFuncionTerminado = false;
        GridDetalleRemisionConsultar.Altura = 200;
        GridDetalleRemisionConsultar.Ancho = 870;
        GridDetalleRemisionConsultar.NumeroRegistros = 15;
        GridDetalleRemisionConsultar.RangoNumeroRegistros = "15,30,60";

        //IdDetalleRemision
        CJQColumn ColIdDetalleRemisionConsultar = new CJQColumn();
        ColIdDetalleRemisionConsultar.Nombre = "IdDetalleRemision";
        ColIdDetalleRemisionConsultar.Oculto = "true";
        ColIdDetalleRemisionConsultar.Encabezado = "IdDetalleRemision";
        ColIdDetalleRemisionConsultar.Buscador = "false";
        GridDetalleRemisionConsultar.Columnas.Add(ColIdDetalleRemisionConsultar);

        //Numero RemisionConsultar
        CJQColumn ColNumeroRemisionConsultar = new CJQColumn();
        ColNumeroRemisionConsultar.Nombre = "NumeroRemision";
        ColNumeroRemisionConsultar.Encabezado = "No Remisión";
        ColNumeroRemisionConsultar.Ancho = "85";
        ColNumeroRemisionConsultar.Buscador = "false";
        ColNumeroRemisionConsultar.Alineacion = "Right";
        GridDetalleRemisionConsultar.Columnas.Add(ColNumeroRemisionConsultar);

        //Numero Producto
        CJQColumn ColIdProductoRemisionConsultar = new CJQColumn();
        ColIdProductoRemisionConsultar.Encabezado = "No Producto";
        ColIdProductoRemisionConsultar.Nombre = "IdProducto";
        ColIdProductoRemisionConsultar.Ancho = "85";
        ColIdProductoRemisionConsultar.Ordenable = "false";
        ColIdProductoRemisionConsultar.Buscador = "false";
        ColIdProductoRemisionConsultar.Alineacion = "Center";
        ColIdProductoRemisionConsultar.Oculto = "true";
        GridDetalleRemisionConsultar.Columnas.Add(ColIdProductoRemisionConsultar);

        //Descipcion Producto
        CJQColumn ColDescripcionRemisionConsultar = new CJQColumn();
        ColDescripcionRemisionConsultar.Encabezado = "Descripción";
        ColDescripcionRemisionConsultar.Nombre = "Descripcion";
        ColDescripcionRemisionConsultar.Ancho = "200";
        ColDescripcionRemisionConsultar.Buscador = "false";
        ColDescripcionRemisionConsultar.Alineacion = "Center";
        GridDetalleRemisionConsultar.Columnas.Add(ColDescripcionRemisionConsultar);

        //Cantidad
        CJQColumn ColCantidadConsultar = new CJQColumn();
        ColCantidadConsultar.Nombre = "Cantidad";
        ColCantidadConsultar.Encabezado = "Cantidad";
        ColCantidadConsultar.Ancho = "80";
        ColCantidadConsultar.Alineacion = "Right";
        ColCantidadConsultar.Buscador = "false";
        GridDetalleRemisionConsultar.Columnas.Add(ColCantidadConsultar);

        //Precio Unitario
        CJQColumn ColPrecioUConsultar = new CJQColumn();
        ColPrecioUConsultar.Nombre = "PrecioUnitario";
        ColPrecioUConsultar.Encabezado = "Precio U";
        ColPrecioUConsultar.Formato = "FormatoMoneda";
        ColPrecioUConsultar.Ancho = "80";
        ColPrecioUConsultar.Alineacion = "right";
        ColPrecioUConsultar.Buscador = "false";
        GridDetalleRemisionConsultar.Columnas.Add(ColPrecioUConsultar);

        //Monto
        CJQColumn ColMontoConsultar = new CJQColumn();
        ColMontoConsultar.Nombre = "Monto";
        ColMontoConsultar.Encabezado = "Monto";
        ColMontoConsultar.Formato = "FormatoMoneda";
        ColMontoConsultar.Ancho = "80";
        ColMontoConsultar.Alineacion = "right";
        ColMontoConsultar.Buscador = "false";
        GridDetalleRemisionConsultar.Columnas.Add(ColMontoConsultar);

        //No Serie
        CJQColumn ColNoSerieRemisionConsultar = new CJQColumn();
        ColNoSerieRemisionConsultar.Nombre = "NumeroSerie";
        ColNoSerieRemisionConsultar.Encabezado = "No Serie";
        ColNoSerieRemisionConsultar.Ancho = "80";
        ColNoSerieRemisionConsultar.Alineacion = "Center";
        ColNoSerieRemisionConsultar.Buscador = "false";
        GridDetalleRemisionConsultar.Columnas.Add(ColNoSerieRemisionConsultar);

        //No pedido
        CJQColumn ColNoPedidoConsultar = new CJQColumn();
        ColNoPedidoConsultar.Nombre = "NumeroPedido";
        ColNoPedidoConsultar.Encabezado = "No pedido";
        ColNoPedidoConsultar.Ancho = "80";
        ColNoPedidoConsultar.Alineacion = "right";
        ColNoPedidoConsultar.Buscador = "false";
        GridDetalleRemisionConsultar.Columnas.Add(ColNoPedidoConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleRemisionConsultar", GridDetalleRemisionConsultar.GeneraGrid(), true);

        //Grid Detalle RemisionEditar 
        CJQGrid GridDetalleRemisionEditar = new CJQGrid();
        GridDetalleRemisionEditar.NombreTabla = "grdDetalleRemisionEditar";
        GridDetalleRemisionEditar.CampoIdentificador = "IdDetalleRemision";
        GridDetalleRemisionEditar.ColumnaOrdenacion = "IdDetalleRemision";
        GridDetalleRemisionEditar.Metodo = "ObtenerDetalleRemisionEditar";
        GridDetalleRemisionEditar.TituloTabla = "Detalle de Remisión";
        GridDetalleRemisionEditar.GenerarGridCargaInicial = false;
        GridDetalleRemisionEditar.GenerarFuncionFiltro = false;
        GridDetalleRemisionEditar.GenerarFuncionTerminado = false;
        GridDetalleRemisionEditar.Altura = 150;
        GridDetalleRemisionEditar.Ancho = 870;
        GridDetalleRemisionEditar.NumeroRegistros = 15;
        GridDetalleRemisionEditar.RangoNumeroRegistros = "15,30,60";

        //IdDetalleRemision
        CJQColumn ColIdDetalleRemisionEditar = new CJQColumn();
        ColIdDetalleRemisionEditar.Nombre = "IdDetalleRemision";
        ColIdDetalleRemisionEditar.Oculto = "true";
        ColIdDetalleRemisionEditar.Encabezado = "IdDetalleRemision";
        ColIdDetalleRemisionEditar.Buscador = "false";
        GridDetalleRemisionEditar.Columnas.Add(ColIdDetalleRemisionEditar);

        //Numero RemisionEditar
        CJQColumn ColNumeroRemisionEditar = new CJQColumn();
        ColNumeroRemisionEditar.Nombre = "NumeroRemision";
        ColNumeroRemisionEditar.Encabezado = "No Remisión";
        ColNumeroRemisionEditar.Ancho = "85";
        ColNumeroRemisionEditar.Buscador = "false";
        ColNumeroRemisionEditar.Alineacion = "Right";
        GridDetalleRemisionEditar.Columnas.Add(ColNumeroRemisionEditar);

        //Numero Producto
        CJQColumn ColIdProductoRemisionEditar = new CJQColumn();
        ColIdProductoRemisionEditar.Encabezado = "No Producto";
        ColIdProductoRemisionEditar.Nombre = "IdProducto";
        ColIdProductoRemisionEditar.Ancho = "85";
        ColIdProductoRemisionEditar.Ordenable = "false";
        ColIdProductoRemisionEditar.Buscador = "false";
        ColIdProductoRemisionEditar.Alineacion = "Center";
        ColIdProductoRemisionEditar.Oculto = "true";
        GridDetalleRemisionEditar.Columnas.Add(ColIdProductoRemisionEditar);

        //Descipcion Producto
        CJQColumn ColDescripcionRemisionEditar = new CJQColumn();
        ColDescripcionRemisionEditar.Encabezado = "Descripción";
        ColDescripcionRemisionEditar.Nombre = "Descripcion";
        ColDescripcionRemisionEditar.Ancho = "200";
        ColDescripcionRemisionEditar.Buscador = "false";
        ColDescripcionRemisionEditar.Alineacion = "Center";
        GridDetalleRemisionEditar.Columnas.Add(ColDescripcionRemisionEditar);

        //Cantidad
        CJQColumn ColCantidadEditar = new CJQColumn();
        ColCantidadEditar.Nombre = "Cantidad";
        ColCantidadEditar.Encabezado = "Cantidad";
        ColCantidadEditar.Ancho = "80";
        ColCantidadEditar.Alineacion = "Right";
        ColCantidadEditar.Buscador = "false";
        GridDetalleRemisionEditar.Columnas.Add(ColCantidadEditar);

        //Precio Unitario
        CJQColumn ColPrecioUEditar = new CJQColumn();
        ColPrecioUEditar.Nombre = "PrecioUnitario";
        ColPrecioUEditar.Encabezado = "Precio U";
        ColPrecioUEditar.Formato = "FormatoMoneda";
        ColPrecioUEditar.Ancho = "80";
        ColPrecioUEditar.Alineacion = "right";
        ColPrecioUEditar.Buscador = "false";
        GridDetalleRemisionEditar.Columnas.Add(ColPrecioUEditar);

        //Monto
        CJQColumn ColMontoEditar = new CJQColumn();
        ColMontoEditar.Nombre = "Monto";
        ColMontoEditar.Encabezado = "Monto";
        ColMontoEditar.Formato = "FormatoMoneda";
        ColMontoEditar.Ancho = "80";
        ColMontoEditar.Alineacion = "right";
        ColMontoEditar.Buscador = "false";
        GridDetalleRemisionEditar.Columnas.Add(ColMontoEditar);

        //No Serie
        CJQColumn ColNoSerieRemisionEditar = new CJQColumn();
        ColNoSerieRemisionEditar.Nombre = "NumeroSerie";
        ColNoSerieRemisionEditar.Encabezado = "No Serie";
        ColNoSerieRemisionEditar.Ancho = "80";
        ColNoSerieRemisionEditar.Alineacion = "Center";
        ColNoSerieRemisionEditar.Buscador = "false";
        GridDetalleRemisionEditar.Columnas.Add(ColNoSerieRemisionEditar);

        //No pedido
        CJQColumn ColNoPedidoEditar = new CJQColumn();
        ColNoPedidoEditar.Nombre = "NumeroPedido";
        ColNoPedidoEditar.Encabezado = "No pedido";
        ColNoPedidoEditar.Ancho = "80";
        ColNoPedidoEditar.Alineacion = "right";
        ColNoPedidoEditar.Buscador = "false";
        GridDetalleRemisionEditar.Columnas.Add(ColNoPedidoEditar);

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
        GridDetalleRemisionEditar.Columnas.Add(ColEliminarConceptoEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleRemisionEditar", GridDetalleRemisionEditar.GeneraGrid(), true);


        //GridDetallePedido
        CJQGrid GridDetallePedido = new CJQGrid();
        GridDetallePedido.NombreTabla = "grdDetallePedido";
        GridDetallePedido.CampoIdentificador = "IdCotizacionDetalle";
        GridDetallePedido.ColumnaOrdenacion = "Clave";
        GridDetallePedido.TipoOrdenacion = "DESC";
        GridDetallePedido.Metodo = "ObtenerDetallePedido";
        GridDetallePedido.TituloTabla = "Detalle pedido pendientes de recepcionar";
        GridDetallePedido.GenerarFuncionFiltro = false;
        GridDetallePedido.GenerarFuncionTerminado = false;
        GridDetallePedido.Altura = 200;
        GridDetallePedido.Ancho = 800;
        GridDetallePedido.NumeroRegistros = 15;
        GridDetallePedido.RangoNumeroRegistros = "15,30,60";

        //IdCotizacionDetalle
        CJQColumn ColIdCotizacionDetalleP = new CJQColumn();
        ColIdCotizacionDetalleP.Nombre = "IdCotizacionDetalle";
        ColIdCotizacionDetalleP.Oculto = "true";
        ColIdCotizacionDetalleP.Encabezado = "IdCotizacionDetalle";
        ColIdCotizacionDetalleP.Buscador = "false";
        GridDetallePedido.Columnas.Add(ColIdCotizacionDetalleP);

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
        ColDescripcionProductoPedido.Encabezado = "Descripcion";
        ColDescripcionProductoPedido.Alineacion = "left";
        ColDescripcionProductoPedido.Buscador = "false";
        ColDescripcionProductoPedido.Ancho = "80";
        GridDetallePedido.Columnas.Add(ColDescripcionProductoPedido);

        //CantidadProductoPedido
        CJQColumn ColCantidadProductoPedido = new CJQColumn();
        ColCantidadProductoPedido.Nombre = "Cantidad";
        ColCantidadProductoPedido.Encabezado = "Cantidad";
        ColCantidadProductoPedido.Buscador = "false";
        ColCantidadProductoPedido.Alineacion = "right";
        ColCantidadProductoPedido.Ancho = "20";
        GridDetallePedido.Columnas.Add(ColCantidadProductoPedido);

        //SaldoProductoPedido
        CJQColumn ColSaldoProductoPedido = new CJQColumn();
        ColSaldoProductoPedido.Nombre = "Saldo";
        ColSaldoProductoPedido.Encabezado = "Pendientes de recepcionar";
        ColSaldoProductoPedido.Buscador = "false";
        ColSaldoProductoPedido.Alineacion = "left";
        ColSaldoProductoPedido.Ancho = "50";
        GridDetallePedido.Columnas.Add(ColSaldoProductoPedido);

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

        //SeleccionarDetallePedido
        CJQColumn ColSeleccionarDetallePedido = new CJQColumn();
        ColSeleccionarDetallePedido.Nombre = "saldoImagen";
        ColSeleccionarDetallePedido.Encabezado = "Seleccionar";
        ColSeleccionarDetallePedido.Etiquetado = "Imagen";
        ColSeleccionarDetallePedido.Imagen = "select.png";
        ColSeleccionarDetallePedido.Buscador = "false";
        ColSeleccionarDetallePedido.Ordenable = "false";
        ColSeleccionarDetallePedido.Estilo = "divImagenConsultar imgSeleccionarDetallePedido";
        ColSeleccionarDetallePedido.Ancho = "25";
        GridDetallePedido.Columnas.Add(ColSeleccionarDetallePedido);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetallePedido", GridDetallePedido.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerRemision(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, int pIdEstatusRemision, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pNumeroSerie, string pNumeroPedido)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEncabezadoRemision", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdEstatusRemision", SqlDbType.Int).Value = pIdEstatusRemision;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pNumeroSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroSerie);
        Stored.Parameters.Add("pNumeroPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroPedido);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleProducto(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdProducto, int pIdPedidoDetalle, string pNumeroSerie, int pIdProyecto, int pIdAlmacen, string pFolioPedido, string pClave)
    {
        CConexion conexion = new CConexion();
        string respuesta = conexion.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), conexion);
        int IdSucursalActual = Usuario.IdSucursalActual;
        conexion.CerrarBaseDatosSqlServer();

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleProducto", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdProducto);
        Stored.Parameters.Add("pIdSucursalActual", SqlDbType.VarChar, 250).Value = Convert.ToString(IdSucursalActual);
        Stored.Parameters.Add("pIdPedidoDetalle", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdPedidoDetalle);
        Stored.Parameters.Add("pNumeroSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroSerie);
        Stored.Parameters.Add("pFolioPedido", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolioPedido);
        Stored.Parameters.Add("pClave", SqlDbType.VarChar, 250).Value = Convert.ToString(pClave);
        Stored.Parameters.Add("pIdProyecto", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdProyecto);
        Stored.Parameters.Add("pIdAlmacen", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdAlmacen);
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleRemision(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEncabezadoRemision)
    {
        CConexion conexion = new CConexion();
        string respuesta = conexion.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), conexion);
        int IdSucursalActual = Usuario.IdSucursalActual;
        conexion.CerrarBaseDatosSqlServer();

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleRemision", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEncabezadoRemision", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdEncabezadoRemision);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleRemisionConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEncabezadoRemision)
    {
        CConexion conexion = new CConexion();
        string respuesta = conexion.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), conexion);
        int IdSucursalActual = Usuario.IdSucursalActual;
        conexion.CerrarBaseDatosSqlServer();

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleRemision", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEncabezadoRemision", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdEncabezadoRemision);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleRemisionEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEncabezadoRemision)
    {
        CConexion conexion = new CConexion();
        string respuesta = conexion.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), conexion);
        int IdSucursalActual = Usuario.IdSucursalActual;
        conexion.CerrarBaseDatosSqlServer();

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleRemision", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEncabezadoRemision", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdEncabezadoRemision);

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
        SqlCommand Stored = new SqlCommand("spg_grdDetallePedidoConsultaRemision", sqlCon);
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
    public static string BuscarNombreComercial(string pNombreComercial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoCliente = new CJson();
        jsonTipoCliente.StoredProcedure.CommandText = "sp_Remision_ConsultarClienteFiltro";
        jsonTipoCliente.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pNombreComercial);
        jsonTipoCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonTipoCliente.ObtenerJsonString(ConexionBaseDatos);
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
    public static string AgregarDetalleRemision(string pIdProducto, int pIdTipoMonedaOrigen, int pIdPedidoDetalle)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CJson jsonDetalleProducto = new CJson();
        CJson jsonTipoMonedaOrigen = new CJson();
        CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
        CotizacionDetalle.LlenaObjeto(pIdPedidoDetalle, ConexionBaseDatos);

        jsonTipoMonedaOrigen.StoredProcedure.CommandText = "sp_Remision_ConsultarTipoMoneda";
        jsonTipoMonedaOrigen.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", pIdTipoMonedaOrigen);
        string jsonTMoneda = jsonTipoMonedaOrigen.ObtenerJsonString(ConexionBaseDatos);
        var tMoneda = JObject.Parse(jsonTMoneda);
        string monedaSimbolo = (string)tMoneda["Table"][0]["Simbolo"];
        oRespuesta.Add(new JProperty("SimboloOrigen", monedaSimbolo));

        jsonDetalleProducto.StoredProcedure.CommandText = "sp_Remision_ConsultarDetalleProducto";
        jsonDetalleProducto.StoredProcedure.Parameters.AddWithValue("@pIdProducto", pIdProducto);

        string json = jsonDetalleProducto.ObtenerJsonString(ConexionBaseDatos);
        oRespuesta.Add(new JProperty("jsonDp", json));

        var registros = JObject.Parse(json);
        int pIdTipoMonedaDestino = (int)registros["Table"][0]["IdTipoMoneda"];

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(pIdTipoMonedaOrigen));
        Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(pIdTipoMonedaDestino));
        Parametros.Add("Fecha", DateTime.Today);


        CTipoCambio TipoCambio = new CTipoCambio();
        TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
        oRespuesta.Add(new JProperty("PrecioDetallePedido", Convert.ToInt32(CotizacionDetalle.PrecioUnitario)));
        oRespuesta.Add(new JProperty("TipoCambioActual", TipoCambio.TipoCambio));
        oRespuesta.Add(new JProperty("Cantidad", 1));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string BuscarNombreProyecto(string pNombreComercial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoProyecto = new CJson();
        jsonTipoProyecto.StoredProcedure.CommandText = "sp_Proyecto_ConsultarNombreProyecto";
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@pNombreProyecto", pNombreComercial);
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonTipoProyecto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarClave(string pClave)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoProyecto = new CJson();
        jsonTipoProyecto.StoredProcedure.CommandText = "sp_Producto_ConsultarFiltros";
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pClave));
        return jsonTipoProyecto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarNumeroSerie(string pNumeroSerie)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoProyecto = new CJson();
        jsonTipoProyecto.StoredProcedure.CommandText = "sp_Remision_ConsultarProductoNumeroSerie";
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@pNumeroSerie", Convert.ToString(pNumeroSerie));
        return jsonTipoProyecto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarRemision(int pIdAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
        CTipoCambio TipoCambio = new CTipoCambio();
        DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());

        if (respuesta == "Conexion Establecida")
        {
            string validacion = ValidarExisteTipoCambio(TipoCambio, Sucursal, Fecha, ConexionBaseDatos);
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(Sucursal.IdTipoMoneda, ConexionBaseDatos));
                Modelo.Add(new JProperty("FechaActual", Convert.ToDateTime(DateTime.Now).ToShortDateString()));

                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(1));
                Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
                Parametros.Add("Fecha", DateTime.Today);
                TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
                Modelo.Add("TipoCambioRemision", TipoCambio.TipoCambio);
                Modelo.Add("IdEncabezadoRemision", 0);

                Modelo.Add(new JProperty("IdAlmacen", Convert.ToInt32(pIdAlmacen)));

                CAlmacen Almacen = new CAlmacen();
                Almacen.LlenaObjeto(Convert.ToInt32(pIdAlmacen), ConexionBaseDatos);
                Modelo.Add(new JProperty("Almacen", Almacen.Almacen));

                oRespuesta.Add(new JProperty("Modelo", Modelo));
                oRespuesta.Add(new JProperty("Error", 0));
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
                    Modelo.Add("TipoCambioRemision", TipoCambio.TipoCambio);
                }
                else
                {
                    Modelo.Add("TipoCambioRemision", 1);
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
        jsonAlmacenAsignado.StoredProcedure.CommandText = "sp_Remision_ConsultarAlmacenAsignado";
        jsonAlmacenAsignado.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", IdSucursal);
        almacen.Add("Opciones", jsonAlmacenAsignado.ObtenerJsonJObject(ConexionBaseDatos));

        oRespuesta.Add(new JProperty("Modelo", almacen));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string ObtenerFormaEncabezadoRemision(int pIdEncabezadoRemision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEncabezadoRemision = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEncabezadoRemision" }, ConexionBaseDatos) == "")
        {
            puedeEditarEncabezadoRemision = 1;
        }
        oPermisos.Add("puedeEditarEncabezadoRemision", puedeEditarEncabezadoRemision);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEncabezadoRemision.ObtenerEncabezadoRemision(Modelo, pIdEncabezadoRemision, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarEncabezadoRemision(int IdEncabezadoRemision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEncabezadoRemision = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEncabezadoRemision" }, ConexionBaseDatos) == "")
        {
            puedeEditarEncabezadoRemision = 1;
        }
        oPermisos.Add("puedeEditarEncabezadoRemision", puedeEditarEncabezadoRemision);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEncabezadoRemision.ObtenerEncabezadoRemision(Modelo, IdEncabezadoRemision, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("Cotizaciones", CJson.ObtenerJsonCotizacion(Convert.ToInt32(Modelo["IdCotizacion"].ToString()), ConexionBaseDatos));
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
    public static string obtenerPedidosCliente(int IdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CCotizacion.ObtenerPedidosClienteRemision(Convert.ToInt32(IdCliente), ConexionBaseDatos));
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

    [WebMethod]
    public static string ObtenerPrecioPorMoneda(Dictionary<string, object> pDato)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            if (Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()) != 0 && Convert.ToInt32(pDato["IdTipoMonedaDestino"].ToString()) != 0)
            {
                CTipoCambio TipoCambio = new CTipoCambio();
                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()));
                Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(pDato["IdTipoMonedaDestino"].ToString()));
                Parametros.Add("Fecha", DateTime.Today);
                TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
                Modelo.Add("MonedaPrecio", Convert.ToDecimal(pDato["Precio"].ToString()) / Convert.ToDecimal(TipoCambio.TipoCambio));
                Modelo.Add("TipoCambioActual", TipoCambio.TipoCambio);
            }
            else
            {
                Modelo.Add("MonedaPrecio", Convert.ToDecimal(0));

            }

            JObject TipoMoneda = new JObject();
            TipoMoneda.Add("Opciones", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()), ConexionBaseDatos));
            TipoMoneda.Add("ValorDefault", "0");
            TipoMoneda.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("ListaTipoMoneda", TipoMoneda);

            oRespuesta.Add(new JProperty("Error", 0));
            Modelo.Add(new JProperty("Permisos", oPermisos));
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
    public static string ObtenerFormaFiltroEncabezadoRemision()
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
    public static string ObtenerTotalesEstatusRemision()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Dictionary<Int32, Int32> TotalesEstatusRemision = EncabezadoRemision.ObtenerTotalesEstatusRemision(Usuario.IdSucursalActual, ConexionBaseDatos);
            int TotalRemisiones = 0;
            JArray JTotalesEstatusRemision = new JArray();
            foreach (var Valor in TotalesEstatusRemision)
            {
                JObject JTotales = new JObject();
                JTotales.Add(new JProperty("IdEstatusRemision", Valor.Key));
                JTotales.Add(new JProperty("Contador", Valor.Value));
                JTotalesEstatusRemision.Add(JTotales);
                TotalRemisiones = TotalRemisiones + Valor.Value;
            }

            JObject JTotal = new JObject();
            JTotal.Add(new JProperty("IdEstatusRemision", 4));
            JTotal.Add(new JProperty("Contador", TotalRemisiones));
            JTotalesEstatusRemision.Add(JTotal);

            Modelo.Add("TotalesEstatusRemision", JTotalesEstatusRemision);
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
    public static string AgregarProductoDetalleRemisionEncabezado(Dictionary<string, object> pRegistros)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string TotalLetras = "";
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        JObject Modelo = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
            CDetalleRemision DetalleRemision = new CDetalleRemision();
            CProducto Producto = new CProducto();
            Producto.LlenaObjeto(Convert.ToInt32(pRegistros["pIdProducto"]), ConexionBaseDatos);

            EncabezadoRemision.FechaRemision = Convert.ToDateTime(pRegistros["pFechaRemision"]);
            EncabezadoRemision.Nota = Convert.ToString(pRegistros["pNota"]);
            EncabezadoRemision.IdCliente = Convert.ToInt32(pRegistros["pIdCliente"]);
            EncabezadoRemision.IdTipoMoneda = Convert.ToInt32(pRegistros["pIdTipoMoneda"]);
            EncabezadoRemision.TipoCambio = Convert.ToDecimal(pRegistros["pTipoCambio"]);
            EncabezadoRemision.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            EncabezadoRemision.Baja = false;
            EncabezadoRemision.Consolidado = false;
            EncabezadoRemision.IdAlmacen = Convert.ToInt32(pRegistros["pIdAlmacen"]);

            EncabezadoRemision.AgregarEncabezadoRemision(ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CEncabezadoRemisionSucursal EncabezadoRemisionSucursal = new CEncabezadoRemisionSucursal();
            EncabezadoRemisionSucursal.IdEncabezadoRemision = EncabezadoRemision.IdEncabezadoRemision;
            EncabezadoRemisionSucursal.IdSucursal = Usuario.IdSucursalActual;
            EncabezadoRemisionSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
            EncabezadoRemisionSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            EncabezadoRemisionSucursal.Agregar(ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = EncabezadoRemision.IdEncabezadoRemision;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se inserto una nueva remisión";
            HistorialGenerico.AgregarHistorialGenerico("EncabezadoRemision", ConexionBaseDatos);

            CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();

            if (Convert.ToInt32(pRegistros["pTodos"]) == 0)
            {
                DetalleFacturaProveedor.LlenaObjeto(Convert.ToInt32(pRegistros["pIdDetFacProveedor"]), ConexionBaseDatos);
                CotizacionDetalle.LlenaObjeto(Convert.ToInt32(pRegistros["pIdDetPedido"]), ConexionBaseDatos);
                DetalleRemision.IdEncabezadoRemision = EncabezadoRemision.IdEncabezadoRemision;
                DetalleRemision.Cantidad = Convert.ToInt32(pRegistros["pCantidad"]);
                DetalleRemision.PrecioUnitario = Convert.ToDecimal(pRegistros["pPrecioConTipoCambio"]);
                DetalleRemision.Monto = (Convert.ToInt32(pRegistros["pCantidad"]) * Convert.ToDecimal(pRegistros["pPrecioConTipoCambio"]));
                DetalleRemision.IdProducto = Convert.ToInt32(pRegistros["pIdProducto"]);
                DetalleRemision.IdEncabezadoFacturaProveedor = Convert.ToInt32(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor);
                DetalleRemision.IdDetalleFacturaProveedor = Convert.ToInt32(pRegistros["pIdDetFacProveedor"]);
                DetalleRemision.IdEncabezadoPedido = Convert.ToInt32(CotizacionDetalle.IdCotizacion);
                DetalleRemision.IdDetallePedido = Convert.ToInt32(pRegistros["pIdDetPedido"]);
                DetalleRemision.IdProyecto = Convert.ToInt32(pRegistros["pIdProyecto"]);
                DetalleRemision.IdAlmacen = Convert.ToInt32(pRegistros["pIdAlmacen"]);
                DetalleRemision.FechaAlta = Convert.ToDateTime(DateTime.Now);
                DetalleRemision.Baja = false;
                DetalleRemision.AgregarDetalleRemision(ConexionBaseDatos);

                CExistenciaDistribuida ExistenciaDistribuida = new CExistenciaDistribuida();
                ExistenciaDistribuida.LlenaObjeto(Convert.ToInt32(pRegistros["pIdExistenciaDistribuida"]), ConexionBaseDatos);
                ExistenciaDistribuida.Saldo = ExistenciaDistribuida.Saldo - Convert.ToInt32(pRegistros["pCantidad"]);
                ExistenciaDistribuida.Editar(ConexionBaseDatos);

                HistorialGenerico.IdGenerico = DetalleRemision.IdDetalleRemision;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nueva partida de remisión";
                HistorialGenerico.AgregarHistorialGenerico("DetalleRemision", ConexionBaseDatos);
            }
            else
            {
                foreach (Dictionary<string, object> oPartidas in (Array)pRegistros["DetallePartidas"])
                {

                    CTipoCambio TipoCambio = new CTipoCambio();
                    Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
                    ParametrosTS.Add("Opcion", 1);
                    ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(oPartidas["IdTipoCambioOrigen"]));
                    ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(oPartidas["IdTipoCambioDestino"]));
                    ParametrosTS.Add("Fecha", DateTime.Today);
                    TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, ConexionBaseDatos);


                    DetalleFacturaProveedor.LlenaObjeto(Convert.ToInt32(oPartidas["IdDetalleFacturaProveedor"]), ConexionBaseDatos);
                    CotizacionDetalle.LlenaObjeto(Convert.ToInt32(oPartidas["IdPedidoDetalle"]), ConexionBaseDatos);
                    DetalleRemision.IdEncabezadoRemision = EncabezadoRemision.IdEncabezadoRemision;
                    DetalleRemision.Cantidad = Convert.ToInt32(oPartidas["Cantidad"]);

                    DetalleRemision.PrecioUnitario = Convert.ToDecimal(oPartidas["Precio"]) / TipoCambio.TipoCambio;
                    DetalleRemision.Monto = (Convert.ToInt32(oPartidas["Cantidad"]) * Convert.ToDecimal(DetalleRemision.PrecioUnitario));

                    DetalleRemision.IdProducto = Convert.ToInt32(oPartidas["IdProducto"]);
                    DetalleRemision.IdEncabezadoFacturaProveedor = Convert.ToInt32(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor);
                    DetalleRemision.IdDetalleFacturaProveedor = Convert.ToInt32(oPartidas["IdDetalleFacturaProveedor"]);
                    DetalleRemision.IdEncabezadoPedido = Convert.ToInt32(CotizacionDetalle.IdCotizacion);
                    DetalleRemision.IdDetallePedido = Convert.ToInt32(oPartidas["IdPedidoDetalle"]);
                    DetalleRemision.IdProyecto = Convert.ToInt32(pRegistros["pIdProyecto"]);
                    DetalleRemision.IdAlmacen = Convert.ToInt32(pRegistros["pIdAlmacen"]);
                    DetalleRemision.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    DetalleRemision.Baja = false;
                    DetalleRemision.AgregarDetalleRemision(ConexionBaseDatos);

                    CExistenciaDistribuida ExistenciaDistribuida = new CExistenciaDistribuida();
                    ExistenciaDistribuida.LlenaObjeto(Convert.ToInt32(pRegistros["pIdExistenciaDistribuida"]), ConexionBaseDatos);
                    ExistenciaDistribuida.Saldo = ExistenciaDistribuida.Saldo - Convert.ToInt32(pRegistros["pCantidad"]);
                    ExistenciaDistribuida.Editar(ConexionBaseDatos);

                    HistorialGenerico.IdGenerico = DetalleRemision.IdDetalleRemision;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva partida de remisión";
                    HistorialGenerico.AgregarHistorialGenerico("DetalleRemision", ConexionBaseDatos);
                }
            }
            EncabezadoRemision.LlenaObjeto(EncabezadoRemision.IdEncabezadoRemision, ConexionBaseDatos);

            CUtilerias Utilerias = new CUtilerias();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(EncabezadoRemision.IdTipoMoneda, ConexionBaseDatos);
            TotalLetras = Utilerias.ConvertLetter(EncabezadoRemision.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
            EncabezadoRemision.TotalLetra = TotalLetras;
            EncabezadoRemision.Editar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("IdEncabezadoRemision", EncabezadoRemision.IdEncabezadoRemision));
            oRespuesta.Add(new JProperty("Total", EncabezadoRemision.Total));
            oRespuesta.Add(new JProperty("NumeroRemision", EncabezadoRemision.Folio));
            oRespuesta.Add(new JProperty("Error", 0));

            //Modelo.Add(new JProperty("Permisos", oPermisos));
            //oRespuesta.Add(new JProperty("Error", 0));
            //oRespuesta.Add(new JProperty("Modelo", Modelo));

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
    public static string AgregarDetalleProductoRemision(Dictionary<string, object> pRegistros)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string TotalLetras = "";
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
            CDetalleRemision DetalleRemision = new CDetalleRemision();
            CProducto Producto = new CProducto();
            CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();

            if (Convert.ToInt32(pRegistros["pTodos"]) == 0)
            {
                DetalleFacturaProveedor.LlenaObjeto(Convert.ToInt32(pRegistros["pIdDetFacProveedor"]), ConexionBaseDatos);
                CotizacionDetalle.LlenaObjeto(Convert.ToInt32(pRegistros["pIdDetPedido"]), ConexionBaseDatos);
                DetalleRemision.IdEncabezadoRemision = Convert.ToInt32(pRegistros["pIdEncabezadoRemision"]);
                DetalleRemision.Cantidad = Convert.ToInt32(pRegistros["pCantidad"]);
                DetalleRemision.PrecioUnitario = Convert.ToDecimal(pRegistros["pPrecioConTipoCambio"]);
                DetalleRemision.Monto = (Convert.ToInt32(pRegistros["pCantidad"]) * Convert.ToDecimal(pRegistros["pPrecioConTipoCambio"]));
                DetalleRemision.IdProducto = Convert.ToInt32(pRegistros["pIdProducto"]);
                DetalleRemision.IdEncabezadoFacturaProveedor = Convert.ToInt32(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor);
                DetalleRemision.IdDetalleFacturaProveedor = Convert.ToInt32(pRegistros["pIdDetFacProveedor"]);
                DetalleRemision.IdEncabezadoPedido = Convert.ToInt32(CotizacionDetalle.IdCotizacion);
                DetalleRemision.IdDetallePedido = Convert.ToInt32(pRegistros["pIdDetPedido"]);
                DetalleRemision.IdProyecto = Convert.ToInt32(pRegistros["pIdProyecto"]);
                DetalleRemision.IdAlmacen = Convert.ToInt32(pRegistros["pIdAlmacen"]);
                DetalleRemision.FechaAlta = Convert.ToDateTime(DateTime.Now);
                DetalleRemision.Baja = false;
                DetalleRemision.AgregarDetalleRemision(ConexionBaseDatos);

                CExistenciaDistribuida ExistenciaDistribuida = new CExistenciaDistribuida();
                ExistenciaDistribuida.LlenaObjeto(Convert.ToInt32(pRegistros["pIdExistenciaDistribuida"]), ConexionBaseDatos);
                ExistenciaDistribuida.Saldo = ExistenciaDistribuida.Saldo - Convert.ToInt32(pRegistros["pCantidad"]);
                ExistenciaDistribuida.Editar(ConexionBaseDatos);

                EncabezadoRemision.LlenaObjeto(Convert.ToInt32(pRegistros["pIdEncabezadoRemision"]), ConexionBaseDatos);
                HistorialGenerico.IdGenerico = DetalleRemision.IdDetalleRemision;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nueva partida de remisión";
                HistorialGenerico.AgregarHistorialGenerico("DetalleRemision", ConexionBaseDatos);
            }
            else
            {

                foreach (Dictionary<string, object> oPartidas in (Array)pRegistros["DetallePartidas"])
                {
                    CTipoCambio TipoCambio = new CTipoCambio();

                    Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
                    ParametrosTS.Add("Opcion", 1);
                    ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(oPartidas["IdTipoCambioOrigen"]));
                    ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(oPartidas["IdTipoCambioDestino"]));
                    ParametrosTS.Add("Fecha", DateTime.Today);
                    TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, ConexionBaseDatos);

                    DetalleFacturaProveedor.LlenaObjeto(Convert.ToInt32(oPartidas["IdDetalleFacturaProveedor"]), ConexionBaseDatos);
                    CotizacionDetalle.LlenaObjeto(Convert.ToInt32(oPartidas["IdPedidoDetalle"]), ConexionBaseDatos);
                    DetalleRemision.IdEncabezadoRemision = Convert.ToInt32(pRegistros["pIdEncabezadoRemision"]);
                    DetalleRemision.Cantidad = Convert.ToInt32(oPartidas["Cantidad"]);

                    DetalleRemision.PrecioUnitario = Convert.ToDecimal(oPartidas["Precio"]) / TipoCambio.TipoCambio;
                    DetalleRemision.Monto = (Convert.ToInt32(oPartidas["Cantidad"]) * Convert.ToDecimal(DetalleRemision.PrecioUnitario));

                    DetalleRemision.IdProducto = Convert.ToInt32(oPartidas["IdProducto"]);
                    DetalleRemision.IdEncabezadoFacturaProveedor = Convert.ToInt32(DetalleFacturaProveedor.IdEncabezadoFacturaProveedor);
                    DetalleRemision.IdDetalleFacturaProveedor = Convert.ToInt32(oPartidas["IdDetalleFacturaProveedor"]);
                    DetalleRemision.IdEncabezadoPedido = Convert.ToInt32(CotizacionDetalle.IdCotizacion);
                    DetalleRemision.IdDetallePedido = Convert.ToInt32(oPartidas["IdPedidoDetalle"]);
                    DetalleRemision.IdProyecto = Convert.ToInt32(pRegistros["pIdProyecto"]);
                    DetalleRemision.IdAlmacen = Convert.ToInt32(pRegistros["pIdAlmacen"]);
                    DetalleRemision.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    DetalleRemision.Baja = false;
                    DetalleRemision.AgregarDetalleRemision(ConexionBaseDatos);

                    CExistenciaDistribuida ExistenciaDistribuida = new CExistenciaDistribuida();
                    ExistenciaDistribuida.LlenaObjeto(Convert.ToInt32(pRegistros["pIdExistenciaDistribuida"]), ConexionBaseDatos);
                    ExistenciaDistribuida.Saldo = ExistenciaDistribuida.Saldo - Convert.ToInt32(pRegistros["pCantidad"]);
                    ExistenciaDistribuida.Editar(ConexionBaseDatos);

                    EncabezadoRemision.LlenaObjeto(Convert.ToInt32(pRegistros["pIdEncabezadoRemision"]), ConexionBaseDatos);

                    HistorialGenerico.IdGenerico = DetalleRemision.IdDetalleRemision;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva partida de remisión";
                    HistorialGenerico.AgregarHistorialGenerico("DetalleRemision", ConexionBaseDatos);
                }

            }
            CUtilerias Utilerias = new CUtilerias();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(EncabezadoRemision.IdTipoMoneda, ConexionBaseDatos);
            TotalLetras = Utilerias.ConvertLetter(EncabezadoRemision.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
            EncabezadoRemision.TotalLetra = TotalLetras;
            EncabezadoRemision.Editar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Total", Convert.ToDecimal(EncabezadoRemision.Total)));
            oRespuesta.Add(new JProperty("NumeroRemision", EncabezadoRemision.Folio));
            oRespuesta.Add(new JProperty("Error", 0));

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
    public static string EditarEncabezadoRemision(Dictionary<string, object> pEncabezadoRemision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
        EncabezadoRemision.LlenaObjeto(Convert.ToInt32(pEncabezadoRemision["IdEncabezadoRemision"]), ConexionBaseDatos);
        EncabezadoRemision.IdEncabezadoRemision = Convert.ToInt32(pEncabezadoRemision["IdEncabezadoRemision"]);
        EncabezadoRemision.Nota = Convert.ToString(pEncabezadoRemision["Nota"]);
        string validacion = "";
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            EncabezadoRemision.Editar(ConexionBaseDatos);
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
    public static string EliminarDetalleRemision(Dictionary<string, object> pDetalleRemision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        int PuedeEliminarPartida = 0;

        CDetalleRemision DetalleRemision = new CDetalleRemision();
        DetalleRemision.LlenaObjeto(Convert.ToInt32(pDetalleRemision["pIdDetalleRemision"]), ConexionBaseDatos);
        DetalleRemision.IdDetalleRemision = Convert.ToInt32(pDetalleRemision["pIdDetalleRemision"]);
        DetalleRemision.Baja = true;

        JObject oRespuesta = new JObject();
        if (PuedeEliminarPartida == 0)
        {
            DetalleRemision.EliminarDetalleRemision(ConexionBaseDatos);
            CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
            EncabezadoRemision.LlenaObjeto(DetalleRemision.IdEncabezadoRemision, ConexionBaseDatos);
            oRespuesta.Add("Total", EncabezadoRemision.Total);
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

    [WebMethod]
    public static string Imprimir(int pIdEncabezadoRemision, string pFolio, string pTemplate, int pSinPrecio)
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

        JArray datos = (JArray)CEncabezadoRemision.obtenerDatosImpresionRemision(pIdEncabezadoRemision.ToString(), Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), Convert.ToInt32(pSinPrecio));

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "Remision_" + pFolio + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
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
            Modelo = CEncabezadoRemision.ObtenerEncabezadoRemision(Modelo, pIdEncabezadoRemision, ConexionBaseDatos);
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

    [WebMethod]
    public static string ConsolidarRemision(string pIdCotizacion, int pIdEncabezadoRemision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            CJson jsonRemisionados = new CJson();
            jsonRemisionados.StoredProcedure.CommandText = "sp_Remision_ProductoDetallePedido";
            jsonRemisionados.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdCotizacion);
            string jsonProductosFaltantes = jsonRemisionados.ObtenerJsonString(ConexionBaseDatos);
            var jProductos = JObject.Parse(jsonProductosFaltantes);
            int Cuantos = (int)jProductos["Table"][0]["Productos"];

            if (Cuantos > 0)
            {
                oRespuesta.Add(new JProperty("Error", 1));
            }
            else
            {
                CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
                EncabezadoRemision.LlenaObjeto(pIdEncabezadoRemision, ConexionBaseDatos);
                EncabezadoRemision.Consolidado = true;
                EncabezadoRemision.Editar(ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
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
    public static string CambiarEstatus(int pIdRemision, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int PuedeCancelarFactura = 0;
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
            EncabezadoRemision.IdEncabezadoRemision = Convert.ToInt32(pIdRemision);
            EncabezadoRemision.Baja = pBaja;
            JObject oRespuesta = new JObject();
            if (PuedeCancelarFactura == 0)
            {
                EncabezadoRemision.CancelarRemision(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "La remisión se cancelo correctamente"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta remisión porque existen movimientos con alguna de sus partidas"));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ActivarEstatus(int pIdRemision, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        bool reactiva = true;
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
            EncabezadoRemision.IdEncabezadoRemision = Convert.ToInt32(pIdRemision);
            EncabezadoRemision.Baja = pBaja;

            CDetalleRemision ListaPartidas = new CDetalleRemision();
            Dictionary<string, object> ParametrosOCD = new Dictionary<string, object>();
            ParametrosOCD.Add("Baja", 0);
            ParametrosOCD.Add("IdEncabezadoRemision", pIdRemision);

            int Saldo = 0;

            foreach (CDetalleRemision oDetalleRemision in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, ConexionBaseDatos))
            {
                if (Convert.ToInt32(oDetalleRemision.IdDetalleFacturaProveedor) != 0)
                {
                    CExistenciaDistribuida ExistenciaDistribuida = new CExistenciaDistribuida();
                    Dictionary<string, object> ParametrosED = new Dictionary<string, object>();
                    ParametrosED.Add("IdDetalleFacturaProveedor", oDetalleRemision.IdDetalleFacturaProveedor);
                    ExistenciaDistribuida.LlenaObjetoFiltros(ParametrosED, ConexionBaseDatos);
                    Saldo = ExistenciaDistribuida.Saldo;
                    int vCantidad = Saldo;
                    vCantidad -= Convert.ToInt32(oDetalleRemision.Cantidad);
                    reactiva = !(vCantidad < 0);
                    if (!reactiva) break;
                }
            }

            JObject oRespuesta = new JObject();
            if (reactiva)
            {
                EncabezadoRemision.ActivarRemision(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "La remisión se reactivo correctamente"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede reactivar esta remisión porque existen movimientos con alguna de sus partidas"));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string CancelarEncabezadoRemision(int pIdEncabezadoRemision)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int PuedeCancelarFactura = 0;
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
            EncabezadoRemision.IdEncabezadoRemision = Convert.ToInt32(pIdEncabezadoRemision);
            JObject oRespuesta = new JObject();
            if (PuedeCancelarFactura == 0)
            {
                EncabezadoRemision.CancelarRemision(ConexionBaseDatos);
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
    public static string ValidarGenerarRemisionSinCliente()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oPermisos = new JObject();
        oPermisos.Add("generaRemisionSinCliente", generaRemisionSinCliente);

        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            if (generaRemisionSinCliente == 1)
            {
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No tiene permisos para generar una remision sin cliente"));
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
}