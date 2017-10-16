using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;

public partial class CUsuarioCuentaBancaria
{
	//Constructores
	
	//Metodos Especiales
    public void BajaCuentaBancariaUsuario(CConexion pConexion)
    {
        CConsultaAccion Eliminar = new CConsultaAccion();
        Eliminar.StoredProcedure.CommandText = "sp_BajaCuentaBancariasPorUsuario";
        Eliminar.StoredProcedure.Parameters.AddWithValue("@IdCuentaBancaria", IdCuentaBancaria);
        Eliminar.Delete(pConexion);
    }

    public void EnrolarCuentaBancariaUsuario(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();

        Agregar.StoredProcedure.CommandText = "sp_EnrolarCuentaBancariaUsuario";
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdCuentaBancaria", IdCuentaBancaria);
        Agregar.StoredProcedure.Parameters.AddWithValue("@IdUsuario", IdUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@PuedeVerSaldo", PuedeVerSaldo);
        Agregar.Insert(pConexion);
    }

    public List<object> LlenaUsuariosDisponibles(int pIdCuentaBancaria, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_UsuariosDisponibles_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdCuentaBancaria", pIdCuentaBancaria);
        Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaUsuariosTodos(int pIdCuentaBancaria, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_UsuariosTodos_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdCuentaBancaria", pIdCuentaBancaria);
        Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
        return Obten.ListaRegistros;
    }

    public List<object> LlenaUsuariosAsignados(int pIdCuentaBancaria, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_UsuariosAsignados_Consultar";
        Obten.StoredProcedure.Parameters.AddWithValue("@IdCuentaBancaria", pIdCuentaBancaria);
        Obten.Llena<CUsuario>(typeof(CUsuario), pConexion);
        return Obten.ListaRegistros;
    }

}
