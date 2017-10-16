using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CChequesEncabezadoFacturaProveedor
{
    //Constructores

    //Metodos Especiales
    public void AgregarChequesEncabezadoFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_ChequesEncabezadoFacturaProveedor_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdChequesEncabezadoFacturaProveedor", 0);
        Agregar.StoredProcedure.Parameters["@pIdChequesEncabezadoFacturaProveedor"].Direction = ParameterDirection.Output;
        if (fechaPago.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPago", fechaPago);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pMonto", monto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCheques", idCheques);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idChequesEncabezadoFacturaProveedor = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdChequesEncabezadoFacturaProveedor"].Value);
    }
    public decimal TotalAbonosCheques(int pIdCheques, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        decimal AbonosCheques = 0;
        Select.StoredProcedure.CommandText = "sp_Cheques_Abonos";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdCheques", Convert.ToInt32(pIdCheques));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            AbonosCheques = Convert.ToDecimal(Select.Registros["AbonosCheques"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return AbonosCheques;
    }
    public void EliminarChequesEncabezadoFacturaProveedor(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_ChequesEncabezadoFacturaProveedor_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdChequesEncabezadoFacturaProveedor", idChequesEncabezadoFacturaProveedor);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

}