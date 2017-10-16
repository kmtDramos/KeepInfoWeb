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

public partial class CBitacoraNotasOportunidad
{
    //Propiedades Privadas
    private int idBitacoraNotasOportunidad;
    private int idUsuario;
    private DateTime fechaCreacion;
    private int idOportunidad;
    private string bitacoraNotaOportunidad;

    //Propiedades
    public int IdBitacoraNotasOportunidad
    {
        get { return idBitacoraNotasOportunidad; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idBitacoraNotasOportunidad = value;
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

    public DateTime FechaCreacion
    {
        get { return fechaCreacion; }
        set { fechaCreacion = value; }
    }

    public int IdOportunidad
    {
        get { return idOportunidad; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idOportunidad = value;
        }
    }

    public string BitacoraNotaOportunidad
    {
        get { return bitacoraNotaOportunidad; }
        set
        {
            bitacoraNotaOportunidad = value;
        }
    }

    //Constructores
    public CBitacoraNotasOportunidad()
    {
        idBitacoraNotasOportunidad = 0;
        idUsuario = 0;
        fechaCreacion = new DateTime(1, 1, 1);
        idOportunidad = 0;
        bitacoraNotaOportunidad = "";
    }

    public CBitacoraNotasOportunidad(int pIdBitacoraNotasOportunidad)
    {
        idBitacoraNotasOportunidad = pIdBitacoraNotasOportunidad;
        idUsuario = 0;
        fechaCreacion = new DateTime(1, 1, 1);
        idOportunidad = 0;
        bitacoraNotaOportunidad = "";
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", pIdentificador);
        Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
        foreach (CBitacoraNotasOportunidad O in Obten.ListaRegistros)
        {
            idBitacoraNotasOportunidad = O.IdBitacoraNotasOportunidad;
            idUsuario = O.IdUsuario;
            fechaCreacion = O.FechaCreacion;
            idOportunidad = O.IdOportunidad;
            bitacoraNotaOportunidad = O.BitacoraNotaOportunidad;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_ConsultarFiltros";
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
        Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
        foreach (CBitacoraNotasOportunidad O in Obten.ListaRegistros)
        {
            idBitacoraNotasOportunidad = O.IdBitacoraNotasOportunidad;
            idUsuario = O.IdUsuario;
            fechaCreacion = O.FechaCreacion;
            idOportunidad = O.IdOportunidad;
            bitacoraNotaOportunidad = O.BitacoraNotaOportunidad;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_ConsultarFiltros";
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
        Obten.Llena<CBitacoraNotasOportunidad>(typeof(CBitacoraNotasOportunidad), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", 0);
        Agregar.StoredProcedure.Parameters["@pIdBitacoraNotasOportunidad"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        if (fechaCreacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBitacoraNotaOportunidad", bitacoraNotaOportunidad);
        Agregar.Insert(pConexion);
        idBitacoraNotasOportunidad = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdBitacoraNotasOportunidad"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", idBitacoraNotasOportunidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        if (fechaCreacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
        }
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBitacoraNotaOportunidad", bitacoraNotaOportunidad);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_BitacoraNotasOportunidad_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdBitacoraNotasOportunidad", idBitacoraNotasOportunidad);
        Eliminar.Delete(pConexion);
    }
}