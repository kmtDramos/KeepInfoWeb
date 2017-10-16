using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CAsientoContableDetalle
{
    //Constructores

    //Metodos Especiales
    public void AgregarAsientosDetalleFacturaProveedor(int pIdFacturaProveedor, int pIdAsientoContable, CConexion pConexion)
    {
        CConsultaAccion AgregarAsiento = new CConsultaAccion();
        AgregarAsiento.StoredProcedure.CommandText = "sp_AsientoContableDetalle_Agregar_AgregarAsientosDetalleFacturaProveedor";
        AgregarAsiento.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", pIdFacturaProveedor);
        AgregarAsiento.StoredProcedure.Parameters.AddWithValue("@pIdAsientoContable", pIdAsientoContable);
        AgregarAsiento.Insert(pConexion);
    }
}