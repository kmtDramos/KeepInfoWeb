//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    SetFormaAltaGenerador();
    
    //-----Eventos------------------------------------------------------
    $("#grdClasesGeneradas").on("click", ".imgFormaConsultarClaseGenerada", function() {
        var registro = $(this).parents("tr");
        var ClaseGenerador = new Object();
        ClaseGenerador.pIdClaseGenerador = parseInt($(registro).children("td[aria-describedby='grdClasesGeneradas_IdClaseGenerador']").html());
        var request = JSON.stringify(ClaseGenerador);

        $("#divVistaFormas").obtenerVista({
            nombreTemplate: "tmplConsultarClaseGenerada.html",
            url: "Generador.aspx/ObtenerClaseGenerador",
            parametros: request,
            efecto: "slide"
        });
    });

    $(".ulListaAtributos").livequery(function() {
        //----CONFIGURA LOS BOTONES
        $(".btnJQuery").button({
            icons: { primary: "ui-icon-plus" }
        }).click(function(event) {
            event.preventDefault();
            switch ($(this).attr("tipoAtributo")) {
                case 'S':
                    $('.ulListaAtributos').append('<li class="liAtributo liMove" idClaseAtributo="0">' + $('#divString').html() + '</li>');
                    break;
                case 'I':
                    $('.ulListaAtributos').append('<li class="liAtributo liMove" idClaseAtributo="0">' + $('#divInteger').html() + '</li>');
                    break;
                case 'D':
                    $('.ulListaAtributos').append('<li class="liAtributo liMove" idClaseAtributo="0">' + $('#divDecimal').html() + '</li>');
                    break;
                case 'DT':
                    $('.ulListaAtributos').append('<li class="liAtributo liMove" idClaseAtributo="0">' + $('#divDateTime').html() + '</li>');
                    break;
                case 'B':
                    $('.ulListaAtributos').append('<li class="liAtributo liMove" idClaseAtributo="0">' + $('#divBoolean').html() + '</li>');
                    break;
                default:
                    break;
            }
            $(".ulListaAtributos .liAtributo:last").slideDown('slow');
        });

        //----CONFIGURA EL CHECKBOX
        $("#lblBaja").button({
            icons: { primary: "ui-icon-circle-close" }
        }).click(function(event) {
            event.preventDefault();
            if ($("#chkBaja").is(":checked")) {
                $("#lblBaja").button({ icons: { primary: "ui-icon-circle-close"} });
                $("#chkBaja").removeAttr("checked");
            }
            else {
                $("#lblBaja").button({ icons: { primary: "ui-icon-circle-check"} });
                $("#chkBaja").attr("checked", "checked");
            }
        });

        if ($("#chkBaja").is(":checked")) {
            $("#lblBaja").button({ icons: { primary: "ui-icon-circle-check"} });
        }

        //----CONFIGURA EL CHECKBOX BLOQUEO
        $("#lblBloqueo").button({
            icons: { primary: "ui-icon-circle-close" }
        }).click(function(event) {
            event.preventDefault();
            if ($("#chkBloqueo").is(":checked")) {
                $("#lblBloqueo").button({ icons: { primary: "ui-icon-circle-close"} });
                $("#chkBloqueo").removeAttr("checked");
            }
            else {
                $("#lblBloqueo").button({ icons: { primary: "ui-icon-circle-check"} });
                $("#chkBloqueo").attr("checked", "checked");
            }
        });

        if ($("#chkBloqueo").is(":checked")) {
            $("#lblBloqueo").button({ icons: { primary: "ui-icon-circle-check"} });
        }
    });

    $("#ulListaAtributos").livequery(function() {
        $("#ulListaAtributos").sortable({
            items: "li:not(.liNotMove)",
            axis: "y"
        });
        $("#ulListaAtributos").disableSelection();
    });

    $("#divVistaFormas").on("click", "#btnFormaAltaClase", function() {
        SetFormaAltaGenerador();
    });

    $("#divVistaFormas").on("click", "#btnAgregarClase", function() {
        var claseGenerador = ObtenerAtributos();
        var validacion = ValidaGenerador(claseGenerador);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pClaseGenerador = claseGenerador;
        SetAgregarGenerador(JSON.stringify(oRequest));
    });

    $("#divVistaFormas").on("click", "#btnFormaEditarClase", function() {
        var ClaseGenerador = new Object();
        ClaseGenerador.pIdClaseGenerador = parseInt($("#divFormulario").attr("idClaseGenerador"));
        var request = JSON.stringify(ClaseGenerador);
        $("#divVistaFormas").obtenerVista({
            nombreTemplate: "tmplEditarClaseGenerada.html",
            url: "Generador.aspx/ObtenerClaseGenerador",
            parametros: request,
            efecto: "slide"
        });
    });

    $('#divVistaFormas').on('click', '.chkAtributo:checkbox[readonly="readonly"]', function(event) {
        return false;
    });

    $("#divVistaFormas").on("click", "#btnEditarClase", function(event) {
        var claseGenerador = ObtenerAtributos();
        var validacion = ValidaGenerador(claseGenerador);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pClaseGenerador = claseGenerador;
        SetEditarGenerador(JSON.stringify(oRequest));
    });
});

