using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CClaseAtributo
{
    //Constructores


    //Metodos Especiales
    public List<object> LlenaObjetos_FiltroIdClaseGenerador(int pIdClaseGenerador, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_ClaseAtributo_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", pIdClaseGenerador);
        ObtenObjeto.Llena<CClaseAtributo>(typeof(CClaseAtributo), pConexion);
        return ObtenObjeto.ListaRegistros;
    }
}