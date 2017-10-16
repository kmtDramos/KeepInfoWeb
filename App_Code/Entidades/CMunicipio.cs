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


public partial class CMunicipio
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonMunicipios(int pIdEstado, CConexion pConexion)
    {
        CMunicipio Municipio = new CMunicipio();
        JArray JMunicipios = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdEstado", pIdEstado);
        foreach (CMunicipio oMunicipio in Municipio.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JMunicipio = new JObject();
            JMunicipio.Add("Valor", oMunicipio.IdMunicipio);
            JMunicipio.Add("Descripcion", oMunicipio.Municipio);
            JMunicipios.Add(JMunicipio);
        }
        return JMunicipios;
    }

    public static JArray ObtenerJsonMunicipios(int pIdEstado, int pIdMunicipio, CConexion pConexion)
    {
        CMunicipio Municipio = new CMunicipio();
        JArray JMunicipios = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdEstado", pIdEstado);
        foreach (CMunicipio oMunicipio in Municipio.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JMunicipio = new JObject();
            JMunicipio.Add("Valor", oMunicipio.IdMunicipio);
            JMunicipio.Add("Descripcion", oMunicipio.Municipio);
            if (oMunicipio.IdMunicipio == pIdMunicipio)
            {
                JMunicipio.Add(new JProperty("Selected", 1));
            }
            else
            {
                JMunicipio.Add(new JProperty("Selected", 0));
            }
            JMunicipios.Add(JMunicipio);
        }
        return JMunicipios;
    }
}