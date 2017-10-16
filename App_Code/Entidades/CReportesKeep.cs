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
using Newtonsoft.Json.Linq;


public partial class CReportesKeep
{
    //Constructores

    //Metodos Especiales
    public string ObtenerJsonConsultas(CConexion pConexion)
    {
        CSelect ObtenPagina = new CSelect();
        ObtenPagina.StoredProcedure.CommandText = "sp_ReportesKeep_Consulta";
        ObtenPagina.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        ObtenPagina.Llena<CPagina>(typeof(CPagina), pConexion);

        JObject oRespuesta = new JObject();
        oRespuesta.Add(new JProperty("Error", 0));
        JObject Modelo = new JObject();
        Modelo.Add(new JProperty("id", 0));
        JArray JItem = new JArray();
        foreach (CPagina OPagina in ObtenPagina.ListaRegistros)
        {
            JObject JPagina = new JObject();
            JPagina.Add(new JProperty("id", OPagina.IdPagina.ToString()));
            JPagina.Add(new JProperty("text", OPagina.Pagina));
            JPagina.Add(new JProperty("im0", "html.png"));
            JPagina.Add(new JProperty("im1", "html.png"));
            JPagina.Add(new JProperty("im2", "html.png"));
            JItem.Add(JPagina);
        }
        JObject oPagina = new JObject();
        oPagina.Add(new JProperty("id", "Pagina"));
        oPagina.Add(new JProperty("text", "Páginas"));
        oPagina.Add(new JProperty("open", "1"));
        oPagina.Add(new JProperty("select", "1"));
        oPagina.Add(new JProperty("item", JItem));

        JItem = new JArray();
        JItem.Add(oPagina);
        Modelo.Add(new JProperty("item", JItem));

        oRespuesta.Add(new JProperty("Modelo", Modelo));
        return oRespuesta.ToString();
    }

    //public static JToken obtenerDatosImpresionEstadoCuentaCliente(string pIdCliente, int pUsuario, string pFechaInicial, string pFechaFinal, int pIdSucursal)
    //{
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //    CCotizacion jsonImpresionC = new CCotizacion();
    //    jsonImpresionC.StoredProcedure.CommandText = "SP_Impresion_EstadoCuentaCliente";
    //    jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
    //    jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pIdCliente", pIdCliente);
    //    jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
    //    jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
    //    jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
    //    jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
    //    return jsonImpresionC.ObtenerJsonJObject(ConexionBaseDatos);
    //}

    public static JObject obtenerDatosImpresionEstadoCuentaCliente(string pIdCliente, int pUsuario, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdTipoCambio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        SqlCommand jsonImpresionC = new SqlCommand();
        jsonImpresionC.CommandText = "SP_Impresion_EstadoCuentaCliente";
        jsonImpresionC.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.Parameters.AddWithValue("@pIdCliente", pIdCliente);
        jsonImpresionC.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        jsonImpresionC.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
        jsonImpresionC.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonImpresionC.Parameters.AddWithValue("@pIdTipoCambio", pIdTipoCambio);
        return ObtenerJsonJObject(jsonImpresionC, ConexionBaseDatos);
    }

    public static JObject obtenerDatosImpresionEstadoCuentaClientePesosDolares(string pIdCliente, int pUsuario, string pFechaInicial, int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        SqlCommand jsonImpresionC = new SqlCommand();
        jsonImpresionC.CommandText = "SP_Impresion_EstadoCuentaClientePesosDolares";
        jsonImpresionC.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.Parameters.AddWithValue("@pIdCliente", pIdCliente);
        jsonImpresionC.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        //jsonImpresionC.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
        jsonImpresionC.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        return ObtenerJsonJObject(jsonImpresionC, ConexionBaseDatos);
    }

    public static JObject obtenerDatosImpresionEstadoCuentaProveedor(string pIdProveedor, int pUsuario, string pFechaInicial, int pIdSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        SqlCommand jsonImpresionC = new SqlCommand();
        jsonImpresionC.CommandText = "SP_Impresion_EstadoCuentaProveedor";
        jsonImpresionC.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.Parameters.AddWithValue("@pIdProveedor", pIdProveedor);
        jsonImpresionC.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        jsonImpresionC.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        return ObtenerJsonJObject(jsonImpresionC, ConexionBaseDatos);
    }

