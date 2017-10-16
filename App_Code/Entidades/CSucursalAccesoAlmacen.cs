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


public partial class CSucursalAccesoAlmacen
{
    //Constructores

    //Metodos Especiales
    public List<object> LlenaSucursalesDisponibles(int pIdAlmacen, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_SucursalesDisponibles_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdAlmacen", pIdAlmacen);
        Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaSucursalesAsignadas(int pIdAlmacen, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_SucursalesAsignadas_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdAlmacen", pIdAlmacen);
        Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
        return Obten.ListaRegistros;
    }

    public void BajaSucursalAlmacen(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_BajaSucursalesPorAlmacen";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@IdAlmacen", IdAlmacen);
        Eliminar.Delete(pConexion);
    }

    public void EnrolarSucursalAlmacen(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_EnrolarSucursalMuseo";
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdAlmacen", IdAlmacen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdSucursal", IdSucursal);
        Agregar.Insert(pConexion);
    }

    public static JArray ObtenerJsonSucursalAlmacen(int pIdSucursal, CConexion pConexion)
    {
        CAlmacen Almacen = new CAlmacen();
        JArray JAlmacenes = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("IdSucursal", pIdSucursal);
        foreach (CAlmacen oAlmacen in Almacen.LlenaObjetosFiltrosAlmacenes(ParametrosTI, pConexion))
        {
            JObject JAlmacen = new JObject();
            JAlmacen.Add("IdAlmacen", oAlmacen.IdAlmacen);
            JAlmacen.Add("Almacen", oAlmacen.Almacen);
            JAlmacenes.Add(JAlmacen);
        }
        return JAlmacenes;
    }
}