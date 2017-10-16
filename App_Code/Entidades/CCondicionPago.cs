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


public partial class CCondicionPago
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonCondicionesPago(CConexion pConexion)
    {
        CCondicionPago CondicionPago = new CCondicionPago();
        Dictionary<string, object> ParametrosCondicionPago = new Dictionary<string, object>();
        ParametrosCondicionPago.Add("Baja", false);

        JArray JACondicionesPago = new JArray();
        foreach (CCondicionPago oCondicionPago in CondicionPago.LlenaObjetosFiltros(ParametrosCondicionPago, pConexion))
        {
            JObject JCondicionPago = new JObject();
            JCondicionPago.Add("Valor", oCondicionPago.IdCondicionPago);
            JCondicionPago.Add("Descripcion", oCondicionPago.CondicionPago);
            JACondicionesPago.Add(JCondicionPago);
        }
        return JACondicionesPago;
    }
}