using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public partial class CDescuentoServicio
{
    //Constructores

    //Metodos Especiales
    public bool ExisteDescuento(Decimal pDescuento, int pIdServicio, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteDescuento = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_DescuentoServicio_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pDescuento", Convert.ToDecimal(pDescuento));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdServicio", Convert.ToInt32(pIdServicio));
        ObtenObjeto.Llena<CDescuentoServicio>(typeof(CDescuentoServicio), pConexion);
        foreach (CDescuentoServicio DescuentoServicio in ObtenObjeto.ListaRegistros)
        {
            ExisteDescuento = 1;
        }
        if (ExisteDescuento == 0)
        { return false; }
        else
        { return true; }
    }

    public static JArray ObtenerJsonDescuento(int pIdServicio, CConexion pConexion)
    {
        CDescuentoServicio DescuentoServicio = new CDescuentoServicio();
        JArray JDescuentoServicios = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        Parametros.Add("IdServicio", pIdServicio);
        foreach (CDescuentoServicio oDescuentoServicio in DescuentoServicio.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JDescuentoServicio = new JObject();
            JDescuentoServicio.Add("Valor", oDescuentoServicio.IdDescuentoServicio);
            JDescuentoServicio.Add("Descripcion", oDescuentoServicio.DescuentoServicio);

            JDescuentoServicios.Add(JDescuentoServicio);
        }
        return JDescuentoServicios;
    }

}