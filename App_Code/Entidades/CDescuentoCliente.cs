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

public partial class CDescuentoCliente
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonDescuentosCliente(int pIdCliente, CConexion pConexion)
    {
        CDescuentoCliente DescuentoCliente = new CDescuentoCliente();
        Dictionary<string, object> ParametrosDescuentoCliente = new Dictionary<string, object>();
        ParametrosDescuentoCliente.Add("Baja", false);
        ParametrosDescuentoCliente.Add("IdCliente", pIdCliente);

        JArray JADescuentosCliente = new JArray();
        foreach (CDescuentoCliente oDescuentoCliente in DescuentoCliente.LlenaObjetosFiltros(ParametrosDescuentoCliente, pConexion))
        {
            JObject JDescuentoCliente = new JObject();
            JDescuentoCliente.Add("Valor", oDescuentoCliente.IdDescuentoCliente);
            JDescuentoCliente.Add("Descripcion", oDescuentoCliente.Descripcion);
            JADescuentosCliente.Add(JDescuentoCliente);
        }
        return JADescuentosCliente;
    }

    public static JArray ObtenerJsonDescuentosCliente(int pIdCliente, int pIdDescuentoCliente, CConexion pConexion)
    {
        CDescuentoCliente DescuentoCliente = new CDescuentoCliente();
        Dictionary<string, object> ParametrosDescuentoCliente = new Dictionary<string, object>();
        ParametrosDescuentoCliente.Add("Baja", false);
        ParametrosDescuentoCliente.Add("IdCliente", pIdCliente);

        JArray JADescuentosCliente = new JArray();
        foreach (CDescuentoCliente oDescuentoCliente in DescuentoCliente.LlenaObjetosFiltros(ParametrosDescuentoCliente, pConexion))
        {
            JObject JDescuentoCliente = new JObject();
            JDescuentoCliente.Add("IdDescuentoCliente", oDescuentoCliente.IdDescuentoCliente);
            JDescuentoCliente.Add("Descripcion", oDescuentoCliente.Descripcion);

            if (oDescuentoCliente.IdDescuentoCliente == pIdDescuentoCliente)
            {
                JDescuentoCliente.Add(new JProperty("Selected", 1));
            }
            else
            {
                JDescuentoCliente.Add(new JProperty("Selected", 0));
            }

            JADescuentosCliente.Add(JDescuentoCliente);
        }
        return JADescuentosCliente;
    }

}