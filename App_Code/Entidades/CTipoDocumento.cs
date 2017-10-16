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

public partial class CTipoDocumento
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTipoDocumento(CConexion pConexion)
    {
        CTipoDocumento TipoDocumento = new CTipoDocumento();
        JArray JTipoDocumentos = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoDocumento oTipoDocumento in TipoDocumento.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoDocumento = new JObject();
            JTipoDocumento.Add("Valor", oTipoDocumento.IdTipoDocumento);
            JTipoDocumento.Add("Descripcion", oTipoDocumento.TipoDocumento);
            JTipoDocumentos.Add(JTipoDocumento);
        }
        return JTipoDocumentos;
    }

    public static JArray ObtenerJsonTipoDocumento(int pIdTipoDocumento, CConexion pConexion)
    {
        CTipoDocumento TipoDocumento = new CTipoDocumento();
        JArray JTipoDocumentos = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);

        foreach (CTipoDocumento oTipoDocumento in TipoDocumento.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoDocumento = new JObject();
            JTipoDocumento.Add("Valor", oTipoDocumento.IdTipoDocumento);
            JTipoDocumento.Add("Descripcion", oTipoDocumento.TipoDocumento);
            if (oTipoDocumento.IdTipoDocumento == pIdTipoDocumento)
            {
                JTipoDocumento.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoDocumento.Add(new JProperty("Selected", 0));
            }
            JTipoDocumentos.Add(JTipoDocumento);
        }
        return JTipoDocumentos;
    }
}