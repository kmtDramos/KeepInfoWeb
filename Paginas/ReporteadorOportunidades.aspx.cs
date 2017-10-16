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

public partial class Paginas_ReporteadorOportunidades : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}
		
	[WebMethod]
	public static string ObtenerTotalesReporteador(string Cliente, string Agente, int IdSucursal, int IdDivision, string FechaInicial, string FechaFinal)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{

				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_ReporteadorOportunidades_Totales";
				Consulta.StoredProcedure.Parameters.Add("Cliente", SqlDbType.VarChar, 250).Value = Cliente;
				Consulta.StoredProcedure.Parameters.Add("Agente", SqlDbType.VarChar, 250).Value = Agente;
				Consulta.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
				Consulta.StoredProcedure.Parameters.Add("IdDivision", SqlDbType.Int).Value = IdDivision;
				Consulta.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
				Consulta.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;

				Consulta.Llena(pConexion);

				JArray Totales = new JArray();

				while (Consulta.Registros.Read())
				{
					JArray Fila = new JArray();
					Fila.Add(Convert.ToString(Consulta.Registros["Division"]));
					Fila.Add(Convert.ToString(Consulta.Registros["Oportunidades"]));
					Fila.Add(Convert.ToString(Consulta.Registros["Costo"]));
					Fila.Add(Convert.ToString(Consulta.Registros["MargenReal"]));
					Fila.Add(Convert.ToString(Consulta.Registros["LimiteMargen"]));
					Fila.Add(Convert.ToString(Consulta.Registros["Utilidad"]));
					Totales.Add(Fila);
				}

				Consulta.CerrarConsulta();

				CSelectEspecifico Consulta2 = new CSelectEspecifico();
				Consulta2.StoredProcedure.CommandText = "sp_ReporteadorOportunidades_Detalle";
				Consulta2.StoredProcedure.Parameters.Add("Cliente", SqlDbType.VarChar, 250).Value = Cliente;
				Consulta2.StoredProcedure.Parameters.Add("Agente", SqlDbType.VarChar, 250).Value = Agente;
				Consulta2.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
				Consulta2.StoredProcedure.Parameters.Add("IdDivision", SqlDbType.Int).Value = IdDivision;
				Consulta2.StoredProcedure.Parameters.Add("FechaInicial", SqlDbType.VarChar, 10).Value = FechaInicial;
				Consulta2.StoredProcedure.Parameters.Add("FechaFinal", SqlDbType.VarChar, 10).Value = FechaFinal;

				Consulta2.Llena(pConexion);

				JArray Detalle = new JArray();

				while (Consulta2.Registros.Read())
				{
					JArray Fila = new JArray();
					Fila.Add(Convert.ToString(Consulta2.Registros["IdOportunidad"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Oportunidad"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["RazonSocial"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Agente"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Sucursal"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Division"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Dias"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Monto"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Costo"]));
					Fila.Add(Convert.ToString(Consulta2.Registros["Utilidad"]));
					Detalle.Add(Fila);
				}

				Consulta2.CerrarConsulta();


				Modelo.Add("Totales", Totales);
				Modelo.Add("Detalle", Detalle);

				Respuesta.Add("Modelo", Modelo);

			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

}