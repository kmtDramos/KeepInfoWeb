using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class CTipoMoneda
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTiposMoneda(CConexion pConexion)
    {
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        JArray JTiposMoneda = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoMoneda oTipoMoneda in TipoMoneda.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoMoneda = new JObject();
            JTipoMoneda.Add("Valor", oTipoMoneda.IdTipoMoneda);
            JTipoMoneda.Add("Descripcion", oTipoMoneda.TipoMoneda);
            if (oTipoMoneda.IdTipoMoneda == 1)
            {
                JTipoMoneda.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoMoneda.Add(new JProperty("Selected", 0));
            }
            JTiposMoneda.Add(JTipoMoneda);
        }
        return JTiposMoneda;
    }

    public static JArray ObtenerJsonTiposMoneda(int pIdTipoMoneda, CConexion pConexion)
    {
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        JArray JTiposMoneda = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);

        foreach (CTipoMoneda oTipoMoneda in TipoMoneda.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoMoneda = new JObject();
            JTipoMoneda.Add("Valor", oTipoMoneda.IdTipoMoneda);
            JTipoMoneda.Add("Descripcion", oTipoMoneda.TipoMoneda);
            if (oTipoMoneda.IdTipoMoneda == pIdTipoMoneda)
            {
                JTipoMoneda.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoMoneda.Add(new JProperty("Selected", 0));
            }
            JTiposMoneda.Add(JTipoMoneda);
        }
        return JTiposMoneda;
    }
}