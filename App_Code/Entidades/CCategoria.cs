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


public partial class CCategoria
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonCategorias(CConexion pConexion)
    {
        CCategoria Categoria = new CCategoria();
        JArray JCategorias = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CCategoria oCategoria in Categoria.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCategoria = new JObject();
            JCategoria.Add("Valor", oCategoria.IdCategoria);
            JCategoria.Add("Descripcion", oCategoria.Categoria);
            JCategorias.Add(JCategoria);
        }
        return JCategorias;
    }

    public static JArray ObtenerJsonCategorias(int pIdCategoria, CConexion pConexion)
    {
        CCategoria Categoria = new CCategoria();
        JArray JCategorias = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CCategoria oCategoria in Categoria.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCategoria = new JObject();
            JCategoria.Add("Valor", oCategoria.IdCategoria);
            JCategoria.Add("Descripcion", oCategoria.Categoria);
            if (oCategoria.IdCategoria == pIdCategoria)
            {
                JCategoria.Add(new JProperty("Selected", 1));
            }
            else
            {
                JCategoria.Add(new JProperty("Selected", 0));
            }
            JCategorias.Add(JCategoria);
        }
        return JCategorias;
    }

    public static JArray ObtenerJsonCategoriasFiltroPorGrupo(int pIdGrupo, CConexion pConexion)
    {
        CCategoria Categoria = new CCategoria();
        JArray JCategorias = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdGrupo", pIdGrupo);
        Parametros.Add("Baja", 0);
        foreach (CCategoria oCategoria in Categoria.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCategoria = new JObject();
            JCategoria.Add("Valor", oCategoria.IdCategoria);
            JCategoria.Add("Descripcion", oCategoria.Categoria);
            JCategorias.Add(JCategoria);
        }
        return JCategorias;
    }
}