using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class CEstatusLevantamiento
{
    //Constructores

    //Metodos Especiales

    //Obtener totales de estatus cotizacion
    public Dictionary<Int32, Int32> ObtenerTotalesEstatusLevantamiento(int pIdSucursal, string pFechaInicial, string pFechaFinal, int pPorFecha, int pFolio, string pRazonSocial, int pAI, int pIdEstatusLevantamiento, CConexion pConexion)
    {
        Dictionary<Int32, Int32> TotalesEstatus = new Dictionary<Int32, Int32>();
        CSelectEspecifico ObtenObjeto = new CSelectEspecifico();
        ObtenObjeto.StoredProcedure.CommandText = "sp_EstatusLevantamiento_Consultar_ObtenerTotalesSinFiltro";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", Convert.ToString(pFechaInicial));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", Convert.ToString(pFechaFinal));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pPorFecha", Convert.ToInt32(pPorFecha));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pFolio", Convert.ToInt32(pFolio));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", Convert.ToString(pRazonSocial));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pBaja", Convert.ToInt32(pAI));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdEstatusLevantamiento", Convert.ToInt32(pIdEstatusLevantamiento));

        ObtenObjeto.Llena(pConexion);
        while (ObtenObjeto.Registros.Read())
        {
            TotalesEstatus.Add(Convert.ToInt32(ObtenObjeto.Registros["IdEstatusLevantamiento"]), Convert.ToInt32(ObtenObjeto.Registros["Contador"]));
        }
        ObtenObjeto.CerrarConsulta();
        return TotalesEstatus;
    }
}
