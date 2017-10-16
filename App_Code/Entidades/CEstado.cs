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


public partial class CEstado
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonEstados(int pIdPais, CConexion pConexion)
    {
        CEstado Estado = new CEstado();
        JArray JEstados = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdPais", pIdPais);
        foreach (CEstado oEstado in Estado.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JEstado = new JObject();
            JEstado.Add("Valor", oEstado.IdEstado);
            JEstado.Add("Descripcion", oEstado.Estado);
            JEstados.Add(JEstado);
        }
        return JEstados;
    }

    public static JArray ObtenerJsonEstados(int pIdPais, int pIdEstado, CConexion pConexion)
    {
        CEstado Estado = new CEstado();
        JArray JEstados = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdPais", pIdPais);
        foreach (CEstado oEstado in Estado.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JEstado = new JObject();
            JEstado.Add("Valor", oEstado.IdEstado);
            JEstado.Add("Descripcion", oEstado.Estado);
            if (oEstado.IdEstado == pIdEstado)
            {
                JEstado.Add(new JProperty("Selected", 1));
            }
            else
            {
                JEstado.Add(new JProperty("Selected", 0));
            }
            JEstados.Add(JEstado);
        }
        return JEstados;
    }
}
