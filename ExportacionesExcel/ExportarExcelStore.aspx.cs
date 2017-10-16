using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class ExportarExcelStore : System.Web.UI.Page
{
	
	protected void Page_Load(object sender, EventArgs e)
	{
        string Archivo = Convert.ToString(HttpContext.Current.Request.QueryString["Archivo"]);
        string Store = Convert.ToString(HttpContext.Current.Request.QueryString["Store"]);
        string Datos = Convert.ToString(HttpContext.Current.Request.QueryString["Parametros"]);
		Dictionary<string, object> Parametros = new Dictionary<string, object>();
        if (Datos != null)
        {
            JObject Json = (JObject)JsonConvert.DeserializeObject(Datos);
            foreach (JProperty Elemento in Json.Properties())
            {
                Parametros.Add(Elemento.Name, Elemento.Value);
            }
        }
		Parametros.Add("IdEmpresa", Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]));
		DescargarReporte(Store, Parametros, Archivo);
	}
	
	private void DescargarReporte(string Store, Dictionary<string, object> Parametros, string Archivo)
	{
		CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
		{
			if (Error == 0)
			{
				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = Store;
				foreach (string Indice in Parametros.Keys)
				{
					Consulta.StoredProcedure.Parameters.Add(Indice, SqlDbType.VarChar).Value = Parametros[Indice].ToString();
				}
				Consulta.Llena(pConexion);

				HttpContext.Current.Response.ClearContent();
				string documento = "";
				documento += "<html>" + Environment.NewLine;
				documento += "<body>" + Environment.NewLine;
				documento += "<table border='1' cellspacing='0'>" + Environment.NewLine;
				string columnas = "";
				string filas = "";

				bool agregoColumnas = false;
				while (Consulta.Registros.Read())
				{
					filas += "<tr>" + Environment.NewLine;
					for (int i = 0; i < Consulta.Registros.FieldCount; i++)
					{
						if (!agregoColumnas)
						{
							string Columna = "";
							Columna = Consulta.Registros.GetName(i);
							columnas += "<th style='background-color:#486a8f;color:#FFF;'>" + Columna + "</th>" + Environment.NewLine;
							string valor = Consulta.Registros.GetValue(i).ToString();
							valor = valor.Replace("Á", "A")
										 .Replace("É", "E")
										 .Replace("Í", "I")
										 .Replace("Ó", "O")
										 .Replace("Ú", "U")
										 .Replace("á", "a")
										 .Replace("é", "e")
										 .Replace("í", "i")
										 .Replace("ó", "o")
										 .Replace("ú", "u")
										 .Replace("Ñ", "N")
										 .Replace("ñ", "n");
							filas += "<td style='background-color:#c9d8e8;'>" + valor + "</td>" + Environment.NewLine;
						}
						else
						{
							string valor = Consulta.Registros.GetValue(i).ToString();
							valor = valor.Replace("Á", "A")
										 .Replace("É", "E")
										 .Replace("Í", "I")
										 .Replace("Ó", "O")
										 .Replace("Ú", "U")
										 .Replace("á", "a")
										 .Replace("é", "e")
										 .Replace("í", "i")
										 .Replace("ó", "o")
										 .Replace("ú", "u")
										 .Replace("Ñ", "N")
										 .Replace("ñ", "n");
							filas += "<td style='background-color:#c9d8e8;'>" + valor + "</td>" + Environment.NewLine;
						}
					}
					filas += "</tr>" + Environment.NewLine;
					agregoColumnas = true;
				}

				documento += "<tr>" + Environment.NewLine + columnas + "</tr>" + Environment.NewLine + filas;
				documento += "</table>" + Environment.NewLine;
				documento += "</body>" + Environment.NewLine;
				documento += "</html>" + Environment.NewLine;

				Consulta.CerrarConsulta();
				Response.Write(documento);
				Response.AddHeader("content-disposition", "attachment; filename=" + Archivo);
				Response.ContentType = "application/excel; charset=utf-8";
				Response.End();
			}
			else
			{
				Response.Redirect("../");
			}
		});
	}
	
}