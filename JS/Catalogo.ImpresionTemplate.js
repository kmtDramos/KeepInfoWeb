//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("ImpresionTemplate");
    });

    //Dialogo Eliminar
    $('#dialogMensajeEliminar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Eliminar": function() {
//                $(".qq-upload-list").html("<li><span class='qq-upload-file'>Favor de subir una imagen.</span></li>");
                $("#divSubirTemplate").attr("archivo", "");
                $("#divSubirTemplateCSS").attr("archivo", "");
                $(this).dialog("close");
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });    

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarImpresionTemplate", function() {
        ObtenerFormaAgregarImpresionTemplate();
    });

    $("#grdImpresionTemplate").on("click", ".imgFormaConsultarImpresionTemplate", function() {
        var registro = $(this).parents("tr");
        var ImpresionTemplate = new Object();
        ImpresionTemplate.pIdImpresionTemplate = parseInt($(registro).children("td[aria-describedby='grdImpresionTemplate_IdImpresionTemplate']").html());
        ObtenerFormaConsultarImpresionTemplate(JSON.stringify(ImpresionTemplate));
    });

    $('#grdImpresionTemplate').on('click', '.div_grdImpresionTemplate_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdImpresionTemplate_AI']").children().attr("baja")
        var idImpresionTemplate = $(registro).children("td[aria-describedby='grdImpresionTemplate_IdImpresionTemplate']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idImpresionTemplate, baja);
    });

    $('#dialogAgregarImpresionTemplate').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarImpresionTemplate").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarImpresionTemplate();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarImpresionTemplate').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarImpresionTemplate").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarImpresionTemplate').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarImpresionTemplate").remove();
        },
        buttons: {
            "Editar": function() {
                EditarImpresionTemplate();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#divSubirTemplate").livequery(function() {
        var ctrlSubirLogo = new qq.FileUploader({
            element: document.getElementById('divSubirTemplate'),
            action: '../ControladoresSubirArchivos/SubirTemplateImpresion.ashx',
            allowedExtensions: ["html", "htm"],
            template: '<div class="qq-uploader">' +
                '<div class="qq-upload-drop-area"></div>' +
                '<div class="qq-upload-container-list"><ul class="qq-upload-list"><li><span class="qq-upload-file"></span></li></ul></div>' +
                '<div class="qq-upload-container-buttons">' +
                '<div id="divEliminarTemplate" class="qq-upload-button">- Borrar</div>' + 
                '<div class="qq-upload-button qq-divBotonSubir">+ Subir...</div></div>' +
                '</div>',
            onSubmit: function(id, fileName) {
                $(".qq-upload-list").empty();
            },
            onComplete: function(id, file, responseJSON) {
                $("#txtRutaTemplate").val(responseJSON.name);
                $("#divSubirTemplate").attr("archivo", responseJSON.name);
                OcultarBloqueo();
            }
        });
    });

    $('#dialogAgregarImpresionTemplate, #dialogEditarImpresionTemplate').on('click', '#divEliminarTemplate', function(event) {
        if ($("#divSubirTemplate").attr("archivo") != "") {
            MostrarMensajeEliminar("¿Esta seguro de eliminar el template?");
        }
    });

    $("#divSubirTemplateCSS").livequery(function() {
        var ctrlSubirLogo = new qq.FileUploader({
            element: document.getElementById('divSubirTemplateCSS'),
            action: '../ControladoresSubirArchivos/SubirTemplateImpresion.ashx',
            allowedExtensions: ["css"],
            template: '<div class="qq-uploader">' +
                '<div class="qq-upload-drop-area"></div>' +
                '<div class="qq-upload-container-list"><ul class="qq-upload-list"><li><span class="qq-upload-file"></span></li></ul></div>' +
                '<div class="qq-upload-container-buttons">' +
                '<div id="divEliminarTemplateCSS" class="qq-upload-button">- Borrar</div>' +
                '<div class="qq-upload-button qq-divBotonSubir">+ Subir...</div></div>' +
                '</div>',
            onSubmit: function(id, fileName) {
                $(".qq-upload-list").empty();
            },
            onComplete: function(id, file, responseJSON) {
                $("#txtRutaCSS").val(responseJSON.name);
                $("#divSubirTemplateCSS").attr("archivo", responseJSON.name);
                OcultarBloqueo();
            }
        });
    });

    $('#dialogAgregarImpresionTemplate, #dialogEditarImpresionTemplate').on('click', '#divEliminarTemplateCSS', function(event) {
        if ($("#divSubirTemplateCSS").attr("archivo") != "") {
            MostrarMensajeEliminar("¿Esta seguro de eliminar la hoja de estilo?");
        }
    });

});

    
//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarImpresionTemplate() {

    $("#dialogAgregarImpresionTemplate").obtenerVista({
        url: "ImpresionTemplate.aspx/ObtenerFormaAgregarImpresionTemplate",
        nombreTemplate: "tmplAgregarImpresionTemplate.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarImpresionTemplate").dialog("open");
        }
    });
}

