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


public partial class CAlmacen
{
    //Constructores

    //Metodos Especiales
    public JArray ObtenerJsonSucursalesDisponibles(int pIdAlmacen, CConexion pConexion)
    {

        CSucursalAccesoAlmacen SucursalAccesoAlmacen = new CSucursalAccesoAlmacen();
        JArray JSucursalesDisponibles = new JArray();

        foreach (CSucursal oSucursal in SucursalAccesoAlmacen.LlenaSucursalesDisponibles(pIdAlmacen, pConexion))
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("IdSucursal", oSucursal.IdSucursal);
            JSucursal.Add("Sucursal", oSucursal.Sucursal);
            JSucursalesDisponibles.Add(JSucursal);
        }
        return JSucursalesDisponibles;
    }

    public List<object> ObtenerAlmacenesAsignadas(int pIdUsuario, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Reportes_ConsultarAlmacenAsignado";
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pIdUsuario);
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        return Obten.ListaRegistros;
    }
    public List<object> ObtenerSucursalesAsignadas(CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Reportes_ConsultarSucursales";
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        return Obten.ListaRegistros;
    }

    public JArray ObtenerJsonSucursalesAsignadas(int pIdAlmacen, CConexion pConexion)
    {

        CSucursalAccesoAlmacen SucursalAccesoAlmacen = new CSucursalAccesoAlmacen();
        JArray JSucursalesDisponibles = new JArray();

        foreach (CSucursal oSucursal in SucursalAccesoAlmacen.LlenaSucursalesAsignadas(pIdAlmacen, pConexion))
        {
            JObject JSucursal = new JObject();
            JSucursal.Add("IdSucursal", oSucursal.IdSucursal);
            JSucursal.Add("Sucursal", oSucursal.Sucursal);
            JSucursalesDisponibles.Add(JSucursal);
        }
        return JSucursalesDisponibles;
    }


    public List<object> LlenaObjetosFiltrosAlmacenes(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_EncabezadoFacturaProveedor_ConsultarAlmacenAsignado";
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
        Obten.Llena<CAlmacen>(typeof(CAlmacen), pConexion);
        return Obten.ListaRegistros;
    }

}