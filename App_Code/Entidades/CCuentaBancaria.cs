using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.UI;



public partial class CCuentaBancaria
{
    //Constructores

    //Metodos Especiales
    public bool RevisarExisteRegistro(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        bool existeCuentaBancaria = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_CuentaBancaria_Consultar_ExisteCuentaBancaria";
        Select.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", pParametros["CuentaBancaria"]);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["NoCuentaBancaria"]) > 0)
            {
                existeCuentaBancaria = true;
            }
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return existeCuentaBancaria;
    }

    public static JObject ObtenerCuentaBancaria(JObject pModelo, int pIdCuentaBancaria, CConexion pConexion)
    {
        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        Boolean PuedeVerSaldo = false;
        CuentaBancaria.LlenaObjeto(pIdCuentaBancaria, pConexion);
        pModelo.Add(new JProperty("IdCuentaBancaria", CuentaBancaria.IdCuentaBancaria));
        pModelo.Add(new JProperty("Saldo", CuentaBancaria.saldo));
        pModelo.Add(new JProperty("Descripcion", CuentaBancaria.Descripcion));
        pModelo.Add(new JProperty("CuentaBancaria", CuentaBancaria.CuentaBancaria));
        pModelo.Add(new JProperty("CuentaContable", CuentaBancaria.CuentaContable));
        pModelo.Add(new JProperty("CuentaContableComplemento", CuentaBancaria.CuentaContableComplemento));
        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancaria.IdBanco, pConexion);
        pModelo.Add(new JProperty("IdBanco", Banco.IdBanco));
        pModelo.Add(new JProperty("Banco", Banco.Banco));
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(CuentaBancaria.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoCambio", CTipoCambioDiarioOficial.ObtenerTipoCambio(Convert.ToInt32(TipoMoneda.IdTipoMoneda), DateTime.Now, pConexion)));
        pModelo.Add("Fecha", DateTime.Now.ToShortDateString());

        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
        Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
        ParametrosP.Add("IdCuentaBancaria", pIdCuentaBancaria);
        ParametrosP.Add("IdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));

        foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancaria.LlenaObjetosFiltros(ParametrosP, pConexion))
        {
            PuedeVerSaldo = oCCuentaBancaria.PuedeVerSaldo;
        }
        pModelo.Add(new JProperty("PuedeVerSaldo", PuedeVerSaldo));

        return pModelo;
    }

    public static JObject ObtenerCuentaBancariaDOF(JObject pModelo, int pIdCuentaBancaria, CConexion pConexion)
    {
        CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
        Boolean PuedeVerSaldo = false;
        CuentaBancaria.LlenaObjeto(pIdCuentaBancaria, pConexion);
        pModelo.Add(new JProperty("IdCuentaBancaria", CuentaBancaria.IdCuentaBancaria));
        pModelo.Add(new JProperty("Saldo", CuentaBancaria.saldo));
        pModelo.Add(new JProperty("Descripcion", CuentaBancaria.Descripcion));
        pModelo.Add(new JProperty("CuentaBancaria", CuentaBancaria.CuentaBancaria));
        pModelo.Add(new JProperty("CuentaContable", CuentaBancaria.CuentaContable));
        pModelo.Add(new JProperty("CuentaContableComplemento", CuentaBancaria.CuentaContableComplemento));
        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancaria.IdBanco, pConexion);
        pModelo.Add(new JProperty("IdBanco", Banco.IdBanco));
        pModelo.Add(new JProperty("Banco", Banco.Banco));
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(CuentaBancaria.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoCambio", CTipoCambioDiarioOficial.ObtenerTipoCambio(Convert.ToInt32(2), DateTime.Now, pConexion)));
        pModelo.Add("Fecha", DateTime.Now.ToShortDateString());

        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();
        Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
        ParametrosP.Add("IdCuentaBancaria", pIdCuentaBancaria);
        ParametrosP.Add("IdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));

        foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancaria.LlenaObjetosFiltros(ParametrosP, pConexion))
        {
            PuedeVerSaldo = oCCuentaBancaria.PuedeVerSaldo;
        }
        pModelo.Add(new JProperty("PuedeVerSaldo", PuedeVerSaldo));

        return pModelo;
    }

    public JArray ObtenerJsonUsuariosDisponibles(int pIdCuentaBancaria, CConexion pConexion)
    {
        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria(); 

        JArray JUsuariosDisponibles = new JArray();

        foreach (CUsuario oUsuario in UsuarioCuentaBancaria.LlenaUsuariosDisponibles(pIdCuentaBancaria, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("IdUsuario", oUsuario.IdUsuario);
            JUsuario.Add("Usuario", oUsuario.Usuario);
            JUsuariosDisponibles.Add(JUsuario);
        }
        return JUsuariosDisponibles;
    }

    public JArray ObtenerJsonUsuariosTodos(int pIdCuentaBancaria, CConexion pConexion)
    {
        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria();

        JArray JUsuariosDisponibles = new JArray();

        foreach (CUsuario oUsuario in UsuarioCuentaBancaria.LlenaUsuariosTodos(pIdCuentaBancaria, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("IdUsuario", oUsuario.IdUsuario);
            JUsuario.Add("Usuario", oUsuario.Usuario);
            JUsuariosDisponibles.Add(JUsuario);
        }
        return JUsuariosDisponibles;
    }

    public JArray ObtenerJsonUsuariosAsignados(int pIdCuentaBancaria, CConexion pConexion)
    {

        CUsuarioCuentaBancaria UsuarioCuentaBancaria = new CUsuarioCuentaBancaria(); 
        JArray JUsuariosAsignados = new JArray();

        foreach (CUsuario oUsuario in UsuarioCuentaBancaria.LlenaUsuariosAsignados(pIdCuentaBancaria, pConexion))
        {
            JObject JUsuario = new JObject();
            JUsuario.Add("IdUsuario", oUsuario.IdUsuario);
            JUsuario.Add("Usuario", oUsuario.Usuario);

            CUsuarioCuentaBancaria UsuarioCuentaBancariaS = new CUsuarioCuentaBancaria();
            Dictionary<string, object> ParametrosP = new Dictionary<string, object>();
            ParametrosP.Add("IdCuentaBancaria", pIdCuentaBancaria);
            ParametrosP.Add("IdUsuario", oUsuario.IdUsuario);

            foreach (CUsuarioCuentaBancaria oCCuentaBancaria in UsuarioCuentaBancariaS.LlenaObjetosFiltros(ParametrosP, pConexion))
            {
                JUsuario.Add("PuedeVerSaldo", Convert.ToBoolean(oCCuentaBancaria.PuedeVerSaldo));
            }

            JUsuariosAsignados.Add(JUsuario);
        }
        return JUsuariosAsignados;
    }
}