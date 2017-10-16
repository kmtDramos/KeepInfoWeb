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

public partial class CTipoCambioDiarioOficial
{
    public bool ExisteTipoCambioDiarioOficial(int pIdTipoMonedaOrigen, int pIdTipoMonedaDestino, DateTime pFecha, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteTipoCambio = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_TipoCambioDiarioOficial_ConsultarFiltros";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", pIdTipoMonedaOrigen);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", pIdTipoMonedaDestino);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
        ObtenObjeto.Llena<CTipoCambio>(typeof(CTipoCambio), pConexion);
        foreach (CTipoCambio TipoCambio in ObtenObjeto.ListaRegistros)
        {
            ExisteTipoCambio = 1;
        }
        if (ExisteTipoCambio == 0)
        { return false; }
        else
        { return true; }
    }

    public static decimal ObtenerTipoCambio(int pIdTipoMonedaOrigen, DateTime pFecha, CConexion pConexion)
    {
        decimal tipoCambio = 0;
        CSelectEspecifico SObtenerTipoCambio = new CSelectEspecifico();
        SObtenerTipoCambio.StoredProcedure.CommandText = "sp_TipoCambioDiarioOficial_Consultar_ObtenerTipoCambio";
        SObtenerTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", pIdTipoMonedaOrigen);
        SObtenerTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", 1);
        SObtenerTipoCambio.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
        SObtenerTipoCambio.Llena(pConexion);

        if (SObtenerTipoCambio.Registros.Read())
        {
            tipoCambio = Convert.ToDecimal(SObtenerTipoCambio.Registros["TipoCambioDiarioOficial"]);
        }

        SObtenerTipoCambio.CerrarConsulta();
        return tipoCambio;
    }
}