    public static JObject obtenerDatosImpresionEstadoCuentaBancaria(string pIdCuentaBancaria, int pUsuario, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdTipoCuenta, string pFechaIni, string pFechaF, string pSucursal)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        SqlCommand jsonImpresionC = new SqlCommand();
        jsonImpresionC.CommandText = "SP_Impresion_CuentasBancarias";
        jsonImpresionC.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.Parameters.AddWithValue("@pIdCuentaBancaria", pIdCuentaBancaria);
        jsonImpresionC.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        jsonImpresionC.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
        jsonImpresionC.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonImpresionC.Parameters.AddWithValue("@pIdTipoCuenta", pIdTipoCuenta);
        jsonImpresionC.Parameters.AddWithValue("@pSucursal", pSucursal);
        jsonImpresionC.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
        jsonImpresionC.Parameters.AddWithValue("@pFormatoFechaFin", pFechaF);

        return ObtenerJsonJObject(jsonImpresionC, ConexionBaseDatos);
    }

    public static JObject ObtenerJsonJObject(SqlCommand pStoredProcedure, CConexion pConexion)
    {
        pStoredProcedure.CommandType = CommandType.StoredProcedure;
        pStoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(pStoredProcedure);
        dataAdapter.Fill(dataSet);

        JObject oJson = JObject.Parse(JsonConvert.SerializeObject(dataSet));
        return oJson;
    }


    public static JArray ObtenerDepartamentos(CConexion pConexion)
    {

        CSelectEspecifico Departamentos = new CSelectEspecifico();
        Departamentos.StoredProcedure.CommandText = "sp_ReportesKeep_ConsultarDepartamentos";
        Departamentos.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Departamentos.Llena(pConexion);

        JArray JDepartamentos = new JArray();
        while (Departamentos.Registros.Read())
        {
            JObject JDepartamento = new JObject();
            JDepartamento.Add("Departamento", Convert.ToInt32(Departamentos.Registros["Carpeta"]));
            JDepartamentos.Add(JDepartamento);
        }
        Departamentos.Registros.Close();
        return JDepartamentos;
    }

    public static JObject obtenerDatosImpresionInventarioActualAlmacenProducto(int pUsuario, string pFechaInicial, string pFechaFinal, int pIdAlmacen)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        SqlCommand jsonImpresionC = new SqlCommand();
        jsonImpresionC.CommandText = "SP_Impresion_InventarioActualAlmacenProducto";
        jsonImpresionC.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
        jsonImpresionC.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        jsonImpresionC.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);

        return ObtenerJsonJObject(jsonImpresionC, ConexionBaseDatos);
    }

    public static JObject obtenerDatosImpresionInventarioActualSucursalProducto(int pUsuario, string pFechaInicial, string pFechaFinal, int pIdSucursal, int pIdProducto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        SqlCommand jsonImpresionC = new SqlCommand();
        jsonImpresionC.CommandText = "SSP_Impresion_InventarioActualSucursalProducto";
        jsonImpresionC.Parameters.AddWithValue("@pIdSucursal", pIdSucursal);
        jsonImpresionC.Parameters.AddWithValue("@pIdProducto", pIdProducto);
        jsonImpresionC.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        jsonImpresionC.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);

        return ObtenerJsonJObject(jsonImpresionC, ConexionBaseDatos);
    }

    public static JObject obtenerDatosImpresionInventarioActualGlobalUnidades(int pUsuario, string pFechaInicial, string pFechaFinal, int pIdAlmacen, string pFechaIni, string pFechaF)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        SqlCommand jsonImpresionC = new SqlCommand();
        jsonImpresionC.CommandText = "SP_Impresion_InventarioGlobalUnidades";
        jsonImpresionC.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
        jsonImpresionC.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        jsonImpresionC.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        jsonImpresionC.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
        jsonImpresionC.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
        jsonImpresionC.Parameters.AddWithValue("@pFormatoFechaFin", pFechaF);
        return ObtenerJsonJObject(jsonImpresionC, ConexionBaseDatos);
    }

    public static JObject obtenerDatosImpresionInventarioActualGlobalUnidades(int pUsuario, string pFechaInicial, string pFechaFinal, int pIdAlmacen, string pFechaIni, string pFechaF, CConexion pConexion)
    {
        CSelectEspecifico SelectInventarioActualGlobalUnidades = new CSelectEspecifico();
        SelectInventarioActualGlobalUnidades.StoredProcedure.CommandText = "SP_Impresion_InventarioGlobalUnidades";
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaFin", pFechaF);
        SelectInventarioActualGlobalUnidades.Llena(pConexion);

        JObject JDocumento = new JObject();
        while (SelectInventarioActualGlobalUnidades.Registros.Read())
        {
            JDocumento.Add("RFC", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["RFC"]));
            JDocumento.Add("RazonSocialReceptor", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["RazonSocialReceptor"]));
            JDocumento.Add("Fecha", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Fecha"]));
            JDocumento.Add("FechaInicial", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["FechaInicial"]));
            JDocumento.Add("FechaFinal", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["FechaFinal"]));
            JDocumento.Add("TipoMoneda", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["TipoMoneda"]));
            JDocumento.Add("Reporte", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Reporte"]));
            JDocumento.Add("Almacen", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Almacen"]));
            JDocumento.Add("TotalInventarioInicial", Convert.ToDecimal(SelectInventarioActualGlobalUnidades.Registros["TotalInventarioInicial"]));
            JDocumento.Add("TotalEntradas", Convert.ToDecimal(SelectInventarioActualGlobalUnidades.Registros["TotalEntradas"]));
            JDocumento.Add("TotalSalidas", Convert.ToDecimal(SelectInventarioActualGlobalUnidades.Registros["TotalSalidas"]));
            JDocumento.Add("TotalExistencias", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["TotalExistencias"]));
        }

        JArray JAInventarios = new JArray();
        JObject JInventario = new JObject();
        if (SelectInventarioActualGlobalUnidades.Registros.NextResult())
        {
            string almacen = "";
            JArray JAExistencias = new JArray();
            while (SelectInventarioActualGlobalUnidades.Registros.Read())
            {
                if (almacen == "")
                {
                    almacen = Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Costeo"]));
                    JExistencia.Add("UnidadCompraVenta", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Salidas"]));
                    JExistencia.Add("Existencia", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Existencia"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Rotacion"]));
                    JAExistencias.Add(JExistencia);
                }
                else if (almacen == Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]))
                {
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Costeo"]));
                    JExistencia.Add("UnidadCompraVenta", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Salidas"]));
                    JExistencia.Add("Existencia", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Existencia"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Rotacion"]));
                    JAExistencias.Add(JExistencia);
                }
                else
                {
                    JInventario = new JObject();
                    JInventario.Add("Almacen", almacen);
                    JInventario.Add("Lista", JAExistencias);
                    JAInventarios.Add(JInventario);
                    JAExistencias = new JArray();

                    almacen = Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Costeo"]));
                    JExistencia.Add("UnidadCompraVenta", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Salidas"]));
                    JExistencia.Add("Existencia", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Existencia"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Rotacion"]));
                    JAExistencias.Add(JExistencia);
                }
            }
            JInventario = new JObject();
            JInventario.Add("Almacen", almacen);
            JInventario.Add("Lista", JAExistencias);
            JAInventarios.Add(JInventario);
        }

        JObject JDocumentoInventario = new JObject();
        JDocumentoInventario.Add("Tipo", "Conceptos");
        JDocumentoInventario.Add("NombreTabla", "tblInventarios");
        JDocumentoInventario.Add("Inventarios", JAInventarios);
        JDocumento.Add("Inventarios", JDocumentoInventario);
        SelectInventarioActualGlobalUnidades.CerrarConsulta();
        return JDocumento;
    }

    public static JObject obtenerDatosImpresionInventarioExistenciaActual(int pUsuario, string pFechaInicial, int pIdAlmacen, string pFechaIni, CConexion pConexion)
    {
        CSelectEspecifico SelectInventarioActualGlobalUnidades = new CSelectEspecifico();
        SelectInventarioActualGlobalUnidades.StoredProcedure.CommandText = "SP_Impresion_InventarioGlobalUnidades";
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        SelectInventarioActualGlobalUnidades.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
        SelectInventarioActualGlobalUnidades.Llena(pConexion);

        JObject JDocumento = new JObject();
        while (SelectInventarioActualGlobalUnidades.Registros.Read())
        {
            JDocumento.Add("RFC", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["RFC"]));
            JDocumento.Add("RazonSocialReceptor", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["RazonSocialReceptor"]));
            JDocumento.Add("Fecha", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Fecha"]));
            JDocumento.Add("FechaInicial", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["FechaInicial"]));
            JDocumento.Add("FechaFinal", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["FechaFinal"]));
            JDocumento.Add("TipoMoneda", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["TipoMoneda"]));
            JDocumento.Add("Reporte", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Reporte"]));
            JDocumento.Add("Almacen", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Almacen"]));
            JDocumento.Add("TotalInventarioInicial", Convert.ToDecimal(SelectInventarioActualGlobalUnidades.Registros["TotalInventarioInicial"]));
            JDocumento.Add("TotalEntradas", Convert.ToDecimal(SelectInventarioActualGlobalUnidades.Registros["TotalEntradas"]));
            JDocumento.Add("TotalSalidas", Convert.ToDecimal(SelectInventarioActualGlobalUnidades.Registros["TotalSalidas"]));
            JDocumento.Add("TotalExistencias", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["TotalExistencias"]));
        }

        JArray JAInventarios = new JArray();
        JObject JInventario = new JObject();
        if (SelectInventarioActualGlobalUnidades.Registros.NextResult())
        {
            string almacen = "";
            JArray JAExistencias = new JArray();
            while (SelectInventarioActualGlobalUnidades.Registros.Read())
            {
                if (almacen == "")
                {
                    almacen = Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Producto"]));
                    JExistencia.Add("UnidadCompraVenta", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                    JExistencia.Add("Existencia", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Existencia"]));
                    JAExistencias.Add(JExistencia);
                }
                else if (almacen == Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]))
                {
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Producto"]));
                    JExistencia.Add("UnidadCompraVenta", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                    JExistencia.Add("Existencia", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Existencia"]));
                    JAExistencias.Add(JExistencia);
                }
                else
                {
                    JInventario = new JObject();
                    JInventario.Add("Almacen", almacen);
                    JInventario.Add("Lista", JAExistencias);
                    JAInventarios.Add(JInventario);
                    JAExistencias = new JArray();

                    almacen = Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Producto"]));
                    JExistencia.Add("UnidadCompraVenta", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["UnidadCompraVenta"]));
                    JExistencia.Add("Existencia", Convert.ToString(SelectInventarioActualGlobalUnidades.Registros["Existencia"]));
                    JAExistencias.Add(JExistencia);
                }
            }
            JInventario = new JObject();
            JInventario.Add("Almacen", almacen);
            JInventario.Add("Lista", JAExistencias);
            JAInventarios.Add(JInventario);
        }

        JObject JDocumentoInventario = new JObject();
        JDocumentoInventario.Add("Tipo", "Conceptos");
        JDocumentoInventario.Add("NombreTabla", "tblInventarios");
        JDocumentoInventario.Add("Inventarios", JAInventarios);
        JDocumento.Add("Inventarios", JDocumentoInventario);
        SelectInventarioActualGlobalUnidades.CerrarConsulta();
        return JDocumento;
    }

    public static JObject obtenerDatosImpresionInventarioImportesGlobal(int pUsuario, string pFechaInicial, string pFechaFinal, int pIdAlmacen, string pFechaIni, string pFechaF, CConexion pConexion)
    {
        CSelectEspecifico SelectInventarioImportesGlobal = new CSelectEspecifico();
        SelectInventarioImportesGlobal.StoredProcedure.CommandText = "SP_Impresion_InventarioImportesGlobal";
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaFin", pFechaF);
        SelectInventarioImportesGlobal.Llena(pConexion);

        JObject JDocumento = new JObject();
        while (SelectInventarioImportesGlobal.Registros.Read())
        {
            JDocumento.Add("RFC", Convert.ToString(SelectInventarioImportesGlobal.Registros["RFC"]));
            JDocumento.Add("RazonSocialReceptor", Convert.ToString(SelectInventarioImportesGlobal.Registros["RazonSocialReceptor"]));
            JDocumento.Add("Fecha", Convert.ToString(SelectInventarioImportesGlobal.Registros["Fecha"]));
            JDocumento.Add("FechaInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["FechaInicial"]));
            JDocumento.Add("FechaFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["FechaFinal"]));
            JDocumento.Add("TipoMoneda", Convert.ToString(SelectInventarioImportesGlobal.Registros["TipoMoneda"]));
            JDocumento.Add("Reporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["Reporte"]));
            JDocumento.Add("Almacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["Almacen"]));
            JDocumento.Add("TotalInventarioInicial", Convert.ToDecimal(SelectInventarioImportesGlobal.Registros["TotalInventarioInicial"]));
            JDocumento.Add("TotalEntradas", Convert.ToDecimal(SelectInventarioImportesGlobal.Registros["TotalEntradas"]));
            JDocumento.Add("TotalSalidas", Convert.ToDecimal(SelectInventarioImportesGlobal.Registros["TotalSalidas"]));
            JDocumento.Add("TotalInventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInventarioFinal"]));
            JDocumento.Add("TotalInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInicialImporte"]));
            JDocumento.Add("TotalFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalFinalImporte"]));
        }

        JArray JAInventarios = new JArray();
        JObject JInventario = new JObject();
        if (SelectInventarioImportesGlobal.Registros.NextResult())
        {
            string almacen = "";
            JArray JAExistencias = new JArray();
            while (SelectInventarioImportesGlobal.Registros.Read())
            {
                if (almacen == "")
                {
                    almacen = Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioImportesGlobal.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioImportesGlobal.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioImportesGlobal.Registros["Costeo"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Rotacion"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Salidas"]));
                    JExistencia.Add("InventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinal"]));
                    JExistencia.Add("InventarioInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicialImporte"]));
                    JExistencia.Add("InventarioFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinalImporte"]));
                    JAExistencias.Add(JExistencia);
                }
                else if (almacen == Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]))
                {
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioImportesGlobal.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioImportesGlobal.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioImportesGlobal.Registros["Costeo"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Rotacion"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Salidas"]));
                    JExistencia.Add("InventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinal"]));
                    JExistencia.Add("InventarioInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicialImporte"]));
                    JExistencia.Add("InventarioFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinalImporte"]));
                    JAExistencias.Add(JExistencia);
                }
                else
                {
                    JInventario = new JObject();
                    JInventario.Add("Almacen", almacen);
                    JInventario.Add("Lista", JAExistencias);
                    JAInventarios.Add(JInventario);
                    JAExistencias = new JArray();

                    almacen = Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioImportesGlobal.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioImportesGlobal.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioImportesGlobal.Registros["Costeo"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Rotacion"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Salidas"]));
                    JExistencia.Add("InventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinal"]));
                    JExistencia.Add("InventarioInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicialImporte"]));
                    JExistencia.Add("InventarioFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinalImporte"]));
                    JAExistencias.Add(JExistencia);
                }
            }
            JInventario = new JObject();
            JInventario.Add("Almacen", almacen);
            JInventario.Add("Lista", JAExistencias);
            JAInventarios.Add(JInventario);
        }

        if (SelectInventarioImportesGlobal.Registros.NextResult())
        {

            while (SelectInventarioImportesGlobal.Registros.Read())
            {
                foreach (JObject JOInventario in JAInventarios)
                {
                    if (Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]) == JOInventario["Almacen"].ToString())
                    {
                        JObject JTotalAlmacen = new JObject();
                        JTotalAlmacen.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                        JTotalAlmacen.Add("TotalInicialAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInicialAlmacen"]));
                        JTotalAlmacen.Add("TotalEntradasAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalEntradasAlmacen"]));
                        JTotalAlmacen.Add("TotalSalidasAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalSalidasAlmacen"]));
                        JTotalAlmacen.Add("TotalFinalAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalFinalAlmacen"]));
                        JTotalAlmacen.Add("TotalInicialImporteAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInicialImporteAlmacen"]));
                        JTotalAlmacen.Add("TotalFinalImporteAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalFinalImporteAlmacen"]));
                        JOInventario.Add("Total", JTotalAlmacen);
                    }
                }
            }
        }

        JObject JDocumentoInventario = new JObject();
        JDocumentoInventario.Add("Tipo", "Conceptos");
        JDocumentoInventario.Add("NombreTabla", "tblInventarios");
        JDocumentoInventario.Add("Inventarios", JAInventarios);

        JDocumento.Add("Inventarios", JDocumentoInventario);
        SelectInventarioImportesGlobal.CerrarConsulta();
        return JDocumento;
    }

    public static JObject obtenerDatosImpresionInventarioImportesGlobalCD(int pUsuario, string pFechaInicial, string pFechaFinal, int pIdAlmacen, string pFechaIni, string pFechaF, CConexion pConexion)
    {
        CSelectEspecifico SelectInventarioImportesGlobal = new CSelectEspecifico();
        SelectInventarioImportesGlobal.StoredProcedure.CommandText = "SP_Impresion_InventarioImportesGlobalCD";
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pIdAlmacen", pIdAlmacen);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", pFechaInicial);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", pFechaFinal);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaIni", pFechaIni);
        SelectInventarioImportesGlobal.StoredProcedure.Parameters.AddWithValue("@pFormatoFechaFin", pFechaF);
        SelectInventarioImportesGlobal.Llena(pConexion);

        JObject JDocumento = new JObject();
        while (SelectInventarioImportesGlobal.Registros.Read())
        {
            JDocumento.Add("RFC", Convert.ToString(SelectInventarioImportesGlobal.Registros["RFC"]));
            JDocumento.Add("RazonSocialReceptor", Convert.ToString(SelectInventarioImportesGlobal.Registros["RazonSocialReceptor"]));
            JDocumento.Add("Fecha", Convert.ToString(SelectInventarioImportesGlobal.Registros["Fecha"]));
            JDocumento.Add("FechaInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["FechaInicial"]));
            JDocumento.Add("FechaFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["FechaFinal"]));
            JDocumento.Add("TipoMoneda", Convert.ToString(SelectInventarioImportesGlobal.Registros["TipoMoneda"]));
            JDocumento.Add("Reporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["Reporte"]));
            JDocumento.Add("Almacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["Almacen"]));
            JDocumento.Add("TotalInventarioInicial", Convert.ToDecimal(SelectInventarioImportesGlobal.Registros["TotalInventarioInicial"]));
            JDocumento.Add("TotalEntradas", Convert.ToDecimal(SelectInventarioImportesGlobal.Registros["TotalEntradas"]));
            JDocumento.Add("TotalSalidas", Convert.ToDecimal(SelectInventarioImportesGlobal.Registros["TotalSalidas"]));
            JDocumento.Add("TotalInventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInventarioFinal"]));
            JDocumento.Add("TotalInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInicialImporte"]));
            JDocumento.Add("TotalFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalFinalImporte"]));
        }

        JArray JAInventarios = new JArray();
        JObject JInventario = new JObject();
        if (SelectInventarioImportesGlobal.Registros.NextResult())
        {
            string almacen = "";
            JArray JAExistencias = new JArray();
            while (SelectInventarioImportesGlobal.Registros.Read())
            {
                if (almacen == "")
                {
                    almacen = Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioImportesGlobal.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioImportesGlobal.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioImportesGlobal.Registros["Costeo"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Rotacion"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Salidas"]));
                    JExistencia.Add("InventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinal"]));
                    JExistencia.Add("InventarioInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicialImporte"]));
                    JExistencia.Add("InventarioFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinalImporte"]));
                    JAExistencias.Add(JExistencia);
                }
                else if (almacen == Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]))
                {
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioImportesGlobal.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioImportesGlobal.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioImportesGlobal.Registros["Costeo"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Rotacion"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Salidas"]));
                    JExistencia.Add("InventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinal"]));
                    JExistencia.Add("InventarioInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicialImporte"]));
                    JExistencia.Add("InventarioFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinalImporte"]));
                    JAExistencias.Add(JExistencia);
                }
                else
                {
                    JInventario = new JObject();
                    JInventario.Add("Almacen", almacen);
                    JInventario.Add("Lista", JAExistencias);
                    JAInventarios.Add(JInventario);
                    JAExistencias = new JArray();

                    almacen = Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]);
                    JObject JExistencia = new JObject();
                    JExistencia.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                    JExistencia.Add("Clave", Convert.ToString(SelectInventarioImportesGlobal.Registros["Clave"]));
                    JExistencia.Add("Producto", Convert.ToString(SelectInventarioImportesGlobal.Registros["Producto"]));
                    JExistencia.Add("Costeo", Convert.ToString(SelectInventarioImportesGlobal.Registros["Costeo"]));
                    JExistencia.Add("Rotacion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Rotacion"]));
                    JExistencia.Add("InventarioInicial", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicial"]));
                    JExistencia.Add("Entradas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Entradas"]));
                    JExistencia.Add("Salidas", Convert.ToString(SelectInventarioImportesGlobal.Registros["Salidas"]));
                    JExistencia.Add("InventarioFinal", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinal"]));
                    JExistencia.Add("InventarioInicialImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioInicialImporte"]));
                    JExistencia.Add("InventarioFinalImporte", Convert.ToString(SelectInventarioImportesGlobal.Registros["InventarioFinalImporte"]));
                    JAExistencias.Add(JExistencia);
                }
            }
            JInventario = new JObject();
            JInventario.Add("Almacen", almacen);
            JInventario.Add("Lista", JAExistencias);
            JAInventarios.Add(JInventario);
        }

        if (SelectInventarioImportesGlobal.Registros.NextResult())
        {

            while (SelectInventarioImportesGlobal.Registros.Read())
            {
                foreach (JObject JOInventario in JAInventarios)
                {
                    if (Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]) == JOInventario["Almacen"].ToString())
                    {
                        JObject JTotalAlmacen = new JObject();
                        JTotalAlmacen.Add("Descripcion", Convert.ToString(SelectInventarioImportesGlobal.Registros["Descripcion"]));
                        JTotalAlmacen.Add("TotalInicialAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInicialAlmacen"]));
                        JTotalAlmacen.Add("TotalEntradasAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalEntradasAlmacen"]));
                        JTotalAlmacen.Add("TotalSalidasAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalSalidasAlmacen"]));
                        JTotalAlmacen.Add("TotalFinalAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalFinalAlmacen"]));
                        JTotalAlmacen.Add("TotalInicialImporteAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalInicialImporteAlmacen"]));
                        JTotalAlmacen.Add("TotalFinalImporteAlmacen", Convert.ToString(SelectInventarioImportesGlobal.Registros["TotalFinalImporteAlmacen"]));
                        JOInventario.Add("Total", JTotalAlmacen);
                    }
                }
            }
        }

        JObject JDocumentoInventario = new JObject();
        JDocumentoInventario.Add("Tipo", "Conceptos");
        JDocumentoInventario.Add("NombreTabla", "tblInventarios");
        JDocumentoInventario.Add("Inventarios", JAInventarios);

        JDocumento.Add("Inventarios", JDocumentoInventario);
        SelectInventarioImportesGlobal.CerrarConsulta();
        return JDocumento;
    }
}