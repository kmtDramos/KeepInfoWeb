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


public partial class CDescuentoProducto
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonDescuentoProducto(int pIdProducto, CConexion pConexion)
    {
        CDescuentoProducto DescuentoProducto = new CDescuentoProducto();
        JArray JDescuentoProductos = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        Parametros.Add("IdProducto", pIdProducto);
        foreach (CDescuentoProducto oDescuentoProducto in DescuentoProducto.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JDescuentoProducto = new JObject();
            JDescuentoProducto.Add("Valor", oDescuentoProducto.IdDescuentoProducto);
            JDescuentoProducto.Add("Descripcion", oDescuentoProducto.DescuentoProducto);

            JDescuentoProductos.Add(JDescuentoProducto);
        }
        return JDescuentoProductos;
    }

}