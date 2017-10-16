//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    
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
                $(".qq-upload-list").html("<li><span class='qq-upload-file'>Favor de subir una imagen.</span></li>");
                $("#divLogo").html("<img src='../images/NoImage.png' />");
                $("#divLogo").attr("archivo", "");
                $(this).dialog("close");
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogAgregarEmpresa').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarEmpresa").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarEmpresa();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarEmpresa').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarEmpresa").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            },
            "Cerrar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogEditarEmpresa').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarEmpresa").remove();
        },
        buttons: {
            "Editar": function() {
                EditarEmpresa();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarEmpresa", function() {
        ObtenerFormaAgregarEmpresa();
    });
    
    $("#grdEmpresa").on("click", ".imgFormaConsultarEmpresa", function() {
        var registro = $(this).parents("tr");
        var Empresa = new Object();
        Empresa.pIdEmpresa = parseInt($(registro).children("td[aria-describedby='grdEmpresa_IdEmpresa']").html());
        ObtenerFormaConsultarEmpresa(JSON.stringify(Empresa));
    });
    
    $('#grdEmpresa').one('click', '.div_grdEmpresa_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdEmpresa_AI']").children().attr("baja")
        var idEmpresa = $(registro).children("td[aria-describedby='grdEmpresa_IdEmpresa']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idEmpresa, baja);
    });
    
    $('#dialogAgregarEmpresa, #dialogEditarEmpresa').on('change', '#cmbPais', function(event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstados(JSON.stringify(request));
        
        var request = new Object();
        request.pIdEstado = 0;
        ObtenerListaMunicipios(JSON.stringify(request));
        
        var request = new Object();
        request.pIdMunicipio = 0;
        ObtenerListaLocalidades(JSON.stringify(request));
    });
    
    $('#dialogAgregarEmpresa, #dialogEditarEmpresa').on('change', '#cmbEstado', function(event) {
        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipios(JSON.stringify(request));
        
        var request = new Object();
        request.pIdMunicipio = 0;
        ObtenerListaLocalidades(JSON.stringify(request));
    });
    
    $('#dialogAgregarEmpresa, #dialogEditarEmpresa').on('change', '#cmbMunicipio', function(event) {
        var request = new Object();
        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidades(JSON.stringify(request));
    });
    
    $('#dialogAgregarEmpresa, #dialogEditarEmpresa').on('keypress', '#txtRFC', function(event) {
        if(!ValidarLetraNumero(event, ""))
        {
           return false;
        }
    });
    
    $("#divSubirLogo").livequery(function() {
        var ctrlSubirLogo = new qq.FileUploader({
            element: document.getElementById('divSubirLogo'),
            action: '../ControladoresSubirArchivos/SubirLogo.ashx',
            allowedExtensions: ["png","jpg","jpeg"],
            onSubmit: function(id, fileName){
                $(".qq-upload-list").empty();
            },
            onComplete: function(id, file, responseJSON) {
                $("#divLogo").html("<img src='../Archivos/EmpresaLogo/" + responseJSON.name + "' title='Logo' onload='javascript:LogoCargado();'/>");
                $("#divLogo").attr("archivo", responseJSON.name);
            }
        });
    });
    
    $('#dialogAgregarEmpresa, #dialogEditarEmpresa').on('click', '#divEliminarImagenLogo', function(event) {
        if($("#divLogo").attr("archivo") != "")
        {
            MostrarMensajeEliminar("¿Esta seguro de eliminar el logo?");
        }
    });
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarEmpresa()
{
    $("#dialogAgregarEmpresa").obtenerVista({
        url: "Empresa.aspx/ObtenerFormaAgregarEmpresa",
        nombreTemplate: "tmplAgregarEmpresa.html",
        despuesDeCompilar: function(pRespuesta) {
            var ctrlSubirLogo = new qq.FileUploader({
                element: document.getElementById('divSubirLogo'),
                action: '../ControladoresSubirArchivos/SubirLogo.ashx',
                allowedExtensions: ["png","jpg","jpeg"],
                debug: true,
                onSubmit: function(id, fileName){
                    $(".qq-upload-list").empty();
                },
                onComplete: function(id, file, responseJSON) {
                    $("#divLogo").html("<img src='../Archivos/EmpresaLogo/" + responseJSON.name + "' title='Logo' />");
                    $("#divLogo").attr("archivo", responseJSON.name);
                }
            });
            $("#dialogAgregarEmpresa").dialog("open");
        }
    });
}

