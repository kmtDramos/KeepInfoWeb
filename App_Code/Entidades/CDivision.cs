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


public partial class CDivision
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerDivision(JObject pModelo, int pIdDivision, CConexion pConexion)
    {
        CDivision Division = new CDivision();
        Division.LlenaObjeto(pIdDivision, pConexion);
        pModelo.Add(new JProperty("IdDivision", Division.IdDivision));
        pModelo.Add(new JProperty("Division", Division.Division));
        pModelo.Add(new JProperty("ClaveCuentaContable", Division.ClaveCuentaContable));
        pModelo.Add(new JProperty("EsVenta", Division.EsVenta));
        return pModelo;
    }

    public static string ObtenerNombreDivision(int pIdDivision, CConexion pConexion)
    {
        CDivision Division = new CDivision();
        Division.LlenaObjeto(pIdDivision, pConexion);
        return Division.Division;
    }

    public static JArray ObtenerJsonDivisionesActivas(CConexion pConexion)
    {
        JArray JADivisiones = new JArray();
        CDivision Division = new CDivision();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("Baja", 0);
        foreach (CDivision oDivision in Division.LlenaObjetosFiltros(pParametros,pConexion))
        {
            JObject JDivision = new JObject();
            JDivision.Add("Valor", oDivision.IdDivision);
            JDivision.Add("Descripcion", oDivision.Division);
			JDivision.Add("LimiteDescuento", oDivision.LimiteDescuento);
			JDivision.Add("LimiteMargen", oDivision.LimiteMargen);
            JADivisiones.Add(JDivision);
        }
        return JADivisiones;
    }

    public string ObtenerJsonDivision(CConexion pConexion)
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
        ObtenObjeto.StoredProcedure.CommandText = "sp_Division_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.Llena<CDivision>(typeof(CDivision), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public int ExisteClaveCuentaContableEditar(String pClaveCuentaContable, int pIdDivision, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteClaveCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Division_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdDivision", Convert.ToInt32(pIdDivision));
        ObtenObjeto.Llena<CDivision>(typeof(CDivision), pConexion);
        foreach (CDivision Sucursal in ObtenObjeto.ListaRegistros)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public List<object> LlenaObjetosFiltrosDivisiones(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_SucursalDivision_ConsultarFiltros";
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
        Obten.Llena<CDivision>(typeof(CDivision), pConexion);
        return Obten.ListaRegistros;
    }

    public static JArray ObtenerJsonDivisionesActivas(int pIdDivision, CConexion pConexion)
    {
        JArray JADivisiones = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", false);
        CDivision Division = new CDivision();
        foreach (CDivision oDivision in Division.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JDivision = new JObject();
            JDivision.Add("Valor", oDivision.IdDivision);
            JDivision.Add("Descripcion", oDivision.Division);
            if (oDivision.IdDivision == pIdDivision)
            {
                JDivision.Add("Selected", 1);
            }
            JADivisiones.Add(JDivision);
        }
        return JADivisiones;
    }
}
