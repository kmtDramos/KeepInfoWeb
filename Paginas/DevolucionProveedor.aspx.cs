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

public partial class DevolucionProveedor : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridAlmacenProducto
        CJQGrid GridDevolucionProveedor = new CJQGrid();
        GridDevolucionProveedor.NombreTabla = "grdDevolucionProveedor";
        GridDevolucionProveedor.CampoIdentificador = "IdDevolucionProveedor";
        GridDevolucionProveedor.ColumnaOrdenacion = "Producto";
        GridDevolucionProveedor.Metodo = "ObtenerDevolucionProveedor";
        GridDevolucionProveedor.TituloTabla = "Transacción de Devoluciones de proveedores";
        GridDevolucionProveedor.GenerarGridCargaInicial = false;
        GridDevolucionProveedor.GenerarFuncionFiltro = true;

        //IdDevolucionProveedor
        CJQColumn ColIdDevolucionProveedor = new CJQColumn();
        ColIdDevolucionProveedor.Nombre = "IdDevolucionProveedor";
        ColIdDevolucionProveedor.Oculto = "true";
        ColIdDevolucionProveedor.Encabezado = "IdDevolucionProveedor";
        ColIdDevolucionProveedor.Buscador = "false";
        GridDevolucionProveedor.Columnas.Add(ColIdDevolucionProveedor);

        //IdProducto
        CJQColumn ColIdProducto = new CJQColumn();
        ColIdProducto.Nombre = "IdProducto";
        ColIdProducto.Oculto = "true";
        ColIdProducto.Encabezado = "IdProducto";
        ColIdProducto.Buscador = "false";
        GridDevolucionProveedor.Columnas.Add(ColIdProducto);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Ancho = "200";
        ColProducto.Alineacion = "left";
        GridDevolucionProveedor.Columnas.Add(ColProducto);

        //Nota de cédito
        CJQColumn ColNotaCredito = new CJQColumn();
        ColNotaCredito.Nombre = "NotaCredito";
        ColNotaCredito.Encabezado = "Nota de crédito";
        ColNotaCredito.Ancho = "200";
        ColNotaCredito.Alineacion = "left";
        ColNotaCredito.Buscador = "false";
        GridDevolucionProveedor.Columnas.Add(ColNotaCredito);

        //Numero de factura
        CJQColumn ColFactura = new CJQColumn();
        ColFactura.Nombre = "NumeroFactura";
        ColFactura.Encabezado = "Factura";
        ColFactura.Ancho = "200";
        ColFactura.Alineacion = "left";
        ColFactura.Buscador = "false";
        GridDevolucionProveedor.Columnas.Add(ColFactura);

        //Numero serie
        CJQColumn ColNumeroSerie = new CJQColumn();
        ColNumeroSerie.Nombre = "NumeroSerie";
        ColNumeroSerie.Encabezado = "Número de serie";
        ColNumeroSerie.Ancho = "200";
        ColNumeroSerie.Alineacion = "center";
        GridDevolucionProveedor.Columnas.Add(ColNumeroSerie);

        //DevolucionProveedor
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "DevolucionProveedor";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgGenerarDevolucionProveedor";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "100";
        GridDevolucionProveedor.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDevolucionProveedor", GridDevolucionProveedor.GeneraGrid(), true);

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDevolucionProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNumeroSerie, string pProducto)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDevolucionProveedor", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNumeroSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroSerie);
        Stored.Parameters.Add("pProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(pProducto);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string GenerarDevolucionProveedor(Dictionary<string, object> pDevolucionProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CDevolucionProveedor DevolucionProveedor = new CDevolucionProveedor();
            DevolucionProveedor.IdUsuarioEntrada = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            DevolucionProveedor.IdDevolucionProveedor = Convert.ToInt32(pDevolucionProveedor["pIdDevolucionProveedor"]);
            DevolucionProveedor.GenerarDevolucionProveedor(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }
}