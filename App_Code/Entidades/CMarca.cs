using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;


public partial class CMarca
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonMarcas(CConexion pConexion)
    {
        CMarca Marca = new CMarca();
        JArray JMarcas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CMarca oMarca in Marca.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JMarca = new JObject();
            JMarca.Add("Valor", oMarca.IdMarca);
            JMarca.Add("Descripcion", oMarca.Marca);
            JMarcas.Add(JMarca);
        }
        return JMarcas;
    }

    public static JArray ObtenerJsonMarcas(int pIdMarca, CConexion pConexion)
    {
        CMarca Marca = new CMarca();
        JArray JMarcas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CMarca oMarca in Marca.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JMarca = new JObject();
            JMarca.Add("Valor", oMarca.IdMarca);
            JMarca.Add("Descripcion", oMarca.Marca);
            if (oMarca.IdMarca == pIdMarca)
            {
                JMarca.Add(new JProperty("Selected", 1));
            }
            else
            {
                JMarca.Add(new JProperty("Selected", 0));
            }
            JMarcas.Add(JMarca);
        }
        return JMarcas;
    }
}
