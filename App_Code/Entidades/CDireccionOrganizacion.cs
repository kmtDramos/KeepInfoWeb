using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CDireccionOrganizacion
{
    //Constructores

    //Metodos Especiales
    public List<object> LlenaObjetosFiltrosDireccionOrganizacion(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_DireccionOrganizacion_Consultar_ObtenerDireccionesNoFiscales";
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
        Obten.Llena<CDireccionOrganizacion>(typeof(CDireccionOrganizacion), pConexion);
        return Obten.ListaRegistros;
    }

}