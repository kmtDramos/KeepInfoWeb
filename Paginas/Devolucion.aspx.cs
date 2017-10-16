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

public partial class Devolucion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridAlmacenProducto
        CJQGrid GridDevolucion = new CJQGrid();
        GridDevolucion.NombreTabla = "grdDevolucion";
        GridDevolucion.CampoIdentificador = "IdDevolucion";
        GridDevolucion.ColumnaOrdenacion = "Producto";
        GridDevolucion.Metodo = "ObtenerDevolucion";
        GridDevolucion.TituloTabla = "Transacción de devoluciones";
        GridDevolucion.GenerarGridCargaInicial = false;
        GridDevolucion.GenerarFuncionFiltro = true;

        //IdDevolucion
        CJQColumn ColIdDevolucion = new CJQColumn();
        ColIdDevolucion.Nombre = "IdDevolucion";
        ColIdDevolucion.Oculto = "true";
        ColIdDevolucion.Encabezado = "IdDevolucion";
        ColIdDevolucion.Buscador = "false";
        GridDevolucion.Columnas.Add(ColIdDevolucion);

        //IdProducto
        CJQColumn ColIdProducto = new CJQColumn();
        ColIdProducto.Nombre = "IdProducto";
        ColIdProducto.Oculto = "true";
        ColIdProducto.Encabezado = "IdProducto";
        ColIdProducto.Buscador = "false";
        GridDevolucion.Columnas.Add(ColIdProducto);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Ancho = "400";
        ColClave.Alineacion = "left";
        ColClave.Buscador = "false";
        GridDevolucion.Columnas.Add(ColClave);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Ancho = "400";
        ColProducto.Alineacion = "left";
        GridDevolucion.Columnas.Add(ColProducto);

        //Nota de cédito
        CJQColumn ColNotaCredito = new CJQColumn();
        ColNotaCredito.Nombre = "NotaCredito";
        ColNotaCredito.Encabezado = "Nota de crédito";
        ColNotaCredito.Ancho = "200";
        ColNotaCredito.Alineacion = "left";
        ColNotaCredito.Buscador = "false";
        GridDevolucion.Columnas.Add(ColNotaCredito);

        //Numero de factura
        CJQColumn ColFactura = new CJQColumn();
        ColFactura.Nombre = "NumeroFactura";
        ColFactura.Encabezado = "Factura";
        ColFactura.Ancho = "200";
        ColFactura.Alineacion = "left";
        ColFactura.Buscador = "false";
        GridDevolucion.Columnas.Add(ColFactura);

        ////Numero serie
        //CJQColumn ColNumeroSerie = new CJQColumn();
        //ColNumeroSerie.Nombre = "NumeroSerie";
        //ColNumeroSerie.Encabezado = "Numero de serie";
        //ColNumeroSerie.Ancho = "400";
        //ColNumeroSerie.Alineacion = "center";
        //GridDevolucion.Columnas.Add(ColNumeroSerie);

        //Devolucion
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Devolucion";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgGenerarDevolucion";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridDevolucion.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDevolucion", GridDevolucion.GeneraGrid(), true);

    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDevolucion(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pProducto)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDevolucion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(pProducto);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string GenerarDevolucion(Dictionary<string, object> pDevolucion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CDevolucion Devolucion = new CDevolucion();

            Devolucion.IdUsuarioEntrada = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            Devolucion.IdDevolucion = Convert.ToInt32(pDevolucion["pIdDevolucion"]);
            Devolucion.GenerarDevolucion(ConexionBaseDatos);
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