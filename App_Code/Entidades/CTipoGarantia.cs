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

public partial class CTipoGarantia
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTipoGarantia(CConexion pConexion)
    {
        CTipoGarantia TipoGarantia = new CTipoGarantia();
        JArray JTipoGarantias = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoGarantia oTipoGarantia in TipoGarantia.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoGarantia = new JObject();
            JTipoGarantia.Add("IdTipoGarantia", oTipoGarantia.IdTipoGarantia);
            JTipoGarantia.Add("TipoGarantia", oTipoGarantia.TipoGarantia);
            JTipoGarantias.Add(JTipoGarantia);
        }
        return JTipoGarantias;
    }

    public static JArray ObtenerJsonTipoGarantia(bool pGenerico, CConexion pConexion)
    {
        CTipoGarantia TipoGarantia = new CTipoGarantia();
        JArray JTipoGarantias = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoGarantia oTipoGarantia in TipoGarantia.LlenaObjetosFiltros(Parametros, pConexion))
        {
            if (pGenerico)
            {
                JObject JTipoGarantia = new JObject();
                JTipoGarantia.Add("Valor", oTipoGarantia.IdTipoGarantia);
                JTipoGarantia.Add("Descripcion", oTipoGarantia.TipoGarantia);
                JTipoGarantias.Add(JTipoGarantia);
            }
        }
        return JTipoGarantias;
    }

    public static JArray ObtenerJsonTipoGarantia(int pIdTipoGarantia, CConexion pConexion)
    {
        CTipoGarantia TipoGarantia = new CTipoGarantia();
        JArray JTipoGarantias = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoGarantia oTipoGarantia in TipoGarantia.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoGarantia = new JObject();
            JTipoGarantia.Add("IdTipoGarantia", oTipoGarantia.IdTipoGarantia);
            JTipoGarantia.Add("TipoGarantia", oTipoGarantia.TipoGarantia);
            if (oTipoGarantia.IdTipoGarantia == pIdTipoGarantia)
            {
                JTipoGarantia.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoGarantia.Add(new JProperty("Selected", 0));
            }
            JTipoGarantias.Add(JTipoGarantia);
        }
        return JTipoGarantias;
    }

}