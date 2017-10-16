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


public partial class COrganizacion
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Constructores

    //Metodos Especiales
    public bool ExisteGrupoEmpresarial(string pNombreComercial, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteGrupoEmpresarial = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_GrupoEmpresarial_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pGrupoEmpresarial", Convert.ToString(pNombreComercial));
        ObtenObjeto.Llena<CGrupoEmpresarial>(typeof(CGrupoEmpresarial), pConexion);
        foreach (CGrupoEmpresarial GrupoEmpresarial in ObtenObjeto.ListaRegistros)
        {
            ExisteGrupoEmpresarial = 1;
        }
        if (ExisteGrupoEmpresarial == 0)
        { return false; }
        else
        { return true; }
    }

    public string ObtenerJsonRazonSocial(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public string ObtenerJsonRFC(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public static JObject ObtenerJsonOrganizacion(JObject pModelo, int pIdOrganizacion, CConexion pConexion)
    {

        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        Organizacion.LlenaObjeto(pIdOrganizacion, pConexion);

        Dictionary<string, object> ParametrosD = new Dictionary<string, object>();
        ParametrosD.Add("IdTipoDireccion", 1);
        ParametrosD.Add("IdOrganizacion", Organizacion.IdOrganizacion);
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosD, pConexion);
        //Organizacion
        pModelo.Add("RazonSocial", Organizacion.RazonSocial);
        pModelo.Add("NombreComercial", Organizacion.NombreComercial);
        pModelo.Add("RFC", Organizacion.RFC);
        pModelo.Add("Notas", Organizacion.Notas);
        pModelo.Add("Dominio", Organizacion.Dominio);
        pModelo.Add("TipoIndustrias", CTipoIndustria.ObtenerJsonTipoIndustria(Organizacion.IdTipoIndustria, pConexion));
        pModelo.Add("GrupoEmpresariales", CGrupoEmpresarial.ObtenerJsonGrupoEmpresariales(Organizacion.IdGrupoEmpresarial, pConexion));
        pModelo.Add("SegmentoMercados", CSegmentoMercado.ObtenerJsonSegmentoMercado(Organizacion.IdSegmentoMercado, pConexion));

        //Direccion de la Organizacion
        pModelo.Add("Calle", DireccionOrganizacion.Calle);
        pModelo.Add("NumeroExterior", DireccionOrganizacion.NumeroExterior);
        pModelo.Add("NumeroInterior", DireccionOrganizacion.NumeroInterior);
        pModelo.Add("Colonia", DireccionOrganizacion.Colonia);
        pModelo.Add("CodigoPostal", DireccionOrganizacion.CodigoPostal);
        pModelo.Add("ConmutadorTelefono", DireccionOrganizacion.ConmutadorTelefono);

        CMunicipio Municipio = new CMunicipio();
        Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);

        CEstado Estado = new CEstado();
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);

        pModelo.Add("Localidades", CLocalidad.ObtenerJsonLocalidades(DireccionOrganizacion.IdMunicipio, DireccionOrganizacion.IdLocalidad, pConexion));
        pModelo.Add("Municipios", CMunicipio.ObtenerJsonMunicipios(Estado.IdEstado, DireccionOrganizacion.IdMunicipio, pConexion));
        pModelo.Add("Estados", CEstado.ObtenerJsonEstados(Estado.IdPais, Estado.IdEstado, pConexion));
        pModelo.Add("Paises", CPais.ObtenerJsonPaises(Estado.IdPais, pConexion));
        pModelo.Add("Referencia", DireccionOrganizacion.Referencia);

        return pModelo;
    }
}
