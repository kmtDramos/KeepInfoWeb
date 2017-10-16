//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Addenda");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarAddenda", function() {
        ObtenerFormaAgregarAddenda();
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarEstructuraAddenda", function() {
        var Addenda = new Object();
        Addenda.pIdAddenda = $("#divFormaEstructuraAddenda").attr("IdAddenda");
        ObtenerFormaAgregarEstructuraAddenda(JSON.stringify(Addenda));
    });

    $("#grdAddenda").on("click", ".imgFormaConsultarAddenda", function() {
        var registro = $(this).parents("tr");
        var Addenda = new Object();
        Addenda.pIdAddenda = parseInt($(registro).children("td[aria-describedby='grdAddenda_IdAddenda']").html());
        ObtenerFormaConsultarAddenda(JSON.stringify(Addenda));
    });
    
    $("#grdAddenda").on("click", ".imgFormaConsultarEstructuraAddenda", function() {
        var registro = $(this).parents("tr");
        var Addenda = new Object();
        Addenda.pIdAddenda = parseInt($(registro).children("td[aria-describedby='grdAddenda_IdAddenda']").html());
        ObtenerFormaConsultarEstructuraAddenda(JSON.stringify(Addenda));
    });

    $('#grdAddenda').one('click', '.div_grdAddenda_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdAddenda_AI']").children().attr("baja")
        var idAddenda = $(registro).children("td[aria-describedby='grdAddenda_IdAddenda']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idAddenda, baja);
    });

    $('#dialogAgregarAddenda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarAddenda").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarAddenda();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarAddenda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarAddenda").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogConsultarEstructuraAddenda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
        }
    });
    
    $('#dialogConsultarEstructuraAddendaConsultar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarEstructuraAddendaConsultar").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarAddenda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarAddenda").remove();
        },
        buttons: {
            "Editar": function() {
                EditarAddenda();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogEditarEstructuraAddenda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarEstructuraAddenda").remove();
        },
        buttons: {
            "Editar": function() {
                EditarEstructuraAddenda();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogAgregarEstructuraAddenda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
            "Guardar": function() {
                AgregarEstructuraAddenda();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarAddenda() {
    $("#dialogAgregarAddenda").obtenerVista({
        nombreTemplate: "tmplAgregarAddenda.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarAddenda").dialog("open");
        }
    });
}

function ObtenerFormaAgregarEstructuraAddenda(pRequest) {
    $("#dialogAgregarEstructuraAddenda").obtenerVista({
        nombreTemplate: "tmplAgregarEstructuraAddenda.html",
        url: "Addenda.aspx/ObtenerFormaAgregarEstructuraAddenda",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarEstructuraAddenda").dialog("open");
        }
    });
}

function ObtenerFormaConsultarAddenda(pIdAddenda) {
    $("#dialogConsultarAddenda").obtenerVista({
        nombreTemplate: "tmplConsultarAddenda.html",
        url: "Addenda.aspx/ObtenerFormaAddenda",
        parametros: pIdAddenda,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarAddenda == 1) {
                $("#dialogConsultarAddenda").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Addenda = new Object();
                        Addenda.IdAddenda = parseInt($("#divFormaConsultarAddenda").attr("IdAddenda"));
                        ObtenerFormaEditarAddenda(JSON.stringify(Addenda))
                    }
                });
                $("#dialogConsultarAddenda").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarAddenda").dialog("option", "buttons", {});
                $("#dialogConsultarAddenda").dialog("option", "height", "100");
            }
            $("#dialogConsultarAddenda").dialog("open");
        }
    });
}
function ObtenerFormaConsultarEstructuraAddenda(pIdAddenda) {
    $("#divFormaConsultarEstructuraAddenda").obtenerVista({
        nombreTemplate: "tmplConsultarEstructuraAddenda.html",
        url: "Addenda.aspx/ObtenerFormaConsultarEstructuraAddenda",
        parametros: pIdAddenda,
        despuesDeCompilar: function(pRespuesta) {
            FiltroEstructuraAddenda();
            $("#dialogConsultarEstructuraAddenda").dialog("open");

            $('#grdEstructuraAddenda').one('click', '.div_grdEstructuraAddenda_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEstructuraAddenda_AI']").children().attr("baja")
                var idEstructuraAddenda = $(registro).children("td[aria-describedby='grdEstructuraAddenda_IdEstructuraAddenda']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusEstructuraAddenda(idEstructuraAddenda, baja);
            });        
            
            $("#grdEstructuraAddenda").on("click", ".ConsultarEstructuraAddenda", function() {
                var registro = $(this).parents("tr");
                var Addenda = new Object();
                Addenda.pIdEstructuraAddenda = parseInt($(registro).children("td[aria-describedby='grdEstructuraAddenda_IdEstructuraAddenda']").html());
                ObtenerFormaConsultarEstructuraAddendaConsultar(JSON.stringify(Addenda));
            });
        }
    });
}

