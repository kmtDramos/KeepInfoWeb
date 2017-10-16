using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
public partial class Ejemplo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //[WebMethod]
    //public static string ObtenerResultadosOracle()
    //{
    //    //Abrir Conexion
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
    //    JObject oRespuesta = new JObject();
    //    //¿La conexion se establecio?
    //    if (respuesta == "Conexion Establecida")
    //    {
    //        oRespuesta.Add(new JProperty("Error", 0));
    //        oRespuesta.Add(new JProperty("Descripcion", respuesta));
    //        ConexionBaseDatos.ConectarBaseDatosOracle();
    //    }
    //    else
    //    {
    //        oRespuesta.Add(new JProperty("Error", 1));
    //        oRespuesta.Add(new JProperty("Descripcion", respuesta));
    //    }
    //    return oRespuesta.ToString();
    //}

    [WebMethod]
    public static string CortarPalabra()
    {
        string etiqueta = "{etiqueta}";
        string cadena = "11111 22222 33333 44444 55555 ";
        int longitudCadena = cadena.Length;
        int cadenaMaxima = 255;
        decimal noVueltas = longitudCadena / cadenaMaxima;
        decimal residuo = longitudCadena % cadenaMaxima;

        int actual = 0;
        List<string> imprimir = new List<string>();
        for (int i = 1; i <= noVueltas; i++)
        {
            if (i < noVueltas)
            {
                imprimir.Add(cadena.Substring(actual, cadenaMaxima) + etiqueta);
            }
            else
            {
                if (residuo > 0)
                {
                    imprimir.Add(cadena.Substring(actual, cadenaMaxima) + etiqueta);
                }
                else
                {
                    imprimir.Add(cadena.Substring(actual, cadenaMaxima));
                }
            }
            actual = actual + 6;
        }


        if (residuo > 0)
        {
            imprimir.Add(cadena.Substring(actual, longitudCadena - actual));
        }

        JObject oRespuesta = new JObject();
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Descripcion", "Cadena cortada correctamente."));

        return oRespuesta.ToString();
    }
}