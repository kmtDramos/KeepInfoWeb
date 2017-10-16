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

public partial class ReporteExistenciaPorProducto : System.Web.UI.Page
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
        ColAlmacen.Buscador = "false";
        ColAlmacen.Alineacion = "left";
        GridAlmacenProductoResumen.Columnas.Add(ColAlmacen);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Saldo";
        ColCantidad.Encabezado = "Saldo";
        ColCantidad.Ancho = "400";
        ColCantidad.Alineacion = "center";
        ColCantidad.Buscador = "false";
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
        ColCantidadDetalle.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColCantidadDetalle);

        //Saldo
        CJQColumn ColSaldo = new CJQColumn();
        ColSaldo.Nombre = "Saldo";
        ColSaldo.Encabezado = "Saldo";
        ColSaldo.Ancho = "400";
        ColSaldo.Alineacion = "left";
        ColSaldo.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColSaldo);


        //AlmacenDetalle
        CJQColumn ColAlmacenDetalle = new CJQColumn();
        ColAlmacenDetalle.Nombre = "AlmacenDetalle";
        ColAlmacenDetalle.Encabezado = "AlmacenDetalle";
        ColAlmacenDetalle.Ancho = "400";
        ColAlmacenDetalle.Alineacion = "left";
        ColAlmacenDetalle.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColAlmacenDetalle);


        //FechaFacturacion
        CJQColumn ColFechaFacturacion = new CJQColumn();
        ColFechaFacturacion.Nombre = "FechaFacturacion";
        ColFechaFacturacion.Encabezado = "FechaFacturacion";
        ColFechaFacturacion.Ancho = "400";
        ColFechaFacturacion.Alineacion = "left";
        ColFechaFacturacion.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColFechaFacturacion);

        //NumeroFactura
        CJQColumn ColNumeroFactura = new CJQColumn();
        ColNumeroFactura.Nombre = "NumeroFactura";
        ColNumeroFactura.Encabezado = "NumeroFactura";
        ColNumeroFactura.Ancho = "400";
        ColNumeroFactura.Alineacion = "left";
        ColNumeroFactura.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColNumeroFactura);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Ancho = "400";
        ColProducto.Alineacion = "left";
        ColProducto.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColProducto);

        //Numero serie
        CJQColumn ColNumeroSerie = new CJQColumn();
        ColNumeroSerie.Nombre = "NumeroSerie";
        ColNumeroSerie.Encabezado = "NumeroSerie";
        ColNumeroSerie.Ancho = "400";
        ColNumeroSerie.Alineacion = "left";
        ColNumeroSerie.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColNumeroSerie);

        //IdAlmacenOrigen
        CJQColumn ColIdAlmacenOrigen = new CJQColumn();
        ColIdAlmacenOrigen.Nombre = "IdAlmacenOrigen";
        ColIdAlmacenOrigen.Oculto = "true";
        ColIdAlmacenOrigen.Encabezado = "IdAlmacenOrigen";
        ColIdAlmacenOrigen.Buscador = "false";
        GridAlmacenProductoDetalle.Columnas.Add(ColIdAlmacenOrigen);

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
}