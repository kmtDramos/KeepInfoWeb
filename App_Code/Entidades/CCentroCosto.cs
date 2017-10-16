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


public partial class CCentroCosto
{
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerCentroCosto(JObject pModelo, int pIdCentroCosto, CConexion pConexion)
    {

        CCentroCosto CentroCosto = new CCentroCosto();
        CentroCosto.LlenaObjeto(pIdCentroCosto, pConexion);
        pModelo.Add(new JProperty("IdCentroCosto", CentroCosto.IdCentroCosto));
        pModelo.Add(new JProperty("CentroCosto", CentroCosto.CentroCosto));
        pModelo.Add(new JProperty("Monto", CentroCosto.Monto));
        pModelo.Add(new JProperty("Descripcion", CentroCosto.Descripcion));
        pModelo.Add(new JProperty("IdCuentaContable", CentroCosto.IdCuentaContable));
        pModelo.Add(new JProperty("CuentaContable", CentroCosto.CuentaContable));
        return pModelo;
    }

}