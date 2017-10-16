using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CExistenciasDeInventario
{
    //Constructores

    //Metodos Especiales
    public void ObtenerJsonExistencia(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_ExistenciaDistribuida_Consulta";
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
        Obten.Llena<CExistenciasDeInventario>(typeof(CExistenciasDeInventario), pConexion);
        foreach (CExistenciasDeInventario O in Obten.ListaRegistros)
        {
            idExistenciasDeInventario = O.IdExistenciasDeInventario;
            fechaInicio = O.FechaInicio;
            cantidadExistencia = O.CantidadExistencia;
            fechaFin = O.FechaFin;
            comentario = O.Comentario;
            idProducto = O.IdProducto;
            idAlmacen = O.IdAlmacen;
        }
    }
}