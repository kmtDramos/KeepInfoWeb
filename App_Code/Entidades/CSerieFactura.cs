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


public partial class CSerieFactura
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Constructores

    //Metodos Especiales
    public bool RevisarExisteRegistro(Dictionary<string, object> pParametros, CConexion pConexion)
    {
        bool existeSerieFactura = false;
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_SerieFactura_Consultar_ExisteSerieFactura";
        Select.StoredProcedure.Parameters.AddWithValue("@pSerieFactura", pParametros["SerieFactura"]);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pParametros["IdSucursal"]);
        Select.Llena(pConexion);
        if (Select.Registros.Read())
        {
            if (Convert.ToInt32(Select.Registros["NoSerieFactura"]) > 0)
            {
                existeSerieFactura = true;
            }
        }
        Select.Registros.Close();
        Select.Registros.Dispose();
        return existeSerieFactura;
    }

    public static JObject ObtenerJsonSerieFactura(JObject pModelo, int pIdSerieFactura, CConexion pConexion)
    {
        CSerieFactura SerieFactura = new CSerieFactura();
        SerieFactura.LlenaObjeto(pIdSerieFactura, pConexion);
        pModelo.Add("IdSerieFactura", SerieFactura.IdSerieFactura);
        pModelo.Add("SerieFactura", SerieFactura.SerieFactura);
        pModelo.Add("Timbrado", SerieFactura.Timbrado);
        pModelo.Add("IdSucursal", SerieFactura.IdSucursal);
        pModelo.Add("EsParcialidad", SerieFactura.esParcialidad);
        pModelo.Add("EsVenta", SerieFactura.EsVenta);
        return pModelo;
    }

    public void AgregarActualizarParcialidadSucursal(int pIdSerieFactura, int pIdSucursal, CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_SerieFactura_EditarParcialidadAgregar";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", pIdSerieFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        Editar.Update(pConexion);
    }

    public void ActualizarParcialidadSucursal(int pIdSerieFactura, int pIdSucursal, int pEsParcialidad, CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_SerieFactura_EditarParcialidad";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSerieFactura", pIdSerieFactura);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pEsParcialidad", pEsParcialidad);
        Editar.Update(pConexion);
    }

    public string ObtenerJsonSerieFacturaConsulta(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

}
