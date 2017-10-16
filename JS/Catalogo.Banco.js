//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Banco");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarBanco", function() {
        ObtenerFormaAgregarBanco();
    });

    $("#grdBanco").on("click", ".imgFormaConsultarBanco", function() {
        var registro = $(this).parents("tr");
        var Banco = new Object();
        Banco.pIdBanco = parseInt($(registro).children("td[aria-describedby='grdBanco_IdBanco']").html());
        ObtenerFormaConsultarBanco(JSON.stringify(Banco));
    });

    $('#grdBanco').one('click', '.div_grdBanco_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdBanco_AI']").children().attr("baja")
        var idBanco = $(registro).children("td[aria-describedby='grdBanco_IdBanco']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idBanco, baja);
    });

    $('#dialogAgregarBanco').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarBanco").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarBanco();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarBanco').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarBanco").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarBanco').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarBanco").remove();
        },
        buttons: {
            "Editar": function() {
                EditarBanco();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarBanco()
{
    $("#dialogAgregarBanco").obtenerVista({
        nombreTemplate: "tmplAgregarBanco.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarBanco").dialog("open");
        }
    });
}

function ObtenerFormaConsultarBanco(pIdBanco) {
    $("#dialogConsultarBanco").obtenerVista({
        nombreTemplate: "tmplConsultarBanco.html",
        url: "Banco.aspx/ObtenerFormaBanco",
        parametros: pIdBanco,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarBanco == 1) {
                $("#dialogConsultarBanco").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Banco = new Object();
                        Banco.IdBanco = parseInt($("#divFormaConsultarBanco").attr("IdBanco"));
                        ObtenerFormaEditarBanco(JSON.stringify(Banco))
                    }
                });
                $("#dialogConsultarBanco").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarBanco").dialog("option", "buttons", {});
                $("#dialogConsultarBanco").dialog("option", "height", "100");
            }
            $("#dialogConsultarBanco").dialog("open");
        }
    });
}

function ObtenerFormaEditarBanco(IdBanco) {
    $("#dialogEditarBanco").obtenerVista({
        nombreTemplate: "tmplEditarBanco.html",
        url: "Banco.aspx/ObtenerFormaEditarBanco",
        parametros: IdBanco,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarBanco").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarBanco() {
    var pBanco = new Object();
    pBanco.Banco = $("#txtBanco").val();
    var validacion = ValidaBanco(pBanco);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pBanco = pBanco;
    SetAgregarBanco(JSON.stringify(oRequest)); 
}

function SetAgregarBanco(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Banco.aspx/AgregarBanco",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdBanco").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarBanco").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdBanco, pBaja) {
    var pRequest = "{'pIdBanco':" + pIdBanco + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Banco.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdBanco").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdBanco').one('click', '.div_grdBanco_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdBanco_AI']").children().attr("baja")
                var idBanco = $(registro).children("td[aria-describedby='grdBanco_IdBanco']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idBanco, baja);
            });
        }
    });
}

function EditarBanco() {
    var pBanco = new Object();
    pBanco.IdBanco = $("#divFormaEditarBanco").attr("idBanco");
    pBanco.Banco = $("#txtBanco").val();
    var validacion = ValidaBanco(pBanco);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pBanco = pBanco;
    SetEditarBanco(JSON.stringify(oRequest));
}

function SetEditarBanco(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Banco.aspx/EditarBanco",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdBanco").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarBanco").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaBanco(pBanco) {
    var errores = "";

    if (pBanco.Banco == "")
    { errores = errores + "<span>*</span> El campo Banco esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}