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

public partial class CNotaCredito
{
    //Constructores

    //Metodos Especiales
    public void AgregarNotaCredito(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_NotaCredito_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", 0);
        Agregar.StoredProcedure.Parameters["@pIdNotaCredito"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolioNotaCredito", folioNotaCredito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
        if (fecha.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldoDocumento", saldoDocumento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeIVA", porcentajeIVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        if (fechaCancelacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoNotaCredito", idTipoNotaCredito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);

        Agregar.Insert(pConexion);
        idNotaCredito = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNotaCredito"].Value);
    }

    public static JObject ObtenerNotaCredito(JObject pModelo, int pIdNotaCredito, CConexion pConexion)
    {
        CNotaCredito NotaCredito = new CNotaCredito();
        NotaCredito.LlenaObjeto(pIdNotaCredito, pConexion);
        pModelo.Add(new JProperty("IdNotaCredito", NotaCredito.IdNotaCredito));

        CTxtTimbradosNotaCredito TxtTimbradosNotaCredito = new CTxtTimbradosNotaCredito();
        Dictionary<string, object> ParametrosTXT = new Dictionary<string, object>();
        ParametrosTXT.Add("Folio", NotaCredito.FolioNotaCredito);
        ParametrosTXT.Add("Serie", NotaCredito.SerieNotaCredito);
        TxtTimbradosNotaCredito.LlenaObjetoFiltros(ParametrosTXT, pConexion);

        if (TxtTimbradosNotaCredito.IdTxtTimbradosNotaCredito != 0 && TxtTimbradosNotaCredito.IdTxtTimbradosNotaCredito != null)
        {
            pModelo.Add(new JProperty("IdTxtTimbradosNotaCredito", TxtTimbradosNotaCredito.IdTxtTimbradosNotaCredito));
        }
        else
        {
            pModelo.Add(new JProperty("IdTxtTimbradosNotaCredito", 0));
        }

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(NotaCredito.IdCliente, pConexion);
        pModelo.Add(new JProperty("IdCliente", Cliente.IdCliente));

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));

        pModelo.Add(new JProperty("SerieNotaCredito", NotaCredito.SerieNotaCredito));
        pModelo.Add(new JProperty("FolioNotaCredito", NotaCredito.FolioNotaCredito));
        pModelo.Add(new JProperty("Descripcion", NotaCredito.Descripcion));
        pModelo.Add(new JProperty("Fecha", NotaCredito.Fecha.ToShortDateString()));
        pModelo.Add(new JProperty("Monto", NotaCredito.Monto));
        pModelo.Add(new JProperty("PorcentajeIVA", NotaCredito.PorcentajeIVA));
        pModelo.Add(new JProperty("IVA", NotaCredito.IVA));
        pModelo.Add(new JProperty("Total", NotaCredito.Total));
        pModelo.Add(new JProperty("Referencia", NotaCredito.Referencia));
        pModelo.Add(new JProperty("SaldoDocumento", NotaCredito.SaldoDocumento));
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(NotaCredito.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        pModelo.Add(new JProperty("TipoCambio", NotaCredito.TipoCambio));
        pModelo.Add(new JProperty("Baja", NotaCredito.Baja));
        pModelo.Add("Asociado", NotaCredito.Asociado);
        return pModelo;
    }

    public static JObject ObtenerNotaCreditoAsociarDocumentos(JObject pModelo, int pIdNotaCredito, CConexion pConexion)
    {

        CNotaCredito NotaCredito = new CNotaCredito();
        NotaCredito.LlenaObjeto(pIdNotaCredito, pConexion);
        pModelo.Add(new JProperty("IdNotaCredito", NotaCredito.IdNotaCredito));

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(NotaCredito.IdCliente, pConexion);
        pModelo.Add(new JProperty("IdCliente", Cliente.IdCliente));

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));

        pModelo.Add(new JProperty("SerieNotaCredito", NotaCredito.SerieNotaCredito));
        pModelo.Add(new JProperty("FolioNotaCredito", NotaCredito.FolioNotaCredito));
        pModelo.Add(new JProperty("Descripcion", NotaCredito.Descripcion));
        pModelo.Add(new JProperty("Fecha", NotaCredito.Fecha.ToShortDateString()));
        pModelo.Add(new JProperty("Monto", NotaCredito.Monto));
        pModelo.Add(new JProperty("PorcentajeIVA", NotaCredito.PorcentajeIVA));
        pModelo.Add(new JProperty("IVA", NotaCredito.IVA));
        pModelo.Add(new JProperty("Total", NotaCredito.Total));
        pModelo.Add(new JProperty("Referencia", NotaCredito.Referencia));
        pModelo.Add(new JProperty("SaldoDocumento", NotaCredito.SaldoDocumento));
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(NotaCredito.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));


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
        CTipoCambioNotaCredito TipoCambioNotaCredito = new CTipoCambioNotaCredito();
        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("Opcion", 1);
        ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        ParametrosTS.Add("IdNotaCredito", pIdNotaCredito);
        TipoCambioNotaCredito.LlenaObjetoFiltros(ParametrosTS, pConexion);
        ///////////////////////////////////////////////////////////////////////////////////////////////

