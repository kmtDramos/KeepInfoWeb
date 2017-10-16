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


public partial class CIVA
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerIVA(JObject pModelo, int pIdIVA, CConexion pConexion)
    {
        CIVA IVA = new CIVA();
        IVA.LlenaObjeto(pIdIVA, pConexion);
        pModelo.Add(new JProperty("IdIVA", IVA.IdIVA));
        pModelo.Add(new JProperty("IVA", IVA.IVA));
        pModelo.Add(new JProperty("DescripcionIVA", IVA.DescripcionIVA));
        pModelo.Add(new JProperty("ClaveCuentaContable", IVA.ClaveCuentaContable));
        pModelo.Add(new JProperty("CuentaContableTrasladado", IVA.CuentaContableTrasladado));
        pModelo.Add(new JProperty("CuentaContableAcreditablePagado", IVA.CCAcreditablePagado));
        pModelo.Add(new JProperty("CuentaContableTrasladadoPagado", IVA.CCTrasladadoPagado));
        return pModelo;
    }

    public static JArray ObtenerJsonIVAActivas(CConexion pConexion)
    {
        JArray JAIVAes = new JArray();
        CIVA IVA = new CIVA();
        foreach (CIVA oIVA in IVA.LlenaObjetos(pConexion))
        {
            JObject JIVA = new JObject();
            JIVA.Add("Valor", oIVA.IdIVA);
            JIVA.Add("Descripcion", oIVA.IVA);
            JAIVAes.Add(JIVA);
        }
        return JAIVAes;
    }

    public string ObtenerJsonIVA(CConexion pConexion)
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
        ObtenObjeto.StoredProcedure.CommandText = "sp_IVA_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.Llena<CIVA>(typeof(CIVA), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public int ExisteClaveCuentaContableEditar(String pClaveCuentaContable, int pIdIVA, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteClaveCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_IVA_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdIVA", Convert.ToInt32(pIdIVA));
        ObtenObjeto.Llena<CIVA>(typeof(CIVA), pConexion);
        foreach (CIVA Sucursal in ObtenObjeto.ListaRegistros)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public List<object> LlenaObjetosFiltrosIVA(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_SucursalIVA_ConsultarFiltros";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CIVA>(typeof(CIVA), pConexion);
        return Obten.ListaRegistros;
    }
}