function ObtenerFormaConsultarImpresionTemplate(pIdImpresionTemplate) {
    $("#dialogConsultarImpresionTemplate").obtenerVista({
        nombreTemplate: "tmplConsultarImpresionTemplate.html",
        url: "ImpresionTemplate.aspx/ObtenerFormaImpresionTemplate",
        parametros: pIdImpresionTemplate,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarImpresionTemplate == 1) {
                $("#dialogConsultarImpresionTemplate").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var ImpresionTemplate = new Object();
                        ImpresionTemplate.IdImpresionTemplate = parseInt($("#divFormaConsultarImpresionTemplate").attr("IdImpresionTemplate"));
                        ObtenerFormaEditarImpresionTemplate(JSON.stringify(ImpresionTemplate))
                    }
                });
                $("#dialogConsultarImpresionTemplate").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarImpresionTemplate").dialog("option", "buttons", {});
                $("#dialogConsultarImpresionTemplate").dialog("option", "height", "100");
            }
            $("#dialogConsultarImpresionTemplate").dialog("open");
        }
    });
}

function ObtenerFormaEditarImpresionTemplate(IdImpresionTemplate) {
    $("#dialogEditarImpresionTemplate").obtenerVista({
        nombreTemplate: "tmplEditarImpresionTemplate.html",
        url: "ImpresionTemplate.aspx/ObtenerFormaEditarImpresionTemplate",
        parametros: IdImpresionTemplate,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarImpresionTemplate").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarImpresionTemplate() {
    var pImpresionTemplate = new Object();
    pImpresionTemplate.IdEmpresa = $("#cmbEmpresa").val();
    pImpresionTemplate.IdImpresionDocumento = $("#cmbImpresionDocumento").val();
    pImpresionTemplate.RutaTemplate = $("#txtRutaTemplate").val();
    pImpresionTemplate.RutaCSS = $("#txtRutaCSS").val();

    var validacion = ValidaImpresionTemplate(pImpresionTemplate);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pImpresionTemplate = pImpresionTemplate;
    SetAgregarImpresionTemplate(JSON.stringify(oRequest)); 
}

function SetAgregarImpresionTemplate(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "ImpresionTemplate.aspx/AgregarImpresionTemplate",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdImpresionTemplate").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarImpresionTemplate").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdImpresionTemplate, pBaja) {
    var pRequest = "{'pIdImpresionTemplate':" + pIdImpresionTemplate + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "ImpresionTemplate.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdImpresionTemplate").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdImpresionTemplate').one('click', '.div_grdImpresionTemplate_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdImpresionTemplate_AI']").children().attr("baja")
                var idImpresionTemplate = $(registro).children("td[aria-describedby='grdImpresionTemplate_IdImpresionTemplate']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idImpresionTemplate, baja);
            });
        }
    });
}

function EditarImpresionTemplate() {
    var pImpresionTemplate = new Object();
    pImpresionTemplate.IdImpresionTemplate = $("#divFormaEditarImpresionTemplate").attr("idImpresionTemplate");
    pImpresionTemplate.IdEmpresa = $("#cmbEmpresa").val();
    pImpresionTemplate.IdImpresionDocumento = $("#cmbImpresionDocumento").val();
    pImpresionTemplate.RutaTemplate = $("#txtRutaTemplate").val();
    pImpresionTemplate.RutaCSS = $("#txtRutaCSS").val();
    
    var validacion = ValidaImpresionTemplate(pImpresionTemplate);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pImpresionTemplate = pImpresionTemplate;
    SetEditarImpresionTemplate(JSON.stringify(oRequest));
}

function SetEditarImpresionTemplate(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "ImpresionTemplate.aspx/EditarImpresionTemplate",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdImpresionTemplate").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarImpresionTemplate").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaImpresionTemplate(pImpresionTemplate) {
    var errores = "";

    if (pImpresionTemplate.IdEmpresa == "0")
    { errores = errores + "<span>*</span> El campo empresa esta vacío, favor de capturarlo.<br />"; }

    if (pImpresionTemplate.IdImpresionDocumento == "0")
    { errores = errores + "<span>*</span> El campo impresión documento esta vacío, favor de capturarlo.<br />"; }

    if (pImpresionTemplate.RutaTemplate == "")
    { errores = errores + "<span>*</span> El campo ruta template esta vacío, favor de capturarlo.<br />"; }
        
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
