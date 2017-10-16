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


public partial class CTipoVenta
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonTiposVenta(CConexion pConexion)
    {
        CTipoVenta TipoVenta = new CTipoVenta();
        JArray JTiposVenta = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoVenta oTipoVenta in TipoVenta.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoVenta = new JObject();
            JTipoVenta.Add("Valor", oTipoVenta.IdTipoVenta);
            JTipoVenta.Add("Descripcion", oTipoVenta.TipoVenta);
            JTiposVenta.Add(JTipoVenta);
        }
        return JTiposVenta;
    }

    public static JArray ObtenerJsonTiposVenta(int pIdTipoVenta, CConexion pConexion)
    {
        CTipoVenta TipoVenta = new CTipoVenta();
        JArray JTiposVenta = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CTipoVenta oTipoVenta in TipoVenta.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoVenta = new JObject();
            JTipoVenta.Add("Valor", oTipoVenta.IdTipoVenta);
            JTipoVenta.Add("Descripcion", oTipoVenta.TipoVenta);
            if (oTipoVenta.IdTipoVenta == pIdTipoVenta)
            {
                JTipoVenta.Add(new JProperty("Selected", "1"));
            }
            else
            {
                JTipoVenta.Add(new JProperty("Selected", "0"));
            }
            JTiposVenta.Add(JTipoVenta);
        }
        return JTiposVenta;
    }
}