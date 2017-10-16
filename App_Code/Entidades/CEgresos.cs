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


public partial class CEgresos
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
    public void AgregarEgresos(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_Egresos_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEgresos", 0);
        Agregar.StoredProcedure.Parameters["@pIdEgresos"].Direction = ParameterDirection.Output;
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
        if (fechaConciliacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaConciliacion", fechaConciliacion);
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
        idEgresos = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEgresos"].Value);
    }


    public static JObject ObtenerEgresos(JObject pModelo, int pIdEgresos, CConexion pConexion)
    {
        CEgresos Egresos = new CEgresos();
        Egresos.LlenaObjeto(pIdEgresos, pConexion);
        Boolean PuedeVerSaldo = false;

        pModelo.Add(new JProperty("IdEgresos", Egresos.IdEgresos));
        pModelo.Add(new JProperty("IdCuentaBancaria", Egresos.IdCuentaBancaria));
        pModelo.Add(new JProperty("IdProveedor", Egresos.IdProveedor));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Egresos.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        CuentaBancaria.LlenaObjeto(Egresos.IdCuentaBancaria, pConexion);
        pModelo.Add(new JProperty("CuentaBancaria", CuentaBancaria.CuentaBancaria));
        pModelo.Add(new JProperty("Descripcion", CuentaBancaria.Descripcion));

        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancaria.IdBanco, pConexion);
        pModelo.Add(new JProperty("Banco", Banco.Banco));
        pModelo.Add(new JProperty("Saldo", CuentaBancaria.Saldo));

        CMetodoPago MetodoPago = new CMetodoPago();
        MetodoPago.LlenaObjeto(Egresos.IdMetodoPago, pConexion);
        pModelo.Add(new JProperty("IdMetodoPago", MetodoPago.IdMetodoPago));
        pModelo.Add(new JProperty("MetodoPago", MetodoPago.MetodoPago));

        pModelo.Add(new JProperty("FechaEmision", Egresos.FechaEmision.ToShortDateString()));
        pModelo.Add(new JProperty("Folio", Egresos.Folio));

        pModelo.Add(new JProperty("Monto", Egresos.Importe));

        //OBTIENE EL TIPO DE CAMBIO EN DOLARES QUE SE GUARDO EN EL MOMENTO DE GENERAR LA NOTA DE CRÉDITO
        CTipoCambioDOFEgresos TipoCambioEgresos = new CTipoCambioDOFEgresos();
        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        ParametrosTS.Add("IdEgresos", pIdEgresos);
        TipoCambioEgresos.LlenaObjetoFiltros(ParametrosTS, pConexion);

        if (Egresos.IdTipoMoneda == 2)
        {
            TipoCambioEgresos.TipoCambio = Egresos.TipoCambio;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////

        CEgresosEncabezadoFacturaProveedor EgresosEncabezadoFacturaProveedor = new CEgresosEncabezadoFacturaProveedor();

        if (Egresos.IdTipoMoneda == 1)
        {
            pModelo.Add(new JProperty("Importe", Egresos.Importe));
            pModelo.Add(new JProperty("ImporteDolares", Egresos.Importe / ((TipoCambioEgresos.TipoCambio == 0) ? 1 : TipoCambioEgresos.TipoCambio)));

            pModelo.Add(new JProperty("Disponible", Egresos.Importe - EgresosEncabezadoFacturaProveedor.TotalAbonosEgresos(Egresos.IdEgresos, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", (Egresos.Importe / ((TipoCambioEgresos.TipoCambio == 0) ? 1 : TipoCambioEgresos.TipoCambio)) - (EgresosEncabezadoFacturaProveedor.TotalAbonosEgresos(Egresos.IdEgresos, pConexion) / ((TipoCambioEgresos.TipoCambio == 0) ? 1 : TipoCambioEgresos.TipoCambio))));
        }
        else
        {
            pModelo.Add(new JProperty("Importe", Egresos.Importe * TipoCambioEgresos.TipoCambio));
            pModelo.Add(new JProperty("ImporteDolares", Egresos.Importe));

            pModelo.Add(new JProperty("Disponible", (Egresos.Importe * TipoCambioEgresos.TipoCambio) - EgresosEncabezadoFacturaProveedor.TotalAbonosEgresos(Egresos.IdEgresos, pConexion)));
            pModelo.Add(new JProperty("DisponibleDolares", Egresos.Importe - (EgresosEncabezadoFacturaProveedor.TotalAbonosEgresos(Egresos.IdEgresos, pConexion) / ((TipoCambioEgresos.TipoCambio == 0) ? 1 : TipoCambioEgresos.TipoCambio))));
        }

        pModelo.Add(new JProperty("TipoCambio", TipoCambioEgresos.TipoCambio));

        CProveedor Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(Egresos.IdProveedor, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);

        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));
        pModelo.Add(new JProperty("Referencia", Egresos.Referencia));
        pModelo.Add(new JProperty("ConceptoGeneral", Egresos.ConceptoGeneral));
        pModelo.Add(new JProperty("FechaAplicacion", Egresos.FechaAplicacion.ToShortDateString()));
        if (Egresos.Conciliado)
        {
            pModelo.Add(new JProperty("FechaConciliacion", Egresos.FechaConciliacion.ToShortDateString()));
        }
        else
        {
            pModelo.Add(new JProperty("FechaConciliacion", DateTime.Now.ToShortDateString()));
        }

        pModelo.Add("Conciliado", Egresos.Conciliado);
        pModelo.Add("Asociado", Egresos.Asociado);

        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
        Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
        ParametrosP.Add("IdCuentaBancaria", Egresos.IdCuentaBancaria);
        ParametrosP.Add("IdUsuario", Convert.ToInt32(Egresos.IdUsuarioAlta));

        foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancaria.LlenaObjetosFiltros(ParametrosP, pConexion))
        {
            PuedeVerSaldo = oCCuentaBancaria.PuedeVerSaldo;
        }
        pModelo.Add(new JProperty("PuedeVerSaldo", PuedeVerSaldo));

        return pModelo;
    }

    public int ExisteEgresosMovimientos(int pIdEgresos, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Egresos_Consultar_ExisteEgresosMovimientos";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdEgresos", pIdEgresos);
        ObtenObjeto.Llena<CEgresosEncabezadoFacturaProveedor>(typeof(CEgresosEncabezadoFacturaProveedor), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }

    public void EditarEgresos(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Egresos_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEgresos", idEgresos);
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
        Editar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
        if (fechaConciliacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaConciliacion", fechaConciliacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }
}
