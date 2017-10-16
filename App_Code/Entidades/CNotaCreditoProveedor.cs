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


public partial class CNotaCreditoProveedor
{
    //Constructores

    //Metodos Especiales
    public void AgregarNotaCreditoProveedor(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_NotaCreditoProveedor_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", 0);
        Agregar.StoredProcedure.Parameters["@pIdNotaCreditoProveedor"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolioNotaCredito", folioNotaCredito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
        if (fecha.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
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
        idNotaCreditoProveedor = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNotaCreditoProveedor"].Value);
    }

    public static JObject ObtenerNotaCreditoProveedor(JObject pModelo, int pIdNotaCreditoProveedor, CConexion pConexion)
    {
        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
        NotaCreditoProveedor.LlenaObjeto(pIdNotaCreditoProveedor, pConexion);
        pModelo.Add(new JProperty("IdNotaCreditoProveedor", NotaCreditoProveedor.IdNotaCreditoProveedor));

        CProveedor Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(NotaCreditoProveedor.IdProveedor, pConexion);
        pModelo.Add(new JProperty("IdProveedor", Proveedor.IdProveedor));

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));

        pModelo.Add(new JProperty("SerieNotaCredito", NotaCreditoProveedor.SerieNotaCredito));
        pModelo.Add(new JProperty("FolioNotaCredito", NotaCreditoProveedor.FolioNotaCredito));
        pModelo.Add(new JProperty("Descripcion", NotaCreditoProveedor.Descripcion));
        pModelo.Add(new JProperty("Fecha", NotaCreditoProveedor.Fecha.ToShortDateString()));
        pModelo.Add(new JProperty("Monto", NotaCreditoProveedor.Monto));
        pModelo.Add(new JProperty("PorcentajeIVA", NotaCreditoProveedor.PorcentajeIVA));
        pModelo.Add(new JProperty("IVA", NotaCreditoProveedor.IVA));
        pModelo.Add(new JProperty("Total", NotaCreditoProveedor.Total));
        pModelo.Add(new JProperty("Referencia", NotaCreditoProveedor.Referencia));
        pModelo.Add(new JProperty("SaldoDocumento", NotaCreditoProveedor.SaldoDocumento));
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(NotaCreditoProveedor.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        pModelo.Add(new JProperty("TipoCambio", NotaCreditoProveedor.TipoCambio));
        pModelo.Add(new JProperty("Baja", NotaCreditoProveedor.Baja));

        return pModelo;
    }

    public static JObject ObtenerNotaCreditoProveedorAsociarDocumentos(JObject pModelo, int pIdNotaCreditoProveedor, CConexion pConexion)
    {

        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
        NotaCreditoProveedor.LlenaObjeto(pIdNotaCreditoProveedor, pConexion);
        pModelo.Add(new JProperty("IdNotaCreditoProveedor", NotaCreditoProveedor.IdNotaCreditoProveedor));

        CProveedor Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(NotaCreditoProveedor.IdProveedor, pConexion);
        pModelo.Add(new JProperty("IdProveedor", Proveedor.IdProveedor));

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));

