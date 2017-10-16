//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("Localidad");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarLocalidad", function() {
        ObtenerFormaAgregarLocalidad();
    });

    $("#grdLocalidad").on("click", ".imgFormaConsultarLocalidad", function() {
        var registro = $(this).parents("tr");
        var Localidad = new Object();
        Localidad.pIdLocalidad = parseInt($(registro).children("td[aria-describedby='grdLocalidad_IdLocalidad']").html());
        ObtenerFormaConsultarLocalidad(JSON.stringify(Localidad));
    });

    $('#dialogAgregarLocalidad, #dialogEditarLocalidad').on('change', '#cmbPais', function(event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstados(JSON.stringify(request));
    }); 
    
    $('#dialogAgregarLocalidad, #dialogEditarLocalidad').on('change', '#cmbEstado', function(event) {
        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipios(JSON.stringify(request));
    }); 
    
    $('#grdLocalidad').one('click', '.div_grdLocalidad_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdLocalidad_AI']").children().attr("baja")
        var idLocalidad = $(registro).children("td[aria-describedby='grdLocalidad_IdLocalidad']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idLocalidad, baja);
    });

    $('#dialogAgregarLocalidad').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarLocalidad").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarLocalidad();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarLocalidad').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarLocalidad").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogEditarLocalidad').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarLocalidad").remove();
        },
        buttons: {
            "Editar": function() {
                EditarLocalidad();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarLocalidad() {
    $("#dialogAgregarLocalidad").obtenerVista({
        nombreTemplate: "tmplAgregarLocalidad.html",
        url: "Localidad.aspx/ObtenerFormaAgregarLocalidad",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarLocalidad").dialog("open");
        }
    });
}

function ObtenerFormaConsultarLocalidad(pIdLocalidad) {
    $("#dialogConsultarLocalidad").obtenerVista({
        nombreTemplate: "tmplConsultarLocalidad.html",
        url: "Localidad.aspx/ObtenerFormaLocalidad",
        parametros: pIdLocalidad,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarLocalidad == 1) {
                $("#dialogConsultarLocalidad").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Localidad = new Object();
                        Localidad.IdLocalidad = parseInt($("#divFormaConsultarLocalidad").attr("IdLocalidad"));
                        ObtenerFormaEditarLocalidad(JSON.stringify(Localidad))
                    }
                });
                $("#dialogConsultarLocalidad").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarLocalidad").dialog("option", "buttons", {});
                $("#dialogConsultarLocalidad").dialog("option", "height", "250");
            }
            $("#dialogConsultarLocalidad").dialog("open");
        }
    });
}

function ObtenerFormaEditarLocalidad(IdLocalidad) {
    $("#dialogEditarLocalidad").obtenerVista({
        nombreTemplate: "tmplEditarLocalidad.html",
        url: "Localidad.aspx/ObtenerFormaEditarLocalidad",
        parametros: IdLocalidad,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarLocalidad").dialog("open");
        }
    });
}

function ObtenerListaEstados(pRequest) {
    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Localidad.aspx/ObtenerListaEstados",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) { 
            var request = new Object();
            request.pIdEstado = $("#cmbEstado").val();
            ObtenerListaMunicipios(JSON.stringify(request));   
        }
    });
}

function ObtenerListaMunicipios(pRequest) {
    $("#cmbMunicipio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Localidad.aspx/ObtenerListaMunicipios",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) { 
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarLocalidad() {
    var pLocalidad = new Object();
    pLocalidad.Localidad = $("#txtLocalidad").val();
    pLocalidad.Clave = $("#txtClave").val();
    pLocalidad.IdMunicipio = $("#cmbMunicipio").val();
    
    var validacion = ValidaLocalidad(pLocalidad);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pLocalidad = pLocalidad;
    SetAgregarLocalidad(JSON.stringify(oRequest));
}

function SetAgregarLocalidad(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Localidad.aspx/AgregarLocalidad",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdLocalidad").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarLocalidad").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdLocalidad, pBaja) {
    var pRequest = "{'pIdLocalidad':" + pIdLocalidad + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Localidad.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdLocalidad").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdLocalidad').one('click', '.div_grdLocalidad_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdLocalidad_AI']").children().attr("baja")
                var idLocalidad = $(registro).children("td[aria-describedby='grdLocalidad_IdLocalidad']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idLocalidad, baja);
            });
        }
    });
}

function EditarLocalidad() {
    var pLocalidad = new Object();
    pLocalidad.IdLocalidad = $("#divFormaEditarLocalidad").attr("idLocalidad");
    pLocalidad.Localidad = $("#txtLocalidad").val();
    pLocalidad.Clave = $("#txtClave").val();
    pLocalidad.IdMunicipio = $("#cmbMunicipio").val();
    
    var validacion = ValidaLocalidad(pLocalidad);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pLocalidad = pLocalidad;
    SetEditarLocalidad(JSON.stringify(oRequest));
}

function SetEditarLocalidad(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Localidad.aspx/EditarLocalidad",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdLocalidad").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarLocalidad").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaLocalidad(pLocalidad) {
    var errores = "";

    if (pLocalidad.Localidad == "")
    { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pLocalidad.Abreviatura == "")
    { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pLocalidad.Clave == "")
    { errores = errores + "<span>*</span> El campo clave esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pLocalidad.IdMunicipio == 0)
    { errores = errores + "<span>*</span> El campo municipio esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
