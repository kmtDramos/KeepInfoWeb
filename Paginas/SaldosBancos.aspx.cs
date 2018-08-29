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

public partial class Paginas_SaldosBancos : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static string ObtenerSaldosBancos(int IdBanco, int IdCuentaBancaria, string FechaInicio)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_SaldosBancos";
                Consulta.StoredProcedure.Parameters.Add("IdBanco", SqlDbType.Int).Value = IdBanco;
                Consulta.StoredProcedure.Parameters.Add("IdCuentaBancaria", SqlDbType.Int).Value = IdCuentaBancaria;
                Consulta.StoredProcedure.Parameters.Add("FechaInicio", SqlDbType.VarChar, 10).Value = FechaInicio;

                Consulta.Llena(pConexion);

                JArray Saldos = new JArray();

                while (Consulta.Registros.Read())
                {
                    JObject Saldo = new JObject();
                    Saldo.Add("Banco", Convert.ToString(Consulta.Registros["Banco"]));
                    Saldo.Add("CuentaBancaria", Convert.ToString(Consulta.Registros["CuentaBancaria"]));
                    Saldo.Add("SaldoFinal", Convert.ToString(Consulta.Registros["SaldoFinal"]));
                    Saldos.Add(Saldo);
                }

                Modelo.Add("Saldos", Saldos);

                Consulta.CerrarConsulta();

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerBancos()
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure.CommandText = "sp_Banco_ListarBancos";
                Consulta.Llena(pConexion);

                JArray ListaBancos = new JArray();

                while (Consulta.Registros.Read())
                {
                    JObject OpcionBanco = new JObject();
                    OpcionBanco.Add("Valor", Convert.ToInt32(Consulta.Registros["IdBanco"]));
                    OpcionBanco.Add("Descripcion", Convert.ToString(Consulta.Registros["Banco"]));
                    ListaBancos.Add(OpcionBanco);
                }

                Modelo.Add("ListaBancos", ListaBancos);

                Consulta.CerrarConsulta();

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

}