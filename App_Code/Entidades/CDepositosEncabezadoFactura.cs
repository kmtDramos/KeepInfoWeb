using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class CDepositosEncabezadoFactura
{
    //Constructores

    //Metodos Especiales
    public void AgregarDepositosEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_DepositosEncabezadoFactura_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDepositosEncabezadoFactura", 0);
        Agregar.StoredProcedure.Parameters["@pIdDepositosEncabezadoFactura"].Direction = ParameterDirection.Output;
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFactura", idEncabezadoFactura);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDepositos", idDepositos);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idDepositosEncabezadoFactura = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDepositosEncabezadoFactura"].Value);
    }
    public decimal TotalAbonosDepositos(int pIdDepositos, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        decimal AbonosDepositos = 0;
        Select.StoredProcedure.CommandText = "sp_Depositos_Abonos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdDepositos", Convert.ToInt32(pIdDepositos));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            AbonosDepositos = Convert.ToDecimal(Select.Registros["AbonosDepositos"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return AbonosDepositos;
    }
    public void EliminarDepositosEncabezadoFactura(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_DepositosEncabezadoFactura_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdDepositosEncabezadoFactura", idDepositosEncabezadoFactura);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }
}