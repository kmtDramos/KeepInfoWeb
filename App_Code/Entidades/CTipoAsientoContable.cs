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


public partial class CTipoAsientoContable
{
    //Constructores

    //Metodos Especiales
    public JArray ObtenerJsonTiposAsientosContables(CConexion pConexion)
    {
        CTipoAsientoContable TipoAsientoContable = new CTipoAsientoContable();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", false);

        JArray JATiposAsientosContables = new JArray();
        foreach (CTipoAsientoContable oTipoAsientoContable in TipoAsientoContable.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoAsientoContable = new JObject();
            JTipoAsientoContable.Add("Valor", oTipoAsientoContable.IdTipoAsientoContable);
            JTipoAsientoContable.Add("Descripcion", oTipoAsientoContable.TipoAsientoContable);
            JATiposAsientosContables.Add(JTipoAsientoContable);
        }
        return JATiposAsientosContables;
    }
}