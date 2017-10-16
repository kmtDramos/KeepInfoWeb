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

public partial class CArchivoOportunidad
{
    //Propiedades Privadas
    private int idArchivoOportunidad;
    private string archivoOportunidad;
    private int idOportunidad;
    private int idUsuarioCreacion;
    private DateTime fechaCreacion;

    //Propiedades
    public int IdArchivoOportunidad
    {
        get { return idArchivoOportunidad; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idArchivoOportunidad = value;
        }
    }

    public string ArchivoOportunidad
    {
        get { return archivoOportunidad; }
        set
        {
            archivoOportunidad = value;
        }
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

    public int IdUsuarioCreacion
    {
        get { return idUsuarioCreacion; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idUsuarioCreacion = value;
        }
    }

    public DateTime FechaCreacion
    {
        get { return fechaCreacion; }
        set { fechaCreacion = value; }
    }

    //Constructores
    public CArchivoOportunidad()
    {
        idArchivoOportunidad = 0;
        archivoOportunidad = "";
        idOportunidad = 0;
        idUsuarioCreacion = 0;
        fechaCreacion = new DateTime(1, 1, 1);
    }

    public CArchivoOportunidad(int pIdArchivoOportunidad)
    {
        idArchivoOportunidad = pIdArchivoOportunidad;
        archivoOportunidad = "";
        idOportunidad = 0;
        idUsuarioCreacion = 0;
        fechaCreacion = new DateTime(1, 1, 1);
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ArchivoOportunidad_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CArchivoOportunidad>(typeof(CArchivoOportunidad), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ArchivoOportunidad_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CArchivoOportunidad>(typeof(CArchivoOportunidad), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ArchivoOportunidad_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdArchivoOportunidad", pIdentificador);
        Obten.Llena<CArchivoOportunidad>(typeof(CArchivoOportunidad), pConexion);
        foreach (CArchivoOportunidad O in Obten.ListaRegistros)
        {
            idArchivoOportunidad = O.IdArchivoOportunidad;
            archivoOportunidad = O.ArchivoOportunidad;
            idOportunidad = O.IdOportunidad;
            idUsuarioCreacion = O.IdUsuarioCreacion;
            fechaCreacion = O.FechaCreacion;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ArchivoOportunidad_ConsultarFiltros";
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
        Obten.Llena<CArchivoOportunidad>(typeof(CArchivoOportunidad), pConexion);
        foreach (CArchivoOportunidad O in Obten.ListaRegistros)
        {
            idArchivoOportunidad = O.IdArchivoOportunidad;
            archivoOportunidad = O.ArchivoOportunidad;
            idOportunidad = O.IdOportunidad;
            idUsuarioCreacion = O.IdUsuarioCreacion;
            fechaCreacion = O.FechaCreacion;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_ArchivoOportunidad_ConsultarFiltros";
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
        Obten.Llena<CArchivoOportunidad>(typeof(CArchivoOportunidad), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_ArchivoOportunidad_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdArchivoOportunidad", 0);
        Agregar.StoredProcedure.Parameters["@pIdArchivoOportunidad"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pArchivoOportunidad", archivoOportunidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreacion", idUsuarioCreacion);
        if (fechaCreacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
        }
        Agregar.Insert(pConexion);
        idArchivoOportunidad = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdArchivoOportunidad"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_ArchivoOportunidad_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdArchivoOportunidad", idArchivoOportunidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pArchivoOportunidad", archivoOportunidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCreacion", idUsuarioCreacion);
        if (fechaCreacion.Year != 1)
        {
            Editar.StoredProcedure.Parameters.AddWithValue("@pFechaCreacion", fechaCreacion);
        }
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_ArchivoOportunidad_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdArchivoOportunidad", idArchivoOportunidad);
        Eliminar.Delete(pConexion);
    }
}