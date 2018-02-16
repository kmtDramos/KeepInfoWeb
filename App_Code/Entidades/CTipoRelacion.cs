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

public partial class CTipoRelacion
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTipoRelacionActivas(CConexion pConexion)
    {
        JArray JATipoRelacion= new JArray();
        CTipoRelacion TipoRelacion = new CTipoRelacion();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("Baja", 0);
        foreach (CTipoRelacion oTipoRelacion in TipoRelacion.LlenaObjetosFiltros(pParametros, pConexion))
        {
            JObject JTipoRelacion = new JObject();
            JTipoRelacion.Add("Valor", oTipoRelacion.IdTipoRelacion);
            JTipoRelacion.Add("Descripcion", oTipoRelacion.Clave + " - " + oTipoRelacion.Descripcion);
            JATipoRelacion.Add(JTipoRelacion);
        }
        return JATipoRelacion;
    }

}
