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


public partial class COrdenCompraEncabezado
{
    //Constructores
    public SqlCommand StoredProcedure = new SqlCommand();
    //Metodos Especiales

    public void AgregarOrdenCompraEncabezado(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_OrdenCompraEncabezado_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOrdenCompraEncabezado", 0);
        Agregar.StoredProcedure.Parameters["@pIdOrdenCompraEncabezado"].Direction = ParameterDirection.Output;
        if (fechaRequerida.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRequerida", fechaRequerida);
        }
        if (fechaRealEntrega.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRealEntrega", fechaRealEntrega);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSubtotal", subtotal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSaldo", saldo);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", idUsuario);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProveedor", idProveedor);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pDireccionEntrega", direccionEntrega);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pConsolidado", consolidado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSinPedido", sinPedido);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadTotalLetra", cantidadTotalLetra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdPedidoEncabezado", idPedidoEncabezado);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdProyecto", idProyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pClienteProyecto", clienteProyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pFolio", folio);
        if (fechaRecepcion.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaRecepcion", fechaRecepcion);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idOrdenCompraEncabezado = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdOrdenCompraEncabezado"].Value);
    }

    public static JObject ObtenerOrdenCompraEncabezado(JObject pModelo, int pIdOrdenCompraEncabezado, CConexion pConexion)
    {
        COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
        OrdenCompraEncabezado.LlenaObjeto(pIdOrdenCompraEncabezado, pConexion);
        pModelo.Add(new JProperty("IdOrdenCompraEncabezado", OrdenCompraEncabezado.IdOrdenCompraEncabezado));
        pModelo.Add(new JProperty("IdProveedor", OrdenCompraEncabezado.IdProveedor));
        pModelo.Add(new JProperty("IdCliente", OrdenCompraEncabezado.IdCliente));
        pModelo.Add(new JProperty("IdProyecto", OrdenCompraEncabezado.IdProyecto));
        pModelo.Add(new JProperty("Folio", OrdenCompraEncabezado.Folio));
        pModelo.Add(new JProperty("DireccionEntrega", OrdenCompraEncabezado.DireccionEntrega));
        pModelo.Add(new JProperty("FechaAlta", OrdenCompraEncabezado.FechaAlta.ToShortDateString()));
        pModelo.Add(new JProperty("FechaRealEntrega", OrdenCompraEncabezado.FechaRealEntrega.ToShortDateString()));
        pModelo.Add(new JProperty("FechaRequerida", OrdenCompraEncabezado.FechaRequerida.ToShortDateString()));
        pModelo.Add(new JProperty("IVA", OrdenCompraEncabezado.IVA));
        pModelo.Add(new JProperty("Nota", OrdenCompraEncabezado.Nota));
        pModelo.Add(new JProperty("Saldo", OrdenCompraEncabezado.Saldo));
        pModelo.Add(new JProperty("Subtotal", OrdenCompraEncabezado.Subtotal));
        pModelo.Add(new JProperty("Total", OrdenCompraEncabezado.Total));
        pModelo.Add(new JProperty("Consolidado", OrdenCompraEncabezado.Consolidado));
        pModelo.Add(new JProperty("SinPedido", OrdenCompraEncabezado.SinPedido));
        pModelo.Add(new JProperty("CantidadTotalLetra", OrdenCompraEncabezado.CantidadTotalLetra));
        pModelo.Add(new JProperty("Baja", OrdenCompraEncabezado.Baja));
        pModelo.Add(new JProperty("ClienteProyecto", OrdenCompraEncabezado.ClienteProyecto));

        CProveedor Proveedor = new CProveedor();
        Proveedor.LlenaObjeto(OrdenCompraEncabezado.IdProveedor, pConexion);

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(OrdenCompraEncabezado.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        CDivision Division = new CDivision();
        Division.LlenaObjeto(OrdenCompraEncabezado.IdDivision, pConexion);
        pModelo.Add(new JProperty("IdDivision", Division.IdDivision));
        pModelo.Add(new JProperty("Division", Division.Division));
        pModelo.Add(new JProperty("Observaciones", OrdenCompraEncabezado.Nota));

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(OrdenCompraEncabezado.IdCliente, pConexion);

        Dictionary<string, object> ParametrosOCD = new Dictionary<string, object>();
        ParametrosOCD.Add("Baja", 0);
        ParametrosOCD.Add("IdOrdenCompraEncabezado", OrdenCompraEncabezado.IdOrdenCompraEncabezado);

        COrdenCompraDetalle OrdenCompraDetalle = new COrdenCompraDetalle();

        foreach (COrdenCompraDetalle oOrdenCompraDetalle in OrdenCompraDetalle.LlenaObjetosFiltros(ParametrosOCD, pConexion))
        {
            pModelo.Add(new JProperty("IdPedidoEncabezado", oOrdenCompraDetalle.IdPedidoEncabezado));
            break;
        }

        return pModelo;
    }

    public static JArray ObtenerOrdenCompraProveedor(int pIdProveedor, CConexion pConexion)
    {
        COrdenCompraEncabezado OrdenCompra = new COrdenCompraEncabezado();
        JArray JOrdenCompras = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdProveedor", pIdProveedor);

        foreach (COrdenCompraEncabezado oOrdenCompra in OrdenCompra.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JOrdenCompra = new JObject();
            JOrdenCompra.Add("Valor", oOrdenCompra.IdOrdenCompraEncabezado);
            JOrdenCompra.Add("Descripcion", oOrdenCompra.Folio);
            JOrdenCompras.Add(JOrdenCompra);
        }
        return JOrdenCompras;
    }

    public static JArray ObtenerOrdenCompraProveedorRecepcion(int pIdProveedor, CConexion pConexion)
    {
        CSelectEspecifico OrdenCompra = new CSelectEspecifico();
        OrdenCompra.StoredProcedure.CommandText = "sp_OrdenCompra_ConsultarFiltrosRecepcion";
        OrdenCompra.StoredProcedure.Parameters.AddWithValue("pIdProveedor", pIdProveedor);
        OrdenCompra.Llena(pConexion);

        JArray JOrdeneCompras = new JArray();
        while (OrdenCompra.Registros.Read())
        {
            JObject JOrdenCompra = new JObject();
            JOrdenCompra.Add("Valor", Convert.ToInt32(OrdenCompra.Registros["IdOrdenCompraEncabezado"]));
            JOrdenCompra.Add("Descripcion", Convert.ToString(OrdenCompra.Registros["Folio"]));
            JOrdeneCompras.Add(JOrdenCompra);
        }
        OrdenCompra.Registros.Close();
        return JOrdeneCompras;
    }

    public string ObtenerJsonIndicadores(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);
        return JsonConvert.SerializeObject(dataSet);
    }

    public static int ObtenerSiguienteFolio(int IdSucrsal, CConexion pConexion)
    {
        int Folio = 1;
        CSelectEspecifico SelectEspecifico = new CSelectEspecifico();
        SelectEspecifico.StoredProcedure.CommandText = "sp_OrdenCombraObtenerFolioSucursal";
        SelectEspecifico.StoredProcedure.Parameters.Add("IdSucursal", SqlDbType.Int).Value = IdSucrsal;
        SelectEspecifico.Llena(pConexion);

        if (SelectEspecifico.Registros.Read())
        {
            Folio = Convert.ToInt32(SelectEspecifico.Registros["SiguienteFolio"]);
        }

        SelectEspecifico.CerrarConsulta();

        return Folio;
    }

}
