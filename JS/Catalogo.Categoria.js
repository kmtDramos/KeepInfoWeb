//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Categoria");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCategoria", function() {
        ObtenerFormaAgregarCategoria();
    });
    
    $("#grdCategoria").on("click", ".imgFormaConsultarCategoria", function() {
        var registro = $(this).parents("tr");
        var Categoria = new Object();
        Categoria.pIdCategoria = parseInt($(registro).children("td[aria-describedby='grdCategoria_IdCategoria']").html());
        ObtenerFormaConsultarCategoria(JSON.stringify(Categoria));
    });
    
    $('#grdCategoria').one('click', '.div_grdCategoria_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCategoria_AI']").children().attr("baja")
        var idCategoria = $(registro).children("td[aria-describedby='grdCategoria_IdCategoria']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCategoria, baja);
    });
    
    $('#dialogAgregarCategoria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCategoria").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCategoria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarCategoria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCategoria").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarCategoria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCategoria").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCategoria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCategoria()
{
    $("#dialogAgregarCategoria").obtenerVista({
        nombreTemplate: "tmplAgregarCategoria.html",
        url: "Categoria.aspx/ObtenerFormaAgregarCategoria",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCategoria").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCategoria(pIdCategoria) {
    $("#dialogConsultarCategoria").obtenerVista({
        nombreTemplate: "tmplConsultarCategoria.html",
        url: "Categoria.aspx/ObtenerFormaCategoria",
        parametros: pIdCategoria,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCategoria == 1) {
                $("#dialogConsultarCategoria").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Categoria = new Object();
                        Categoria.IdCategoria = parseInt($("#divFormaConsultarCategoria").attr("IdCategoria"));
                        ObtenerFormaEditarCategoria(JSON.stringify(Categoria))
                    }
                });
                $("#dialogConsultarCategoria").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCategoria").dialog("option", "buttons", {});
                $("#dialogConsultarCategoria").dialog("option", "height", "100");
            }
            $("#dialogConsultarCategoria").dialog("open");
        }
    });
}

function ObtenerFormaEditarCategoria(IdCategoria) {
    $("#dialogEditarCategoria").obtenerVista({
        nombreTemplate: "tmplEditarCategoria.html",
        url: "Categoria.aspx/ObtenerFormaEditarCategoria",
        parametros: IdCategoria,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCategoria").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCategoria() {
    var pCategoria = new Object();
    pCategoria.Categoria = $("#txtCategoria").val();
    pCategoria.IdGrupo = $("#cmbGrupo").val();
    var validacion = ValidaCategoria(pCategoria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCategoria = pCategoria;
    SetAgregarCategoria(JSON.stringify(oRequest)); 
}

function SetAgregarCategoria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Categoria.aspx/AgregarCategoria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCategoria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCategoria").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdCategoria, pBaja) {
    var pRequest = "{'pIdCategoria':" + pIdCategoria + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Categoria.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCategoria").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCategoria').one('click', '.div_grdCategoria_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCategoria_AI']").children().attr("baja")
                var idCategoria = $(registro).children("td[aria-describedby='grdCategoria_IdCategoria']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCategoria, baja);
            });
        }
    });
}

function EditarCategoria() {
    var pCategoria = new Object();
    pCategoria.IdCategoria = $("#divFormaEditarCategoria").attr("idCategoria");
    pCategoria.Categoria = $("#txtCategoria").val();
    pCategoria.IdGrupo = $("#cmbGrupo").val();
    var validacion = ValidaCategoria(pCategoria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCategoria = pCategoria;
    SetEditarCategoria(JSON.stringify(oRequest));
}

function SetEditarCategoria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Categoria.aspx/EditarCategoria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCategoria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarCategoria").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaCategoria(pCategoria) {
    var errores = "";

    if (pCategoria.Categoria == "")
    { errores = errores + "<span>*</span> El campo categoría esta vacío, favor de capturarlo.<br />"; }

    if (pCategoria.IdGrupo == 0)
    { errores = errores + "<span>*</span> El campo grupo esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
