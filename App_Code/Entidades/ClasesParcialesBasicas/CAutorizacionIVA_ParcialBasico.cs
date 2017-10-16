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

public partial class CAutorizacionIVA
{
    //Propiedades Privadas
    private int idAutorizacionIVA;
    private int idUsuarioAutorizo;
    private int idUsuarioSolicito;
    private decimal iVA;
    private DateTime fecha;
    private string claveAutorizacion;
    private DateTime fechaVigencia;
    private bool disponible;
    private int idDocumento;
    private string tipoDocumento;
    private bool baja;

    //Propiedades
    public int IdAutorizacionIVA
    {
        get { return idAutorizacionIVA; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAutorizacionIVA = value;
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

    public decimal IVA
    {
        get { return iVA; }
        set
        {
            if (value < 0)
            {
                return;
            }
            iVA = value;
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
            if (value.Length == 0)
            {
                return;
            }
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
            if (value.Length == 0)
            {
                return;
            }
            tipoDocumento = value;
        }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CAutorizacionIVA()
    {
        idAutorizacionIVA = 0;
        idUsuarioAutorizo = 0;
        idUsuarioSolicito = 0;
        iVA = 0;
        fecha = new DateTime(1, 1, 1);
        claveAutorizacion = "";
        fechaVigencia = new DateTime(1, 1, 1);
        disponible = false;
        idDocumento = 0;
        tipoDocumento = "";
        baja = false;
    }

    public CAutorizacionIVA(int pIdAutorizacionIVA)
    {
        idAutorizacionIVA = pIdAutorizacionIVA;
        idUsuarioAutorizo = 0;
        idUsuarioSolicito = 0;
        iVA = 0;
        fecha = new DateTime(1, 1, 1);
        claveAutorizacion = "";
        fechaVigencia = new DateTime(1, 1, 1);
        disponible = false;
        idDocumento = 0;
        tipoDocumento = "";
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionIVA_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAutorizacionIVA>(typeof(CAutorizacionIVA), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionIVA_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAutorizacionIVA>(typeof(CAutorizacionIVA), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionIVA_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionIVA", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAutorizacionIVA>(typeof(CAutorizacionIVA), pConexion);
        foreach (CAutorizacionIVA O in Obten.ListaRegistros)
        {
            idAutorizacionIVA = O.IdAutorizacionIVA;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
            idUsuarioSolicito = O.IdUsuarioSolicito;
            iVA = O.IVA;
            fecha = O.Fecha;
            claveAutorizacion = O.ClaveAutorizacion;
            fechaVigencia = O.FechaVigencia;
            disponible = O.Disponible;
            idDocumento = O.IdDocumento;
            tipoDocumento = O.TipoDocumento;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionIVA_ConsultarFiltros";
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
        Obten.Llena<CAutorizacionIVA>(typeof(CAutorizacionIVA), pConexion);
        foreach (CAutorizacionIVA O in Obten.ListaRegistros)
        {
            idAutorizacionIVA = O.IdAutorizacionIVA;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
            idUsuarioSolicito = O.IdUsuarioSolicito;
            iVA = O.IVA;
            fecha = O.Fecha;
            claveAutorizacion = O.ClaveAutorizacion;
            fechaVigencia = O.FechaVigencia;
            disponible = O.Disponible;
            idDocumento = O.IdDocumento;
            tipoDocumento = O.TipoDocumento;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AutorizacionIVA_ConsultarFiltros";
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
        Obten.Llena<CAutorizacionIVA>(typeof(CAutorizacionIVA), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_AutorizacionIVA_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionIVA", 0);
        Agregar.StoredProcedure.Parameters["@pIdAutorizacionIVA"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", idUsuarioSolicito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
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
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idAutorizacionIVA = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAutorizacionIVA"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_AutorizacionIVA_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionIVA", idAutorizacionIVA);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", idUsuarioSolicito);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
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
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_AutorizacionIVA_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAutorizacionIVA", idAutorizacionIVA);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}