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
        GridMovimientos.TipoOrdenacion = "DESC";
        GridMovimientos.Metodo = "ObtenerMovimientos";
        GridMovimientos.TituloTabla = "Movimientos";
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
        ColBanco.Buscador = "false";
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
        ColFechaMovimiento.Buscador = "false";
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdMovimientos", sqlCon);

        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return new CJQGridJsonResponse(dataSet);
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
    public static string AgregarMovimiento (int IdCuentaBancaria, int IdTipoMovimiento, string FechaMovimiento, decimal Monto, string Referencia)
    {

        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {

                CMovimiento Movimiento = new CMovimiento();
                Movimiento.IdCuentaBancaria = IdCuentaBancaria;
                Movimiento.IdTipoMovimiento = IdTipoMovimiento;
                Movimiento.FechaAlta = DateTime.Now;
                Movimiento.FechaMovimiento = Convert.ToDateTime(FechaMovimiento);
                Movimiento.Monto = Monto;
                Movimiento.Referencia = Referencia;
                Movimiento.IdUsuarioAlta = UsuarioSesion.IdUsuario;
                Movimiento.IdTipoMoneda = 1;
                Movimiento.TipoCambio = 1;
                Movimiento.Baja = false;

                string validacion = ValidarMovimiento(Movimiento);

                if (validacion == "")
                {

                    CSelectEspecifico Consulta = new CSelectEspecifico();
                    Consulta.StoredProcedure.CommandText = "sp_Movimiento_BuscarUltimoMovimiento";
                    Consulta.StoredProcedure.Parameters.Add("IdCuentaBancaria", SqlDbType.Int).Value = Movimiento.IdCuentaBancaria;

                    Consulta.Llena(pConexion);

                    decimal SaldoInicial = 0;

                    if (Consulta.Registros.Read())
                    {
                        SaldoInicial = Convert.ToDecimal(Consulta.Registros["Saldo"]);
                    }

                    Consulta.CerrarConsulta();

                    Movimiento.SaldoInicial = SaldoInicial;
                    Movimiento.SaldoFinal = (Movimiento.IdTipoMovimiento == 1) ? SaldoInicial + Movimiento.Monto : Movimiento.SaldoInicial - Movimiento.Monto;

                    Movimiento.Agregar(pConexion);

                    //Afectar cuentas por cobrar y cuentas por pagar

                }
                else
                {
                    DescripcionError = validacion;
                    Error = 1;
                }

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();

    }

    public static string ValidarMovimiento(CMovimiento Movimiento)
    {
        string valido = "";

        valido += (Movimiento.IdCuentaBancaria != 0) ? valido : "<li>Favor de seleccionar una cuenta bancaria.</li>";
        valido += (Movimiento.IdTipoMovimiento != 0) ? valido : "<li>Favor de seleccionar un tipo de movimiento.</li>";
        valido += (Movimiento.FechaMovimiento < DateTime.Now) ? valido : "<li>Favor de seleccionar una fecha de movimiento.</li>";
        valido += (Movimiento.Monto > 0) ? valido : "<li>Favor de ingresar un monto.</li>";
        valido += (Movimiento.Referencia != "") ? valido : "<li>Favor de ingresar una referencia.</li>";

        valido += (valido != "") ? "Favor de completar los siguientes campos:": valido;

        return valido;
    }

}