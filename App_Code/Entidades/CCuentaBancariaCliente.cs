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


public partial class CCuentaBancariaCliente
{
    //Constructores

    //Metodos Especiales
    public bool RevisarExisteRegistro(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        bool existeCuentaBancaria = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_CuentaBancariaCliente_Consultar_ExisteCuentaBancariaCliente";
        Select.StoredProcedure.Parameters.AddWithValue("@pCuentaBancariaCliente", pParametros["CuentaBancariaCliente"]);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdCliente", pParametros["IdCliente"]);
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

    public static JObject ObtenerJsonCuentaBancariaCliente(JObject pModelo, int pIdCuentaBancariaCliente, CConexion pConexion)
    {

        CCuentaBancariaCliente CuentaBancariaCliente = new CCuentaBancariaCliente();
        CuentaBancariaCliente.LlenaObjeto(pIdCuentaBancariaCliente, pConexion);

        pModelo.Add("IdCuentaBancariaCliente", CuentaBancariaCliente.IdCuentaBancariaCliente);
        pModelo.Add("CuentaBancariaCliente", CuentaBancariaCliente.CuentaBancariaCliente);
        pModelo.Add("Descripcion", CuentaBancariaCliente.Descripcion);

        CBanco Banco = new CBanco();
        Banco.LlenaObjeto(CuentaBancariaCliente.IdBanco, pConexion);
        pModelo.Add("IdBanco", Banco.IdBanco);
        pModelo.Add("Banco", Banco.Banco);

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(CuentaBancariaCliente.IdTipoMoneda, pConexion);
        pModelo.Add("IdTipoMoneda", TipoMoneda.IdTipoMoneda);
        pModelo.Add("TipoMoneda", TipoMoneda.TipoMoneda);

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(CuentaBancariaCliente.IdCliente, pConexion);
        pModelo.Add("IdCliente", Cliente.IdCliente);
        if (CuentaBancariaCliente.IdMetodoPago != 0)
        {
            CMetodoPago MetodoPago = new CMetodoPago();
            MetodoPago.LlenaObjeto(CuentaBancariaCliente.IdMetodoPago, pConexion);
            pModelo.Add("IdMetodoPago", MetodoPago.IdMetodoPago);
            pModelo.Add("MetodoPago", MetodoPago.MetodoPago);
        }
        else
        {
            pModelo.Add("IdMetodoPago", 0);
            pModelo.Add("MetodoPago", "No identificado");
        }

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        pModelo.Add("RazonSocial", Organizacion.RazonSocial);


        return pModelo;
    }

}