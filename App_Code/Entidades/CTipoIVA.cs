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

public partial class CTipoIVA
{
    //Constructores

    //Metodos Especiales

    public static JArray ObtenerJsonTiposIVA(CConexion pConexion)
    {
        CTipoIVA TipoIVA = new CTipoIVA();
        JArray JTiposIVA = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoIVA oTipoIVA in TipoIVA.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoIVA = new JObject();
            JTipoIVA.Add("IdTipoIVA", oTipoIVA.IdTipoIVA);
            JTipoIVA.Add("TipoIVA", oTipoIVA.TipoIVA);
            if (oTipoIVA.IdTipoIVA == 1)
            {
                JTipoIVA.Add(new JProperty("Selected", "1"));
            }
            else
            {
                JTipoIVA.Add(new JProperty("Selected", "0"));
            }

            JTiposIVA.Add(JTipoIVA);
        }
        return JTiposIVA;
    }

    public static JArray ObtenerJsonTiposIVA(int pIdTipoIVA, CConexion pConexion)
    {
        CTipoIVA TipoIVA = new CTipoIVA();
        JArray JTiposIVA = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoIVA oTipoIVA in TipoIVA.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoIVA = new JObject();
            JTipoIVA.Add("IdTipoIVA", oTipoIVA.IdTipoIVA);
            JTipoIVA.Add("TipoIVA", oTipoIVA.TipoIVA);
            if (oTipoIVA.IdTipoIVA == pIdTipoIVA)
            {
                JTipoIVA.Add(new JProperty("Selected", "1"));
            }
            else
            {
                JTipoIVA.Add(new JProperty("Selected", "0"));
            }
            JTiposIVA.Add(JTipoIVA);
        }
        return JTiposIVA;
    }
}
