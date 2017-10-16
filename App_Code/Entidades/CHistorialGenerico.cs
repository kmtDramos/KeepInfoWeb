using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CHistorialGenerico
{
    //Propiedades Privadas
    //Constructores

    //Metodos Especiales
    public void AgregarHistorialGenerico(string pClase, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_ClaseGenerador_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pClase", pClase);
        Obten.Llena<CClaseGenerador>(typeof(CClaseGenerador), pConexion);
        foreach (CClaseGenerador O in Obten.ListaRegistros)
        {
            idClaseGenerador = O.IdClaseGenerador;
        }

        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "spb_HistorialGenerico_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdHistorialGenerico", 0);
        Agregar.StoredProcedure.Parameters["@pIdHistorialGenerico"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdClaseGenerador", idClaseGenerador);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdGenerico", idGenerico);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        if (fecha.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFecha", fecha);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pComentario", comentario);
        Agregar.Insert(pConexion);
        idHistorialGenerico = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdHistorialGenerico"].Value);
    }
}