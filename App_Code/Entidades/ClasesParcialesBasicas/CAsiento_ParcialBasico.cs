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

public partial class CAsiento
{
    //Propiedades Privadas
    private int idAsiento;
    private int idUsuarioAutorizo;

    //Propiedades
    public int IdAsiento
    {
        get { return idAsiento; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idAsiento = value;
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

    //Constructores
    public CAsiento()
    {
        idAsiento = 0;
        idUsuarioAutorizo = 0;
    }

    public CAsiento(int pIdAsiento)
    {
        idAsiento = pIdAsiento;
        idUsuarioAutorizo = 0;
    }

    //Metodos Basicos
    public List<object> LlenaObjetos(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Asiento_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Llena<CAsiento>(typeof(CAsiento), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaObjetos(string[] pColumnas, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Asiento_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.Columnas = new string[pColumnas.Length];
        Obten.Columnas = pColumnas;
        Obten.Llena<CAsiento>(typeof(CAsiento), pConexion);
        return Obten.ListaRegistros;
    }

    public void LlenaObjeto(int pIdentificador, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Asiento_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdAsiento", pIdentificador);
        Obten.Llena<CAsiento>(typeof(CAsiento), pConexion);
        foreach (CAsiento O in Obten.ListaRegistros)
        {
            idAsiento = O.IdAsiento;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
        }
    }

    public void LlenaObjetoFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Asiento_ConsultarFiltros";
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
        Obten.Llena<CAsiento>(typeof(CAsiento), pConexion);
        foreach (CAsiento O in Obten.ListaRegistros)
        {
            idAsiento = O.IdAsiento;
            idUsuarioAutorizo = O.IdUsuarioAutorizo;
        }
    }

    public List<object> LlenaObjetosFiltros(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_Asiento_ConsultarFiltros";
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
        Obten.Llena<CAsiento>(typeof(CAsiento), pConexion);
        return Obten.ListaRegistros;
    }

    public void Agregar(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_Asiento_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAsiento", 0);
        Agregar.StoredProcedure.Parameters["@pIdAsiento"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        Agregar.Insert(pConexion);
        idAsiento = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAsiento"].Value);
    }

    public void Editar(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "spb_Asiento_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdAsiento", idAsiento);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
        Editar.Update(pConexion);
    }

    public void Eliminar(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "spb_Asiento_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdAsiento", idAsiento);
        Eliminar.Delete(pConexion);
    }
}