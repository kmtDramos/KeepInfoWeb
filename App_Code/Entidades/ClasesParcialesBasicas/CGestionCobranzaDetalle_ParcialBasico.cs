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

public partial class CGestionCobranzaDetalle
{
    //Propiedades Privadas
    private int idGestionCobranzaDetalle;
    private int idGestionCobranza;
    private string comentario;
    private DateTime fechaProgramada;
    private int idTipoGestion;
    private DateTime fechaAlta;
    private int idUsuarioAlta;
    private bool gestionado;
    private bool baja;

    //Propiedades
    public int IdGestionCobranzaDetalle
    {
        get { return idGestionCobranzaDetalle; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idGestionCobranzaDetalle = value;
        }
    }

    public int IdGestionCobranza
    {
        get { return idGestionCobranza; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idGestionCobranza = value;
        }
    }

    public string Comentario
    {
        get { return comentario; }
        set
        {
            comentario = value;
        }
    }

    public DateTime FechaProgramada
    {
        get { return fechaProgramada; }
        set { fechaProgramada = value; }
    }

    public int IdTipoGestion
    {
        get { return idTipoGestion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoGestion = value;
        }
    }

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
    }

    public int IdUsuarioAlta
    {
        get { return idUsuarioAlta; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioAlta = value;
        }
    }

    public bool Gestionado
    {
        get { return gestionado; }
        set { gestionado = value; }
    }

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CGestionCobranzaDetalle()
    {
        idGestionCobranzaDetalle = 0;
        idGestionCobranza = 0;
        comentario = "";
        fechaProgramada = new DateTime(1, 1, 1);
        idTipoGestion = 0;
        fechaAlta = new DateTime(1, 1, 1);
        idUsuarioAlta = 0;
        gestionado = false;
        baja = false;
    }

    public CGestionCobranzaDetalle(int pIdGestionCobranzaDetalle)
    {
        idGestionCobranzaDetalle = pIdGestionCobranzaDetalle;
        idGestionCobranza = 0;
        comentario = "";
        fechaProgramada = new DateTime(1, 1, 1);
        idTipoGestion = 0;
        fechaAlta = new DateTime(1, 1, 1);
        idUsuarioAlta = 0;
        gestionado = false;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CGestionCobranzaDetalle>(typeof(CGestionCobranzaDetalle), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CGestionCobranzaDetalle>(typeof(CGestionCobranzaDetalle), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranzaDetalle", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CGestionCobranzaDetalle>(typeof(CGestionCobranzaDetalle), pConexion);
        foreach (CGestionCobranzaDetalle O in Obten.ListaRegistros)
        {
            idGestionCobranzaDetalle = O.IdGestionCobranzaDetalle;
            idGestionCobranza = O.IdGestionCobranza;
            comentario = O.Comentario;
            fechaProgramada = O.FechaProgramada;
            idTipoGestion = O.IdTipoGestion;
            fechaAlta = O.FechaAlta;
            idUsuarioAlta = O.IdUsuarioAlta;
            gestionado = O.Gestionado;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_ConsultarFiltros";
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
        Obten.Llena<CGestionCobranzaDetalle>(typeof(CGestionCobranzaDetalle), pConexion);
        foreach (CGestionCobranzaDetalle O in Obten.ListaRegistros)
        {
            idGestionCobranzaDetalle = O.IdGestionCobranzaDetalle;
            idGestionCobranza = O.IdGestionCobranza;
            comentario = O.Comentario;
            fechaProgramada = O.FechaProgramada;
            idTipoGestion = O.IdTipoGestion;
            fechaAlta = O.FechaAlta;
            idUsuarioAlta = O.IdUsuarioAlta;
            gestionado = O.Gestionado;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_ConsultarFiltros";
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
        Obten.Llena<CGestionCobranzaDetalle>(typeof(CGestionCobranzaDetalle), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranzaDetalle", 0);
        Agregar.StoredProcedure.Parameters["@pIdGestionCobranzaDetalle"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranza", idGestionCobranza);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pComentario", comentario);
        if (fechaProgramada.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaProgramada", fechaProgramada);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGestion", idTipoGestion);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pGestionado", gestionado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idGestionCobranzaDetalle = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdGestionCobranzaDetalle"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranzaDetalle", idGestionCobranzaDetalle);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranza", idGestionCobranza);
        Editar.StoredProcedure.Parameters.AddWithValue("@pComentario", comentario);
        if (fechaProgramada.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaProgramada", fechaProgramada);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGestion", idTipoGestion);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        Editar.StoredProcedure.Parameters.AddWithValue("@pGestionado", gestionado);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranzaDetalle", idGestionCobranzaDetalle);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}