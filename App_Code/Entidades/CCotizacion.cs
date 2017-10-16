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


public partial class CCotizacion
{
    //Atributos
    public SqlCommand StoredProcedure = new SqlCommand();

    //Constructores

    //Metodos
    public JToken ObtenerJsonJObject(CConexion pConexion)
    {
        StoredProcedure.CommandType = CommandType.StoredProcedure;
        StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(StoredProcedure);
        dataAdapter.Fill(dataSet);

        JObject oJson = JObject.Parse(JsonConvert.SerializeObject(dataSet));
        return oJson["Table"];
    }

    //Metodos Especiales
    public static JObject ObtenerJsonCotizacionEncabezado(JObject pModelo, int pIdCotizacion, CConexion pConexion)
    {
        CCotizacion Cotizacion = new CCotizacion();
        Cotizacion.LlenaObjeto(pIdCotizacion, pConexion);
        pModelo.Add(new JProperty("IdCotizacion", Cotizacion.IdCotizacion));
        pModelo.Add(new JProperty("IdCliente", Cotizacion.IdCliente));
        pModelo.Add(new JProperty("Folio", Cotizacion.Folio));
        pModelo.Add(new JProperty("IdSucursalEjecutaServicio", Cotizacion.IdSucursalEjecutaServicio));
        pModelo.Add(new JProperty("IdEstatusCotizacion", Cotizacion.IdEstatusCotizacion));
        if (Cotizacion.IdEstatusCotizacion == 1)
        {
            pModelo.Add(new JProperty("TipoFormato", "Borrador"));
        }
        else if (Cotizacion.IdEstatusCotizacion == 2)
        {
            pModelo.Add(new JProperty("TipoFormato", "Cotización"));
        }
        else if (Cotizacion.IdEstatusCotizacion == 3)
        {
            pModelo.Add(new JProperty("TipoFormato", "Pedido"));
        }
        else if (Cotizacion.IdEstatusCotizacion == 6)
        {
            pModelo.Add(new JProperty("TipoFormato", "Facturado"));
        }
        else
        {
            pModelo.Add(new JProperty("TipoFormato", "Cancelado"));
        }
        pModelo.Add(new JProperty("Subtotal", Cotizacion.SubTotal));
        pModelo.Add(new JProperty("IVA", Cotizacion.IVA));
        pModelo.Add(new JProperty("Total", Cotizacion.Total));
        pModelo.Add(new JProperty("CantidadTotalLetra", Cotizacion.CantidadTotalLetra));

        CCliente Cliente = new CCliente();
        Cliente.LlenaObjeto(Cotizacion.IdCliente, pConexion);

        COrganizacion Organizacion = new COrganizacion();
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        pModelo.Add(new JProperty("IdOrganizacion", Organizacion.IdOrganizacion));
        pModelo.Add(new JProperty("RFC", Organizacion.RFC));
        pModelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.LlenaObjeto(Cotizacion.IdTipoMoneda, pConexion);
        pModelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
        pModelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

        pModelo.Add(new JProperty("FechaAlta", Cotizacion.FechaAlta.ToShortDateString()));
        pModelo.Add(new JProperty("ValidoHasta", Cotizacion.ValidoHasta.ToShortDateString()));
        pModelo.Add(new JProperty("Nota", Cotizacion.Nota));
        pModelo.Add(new JProperty("IdUsuarioCotizador", Cotizacion.IdUsuarioCotizador));
        pModelo.Add(new JProperty("IdUsuarioAgente", Cotizacion.IdUsuarioAgente));
        pModelo.Add(new JProperty("AutorizacionIVA", Cotizacion.AutorizacionIVA));

        CNivelInteresCotizacion NivelInteresCotizacion = new CNivelInteresCotizacion();
        NivelInteresCotizacion.LlenaObjeto(Cotizacion.IdNivelInteresCotizacion, pConexion);
        pModelo.Add(new JProperty("NivelInteresCotizacion", NivelInteresCotizacion.NivelInteresCotizacion));

        CDivision Division = new CDivision();
        Division.LlenaObjeto(Cotizacion.IdDivision, pConexion);
        pModelo.Add(new JProperty("Division", Division.Division));

        COportunidad OportunidadDescripcion = new COportunidad();
        OportunidadDescripcion.LlenaObjeto(Cotizacion.IdOportunidad, pConexion);
        pModelo.Add("Oportunidad", OportunidadDescripcion.Oportunidad);

        COportunidad Oportunidad = new COportunidad();
        Oportunidad.LlenaObjeto(Cotizacion.IdOportunidad, pConexion);

        if (Oportunidad.IdUsuarioCreacion == Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]))
        {
            pModelo.Add(new JProperty("selecionarOportunidad", 1));
        }
        else
        {
            pModelo.Add(new JProperty("selecionarOportunidad", 0));
        }

        CUsuario UC = new CUsuario();
        UC.LlenaObjeto(Cotizacion.IdUsuarioCotizador, pConexion);
        pModelo.Add(new JProperty("UsuarioCotizador", UC.Nombre + ' ' + UC.ApellidoPaterno + ' ' + UC.ApellidoMaterno));

        CUsuario Agente = new CUsuario();
        Agente.LlenaObjeto(Cotizacion.IdUsuarioAgente, pConexion);
        pModelo.Add(new JProperty("UsuarioAgente", Agente.Nombre + " " + Agente.ApellidoPaterno + " " + Agente.ApellidoMaterno));

        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        ContactoOrganizacion.LlenaObjeto(Cotizacion.IdContactoOrganizacion, pConexion);
        pModelo.Add("IdContactoOrganizacion", ContactoOrganizacion.IdContactoOrganizacion);
        pModelo.Add("ContactoOrganizacion", ContactoOrganizacion.Nombre);
        pModelo.Add("Puesto", ContactoOrganizacion.Puesto);

        CTelefonoContactoOrganizacion TelefonoContactoOrganizacion = new CTelefonoContactoOrganizacion();
        TelefonoContactoOrganizacion.LlenaObjeto(ContactoOrganizacion.IdContactoOrganizacion, pConexion);
        pModelo.Add("IdTelefonoContactoOrganizacion", TelefonoContactoOrganizacion.IdTelefonoContactoOrganizacion);
        pModelo.Add("Telefono", TelefonoContactoOrganizacion.Telefono);

        CCorreoContactoOrganizacion CorreoContactoOrganizacion = new CCorreoContactoOrganizacion();
        CorreoContactoOrganizacion.LlenaObjeto(ContactoOrganizacion.IdContactoOrganizacion, pConexion);
        pModelo.Add("IdCorreoContactoOrganizacion", CorreoContactoOrganizacion.IdCorreoContactoOrganizacion);
        pModelo.Add("Correo", CorreoContactoOrganizacion.Correo);

        CCampana Campana = new CCampana();
        Campana.LlenaObjeto(Cotizacion.IdCampana, pConexion);
        pModelo.Add("IdCampana", Campana.IdCampana);
        pModelo.Add("Campana", Campana.Campana);

        pModelo.Add("MotivoDeclinar", Cotizacion.MotivoDeclinar);
        pModelo.Add("IdUsuarioDeclinar", Cotizacion.IdUsuarioDeclinar);
        pModelo.Add("FechaDeclinar", Cotizacion.FechaDeclinar);

        return pModelo;
    }

    public void AgregarCotizacion(CConexion pConexion)
    {
        CConsultaAccion Agregar = new CConsultaAccion();
        Agregar.StoredProcedure.CommandText = "sp_Cotizacion_Agregar";
        Agregar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", 0);
        Agregar.StoredProcedure.Parameters["@pIdCotizacion"].Direction = ParameterDirection.Output;
        if (fechaAlta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaAlta", fechaAlta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pSubTotal", subTotal);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pNota", nota);
        if (validoHasta.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pValidoHasta", validoHasta);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCliente", idCliente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdContactoOrganizacion", idContactoOrganizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdTipoMoneda", idTipoMoneda);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdEstatusCotizacion", idEstatusCotizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioAgente", idUsuarioAgente);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioCotizador", idUsuarioCotizador);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pProyecto", proyecto);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdCampana", idCampana);
        if (fechaPedido.Year != 1)
        {
            Agregar.StoredProcedure.Parameters.AddWithValue("@pFechaPedido", fechaPedido);
        }
        Agregar.StoredProcedure.Parameters.AddWithValue("@pTipoCambio", tipoCambio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pAutorizacionIVA", autorizacionIVA);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pCantidadTotalLetra", cantidadTotalLetra);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdSucursalEjecutaServicio", idSucursalEjecutaServicio);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pOportunidad", oportunidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdDivision", idDivision);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdOportunidad", idOportunidad);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pIdNivelInteresCotizacion", IdNivelInteresCotizacion);
        Agregar.StoredProcedure.Parameters.AddWithValue("@pBaja", baja);
        Agregar.Insert(pConexion);
        idCotizacion = Convert.ToInt32(Agregar.StoredProcedure.Parameters["@pIdCotizacion"].Value);
    }

    public static JArray ObtenerPedidosCliente(int pIdCliente, CConexion pConexion)
    {
        CCotizacion Cotizacion = new CCotizacion();
        JArray JCotizaciones = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Baja", 0);
        ParametrosTI.Add("IdEstatusCotizacion", 3);
        ParametrosTI.Add("IdCliente", pIdCliente);

        foreach (CCotizacion oCotizacion in Cotizacion.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JCotizacion = new JObject();
            JCotizacion.Add("Valor", oCotizacion.IdCotizacion);
            JCotizacion.Add("Descripcion", oCotizacion.Folio);
            JCotizaciones.Add(JCotizacion);
        }
        return JCotizaciones;
    }

    public static JArray ObtenerPedidosClienteOrdenCompra(int pIdCliente, CConexion pConexion)
    {
        CSelectEspecifico Pedidos = new CSelectEspecifico();
        Pedidos.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosPedidosOrdenCompra";
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdCliente", pIdCliente);
        Pedidos.Llena(pConexion);

        JArray JCotizaciones = new JArray();
        while (Pedidos.Registros.Read())
        {
            JObject JPedido = new JObject();
            JPedido.Add("Valor", Convert.ToInt32(Pedidos.Registros["IdCotizacion"]));
            JPedido.Add("Descripcion", Convert.ToString(Pedidos.Registros["Folio"]));
            JCotizaciones.Add(JPedido);
        }
        Pedidos.Registros.Close();
        return JCotizaciones;
    }

    public static JArray ObtenerPedidosClienteRecepcion(int pIdCliente, CConexion pConexion)
    {
        CSelectEspecifico Pedidos = new CSelectEspecifico();
        Pedidos.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosPedidosRecepcion";
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdCliente", pIdCliente);
        Pedidos.Llena(pConexion);

        JArray JCotizaciones = new JArray();
        while (Pedidos.Registros.Read())
        {
            JObject JPedido = new JObject();
            JPedido.Add("Valor", Convert.ToInt32(Pedidos.Registros["IdCotizacion"]));
            JPedido.Add("Descripcion", Convert.ToString(Pedidos.Registros["Folio"]));
            JCotizaciones.Add(JPedido);
        }
        Pedidos.Registros.Close();
        return JCotizaciones;
    }

    public static JArray ObtenerPedidosClienteRecepcion(int pIdCliente, int pIdCotizacion, CConexion pConexion)
    {
        CSelectEspecifico Pedidos = new CSelectEspecifico();
        Pedidos.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosPedidosRecepcion";
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdCliente", pIdCliente);
        Pedidos.Llena(pConexion);

        JArray JCotizaciones = new JArray();
        while (Pedidos.Registros.Read())
        {
            JObject JPedido = new JObject();
            JPedido.Add("Valor", Convert.ToInt32(Pedidos.Registros["IdCotizacion"]));
            JPedido.Add("Descripcion", Convert.ToString(Pedidos.Registros["Folio"]));
            if (Convert.ToInt32(Pedidos.Registros["IdCotizacion"]) == pIdCotizacion)
            {
                JPedido.Add("Selected", 1);
            }
            else
            {
                JPedido.Add("Selected", 0);
            }
            JCotizaciones.Add(JPedido);
        }
        Pedidos.Registros.Close();
        return JCotizaciones;
    }

    public static JArray ObtenerPedidosClienteRemision(int pIdCliente, CConexion pConexion)
    {

        CSelectEspecifico Pedidos = new CSelectEspecifico();
        Pedidos.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosPedidosRemision";
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdCliente", pIdCliente);
        Pedidos.Llena(pConexion);

        JArray JCotizaciones = new JArray();
        while (Pedidos.Registros.Read())
        {
            JObject JPedido = new JObject();
            JPedido.Add("Valor", Convert.ToInt32(Pedidos.Registros["IdCotizacion"]));
            JPedido.Add("Descripcion", Convert.ToString(Pedidos.Registros["Folio"]));
            JCotizaciones.Add(JPedido);
        }
        Pedidos.Registros.Close();
        return JCotizaciones;
    }

    public static JArray ObtenerPedidosClienteFacturaConDocumentacion(int pIdCliente, int pIdTipoMonedaFactura, int pPorFiltroTipoMoneda, CConexion pConexion)
    {

        CSelectEspecifico Pedidos = new CSelectEspecifico();
        Pedidos.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosPedidosFactura";
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdCliente", pIdCliente);
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdTipoMonedaFactura", pIdTipoMonedaFactura);
        Pedidos.StoredProcedure.Parameters.AddWithValue("pPorFiltroTipoMoneda", pPorFiltroTipoMoneda);
        Pedidos.Llena(pConexion);

        JArray JCotizaciones = new JArray();
        while (Pedidos.Registros.Read())
        {
            JObject JPedido = new JObject();
            JPedido.Add("Valor", Convert.ToInt32(Pedidos.Registros["IdCotizacion"]));
            JPedido.Add("Descripcion", Convert.ToString(Pedidos.Registros["Folio"]));
            JCotizaciones.Add(JPedido);
        }
        Pedidos.Registros.Close();
        return JCotizaciones;
    }

    public static JArray ObtenerPedidosClienteFacturaSinDocumentacion(int pIdCliente, int pIdTipoMonedaFactura, int pPorFiltroTipoMoneda, CConexion pConexion)
    {

        CSelectEspecifico Pedidos = new CSelectEspecifico();
        Pedidos.StoredProcedure.CommandText = "sp_Cotizacion_ConsultarFiltrosPedidosFacturaSinDocumentacion";
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdCliente", pIdCliente);
        Pedidos.StoredProcedure.Parameters.AddWithValue("pIdTipoMonedaFactura", pIdTipoMonedaFactura);
        Pedidos.StoredProcedure.Parameters.AddWithValue("pPorFiltroTipoMoneda", pPorFiltroTipoMoneda);
        Pedidos.Llena(pConexion);

        JArray JCotizaciones = new JArray();
        while (Pedidos.Registros.Read())
        {
            JObject JPedido = new JObject();
            JPedido.Add("Valor", Convert.ToInt32(Pedidos.Registros["IdCotizacion"]));
            JPedido.Add("Descripcion", Convert.ToString(Pedidos.Registros["Folio"]));
            JCotizaciones.Add(JPedido);
        }
        Pedidos.Registros.Close();
        return JCotizaciones;
    }

    public void EditarTotales(CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Cotizacion_Actualizar_Total";
        Editar.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", idCotizacion);
        Editar.StoredProcedure.Parameters.AddWithValue("@pSubTotal", subTotal);
        Editar.StoredProcedure.Parameters.AddWithValue("@pTotal", total);
        Editar.StoredProcedure.Parameters.AddWithValue("@pIVA", iVA);
        Editar.Update(pConexion);
    }



    public static int ExisteCotizacionDetalle(int pIdCotizacion, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int ExisteCotizacionDetalle = 0; //No existe
        ObtenObjeto.StoredProcedure.CommandText = "sp_CotizacionDetalle_Consultar";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", Convert.ToInt32(pIdCotizacion));
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        ObtenObjeto.Llena<CCotizacionDetalle>(typeof(CCotizacionDetalle), pConexion);
        foreach (CCotizacionDetalle CotizacionDetalle in ObtenObjeto.ListaRegistros)
        {
            if (CotizacionDetalle.IdCotizacion != 0)
            {
                ExisteCotizacionDetalle = CotizacionDetalle.IdCotizacion; //Existe
                break;
            }
        }
        return ExisteCotizacionDetalle;
    }

    public static JArray ObtenerJsonPedidos(int pIdCliente, CConexion pConexion)
    {
        CCotizacion Cotizacion = new CCotizacion();
        Dictionary<string, object> ParametrosPedidos = new Dictionary<string, object>();
        ParametrosPedidos.Add("IdCliente", pIdCliente);
        ParametrosPedidos.Add("Baja", false);

        JArray JACotizaciones = new JArray();
        foreach (CCotizacion oCotizacion in Cotizacion.LlenaObjetosFiltros(ParametrosPedidos, pConexion))
        {
            JObject JCotizacion = new JObject();
            JCotizacion.Add("Valor", oCotizacion.IdCotizacion);
            JCotizacion.Add("Descripcion", oCotizacion.Folio);
            JACotizaciones.Add(JCotizacion);
        }
        return JACotizaciones;
    }

    public bool ValidarBaja(int pIdCotizacion, CConexion pConexion)
    {
        CSelect ObtenObjeto = new CSelect();
        int DocumentoLigado = 0;
        ObtenObjeto.StoredProcedure.CommandText = "sp_Cotizacion_Consultar";
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdCotizacion);
        ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        ObtenObjeto.Llena<CCotizacion>(typeof(CCotizacion), pConexion);
        foreach (CCotizacion Cotizacion in ObtenObjeto.ListaRegistros)
        {
            DocumentoLigado = 1;
        }
        if (DocumentoLigado == 0)
        { return false; }
        else
        { return true; }
    }

    public static JArray ObtenerPedidosCliente(int pIdCliente, int pIdPedido, CConexion pConexion)
    {
        CCotizacion Cotizacion = new CCotizacion();
        JArray JCotizaciones = new JArray();
        CSelectEspecifico Select = new CSelectEspecifico();
        Select.StoredProcedure.CommandText = "sp_Cotizacion_Consultar_ObtenerPedidosCliente";
        Select.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Select.StoredProcedure.Parameters.AddWithValue("@pIdCliente", pIdCliente);

        Select.Llena(pConexion);

        while (Select.Registros.Read())
        {
            JObject JCotizacion = new JObject();
            JCotizacion.Add("Valor", Convert.ToInt32(Select.Registros["IdCotizacion"]));
            JCotizacion.Add("Descripcion", Convert.ToString(Select.Registros["Folio"]));

            if (Convert.ToInt32(Select.Registros["IdCotizacion"]) == pIdPedido)
            {
                JCotizacion.Add(new JProperty("Selected", 1));
            }
            else
            {
                JCotizacion.Add(new JProperty("Selected", 0));
            }

            JCotizaciones.Add(JCotizacion);
        }
        return JCotizaciones;
    }

    public static JObject ObtenerJsonCotizacionTotales(JObject Modelo, int pIdCotizacion, int pIva, CConexion pConexion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();

        //Modelo = CCotizacionDetalle.ObtenerCotizacionDetalleTotales(Modelo, pIdCotizacion, pIva, pConexion);


        CSelectEspecifico Obten = new CSelectEspecifico();
        Obten.StoredProcedure.CommandText = "sp_CotizacionDetalle_ConsultarTipoIVA";
        Obten.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdCotizacion);
        Obten.StoredProcedure.Parameters.AddWithValue("@pIva", Convert.ToDecimal(pIva));
        Obten.StoredProcedure.Parameters.AddWithValue("@pBaja", 0);
        Obten.Llena(pConexion);
        if (Obten.Registros.Read())
        {
            Modelo.Add("Subtotal", Math.Round(Convert.ToDecimal(Obten.Registros["Subtotal"]), 2));
            Modelo.Add("Descuento", Math.Round(Convert.ToDecimal(Obten.Registros["Descuento"]), 2));
            Modelo.Add("SubtotalDescuento", Math.Round(Convert.ToDecimal(Obten.Registros["SubtotalDescuento"]), 2));
            Modelo.Add("Iva", Math.Round(Convert.ToDecimal(Obten.Registros["Iva"]), 2));
            Modelo.Add("Total", Math.Round(Convert.ToDecimal(Obten.Registros["Total"]), 2));
        }
        Obten.Registros.Close();
        Obten.Registros.Dispose();
        return Modelo;
    }

    public static JToken obtenerDatosImpresionCotizacion(string pIdCotizacion, int pUsuario)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCotizacion jsonImpresionC = new CCotizacion();
        jsonImpresionC.StoredProcedure.CommandText = "SP_Impresion_Cotizacion";
        jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdCotizacion);
        jsonImpresionC.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", pUsuario);
        return jsonImpresionC.ObtenerJsonJObject(ConexionBaseDatos);
    }

    public static void ActualizarFacturado(int pIdCotizacion, CConexion pConexion)
    {
        CConsultaAccion Editar = new CConsultaAccion();
        Editar.StoredProcedure.CommandText = "sp_Cotizacion_DetalleFacturado";
        Editar.StoredProcedure.CommandType = CommandType.StoredProcedure;
        Editar.StoredProcedure.Connection = pConexion.ConexionBaseDatosSqlServer;
        Editar.StoredProcedure.Parameters.AddWithValue("@pIdCotizacion", pIdCotizacion);
        Editar.Update(pConexion);
    }

}