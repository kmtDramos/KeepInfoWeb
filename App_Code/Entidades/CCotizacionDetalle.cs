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

public partial class CCotizacionDetalle
{
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerCotizacionDetalleTotales(JObject pModelo, int pIdCotizacion, decimal pIva, CConexion pConexion)
    {

        CSelectEspecifico Obten = new CSelectEspecifico();
        Obten.StoredProcedure.CommandText = "sp_CotizacionDetalle_ConsultarTipoIVA";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdCotizacion);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIva", Convert.ToDecimal(pIva));
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena(pConexion);
        if (Obten.Registros.Read())
        {
            pModelo.Add("Subtotal", Math.Round(Convert.ToDecimal(Obten.Registros["Subtotal"]), 2));
            pModelo.Add("Descuento", Math.Round(Convert.ToDecimal(Obten.Registros["Descuento"]), 2));
            pModelo.Add("SubtotalDescuento", Math.Round(Convert.ToDecimal(Obten.Registros["SubtotalDescuento"]), 2));
            pModelo.Add("Iva", Math.Round(Convert.ToDecimal(Obten.Registros["Iva"]), 2));
            pModelo.Add("Total", Math.Round(Convert.ToDecimal(Obten.Registros["Total"]), 2));
        }
        Obten.Registros.Close();
        Obten.Registros.Dispose();
        return pModelo;
    }

    public int ExisteCotizacionDetalleClave(int pIdCotizacion, string pClave, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteCotizacionDetalleClave = 0; //No existe
        ObtenObjeto.StoredProcedure.CommandText = "sp_CotizacionDetalle_Consultar";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", Convert.ToInt32(pIdCotizacion));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pClave));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        ObtenObjeto.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
        foreach (CCotizacionDetalle CotizacionDetalle in ObtenObjeto.ListaRegistros)
        {
            if (CotizacionDetalle.IdCotizacion == pIdCotizacion)
            {
                ExisteCotizacionDetalleClave = 1; //Existe
                break;
            }
        }
        return ExisteCotizacionDetalleClave;
    }

    public void EditarOrdenacion(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_CotizacionDetalle_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", idCotizacionDetalle);
        Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenacion", ordenacion);
        Editar.Update(pConexion);
    }

    public bool ValidarBaja(int pIdCotizacion, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int DocumentoLigado = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_CotizacionDetalle_Consultar";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdCotizacion);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
        ObtenObjeto.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
        foreach (CCotizacionDetalle CotizacionDetalle in ObtenObjeto.ListaRegistros)
        {
            DocumentoLigado = 1;
        }
        if (DocumentoLigado == 0)
        { return false; }
        else
        { return true; }
    }

}