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


public partial class CProyecto
{
    //Constructores
    public SqlCommand StoredProcedure = new SqlCommand();

    //Metodos Especiales
    public static JObject ObtenerProyecto(JObject pModelo, int pIdProyecto, CConexion pConexion)
    {
        CProyecto Proyecto = new CProyecto();
        Proyecto.LlenaObjeto(pIdProyecto, pConexion);
        pModelo.Add("IdProyecto", Proyecto.IdProyecto);
        pModelo.Add("IdCliente", Proyecto.IdCliente);
        pModelo.Add("NombreProyecto", Proyecto.NombreProyecto);

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(Proyecto.IdCliente, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

        pModelo.Add("RazonSocial", Organizacion.RazonSocial);
        pModelo.Add("FechaInicio", Proyecto.FechaInicio.ToShortDateString());
        pModelo.Add("FechaTermino", Proyecto.FechaTermino.ToShortDateString());
        pModelo.Add("CostoTeorico", Proyecto.CostoTeorico);
        pModelo.Add("PrecioTeorico", Proyecto.PrecioTeorico);

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Proyecto.IdUsuarioResponsable, pConexion);

        pModelo.Add("Responsable", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
        pModelo.Add("IdUsuario", Proyecto.IdUsuarioResponsable);

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Proyecto.IdTipoMoneda, pConexion);
        pModelo.Add("IdTipoMoneda", TipoMoneda.IdTipoMoneda);
        pModelo.Add("Moneda", TipoMoneda.TipoMoneda);

        CEstatusProyecto Estatus = new CEstatusProyecto();
        Estatus.LlenaObjeto(Proyecto.IdEstatusProyecto, pConexion);
        pModelo.Add("Estatus", Estatus.Estatus);

        COportunidad Oportunidad = new COportunidad();
        Oportunidad.LlenaObjeto(Proyecto.IdOportunidad, pConexion);
        pModelo.Add("IdOportunidad", Oportunidad.IdOportunidad);
        pModelo.Add("Oportunidad", Oportunidad.Oportunidad);

        CNivelInteresCotizacion NivelInteres = new CNivelInteresCotizacion();
        NivelInteres.LlenaObjeto(Proyecto.IdNivelInteres, pConexion);
        pModelo.Add("IdNivelInteres", NivelInteres.IdNivelInteresCotizacion);
        pModelo.Add("NivelInteres", NivelInteres.NivelInteresCotizacion);
        
        CDivision Division = new CDivision();
        Division.LlenaObjeto(Proyecto.IdDivision, pConexion);
        pModelo.Add("Division", Division.Division);

        pModelo.Add("EstatusFactura", CProyecto.ObtenerEstatusSolicitudesProyecto(Proyecto.IdProyecto, pConexion));
        
        
        var progreso = 0;
        var transcurridos = ((DateTime.Now - Proyecto.FechaInicio).Days);
        var periodoTotal = ((Proyecto.FechaTermino - Proyecto.FechaInicio).Days);
        if (periodoTotal == 0)
        {
            periodoTotal = 1;
        }
        progreso = Convert.ToInt32((transcurridos * 100) / periodoTotal);
        if (Proyecto.FechaInicio > DateTime.Now)
        {
            progreso = 0;
        }
        pModelo.Add("Progreso", Convert.ToDecimal(progreso));
        pModelo.Add("TipoCambio", Proyecto.TipoCambio);
        pModelo.Add("Notas", Proyecto.Notas);

        return pModelo;
    }

    public string ObtenerJsonNombreProyecto(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public static string ObtenerEstatusSolicitudesProyecto(int pIdProyecto, CConexion pConexion)
    {
        string estatus = CSolicitudFacturacion.ObtenerEstatusSolicitudesProyecto(pIdProyecto, pConexion);
        return estatus;
    }

    public static decimal ObtenerCostoRealProyecto(int pIdProyecto, int pIdTipoMoneda,CConexion pConexion)
    {
        decimal CostoReal = 0;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_ObtenerCostoRealProyecto";
        Select.StoredProcedure.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
        Select.StoredProcedure.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;

        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            CostoReal = Convert.ToDecimal(Select.Registros["CostoReal"]);
        }

        Select.CerrarConsulta();

        return CostoReal;
    }

    public static decimal ObtenerFacturadoProyecto(int pIdProyecto, int pIdTipoMoneda, CConexion pConexion)
    {
        decimal Facturado = 0;


        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_ObtenerFacturadoProyecto";
        Select.StoredProcedure.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
        Select.StoredProcedure.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;

        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            Facturado = Convert.ToDecimal(Select.Registros["Facturado"]);
        }

        Select.CerrarConsulta();

        return Facturado;
    }

    public static decimal ObtenerProgramadoProyecto(int pIdProyecto, int pIdTipoMoneda, CConexion pConexion)
    {
        decimal Programado = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_ObtenerProgramadoProyecto";
        Select.StoredProcedure.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
        Select.StoredProcedure.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;

        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            Programado = Convert.ToDecimal(Select.Registros["Programado"]);
        }

        Select.CerrarConsulta();

        return Programado;
    }

    public static decimal ObtenerCobradoProyecto(int pIdProyecto, int pIdTipoMoneda, CConexion pConexion)
    {
        decimal Programado = 0;

        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_ObtenerCobradoProyecto";
        Select.StoredProcedure.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
        Select.StoredProcedure.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;

        Select.Llena(pConexion);

        if (Select.Registros.Read())
        {
            Programado = Convert.ToDecimal(Select.Registros["Cobrado"]);
        }

        Select.CerrarConsulta();

        return Programado;
    }

    public static void ActualizarTotales(int pIdProyecto, CConexion pConexion)
    {
        if (pIdProyecto != 0 && pIdProyecto != null)
        {
            CConsultaAccion Editar = new CConsultaAccion();
            Editar.StoredProcedure.CommandText = "sp_Proyecto_Actualizar";
            Editar.StoredProcedure.CommandType = CommandType.StoredProcedure;
            Editar.StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
            Editar.StoredProcedure.Parameters.AddWithValue("pIdProyecto", pIdProyecto);
            Editar.Update(pConexion);
        }
    }

}