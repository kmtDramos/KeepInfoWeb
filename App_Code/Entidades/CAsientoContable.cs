using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;


public partial class CAsientoContable
{
    #region Constructores
    #endregion

    #region MetodosObtenerAsientos
    public JObject ObtenerAsientosFacturaCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, CConexion pConexion)
    {
        CSelectEspecifico ObtenerAsientos = new CSelectEspecifico();
        ObtenerAsientos.StoredProcedure.CommandText = "sp_Asiento_Consultar_ObtenerAsientoFacturaCliente";
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TamanoPaginacion", pTamanoPaginacion);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@PaginaActual", pPaginaActual);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@ColumnaOrden", pColumnaOrden);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TipoOrden", pTipoOrden);
        ObtenerAsientos.Llena(pConexion);

        JObject ListaAsientosContablesPendientes = new JObject();
        JObject Paginador = new JObject();
        if (ObtenerAsientos.Registros.Read())
        {
            Paginador.Add("NoRegistros", Convert.ToInt32(ObtenerAsientos.Registros["NoRegistros"]) + " registros");
            Paginador.Add("NoPaginas", Convert.ToInt32(ObtenerAsientos.Registros["NoPaginas"]));
            Paginador.Add("PaginaActual", Convert.ToInt32(ObtenerAsientos.Registros["PaginaActual"]));
            ListaAsientosContablesPendientes.Add("Paginador", Paginador);
        }

        ObtenerAsientos.Registros.NextResult();
        JArray AsientosContablesPendientes = new JArray();
        while (ObtenerAsientos.Registros.Read())
        {
            JObject AsientoContablePendiente = new JObject();
            AsientoContablePendiente.Add("IdFacturaEncabezado", Convert.ToInt32(ObtenerAsientos.Registros["IdFacturaEncabezado"]));
            AsientoContablePendiente.Add("CuentaContableCliente", Convert.ToString(ObtenerAsientos.Registros["CuentaContableCliente"]));
            AsientoContablePendiente.Add("RazonSocial", Convert.ToString(ObtenerAsientos.Registros["RazonSocial"]));
            AsientoContablePendiente.Add("CuentaContableMovimientos", Convert.ToString(ObtenerAsientos.Registros["CuentaContableMovimientos"]));
            AsientoContablePendiente.Add("CuentaMovimientos", Convert.ToString(ObtenerAsientos.Registros["DescripcionCuentaContableMovimientos"]));
            AsientoContablePendiente.Add("CuentaContableIVA", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVA"]));
            AsientoContablePendiente.Add("CuentaContableIVAAcreedor", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVAAcreedor"]));
            AsientoContablePendiente.Add("Subtotal", Convert.ToString(ObtenerAsientos.Registros["Subtotal"]));
            AsientoContablePendiente.Add("IVA", Convert.ToString(ObtenerAsientos.Registros["IVA"]));
            AsientoContablePendiente.Add("Total", Convert.ToString(ObtenerAsientos.Registros["Total"]));
            AsientoContablePendiente.Add("TotalClienteComplemento", Convert.ToString(ObtenerAsientos.Registros["TotalClienteComplemento"]));
            AsientoContablePendiente.Add("TotalConversion", Convert.ToString(ObtenerAsientos.Registros["TotalConversion"]));
            AsientoContablePendiente.Add("TotalVentasConversion", Convert.ToString(ObtenerAsientos.Registros["TotalVentasConversion"]));
            AsientosContablesPendientes.Add(AsientoContablePendiente);
        }
        ObtenerAsientos.Registros.Close();
        ObtenerAsientos.Registros.Dispose();
        ListaAsientosContablesPendientes.Add("AsientosContablesPendientes", AsientosContablesPendientes);
        return ListaAsientosContablesPendientes;
    }

    public JObject ObtenerAsientosCobroCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, CConexion pConexion)
    {
        CSelectEspecifico ObtenerAsientos = new CSelectEspecifico();
        ObtenerAsientos.StoredProcedure.CommandText = "sp_Asiento_Consultar_ObtenerAsientosCobroCliente";
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TamanoPaginacion", pTamanoPaginacion);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@PaginaActual", pPaginaActual);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@ColumnaOrden", pColumnaOrden);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TipoOrden", pTipoOrden);
        ObtenerAsientos.Llena(pConexion);

        JObject ListaAsientosContablesPendientes = new JObject();
        JObject Paginador = new JObject();
        if (ObtenerAsientos.Registros.Read())
        {
            Paginador.Add("NoRegistros", Convert.ToInt32(ObtenerAsientos.Registros["NoRegistros"]) + " registros");
            Paginador.Add("NoPaginas", Convert.ToInt32(ObtenerAsientos.Registros["NoPaginas"]));
            Paginador.Add("PaginaActual", Convert.ToInt32(ObtenerAsientos.Registros["PaginaActual"]));
            ListaAsientosContablesPendientes.Add("Paginador", Paginador);
        }

        ObtenerAsientos.Registros.NextResult();
        JArray AsientosContablesPendientes = new JArray();
        while (ObtenerAsientos.Registros.Read())
        {
            JObject AsientoContablePendiente = new JObject();
            AsientoContablePendiente.Add("TipoEntrada", Convert.ToString(ObtenerAsientos.Registros["TipoEntrada"]));
            AsientoContablePendiente.Add("FechaEmisionCC", Convert.ToString(ObtenerAsientos.Registros["FechaEmisionCC"]));
            AsientoContablePendiente.Add("FechaEmisionDiarioOficial", Convert.ToString(ObtenerAsientos.Registros["FechaEmisionDiarioOficial"]));
            AsientoContablePendiente.Add("IdTipoMonedaFactura", Convert.ToString(ObtenerAsientos.Registros["IdTipoMonedaFactura"]));
            AsientoContablePendiente.Add("IdTipoMonedaIngreso", Convert.ToString(ObtenerAsientos.Registros["IdTipoMonedaIngreso"]));
            AsientoContablePendiente.Add("IdFacturaEncabezado", Convert.ToString(ObtenerAsientos.Registros["IdFacturaEncabezado"]));
            AsientoContablePendiente.Add("Total", Convert.ToString(ObtenerAsientos.Registros["Total"]));
            AsientoContablePendiente.Add("IdIngreso", Convert.ToString(ObtenerAsientos.Registros["IdIngreso"]));
            AsientoContablePendiente.Add("CuentaContableCliente", Convert.ToString(ObtenerAsientos.Registros["CuentaContableCliente"]));
            AsientoContablePendiente.Add("CuentaContableClienteComplemento", Convert.ToString(ObtenerAsientos.Registros["CuentaContableClienteComplemento"]));
            AsientoContablePendiente.Add("CuentaContableClienteComplementoDesc", Convert.ToString(ObtenerAsientos.Registros["CuentaContableClienteComplementoDesc"]));
            AsientoContablePendiente.Add("RazonSocial", Convert.ToString(ObtenerAsientos.Registros["RazonSocial"]));
            AsientoContablePendiente.Add("CuentaContableBancosComplemento", Convert.ToString(ObtenerAsientos.Registros["CuentaContableBancosComplemento"]));
            AsientoContablePendiente.Add("CuentaContableBancos", Convert.ToString(ObtenerAsientos.Registros["CuentaContableBancos"]));
            AsientoContablePendiente.Add("CuentaBancaria", Convert.ToString(ObtenerAsientos.Registros["CuentaBancaria"]));
            AsientoContablePendiente.Add("TipoCambio", Convert.ToString(ObtenerAsientos.Registros["TipoCambio"]));
            AsientoContablePendiente.Add("TipoCambioDiarioOficial", Convert.ToString(ObtenerAsientos.Registros["TipoCambioDiarioOficial"]));
            AsientoContablePendiente.Add("ExisteTipoCambioDiarioOficial", Convert.ToString(ObtenerAsientos.Registros["ExisteTipoCambioDiarioOficial"]));
            AsientoContablePendiente.Add("TipoPoliza", Convert.ToString(ObtenerAsientos.Registros["TipoPoliza"]));
            AsientoContablePendiente.Add("RealSinTipoCambio", Convert.ToString(ObtenerAsientos.Registros["RealSinTipoCambio"]));
            AsientoContablePendiente.Add("Importe", Convert.ToString(ObtenerAsientos.Registros["Importe"]));
            AsientoContablePendiente.Add("CuentasComplementos", Convert.ToString(ObtenerAsientos.Registros["CuentasComplementos"]));
            AsientoContablePendiente.Add("TipoGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["TipoGananciaPerdida"]));
            AsientoContablePendiente.Add("MontoNuevo", Convert.ToString(ObtenerAsientos.Registros["MontoNuevo"]));
            AsientoContablePendiente.Add("ClienteComplemento", Convert.ToString(ObtenerAsientos.Registros["ClienteComplemento"]));
            AsientoContablePendiente.Add("IVATrasladadoPendienteGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["IVATrasladadoPendienteGananciaPerdida"]));
            AsientoContablePendiente.Add("GananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["GananciaPerdida"]));
            AsientoContablePendiente.Add("IVATrasladadoPagado", Convert.ToString(ObtenerAsientos.Registros["IVATrasladadoPagado"]));
            AsientoContablePendiente.Add("TotalCargo", Convert.ToString(ObtenerAsientos.Registros["TotalCargo"]));
            AsientoContablePendiente.Add("TotalAbono", Convert.ToString(ObtenerAsientos.Registros["TotalAbono"]));
            AsientoContablePendiente.Add("CuentaContableGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["CuentaContableGananciaPerdida"]));
            AsientoContablePendiente.Add("CuentaContableGananciaPerdidaDesc", Convert.ToString(ObtenerAsientos.Registros["CuentaContableGananciaPerdidaDesc"]));
            AsientoContablePendiente.Add("CuentaContableIVATrasladadoPendiente", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVATrasladadoPendiente"]));
            AsientoContablePendiente.Add("CuentaContableIVATrasladadoPagado", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVATrasladadoPagado"]));
            AsientosContablesPendientes.Add(AsientoContablePendiente);
        }

        ObtenerAsientos.Registros.Close();
        ObtenerAsientos.Registros.Dispose();
        ListaAsientosContablesPendientes.Add("AsientosContablesPendientes", AsientosContablesPendientes);
        return ListaAsientosContablesPendientes;
    }

    public JObject ObtenerAsientosFacturaProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, CConexion pConexion)
    {
        CSelectEspecifico ObtenerAsientos = new CSelectEspecifico();
        ObtenerAsientos.StoredProcedure.CommandText = "sp_Asiento_Consultar_ObtenerAsientosFacturaProveedor";
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TamanoPaginacion", pTamanoPaginacion);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@PaginaActual", pPaginaActual);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@ColumnaOrden", pColumnaOrden);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TipoOrden", pTipoOrden);
        ObtenerAsientos.Llena(pConexion);

        JObject ListaAsientosContablesPendientes = new JObject();

        JObject Paginador = new JObject();
        if (ObtenerAsientos.Registros.Read())
        {
            Paginador.Add("NoRegistros", Convert.ToInt32(ObtenerAsientos.Registros["NoRegistros"]) + " registros");
            Paginador.Add("NoPaginas", Convert.ToInt32(ObtenerAsientos.Registros["NoPaginas"]));
            Paginador.Add("PaginaActual", Convert.ToInt32(ObtenerAsientos.Registros["PaginaActual"]));
            ListaAsientosContablesPendientes.Add("Paginador", Paginador);
        }
        ObtenerAsientos.Registros.NextResult();

        JObject AsientoContablePendiente = new JObject();
        JArray AsientosContablesPendientes = new JArray();
        JArray JACuentasContablesMovimientos = new JArray();
        int idFactura = 0;
        decimal totalCargo = 0;
        while (ObtenerAsientos.Registros.Read())
        {
            if (idFactura != Convert.ToInt32(ObtenerAsientos.Registros["IdFactura"]))
            {
                idFactura = Convert.ToInt32(ObtenerAsientos.Registros["IdFactura"]);
                if (JACuentasContablesMovimientos.Count > 0)
                {
                    totalCargo = totalCargo + Convert.ToDecimal(AsientoContablePendiente["IVA"].ToString());
                    AsientoContablePendiente.Add("CuentasContablesMovimientos", JACuentasContablesMovimientos);
                    AsientoContablePendiente.Add("TotalCargo", totalCargo);
                    AsientoContablePendiente.Add("TotalAbono", Convert.ToDecimal(AsientoContablePendiente["TotalProveedorComplemento"].ToString()) + Convert.ToDecimal(AsientoContablePendiente["Total"].ToString()));
                    AsientosContablesPendientes.Add(AsientoContablePendiente);
                }

                AsientoContablePendiente = new JObject();
                AsientoContablePendiente.Add("CuentaContableProveedor", Convert.ToString(ObtenerAsientos.Registros["CuentaContableProveedor"]));
                AsientoContablePendiente.Add("RazonSocial", Convert.ToString(ObtenerAsientos.Registros["RazonSocial"]));
                AsientoContablePendiente.Add("CuentaContableIVA", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVA"]));
                AsientoContablePendiente.Add("CuentaContableIVADeudor", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVADeudor"]));
                AsientoContablePendiente.Add("Total", Convert.ToString(ObtenerAsientos.Registros["Total"]));
                AsientoContablePendiente.Add("Subtotal", Convert.ToString(ObtenerAsientos.Registros["Subtotal"]));
                AsientoContablePendiente.Add("IVA", Convert.ToString(ObtenerAsientos.Registros["IVA"]));
                AsientoContablePendiente.Add("IdFacturaProveedor", Convert.ToString(ObtenerAsientos.Registros["IdFactura"]));
                AsientoContablePendiente.Add("TotalProveedorComplemento", Convert.ToString(ObtenerAsientos.Registros["TotalProveedorComplemento"]));
                AsientoContablePendiente.Add("CuentaProveedorComplemento", Convert.ToString(ObtenerAsientos.Registros["CuentaProveedorComplemento"]));

                JACuentasContablesMovimientos = new JArray();
                JObject JCuentaContableMovimiento = new JObject();
                JCuentaContableMovimiento.Add("CuentaContableMovimientos", Convert.ToString(ObtenerAsientos.Registros["CuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("CuentaMovimientos", Convert.ToString(ObtenerAsientos.Registros["DescripcionCuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("TotalTipoCompra", Convert.ToString(ObtenerAsientos.Registros["TotalTipoCompra"]));
                totalCargo = Convert.ToDecimal(ObtenerAsientos.Registros["TotalTipoCompra"]);
                JACuentasContablesMovimientos.Add(JCuentaContableMovimiento);
            }
            else
            {
                JObject JCuentaContableMovimiento = new JObject();
                JCuentaContableMovimiento.Add("CuentaContableMovimientos", Convert.ToString(ObtenerAsientos.Registros["CuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("CuentaMovimientos", Convert.ToString(ObtenerAsientos.Registros["DescripcionCuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("TotalTipoCompra", Convert.ToString(ObtenerAsientos.Registros["TotalTipoCompra"]));
                totalCargo = Convert.ToDecimal(Convert.ToString(ObtenerAsientos.Registros["TotalTipoCompra"]));
                JACuentasContablesMovimientos.Add(JCuentaContableMovimiento);
            }
        }
        totalCargo = totalCargo + Convert.ToDecimal(AsientoContablePendiente["IVA"].ToString());
        AsientoContablePendiente.Add("CuentasContablesMovimientos", JACuentasContablesMovimientos);
        AsientosContablesPendientes.Add(AsientoContablePendiente);
        AsientoContablePendiente.Add("TotalCargo", totalCargo);
        AsientoContablePendiente.Add("TotalAbono", Convert.ToDecimal(AsientoContablePendiente["TotalProveedorComplemento"].ToString()) + Convert.ToDecimal(AsientoContablePendiente["Total"].ToString()));
        ListaAsientosContablesPendientes.Add("AsientosContablesPendientes", AsientosContablesPendientes);
        ObtenerAsientos.Registros.Close();
        ObtenerAsientos.Registros.Dispose();
        return ListaAsientosContablesPendientes;
    }

    public JObject ObtenerAsientosPagoProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, CConexion pConexion)
    {
        CSelectEspecifico ObtenerAsientos = new CSelectEspecifico();
        ObtenerAsientos.StoredProcedure.CommandText = "sp_Asiento_Consultar_ObtenerAsientosPagoProveedor";
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TamanoPaginacion", pTamanoPaginacion);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@PaginaActual", pPaginaActual);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@ColumnaOrden", pColumnaOrden);
        ObtenerAsientos.StoredProcedure.Parameters.AddWithValue("@TipoOrden", pTipoOrden);
        ObtenerAsientos.Llena(pConexion);
        JObject ListaAsientosContablesPendientes = new JObject();
        JObject Paginador = new JObject();
        if (ObtenerAsientos.Registros.Read())
        {
            Paginador.Add("NoRegistros", Convert.ToInt32(ObtenerAsientos.Registros["NoRegistros"]) + " registros");
            Paginador.Add("NoPaginas", Convert.ToInt32(ObtenerAsientos.Registros["NoPaginas"]));
            Paginador.Add("PaginaActual", Convert.ToInt32(ObtenerAsientos.Registros["PaginaActual"]));
            ListaAsientosContablesPendientes.Add("Paginador", Paginador);
        }

        ObtenerAsientos.Registros.NextResult();
        JArray AsientosContablesPendientes = new JArray();
        while (ObtenerAsientos.Registros.Read())
        {
            JObject AsientoContablePendiente = new JObject();
            AsientoContablePendiente.Add("TipoSalida", Convert.ToString(ObtenerAsientos.Registros["TipoSalida"]));
            AsientoContablePendiente.Add("FechaEmisionCC", Convert.ToString(ObtenerAsientos.Registros["FechaEmisionCC"]));
            AsientoContablePendiente.Add("FechaEmisionDiarioOficial", Convert.ToString(ObtenerAsientos.Registros["FechaEmisionDiarioOficial"]));
            AsientoContablePendiente.Add("IdTipoMonedaFactura", Convert.ToString(ObtenerAsientos.Registros["IdTipoMonedaFactura"]));
            AsientoContablePendiente.Add("IdTipoMonedaEgreso", Convert.ToString(ObtenerAsientos.Registros["IdTipoMonedaEgreso"]));
            AsientoContablePendiente.Add("IdEncabezadoFacturaProveedor", Convert.ToString(ObtenerAsientos.Registros["IdEncabezadoFacturaProveedor"]));
            AsientoContablePendiente.Add("Total", Convert.ToString(ObtenerAsientos.Registros["Total"]));
            AsientoContablePendiente.Add("IdEgreso", Convert.ToString(ObtenerAsientos.Registros["IdEgreso"]));
            AsientoContablePendiente.Add("CuentaContableProveedor", Convert.ToString(ObtenerAsientos.Registros["CuentaContableProveedor"]));
            AsientoContablePendiente.Add("CuentaContableDolares", Convert.ToString(ObtenerAsientos.Registros["CuentaContableProveedorComplemento"]));
            AsientoContablePendiente.Add("RazonSocial", Convert.ToString(ObtenerAsientos.Registros["RazonSocial"]));
            AsientoContablePendiente.Add("CuentaContableBancos", Convert.ToString(ObtenerAsientos.Registros["CuentaContableBancos"]));
            AsientoContablePendiente.Add("CuentaContableBancosComplemento", Convert.ToString(ObtenerAsientos.Registros["CuentaContableBancosComplemento"]));
            AsientoContablePendiente.Add("IdCuentaBancaria", Convert.ToString(ObtenerAsientos.Registros["IdCuentaBancaria"]));
            AsientoContablePendiente.Add("CuentaBancaria", Convert.ToString(ObtenerAsientos.Registros["CuentaBancaria"]));
            AsientoContablePendiente.Add("TipoCambio", Convert.ToString(ObtenerAsientos.Registros["TipoCambio"]));
            AsientoContablePendiente.Add("TipoCambioDiarioOficial", Convert.ToString(ObtenerAsientos.Registros["TipoCambioDiarioOficial"]));
            AsientoContablePendiente.Add("ExisteTipoCambioDiarioOficial", Convert.ToString(ObtenerAsientos.Registros["ExisteTipoCambioDiarioOficial"]));
            AsientoContablePendiente.Add("TipoPoliza", Convert.ToString(ObtenerAsientos.Registros["TipoPoliza"]));
            AsientoContablePendiente.Add("RealSinTipoCambio", Convert.ToString(ObtenerAsientos.Registros["RealSinTipoCambio"]));
            AsientoContablePendiente.Add("Importe", Convert.ToString(ObtenerAsientos.Registros["Importe"]));
            AsientoContablePendiente.Add("CuentasComplementos", Convert.ToString(ObtenerAsientos.Registros["CuentasComplementos"]));
            AsientoContablePendiente.Add("TipoGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["TipoGananciaPerdida"]));
            AsientoContablePendiente.Add("IdTipoCuentaContableGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["IdTipoCuentaContableGananciaPerdida"]));
            AsientoContablePendiente.Add("MontoAnterior", Convert.ToString(ObtenerAsientos.Registros["MontoAnterior"]));
            AsientoContablePendiente.Add("MontoNuevo", Convert.ToString(ObtenerAsientos.Registros["MontoNuevo"]));
            AsientoContablePendiente.Add("ProveedorComplemento", Convert.ToString(ObtenerAsientos.Registros["ProveedorComplemento"]));
            AsientoContablePendiente.Add("IVAAcreditablePendienteGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["IVAAcreditablePendienteGananciaPerdida"]));
            AsientoContablePendiente.Add("GananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["GananciaPerdida"]));
            AsientoContablePendiente.Add("IVAAcreditablePagado", Convert.ToString(ObtenerAsientos.Registros["IVAAcreditablePagado"]));
            AsientoContablePendiente.Add("TotalCargo", Convert.ToString(ObtenerAsientos.Registros["TotalCargo"]));
            AsientoContablePendiente.Add("TotalAbono", Convert.ToString(ObtenerAsientos.Registros["TotalAbono"]));
            AsientoContablePendiente.Add("IdCuentaContableGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["IdCuentaContableGananciaPerdida"]));
            AsientoContablePendiente.Add("CuentaContableGananciaPerdida", Convert.ToString(ObtenerAsientos.Registros["CuentaContableGananciaPerdida"]));
            AsientoContablePendiente.Add("CuentaContableGananciaPerdidaDesc", Convert.ToString(ObtenerAsientos.Registros["CuentaContableGananciaPerdidaDesc"]));
            AsientoContablePendiente.Add("CuentaContableIVAAcreditablePendiente", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVAAcreditablePendiente"]));
            AsientoContablePendiente.Add("CuentaContableIVAAcreditablePagado", Convert.ToString(ObtenerAsientos.Registros["CuentaContableIVAAcreditablePagado"]));

            AsientoContablePendiente.Add("IdCuentaContableProveedor", Convert.ToString(ObtenerAsientos.Registros["IdCuentaContableProveedor"]));
            AsientoContablePendiente.Add("CuentaContableProveedorComplemento", Convert.ToString(ObtenerAsientos.Registros["CuentaContableProveedorComplemento"]));
            AsientoContablePendiente.Add("CuentaContableProveedorComplementoDesc", Convert.ToString(ObtenerAsientos.Registros["CuentaContableProveedorComplementoDesc"]));

            AsientoContablePendiente.Add("IdProveedor", Convert.ToString(ObtenerAsientos.Registros["IdProveedor"]));
            AsientoContablePendiente.Add("IdIVA", Convert.ToString(ObtenerAsientos.Registros["IdIVA"]));
            AsientosContablesPendientes.Add(AsientoContablePendiente);
        }
        ObtenerAsientos.Registros.Close();
        ObtenerAsientos.Registros.Dispose();
        ListaAsientosContablesPendientes.Add("AsientosContablesPendientes", AsientosContablesPendientes);
        return ListaAsientosContablesPendientes;
    }
    #endregion

    #region MetodosObtenerAsientosGenerados
    public static JObject ObtenerJsonAsientoGeneradoCobroCliente(int pIdIngreso, int pIdTipoEntrada, int pIdFacturaEncabezado, CConexion pConexion)
    {
        JObject JAsiento = new JObject();
        CSelectEspecifico ObtenerAsiento = new CSelectEspecifico();
        ObtenerAsiento.StoredProcedure.CommandType = CommandType.StoredProcedure;
        ObtenerAsiento.StoredProcedure.CommandText = "sp_Asiento_Consultar_ObtenerAsientoCobroCliente";
        ObtenerAsiento.StoredProcedure.Parameters.AddWithValue("pIdIngreso", pIdIngreso);
        ObtenerAsiento.StoredProcedure.Parameters.AddWithValue("pIdTipoEntrada", pIdTipoEntrada);
        ObtenerAsiento.StoredProcedure.Parameters.AddWithValue("pIdFacturaEncabezado", pIdFacturaEncabezado);
        ObtenerAsiento.Llena(pConexion);
        if (ObtenerAsiento.Registros.Read())
        {
            JAsiento.Add("TipoEntrada", Convert.ToString(ObtenerAsiento.Registros["TipoEntrada"]));
            JAsiento.Add("FechaEmisionCC", Convert.ToString(ObtenerAsiento.Registros["FechaEmisionCC"]));
            JAsiento.Add("FechaEmisionDiarioOficial", Convert.ToString(ObtenerAsiento.Registros["FechaEmisionDiarioOficial"]));
            JAsiento.Add("IdTipoMonedaFactura", Convert.ToInt32(ObtenerAsiento.Registros["IdTipoMonedaFactura"]));
            JAsiento.Add("IdTipoMonedaIngreso", Convert.ToInt32(ObtenerAsiento.Registros["IdTipoMonedaIngreso"]));

            JAsiento.Add("IdFacturaEncabezado", Convert.ToInt32(ObtenerAsiento.Registros["IdFacturaEncabezado"]));
            JAsiento.Add("Total", Convert.ToDecimal(ObtenerAsiento.Registros["Total"]));
            JAsiento.Add("IdIngreso", Convert.ToInt32(ObtenerAsiento.Registros["IdIngreso"]));
            JAsiento.Add("CuentaContableCliente", Convert.ToString(ObtenerAsiento.Registros["CuentaContableCliente"]));
            JAsiento.Add("CuentaContableDolares", Convert.ToString(ObtenerAsiento.Registros["CuentaContableDolares"]));

            JAsiento.Add("RazonSocial", Convert.ToString(ObtenerAsiento.Registros["RazonSocial"]));
            JAsiento.Add("CuentaContableBancos", Convert.ToString(ObtenerAsiento.Registros["CuentaContableBancos"]));
            JAsiento.Add("CuentaContableBancosComplemento", Convert.ToString(ObtenerAsiento.Registros["CuentaContableBancosComplemento"]));
            JAsiento.Add("CuentaBancaria", Convert.ToString(ObtenerAsiento.Registros["CuentaBancaria"]));
            JAsiento.Add("TipoCambio", Convert.ToDecimal(ObtenerAsiento.Registros["TipoCambio"]));

            JAsiento.Add("TipoCambioDiarioOficial", Convert.ToDecimal(ObtenerAsiento.Registros["TipoCambioDiarioOficial"]));
            JAsiento.Add("ExisteTipoCambioDiarioOficial", Convert.ToString(ObtenerAsiento.Registros["ExisteTipoCambioDiarioOficial"]));
            JAsiento.Add("TipoPoliza", Convert.ToString(ObtenerAsiento.Registros["TipoPoliza"]));
            JAsiento.Add("RealSinTipoCambio", Convert.ToDecimal(ObtenerAsiento.Registros["RealSinTipoCambio"]));
            JAsiento.Add("Importe", Convert.ToDecimal(ObtenerAsiento.Registros["Importe"]));

            JAsiento.Add("CuentaContableClienteComplemento", Convert.ToString(ObtenerAsiento.Registros["CuentaContableClienteComplemento"]));
            JAsiento.Add("CuentaContableClienteComplementoDesc", Convert.ToString(ObtenerAsiento.Registros["CuentaContableClienteComplementoDesc"]));
            JAsiento.Add("CuentasComplementos", Convert.ToString(ObtenerAsiento.Registros["CuentasComplementos"]));
            JAsiento.Add("TipoGananciaPerdida", Convert.ToString(ObtenerAsiento.Registros["TipoGananciaPerdida"]));
            JAsiento.Add("IdTipoCuentaContableGananciaPerdida", Convert.ToInt32(ObtenerAsiento.Registros["IdTipoCuentaContableGananciaPerdida"]));

            JAsiento.Add("MontoAnterior", Convert.ToDecimal(ObtenerAsiento.Registros["MontoAnterior"]));
            JAsiento.Add("MontoNuevo", Convert.ToDecimal(ObtenerAsiento.Registros["MontoNuevo"]));
            JAsiento.Add("ClienteComplemento", Convert.ToString(ObtenerAsiento.Registros["ClienteComplemento"]));
            JAsiento.Add("IVATrasladadoPendienteGananciaPerdida", Convert.ToDecimal(ObtenerAsiento.Registros["IVATrasladadoPendienteGananciaPerdida"]));
            JAsiento.Add("GananciaPerdida", Convert.ToDecimal(ObtenerAsiento.Registros["GananciaPerdida"]));

            JAsiento.Add("IVATrasladadoPagado", Convert.ToDecimal(ObtenerAsiento.Registros["IVATrasladadoPagado"]));
            JAsiento.Add("CuentaContableIVATrasladadoPendiente", Convert.ToString(ObtenerAsiento.Registros["CuentaContableIVATrasladadoPendiente"]));
            JAsiento.Add("CuentaContableIVATrasladadoPagado", Convert.ToString(ObtenerAsiento.Registros["CuentaContableIVATrasladadoPagado"]));
            JAsiento.Add("TotalCargo", Convert.ToDecimal(ObtenerAsiento.Registros["TotalCargo"]));
            JAsiento.Add("TotalAbono", Convert.ToDecimal(ObtenerAsiento.Registros["TotalAbono"]));

            JAsiento.Add("CuentaContableGananciaPerdida", Convert.ToString(ObtenerAsiento.Registros["CuentaContableGananciaPerdida"]));
            JAsiento.Add("CuentaContableGananciaPerdidaDesc", Convert.ToString(ObtenerAsiento.Registros["CuentaContableGananciaPerdidaDesc"]));
            JAsiento.Add("IdCuentaBancaria", Convert.ToInt32(ObtenerAsiento.Registros["IdCuentaBancaria"]));
            JAsiento.Add("IdCliente", Convert.ToInt32(ObtenerAsiento.Registros["IdCliente"]));
            JAsiento.Add("IdCuentaContableCliente", Convert.ToInt32(ObtenerAsiento.Registros["IdCuentaContableCliente"]));

            JAsiento.Add("IdCuentaContableGananciaPerdida", Convert.ToInt32(ObtenerAsiento.Registros["IdCuentaContableGananciaPerdida"]));
            JAsiento.Add("IdIVA", Convert.ToInt32(ObtenerAsiento.Registros["IdIVA"]));
        }
        ObtenerAsiento.CerrarConsulta();
        return JAsiento;
    }

    public static JObject ObtenerJsonAsientoGeneradoPagoProveedor(int pIdEgreso, int pIdTipoSalida, int pIdEncabezadoFactura, CConexion pConexion)
    {
        JObject JAsiento = new JObject();
        CSelectEspecifico ObtenerAsiento = new CSelectEspecifico();
        ObtenerAsiento.StoredProcedure.CommandType = CommandType.StoredProcedure;
        ObtenerAsiento.StoredProcedure.CommandText = "sp_Asiento_Consultar_ObtenerAsientoPagoProveedor";
        ObtenerAsiento.StoredProcedure.Parameters.AddWithValue("pIdEgreso", pIdEgreso);
        ObtenerAsiento.StoredProcedure.Parameters.AddWithValue("pIdTipoSalida", pIdTipoSalida);
        ObtenerAsiento.StoredProcedure.Parameters.AddWithValue("pIdEncabezadoFactura", pIdEncabezadoFactura);
        ObtenerAsiento.Llena(pConexion);
        if (ObtenerAsiento.Registros.Read())
        {
            JAsiento.Add("TipoSalida", Convert.ToString(ObtenerAsiento.Registros["TipoSalida"]));
            JAsiento.Add("FechaEmisionCC", Convert.ToString(ObtenerAsiento.Registros["FechaEmisionCC"]));
            JAsiento.Add("FechaEmisionDiarioOficial", Convert.ToString(ObtenerAsiento.Registros["FechaEmisionDiarioOficial"]));
            JAsiento.Add("IdTipoMonedaFactura", Convert.ToInt32(ObtenerAsiento.Registros["IdTipoMonedaFactura"]));
            JAsiento.Add("IdTipoMonedaEgreso", Convert.ToInt32(ObtenerAsiento.Registros["IdTipoMonedaEgreso"]));

            JAsiento.Add("IdEncabezadoFacturaProveedor", Convert.ToInt32(ObtenerAsiento.Registros["IdEncabezadoFacturaProveedor"]));
            JAsiento.Add("Total", Convert.ToDecimal(ObtenerAsiento.Registros["Total"]));
            JAsiento.Add("IdEgreso", Convert.ToInt32(ObtenerAsiento.Registros["IdEgreso"]));
            JAsiento.Add("CuentaContableProveedor", Convert.ToString(ObtenerAsiento.Registros["CuentaContableProveedor"]));
            JAsiento.Add("CuentaContableDolares", Convert.ToString(ObtenerAsiento.Registros["CuentaContableDolares"]));

            JAsiento.Add("RazonSocial", Convert.ToString(ObtenerAsiento.Registros["RazonSocial"]));
            JAsiento.Add("CuentaContableBancos", Convert.ToString(ObtenerAsiento.Registros["CuentaContableBancos"]));
            JAsiento.Add("CuentaContableBancosComplemento", Convert.ToString(ObtenerAsiento.Registros["CuentaContableBancosComplemento"]));
            JAsiento.Add("IdCuentaBancaria", Convert.ToString(ObtenerAsiento.Registros["IdCuentaBancaria"]));
            JAsiento.Add("CuentaBancaria", Convert.ToString(ObtenerAsiento.Registros["CuentaBancaria"]));
            JAsiento.Add("TipoCambio", Convert.ToDecimal(ObtenerAsiento.Registros["TipoCambio"]));

            JAsiento.Add("TipoCambioDiarioOficial", Convert.ToDecimal(ObtenerAsiento.Registros["TipoCambioDiarioOficial"]));
            JAsiento.Add("ExisteTipoCambioDiarioOficial", Convert.ToString(ObtenerAsiento.Registros["ExisteTipoCambioDiarioOficial"]));
            JAsiento.Add("TipoPoliza", Convert.ToString(ObtenerAsiento.Registros["TipoPoliza"]));
            JAsiento.Add("RealSinTipoCambio", Convert.ToDecimal(ObtenerAsiento.Registros["RealSinTipoCambio"]));
            JAsiento.Add("Importe", Convert.ToDecimal(ObtenerAsiento.Registros["Importe"]));

            JAsiento.Add("CuentaContableProveedorComplemento", Convert.ToString(ObtenerAsiento.Registros["CuentaContableProveedorComplemento"]));
            JAsiento.Add("CuentaContableProveedorComplementoDesc", Convert.ToString(ObtenerAsiento.Registros["CuentaContableProveedorComplementoDesc"]));
            JAsiento.Add("CuentasComplementos", Convert.ToString(ObtenerAsiento.Registros["CuentasComplementos"]));
            JAsiento.Add("TipoGananciaPerdida", Convert.ToString(ObtenerAsiento.Registros["TipoGananciaPerdida"]));
            JAsiento.Add("IdTipoCuentaContableGananciaPerdida", Convert.ToInt32(ObtenerAsiento.Registros["IdTipoCuentaContableGananciaPerdida"]));

            JAsiento.Add("MontoAnterior", Convert.ToDecimal(ObtenerAsiento.Registros["MontoAnterior"]));
            JAsiento.Add("MontoNuevo", Convert.ToDecimal(ObtenerAsiento.Registros["MontoNuevo"]));
            JAsiento.Add("ProveedorComplemento", Convert.ToString(ObtenerAsiento.Registros["ProveedorComplemento"]));
            JAsiento.Add("IVAAcreditablePendienteGananciaPerdida", Convert.ToDecimal(ObtenerAsiento.Registros["IVAAcreditablePendienteGananciaPerdida"]));
            JAsiento.Add("GananciaPerdida", Convert.ToDecimal(ObtenerAsiento.Registros["GananciaPerdida"]));

            JAsiento.Add("IVAAcreditablePagado", Convert.ToDecimal(ObtenerAsiento.Registros["IVAAcreditablePagado"]));
            JAsiento.Add("CuentaContableIVAAcreditablePendiente", Convert.ToString(ObtenerAsiento.Registros["CuentaContableIVAAcreditablePendiente"]));
            JAsiento.Add("CuentaContableIVAAcreditablePagado", Convert.ToString(ObtenerAsiento.Registros["CuentaContableIVAAcreditablePagado"]));
            JAsiento.Add("TotalCargo", Convert.ToDecimal(ObtenerAsiento.Registros["TotalCargo"]));
            JAsiento.Add("TotalAbono", Convert.ToDecimal(ObtenerAsiento.Registros["TotalAbono"]));

            JAsiento.Add("CuentaContableGananciaPerdida", Convert.ToString(ObtenerAsiento.Registros["CuentaContableGananciaPerdida"]));
            JAsiento.Add("CuentaContableGananciaPerdidaDesc", Convert.ToString(ObtenerAsiento.Registros["CuentaContableGananciaPerdidaDesc"]));
            JAsiento.Add("IdProveedor", Convert.ToInt32(ObtenerAsiento.Registros["IdProveedor"]));
            JAsiento.Add("IdCuentaContableProveedor", Convert.ToInt32(ObtenerAsiento.Registros["IdCuentaContableProveedor"]));

            JAsiento.Add("IdCuentaContableGananciaPerdida", Convert.ToInt32(ObtenerAsiento.Registros["IdCuentaContableGananciaPerdida"]));
            JAsiento.Add("IdIVA", Convert.ToInt32(ObtenerAsiento.Registros["IdIVA"]));
        }
        ObtenerAsiento.CerrarConsulta();
        return JAsiento;
    }

    public static JObject ObtenerJsonAsientoGeneradoFacturaProveedor(int pIdEncabezadoFacturaProveedor, CConexion pConexion)
    {
        CSelectEspecifico ObtenerAsiento = new CSelectEspecifico();
        ObtenerAsiento.StoredProcedure.CommandType = CommandType.StoredProcedure;
        ObtenerAsiento.StoredProcedure.CommandText = "sp_Asiento_Consultar_ObtenerAsientoFacturaProveedor";
        ObtenerAsiento.StoredProcedure.Parameters.AddWithValue("pIdEncabezadoFacturaProveedor", pIdEncabezadoFacturaProveedor);
        ObtenerAsiento.Llena(pConexion);

        JObject AsientoContablePendiente = new JObject();
        JArray JACuentasContablesMovimientos = new JArray();
        int idFactura = 0;
        decimal totalCargo = 0;
        while (ObtenerAsiento.Registros.Read())
        {
            if (idFactura != Convert.ToInt32(ObtenerAsiento.Registros["IdFactura"]))
            {
                idFactura = Convert.ToInt32(ObtenerAsiento.Registros["IdFactura"]);
                if (JACuentasContablesMovimientos.Count > 0)
                {
                    totalCargo = totalCargo + Convert.ToDecimal(AsientoContablePendiente["IVA"].ToString());
                    AsientoContablePendiente.Add("CuentasContablesMovimientos", JACuentasContablesMovimientos);
                    AsientoContablePendiente.Add("TotalCargo", totalCargo);
                    AsientoContablePendiente.Add("TotalAbono", Convert.ToDecimal(AsientoContablePendiente["TotalProveedorComplemento"].ToString()) + Convert.ToDecimal(AsientoContablePendiente["Total"].ToString()));
                }

                AsientoContablePendiente = new JObject();
                AsientoContablePendiente.Add("IdFactura", Convert.ToString(ObtenerAsiento.Registros["IdFactura"]));
                AsientoContablePendiente.Add("IdProveedor", Convert.ToString(ObtenerAsiento.Registros["IdProveedor"]));
                AsientoContablePendiente.Add("CuentaContableProveedor", Convert.ToString(ObtenerAsiento.Registros["CuentaContableProveedor"]));
                AsientoContablePendiente.Add("RazonSocial", Convert.ToString(ObtenerAsiento.Registros["RazonSocial"]));
                AsientoContablePendiente.Add("CuentaContableIVA", Convert.ToString(ObtenerAsiento.Registros["CuentaContableIVA"]));
                AsientoContablePendiente.Add("CuentaContableIVADeudor", Convert.ToString(ObtenerAsiento.Registros["CuentaContableIVADeudor"]));
                AsientoContablePendiente.Add("Total", Convert.ToString(ObtenerAsiento.Registros["Total"]));
                AsientoContablePendiente.Add("Subtotal", Convert.ToString(ObtenerAsiento.Registros["Subtotal"]));
                AsientoContablePendiente.Add("IVA", Convert.ToString(ObtenerAsiento.Registros["IVA"]));
                AsientoContablePendiente.Add("IdIVA", Convert.ToString(ObtenerAsiento.Registros["IdIVA"]));
                AsientoContablePendiente.Add("CuentaContableTrasladado", Convert.ToString(ObtenerAsiento.Registros["CuentaContableTrasladado"]));
                AsientoContablePendiente.Add("DescripcionIVA", Convert.ToString(ObtenerAsiento.Registros["DescripcionIVA"]) + " trasladado");
                AsientoContablePendiente.Add("IdFacturaProveedor", Convert.ToString(ObtenerAsiento.Registros["IdFactura"]));
                AsientoContablePendiente.Add("TotalProveedorComplemento", Convert.ToString(ObtenerAsiento.Registros["TotalProveedorComplemento"]));
                AsientoContablePendiente.Add("IdCuentaProveedorComplemento", Convert.ToString(ObtenerAsiento.Registros["IdCuentaProveedorComplemento"]));
                AsientoContablePendiente.Add("CuentaProveedorComplemento", Convert.ToString(ObtenerAsiento.Registros["CuentaProveedorComplemento"]));
                AsientoContablePendiente.Add("CuentaProveedorComplementoDesc", Convert.ToString(ObtenerAsiento.Registros["CuentaProveedorComplementoDesc"]));
                AsientoContablePendiente.Add("TotalConversion", Convert.ToString(ObtenerAsiento.Registros["TotalConversion"]));

                JACuentasContablesMovimientos = new JArray();
                JObject JCuentaContableMovimiento = new JObject();
                JCuentaContableMovimiento.Add("IdSucursal", Convert.ToString(ObtenerAsiento.Registros["IdSucursal"]));
                JCuentaContableMovimiento.Add("IdDivision", Convert.ToString(ObtenerAsiento.Registros["IdDivision"]));
                JCuentaContableMovimiento.Add("IdTipoCompra", Convert.ToString(ObtenerAsiento.Registros["IdTipoCompra"]));
                JCuentaContableMovimiento.Add("IdCuentaContable", Convert.ToString(ObtenerAsiento.Registros["IdCuentaContable"]));
                JCuentaContableMovimiento.Add("CuentaContableMovimientos", Convert.ToString(ObtenerAsiento.Registros["CuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("CuentaMovimientos", Convert.ToString(ObtenerAsiento.Registros["DescripcionCuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("TotalTipoCompra", Convert.ToString(ObtenerAsiento.Registros["TotalTipoCompra"]));
                totalCargo = Convert.ToDecimal(ObtenerAsiento.Registros["TotalTipoCompra"]);
                JACuentasContablesMovimientos.Add(JCuentaContableMovimiento);
            }
            else
            {
                JObject JCuentaContableMovimiento = new JObject();
                JCuentaContableMovimiento.Add("IdSucursal", Convert.ToString(ObtenerAsiento.Registros["IdSucursal"]));
                JCuentaContableMovimiento.Add("IdDivision", Convert.ToString(ObtenerAsiento.Registros["IdDivision"]));
                JCuentaContableMovimiento.Add("IdTipoCompra", Convert.ToString(ObtenerAsiento.Registros["IdTipoCompra"]));
                JCuentaContableMovimiento.Add("IdCuentaContable", Convert.ToString(ObtenerAsiento.Registros["IdCuentaContable"]));
                JCuentaContableMovimiento.Add("CuentaContableMovimientos", Convert.ToString(ObtenerAsiento.Registros["CuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("CuentaMovimientos", Convert.ToString(ObtenerAsiento.Registros["DescripcionCuentaContableMovimientos"]));
                JCuentaContableMovimiento.Add("TotalTipoCompra", Convert.ToString(ObtenerAsiento.Registros["TotalTipoCompra"]));
                totalCargo = Convert.ToDecimal(Convert.ToString(ObtenerAsiento.Registros["TotalTipoCompra"]));
                JACuentasContablesMovimientos.Add(JCuentaContableMovimiento);
            }
        }
        totalCargo = totalCargo + Convert.ToDecimal(AsientoContablePendiente["IVA"].ToString());
        AsientoContablePendiente.Add("CuentasContablesMovimientos", JACuentasContablesMovimientos);
        AsientoContablePendiente.Add("TotalCargo", totalCargo);
        AsientoContablePendiente.Add("TotalAbono", Convert.ToDecimal(AsientoContablePendiente["TotalProveedorComplemento"].ToString()) + Convert.ToDecimal(AsientoContablePendiente["Total"].ToString()));
        ObtenerAsiento.CerrarConsulta();
        return AsientoContablePendiente;
    }
    #endregion
}