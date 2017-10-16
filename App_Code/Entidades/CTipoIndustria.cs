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

public partial class CTipoIndustria
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTipoIndustria(CConexion pConexion)
    {
        CTipoIndustria TipoIndustria = new CTipoIndustria();
        JArray JTipoIndustrias = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoIndustria oTipoIndustria in TipoIndustria.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoIndustria = new JObject();
            JTipoIndustria.Add("IdTipoIndustria", oTipoIndustria.IdTipoIndustria);
            JTipoIndustria.Add("TipoIndustria", oTipoIndustria.TipoIndustria);
            JTipoIndustrias.Add(JTipoIndustria);
        }
        return JTipoIndustrias;
    }

    public static JArray ObtenerJsonTipoIndustria(int pIdTipoIndustria, CConexion pConexion)
    {

        CTipoIndustria TipoIndustria = new CTipoIndustria();
        JArray JTipoIndustrias = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoIndustria oTipoIndustria in TipoIndustria.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoIndustria = new JObject();
            JTipoIndustria.Add("IdTipoIndustria", oTipoIndustria.IdTipoIndustria);
            JTipoIndustria.Add("TipoIndustria", oTipoIndustria.TipoIndustria);
            if (oTipoIndustria.IdTipoIndustria == pIdTipoIndustria)
            {
                JTipoIndustria.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoIndustria.Add(new JProperty("Selected", 0));
            }
            JTipoIndustrias.Add(JTipoIndustria);
        }
        return JTipoIndustrias;
    }

}
