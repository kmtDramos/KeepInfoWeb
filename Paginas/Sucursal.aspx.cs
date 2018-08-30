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

public partial class Sucursal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridSucursal
        CJQGrid GridSucursal = new CJQGrid();
        GridSucursal.NombreTabla = "grdSucursal";
        GridSucursal.CampoIdentificador = "IdSucursal";
        GridSucursal.ColumnaOrdenacion = "Sucursal";
        GridSucursal.Metodo = "ObtenerSucursal";
        GridSucursal.TituloTabla = "Catálogo de sucursales";

        //IdSucursal
        CJQColumn ColIdSucursal = new CJQColumn();
        ColIdSucursal.Nombre = "IdSucursal";
        ColIdSucursal.Oculto = "true";
        ColIdSucursal.Encabezado = "IdSucursal";
        ColIdSucursal.Buscador = "false";
        GridSucursal.Columnas.Add(ColIdSucursal);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "150";
        ColSucursal.Alineacion = "left";
        GridSucursal.Columnas.Add(ColSucursal);

        //Empresa
        CJQColumn ColEmpresa = new CJQColumn();
        ColEmpresa.Nombre = "Empresa";
        ColEmpresa.Encabezado = "Empresa";
        ColEmpresa.Ancho = "200";
        ColEmpresa.Buscador = "false";
        ColEmpresa.Alineacion = "left";
        GridSucursal.Columnas.Add(ColEmpresa);

        //ClaveCuentaContable
        CJQColumn ColClaveCuentaContable = new CJQColumn();
        ColClaveCuentaContable.Nombre = "ClaveCuentaContable";
        ColClaveCuentaContable.Encabezado = "Clave de cuenta contable";
        ColClaveCuentaContable.Ancho = "50";
        ColClaveCuentaContable.Buscador = "false";
        ColClaveCuentaContable.Alineacion = "left";
        GridSucursal.Columnas.Add(ColClaveCuentaContable);

        //SeriesFacturas
        CJQColumn ColSeriesFacturas = new CJQColumn();
        ColSeriesFacturas.Nombre = "SerieFactura";
        ColSeriesFacturas.Encabezado = "Series facturas";
        ColSeriesFacturas.Etiquetado = "Imagen";
        ColSeriesFacturas.Imagen = "SerieFactura.png";
        ColSeriesFacturas.Estilo = "divImagenSerieFactura imgFormaConsultarSerieFactura";
        ColSeriesFacturas.Buscador = "false";
        ColSeriesFacturas.Ordenable = "false";
        ColSeriesFacturas.Ancho = "70";
        GridSucursal.Columnas.Add(ColSeriesFacturas);

        //SeriesNotaCredito
        CJQColumn ColSeriesNotaCredito = new CJQColumn();
        ColSeriesNotaCredito.Nombre = "SerieNotaCredito";
        ColSeriesNotaCredito.Encabezado = "Series NC";
        ColSeriesNotaCredito.Etiquetado = "Imagen";
        ColSeriesNotaCredito.Imagen = "SerieFactura.png";
        ColSeriesNotaCredito.Estilo = "divImagenSerieNotaCredito imgFormaConsultarSerieNotaCredito";
        ColSeriesNotaCredito.Buscador = "false";
        ColSeriesNotaCredito.Ordenable = "false";
        ColSeriesNotaCredito.Ancho = "70";
        GridSucursal.Columnas.Add(ColSeriesNotaCredito);
        
        //SeriesPagos
        CJQColumn ColSeriesPagos = new CJQColumn();
        ColSeriesPagos.Nombre = "SeriePagos";
        ColSeriesPagos.Encabezado = "Series CP";
        ColSeriesPagos.Etiquetado = "Imagen";
        ColSeriesPagos.Imagen = "SerieFactura.png";
        ColSeriesPagos.Estilo = "divImagenSeriePago imgFormaConsultarSeriePago";
        ColSeriesPagos.Buscador = "false";
        ColSeriesPagos.Ordenable = "false";
        ColSeriesPagos.Ancho = "70";
        GridSucursal.Columnas.Add(ColSeriesPagos);
        
        //RutaArchivos
        CJQColumn Timbrado = new CJQColumn();
        Timbrado.Nombre = "Timbrado";
        Timbrado.Encabezado = "Ruta DFDI";
        Timbrado.Ordenable = "true";
        Timbrado.Ancho = "70";
        Timbrado.Etiquetado = "EstatusRuta";
        Timbrado.Buscador = "false";
        Timbrado.StoredProcedure.CommandText = "spc_ManejadorConvertirAPedido_Consulta";
        GridSucursal.Columnas.Add(Timbrado);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridSucursal.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarSucursal";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridSucursal.Columnas.Add(ColConsultar);

        //DivisionAsignada
        CJQColumn ColDivisionAsignada = new CJQColumn();
        ColDivisionAsignada.Nombre = "DivisionAsignada";
        ColDivisionAsignada.Encabezado = "División";
        ColDivisionAsignada.Etiquetado = "Imagen";
        ColDivisionAsignada.Imagen = "refrescar.png";
        ColDivisionAsignada.Estilo = "divImagenConsultar imgFormaDivisionAsignada";
        ColDivisionAsignada.Buscador = "false";
        ColDivisionAsignada.Ordenable = "false";
        ColDivisionAsignada.Ancho = "50";
        GridSucursal.Columnas.Add(ColDivisionAsignada);

        //CuentaBancariaAsignada
        CJQColumn ColCuentaBancariaAsignada = new CJQColumn();
        ColCuentaBancariaAsignada.Nombre = "CuentaBancariaAsignada";
        ColCuentaBancariaAsignada.Encabezado = "Cuenta bancaria";
        ColCuentaBancariaAsignada.Etiquetado = "Imagen";
        ColCuentaBancariaAsignada.Imagen = "refrescar.png";
        ColCuentaBancariaAsignada.Estilo = "divImagenConsultar imgFormaCuentaBancariaAsignada";
        ColCuentaBancariaAsignada.Buscador = "false";
        ColCuentaBancariaAsignada.Ordenable = "false";
        ColCuentaBancariaAsignada.Ancho = "50";
        GridSucursal.Columnas.Add(ColCuentaBancariaAsignada);

        //ConexionContpaq
        CJQColumn ColConexionContpaq = new CJQColumn();
        ColConexionContpaq.Nombre = "ConexionContpaq";
        ColConexionContpaq.Encabezado = "Conexión CONTPAQ";
        ColConexionContpaq.Etiquetado = "Imagen";
        ColConexionContpaq.Imagen = "Contpaq.png";
        ColConexionContpaq.Estilo = "divImagenConsultar imgFormaAsignarConexionContpaq";
        ColConexionContpaq.Buscador = "false";
        ColConexionContpaq.Ordenable = "false";
        ColConexionContpaq.Ancho = "50";
        GridSucursal.Columnas.Add(ColConexionContpaq);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSucursal", GridSucursal.GeneraGrid(), true);

        //GridSerieFactura
        CJQGrid GridSerieFactura = new CJQGrid();
        GridSerieFactura.NombreTabla = "grdSerieFactura";
        GridSerieFactura.CampoIdentificador = "IdSerieFactura";
        GridSerieFactura.ColumnaOrdenacion = "SerieFactura";
        GridSerieFactura.TipoOrdenacion = "DESC";
        GridSerieFactura.Metodo = "ObtenerSerieFactura";
        GridSerieFactura.TituloTabla = "Series de factura de la sucursal";
        GridSerieFactura.GenerarFuncionFiltro = false;
        GridSerieFactura.GenerarFuncionTerminado = false;
        GridSerieFactura.Altura = 300;
        GridSerieFactura.Ancho = 600;
        GridSerieFactura.NumeroRegistros = 10;
        GridSerieFactura.RangoNumeroRegistros = "10,20,30";

        //IdSerieFactura
        CJQColumn ColIdSerieFactura = new CJQColumn();
        ColIdSerieFactura.Nombre = "IdSerieFactura";
        ColIdSerieFactura.Oculto = "true";
        ColIdSerieFactura.Encabezado = "IdSerieFactura";
        ColIdSerieFactura.Buscador = "false";
        GridSerieFactura.Columnas.Add(ColIdSerieFactura);

        //SerieFactura
        CJQColumn ColSerieFactura = new CJQColumn();
        ColSerieFactura.Nombre = "SerieFactura";
        ColSerieFactura.Encabezado = "SerieFactura";
        ColSerieFactura.Buscador = "false";
        ColSerieFactura.Alineacion = "left";
        ColSerieFactura.Ancho = "80";
        GridSerieFactura.Columnas.Add(ColSerieFactura);

        //Parcialidad
        CJQColumn ColParcialidad = new CJQColumn();
        ColParcialidad.Nombre = "EsParcialidad";
        ColParcialidad.Encabezado = "Parcialidad";
        ColParcialidad.Buscador = "false";
        ColParcialidad.Alineacion = "center";
        ColParcialidad.Ancho = "25";
        GridSerieFactura.Columnas.Add(ColParcialidad);

        //Baja
        CJQColumn ColBajaSerieFactura = new CJQColumn();
        ColBajaSerieFactura.Nombre = "AI";
        ColBajaSerieFactura.Encabezado = "A/I";
        ColBajaSerieFactura.Ordenable = "false";
        ColBajaSerieFactura.Etiquetado = "A/I";
        ColBajaSerieFactura.Ancho = "60";
        ColBajaSerieFactura.Buscador = "true";
        ColBajaSerieFactura.TipoBuscador = "Combo";
        ColBajaSerieFactura.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridSerieFactura.Columnas.Add(ColBajaSerieFactura);

        //ConsultarSerieFactura
        CJQColumn ColConsultarSerieFactura = new CJQColumn();
        ColConsultarSerieFactura.Nombre = "Consultar";
        ColConsultarSerieFactura.Encabezado = "Ver";
        ColConsultarSerieFactura.Etiquetado = "ImagenConsultar";
        ColConsultarSerieFactura.Estilo = "divImagenConsultar ConsultarSerieFactura";
        ColConsultarSerieFactura.Buscador = "false";
        ColConsultarSerieFactura.Ordenable = "false";
        ColConsultarSerieFactura.Ancho = "25";
        GridSerieFactura.Columnas.Add(ColConsultarSerieFactura);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSerieFactura", GridSerieFactura.GeneraGrid(), true);

        //GridSerieNotaCredito
        CJQGrid GridSerieNotaCredito = new CJQGrid();
        GridSerieNotaCredito.NombreTabla = "grdSerieNotaCredito";
        GridSerieNotaCredito.CampoIdentificador = "IdSerieNotaCredito";
        GridSerieNotaCredito.ColumnaOrdenacion = "SerieNotaCredito";
        GridSerieNotaCredito.TipoOrdenacion = "DESC";
        GridSerieNotaCredito.Metodo = "ObtenerSerieNotaCredito";
        GridSerieNotaCredito.TituloTabla = "Series de notas de crédito de la sucursal";
        GridSerieNotaCredito.GenerarFuncionFiltro = false;
        GridSerieNotaCredito.GenerarFuncionTerminado = false;
        GridSerieNotaCredito.Altura = 300;
        GridSerieNotaCredito.Ancho = 600;
        GridSerieNotaCredito.NumeroRegistros = 10;
        GridSerieNotaCredito.RangoNumeroRegistros = "10,20,30";

        //IdSerieNotaCredito
        CJQColumn ColIdSerieNotaCredito = new CJQColumn();
        ColIdSerieNotaCredito.Nombre = "IdSerieNotaCredito";
        ColIdSerieNotaCredito.Oculto = "true";
        ColIdSerieNotaCredito.Encabezado = "IdSerieNotaCredito";
        ColIdSerieNotaCredito.Buscador = "false";
        GridSerieNotaCredito.Columnas.Add(ColIdSerieNotaCredito);

        //SerieNotaCredito
        CJQColumn ColSerieNotaCredito = new CJQColumn();
        ColSerieNotaCredito.Nombre = "SerieNotaCredito";
        ColSerieNotaCredito.Encabezado = "SerieNotaCredito";
        ColSerieNotaCredito.Buscador = "false";
        ColSerieNotaCredito.Alineacion = "left";
        ColSerieNotaCredito.Ancho = "80";
        GridSerieNotaCredito.Columnas.Add(ColSerieNotaCredito);

        //Baja
        CJQColumn ColBajaSerieNotaCredito = new CJQColumn();
        ColBajaSerieNotaCredito.Nombre = "AI";
        ColBajaSerieNotaCredito.Encabezado = "A/I";
        ColBajaSerieNotaCredito.Ordenable = "false";
        ColBajaSerieNotaCredito.Etiquetado = "A/I";
        ColBajaSerieNotaCredito.Ancho = "60";
        ColBajaSerieNotaCredito.Buscador = "true";
        ColBajaSerieNotaCredito.TipoBuscador = "Combo";
        ColBajaSerieNotaCredito.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridSerieNotaCredito.Columnas.Add(ColBajaSerieNotaCredito);

        //ConsultarSerieNotaCredito
        CJQColumn ColConsultarSerieNotaCredito = new CJQColumn();
        ColConsultarSerieNotaCredito.Nombre = "Consultar";
        ColConsultarSerieNotaCredito.Encabezado = "Ver";
        ColConsultarSerieNotaCredito.Etiquetado = "ImagenConsultar";
        ColConsultarSerieNotaCredito.Estilo = "divImagenConsultar ConsultarSerieNotaCredito";
        ColConsultarSerieNotaCredito.Buscador = "false";
        ColConsultarSerieNotaCredito.Ordenable = "false";
        ColConsultarSerieNotaCredito.Ancho = "25";
        GridSerieNotaCredito.Columnas.Add(ColConsultarSerieNotaCredito);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSerieNotaCredito", GridSerieNotaCredito.GeneraGrid(), true);

        //GridSeriePago
        CJQGrid GridSeriePago = new CJQGrid();
        GridSeriePago.NombreTabla = "grdSeriePago";
        GridSeriePago.CampoIdentificador = "IdSeriePago";
        GridSeriePago.ColumnaOrdenacion = "SerieComplementoPago";
        GridSeriePago.TipoOrdenacion = "DESC";
        GridSeriePago.Metodo = "ObtenerSeriePago";
        GridSeriePago.TituloTabla = "Series de Complemento de pago de la sucursal";
        GridSeriePago.GenerarFuncionFiltro = false;
        GridSeriePago.GenerarFuncionTerminado = false;
        GridSeriePago.Altura = 300;
        GridSeriePago.Ancho = 600;
        GridSeriePago.NumeroRegistros = 10;
        GridSeriePago.RangoNumeroRegistros = "10,20,30";

        //IdSeriePago
        CJQColumn ColIdSeriePago = new CJQColumn();
        ColIdSeriePago.Nombre = "IdSeriePago";
        ColIdSeriePago.Oculto = "true";
        ColIdSeriePago.Encabezado = "IdSeriePago";
        ColIdSeriePago.Buscador = "false";
        GridSeriePago.Columnas.Add(ColIdSeriePago);

        //SeriePago
        CJQColumn ColSeriePago = new CJQColumn();
        ColSeriePago.Nombre = "SerieComplementoPago";
        ColSeriePago.Encabezado = "SeriePago";
        ColSeriePago.Buscador = "false";
        ColSeriePago.Alineacion = "left";
        ColSeriePago.Ancho = "80";
        GridSeriePago.Columnas.Add(ColSeriePago);

        //Baja
        CJQColumn ColBajaSeriePago = new CJQColumn();
        ColBajaSeriePago.Nombre = "AI";
        ColBajaSeriePago.Encabezado = "A/I";
        ColBajaSeriePago.Ordenable = "false";
        ColBajaSeriePago.Etiquetado = "A/I";
        ColBajaSeriePago.Ancho = "60";
        ColBajaSeriePago.Buscador = "true";
        ColBajaSeriePago.TipoBuscador = "Combo";
        ColBajaSeriePago.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridSeriePago.Columnas.Add(ColBajaSeriePago);

        //ConsultarSeriePago
        CJQColumn ColConsultarSeriePago = new CJQColumn();
        ColConsultarSeriePago.Nombre = "Consultar";
        ColConsultarSeriePago.Encabezado = "Ver";
        ColConsultarSeriePago.Etiquetado = "ImagenConsultar";
        ColConsultarSeriePago.Estilo = "divImagenConsultar ConsultarSeriePago";
        ColConsultarSeriePago.Buscador = "false";
        ColConsultarSeriePago.Ordenable = "false";
        ColConsultarSeriePago.Ancho = "25";
        GridSeriePago.Columnas.Add(ColConsultarSeriePago);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSeriePago", GridSeriePago.GeneraGrid(), true);

        //GridRutaCFDI
        CJQGrid GridRutaCFDI = new CJQGrid();
        GridRutaCFDI.NombreTabla = "grdRutaCFDI";
        GridRutaCFDI.CampoIdentificador = "IdRutaCFDI";
        GridRutaCFDI.ColumnaOrdenacion = "RutaCFDI";
        GridRutaCFDI.TipoOrdenacion = "DESC";
        GridRutaCFDI.Metodo = "ObtenerRutaCFDI";
        GridRutaCFDI.TituloTabla = "Series de notas de crédito de la sucursal";
        GridRutaCFDI.GenerarFuncionFiltro = false;
        GridRutaCFDI.GenerarFuncionTerminado = false;
        GridRutaCFDI.Altura = 300;
        GridRutaCFDI.Ancho = 600;
        GridRutaCFDI.NumeroRegistros = 10;
        GridRutaCFDI.RangoNumeroRegistros = "10,20,30";

        //IdRutaCFDI
        CJQColumn ColIdRutaCFDI = new CJQColumn();
        ColIdRutaCFDI.Nombre = "IdRutaCFDI";
        ColIdRutaCFDI.Oculto = "true";
        ColIdRutaCFDI.Encabezado = "IdRutaCFDI";
        ColIdRutaCFDI.Buscador = "false";
        GridRutaCFDI.Columnas.Add(ColIdRutaCFDI);

        //RutaCFDI
        CJQColumn ColRutaCFDI = new CJQColumn();
        ColRutaCFDI.Nombre = "RutaCFDI";
        ColRutaCFDI.Encabezado = "RutaCFDI";
        ColRutaCFDI.Buscador = "false";
        ColRutaCFDI.Alineacion = "left";
        ColRutaCFDI.Ancho = "80";
        GridRutaCFDI.Columnas.Add(ColRutaCFDI);

        //Baja
        CJQColumn ColBajaRutaCFDI = new CJQColumn();
        ColBajaRutaCFDI.Nombre = "AI";
        ColBajaRutaCFDI.Encabezado = "A/I";
        ColBajaRutaCFDI.Ordenable = "false";
        ColBajaRutaCFDI.Etiquetado = "A/I";
        ColBajaRutaCFDI.Ancho = "60";
        ColBajaRutaCFDI.Buscador = "true";
        ColBajaRutaCFDI.TipoBuscador = "Combo";
        ColBajaRutaCFDI.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridRutaCFDI.Columnas.Add(ColBajaRutaCFDI);

        //ConsultarRutaCFDI
        CJQColumn ColConsultarRutaCFDI = new CJQColumn();
        ColConsultarRutaCFDI.Nombre = "Consultar";
        ColConsultarRutaCFDI.Encabezado = "Ver";
        ColConsultarRutaCFDI.Etiquetado = "ImagenConsultar";
        ColConsultarRutaCFDI.Estilo = "divImagenConsultar ConsultarRutaCFDI";
        ColConsultarRutaCFDI.Buscador = "false";
        ColConsultarRutaCFDI.Ordenable = "false";
        ColConsultarRutaCFDI.Ancho = "25";
        GridRutaCFDI.Columnas.Add(ColConsultarRutaCFDI);

        ClientScript.RegisterStartupScript(this.GetType(), "grdRutaCFDI", GridRutaCFDI.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSucursal(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSucursal, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSucursal", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSucursal", SqlDbType.VarChar, 250).Value = pSucursal;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSerieFactura(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSucursal, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSerieFactura", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("Baja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSerieNotaCredito(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSucursal, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSerieNotaCredito", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("Baja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSeriePago(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSucursal, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSeriePago", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("Baja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerRutaCFDI(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSucursal, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdRutaCFDI", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdSucursal", SqlDbType.Int).Value = pIdSucursal;
        Stored.Parameters.Add("Baja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarSucursal(string pSucursal)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonSucursal = new CJson();
        jsonSucursal.StoredProcedure.CommandText = "sp_Sucursal_Consultar_FiltroPorSucursal";
        jsonSucursal.StoredProcedure.Parameters.AddWithValue("@pSucursal", pSucursal);
        return jsonSucursal.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarCuentaContableIVA(string pCuentaContable)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCuentaContable jsonCuentaContable = new CCuentaContable();
        jsonCuentaContable.StoredProcedure.CommandText = "sp_Proveedor_ConsultarCuentaContable";
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", pCuentaContable);
        return jsonCuentaContable.ObtenerJsonCuentaContable(ConexionBaseDatos);

    }

    [WebMethod]
    public static string CambiarEstatus(int pIdSucursal, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSucursal Sucursal = new CSucursal();
            Sucursal.IdSucursal = pIdSucursal;
            Sucursal.Baja = pBaja;
            Sucursal.Eliminar(ConexionBaseDatos);
            respuesta = "0|SucursalEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusCuentaBancaria(int pIdCuentaBancaria, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
            CuentaBancaria.IdCuentaBancaria = pIdCuentaBancaria;
            CuentaBancaria.Baja = pBaja;
            CuentaBancaria.Eliminar(ConexionBaseDatos);
            respuesta = "0|CuentaBancariaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusSerieFactura(int pIdSerieFactura, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSerieFactura SerieFactura = new CSerieFactura();
            SerieFactura.IdSerieFactura = pIdSerieFactura;
            SerieFactura.Baja = pBaja;
            SerieFactura.Eliminar(ConexionBaseDatos);
            respuesta = "0|SerieFacturaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusSerieNotaCredito(int pIdSerieNotaCredito, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSerieNotaCredito SerieNotaCredito = new CSerieNotaCredito();
            SerieNotaCredito.IdSerieNotaCredito = pIdSerieNotaCredito;
            SerieNotaCredito.Baja = pBaja;
            SerieNotaCredito.Eliminar(ConexionBaseDatos);
            respuesta = "0|SerieNotaCreditoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusSeriePago(int pIdSeriePago, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSerieComplementoPago SeriePago = new CSerieComplementoPago();
            SeriePago.IdSerieComplementoPago = pIdSeriePago;
            SeriePago.Baja = pBaja;
            SeriePago.Eliminar(ConexionBaseDatos);
            respuesta = "0|SeriePagoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusRutaCFDI(int pIdRutaCFDI, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CRutaCFDI RutaCFDI = new CRutaCFDI();
            RutaCFDI.IdRutaCFDI = pIdRutaCFDI;
            RutaCFDI.Baja = pBaja;
            RutaCFDI.Eliminar(ConexionBaseDatos);
            respuesta = "0|RutaCFDIEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerFormaAgregarSucursal()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CEmpresa Empresa = new CEmpresa();
            JArray JEmpresas = new JArray();

            foreach (CEmpresa oEmpresa in Empresa.LlenaObjetos(ConexionBaseDatos))
            {
                JObject JEmpresa = new JObject();
                JEmpresa.Add(new JProperty("IdEmpresa", oEmpresa.IdEmpresa));
                JEmpresa.Add(new JProperty("Empresa", oEmpresa.Empresa));
                JEmpresas.Add(JEmpresa);
            }

            Modelo.Add(new JProperty("Empresas", JEmpresas));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add("IVAs", CJson.ObtenerJsonIVA(ConexionBaseDatos));

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
    public static string ObtenerFormaDivisionAsignada(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        CSucursal Sucursal = new CSucursal();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("IdSucursal", pIdSucursal);
            Modelo.Add("DivisionesDispobiles", Sucursal.ObtenerJsonDivisionesDisponibles(pIdSucursal, ConexionBaseDatos));
            Modelo.Add("DivisionesAsignadas", Sucursal.ObtenerJsonDivisionesAsignadas(pIdSucursal, ConexionBaseDatos));

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
    public static string ObtenerFormaCuentaBancariaAsignada(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        CSucursal Sucursal = new CSucursal();

        if (respuesta == "Conexion Establecida")
        {
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add("IdSucursal", pIdSucursal);
            Modelo.Add("CuentaBancariasDispobiles", Sucursal.ObtenerJsonCuentaBancariasDisponibles(pIdSucursal, ConexionBaseDatos));
            Modelo.Add("CuentaBancariasAsignadas", Sucursal.ObtenerJsonCuentaBancariasAsignadas(pIdSucursal, ConexionBaseDatos));
            Modelo.Add("Sucursal", Sucursal.Sucursal);
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
    public static string ObtenerDireccionFiscalEmpresa(int pIdEmpresa)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(pIdEmpresa, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonEmpresa(Modelo, pIdEmpresa, ConexionBaseDatos);

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
    public static string ObtenerDireccionExpedicionEmpresa(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("Calle", Sucursal.Calle));
            Modelo.Add(new JProperty("NumeroExterior", Sucursal.NumeroExterior));
            Modelo.Add(new JProperty("NumeroInterior", Sucursal.NumeroInterior));
            Modelo.Add(new JProperty("Colonia", Sucursal.Colonia));
            Modelo.Add(new JProperty("CodigoPostal", Sucursal.CodigoPostal));
            Modelo.Add(new JProperty("Referencia", Sucursal.Referencia));

            CMunicipio Municipio = new CMunicipio();
            Municipio.LlenaObjeto(Sucursal.IdMunicipio, ConexionBaseDatos);

            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);

            Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Sucursal.IdMunicipio, Sucursal.IdLocalidad, ConexionBaseDatos));
            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Municipio.IdEstado, Sucursal.IdMunicipio, ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Estado.IdPais, Estado.IdEstado, ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Estado.IdPais, ConexionBaseDatos));

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
    public static string ObtenerFormaSerieFacturaConsultar(int pIdSerieFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSerieFactura = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSerieFactura" }, ConexionBaseDatos) == "")
        {
            puedeEditarSerieFactura = 1;
        }
        oPermisos.Add("puedeEditarSerieFactura", puedeEditarSerieFactura);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CSerieFactura.ObtenerJsonSerieFactura(Modelo, pIdSerieFactura, ConexionBaseDatos);
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
    public static string ObtenerFormaSerieNotaCreditoConsultar(int pIdSerieNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSerieNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSerieNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEditarSerieNotaCredito = 1;
        }
        oPermisos.Add("puedeEditarSerieNotaCredito", puedeEditarSerieNotaCredito);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CSerieNotaCredito.ObtenerJsonSerieNotaCredito(Modelo, pIdSerieNotaCredito, ConexionBaseDatos);
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
    public static string ObtenerFormaSeriePago(int pIdSeriePago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSeriePago = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSeriePago" }, ConexionBaseDatos) == "")
        {
            puedeEditarSeriePago = 1;
        }
        oPermisos.Add("puedeEditarSeriePago", puedeEditarSeriePago);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CSerieComplementoPago.ObtenerJsonSeriePago(Modelo, pIdSeriePago, ConexionBaseDatos);
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
    public static string ObtenerFormaRutaCFDIConsultar(int pIdRutaCFDI)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarRutaCFDI = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarRutaCFDI" }, ConexionBaseDatos) == "")
        {
            puedeEditarRutaCFDI = 1;
        }
        oPermisos.Add("puedeEditarRutaCFDI", puedeEditarRutaCFDI);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CRutaCFDI.ObtenerJsonRutaCFDI(Modelo, pIdRutaCFDI, ConexionBaseDatos);
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
    public static string AgregarSucursal(Dictionary<string, object> pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CSucursal Sucursal = new CSucursal();
        Sucursal.Sucursal = Convert.ToString(pSucursal["Sucursal"]);
        Sucursal.IdEmpresa = Convert.ToInt32(pSucursal["IdEmpresa"]);
        Sucursal.Telefono = Convert.ToString(pSucursal["Telefono"]);
        Sucursal.Correo = Convert.ToString(pSucursal["Correo"]);
        Sucursal.ClaveCuentaContable = Convert.ToString(pSucursal["ClaveCuentaContable"]);
        Sucursal.IdTipoMoneda = Convert.ToInt32(pSucursal["IdTipoMoneda"]);
        Sucursal.Dominio = Convert.ToString(pSucursal["Dominio"]);
        Sucursal.IVAActual = Convert.ToDecimal(pSucursal["IVAActual"]);
        Sucursal.IdIVA = Convert.ToInt32(pSucursal["IdIVA"]);
        Sucursal.Alias = Convert.ToString(pSucursal["Alias"]);
        Sucursal.DireccionFiscal = Convert.ToBoolean(Convert.ToInt32(pSucursal["DireccionFiscal"]));
        if (Convert.ToInt32(pSucursal["DireccionFiscal"]) == 1)
        {
            Sucursal.Calle = "";
            Sucursal.NumeroExterior = "";
            Sucursal.NumeroInterior = "";
            Sucursal.Colonia = "";
            Sucursal.IdLocalidad = 0;
            Sucursal.CodigoPostal = "";
            Sucursal.Referencia = "";
            Sucursal.IdMunicipio = 0;
        }
        else
        {
            Sucursal.Calle = Convert.ToString(pSucursal["Calle"]);
            Sucursal.NumeroExterior = Convert.ToString(pSucursal["NumeroExterior"]);
            Sucursal.NumeroInterior = Convert.ToString(pSucursal["NumeroInterior"]);
            Sucursal.Colonia = Convert.ToString(pSucursal["Colonia"]);
            Sucursal.IdLocalidad = Convert.ToInt32(pSucursal["IdLocalidad"]);
            Sucursal.CodigoPostal = Convert.ToString(pSucursal["CodigoPostal"]);
            Sucursal.Referencia = Convert.ToString(pSucursal["Referencia"]);
            Sucursal.IdMunicipio = Convert.ToInt32(pSucursal["IdMunicipio"]);
        }

        string validacion = ValidarSucursal(Sucursal, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Sucursal.Agregar(ConexionBaseDatos);
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
    public static string AgregarDivisionSucursal(Dictionary<string, object> pDivision)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            try
            {
                JObject Modelo = new JObject();
                CSucursalDivision SucursalDivision = new CSucursalDivision();
                SucursalDivision.IdSucursal = Convert.ToInt32(pDivision["IdSucursal"]);
                SucursalDivision.BajaDivisionSucursal(ConexionBaseDatos);

                foreach (Dictionary<string, object> oDivision in (Array)pDivision["Divisiones"])
                {
                    SucursalDivision.IdDivision = Convert.ToInt32(oDivision["IdDivision"]);
                    SucursalDivision.EnrolarDivisionSucursal(ConexionBaseDatos);
                }

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            catch (Exception ex)
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", ex.Message));
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
    public static string AgregarCuentaBancariaSucursal(Dictionary<string, object> pCuentaBancaria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            try
            {
                JObject Modelo = new JObject();
                CSucursalCuentaBancaria SucursalCuentaBancaria = new CSucursalCuentaBancaria();
                SucursalCuentaBancaria.IdSucursal = Convert.ToInt32(pCuentaBancaria["IdSucursal"]);
                SucursalCuentaBancaria.BajaCuentaBancariaSucursal(ConexionBaseDatos);

                foreach (Dictionary<string, object> oCuentaBancaria in (Array)pCuentaBancaria["CuentaBancarias"])
                {
                    SucursalCuentaBancaria.IdCuentaBancaria = Convert.ToInt32(oCuentaBancaria["IdCuentaBancaria"]);
                    SucursalCuentaBancaria.EnrolarCuentaBancariaSucursal(ConexionBaseDatos);
                }

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            catch (Exception ex)
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", ex.Message));
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
    public static string ObtenerFormaConsultarSucursal(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSucursal = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSucursal" }, ConexionBaseDatos) == "")
        {
            puedeEditarSucursal = 1;
        }
        oPermisos.Add("puedeEditarSucursal", puedeEditarSucursal);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add(new JProperty("Telefono", Sucursal.Telefono));
            Modelo.Add(new JProperty("Correo", Sucursal.Correo));
            Modelo.Add(new JProperty("ClaveCuentaContable", Sucursal.ClaveCuentaContable));
            Modelo.Add(new JProperty("Dominio", Sucursal.Dominio));
            Modelo.Add(new JProperty("IVA", Sucursal.IVAActual.ToString() + "%"));
            Modelo.Add(new JProperty("IdTipoMoneda", Sucursal.IdTipoMoneda));
            Modelo.Add(new JProperty("Alias", Sucursal.Alias));
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(Sucursal.IdTipoMoneda, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);
            Modelo.Add(new JProperty("Empresa", Empresa.Empresa));

            CMunicipio Municipio = new CMunicipio();
            CLocalidad Localidad = new CLocalidad();

            if (Sucursal.DireccionFiscal)
            {
                Modelo.Add(new JProperty("Calle", Empresa.Calle));
                Modelo.Add(new JProperty("NumeroExterior", Empresa.NumeroExterior));
                Modelo.Add(new JProperty("NumeroInterior", Empresa.NumeroInterior));
                Modelo.Add(new JProperty("Colonia", Empresa.Colonia));
                Modelo.Add(new JProperty("CodigoPostal", Empresa.CodigoPostal));
                Modelo.Add(new JProperty("Referencia", Empresa.Referencia));
                Municipio.LlenaObjeto(Empresa.IdMunicipio, ConexionBaseDatos);
                Modelo.Add(new JProperty("Municipio", Municipio.Municipio));

                Localidad.LlenaObjeto(Empresa.IdLocalidad, ConexionBaseDatos);
                Modelo.Add(new JProperty("Localidad", Localidad.Localidad));
            }
            else
            {
                Modelo.Add(new JProperty("Calle", Sucursal.Calle));
                Modelo.Add(new JProperty("NumeroExterior", Sucursal.NumeroExterior));
                Modelo.Add(new JProperty("NumeroInterior", Sucursal.NumeroInterior));
                Modelo.Add(new JProperty("Colonia", Sucursal.Colonia));
                Modelo.Add(new JProperty("IdLocalidad", Sucursal.IdLocalidad));
                Modelo.Add(new JProperty("CodigoPostal", Sucursal.CodigoPostal));
                Modelo.Add(new JProperty("Referencia", Sucursal.Referencia));
                Municipio.LlenaObjeto(Sucursal.IdMunicipio, ConexionBaseDatos);
                Modelo.Add(new JProperty("Municipio", Municipio.Municipio));

                Localidad.LlenaObjeto(Sucursal.IdLocalidad, ConexionBaseDatos);
                Modelo.Add(new JProperty("Localidad", Localidad.Localidad));
            }

            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
            Modelo.Add(new JProperty("Estado", Estado.Estado));

            CPais Pais = new CPais();
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
            Modelo.Add(new JProperty("Pais", Pais.Pais));

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
    public static string ObtenerFormaEditarSucursal(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSucursal = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSucursal" }, ConexionBaseDatos) == "")
        {
            puedeEditarSucursal = 1;
        }
        oPermisos.Add("puedeEditarSucursal", puedeEditarSucursal);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add(new JProperty("Telefono", Sucursal.Telefono));
            Modelo.Add(new JProperty("Correo", Sucursal.Correo));
            Modelo.Add(new JProperty("Dominio", Sucursal.Dominio));
            Modelo.Add(new JProperty("IVAActual", Sucursal.IVAActual.ToString()));
            Modelo.Add(new JProperty("ClaveCuentaContable", Sucursal.ClaveCuentaContable));
            Modelo.Add(new JProperty("DireccionFiscal", Sucursal.DireccionFiscal));
            Modelo.Add(new JProperty("IdTipoMoneda", Sucursal.IdTipoMoneda));
            Modelo.Add(new JProperty("Alias", Sucursal.Alias));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Sucursal.IdTipoMoneda), ConexionBaseDatos));
            Modelo.Add("IVAs", CJson.ObtenerJsonIVA(Convert.ToInt32(Sucursal.IdIVA), ConexionBaseDatos));
            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

            if (Sucursal.DireccionFiscal)
            {
                Modelo.Add(new JProperty("Calle", Empresa.Calle));
                Modelo.Add(new JProperty("NumeroExterior", Empresa.NumeroExterior));
                Modelo.Add(new JProperty("NumeroInterior", Empresa.NumeroInterior));
                Modelo.Add(new JProperty("Colonia", Empresa.Colonia));
                Modelo.Add(new JProperty("CodigoPostal", Empresa.CodigoPostal));
                Modelo.Add(new JProperty("Referencia", Empresa.Referencia));
                Modelo.Add(new JProperty("Empresa", Empresa.Empresa));

                CLocalidad Localidad = new CLocalidad();
                Localidad.LlenaObjeto(Empresa.IdLocalidad, ConexionBaseDatos);
                Modelo.Add(new JProperty("Localidad", Localidad.Localidad));

                CMunicipio Municipio = new CMunicipio();
                Municipio.LlenaObjeto(Empresa.IdMunicipio, ConexionBaseDatos);
                Modelo.Add(new JProperty("Municipio", Municipio.Municipio));

                CEstado Estado = new CEstado();
                Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
                Modelo.Add(new JProperty("Estado", Estado.Estado));

                CPais Pais = new CPais();
                Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
                Modelo.Add(new JProperty("Pais", Pais.Pais));
            }
            else
            {
                Modelo.Add(new JProperty("Calle", Sucursal.Calle));
                Modelo.Add(new JProperty("NumeroExterior", Sucursal.NumeroExterior));
                Modelo.Add(new JProperty("NumeroInterior", Sucursal.NumeroInterior));
                Modelo.Add(new JProperty("Colonia", Sucursal.Colonia));
                Modelo.Add(new JProperty("IdLocalidad", Sucursal.IdLocalidad));
                Modelo.Add(new JProperty("CodigoPostal", Sucursal.CodigoPostal));
                Modelo.Add(new JProperty("Referencia", Sucursal.Referencia));

                CMunicipio Municipio = new CMunicipio();
                Municipio.LlenaObjeto(Sucursal.IdMunicipio, ConexionBaseDatos);

                CEstado Estado = new CEstado();
                Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);

                Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Sucursal.IdMunicipio, Sucursal.IdLocalidad, ConexionBaseDatos));
                Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Municipio.IdEstado, Sucursal.IdMunicipio, ConexionBaseDatos));
                Modelo.Add("Estados", CJson.ObtenerJsonEstados(Estado.IdPais, Estado.IdEstado, ConexionBaseDatos));
                Modelo.Add("Paises", CJson.ObtenerJsonPaises(Estado.IdPais, ConexionBaseDatos));
            }

            JArray JEmpresas = new JArray();
            foreach (CEmpresa oEmpresa in Empresa.LlenaObjetos(ConexionBaseDatos))
            {
                JObject JEmpresa = new JObject();
                JEmpresa.Add(new JProperty("IdEmpresa", oEmpresa.IdEmpresa));
                JEmpresa.Add(new JProperty("Empresa", oEmpresa.Empresa));
                if (Empresa.IdEmpresa == oEmpresa.IdEmpresa)
                {
                    JEmpresa.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JEmpresa.Add(new JProperty("Selected", 0));
                }
                JEmpresas.Add(JEmpresa);
            }
            Modelo.Add(new JProperty("Empresas", JEmpresas));

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
    public static string ObtenerFormaEditarSerieFactura(int IdSerieFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSerieFactura = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSerieFactura" }, ConexionBaseDatos) == "")
        {
            puedeEditarSerieFactura = 1;
        }
        oPermisos.Add("puedeEditarSerieFactura", puedeEditarSerieFactura);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CSerieFactura.ObtenerJsonSerieFactura(Modelo, IdSerieFactura, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarSerieNotaCredito(int IdSerieNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSerieNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSerieNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeEditarSerieNotaCredito = 1;
        }
        oPermisos.Add("puedeEditarSerieNotaCredito", puedeEditarSerieNotaCredito);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CSerieNotaCredito.ObtenerJsonSerieNotaCredito(Modelo, IdSerieNotaCredito, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarSeriePago(int IdSeriePago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSeriePago = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSeriePago" }, ConexionBaseDatos) == "")
        {
            puedeEditarSeriePago = 1;
        }
        oPermisos.Add("puedeEditarSeriePago", puedeEditarSeriePago);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CSerieComplementoPago.ObtenerJsonSeriePago(Modelo, IdSeriePago, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarRutaCFDI(int IdRutaCFDI)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarRutaCFDI = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarRutaCFDI" }, ConexionBaseDatos) == "")
        {
            puedeEditarRutaCFDI = 1;
        }
        oPermisos.Add("puedeEditarRutaCFDI", puedeEditarRutaCFDI);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CRutaCFDI.ObtenerJsonRutaCFDI(Modelo, IdRutaCFDI, ConexionBaseDatos);
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
    public static string EditarSucursal(Dictionary<string, object> pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Convert.ToInt32(pSucursal["IdSucursal"]), ConexionBaseDatos);
        decimal IVA_Anterior = Sucursal.IVAActual;
        Sucursal.IdSucursal = Convert.ToInt32(pSucursal["IdSucursal"]);
        Sucursal.Sucursal = Convert.ToString(pSucursal["Sucursal"]);
        Sucursal.IdEmpresa = Convert.ToInt32(pSucursal["IdEmpresa"]);
        Sucursal.Telefono = Convert.ToString(pSucursal["Telefono"]);
        Sucursal.Correo = Convert.ToString(pSucursal["Correo"]);
        Sucursal.ClaveCuentaContable = Convert.ToString(pSucursal["ClaveCuentaContable"]);
        Sucursal.Dominio = Convert.ToString(pSucursal["Dominio"]);
        Sucursal.IVAActual = Convert.ToDecimal(pSucursal["IVAActual"]);
        Sucursal.DireccionFiscal = Convert.ToBoolean(pSucursal["DireccionFiscal"]);
        Sucursal.IdTipoMoneda = Convert.ToInt32(pSucursal["IdTipoMoneda"]);
        Sucursal.IdIVA = Convert.ToInt32(pSucursal["IdIVA"]);

        if (Sucursal.DireccionFiscal)
        {
            Sucursal.Calle = "";
            Sucursal.NumeroExterior = "";
            Sucursal.NumeroInterior = "";
            Sucursal.Colonia = "";
            Sucursal.IdLocalidad = 0;
            Sucursal.CodigoPostal = "";
            Sucursal.Referencia = "";
            Sucursal.IdMunicipio = 0;
            Sucursal.Alias = "";
        }
        else
        {
            Sucursal.Calle = Convert.ToString(pSucursal["Calle"]);
            Sucursal.NumeroExterior = Convert.ToString(pSucursal["NumeroExterior"]);
            Sucursal.NumeroInterior = Convert.ToString(pSucursal["NumeroInterior"]);
            Sucursal.Colonia = Convert.ToString(pSucursal["Colonia"]);
            Sucursal.IdLocalidad = Convert.ToInt32(pSucursal["IdLocalidad"]);
            Sucursal.CodigoPostal = Convert.ToString(pSucursal["CodigoPostal"]);
            Sucursal.Referencia = Convert.ToString(pSucursal["Referencia"]);
            Sucursal.IdMunicipio = Convert.ToInt32(pSucursal["IdMunicipio"]);
            Sucursal.Alias = Convert.ToString(pSucursal["Alias"]);
        }

        string validacion = ValidarSucursal(Sucursal, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Sucursal.Editar(ConexionBaseDatos);

            string cambioIVA = string.Empty;
            if (IVA_Anterior != Sucursal.IVAActual)
            {
                cambioIVA = "El IVA cambio de" + IVA_Anterior.ToString() + " a " + Sucursal.IVAActual.ToString();
            }

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Sucursal.IdSucursal;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico la sucursal. " + cambioIVA;
            HistorialGenerico.AgregarHistorialGenerico("Proveedor", ConexionBaseDatos);

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
    public static string EditarSerieFactura(Dictionary<string, object> pSerieFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CSerieFactura SerieFactura = new CSerieFactura();
        SerieFactura.LlenaObjeto(Convert.ToInt32(pSerieFactura["IdSerieFactura"]), ConexionBaseDatos);
        SerieFactura.IdSerieFactura = Convert.ToInt32(pSerieFactura["IdSerieFactura"]);
        SerieFactura.SerieFactura = Convert.ToString(pSerieFactura["SerieFactura"]);
        SerieFactura.Timbrado = Convert.ToBoolean(pSerieFactura["Timbrado"]);
        SerieFactura.EsParcialidad = Convert.ToBoolean(pSerieFactura["EsParcialidad"]);
        SerieFactura.EsVenta = Convert.ToBoolean(pSerieFactura["EsVenta"]);
        string validacion = ValidarSerieFactura(SerieFactura, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            SerieFactura.Editar(ConexionBaseDatos);

            SerieFactura.ActualizarParcialidadSucursal(SerieFactura.IdSerieFactura, SerieFactura.IdSucursal, Convert.ToInt32(pSerieFactura["EsParcialidad"]), ConexionBaseDatos);
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
    public static string EditarSerieNotaCredito(Dictionary<string, object> pSerieNotaCredito)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CSerieNotaCredito SerieNotaCredito = new CSerieNotaCredito();
        SerieNotaCredito.LlenaObjeto(Convert.ToInt32(pSerieNotaCredito["IdSerieNotaCredito"]), ConexionBaseDatos);
        SerieNotaCredito.IdSerieNotaCredito = Convert.ToInt32(pSerieNotaCredito["IdSerieNotaCredito"]);
        SerieNotaCredito.SerieNotaCredito = Convert.ToString(pSerieNotaCredito["SerieNotaCredito"]);
        string validacion = ValidarSerieNotaCredito(SerieNotaCredito, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            SerieNotaCredito.Editar(ConexionBaseDatos);
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
    public static string EditarSeriePago(Dictionary<string, object> pSeriePago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CSerieComplementoPago SeriePago = new CSerieComplementoPago();
        SeriePago.LlenaObjeto(Convert.ToInt32(pSeriePago["IdSeriePago"]), ConexionBaseDatos);
        SeriePago.IdSerieComplementoPago = Convert.ToInt32(pSeriePago["IdSeriePago"]);
        SeriePago.SerieComplementoPago = Convert.ToString(pSeriePago["SeriePago"]);
        string validacion = ValidarSeriePago(SeriePago, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            SeriePago.Editar(ConexionBaseDatos);
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
    public static string EditarRutaCFDI(Dictionary<string, object> pRutaCFDI)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        RutaCFDI.LlenaObjeto(Convert.ToInt32(pRutaCFDI["IdRutaCFDI"]), ConexionBaseDatos);
        RutaCFDI.IdRutaCFDI = Convert.ToInt32(pRutaCFDI["IdRutaCFDI"]);
        RutaCFDI.RutaCFDI = Convert.ToString(pRutaCFDI["RutaCFDI"]);
        RutaCFDI.TipoRuta = Convert.ToInt32(pRutaCFDI["TipoRuta"]);
        string validacion = ValidarRutaCFDI(RutaCFDI, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            RutaCFDI.Editar(ConexionBaseDatos);
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
    public static string ObtenerFormaConsultarCuentaBancaria(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSucursal = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSucursal" }, ConexionBaseDatos) == "")
        {
            puedeEditarSucursal = 1;
        }
        oPermisos.Add("puedeEditarSucursal", puedeEditarSucursal);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarSerieFactura(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSucursal = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSucursal" }, ConexionBaseDatos) == "")
        {
            puedeEditarSucursal = 1;
        }
        oPermisos.Add("puedeEditarSucursal", puedeEditarSucursal);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarSerieNotaCredito(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSucursal = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSucursal" }, ConexionBaseDatos) == "")
        {
            puedeEditarSucursal = 1;
        }
        oPermisos.Add("puedeEditarSucursal", puedeEditarSucursal);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarSeriePago(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSucursal = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSucursal" }, ConexionBaseDatos) == "")
        {
            puedeEditarSucursal = 1;
        }
        oPermisos.Add("puedeEditarSucursal", puedeEditarSucursal);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarRutaCFDI(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarSucursal = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarSucursal" }, ConexionBaseDatos) == "")
        {
            puedeEditarSucursal = 1;
        }
        oPermisos.Add("puedeEditarSucursal", puedeEditarSucursal);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAsignarConexionContpaq(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("BaseDatos", Sucursal.BaseDatosContpaq));
            Modelo.Add(new JProperty("Usuario", Sucursal.UsuarioContpaq));
            Modelo.Add(new JProperty("Contrasena", Sucursal.ContrasenaContpaq));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarCuentaBancaria(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarCuentaBancaria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarCuentaBancaria" }, ConexionBaseDatos) == "")
        {
            puedeAgregarCuentaBancaria = 1;
        }
        oPermisos.Add("puedeAgregarCuentaBancaria", puedeAgregarCuentaBancaria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));
            Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(ConexionBaseDatos));
            Modelo.Add("TipoBancos", CBanco.ObtenerJsonBanco(ConexionBaseDatos));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarSerieFactura(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarSerieFactura = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarSerieFactura" }, ConexionBaseDatos) == "")
        {
            puedeAgregarSerieFactura = 1;
        }
        oPermisos.Add("puedeAgregarSerieFactura", puedeAgregarSerieFactura);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarSerieNotaCredito(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarSerieNotaCredito = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarSerieNotaCredito" }, ConexionBaseDatos) == "")
        {
            puedeAgregarSerieNotaCredito = 1;
        }
        oPermisos.Add("puedeAgregarSerieNotaCredito", puedeAgregarSerieNotaCredito);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarSeriePago(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarSeriePago = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarSeriePago" }, ConexionBaseDatos) == "")
        {
            puedeAgregarSeriePago = 1;
        }
        oPermisos.Add("puedeAgregarSeriePago", puedeAgregarSeriePago);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarRutaCFDI(int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarRutaCFDI = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarRutaCFDI" }, ConexionBaseDatos) == "")
        {
            puedeAgregarRutaCFDI = 1;
        }
        oPermisos.Add("puedeAgregarRutaCFDI", puedeAgregarRutaCFDI);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(pIdSucursal, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdSucursal", Sucursal.IdSucursal));
            Modelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string AgregarSerieFactura(Dictionary<string, object> pSerieFactura)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CSerieFactura SerieFactura = new CSerieFactura();
        SerieFactura.IdSucursal = Convert.ToInt32(pSerieFactura["IdSucursal"]);
        SerieFactura.SerieFactura = Convert.ToString(pSerieFactura["SerieFactura"]);
        SerieFactura.FechaAlta = Convert.ToDateTime(DateTime.Now);
        SerieFactura.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        SerieFactura.Timbrado = Convert.ToBoolean(pSerieFactura["Timbrado"]);
        SerieFactura.EsParcialidad = Convert.ToBoolean(pSerieFactura["EsParcialidad"]);
        SerieFactura.EsVenta = Convert.ToBoolean(pSerieFactura["EsVenta"]);

        Dictionary<string, object> ParametrosCB = new Dictionary<string, object>();
        ParametrosCB.Add("IdSucursal", SerieFactura.IdSucursal);
        ParametrosCB.Add("SerieFactura", SerieFactura.SerieFactura);
        JObject oRespuesta = new JObject();
        if (SerieFactura.RevisarExisteRegistro(ParametrosCB, ConexionBaseDatos) == false)
        {
            SerieFactura.Agregar(ConexionBaseDatos);
            var IdSerieFactura = SerieFactura.IdSerieFactura;
            if (SerieFactura.EsParcialidad == true)
            {
                SerieFactura.AgregarActualizarParcialidadSucursal(IdSerieFactura, SerieFactura.IdSucursal, ConexionBaseDatos);

            }

            respuesta = "SerieFacturaAgregada";
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "Ya existe la serie dada de alta"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string AgregarSerieNotaCredito(Dictionary<string, object> pSerieNotaCredito)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CSerieNotaCredito SerieNotaCredito = new CSerieNotaCredito();
        SerieNotaCredito.IdSucursal = Convert.ToInt32(pSerieNotaCredito["IdSucursal"]);
        SerieNotaCredito.SerieNotaCredito = Convert.ToString(pSerieNotaCredito["SerieNotaCredito"]);
        SerieNotaCredito.FechaAlta = Convert.ToDateTime(DateTime.Now);
        SerieNotaCredito.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        Dictionary<string, object> ParametrosCB = new Dictionary<string, object>();
        ParametrosCB.Add("IdSucursal", SerieNotaCredito.IdSucursal);
        ParametrosCB.Add("SerieNotaCredito", SerieNotaCredito.SerieNotaCredito);
        JObject oRespuesta = new JObject();
        if (SerieNotaCredito.RevisarExisteRegistro(ParametrosCB, ConexionBaseDatos) == false)
        {
            SerieNotaCredito.Agregar(ConexionBaseDatos);
            respuesta = "SerieNotaCreditoAgregada";
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "Ya existe la serie"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string AgregarSeriePago(Dictionary<string, object> pSeriePago)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CSerieComplementoPago SeriePago = new CSerieComplementoPago();
        SeriePago.IdSucursal = Convert.ToInt32(pSeriePago["IdSucursal"]);
        SeriePago.SerieComplementoPago = Convert.ToString(pSeriePago["SeriePago"]);
        SeriePago.FechaAlta = Convert.ToDateTime(DateTime.Now);
        SeriePago.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        Dictionary<string, object> ParametrosCB = new Dictionary<string, object>();
        ParametrosCB.Add("IdSucursal", SeriePago.IdSucursal);
        ParametrosCB.Add("SeriePago", SeriePago.SerieComplementoPago);
        JObject oRespuesta = new JObject();
        if (SeriePago.RevisarExisteRegistro(ParametrosCB, ConexionBaseDatos) == false)
        {
            SeriePago.Agregar(ConexionBaseDatos);
            respuesta = "SeriePagoAgregada";
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "Ya existe la serie"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string AgregarRutaCFDI(Dictionary<string, object> pRutaCFDI)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CRutaCFDI RutaCFDI = new CRutaCFDI();
        RutaCFDI.IdSucursal = Convert.ToInt32(pRutaCFDI["IdSucursal"]);
        RutaCFDI.RutaCFDI = Convert.ToString(pRutaCFDI["RutaCFDI"]);
        RutaCFDI.TipoRuta = Convert.ToInt32(pRutaCFDI["TipoRuta"]);
        RutaCFDI.FechaAlta = Convert.ToDateTime(DateTime.Now);
        RutaCFDI.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        Dictionary<string, object> ParametrosCB = new Dictionary<string, object>();
        ParametrosCB.Add("IdSucursal", RutaCFDI.IdSucursal);
        ParametrosCB.Add("RutaCFDI", RutaCFDI.RutaCFDI);
        JObject oRespuesta = new JObject();

        RutaCFDI.Agregar(ConexionBaseDatos);
        respuesta = "RutaCFDIAgregada";
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();

        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string AsignarConexionContpaq(Dictionary<string, object> pAsignarConexionContpaq)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Convert.ToInt32(pAsignarConexionContpaq["IdSucursal"]), ConexionBaseDatos);
            Sucursal.BaseDatosContpaq = pAsignarConexionContpaq["BaseDatos"].ToString();
            Sucursal.UsuarioContpaq = pAsignarConexionContpaq["Usuario"].ToString();
            Sucursal.ContrasenaContpaq = pAsignarConexionContpaq["Contrasena"].ToString();
            Sucursal.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Descripcion", "Se asignó la conexión de base de datos para CONTPAQ."));
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
    public static string ObtenerListaBancos()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CBanco.ObtenerJsonBanco(true, ConexionBaseDatos));
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

    public static string ValidarSucursal(CSucursal pSucursal, CConexion pConexion)
    {
        string errores = "";

        if (pSucursal.Sucursal == "")
        { errores = errores + "<span>*</span> El nombre de la sucursal esta vacía, favor de capturarla.<br />"; }

        if (pSucursal.IdEmpresa == 0)
        { errores = errores + "<span>*</span> No se indicó la empresa, favor de seleccionarla.<br />"; }

        if (pSucursal.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> No se indicó el tipo de moneda, favor de seleccionarla.<br />"; }

        if (pSucursal.IdIVA == 0)
        { errores = errores + "<span>*</span> No se indicó el IVA, favor de seleccionarla.<br />"; }

        if (pSucursal.Telefono == "")
        { errores = errores + "<span>*</span> El teléfono está vacía, favor de capturarla.<br />"; }

        if (pSucursal.ClaveCuentaContable == "")
        { errores = errores + "<span>*</span> La clave de cuenta contable está vacía, favor de capturarla.<br />"; }

        if (pSucursal.IdSucursal == 0)
        {
            if (pSucursal.ExisteClaveCuentaContable(pSucursal.ClaveCuentaContable, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pSucursal.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }
        else
        {
            if (pSucursal.ExisteClaveCuentaContableEditar(pSucursal.ClaveCuentaContable, pSucursal.IdSucursal, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de cuenta contable " + pSucursal.ClaveCuentaContable + " ya esta dada de alta.<br />";
            }
        }


        if (pSucursal.DireccionFiscal == false)
        {
            if (pSucursal.Calle == "")
            { errores = errores + "<span>*</span> La calle está vacía, favor de capturarla.<br />"; }

            if (pSucursal.NumeroExterior == "")
            { errores = errores + "<span>*</span> El número exterior está vacío, favor de capturarlo.<br />"; }

            if (pSucursal.Colonia == "")
            { errores = errores + "<span>*</span> La colonia está vacía, favor de capturarla.<br />"; }

            if (pSucursal.CodigoPostal == "")
            { errores = errores + "<span>*</span> El código postal está vacío, favor de capturarlo.<br />"; }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    public static string ValidarCuentaBancaria(CCuentaBancaria pCuentaBancaria, CConexion pConexion)
    {
        string errores = "";

        if (pCuentaBancaria.CuentaBancaria == "")
        { errores = errores + "<span>*</span> La cuenta bancaria esta vacía, favor de capturarla.<br />"; }

        if (pCuentaBancaria.IdBanco == 0)
        { errores = errores + "<span>*</span> El banco esta vacio, favor de capturarlo.<br />"; }

        if (pCuentaBancaria.CuentaBancaria == "")
        { errores = errores + "<span>*</span> La cuenta bancaria esta vacía, favor de capturarla.<br />"; }

        if (pCuentaBancaria.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> La moneda esta vacía, favor de capturarla.<br />"; }

        if (pCuentaBancaria.Descripcion == "")
        { errores = errores + "<span>*</span> La moneda esta vacía, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    public static string ValidarSerieFactura(CSerieFactura pSerieFactura, CConexion pConexion)
    {
        string errores = "";

        if (pSerieFactura.SerieFactura == "")
        { errores = errores + "<span>*</span> La serie para la factura esta vacía, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    public static string ValidarSerieNotaCredito(CSerieNotaCredito pSerieNotaCredito, CConexion pConexion)
    {
        string errores = "";

        if (pSerieNotaCredito.SerieNotaCredito == "")
        { errores = errores + "<span>*</span> La serie para la nota de crédito esta vacía, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    public static string ValidarSeriePago(CSerieComplementoPago pSeriePago, CConexion pConexion)
    {
        string errores = "";

        if (pSeriePago.SerieComplementoPago == "")
        { errores = errores + "<span>*</span> La serie para Complemento de Pago esta vacía, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    public static string ValidarRutaCFDI(CRutaCFDI pRutaCFDI, CConexion pConexion)
    {
        string errores = "";

        if (pRutaCFDI.RutaCFDI == "")
        { errores = errores + "<span>*</span> La ruta esta vacía, favor de capturarla.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}