function ObtenerFormaConsultarEmpresa(pIdEmpresa) {
    $("#dialogConsultarEmpresa").obtenerVista({
        nombreTemplate: "tmplConsultarEmpresa.html",
        url: "Empresa.aspx/ObtenerFormaEmpresa",
        parametros: pIdEmpresa,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarEmpresa == 1) {
                $("#dialogConsultarEmpresa").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Empresa = new Object();
                        Empresa.pIdEmpresa = parseInt($("#divFormaConsultarEmpresa").attr("IdEmpresa"));
                        ObtenerFormaEditarEmpresa(JSON.stringify(Empresa))
                    },
                    "Cerrar": function() {
                        $(this).dialog("close");
                    }
                });
                $("#dialogConsultarEmpresa").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarEmpresa").dialog("option", "buttons", {});
                $("#dialogConsultarEmpresa").dialog("option", "height", "100");
            }
            $("#dialogConsultarEmpresa").dialog("open");
        }
    });
}

function ObtenerFormaEditarEmpresa(IdEmpresa) {
    $("#dialogEditarEmpresa").obtenerVista({
        nombreTemplate: "tmplEditarEmpresa.html",
        url: "Empresa.aspx/ObtenerFormaEditarEmpresa",
        parametros: IdEmpresa,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarEmpresa").dialog("open");
        }
    });
}

function ObtenerListaEstados(pRequest){
    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Empresa.aspx/ObtenerListaEstados",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaMunicipios(pRequest){
    $("#cmbMunicipio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Empresa.aspx/ObtenerListaMunicipios",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaLocalidades(pRequest){
    $("#cmbLocalidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Empresa.aspx/ObtenerListaLocalidades",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarEmpresa() {
    var pEmpresa = new Object();
    pEmpresa.RazonSocial = $("#txtRazonSocial").val();
    pEmpresa.Empresa = $("#txtEmpresa").val();
    pEmpresa.RFC = $("#txtRFC").val().toUpperCase();
    pEmpresa.Telefono = $("#txtTelefono").val();
    pEmpresa.Correo = $("#txtCorreo").val();
    pEmpresa.RegimenFiscal = $("#txtRegimenFiscal").val();
    pEmpresa.Dominio = $("#txtDominio").val();
    pEmpresa.Calle = $("#txtCalle").val();
    pEmpresa.NumeroExterior = $("#txtNumeroExterior").val();
    pEmpresa.NumeroInterior = $("#txtNumeroInterior").val();
    pEmpresa.Colonia = $("#txtColonia").val();
    pEmpresa.IdLocalidad = $("#cmbLocalidad").val();
    pEmpresa.CodigoPostal = $("#txtCodigoPostal").val();
    pEmpresa.IdPais = $("#cmbPais").val();
    pEmpresa.IdEstado = $("#cmbEstado").val();
    pEmpresa.IdMunicipio = $("#cmbMunicipio").val();
    pEmpresa.Referencia = $("#txtReferencia").val();
    pEmpresa.Logo = $("#divLogo").attr("archivo");

    var validacion = ValidarEmpresa(pEmpresa);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEmpresa = pEmpresa;
    SetAgregarEmpresa(JSON.stringify(oRequest));
}

function SetAgregarEmpresa(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Empresa.aspx/AgregarEmpresa",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEmpresa").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarEmpresa").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdEmpresa, pBaja) {
    var pRequest = "{'pIdEmpresa':" + pIdEmpresa + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Empresa.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdEmpresa").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdEmpresa').one('click', '.div_grdEmpresa_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEmpresa_AI']").children().attr("baja")
                var idEmpresa = $(registro).children("td[aria-describedby='grdEmpresa_IdEmpresa']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idEmpresa, baja);
            });
        }
    });
}

