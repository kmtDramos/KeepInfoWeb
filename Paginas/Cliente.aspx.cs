using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class Cliente : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeEliminarCliente;
        if (Usuario.TienePermisos(new string[] { "puedeEliminarCliente" }, ConexionBaseDatos) == "")
        {
            puedeEliminarCliente = 1;
        }
        else
        {
            puedeEliminarCliente = 0;
        }

        int puedeEliminarCuentaBancariaCliente;
        if (Usuario.TienePermisos(new string[] { "puedeEliminarCuentaBancariaCliente" }, ConexionBaseDatos) == "")
        {
            puedeEliminarCuentaBancariaCliente = 1;
        }
        else
        {
            puedeEliminarCuentaBancariaCliente = 0;
        }

        int puedeConsultarCuentaBancariaCliente;
        if (Usuario.TienePermisos(new string[] { "puedeConsultarCuentaBancariaCliente" }, ConexionBaseDatos) == "")
        {
            puedeConsultarCuentaBancariaCliente = 1;
        }
        else
        {
            puedeConsultarCuentaBancariaCliente = 0;
        }

        GeneraGridClientes(puedeEliminarCliente);
        GeneraGridDirecciones();
        GeneraGridContactos();
        GeneraGridDescuentos();
        GeneraGridCuentasBancarias(puedeEliminarCuentaBancariaCliente, puedeConsultarCuentaBancariaCliente);
        CActividad.GenerarGridActividadesClienteOportunidad("Actividades de cliente", this, ClientScript);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    private void GeneraGridClientes(int pPuedeEliminarCliente)
    {
        //GridCliente
        CJQGrid GridCliente = new CJQGrid();
        GridCliente.NombreTabla = "grdCliente";
        GridCliente.CampoIdentificador = "IdCliente";
        GridCliente.ColumnaOrdenacion = "Oportunidad";
        GridCliente.TipoOrdenacion = "DESC";
        GridCliente.Metodo = "ObtenerCliente";
        GridCliente.GenerarFuncionFiltro = false;
        GridCliente.GenerarFuncionTerminado = true;
        GridCliente.Altura = 400;
        GridCliente.TituloTabla = "Catálogo de clientes";
        GridCliente.NumeroRegistros = 30;
        GridCliente.RangoNumeroRegistros = "30,50,100";

        //IdCliente
        CJQColumn ColIdCliente = new CJQColumn();
        ColIdCliente.Nombre = "IdCliente";
        ColIdCliente.Oculto = "true";
        ColIdCliente.Encabezado = "IdCliente";
        ColIdCliente.Buscador = "false";
        GridCliente.Columnas.Add(ColIdCliente);

        //TipoCliente
        CJQColumn ColTipoCliente = new CJQColumn();
        ColTipoCliente.Nombre = "IdTipoCliente";
        ColTipoCliente.Encabezado = "Tipo Cliente";
        ColTipoCliente.Buscador = "true";
        ColTipoCliente.TipoBuscador = "Combo";
        ColTipoCliente.StoredProcedure.CommandText = "sp_Cliente_Consultar_TipoCliente";
        ColTipoCliente.Alineacion = "left";
        ColTipoCliente.Ancho = "60";
        GridCliente.Columnas.Add(ColTipoCliente);

        //Oportunidad
        CJQColumn ColOportunidad = new CJQColumn();
        ColOportunidad.Nombre = "Oportunidad";
        ColOportunidad.Encabezado = "Opt";
        ColOportunidad.Buscador = "false";
        ColOportunidad.Alineacion = "left";
        ColOportunidad.Ancho = "50";
        GridCliente.Columnas.Add(ColOportunidad);

        //Actividades
        CJQColumn ColActividades = new CJQColumn();
        ColActividades.Nombre = "Actividades";
        ColActividades.Encabezado = "Acts";
        ColActividades.Buscador = "false";
        ColActividades.Alineacion = "left";
        ColActividades.Estilo = "ver_actividades";
        ColActividades.Ancho = "50";
        GridCliente.Columnas.Add(ColActividades);

        //NombreComercial
        CJQColumn ColNombreComercial = new CJQColumn();
        ColNombreComercial.Nombre = "NombreComercial";
        ColNombreComercial.Encabezado = "Nombre comercial";
        ColNombreComercial.Buscador = "true";
        ColNombreComercial.Alineacion = "left";
        ColNombreComercial.Ancho = "250";
        GridCliente.Columnas.Add(ColNombreComercial);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "250";
        GridCliente.Columnas.Add(ColRazonSocial);

        //RFC
        CJQColumn ColRFC = new CJQColumn();
        ColRFC.Nombre = "RFC";
        ColRFC.Encabezado = "RFC";
        ColRFC.Buscador = "true";
        ColRFC.Alineacion = "left";
        ColRFC.Ancho = "100";
        GridCliente.Columnas.Add(ColRFC);


        //Agente
        CJQColumn ColAgente = new CJQColumn();
        ColAgente.Nombre = "Agente";
        ColAgente.Encabezado = "Agente";
        ColAgente.Ancho = "150";
        ColAgente.Alineacion = "Left";
        ColAgente.Buscador = "true";
        GridCliente.Columnas.Add(ColAgente);

		//TipoIndustria
		CJQColumn ColTipoIndustria = new CJQColumn();
		ColTipoIndustria.Nombre = "TipoIndustria";
		ColTipoIndustria.Encabezado = "Tipo Industria";
		ColTipoIndustria.TipoBuscador = "Combo";
		ColTipoIndustria.StoredProcedure.CommandText = "sp_Buscador_Cliente_TipoIndustria";
		GridCliente.Columnas.Add(ColTipoIndustria);

        //CuentasBancarias
        CJQColumn ColCuentasBancarias = new CJQColumn();
        ColCuentasBancarias.Nombre = "CuentaBancariaCliente";
        ColCuentasBancarias.Encabezado = "Cuentas bancarias";
        ColCuentasBancarias.Etiquetado = "Imagen";
        ColCuentasBancarias.Imagen = "cuentasBancarias2.png";
        ColCuentasBancarias.Estilo = "divImagenCuentaBancariaCliente imgFormaCuentaBancariaCliente";
        ColCuentasBancarias.Buscador = "false";
        ColCuentasBancarias.Ordenable = "false";
        ColCuentasBancarias.Ancho = "70";
        GridCliente.Columnas.Add(ColCuentasBancarias);

        //Direcciones
        CJQColumn ColConsultarDireccion = new CJQColumn();
        ColConsultarDireccion.Nombre = "Direcciones";
        ColConsultarDireccion.Encabezado = "Direcciones";
        ColConsultarDireccion.Etiquetado = "Imagen";
        ColConsultarDireccion.Imagen = "address.png";
        ColConsultarDireccion.Estilo = "divImagenDireccion imgFormaDirecciones";
        ColConsultarDireccion.Buscador = "false";
        ColConsultarDireccion.Ordenable = "false";
        ColConsultarDireccion.Ancho = "65";
        GridCliente.Columnas.Add(ColConsultarDireccion);

        //Contacto
        CJQColumn ColConsultarContacto = new CJQColumn();
        ColConsultarContacto.Nombre = "Contactos";
        ColConsultarContacto.Encabezado = "Contactos";
        ColConsultarContacto.Etiquetado = "Imagen";
        ColConsultarContacto.Imagen = "contacto.png";
        ColConsultarContacto.Estilo = "divImagenContacto imgFormaContactos";
        ColConsultarContacto.Buscador = "false";
        ColConsultarContacto.Ordenable = "false";
        ColConsultarContacto.Ancho = "55";
        GridCliente.Columnas.Add(ColConsultarContacto);

        //EsCliente
        CJQColumn ColEsCliente = new CJQColumn();
        ColEsCliente.Nombre = "EsCliente";
        ColEsCliente.Encabezado = "Cliente/Prospecto";
        ColEsCliente.Ordenable = "true";
        //ColEsCliente.Etiquetado = "A/I";
        ColEsCliente.Ancho = "55";
        ColEsCliente.Buscador = "true";
        ColEsCliente.TipoBuscador = "Combo";
        ColEsCliente.StoredProcedure.CommandText = "sp_Cliente_Consultar_EsCliente";
        GridCliente.Columnas.Add(ColEsCliente);


		//Baja
		CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.NombreBaja = "Baja";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "60";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBaja.Oculto = pPuedeEliminarCliente == 1 ? "false" : "true";
        GridCliente.Columnas.Add(ColBaja);

		// Polizas
		CJQColumn ColPolizas = new CJQColumn();
		ColPolizas.Nombre = "Polizas";
		ColPolizas.Encabezado = "Documentos";
		ColPolizas.Ordenable = "false";
		ColPolizas.Buscador = "false";
		ColPolizas.Imagen = "cotizado.png";
		ColPolizas.Ancho = "55";
		ColPolizas.Etiquetado = "Imagen";
		ColPolizas.Estilo = "divImagenPolizas imgFormaPolizas";
		GridCliente.Columnas.Add(ColPolizas);

		//Descuentos
		CJQColumn ColConsultarDescuento = new CJQColumn();
        ColConsultarDescuento.Nombre = "Descuento";
        ColConsultarDescuento.Encabezado = "Descuento";
        ColConsultarDescuento.Etiquetado = "Imagen";
        ColConsultarDescuento.Imagen = "descuento.png";
        ColConsultarDescuento.Estilo = "divImagenConsultar imgFormaConsultarDescuentoCliente";
        ColConsultarDescuento.Buscador = "false";
        ColConsultarDescuento.Ordenable = "false";
        ColConsultarDescuento.Ancho = "45";
        GridCliente.Columnas.Add(ColConsultarDescuento);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCliente";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCliente.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCliente", GridCliente.GeneraGrid(), true);
    }

	private void GeneraGridPolizasOportunidadesCliente() {

		//
		CJQGrid GridOportunidadesCliente = new CJQGrid();
		GridOportunidadesCliente.NombreTabla = "grdPolizasOportunidades";
		GridOportunidadesCliente.CampoIdentificador = "IdOportunidad";
		GridOportunidadesCliente.TipoOrdenacion = "DESC";
		GridOportunidadesCliente.Metodo = "ObtenerOportunidadesCliente";
		GridOportunidadesCliente.TituloTabla = "Oportunidades de cliente";
		GridOportunidadesCliente.GenerarFuncionFiltro = false;
		GridOportunidadesCliente.GenerarFuncionTerminado = false;
		GridOportunidadesCliente.Altura = 300;
		GridOportunidadesCliente.Ancho = 800;
		GridOportunidadesCliente.NumeroRegistros = 30;
		GridOportunidadesCliente.RangoNumeroRegistros = "30,50,100";

		//


		ClientScript.RegisterStartupScript(this.GetType(), "grdPolizasOportunidades", GridOportunidadesCliente.GeneraGrid(), true);

	}

    private void GeneraGridActividades() {
        CJQGrid GridActividades = new CJQGrid();
        GridActividades.NombreTabla = "grdActividades";
        GridActividades.CampoIdentificador = "IdActividad";
        GridActividades.ColumnaOrdenacion = "Fecha";
        GridActividades.TipoOrdenacion = "DESC";
        GridActividades.Metodo = "ObtenerActividadesCliente";
        GridActividades.TituloTabla = "Actividades de cliente";
        GridActividades.GenerarFuncionFiltro = false;
        GridActividades.GenerarFuncionTerminado = false;
        GridActividades.Altura = 300;
        GridActividades.Ancho = 600;
        GridActividades.NumeroRegistros = 30;
        GridActividades.RangoNumeroRegistros = "15,30,60";

        //IdActividad
        CJQColumn ColIdActividad = new CJQColumn();
        ColIdActividad.Nombre = "IdActividad";
        ColIdActividad.Oculto = "true";
        ColIdActividad.Encabezado = "IdActividad";
        ColIdActividad.Buscador = "false";
        GridActividades.Columnas.Add(ColIdActividad);

        //TipoActividad
        CJQColumn ColTipoActividad = new CJQColumn();
        ColTipoActividad.Nombre = "TipoActividad";
        ColTipoActividad.Encabezado = "Tipo de actividad";
        ColTipoActividad.Buscador = "true";
        ColTipoActividad.TipoBuscador = "Combo";
        ColTipoActividad.StoredProcedure.CommandText = "";
        ColTipoActividad.Alineacion = "left";
        ColTipoActividad.Ancho = "100";
        GridActividades.Columnas.Add(ColTipoActividad);

        ClientScript.RegisterStartupScript(this.GetType(), "grdActividades", GridActividades.GeneraGrid(), true);

    }

    private void GeneraGridDirecciones()
    {
        //GridDirecciones
        CJQGrid GridDirecciones = new CJQGrid();
        GridDirecciones.NombreTabla = "grdDirecciones";
        GridDirecciones.CampoIdentificador = "IdDireccionOrganizacion";
        GridDirecciones.ColumnaOrdenacion = "Calle";
        GridDirecciones.TipoOrdenacion = "DESC";
        GridDirecciones.Metodo = "ObtenerDirecciones";
        GridDirecciones.TituloTabla = "Direcciones de la organización";
        GridDirecciones.GenerarFuncionFiltro = false;
        GridDirecciones.GenerarFuncionTerminado = false;
        GridDirecciones.Altura = 300;
        GridDirecciones.Ancho = 600;
        GridDirecciones.NumeroRegistros = 15;
        GridDirecciones.RangoNumeroRegistros = "15,30,60";

        //IdDireccionOrganizacion
        CJQColumn ColIdDireccionOrganizacion = new CJQColumn();
        ColIdDireccionOrganizacion.Nombre = "IdDireccionOrganizacion";
        ColIdDireccionOrganizacion.Oculto = "true";
        ColIdDireccionOrganizacion.Encabezado = "IdServicio";
        ColIdDireccionOrganizacion.Buscador = "false";
        GridDirecciones.Columnas.Add(ColIdDireccionOrganizacion);

        //TipoDireccion
        CJQColumn ColTipoDireccion = new CJQColumn();
        ColTipoDireccion.Nombre = "TipoDireccion";
        ColTipoDireccion.Encabezado = "Tipo de dirección";
        ColTipoDireccion.Buscador = "false";
        ColTipoDireccion.Alineacion = "left";
        ColTipoDireccion.Ancho = "100";
        GridDirecciones.Columnas.Add(ColTipoDireccion);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "250";
        GridDirecciones.Columnas.Add(ColDescripcion);

        //Calle
        CJQColumn ColCalle = new CJQColumn();
        ColCalle.Nombre = "Calle";
        ColCalle.Encabezado = "Dirección";
        ColCalle.Buscador = "false";
        ColCalle.Alineacion = "left";
        ColCalle.Ancho = "250";
        GridDirecciones.Columnas.Add(ColCalle);

        //Baja
        CJQColumn ColBajaDireccion = new CJQColumn();
        ColBajaDireccion.Nombre = "AI";
        ColBajaDireccion.Encabezado = "A/I";
        ColBajaDireccion.Ordenable = "false";
        ColBajaDireccion.Etiquetado = "A/I";
        ColBajaDireccion.Ancho = "55";
        ColBajaDireccion.Buscador = "true";
        ColBajaDireccion.TipoBuscador = "Combo";
        ColBajaDireccion.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridDirecciones.Columnas.Add(ColBajaDireccion);

        //Consultar
        CJQColumn ColConsultarDireccionOrganizacion = new CJQColumn();
        ColConsultarDireccionOrganizacion.Nombre = "Consultar";
        ColConsultarDireccionOrganizacion.Encabezado = "Ver";
        ColConsultarDireccionOrganizacion.Etiquetado = "ImagenConsultar";
        ColConsultarDireccionOrganizacion.Estilo = "divImagenConsultar imgFormaConsultarDireccion";
        ColConsultarDireccionOrganizacion.Buscador = "false";
        ColConsultarDireccionOrganizacion.Ordenable = "false";
        ColConsultarDireccionOrganizacion.Ancho = "25";
        GridDirecciones.Columnas.Add(ColConsultarDireccionOrganizacion);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDirecciones", GridDirecciones.GeneraGrid(), true);
    }

    private void GeneraGridContactos()
    {
        //GridContactoOrganizacion
        CJQGrid GridContactoOrganizacion = new CJQGrid();
        GridContactoOrganizacion.NombreTabla = "grdContactoOrganizacion";
        GridContactoOrganizacion.CampoIdentificador = "IdContactoOrganizacion";
        GridContactoOrganizacion.ColumnaOrdenacion = "Nombre";
        GridContactoOrganizacion.TipoOrdenacion = "DESC";
        GridContactoOrganizacion.Metodo = "ObtenerContactoOrganizacion";
        GridContactoOrganizacion.TituloTabla = "Contactos de la organización";
        GridContactoOrganizacion.GenerarFuncionFiltro = false;
        GridContactoOrganizacion.GenerarFuncionTerminado = false;
        GridContactoOrganizacion.Altura = 300;
        GridContactoOrganizacion.Ancho = 600;
        GridContactoOrganizacion.NumeroRegistros = 15;
        GridContactoOrganizacion.RangoNumeroRegistros = "15,30,60";

        //IdDireccionOrganizacion
        CJQColumn ColIdContactoOrganizacion = new CJQColumn();
        ColIdContactoOrganizacion.Nombre = "IdContactoOrganizacion";
        ColIdContactoOrganizacion.Oculto = "true";
        ColIdContactoOrganizacion.Encabezado = "IdContactoOrganizacion";
        ColIdContactoOrganizacion.Buscador = "false";
        GridContactoOrganizacion.Columnas.Add(ColIdContactoOrganizacion);

        //Nombre
        CJQColumn ColNombre = new CJQColumn();
        ColNombre.Nombre = "Nombre";
        ColNombre.Encabezado = "Nombre";
        ColNombre.Buscador = "false";
        ColNombre.Alineacion = "left";
        ColNombre.Ancho = "250";
        GridContactoOrganizacion.Columnas.Add(ColNombre);

        //EsCliente
        //CJQColumn ColEsCliente = new CJQColumn();
        //ColEsCliente.Nombre = "EsCliente";
        //ColEsCliente.Encabezado = "Es Cliente";
        //ColEsCliente.Ordenable = "true";
        //ColEsCliente.Etiquetado = "Es Cliente";
        //ColEsCliente.Ancho = "55";
        //ColEsCliente.Buscador = "true";
        //ColEsCliente.TipoBuscador = "Combo";
        //ColEsCliente.StoredProcedure.CommandText = "sp_Cliente_Consultar_EsCliente";
        //GridContactoOrganizacion.Columnas.Add(ColEsCliente);

        //Baja
        CJQColumn ColBajaContacto = new CJQColumn();
        ColBajaContacto.Nombre = "AI";
        ColBajaContacto.Encabezado = "A/I";
        ColBajaContacto.Ordenable = "false";
        ColBajaContacto.Etiquetado = "A/I";
        ColBajaContacto.Ancho = "50";
        ColBajaContacto.Buscador = "true";
        ColBajaContacto.TipoBuscador = "Combo";
        ColBajaContacto.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridContactoOrganizacion.Columnas.Add(ColBajaContacto);

        //Consultar
        CJQColumn ColConsultarContactoOrganizacion = new CJQColumn();
        ColConsultarContactoOrganizacion.Nombre = "Consultar";
        ColConsultarContactoOrganizacion.Encabezado = "Ver";
        ColConsultarContactoOrganizacion.Etiquetado = "ImagenConsultar";
        ColConsultarContactoOrganizacion.Estilo = "divImagenConsultar imgFormaConsultarContacto";
        ColConsultarContactoOrganizacion.Buscador = "false";
        ColConsultarContactoOrganizacion.Ordenable = "false";
        ColConsultarContactoOrganizacion.Ancho = "25";
        GridContactoOrganizacion.Columnas.Add(ColConsultarContactoOrganizacion);

        ClientScript.RegisterStartupScript(this.GetType(), "grdContactoOrganizacion", GridContactoOrganizacion.GeneraGrid(), true);
    }

    private void GeneraGridDescuentos()
    {
        //GridDescuento
        CJQGrid GridDescuentoCliente = new CJQGrid();
        GridDescuentoCliente.NombreTabla = "grdDescuentoCliente";
        GridDescuentoCliente.CampoIdentificador = "IdDescuentoCliente";
        GridDescuentoCliente.ColumnaOrdenacion = "Descripcion";
        GridDescuentoCliente.TipoOrdenacion = "DESC";
        GridDescuentoCliente.Metodo = "ObtenerDescuentoCliente";
        GridDescuentoCliente.TituloTabla = "Descuentos del cliente";
        GridDescuentoCliente.GenerarFuncionFiltro = false;
        GridDescuentoCliente.GenerarFuncionTerminado = false;
        GridDescuentoCliente.Altura = 300;
        GridDescuentoCliente.Ancho = 600;
        GridDescuentoCliente.NumeroRegistros = 15;
        GridDescuentoCliente.RangoNumeroRegistros = "15,30,60";

        //IdDescuentoCliente
        CJQColumn ColIdDescuentoCliente = new CJQColumn();
        ColIdDescuentoCliente.Nombre = "IdDescuentoCliente";
        ColIdDescuentoCliente.Oculto = "true";
        ColIdDescuentoCliente.Encabezado = "IdCliente";
        ColIdDescuentoCliente.Buscador = "false";
        GridDescuentoCliente.Columnas.Add(ColIdDescuentoCliente);

        //Producto
        CJQColumn ColClienteDelDescuento = new CJQColumn();
        ColClienteDelDescuento.Nombre = "NombreComercial";
        ColClienteDelDescuento.Encabezado = "Cliente";
        ColClienteDelDescuento.Buscador = "false";
        ColClienteDelDescuento.Alineacion = "left";
        ColClienteDelDescuento.Ancho = "90";
        GridDescuentoCliente.Columnas.Add(ColClienteDelDescuento);

        //Descuento del Producto
        CJQColumn ColDescuentoCliente = new CJQColumn();
        ColDescuentoCliente.Nombre = "Descripcion";
        ColDescuentoCliente.Encabezado = "Descripción";
        ColDescuentoCliente.Buscador = "false";
        ColDescuentoCliente.Alineacion = "left";
        ColDescuentoCliente.Ancho = "190";
        GridDescuentoCliente.Columnas.Add(ColDescuentoCliente);

        //Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "DescuentoCliente";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Buscador = "false";
        ColDescuento.Ancho = "80";
        ColDescuento.Alineacion = "right";
        GridDescuentoCliente.Columnas.Add(ColDescuento);

        //Baja
        CJQColumn ColBajaDescuento = new CJQColumn();
        ColBajaDescuento.Nombre = "AI_DescuentoCliente";
        ColBajaDescuento.Encabezado = "A/I";
        ColBajaDescuento.Ordenable = "false";
        ColBajaDescuento.Etiquetado = "A/I";
        ColBajaDescuento.Ancho = "55";
        ColBajaDescuento.Buscador = "true";
        ColBajaDescuento.TipoBuscador = "Combo";
        ColBajaDescuento.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridDescuentoCliente.Columnas.Add(ColBajaDescuento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDescuentoCliente", GridDescuentoCliente.GeneraGrid(), true);
    }

    private void GeneraGridOportunidadesCliente() {

        //GridDescuento
        CJQGrid GridOportunidadesCliente = new CJQGrid();
        GridOportunidadesCliente.NombreTabla = "grdOportunidadCliente";
        GridOportunidadesCliente.CampoIdentificador = "IdOportunidad";
        GridOportunidadesCliente.ColumnaOrdenacion = "IdOportunidad";
        GridOportunidadesCliente.TipoOrdenacion = "DESC";
        GridOportunidadesCliente.Metodo = "ObtenerOportunidadesCliente";
        GridOportunidadesCliente.TituloTabla = "Oportunidades de cliente";
        GridOportunidadesCliente.Altura = 300;
        GridOportunidadesCliente.Ancho = 600;
        GridOportunidadesCliente.NumeroRegistros = 15;
        GridOportunidadesCliente.RangoNumeroRegistros = "15,30,60";

        // IdOportunidad
        CJQColumn ColIdOportunidad = new CJQColumn();
        ColIdOportunidad.Nombre = "IdOportunidad";
        ColIdOportunidad.Encabezado = "#";
        ColIdOportunidad.Buscador = "false";
        ColIdOportunidad.Ancho = "80px";
        GridOportunidadesCliente.Columnas.Add(ColIdOportunidad);

        // Oportunidad
        CJQColumn ColOportunidad = new CJQColumn();
        ColOportunidad.Nombre = "Oportunidaad";
        ColOportunidad.Encabezado = "Oportunidad";
        ColOportunidad.Buscador = "false";
        ColOportunidad.Ancho = "200px";
        GridOportunidadesCliente.Columnas.Add(ColOportunidad);

        // Agente
        CJQColumn ColAgente = new CJQColumn();
        ColAgente.Nombre = "Agente";
        ColAgente.Encabezado = "Agente";
        ColAgente.Buscador = "false";
        ColAgente.Ancho = "200px";
        GridOportunidadesCliente.Columnas.Add(ColAgente);

        // Dias
        CJQColumn ColDias = new CJQColumn();
        ColDias.Nombre = "Dias";
        ColDias.Encabezado = "Dias";
        ColDias.Buscador = "false";
        ColDias.Ancho = "80px";
        GridOportunidadesCliente.Columnas.Add(ColDias);

        // Monto
        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Buscador = "false";
        ColMonto.Ancho = "120px";
        GridOportunidadesCliente.Columnas.Add(ColMonto);

        ClientScript.RegisterStartupScript(this.GetType(), "grdOportunidadCliente", GridOportunidadesCliente.GeneraGrid(), true);
    }

    private void GeneraGridCuentasBancarias(int pPuedeEliminarCuentaBancariaCliente, int pPuedeConsultarCuentaBancariaCliente)
    {
        //GridCuentaBancariaCliente
        CJQGrid GridCuentaBancariaCliente = new CJQGrid();
        GridCuentaBancariaCliente.NombreTabla = "grdCuentaBancariaCliente";
        GridCuentaBancariaCliente.CampoIdentificador = "IdCuentaBancariaCliente";
        GridCuentaBancariaCliente.ColumnaOrdenacion = "Banco";
        GridCuentaBancariaCliente.TipoOrdenacion = "DESC";
        GridCuentaBancariaCliente.Metodo = "ObtenerCuentaBancariaCliente";
        GridCuentaBancariaCliente.TituloTabla = "Cuentas bancarias del cliente";
        GridCuentaBancariaCliente.GenerarFuncionFiltro = false;
        GridCuentaBancariaCliente.GenerarFuncionTerminado = false;
        GridCuentaBancariaCliente.GenerarGridCargaInicial = false;
        GridCuentaBancariaCliente.Altura = 300;
        GridCuentaBancariaCliente.Ancho = 800;
        GridCuentaBancariaCliente.NumeroRegistros = 10;
        GridCuentaBancariaCliente.RangoNumeroRegistros = "10,20,30";

        //IdCuentaBancaria
        CJQColumn ColIdCuentaBancariaCliente = new CJQColumn();
        ColIdCuentaBancariaCliente.Nombre = "IdCuentaBancariaCliente";
        ColIdCuentaBancariaCliente.Oculto = "true";
        ColIdCuentaBancariaCliente.Encabezado = "IdCuentaBancariaCliente";
        ColIdCuentaBancariaCliente.Buscador = "false";
        GridCuentaBancariaCliente.Columnas.Add(ColIdCuentaBancariaCliente);

        //Banco
        CJQColumn ColBanco = new CJQColumn();
        ColBanco.Nombre = "Banco";
        ColBanco.Encabezado = "Banco";
        ColBanco.Buscador = "false";
        ColBanco.Ancho = "150";
        ColBanco.Alineacion = "left";
        GridCuentaBancariaCliente.Columnas.Add(ColBanco);

        //CuentaBancariaCliente
        CJQColumn ColCuentaBancariaCliente = new CJQColumn();
        ColCuentaBancariaCliente.Nombre = "CuentaBancariaCliente";
        ColCuentaBancariaCliente.Encabezado = "Cuenta Bancaria";
        ColCuentaBancariaCliente.Buscador = "false";
        ColCuentaBancariaCliente.Ancho = "150";
        ColCuentaBancariaCliente.Alineacion = "left";
        GridCuentaBancariaCliente.Columnas.Add(ColCuentaBancariaCliente);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Ancho = "150";
        ColDescripcion.Alineacion = "left";
        GridCuentaBancariaCliente.Columnas.Add(ColDescripcion);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Ancho = "80";
        ColTipoMoneda.Alineacion = "left";
        GridCuentaBancariaCliente.Columnas.Add(ColTipoMoneda);

        //Metodo de pago
        CJQColumn ColMetodoPago = new CJQColumn();
        ColMetodoPago.Nombre = "MetodoPado";
        ColMetodoPago.Encabezado = "Método de pago";
        ColMetodoPago.Buscador = "false";
        ColMetodoPago.Ancho = "80";
        ColMetodoPago.Alineacion = "left";
        GridCuentaBancariaCliente.Columnas.Add(ColMetodoPago);

        //Baja
        CJQColumn ColBajaCuentaBancariaCliente = new CJQColumn();
        ColBajaCuentaBancariaCliente.Nombre = "AI";
        ColBajaCuentaBancariaCliente.Encabezado = "A/I";
        ColBajaCuentaBancariaCliente.Ordenable = "false";
        ColBajaCuentaBancariaCliente.Etiquetado = "A/I";
        ColBajaCuentaBancariaCliente.Ancho = "55";
        ColBajaCuentaBancariaCliente.Buscador = "true";
        ColBajaCuentaBancariaCliente.TipoBuscador = "Combo";
        ColBajaCuentaBancariaCliente.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBajaCuentaBancariaCliente.Oculto = pPuedeEliminarCuentaBancariaCliente == 1 ? "false" : "true";
        GridCuentaBancariaCliente.Columnas.Add(ColBajaCuentaBancariaCliente);

        //Consultar
        CJQColumn ColConsultarCuentaBancariaCliente = new CJQColumn();
        ColConsultarCuentaBancariaCliente.Nombre = "Consultar";
        ColConsultarCuentaBancariaCliente.Encabezado = "Ver";
        ColConsultarCuentaBancariaCliente.Etiquetado = "ImagenConsultar";
        ColConsultarCuentaBancariaCliente.Estilo = "divImagenConsultar imgFormaConsultarCuentaBancariaCliente";
        ColConsultarCuentaBancariaCliente.Buscador = "false";
        ColConsultarCuentaBancariaCliente.Ordenable = "false";
        ColConsultarCuentaBancariaCliente.Ancho = "25";
        ColConsultarCuentaBancariaCliente.Oculto = pPuedeConsultarCuentaBancariaCliente == 1 ? "false" : "true";
        GridCuentaBancariaCliente.Columnas.Add(ColConsultarCuentaBancariaCliente);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCuentaBancariaCliente", GridCuentaBancariaCliente.GeneraGrid(), true);
    }
    
    [WebMethod]
    public static string BuscarAgente(string pAgente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_ConsultarFiltros_ClienteAgente";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pAgente", pAgente);
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pNombreComercial, string pRFC, int pEsCliente, string pAgente, int pIdTipoCliente, int pIdTipoIndustria, int pAI, int pVerTodos)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pNombreComercial", SqlDbType.VarChar, 255).Value = pNombreComercial;
        Stored.Parameters.Add("pRFC", SqlDbType.VarChar, 255).Value = pRFC;
        Stored.Parameters.Add("pEsCliente", SqlDbType.Int).Value = pEsCliente;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pAgente", SqlDbType.VarChar, 255).Value = pAgente;
		Stored.Parameters.Add("pIdTipoCliente", SqlDbType.Int).Value = pIdTipoCliente;
		Stored.Parameters.Add("pIdTipoIndustria", SqlDbType.Int).Value = pIdTipoIndustria;
		Stored.Parameters.Add("pVerTodos", SqlDbType.Int).Value = pVerTodos;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFormaOportunidadesCliente(int IdCliente) {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
            if (Error == 0) {
                JObject Modelo = new JObject();
                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(IdCliente, pConexion);
                Modelo.Add("IdCliente", Cliente.IdCliente);
                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", Descripcion);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerClientesOportunidades(string pRazonSocial, string pNombreComercial, string pRFC, int pEsCliente, string pAgente, int pIdTipoCliente, int pAI){
        
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        
        JObject oRespuesta = new JObject();
        
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            CSelectEspecifico SelectEspecifico = new CSelectEspecifico();
            SelectEspecifico.StoredProcedure.CommandText = "sp_ClientesOportunidades";
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pRazonSocial", pRazonSocial);
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pNombreComercial", pNombreComercial);
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pRFC", pRFC);
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pBaja", pAI);
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pEsCliente", pEsCliente);
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pAgente", pAgente);
            SelectEspecifico.StoredProcedure.Parameters.AddWithValue("pIdTipoCliente", pIdTipoCliente);

            SelectEspecifico.Llena(ConexionBaseDatos);

            int ConOportunidades = 0;
            int SinOportunidades = 0;

            if (SelectEspecifico.Registros.Read())
            {
                ConOportunidades = Convert.ToInt32(SelectEspecifico.Registros["ConOportunidades"]);
                SinOportunidades = Convert.ToInt32(SelectEspecifico.Registros["SinOportunidades"]);
            }

            Modelo.Add("ConOportunidades", ConOportunidades);
            Modelo.Add("SinOportunidades", SinOportunidades);

            oRespuesta.Add("Error", 0);
            oRespuesta.Add("Modelo", Modelo);
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDirecciones(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCliente, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDireccionOrganizacion_Cliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerContactoOrganizacion(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCliente, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdContactoOrganizacion_Cliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCuentaBancariaCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCliente, int pIdBanco, int pIdTipoMoneda, int pAI)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentaBancariaCliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("IdBanco", SqlDbType.Int).Value = pIdBanco;
        Stored.Parameters.Add("IdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;
        Stored.Parameters.Add("Baja", SqlDbType.Int).Value = pAI;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDescuentoCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCliente, int pBaja)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDescuentoCliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pBaja;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }
    
    [WebMethod]
    public static string BuscarNombreComercial(string pNombreComercial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonNombreComercial = new CJson();
        jsonNombreComercial.StoredProcedure.CommandText = "sp_Cliente_Consultar_FiltroPorNombreComercial";
        jsonNombreComercial.StoredProcedure.Parameters.AddWithValue("@pNombreComercial", pNombreComercial);
        string jsonNombreComercialString = jsonNombreComercial.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonNombreComercialString;
    }

    [WebMethod]
    public static string BuscarRazonSocial(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonRazonSocial = new CJson();
        jsonRazonSocial.StoredProcedure.CommandText = "sp_Cliente_Consultar_FiltroPorRazonSocialGrid";
        jsonRazonSocial.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        string jsonRazonSocialString = jsonRazonSocial.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonRazonSocialString;
    }

    [WebMethod]
    public static string BuscarRFC(string pRFC)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonRFC = new CJson();
        jsonRFC.StoredProcedure.CommandText = "sp_Cliente_Consultar_FiltroPorRFC";
        jsonRFC.StoredProcedure.Parameters.AddWithValue("@pRFC", pRFC);
        string jsonRFCString = jsonRFC.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonRFCString;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerOportunidadesCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCliente, string pIdOportunidad, string pAgente) {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCliente_Oportunidades", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        Stored.Parameters.Add("pIdOportunidad", SqlDbType.VarChar,11).Value = pIdCliente;
        Stored.Parameters.Add("pAgente", SqlDbType.VarChar,255).Value = pIdCliente;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

	[WebMethod]
	public static string ObtenerPolizasCliente(int IdCliente) {
		JObject Respuesta = new JObject();
		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0) {
				JObject Modelo = new JObject();
				CCliente Cliente = new CCliente();
				Cliente.LlenaObjeto(IdCliente, pConexion);
				COrganizacion Organizacion = new COrganizacion();
				Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

				decimal Facturas = MontoFacturasCliente(Cliente.IdCliente, pConexion);
				decimal Ingresos = MontoIngresosCliente(Cliente.IdCliente, pConexion);
				decimal OportunidadAlta = MontoOportunidadesAlta(Cliente.IdCliente, pConexion);
				decimal OportunidadMedia = MontoOportunidadesMedia(Cliente.IdCliente, pConexion);
				decimal OportunidadBaja = MontoOportunidadesBaja(Cliente.IdCliente, pConexion);

				Modelo.Add("IdCliente", Cliente.IdCliente);
				Modelo.Add("Cliente", Organizacion.RazonSocial);
				Modelo.Add("Facturas", Facturas.ToString("C"));
				Modelo.Add("Ingresos", Ingresos.ToString("C"));
				Modelo.Add("Saldo", (Facturas - Ingresos).ToString("C"));
				

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("Descripcion", DescripcionError);
		});
		return Respuesta.ToString();
	}

	private static decimal MontoFacturasCliente(int IdCliente, CConexion pConexion) {
		decimal Monto = 0;

		CFacturaEncabezado Facturas = new CFacturaEncabezado();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdCliente", IdCliente);
		pParametros.Add("Baja", 0);

		foreach (CFacturaEncabezado Factura in Facturas.LlenaObjetosFiltros(pParametros, pConexion)) {
			Monto += Factura.Total * Factura.TipoCambio;
		}

		return Monto;
	}

	private static decimal MontoIngresosCliente(int IdCliente, CConexion pConexion) {
		decimal Monto = 0;

		CCuentasPorCobrar Ingresos = new CCuentasPorCobrar();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdCliente", IdCliente);
		pParametros.Add("Baja", 0);

		foreach (CCuentasPorCobrar Ingreso in Ingresos.LlenaObjetosFiltros(pParametros, pConexion))
		{
			Monto += Ingreso.Importe * Ingreso.TipoCambio;
		}

		return Monto;
	}

	private static decimal MontoOportunidadesAlta(int IdCliente, CConexion pConexion) {
		decimal Monto = 0;

		COportunidad Oportunidades = new COportunidad();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdCliente", IdCliente);
		pParametros.Add("IdNivelInteresOportunidad", 1);
		pParametros.Add("Baja", 0);
		pParametros.Add("Cerrado", 0);

		foreach (COportunidad Oportunidad in Oportunidades.LlenaObjetosFiltros(pParametros, pConexion)) {
			Monto += Oportunidad.Monto;
		}

		return Monto;
	}

	private static decimal MontoOportunidadesMedia(int IdCliente, CConexion pConexion) {
		decimal Monto = 0;

		COportunidad Oportunidades = new COportunidad();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdCliente", IdCliente);
		pParametros.Add("IdNivelInteresOportunidad", 2);
		pParametros.Add("Baja", 0);
		pParametros.Add("Cerrado", 0);

		foreach (COportunidad Oportunidad in Oportunidades.LlenaObjetosFiltros(pParametros, pConexion)) {
			Monto += Oportunidad.Monto;
		}

		return Monto;
	}

	private static decimal MontoOportunidadesBaja(int IdCliente, CConexion pConexion) {
		decimal Monto = 0;

		COportunidad Oportunidades = new COportunidad();
		Dictionary<string, object> pParametros = new Dictionary<string, object>();
		pParametros.Add("IdCliente", IdCliente);
		pParametros.Add("IdNivelInteresOportunidad", 3);
		pParametros.Add("Baja", 0);
		pParametros.Add("Cerrado", 0);

		foreach (COportunidad Oportunidad in Oportunidades.LlenaObjetosFiltros(pParametros, pConexion)) {
			Monto += Oportunidad.Monto;
		}

		return Monto;
	}

	[WebMethod]
    public static string ObtenerFormaAgregarCliente(int pIdOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();
        CCliente Cliente = new CCliente();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeAgregarCliente;
        if (Usuario.TienePermisos(new string[] { "puedeAgregarCliente" }, ConexionBaseDatos) == "") { puedeAgregarCliente = 1; }
        else { puedeAgregarCliente = 0; }
        oPermisos.Add("puedeAgregarCliente", puedeAgregarCliente);

        int puedeInsertarCaracteresRaros;
        if (Usuario.TienePermisos(new string[] { "puedeInsertarCaracteresRaros" }, ConexionBaseDatos) == "") { puedeInsertarCaracteresRaros = 1; }
        else { puedeInsertarCaracteresRaros = 0; }
        oPermisos.Add("puedeInsertarCaracteresRaros", puedeInsertarCaracteresRaros);

        int puedeAgregarLimiteCredito;
        if (Usuario.TienePermisos(new string[] { "puedeAgregarLimiteCredito" }, ConexionBaseDatos) == "") { puedeAgregarLimiteCredito = 1; }
        else { puedeAgregarLimiteCredito = 0; }
        oPermisos.Add("puedeAgregarLimiteCredito", puedeAgregarLimiteCredito);

        int puedeEditarDatosOrganizacion;
        if (Usuario.TienePermisos(new string[] { "puedeEditarDatosOrganizacion" }, ConexionBaseDatos) == "") { puedeEditarDatosOrganizacion = 1; }
        else { puedeEditarDatosOrganizacion = 0; }
        oPermisos.Add("puedeEditarDatosOrganizacion", puedeEditarDatosOrganizacion);

        int puedeEditarDatosFiscales;
        if (Usuario.TienePermisos(new string[] { "puedeEditarDatosFiscales" }, ConexionBaseDatos) == "") { puedeEditarDatosFiscales = 1; }
        else { puedeEditarDatosFiscales = 0; }
        oPermisos.Add("puedeEditarDatosFiscales", puedeEditarDatosFiscales);

        int puedeAsignarAgenteCliente;
        if (Usuario.TienePermisos(new string[] { "puedeAsignarAgenteCliente" }, ConexionBaseDatos) == "") { puedeAsignarAgenteCliente = 1; }
        else { puedeAsignarAgenteCliente = 0; }
        oPermisos.Add("puedeAsignarAgenteCliente", puedeAsignarAgenteCliente);

        bool puedeAgregarAsignacionCuentaContableCliente = false;
        if (Usuario.TienePermisos(new string[] { "puedeAgregarAsignacionCuentaContableCliente" }, ConexionBaseDatos) != "")
        {
            puedeAgregarAsignacionCuentaContableCliente = true;
        }
        oPermisos.Add("puedeAgregarAsignacionCuentaContableCliente", puedeAgregarAsignacionCuentaContableCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add("FormaContactos", CJson.ObtenerJsonFormaContactos(ConexionBaseDatos));
            Modelo.Add("TipoClientes", CJson.ObtenerJsonTipoClientes(ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(1, ConexionBaseDatos));
            Modelo.Add("TipoGarantias", CTipoGarantia.ObtenerJsonTipoGarantia(ConexionBaseDatos));
            Modelo.Add("Campanas", CCampana.ObtenerJsonCampana(true, ConexionBaseDatos));
            if (Usuario.EsAgente)
            {
                Modelo.Add("UsuarioAgentes", CUsuario.ObtenerJsonUsuarioAgente(Usuario.IdUsuario, 0, ConexionBaseDatos));
            }
            else
            {
                Modelo.Add("UsuarioAgentes", CUsuario.ObtenerJsonUsuarioAgenteTodos(Usuario.IdUsuario, ConexionBaseDatos));
            }

            if (pIdOrganizacion == 0)
            {
                Modelo.Add("TipoIndustrias", CJson.ObtenerJsonTipoIndustria(ConexionBaseDatos));
                Modelo.Add("Paises", CJson.ObtenerJsonPaises(ConexionBaseDatos));
                Modelo.Add("GrupoEmpresariales", CJson.ObtenerJsonGrupoEmpresariales(ConexionBaseDatos));
                Modelo.Add("SegmentoMercados", CSegmentoMercado.ObtenerJsonSegmentoMercado(ConexionBaseDatos));
            }
            else
            {

                Modelo = COrganizacion.ObtenerJsonOrganizacion(Modelo, pIdOrganizacion, ConexionBaseDatos);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(pIdOrganizacion, ConexionBaseDatos);

                Dictionary<string, object> ParametrosSucursalAlta = new Dictionary<string, object>();
                ParametrosSucursalAlta.Add("IdUsuario", Organizacion.IdUsuarioAlta);
                Usuario.LlenaObjetoFiltros(ParametrosSucursalAlta, ConexionBaseDatos);
                Modelo.Add("IdSucursalAlta", Usuario.IdSucursalActual);
                Modelo.Add("IdSucursalActual", Usuario.IdSucursalActual);
                Modelo.Add("IdOrganizacion", pIdOrganizacion);
            }

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaClienteAEnrolar(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonCliente(Modelo, pIdCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerDireccionFiscalCliente(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(pIdCliente, ConexionBaseDatos);

            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            Dictionary<string, object> ParametrosDireccionOrganizacion = new Dictionary<string, object>();
            ParametrosDireccionOrganizacion.Add("IdOrganizacion", Cliente.IdOrganizacion);
            ParametrosDireccionOrganizacion.Add("IdTipoDireccion", 1);
            DireccionOrganizacion.LlenaObjetoFiltros(ParametrosDireccionOrganizacion, ConexionBaseDatos);

            Modelo = CJson.ObtenerJsonDireccionOrganizacion(Modelo, DireccionOrganizacion.IdDireccionOrganizacion, ConexionBaseDatos);
            Modelo.Add("TipoDirecciones", CJson.ObtenerJsonTipoDireccion(Convert.ToInt32(Modelo["IdTipoDireccion"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaProveedorAEnrolar(int pIdOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCliente = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CProveedor Proveedor = new CProveedor();
            Dictionary<string, object> ParametrosProveedor = new Dictionary<string, object>();
            ParametrosProveedor.Add("IdOrganizacion", pIdOrganizacion);
            Proveedor.LlenaObjetoFiltros(ParametrosProveedor, ConexionBaseDatos);

            Modelo = CJson.ObtenerJsonProveedor(Modelo, Proveedor.IdProveedor, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarCliente(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeConsultarCliente;
        if (Usuario.TienePermisos(new string[] { "puedeConsultarCliente" }, ConexionBaseDatos) == "") { puedeConsultarCliente = 1; }
        else { puedeConsultarCliente = 0; }
        oPermisos.Add("puedeConsultarCliente", puedeConsultarCliente);

        int puedeEditarCliente;
        if (Usuario.TienePermisos(new string[] { "puedeEditarCliente" }, ConexionBaseDatos) == "") { puedeEditarCliente = 1; }
        else { puedeEditarCliente = 0; }
        oPermisos.Add("puedeEditarCliente", puedeEditarCliente);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonCliente(Modelo, pIdCliente, ConexionBaseDatos);
            if (Sucursal.IdEmpresa != Convert.ToInt32(Modelo["IdEmpresaAlta"].ToString()))
            {
                oPermisos.Add("diferenteSucursal", 0);
            }
            else
            {
                oPermisos.Add("diferenteSucursal", 0);
            }
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarCliente(int IdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //conexion a la base de datos
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        //Restablecer permisos de usuario
        int puedeEditarDatosOrganizacion;
        if (Usuario.TienePermisos(new string[] { "puedeEditarDatosOrganizacion" }, ConexionBaseDatos) == "") { puedeEditarDatosOrganizacion = 1; }
        else { puedeEditarDatosOrganizacion = 0; }
        oPermisos.Add("puedeEditarDatosOrganizacion", puedeEditarDatosOrganizacion);

        int puedeEditarDatosFiscales;
        if (Usuario.TienePermisos(new string[] { "puedeEditarDatosFiscales" }, ConexionBaseDatos) == "") { puedeEditarDatosFiscales = 1; }
        else { puedeEditarDatosFiscales = 0; }
        oPermisos.Add("puedeEditarDatosFiscales", puedeEditarDatosFiscales);

        int puedeEditarLimiteCredito;
        if (Usuario.TienePermisos(new string[] { "puedeEditarLimiteCredito" }, ConexionBaseDatos) == "") { puedeEditarLimiteCredito = 1; }
        else { puedeEditarLimiteCredito = 0; }
        oPermisos.Add("puedeEditarLimiteCredito", puedeEditarLimiteCredito);

        int puedeEditarAsignacionCuentaContableCliente = 0;
        if (Usuario.TienePermisos(new string[] { "puedeEditarAsignacionCuentaContableCliente" }, ConexionBaseDatos) != "")
        {
            puedeEditarAsignacionCuentaContableCliente = 1;
        }
        oPermisos.Add("puedeEditarAsignacionCuentaContableCliente", puedeEditarAsignacionCuentaContableCliente);

        int puedeEditarAsignarAgenteCliente;
        if (Usuario.TienePermisos(new string[] { "puedeEditarAsignarAgenteCliente" }, ConexionBaseDatos) == "") { puedeEditarAsignarAgenteCliente = 1; }
        else { puedeEditarAsignarAgenteCliente = 0; }
        oPermisos.Add("puedeEditarAsignarAgenteCliente", puedeEditarAsignarAgenteCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(IdCliente, ConexionBaseDatos);
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            //llenamos los Combos
            Modelo.Add("IdSucursalActual", Usuario.IdSucursalActual);
            Modelo = CJson.ObtenerJsonCliente(Modelo, IdCliente, ConexionBaseDatos);
            Modelo.Add("TipoIndustrias", CJson.ObtenerJsonTipoIndustria(Convert.ToInt32(Modelo["IdTipoIndustria"].ToString()), ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(Convert.ToInt32(Modelo["IdCondicionPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("FormaContactos", CJson.ObtenerJsonFormaContactos(Convert.ToInt32(Modelo["IdFormaContacto"].ToString()), ConexionBaseDatos));

            CSelectEspecifico Select = new CSelectEspecifico();
            Select.StoredProcedure.CommandType = CommandType.StoredProcedure;
            Select.StoredProcedure.CommandText = "sp_Oportunidad_Consultar_ObtenerTotalesOportunidad";
            Select.StoredProcedure.Parameters.Add("pIdSucursal", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdSucursal"]);

            Modelo.Add("UsuarioAgentes", CUsuario.ObtenerJsonUsuarioAgente(Usuario.IdUsuario, Cliente.IdUsuarioAgente, ConexionBaseDatos));

            Modelo.Add("TipoClientes", CJson.ObtenerJsonTipoClientes(Convert.ToInt32(Modelo["IdTipoCliente"].ToString()), ConexionBaseDatos));
            Modelo.Add("GrupoEmpresariales", CJson.ObtenerJsonGrupoEmpresariales(Convert.ToInt32(Modelo["IdGrupoEmpresarial"].ToString()), ConexionBaseDatos));
            Modelo.Add("Localidades", CLocalidad.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CMunicipio.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CEstado.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CPais.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));
            Modelo.Add("Campanas", CCampana.ObtenerJsonCampana(Cliente.IdCampana, ConexionBaseDatos));


            COrganizacion Organizacion = new COrganizacion();
            Cliente.LlenaObjeto(IdCliente, ConexionBaseDatos);
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

            Modelo.Add("SegmentoMercados", CSegmentoMercado.ObtenerJsonSegmentoMercado(Organizacion.IdSegmentoMercado, ConexionBaseDatos));
            Modelo.Add("TipoGarantias", CTipoGarantia.ObtenerJsonTipoGarantia(Convert.ToInt32(Modelo["IdTipoGarantia"].ToString()), ConexionBaseDatos));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaEstados(int pIdPais)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonEstados(pIdPais, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaMunicipios(int pIdEstado)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonMunicipios(pIdEstado, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaLocalidades(int pIdMunicipio)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CJson.ObtenerJsonLocalidades(pIdMunicipio, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaDirecciones(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCliente = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCliente" }, ConexionBaseDatos) == "")
        {
            puedeEditarCliente = 1;
        }
        oPermisos.Add("puedeEditarCliente", puedeEditarCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonCliente(Modelo, pIdCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarDireccion()
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoIndustria TipoIndustria = new CTipoIndustria();
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(ConexionBaseDatos));
            Modelo.Add("TipoDirecciones", CJson.ObtenerJsonTipoDireccion(ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarCuentaBancariaCliente(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeAgregarCuentaBancaria = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeAgregarCuentaBancaria" }, ConexionBaseDatos) == "")
        {
            puedeAgregarCuentaBancaria = 1;
        }
        oPermisos.Add("puedeAgregarCuentaBancaria", puedeAgregarCuentaBancaria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            COrganizacion Organizacion = new COrganizacion();
            Cliente.LlenaObjeto(pIdCliente, ConexionBaseDatos);
            Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);

            Modelo.Add(new JProperty("IdCliente", Cliente.IdCliente));
            Modelo.Add(new JProperty("RazonSocial", Organizacion.RazonSocial));
            Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(ConexionBaseDatos));
            Modelo.Add("TipoBancos", CBanco.ObtenerJsonBanco(ConexionBaseDatos));
            Modelo.Add("MetodosPago", CMetodoPago.ObtenerJsonMetodosPago(ConexionBaseDatos));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaDireccionOrganizacion(int pIdDireccionOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDireccion = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDireccion" }, ConexionBaseDatos) == "")
        {
            puedeEditarDireccion = 1;
        }
        oPermisos.Add("puedeEditarDireccion", puedeEditarDireccion);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonDireccionOrganizacion(Modelo, pIdDireccionOrganizacion, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarDireccion(int IdDireccionOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDireccion = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDireccion" }, ConexionBaseDatos) == "")
        {
            puedeEditarDireccion = 1;
        }
        oPermisos.Add("puedeEditarDireccion", puedeEditarDireccion);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            DireccionOrganizacion.LlenaObjeto(IdDireccionOrganizacion, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonDireccionOrganizacion(Modelo, IdDireccionOrganizacion, ConexionBaseDatos);
            Modelo.Add("TipoDirecciones", CJson.ObtenerJsonTipoDireccion(Convert.ToInt32(Modelo["IdTipoDireccion"].ToString()), ConexionBaseDatos));
            Modelo.Add("Paises", CJson.ObtenerJsonPaises(Convert.ToInt32(Modelo["IdPais"].ToString()), ConexionBaseDatos));
            Modelo.Add("Estados", CJson.ObtenerJsonEstados(Convert.ToInt32(Modelo["IdPais"].ToString()), Convert.ToInt32(Modelo["IdEstado"].ToString()), ConexionBaseDatos));
            Modelo.Add("Municipios", CJson.ObtenerJsonMunicipios(Convert.ToInt32(Modelo["IdEstado"].ToString()), Convert.ToInt32(Modelo["IdMunicipio"].ToString()), ConexionBaseDatos));
            Modelo.Add("Localidades", CJson.ObtenerJsonLocalidades(Convert.ToInt32(Modelo["IdMunicipio"].ToString()), Convert.ToInt32(Modelo["IdLocalidad"].ToString()), ConexionBaseDatos));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarContactoOrganizacion(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarContacto = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarContacto" }, ConexionBaseDatos) == "")
        {
            puedeEditarContacto = 1;
        }
        oPermisos.Add("puedeEditarContacto", puedeEditarContacto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonCliente(Modelo, pIdCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarContactoOrganizacion()
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaContactoOrganizacion(int pIdContactoOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarContacto = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarContacto" }, ConexionBaseDatos) == "")
        {
            puedeEditarContacto = 1;
        }
        oPermisos.Add("puedeEditarContacto", puedeEditarContacto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CJson.ObtenerJsonContactoOrganizacion(Modelo, pIdContactoOrganizacion, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarContacto(int IdContactoOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarContacto = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarContacto" }, ConexionBaseDatos) == "")
        {
            puedeEditarContacto = 1;
        }
        oPermisos.Add("puedeEditarContacto", puedeEditarContacto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
            ContactoOrganizacion.LlenaObjeto(IdContactoOrganizacion, ConexionBaseDatos);
            Modelo = CJson.ObtenerJsonContactoOrganizacion(Modelo, IdContactoOrganizacion, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaTiposIndustrias()
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonTipoIndustria(true, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaCampanas()
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CCampana.ObtenerJsonCampana(true, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaTiposGarantia()
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CTipoGarantia.ObtenerJsonTipoGarantia(true, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaCondicionesPago()
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();


        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonCondicionPago(true, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerListaTiposClientes()
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CTipoCliente.ObtenerJsonTipoCliente(true, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaCuentaBancariaCliente(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeConsultarCuentaBancariaCliente;
        if (Usuario.TienePermisos(new string[] { "puedeConsultarCuentaBancariaCliente" }, ConexionBaseDatos) == "")
        {
            puedeConsultarCuentaBancariaCliente = 1;
        }
        else
        {
            puedeConsultarCuentaBancariaCliente = 0;
        }
        oPermisos.Add("puedeConsultarCuentaBancariaCliente", puedeConsultarCuentaBancariaCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(pIdCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCliente", Cliente.IdCliente));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaListaDescuentosCliente(int pIdCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeConsultarDescuentos;
        if (Usuario.TienePermisos(new string[] { "puedeConsultarDescuentosCliente" }, ConexionBaseDatos) == "")
        {
            puedeConsultarDescuentos = 1;
        }
        else
        {
            puedeConsultarDescuentos = 0;
        }
        oPermisos.Add("puedeConsultarDescuentos", puedeConsultarDescuentos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCliente Cliente = new CCliente();
            Cliente.LlenaObjeto(pIdCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCliente", Cliente.IdCliente));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConsultarCuentaBancariaCliente(int pIdCuentaBancariaCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeConsultarCuentaBancariaCliente = 0;
        if (Usuario.TienePermisos(new string[] { "puedeConsultarCuentaBancariaCliente" }, ConexionBaseDatos) == "") { puedeConsultarCuentaBancariaCliente = 1; }
        oPermisos.Add("puedeConsultarCuentaBancariaCliente", puedeConsultarCuentaBancariaCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CCuentaBancariaCliente.ObtenerJsonCuentaBancariaCliente(Modelo, pIdCuentaBancariaCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarCuentaBancariaCliente(int IdCuentaBancariaCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeEditarCuentaBancariaCliente = 0;
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentaBancariaCliente" }, ConexionBaseDatos) == "") { puedeEditarCuentaBancariaCliente = 1; }
        else { puedeEditarCuentaBancariaCliente = 0; }
        oPermisos.Add("puedeEditarCuentaBancariaCliente", puedeEditarCuentaBancariaCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCuentaBancaria CuentaBancaria = new CCuentaBancaria();

            Modelo = CCuentaBancariaCliente.ObtenerJsonCuentaBancariaCliente(Modelo, IdCuentaBancariaCliente, ConexionBaseDatos);

            Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("TipoBancos", CBanco.ObtenerJsonBanco(Convert.ToInt32(Modelo["IdBanco"].ToString()), ConexionBaseDatos));
            Modelo.Add("MetodosPago", CMetodoPago.ObtenerJsonMetodoPago(Convert.ToInt32(Modelo["IdMetodoPago"].ToString()), ConexionBaseDatos));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }
    
    [WebMethod]
    public static string AgregarCliente(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CSucursal Sucursal = new CSucursal();
            Organizacion.NombreComercial = Convert.ToString(pCliente["NombreComercial"]).ToUpper();
            Organizacion.RazonSocial = Convert.ToString(pCliente["RazonSocial"]).ToUpper();
            Organizacion.FechaAlta = Convert.ToDateTime(DateTime.Now);
            Organizacion.RFC = Convert.ToString(pCliente["RFC"]).ToUpper();
            Organizacion.Notas = Convert.ToString(pCliente["Notas"]);
            Organizacion.Dominio = Convert.ToString(pCliente["Dominio"]);
            Organizacion.IdTipoIndustria = Convert.ToInt32(pCliente["IdTipoIndustria"]);
            Organizacion.IdGrupoEmpresarial = Convert.ToInt32(pCliente["IdGrupoEmpresarial"]);
            Organizacion.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            Organizacion.IdEmpresaAlta = Usuario.IdSucursalActual;
            Organizacion.IdSegmentoMercado = Convert.ToInt32(pCliente["IdSegmentoMercado"]);

            CMunicipio Municipio = new CMunicipio();
            Municipio.LlenaObjeto(Convert.ToInt32(pCliente["IdMunicipio"]), ConexionBaseDatos);

            CEstado Estado = new CEstado();
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);

            DireccionOrganizacion.Descripcion = "Dirección Fiscal Inicial 1 (" + Estado.Estado + ")";
            DireccionOrganizacion.Calle = Convert.ToString(pCliente["Calle"]);
            DireccionOrganizacion.NumeroExterior = Convert.ToString(pCliente["NumeroExterior"]);
            DireccionOrganizacion.NumeroInterior = Convert.ToString(pCliente["NumeroInterior"]);
            DireccionOrganizacion.Colonia = Convert.ToString(pCliente["Colonia"]);
            DireccionOrganizacion.CodigoPostal = Convert.ToString(pCliente["CodigoPostal"]);
            DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pCliente["Conmutador"]);
            DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pCliente["IdMunicipio"]);
            DireccionOrganizacion.Referencia = Convert.ToString(pCliente["Referencia"]);
            DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pCliente["IdLocalidad"]);
            DireccionOrganizacion.IdTipoDireccion = 1;

            Cliente.FechaAlta = Convert.ToDateTime(DateTime.Now);
            Cliente.LimiteDeCredito = Convert.ToString(pCliente["LimiteDeCredito"]);
            Cliente.Correo = Convert.ToString(pCliente["Correo"]);
            Cliente.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            Cliente.IdTipoCliente = Convert.ToInt32(pCliente["IdTipoCliente"]);
            Cliente.IdCondicionPago = Convert.ToInt32(pCliente["IdCondicionPago"]);
            Cliente.IdFormaContacto = Convert.ToInt32(pCliente["IdFormaContacto"]);
            Cliente.IVAActual = Convert.ToDecimal(pCliente["IVAActual"]);
            Cliente.IdTipoGarantia = Convert.ToInt32(pCliente["IdTipoGarantia"]);
            Cliente.IdUsuarioAgente = Convert.ToInt32(pCliente["IdUsuarioAgente"]);
            Cliente.CuentaContable = Convert.ToString(pCliente["CuentaContable"]);
            Cliente.CuentaContableDolares = Convert.ToString(pCliente["CuentaContableDolares"]);
            Cliente.IdCampana = Convert.ToInt32(pCliente["IdCampana"]);

            string validacion = ValidarCliente(Cliente, Organizacion, DireccionOrganizacion, Usuario, ConexionBaseDatos);
            if (validacion == "" || validacion == "noexisteGE")
            {
                if (validacion == "noexisteGE")
                {
                    CGrupoEmpresarial GrupoEmpresarial = new CGrupoEmpresarial();
                    GrupoEmpresarial.GrupoEmpresarial = Convert.ToString(pCliente["NombreComercial"]).ToUpper();
                    GrupoEmpresarial.Agregar(ConexionBaseDatos);
                    Organizacion.IdGrupoEmpresarial = Convert.ToInt32(GrupoEmpresarial.IdGrupoEmpresarial);
                }

                if (validacion != "existeProveedor")
                {
                    Organizacion.Agregar(ConexionBaseDatos);
                    Cliente.IdOrganizacion = Organizacion.IdOrganizacion;
                    DireccionOrganizacion.IdOrganizacion = Organizacion.IdOrganizacion;
                    DireccionOrganizacion.Agregar(ConexionBaseDatos);
                }

                Cliente.Agregar(ConexionBaseDatos);
                CClienteSucursal ClienteSucursal = new CClienteSucursal();
                ClienteSucursal.IdCliente = Cliente.IdCliente;
                ClienteSucursal.IdSucursal = Usuario.IdSucursalActual;
                ClienteSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                ClienteSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                ClienteSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Cliente.IdCliente;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto el Cliente";
                HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                if (validacion == "existeProveedor") //Agrego Cliente y enrolamiento
                {

                    Dictionary<string, object> ParametrosRFC = new Dictionary<string, object>();
                    ParametrosRFC.Add("RFC", Organizacion.RFC);
                    Organizacion.LlenaObjetoFiltros(ParametrosRFC, ConexionBaseDatos);

                    Cliente.IdOrganizacion = Organizacion.IdOrganizacion;

                    Cliente.Agregar(ConexionBaseDatos);
                    CClienteSucursal ClienteSucursal = new CClienteSucursal();
                    ClienteSucursal.IdCliente = Cliente.IdCliente;
                    ClienteSucursal.IdSucursal = Usuario.IdSucursalActual;
                    ClienteSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    ClienteSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    ClienteSucursal.Agregar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Cliente.IdCliente;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto el Cliente y se enrolo";
                    HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);

                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                else if (validacion == "enrolar")   //Solo se hace enrolamiento    
                {
                    Dictionary<string, object> ParametrosRFC = new Dictionary<string, object>();
                    ParametrosRFC.Add("RFC", Organizacion.RFC);
                    Organizacion.LlenaObjetoFiltros(ParametrosRFC, ConexionBaseDatos);

                    CCliente IdCliente = new CCliente();

                    Dictionary<string, object> ParametrosO = new Dictionary<string, object>();
                    ParametrosO.Add("IdOrganizacion", Organizacion.IdOrganizacion);
                    IdCliente.LlenaObjetoFiltros(ParametrosO, ConexionBaseDatos);

                    CClienteSucursal ClienteSucursal = new CClienteSucursal();
                    ClienteSucursal.IdCliente = IdCliente.IdCliente;
                    ClienteSucursal.IdSucursal = Usuario.IdSucursalActual;
                    ClienteSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    ClienteSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    ClienteSucursal.Agregar(ConexionBaseDatos);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = Cliente.IdCliente;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se enrolo el cliente a la sucursal";
                    HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);

                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("IdCliente", IdCliente.IdCliente));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                    return oRespuesta.ToString();
                }



                CSelect ObtenObjeto = new CSelect();
                ObtenObjeto.StoredProcedure.CommandText = "sp_Cliente_Organizacion_Consulta";
                ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                ObtenObjeto.StoredProcedure.Parameters.AddWithValue("@pRFC", Convert.ToString(Organizacion.RFC));
                ObtenObjeto.Llena<CClienteSucursal>(typeof(CClienteSucursal), ConexionBaseDatos);
                foreach (CClienteSucursal ClienteSucursal in ObtenObjeto.ListaRegistros)
                {
                    JObject Modelo = new JObject();
                    Modelo.Add("IdCliente", ClienteSucursal.IdCliente);
                    oRespuesta.Add(new JProperty("Modelo", Modelo));
                }

            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string EnrolarCliente(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCliente Cliente = new CCliente();

        Cliente.IdCliente = Convert.ToInt32(pCliente["IdCliente"]);
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CClienteSucursal ClienteSucursal = new CClienteSucursal();
        ClienteSucursal.IdCliente = Cliente.IdCliente;
        ClienteSucursal.IdSucursal = Usuario.IdSucursalActual;
        ClienteSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
        ClienteSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        ClienteSucursal.Agregar(ConexionBaseDatos);

        CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
        HistorialGenerico.IdGenerico = Cliente.IdCliente;
        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
        HistorialGenerico.Comentario = "Se enrolo el Cliente a otra sucursal";
        HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);

        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("IdCliente", Cliente.IdCliente));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EnrolarProveedor(Dictionary<string, object> pProveedor)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CProveedor Proveedor = new CProveedor();
        Proveedor.IdProveedor = Convert.ToInt32(pProveedor["IdProveedor"]);
        Proveedor.LlenaObjeto(Proveedor.IdProveedor, ConexionBaseDatos);
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CCliente Cliente = new CCliente();
        Cliente.FechaAlta = Convert.ToDateTime(DateTime.Now);
        Cliente.Correo = Convert.ToString(Proveedor.Correo);
        Cliente.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Cliente.IdOrganizacion = Convert.ToInt32(Proveedor.IdOrganizacion);
        Cliente.Agregar(ConexionBaseDatos);

        CClienteSucursal ClienteSucursal = new CClienteSucursal();
        ClienteSucursal.IdCliente = Cliente.IdCliente;
        ClienteSucursal.IdSucursal = Usuario.IdSucursalActual;
        ClienteSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
        ClienteSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        ClienteSucursal.Agregar(ConexionBaseDatos);

        CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
        HistorialGenerico.IdGenerico = Cliente.IdCliente;
        HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
        HistorialGenerico.Comentario = "Se enrolo el proveedor como cliente";
        HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);

        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("IdCliente", Cliente.IdCliente));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarCliente(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        Cliente.LlenaObjeto(Convert.ToInt32(pCliente["IdCliente"]), ConexionBaseDatos);
        Cliente.IdCliente = Cliente.IdCliente;
        Cliente.FechaModificacion = Convert.ToDateTime(DateTime.Now);
        Cliente.LimiteDeCredito = Convert.ToString(pCliente["LimiteDeCredito"]);
        Cliente.Correo = Convert.ToString(pCliente["Correo"]);
        Cliente.IdTipoCliente = Convert.ToInt32(pCliente["IdTipoCliente"]);
        Cliente.IdCondicionPago = Convert.ToInt32(pCliente["IdCondicionPago"]);
        Cliente.IdFormaContacto = Convert.ToInt32(pCliente["IdFormaContacto"]);
        Cliente.IdUsuarioModifico = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Cliente.IVAActual = Convert.ToDecimal(pCliente["IVAActual"]);
        Cliente.IdTipoGarantia = Convert.ToInt32(pCliente["IdTipoGarantia"]);

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        if (Usuario.TienePermisos(new string[] { "puedeAsignarAgenteCliente" }, ConexionBaseDatos) == "")
        {
            if (pCliente["IdUsuarioAgente"] != "" && Convert.ToInt32(pCliente["IdUsuarioAgente"]) != 0)
            {
                Cliente.IdUsuarioAgente = Convert.ToInt32(pCliente["IdUsuarioAgente"]);
            }
        }

        Cliente.CuentaContable = Convert.ToString(pCliente["CuentaContable"]);
        Cliente.CuentaContableDolares = Convert.ToString(pCliente["CuentaContableDolares"]);
        Cliente.IdCampana = Convert.ToInt32(pCliente["IdCampana"]);

        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, ConexionBaseDatos);
        Organizacion.IdOrganizacion = Organizacion.IdOrganizacion;
        Organizacion.RazonSocial = Convert.ToString(pCliente["RazonSocial"]).ToUpper();
        Organizacion.NombreComercial = Convert.ToString(pCliente["NombreComercial"]).ToUpper();
        Organizacion.RFC = Convert.ToString(pCliente["RFC"]).ToUpper();
        Organizacion.Notas = Convert.ToString(pCliente["Notas"]);
        Organizacion.Dominio = Convert.ToString(pCliente["Dominio"]);
        Organizacion.IdTipoIndustria = Convert.ToInt32(pCliente["IdTipoIndustria"]);
        Organizacion.IdGrupoEmpresarial = Convert.ToInt32(pCliente["IdGrupoEmpresarial"]);
        Organizacion.FechaModificacion = Convert.ToDateTime(DateTime.Now);
        Organizacion.IdSegmentoMercado = Convert.ToInt32(pCliente["IdSegmentoMercado"]);

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdTipoDireccion", 1);
        Parametros.Add("IdOrganizacion", Cliente.IdOrganizacion);
        DireccionOrganizacion.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
        DireccionOrganizacion.IdDireccionOrganizacion = DireccionOrganizacion.IdDireccionOrganizacion;
        DireccionOrganizacion.Calle = Convert.ToString(pCliente["Calle"]);
        DireccionOrganizacion.NumeroExterior = Convert.ToString(pCliente["NumeroExterior"]);
        DireccionOrganizacion.NumeroInterior = Convert.ToString(pCliente["NumeroInterior"]);
        DireccionOrganizacion.Colonia = Convert.ToString(pCliente["Colonia"]);
        DireccionOrganizacion.CodigoPostal = Convert.ToString(pCliente["CodigoPostal"]);
        DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pCliente["Conmutador"]);
        DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pCliente["IdMunicipio"]);
        DireccionOrganizacion.Referencia = Convert.ToString(pCliente["Referencia"].ToString());
        DireccionOrganizacion.IdTipoDireccion = 1;
        DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pCliente["IdLocalidad"]);

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        string validacion = ValidarCliente(Cliente, Organizacion, DireccionOrganizacion, Usuario, ConexionBaseDatos);

        if (validacion == "")
        {

            Cliente.Editar(ConexionBaseDatos);
            Organizacion.Editar(ConexionBaseDatos);
            DireccionOrganizacion.Editar(ConexionBaseDatos);

            decimal IVA_Anterior = Cliente.IVAActual;
            string cambioIVA = string.Empty;
            if (IVA_Anterior != Cliente.IVAActual)
            {
                cambioIVA = "El IVA cambio de" + IVA_Anterior.ToString() + " a " + Cliente.IVAActual.ToString();
            }

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Cliente.IdCliente;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico el Cliente. " + cambioIVA;
            HistorialGenerico.AgregarHistorialGenerico("Cliente", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string AgregarDireccion(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCliente Cliente = new CCliente();
            COrganizacion Organizacion = new COrganizacion();
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            Cliente.LlenaObjeto(Convert.ToInt32(pCliente["IdCliente"]), ConexionBaseDatos);

            DireccionOrganizacion.IdOrganizacion = Cliente.IdOrganizacion;
            DireccionOrganizacion.Calle = Convert.ToString(pCliente["Calle"]);
            DireccionOrganizacion.NumeroExterior = Convert.ToString(pCliente["NumeroExterior"]);
            DireccionOrganizacion.NumeroInterior = Convert.ToString(pCliente["NumeroInterior"]);
            DireccionOrganizacion.Colonia = Convert.ToString(pCliente["Colonia"]);
            DireccionOrganizacion.CodigoPostal = Convert.ToString(pCliente["CodigoPostal"]);
            DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pCliente["Conmutador"]);
            DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pCliente["IdMunicipio"]);
            DireccionOrganizacion.Referencia = Convert.ToString(pCliente["Referencia"]);
            DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pCliente["IdLocalidad"]);
            DireccionOrganizacion.IdTipoDireccion = Convert.ToInt32(pCliente["IdTipoDireccion"]);
            DireccionOrganizacion.Descripcion = Convert.ToString(pCliente["Descripcion"]);


            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            string validacion = ValidarDireccion(DireccionOrganizacion, ConexionBaseDatos);
            if (validacion == "")
            {
                DireccionOrganizacion.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = DireccionOrganizacion.IdDireccionOrganizacion;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto el una nueva dirección";
                HistorialGenerico.AgregarHistorialGenerico("DireccionOrganizacion", ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EditarDireccion(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();


        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        DireccionOrganizacion.LlenaObjeto(Convert.ToInt32(pCliente["IdDireccionOrganizacion"]), ConexionBaseDatos);
        DireccionOrganizacion.Calle = Convert.ToString(pCliente["Calle"]);
        DireccionOrganizacion.NumeroExterior = Convert.ToString(pCliente["NumeroExterior"]);
        DireccionOrganizacion.NumeroInterior = Convert.ToString(pCliente["NumeroInterior"]);
        DireccionOrganizacion.Colonia = Convert.ToString(pCliente["Colonia"]);
        DireccionOrganizacion.CodigoPostal = Convert.ToString(pCliente["CodigoPostal"]);
        DireccionOrganizacion.ConmutadorTelefono = Convert.ToString(pCliente["Conmutador"]);
        DireccionOrganizacion.IdMunicipio = Convert.ToInt32(pCliente["IdMunicipio"]);
        DireccionOrganizacion.Referencia = Convert.ToString(pCliente["Referencia"]);
        DireccionOrganizacion.IdLocalidad = Convert.ToInt32(pCliente["IdLocalidad"]);
        DireccionOrganizacion.IdTipoDireccion = Convert.ToInt32(pCliente["IdTipoDireccion"]);
        DireccionOrganizacion.Descripcion = Convert.ToString(pCliente["Descripcion"]);

        string validacion = ValidarDireccion(DireccionOrganizacion, ConexionBaseDatos);
        if (validacion == "")
        {
            DireccionOrganizacion.Editar(ConexionBaseDatos);
            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = DireccionOrganizacion.IdDireccionOrganizacion;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico la dirección";
            HistorialGenerico.AgregarHistorialGenerico("DireccionOrganizacion", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus_Cliente(int pIdCliente, bool pBaja)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            //Sucursal quien dio de alta la organizacion
            CCliente C = new CCliente();
            C.LlenaObjeto(pIdCliente, ConexionBaseDatos);
            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(C.IdOrganizacion, ConexionBaseDatos);
            CUsuario U = new CUsuario();
            U.LlenaObjeto(Organizacion.IdUsuarioAlta, ConexionBaseDatos);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(U.IdSucursalActual, ConexionBaseDatos);

            //Mi IdSucursalActual
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (Sucursal.IdSucursal == Usuario.IdSucursalActual)
            {
                CCliente Cliente = new CCliente();
                Cliente.IdCliente = pIdCliente;
                Cliente.Baja = pBaja;
                Cliente.Eliminar(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", "0"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", "1"));
                oRespuesta.Add(new JProperty("Descripcion", "No tiene permisos para dar de baja al cliente de la sucursal " + Sucursal.Sucursal));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {

            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string CambiarEstatus_Direccion(int pIdDireccionOrganizacion, bool pBaja)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            //Sucursal quien dio de alta la organizacion
            CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
            DireccionOrganizacion.LlenaObjeto(pIdDireccionOrganizacion, ConexionBaseDatos);
            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(DireccionOrganizacion.IdOrganizacion, ConexionBaseDatos);
            CUsuario U = new CUsuario();
            U.LlenaObjeto(Organizacion.IdUsuarioAlta, ConexionBaseDatos);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(U.IdSucursalActual, ConexionBaseDatos);

            //Mi IdSucursalActual
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (Sucursal.IdSucursal == Usuario.IdSucursalActual)
            {

                DireccionOrganizacion.IdDireccionOrganizacion = pIdDireccionOrganizacion;
                DireccionOrganizacion.Baja = pBaja;
                DireccionOrganizacion.Eliminar(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", "0"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", "1"));
                oRespuesta.Add(new JProperty("Descripcion", "No tiene permisos para dar de baja la direccion de la sucursal " + Sucursal.Sucursal));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string CambiarEstatus_ContactoOrganizacion(int pIdContactoOrganizacion, bool pBaja)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            //Sucursal quien dio de alta la organizacion
            CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
            ContactoOrganizacion.LlenaObjeto(pIdContactoOrganizacion, ConexionBaseDatos);
            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(ContactoOrganizacion.IdOrganizacion, ConexionBaseDatos);
            CUsuario U = new CUsuario();
            U.LlenaObjeto(Organizacion.IdUsuarioAlta, ConexionBaseDatos);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(U.IdSucursalActual, ConexionBaseDatos);

            //Mi IdSucursalActual
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (Sucursal.IdSucursal == Usuario.IdSucursalActual || Usuario.TienePermisos(new string[] {"puedeCambiarEstatusContacto"}, ConexionBaseDatos) == "")
            {
                ContactoOrganizacion.IdCliente = pIdContactoOrganizacion;
                ContactoOrganizacion.Baja = pBaja;
                ContactoOrganizacion.Eliminar(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", "0"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", "1"));
                oRespuesta.Add(new JProperty("Descripcion", "No tiene permisos para dar de baja al contacto de la sucursal " + Sucursal.Sucursal));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string AgregarContacto(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCliente Cliente = new CCliente();
            COrganizacion Organizacion = new COrganizacion();
            CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
            Cliente.LlenaObjeto(Convert.ToInt32(pCliente["IdCliente"]), ConexionBaseDatos);
            ContactoOrganizacion.Nombre = Convert.ToString(pCliente["Nombre"]);
            ContactoOrganizacion.Puesto = Convert.ToString(pCliente["Puesto"]);
            ContactoOrganizacion.Notas = Convert.ToString(pCliente["Notas"]);
            if (!(Convert.ToString(pCliente["FechaCumpleanio"]) == ""))
            {
                ContactoOrganizacion.Cumpleanio = Convert.ToDateTime(pCliente["FechaCumpleanio"]);
            }

            ContactoOrganizacion.IdCliente = Convert.ToInt32(Cliente.IdCliente);
            ContactoOrganizacion.IdOrganizacion = Convert.ToInt32(Cliente.IdOrganizacion);

            List<CTelefonoContactoOrganizacion> Telefonos = new List<CTelefonoContactoOrganizacion>();
            foreach (Dictionary<string, object> oTelefono in (Array)pCliente["Telefonos"])
            {

                CTelefonoContactoOrganizacion Telefono = new CTelefonoContactoOrganizacion();
                Telefono.Telefono = Convert.ToString(oTelefono["Telefono"]);
                Telefono.Descripcion = Convert.ToString(oTelefono["Descripcion"]);
                Telefonos.Add(Telefono);
            }

            List<CCorreoContactoOrganizacion> Correos = new List<CCorreoContactoOrganizacion>();
            foreach (string oCorreo in (Array)pCliente["Correos"])
            {
                CCorreoContactoOrganizacion Correo = new CCorreoContactoOrganizacion();
                Correo.Correo = oCorreo;
                Correos.Add(Correo);
            }


            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            string validacion = ValidarContacto(ContactoOrganizacion, ConexionBaseDatos);
            if (validacion == "")
            {
                ContactoOrganizacion.Agregar(ConexionBaseDatos);

                foreach (CTelefonoContactoOrganizacion oTelefono in Telefonos)
                {
                    oTelefono.IdContactoOrganizacion = ContactoOrganizacion.IdContactoOrganizacion;
                    oTelefono.Agregar(ConexionBaseDatos);
                }

                foreach (CCorreoContactoOrganizacion oCorreo in Correos)
                {
                    oCorreo.IdContactoOrganizacion = ContactoOrganizacion.IdContactoOrganizacion;
                    oCorreo.Agregar(ConexionBaseDatos);
                }

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = ContactoOrganizacion.IdContactoOrganizacion;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto el una nuevo contacto";
                HistorialGenerico.AgregarHistorialGenerico("ContactoOrganizacion", ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EditarContacto(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();


        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();

        CContactoOrganizacion ContactoOrganizacion = new CContactoOrganizacion();
        ContactoOrganizacion.LlenaObjeto(Convert.ToInt32(pCliente["IdContactoOrganizacion"]), ConexionBaseDatos);

        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();

        ContactoOrganizacion.Nombre = Convert.ToString(pCliente["Nombre"]);
        ContactoOrganizacion.Puesto = Convert.ToString(pCliente["Puesto"]);
        ContactoOrganizacion.Notas = Convert.ToString(pCliente["Notas"]);
        ContactoOrganizacion.Cumpleanio = Convert.ToDateTime(pCliente["FechaCumpleanio"]);

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        string validacion = ValidarContacto(ContactoOrganizacion, ConexionBaseDatos);
        if (validacion == "")
        {
            ContactoOrganizacion.Editar(ConexionBaseDatos);

            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = ContactoOrganizacion.IdContactoOrganizacion;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modifico el contacto";
            HistorialGenerico.AgregarHistorialGenerico("ContactoOrganizacion", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string AgregarTelefonoEditar(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTelefonoContactoOrganizacion TelefonoContactoOrganizacion = new CTelefonoContactoOrganizacion();
            TelefonoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pCliente["IdContactoOrganizacion"]);
            TelefonoContactoOrganizacion.Telefono = Convert.ToString(pCliente["Telefono"]);
            TelefonoContactoOrganizacion.Descripcion = Convert.ToString(pCliente["Descripcion"]);
            TelefonoContactoOrganizacion.Agregar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarCorreoEditar(Dictionary<string, object> pCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCorreoContactoOrganizacion CorreoContactoOrganizacion = new CCorreoContactoOrganizacion();
            CorreoContactoOrganizacion.IdContactoOrganizacion = Convert.ToInt32(pCliente["IdContactoOrganizacion"]);
            CorreoContactoOrganizacion.Correo = Convert.ToString(pCliente["Correo"]);
            CorreoContactoOrganizacion.Agregar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EliminaTelefonoContactoOrganizacion(int pIdTelefonoContactoOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CTelefonoContactoOrganizacion TelefonoContactoOrganizacion = new CTelefonoContactoOrganizacion();
        TelefonoContactoOrganizacion.IdTelefonoContactoOrganizacion = Convert.ToInt32(pIdTelefonoContactoOrganizacion);
        TelefonoContactoOrganizacion.Baja = true;
        TelefonoContactoOrganizacion.Eliminar(ConexionBaseDatos);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string EliminaCorreoContactoOrganizacion(int pIdCorreoContactoOrganizacion)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CCorreoContactoOrganizacion CorreoContactoOrganizacion = new CCorreoContactoOrganizacion();
        CorreoContactoOrganizacion.IdCorreoContactoOrganizacion = Convert.ToInt32(pIdCorreoContactoOrganizacion);
        CorreoContactoOrganizacion.Baja = true;
        CorreoContactoOrganizacion.Eliminar(ConexionBaseDatos);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string AgregarCuentaBancariaCliente(Dictionary<string, object> pCuentaBancariaCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();
        JObject Modelo = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeAgregarCuentaBancariaCliente = 0;
        if (Usuario.TienePermisos(new string[] { "puedeAgregarCuentaBancariaCliente" }, ConexionBaseDatos) == "")
        {
            puedeAgregarCuentaBancariaCliente = 1;
        }
        oPermisos.Add("puedeAgregarCuentaBancariaCliente", puedeAgregarCuentaBancariaCliente);

        CCuentaBancariaCliente CuentaBancariaCliente = new CCuentaBancariaCliente();
        CuentaBancariaCliente.IdCliente = Convert.ToInt32(pCuentaBancariaCliente["IdCliente"]);
        CuentaBancariaCliente.CuentaBancariaCliente = Convert.ToString(pCuentaBancariaCliente["CuentaBancariaCliente"]);
        CuentaBancariaCliente.Descripcion = Convert.ToString(pCuentaBancariaCliente["Descripcion"]);
        CuentaBancariaCliente.IdBanco = Convert.ToInt32(pCuentaBancariaCliente["IdBanco"]);
        CuentaBancariaCliente.IdTipoMoneda = Convert.ToInt32(pCuentaBancariaCliente["IdTipoMoneda"]);
        CuentaBancariaCliente.IdMetodoPago = Convert.ToInt32(pCuentaBancariaCliente["IdMetodoPago"]);

        Dictionary<string, object> ParametrosCB = new Dictionary<string, object>();
        ParametrosCB.Add("IdCliente", CuentaBancariaCliente.IdCliente);
        ParametrosCB.Add("CuentaBancariaCliente", CuentaBancariaCliente.CuentaBancariaCliente);
        if (CuentaBancariaCliente.RevisarExisteRegistro(ParametrosCB, ConexionBaseDatos) == false)
        {
            CuentaBancariaCliente.Agregar(ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            respuesta = "CuentaBancariaClienteAgregada";
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "Ya existe la cuenta bancaria del cliente"));
        }

        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string CambiarEstatusCuentaBancariaCliente(int pIdCuentaBancariaCliente, bool pBaja)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oPermisos = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCuentaBancariaCliente CuentaBancariaCliente = new CCuentaBancariaCliente();
            CuentaBancariaCliente.IdCuentaBancariaCliente = pIdCuentaBancariaCliente;
            CuentaBancariaCliente.Baja = pBaja;
            CuentaBancariaCliente.Eliminar(ConexionBaseDatos);
            respuesta = "0|CuentaBancariaClienteEliminado";
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarCuentaBancariaCliente(Dictionary<string, object> pCuentaBancariaCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCuentaBancariaCliente CuentaBancariaCliente = new CCuentaBancariaCliente();

        CuentaBancariaCliente.LlenaObjeto(Convert.ToInt32(pCuentaBancariaCliente["IdCuentaBancariaCliente"]), ConexionBaseDatos);
        CuentaBancariaCliente.CuentaBancariaCliente = Convert.ToString(pCuentaBancariaCliente["CuentaBancariaCliente"]);
        CuentaBancariaCliente.Descripcion = Convert.ToString(pCuentaBancariaCliente["Descripcion"]);
        CuentaBancariaCliente.IdBanco = Convert.ToInt32(pCuentaBancariaCliente["IdBanco"]);
        CuentaBancariaCliente.IdTipoMoneda = Convert.ToInt32(pCuentaBancariaCliente["IdTipoMoneda"]);
        CuentaBancariaCliente.IdMetodoPago = Convert.ToInt32(pCuentaBancariaCliente["IdMetodoPago"]);

        string validacion = ValidarCuentaBancariaCliente(CuentaBancariaCliente, ConexionBaseDatos);
        if (validacion == "")
        {
            CuentaBancariaCliente.Editar(ConexionBaseDatos);
            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = CuentaBancariaCliente.IdCuentaBancariaCliente;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se modificó la cuenta bancaria del cliente";
            HistorialGenerico.AgregarHistorialGenerico("CuentaBancariaCliente", ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatusDescuentoCliente(int pIdDescuentoCliente, bool pBaja)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDescuentoCliente DescuentoCliente = new CDescuentoCliente();
            DescuentoCliente.IdDescuentoCliente = pIdDescuentoCliente;
            DescuentoCliente.Baja = pBaja;
            DescuentoCliente.Eliminar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string AgregarDescuentoCliente(Dictionary<string, object> pDescuentoCliente)
    {
        JObject oRespuesta = new JObject();
        int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        if (idUsuario == 0)
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "La sesión caduco favor de volver a iniciar sesión"));
            return oRespuesta.ToString();
        }

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CDescuentoCliente DescuentoCliente = new CDescuentoCliente();
            DescuentoCliente.IdCliente = Convert.ToInt32(pDescuentoCliente["IdCliente"]);
            DescuentoCliente.DescuentoCliente = Convert.ToDecimal(pDescuentoCliente["Descuento"]);
            DescuentoCliente.Descripcion = Convert.ToString(pDescuentoCliente["DescripcionDescuento"]);
            DescuentoCliente.Baja = false;

            string validacion = ValidarDescuentoCliente(DescuentoCliente, ConexionBaseDatos);
            if (validacion == "")
            {
                DescuentoCliente.Agregar(ConexionBaseDatos);
                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = DateTime.Now;
                HistorialGenerico.IdGenerico = DescuentoCliente.IdDescuentoCliente;
                HistorialGenerico.Comentario = "Se agregó un descuento con el " + DescuentoCliente.Descripcion.ToString() + " al cliente con Id: " + DescuentoCliente.IdCliente;
                HistorialGenerico.Agregar(ConexionBaseDatos);

                oRespuesta.Add("Error", 0);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add("Error", 1);
                oRespuesta.Add("Descripcion", validacion);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }
    
    [WebMethod]
    public static string RevisaPermisoRFC()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CCliente Cliente = new CCliente();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        int puedeInsertarCaracteresRaros;
        if (Usuario.TienePermisos(new string[] { "puedeInsertarCaracteresRaros" }, ConexionBaseDatos) == "") { puedeInsertarCaracteresRaros = 1; }
        else { puedeInsertarCaracteresRaros = 0; }
        oPermisos.Add("puedeInsertarCaracteresRaros", puedeInsertarCaracteresRaros);


        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));

        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string RevisaExisteRazonSocial(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        string jsonOrganizacionString = jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonOrganizacionString;
    }

    [WebMethod]
    public static string RevisaExisteOrganizacion(int pIdOrganizacion)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            CCliente Cliente = new CCliente();
            CClienteSucursal ClienteSucursal = new CClienteSucursal();
            Dictionary<string, object> ParametrosC = new Dictionary<string, object>();
            ParametrosC.Add("IdOrganizacion", pIdOrganizacion);

            Cliente.LlenaObjetoFiltros(ParametrosC, ConexionBaseDatos);
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            int validacion = Cliente.RevisaExisteCliente(pIdOrganizacion, Usuario.IdSucursalActual, ConexionBaseDatos);

            if (validacion == 1) //Ya existe en mi sucursal, solo imprimir datos
            {
                JObject Modelo = new JObject();
                Modelo = CJson.ObtenerJsonCliente(Modelo, Cliente.IdCliente, ConexionBaseDatos);
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else if (validacion == 2) //Ya existe como cliente, solo enrolar
            {
                oRespuesta.Add(new JProperty("Modelo", "enrolar"));
                oRespuesta.Add(new JProperty("IdCliente", Cliente.IdCliente));

            }
            else if (validacion == 3) //Existe como proveedor, agregar cliente y enrolar
            {
                oRespuesta.Add(new JProperty("Modelo", "agregarCliente"));
                oRespuesta.Add(new JProperty("IdOrganizacion", pIdOrganizacion));

            }
            else
            {
                oRespuesta.Add(new JProperty("IdOrganizacion", pIdOrganizacion));
                oRespuesta.Add(new JProperty("error", "No existe proveedor ni cliente"));
            }

            oRespuesta.Add(new JProperty("IdSucursalActual", Usuario.IdSucursalActual));

            COrganizacion Organizacion = new COrganizacion();
            Organizacion.LlenaObjeto(pIdOrganizacion, ConexionBaseDatos);

            Dictionary<string, object> ParametrosSucursalAlta = new Dictionary<string, object>();
            ParametrosSucursalAlta.Add("IdUsuario", Organizacion.IdUsuarioAlta);
            Usuario.LlenaObjetoFiltros(ParametrosSucursalAlta, ConexionBaseDatos);
            oRespuesta.Add(new JProperty("IdSucursalAlta", Usuario.IdSucursalActual));


            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string RevisaClienteRFC(string pRFC)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CCliente Cliente = new CCliente();
            CClienteSucursal ClienteSucursal = new CClienteSucursal();
            COrganizacion Organizacion = new COrganizacion();

            Dictionary<string, object> ParametrosRFC = new Dictionary<string, object>();
            ParametrosRFC.Add("RFC", pRFC);
            Organizacion.LlenaObjetoFiltros(ParametrosRFC, ConexionBaseDatos);

            if (Organizacion.IdOrganizacion != 0)
            {

                Dictionary<string, object> ParametrosC = new Dictionary<string, object>();
                ParametrosC.Add("IdOrganizacion", Organizacion.IdOrganizacion);
                Cliente.LlenaObjetoFiltros(ParametrosC, ConexionBaseDatos);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                int validacion = Cliente.RevisaExisteCliente(Organizacion.IdOrganizacion, Usuario.IdSucursalActual, ConexionBaseDatos);

                if (validacion == 1) //Ya existe en mi sucursal, solo imprimir datos
                {
                    JObject Modelo = new JObject();
                    Modelo = CJson.ObtenerJsonCliente(Modelo, Cliente.IdCliente, ConexionBaseDatos);
                    Modelo.Add(new JProperty("Permisos", oPermisos));
                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Modelo", Modelo));
                }
                else if (validacion == 2) //Ya existe como cliente, solo enrolar
                {
                    oRespuesta.Add(new JProperty("Modelo", "enrolar"));
                    oRespuesta.Add(new JProperty("IdCliente", Cliente.IdCliente));

                }
                else if (validacion == 3) //Existe como proveedor, agregar cliente y enrolar
                {
                    oRespuesta.Add(new JProperty("Modelo", "agregarCliente"));
                    oRespuesta.Add(new JProperty("IdOrganizacion", Organizacion.IdOrganizacion));
                }
                else
                {
                    oRespuesta.Add(new JProperty("IdOrganizacion", Organizacion.IdOrganizacion));
                    oRespuesta.Add(new JProperty("error", "No existe proveedor ni cliente"));
                }

                oRespuesta.Add(new JProperty("IdSucursalActual", Usuario.IdSucursalActual));
                Dictionary<string, object> ParametrosSucursalAlta = new Dictionary<string, object>();
                ParametrosSucursalAlta.Add("IdUsuario", Organizacion.IdUsuarioAlta);
                Usuario.LlenaObjetoFiltros(ParametrosSucursalAlta, ConexionBaseDatos);
                oRespuesta.Add(new JProperty("IdSucursalAlta", Usuario.IdSucursalActual));

            }
            else
            {
                oRespuesta.Add(new JProperty("Modelo", "noExiste"));
            }



            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }
    
    private static string ValidarCliente(CCliente pCliente, COrganizacion pOrganizacion, CDireccionOrganizacion pDireccionOrganizacion, CUsuario pUsuario, CConexion pConexion)
    {
        string errores = "";
        int ExisteCliente = 0;

        if (pOrganizacion.RazonSocial == "")
        { errores = errores + "<span>*</span> La razón social esta vacia, favor de capturarla.<br />"; }

        if (pOrganizacion.NombreComercial == "")
        { errores = errores + "<span>*</span> El nombre comercial del Cliente esta vacío, favor de capturarlo.<br />"; }

        if (pOrganizacion.RFC == "")
        { errores = errores + "<span>*</span> El RFC esta vacío, favor de capturarlo.<br />"; }

        //if (pOrganizacion.IdTipoIndustria == 0)
        //{ errores = errores + "<span>*</span> El campo de tipo industria esta vacío, favor de seleccionarlo.<br />"; }

        //if (pCliente.IdUsuarioAgente == 0)
        //{ errores = errores + "<span>*</span> El campo usuario agente esta vacío, favor de seleccionarlo.<br />"; }

        //if (pOrganizacion.IdSegmentoMercado == 0)
        //{ errores = errores + "<span>*</span> El campo usuario agente esta vacío, favor de seleccionarlo.<br />"; }

        //if (pDireccionOrganizacion.Calle == "")
        //{ errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

        //if (pDireccionOrganizacion.NumeroExterior == "")
        //{ errores = errores + "<span>*</span> El campo numero exterior esta vacío, favor de capturarlo.<br />"; }

        //if (pDireccionOrganizacion.Colonia == "")
        //{ errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

        //if (pDireccionOrganizacion.CodigoPostal == "")
        //{ errores = errores + "<span>*</span> El campo codigo postal esta vacío, favor de capturarlo.<br />"; }

        //if (pDireccionOrganizacion.IdMunicipio == 0)
        //{ errores = errores + "<span>*</span> El campo Municipio esta vacío, favor de seleccionarlo.<br />"; }

        //if (pCliente.IdTipoCliente == 0)
        //{ errores = errores + "<span>*</span> El campo tipo cliente esta vacío, favor de seleccionarlo.<br />"; }

        //if (pCliente.IdFormaContacto == 0)
        //{ errores = errores + "<span>*</span> El campo forma de pago esta vacío, favor de seleccionarlo.<br />"; }

        if (pCliente.IdCliente == 0)
        {
            ExisteCliente = pCliente.ExisteCliente(pOrganizacion.RFC, pUsuario.IdSucursalActual, pConexion);
            if (ExisteCliente != 0)
            {
                if (ExisteCliente == 1 && false) //Ya existe en mi sucursal, solo imprimir datos
                {
                    //errores = errores + "<span>*</span> Este RFC: " + pOrganizacion.RFC + " ya esta dada de alta.<br />";
                }
                else if (ExisteCliente == 2) //Ya existe como cliente, solo enrolar
                {
                    errores = errores + "enrolar";
                    return errores;
                }
                else if (ExisteCliente == 3) //Existe como proveedor, mostrar datos de organizacion y direccion de organizacion
                {
                    errores = errores + "existeProveedor";
                    return errores;
                }
            }

            if (pOrganizacion.IdGrupoEmpresarial == 0)
            {
                if (pOrganizacion.ExisteGrupoEmpresarial(pOrganizacion.NombreComercial, pConexion))
                {
                    errores = errores + "<span>*</span> El campo grupo empresarial ya existe, favor de seleccionarlo.<br />";
                }
                else
                {
                    errores = errores + "noexisteGE";
                    return errores;
                }
            }
        }
        else
        {
            ExisteCliente = pCliente.ExisteClienteEditar(pOrganizacion.RFC, pCliente.IdCliente, pUsuario.IdSucursalActual, pConexion);
            if (ExisteCliente != 0)
            {
                if (ExisteCliente == 1)
                {
                    //errores = errores + "<span>*</span> Este RFC " + pOrganizacion.RFC + " ya esta dada de alta.<br />";
                }
                else
                {
                    errores = errores + "<span>*</span> No puede editar un Cliente que no este enrolada a su empresa<br />";
                    return errores;
                }
            }
        }
        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarDireccion(CDireccionOrganizacion pDireccionOrganizacion, CConexion pConexion)
    {
        string errores = "";

        if (pDireccionOrganizacion.IdTipoDireccion == 0)
        { errores = errores + "<span>*</span> El campo tipo de dirección esta vacío, favor de seleccionarlo.<br />"; }

        if (pDireccionOrganizacion.Calle == "")
        { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.NumeroExterior == "")
        { errores = errores + "<span>*</span> El campo número exterior esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.Colonia == "")
        { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.CodigoPostal == "")
        { errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }

        if (pDireccionOrganizacion.IdMunicipio == 0)
        { errores = errores + "<span>*</span> El campo municipio esta vacío, favor de seleccionarlo.<br />"; }


        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarContacto(CContactoOrganizacion pContactoOrganizacion, CConexion pConexion)
    {
        string errores = "";

        if (pContactoOrganizacion.Nombre == "")
        { errores = errores + "<span>*</span> El campo nombre esta vacío, favor de capturarlo.<br />"; }

        if (pContactoOrganizacion.Puesto == "")
        { errores = errores + "<span>*</span> El campo puesto esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarCuentaBancariaCliente(CCuentaBancariaCliente pCuentaBancariaCliente, CConexion pConexion)
    {
        string errores = "";

        if (pCuentaBancariaCliente.IdBanco == 0)
        { errores = errores + "<span>*</span> El campo banco esta vacío, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarDescuentoCliente(CDescuentoCliente pDescuentoCliente, CConexion pConexion)
    {
        string errores = "";

        if (pDescuentoCliente.DescuentoCliente == 0)
        { errores = errores + "<span>*</span> El campo descuento esta vacío, favor de capturarlo.<br />"; }
        if (pDescuentoCliente.Descripcion == "")
        { errores = errores + "<span>*</span> El campo descripción del descuento esta vacío, favor de capturarlo.<br />"; }

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdCliente", pDescuentoCliente.IdCliente);
        Parametros.Add("DescuentoCliente", pDescuentoCliente.DescuentoCliente);

        /*
        CDescuentoCliente ValidarDescuentoCliente = new CDescuentoCliente();
        ValidarDescuentoCliente.LlenaObjetoFiltros(Parametros, pConexion);
        if (ValidarDescuentoCliente.IdDescuentoCliente != 0)
        {
            errores = errores + "<span>*</span> El descuento ya existe para este producto, favor de verificar.<br />";
        }
        */

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
    
}