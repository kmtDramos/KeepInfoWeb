using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public class CJQGrid
{
    //Atributos
    private string nombreTabla;
    private string metodo;
    private string campoIdentificador;
    private string etiquetaPaginador;
    private int numeroRegistros;
    private string rangoNumeroRegistros;
    private string columnaOrdenacion;
    private string tipoOrdenacion;
    private int ancho;
    private int altura;
    private string tituloTabla;
    private string ocultarRegistros;
    private bool generarFuncionFiltro;
    private bool generarFuncionTerminado;
    private bool generarGridCargaInicial;
    private bool editable;
    private bool numeroFila;
    public List<CJQColumn> Columnas = new List<CJQColumn>();
    private string eventoRegistroSeleccionado;

    //Propiedades
    public string NombreTabla
    {
        get { return nombreTabla; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            nombreTabla = value;
        }
    }

    public string Metodo
    {
        get { return metodo; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            metodo = value;
        }
    }

    public string CampoIdentificador
    {
        get { return campoIdentificador; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            campoIdentificador = value;
        }
    }

    public string EtiquetaPaginador
    {
        get { return etiquetaPaginador; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            etiquetaPaginador = value;
        }
    }

    public int NumeroRegistros
    {
        get { return numeroRegistros; }
        set
        {
            if (value < 0)
            {
                return;
            }
            numeroRegistros = value;
        }
    }

    public string RangoNumeroRegistros
    {
        get { return rangoNumeroRegistros; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            rangoNumeroRegistros = value;
        }
    }

    public string ColumnaOrdenacion
    {
        get { return columnaOrdenacion; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            columnaOrdenacion = value;
        }
    }

    public string TipoOrdenacion
    {
        get { return tipoOrdenacion; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            tipoOrdenacion = value;
        }
    }

    public int Ancho
    {
        get { return ancho; }
        set
        {
            if (value < 0)
            {
                return;
            }
            ancho = value;
        }
    }

    public int Altura
    {
        get { return altura; }
        set
        {
            if (value < 0)
            {
                return;
            }
            altura = value;
        }
    }

    public string TituloTabla
    {
        get { return tituloTabla; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            tituloTabla = value;
        }
    }

    public string OcultarRegistros
    {
        get { return ocultarRegistros; }
        set { ocultarRegistros = value; }
    }

    public bool GenerarFuncionFiltro
    {
        get { return generarFuncionFiltro; }
        set { generarFuncionFiltro = value; }
    }

    public bool GenerarFuncionTerminado
    {
        get { return generarFuncionTerminado; }
        set { generarFuncionTerminado = value; }
    }

    public bool GenerarGridCargaInicial
    {
        get { return generarGridCargaInicial; }
        set { generarGridCargaInicial = value; }
    }

    public bool NumeroFila
    {
        get { return numeroFila; }
        set { numeroFila = value; }
    }

    public bool Editable
    {
        get { return editable; }
        set { editable = value; }
    }

    public string EventoRegistroSeleccionado
    {
        get { return eventoRegistroSeleccionado; }
        set
        {
            if (value.Length == 0)
            {
                return;
            }
            eventoRegistroSeleccionado = value;
        }
    }

    public CJQGrid()
    {
        altura = 190;
        ancho = 930;
        tipoOrdenacion = "asc";
        tituloTabla = "";
        etiquetaPaginador = "";
        numeroRegistros = 10;
        rangoNumeroRegistros = "10,20,30";
        ocultarRegistros = "false";
        generarFuncionFiltro = true;
        generarFuncionTerminado = false;
        generarGridCargaInicial = true;
        numeroFila = false;
        editable = false;
        eventoRegistroSeleccionado = "";
    }

    //Metodos
    public string GeneraGrid()
    {
        string funciones = "";
        if (etiquetaPaginador == "")
        { etiquetaPaginador = "pag" + nombreTabla.Substring(3, nombreTabla.Length - 3); }

        string jsGrid = "";
        if (generarGridCargaInicial)
        {
            jsGrid = "$(document).ready(function() {";
        }
        else
        {
            jsGrid = "function Inicializar_" + nombreTabla + "(){";
        }
        jsGrid = jsGrid + "$('#" + nombreTabla + "').jqGrid(";
        jsGrid = jsGrid + "{";
        jsGrid = jsGrid + "datatype: function(){ Filtro" + nombreTabla.Substring(3, nombreTabla.Length - 3) + "(); },";
        jsGrid = jsGrid + "jsonReader :";
        jsGrid = jsGrid + "{";
        jsGrid = jsGrid + "root: 'Elementos',";
        jsGrid = jsGrid + "page: 'PaginaActual',";
        jsGrid = jsGrid + "total: 'NoPaginas',";
        jsGrid = jsGrid + "records: 'NoRegistros',";
        jsGrid = jsGrid + "repeatitems: true,";
        jsGrid = jsGrid + "cell: 'Row',";
        jsGrid = jsGrid + "id: '" + campoIdentificador + "'},";
        jsGrid = jsGrid + "colModel:[";
        string jsColumnas = "";
        string jsFuncionesFiltros = "$('#" + nombreTabla + "').jqGrid('filterToolbar',{searchOnEnter:true});";
        string jsParametroIniciados = "";
        string jsCadenaParametros = "";
        foreach (CJQColumn CJQColumnas in Columnas)
        {
            jsColumnas = jsColumnas + "{";
            if (CJQColumnas.Nombre != "")
            { jsColumnas = jsColumnas + "name:'" + CJQColumnas.Nombre + "',"; }
            if (CJQColumnas.Editable == "true")
            {
                jsColumnas = jsColumnas + "editable:true,";
                jsColumnas = jsColumnas + "editoptions:{maxlength: 50},";
            }
            if (numeroFila == true)
            {
                jsColumnas = jsColumnas + "rownumbers:true,";
            }
            if (CJQColumnas.EditableTexto == "true")
            {
                jsColumnas = jsColumnas + "editable:true,";
                jsColumnas = jsColumnas + "editoptions:{maxlength: '100', class:'txt_" + nombreTabla + "_" + CJQColumnas.Nombre + "'},";
            }

            if (CJQColumnas.Ancho != "")
            { jsColumnas = jsColumnas + "width:'" + CJQColumnas.Ancho + "',"; }
            if (CJQColumnas.Alineacion != "")
            { jsColumnas = jsColumnas + "align:'" + CJQColumnas.Alineacion + "',"; }
            if (CJQColumnas.Encabezado != "")
            { jsColumnas = jsColumnas + "label:'" + CJQColumnas.Encabezado + "',"; }
            if (CJQColumnas.Oculto != "")
            { jsColumnas = jsColumnas + "hidden:" + CJQColumnas.Oculto + ","; }
            if (CJQColumnas.Formato != "")
            {
                switch (CJQColumnas.Formato)
                {
                    case "FormatoMoneda":
                        jsColumnas = jsColumnas + "formatter:'currency', formatoptions:{decimalSeparator:'.', thousandsSeparator: ',', decimalPlaces: 2, prefix: '$'},";
                        break;
                    case "FormatoPorciento":
                        jsColumnas = jsColumnas + "formatter: 'currency', formatoptions: { decimalPlaces: '0', suffix:'%', defaultValue: '0' },";

                        break;
                    default:
                        break;
                }
            }
            if (CJQColumnas.Ordenable != "")
            { jsColumnas = jsColumnas + "sortable:" + CJQColumnas.Ordenable + ","; }
            if (CJQColumnas.Buscador == "true")
            {
                if (CJQColumnas.TipoBuscador == "Combo")
                {
                    jsColumnas = jsColumnas + "search:" + CJQColumnas.Buscador + ",stype:'select',editoptions:{value:\"" + ObtenerFuncionFiltro(CJQColumnas.TipoBuscador, CJQColumnas.Nombre, CJQColumnas.StoredProcedure) + "\"},";
                    jsParametroIniciados = jsParametroIniciados + " " + ObtenerParametrosIniciados(CJQColumnas.TipoBuscador, CJQColumnas.Nombre);
                    jsCadenaParametros = jsCadenaParametros + ObtenerCadenaParametros(CJQColumnas.TipoBuscador, CJQColumnas.Nombre);
                }
                else
                {
                    jsColumnas = jsColumnas + "search:" + CJQColumnas.Buscador + ",";
                    jsFuncionesFiltros = jsFuncionesFiltros + " " + ObtenerFuncionFiltro(CJQColumnas.TipoBuscador, CJQColumnas.Nombre);
                    jsParametroIniciados = jsParametroIniciados + " " + ObtenerParametrosIniciados(CJQColumnas.TipoBuscador, CJQColumnas.Nombre);
                    jsCadenaParametros = jsCadenaParametros + ObtenerCadenaParametros(CJQColumnas.TipoBuscador, CJQColumnas.Nombre);
                }
            }
            else
            {
                jsColumnas = jsColumnas + "search:" + CJQColumnas.Buscador + ",";
            }
            jsColumnas = jsColumnas.Remove(jsColumnas.Length - 1, 1);
            jsColumnas = jsColumnas + "},";
        }
        jsColumnas = jsColumnas.Remove(jsColumnas.Length - 1, 1);
        jsColumnas = jsColumnas + "],";
        jsGrid = jsGrid + jsColumnas;
        jsGrid = jsGrid + "pager: '#" + etiquetaPaginador + "',";
        jsGrid = jsGrid + "loadtext: 'Cargando datos...',";
        jsGrid = jsGrid + "recordtext: '{0} - {1} de {2} elementos',";
        jsGrid = jsGrid + "emptyrecords: 'No hay resultados',";
        jsGrid = jsGrid + "pgtext : 'Pág: {0} de {1}',";
        jsGrid = jsGrid + "rowNum: '" + numeroRegistros.ToString() + "',";
        jsGrid = jsGrid + "rowList: [" + RangoNumeroRegistros + "],";
        jsGrid = jsGrid + "viewrecords: true,";
        jsGrid = jsGrid + "multiselect: false,";
        jsGrid = jsGrid + "sortname: '" + columnaOrdenacion + "',";
        jsGrid = jsGrid + "sortorder: '" + tipoOrdenacion + "',";
        jsGrid = jsGrid + "width: '" + ancho.ToString() + "',";
        jsGrid = jsGrid + "height: '" + altura.ToString() + "',";
        if (tituloTabla != "")
        {
            jsGrid = jsGrid + "caption: '" + tituloTabla + "',";
            jsGrid = jsGrid + "hiddengrid: " + ocultarRegistros + ",";
        }
        if (numeroFila == true)
        {
            jsGrid = jsGrid + "rownumbers: true,";
        }

        if (eventoRegistroSeleccionado != "")
        {
            jsGrid = jsGrid + "onSelectRow: function(id){" + eventoRegistroSeleccionado + "(id);},";
        }

        if (Editable == true)
        {
            jsGrid = jsGrid + "forceFit: true,";
            jsGrid = jsGrid + "cellEdit: true,";
            jsGrid = jsGrid + "cellsubmit: 'clientArray',";
            jsGrid = jsGrid + "afterSaveCell: function(rowid,name,val,iRow,iCol) {";
            jsGrid = jsGrid + "var valor = jQuery('#" + nombreTabla + "').jqGrid('getCell',rowid,iCol);";
            jsGrid = jsGrid + "var id = jQuery('#" + nombreTabla + "').getCell(rowid,'" + campoIdentificador + "');";
            jsGrid = jsGrid + "Edicion" + nombreTabla.Substring(3, nombreTabla.Length - 3) + "(valor, id, rowid, iCol);";
            jsGrid = jsGrid + "},";
        }

        jsGrid = jsGrid + "gridComplete: function(){";
        jsGrid = jsGrid + "var ids = $('#" + nombreTabla + "').jqGrid('getDataIDs');";
        jsGrid = jsGrid + "for(var i=0;i < ids.length;i++){";
        string jsEtiquetado = "";
        foreach (CJQColumn CJQColumnas in Columnas)
        {
            switch (CJQColumnas.Etiquetado)
            {
                case "CheckBox":
                    jsEtiquetado = jsEtiquetado + "var checkbox" + CJQColumnas.Nombre + "=\"<input type='checkbox' ";
                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "onclick='javascript:" + CJQColumnas.Funcion + "(\"+ $('#" + nombreTabla + " #'+ids[i]+' td').attr('title') +\", this)'"; }
                    if (CJQColumnas.Id != "")
                    { jsEtiquetado = jsEtiquetado + " value=\"+$('#" + nombreTabla + " #'+ids[i]+' td').attr('title')+\" Id='" + CJQColumnas.Id + "' name='" + CJQColumnas.Id + "' " + CJQColumnas.Id + "=\"+$('#" + nombreTabla + " #'+ids[i]+' td').attr('title')+\""; }
                    if (CJQColumnas.Estilo != "")
                    { jsEtiquetado = jsEtiquetado + " class='" + CJQColumnas.Estilo + "'>"; }
                    jsEtiquetado = jsEtiquetado + " \";";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":checkbox" + CJQColumnas.Nombre + "});";
                    break;
                case "CheckBoxchecked":
                    jsEtiquetado = jsEtiquetado + "var checkbox" + CJQColumnas.Nombre + "=\"<input type='checkbox' checked";
                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "onclick='javascript:" + CJQColumnas.Funcion + "(\"+ $('#" + nombreTabla + " #'+ids[i]+' td').attr('title') +\", this)'"; }
                    if (CJQColumnas.Id != "")
                    { jsEtiquetado = jsEtiquetado + " value=\"+$('#" + nombreTabla + " #'+ids[i]+' td').attr('title')+\" Id='" + CJQColumnas.Id + "' name='" + CJQColumnas.Id + "' " + CJQColumnas.Id + "=\"+$('#" + nombreTabla + " #'+ids[i]+' td').attr('title')+\""; }
                    if (CJQColumnas.Estilo != "")
                    { jsEtiquetado = jsEtiquetado + " class='" + CJQColumnas.Estilo + "'>"; }
                    jsEtiquetado = jsEtiquetado + " \";";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":checkbox" + CJQColumnas.Nombre + "});";
                    break;
                case "Correo":
                    jsEtiquetado = jsEtiquetado + "var correo = $('#" + nombreTabla + "').jqGrid('getCell',ids[i],'" + CJQColumnas.Nombre + "'); ";
                    jsEtiquetado = jsEtiquetado + "var etiquetaCorreo = \"<a id='aCorreo\" + ids[i] + \"' href='mailto:\" + correo + \"')' class='" + CJQColumnas.Estilo + "'><span class='" + CJQColumnas.Estilo + "'>\" + correo + \"</span></a>\";";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setCell',ids[i],'" + CJQColumnas.Nombre + "',etiquetaCorreo); ";
                    jsEtiquetado = jsEtiquetado + "$('#aCorreo'+ids[i]).parent().attr('title','Enviar correo a: ' + correo); ";
                    break;
                case "A/I":
                    jsEtiquetado = jsEtiquetado + "var estatusBaja = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";
                    jsEtiquetado = jsEtiquetado + "if(estatusBaja == '0' || estatusBaja == 'False') ";
                    jsEtiquetado = jsEtiquetado + "{ var etiquetaAI = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" baja=\"' + estatusBaja + '\"><img src=\"../images/on.png\" /></div>'; } ";
                    jsEtiquetado = jsEtiquetado + "else ";
                    jsEtiquetado = jsEtiquetado + "{ var etiquetaAI = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" baja=\"' + estatusBaja + '\"><img src=\"../images/off.png\" /></div>'; } ";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiquetaAI}); ";
                    break;
                case "AgregaProductoDetalle":
                    jsEtiquetado = jsEtiquetado + "var estatusSaldo = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";
                    jsEtiquetado = jsEtiquetado + "if(estatusSaldo == '0' || estatusSaldo == 'False') ";
                    jsEtiquetado = jsEtiquetado + "{ var etiquetaAI = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" baja=\"' + estatusSaldo + '\"><img src=\"../images/on.png\" /></div>'; } ";
                    jsEtiquetado = jsEtiquetado + "else ";
                    jsEtiquetado = jsEtiquetado + "{ var etiquetaAI = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" baja=\"' + estatusSaldo + '\"><img src=\"../images/off.png\" /></div>'; } ";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiquetaAI}); ";
                    break;
                case "EstatusInvertido":
                    jsEtiquetado = jsEtiquetado + "var idEstatusCotizacion" + CJQColumnas.Nombre + " = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";

                    jsEtiquetado = jsEtiquetado + "if(idEstatusCotizacion" + CJQColumnas.Nombre + " == '1' ) "; //si borrador 
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + "\" idEstatusCotizacion=\"1\"><img src=\"../images/borrador.png\"  title=\"Borrador\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "else if (idEstatusCotizacion" + CJQColumnas.Nombre + " == '2' ) ";  //si es cotizacion
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" idEstatusCotizacion=\"2\"><img src=\"../images/cotizado.png\" title=\"Cotización: Clic para convertir a Pedido\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "else if (idEstatusCotizacion" + CJQColumnas.Nombre + " == '3' )  ";  //si es pedido
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + "\" idEstatusCotizacion=\"3\"><img src=\"../images/pedido.png\" title=\"Pedido\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "else if (idEstatusCotizacion" + CJQColumnas.Nombre + " == '5' )  ";  //si está vencido
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + "\" idEstatusCotizacion=\"5\"><img src=\"../images/vencido.png\" title=\"Vencido\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "else if (idEstatusCotizacion" + CJQColumnas.Nombre + " == '6' )  ";  //si está vencido
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + "\" idEstatusCotizacion=\"6\"><img src=\"../images/facturado.png\" title=\"Facturado\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "else  ";  //si es cancelado
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + "\" idEstatusCotizacion=\"4\"><img src=\"../images/cancelado.png\" title=\"Baja\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "}); ";
                    break;
                case "EstatusRuta":
                    jsEtiquetado = jsEtiquetado + "var Timbrado" + CJQColumnas.Nombre + " = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";

                    jsEtiquetado = jsEtiquetado + "if(Timbrado" + CJQColumnas.Nombre + " == '0' ) "; //si no esta timbrado 
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" \"><img src=\"../images/rutano.png\"  title=\"Sin ruta\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "else  ";  //si esta timbrado
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" \"><img src=\"../images/rutaok.png\" title=\"Con ruta\" /></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "}); ";
                    break;
                case "EstatusTimbradoFile":
                    jsEtiquetado = jsEtiquetado + "var Timbrado" + CJQColumnas.Nombre + " = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";

                    jsEtiquetado = jsEtiquetado + "if(Timbrado" + CJQColumnas.Nombre + " == '0' ) "; //si no esta timbrado 
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " \"><img src=\"../images/fileno.png\"  title=\"Documento sin timbrar\" style=\"width:20px;height:20px;\"/></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "else  ";  //si esta timbrado
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " \"><img src=\"../images/fileok.png\" title=\"Documento timbrado\" style=\"width:20px;height:20px;\"/></div>'; } ";

                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "}); ";
                    break;
                case "FuncionExterna":
                    jsEtiquetado = jsEtiquetado = "GridCargado();";
                    break;
                case "ImagenConsultar":
                    jsEtiquetado = jsEtiquetado + "var etiqueta" + CJQColumnas.Nombre + "= \"";
                    jsEtiquetado = jsEtiquetado + "<div ";

                    if (CJQColumnas.Estilo != "")
                    { jsEtiquetado = jsEtiquetado + " class='" + CJQColumnas.Estilo + "'>"; }
                    else
                    { jsEtiquetado = jsEtiquetado + ">"; }

                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "<a href='" + CJQColumnas.Funcion + "'>"; }
                    jsEtiquetado = jsEtiquetado + "<img src='../images/view.png' />";

                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "</a>"; }
                    jsEtiquetado = jsEtiquetado + "</div>\";";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "});";
                    break;
                case "ImagenConsultarOC":
                    jsEtiquetado = jsEtiquetado + "if(estatusBaja == '0' || estatusBaja == 'False') ";
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"" + CJQColumnas.Estilo + "\"><img src=\"../images/view.png\" /></div>'; } ";
                    jsEtiquetado = jsEtiquetado + "else";
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = '<div class=\"" + CJQColumnas.Estilo + "_disabled style=\"width:" + CJQColumnas.Ancho + ";\"><img src=\"../images/viewdis.png\"></div>'; } ";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{Consultar:etiqueta" + CJQColumnas.Nombre + "});";
                    break;
                case "Imagen":
                    jsEtiquetado = jsEtiquetado + "var etiqueta" + CJQColumnas.Nombre + "= \"";
                    jsEtiquetado = jsEtiquetado + "<div ";

                    if (CJQColumnas.Estilo != "")
                    { jsEtiquetado = jsEtiquetado + " class='" + CJQColumnas.Estilo + "'>"; }
                    else
                    { jsEtiquetado = jsEtiquetado + ">"; }

                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "<a href='" + CJQColumnas.Funcion + "'>"; }
                    jsEtiquetado = jsEtiquetado + "<img src='../images/" + CJQColumnas.Imagen + "' />";

                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "</a>"; }
                    jsEtiquetado = jsEtiquetado + "</div>\";";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "});";
                    break;
                case "Funcion":
                    if (CJQColumnas.Funcion != "")
                    {
                        jsEtiquetado = jsEtiquetado = "EtiquetarColumnas(i, ids);";
                    }
                    break;
                case "Indicadores":
                    if (CJQColumnas.Etiquetado != "")
                    {
                        jsEtiquetado += "var indicador_" + CJQColumnas.Nombre + " = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";
                        jsEtiquetado += "var etiqueta" + CJQColumnas.Nombre + "=\"<div ";

                        string valorEtiquetado = "$('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html()";

                        if (CJQColumnas.Estilo != "")
                        { jsEtiquetado += " class='" + CJQColumnas.Estilo + "'>"; }
                        jsEtiquetado += "\" + indicador_" + CJQColumnas.Nombre + "+\" </div>\";";
                        jsEtiquetado += "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "}); ";
                    }
                    break;
                case "Calendario":
                    jsEtiquetado = jsEtiquetado + "var " + CJQColumnas.Nombre + " = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";
                    jsEtiquetado = jsEtiquetado + "if(" + CJQColumnas.Nombre + " == '' || " + CJQColumnas.Nombre + " == '&nbsp;') ";
                    jsEtiquetado = jsEtiquetado + "{ var etiqueta" + CJQColumnas.Nombre + " = \"" + "<div ";
                    if (CJQColumnas.Estilo != "")
                    { jsEtiquetado = jsEtiquetado + " class='" + CJQColumnas.Estilo + "'>"; }
                    else
                    { jsEtiquetado = jsEtiquetado + ">"; }

                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "<a href='" + CJQColumnas.Funcion + "'>"; }
                    jsEtiquetado = jsEtiquetado + "<img src='../images/" + CJQColumnas.Imagen + "' />";

                    if (CJQColumnas.Funcion != "")
                    { jsEtiquetado = jsEtiquetado + "</a>"; }
                    jsEtiquetado = jsEtiquetado + "</div>\";";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "});}";

                    jsEtiquetado = jsEtiquetado + "else{";
                    jsEtiquetado = jsEtiquetado + "var etiqueta" + CJQColumnas.Nombre + " = \"" + "<div ";
                    jsEtiquetado = jsEtiquetado + "class='divEditar" + CJQColumnas.Nombre + " cursorPointer'>\"+";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html()+\"";
                    jsEtiquetado = jsEtiquetado + "</div>\";";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "});}";
                    break;
                case "Imprimir":
                    jsEtiquetado = jsEtiquetado + "var imprimir = $('#" + nombreTabla + " #' + ids[i] + ' td[aria-describedby=\"" + nombreTabla + "_" + CJQColumnas.Nombre + "\"]').html();";
                    jsEtiquetado = jsEtiquetado + "if(imprimir == '0' || imprimir == 'False') ";
                    jsEtiquetado = jsEtiquetado + "{ var etiquetaImprimir = '<div class=\"div_" + nombreTabla + "_" + CJQColumnas.Nombre + " cursorPointer\" baja=\"' + imprimir + '\"><img src=\"../images/imprimir.png\" /></div>'; } ";
                    jsEtiquetado = jsEtiquetado + "else ";
                    jsEtiquetado = jsEtiquetado + "{ var etiquetaImprimir = '';";
                    jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiquetaAI}); ";
                    break;
                default:
                    if (CJQColumnas.Etiquetado != "")
                    {
                        jsEtiquetado = jsEtiquetado + "var etiqueta" + CJQColumnas.Nombre + "=\"<div ";
                        if (CJQColumnas.Funcion != "")
                        { jsEtiquetado = jsEtiquetado + "onclick='javascript:" + CJQColumnas.Funcion + "(\"+ $('#" + nombreTabla + " #'+ids[i]+' td').attr('title') +\")'"; }
                        if (CJQColumnas.Estilo != "")
                        { jsEtiquetado = jsEtiquetado + " class='" + CJQColumnas.Estilo + "'>"; }
                        jsEtiquetado = jsEtiquetado + CJQColumnas.Etiquetado + "</div>\";";
                        jsEtiquetado = jsEtiquetado + "$('#" + nombreTabla + "').jqGrid('setRowData',ids[i],{" + CJQColumnas.Nombre + ":etiqueta" + CJQColumnas.Nombre + "});";
                    }
                    break;
            }
        }
        jsEtiquetado = jsEtiquetado + "}";
        if (GenerarFuncionTerminado)
        {
            jsEtiquetado = jsEtiquetado + "Termino_" + nombreTabla + "();}";
        }
        else
        {
            jsEtiquetado = jsEtiquetado + "}";
        }
        jsGrid = jsGrid + jsEtiquetado + "}).navGrid('#" + etiquetaPaginador + "', {edit:false, add:false, search:false, del:false});";
        jsGrid = jsGrid + jsFuncionesFiltros;

        if (generarGridCargaInicial)
        {
            jsGrid = jsGrid + "});";
        }
        else
        {
            jsGrid = jsGrid + "}";
        }

        //Contruye Funcion Filtro
        jsCadenaParametros = "\"{'pTamanoPaginacion':\" + $('#" + nombreTabla + "').getGridParam('rowNum') +\",'pPaginaActual':\" + $('#" + nombreTabla + "').getGridParam('page') +\",'pColumnaOrden':'\" + $('#" + nombreTabla + "').getGridParam('sortname') +\"','pTipoOrden':'\" + $('#" + nombreTabla + "').getGridParam('sortorder') + \"'," + jsCadenaParametros;
        jsCadenaParametros = jsCadenaParametros.Remove(jsCadenaParametros.Length - 1, 1);
        string prueba = jsCadenaParametros.Substring(jsCadenaParametros.Length - 1);
        if (jsCadenaParametros.Substring(jsCadenaParametros.Length - 1) == "'")
        {
            jsCadenaParametros = jsCadenaParametros + "}\"";
        }
        else if (jsCadenaParametros.Substring(jsCadenaParametros.Length - 1) == "\"")
        {
            jsCadenaParametros = jsCadenaParametros + "}\"";
        }
        else
        {
            jsCadenaParametros = jsCadenaParametros + "\"}\"";
        }

        string jsFuncionFiltro = "";
        if (GenerarFuncionFiltro == true)
        {
            jsFuncionFiltro = "function Filtro" + nombreTabla.Substring(3, nombreTabla.Length - 3) + "() {";
            jsFuncionFiltro = jsFuncionFiltro + jsParametroIniciados + " ";
            jsFuncionFiltro = jsFuncionFiltro + "$.ajax({";
            jsFuncionFiltro = jsFuncionFiltro + "url:'" + ObtenerPaginaActual() + "/" + metodo + "',";
            jsFuncionFiltro = jsFuncionFiltro + "data: ";
            jsFuncionFiltro = jsFuncionFiltro + jsCadenaParametros;
            jsFuncionFiltro = jsFuncionFiltro + ",";
            jsFuncionFiltro = jsFuncionFiltro + "dataType: 'json',";
            jsFuncionFiltro = jsFuncionFiltro + "type: 'post',";
            jsFuncionFiltro = jsFuncionFiltro + "contentType: 'application/json; charset=utf-8',";
            jsFuncionFiltro = jsFuncionFiltro + "complete: function(jsondata, stat)";
            jsFuncionFiltro = jsFuncionFiltro + "{";
            jsFuncionFiltro = jsFuncionFiltro + "if (stat == 'success')";
            jsFuncionFiltro = jsFuncionFiltro + "$('#" + nombreTabla + "')[0].addJSONData(JSON.parse(jsondata.responseText).d); ";
            jsFuncionFiltro = jsFuncionFiltro + "else ";
            jsFuncionFiltro = jsFuncionFiltro + "alert(JSON.parse(jsondata.responseText).Message);";
            jsFuncionFiltro = jsFuncionFiltro + "}});}";
        }
        return jsGrid + jsFuncionFiltro + funciones;
    }

    //Metodos

    private string ObtenerFuncionFiltro(string pTipoBuscador, string pNombreColumna)
    {
        string funcionFiltro = "";
        switch (pTipoBuscador)
        {
            case "Autocompletar":
                funcionFiltro = funcionFiltro + "$('#gbox_" + nombreTabla + " #gs_" + pNombreColumna + "').autocomplete({";
                funcionFiltro = funcionFiltro + "source: function( request, response ) {";
                funcionFiltro = funcionFiltro + "var pRequest = \"{'p" + pNombreColumna + "':'\"+$('#gbox_" + NombreTabla + " #gs_" + pNombreColumna + "').val()+\"'}\";";
                funcionFiltro = funcionFiltro + "$.ajax({";
                funcionFiltro = funcionFiltro + "type: 'POST',";
                funcionFiltro = funcionFiltro + "url: '" + ObtenerPaginaActual() + "/Buscar" + pNombreColumna + "',";
                funcionFiltro = funcionFiltro + "data: pRequest,";
                funcionFiltro = funcionFiltro + "dataType: 'json',";
                funcionFiltro = funcionFiltro + "contentType: 'application/json; charset=utf-8',";
                funcionFiltro = funcionFiltro + "success: function(pRespuesta){";
                funcionFiltro = funcionFiltro + "var json = jQuery.parseJSON(pRespuesta.d);";
                funcionFiltro = funcionFiltro + "response($.map(json.Table, function(item){";
                funcionFiltro = funcionFiltro + "return {";
                funcionFiltro = funcionFiltro + "label: item." + pNombreColumna + ",";
                funcionFiltro = funcionFiltro + "value: item." + pNombreColumna;
                funcionFiltro = funcionFiltro + "}";
                funcionFiltro = funcionFiltro + "}));";
                funcionFiltro = funcionFiltro + "}";
                funcionFiltro = funcionFiltro + "});";
                funcionFiltro = funcionFiltro + "},";
                funcionFiltro = funcionFiltro + "minLength: 2,";
                funcionFiltro = funcionFiltro + "select: function( event, ui ) {Filtro" + nombreTabla.Substring(3, nombreTabla.Length - 3) + "();},";
                funcionFiltro = funcionFiltro + "change: function(event, ui) {},";
                funcionFiltro = funcionFiltro + "open: function() {$(this).removeClass(\"ui-corner-all\").addClass(\"ui-corner-top\");},";
                funcionFiltro = funcionFiltro + "close: function() { $(this).removeClass(\"ui-corner-top\").addClass(\"ui-corner-all\");}";
                funcionFiltro = funcionFiltro + "});";
                break;
            case "Fecha":
                funcionFiltro = funcionFiltro + "$('#gbox_" + nombreTabla + " #gs_" + pNombreColumna + "').datepicker({";
                funcionFiltro = funcionFiltro + "dateFormat: 'dd/mm/yy',";
                funcionFiltro = funcionFiltro + "changeMonth: true,";
                funcionFiltro = funcionFiltro + "changeYear: true,";
                funcionFiltro = funcionFiltro + "monthNames: ['Enero','Febrero','Marzo','Abril','Mayo','Junio','Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'],";
                funcionFiltro = funcionFiltro + "monthNamesShort:['Ene','Feb','Mar','Abr','May','Jun','Jul','Ago','Sep','Oct','Nov','Dic'],";
                funcionFiltro = funcionFiltro + "onSelect: function(dateText, inst) { Filtro" + nombreTabla.Substring(3, nombreTabla.Length - 3) + "(); }";
                funcionFiltro = funcionFiltro + "});";
                break;
        }
        return funcionFiltro;
    }

    private string ObtenerFuncionFiltro(string pTipoBuscador, string pNombreColumna, SqlCommand pStoredProcedure)
    {
        string funcionFiltro = "";
        switch (pTipoBuscador)
        {
            case "Autocompletar":
                funcionFiltro = funcionFiltro + "$('#gbox_" + nombreTabla + " #gs_" + pNombreColumna + "').autocomplete({";
                funcionFiltro = funcionFiltro + "source: function( request, response ) {";
                funcionFiltro = funcionFiltro + "var pRequest = \"{'p" + pNombreColumna + "':'\"+$('#gs_" + pNombreColumna + "').val()+\"'}\";";
                funcionFiltro = funcionFiltro + "$.ajax({";
                funcionFiltro = funcionFiltro + "type: 'POST',";
                funcionFiltro = funcionFiltro + "url: '" + ObtenerPaginaActual() + "/Buscar" + pNombreColumna + "',";
                funcionFiltro = funcionFiltro + "data: pRequest,";
                funcionFiltro = funcionFiltro + "dataType: 'json',";
                funcionFiltro = funcionFiltro + "contentType: 'application/json; charset=utf-8',";
                funcionFiltro = funcionFiltro + "success: function(pRespuesta){";
                funcionFiltro = funcionFiltro + "response($.map(json.Table, function(item){";
                funcionFiltro = funcionFiltro + "return {";
                funcionFiltro = funcionFiltro + "label: item." + pNombreColumna + ",";
                funcionFiltro = funcionFiltro + "value: item." + pNombreColumna;
                funcionFiltro = funcionFiltro + "}";
                funcionFiltro = funcionFiltro + "}));";
                funcionFiltro = funcionFiltro + "}";
                funcionFiltro = funcionFiltro + "});";
                funcionFiltro = funcionFiltro + "},";
                funcionFiltro = funcionFiltro + "minLength: 2,";
                funcionFiltro = funcionFiltro + "select: function( event, ui ) {Filtro" + nombreTabla.Substring(3, nombreTabla.Length - 3) + "();},";
                funcionFiltro = funcionFiltro + "change: function(event, ui) {},";
                funcionFiltro = funcionFiltro + "open: function() {$(this).removeClass(\"ui-corner-all\").addClass(\"ui-corner-top\");},";
                funcionFiltro = funcionFiltro + "close: function() { $(this).removeClass(\"ui-corner-top\").addClass(\"ui-corner-all\");}";
                funcionFiltro = funcionFiltro + "});";
                break;
            case "Fecha":
                funcionFiltro = funcionFiltro + "$('#gbox_" + nombreTabla + "#gs_" + pNombreColumna + "').datepicker({";
                funcionFiltro = funcionFiltro + "dateFormat: 'dd/mm/yy',";
                funcionFiltro = funcionFiltro + "changeMonth: true,";
                funcionFiltro = funcionFiltro + "changeYear: true,";
                funcionFiltro = funcionFiltro + "monthNames: ['Enero','Febrero','Marzo','Abril','Mayo','Junio','Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'],";
                funcionFiltro = funcionFiltro + "monthNamesShort:['Ene','Feb','Mar','Abr','May','Jun','Jul','Ago','Sep','Oct','Nov','Dic'],";
                funcionFiltro = funcionFiltro + "onSelect: function(dateText, inst) { Filtro" + nombreTabla.Substring(3, nombreTabla.Length - 3) + "(); }";
                funcionFiltro = funcionFiltro + "});";
                break;
            case "Combo":
                //Abrir Conexion
                CConexion ConexionBaseDatos = new CConexion();
                string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

                //Instancia StoredProcedure
                pStoredProcedure.CommandType = CommandType.StoredProcedure;
                CSelectEspecifico Consulta = new CSelectEspecifico();
                Consulta.StoredProcedure = pStoredProcedure;
                Consulta.Llena(ConexionBaseDatos);

                //Construye Cadena Combo
                while (Consulta.Registros.Read())
                {
                    funcionFiltro = funcionFiltro + Consulta.Registros[0] + ":" + Consulta.Registros[1] + ";";
                }
                funcionFiltro = funcionFiltro.Remove(funcionFiltro.Length - 1, 1);
                Consulta.CerrarConsulta();

                //Cerrar Conexion
                ConexionBaseDatos.CerrarBaseDatos();
                break;
        }
        return funcionFiltro;
    }

    private string ObtenerParametrosIniciados(string pTipoBuscador, string pNombreColumna)
    {
        string parametrosIniciados = "";
        switch (pTipoBuscador)
        {
            case "Autocompletar":
                parametrosIniciados = parametrosIniciados + "var " + pNombreColumna.ToLower() + " = ''; ";
                parametrosIniciados = parametrosIniciados + "if($('#gbox_" + nombreTabla + " #gs_" + pNombreColumna + "').val() != null){";
                parametrosIniciados = parametrosIniciados + pNombreColumna.ToLower() + " = $('#gs_" + pNombreColumna + "').val();}";
                break;
            case "Combo":
                parametrosIniciados = parametrosIniciados + "var " + pNombreColumna.ToLower() + " = -1; ";
                parametrosIniciados = parametrosIniciados + "if($('#gbox_" + nombreTabla + " #gs_" + pNombreColumna + "').val() != null){";
                parametrosIniciados = parametrosIniciados + pNombreColumna.ToLower() + " = $('#gbox_" + nombreTabla + " #gs_" + pNombreColumna + "').val();}";
                break;
            case "Fecha":
                break;
        }
        return parametrosIniciados;
    }

    private string ObtenerCadenaParametros(string pTipoBuscador, string pNombreColumna)
    {
        string cadenaParametros = "";
        switch (pTipoBuscador)
        {
            case "Autocompletar":
                cadenaParametros = cadenaParametros + "'p" + pNombreColumna + "':'\"+ " + pNombreColumna.ToLower() + " + \"',";
                break;
            case "Combo":
                cadenaParametros = cadenaParametros + "'p" + pNombreColumna + "':\"+ " + pNombreColumna.ToLower() + " + \",";
                break;
            case "Fecha":
                cadenaParametros = cadenaParametros + "'p" + pNombreColumna + "':'\" + ConvertirFecha($('#gbox_" + nombreTabla + " #gs_" + pNombreColumna + "').val(),'aaaammdd') + \"',";
                break;
        }
        return cadenaParametros;
    }

    static public string ObtenerPaginaActual()
    {
        string[] arrResult = HttpContext.Current.Request.RawUrl.Split('/');
        string result = arrResult[arrResult.GetUpperBound(0)];
        arrResult = result.Split('?');
        return arrResult[arrResult.GetLowerBound(0)];
    }
}