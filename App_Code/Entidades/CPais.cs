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


public partial class CPais
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonPaises(CConexion pConexion)
    {
        CPais Pais = new CPais();
        JArray JPaises = new JArray();
        foreach (CPais oPais in Pais.LlenaObjetos(pConexion))
        {
            JObject JPais = new JObject();
            JPais.Add("Valor", oPais.IdPais);
            JPais.Add("Descripcion", oPais.Pais);
            JPaises.Add(JPais);
        }
        return JPaises;
    }

    public static JArray ObtenerJsonPaises(int pIdPais, CConexion pConexion)
    {
        CPais Pais = new CPais();
        JArray JPaises = new JArray();
        foreach (CPais oPais in Pais.LlenaObjetos(pConexion))
        {
            JObject JPais = new JObject();
            JPais.Add("Valor", oPais.IdPais);
            JPais.Add("Descripcion", oPais.Pais);
            if (oPais.IdPais == pIdPais)
            {
                JPais.Add(new JProperty("Selected", 1));
            }
            else
            {
                JPais.Add(new JProperty("Selected", 0));
            }
            JPaises.Add(JPais);
        }
        return JPaises;
    }

}