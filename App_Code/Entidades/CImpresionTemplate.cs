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


public partial class CImpresionTemplate
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonImpresionTemplates(CConexion pConexion)
    {
        CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
        JArray JImpresionTemplates = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CImpresionTemplate oImpresionTemplate in ImpresionTemplate.LlenaObjetosFiltros(Parametros, pConexion))
        {
            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(oImpresionTemplate.IdEmpresa, pConexion);

            CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
            ImpresionDocumento.LlenaObjeto(oImpresionTemplate.IdImpresionDocumento, pConexion);

            JObject JImpresionTemplate = new JObject();
            JImpresionTemplate.Add("Valor", oImpresionTemplate.IdImpresionTemplate);
            JImpresionTemplate.Add("Descripcion", Empresa.Empresa + " - " + ImpresionDocumento.ImpresionDocumento);
            JImpresionTemplates.Add(JImpresionTemplate);
        }
        return JImpresionTemplates;
    }

    public static JArray ObtenerJsonImpresionTemplates(int pIdImpresionTemplate, CConexion pConexion)
    {
        CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
        JArray JImpresionTemplates = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CImpresionTemplate oImpresionTemplate in ImpresionTemplate.LlenaObjetosFiltros(Parametros, pConexion))
        {
            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(oImpresionTemplate.IdEmpresa, pConexion);

            CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
            ImpresionDocumento.LlenaObjeto(oImpresionTemplate.IdImpresionDocumento, pConexion);

            JObject JImpresionTemplate = new JObject();
            JImpresionTemplate.Add("Valor", oImpresionTemplate.IdImpresionTemplate);
            JImpresionTemplate.Add("Descripcion", Empresa.Empresa + " - " + ImpresionDocumento.ImpresionDocumento);
            if (oImpresionTemplate.IdImpresionTemplate == pIdImpresionTemplate)
            {
                JImpresionTemplate.Add(new JProperty("Selected", 1));
            }
            else
            {
                JImpresionTemplate.Add(new JProperty("Selected", 0));
            }
            JImpresionTemplates.Add(JImpresionTemplate);
        }
        return JImpresionTemplates;
    }
}
