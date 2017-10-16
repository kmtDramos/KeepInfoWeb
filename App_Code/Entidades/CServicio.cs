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


public partial class CServicio
{
    //Constructores
    public SqlCommand StoredProcedure = new SqlCommand();
    //Metodos Especiales
    public int ExisteServicio(String pClave, int pIdSucursalActual, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteServicio = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Servicio_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pClave));
        ObtenObjeto.Llena<CServicio>(typeof(CServicio), pConexion);
        if (ObtenObjeto.ListaRegistros.Count > 0)
        {
            ExisteServicio = 1;
        }
        return ExisteServicio;
    }
    public int ExisteServicioEditar(String pClave, int pIdServicio, int pIdSucursalActual, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteServicio = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Servicio_Consulta";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pClave));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdServicio", Convert.ToInt32(pIdServicio));
        ObtenObjeto.Llena<CServicioEmpresa>(typeof(CServicioEmpresa), pConexion);
        foreach (CServicioEmpresa ServicioEmpresa in ObtenObjeto.ListaRegistros)
        {
            ExisteServicio = 1;
        }
        return ExisteServicio;
    }

    public int ValidaServicioSucursal(int pIdUsuario, int pIdSucursalActual, CConexion pConexion)
    {

        CSelect ObtenObjeto = new CSelect();
        int ExisteServicio = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_SucursalAsignadaUsuario_Consultar_ObtenerSucursalesAsignadas";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pIdUsuario);
        ObtenObjeto.Llena<CSucursalAsignada>(typeof(CSucursalAsignada), pConexion);
        foreach (CSucursalAsignada SucursalAsignada in ObtenObjeto.ListaRegistros)
        {
            if (SucursalAsignada.IdSucursal == pIdSucursalActual)
            {
                ExisteServicio = 1;
                break;
            }
        }
        return ExisteServicio;
    }

    public static JObject ObtenerJsonServicio(JObject pModelo, int pIdServicio, CConexion pConexion)
    {
        CServicio Servicio = new CServicio();
        Servicio.LlenaObjeto(pIdServicio, pConexion);

        pModelo.Add("IdServicio", Servicio.IdServicio);
        pModelo.Add("Servicio", Servicio.Servicio);
        pModelo.Add("Clave", Servicio.Clave);
        pModelo.Add("IdTipoIVA", Servicio.IdTipoIVA);
        pModelo.Add("Precio", Servicio.Precio);

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Servicio.IdTipoMoneda, pConexion);
        pModelo.Add("IdTipoMoneda", TipoMoneda.IdTipoMoneda);
        pModelo.Add("TipoMoneda", TipoMoneda.TipoMoneda);

        CTipoVenta TipoVenta = new CTipoVenta();
        TipoVenta.LlenaObjeto(Servicio.IdTipoVenta, pConexion);
        pModelo.Add("IdTipoVenta", TipoVenta.IdTipoVenta);
        pModelo.Add("TipoVenta", TipoVenta.TipoVenta);

        CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
        UnidadCompraVenta.LlenaObjeto(Servicio.IdUnidadCompraVenta, pConexion);
        pModelo.Add("IdUnidadCompraVenta", UnidadCompraVenta.IdUnidadCompraVenta);
        pModelo.Add("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta);

        CTipoIVA TipoIVA = new CTipoIVA();
        TipoIVA.LlenaObjeto(Servicio.IdTipoIVA, pConexion);
        pModelo.Add("TipoIVA", TipoIVA.TipoIVA);

        if (Servicio.IdTipoIVA == 1)
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
            pModelo.Add("IVA", Sucursal.IVAActual);
        }
        else
        {
            pModelo.Add("IVA", 0);
        }

        return pModelo;
    }

    public string ObtenerJsonServicio(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }
}
