using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;


public partial class CCaracteristicaProducto
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonCaracteristicasProducto(int pIdProducto, CConexion pConexion)
    {
        CCaracteristicaProducto CaracteristicaProducto = new CCaracteristicaProducto();
        JArray JCaracteristicasProducto = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdProducto", pIdProducto);
        Parametros.Add("Baja", 0);
        foreach (CCaracteristicaProducto oCaracteristicaProducto in CaracteristicaProducto.LlenaObjetosFiltros(Parametros, pConexion))
        {
            CCaracteristica Caracteristica = new CCaracteristica();
            Caracteristica.LlenaObjeto(oCaracteristicaProducto.IdCaracteristica, pConexion);

            CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
            UnidadCaracteristica.LlenaObjeto(oCaracteristicaProducto.IdUnidadCaracteristica, pConexion);

            JObject JCaracteristicaProducto = new JObject();
            JCaracteristicaProducto.Add("IdCaracteristicaProducto", oCaracteristicaProducto.IdCaracteristicaProducto);
            JCaracteristicaProducto.Add("Caracteristica", Caracteristica.Caracteristica);
            JCaracteristicaProducto.Add("UnidadCaracteristica", UnidadCaracteristica.UnidadCaracteristica);
            JCaracteristicaProducto.Add("Valor", oCaracteristicaProducto.Valor);
            JCaracteristicasProducto.Add(JCaracteristicaProducto);
        }
        return JCaracteristicasProducto;
    }
}