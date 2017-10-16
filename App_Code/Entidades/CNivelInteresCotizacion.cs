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


public partial class CNivelInteresCotizacion
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonNivelInteresCotizacion(CConexion Conexion)
    {

        CNivelInteresCotizacion NivelInteresCotizacion = new CNivelInteresCotizacion();
        JArray JNivelesInteresCotizacion = new JArray();

        foreach (CNivelInteresCotizacion oNivelInteresCotizacion in NivelInteresCotizacion.LlenaObjetos(Conexion))
        {
            JObject JNivelInteresCotizacion = new JObject();
            JNivelInteresCotizacion.Add("Valor", oNivelInteresCotizacion.IdNivelInteresCotizacion);
            JNivelInteresCotizacion.Add("Descripcion", oNivelInteresCotizacion.NivelInteresCotizacion);
            JNivelInteresCotizacion.Add("Selected", 0);
            JNivelesInteresCotizacion.Add(JNivelInteresCotizacion);
        }

        return JNivelesInteresCotizacion;
    }

    public static JArray ObtenerJsonNivelInteresCotizacion(int pIdNivelInteresCotizacion, CConexion Conexion)
    {

        CNivelInteresCotizacion NivelInteresCotizacion = new CNivelInteresCotizacion();
        JArray JNivelesInteresCotizacion = new JArray();

        foreach (CNivelInteresCotizacion oNivelInteresCotizacion in NivelInteresCotizacion.LlenaObjetos(Conexion))
        {
            JObject JNivelInteresCotizacion = new JObject();
            JNivelInteresCotizacion.Add("Valor", oNivelInteresCotizacion.IdNivelInteresCotizacion);
            JNivelInteresCotizacion.Add("Descripcion", oNivelInteresCotizacion.NivelInteresCotizacion);
            if (pIdNivelInteresCotizacion == oNivelInteresCotizacion.IdNivelInteresCotizacion)
            {
                JNivelInteresCotizacion.Add("Selected", 1);
            }
            else
            {
                JNivelInteresCotizacion.Add("Selected", 0);
            }
            JNivelesInteresCotizacion.Add(JNivelInteresCotizacion);
        }

        return JNivelesInteresCotizacion;
    }

}
