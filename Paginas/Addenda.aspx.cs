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
using System.Xml;

public partial class Addenda : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //CrearDocumentoXML();
        //LeerXML();
        //GridAddenda
        CJQGrid GridAddenda = new CJQGrid();
        GridAddenda.NombreTabla = "grdAddenda";
        GridAddenda.CampoIdentificador = "IdAddenda";
        GridAddenda.ColumnaOrdenacion = "Addenda";
        GridAddenda.Metodo = "ObtenerAddenda";
        GridAddenda.TituloTabla = "Catálogo de addenda";

        //IdAddenda
        CJQColumn ColIdAddenda = new CJQColumn();
        ColIdAddenda.Nombre = "IdAddenda";
        ColIdAddenda.Oculto = "true";
        ColIdAddenda.Encabezado = "IdAddenda";
        ColIdAddenda.Buscador = "false";
        GridAddenda.Columnas.Add(ColIdAddenda);

        //Addenda
        CJQColumn ColAddenda = new CJQColumn();
        ColAddenda.Nombre = "Addenda";
        ColAddenda.Encabezado = "Addenda";
        ColAddenda.Ancho = "400";
        ColAddenda.Alineacion = "left";
        GridAddenda.Columnas.Add(ColAddenda);

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
        GridAddenda.Columnas.Add(ColBaja);


        //Estructura
        CJQColumn ColEstructura = new CJQColumn();
        ColEstructura.Nombre = "Estructura";
        ColEstructura.Encabezado = "Estructura";
        ColEstructura.Etiquetado = "Imagen";
        ColEstructura.Imagen = "SerieFactura.png";
        ColEstructura.Estilo = "divImagenEstructuraAddenda imgFormaConsultarEstructuraAddenda";
        ColEstructura.Buscador = "false";
        ColEstructura.Ordenable = "false";
        ColEstructura.Ancho = "70";
        GridAddenda.Columnas.Add(ColEstructura);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarAddenda";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridAddenda.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdAddenda", GridAddenda.GeneraGrid(), true);

        //GridEstructuraAddenda
        CJQGrid GridEstructuraAddenda = new CJQGrid();
        GridEstructuraAddenda.NombreTabla = "grdEstructuraAddenda";
        GridEstructuraAddenda.CampoIdentificador = "IdEstructuraAddenda";
        GridEstructuraAddenda.ColumnaOrdenacion = "EstructuraAddenda";
        GridEstructuraAddenda.TipoOrdenacion = "DESC";
        GridEstructuraAddenda.Metodo = "ObtenerEstructuraAddenda";
        GridEstructuraAddenda.TituloTabla = "Elementos de la estructura de la addenda";
        GridEstructuraAddenda.GenerarFuncionFiltro = false;
        GridEstructuraAddenda.GenerarFuncionTerminado = false;
        GridEstructuraAddenda.Altura = 300;
        GridEstructuraAddenda.Ancho = 600;
        GridEstructuraAddenda.NumeroRegistros = 10;
        GridEstructuraAddenda.RangoNumeroRegistros = "10,20,30";

        //IdEstructuraAddenda
        CJQColumn ColIdEstructuraAddenda = new CJQColumn();
        ColIdEstructuraAddenda.Nombre = "IdEstructuraAddenda";
        ColIdEstructuraAddenda.Oculto = "true";
        ColIdEstructuraAddenda.Encabezado = "IdEstructuraAddenda";
        ColIdEstructuraAddenda.Buscador = "false";
        GridEstructuraAddenda.Columnas.Add(ColIdEstructuraAddenda);

        //EstructuraAddenda
        CJQColumn ColEstructuraAddenda = new CJQColumn();
        ColEstructuraAddenda.Nombre = "EstructuraAddenda";
        ColEstructuraAddenda.Encabezado = "Estructura addenda";
        ColEstructuraAddenda.Buscador = "false";
        ColEstructuraAddenda.Alineacion = "left";
        ColEstructuraAddenda.Ancho = "125";
        GridEstructuraAddenda.Columnas.Add(ColEstructuraAddenda);

        //TipoElemento
        CJQColumn ColTipoElemento = new CJQColumn();
        ColTipoElemento.Nombre = "TipoElemento";
        ColTipoElemento.Encabezado = "Tipo de elemento";
        ColTipoElemento.Buscador = "false";
        ColTipoElemento.Alineacion = "center";
        ColTipoElemento.Ancho = "75";
        GridEstructuraAddenda.Columnas.Add(ColTipoElemento);

        //Baja
        CJQColumn ColBajaEstructuraAddenda = new CJQColumn();
        ColBajaEstructuraAddenda.Nombre = "AI";
        ColBajaEstructuraAddenda.Encabezado = "A/I";
        ColBajaEstructuraAddenda.Ordenable = "false";
        ColBajaEstructuraAddenda.Etiquetado = "A/I";
        ColBajaEstructuraAddenda.Ancho = "25";
        ColBajaEstructuraAddenda.Buscador = "true";
        ColBajaEstructuraAddenda.TipoBuscador = "Combo";
        ColBajaEstructuraAddenda.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridEstructuraAddenda.Columnas.Add(ColBajaEstructuraAddenda);

        //ConsultarEstructuraAddenda
        CJQColumn ColConsultarEstructuraAddenda = new CJQColumn();
        ColConsultarEstructuraAddenda.Nombre = "Consultar";
        ColConsultarEstructuraAddenda.Encabezado = "Ver";
        ColConsultarEstructuraAddenda.Etiquetado = "ImagenConsultar";
        ColConsultarEstructuraAddenda.Estilo = "divImagenConsultar ConsultarEstructuraAddenda";
        ColConsultarEstructuraAddenda.Buscador = "false";
        ColConsultarEstructuraAddenda.Ordenable = "false";
        ColConsultarEstructuraAddenda.Ancho = "25";
        GridEstructuraAddenda.Columnas.Add(ColConsultarEstructuraAddenda);

        ClientScript.RegisterStartupScript(this.GetType(), "grdEstructuraAddenda", GridEstructuraAddenda.GeneraGrid(), true);


    }
    protected void LeerXML()
    {
        XmlTextReader reader = new XmlTextReader(@"c:\FTG\miXML.xml");
        string cadena = "";
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    cadena = cadena + "<" + reader.Name;
                    cadena = cadena + ">";

                    break;
                case XmlNodeType.Text: //Display the text in each element.
                    cadena = cadena + reader.Value;
                    break;
                case XmlNodeType.EndElement: //Display the end of the element.
                    cadena = cadena + "</" + reader.Name;
                    cadena = cadena + ">";
                    break;
            }
        }
        cadena = cadena;

    }

    protected void CrearDocumentoXML()
    {

        //CREAR OL OBJETO DEL DOCUMENTO
        XmlDocument objXMLDoc = new XmlDocument();
        XmlElement nAddenda;
        XmlElement nFactura;
        XmlElement nMoneda;
        XmlElement nProveedor;
        XmlElement nDestino;
        XmlElement nPartes;
        XmlElement nPart;
        XmlElement nReferences;
        XmlNode nNode;
        objXMLDoc.Load(@"c:\FTG\AMT228.xml");
        XmlNode root = objXMLDoc.DocumentElement;
        root.RemoveChild(root.LastChild);

        nAddenda = objXMLDoc.CreateElement("cfdi", "Addenda", "http://www.sat.gob.mx/cfd/3");
        root.InsertAfter(nAddenda, root.LastChild);
        root.AppendChild(nAddenda);




        nFactura = objXMLDoc.CreateElement("factura");
        nFactura.SetAttribute("tipodocumento", "CMX");
        nFactura.SetAttribute("TipodocumentoFiscal", "FA");
        nFactura.SetAttribute("version", "1.0");
        nFactura.SetAttribute("folioFiscal", "39013");
        nFactura.SetAttribute("fecha", "2014-03-13");
        nFactura.SetAttribute("montoTotal", "674.77");
        nFactura.SetAttribute("referenciaProveedor", "39013");
        nAddenda.AppendChild(nFactura);

        nMoneda = objXMLDoc.CreateElement("moneda");
        nMoneda.SetAttribute("tipoMoneda", "USD");
        nMoneda.SetAttribute("tipoCambio", "13.3200");
        nAddenda.AppendChild(nMoneda);

        nProveedor = objXMLDoc.CreateElement("proveedor");
        nProveedor.SetAttribute("codigo", "10573");
        nProveedor.SetAttribute("nombre", "METALWORK $ STAMPING SA DE CV");
        nProveedor.SetAttribute("rfc", "FME04");
        nAddenda.AppendChild(nProveedor);


        nDestino = objXMLDoc.CreateElement("destino");
        nDestino.SetAttribute("codigo", "0547");
        nDestino.SetAttribute("nombre", "FORMEX DE MEXICO SA DE CV");
        nAddenda.AppendChild(nDestino);

        nPartes = objXMLDoc.CreateElement("partes");
        nAddenda.AppendChild(nPartes);

        for (int i = 0; i <= 2; i++)
        {
            nPart = objXMLDoc.CreateElement("part");
            nPart.SetAttribute("numero", i.ToString());
            nPart.SetAttribute("cantidad", "48");
            nPart.SetAttribute("unidadDeMedida", "EA");
            nPart.SetAttribute("precioUnitario", "1.70");
            nPart.SetAttribute("montoDeLinea", "81.70");
            nPartes.AppendChild(nPart);

            nReferences = objXMLDoc.CreateElement("references");
            nReferences.SetAttribute("ordenCompra", "5500004559");
            nReferences.SetAttribute("billOfLading", "A39013");
            nReferences.SetAttribute("packingList", "A39013");
            nReferences.SetAttribute("asn", "A39013");
            nReferences.SetAttribute("lineadepedido", i.ToString());
            nPart.PrependChild(nReferences);
        }
        objXMLDoc.Save(@"c:\FTG\AMT228OLD.xml");
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerAddenda(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pAddenda, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdAddenda", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pAddenda", SqlDbType.VarChar, 250).Value = Convert.ToString(pAddenda);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerEstructuraAddenda(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdAddenda, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEstructuraAddenda", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdAddenda", SqlDbType.Int).Value = pIdAddenda;
        Stored.Parameters.Add("Baja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarAddenda(string pAddenda)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonAddenda = new CJson();
        jsonAddenda.StoredProcedure.CommandText = "sp_Addenda_Consultar_FiltroPorAddenda";
        jsonAddenda.StoredProcedure.Parameters.AddWithValue("@pAddenda", pAddenda);
        return jsonAddenda.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarAddenda(Dictionary<string, object> pAddenda)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CAddenda Addenda = new CAddenda();
            Addenda.Addenda = Convert.ToString(pAddenda["Addenda"]);

            string validacion = ValidarAddenda(Addenda, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Addenda.Agregar(ConexionBaseDatos);
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
    public static string AgregarEstructuraAddenda(Dictionary<string, object> pEstructuraAddenda)
    {

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CEstructuraAddenda EstructuraAddenda = new CEstructuraAddenda();
        EstructuraAddenda.IdAddenda = Convert.ToInt32(pEstructuraAddenda["IdAddenda"]);
        EstructuraAddenda.EstructuraAddenda = Convert.ToString(pEstructuraAddenda["EstructuraAddenda"]);
        EstructuraAddenda.IdTipoElemento = Convert.ToInt32(pEstructuraAddenda["IdTipoElemento"]);
        EstructuraAddenda.Agregar(ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        respuesta = "EstructuraAddendaAgregada";
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();

        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string ObtenerFormaAddenda(int pIdAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarAddenda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarAddenda" }, ConexionBaseDatos) == "")
        {
            puedeEditarAddenda = 1;
        }
        oPermisos.Add("puedeEditarAddenda", puedeEditarAddenda);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAddenda Addenda = new CAddenda();
            Addenda.LlenaObjeto(pIdAddenda, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAddenda", Addenda.IdAddenda));
            Modelo.Add(new JProperty("Addenda", Addenda.Addenda));

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
    public static string ObtenerFormaAgregarEstructuraAddenda(int pIdAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarEstructuraAddenda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarEstructuraAddenda" }, ConexionBaseDatos) == "")
        {
            puedeAgregarEstructuraAddenda = 1;
        }
        oPermisos.Add("puedeAgregarEstructuraAddenda", puedeAgregarEstructuraAddenda);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAddenda Addenda = new CAddenda();
            CTipoElemento TipoElemento = new CTipoElemento();
            JArray JTipoElementos = new JArray();
            foreach (CTipoElemento oTipoElemento in TipoElemento.LlenaObjetos(ConexionBaseDatos))
            {
                JObject JTipoElemento = new JObject();
                JTipoElemento.Add(new JProperty("IdTipoElemento", oTipoElemento.IdTipoElemento));
                JTipoElemento.Add(new JProperty("TipoElemento", oTipoElemento.TipoElemento));
                JTipoElementos.Add(JTipoElemento);
            }

            Addenda.LlenaObjeto(pIdAddenda, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAddenda", Addenda.IdAddenda));
            Modelo.Add(new JProperty("Addenda", Addenda.Addenda));
            Modelo.Add(new JProperty("TipoElementos", JTipoElementos));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarEstructuraAddenda(int pIdAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarAddenda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarAddenda" }, ConexionBaseDatos) == "")
        {
            puedeEditarAddenda = 1;
        }
        oPermisos.Add("puedeEditarAddenda", puedeEditarAddenda);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAddenda Addenda = new CAddenda();
            Addenda.LlenaObjeto(pIdAddenda, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAddenda", Addenda.IdAddenda));
            Modelo.Add(new JProperty("Addenda", Addenda.Addenda));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEstructuraAddendaConsultar(int pIdEstructuraAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEstructuraAddenda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEstructuraAddenda" }, ConexionBaseDatos) == "")
        {
            puedeEditarEstructuraAddenda = 1;
        }
        oPermisos.Add("puedeEditarEstructuraAddenda", puedeEditarEstructuraAddenda);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEstructuraAddenda.ObtenerJsonEstructuraAddenda(Modelo, pIdEstructuraAddenda, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarAddenda(int IdAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarAddenda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarAddenda" }, ConexionBaseDatos) == "")
        {
            puedeEditarAddenda = 1;
        }
        oPermisos.Add("puedeEditarAddenda", puedeEditarAddenda);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CAddenda Addenda = new CAddenda();
            Addenda.LlenaObjeto(IdAddenda, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdAddenda", Addenda.IdAddenda));
            Modelo.Add(new JProperty("Addenda", Addenda.Addenda));
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
    public static string ObtenerFormaEditarEstructuraAddenda(int IdEstructuraAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEstructuraAddenda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CEstructuraAddenda EstructuraAddenda = new CEstructuraAddenda();
        EstructuraAddenda.LlenaObjeto(IdEstructuraAddenda, ConexionBaseDatos);
        if (Usuario.TienePermisos(new string[] { "puedeEditarEstructuraAddenda" }, ConexionBaseDatos) == "")
        {
            puedeEditarEstructuraAddenda = 1;
        }
        oPermisos.Add("puedeEditarEstructuraAddenda", puedeEditarEstructuraAddenda);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEstructuraAddenda.ObtenerJsonEstructuraAddenda(Modelo, IdEstructuraAddenda, ConexionBaseDatos);
            Modelo.Add("TipoElementos", CEstructuraAddenda.ObtenerJsonTipoElementos(IdEstructuraAddenda, ConexionBaseDatos));

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
    public static string EditarAddenda(Dictionary<string, object> pAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAddenda Addenda = new CAddenda();
        Addenda.IdAddenda = Convert.ToInt32(pAddenda["IdAddenda"]); ;
        Addenda.Addenda = Convert.ToString(pAddenda["Addenda"]);

        string validacion = ValidarAddenda(Addenda, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Addenda.Editar(ConexionBaseDatos);
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
    public static string EditarEstructuraAddenda(Dictionary<string, object> pEstructuraAddenda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CEstructuraAddenda EstructuraAddenda = new CEstructuraAddenda();
        EstructuraAddenda.LlenaObjeto(Convert.ToInt32(pEstructuraAddenda["IdEstructuraAddenda"]), ConexionBaseDatos);
        EstructuraAddenda.IdEstructuraAddenda = Convert.ToInt32(pEstructuraAddenda["IdEstructuraAddenda"]);
        EstructuraAddenda.EstructuraAddenda = Convert.ToString(pEstructuraAddenda["EstructuraAddenda"]);
        EstructuraAddenda.IdTipoElemento = Convert.ToInt32(pEstructuraAddenda["IdTipoElemento"]);
        string validacion = ValidarEstructuraAddenda(EstructuraAddenda, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            EstructuraAddenda.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdAddenda, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CAddenda Addenda = new CAddenda();
            Addenda.IdAddenda = pIdAddenda;
            Addenda.Baja = pBaja;
            Addenda.Eliminar(ConexionBaseDatos);
            respuesta = "0|AddendaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusEstructuraAddenda(int pIdEstructuraAddenda, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEstructuraAddenda EstructuraAddenda = new CEstructuraAddenda();
            EstructuraAddenda.IdEstructuraAddenda = pIdEstructuraAddenda;
            EstructuraAddenda.Baja = pBaja;
            EstructuraAddenda.Eliminar(ConexionBaseDatos);
            respuesta = "0|EstructuraAddendaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarAddenda(CAddenda pAddenda, CConexion pConexion)
    {
        string errores = "";
        if (pAddenda.Addenda == "")
        { errores = errores + "<span>*</span> El campo addenda esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    public static string ValidarEstructuraAddenda(CEstructuraAddenda pEstructuraAddenda, CConexion pConexion)
    {
        string errores = "";

        if (pEstructuraAddenda.EstructuraAddenda == "")
        { errores = errores + "<span>*</span> El elemento esta vacio, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}