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

public partial class CUnidadCompraVenta
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonUnidadesCompraVenta(CConexion pConexion)
    {
        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        JArray JUnidadesCompraVenta = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CUnidadCompraVenta oUnidadCompraVenta in UnidadCompraVenta.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JUnidadCompraVenta = new JObject();
            JUnidadCompraVenta.Add("Valor", oUnidadCompraVenta.IdUnidadCompraVenta);
            JUnidadCompraVenta.Add("Descripcion", oUnidadCompraVenta.UnidadCompraVenta);
            JUnidadesCompraVenta.Add(JUnidadCompraVenta);
        }
        return JUnidadesCompraVenta;
    }

    public static JArray ObtenerJsonUnidadesCompraVenta(int pIdUnidadCompraVenta, CConexion pConexion)
    {
        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        JArray JUnidadesCompraVenta = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CUnidadCompraVenta oUnidadCompraVenta in UnidadCompraVenta.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JUnidadCompraVenta = new JObject();
            JUnidadCompraVenta.Add("Valor", oUnidadCompraVenta.IdUnidadCompraVenta);
            JUnidadCompraVenta.Add("Descripcion", oUnidadCompraVenta.UnidadCompraVenta);
            if (oUnidadCompraVenta.IdUnidadCompraVenta == pIdUnidadCompraVenta)
            {
                JUnidadCompraVenta.Add(new JProperty("Selected", 1));
            }
            else
            {
                JUnidadCompraVenta.Add(new JProperty("Selected", 0));
            }
            JUnidadesCompraVenta.Add(JUnidadCompraVenta);
        }
        return JUnidadesCompraVenta;
    }
}