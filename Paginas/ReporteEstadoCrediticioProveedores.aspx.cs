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

public partial class ReporteEstadoCrediticioProveedores : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ObtenerFormaFiltroReporteEstadoCrediticioProveedores()
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

        Modelo.Add("Fecha", DateTime.Now.ToShortDateString());
        Modelo.Add(new JProperty("Permisos", oPermisos));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneRelacionSaldos(int pIdSucursal, string pSucursal, string pFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioProveedor = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioProveedorRelacionSaldos";
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
            ConsultaReporteEstadoCrediticioProveedor.Llena(ConexionBaseDatos);

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
            while (ConsultaReporteEstadoCrediticioProveedor.Registros.Read())
            {
                JObject JSaldos = new JObject();
                JSaldos.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["RazonSocial"]));
                JSaldos.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"]));
                JSaldos.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrientePesos"]));
                JSaldos.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaPesos"]));
                JSaldos.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaPesos"]));
                JSaldos.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaPesos"]));
                JSaldos.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaPesos"]));
                JSaldos.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoPesos"]));
                JASaldos.Add(JSaldos);
                TotalGeneral = TotalGeneral + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalCorriente = TotalCorriente + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrientePesos"].ToString().Replace("$", ""));
                TotalUnoTreintaPesos = TotalUnoTreintaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaPesos"].ToString().Replace("$", ""));
                TotalTreintaSesentaPesos = TotalTreintaSesentaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaPesos"].ToString().Replace("$", ""));
                TotalSesentaNoventaPesos = TotalSesentaNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaPesos"].ToString().Replace("$", ""));
                TotalMasNoventaPesos = TotalMasNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaPesos"].ToString().Replace("$", ""));
                TotalVencidoPesos = TotalVencidoPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoPesos"].ToString().Replace("$", ""));
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
    public static string ObtieneRelacionSaldosPesosGeneral(int pIdSucursal, string pSucursal, string pFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioProveedor = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioProveedorRelacionSaldosPesosGeneral";
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
            ConsultaReporteEstadoCrediticioProveedor.Llena(ConexionBaseDatos);

            JArray JASaldos = new JArray();
            decimal TotalGeneralPesos = 0;
            decimal TotalGeneralCorriente = 0;
            decimal TotalGeneralUnoTreintaPesos = 0;
            decimal TotalGeneralTreintaSesentaPesos = 0;
            decimal TotalGeneralSesentaNoventaPesos = 0;
            decimal TotalGeneralMasNoventaPesos = 0;
            decimal TotalGeneralVencidoPesos = 0;

            if (pSucursal == "Todas...")
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioProveedor.Registros.Read())
            {
                JObject JSaldos = new JObject();
                JSaldos.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["RazonSocial"]));
                JSaldos.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"]));
                JSaldos.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrientePesos"]));
                JSaldos.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaPesos"]));
                JSaldos.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaPesos"]));
                JSaldos.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaPesos"]));
                JSaldos.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaPesos"]));
                JSaldos.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoPesos"]));
                JASaldos.Add(JSaldos);
                TotalGeneralPesos = TotalGeneralPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalGeneralCorriente = TotalGeneralCorriente + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrientePesos"].ToString().Replace("$", ""));
                TotalGeneralUnoTreintaPesos = TotalGeneralUnoTreintaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaPesos"].ToString().Replace("$", ""));
                TotalGeneralTreintaSesentaPesos = TotalGeneralTreintaSesentaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaPesos"].ToString().Replace("$", ""));
                TotalGeneralSesentaNoventaPesos = TotalGeneralSesentaNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaPesos"].ToString().Replace("$", ""));
                TotalGeneralMasNoventaPesos = TotalGeneralMasNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaPesos"].ToString().Replace("$", ""));
                TotalGeneralVencidoPesos = TotalGeneralVencidoPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoPesos"].ToString().Replace("$", ""));
            }
            Modelo.Add("Saldos", JASaldos);
            Modelo.Add("TotalGeneralPesos", "$" + TotalGeneralPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalGeneralCorriente", "$" + TotalGeneralCorriente.ToString("#,###,##0.00"));
            Modelo.Add("TotalGeneralUnoTreintaPesos", "$" + TotalGeneralUnoTreintaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalGeneralTreintaSesentaPesos", "$" + TotalGeneralTreintaSesentaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalGeneralSesentaNoventaPesos", "$" + TotalGeneralSesentaNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalGeneralMasNoventaPesos", "$" + TotalGeneralMasNoventaPesos.ToString("#,###,##0.00"));
            Modelo.Add("TotalGeneralVencidoPesos", "$" + TotalGeneralVencidoPesos.ToString("#,###,##0.00"));

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
    public static string ObtieneRelacionSaldosPesosGeneralTCD(int pIdSucursal, string pSucursal, string pFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioProveedor = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioProveedorRelacionSaldosPesosGeneralTCD";
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
            ConsultaReporteEstadoCrediticioProveedor.Llena(ConexionBaseDatos);

            JArray JASaldos = new JArray();
            decimal TotalGeneral = 0;
            decimal TotalCorriente = 0;
            decimal TotalUnoTreintaPesos = 0;
            decimal TotalTreintaSesentaPesos = 0;
            decimal TotalSesentaNoventaPesos = 0;
            decimal TotalMasNoventaPesos = 0;
            decimal TotalVencidoPesos = 0;

            if (pSucursal == "Todas...")
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioProveedor.Registros.Read())
            {
                JObject JSaldos = new JObject();
                JSaldos.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["RazonSocial"]));
                JSaldos.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"]));
                JSaldos.Add("CORRIENTEPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrientePesos"]));
                JSaldos.Add("UNOTREINTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaPesos"]));
                JSaldos.Add("TREINTASESENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaPesos"]));
                JSaldos.Add("SESENTANOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaPesos"]));
                JSaldos.Add("MASNOVENTAPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaPesos"]));
                JSaldos.Add("TOTALVENCIDOPESOS", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoPesos"]));
                JASaldos.Add(JSaldos);
                TotalGeneral = TotalGeneral + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalCorriente = TotalCorriente + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrientePesos"].ToString().Replace("$", ""));
                TotalUnoTreintaPesos = TotalUnoTreintaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaPesos"].ToString().Replace("$", ""));
                TotalTreintaSesentaPesos = TotalTreintaSesentaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaPesos"].ToString().Replace("$", ""));
                TotalSesentaNoventaPesos = TotalSesentaNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaPesos"].ToString().Replace("$", ""));
                TotalMasNoventaPesos = TotalMasNoventaPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaPesos"].ToString().Replace("$", ""));
                TotalVencidoPesos = TotalVencidoPesos + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoPesos"].ToString().Replace("$", ""));
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
    public static string ObtieneRelacionSaldosDolares(int pIdSucursal, string pSucursal, string pFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioProveedor = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioProveedorRelacionSaldosDolares";
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
            ConsultaReporteEstadoCrediticioProveedor.Llena(ConexionBaseDatos);

            JArray JASaldosDolares = new JArray();
            decimal TotalGeneralDolares = 0;
            decimal TotalCorrienteDolares = 0;
            decimal TotalUnoTreintaDolares = 0;
            decimal TotalTreintaSesentaDolares = 0;
            decimal TotalSesentaNoventaDolares = 0;
            decimal TotalMasNoventaDolares = 0;
            decimal TotalVencidoDolares = 0;

            if (pSucursal == "Todas...")
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioProveedor.Registros.Read())
            {
                JObject JSaldosDolares = new JObject();
                JSaldosDolares.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["RazonSocial"]));
                JSaldosDolares.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"]));
                JSaldosDolares.Add("CORRIENTEDOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrienteDolares"]));
                JSaldosDolares.Add("UNOTREINTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaDolares"]));
                JSaldosDolares.Add("TREINTASESENTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaDolares"]));
                JSaldosDolares.Add("SESENTANOVENTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaDolares"]));
                JSaldosDolares.Add("MASNOVENTADOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaDolares"]));
                JSaldosDolares.Add("TOTALVENCIDODOLARES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoDolares"]));
                JASaldosDolares.Add(JSaldosDolares);
                TotalGeneralDolares = TotalGeneralDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"].ToString().Replace("$", ""));
                TotalCorrienteDolares = TotalCorrienteDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrienteDolares"].ToString().Replace("$", ""));
                TotalUnoTreintaDolares = TotalUnoTreintaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaDolares"].ToString().Replace("$", ""));
                TotalTreintaSesentaDolares = TotalTreintaSesentaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaDolares"].ToString().Replace("$", ""));
                TotalSesentaNoventaDolares = TotalSesentaNoventaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaDolares"].ToString().Replace("$", ""));
                TotalMasNoventaDolares = TotalMasNoventaDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaDolares"].ToString().Replace("$", ""));
                TotalVencidoDolares = TotalVencidoDolares + Convert.ToDecimal(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoDolares"].ToString().Replace("$", ""));
            }
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
    public static string ObtieneRelacionSaldosTotales(int pIdSucursal, string pSucursal, string pFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioProveedor = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioProveedorRelacionSaldosPesosGeneralConTCDDocumento";
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
            ConsultaReporteEstadoCrediticioProveedor.Llena(ConexionBaseDatos);

            JArray JASaldosTotales = new JArray();
            decimal TotalGeneralTotales = 0;
            decimal TotalCorrienteTotales = 0;
            decimal TotalUnoTreintaTotales = 0;
            decimal TotalTreintaSesentaTotales = 0;
            decimal TotalSesentaNoventaTotales = 0;
            decimal TotalMasNoventaTotales = 0;
            decimal TotalVencidoTotales = 0;

            if (pSucursal == "Todas...")
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioProveedor.Registros.Read())
            {
                JObject JSaldosTotales = new JObject();
                JSaldosTotales.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["RazonSocial"]));
                JSaldosTotales.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"]));
                JSaldosTotales.Add("CORRIENTETOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrienteTotal"]));
                JSaldosTotales.Add("UNOTREINTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaTotal"]));
                JSaldosTotales.Add("TREINTASESENTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaTotal"]));
                JSaldosTotales.Add("SESENTANOVENTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaTotal"]));
                JSaldosTotales.Add("MASNOVENTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaTotal"]));
                JSaldosTotales.Add("TOTALVENCIDOTOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoTotal"]));

                JASaldosTotales.Add(JSaldosTotales);

            }

            Modelo.Add("SaldosTotales", JASaldosTotales);


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
    public static string ObtieneRelacionSaldosTotalesTCDelDia(int pIdSucursal, string pSucursal, string pFecha)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            DateTime Fecha = DateTime.Now;
            CSelectEspecifico ConsultaReporteEstadoCrediticioProveedor = new CSelectEspecifico();
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.CommandText = "SP_Impresion_EstadoCrediticioProveedorRelacionSaldosPesosGeneralConTCDelDia";
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            ConsultaReporteEstadoCrediticioProveedor.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
            ConsultaReporteEstadoCrediticioProveedor.Llena(ConexionBaseDatos);

            JArray JASaldosTotales = new JArray();
            decimal TotalGeneralTotales = 0;
            decimal TotalCorrienteTotales = 0;
            decimal TotalUnoTreintaTotales = 0;
            decimal TotalTreintaSesentaTotales = 0;
            decimal TotalSesentaNoventaTotales = 0;
            decimal TotalMasNoventaTotales = 0;
            decimal TotalVencidoTotales = 0;

            if (pSucursal == "Todas...")
            {
                Modelo.Add("Sucursal", "Sucursal: Todas");
            }
            else
            {
                Modelo.Add("Sucursal", "Sucursal: " + pSucursal);
            }
            while (ConsultaReporteEstadoCrediticioProveedor.Registros.Read())
            {
                JObject JSaldosTotales = new JObject();
                JSaldosTotales.Add("RAZONSOCIAL", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["RazonSocial"]));
                JSaldosTotales.Add("SALDOFACTURA", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SaldoFactura"]));
                JSaldosTotales.Add("CORRIENTETOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["CorrienteTotal"]));
                JSaldosTotales.Add("UNOTREINTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["UnoTreintaTotal"]));
                JSaldosTotales.Add("TREINTASESENTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TreintaSesentaTotal"]));
                JSaldosTotales.Add("SESENTANOVENTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["SesentaNoventaTotal"]));
                JSaldosTotales.Add("MASNOVENTATOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["MasNoventaTotal"]));
                JSaldosTotales.Add("TOTALVENCIDOTOTALES", Convert.ToString(ConsultaReporteEstadoCrediticioProveedor.Registros["TotalVencidoTotal"]));

                JASaldosTotales.Add(JSaldosTotales);

            }

            Modelo.Add("SaldosTotales", JASaldosTotales);


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
}