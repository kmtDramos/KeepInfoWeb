using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;


public partial class CSucursalAsignada
{
    //Constructores

    //Metodos Especiales

    public List<object> ObtenerSucursalesDisponibles(int pIdUsuario, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_SucursalAsignadaUsuario_Consultar_ObtenerSucursalesDisponibles";
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pIdUsuario);
        Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
        return Obten.ListaRegistros;
    }
    public List<object> ObtenerSucursalesAsignadas(int pIdUsuario, CConexion pConexion)
    {
        CSelect Obten = new CSelect();
        Obten.StoredProcedure.CommandText = "sp_SucursalAsignadaUsuario_Consultar_ObtenerSucursalesAsignadas";
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pIdUsuario);
        Obten.Llena<CSucursal>(typeof(CSucursal), pConexion);
        return Obten.ListaRegistros;
    }

    public bool RevisarExisteRegistro(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        bool existeSucursalAsignada = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_SucursalAsignada_Consultar_ExisteSucursalAsignada";
        Select.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pParametros["IdUsuario"]);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pParametros["IdSucursal"]);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["NoSucursalesAsignadas"]) > 0)
            {
                existeSucursalAsignada = true;
            }
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return existeSucursalAsignada;

    }

    public void EditarDatos(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_SucursalAsignada_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAsignada", idSucursalAsignada);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }

    public void EditarCampoBaja(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_SucursalAsignada_Editar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalAsignada", idSucursalAsignada);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", idSucursal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdPerfil", idPerfil);
        Editar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Editar.Update(pConexion);
    }
}