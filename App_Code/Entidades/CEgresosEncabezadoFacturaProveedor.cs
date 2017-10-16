using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CEgresosEncabezadoFacturaProveedor
{
    //Constructores

    //Metodos Especiales
    public void AgregarEgresosEncabezadoFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_EgresosEncabezadoFacturaProveedor_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEgresosEncabezadoFacturaProveedor", 0);
        Agregar.StoredProcedure.Parameters["@pIdEgresosEncabezadoFacturaProveedor"].Direction = ParameterDirection.Output;
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEgresos", idEgresos);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idEgresosEncabezadoFacturaProveedor = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdEgresosEncabezadoFacturaProveedor"].Value);
    }
    public decimal TotalAbonosEgresos(int pIdEgresos, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        decimal AbonosEgresos = 0;
        Select.StoredProcedure.CommandText = "sp_Egresos_Abonos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdEgresos", Convert.ToInt32(pIdEgresos));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            AbonosEgresos = Convert.ToDecimal(Select.Registros["AbonosEgresos"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return AbonosEgresos;
    }
    public void EliminarEgresosEncabezadoFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_EgresosEncabezadoFacturaProveedor_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdEgresosEncabezadoFacturaProveedor", idEgresosEncabezadoFacturaProveedor);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}
