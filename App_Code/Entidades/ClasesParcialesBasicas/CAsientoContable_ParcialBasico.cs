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

public partial class CAsientoContable
{
    //Propiedades Privadas
    private int idAsientoContable;
    private int idTipoAsientoContable;
    private int idDocumento;
    private int idClaseGenerador;
    private int idUsuarioAlta;
    private DateTime fechaAlta;
    private int idUsuarioAutorizo;
    private DateTime fechaAutorizo;
    private decimal total;
    private bool baja;

    //Propiedades
    public int IdAsientoContable
    {
        get { return idAsientoContable; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAsientoContable = value;
        }
    }

    public int IdTipoAsientoContable
    {
        get { return idTipoAsientoContable; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoAsientoContable = value;
        }
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

    public int IdClaseGenerador
    {
        get { return idClaseGenerador; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idClaseGenerador = value;
        }
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

    public DateTime FechaAlta
    {
        get { return fechaAlta; }
        set { fechaAlta = value; }
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

    public DateTime FechaAutorizo
    {
        get { return fechaAutorizo; }
        set { fechaAutorizo = value; }
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

    public bool Baja
    {
        get { return baja; }
        set { baja = value; }
    }

    //Constructores
    public CAsientoContable()
    {
        idAsientoContable = 0;
        idTipoAsientoContable = 0;
        idDocumento = 0;
        idClaseGenerador = 0;
        idUsuarioAlta = 0;
        fechaAlta = new DateTime(1, 1, 1);
        idUsuarioAutorizo = 0;
        fechaAutorizo = new DateTime(1, 1, 1);
        total = 0;
        baja = false;
    }

    public CAsientoContable(int pIdAsientoContable)
    {
        idAsientoContable = pIdAsientoContable;
        idTipoAsientoContable = 0;
        idDocumento = 0;
        idClaseGenerador = 0;
        idUsuarioAlta = 0;
        fechaAlta = new DateTime(1, 1, 1);
        idUsuarioAutorizo = 0;
        fechaAutorizo = new DateTime(1, 1, 1);
        total = 0;
        baja = false;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContable_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAsientoContable>(typeof(CAsientoContable), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContable_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAsientoContable>(typeof(CAsientoContable), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContable_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContable", pIdentificador);
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena<CAsientoContable>(typeof(CAsientoContable), pConexion);
        foreach (CAsientoContable O in Obten.ListaRegistros)
        {
            idAsientoContable = O.IdAsientoContable;
            idTipoAsientoContable = O.IdTipoAsientoContable;
            idDocumento = O.IdDocumento;
            idClaseGenerador = O.IdClaseGenerador;
            idUsuarioAlta = O.IdUsuarioAlta;
            fechaAlta = O.FechaAlta;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
            fechaAutorizo = O.FechaAutorizo;
            total = O.Total;
            baja = O.Baja;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContable_ConsultarFiltros";
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
        Obten.Llena<CAsientoContable>(typeof(CAsientoContable), pConexion);
        foreach (CAsientoContable O in Obten.ListaRegistros)
        {
            idAsientoContable = O.IdAsientoContable;
            idTipoAsientoContable = O.IdTipoAsientoContable;
            idDocumento = O.IdDocumento;
            idClaseGenerador = O.IdClaseGenerador;
            idUsuarioAlta = O.IdUsuarioAlta;
            fechaAlta = O.FechaAlta;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
            fechaAutorizo = O.FechaAutorizo;
            total = O.Total;
            baja = O.Baja;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_AsientoContable_ConsultarFiltros";
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
        Obten.Llena<CAsientoContable>(typeof(CAsientoContable), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_AsientoContable_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContable", 0);
        Agregar.StoredProcedure.Parameters["@pIdAsientoContable"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoAsientoContable", idTipoAsientoContable);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDocumento", idDocumento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        if (fechaAutorizo.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAutorizo", fechaAutorizo);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idAsientoContable = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAsientoContable"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_AsientoContable_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContable", idAsientoContable);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdTipoAsientoContable", idTipoAsientoContable);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdDocumento", idDocumento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
        if (fechaAlta.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        if (fechaAutorizo.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaAutorizo", fechaAutorizo);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_AsientoContable_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContable", idAsientoContable);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}