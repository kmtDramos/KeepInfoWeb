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

        CJQColumn ColBanco = new CJQColumn();
        ColBanco.Nombre = "Banco";
        ColBanco.Encabezado = "Banco";
        ColBanco.Ancho = "120";
        ColBanco.Alineacion = "Left";
        ColBanco.Buscador = "false";
        GridMovimientos.Columnas.Add(ColBanco);

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

        CJQColumn ColMoneda = new CJQColumn();
        ColMoneda.Nombre = "Moneda";
        ColMoneda.Encabezado = "Moneda";
        ColMoneda.Buscador = "false";
        ColMoneda.Ancho = "100";
        ColMoneda.Alineacion = "left";
        GridMovimientos.Columnas.Add(ColMoneda);

        CJQColumn ColTipoCambio = new CJQColumn();
        ColTipoCambio.Nombre = "TipoCambio";
        ColTipoCambio.Encabezado = "Tipo de cambio";
        ColTipoCambio.Buscador = "false";
        ColTipoCambio.Ancho = "100";
        ColTipoCambio.Alineacion = "right";
        GridMovimientos.Columnas.Add(ColTipoCambio);

        CJQColumn ColMontoOriginal = new CJQColumn();
        ColMontoOriginal.Nombre = "MontoOriginal";
        ColMontoOriginal.Encabezado = "Monto Original";
        ColMontoOriginal.Buscador = "false";
        ColMontoOriginal.Ancho = "100";
        ColMontoOriginal.Alineacion = "right";
        GridMovimientos.Columnas.Add(ColMontoOriginal);

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
    public static CJQGridJsonResponse ObtenerMovimientos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdBanco)
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
        Stored.Parameters.Add("IdBanco", SqlDbType.Int).Value = pIdBanco;

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

                CTipoMovimientos TipoMovimientos = new CTipoMovimientos();
                
                JArray cmbTipoMovimientos = new JArray();
                
                foreach (CTipoMovimientos TipoMovimiento in TipoMovimientos.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Opcion = new JObject();
                    Opcion.Add("Valor", TipoMovimiento.IdTipoMovimientos);
                    Opcion.Add("Descripcion", TipoMovimiento.TipoMovimiento);
                    cmbTipoMovimientos.Add(Opcion);
                }

                Modelo.Add("TipoMovimientos", cmbTipoMovimientos);

                CFlujoCaja FlujosCaja = new CFlujoCaja();

                JArray cmbFlujoCaja = new JArray();

                foreach (CFlujoCaja FlujoCaja in FlujosCaja.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Opcion = new JObject();
                    Opcion.Add("Valor", FlujoCaja.IdFlujoCaja);
                    Opcion.Add("Descripcion", FlujoCaja.FlujoCaja);
                    cmbFlujoCaja.Add(Opcion);
                }

                Modelo.Add("FlujoCaja", cmbFlujoCaja);

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_Movimiento_TipoCambioDiarioOficial";

                Consulta.Llena(pConexion);

                if (Consulta.Registros.Read())
                {
                    Modelo.Add("Dolares", Convert.ToDecimal(Consulta.Registros["TipoCambio"]));
                }

                Consulta.CerrarConsulta();

                Respuesta.Add("Modelo", Modelo);

            }
            
            Respuesta.Add("Descripcion", DescripcionError);
            Respuesta.Add("Error", Error);

        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaTraspaso()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
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

                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
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
    public static string AgregarMovimiento (int IdCuentaBancaria, int IdTipoMovimiento, string FechaMovimiento, decimal Monto, string Referencia, int IdOrganizacion, int IdFlujoCaja, decimal TipoCambio, int IdTipoMoneda)
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
                Movimiento.IdOrganizacion = IdOrganizacion;
                Movimiento.IdFlujoCaja = IdFlujoCaja;
                Movimiento.Monto = Monto * TipoCambio;
                Movimiento.Referencia = Referencia;
                Movimiento.IdUsuarioAlta = UsuarioSesion.IdUsuario;
                Movimiento.IdTipoMoneda = IdTipoMoneda;
                Movimiento.TipoCambio = TipoCambio;
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
                    CTipoMovimientos TipoMovimiento = new CTipoMovimientos();
                    TipoMovimiento.LlenaObjeto(Movimiento.IdTipoMovimiento, pConexion);
                    Movimiento.SaldoFinal =  SaldoInicial + (Movimiento.Monto * TipoMovimiento.Afectacion);

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

    [WebMethod]
    public static string GenerarTraspaso(int IdCuentaBancariaOrigen, int IdTipoMonedaOrigen, decimal TipoCambioOrigen, decimal MontoOrigen, string ReferenciaOrigen, int IdCuentaBancariaDestino,
                                        int IdTipoMonedaDestino, decimal TipoCambioDestino, decimal MontoDestino, string ReferenciaDestino)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CMovimiento MovimientoOrigen = new CMovimiento();

                MovimientoOrigen.IdCuentaBancaria = IdCuentaBancariaOrigen;
                MovimientoOrigen.IdTipoMoneda = IdTipoMonedaOrigen;
                MovimientoOrigen.TipoCambio = TipoCambioOrigen;
                MovimientoOrigen.Monto = MontoOrigen;
                MovimientoOrigen.Referencia = ReferenciaOrigen;
                MovimientoOrigen.IdOrganizacion = 1415;
                MovimientoOrigen.IdFlujoCaja = 12;
                MovimientoOrigen.IdTipoMovimiento = 4;
                MovimientoOrigen.FechaMovimiento = DateTime.Now;
                MovimientoOrigen.FechaAlta = DateTime.Now;

                CSelectEspecifico ConsultaOrigen = new CSelectEspecifico();
                ConsultaOrigen.StoredProcedure.CommandText = "sp_Movimiento_BuscarUltimoMovimiento";
                ConsultaOrigen.StoredProcedure.Parameters.Add("IdCuentaBancaria", SqlDbType.Int).Value = MovimientoOrigen.IdCuentaBancaria;

                ConsultaOrigen.Llena(pConexion);

                decimal SaldoInicialOrigen = 0;

                if (ConsultaOrigen.Registros.Read())
                {
                    SaldoInicialOrigen = Convert.ToDecimal(ConsultaOrigen.Registros["Saldo"]);
                }

                ConsultaOrigen.CerrarConsulta();

                MovimientoOrigen.SaldoInicial = SaldoInicialOrigen;
                MovimientoOrigen.SaldoFinal = SaldoInicialOrigen - MontoOrigen;

                MovimientoOrigen.Agregar(pConexion);

                CMovimiento MovimientoDestino = new CMovimiento();

                MovimientoDestino.IdCuentaBancaria = IdCuentaBancariaDestino;
                MovimientoDestino.IdTipoMoneda = IdTipoMonedaDestino;
                MovimientoDestino.TipoCambio = TipoCambioDestino;
                MovimientoDestino.Monto = MontoDestino;
                MovimientoDestino.Referencia = ReferenciaDestino;
                MovimientoDestino.IdOrganizacion = 1415;
                MovimientoDestino.IdFlujoCaja = 2;
                MovimientoDestino.IdTipoMovimiento = 1;
                MovimientoDestino.FechaAlta = DateTime.Now;
                MovimientoDestino.FechaMovimiento = DateTime.Now;

                CSelectEspecifico ConsultaDestino = new CSelectEspecifico();
                ConsultaDestino.StoredProcedure.CommandText = "sp_Movimiento_BuscarUltimoMovimiento";
                ConsultaDestino.StoredProcedure.Parameters.Add("IdCuentaBancaria", SqlDbType.Int).Value = MovimientoDestino.IdCuentaBancaria;

                ConsultaDestino.Llena(pConexion);

                decimal SaldoInicialDestino = 0;

                if (ConsultaDestino.Registros.Read())
                {
                    SaldoInicialDestino = Convert.ToDecimal(ConsultaDestino.Registros["Saldo"]);
                }

                ConsultaDestino.CerrarConsulta();

                MovimientoDestino.SaldoInicial = SaldoInicialDestino;
                MovimientoDestino.SaldoFinal = SaldoInicialDestino + MontoOrigen;

                MovimientoDestino.Agregar(pConexion);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerBancos()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_Banco_ListarBancos";
                Consulta.Llena(pConexion);

                JArray ListaBancos = new JArray();

                while (Consulta.Registros.Read())
                {
                    JObject OpcionBanco = new JObject();
                    OpcionBanco.Add("Valor", Convert.ToInt32(Consulta.Registros["IdBanco"]));
                    OpcionBanco.Add("Descripcion", Convert.ToString(Consulta.Registros["Banco"]));
                    ListaBancos.Add(OpcionBanco);
                }

                Modelo.Add("ListaBancos", ListaBancos);

                Consulta.CerrarConsulta();

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string BuscarRazonSocial(string pRazonSocial)
    {
        string respuesta = "";
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            SqlCommand Handler = new SqlCommand();
            Handler.CommandText = "sp_Movimientos_BuscarOrganizacion";
            Handler.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
            Handler.CommandType = CommandType.StoredProcedure;
            Handler.Connection = pConexion.ConexionBaseDatosSqlServer;
            DataSet dataSet = new DataSet();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(Handler);
            dataAdapter.Fill(dataSet);
            respuesta = JsonConvert.SerializeObject(dataSet);
        });
        return respuesta;
    }

}