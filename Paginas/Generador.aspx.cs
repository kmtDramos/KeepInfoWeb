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

public partial class Generador : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridClasesGeneradas
        CJQGrid GridClasesGeneradas = new CJQGrid();
        GridClasesGeneradas.NombreTabla = "grdClasesGeneradas";
        GridClasesGeneradas.CampoIdentificador = "IdClaseGenerador";
        GridClasesGeneradas.ColumnaOrdenacion = "Clase";
        GridClasesGeneradas.Metodo = "ObtenerClasesGeneradas";
        GridClasesGeneradas.TituloTabla = "Catálogo de Clases Generadas";
        GridClasesGeneradas.GenerarFuncionTerminado = true;

        //IdClaseGenerador
        CJQColumn ColIdClaseGenerador = new CJQColumn();
        ColIdClaseGenerador.Nombre = "IdClaseGenerador";
        ColIdClaseGenerador.Oculto = "true";
        ColIdClaseGenerador.Encabezado = "IdClaseGenerador";
        ColIdClaseGenerador.Buscador = "false";
        GridClasesGeneradas.Columnas.Add(ColIdClaseGenerador);

        //Clase
        CJQColumn ColClase = new CJQColumn();
        ColClase.Nombre = "Clase";
        ColClase.Encabezado = "Clase";
        ColClase.Ancho = "320";
        ColClase.Buscador = "true";
        ColClase.Alineacion = "left";
        GridClasesGeneradas.Columnas.Add(ColClase);

        //Abreviatura
        CJQColumn ColAbreviatura = new CJQColumn();
        ColAbreviatura.Nombre = "Abreviatura";
        ColAbreviatura.Encabezado = "Abrev.";
        ColAbreviatura.Ancho = "25";
        ColAbreviatura.Buscador = "false";
        GridClasesGeneradas.Columnas.Add(ColAbreviatura);

        //ManejaBaja
        CJQColumn ColManejaBaja = new CJQColumn();
        ColManejaBaja.Nombre = "ManejaBaja";
        ColManejaBaja.Encabezado = "Maneja baja";
        ColManejaBaja.Buscador = "false";
        ColManejaBaja.Ancho = "40";
        GridClasesGeneradas.Columnas.Add(ColManejaBaja);

        //Bloqueo
        CJQColumn ColBloqueo = new CJQColumn();
        ColBloqueo.Nombre = "Bloqueo";
        ColBloqueo.Encabezado = "B";
        ColBloqueo.Buscador = "false";
        ColBloqueo.Ancho = "25";
        GridClasesGeneradas.Columnas.Add(ColBloqueo);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarClaseGenerada";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridClasesGeneradas.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdClasesGeneradas", GridClasesGeneradas.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerClasesGeneradas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pClase)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdClaseGeneradas", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pClase", SqlDbType.VarChar, 250).Value = pClase;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    //Metodos Ajax
    [WebMethod]
    public static string BuscarClase(string pClase)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonUsuario = new CJson();
        jsonUsuario.StoredProcedure.CommandText = "sp_ClaseGenerador_Consulta";
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@Opcion", 4);
        jsonUsuario.StoredProcedure.Parameters.AddWithValue("@pClase", pClase);
        return jsonUsuario.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarGenerador(Dictionary<string, object> pClaseGenerador)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CClaseGenerador ClaseGenerador = new CClaseGenerador();
        ClaseGenerador.IdClaseGenerador = Convert.ToInt32(pClaseGenerador["IdClaseGenerador"]);
        ClaseGenerador.Clase = Convert.ToString(pClaseGenerador["Clase"]);
        ClaseGenerador.Bloqueo = Convert.ToBoolean(pClaseGenerador["Bloqueo"]);
        ClaseGenerador.ManejaBaja = Convert.ToBoolean(pClaseGenerador["ManejaBaja"]);
        ClaseGenerador.Abreviatura = Convert.ToString(pClaseGenerador["Abreviatura"]);

        List<CClaseAtributo> Atributos = new List<CClaseAtributo>();
        foreach (Dictionary<string, object> OAtributo in (Array)pClaseGenerador["Atributos"])
        {
            CClaseAtributo CClaseAtributo = new CClaseAtributo();
            CClaseAtributo.Atributo = Convert.ToString(OAtributo["Atributo"]);
            CClaseAtributo.TipoAtributo = Convert.ToString(OAtributo["TipoAtributo"]);
            CClaseAtributo.LlavePrimaria = Convert.ToString(OAtributo["LlavePrimaria"]).ToLower().Trim();
            CClaseAtributo.Identidad = Convert.ToString(OAtributo["Identidad"]).ToLower().Trim();
            CClaseAtributo.Longitud = Convert.ToString(OAtributo["Longitud"]);
            CClaseAtributo.Decimales = Convert.ToString(OAtributo["NumeroDecimales"]);
            CClaseAtributo.IdClaseGenerador = ClaseGenerador.IdClaseGenerador;
            Atributos.Add(CClaseAtributo);
        }

        string validacion = ValidaClaseGenerador(ClaseGenerador, Atributos, ConexionBaseDatos);
        JObject o = new JObject();
        o.Add(new JProperty("Error", 0));
        if (validacion == "")
        {
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("IdClaseGenerador", ClaseGenerador.IdClaseGenerador));

            ClaseGenerador.CrearTabla(ClaseGenerador, Atributos, ConexionBaseDatos);
            ClaseGenerador.CrearStoredProcedures(ClaseGenerador, Atributos, ConexionBaseDatos);
            ClaseGenerador.CrearClase(ClaseGenerador, Atributos, ConexionBaseDatos);
            ClaseGenerador.CrearClaseConfigurable(ClaseGenerador, ConexionBaseDatos);
            ClaseGenerador.Agregar(ConexionBaseDatos);

            foreach (CClaseAtributo OAtributo in Atributos)
            {
                OAtributo.IdClaseGenerador = ClaseGenerador.IdClaseGenerador;
                OAtributo.Agregar(ConexionBaseDatos);
            }

            o.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            o["Error"] = 1;
            o.Add(new JProperty("Descripcion", validacion));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return o.ToString();
    }

    [WebMethod]
    public static string EditarGenerador(Dictionary<string, object> pClaseGenerador)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CClaseGenerador ClaseGenerador = new CClaseGenerador();
        ClaseGenerador.IdClaseGenerador = Convert.ToInt32(pClaseGenerador["IdClaseGenerador"]);
        ClaseGenerador.Clase = Convert.ToString(pClaseGenerador["Clase"]);
        ClaseGenerador.Bloqueo = Convert.ToBoolean(pClaseGenerador["Bloqueo"]);
        ClaseGenerador.ManejaBaja = Convert.ToBoolean(pClaseGenerador["ManejaBaja"]);
        ClaseGenerador.Abreviatura = Convert.ToString(pClaseGenerador["Abreviatura"]);
        List<CClaseAtributo> Atributos = new List<CClaseAtributo>();
        foreach (Dictionary<string, object> OAtributo in (Array)pClaseGenerador["Atributos"])
        {
            CClaseAtributo CClaseAtributo = new CClaseAtributo();
            CClaseAtributo.IdClaseAtributo = Convert.ToInt32(OAtributo["IdClaseAtributo"]);
            CClaseAtributo.Atributo = Convert.ToString(OAtributo["Atributo"]);
            CClaseAtributo.TipoAtributo = Convert.ToString(OAtributo["TipoAtributo"]);
            CClaseAtributo.LlavePrimaria = Convert.ToString(OAtributo["LlavePrimaria"]).ToLower().Trim();
            CClaseAtributo.Identidad = Convert.ToString(OAtributo["Identidad"]).ToLower().Trim();
            CClaseAtributo.Longitud = Convert.ToString(OAtributo["Longitud"]);
            CClaseAtributo.Decimales = Convert.ToString(OAtributo["NumeroDecimales"]);
            CClaseAtributo.IdClaseGenerador = ClaseGenerador.IdClaseGenerador;
            Atributos.Add(CClaseAtributo);
        }

        string validacion = ValidaClaseGenerador(ClaseGenerador, Atributos, ConexionBaseDatos);
        JObject o = new JObject();
        o.Add(new JProperty("Error", 0));
        if (validacion == "")
        {
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("IdClaseGenerador", ClaseGenerador.IdClaseGenerador));
            ClaseGenerador.EditarTabla(ClaseGenerador, Atributos, Convert.ToString(pClaseGenerador["ClaseAnterior"]), ConexionBaseDatos);
            ClaseGenerador.EliminarStoredProcedures(ClaseGenerador, ConexionBaseDatos);

            if (ClaseGenerador.CrearStoredProcedures(ClaseGenerador, Atributos, ConexionBaseDatos) == "")
            {
                o["Error"] = 1;
                o.Add(new JProperty("Descripcion", "<span>*</span> Ocurrio un error al editar los stored procedures."));
            }

            if ((int)o["Error"] == 0)
            {
                ClaseGenerador.CrearClase(ClaseGenerador, Atributos, ConexionBaseDatos);
            }

            ClaseGenerador.Editar(ConexionBaseDatos);
            CClaseAtributo ObtenerAtributos = new CClaseAtributo();
            List<CClaseAtributo> AtributosActuales = new List<CClaseAtributo>();

            foreach (CClaseAtributo Atributo in ObtenerAtributos.LlenaObjetos_FiltroIdClaseGenerador(ClaseGenerador.IdClaseGenerador, ConexionBaseDatos))
            {
                AtributosActuales.Add(Atributo);
            }

            foreach (CClaseAtributo AtributoActual in AtributosActuales)
            {
                bool eliminar = true;
                foreach (CClaseAtributo Atributo in Atributos)
                {
                    if (AtributoActual.IdClaseAtributo == Atributo.IdClaseAtributo)
                    {
                        eliminar = false;
                    }

                    if (eliminar == false)
                    {
                        break;
                    }
                }

                if (eliminar == true)
                {
                    AtributoActual.Eliminar(ConexionBaseDatos);
                }
            }

            ClaseGenerador.Editar(ConexionBaseDatos);
            foreach (CClaseAtributo Atributo in Atributos)
            {
                if (Atributo.IdClaseAtributo == 0)
                {
                    Atributo.Agregar(ConexionBaseDatos);
                }
                else
                {
                    Atributo.Editar(ConexionBaseDatos);
                }
            }

            o.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            o.Add(new JProperty("Error", 1));
            o.Add(new JProperty("Descripcion", validacion));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return o.ToString();
    }

    [WebMethod]
    public static string ObtenerClaseGenerador(int pIdClaseGenerador)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        bool puedeEditarClasesBloqueadas = false;
        JObject o = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarClasesBloquedas" }, ConexionBaseDatos) == "")
        {
            puedeEditarClasesBloqueadas = true;
        }
        oPermisos.Add("PuedeEditarClasesBloqueadas", puedeEditarClasesBloqueadas);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CClaseGenerador ClaseGenerador = new CClaseGenerador();
            ClaseGenerador.LlenaObjeto(pIdClaseGenerador, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdClaseGenerador", ClaseGenerador.IdClaseGenerador));
            Modelo.Add(new JProperty("Clase", ClaseGenerador.Clase));
            Modelo.Add(new JProperty("Bloqueo", ClaseGenerador.Bloqueo));
            Modelo.Add(new JProperty("ManejaBaja", ClaseGenerador.ManejaBaja));
            Modelo.Add(new JProperty("Abreviatura", ClaseGenerador.Abreviatura));

            JArray Atributos = new JArray();
            CClaseAtributo ClaseAtributo = new CClaseAtributo();
            ClaseAtributo.IdClaseGenerador = ClaseGenerador.IdClaseGenerador;
            foreach (CClaseAtributo OCA in ClaseAtributo.LlenaObjetos_FiltroIdClaseGenerador(ClaseGenerador.IdClaseGenerador, ConexionBaseDatos))
            {
                JObject Atributo = new JObject();
                Atributo.Add(new JProperty("IdClaseAtributo", OCA.IdClaseAtributo));
                Atributo.Add(new JProperty("Atributo", OCA.Atributo));
                Atributo.Add(new JProperty("TipoAtributo", OCA.TipoAtributo));
                Atributo.Add(new JProperty("Longitud", OCA.Longitud));
                Atributo.Add(new JProperty("Decimales", OCA.Decimales));
                Atributo.Add(new JProperty("LlavePrimaria", OCA.LlavePrimaria));
                Atributo.Add(new JProperty("Identidad", OCA.Identidad));
                Atributos.Add(Atributo);
            }
            Modelo.Add(new JProperty("Atributos", Atributos));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            o.Add(new JProperty("Error", 0));
            o.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            o.Add(new JProperty("Error", 1));
            o.Add(new JProperty("Descripcion", "Esto fue un error"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return o.ToString();
    }

    //Validaciones
    public static string ValidaClaseGenerador(CClaseGenerador pClaseGenerador, List<CClaseAtributo> pAtributos, CConexion pConexion)
    {
        string errores = "";
        bool bEntro = false;
        bool bAtributo = false;
        bool bLongitud = false;
        bool bLongitudString = false;
        bool bLongitudDecimal = false;
        bool bDecimal = false;
        bool bBaja = false;

        if (pClaseGenerador.IdClaseGenerador == 0)
        {
            if (pClaseGenerador.ValidaNombreClase(pClaseGenerador.Clase, pConexion))
            { errores = errores + "<span>*</span> El campo nombre de la clase ya existe, favor de cambiarlo.<br />"; }
        }
        else
        {
            if (pClaseGenerador.ValidaNombreClase(pClaseGenerador.Clase, pClaseGenerador.IdClaseGenerador, pConexion))
            { errores = errores + "<span>*</span> El campo nombre de la clase ya existe, favor de cambiarlo.<br />"; }
        }

        if (pClaseGenerador.Clase == "")
        { errores = errores + "<span>*</span> El campo nombre de la clase esta vac&iacute;o, favor de capturarlo.<br />"; }

        foreach (CClaseAtributo OAtributo in pAtributos)
        {
            bEntro = true;
            if ((OAtributo.Atributo == "" || OAtributo.Atributo == null) && bAtributo == false)
            {
                errores = errores + "<span>*</span> Todos los campos de los atributos son obligatorios, favor de capturarlos.<br />";
                bAtributo = true;
            }

            if (OAtributo.Atributo != null)
            {
                if (OAtributo.Atributo.ToLower() == "baja" && bBaja == false && pClaseGenerador.ManejaBaja == true)
                {
                    errores = errores + "<span>*</span> No puedes nombrar 'Baja' a un atributo si esta activada la opción Baja, favor de cambiarlo.<br />";
                    bBaja = true;
                }

                if (OAtributo.Atributo.ToLower() == "id" + pClaseGenerador.Clase.ToLower())
                {
                    errores = errores + "<span>*</span> No puedes nombrar el 'id' de la clase, favor de cambiarlo.<br />";
                    bBaja = true;
                }
            }

            if ((OAtributo.TipoAtributo == "S" || OAtributo.TipoAtributo == "D") && OAtributo.Longitud == "0" && bLongitud == false)
            {
                errores = errores + "<span>*</span> Los campos longitud no pueden tener valor 0.<br />";
                bLongitud = true;
            }

            if (OAtributo.TipoAtributo == "S" && Convert.ToInt32(OAtributo.Longitud) > 8000)
            {
                errores = errores + "<span>*</span> Los campos longitud de tipo string no pueden ser mayor a 8000.<br />";
                bLongitudString = true;
            }

            if (OAtributo.TipoAtributo == "D" && Convert.ToInt32(OAtributo.Longitud) > 18)
            {
                errores = errores + "<span>*</span> Los campos longitud de tipo decimal no pueden ser mayor a 18.<br />";
                bLongitudDecimal = true;
            }

            if (OAtributo.TipoAtributo == "D" && Convert.ToInt32(OAtributo.Decimales) == 0)
            {
                errores = errores + "<span>*</span> Los datos tipo decimal no pueden tener 0 decimales, capturalos o cambia a tipo integer.<br />";
                bDecimal = true;
            }
            else if (OAtributo.TipoAtributo == "D" && Convert.ToInt32(OAtributo.Longitud) < Convert.ToInt32(OAtributo.Decimales))
            {
                errores = errores + "<span>*</span> Los campos decimal no pueden ser mayor que el campo longitud del atributo.<br />";
                bDecimal = true;
            }

            if (bAtributo == true && bLongitud == true && bLongitudDecimal == true && bLongitudString == true && bBaja == true && bDecimal == true)
            {
                break;
            }
        }

        if (bEntro == false)
        { errores = errores + "<span>*</span> No agrego ningun atributo a la clase.<br />"; }
        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}