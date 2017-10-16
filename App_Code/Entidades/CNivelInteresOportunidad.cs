using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;


public partial class CNivelInteresOportunidad
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonNivelInteresOportunidad(CConexion Conexion)
    {

        CNivelInteresOportunidad NivelInteresOportunidad = new CNivelInteresOportunidad();
        JArray JNivelesInteresOportunidad = new JArray();

        foreach (CNivelInteresOportunidad oNivelInteresOportunidad in NivelInteresOportunidad.LlenaObjetos(Conexion))
        {
            JObject JNivelInteresOportunidad = new JObject();
            JNivelInteresOportunidad.Add("Valor", oNivelInteresOportunidad.IdNivelInteresOportunidad);
            JNivelInteresOportunidad.Add("Descripcion", oNivelInteresOportunidad.NivelInteresOportunidad);
            JNivelInteresOportunidad.Add("Selected", 0);
            JNivelesInteresOportunidad.Add(JNivelInteresOportunidad);
        }

        return JNivelesInteresOportunidad;
    }

    public static JArray ObtenerJsonNivelInteresOportunidad(int pIdNivelInteresOportunidad, CConexion Conexion)
    {

        CNivelInteresOportunidad NivelInteresOportunidad = new CNivelInteresOportunidad();
        JArray JNivelesInteresOportunidad = new JArray();

        foreach (CNivelInteresOportunidad oNivelInteresOportunidad in NivelInteresOportunidad.LlenaObjetos(Conexion))
        {
            JObject JNivelInteresOportunidad = new JObject();
            JNivelInteresOportunidad.Add("Valor", oNivelInteresOportunidad.IdNivelInteresOportunidad);
            JNivelInteresOportunidad.Add("Descripcion", oNivelInteresOportunidad.NivelInteresOportunidad);
            if (oNivelInteresOportunidad.IdNivelInteresOportunidad == pIdNivelInteresOportunidad)
            {
                JNivelInteresOportunidad.Add("Selected", 1);
            }
            else
            {
                JNivelInteresOportunidad.Add("Selected", 0);
            }
            JNivelesInteresOportunidad.Add(JNivelInteresOportunidad);
        }

        return JNivelesInteresOportunidad;
    }

}