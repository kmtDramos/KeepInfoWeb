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

public partial class CImpresionDocumento
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Constructores

    //Metodos Especiales
    public static JObject ObtenerImpresionDocumento(JObject pModelo, int pIdImpresionDocumento, CConexion pConexion)
    {
        CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
        ImpresionDocumento.LlenaObjeto(pIdImpresionDocumento, pConexion);
        pModelo.Add(new JProperty("IdImpresionDocumento", ImpresionDocumento.IdImpresionDocumento));
        pModelo.Add(new JProperty("ImpresionDocumento", ImpresionDocumento.ImpresionDocumento));
        pModelo.Add(new JProperty("Procedimiento", ImpresionDocumento.Procedimiento));
        return pModelo;
    }
    public string ObtenerJsonImpresionDocumento(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public int ExisteImpresionDocumento(string pImpresionDocumento, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteImpresionDocumento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_ImpresionDocumento_ConsultaImpresionDocumento";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pImpresionDocumento", Convert.ToString(pImpresionDocumento));
        ObtenObjeto.Llena<CImpresionDocumento>(typeof(CImpresionDocumento), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteImpresionDocumento = 1;
        }
        return ExisteImpresionDocumento;
    }

    public int ExisteImpresionDocumentoEditar(string pImpresionDocumento, int pIdImpresionDocumento, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteImpresionDocumento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_ImpresionDocumento_ConsultaImpresionDocumento";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pImpresionDocumento", Convert.ToString(pImpresionDocumento));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdImpresionDocumento", Convert.ToInt32(pIdImpresionDocumento));
        ObtenObjeto.Llena<CImpresionDocumento>(typeof(CImpresionDocumento), pConexion);
        foreach (CImpresionDocumento Imp in ObtenObjeto.ListaRegistros)
        {
            ExisteImpresionDocumento = 1;
        }
        return ExisteImpresionDocumento;
    }

    public static JArray ObtenerJsonImpresionDocumentos(CConexion pConexion)
    {
        CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
        JArray JImpresionDocumentos = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CImpresionDocumento oImpresionDocumento in ImpresionDocumento.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JImpresionDocumento = new JObject();
            JImpresionDocumento.Add("Valor", oImpresionDocumento.IdImpresionDocumento);
            JImpresionDocumento.Add("Descripcion", oImpresionDocumento.ImpresionDocumento);
            JImpresionDocumentos.Add(JImpresionDocumento);
        }
        return JImpresionDocumentos;
    }

    public static JArray ObtenerJsonImpresionDocumentos(int pIdImpresionDocumento, CConexion pConexion)
    {
        CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
        JArray JImpresionDocumentos = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CImpresionDocumento oImpresionDocumento in ImpresionDocumento.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JImpresionDocumento = new JObject();
            JImpresionDocumento.Add("Valor", oImpresionDocumento.IdImpresionDocumento);
            JImpresionDocumento.Add("Descripcion", oImpresionDocumento.ImpresionDocumento);
            if (oImpresionDocumento.IdImpresionDocumento == pIdImpresionDocumento)
            {
                JImpresionDocumento.Add(new JProperty("Selected", 1));
            }
            else
            {
                JImpresionDocumento.Add(new JProperty("Selected", 0));
            }
            JImpresionDocumentos.Add(JImpresionDocumento);
        }
        return JImpresionDocumentos;
    }
}