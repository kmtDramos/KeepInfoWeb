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


public partial class CAutorizacionTipoCambio
{
    //Propiedades Privadas
    private int idAutorizacionTipoCambio;
    private int idUsuarioAutorizo;
    private int idUsuarioSolicito;
    private int idTipoMonedaOrigen;
    private int idTipoMonedaDestino;
    private decimal tipoCambio;
    private DateTime fecha;
    private string claveAutorizacion;
    private DateTime fechaVigencia;
    private bool disponible;
    private int idDocumento;
    private string tipoDocumento;
    private int idTipoDocumento;
    private bool baja;

    //Propiedades
    public int IdAutorizacionTipoCambio
    {
        get { return idAutorizacionTipoCambio; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAutorizacionTipoCambio = value;
        }
    }

    public int IdUsuarioAutorizo
    {
        get { return idUsuarioAutorizo; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioAutorizo = value;
        }
    }

    public int IdUsuarioSolicito
    {
        get { return idUsuarioSolicito; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioSolicito = value;
        }
    }

    public int IdTipoMonedaOrigen
    {
        get { return idTipoMonedaOrigen; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoMonedaOrigen = value;
        }
    }

    public int IdTipoMonedaDestino
    {
        get { return idTipoMonedaDestino; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoMonedaDestino = value;
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

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public string ClaveAutorizacion
    {
        get { return claveAutorizacion; }
        set
        {
            claveAutorizacion = value;
        }
    }

    public DateTime FechaVigencia
    {
        get { return fechaVigencia; }
        set { fechaVigencia = value; }
    }

    public bool Disponible
    {
        get { return disponible; }
        set { disponible = value; }
    }

    public int IdDocumento
    {
        get { return idDocumento; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idDocumento = value;
        }
    }

    public string TipoDocumento
    {
        get { return tipoDocumento; }
        set
        {
            tipoDocumento = value;
        }
    }

    public int IdTipoDocumento
    {
        get { return idTipoDocumento; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoDocumento = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CAutorizacionTipoCambio()
    {
        idAutorizacionTipoCambio = 0;
        idUsuarioAutorizo = 0;
        idUsuarioSolicito = 0;
        idTipoMonedaOrigen = 0;
        idTipoMonedaDestino = 0;
        tipoCambio = 0;
        fecha = new DateTime(1, 1, 1);
        claveAutorizacion = "";
        fechaVigencia = new DateTime(1, 1, 1);
        disponible = false;
        idDocumento = 0;
        tipoDocumento = "";
        idTipoDocumento = 0;
        baja = false;
    }

    public CAutorizacionTipoCambio(int pIdAutorizacionTipoCambio)
    {
        idAutorizacionTipoCambio = pIdAutorizacionTipoCambio;
        idUsuarioAutorizo = 0;
        idUsuarioSolicito = 0;
        idTipoMonedaOrigen = 0;
        idTipoMonedaDestino = 0;
        tipoCambio = 0;
        fecha = new DateTime(1, 1, 1);
        claveAutorizacion = "";
        fechaVigencia = new DateTime(1, 1, 1);
        disponible = false;
        idDocumento = 0;
        tipoDocumento = "";
        idTipoDocumento = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAutorizacionTipoCambio>(typeof(CAutorizacionTipoCambio), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAutorizacionTipoCambio>(typeof(CAutorizacionTipoCambio), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionTipoCambio", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAutorizacionTipoCambio>(typeof(CAutorizacionTipoCambio), pConexion);
        foreach (CAutorizacionTipoCambio O in Obten.ListaRegistros)
        {
            idAutorizacionTipoCambio = O.IdAutorizacionTipoCambio;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
            idUsuarioSolicito = O.IdUsuarioSolicito;
            idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
            idTipoMonedaDestino = O.IdTipoMonedaDestino;
            tipoCambio = O.TipoCambio;
            fecha = O.Fecha;
            claveAutorizacion = O.ClaveAutorizacion;
            fechaVigencia = O.FechaVigencia;
            disponible = O.Disponible;
            idDocumento = O.IdDocumento;
            tipoDocumento = O.TipoDocumento;
            idTipoDocumento = O.IdTipoDocumento;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_ConsultarFiltros";
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
        Obten.Llena<CAutorizacionTipoCambio>(typeof(CAutorizacionTipoCambio), pConexion);
        foreach (CAutorizacionTipoCambio O in Obten.ListaRegistros)
        {
            idAutorizacionTipoCambio = O.IdAutorizacionTipoCambio;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
            idUsuarioSolicito = O.IdUsuarioSolicito;
            idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
            idTipoMonedaDestino = O.IdTipoMonedaDestino;
            tipoCambio = O.TipoCambio;
            fecha = O.Fecha;
            claveAutorizacion = O.ClaveAutorizacion;
            fechaVigencia = O.FechaVigencia;
            disponible = O.Disponible;
            idDocumento = O.IdDocumento;
            tipoDocumento = O.TipoDocumento;
            idTipoDocumento = O.IdTipoDocumento;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_ConsultarFiltros";
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
        Obten.Llena<CAutorizacionTipoCambio>(typeof(CAutorizacionTipoCambio), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionTipoCambio", 0);
        Agregar.StoredProcedure.Parameters["@pIdAutorizacionTipoCambio"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", idUsuarioSolicito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        if (fecha.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", claveAutorizacion);
        if (fechaVigencia.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaVigencia", fechaVigencia);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDisponible", disponible);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDocumento", idDocumento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoDocumento", tipoDocumento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDocumento", idTipoDocumento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idAutorizacionTipoCambio = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAutorizacionTipoCambio"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionTipoCambio", idAutorizacionTipoCambio);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", idUsuarioSolicito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", idTipoMonedaOrigen);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", idTipoMonedaDestino);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        if (fecha.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", claveAutorizacion);
        if (fechaVigencia.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaVigencia", fechaVigencia);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pDisponible", disponible);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDocumento", idDocumento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTipoDocumento", tipoDocumento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoDocumento", idTipoDocumento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_AutorizacionTipoCambio_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionTipoCambio", idAutorizacionTipoCambio);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}