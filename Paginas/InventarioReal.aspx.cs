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

public partial class Paginas_InventarioReal : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{
		CJQGrid GridInventario = new CJQGrid();
		GridInventario.NombreTabla = "grdInventario";
		GridInventario.CampoIdentificador = "IdExperienciaReal";
		GridInventario.ColumnaOrdenacion = "ClaveInterna";
		GridInventario.TipoOrdenacion = "ASC";
		GridInventario.Metodo = "ObtenerInventario";
		GridInventario.TituloTabla = "Inventario";
		GridInventario.GenerarFuncionFiltro = false;
		GridInventario.GenerarFuncionTerminado = false;
		GridInventario.Altura = 231;
		GridInventario.Ancho = 940;
		GridInventario.NumeroRegistros = 25;
		GridInventario.RangoNumeroRegistros = "25,50,100";

		#region Columnas

		CJQColumn ColIdExperienciaReal = new CJQColumn();
		ColIdExperienciaReal.Nombre = "IdExperienciaReal";
		ColIdExperienciaReal.Encabezado = "IdExperienciaReal";
		ColIdExperienciaReal.Oculto = "true";
		ColIdExperienciaReal.Buscador = "false";
		GridInventario.Columnas.Add(ColIdExperienciaReal);

		CJQColumn ColClaveInterna = new CJQColumn();
        ColClaveInterna.Nombre = "ClaveInterna";
        ColClaveInterna.Encabezado = "Clave Interna";
        ColClaveInterna.Ancho = "120";
		GridInventario.Columnas.Add(ColClaveInterna);

		CJQColumn ColClave = new CJQColumn();
		ColClave.Nombre = "Clave";
		ColClave.Encabezado = "Numero Parte";
		ColClave.Alineacion = "left";
		ColClave.Ancho = "120";
		GridInventario.Columnas.Add(ColClave);

		CJQColumn ColDescripcion = new CJQColumn();
		ColDescripcion.Nombre = "Descripcion";
		ColDescripcion.Encabezado = "Descripcion";
		ColDescripcion.Alineacion = "left";
		ColDescripcion.Ancho = "450";
		GridInventario.Columnas.Add(ColDescripcion);

        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Familia";
        ColDivision.Alineacion = "left";
        ColDivision.Ancho = "120";
        GridInventario.Columnas.Add(ColDivision);

        CJQColumn ColLinea = new CJQColumn();
        ColLinea.Nombre = "Linea";
        ColLinea.Encabezado = "Linea";
        ColLinea.Alineacion = "center";
        ColLinea.Ancho = "80";
        GridInventario.Columnas.Add(ColLinea);

        CJQColumn ColEstante = new CJQColumn();
        ColEstante.Nombre = "Estante";
        ColEstante.Encabezado = "Rack";
        ColEstante.Alineacion = "center";
        ColEstante.Ancho = "80";
        GridInventario.Columnas.Add(ColEstante);

        CJQColumn ColRepisa = new CJQColumn();
        ColRepisa.Nombre = "Repisa";
        ColRepisa.Encabezado = "Repisa";
        ColRepisa.Alineacion = "center";
        ColRepisa.Ancho = "80";
        GridInventario.Columnas.Add(ColRepisa);

        CJQColumn ColCostoPromedio = new CJQColumn();
		ColCostoPromedio.Nombre = "CostoPromedio";
		ColCostoPromedio.Encabezado = "Ultimo Costo";
		ColCostoPromedio.Buscador = "false";
		ColCostoPromedio.Alineacion = "right";
		ColCostoPromedio.Ancho = "90";
		GridInventario.Columnas.Add(ColCostoPromedio);

		CJQColumn ColExistencia = new CJQColumn();
		ColExistencia.Nombre = "Existencia";
		ColExistencia.Encabezado = "Existencia";
		ColExistencia.Buscador = "false";
		ColExistencia.Ancho = "90";
		GridInventario.Columnas.Add(ColExistencia);

		#endregion

		ClientScript.RegisterStartupScript(Page.GetType(), "grdInventario", GridInventario.GeneraGrid(), true);
	}

	[WebMethod]
	[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
	public static CJQGridJsonResponse ObtenerInventario(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pClaveInterna, string pClave, string pDescripcion, string pDivision)
	{
		CConexion ConexionBaseDatos = new CConexion();
		string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
		SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
		SqlCommand Stored = new SqlCommand("spg_grdInventarioReal", sqlCon);

		Stored.CommandType = CommandType.StoredProcedure;
		Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
		Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
		Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
		Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
		Stored.Parameters.Add("pClaveInterna", SqlDbType.VarChar, 50).Value = pClaveInterna;
		Stored.Parameters.Add("pClave", SqlDbType.VarChar, 100).Value = pClave;
		Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 100).Value = pDescripcion;
		Stored.Parameters.Add("pDivision", SqlDbType.VarChar, 100).Value = pDivision;
        
		DataSet dataSet = new DataSet();
		SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
		dataAdapter.Fill(dataSet);
		ConexionBaseDatos.CerrarBaseDatosSqlServer();
		return new CJQGridJsonResponse(dataSet);
	}

	[WebMethod]
	public static string ActualizarExistenciaProducto(int IdExperienciaReal, int Existencia)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CExistenciaReal Inventario = new CExistenciaReal();
				Inventario.LlenaObjeto(IdExperienciaReal, pConexion);

				CExistenciaHistorico InventarioHistorico = new CExistenciaHistorico();
				InventarioHistorico.IdUsuario = UsuarioSesion.IdUsuario;
				InventarioHistorico.CantidadInicial = Inventario.CantidadFinal;
				InventarioHistorico.CantidadFinal = Existencia;
				InventarioHistorico.Fecha = DateTime.Now;
				InventarioHistorico.IdProducto = Inventario.IdProducto;
				InventarioHistorico.IdSucursal = Inventario.IdSucursal;
				InventarioHistorico.IdAlmacen = Inventario.IdAlmacen;
				InventarioHistorico.IdExistenciaReal = Inventario.IdExistenciaReal;
				InventarioHistorico.Agregar(pConexion);

				Inventario.IdUsuario = UsuarioSesion.IdUsuario;
				Inventario.CantidadInicial = Inventario.CantidadFinal;
				Inventario.CantidadFinal = Existencia;
				Inventario.Fecha = DateTime.Now;
				Inventario.Editar(pConexion);

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}



}