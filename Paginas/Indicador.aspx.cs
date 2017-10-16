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
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;

public partial class Indicadores : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ObtenerIndicadores(int IdUsuario)
	{
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				if (IdUsuario == 0)
				{
					IdUsuario = UsuarioSesion.IdUsuario;
				}

				CUsuario Usuario = new CUsuario();
				Usuario.LlenaObjeto(IdUsuario, pConexion);

				CSucursal Sucursal = new CSucursal();
				Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);

				CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
				Dictionary<string, object> Parametros = new Dictionary<string, object>();
				Parametros.Add("IdSucursal", Sucursal.IdSucursal);
				Parametros.Add("IdUsuario", UsuarioSesion.IdUsuario);
				Parametros.Add("Baja", 0);
				SucursalAsignada.LlenaObjetoFiltros(Parametros, pConexion);

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_Inidicadores";
				Consulta.StoredProcedure.Parameters.Add("IdEmpresa", SqlDbType.Int).Value = Sucursal.IdEmpresa;
				Consulta.StoredProcedure.Parameters.Add("IdUsuario", SqlDbType.Int).Value = Usuario.IdUsuario;
				Consulta.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = Sucursal.IdSucursal;
				Consulta.Llena(pConexion);

				if (Consulta.Registros.Read())
				{
					int TotalClientes = Convert.ToInt32(Consulta.Registros["TotalClientes"]);
					int ClientesAtendidos = Convert.ToInt32(Consulta.Registros["ClientesAtendidos"]);
					decimal Meta = Convert.ToDecimal(Consulta.Registros["Meta"]);
					decimal Venta = Convert.ToDecimal(Consulta.Registros["Venta"]);
					int TotalOportunidades = Convert.ToInt32(Consulta.Registros["TotalOportunidades"]);
					int OportunidadesConSeguimiento = Convert.ToInt32(Consulta.Registros["OportunidadesConSeguimiento"]);

					Modelo.Add("EsVendedor", UsuarioSesion.EsVendedor);
					Modelo.Add("TotalClientes", TotalClientes);
					Modelo.Add("ClientesAtendidos", ClientesAtendidos);
					Modelo.Add("Meta", Meta);
					Modelo.Add("Venta", Venta);
					Modelo.Add("TotalOportunidades", TotalOportunidades);
					Modelo.Add("OportunidadesConSeguimiento", OportunidadesConSeguimiento);
				}

				Consulta.CerrarConsulta();
				Modelo.Add("IdUsuario", UsuarioSesion.IdUsuario);
				Modelo.Add("IdPerfil", SucursalAsignada.IdPerfil);
				Modelo.Add("Usuarios", CUsuario.ObtenerJsonAgentes(Sucursal.IdEmpresa, pConexion));

				Respuesta.Add("Modelo", Modelo);

			}

			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);

		});
		return Respuesta.ToString();
	}

}