function ObtenerFormaConsultarEstructuraAddendaConsultar(pIdEstructuraAddenda) {
    $("#dialogConsultarEstructuraAddendaConsultar").obtenerVista({
        nombreTemplate: "tmplConsultarEstructuraAddendaConsultar.html",
        url: "Addenda.aspx/ObtenerFormaEstructuraAddendaConsultar",
        parametros: pIdEstructuraAddenda,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarEstructuraAddenda == 1) {
                $("#dialogConsultarEstructuraAddendaConsultar").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var EstructuraAddenda = new Object();
                        EstructuraAddenda.IdEstructuraAddenda = parseInt($("#divFormaConsultarEstructuraAddendaConsultar").attr("IdEstructuraAddenda"));
                        ObtenerFormaEditarEstructuraAddenda(JSON.stringify(EstructuraAddenda))

                    }
                });
                $("#dialogConsultarEstructuraAddendaConsultar").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarEstructuraAddendaConsultar").dialog("option", "buttons", {});
                $("#dialogConsultarEstructuraAddendaConsultar").dialog("option", "height", "300");
            }
            $("#dialogConsultarEstructuraAddendaConsultar").dialog("open");
        }
    });
}

function ObtenerFormaEditarAddenda(IdAddenda) {
    $("#dialogEditarAddenda").obtenerVista({
        nombreTemplate: "tmplEditarAddenda.html",
        url: "Addenda.aspx/ObtenerFormaEditarAddenda",
        parametros: IdAddenda,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarAddenda").dialog("open");
        }
    });
}

function ObtenerFormaEditarEstructuraAddenda(IdEstructuraAddenda) {
    $("#dialogEditarEstructuraAddenda").obtenerVista({
        nombreTemplate: "tmplEditarEstructuraAddenda.html",
        url: "Addenda.aspx/ObtenerFormaEditarEstructuraAddenda",
        parametros: IdEstructuraAddenda,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarEstructuraAddenda").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarAddenda() {
    var pAddenda = new Object();
    pAddenda.Addenda = $("#txtAddenda").val();
    var validacion = ValidaAddenda(pAddenda);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAddenda = pAddenda;
    SetAgregarAddenda(JSON.stringify(oRequest));
}

function SetAgregarAddenda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Addenda.aspx/AgregarAddenda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAddenda").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarAddenda").dialog("close");
        }
    });
}

function AgregarEstructuraAddenda() {
    var pEstructuraAddenda = new Object();
    pEstructuraAddenda.IdAddenda = $(".divFormaAgregarEstructuraAddenda").attr("IdAddenda");
    pEstructuraAddenda.EstructuraAddenda = $("#txtEstructuraAddenda").val();
    pEstructuraAddenda.IdTipoElemento = $("#cmbTipoElemento").val();
    
    var validacion = ValidaEstructuraAddenda(pEstructuraAddenda);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEstructuraAddenda = pEstructuraAddenda;
    SetAgregarEstructuraAddenda(JSON.stringify(oRequest));
}

