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

public partial class CTipoCliente
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTipoCliente(bool pGenerico, CConexion pConexion)
    {
        CTipoCliente TipoCliente = new CTipoCliente();
        JArray JTiposCliente = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoCliente oTipoCliente in TipoCliente.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoCliente = new JObject();
            if (pGenerico)
            {
                JTipoCliente.Add("Valor", oTipoCliente.IdTipoCliente);
                JTipoCliente.Add("Descripcion", oTipoCliente.TipoCliente);
            }
            JTiposCliente.Add(JTipoCliente);
        }
        return JTiposCliente;
    }
}