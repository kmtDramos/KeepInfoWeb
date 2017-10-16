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


public partial class CGrupo
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonGrupos(CConexion pConexion)
    {
        CGrupo Grupo = new CGrupo();
        JArray JGrupos = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdEmpresa", Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]));
        Parametros.Add("Baja", 0);
        foreach (CGrupo oGrupo in Grupo.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JGrupo = new JObject();
            JGrupo.Add("Valor", oGrupo.IdGrupo);
            JGrupo.Add("Descripcion", oGrupo.Grupo);
            JGrupos.Add(JGrupo);
        }
        return JGrupos;
    }

    public static JArray ObtenerJsonGrupos(int pIdGrupo, CConexion pConexion)
    {
        CGrupo Grupo = new CGrupo();
        JArray JGrupos = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CGrupo oGrupo in Grupo.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JGrupo = new JObject();
            JGrupo.Add("Valor", oGrupo.IdGrupo);
            JGrupo.Add("Descripcion", oGrupo.Grupo);
            if (oGrupo.IdGrupo == pIdGrupo)
            {
                JGrupo.Add(new JProperty("Selected", 1));
            }
            else
            {
                JGrupo.Add(new JProperty("Selected", 0));
            }
            JGrupos.Add(JGrupo);
        }
        return JGrupos;
    }
}