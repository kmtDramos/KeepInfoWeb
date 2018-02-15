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


public partial class CFormaPago
{
    //Constructores

    //Metodos Especiales

    //Formas de pago ingresos activos
    public static JArray ObtenerFormaPagoIngresos(CConexion pConexion)
    {
        CFormaPago FormaPago = new CFormaPago();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("Baja", 0);
        JArray JAFormaPago = new JArray();
        foreach (CFormaPago oFormaPago in FormaPago.LlenaObjetosFiltros(pParametros, pConexion))
        {
            JObject JFormaPago = new JObject();
            JFormaPago.Add("Valor", oFormaPago.idFormaPago);
            JFormaPago.Add("Descripcion", oFormaPago.formaPago);

            JAFormaPago.Add(JFormaPago);
        }
        return JAFormaPago;
    }
}