function EditarEmpresa() {
    var pEmpresa = new Object();
    pEmpresa.IdEmpresa = $("#divFormaEditarEmpresa").attr("idEmpresa");
    pEmpresa.RazonSocial = $("#txtRazonSocial").val();
    pEmpresa.Empresa = $("#txtEmpresa").val();
    pEmpresa.RFC = $("#txtRFC").val().toUpperCase();
    pEmpresa.Telefono = $("#txtTelefono").val();
    pEmpresa.Correo = $("#txtCorreo").val();
    pEmpresa.RegimenFiscal = $("#txtRegimenFiscal").val();
    pEmpresa.Dominio = $("#txtDominio").val();
    pEmpresa.Calle = $("#txtCalle").val();
    pEmpresa.NumeroExterior = $("#txtNumeroExterior").val();
    pEmpresa.NumeroInterior = $("#txtNumeroInterior").val();
    pEmpresa.Colonia = $("#txtColonia").val();
    pEmpresa.IdLocalidad = $("#cmbLocalidad").val();
    pEmpresa.CodigoPostal = $("#txtCodigoPostal").val();
    pEmpresa.IdPais = $("#cmbPais").val();
    pEmpresa.IdEstado = $("#cmbEstado").val();
    pEmpresa.IdMunicipio = $("#cmbMunicipio").val();
    pEmpresa.Referencia = $("#txtReferencia").val();
    pEmpresa.Logo = $("#divLogo").attr("archivo");
    
    var validacion = ValidarEmpresa(pEmpresa);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEmpresa = pEmpresa;
    SetEditarEmpresa(JSON.stringify(oRequest));
}
function SetEditarEmpresa(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Empresa.aspx/EditarEmpresa",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEmpresa").trigger("reloadGrid");
                $("#dialogEditarEmpresa").dialog("close");
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


//-----Validaciones------------------------------------------------------
function ValidarEmpresa(pEmpresa) {
    var errores = "";

    if (pEmpresa.RazonSocial == "")
    { errores = errores + "<span>*</span> El campo razón social esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Empresa == "")
    { errores = errores + "<span>*</span> El campo nombre comercial esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.RFC == "")
    { errores = errores + "<span>*</span> El campo RFC esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.RegimenFiscal == "")
    { errores = errores + "<span>*</span> El campo Régimen fiscal esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Telefono == "")
    { errores = errores + "<span>*</span> El campo teléfono esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Calle == "")
    { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Numero == "")
    { errores = errores + "<span>*</span> El campo numero esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Colonia == "")
    { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.CodigoPostal == "")
    { errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Pais == "")
    { errores = errores + "<span>*</span> El campo país esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Estado == "")
    { errores = errores + "<span>*</span> El campo estado esta vacío, favor de capturarlo.<br />"; }
    if (pEmpresa.Municipio == "")
    { errores = errores + "<span>*</span> El campo municipio esta vacío, favor de capturarlo.<br />"; }
    
    if (pEmpresa.Correo != "") {
        if (ValidarCorreo(pEmpresa.Correo))
        { errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
    }
    
    if (pEmpresa.RFC != "") {
        if (RFCValidoEmpresarial(pEmpresa.RFC) == false)
        { errores = errores + "<span>*</span> El formato del RFC no es valido, favor de capturar un RFC valido.<br />"; }
    }
   
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function LogoCargado(){
    var marginLeft = (250 - parseInt($("#divLogo img").css("width").replace("px",""),10)) / 2;
    $("#divLogo img").css("margin-left", marginLeft);
}