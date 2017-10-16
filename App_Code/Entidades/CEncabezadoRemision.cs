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


public partial class CEncabezadoRemision
{
    //Constructores

    //Metodos Especiales
    public void AgregarEncabezadoRemision(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_EncabezadoRemision_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", 0);
        Agregar.StoredProcedure.Parameters["@pIdEncabezadoRemision"].Direction = ParameterDirection.Output;
        if (fechaRemision.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRemision", fechaRemision);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pConsolidado", consolidado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        if (fechaFacturacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFacturacion", fechaFacturacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idEncabezadoRemision = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEncabezadoRemision"].Value);
    }

    public static JObject ObtenerEncabezadoRemision(JObject pModelo, int pIdEncabezadoRemision, CConexion pConexion)
    {
        CEncabezadoRemision EncabezadoRemision = new CEncabezadoRemision();
        EncabezadoRemision.LlenaObjeto(pIdEncabezadoRemision, pConexion);

        pModelo.Add(new JProperty("IdEncabezadoRemision", EncabezadoRemision.IdEncabezadoRemision));
        pModelo.Add(new JProperty("IdCliente", EncabezadoRemision.IdCliente));
        pModelo.Add(new JProperty("Fecha", EncabezadoRemision.FechaRemision.ToShortDateString()));
        pModelo.Add(new JProperty("NumeroRemision", EncabezadoRemision.Folio));
        pModelo.Add(new JProperty("Nota", EncabezadoRemision.Nota));
        pModelo.Add(new JProperty("Total", EncabezadoRemision.Total));
        pModelo.Add(new JProperty("TipoCambio", EncabezadoRemision.TipoCambio));
        pModelo.Add(new JProperty("TotalFacturaLetra", EncabezadoRemision.TotalLetra));


        pModelo.Add(new JProperty("IdAlmacen", EncabezadoRemision.IdAlmacen));
        CAlmacen Almacen = new CAlmacen();
        Almacen.LlenaObjeto(EncabezadoRemision.IdAlmacen, pConexion);
        pModelo.Add(new JProperty("Almacen", Almacen.Almacen));


        if (EncabezadoRemision.Baja == true)
        {
            pModelo.Add(new JProperty("Estatus", "CANCELADA"));
            pModelo.Add(new JProperty("IdEstatus", 1));
        }
        else
        {
            pModelo.Add(new JProperty("Estatus", "ACTIVA"));
            pModelo.Add(new JProperty("IdEstatus", 0));
        }

        CDetalleRemision DetalleRemision = new CDetalleRemision();

        Dictionary<string, object> ParametrosD = new Dictionary<string, object>();
        ParametrosD.Add("IdEncabezadoRemision", EncabezadoRemision.IdEncabezadoRemision);
        DetalleRemision.LlenaObjetoFiltros(ParametrosD, pConexion);

        CCotizacion Cotizacion = new CCotizacion();
        Cotizacion.LlenaObjeto(DetalleRemision.IdEncabezadoPedido, pConexion);
        pModelo.Add(new JProperty("Pedido", Cotizacion.Folio));
        pModelo.Add(new JProperty("IdCotizacion", Cotizacion.IdCotizacion));

        pModelo.Add(new JProperty("IdProyecto", DetalleRemision.IdProyecto));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(EncabezadoRemision.IdTipoMoneda, pConexion);
        pModelo.Add("IdTipoMoneda", TipoMoneda.IdTipoMoneda);
        pModelo.Add("TipoMoneda", TipoMoneda.TipoMoneda);

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(EncabezadoRemision.IdCliente, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));

        return pModelo;
    }

    public static JToken obtenerDatosImpresionRemision(string pIdEncabezadoRemision, int pUsuario, int pSinPrecio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCotizacion jsonImpresionC = new CCotizacion();
        jsonImpresionC.StoredProcedure.CommandText = "SP_Impresion_Remision";
        jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", pIdEncabezadoRemision);
        jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pSinPrecio", pSinPrecio);
        return jsonImpresionC.ObtenerJsonJObject(ConexionBaseDatos);
    }

    public void CancelarRemision(CConexion pConexion)
    {
        CConsultaAccion Cancelar = new CConsultaAccion();
        Cancelar.StoredProcedure.CommandText = "sp_EncabezadoRemision_Cancelar";
        Cancelar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", IdEncabezadoRemision);
        Cancelar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Cancelar.Update(pConexion);
    }

    public void ActivarRemision(CConexion pConexion)
    {
        CConsultaAccion Activar = new CConsultaAccion();
        Activar.StoredProcedure.CommandText = "sp_EncabezadoRemision_Activar";
        Activar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", IdEncabezadoRemision);
        Activar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Activar.Update(pConexion);
    }

    //Obtener totales de estatus cotizacion
    public Dictionary<Int32, Int32> ObtenerTotalesEstatusRemision(int pIdSucursal, CConexion pConexion)
    {
        Dictionary<Int32, Int32> TotalesEstatus = new Dictionary<Int32, Int32>();
        CSelectEspecifico ObtenObjeto = new CSelectEspecifico();
        ObtenObjeto.StoredProcedure.CommandText = "sp_EstatusRemision_Consultar_ObtenerTotalesSinFiltro";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        ObtenObjeto.Llena(pConexion);
        while (ObtenObjeto.Registros.Read())
        {
            TotalesEstatus.Add(Convert.ToInt32(ObtenObjeto.Registros["IdEstatusRemision"]), Convert.ToInt32(ObtenObjeto.Registros["Contador"]));
        }
        ObtenObjeto.CerrarConsulta();
        return TotalesEstatus;
    }
}
