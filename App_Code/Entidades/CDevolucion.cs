using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CDevolucion
{
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
    //Constructores

    //Metodos Especiales
    public void AgregarDevolucion(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_Devolucion_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDevolucion", 0);
        Agregar.StoredProcedure.Parameters["@pIdDevolucion"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", idNotaCredito);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", idFacturaDetalle);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", idUsuarioEntrada);
        if (fechaEntrada.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaEntrada", fechaEntrada);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Agregar.Insert(pConexion);
        idDevolucion = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdDevolucion"].Value);
    }

    public decimal ObtieneMonto(int pIdNotaCredito, CConexion pConexion)
    {
        decimal Monto = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Devolucion_ObtieneMonto";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdNotaCredito", Convert.ToInt32(pIdNotaCredito));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            Monto = Convert.ToInt32(Select.Registros["Monto"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return Monto;
    }

    public void GenerarDevolucion(CConexion pConexion)
    {
        CConsultaAccion Devolucion = new CConsultaAccion();
        Devolucion.StoredProcedure.CommandText = "sp_Devolucion_Agregar_GenerarDevolucion";
        Devolucion.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioEntrada", IdUsuarioEntrada);
        Devolucion.StoredProcedure.Parameters.AddWithValue("@pIdDevolucion", IdDevolucion);
        Devolucion.Update(pConexion);

    }

}
