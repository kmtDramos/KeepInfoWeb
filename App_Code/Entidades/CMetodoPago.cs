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


public partial class CMetodoPago
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerJsonMetodosPago(CConexion pConexion)
    {
        CMetodoPago MetodoPago = new CMetodoPago();
        Dictionary<string, object> ParametrosMetodoPago = new Dictionary<string, object>();
        ParametrosMetodoPago.Add("Baja", 0);

        JArray JAMetodosPago = new JArray();
        foreach (CMetodoPago oMetodoPago in MetodoPago.LlenaObjetosFiltros(ParametrosMetodoPago, pConexion))
        {
            JObject JMetodoPago = new JObject();
            JMetodoPago.Add("Valor", oMetodoPago.IdMetodoPago);
            JMetodoPago.Add("Descripcion", oMetodoPago.MetodoPago);
            JMetodoPago.Add("Clave", oMetodoPago.Clave);
            JAMetodosPago.Add(JMetodoPago);
        }
        return JAMetodosPago;
    }

    public static JArray ObtenerJsonMetodoPago(int pIdMetodoPago, CConexion pConexion)
    {
        CMetodoPago MetodoPago = new CMetodoPago();
        JArray JMetodoPagos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdTipoMovimiento", 1);

        foreach (CMetodoPago oMetodoPago in MetodoPago.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JMetodoPago = new JObject();
            JMetodoPago.Add("IdMetodoPago", oMetodoPago.IdMetodoPago);
            JMetodoPago.Add("MetodoPago", oMetodoPago.MetodoPago);
            if (oMetodoPago.IdMetodoPago == pIdMetodoPago)
            {
                JMetodoPago.Add(new JProperty("Selected", 1));
            }
            else
            {
                JMetodoPago.Add(new JProperty("Selected", 0));
            }
            JMetodoPagos.Add(JMetodoPago);
        }
        return JMetodoPagos;
    }

    /*
    public static JArray ObtenerGiros(CConexion pConexion) {
        JArray Giros = new JArray();

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Giro_SelectGirosActivos";

        Select.Llena(pConexion);

        while (Select.Registros.Read()) {
            JObject Giro = new JObject();
            Giro.Add("Valor", Convert.ToString(Select.Registros["IdGiro"]));
            Giro.Add("Descripcion", Convert.ToString(Select.Registros["Giro"]));
            Giros.Add(Giro);
        }

        Select.CerrarConsulta();

        return Giros;
    }
     */

    //Metodos de pago ingresos activos
    public static JArray ObtenerMetodoPagoIngresos(CConexion pConexion)
    {
        JArray MetodosPago = new JArray();

        CSelectEspecifico Ingresos = new CSelectEspecifico();
        Ingresos.StoredProcedure.CommandText = "sp_MetodoPago_SelectIngresosActivos";

        Ingresos.Llena(pConexion);

        while (Ingresos.Registros.Read())
        {
            JObject MetPago = new JObject();
            MetPago.Add("Valor", Convert.ToString(Ingresos.Registros["IdMetodoPago"]));
            MetPago.Add("Descripcion", Convert.ToString(Ingresos.Registros["metodopago"]));
            MetPago.Add("Clave", Convert.ToString(Ingresos.Registros["clave"]));
            MetodosPago.Add(MetPago);
        }

        return MetodosPago;

    }

}
