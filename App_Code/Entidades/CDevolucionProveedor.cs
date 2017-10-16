using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CDevolucionProveedor
{
    //Constructores
    private int cantidad;
    public int Cantidad
    {
        get { return cantidad; }
        set
        {
            if (value < 0)
            {
                return;
            }
            cantidad = value;
        }
    }
    //Metodos Especiales
    public void AgregarDevolucion(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_DevolucionProveedor_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucionProveedor", 0);
        Agregar.StoredProcedure.Parameters["@pIdDevolucionProveedor"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", idNotaCreditoProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleFacturaProveedor", IdDetalleFacturaProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", idUsuarioEntrada);
        if (fechaEntrada.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrada", fechaEntrada);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Agregar.Insert(pConexion);
        idDevolucionProveedor = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDevolucionProveedor"].Value);
    }


    public decimal ObtieneMonto(int pIdNotaCreditoProveedor, CConexion pConexion)
    {
        decimal Monto = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_DevolucionProveedor_ObtieneMonto";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdNotaCreditoProveedor", Convert.ToInt32(pIdNotaCreditoProveedor));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            Monto = Convert.ToDecimal(Select.Registros["Monto"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return Monto;
    }

    public void GenerarDevolucionProveedor(CConexion pConexion)
    {
        CConsultaAccion DevolucionProveedor = new CConsultaAccion();
        DevolucionProveedor.StoredProcedure.CommandText = "sp_DevolucionProveedor_Agregar_GenerarDevolucionProveedor";
        DevolucionProveedor.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", IdUsuarioEntrada);
        DevolucionProveedor.StoredProcedure.Parameters.AddWithValue("@pIdDevolucionProveedor", IdDevolucionProveedor);
        DevolucionProveedor.Update(pConexion);

    }

}
