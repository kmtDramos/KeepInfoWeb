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


public partial class CTipoCambio
{
    //Constructores

    //Metodos Especiales
    public void LlenaObjetoFiltrosTipoCambio(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_TipoCambio_ConsultarFiltros";
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
        Obten.Llena<CTipoCambio>(typeof(CTipoCambio), pConexion);
        foreach (CTipoCambio O in Obten.ListaRegistros)
        {
            idTipoCambio = O.IdTipoCambio;
            tipoCambio = O.TipoCambio;
            fecha = O.Fecha;
            idTipoMonedaOrigen = O.IdTipoMonedaOrigen;
            idTipoMonedaDestino = O.IdTipoMonedaDestino;
            baja = O.Baja;


        }
    }

    public bool ExisteTipoCambio(int pIdTipoMonedaOrigen, int pIdTipoMonedaDestino, DateTime pFecha, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteTipoCambio = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_TipoCambio_ConsultarFiltros";
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

    public bool ExisteTipoCambioOrigen(int pIdTipoMonedaOrigen, DateTime pFecha, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteTipoCambioOrigen = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_TipoCambio_ConsultarFiltros";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", pIdTipoMonedaOrigen);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pFecha", pFecha);
        ObtenObjeto.Llena<CTipoCambio>(typeof(CTipoCambio), pConexion);
        foreach (CTipoCambio TipoCambio in ObtenObjeto.ListaRegistros)
        {
            ExisteTipoCambioOrigen = 1;
        }
        if (ExisteTipoCambioOrigen == 0)
        { return false; }
        else
        { return true; }
    }
}