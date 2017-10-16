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

public partial class CGestionCobranza
{
    //Propiedades Privadas
    private int idGestionCobranza;
    private int idFactura;
    private int idCliente;
    private DateTime fechaProgramada;
    private int idUsuario;
    private DateTime fechaAlta;
    private int idTipoGestion;
    private bool baja;

    //Propiedades
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

    public int IdFactura
    {
        get { return idFactura; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idFactura = value;
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

    public DateTime FechaProgramada
    {
        get { return fechaProgramada; }
        set { fechaProgramada = value; }
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

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
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

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CGestionCobranza()
    {
        idGestionCobranza = 0;
        idFactura = 0;
        idCliente = 0;
        fechaProgramada = new DateTime(1, 1, 1);
        idUsuario = 0;
        fechaAlta = new DateTime(1, 1, 1);
        idTipoGestion = 0;
        baja = false;
    }

    public CGestionCobranza(int pIdGestionCobranza)
    {
        idGestionCobranza = pIdGestionCobranza;
        idFactura = 0;
        idCliente = 0;
        fechaProgramada = new DateTime(1, 1, 1);
        idUsuario = 0;
        fechaAlta = new DateTime(1, 1, 1);
        idTipoGestion = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranza_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CGestionCobranza>(typeof(CGestionCobranza), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranza_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CGestionCobranza>(typeof(CGestionCobranza), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranza_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranza", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CGestionCobranza>(typeof(CGestionCobranza), pConexion);
        foreach (CGestionCobranza O in Obten.ListaRegistros)
        {
            idGestionCobranza = O.IdGestionCobranza;
            idFactura = O.IdFactura;
            idCliente = O.IdCliente;
            fechaProgramada = O.FechaProgramada;
            idUsuario = O.IdUsuario;
            fechaAlta = O.FechaAlta;
            idTipoGestion = O.IdTipoGestion;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranza_ConsultarFiltros";
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
        Obten.Llena<CGestionCobranza>(typeof(CGestionCobranza), pConexion);
        foreach (CGestionCobranza O in Obten.ListaRegistros)
        {
            idGestionCobranza = O.IdGestionCobranza;
            idFactura = O.IdFactura;
            idCliente = O.IdCliente;
            fechaProgramada = O.FechaProgramada;
            idUsuario = O.IdUsuario;
            fechaAlta = O.FechaAlta;
            idTipoGestion = O.IdTipoGestion;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranza_ConsultarFiltros";
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
        Obten.Llena<CGestionCobranza>(typeof(CGestionCobranza), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_GestionCobranza_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranza", 0);
        Agregar.StoredProcedure.Parameters["@pIdGestionCobranza"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFactura", idFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        if (fechaProgramada.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaProgramada", fechaProgramada);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGestion", idTipoGestion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idGestionCobranza = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdGestionCobranza"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_GestionCobranza_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranza", idGestionCobranza);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdFactura", idFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        if (fechaProgramada.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaProgramada", fechaProgramada);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoGestion", idTipoGestion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_GestionCobranza_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdGestionCobranza", idGestionCobranza);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}