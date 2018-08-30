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

public partial class CSerieComplementoPago
{
    //Constructores

    //Metodos Especiales
    public bool RevisarExisteRegistro(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        bool existeSeriePago = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_SerieComplementoPago_Consultar_ExisteSerieComplementoPago";
        Select.StoredProcedure.Parameters.AddWithValue("@pSeriePago", pParametros["SeriePago"]);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pParametros["IdSucursal"]);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["NoSeriePago"]) > 0)
            {
                existeSeriePago = true;
            }
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return existeSeriePago;
    }
    public static JObject ObtenerJsonSeriePago(JObject pModelo, int pIdSeriePago, CConexion pConexion)
    {
        CSerieComplementoPago SeriePago = new CSerieComplementoPago();
        SeriePago.LlenaObjeto(pIdSeriePago, pConexion);
        pModelo.Add("IdSeriePago", SeriePago.IdSerieComplementoPago);
        pModelo.Add("SeriePago", SeriePago.SerieComplementoPago);
        pModelo.Add("IdSucursal", SeriePago.IdSucursal);
        return pModelo;
    }

}
