using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CNotaCreditoEncabezadoFactura
{
    //Constructores

    //Metodos Especiales
    public void AgregarNotaCreditoEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_NotaCreditoEncabezadoFactura_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoEncabezadoFactura", 0);
        Agregar.StoredProcedure.Parameters["@pIdNotaCreditoEncabezadoFactura"].Direction = ParameterDirection.Output;
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idNotaCreditoEncabezadoFactura = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdNotaCreditoEncabezadoFactura"].Value);
    }

    public decimal TotalAbonosNotaCredito(int pIdNotaCredito, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        decimal AbonosNotaCredito = 0;
        Select.StoredProcedure.CommandText = "sp_NotaCredito_Abonos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", Convert.ToInt32(pIdNotaCredito));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            AbonosNotaCredito = Convert.ToDecimal(Select.Registros["AbonosNotaCredito"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return AbonosNotaCredito;
    }

    public void EliminarNotaCreditoEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_NotaCreditoEncabezadoFactura_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoEncabezadoFactura", idNotaCreditoEncabezadoFactura);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

    public int ValidaEliminarCuentasPorCobrarDetalle(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int IdFacturaEncabezadoParcial = 0;
        Select.StoredProcedure.CommandText = "sp_NotaCreditoEncabezadoFactura_PuedeEliminar";
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
