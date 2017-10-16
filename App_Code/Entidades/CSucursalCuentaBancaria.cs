using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CSucursalCuentaBancaria
{
    //Constructores

    //Metodos Especiales
    public void BajaCuentaBancariaSucursal(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_BajaCuentaBancariasPorSucursal";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@IdSucursal", IdSucursal);
        Eliminar.Delete(pConexion);
    }

    public void EnrolarCuentaBancariaSucursal(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_EnrolarCuentaBancariaSucursal";
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdSucursal", IdSucursal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdCuentaBancaria", IdCuentaBancaria);
        Agregar.Insert(pConexion);
    }

    public List<object> LlenaCuentaBancariasDisponibles(int pIdSucursal, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_CuentaBancariasDisponibles_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdSucursal", pIdSucursal);
        Obten.Llena<CCuentaBancaria>(typeof(CCuentaBancaria), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaCuentaBancariasAsignadas(int pIdSucursal, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_CuentaBancariasAsignadas_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdSucursal", pIdSucursal);
        Obten.Llena<CCuentaBancaria>(typeof(CCuentaBancaria), pConexion);
        return Obten.ListaRegistros;
    }
}