        pModelo.Add(new JProperty("SerieNotaCredito", NotaCreditoProveedor.SerieNotaCredito));
        pModelo.Add(new JProperty("FolioNotaCredito", NotaCreditoProveedor.FolioNotaCredito));
        pModelo.Add(new JProperty("Descripcion", NotaCreditoProveedor.Descripcion));
        pModelo.Add(new JProperty("Fecha", NotaCreditoProveedor.Fecha.ToShortDateString()));
        pModelo.Add(new JProperty("Monto", NotaCreditoProveedor.Monto));
        pModelo.Add(new JProperty("PorcentajeIVA", NotaCreditoProveedor.PorcentajeIVA));
        pModelo.Add(new JProperty("IVA", NotaCreditoProveedor.IVA));
        pModelo.Add(new JProperty("Total", NotaCreditoProveedor.Total));
        pModelo.Add(new JProperty("Referencia", NotaCreditoProveedor.Referencia));
        pModelo.Add(new JProperty("SaldoDocumento", NotaCreditoProveedor.SaldoDocumento));
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(NotaCreditoProveedor.IdTipoMoneda, pConexion);
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
        CTipoCambioNotaCreditoProveedor TipoCambioNotaCreditoProveedor = new CTipoCambioNotaCreditoProveedor();
        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("Opcion", 1);
        ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(2));
        ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
        ParametrosTS.Add("IdNotaCreditoProveedor", pIdNotaCreditoProveedor);
        TipoCambioNotaCreditoProveedor.LlenaObjetoFiltros(ParametrosTS, pConexion);
        ///////////////////////////////////////////////////////////////////////////////////////////////

        CNotaCreditoProveedorEncabezadoFacturaProveedor NotaCreditoProveedorEncabezadoFacturaProveedor = new CNotaCreditoProveedorEncabezadoFacturaProveedor();
        if (NotaCreditoProveedor.IdTipoMoneda == 1)
        {
            pModelo.Add(new JProperty("Importe", NotaCreditoProveedor.Total));
            if (TipoCambioNotaCreditoProveedor.TipoCambio != 0)
            {
                pModelo.Add(new JProperty("ImporteDolares", NotaCreditoProveedor.Total / TipoCambioNotaCreditoProveedor.TipoCambio));
            }
            else
            {
                pModelo.Add(new JProperty("ImporteDolares", 0));
            }

            pModelo.Add(new JProperty("Disponible", NotaCreditoProveedor.Total - NotaCreditoProveedorEncabezadoFacturaProveedor.TotalAbonosNotaCreditoProveedor(NotaCreditoProveedor.IdNotaCreditoProveedor, pConexion)));
            if (TipoCambioNotaCreditoProveedor.TipoCambio != 0)
            {
                pModelo.Add(new JProperty("DisponibleDolares", (NotaCreditoProveedor.Total / TipoCambioNotaCreditoProveedor.TipoCambio) - (NotaCreditoProveedorEncabezadoFacturaProveedor.TotalAbonosNotaCreditoProveedor(NotaCreditoProveedor.IdNotaCreditoProveedor, pConexion) / TipoCambioNotaCreditoProveedor.TipoCambio)));
            }
            else
            {
                pModelo.Add(new JProperty("DisponibleDolares", 0));
            }
        }
        else
        {
            pModelo.Add(new JProperty("Importe", NotaCreditoProveedor.Total * TipoCambioNotaCreditoProveedor.TipoCambio));
            pModelo.Add(new JProperty("ImporteDolares", NotaCreditoProveedor.Total));

            pModelo.Add(new JProperty("Disponible", (NotaCreditoProveedor.Total * TipoCambioNotaCreditoProveedor.TipoCambio) - NotaCreditoProveedorEncabezadoFacturaProveedor.TotalAbonosNotaCreditoProveedor(NotaCreditoProveedor.IdNotaCreditoProveedor, pConexion)));
            if (TipoCambioNotaCreditoProveedor.TipoCambio != 0)
            {
                pModelo.Add(new JProperty("DisponibleDolares", NotaCreditoProveedor.Total - (NotaCreditoProveedorEncabezadoFacturaProveedor.TotalAbonosNotaCreditoProveedor(NotaCreditoProveedor.IdNotaCreditoProveedor, pConexion) / TipoCambioNotaCreditoProveedor.TipoCambio)));
            }
            else
            {
                pModelo.Add(new JProperty("DisponibleDolares", NotaCreditoProveedor.Total - (NotaCreditoProveedorEncabezadoFacturaProveedor.TotalAbonosNotaCreditoProveedor(NotaCreditoProveedor.IdNotaCreditoProveedor, pConexion))));
            }
        }

        pModelo.Add(new JProperty("TipoCambio", TipoCambioNotaCreditoProveedor.TipoCambio));

        return pModelo;
    }

    public int ObtieneNumeroNotaCreditoProveedor(string pSerieNotaCreditoProveedor, int pIdSucursal, CConexion pConexion)
    {
        int NumeroNotaCreditoProveedor = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_NotaCreditoProveedor_ObtieneNumeroNotaCreditoProveedor";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCreditoProveedor", Convert.ToString(pSerieNotaCreditoProveedor));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            NumeroNotaCreditoProveedor = Convert.ToInt32(Select.Registros["NumeroNotaCreditoProveedor"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return NumeroNotaCreditoProveedor;
    }

    //public int ExisteNotaCreditoProveedorTimbrada(int pIdNotaCreditoProveedor, CConexion pConexion)
    //{
    //    CSelect ObtenObjeto = new CSelect();
    //    int ExisteNotaCreditoProveedor = 0;
    //    ObtenObjeto.StoredProcedure.CommandText = "sp_TxtTimbrados_Consulta";
    //    ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
    //    ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", Convert.ToInt32(pIdNotaCreditoProveedor));
    //    ObtenObjeto.Llena<CTxtTimbradosNotaCreditoProveedor>(typeof(CTxtTimbradosNotaCreditoProveedor), pConexion);
    //    if (ObtenObjeto.ListaRegistros.Count > 0)
    //    {
    //        ExisteNotaCreditoProveedor = 1;
    //    }
    //    return ExisteNotaCreditoProveedor;
    //}

    public int ExisteNotaCreditoProveedorMovimientos(int pIdNotaCreditoProveedor, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_NotaCreditoProveedorMovimientos_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", Convert.ToInt32(pIdNotaCreditoProveedor));
        ObtenObjeto.Llena<CNotaCreditoProveedorEncabezadoFacturaProveedor>(typeof(CNotaCreditoProveedorEncabezadoFacturaProveedor), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }
    ///Nota de Credito Devolucion
    public int ObtieneNotaTimbrada(int pIdNotaCreditoProveedor, CConexion pConexion)
    {
        int TotalNotaTimbrada = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_NotaCreditoProveedor_ObtieneNotaCreditoProveedorTimbrada";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", pIdNotaCreditoProveedor);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            TotalNotaTimbrada = Convert.ToInt32(Select.Registros["TotalNotaTimbrada"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return TotalNotaTimbrada;
    }

    public void EditarNotaCreditoProveedorCancelacion(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_NotaCreditoProveedor_EditarCancelacion";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFolioNotaCredito", folioNotaCredito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", serieNotaCredito);
        if (fecha.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
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
