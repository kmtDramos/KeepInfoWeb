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


public partial class CEncabezadoFacturaProveedor
{
    //Constructores

    //Metodos Especiales
    public static JObject ObtenerEncabezadoFacturaProveedor(JObject pModelo, int pIdEncabezadoFacturaProveedor, CConexion pConexion)
    {
        CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
        EncabezadoFacturaProveedor.LlenaObjeto(pIdEncabezadoFacturaProveedor, pConexion);
        pModelo.Add(new JProperty("IdEncabezadoFacturaProveedor", EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor));
        pModelo.Add(new JProperty("IdProveedor", EncabezadoFacturaProveedor.IdProveedor));
        pModelo.Add(new JProperty("NumeroFactura", EncabezadoFacturaProveedor.NumeroFactura));

        CProveedor Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(EncabezadoFacturaProveedor.IdProveedor, pConexion);

        CCondicionPago CondicionPago = new CCondicionPago();
        CondicionPago.LlenaObjeto(EncabezadoFacturaProveedor.IdCondicionPago, pConexion);
        pModelo.Add(new JProperty("IdCondicionPago", CondicionPago.IdCondicionPago));
        pModelo.Add(new JProperty("CondicionPago", CondicionPago.CondicionPago));

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));

        CDivision Division = new CDivision();
        Division.LlenaObjeto(EncabezadoFacturaProveedor.IdDivision, pConexion);
        pModelo.Add(new JProperty("IdDivision", Division.IdDivision));
        pModelo.Add(new JProperty("Division", Division.Division));

        pModelo.Add(new JProperty("IdAlmacen", EncabezadoFacturaProveedor.IdAlmacen));
        CAlmacen Almacen = new CAlmacen();
        Almacen.LlenaObjeto(EncabezadoFacturaProveedor.IdAlmacen, pConexion);
        pModelo.Add(new JProperty("Almacen", Almacen.Almacen));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(EncabezadoFacturaProveedor.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        pModelo.Add(new JProperty("FechaFactura", EncabezadoFacturaProveedor.Fecha.ToShortDateString()));
        pModelo.Add(new JProperty("FechaPago", EncabezadoFacturaProveedor.FechaPago.ToShortDateString()));
        pModelo.Add(new JProperty("NumeroGuia", EncabezadoFacturaProveedor.NumeroGuia));

        pModelo.Add(new JProperty("SubtotalFactura", EncabezadoFacturaProveedor.Subtotal));
        pModelo.Add(new JProperty("IVAFactura", EncabezadoFacturaProveedor.IVA));
        pModelo.Add(new JProperty("TotalFactura", EncabezadoFacturaProveedor.Total));
        pModelo.Add(new JProperty("TotalFacturaLetra", EncabezadoFacturaProveedor.TotalLetra));
        if (EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor == 1)
        {
            pModelo.Add(new JProperty("Estatus", "CANCELADA"));
        }
        else if (EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor == 2)
        {
            pModelo.Add(new JProperty("Estatus", "PAGADA PARCIAL"));
        }
        else if (EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor == 3)
        {
            pModelo.Add(new JProperty("Estatus", "PAGADA TOTAL"));
        }
        else
        {
            pModelo.Add(new JProperty("Estatus", "PENDIENTE"));
        }

        pModelo.Add(new JProperty("IdEstatus", Convert.ToInt32(EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor)));

        pModelo.Add(new JProperty("TipoCambioFactura", EncabezadoFacturaProveedor.TipoCambio));

        // Cliente
        CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
        CCliente Cliente = new CCliente();
        COrganizacion ClienteOrganizacion = new COrganizacion();

        // DANIEL
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdEncabezadoFacturaProveedor", EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor);
        DetalleFacturaProveedor.LlenaObjetoFiltros(Parametros, pConexion);
        /*
         * No se esta llenano bien el objeto DetalleFacturaProveedor
         * Siempre devuelve 0 el IdPedido aunque si devuelve bien el id del cliente
         * La el error esta en las lineas debajo de // Cotizacion
         */

        Cliente.LlenaObjeto(DetalleFacturaProveedor.IdCliente, pConexion);
        ClienteOrganizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("Cliente", ClienteOrganizacion.RazonSocial));
        pModelo.Add(new JProperty("IdCliente", Cliente.IdCliente));
        Parametros.Clear();

        // Cotizacion 
        pModelo.Add(new JProperty("Cotizaciones", CCotizacion.ObtenerPedidosClienteRecepcion(Cliente.IdCliente, DetalleFacturaProveedor.IdPedido, pConexion)));
        // linea de debug para ver el idPedido se se esta obteniendo.
        pModelo.Add(new JProperty("IdPedido", DetalleFacturaProveedor.IdPedido));


        return pModelo;
    }
    public int ExisteNumeroFactura(string pNumeroFactura, int pIdProveedor, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteNumeroFactura = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_EncabezadoNumeroFacturaProveedor_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pNumeroFactura", Convert.ToString(pNumeroFactura));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", Convert.ToInt32(pIdProveedor));
        ObtenObjeto.Llena<CServicio>(typeof(CServicio), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteNumeroFactura = 1;
        }
        return ExisteNumeroFactura;
    }

    public static JObject ObtenerDetallePedido(JObject pModelo, int pIdDetallePedido, CConexion pConexion)
    {
        CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
        CotizacionDetalle.LlenaObjeto(pIdDetallePedido, pConexion);
        pModelo.Add(new JProperty("IdCotizacionDetalle", CotizacionDetalle.IdCotizacionDetalle));
        pModelo.Add(new JProperty("IdProducto", CotizacionDetalle.IdProducto));
        pModelo.Add(new JProperty("IdServicio", CotizacionDetalle.IdServicio));
        pModelo.Add(new JProperty("Clave", CotizacionDetalle.Clave));
        pModelo.Add(new JProperty("Descripcion", CotizacionDetalle.Descripcion));
        pModelo.Add(new JProperty("Cantidad", CotizacionDetalle.RecepcionCantidad));

        CCotizacion Cotizacion = new CCotizacion();
        Cotizacion.LlenaObjeto(CotizacionDetalle.IdCotizacion, pConexion);

        pModelo.Add(new JProperty("IdUsuarioSolicitante", Cotizacion.IdUsuarioAgente));
        return pModelo;
    }

    public static JObject ObtenerDetalleOrdenCompra(JObject pModelo, int pIdDetalleOrdenCompra, CConexion pConexion)
    {
        COrdenCompraDetalle OrdenCompraDetalle = new COrdenCompraDetalle();
        OrdenCompraDetalle.LlenaObjeto(pIdDetalleOrdenCompra, pConexion);

        COrdenCompraEncabezado OrdenCompra = new COrdenCompraEncabezado();
        OrdenCompra.LlenaObjeto(OrdenCompraDetalle.IdOrdenCompraEncabezado, pConexion);

        pModelo.Add(new JProperty("IdOrdenCompraDetalle", OrdenCompraDetalle.IdOrdenCompraDetalle));
        pModelo.Add(new JProperty("IdProducto", OrdenCompraDetalle.IdProducto));
        pModelo.Add(new JProperty("IdServicio", OrdenCompraDetalle.IdServicio));
        pModelo.Add(new JProperty("Cantidad", OrdenCompraDetalle.RecepcionCantidad));
        pModelo.Add(new JProperty("Costo", OrdenCompraDetalle.Costo));
        pModelo.Add(new JProperty("IdTipoMoneda", OrdenCompra.IdTipoMoneda));

        COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
        OrdenCompraEncabezado.LlenaObjeto(OrdenCompraDetalle.IdOrdenCompraEncabezado, pConexion);
        pModelo.Add(new JProperty("IdTipoCompra", OrdenCompraDetalle.IdTipoCompra));

        pModelo.Add(new JProperty("IdCliente", OrdenCompraEncabezado.IdCliente));
        pModelo.Add(new JProperty("IdProyecto", OrdenCompraEncabezado.IdProyecto));

        if (OrdenCompraEncabezado.IdProyecto != 0)
        {
            CProyecto Proyecto = new CProyecto();
            Proyecto.LlenaObjeto(OrdenCompraEncabezado.IdProyecto, pConexion);
            pModelo.Add(new JProperty("Proyecto", Proyecto.NombreProyecto));
        }

        if (OrdenCompraDetalle.IdPedidoDetalle != 0)
        {
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            CotizacionDetalle.LlenaObjeto(OrdenCompraDetalle.IdPedidoDetalle, pConexion);
            CCotizacion Cotizacion = new CCotizacion();
            Cotizacion.LlenaObjeto(CotizacionDetalle.IdCotizacion, pConexion);

            pModelo.Add(new JProperty("IdUsuarioSolicitante", Cotizacion.IdUsuarioAgente));
            pModelo.Add(new JProperty("IdCotizacion", Cotizacion.IdCotizacion));

            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(Cotizacion.IdCliente, pConexion);

            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

            pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));

        }

        return pModelo;
    }

    public void CancelarFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Cancelar = new CConsultaAccion();
        Cancelar.StoredProcedure.CommandText = "sp_EncabezadoFacturaProveedor_Cancelar";
        Cancelar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
        Cancelar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Cancelar.Update(pConexion);
    }

    public void ActivarFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Activar = new CConsultaAccion();
        Activar.StoredProcedure.CommandText = "sp_EncabezadoFacturaProveedor_Activar";
        Activar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
        Activar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Activar.Update(pConexion);
    }

    public int ValidaCancelarEncabezadoFacturaProveedor(int pIdEncabezadoFacturaProveedor, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int puedeCancelar = 0;
        Select.StoredProcedure.CommandText = "sp_EncabezadoFacturaProveedor_PuedeCancelar";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", pIdEncabezadoFacturaProveedor);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            puedeCancelar = Convert.ToInt32(Select.Registros["IdDetalleRemision"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return puedeCancelar;
    }

    public int ValidaActivarEncabezadoFacturaProveedor(int pIdEncabezadoFacturaProveedor, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int puedeCancelar = 0;
        Select.StoredProcedure.CommandText = "sp_EncabezadoFacturaProveedor_PuedeActivar";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", pIdEncabezadoFacturaProveedor);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            puedeCancelar = Convert.ToInt32(Select.Registros["IdDetalleRemision"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return puedeCancelar;
    }

    //Obtener totales de estatus cotizacion
    public Dictionary<Int32, Int32> ObtenerTotalesEstatusRecepcion(int pIdSucursal, CConexion pConexion)
    {
        Dictionary<Int32, Int32> TotalesEstatus = new Dictionary<Int32, Int32>();
        CSelectEspecifico ObtenObjeto = new CSelectEspecifico();
        ObtenObjeto.StoredProcedure.CommandText = "sp_EstatusRecepcion_Consultar_ObtenerTotalesSinFiltro";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", Convert.ToInt32(pIdSucursal));
        ObtenObjeto.Llena(pConexion);
        while (ObtenObjeto.Registros.Read())
        {
            TotalesEstatus.Add(Convert.ToInt32(ObtenObjeto.Registros["IdEstatusRecepcion"]), Convert.ToInt32(ObtenObjeto.Registros["Contador"]));
        }
        ObtenObjeto.CerrarConsulta();
        return TotalesEstatus;
    }
}