        CNotaCreditoEncabezadoFactura NotaCreditoEncabezadoFactura = new CNotaCreditoEncabezadoFactura();
        if (NotaCredito.IdTipoMoneda == 1)
        {
            pModelo.Add(new JProperty("Importe", NotaCredito.Total));
            if (TipoCambioNotaCredito.TipoCambio != 0)
            {
                pModelo.Add(new JProperty("ImporteDolares", NotaCredito.Total / TipoCambioNotaCredito.TipoCambio));
            }
            else
            {
                pModelo.Add(new JProperty("ImporteDolares", 0));
            }



            pModelo.Add(new JProperty("Disponible", NotaCredito.Total - NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(NotaCredito.IdNotaCredito, pConexion)));
            if (TipoCambioNotaCredito.TipoCambio != 0)
            {
                pModelo.Add(new JProperty("DisponibleDolares", (NotaCredito.Total / TipoCambioNotaCredito.TipoCambio) - (NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(NotaCredito.IdNotaCredito, pConexion) / TipoCambioNotaCredito.TipoCambio)));
            }
            else
            {
                pModelo.Add(new JProperty("DisponibleDolares", 0));
            }
        }
        else
        {
            pModelo.Add(new JProperty("Importe", NotaCredito.Total * TipoCambioNotaCredito.TipoCambio));
            pModelo.Add(new JProperty("ImporteDolares", NotaCredito.Total));

            pModelo.Add(new JProperty("Disponible", (NotaCredito.Total * TipoCambioNotaCredito.TipoCambio) - NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(NotaCredito.IdNotaCredito, pConexion)));
            if (TipoCambioNotaCredito.TipoCambio != 0)
            {
                pModelo.Add(new JProperty("DisponibleDolares", NotaCredito.Total - (NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(NotaCredito.IdNotaCredito, pConexion) / TipoCambioNotaCredito.TipoCambio)));
            }
            else
            {
                pModelo.Add(new JProperty("DisponibleDolares", NotaCredito.Total - (NotaCreditoEncabezadoFactura.TotalAbonosNotaCredito(NotaCredito.IdNotaCredito, pConexion))));
            }
        }

        pModelo.Add(new JProperty("TipoCambio", TipoCambioNotaCredito.TipoCambio));

        return pModelo;
    }

    public int ObtieneNumeroNotaCredito(string pSerieNotaCredito, int pIdSucursal, CConexion pConexion)
    {
        int NumeroNotaCredito = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_NotaCredito_ObtieneNumeroNotaCredito";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", Convert.ToString(pSerieNotaCredito));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            NumeroNotaCredito = Convert.ToInt32(Select.Registros["NumeroNotaCredito"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return NumeroNotaCredito;
    }

    public int ExisteNotaCreditoTimbrada(int pIdNotaCredito, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteNotaCredito = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_TxtTimbrados_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", Convert.ToInt32(pIdNotaCredito));
        ObtenObjeto.Llena<CTxtTimbradosNotaCredito>(typeof(CTxtTimbradosNotaCredito), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteNotaCredito = 1;
        }
        return ExisteNotaCredito;
    }

    public int ExisteNotaCreditoMovimientos(int pIdNotaCredito, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_NotaCreditoMovimientos_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", Convert.ToInt32(pIdNotaCredito));
        ObtenObjeto.Llena<CNotaCreditoEncabezadoFactura>(typeof(CNotaCreditoEncabezadoFactura), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }
    ///Nota de Credito Devolucion
    public int ObtieneNotaTimbrada(int pIdNotaCredito, CConexion pConexion)
    {
        int TotalNotaTimbrada = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_NotaCredito_ObtieneNotaCreditoTimbrada";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", pIdNotaCredito);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            TotalNotaTimbrada = Convert.ToInt32(Select.Registros["TotalNotaTimbrada"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return TotalNotaTimbrada;
    }

    public void EditarNotaCreditoCancelacion(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_NotaCredito_EditarCancelacion";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFolioNotaCredito", folioNotaCredito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
        if (fecha.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoDocumento", saldoDocumento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pReferencia", referencia);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeIVA", porcentajeIVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        if (fechaCancelacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoNotaCredito", idTipoNotaCredito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }
}