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


public partial class CSubCategoria
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonSubCategoria(CConexion pConexion)
    {
        CSubCategoria SubCategoria = new CSubCategoria();
        JArray JSubCategorias = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CSubCategoria oSubCategoria in SubCategoria.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JSubCategoria = new JObject();
            JSubCategoria.Add("Valor", oSubCategoria.IdSubCategoria);
            JSubCategoria.Add("Descripcion", oSubCategoria.SubCategoria);
            JSubCategorias.Add(JSubCategoria);
        }
        return JSubCategorias;
    }

    public static JArray ObtenerListadoSubCategoria(int pIdCategoria, CConexion pConexion)
    {
        CSubCategoria SubCategoria = new CSubCategoria();
        JArray JSubCategorias = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdCategoria", pIdCategoria);
        Parametros.Add("Baja", 0);
        foreach (CSubCategoria oSubCategoria in SubCategoria.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JSubCategoria = new JObject();
            JSubCategoria.Add("Valor", oSubCategoria.IdSubCategoria);
            JSubCategoria.Add("Descripcion", oSubCategoria.SubCategoria);
            JSubCategorias.Add(JSubCategoria);
        }
        return JSubCategorias;
    }

    public static JArray ObtenerJsonSubCategoria(int pIdSubCategoria, CConexion pConexion)
    {
        CSubCategoria SubCategoria = new CSubCategoria();
        JArray JSubCategorias = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CSubCategoria oSubCategoria in SubCategoria.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JSubCategoria = new JObject();
            JSubCategoria.Add("Valor", oSubCategoria.IdSubCategoria);
            JSubCategoria.Add("Descripcion", oSubCategoria.SubCategoria);
            if (oSubCategoria.IdSubCategoria == pIdSubCategoria)
            {
                JSubCategoria.Add(new JProperty("Selected", 1));
            }
            else
            {
                JSubCategoria.Add(new JProperty("Selected", 0));
            }
            JSubCategorias.Add(JSubCategoria);
        }
        return JSubCategorias;
    }

}