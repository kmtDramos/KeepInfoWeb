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



public partial class CAsignacionCliente
{
    //Constructores

    //Metodos Especiales
    public JArray ObtenerJsonCliente(string pCliente, CConexion pConexion)
    {

        CAsignacionCliente AsignacionCliente = new CAsignacionCliente();
        JArray JClientes = new JArray();

        foreach (CAsignacionCliente oAsignacion in AsignacionCliente.ObtenerClientes(pCliente, pConexion))
        {
            JObject JAsignacion = new JObject();
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(oAsignacion.IdCliente, pConexion);
            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

            JAsignacion.Add("IdCliente", oAsignacion.IdCliente);
            JAsignacion.Add("Cliente", Organizacion.NombreComercial);
            JClientes.Add(JAsignacion);
        }
        return JClientes;
    }

    public JObject ObtenerJsonClientePorId(int pIdCliente, int pIdUsuario, CConexion pConexion)
    {
        JObject JCliente = new JObject();
        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(pIdCliente, pConexion);
        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        GrupoEmpresarial.LlenaObjeto(Organizacion.IdGrupoEmpresarial, pConexion);
        CTipoIndustria TipoIndustria = new CTipoIndustria();
        TipoIndustria.LlenaObjeto(Organizacion.IdTipoIndustria, pConexion);

        JCliente.Add("IdUsuario", pIdUsuario);
        JCliente.Add("IdCliente", Cliente.IdCliente);
        JCliente.Add("RazonSocial", Organizacion.RazonSocial);
        JCliente.Add("GrupoEmpresarial", GrupoEmpresarial.GrupoEmpresarial);
        JCliente.Add("NombreComercial", Organizacion.NombreComercial);
        JCliente.Add("RFC", Organizacion.RFC);
        JCliente.Add("Notas", Organizacion.Notas);
        JCliente.Add("Dominio", Organizacion.Dominio);
        JCliente.Add("TipoIndustria", TipoIndustria.TipoIndustria);

        return JCliente;
    }

    private List<object> ObtenerClientes(string pCliente, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_FiltroClientes";
        Obten.StoredProcedure.Parameters.AddWithValue("@pCliente", pCliente);
        Obten.Llena<CAsignacionCliente>(typeof(CAsignacionCliente), pConexion);
        return Obten.ListaRegistros;
    }

    public bool VerificaAsignacionCliente(CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        ObtenObjeto.StoredProcedure.CommandText = "sp_VerificaAsignacionCliente";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", IdUsuario);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCliente", IdCliente);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pExiste", 0);
        ObtenObjeto.Llena<CAsignacionCliente>(typeof(CAsignacionCliente), pConexion);
        foreach (CAsignacionCliente CA in ObtenObjeto.ListaRegistros)
        {
            Existe = CA.Existe;
        }
        if (Existe == null || Existe == false)
        { return false; }
        else
        { return true; }
    }
}