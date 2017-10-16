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

public partial class Estado : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //GridEstado
        CJQGrid GridEstado = new CJQGrid();
        GridEstado.NombreTabla = "grdEstado";
        GridEstado.CampoIdentificador = "IdEstado";
        GridEstado.ColumnaOrdenacion = "Estado";
        GridEstado.Metodo = "ObtenerEstado";
        GridEstado.TituloTabla = "Catálogo de estados";

        //IdEstado
        CJQColumn ColIdEstado = new CJQColumn();
        ColIdEstado.Nombre = "IdEstado";
        ColIdEstado.Encabezado = "IdEstado";
        ColIdEstado.Oculto = "true";
        ColIdEstado.Buscador = "false";
        GridEstado.Columnas.Add(ColIdEstado);

        //Estado
        CJQColumn ColEstado = new CJQColumn();
        ColEstado.Nombre = "Estado";
        ColEstado.Encabezado = "Estado";
        ColEstado.Buscador = "true";
        ColEstado.Alineacion = "left";
        ColEstado.Ancho = "100";
        GridEstado.Columnas.Add(ColEstado);

        //Abreviatura
        CJQColumn ColAbreviatura = new CJQColumn();
        ColAbreviatura.Nombre = "Abreviatura";
        ColAbreviatura.Encabezado = "Abreviatura";
        ColAbreviatura.Buscador = "false";
        ColAbreviatura.Alineacion = "left";
        ColAbreviatura.Ancho = "75";
        GridEstado.Columnas.Add(ColAbreviatura);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Buscador = "false";
        ColClave.Alineacion = "left";
        ColClave.Ancho = "75";
        GridEstado.Columnas.Add(ColClave);

        //Pais
        CJQColumn ColPais = new CJQColumn();
        ColPais.Nombre = "Pais";
        ColPais.Encabezado = "Pais";
        ColPais.Buscador = "true";
        ColPais.Alineacion = "left";
        ColPais.Ancho = "75";
        GridEstado.Columnas.Add(ColPais);

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
        GridEstado.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarEstado";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridEstado.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdEstado", GridEstado.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerEstado(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pEstado, string pPais, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEstado", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pEstado", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstado);
        Stored.Parameters.Add("pPais", SqlDbType.VarChar, 250).Value = Convert.ToString(pEstado);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaAgregarEstado()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add(new JProperty("Paises", CPais.ObtenerJsonPaises(ConexionBaseDatos)));
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

    [WebMethod]//
    public static string ObtenerFormaEstado(int pIdEstado)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEstado = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CEstado Estado = new CEstado();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarEstado" }, ConexionBaseDatos) == "")
        {
            puedeEditarEstado = 1;
        }
        oPermisos.Add("puedeEditarEstado", puedeEditarEstado);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Estado.LlenaObjeto(pIdEstado, ConexionBaseDatos);
            CPais Pais = new CPais();
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);

            Modelo.Add("IdEstado", Estado.IdEstado);
            Modelo.Add("Estado", Estado.Estado);
            Modelo.Add("Abreviatura", Estado.Abreviatura);
            Modelo.Add("Clave", Estado.Clave);
            Modelo.Add("Pais", Pais.Pais);

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

    [WebMethod]// en este metodo obtien los datos para la pantalla de consulta
    public static string ObtenerFormaEditarEstado(int IdEstado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEstado = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CEstado Estado = new CEstado();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEstado" }, ConexionBaseDatos) == "")
        {
            puedeEditarEstado = 1;
        }
        oPermisos.Add("puedeEditarEstado", puedeEditarEstado);

        if (respuesta == "Conexion Establecida")
        {
            Estado.LlenaObjeto(IdEstado, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("IdEstado", Estado.IdEstado));
            Modelo.Add(new JProperty("Estado", Estado.Estado));
            Modelo.Add(new JProperty("Abreviatura", Estado.Abreviatura));
            Modelo.Add(new JProperty("Clave", Estado.Clave));

            Modelo.Add("Paises", CPais.ObtenerJsonPaises(Estado.IdPais, ConexionBaseDatos));

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
    public static string AgregarEstado(Dictionary<string, object> pEstado)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            CEstado Estado = new CEstado();
            Estado.Estado = pEstado["Estado"].ToString();
            Estado.Abreviatura = pEstado["Abreviatura"].ToString();
            Estado.Clave = pEstado["Clave"].ToString();
            Estado.IdPais = Convert.ToInt32(pEstado["IdPais"]);

            string validacion = ValidarEstado(Estado);
            if (validacion == "")
            {
                Estado.Agregar(ConexionBaseDatos);
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
    public static string EditarEstado(Dictionary<string, object> pEstado)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CEstado Estado = new CEstado();
        Estado.LlenaObjeto(Convert.ToInt32(pEstado["IdEstado"]), ConexionBaseDatos);
        Estado.Estado = pEstado["Estado"].ToString();
        Estado.Abreviatura = pEstado["Abreviatura"].ToString();
        Estado.Clave = pEstado["Clave"].ToString();
        Estado.IdPais = Convert.ToInt32(pEstado["IdPais"]);


        string validacion = ValidarEstado(Estado);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Estado.Editar(ConexionBaseDatos);

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
    public static string CambiarEstatus(int pIdEstado, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEstado Estado = new CEstado();
            Estado.IdEstado = pIdEstado;
            Estado.Baja = pBaja;
            Estado.Eliminar(ConexionBaseDatos);
            respuesta = "0|EstadoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarEstado(CEstado pEstado)
    {
        string errores = "";
        if (pEstado.Estado == "")
        { errores = errores + "<span>*</span> El campo estado esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pEstado.Abreviatura == "")
        { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pEstado.IdPais == 0)
        { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}