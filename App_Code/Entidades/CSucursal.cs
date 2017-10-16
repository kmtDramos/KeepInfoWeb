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


public partial class CSucursal
{
    //Constructores

    //Metodos Especiales
    public List<object> ObtenerSucursalesAsignadas(int pIdUsuario, int pIdEmpresa, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Sucursal_Consultar_ObtenerSucursalesAsignadas";
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pIdUsuario);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdEmpresa", pIdEmpresa);
        Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
        return Obten.ListaRegistros;
    }

    public int ExisteClaveCuentaContable(String pClaveCuentaContable, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteClaveCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Sucursal_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.Llena<CSucursal>(typeof(CSucursal), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public int ExisteClaveCuentaContableEditar(String pClaveCuentaContable, int pIdSucursal, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteClaveCuentaContable = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Sucursal_ConsultaClaveCuentaContable";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClaveCuentaContable", Convert.ToString(pClaveCuentaContable));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        ObtenObjeto.Llena<CSucursal>(typeof(CSucursal), pConexion);
        foreach (CSucursal Sucursal in ObtenObjeto.ListaRegistros)
        {
            ExisteClaveCuentaContable = 1;
        }
        return ExisteClaveCuentaContable;
    }

    public JArray ObtenerJsonDivisionesDisponibles(int pIdSucursal, CConexion pConexion)
    {
        CSucursalDivision SucursalDivision = new CSucursalDivision();

        JArray JDivisionesDisponibles = new JArray();

        foreach (CDivision oDivision in SucursalDivision.LlenaDivisionesDisponibles(pIdSucursal, pConexion))
        {
            JObject JDivision = new JObject();
            JDivision.Add("IdDivision", oDivision.IdDivision);
            JDivision.Add("Division", oDivision.Division);
            JDivisionesDisponibles.Add(JDivision);
        }
        return JDivisionesDisponibles;
    }

    public JArray ObtenerJsonCuentaBancariasDisponibles(int pIdSucursal, CConexion pConexion)
    {
        CSucursalCuentaBancaria SucursalCuentaBancaria = new CSucursalCuentaBancaria();

        JArray JCuentaBancariasDisponibles = new JArray();

        foreach (CCuentaBancaria oCuentaBancaria in SucursalCuentaBancaria.LlenaCuentaBancariasDisponibles(pIdSucursal, pConexion))
        {
            JObject JCuentaBancaria = new JObject();
            JCuentaBancaria.Add("IdCuentaBancaria", oCuentaBancaria.IdCuentaBancaria);
            JCuentaBancaria.Add("CuentaBancaria", oCuentaBancaria.CuentaBancaria);
            JCuentaBancariasDisponibles.Add(JCuentaBancaria);
        }
        return JCuentaBancariasDisponibles;
    }

    public JArray ObtenerJsonDivisionesAsignadas(int pIdSucursal, CConexion pConexion)
    {

        CSucursalDivision SucursalDivision = new CSucursalDivision();
        JArray JDivisionesDisponibles = new JArray();

        foreach (CDivision oDivision in SucursalDivision.LlenaDivisionesAsignadas(pIdSucursal, pConexion))
        {
            JObject JDivision = new JObject();
            JDivision.Add("IdDivision", oDivision.IdDivision);
            JDivision.Add("Division", oDivision.Division);
            JDivisionesDisponibles.Add(JDivision);
        }
        return JDivisionesDisponibles;
    }

    public JArray ObtenerJsonCuentaBancariasAsignadas(int pIdSucursal, CConexion pConexion)
    {

        CSucursalCuentaBancaria SucursalCuentaBancaria = new CSucursalCuentaBancaria();
        JArray JCuentaBancariasDisponibles = new JArray();

        foreach (CCuentaBancaria oCuentaBancaria in SucursalCuentaBancaria.LlenaCuentaBancariasAsignadas(pIdSucursal, pConexion))
        {
            JObject JCuentaBancaria = new JObject();
            JCuentaBancaria.Add("IdCuentaBancaria", oCuentaBancaria.IdCuentaBancaria);
            JCuentaBancaria.Add("CuentaBancaria", oCuentaBancaria.CuentaBancaria);
            JCuentaBancariasDisponibles.Add(JCuentaBancaria);
        }
        return JCuentaBancariasDisponibles;
    }

    public static JArray ObtenerSucursales(CConexion pConexion)
    {

        CSelectEspecifico Sucursales = new CSelectEspecifico();
        Sucursales.StoredProcedure.CommandText = "sp_Sucursal_ConsultarFiltros";
        Sucursales.StoredProcedure.Parameters.AddWithValue("Opcion", 1);
        Sucursales.Llena(pConexion);

        JArray JSucursales = new JArray();
        while (Sucursales.Registros.Read())
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("Valor", Convert.ToInt32(Sucursales.Registros["IdSucursal"]));
            JSucursal.Add("Descripcion", Convert.ToString(Sucursales.Registros["Sucursal"]));
            JSucursales.Add(JSucursal);
        }
        Sucursales.Registros.Close();
        return JSucursales;
    }


    public static JArray ObtenerSucursalesEmpresa(CConexion pConexion)
    {

        CSelectEspecifico Sucursales = new CSelectEspecifico();
        Sucursales.StoredProcedure.CommandText = "sp_SucursalesEmpresa_ConsultarFiltros";
        Sucursales.StoredProcedure.Parameters.AddWithValue("IdEmpresa", Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]));
        Sucursales.Llena(pConexion);

        JArray JSucursales = new JArray();
        while (Sucursales.Registros.Read())
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("Valor", Convert.ToInt32(Sucursales.Registros["IdSucursal"]));
            JSucursal.Add("Descripcion", Convert.ToString(Sucursales.Registros["Sucursal"]));
            JSucursales.Add(JSucursal);
        }
        Sucursales.Registros.Close();
        return JSucursales;
    }

    public static JArray ObtenerSucursales(int IdSucursalEjecutaServicio, CConexion pConexion)
    {
        CSelectEspecifico Sucursales = new CSelectEspecifico();
        Sucursales.StoredProcedure.CommandText = "sp_Sucursal_ConsultarFiltros";
        Sucursales.StoredProcedure.Parameters.AddWithValue("Opcion", 1);
        Sucursales.Llena(pConexion);

        JArray JSucursales = new JArray();
        while (Sucursales.Registros.Read())
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("Valor", Convert.ToInt32(Sucursales.Registros["IdSucursal"]));
            JSucursal.Add("Descripcion", Convert.ToString(Sucursales.Registros["Sucursal"]));

            if (Convert.ToInt32(Sucursales.Registros["IdSucursal"].ToString()) == IdSucursalEjecutaServicio)
            {
                JSucursal.Add(new JProperty("Selected", 1));
            }
            else
            {
                JSucursal.Add(new JProperty("Selected", 0));
            }

            JSucursales.Add(JSucursal);
        }
        Sucursales.Registros.Close();
        return JSucursales;
    }
}