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
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net;
using arquitecturaNet;
using System.IO;
using System.Diagnostics;

public partial class ScoreBoard : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	[WebMethod]
	public static string ObtenerResultado()
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccionAnonimo(delegate (CConexion pConexion, int Error, string DescripcionError) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				CSelectEspecifico Consulta = new CSelectEspecifico();
				Consulta.StoredProcedure.CommandText = "sp_ScoreBoard";

				Modelo.Add("Resultados", CUtilerias.ObtenerConsulta(Consulta, pConexion));

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});

		return Respuesta.ToString();
	}

}