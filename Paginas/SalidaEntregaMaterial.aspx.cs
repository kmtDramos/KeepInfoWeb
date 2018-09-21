using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
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
using System.IO;

public partial class Paginas_SalidaEntregaMaterial : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        CJQGrid GridSolicitudMaterial = new CJQGrid();
        GridSolicitudMaterial.NombreTabla = "grdEntregaMaterial";
        GridSolicitudMaterial.CampoIdentificador = "IdSolicitudMaterial";
        GridSolicitudMaterial.ColumnaOrdenacion = "IdSolicitudMaterial";
        GridSolicitudMaterial.TipoOrdenacion = "ASC";
        GridSolicitudMaterial.Metodo = "ObtenerSolicitudEntregaMaterial";
        GridSolicitudMaterial.TituloTabla = "Salida Material";
        GridSolicitudMaterial.GenerarFuncionFiltro = false;
        GridSolicitudMaterial.GenerarFuncionTerminado = false;
        GridSolicitudMaterial.Altura = 231;
        GridSolicitudMaterial.Ancho = 870;
        GridSolicitudMaterial.NumeroRegistros = 25;
        GridSolicitudMaterial.RangoNumeroRegistros = "25,50,100";

        #region Columnas

        //IdSolicitudMaterial
        CJQColumn ColIdSolicitudMaterial = new CJQColumn();
        ColIdSolicitudMaterial.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMaterial.Encabezado = "Solicitud Material";
        ColIdSolicitudMaterial.Oculto = "false";
        ColIdSolicitudMaterial.Buscador = "true";
        ColIdSolicitudMaterial.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColIdSolicitudMaterial);

        //Solicitante
        CJQColumn ColSolicitante = new CJQColumn();
        ColSolicitante.Nombre = "Solicitante";
        ColSolicitante.Encabezado = "Solicitante";
        ColSolicitante.Alineacion = "left";
        ColSolicitante.Buscador = "false";
        ColSolicitante.Ancho = "100";
        GridSolicitudMaterial.Columnas.Add(ColSolicitante);

        //Fecha Alta
        CJQColumn ColFechaAlta = new CJQColumn();
        ColFechaAlta.Nombre = "FechaAlta";
        ColFechaAlta.Encabezado = "Fecha Alta";
        ColFechaAlta.Alineacion = "left";
        ColFechaAlta.Buscador = "false";
        ColFechaAlta.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColFechaAlta);

        //Estatus
        CJQColumn ColEstatus = new CJQColumn();
        ColEstatus.Nombre = "Estatus";
        ColEstatus.Encabezado = "Estatus";
        ColEstatus.Alineacion = "left";
        ColEstatus.Buscador = "false";
        ColEstatus.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColEstatus);

        //Fecha Entrega
        CJQColumn ColFechaEntrega = new CJQColumn();
        ColFechaEntrega.Nombre = "FechaEntrega";
        ColFechaEntrega.Encabezado = "Fecha Entrega";
        ColFechaEntrega.Alineacion = "left";
        ColFechaEntrega.Buscador = "false";
        ColFechaEntrega.Ancho = "50";
        GridSolicitudMaterial.Columnas.Add(ColFechaEntrega);

        //Comentarios
        CJQColumn ColComentarios = new CJQColumn();
        ColComentarios.Nombre = "Comentarios";
        ColComentarios.Encabezado = "Comentarios";
        ColComentarios.Alineacion = "left";
        ColComentarios.Buscador = "false";
        ColComentarios.Ancho = "150";
        GridSolicitudMaterial.Columnas.Add(ColComentarios);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarSolicitudMaterial";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridSolicitudMaterial.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(Page.GetType(), "grdEntregaMaterial", GridSolicitudMaterial.GeneraGrid(), true);

        //Grids
        PintaGridConceptosConsultar();
        PintaGridConceptosEditar();
        PintaGridSalidaMaterial();
        PintaGridDetalleSalidaMaterial();
        PintaGridDetalleSalidaMaterialConsular();
    }

    public void PintaGridConceptosConsultar()
    {
        //GridPartidasSolicitudMaterialConsultar
        CJQGrid grdPartidasSolicitudMaterialConsultar = new CJQGrid();
        grdPartidasSolicitudMaterialConsultar.NombreTabla = "grdPartidasSolicitudMaterialConsultar";
        grdPartidasSolicitudMaterialConsultar.CampoIdentificador = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialConsultar.ColumnaOrdenacion = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialConsultar.TipoOrdenacion = "DESC";
        grdPartidasSolicitudMaterialConsultar.Metodo = "ObtenerSolicitudEntregaMaterialConceptosConsultar";
        grdPartidasSolicitudMaterialConsultar.TituloTabla = "Conceptos";
        grdPartidasSolicitudMaterialConsultar.GenerarGridCargaInicial = false;
        grdPartidasSolicitudMaterialConsultar.GenerarFuncionFiltro = false;
        grdPartidasSolicitudMaterialConsultar.GenerarFuncionTerminado = false;
        grdPartidasSolicitudMaterialConsultar.Altura = 120;
        grdPartidasSolicitudMaterialConsultar.Ancho = 670;
        grdPartidasSolicitudMaterialConsultar.NumeroRegistros = 15;
        grdPartidasSolicitudMaterialConsultar.RangoNumeroRegistros = "15,30,60";

        //IdSolcitudMaterial
        CJQColumn ColIdSolicitudMateriall = new CJQColumn();
        ColIdSolicitudMateriall.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Oculto = "true";
        ColIdSolicitudMateriall.Encabezado = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Buscador = "false";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColIdSolicitudMateriall);

        //IdSolcitudMaterialConcepto
        CJQColumn ColIdSolicitudMaterialConcepto = new CJQColumn();
        ColIdSolicitudMaterialConcepto.Nombre = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Oculto = "true";
        ColIdSolicitudMaterialConcepto.Encabezado = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Buscador = "false";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColIdSolicitudMaterialConcepto);

        //Clave Interna
        CJQColumn ColClaveInterna = new CJQColumn();
        ColClaveInterna.Nombre = "ClaveInterna";
        ColClaveInterna.Encabezado = "Clave Interna";
        ColClaveInterna.Buscador = "false";
        ColClaveInterna.Alineacion = "left";
        ColClaveInterna.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColClaveInterna);

        //Numero Parte
        CJQColumn ColNumeroParte = new CJQColumn();
        ColNumeroParte.Nombre = "NumeroParte";
        ColNumeroParte.Encabezado = "Numero Parte";
        ColNumeroParte.Buscador = "false";
        ColNumeroParte.Alineacion = "left";
        ColNumeroParte.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColNumeroParte);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        ColCantidad.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColCantidad);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Division";
        ColDivision.Buscador = "false";
        ColDivision.Alineacion = "left";
        ColDivision.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDivision);

        //Clave SAT
        CJQColumn ColClaveProdServ = new CJQColumn();
        ColClaveProdServ.Nombre = "ClaveProdServ";
        ColClaveProdServ.Encabezado = "Clave [SAT]";
        ColClaveProdServ.Buscador = "false";
        ColClaveProdServ.Alineacion = "left";
        ColClaveProdServ.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColClaveProdServ);

        //Disponible Inventario
        CJQColumn ColDisponibleInventario = new CJQColumn();
        ColDisponibleInventario.Nombre = "DisponibleInventario";
        ColDisponibleInventario.Encabezado = "Inventario";
        ColDisponibleInventario.Buscador = "false";
        ColDisponibleInventario.Alineacion = "left";
        ColDisponibleInventario.Ancho = "80";
        grdPartidasSolicitudMaterialConsultar.Columnas.Add(ColDisponibleInventario);
        
        ClientScript.RegisterStartupScript(this.GetType(), "grdPartidasSolicitudMaterialConsultar", grdPartidasSolicitudMaterialConsultar.GeneraGrid(), true);
    }

    public void PintaGridConceptosEditar()
    {
        //GridPartidasSolicitudMaterialConsultar
        CJQGrid grdPartidasSolicitudMaterialEditar = new CJQGrid();
        grdPartidasSolicitudMaterialEditar.NombreTabla = "grdPartidasSolicitudMaterialEditar";
        grdPartidasSolicitudMaterialEditar.CampoIdentificador = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialEditar.ColumnaOrdenacion = "IdSolicitudMaterialConcepto";
        grdPartidasSolicitudMaterialEditar.TipoOrdenacion = "DESC";
        grdPartidasSolicitudMaterialEditar.Metodo = "ObtenerSolicitudEntregaMaterialConceptosEditar";
        grdPartidasSolicitudMaterialEditar.TituloTabla = "Conceptos";
        grdPartidasSolicitudMaterialEditar.GenerarGridCargaInicial = false;
        grdPartidasSolicitudMaterialEditar.GenerarFuncionFiltro = false;
        grdPartidasSolicitudMaterialEditar.GenerarFuncionTerminado = false;
        grdPartidasSolicitudMaterialEditar.Altura = 120;
        grdPartidasSolicitudMaterialEditar.Ancho = 670;
        grdPartidasSolicitudMaterialEditar.NumeroRegistros = 15;
        grdPartidasSolicitudMaterialEditar.RangoNumeroRegistros = "15,30,60";

        //IdSolcitudMaterial
        CJQColumn ColIdSolicitudMateriall = new CJQColumn();
        ColIdSolicitudMateriall.Nombre = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Oculto = "true";
        ColIdSolicitudMateriall.Encabezado = "IdSolicitudMaterial";
        ColIdSolicitudMateriall.Buscador = "false";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColIdSolicitudMateriall);

        //IdSolcitudMaterialConcepto
        CJQColumn ColIdSolicitudMaterialConcepto = new CJQColumn();
        ColIdSolicitudMaterialConcepto.Nombre = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Oculto = "true";
        ColIdSolicitudMaterialConcepto.Encabezado = "IdSolicitudMaterialConcepto";
        ColIdSolicitudMaterialConcepto.Buscador = "false";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColIdSolicitudMaterialConcepto);

        //Clave Interna
        CJQColumn ColClaveInterna = new CJQColumn();
        ColClaveInterna.Nombre = "ClaveInterna";
        ColClaveInterna.Encabezado = "Clave Interna";
        ColClaveInterna.Buscador = "false";
        ColClaveInterna.Alineacion = "left";
        ColClaveInterna.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColClaveInterna);

        //Numero Parte
        CJQColumn ColNumeroParte = new CJQColumn();
        ColNumeroParte.Nombre = "NumeroParte";
        ColNumeroParte.Encabezado = "Numero Parte";
        ColNumeroParte.Buscador = "false";
        ColNumeroParte.Alineacion = "left";
        ColNumeroParte.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColNumeroParte);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripcion";
        ColDescripcion.Buscador = "false";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColDescripcion);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "false";
        ColCantidad.Alineacion = "left";
        ColCantidad.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColCantidad);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "Division";
        ColDivision.Buscador = "false";
        ColDivision.Alineacion = "left";
        ColDivision.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColDivision);

        //Clave SAT
        CJQColumn ColClaveProdServ = new CJQColumn();
        ColClaveProdServ.Nombre = "ClaveProdServ";
        ColClaveProdServ.Encabezado = "Clave [SAT]";
        ColClaveProdServ.Buscador = "false";
        ColClaveProdServ.Alineacion = "left";
        ColClaveProdServ.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColClaveProdServ);

        //Disponible Inventario
        CJQColumn ColDisponibleInventario = new CJQColumn();
        ColDisponibleInventario.Nombre = "DisponibleInventario";
        ColDisponibleInventario.Encabezado = "Inventario";
        ColDisponibleInventario.Buscador = "false";
        ColDisponibleInventario.Alineacion = "left";
        ColDisponibleInventario.Ancho = "80";
        grdPartidasSolicitudMaterialEditar.Columnas.Add(ColDisponibleInventario);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPartidasSolicitudMaterialEditar", grdPartidasSolicitudMaterialEditar.GeneraGrid(), true);
    }

    public void PintaGridSalidaMaterial()
    {
        //GridReingresoMaterial
        CJQGrid GridSalidaMaterial = new CJQGrid();
        GridSalidaMaterial.NombreTabla = "grdSalidaMaterial";
        GridSalidaMaterial.CampoIdentificador = "IdSalidaMaterial";
        GridSalidaMaterial.ColumnaOrdenacion = "IdSalidaMaterial";
        GridSalidaMaterial.Metodo = "ObtenerSalidaMaterial";
        GridSalidaMaterial.TituloTabla = "Salidas Material";
        GridSalidaMaterial.Ancho = 870;
        GridSalidaMaterial.GenerarFuncionFiltro = false;

        //Id
        CJQColumn ColIdSalidaMaterial = new CJQColumn();
        ColIdSalidaMaterial.Nombre = "IdSalidaMaterial";
        ColIdSalidaMaterial.Oculto = "false";
        ColIdSalidaMaterial.Encabezado = "IdSalidaMaterial";
        ColIdSalidaMaterial.Buscador = "false";
        ColIdSalidaMaterial.Ancho = "5";
        GridSalidaMaterial.Columnas.Add(ColIdSalidaMaterial);

        //Cliente
        CJQColumn ColClienteSalidaMaterial = new CJQColumn();
        ColClienteSalidaMaterial.Nombre = "RazonSocialS";
        ColClienteSalidaMaterial.Encabezado = "Razón social";
        ColClienteSalidaMaterial.Ancho = "120";
        ColClienteSalidaMaterial.Alineacion = "left";
        GridSalidaMaterial.Columnas.Add(ColClienteSalidaMaterial);

        //FechaAlta
        CJQColumn ColFechaAlta = new CJQColumn();
        ColFechaAlta.Nombre = "Fecha";
        ColFechaAlta.Encabezado = "Fecha";
        ColFechaAlta.Ancho = "40";
        ColFechaAlta.Alineacion = "left";
        ColFechaAlta.Buscador = "false";
        GridSalidaMaterial.Columnas.Add(ColFechaAlta);

        //Baja
        CJQColumn ColBajaSalida = new CJQColumn();
        ColBajaSalida.Nombre = "AI";
        ColBajaSalida.Encabezado = "A/I";
        ColBajaSalida.Ordenable = "false";
        ColBajaSalida.Etiquetado = "A/I";
        ColBajaSalida.Ancho = "30";
        ColBajaSalida.Buscador = "true";
        ColBajaSalida.TipoBuscador = "Combo";
        ColBajaSalida.Oculto = "true";
        //ColBajaReingreso.Oculto = puedeEliminarEncabezadoFacturaProveedor == 1 ? "false" : "true";
        ColBajaSalida.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridSalidaMaterial.Columnas.Add(ColBajaSalida);

        //Consultar
        CJQColumn ColConsultarSalidaMaterial = new CJQColumn();
        ColConsultarSalidaMaterial.Nombre = "Consultar";
        ColConsultarSalidaMaterial.Encabezado = "Ver";
        ColConsultarSalidaMaterial.Etiquetado = "ImagenConsultar";
        ColConsultarSalidaMaterial.Estilo = "divImagenConsultar imgFormaConsultarSalidaMaterial";
        ColConsultarSalidaMaterial.Buscador = "false";
        ColConsultarSalidaMaterial.Ordenable = "false";
        ColConsultarSalidaMaterial.Ancho = "25";
        GridSalidaMaterial.Columnas.Add(ColConsultarSalidaMaterial);

        ClientScript.RegisterStartupScript(this.GetType(), "grdSalidaMaterial", GridSalidaMaterial.GeneraGrid(), true);
    }

    public void PintaGridDetalleSalidaMaterial() {

        //GridDetalleReingresoMaterial
        CJQGrid grdDetalleSalidaMaterial = new CJQGrid();
        grdDetalleSalidaMaterial.NombreTabla = "grdDetalleSalidaMaterial";
        grdDetalleSalidaMaterial.CampoIdentificador = "IdDetalleSalidaMaterial";
        grdDetalleSalidaMaterial.TipoOrdenacion = "DESC";
        grdDetalleSalidaMaterial.Metodo = "ObtenerDetalleSalidaMaterial";
        grdDetalleSalidaMaterial.ColumnaOrdenacion = "IdDetalleSalidaMaterial";
        grdDetalleSalidaMaterial.TituloTabla = "Detalle Salidas Material";
        grdDetalleSalidaMaterial.GenerarGridCargaInicial = false;
        grdDetalleSalidaMaterial.GenerarFuncionFiltro = false;
        grdDetalleSalidaMaterial.GenerarFuncionTerminado = false;
        grdDetalleSalidaMaterial.Altura = 150;
        grdDetalleSalidaMaterial.Ancho = 870;
        grdDetalleSalidaMaterial.NumeroRegistros = 15;
        grdDetalleSalidaMaterial.RangoNumeroRegistros = "15,30,60";

        //IdDetalleFacturaProveedor
        CJQColumn ColIdDetalleSalidaMaterial = new CJQColumn();
        ColIdDetalleSalidaMaterial.Nombre = "IdDetalleSalidaMaterial";
        ColIdDetalleSalidaMaterial.Oculto = "true";
        ColIdDetalleSalidaMaterial.Encabezado = "IdDetalleSalidaMaterial";
        ColIdDetalleSalidaMaterial.Buscador = "false";
        grdDetalleSalidaMaterial.Columnas.Add(ColIdDetalleSalidaMaterial);

        //ClaveInterna
        CJQColumn ColClaveInternaSalidaMaterial = new CJQColumn();
        ColClaveInternaSalidaMaterial.Nombre = "Clave Interna";
        ColClaveInternaSalidaMaterial.Encabezado = "ClaveInterna";
        ColClaveInternaSalidaMaterial.Buscador = "false";
        ColClaveInternaSalidaMaterial.Alineacion = "left";
        ColClaveInternaSalidaMaterial.Ancho = "90";
        grdDetalleSalidaMaterial.Columnas.Add(ColClaveInternaSalidaMaterial);

        //Descripcion
        CJQColumn ColDescripcionSalidaMaterial = new CJQColumn();
        ColDescripcionSalidaMaterial.Nombre = "Descripcion";
        ColDescripcionSalidaMaterial.Encabezado = "Descripción";
        ColDescripcionSalidaMaterial.Buscador = "false";
        ColDescripcionSalidaMaterial.Alineacion = "left";
        ColDescripcionSalidaMaterial.Ancho = "90";
        grdDetalleSalidaMaterial.Columnas.Add(ColDescripcionSalidaMaterial);

        //Cantidad
        CJQColumn ColCantidadSalidaMaterial = new CJQColumn();
        ColCantidadSalidaMaterial.Nombre = "Cantidad";
        ColCantidadSalidaMaterial.Encabezado = "Cantidad";
        ColCantidadSalidaMaterial.Buscador = "false";
        ColCantidadSalidaMaterial.Alineacion = "left";
        ColCantidadSalidaMaterial.Ancho = "30";
        grdDetalleSalidaMaterial.Columnas.Add(ColCantidadSalidaMaterial);

        //Eliminar concepto factura de proveedor consultar
        CJQColumn ColEliminarConceptoSalidaMaterial = new CJQColumn();
        ColEliminarConceptoSalidaMaterial.Nombre = "Eliminar";
        ColEliminarConceptoSalidaMaterial.Encabezado = "Eliminar";
        ColEliminarConceptoSalidaMaterial.Etiquetado = "Imagen";
        ColEliminarConceptoSalidaMaterial.Imagen = "eliminar.png";
        ColEliminarConceptoSalidaMaterial.Estilo = "divImagenConsultar imgEliminarConceptoSalidaMaterial";
        ColEliminarConceptoSalidaMaterial.Buscador = "false";
        ColEliminarConceptoSalidaMaterial.Ordenable = "false";
        ColEliminarConceptoSalidaMaterial.Ancho = "60";
        grdDetalleSalidaMaterial.Columnas.Add(ColEliminarConceptoSalidaMaterial);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleSalidaMaterial", grdDetalleSalidaMaterial.GeneraGrid(), true);
    }

    public void PintaGridDetalleSalidaMaterialConsular()
    {
        //GridDetalleSalidaMaterialConsultar
        CJQGrid grdDetalleSalidaMaterialConsultar = new CJQGrid();
        grdDetalleSalidaMaterialConsultar.NombreTabla = "grdDetalleSalidaMaterialConsultar";
        grdDetalleSalidaMaterialConsultar.ColumnaOrdenacion = "IdDetalleSalidaMaterial";
        grdDetalleSalidaMaterialConsultar.TipoOrdenacion = "DESC";
        grdDetalleSalidaMaterialConsultar.Metodo = "ObtenerDetalleSalidaMaterialConsultar";
        grdDetalleSalidaMaterialConsultar.TituloTabla = "Detalle Salida Material";
        grdDetalleSalidaMaterialConsultar.GenerarGridCargaInicial = false;
        grdDetalleSalidaMaterialConsultar.GenerarFuncionFiltro = false;
        grdDetalleSalidaMaterialConsultar.GenerarFuncionTerminado = false;
        grdDetalleSalidaMaterialConsultar.Altura = 150;
        grdDetalleSalidaMaterialConsultar.Ancho = 870;
        grdDetalleSalidaMaterialConsultar.NumeroRegistros = 15;
        grdDetalleSalidaMaterialConsultar.RangoNumeroRegistros = "15,30,60";

        //Id
        CJQColumn ColIdDetalleSalidaMaterialConsultar = new CJQColumn();
        ColIdDetalleSalidaMaterialConsultar.Nombre = "IdDetalleSalidaMaterial";
        ColIdDetalleSalidaMaterialConsultar.Oculto = "true";
        ColIdDetalleSalidaMaterialConsultar.Encabezado = "IdDetalleSalidaMaterial";
        ColIdDetalleSalidaMaterialConsultar.Buscador = "false";
        grdDetalleSalidaMaterialConsultar.Columnas.Add(ColIdDetalleSalidaMaterialConsultar);

        //ClaveInterna
        CJQColumn ColClaveInternaSalidaMaterialConsultar = new CJQColumn();
        ColClaveInternaSalidaMaterialConsultar.Nombre = "Clave Interna";
        ColClaveInternaSalidaMaterialConsultar.Encabezado = "ClaveInterna";
        ColClaveInternaSalidaMaterialConsultar.Buscador = "false";
        ColClaveInternaSalidaMaterialConsultar.Alineacion = "left";
        ColClaveInternaSalidaMaterialConsultar.Ancho = "90";
        grdDetalleSalidaMaterialConsultar.Columnas.Add(ColClaveInternaSalidaMaterialConsultar);

        //Descripcion
        CJQColumn ColDescripcionSalidaMaterialConsultar = new CJQColumn();
        ColDescripcionSalidaMaterialConsultar.Nombre = "Descripcion";
        ColDescripcionSalidaMaterialConsultar.Encabezado = "Descripción";
        ColDescripcionSalidaMaterialConsultar.Buscador = "false";
        ColDescripcionSalidaMaterialConsultar.Alineacion = "left";
        ColDescripcionSalidaMaterialConsultar.Ancho = "90";
        grdDetalleSalidaMaterialConsultar.Columnas.Add(ColDescripcionSalidaMaterialConsultar);

        //Cantidad
        CJQColumn ColCantidadSalidaMaterialConsultar = new CJQColumn();
        ColCantidadSalidaMaterialConsultar.Nombre = "Cantidad";
        ColCantidadSalidaMaterialConsultar.Encabezado = "Cantidad";
        ColCantidadSalidaMaterialConsultar.Buscador = "false";
        ColCantidadSalidaMaterialConsultar.Alineacion = "left";
        ColCantidadSalidaMaterialConsultar.Ancho = "30";
        grdDetalleSalidaMaterialConsultar.Columnas.Add(ColCantidadSalidaMaterialConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetalleSalidaMaterialConsultar", grdDetalleSalidaMaterialConsultar.GeneraGrid(), true);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSalidaMaterial(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFechaInicial, string pFechaFinal, int pPorFecha, string pFolio)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaMaterial", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleSalidaMaterial(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSalidaMaterial)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleSalidaMaterialConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSalidaMaterial", SqlDbType.Int).Value = pIdSalidaMaterial;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetalleSalidaMaterialConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdSalidaMaterial)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetalleSalidaMaterialConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSalidaMaterial", SqlDbType.Int).Value = pIdSalidaMaterial;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterial(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdSolicitudMaterial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial", sqlCon);

        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterialConceptosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdSolicitudMaterial)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial_Conceptos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerSolicitudEntregaMaterialConceptosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pIdSolicitudMaterial)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdSalidaEntregaMaterial_Conceptos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdSolicitudMaterial", SqlDbType.VarChar, 50).Value = pIdSolicitudMaterial;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string ObtenerFormaSolicitudEntregaMaterial(int pIdSolicitudMaterial)
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarSalidaEntregaMaterial = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                //Permisos
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarSalidaEntregaMaterial" }, pConexion) == "")
                {
                    puedeEditarSalidaEntregaMaterial = 1;
                }
                oPermisos.Add("puedeEditarSalidaEntregaMaterial", puedeEditarSalidaEntregaMaterial);

                Modelo.Add("Permisos", oPermisos);


                CSolicitudMaterial solicitudMaterial = new CSolicitudMaterial();
                solicitudMaterial.LlenaObjeto(pIdSolicitudMaterial, pConexion);

                Modelo.Add("IdSolicitudMaterial", solicitudMaterial.IdSolicitudMaterial);
                Modelo.Add("FechaAlta",Convert.ToString(solicitudMaterial.FechaAlta.ToShortDateString()));

                Modelo.Add("DescripcionEntrega", solicitudMaterial.DescripcionEntrega);
                Modelo.Add("Confirmado", solicitudMaterial.Aprobar);
                Modelo.Add("Comentarios", solicitudMaterial.Comentarios);

                CUsuario solicitante = new CUsuario();
                solicitante.LlenaObjeto(solicitudMaterial.IdUsuarioCreador, pConexion);
                Modelo.Add("Solicitante", solicitante.Nombre+" "+solicitante.ApellidoPaterno+" "+solicitante.ApellidoMaterno);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudMaterial.IdOportunidad, pConexion);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(oportunidad.IdCliente, pConexion);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add("RazonSocial", organizacion.RazonSocial);
                Modelo.Add("RFC", organizacion.RFC);

                Modelo.Add("Oportunidad", oportunidad.Oportunidad);

                CUsuario agente = new CUsuario();
                agente.LlenaObjeto(oportunidad.IdUsuarioCreacion, pConexion);

                Modelo.Add("Agente", agente.Nombre+" "+agente.ApellidoPaterno+" "+agente.ApellidoMaterno);

                CDivision division = new CDivision();
                division.LlenaObjeto(oportunidad.IdDivision, pConexion);

                Modelo.Add("Division", division.Division);
              
                
                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarSolicitudEntregaMaterial(int IdSolicitudMaterial)
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarSalidaEntregaMaterial = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                //Permisos
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarSalidaEntregaMaterial" }, pConexion) == "")
                {
                    puedeEditarSalidaEntregaMaterial = 1;
                }
                oPermisos.Add("puedeEditarSalidaEntregaMaterial", puedeEditarSalidaEntregaMaterial);

                Modelo.Add("Permisos", oPermisos);


                CSolicitudMaterial solicitudMaterial = new CSolicitudMaterial();
                solicitudMaterial.LlenaObjeto(IdSolicitudMaterial, pConexion);

                Modelo.Add("IdSolicitudMaterial", solicitudMaterial.IdSolicitudMaterial);
                Modelo.Add("FechaAlta", Convert.ToString(solicitudMaterial.FechaAlta.ToShortDateString()));
                Modelo.Add("Confirmado", solicitudMaterial.Aprobar);
                Modelo.Add("Comentarios", solicitudMaterial.Comentarios);
                Modelo.Add("DescripcionEntrega", solicitudMaterial.DescripcionEntrega);
                CUsuario solicitante = new CUsuario();
                solicitante.LlenaObjeto(solicitudMaterial.IdUsuarioCreador, pConexion);
                Modelo.Add("Solicitante", solicitante.Nombre + " " + solicitante.ApellidoPaterno + " " + solicitante.ApellidoMaterno);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudMaterial.IdOportunidad, pConexion);

                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(oportunidad.IdCliente, pConexion);

                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, pConexion);

                Modelo.Add("RazonSocial", organizacion.RazonSocial);
                Modelo.Add("RFC", organizacion.RFC);

                Modelo.Add("Oportunidad", oportunidad.Oportunidad);

                CUsuario agente = new CUsuario();
                agente.LlenaObjeto(oportunidad.IdUsuarioCreacion, pConexion);

                Modelo.Add("Agente", agente.Nombre + " " + agente.ApellidoPaterno + " " + agente.ApellidoMaterno);

                CDivision division = new CDivision();
                division.LlenaObjeto(oportunidad.IdDivision, pConexion);

                Modelo.Add("Division", division.Division);


                Respuesta.Add("Modelo", Modelo);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarSolicitudEntregaMaterial(int IdSolicitudMaterial, int Aprobar, string Comentarios)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                Dictionary<string, object> pParametros = new Dictionary<string, object>();

                if (Convert.ToBoolean(Aprobar))
                {
                    CSolicitudMaterial solicitudMaterial = new CSolicitudMaterial();
                    solicitudMaterial.LlenaObjeto(IdSolicitudMaterial, pConexion);

                    pParametros.Add("IdSolicitudMaterial", solicitudMaterial.IdSolicitudMaterial);
                    CSolicitudMaterialConcepto solicitudMaterialConcepto = new CSolicitudMaterialConcepto();

                    foreach (CSolicitudMaterialConcepto concepto in solicitudMaterialConcepto.LlenaObjetosFiltros(pParametros, pConexion))
                    {
                        CPresupuestoConcepto presupuestoConcepto = new CPresupuestoConcepto();
                        presupuestoConcepto.LlenaObjeto(concepto.IdPresupuestoConcepto, pConexion);

                        pParametros.Clear();
                        pParametros.Add("IdProducto", presupuestoConcepto.IdProducto);

                        CExistenciaReal inventario = new CExistenciaReal();
                        inventario.LlenaObjetoFiltros(pParametros, pConexion);

                        inventario.CantidadFinal = inventario.CantidadFinal - concepto.Cantidad;

                        inventario.Editar(pConexion);

                    }

                    solicitudMaterial.FechaEntrega = DateTime.Now;
                    solicitudMaterial.Aprobar = Convert.ToBoolean(Aprobar);
                    solicitudMaterial.Comentarios = Convert.ToString(Comentarios);
                    solicitudMaterial.Editar(pConexion);
                }
                else
                {
                    Error = 1;
                    DescripcionError = "No se ha confirmado, por lo cual no impactara la salida de producto en el inventario.";
                }
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string Imprimir(int IdSolicitudMaterial)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                JObject Modelo = new JObject();
                
                CSolicitudMaterial solicitudMaterial = new CSolicitudMaterial();
                solicitudMaterial.LlenaObjeto(IdSolicitudMaterial, pConexion);

                int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(IdEmpresa, pConexion);

                CMunicipio Municipio = new CMunicipio();
                Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);

                CEstado Estado = new CEstado();
                Estado.LlenaObjeto(Municipio.IdEstado, pConexion);

                Modelo.Add("TIPODOCUMENTO", "Entrega Material");
                Modelo.Add("FOLIO", IdSolicitudMaterial);
                Modelo.Add("RAZONSOCIALEMISOR", Empresa.RazonSocial);
                Modelo.Add("RFCEMISOR", Empresa.RFC);
                Modelo.Add("CALLEEMISOR", Empresa.Calle);
                Modelo.Add("COLONIAEMISOR", Empresa.Colonia);
                Modelo.Add("CODIGOPOSTALEMISOR", Empresa.CodigoPostal);
                Modelo.Add("MUNICIPIOEMISOR", Municipio.Municipio);
                Modelo.Add("ESTADOEMISOR", Estado.Estado);

                COportunidad oportunidad = new COportunidad();
                oportunidad.LlenaObjeto(solicitudMaterial.IdOportunidad, pConexion);

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(oportunidad.IdCliente, pConexion);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
                Dictionary<string, object> pParametros = new Dictionary<string, object>();

                CDireccionOrganizacion direccionOrganizacion = new CDireccionOrganizacion();
                pParametros.Add("IdOrganizacion", Organizacion.IdOrganizacion);
                direccionOrganizacion.LlenaObjetoFiltros(pParametros, pConexion);

                CMunicipio municipioC = new CMunicipio();
                municipioC.LlenaObjeto(direccionOrganizacion.IdMunicipio, pConexion);

                CEstado estadoC = new CEstado();
                estadoC.LlenaObjeto(municipioC.IdEstado, pConexion);

                CPais paisC = new CPais();
                paisC.LlenaObjeto(estadoC.IdPais, pConexion);

                Modelo.Add("RAZONSOCIALRECEPTOR", Organizacion.RazonSocial);
                Modelo.Add("RFCRECEPTOR", Organizacion.RFC);
                Modelo.Add("CALLERECEPTOR", direccionOrganizacion.Calle);
                Modelo.Add("NUMEROEXTERIORRECEPTOR", direccionOrganizacion.NumeroExterior);
                Modelo.Add("REFERENCIARECEPTOR", direccionOrganizacion.Referencia);
                Modelo.Add("COLONIARECEPTOR", direccionOrganizacion.Colonia);
                Modelo.Add("CODIGOPOSTALRECEPTOR", direccionOrganizacion.CodigoPostal);
                Modelo.Add("MUNICIPIORECEPTOR", municipioC.Municipio);
                Modelo.Add("ESTADORECEPTOR", estadoC.Estado);
                Modelo.Add("PAISRECEPTOR", paisC.Pais);
                Modelo.Add("FECHASALIDA", DateTime.Now.Day+"/"+DateTime.Now.Month+"/"+DateTime.Now.Year);

                Modelo.Add("DescripcionEntrega", solicitudMaterial.DescripcionEntrega);

                JArray Conceptos = new JArray();
                CSolicitudMaterialConcepto Detalle = new CSolicitudMaterialConcepto();
                pParametros.Clear();
                pParametros.Add("IdSolicitudMaterial", IdSolicitudMaterial);
                pParametros.Add("Baja", 0);

                foreach (CSolicitudMaterialConcepto Partida in Detalle.LlenaObjetosFiltros(pParametros, pConexion))
                {
                    JObject Concepto = new JObject();
                    CPresupuestoConcepto concepto = new CPresupuestoConcepto();
                    concepto.LlenaObjeto(Partida.IdPresupuestoConcepto, pConexion);
                    
                    Concepto.Add("CANTIDADENTREGA", Partida.Cantidad);
                    Concepto.Add("DESCRIPCIONDETALLE", concepto.Descripcion);
                    Concepto.Add("RESTANTEDETALLE", 0);

                    Conceptos.Add(Concepto);
                }
                Modelo.Add("Conceptos", Conceptos);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaFiltroSalidaMaterial()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;
        Modelo.Add("FechaInicial", Convert.ToString(Fecha.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(Fecha.ToShortDateString()));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAgregarSalidaMaterial()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            //CSucursal Sucursal = new CSucursal();
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            //Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
            //CTipoCambio TipoCambio = new CTipoCambio();
            DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            //string validacion = ValidarExisteTipoCambio(TipoCambio, Sucursal, Fecha, ConexionBaseDatos);
            //if (validacion == "")
            //{
                //string CuentaContableGenerada = "";

                JObject Modelo = new JObject();
                //Modelo.Add("Divisiones", CSucursalDivision.ObtenerJsonSucursalDivision(Usuario.IdSucursalActual, ConexionBaseDatos));
                //Modelo.Add("Usuarios", CUsuario.ObtenerJsonUsuarioAgenteTodos(Usuario.IdUsuario, ConexionBaseDatos));
                //Modelo.Add("Almacenes", CSucursalAccesoAlmacen.ObtenerJsonSucursalAlmacen(Usuario.IdSucursalActual, ConexionBaseDatos));
                //Modelo.Add(new JProperty("FechaFactura", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("MotivoSalida", ObtenerJsonMotivoSalida(0, ConexionBaseDatos)));

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            //}
            //else
            //{
            //    oRespuesta.Add(new JProperty("Error", 1));
            //    oRespuesta.Add(new JProperty("Descripcion", "No hay tipo de cambio del dia"));
            //}
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaSalidaMaterial(int pIdSalidaMaterial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            CSalidaMaterial salidaMaterial = new CSalidaMaterial();
            salidaMaterial.LlenaObjeto(pIdSalidaMaterial, ConexionBaseDatos);

            string ClienteProyecto = "";
            if (salidaMaterial.IdCliente != 0)
            {
                CCliente cliente = new CCliente();
                cliente.LlenaObjeto(salidaMaterial.IdCliente, ConexionBaseDatos);
                COrganizacion organizacion = new COrganizacion();
                organizacion.LlenaObjeto(cliente.IdOrganizacion, ConexionBaseDatos);
                Modelo.Add(new JProperty("IdCliente", cliente.IdCliente));
                ClienteProyecto = organizacion.RazonSocial;
            }
            else
            {
                CProyecto proyecto = new CProyecto();
                proyecto.LlenaObjeto(salidaMaterial.IdProyecto, ConexionBaseDatos);
                Modelo.Add(new JProperty("IdProyecto", proyecto.IdProyecto));
                ClienteProyecto = proyecto.NombreProyecto;
            }

            CMotivoSalida motivoSalida = new CMotivoSalida();
            motivoSalida.LlenaObjeto(salidaMaterial.IdMotivoSalida, ConexionBaseDatos);

            Modelo.Add(new JProperty("ClienteProyecto", ClienteProyecto));
            Modelo.Add(new JProperty("MotivoSalida", motivoSalida.Descripcion));
            Modelo.Add(new JProperty("IdSalidaMaterial", pIdSalidaMaterial));
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
    public static string AgregarDetalleSalidaMaterialNormal(Dictionary<string, object> pSalidaMaterial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDetalleSalidaMaterial DetalleSalidaMaterial = new CDetalleSalidaMaterial();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CCliente Cliente = new CCliente();
            CProyecto Proyecto = new CProyecto();
            DetalleSalidaMaterial.IdSalidaMaterial = Convert.ToInt32(pSalidaMaterial["IdSalidaMaterial"]);
            if (Convert.ToInt32(pSalidaMaterial["IdProducto"]) != 0)
            {
                Producto.LlenaObjeto(Convert.ToInt32(pSalidaMaterial["IdProducto"]), ConexionBaseDatos);
                DetalleSalidaMaterial.IdProducto = Producto.IdProducto;
                DetalleSalidaMaterial.Descripcion = Convert.ToString(Producto.Producto);
            }
            else
            {
                Servicio.LlenaObjeto(Convert.ToInt32(pSalidaMaterial["IdServicio"]), ConexionBaseDatos);
                DetalleSalidaMaterial.IdServicio = Servicio.IdServicio;
                DetalleSalidaMaterial.Descripcion = Convert.ToString(Servicio.Servicio);
            }
            DetalleSalidaMaterial.Cantidad = Convert.ToDecimal(pSalidaMaterial["Cantidad"]);
            DetalleSalidaMaterial.FechaAlta = Convert.ToDateTime(DateTime.Now);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);


            if (DetalleSalidaMaterial.IdSalidaMaterial != 0)
            {
                DetalleSalidaMaterial.Agregar(ConexionBaseDatos);

                //Reingreso Material afecta a InvetarioReal 
                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdProducto", Convert.ToInt32(DetalleSalidaMaterial.IdProducto));

                CExistenciaReal inventario = new CExistenciaReal();
                inventario.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

                CSalidaMaterial SalidaMaterial = new CSalidaMaterial();
                SalidaMaterial.LlenaObjeto(DetalleSalidaMaterial.IdSalidaMaterial, ConexionBaseDatos);

                if (inventario.IdExistenciaReal != 0)
                {

                    inventario.CantidadInicial = inventario.CantidadFinal;
                    inventario.CantidadFinal = inventario.CantidadFinal - Convert.ToInt32(DetalleSalidaMaterial.Cantidad);
                    // inventario.IdUsuario = Usuario.IdUsuario;
                    //inventario.Costo = Convert.ToInt32(DetalleFacturaProveedor.Precio);
                    //inventario.IdTipoMoneda = Convert.ToInt32(facturaProveedor.IdTipoMoneda);
                    //inventario.IdAlmacen = DetalleFacturaProveedor.IdAlmacen;
                    inventario.IdSucursal = Usuario.IdSucursalActual;
                    inventario.Editar(ConexionBaseDatos);
                }
                else
                {

                    inventario.IdProducto = DetalleSalidaMaterial.IdProducto;
                    inventario.Fecha = DateTime.Now;
                    inventario.CantidadInicial = inventario.CantidadFinal;
                    inventario.CantidadFinal = inventario.CantidadFinal - Convert.ToInt32(DetalleSalidaMaterial.Cantidad);
                    inventario.IdUsuario = Usuario.IdUsuario;
                    //inventario.Costo = Convert.ToInt32(DetalleFacturaProveedor.Precio);
                    //inventario.IdTipoMoneda = Convert.ToInt32(facturaProveedor.IdTipoMoneda);
                    //inventario.IdAlmacen = DetalleFacturaProveedor.IdAlmacen;
                    inventario.IdSucursal = Usuario.IdSucursalActual;
                    inventario.Agregar(ConexionBaseDatos);
                }

                CExistenciaHistorico inventarioHistorico = new CExistenciaHistorico();
                inventarioHistorico.IdProducto = inventario.IdProducto;
                inventarioHistorico.Fecha = DateTime.Now;
                inventarioHistorico.CantidadInicial = inventario.CantidadInicial;
                inventarioHistorico.CantidadFinal = inventario.CantidadFinal;
                inventarioHistorico.IdUsuario = Usuario.IdUsuario;
                inventarioHistorico.Costo = inventario.Costo;
                inventarioHistorico.IdExistenciaReal = inventario.IdExistenciaReal;
                inventarioHistorico.IdAlmacen = inventario.IdAlmacen;
                inventarioHistorico.IdSucursal = inventario.IdSucursal;
                inventarioHistorico.Agregar(ConexionBaseDatos);
            }
            else
            {
                CSalidaMaterial salidaMaterial = new CSalidaMaterial();
                salidaMaterial.IdCliente = Convert.ToInt32(pSalidaMaterial["IdCliente"]);
                salidaMaterial.IdProyecto = Convert.ToInt32(pSalidaMaterial["IdProyecto"]);
                salidaMaterial.IdMotivoSalida = Convert.ToInt32(pSalidaMaterial["IdMotivoSalida"]);
                salidaMaterial.FechaAlta = Convert.ToDateTime(DateTime.Now);
                salidaMaterial.IdUsuarioAlta = Usuario.IdUsuario;
                salidaMaterial.Agregar(ConexionBaseDatos);

                DetalleSalidaMaterial.IdSalidaMaterial = salidaMaterial.IdSalidaMaterial;
                DetalleSalidaMaterial.Agregar(ConexionBaseDatos);


                //Salida Material afecta a InvetarioReal 
                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdProducto", Convert.ToInt32(DetalleSalidaMaterial.IdProducto));

                CExistenciaReal inventario = new CExistenciaReal();
                inventario.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

                salidaMaterial = new CSalidaMaterial();
                salidaMaterial.LlenaObjeto(DetalleSalidaMaterial.IdSalidaMaterial, ConexionBaseDatos);

                if (inventario.IdExistenciaReal != 0)
                {

                    inventario.CantidadInicial = inventario.CantidadFinal;
                    inventario.CantidadFinal = inventario.CantidadFinal - Convert.ToInt32(DetalleSalidaMaterial.Cantidad);
                    // inventario.IdUsuario = Usuario.IdUsuario;
                    //inventario.Costo = Convert.ToInt32(DetalleFacturaProveedor.Precio);
                    //inventario.IdTipoMoneda = Convert.ToInt32(facturaProveedor.IdTipoMoneda);
                    //inventario.IdAlmacen = DetalleFacturaProveedor.IdAlmacen;
                    inventario.IdSucursal = Usuario.IdSucursalActual;
                    inventario.Editar(ConexionBaseDatos);
                }
                else
                {

                    inventario.IdProducto = DetalleSalidaMaterial.IdProducto;
                    inventario.Fecha = DateTime.Now;
                    inventario.CantidadInicial = inventario.CantidadFinal;
                    inventario.CantidadFinal = inventario.CantidadFinal - Convert.ToInt32(DetalleSalidaMaterial.Cantidad);
                    inventario.IdUsuario = Usuario.IdUsuario;
                    //inventario.Costo = Convert.ToInt32(DetalleFacturaProveedor.Precio);
                    //inventario.IdTipoMoneda = Convert.ToInt32(facturaProveedor.IdTipoMoneda);
                    //inventario.IdAlmacen = DetalleFacturaProveedor.IdAlmacen;
                    inventario.IdSucursal = Usuario.IdSucursalActual;
                    //inventario.Agregar(ConexionBaseDatos);
                }

                CExistenciaHistorico inventarioHistorico = new CExistenciaHistorico();
                inventarioHistorico.IdProducto = inventario.IdProducto;
                inventarioHistorico.Fecha = DateTime.Now;
                inventarioHistorico.CantidadInicial = inventario.CantidadInicial;
                inventarioHistorico.CantidadFinal = inventario.CantidadFinal;
                inventarioHistorico.IdUsuario = Usuario.IdUsuario;
                inventarioHistorico.Costo = inventario.Costo;
                inventarioHistorico.IdExistenciaReal = inventario.IdExistenciaReal;
                inventarioHistorico.IdAlmacen = inventario.IdAlmacen;
                inventarioHistorico.IdSucursal = inventario.IdSucursal;
                //inventarioHistorico.Agregar(ConexionBaseDatos);
            }


            oRespuesta.Add("IdSalidaMaterial", DetalleSalidaMaterial.IdSalidaMaterial);
            oRespuesta.Add(new JProperty("Error", 0));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "true"));
        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    [WebMethod]
    public static string EliminarDetalleSalidaMaterial(Dictionary<string, object> pDetalleSalidaMaterial)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();

        CDetalleSalidaMaterial DetalleSalidaMaterial = new CDetalleSalidaMaterial();
        DetalleSalidaMaterial.LlenaObjeto(Convert.ToInt32(pDetalleSalidaMaterial["pIdDetalleSalidaMaterial"]), ConexionBaseDatos);
        DetalleSalidaMaterial.IdDetalleSalidaMaterial = Convert.ToInt32(pDetalleSalidaMaterial["pIdDetalleSalidaMaterial"]);
        DetalleSalidaMaterial.Baja = true;
        DetalleSalidaMaterial.Editar(ConexionBaseDatos);

        JObject oRespuesta = new JObject();

        //SALIDA Material afecta a InvetarioReal 
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdProducto", Convert.ToInt32(DetalleSalidaMaterial.IdProducto));

        CExistenciaReal inventario = new CExistenciaReal();
        inventario.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);

        CSalidaMaterial salidaMaterial = new CSalidaMaterial();
        salidaMaterial.LlenaObjeto(DetalleSalidaMaterial.IdSalidaMaterial, ConexionBaseDatos);

        if (inventario.IdExistenciaReal != 0)
        {

            inventario.CantidadInicial = inventario.CantidadFinal;
            inventario.CantidadFinal = inventario.CantidadFinal + Convert.ToInt32(DetalleSalidaMaterial.Cantidad);
            // inventario.IdUsuario = Usuario.IdUsuario;
            //inventario.Costo = Convert.ToInt32(DetalleFacturaProveedor.Precio);
            //inventario.IdTipoMoneda = Convert.ToInt32(facturaProveedor.IdTipoMoneda);
            //inventario.IdAlmacen = DetalleFacturaProveedor.IdAlmacen;
            inventario.IdSucursal = Usuario.IdSucursalActual;
            inventario.Editar(ConexionBaseDatos);
        }
        else
        {

            inventario.IdProducto = DetalleSalidaMaterial.IdProducto;
            inventario.Fecha = DateTime.Now;
            inventario.CantidadInicial = inventario.CantidadFinal;
            inventario.CantidadFinal = inventario.CantidadFinal + Convert.ToInt32(DetalleSalidaMaterial.Cantidad);
            inventario.IdUsuario = Usuario.IdUsuario;
            //inventario.Costo = Convert.ToInt32(DetalleFacturaProveedor.Precio);
            //inventario.IdTipoMoneda = Convert.ToInt32(facturaProveedor.IdTipoMoneda);
            //inventario.IdAlmacen = DetalleFacturaProveedor.IdAlmacen;
            inventario.IdSucursal = Usuario.IdSucursalActual;
            inventario.Agregar(ConexionBaseDatos);
        }

        CExistenciaHistorico inventarioHistorico = new CExistenciaHistorico();
        inventarioHistorico.IdProducto = inventario.IdProducto;
        inventarioHistorico.Fecha = DateTime.Now;
        inventarioHistorico.CantidadInicial = inventario.CantidadInicial;
        inventarioHistorico.CantidadFinal = inventario.CantidadFinal;
        inventarioHistorico.IdUsuario = Usuario.IdUsuario;
        inventarioHistorico.Costo = inventario.Costo;
        inventarioHistorico.IdExistenciaReal = inventario.IdExistenciaReal;
        inventarioHistorico.IdAlmacen = inventario.IdAlmacen;
        inventarioHistorico.IdSucursal = inventario.IdSucursal;
        inventarioHistorico.Agregar(ConexionBaseDatos);

        oRespuesta.Add(new JProperty("Error", 0));

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    public static JArray ObtenerJsonMotivoSalida(int pIdMotivoSalida, CConexion pConexion)
    {
        CMotivoSalida MotivoSalida = new CMotivoSalida();

        JArray JMotivos = new JArray();
        Dictionary<string, object> ParametrosTI = new Dictionary<string, object>();
        ParametrosTI.Add("Opcion", 1);
        ParametrosTI.Add("Baja", 0);
        //ParametrosTI.Add("IdMotivoReingreso", pIdMotivoReingreso);
        foreach (CMotivoSalida oMotivo in MotivoSalida.LlenaObjetosFiltros(ParametrosTI, pConexion))
        {
            JObject JMotivo = new JObject();
            JMotivo.Add("IdMotivoSalida", oMotivo.IdMotivoSalida);
            JMotivo.Add("Descripcion", oMotivo.Descripcion);
            JMotivos.Add(JMotivo);
        }
        return JMotivos;
    }

    #endregion
}
