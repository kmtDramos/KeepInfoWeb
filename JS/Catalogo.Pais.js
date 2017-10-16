//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("Pais");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarPais", function() {
        ObtenerFormaAgregarPais();
    });

    $("#grdPais").on("click", ".imgFormaConsultarPais", function() {
        var registro = $(this).parents("tr");
        var Pais = new Object();
        Pais.pIdPais = parseInt($(registro).children("td[aria-describedby='grdPais_IdPais']").html());
        ObtenerFormaConsultarPais(JSON.stringify(Pais));
    });

    $('#grdPais').one('click', '.div_grdPais_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdPais_AI']").children().attr("baja")
        var idPais = $(registro).children("td[aria-describedby='grdPais_IdPais']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idPais, baja);
    });

    $('#dialogAgregarPais').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarPais").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarPais();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarPais').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarPais").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogEditarPais').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarPais").remove();
        },
        buttons: {
            "Editar": function() {
                EditarPais();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarPais() {
    $("#dialogAgregarPais").obtenerVista({
        nombreTemplate: "tmplAgregarPais.html",
        url: "Pais.aspx/ObtenerFormaAgregarPais",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarPais").dialog("open");
            $("#cmbTipoVenta option[value=1]").attr("selected", true);
            $("#cmbTipoMoneda option[value=1]").attr("selected", true); 
        }
    });
}

function ObtenerFormaConsultarPais(pIdPais) {
    $("#dialogConsultarPais").obtenerVista({
        nombreTemplate: "tmplConsultarPais.html",
        url: "Pais.aspx/ObtenerFormaPais",
        parametros: pIdPais,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarPais == 1) {
                $("#dialogConsultarPais").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Pais = new Object();
                        Pais.IdPais = parseInt($("#divFormaConsultarPais").attr("IdPais"));
                        ObtenerFormaEditarPais(JSON.stringify(Pais))
                    }
                });
                $("#dialogConsultarPais").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarPais").dialog("option", "buttons", {});
                $("#dialogConsultarPais").dialog("option", "height", "250");
            }
            $("#dialogConsultarPais").dialog("open");
        }
    });
}

function ObtenerFormaEditarPais(IdPais) {
    $("#dialogEditarPais").obtenerVista({
        nombreTemplate: "tmplEditarPais.html",
        url: "Pais.aspx/ObtenerFormaEditarPais",
        parametros: IdPais,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarPais").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarPais() {
    var pPais = new Object();
    pPais.Pais = $("#txtPais").val();
    pPais.Nacionalidad = $("#txtNacionalidad").val();
    
    var validacion = ValidaPais(pPais);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pPais = pPais;
    SetAgregarPais(JSON.stringify(oRequest));
}

function SetAgregarPais(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pais.aspx/AgregarPais",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdPais").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarPais").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdPais, pBaja) {
    var pRequest = "{'pIdPais':" + pIdPais + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Pais.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdPais").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdPais').one('click', '.div_grdPais_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdPais_AI']").children().attr("baja")
                var idPais = $(registro).children("td[aria-describedby='grdPais_IdPais']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idPais, baja);
            });
        }
    });
}

function EditarPais() {
    var pPais = new Object();
    pPais.IdPais = $("#divFormaEditarPais").attr("idPais");
    pPais.Pais = $("#txtPais").val();
    pPais.Nacionalidad = $("#txtNacionalidad").val();
    
    var validacion = ValidaPais(pPais);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pPais = pPais;
    SetEditarPais(JSON.stringify(oRequest));
}

function SetEditarPais(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pais.aspx/EditarPais",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdPais").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarPais").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaPais(pPais) {
    var errores = "";

    if (pPais.Pais == "")
    { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pPais.Nacionalidad == "")
    { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
