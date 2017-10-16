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

public partial class CTipoCuentaContable
{
    //Constructores

    //Metodos Especiales
    public static JArray ObtenerTiposCuentasContables(CConexion pConexion)
    {
        CSelectEspecifico TiposCuentasContables = new CSelectEspecifico();
        TiposCuentasContables.StoredProcedure.CommandText = "sp_TipoCuentaContable_ConsultarFiltros";
        TiposCuentasContables.StoredProcedure.Parameters.AddWithValue("Opcion", 1);
        TiposCuentasContables.Llena(pConexion);

        JArray JTiposCuentasContables = new JArray();
        while (TiposCuentasContables.Registros.Read())
        {
            JObject JTipoCuentaContable = new JObject();
            JTipoCuentaContable.Add("Valor", Convert.ToInt32(TiposCuentasContables.Registros["IdTipoCuentaContable"]));
            JTipoCuentaContable.Add("Descripcion", Convert.ToString(TiposCuentasContables.Registros["TipoCuentaContable"]));
            JTiposCuentasContables.Add(JTipoCuentaContable);
        }
        TiposCuentasContables.Registros.Close();
        return JTiposCuentasContables;
    }
}