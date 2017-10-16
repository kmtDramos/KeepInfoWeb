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
using System.IO;


public partial class AnalisisCartera : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				GenerarGridCartera(this, ClientScript, pConexion);
			}
		});

	}

	private void GenerarGridCartera(Page Pagina, ClientScriptManager ClienteScript,CConexion pConexion)
	{

		CJQGrid GridAnalisisCartera = new CJQGrid();
		GridAnalisisCartera.NombreTabla = "grdAnalisisCartera";
		GridAnalisisCartera.CampoIdentificador = "IdCliente";
		GridAnalisisCartera.ColumnaOrdenacion = "Cliente";
		GridAnalisisCartera.TipoOrdenacion = "DESC";
		GridAnalisisCartera.Metodo = "ObtenerAnalisisCartera";
		GridAnalisisCartera.TituloTabla = "Análisis de Cartera";
		GridAnalisisCartera.GenerarGridCargaInicial = true;
		GridAnalisisCartera.GenerarFuncionFiltro = false;
		GridAnalisisCartera.GenerarFuncionTerminado = true;
		GridAnalisisCartera.NumeroFila = true;
		GridAnalisisCartera.Altura = 230;
		GridAnalisisCartera.NumeroRegistros = 50;
		GridAnalisisCartera.RangoNumeroRegistros = "20,50,100";

		CJQColumn ColIdCliente = new CJQColumn();
		ColIdCliente.Nombre = "IdCliente";
		ColIdCliente.Encabezado = "IdCliente";
		ColIdCliente.Oculto = "true";
		ColIdCliente.Buscador = "false";
		GridAnalisisCartera.Columnas.Add(ColIdCliente);

		CJQColumn ColCliente = new CJQColumn();
		ColCliente.Nombre = "Cliente";
		ColCliente.Encabezado = "Cliente";
		ColCliente.Ancho = "180";
		ColCliente.Alineacion = "Left";
		GridAnalisisCartera.Columnas.Add(ColCliente);

		CJQColumn ColTipoCliente = new CJQColumn();
		ColTipoCliente.Nombre = "IdTipoCliente";
		ColTipoCliente.Encabezado = "Tipo de Cliente";
		ColTipoCliente.Ancho = "80";
		ColTipoCliente.Alineacion = "Left";
		ColTipoCliente.TipoBuscador = "Combo";
		ColTipoCliente.StoredProcedure.CommandText = "sp_Cliente_Consultar_TipoCliente";
		GridAnalisisCartera.Columnas.Add(ColTipoCliente);

		CJQColumn ColAgente = new CJQColumn();
		ColAgente.Nombre = "Agente";
		ColAgente.Encabezado = "Agente";
		ColAgente.Ancho = "120";
		ColAgente.Alineacion = "Left";
		GridAnalisisCartera.Columnas.Add(ColAgente);

		CJQColumn ColVendido = new CJQColumn();
		ColVendido.Nombre = "Ventas";
		ColVendido.Encabezado = "Vendido";
		ColVendido.Ancho = "80";
		ColVendido.Alineacion = "Right";
		ColVendido.Buscador = "false";
		GridAnalisisCartera.Columnas.Add(ColVendido);

		CJQColumn ColIngreso = new CJQColumn();
		ColIngreso.Nombre = "Ingresos";
		ColIngreso.Encabezado = "Ingreso";
		ColIngreso.Ancho = "80";
		ColIngreso.Alineacion = "Right";
		ColIngreso.Buscador = "false";
		GridAnalisisCartera.Columnas.Add(ColIngreso);

		CJQColumn ColSaldo = new CJQColumn();
		ColSaldo.Nombre = "Saldo";
		ColSaldo.Encabezado = "Saldo";
		ColSaldo.Ancho = "80";
		ColSaldo.Alineacion = "Right";
		ColSaldo.Buscador = "false";
		GridAnalisisCartera.Columnas.Add(ColSaldo);

		CJQColumn ColVentasPorDivision = new CJQColumn();
		ColVentasPorDivision.Nombre = "VentasDivision";
		ColVentasPorDivision.Encabezado = "Vts x Fam.";
		ColVentasPorDivision.Ancho = "60";
		ColVentasPorDivision.Alineacion = "Center";
		ColVentasPorDivision.Buscador = "false";
		ColVentasPorDivision.Ordenable = "false";
		GridAnalisisCartera.Columnas.Add(ColVentasPorDivision);

		ClienteScript.RegisterStartupScript(Pagina.GetType(), "grdAnalisisCartera", GridAnalisisCartera.GeneraGrid(), true);

	}
	
	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerAnalisisCartera(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCliente, int pIdTipoCliente, string pAgente, string pFechaInicial, string pFechaFinal)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdAnalisisCartera", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("Cliente", SqlDbType.VarChar, 255).Value = pCliente;
		Stored.Parameters.Add("IdTipoCliente", SqlDbType.Int).Value = pIdTipoCliente;
		Stored.Parameters.Add("Agente", SqlDbType.VarChar, 255).Value = pAgente;
		Stored.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = pFechaInicial;
		Stored.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = pFechaFinal;
		Stored.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

	[WebMethod]
	public static string BuscarAgente(string pAgente)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string repuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		COportunidad JsonOportunidad = new COportunidad();
		JsonOportunidad.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_Agente";
		JsonOportunidad.StoredProcedure.Parameters.AddWithValue("@pAgente", pAgente);
		string sJson = JsonOportunidad.ObtenerJsonOportunidad(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return sJson;
	}
	
	[WebMethod]
	public static string BuscarCliente(string pCliente)
	{
		//Abrir Conexion
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

		CJson jsonRazonSocial = new CJson();
		jsonRazonSocial.StoredProcedure.CommandText = "sp_Cliente_Consultar_FiltroPorClienteGrid";
		jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pCliente", pCliente);
		string jsonRazonSocialString = jsonRazonSocial.ObtenerJsonString(ConexionBaseDatos);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return jsonRazonSocialString;
	}
	
	[WebMethod]
	public static string ObtenerTablaVentasClienteDivisiones(int IdCliente)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				JArray Columnas = new JArray();
				JArray Datos = new JArray();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_VentasClienteDivisiones";
				Consulta.StoredProcedure.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
				Consulta.StoredProcedure.Parameters.Add("@IdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

				Consulta.Llena(pConexion);

				int leido = 0;

				while (Consulta.Registros.Read())
				{
					for (int i = 0; i < Consulta.Registros.FieldCount; i++) {
						if (leido == 0)
						{
							JObject Columna = new JObject();
							Columna.Add("Columna", Consulta.Registros.GetName(i));
							Columnas.Add(Columna);
						}
						JObject Dato = new JObject();
						Dato.Add("Dato", Consulta.Registros.GetValue(i).ToString());
						Datos.Add(Dato);
					}
					leido = 1;
				}

				Consulta.CerrarConsulta();

				Modelo.Add("Columnas", Columnas);
				Modelo.Add("Datos", Datos);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

}