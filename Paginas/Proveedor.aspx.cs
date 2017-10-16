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

public partial class Proveedor : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    private static int idEmpresa;
    public static int puedeAgregarProveedor = 0;
    public static int puedeEditarProveedor = 0;
    public static int puedeConsultarProveedor = 0;
    public static int puedeAccesarDireccionProveedor = 0;
    public static int puedeAccesarContactosProveedor = 0;
    public static int puedeAgregarLimiteCredito = 0;
    public static int puedeEditarLimiteCredito = 0;
    public static int puedeEditarDatosOrganizacion = 0;
    public static int puedeEditarDatosFiscales = 0;
    public static int puedeEditarDatosLimiteCredito = 0;
    public static int puedeEditarDireccion = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        idSucursal = Usuario.IdSucursalActual;//Esta es la sucursal en la cual actualmente el usuario esta logueado

        if (Usuario.TienePermisos(new string[] { "puedeAgregarProveedor" }, ConexionBaseDatos) == "") { puedeAgregarProveedor = 1; }
        else { puedeAgregarProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeConsultarProveedor" }, ConexionBaseDatos) == "") { puedeConsultarProveedor = 1; }
        else { puedeConsultarProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeAccesarDireccionProveedor" }, ConexionBaseDatos) == "") { puedeAccesarDireccionProveedor = 1; }
        else { puedeAccesarDireccionProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeAccesarContactosProveedor" }, ConexionBaseDatos) == "") { puedeAccesarContactosProveedor = 1; }
        else { puedeAccesarContactosProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeAgregarLimiteCredito" }, ConexionBaseDatos) == "") { puedeAgregarLimiteCredito = 1; }
        else { puedeAgregarLimiteCredito = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarLimiteCredito" }, ConexionBaseDatos) == "") { puedeEditarLimiteCredito = 1; }
        else { puedeEditarLimiteCredito = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarProveedor" }, ConexionBaseDatos) == "") { puedeEditarProveedor = 1; }
        else { puedeEditarProveedor = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarDatosOrganizacion" }, ConexionBaseDatos) == "") { puedeEditarDatosOrganizacion = 1; }
        else { puedeEditarDatosOrganizacion = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarDatosFiscales" }, ConexionBaseDatos) == "") { puedeEditarDatosFiscales = 1; }
        else { puedeEditarDatosFiscales = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarDatosLimiteCredito" }, ConexionBaseDatos) == "") { puedeEditarDatosLimiteCredito = 1; }
        else { puedeEditarDatosLimiteCredito = 0; }




        //GridProveedor
        CJQGrid GridProveedor = new CJQGrid();
        GridProveedor.NombreTabla = "grdProveedor";
        GridProveedor.CampoIdentificador = "IdProveedor";
        GridProveedor.ColumnaOrdenacion = "NombreComercial";
        GridProveedor.Metodo = "ObtenerProveedor";
        GridProveedor.TituloTabla = "Catálogo de proveedores";

        //IdProveedor
        CJQColumn ColIdProveedor = new CJQColumn();
        ColIdProveedor.Nombre = "IdProveedor";
        ColIdProveedor.Oculto = "true";
        ColIdProveedor.Encabezado = "IdProveedor";
        ColIdProveedor.Buscador = "false";
        GridProveedor.Columnas.Add(ColIdProveedor);

        //NombreComercial
        CJQColumn ColNombreComercial = new CJQColumn();
        ColNombreComercial.Nombre = "NombreComercial";
        ColNombreComercial.Encabezado = "Nombre comercial";
        ColNombreComercial.Buscador = "true";
        ColNombreComercial.Alineacion = "left";
        ColNombreComercial.Ancho = "250";
        GridProveedor.Columnas.Add(ColNombreComercial);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "250";
        GridProveedor.Columnas.Add(ColRazonSocial);

        //RFC
        CJQColumn ColRFC = new CJQColumn();
        ColRFC.Nombre = "RFC";
        ColRFC.Encabezado = "RFC";
        ColRFC.Alineacion = "left";
        ColRFC.Ancho = "100";
        GridProveedor.Columnas.Add(ColRFC);

        //Direcciones
        CJQColumn ColConsultarDireccion = new CJQColumn();
        ColConsultarDireccion.Nombre = "Direcciones";
        ColConsultarDireccion.Encabezado = "Direcciones";
        ColConsultarDireccion.Etiquetado = "Imagen";
        ColConsultarDireccion.Imagen = "address.png";
        ColConsultarDireccion.Estilo = "divImagenDireccion imgFormaDirecciones";
        ColConsultarDireccion.Buscador = "false";
        ColConsultarDireccion.Ordenable = "false";
        ColConsultarDireccion.Ancho = "65";
        //ColConsultarDireccion.Oculto = puedeAccesarDireccionProveedor == 1 ? "false" : "true";
        GridProveedor.Columnas.Add(ColConsultarDireccion);

        //Contacto
        CJQColumn ColConsultarContacto = new CJQColumn();
        ColConsultarContacto.Nombre = "Contactos";
        ColConsultarContacto.Encabezado = "Contactos";
        ColConsultarContacto.Etiquetado = "Imagen";
        ColConsultarContacto.Imagen = "contacto.png";
        ColConsultarContacto.Estilo = "divImagenContacto imgFormaContactos";
        ColConsultarContacto.Buscador = "false";
        ColConsultarContacto.Ordenable = "false";
        ColConsultarContacto.Ancho = "55";
        //ColConsultarContacto.Oculto = puedeAccesarContactoProveedor == 1 ? "false" : "true";
        GridProveedor.Columnas.Add(ColConsultarContacto);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "60";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridProveedor.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarProveedor";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridProveedor.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProveedor", GridProveedor.GeneraGrid(), true);

        //GridDirecciones
        CJQGrid GridDirecciones = new CJQGrid();
        GridDirecciones.NombreTabla = "grdDirecciones";
        GridDirecciones.CampoIdentificador = "IdDireccionOrganizacion";
        GridDirecciones.ColumnaOrdenacion = "Calle";
        GridDirecciones.TipoOrdenacion = "DESC";
        GridDirecciones.Metodo = "ObtenerDirecciones";
        GridDirecciones.TituloTabla = "Direcciones de la organización";
        GridDirecciones.GenerarFuncionFiltro = false;
        GridDirecciones.GenerarFuncionTerminado = false;
        GridDirecciones.Altura = 300;
        GridDirecciones.Ancho = 600;
        GridDirecciones.NumeroRegistros = 15;
        GridDirecciones.RangoNumeroRegistros = "15,30,60";

        //IdDireccionOrganizacion
        CJQColumn ColIdDireccionOrganizacion = new CJQColumn();
        ColIdDireccionOrganizacion.Nombre = "IdDireccionOrganizacion";
        ColIdDireccionOrganizacion.Oculto = "true";
        ColIdDireccionOrganizacion.Encabezado = "IdDireccionOrganizacion";
        ColIdDireccionOrganizacion.Buscador = "false";
        GridDirecciones.Columnas.Add(ColIdDireccionOrganizacion);

        //TipoDireccion
        CJQColumn ColTipoDireccion = new CJQColumn();
        ColTipoDireccion.Nombre = "TipoDireccion";
        ColTipoDireccion.Encabezado = "Tipo de dirección";
        ColTipoDireccion.Buscador = "false";
        ColTipoDireccion.Alineacion = "left";
        ColTipoDireccion.Ancho = "100";
        GridDirecciones.Columnas.Add(ColTipoDireccion);

        //Calle
        CJQColumn ColCalle = new CJQColumn();
        ColCalle.Nombre = "Calle";
        ColCalle.Encabezado = "Dirección";
        ColCalle.Buscador = "false";
        ColCalle.Alineacion = "left";
        ColCalle.Ancho = "250";
        GridDirecciones.Columnas.Add(ColCalle);

        //Baja
        CJQColumn ColBajaDireccion = new CJQColumn();
        ColBajaDireccion.Nombre = "AI";
        ColBajaDireccion.Encabezado = "A/I";
        ColBajaDireccion.Ordenable = "false";
        ColBajaDireccion.Etiquetado = "A/I";
        ColBajaDireccion.Ancho = "55";
        ColBajaDireccion.Buscador = "true";
        ColBajaDireccion.TipoBuscador = "Combo";
        ColBajaDireccion.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridDirecciones.Columnas.Add(ColBajaDireccion);

        //Consultar
        CJQColumn ColConsultarDireccionOrganizacion = new CJQColumn();
        ColConsultarDireccionOrganizacion.Nombre = "Consultar";
        ColConsultarDireccionOrganizacion.Encabezado = "Ver";
        ColConsultarDireccionOrganizacion.Etiquetado = "ImagenConsultar";
        ColConsultarDireccionOrganizacion.Estilo = "divImagenConsultar imgFormaConsultarDireccion";
        ColConsultarDireccionOrganizacion.Buscador = "false";
        ColConsultarDireccionOrganizacion.Ordenable = "false";
        ColConsultarDireccionOrganizacion.Ancho = "25";
        GridDirecciones.Columnas.Add(ColConsultarDireccionOrganizacion);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDirecciones", GridDirecciones.GeneraGrid(), true);

        //GridContactos
        CJQGrid GridContactos = new CJQGrid();
        GridContactos.NombreTabla = "grdContactos";
        GridContactos.CampoIdentificador = "IdContactoOrganizacion";
        GridContactos.ColumnaOrdenacion = "Nombre";
        GridContactos.TipoOrdenacion = "DESC";
        GridContactos.Metodo = "ObtenerContactos";
        GridContactos.TituloTabla = "Contactos de la organización";
        GridContactos.GenerarFuncionFiltro = false;
        GridContactos.GenerarFuncionTerminado = false;
        GridContactos.Altura = 300;
        GridContactos.Ancho = 600;
        GridContactos.NumeroRegistros = 15;
        GridContactos.RangoNumeroRegistros = "15,30,60";

        //IdDireccionOrganizacion
        CJQColumn ColIdContactoOrganizacion = new CJQColumn();
        ColIdContactoOrganizacion.Nombre = "IdContactoOrganizacion";
        ColIdContactoOrganizacion.Oculto = "true";
        ColIdContactoOrganizacion.Encabezado = "IdContactoOrganizacion";
        ColIdContactoOrganizacion.Buscador = "false";
        GridContactos.Columnas.Add(ColIdContactoOrganizacion);

        //Nombre
        CJQColumn ColNombre = new CJQColumn();
        ColNombre.Nombre = "Nombre";
        ColNombre.Encabezado = "Nombre";
        ColNombre.Buscador = "false";
        ColNombre.Alineacion = "left";
        ColNombre.Ancho = "250";
        GridContactos.Columnas.Add(ColNombre);

        //Baja
        CJQColumn ColBajaContacto = new CJQColumn();
        ColBajaContacto.Nombre = "AI";
        ColBajaContacto.Encabezado = "A/I";
        ColBajaContacto.Ordenable = "false";
        ColBajaContacto.Etiquetado = "A/I";
        ColBajaContacto.Ancho = "50";
        ColBajaContacto.Buscador = "true";
        ColBajaContacto.TipoBuscador = "Combo";
        ColBajaContacto.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridContactos.Columnas.Add(ColBajaContacto);

        //Consultar
        CJQColumn ColConsultarContactoOrganizacion = new CJQColumn();
        ColConsultarContactoOrganizacion.Nombre = "Consultar";
        ColConsultarContactoOrganizacion.Encabezado = "Ver";
        ColConsultarContactoOrganizacion.Etiquetado = "ImagenConsultar";
        ColConsultarContactoOrganizacion.Estilo = "divImagenConsultar imgFormaConsultarContacto";
        ColConsultarContactoOrganizacion.Buscador = "false";
        ColConsultarContactoOrganizacion.Ordenable = "false";
        ColConsultarContactoOrganizacion.Ancho = "25";
        GridContactos.Columnas.Add(ColConsultarContactoOrganizacion);
        ClientScript.RegisterStartupScript(this.GetType(), "grdContactos", GridContactos.GeneraGrid(), true);

        //GridOrganizacionIVA
        CJQGrid GridOrganizacionIVA = new CJQGrid();
        GridOrganizacionIVA.NombreTabla = "grdOrganizacionIVA";
        GridOrganizacionIVA.CampoIdentificador = "IdOrganizacionIVA";
        GridOrganizacionIVA.ColumnaOrdenacion = "IVA";
        GridOrganizacionIVA.TipoOrdenacion = "DESC";
        GridOrganizacionIVA.Metodo = "ObtenerOrganizacionIVA";
        GridOrganizacionIVA.TituloTabla = "Tasas de IVA organización";
        GridOrganizacionIVA.GenerarFuncionFiltro = false;
        GridOrganizacionIVA.GenerarFuncionTerminado = false;
        GridOrganizacionIVA.Altura = 300;
        GridOrganizacionIVA.Ancho = 600;
        GridOrganizacionIVA.NumeroRegistros = 15;
        GridOrganizacionIVA.RangoNumeroRegistros = "15,30,60";

        //IdOrganizacionIVA
        CJQColumn ColIdOrganizacionIVA = new CJQColumn();
        ColIdOrganizacionIVA.Nombre = "IdOrganizacionIVA";
        ColIdOrganizacionIVA.Oculto = "true";
        ColIdOrganizacionIVA.Encabezado = "IdOrganizacionIVA";
        ColIdOrganizacionIVA.Buscador = "false";
        GridOrganizacionIVA.Columnas.Add(ColIdOrganizacionIVA);

        //Nombre
        CJQColumn ColIVA = new CJQColumn();
        ColIVA.Nombre = "Nombre";
        ColIVA.Encabezado = "Nombre";
        ColIVA.Buscador = "false";
        ColIVA.Alineacion = "left";
        ColIVA.Ancho = "80";
        GridOrganizacionIVA.Columnas.Add(ColIVA);

        //Baja
        CJQColumn ColBajaOrganizacionIVA = new CJQColumn();
        ColBajaOrganizacionIVA.Nombre = "AI";
        ColBajaOrganizacionIVA.Encabezado = "A/I";
        ColBajaOrganizacionIVA.Ordenable = "false";
        ColBajaOrganizacionIVA.Etiquetado = "A/I";
        ColBajaOrganizacionIVA.Ancho = "55";
        ColBajaOrganizacionIVA.Buscador = "true";
        ColBajaOrganizacionIVA.TipoBuscador = "Combo";
        ColBajaOrganizacionIVA.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridOrganizacionIVA.Columnas.Add(ColBajaOrganizacionIVA);

        //Consultar
        CJQColumn ColConsultarOrganizacionIVA = new CJQColumn();
        ColConsultarOrganizacionIVA.Nombre = "Consultar";
        ColConsultarOrganizacionIVA.Encabezado = "Ver";
        ColConsultarOrganizacionIVA.Etiquetado = "ImagenConsultar";
        ColConsultarOrganizacionIVA.Estilo = "divImagenConsultar imgFormaConsultarIVA";
        ColConsultarOrganizacionIVA.Buscador = "false";
        ColConsultarOrganizacionIVA.Ordenable = "false";
        ColConsultarOrganizacionIVA.Ancho = "25";
        GridOrganizacionIVA.Columnas.Add(ColConsultarOrganizacionIVA);

        ClientScript.RegisterStartupScript(this.GetType(), "grdOrganizacionIVA", GridOrganizacionIVA.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNombreComercial, string pRazonSocial, string pRFC, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdProveedor", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNombreComercial", SqlDbType.VarChar, 255).Value = pNombreComercial;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pRFC", SqlDbType.VarChar, 255).Value = pRFC;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDirecciones(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdProveedor, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDireccionOrganizacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdProveedor", SqlDbType.Int).Value = pIdProveedor;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerContactos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdProveedor, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdContactoOrganizacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdProveedor", SqlDbType.Int).Value = pIdProveedor;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerOrganizacionIVA(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdProveedor, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdOrganizacionIVA", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdProveedor", SqlDbType.Int).Value = pIdProveedor;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string BuscarNombreComercial(string pNombreComercial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonNombreComercial = new CJson();
        jsonNombreComercial.StoredProcedure.CommandText = "sp_Proveedor_Consultar_FiltroPorNombreComercial";
        jsonNombreComercial.StoredProcedure.Parameters.AddWithValue("@pNombreComercial", pNombreComercial);
        return jsonNombreComercial.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarRFC(string pRFC)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonRFC = new CJson();
        jsonRFC.StoredProcedure.CommandText = "sp_Cliente_Consultar_FiltroPorRFC_Proveedor";
        jsonRFC.StoredProcedure.Parameters.AddWithValue("@pRFC", pRFC);
        return jsonRFC.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarCuentaContable(string pCuentaContable)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCuentaContable jsonCuentaContable = new CCuentaContable();
        jsonCuentaContable.StoredProcedure.CommandText = "sp_Proveedor_ConsultarCuentaContable";
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonCuentaContable.StoredProcedure.Parameters.AddWithValue("@pCuentaContable", pCuentaContable);
        return jsonCuentaContable.ObtenerJsonCuentaContable(ConexionBaseDatos);

    }

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarProveedor()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            oPermisos.Add("puedeAgregarProveedor", puedeAgregarProveedor);
            oPermisos.Add("puedeAgregarLimiteCredito", puedeAgregarLimiteCredito);

            bool puedeAgregarAsignacionCuentaContableProveedor = false;
            if (Usuario.TienePermisos(new string[] { "puedeAgregarAsignacionCuentaContableProveedor" }, ConexionBaseDatos) != "")
            {
                puedeAgregarAsignacionCuentaContableProveedor = true;
            }
            oPermisos.Add("puedeAgregarAsignacionCuentaContableProveedor", puedeAgregarAsignacionCuentaContableProveedor);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
            Modelo.Add("IVA", Sucursal.IVAActual);

            CTipoIndustria TipoIndustria = new CTipoIndustria();
            Modelo.Add("TipoIndustrias", CJson.ObtenerJsonTipoIndustria(ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add("GrupoEmpresariales", CJson.ObtenerJsonGrupoEmpresariales(ConexionBaseDatos));
            Modelo.Add("TipoGarantias", CTipoGarantia.ObtenerJsonTipoGarantia(ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(ConexionBaseDatos));
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

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarDireccion()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoIndustria TipoIndustria = new CTipoIndustria();
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(ConexionBaseDatos));
            Modelo.Add("TipoDirecciones", CJson.ObtenerJsonTipoDireccion(ConexionBaseDatos));
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
    public static string ObtenerDireccionFiscalProveedor(int pIdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CProveedor Proveedor = new CProveedor();
            Proveedor.LlenaObjeto(pIdProveedor, ConexionBaseDatos);

            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            Dictionary<string, object> ParametrosDireccionOrganizacion = new Dictionary<string, object>();
            ParametrosDireccionOrganizacion.Add("IdOrganizacion", Proveedor.IdOrganizacion);
            ParametrosDireccionOrganizacion.Add("IdTipoDireccion", 1);
            DireccionOrganizacion.LlenaObjetoFiltros(ParametrosDireccionOrganizacion, ConexionBaseDatos);

            Modelo = CJson.ObtenerJsonDireccionOrganizacion(Modelo, DireccionOrganizacion.IdDireccionOrganizacion, ConexionBaseDatos);
            Modelo.Add("TipoDirecciones", CJson.ObtenerJsonTipoDireccion(Convert.ToInt32(Modelo["IdTipoDireccion"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));
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

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarContacto()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
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

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarOrganizacionIVA()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
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
    public static string ObtenerListaLocalidades(int pIdMunicipio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonLocalidades(pIdMunicipio, ConexionBaseDatos));
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
    public static string AgregarProveedor(Dictionary<string, object> pProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CProveedor Proveedor = new CProveedor();
            COrganizacion Organizacion = new COrganizacion();
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            Organizacion.RazonSocial = Convert.ToString(pProveedor["RazonSocial"]).ToUpper();
            Organizacion.NombreComercial = Convert.ToString(pProveedor["NombreComercial"]).ToUpper();
            Organizacion.RFC = Convert.ToString(pProveedor["RFC"]).ToUpper();
            Organizacion.Notas = Convert.ToString(pProveedor["Notas"]);
            Organizacion.Dominio = Convert.ToString(pProveedor["Dominio"]);
            Organizacion.IdTipoIndustria = Convert.ToInt32(pProveedor["IdTipoIndustria"]);
            Organizacion.IdGrupoEmpresarial = Convert.ToInt32(pProveedor["IdGrupoEmpresarial"]);
            Organizacion.FechaAlta = Convert.ToDateTime(DateTime.Now);
            Organizacion.IdEmpresaAlta = Usuario.IdSucursalActual;
            Organizacion.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            DireccionOrganizacion.Calle = Convert.ToString(pProveedor["Calle"]);
            DireccionOrganizacion.NumeroExterior = Convert.ToString(pProveedor["NumeroExterior"]);
            DireccionOrganizacion.NumeroInterior = Convert.ToString(pProveedor["NumeroInterior"]);
            DireccionOrganizacion.Colonia = Convert.ToString(pProveedor["Colonia"]);
            DireccionOrganizacion.CodigoPostal = Convert.ToString(pProveedor["CodigoPostal"]);
            DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pProveedor["Conmutador"]);
            DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pProveedor["IdMunicipio"]);
            DireccionOrganizacion.Referencia = Convert.ToString(pProveedor["Referencia"]);
            DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pProveedor["IdLocalidad"]);
            DireccionOrganizacion.IdTipoDireccion = 1;
            Proveedor.FechaAlta = Convert.ToDateTime(DateTime.Now);
            Proveedor.IdCondicionPago = Convert.ToInt32(pProveedor["IdCondicionPago"]);
            Proveedor.IdTipoMoneda = Convert.ToInt32(pProveedor["IdTipoMoneda"]);
            Proveedor.IVAActual = Convert.ToDecimal(pProveedor["IVAActual"]);
            Proveedor.CuentaContable = Convert.ToString(pProveedor["CuentaContable"]);
            Proveedor.CuentaContableDolares = Convert.ToString(pProveedor["CuentaContableDolares"]);
            Proveedor.IdTipoGarantia = Convert.ToInt32(pProveedor["IdTipoGarantia"]);
            Proveedor.LimiteCredito = Convert.ToString(pProveedor["LimiteCredito"]);
            Proveedor.Correo = Convert.ToString(pProveedor["Correo"]);
            Proveedor.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

            string validacion = ValidarProveedor(Proveedor, Organizacion, DireccionOrganizacion, Usuario, ConexionBaseDatos);
            if (validacion == "" || validacion == "noexisteGE")
            {
                if (validacion == "noexisteGE")
                {
                    CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
                    GrupoEmpresarial.GrupoEmpresarial = Convert.ToString(pProveedor["NombreComercial"]);
                    GrupoEmpresarial.Agregar(ConexionBaseDatos);
                    Organizacion.IdGrupoEmpresarial = Convert.ToInt32(GrupoEmpresarial.IdGrupoEmpresarial);
                }

                if (validacion != "existeCliente")
                {
                    Organizacion.Agregar(ConexionBaseDatos);
                    Proveedor.IdOrganizacion = Organizacion.IdOrganizacion;
                    DireccionOrganizacion.IdOrganizacion = Organizacion.IdOrganizacion;
                    DireccionOrganizacion.Agregar(ConexionBaseDatos);
                }

                Proveedor.Agregar(ConexionBaseDatos);
                CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
                ProveedorSucursal.IdProveedor = Proveedor.IdProveedor;
                ProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                ProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                ProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                ProveedorSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Proveedor.IdProveedor;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto el proveedor";
                HistorialGenerico.AgregarHistorialGenerico("Proveedor", ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                if (validacion == "existeCliente") //Agrego Proveedor y enrolamiento
                {
                    Dictionary<string, object> ParametrosRFC = new Dictionary<string, object>();
                    ParametrosRFC.Add("RFC", Organizacion.RFC);
                    Organizacion.LlenaObjetoFiltros(ParametrosRFC, ConexionBaseDatos);

                    Proveedor.IdOrganizacion = Organizacion.IdOrganizacion;

                    Proveedor.Agregar(ConexionBaseDatos);
                    CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
                    ProveedorSucursal.IdProveedor = Proveedor.IdProveedor;
                    ProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                    ProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    ProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    ProveedorSucursal.Agregar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Proveedor.IdProveedor;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto el Proveedor y se enrolo";
                    HistorialGenerico.AgregarHistorialGenerico("Proveedor", ConexionBaseDatos);

                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                else if (validacion == "enrolar")   //Solo se hace enrolamiento    
                {
                    Dictionary<string, object> ParametrosRFC = new Dictionary<string, object>();
                    ParametrosRFC.Add("RFC", Organizacion.RFC);
                    Organizacion.LlenaObjetoFiltros(ParametrosRFC, ConexionBaseDatos);

                    CProveedor IdProveedor = new CProveedor();
                    Dictionary<string, object> ParametrosO = new Dictionary<string, object>();
                    ParametrosO.Add("IdOrganizacion", Organizacion.IdOrganizacion);
                    IdProveedor.LlenaObjetoFiltros(ParametrosO, ConexionBaseDatos);

                    CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
                    ProveedorSucursal.IdProveedor = IdProveedor.IdProveedor;
                    ProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                    ProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    ProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    ProveedorSucursal.Agregar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Proveedor.IdProveedor;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se enrolo el proveedor a la sucursal";
                    HistorialGenerico.AgregarHistorialGenerico("Proveedor", ConexionBaseDatos);

                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("IdProveedor", IdProveedor.IdProveedor));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                    return oRespuesta.ToString();
                }

                CSelect ObtenObjeto = new CSelect();
                ObtenObjeto.StoredProcedure.CommandText = "sp_Organizacion_Consulta";
                ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(Organizacion.RFC));
                ObtenObjeto.Llena<CProveedorSucursal>(typeof(CProveedorSucursal), ConexionBaseDatos);
                foreach (CProveedorSucursal ProveedorSucursal in ObtenObjeto.ListaRegistros)
                {
                    JObject Modelo = new JObject();
                    Modelo.Add("IdProveedor", ProveedorSucursal.IdProveedor);
                    oRespuesta.Add(new JProperty("Modelo", Modelo));
                }
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string AgregarDireccion(Dictionary<string, object> pProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProveedor Proveedor = new CProveedor();
            COrganizacion Organizacion = new COrganizacion();
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            Proveedor.LlenaObjeto(Convert.ToInt32(pProveedor["IdProveedor"]), ConexionBaseDatos);
            DireccionOrganizacion.IdOrganizacion = Proveedor.IdOrganizacion;
            DireccionOrganizacion.Descripcion = Convert.ToString(pProveedor["DescripcionDireccion"]);
            DireccionOrganizacion.Calle = Convert.ToString(pProveedor["Calle"]);
            DireccionOrganizacion.NumeroExterior = Convert.ToString(pProveedor["NumeroExterior"]);
            DireccionOrganizacion.NumeroInterior = Convert.ToString(pProveedor["NumeroInterior"]);
            DireccionOrganizacion.Colonia = Convert.ToString(pProveedor["Colonia"]);
            DireccionOrganizacion.CodigoPostal = Convert.ToString(pProveedor["CodigoPostal"]);
            DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pProveedor["Conmutador"]);
            DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pProveedor["IdMunicipio"]);
            DireccionOrganizacion.Referencia = Convert.ToString(pProveedor["Referencia"]);
            DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pProveedor["IdLocalidad"]);
            DireccionOrganizacion.IdTipoDireccion = Convert.ToInt32(pProveedor["IdTipoDireccion"]);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            string validacion = ValidarDireccion(DireccionOrganizacion, ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                DireccionOrganizacion.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = DireccionOrganizacion.IdDireccionOrganizacion;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto el una nueva dirección";
                HistorialGenerico.AgregarHistorialGenerico("DireccionOrganizacion", ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarContacto(Dictionary<string, object> pProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProveedor Proveedor = new CProveedor();
            COrganizacion Organizacion = new COrganizacion();
            CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
            Proveedor.LlenaObjeto(Convert.ToInt32(pProveedor["IdProveedor"]), ConexionBaseDatos);
            ContactoOrganizacion.Nombre = Convert.ToString(pProveedor["Nombre"]);
            ContactoOrganizacion.Puesto = Convert.ToString(pProveedor["Puesto"]);
            ContactoOrganizacion.Notas = Convert.ToString(pProveedor["Notas"]);
            ContactoOrganizacion.Cumpleanio = Convert.ToDateTime(pProveedor["FechaCumpleanio"]);
            ContactoOrganizacion.IdProveedor = Convert.ToInt32(Proveedor.IdProveedor);
            ContactoOrganizacion.IdOrganizacion = Convert.ToInt32(Proveedor.IdOrganizacion);

            List<CTelefonoContactoOrganizacion> Telefonos = new List<CTelefonoContactoOrganizacion>();
            foreach (Dictionary<string, object> oTelefono in (Array)pProveedor["Telefonos"])
            {

                CTelefonoContactoOrganizacion Telefono = new CTelefonoContactoOrganizacion();
                Telefono.Telefono = Convert.ToString(oTelefono["Telefono"]);
                Telefono.Descripcion = Convert.ToString(oTelefono["Descripcion"]);
                Telefonos.Add(Telefono);
            }

            List<CCorreoContactoOrganizacion> Correos = new List<CCorreoContactoOrganizacion>();
            foreach (string oCorreo in (Array)pProveedor["Correos"])
            {
                CCorreoContactoOrganizacion Correo = new CCorreoContactoOrganizacion();
                Correo.Correo = oCorreo;
                Correos.Add(Correo);
            }


            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            string validacion = ValidarContacto(ContactoOrganizacion, ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                ContactoOrganizacion.Agregar(ConexionBaseDatos);

                foreach (CTelefonoContactoOrganizacion oTelefono in Telefonos)
                {
                    oTelefono.IdContactoOrganizacion = ContactoOrganizacion.IdContactoOrganizacion;
                    oTelefono.Agregar(ConexionBaseDatos);
                }

                foreach (CCorreoContactoOrganizacion oCorreo in Correos)
                {
                    oCorreo.IdContactoOrganizacion = ContactoOrganizacion.IdContactoOrganizacion;
                    oCorreo.Agregar(ConexionBaseDatos);
                }

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = ContactoOrganizacion.IdContactoOrganizacion;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto el una nuevo contacto";
                HistorialGenerico.AgregarHistorialGenerico("ContactoOrganizacion", ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarTelefonoEditar(Dictionary<string, object> pProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTelefonoContactoOrganizacion TelefonoContactoOrganizacion = new CTelefonoContactoOrganizacion();
            TelefonoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pProveedor["IdContactoOrganizacion"]);
            TelefonoContactoOrganizacion.Telefono = Convert.ToString(pProveedor["Telefono"]);
            TelefonoContactoOrganizacion.Descripcion = Convert.ToString(pProveedor["Descripcion"]);
            TelefonoContactoOrganizacion.Agregar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarOrganizacionIVA(Dictionary<string, object> pProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            COrganizacionIVA OrganizacionIVA = new COrganizacionIVA();
            CProveedor Proveedor = new CProveedor();
            Proveedor.LlenaObjeto(Convert.ToInt32(pProveedor["IdProveedor"]), ConexionBaseDatos);
            OrganizacionIVA.IdOrganizacion = Proveedor.IdOrganizacion;
            OrganizacionIVA.IVA = Convert.ToInt32(pProveedor["IVA"]);
            OrganizacionIVA.EsPrincipal = Convert.ToBoolean(pProveedor["EsPrincipal"]);
            OrganizacionIVA.AgregarOrganizacionIVA(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarCorreoEditar(Dictionary<string, object> pProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCorreoContactoOrganizacion CorreoContactoOrganizacion = new CCorreoContactoOrganizacion();
            CorreoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pProveedor["IdContactoOrganizacion"]);
            CorreoContactoOrganizacion.Correo = Convert.ToString(pProveedor["Correo"]);
            CorreoContactoOrganizacion.Agregar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFormaProveedor(int pIdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        oPermisos.Add("puedeEditarProveedor", puedeEditarProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonProveedor(Modelo, pIdProveedor, ConexionBaseDatos);
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
    public static string ObtenerFormaDireccionOrganizacion(int pIdDireccionOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeEditarDireccion", puedeEditarDireccion);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonDireccionOrganizacion(Modelo, pIdDireccionOrganizacion, ConexionBaseDatos);
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
    public static string ObtenerFormaContactoOrganizacion(int pIdContactoOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarContactoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarContactoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarContactoProveedor = 1;
        }
        oPermisos.Add("puedeEditarContactoProveedor", puedeEditarContactoProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonContactoOrganizacion(Modelo, pIdContactoOrganizacion, ConexionBaseDatos);
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
    public static string ObtenerFormaIVA(int pIdOrganizacionIVA)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarOrganizacionIVA = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarOrganizacionIVA" }, ConexionBaseDatos) == "")
        {
            puedeEditarOrganizacionIVA = 1;
        }
        oPermisos.Add("puedeEditarOrganizacionIVA", puedeEditarOrganizacionIVA);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = COrganizacionIVA.ObtenerIVA(Modelo, pIdOrganizacionIVA, ConexionBaseDatos);
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
    public static string ObtenerFormaDirecciones(int pIdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDireccion = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDireccion" }, ConexionBaseDatos) == "")
        {
            puedeEditarDireccion = 1;
        }
        oPermisos.Add("puedeEditarDireccion", puedeEditarDireccion);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonProveedor(Modelo, pIdProveedor, ConexionBaseDatos);
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
    public static string ObtenerFormaContactos(int pIdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarContacto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarContacto" }, ConexionBaseDatos) == "")
        {
            puedeEditarContacto = 1;
        }
        oPermisos.Add("puedeEditarContacto", puedeEditarContacto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonProveedor(Modelo, pIdProveedor, ConexionBaseDatos);
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
    public static string ObtenerFormaOrganizacionIVA(int pIdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarOrganizacionIVA = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarOrganizacionIVA" }, ConexionBaseDatos) == "")
        {
            puedeEditarOrganizacionIVA = 1;
        }
        oPermisos.Add("puedeEditarOrganizacionIVA", puedeEditarOrganizacionIVA);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonProveedor(Modelo, pIdProveedor, ConexionBaseDatos);
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
    public static string ObtenerFormaTelefonosCorreos(int IdContactoOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarContacto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarContacto" }, ConexionBaseDatos) == "")
        {
            puedeEditarContacto = 1;
        }
        oPermisos.Add("puedeEditarContacto", puedeEditarContacto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonTelefonosCorreos(Modelo, IdContactoOrganizacion, ConexionBaseDatos);
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
    public static string ObtenerFormaProveedorAEnrolar(int pIdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarProveedor = 1;
        }
        oPermisos.Add("puedeEditarProveedor", puedeEditarProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonProveedor(Modelo, pIdProveedor, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarProveedor(int IdProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();


        oPermisos.Add("puedeEditarDatosOrganizacion", puedeEditarDatosOrganizacion);
        oPermisos.Add("puedeEditarDatosFiscales", puedeEditarDatosFiscales);
        oPermisos.Add("puedeEditarProveedor", puedeEditarProveedor);
        oPermisos.Add("puedeEditarDatosLimiteCredito", puedeEditarDatosLimiteCredito);



        if (respuesta == "Conexion Establecida")
        {
            bool puedeEditarAsignacionCuentaContableProveedor = false;
            if (Usuario.TienePermisos(new string[] { "puedeEditarAsignacionCuentaContableProveedor" }, ConexionBaseDatos) != "")
            {
                puedeEditarAsignacionCuentaContableProveedor = true;
            }
            oPermisos.Add("puedeEditarAsignacionCuentaContableProveedor", puedeEditarAsignacionCuentaContableProveedor);

            JObject Modelo = new JObject();
            CProveedor Proveedor = new CProveedor();
            Proveedor.LlenaObjeto(IdProveedor, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonProveedor(Modelo, IdProveedor, ConexionBaseDatos);
            //mike
            Modelo.Add("IdUsuarioSucursalActual", idSucursal);//esta la sucursal en la cual estamos logueados.


            Modelo.Add("TipoIndustrias", CJson.ObtenerJsonTipoIndustria(Convert.ToInt32(Modelo["IdTipoIndustria"].ToString()), ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(Convert.ToInt32(Modelo["IdCondicionPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("GrupoEmpresariales", CJson.ObtenerJsonGrupoEmpresariales(Convert.ToInt32(Modelo["IdGrupoEmpresarial"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));
            Modelo.Add("TipoGarantias", CTipoGarantia.ObtenerJsonTipoGarantia(Proveedor.IdTipoGarantia, ConexionBaseDatos));
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
    public static string ObtenerFormaEditarDireccion(int IdDireccionOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDireccion = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDireccion" }, ConexionBaseDatos) == "")
        {
            puedeEditarDireccion = 1;
        }
        oPermisos.Add("puedeEditarDireccion", puedeEditarDireccion);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            DireccionOrganizacion.LlenaObjeto(IdDireccionOrganizacion, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonDireccionOrganizacion(Modelo, IdDireccionOrganizacion, ConexionBaseDatos);
            Modelo.Add("TipoDirecciones", CJson.ObtenerJsonTipoDireccion(Convert.ToInt32(Modelo["IdTipoDireccion"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));
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
    public static string ObtenerFormaEditarContacto(int IdContactoOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarContactoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarContactoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarContactoProveedor = 1;
        }
        oPermisos.Add("puedeEditarContactoProveedor", puedeEditarContactoProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
            ContactoOrganizacion.LlenaObjeto(IdContactoOrganizacion, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonContactoOrganizacion(Modelo, IdContactoOrganizacion, ConexionBaseDatos);
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
    public static string EditarProveedor(Dictionary<string, object> pProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CProveedor Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(Convert.ToInt32(pProveedor["IdProveedor"]), ConexionBaseDatos);
        decimal IVA_Anterior = Proveedor.IVAActual;

        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(Convert.ToInt32(pProveedor["IdProveedor"]), ConexionBaseDatos);
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, ConexionBaseDatos);
        Dictionary<string, object> ParametrosD = new Dictionary<string, object>();
        ParametrosD.Add("IdTipoDireccion", 1);
        ParametrosD.Add("IdOrganizacion", Proveedor.IdOrganizacion);
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosD, ConexionBaseDatos);

        Proveedor.IdProveedor = Convert.ToInt32(pProveedor["IdProveedor"]);
        Organizacion.RazonSocial = Convert.ToString(pProveedor["RazonSocial"]).ToUpper();
        Organizacion.NombreComercial = Convert.ToString(pProveedor["NombreComercial"]).ToUpper();
        Organizacion.RFC = Convert.ToString(pProveedor["RFC"]).ToUpper();
        Organizacion.Notas = Convert.ToString(pProveedor["Notas"]);
        Organizacion.Dominio = Convert.ToString(pProveedor["Dominio"]);
        Organizacion.IdTipoIndustria = Convert.ToInt32(pProveedor["IdTipoIndustria"]);
        Organizacion.IdGrupoEmpresarial = Convert.ToInt32(pProveedor["IdGrupoEmpresarial"]);
        Organizacion.FechaModificacion = Convert.ToDateTime(DateTime.Now);
        DireccionOrganizacion.Calle = Convert.ToString(pProveedor["Calle"]);
        DireccionOrganizacion.NumeroExterior = Convert.ToString(pProveedor["NumeroExterior"]);
        DireccionOrganizacion.NumeroInterior = Convert.ToString(pProveedor["NumeroInterior"]);
        DireccionOrganizacion.Colonia = Convert.ToString(pProveedor["Colonia"]);
        DireccionOrganizacion.CodigoPostal = Convert.ToString(pProveedor["CodigoPostal"]);
        DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pProveedor["Conmutador"]);
        DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pProveedor["IdMunicipio"]);
        DireccionOrganizacion.Referencia = Convert.ToString(pProveedor["Referencia"]);
        DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pProveedor["IdLocalidad"]);
        DireccionOrganizacion.IdTipoDireccion = 1;
        Proveedor.FechaModificacion = Convert.ToDateTime(DateTime.Now);
        Proveedor.IdCondicionPago = Convert.ToInt32(pProveedor["IdCondicionPago"]);
        Proveedor.IdTipoMoneda = Convert.ToInt32(pProveedor["IdTipoMoneda"]);
        Proveedor.IVAActual = Math.Truncate(Convert.ToDecimal(pProveedor["IVAActual"]) * 100) / 100;
        Proveedor.IVAActual = Convert.ToDecimal(pProveedor["IVAActual"]);
        Proveedor.IdUsuarioModifico = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Proveedor.CuentaContable = Convert.ToString(pProveedor["CuentaContable"]);
        Proveedor.CuentaContableDolares = Convert.ToString(pProveedor["CuentaContableDolares"]);
        Proveedor.Correo = Convert.ToString(pProveedor["Correo"]);
        Proveedor.IdTipoGarantia = Convert.ToInt32(pProveedor["IdTipoGarantia"]);
        Proveedor.LimiteCredito = Convert.ToString(pProveedor["LimiteCredito"]);

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        string validacion = ValidarProveedor(Proveedor, Organizacion, DireccionOrganizacion, Usuario, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Proveedor.Editar(ConexionBaseDatos);
            Organizacion.Editar(ConexionBaseDatos);
            DireccionOrganizacion.Editar(ConexionBaseDatos);

            string cambioIVA = string.Empty;
            if (IVA_Anterior != Proveedor.IVAActual)
            {
                cambioIVA = "El IVA cambio de" + IVA_Anterior.ToString() + " a " + Proveedor.IVAActual.ToString();
            }

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Proveedor.IdProveedor;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico el proveedor. " + cambioIVA;
            HistorialGenerico.AgregarHistorialGenerico("Proveedor", ConexionBaseDatos);


            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarDireccion(Dictionary<string, object> pProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CProveedor Proveedor = new CProveedor();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        DireccionOrganizacion.LlenaObjeto(Convert.ToInt32(pProveedor["IdDireccionOrganizacion"]), ConexionBaseDatos);
        DireccionOrganizacion.Descripcion = Convert.ToString(pProveedor["DescripcionDireccion"]);
        DireccionOrganizacion.Calle = Convert.ToString(pProveedor["Calle"]);
        DireccionOrganizacion.NumeroExterior = Convert.ToString(pProveedor["NumeroExterior"]);
        DireccionOrganizacion.NumeroInterior = Convert.ToString(pProveedor["NumeroInterior"]);
        DireccionOrganizacion.Colonia = Convert.ToString(pProveedor["Colonia"]);
        DireccionOrganizacion.CodigoPostal = Convert.ToString(pProveedor["CodigoPostal"]);
        DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pProveedor["Conmutador"]);
        DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pProveedor["IdMunicipio"]);
        DireccionOrganizacion.Referencia = Convert.ToString(pProveedor["Referencia"]);
        DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pProveedor["IdLocalidad"]);
        DireccionOrganizacion.IdTipoDireccion = Convert.ToInt32(pProveedor["IdTipoDireccion"]);

        string validacion = ValidarDireccion(DireccionOrganizacion, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            DireccionOrganizacion.Editar(ConexionBaseDatos);
            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = DireccionOrganizacion.IdDireccionOrganizacion;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico la dirección";
            HistorialGenerico.AgregarHistorialGenerico("DireccionOrganizacion", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarContacto(Dictionary<string, object> pProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();


        CProveedor Proveedor = new CProveedor();
        COrganizacion Organizacion = new COrganizacion();

        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        ContactoOrganizacion.LlenaObjeto(Convert.ToInt32(pProveedor["IdContactoOrganizacion"]), ConexionBaseDatos);

        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        ContactoOrganizacion.Nombre = Convert.ToString(pProveedor["Nombre"]);
        ContactoOrganizacion.Puesto = Convert.ToString(pProveedor["Puesto"]);
        ContactoOrganizacion.Notas = Convert.ToString(pProveedor["Notas"]);
        ContactoOrganizacion.Cumpleanio = Convert.ToDateTime(pProveedor["FechaCumpleanio"]);

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        string validacion = ValidarContacto(ContactoOrganizacion, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            ContactoOrganizacion.Editar(ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = ContactoOrganizacion.IdContactoOrganizacion;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico el contacto";
            HistorialGenerico.AgregarHistorialGenerico("ContactoOrganizacion", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string EditarOrganizacionIVA(Dictionary<string, object> pProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            COrganizacionIVA OrganizacionIVA = new COrganizacionIVA();
            OrganizacionIVA.LlenaObjeto(Convert.ToInt32(pProveedor["IdOrganizacionIVA"]), ConexionBaseDatos);
            OrganizacionIVA.IdOrganizacionIVA = Convert.ToInt32(pProveedor["IdOrganizacionIVA"]);
            OrganizacionIVA.IVA = Convert.ToInt32(pProveedor["IVA"]);
            OrganizacionIVA.EsPrincipal = Convert.ToBoolean(pProveedor["EsPrincipal"]);
            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(Convert.ToInt32(pProveedor["IdProveedor"]), ConexionBaseDatos);
            OrganizacionIVA.IdOrganizacion = Organizacion.IdOrganizacion;
            OrganizacionIVA.EditarOrganizacionIVA(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EliminaTelefonoContactoOrganizacion(int pIdTelefonoContactoOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CTelefonoContactoOrganizacion TelefonoContactoOrganizacion = new CTelefonoContactoOrganizacion();
        TelefonoContactoOrganizacion.IdTelefonoContactoOrganizacion = Convert.ToInt32(pIdTelefonoContactoOrganizacion);
        TelefonoContactoOrganizacion.Baja = true;
        TelefonoContactoOrganizacion.Eliminar(ConexionBaseDatos);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string EliminaCorreoContactoOrganizacion(int pIdCorreoContactoOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CCorreoContactoOrganizacion CorreoContactoOrganizacion = new CCorreoContactoOrganizacion();
        CorreoContactoOrganizacion.IdCorreoContactoOrganizacion = Convert.ToInt32(pIdCorreoContactoOrganizacion);
        CorreoContactoOrganizacion.Baja = true;
        CorreoContactoOrganizacion.Eliminar(ConexionBaseDatos);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]//metodo para la enrolar en la BD
    public static string EnrolarProveedor(Dictionary<string, object> pProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        CProveedor Proveedor = new CProveedor();

        Proveedor.IdProveedor = Convert.ToInt32(pProveedor["IdProveedor"]);
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
        ProveedorSucursal.IdProveedor = Proveedor.IdProveedor;
        ProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
        ProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
        ProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        ProveedorSucursal.Agregar(ConexionBaseDatos);

        CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
        HistorialGenerico.IdGenerico = Proveedor.IdProveedor;
        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
        HistorialGenerico.Comentario = "Se enrolo el proveedor a otra sucursal";
        HistorialGenerico.AgregarHistorialGenerico("Proveedor", ConexionBaseDatos);

        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("IdProveedor", Proveedor.IdProveedor));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdProveedor, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProveedor Proveedor = new CProveedor();
            Proveedor.IdProveedor = pIdProveedor;
            Proveedor.Baja = pBaja;
            Proveedor.Eliminar(ConexionBaseDatos);
            respuesta = "0|ProveedorEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusDireccion(int pIdDireccionOrganizacion, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            DireccionOrganizacion.IdDireccionOrganizacion = pIdDireccionOrganizacion;
            DireccionOrganizacion.Baja = pBaja;
            DireccionOrganizacion.Eliminar(ConexionBaseDatos);
            respuesta = "0|DireccionOrganizacionEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusContacto(int pIdContactoOrganizacion, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
            ContactoOrganizacion.IdContactoOrganizacion = pIdContactoOrganizacion;
            ContactoOrganizacion.Baja = pBaja;
            ContactoOrganizacion.Eliminar(ConexionBaseDatos);
            respuesta = "0|ContactoOrganizacionEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerListaTiposIndustrias()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonTipoIndustria(true, ConexionBaseDatos));
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
    public static string ObtenerListaCondicionesPago()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonCondicionPago(true, ConexionBaseDatos));
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
    public static string ObtenerListaGrupoEmpresarial()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonGrupoEmpresariales(true, ConexionBaseDatos));
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
    public static string BuscarRazonSocial(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonRazonSocial = new CJson();
        jsonRazonSocial.StoredProcedure.CommandText = "sp_Proveedor_Consultar_FiltroPorRazonSocialGrid";
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        return jsonRazonSocial.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerProveedorExistente(int pIdOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CProveedor Proveedor = new CProveedor();
            CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
            Dictionary<string, object> ParametrosProveedor = new Dictionary<string, object>();
            ParametrosProveedor.Add("IdOrganizacion", pIdOrganizacion);

            Proveedor.LlenaObjetoFiltros(ParametrosProveedor, ConexionBaseDatos);
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            int validacion = Proveedor.RevisaExisteProveedor(pIdOrganizacion, Usuario.IdSucursalActual, ConexionBaseDatos);
            if (validacion == 1)
            {
                //----Ya existe en mi sucursal, solo imprimir datos
                JObject Modelo = new JObject();
                Modelo = CJson.ObtenerJsonProveedor(Modelo, Proveedor.IdProveedor, ConexionBaseDatos);
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else if (validacion == 2)
            {
                //----Ya existe como proveedor, solo enrolar
                oRespuesta.Add(new JProperty("Modelo", "enrolar"));
                oRespuesta.Add(new JProperty("IdProveedor", Proveedor.IdProveedor));

            }
            else if (validacion == 3)
            {
                //----Existe como cliente, agregar proveedor y enrolar
                oRespuesta.Add(new JProperty("agregarProveedor", "¿Desea agregarlo a su lista de proveedor?"));
                oRespuesta.Add(new JProperty("IdOrganizacion", pIdOrganizacion));
                oRespuesta.Add(new JProperty("IdSucursalActual", Usuario.IdSucursalActual));

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(pIdOrganizacion, ConexionBaseDatos);

                Dictionary<string, object> ParametrosSucursalAlta = new Dictionary<string, object>();
                ParametrosSucursalAlta.Add("IdUsuario", Organizacion.IdUsuarioAlta);
                Usuario.LlenaObjetoFiltros(ParametrosSucursalAlta, ConexionBaseDatos);

                oRespuesta.Add(new JProperty("IdSucursalAlta", Usuario.IdSucursalActual));
            }
            else
            {
                oRespuesta.Add(new JProperty("error", "No existe proveedor ni cliente"));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string RevisaExisteRFC(string pRFC)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRFC", pRFC);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string RevisaProveedorRFC(string pRFC)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CProveedor Proveedor = new CProveedor();
            CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
            COrganizacion Organizacion = new COrganizacion();

            Dictionary<string, object> ParametrosRFC = new Dictionary<string, object>();
            ParametrosRFC.Add("RFC", pRFC);
            Organizacion.LlenaObjetoFiltros(ParametrosRFC, ConexionBaseDatos);


            if (Organizacion.IdOrganizacion != 0)
            {
                Dictionary<string, object> ParametrosC = new Dictionary<string, object>();
                ParametrosC.Add("IdOrganizacion", Organizacion.IdOrganizacion);
                Proveedor.LlenaObjetoFiltros(ParametrosC, ConexionBaseDatos);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                int validacion = Proveedor.RevisaExisteProveedor(Organizacion.IdOrganizacion, Usuario.IdSucursalActual, ConexionBaseDatos);

                if (validacion == 1) //Ya existe en mi sucursal, solo imprimir datos
                {

                    JObject Modelo = new JObject();
                    Modelo = CJson.ObtenerJsonProveedor(Modelo, Proveedor.IdProveedor, ConexionBaseDatos);
                    Modelo.Add(new JProperty("Permisos", oPermisos));
                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Modelo", Modelo));
                }
                else if (validacion == 2) //Ya existe como cliente, solo enrolar
                {
                    oRespuesta.Add(new JProperty("Modelo", "enrolar"));
                    oRespuesta.Add(new JProperty("IdProveedor", Proveedor.IdProveedor));

                }
                else if (validacion == 3) //Existe como Cliente, mostrar datos de organizacion y direccion de organizacion
                {
                    oRespuesta.Add(new JProperty("Modelo", "agregarProveedor"));
                    oRespuesta.Add(new JProperty("IdOrganizacion", Organizacion.IdOrganizacion));

                    //COrganizacion Organizacion = new COrganizacion();
                    //Organizacion.LlenaObjeto(Organizacion.IdOrganizacion, ConexionBaseDatos);
                }

                else
                {
                    oRespuesta.Add(new JProperty("Descripcion", "noExisteProveedorCliente"));
                }

                Dictionary<string, object> ParametrosSucursalAlta = new Dictionary<string, object>();
                ParametrosSucursalAlta.Add("IdUsuario", Organizacion.IdUsuarioAlta);
                Usuario.LlenaObjetoFiltros(ParametrosSucursalAlta, ConexionBaseDatos);

                oRespuesta.Add(new JProperty("IdSucursalActual", Usuario.IdSucursalActual));
                oRespuesta.Add(new JProperty("IdSucursalAlta", Usuario.IdSucursalActual));

            }
            else
            {
                oRespuesta.Add(new JProperty("Modelo", "noExiste"));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string RevisaExisteOrganizacion(int pIdOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            CProveedor Proveedor = new CProveedor();
            CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
            Dictionary<string, object> ParametrosC = new Dictionary<string, object>();
            ParametrosC.Add("IdOrganizacion", pIdOrganizacion);

            Proveedor.LlenaObjetoFiltros(ParametrosC, ConexionBaseDatos);
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            int validacion = Proveedor.RevisaExisteProveedor(pIdOrganizacion, Usuario.IdSucursalActual, ConexionBaseDatos);

            if (validacion == 1) //Ya existe en mi sucursal, solo imprimir datos
            {
                JObject Modelo = new JObject();
                Modelo = CJson.ObtenerJsonProveedor(Modelo, Proveedor.IdProveedor, ConexionBaseDatos);
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else if (validacion == 2) //Ya existe como proveedor, solo enrolar
            {
                oRespuesta.Add(new JProperty("Modelo", "enrolar"));
                oRespuesta.Add(new JProperty("IdProveedor", Proveedor.IdProveedor));

            }
            else if (validacion == 3) //Existe como cliente, agregar proveedor y enrolar
            {
                oRespuesta.Add(new JProperty("Modelo", "agregarProveedor"));
                oRespuesta.Add(new JProperty("IdOrganizacion", pIdOrganizacion));

                oRespuesta.Add(new JProperty("IdSucursalActual", Usuario.IdSucursalActual));

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(pIdOrganizacion, ConexionBaseDatos);

                Dictionary<string, object> ParametrosSucursalAlta = new Dictionary<string, object>();
                ParametrosSucursalAlta.Add("IdUsuario", Organizacion.IdUsuarioAlta);
                Usuario.LlenaObjetoFiltros(ParametrosSucursalAlta, ConexionBaseDatos);

                oRespuesta.Add(new JProperty("IdSucursalAlta", Usuario.IdSucursalActual));
            }
            else
            {
                oRespuesta.Add(new JProperty("error", "No existe proveedor ni cliente"));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string ObtenerFormaClienteAEnrolar(int IdOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarProveedor = 1;
        }
        oPermisos.Add("puedeEditarProveedor", puedeEditarProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            Dictionary<string, object> ParametrosCliente = new Dictionary<string, object>();
            ParametrosCliente.Add("IdOrganizacion", IdOrganizacion);
            Cliente.LlenaObjetoFiltros(ParametrosCliente, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonCliente(Modelo, Cliente.IdCliente, ConexionBaseDatos);
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
    public static string ObtenerFormaConsultarCliente(int pIdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCliente = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        if (Usuario.TienePermisos(new string[] { "puedeEditarCliente" }, ConexionBaseDatos) == "")
        {
            puedeEditarCliente = 1;
        }
        oPermisos.Add("puedeEditarCliente", puedeEditarCliente);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonCliente(Modelo, pIdCliente, ConexionBaseDatos);
            if (Sucursal.IdEmpresa != Convert.ToInt32(Modelo["IdEmpresaAlta"].ToString()))
            {
                oPermisos.Add("diferenteSucursal", 1);
            }
            else
            {
                oPermisos.Add("diferenteSucursal", 0);
            }
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
    public static string EnrolarClienteAProveedor(Dictionary<string, object> pCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        CCliente Cliente = new CCliente();

        Cliente.IdCliente = Convert.ToInt32(pCliente["IdCliente"]);
        Cliente.LlenaObjeto(Cliente.IdCliente, ConexionBaseDatos);
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CProveedor Proveedor = new CProveedor();
        Proveedor.FechaAlta = Convert.ToDateTime(DateTime.Now);
        Proveedor.Correo = Convert.ToString(Cliente.Correo);
        Proveedor.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Proveedor.IdOrganizacion = Convert.ToInt32(Cliente.IdOrganizacion);
        Proveedor.Baja = false;
        Proveedor.Agregar(ConexionBaseDatos);

        CProveedorSucursal ProveedorSucursal = new CProveedorSucursal();
        ProveedorSucursal.IdProveedor = Proveedor.IdProveedor;
        ProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
        ProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
        ProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        ProveedorSucursal.Agregar(ConexionBaseDatos);

        CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
        HistorialGenerico.IdGenerico = Proveedor.IdProveedor;
        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
        HistorialGenerico.Comentario = "Se enrolo el cliente como proveedor";
        HistorialGenerico.AgregarHistorialGenerico("Proveedor", ConexionBaseDatos);

        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("IdProveedor", Proveedor.IdProveedor));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string RevisaExisteRazonSocial(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
    }

    [WebMethod]
    public static string RevisaExisteOrganizacionRFC(string pRFC)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRFC);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        //return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
        return jsonOrganizacion.ObtenerJsonRFC(ConexionBaseDatos);
    }


    static string ValidarProveedor(CProveedor pProveedor, COrganizacion pOrganizacion, CDireccionOrganizacion pDireccionOrganizacion, CUsuario pUsuario, CConexion pConexion)
    {
        string errores = "";
        int ExisteProveedor = 0;
        if (pOrganizacion.RazonSocial == "")
        { errores = errores + "<span>*</span> La razón social esta vacía, favor de capturarla.<br />"; }

        if (pOrganizacion.NombreComercial == "")
        { errores = errores + "<span>*</span> El nombre comercial del proveedor esta vacío, favor de capturarlo.<br />"; }

        if (pOrganizacion.RFC == "")
        { errores = errores + "<span>*</span> El RFC esta vacío, favor de capturarlo.<br />"; }

        if (pOrganizacion.IdTipoIndustria == 0)
        { errores = errores + "<span>*</span> El campo tipo de industria esta vacío, favor de seleccionarlo.<br />"; }

        if (pProveedor.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacío, favor de seleccionarlo.<br />"; }

        if (pDireccionOrganizacion.Calle == "")
        { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.NumeroExterior == "")
        { errores = errores + "<span>*</span> El campo número exterior esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.Colonia == "")
        { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.CodigoPostal == "")
        { errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.IdMunicipio == 0)
        { errores = errores + "<span>*</span> El campo municipio esta vacío, favor de seleccionarlo.<br />"; }

        if (pDireccionOrganizacion.IdLocalidad == 0)
        { errores = errores + "<span>*</span> El campo localidad esta vacío, favor de seleccionarlo.<br />"; }

        if (pProveedor.IdProveedor == 0)
        {
            ExisteProveedor = pProveedor.ExisteProveedor(pOrganizacion.RFC, pUsuario.IdSucursalActual, pConexion);
            if (ExisteProveedor != 0)
            {
                if (ExisteProveedor == 1)
                {
                    errores = errores + "<span>*</span> El RFC '" + pOrganizacion.RFC + "' ya existe, favor de revisarlo.<br />";
                }
                else if (ExisteProveedor == 2) //Ya existe como cliente, solo enrolar
                {
                    errores = errores + "enrolar";
                    return errores;
                }
                else if (ExisteProveedor == 3) //Existe como proveedor, mostrar datos de organizacion y direccion de organizacion
                {
                    errores = errores + "existeCliente";
                    return errores;
                }
            }
            if (pOrganizacion.IdGrupoEmpresarial == 0)
            {
                if (pOrganizacion.ExisteGrupoEmpresarial(pOrganizacion.NombreComercial, pConexion))
                {
                    errores = errores + "<span>*</span> El grupo empresarial ya existe, favor de revisarlo.<br />";
                }
                else
                {
                    errores = errores + "noexisteGE";
                    return errores;
                }
            }
        }
        else
        {
            ExisteProveedor = pProveedor.ExisteProveedorEditar(pOrganizacion.RFC, pProveedor.IdProveedor, pUsuario.IdSucursalActual, pConexion);
            if (ExisteProveedor != 0)
            {
                if (ExisteProveedor == 1)
                {
                    errores = errores + "<span>*</span> El RFC '" + pOrganizacion.RFC + "' ya existe, favor de revisarlo.<br />";
                }
                else
                {
                    errores = errores + "<span>*</span> No puede editar un proveedor que no este enrolada a su empresa.<br />";
                    return errores;
                }
            }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarDireccion(CDireccionOrganizacion pDireccionOrganizacion, CConexion pConexion)
    {
        string errores = "";

        if (pDireccionOrganizacion.Calle == "")
        { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.NumeroExterior == "")
        { errores = errores + "<span>*</span> El campo numero exterior esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.Colonia == "")
        { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.CodigoPostal == "")
        { errores = errores + "<span>*</span> El campo codigo postal esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.IdMunicipio == 0)
        { errores = errores + "<span>*</span> El campo Municipio esta vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarContacto(CContactoOrganizacion pContactoOrganizacion, CConexion pConexion)
    {
        string errores = "";

        if (pContactoOrganizacion.Nombre == "")
        { errores = errores + "<span>*</span> El campo nombre esta vacío, favor de capturarlo.<br />"; }

        if (pContactoOrganizacion.Puesto == "")
        { errores = errores + "<span>*</span> El campo puesto esta vacío, favor de capturarlo.<br />"; }

        if (pContactoOrganizacion.Notas == "")
        { errores = errores + "<span>*</span> El campo de notas esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}