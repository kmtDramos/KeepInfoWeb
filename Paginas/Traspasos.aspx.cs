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

public partial class Traspasos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridAlmacenProducto
        CJQGrid GridAlmacenProductoResumen = new CJQGrid();
        GridAlmacenProductoResumen.NombreTabla = "grdAlmacenProductoResumen";
        GridAlmacenProductoResumen.CampoIdentificador = "IdAlmacen";
        GridAlmacenProductoResumen.ColumnaOrdenacion = "Almacen";
        GridAlmacenProductoResumen.Metodo = "ObtenerAlmacenProductoResumen";
        GridAlmacenProductoResumen.TituloTabla = "Almacenes con el producto";
        GridAlmacenProductoResumen.GenerarGridCargaInicial = false;
        GridAlmacenProductoResumen.GenerarFuncionFiltro = false;

        //IdAlmacen
        CJQColumn ColIdAlmacen = new CJQColumn();
        ColIdAlmacen.Nombre = "IdAlmacen";
        ColIdAlmacen.Oculto = "true";
        ColIdAlmacen.Encabezado = "IdAlmacen";
        ColIdAlmacen.Buscador = "false";
        GridAlmacenProductoResumen.Columnas.Add(ColIdAlmacen);

        //IdProducto
        CJQColumn ColIdProducto = new CJQColumn();
        ColIdProducto.Nombre = "IdProducto";
        ColIdProducto.Oculto = "true";
        ColIdProducto.Encabezado = "IdProducto";
        ColIdProducto.Buscador = "false";
        GridAlmacenProductoResumen.Columnas.Add(ColIdProducto);

        //Almacen
        CJQColumn ColAlmacen = new CJQColumn();
        ColAlmacen.Nombre = "Almacen";
        ColAlmacen.Encabezado = "Almacen";
        ColAlmacen.Ancho = "400";
        ColAlmacen.Alineacion = "left";
        GridAlmacenProductoResumen.Columnas.Add(ColAlmacen);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Saldo";
        ColCantidad.Encabezado = "Saldo";
        ColCantidad.Ancho = "400";
        ColCantidad.Alineacion = "center";
        GridAlmacenProductoResumen.Columnas.Add(ColCantidad);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgConsultarAlmacenDetalle";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridAlmacenProductoResumen.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAlmacenProductoResumen", GridAlmacenProductoResumen.GeneraGrid(), true);

        //GridAlmacenProductoDetalle
        CJQGrid GridAlmacenProductoDetalle = new CJQGrid();
        GridAlmacenProductoDetalle.NombreTabla = "grdAlmacenProductoDetalle";
        GridAlmacenProductoDetalle.CampoIdentificador = "IdAlmacen";
        GridAlmacenProductoDetalle.ColumnaOrdenacion = "Almacen";
        GridAlmacenProductoDetalle.Metodo = "ObtenerAlmacenProductoDetalle";
        GridAlmacenProductoDetalle.TituloTabla = "Almacenes con el producto";
        GridAlmacenProductoDetalle.GenerarGridCargaInicial = false;
        GridAlmacenProductoDetalle.GenerarFuncionFiltro = false;

        //IdExistenciaDistribuida
        CJQColumn ColIdExistenciaDistribuida = new CJQColumn();
        ColIdExistenciaDistribuida.Nombre = "IdExistenciaDistribuida";
        ColIdExistenciaDistribuida.Oculto = "true";
        ColIdExistenciaDistribuida.Encabezado = "IdExistenciaDistribuida";
        ColIdExistenciaDistribuida.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColIdExistenciaDistribuida);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Oculto = "true";
        ColFolio.Encabezado = "Folio";
        ColFolio.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColFolio);

        //CantidadDetalle
        CJQColumn ColCantidadDetalle = new CJQColumn();
        ColCantidadDetalle.Nombre = "CantidadDetalle";
        ColCantidadDetalle.Encabezado = "CantidadDetalle";
        ColCantidadDetalle.Ancho = "400";
        ColCantidadDetalle.Alineacion = "left";
        GridAlmacenProductoDetalle.Columnas.Add(ColCantidadDetalle);

        //Saldo
        CJQColumn ColSaldo = new CJQColumn();
        ColSaldo.Nombre = "Saldo";
        ColSaldo.Encabezado = "Saldo";
        ColSaldo.Ancho = "400";
        ColSaldo.Alineacion = "left";
        GridAlmacenProductoDetalle.Columnas.Add(ColSaldo);


        //AlmacenDetalle
        CJQColumn ColAlmacenDetalle = new CJQColumn();
        ColAlmacenDetalle.Nombre = "AlmacenDetalle";
        ColAlmacenDetalle.Encabezado = "AlmacenDetalle";
        ColAlmacenDetalle.Ancho = "400";
        ColAlmacenDetalle.Alineacion = "left";
        GridAlmacenProductoDetalle.Columnas.Add(ColAlmacenDetalle);


        //FechaFacturacion
        CJQColumn ColFechaFacturacion = new CJQColumn();
        ColFechaFacturacion.Nombre = "FechaFacturacion";
        ColFechaFacturacion.Encabezado = "FechaFacturacion";
        ColFechaFacturacion.Ancho = "400";
        ColFechaFacturacion.Alineacion = "left";
        GridAlmacenProductoDetalle.Columnas.Add(ColFechaFacturacion);

        //NumeroFactura
        CJQColumn ColNumeroFactura = new CJQColumn();
        ColNumeroFactura.Nombre = "NumeroFactura";
        ColNumeroFactura.Encabezado = "NumeroFactura";
        ColNumeroFactura.Ancho = "400";
        ColNumeroFactura.Alineacion = "left";
        GridAlmacenProductoDetalle.Columnas.Add(ColNumeroFactura);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Ancho = "400";
        ColProducto.Alineacion = "left";
        GridAlmacenProductoDetalle.Columnas.Add(ColProducto);

        //Numero serie
        CJQColumn ColNumeroSerie = new CJQColumn();
        ColNumeroSerie.Nombre = "NumeroSerie";
        ColNumeroSerie.Encabezado = "NumeroSerie";
        ColNumeroSerie.Ancho = "400";
        ColNumeroSerie.Alineacion = "left";
        GridAlmacenProductoDetalle.Columnas.Add(ColNumeroSerie);

        //IdAlmacenOrigen
        CJQColumn ColIdAlmacenOrigen = new CJQColumn();
        ColIdAlmacenOrigen.Nombre = "IdAlmacenOrigen";
        ColIdAlmacenOrigen.Oculto = "true";
        ColIdAlmacenOrigen.Encabezado = "IdAlmacenOrigen";
        ColIdAlmacenOrigen.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColIdAlmacenOrigen);

        //Consultar
        CJQColumn ColConsultarDetalles = new CJQColumn();
        ColConsultarDetalles.Nombre = "Consultar";
        ColConsultarDetalles.Encabezado = "Ver";
        ColConsultarDetalles.Etiquetado = "ImagenConsultar";
        ColConsultarDetalles.Estilo = "divImagenConsultar imgObtenerDatosTraspaso";
        ColConsultarDetalles.Buscador = "false";
        ColConsultarDetalles.Ordenable = "false";
        ColConsultarDetalles.Ancho = "25";
        GridAlmacenProductoDetalle.Columnas.Add(ColConsultarDetalles);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAlmacenProductoDetalle", GridAlmacenProductoDetalle.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAlmacenProducto(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pProducto, string pIdProducto)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEncabezadoRemision", sqlCon);//store q hice resumen
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarClave(string pClave)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoProyecto = new CJson();
        jsonTipoProyecto.StoredProcedure.CommandText = "sp_Producto_ConsultarFiltros";
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pClave));
        return jsonTipoProyecto.ObtenerJsonString(ConexionBaseDatos);
        //asegurarme de regresar lso tres vbalores q pido en js
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTraspaso(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTraspaso, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTraspaso", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTraspaso", SqlDbType.VarChar, 250).Value = Convert.ToString(pTraspaso);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTraspaso(string pTraspaso)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonTraspaso = new CJson();
        jsonTraspaso.StoredProcedure.CommandText = "sp_Traspaso_Consultar_FiltroPorTraspaso";
        jsonTraspaso.StoredProcedure.Parameters.AddWithValue("@pTraspaso", pTraspaso);
        return jsonTraspaso.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTraspaso(Dictionary<string, object> pTraspaso)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTraspaso Traspaso = new CTraspaso();
            //Traspaso.Traspaso = Convert.ToString(pTraspaso["Traspaso"]);

            string validacion = "";//ValidarTraspaso(Traspaso, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Traspaso.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTraspaso(int pIdTraspaso)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTraspaso = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTraspaso" }, ConexionBaseDatos) == "")
        {
            puedeEditarTraspaso = 1;
        }
        oPermisos.Add("puedeEditarTraspaso", puedeEditarTraspaso);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTraspaso Traspaso = new CTraspaso();
            Traspaso.LlenaObjeto(pIdTraspaso, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTraspaso", Traspaso.IdTraspaso));
            //Modelo.Add(new JProperty("Traspaso", Traspaso.Traspaso));

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
    public static string ObtenerFormaEditarTraspaso(int IdTraspaso)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTraspaso = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTraspaso" }, ConexionBaseDatos) == "")
        {
            puedeEditarTraspaso = 1;
        }
        oPermisos.Add("puedeEditarTraspaso", puedeEditarTraspaso);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTraspaso Traspaso = new CTraspaso();
            Traspaso.LlenaObjeto(IdTraspaso, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTraspaso", Traspaso.IdTraspaso));
            //Modelo.Add(new JProperty("Traspaso", Traspaso.Traspaso));
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
    public static string EditarTraspaso(Dictionary<string, object> pTraspaso)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTraspaso Traspaso = new CTraspaso();
        Traspaso.IdTraspaso = Convert.ToInt32(pTraspaso["IdTraspaso"]); ;
        //Traspaso.Traspaso = Convert.ToString(pTraspaso["Traspaso"]);

        string validacion = "";//ValidarTraspaso(Traspaso, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Traspaso.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
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
    public static string CambiarEstatus(int pIdTraspaso, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTraspaso Traspaso = new CTraspaso();
            Traspaso.IdTraspaso = pIdTraspaso;
            //Traspaso.Baja = pBaja;
            Traspaso.Eliminar(ConexionBaseDatos);
            respuesta = "0|TraspasoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    //private static string ValidarTraspaso(CTraspaso pTraspaso, CConexion pConexion)
    //{
    //    string errores = "";
    //    if (pTraspaso.Traspaso == "")
    //    { errores = errores + "<span>*</span> El campo tipo de venta esta vac&iacute;o, favor de capturarlo.<br />"; }

    //    if (errores != "")
    //    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    //    return errores;
    //}

    public static string BuscarProducto(string pProducto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoProyecto = new CJson();
        jsonTipoProyecto.StoredProcedure.CommandText = "sp_Remision_ConsultarProductoNumeroSerie";
        //jsonTipoProyecto.StoredProcedure.Parameters.AddWithValue("@pNumeroSerie", Convert.ToString(pNumeroSerie));
        return jsonTipoProyecto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAlmacenProductoResumen(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdProducto)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAlmacenProductoResumen", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;

        if (pIdProducto != "${IdProducto}")
        {
            Stored.Parameters.Add("pIdProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdProducto);
        }
        else
        {
            Stored.Parameters.Add("pIdProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(0);
        }
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAlmacenProductoDetalle(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdProducto, string pIdAlmacen)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAlmacenProductoDetalle", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;

        if (pIdProducto != "${IdAlmacen}")
        {
            Stored.Parameters.Add("pIdAlmacen", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdAlmacen);
        }
        else
        {
            Stored.Parameters.Add("pIdAlmacen", SqlDbType.VarChar, 250).Value = Convert.ToString(0);
        }

        if (pIdProducto != "${IdProducto}")
        {
            Stored.Parameters.Add("pIdProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdProducto);
        }
        else
        {
            Stored.Parameters.Add("pIdProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(0);
        }

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerListaAlmacenDestino()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerListaAlmacenDestino(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos));
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
    public static string GenerarTraspaso(Dictionary<string, object> pTraspaso)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        int puedeTraspasarProductosSucursales = 1;

        if (respuesta == "Conexion Establecida")
        {
            //valido que el usuario este asignado al almacen origen
            string validacion = ValidaUsuario(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), Convert.ToInt32(pTraspaso["pIdExistenciaDistribuida"]), ConexionBaseDatos);
            if (validacion == "Error")
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "El usuario no esta asignado al almacén origen."));
            }
            else
            {
                CFacturaEncabezado EncabezadoFactura = new CFacturaEncabezado();
                CExistenciaDistribuida ExistenciaDistribuida = new CExistenciaDistribuida();
                CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();

                //Verifico que los datos no vayan vacios y en su caso, en ceros.
                if (Convert.ToString(HttpContext.Current.Session["IdUsuario"]).ToString() != "" && Convert.ToString(HttpContext.Current.Session["IdUsuario"]).ToString() != "0" && Convert.ToString(pTraspaso["pIdAlmacenDestino"]).ToString() != "" && Convert.ToString(pTraspaso["pIdAlmacenDestino"]).ToString() != "0" && Convert.ToString(pTraspaso["pCantidadDeEntrada"]).ToString() != "" && Convert.ToString(pTraspaso["pCantidadDeEntrada"]).ToString() != "0" && Convert.ToString(pTraspaso["pCantidadDeEntrada"]).ToString() != "" && Convert.ToString(pTraspaso["pCantidadDeEntrada"]).ToString() != "0" && Convert.ToString(pTraspaso["pSaldo"]).ToString() != "0" && Convert.ToString(pTraspaso["pSaldo"]).ToString() != "" && Convert.ToString(pTraspaso["pIdExistenciaDistribuida"]).ToString() != "" && Convert.ToString(pTraspaso["pIdExistenciaDistribuida"]).ToString() != "0")
                {

                    //Valido si las sucursales son distintas
                    int validacionSucursales = ValidaSucursal(Convert.ToInt32(pTraspaso["pIdAlmacenOrigen"]), Convert.ToInt32(pTraspaso["pIdAlmacenDestino"]), ConexionBaseDatos);
                    if (validacionSucursales == 0)
                    {
                        //Si resulto ser de distintas sucursales verificamos si tiene permiso
                        JObject oPermisos = new JObject();
                        CUsuario Usuario = new CUsuario();
                        if (Usuario.TienePermisos(new string[] { "puedeTraspasarProductosSucursales" }, ConexionBaseDatos) == "")
                        {
                            puedeTraspasarProductosSucursales = 1;
                        }
                        else
                        {
                            puedeTraspasarProductosSucursales = 0;
                        }
                        oPermisos.Add("puedeEditarTraspaso", puedeTraspasarProductosSucursales);
                    }

                    //Fin de Valido si las sucursales son distintas

                    if (puedeTraspasarProductosSucursales == 1)
                    {
                        //Para hacerlo mas legible, verifico aquí que la cantidad pedida no sea mayor al saldo existente
                        if (Convert.ToInt32(pTraspaso["pSaldo"]) >= Convert.ToInt32(pTraspaso["pCantidadDeEntrada"]) && Convert.ToInt32(pTraspaso["pCantidad"]) >= Convert.ToInt32(pTraspaso["pCantidadDeEntrada"]) && Convert.ToInt32(pTraspaso["pCantidad"]) >= 0 && Convert.ToInt32(pTraspaso["pSaldo"]) >= 0)
                        {
                            ExistenciaDistribuida.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                            ExistenciaDistribuida.IdAlmacen = Convert.ToInt32(pTraspaso["pIdAlmacenDestino"]);
                            ExistenciaDistribuida.Cantidad = Convert.ToInt32(pTraspaso["pCantidadDeEntrada"]);
                            ExistenciaDistribuida.Saldo = Convert.ToInt32(pTraspaso["pSaldo"]);
                            ExistenciaDistribuida.IdExistenciaDistribuida = Convert.ToInt32(pTraspaso["pIdExistenciaDistribuida"]);
                            ExistenciaDistribuida.GenerarTraspaso(ConexionBaseDatos);
                            oRespuesta.Add(new JProperty("Error", 0));
                        }
                        else
                        {
                            oRespuesta.Add(new JProperty("Error", 1));
                            oRespuesta.Add(new JProperty("Descripcion", "La cantidad solicitada no puede ser mayor al saldo disponible del almacén ni a la cantidad detalle."));
                        }
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                        oRespuesta.Add(new JProperty("Descripcion", "No cuenta con los permisos necesarios para traspasar productos de diversas sucursales."));
                    }


                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", "Asegúrese de que todos los datos esten capturados."));
                }

            }
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    private static string ValidaUsuario(int pIdUsuario, int pIdExistenciaDistribuida, CConexion pConexion)
    {
        string errores = "";
        CUsuario Usuario = new CUsuario();
        if (!Usuario.VerificaUsuarioAsignadoAlmacen(pIdUsuario, pIdExistenciaDistribuida, pConexion))
        { errores = errores + "Error"; }

        return errores;
    }

    private static int ValidaSucursal(int pIdAlmacenOrigen, int pIdAlmacenDestino, CConexion pConexion)
    {
        int cantidadRegistros = 0;
        CSelectEspecifico ObtenObjeto = new CSelectEspecifico();
        ObtenObjeto.StoredProcedure.CommandText = "sp_Almacen_Consultar_Sucursales";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdAlmacenOrigen", Convert.ToInt32(pIdAlmacenOrigen));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdAlmacenDestino", Convert.ToInt32(pIdAlmacenDestino));

        ObtenObjeto.Llena(pConexion);
        if (ObtenObjeto.Registros.Read())
        {
            cantidadRegistros = Convert.ToInt32(ObtenObjeto.Registros["Contador"]);
        }
        ObtenObjeto.CerrarConsulta();
        return cantidadRegistros;
    }
}