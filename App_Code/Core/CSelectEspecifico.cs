using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

public class CSelectEspecifico
{
    //Atributos
    private string consulta;
    public SqlCommand StoredProcedure = new SqlCommand();
    public SqlDataReader Registros;
	public string Error = "";

    //Propiedades
    public string Consulta
    {
        get { return consulta; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            consulta = value;
        }
    }

    public void Llena(CConexion pConexion)
    {
        try
        {
            StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
            StoredProcedure.CommandType = CommandType.StoredProcedure;
            Registros = StoredProcedure.ExecuteReader();
        }
        catch (Exception ex) { Error = ex.StackTrace; }
    }

    public void CerrarConsulta()
    {
        Registros.Close();
        StoredProcedure.Dispose();
    }
}