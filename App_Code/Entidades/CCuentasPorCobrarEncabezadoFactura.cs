using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CCuentasPorCobrarEncabezadoFactura
{
    //Constructores

    //Metodos Especiales
    public void AgregarCuentasPorCobrarEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_CuentasPorCobrarEncabezadoFactura_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrarEncabezadoFactura", 0);
        Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrarEncabezadoFactura"].Direction = ParameterDirection.Output;
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", idCuentasPorCobrar);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCuentasPorCobrarEncabezadoFactura = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCuentasPorCobrarEncabezadoFactura"].Value);
    }

    public decimal TotalAbonosCuentasPorCobrar(int pIdCuentasPorCobrar, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        decimal AbonosCuentasPorCobrar = 0;
        Select.StoredProcedure.CommandText = "sp_CuentasPorCobrar_Abonos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrar", Convert.ToInt32(pIdCuentasPorCobrar));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            AbonosCuentasPorCobrar = Convert.ToDecimal(Select.Registros["AbonosCuentasPorCobrar"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return AbonosCuentasPorCobrar;
    }

    public void EliminarCuentasPorCobrarEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_CuentasPorCobrarEncabezadoFactura_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdCuentasPorCobrarEncabezadoFactura", idCuentasPorCobrarEncabezadoFactura);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

    public int ValidaEliminarCuentasPorCobrarDetalle(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int IdFacturaEncabezadoParcial = 0;
        Select.StoredProcedure.CommandText = "sp_CuentasPorCobrarEncabezadoFactura_PuedeEliminar";
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