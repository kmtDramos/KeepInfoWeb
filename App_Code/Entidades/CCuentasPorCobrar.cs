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


public partial class CCuentasPorCobrar
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
    public void AgregarCuentasPorCobrar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_CuentasPorCobrar_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", 0);
        Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrar"].Direction = ParameterDirection.Output;
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
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDOF", tipoCambioDOF);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCuentasPorCobrar = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrar"].Value);
    }

    public static JObject ObtenerCuentasPorCobrar(JObject pModelo, int pIdCuentasPorCobrar, CConexion pConexion)
    {
        CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
        CuentasPorCobrar.LlenaObjeto(pIdCuentasPorCobrar, pConexion);

        Boolean PuedeVerSaldo = false;

        pModelo.Add(new JProperty("IdCuentasPorCobrar", CuentasPorCobrar.IdCuentasPorCobrar));
        pModelo.Add(new JProperty("IdCuentaBancaria", CuentasPorCobrar.IdCuentaBancaria));
        pModelo.Add(new JProperty("IdCliente", CuentasPorCobrar.IdCliente));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(CuentasPorCobrar.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        CuentaBancaria.LlenaObjeto(CuentasPorCobrar.IdCuentaBancaria, pConexion);
        pModelo.Add(new JProperty("CuentaBancaria", CuentaBancaria.CuentaBancaria));
        pModelo.Add(new JProperty("Descripcion", CuentaBancaria.Descripcion));

        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancaria.IdBanco, pConexion);
        pModelo.Add(new JProperty("Banco", Banco.Banco));
        pModelo.Add(new JProperty("Saldo", CuentaBancaria.Saldo));

        CMetodoPago MetodoPago = new CMetodoPago();
        MetodoPago.LlenaObjeto(CuentasPorCobrar.IdMetodoPago, pConexion);
        pModelo.Add(new JProperty("IdMetodoPago", MetodoPago.IdMetodoPago));
        pModelo.Add(new JProperty("MetodoPago", MetodoPago.MetodoPago));

        if (CuentasPorCobrar.FechaConciliacion.Year == 1)
        {
            pModelo.Add(new JProperty("FechaConciliacion", "-"));
        }
        else
        {
            pModelo.Add(new JProperty("FechaConciliacion", CuentasPorCobrar.FechaConciliacion.ToShortDateString()));
        }

        pModelo.Add(new JProperty("FechaEmision", CuentasPorCobrar.FechaEmision.ToShortDateString()));
        pModelo.Add(new JProperty("Folio", CuentasPorCobrar.Folio));

        pModelo.Add(new JProperty("Monto", CuentasPorCobrar.Importe));

        //OBTIENE EL TIPO DE CAMBIO EN DOLARES DEL DIA ACTUAL//////////////////////////////////////////
        //CTipoCambio TipoCambio = new CTipoCambio();
        //Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        //ParametrosTS.Add("Opcion", 1);
        //ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        //ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        //ParametrosTS.Add("Fecha", DateTime.Today);
        //TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, pConexion);
        ///////////////////////////////////////////////////////////////////////////////////////////////

        //OBTIENE EL TIPO DE CAMBIO DOF EN DOLARES QUE SE GUARDO EN EL MOMENTO DE GENERAR LA NOTA DE CRÉDITO
        CTipoCambioDOFCuentasPorCobrar TipoCambioCuentasPorCobrar = new CTipoCambioDOFCuentasPorCobrar();
        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        ParametrosTS.Add("IdCuentasPorCobrar", CuentasPorCobrar.idCuentasPorCobrar);
        TipoCambioCuentasPorCobrar.LlenaObjetoFiltros(ParametrosTS, pConexion);

        if (CuentasPorCobrar.IdTipoMoneda == 2)
        {
            TipoCambioCuentasPorCobrar.TipoCambio = CuentasPorCobrar.TipoCambio;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        CCuentasPorCobrarEncabezadoFactura CuentasPorCobrarEncabezadoFactura = new CCuentasPorCobrarEncabezadoFactura();
        if (CuentasPorCobrar.IdTipoMoneda == 1)
        {
            pModelo.Add(new JProperty("Importe", CuentasPorCobrar.Importe));
            decimal tipocambio = TipoCambioCuentasPorCobrar.TipoCambio;
            if (tipocambio == 0)
            {
                tipocambio = 1;
            }
            pModelo.Add(new JProperty("ImporteDolares", CuentasPorCobrar.Importe / tipocambio));

            pModelo.Add(new JProperty("Disponible", CuentasPorCobrar.Importe - CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(CuentasPorCobrar.IdCuentasPorCobrar, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", (CuentasPorCobrar.Importe / tipocambio) - (CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(CuentasPorCobrar.IdCuentasPorCobrar, pConexion) / tipocambio)));
        }
        else
        {
            decimal tipocambio = TipoCambioCuentasPorCobrar.TipoCambio;
            if (tipocambio == 0)
            {
                tipocambio = 1;
            }
            pModelo.Add(new JProperty("Importe", CuentasPorCobrar.Importe * tipocambio));
            pModelo.Add(new JProperty("ImporteDolares", CuentasPorCobrar.Importe));

            pModelo.Add(new JProperty("Disponible", (CuentasPorCobrar.Importe * tipocambio) - CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(CuentasPorCobrar.IdCuentasPorCobrar, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", CuentasPorCobrar.Importe - (CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(CuentasPorCobrar.IdCuentasPorCobrar, pConexion) / tipocambio)));
        }

        pModelo.Add(new JProperty("TipoCambio", TipoCambioCuentasPorCobrar.TipoCambio));

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(CuentasPorCobrar.IdCliente, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));
        pModelo.Add(new JProperty("Referencia", CuentasPorCobrar.Referencia));
        pModelo.Add(new JProperty("ConceptoGeneral", CuentasPorCobrar.ConceptoGeneral));
        pModelo.Add(new JProperty("FechaAplicacion", CuentasPorCobrar.FechaAplicacion.ToShortDateString()));
        pModelo.Add("Conciliado", CuentasPorCobrar.Conciliado);
        pModelo.Add("Asociado", CuentasPorCobrar.Asociado);


        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
        Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
        ParametrosP.Add("IdCuentaBancaria", CuentasPorCobrar.IdCuentaBancaria);
        ParametrosP.Add("IdUsuario", Convert.ToInt32(CuentasPorCobrar.IdUsuarioAlta));

        foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancaria.LlenaObjetosFiltros(ParametrosP, pConexion))
        {
            PuedeVerSaldo = oCCuentaBancaria.PuedeVerSaldo;
        }
        pModelo.Add(new JProperty("PuedeVerSaldo", PuedeVerSaldo));

        return pModelo;
    }

    public void EditarCuentasPorCobrar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_CuentasPorCobrar_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", idCuentasPorCobrar);
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
        
        if (fechaAplicacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAplicacion", fechaAplicacion);
        }
        if (fechaEmision.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
        }


        Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pConceptoGeneral", conceptoGeneral);
        Editar.StoredProcedure.Parameters.AddWithValue("@pConciliado", conciliado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambioDOF", tipoCambioDOF);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public int ExisteCuentasPorCobrarMovimientos(int pIdCuentasPorCobrar, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_CuentasPorCobrar_Consultar_ExisteCuentasPorCobrarMovimientos";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", pIdCuentasPorCobrar);
        ObtenObjeto.Llena<CCuentasPorCobrarEncabezadoFactura>(typeof(CCuentasPorCobrarEncabezadoFactura), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }
}