using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CSolicitudFacturacion
{
    //Constructores

    //Metodos Especiales
    public List<Object> ObtenerSolicitudFiltroIdProyecto(int pIdProyecto, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Solicitud_FiltroPorProyecto";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", pIdProyecto);
        Obten.Llena<CSolicitudFacturacion>(typeof(CSolicitudFacturacion), pConexion);
        return Obten.ListaRegistros;
    }

    public int ObtenerMaximoOrdenFactura(int pIdProyecto, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int MaximoOrdenFactura = 0;
        Select.StoredProcedure.CommandText = "sp_Solicitud_FiltroPorProyecto";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", pIdProyecto);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            MaximoOrdenFactura = Convert.ToInt32(Select.Registros["MaximoOrdenFactura"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return MaximoOrdenFactura;
    }

    public void EditarOrdenamiento(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_SolicitudFacturacion_EditarOrdenamiento";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", idSolicitudFacturacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenFactura", ordenFactura);
        Editar.Update(pConexion);
    }

    public decimal ObtenerTotalSolicitudFactura(CConexion pConexion)
    {
        decimal totalSolicitudFactura = 0;
        CSelectEspecifico Select = new CSelectEspecifico();

        Select.StoredProcedure.CommandText = "sp_SolicitudFactura_Consultar_ObtenerTotalSolicitudFactura";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSolicitudFacturacion", idSolicitudFacturacion);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            totalSolicitudFactura = Convert.ToInt32(Select.Registros["TotalSolicitudFactura"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();

        return totalSolicitudFactura;
    }

    public static string ObtenerEstatusSolicitudesProyecto(int pIdProyecto, CConexion pConexion)
    {
        CSolicitudFacturacion Solicitudes = new CSolicitudFacturacion();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("IdProyecto", pIdProyecto);
        string estatus = "Sin facturar";
        foreach (CSolicitudFacturacion Solicitud in Solicitudes.LlenaObjetosFiltros(pParametros, pConexion))
        {
            CFacturaEncabezado Factura = new CFacturaEncabezado();
            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdSolicitudFacturacion", Solicitud.IdSolicitudFacturacion);
            int idFactura = CConceptoProyecto.ObtenerIdFacturaEncabezadoRelacionado(Parametros, pConexion);
            Factura.LlenaObjeto(idFactura, pConexion);
            if(Factura.IdEstatusFacturaEncabezado != 0){
                CEstatusFacturaEncabezado Estatus = new CEstatusFacturaEncabezado();
                Estatus.LlenaObjeto(Factura.IdEstatusFacturaEncabezado, pConexion);
                estatus = Estatus.EstatusFacturaEncabezado;
            }
        }
        return estatus;
    }

}