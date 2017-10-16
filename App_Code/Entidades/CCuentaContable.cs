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


public partial class CCuentaContable
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();
    //Constructores
    //Metodos Especiales

    public static JObject ObtenerCuentaContable(JObject pModelo, int pIdCuentaContable, CConexion pConexion)
    {
        CCuentaContable CuentaContable = new CCuentaContable();
        CuentaContable.LlenaObjeto(pIdCuentaContable, pConexion);
        pModelo.Add(new JProperty("IdCuentaContable", CuentaContable.IdCuentaContable));
        pModelo.Add(new JProperty("CuentaContable", CuentaContable.CuentaContable));

        string[] segmentosCuentaContables;
        segmentosCuentaContables = CuentaContable.CuentaContable.Split('-');

        int contador = 0;
        JArray JASegmentos = new JArray();
        foreach (string segmento in segmentosCuentaContables)
        {
            contador++;
            JObject JSegmento = new JObject();
            JSegmento.Add("Id", contador);
            JSegmento.Add("Segmento", segmento);
            JASegmentos.Add(JSegmento);
        }

        pModelo.Add(new JProperty("Segmentos", JASegmentos));
        pModelo.Add(new JProperty("Descripcion", CuentaContable.Descripcion));
        pModelo.Add(new JProperty("IdDivision", CuentaContable.IdDivision));
        pModelo.Add(new JProperty("IdTipoCompra", CuentaContable.IdTipoCompra));
        pModelo.Add(new JProperty("IdSucursal", CuentaContable.IdSucursal));

        CDivision Division = new CDivision();
        Division.LlenaObjeto(CuentaContable.IdDivision, pConexion);
        pModelo.Add(new JProperty("Division", Division.Division));

        CTipoCompra TipoCompra = new CTipoCompra();
        TipoCompra.LlenaObjeto(CuentaContable.IdTipoCompra, pConexion);
        pModelo.Add(new JProperty("TipoCompra", TipoCompra.TipoCompra));

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(CuentaContable.IdSucursal, pConexion);
        pModelo.Add(new JProperty("Sucursal", Sucursal.Sucursal));

        return pModelo;
    }

    public string ObtenerJsonCuentaContable(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public string ObtenerCuentaContableGenerada(int pIdSucursal, int pIdDivision, int pIdTipoCompra, int pIdSubcuentaContable, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        string CuentaContable = "";
        Select.StoredProcedure.CommandText = "sp_CuentaContable_ObtenerCuenta";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdDivision", Convert.ToInt32(pIdDivision));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", Convert.ToInt32(pIdTipoCompra));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", Convert.ToInt32(pIdSubcuentaContable));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            CuentaContable = Convert.ToString(Select.Registros["CuentaContable"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return CuentaContable;
    }

    public Dictionary<string, object> ObtenerCuentaContableGeneradaSegmentos(int pIdSucursal, int pIdDivision, int pIdTipoCompra, int pIdSubcuentaContable, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_CuentaContable_ObtenerCuentaSegmentos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdDivision", Convert.ToInt32(pIdDivision));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", Convert.ToInt32(pIdTipoCompra));
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", Convert.ToInt32(pIdSubcuentaContable));
        Select.Llena(pConexion);
        Dictionary<string, object> Segmentos = new Dictionary<string, object>();
        if (Select.Registros.Read())
        {
            Segmentos.Add("SegmentoSucursal", Convert.ToString(Select.Registros["SegmentoSucursal"]));
            Segmentos.Add("SegmentoDivision", Convert.ToString(Select.Registros["SegmentoDivision"]));
            Segmentos.Add("SegmentoTipoCompra", Convert.ToString(Select.Registros["SegmentoTipoCompra"]));
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return Segmentos;
    }

    public int ExisteCuentaContable(String pCuentaContable, CConexion pConexion)
    {


        CSelect ObtenObjeto = new CSelect();
        int ExisteCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_CuentaContable_ConsultaCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", Convert.ToString(pCuentaContable));
        ObtenObjeto.Llena<CCuentaContable>(typeof(CCuentaContable), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteCuentaContable = 1;
        }
        return ExisteCuentaContable;
    }

    public int ExisteCuentaContableEditar(String pCuentaContable, int pIdCuentaContable, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_CuentaContable_ConsultaCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", Convert.ToString(pCuentaContable));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", Convert.ToInt32(pIdCuentaContable));
        ObtenObjeto.Llena<CCuentaContable>(typeof(CCuentaContable), pConexion);
        foreach (CCuentaContable CuentaContable in ObtenObjeto.ListaRegistros)
        {
            ExisteCuentaContable = 1;
        }
        return ExisteCuentaContable;
    }
}