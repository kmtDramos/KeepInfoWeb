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


public partial class CBanco
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonBanco(CConexion pConexion)
    {
        CBanco Banco = new CBanco();
        JArray JBancos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CBanco oBanco in Banco.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JBanco = new JObject();
            JBanco.Add("IdBanco", oBanco.IdBanco);
            JBanco.Add("Banco", oBanco.Banco);
            JBancos.Add(JBanco);
        }
        return JBancos;
    }

    public static JArray ObtenerJsonBanco(int pIdBanco, CConexion pConexion)
    {
        CBanco Banco = new CBanco();
        JArray JBancos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CBanco oBanco in Banco.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JBanco = new JObject();
            JBanco.Add("IdBanco", oBanco.IdBanco);
            JBanco.Add("Banco", oBanco.Banco);
            if (oBanco.IdBanco == pIdBanco)
            {
                JBanco.Add(new JProperty("Selected", 1));
            }
            else
            {
                JBanco.Add(new JProperty("Selected", 0));
            }
            JBancos.Add(JBanco);
        }
        return JBancos;
    }

    public static JArray ObtenerJsonBanco(bool pGenerico, CConexion pConexion)
    {
        CBanco Banco = new CBanco();
        JArray JBancos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CBanco oBanco in Banco.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JBanco = new JObject();
            if (pGenerico)
            {
                JBanco.Add("Valor", oBanco.IdBanco);
                JBanco.Add("Descripcion", oBanco.Banco);
            }
            JBancos.Add(JBanco);
        }
        return JBancos;
    }

}