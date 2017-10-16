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


public partial class CSucursalDivision
{
    //Constructores

    //Metodos Especiales
    public List<object> LlenaDivisionesDisponibles(int pIdSucursal, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_DivisionesDisponibles_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdSucursal", pIdSucursal);
        Obten.Llena<CDivision>(typeof(CDivision), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaDivisionesAsignadas(int pIdSucursal, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_DivisionesAsignadas_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdSucursal", pIdSucursal);
        Obten.Llena<CDivision>(typeof(CDivision), pConexion);
        return Obten.ListaRegistros;
    }

    public void BajaDivisionSucursal(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_BajaDivisionesPorSucursal";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@IdSucursal", IdSucursal);
        Eliminar.Delete(pConexion);
    }

    public void EnrolarDivisionSucursal(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_EnrolarDivisionSucursal";
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdSucursal", IdSucursal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdDivision", IdDivision);
        Agregar.Insert(pConexion);
    }

    public static JArray ObtenerJsonSucursalDivision(int pIdSucursal, CConexion pConexion)
    {
        CDivision Division = new CDivision();

        JArray JDivisiones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Opcion", 1);
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdSucursal", pIdSucursal);
        foreach (CDivision oDivision in Division.LlenaObjetosFiltrosDivisiones(ParametrosTI, pConexion))
        {
            JObject JDivision = new JObject();
            JDivision.Add("IdDivision", oDivision.IdDivision);
            JDivision.Add("Division", oDivision.Division);
            JDivisiones.Add(JDivision);
        }
        return JDivisiones;
    }
}
