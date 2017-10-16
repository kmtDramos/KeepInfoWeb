using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

public class CConsultaAccion
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();
    private string[] columnas;

    //Propiedades
    public string[] Columnas
    {
        get { return columnas; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            columnas = value;
        }
    }

    //Metodos
    public void Insert(CConexion pConexion)
    {
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.ExecuteNonQuery();
    }

    public void Update(CConexion pConexion)
    {
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.ExecuteNonQuery();
    }

    public void Delete(CConexion pConexion)
    {
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.ExecuteNonQuery();
    }
}