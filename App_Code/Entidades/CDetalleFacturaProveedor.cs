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


public partial class CDetalleFacturaProveedor
{
    //Constructores

    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Metodos Especiales
    public static JObject ObtenerDetalleFacturaProveedor(JObject pModelo, int pIdDetalleFacturaProveedor, CConexion pConexion)
    {
        CDetalleFacturaProveedor DetalleFacturaProveedor = new CDetalleFacturaProveedor();
        DetalleFacturaProveedor.LlenaObjeto(pIdDetalleFacturaProveedor, pConexion);
        pModelo.Add(new JProperty("IdDetalleFacturaProveedor", DetalleFacturaProveedor.IdDetalleFacturaProveedor));
        pModelo.Add(new JProperty("IdCliente", DetalleFacturaProveedor.IdCliente));
        pModelo.Add(new JProperty("IdProyecto", DetalleFacturaProveedor.IdProyecto));

        if (DetalleFacturaProveedor.IdProyecto != 0)
        {
            CProyecto Proyecto = new CProyecto();
            Proyecto.LlenaObjeto(DetalleFacturaProveedor.IdProyecto, pConexion);
            pModelo.Add(new JProperty("Proyecto", Proyecto.NombreProyecto));
        }
        else
        {
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(DetalleFacturaProveedor.IdCliente, pConexion);

            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

            pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        }
        if (DetalleFacturaProveedor.IdPedidoDetalle != 0)
        {
            pModelo.Add(new JProperty("IdCotizacionDetalle", DetalleFacturaProveedor.IdPedidoDetalle));
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
            CotizacionDetalle.LlenaObjeto(DetalleFacturaProveedor.IdPedidoDetalle, pConexion);
            pModelo.Add(new JProperty("IdCotizacion", CotizacionDetalle.IdCotizacion));

        }
        if (DetalleFacturaProveedor.IdOrdenCompraDetalle != 0)
        {
            pModelo.Add(new JProperty("IdOrdenCompraDetalle", DetalleFacturaProveedor.IdOrdenCompraDetalle));

        }

        pModelo.Add(new JProperty("IdProducto", DetalleFacturaProveedor.IdProducto));
        pModelo.Add(new JProperty("IdServicio", DetalleFacturaProveedor.IdServicio));
        pModelo.Add(new JProperty("Cantidad", DetalleFacturaProveedor.Cantidad));
        pModelo.Add(new JProperty("Descripcion", DetalleFacturaProveedor.Descripcion));
        pModelo.Add(new JProperty("Clave", DetalleFacturaProveedor.Clave));
        pModelo.Add(new JProperty("NumeroSerie", DetalleFacturaProveedor.NumeroSerie));
        pModelo.Add(new JProperty("IdTipoCompra", DetalleFacturaProveedor.IdTipoCompra));
        pModelo.Add(new JProperty("IdUsuarioSolicitante", DetalleFacturaProveedor.IdUsuarioSolicito));

        return pModelo;
    }

    public void EliminarDetalleFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

    public void AgregarDetalleFacturaProveedor(CConexion pConexion)
    {

        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", 0);
        Agregar.StoredProcedure.Parameters["@pIdDetalleFacturaProveedor"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipo", tipo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecio", precio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNumeroSerie", numeroSerie);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPedimento", pedimento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", idOrdenCompraDetalle);
        if (fechaFacturacion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaFacturacion", fechaFacturacion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClienteProyecto", clienteProyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pReferenciaEntrega", referenciaEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedido", idPedido);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoDetalle", idPedidoDetalle);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", idUsuarioSolicito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", idSubCuentaContable);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        Agregar.Insert(pConexion);
        idDetalleFacturaProveedor = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDetalleFacturaProveedor"].Value);
    }

    public decimal ObtieneSubtotalDetalleFacturaProveedor(int pIdEncabezadoFacturaProveedor, CConexion pConexion)
    {
        decimal SubtotalFacturaProveedor = 0;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_Subtotal";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", Convert.ToInt32(pIdEncabezadoFacturaProveedor));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            SubtotalFacturaProveedor = Convert.ToInt32(Select.Registros["Subtotal"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return SubtotalFacturaProveedor;
    }

    public decimal ObtieneIVADetalleFacturaProveedor(int pIdEncabezadoFacturaProveedor, CConexion pConexion)
    {
        decimal IVADetalle = 0;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_Subtotal";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", Convert.ToInt32(pIdEncabezadoFacturaProveedor));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            IVADetalle = Convert.ToInt32(Select.Registros["IVADetalle"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return IVADetalle;
    }

    public DateTime ObtieneFechaPago(int pIdCondicionPago, DateTime pFechaFactura, CConexion pConexion)
    {
        DateTime FechaPago;
        FechaPago = new DateTime(1, 1, 1);
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_FechaPago";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdCondicionPago", Convert.ToInt32(pIdCondicionPago));
        Select.StoredProcedure.Parameters.AddWithValue("@pFechaFactura", Convert.ToDateTime(pFechaFactura));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            FechaPago = Convert.ToDateTime(Select.Registros["FechaPago"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return FechaPago;
    }

    public decimal ObtieneIVAProveedor(int pIdProveedor, CConexion pConexion)
    {
        decimal IVAProveedor = 0;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_IVAProveedor";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", Convert.ToInt32(pIdProveedor));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            IVAProveedor = Convert.ToInt32(Select.Registros["IVAProveedor"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return IVAProveedor;
    }

    public int ValidaEliminarDetalleFacturaProveedor(int pIdDetalleFacturaProveedor, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int puedeEliminar = 0;
        Select.StoredProcedure.CommandText = "sp_DetalleFacturaProveedor_PuedeEliminar";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", pIdDetalleFacturaProveedor);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            puedeEliminar = Convert.ToInt32(Select.Registros["IdDetalleRemision"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return puedeEliminar;
    }

    public string JsonStoredProcedure(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

}