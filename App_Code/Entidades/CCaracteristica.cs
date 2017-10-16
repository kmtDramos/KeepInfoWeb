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


public partial class CCaracteristica
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonCaracteristicas(CConexion pConexion)
    {
        CCaracteristica Caracteristica = new CCaracteristica();
        JArray JCaracteristicas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CCaracteristica oCaracteristica in Caracteristica.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCaracteristica = new JObject();
            JCaracteristica.Add("Valor", oCaracteristica.IdCaracteristica);
            JCaracteristica.Add("Descripcion", oCaracteristica.Caracteristica);
            JCaracteristicas.Add(JCaracteristica);
        }
        return JCaracteristicas;
    }

    public static JArray ObtenerJsonCaracteristicas(int pIdCaracteristica, CConexion pConexion)
    {
        CCaracteristica Caracteristica = new CCaracteristica();
        JArray JCaracteristicas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CCaracteristica oCaracteristica in Caracteristica.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCaracteristica = new JObject();
            JCaracteristica.Add("Valor", oCaracteristica.IdCaracteristica);
            JCaracteristica.Add("Descripcion", oCaracteristica.Caracteristica);
            if (oCaracteristica.IdCaracteristica == pIdCaracteristica)
            {
                JCaracteristica.Add(new JProperty("Selected", 1));
            }
            else
            {
                JCaracteristica.Add(new JProperty("Selected", 0));
            }
            JCaracteristicas.Add(JCaracteristica);
        }
        return JCaracteristicas;
    }
}