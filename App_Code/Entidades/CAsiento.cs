using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;


public partial class CAsiento
{
    //Constructores

    //Metodos Especiales
    //public void AgregarAsiento(CConexion pConexion)
    //{
    //    CConsultaAccion Agregar = new CConsultaAccion();
    //    Agregar.StoredProcedure.CommandText = "sp_Asiento_Agregar";
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdAsiento", 0);
    //    Agregar.StoredProcedure.Parameters["@pIdAsiento"].Direction = ParameterDirection.Output;
    //    if (fechaAsiento.Year != 1)
    //    {
    //        Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAsiento", fechaAsiento);
    //    }
    //    if (fechaAutorizado.Year != 1)
    //    {
    //        Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAutorizado", fechaAutorizado);
    //    }
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pAutorizado", autorizado);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pConsumido", consumido);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", cuentaContable);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCuentaContable", idCuentaContable);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoCompra", idTipoCompra);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSubCuentaContable", idSubCuentaContable);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAlta", idUsuarioAlta);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAutorizo", idUsuarioAutorizo);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioModifico", idUsuarioModifico);
    //    Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoFacturaProveedor", idEncabezadoFacturaProveedor);
    //    Agregar.Insert(pConexion);
    //    idAsiento = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdAsiento"].Value);
    //}
}