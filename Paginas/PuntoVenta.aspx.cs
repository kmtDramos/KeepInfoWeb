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

public partial class Paginas_PuntoVenta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //GridDetalleVenta
        CJQGrid GridDetalleVenta = new CJQGrid();
        GridDetalleVenta.NombreTabla = "grdDetalleVenta";
        GridDetalleVenta.CampoIdentificador = "IdDetalleVenta";
        GridDetalleVenta.ColumnaOrdenacion = "IdDetalleVenta";
        GridDetalleVenta.Metodo = "ObtenerDetalleVenta";
        GridDetalleVenta.TituloTabla = "Productos";
        GridDetalleVenta.Ancho = 500;

        //IdDetalleVenta
        CJQColumn ColIdDetalleVenta = new CJQColumn();
        ColIdDetalleVenta.Nombre = "IdDetalleVenta";
        ColIdDetalleVenta.Oculto = "true";
        ColIdDetalleVenta.Encabezado = "IdDetalleVenta";
        ColIdDetalleVenta.Buscador = "false";
        GridDetalleVenta.Columnas.Add(ColIdDetalleVenta);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Ancho = "200";
        ColProducto.Alineacion = "left";
        ColProducto.Buscador = "false";
        GridDetalleVenta.Columnas.Add(ColProducto);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Ancho = "50";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        GridDetalleVenta.Columnas.Add(ColCantidad);

        //ColPrecioUnitario
        CJQColumn ColPrecioUnitario = new CJQColumn();
        ColPrecioUnitario.Nombre = "PrecioUnitario";
        ColPrecioUnitario.Encabezado = "P.U.";
        ColPrecioUnitario.Ordenable = "false";
        ColPrecioUnitario.Ancho = "55";
        ColPrecioUnitario.Buscador = "false";
        GridDetalleVenta.Columnas.Add(ColPrecioUnitario);

        //ColImporte
        CJQColumn ColImporte = new CJQColumn();
        ColImporte.Nombre = "Importe";
        ColImporte.Encabezado = "Importe";
        ColImporte.Ordenable = "false";
        ColImporte.Ancho = "55";
        ColImporte.Buscador = "false";
        GridDetalleVenta.Columnas.Add(ColImporte);

        //ColCancelar
        CJQColumn ColCancelar = new CJQColumn();
        ColCancelar.Nombre = "Cancelar";
        ColCancelar.Encabezado = "C";
        ColCancelar.Ordenable = "false";
        ColCancelar.Ancho = "20";
        ColCancelar.Buscador = "false";
        GridDetalleVenta.Columnas.Add(ColCancelar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleVenta", GridDetalleVenta.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleVenta(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleVenta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }
}