//---------Funciones--------//
//--------------------------//
function ObtenerAtributos() {
    var oClaseGenerador = new Object();
    oClaseGenerador.IdClaseGenerador = $("#divFormulario").attr("idClaseGenerador");
    oClaseGenerador.Clase = $("#txtClase").val();
    oClaseGenerador.ClaseAnterior = $("#divFormulario").attr("clase");
    oClaseGenerador.Abreviatura = $("#txtAbreviatura").val();
    
    if ($("#chkBloqueo").is(':checked'))
    { oClaseGenerador.Bloqueo = true; }
    else
    { oClaseGenerador.Bloqueo = false; }
    
    if ($("#chkBaja").is(':checked'))
    { oClaseGenerador.ManejaBaja = true; }
    else
    { oClaseGenerador.ManejaBaja = false; }
    oClaseGenerador.Atributos = new Array();

    $('#ulListaAtributos').children('li').each(function() {
        var oAtributo = new Object();
        oAtributo.TipoAtributo = $(this).children('div:first').text();
        oAtributo.IdClaseAtributo = $(this).attr('idClaseAtributo');
        oAtributo.Atributo = "";
        oAtributo.Longitud = 0;
        oAtributo.NumeroDecimales = 0;
        oAtributo.LlavePrimaria = "false";
        oAtributo.Identidad = "false";

        $(this).children('input').each(function() {
            switch ($(this).attr("tipo")) {
                case "nombreAtributo":
                    oAtributo.Atributo = $(this).val();
                    break;
                case "longitud":
                    oAtributo.Longitud = parseInt($(this).val());
                    break;
                case "numeroDecimales":
                    oAtributo.NumeroDecimales = parseInt($(this).val());
                    break;
            }
        });
        oClaseGenerador.Atributos.push(oAtributo);
    });
    return oClaseGenerador;
}

function EliminarAtributo(pElemento)
{
    $(pElemento).parents('.liAtributo').slideUp('slow', function(){
        $(pElemento).parents('.liAtributo').remove();
    });
}

