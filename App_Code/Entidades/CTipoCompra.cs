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

public partial class CTipoCompra
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Constructores

    //Metodos Especiales
    public static JObject ObtenerTipoCompra(JObject pModelo, int pIdTipoCompra, CConexion pConexion)
    {
        CTipoCompra TipoCompra = new CTipoCompra();
        TipoCompra.LlenaObjeto(pIdTipoCompra, pConexion);
        pModelo.Add(new JProperty("IdTipoCompra", TipoCompra.IdTipoCompra));
        pModelo.Add(new JProperty("TipoCompra", TipoCompra.TipoCompra));
        pModelo.Add(new JProperty("ClaveCuentaContable", TipoCompra.ClaveCuentaContable));
        return pModelo;
    }

    public string ObtenerJsonTipoCompra(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public int ExisteClaveCuentaContable(String pClaveCuentaContable, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteClaveCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_TipoCompra_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.Llena<CTipoCompra>(typeof(CTipoCompra), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public int ExisteClaveCuentaContableEditar(String pClaveCuentaContable, int pIdTipoCompra, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteClaveCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_TipoCompra_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", Convert.ToInt32(pIdTipoCompra));
        ObtenObjeto.Llena<CTipoCompra>(typeof(CTipoCompra), pConexion);
        foreach (CTipoCompra Sucursal in ObtenObjeto.ListaRegistros)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public static JArray ObtenerJsonTiposCompra(int pIdTipoCompra, CConexion pConexion)
    {
        JArray JATiposCompra = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", false);
        CTipoCompra TipoCompra = new CTipoCompra();
        foreach (CTipoCompra oTipoCompra in TipoCompra.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JTipoCompra = new JObject();
            JTipoCompra.Add("Valor", oTipoCompra.IdTipoCompra);
            JTipoCompra.Add("Descripcion", oTipoCompra.TipoCompra);
            if (oTipoCompra.IdTipoCompra == pIdTipoCompra)
            {
                JTipoCompra.Add("Selected", 1);
            }
            JATiposCompra.Add(JTipoCompra);
        }
        return JATiposCompra;
    }
}