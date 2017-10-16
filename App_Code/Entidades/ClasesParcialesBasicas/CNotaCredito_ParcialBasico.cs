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
    //Propiedades Privadas
    private int idNotaCredito;
    private string descripcion;
    private int folioNotaCredito;
    private string serieNotaCredito;
    private DateTime fecha;
    private int idCliente;
    private decimal monto;
    private decimal iVA;
    private decimal total;
    private int idTipoMoneda;
    private decimal tipoCambio;
    private decimal saldoDocumento;
    private string referencia;
    private decimal porcentajeIVA;
    private string totalLetra;
    private int idUsuarioAlta;
    private DateTime fechaAlta;
    private DateTime fechaCancelacion;
    private string motivoCancelacion;
    private int idUsuarioCancelacion;
    private int idTipoNotaCredito;
    private string refid;
    private bool asociado;
    private bool baja;

    //Propiedades
    public int IdNotaCredito
    {
        get { return idNotaCredito; }
        set
        {
            idNotaCredito = value;
        }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set
        {
            descripcion = value;
        }
    }

    public int FolioNotaCredito
    {
        get { return folioNotaCredito; }
        set
        {
            folioNotaCredito = value;
        }
    }

    public string SerieNotaCredito
    {
        get { return serieNotaCredito; }
        set
        {
            serieNotaCredito = value;
        }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public int IdCliente
    {
        get { return idCliente; }
        set
        {
            idCliente = value;
        }
    }

    public decimal Monto
    {
        get { return monto; }
        set
        {
            monto = value;
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

    public int IdTipoMoneda
    {
        get { return idTipoMoneda; }
        set
        {
            idTipoMoneda = value;
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

    public decimal SaldoDocumento
    {
        get { return saldoDocumento; }
        set
        {
            saldoDocumento = value;
        }
    }

    public string Referencia
    {
        get { return referencia; }
        set
        {
            referencia = value;
        }
    }

    public decimal PorcentajeIVA
    {
        get { return porcentajeIVA; }
        set
        {
            porcentajeIVA = value;
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

    public int IdUsuarioAlta
    {
        get { return idUsuarioAlta; }
        set
        {
            idUsuarioAlta = value;
        }
    }

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
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

    public int IdUsuarioCancelacion
    {
        get { return idUsuarioCancelacion; }
        set
        {
            idUsuarioCancelacion = value;
        }
    }

    public int IdTipoNotaCredito
    {
        get { return idTipoNotaCredito; }
        set
        {
            idTipoNotaCredito = value;
        }
    }

    public string Refid
    {
        get { return refid; }
        set
        {
            refid = value;
        }
    }

    public bool Asociado
    {
        get { return asociado; }
        set { asociado = value; }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CNotaCredito()
    {
        idNotaCredito = 0;
        descripcion = "";
        folioNotaCredito = 0;
        serieNotaCredito = "";
        fecha = new DateTime(1, 1, 1);
        idCliente = 0;
        monto = 0;
        iVA = 0;
        total = 0;
        idTipoMoneda = 0;
        tipoCambio = 0;
        saldoDocumento = 0;
        referencia = "";
        porcentajeIVA = 0;
        totalLetra = "";
        idUsuarioAlta = 0;
        fechaAlta = new DateTime(1, 1, 1);
        fechaCancelacion = new DateTime(1, 1, 1);
        motivoCancelacion = "";
        idUsuarioCancelacion = 0;
        idTipoNotaCredito = 0;
        refid = "";
        asociado = false;
        baja = false;
    }

    public CNotaCredito(int pIdNotaCredito)
    {
        idNotaCredito = pIdNotaCredito;
        descripcion = "";
        folioNotaCredito = 0;
        serieNotaCredito = "";
        fecha = new DateTime(1, 1, 1);
        idCliente = 0;
        monto = 0;
        iVA = 0;
        total = 0;
        idTipoMoneda = 0;
        tipoCambio = 0;
        saldoDocumento = 0;
        referencia = "";
        porcentajeIVA = 0;
        totalLetra = "";
        idUsuarioAlta = 0;
        fechaAlta = new DateTime(1, 1, 1);
        fechaCancelacion = new DateTime(1, 1, 1);
        motivoCancelacion = "";
        idUsuarioCancelacion = 0;
        idTipoNotaCredito = 0;
        refid = "";
        asociado = false;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_NotaCredito_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CNotaCredito>(typeof(CNotaCredito), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_NotaCredito_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CNotaCredito>(typeof(CNotaCredito), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_NotaCredito_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CNotaCredito>(typeof(CNotaCredito), pConexion);
        foreach (CNotaCredito O in Obten.ListaRegistros)
        {
            idNotaCredito = O.IdNotaCredito;
            descripcion = O.Descripcion;
            folioNotaCredito = O.FolioNotaCredito;
            serieNotaCredito = O.SerieNotaCredito;
            fecha = O.Fecha;
            idCliente = O.IdCliente;
            monto = O.Monto;
            iVA = O.IVA;
            total = O.Total;
            idTipoMoneda = O.IdTipoMoneda;
            tipoCambio = O.TipoCambio;
            saldoDocumento = O.SaldoDocumento;
            referencia = O.Referencia;
            porcentajeIVA = O.PorcentajeIVA;
            totalLetra = O.TotalLetra;
            idUsuarioAlta = O.IdUsuarioAlta;
            fechaAlta = O.FechaAlta;
            fechaCancelacion = O.FechaCancelacion;
            motivoCancelacion = O.MotivoCancelacion;
            idUsuarioCancelacion = O.IdUsuarioCancelacion;
            idTipoNotaCredito = O.IdTipoNotaCredito;
            refid = O.Refid;
            asociado = O.Asociado;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_NotaCredito_ConsultarFiltros";
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
        Obten.Llena<CNotaCredito>(typeof(CNotaCredito), pConexion);
        foreach (CNotaCredito O in Obten.ListaRegistros)
        {
            idNotaCredito = O.IdNotaCredito;
            descripcion = O.Descripcion;
            folioNotaCredito = O.FolioNotaCredito;
            serieNotaCredito = O.SerieNotaCredito;
            fecha = O.Fecha;
            idCliente = O.IdCliente;
            monto = O.Monto;
            iVA = O.IVA;
            total = O.Total;
            idTipoMoneda = O.IdTipoMoneda;
            tipoCambio = O.TipoCambio;
            saldoDocumento = O.SaldoDocumento;
            referencia = O.Referencia;
            porcentajeIVA = O.PorcentajeIVA;
            totalLetra = O.TotalLetra;
            idUsuarioAlta = O.IdUsuarioAlta;
            fechaAlta = O.FechaAlta;
            fechaCancelacion = O.FechaCancelacion;
            motivoCancelacion = O.MotivoCancelacion;
            idUsuarioCancelacion = O.IdUsuarioCancelacion;
            idTipoNotaCredito = O.IdTipoNotaCredito;
            refid = O.Refid;
            asociado = O.Asociado;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_NotaCredito_ConsultarFiltros";
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
        Obten.Llena<CNotaCredito>(typeof(CNotaCredito), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_NotaCredito_Agregar";
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
        Agregar.StoredProcedure.Parameters.AddWithValue("@pRefid", refid);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idNotaCredito = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNotaCredito"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_NotaCredito_Editar";
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
        Editar.StoredProcedure.Parameters.AddWithValue("@pAsociado", asociado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_NotaCredito_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}