using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class Almacen : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //GridAlmacen
        CJQGrid GridAlmacen = new CJQGrid();
        GridAlmacen.NombreTabla = "grdAlmacen";
        GridAlmacen.CampoIdentificador = "IdAlmacen";
        GridAlmacen.ColumnaOrdenacion = "Almacen";
        GridAlmacen.Metodo = "ObtenerAlmacen";
        GridAlmacen.TituloTabla = "Catálogo de almacenes";

        //IdAlmacen
        CJQColumn ColIdAlmacen = new CJQColumn();
        ColIdAlmacen.Nombre = "IdAlmacen";
        ColIdAlmacen.Oculto = "true";
        ColIdAlmacen.Encabezado = "IdAlmacen";
        ColIdAlmacen.Buscador = "false";
        GridAlmacen.Columnas.Add(ColIdAlmacen);

        //Almacen
        CJQColumn ColAlmacen = new CJQColumn();
        ColAlmacen.Nombre = "Almacen";
        ColAlmacen.Encabezado = "Almacen";
        ColAlmacen.Ancho = "200";
        ColAlmacen.Alineacion = "left";
        GridAlmacen.Columnas.Add(ColAlmacen);

        //Empresa
        CJQColumn ColEmpresa = new CJQColumn();
        ColEmpresa.Nombre = "Empresa";
        ColEmpresa.Encabezado = "Empresa";
        ColEmpresa.Ancho = "200";
        ColEmpresa.Buscador = "false";
        ColEmpresa.Alineacion = "left";
        GridAlmacen.Columnas.Add(ColEmpresa);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridAlmacen.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarAlmacen";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridAlmacen.Columnas.Add(ColConsultar);

        //SucursalAsignada
        CJQColumn ColSucursalAsignada = new CJQColumn();
        ColSucursalAsignada.Nombre = "SucursalAsignada";
        ColSucursalAsignada.Encabezado = "Sucursal";
        ColSucursalAsignada.Etiquetado = "Imagen";
        ColSucursalAsignada.Imagen = "refrescar.png";
        ColSucursalAsignada.Estilo = "divImagenConsultar imgFormaSucursalAsignada";
        ColSucursalAsignada.Buscador = "false";
        ColSucursalAsignada.Ordenable = "false";
        ColSucursalAsignada.Ancho = "50";
        GridAlmacen.Columnas.Add(ColSucursalAsignada);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAlmacen", GridAlmacen.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAlmacen(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pAlmacen, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAlmacen", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pAlmacen", SqlDbType.VarChar, 250).Value = pAlmacen;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarAlmacen(string pAlmacen)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonAlmacen = new CJson();
        jsonAlmacen.StoredProcedure.CommandText = "sp_Almacen_Consultar_FiltroPorAlmacen";
        jsonAlmacen.StoredProcedure.Parameters.AddWithValue("@pAlmacen", pAlmacen);
        string jsonAlmacenString = jsonAlmacen.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonAlmacenString;
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdAlmacen, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CAlmacen Almacen = new CAlmacen();
            Almacen.IdAlmacen = pIdAlmacen;
            Almacen.Baja = pBaja;
            Almacen.Eliminar(ConexionBaseDatos);
            respuesta = "0|AlmacenEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatos();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerFormaAgregarAlmacen()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            CEmpresa Empresa = new CEmpresa();
            JArray JEmpresas = new JArray();
            foreach (CEmpresa oEmpresa in Empresa.LlenaObjetos(ConexionBaseDatos))
            {
                JObject JEmpresa = new JObject();
                JEmpresa.Add(new JProperty("IdEmpresa", oEmpresa.IdEmpresa));
                JEmpresa.Add(new JProperty("Empresa", oEmpresa.Empresa));
                JEmpresas.Add(JEmpresa);
            }

            Modelo.Add(new JProperty("Empresas", JEmpresas));

            CTipoAlmacen TipoAlmacen = new CTipoAlmacen();
            JArray JTiposAlmacen = new JArray();
            foreach (CTipoAlmacen oTipoAlmacen in TipoAlmacen.LlenaObjetos(ConexionBaseDatos))
            {
                JObject JTipoAlmacen = new JObject();
                JTipoAlmacen.Add(new JProperty("IdTipoAlmacen", oTipoAlmacen.IdTipoAlmacen));
                JTipoAlmacen.Add(new JProperty("TipoAlmacen", oTipoAlmacen.TipoAlmacen));
                JTiposAlmacen.Add(JTipoAlmacen);
            }

            Modelo.Add("TiposAlmacen", JTiposAlmacen);
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(ConexionBaseDatos));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarAlmacen(int pIdAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarAlmacen = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarAlmacen" }, ConexionBaseDatos) == "")
        {
            puedeEditarAlmacen = 1;
        }
        oPermisos.Add("puedeEditarAlmacen", puedeEditarAlmacen);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAlmacen Almacen = new CAlmacen();
            Almacen.LlenaObjeto(pIdAlmacen, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAlmacen", Almacen.IdAlmacen));
            Modelo.Add(new JProperty("Almacen", Almacen.Almacen));
            Modelo.Add(new JProperty("Telefono", Almacen.Telefono));
            Modelo.Add(new JProperty("Correo", Almacen.Correo));

            CTipoAlmacen TipoAlmacen = new CTipoAlmacen();
            TipoAlmacen.LlenaObjeto(Almacen.IdTipoAlmacen, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoAlmacen", TipoAlmacen.TipoAlmacen));

            if (Almacen.DisponibleVenta)
            {
                Modelo.Add(new JProperty("DispobibleVenta", "Si"));
            }
            else
            {
                Modelo.Add(new JProperty("DispobibleVenta", "No"));
            }

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Almacen.IdEmpresa, ConexionBaseDatos);
            Modelo.Add(new JProperty("Empresa", Empresa.Empresa));
            Modelo.Add(new JProperty("Calle", Almacen.Calle));
            Modelo.Add(new JProperty("NumeroExterior", Almacen.NumeroExterior));
            Modelo.Add(new JProperty("NumeroInterior", Almacen.NumeroInterior));
            Modelo.Add(new JProperty("Colonia", Almacen.Colonia));
            Modelo.Add(new JProperty("CodigoPostal", Almacen.CodigoPostal));

            CPais Pais = new CPais();
            Pais.LlenaObjeto(Almacen.IdPais, ConexionBaseDatos);
            Modelo.Add(new JProperty("Pais", Pais.Pais));


            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Almacen.IdEstado, ConexionBaseDatos);
            Modelo.Add(new JProperty("Estado", Estado.Estado));

            CMunicipio Municipio = new CMunicipio();
            Municipio.LlenaObjeto(Almacen.IdMunicipio, ConexionBaseDatos);
            Modelo.Add(new JProperty("Municipio", Municipio.Municipio));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarAlmacen(int pIdAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarAlmacen = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarAlmacen" }, ConexionBaseDatos) == "")
        {
            puedeEditarAlmacen = 1;
        }
        oPermisos.Add("puedeEditarAlmacen", puedeEditarAlmacen);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAlmacen Almacen = new CAlmacen();
            Almacen.LlenaObjeto(pIdAlmacen, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAlmacen", Almacen.IdAlmacen));
            Modelo.Add(new JProperty("Almacen", Almacen.Almacen));
            Modelo.Add(new JProperty("Telefono", Almacen.Telefono));
            Modelo.Add(new JProperty("Correo", Almacen.Correo));

            CEmpresa Empresa = new CEmpresa();
            Empresa.LlenaObjeto(Almacen.IdEmpresa, ConexionBaseDatos);

            Modelo.Add(new JProperty("Calle", Almacen.Calle));
            Modelo.Add(new JProperty("NumeroExterior", Almacen.NumeroExterior));
            Modelo.Add(new JProperty("NumeroInterior", Almacen.NumeroInterior));
            Modelo.Add(new JProperty("Colonia", Almacen.Colonia));
            Modelo.Add(new JProperty("CodigoPostal", Almacen.CodigoPostal));
            Modelo.Add(new JProperty("Empresa", Empresa.Empresa));

            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Almacen.IdEstado, Almacen.IdMunicipio, ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Almacen.IdPais, Almacen.IdEstado, ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Almacen.IdPais, ConexionBaseDatos));

            JArray JEmpresas = new JArray();
            foreach (CEmpresa oEmpresa in Empresa.LlenaObjetos(ConexionBaseDatos))
            {
                JObject JEmpresa = new JObject();
                JEmpresa.Add(new JProperty("IdEmpresa", oEmpresa.IdEmpresa));
                JEmpresa.Add(new JProperty("Empresa", oEmpresa.Empresa));
                if (Empresa.IdEmpresa == oEmpresa.IdEmpresa)
                {
                    JEmpresa.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JEmpresa.Add(new JProperty("Selected", 0));
                }
                JEmpresas.Add(JEmpresa);
            }
            Modelo.Add(new JProperty("Empresas", JEmpresas));

            JArray JTiposAlmacen = new JArray();
            CTipoAlmacen TipoAlmacen = new CTipoAlmacen();
            TipoAlmacen.LlenaObjeto(Almacen.IdTipoAlmacen, ConexionBaseDatos);
            foreach (CTipoAlmacen oTipoAlmacen in TipoAlmacen.LlenaObjetos(ConexionBaseDatos))
            {
                JObject JTipoAlmacen = new JObject();
                JTipoAlmacen.Add(new JProperty("IdTipoAlmacen", oTipoAlmacen.IdTipoAlmacen));
                JTipoAlmacen.Add(new JProperty("TipoAlmacen", oTipoAlmacen.TipoAlmacen));

                if (TipoAlmacen.IdTipoAlmacen == oTipoAlmacen.IdTipoAlmacen)
                {
                    JTipoAlmacen.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JTipoAlmacen.Add(new JProperty("Selected", 0));
                }
                JTiposAlmacen.Add(JTipoAlmacen);
            }
            Modelo.Add(new JProperty("TiposAlmacen", JTiposAlmacen));
            Modelo.Add(new JProperty("DisponibleVenta", Almacen.DisponibleVenta));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaSucursalAsignada(int pIdAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();
        CAlmacen Almacen = new CAlmacen();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("IdAlmacen", pIdAlmacen);
            Modelo.Add("SucursalesDispobiles", Almacen.ObtenerJsonSucursalesDisponibles(pIdAlmacen, ConexionBaseDatos));
            Modelo.Add("SucursalesAsignadas", Almacen.ObtenerJsonSucursalesAsignadas(pIdAlmacen, ConexionBaseDatos));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaEstados(int pIdPais)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonEstados(pIdPais, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaMunicipios(int pIdEstado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonMunicipios(pIdEstado, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string AgregarAlmacen(Dictionary<string, object> pAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAlmacen Almacen = new CAlmacen();
        Almacen.Almacen = Convert.ToString(pAlmacen["Almacen"]);
        Almacen.IdEmpresa = Convert.ToInt32(pAlmacen["IdEmpresa"]);
        Almacen.Telefono = Convert.ToString(pAlmacen["Telefono"]);
        Almacen.Correo = Convert.ToString(pAlmacen["Correo"]);
        Almacen.Calle = Convert.ToString(pAlmacen["Calle"]);
        Almacen.NumeroExterior = Convert.ToString(pAlmacen["NumeroExterior"]);
        Almacen.NumeroInterior = Convert.ToString(pAlmacen["NumeroInterior"]);
        Almacen.IdPais = Convert.ToInt32(pAlmacen["IdPais"]);
        Almacen.IdEstado = Convert.ToInt32(pAlmacen["IdEstado"]);
        Almacen.Colonia = Convert.ToString(pAlmacen["Colonia"]);
        Almacen.IdMunicipio = Convert.ToInt32(pAlmacen["IdMunicipio"]);
        Almacen.CodigoPostal = Convert.ToString(pAlmacen["CodigoPostal"]);
        Almacen.IdTipoAlmacen = Convert.ToInt32(pAlmacen["IdTipoAlmacen"]);
        Almacen.DisponibleVenta = Convert.ToBoolean(pAlmacen["DisponibleVenta"]);

        string validacion = ValidarAlmacen(Almacen, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Almacen.Agregar(ConexionBaseDatos);
            Almacen.LlenaObjeto(Almacen.IdAlmacen, ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Almacen.IdAlmacen;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se inserto el almacen " + Almacen.Almacen;
            HistorialGenerico.AgregarHistorialGenerico("Almacen", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarAlmacen(Dictionary<string, object> pAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAlmacen Almacen = new CAlmacen();
        Almacen.IdAlmacen = Convert.ToInt32(pAlmacen["IdAlmacen"]);
        Almacen.Almacen = Convert.ToString(pAlmacen["Almacen"]);
        Almacen.IdEmpresa = Convert.ToInt32(pAlmacen["IdEmpresa"]);
        Almacen.Telefono = Convert.ToString(pAlmacen["Telefono"]);
        Almacen.Correo = Convert.ToString(pAlmacen["Correo"]);
        Almacen.Calle = Convert.ToString(pAlmacen["Calle"]);
        Almacen.NumeroExterior = Convert.ToString(pAlmacen["NumeroExterior"]);
        Almacen.NumeroInterior = Convert.ToString(pAlmacen["NumeroInterior"]);
        Almacen.IdPais = Convert.ToInt32(pAlmacen["IdPais"]);
        Almacen.IdEstado = Convert.ToInt32(pAlmacen["IdEstado"]);
        Almacen.Colonia = Convert.ToString(pAlmacen["Colonia"]);
        Almacen.IdMunicipio = Convert.ToInt32(pAlmacen["IdMunicipio"]);
        Almacen.CodigoPostal = Convert.ToString(pAlmacen["CodigoPostal"]);
        Almacen.IdTipoAlmacen = Convert.ToInt32(pAlmacen["IdTipoAlmacen"]);
        Almacen.DisponibleVenta = Convert.ToBoolean(pAlmacen["DisponibleVenta"]);

        string validacion = ValidarAlmacen(Almacen, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Almacen.Editar(ConexionBaseDatos);

            Almacen.LlenaObjeto(Almacen.IdAlmacen, ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Almacen.IdAlmacen;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico el almacen " + Almacen.Almacen;
            HistorialGenerico.AgregarHistorialGenerico("Almacen", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string AgregarSucursalAlmacen(Dictionary<string, object> pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            try
            {
                JObject Modelo = new JObject();
                CSucursalAccesoAlmacen SucursalAccesoAlmacen = new CSucursalAccesoAlmacen();
                SucursalAccesoAlmacen.IdAlmacen = Convert.ToInt32(pSucursal["IdAlmacen"]);
                SucursalAccesoAlmacen.BajaSucursalAlmacen(ConexionBaseDatos);

                foreach (Dictionary<string, object> oSucursal in (Array)pSucursal["Sucursales"])
                {
                    SucursalAccesoAlmacen.IdSucursal = Convert.ToInt32(oSucursal["IdSucursal"]);
                    SucursalAccesoAlmacen.EnrolarSucursalAlmacen(ConexionBaseDatos);
                }

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            catch (Exception ex)
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", ex.Message));
            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }

        return oRespuesta.ToString();
    }

    public static string ValidarAlmacen(CAlmacen pAlmacen, CConexion pConexion)
    {
        string errores = "";

        if (pAlmacen.Almacen == "")
        { errores = errores + "<span>*</span> El nombre del almacen esta vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.IdEmpresa == 0)
        { errores = errores + "<span>*</span> No se indicó la empresa, favor de seleccionarla.<br />"; }

        if (pAlmacen.Telefono == "")
        { errores = errores + "<span>*</span> El teléfono está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.Correo == "")
        { errores = errores + "<span>*</span> El correo está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.Calle == "")
        { errores = errores + "<span>*</span> La calle está vacía, favor de capturarla.<br />"; }

        if (pAlmacen.NumeroExterior == "")
        { errores = errores + "<span>*</span> El número exterior está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.NumeroInterior == "")
        { errores = errores + "<span>*</span> El número interior está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.Colonia == "")
        { errores = errores + "<span>*</span> La colonia está vacía, favor de capturarla.<br />"; }

        if (pAlmacen.CodigoPostal == "")
        { errores = errores + "<span>*</span> El código postal está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.IdPais == 0)
        { errores = errores + "<span>*</span> El país está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.IdEstado == 0)
        { errores = errores + "<span>*</span> El estado está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.IdMunicipio == 0)
        { errores = errores + "<span>*</span> El municipio está vacío, favor de capturarlo.<br />"; }

        if (pAlmacen.IdTipoAlmacen == 0)
        { errores = errores + "<span>*</span> El tipo de almacen está vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}