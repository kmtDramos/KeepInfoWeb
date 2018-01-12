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


public partial class CConceptoProyecto
{
    //Constructores

    //Metodos Especiales
    public List<Object> ObtenerConceptoFiltroIdProyecto(int pIdSolicitud, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_Concepto_FiltroPorProyecto";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdSolicitud", pIdSolicitud);
        Obten.Llena<CConceptoProyecto>(typeof(CConceptoProyecto), pConexion);
        return Obten.ListaRegistros;
    }
    public bool ExistenConceptosEnSolicitud(int pIdSolicitud, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        bool ExistenConceptos = false;
        ObtenObjeto.StoredProcedure.CommandText = "sp_ExistenConceptosSolicitud_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdSolicitud", Convert.ToInt32(pIdSolicitud));
        ObtenObjeto.Llena<CConceptoProyecto>(typeof(CConceptoProyecto), pConexion);
        foreach (CConceptoProyecto ConceptoProyecto in ObtenObjeto.ListaRegistros)
        {
            ExistenConceptos = true;
            break;
        }
        return ExistenConceptos;
    }

    public int MaximoNumeroOrden(int pIdSolicitud, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        int MaximoOrdenConcepto = 0;
        Select.StoredProcedure.CommandText = "sp_Concepto_FiltroPorProyecto";
        Select.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSolicitud", Convert.ToInt32(pIdSolicitud));
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            MaximoOrdenConcepto = Convert.ToInt32(Select.Registros["MaximoOrdenConcepto"]);
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return MaximoOrdenConcepto;
    }

    public void EditarOrdenamiento(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_ConceptoProyecto_EditarOrdenamiento";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdConceptoProyecto", idConceptoProyecto);
        Editar.StoredProcedure.Parameters.AddWithValue("@pOrdenConcepto", ordenConcepto);
        Editar.Update(pConexion);
    }

    public static JObject ObtenerConceptoProyecto(JObject pModelo, int pIdConcepto, CConexion pConexion)
    {
        CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();
        ConceptoProyecto.LlenaObjeto(pIdConcepto, pConexion);
        pModelo.Add("IdProyecto", ConceptoProyecto.IdProyecto);
        pModelo.Add("IdConcepto", ConceptoProyecto.IdConceptoProyecto);
        pModelo.Add("NombreConcepto", ConceptoProyecto.Descripcion);
        pModelo.Add("Monto", ConceptoProyecto.Monto);
        pModelo.Add("Cantidad", ConceptoProyecto.Cantidad);
        pModelo.Add("IdTipoVenta", ConceptoProyecto.IdTipoVenta);
        pModelo.Add("IdTipoMoneda", ConceptoProyecto.IdTipoMoneda);
        pModelo.Add("IdUnidadCompraVenta", ConceptoProyecto.IdUnidadCompraVenta);
        pModelo.Add("ClaveProdServ",ConceptoProyecto.ClaveProdServ);
        return pModelo;
    }

    public static int ObtenerIdFacturaEncabezadoRelacionado(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        CConceptoProyecto Concepto = new CConceptoProyecto();
        Concepto.LlenaObjetoFiltros(pParametros, pConexion);
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdConceptoProyecto", Concepto.IdConceptoProyecto); 
        int idFactura = CFacturaDetalle.ObtenerIdFacturaEncabezado(Parametros, pConexion);
        return idFactura;
    }

}