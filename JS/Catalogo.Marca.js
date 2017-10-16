//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Marca");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarMarca", function() {
        ObtenerFormaAgregarMarca();
    });
    
    $("#grdMarca").on("click", ".imgFormaConsultarMarca", function() {
        var registro = $(this).parents("tr");
        var Marca = new Object();
        Marca.pIdMarca = parseInt($(registro).children("td[aria-describedby='grdMarca_IdMarca']").html());
        ObtenerFormaConsultarMarca(JSON.stringify(Marca));
    });
    
    $('#grdMarca').one('click', '.div_grdMarca_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdMarca_AI']").children().attr("baja")
        var idMarca = $(registro).children("td[aria-describedby='grdMarca_IdMarca']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idMarca, baja);
    });
    
    $('#dialogAgregarMarca').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarMarca").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarMarca();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarMarca').dialog({
        autoOpen: false,
        height: 'auto',
        width: '400',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarMarca").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarMarca').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarMarca").remove();
        },
        buttons: {
            "Editar": function() {
                EditarMarca();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogAgregarMarca, #dialogEditarMarca').on('focusin', '#txtCuotaCompra', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });
    
    $('#dialogAgregarMarca, #dialogEditarMarca').on('focusout', '#txtCuotaCompra', function(event) {
        $(this).valorPredeterminado("Moneda");
    });
    
    $('#dialogAgregarMarca, #dialogEditarMarca').on('keypress', '#txtCuotaCompra', function(event) {
        if(!ValidarNumeroPunto(event,$(this).val())) {
            return false;
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarMarca()
{
    $("#dialogAgregarMarca").obtenerVista({
        nombreTemplate: "tmplAgregarMarca.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarMarca").dialog("open");
        }
    });
}

function ObtenerFormaConsultarMarca(pIdMarca) {
    $("#dialogConsultarMarca").obtenerVista({
        nombreTemplate: "tmplConsultarMarca.html",
        url: "Marca.aspx/ObtenerFormaMarca",
        parametros: pIdMarca,
        despuesDeCompilar: function(pRespuesta) {
            var respuesta = pRespuesta.modelo;
            
            if (respuesta.Permisos.puedeConsultarMarca == 1) {                
                $("#dialogConsultarMarca").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Marca = new Object();
                        Marca.IdMarca = parseInt($("#divFormaConsultarMarca").attr("IdMarca"));
                        ObtenerFormaEditarMarca(JSON.stringify(Marca))
                    }
                });
                $("#dialogConsultarMarca").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarMarca").dialog("option", "buttons", {});
                $("#dialogConsultarMarca").dialog("option", "height", "100");
            }
            $("#dialogConsultarMarca").dialog("open");
            $("#txtCuotaCompra").text(formato.moneda(respuesta.CuotaCompra,'$'));
        }
    });
}

function ObtenerFormaEditarMarca(IdMarca) {
    $("#dialogEditarMarca").obtenerVista({
        nombreTemplate: "tmplEditarMarca.html",
        url: "Marca.aspx/ObtenerFormaEditarMarca",
        parametros: IdMarca,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarMarca").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarMarca() {
    var pMarca = new Object();
    pMarca.Marca = $("#txtMarca").val();
    var cuotacompra = QuitarFormatoNumero($("#txtCuotaCompra").val());
    pMarca.CuotaCompra = cuotacompra !='' ? cuotacompra : 0; 
    var validacion = ValidaMarca(pMarca);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pMarca = pMarca;
    SetAgregarMarca(JSON.stringify(oRequest)); 
}

function SetAgregarMarca(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Marca.aspx/AgregarMarca",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdMarca").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarMarca").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdMarca, pBaja) {
    var pRequest = "{'pIdMarca':" + pIdMarca + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Marca.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdMarca").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdMarca').one('click', '.div_grdMarca_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdMarca_AI']").children().attr("baja")
                var idMarca = $(registro).children("td[aria-describedby='grdMarca_IdMarca']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idMarca, baja);
            });
        }
    });
}

function EditarMarca() {
    var pMarca = new Object();
    pMarca.IdMarca = $("#divFormaEditarMarca").attr("idMarca");
    pMarca.Marca = $("#txtMarca").val();
    cuotacompra = QuitarFormatoNumero($("#txtCuotaCompra").val());     
    pMarca.CuotaCompra = cuotacompra !='' ? cuotacompra : 0;    
    var validacion = ValidaMarca(pMarca);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pMarca = pMarca;
    SetEditarMarca(JSON.stringify(oRequest));
}

function SetEditarMarca(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Marca.aspx/EditarMarca",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdMarca").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarMarca").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaMarca(pMarca) {
    var errores = "";

    if (pMarca.Marca == "")
    { errores = errores + "<span>*</span> El campo marca esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
