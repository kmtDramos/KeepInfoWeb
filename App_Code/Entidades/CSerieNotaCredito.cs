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


public partial class CSerieNotaCredito
{
    //Constructores

    //Metodos Especiales
    public bool RevisarExisteRegistro(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        bool existeSerieNotaCredito = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_SerieNotaCredito_Consultar_ExisteSerieNotaCredito";
        Select.StoredProcedure.Parameters.AddWithValue("@pSerieNotaCredito", pParametros["SerieNotaCredito"]);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pParametros["IdSucursal"]);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["NoSerieNotaCredito"]) > 0)
            {
                existeSerieNotaCredito = true;
            }
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return existeSerieNotaCredito;
    }

    public static JObject ObtenerJsonSerieNotaCredito(JObject pModelo, int pIdSerieNotaCredito, CConexion pConexion)
    {
        CSerieNotaCredito SerieNotaCredito = new CSerieNotaCredito();
        SerieNotaCredito.LlenaObjeto(pIdSerieNotaCredito, pConexion);
        pModelo.Add("IdSerieNotaCredito", SerieNotaCredito.IdSerieNotaCredito);
        pModelo.Add("SerieNotaCredito", SerieNotaCredito.SerieNotaCredito);
        pModelo.Add("IdSucursal", SerieNotaCredito.IdSucursal);
        return pModelo;
    }
}
