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


public partial class CEstatusProyecto
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonEstatusProyectos(CConexion pConexion)
    {
        CEstatusProyecto Estatus = new CEstatusProyecto();
        JArray JAEstatus = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CEstatusProyecto oEstatus in Estatus.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JEstatus = new JObject();
            JEstatus.Add("Valor", oEstatus.IdEstatusProyecto);
            JEstatus.Add("Descripcion", oEstatus.Estatus);
            JAEstatus.Add(JEstatus);
        }
        return JAEstatus;
    }

    public static JArray ObtenerJsonEstatusProyectos(int pIdEstatusProyecto, CConexion pConexion)
    {
        CEstatusProyecto Estatus = new CEstatusProyecto();
        JArray JAEstatus = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CEstatusProyecto oEstatus in Estatus.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JEstatus = new JObject();
            JEstatus.Add("Valor", oEstatus.IdEstatusProyecto);
            JEstatus.Add("Descripcion", oEstatus.Estatus);
            if (oEstatus.IdEstatusProyecto == pIdEstatusProyecto)
            {
                JEstatus.Add(new JProperty("Selected", 1));
            }
            else
            {
                JEstatus.Add(new JProperty("Selected", 0));
            }
            JAEstatus.Add(JEstatus);
        }
        return JAEstatus;
    }
}
