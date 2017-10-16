using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

public class CSelect
{
    //Atributos
    private string consulta;
    private string[] columnas;
    private List<object> listaRegistros = new List<object>();
    public SqlCommand StoredProcedure = new SqlCommand();

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

    public List<object> ListaRegistros
    {
        get { return listaRegistros; }
    }

    //Metodos
    public void Llena<T>(Type pTipoObjeto, CConexion pConexion)
    {
        SqlDataReader drSelect;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        drSelect = StoredProcedure.ExecuteReader();

        int index = 0;
        // Corre el lector
        while (drSelect.Read())
        {
            // Crea una instacia del tipo de objeto
            T item = (T)Activator.CreateInstance(pTipoObjeto);
            // Obtiene todas las propiedades del tipo
            PropertyInfo[] propiedades = ((Type)item.GetType()).GetProperties();
            if (columnas == null)
            {
                foreach (PropertyInfo p in propiedades)
                {
                    // Obtiene el index de la propiedad
                    index = EncuentraPropiedadIndexPorNombreColumna(p.Name, propiedades);
                    // Asigna el valor a propiedad
                    try
                    {
                        if (propiedades[index].PropertyType.Name == "Single")
                        {
                            propiedades[index].SetValue(item,Convert.ToSingle(drSelect[p.Name]), null);
                        }
                        else
                        {
                            propiedades[index].SetValue(item, drSelect[p.Name], null);
                        }
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
            }
            else
            {
                for (int j = 0; j < columnas.Length; j++)
                {
                    // Obtiene el index de la propiedad
                    index = EncuentraPropiedadIndexPorNombreColumna(columnas[j], propiedades);
                    // Asigna el valor a propiedad
                    try
                    {
                        propiedades[index].SetValue(item,drSelect[columnas[j]], null);
                    }
                    catch (Exception e)
                    {
                        //No hace nada
                    }
                }
            }
            // Agrega el elemento a la lista
            listaRegistros.Add(item);
        }
        drSelect.Close();
        StoredProcedure.Dispose();
    }

    private static int EncuentraPropiedadIndexPorNombreColumna(string nombreColumna, PropertyInfo[] prop)
    {
        int index = -1;
        for (int i = 0; i < prop.Length; i++)
        {
            if (prop[i].Name.Equals(nombreColumna))
            {
                index = i;
                break;
            }
        }
        if (index == -1) throw new ArgumentOutOfRangeException("Columna no encontrada");
        return index;
    }
}