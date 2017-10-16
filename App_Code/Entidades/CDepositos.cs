using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public partial class CDepositos
{
    //Constructores
    private string idsIngresosNoDepositados;

    public string IdsIngresosNoDepositados
    {
        get { return idsIngresosNoDepositados; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            idsIngresosNoDepositados = value;
        }
    }

    //Metodos Especiales
    //Metodos Especiales
    public void AgregarDepositos(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_Depositos_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDepositos", 0);
        Agregar.StoredProcedure.Parameters["@pIdDepositos"].Direction = ParameterDirection.Output;
        if (fechaMovimiento.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDcocumento", idTipoDcocumento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pImporte", importe);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaAplicacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAplicacion", fechaAplicacion);
        }
        if (fechaEmision.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pConciliado", conciliado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdsIngresosNoDepositados", idsIngresosNoDepositados);
        Agregar.Insert(pConexion);
        idDepositos = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDepositos"].Value);
    }
    public static JObject ObtenerDepositos(JObject pModelo, int pIdDepositos, CConexion pConexion)
    {
        CDepositos Depositos = new CDepositos();
        Depositos.LlenaObjeto(pIdDepositos, pConexion);
        Boolean PuedeVerSaldo = false;
        pModelo.Add(new JProperty("IdDepositos", Depositos.IdDepositos));
        pModelo.Add(new JProperty("IdCuentaBancaria", Depositos.IdCuentaBancaria));
        pModelo.Add(new JProperty("IdCliente", Depositos.IdCliente));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Depositos.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        CuentaBancaria.LlenaObjeto(Depositos.IdCuentaBancaria, pConexion);
        pModelo.Add(new JProperty("CuentaBancaria", CuentaBancaria.CuentaBancaria));
        pModelo.Add(new JProperty("Descripcion", CuentaBancaria.Descripcion));

        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancaria.IdBanco, pConexion);
        pModelo.Add(new JProperty("Banco", Banco.Banco));
        pModelo.Add(new JProperty("TipoMonedaBanco", TipoMoneda.TipoMoneda));
        pModelo.Add(new JProperty("Saldo", CuentaBancaria.Saldo));

        CMetodoPago MetodoPago = new CMetodoPago();
        MetodoPago.LlenaObjeto(Depositos.IdMetodoPago, pConexion);
        pModelo.Add(new JProperty("IdMetodoPago", MetodoPago.IdMetodoPago));
        pModelo.Add(new JProperty("MetodoPago", MetodoPago.MetodoPago));

        pModelo.Add(new JProperty("FechaEmision", Depositos.FechaEmision.ToShortDateString()));
        pModelo.Add(new JProperty("Folio", Depositos.Folio));
        pModelo.Add(new JProperty("Importe", Depositos.Importe));
        pModelo.Add(new JProperty("TipoCambio", Depositos.TipoCambio));

        CDepositosEncabezadoFactura DepositosEncabezadoFactura = new CDepositosEncabezadoFactura();
        pModelo.Add(new JProperty("Disponible", Depositos.Importe - DepositosEncabezadoFactura.TotalAbonosDepositos(Depositos.IdDepositos, pConexion)));

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(Depositos.IdCliente, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));
        pModelo.Add(new JProperty("Referencia", Depositos.Referencia));
        pModelo.Add(new JProperty("ConceptoGeneral", Depositos.ConceptoGeneral));
        pModelo.Add(new JProperty("FechaAplicacion", Depositos.FechaAplicacion.ToShortDateString()));
        pModelo.Add("Conciliado", Depositos.Conciliado);
        pModelo.Add("Asociado", Depositos.Asociado);

        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
        Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
        ParametrosP.Add("IdCuentaBancaria", Depositos.IdCuentaBancaria);
        ParametrosP.Add("IdUsuario", Convert.ToInt32(Depositos.IdUsuarioAlta));

        foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancaria.LlenaObjetosFiltros(ParametrosP, pConexion))
        {
            PuedeVerSaldo = oCCuentaBancaria.PuedeVerSaldo;
        }
        pModelo.Add(new JProperty("PuedeVerSaldo", PuedeVerSaldo));

        return pModelo;
    }

    public int ExisteDepositosMovimientos(int pIdDepositos, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Depositos_Consultar_ExisteDepositosMovimientos";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdDepositos", pIdDepositos);
        ObtenObjeto.Llena<CDepositosEncabezadoFactura>(typeof(CDepositosEncabezadoFactura), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }
}