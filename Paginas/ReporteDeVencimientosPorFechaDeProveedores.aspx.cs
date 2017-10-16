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

public partial class ReporteDeVencimientosPorFechaDeProveedores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    public static string ObtenerFormaFiltroReporteEstadoCuentaProveedores()
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
        //DateTime FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime FechaInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //DateTime FechaFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        DateTime dtFechaFin = DateTime.Now;


        DateTime FechaFinal = dtFechaFin.AddDays(7);
        Modelo.Add("FechaInicial", Convert.ToString(FechaInicial.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(FechaFinal.ToShortDateString()));

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
    public static string ObtieneReporteVencimientosPorProveedor(int pIdProveedor, string pFechaInicial, string pFechaFinal, int pIdSucursal, string pSucursal, string pFechaIni, string pFechaF)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;

            CSelectEspecifico ConsultaReporteProveedorDeVencimientosPorFecha = new CSelectEspecifico();
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.CommandText = "SP_Impresion_VencimientosPorFechaProveedores_Reporte";
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", pIdProveedor);
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteProveedorDeVencimientosPorFecha.StoredProcedure.Parameters.AddWithValue("@pFechaInicialR", pFechaIni);
            ConsultaReporteProveedorDeVencimientosPorFecha.Llena(ConexionBaseDatos);

            if (pIdSucursal != 0)
            {
                Modelo.Add("SUCURSAL", Convert.ToString(pSucursal).ToUpper());
            }
            else
            {
                Modelo.Add("SUCURSAL", "TODAS");
            }

            if (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.Read())
            {
                Modelo.Add("RAZONSOCIALRECEPTOR", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["RazonSocialReceptor"]));
                Modelo.Add("FECHA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Fecha"]));
                Modelo.Add("FECHAINICIAL", Convert.ToString(pFechaIni));
                Modelo.Add("FECHAFINAL", Convert.ToString(pFechaF));
                Modelo.Add("TIPOMONEDA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["TipoMoneda"]));
                Modelo.Add("ESTADOCUENTA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["EstadoCuenta"]));
                Modelo.Add("PROVEEDOR", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Proveedor"]));
                Modelo.Add("NOMBRE", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Nombre"]));
            }

            if (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.NextResult())
            {
                JArray JAMovimientosFacturasVencidas = new JArray();
                while (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.Read())
                {
                    JObject JMovimientoFacturasVencidas = new JObject();
                    JMovimientoFacturasVencidas.Add("FECHAFACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["FechaFactura"]));
                    JMovimientoFacturasVencidas.Add("FACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Factura"]));
                    JMovimientoFacturasVencidas.Add("SUBTOTAL", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Subtotal"]));
                    JMovimientoFacturasVencidas.Add("IVA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["IVA"]));
                    JMovimientoFacturasVencidas.Add("MONTOFACTURADO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["MontoFacturado"]));
                    JMovimientoFacturasVencidas.Add("SALDO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Saldo"]));
                    JMovimientoFacturasVencidas.Add("VENCE", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Vence"]));
                    JMovimientoFacturasVencidas.Add("DIASVENCHOY", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["DiasVencidosAHoy"]));
                    JMovimientoFacturasVencidas.Add("DIASVENCIDOSFI", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["DiasVDesdeFI"]));
                    JMovimientoFacturasVencidas.Add("MONEDA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["TipoMoneda"]));
                    JMovimientoFacturasVencidas.Add("REFERENCIA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Descripcion"]));
                    JAMovimientosFacturasVencidas.Add(JMovimientoFacturasVencidas);
                }
                Modelo.Add("MovimientosFacturasVencidas", JAMovimientosFacturasVencidas);
            }

            if (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.NextResult())
            {
                JArray JAMovimientosFacturasCorriente = new JArray();
                while (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.Read())
                {
                    JObject JMovimientoFacturasAlCorriente = new JObject();
                    JMovimientoFacturasAlCorriente.Add("FECHAFACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["FechaFactura"]));
                    JMovimientoFacturasAlCorriente.Add("FACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Factura"]));
                    JMovimientoFacturasAlCorriente.Add("SUBTOTAL", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Subtotal"]));
                    JMovimientoFacturasAlCorriente.Add("IVA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["IVA"]));
                    JMovimientoFacturasAlCorriente.Add("MONTOFACTURADO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["MontoFacturado"]));
                    JMovimientoFacturasAlCorriente.Add("SALDO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Saldo"]));
                    JMovimientoFacturasAlCorriente.Add("VENCE", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Vence"]));
                    JMovimientoFacturasAlCorriente.Add("DIASPORVENCER", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["DiasFaltanVencer"]));
                    JMovimientoFacturasAlCorriente.Add("MONEDA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["TipoMoneda"]));
                    JMovimientoFacturasAlCorriente.Add("REFERENCIA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Descripcion"]));
                    JAMovimientosFacturasCorriente.Add(JMovimientoFacturasAlCorriente);
                }
                Modelo.Add("MovimientosFacturasAlCorriente", JAMovimientosFacturasCorriente);
            }


            if (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.NextResult())
            {
                JArray JAMovimientosFacturasVencidasDolares = new JArray();
                while (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.Read())
                {
                    JObject JMovimientoFacturasVencidasDolares = new JObject();
                    JMovimientoFacturasVencidasDolares.Add("FECHAFACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["FechaFactura"]));
                    JMovimientoFacturasVencidasDolares.Add("FACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Factura"]));
                    JMovimientoFacturasVencidasDolares.Add("SUBTOTAL", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Subtotal"]));
                    JMovimientoFacturasVencidasDolares.Add("IVA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["IVA"]));
                    JMovimientoFacturasVencidasDolares.Add("MONTOFACTURADO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["MontoFacturado"]));
                    JMovimientoFacturasVencidasDolares.Add("SALDO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Saldo"]));
                    JMovimientoFacturasVencidasDolares.Add("VENCE", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Vence"]));
                    JMovimientoFacturasVencidasDolares.Add("DIASVENCHOY", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["DiasVencidosAHoy"]));
                    JMovimientoFacturasVencidasDolares.Add("DIASVENCIDOSFI", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["DiasVDesdeFI"]));
                    JMovimientoFacturasVencidasDolares.Add("MONEDA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["TipoMoneda"]));
                    JMovimientoFacturasVencidasDolares.Add("REFERENCIA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Descripcion"]));
                    JAMovimientosFacturasVencidasDolares.Add(JMovimientoFacturasVencidasDolares);
                }
                Modelo.Add("MovimientosFacturasVencidasDolares", JAMovimientosFacturasVencidasDolares);
            }

            if (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.NextResult())
            {
                JArray JAMovimientosFacturasCorrienteDolares = new JArray();
                while (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.Read())
                {
                    JObject JMovimientoFacturasAlCorrienteDolares = new JObject();
                    JMovimientoFacturasAlCorrienteDolares.Add("FECHAFACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["FechaFactura"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("FACTURA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Factura"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("SUBTOTAL", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Subtotal"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("IVA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["IVA"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("MONTOFACTURADO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["MontoFacturado"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("SALDO", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Saldo"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("VENCE", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Vence"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("DIASPORVENCER", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["DiasFaltanVencer"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("MONEDA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["TipoMoneda"]));
                    JMovimientoFacturasAlCorrienteDolares.Add("REFERENCIA", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["Descripcion"]));
                    JAMovimientosFacturasCorrienteDolares.Add(JMovimientoFacturasAlCorrienteDolares);
                }
                Modelo.Add("MovimientosFacturasAlCorrienteDolares", JAMovimientosFacturasCorrienteDolares);
            }

            if (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.NextResult())
            {
                if (ConsultaReporteProveedorDeVencimientosPorFecha.Registros.Read())
                {
                    Modelo.Add("VENCIDASENPESOS", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["SaldoFacturasVencidasPesos"]));
                    Modelo.Add("PORVENCERENPESOS", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["SaldoFacturasAlCorrientePesos"]));
                    Modelo.Add("VENCIDASENDOLARES", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["SaldoFacturasVencidasDolares"]));
                    Modelo.Add("PORVENCERENDOLARES", Convert.ToString(ConsultaReporteProveedorDeVencimientosPorFecha.Registros["SaldoFacturasAlCorrienteDolares"]));
                }
            }

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


    //Busquedas
    [WebMethod]
    public static string BuscarRazonSocialProveedor(string pRazonSocial, int pIdSucursal)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proveedor_ConsultarRazonSocial_Reportes";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        string jsonOrganizacionString = jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacionString;

    }
}