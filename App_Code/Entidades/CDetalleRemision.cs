using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CDetalleRemision
{
    //Constructores

    //Metodos Especiales
    public void AgregarDetalleRemision(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_DetalleRemision_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", 0);
        Agregar.StoredProcedure.Parameters["@pIdDetalleRemision"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", idDetalleFacturaProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoPedido", idEncabezadoPedido);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetallePedido", idDetallePedido);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", idAlmacen);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idDetalleRemision = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDetalleRemision"].Value);
    }

    public void EliminarDetalleRemision(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_DetalleRemision_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", idDetalleRemision);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

}