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

public partial class Paginas_AgregarOportunidad : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

		JObject Parametros = new JObject();

		foreach (string Key in HttpContext.Current.Request.Form.Keys)
		{
			Parametros.Add(Key, HttpContext.Current.Request.Form.Get(Key));
		}

		System.IO.File.WriteAllText("C:\\inetpub\\Oportunidades\\"+ DateTime.Now.Ticks.ToString().Substring(0,14) +".txt", Parametros.ToString());

	}
}