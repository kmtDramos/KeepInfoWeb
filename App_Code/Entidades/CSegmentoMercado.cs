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


public partial class CSegmentoMercado
{
    //Constructores

    public static JArray ObtenerJsonSegmentoMercado(CConexion pConexion)
    {
        CSegmentoMercado SegmentoMercado = new CSegmentoMercado();
        JArray JSegmentoMercados = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CSegmentoMercado oSegmentoMercado in SegmentoMercado.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JSegmentoMercado = new JObject();
            JSegmentoMercado.Add("IdSegmentoMercado", oSegmentoMercado.IdSegmentoMercado);
            JSegmentoMercado.Add("SegmentoMercado", oSegmentoMercado.SegmentoMercado);
            JSegmentoMercados.Add(JSegmentoMercado);
        }
        return JSegmentoMercados;
    }

    public static JArray ObtenerJsonSegmentoMercado(int pIdSegmentoMercado, CConexion pConexion)
    {
        CSegmentoMercado SegmentoMercado = new CSegmentoMercado();
        JArray JSegmentoMercados = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CSegmentoMercado oSegmentoMercado in SegmentoMercado.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JSegmentoMercado = new JObject();
            JSegmentoMercado.Add("IdSegmentoMercado", oSegmentoMercado.IdSegmentoMercado);
            JSegmentoMercado.Add("SegmentoMercado", oSegmentoMercado.SegmentoMercado);
            if (oSegmentoMercado.IdSegmentoMercado == pIdSegmentoMercado)
            {
                JSegmentoMercado.Add(new JProperty("Selected", 1));
            }
            else
            {
                JSegmentoMercado.Add(new JProperty("Selected", 0));
            }
            JSegmentoMercados.Add(JSegmentoMercado);
        }
        return JSegmentoMercados;
    }

}
