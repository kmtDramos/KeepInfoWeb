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


public partial class CCampana
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonCampana(bool pGenerico, CConexion pConexion)
    {
        CCampana Campana = new CCampana();
        JArray JCampanas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CCampana oCampana in Campana.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JCampana = new JObject();
            if (pGenerico)
            {
                JCampana.Add("Valor", oCampana.IdCampana);
                JCampana.Add("Descripcion", oCampana.Campana);
            }
            JCampanas.Add(JCampana);
        }
        return JCampanas;
    }

    public static JArray ObtenerJsonCampana(int pIdCampana, CConexion pConexion)
    {
        CCampana Campana = new CCampana();
        JArray JCampanas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);

        foreach (CCampana oCampana in Campana.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCampana = new JObject();
            JCampana.Add("Valor", oCampana.IdCampana);
            JCampana.Add("Descripcion", oCampana.Campana);

            if (oCampana.IdCampana == pIdCampana)
            {
                JCampana.Add(new JProperty("Selected", 1));
            }
            else
            {
                JCampana.Add(new JProperty("Selected", 0));
            }

            JCampanas.Add(JCampana);
        }
        return JCampanas;
    }

}