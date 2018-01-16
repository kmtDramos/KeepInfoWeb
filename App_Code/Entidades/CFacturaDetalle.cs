using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CFacturaDetalle
{

    private string idsConceptosProyectos;
    private int sinIVA;
    private int idTipoMoneda;
    private decimal tipoCambio;
    private decimal porcentajeDescuento;
    private int idDescuentoCliente;

    public string IdsConceptosProyectos
    {
        get { return idsConceptosProyectos; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            idsConceptosProyectos = value;
        }
    }

    public int SinIVA
    {
        get { return sinIVA; }
        set
        {
            if (value < 0)
            {
                return;
            }
            sinIVA = value;
        }
    }

    public int IdTipoMoneda
    {
        get { return idTipoMoneda; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idTipoMoneda = value;
        }
    }

    public decimal TipoCambio
    {
        get { return tipoCambio; }
        set
        {
            if (value < 0)
            {
                return;
            }
            tipoCambio = value;
        }
    }

    public decimal PorcentajeDescuento
    {
        get { return porcentajeDescuento; }
        set
        {
            if (value < 0)
            {
                return;
            }
            porcentajeDescuento = value;
        }
    }

    public int IdDescuentoCliente
    {
        get { return idDescuentoCliente; }
        set
        {
            if (value < 0)
            {
                return;
            }
            idDescuentoCliente = value;
        }
    }
    //Constructores

    //Metodos Especiales
    public void AgregarFacturaDetalle(Dictionary<string, object> pFacturaDetalle, int pIdFacturaEncabezado, int pIdCotizacionDetalle, CConexion pConexion)
    {
        CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CFacturaDetalle FacturaDetalle = new CFacturaDetalle();
        CotizacionDetalle.LlenaObjeto(pIdCotizacionDetalle, pConexion);
        FacturaEncabezado.LlenaObjeto(pIdFacturaEncabezado, pConexion);

        FacturaDetalle.Baja = false;
        //FacturaDetalle.Cantidad = Convert.ToInt32(pFacturaDetalle["Cantidad"]);
    }

    public void AgregarDetalleFactura(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_FacturaDetalle_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", 0);
        Agregar.StoredProcedure.Parameters["@pIdFacturaDetalle"].Direction = ParameterDirection.Output;
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClave", clave);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescripcion", descripcion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidad", cantidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPrecioUnitario", precioUnitario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDescuento", descuento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProducto", idProducto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdServicio", idServicio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaEncabezado", idFacturaEncabezado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCosto", costo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacionDetalle", idCotizacionDetalle);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSinDocumentacion", sinDocumentacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", idConceptoProyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEncabezadoRemision", idEncabezadoRemision);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDetalleRemision", idDetalleRemision);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoIVA", idTipoIVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdsConceptosProyectos", idsConceptosProyectos);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSinIVA", sinIVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDescuentoCliente", idDescuentoCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pPorcentajeDescuento", porcentajeDescuento);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClaveProdServ", ClaveProdServ);
        Agregar.Insert(pConexion);
        idFacturaDetalle = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdFacturaDetalle"].Value);
    }

    public int ValidaEliminarFacturaDetalle(int pIdFacturaDetalle, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int puedeEliminar = 0;
        Select.StoredProcedure.CommandText = "sp_FacturaDetalle_PuedeEliminar";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", pIdFacturaDetalle);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            puedeEliminar = Convert.ToInt32(Select.Registros["puedeEliminar"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return puedeEliminar;
    }

    public int ExisteIngresoAsociadoFactura(int pIdFacturaDetalle, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int existeIngresoAsociadoFactura = 0;
        Select.StoredProcedure.CommandText = "sp_FacturaDetalle_Consultar_ExisteIngresoAsociadoFactura";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", pIdFacturaDetalle);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            existeIngresoAsociadoFactura = Convert.ToInt32(Select.Registros["existeIngresoAsociadoFactura"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return existeIngresoAsociadoFactura;
    }

    public void EliminarFacturaDetalle(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_FacturaDetalle_Eliminar";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pIdFacturaDetalle", IdFacturaDetalle);
        Eliminar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Eliminar.Delete(pConexion);
    }

    public static int ObtenerIdFacturaEncabezado(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CFacturaDetalle Detalle = new CFacturaDetalle();
        Detalle.LlenaObjetoFiltros(pParametros, pConexion);
        return Detalle.idFacturaEncabezado;
    }
}