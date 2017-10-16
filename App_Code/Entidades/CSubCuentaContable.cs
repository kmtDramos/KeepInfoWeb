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


public partial class CSubCuentaContable
{
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerSubCuentaContable(JObject pModelo, int pIdSubCuentaContable, CConexion pConexion)
    {

        CSubCuentaContable SubCuentaContable = new CSubCuentaContable();
        SubCuentaContable.LlenaObjeto(pIdSubCuentaContable, pConexion);
        pModelo.Add(new JProperty("IdSubCuentaContable", SubCuentaContable.IdSubCuentaContable));
        pModelo.Add(new JProperty("SubCuentaContable", SubCuentaContable.SubCuentaContable));
        pModelo.Add(new JProperty("Descripcion", SubCuentaContable.Descripcion));
        pModelo.Add(new JProperty("IdCuentaContable", SubCuentaContable.IdCuentaContable));
        pModelo.Add(new JProperty("CuentaContable", SubCuentaContable.CuentaContable));
        return pModelo;
    }

}
