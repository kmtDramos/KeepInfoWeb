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

public partial class Asientos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GeneraGridAsientosContables();
        GeneraGridFacturaClienteDetalle();
        GeneraGridFacturaProveedorDetalle();
        GeneraGridCuentasPorCobrar();
        GeneraGridDepositos();
        GeneraGridEgresos();
        GeneraGridCheques();
        GeneraGridDetalleFacturaProveedorEditar();
    }

    #region Grids
    public void GeneraGridAsientosContables()
    {
        //GridAsientosContables
        CJQGrid GridAsientosContables = new CJQGrid();
        GridAsientosContables.NombreTabla = "grdAsientosContables";
        GridAsientosContables.CampoIdentificador = "IdAsientoContableDetalle";
        GridAsientosContables.ColumnaOrdenacion = "IdAsientoContableDetalle";
        GridAsientosContables.Metodo = "ObtenerAsientosContables";
        GridAsientosContables.TituloTabla = "AsientosContables";
        //GridAsientosContables.Ancho = 870;
        //GridAsientosContables.Altura = 80;

        //IdAsientoContableDetalle
        CJQColumn ColIdAsientoContableDetalle = new CJQColumn();
        ColIdAsientoContableDetalle.Nombre = "IdAsientoContableDetalle";
        ColIdAsientoContableDetalle.Oculto = "true";
        ColIdAsientoContableDetalle.Encabezado = "IdAsientoContableDetalle";
        ColIdAsientoContableDetalle.Buscador = "false";
        ColIdAsientoContableDetalle.Ordenable = "false";
        GridAsientosContables.Columnas.Add(ColIdAsientoContableDetalle);

        //IdAsientoContable
        CJQColumn ColIdAsientoContable = new CJQColumn();
        ColIdAsientoContable.Nombre = "IdAsientoContable";
        ColIdAsientoContable.Encabezado = "Asiento contable";
        ColIdAsientoContable.Buscador = "true";
        ColIdAsientoContable.Alineacion = "left";
        ColIdAsientoContable.Ancho = "50";
        ColIdAsientoContable.Ordenable = "false";
        GridAsientosContables.Columnas.Add(ColIdAsientoContable);

        //ColTipoAsientoContable
        CJQColumn ColTipoAsientoContable = new CJQColumn();
        ColTipoAsientoContable.Nombre = "TipoAsientoContable";
        ColTipoAsientoContable.Encabezado = "Tipo asiento contable";
        ColTipoAsientoContable.Buscador = "false";
        ColTipoAsientoContable.Alineacion = "left";
        ColTipoAsientoContable.Ancho = "70";
        ColTipoAsientoContable.Ordenable = "false";
        GridAsientosContables.Columnas.Add(ColTipoAsientoContable);

        //CuentaContable
        CJQColumn ColCuentaContable = new CJQColumn();
        ColCuentaContable.Nombre = "CuentaContable";
        ColCuentaContable.Encabezado = "Cuenta contable";
        ColCuentaContable.Buscador = "false";
        ColCuentaContable.Alineacion = "right";
        ColCuentaContable.Ancho = "70";
        ColCuentaContable.Ordenable = "false";
        GridAsientosContables.Columnas.Add(ColCuentaContable);

        //DescripcionCuentaContable
        CJQColumn ColDescripcionCuentaContable = new CJQColumn();
        ColDescripcionCuentaContable.Nombre = "DescripcionCuentaContable";
        ColDescripcionCuentaContable.Encabezado = "Descripción cuenta contable";
        ColDescripcionCuentaContable.Buscador = "false";
        ColDescripcionCuentaContable.Alineacion = "left";
        ColDescripcionCuentaContable.Ancho = "130";
        ColDescripcionCuentaContable.Ordenable = "false";
        GridAsientosContables.Columnas.Add(ColDescripcionCuentaContable);

        //Cargo
        CJQColumn ColCargo = new CJQColumn();
        ColCargo.Nombre = "Cargo";
        ColCargo.Encabezado = "Cargo";
        ColCargo.Buscador = "false";
        ColCargo.Alineacion = "right";
        ColCargo.Formato = "FormatoMoneda";
        ColCargo.Ancho = "50";
        ColCargo.Ordenable = "false";
        GridAsientosContables.Columnas.Add(ColCargo);

        //Abono
        CJQColumn ColAbono = new CJQColumn();
        ColAbono.Nombre = "Abono";
        ColAbono.Encabezado = "Abono";
        ColAbono.Buscador = "false";
        ColAbono.Alineacion = "right";
        ColAbono.Formato = "FormatoMoneda";
        ColAbono.Ancho = "50";
        ColAbono.Ordenable = "false";
        GridAsientosContables.Columnas.Add(ColAbono);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAsientosContables", GridAsientosContables.GeneraGrid(), true);
    }

    public void GeneraGridFacturaClienteDetalle()
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

    public void GeneraGridFacturaProveedorDetalle()
    {
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
        grdDetalleFacturaProveedorConsultar.Ancho = 870;
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
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColNumeroSerieConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleFacturaProveedorConsultar", grdDetalleFacturaProveedorConsultar.GeneraGrid(), true);
    }

    public void GeneraGridCuentasPorCobrar()
    {
        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobrosConsultar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosConsultar.Metodo = "ObtenerMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.TituloTabla = "Movimientos de cobros";
        grdMovimientosCobrosConsultar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosConsultar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosConsultar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosConsultar.Altura = 120;
        grdMovimientosCobrosConsultar.Ancho = 770;
        grdMovimientosCobrosConsultar.NumeroRegistros = 15;
        grdMovimientosCobrosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrarEncabezadoFactura
        CJQColumn ColIdCuentasPorCobrarEncabezadoFacturaConsultar = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Nombre = "IdCuentasPorCobrarEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Encabezado = "IdCuentasPorCobrarEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaConsultar);

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
        ColFechaPagoConsultar.Encabezado = "Fecha de pago";
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
    }

    public void GeneraGridDepositos()
    {
        //GridIngresosNoDepositadosConsultar
        CJQGrid GridIngresosNoDepositadosConsultar = new CJQGrid();
        GridIngresosNoDepositadosConsultar.NombreTabla = "grdIngresosNoDepositadosConsultar";
        GridIngresosNoDepositadosConsultar.CampoIdentificador = "IdIngresosNoDepositados";
        GridIngresosNoDepositadosConsultar.ColumnaOrdenacion = "Sucursal,Folio";
        GridIngresosNoDepositadosConsultar.Metodo = "ObtenerIngresosNoDepositadosConsultar";
        GridIngresosNoDepositadosConsultar.TituloTabla = "Ingresos no depositados";
        GridIngresosNoDepositadosConsultar.GenerarGridCargaInicial = false;
        GridIngresosNoDepositadosConsultar.GenerarFuncionFiltro = false;
        GridIngresosNoDepositadosConsultar.GenerarFuncionTerminado = false;
        GridIngresosNoDepositadosConsultar.Altura = 120;
        GridIngresosNoDepositadosConsultar.Ancho = 770;
        GridIngresosNoDepositadosConsultar.NumeroRegistros = 15;
        GridIngresosNoDepositadosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdIngresosNoDepositados
        CJQColumn ColIdIngresosNoDepositadosConsultar = new CJQColumn();
        ColIdIngresosNoDepositadosConsultar.Nombre = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositadosConsultar.Oculto = "true";
        ColIdIngresosNoDepositadosConsultar.Encabezado = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositadosConsultar.Buscador = "false";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColIdIngresosNoDepositadosConsultar);

        //Sucursal
        CJQColumn ColSucursalINDConsultar = new CJQColumn();
        ColSucursalINDConsultar.Nombre = "Sucursal";
        ColSucursalINDConsultar.Encabezado = "Sucursal";
        ColSucursalINDConsultar.Ancho = "10";
        ColSucursalINDConsultar.Buscador = "false";
        ColSucursalINDConsultar.Alineacion = "left";
        ColSucursalINDConsultar.Oculto = "true";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColSucursalINDConsultar);

        //Folio
        CJQColumn ColFolioINDConsultar = new CJQColumn();
        ColFolioINDConsultar.Nombre = "Folio";
        ColFolioINDConsultar.Encabezado = "Folio";
        ColFolioINDConsultar.Ancho = "80";
        ColFolioINDConsultar.Buscador = "false";
        ColFolioINDConsultar.Alineacion = "left";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColFolioINDConsultar);

        //Importe
        CJQColumn ColImporteINDConsultar = new CJQColumn();
        ColImporteINDConsultar.Nombre = "Importe";
        ColImporteINDConsultar.Encabezado = "Importe";
        ColImporteINDConsultar.Buscador = "false";
        ColImporteINDConsultar.Formato = "FormatoMoneda";
        ColImporteINDConsultar.Alineacion = "right";
        ColImporteINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColImporteINDConsultar);

        //Cuenta
        CJQColumn ColCuentaINDConsultar = new CJQColumn();
        ColCuentaINDConsultar.Nombre = "CuentaBancaria";
        ColCuentaINDConsultar.Encabezado = "Cuenta bancaria";
        ColCuentaINDConsultar.Buscador = "false";
        ColCuentaINDConsultar.Alineacion = "left";
        ColCuentaINDConsultar.Ancho = "100";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColCuentaINDConsultar);

        //TipoMoneda
        CJQColumn ColTipoMonedaINDConsultar = new CJQColumn();
        ColTipoMonedaINDConsultar.Nombre = "TipoMoneda";
        ColTipoMonedaINDConsultar.Encabezado = "Tipo de moneda";
        ColTipoMonedaINDConsultar.Buscador = "false";
        ColTipoMonedaINDConsultar.Alineacion = "left";
        ColTipoMonedaINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColTipoMonedaINDConsultar);

        //FechaEmision
        CJQColumn ColFechaEmisionINDConsultar = new CJQColumn();
        ColFechaEmisionINDConsultar.Nombre = "FechaEmision";
        ColFechaEmisionINDConsultar.Encabezado = "Fecha de emisión";
        ColFechaEmisionINDConsultar.Buscador = "false";
        ColFechaEmisionINDConsultar.Alineacion = "left";
        ColFechaEmisionINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColFechaEmisionINDConsultar);

        //FechaDeposito
        CJQColumn ColFechaDepositoINDConsultar = new CJQColumn();
        ColFechaDepositoINDConsultar.Nombre = "FechaDeposito";
        ColFechaDepositoINDConsultar.Encabezado = "Fecha de depósito";
        ColFechaDepositoINDConsultar.Buscador = "false";
        ColFechaDepositoINDConsultar.Alineacion = "left";
        ColFechaDepositoINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColFechaDepositoINDConsultar);

        //Asociado
        CJQColumn ColAsociadoINDConsultar = new CJQColumn();
        ColAsociadoINDConsultar.Nombre = "Asociado";
        ColAsociadoINDConsultar.Encabezado = "Asociado";
        ColAsociadoINDConsultar.Buscador = "false";
        ColAsociadoINDConsultar.Alineacion = "left";
        ColAsociadoINDConsultar.Ancho = "40";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColAsociadoINDConsultar);

        //Depositado
        CJQColumn ColDepositadoINDConsultar = new CJQColumn();
        ColDepositadoINDConsultar.Nombre = "Depositado";
        ColDepositadoINDConsultar.Encabezado = "Depositado";
        ColDepositadoINDConsultar.Buscador = "false";
        ColDepositadoINDConsultar.Alineacion = "left";
        ColDepositadoINDConsultar.Ancho = "40";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColDepositadoINDConsultar);

        //RazonSocial
        CJQColumn ColRazonSocialINDConsultar = new CJQColumn();
        ColRazonSocialINDConsultar.Nombre = "RazonSocial";
        ColRazonSocialINDConsultar.Encabezado = "Razón social";
        ColRazonSocialINDConsultar.Buscador = "false";
        ColRazonSocialINDConsultar.Alineacion = "left";
        ColRazonSocialINDConsultar.Ancho = "100";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColRazonSocialINDConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdIngresosNoDepositadosConsultar", GridIngresosNoDepositadosConsultar.GeneraGrid(), true);
    }

    public void GeneraGridEgresos()
    {
        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultarEgresos";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosConsultar.Metodo = "ObtenerMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.TituloTabla = "Movimientos de pagos";
        grdMovimientosCobrosConsultar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosConsultar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosConsultar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosConsultar.Altura = 120;
        grdMovimientosCobrosConsultar.Ancho = 770;
        grdMovimientosCobrosConsultar.NumeroRegistros = 15;
        grdMovimientosCobrosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrarEncabezadoFactura
        CJQColumn ColIdCuentasPorCobrarEncabezadoFacturaConsultar = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Nombre = "IdEgresosEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Encabezado = "IdEgresosEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaConsultar);

        //NumeroFacturaConsultar
        CJQColumn ColNumeroFacturaConsultar = new CJQColumn();
        ColNumeroFacturaConsultar.Nombre = "NumeroFactura";
        ColNumeroFacturaConsultar.Encabezado = "Número de factura";
        ColNumeroFacturaConsultar.Buscador = "false";
        ColNumeroFacturaConsultar.Alineacion = "left";
        ColNumeroFacturaConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColNumeroFacturaConsultar);

        //Fecha pago
        CJQColumn ColFechaPagoConsultar = new CJQColumn();
        ColFechaPagoConsultar.Nombre = "FechaPago";
        ColFechaPagoConsultar.Encabezado = "Fecha de pago";
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

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobrosConsultarEgresos", grdMovimientosCobrosConsultar.GeneraGrid(), true);
    }

    public void GeneraGridCheques()
    {
        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultarCheques";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdChequesEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdChequesEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosConsultar.Metodo = "ObtenerMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.TituloTabla = "Movimientos de pagos";
        grdMovimientosCobrosConsultar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosConsultar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosConsultar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosConsultar.Altura = 120;
        grdMovimientosCobrosConsultar.Ancho = 770;
        grdMovimientosCobrosConsultar.NumeroRegistros = 15;
        grdMovimientosCobrosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrarEncabezadoFactura
        CJQColumn ColIdCuentasPorCobrarEncabezadoFacturaConsultar = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Nombre = "IdChequesEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Encabezado = "IdChequesEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaConsultar);

        //NumeroFacturaConsultar
        CJQColumn ColNumeroFacturaConsultar = new CJQColumn();
        ColNumeroFacturaConsultar.Nombre = "NumeroFactura";
        ColNumeroFacturaConsultar.Encabezado = "Número de factura";
        ColNumeroFacturaConsultar.Buscador = "false";
        ColNumeroFacturaConsultar.Alineacion = "left";
        ColNumeroFacturaConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColNumeroFacturaConsultar);

        //Fecha pago
        CJQColumn ColFechaPagoConsultar = new CJQColumn();
        ColFechaPagoConsultar.Nombre = "FechaPago";
        ColFechaPagoConsultar.Encabezado = "Fecha de pago";
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

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobrosConsultarCheques", grdMovimientosCobrosConsultar.GeneraGrid(), true);
    }

    public void GeneraGridDetalleFacturaProveedorEditar()
    {
        //GridDetalleFacturaProveedorConsultar
        CJQGrid grdDetalleFacturaProveedorConsultar = new CJQGrid();
        grdDetalleFacturaProveedorConsultar.NombreTabla = "grdDetalleFacturaProveedorEditar";
        grdDetalleFacturaProveedorConsultar.CampoIdentificador = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedorConsultar.ColumnaOrdenacion = "IdDetalleFacturaProveedor";
        grdDetalleFacturaProveedorConsultar.TipoOrdenacion = "DESC";
        grdDetalleFacturaProveedorConsultar.Metodo = "ObtenerDetalleFacturaProveedorEditar";
        grdDetalleFacturaProveedorConsultar.TituloTabla = "DetalleFacturaProveedor";
        grdDetalleFacturaProveedorConsultar.GenerarGridCargaInicial = false;
        grdDetalleFacturaProveedorConsultar.GenerarFuncionFiltro = false;
        grdDetalleFacturaProveedorConsultar.GenerarFuncionTerminado = false;
        grdDetalleFacturaProveedorConsultar.Altura = 150;
        grdDetalleFacturaProveedorConsultar.Ancho = 870;
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

        //TotalConsultar
        CJQColumn ColTotalConsultar = new CJQColumn();
        ColTotalConsultar.Nombre = "Total";
        ColTotalConsultar.Encabezado = "Total";
        ColTotalConsultar.Buscador = "false";
        ColTotalConsultar.Formato = "FormatoMoneda";
        ColTotalConsultar.Alineacion = "right";
        ColTotalConsultar.Ancho = "30";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColTotalConsultar);

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
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColNumeroSerieConsultar);

        //TipoCompra
        CJQColumn ColTipoCompra = new CJQColumn();
        ColTipoCompra.Nombre = "TipoCompra";
        ColTipoCompra.Encabezado = "Tipo de compra";
        ColTipoCompra.Buscador = "false";
        ColTipoCompra.Alineacion = "left";
        ColTipoCompra.Ancho = "70";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColTipoCompra);

        //SubCuenta
        CJQColumn ColSubCuenta = new CJQColumn();
        ColSubCuenta.Nombre = "Subcuenta";
        ColSubCuenta.Encabezado = "SubCuenta";
        ColSubCuenta.Buscador = "false";
        ColSubCuenta.Alineacion = "left";
        ColSubCuenta.Ancho = "70";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColSubCuenta);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaEditarDetalleFacturaProveedor";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        grdDetalleFacturaProveedorConsultar.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleFacturaProveedorEditar", grdDetalleFacturaProveedorConsultar.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAsientosContables(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdAsientoContable)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAsientosContables", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdAsientoContable", SqlDbType.VarChar, 10).Value = pIdAsientoContable;
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
        SqlCommand Stored = new SqlCommand("spg_grdDetalleFacturaProveedorEditar", sqlCon);
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
    #endregion

    #region Formas de asientos
    [WebMethod]
    public static string ObtenerFormaRevisarAsientosPendientes(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAsientoContable AsientoContable = new CAsientoContable();
            CTipoAsientoContable TipoAsientoContable = new CTipoAsientoContable();
            JObject ListaAsientosContablesPendientes = new JObject();
            ListaAsientosContablesPendientes = AsientoContable.ObtenerAsientosFacturaCliente(pTamanoPaginacion, pPaginaActual, pColumnaOrden, pTipoOrden, ConexionBaseDatos);
            Modelo.Add("Paginador", ListaAsientosContablesPendientes["Paginador"]);
            Modelo.Add("AsientosContablesPendientes", ListaAsientosContablesPendientes["AsientosContablesPendientes"]);
            Modelo.Add("TiposAsientosContables", TipoAsientoContable.ObtenerJsonTiposAsientosContables(ConexionBaseDatos));
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
    public static string ObtenerFormaAsientosFacturaProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAsientoContable AsientoContable = new CAsientoContable();
            CTipoAsientoContable TipoAsientoContable = new CTipoAsientoContable();
            JObject ListaAsientosContablesPendientes = new JObject();
            ListaAsientosContablesPendientes = AsientoContable.ObtenerAsientosFacturaProveedor(pTamanoPaginacion, pPaginaActual, pColumnaOrden, pTipoOrden, ConexionBaseDatos);
            Modelo.Add("Paginador", ListaAsientosContablesPendientes["Paginador"]);
            Modelo.Add("AsientosFacturasProveedores", ListaAsientosContablesPendientes["AsientosContablesPendientes"]);
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
    public static string ObtenerFormaAsientosCobroCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAsientoContable AsientoContable = new CAsientoContable();
            CTipoAsientoContable TipoAsientoContable = new CTipoAsientoContable();
            JObject ListaAsientosContablesPendientes = new JObject();
            ListaAsientosContablesPendientes = AsientoContable.ObtenerAsientosCobroCliente(pTamanoPaginacion, pPaginaActual, pColumnaOrden, pTipoOrden, ConexionBaseDatos);
            Modelo.Add("Paginador", ListaAsientosContablesPendientes["Paginador"]);
            Modelo.Add("AsientosCobroCliente", ListaAsientosContablesPendientes["AsientosContablesPendientes"]);
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
    public static string ObtenerFormaAsientosPagoProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAsientoContable AsientoContable = new CAsientoContable();
            CTipoAsientoContable TipoAsientoContable = new CTipoAsientoContable();
            JObject ListaAsientosContablesPendientes = new JObject();
            ListaAsientosContablesPendientes = AsientoContable.ObtenerAsientosPagoProveedor(pTamanoPaginacion, pPaginaActual, pColumnaOrden, pTipoOrden, ConexionBaseDatos);
            Modelo.Add("Paginador", ListaAsientosContablesPendientes["Paginador"]);
            Modelo.Add("AsientosPagoProveedor", ListaAsientosContablesPendientes["AsientosContablesPendientes"]);
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
    #endregion

    #region Formas editar cuentas de movimientos
    [WebMethod]
    public static string ObtenerFormaEditarCuentaMovimientosFacturaCliente(int pIdFacturaEncabezado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CFacturaEncabezado Factura = new CFacturaEncabezado();
            Factura.LlenaObjeto(pIdFacturaEncabezado, ConexionBaseDatos);

            CFacturaEncabezadoSucursal SucursalFacturo = new CFacturaEncabezadoSucursal();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdFacturaEncabezado", pIdFacturaEncabezado);
            SucursalFacturo.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

            Modelo.Add("IdFacturaEncabezado", pIdFacturaEncabezado);
            Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(SucursalFacturo.IdSucursal, ConexionBaseDatos));
            Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(Factura.IdDivision, ConexionBaseDatos));
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
    public static string ObtenerFormaEditarCuentaMovimientosFacturaProveedor(int pIdFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CEncabezadoFacturaProveedor Factura = new CEncabezadoFacturaProveedor();
            Factura.LlenaObjeto(pIdFacturaProveedor, ConexionBaseDatos);

            CEncabezadoFacturaProveedorSucursal SucursalRecibioFactura = new CEncabezadoFacturaProveedorSucursal();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdEncabezadoFacturaProveedor", pIdFacturaProveedor);
            SucursalRecibioFactura.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

            Modelo.Add("IdFacturaProveedor", pIdFacturaProveedor);
            Modelo.Add("Sucursales", CSucursal.ObtenerSucursales(SucursalRecibioFactura.IdSucursal, ConexionBaseDatos));
            Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(Factura.IdDivision, ConexionBaseDatos));
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
    public static string ObtenerEditarAsientoDetalleFacturaProveedor(int pIdDetalleFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("IdDetalleFacturaProveedor", pIdDetalleFacturaProveedor);
            CDetalleFacturaProveedor DetalleFactura = new CDetalleFacturaProveedor();
            DetalleFactura.LlenaObjeto(pIdDetalleFacturaProveedor, ConexionBaseDatos);

            CEncabezadoFacturaProveedorSucursal FacturaSucursal = new CEncabezadoFacturaProveedorSucursal();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdEncabezadoFacturaProveedor", DetalleFactura.IdEncabezadoFacturaProveedor);
            FacturaSucursal.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

            CEncabezadoFacturaProveedor FacturaProveedor = new CEncabezadoFacturaProveedor();
            FacturaProveedor.LlenaObjeto(DetalleFactura.IdEncabezadoFacturaProveedor, ConexionBaseDatos);

            Modelo.Add("TiposCompra", CTipoCompra.ObtenerJsonTiposCompra(DetalleFactura.IdTipoCompra, ConexionBaseDatos));
            CSelectEspecifico ObtenerCuentasContables = new CSelectEspecifico();
            ObtenerCuentasContables.StoredProcedure.CommandText = "sp_CuentaContable_Consultar_ObtenerCuentasContables";
            ObtenerCuentasContables.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(FacturaSucursal.IdSucursal));
            ObtenerCuentasContables.StoredProcedure.Parameters.AddWithValue("@pIdDivision", Convert.ToInt32(FacturaProveedor.IdDivision));
            ObtenerCuentasContables.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", Convert.ToInt32(DetalleFactura.IdTipoCompra));
            ObtenerCuentasContables.Llena(ConexionBaseDatos);

            JArray JACuentasContables = new JArray();
            while (ObtenerCuentasContables.Registros.Read())
            {
                JObject JCuentaContable = new JObject();
                JCuentaContable.Add("IdCuentaContable", Convert.ToInt32(ObtenerCuentasContables.Registros["IdCuentaContable"]));
                JCuentaContable.Add("CuentaContable", Convert.ToString(ObtenerCuentasContables.Registros["CuentaContable"]));
                JACuentasContables.Add(JCuentaContable);
            }
            Modelo.Add("CuentasContables", JACuentasContables);

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
    public static string ObtenerComboCuentasContables(int pIdDetalleFacturaProveedor, int pIdTipoCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("IdDetalleFacturaProveedor", pIdDetalleFacturaProveedor);
            CDetalleFacturaProveedor DetalleFactura = new CDetalleFacturaProveedor();
            DetalleFactura.LlenaObjeto(pIdDetalleFacturaProveedor, ConexionBaseDatos);

            CEncabezadoFacturaProveedorSucursal FacturaSucursal = new CEncabezadoFacturaProveedorSucursal();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdEncabezadoFacturaProveedor", DetalleFactura.IdEncabezadoFacturaProveedor);
            FacturaSucursal.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

            CEncabezadoFacturaProveedor FacturaProveedor = new CEncabezadoFacturaProveedor();
            FacturaProveedor.LlenaObjeto(DetalleFactura.IdEncabezadoFacturaProveedor, ConexionBaseDatos);

            CSelectEspecifico ObtenerCuentasContables = new CSelectEspecifico();
            ObtenerCuentasContables.StoredProcedure.CommandText = "sp_CuentaContable_Consultar_ObtenerCuentasContables";
            ObtenerCuentasContables.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(FacturaSucursal.IdSucursal));
            ObtenerCuentasContables.StoredProcedure.Parameters.AddWithValue("@pIdDivision", Convert.ToInt32(FacturaProveedor.IdDivision));
            ObtenerCuentasContables.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", Convert.ToInt32(DetalleFactura.IdTipoCompra));
            ObtenerCuentasContables.Llena(ConexionBaseDatos);

            JArray JACuentasContables = new JArray();
            while (ObtenerCuentasContables.Registros.Read())
            {
                JObject JCuentaContable = new JObject();
                JCuentaContable.Add("Valor", Convert.ToInt32(ObtenerCuentasContables.Registros["IdCuentaContable"]));
                JCuentaContable.Add("Descripcion", Convert.ToString(ObtenerCuentasContables.Registros["CuentaContable"]));
                JACuentasContables.Add(JCuentaContable);
            }
            Modelo.Add("Opciones", JACuentasContables);

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
    #endregion

    #region Editar cuenta de movimientos
    [WebMethod]
    public static string EditarCuentaMovimientosFacturaCliente(int pIdFacturaEncabezado, int pIdSucursal, int pIdDivision)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CFacturaEncabezado Factura = new CFacturaEncabezado();
            Factura.LlenaObjeto(pIdFacturaEncabezado, ConexionBaseDatos);
            Factura.IdDivision = pIdDivision;
            Factura.Editar(ConexionBaseDatos);

            CFacturaEncabezadoSucursal SucursalFacturo = new CFacturaEncabezadoSucursal();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdFacturaEncabezado", pIdFacturaEncabezado);
            SucursalFacturo.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
            SucursalFacturo.IdSucursal = pIdSucursal;
            SucursalFacturo.Editar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string EditarCuentaMovimientosDetalleFacturaProveedor(int pIdDetalleFacturaProveedor, int pIdTipoCompra, int pIdCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
            DetalleFacturaProveedor.LlenaObjeto(pIdDetalleFacturaProveedor, ConexionBaseDatos);
            DetalleFacturaProveedor.IdTipoCompra = pIdTipoCompra;
            DetalleFacturaProveedor.IdSubCuentaContable = pIdCuentaContable;
            DetalleFacturaProveedor.Editar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatos();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string EditarCuentaMovimientosFacturaProveedor(int pIdFacturaProveedor, int pIdDivision)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoFacturaProveedor Factura = new CEncabezadoFacturaProveedor();
            Factura.LlenaObjeto(pIdFacturaProveedor, ConexionBaseDatos);
            Factura.IdDivision = pIdDivision;
            Factura.Editar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatos();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
            return oRespuesta.ToString();
        }
    }
    #endregion

    #region Agregar asientos
    [WebMethod]
    public static string AgregarAsientoFacturaCliente(int pIdFacturaCliente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string validarAsientoFacturaCliente = "";
            validarAsientoFacturaCliente = ValidarAsientoFacturaCliente(pIdFacturaCliente, ConexionBaseDatos);

            if (validarAsientoFacturaCliente != "")
            {
                oRespuesta.Add("Error", "1");
                oRespuesta.Add("Descripcion", validarAsientoFacturaCliente);
            }
            else
            {
                JObject ValidarImportacion = new JObject();
                ValidarImportacion = ImportarPolizaFacturaCliente(pIdFacturaCliente);

                if (ValidarImportacion["Error"].ToString() == "1")
                {
                    oRespuesta = ValidarImportacion;
                }
                else
                {
                    CFacturaEncabezado Factura = new CFacturaEncabezado();
                    Factura.LlenaObjeto(pIdFacturaCliente, ConexionBaseDatos);

                    Dictionary<string, object> pFacturaSucursal = new Dictionary<string, object>();
                    pFacturaSucursal.Add("IdFacturaEncabezado", pIdFacturaCliente);
                    CFacturaEncabezadoSucursal FacturaSucursal = new CFacturaEncabezadoSucursal();
                    FacturaSucursal.LlenaObjetoFiltros(pFacturaSucursal, ConexionBaseDatos);

                    CSucursal Sucursal = new CSucursal();
                    Sucursal.LlenaObjeto(FacturaSucursal.IdSucursal, ConexionBaseDatos);

                    CDivision Division = new CDivision();
                    Division.LlenaObjeto(Factura.IdDivision, ConexionBaseDatos);

                    CCliente Cliente = new CCliente();
                    Cliente.LlenaObjeto(Factura.IdCliente, ConexionBaseDatos);

                    COrganizacion Organizacion = new COrganizacion();
                    Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

                    CIVA IVA = new CIVA();
                    IVA.LlenaObjeto(Sucursal.IdIVA, ConexionBaseDatos);

                    CAsientoContable AsientoContable = new CAsientoContable();
                    AsientoContable.FechaAlta = DateTime.Now;
                    AsientoContable.FechaAutorizo = DateTime.Now;
                    AsientoContable.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdUsuarioAutorizo = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdDocumento = pIdFacturaCliente;
                    AsientoContable.IdTipoAsientoContable = 1;
                    AsientoContable.IdClaseGenerador = 118;

                    if (Factura.IdTipoMoneda != 1)
                    {
                        AsientoContable.Total = Factura.Total;
                    }
                    else
                    {
                        AsientoContable.Total = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) - Factura.Total) + Factura.Total;
                    }

                    AsientoContable.Agregar(ConexionBaseDatos);
                    CAsientoContableDetalle AsientoContableDetalle = new CAsientoContableDetalle();

                    //--Cargos
                    AsientoContableDetalle.Cargo = Factura.Total;
                    AsientoContableDetalle.IdCliente = Factura.IdCliente;
                    AsientoContableDetalle.CuentaContable = Cliente.CuentaContable;
                    AsientoContableDetalle.DescripcionCuentaContable = Organizacion.RazonSocial;
                    AsientoContableDetalle.Baja = false;
                    AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                    AsientoContableDetalle.Agregar(ConexionBaseDatos);

                    if (Factura.IdTipoMoneda != 1)
                    {
                        CCuentaContable CuentaContable = new CCuentaContable();
                        Dictionary<string, object> Parametros = new Dictionary<string, object>();
                        Parametros.Add("IdTipoCuentaContable", 2);
                        CuentaContable.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

                        AsientoContableDetalle = new CAsientoContableDetalle();
                        AsientoContableDetalle.Cargo = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) - Factura.Total);
                        AsientoContableDetalle.CuentaContable = CuentaContable.CuentaContable;
                        AsientoContableDetalle.DescripcionCuentaContable = CuentaContable.Descripcion;
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);
                    }

                    //--Abonos
                    AsientoContableDetalle = new CAsientoContableDetalle();
                    if (Factura.IdTipoMoneda != 1)
                    {
                        AsientoContableDetalle.Abono = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) / Convert.ToDecimal(1.16));
                    }
                    else
                    {
                        AsientoContableDetalle.Abono = Factura.Subtotal;
                    }
                    AsientoContableDetalle.IdSucursal = Sucursal.IdSucursal;
                    AsientoContableDetalle.IdDivision = Division.IdDivision;
                    AsientoContableDetalle.CuentaContable = Sucursal.ClaveCuentaContable + "-" + Division.ClaveCuentaContable + "-000-000";
                    AsientoContableDetalle.DescripcionCuentaContable = Sucursal.Sucursal + "-" + Division.Division;
                    AsientoContableDetalle.Baja = false;
                    AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                    AsientoContableDetalle.Agregar(ConexionBaseDatos);

                    AsientoContableDetalle = new CAsientoContableDetalle();
                    if (Factura.IdTipoMoneda != 1)
                    {
                        AsientoContableDetalle.Abono = Convert.ToDecimal(((Factura.Total * Factura.TipoCambio) / Convert.ToDecimal(1.16)) * Convert.ToDecimal(.16));
                    }
                    else
                    {
                        AsientoContableDetalle.Abono = Factura.IVA;
                    }
                    AsientoContableDetalle.IdIVA = IVA.IdIVA;
                    AsientoContableDetalle.CuentaContable = IVA.ClaveCuentaContable;
                    AsientoContableDetalle.DescripcionCuentaContable = IVA.DescripcionIVA + " ACREDITABLE";
                    AsientoContableDetalle.Baja = false;
                    AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                    AsientoContableDetalle.Agregar(ConexionBaseDatos);

                    Factura.SeGeneroAsiento = true;
                    Factura.Editar(ConexionBaseDatos);
                    oRespuesta.Add(new JProperty("Error", 0));
                    ConexionBaseDatos.CerrarBaseDatosSqlServer();
                }
            }
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string AgregarAsientoFacturaProveedor(int pIdFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string validarAsientoFacturaProveedor = ValidarAsientoFacturaProveedor(pIdFacturaProveedor, ConexionBaseDatos);
            JObject ValidarImportacion = new JObject();
            ValidarImportacion = ImportarPolizaFacturaProveedor(pIdFacturaProveedor);

            if (validarAsientoFacturaProveedor != "")
            {
                oRespuesta.Add("Error", "1");
                oRespuesta.Add("Descripcion", validarAsientoFacturaProveedor);
            }
            else
            {
                if (ValidarImportacion["Error"].ToString() == "1")
                {
                    oRespuesta = ValidarImportacion;
                }
                else
                {
                    JObject JAsientoContable = new JObject();
                    JAsientoContable = CAsientoContable.ObtenerJsonAsientoGeneradoFacturaProveedor(pIdFacturaProveedor, ConexionBaseDatos);

                    CAsientoContable AsientoContable = new CAsientoContable();
                    AsientoContable.FechaAlta = DateTime.Now;
                    AsientoContable.FechaAutorizo = DateTime.Now;
                    AsientoContable.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdUsuarioAutorizo = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdDocumento = pIdFacturaProveedor;
                    AsientoContable.IdTipoAsientoContable = 2;
                    AsientoContable.IdClaseGenerador = 82;
                    AsientoContable.Total = Convert.ToDecimal(JAsientoContable["TotalConversion"].ToString());
                    AsientoContable.Agregar(ConexionBaseDatos);
                    CAsientoContableDetalle AsientoContableDetalle = new CAsientoContableDetalle();

                    if (Convert.ToDecimal(JAsientoContable["TotalProveedorComplemento"].ToString()) == 0)
                    {
                        //--Cargo
                        AsientoContableDetalle = new CAsientoContableDetalle();
                        AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContable["IVA"].ToString());
                        AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContable["IdIVA"].ToString());
                        AsientoContableDetalle.CuentaContable = JAsientoContable["CuentaContableTrasladado"].ToString();
                        AsientoContableDetalle.DescripcionCuentaContable = JAsientoContable["DescripcionIVA"].ToString() + " trasladado";
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);

                        AsientoContableDetalle.AgregarAsientosDetalleFacturaProveedor(pIdFacturaProveedor, AsientoContable.IdAsientoContable, ConexionBaseDatos);

                        //--Abono
                        AsientoContableDetalle = new CAsientoContableDetalle();
                        AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContable["Total"].ToString());
                        AsientoContableDetalle.IdProveedor = Convert.ToInt32(JAsientoContable["IdProveedor"].ToString());
                        AsientoContableDetalle.CuentaContable = JAsientoContable["CuentaContableProveedor"].ToString();
                        AsientoContableDetalle.DescripcionCuentaContable = JAsientoContable["RazonSocial"].ToString();
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);

                        CEncabezadoFacturaProveedor Factura = new CEncabezadoFacturaProveedor();
                        Factura.LlenaObjeto(Convert.ToInt32(JAsientoContable["IdFactura"].ToString()), ConexionBaseDatos);
                        Factura.SeGeneroAsiento = true;
                        Factura.Editar(ConexionBaseDatos);
                    }
                    if (Convert.ToDecimal(JAsientoContable["TotalProveedorComplemento"].ToString()) > 0)
                    {
                        //--Cargo
                        AsientoContableDetalle = new CAsientoContableDetalle();
                        AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContable["IVA"].ToString());
                        AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContable["IdIVA"].ToString());
                        AsientoContableDetalle.CuentaContable = JAsientoContable["CuentaContableTrasladado"].ToString();
                        AsientoContableDetalle.DescripcionCuentaContable = JAsientoContable["DescripcionIVA"].ToString() + " trasladado";
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);

                        foreach (JObject JCuentaMovimiento in JAsientoContable["CuentasContablesMovimientos"])
                        {
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JCuentaMovimiento["TotalTipoCompra"].ToString());
                            AsientoContableDetalle.IdSucursal = Convert.ToInt32(JCuentaMovimiento["IdSucursal"].ToString());
                            AsientoContableDetalle.IdDivision = Convert.ToInt32(JCuentaMovimiento["IdDivision"].ToString());
                            AsientoContableDetalle.IdTipoCompra = Convert.ToInt32(JCuentaMovimiento["IdTipoCompra"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JCuentaMovimiento["IdCuentaContable"].ToString());
                            AsientoContableDetalle.CuentaContable = JCuentaMovimiento["CuentaContableMovimientos"].ToString();
                            AsientoContableDetalle.DescripcionCuentaContable = JCuentaMovimiento["CuentaMovimientos"].ToString();
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);
                        }
                        AsientoContableDetalle = new CAsientoContableDetalle();
                        //AsientoContableDetalle.AgregarAsientosDetalleFacturaProveedor(pIdFacturaProveedor, AsientoContable.IdAsientoContable, ConexionBaseDatos);

                        //--Abono
                        AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContable["Total"].ToString());
                        AsientoContableDetalle.IdProveedor = Convert.ToInt32(JAsientoContable["IdProveedor"].ToString());
                        AsientoContableDetalle.CuentaContable = JAsientoContable["CuentaContableProveedor"].ToString();
                        AsientoContableDetalle.DescripcionCuentaContable = JAsientoContable["RazonSocial"].ToString();
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);

                        //--Abono
                        AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContable["TotalProveedorComplemento"].ToString());
                        AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContable["IdCuentaProveedorComplemento"].ToString());
                        AsientoContableDetalle.CuentaContable = JAsientoContable["CuentaProveedorComplemento"].ToString();
                        AsientoContableDetalle.DescripcionCuentaContable = JAsientoContable["CuentaProveedorComplementoDesc"].ToString();
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);

                        CEncabezadoFacturaProveedor Factura = new CEncabezadoFacturaProveedor();
                        Factura.LlenaObjeto(Convert.ToInt32(JAsientoContable["IdFactura"].ToString()), ConexionBaseDatos);
                        Factura.SeGeneroAsiento = true;
                        Factura.Editar(ConexionBaseDatos);
                    }
                    oRespuesta.Add(new JProperty("Error", 0));
                }
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string AgregarAsientoCobroCliente(int pIdCobroCliente, int pTipoEntrada, int pIdFacturaEncabezado)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject ValidarImportacion = new JObject();
            ValidarImportacion = ImportarPolizaCobroCliente(pIdCobroCliente, pTipoEntrada, pIdFacturaEncabezado);
            string validarAsientoCobroCliente = ValidarAsientoCobroCliente(pIdCobroCliente, pTipoEntrada, pIdFacturaEncabezado, ConexionBaseDatos);
            if (validarAsientoCobroCliente != "")
            {
                oRespuesta.Add("Error", "1");
                oRespuesta.Add("Descripcion", validarAsientoCobroCliente);
            }
            else
            {
                if (ValidarImportacion["Error"].ToString() == "1")
                {
                    oRespuesta = ValidarImportacion;
                }
                else
                {
                    int idIngreso = 0;
                    int idClaseGenerador = 0;
                    decimal total = 0;
                    int idCuentaBancaria = 0;
                    int idCliente = 0;

                    CCuentasPorCobrar Ingreso = new CCuentasPorCobrar();
                    CDepositos Deposito = new CDepositos();
                    CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
                    if (pTipoEntrada == 1)
                    {
                        Ingreso.LlenaObjeto(pIdCobroCliente, ConexionBaseDatos);
                        idIngreso = Ingreso.IdCuentasPorCobrar;
                        idClaseGenerador = 71;
                        total = Ingreso.Importe;
                        idCuentaBancaria = Ingreso.IdCuentaBancaria;
                        idCliente = Ingreso.IdCliente;
                        Ingreso.SeGeneroAsiento = true;
                    }
                    else
                    {
                        Deposito.LlenaObjeto(pIdCobroCliente, ConexionBaseDatos);
                        idIngreso = Deposito.IdDepositos;
                        idClaseGenerador = 90;
                        total = Deposito.Importe;
                        idCuentaBancaria = Deposito.IdCuentaBancaria;

                        Dictionary<string, object> Parametros = new Dictionary<string, object>();
                        Parametros.Add("IdDeposito", Deposito.IdDepositos);
                        IngresosNoDepositados.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                        idCliente = IngresosNoDepositados.IdCliente;
                        Deposito.SeGeneroAsiento = true;
                    }

                    JObject JAsientoContableGenerado = new JObject();
                    JAsientoContableGenerado = CAsientoContable.ObtenerJsonAsientoGeneradoCobroCliente(pIdCobroCliente, pTipoEntrada, pIdFacturaEncabezado, ConexionBaseDatos);

                    CAsientoContable AsientoContable = new CAsientoContable();
                    AsientoContable.FechaAlta = DateTime.Now;
                    AsientoContable.FechaAutorizo = DateTime.Now;
                    AsientoContable.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdUsuarioAutorizo = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdDocumento = idIngreso;
                    AsientoContable.IdTipoAsientoContable = 3;
                    AsientoContable.IdClaseGenerador = idClaseGenerador;
                    AsientoContable.Total = Convert.ToDecimal(JAsientoContableGenerado["TotalCargo"].ToString());
                    AsientoContable.Agregar(ConexionBaseDatos);
                    CAsientoContableDetalle AsientoContableDetalle = new CAsientoContableDetalle();

                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosAPesos" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosADolares")
                    {
                        //--Cargo
                        AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                        AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                        AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"].ToString());
                        AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"].ToString());
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);

                        //--Abonos
                        AsientoContableDetalle = new CAsientoContableDetalle();
                        AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                        AsientoContableDetalle.IdCliente = Convert.ToInt32(JAsientoContableGenerado["IdCliente"].ToString());
                        AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableCliente"].ToString());
                        AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["RazonSocial"].ToString());
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);
                    }

                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresADolares" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresAPesos")
                    {
                        if (JAsientoContableGenerado["TipoGananciaPerdida"].ToString() == "Perdida")
                        {
                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]) + " - Complemento";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abonos
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdCliente = Convert.ToInt32(JAsientoContableGenerado["IdCliente"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["RazonSocial"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableCliente"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["ClienteComplemento"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableCliente"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableGananciaPerdida"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdidaDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPendienteGananciaPerdida"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA trasladado pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPagado"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA trasladado pagado";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA trasladado pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);
                        }
                        else
                        {
                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]) + " - Complemento";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abonos
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdCliente = Convert.ToInt32(JAsientoContableGenerado["IdCliente"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["RazonSocial"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abonos
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableCliente"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableGananciaPerdida"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdidaDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPendienteGananciaPerdida"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA trasladado pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["ClienteComplemento"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableCliente"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPagado"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA trasladado pagado";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA trasladado pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);
                        }
                    }

                    if (pTipoEntrada == 1)
                    {
                        Ingreso.Editar(ConexionBaseDatos);
                    }
                    else
                    {
                        Deposito.Editar(ConexionBaseDatos);
                    }
                    oRespuesta.Add(new JProperty("Error", 0));
                }
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
    public static string AgregarAsientoPagoProveedor(int pIdPagoProveedor, int pTipoSalida, int pIdEncabezadoFactura)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject ValidarImportacion = new JObject();
            ValidarImportacion = ImportarPolizaPagoProveedor(pIdPagoProveedor, pTipoSalida, pIdEncabezadoFactura);
            string validarAsientoCobroCliente = ValidarAsientoPagoProveedor(pIdPagoProveedor, pTipoSalida, pIdEncabezadoFactura, ConexionBaseDatos);
            if (validarAsientoCobroCliente != "")
            {
                oRespuesta.Add("Error", "1");
                oRespuesta.Add("Descripcion", validarAsientoCobroCliente);
            }
            else
            {
                if (ValidarImportacion["Error"].ToString() == "1")
                {
                    oRespuesta = ValidarImportacion;
                }
                else
                {
                    int idEgreso = 0;
                    int idClaseGenerador = 0;
                    decimal total = 0;
                    int idCuentaBancaria = 0;
                    int idProveedor = 0;

                    CEgresos Egreso = new CEgresos();
                    CCheques Cheques = new CCheques();
                    if (pTipoSalida == 1)
                    {
                        Egreso.LlenaObjeto(pIdPagoProveedor, ConexionBaseDatos);
                        idEgreso = Egreso.IdEgresos;
                        idClaseGenerador = 79;
                        total = Egreso.Importe;
                        idCuentaBancaria = Egreso.IdCuentaBancaria;
                        idProveedor = Egreso.IdEgresos;
                        Egreso.SeGeneroAsiento = true;
                    }
                    else
                    {
                        Cheques.LlenaObjeto(pIdPagoProveedor, ConexionBaseDatos);
                        idEgreso = Cheques.IdCheques;
                        idClaseGenerador = 93;
                        total = Cheques.Importe;
                        idCuentaBancaria = Cheques.IdCuentaBancaria;
                        idProveedor = Cheques.IdProveedor;
                        Cheques.SeGeneroAsiento = true;
                    }

                    JObject JAsientoContableGenerado = new JObject();
                    JAsientoContableGenerado = CAsientoContable.ObtenerJsonAsientoGeneradoPagoProveedor(pIdPagoProveedor, pTipoSalida, pIdEncabezadoFactura, ConexionBaseDatos);

                    CAsientoContable AsientoContable = new CAsientoContable();
                    AsientoContable.FechaAlta = DateTime.Now;
                    AsientoContable.FechaAutorizo = DateTime.Now;
                    AsientoContable.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdUsuarioAutorizo = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    AsientoContable.IdDocumento = idEgreso;
                    AsientoContable.IdTipoAsientoContable = 4;
                    AsientoContable.IdClaseGenerador = idClaseGenerador;
                    AsientoContable.Total = Convert.ToDecimal(JAsientoContableGenerado["TotalCargo"].ToString());
                    AsientoContable.Agregar(ConexionBaseDatos);
                    CAsientoContableDetalle AsientoContableDetalle = new CAsientoContableDetalle();

                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosAPesos" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosADolares")
                    {
                        //--Cargo
                        AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                        AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                        AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"].ToString());
                        AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"].ToString());
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);

                        //--Abonos
                        AsientoContableDetalle = new CAsientoContableDetalle();
                        AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                        AsientoContableDetalle.IdCliente = Convert.ToInt32(JAsientoContableGenerado["IdProveedor"].ToString());
                        AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedor"].ToString());
                        AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["RazonSocial"].ToString());
                        AsientoContableDetalle.Baja = false;
                        AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                        AsientoContableDetalle.Agregar(ConexionBaseDatos);
                    }

                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresADolares" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresAPesos")
                    {
                        if (JAsientoContableGenerado["TipoGananciaPerdida"].ToString() == "Perdida")
                        {
                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdProveedor = Convert.ToInt32(JAsientoContableGenerado["IdProveedor"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["RazonSocial"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableProveedor"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]) + " - Complemento";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableGananciaPerdida"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdidaDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePendienteGananciaPerdida"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA acreditable pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["ProveedorComplemento"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableProveedor"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePagado"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA acreditable pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePagado"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePagado"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA acreditable pagado";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);
                        }
                        else
                        {
                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdProveedor = Convert.ToInt32(JAsientoContableGenerado["IdProveedor"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["RazonSocial"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableProveedor"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());
                            AsientoContableDetalle.IdCuentaBancaria = Convert.ToInt32(JAsientoContableGenerado["IdCuentaBancaria"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaBancaria"]) + " - Complemento";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["ProveedorComplemento"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableProveedor"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplementoDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdCuentaContableGananciaPerdida"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]);
                            AsientoContableDetalle.DescripcionCuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdidaDesc"]);
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePendienteGananciaPerdida"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA acreditable pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Cargo
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Cargo = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePagado"].ToString());
                            AsientoContableDetalle.IdCuentaContable = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA acreditable pendiente";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);

                            //--Abono
                            AsientoContableDetalle = new CAsientoContableDetalle();
                            AsientoContableDetalle.Abono = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePagado"].ToString());
                            AsientoContableDetalle.IdIVA = Convert.ToInt32(JAsientoContableGenerado["IdIVA"].ToString());
                            AsientoContableDetalle.CuentaContable = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePagado"]);
                            AsientoContableDetalle.DescripcionCuentaContable = "IVA acreditable pagado";
                            AsientoContableDetalle.Baja = false;
                            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
                            AsientoContableDetalle.Agregar(ConexionBaseDatos);
                        }
                    }

                    if (pTipoSalida == 1)
                    {
                        Egreso.Editar(ConexionBaseDatos);
                    }
                    else
                    {
                        Cheques.Editar(ConexionBaseDatos);
                    }
                    oRespuesta.Add(new JProperty("Error", 0));
                }
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
    #endregion

    #region Anteriores
    //[WebMethod]
    //public static string AgregarAsientoCobroCliente(int pIdCobroCliente, int pTipoEntrada)
    //{
    //    //Abrir Conexion
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
    //    JObject oRespuesta = new JObject();
    //    //¿La conexion se establecio?
    //    if (respuesta == "Conexion Establecida")
    //    {
    //        JObject ValidarImportacion = new JObject();
    //        ValidarImportacion = ImportarPolizaCobroCliente(pIdCobroCliente, pTipoEntrada);

    //        if (ValidarImportacion["Error"].ToString() == "1")
    //        {
    //            oRespuesta = ValidarImportacion;
    //        }
    //        else
    //        {
    //            int idIngreso = 0;
    //            int idClaseGenerador = 0;
    //            decimal total = 0;
    //            int idCuentaBancaria = 0;
    //            int idCliente = 0;
    //            if (pTipoEntrada == 1)
    //            {
    //                CCuentasPorCobrar Ingreso = new CCuentasPorCobrar();
    //                Ingreso.LlenaObjeto(pIdCobroCliente, ConexionBaseDatos);
    //                idIngreso = Ingreso.IdCuentasPorCobrar;
    //                idClaseGenerador = 71;
    //                total = Ingreso.Importe;
    //                idCuentaBancaria = Ingreso.IdCuentaBancaria;
    //                idCliente = Ingreso.IdCliente;
    //                Ingreso.SeGeneroAsiento = true;
    //                Ingreso.Editar(ConexionBaseDatos);
    //            }
    //            else
    //            {
    //                CDepositos Deposito = new CDepositos();
    //                Deposito.LlenaObjeto(pIdCobroCliente, ConexionBaseDatos);
    //                idIngreso = Deposito.IdDepositos;
    //                idClaseGenerador = 90;
    //                total = Deposito.Importe;
    //                idCuentaBancaria = Deposito.IdCuentaBancaria;

    //                CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
    //                Dictionary<string, object> Parametros = new Dictionary<string, object>();
    //                Parametros.Add("IdDeposito", Deposito.IdDepositos);
    //                IngresosNoDepositados.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
    //                idCliente = IngresosNoDepositados.IdCliente;
    //                Deposito.SeGeneroAsiento = true;
    //                Deposito.Editar(ConexionBaseDatos);
    //            }

    //            CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
    //            CuentaBancaria.LlenaObjeto(idCuentaBancaria, ConexionBaseDatos);

    //            CCliente Cliente = new CCliente();
    //            Cliente.LlenaObjeto(idCliente, ConexionBaseDatos);

    //            COrganizacion Organizacion = new COrganizacion();
    //            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

    //            CAsientoContable AsientoContable = new CAsientoContable();
    //            AsientoContable.FechaAlta = DateTime.Now;
    //            AsientoContable.FechaAutorizo = DateTime.Now;
    //            AsientoContable.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
    //            AsientoContable.IdUsuarioAutorizo = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
    //            AsientoContable.IdDocumento = idIngreso;
    //            AsientoContable.IdTipoAsientoContable = 3;
    //            AsientoContable.IdClaseGenerador = idClaseGenerador;
    //            AsientoContable.Total = total;
    //            AsientoContable.Agregar(ConexionBaseDatos);
    //            CAsientoContableDetalle AsientoContableDetalle = new CAsientoContableDetalle();

    //            //--Cargos
    //            AsientoContableDetalle.Cargo = total;
    //            AsientoContableDetalle.IdCuentaBancaria = idCuentaBancaria;
    //            AsientoContableDetalle.CuentaContable = CuentaBancaria.CuentaContable;
    //            AsientoContableDetalle.DescripcionCuentaContable = CuentaBancaria.Descripcion;
    //            AsientoContableDetalle.Baja = false;
    //            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
    //            AsientoContableDetalle.Agregar(ConexionBaseDatos);

    //            //--Abonos
    //            AsientoContableDetalle = new CAsientoContableDetalle();
    //            AsientoContableDetalle.Abono = total;
    //            AsientoContableDetalle.IdCliente = Cliente.IdCliente;
    //            AsientoContableDetalle.CuentaContable = Cliente.CuentaContable;
    //            AsientoContableDetalle.DescripcionCuentaContable = Organizacion.RazonSocial;
    //            AsientoContableDetalle.Baja = false;
    //            AsientoContableDetalle.IdAsientoContable = AsientoContable.IdAsientoContable;
    //            AsientoContableDetalle.Agregar(ConexionBaseDatos);

    //            oRespuesta.Add(new JProperty("Error", 0));
    //        }
    //        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //        return oRespuesta.ToString();
    //    }
    //    else
    //    {
    //        oRespuesta.Add(new JProperty("Error", 1));
    //        oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
    //        return oRespuesta.ToString();
    //    }
    //}
    #endregion

    #region Importar asientos
    [WebMethod]
    public static JObject ImportarPolizaFacturaCliente(int pIdFacturaCliente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CFacturaEncabezado Factura = new CFacturaEncabezado();
            Factura.LlenaObjeto(pIdFacturaCliente, ConexionBaseDatos);

            Dictionary<string, object> pFacturaSucursal = new Dictionary<string, object>();
            pFacturaSucursal.Add("IdFacturaEncabezado", pIdFacturaCliente);
            CFacturaEncabezadoSucursal FacturaSucursal = new CFacturaEncabezadoSucursal();
            FacturaSucursal.LlenaObjetoFiltros(pFacturaSucursal, ConexionBaseDatos);

            CSerieFactura SerieFactura = new CSerieFactura();
            SerieFactura.LlenaObjeto(Factura.IdSerieFactura, ConexionBaseDatos);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(FacturaSucursal.IdSucursal, ConexionBaseDatos);

            CDivision Division = new CDivision();
            Division.LlenaObjeto(Factura.IdDivision, ConexionBaseDatos);

            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(Factura.IdCliente, ConexionBaseDatos);

            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

            CIVA IVA = new CIVA();
            IVA.LlenaObjeto(Sucursal.IdIVA, ConexionBaseDatos);

            CUsuario UsuarioSesion = new CUsuario();
            UsuarioSesion.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CSucursal SucursalSesion = new CSucursal();
            SucursalSesion.LlenaObjeto(UsuarioSesion.IdSucursalActual, ConexionBaseDatos);

            int idEmpresa;
            SDKCONTPAQNGLib.TSdkPoliza Poliza = new SDKCONTPAQNGLib.TSdkPoliza();
            SDKCONTPAQNGLib.TSdkTipoPoliza TipoPoliza = new SDKCONTPAQNGLib.TSdkTipoPoliza();
            SDKCONTPAQNGLib.TSdkSesion Sesion = new SDKCONTPAQNGLib.TSdkSesion();
            SDKCONTPAQNGLib.TSdkMovimientoPoliza MovimientosPoliza = new SDKCONTPAQNGLib.TSdkMovimientoPoliza();
            SDKCONTPAQNGLib.TSdkCuenta Cuenta = new SDKCONTPAQNGLib.TSdkCuenta();
            SDKCONTPAQNGLib.TSdkEmpresa Empresa = new SDKCONTPAQNGLib.TSdkEmpresa();
            SDKCONTPAQNGLib.TSdkControlIVA ControlIVA = new SDKCONTPAQNGLib.TSdkControlIVA();

            Sesion.finalizaConexion();
            Sesion.iniciaConexion();
            int conexionActiva = Sesion.conexionActiva;

            if (SucursalSesion.BaseDatosContpaq != "" && SucursalSesion.UsuarioContpaq != "" && SucursalSesion.ContrasenaContpaq != "")
            {
                if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 0)
                {
                    Sesion.firmaUsuarioParams(Sucursal.UsuarioContpaq, Sucursal.ContrasenaContpaq);
                }
                int abrioEmpresa = 0;
                if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 1)
                {
                    abrioEmpresa = Sesion.abreEmpresa(Sucursal.BaseDatosContpaq);
                }

                if (abrioEmpresa == 1)
                {
                    Empresa.setSesion(Sesion);
                    Poliza.setSesion(Sesion);
                    Cuenta.setSesion(Sesion);
                    MovimientosPoliza.setSesion(Sesion);
                    TipoPoliza.setSesion(Sesion);

                    try
                    {
                        idEmpresa = Empresa.IdEmpresa;
                        Poliza.iniciarInfo();
                        TipoPoliza.iniciarInfo();
                        TipoPoliza.Tipo = SDKCONTPAQNGLib.ETIPOPOLIZA.TIPO_DIARIO;

                        Poliza.setUsuario = 1;
                        Poliza.Fecha = DateTime.Now;
                        Poliza.Tipo = TipoPoliza.Tipo;
                        Poliza.Clase = SDKCONTPAQNGLib.ECLASEPOLIZA.CLASE_AFECTAR;
                        Poliza.Impresa = 0;
                        Poliza.Concepto = "FACTURA DE  " + Organizacion.RazonSocial + ": " + SerieFactura.SerieFactura + "-" + Factura.NumeroFactura;
                        Poliza.Ajuste = 0;
                        Poliza.setDiario = 0;
                        Poliza.CodigoDiario = "";
                        Poliza.SistOrigen = SDKCONTPAQNGLib.ESISTORIGEN.ORIG_CONTPAQNG;

                        int noMovimiento = 0;
                        noMovimiento = noMovimiento + 1;

                        decimal totalCargo = Convert.ToDecimal(Factura.Total) + (Convert.ToDecimal((Factura.Total * Factura.TipoCambio) - Factura.Total));
                        decimal totalAbono = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) / Convert.ToDecimal(1.16)) + Convert.ToDecimal(((Factura.Total * Factura.TipoCambio) / Convert.ToDecimal(1.16)) * Convert.ToDecimal(.16));
                        decimal ajustador;

                        //Cargo
                        MovimientosPoliza.setSesion(Sesion);
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = noMovimiento;
                        if (Factura.IdTipoMoneda != 1)
                        {
                            MovimientosPoliza.CodigoCuenta = Cliente.CuentaContableDolares.Replace("-", "");
                        }
                        else
                        {
                            MovimientosPoliza.CodigoCuenta = Cliente.CuentaContable.Replace("-", "");
                        }
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(Factura.Total);//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE  " + Organizacion.RazonSocial + ": " + SerieFactura.SerieFactura + "-" + Factura.NumeroFactura;

                        int movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        if (Factura.IdTipoMoneda != 1)
                        {
                            CCuentaContable CuentaContable = new CCuentaContable();
                            Dictionary<string, object> Parametros = new Dictionary<string, object>();
                            Parametros.Add("IdTipoCuentaContable", 2);
                            CuentaContable.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

                            noMovimiento = noMovimiento + 1;
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = noMovimiento;
                            MovimientosPoliza.CodigoCuenta = CuentaContable.CuentaContable.Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            if (totalCargo < totalAbono)
                            {
                                ajustador = totalAbono - totalCargo;
                                MovimientosPoliza.Importe = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) - Factura.Total) + ajustador; //Importe en moneda base de la cuenta
                            }
                            else
                            {
                                MovimientosPoliza.Importe = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) - Factura.Total); //Importe en moneda base de la cuenta
                            }

                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "FACTURA DE  " + Organizacion.RazonSocial + ": " + SerieFactura.SerieFactura + "-" + Factura.NumeroFactura;

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }
                        }

                        //Abono
                        noMovimiento = noMovimiento + 1;
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = noMovimiento;
                        MovimientosPoliza.CodigoCuenta = Sucursal.ClaveCuentaContable + Division.ClaveCuentaContable + "000000";
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                        if (Factura.IdTipoMoneda != 1)
                        {
                            if (totalCargo > totalAbono)
                            {
                                ajustador = totalCargo - totalAbono;
                                MovimientosPoliza.Importe = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) / Convert.ToDecimal(1.16)) + ajustador; //Importe en moneda base de la cuenta
                            }
                            else
                            {
                                MovimientosPoliza.Importe = Convert.ToDecimal((Factura.Total * Factura.TipoCambio) / Convert.ToDecimal(1.16)); //Importe en moneda base de la cuenta
                            }
                        }
                        else
                        {
                            MovimientosPoliza.Importe = Convert.ToDecimal(Factura.Subtotal);//Importe en moneda base de la cuenta
                        }
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE  " + Organizacion.RazonSocial + ": " + SerieFactura.SerieFactura + "-" + Factura.NumeroFactura;

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        noMovimiento = noMovimiento + 1;
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = noMovimiento;
                        MovimientosPoliza.CodigoCuenta = IVA.ClaveCuentaContable.Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                        if (Factura.IdTipoMoneda != 1)
                        {
                            MovimientosPoliza.Importe = Convert.ToDecimal(((Factura.Total * Factura.TipoCambio) / Convert.ToDecimal(1.16)) * Convert.ToDecimal(.16));
                        }
                        else
                        {
                            MovimientosPoliza.Importe = Factura.IVA;
                        }
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE  " + Organizacion.RazonSocial + ": " + SerieFactura.SerieFactura + "-" + Factura.NumeroFactura;

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        int grabada = Poliza.crea();
                        if (grabada == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo grabar " + Poliza.getMensajeError()));
                        }
                        else
                        {
                            oRespuesta.Add(new JProperty("Error", 0));
                            oRespuesta.Add(new JProperty("Descripcion", "Poliza grabada con exito"));
                        }
                    }
                    catch (Exception a)
                    {
                        oRespuesta.Add(new JProperty("Error", 0));
                        oRespuesta.Add(new JProperty("Descripcion", a.Message + " " + Poliza.UltimoMsjError));
                    }
                    finally
                    {
                        Sesion.cierraEmpresa();
                        Sesion.finalizaConexion();
                    }
                }
                else
                {
                    oRespuesta.Add("Error", 1);
                    oRespuesta.Add("Descripcion", "No se pudo abrir la empresa en Contpaq");
                }
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No estan completos los accesos de la conexión con CONTPAQ."));
            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta;
    }

    [WebMethod]
    public static JObject ImportarPolizaCobroCliente(int pIdCobroCliente, int pTipoEntrada, int pIdFacturaCliente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        CUsuario UsuarioSesion = new CUsuario();
        UsuarioSesion.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal SucursalSesion = new CSucursal();
        SucursalSesion.LlenaObjeto(UsuarioSesion.IdSucursalActual, ConexionBaseDatos);

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            int idIngreso = 0;
            int idClaseGenerador = 0;
            decimal total = 0;
            int idCuentaBancaria = 0;
            int idCliente = 0;
            DateTime Fecha;
            if (pTipoEntrada == 1)
            {
                CCuentasPorCobrar Ingreso = new CCuentasPorCobrar();
                Ingreso.LlenaObjeto(pIdCobroCliente, ConexionBaseDatos);
                idIngreso = Ingreso.IdCuentasPorCobrar;
                idClaseGenerador = 71;
                total = Ingreso.Importe;
                idCuentaBancaria = Ingreso.IdCuentaBancaria;
                idCliente = Ingreso.IdCliente;
                Ingreso.SeGeneroAsiento = true;
                Fecha = Ingreso.FechaEmision;
            }
            else
            {
                CDepositos Deposito = new CDepositos();
                Deposito.LlenaObjeto(pIdCobroCliente, ConexionBaseDatos);
                idIngreso = Deposito.IdDepositos;
                idClaseGenerador = 90;
                total = Deposito.Importe;
                idCuentaBancaria = Deposito.IdCuentaBancaria;
                Fecha = Deposito.FechaEmision;

                CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdDeposito", Deposito.IdDepositos);
                IngresosNoDepositados.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
                idCliente = IngresosNoDepositados.IdCliente;
                Deposito.SeGeneroAsiento = true;
            }

            JObject JAsientoContableGenerado = new JObject();
            JAsientoContableGenerado = CAsientoContable.ObtenerJsonAsientoGeneradoCobroCliente(pIdCobroCliente, pTipoEntrada, pIdFacturaCliente, ConexionBaseDatos);

            CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
            CuentaBancaria.LlenaObjeto(idCuentaBancaria, ConexionBaseDatos);

            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(idCliente, ConexionBaseDatos);

            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

            int idEmpresa;
            SDKCONTPAQNGLib.TSdkPoliza Poliza = new SDKCONTPAQNGLib.TSdkPoliza();
            SDKCONTPAQNGLib.TSdkTipoPoliza TipoPoliza = new SDKCONTPAQNGLib.TSdkTipoPoliza();
            SDKCONTPAQNGLib.TSdkSesion Sesion = new SDKCONTPAQNGLib.TSdkSesion();
            SDKCONTPAQNGLib.TSdkMovimientoPoliza MovimientosPoliza = new SDKCONTPAQNGLib.TSdkMovimientoPoliza();
            SDKCONTPAQNGLib.TSdkCuenta Cuenta = new SDKCONTPAQNGLib.TSdkCuenta();
            SDKCONTPAQNGLib.TSdkEmpresa Empresa = new SDKCONTPAQNGLib.TSdkEmpresa();
            SDKCONTPAQNGLib.TSdkControlIVA ControlIVA = new SDKCONTPAQNGLib.TSdkControlIVA();

            Sesion.iniciaConexion();
            int conexionActiva = Sesion.conexionActiva;
            if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 0)
            {
                Sesion.firmaUsuarioParams(SucursalSesion.UsuarioContpaq, SucursalSesion.ContrasenaContpaq);
            }
            int abrioEmpresa = 0;
            if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 1)
            {
                abrioEmpresa = Sesion.abreEmpresa(SucursalSesion.BaseDatosContpaq);
            }
            Empresa.setSesion(Sesion);
            Poliza.setSesion(Sesion);
            Cuenta.setSesion(Sesion);
            MovimientosPoliza.setSesion(Sesion);
            TipoPoliza.setSesion(Sesion);

            if (abrioEmpresa == 1)
            {
                try
                {
                    idEmpresa = Empresa.IdEmpresa;
                    Poliza.iniciarInfo();
                    TipoPoliza.iniciarInfo();
                    TipoPoliza.Tipo = SDKCONTPAQNGLib.ETIPOPOLIZA.TIPO_INGRESOS;

                    Poliza.setUsuario = 1;
                    Poliza.Fecha = DateTime.Now;
                    Poliza.Tipo = TipoPoliza.Tipo;
                    Poliza.Clase = SDKCONTPAQNGLib.ECLASEPOLIZA.CLASE_AFECTAR;
                    Poliza.Impresa = 0;
                    Poliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();
                    Poliza.Ajuste = 0;
                    Poliza.setDiario = 0;
                    Poliza.CodigoDiario = "";
                    Poliza.SistOrigen = SDKCONTPAQNGLib.ESISTORIGEN.ORIG_CONTPAQNG;

                    int movAgregado = 0;
                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosAPesos" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosADolares")
                    {
                        //Cargo
                        MovimientosPoliza.setSesion(Sesion);
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = 1;
                        MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]).Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        //Abono
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = 2;
                        MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableCliente"]).Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }
                    }

                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresADolares" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresAPesos")
                    {
                        if (JAsientoContableGenerado["TipoGananciaPerdida"].ToString() == "Perdida")
                        {
                            #region Importar perdida
                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 1;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 2;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 3;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 4;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 5;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["ClienteComplemento"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 6;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 7;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 8;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPagado"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 9;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }
                            #endregion
                        }
                        else
                        {
                            #region Importa ganancias
                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 1;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 2;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 3;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 4;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 5;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 6;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPendienteGananciaPerdida"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 7;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableClienteComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["ClienteComplemento"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 8;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPagado"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 9;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVATrasladadoPagado"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO DEL CLIENTE " + Organizacion.RazonSocial + ": " + Fecha.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }
                            #endregion
                        }
                    }

                    int grabada = Poliza.crea();
                    if (grabada == 0)
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                        oRespuesta.Add(new JProperty("Descripcion", "No se pudo grabar " + Poliza.getMensajeError()));
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 0));
                        oRespuesta.Add(new JProperty("Descripcion", "Poliza grabada con exito"));
                    }
                }
                catch (Exception a)
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", a.Message + " " + Poliza.UltimoMsjError));
                }
                finally
                {
                    Sesion.cierraEmpresa();
                    Sesion.finalizaConexion();
                }
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add("Descripcion", "No se pudo abrir la empresa en Contpaq");
            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta;
    }

    [WebMethod]
    public static JObject ImportarPolizaPagoProveedor(int pIdPagoProveedor, int pTipoSalida, int pIdFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            int idEgresos = 0;
            int idClaseGenerador = 0;
            decimal total = 0;
            int idCuentaBancaria = 0;
            int idProveedor = 0;
            DateTime fechaEmision;
            if (pTipoSalida == 1)
            {
                CEgresos Egresos = new CEgresos();
                Egresos.LlenaObjeto(pIdPagoProveedor, ConexionBaseDatos);
                idEgresos = Egresos.IdEgresos;
                idClaseGenerador = 79;
                total = Egresos.Importe;
                idCuentaBancaria = Egresos.IdCuentaBancaria;
                idProveedor = Egresos.IdProveedor;
                Egresos.SeGeneroAsiento = true;
                fechaEmision = Egresos.FechaEmision;
            }
            else
            {
                CCheques Cheques = new CCheques();
                Cheques.LlenaObjeto(pIdPagoProveedor, ConexionBaseDatos);
                idEgresos = Cheques.IdCheques;
                idClaseGenerador = 93;
                total = Cheques.Importe;
                idCuentaBancaria = Cheques.IdCuentaBancaria;
                idProveedor = Cheques.IdProveedor;
                Cheques.SeGeneroAsiento = true;
                fechaEmision = Cheques.FechaEmision;
            }

            CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
            CuentaBancaria.LlenaObjeto(idCuentaBancaria, ConexionBaseDatos);

            CProveedor Proveedor = new CProveedor();
            Proveedor.LlenaObjeto(idProveedor, ConexionBaseDatos);

            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, ConexionBaseDatos);

            JObject JAsientoContableGenerado = new JObject();
            JAsientoContableGenerado = CAsientoContable.ObtenerJsonAsientoGeneradoPagoProveedor(pIdPagoProveedor, pTipoSalida, pIdFacturaProveedor, ConexionBaseDatos);

            int idEmpresa;
            SDKCONTPAQNGLib.TSdkPoliza Poliza = new SDKCONTPAQNGLib.TSdkPoliza();
            SDKCONTPAQNGLib.TSdkTipoPoliza TipoPoliza = new SDKCONTPAQNGLib.TSdkTipoPoliza();
            SDKCONTPAQNGLib.TSdkSesion Sesion = new SDKCONTPAQNGLib.TSdkSesion();
            SDKCONTPAQNGLib.TSdkMovimientoPoliza MovimientosPoliza = new SDKCONTPAQNGLib.TSdkMovimientoPoliza();
            SDKCONTPAQNGLib.TSdkCuenta Cuenta = new SDKCONTPAQNGLib.TSdkCuenta();
            SDKCONTPAQNGLib.TSdkEmpresa Empresa = new SDKCONTPAQNGLib.TSdkEmpresa();
            SDKCONTPAQNGLib.TSdkControlIVA ControlIVA = new SDKCONTPAQNGLib.TSdkControlIVA();

            Sesion.iniciaConexion();
            int conexionActiva = Sesion.conexionActiva;

            if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 0)
            {
                Sesion.firmaUsuarioParams("111000", "dno0admin");
            }
            int abrioEmpresa = 0;
            if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 1)
            {
                abrioEmpresa = Sesion.abreEmpresa("ctGrupoAsercom");
            }

            Empresa.setSesion(Sesion);
            Poliza.setSesion(Sesion);
            Cuenta.setSesion(Sesion);
            MovimientosPoliza.setSesion(Sesion);
            TipoPoliza.setSesion(Sesion);

            if (abrioEmpresa == 1)
            {
                try
                {
                    int movAgregado = 0;
                    idEmpresa = Empresa.IdEmpresa;
                    Poliza.iniciarInfo();
                    TipoPoliza.iniciarInfo();
                    TipoPoliza.Tipo = SDKCONTPAQNGLib.ETIPOPOLIZA.TIPO_EGRESOS;

                    Poliza.setUsuario = 1;
                    Poliza.Fecha = DateTime.Now;
                    Poliza.Tipo = TipoPoliza.Tipo;
                    Poliza.Clase = SDKCONTPAQNGLib.ECLASEPOLIZA.CLASE_AFECTAR;
                    Poliza.Impresa = 0;
                    Poliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();
                    Poliza.Ajuste = 0;
                    Poliza.setDiario = 0;
                    Poliza.CodigoDiario = "";
                    Poliza.SistOrigen = SDKCONTPAQNGLib.ESISTORIGEN.ORIG_CONTPAQNG;

                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosAPesos" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "PesosADolares")
                    {
                        //Cargo
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = 1;
                        MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedor"]).Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(Convert.ToString(JAsientoContableGenerado["RealSinTipoCambio"]));//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        //Abono
                        MovimientosPoliza.setSesion(Sesion);
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = 2;
                        MovimientosPoliza.CodigoCuenta = CuentaBancaria.CuentaContable.Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(total);//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }
                    }
                    if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresADolares" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresAPesos")
                    {
                        if (JAsientoContableGenerado["TipoGananciaPerdida"].ToString() == "Perdida")
                        {
                            #region Importar perdida
                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 1;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 2;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 3;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 4;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 5;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 6;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePendienteGananciaPerdida"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 7;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["ProveedorComplemento"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 8;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePagado"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 9;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["ProveedorComplemento"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }
                            #endregion
                        }
                        else
                        {
                            #region Importar ganancia

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 1;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableDolares"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 2;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 3;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancos"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["RealSinTipoCambio"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 4;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableBancosComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["CuentasComplementos"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 7;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["ProveedorComplemento"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 5;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableGananciaPerdida"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["GananciaPerdida"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 6;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePendienteGananciaPerdida"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Cargo
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 8;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["IVAAcreditablePagado"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }

                            //Abono
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = 9;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(JAsientoContableGenerado["CuentaContableProveedorComplemento"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContableGenerado["ProveedorComplemento"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "PAGO A PROVEEDOR " + Organizacion.RazonSocial + ": " + fechaEmision.ToShortDateString();

                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                            if (movAgregado == 0)
                            {
                                oRespuesta.Add(new JProperty("Error", 1));
                                oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                            }
                            #endregion
                        }
                    }

                    int grabada = Poliza.crea();
                    if (grabada == 0)
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                        oRespuesta.Add(new JProperty("Descripcion", "No se pudo grabar " + Poliza.getMensajeError()));
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 0));
                        oRespuesta.Add(new JProperty("Descripcion", "Poliza grabada con exito"));
                    }
                }
                catch (Exception a)
                {
                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", a.Message + " " + Poliza.UltimoMsjError));
                }
                finally
                {
                    Sesion.cierraEmpresa();
                    Sesion.finalizaConexion();
                }
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add("Descripcion", "No se pudo abrir la empresa en Contpaq");
            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta;
    }

    [WebMethod]
    public static JObject ImportarPolizaFacturaProveedor(int pIdFacturaProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEncabezadoFacturaProveedor Factura = new CEncabezadoFacturaProveedor();
            Factura.LlenaObjeto(pIdFacturaProveedor, ConexionBaseDatos);

            Dictionary<string, object> pFacturaSucursal = new Dictionary<string, object>();
            pFacturaSucursal.Add("IdEncabezadoFacturaProveedor", pIdFacturaProveedor);
            CEncabezadoFacturaProveedorSucursal FacturaSucursal = new CEncabezadoFacturaProveedorSucursal();
            FacturaSucursal.LlenaObjetoFiltros(pFacturaSucursal, ConexionBaseDatos);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(FacturaSucursal.IdSucursal, ConexionBaseDatos);

            CDivision Division = new CDivision();
            Division.LlenaObjeto(Factura.IdDivision, ConexionBaseDatos);

            CProveedor Proveedor = new CProveedor();
            Proveedor.LlenaObjeto(Factura.IdProveedor, ConexionBaseDatos);

            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, ConexionBaseDatos);

            CIVA IVA = new CIVA();
            IVA.LlenaObjeto(Sucursal.IdIVA, ConexionBaseDatos);

            int idEmpresa;
            SDKCONTPAQNGLib.TSdkPoliza Poliza = new SDKCONTPAQNGLib.TSdkPoliza();
            SDKCONTPAQNGLib.TSdkTipoPoliza TipoPoliza = new SDKCONTPAQNGLib.TSdkTipoPoliza();
            SDKCONTPAQNGLib.TSdkSesion Sesion = new SDKCONTPAQNGLib.TSdkSesion();
            SDKCONTPAQNGLib.TSdkMovimientoPoliza MovimientosPoliza = new SDKCONTPAQNGLib.TSdkMovimientoPoliza();
            SDKCONTPAQNGLib.TSdkCuenta Cuenta = new SDKCONTPAQNGLib.TSdkCuenta();
            SDKCONTPAQNGLib.TSdkEmpresa Empresa = new SDKCONTPAQNGLib.TSdkEmpresa();
            SDKCONTPAQNGLib.TSdkControlIVA ControlIVA = new SDKCONTPAQNGLib.TSdkControlIVA();

            Sesion.iniciaConexion();
            int conexionActiva = Sesion.conexionActiva;
            if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 0)
            {
                Sesion.firmaUsuarioParams("111000", "dno0admin");
            }
            int abrioEmpresa = 0;
            if (Sesion.conexionActiva == 1 && Sesion.ingresoUsuario == 1)
            {
                abrioEmpresa = Sesion.abreEmpresa("ctGrupoAsercom");
            }

            JObject JAsientoContable = new JObject();
            JAsientoContable = CAsientoContable.ObtenerJsonAsientoGeneradoFacturaProveedor(pIdFacturaProveedor, ConexionBaseDatos);

            Empresa.setSesion(Sesion);
            Poliza.setSesion(Sesion);
            Cuenta.setSesion(Sesion);
            MovimientosPoliza.setSesion(Sesion);
            TipoPoliza.setSesion(Sesion);

            if (abrioEmpresa == 1)
            {
                try
                {
                    idEmpresa = Empresa.IdEmpresa;
                    int movAgregado = 0;
                    Poliza.iniciarInfo();
                    TipoPoliza.iniciarInfo();
                    TipoPoliza.Tipo = SDKCONTPAQNGLib.ETIPOPOLIZA.TIPO_DIARIO;

                    Poliza.setUsuario = 1;
                    Poliza.Fecha = DateTime.Now;
                    Poliza.Tipo = TipoPoliza.Tipo;
                    Poliza.Clase = SDKCONTPAQNGLib.ECLASEPOLIZA.CLASE_AFECTAR;
                    Poliza.Impresa = 0;
                    Poliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;
                    Poliza.Ajuste = 0;
                    Poliza.setDiario = 0;
                    Poliza.CodigoDiario = "";
                    Poliza.SistOrigen = SDKCONTPAQNGLib.ESISTORIGEN.ORIG_CONTPAQNG;

                    CSelectEspecifico SelectDetalleFactura = new CSelectEspecifico();
                    SelectDetalleFactura.StoredProcedure.CommandText = "sp_AsientoContableDetalle_Consultar_ImportarAsientosDetalleFacturaProveedor";
                    SelectDetalleFactura.StoredProcedure.Parameters.AddWithValue("pIdEncabezadoFacturaProveedor", Factura.IdEncabezadoFacturaProveedor);
                    SelectDetalleFactura.Llena(ConexionBaseDatos);

                    int noMovimiento = 0;

                    if (Convert.ToDecimal(JAsientoContable["TotalProveedorComplemento"].ToString()) == 0)
                    {
                        //Cargo
                        while (SelectDetalleFactura.Registros.Read())
                        {
                            noMovimiento = noMovimiento + 1;
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = noMovimiento;
                            MovimientosPoliza.CodigoCuenta = Convert.ToString(SelectDetalleFactura.Registros["CuentaContableMovimientos"]).Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(SelectDetalleFactura.Registros["Abono"]);//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;
                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        }

                        MovimientosPoliza.iniciarInfo();
                        noMovimiento = noMovimiento + 1;
                        MovimientosPoliza.NumMovto = noMovimiento;
                        MovimientosPoliza.CodigoCuenta = IVA.ClaveCuentaContable.Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(Factura.IVA);//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        //Abono
                        noMovimiento = noMovimiento + 1;
                        MovimientosPoliza.setSesion(Sesion);
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = noMovimiento;
                        MovimientosPoliza.CodigoCuenta = Proveedor.CuentaContable.Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(Factura.Total);//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }
                    }
                    if (Convert.ToDecimal(JAsientoContable["TotalProveedorComplemento"].ToString()) > 0)
                    {
                        foreach (JObject JCuentaMovimiento in JAsientoContable["CuentasContablesMovimientos"])
                        {
                            //Cargo
                            noMovimiento = noMovimiento + 1;
                            MovimientosPoliza.setSesion(Sesion);
                            MovimientosPoliza.iniciarInfo();
                            MovimientosPoliza.NumMovto = noMovimiento;
                            MovimientosPoliza.CodigoCuenta = JCuentaMovimiento["CuentaContableMovimientos"].ToString().Replace("-", "");
                            MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                            MovimientosPoliza.Importe = Convert.ToDecimal(JCuentaMovimiento["TotalTipoCompra"].ToString());//Importe en moneda base de la cuenta
                            MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                            MovimientosPoliza.Referencia = "";
                            MovimientosPoliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;
                            movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        }

                        //Cargo
                        noMovimiento = noMovimiento + 1;
                        MovimientosPoliza.setSesion(Sesion);
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = noMovimiento;
                        MovimientosPoliza.CodigoCuenta = JAsientoContable["CuentaContableTrasladado"].ToString().Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_CARGO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContable["IVA"].ToString());//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;

                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        //Abono
                        noMovimiento = noMovimiento + 1;
                        MovimientosPoliza.setSesion(Sesion);
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = noMovimiento;
                        MovimientosPoliza.CodigoCuenta = JAsientoContable["CuentaContableProveedor"].ToString().Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContable["Total"].ToString());//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;
                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }

                        //Abono
                        noMovimiento = noMovimiento + 1;
                        MovimientosPoliza.setSesion(Sesion);
                        MovimientosPoliza.iniciarInfo();
                        MovimientosPoliza.NumMovto = noMovimiento;
                        MovimientosPoliza.CodigoCuenta = JAsientoContable["CuentaProveedorComplemento"].ToString().Replace("-", "");
                        MovimientosPoliza.TipoMovto = SDKCONTPAQNGLib.ETIPOIMPORTEMOVPOLIZA.MOVPOLIZA_ABONO;
                        MovimientosPoliza.Importe = Convert.ToDecimal(JAsientoContable["TotalProveedorComplemento"].ToString());//Importe en moneda base de la cuenta
                        MovimientosPoliza.ImporteME = Convert.ToDecimal(0); //Importe en moneda extranjera
                        MovimientosPoliza.Referencia = "";
                        MovimientosPoliza.Concepto = "FACTURA DE PROVEEDOR " + Organizacion.RazonSocial + ": " + Factura.NumeroFactura;
                        movAgregado = Poliza.agregaMovimiento(MovimientosPoliza);
                        if (movAgregado == 0)
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "No se pudo agregar Movimiento"));
                        }
                    }

                    int grabada = Poliza.crea();
                    if (grabada == 0)
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                        oRespuesta.Add(new JProperty("Descripcion", "No se pudo grabar " + Poliza.getMensajeError()));
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 0));
                        oRespuesta.Add(new JProperty("Descripcion", "Poliza grabada con exito"));
                    }
                }
                catch (Exception a)
                {
                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", a.Message + " " + Poliza.UltimoMsjError));
                }
                finally
                {
                    Sesion.cierraEmpresa();
                    Sesion.finalizaConexion();
                }
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add("Descripcion", "No se pudo abrir la empresa en Contpaq");
            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta;
    }
    #endregion

    #region Validaciones
    private static string ValidarAsientoFacturaCliente(int pIdFacturaCliente, CConexion pConexion)
    {
        string errores = "";

        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        FacturaEncabezado.LlenaObjeto(pIdFacturaCliente, pConexion);

        if (FacturaEncabezado.IdFacturaEncabezado == 0)
        { errores = errores + "<span>*</span> No hay una factura asociada a la poliza.<br />"; }

        if (FacturaEncabezado.IVA <= 0)
        { errores = errores + "<span>*</span> No hay IVA registrado a la factura<br />"; }

        if (FacturaEncabezado.TipoCambio <= 0)
        { errores = errores + "<span>*</span> No hay tipo de cambio registrado en la factura.<br />"; }

        if (FacturaEncabezado.Total <= 0)
        { errores = errores + "<span>*</span> No hay total registrado en factura.<br />"; }

        if (FacturaEncabezado.Subtotal <= 0)
        { errores = errores + "<span>*</span> No hay subtotal registrado en la factura.<br />"; }

        Dictionary<string, object> pFacturaSucursal = new Dictionary<string, object>();
        pFacturaSucursal.Add("IdFacturaEncabezado", pIdFacturaCliente);
        CFacturaEncabezadoSucursal FacturaSucursal = new CFacturaEncabezadoSucursal();
        FacturaSucursal.LlenaObjetoFiltros(pFacturaSucursal, pConexion);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(FacturaSucursal.IdSucursal, pConexion);
        if (Sucursal.IdSucursal <= 0)
        { errores = errores + "<span>*</span> No hay una sucursal registrada en la factura.<br />"; }
        else
        {
            if (Sucursal.ClaveCuentaContable.Trim() == "")
            { errores = errores + "<span>*</span> No hay una clave cuenta contable registrada en la sucursal.<br />"; }
        }

        CDivision Division = new CDivision();
        Division.LlenaObjeto(FacturaEncabezado.IdDivision, pConexion);
        if (Division.IdDivision <= 0)
        { errores = errores + "<span>*</span> No hay una división registrada en la factura.<br />"; }
        else
        {
            if (Division.ClaveCuentaContable.Trim() == "")
            { errores = errores + "<span>*</span> No hay una clave cuenta contable registrada en la división.<br />"; }
        }

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(FacturaEncabezado.IdCliente, pConexion);
        if (Cliente.IdCliente <= 0)
        { errores = errores + "<span>*</span> No hay un cliente registrado en la factura.<br />"; }
        else
        {
            if (Cliente.CuentaContable.Trim() == "")
            { errores = errores + "<span>*</span> No hay una cuenta contable registrada en el cliente.<br />"; }
        }

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        if (Organizacion.IdOrganizacion <= 0)
        { errores = errores + "<span>*</span> No hay una razón social registrada en el cliente.<br />"; }
        else
        {
            if (Cliente.CuentaContable.Trim() == "")
            { errores = errores + "<span>*</span> No hay una cuenta contable registrada en el cliente.<br />"; }
        }

        if (FacturaEncabezado.IdTipoMoneda != 1)
        {
            CCuentaContable CuentaContable = new CCuentaContable();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdTipoCuentaContable", 2);
            CuentaContable.LlenaObjetoFiltros(Parametros, pConexion);

            if (CuentaContable.IdCuentaContable <= 0)
            { errores = errores + "<span>*</span> No hay una cuenta contable cliente complemento registrada.<br />"; }
            else
            {
                if (CuentaContable.CuentaContable.Trim() == "")
                { errores = errores + "<span>*</span> No hay una cuenta contable cliente complemento registrada.<br />"; }
            }
        }

        CIVA IVA = new CIVA();
        IVA.LlenaObjeto(Sucursal.IdIVA, pConexion);
        if (IVA.IdIVA <= 0)
        { errores = errores + "<span>*</span> No hay un IVA registrado en la factura.<br />"; }
        else
        {
            if (IVA.ClaveCuentaContable.Trim() == "")
            { errores = errores + "<span>*</span> No hay una cuenta contable registrada en el IVA.<br />"; }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    public static string ValidarAsientoFacturaProveedor(int pIdFacturaProveedor, CConexion pConexion)
    {
        string errores = "";
        JObject JAsientoContable = new JObject();
        JAsientoContable = CAsientoContable.ObtenerJsonAsientoGeneradoFacturaProveedor(pIdFacturaProveedor, pConexion);

        if (JAsientoContable["CuentaContableProveedor"].ToString() == "" || JAsientoContable["CuentaContableProveedor"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable registrado a la factura<br />"; }

        if (Convert.ToDecimal(JAsientoContable["TotalProveedorComplemento"].ToString()) > 0)
        {
            if (Convert.ToDecimal(JAsientoContable["TotalConversion"].ToString()) == 0)
            { errores = errores + "<span>*</span> No hay total registrado a la factura<br />"; }
        }

        if (JAsientoContable["CuentaContableTrasladado"].ToString() == "" || JAsientoContable["CuentaContableTrasladado"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable para IVA trasladado pendiente.<br />"; }

        if (JAsientoContable["CuentaContableProveedor"].ToString() == "" || JAsientoContable["CuentaContableProveedor"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable para proveedor.<br />"; }

        if (JAsientoContable["CuentaProveedorComplemento"].ToString() == "" || JAsientoContable["CuentaProveedorComplemento"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable para proveedor complemento.<br />"; }

        return errores;
    }

    public static string ValidarAsientoCobroCliente(int pIdCobroCliente, int pIdTipoEntrada, int pIdFacturaCliente, CConexion pConexion)
    {
        string errores = "";
        JObject JAsientoContableGenerado = new JObject();
        JAsientoContableGenerado = CAsientoContable.ObtenerJsonAsientoGeneradoCobroCliente(pIdCobroCliente, pIdTipoEntrada, pIdFacturaCliente, pConexion);

        if (JAsientoContableGenerado["CuentaContableBancos"].ToString() == "" || JAsientoContableGenerado["CuentaContableBancos"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable cliente registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableBancosComplemento"].ToString() == "" || JAsientoContableGenerado["CuentaContableBancosComplemento"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable bancos registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableClienteComplemento"].ToString() == "" || JAsientoContableGenerado["CuentaContableClienteComplemento"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable cliente complemento registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"].ToString() == "" || JAsientoContableGenerado["CuentaContableIVATrasladadoPendiente"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable IVA trasladado pendiente registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableIVATrasladadoPagado"].ToString() == "" || JAsientoContableGenerado["CuentaContableIVATrasladadoPagado"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable IVA trasladado pendiente pagado registrada.<br />"; }

        if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresADolares" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresAPesos")
        {
            if (Convert.ToInt32(JAsientoContableGenerado["ExisteTipoCambioDiarioOficial"].ToString()) == 0)
            { errores = errores + "<span>*</span> No hay tipo de cambio diario oficial registrada.<br />"; }

            if (JAsientoContableGenerado["CuentaContableDolares"].ToString() == "" || JAsientoContableGenerado["CuentaContableDolares"].ToString() == "Sin asignar")
            { errores = errores + "<span>*</span> No hay cuenta contable cliente registrada.<br />"; }

            if (JAsientoContableGenerado["CuentaContableGananciaPerdida"].ToString() == "" || JAsientoContableGenerado["CuentaContableGananciaPerdida"].ToString() == "Sin asignar")
            { errores = errores + "<span>*</span> No hay cuenta contable ganancia/perdida registrada.<br />"; }
        }

        return errores;
    }

    public static string ValidarAsientoPagoProveedor(int pIdEgreso, int pIdTipoSalida, int pIdFacturaProveedor, CConexion pConexion)
    {
        string errores = "";
        JObject JAsientoContableGenerado = new JObject();
        JAsientoContableGenerado = CAsientoContable.ObtenerJsonAsientoGeneradoPagoProveedor(pIdEgreso, pIdTipoSalida, pIdFacturaProveedor, pConexion);

        if (JAsientoContableGenerado["CuentaContableBancos"].ToString() == "" || JAsientoContableGenerado["CuentaContableBancos"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable cliente registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableBancosComplemento"].ToString() == "" || JAsientoContableGenerado["CuentaContableBancosComplemento"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable bancos registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableDolares"].ToString() == "" || JAsientoContableGenerado["CuentaContableDolares"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable proveedor registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableProveedorComplemento"].ToString() == "" || JAsientoContableGenerado["CuentaContableProveedorComplemento"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable proveedor complemento registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableGananciaPerdida"].ToString() == "" || JAsientoContableGenerado["CuentaContableGananciaPerdida"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable ganancia/perdida registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"].ToString() == "" || JAsientoContableGenerado["CuentaContableIVAAcreditablePendiente"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable IVA trasladado pendiente registrada.<br />"; }

        if (JAsientoContableGenerado["CuentaContableIVAAcreditablePagado"].ToString() == "" || JAsientoContableGenerado["CuentaContableIVAAcreditablePagado"].ToString() == "Sin asignar")
        { errores = errores + "<span>*</span> No hay cuenta contable IVA trasladado pendiente pagado registrada.<br />"; }

        if (Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresADolares" || Convert.ToString(JAsientoContableGenerado["TipoPoliza"]) == "DolaresAPesos")
        {
            if (Convert.ToInt32(JAsientoContableGenerado["ExisteTipoCambioDiarioOficial"].ToString()) == 0)
            { errores = errores + "<span>*</span> No hay tipo de cambio diario oficial registrada.<br />"; }

        }

        return errores;
    }
    #endregion
}