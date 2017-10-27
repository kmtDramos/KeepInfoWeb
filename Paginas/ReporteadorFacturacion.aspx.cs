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

public partial class Paginas_ReporteadorFacturacion : System.Web.UI.Page
{

	protected void Page_Load(object sender, EventArgs e)
	{

	}

    [WebMethod]
    public static string ObtenerTotalesReporteador(int IdSucursal, string FechaInicial, string FechaFinal, string Usuario)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {

                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_ReporteadorFacturacion_FacturacionDivision";
                Consulta.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
                Consulta.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 10).Value = FechaInicial;
                Consulta.StoredProcedure.Parameters.Add("FechaFin", SqlDbType.VarChar, 10).Value = FechaFinal;
				Consulta.StoredProcedure.Parameters.Add("Usuario", SqlDbType.VarChar, 50).Value = Usuario;

                CSelectEspecifico Consulta2 = new CSelectEspecifico();
                Consulta2.StoredProcedure.CommandText = "sp_ReporteadorFacturacion_FacturacionVendedor";
                Consulta2.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucursal;
                Consulta2.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 10).Value = FechaInicial;
                Consulta2.StoredProcedure.Parameters.Add("FechaFin", SqlDbType.VarChar, 10).Value = FechaFinal;
				Consulta2.StoredProcedure.Parameters.Add("Usuario", SqlDbType.VarChar, 50).Value = Usuario;

				JArray Divisiones = CUtilerias.ObtenerConsulta(Consulta, pConexion);
                JArray Vendedores = CUtilerias.ObtenerConsulta(Consulta2, pConexion);

                Modelo.Add("Divisiones", Divisiones);
                Modelo.Add("Vendedores", Vendedores);

                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
	public static string ObtenerSucursales()
	{
		JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSession) {
			if (Error == 0)
			{
                
                JObject Modelo = new JObject();
                Modelo.Add("Sucursales", CSucursal.ObtenerSucursalesEmpresa(pConexion));
                Respuesta.Add("Modelo", Modelo);
            }
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

	[WebMethod]
	public static string ObtenerUsuario(string Usuario)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_Agente";
				Consulta.StoredProcedure.Parameters.Add("pAgente", SqlDbType.VarChar, 50).Value = Usuario;

				Modelo.Add("Usuarios", CUtilerias.ObtenerConsulta(Consulta, pConexion));

				Respuesta.Add("Modelo", Modelo);

			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

}