function SetAgregarEstructuraAddenda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Addenda.aspx/AgregarEstructuraAddenda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEstructuraAddenda").trigger("reloadGrid");
                $("#grdAddenda").trigger("reloadGrid");
                $("#dialogAgregarEstructuraAddenda").dialog("close")
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

function SetCambiarEstatus(pIdAddenda, pBaja) {
    var pRequest = "{'pIdAddenda':" + pIdAddenda + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Addenda.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdAddenda").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdAddenda').one('click', '.div_grdAddenda_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdAddenda_AI']").children().attr("baja")
                var idAddenda = $(registro).children("td[aria-describedby='grdAddenda_IdAddenda']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idAddenda, baja);
            });
        }
    });
}

function SetCambiarEstatusEstructuraAddenda(pIdEstructuraAddenda, pBaja) {
    var pRequest = "{'pIdEstructuraAddenda':" + pIdEstructuraAddenda + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Addenda.aspx/CambiarEstatusEstructuraAddenda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdEstructuraAddenda").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdEstructuraAddenda').one('click', '.div_grdEstructuraAddenda_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEstructuraAddenda_AI']").children().attr("baja")
                var idEstructuraAddenda = $(registro).children("td[aria-describedby='grdEstructuraAddenda_IdEstructuraAddenda']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusEstructuraAddenda(idEstructuraAddenda, baja);
            });
        }
    });
}

function EditarAddenda() {
    var pAddenda = new Object();
    pAddenda.IdAddenda = $("#divFormaEditarAddenda").attr("idAddenda");
    pAddenda.Addenda = $("#txtAddenda").val();
    var validacion = ValidaAddenda(pAddenda);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAddenda = pAddenda;
    SetEditarAddenda(JSON.stringify(oRequest));
}
function SetEditarAddenda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Addenda.aspx/EditarAddenda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAddenda").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarAddenda").dialog("close");
        }
    });
}

function EditarEstructuraAddenda() {
    var pEstructuraAddenda = new Object();
    pEstructuraAddenda.IdEstructuraAddenda = $("#divFormaEditarEstructuraAddenda").attr("idEstructuraAddenda");
    pEstructuraAddenda.EstructuraAddenda = $("#txtEstructuraAddenda").val();
    pEstructuraAddenda.IdTipoElemento = $("#cmbTipoElemento").val();
        
    var validacion = ValidaEstructuraAddenda(pEstructuraAddenda);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEstructuraAddenda = pEstructuraAddenda;
    SetEditarEstructuraAddenda(JSON.stringify(oRequest));
}

function SetEditarEstructuraAddenda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Addenda.aspx/EditarEstructuraAddenda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEstructuraAddenda").trigger("reloadGrid");
                $("#grdAddenda").trigger("reloadGrid");
                $("#dialogEditarEstructuraAddenda").dialog("close");
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


function FiltroEstructuraAddenda() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdEstructuraAddenda').getGridParam('rowNum');
    request.pPaginaActual = $('#grdEstructuraAddenda').getGridParam('page');
    request.pColumnaOrden = $('#grdEstructuraAddenda').getGridParam('sortname');
    request.pTipoOrden = $('#grdEstructuraAddenda').getGridParam('sortorder');
    request.pIdAddenda = 0;
    request.pAI = -1;

    if ($("#divFormaEstructuraAddenda").attr("IdAddenda") != null) {
        request.pIdAddenda = $("#divFormaEstructuraAddenda").attr("IdAddenda");
    }

    if ($('#divContGridEstructuraAddenda').find(gs_AI).existe()) {
        request.pAI = $('#divContGridEstructuraAddenda').find(gs_AI).val();
    }
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Addenda.aspx/ObtenerEstructuraAddenda',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdEstructuraAddenda')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//-----Validaciones------------------------------------------------------
function ValidaAddenda(pAddenda) {
    var errores = "";

    if (pAddenda.Addenda == "")
    { errores = errores + "<span>*</span> El nombre de la addenda esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaEstructuraAddenda(pEstructuraAddenda) {
    var errores = "";

    if (pEstructuraAddenda.EstructuraAddenda == "")
    { errores = errores + "<span>*</span> El elemento esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
