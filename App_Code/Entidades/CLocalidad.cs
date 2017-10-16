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


public partial class CLocalidad
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonLocalidades(int pIdMunicipio, CConexion pConexion)
    {
        CLocalidad Localidad = new CLocalidad();
        JArray JLocalidades = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdMunicipio", pIdMunicipio);
        foreach (CLocalidad oLocalidad in Localidad.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JLocalidad = new JObject();
            JLocalidad.Add("Valor", oLocalidad.IdLocalidad);
            JLocalidad.Add("Descripcion", oLocalidad.Localidad);
            JLocalidades.Add(JLocalidad);
        }
        return JLocalidades;
    }

    public static JArray ObtenerJsonLocalidades(int pIdMunicipio, int pIdLocalidad, CConexion pConexion)
    {
        CLocalidad Localidad = new CLocalidad();
        JArray JLocalidades = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdMunicipio", pIdMunicipio);
        foreach (CLocalidad oLocalidad in Localidad.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JLocalidad = new JObject();
            JLocalidad.Add("Valor", oLocalidad.IdLocalidad);
            JLocalidad.Add("Descripcion", oLocalidad.Localidad);
            if (oLocalidad.IdLocalidad == pIdLocalidad)
            {
                JLocalidad.Add(new JProperty("Selected", 1));
            }
            else
            {
                JLocalidad.Add(new JProperty("Selected", 0));
            }
            JLocalidades.Add(JLocalidad);
        }
        return JLocalidades;
    }

}
