using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class CCliente
{
    //Constructores
    public SqlCommand StoredProcedure = new SqlCommand();

    //Metodos Especiales
    public int ExisteCliente(String pRFC, int pIdSucursalActual, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteCliente = 0; //No existe ni en cliente ni en proveedor
        ObtenObjeto.StoredProcedure.CommandText = "sp_Cliente_Organizacion_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(pRFC));
        ObtenObjeto.Llena<CClienteSucursal>(typeof(CClienteSucursal), pConexion);
        foreach (CClienteSucursal ClienteSucursal in ObtenObjeto.ListaRegistros)
        {
            if (ClienteSucursal.IdSucursal == pIdSucursalActual)
            {
                ExisteCliente = 1; //En mi sucursal
                break;
            }
            else
            {
                ExisteCliente = 2; //En otra sucursal
            }
        }

        if (ExisteCliente == 0) //No existe en Cliente busco en Proveedor
        {
            CSelect ObtenObjetoProveedor = new CSelect();
            ObtenObjetoProveedor.StoredProcedure.CommandText = "sp_Cliente_Organizacion_Consulta";
            ObtenObjetoProveedor.StoredProcedure.Parameters.AddWithValue("@Opcion", 3);
            ObtenObjetoProveedor.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(pRFC));
            ObtenObjetoProveedor.Llena<CProveedorSucursal>(typeof(CProveedorSucursal), pConexion);
            foreach (CProveedorSucursal ProveedorSucursal in ObtenObjetoProveedor.ListaRegistros)
            {
                if (ProveedorSucursal.IdSucursal == pIdSucursalActual)
                {
                    ExisteCliente = 3; //Existe como proveedor
                    break;
                }
            }
        }

        return ExisteCliente;
    }

    public int ExisteClienteEditar(String pRFC, int pIdCliente, int pIdSucursalActual, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteCliente = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Cliente_Organizacion_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(pRFC));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCliente", Convert.ToInt32(pIdCliente));
        ObtenObjeto.Llena<CClienteSucursal>(typeof(CClienteSucursal), pConexion);
        foreach (CClienteSucursal ClienteSucursal in ObtenObjeto.ListaRegistros)
        {
            if (ClienteSucursal.IdSucursal == pIdSucursalActual)
            {
                ExisteCliente = 1;
                break;
            }
            else
            {
                ExisteCliente = 2;
            }

        }
        return ExisteCliente;
    }

    public int RevisaExisteCliente(int pIdOrganizacion, int pIdSucursalActual, CConexion pConexion)
    {

        CSelect ObtenObjeto = new CSelect();
        int Existe = 0; //No existe ni en cliente ni en proveedor
        ObtenObjeto.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 5);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", Convert.ToInt32(pIdOrganizacion));
        ObtenObjeto.Llena<CClienteSucursal>(typeof(CClienteSucursal), pConexion);
        foreach (CClienteSucursal ClienteSucursal in ObtenObjeto.ListaRegistros)
        {
            if (ClienteSucursal.IdSucursal == pIdSucursalActual)
            {
                Existe = 1; //Existe en mi sucursal
                break;
            }
            else
            {
                Existe = 2; //Existe en otra sucursal
            }
        }

        if (Existe == 0) //No existe en Cliente busco en Proveedor
        {
            CSelect ObtenObjetoProveedor = new CSelect();
            ObtenObjetoProveedor.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
            ObtenObjetoProveedor.StoredProcedure.Parameters.AddWithValue("@Opcion", 6);
            ObtenObjetoProveedor.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", Convert.ToInt32(pIdOrganizacion));
            ObtenObjetoProveedor.Llena<CProveedorSucursal>(typeof(CProveedorSucursal), pConexion);
            foreach (CProveedorSucursal ProveedorSucursal in ObtenObjetoProveedor.ListaRegistros)
            {
                if (ProveedorSucursal.IdSucursal > 0)
                {
                    Existe = 3; //Existe como proveedor
                    break;
                }
            }
        }

        return Existe;
    }

    public string EjecutarStore(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public static int ClientesNuevos(string pFechaInicio, string pFechaFin, string pUsuario, int pVerTodos, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Oportunidad_ConsultarClientesNuevos";
        Select.StoredProcedure.Parameters.AddWithValue("pFechaInicio", pFechaInicio);
        Select.StoredProcedure.Parameters.AddWithValue("pFechaFin", pFechaFin);
        Select.StoredProcedure.Parameters.AddWithValue("pUsuario", pUsuario);
        Select.StoredProcedure.Parameters.AddWithValue("pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        Select.StoredProcedure.Parameters.AddWithValue("pVerTodos", pVerTodos);
        Select.Llena(pConexion);

        int nuevosClientes = 0;
        while (Select.Registros.Read())
        {
            nuevosClientes = Convert.ToInt32(Select.Registros["ClientesNuevos"].ToString());
        }
        Select.CerrarConsulta();
        return nuevosClientes;
    }

    public static int ClienteOrganizacionSucursal(int IdOrganizacion, int IdSucursal, CConexion pConexion)
    {
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Cliente_ObtenerClienteOrganizacionSucursal";
        Select.StoredProcedure.Parameters.AddWithValue("IdOrganizacion", IdOrganizacion);
        Select.StoredProcedure.Parameters.AddWithValue("IdSucursal", IdSucursal);
        Select.Llena(pConexion);

        int IdCliente = 0;
        if (Select.Registros.Read()) {
            IdCliente = Convert.ToInt32(Select.Registros["IdCliente"]);
        }

        return IdCliente;
    }

	public static decimal SaldoCliente (int IdCliente, CConexion pConexion)
	{
		decimal Saldo = 0;

		CSelectEspecifico Consulta = new CSelectEspecifico();
		Consulta.StoredProcedure.CommandText = "sp_ClienteSaldoVencido";
		Consulta.StoredProcedure.Parameters.Add("IdCliente", SqlDbType.Int).Value = IdCliente;

		Consulta.Llena(pConexion);

		if (Consulta.Registros.Read())
		{
			Saldo = Convert.ToDecimal(Consulta.Registros["Saldo"]);
		}

		return Saldo;
	}

}