//--------Validaciones-------//
//--------------------------//
function ValidaGenerador(pClaseGenerador) 
{
    var errores = "";
    var bEntro = false;
    var bAtributo = false;
    var bLongitud = false;
    var bLongitudString = false;
    var bLongitudDecimal = false;
    var bDecimal = false;
    var bBaja = false;
    
    if (pClaseGenerador.Clase == "")
    { errores = errores + "<span>*</span> El campo nombre de la clase esta vac&iacute;o, favor de capturarlo.<br />"; }

    $.each(pClaseGenerador.Atributos, function(i, objeto) {
        bEntro = true;
        if (objeto.Atributo == "" && bAtributo == false) {
            errores = errores + "<span>*</span> Todos los campos de los atributos son obligatorios, favor de capturarlos.<br />";
            bAtributo = true;
        }

        if (objeto.Atributo.toLowerCase() == "baja" && bBaja == false) {
            errores = errores + "<span>*</span> No puedes nombrar 'Baja' a un atributo, favor de cambiarlo.<br />";
            bBaja = true;
        }

        if (objeto.Atributo.toLowerCase() == "id" + pClaseGenerador.Clase.toLowerCase() && bBaja == false) {
            errores = errores + "<span>*</span> No puedes nombrar el 'id' de la clase, favor de cambiarlo.<br />";
            bBaja = true;
        }

        if ((objeto.TipoAtributo == "S" || objeto.TipoAtributo == "D") && objeto.Longitud == 0 && bLongitud == false) {
            errores = errores + "<span>*</span> Los campos longitud no pueden tener valor 0.<br />";
            bLongitud = true;
        }

        if (objeto.TipoAtributo == "S" && objeto.Longitud > 8000) {
            errores = errores + "<span>*</span> Los campos longitud de tipo string no pueden ser mayor a 8000.<br />";
            bLongitudString = true;
        }

        if (objeto.TipoAtributo == "D" && objeto.Longitud > 18) {
            errores = errores + "<span>*</span> Los campos longitud de tipo decimal no pueden ser mayor a 18.<br />";
            bLongitudDecimal = true;
        }

        if (objeto.TipoAtributo == "D" && objeto.NumeroDecimales == 0) {
            errores = errores + "<span>*</span> Los datos tipo decimal no pueden tener 0 decimales, capturalos o cambia a tipo integer.<br />";
            bDecimal = true;
        }
        else if (objeto.TipoAtributo == "D" && objeto.Longitud < objeto.NumeroDecimales) {
            errores = errores + "<span>*</span> Los campos decimal no pueden ser mayor que el campo longitud del atributo.<br />";
            bDecimal = true;
        }

        return (bAtributo == false || bLongitud == false || bLongitudDecimal == false || bLongitudString == false || bBaja == false || bDecimal == false);
    });

    if (bEntro == false)
    { errores = errores + "<span>*</span> No agrego ningun atributo a la clase.<br />"; }
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

//-----------AJAX-----------//
//--------------------------//

//-----------AJAX------------//
//----Funciones de Accion----//
function SetAgregarGenerador(pRequest) 
{
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Generador.aspx/AgregarGenerador",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                SetFormaAltaGenerador();
                var claseGenerador = respuesta.Modelo;
                $("#grdClasesGeneradas").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetEditarGenerador(pRequest) 
{
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Generador.aspx/EditarGenerador",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var claseGenerador = respuesta.Modelo;
                SetFormaAltaGenerador();
                $("#grdClasesGeneradas").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function SetFormaAltaGenerador() 
{
    $("#divVistaFormas").obtenerVista({
        nombreTemplate: "tmplAgregarClaseGenerador.html",
        efecto: "slide"
    });
}

function ObtenerFormaConsultarClaseGenerada(pIdClase) 
{
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarClaseGenerada.aspx",
        data: {
            IdClase: pIdClase
        },
        datatype: "html",
        success: function(pRespuesta) {
            if ($("#divFormulario").length) {
                $('#divFormulario').fadeOut('slow', function() {
                    $('#divVistaFormas').html(pRespuesta);
                    $("#divFormulario").fadeIn('slow');
                });
            }
            else {
                $('#divVistaFormas').html(pRespuesta);
                $("#divFormulario").fadeIn('slow');
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function Termino_grdClasesGeneradas() 
{
    var ids = $('#grdClasesGeneradas').jqGrid('getDataIDs');
    for (var i = 0; i < ids.length; i++)
    {
        var bloqueado = $('#grdClasesGeneradas #' + ids[i] + ' td[aria-describedby="grdClasesGeneradas_Bloqueo"]').html();
        if (bloqueado == '0')
        { var etiquetaBloqueo = '<div class="div_grdClasesGeneradas_Bloqueo divImagenBloqueo"><img title ="No bloqueado" src="../images/stock_lock-open.png" /></div>'; }
        else
        { var etiquetaBloqueo = '<div class="div_grdClasesGeneradas_Bloqueo divImagenBloqueo"><img title ="Bloqueado" src="../images/stock_lock.png" /></div>'; }
        $('#grdClasesGeneradas').jqGrid('setRowData', ids[i], { Bloqueo: etiquetaBloqueo });

        var manejaBaja = $('#grdClasesGeneradas #' + ids[i] + ' td[aria-describedby="grdClasesGeneradas_ManejaBaja"]').html();
        if (manejaBaja == '0')
        { var etiquetaManejaBaja = '<div class="div_grdClasesGeneradas_ManejaBaja divImagenManejaBaja"><img title="No maneja baja lógica" src="../images/false.png" /></div>'; }
        else
        { var etiquetaManejaBaja = '<div class="div_grdClasesGeneradas_ManejaBaja divImagenManejaBaja"><img title="Maneja baja lógica" src="../images/true.png" /></div>'; }
        $('#grdClasesGeneradas').jqGrid('setRowData', ids[i], { ManejaBaja:etiquetaManejaBaja });
    }
}