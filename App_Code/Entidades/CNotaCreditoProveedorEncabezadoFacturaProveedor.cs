using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CNotaCreditoProveedorEncabezadoFacturaProveedor
{
    //Constructores

    //Metodos Especiales
    public void AgregarNotaCreditoProveedorEncabezadoFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_NotaCreditoProveedorEncabezadoFacturaProveedor_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedorEncabezadoFacturaProveedor", 0);
        Agregar.StoredProcedure.Parameters["@pIdNotaCreditoProveedorEncabezadoFacturaProveedor"].Direction = ParameterDirection.Output;
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idNotaCreditoProveedorEncabezadoFacturaProveedor = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNotaCreditoProveedorEncabezadoFacturaProveedor"].Value);
    }

    public decimal TotalAbonosNotaCreditoProveedor(int pIdNotaCreditoProveedor, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        decimal AbonosNotaCreditoProveedor = 0;
        Select.StoredProcedure.CommandText = "sp_NotaCreditoProveedor_Abonos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", Convert.ToInt32(pIdNotaCreditoProveedor));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            AbonosNotaCreditoProveedor = Convert.ToDecimal(Select.Registros["AbonosNotaCreditoProveedor"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return AbonosNotaCreditoProveedor;
    }

    public void EliminarNotaCreditoProveedorEncabezadoFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_NotaCreditoProveedorEncabezadoFacturaProveedor_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedorEncabezadoFacturaProveedor", idNotaCreditoProveedorEncabezadoFacturaProveedor);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

    public int ValidaEliminarCuentasPorCobrarDetalle(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int IdFacturaEncabezadoParcial = 0;
        Select.StoredProcedure.CommandText = "sp_NotaCreditoProveedorEncabezadoFacturaProveedor_PuedeEliminar";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", pIdFacturaEncabezado);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            IdFacturaEncabezadoParcial = Convert.ToInt32(Select.Registros["IdFacturaEncabezadoParcial"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return IdFacturaEncabezadoParcial;
    }

}

