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

public partial class Proyecto : System.Web.UI.Page
{
    public string btnObtenerFormaAgregarProyecto = "";

    protected void Page_Init(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (Usuario.TienePermisos(new string[] { "puedeAgregarProyecto" }, ConexionBaseDatos) == "")
            {
                btnObtenerFormaAgregarProyecto = "<input type='button' id='btnObtenerFormaAgregarProyecto' value='+ Agregar proyecto' class='buttonLTR' />";
            }
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        GenerarGridProyecto(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    private void GenerarGridProyecto(CConexion pConexion)
    {
        string puedeConsultarProyecto = "true";
        string puedeDesactivarProyecto = "true";

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);

        if (Usuario.TienePermisos(new string[] { "puedeConsultarProyecto" }, pConexion) == "")
        {
            puedeConsultarProyecto = "false";
        }

        if (Usuario.TienePermisos(new string[] { "puedeDesactivarProyecto" }, pConexion) == "")
        {
            puedeDesactivarProyecto = "false";
        }

        //GridProyecto
        CJQGrid GridProyecto = new CJQGrid();
        GridProyecto.NombreTabla = "grdProyecto";
        GridProyecto.CampoIdentificador = "IdProyecto";
        GridProyecto.ColumnaOrdenacion = "IdProyecto";
        GridProyecto.TipoOrdenacion = "DESC";
        GridProyecto.Metodo = "ObtenerProyecto";
        GridProyecto.GenerarFuncionFiltro = false;
		GridProyecto.GenerarGridCargaInicial = false;
        GridProyecto.GenerarFuncionTerminado = true;
        GridProyecto.TituloTabla = "Catálogo de proyectos";

        //IdProyecto
        CJQColumn ColIdProyecto = new CJQColumn();
        ColIdProyecto.Nombre = "IdProyecto";
        ColIdProyecto.Oculto = "false";
        ColIdProyecto.Encabezado = "No.";
        ColIdProyecto.Ancho = "40";
        GridProyecto.Columnas.Add(ColIdProyecto);

        //Proyecto
        CJQColumn ColProyecto = new CJQColumn();
        ColProyecto.Nombre = "NombreProyecto";
        ColProyecto.Encabezado = "Nombre del proyecto";
        ColProyecto.Ancho = "150";
        ColProyecto.Alineacion = "left";
        GridProyecto.Columnas.Add(ColProyecto);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Cliente";
        ColRazonSocial.Ancho = "160";
        ColRazonSocial.Alineacion = "left";
        GridProyecto.Columnas.Add(ColRazonSocial);

		//IdOportunidad
		CJQColumn ColIdOpotunidad = new CJQColumn();
		ColIdOpotunidad.Nombre = "IdOportunidad";
		ColIdOpotunidad.Encabezado = "No.";
		ColIdOpotunidad.Oculto = "false";
		ColIdOpotunidad.Ancho = "50";
		GridProyecto.Columnas.Add(ColIdOpotunidad);

		//FechaInicio
		CJQColumn ColFechaInicio = new CJQColumn();
        ColFechaInicio.Nombre = "FechaInicio";
        ColFechaInicio.Encabezado = "Fecha Inicio Proyecto";
        ColFechaInicio.Ancho = "110";
        ColFechaInicio.Alineacion = "right";
        ColFechaInicio.Buscador = "false";
        GridProyecto.Columnas.Add(ColFechaInicio);

        //FechaTermino
        CJQColumn ColFechaTermino = new CJQColumn();
        ColFechaTermino.Nombre = "FechaTermino";
        ColFechaTermino.Encabezado = "Fecha Fin Proyecto";
        ColFechaTermino.Ancho = "110";
        ColFechaTermino.Alineacion = "right";
        ColFechaTermino.Buscador = "false";
        GridProyecto.Columnas.Add(ColFechaTermino);

		//Moneda
		CJQColumn ColMoneda = new CJQColumn();
		ColMoneda.Nombre = "Moneda";
		ColMoneda.Encabezado = "Moneda";
		ColMoneda.Ancho = "110";
		ColMoneda.Alineacion = "left";
		ColMoneda.Buscador = "true";
		ColMoneda.TipoBuscador = "Combo";
		ColMoneda.StoredProcedure.CommandText = "sp_TipoMoneda_Filtro";
		GridProyecto.Columnas.Add(ColMoneda);

		//TipoCambio
		CJQColumn ColTipoCambio = new CJQColumn();
		ColTipoCambio.Nombre = "TipoCambio";
		ColTipoCambio.Encabezado = "Tipo cambio";
		ColTipoCambio.Ancho = "80";
		ColTipoCambio.Alineacion = "right";
		ColTipoCambio.Buscador = "false";
		GridProyecto.Columnas.Add(ColTipoCambio);

        //PrecioTeorico
        CJQColumn ColPrecioTeorico = new CJQColumn();
        ColPrecioTeorico.Nombre = "PrecioTeorico";
        ColPrecioTeorico.Encabezado = "Precio Teorico";
        ColPrecioTeorico.Ancho = "150";
        ColPrecioTeorico.Alineacion = "left";
        GridProyecto.Columnas.Add(ColPrecioTeorico);

        //MontoTotalPesos
        CJQColumn ColMontoTotalPesos = new CJQColumn();
        ColMontoTotalPesos.Nombre = "MontoTotalPesos";
        ColMontoTotalPesos.Encabezado = "Programado pesos";
        ColMontoTotalPesos.Ancho = "110";
        ColMontoTotalPesos.Formato = "FormatoMoneda";
        ColMontoTotalPesos.Alineacion = "right";
        ColMontoTotalPesos.Buscador = "false";
        GridProyecto.Columnas.Add(ColMontoTotalPesos);

        //MontoSubtotal
        CJQColumn ColMontoSubtotal = new CJQColumn();
        ColMontoSubtotal.Nombre = "Subtotal";
        ColMontoSubtotal.Encabezado = "Facturado sin IVA";
        ColMontoSubtotal.Ancho = "110";
        ColMontoSubtotal.Formato = "FormatoMoneda";
        ColMontoSubtotal.Alineacion = "right";
        ColMontoSubtotal.Buscador = "false";
        GridProyecto.Columnas.Add(ColMontoSubtotal);

        //MontoTotalPesosFacturado
        CJQColumn ColMontoTotalPesosFacturado = new CJQColumn();
        ColMontoTotalPesosFacturado.Nombre = "MontoTotalPesosFacturado";
        ColMontoTotalPesosFacturado.Encabezado = "Facturado pesos";
        ColMontoTotalPesosFacturado.Ancho = "100";
        ColMontoTotalPesosFacturado.Formato = "FormatoMoneda";
        ColMontoTotalPesosFacturado.Alineacion = "right";
        ColMontoTotalPesosFacturado.Buscador = "false";
        GridProyecto.Columnas.Add(ColMontoTotalPesosFacturado);

        //MontoTotalPesosFacturado
        CJQColumn ColMontoTotalCobrado = new CJQColumn();
        ColMontoTotalCobrado.Nombre = "MontoTotalCobrado";
        ColMontoTotalCobrado.Encabezado = "Cobrado pesos";
        ColMontoTotalCobrado.Ancho = "100";
        ColMontoTotalCobrado.Formato = "FormatoMoneda";
        ColMontoTotalCobrado.Alineacion = "right";
        ColMontoTotalCobrado.Buscador = "false";
        GridProyecto.Columnas.Add(ColMontoTotalCobrado);

        //MontoTotalPesosFacturado
        CJQColumn ColMontoTotalPorCobrar = new CJQColumn();
        ColMontoTotalPorCobrar.Nombre = "MontoTotalPesosPorCobrar";
        ColMontoTotalPorCobrar.Encabezado = "Por cobrar pesos";
        ColMontoTotalPorCobrar.Ancho = "100";
        ColMontoTotalPorCobrar.Formato = "FormatoMoneda";
        ColMontoTotalPorCobrar.Alineacion = "right";
        ColMontoTotalPorCobrar.Buscador = "false";
        GridProyecto.Columnas.Add(ColMontoTotalPorCobrar);

        //CostoTeorico
        CJQColumn ColCostoTeorico = new CJQColumn();
        ColCostoTeorico.Nombre = "CostoTeorico";
        ColCostoTeorico.Encabezado = "Costo teorico";
        ColCostoTeorico.Ancho = "100";
        ColCostoTeorico.Formato = "FormatoMoneda";
        ColCostoTeorico.Alineacion = "right";
        ColCostoTeorico.Buscador = "false";
        GridProyecto.Columnas.Add(ColCostoTeorico);

        //CostoTotalPesos
        CJQColumn ColCostoProyecto = new CJQColumn();
        ColCostoProyecto.Nombre = "CostoProyecto";
        ColCostoProyecto.Encabezado = "Costo real";
        ColCostoProyecto.Ancho = "100";
        ColCostoProyecto.Formato = "FormatoMoneda";
        ColCostoProyecto.Alineacion = "right";
        ColCostoProyecto.Buscador = "false";
        GridProyecto.Columnas.Add(ColCostoProyecto);

        //Responsable
        CJQColumn ColResponsableProyecto = new CJQColumn();
        ColResponsableProyecto.Nombre = "Responsable";
        ColResponsableProyecto.Encabezado = "Responsable";
        ColResponsableProyecto.Ancho = "80";
        ColResponsableProyecto.Alineacion = "Center";
        ColResponsableProyecto.Buscador = "true";
        GridProyecto.Columnas.Add(ColResponsableProyecto);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "IdDivision";
        ColDivision.Encabezado = "Division";
        ColDivision.Ancho = "80";
        ColDivision.Alineacion = "Center";
        ColDivision.Buscador = "true";
        ColDivision.TipoBuscador = "Combo";
        ColDivision.StoredProcedure.CommandText = "sp_Proyecto_Division";
        GridProyecto.Columnas.Add(ColDivision);

        //Estatus
        CJQColumn ColEstatusProyecto = new CJQColumn();
        ColEstatusProyecto.Nombre = "EstatusProyecto";
        ColEstatusProyecto.Encabezado = "Estatus";
        ColEstatusProyecto.Ancho = "80";
        ColEstatusProyecto.Alineacion = "Center";
        ColEstatusProyecto.Buscador = "false";
        GridProyecto.Columnas.Add(ColEstatusProyecto);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "45";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBaja.Oculto = puedeDesactivarProyecto;
        GridProyecto.Columnas.Add(ColBaja);

        //Facturas
        CJQColumn ColListaSolicitudesFacturas = new CJQColumn();
        ColListaSolicitudesFacturas.Nombre = "Facturas";
        ColListaSolicitudesFacturas.Encabezado = "Facturas";
        ColListaSolicitudesFacturas.Etiquetado = "Imagen";
        ColListaSolicitudesFacturas.Imagen = "concepto.png";
        ColListaSolicitudesFacturas.Estilo = "divImagenConsultar imgFormaListaSolicitudesFacturas";
        ColListaSolicitudesFacturas.Buscador = "false";
        ColListaSolicitudesFacturas.Ordenable = "false";
        ColListaSolicitudesFacturas.Ancho = "60";
        GridProyecto.Columnas.Add(ColListaSolicitudesFacturas);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarProyecto";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        ColConsultar.Oculto = puedeConsultarProyecto;
        GridProyecto.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProyecto", GridProyecto.GeneraGrid(), true);
    }

	private void GenerarGridFacturas(CConexion pConexion)
	{
		CJQGrid GridFacturas = new CJQGrid();
		GridFacturas.NombreTabla = "Facturas";
	}

    [WebMethod]
    public static string ObtenerDatosGrafica(int pIdProyecto, string pNombreProyecto, string pCliente, string pResponsable, int pIdDivision, int pIdEstatusProyecto, int pAI)
    {
        JObject JTotales = new JObject();

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();


        JObject Permisos = new JObject();
        Permisos.Add("puedeVerGrafica", 1);

        if (respuesta == "Conexion Establecida")
        {
            CUsuario UsuarioSesion = new CUsuario();
            UsuarioSesion.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (UsuarioSesion.IdUsuario != 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Select = new CSelectEspecifico();
                Select.StoredProcedure.CommandText = "sp_Proyecto_Totales";
                Select.StoredProcedure.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
                Select.StoredProcedure.Parameters.Add("pNombreProyecto", SqlDbType.VarChar, 255).Value = pNombreProyecto;
                Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
                Select.StoredProcedure.Parameters.Add("pResponsable", SqlDbType.VarChar, 255).Value = pResponsable;
                Select.StoredProcedure.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
                Select.StoredProcedure.Parameters.Add("pIdEstatusProyecto", SqlDbType.Int).Value = pIdEstatusProyecto;
                Select.StoredProcedure.Parameters.Add("pAI", SqlDbType.Int).Value = pAI;
                Select.StoredProcedure.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = UsuarioSesion.IdUsuario;

                Select.Llena(ConexionBaseDatos);

                if (Select.Registros.Read())
                {
                    Modelo.Add("Proyectos", Convert.ToInt32(Select.Registros["Proyectos"]));
                    Modelo.Add("Programado", Convert.ToInt32(Select.Registros["Programado"]).ToString("C"));
                    Modelo.Add("Facturado", Convert.ToInt32(Select.Registros["Facturado"]).ToString("C"));
                    Modelo.Add("PorCobrar", Convert.ToInt32(Select.Registros["PorCobrar"]).ToString("C"));
                    Modelo.Add("Cobrado", Convert.ToInt32(Select.Registros["Cobrado"]).ToString("C"));
                    Modelo.Add("CostoTeorico", Convert.ToInt32(Select.Registros["CostoTeorico"]).ToString("C"));
                    Modelo.Add("CostoReal", Convert.ToInt32(Select.Registros["CostoReal"]).ToString("C"));
                    Modelo.Add("Diferencia", Convert.ToInt32(Select.Registros["Diferencia"]).ToString("C"));
                }

                JTotales.Add("Errores", 0);
                JTotales.Add("Modelo", Modelo);
                JTotales.Add("Permisos", Permisos);

                Select.CerrarConsulta();
            }
            else
            {
                JTotales.Add("Error", 1);
                JTotales.Add("Descripcion", "La sesión a expirado");
                JTotales.Add("Permisos", Permisos);
            }
        }
        else
        {
            JTotales.Add("Error", 1);
            JTotales.Add("Descripcion", respuesta);
            JTotales.Add("Permisos", Permisos);
        }
        return JTotales.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProyecto(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdProyecto, string pNombreProyecto, string pRazonSocial, string  pEstatusProyecto, string pResponsable, int pIdDivision, int pAI, string pIdOportunidad, int pIdTipoMoneda)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdProyecto", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 500).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdProyecto", SqlDbType.VarChar, 6).Value = pIdProyecto;
        Stored.Parameters.Add("pNombreProyecto", SqlDbType.VarChar, 250).Value = Convert.ToString(pNombreProyecto);
        Stored.Parameters.Add("pEstatusProyecto", SqlDbType.VarChar, 250).Value = pEstatusProyecto;
		Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
		Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar, 250).Value = Convert.ToString(pIdOportunidad);
		Stored.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = Convert.ToInt32(pIdTipoMoneda);
		Stored.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
        Stored.Parameters.Add("pResponsable", SqlDbType.VarChar, 255).Value = pResponsable;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				CEstatusProyectoUsuario Estatus = new CEstatusProyectoUsuario();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
				pParametros.Add("Baja", 0);
				Estatus.LlenaObjetoFiltros(pParametros, pConexion);
				Estatus.IdUsuario = UsuarioSesion.IdUsuario;
				Estatus.Estatus = pEstatusProyecto;
				if (Estatus.IdEstatusProyectoUsuario != 0)
				{
					Estatus.Editar(pConexion);
				}
				else
				{
					Estatus.Agregar(pConexion);
				}
			}
		});


        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarProyecto(string pNombreProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonProyecto = new CJson();
        jsonProyecto.StoredProcedure.CommandText = "sp_Proyecto_Consultar_FiltroPorProyecto";
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@pProyecto", pNombreProyecto);
        return jsonProyecto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarRazonSocialCliente(string pRazonSocial)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proyecto_BuscarCliente";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }
	
	[WebMethod]
	public static string BuscarIdOportunidad(string pIdOportunidad)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		COportunidad JsonOportunidad = new COportunidad();
		JsonOportunidad.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_IdOportunidad";
		JsonOportunidad.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", pIdOportunidad);
		string sJson = JsonOportunidad.ObtenerJsonOportunidad(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return sJson;
	}
	
	[WebMethod]
    public static string BuscarResponsable(string pResponsable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proyecto_Responsable";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pResponsable", pResponsable);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    public static string BuscarRazonSocial(String pRazonSocial)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proyecto_RazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);

        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    public static string BuscarRazonSocialClienteEmpresarial(Dictionary<string, object> pProyecto)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", Convert.ToString(pProyecto["pRazonSocial"]));
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdCliente", Convert.ToInt32(pProyecto["pIdClienteProyecto"]));

        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    public static string ObtenerFormaAgregarProyecto()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CTipoCambio TipoCambio = new CTipoCambio();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            JObject oPermisos = new JObject();

            #region PERMISOS
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            int accesoCliente = 0;
            if (Usuario.TienePermisos(new string[] { "accesoCliente" }, ConexionBaseDatos) == "")
            {
                accesoCliente = 1;
            }
            oPermisos.Add("accesoCliente", accesoCliente);

            int accesoOportunidad = 0;
            if (Usuario.TienePermisos(new string[] { "accesoOportunidad" }, ConexionBaseDatos) == "")
            {
                accesoOportunidad = 1;
            }
            oPermisos.Add("accesoOportunidad", accesoOportunidad);
            #endregion

            #region MODELO
            Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(ConexionBaseDatos));
            Modelo.Add("Usuarios", UsuariosProyectos(ConexionBaseDatos));

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
            Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(1));
            Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(1));
            Parametros.Add("Fecha", DateTime.Today);
            TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);
            Modelo.Add("TipoCambioFactura", TipoCambio.TipoCambio);
            Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos));
            #endregion

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

	private static JArray UsuariosProyectos(CConexion pConexion)
	{
		JArray Usuarios = new JArray();
		CUsuario UsuariosProyecto = new CUsuario();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("Baja", 0);
		pParametros.Add("IdDepartamento", 6);
		foreach(CUsuario UsuarioProyecto in UsuariosProyecto.LlenaObjetosFiltros(pParametros, pConexion))
		{
			JObject Usuario = new JObject();
			Usuario.Add("Valor",UsuarioProyecto.IdUsuario);
			Usuario.Add("Descripcion", UsuarioProyecto.Nombre + ' ' + UsuarioProyecto.ApellidoPaterno + ' ' + UsuarioProyecto.ApellidoMaterno);
			Usuarios.Add(Usuario);
		}

		return Usuarios;
	}

    [WebMethod]
    public static string ObtenerFormaEditarProyecto(int IdProyecto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarProyecto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarProyecto" }, ConexionBaseDatos) == "")
        {
            puedeEditarProyecto = 1;
        }
        oPermisos.Add("puedeEditarProyecto", puedeEditarProyecto);

        if (respuesta == "Conexion Establecida")
        {
            int accesoCliente = 0;
            if (Usuario.TienePermisos(new string[] { "accesoCliente" }, ConexionBaseDatos) == "")
            {
                accesoCliente = 1;
            }
            oPermisos.Add("accesoCliente", accesoCliente);

            int accesoOportunidad = 0;
            if (Usuario.TienePermisos(new string[] { "accesoOportunidad" }, ConexionBaseDatos) == "")
            {
                accesoOportunidad = 1;
            }
            oPermisos.Add("accesoOportunidad", accesoOportunidad);

            CProyecto Proyecto = new CProyecto();
            Proyecto.LlenaObjeto(IdProyecto, ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo = CProyecto.ObtenerProyecto(Modelo, IdProyecto, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
			//Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuarioNombre(Convert.ToInt32(Modelo["IdUsuario"].ToString()), ConexionBaseDatos));
			Modelo.Add("Usuarios", UsuariosProyectos(ConexionBaseDatos));

			Modelo.Add("Oportunidades", COportunidad.ObtenerOportunidadProyecto(Proyecto.IdCliente, Usuario.IdUsuario, Proyecto.IdOportunidad, ConexionBaseDatos));
            Modelo.Add("NivelesInteres", CNivelInteresCotizacion.ObtenerJsonNivelInteresCotizacion(Proyecto.IdNivelInteres, ConexionBaseDatos));
            Modelo.Add("ListaEstatus", CEstatusProyecto.ObtenerJsonEstatusProyectos(Proyecto.IdEstatusProyecto, ConexionBaseDatos));
            Modelo.Add("Divisiones", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos));
            Modelo.Add("IdDivision", Proyecto.IdDivision);

            Modelo.Add("CostoReal", Proyecto.CostoReal * (1 / Proyecto.TipoCambio));
            Modelo.Add("PorcentajeCosto", Proyecto.CostoReal * (1 / Proyecto.TipoCambio) / Proyecto.CostoTeorico * 100);
			Modelo.Add("Facturado", Proyecto.Facturado * (1 / Proyecto.TipoCambio));
			Modelo.Add("PorcentajeFacturado", Proyecto.Facturado * (1 / Proyecto.TipoCambio) / Proyecto.PrecioTeorico * 100);
			Modelo.Add("Programado", Proyecto.Programado);
            Modelo.Add("Cobrado", Proyecto.Cobrado * (1 / Proyecto.TipoCambio));

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
    public static string ObtenerFormaConsultarProyecto(int pIdProyecto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarProyecto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarProyecto" }, ConexionBaseDatos) == "")
        {
            puedeEditarProyecto = 1;
        }
        oPermisos.Add("puedeEditarProyecto", puedeEditarProyecto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CProyecto.ObtenerProyecto(Modelo, pIdProyecto, ConexionBaseDatos);
            CProyecto Proyecto = new CProyecto();
            Proyecto.LlenaObjeto(pIdProyecto, ConexionBaseDatos);

			Modelo.Add("FechaAlta", Proyecto.FechaAlta.ToShortDateString());
			Modelo.Add("CostoReal", Proyecto.CostoReal * (1 / Proyecto.TipoCambio));
			Modelo.Add("PorcentajeCosto", Proyecto.CostoReal * (1 / Proyecto.TipoCambio) / Proyecto.CostoTeorico * 100);
			Modelo.Add("Facturado", Proyecto.Facturado * (1 / Proyecto.TipoCambio));
            Modelo.Add("PorcentajeFacturado", Proyecto.Facturado * (1 / Proyecto.TipoCambio) / Proyecto.PrecioTeorico * 100);
            Modelo.Add("Programado", Proyecto.Programado);
            Modelo.Add("Cobrado", Proyecto.Cobrado);

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
    public static string ObtenerFormaListaSolicitudesFacturas(int pIdProyecto)
    {
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_SolicitudFacturacion_Listar";
				Consulta.StoredProcedure.Parameters.Add("IdProyecto", SqlDbType.Int).Value = pIdProyecto;

				Consulta.Llena(pConexion);

				JArray SolicitudesFacturas = new JArray();
				Modelo.Add("IdProyecto", pIdProyecto);

				while (Consulta.Registros.Read())
				{
					JObject SolicitudFactura = new JObject();

					SolicitudFactura.Add("IdSolicitudFacturacion", Convert.ToInt32(Consulta.Registros["IdSolicitudFacturacion"]));
					SolicitudFactura.Add("Solicitud", Convert.ToString(Consulta.Registros["Solicitud"]));
					SolicitudFactura.Add("EstatusFacturado", Convert.ToString(Consulta.Registros["EstatusFacturado"]));
					SolicitudFactura.Add("Orden", Convert.ToInt32(Consulta.Registros["OrdenFactura"]));
					SolicitudFactura.Add("TotalSolicitudFactura", Convert.ToDecimal(Consulta.Registros["TotalSolicitudFactura"]));

					SolicitudesFacturas.Add(SolicitudFactura);
				}

				Consulta.CerrarConsulta();

				Modelo.Add("SolicitudFacturas", SolicitudesFacturas);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarSolicitudFactura(int pIdProyecto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            JObject oModelo = new JObject();
            oModelo = CProyecto.ObtenerProyecto(oModelo, pIdProyecto, ConexionBaseDatos);
            oRespuesta.Add("Modelo", oModelo);
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarConceptoSolicitudFactura(int pIdProyecto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            JObject oPermisos = new JObject();

            #region PERMISOS
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            int accesoUnidadCompraVenta = 0;
            if (Usuario.TienePermisos(new string[] { "accesoUnidadCompraVenta" }, ConexionBaseDatos) == "")
            {
                accesoUnidadCompraVenta = 1;
            }
            oPermisos.Add("accesoUnidadCompraVenta", accesoUnidadCompraVenta);
            #endregion

            #region MODELO
            Modelo = CProyecto.ObtenerProyecto(Modelo, pIdProyecto, ConexionBaseDatos);
            Modelo.Add("UnidadCompraVentas", CJson.ObtenerJsonUnidadCompraVenta(ConexionBaseDatos));
            Modelo.Add("TipoVentas", CJson.ObtenerJsonTipoVenta(ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add("TiposIVA", CTipoIVA.ObtenerJsonTiposIVA(ConexionBaseDatos));
            #endregion

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
    public static string ObtenerFormaAgregarConcepto(int IdSolicitudFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            CSolicitudFacturacion SolicitudFactura = new CSolicitudFacturacion();
            SolicitudFactura.LlenaObjeto(IdSolicitudFactura, ConexionBaseDatos);

            JObject oModelo = new JObject();
            oModelo = CProyecto.ObtenerProyecto(oModelo, SolicitudFactura.IdProyecto, ConexionBaseDatos);
            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();
            JArray JConceptos = new JArray();
            int Orden = 1;
            foreach (CConceptoProyecto oConceptoProyecto in ConceptoProyecto.ObtenerConceptoFiltroIdProyecto(IdSolicitudFactura, ConexionBaseDatos))
            {
                JObject JConcepto = new JObject();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                TipoMoneda.LlenaObjeto(oConceptoProyecto.IdTipoMoneda, ConexionBaseDatos);
                JConcepto.Add("IdConcepto", oConceptoProyecto.IdConceptoProyecto);
                oConceptoProyecto.OrdenConcepto = Orden;
                oConceptoProyecto.EditarOrdenamiento(ConexionBaseDatos);
                JConcepto.Add("NombreConcepto", oConceptoProyecto.Descripcion);
                decimal Monto = oConceptoProyecto.Monto * oConceptoProyecto.Cantidad;
                JConcepto.Add("Monto", Monto.ToString("C"));
                JConcepto.Add("TipoMoneda", TipoMoneda.TipoMoneda);
                JConcepto.Add("Orden", oConceptoProyecto.OrdenConcepto);
                JConceptos.Add(JConcepto);
                Orden++;
            }
            oModelo.Add("IdSolicitudFactura", IdSolicitudFactura);
            oModelo.Add("Solicitud", SolicitudFactura.Solicitud);
            oModelo.Add("Conceptos", JConceptos);
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add("Modelo", oModelo);

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
    public static string ObtenerFormaEditarSolicitudFactura(int pIdSolicitud)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject JRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            JObject JModelo = new JObject();
            CSolicitudFacturacion SolicitudFactura = new CSolicitudFacturacion();
            SolicitudFactura.LlenaObjeto(pIdSolicitud, ConexionBaseDatos);
            JModelo = CProyecto.ObtenerProyecto(JModelo, SolicitudFactura.IdProyecto, ConexionBaseDatos);
            JModelo.Add("IdSolicitudFactura", SolicitudFactura.IdSolicitudFacturacion);
            JModelo.Add("Solicitud", SolicitudFactura.Solicitud);

            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();
            JArray JConceptos = new JArray();
            foreach (CConceptoProyecto oConceptoProyecto in ConceptoProyecto.ObtenerConceptoFiltroIdProyecto(pIdSolicitud, ConexionBaseDatos))
            {
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                TipoMoneda.LlenaObjeto(oConceptoProyecto.IdTipoMoneda, ConexionBaseDatos);

                JObject JConcepto = new JObject();
                JConcepto.Add("IdConcepto", oConceptoProyecto.IdConceptoProyecto);
                JConcepto.Add("NombreConcepto", oConceptoProyecto.Descripcion);
                decimal Monto = oConceptoProyecto.Cantidad * oConceptoProyecto.Monto;
                JConcepto.Add("Monto", Monto.ToString("C"));
                JConcepto.Add("TipoMoneda", TipoMoneda.TipoMoneda);
                JConcepto.Add("Orden", oConceptoProyecto.OrdenConcepto);
                JConceptos.Add(JConcepto);
            }

            JModelo.Add("Conceptos", JConceptos);



            JRespuesta.Add("Modelo", JModelo);
            JRespuesta.Add("Error", 0);
        }
        else
        {
            JRespuesta.Add(new JProperty("Error", 1));
            JRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return JRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarConceptoSolicitudFactura(int pIdConcepto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        int puedeEditarConcepto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarConcepto" }, ConexionBaseDatos) == "")
        {
            puedeEditarConcepto = 1;
        }
        oPermisos.Add("puedeEditarConcepto", puedeEditarConcepto);

        if (respuesta == "Conexion Establecida")
        {
            CConceptoProyecto Concepto = new CConceptoProyecto();
            Concepto.LlenaObjeto(pIdConcepto, ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CConceptoProyecto.ObtenerConceptoProyecto(Modelo, pIdConcepto, ConexionBaseDatos);
            Modelo.Add("TipoVentas", CJson.ObtenerJsonTipoVenta(Convert.ToInt32(Modelo["IdTipoVenta"].ToString()), ConexionBaseDatos));
            Modelo.Add("UnidadCompraVentas", CJson.ObtenerJsonUnidadCompraVenta(Convert.ToInt32(Modelo["IdUnidadCompraVenta"].ToString()), ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("TiposIVA", CTipoIVA.ObtenerJsonTiposIVA(Concepto.IdTipoIVA, ConexionBaseDatos));

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
	public static string ObtenerEstatusProyecto()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_ConsultarFiltro_Proyecto_Estatus";

				Consulta.Llena(pConexion);

				JArray Opciones = new JArray();

				while(Consulta.Registros.Read())
				{
					JObject Opcion = new JObject();
					Opcion.Add("Valor", Convert.ToInt32(Consulta.Registros["IdEstatusProyecto"]));
					Opcion.Add("Descripcion", Convert.ToString(Consulta.Registros["Estatus"]));
					Opciones.Add(Opcion);
				}

				Consulta.CerrarConsulta();

				CEstatusProyectoUsuario EstatusUsuario = new CEstatusProyectoUsuario();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
				pParametros.Add("Baja", 0);
				EstatusUsuario.LlenaObjetoFiltros(pParametros, pConexion);

				Modelo.Add("Estatus", Opciones);
				Modelo.Add("EstatusUsuario", EstatusUsuario.Estatus);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

    [WebMethod]
    public static string ObtenerTipoCambio(Dictionary<string, object> pTipoCambio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCambio TipoCambio = new CTipoCambio();

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Opcion", 1);
            ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(pTipoCambio["IdTipoCambioOrigen"]));
            ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(pTipoCambio["IdTipoCambioDestino"]));
            ParametrosTS.Add("Fecha", DateTime.Today);
            TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("TipoCambioActual", TipoCambio.TipoCambio);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
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
    public static string ObtenerTotalProyectos(int pIdProyecto, string pNombreProyecto, string pCliente, string pResponsable, int pIdDivision, int pIdEstatusProyecto, int pAI)
    {
        JObject JTotales = new JObject();

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        
        if (respuesta == "Conexion Establecida"){
            CUsuario UsuarioSesion = new CUsuario();
            UsuarioSesion.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (UsuarioSesion.IdUsuario != 0){
                JObject Modelo = new JObject();

                CSelectEspecifico Select = new CSelectEspecifico();
                Select.StoredProcedure.CommandText = "sp_Proyecto_Totales";
                Select.StoredProcedure.Parameters.Add("pIdProyecto", SqlDbType.Int).Value = pIdProyecto;
                Select.StoredProcedure.Parameters.Add("pNombreProyecto", SqlDbType.VarChar, 255).Value = pNombreProyecto;
                Select.StoredProcedure.Parameters.Add("pCliente", SqlDbType.VarChar, 255).Value = pCliente;
                Select.StoredProcedure.Parameters.Add("pResponsable", SqlDbType.VarChar, 255).Value = pResponsable;
                Select.StoredProcedure.Parameters.Add("pIdDivision", SqlDbType.Int).Value = pIdDivision;
                Select.StoredProcedure.Parameters.Add("pIdEstatusProyecto", SqlDbType.Int).Value = pIdEstatusProyecto;
                Select.StoredProcedure.Parameters.Add("pAI", SqlDbType.Int).Value = pAI;
                Select.StoredProcedure.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = UsuarioSesion.IdUsuario;

                Select.Llena(ConexionBaseDatos);

                if (Select.Registros.Read()){
                    Modelo.Add("Proyectos", Convert.ToInt32(Select.Registros["Proyectos"]));
                    Modelo.Add("Programado", Convert.ToInt32(Select.Registros["Programado"]).ToString("C"));
                    Modelo.Add("Facturado", Convert.ToInt32(Select.Registros["Facturado"]).ToString("C"));
                    Modelo.Add("PorCobrar", Convert.ToInt32(Select.Registros["PorCobrar"]).ToString("C"));
                    Modelo.Add("Cobrado", Convert.ToInt32(Select.Registros["Cobrado"]).ToString("C"));
                    Modelo.Add("CostoTeorico", Convert.ToInt32(Select.Registros["CostoTeorico"]).ToString("C"));
                    Modelo.Add("CostoReal", Convert.ToInt32(Select.Registros["CostoReal"]).ToString("C"));
                    Modelo.Add("Diferencia", Convert.ToInt32(Select.Registros["Diferencia"]).ToString("C"));
                }

                JTotales.Add("Errores", 0);
                JTotales.Add("Modelo", Modelo);

                Select.CerrarConsulta();
            }
            else
            {
                JTotales.Add("Error", 1);
                JTotales.Add("Descripcion", "La sesión a expirado");
            }
        }
        else
        {
            JTotales.Add("Error", 1);
            JTotales.Add("Descripcion", respuesta);
        }
        return JTotales.ToString();
    }

    [WebMethod]
    public static string CalculaMontoPorcentaje(Dictionary<string, object> pProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            decimal MontoPorcentaje = 0;
            decimal CantidadPorcentaje = 0;
            CProyecto Proyecto = new CProyecto();
            Proyecto.LlenaObjeto(Convert.ToInt32(pProyecto["IdProyecto"]), ConexionBaseDatos);
            CantidadPorcentaje = Convert.ToDecimal(pProyecto["CantidadProcentaje"]);
            MontoPorcentaje = (Proyecto.PrecioTeorico * CantidadPorcentaje) / 100;
            oRespuesta.Add("CantidadProcentaje", MontoPorcentaje);
            oRespuesta.Add("IdTipoMoneda", Proyecto.IdTipoMoneda);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarProyecto(Dictionary<string, object> pProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProyecto Proyecto = new CProyecto();
            Proyecto.NombreProyecto = Convert.ToString(pProyecto["NombreProyecto"]);
            Proyecto.IdCliente = Convert.ToInt32(pProyecto["IdCliente"]);
            Proyecto.FechaAlta = Convert.ToDateTime(DateTime.Now);
            Proyecto.FechaInicio = Convert.ToDateTime(pProyecto["FechaInicio"]);
            Proyecto.FechaTermino = Convert.ToDateTime(pProyecto["FechaTermino"]);
            Proyecto.PrecioTeorico = Convert.ToDecimal(pProyecto["PrecioTeorico"]);
            Proyecto.CostoTeorico = Convert.ToDecimal(pProyecto["CostoTeorico"]);
            Proyecto.Notas = Convert.ToString(pProyecto["Notas"]);
			Proyecto.IdDivision = Convert.ToInt32(pProyecto["IdDivision"]);
            Proyecto.IdEstatusProyecto = 7;
            Proyecto.IdTipoMoneda = Convert.ToInt32(pProyecto["IdTipoMoneda"]);
            Proyecto.TipoCambio = Convert.ToDecimal(pProyecto["TipoCambio"]);
            Proyecto.IdUsuarioResponsable = Convert.ToInt32(pProyecto["IdUsuarioResponsable"]);
            Proyecto.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            Proyecto.IdOportunidad = Convert.ToInt32(pProyecto["IdOportunidad"]);
            Proyecto.IdNivelInteres = Convert.ToInt32(pProyecto["IdNivelInteres"]);

            string validacion = ValidarProyecto(Proyecto, ConexionBaseDatos);
            JObject oRespuesta = new JObject();

            if (validacion == "")
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

                Proyecto.Agregar(ConexionBaseDatos);
                CProyectoEmpresa ProyectoEmpresa = new CProyectoEmpresa();
                ProyectoEmpresa.FechaAlta = Convert.ToDateTime(DateTime.Now);
                ProyectoEmpresa.IdProyecto = Proyecto.IdProyecto;
                ProyectoEmpresa.IdEmpresa = Sucursal.IdEmpresa;
                ProyectoEmpresa.IdSucursal = Usuario.IdSucursalActual;
                ProyectoEmpresa.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                ProyectoEmpresa.Agregar(ConexionBaseDatos);

                COportunidad.ActualizarTotalesOportunidad(Convert.ToInt32(pProyecto["IdOportunidad"]), ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Proyecto.IdProyecto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un nuevo proyecto";
                HistorialGenerico.AgregarHistorialGenerico("Proyecto", ConexionBaseDatos);

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
    public static string AgregarConceptoProyecto(Dictionary<string, object> pProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int ConversionMoneda = 0;
        int IdTipoMonedaConversion = 0;
        decimal TipoCambioConversion = 0;

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();
            ConceptoProyecto.Descripcion = Convert.ToString(pProyecto["NombreConcepto"]);
            ConceptoProyecto.IdProyecto = Convert.ToInt32(pProyecto["IdProyecto"]);
            ConceptoProyecto.IdUnidadCompraVenta = Convert.ToInt32(pProyecto["IdUnidadCompraVenta"]);
            ConceptoProyecto.IdTipoVenta = Convert.ToInt32(pProyecto["IdTipoVenta"]);
            ConceptoProyecto.Cantidad = Convert.ToDecimal(pProyecto["Cantidad"]);
            ConceptoProyecto.IdTipoIVA = Convert.ToInt32(pProyecto["IdTipoIVA"]);
            ConceptoProyecto.ClaveProdServ = Convert.ToString(pProyecto["ClaveProdServ"]);

            if (ConceptoProyecto.IdTipoIVA == 1)
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
                ConceptoProyecto.IVA = Sucursal.IVAActual;
            }
            else
            {
                ConceptoProyecto.IVA = 0;
            }


            ConversionMoneda = Convert.ToInt32(pProyecto["ConversionMoneda"]);

            if (ConversionMoneda == 1)
            {
                IdTipoMonedaConversion = Convert.ToInt32(pProyecto["IdTipoMonedaConversion"]);
                TipoCambioConversion = Convert.ToDecimal(pProyecto["TipoCambioConversion"]);

                if (Convert.ToInt32(pProyecto["IdTipoMoneda"]) == IdTipoMonedaConversion)
                {
                    ConceptoProyecto.Monto = Convert.ToDecimal(pProyecto["Monto"]);
                }
                else if (Convert.ToInt32(pProyecto["IdTipoMoneda"]) == 1)
                {
                    ConceptoProyecto.Monto = Convert.ToDecimal(pProyecto["Monto"]) / TipoCambioConversion;
                }
                else
                {
                    ConceptoProyecto.Monto = Convert.ToDecimal(pProyecto["Monto"]) * TipoCambioConversion;
                }

                ConceptoProyecto.IdTipoMoneda = Convert.ToInt32(IdTipoMonedaConversion);

            }
            else
            {
                ConceptoProyecto.Monto = Convert.ToDecimal(pProyecto["Monto"]);
                ConceptoProyecto.IdTipoMoneda = Convert.ToInt32(pProyecto["IdTipoMoneda"]);
            }

            string validacion = ValidarConceptoProyecto(ConceptoProyecto, ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                if (Convert.ToString(pProyecto["IdSolicitudFactura"]) == "")
                {
                    CSolicitudFacturacion SolicitudFactura = new CSolicitudFacturacion();
                    SolicitudFactura.IdProyecto = Convert.ToInt32(pProyecto["IdProyecto"]);
                    SolicitudFactura.IdCliente = Convert.ToInt32(pProyecto["IdCliente"]);
                    SolicitudFactura.Solicitud = Convert.ToString(pProyecto["Solicitud"]);
                    SolicitudFactura.IdEstatusSolicitudFacturacion = 1;
                    validacion = ValidarSolicitud(SolicitudFactura, ConexionBaseDatos);
                    if (validacion == "")
                    {
                        SolicitudFactura.OrdenFactura = SolicitudFactura.ObtenerMaximoOrdenFactura(SolicitudFactura.IdProyecto, ConexionBaseDatos);
                        SolicitudFactura.Agregar(ConexionBaseDatos);
                        ConceptoProyecto.IdSolicitudFacturacion = SolicitudFactura.IdSolicitudFacturacion;
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                        oRespuesta.Add(new JProperty("Descripcion", validacion));
                    }
                }
                else
                {
                    ConceptoProyecto.IdSolicitudFacturacion = Convert.ToInt32(pProyecto["IdSolicitudFactura"]);
                }
                ConceptoProyecto.OrdenConcepto = ConceptoProyecto.MaximoNumeroOrden(ConceptoProyecto.IdSolicitudFacturacion, ConexionBaseDatos);
                ConceptoProyecto.Agregar(ConexionBaseDatos);

                // Actualiza Proyecto
                if (ConceptoProyecto.IdProyecto != 0)
                {
                    CProyecto.ActualizarTotales(ConceptoProyecto.IdProyecto, ConexionBaseDatos);
                }

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = ConceptoProyecto.IdConceptoProyecto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un nuevo concepto de proyecto";
                HistorialGenerico.AgregarHistorialGenerico("ConceptoProyecto", ConexionBaseDatos);

                oRespuesta.Add("IdProyecto", ConceptoProyecto.IdProyecto);
                oRespuesta.Add("IdSolicitudFactura", ConceptoProyecto.IdSolicitudFacturacion);
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
    public static string AgregarSolicitud(Dictionary<string, object> pProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CSolicitudFacturacion SolicitudFacturacion = new CSolicitudFacturacion();
            SolicitudFacturacion.LlenaObjeto(Convert.ToInt32(pProyecto["IdSolicitudFactura"]), ConexionBaseDatos);
            SolicitudFacturacion.Solicitud = Convert.ToString(pProyecto["Solicitud"]);
            SolicitudFacturacion.IdCliente = Convert.ToInt32(pProyecto["IdCliente"]);
            SolicitudFacturacion.IdProyecto = Convert.ToInt32(pProyecto["IdProyecto"]);
            SolicitudFacturacion.IdEstatusSolicitudFacturacion = 1;

            string validacion = ValidarSolicitud(SolicitudFacturacion, ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                SolicitudFacturacion.OrdenFactura = SolicitudFacturacion.ObtenerMaximoOrdenFactura(SolicitudFacturacion.IdProyecto, ConexionBaseDatos);
                SolicitudFacturacion.Editar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = SolicitudFacturacion.IdSolicitudFacturacion;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nueva solicitud de facturacion para proyectos";
                HistorialGenerico.AgregarHistorialGenerico("SolicitudFacturacion", ConexionBaseDatos);

                oRespuesta.Add("IdProyecto", SolicitudFacturacion.IdProyecto);
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
    public static string EditarProyecto(Dictionary<string, object> pProyecto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int IdProyecto = Convert.ToInt32(pProyecto["IdProyecto"]);

        CProyecto Proyecto = new CProyecto();
        Proyecto.LlenaObjeto(Convert.ToInt32(pProyecto["IdProyecto"]), ConexionBaseDatos);
        Proyecto.NombreProyecto = Convert.ToString(pProyecto["NombreProyecto"]);
        Proyecto.IdCliente = Convert.ToInt32(pProyecto["IdCliente"]);
        Proyecto.FechaInicio = Convert.ToDateTime(pProyecto["FechaInicio"]);
        Proyecto.FechaTermino = Convert.ToDateTime(pProyecto["FechaTermino"]);
        Proyecto.PrecioTeorico = Convert.ToDecimal(pProyecto["PrecioTeorico"]);
        Proyecto.CostoTeorico = Convert.ToDecimal(pProyecto["CostoTeorico"]);
        Proyecto.Notas = Convert.ToString(pProyecto["Notas"]);
        Proyecto.IdTipoMoneda = Convert.ToInt32(pProyecto["IdTipoMoneda"]);
        Proyecto.TipoCambio = Convert.ToDecimal(pProyecto["TipoCambio"]);
        Proyecto.IdUsuarioResponsable = Convert.ToInt32(pProyecto["IdUsuarioResponsable"]);
        Proyecto.IdEstatusProyecto = Convert.ToInt32(pProyecto["IdEstatusProyecto"]);
        Proyecto.IdOportunidad = Convert.ToInt32(pProyecto["IdOportunidad"]);
        Proyecto.IdNivelInteres = Convert.ToInt32(pProyecto["IdNivelInteres"]);
        Proyecto.IdDivision = Convert.ToInt32(pProyecto["IdDivision"]);

        string validacion = ValidarProyecto(Proyecto, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CProyecto.ActualizarTotales(IdProyecto, ConexionBaseDatos);
            Proyecto.Editar(ConexionBaseDatos);

            COportunidad.ActualizarTotalesOportunidad(Convert.ToInt32(pProyecto["IdOportunidad"]), ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Proyecto.IdProyecto;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico el proyecto";
            HistorialGenerico.AgregarHistorialGenerico("Proyecto", ConexionBaseDatos);

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
    public static string EditarConceptoProyecto(Dictionary<string, object> pConcepto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int ConversionMoneda = 0;
        int IdTipoMonedaConversion = 0;
        decimal TipoCambioConversion = 0;
        //¿La conexion se establecio?

        if (respuesta == "Conexion Establecida")
        {
            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();
            ConceptoProyecto.LlenaObjeto(Convert.ToInt32(pConcepto["IdConcepto"]), ConexionBaseDatos);
            int idTipoIVA_Anterior = ConceptoProyecto.IdTipoIVA;

            ConceptoProyecto = new CConceptoProyecto();
            ConceptoProyecto.LlenaObjeto(Convert.ToInt32(pConcepto["IdConcepto"]), ConexionBaseDatos);
            ConceptoProyecto.IdConceptoProyecto = Convert.ToInt32(pConcepto["IdConcepto"]);
            ConceptoProyecto.Descripcion = Convert.ToString(pConcepto["NombreConcepto"]);
            ConceptoProyecto.IdProyecto = Convert.ToInt32(pConcepto["IdProyecto"]);
            ConceptoProyecto.IdUnidadCompraVenta = Convert.ToInt32(pConcepto["IdUnidadCompraVenta"]);
            ConceptoProyecto.IdTipoVenta = Convert.ToInt32(pConcepto["IdTipoVenta"]);
            ConceptoProyecto.IdTipoMoneda = Convert.ToInt32(pConcepto["IdTipoMoneda"]);
            ConceptoProyecto.Monto = Convert.ToDecimal(pConcepto["Monto"]);
            ConceptoProyecto.Cantidad = Convert.ToInt32(pConcepto["Cantidad"]);
            ConceptoProyecto.IdTipoIVA = Convert.ToInt32(pConcepto["IdTipoIVA"]);
            ConceptoProyecto.ClaveProdServ = Convert.ToString(pConcepto["ClaveProdServ"]);

            if (ConceptoProyecto.IdTipoIVA == 1)
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
                ConceptoProyecto.IVA = Sucursal.IVAActual;
            }
            else
            {
                ConceptoProyecto.IVA = 0;
            }

            ConversionMoneda = Convert.ToInt32(pConcepto["ConversionMoneda"]);

            if (ConversionMoneda == 1)
            {
                IdTipoMonedaConversion = Convert.ToInt32(pConcepto["IdTipoMonedaConversion"]);
                TipoCambioConversion = Convert.ToDecimal(pConcepto["TipoCambioConversion"]);
                if (Convert.ToInt32(pConcepto["IdTipoMoneda"]) == IdTipoMonedaConversion)
                {
                    ConceptoProyecto.Monto = Convert.ToDecimal(pConcepto["Monto"]);
                }
                else if (Convert.ToInt32(pConcepto["IdTipoMoneda"]) == 1)
                {
                    ConceptoProyecto.Monto = Convert.ToDecimal(pConcepto["Monto"]) / TipoCambioConversion;
                }
                else
                {
                    ConceptoProyecto.Monto = Convert.ToDecimal(pConcepto["Monto"]) * TipoCambioConversion;
                }

                ConceptoProyecto.IdTipoMoneda = Convert.ToInt32(IdTipoMonedaConversion);
            }
            else
            {
                ConceptoProyecto.Monto = Convert.ToDecimal(pConcepto["Monto"]);
                ConceptoProyecto.IdTipoMoneda = Convert.ToInt32(pConcepto["IdTipoMoneda"]);
            }


            string validacion = ValidarConceptoProyecto(ConceptoProyecto, ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                oRespuesta.Add("IdSolicitudFactura", ConceptoProyecto.IdSolicitudFacturacion);
                ConceptoProyecto.Editar(ConexionBaseDatos);

                string cambioIVA = string.Empty;
                if (idTipoIVA_Anterior != ConceptoProyecto.IdTipoIVA)
                {
                    string TipoIVA_Anterior = string.Empty;
                    string TipoIVA_Actual = string.Empty;

                    CTipoIVA TipoIVA = new CTipoIVA();
                    TipoIVA.LlenaObjeto(idTipoIVA_Anterior, ConexionBaseDatos);
                    TipoIVA_Anterior = TipoIVA.TipoIVA;

                    TipoIVA = new CTipoIVA();
                    TipoIVA.LlenaObjeto(ConceptoProyecto.IdTipoIVA, ConexionBaseDatos);
                    TipoIVA_Actual = TipoIVA.TipoIVA;

                    cambioIVA = "El IVA cambio de" + TipoIVA_Anterior + " a " + TipoIVA_Actual;
                }


                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = ConceptoProyecto.IdConceptoProyecto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se edito un concepto de proyecto. " + cambioIVA;
                HistorialGenerico.AgregarHistorialGenerico("ConceptoProyecto", ConexionBaseDatos);
                
                //Actualiza todas las FacturaDetalle
                CSelectEspecifico actualizar = new CSelectEspecifico();
                actualizar.StoredProcedure.CommandText = "sp_ConceptoProyecto_FacturaDetalle_ActualizarClaveProdServ";
                actualizar.StoredProcedure.Parameters.Add("@ClaveProdServ", SqlDbType.VarChar, 50).Value = Convert.ToString(pConcepto["ClaveProdServ"]);
                actualizar.StoredProcedure.Parameters.Add("@IdConceptoProyecto", SqlDbType.Int).Value = ConceptoProyecto.IdConceptoProyecto;
                actualizar.StoredProcedure.Parameters.Add("@IdProyecto", SqlDbType.Int).Value = ConceptoProyecto.IdProyecto;
                actualizar.Llena(ConexionBaseDatos);
                actualizar.CerrarConsulta();


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
    public static string ActualizarOrdenamiento(Dictionary<string, object> pConcepto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        int Orden = 1;
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();
            List<CConceptoProyecto> Conceptos = new List<CConceptoProyecto>();
            foreach (string oConcepto in (Array)pConcepto["Conceptos"])
            {
                CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();
                ConceptoProyecto.IdConceptoProyecto = Convert.ToInt32(oConcepto);
                ConceptoProyecto.OrdenConcepto = Orden;
                ConceptoProyecto.EditarOrdenamiento(ConexionBaseDatos);
                Orden++;
            }
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ActualizarOrdenamientoSolicitudFacturas(Dictionary<string, object> pSolicitud)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        int Orden = 1;
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            List<CSolicitudFacturacion> Solicitudes = new List<CSolicitudFacturacion>();
            foreach (string oSolicitud in (Array)pSolicitud["Solicitudes"])
            {
                CSolicitudFacturacion SolicitudFactura = new CSolicitudFacturacion();
                SolicitudFactura.IdSolicitudFacturacion = Convert.ToInt32(oSolicitud);
                SolicitudFactura.OrdenFactura = Orden;
                SolicitudFactura.EditarOrdenamiento(ConexionBaseDatos);
                Orden++;
            }
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EditarSolicitudFactura(Dictionary<string, object> pSolicitud)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CSolicitudFacturacion SolicitudFactura = new CSolicitudFacturacion();
            SolicitudFactura.LlenaObjeto(Convert.ToInt32(pSolicitud["IdSolicitudFactura"]), ConexionBaseDatos);
            SolicitudFactura.IdSolicitudFacturacion = Convert.ToInt32(pSolicitud["IdSolicitudFactura"]);
            SolicitudFactura.IdCliente = Convert.ToInt32(pSolicitud["IdCliente"]);
            SolicitudFactura.IdProyecto = Convert.ToInt32(pSolicitud["IdProyecto"]);
            SolicitudFactura.Solicitud = Convert.ToString(pSolicitud["Solicitud"]);

            string validacion = ValidarSolicitud(SolicitudFactura, ConexionBaseDatos);
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                SolicitudFactura.Editar(ConexionBaseDatos);

                // Actualiza Proyecto
                if (SolicitudFactura.IdProyecto != 0)
                {
                    CProyecto.ActualizarTotales(SolicitudFactura.IdProyecto, ConexionBaseDatos);
                }

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = SolicitudFactura.IdSolicitudFacturacion;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se edito la solicitud de factura";
                HistorialGenerico.AgregarHistorialGenerico("SolicitudFacturacion", ConexionBaseDatos);

                oRespuesta.Add("IdProyecto", Convert.ToInt32(pSolicitud["IdProyecto"]));
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
    public static string CambiarEstatus(int pIdProyecto, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProyecto Proyecto = new CProyecto();
            Proyecto.LlenaObjeto(pIdProyecto, ConexionBaseDatos);

            Proyecto.IdProyecto = pIdProyecto;
            Proyecto.Baja = pBaja;
            Proyecto.Eliminar(ConexionBaseDatos);
            respuesta = "0|ProyectoEliminado";

            COportunidad.ActualizarTotalesOportunidad(Proyecto.IdOportunidad, ConexionBaseDatos);

        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string EliminarConcepto(int pIdConcepto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();
            ConceptoProyecto.LlenaObjeto(pIdConcepto, ConexionBaseDatos);
            ConceptoProyecto.IdConceptoProyecto = pIdConcepto;
            ConceptoProyecto.Baja = true;
            ConceptoProyecto.Eliminar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("IdSolicitudFactura", ConceptoProyecto.IdSolicitudFacturacion));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EliminarSolicitud(int pIdSolicitud)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();

            CSolicitudFacturacion SolicitudFacturacion = new CSolicitudFacturacion();
            SolicitudFacturacion.LlenaObjeto(pIdSolicitud, ConexionBaseDatos);
            SolicitudFacturacion.IdSolicitudFacturacion = pIdSolicitud;
            SolicitudFacturacion.Baja = true;

            if (!ConceptoProyecto.ExistenConceptosEnSolicitud(pIdSolicitud, ConexionBaseDatos))
            {
                SolicitudFacturacion.Eliminar(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("IdProyecto", SolicitudFacturacion.IdProyecto));
            }
            else
            {
                respuesta = "Esta solicitud no se puede eliminar, porque tiene conceptos asignados, elimine los conceptos";
                oRespuesta.Add(new JProperty("Error", 1));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
        }
        oRespuesta.Add(new JProperty("Descripcion", respuesta));

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaUnidadCompraVenta()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonUnidadCompraVenta(true, ConexionBaseDatos));
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
    public static string ObtenerListaOportunidad(int pIdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", COportunidad.ObtenerOportunidadProyecto(pIdCliente, Usuario.IdUsuario, ConexionBaseDatos));
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
    public static string ObtenerListaNivelInteres(int pIdOportunidad)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            COportunidad Oportunidad = new COportunidad();
            Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CNivelInteresCotizacion.ObtenerJsonNivelInteresCotizacion(Oportunidad.IdNivelInteresOportunidad, ConexionBaseDatos));
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
    public static string ObtenerListaDivision(int pIdOportunidad)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            COportunidad Oportunidad = new COportunidad();
            Oportunidad.LlenaObjeto(pIdOportunidad, ConexionBaseDatos);

            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CNivelInteresCotizacion.ObtenerJsonNivelInteresCotizacion(Oportunidad.IdNivelInteresOportunidad, ConexionBaseDatos));
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

    private static string ValidarProyecto(CProyecto pProyecto, CConexion pConexion)
    {
        string errores = "";

        if (pProyecto.NombreProyecto == "")
        { errores = errores + "<span>*</span> El campo nombre del proyecto esta vacío, favor de capturarlo.<br />"; }

        if (pProyecto.IdCliente == 0)
        { errores = errores + "<span>*</span> Debe de seleccionar un cliente de la lista, favor de seleccionarlo.<br />"; }

        if (pProyecto.IdOportunidad == 0)
        { errores = errores + "<span>*</span> Debe de seleccionar una oportunidad, favor de seleccionarlo.<br />"; }

		if (pProyecto.IdDivision == 0)
		{ errores = errores + "<span></span> Debe de seleccionar una especialidad, favor de seleccionarlo.<br />"; }

        if (pProyecto.IdNivelInteres == 0)
        { errores = errores + "<span>*</span> Debe de seleccionar un nivel de interes, favor de seleccionarlo.<br />"; }

        if (pProyecto.CostoTeorico == 0)
        { errores = errores + "<span>*</span> El campo costo teorico esta vacío, favor de capturarlo.<br />"; }

        if (pProyecto.PrecioTeorico == 0)
        { errores = errores + "<span>*</span> El campo precio teorico esta vacío, favor de capturarlo.<br />"; }

        if (pProyecto.PrecioTeorico <= pProyecto.CostoTeorico)
        { errores = errores + "<span>*</span> El costo del proyecto no puede ser mayor o igual al precio, debe tener una ganancia<br />"; }

        if (pProyecto.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacío, favor de seleccionarlo.<br />"; }

        if (pProyecto.TipoCambio == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacío, favor de seleccionarlo.<br />"; }

        if (pProyecto.IdTipoMoneda == 1 && pProyecto.TipoCambio != 1)
        { errores = errores + "<span>*</span> La moneda pesos no puede tener un tipo de cambio distinto de 1<br />"; }

        if (pProyecto.IdUsuarioResponsable == 0)
        { errores = errores + "<span>*</span> El campo usuario responsable esta vacío, favor de seleccionarlo.<br />"; }

        if (pProyecto.Notas == "")
        { errores = errores + "<span>*</span> El campo notas esta vacío, favor de capturarlo.<br />"; }

		CProyecto Proyectos = new CProyecto();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdOportunidad", pProyecto.IdOportunidad);
		pParametros.Add("Baja", 0);

		if (Proyectos.LlenaObjetosFiltros(pParametros, pConexion).Count > 0)
		{ errores += "<p>La oportunidad ya tiene un proyecto asignado.</p>"; }

		CCotizacion Cotizaciones = new CCotizacion();
		if (Cotizaciones.LlenaObjetosFiltros(pParametros, pConexion).Count > 0)
		{ errores += "<p>La oportunidad ya tiene un pedido asignado.</p>"; }

		if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarConceptoProyecto(CConceptoProyecto pConceptoProyecto, CConexion pConexion)
    {
        string errores = "";

        if (pConceptoProyecto.Descripcion == "")
        { errores = errores + "<span>*</span> El campo nombre del concepto esta vacío, favor de capturarlo.<br />"; }

        if (pConceptoProyecto.IdUnidadCompraVenta == 0)
        { errores = errores + "<span>*</span> El campo unidad de compra venta esta vacio, favor de seleccionarlo.<br />"; }

        if (pConceptoProyecto.IdTipoVenta == 0)
        { errores = errores + "<span>*</span> El campo tipo de venta esta vacio, favor de seleccionarlo.<br />"; }

        if (pConceptoProyecto.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (pConceptoProyecto.Cantidad == 0)
        { errores = errores + "<span>*</span> El campo cantidad esta vacio, favor de seleccionarlo.<br />"; }


        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarSolicitud(CSolicitudFacturacion SolicitudFacturacion, CConexion pConexion)
    {
        string errores = "";

        if (SolicitudFacturacion.IdProyecto == 0)
        { errores = errores + "<span>*</span> Debe de seleccionar un proyecto, elegirlo de la lista.<br />"; }

        if (SolicitudFacturacion.IdCliente == 0)
        { errores = errores + "<span>*</span> Debe de seleccionar un cliente, elegirlo de la lista.<br />"; }

        if (SolicitudFacturacion.Solicitud == "")
        { errores = errores + "<span>*</span> El campo solicitud esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

}