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

public partial class Paginas_SalidaEntregaMaterial : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        CJQGrid GridSolicitudMaterial = new CJQGrid();
        GridSolicitudMaterial.NombreTabla = "grdEntregaMaterial";
        GridSolicitudMaterial.CampoIdentificador = "IdSolicitudMaterial";
        GridSolicitudMaterial.ColumnaOrdenacion = "IdSolicitudMaterial";
        GridSolicitudMaterial.TipoOrdenacion = "ASC";
        GridSolicitudMaterial.Metodo = "ObtenerSolicitudEntregaMaterial";
        GridSolicitudMaterial.TituloTabla = "Salida Material";
        GridSolicitudMaterial.GenerarFuncionFiltro = false;
        GridSolicitudMaterial.GenerarFuncionTerminado = false;
        GridSolicitudMaterial.Altura = 231;
        GridSolicitudMaterial.Ancho = 940;
        GridSolicitudMaterial.NumeroRegistros = 25;
        GridSolicitudMaterial.RangoNumeroRegistros = "25,50,100";

        #region Columnas

        CJQColumn ColIdSolicitudMaterial = new CJQColumn();
        ColIdSolicitudMaterial.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMaterial.Encabezado = "Solicitud Material";
        ColIdSolicitudMaterial.Oculto = "false";
        ColIdSolicitudMaterial.Buscador = "true";
        GridSolicitudMaterial.Columnas.Add(ColIdSolicitudMaterial);

        CJQColumn ColSolicitante = new CJQColumn();
        ColSolicitante.Nombre = "Solicitante";
        ColSolicitante.Encabezado = "Solicitante";
        ColSolicitante.Alineacion = "left";
        ColSolicitante.Buscador = "false";
        ColSolicitante.Ancho = "150";
        GridSolicitudMaterial.Columnas.Add(ColSolicitante);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarSolicitudMaterial";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridSolicitudMaterial.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(Page.GetType(), "grdEntregaMaterial", GridSolicitudMaterial.GeneraGrid(), true);



        //GridPartidasSolicitudMaterialConsultar
        CJQGrid grdPartidasSolicitudMaterialConsultar = new CJQGrid();
        grdPartidasSolicitudMaterialConsultar.NombreTabla = "grdPartidasSolicitudMaterialConsultar";
        grdPartidasSolicitudMaterialConsultar.CampoIdentificador = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialConsultar.ColumnaOrdenacion = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialConsultar.TipoOrdenacion = "DESC";
        grdPartidasSolicitudMaterialConsultar.Metodo = "ObtenerSolicitudEntregaMaterialConceptos";
        grdPartidasSolicitudMaterialConsultar.TituloTabla = "Conceptos";
        grdPartidasSolicitudMaterialConsultar.GenerarGridCargaInicial = false;
        grdPartidasSolicitudMaterialConsultar.GenerarFuncionFiltro = false;
        grdPartidasSolicitudMaterialConsultar.GenerarFuncionTerminado = false;
        grdPartidasSolicitudMaterialConsultar.Altura = 120;
        grdPartidasSolicitudMaterialConsultar.Ancho = 670;
        grdPartidasSolicitudMaterialConsultar.NumeroRegistros = 15;
        grdPartidasSolicitudMaterialConsultar.RangoNumeroRegistros = "15,30,60";

        //IdSolcitudMaterial
        CJQColumn ColIdSolicitudMateriall = new CJQColumn();
        ColIdSolicitudMateriall.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Oculto = "true";
        ColIdSolicitudMateriall.Encabezado = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Buscador = "false";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColIdSolicitudMateriall);

        //IdSolcitudMaterialConcepto
        CJQColumn ColIdSolicitudMaterialConcepto = new CJQColumn();
        ColIdSolicitudMaterialConcepto.Nombre = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Oculto = "true";
        ColIdSolicitudMaterialConcepto.Encabezado = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Buscador = "false";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColIdSolicitudMaterialConcepto);

        //Clave Interna
        CJQColumn ColClaveInterna = new CJQColumn();
        ColClaveInterna.Nombre = "ClaveInterna";
        ColClaveInterna.Encabezado = "Clave Interna";
        ColClaveInterna.Buscador = "false";
        ColClaveInterna.Alineacion = "left";
        ColClaveInterna.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColClaveInterna);

        //Numero Parte
        CJQColumn ColNumeroParte = new CJQColumn();
        ColNumeroParte.Nombre = "NumeroParte";
        ColNumeroParte.Encabezado = "Numero Parte";
        ColNumeroParte.Buscador = "false";
        ColNumeroParte.Alineacion = "left";
        ColNumeroParte.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColNumeroParte);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        ColCantidad.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColCantidad);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Division";
        ColDivision.Buscador = "false";
        ColDivision.Alineacion = "left";
        ColDivision.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDivision);

        //Clave SAT
        CJQColumn ColClaveProdServ = new CJQColumn();
        ColClaveProdServ.Nombre = "ClaveProdServ";
        ColClaveProdServ.Encabezado = "Clave [SAT]";
        ColClaveProdServ.Buscador = "false";
        ColClaveProdServ.Alineacion = "left";
        ColClaveProdServ.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColClaveProdServ);


        ClientScript.RegisterStartupScript(this.GetType(), "grdPartidasSolicitudMaterialConsultar", grdPartidasSolicitudMaterialConsultar.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterial(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdSolicitudMaterial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial", sqlCon);

        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterialConceptos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSolicitudMaterial)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial_Conceptos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string ObtenerFormaSolicitudEntregaMaterial(int pIdSolicitudMaterial)
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarSalidaEntregaMaterial = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                //Permisos
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarSalidaEntregaMaterial" }, pConexion) == "")
                {
                    puedeEditarSalidaEntregaMaterial = 1;
                }
                oPermisos.Add("puedeEditarSalidaEntregaMaterial", puedeEditarSalidaEntregaMaterial);

                Modelo.Add("Permisos", oPermisos);


                CSolicitudMaterial solicitudMaterial = new CSolicitudMaterial();
                solicitudMaterial.LlenaObjeto(pIdSolicitudMaterial, pConexion);

                Modelo.Add("IdSolicitudMaterial", solicitudMaterial.IdSolicitudMaterial);
                Modelo.Add("FechaAlta",Convert.ToString(solicitudMaterial.FechaAlta.ToShortDateString()));

                CUsuario solicitante = new CUsuario();
                solicitante.LlenaObjeto(solicitudMaterial.IdUsuarioCreador, pConexion);
                Modelo.Add("Solicitante", solicitante.Nombre+" "+solicitante.ApellidoPaterno+" "+solicitante.ApellidoMaterno);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudMaterial.IdOportunidad, pConexion);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(oportunidad.IdCliente, pConexion);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add("RazonSocial", organizacion.RazonSocial);
                Modelo.Add("RFC", organizacion.RFC);

                Modelo.Add("Oportundiad", oportunidad.Oportunidad);

                CUsuario agente = new CUsuario();
                agente.LlenaObjeto(oportunidad.IdUsuarioCreacion, pConexion);

                Modelo.Add("Agente", agente.Nombre+" "+agente.ApellidoPaterno+" "+agente.ApellidoMaterno);

                CDivision division = new CDivision();
                division.LlenaObjeto(oportunidad.IdDivision, pConexion);

                Modelo.Add("Division", division.Division);
              
                
                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ImprimirFacturaEncabezado(int IdFacturaEncabezado)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CFacturaEncabezado Factura = new CFacturaEncabezado();
                Factura.LlenaObjeto(IdFacturaEncabezado, pConexion);

                int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(IdEmpresa, pConexion);

                CMunicipio Municipio = new CMunicipio();
                Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);

                CEstado Estado = new CEstado();
                Estado.LlenaObjeto(Municipio.IdEstado, pConexion);

                Modelo.Add("TIPODOCUMENTO", "Nota de venta");
                Modelo.Add("FOLIO", Factura.NumeroFactura);
                Modelo.Add("RAZONSOCIALEMISOR", Empresa.RazonSocial);
                Modelo.Add("RFCEMISOR", Empresa.RFC);
                Modelo.Add("CALLEEMISOR", Empresa.Calle);
                Modelo.Add("COLONIAEMISOR", Empresa.Colonia);
                Modelo.Add("CODIGOPOSTALEMISOR", Empresa.CodigoPostal);
                Modelo.Add("MUNICIPIOEMISOR", Municipio.Municipio);
                Modelo.Add("ESTADOEMISOR", Estado.Estado);

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(Factura.IdCliente, pConexion);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

                CContactoOrganizacion Contacto = new CContactoOrganizacion();
                Contacto.LlenaObjeto(Factura.IdContactoCliente, pConexion);

                CTelefonoContactoOrganizacion Telefono = new CTelefonoContactoOrganizacion();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdContactoOrganizacion", Contacto.IdContactoOrganizacion);
                Telefono.LlenaObjetoFiltros(pParametros, pConexion);

                Modelo.Add("RAZONSOCIALRECEPTOR", Factura.RazonSocial);
                Modelo.Add("RFCRECEPTOR", Factura.RFC);
                Modelo.Add("CALLERECEPTOR", Factura.CalleFiscal);
                Modelo.Add("NUMEROEXTERIORRECEPTOR", Factura.NumeroExteriorFiscal);
                Modelo.Add("REFERENCIARECEPTOR", Factura.ReferenciaFiscal);
                Modelo.Add("COLONIARECEPTOR", Factura.ColoniaFiscal);
                Modelo.Add("CODIGOPOSTALRECEPTOR", Factura.CodigoPostalFiscal);
                Modelo.Add("MUNICIPIORECEPTOR", Factura.MunicipioFiscal);
                Modelo.Add("ESTADORECEPTOR", Factura.EstadoFiscal);
                Modelo.Add("PAISRECEPTOR", Factura.PaisFiscal);
                Modelo.Add("TELEFONORECEPTOR", Telefono.Telefono);
                Modelo.Add("CONDICIONPAGO", Factura.CondicionPago);
                Modelo.Add("SUBTOTALFACTURACLIENTE", Factura.Subtotal.ToString("C"));
                Modelo.Add("DESCUENTOFACTURACLIENTE", Factura.Descuento.ToString("C"));
                Modelo.Add("IVAFACTURACLIENTE", Factura.IVA.ToString("C"));
                Modelo.Add("TOTALFACTURACLIENTE", Factura.Total.ToString("C"));
                Modelo.Add("CANTIDADTOTALLETRA", Factura.TotalLetra);
                Modelo.Add("FECHAPAGO", Factura.FechaEmision.ToShortDateString());

                JArray Conceptos = new JArray();
                CFacturaDetalle Detalle = new CFacturaDetalle();
                pParametros.Clear();
                pParametros.Add("IdFacturaEncabezado", Factura.IdFacturaEncabezado);
                pParametros.Add("Baja", 0);

                foreach (CFacturaDetalle Partida in Detalle.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Concepto = new JObject();
                    Concepto.Add("CANTIDADDETALLE", Partida.Cantidad);
                    Concepto.Add("DESCRIPCIONDETALLE", Partida.Descripcion);
                    Concepto.Add("PRECIOUNITARIODETALLE", Partida.PrecioUnitario.ToString("C"));
                    Concepto.Add("TOTALDETALLE", Partida.Total.ToString("C"));
                    Conceptos.Add(Concepto);
                }
                Modelo.Add("Conceptos", Conceptos);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    #endregion
}