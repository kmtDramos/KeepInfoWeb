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


public partial class CIngresosNoDepositados
{

    private decimal tipoCambioDOF;

    public decimal TipoCambioDOF
    {
        get { return tipoCambioDOF; }
        set
        {
            tipoCambioDOF = value;
        }
    }
    //Constructores

    //Metodos Especiales
    public void AgregarIngresosNoDepositados(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_IngresosNoDepositados_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", 0);
        Agregar.StoredProcedure.Parameters["@pIdIngresosNoDepositados"].Direction = ParameterDirection.Output;
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
        if (FechaDeposito.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaDeposito", fechaDeposito);
        }
        if (fechaEmision.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
        }

        Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDepositado", depositado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDOF", tipoCambioDOF);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDeposito", idDeposito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDeposito", tipoCambioDeposito);
        if (FechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idIngresosNoDepositados = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdIngresosNoDepositados"].Value);
    }

    public static JObject ObtenerIngresosNoDepositados(JObject pModelo, int pIdIngresosNoDepositados, CConexion pConexion)
    {
        CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
        IngresosNoDepositados.LlenaObjeto(pIdIngresosNoDepositados, pConexion);
        Boolean PuedeVerSaldo = false;

        pModelo.Add(new JProperty("IdIngresosNoDepositados", IngresosNoDepositados.IdIngresosNoDepositados));
        pModelo.Add(new JProperty("IdCuentaBancaria", IngresosNoDepositados.IdCuentaBancaria));
        pModelo.Add(new JProperty("IdCliente", IngresosNoDepositados.IdCliente));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(IngresosNoDepositados.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        CuentaBancaria.LlenaObjeto(IngresosNoDepositados.IdCuentaBancaria, pConexion);
        pModelo.Add(new JProperty("CuentaBancaria", CuentaBancaria.CuentaBancaria));
        pModelo.Add(new JProperty("Descripcion", CuentaBancaria.Descripcion));

        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancaria.IdBanco, pConexion);
        pModelo.Add(new JProperty("Banco", Banco.Banco));
        pModelo.Add(new JProperty("Saldo", CuentaBancaria.Saldo));

        CMetodoPago MetodoPago = new CMetodoPago();
        MetodoPago.LlenaObjeto(IngresosNoDepositados.IdMetodoPago, pConexion);
        pModelo.Add(new JProperty("IdMetodoPago", MetodoPago.IdMetodoPago));
        pModelo.Add(new JProperty("MetodoPago", MetodoPago.MetodoPago));

        pModelo.Add(new JProperty("FechaEmision", IngresosNoDepositados.FechaEmision.ToShortDateString()));
        pModelo.Add(new JProperty("Folio", IngresosNoDepositados.Folio));

        pModelo.Add(new JProperty("Monto", IngresosNoDepositados.Importe));
        //OBTIENE EL TIPO DE CAMBIO EN DOLARES DEL DIA ACTUAL//////////////////////////////////////////
        //CTipoCambio TipoCambio = new CTipoCambio();
        //Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        //ParametrosTS.Add("Opcion", 1);
        //ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        //ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        //ParametrosTS.Add("Fecha", DateTime.Today);
        //TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, pConexion);
        ///////////////////////////////////////////////////////////////////////////////////////////////

        //OBTIENE EL TIPO DE CAMBIO EN DOLARES QUE SE GUARDO EN EL MOMENTO DE GENERAR LA NOTA DE CRÉDITO
        CTipoCambioDOFIngresosNoDepositados TipoCambioIngresosNoDepositados = new CTipoCambioDOFIngresosNoDepositados();
        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("Opcion", 1);
        ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        ParametrosTS.Add("IdIngresosNoDepositados", pIdIngresosNoDepositados);
        TipoCambioIngresosNoDepositados.LlenaObjetoFiltros(ParametrosTS, pConexion);

        if (IngresosNoDepositados.IdTipoMoneda == 2)
        {
            TipoCambioIngresosNoDepositados.TipoCambio = IngresosNoDepositados.TipoCambio;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        CIngresosNoDepositadosEncabezadoFactura IngresosNoDepositadosEncabezadoFactura = new CIngresosNoDepositadosEncabezadoFactura();
        if (IngresosNoDepositados.IdTipoMoneda == 1)
        {
            pModelo.Add(new JProperty("Importe", IngresosNoDepositados.Importe));
            pModelo.Add(new JProperty("ImporteDolares", IngresosNoDepositados.Importe / TipoCambioIngresosNoDepositados.TipoCambio));

            pModelo.Add(new JProperty("Disponible", IngresosNoDepositados.Importe - IngresosNoDepositadosEncabezadoFactura.TotalAbonosIngresosNoDepositados(IngresosNoDepositados.IdIngresosNoDepositados, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", (IngresosNoDepositados.Importe / TipoCambioIngresosNoDepositados.TipoCambio) - (IngresosNoDepositadosEncabezadoFactura.TotalAbonosIngresosNoDepositados(IngresosNoDepositados.IdIngresosNoDepositados, pConexion) / TipoCambioIngresosNoDepositados.TipoCambio)));
        }
        else
        {
            pModelo.Add(new JProperty("Importe", IngresosNoDepositados.Importe * TipoCambioIngresosNoDepositados.TipoCambio));
            pModelo.Add(new JProperty("ImporteDolares", IngresosNoDepositados.Importe));

            pModelo.Add(new JProperty("Disponible", (IngresosNoDepositados.Importe * TipoCambioIngresosNoDepositados.TipoCambio) - IngresosNoDepositadosEncabezadoFactura.TotalAbonosIngresosNoDepositados(IngresosNoDepositados.IdIngresosNoDepositados, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", IngresosNoDepositados.Importe - (IngresosNoDepositadosEncabezadoFactura.TotalAbonosIngresosNoDepositados(IngresosNoDepositados.IdIngresosNoDepositados, pConexion) / TipoCambioIngresosNoDepositados.TipoCambio)));
        }

        pModelo.Add(new JProperty("TipoCambio", TipoCambioIngresosNoDepositados.TipoCambio));

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(IngresosNoDepositados.IdCliente, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));
        pModelo.Add(new JProperty("Referencia", IngresosNoDepositados.Referencia));
        pModelo.Add(new JProperty("ConceptoGeneral", IngresosNoDepositados.ConceptoGeneral));
        pModelo.Add(new JProperty("FechaDeposito", IngresosNoDepositados.FechaDeposito.ToShortDateString()));
        pModelo.Add(new JProperty("FechaPago", IngresosNoDepositados.FechaPago.ToShortDateString()));
        pModelo.Add("Depositado", IngresosNoDepositados.Depositado);
        pModelo.Add("Asociado", IngresosNoDepositados.Asociado);

        if (IngresosNoDepositados.Depositado)
        {
            pModelo.Add(new JProperty("FechaDepositado", IngresosNoDepositados.FechaMovimiento.ToShortDateString()));
        }
        else
        {
            pModelo.Add(new JProperty("FechaDepositado", "-"));
        }

        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
        Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
        ParametrosP.Add("IdCuentaBancaria", IngresosNoDepositados.IdCuentaBancaria);
        ParametrosP.Add("IdUsuario", Convert.ToInt32(IngresosNoDepositados.IdUsuarioAlta));
        
        foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancaria.LlenaObjetosFiltros(ParametrosP, pConexion))
        {
            PuedeVerSaldo = oCCuentaBancaria.PuedeVerSaldo;
        }
        pModelo.Add(new JProperty("PuedeVerSaldo", PuedeVerSaldo));

        return pModelo;
    }

    public int ExisteIngresosNoDepositadosMovimientos(int pIdIngresosNoDepositados, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_IngresosNoDepositados_Consultar_ExisteIngresosNoDepositadosMovimientos";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", pIdIngresosNoDepositados);
        ObtenObjeto.Llena<CIngresosNoDepositadosEncabezadoFactura>(typeof(CIngresosNoDepositadosEncabezadoFactura), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }

    public void EditarIngresosNoDepositados(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_IngresosNoDepositados_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", idIngresosNoDepositados);
        if (fechaMovimiento.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDcocumento", idTipoDcocumento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
        Editar.StoredProcedure.Parameters.AddWithValue("@pImporte", importe);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaDeposito.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaDeposito", fechaDeposito);
        }
        if (fechaEmision.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDepositado", depositado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDOF", tipoCambioDOF);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDeposito", idDeposito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDeposito", tipoCambioDeposito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }
}
