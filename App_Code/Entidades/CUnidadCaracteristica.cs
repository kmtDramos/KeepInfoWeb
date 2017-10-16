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

public partial class CUnidadCaracteristica
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonUnidadesCaracteristica(CConexion pConexion)
    {
        CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
        JArray JUnidadesCaracteristica = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CUnidadCaracteristica oUnidadCaracteristica in UnidadCaracteristica.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JUnidadCaracteristica = new JObject();
            JUnidadCaracteristica.Add("Valor", oUnidadCaracteristica.IdUnidadCaracteristica);
            JUnidadCaracteristica.Add("Descripcion", oUnidadCaracteristica.UnidadCaracteristica);
            JUnidadesCaracteristica.Add(JUnidadCaracteristica);
        }
        return JUnidadesCaracteristica;
    }

    public static JArray ObtenerJsonUnidadesCaracteristica(int pIdUnidadCaracteristica, CConexion pConexion)
    {
        CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
        JArray JUnidadesCaracteristica = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CUnidadCaracteristica oUnidadCaracteristica in UnidadCaracteristica.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JUnidadCaracteristica = new JObject();
            JUnidadCaracteristica.Add("Valor", oUnidadCaracteristica.IdUnidadCaracteristica);
            JUnidadCaracteristica.Add("Descripcion", oUnidadCaracteristica.UnidadCaracteristica);
            if (oUnidadCaracteristica.IdUnidadCaracteristica == pIdUnidadCaracteristica)
            {
                JUnidadCaracteristica.Add(new JProperty("Selected", 1));
            }
            else
            {
                JUnidadCaracteristica.Add(new JProperty("Selected", 0));
            }
            JUnidadesCaracteristica.Add(JUnidadCaracteristica);
        }
        return JUnidadesCaracteristica;
    }
}
