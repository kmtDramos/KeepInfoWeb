using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
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
using System.IO;

public partial class Paginas_SolicitudPago : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        CJQGrid GridSolicitudPago = new CJQGrid();
        GridSolicitudPago.NombreTabla = "grdSolicitudPago";
        GridSolicitudPago.CampoIdentificador = "IdSolicitudPag";
        GridSolicitudPago.ColumnaOrdenacion = "IdSolicitudPag";
        GridSolicitudPago.TipoOrdenacion = "DESC";
        GridSolicitudPago.Metodo = "ObtenerSolicitudPago";
        GridSolicitudPago.TituloTabla = "Solicitudes de pago";
        //GridSolicitudPago.GenerarGridCargaInicial = false;
        GridSolicitudPago.GenerarFuncionFiltro = false;
        GridSolicitudPago.GenerarFuncionTerminado = false;
        GridSolicitudPago.NumeroFila = false;
        GridSolicitudPago.Altura = 350;
        GridSolicitudPago.Ancho = 940;
        GridSolicitudPago.NumeroRegistros = 25;
        GridSolicitudPago.RangoNumeroRegistros = "25,40,100";


        CJQColumn ColIdSolicitudPago = new CJQColumn();
        ColIdSolicitudPago.Nombre = "IdSolicitudPago";
        ColIdSolicitudPago.Encabezado = "Folio";
        ColIdSolicitudPago.Ancho = "70";
        GridSolicitudPago.Columnas.Add(ColIdSolicitudPago);

        CJQColumn ColProveedor = new CJQColumn();
        ColProveedor.Nombre = "Proveedor";
        ColProveedor.Encabezado = "Proveedor";
        ColProveedor.Ancho = "180";
        GridSolicitudPago.Columnas.Add(ColProveedor);

        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Ancho = "120";
        ColMonto.Buscador = "false";
        GridSolicitudPago.Columnas.Add(ColMonto);

        CJQColumn ColFechaRequerida = new CJQColumn();
        ColFechaRequerida.Nombre = "FechaRequerida";
        ColFechaRequerida.Encabezado = "Fecha Req.";
        ColFechaRequerida.Ancho = "100";
        ColFechaRequerida.Buscador = "false";
        GridSolicitudPago.Columnas.Add(ColFechaRequerida);

        CJQColumn ColEstatus = new CJQColumn();
        ColEstatus.Nombre = "Pagada";
        ColEstatus.Encabezado = "Estatus";
        ColEstatus.Ancho = "120";
        GridSolicitudPago.Columnas.Add(ColEstatus);

        CJQColumn ColDias = new CJQColumn();
        ColDias.Nombre = "Dias";
        ColDias.Encabezado = "Dias";
        ColDias.Ancho = "70";
        ColDias.Buscador = "false";
        GridSolicitudPago.Columnas.Add(ColDias);

        ClientScript.RegisterStartupScript(Page.GetType(), "grdSolicitudPago", GridSolicitudPago.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudPago(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCotizador", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatos();
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string GuardarSolicitudPago(int IdProveedor, decimal Monto, string FechaRequerida, int IdOportunidad)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSolicitudPago SolicitudPago = new CSolicitudPago();
                SolicitudPago.IdProveedor = IdProveedor;
                SolicitudPago.Monto = Monto;
                SolicitudPago.FechaCreacion = DateTime.Now;
                SolicitudPago.FechaRequerida = Convert.ToDateTime(FechaRequerida);
                SolicitudPago.IdUsuario = UsuarioSesion.IdUsuario;
                SolicitudPago.IdOportunidad = IdOportunidad;

                SolicitudPago.Agregar(pConexion);

                Respuesta.Add("Modelo", Modelo);
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string BuscarProveedor(string Proveedor)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_BuscarProveedor_Cotizador";
                Consulta.StoredProcedure.Parameters.Add("Proveedor", SqlDbType.VarChar, 50).Value = Proveedor;

                Consulta.Llena(pConexion);

                JArray Proveedores = new JArray();

                while (Consulta.Registros.Read())
                {
                    JObject Opcion = new JObject();
                    Opcion.Add("IdProveedor", Convert.ToInt32(Consulta.Registros["IdProveedor"]));
                    Opcion.Add("Proveedor", Convert.ToString(Consulta.Registros["RazonSocial"]));
                    Proveedores.Add(Opcion);
                }

                Modelo.Add("Table", Proveedores);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

}