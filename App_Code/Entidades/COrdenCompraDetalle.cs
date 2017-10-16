using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class COrdenCompraDetalle
{
    //Constructores

    //Metodos Especiales
    public void EliminarOrdenCompraDetalle(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_OrdenCompraDetalle_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", idOrdenCompraDetalle);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

    public void AgregarOrdenCompraDetalle(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_OrdenCompraDetalle_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraDetalle", 0);
        Agregar.StoredProcedure.Parameters["@pIdOrdenCompraDetalle"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoEncabezado", idPedidoEncabezado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoDetalle", idPedidoDetalle);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", idOrdenCompraEncabezado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEstatus", idOrdenCompraEstatus);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUnidadCompraVenta", idUnidadCompraVenta);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pRecepcionCantidad", recepcionCantidad);
        if (fechaRecepcion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRecepcion", fechaRecepcion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idOrdenCompraDetalle = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrdenCompraDetalle"].Value);
    }

}