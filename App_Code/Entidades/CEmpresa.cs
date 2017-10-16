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


public partial class CEmpresa
{
    //Constructores

    //Metodos Especiales
    public List<object> ObtenerEmpresasAsignadas(int pIdUsuario, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Empresa_Consultar_ObtenerEmpresasAsignadas";
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", @pIdUsuario);
        Obten.Llena<CEmpresa>(typeof(CEmpresa), pConexion);
        return Obten.ListaRegistros;
    }

    public static JArray ObtenerJsonEmpresas(CConexion pConexion)
    {
        CEmpresa Empresa = new CEmpresa();
        JArray JEmpresas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CEmpresa oEmpresa in Empresa.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JEmpresa = new JObject();
            JEmpresa.Add("Valor", oEmpresa.IdEmpresa);
            JEmpresa.Add("Descripcion", oEmpresa.Empresa);
            JEmpresas.Add(JEmpresa);
        }
        return JEmpresas;
    }

    public static JArray ObtenerJsonEmpresas(int pIdEmpresa, CConexion pConexion)
    {
        CEmpresa Empresa = new CEmpresa();
        JArray JEmpresas = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CEmpresa oEmpresa in Empresa.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JEmpresa = new JObject();
            JEmpresa.Add("Valor", oEmpresa.IdEmpresa);
            JEmpresa.Add("Descripcion", oEmpresa.Empresa);
            if (oEmpresa.IdEmpresa == pIdEmpresa)
            {
                JEmpresa.Add(new JProperty("Selected", 1));
            }
            else
            {
                JEmpresa.Add(new JProperty("Selected", 0));
            }
            JEmpresas.Add(JEmpresa);
        }
        return JEmpresas;
    }
}