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


public partial class CCheques
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
    public void AgregarCheques(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_Cheques_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCheques", 0);
        Agregar.StoredProcedure.Parameters["@pIdCheques"].Direction = ParameterDirection.Output;
        if (fechaMovimiento.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDcocumento", idTipoDcocumento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pImporte", importe);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
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
        if (fechaConciliacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaConciliacion", fechaConciliacion);
        }
        Agregar.Insert(pConexion);
        idCheques = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCheques"].Value);
    }
    public static JObject ObtenerCheques(JObject pModelo, int pIdCheques, CConexion pConexion)
    {
        CCheques Cheques = new CCheques();
        Cheques.LlenaObjeto(pIdCheques, pConexion);
        Boolean PuedeVerSaldo = false;

        pModelo.Add(new JProperty("IdCheques", Cheques.IdCheques));
        pModelo.Add(new JProperty("IdCuentaBancaria", Cheques.IdCuentaBancaria));
        pModelo.Add(new JProperty("IdProveedor", Cheques.IdProveedor));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Cheques.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        CuentaBancaria.LlenaObjeto(Cheques.IdCuentaBancaria, pConexion);
        pModelo.Add(new JProperty("CuentaBancaria", CuentaBancaria.CuentaBancaria));
        pModelo.Add(new JProperty("Descripcion", CuentaBancaria.Descripcion));

        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancaria.IdBanco, pConexion);
        pModelo.Add(new JProperty("Banco", Banco.Banco));
        pModelo.Add(new JProperty("Saldo", CuentaBancaria.Saldo));

        CMetodoPago MetodoPago = new CMetodoPago();
        MetodoPago.LlenaObjeto(Cheques.IdMetodoPago, pConexion);
        pModelo.Add(new JProperty("IdMetodoPago", MetodoPago.IdMetodoPago));
        pModelo.Add(new JProperty("MetodoPago", MetodoPago.MetodoPago));

        pModelo.Add(new JProperty("FechaEmision", Cheques.FechaEmision.ToShortDateString()));
        pModelo.Add(new JProperty("Folio", Cheques.Folio));

        pModelo.Add(new JProperty("Monto", Cheques.Importe));


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
        CTipoCambioDOFCheques TipoCambioCheques = new CTipoCambioDOFCheques();
        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("Opcion", 1);
        ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        ParametrosTS.Add("IdCheques", pIdCheques);
        TipoCambioCheques.LlenaObjetoFiltros(ParametrosTS, pConexion);

        if (Cheques.IdTipoMoneda == 2)
        {
            TipoCambioCheques.TipoCambio = Cheques.TipoCambio;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        CChequesEncabezadoFacturaProveedor ChequesEncabezadoFacturaProveedor = new CChequesEncabezadoFacturaProveedor();
        if (Cheques.IdTipoMoneda == 1)
        {
            pModelo.Add(new JProperty("Importe", Cheques.Importe));
            decimal tipocambio = TipoCambioCheques.TipoCambio;
            if (tipocambio == 0)
            {
                tipocambio = 1;
            }
            pModelo.Add(new JProperty("ImporteDolares", Cheques.Importe / tipocambio));

            pModelo.Add(new JProperty("Disponible", Cheques.Importe - ChequesEncabezadoFacturaProveedor.TotalAbonosCheques(Cheques.IdCheques, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", (Cheques.Importe / tipocambio) - (ChequesEncabezadoFacturaProveedor.TotalAbonosCheques(Cheques.IdCheques, pConexion) / tipocambio)));
        }
        else
        {
            pModelo.Add(new JProperty("Importe", Cheques.Importe * TipoCambioCheques.TipoCambio));
            pModelo.Add(new JProperty("ImporteDolares", Cheques.Importe));

            pModelo.Add(new JProperty("Disponible", (Cheques.Importe * TipoCambioCheques.TipoCambio) - ChequesEncabezadoFacturaProveedor.TotalAbonosCheques(Cheques.IdCheques, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", Cheques.Importe - (ChequesEncabezadoFacturaProveedor.TotalAbonosCheques(Cheques.IdCheques, pConexion) / TipoCambioCheques.TipoCambio)));
        }

        pModelo.Add(new JProperty("TipoCambio", TipoCambioCheques.TipoCambio));

        CProveedor Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(Cheques.IdProveedor, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);

        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));
        pModelo.Add(new JProperty("Referencia", Cheques.Referencia));
        pModelo.Add(new JProperty("ConceptoGeneral", Cheques.ConceptoGeneral));
        pModelo.Add(new JProperty("FechaAplicacion", Cheques.FechaAplicacion.ToShortDateString()));
        pModelo.Add(new JProperty("FechaConciliacion", (Cheques.conciliado)? Cheques.FechaConciliacion.ToShortDateString() : "Sin conciliar"));
        pModelo.Add("Conciliado", Cheques.Conciliado);
        pModelo.Add("Impreso", Cheques.Impreso);
        pModelo.Add("Asociado", Cheques.Asociado);

        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
        Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
        ParametrosP.Add("IdCuentaBancaria", Cheques.IdCuentaBancaria);
        ParametrosP.Add("IdUsuario", Convert.ToInt32(Cheques.IdUsuarioAlta));

        foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancaria.LlenaObjetosFiltros(ParametrosP, pConexion))
        {
            PuedeVerSaldo = oCCuentaBancaria.PuedeVerSaldo;
        }
        pModelo.Add(new JProperty("PuedeVerSaldo", PuedeVerSaldo));

        return pModelo;
    }

    public int ExisteChequesMovimientos(int pIdCheques, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Cheques_Consultar_ExisteChequesMovimientos";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCheques", pIdCheques);
        ObtenObjeto.Llena<CChequesEncabezadoFacturaProveedor>(typeof(CChequesEncabezadoFacturaProveedor), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }

    public void EditarCheques(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Cheques_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCheques", idCheques);
        if (fechaMovimiento.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaMovimiento", fechaMovimiento);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDcocumento", idTipoDcocumento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaBancaria", idCuentaBancaria);
        Editar.StoredProcedure.Parameters.AddWithValue("@pImporte", importe);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
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
        Editar.StoredProcedure.Parameters.AddWithValue("@pImpreso", impreso);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCancelado", cancelado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pAutorizado", autorizado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDevuelto", devuelto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
        if (fechaConciliacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaConciliacion", fechaConciliacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }
}