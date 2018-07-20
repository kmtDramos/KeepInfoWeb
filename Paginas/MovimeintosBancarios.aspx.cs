using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
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
using System.IO;

public partial class Paginas_MovimeintosBancarios : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        CJQGrid GridMovimientos = new CJQGrid();
        GridMovimientos.NombreTabla = "grdMovimientos";
        GridMovimientos.CampoIdentificador = "IdMoviento";
        GridMovimientos.ColumnaOrdenacion = "IdMovimiento";
        GridMovimientos.TipoOrdenacion = "ASC";
        GridMovimientos.Metodo = "ObtenerMovimientos";
        GridMovimientos.TituloTabla = "Inventario";
        GridMovimientos.GenerarFuncionFiltro = false;
        GridMovimientos.GenerarFuncionTerminado = false;
        GridMovimientos.Altura = 231;
        GridMovimientos.Ancho = 940;
        GridMovimientos.NumeroRegistros = 25;
        GridMovimientos.RangoNumeroRegistros = "25,50,100";

        CJQColumn ColIdMovimiento = new CJQColumn();
        ColIdMovimiento.Nombre = "IdMovimiento";
        ColIdMovimiento.Encabezado = "No. Movimiento";
        ColIdMovimiento.Ancho = "80";
        ColIdMovimiento.Alineacion = "Left";
        GridMovimientos.Columnas.Add(ColIdMovimiento);

        CJQColumn ColCuentaBancaria = new CJQColumn();
        ColCuentaBancaria.Nombre = "CuentaBancaria";
        ColCuentaBancaria.Encabezado = "Cuenta Bancaria";
        ColCuentaBancaria.Ancho = "120";
        ColCuentaBancaria.Alineacion = "Left";
        GridMovimientos.Columnas.Add(ColCuentaBancaria);

        CJQColumn ColReferencia = new CJQColumn();
        ColReferencia.Nombre = "Referencia";
        ColReferencia.Encabezado = "Referencia";
        ColReferencia.Ancho = "120";
        ColReferencia.Alineacion = "left";
        GridMovimientos.Columnas.Add(ColReferencia);

        CJQColumn ColBanco = new CJQColumn();
        ColBanco.Nombre = "Banco";
        ColBanco.Encabezado = "Banco";
        ColBanco.Ancho = "120";
        ColBanco.Alineacion = "Left";
        GridMovimientos.Columnas.Add(ColBanco);

        CJQColumn ColTipoMovimiento = new CJQColumn();
        ColTipoMovimiento.Nombre = "TipoMovimiento";
        ColTipoMovimiento.Encabezado = "Tipo movimiento";
        ColTipoMovimiento.Ancho = "120";
        ColTipoMovimiento.Alineacion = "left";
        GridMovimientos.Columnas.Add(ColTipoMovimiento);

        CJQColumn ColFechaMovimiento = new CJQColumn();
        ColFechaMovimiento.Nombre = "FechaMovimiento";
        ColFechaMovimiento.Encabezado = "Fecha movimiento";
        ColFechaMovimiento.Ancho = "100";
        ColFechaMovimiento.Alineacion = "left";
        GridMovimientos.Columnas.Add(ColFechaMovimiento);

        CJQColumn ColSaldoInicial = new CJQColumn();
        ColSaldoInicial.Nombre = "SaldoInicial";
        ColSaldoInicial.Encabezado = "Saldo inicial";
        ColSaldoInicial.Buscador = "false";
        ColSaldoInicial.Ancho = "100";
        ColSaldoInicial.Alineacion = "right";
        GridMovimientos.Columnas.Add(ColSaldoInicial);

        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Buscador = "false";
        ColMonto.Ancho = "100";
        ColMonto.Alineacion = "right";
        GridMovimientos.Columnas.Add(ColMonto);

        CJQColumn ColSaldoFinal = new CJQColumn();
        ColSaldoFinal.Nombre = "SaldoFinal";
        ColSaldoFinal.Encabezado = "Saldo final";
        ColSaldoFinal.Buscador = "false";
        ColSaldoFinal.Ancho = "100";
        ColSaldoFinal.Alineacion = "right";
        GridMovimientos.Columnas.Add(ColSaldoFinal);
        


        ClientScript.RegisterStartupScript(Page.GetType(), "grdMovimientos", GridMovimientos.GeneraGrid(), true);
        
    }

    [WebMethod]
    public static string ObtenerFormaAgregarMovimiento()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {

            if (Error == 0)
            {
                JObject Modelo = new JObject();
                
                CBanco Bancos = new CBanco();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("Baja", 0);

                JArray cmbBancos = new JArray();

                foreach (CBanco Banco in Bancos.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Opcion = new JObject();
                    Opcion.Add("Valor", Banco.IdBanco);
                    Opcion.Add("Descripcion", Banco.Banco);
                    cmbBancos.Add(Opcion);
                }
                
                Modelo.Add("Bancos", cmbBancos);

                CTipoMovimiento TipoMovimientos = new CTipoMovimiento();
                pParametros.Add("Movimientos", 1);

                JArray cmbTipoMovimientos = new JArray();
                
                foreach (CTipoMovimiento TipoMovimiento in TipoMovimientos.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Opcion = new JObject();
                    Opcion.Add("Valor", TipoMovimiento.IdTipoMovimiento);
                    Opcion.Add("Descripcion", TipoMovimiento.TipoMovimiento);
                    cmbTipoMovimientos.Add(Opcion);
                }

                Modelo.Add("TipoMovimientos", cmbTipoMovimientos);

                Respuesta.Add("Modelo", Modelo);

            }
            
            Respuesta.Add("Descripcion", DescripcionError);
            Respuesta.Add("Error", Error);

        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerCuentaBancaria(int IdBanco)
    {

        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CCuentaBancaria Cuentas = new CCuentaBancaria();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdBanco", IdBanco);
                pParametros.Add("Baja", 0);

                JArray cmbCuentas = new JArray();

                foreach(CCuentaBancaria Cuenta in Cuentas.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Opcion = new JObject();
                    Opcion.Add("Valor", Cuenta.IdCuentaBancaria);
                    Opcion.Add("Descripcion", Cuenta.Descripcion +"("+ Cuenta.CuentaBancaria +")");
                    cmbCuentas.Add(Opcion);
                }

                Modelo.Add("CuentasBancarias", cmbCuentas);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();

    }

    [WebMethod]
    public static string AgregarMovimiento (int IdCuentaBancaria, int IdTipoMovimiento, string FechaMovimiento, decimal Monto)
    {

        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();

    }

}