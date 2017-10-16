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
    //Propiedades Privadas
    private int idFacturaEncabezado;
    private int numeroFactura;
    private int idSerieFactura;
    private int idUsuario;
    private int idUsuarioAgente;
    private int idCliente;
    private string razonSocial;
    private string rFC;
    private DateTime fechaRequeridaFacturacion;
    private int idMetodoPago;
    private int idCondicionPago;
    private int numeroParcialidades;
    private string nota;
    private DateTime fechaEmision;
    private DateTime fechaPago;
    private decimal subtotal;
    private decimal iVA;
    private decimal total;
    private string totalLetra;
    private decimal saldoFactura;
    private decimal descuento;
    private string numeroCuenta;
    private string calleFiscal;
    private string numeroExteriorFiscal;
    private string numeroInteriorFiscal;
    private string coloniaFiscal;
    private string codigoPostalFiscal;
    private string paisFiscal;
    private string estadoFiscal;
    private string municipioFiscal;
    private string localidadFiscal;
    private string referenciaFiscal;
    private int idMunicipioFiscal;
    private int idLocalidadFiscal;
    private string calleEntrega;
    private string numeroExteriorEntrega;
    private string numeroInteriorEntrega;
    private string coloniaEntrega;
    private string codigoPostalEntrega;
    private string referenciaEntrega;
    private string paisEntrega;
    private string estadoEntrega;
    private string municipioEntrega;
    private string localidadEntrega;
    private int idMunicipioEntrega;
    private int idLocalidadEntrega;
    private int idDivision;
    private int idTipoMoneda;
    private int idEstatusFacturaEncabezado;
    private decimal tipoCambio;
    private string metodoPago;
    private string condicionPago;
    private int idNumeroCuenta;
    private string regimenFiscal;
    private string lugarExpedicion;
    private bool esRefactura;
    private bool parcialidades;
    private string numeroOrdenCompra;
    private int idUsuarioCancelacion;
    private DateTime fechaCancelacion;
    private string motivoCancelacion;
    private int numeroParcialidadesPendientes;
    private int idFacturaGlobal;
    private string uUIDGlobal;
    private string fechaGlobal;
    private decimal montoGlobal;
    private string serieGlobal;
    private bool parcialidadIndividual;
    private string folioGlobal;
    private string serie;
    private decimal porcentajeDescuento;
    private int idDescuentoCliente;
    private bool seGeneroAsiento;
    private string refid;
    private int idContactoCliente;
    private bool baja;

    //Propiedades
    public int IdFacturaEncabezado
    {
        get { return idFacturaEncabezado; }
        set
        {
            idFacturaEncabezado = value;
        }
    }

    public int NumeroFactura
    {
        get { return numeroFactura; }
        set
        {
            numeroFactura = value;
        }
    }

    public int IdSerieFactura
    {
        get { return idSerieFactura; }
        set
        {
            idSerieFactura = value;
        }
    }

    public int IdUsuario
    {
        get { return idUsuario; }
        set
        {
            idUsuario = value;
        }
    }

    public int IdUsuarioAgente
    {
        get { return idUsuarioAgente; }
        set
        {
            idUsuarioAgente = value;
        }
    }

    public int IdCliente
    {
        get { return idCliente; }
        set
        {
            idCliente = value;
        }
    }

    public string RazonSocial
    {
        get { return razonSocial; }
        set
        {
            razonSocial = value;
        }
    }

    public string RFC
    {
        get { return rFC; }
        set
        {
            rFC = value;
        }
    }

    public DateTime FechaRequeridaFacturacion
    {
        get { return fechaRequeridaFacturacion; }
        set { fechaRequeridaFacturacion = value; }
    }

    public int IdMetodoPago
    {
        get { return idMetodoPago; }
        set
        {
            idMetodoPago = value;
        }
    }

    public int IdCondicionPago
    {
        get { return idCondicionPago; }
        set
        {
            idCondicionPago = value;
        }
    }

    public int NumeroParcialidades
    {
        get { return numeroParcialidades; }
        set
        {
            numeroParcialidades = value;
        }
    }

    public string Nota
    {
        get { return nota; }
        set
        {
            nota = value;
        }
    }

    public DateTime FechaEmision
    {
        get { return fechaEmision; }
        set { fechaEmision = value; }
    }

    public DateTime FechaPago
    {
        get { return fechaPago; }
        set { fechaPago = value; }
    }

    public decimal Subtotal
    {
        get { return subtotal; }
        set
        {
            subtotal = value;
        }
    }

    public decimal IVA
    {
        get { return iVA; }
        set
        {
            iVA = value;
        }
    }

    public decimal Total
    {
        get { return total; }
        set
        {
            total = value;
        }
    }

    public string TotalLetra
    {
        get { return totalLetra; }
        set
        {
            totalLetra = value;
        }
    }

    public decimal SaldoFactura
    {
        get { return saldoFactura; }
        set
        {
            saldoFactura = value;
        }
    }

    public decimal Descuento
    {
        get { return descuento; }
        set
        {
            descuento = value;
        }
    }

    public string NumeroCuenta
    {
        get { return numeroCuenta; }
        set
        {
            numeroCuenta = value;
        }
    }

    public string CalleFiscal
    {
        get { return calleFiscal; }
        set
        {
            calleFiscal = value;
        }
    }

    public string NumeroExteriorFiscal
    {
        get { return numeroExteriorFiscal; }
        set
        {
            numeroExteriorFiscal = value;
        }
    }

    public string NumeroInteriorFiscal
    {
        get { return numeroInteriorFiscal; }
        set
        {
            numeroInteriorFiscal = value;
        }
    }

    public string ColoniaFiscal
    {
        get { return coloniaFiscal; }
        set
        {
            coloniaFiscal = value;
        }
    }

    public string CodigoPostalFiscal
    {
        get { return codigoPostalFiscal; }
        set
        {
            codigoPostalFiscal = value;
        }
    }

    public string PaisFiscal
    {
        get { return paisFiscal; }
        set
        {
            paisFiscal = value;
        }
    }

    public string EstadoFiscal
    {
        get { return estadoFiscal; }
        set
        {
            estadoFiscal = value;
        }
    }

    public string MunicipioFiscal
    {
        get { return municipioFiscal; }
        set
        {
            municipioFiscal = value;
        }
    }

    public string LocalidadFiscal
    {
        get { return localidadFiscal; }
        set
        {
            localidadFiscal = value;
        }
    }

    public string ReferenciaFiscal
    {
        get { return referenciaFiscal; }
        set
        {
            referenciaFiscal = value;
        }
    }

    public int IdMunicipioFiscal
    {
        get { return idMunicipioFiscal; }
        set
        {
            idMunicipioFiscal = value;
        }
    }

    public int IdLocalidadFiscal
    {
        get { return idLocalidadFiscal; }
        set
        {
            idLocalidadFiscal = value;
        }
    }

    public string CalleEntrega
    {
        get { return calleEntrega; }
        set
        {
            calleEntrega = value;
        }
    }

    public string NumeroExteriorEntrega
    {
        get { return numeroExteriorEntrega; }
        set
        {
            numeroExteriorEntrega = value;
        }
    }

    public string NumeroInteriorEntrega
    {
        get { return numeroInteriorEntrega; }
        set
        {
            numeroInteriorEntrega = value;
        }
    }

    public string ColoniaEntrega
    {
        get { return coloniaEntrega; }
        set
        {
            coloniaEntrega = value;
        }
    }

    public string CodigoPostalEntrega
    {
        get { return codigoPostalEntrega; }
        set
        {
            codigoPostalEntrega = value;
        }
    }

    public string ReferenciaEntrega
    {
        get { return referenciaEntrega; }
        set
        {
            referenciaEntrega = value;
        }
    }

    public string PaisEntrega
    {
        get { return paisEntrega; }
        set
        {
            paisEntrega = value;
        }
    }

    public string EstadoEntrega
    {
        get { return estadoEntrega; }
        set
        {
            estadoEntrega = value;
        }
    }

    public string MunicipioEntrega
    {
        get { return municipioEntrega; }
        set
        {
            municipioEntrega = value;
        }
    }

    public string LocalidadEntrega
    {
        get { return localidadEntrega; }
        set
        {
            localidadEntrega = value;
        }
    }

    public int IdMunicipioEntrega
    {
        get { return idMunicipioEntrega; }
        set
        {
            idMunicipioEntrega = value;
        }
    }

    public int IdLocalidadEntrega
    {
        get { return idLocalidadEntrega; }
        set
        {
            idLocalidadEntrega = value;
        }
    }

    public int IdDivision
    {
        get { return idDivision; }
        set
        {
            idDivision = value;
        }
    }

    public int IdTipoMoneda
    {
        get { return idTipoMoneda; }
        set
        {
            idTipoMoneda = value;
        }
    }

    public int IdEstatusFacturaEncabezado
    {
        get { return idEstatusFacturaEncabezado; }
        set
        {
            idEstatusFacturaEncabezado = value;
        }
    }

    public decimal TipoCambio
    {
        get { return tipoCambio; }
        set
        {
            tipoCambio = value;
        }
    }

    public string MetodoPago
    {
        get { return metodoPago; }
        set
        {
            metodoPago = value;
        }
    }

    public string CondicionPago
    {
        get { return condicionPago; }
        set
        {
            condicionPago = value;
        }
    }

    public int IdNumeroCuenta
    {
        get { return idNumeroCuenta; }
        set
        {
            idNumeroCuenta = value;
        }
    }

    public string RegimenFiscal
    {
        get { return regimenFiscal; }
        set
        {
            regimenFiscal = value;
        }
    }

    public string LugarExpedicion
    {
        get { return lugarExpedicion; }
        set
        {
            lugarExpedicion = value;
        }
    }

    public bool EsRefactura
    {
        get { return esRefactura; }
        set { esRefactura = value; }
    }

    public bool Parcialidades
    {
        get { return parcialidades; }
        set { parcialidades = value; }
    }

    public string NumeroOrdenCompra
    {
        get { return numeroOrdenCompra; }
        set
        {
            numeroOrdenCompra = value;
        }
    }

    public int IdUsuarioCancelacion
    {
        get { return idUsuarioCancelacion; }
        set
        {
            idUsuarioCancelacion = value;
        }
    }

    public DateTime FechaCancelacion
    {
        get { return fechaCancelacion; }
        set { fechaCancelacion = value; }
    }

    public string MotivoCancelacion
    {
        get { return motivoCancelacion; }
        set
        {
            motivoCancelacion = value;
        }
    }

    public int NumeroParcialidadesPendientes
    {
        get { return numeroParcialidadesPendientes; }
        set
        {
            numeroParcialidadesPendientes = value;
        }
    }

    public int IdFacturaGlobal
    {
        get { return idFacturaGlobal; }
        set
        {
            idFacturaGlobal = value;
        }
    }

    public string UUIDGlobal
    {
        get { return uUIDGlobal; }
        set
        {
            uUIDGlobal = value;
        }
    }

    public string FechaGlobal
    {
        get { return fechaGlobal; }
        set
        {
            fechaGlobal = value;
        }
    }

    public decimal MontoGlobal
    {
        get { return montoGlobal; }
        set
        {
            montoGlobal = value;
        }
    }

    public string SerieGlobal
    {
        get { return serieGlobal; }
        set
        {
            serieGlobal = value;
        }
    }

    public bool ParcialidadIndividual
    {
        get { return parcialidadIndividual; }
        set { parcialidadIndividual = value; }
    }

    public string FolioGlobal
    {
        get { return folioGlobal; }
        set
        {
            folioGlobal = value;
        }
    }

    public string Serie
    {
        get { return serie; }
        set
        {
            serie = value;
        }
    }

    public decimal PorcentajeDescuento
    {
        get { return porcentajeDescuento; }
        set
        {
            porcentajeDescuento = value;
        }
    }

    public int IdDescuentoCliente
    {
        get { return idDescuentoCliente; }
        set
        {
            idDescuentoCliente = value;
        }
    }

    public bool SeGeneroAsiento
    {
        get { return seGeneroAsiento; }
        set { seGeneroAsiento = value; }
    }

    public string Refid
    {
        get { return refid; }
        set
        {
            refid = value;
        }
    }

    public int IdContactoCliente
    {
        get { return idContactoCliente; }
        set
        {
            idContactoCliente = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CFacturaEncabezado()
    {
        idFacturaEncabezado = 0;
        numeroFactura = 0;
        idSerieFactura = 0;
        idUsuario = 0;
        idUsuarioAgente = 0;
        idCliente = 0;
        razonSocial = "";
        rFC = "";
        fechaRequeridaFacturacion = new DateTime(1, 1, 1);
        idMetodoPago = 0;
        idCondicionPago = 0;
        numeroParcialidades = 0;
        nota = "";
        fechaEmision = new DateTime(1, 1, 1);
        fechaPago = new DateTime(1, 1, 1);
        subtotal = 0;
        iVA = 0;
        total = 0;
        totalLetra = "";
        saldoFactura = 0;
        descuento = 0;
        numeroCuenta = "";
        calleFiscal = "";
        numeroExteriorFiscal = "";
        numeroInteriorFiscal = "";
        coloniaFiscal = "";
        codigoPostalFiscal = "";
        paisFiscal = "";
        estadoFiscal = "";
        municipioFiscal = "";
        localidadFiscal = "";
        referenciaFiscal = "";
        idMunicipioFiscal = 0;
        idLocalidadFiscal = 0;
        calleEntrega = "";
        numeroExteriorEntrega = "";
        numeroInteriorEntrega = "";
        coloniaEntrega = "";
        codigoPostalEntrega = "";
        referenciaEntrega = "";
        paisEntrega = "";
        estadoEntrega = "";
        municipioEntrega = "";
        localidadEntrega = "";
        idMunicipioEntrega = 0;
        idLocalidadEntrega = 0;
        idDivision = 0;
        idTipoMoneda = 0;
        idEstatusFacturaEncabezado = 0;
        tipoCambio = 0;
        metodoPago = "";
        condicionPago = "";
        idNumeroCuenta = 0;
        regimenFiscal = "";
        lugarExpedicion = "";
        esRefactura = false;
        parcialidades = false;
        numeroOrdenCompra = "";
        idUsuarioCancelacion = 0;
        fechaCancelacion = new DateTime(1, 1, 1);
        motivoCancelacion = "";
        numeroParcialidadesPendientes = 0;
        idFacturaGlobal = 0;
        uUIDGlobal = "";
        fechaGlobal = "";
        montoGlobal = 0;
        serieGlobal = "";
        parcialidadIndividual = false;
        folioGlobal = "";
        serie = "";
        porcentajeDescuento = 0;
        idDescuentoCliente = 0;
        seGeneroAsiento = false;
        refid = "";
        idContactoCliente = 0;
        baja = false;
    }

    public CFacturaEncabezado(int pIdFacturaEncabezado)
    {
        idFacturaEncabezado = pIdFacturaEncabezado;
        numeroFactura = 0;
        idSerieFactura = 0;
        idUsuario = 0;
        idUsuarioAgente = 0;
        idCliente = 0;
        razonSocial = "";
        rFC = "";
        fechaRequeridaFacturacion = new DateTime(1, 1, 1);
        idMetodoPago = 0;
        idCondicionPago = 0;
        numeroParcialidades = 0;
        nota = "";
        fechaEmision = new DateTime(1, 1, 1);
        fechaPago = new DateTime(1, 1, 1);
        subtotal = 0;
        iVA = 0;
        total = 0;
        totalLetra = "";
        saldoFactura = 0;
        descuento = 0;
        numeroCuenta = "";
        calleFiscal = "";
        numeroExteriorFiscal = "";
        numeroInteriorFiscal = "";
        coloniaFiscal = "";
        codigoPostalFiscal = "";
        paisFiscal = "";
        estadoFiscal = "";
        municipioFiscal = "";
        localidadFiscal = "";
        referenciaFiscal = "";
        idMunicipioFiscal = 0;
        idLocalidadFiscal = 0;
        calleEntrega = "";
        numeroExteriorEntrega = "";
        numeroInteriorEntrega = "";
        coloniaEntrega = "";
        codigoPostalEntrega = "";
        referenciaEntrega = "";
        paisEntrega = "";
        estadoEntrega = "";
        municipioEntrega = "";
        localidadEntrega = "";
        idMunicipioEntrega = 0;
        idLocalidadEntrega = 0;
        idDivision = 0;
        idTipoMoneda = 0;
        idEstatusFacturaEncabezado = 0;
        tipoCambio = 0;
        metodoPago = "";
        condicionPago = "";
        idNumeroCuenta = 0;
        regimenFiscal = "";
        lugarExpedicion = "";
        esRefactura = false;
        parcialidades = false;
        numeroOrdenCompra = "";
        idUsuarioCancelacion = 0;
        fechaCancelacion = new DateTime(1, 1, 1);
        motivoCancelacion = "";
        numeroParcialidadesPendientes = 0;
        idFacturaGlobal = 0;
        uUIDGlobal = "";
        fechaGlobal = "";
        montoGlobal = 0;
        serieGlobal = "";
        parcialidadIndividual = false;
        folioGlobal = "";
        serie = "";
        porcentajeDescuento = 0;
        idDescuentoCliente = 0;
        seGeneroAsiento = false;
        refid = "";
        idContactoCliente = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_FacturaEncabezado_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CFacturaEncabezado>(typeof(CFacturaEncabezado), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_FacturaEncabezado_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CFacturaEncabezado>(typeof(CFacturaEncabezado), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_FacturaEncabezado_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CFacturaEncabezado>(typeof(CFacturaEncabezado), pConexion);
        foreach (CFacturaEncabezado O in Obten.ListaRegistros)
        {
            idFacturaEncabezado = O.IdFacturaEncabezado;
            numeroFactura = O.NumeroFactura;
            idSerieFactura = O.IdSerieFactura;
            idUsuario = O.IdUsuario;
            idUsuarioAgente = O.IdUsuarioAgente;
            idCliente = O.IdCliente;
            razonSocial = O.RazonSocial;
            rFC = O.RFC;
            fechaRequeridaFacturacion = O.FechaRequeridaFacturacion;
            idMetodoPago = O.IdMetodoPago;
            idCondicionPago = O.IdCondicionPago;
            numeroParcialidades = O.NumeroParcialidades;
            nota = O.Nota;
            fechaEmision = O.FechaEmision;
            fechaPago = O.FechaPago;
            subtotal = O.Subtotal;
            iVA = O.IVA;
            total = O.Total;
            totalLetra = O.TotalLetra;
            saldoFactura = O.SaldoFactura;
            descuento = O.Descuento;
            numeroCuenta = O.NumeroCuenta;
            calleFiscal = O.CalleFiscal;
            numeroExteriorFiscal = O.NumeroExteriorFiscal;
            numeroInteriorFiscal = O.NumeroInteriorFiscal;
            coloniaFiscal = O.ColoniaFiscal;
            codigoPostalFiscal = O.CodigoPostalFiscal;
            paisFiscal = O.PaisFiscal;
            estadoFiscal = O.EstadoFiscal;
            municipioFiscal = O.MunicipioFiscal;
            localidadFiscal = O.LocalidadFiscal;
            referenciaFiscal = O.ReferenciaFiscal;
            idMunicipioFiscal = O.IdMunicipioFiscal;
            idLocalidadFiscal = O.IdLocalidadFiscal;
            calleEntrega = O.CalleEntrega;
            numeroExteriorEntrega = O.NumeroExteriorEntrega;
            numeroInteriorEntrega = O.NumeroInteriorEntrega;
            coloniaEntrega = O.ColoniaEntrega;
            codigoPostalEntrega = O.CodigoPostalEntrega;
            referenciaEntrega = O.ReferenciaEntrega;
            paisEntrega = O.PaisEntrega;
            estadoEntrega = O.EstadoEntrega;
            municipioEntrega = O.MunicipioEntrega;
            localidadEntrega = O.LocalidadEntrega;
            idMunicipioEntrega = O.IdMunicipioEntrega;
            idLocalidadEntrega = O.IdLocalidadEntrega;
            idDivision = O.IdDivision;
            idTipoMoneda = O.IdTipoMoneda;
            idEstatusFacturaEncabezado = O.IdEstatusFacturaEncabezado;
            tipoCambio = O.TipoCambio;
            metodoPago = O.MetodoPago;
            condicionPago = O.CondicionPago;
            idNumeroCuenta = O.IdNumeroCuenta;
            regimenFiscal = O.RegimenFiscal;
            lugarExpedicion = O.LugarExpedicion;
            esRefactura = O.EsRefactura;
            parcialidades = O.Parcialidades;
            numeroOrdenCompra = O.NumeroOrdenCompra;
            idUsuarioCancelacion = O.IdUsuarioCancelacion;
            fechaCancelacion = O.FechaCancelacion;
            motivoCancelacion = O.MotivoCancelacion;
            numeroParcialidadesPendientes = O.NumeroParcialidadesPendientes;
            idFacturaGlobal = O.IdFacturaGlobal;
            uUIDGlobal = O.UUIDGlobal;
            fechaGlobal = O.FechaGlobal;
            montoGlobal = O.MontoGlobal;
            serieGlobal = O.SerieGlobal;
            parcialidadIndividual = O.ParcialidadIndividual;
            folioGlobal = O.FolioGlobal;
            serie = O.Serie;
            porcentajeDescuento = O.PorcentajeDescuento;
            idDescuentoCliente = O.IdDescuentoCliente;
            seGeneroAsiento = O.SeGeneroAsiento;
            refid = O.Refid;
            idContactoCliente = O.IdContactoCliente;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_FacturaEncabezado_ConsultarFiltros";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CFacturaEncabezado>(typeof(CFacturaEncabezado), pConexion);
        foreach (CFacturaEncabezado O in Obten.ListaRegistros)
        {
            idFacturaEncabezado = O.IdFacturaEncabezado;
            numeroFactura = O.NumeroFactura;
            idSerieFactura = O.IdSerieFactura;
            idUsuario = O.IdUsuario;
            idUsuarioAgente = O.IdUsuarioAgente;
            idCliente = O.IdCliente;
            razonSocial = O.RazonSocial;
            rFC = O.RFC;
            fechaRequeridaFacturacion = O.FechaRequeridaFacturacion;
            idMetodoPago = O.IdMetodoPago;
            idCondicionPago = O.IdCondicionPago;
            numeroParcialidades = O.NumeroParcialidades;
            nota = O.Nota;
            fechaEmision = O.FechaEmision;
            fechaPago = O.FechaPago;
            subtotal = O.Subtotal;
            iVA = O.IVA;
            total = O.Total;
            totalLetra = O.TotalLetra;
            saldoFactura = O.SaldoFactura;
            descuento = O.Descuento;
            numeroCuenta = O.NumeroCuenta;
            calleFiscal = O.CalleFiscal;
            numeroExteriorFiscal = O.NumeroExteriorFiscal;
            numeroInteriorFiscal = O.NumeroInteriorFiscal;
            coloniaFiscal = O.ColoniaFiscal;
            codigoPostalFiscal = O.CodigoPostalFiscal;
            paisFiscal = O.PaisFiscal;
            estadoFiscal = O.EstadoFiscal;
            municipioFiscal = O.MunicipioFiscal;
            localidadFiscal = O.LocalidadFiscal;
            referenciaFiscal = O.ReferenciaFiscal;
            idMunicipioFiscal = O.IdMunicipioFiscal;
            idLocalidadFiscal = O.IdLocalidadFiscal;
            calleEntrega = O.CalleEntrega;
            numeroExteriorEntrega = O.NumeroExteriorEntrega;
            numeroInteriorEntrega = O.NumeroInteriorEntrega;
            coloniaEntrega = O.ColoniaEntrega;
            codigoPostalEntrega = O.CodigoPostalEntrega;
            referenciaEntrega = O.ReferenciaEntrega;
            paisEntrega = O.PaisEntrega;
            estadoEntrega = O.EstadoEntrega;
            municipioEntrega = O.MunicipioEntrega;
            localidadEntrega = O.LocalidadEntrega;
            idMunicipioEntrega = O.IdMunicipioEntrega;
            idLocalidadEntrega = O.IdLocalidadEntrega;
            idDivision = O.IdDivision;
            idTipoMoneda = O.IdTipoMoneda;
            idEstatusFacturaEncabezado = O.IdEstatusFacturaEncabezado;
            tipoCambio = O.TipoCambio;
            metodoPago = O.MetodoPago;
            condicionPago = O.CondicionPago;
            idNumeroCuenta = O.IdNumeroCuenta;
            regimenFiscal = O.RegimenFiscal;
            lugarExpedicion = O.LugarExpedicion;
            esRefactura = O.EsRefactura;
            parcialidades = O.Parcialidades;
            numeroOrdenCompra = O.NumeroOrdenCompra;
            idUsuarioCancelacion = O.IdUsuarioCancelacion;
            fechaCancelacion = O.FechaCancelacion;
            motivoCancelacion = O.MotivoCancelacion;
            numeroParcialidadesPendientes = O.NumeroParcialidadesPendientes;
            idFacturaGlobal = O.IdFacturaGlobal;
            uUIDGlobal = O.UUIDGlobal;
            fechaGlobal = O.FechaGlobal;
            montoGlobal = O.MontoGlobal;
            serieGlobal = O.SerieGlobal;
            parcialidadIndividual = O.ParcialidadIndividual;
            folioGlobal = O.FolioGlobal;
            serie = O.Serie;
            porcentajeDescuento = O.PorcentajeDescuento;
            idDescuentoCliente = O.IdDescuentoCliente;
            seGeneroAsiento = O.SeGeneroAsiento;
            refid = O.Refid;
            idContactoCliente = O.IdContactoCliente;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_FacturaEncabezado_ConsultarFiltros";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CFacturaEncabezado>(typeof(CFacturaEncabezado), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_FacturaEncabezado_Agregar";
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
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoCliente", idContactoCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idFacturaEncabezado = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFacturaEncabezado"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_FacturaEncabezado_Editar";
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
        Editar.StoredProcedure.Parameters.AddWithValue("@pNumeroParcialidadesPendientes", numeroParcialidadesPendientes);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaGlobal", idFacturaGlobal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pUUIDGlobal", uUIDGlobal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFechaGlobal", fechaGlobal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pMontoGlobal", montoGlobal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSerieGlobal", serieGlobal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pParcialidadIndividual", parcialidadIndividual);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFolioGlobal", folioGlobal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSerie", serie);
        Editar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeDescuento", porcentajeDescuento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoCliente", idDescuentoCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSeGeneroAsiento", seGeneroAsiento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdContactoCliente", idContactoCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_FacturaEncabezado_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}