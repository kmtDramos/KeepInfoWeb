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

public partial class Cotizacion : System.Web.UI.Page
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
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        ticks = DateTime.Now.Ticks.ToString();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeDarMantenimiento" }, ConexionBaseDatos) == "") { puedeDarMantenimiento = 1; }
        else { puedeDarMantenimiento = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeAgregarCotizacion" }, ConexionBaseDatos) == "") { puedeAgregarCotizacion = 1; }
        else { puedeAgregarCotizacion = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarCotizacion" }, ConexionBaseDatos) == "") { puedeEditarCotizacion = 1; }
        else { puedeEditarCotizacion = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEliminarCotizacion" }, ConexionBaseDatos) == "") { puedeEliminarCotizacion = 1; }
        else { puedeEliminarCotizacion = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeConsultarCotizacion" }, ConexionBaseDatos) == "") { puedeConsultarCotizacion = 1; }
        else { puedeConsultarCotizacion = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeGenerarPedido" }, ConexionBaseDatos) == "") { puedeGenerarPedido = 1; }
        else { puedeGenerarPedido = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarVigenciaCotizacion" }, ConexionBaseDatos) == "") { puedeEditarVigenciaCotizacion = 1; }
        else { puedeEditarVigenciaCotizacion = 0; }

        if (Usuario.TienePermisos(new string[] { "puedePasarPedidoACotizado" }, ConexionBaseDatos) == "") { puedePasarPedidoACotizado = 1; }
        else { puedePasarPedidoACotizado = 0; }

        //GridCotizacion
        CJQGrid GridCotizacion = new CJQGrid();
        GridCotizacion.NombreTabla = "grdCotizacion";
        GridCotizacion.CampoIdentificador = "IdCotizacion";
		GridCotizacion.TipoOrdenacion = "DESC";
        GridCotizacion.ColumnaOrdenacion = "Folio";
        GridCotizacion.Metodo = "ObtenerCotizacion";
        GridCotizacion.TituloTabla = "Catálogo de cotizaciones";
        GridCotizacion.GenerarFuncionFiltro = false;

        //IdCotizacion
        CJQColumn ColIdCotizacion = new CJQColumn();
        ColIdCotizacion.Nombre = "IdCotizacion";
        ColIdCotizacion.Oculto = "true";
        ColIdCotizacion.Encabezado = "IdCotizacion";
        ColIdCotizacion.Buscador = "false";
        GridCotizacion.Columnas.Add(ColIdCotizacion);

        //NoCotizacion
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        ColFolio.Ancho = "50";
        GridCotizacion.Columnas.Add(ColFolio);

        //Razon Social
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "200";
        GridCotizacion.Columnas.Add(ColRazonSocial);

        //Oportunidad
        CJQColumn ColOportunidad = new CJQColumn();
        ColOportunidad.Nombre = "IdOportunidad";
        ColOportunidad.Encabezado = "Oportunidad";
        ColOportunidad.Buscador = "true";
        ColOportunidad.Alineacion = "left";
        ColOportunidad.Ancho = "50";
        GridCotizacion.Columnas.Add(ColOportunidad);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "Fecha";
        ColFecha.Encabezado = "Alta";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridCotizacion.Columnas.Add(ColFecha);

        //Valido Hasta
        CJQColumn ColValidoHasta = new CJQColumn();
        ColValidoHasta.Nombre = "ValidoHasta";
        ColValidoHasta.Encabezado = "Vencimiento";
        ColValidoHasta.Buscador = "false";
        ColValidoHasta.Alineacion = "left";
        ColValidoHasta.Ancho = "80";
        GridCotizacion.Columnas.Add(ColValidoHasta);

        //Simbolo
        CJQColumn ColSimbolo = new CJQColumn();
        ColSimbolo.Nombre = "Simbolo";
        ColSimbolo.Oculto = "true";
        ColSimbolo.Encabezado = "Simbolo";
        ColSimbolo.Buscador = "false";
        GridCotizacion.Columnas.Add(ColSimbolo);

        //Nota
        CJQColumn ColNota = new CJQColumn();
        ColNota.Nombre = "Nota";
        ColNota.Encabezado = "Nota";
        ColNota.Buscador = "false";
        ColNota.Alineacion = "left";
        ColNota.Ancho = "170";
        GridCotizacion.Columnas.Add(ColNota);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "100";
        GridCotizacion.Columnas.Add(ColTotal);

        //Moneda
        CJQColumn ColMoneda = new CJQColumn();
        ColMoneda.Nombre = "Moneda";
        ColMoneda.Encabezado = "Moneda";
        ColMoneda.Buscador = "false";
        ColMoneda.Alineacion = "left";
        ColMoneda.Ancho = "70";
        GridCotizacion.Columnas.Add(ColMoneda);

		CJQColumn ColCompras = new CJQColumn();
		ColCompras.Nombre = "Compras";
		ColCompras.Encabezado = "Compras";
		ColCompras.Buscador = "false";
		ColCompras.Formato = "FormatoMoneda";
		ColCompras.Oculto = (Usuario.TienePermisos(new string[] { "verComprasPedidos" }, ConexionBaseDatos) == "") ? "false" : "true";
		ColCompras.Alineacion = "right";
		ColCompras.Ancho = "100";
		GridCotizacion.Columnas.Add(ColCompras);

		CJQColumn ColUtilidad = new CJQColumn();
		ColUtilidad.Nombre = "Utilidad";
		ColUtilidad.Encabezado = "Utilidad";
		ColUtilidad.Buscador = "false";
		ColUtilidad.Formato = "FormatoMoneda";
		ColUtilidad.Oculto = (Usuario.TienePermisos(new string[] { "verComprasPedidos" }, ConexionBaseDatos) == "") ? "false" : "true";
		ColUtilidad.Alineacion = "right";
		ColUtilidad.Ancho = "100";
		GridCotizacion.Columnas.Add(ColUtilidad);

		//Convertir a Pedido
		CJQColumn ConsolidarPedido = new CJQColumn();
        ConsolidarPedido.Nombre = "IdEstatusCotizacion";
        ConsolidarPedido.Encabezado = "Estatus";
        ConsolidarPedido.Ordenable = "true";
        ConsolidarPedido.Ancho = "70";
        ConsolidarPedido.Etiquetado = "EstatusInvertido";
        ConsolidarPedido.Buscador = "false";
        ConsolidarPedido.StoredProcedure.CommandText = "spc_ManejadorConvertirAPedido_Consulta";
        GridCotizacion.Columnas.Add(ConsolidarPedido);

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
        ColBaja.Oculto = puedeEliminarCotizacion == 1 ? "false" : "true";
        GridCotizacion.Columnas.Add(ColBaja);

        // Declinar
        if (Usuario.TienePermisos(new string[] {"puedeDeclinarCotizaciones"}, ConexionBaseDatos) == "")
        {
            CJQColumn ColDeclinar = new CJQColumn();
            ColDeclinar.Nombre = "Declinar";
            ColDeclinar.Encabezado = "Declinar";
            ColDeclinar.Estilo = "Declinar";
            ColDeclinar.Ordenable = "false";
            ColDeclinar.Ancho = "70";
            ColDeclinar.Buscador = "false";
            GridCotizacion.Columnas.Add(ColDeclinar);
        }

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCotizacion";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCotizacion.Columnas.Add(ColConsultar);

        //Mantenimiento
        CJQColumn ColMantenimiento = new CJQColumn();
        ColMantenimiento.Oculto = (puedeDarMantenimiento == 1)?"false" : "true";
        ColMantenimiento.Nombre = "Mantenimiento";
        ColMantenimiento.Encabezado = "Mantenimiento";
        ColMantenimiento.Etiquetado = "Mantenimiento";
        ColMantenimiento.Estilo = "divMantenimiento imgMantenimiento";
        ColMantenimiento.Buscador = "false";
        ColMantenimiento.Ordenable = "false";
        ColMantenimiento.Ancho = "25";
        GridCotizacion.Columnas.Add(ColMantenimiento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCotizacion", GridCotizacion.GeneraGrid(), true);

        //GridCotizacionDetalle
        CJQGrid grdCotizacionDetalle = new CJQGrid();
        grdCotizacionDetalle.NombreTabla = "grdCotizacionDetalle";
        grdCotizacionDetalle.CampoIdentificador = "IdCotizacionDetalle";
        grdCotizacionDetalle.ColumnaOrdenacion = "Ordenacion";
        grdCotizacionDetalle.TipoOrdenacion = "ASC";
        grdCotizacionDetalle.Metodo = "ObtenerCotizacionDetalle";
        grdCotizacionDetalle.TituloTabla = "Detalle de la cotización";
        grdCotizacionDetalle.GenerarGridCargaInicial = false;
        grdCotizacionDetalle.GenerarFuncionFiltro = false;
        grdCotizacionDetalle.GenerarFuncionTerminado = false;
        grdCotizacionDetalle.NumeroFila = true;
        grdCotizacionDetalle.Altura = 100;
        grdCotizacionDetalle.Ancho = 722;
        grdCotizacionDetalle.NumeroRegistros = 15;
        grdCotizacionDetalle.RangoNumeroRegistros = "15,30,60";

        //IdCotizacionDetalle
        CJQColumn ColIdCotizacionDetalle = new CJQColumn();
        ColIdCotizacionDetalle.Nombre = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Oculto = "true";
        ColIdCotizacionDetalle.Encabezado = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Buscador = "false";
        grdCotizacionDetalle.Columnas.Add(ColIdCotizacionDetalle);

        //Clavedetalle
        CJQColumn ColClaveDetalle = new CJQColumn();
        ColClaveDetalle.Nombre = "Clave";
        ColClaveDetalle.Encabezado = "Clave";
        ColClaveDetalle.Buscador = "false";
        ColClaveDetalle.Alineacion = "left";
        ColClaveDetalle.Ancho = "66";
        grdCotizacionDetalle.Columnas.Add(ColClaveDetalle);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "200";
        grdCotizacionDetalle.Columnas.Add(ColDescripcion);

        //ClaveProdServ
        CJQColumn ColClaveProdServ = new CJQColumn();
        ColClaveProdServ.Nombre = "ClaveProdServ";
        ColClaveProdServ.Encabezado = "Clave (SAT)";
        ColClaveProdServ.Buscador = "false";
        ColClaveProdServ.Alineacion = "left";
        ColClaveProdServ.Ancho = "80";
        grdCotizacionDetalle.Columnas.Add(ColClaveProdServ);

        //Tipo Moneda Detalle
        CJQColumn ColTipoMonedaDetalle = new CJQColumn();
        ColTipoMonedaDetalle.Nombre = "TipoMoneda";
        ColTipoMonedaDetalle.Encabezado = "Moneda";
        ColTipoMonedaDetalle.Buscador = "false";
        ColTipoMonedaDetalle.Alineacion = "left";
        ColTipoMonedaDetalle.Ancho = "54";
        grdCotizacionDetalle.Columnas.Add(ColTipoMonedaDetalle);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "right";
        ColCantidad.Ancho = "54";
        grdCotizacionDetalle.Columnas.Add(ColCantidad);

        //Precio Unitario
        CJQColumn ColPrecio = new CJQColumn();
        ColPrecio.Nombre = "PrecioUnitario";
        ColPrecio.Encabezado = "Precio U.";
        ColPrecio.Buscador = "false";
        ColPrecio.Formato = "FormatoMoneda";
        ColPrecio.Alineacion = "right";
        ColPrecio.Ancho = "72";
        grdCotizacionDetalle.Columnas.Add(ColPrecio);

        //Subtotal
        CJQColumn ColSubtotal = new CJQColumn();
        ColSubtotal.Nombre = "Subtotal";
        ColSubtotal.Encabezado = "Subtotal";
        ColSubtotal.Buscador = "false";
        ColSubtotal.Formato = "FormatoMoneda";
        ColSubtotal.Alineacion = "right";
        ColSubtotal.Ancho = "72";
        grdCotizacionDetalle.Columnas.Add(ColSubtotal);

        //Tipo Producto/Servicio
        CJQColumn ColPS = new CJQColumn();
        ColPS.Nombre = "TipoPS";
        ColPS.Encabezado = "PS";
        ColPS.Buscador = "false";
        ColPS.Alineacion = "right";
        ColPS.Ancho = "63";
        grdCotizacionDetalle.Columnas.Add(ColPS);

        //Tipo IVA
        CJQColumn ColTipoIva = new CJQColumn();
        ColTipoIva.Nombre = "TipoIva";
        ColTipoIva.Encabezado = "TipoIVA";
        ColTipoIva.Buscador = "false";
        ColTipoIva.Alineacion = "right";
        ColTipoIva.Ancho = "63";
        grdCotizacionDetalle.Columnas.Add(ColTipoIva);

        //Porcentaje Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "PorcentajeDescuento";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Buscador = "false";
        ColDescuento.Formato = "FormatoPorciento";
        ColDescuento.Alineacion = "right";
        ColDescuento.Ancho = "63";
        grdCotizacionDetalle.Columnas.Add(ColDescuento);

        //Cantidad Descuento
        CJQColumn ColCantidadDescuento = new CJQColumn();
        ColCantidadDescuento.Nombre = "CantidadDescuento";
        ColCantidadDescuento.Oculto = "true";
        ColCantidadDescuento.Encabezado = "Cantidad Descuento";
        ColCantidadDescuento.Formato = "FormatoMoneda";
        grdCotizacionDetalle.Columnas.Add(ColCantidadDescuento);

        //Id Tipo IVA
        CJQColumn ColIdTipoIVA = new CJQColumn();
        ColIdTipoIVA.Nombre = "IdTipoIVA";
        ColIdTipoIVA.Oculto = "true";
        ColIdTipoIVA.Encabezado = "IdTipoIVA";
        ColIdTipoIVA.Buscador = "false";
        grdCotizacionDetalle.Columnas.Add(ColIdTipoIVA);


        //Eliminar concepto detalle cotizacion
        CJQColumn ColEliminarConcepto = new CJQColumn();
        ColEliminarConcepto.Nombre = "Eliminar";
        ColEliminarConcepto.Encabezado = "Eliminar";
        ColEliminarConcepto.Etiquetado = "Imagen";
        ColEliminarConcepto.Imagen = "false.png";
        ColEliminarConcepto.Estilo = "divImagenConsultar imgEliminarConceptoEditar";
        ColEliminarConcepto.Buscador = "false";
        ColEliminarConcepto.Ordenable = "false";
        ColEliminarConcepto.Ancho = "60";
        grdCotizacionDetalle.Columnas.Add(ColEliminarConcepto);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCotizacionDetalle", grdCotizacionDetalle.GeneraGrid(), true);

        //GridCotizacionDetalleConsultar
        CJQGrid grdCotizacionDetalleConsultar = new CJQGrid();
        grdCotizacionDetalleConsultar.NombreTabla = "grdCotizacionDetalleConsultar";
        grdCotizacionDetalleConsultar.CampoIdentificador = "IdCotizacionDetalle";
        grdCotizacionDetalleConsultar.ColumnaOrdenacion = "Ordenacion";
        grdCotizacionDetalleConsultar.TipoOrdenacion = "ASC";
        grdCotizacionDetalleConsultar.Metodo = "ObtenerCotizacionDetalleConsultar";
        grdCotizacionDetalleConsultar.TituloTabla = "Detalle de la cotización";
        grdCotizacionDetalleConsultar.GenerarGridCargaInicial = false;
        grdCotizacionDetalleConsultar.GenerarFuncionFiltro = false;
        grdCotizacionDetalleConsultar.GenerarFuncionTerminado = false;
        grdCotizacionDetalleConsultar.Altura = 150;
        grdCotizacionDetalleConsultar.Ancho = 870;
        grdCotizacionDetalleConsultar.NumeroRegistros = 15;
        grdCotizacionDetalleConsultar.RangoNumeroRegistros = "15,30,60";

        //IdCotizacionDetalle
        CJQColumn ColIdCotizacionDetalleConsultar = new CJQColumn();
        ColIdCotizacionDetalleConsultar.Nombre = "IdCotizacionDetalle";
        ColIdCotizacionDetalleConsultar.Oculto = "true";
        ColIdCotizacionDetalleConsultar.Encabezado = "IdCotizacionDetalle";
        ColIdCotizacionDetalleConsultar.Buscador = "false";
        grdCotizacionDetalleConsultar.Columnas.Add(ColIdCotizacionDetalleConsultar);

        //ClaveDetalleConsultar
        CJQColumn ColClaveDetalleConsultar = new CJQColumn();
        ColClaveDetalleConsultar.Nombre = "Clave";
        ColClaveDetalleConsultar.Encabezado = "Clave";
        ColClaveDetalleConsultar.Buscador = "false";
        ColClaveDetalleConsultar.Alineacion = "left";
        ColClaveDetalleConsultar.Ancho = "50";
        grdCotizacionDetalleConsultar.Columnas.Add(ColClaveDetalleConsultar);

        //DescripcionConsultar
        CJQColumn ColDescripcionConsultar = new CJQColumn();
        ColDescripcionConsultar.Nombre = "Descripcion";
        ColDescripcionConsultar.Encabezado = "Descripción";
        ColDescripcionConsultar.Buscador = "false";
        ColDescripcionConsultar.Alineacion = "left";
        ColDescripcionConsultar.Ancho = "300";
        grdCotizacionDetalleConsultar.Columnas.Add(ColDescripcionConsultar);

        //ClaveProdServConsultar
        CJQColumn ClaveProdServConsultar = new CJQColumn();
        ClaveProdServConsultar.Nombre = "ClaveProdServ";
        ClaveProdServConsultar.Encabezado = "Clave (SAT)";
        ClaveProdServConsultar.Buscador = "false";
        ClaveProdServConsultar.Alineacion = "left";
        ClaveProdServConsultar.Ancho = "80";
        grdCotizacionDetalleConsultar.Columnas.Add(ClaveProdServConsultar);

        //Tipo Moneda Consultar
        CJQColumn ColTipoMonedaConsultar = new CJQColumn();
        ColTipoMonedaConsultar.Nombre = "TipoMoneda";
        ColTipoMonedaConsultar.Encabezado = "Moneda";
        ColTipoMonedaConsultar.Buscador = "false";
        ColTipoMonedaConsultar.Alineacion = "left";
        ColTipoMonedaConsultar.Ancho = "50";
        grdCotizacionDetalleConsultar.Columnas.Add(ColTipoMonedaConsultar);

        //CantidadConsultar
        CJQColumn ColCantidadConsultar = new CJQColumn();
        ColCantidadConsultar.Nombre = "Cantidad";
        ColCantidadConsultar.Encabezado = "Cantidad";
        ColCantidadConsultar.Buscador = "false";
        ColCantidadConsultar.Alineacion = "right";
        ColCantidadConsultar.Ancho = "50";
        grdCotizacionDetalleConsultar.Columnas.Add(ColCantidadConsultar);

        //PrecioConsultar
        CJQColumn ColPrecioConsultar = new CJQColumn();
        ColPrecioConsultar.Nombre = "Precio";
        ColPrecioConsultar.Encabezado = "Precio Unitario";
        ColPrecioConsultar.Buscador = "false";
        ColPrecioConsultar.Formato = "FormatoMoneda";
        ColPrecioConsultar.Alineacion = "right";
        ColPrecioConsultar.Ancho = "100";
        grdCotizacionDetalleConsultar.Columnas.Add(ColPrecioConsultar);

        //SubtotalConsultar
        CJQColumn ColSubtotalConsultar = new CJQColumn();
        ColSubtotalConsultar.Nombre = "Subtotal";
        ColSubtotalConsultar.Encabezado = "Subtotal";
        ColSubtotalConsultar.Buscador = "false";
        ColSubtotalConsultar.Formato = "FormatoMoneda";
        ColSubtotalConsultar.Alineacion = "right";
        ColSubtotalConsultar.Ancho = "100";
        grdCotizacionDetalleConsultar.Columnas.Add(ColSubtotalConsultar);

        //Tipo Producto/Servicio
        CJQColumn ColPSConsultar = new CJQColumn();
        ColPSConsultar.Nombre = "TipoPS";
        ColPSConsultar.Encabezado = "PS";
        ColPSConsultar.Buscador = "false";
        ColPSConsultar.Alineacion = "right";
        ColPSConsultar.Ancho = "100";
        grdCotizacionDetalleConsultar.Columnas.Add(ColPSConsultar);


        //Tipo IVA
        CJQColumn ColTipoIVA = new CJQColumn();
        ColTipoIVA.Nombre = "TipoIVA";
        ColTipoIVA.Encabezado = "TipoIVA";
        ColTipoIVA.Buscador = "false";
        ColTipoIVA.Alineacion = "right";
        ColTipoIVA.Ancho = "63";
        grdCotizacionDetalleConsultar.Columnas.Add(ColTipoIVA);

        //DescuentoConsultar
        CJQColumn ColDescuentoConsultar = new CJQColumn();
        ColDescuentoConsultar.Nombre = "PorcentajeDescuento";
        ColDescuentoConsultar.Encabezado = "Descuento";
        ColDescuentoConsultar.Buscador = "false";
        ColDescuentoConsultar.Formato = "FormatoPorciento";
        ColDescuentoConsultar.Alineacion = "right";
        ColDescuentoConsultar.Ancho = "65";
        grdCotizacionDetalleConsultar.Columnas.Add(ColDescuentoConsultar);

        //Cantidad Descuento
        CJQColumn ColCantidadDescuentoConsultar = new CJQColumn();
        ColCantidadDescuentoConsultar.Nombre = "CantidadDescuento";
        ColCantidadDescuentoConsultar.Oculto = "true";
        ColCantidadDescuentoConsultar.Encabezado = "Cantidad Descuento";
        ColCantidadDescuentoConsultar.Formato = "FormatoMoneda";
        grdCotizacionDetalleConsultar.Columnas.Add(ColCantidadDescuentoConsultar);

        //Id Tipo IVA
        CJQColumn ColIdTipoIVAConsultar = new CJQColumn();
        ColIdTipoIVAConsultar.Nombre = "IdTipoIVA";
        ColIdTipoIVAConsultar.Oculto = "true";
        ColIdTipoIVAConsultar.Encabezado = "IdTipoIVA";
        ColIdTipoIVAConsultar.Buscador = "false";
        grdCotizacionDetalleConsultar.Columnas.Add(ColIdTipoIVAConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCotizacionDetalleConsultar", grdCotizacionDetalleConsultar.GeneraGrid(), true);

        //GridCotizacionDetalleEditar
        CJQGrid grdCotizacionDetalleEditar = new CJQGrid();
        grdCotizacionDetalleEditar.NombreTabla = "grdCotizacionDetalleEditar";
        grdCotizacionDetalleEditar.CampoIdentificador = "IdCotizacionDetalle";
        grdCotizacionDetalleEditar.ColumnaOrdenacion = "Ordenacion";
        grdCotizacionDetalleEditar.TipoOrdenacion = "ASC";
        grdCotizacionDetalleEditar.Metodo = "ObtenerCotizacionDetalleEditar";
        grdCotizacionDetalleEditar.TituloTabla = "Detalle de la cotización";
        grdCotizacionDetalleEditar.GenerarGridCargaInicial = false;
        grdCotizacionDetalleEditar.GenerarFuncionFiltro = false;
        grdCotizacionDetalleEditar.GenerarFuncionTerminado = false;
        grdCotizacionDetalleEditar.NumeroFila = true;
        grdCotizacionDetalleEditar.Altura = 100;
        grdCotizacionDetalleEditar.Ancho = 722;
        grdCotizacionDetalleEditar.NumeroRegistros = 15;
        grdCotizacionDetalleEditar.RangoNumeroRegistros = "15,30,60";

        //IdCotizacionDetalle
        CJQColumn ColIdCotizacionDetalleEditar = new CJQColumn();
        ColIdCotizacionDetalleEditar.Nombre = "IdCotizacionDetalle";
        ColIdCotizacionDetalleEditar.Oculto = "true";
        ColIdCotizacionDetalleEditar.Encabezado = "IdCotizacionDetalle";
        ColIdCotizacionDetalleEditar.Buscador = "false";
        grdCotizacionDetalleEditar.Columnas.Add(ColIdCotizacionDetalleEditar);

        //ClaveDetalleEditar
        CJQColumn ColClaveDetalleEditar = new CJQColumn();
        ColClaveDetalleEditar.Nombre = "Clave";
        ColClaveDetalleEditar.Encabezado = "Clave";
        ColClaveDetalleEditar.Buscador = "false";
        ColClaveDetalleEditar.Alineacion = "left";
        ColClaveDetalleEditar.Ancho = "50";
        grdCotizacionDetalleEditar.Columnas.Add(ColClaveDetalleEditar);

        //DescripcionEditar
        CJQColumn ColDescripcionEditar = new CJQColumn();
        ColDescripcionEditar.Nombre = "Descripcion";
        ColDescripcionEditar.Encabezado = "Descripción";
        ColDescripcionEditar.Buscador = "false";
        ColDescripcionEditar.Alineacion = "left";
        ColDescripcionEditar.Ancho = "200";
        grdCotizacionDetalleEditar.Columnas.Add(ColDescripcionEditar);

        //ClaveProdServEditar
        CJQColumn ClaveProdServEditar = new CJQColumn();
        ClaveProdServEditar.Nombre = "ClaveProdServ";
        ClaveProdServEditar.Encabezado = "Clave (SAT)";
        ClaveProdServEditar.Buscador = "false";
        ClaveProdServEditar.Alineacion = "left";
        ClaveProdServEditar.Ancho = "80";
        grdCotizacionDetalleEditar.Columnas.Add(ClaveProdServEditar);

        //Tipo Moneda Editar
        CJQColumn ColTipoMonedaEditar = new CJQColumn();
        ColTipoMonedaEditar.Nombre = "TipoMoneda";
        ColTipoMonedaEditar.Encabezado = "Moneda";
        ColTipoMonedaEditar.Buscador = "false";
        ColTipoMonedaEditar.Alineacion = "left";
        ColTipoMonedaEditar.Ancho = "50";
        grdCotizacionDetalleEditar.Columnas.Add(ColTipoMonedaEditar);

        //CantidadEditar
        CJQColumn ColCantidadEditar = new CJQColumn();
        ColCantidadEditar.Nombre = "Cantidad";
        ColCantidadEditar.Encabezado = "Cantidad";
        ColCantidadEditar.Buscador = "false";
        ColCantidadEditar.Alineacion = "right";
        ColCantidadEditar.Ancho = "50";
        grdCotizacionDetalleEditar.Columnas.Add(ColCantidadEditar);

        //PrecioEditar
        CJQColumn ColPrecioEditar = new CJQColumn();
        ColPrecioEditar.Nombre = "Precio";
        ColPrecioEditar.Encabezado = "Precio Unitario";
        ColPrecioEditar.Buscador = "false";
        ColPrecioEditar.Formato = "FormatoMoneda";
        ColPrecioEditar.Alineacion = "right";
        ColPrecioEditar.Ancho = "100";
        grdCotizacionDetalleEditar.Columnas.Add(ColPrecioEditar);

        //SubtotalEditar
        CJQColumn ColSubtotalEditar = new CJQColumn();
        ColSubtotalEditar.Nombre = "Subtotal";
        ColSubtotalEditar.Encabezado = "Subtotal";
        ColSubtotalEditar.Buscador = "false";
        ColSubtotalEditar.Formato = "FormatoMoneda";
        ColSubtotalEditar.Alineacion = "right";
        ColSubtotalEditar.Ancho = "100";
        grdCotizacionDetalleEditar.Columnas.Add(ColSubtotalEditar);

        //Tipo Producto/Servicio
        CJQColumn ColPSEditar = new CJQColumn();
        ColPSEditar.Nombre = "TipoPS";
        ColPSEditar.Encabezado = "PS";
        ColPSEditar.Buscador = "false";
        ColPSEditar.Alineacion = "right";
        ColPSEditar.Ancho = "100";
        grdCotizacionDetalleEditar.Columnas.Add(ColPSEditar);


        //Tipo IVA
        CJQColumn ColTipoIVAE = new CJQColumn();
        ColTipoIVAE.Nombre = "TipoIVA";
        ColTipoIVAE.Encabezado = "TipoIVA";
        ColTipoIVAE.Buscador = "false";
        ColTipoIVAE.Alineacion = "right";
        ColTipoIVAE.Ancho = "63";
        grdCotizacionDetalleEditar.Columnas.Add(ColTipoIVAE);

        //DescuentoEditar
        CJQColumn ColDescuentoEditar = new CJQColumn();
        ColDescuentoEditar.Nombre = "PorcentajeDescuento";
        ColDescuentoEditar.Encabezado = "Descuento";
        ColDescuentoEditar.Buscador = "false";
        ColDescuentoEditar.Formato = "FormatoPorciento";
        ColDescuentoEditar.Alineacion = "right";
        ColDescuentoEditar.Ancho = "50";
        grdCotizacionDetalleEditar.Columnas.Add(ColDescuentoEditar);

        //Cantidad Descuento
        CJQColumn ColCantidadDescuentoEditar = new CJQColumn();
        ColCantidadDescuentoEditar.Nombre = "CantidadDescuento";
        ColCantidadDescuentoEditar.Oculto = "true";
        ColCantidadDescuentoEditar.Encabezado = "Cantidad Descuento";
        ColCantidadDescuentoEditar.Formato = "FormatoMoneda";
        grdCotizacionDetalleEditar.Columnas.Add(ColCantidadDescuentoEditar);

        //Id Tipo IVA
        CJQColumn ColIdTipoIVAEditar = new CJQColumn();
        ColIdTipoIVAEditar.Nombre = "IdTipoIVA";
        ColIdTipoIVAEditar.Oculto = "true";
        ColIdTipoIVAEditar.Encabezado = "IdTipoIVA";
        ColIdTipoIVAEditar.Buscador = "false";
        grdCotizacionDetalleEditar.Columnas.Add(ColIdTipoIVAEditar);

        //Eliminar concepto cotizacion editar
        CJQColumn ColEliminarConceptoEditar = new CJQColumn();
        ColEliminarConceptoEditar.Nombre = "Eliminar";
        ColEliminarConceptoEditar.Encabezado = "Eliminar";
        ColEliminarConceptoEditar.Etiquetado = "Imagen";
        ColEliminarConceptoEditar.Imagen = "false.png";
        ColEliminarConceptoEditar.Estilo = "divImagenConsultar imgEliminarConceptoEditar";
        ColEliminarConceptoEditar.Buscador = "false";
        ColEliminarConceptoEditar.Ordenable = "false";
        ColEliminarConceptoEditar.Ancho = "25";
        grdCotizacionDetalleEditar.Columnas.Add(ColEliminarConceptoEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCotizacionDetalleEditar", grdCotizacionDetalleEditar.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCotizacion(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, string pIdOportunidad, int pIdEstatusCotizacion, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
		
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCotizacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdOportunidad);
        Stored.Parameters.Add("pIdEstatusCotizacion", SqlDbType.Int).Value = pIdEstatusCotizacion;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = Usuario.IdSucursalActual;
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCotizacionDetalle(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCotizacion)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCotizacionDetalleConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCotizacion", SqlDbType.Int).Value = pIdCotizacion;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCotizacionDetalleConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCotizacion)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCotizacionDetalleConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCotizacion", SqlDbType.Int).Value = pIdCotizacion;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCotizacionDetalleEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCotizacion)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCotizacionDetalleConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCotizacion", SqlDbType.Int).Value = pIdCotizacion;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string GenerarPedido(int pIdCotizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oPermisos = new JObject();
        oPermisos.Add("puedeGenerarPedido", puedeGenerarPedido);

        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            if (puedeGenerarPedido == 1)
            {

                CCotizacion Cotizacion = new CCotizacion();
                Cotizacion.LlenaObjeto(pIdCotizacion, ConexionBaseDatos);
                Cotizacion.IdCotizacion = Cotizacion.IdCotizacion;
                Cotizacion.IdEstatusCotizacion = 3;
                Cotizacion.FechaPedido = Convert.ToDateTime(DateTime.Now);
                Cotizacion.Editar(ConexionBaseDatos);


                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(Cotizacion.IdCliente, ConexionBaseDatos);

                if (Cliente.EsCliente == false)
                {
                    Cliente.EsCliente = true;
                    Cliente.Editar(ConexionBaseDatos);


                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Cliente.IdCliente;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se cambio el prospecto a cliente";
                    HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);
                }

                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                COportunidad.ActualizarTotalesOportunidad(Cotizacion.IdOportunidad, ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Modelo", Modelo));

            }
            else
            {
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No tiene permisos para convertir a pedido"));
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
    public static string RegresarACotizado(int pIdCotizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        CCotizacion Cotizacion = new CCotizacion();
        CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();

        oPermisos.Add("puedePasarPedidoACotizado", puedePasarPedidoACotizado);

        Cotizacion.LlenaObjeto(pIdCotizacion, ConexionBaseDatos);
        Cotizacion.IdCotizacion = Cotizacion.IdCotizacion;
        Cotizacion.IdEstatusCotizacion = 2;
        Cotizacion.FechaPedido = new DateTime();

        string validacion = ValidarDocumentoLigado(CotizacionDetalle, Cotizacion, ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            if (puedePasarPedidoACotizado == 1)
            {
                if (validacion == "")
                {
                    Cotizacion.Editar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Cotizacion.IdCotizacion;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se regreso el pedido a cotización";
                    HistorialGenerico.AgregarHistorialGenerico("Cotizacion", ConexionBaseDatos);

                    Modelo.Add(new JProperty("Permisos", oPermisos));
                    oRespuesta.Add(new JProperty("Error", 0));
                    COportunidad.ActualizarTotalesOportunidad(Cotizacion.IdOportunidad, ConexionBaseDatos);
                    oRespuesta.Add(new JProperty("Modelo", Modelo));

                    CCliente Cliente = new CCliente();
                    Cliente.LlenaObjeto(Cotizacion.IdCliente, ConexionBaseDatos);

                    Dictionary<string, object> Parametros = new Dictionary<string, object>();
                    Parametros.Add("IdCliente", Cliente.IdCliente);
                    Parametros.Add("IdEstatusCotizacion", 3);

                    CCotizacion Cotizaciones = new CCotizacion();
                    int cotizaciones = 0;
                    foreach (CCotizacion oCotizacion in Cotizaciones.LlenaObjetosFiltros(Parametros, ConexionBaseDatos))
                    {
                        cotizaciones++;
                    }

                    if (cotizaciones == 0)
                    {
                        Cliente.EsCliente = false;
                        Cliente.Editar(ConexionBaseDatos);

                        HistorialGenerico.IdGenerico = Cliente.IdCliente;
                        HistorialGenerico.IdUsuario = Convert.ToInt32(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
                        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                        HistorialGenerico.Comentario = "Se regreso el cliente a prospecto";
                        HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);

                    }
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));

                }
            }
            else
            {
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No tiene permisos para convertir a pedido"));
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
    public static string BuscarFolio(string pFolio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CJson jsonFolio = new CJson();
        jsonFolio.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosGrid";
        jsonFolio.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonFolio.StoredProcedure.Parameters.AddWithValue("@pFolio", pFolio);
        jsonFolio.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Usuario.IdSucursalActual);
        jsonFolio.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonFolio.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        return jsonFolio.ObtenerJsonString(ConexionBaseDatos);

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdCotizacion, bool pBaja, int pIdEstatusCotizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCotizacion Cotizacion = new CCotizacion();
        CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
        Cotizacion.LlenaObjeto(pIdCotizacion, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        JObject Modelo = new JObject();

        CUsuario Usuario = new CUsuario();

        bool estado = pBaja;
        string textoestado;

        if (estado == true)
        {
            textoestado = "inactivo";
        }
        else
        {
            textoestado = "activo";
        }

        oPermisos.Add("puedeEliminarCotizacion", puedeEliminarCotizacion);
        string validacion = ValidarBaja(CotizacionDetalle, Cotizacion, ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            if (validacion == "")
            {

                Cotizacion.IdCotizacion = pIdCotizacion;
                Cotizacion.Baja = pBaja;
                Cotizacion.Eliminar(ConexionBaseDatos);

                Cotizacion.IdCotizacion = pIdCotizacion;
                Cotizacion.IdEstatusCotizacion = pIdEstatusCotizacion;
                Cotizacion.Editar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Cotizacion.IdCotizacion;
                HistorialGenerico.IdUsuario = Convert.ToInt32(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "El estatus de la cotización cambió a " + textoestado + "";
                HistorialGenerico.AgregarHistorialGenerico("Cotizacion", ConexionBaseDatos);

                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                COportunidad.ActualizarTotalesOportunidad(Cotizacion.IdOportunidad, ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Modelo", Modelo));

            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                COportunidad.ActualizarTotalesOportunidad(Cotizacion.IdOportunidad, ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string AgregarCotizacion(Dictionary<string, object> pCotizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CCotizacion Cotizacion = new CCotizacion();
            Cotizacion.LlenaObjeto(Convert.ToInt32(pCotizacion["IdCotizacion"]), ConexionBaseDatos);
            Cotizacion.Nota = Convert.ToString(pCotizacion["Nota"]);
            Cotizacion.IdCliente = Convert.ToInt32(pCotizacion["IdCliente"]);
            Cotizacion.ValidoHasta = Convert.ToDateTime(pCotizacion["ValidoHasta"]);
            Cotizacion.IdContactoOrganizacion = Convert.ToInt32(pCotizacion["IdContactoOrganizacion"]);
            Cotizacion.FechaAlta = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            Cotizacion.IdUsuarioAgente = Convert.ToInt32(pCotizacion["IdUsuarioAgente"]);
            Cotizacion.IdCampana = Convert.ToInt32(pCotizacion["IdCampana"]);
            Cotizacion.IdUsuarioCotizador = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            //if (Convert.ToInt32(pCotizacion["IdCotizacion"]) == 0)
            //{
                Cotizacion.IdTipoMoneda = Convert.ToInt32(pCotizacion["IdTipoMonedaOrigen"]);
                Cotizacion.TipoCambio = Convert.ToDecimal(pCotizacion["TipoCambio"]);
           // }
            Cotizacion.AutorizacionIVA = Convert.ToDecimal(pCotizacion["AutorizacionIVA"]);
            Cotizacion.Proyecto = Convert.ToString(pCotizacion["Proyecto"]);
            Cotizacion.IdNivelInteresCotizacion = Convert.ToInt32(pCotizacion["IdNivelInteresCotizacion"]);
            Cotizacion.IdDivision = Convert.ToInt32(pCotizacion["IdDivision"]);
            Cotizacion.IdSucursalEjecutaServicio = Convert.ToInt32(pCotizacion["IdSucursalEjecutaServicio"]);
            Cotizacion.Oportunidad = Convert.ToString(pCotizacion["Oportunidad"]);
            Cotizacion.IdOportunidad = Convert.ToInt32(pCotizacion["IdOportunidad"]);

            string validacion = ValidarCotizacion(Cotizacion, ConexionBaseDatos);

            CUsuario UsuarioSesion = new CUsuario();
            int IdUsuarioSesion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            UsuarioSesion.LlenaObjeto(IdUsuarioSesion, ConexionBaseDatos);
            if (UsuarioSesion.IdUsuario == 0)
            {
                validacion = "La sesión se a terminado, favor de volver a ingresar al sistema";
            }
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Cotizacion.IdCotizacion = Convert.ToInt32(pCotizacion["IdCotizacion"]);
                if (Cotizacion.IdCotizacion == 0)
                {
                    Cotizacion.IdEstatusCotizacion = Convert.ToInt32(pCotizacion["IdEstatusCotizacion"]);
                    Cotizacion.AgregarCotizacion(ConexionBaseDatos);//especial para el folio

                    CCotizacionSucursal CotizacionSucursal = new CCotizacionSucursal();
                    CotizacionSucursal.IdCotizacion = Cotizacion.IdCotizacion;
                    CotizacionSucursal.IdSucursal = Usuario.IdSucursalActual;
                    CotizacionSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    CotizacionSucursal.IdUsuarioAlta = Convert.ToInt32(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
                    CotizacionSucursal.Agregar(ConexionBaseDatos);

                    /**** INI - Guarda Tipo de cambio para la cotizacion ****/
                    CTipoCambio TiposCambio = new CTipoCambio();

                    Dictionary<string, object> ParametrosTC = new Dictionary<string, object>();
                    ParametrosTC.Add("Fecha", DateTime.Today);

                    foreach (CTipoCambio oTipoCambio in TiposCambio.LlenaObjetosFiltros(ParametrosTC, ConexionBaseDatos))
                    {
                        Dictionary<string, object> Parametros = new Dictionary<string, object>();
                        Parametros.Add("IdCotizacion", Convert.ToInt32(Cotizacion.IdCotizacion));
                        Parametros.Add("IdTipoMonedaOrigen", oTipoCambio.IdTipoMonedaOrigen);
                        Parametros.Add("IdTipoMonedaDestino", oTipoCambio.IdTipoMonedaDestino);
                        Parametros.Add("Fecha", DateTime.Today);

                        CTipoCambioCotizacion vTipoCambioC = new CTipoCambioCotizacion();
                        vTipoCambioC.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                        if (vTipoCambioC.IdTipoCambioCotizacion > 0) break;

                        CTipoCambioCotizacion TipoCambioC = new CTipoCambioCotizacion();
                        TipoCambioC.Fecha = oTipoCambio.Fecha;
                        TipoCambioC.IdCotizacion = Convert.ToInt32(Cotizacion.IdCotizacion);
                        TipoCambioC.IdTipoMonedaOrigen = oTipoCambio.IdTipoMonedaOrigen;
                        TipoCambioC.IdTipoMonedaDestino = oTipoCambio.IdTipoMonedaDestino;
                        TipoCambioC.TipoCambio = oTipoCambio.TipoCambio;
                        TipoCambioC.Agregar(ConexionBaseDatos);
                    }
                    /**** FIN - Guarda Tipo de cambio para la cotizacion ****/

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Cotizacion.IdCotizacion;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto una nueva cotizacion";
                    HistorialGenerico.AgregarHistorialGenerico("Cotizacion", ConexionBaseDatos);
                }
                else
                {
                    Cotizacion.Editar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Cotizacion.IdCotizacion;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se modificó el encabezado de la cotizacion";
                    HistorialGenerico.AgregarHistorialGenerico("Cotizacion", ConexionBaseDatos);
                }

                oRespuesta.Add(new JProperty("Error", 0));
                COportunidad.ActualizarTotalesOportunidad(Convert.ToInt32(pCotizacion["IdOportunidad"]), ConexionBaseDatos);
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
        else
        { return "1|" + respuesta; }
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
    public static string ObtenerFormaAgregarCotizacion()
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
            string validacion = ValidarExisteTipoCambio(TipoCambio, Sucursal, Fecha, ConexionBaseDatos);
            CNivelInteresCotizacion NivelInteresCotizacion = new CNivelInteresCotizacion();
            if (validacion == "")
            {
                Modelo.Add("FechaAlta", DateTime.Now.ToShortDateString());
                DateTime fechaValidoHasta = DateTime.Now.AddDays(15);
                Modelo.Add("ValidoHasta", fechaValidoHasta.ToShortDateString());
                Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(Sucursal.IdTipoMoneda, ConexionBaseDatos));
                Modelo.Add("IVAActual", Convert.ToDecimal(Sucursal.IVAActual));
                Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuario(ConexionBaseDatos));
                Modelo.Add("Campanas", CCampana.ObtenerJsonCampana(true, ConexionBaseDatos));
                Modelo.Add("TiempoEntregas", CTiempoEntrega.ObtenerJsonTiempoEntrega(ConexionBaseDatos));
                Modelo.Add("NivelInteresCotizacion", CNivelInteresCotizacion.ObtenerJsonNivelInteresCotizacion(ConexionBaseDatos));
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
	public static string ObtenerFormaFiltroCotizacion()
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
		Consulta.StoredProcedure.CommandText = "sp_Cotizacion_Filtro_grdCotizacion_Estatus";
		Consulta.Llena(ConexionBaseDatos);

		while (Consulta.Registros.Read()) {
			JObject oEstatus = new JObject();
			oEstatus.Add("Valor", Convert.ToInt32(Consulta.Registros["IdEstatusCotizacion"]));
			oEstatus.Add("Descripcion", Convert.ToString(Consulta.Registros["EstatusCotizacion"]));
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
    public static string BuscarProducto(Dictionary<string, object> pProducto)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CProducto jsonProducto = new CProducto();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_ConsultarFiltros";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);

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

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;

    }

    [WebMethod]
    public static string BuscarServicio(Dictionary<string, object> pServicio)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CServicio jsonServicio = new CServicio();
        jsonServicio.StoredProcedure.CommandText = "sp_Servicio_ConsultarFiltros";
        jsonServicio.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);

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
        string jsonServicioString = jsonServicio.ObtenerJsonServicio(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonServicioString;

    }

    [WebMethod]
    public static string obtenerProducto(int IdProducto)
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
            CProducto Producto = new CProducto();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            CExistenciaDistribuida ExistenciaDistribuida = new CExistenciaDistribuida();

            Producto.LlenaObjeto(IdProducto, ConexionBaseDatos);
            TipoMoneda.LlenaObjeto(Producto.IdTipoMoneda, ConexionBaseDatos);
            UnidadCompraVenta.LlenaObjeto(Producto.IdUnidadCompraVenta, ConexionBaseDatos);

            Modelo.Add("Precio", Convert.ToDecimal(Producto.Precio));
            Modelo.Add("ValorMedida", Producto.ValorMedida);
            Modelo.Add("IdTipoMonedaProducto", Producto.IdTipoMoneda);
            Modelo.Add("TipoMonedaProducto", TipoMoneda.TipoMoneda);
            Modelo.Add("SimboloMonedaProducto", TipoMoneda.Simbolo);
            Modelo.Add("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta);
            Modelo.Add("Descripcion", Producto.Descripcion);
            Modelo.Add("IdTipoIVA", Producto.IdTipoIVA);

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdUsuario", Usuario.IdUsuario);
            Parametros.Add("IdProducto", IdProducto);
            ExistenciaDistribuida.ObtenerJsonExistencia(Parametros, ConexionBaseDatos);
            Modelo.Add("Existencia", ExistenciaDistribuida.Saldo);

            JObject ComboDescuento =  new JObject();
            ComboDescuento.Add("Opciones", CDescuentoProducto.ObtenerJsonDescuentoProducto(IdProducto, ConexionBaseDatos));
            ComboDescuento.Add("ValorDefault", "0");
            ComboDescuento.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("ListaDescuento", ComboDescuento);

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
    public static string ObtenerPrecio(int IdProducto, int IdServicio, int IdTipoMonedaOrigen, int IdTipoMonedaDestino, decimal Precio)
    {
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {

			if (Error == 0)
			{
				JObject Modelo = new JObject();

				//############################################################################################################

				CProducto Producto = new CProducto();
				Producto.LlenaObjeto(IdProducto, pConexion);

				CServicio Servicio = new CServicio();
				Servicio.LlenaObjeto(IdServicio, pConexion);

				CTipoCambio TipoCambio = new CTipoCambio();

				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdTipoMonedaOrigen", IdTipoMonedaDestino);
				pParametros.Add("IdTipoMonedaDestino", IdTipoMonedaOrigen);
				pParametros.Add("Fecha", DateTime.Today);

				TipoCambio.LlenaObjetoFiltrosTipoCambio(pParametros, pConexion);

				decimal tipoCambio = (IdTipoMonedaDestino == 1) ? 1 : TipoCambio.TipoCambio;

				Precio = (Producto.IdProducto != 0) ? Producto.Precio * tipoCambio : Precio;
				Precio = (Servicio.IdServicio != 0) ? Servicio.Precio * tipoCambio : Precio;


				JObject TipoMoneda = new JObject();
				TipoMoneda.Add("Opciones", CTipoMoneda.ObtenerJsonTiposMoneda(IdTipoMonedaOrigen, pConexion));
				TipoMoneda.Add("ValorDefault", "0");
				TipoMoneda.Add("DescripcionDefault", "Elegir una opción...");
				Modelo.Add("ListaTipoMoneda", TipoMoneda);

				Modelo.Add("MonedaPrecio", Precio);
				Modelo.Add("TipoCambioActual", tipoCambio);

				//############################################################################################################

				Respuesta.Add("Modelo", Modelo);
			}

			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);

		});

		return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaDescuento(int IdProducto)
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

            Modelo.Add("Opciones", CDescuentoProducto.ObtenerJsonDescuentoProducto(IdProducto, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");

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
    public static string AgregarCotizacionDetalle(Dictionary<string, object> pCotizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CCliente Cliente = new CCliente();
            COrganizacion Organizacion = new COrganizacion();
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            CCotizacion Cotizacion = new CCotizacion();

            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            int ExisteCotizacionDetalle = 1;

            CotizacionDetalle.IdCotizacion = Convert.ToInt32(pCotizacion["IdCotizacion"]);

            Boolean valida = true;
            if (Convert.ToInt32(pCotizacion["IdProducto"]) != 0)
            {
                Producto.LlenaObjeto(Convert.ToInt32(pCotizacion["IdProducto"]), ConexionBaseDatos);
                CotizacionDetalle.Clave = Convert.ToString(Producto.Clave);
                CotizacionDetalle.IdProducto = Convert.ToInt32(Producto.IdProducto);

                //if (Producto.ClaveProdServ == "" || Producto.ClaveProdServ == null)
                //{
                //    valida = false;
                //    oRespuesta.Add(new JProperty("Error", 1));
                //    oRespuesta.Add(new JProperty("Descripcion", "No es posible cargar el producto. Favor de colocar la ClaveProdServ (SAT) en el producto"));
                //}
                //else
                //{
                    CotizacionDetalle.ClaveProdServ = Producto.ClaveProdServ;
                //}
            }
            else
            {
                Servicio.LlenaObjeto(Convert.ToInt32(pCotizacion["IdServicio"]), ConexionBaseDatos);
                CotizacionDetalle.Clave = Convert.ToString(Servicio.Clave);
                CotizacionDetalle.IdServicio = Convert.ToInt32(Servicio.IdServicio);

                //if (Servicio.ClaveProdServ == "" || Servicio.ClaveProdServ == null)
                //{
                //    valida = false;
                //    oRespuesta.Add(new JProperty("Error", 1));
                //    oRespuesta.Add(new JProperty("Descripcion", "No es posible cargar el Servicio. Favor de colocarle la ClaveProdServ (SAT) en el servicio"));
                //}
                //else
                //{
                    CotizacionDetalle.ClaveProdServ = Servicio.ClaveProdServ;
                //}
            }

            if (Convert.ToInt32(pCotizacion["IdCliente"]) != 0)
            {
                Cliente.LlenaObjeto(Convert.ToInt32(pCotizacion["IdCliente"]), ConexionBaseDatos);
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);
            }

            if (valida)
            {
                CotizacionDetalle.Descripcion = Convert.ToString(pCotizacion["Descripcion"].ToString());
                CotizacionDetalle.Cantidad = Convert.ToInt32(pCotizacion["Cantidad"]);
                CotizacionDetalle.PrecioUnitario = Convert.ToDecimal(pCotizacion["PrecioUnitario"]);
                CotizacionDetalle.Total = Convert.ToDecimal(pCotizacion["Total"]);
                CotizacionDetalle.Descuento = Convert.ToDecimal(pCotizacion["Descuento"]);
                CotizacionDetalle.OrdenDeCompraCantidad = Convert.ToInt32(pCotizacion["Cantidad"]);
                CotizacionDetalle.RecepcionCantidad = Convert.ToInt32(pCotizacion["Cantidad"]);
                CotizacionDetalle.RemisionCantidad = Convert.ToInt32(pCotizacion["Cantidad"]);
                CotizacionDetalle.FacturacionCantidad = Convert.ToInt32(pCotizacion["Cantidad"]);
                CotizacionDetalle.IdTiempoDeEntrega = Convert.ToInt32(pCotizacion["IdTiempoEntrega"]);
                CotizacionDetalle.Ordenacion = ExisteCotizacionDetalle;
                //CotizacionDetalle.PartidaCompuesta = Convert.ToBoolean(pCotizacion["PartidaCompuesta"]);
                // cosas que no se pueden actualizar despues de creado
                if (CotizacionDetalle.IdCotizacion == 0)
                {
                    Cotizacion.TipoCambio = Convert.ToDecimal(pCotizacion["TipoCambio"]);
                    Cotizacion.IdTipoMoneda = Convert.ToInt32(pCotizacion["IdTipoMonedaOrigen"]);
                }

                CotizacionDetalle.IdTipoIVA = Convert.ToInt32(pCotizacion["IdTipoIVA"]);
                CotizacionDetalle.IVA = Convert.ToDecimal(pCotizacion["IVADetalle"]);

                if (CotizacionDetalle.IdCotizacion != 0)
                {
                    Cotizacion.LlenaObjeto(Convert.ToInt32(CotizacionDetalle.IdCotizacion), ConexionBaseDatos);
                }
                Cotizacion.FechaAlta = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                Cotizacion.SubTotal = Convert.ToDecimal(pCotizacion["SubtotalConDescuento"]);
                Cotizacion.Total = Convert.ToDecimal(pCotizacion["TotalCot"]);
                Cotizacion.Nota = Convert.ToString(pCotizacion["Nota"]);
                Cotizacion.ValidoHasta = Convert.ToDateTime(pCotizacion["ValidoHasta"]);
                Cotizacion.IdCliente = Convert.ToInt32(pCotizacion["IdCliente"]);
                Cotizacion.IdContactoOrganizacion = Convert.ToInt32(pCotizacion["IdContactoOrganizacion"]);
                Cotizacion.IdUsuarioAgente = Convert.ToInt32(pCotizacion["IdUsuarioAgente"]);
                Cotizacion.IdUsuarioCotizador = Convert.ToInt32(Usuario.IdUsuario);
                Cotizacion.IVA = Convert.ToDecimal(pCotizacion["IVACot"]);
                Cotizacion.IdCampana = Convert.ToInt32(pCotizacion["IdCampana"]);
                Cotizacion.AutorizacionIVA = Convert.ToDecimal(pCotizacion["AutorizacionIVA"]);
                Cotizacion.CantidadTotalLetra = Convert.ToString(pCotizacion["CantidadTotalLetra"].ToString());
                Cotizacion.Proyecto = "";//Convert.ToString(pCotizacion["Proyecto"]);
                Cotizacion.IdNivelInteresCotizacion = Convert.ToInt32(pCotizacion["IdNivelInteresCotizacion"]);
                Cotizacion.IdDivision = Convert.ToInt32(pCotizacion["IdDivision"]);
                Cotizacion.Oportunidad = Convert.ToString(pCotizacion["Oportunidad"]);
                Cotizacion.IdOportunidad = Convert.ToInt32(pCotizacion["IdOportunidad"]);
                Cotizacion.IdSucursalEjecutaServicio = Convert.ToInt32(pCotizacion["IdSucursalEjecutaServicio"]);
                Cotizacion.Baja = false;

                string validacion = ValidarDatosCotizacionDetalle(CotizacionDetalle, ConexionBaseDatos);

                CUsuario UsuarioSesion = new CUsuario();
                int IdUsuarioSesion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                UsuarioSesion.LlenaObjeto(IdUsuarioSesion, ConexionBaseDatos);
                if (UsuarioSesion.IdUsuario == 0)
                {
                    validacion = "La sesión se a terminado, favor de volver a ingresar al sistema";
                }
                if (validacion == "")
                {
                    if (CotizacionDetalle.IdCotizacion != 0)
                    {
                        Cotizacion.IdCotizacion = CotizacionDetalle.IdCotizacion;
                        Cotizacion.Editar(ConexionBaseDatos);

                        ExisteCotizacionDetalle = CCotizacion.ExisteCotizacionDetalle(CotizacionDetalle.IdCotizacion, ConexionBaseDatos);
                        if (ExisteCotizacionDetalle != 0)
                        {
                            ExisteCotizacionDetalle = ExisteCotizacionDetalle + 1;
                            CotizacionDetalle.Ordenacion = ExisteCotizacionDetalle;
                        }

                        CotizacionDetalle.Agregar(ConexionBaseDatos);

                        CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                        HistorialGenerico.IdGenerico = CotizacionDetalle.IdCotizacionDetalle;
                        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                        HistorialGenerico.Comentario = "Se inserto una nueva partida a la cotización";
                        HistorialGenerico.AgregarHistorialGenerico("CotizacionDetalle", ConexionBaseDatos);
                        Modelo.Add("Agregar", 0);
                    }
                    else
                    {
                        Cotizacion.IdEstatusCotizacion = 1;
                        Cotizacion.AgregarCotizacion(ConexionBaseDatos); //especial para folio

                        CCotizacionSucursal CotizacionSucursal = new CCotizacionSucursal();
                        CotizacionSucursal.IdCotizacion = Cotizacion.IdCotizacion;
                        CotizacionSucursal.IdSucursal = Usuario.IdSucursalActual;
                        CotizacionSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                        CotizacionSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                        CotizacionSucursal.Agregar(ConexionBaseDatos);

                        /**** INI - Guarda Tipo de cambio para la cotizacion ****/
                        CTipoCambio TiposCambio = new CTipoCambio();
                        Dictionary<string, object> ParametrosTC = new Dictionary<string, object>();
                        ParametrosTC.Add("Fecha", DateTime.Today);
                        foreach (CTipoCambio oTipoCambio in TiposCambio.LlenaObjetosFiltros(ParametrosTC, ConexionBaseDatos))
                        {
                            Dictionary<string, object> Parametros = new Dictionary<string, object>();
                            Parametros.Add("IdCotizacion", Convert.ToInt32(Cotizacion.IdCotizacion));
                            Parametros.Add("IdTipoMonedaOrigen", oTipoCambio.IdTipoMonedaOrigen);
                            Parametros.Add("IdTipoMonedaDestino", oTipoCambio.IdTipoMonedaDestino);
                            Parametros.Add("Fecha", DateTime.Today);

                            CTipoCambioCotizacion vTipoCambioC = new CTipoCambioCotizacion();
                            vTipoCambioC.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                            if (vTipoCambioC.IdTipoCambioCotizacion > 0) break;

                            CTipoCambioCotizacion TipoCambioC = new CTipoCambioCotizacion();
                            TipoCambioC.Fecha = oTipoCambio.Fecha;
                            TipoCambioC.IdCotizacion = Convert.ToInt32(Cotizacion.IdCotizacion);
                            TipoCambioC.IdTipoMonedaOrigen = oTipoCambio.IdTipoMonedaOrigen;
                            TipoCambioC.IdTipoMonedaDestino = oTipoCambio.IdTipoMonedaDestino;
                            TipoCambioC.TipoCambio = oTipoCambio.TipoCambio;
                            TipoCambioC.Agregar(ConexionBaseDatos);
                        }
                        /**** FIN - Guarda Tipo de cambio para la cotizacion ****/

                        CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                        HistorialGenerico.IdGenerico = Cotizacion.IdCotizacion;
                        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                        HistorialGenerico.Comentario = "Se inserto una nueva cotizacion";
                        HistorialGenerico.AgregarHistorialGenerico("Cotizacion", ConexionBaseDatos);

                        CotizacionDetalle.IdCotizacion = Cotizacion.IdCotizacion;
                        CotizacionDetalle.Agregar(ConexionBaseDatos);

                        HistorialGenerico.IdGenerico = CotizacionDetalle.IdCotizacionDetalle;
                        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                        HistorialGenerico.Comentario = "Se inserto una nueva partida a la cotizacion";
                        HistorialGenerico.AgregarHistorialGenerico("CotizacionDetalle", ConexionBaseDatos);
                        Modelo.Add("Agregar", 1);

                        Cotizacion.LlenaObjeto(Convert.ToInt32(Cotizacion.IdCotizacion), ConexionBaseDatos);
                        Modelo.Add("Folio", Cotizacion.Folio);
                    }

                    CAutorizacionTipoCambio AutTipoCambio = new CAutorizacionTipoCambio();
                    AutTipoCambio.LlenaObjeto(Convert.ToInt32(pCotizacion["IdAutorizacionTipoCambio"]), ConexionBaseDatos);
                    AutTipoCambio.IdDocumento = Cotizacion.IdCotizacion;
                    AutTipoCambio.TipoDocumento = "Cotizacion";
                    AutTipoCambio.Editar(ConexionBaseDatos);

                    CAutorizacionIVA AutIVA = new CAutorizacionIVA();
                    AutIVA.LlenaObjeto(Convert.ToInt32(pCotizacion["IdAutorizacionIVA"]), ConexionBaseDatos);
                    AutIVA.IdDocumento = Cotizacion.IdCotizacion;
                    AutIVA.TipoDocumento = "Cotizacion";
                    AutIVA.Editar(ConexionBaseDatos);
				
                    Modelo.Add("IdCotizacion", CotizacionDetalle.IdCotizacion);

                    Modelo.Add("Subtotal", Cotizacion.SubTotal);
                    Modelo.Add("DescuentoCantidad", Convert.ToDecimal(pCotizacion["DescuentoCantidad"].ToString()));
                    Modelo.Add("SubtotalConDescuento", Convert.ToDecimal(pCotizacion["SubtotalConDescuento"].ToString()));
                    Modelo.Add("IVA", Cotizacion.IVA);
                    Modelo.Add("Total", Cotizacion.Total);
                    Modelo.Add("CantidadTotalLetra", Cotizacion.CantidadTotalLetra);
                    Modelo.Add("PorcentajeIVA", Cotizacion.AutorizacionIVA);

                    oRespuesta.Add(new JProperty("Modelo", Modelo));

				    ActualizarCotizacion(Cotizacion.IdCotizacion, ConexionBaseDatos);

                    COportunidad.ActualizarTotalesOportunidad(Convert.ToInt32(pCotizacion["IdOportunidad"]), ConexionBaseDatos);
                    oRespuesta.Add(new JProperty("Error", 0));
                    ConexionBaseDatos.CerrarBaseDatosSqlServer();

                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            return "1|" + respuesta;
        }
    }

	public static void ActualizarCotizacion(int IdCotizacion, CConexion pConexion)
	{

		CSelectEspecifico Consulta = new CSelectEspecifico();
		Consulta.StoredProcedure.CommandText = "sp_Cotizacion_ActualizarEncabezado";
		Consulta.StoredProcedure.Parameters.Add("IdCotizacion", SqlDbType.Int).Value = IdCotizacion;
		Consulta.Llena(pConexion);
		Consulta.CerrarConsulta();

	}

    [WebMethod]
    public static string ObtenerValorDescuento(int pIdDescuento, int pIdProducto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            if (pIdProducto != 0)
            {
                CDescuentoProducto DescuentoProducto = new CDescuentoProducto();
                DescuentoProducto.LlenaObjeto(pIdDescuento, ConexionBaseDatos);
                Modelo.Add(new JProperty("Descuento", DescuentoProducto.Descuento));
            }
            else
            {
                CDescuentoServicio DescuentoServicio = new CDescuentoServicio();
                DescuentoServicio.LlenaObjeto(pIdDescuento, ConexionBaseDatos);
                Modelo.Add(new JProperty("Descuento", DescuentoServicio.Descuento));
            }


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
    public static string ObtenerFormaAgregarCotizacionDetalle(int pIdCotizacionDetalle)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdCotizacionDetalle", pIdCotizacionDetalle);
            Parametros.Add("IdUsuarioCotizador", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            CotizacionDetalle.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdCotizacionDetalle", CotizacionDetalle.IdCotizacionDetalle));
            Modelo.Add(new JProperty("Clave", CotizacionDetalle.Clave));
            Modelo.Add(new JProperty("Descripcion", CotizacionDetalle.Descripcion));

            if (CotizacionDetalle.Descripcion.Length > 50)
            {

                Modelo.Add(new JProperty("DescripcionCorta", Convert.ToString(CotizacionDetalle.Descripcion.Substring(0, 50))));
            }
            else
            {
                Modelo.Add(new JProperty("DescripcionCorta", Convert.ToString(CotizacionDetalle.Descripcion)));
            }
            Modelo.Add(new JProperty("Cantidad", CotizacionDetalle.Cantidad));
            Modelo.Add(new JProperty("PrecioUnitario", CotizacionDetalle.PrecioUnitario));
            Modelo.Add(new JProperty("Total", CotizacionDetalle.Total));
            Modelo.Add(new JProperty("Descuento", CotizacionDetalle.Descuento));

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
    public static string ObtenerDatosCliente(int IdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            JObject ContactoOrganizacion = new JObject();
            JObject Usuario = new JObject();
            CCliente Cliente = new CCliente();

            Cliente.LlenaObjeto(IdCliente, ConexionBaseDatos);

            ContactoOrganizacion.Add("Opciones", CContactoOrganizacion.ObtenerJsonContactoOrganizacionFiltroIdCliente(IdCliente, ConexionBaseDatos));
            ContactoOrganizacion.Add("ValorDefault", "0");
            ContactoOrganizacion.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("ListaContactoOrganizacion", ContactoOrganizacion);

            CCondicionPago CondicionPago = new CCondicionPago();
            CondicionPago.LlenaObjeto(Cliente.IdCondicionPago, ConexionBaseDatos);
            Modelo.Add("CondicionPago", CondicionPago.CondicionPago);


            Usuario.Add("Opciones", CUsuario.ObtenerJsonUsuarioNombre(Cliente.IdUsuarioAgente, ConexionBaseDatos));
            Usuario.Add("ValorDefault", "0");
            Usuario.Add("DescripcionDefault", "");
            Modelo.Add("ListaAgente", Usuario);

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
    public static string obtenerServicio(int pIdServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            //Modelo = CServicio.ObtenerJsonServicio(Modelo, IdServicio, ConexionBaseDatos);


            CServicio Servicio = new CServicio();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();

            Servicio.LlenaObjeto(pIdServicio, ConexionBaseDatos);
            TipoMoneda.LlenaObjeto(Servicio.IdTipoMoneda, ConexionBaseDatos);
            UnidadCompraVenta.LlenaObjeto(Servicio.IdUnidadCompraVenta, ConexionBaseDatos);

            Modelo.Add("Precio", Convert.ToDecimal(Servicio.Precio));
            Modelo.Add("IdTipoMonedaServicio", Servicio.IdTipoMoneda);
            Modelo.Add("TipoMonedaServicio", TipoMoneda.TipoMoneda);
            Modelo.Add("SimboloMonedaServicio", TipoMoneda.Simbolo);

            Modelo.Add("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta);
            Modelo.Add("Servicio", Servicio.Servicio);
            Modelo.Add("IdTipoIVA", Servicio.IdTipoIVA);

            JObject ComboDescuento = new JObject();
            ComboDescuento.Add("Opciones", CDescuentoServicio.ObtenerJsonDescuento(pIdServicio, ConexionBaseDatos));
            ComboDescuento.Add("ValorDefault", "0");
            ComboDescuento.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("ListaDescuento", ComboDescuento);

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
    public static string ObtenerFormaCotizacion(int pIdCotizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        JObject Modelo = new JObject();

        oPermisos.Add("puedeConsultarCotizacion", puedeConsultarCotizacion);

        if (respuesta == "Conexion Establecida")
        {
            Modelo = CCotizacion.ObtenerJsonCotizacionEncabezado(Modelo, pIdCotizacion, ConexionBaseDatos);
            Modelo.Add("Permisos", oPermisos);
            
            decimal descuento = 0;
			decimal subtotal = 0;
            CCotizacionDetalle Detalle = new CCotizacionDetalle();
            Dictionary<string, object> Parametros = new Dictionary<string,object>();
            Parametros.Add("IdCotizacion", pIdCotizacion);
            Parametros.Add("Baja", 0);
            foreach (CCotizacionDetalle oDetalle in Detalle.LlenaObjetosFiltros(Parametros, ConexionBaseDatos))
            {
				subtotal += oDetalle.Total;
				descuento += oDetalle.Total * (oDetalle.Descuento / 100);
            }

            Modelo.Add("descuento", descuento.ToString("C"));
			Modelo.Add("SubtotalDescuento", (subtotal-descuento).ToString("C"));
            oRespuesta.Add("Error", 0);
            oRespuesta.Add("Modelo", Modelo);

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
    public static string EliminarCotizacionDetalle(Dictionary<string, object> pCotizacionDetalle)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        string TotalLetras = "";

        CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
        CotizacionDetalle.LlenaObjeto(Convert.ToInt32(pCotizacionDetalle["pIdCotizacionDetalle"]), ConexionBaseDatos);
        CotizacionDetalle.IdCotizacionDetalle = Convert.ToInt32(pCotizacionDetalle["pIdCotizacionDetalle"]);
        CotizacionDetalle.Baja = true;
        CotizacionDetalle.Eliminar(ConexionBaseDatos);

        Modelo = CCotizacion.ObtenerJsonCotizacionTotales(Modelo, CotizacionDetalle.IdCotizacion, Convert.ToInt32(pCotizacionDetalle["pIva"]), ConexionBaseDatos);

        CCotizacion Cotizacion = new CCotizacion();
        CUtilerias Utilerias = new CUtilerias();
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        Cotizacion.LlenaObjeto(CotizacionDetalle.IdCotizacion, ConexionBaseDatos);
        TipoMoneda.LlenaObjeto(Cotizacion.IdTipoMoneda, ConexionBaseDatos);
        TotalLetras = Utilerias.ConvertLetter(Modelo["Total"].ToString(), TipoMoneda.TipoMoneda.ToString());
        Cotizacion.IdCotizacion = CotizacionDetalle.IdCotizacion;
        Cotizacion.SubTotal = Convert.ToDecimal(Modelo["Subtotal"].ToString());
        Cotizacion.IVA = Convert.ToDecimal(Modelo["Iva"].ToString());
        Cotizacion.Total = Convert.ToDecimal(Modelo["Total"].ToString());
        Cotizacion.CantidadTotalLetra = TotalLetras;
        Cotizacion.Editar(ConexionBaseDatos);

        oRespuesta.Add(new JProperty("Modelo", Modelo));
        COportunidad.ActualizarTotalesOportunidad(Cotizacion.IdOportunidad, ConexionBaseDatos);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();

        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string ObtenerFormaEditarCotizacion(int IdCotizacion, int IdEstatusCotizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeEditarCotizacion", puedeEditarCotizacion);

        if (respuesta == "Conexion Establecida")
        {
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
            CTipoCambio TipoCambio = new CTipoCambio();
            DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            string validacion = ValidarExisteTipoCambio(TipoCambio, Sucursal, Fecha, ConexionBaseDatos);
            if (validacion == "")
            {
                if (IdEstatusCotizacion == 2)
                {
                    CCotizacion oCotizacion = new CCotizacion();
                    oCotizacion.LlenaObjeto(Convert.ToInt32(IdCotizacion), ConexionBaseDatos);
                    oCotizacion.IdEstatusCotizacion = 1;
                    oCotizacion.Editar(ConexionBaseDatos);

                    COportunidad.ActualizarTotalesOportunidad(oCotizacion.IdOportunidad, ConexionBaseDatos);
                }

                CCotizacion Cotizacion = new CCotizacion();
                Cotizacion.LlenaObjeto(IdCotizacion, ConexionBaseDatos);

                JObject Modelo = new JObject();
                Modelo = CCotizacion.ObtenerJsonCotizacionEncabezado(Modelo, IdCotizacion, ConexionBaseDatos);
                Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
                Modelo.Add("UsuarioAgentes", CUsuario.ObtenerJsonUsuarioNombre(Convert.ToInt32(Modelo["IdUsuarioAgente"].ToString()), ConexionBaseDatos));
                Modelo.Add("Contactos", CContactoOrganizacion.ObtenerJsonContactoOrganizacion(Convert.ToInt32(Modelo["IdOrganizacion"].ToString()), Convert.ToInt32(Modelo["IdContactoOrganizacion"].ToString()), ConexionBaseDatos));
                Modelo.Add("Campanas", CCampana.ObtenerJsonCampana(Convert.ToInt32(Modelo["IdCampana"].ToString()), ConexionBaseDatos));
                Modelo.Add("IVAActual", Convert.ToDecimal(Sucursal.IVAActual));
                Modelo.Add("TiempoEntregas", CTiempoEntrega.ObtenerJsonTiempoEntrega(ConexionBaseDatos));

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_Cotizacion_Oportunidad_Cliente";
				Consulta.StoredProcedure.Parameters.Add("IdCliente", SqlDbType.Int).Value = Cotizacion.IdCliente;

				JArray Opciones = CUtilerias.ObtenerConsulta(Consulta, ConexionBaseDatos);

				Modelo.Add("Oportunidades", Opciones);
                Modelo.Add("NivelInteres", CNivelInteresCotizacion.ObtenerJsonNivelInteresCotizacion(Cotizacion.IdNivelInteresCotizacion, ConexionBaseDatos));
                Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(Cotizacion.IdDivision, ConexionBaseDatos));

                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdDocumento", Convert.ToInt32(IdCotizacion));
                Parametros.Add("TipoDocumento", "Cotizacion");
                CAutorizacionTipoCambio AutTipoCambio = new CAutorizacionTipoCambio();
                AutTipoCambio.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                Modelo.Add("ClaveAutorizacion", Convert.ToString(AutTipoCambio.ClaveAutorizacion));

                CAutorizacionIVA AutIVA = new CAutorizacionIVA();
                AutIVA.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                Modelo.Add("ClaveAutorizacionIVA", Convert.ToString(AutIVA.ClaveAutorizacion));
                Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(Convert.ToInt32(Modelo["IdSucursalEjecutaServicio"].ToString()), ConexionBaseDatos));


                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No hay tipo de cambio del dia"));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
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
    public static string CalculaSumaTotal(int pIdCotizacion, int pIva)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();

            Modelo = CCotizacionDetalle.ObtenerCotizacionDetalleTotales(Modelo, pIdCotizacion, pIva, ConexionBaseDatos);

            CCotizacion Cotizacion = new CCotizacion();
            Cotizacion.LlenaObjeto(pIdCotizacion, ConexionBaseDatos);
            Cotizacion.IdCotizacion = pIdCotizacion;
            Cotizacion.SubTotal = Convert.ToDecimal(Modelo["Subtotal"].ToString());
            Cotizacion.IVA = Convert.ToDecimal(Modelo["Iva"].ToString());
            Cotizacion.Total = Convert.ToDecimal(Modelo["Total"].ToString());
            //Cotizacion.Editar(ConexionBaseDatos);


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
    public static string ObtenerTotalesEstatusCotizacion(string pFechaInicial, string pFechaFinal, int pPorFecha, int pFolio, string pRazonSocial, int pAI, int pIdEstatusCotizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CEstatusCotizacion EstatusCotizacion = new CEstatusCotizacion();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Dictionary<Int32, Int32> TotalesEstatusCotizacion = EstatusCotizacion.ObtenerTotalesEstatusCotizacion(Usuario.IdSucursalActual, pFechaInicial, pFechaFinal, pPorFecha, pFolio, pRazonSocial, pAI, pIdEstatusCotizacion, ConexionBaseDatos);
            int TotalCotizaciones = 0;
            JArray JTotalesEstatusCotizacion = new JArray();
            foreach (var Valor in TotalesEstatusCotizacion)
            {
                JObject JTotales = new JObject();
                JTotales.Add(new JProperty("IdEstatusCotizacion", Valor.Key));
                JTotales.Add(new JProperty("Contador", Valor.Value));
                JTotalesEstatusCotizacion.Add(JTotales);
                TotalCotizaciones = TotalCotizaciones + Valor.Value;
            }

            JObject JTotal = new JObject();
            JTotal.Add(new JProperty("IdEstatusCotizacion", 0));
            JTotal.Add(new JProperty("Contador", TotalCotizaciones));
            JTotalesEstatusCotizacion.Add(JTotal);

            Modelo.Add("TotalesEstatusCotizacion", JTotalesEstatusCotizacion);
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
    public static string ActualizarCotizacionDetalleOrdenacion(Dictionary<string, object> pCotizacionDetalle)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            List<CCotizacionDetalle> Partidas = new List<CCotizacionDetalle>();
            foreach (Dictionary<string, object> oPartida in (Array)pCotizacionDetalle["Partidas"])
            {

                CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
                CotizacionDetalle.IdCotizacionDetalle = Convert.ToInt32(oPartida["IdCotizacionDetalle"]);
                CotizacionDetalle.Ordenacion = Convert.ToInt32(oPartida["Ordenacion"]);
                Partidas.Add(CotizacionDetalle);
            }

            foreach (CCotizacionDetalle oPartida in Partidas)
            {
                oPartida.IdCotizacionDetalle = oPartida.IdCotizacionDetalle;
                oPartida.Ordenacion = oPartida.Ordenacion;
                oPartida.EditarOrdenacion(ConexionBaseDatos);
            }

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string GenerarCotizacion(Dictionary<string, object> pCotizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            JObject Modelo = new JObject();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CCotizacion Cotizacion = new CCotizacion();
            Cotizacion.LlenaObjeto(Convert.ToInt32(pCotizacion["IdCotizacion"]), ConexionBaseDatos);
            Cotizacion.IdEstatusCotizacion = Convert.ToInt32(pCotizacion["IdEstatusCotizacion"]);
            Cotizacion.IdOportunidad = Convert.ToInt32(pCotizacion["IdOportunidad"]);
            Cotizacion.FechaAlta = Convert.ToDateTime(DateTime.Now);

            string validacion = ValidarCotizacion(Cotizacion, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Cotizacion.IdCotizacion = Convert.ToInt32(pCotizacion["IdCotizacion"]);
                if (Cotizacion.IdCotizacion != 0)
                {
                    Cotizacion.Editar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Cotizacion.IdCotizacion;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se generó una cotizacion";
                    HistorialGenerico.AgregarHistorialGenerico("Cotizacion", ConexionBaseDatos);


                    Modelo.Add("Folio", Cotizacion.Folio);

                }

                oRespuesta.Add(new JProperty("Error", 0));
                COportunidad.ActualizarTotalesOportunidad(Cotizacion.IdOportunidad, ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Modelo", Modelo));
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
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerTipoCambio(int IdTipoMonedaOrigen, int IdTipoMonedaDestino, string ClaveAutorizacion, int IdCotizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        decimal vAutTipoCambio = 0;
        decimal vTipoCambio = 0;

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(IdTipoMonedaOrigen));
        Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(IdTipoMonedaDestino));
        Parametros.Add("Fecha", DateTime.Today);

        CTipoCambio TipoCambio = new CTipoCambio();
        TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);

        Parametros.Clear();
        Parametros.Add("Opcion", 1);
        Parametros.Add("Baja", 0);
        Parametros.Add("IdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(IdTipoMonedaOrigen));
        Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(IdTipoMonedaDestino));
        Parametros.Add("ClaveAutorizacion", ClaveAutorizacion);
        Parametros.Add("IdDocumento", IdCotizacion);
        Parametros.Add("TipoDocumento", "Cotizacion");

        CAutorizacionTipoCambio AutTipoCambio = new CAutorizacionTipoCambio();
        AutTipoCambio.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
        vAutTipoCambio = AutTipoCambio.TipoCambio;

        Parametros.Clear();
        Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(IdTipoMonedaOrigen));
        Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(IdTipoMonedaDestino));
        Parametros.Add("IdCotizacion", IdCotizacion);
        Parametros.Add("Fecha", DateTime.Today);

        CTipoCambioCotizacion TipoCambioC = new CTipoCambioCotizacion();
        TipoCambioC.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
        vTipoCambio = TipoCambioC.TipoCambio;

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
			Modelo.Add("TipoCambioUsuado", vTipoCambio > 0 ? 1 : vAutTipoCambio > 0 ? 2 : 3);
            Modelo.Add("TipoCambioActual", vTipoCambio > 0 ? vTipoCambio : vAutTipoCambio > 0 ? vAutTipoCambio : TipoCambio.TipoCambio);
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
                Modelo.Add("TipoCambioActual", 1);

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

    //actualiza partidas
    [WebMethod]
    public static string ObtenerPrecioPorMoneda2(Dictionary<string, object> pDato)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            if (Convert.ToInt32(pDato["Detalle"].ToString()) != 0)
            {
                CCotizacion Cotizacion = new CCotizacion();
                CTipoCambio TipoCambioCotizacion = new CTipoCambio();
                CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();

                Cotizacion.LlenaObjeto(Convert.ToInt32(pDato["IdCotizacion"].ToString()), ConexionBaseDatos);


                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()));
                Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(Cotizacion.IdTipoMoneda));
                Parametros.Add("Fecha", DateTime.Today);
                TipoCambioCotizacion.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);


                Dictionary<string, object> ParametrosC = new Dictionary<string, object>();
                ParametrosC.Add("Baja", 0);
                ParametrosC.Add("IdCotizacion", Convert.ToInt32(pDato["IdCotizacion"].ToString()));
                foreach (CCotizacionDetalle oCCotizacionDetalle in CotizacionDetalle.LlenaObjetosFiltros(ParametrosC, ConexionBaseDatos))
                {
                    CotizacionDetalle.LlenaObjeto(oCCotizacionDetalle.IdCotizacionDetalle, ConexionBaseDatos);
                    CotizacionDetalle.PrecioUnitario = (Convert.ToDecimal(oCCotizacionDetalle.PrecioUnitario) / Convert.ToDecimal(TipoCambioCotizacion.TipoCambio));
                    CotizacionDetalle.Total = (Convert.ToDecimal(oCCotizacionDetalle.Total) / Convert.ToDecimal(TipoCambioCotizacion.TipoCambio));
                    CotizacionDetalle.Editar(ConexionBaseDatos);
                }

                Modelo = CCotizacionDetalle.ObtenerCotizacionDetalleTotales(Modelo, Convert.ToInt32(pDato["IdCotizacion"].ToString()), Convert.ToInt32(pDato["Iva"].ToString()), ConexionBaseDatos);
                Cotizacion.SubTotal = Convert.ToDecimal(Modelo["Subtotal"].ToString());
                Cotizacion.IVA = Convert.ToDecimal(Modelo["Iva"].ToString());
                Cotizacion.Total = Convert.ToDecimal(Modelo["Total"].ToString());
                Cotizacion.IdTipoMoneda = Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString());
                Cotizacion.Editar(ConexionBaseDatos);

                Modelo.Add("IdCotizacion", Cotizacion.IdCotizacion);

                if (Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()) != 0 && Convert.ToInt32(pDato["IdTipoMonedaDestino"].ToString()) != 0)
                {
                    CTipoCambio TipoCambio = new CTipoCambio();
                    Dictionary<string, object> ParametrosTC = new Dictionary<string, object>();
                    ParametrosTC.Add("IdTipoMonedaOrigen", Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()));
                    ParametrosTC.Add("IdTipoMonedaDestino", Convert.ToInt32(pDato["IdTipoMonedaDestino"].ToString()));
                    ParametrosTC.Add("Fecha", DateTime.Today);
                    TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
                    Modelo.Add("MonedaPrecio", Convert.ToDecimal(pDato["Precio"].ToString()) / Convert.ToDecimal(TipoCambio.TipoCambio));
                    Modelo.Add("TipoCambioActual", TipoCambio.TipoCambio);
                }
                else
                {
                    Modelo.Add("MonedaPrecio", Convert.ToDecimal(0));

                }
            }
            else
            {

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
    public static string ObtenerAutorizacionTipoCambio(int IdTipoMonedaOrigen, int IdTipoMonedaDestino, string ClaveAutorizacion, int IdDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CAutorizacionTipoCambio jsonTipoCambio = new CAutorizacionTipoCambio();
            jsonTipoCambio.StoredProcedure.CommandText = "sp_AutorizacionTipoCambio_Obtener";
            jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", IdTipoMonedaOrigen);
            jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", IdTipoMonedaDestino);
            jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", ClaveAutorizacion);
            jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdDocumento", IdDocumento);
            return jsonTipoCambio.ObtenerJsonAutorizacionTipoCambio(ConexionBaseDatos);
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
    public static string DesactivaAutorizacionTipoCambio(int IdTipoMonedaOrigen, int IdTipoMonedaDestino, string ClaveAutorizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAutorizacionTipoCambio jsonTipoCambio = new CAutorizacionTipoCambio();
        jsonTipoCambio.StoredProcedure.CommandText = "sp_AutorizacionTipoCambio_Obtener";
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", IdTipoMonedaOrigen);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", IdTipoMonedaDestino);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", ClaveAutorizacion);
        string jsonTipoCambioString = jsonTipoCambio.ObtenerJsonAutorizacionTipoCambio(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonTipoCambioString;
    }

    [WebMethod]
    public static string ObtenerAutorizacionIVA(string ClaveAutorizacion, int IdDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAutorizacionIVA jsonIVA = new CAutorizacionIVA();
        jsonIVA.StoredProcedure.CommandText = "sp_AutorizacionIVA_Obtener";
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", ClaveAutorizacion);
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pIdDocumento", IdDocumento);
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pDisponible", true);
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        string jsonIVAString = jsonIVA.ObtenerJsonAutorizacionIVA(ConexionBaseDatos);
        return jsonIVAString;
    }

    [WebMethod]
    public static string DeclinarCotizacion(int IdCotizacion, string MotivoDeclinar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        JObject Respuesta = new JObject();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]),ConexionBaseDatos);
            if (Usuario.IdUsuario != 0)
            {
                if (Usuario.TienePermisos(new string[] { "puedeDeclinarCotizaciones" }, ConexionBaseDatos) == "")
                {
                    CCotizacion Cotizacion = new CCotizacion();
                    Cotizacion.LlenaObjeto(IdCotizacion, ConexionBaseDatos);
                    if (Cotizacion.IdEstatusCotizacion != 7)
                    {
                        Cotizacion.IdEstatusCotizacion = 7;
                        Cotizacion.IdUsuarioDeclinar = Usuario.IdUsuario;
                        Cotizacion.MotivoDeclinar = MotivoDeclinar;
                        Respuesta.Add("Error", 0);
                        Respuesta.Add("Descripcion", "Pedido declinado.");
                    }
                    else
                    {
                        Cotizacion.IdEstatusCotizacion = 1;
                        Cotizacion.IdUsuarioDeclinar = 0;
                        Cotizacion.MotivoDeclinar = "";
                        Respuesta.Add("Error", 0);
                        Respuesta.Add("Descripcion", "Pedido revertido.");
                    }
                    Cotizacion.Editar(ConexionBaseDatos);
                }
                else
                {
                    Respuesta.Add("Error", 1);
                    Respuesta.Add("Descripcion", "No cuentas con los permisos necesarios.");
                }
            }
            else
            {
                Respuesta.Add("Error", 1);
                Respuesta.Add("Descripcion", "Su sesión ha caducado, favor de iniciar sesión nuevamente.");
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
    public static string DesactivaAutorizacionIVA(string ClaveAutorizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAutorizacionIVA jsonIVA = new CAutorizacionIVA();
        jsonIVA.StoredProcedure.CommandText = "sp_AutorizacionIVA_Obtener";
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", ClaveAutorizacion);
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pDisponible", true);
        jsonIVA.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        string jsonIVAString = jsonIVA.ObtenerJsonAutorizacionIVA(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonIVAString;
    }

	[WebMethod]
	public static string ImpirmirCotizacion(int IdCotizacion)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
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
					Modelo.Add("ESTADORECEPTOR",EstadoR.Estado);
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

    [WebMethod]
    public static string Imprimir(int pIdCotizacion, string pFolio, string pTemplate)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUtilerias Util = new CUtilerias();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        CCotizacion Cotizacion = new CCotizacion();
        Cotizacion.LlenaObjeto(pIdCotizacion, ConexionBaseDatos);

        CUsuario UsuarioCotizador = new CUsuario();
        UsuarioCotizador.LlenaObjeto(Cotizacion.IdUsuarioAgente, ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;
        idSucursal = Sucursal.IdSucursal;
        idEmpresa = Empresa.IdEmpresa;
        logoEmpresa = Empresa.Logo;

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("ImpresionDocumento", pTemplate);

        CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
        ImpresionDocumento.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTempl = new Dictionary<string, object>();
        //ParametrosTempl.Add("IdEmpresa", idEmpresa);
        ParametrosTempl.Add("Baja", 0);
        ParametrosTempl.Add("IdImpresionDocumento", ImpresionDocumento.IdImpresionDocumento);

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(Cotizacion.IdCliente, ConexionBaseDatos);

        CDireccionOrganizacion Direccion = new CDireccionOrganizacion();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("IdTipoDireccion", 1);
        pParametros.Add("IdOrganizacion", Cliente.IdOrganizacion);
        int direcciones = 0;
        foreach (CDireccionOrganizacion oDireccion in Direccion.LlenaObjetosFiltros(pParametros, ConexionBaseDatos))
        {
            direcciones++;
        }

        if (direcciones > 1 || direcciones == 0)
        {
            ConexionBaseDatos.CerrarBaseDatos();
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "Favor de seleccionar una direccion fiscal al cliente"));
            return oRespuesta.ToString();
        }


        CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
        ImpresionTemplate.LlenaObjetoFiltros(ParametrosTempl, ConexionBaseDatos);

        JArray datos = (JArray)CCotizacion.obtenerDatosImpresionCotizacion(pIdCotizacion.ToString(), UsuarioCotizador.IdUsuario);

        string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "cotizacion_" + pFolio + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
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
            Modelo = CCotizacion.ObtenerJsonCotizacionEncabezado(Modelo, pIdCotizacion, ConexionBaseDatos);
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
    public static string ActivarCotizacionVencida(Dictionary<string, object> pCotizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt16(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        JObject oPermisos = new JObject();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            if (Usuario.TienePermisos(new string[] { "puedeEditarVigenciaCotizacion" }, ConexionBaseDatos) == "")
            {
                CCotizacion Cotizacion = new CCotizacion();
                Cotizacion.LlenaObjeto(Convert.ToInt32(pCotizacion["IdCotizacion"]), ConexionBaseDatos);
                Cotizacion.IdCotizacion = Cotizacion.IdCotizacion;
                Cotizacion.ValidoHasta = Convert.ToDateTime(pCotizacion["ValidoHasta"]);
                Cotizacion.Editar(ConexionBaseDatos);

                oPermisos.Add("puedeEditarVigenciaCotizacion", 1);

                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {

                oPermisos.Add("puedeEditarVigenciaCotizacion", 0);
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No tiene permiso para reasignar nueva fecha de vencimiento la cotización"));
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

    private static string ValidarDatosCotizacionDetalle(CCotizacionDetalle pCotizacionDetalle, CConexion pConexion)
    {
        string errores = "";

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarCotizacion(CCotizacion pCotizacion, CConexion pConexion)
    {
        string errores = "";
        int ExisteCotizacionDetalle = 0;

        if (pCotizacion.IdCliente == 0)
        { errores = errores + "<span>*</span> No hay Cliente por asociar, favor de elegir alguno.<br />"; }

        if (pCotizacion.IdUsuarioAgente == 0)
        { errores = errores + "<span>*</span> El campo agente esta vac&iacute;o, favor de capturarlo.<br />"; }


        if (pCotizacion.IdCotizacion != 0)
        {
            ExisteCotizacionDetalle = CCotizacion.ExisteCotizacionDetalle(pCotizacion.IdCotizacion, pConexion);
            if (ExisteCotizacionDetalle == 0)
            {
                errores = errores + "<span>*</span> No hay productos o servicios para cotizar.<br />";
                return errores;
            }
        }

        if (pCotizacion.IdOportunidad == 0)
        {
			errores = "<p>Favor de seleccionar una oportunidad</p>" + errores;
		}

		CProyecto Proyectos = new CProyecto();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdOportunidad", pCotizacion.IdOportunidad);
		pParametros.Add("Baja", 0);

		if (Proyectos.LlenaObjetosFiltros(pParametros, pConexion).Count > 0)
		{ errores += "<p>La oportunidad ya tiene un proyecto asignado.</p>"; }

		CCotizacion Cotizaciones = new CCotizacion();
		if (Cotizaciones.LlenaObjetosFiltros(pParametros, pConexion).Count > 0)
		{ errores += "<p>La oportunidad ya tiene un pedido asignado.</p>"; }

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

    private static string ValidarBaja(CCotizacionDetalle CotizacionDetalle, CCotizacion Cotizacion, CConexion pConexion)
    {
        string errores = "";
        bool DocumentoLigado = false;
        bool EsPedido = false;


        DocumentoLigado = CotizacionDetalle.ValidarBaja(Cotizacion.IdCotizacion, pConexion);
        if (DocumentoLigado == true)
        {
            errores = errores + "<span>*</span> El documento ya está ligado a un proceso, no se puede dar de baja <br />";
        }
        else
        {
            EsPedido = Cotizacion.ValidarBaja(Cotizacion.IdCotizacion, pConexion);
            if (EsPedido == true)
            {
                errores = errores + "<span>*</span> El documento ya se convirtió a pedido, no se puede dar de baja <br />";
            }
        }

        return errores;
    }

    private static string ValidarDocumentoLigado(CCotizacionDetalle CotizacionDetalle, CCotizacion Cotizacion, CConexion pConexion)
    {
        string errores = "";
        bool DocumentoLigado = false;

        DocumentoLigado = CotizacionDetalle.ValidarBaja(Cotizacion.IdCotizacion, pConexion);
        if (DocumentoLigado == true)
        {
            errores = errores + "<span>*</span> El documento ya está ligado a un proceso, no se puede regresar a cotización <br />";
        }
        return errores;
    }

    [WebMethod]
    public static string ObtenerListaTiempoEntrega(int pIdTiempoEntrega)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CTiempoEntrega.ObtenerJsonTiempoEntrega(pIdTiempoEntrega, ConexionBaseDatos));
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
    public static string ObtenerListaCampana(int pIdCampana)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CCampana.ObtenerJsonCampana(pIdCampana, ConexionBaseDatos));
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
    public static string ObtenerListaCampanaOportunidad(int pIdOportunidad)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            COportunidad Oportunidad = new COportunidad();
            Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);

            Modelo.Add("Opciones", CCampana.ObtenerJsonCampana(Oportunidad.IdCampana, ConexionBaseDatos));
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
    public static string ObtenerListaOportunidad(int pIdCliente)
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

			CSelectEspecifico Consulta = new CSelectEspecifico();
			Consulta.StoredProcedure.CommandText = "sp_Cotizacion_Oportunidad_Cliente";
			Consulta.StoredProcedure.Parameters.Add("IdCliente", SqlDbType.Int).Value = pIdCliente;

			JArray Opciones = CUtilerias.ObtenerConsulta(Consulta, ConexionBaseDatos);

            Modelo.Add("Opciones", Opciones);
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
    public static string ObtenerListaNivelInteres(int pIdOportunidad)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            COportunidad Oportunidad = new COportunidad();
            Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CNivelInteresCotizacion.ObtenerJsonNivelInteresCotizacion(Oportunidad.IdNivelInteresOportunidad, ConexionBaseDatos));
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

}