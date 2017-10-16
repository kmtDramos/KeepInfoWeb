//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("SubCategoria");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarSubCategoria", function() {
        ObtenerFormaAgregarSubCategoria();
    });

    $("#grdSubCategoria").on("click", ".imgFormaConsultarSubCategoria", function() {
        var registro = $(this).parents("tr");
        var SubCategoria = new Object();
        SubCategoria.pIdSubCategoria = parseInt($(registro).children("td[aria-describedby='grdSubCategoria_IdSubCategoria']").html());
        ObtenerFormaConsultarSubCategoria(JSON.stringify(SubCategoria));
    });

    $('#grdSubCategoria').one('click', '.div_grdSubCategoria_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdSubCategoria_AI']").children().attr("baja")
        var idSubCategoria = $(registro).children("td[aria-describedby='grdSubCategoria_IdSubCategoria']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idSubCategoria, baja);
    });

    $('#dialogAgregarSubCategoria').dialog({
        autoOpen: false,
        height: 'auto',
        width: '600',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarSubCategoria").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarSubCategoria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarSubCategoria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarSubCategoria").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarSubCategoria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarSubCategoria").remove();
        },
        buttons: {
            "Editar": function() {
                EditarSubCategoria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });  
    
     
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarSubCategoria(){
    $("#dialogAgregarSubCategoria").obtenerVista({
        nombreTemplate: "tmplAgregarSubCategoria.html",
        url: "SubCategoria.aspx/ObtenerFormaAgregarSubCategoria",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarSubCategoria").dialog("open");
        }
    });
}

function ObtenerFormaConsultarSubCategoria(pIdSubCategoria) {
    $("#dialogConsultarSubCategoria").obtenerVista({
        nombreTemplate: "tmplConsultarSubCategoria.html",
        url: "SubCategoria.aspx/ObtenerFormaConsultarSubCategoria",
        parametros: pIdSubCategoria,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarSubCategoria == 1) {
                $("#dialogConsultarSubCategoria").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var SubCategoria = new Object();
                        SubCategoria.IdSubCategoria = parseInt($("#divFormaConsultarSubCategoria").attr("IdSubCategoria"));
                        ObtenerFormaEditarSubCategoria(JSON.stringify(SubCategoria))
                    }
                });
                $("#dialogConsultarSubCategoria").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarSubCategoria").dialog("option", "buttons", {});
                $("#dialogConsultarSubCategoria").dialog("option", "height", "100");
            }
            $("#dialogConsultarSubCategoria").dialog("open");
        }
    });
}

function ObtenerFormaEditarSubCategoria(IdSubCategoria) {
    $("#dialogEditarSubCategoria").obtenerVista({
        nombreTemplate: "tmplEditarSubCategoria.html",
        url: "SubCategoria.aspx/ObtenerFormaEditarSubCategoria",
        parametros: IdSubCategoria,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarSubCategoria").dialog("open");
        }
    });
}



//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarSubCategoria() {
    var pSubCategoria = new Object();
    pSubCategoria.SubCategoria = $("#txtSubCategoria").val();
    pSubCategoria.IdCategoria = $("#cmbCategoria").val();
    var validacion = ValidaSubCategoria(pSubCategoria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSubCategoria = pSubCategoria;
    SetAgregarSubCategoria(JSON.stringify(oRequest)); 
}

function SetAgregarSubCategoria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SubCategoria.aspx/AgregarSubCategoria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSubCategoria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarSubCategoria").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdSubCategoria, pBaja) {
    var pRequest = "{'pIdSubCategoria':" + pIdSubCategoria + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "SubCategoria.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdSubCategoria").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdSubCategoria').one('click', '.div_grdSubCategoria_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSubCategoria_AI']").children().attr("baja")
                var idSubCategoria = $(registro).children("td[aria-describedby='grdSubCategoria_IdSubCategoria']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idSubCategoria, baja);
            });
        }
    });
}

function EditarSubCategoria() {
    var pSubCategoria = new Object();
    pSubCategoria.IdSubCategoria = $("#divFormaEditarSubCategoria").attr("idSubCategoria");
    pSubCategoria.SubCategoria = $("#txtSubCategoria").val();
    pSubCategoria.IdCategoria = $("#cmbCategoria").val();
    var validacion = ValidaSubCategoria(pSubCategoria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSubCategoria = pSubCategoria;
    SetEditarSubCategoria(JSON.stringify(oRequest));
}

function SetEditarSubCategoria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SubCategoria.aspx/EditarSubCategoria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSubCategoria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarSubCategoria").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaSubCategoria(pSubCategoria) {
    var errores = "";
    
    if (pSubCategoria.IdCategoría == "")
    { errores = errores + "<span>*</span> El campo categoría esta vacío, favor de capturarlo.<br />"; }

    if (pSubCategoria.SubCategoria == "")
    { errores = errores + "<span>*</span> El campo subcategoría esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}