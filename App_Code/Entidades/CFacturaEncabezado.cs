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


public partial class CFacturaEncabezado
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Metodos Especiales

    public JToken ObtenerJsonJObject(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);

        JObject oJson = JObject.Parse(JsonConvert.SerializeObject(dataSet));
        return oJson["Table"];
    }

    public int ObtieneNumeroFactura(string pSerieFactura, int pIdSucursal, CConexion pConexion)
    {
        int NumeroFactura = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Factura_ObtieneNumeroFactura";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pSerieFactura", Convert.ToString(pSerieFactura));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            NumeroFactura = Convert.ToInt32(Select.Registros["NumeroFactura"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return NumeroFactura;
    }

    public void AgregarFacturaEncabezado(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_FacturaEncabezado_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", 0);
        Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezado"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", numeroFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", idSerieFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", razonSocial);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pRFC", rFC);
        if (fechaRequeridaFacturacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRequeridaFacturacion", fechaRequeridaFacturacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroParcialidades", numeroParcialidades);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        if (fechaEmision.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
        }
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldoFactura", saldoFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroCuenta", numeroCuenta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCalleFiscal", calleFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroExteriorFiscal", numeroExteriorFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroInteriorFiscal", numeroInteriorFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pColoniaFiscal", coloniaFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostalFiscal", codigoPostalFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPaisFiscal", paisFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEstadoFiscal", estadoFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMunicipioFiscal", municipioFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pLocalidadFiscal", localidadFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pReferenciaFiscal", referenciaFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipioFiscal", idMunicipioFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidadFiscal", idLocalidadFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCalleEntrega", calleEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroExteriorEntrega", numeroExteriorEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroInteriorEntrega", numeroInteriorEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pColoniaEntrega", coloniaEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostalEntrega", codigoPostalEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pReferenciaEntrega", referenciaEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPaisEntrega", paisEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEstadoEntrega", estadoEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMunicipioEntrega", municipioEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pLocalidadEntrega", localidadEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipioEntrega", idMunicipioEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidadEntrega", idLocalidadEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", idEstatusFacturaEncabezado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMetodoPago", metodoPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCondicionPago", condicionPago);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNumeroCuenta", idNumeroCuenta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pRegimenFiscal", regimenFiscal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pLugarExpedicion", lugarExpedicion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pEsRefactura", esRefactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pParcialidades", parcialidades);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroOrdenCompra", numeroOrdenCompra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
        if (fechaCancelacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroParcialidadesPendientes", numeroParcialidadesPendientes);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaGlobal", idFacturaGlobal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pUUIDGlobal", uUIDGlobal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaGlobal", fechaGlobal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMontoGlobal", montoGlobal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSerieGlobal", serieGlobal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pParcialidadIndividual", parcialidadIndividual);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolioGlobal", folioGlobal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSerie", serie);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeDescuento", porcentajeDescuento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoCliente", idDescuentoCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsoCFDI", idUsoCFDI);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAnticipo", Anticipo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaAnticipo", idFacturaAnticipo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoRelacion", idTipoRelacion);
        Agregar.Insert(pConexion);
        idFacturaEncabezado = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezado"].Value);
    }

    public static JObject ObtenerFacturaEncabezado(JObject pModelo, int pIdFacturaEncabezado, CConexion pConexion)
    {
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        COrganizacion Organizacion = new COrganizacion();
        FacturaEncabezado.LlenaObjeto(pIdFacturaEncabezado, pConexion);
        pModelo.Add(new JProperty("IdFacturaEncabezado", FacturaEncabezado.IdFacturaEncabezado));
        pModelo.Add(new JProperty("IdCliente", FacturaEncabezado.IdCliente));

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(FacturaEncabezado.IdCliente, pConexion);

        pModelo.Add(new JProperty("IdOrganizacion", Cliente.IdOrganizacion));
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("NombreComercial", Organizacion.NombreComercial));

        pModelo.Add(new JProperty("RegimenFiscal", FacturaEncabezado.RegimenFiscal));
        pModelo.Add(new JProperty("LugarExpedicion", FacturaEncabezado.LugarExpedicion));
        pModelo.Add(new JProperty("FechaEmision", FacturaEncabezado.FechaEmision.ToShortDateString()));
        pModelo.Add(new JProperty("RazonSocial", FacturaEncabezado.RazonSocial));
        pModelo.Add(new JProperty("RFC", FacturaEncabezado.RFC));
        pModelo.Add(new JProperty("IdCondicionPago", FacturaEncabezado.IdCondicionPago));
        pModelo.Add(new JProperty("CondicionPago", FacturaEncabezado.CondicionPago));
        pModelo.Add(new JProperty("IdMetodoPago", FacturaEncabezado.IdMetodoPago));
        pModelo.Add(new JProperty("MetodoPago", FacturaEncabezado.MetodoPago));

        CUsoCFDI usoCFDI = new CUsoCFDI();
        if (FacturaEncabezado.IdUsoCFDI == "" || FacturaEncabezado.IdUsoCFDI == null)
            FacturaEncabezado.IdUsoCFDI = "3";
        
        usoCFDI.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsoCFDI), pConexion);
        pModelo.Add(new JProperty("IdUsoCFDI", usoCFDI.IdUsoCFDI));
        pModelo.Add(new JProperty("UsoCFDI", usoCFDI.ClaveUsoCFDI+" - "+usoCFDI.Descricpion));
        pModelo.Add(new JProperty("FechaPago", FacturaEncabezado.FechaPago.ToShortDateString()));
        pModelo.Add(new JProperty("Anticipo", FacturaEncabezado.Anticipo));

        CFacturaEncabezado FacturaRelacionada = new CFacturaEncabezado();
        FacturaRelacionada.LlenaObjeto(FacturaEncabezado.IdFacturaAnticipo,pConexion);
        string facturaR = Convert.ToString(FacturaRelacionada.NumeroFactura);
        pModelo.Add(new JProperty("FacturaRelacionada", (facturaR == "0")? "":facturaR));
        pModelo.Add(new JProperty("IdFacturaRelacionada", FacturaEncabezado.IdFacturaAnticipo));
        CTipoRelacion TipoRelacion = new CTipoRelacion();
        TipoRelacion.LlenaObjeto(FacturaEncabezado.idTipoRelacion,pConexion);
        pModelo.Add(new JProperty("TipoRelacion", TipoRelacion.Clave+" - "+TipoRelacion.Descripcion));
        pModelo.Add(new JProperty("IdTipoRelacion",FacturaEncabezado.IdTipoRelacion));

        // Obtener factura padre
        CFacturaEncabezadoSustituye FacturaSustituye = new CFacturaEncabezadoSustituye();
        Dictionary<string, object> ParametrosFS = new Dictionary<string, object>();
        ParametrosFS.Add("IdFacturaSustituye", FacturaEncabezado.IdFacturaEncabezado);
        ParametrosFS.Add("Baja", 0);
        FacturaSustituye.LlenaObjetoFiltros(ParametrosFS, pConexion);

        string FolioPadre = "";

        if (FacturaSustituye.IdFacturaEncabezadoSustituye != 0)
        {
            CFacturaEncabezado FacturaPadre = new CFacturaEncabezado();
            FacturaPadre.LlenaObjeto(FacturaSustituye.IdFactura, pConexion);
            FolioPadre = FacturaPadre.Serie + FacturaPadre.NumeroFactura;
        }
        pModelo.Add("FolioHijo", FolioPadre);

        // Obtener factura hijo
        CFacturaEncabezadoSustituye SustituyeFactura = new CFacturaEncabezadoSustituye();
        Dictionary<string, object> ParametrosSF = new Dictionary<string, object>();
        ParametrosSF.Add("IdFactura", FacturaEncabezado.IdFacturaEncabezado);
        ParametrosSF.Add("Baja", 0);
        SustituyeFactura.LlenaObjetoFiltros(ParametrosSF, pConexion);

        string FolioHijo = "";

        if (SustituyeFactura.IdFacturaEncabezadoSustituye != 0)
        {
            CFacturaEncabezado FacturaHijo = new CFacturaEncabezado();
            FacturaHijo.LlenaObjeto(SustituyeFactura.IdFacturaSustituye, pConexion);
            FolioHijo = FacturaHijo.Serie + FacturaHijo.NumeroFactura;
        }
        pModelo.Add("FolioPadre", FolioHijo);

        pModelo.Add(new JProperty("CalleFiscal", FacturaEncabezado.CalleFiscal));
        pModelo.Add(new JProperty("IdNumeroCuenta", FacturaEncabezado.IdNumeroCuenta));
        pModelo.Add(new JProperty("NumeroCuenta", FacturaEncabezado.NumeroCuenta));
        pModelo.Add(new JProperty("NumeroExteriorFiscal", FacturaEncabezado.NumeroExteriorFiscal));
        pModelo.Add(new JProperty("NumeroInteriorFiscal", FacturaEncabezado.NumeroInteriorFiscal));
        pModelo.Add(new JProperty("ColoniaFiscal", FacturaEncabezado.ColoniaFiscal));
        pModelo.Add(new JProperty("PaisFiscal", FacturaEncabezado.PaisFiscal));
        pModelo.Add(new JProperty("EstadoFiscal", FacturaEncabezado.EstadoFiscal));
        pModelo.Add(new JProperty("CodigoFiscal", FacturaEncabezado.CodigoPostalFiscal));
        pModelo.Add(new JProperty("MunicipioFiscal", FacturaEncabezado.MunicipioFiscal));
        pModelo.Add(new JProperty("LocalidadFiscal", FacturaEncabezado.LocalidadFiscal));
        pModelo.Add(new JProperty("ReferenciaFiscal", FacturaEncabezado.ReferenciaFiscal));

        pModelo.Add(new JProperty("CalleEntrega", FacturaEncabezado.CalleEntrega));
        pModelo.Add(new JProperty("NumeroExteriorEntrega", FacturaEncabezado.NumeroExteriorEntrega));
        pModelo.Add(new JProperty("NumeroInteriorEntrega", FacturaEncabezado.NumeroInteriorEntrega));
        pModelo.Add(new JProperty("ColoniaEntrega", FacturaEncabezado.ColoniaEntrega));
        pModelo.Add(new JProperty("PaisEntrega", FacturaEncabezado.PaisEntrega));
        pModelo.Add(new JProperty("EstadoEntrega", FacturaEncabezado.EstadoEntrega));
        pModelo.Add(new JProperty("CodigoPostalEntrega", FacturaEncabezado.CodigoPostalEntrega));
        pModelo.Add(new JProperty("MunicipioEntrega", FacturaEncabezado.MunicipioEntrega));
        pModelo.Add(new JProperty("LocalidadEntrega", FacturaEncabezado.LocalidadEntrega));
        pModelo.Add(new JProperty("ReferenciaEntrega", FacturaEncabezado.ReferenciaEntrega));
        pModelo.Add(new JProperty("EsRefactura", FacturaEncabezado.EsRefactura));

        pModelo.Add(new JProperty("IdDescuentoCliente", FacturaEncabezado.IdDescuentoCliente));
        pModelo.Add(new JProperty("DescuentoFacturaCliente", FacturaEncabezado.Descuento));
        pModelo.Add(new JProperty("PorcentajeDescuento", FacturaEncabezado.PorcentajeDescuento));

        pModelo.Add(new JProperty("FechaRequeridaFacturacion", FacturaEncabezado.FechaRequeridaFacturacion.ToShortDateString()));

        CSerieFactura SerieFactura = new CSerieFactura();
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);
        pModelo.Add(new JProperty("IdSerieFactura", FacturaEncabezado.IdSerieFactura));
        pModelo.Add(new JProperty("SerieFactura", SerieFactura.SerieFactura));
        pModelo.Add(new JProperty("NumeroFactura", FacturaEncabezado.NumeroFactura));
        pModelo.Add(new JProperty("NumeroOrdenCompra", FacturaEncabezado.NumeroOrdenCompra));


        if (FacturaEncabezado.IdEstatusFacturaEncabezado == 2)
        {
            pModelo.Add(new JProperty("Estatus", "CANCELADA"));
        }
        else if (FacturaEncabezado.IdEstatusFacturaEncabezado == 3)
        {
            pModelo.Add(new JProperty("Estatus", "PAGADA PARCIAL"));
        }
        else if (FacturaEncabezado.IdEstatusFacturaEncabezado == 4)
        {
            pModelo.Add(new JProperty("Estatus", "PAGADA TOTAL"));
        }
        else
        {
            pModelo.Add(new JProperty("Estatus", "PENDIENTE"));
        }

        pModelo.Add(new JProperty("IdEstatus", Convert.ToInt32(FacturaEncabezado.IdEstatusFacturaEncabezado)));


        if (SerieFactura.Timbrado == true)
        {
            pModelo.Add(new JProperty("SerieTimbrado", 1));
        }
        else
        {
            pModelo.Add(new JProperty("SerieTimbrado", 0));
        }

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(FacturaEncabezado.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));
        pModelo.Add(new JProperty("TipoCambio", FacturaEncabezado.TipoCambio));
        pModelo.Add(new JProperty("NumeroParcialidades", FacturaEncabezado.NumeroParcialidades));
        pModelo.Add(new JProperty("Parcialidades", FacturaEncabezado.Parcialidades));

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(FacturaEncabezado.IdUsuarioAgente, pConexion);

        pModelo.Add(new JProperty("IdUsuarioAgente", FacturaEncabezado.IdUsuarioAgente));
        pModelo.Add(new JProperty("Agente", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno));
        pModelo.Add(new JProperty("MotivoCancelacion", FacturaEncabezado.MotivoCancelacion));

        CDivision Division = new CDivision();
        Division.LlenaObjeto(FacturaEncabezado.IdDivision, pConexion);
        pModelo.Add(new JProperty("IdDivision", FacturaEncabezado.IdDivision));
        pModelo.Add(new JProperty("Division", Division.Division));
        pModelo.Add(new JProperty("Nota", FacturaEncabezado.Nota));
        pModelo.Add(new JProperty("Subtotal", FacturaEncabezado.Subtotal));
        pModelo.Add(new JProperty("Descuento", FacturaEncabezado.Descuento));
        pModelo.Add(new JProperty("SubtotalDescuento", FacturaEncabezado.Subtotal - FacturaEncabezado.Descuento));
        pModelo.Add(new JProperty("IVA", FacturaEncabezado.IVA));
        pModelo.Add(new JProperty("Total", FacturaEncabezado.Total));
        pModelo.Add(new JProperty("CantidadLetra", FacturaEncabezado.TotalLetra));



        CTxtTimbradosFactura TxtTimbradosFactura = new CTxtTimbradosFactura();
        Dictionary<string, object> ParametrosTXT = new Dictionary<string, object>();
        ParametrosTXT.Add("Folio", Convert.ToInt32(FacturaEncabezado.NumeroFactura));
        ParametrosTXT.Add("Serie", Convert.ToString(SerieFactura.SerieFactura));
        TxtTimbradosFactura.LlenaObjetoFiltros(ParametrosTXT, pConexion);

        if (TxtTimbradosFactura.IdTxtTimbradosFactura != 0)
        {
            pModelo.Add(new JProperty("IdTxtTimbradosFactura", TxtTimbradosFactura.IdTxtTimbradosFactura));
        }
        else
        {
            pModelo.Add(new JProperty("IdTxtTimbradosFactura", 0));
        }

        return pModelo;
    }

    public int ExisteFacturaEncabezadoMovimientos(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteMovimiento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_FacturaEncabezadoMovimientos_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", Convert.ToInt32(pIdFacturaEncabezado));
        ObtenObjeto.Llena<CNotaCreditoEncabezadoFactura>(typeof(CNotaCreditoEncabezadoFactura), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteMovimiento = 1;
        }
        return ExisteMovimiento;
    }

    public int ExisteFacturaEncabezadoTimbrada(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteFacturaEncabezado = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_TxtTimbradosFactura_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", Convert.ToInt32(pIdFacturaEncabezado));
        ObtenObjeto.Llena<CTxtTimbradosFactura>(typeof(CTxtTimbradosFactura), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteFacturaEncabezado = 1;
        }
        return ExisteFacturaEncabezado;
    }

    public int ValidaFactura(int pIdSerieFactura, int pNumeroFactura, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ValidaFactura = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_FacturaEncabezado_ValidaFactura";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", Convert.ToInt32(pIdSerieFactura));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", Convert.ToInt32(pNumeroFactura));
        ObtenObjeto.Llena<CFacturaEncabezado>(typeof(CFacturaEncabezado), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ValidaFactura = 1;
        }
        return ValidaFactura;
    }

    public int ValidaExisteFactura(int pIdSerieFactura, int pNumeroFactura, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ValidaFactura = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_FacturaEncabezadoSustituye_ValidaExisteFactura";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", Convert.ToInt32(pIdSerieFactura));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", Convert.ToInt32(pNumeroFactura));
        ObtenObjeto.Llena<CFacturaEncabezadoSustituye>(typeof(CFacturaEncabezadoSustituye), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ValidaFactura = 1;
        }
        return ValidaFactura;
    }

    public void EditarFacturaEncabezado(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_FacturaEncabezado_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", numeroFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", idSerieFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", razonSocial);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRFC", rFC);
        if (fechaRequeridaFacturacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRequeridaFacturacion", fechaRequeridaFacturacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroParcialidades", numeroParcialidades);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        if (fechaEmision.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
        }
        if (fechaPago.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoFactura", saldoFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroCuenta", numeroCuenta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCalleFiscal", calleFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExteriorFiscal", numeroExteriorFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInteriorFiscal", numeroInteriorFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pColoniaFiscal", coloniaFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostalFiscal", codigoPostalFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPaisFiscal", paisFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEstadoFiscal", estadoFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMunicipioFiscal", municipioFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLocalidadFiscal", localidadFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pReferenciaFiscal", referenciaFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipioFiscal", idMunicipioFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidadFiscal", idLocalidadFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCalleEntrega", calleEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExteriorEntrega", numeroExteriorEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInteriorEntrega", numeroInteriorEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pColoniaEntrega", coloniaEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostalEntrega", codigoPostalEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pReferenciaEntrega", referenciaEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPaisEntrega", paisEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEstadoEntrega", estadoEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMunicipioEntrega", municipioEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLocalidadEntrega", localidadEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipioEntrega", idMunicipioEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidadEntrega", idLocalidadEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", idEstatusFacturaEncabezado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMetodoPago", metodoPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCondicionPago", condicionPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdNumeroCuenta", idNumeroCuenta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRegimenFiscal", regimenFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLugarExpedicion", lugarExpedicion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsRefactura", esRefactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pParcialidades", parcialidades);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroOrdenCompra", numeroOrdenCompra);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
        if (fechaCancelacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void EditarFacturaEncabezadoFolio(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_FacturaEncabezado_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", numeroFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", idSerieFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", razonSocial);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRFC", rFC);
        if (fechaRequeridaFacturacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRequeridaFacturacion", fechaRequeridaFacturacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMetodoPago", idMetodoPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", idCondicionPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroParcialidades", numeroParcialidades);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        if (fechaEmision.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaEmision", fechaEmision);
        }
        if (fechaPago.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSaldoFactura", saldoFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroCuenta", numeroCuenta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCalleFiscal", calleFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExteriorFiscal", numeroExteriorFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInteriorFiscal", numeroInteriorFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pColoniaFiscal", coloniaFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostalFiscal", codigoPostalFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPaisFiscal", paisFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEstadoFiscal", estadoFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMunicipioFiscal", municipioFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLocalidadFiscal", localidadFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pReferenciaFiscal", referenciaFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipioFiscal", idMunicipioFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidadFiscal", idLocalidadFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCalleEntrega", calleEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroExteriorEntrega", numeroExteriorEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroInteriorEntrega", numeroInteriorEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pColoniaEntrega", coloniaEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCodigoPostalEntrega", codigoPostalEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pReferenciaEntrega", referenciaEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPaisEntrega", paisEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEstadoEntrega", estadoEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMunicipioEntrega", municipioEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLocalidadEntrega", localidadEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdMunicipioEntrega", idMunicipioEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdLocalidadEntrega", idLocalidadEntrega);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", idEstatusFacturaEncabezado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMetodoPago", metodoPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pCondicionPago", condicionPago);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdNumeroCuenta", idNumeroCuenta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRegimenFiscal", regimenFiscal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pLugarExpedicion", lugarExpedicion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsRefactura", esRefactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pParcialidades", parcialidades);
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroOrdenCompra", numeroOrdenCompra);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCancelacion", idUsuarioCancelacion);
        if (fechaCancelacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCancelacion", fechaCancelacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pMotivoCancelacion", motivoCancelacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void AgregarFacturaIndividual(CConexion pConexion, string pDescripcion, decimal pMonto)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_FacturaEncabezado_AgregarFacturaIndividual";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
        Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezado"].Direction = ParameterDirection.InputOutput;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", pDescripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", pMonto);
        Agregar.Insert(pConexion);
        idFacturaEncabezado = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezado"].Value);
    }

    public static JToken obtenerDatosImpresionNotaVenta(string pIdFacturaEncabezado, int pUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCotizacion jsonImpresion = new CCotizacion();
        jsonImpresion.StoredProcedure.CommandText = "SP_Impresion_NotaVenta";
        jsonImpresion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresion.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", pIdFacturaEncabezado);
        jsonImpresion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        return jsonImpresion.ObtenerJsonJObject(ConexionBaseDatos);
    }

    public void ActualizarEstatusFacturadoCotizacion(int pIdFacturaEncabezado, int pIdEstatusFacturaEncabezado, CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_FacturaEncabezado_Editar_ActualizarEstatusFacturadoCotizacion";
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", pIdFacturaEncabezado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusFacturaEncabezado", pIdEstatusFacturaEncabezado);
        Editar.Update(pConexion);
    }

    public int ExistenPartidasPendientesCotizacion(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int exitenPartidasPendientes = 0;
        Select.StoredProcedure.CommandText = "sp_FacturaEncabezado_Consultar_ExistenPartidasPendientesCotizacion";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", pIdFacturaEncabezado);
        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            exitenPartidasPendientes = Convert.ToInt32(Select.Registros["ExistenPartidasPendientes"]);
        }
        Select.CerrarConsulta();
        return exitenPartidasPendientes;
    }

    public int ValidarEsFacturaMesAnterior(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int esFacturaMesAnterior = 0;
        Select.StoredProcedure.CommandText = "sp_FacturaEncabezado_Consultar_ValidarEsFacturaMesAnterior";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", pIdFacturaEncabezado);
        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            esFacturaMesAnterior = Convert.ToInt32(Select.Registros["EsFacturaMesAnterior"]);
        }
        Select.CerrarConsulta();
        return esFacturaMesAnterior;
    }
}