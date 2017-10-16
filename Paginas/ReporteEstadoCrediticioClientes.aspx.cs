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
using System.Xml;
using System.Net;
using arquitecturaNet;
using System.IO;
using System.Diagnostics;

public partial class ReporteEstadoCrediticioClientes : System.Web.UI.Page
{
    private static string logoEmpresa;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobrosConsultar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosConsultar.Metodo = "ObtenerMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.TituloTabla = "Pagos";
        grdMovimientosCobrosConsultar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosConsultar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosConsultar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosConsultar.Altura = 120;
        grdMovimientosCobrosConsultar.Ancho = 900;
        grdMovimientosCobrosConsultar.NumeroRegistros = 15;
        grdMovimientosCobrosConsultar.RangoNumeroRegistros = "15,30,60";


        //IdFacturaEncabezado
        CJQColumn ColIdCuentasPorCobrarEncabezadoFacturaConsultar = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Nombre = "IdFacturaEncabezado";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Encabezado = "IdFacturaEncabezado";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaConsultar);

        //Serie
        CJQColumn ColSerieMovimientoConsultar = new CJQColumn();
        ColSerieMovimientoConsultar.Nombre = "Serie";
        ColSerieMovimientoConsultar.Encabezado = "Serie";
        ColSerieMovimientoConsultar.Buscador = "false";
        ColSerieMovimientoConsultar.Alineacion = "left";
        ColSerieMovimientoConsultar.Ancho = "50";
        grdMovimientosCobrosConsultar.Columnas.Add(ColSerieMovimientoConsultar);

        //NumeroFactura
        CJQColumn ColNumeroFacturaMovimientoConsultar = new CJQColumn();
        ColNumeroFacturaMovimientoConsultar.Nombre = "NumeroFactura";
        ColNumeroFacturaMovimientoConsultar.Encabezado = "No factura";
        ColNumeroFacturaMovimientoConsultar.Buscador = "false";
        ColNumeroFacturaMovimientoConsultar.Alineacion = "left";
        ColNumeroFacturaMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColNumeroFacturaMovimientoConsultar);

        //FechaFactura
        CJQColumn ColFechaFacturaMovimientoConsultar = new CJQColumn();
        ColFechaFacturaMovimientoConsultar.Nombre = "FechaFactura";
        ColFechaFacturaMovimientoConsultar.Encabezado = "Fecha factura";
        ColFechaFacturaMovimientoConsultar.Buscador = "false";
        ColFechaFacturaMovimientoConsultar.Alineacion = "left";
        ColFechaFacturaMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFechaFacturaMovimientoConsultar);

        //Fecha de asocicacion
        CJQColumn ColFechaPagoConsultar = new CJQColumn();
        ColFechaPagoConsultar.Nombre = "FechaPago";
        ColFechaPagoConsultar.Encabezado = "Fecha asociación";
        ColFechaPagoConsultar.Buscador = "false";
        ColFechaPagoConsultar.Alineacion = "left";
        ColFechaPagoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFechaPagoConsultar);

        //Fecha pago basada en fecha de aplicacion
        CJQColumn ColFechaPagoAplicacionConsultar = new CJQColumn();
        ColFechaPagoAplicacionConsultar.Nombre = "FechaAplicacion";
        ColFechaPagoAplicacionConsultar.Encabezado = "Fecha pago (Aplicacion)";
        ColFechaPagoAplicacionConsultar.Buscador = "false";
        ColFechaPagoAplicacionConsultar.Alineacion = "left";
        ColFechaPagoAplicacionConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFechaPagoAplicacionConsultar);

        //Fecha Vencimiento
        CJQColumn ColFechaVencimientoConsultar = new CJQColumn();
        ColFechaVencimientoConsultar.Nombre = "FechaVencimiento";
        ColFechaVencimientoConsultar.Encabezado = "Fecha vencimiento";
        ColFechaVencimientoConsultar.Buscador = "false";
        ColFechaVencimientoConsultar.Alineacion = "left";
        ColFechaVencimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFechaVencimientoConsultar);

        //Total
        CJQColumn ColTotalFacturaMovimientoConsultar = new CJQColumn();
        ColTotalFacturaMovimientoConsultar.Nombre = "Total";
        ColTotalFacturaMovimientoConsultar.Encabezado = "Total";
        ColTotalFacturaMovimientoConsultar.Buscador = "false";
        ColTotalFacturaMovimientoConsultar.Formato = "FormatoMoneda";
        ColTotalFacturaMovimientoConsultar.Alineacion = "right";
        ColTotalFacturaMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColTotalFacturaMovimientoConsultar);

        //TipoMoneda
        CJQColumn ColTipoMonedaFacturaMovimientoConsultar = new CJQColumn();
        ColTipoMonedaFacturaMovimientoConsultar.Nombre = "TipoMoneda";
        ColTipoMonedaFacturaMovimientoConsultar.Encabezado = "Moneda";
        ColTipoMonedaFacturaMovimientoConsultar.Buscador = "false";
        ColTipoMonedaFacturaMovimientoConsultar.Alineacion = "left";
        ColTipoMonedaFacturaMovimientoConsultar.Ancho = "70";
        grdMovimientosCobrosConsultar.Columnas.Add(ColTipoMonedaFacturaMovimientoConsultar);


        //Referencia
        CJQColumn ColReferenciaMovimientoConsultar = new CJQColumn();
        ColReferenciaMovimientoConsultar.Nombre = "Referencia";
        ColReferenciaMovimientoConsultar.Encabezado = "Referencia";
        ColReferenciaMovimientoConsultar.Buscador = "false";
        ColReferenciaMovimientoConsultar.Alineacion = "left";
        ColReferenciaMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColReferenciaMovimientoConsultar);

        //MontoPago
        CJQColumn ColMontoPagoConsultar = new CJQColumn();
        ColMontoPagoConsultar.Nombre = "Monto";
        ColMontoPagoConsultar.Encabezado = "Monto abonado";
        ColMontoPagoConsultar.Buscador = "false";
        ColMontoPagoConsultar.Formato = "FormatoMoneda";
        ColMontoPagoConsultar.Alineacion = "right";
        ColMontoPagoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColMontoPagoConsultar);

        //TipoMonedaPago
        CJQColumn ColTipoMonedaPagoFacturaMovimientoConsultar = new CJQColumn();
        ColTipoMonedaPagoFacturaMovimientoConsultar.Nombre = "MonedaPago";
        ColTipoMonedaPagoFacturaMovimientoConsultar.Encabezado = "Moneda pago";
        ColTipoMonedaPagoFacturaMovimientoConsultar.Buscador = "false";
        ColTipoMonedaPagoFacturaMovimientoConsultar.Alineacion = "left";
        ColTipoMonedaPagoFacturaMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColTipoMonedaPagoFacturaMovimientoConsultar);


        //Dias Vencidos
        CJQColumn ColDiasVencidosConsultar = new CJQColumn();
        ColDiasVencidosConsultar.Nombre = "DiasVencidos";
        ColDiasVencidosConsultar.Encabezado = "Días vencidos";
        ColDiasVencidosConsultar.Buscador = "false";
        ColDiasVencidosConsultar.Alineacion = "left";
        ColDiasVencidosConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColDiasVencidosConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobrosConsultar", grdMovimientosCobrosConsultar.GeneraGrid(), true);

        //GridFacturasPendientesConsultar
        CJQGrid grdFacturasPendientesConsultar = new CJQGrid();
        grdFacturasPendientesConsultar.NombreTabla = "grdFacturasPendientesConsultar";
        grdFacturasPendientesConsultar.CampoIdentificador = "IdFacturaEncabezado";
        grdFacturasPendientesConsultar.ColumnaOrdenacion = "IdFacturaEncabezado";
        grdFacturasPendientesConsultar.TipoOrdenacion = "DESC";
        grdFacturasPendientesConsultar.Metodo = "ObtenerFacturasPendientesConsultar";
        grdFacturasPendientesConsultar.TituloTabla = "Facturas pendientes de pago";
        grdFacturasPendientesConsultar.GenerarGridCargaInicial = false;
        grdFacturasPendientesConsultar.GenerarFuncionFiltro = false;
        grdFacturasPendientesConsultar.GenerarFuncionTerminado = false;
        grdFacturasPendientesConsultar.Altura = 120;
        grdFacturasPendientesConsultar.Ancho = 900;
        grdFacturasPendientesConsultar.NumeroRegistros = 15;
        grdFacturasPendientesConsultar.RangoNumeroRegistros = "15,30,60";


        //IdFacturaEncabezado
        CJQColumn ColIdFacturaEncabezadoConsultar = new CJQColumn();
        ColIdFacturaEncabezadoConsultar.Nombre = "IdFacturaEncabezado";
        ColIdFacturaEncabezadoConsultar.Oculto = "true";
        ColIdFacturaEncabezadoConsultar.Encabezado = "IdFacturaEncabezado";
        ColIdFacturaEncabezadoConsultar.Buscador = "false";
        grdFacturasPendientesConsultar.Columnas.Add(ColIdFacturaEncabezadoConsultar);


        //NumeroFactura
        CJQColumn ColNumeroFacturaPendienteConsultar = new CJQColumn();
        ColNumeroFacturaPendienteConsultar.Nombre = "NumeroFactura";
        ColNumeroFacturaPendienteConsultar.Encabezado = "No factura";
        ColNumeroFacturaPendienteConsultar.Buscador = "false";
        ColNumeroFacturaPendienteConsultar.Alineacion = "left";
        ColNumeroFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColNumeroFacturaPendienteConsultar);

        //Serie
        CJQColumn ColSerieFacturaPendienteConsultar = new CJQColumn();
        ColSerieFacturaPendienteConsultar.Nombre = "SerieFactura";
        ColSerieFacturaPendienteConsultar.Encabezado = "Serie";
        ColSerieFacturaPendienteConsultar.Buscador = "false";
        ColSerieFacturaPendienteConsultar.Alineacion = "left";
        ColSerieFacturaPendienteConsultar.Ancho = "50";
        grdFacturasPendientesConsultar.Columnas.Add(ColSerieFacturaPendienteConsultar);


        //FechaFactura
        CJQColumn ColFechaFacturaFacturaPendienteConsultar = new CJQColumn();
        ColFechaFacturaFacturaPendienteConsultar.Nombre = "Fecha";
        ColFechaFacturaFacturaPendienteConsultar.Encabezado = "Fecha factura";
        ColFechaFacturaFacturaPendienteConsultar.Buscador = "false";
        ColFechaFacturaFacturaPendienteConsultar.Alineacion = "left";
        ColFechaFacturaFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColFechaFacturaFacturaPendienteConsultar);

        //Fecha pago
        CJQColumn ColFechaPagoFacturaPendienteConsultar = new CJQColumn();
        ColFechaPagoFacturaPendienteConsultar.Nombre = "FechaPago";
        ColFechaPagoFacturaPendienteConsultar.Encabezado = "Fecha pago";
        ColFechaPagoFacturaPendienteConsultar.Buscador = "false";
        ColFechaPagoFacturaPendienteConsultar.Alineacion = "left";
        ColFechaPagoFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColFechaPagoFacturaPendienteConsultar);

        //Subtotal
        CJQColumn ColSubTotalFacturaPendienteConsultar = new CJQColumn();
        ColSubTotalFacturaPendienteConsultar.Nombre = "Subtotal";
        ColSubTotalFacturaPendienteConsultar.Encabezado = "Subtotal";
        ColSubTotalFacturaPendienteConsultar.Buscador = "false";
        ColSubTotalFacturaPendienteConsultar.Formato = "FormatoMoneda";
        ColSubTotalFacturaPendienteConsultar.Alineacion = "right";
        ColSubTotalFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColSubTotalFacturaPendienteConsultar);

        //IVA
        CJQColumn ColIVAFacturaPendienteConsultar = new CJQColumn();
        ColIVAFacturaPendienteConsultar.Nombre = "IVA";
        ColIVAFacturaPendienteConsultar.Encabezado = "Iva";
        ColIVAFacturaPendienteConsultar.Buscador = "false";
        ColIVAFacturaPendienteConsultar.Formato = "FormatoMoneda";
        ColIVAFacturaPendienteConsultar.Alineacion = "right";
        ColIVAFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColIVAFacturaPendienteConsultar);

        //Descuento
        CJQColumn ColDescuentoFacturaPendienteConsultar = new CJQColumn();
        ColDescuentoFacturaPendienteConsultar.Nombre = "Descuento";
        ColDescuentoFacturaPendienteConsultar.Encabezado = "Descuento";
        ColDescuentoFacturaPendienteConsultar.Buscador = "false";
        ColDescuentoFacturaPendienteConsultar.Formato = "FormatoMoneda";
        ColDescuentoFacturaPendienteConsultar.Alineacion = "right";
        ColDescuentoFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColDescuentoFacturaPendienteConsultar);

        //Total
        CJQColumn ColTotalFacturaPendienteConsultar = new CJQColumn();
        ColTotalFacturaPendienteConsultar.Nombre = "Total";
        ColTotalFacturaPendienteConsultar.Encabezado = "Total";
        ColTotalFacturaPendienteConsultar.Buscador = "false";
        ColTotalFacturaPendienteConsultar.Formato = "FormatoMoneda";
        ColTotalFacturaPendienteConsultar.Alineacion = "right";
        ColTotalFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColTotalFacturaPendienteConsultar);

        //Saldo factura
        CJQColumn ColSaldoFacturaPendienteConsultar = new CJQColumn();
        ColSaldoFacturaPendienteConsultar.Nombre = "SaldoFactura";
        ColSaldoFacturaPendienteConsultar.Encabezado = "Saldo";
        ColSaldoFacturaPendienteConsultar.Buscador = "false";
        ColSaldoFacturaPendienteConsultar.Formato = "FormatoMoneda";
        ColSaldoFacturaPendienteConsultar.Alineacion = "right";
        ColSaldoFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColSaldoFacturaPendienteConsultar);

        //TipoMoneda
        CJQColumn ColTipoMonedaFacturaPendienteConsultar = new CJQColumn();
        ColTipoMonedaFacturaPendienteConsultar.Nombre = "TipoMoneda";
        ColTipoMonedaFacturaPendienteConsultar.Encabezado = "Moneda";
        ColTipoMonedaFacturaPendienteConsultar.Buscador = "false";
        ColTipoMonedaFacturaPendienteConsultar.Alineacion = "left";
        ColTipoMonedaFacturaPendienteConsultar.Ancho = "70";
        grdFacturasPendientesConsultar.Columnas.Add(ColTipoMonedaFacturaPendienteConsultar);

        //Saldo pesos factura
        CJQColumn ColSaldoPesosFacturaPendienteConsultar = new CJQColumn();
        ColSaldoPesosFacturaPendienteConsultar.Nombre = "SaldoPesos";
        ColSaldoPesosFacturaPendienteConsultar.Encabezado = "Saldo pesos";
        ColSaldoPesosFacturaPendienteConsultar.Buscador = "false";
        ColSaldoPesosFacturaPendienteConsultar.Formato = "FormatoMoneda";
        ColSaldoPesosFacturaPendienteConsultar.Alineacion = "right";
        ColSaldoPesosFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColSaldoPesosFacturaPendienteConsultar);

        //Estatus
        CJQColumn ColEstatusFacturaPendienteConsultar = new CJQColumn();
        ColEstatusFacturaPendienteConsultar.Nombre = "Estatus";
        ColEstatusFacturaPendienteConsultar.Encabezado = "Status";
        ColEstatusFacturaPendienteConsultar.Buscador = "false";
        ColEstatusFacturaPendienteConsultar.Alineacion = "left";
        ColEstatusFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColEstatusFacturaPendienteConsultar);

        //Agente
        CJQColumn ColAgenteFacturaPendienteConsultar = new CJQColumn();
        ColAgenteFacturaPendienteConsultar.Nombre = "Referencia";
        ColAgenteFacturaPendienteConsultar.Encabezado = "Referencia";
        ColAgenteFacturaPendienteConsultar.Buscador = "false";
        ColAgenteFacturaPendienteConsultar.Alineacion = "left";
        ColAgenteFacturaPendienteConsultar.Ancho = "80";
        grdFacturasPendientesConsultar.Columnas.Add(ColAgenteFacturaPendienteConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturasPendientesConsultar", grdFacturasPendientesConsultar.GeneraGrid(), true);

        //GridProyectosConsultar
        CJQGrid grdProyectosConsultar = new CJQGrid();
        grdProyectosConsultar.NombreTabla = "grdProyectosConsultar";
        grdProyectosConsultar.CampoIdentificador = "IdProyecto";
        grdProyectosConsultar.ColumnaOrdenacion = "IdProyecto";
        grdProyectosConsultar.TipoOrdenacion = "DESC";
        grdProyectosConsultar.Metodo = "ObtenerProyectosConsultar";
        grdProyectosConsultar.TituloTabla = "Proyectos";
        grdProyectosConsultar.GenerarGridCargaInicial = false;
        grdProyectosConsultar.GenerarFuncionFiltro = false;
        grdProyectosConsultar.GenerarFuncionTerminado = false;
        grdProyectosConsultar.Altura = 120;
        grdProyectosConsultar.Ancho = 900;
        grdProyectosConsultar.NumeroRegistros = 15;
        grdProyectosConsultar.RangoNumeroRegistros = "15,30,60";


        //IdFacturaEncabezado
        CJQColumn ColIdProyectoConsultar = new CJQColumn();
        ColIdProyectoConsultar.Nombre = "IdProyecto";
        ColIdProyectoConsultar.Oculto = "false";
        ColIdProyectoConsultar.Encabezado = "No. proyecto";
        ColIdProyectoConsultar.Buscador = "false";
        ColIdProyectoConsultar.Ancho = "60";
        grdProyectosConsultar.Columnas.Add(ColIdProyectoConsultar);


        //Nombre proyecto
        CJQColumn ColNombreProyectoConsultar = new CJQColumn();
        ColNombreProyectoConsultar.Nombre = "NombreProyecto";
        ColNombreProyectoConsultar.Encabezado = "Nombre proyecto";
        ColNombreProyectoConsultar.Buscador = "false";
        ColNombreProyectoConsultar.Alineacion = "left";
        ColNombreProyectoConsultar.Ancho = "120";
        grdProyectosConsultar.Columnas.Add(ColNombreProyectoConsultar);

        //Fecha inicio proyecto
        CJQColumn ColFechaInicioProyectoConsultar = new CJQColumn();
        ColFechaInicioProyectoConsultar.Nombre = "FechaInicio";
        ColFechaInicioProyectoConsultar.Encabezado = "Fecha inicio";
        ColFechaInicioProyectoConsultar.Buscador = "false";
        ColFechaInicioProyectoConsultar.Alineacion = "left";
        ColFechaInicioProyectoConsultar.Ancho = "60";
        grdProyectosConsultar.Columnas.Add(ColFechaInicioProyectoConsultar);


        //Fecha termino proyecto
        CJQColumn ColFechaTerminoProyectoConsultar = new CJQColumn();
        ColFechaTerminoProyectoConsultar.Nombre = "FechaTermino";
        ColFechaTerminoProyectoConsultar.Encabezado = "Fecha termino";
        ColFechaTerminoProyectoConsultar.Buscador = "false";
        ColFechaTerminoProyectoConsultar.Alineacion = "left";
        ColFechaTerminoProyectoConsultar.Ancho = "70";
        grdProyectosConsultar.Columnas.Add(ColFechaTerminoProyectoConsultar);

        //Razon social
        CJQColumn ColRazonSocialProyectoConsultar = new CJQColumn();
        ColRazonSocialProyectoConsultar.Nombre = "RazonSocial";
        ColRazonSocialProyectoConsultar.Encabezado = "Razón social";
        ColRazonSocialProyectoConsultar.Buscador = "false";
        ColRazonSocialProyectoConsultar.Alineacion = "left";
        ColRazonSocialProyectoConsultar.Ancho = "120";
        grdProyectosConsultar.Columnas.Add(ColRazonSocialProyectoConsultar);

        //Precio teorico
        CJQColumn ColPrecioTeoricoProyectoConsultar = new CJQColumn();
        ColPrecioTeoricoProyectoConsultar.Nombre = "PrecioTeorico";
        ColPrecioTeoricoProyectoConsultar.Encabezado = "Precio teorico";
        ColPrecioTeoricoProyectoConsultar.Buscador = "false";
        ColPrecioTeoricoProyectoConsultar.Formato = "FormatoMoneda";
        ColPrecioTeoricoProyectoConsultar.Alineacion = "right";
        ColPrecioTeoricoProyectoConsultar.Ancho = "80";
        grdProyectosConsultar.Columnas.Add(ColPrecioTeoricoProyectoConsultar);

        //Costo teorico
        CJQColumn ColCostoTeoricoProyectoConsultar = new CJQColumn();
        ColCostoTeoricoProyectoConsultar.Nombre = "CostoTeorico";
        ColCostoTeoricoProyectoConsultar.Encabezado = "Costo teorico";
        ColCostoTeoricoProyectoConsultar.Buscador = "false";
        ColCostoTeoricoProyectoConsultar.Formato = "FormatoMoneda";
        ColCostoTeoricoProyectoConsultar.Alineacion = "right";
        ColCostoTeoricoProyectoConsultar.Ancho = "80";
        grdProyectosConsultar.Columnas.Add(ColCostoTeoricoProyectoConsultar);

        //TipoMoneda
        CJQColumn ColTipoMonedaProyectoConsultar = new CJQColumn();
        ColTipoMonedaProyectoConsultar.Nombre = "TipoMoneda";
        ColTipoMonedaProyectoConsultar.Encabezado = "Moneda";
        ColTipoMonedaProyectoConsultar.Buscador = "false";
        ColTipoMonedaProyectoConsultar.Alineacion = "left";
        ColTipoMonedaProyectoConsultar.Ancho = "60";
        grdProyectosConsultar.Columnas.Add(ColTipoMonedaProyectoConsultar);

        //Responsable
        CJQColumn ColResponsableProyectoConsultar = new CJQColumn();
        ColResponsableProyectoConsultar.Nombre = "Responsable";
        ColResponsableProyectoConsultar.Encabezado = "Responsable";
        ColResponsableProyectoConsultar.Buscador = "false";
        ColResponsableProyectoConsultar.Alineacion = "left";
        ColResponsableProyectoConsultar.Ancho = "80";
        grdProyectosConsultar.Columnas.Add(ColResponsableProyectoConsultar);

        //Estatus
        CJQColumn ColEstatusProyectoConsultar = new CJQColumn();
        ColEstatusProyectoConsultar.Nombre = "Estatus";
        ColEstatusProyectoConsultar.Encabezado = "Estado";
        ColEstatusProyectoConsultar.Buscador = "false";
        ColEstatusProyectoConsultar.Alineacion = "left";
        ColEstatusProyectoConsultar.Ancho = "60";
        grdProyectosConsultar.Columnas.Add(ColEstatusProyectoConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProyectosConsultar", grdProyectosConsultar.GeneraGrid(), true);

        //GridFacturacion Consultar
        CJQGrid grdFacturacionConsultar = new CJQGrid();
        grdFacturacionConsultar.NombreTabla = "grdFacturacionConsultar";
        grdFacturacionConsultar.CampoIdentificador = "IdFacturaEncabezado";
        grdFacturacionConsultar.ColumnaOrdenacion = "IdFacturaEncabezado";
        grdFacturacionConsultar.TipoOrdenacion = "DESC";
        grdFacturacionConsultar.Metodo = "ObtenerFacturacionConsultar";
        grdFacturacionConsultar.TituloTabla = "Facturas";
        grdFacturacionConsultar.GenerarGridCargaInicial = false;
        grdFacturacionConsultar.GenerarFuncionFiltro = false;
        grdFacturacionConsultar.GenerarFuncionTerminado = false;
        grdFacturacionConsultar.Altura = 120;
        grdFacturacionConsultar.Ancho = 900;
        grdFacturacionConsultar.NumeroRegistros = 15;
        grdFacturacionConsultar.RangoNumeroRegistros = "15,30,60";


        //IdFacturaEncabezado
        CJQColumn ColIdFacturaEncabezadoFacturacionConsultar = new CJQColumn();
        ColIdFacturaEncabezadoFacturacionConsultar.Nombre = "IdFacturaEncabezado";
        ColIdFacturaEncabezadoFacturacionConsultar.Oculto = "true";
        ColIdFacturaEncabezadoFacturacionConsultar.Encabezado = "IdFacturaEncabezado";
        ColIdFacturaEncabezadoFacturacionConsultar.Buscador = "false";
        grdFacturacionConsultar.Columnas.Add(ColIdFacturaEncabezadoFacturacionConsultar);


        //NumeroFactura
        CJQColumn ColNumeroFacturaFacturacionConsultar = new CJQColumn();
        ColNumeroFacturaFacturacionConsultar.Nombre = "NumeroFactura";
        ColNumeroFacturaFacturacionConsultar.Encabezado = "No factura";
        ColNumeroFacturaFacturacionConsultar.Buscador = "false";
        ColNumeroFacturaFacturacionConsultar.Alineacion = "left";
        ColNumeroFacturaFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColNumeroFacturaFacturacionConsultar);

        //Serie
        CJQColumn ColSerieFacturacionConsultar = new CJQColumn();
        ColSerieFacturacionConsultar.Nombre = "SerieFactura";
        ColSerieFacturacionConsultar.Encabezado = "Serie";
        ColSerieFacturacionConsultar.Buscador = "false";
        ColSerieFacturacionConsultar.Alineacion = "left";
        ColSerieFacturacionConsultar.Ancho = "50";
        grdFacturacionConsultar.Columnas.Add(ColSerieFacturacionConsultar);


        //FechaFactura
        CJQColumn ColFechaFacturacionConsultar = new CJQColumn();
        ColFechaFacturacionConsultar.Nombre = "Fecha";
        ColFechaFacturacionConsultar.Encabezado = "Fecha factura";
        ColFechaFacturacionConsultar.Buscador = "false";
        ColFechaFacturacionConsultar.Alineacion = "left";
        ColFechaFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColFechaFacturacionConsultar);

        //Fecha pago
        CJQColumn ColFechaPagoFacturacionConsultar = new CJQColumn();
        ColFechaPagoFacturacionConsultar.Nombre = "FechaPago";
        ColFechaPagoFacturacionConsultar.Encabezado = "Fecha Vencimiento";
        ColFechaPagoFacturacionConsultar.Buscador = "false";
        ColFechaPagoFacturacionConsultar.Alineacion = "left";
        ColFechaPagoFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColFechaPagoFacturacionConsultar);

        //Subtotal
        CJQColumn ColSubTotalFacturacionConsultar = new CJQColumn();
        ColSubTotalFacturacionConsultar.Nombre = "Total";
        ColSubTotalFacturacionConsultar.Encabezado = "Total";
        ColSubTotalFacturacionConsultar.Buscador = "false";
        ColSubTotalFacturacionConsultar.Formato = "FormatoMoneda";
        ColSubTotalFacturacionConsultar.Alineacion = "right";
        ColSubTotalFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColSubTotalFacturacionConsultar);

        //IVA
        CJQColumn ColIVAFacturacionConsultar = new CJQColumn();
        ColIVAFacturacionConsultar.Nombre = "IVA";
        ColIVAFacturacionConsultar.Encabezado = "Iva";
        ColIVAFacturacionConsultar.Buscador = "false";
        ColIVAFacturacionConsultar.Formato = "FormatoMoneda";
        ColIVAFacturacionConsultar.Alineacion = "right";
        ColIVAFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColIVAFacturacionConsultar);

        //Descuento
        CJQColumn ColDescuentoFacturacionConsultar = new CJQColumn();
        ColDescuentoFacturacionConsultar.Nombre = "Descuento";
        ColDescuentoFacturacionConsultar.Encabezado = "Descuento";
        ColDescuentoFacturacionConsultar.Buscador = "false";
        ColDescuentoFacturacionConsultar.Formato = "FormatoMoneda";
        ColDescuentoFacturacionConsultar.Alineacion = "right";
        ColDescuentoFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColDescuentoFacturacionConsultar);

        //Total
        CJQColumn ColTotalFacturacionConsultar = new CJQColumn();
        ColTotalFacturacionConsultar.Nombre = "Total";
        ColTotalFacturacionConsultar.Encabezado = "Total";
        ColTotalFacturacionConsultar.Buscador = "false";
        ColTotalFacturacionConsultar.Formato = "FormatoMoneda";
        ColTotalFacturacionConsultar.Alineacion = "right";
        ColTotalFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColTotalFacturacionConsultar);

        //Saldo factura
        CJQColumn ColSaldoFacturacionConsultar = new CJQColumn();
        ColSaldoFacturacionConsultar.Nombre = "SaldoFactura";
        ColSaldoFacturacionConsultar.Encabezado = "Saldo";
        ColSaldoFacturacionConsultar.Buscador = "false";
        ColSaldoFacturacionConsultar.Formato = "FormatoMoneda";
        ColSaldoFacturacionConsultar.Alineacion = "right";
        ColSaldoFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColSaldoFacturacionConsultar);

        //TipoMoneda
        CJQColumn ColTipoMonedaFacturacionConsultar = new CJQColumn();
        ColTipoMonedaFacturacionConsultar.Nombre = "TipoMoneda";
        ColTipoMonedaFacturacionConsultar.Encabezado = "Moneda";
        ColTipoMonedaFacturacionConsultar.Buscador = "false";
        ColTipoMonedaFacturacionConsultar.Alineacion = "left";
        ColTipoMonedaFacturacionConsultar.Ancho = "70";
        grdFacturacionConsultar.Columnas.Add(ColTipoMonedaFacturacionConsultar);

        //Saldo pesos factura
        CJQColumn ColSaldoPesosFacturacionConsultar = new CJQColumn();
        ColSaldoPesosFacturacionConsultar.Nombre = "SaldoPesos";
        ColSaldoPesosFacturacionConsultar.Encabezado = "Saldo pesos";
        ColSaldoPesosFacturacionConsultar.Buscador = "false";
        ColSaldoPesosFacturacionConsultar.Formato = "FormatoMoneda";
        ColSaldoPesosFacturacionConsultar.Alineacion = "right";
        ColSaldoPesosFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColSaldoPesosFacturacionConsultar);

        //Estatus
        CJQColumn ColEstatusFacturacionConsultar = new CJQColumn();
        ColEstatusFacturacionConsultar.Nombre = "Estatus";
        ColEstatusFacturacionConsultar.Encabezado = "Status";
        ColEstatusFacturacionConsultar.Buscador = "false";
        ColEstatusFacturacionConsultar.Alineacion = "left";
        ColEstatusFacturacionConsultar.Ancho = "80";
        grdFacturacionConsultar.Columnas.Add(ColEstatusFacturacionConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturacionConsultar", grdFacturacionConsultar.GeneraGrid(), true);

        PintaGridDetalleFacturarConsultar();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
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

    public static string ObtenerFormaFiltroReporteEstadoCrediticioClientes()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CSucursal SucursalActual = new CSucursal();
        SucursalActual.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        JObject oPermisos = new JObject();
        int puedeVerSucursales = 0;

        if (Usuario.TienePermisos(new string[] { "puedeVerSucursales" }, ConexionBaseDatos) == "")
        {
            puedeVerSucursales = 1;
        }
        oPermisos.Add("puedeVerSucursales", puedeVerSucursales);


        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;
        DateTime FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime FechaFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        Modelo.Add("FechaInicial", Convert.ToString(Fecha.ToShortDateString()));
        JArray JASucursales = new JArray();
        foreach (CSucursal oSucursal in SucursalActual.ObtenerSucursalesAsignadas(Usuario.IdUsuario, SucursalActual.IdEmpresa, ConexionBaseDatos))
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("Valor", oSucursal.IdSucursal);
            JSucursal.Add("Descripcion", oSucursal.Sucursal);
            if (SucursalActual.IdSucursal == oSucursal.IdSucursal)
            {
                JSucursal.Add("Selected", 1);
            }
            else
            {
                JSucursal.Add("Selected", 0);
            }
            JASucursales.Add(JSucursal);
        }
        Modelo.Add("Sucursales", JASucursales);
        Modelo.Add(new JProperty("Permisos", oPermisos));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneEstadoCrediticioCliente(int pIdCliente, string pNombreCliente, int pIdSucursal, string pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            string DetalleCrediticio = "";
            string PromedioDiasVencidos = "";
            string SaldoActualPesos = "";
            string PagosMontoAplicarPesos = "";
            string PagosMontoAplicarD = "";
            CSelectEspecifico ConsultaReporteEstadoCrediticioCliente = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioCliente";
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdCliente", pIdCliente);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioCliente.Llena(ConexionBaseDatos);
            DetalleCrediticio = DetalleCrediticio + "FACTURACIÓN" + Convert.ToChar(13);
            Modelo.Add("Cliente", pNombreCliente);
            if (pIdSucursal == 0)
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            Modelo.Add("TitleFact", "FACTURACIÓN");
            JArray jAResult = new JArray();
            while (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
            {
                JObject JODetalleCd = new JObject();
                DetalleCrediticio = DetalleCrediticio + "AÑO: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Anio"]) + "    TOTAL PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Pesos"]) + "                         TOTAL DOLARES: " + "HOLA" + Convert.ToChar(13);
                JODetalleCd.Add("DetalleCrediticio", "AÑO: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Anio"]) + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp TOTAL PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Pesos"]) + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp   TOTAL DOLARES: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Dolares"]));
                jAResult.Add(JODetalleCd);
            }

            Modelo.Add("ListDetalleCredito", jAResult);

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    DetalleCrediticio = DetalleCrediticio + "SALDO TOTAL PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Pesos"]) + "                                SALDO TOTAL DOLARES: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Dolares"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredSaldoTotal", "SALDO TOTAL PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Pesos"]));
                    Modelo.Add("DetalleCredSaldoTotalDolar", "SALDO TOTAL DOLARES: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Dolares"]));
                }
            }
            DetalleCrediticio = DetalleCrediticio + Convert.ToChar(13);
            DetalleCrediticio = DetalleCrediticio + Convert.ToChar(13);
            DetalleCrediticio = DetalleCrediticio + "SALDO VENCIDO" + Convert.ToChar(13);

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    DetalleCrediticio = DetalleCrediticio + "SALDO VENCIDO TOTAL PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"]) + "                        SALDO VENCIDO TOTAL DOLARES: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoD"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredSaldoVencidoPesos", "SALDO VENCIDO TOTAL PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"]));
                    Modelo.Add("DetalleCredSaldoVencidoDolares", "SALDO VENCIDO TOTAL DOLARES: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoD"]));
                }
            }

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {

                    DetalleCrediticio = DetalleCrediticio + "SALDO VENCIDO PESOS DE 0 - 30 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"]) + "               SALDO VENCIDO DOLARES DE 0 - 30 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaD"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredSaldoVencido30Pesos", "SALDO VENCIDO PESOS DE 0 - 30 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"]));
                    Modelo.Add("DetalleCredSaldoVencido30D", "SALDO VENCIDO DOLARES DE 0 - 30 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaD"]));
                }
            }

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {

                    DetalleCrediticio = DetalleCrediticio + "SALDO VENCIDO PESOS DE 31 - 60 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"]) + "               SALDO VENCIDO DOLARES DE 31 - 60 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaD"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredSaldoVencido31Pesos", "SALDO VENCIDO PESOS DE 31 - 60 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"]));
                    Modelo.Add("DetalleCredSaldoVencido31D", " SALDO VENCIDO DOLARES DE 31 - 60 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaD"]));
                }
            }

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {

                    DetalleCrediticio = DetalleCrediticio + "SALDO VENCIDO PESOS DE 61 - 90 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"]) + "         SALDO VENCIDO DOLARES DE 61 - 90 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaD"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredSaldoVencido61Pesos", "SALDO VENCIDO PESOS DE 61 - 90 DíAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"]));
                    Modelo.Add("DetalleCredSaldoVencido61D", "SALDO VENCIDO DOLARES DE 61 - 90 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaD"]));
                }
            }

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {

                    DetalleCrediticio = DetalleCrediticio + "SALDO VENCIDO PESOS DE MAS DE 90 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"]) + "       SALDO VENCIDO DOLARES DE MAS DE 90 DÍAS: : " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaD"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredSaldoVencido90Pesos", "SALDO VENCIDO PESOS DE MAS DE 90 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"]));
                    Modelo.Add("DetalleCredSaldoVencido90D", "SALDO VENCIDO DOLARES DE MAS DE 90 DÍAS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaD"]));
                }
            }

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    DetalleCrediticio = DetalleCrediticio + "LIMITE DE CRÉDITO PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["LimiteDeCreditoPesos"]) + "               LIMITE DE CRÉDITO DOLARES: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["LimiteDeCreditoDolares"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredLimCredPesos", "LIMITE DE CRÉDITO PESOS: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["LimiteDeCreditoPesos"]));
                    Modelo.Add("DetalleCredLimCredD", "LIMITE DE CRÉDITO DOLARES: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["LimiteDeCreditoDolares"]));
                }
            }

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    DetalleCrediticio = DetalleCrediticio + "CONDICIONES DE CRÉDITO: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["CondicionPago"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredCondCred", "CONDICIONES DE CRÉDITO: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["CondicionPago"]));
                }
            }

            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    DetalleCrediticio = DetalleCrediticio + "ULTIMA FECHA DE PAGO: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["FechaPago"]) + Convert.ToChar(13);
                    Modelo.Add("DetalleCredUltima", "ULTIMA FECHA DE PAGO: " + Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["FechaPago"]));
                }
            }
            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    PromedioDiasVencidos = Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["PromedioDiasVencidos"]);
                }
            }
            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    SaldoActualPesos = Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["FacturasPSaldoActualPesos"]);
                }
            }
            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    PagosMontoAplicarPesos = Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MontoPorAplicarPesos"]);
                }
            }
            if (ConsultaReporteEstadoCrediticioCliente.Registros.NextResult())
            {
                if (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
                {
                    PagosMontoAplicarD = Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MontoPorAplicarD"]);
                }
            }
            Modelo.Add("PromedioDiasVencidos", PromedioDiasVencidos);
            Modelo.Add("PagosMontoPorAplicarPesos", PagosMontoAplicarPesos);
            Modelo.Add("PagosMontoPorAplicarDolares", PagosMontoAplicarD);
            Modelo.Add("FacturasPSaldoActualPesos", SaldoActualPesos);
            Modelo.Add("DetalleCrediticio", Convert.ToString(DetalleCrediticio));

            //if (ConsultaReporteEstadoCuentaCliente.Registros.Read())
            //{
            //    Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["RazonSocialReceptor"]));
            //    Modelo.Add("SALDOINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoInicial"]));
            //    Modelo.Add("FECHA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Fecha"]));
            //    Modelo.Add("FECHAINICIAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaInicial"]));
            //    Modelo.Add("FECHAFINAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaFinal"]));
            //    Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TipoMoneda"]));
            //    Modelo.Add("ESTADOCUENTA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["EstadoCuenta"]));
            //    Modelo.Add("CLIENTE", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Cliente"]));
            //    Modelo.Add("NOMBRE", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Nombre"]));
            //    Modelo.Add("SUMACARGOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SumaCargos"]));
            //    Modelo.Add("SUMAABONOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SumaAbonos"]));
            //    Modelo.Add("SALDOFINAL", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoFinal"]));
            //    Modelo.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["CorrientePesos"]));
            //    Modelo.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["UnoTreintaPesos"]));
            //    Modelo.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TreintaSesentaPesos"]));
            //    Modelo.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SesentaNoventaPesos"]));
            //    Modelo.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["MasNoventaPesos"]));
            //    Modelo.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TotalVencidoPesos"]));

            //    Modelo.Add("SALDOFINALD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoFinalD"]));
            //    Modelo.Add("CORRIENTED", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["CorrienteD"]));
            //    Modelo.Add("UNOTREINTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["UnoTreintaD"]));
            //    Modelo.Add("TREINTASESENTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TreintaSesentaD"]));
            //    Modelo.Add("SESENTANOVENTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SesentaNoventaD"]));
            //    Modelo.Add("MASNOVENTAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["MasNoventaD"]));
            //    Modelo.Add("TOTALVENCIDOD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TotalVencidoD"]));
            //}

            //if (ConsultaReporteEstadoCuentaCliente.Registros.NextResult())
            //{
            //    JArray JAMovimientos = new JArray();
            //    while (ConsultaReporteEstadoCuentaCliente.Registros.Read())
            //    {
            //        JObject JMovimiento = new JObject();
            //        JMovimiento.Add("FECHAMOV", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaMovimiento"]));
            //        JMovimiento.Add("SERIE", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Serie"]));
            //        JMovimiento.Add("FOLIO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Folio"]));
            //        JMovimiento.Add("CONCEPTO", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Concepto"]));
            //        JMovimiento.Add("CARGOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Cargos"]));
            //        JMovimiento.Add("ABONOS", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["Abonos"]));
            //        JMovimiento.Add("SALDODOC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoDocumento"]));
            //        JMovimiento.Add("FECHAVEN", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaVencimiento"]));
            //        JMovimiento.Add("DIASVENC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["DiasVencidos"]));
            //        JMovimiento.Add("MONEDA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TM"]));
            //        JMovimiento.Add("TC", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TC"]));
            //        JMovimiento.Add("REFERENCIA", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["REF"]));
            //        JAMovimientos.Add(JMovimiento);
            //    }
            //    Modelo.Add("Movimientos", JAMovimientos);
            //}

            //if (ConsultaReporteEstadoCuentaCliente.Registros.NextResult())
            //{
            //    JArray JAMovimientosD = new JArray();
            //    while (ConsultaReporteEstadoCuentaCliente.Registros.Read())
            //    {
            //        JObject JMovimientoD = new JObject();
            //        JMovimientoD.Add("FECHAMOVD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaMovimientoD"]));
            //        JMovimientoD.Add("SERIED", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SerieD"]));
            //        JMovimientoD.Add("FOLIOD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FolioD"]));
            //        JMovimientoD.Add("CONCEPTOD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["ConceptoD"]));
            //        JMovimientoD.Add("CARGOSD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["CargosD"]));
            //        JMovimientoD.Add("ABONOSD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["AbonosD"]));
            //        JMovimientoD.Add("SALDODOCD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["SaldoDocumentoD"]));
            //        JMovimientoD.Add("FECHAVEND", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["FechaVencimientoD"]));
            //        JMovimientoD.Add("DIASVENCD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["DiasVencidosD"]));
            //        JMovimientoD.Add("MONEDAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TMD"]));
            //        JMovimientoD.Add("TCD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["TCD"]));
            //        JMovimientoD.Add("REFERENCIAD", Convert.ToString(ConsultaReporteEstadoCuentaCliente.Registros["REFD"]));
            //        JAMovimientosD.Add(JMovimientoD);
            //    }
            //    Modelo.Add("MovimientosD", JAMovimientosD);
            //}


            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));


        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a base de datos"));
        }


        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string BuscarRazonSocial(string pRazonSocial, int pIdSucursal)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        COrganizacion jsonRazonSocial = new COrganizacion();
        jsonRazonSocial.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial_Reportes";
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonRazonSocial.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdCliente, int pIdSucursal)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdReporteCrediticioConsultaPagos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = (HttpContext.Current.Session["IdUsuario"]);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturasPendientesConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdCliente, int pIdSucursal)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdReporteCrediticioConsultaFacturasPendientes", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = (HttpContext.Current.Session["IdUsuario"]);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProyectosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdCliente, int pEstatusProyecto, int pIdSucursal)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdReporteCrediticioConsultaProyectos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pEstatusProyecto", SqlDbType.Int).Value = pEstatusProyecto;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = (HttpContext.Current.Session["IdUsuario"]);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturacionConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdCliente, int pIdSucursal)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdReporteCrediticioConsultaFacturacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = (HttpContext.Current.Session["IdUsuario"]);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtieneRelacionSaldos(int pIdSucursal, string pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioCliente = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioClienteRelacionSaldos";
            //ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioCliente.Llena(ConexionBaseDatos);

            JArray JASaldos = new JArray();
            decimal TotalGeneral = 0;
            decimal TotalCorriente = 0;
            decimal TotalUnoTreintaPesos = 0;
            decimal TotalTreintaSesentaPesos = 0;
            decimal TotalSesentaNoventaPesos = 0;
            decimal TotalMasNoventaPesos = 0;
            decimal TotalVencidoPesos = 0;

            if (pIdSucursal == 0)
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
            {
                JObject JSaldos = new JObject();
                JSaldos.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["RazonSocial"]));
                JSaldos.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"]));
                JSaldos.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["CorrientePesos"]));
                JSaldos.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"]));
                JSaldos.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"]));
                JSaldos.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"]));
                JSaldos.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"]));
                JSaldos.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"]));
                JSaldos.Add("GESTOR", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Gestor"]));
                JASaldos.Add(JSaldos);
                TotalGeneral = TotalGeneral + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalCorriente = TotalCorriente + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["CorrientePesos"].ToString().Replace("$", ""));
                TotalUnoTreintaPesos = TotalUnoTreintaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"].ToString().Replace("$", ""));
                TotalTreintaSesentaPesos = TotalTreintaSesentaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"].ToString().Replace("$", ""));
                TotalSesentaNoventaPesos = TotalSesentaNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"].ToString().Replace("$", ""));
                TotalMasNoventaPesos = TotalMasNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"].ToString().Replace("$", ""));
                TotalVencidoPesos = TotalVencidoPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"].ToString().Replace("$", ""));
            }
            Modelo.Add("Saldos", JASaldos);
            Modelo.Add("TotalGeneral", "$" + TotalGeneral.ToString("#,###,##0.00"));
            Modelo.Add("TotalCorriente", "$" + TotalCorriente.ToString("#,###,##0.00"));
            Modelo.Add("TotalUnoTreintaPesos", "$" + TotalUnoTreintaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalTreintaSesentaPesos", "$" + TotalTreintaSesentaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalSesentaNoventaPesos", "$" + TotalSesentaNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalMasNoventaPesos", "$" + TotalMasNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalVencidoPesos", "$" + TotalVencidoPesos.ToString("#,###,##0.00"));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));


        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a base de datos"));
        }


        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneRelacionSaldosPesosGeneral(int pIdSucursal, string pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioCliente = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioClienteRelacionSaldosPesosGeneral";
            //ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioCliente.Llena(ConexionBaseDatos);

            JArray JASaldos = new JArray();
            decimal TotalGeneral = 0;
            decimal TotalCorriente = 0;
            decimal TotalUnoTreintaPesos = 0;
            decimal TotalTreintaSesentaPesos = 0;
            decimal TotalSesentaNoventaPesos = 0;
            decimal TotalMasNoventaPesos = 0;
            decimal TotalVencidoPesos = 0;

            if (pIdSucursal == 0)
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
            {
                JObject JSaldos = new JObject();
                JSaldos.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["RazonSocial"]));
                JSaldos.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"]));
                JSaldos.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["CorrientePesos"]));
                JSaldos.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"]));
                JSaldos.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"]));
                JSaldos.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"]));
                JSaldos.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"]));
                JSaldos.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"]));
                JASaldos.Add(JSaldos);
                TotalGeneral = TotalGeneral + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalCorriente = TotalCorriente + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["CorrientePesos"].ToString().Replace("$", ""));
                TotalUnoTreintaPesos = TotalUnoTreintaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"].ToString().Replace("$", ""));
                TotalTreintaSesentaPesos = TotalTreintaSesentaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"].ToString().Replace("$", ""));
                TotalSesentaNoventaPesos = TotalSesentaNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"].ToString().Replace("$", ""));
                TotalMasNoventaPesos = TotalMasNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"].ToString().Replace("$", ""));
                TotalVencidoPesos = TotalVencidoPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"].ToString().Replace("$", ""));
            }
            Modelo.Add("Saldos", JASaldos);
            Modelo.Add("TotalGeneral", "$" + TotalGeneral.ToString("#,###,##0.00"));
            Modelo.Add("TotalCorriente", "$" + TotalCorriente.ToString("#,###,##0.00"));
            Modelo.Add("TotalUnoTreintaPesos", "$" + TotalUnoTreintaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalTreintaSesentaPesos", "$" + TotalTreintaSesentaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalSesentaNoventaPesos", "$" + TotalSesentaNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalMasNoventaPesos", "$" + TotalMasNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalVencidoPesos", "$" + TotalVencidoPesos.ToString("#,###,##0.00"));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));


        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a base de datos"));
        }


        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneRelacionSaldosPesosGeneralTCD(int pIdSucursal, string pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioCliente = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioClienteRelacionSaldosPesosGeneralTCD";
            //ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioCliente.Llena(ConexionBaseDatos);

            JArray JASaldos = new JArray();
            decimal TotalGeneral = 0;
            decimal TotalCorriente = 0;
            decimal TotalUnoTreintaPesos = 0;
            decimal TotalTreintaSesentaPesos = 0;
            decimal TotalSesentaNoventaPesos = 0;
            decimal TotalMasNoventaPesos = 0;
            decimal TotalVencidoPesos = 0;

            if (pIdSucursal == 0)
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
            {
                JObject JSaldos = new JObject();
                JSaldos.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["RazonSocial"]));
                JSaldos.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"]));
                JSaldos.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["CorrientePesos"]));
                JSaldos.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"]));
                JSaldos.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"]));
                JSaldos.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"]));
                JSaldos.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"]));
                JSaldos.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"]));
                JSaldos.Add("GESTOR", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Gestor"]));
                JASaldos.Add(JSaldos);
                TotalGeneral = TotalGeneral + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalCorriente = TotalCorriente + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["CorrientePesos"].ToString().Replace("$", ""));
                TotalUnoTreintaPesos = TotalUnoTreintaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaPesos"].ToString().Replace("$", ""));
                TotalTreintaSesentaPesos = TotalTreintaSesentaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaPesos"].ToString().Replace("$", ""));
                TotalSesentaNoventaPesos = TotalSesentaNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaPesos"].ToString().Replace("$", ""));
                TotalMasNoventaPesos = TotalMasNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaPesos"].ToString().Replace("$", ""));
                TotalVencidoPesos = TotalVencidoPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoPesos"].ToString().Replace("$", ""));
            }
            Modelo.Add("Saldos", JASaldos);
            Modelo.Add("TotalGeneral", "$" + TotalGeneral.ToString("#,###,##0.00"));
            Modelo.Add("TotalCorriente", "$" + TotalCorriente.ToString("#,###,##0.00"));
            Modelo.Add("TotalUnoTreintaPesos", "$" + TotalUnoTreintaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalTreintaSesentaPesos", "$" + TotalTreintaSesentaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalSesentaNoventaPesos", "$" + TotalSesentaNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalMasNoventaPesos", "$" + TotalMasNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalVencidoPesos", "$" + TotalVencidoPesos.ToString("#,###,##0.00"));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));


        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a base de datos"));
        }


        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneRelacionSaldosDolares(int pIdSucursal, string pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioCliente = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioClienteRelacionSaldosDolares";
            //ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioCliente.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioCliente.Llena(ConexionBaseDatos);

            JArray JASaldosDolares = new JArray();
            decimal TotalGeneralDolares = 0;
            decimal TotalCorrienteDolares = 0;
            decimal TotalUnoTreintaDolares = 0;
            decimal TotalTreintaSesentaDolares = 0;
            decimal TotalSesentaNoventaDolares = 0;
            decimal TotalMasNoventaDolares = 0;
            decimal TotalVencidoDolares = 0;
            if (pIdSucursal == 0)
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioCliente.Registros.Read())
            {
                JObject JSaldosDolares = new JObject();
                JSaldosDolares.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["RazonSocial"]));
                JSaldosDolares.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"]));
                JSaldosDolares.Add("CORRIENTEDOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["CorrienteDolares"]));
                JSaldosDolares.Add("UNOTREINTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaDolares"]));
                JSaldosDolares.Add("TREINTASESENTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaDolares"]));
                JSaldosDolares.Add("SESENTANOVENTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaDolares"]));
                JSaldosDolares.Add("MASNOVENTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaDolares"]));
                JSaldosDolares.Add("TOTALVENCIDODOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoDolares"]));
                JSaldosDolares.Add("GESTOR", Convert.ToString(ConsultaReporteEstadoCrediticioCliente.Registros["Gestor"]));
                JASaldosDolares.Add(JSaldosDolares);
                TotalGeneralDolares = TotalGeneralDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalCorrienteDolares = TotalCorrienteDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["CorrienteDolares"].ToString().Replace("$", ""));
                TotalUnoTreintaDolares = TotalUnoTreintaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["UnoTreintaDolares"].ToString().Replace("$", ""));
                TotalTreintaSesentaDolares = TotalTreintaSesentaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TreintaSesentaDolares"].ToString().Replace("$", ""));
                TotalSesentaNoventaDolares = TotalSesentaNoventaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["SesentaNoventaDolares"].ToString().Replace("$", ""));
                TotalMasNoventaDolares = TotalMasNoventaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["MasNoventaDolares"].ToString().Replace("$", ""));
                TotalVencidoDolares = TotalVencidoDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioCliente.Registros["TotalVencidoDolares"].ToString().Replace("$", ""));
            }
            ConsultaReporteEstadoCrediticioCliente.CerrarConsulta();
            Modelo.Add("SaldosDolares", JASaldosDolares);
            Modelo.Add("TotalGeneralDolares", "$" + TotalGeneralDolares.ToString("#,###,##0.00"));
            Modelo.Add("TotalCorrienteDolares", "$" + TotalCorrienteDolares.ToString("#,###,##0.00"));
            Modelo.Add("TotalUnoTreintaDolares", "$" + TotalUnoTreintaDolares.ToString("#,###,##0.00"));
            Modelo.Add("TotalTreintaSesentaDolares", "$" + TotalTreintaSesentaDolares.ToString("#,###,##0.00"));
            Modelo.Add("TotalSesentaNoventaDolares", "$" + TotalSesentaNoventaDolares.ToString("#,###,##0.00"));
            Modelo.Add("TotalMasNoventaDolares", "$" + TotalMasNoventaDolares.ToString("#,###,##0.00"));
            Modelo.Add("TotalVencidoDolares", "$" + TotalVencidoDolares.ToString("#,###,##0.00"));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));


        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a base de datos"));
        }


        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ImprimirDoc(int pIdFacturaEncabezado, string pTemplate)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        logoEmpresa = Empresa.Logo;

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
}