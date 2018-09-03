using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public class CJson
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Metodos
    public string ObtenerJsonString(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        string jsonObject = JsonConvert.SerializeObject(dataSet);
        return jsonObject;
    }

    public JToken ObtenerJsonJObject(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);

        JObject oJson = JObject.Parse(JsonConvert.SerializeObject(dataSet));
        return oJson["Table"];
    }

    public static JArray ObtenerListaAlmacenDestino(int pIdUsuarioSession, CConexion pConexion)
    {
        CAlmacen Almacen = new CAlmacen();
        JArray JAlmacenes = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdUsuarioSesion", pIdUsuarioSession);
        Parametros.Add("Opcion", 1);
        foreach (CAlmacen oAlmacen in Almacen.LlenaObjetosAlmacenAsignadoUsuario(Parametros, pConexion))
        {
            JObject JAlmacen = new JObject();
            JAlmacen.Add("Valor", oAlmacen.IdAlmacen);
            JAlmacen.Add("Descripcion", oAlmacen.Almacen);
            JAlmacenes.Add(JAlmacen);
        }
        return JAlmacenes;
    }

    public static JArray ObtenerJsonEstados(int pIdPais, CConexion pConexion)
    {
        CEstado Estado = new CEstado();
        JArray JEstados = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdPais", pIdPais);
        Parametros.Add("Baja", 0);
        foreach (CEstado oEstado in Estado.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JEstado = new JObject();
            JEstado.Add("Valor", oEstado.IdEstado);
            JEstado.Add("Descripcion", oEstado.Estado);
            JEstados.Add(JEstado);
        }
        return JEstados;
    }

    public static JArray ObtenerJsonEstados(int pIdPais, int pIdEstado, CConexion pConexion)
    {
        CEstado Estado = new CEstado();
        JArray JEstados = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdPais", pIdPais);
        Parametros.Add("Baja", 0);
        foreach (CEstado oEstado in Estado.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JEstado = new JObject();
            JEstado.Add("Valor", oEstado.IdEstado);
            JEstado.Add("Descripcion", oEstado.Estado);
            if (oEstado.IdEstado == pIdEstado)
            {
                JEstado.Add(new JProperty("Selected", 1));
            }
            else
            {
                JEstado.Add(new JProperty("Selected", 0));
            }
            JEstados.Add(JEstado);
        }
        return JEstados;
    }

    public static JArray ObtenerJsonPaises(CConexion pConexion)
    {
        CPais Pais = new CPais();
        JArray JPaises = new JArray();
        foreach (CPais oPais in Pais.LlenaObjetos(pConexion))
        {
            JObject JPais = new JObject();
            JPais.Add("Valor", oPais.IdPais);
            JPais.Add("Descripcion", oPais.Pais);
            JPaises.Add(JPais);
        }
        return JPaises;
    }

    public static JArray ObtenerJsonPaises(int pIdPais, CConexion pConexion)
    {
        CPais Pais = new CPais();
        JArray JPaises = new JArray();
        foreach (CPais oPais in Pais.LlenaObjetos(pConexion))
        {
            JObject JPais = new JObject();
            JPais.Add("Valor", oPais.IdPais);
            JPais.Add("Descripcion", oPais.Pais);
            if (oPais.IdPais == pIdPais)
            {
                JPais.Add(new JProperty("Selected", 1));
            }
            else
            {
                JPais.Add(new JProperty("Selected", 0));
            }
            JPaises.Add(JPais);
        }
        return JPaises;
    }

    public static JObject ObtenerJsonEmpresa(JObject pModelo, int pIdEmpresa, CConexion pConexion)
    {
        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(pIdEmpresa, pConexion);
        pModelo.Add("IdEmpresa", Empresa.IdEmpresa);
        pModelo.Add("RazonSocial", Empresa.RazonSocial);
        pModelo.Add("Empresa", Empresa.Empresa);
        pModelo.Add("RFC", Empresa.RFC);
        pModelo.Add("Telefono", Empresa.Telefono);
        pModelo.Add("Correo", Empresa.Correo);
        pModelo.Add("Calle", Empresa.Calle);
        pModelo.Add("NumeroExterior", Empresa.NumeroExterior);
        pModelo.Add("NumeroInterior", Empresa.NumeroInterior);
        pModelo.Add("Colonia", Empresa.Colonia);
        pModelo.Add("CodigoPostal", Empresa.CodigoPostal);
        pModelo.Add("RegimenFiscal", Empresa.RegimenFiscal);
        pModelo.Add("Dominio", Empresa.Dominio);
        pModelo.Add("Referencia", Empresa.Referencia);
        pModelo.Add("ReferenciaEtiqueta", Empresa.Referencia.Replace("" + (char)13, "<br />"));
        pModelo.Add("Logo", Empresa.Logo);

        CLocalidad Localidad = new CLocalidad();
        Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
        pModelo.Add("IdLocalidad", Localidad.IdLocalidad);
        pModelo.Add("Localidad", Localidad.Localidad);

        CMunicipio Municipio = new CMunicipio();
        Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
        pModelo.Add("IdMunicipio", Municipio.IdMunicipio);
        pModelo.Add("Municipio", Municipio.Municipio);

        CEstado Estado = new CEstado();
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        pModelo.Add("IdEstado", Estado.IdEstado);
        pModelo.Add("Estado", Estado.Estado);

        CPais Pais = new CPais();
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        pModelo.Add("IdPais", Pais.IdPais);
        pModelo.Add("Pais", Pais.Pais);

        return pModelo;
    }

    public static JArray ObtenerJsonTipoIndustria(CConexion pConexion)
    {
        CTipoIndustria TipoIndustria = new CTipoIndustria();
        JArray JTipoIndustrias = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoIndustria oTipoIndustria in TipoIndustria.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoIndustria = new JObject();
            JTipoIndustria.Add("IdTipoIndustria", oTipoIndustria.IdTipoIndustria);
            JTipoIndustria.Add("TipoIndustria", oTipoIndustria.TipoIndustria);
            JTipoIndustrias.Add(JTipoIndustria);
        }
        return JTipoIndustrias;
    }

    public static JArray ObtenerJsonCondicionPago(CConexion pConexion)
    {
        CCondicionPago CondicionPago = new CCondicionPago();
        JArray JCondicionPagos = new JArray();
        Dictionary<string, object> ParametrosCP = new Dictionary<string, object>();
        ParametrosCP.Add("Baja", 0);
        foreach (CCondicionPago oCondicionPago in CondicionPago.LlenaObjetosFiltros(ParametrosCP, pConexion))
        {
            JObject JCondicionPago = new JObject();
            JCondicionPago.Add("IdCondicionPago", oCondicionPago.IdCondicionPago);
            JCondicionPago.Add("CondicionPago", oCondicionPago.CondicionPago);
            JCondicionPagos.Add(JCondicionPago);
        }
        return JCondicionPagos;
    }

    public static JArray ObtenerJsonTipoMoneda(CConexion pConexion)
    {
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        JArray JTipoMonedas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoMoneda oTipoMoneda in TipoMoneda.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoMoneda = new JObject();
            JTipoMoneda.Add("IdTipoMoneda", oTipoMoneda.IdTipoMoneda);
            JTipoMoneda.Add("TipoMoneda", oTipoMoneda.TipoMoneda);
            JTipoMonedas.Add(JTipoMoneda);
        }
        return JTipoMonedas;
    }
    
    public static JArray ObtenerJsonFormaContactos(CConexion pConexion)
    {
        CFormaContacto FormaContacto = new CFormaContacto();
        JArray JFormaContactos = new JArray();
        foreach (CFormaContacto oFormaContacto in FormaContacto.LlenaObjetos(pConexion))
        {
            JObject JFormaContacto = new JObject();
            JFormaContacto.Add("IdFormaContacto", oFormaContacto.IdFormaContacto);
            JFormaContacto.Add("FormaContacto", oFormaContacto.FormaContacto);
            JFormaContactos.Add(JFormaContacto);
        }
        return JFormaContactos;
    }

    public static JArray ObtenerJsonTipoClientes(CConexion pConexion)
    {
        CTipoCliente TipoCliente = new CTipoCliente();
        JArray JTipoClientes = new JArray();
        foreach (CTipoCliente oTipoCliente in TipoCliente.LlenaObjetos(pConexion))
        {
            JObject JTipoCliente = new JObject();
            JTipoCliente.Add("IdTipoCliente", oTipoCliente.IdTipoCliente);
            JTipoCliente.Add("TipoCliente", oTipoCliente.TipoCliente);
            JTipoClientes.Add(JTipoCliente);
        }
        return JTipoClientes;
    }

    public static JArray ObtenerJsonGrupoEmpresariales(CConexion pConexion)
    {
        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        JArray JGrupoEmpresarials = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CGrupoEmpresarial oGrupoEmpresarial in GrupoEmpresarial.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JGrupoEmpresarial = new JObject();
            JGrupoEmpresarial.Add("IdGrupoEmpresarial", oGrupoEmpresarial.IdGrupoEmpresarial);
            JGrupoEmpresarial.Add("GrupoEmpresarial", oGrupoEmpresarial.GrupoEmpresarial);
            JGrupoEmpresarials.Add(JGrupoEmpresarial);
        }
        return JGrupoEmpresarials;
    }

    public static JObject ObtenerJsonProveedor(JObject pModelo, int pIdProveedor, CConexion pConexion)
    {
        CProveedor Proveedor = new CProveedor();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
        CUsuario Usuario = new CUsuario();

        Proveedor.LlenaObjeto(pIdProveedor, pConexion);
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);//Obtengo los datos de la organizacion

        Usuario.LlenaObjeto(Organizacion.IdUsuarioAlta, pConexion);//Con los datos de la organizacion, obtengo los datos del usuario que lo dio de alta para posteriormente obtener la sucurslal
        pModelo.Add("IdUsuarioSucursalDioDeAlta", Usuario.IdSucursalActual);//Sucursal a la que pertenece ese usuario que dio de alta la organizacion




        Dictionary<string, object> ParametrosD = new Dictionary<string, object>();
        ParametrosD.Add("IdTipoDireccion", 1);
        ParametrosD.Add("IdOrganizacion", Proveedor.IdOrganizacion);
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosD, pConexion);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Organizacion.IdEmpresaAlta, pConexion);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

        pModelo.Add("SucursalAlta", Sucursal.Sucursal + " (" + Empresa.Empresa + ")");
        pModelo.Add("IdEmpresaAlta", Empresa.IdEmpresa);

        pModelo.Add("IdProveedor", Proveedor.IdProveedor);
        pModelo.Add("IVAActual", Proveedor.IVAActual.ToString());
        pModelo.Add("CuentaContable", Proveedor.CuentaContable);
        pModelo.Add("CuentaContableDolares", Proveedor.CuentaContableDolares);
        pModelo.Add("RazonSocial", Organizacion.RazonSocial);
        pModelo.Add("NombreComercial", Organizacion.NombreComercial);
        pModelo.Add("RFC", Organizacion.RFC);
        pModelo.Add("Notas", Organizacion.Notas);
        pModelo.Add("Dominio", Organizacion.Dominio);
        pModelo.Add("LimiteCredito", Proveedor.LimiteCredito);
        pModelo.Add("Correo", Proveedor.Correo);

        CTipoIndustria TipoIndustria = new CTipoIndustria();
        TipoIndustria.LlenaObjeto(Organizacion.IdTipoIndustria, pConexion);
        pModelo.Add("IdTipoIndustria", TipoIndustria.IdTipoIndustria);
        pModelo.Add("TipoIndustria", TipoIndustria.TipoIndustria);

        pModelo.Add("Calle", DireccionOrganizacion.Calle);
        pModelo.Add("NumeroExterior", DireccionOrganizacion.NumeroExterior);
        pModelo.Add("NumeroInterior", DireccionOrganizacion.NumeroInterior);
        pModelo.Add("Colonia", DireccionOrganizacion.Colonia);
        pModelo.Add("CodigoPostal", DireccionOrganizacion.CodigoPostal);
        pModelo.Add("ConmutadorTelefono", DireccionOrganizacion.ConmutadorTelefono);

        CLocalidad Localidad = new CLocalidad();
        Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
        pModelo.Add("IdLocalidad", Localidad.IdLocalidad);
        pModelo.Add("Localidad", Localidad.Localidad);

        CMunicipio Municipio = new CMunicipio();
        Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
        pModelo.Add("IdMunicipio", Municipio.IdMunicipio);
        pModelo.Add("Municipio", Municipio.Municipio);

        CEstado Estado = new CEstado();
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        pModelo.Add("IdEstado", Estado.IdEstado);
        pModelo.Add("Estado", Estado.Estado);

        CPais Pais = new CPais();
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        pModelo.Add("IdPais", Pais.IdPais);
        pModelo.Add("Pais", Pais.Pais);
        pModelo.Add("Referencia", DireccionOrganizacion.Referencia);

        CCondicionPago CondicionPago = new CCondicionPago();
        CondicionPago.LlenaObjeto(Proveedor.IdCondicionPago, pConexion);
        pModelo.Add("IdCondicionPago", CondicionPago.IdCondicionPago);
        pModelo.Add("CondicionPago", CondicionPago.CondicionPago);

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Proveedor.IdTipoMoneda, pConexion);
        pModelo.Add("IdTipoMoneda", TipoMoneda.IdTipoMoneda);
        pModelo.Add("TipoMoneda", TipoMoneda.TipoMoneda);

        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        GrupoEmpresarial.LlenaObjeto(Organizacion.IdGrupoEmpresarial, pConexion);
        pModelo.Add("IdGrupoEmpresarial", GrupoEmpresarial.IdGrupoEmpresarial);
        pModelo.Add("GrupoEmpresarial", GrupoEmpresarial.GrupoEmpresarial);

        CTipoGarantia TipoGarantia = new CTipoGarantia();
        TipoGarantia.LlenaObjeto(Proveedor.IdTipoGarantia, pConexion);
        pModelo.Add("IdTipoGarantia", TipoGarantia.IdTipoGarantia);
        pModelo.Add("TipoGarantia", TipoGarantia.TipoGarantia);

        return pModelo;
    }

    public static JArray ObtenerJsonTipoIndustria(int pIdTipoIndustria, CConexion pConexion)
    {

        CTipoIndustria TipoIndustria = new CTipoIndustria();
        JArray JTipoIndustrias = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoIndustria oTipoIndustria in TipoIndustria.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoIndustria = new JObject();
            JTipoIndustria.Add("IdTipoIndustria", oTipoIndustria.IdTipoIndustria);
            JTipoIndustria.Add("TipoIndustria", oTipoIndustria.TipoIndustria);
            if (oTipoIndustria.IdTipoIndustria == pIdTipoIndustria)
            {
                JTipoIndustria.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoIndustria.Add(new JProperty("Selected", 0));
            }
            JTipoIndustrias.Add(JTipoIndustria);
        }
        return JTipoIndustrias;
    }

    public static JArray ObtenerJsonCondicionPago(int pIdCondicionPago, CConexion pConexion)
    {
        CCondicionPago CondicionPago = new CCondicionPago();
        JArray JCondicionPagos = new JArray();
        Dictionary<string, object> ParametrosCP = new Dictionary<string, object>();
        ParametrosCP.Add("Baja", 0);
        foreach (CCondicionPago oCondicionPago in CondicionPago.LlenaObjetosFiltros(ParametrosCP, pConexion))
        {
            JObject JCondicionPago = new JObject();
            JCondicionPago.Add("IdCondicionPago", oCondicionPago.IdCondicionPago);
            JCondicionPago.Add("CondicionPago", oCondicionPago.CondicionPago);
            if (oCondicionPago.IdCondicionPago == pIdCondicionPago)
            {
                JCondicionPago.Add(new JProperty("Selected", 1));
            }
            else
            {
                JCondicionPago.Add(new JProperty("Selected", 0));
            }
            JCondicionPagos.Add(JCondicionPago);
        }
        return JCondicionPagos;
    }
    
    public static JArray ObtenerJsonTipoMoneda(int pIdTipoMoneda, CConexion pConexion)
    {
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        JArray JTipoMonedas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoMoneda oTipoMoneda in TipoMoneda.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoMoneda = new JObject();
            JTipoMoneda.Add("IdTipoMoneda", oTipoMoneda.IdTipoMoneda);
            JTipoMoneda.Add("TipoMoneda", oTipoMoneda.TipoMoneda);
            if (oTipoMoneda.IdTipoMoneda == pIdTipoMoneda)
            {
                JTipoMoneda.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoMoneda.Add(new JProperty("Selected", 0));
            }
            JTipoMonedas.Add(JTipoMoneda);
        }
        return JTipoMonedas;
    }

    public static JArray ObtenerJsonGrupoEmpresariales(int pIdGrupoEmpresarial, CConexion pConexion)
    {
        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        JArray JGrupoEmpresarials = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CGrupoEmpresarial oGrupoEmpresarial in GrupoEmpresarial.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JGrupoEmpresarial = new JObject();
            JGrupoEmpresarial.Add("IdGrupoEmpresarial", oGrupoEmpresarial.IdGrupoEmpresarial);
            JGrupoEmpresarial.Add("GrupoEmpresarial", oGrupoEmpresarial.GrupoEmpresarial);
            if (oGrupoEmpresarial.IdGrupoEmpresarial == pIdGrupoEmpresarial)
            {
                JGrupoEmpresarial.Add(new JProperty("Selected", 1));
            }
            else
            {
                JGrupoEmpresarial.Add(new JProperty("Selected", 0));
            }
            JGrupoEmpresarials.Add(JGrupoEmpresarial);
        }
        return JGrupoEmpresarials;
    }

    public static JArray ObtenerJsonTipoDireccion(int pIdTipoDireccion, CConexion pConexion)
    {

        CTipoDireccion TipoDireccion = new CTipoDireccion();
        JArray JTipoDirecciones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoDireccion oTipoDireccion in TipoDireccion.LlenaObjetosFiltrosConDireccionFiscal(ParametrosTI, pConexion))
        {
            JObject JTipoDireccion = new JObject();
            JTipoDireccion.Add("IdTipoDireccion", oTipoDireccion.IdTipoDireccion);
            JTipoDireccion.Add("TipoDireccion", oTipoDireccion.TipoDireccion);
            if (oTipoDireccion.IdTipoDireccion == pIdTipoDireccion)
            {
                JTipoDireccion.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoDireccion.Add(new JProperty("Selected", 0));
            }
            JTipoDirecciones.Add(JTipoDireccion);
        }
        return JTipoDirecciones;
    }

    public static JArray ObtenerJsonTipoDireccion(CConexion pConexion)
    {

        CTipoDireccion TipoDireccion = new CTipoDireccion();
        JArray JTipoDirecciones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoDireccion oTipoDireccion in TipoDireccion.LlenaObjetosFiltrosConDireccionFiscal(ParametrosTI, pConexion))
        {
            JObject JTipoDireccion = new JObject();
            JTipoDireccion.Add("IdTipoDireccion", oTipoDireccion.IdTipoDireccion);
            JTipoDireccion.Add("TipoDireccion", oTipoDireccion.TipoDireccion);
            JTipoDirecciones.Add(JTipoDireccion);
        }
        return JTipoDirecciones;
    }

    public static JObject ObtenerJsonDireccionOrganizacion(JObject pModelo, int pIdDireccionOrganizacion, CConexion pConexion)
    {

        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
        DireccionOrganizacion.LlenaObjeto(pIdDireccionOrganizacion, pConexion);
        pModelo.Add("IdDireccionOrganizacion", DireccionOrganizacion.IdDireccionOrganizacion);
        pModelo.Add("Calle", DireccionOrganizacion.Calle);
        pModelo.Add("NumeroExterior", DireccionOrganizacion.NumeroExterior);
        pModelo.Add("NumeroInterior", DireccionOrganizacion.NumeroInterior);
        pModelo.Add("Colonia", DireccionOrganizacion.Colonia);
        pModelo.Add("CodigoPostal", DireccionOrganizacion.CodigoPostal);
        pModelo.Add("ConmutadorTelefono", DireccionOrganizacion.ConmutadorTelefono);
        pModelo.Add("Descripcion", DireccionOrganizacion.Descripcion);

        CTipoDireccion TipoDireccion = new CTipoDireccion();
        TipoDireccion.LlenaObjeto(DireccionOrganizacion.IdTipoDireccion, pConexion);
        pModelo.Add("IdTipoDireccion", TipoDireccion.IdTipoDireccion);
        pModelo.Add("TipoDireccion", TipoDireccion.TipoDireccion);

        CLocalidad Localidad = new CLocalidad();
        Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
        pModelo.Add("IdLocalidad", Localidad.IdLocalidad);
        pModelo.Add("Localidad", Localidad.Localidad);

        CMunicipio Municipio = new CMunicipio();
        Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
        pModelo.Add("IdMunicipio", Municipio.IdMunicipio);
        pModelo.Add("Municipio", Municipio.Municipio);

        CEstado Estado = new CEstado();
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        pModelo.Add("IdEstado", Estado.IdEstado);
        pModelo.Add("Estado", Estado.Estado);

        CPais Pais = new CPais();
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        pModelo.Add("IdPais", Pais.IdPais);
        pModelo.Add("Pais", Pais.Pais);

        pModelo.Add("Referencia", DireccionOrganizacion.Referencia);

        return pModelo;
    }

    public static JArray ObtenerJsonMunicipios(int pIdEstado, CConexion pConexion)
    {
        CMunicipio Municipio = new CMunicipio();
        JArray JMunicipios = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdEstado", pIdEstado);
        Parametros.Add("Baja", 0);
        foreach (CMunicipio oMunicipio in Municipio.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JMunicipio = new JObject();
            JMunicipio.Add("Valor", oMunicipio.IdMunicipio);
            JMunicipio.Add("Descripcion", oMunicipio.Municipio);
            JMunicipios.Add(JMunicipio);
        }
        return JMunicipios;
    }

    public static JArray ObtenerJsonMunicipios(int pIdEstado, int pIdMunicipio, CConexion pConexion)
    {
        CMunicipio Municipio = new CMunicipio();
        JArray JMunicipios = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdEstado", pIdEstado);
        Parametros.Add("Baja", 0);
        foreach (CMunicipio oMunicipio in Municipio.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JMunicipio = new JObject();
            JMunicipio.Add("Valor", oMunicipio.IdMunicipio);
            JMunicipio.Add("Descripcion", oMunicipio.Municipio);
            if (oMunicipio.IdMunicipio == pIdMunicipio)
            {
                JMunicipio.Add(new JProperty("Selected", 1));
            }
            else
            {
                JMunicipio.Add(new JProperty("Selected", 0));
            }
            JMunicipios.Add(JMunicipio);
        }
        return JMunicipios;
    }

    public static JArray ObtenerJsonLocalidades(int pIdMunicipio, CConexion pConexion)
    {
        CLocalidad Localidad = new CLocalidad();
        JArray JLocalidades = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdMunicipio", pIdMunicipio);
        Parametros.Add("Baja", 0);
        foreach (CLocalidad oLocalidad in Localidad.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JLocalidad = new JObject();
            JLocalidad.Add("Valor", oLocalidad.IdLocalidad);
            JLocalidad.Add("Descripcion", oLocalidad.Localidad);
            JLocalidades.Add(JLocalidad);
        }
        return JLocalidades;
    }

    public static JArray ObtenerJsonLocalidades(int pIdMunicipio, int pIdLocalidad, CConexion pConexion)
    {
        CLocalidad Localidad = new CLocalidad();
        JArray JLocalidades = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdMunicipio", pIdMunicipio);
        Parametros.Add("Baja", 0);
        foreach (CLocalidad oLocalidad in Localidad.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JLocalidad = new JObject();
            JLocalidad.Add("Valor", oLocalidad.IdLocalidad);
            JLocalidad.Add("Descripcion", oLocalidad.Localidad);
            if (oLocalidad.IdLocalidad == pIdLocalidad)
            {
                JLocalidad.Add(new JProperty("Selected", 1));
            }
            else
            {
                JLocalidad.Add(new JProperty("Selected", 0));
            }
            JLocalidades.Add(JLocalidad);
        }
        return JLocalidades;
    }

    public static JObject ObtenerJsonContactoOrganizacion(JObject pModelo, int pIdContactoOrganizacion, CConexion pConexion)
    {

        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        ContactoOrganizacion.LlenaObjeto(pIdContactoOrganizacion, pConexion);
        pModelo.Add("IdContactoOrganizacion", ContactoOrganizacion.IdContactoOrganizacion);
        pModelo.Add("Nombre", ContactoOrganizacion.Nombre);
        pModelo.Add("Puesto", ContactoOrganizacion.Puesto);
        pModelo.Add("Notas", ContactoOrganizacion.Notas);
        pModelo.Add("Cumpleanio", ContactoOrganizacion.Cumpleanio.ToShortDateString());

        CTelefonoContactoOrganizacion TelefonoContactoOrganizacion = new CTelefonoContactoOrganizacion();
        TelefonoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pIdContactoOrganizacion);

        JArray Telefonos = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdContactoOrganizacion", pIdContactoOrganizacion);
        Parametros.Add("Baja", false);

        foreach (CTelefonoContactoOrganizacion TCO in TelefonoContactoOrganizacion.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject Telefono = new JObject();
            Telefono.Add(new JProperty("IdTelefonoContactoOrganizacion", TCO.IdTelefonoContactoOrganizacion));
            Telefono.Add(new JProperty("Telefono", TCO.Telefono));
            Telefono.Add(new JProperty("IdContactoOrganizacion", TCO.IdContactoOrganizacion));
            Telefono.Add(new JProperty("Descripcion", TCO.Descripcion));
            Telefonos.Add(Telefono);
        }

        pModelo.Add(new JProperty("Telefonos", Telefonos));

        CCorreoContactoOrganizacion CorreoContactoOrganizacion = new CCorreoContactoOrganizacion();
        CorreoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pIdContactoOrganizacion);
        JArray Correos = new JArray();
        Dictionary<string, object> ParametrosC = new Dictionary<string, object>();
        ParametrosC.Add("IdContactoOrganizacion", pIdContactoOrganizacion);
        ParametrosC.Add("Baja", false);
        foreach (CCorreoContactoOrganizacion CCO in CorreoContactoOrganizacion.LlenaObjetosFiltros(ParametrosC, pConexion))
        {
            JObject Correo = new JObject();
            Correo.Add(new JProperty("IdCorreoContactoOrganizacion", CCO.IdCorreoContactoOrganizacion));
            Correo.Add(new JProperty("Correo", CCO.Correo));
            Correo.Add(new JProperty("IdContactoOrganizacion", CCO.IdContactoOrganizacion));
            Correos.Add(Correo);
        }
        pModelo.Add(new JProperty("Correos", Correos));

        return pModelo;
    }

    public static JArray ObtenerJsonFormaContactos(int pIdFormaContacto, CConexion pConexion)
    {

        CFormaContacto FormaContacto = new CFormaContacto();
        JArray JFormaContactos = new JArray();
        Dictionary<string, object> ParametrosFC = new Dictionary<string, object>();
        ParametrosFC.Add("Baja", 0);
        foreach (CFormaContacto oFormaContacto in FormaContacto.LlenaObjetosFiltros(ParametrosFC, pConexion))
        {
            JObject JFormaContacto = new JObject();
            JFormaContacto.Add("IdFormaContacto", oFormaContacto.IdFormaContacto);
            JFormaContacto.Add("FormaContacto", oFormaContacto.FormaContacto);
            if (oFormaContacto.IdFormaContacto == pIdFormaContacto)
            {
                JFormaContacto.Add(new JProperty("Selected", 1));
            }
            else
            {
                JFormaContacto.Add(new JProperty("Selected", 0));
            }
            JFormaContactos.Add(JFormaContacto);
        }
        return JFormaContactos;
    }

    public static JArray ObtenerJsonTipoClientes(int pIdTipoCliente, CConexion pConexion)
    {

        CTipoCliente TipoCliente = new CTipoCliente();
        JArray JTipoClientes = new JArray();
        Dictionary<string, object> ParametrosFC = new Dictionary<string, object>();
        ParametrosFC.Add("Baja", 0);
        foreach (CTipoCliente oTipoCliente in TipoCliente.LlenaObjetosFiltros(ParametrosFC, pConexion))
        {
            JObject JTipoCliente = new JObject();
            JTipoCliente.Add("IdTipoCliente", oTipoCliente.IdTipoCliente);
            JTipoCliente.Add("TipoCliente", oTipoCliente.TipoCliente);
            if (oTipoCliente.IdTipoCliente == pIdTipoCliente)
            {
                JTipoCliente.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoCliente.Add(new JProperty("Selected", 0));
            }
            JTipoClientes.Add(JTipoCliente);
        }
        return JTipoClientes;
    }

    public static JObject ObtenerJsonCliente(JObject pModelo, int pIdCliente, CConexion pConexion)
    {
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
        CUsuario UsuarioAlta = new CUsuario();
        Cliente.LlenaObjeto(pIdCliente, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        UsuarioAlta.LlenaObjeto(Organizacion.IdUsuarioAlta, pConexion);
        pModelo.Add("IdSucursalAlta", Organizacion.IdEmpresaAlta);

        Dictionary<string, object> ParametrosD = new Dictionary<string, object>();
        ParametrosD.Add("IdTipoDireccion", 1);
        ParametrosD.Add("IdOrganizacion", Cliente.IdOrganizacion);
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosD, pConexion);

        pModelo.Add("IdCliente", Cliente.IdCliente);
        pModelo.Add("RazonSocial", Organizacion.RazonSocial);
        pModelo.Add("NombreComercial", Organizacion.NombreComercial);
        pModelo.Add("RFC", Organizacion.RFC);
        pModelo.Add("Notas", Organizacion.Notas);
        pModelo.Add("Dominio", Organizacion.Dominio);
        pModelo.Add("LimiteDeCredito", Cliente.LimiteDeCredito);
        pModelo.Add("Correo", Cliente.Correo);
        pModelo.Add("IVAActual", Cliente.IVAActual.ToString());
        pModelo.Add("IdUsuarioAlta", Cliente.IdUsuarioAlta);
        pModelo.Add("CuentaContable", Cliente.CuentaContable);
        pModelo.Add("CuentaContableDolares", Cliente.CuentaContableDolares);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Organizacion.IdEmpresaAlta, pConexion);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

        pModelo.Add("SucursalAlta", Sucursal.Sucursal + " (" + Empresa.Empresa + ")");
        pModelo.Add("IdEmpresaAlta", Empresa.IdEmpresa);

        pModelo.Add("IdUsuarioAgente", Cliente.IdUsuarioAgente);

        CUsuario Agente = new CUsuario();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("EsAgente", 1);
        pParametros.Add("Baja", 0);
        pParametros.Add("IdUsuario", Cliente.IdUsuarioAgente);
        Agente.LlenaObjetoFiltros(pParametros, pConexion);
        pModelo.Add("Agente", Agente.Nombre + ' ' + Agente.ApellidoPaterno + ' ' + Agente.ApellidoMaterno);

        CTipoIndustria TipoIndustria = new CTipoIndustria();
        TipoIndustria.LlenaObjeto(Organizacion.IdTipoIndustria, pConexion);
        pModelo.Add("IdTipoIndustria", TipoIndustria.IdTipoIndustria);
        pModelo.Add("TipoIndustria", TipoIndustria.TipoIndustria);

        CCampana Campana = new CCampana();
        Campana.LlenaObjeto(Cliente.IdCampana, pConexion);
        pModelo.Add("IdCampana", Campana.IdCampana);
        pModelo.Add("Campana", Campana.Campana);

        pModelo.Add("Calle", DireccionOrganizacion.Calle);
        pModelo.Add("NumeroExterior", DireccionOrganizacion.NumeroExterior);
        pModelo.Add("NumeroInterior", DireccionOrganizacion.NumeroInterior);
        pModelo.Add("Colonia", DireccionOrganizacion.Colonia);
        pModelo.Add("CodigoPostal", DireccionOrganizacion.CodigoPostal);
        pModelo.Add("ConmutadorTelefono", DireccionOrganizacion.ConmutadorTelefono);

        CLocalidad Localidad = new CLocalidad();
        Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
        pModelo.Add("IdLocalidad", Localidad.IdLocalidad);
        pModelo.Add("Localidad", Localidad.Localidad);

        CMunicipio Municipio = new CMunicipio();
        Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
        pModelo.Add("IdMunicipio", Municipio.IdMunicipio);
        pModelo.Add("Municipio", Municipio.Municipio);

        CEstado Estado = new CEstado();
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        pModelo.Add("IdEstado", Estado.IdEstado);
        pModelo.Add("Estado", Estado.Estado);

        CPais Pais = new CPais();
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        pModelo.Add("IdPais", Pais.IdPais);
        pModelo.Add("Pais", Pais.Pais);

        pModelo.Add("Referencia", DireccionOrganizacion.Referencia);

        CCondicionPago CondicionPago = new CCondicionPago();
        CondicionPago.LlenaObjeto(Cliente.IdCondicionPago, pConexion);
        pModelo.Add("IdCondicionPago", CondicionPago.IdCondicionPago);
        pModelo.Add("CondicionPago", CondicionPago.CondicionPago);

        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        GrupoEmpresarial.LlenaObjeto(Organizacion.IdGrupoEmpresarial, pConexion);
        pModelo.Add("IdGrupoEmpresarial", GrupoEmpresarial.IdGrupoEmpresarial);
        pModelo.Add("GrupoEmpresarial", GrupoEmpresarial.GrupoEmpresarial);

        CTipoCliente TipoCliente = new CTipoCliente();
        TipoCliente.LlenaObjeto(Cliente.IdTipoCliente, pConexion);
        pModelo.Add("IdTipoCliente", TipoCliente.IdTipoCliente);
        pModelo.Add("TipoCliente", TipoCliente.TipoCliente);

        CFormaContacto FormaContacto = new CFormaContacto();
        FormaContacto.LlenaObjeto(Cliente.IdFormaContacto, pConexion);
        pModelo.Add("IdFormaContacto", FormaContacto.IdFormaContacto);
        pModelo.Add("FormaContacto", FormaContacto.FormaContacto);

        CSegmentoMercado SegmentoMercado = new CSegmentoMercado();
        SegmentoMercado.LlenaObjeto(Organizacion.IdSegmentoMercado, pConexion);
        pModelo.Add("SegmentoMercado", SegmentoMercado.SegmentoMercado);

        CTipoGarantia TipoGarantia = new CTipoGarantia();
        TipoGarantia.LlenaObjeto(Cliente.IdTipoGarantia, pConexion);
        pModelo.Add("IdTipoGarantia", TipoGarantia.IdTipoGarantia);
        pModelo.Add("TipoGarantia", TipoGarantia.TipoGarantia);

        return pModelo;
    }

    public static JObject ObtenerJsonTelefonosCorreos(JObject pModelo, int pIdContactoOrganizacion, CConexion pConexion)
    {

        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        ContactoOrganizacion.LlenaObjeto(pIdContactoOrganizacion, pConexion);
        pModelo.Add("IdContactoOrganizacion", ContactoOrganizacion.IdContactoOrganizacion);
        CTelefonoContactoOrganizacion TelefonoContactoOrganizacion = new CTelefonoContactoOrganizacion();
        TelefonoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pIdContactoOrganizacion);
        JArray Telefonos = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdContactoOrganizacion", pIdContactoOrganizacion);
        Parametros.Add("Baja", false);
        foreach (CTelefonoContactoOrganizacion TCO in TelefonoContactoOrganizacion.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject Telefono = new JObject();
            Telefono.Add(new JProperty("IdTelefonoContactoOrganizacion", TCO.IdTelefonoContactoOrganizacion));
            Telefono.Add(new JProperty("Telefono", TCO.Telefono));
            Telefono.Add(new JProperty("IdContactoOrganizacion", TCO.IdContactoOrganizacion));
            Telefono.Add(new JProperty("Descripcion", TCO.Descripcion));
            Telefonos.Add(Telefono);
        }
        pModelo.Add(new JProperty("Telefonos", Telefonos));

        CCorreoContactoOrganizacion CorreoContactoOrganizacion = new CCorreoContactoOrganizacion();
        CorreoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pIdContactoOrganizacion);
        JArray Correos = new JArray();
        Dictionary<string, object> ParametrosC = new Dictionary<string, object>();
        ParametrosC.Add("IdContactoOrganizacion", pIdContactoOrganizacion);
        ParametrosC.Add("Baja", false);
        foreach (CCorreoContactoOrganizacion CCO in CorreoContactoOrganizacion.LlenaObjetosFiltros(ParametrosC, pConexion))
        {
            JObject Correo = new JObject();
            Correo.Add(new JProperty("IdCorreoContactoOrganizacion", CCO.IdCorreoContactoOrganizacion));
            Correo.Add(new JProperty("Correo", CCO.Correo));
            Correo.Add(new JProperty("IdContactoOrganizacion", CCO.IdContactoOrganizacion));
            Correos.Add(Correo);
        }
        pModelo.Add(new JProperty("Correos", Correos));

        return pModelo;
    }

    public static JArray ObtenerJsonCondicionPago(bool pGenerico, CConexion pConexion)
    {
        CCondicionPago CondicionPago = new CCondicionPago();
        JArray JCondicionesPago = new JArray();
        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CCondicionPago oCondicionPago in CondicionPago.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCondicionPago = new JObject();
            if (pGenerico)
            {
                JCondicionPago.Add("Valor", oCondicionPago.IdCondicionPago);
                JCondicionPago.Add("Descripcion", oCondicionPago.CondicionPago);
            }
            JCondicionesPago.Add(JCondicionPago);
        }
        return JCondicionesPago;
    }

    public static JArray ObtenerJsonTipoIndustria(bool pGenerico, CConexion pConexion)
    {
        CTipoIndustria TipoIndustria = new CTipoIndustria();
        JArray JTipoIndustrias = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoIndustria oTipoIndustria in TipoIndustria.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoIndustria = new JObject();
            if (pGenerico)
            {
                JTipoIndustria.Add("Valor", oTipoIndustria.IdTipoIndustria);
                JTipoIndustria.Add("Descripcion", oTipoIndustria.TipoIndustria);
            }
            JTipoIndustrias.Add(JTipoIndustria);
        }
        return JTipoIndustrias;
    }
    
    public static JArray ObtenerJsonGrupoEmpresariales(bool pGenerico, CConexion pConexion)
    {
        CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
        JArray JGrupoEmpresarials = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CGrupoEmpresarial oGrupoEmpresarial in GrupoEmpresarial.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JGrupoEmpresarial = new JObject();
            if (pGenerico)
            {
                JGrupoEmpresarial.Add("IdGrupoEmpresarial", oGrupoEmpresarial.IdGrupoEmpresarial);
                JGrupoEmpresarial.Add("GrupoEmpresarial", oGrupoEmpresarial.GrupoEmpresarial);
            }
            JGrupoEmpresarials.Add(JGrupoEmpresarial);
        }
        return JGrupoEmpresarials;
    }

    public static JArray ObtenerJsonTipoServicio(bool pGenerico, CConexion pConexion)
    {
        CTipoServicio TipoServicio = new CTipoServicio();
        JArray JTipoServicios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoServicio oTipoServicio in TipoServicio.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoServicio = new JObject();
            if (pGenerico)
            {
                JTipoServicio.Add("Valor", oTipoServicio.IdTipoServicio);
                JTipoServicio.Add("Descripcion", oTipoServicio.TipoServicio);
            }
            JTipoServicios.Add(JTipoServicio);
        }
        return JTipoServicios;
    }

    public static JArray ObtenerJsonTipoVenta(bool pGenerico, CConexion pConexion)
    {
        CTipoVenta TipoVenta = new CTipoVenta();
        JArray JTipoVentas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoVenta oTipoVenta in TipoVenta.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoVenta = new JObject();
            if (pGenerico)
            {
                JTipoVenta.Add("Valor", oTipoVenta.IdTipoVenta);
                JTipoVenta.Add("Descripcion", oTipoVenta.TipoVenta);
            }
            JTipoVentas.Add(JTipoVenta);
        }
        return JTipoVentas;
    }
    
    public static JArray ObtenerJsonUnidadCompraVenta(bool pGenerico, CConexion pConexion)
    {
        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        JArray JUnidadCompraVentas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUnidadCompraVenta oUnidadCompraVenta in UnidadCompraVenta.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JUnidadCompraVenta = new JObject();
            if (pGenerico)
            {
                JUnidadCompraVenta.Add("Valor", oUnidadCompraVenta.IdUnidadCompraVenta);
                JUnidadCompraVenta.Add("Descripcion", oUnidadCompraVenta.UnidadCompraVenta);
            }
            JUnidadCompraVentas.Add(JUnidadCompraVenta);
        }
        return JUnidadCompraVentas;
    }
    
    public static JArray ObtenerJsonUsuario(CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        JArray JUsuarios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUsuario oUsuario in Usuario.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("IdUsuario", oUsuario.IdUsuario);
            JUsuario.Add("Usuario", oUsuario.Usuario);
            JUsuarios.Add(JUsuario);
        }
        return JUsuarios;
    }

    public static JArray ObtenerJsonUsuario(int pIdUsuario, CConexion pConexion)
    {
        CUsuario Usuario = new CUsuario();
        JArray JUsuarios = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUsuario oUsuario in Usuario.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("IdUsuario", oUsuario.IdUsuario);
            JUsuario.Add("Usuario", oUsuario.Usuario);
            if (oUsuario.IdUsuario == pIdUsuario)
            {
                JUsuario.Add(new JProperty("Selected", 1));
            }
            else
            {
                JUsuario.Add(new JProperty("Selected", 0));
            }
            JUsuarios.Add(JUsuario);
        }
        return JUsuarios;
    }

    public static JArray ObtenerJsonUnidadCompraVenta(CConexion pConexion)
    {
        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        JArray JUnidadCompraVentas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUnidadCompraVenta oUnidadCompraVenta in UnidadCompraVenta.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JUnidadCompraVenta = new JObject();
            JUnidadCompraVenta.Add("IdUnidadCompraVenta", oUnidadCompraVenta.IdUnidadCompraVenta);
            JUnidadCompraVenta.Add("UnidadCompraVenta", oUnidadCompraVenta.UnidadCompraVenta);
            JUnidadCompraVentas.Add(JUnidadCompraVenta);
        }
        return JUnidadCompraVentas;
    }

    public static JArray ObtenerJsonTipoVenta(CConexion pConexion)
    {
        CTipoVenta TipoVenta = new CTipoVenta();
        JArray JTipoVentas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoVenta oTipoVenta in TipoVenta.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoVenta = new JObject();
            JTipoVenta.Add("IdTipoVenta", oTipoVenta.IdTipoVenta);
            JTipoVenta.Add("TipoVenta", oTipoVenta.TipoVenta);
            JTipoVentas.Add(JTipoVenta);
        }
        return JTipoVentas;
    }
    
    public static JArray ObtenerJsonUnidadCompraVenta(int pIdUnidadCompraVenta, CConexion pConexion)
    {
        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        JArray JUnidadCompraVentas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUnidadCompraVenta oUnidadCompraVenta in UnidadCompraVenta.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JUnidadCompraVenta = new JObject();
            JUnidadCompraVenta.Add("IdUnidadCompraVenta", oUnidadCompraVenta.IdUnidadCompraVenta);
            JUnidadCompraVenta.Add("UnidadCompraVenta", oUnidadCompraVenta.UnidadCompraVenta);
            if (oUnidadCompraVenta.IdUnidadCompraVenta == pIdUnidadCompraVenta)
            {
                JUnidadCompraVenta.Add(new JProperty("Selected", 1));
            }
            else
            {
                JUnidadCompraVenta.Add(new JProperty("Selected", 0));
            }
            JUnidadCompraVentas.Add(JUnidadCompraVenta);
        }
        return JUnidadCompraVentas;
    }

    public static JArray ObtenerJsonTipoVenta(int pIdTipoVenta, CConexion pConexion)
    {
        CTipoVenta TipoVenta = new CTipoVenta();
        JArray JTipoVentas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoVenta oTipoVenta in TipoVenta.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoVenta = new JObject();
            JTipoVenta.Add("IdTipoVenta", oTipoVenta.IdTipoVenta);
            JTipoVenta.Add("TipoVenta", oTipoVenta.TipoVenta);
            if (oTipoVenta.IdTipoVenta == pIdTipoVenta)
            {
                JTipoVenta.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoVenta.Add(new JProperty("Selected", 0));
            }
            JTipoVentas.Add(JTipoVenta);
        }
        return JTipoVentas;
    }

    public static JArray ObtenerJsonMetodoPago(CConexion pConexion)
    {
        CMetodoPago MetodoPago = new CMetodoPago();
        JArray JMetodoPagos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CMetodoPago oMetodoPago in MetodoPago.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JMetodoPago = new JObject();
            JMetodoPago.Add("IdMetodoPago", oMetodoPago.IdMetodoPago);
            JMetodoPago.Add("MetodoPago", oMetodoPago.MetodoPago);
            JMetodoPagos.Add(JMetodoPago);
        }
        return JMetodoPagos;
	}

	public static JArray ObtenerJsonMetodoPago(int pIdMetodoPago, CConexion pConexion)
	{
		CMetodoPago MetodoPago = new CMetodoPago();
		JArray JMetodoPagos = new JArray();
		Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
		ParametrosTI.Add("Baja", 0);
		foreach (CMetodoPago oMetodoPago in MetodoPago.LlenaObjetosFiltros(ParametrosTI, pConexion))
		{
			JObject JMetodoPago = new JObject();
			JMetodoPago.Add("IdMetodoPago", oMetodoPago.IdMetodoPago);
			JMetodoPago.Add("MetodoPago", oMetodoPago.MetodoPago);
			if (oMetodoPago.IdMetodoPago == pIdMetodoPago)
			{
				JMetodoPago.Add(new JProperty("Selected", 1));
			}
			else
			{
				JMetodoPago.Add(new JProperty("Selected", 0));
			}
			JMetodoPagos.Add(JMetodoPago);
		}
		return JMetodoPagos;
	}

	public static JArray ObtenerJsonMetodoPagoBaja(int pIdMetodoPago, CConexion pConexion)
	{
		CMetodoPago MetodoPago = new CMetodoPago();
		JArray JMetodoPagos = new JArray();
		Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
		foreach (CMetodoPago oMetodoPago in MetodoPago.LlenaObjetosFiltros(ParametrosTI, pConexion))
		{
			JObject JMetodoPago = new JObject();
			JMetodoPago.Add("IdMetodoPago", oMetodoPago.IdMetodoPago);
			JMetodoPago.Add("MetodoPago", oMetodoPago.MetodoPago);
			if (oMetodoPago.IdMetodoPago == pIdMetodoPago)
			{
				JMetodoPago.Add(new JProperty("Selected", 1));
			}
			else
			{
				JMetodoPago.Add(new JProperty("Selected", 0));
			}
			JMetodoPagos.Add(JMetodoPago);
		}
		return JMetodoPagos;
	}

	public static JArray ObtenerJsonMetodoPago(CConexion pConexion, int pIdTipoMovimiento)
    {
        CMetodoPago MetodoPago = new CMetodoPago();
        JArray JMetodoPagos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdTipoMovimiento", pIdTipoMovimiento);
        foreach (CMetodoPago oMetodoPago in MetodoPago.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JMetodoPago = new JObject();
            JMetodoPago.Add("IdMetodoPago", oMetodoPago.IdMetodoPago);
            JMetodoPago.Add("MetodoPago", oMetodoPago.MetodoPago);
            JMetodoPagos.Add(JMetodoPago);
        }
        return JMetodoPagos;
    }

    public static JArray ObtenerJsonDivision(CConexion pConexion)
    {
        CDivision Division = new CDivision();
        JArray JDivisiones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CDivision oDivision in Division.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JDivision = new JObject();
            JDivision.Add("IdDivision", oDivision.IdDivision);
            JDivision.Add("Division", oDivision.Division);
            JDivisiones.Add(JDivision);
        }
        return JDivisiones;
    }

    public static JArray ObtenerJsonDivision(int pIdDivision, CConexion pConexion)
    {
        CDivision Division = new CDivision();
        JArray JDivisiones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CDivision oDivision in Division.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JDivision = new JObject();
            JDivision.Add("IdDivision", oDivision.IdDivision);
            JDivision.Add("Division", oDivision.Division);
            if (oDivision.IdDivision == pIdDivision)
            {
                JDivision.Add(new JProperty("Selected", 1));
            }
            else
            {
                JDivision.Add(new JProperty("Selected", 0));
            }
            JDivisiones.Add(JDivision);
        }
        return JDivisiones;
    }

    public static JArray ObtenerJsonTipoCompra(CConexion pConexion)
    {
        CTipoCompra TipoCompra = new CTipoCompra();
        JArray JTipoCompras = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoCompra oTipoCompra in TipoCompra.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoCompra = new JObject();
            JTipoCompra.Add("IdTipoCompra", oTipoCompra.IdTipoCompra);
            JTipoCompra.Add("TipoCompra", oTipoCompra.TipoCompra);
            JTipoCompras.Add(JTipoCompra);
        }
        return JTipoCompras;
    }

    public static JArray ObtenerJsonTipoCompra(int pIdTipoCompra, CConexion pConexion)
    {
        CTipoCompra TipoCompra = new CTipoCompra();
        JArray JTipoCompras = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoCompra oTipoCompra in TipoCompra.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoCompra = new JObject();
            JTipoCompra.Add("IdTipoCompra", oTipoCompra.IdTipoCompra);
            JTipoCompra.Add("TipoCompra", oTipoCompra.TipoCompra);
            if (oTipoCompra.IdTipoCompra == pIdTipoCompra)
            {
                JTipoCompra.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoCompra.Add(new JProperty("Selected", 0));
            }
            JTipoCompras.Add(JTipoCompra);
        }
        return JTipoCompras;
    }

    public static JArray ObtenerJsonTipoCompra(bool pGenerico, CConexion pConexion)
    {
        CTipoCompra TipoCompra = new CTipoCompra();
        JArray JTipoCompras = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CTipoCompra oTipoCompra in TipoCompra.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JTipoCompra = new JObject();
            if (pGenerico)
            {
                JTipoCompra.Add("Valor", oTipoCompra.IdTipoCompra);
                JTipoCompra.Add("Descripcion", oTipoCompra.TipoCompra);
            }
            JTipoCompras.Add(JTipoCompra);
        }
        return JTipoCompras;
    }

    public static JArray ObtenerJsonCuentaContable(int pIdCuentaContable, int pIdSucursal, int pIdDivision, int pIdTipoCompra, CConexion pConexion)
    {
        CCuentaContable CuentaContable = new CCuentaContable();
        JArray JACuentasContables = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdSucursal", pIdSucursal);
        Parametros.Add("IdDivision", pIdDivision);
        Parametros.Add("IdTipoCompra", pIdTipoCompra);
        foreach (CCuentaContable oCuentaContable in CuentaContable.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCuentaContable = new JObject();
            JCuentaContable.Add("Valor", oCuentaContable.IdCuentaContable);
            JCuentaContable.Add("Descripcion", oCuentaContable.Descripcion + " (" + oCuentaContable.CuentaContable + ")");
            JACuentasContables.Add(JCuentaContable);
        }
        return JACuentasContables;
    }

    public static JArray ObtenerJsonIVA(CConexion pConexion)
    {
        CIVA IVA = new CIVA();
        JArray JIVAs = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CIVA oIVA in IVA.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JIVA = new JObject();
            JIVA.Add("IdIVA", oIVA.IdIVA);
            JIVA.Add("IVA", oIVA.IVA);
            JIVAs.Add(JIVA);
        }
        return JIVAs;
    }

    public static JArray ObtenerJsonIVA(int pIdIVA, CConexion pConexion)
    {
        CIVA IVA = new CIVA();
        JArray JIVAs = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CIVA oIVA in IVA.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JIVA = new JObject();
            JIVA.Add("IdIVA", oIVA.IdIVA);
            JIVA.Add("IVA", oIVA.IVA);
            if (oIVA.IdIVA == pIdIVA)
            {
                JIVA.Add(new JProperty("Selected", 1));
            }
            else
            {
                JIVA.Add(new JProperty("Selected", 0));
            }
            JIVAs.Add(JIVA);
        }
        return JIVAs;
    }

    public static JToken obtenerDatosImpresionOrdenCompra(string pIdOrdenCompraEncabezado, int pUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonImpresionOC = new CJson();
        jsonImpresionOC.StoredProcedure.CommandText = "SP_Impresion_OrdenCompra";
        jsonImpresionOC.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionOC.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", pIdOrdenCompraEncabezado);
        jsonImpresionOC.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        return jsonImpresionOC.ObtenerJsonJObject(ConexionBaseDatos);
    }

    public static JArray ObtenerJsonSerieNotaCredito(int IdSucursal, CConexion pConexion)
    {
        CSerieNotaCredito SerieNotaCredito = new CSerieNotaCredito();
        JArray JSerieNotaCreditoes = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdSucursal", IdSucursal);
        foreach (CSerieNotaCredito oSerieNotaCredito in SerieNotaCredito.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JSerieNotaCredito = new JObject();
            JSerieNotaCredito.Add("IdSerieNotaCredito", oSerieNotaCredito.IdSerieNotaCredito);
            JSerieNotaCredito.Add("SerieNotaCredito", oSerieNotaCredito.SerieNotaCredito);
            JSerieNotaCreditoes.Add(JSerieNotaCredito);
        }
        return JSerieNotaCreditoes;
    }

    public static JArray ObtenerJsonCotizacion(int pIdCotizacion, CConexion pConexion)
    {
        CCotizacion Cotizacion = new CCotizacion();
        JArray JCotizaciones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CCotizacion oCotizacion in Cotizacion.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JCotizacion = new JObject();
            JCotizacion.Add("IdCotizacion", oCotizacion.IdCotizacion);
            JCotizacion.Add("Folio", oCotizacion.Folio);
            if (oCotizacion.IdCotizacion == pIdCotizacion)
            {
                JCotizacion.Add(new JProperty("Selected", 1));
            }
            else
            {
                JCotizacion.Add(new JProperty("Selected", 0));
            }
            JCotizaciones.Add(JCotizacion);
        }
        return JCotizaciones;
    }

    public static JArray ObtenerJsonFormaPago(CConexion pConexion)
    {
        CFormaPago FormaPago = new CFormaPago();
        JArray JFormaPagos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CFormaPago oFormaPago in FormaPago.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JFormaPago = new JObject();
            JFormaPago.Add("IdFormaPago", oFormaPago.IdFormaPago);
            JFormaPago.Add("FormaPago", oFormaPago.FormaPago);
            JFormaPagos.Add(JFormaPago);
        }
        return JFormaPagos;
    }

    public static JArray ObtenerJsonFormaPago(int pIdFormaPago, CConexion pConexion)
    {
        CFormaPago FormaPago = new CFormaPago();
        JArray JFormaPagos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CFormaPago oFormaPago in FormaPago.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JFormaPago = new JObject();
            JFormaPago.Add("IdFormaPago", oFormaPago.IdFormaPago);
            JFormaPago.Add("FormaPago", oFormaPago.FormaPago);
            if (oFormaPago.IdFormaPago == pIdFormaPago)
            {
                JFormaPago.Add(new JProperty("Selected", 1));
            }
            else
            {
                JFormaPago.Add(new JProperty("Selected", 0));
            }
            JFormaPagos.Add(JFormaPago);
        }
        return JFormaPagos;
    }

    public static JArray ObtenerJsonCuentaBancariaCliente(int pIdCliente, CConexion pConexion)
    {

        CCuentaBancariaCliente CuentaBancariaCliente = new CCuentaBancariaCliente();
        JArray JCuentaBancariaClientes = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdCliente", pIdCliente);
        foreach (CCuentaBancariaCliente oCuentaBancariaCliente in CuentaBancariaCliente.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject JCuentaBancariaCliente = new JObject();
            JCuentaBancariaCliente.Add("IdCuentaBancariaCliente", oCuentaBancariaCliente.IdCuentaBancariaCliente);
            JCuentaBancariaCliente.Add("CuentaBancariaCliente", oCuentaBancariaCliente.CuentaBancariaCliente);
            JCuentaBancariaClientes.Add(JCuentaBancariaCliente);
        }
        return JCuentaBancariaClientes;
    }

    public static JArray ObtenerJsonSerieFactura(int IdSucursal, CConexion pConexion)
    {
        CSerieFactura SerieFactura = new CSerieFactura();
        JArray JSerieFacturaes = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdSucursal", IdSucursal);
        foreach (CSerieFactura oSerieFactura in SerieFactura.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JSerieFactura = new JObject();
            JSerieFactura.Add("IdSerieFactura", oSerieFactura.IdSerieFactura);
            JSerieFactura.Add("SerieFactura", oSerieFactura.SerieFactura);
            JSerieFacturaes.Add(JSerieFactura);
        }
        return JSerieFacturaes;
    }

    public static JArray ObtenerJsonNumeroCuentaCliente(int pIdNumeroCuenta, int pIdCliente, CConexion pConexion)
    {

        CCuentaBancariaCliente NumeroCuenta = new CCuentaBancariaCliente();
        JArray JNumeroCuentas = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdCliente", pIdCliente);
        foreach (CCuentaBancariaCliente oNumeroCuenta in NumeroCuenta.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JNumeroCuenta = new JObject();
            JNumeroCuenta.Add("IdNumeroCuenta", oNumeroCuenta.IdCuentaBancariaCliente);
            JNumeroCuenta.Add("NumeroCuenta", oNumeroCuenta.CuentaBancariaCliente);
            if (oNumeroCuenta.IdCuentaBancariaCliente == pIdNumeroCuenta)
            {
                JNumeroCuenta.Add(new JProperty("Selected", 1));
            }
            else
            {
                JNumeroCuenta.Add(new JProperty("Selected", 0));
            }
            JNumeroCuentas.Add(JNumeroCuenta);
        }
        return JNumeroCuentas;
    }

    public static JArray ObtenerJsonDireccionOrganizacion(int IdOrganizacion, CConexion pConexion)
    {
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
        JArray JDireccionOrganizaciones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("IdOrganizacion", IdOrganizacion);
        foreach (CDireccionOrganizacion oDireccionOrganizacion in DireccionOrganizacion.LlenaObjetosFiltrosDireccionOrganizacion(ParametrosTI, pConexion))
        {
            JObject JDireccionOrganizacion = new JObject();
            JDireccionOrganizacion.Add("IdDireccionOrganizacion", oDireccionOrganizacion.IdDireccionOrganizacion);
            JDireccionOrganizacion.Add("DireccionOrganizacion", oDireccionOrganizacion.Calle);
            JDireccionOrganizaciones.Add(JDireccionOrganizacion);
        }
        return JDireccionOrganizaciones;
    }

    public static JArray ObtenerJsonSerieFactura(int IdSucursal, int pIdSerieFactura, CConexion pConexion)
    {
        CSerieFactura SerieFactura = new CSerieFactura();
        JArray JSerieFacturaes = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdSucursal", IdSucursal);
        foreach (CSerieFactura oSerieFactura in SerieFactura.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JSerieFactura = new JObject();
            JSerieFactura.Add("IdSerieFactura", oSerieFactura.IdSerieFactura);
            JSerieFactura.Add("SerieFactura", oSerieFactura.SerieFactura);

            if (oSerieFactura.IdSerieFactura == pIdSerieFactura)
            {
                JSerieFactura.Add(new JProperty("Selected", 1));
            }
            else
            {
                JSerieFactura.Add(new JProperty("Selected", 0));
            }

            JSerieFacturaes.Add(JSerieFactura);
        }
        return JSerieFacturaes;
    }

    public static JArray ObtenerJsonUsoCFDI(int pIdUsoCFDI, CConexion pConexion)
    {
        CUsoCFDI usoCFDI = new CUsoCFDI();
        JArray JUsoCFDI = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUsoCFDI oUsoCFDI in usoCFDI.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JAUsoCFDI = new JObject();
            JAUsoCFDI.Add("Valor", oUsoCFDI.IdUsoCFDI);
            JAUsoCFDI.Add("Descripcion", oUsoCFDI.ClaveUsoCFDI + " - " + oUsoCFDI.Descricpion);
            if (oUsoCFDI.IdUsoCFDI == pIdUsoCFDI)
            {
                JAUsoCFDI.Add(new JProperty("Selected", 1));
            }
            else
            {
                JAUsoCFDI.Add(new JProperty("Selected", 0));
            }
            JUsoCFDI.Add(JAUsoCFDI);
        }
        return JUsoCFDI;
    }

    public static JArray ObtenerJsonUsoCFDI(CConexion pConexion)
    {
        CUsoCFDI usoCFDI = new CUsoCFDI();
        JArray JUsoCFDI = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        foreach (CUsoCFDI oUsoCFDI in usoCFDI.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JAUsoCFDI = new JObject();
            JAUsoCFDI.Add("Valor", oUsoCFDI.IdUsoCFDI);
            JAUsoCFDI.Add("Descripcion", oUsoCFDI.ClaveUsoCFDI + " - " + oUsoCFDI.Descricpion);
            
            JUsoCFDI.Add(JAUsoCFDI);
        }
        return JUsoCFDI;
    }

    public static JArray ObtenerJsonFacturasRelacionada(int IdCliente, CConexion pConexion)
    {
        CFacturaEncabezado facturasCliente = new CFacturaEncabezado();
        Dictionary<string, object> ParametrosFacturaCliente = new Dictionary<string, object>();
        ParametrosFacturaCliente.Add("IdCliente", Convert.ToInt32(IdCliente));
        ParametrosFacturaCliente.Add("Anticipo", Convert.ToInt32(1));
        ParametrosFacturaCliente.Add("Baja", false);
        JArray JAFacturaCliente = new JArray();
        foreach (CFacturaEncabezado oFacturasCliente in facturasCliente.LlenaObjetosFiltros(ParametrosFacturaCliente, pConexion))
        {
            JObject JFacturaCliente = new JObject();
            JFacturaCliente.Add("Valor", oFacturasCliente.IdFacturaEncabezado);
            JFacturaCliente.Add("Descripcion", "No. " + oFacturasCliente.NumeroFactura);
           
            JAFacturaCliente.Add(JFacturaCliente);
        }
        return JAFacturaCliente;
    }

    public static JArray ObtenerJsonFacturasRelacionada(int IdCliente, int pIdFacturaRelacionada, CConexion pConexion)
    {
        CFacturaEncabezado facturasCliente = new CFacturaEncabezado();
        Dictionary<string, object> ParametrosFacturaCliente = new Dictionary<string, object>();
        ParametrosFacturaCliente.Add("IdCliente", Convert.ToInt32(IdCliente));
        ParametrosFacturaCliente.Add("Anticipo", Convert.ToInt32(1));
        ParametrosFacturaCliente.Add("Refid", Convert.ToInt32(pIdFacturaRelacionada));
        ParametrosFacturaCliente.Add("Baja", false);
        JArray JAFacturaCliente = new JArray();
        foreach (CFacturaEncabezado oFacturasCliente in facturasCliente.LlenaObjetosFiltros(ParametrosFacturaCliente, pConexion))
        {
            JObject JFacturaCliente = new JObject();
            JFacturaCliente.Add("Valor", oFacturasCliente.IdFacturaEncabezado);
            JFacturaCliente.Add("Descripcion", "No. " + oFacturasCliente.NumeroFactura);
            if (oFacturasCliente.IdFacturaEncabezado == pIdFacturaRelacionada)
            {
                JFacturaCliente.Add(new JProperty("Selected", 1));
            }
            else
            {
                JFacturaCliente.Add(new JProperty("Selected", 0));
            }
            JAFacturaCliente.Add(JFacturaCliente);
        }
        return JAFacturaCliente;
    }

    public static JArray ObtenerJsonTipoRelacion(CConexion pConexion)
    {
        CTipoRelacion TipoRelacion = new CTipoRelacion();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("Baja", 0);
        JArray JATipoRelacion = new JArray();
        foreach (CTipoRelacion oTipoRelacion in TipoRelacion.LlenaObjetosFiltros(pParametros, pConexion))
        {
            JObject JTipoRelacion = new JObject();
            JTipoRelacion.Add("Valor", oTipoRelacion.IdTipoRelacion);
            JTipoRelacion.Add("Descripcion", oTipoRelacion.Clave + " - " + oTipoRelacion.Descripcion);
            
            JATipoRelacion.Add(JTipoRelacion);
        }
        return JATipoRelacion;
    }

    public static JArray ObtenerJsonTipoRelacion(int pIdFacturaRelacionada, int pIdTipoRelacion, CConexion pConexion)
    {
        CTipoRelacion TipoRelacion = new CTipoRelacion();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("Baja", 0);
        JArray JATipoRelacion = new JArray();
        foreach (CTipoRelacion oTipoRelacion in TipoRelacion.LlenaObjetosFiltros(pParametros, pConexion))
        {
            JObject JTipoRelacion = new JObject();
            JTipoRelacion.Add("Valor", oTipoRelacion.IdTipoRelacion);
            JTipoRelacion.Add("Descripcion", oTipoRelacion.Clave + " - " + oTipoRelacion.Descripcion);
            if (oTipoRelacion.IdTipoRelacion == pIdTipoRelacion)
            {
                JTipoRelacion.Add(new JProperty("Selected", 1));
            }
            else
            {
                JTipoRelacion.Add(new JProperty("Selected", 0));
            }
            JATipoRelacion.Add(JTipoRelacion);
        }
        return JATipoRelacion;
    }

    public static JArray ObtenerJsonSeriePago(int IdSucursal, CConexion pConexion)
    {
        CSerieComplementoPago SeriePago = new CSerieComplementoPago();
        JArray JSeriePago = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdSucursal", IdSucursal);
        foreach (CSerieComplementoPago oSeriePago in SeriePago.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JoSeriePago = new JObject();
            JoSeriePago.Add("IdSerieComplementoPago", oSeriePago.IdSerieComplementoPago);
            JoSeriePago.Add("SerieComplementoPago", oSeriePago.SerieComplementoPago);
            JSeriePago.Add(JoSeriePago);
        }
        return JSeriePago;
    }

    public static JArray ObtenerJsonSeriePago(int IdSucursal, int IdSeriePago, CConexion pConexion)
    {
        CSerieComplementoPago SeriePago = new CSerieComplementoPago();
        JArray JSeriePago = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdSucursal", IdSucursal);
        foreach (CSerieComplementoPago oSeriePago in SeriePago.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JoSeriePago = new JObject();
            JoSeriePago.Add("IdSerieComplementoPago", oSeriePago.IdSerieComplementoPago);
            JoSeriePago.Add("SerieComplementoPago", oSeriePago.SerieComplementoPago);
            if (oSeriePago.IdSerieComplementoPago == IdSeriePago)
            {
                JoSeriePago.Add(new JProperty("Selected", 1));
            }
            else
            {
                JoSeriePago.Add(new JProperty("Selected", 0));
            }
            JSeriePago.Add(JoSeriePago);
        }
        return JSeriePago;
    }

}