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


public partial class CProveedor
{
    //Constructores

    //Metodos Especiales
    public int ExisteProveedor(String pRFC, int pIdSucursalActual, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteProveedor = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Organizacion_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(pRFC));
        ObtenObjeto.Llena<CProveedorSucursal>(typeof(CProveedorSucursal), pConexion);
        foreach (CProveedorSucursal ProveedorSucursal in ObtenObjeto.ListaRegistros)
        {
            if (ProveedorSucursal.IdSucursal == pIdSucursalActual)
            {
                ExisteProveedor = 1;
                break;
            }
            else
            {
                ExisteProveedor = 2;
            }
        }

        if (ExisteProveedor == 0) //No existe en Proveedor busco en Cliente
        {
            CSelect ObtenObjetoCliente = new CSelect();
            ObtenObjetoCliente.StoredProcedure.CommandText = "sp_Proveedor_Organizacion_ExisteProveedorCliente";
            ObtenObjetoCliente.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(pRFC));
            ObtenObjetoCliente.Llena<CClienteSucursal>(typeof(CClienteSucursal), pConexion);
            foreach (CClienteSucursal ClienteSucursal in ObtenObjetoCliente.ListaRegistros)
            {
                if (ClienteSucursal.IdSucursal == pIdSucursalActual)
                {
                    ExisteProveedor = 3; //Existe como proveedor
                    break;
                }
            }
        }
        return ExisteProveedor;
    }

    public int ExisteProveedorEditar(String pRFC, int pIdProveedor, int pIdSucursalActual, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteProveedor = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Organizacion_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(pRFC));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", Convert.ToInt32(pIdProveedor));
        ObtenObjeto.Llena<CProveedorSucursal>(typeof(CProveedorSucursal), pConexion);
        foreach (CProveedorSucursal ProveedorSucursal in ObtenObjeto.ListaRegistros)
        {
            if (ProveedorSucursal.IdSucursal == pIdSucursalActual)
            {
                ExisteProveedor = 1;
                break;
            }
            else
            {
                ExisteProveedor = 2;
            }

        }
        return ExisteProveedor;
    }

    public int RevisaExisteProveedor(int pIdOrganizacion, int pIdSucursalActual, CConexion pConexion)
    {

        CSelect ObtenObjeto = new CSelect();
        int Existe = 0; //No existe ni en cliente ni en proveedor
        ObtenObjeto.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 6);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", Convert.ToInt32(pIdOrganizacion));
        ObtenObjeto.Llena<CProveedorSucursal>(typeof(CProveedorSucursal), pConexion);
        foreach (CProveedorSucursal ProveedorSucursal in ObtenObjeto.ListaRegistros)
        {
            if (ProveedorSucursal.IdSucursal == pIdSucursalActual)
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
            CSelect ObtenObjetoCliente = new CSelect();
            ObtenObjetoCliente.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
            ObtenObjetoCliente.StoredProcedure.Parameters.AddWithValue("@Opcion", 5);
            ObtenObjetoCliente.StoredProcedure.Parameters.AddWithValue("@pIdOrganizacion", Convert.ToInt32(pIdOrganizacion));
            ObtenObjetoCliente.Llena<CClienteSucursal>(typeof(CClienteSucursal), pConexion);
            foreach (CClienteSucursal ClienteSucursal in ObtenObjetoCliente.ListaRegistros)
            {
                if (ClienteSucursal.IdSucursal > 0)
                {
                    Existe = 3; //Existe como cliente
                    break;
                }
            }
        }

        return Existe;
    }
}
