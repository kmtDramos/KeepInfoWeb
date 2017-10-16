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

public partial class CTiempoEntrega
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTiempoEntrega(CConexion pConexion)
    {
        CTiempoEntrega TiempoEntrega = new CTiempoEntrega();
        JArray JTiempoEntregas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        foreach (CTiempoEntrega oTiempoEntrega in TiempoEntrega.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTiempoEntrega = new JObject();
            JTiempoEntrega.Add("Valor", oTiempoEntrega.IdTiempoEntrega);
            JTiempoEntrega.Add("Descripcion", oTiempoEntrega.TiempoEntrega);

            if (oTiempoEntrega.IdTiempoEntrega == 1)
            {
                JTiempoEntrega.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTiempoEntrega.Add(new JProperty("Selected", 0));
            }

            JTiempoEntregas.Add(JTiempoEntrega);
        }
        return JTiempoEntregas;
    }

    public static JArray ObtenerJsonTiempoEntrega(int pIdTiempoEntrega, CConexion pConexion)
    {
        CTiempoEntrega TiempoEntrega = new CTiempoEntrega();
        JArray JTiemposEntrega = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTiempoEntrega oTiempoEntrega in TiempoEntrega.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTiempoEntrega = new JObject();
            JTiempoEntrega.Add("Valor", oTiempoEntrega.IdTiempoEntrega);
            JTiempoEntrega.Add("Descripcion", oTiempoEntrega.TiempoEntrega);
            if (oTiempoEntrega.IdTiempoEntrega == pIdTiempoEntrega)
            {
                JTiempoEntrega.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTiempoEntrega.Add(new JProperty("Selected", 0));
            }
            JTiemposEntrega.Add(JTiempoEntrega);
        }
        return JTiemposEntrega;
    }
}
