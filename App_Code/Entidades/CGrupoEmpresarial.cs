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


public partial class CGrupoEmpresarial
{
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerGrupoEmpresarial(JObject pModelo, int pIdGrupoEmpresarial, CConexion pConexion)
    {
        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        GrupoEmpresarial.LlenaObjeto(pIdGrupoEmpresarial, pConexion);
        pModelo.Add(new JProperty("IdGrupoEmpresarial", GrupoEmpresarial.IdGrupoEmpresarial));
        pModelo.Add(new JProperty("GrupoEmpresarial", GrupoEmpresarial.GrupoEmpresarial));
        return pModelo;
    }

    public static JArray ObtenerJsonGrupoEmpresariales(CConexion pConexion)
    {
        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        JArray JGrupoEmpresarials = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CGrupoEmpresarial oGrupoEmpresarial in GrupoEmpresarial.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JGrupoEmpresarial = new JObject();
            JGrupoEmpresarial.Add("IdGrupoEmpresarial", oGrupoEmpresarial.IdGrupoEmpresarial);
            JGrupoEmpresarial.Add("GrupoEmpresarial", oGrupoEmpresarial.GrupoEmpresarial);
            JGrupoEmpresarials.Add(JGrupoEmpresarial);
        }
        return JGrupoEmpresarials;
    }

    public static JArray ObtenerJsonGrupoEmpresariales(int pIdGrupoEmpresarial, CConexion pConexion)
    {
        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        JArray JGrupoEmpresarials = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CGrupoEmpresarial oGrupoEmpresarial in GrupoEmpresarial.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JGrupoEmpresarial = new JObject();
            JGrupoEmpresarial.Add("IdGrupoEmpresarial", oGrupoEmpresarial.IdGrupoEmpresarial);
            JGrupoEmpresarial.Add("GrupoEmpresarial", oGrupoEmpresarial.GrupoEmpresarial);
            if (oGrupoEmpresarial.IdGrupoEmpresarial == pIdGrupoEmpresarial)
            {
                JGrupoEmpresarial.Add(new JProperty("Selected", 1));
            }
            else
            {
                JGrupoEmpresarial.Add(new JProperty("Selected", 0));
            }
            JGrupoEmpresarials.Add(JGrupoEmpresarial);
        }
        return JGrupoEmpresarials;
    }
}
