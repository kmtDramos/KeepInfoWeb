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

public partial class CEncabezadoRemision
{
    //Propiedades Privadas
    private int idEncabezadoRemision;
    private DateTime fechaRemision;
    private string nota;
    private int idCliente;
    private int idTipoMoneda;
    private int idUsuario;
    private bool consolidado;
    private decimal total;
    private int folio;
    private decimal tipoCambio;
    private DateTime fechaFacturacion;
    private string totalLetra;
    private int idAlmacen;
    private bool baja;

    //Propiedades
    public int IdEncabezadoRemision
    {
        get { return idEncabezadoRemision; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idEncabezadoRemision = value;
        }
    }

    public DateTime FechaRemision
    {
        get { return fechaRemision; }
        set { fechaRemision = value; }
    }

    public string Nota
    {
        get { return nota; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            nota = value;
        }
    }

    public int IdCliente
    {
        get { return idCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idCliente = value;
        }
    }

    public int IdTipoMoneda
    {
        get { return idTipoMoneda; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoMoneda = value;
        }
    }

    public int IdUsuario
    {
        get { return idUsuario; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuario = value;
        }
    }

    public bool Consolidado
    {
        get { return consolidado; }
        set { consolidado = value; }
    }

    public decimal Total
    {
        get { return total; }
        set
        {
            if (value < 0)
            {
                return;
            }
            total = value;
        }
    }

    public int Folio
    {
        get { return folio; }
        set
        {
            if (value < 0)
            {
                return;
            }
            folio = value;
        }
    }

    public decimal TipoCambio
    {
        get { return tipoCambio; }
        set
        {
            if (value < 0)
            {
                return;
            }
            tipoCambio = value;
        }
    }

    public DateTime FechaFacturacion
    {
        get { return fechaFacturacion; }
        set { fechaFacturacion = value; }
    }

    public string TotalLetra
    {
        get { return totalLetra; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            totalLetra = value;
        }
    }

    public int IdAlmacen
    {
        get { return idAlmacen; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAlmacen = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CEncabezadoRemision()
    {
        idEncabezadoRemision = 0;
        fechaRemision = new DateTime(1, 1, 1);
        nota = "";
        idCliente = 0;
        idTipoMoneda = 0;
        idUsuario = 0;
        consolidado = false;
        total = 0;
        folio = 0;
        tipoCambio = 0;
        fechaFacturacion = new DateTime(1, 1, 1);
        totalLetra = "";
        idAlmacen = 0;
        baja = false;
    }

    public CEncabezadoRemision(int pIdEncabezadoRemision)
    {
        idEncabezadoRemision = pIdEncabezadoRemision;
        fechaRemision = new DateTime(1, 1, 1);
        nota = "";
        idCliente = 0;
        idTipoMoneda = 0;
        idUsuario = 0;
        consolidado = false;
        total = 0;
        folio = 0;
        tipoCambio = 0;
        fechaFacturacion = new DateTime(1, 1, 1);
        totalLetra = "";
        idAlmacen = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EncabezadoRemision_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CEncabezadoRemision>(typeof(CEncabezadoRemision), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EncabezadoRemision_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CEncabezadoRemision>(typeof(CEncabezadoRemision), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EncabezadoRemision_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CEncabezadoRemision>(typeof(CEncabezadoRemision), pConexion);
        foreach (CEncabezadoRemision O in Obten.ListaRegistros)
        {
            idEncabezadoRemision = O.IdEncabezadoRemision;
            fechaRemision = O.FechaRemision;
            nota = O.Nota;
            idCliente = O.IdCliente;
            idTipoMoneda = O.IdTipoMoneda;
            idUsuario = O.IdUsuario;
            consolidado = O.Consolidado;
            total = O.Total;
            folio = O.Folio;
            tipoCambio = O.TipoCambio;
            fechaFacturacion = O.FechaFacturacion;
            totalLetra = O.TotalLetra;
            idAlmacen = O.IdAlmacen;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EncabezadoRemision_ConsultarFiltros";
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
        Obten.Llena<CEncabezadoRemision>(typeof(CEncabezadoRemision), pConexion);
        foreach (CEncabezadoRemision O in Obten.ListaRegistros)
        {
            idEncabezadoRemision = O.IdEncabezadoRemision;
            fechaRemision = O.FechaRemision;
            nota = O.Nota;
            idCliente = O.IdCliente;
            idTipoMoneda = O.IdTipoMoneda;
            idUsuario = O.IdUsuario;
            consolidado = O.Consolidado;
            total = O.Total;
            folio = O.Folio;
            tipoCambio = O.TipoCambio;
            fechaFacturacion = O.FechaFacturacion;
            totalLetra = O.TotalLetra;
            idAlmacen = O.IdAlmacen;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_EncabezadoRemision_ConsultarFiltros";
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
        Obten.Llena<CEncabezadoRemision>(typeof(CEncabezadoRemision), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_EncabezadoRemision_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", 0);
        Agregar.StoredProcedure.Parameters["@pIdEncabezadoRemision"].Direction = ParameterDirection.Output;
        if (fechaRemision.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRemision", fechaRemision);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pConsolidado", consolidado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        if (fechaFacturacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFacturacion", fechaFacturacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idEncabezadoRemision = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEncabezadoRemision"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_EncabezadoRemision_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
        if (fechaRemision.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaRemision", fechaRemision);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pConsolidado", consolidado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Editar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        if (fechaFacturacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaFacturacion", fechaFacturacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotalLetra", totalLetra);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_EncabezadoRemision_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}