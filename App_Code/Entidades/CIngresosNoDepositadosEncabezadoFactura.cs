using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CIngresosNoDepositadosEncabezadoFactura
{
    //Constructores

    //Metodos Especiales
    public void AgregarIngresosNoDepositadosEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_IngresosNoDepositadosEncabezadoFactura_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositadosEncabezadoFactura", 0);
        Agregar.StoredProcedure.Parameters["@pIdIngresosNoDepositadosEncabezadoFactura"].Direction = ParameterDirection.Output;
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", idIngresosNoDepositados);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idIngresosNoDepositadosEncabezadoFactura = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdIngresosNoDepositadosEncabezadoFactura"].Value);
    }
    public decimal TotalAbonosIngresosNoDepositados(int pIdIngresosNoDepositados, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        decimal AbonosIngresosNoDepositados = 0;
        Select.StoredProcedure.CommandText = "sp_IngresosNoDepositados_Abonos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositados", Convert.ToInt32(pIdIngresosNoDepositados));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            AbonosIngresosNoDepositados = Convert.ToDecimal(Select.Registros["AbonosIngresosNoDepositados"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return AbonosIngresosNoDepositados;
    }
    public void EliminarIngresosNoDepositadosEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_IngresosNoDepositadosEncabezadoFactura_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdIngresosNoDepositadosEncabezadoFactura", idIngresosNoDepositadosEncabezadoFactura);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

    public int ValidaEliminarCuentasPorCobrarDetalle(int pIdFacturaEncabezado, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int IdFacturaEncabezadoParcial = 0;
        Select.StoredProcedure.CommandText = "sp_IngresosNoDepositadosEncabezadoFactura_PuedeEliminar";
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
