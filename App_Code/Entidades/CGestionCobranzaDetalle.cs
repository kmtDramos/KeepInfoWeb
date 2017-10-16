using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CGestionCobranzaDetalle
{
    //Constructores

    //Metodos Especiales
    public List<object> LlenaObjetosFiltrosOrdenarIdDesc(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "spb_GestionCobranzaDetalle_ConsultarFiltros_OrdenarIdDesc";
        foreach (KeyValuePair<string, object> parametro in pParametros)
        {
            if (parametro.Key == "Opcion")
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@" + parametro.Key, parametro.Value);
            }
            else
            {
                Obten.StoredProcedure.Parameters.AddWithValue("@p" + parametro.Key, parametro.Value);
            }
        }
        Obten.Llena<CGestionCobranzaDetalle>(typeof(CGestionCobranzaDetalle), pConexion);
        return Obten.ListaRegistros;
    }
}