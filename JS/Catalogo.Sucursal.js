//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarSucursal", function() {
        ObtenerFormaAgregarSucursal();
    });

    $('#grdSucursal').one('click', '.div_grdSucursal_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdSucursal_AI']").children().attr("baja")
        var idSucursal = $(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idSucursal, baja);
    });

    $('#grdCuentaBancaria').one('click', '.div_grdCuentaBancaria_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancaria_AI']").children().attr("baja")
        var idCuentaBancaria = $(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatusCuentaBancaria(idCuentaBancaria, baja);
    });

    $("#grdSucursal").on("click", ".imgFormaConsultarSucursal", function() {
        var registro = $(this).parents("tr");
        var Sucursal = new Object();
        Sucursal.pIdSucursal = parseInt($(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html());
        ObtenerFormaConsultarSucursal(JSON.stringify(Sucursal))
    });

    $("#grdSucursal").on("click", ".imgFormaDivisionAsignada", function() {
        var registro = $(this).parents("tr");
        var idSucursal = $(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html();
        var oRequest = new Object();
        oRequest.pIdSucursal = idSucursal;
        ObtenerFormaDivisionAsignada(JSON.stringify(oRequest));
    });

    $("#grdSucursal").on("click", ".imgFormaCuentaBancariaAsignada", function() {
        var registro = $(this).parents("tr");
        var idSucursal = $(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html();
        var oRequest = new Object();
        oRequest.pIdSucursal = idSucursal;
        ObtenerFormaCuentaBancariaAsignada(JSON.stringify(oRequest));
    });

    $("#dialogConsultarDivisionAsignada").on("click", "#ulDivisionesDisponibles li", function() {
        var registro = $(this);
        var Division = new Object();
        Division.IdDivision = $(registro).attr("idDivision");
        Division.Division = $(registro).attr("division");
        AgregarEnrolamiento(registro, Division);
    });

    $("#dialogConsultarCuentaBancariaAsignada").on("click", "#ulCuentaBancariasDisponibles li", function() {
        var registro = $(this);
        var CuentaBancaria = new Object();
        CuentaBancaria.IdCuentaBancaria = $(registro).attr("idCuentaBancaria");
        CuentaBancaria.CuentaBancaria = $(registro).attr("CuentaBancaria");
        AgregarEnrolamientoCuentaBancaria(registro, CuentaBancaria);
    });

    $("#dialogConsultarDivisionAsignada").on("click", ".Eliminar", function() {
        var registro = $(this).parents("li");
        var Division = new Object();
        Division.IdDivision = $(registro).attr("idDivision");
        Division.Division = $(registro).attr("division");
        EliminarEnrolamiento(registro, Division);
    });

    $("#dialogConsultarCuentaBancariaAsignada").on("click", ".Eliminar", function() {
        var registro = $(this).parents("li");
        var CuentaBancaria = new Object();
        CuentaBancaria.IdCuentaBancaria = $(registro).attr("idCuentaBancaria");
        CuentaBancaria.CuentaBancaria = $(registro).attr("CuentaBancaria");
        EliminarEnrolamientoCuentaBancaria(registro, CuentaBancaria);
    });

    $("#dialogConsultarDivisionAsignada").on("click", "#divDivisionesDisponibles", function() {
        TodasDivisionesAAsignadas();
    });

    $("#dialogConsultarCuentaBancariaAsignada").on("click", "#divCuentaBancariasDisponibles", function() {
        TodasCuentaBancariasAsignadas();
    });

    $("#dialogConsultarDivisionAsignada").on("click", "#divDivisionesAsignadas", function() {
        TodasDivisionesADisponibles();
    });

    $("#dialogConsultarCuentaBancariaAsignada").on("click", "#divCuentaBancariasAsignadas", function() {
        TodasCuentaBancariasDisponibles();
    });

    $("#grdSucursal").on("click", ".imgFormaConsultarCuentaBancaria", function() {
        var registro = $(this).parents("tr");
        var Sucursal = new Object();
        Sucursal.pIdSucursal = parseInt($(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html());
        ObtenerFormaConsultarCuentaBancaria(JSON.stringify(Sucursal));
    });

    $("#grdSucursal").on("click", ".imgFormaConsultarSerieFactura", function() {
        var registro = $(this).parents("tr");
        var Sucursal = new Object();
        Sucursal.pIdSucursal = parseInt($(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html());
        ObtenerFormaConsultarSerieFactura(JSON.stringify(Sucursal));
    });

    $("#grdSucursal").on("click", ".imgFormaConsultarSerieNotaCredito", function() {
        var registro = $(this).parents("tr");
        var Sucursal = new Object();
        Sucursal.pIdSucursal = parseInt($(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html());
        ObtenerFormaConsultarSerieNotaCredito(JSON.stringify(Sucursal));
    });

    $("#grdSucursal").on("click", ".imgFormaConsultarSeriePago", function () {
        var registro = $(this).parents("tr");
        var Sucursal = new Object();
        Sucursal.pIdSucursal = parseInt($(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html()); 
        ObtenerFormaConsultarSeriePago(JSON.stringify(Sucursal));
    });

    $('#grdSucursal').on('click', '.div_grdSucursal_Timbrado', function(event) {
        var registro = $(this).parents("tr");
        var Sucursal = new Object();
        Sucursal.pIdSucursal = parseInt($(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html());
        ObtenerFormaConsultarRutaCFDI(JSON.stringify(Sucursal));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCuentaBancaria", function() {
        var Sucursal = new Object();
        Sucursal.pIdSucursal = $("#divFormaCuentaBancaria").attr("IdSucursal");
        ObtenerFormaAgregarCuentaBancaria(JSON.stringify(Sucursal));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarSerieFactura", function() {
        var Sucursal = new Object();
        Sucursal.pIdSucursal = $("#divFormaSerieFactura").attr("IdSucursal");
        ObtenerFormaAgregarSerieFactura(JSON.stringify(Sucursal));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarSerieNotaCredito", function() {
        var Sucursal = new Object();
        Sucursal.pIdSucursal = $("#divFormaSerieNotaCredito").attr("IdSucursal");
        ObtenerFormaAgregarSerieNotaCredito(JSON.stringify(Sucursal));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarSeriePago", function () {
        var Sucursal = new Object();
        Sucursal.pIdSucursal = $("#divFormaSeriePago").attr("IdSucursal");
        ObtenerFormaAgregarSeriePago(JSON.stringify(Sucursal));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarRutaCFDI", function() {
        var Sucursal = new Object();
        Sucursal.pIdSucursal = $("#divFormaRutaCFDI").attr("IdSucursal");
        ObtenerFormaAgregarRutaCFDI(JSON.stringify(Sucursal));
    });

    $('#dialogAgregarSucursal, #dialogEditarSucursal').on('change', '#cmbPais', function(event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstados(JSON.stringify(request));

        var request = new Object();
        request.pIdEstado = 0;
        ObtenerListaMunicipios(JSON.stringify(request));
    });

    $('#dialogAgregarSucursal, #dialogEditarSucursal').on('change', '#cmbEstado', function(event) {
        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipios(JSON.stringify(request));

        var request = new Object();
        request.pIdMunicipio = 0;
        ObtenerListaLocalidades(JSON.stringify(request));
    });

    $('#dialogAgregarSucursal, #dialogEditarSucursal').on('change', '#cmbMunicipio', function(event) {
        var request = new Object();
        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidades(JSON.stringify(request));
    });


    $('#dialogAgregarSucursal, #dialogEditarSucursal').on('change', '#cmbIVA', function(event) {
        $("#txtIVA").val($("#cmbIVA option:selected").html());
        $("#txtIVA").attr("disabled", "true");
    });

    $('#dialogAgregarSucursal').on('click', '#chkDireccionFiscal', function(event) {
        if ($(this).is(':checked')) {
            if ($("#cmbEmpresa").val() == 0) {
                MostrarMensajeError("Es necesario seleccionar a la empresa para obtener la dirección fiscal.");
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', 'checked');
                var request = new Object();
                request.pIdEmpresa = $("#cmbEmpresa").val();
                var pRequest = JSON.stringify(request);
                $("#tblFormaSucursal-DireccionFiscal").obtenerVista({
                    nombreTemplate: "tmplConsultarSucursal-DireccionFiscal.html",
                    url: "Sucursal.aspx/ObtenerDireccionFiscalEmpresa",
                    parametros: pRequest,
                    despuesDeCompilar: function(pRespuesta) {
                        $("#tblFormaSucursal-DireccionExpedicion").addClass("etiquetaOculta");
                        $("#tblFormaSucursal-DireccionFiscal").removeClass("etiquetaOculta");
                    }
                });
            }
        }
        else {
            $(this).attr('checked', false);
            $("#tblFormaSucursal-DireccionExpedicion").removeClass("etiquetaOculta");
            $("#tblFormaSucursal-DireccionFiscal").addClass("etiquetaOculta");
        }
    });

    $('#dialogAgregarSucursal').on('change', '#cmbEmpresa', function(event) {
        if ($("#chkDireccionFiscal").is(':checked')) {
            if ($("#cmbEmpresa").val() == 0) {
                $("#chkDireccionFiscal").attr('checked', false);
                $("#tblFormaSucursal-DireccionExpedicion").removeClass("etiquetaOculta");
                $("#tblFormaSucursal-DireccionFiscal").addClass("etiquetaOculta");
            }
            else {
                var request = new Object();
                request.pIdEmpresa = $("#cmbEmpresa").val();
                var pRequest = JSON.stringify(request);
                $("#tblFormaSucursal-DireccionFiscal").obtenerVista({
                    nombreTemplate: "tmplConsultarSucursal-DireccionFiscal.html",
                    url: "Sucursal.aspx/ObtenerDireccionFiscalEmpresa",
                    parametros: pRequest,
                    despuesDeCompilar: function(pRespuesta) {
                        $("#tblFormaSucursal-DireccionExpedicion").addClass("etiquetaOculta");
                        $("#tblFormaSucursal-DireccionFiscal").removeClass("etiquetaOculta");
                    }
                });
            }
        }
    });

    $('#dialogEditarSucursal').on('click', '#chkDireccionFiscal', function(event) {
        if ($("#chkDireccionFiscal").is(':checked')) {
            $("#chkDireccionFiscal").attr('checked', 'checked');
            var request = new Object();
            request.pIdEmpresa = $("#cmbEmpresa").val();
            var pRequest = JSON.stringify(request);

            $("#tblFormaSucursal-DireccionFiscal").obtenerVista({
                nombreTemplate: "tmplConsultarSucursal-DireccionFiscal.html",
                url: "Sucursal.aspx/ObtenerDireccionFiscalEmpresa",
                parametros: pRequest,
                despuesDeCompilar: function(pRespuesta) {
                    $("#tblFormaSucursal-DireccionExpedicion").addClass("etiquetaOculta");
                    $("#tblFormaSucursal-DireccionFiscal").removeClass("etiquetaOculta");
                }
            });
        }
        else {
            $("#chkDireccionFiscal").attr('checked', false);
            var request = new Object();
            request.pIdSucursal = $("#divFormaEditarSucursal").attr("idSucursal");
            var pRequest = JSON.stringify(request);
            $("#tblFormaSucursal-DireccionExpedicion").obtenerVista({
                nombreTemplate: "tmplConsultarSucursal-DireccionExpedicion.html",
                url: "Sucursal.aspx/ObtenerDireccionExpedicionEmpresa",
                parametros: pRequest,
                despuesDeCompilar: function(pRespuesta) {
                    $("#tblFormaSucursal-DireccionExpedicion").removeClass("etiquetaOculta");
                    $("#tblFormaSucursal-DireccionFiscal").addClass("etiquetaOculta");
                }
            });
        }
    });

    $("#grdSucursal").on("click", ".imgFormaAsignarConexionContpaq", function() {
        var registro = $(this).parents("tr");
        var Sucursal = new Object();
        Sucursal.pIdSucursal = parseInt($(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html());
        ObtenerFormaAsignarConexionContpaq(JSON.stringify(Sucursal));
    });

    $('#dialogAgregarSucursal').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarSucursal").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarSucursal();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarSucursal').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarSucursal").remove();
        },
        buttons: {
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });


    $('#dialogEditarSucursal').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarSucursal").remove();
        },
        buttons: {
            "Editar": function() {
                EditarSucursal();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogEditarSerieFactura').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarSerieFactura").remove();
        },
        buttons: {
            "Editar": function() {
                EditarSerieFactura();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogEditarSerieNotaCredito').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarSerieNotaCredito").remove();
        },
        buttons: {
            "Editar": function() {
                EditarSerieNotaCredito();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogEditarSeriePago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaEditarSeriePago").remove();
        },
        buttons: {
            "Editar": function () {
                EditarSeriePago();
            },
            "Cancelar": function () {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogEditarRutaCFDI').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarRutaCFDI").remove();
        },
        buttons: {
            "Editar": function() {
                EditarRutaCFDI();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarCuentaBancaria').dialog({
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

    $('#dialogConsultarSerieFactura').dialog({
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

    $('#dialogConsultarSerieNotaCredito').dialog({
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

    $('#dialogConsultarSeriePago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
        },
        buttons: {
        }
    });

    $('#dialogConsultarRutaCFDI').dialog({
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

    $('#dialogConsultarDivisionAsignada').dialog({
        autoOpen: false,
        height: '600',
        width: '832',
        modal: true,
        draggable: true,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function(event, ui) {
        },
        buttons: {
            "Guardar cambios": function() {
                AgregarDivisionAsignadaASucursal();
            }
        }
    });

    $('#dialogConsultarCuentaBancariaAsignada').dialog({
        autoOpen: false,
        height: '600',
        width: '832',
        modal: true,
        draggable: true,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function(event, ui) {
        },
        buttons: {
            "Guardar cambios": function() {
                AgregarCuentaBancariaAsignadaASucursal();
            }
        }
    });


    $('#dialogAgregarCuentaBancaria').dialog({
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
                AgregarCuentaBancaria();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarSerieFactura').dialog({
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
                AgregarSerieFactura();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarSerieNotaCredito').dialog({
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
                AgregarSerieNotaCredito();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarSeriePago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
        },
        buttons: {
            "Guardar": function () {
                AgregarSeriePago();
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarRutaCFDI').dialog({
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
                AgregarRutaCFDI();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarSerieFacturaConsultar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarSerieFacturaConsultar").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarSerieNotaCreditoConsultar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarSerieNotaCreditoConsultar").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarSeriePagoConsultar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaConsultarSeriePagoConsultar").remove();
        },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarRutaCFDIConsultar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarRutaCFDIConsultar").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAsignarConexionContpaq').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $(".divFormaAsignarConexionContpaq").remove();
        },
        buttons: {
            "Guardar": function() {
                AsignarConexionContpaq();
            }
        }
    });
});

//-----------AJAX-----------//
//-Funciones de Acciones-//
function SetCambiarEstatus(pIdSucursal, pBaja) {
    var pRequest = "{'pIdSucursal':" + pIdSucursal + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdSucursal").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdSucursal').one('click', '.div_grdSucursal_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSucursal_AI']").children().attr("baja")
                var idSucursal = $(registro).children("td[aria-describedby='grdSucursal_IdSucursal']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idSucursal, baja);
            });
        }
    });
}

function SetCambiarEstatusCuentaBancaria(pIdCuentaBancaria, pBaja) {
    var pRequest = "{'pIdCuentaBancaria':" + pIdCuentaBancaria + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/CambiarEstatusCuentaBancaria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCuentaBancaria").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCuentaBancaria').one('click', '.div_grdCuentaBancaria_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancaria_AI']").children().attr("baja")
                var idCuentaBancaria = $(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusCuentaBancaria(idCuentaBancaria, baja);
            });
        }
    });
}

function SetCambiarEstatusSerieFactura(pIdSerieFactura, pBaja) {
    var pRequest = "{'pIdSerieFactura':" + pIdSerieFactura + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/CambiarEstatusSerieFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdSerieFactura").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdSerieFactura').one('click', '.div_grdSerieFactura_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSerieFactura_AI']").children().attr("baja")
                var idSerieFactura = $(registro).children("td[aria-describedby='grdSerieFactura_IdSerieFactura']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusSerieFactura(idSerieFactura, baja);
            });
        }
    });
}

function SetCambiarEstatusSerieNotaCredito(pIdSerieNotaCredito, pBaja) {
    var pRequest = "{'pIdSerieNotaCredito':" + pIdSerieNotaCredito + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/CambiarEstatusSerieNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdSerieNotaCredito").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdSerieNotaCredito').one('click', '.div_grdSerieNotaCredito_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSerieNotaCredito_AI']").children().attr("baja")
                var idSerieNotaCredito = $(registro).children("td[aria-describedby='grdSerieNotaCredito_IdSerieNotaCredito']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusSerieNotaCredito(idSerieNotaCredito, baja);
            });
        }
    });
}

function SetCambiarEstatusSeriePago(pIdSeriePago, pBaja) {
    var pRequest = "{'pIdSeriePago':" + pIdSeriePago + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/CambiarEstatusSeriePago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            $("#grdSeriePago").trigger("reloadGrid");
        },
        complete: function () {
            $('#grdSeriePago').one('click', '.div_grdSeriePago_AI', function (event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSeriePago_AI']").children().attr("baja")
                var idSeriePago = $(registro).children("td[aria-describedby='grdSeriePago_IdSeriePago']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusSeriePago(idSeriePago, baja);
            });
        }
    });
}

function SetCambiarEstatusRutaCFDI(pIdRutaCFDI, pBaja) {
    var pRequest = "{'pIdRutaCFDI':" + pIdRutaCFDI + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/CambiarEstatusRutaCFDI",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdRutaCFDI").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdRutaCFDI').one('click', '.div_grdRutaCFDI_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdRutaCFDI_AI']").children().attr("baja")
                var idRutaCFDI = $(registro).children("td[aria-describedby='grdRutaCFDI_IdRutaCFDI']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusRutaCFDI(idRutaCFDI, baja);
            });
        }
    });
}

function AgregarSucursal() {
    var pSucursal = new Object();
    pSucursal.Sucursal = $("#txtSucursal").val();
    pSucursal.IdEmpresa = $("#cmbEmpresa").val();
    pSucursal.Telefono = $("#txtTelefono").val();
    pSucursal.Correo = $("#txtCorreo").val();
    pSucursal.Dominio = $("#txtDominio").val();
    pSucursal.Calle = $("#txtCalle").val();
    pSucursal.NumeroExterior = $("#txtNumeroExterior").val();
    pSucursal.NumeroInterior = $("#txtNumeroInterior").val();
    pSucursal.Colonia = $("#txtColonia").val();
    pSucursal.IdLocalidad = $("#cmbLocalidad").val();
    pSucursal.IdMunicipio = $("#cmbMunicipio").val();
    pSucursal.CodigoPostal = $("#txtCodigoPostal").val();
    pSucursal.Referencia = $("#txtReferencia").val();
    pSucursal.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    pSucursal.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pSucursal.IdIVA = $("#cmbIVA").val();
    pSucursal.Alias = $("#txtAlias").val();
    pSucursal.DireccionFiscal = 0;
    if ($("#chkDireccionFiscal").is(':checked')) {
        pSucursal.DireccionFiscal = 1;
    }
    
    pSucursal.IVAActual = $("#txtIVA").val();
    var validacion = ValidaSucursal(pSucursal);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSucursal = pSucursal;
    SetAgregarSucursal(JSON.stringify(oRequest));
}

function AgregarDivisionAsignadaASucursal() {
    var pDivision = new Object();
    var Divisiones = [];
    var Contador = 0;

    pDivision.IdSucursal = $("#divFormaAsignarDivisionesASucursal").attr("idSucursal");

    $("#ulDivisionesAsignadas li").each(function(i, e) {
        Contador += 1;
        var Division = new Object();
        var registro = $(this);
        Division.IdDivision = $(registro).attr("idDivision");
        Divisiones.push(Division);
    });
    var oRequest = new Object();
    pDivision.Divisiones = Divisiones;
    oRequest.pDivision = pDivision;
    SetAgregarDivisionSucursal(JSON.stringify(oRequest))
}

function AgregarCuentaBancariaAsignadaASucursal() {
    var pCuentaBancaria = new Object();
    var CuentaBancarias = [];
    var Contador = 0;

    pCuentaBancaria.IdSucursal = $("#divFormaAsignarCuentaBancariasASucursal").attr("idSucursal");

    $("#ulCuentaBancariasAsignadas li").each(function(i, e) {
        Contador += 1;
        var CuentaBancaria = new Object();
        var registro = $(this);
        CuentaBancaria.IdCuentaBancaria = $(registro).attr("idCuentaBancaria");
        CuentaBancarias.push(CuentaBancaria);
    });
    var oRequest = new Object();
    pCuentaBancaria.CuentaBancarias = CuentaBancarias;
    oRequest.pCuentaBancaria = pCuentaBancaria;
    SetAgregarCuentaBancariaSucursal(JSON.stringify(oRequest))
}

function SetAgregarDivisionSucursal(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarDivisionSucursal",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogConsultarDivisionAsignada").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarDivisionAsignada").dialog("close");
        }
    });
}

function SetAgregarCuentaBancariaSucursal(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarCuentaBancariaSucursal",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {                
                $("#dialogConsultarCuentaBancariaAsignada").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarCuentaBancariaAsignada").dialog("close");
        }
    });
}

function AgregarEnrolamiento(pElemento, pDivision) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulDivisionesAsignadas").obtenerVista({
        nombreTemplate: "tmplAgregarDivisionAEnrolarSucursal.html",
        modelo: pDivision,
        remplazarVista: false,
        efecto: "slide"
    });
}

function AgregarEnrolamientoCuentaBancaria(pElemento, pCuentaBancaria) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulCuentaBancariasAsignadas").obtenerVista({
        nombreTemplate: "tmplAgregarCuentaBancariaAEnrolarSucursal.html",
        modelo: pCuentaBancaria,
        remplazarVista: false,
        efecto: "slide"
    });
}

function EliminarEnrolamiento(pElemento, pDivision) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulDivisionesDisponibles").obtenerVista({
        nombreTemplate: "tmplEliminarDivisionSucursal.html",
        modelo: pDivision,
        remplazarVista: false,
        efecto: "slide"
    });
}

function EliminarEnrolamientoCuentaBancaria(pElemento, pCuentaBancaria) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulCuentaBancariasDisponibles").obtenerVista({
        nombreTemplate: "tmplEliminarCuentaBancariaSucursal.html",
        modelo: pCuentaBancaria,
        remplazarVista: false,
        efecto: "slide"
    });
}

function TodasDivisionesAAsignadas() {
    var DivisionesEnrolar = new Object();
    var Divisiones = [];
    var Contador = 0;

    DivisionesEnrolar.IdSucursal = $("#divFormaAsignarDivisionesASucursal").attr("idSucursal");

    $("#ulDivisionesDisponibles li").each(function(i, e) {
        Contador += 1;
        var Division = new Object();
        var registro = $(this);
        Division.IdDivision = $(registro).attr("idDivision");
        Division.Division = $(registro).attr("Division");
        AgregarEnrolamiento(registro, Division);
    });

    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay divisiones para asignar.<br />"); return false;
    }
}

function TodasCuentaBancariasAsignadas() {
    var CuentaBancariasEnrolar = new Object();
    var CuentaBancarias = [];
    var Contador = 0;

    CuentaBancariasEnrolar.IdSucursal = $("#divFormaAsignarCuentaBancariasASucursal").attr("idSucursal");

    $("#ulCuentaBancariasDisponibles li").each(function(i, e) {
        Contador += 1;
        var CuentaBancaria = new Object();
        var registro = $(this);
        CuentaBancaria.IdCuentaBancaria = $(registro).attr("idCuentaBancaria");
        CuentaBancaria.CuentaBancaria = $(registro).attr("CuentaBancaria");
        AgregarEnrolamientoCuentaBancaria(registro, CuentaBancaria);
    });

    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay cuentas bancarias para asignar.<br />"); return false;
    }
}


function TodasDivisionesADisponibles() {
    var DivisionesEnrolar = new Object();
    var Divisiones = [];
    var Contador = 0;

    DivisionesEnrolar.IdSucursal = $("#divFormaAsignarDivisionesASucursal").attr("idSucursal");

    $("#ulDivisionesAsignadas li").each(function(i, e) {
        Contador += 1;
        var Division = new Object();
        var registro = $(this);
        Division.IdDivision = $(registro).attr("idDivision");
        Division.Division = $(registro).attr("Division");
        EliminarEnrolamiento(registro, Division);
    });

    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay divisiones para eliminar.<br />"); return false;
    }
}

function TodasCuentaBancariasDisponibles() {
    var CuentaBancariasEnrolar = new Object();
    var CuentaBancarias = [];
    var Contador = 0;

    CuentaBancariasEnrolar.IdSucursal = $("#divFormaAsignarCuentaBancariasASucursal").attr("idSucursal");

    $("#ulCuentaBancariasAsignadas li").each(function(i, e) {
        Contador += 1;
        var CuentaBancaria = new Object();
        var registro = $(this);
        CuentaBancaria.IdCuentaBancaria = $(registro).attr("idCuentaBancaria");
        CuentaBancaria.CuentaBancaria = $(registro).attr("CuentaBancaria");
        EliminarEnrolamientoCuentaBancaria(registro, CuentaBancaria);
    });

    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay cuentas bancarias para eliminar.<br />"); return false;
    }
}


function SetAgregarSucursal(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarSucursal",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogAgregarSucursal").dialog("close");
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

function EditarSucursal() {
    var pSucursal = new Object();
    pSucursal.IdSucursal = $("#divFormaEditarSucursal").attr("idSucursal");
    pSucursal.Sucursal = $("#txtSucursal").val();
    pSucursal.IdEmpresa = $("#cmbEmpresa").val();
    pSucursal.Telefono = $("#txtTelefono").val();
    pSucursal.Correo = $("#txtCorreo").val();
    pSucursal.Dominio = $("#txtDominio").val();
    pSucursal.Calle = $("#txtCalle").val();
    pSucursal.NumeroExterior = $("#txtNumeroExterior").val();
    pSucursal.NumeroInterior = $("#txtNumeroInterior").val();
    pSucursal.Colonia = $("#txtColonia").val();
    pSucursal.IdLocalidad = $("#cmbLocalidad").val();
    pSucursal.IdMunicipio = $("#cmbMunicipio").val();
    pSucursal.CodigoPostal = $("#txtCodigoPostal").val();
    pSucursal.Referencia = $("#txtReferencia").val();
    pSucursal.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    pSucursal.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pSucursal.IdIVA = $("#cmbIVA").val();
    pSucursal.Alias = $("#txtAlias").val();
    pSucursal.DireccionFiscal = 0;    
    
    if ($("#chkDireccionFiscal").is(':checked')) {
        pSucursal.DireccionFiscal = 1;
    }
    pSucursal.IVAActual = $("#txtIVA").val();
    var validacion = ValidaSucursal(pSucursal);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSucursal = pSucursal;
    SetEditarSucursal(JSON.stringify(oRequest));
}

function EditarSerieFactura() {
    var pSerieFactura = new Object();
    pSerieFactura.IdSerieFactura = $("#divFormaEditarSerieFactura").attr("idSerieFactura");
    pSerieFactura.SerieFactura = $("#txtSerieFactura").val();
    if ($("#chkTimbrado").is(':checked')) {
        pSerieFactura.Timbrado = 1;
    }
    else {
        pSerieFactura.Timbrado = 0;
    }

    if ($("#chkParcial").is(':checked')) {
        pSerieFactura.EsParcialidad = 1;
    }
    else {
        pSerieFactura.EsParcialidad = 0;
    }
    
    if ($("#chkEsVenta").is(':checked')) {
        pSerieFactura.EsVenta = 1;
    }
    else {
        pSerieFactura.EsVenta = 0;
    }
    var validacion = ValidaSerieFactura(pSerieFactura);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSerieFactura = pSerieFactura;
    SetEditarSerieFactura(JSON.stringify(oRequest));
}

function EditarSerieNotaCredito() {
    var pSerieNotaCredito = new Object();
    pSerieNotaCredito.IdSerieNotaCredito = $("#divFormaEditarSerieNotaCredito").attr("idSerieNotaCredito");
    pSerieNotaCredito.SerieNotaCredito = $("#txtSerieNotaCredito").val();
    var validacion = ValidaSerieNotaCredito(pSerieNotaCredito);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSerieNotaCredito = pSerieNotaCredito;
    SetEditarSerieNotaCredito(JSON.stringify(oRequest));
}

function EditarSeriePago() {
    var pSeriePago = new Object();
    pSeriePago.IdSeriePago = $("#divFormaEditarSeriePago").attr("idSeriePago");
    pSeriePago.SeriePago = $("#txtSeriePago").val();
    var validacion = ValidaSeriePago(pSeriePago);
    if (validacion != "") { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSeriePago = pSeriePago;
    SetEditarSeriePago(JSON.stringify(oRequest));
}

function EditarRutaCFDI() {
    var pRutaCFDI = new Object();
    pRutaCFDI.IdRutaCFDI = $("#divFormaEditarRutaCFDI").attr("idRutaCFDI");
    pRutaCFDI.RutaCFDI = $("#txtRutaCFDI").val();
    pRutaCFDI.TipoRuta = $("#cmbTipoRuta").val();
    var validacion = ValidaRutaCFDI(pRutaCFDI);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pRutaCFDI = pRutaCFDI;
    SetEditarRutaCFDI(JSON.stringify(oRequest));
}

function SetEditarSucursal(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/EditarSucursal",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSucursal").trigger("reloadGrid");                
                $("#dialogEditarSucursal").dialog("close");
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

function SetEditarSerieFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/EditarSerieFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSerieFactura").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogEditarSerieFactura").dialog("close");
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

function SetEditarSerieNotaCredito(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/EditarSerieNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSerieNotaCredito").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogEditarSerieNotaCredito").dialog("close");
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

function SetEditarSeriePago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/EditarSeriePago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSeriePago").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogEditarSeriePago").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function SetEditarRutaCFDI(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/EditarRutaCFDI",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdRutaCFDI").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogEditarRutaCFDI").dialog("close");
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

function FiltroCuentaBancaria() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCuentaBancaria').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCuentaBancaria').getGridParam('page');
    request.pColumnaOrden = $('#grdCuentaBancaria').getGridParam('sortname');
    request.pTipoOrden = $('#grdCuentaBancaria').getGridParam('sortorder');
    request.pIdSucursal = 0;
    request.pIdBanco = 0;
    request.pIdTipoMoneda = 0;
    request.pAI = -1;
    
    if ($("#divFormaCuentaBancaria").attr("IdSucursal") != null) {
        request.pIdSucursal = $("#divFormaCuentaBancaria").attr("IdSucursal");
    }
    
    if($('#divContGridCuentaBancaria').find(gs_AI).existe()){
        request.pAI = $('#divContGridCuentaBancaria').find(gs_AI).val();
    }
    
    if($('#divContGridCuentaBancaria').find(gs_AI).existe()){
        request.pAI = $('#divContGridCuentaBancaria').find(gs_AI).val();
    }
    
    if($('#divContGridCuentaBancaria').find(gs_AI).existe()){
        request.pAI = $('#divContGridCuentaBancaria').find(gs_AI).val();
    }
    
    var pRequest = JSON.stringify(request);
    $.ajax({
    url: 'Sucursal.aspx/ObtenerCuentaBancaria',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdCuentaBancaria')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroSerieFactura() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdSerieFactura').getGridParam('rowNum');
    request.pPaginaActual = $('#grdSerieFactura').getGridParam('page');
    request.pColumnaOrden = $('#grdSerieFactura').getGridParam('sortname');
    request.pTipoOrden = $('#grdSerieFactura').getGridParam('sortorder');
    request.pIdSucursal = 0;
    request.pAI = -1;

    if ($("#divFormaSerieFactura").attr("IdSucursal") != null) {
        request.pIdSucursal = $("#divFormaSerieFactura").attr("IdSucursal");
    }

    if ($('#divContGridSerieFactura').find(gs_AI).existe()) {
        request.pAI = $('#divContGridSerieFactura').find(gs_AI).val();
    }
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Sucursal.aspx/ObtenerSerieFactura',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdSerieFactura')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroSerieNotaCredito() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdSerieNotaCredito').getGridParam('rowNum');
    request.pPaginaActual = $('#grdSerieNotaCredito').getGridParam('page');
    request.pColumnaOrden = $('#grdSerieNotaCredito').getGridParam('sortname');
    request.pTipoOrden = $('#grdSerieNotaCredito').getGridParam('sortorder');
    request.pIdSucursal = 0;
    request.pAI = -1;

    if ($("#divFormaSerieNotaCredito").attr("IdSucursal") != null) {
        request.pIdSucursal = $("#divFormaSerieNotaCredito").attr("IdSucursal");
    }

    if ($('#divContGridSerieNotaCredito').find(gs_AI).existe()) {
        request.pAI = $('#divContGridSerieNotaCredito').find(gs_AI).val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Sucursal.aspx/ObtenerSerieNotaCredito',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdSerieNotaCredito')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroSeriePago() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdSeriePago').getGridParam('rowNum');
    request.pPaginaActual = $('#grdSeriePago').getGridParam('page');
    request.pColumnaOrden = $('#grdSeriePago').getGridParam('sortname');
    request.pTipoOrden = $('#grdSeriePago').getGridParam('sortorder');
    request.pIdSucursal = 0;
    request.pAI = -1;

    if ($("#divFormaSeriePago").attr("IdSucursal") != null) {
        request.pIdSucursal = $("#divFormaSeriePago").attr("IdSucursal");
    }

    if ($('#divContGridSeriePago').find(gs_AI).existe()) {
        request.pAI = $('#divContGridSeriePago').find(gs_AI).val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Sucursal.aspx/ObtenerSeriePago',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdSeriePago')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroRutaCFDI() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdRutaCFDI').getGridParam('rowNum');
    request.pPaginaActual = $('#grdRutaCFDI').getGridParam('page');
    request.pColumnaOrden = $('#grdRutaCFDI').getGridParam('sortname');
    request.pTipoOrden = $('#grdRutaCFDI').getGridParam('sortorder');
    request.pIdSucursal = 0;
    request.pAI = -1;

    if ($("#divFormaRutaCFDI").attr("IdSucursal") != null) {
        request.pIdSucursal = $("#divFormaRutaCFDI").attr("IdSucursal");
    }

    if ($('#divContGridRutaCFDI').find(gs_AI).existe()) {
        request.pAI = $('#divContGridRutaCFDI').find(gs_AI).val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Sucursal.aspx/ObtenerRutaCFDI',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdRutaCFDI')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function AgregarCuentaBancaria() {
    var pCuentaBancaria = new Object();
    pCuentaBancaria.IdSucursal = $(".divFormaAgregarCuentaBancaria").attr("IdSucursal");
    pCuentaBancaria.IdBanco = $("#cmbBanco").val();
    pCuentaBancaria.CuentaBancaria = $("#txtCuentaBancaria").val(); 
    pCuentaBancaria.IdTipoMoneda = $("#cmbTipoMoneda").val(); 
    pCuentaBancaria.Descripcion = $("#txtDescripcion").val();    
    
    var validacion = ValidaCuentaBancaria(pCuentaBancaria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentaBancaria = pCuentaBancaria;
    SetAgregarCuentaBancaria(JSON.stringify(oRequest));
}

function AgregarSerieFactura() {
    var pSerieFactura = new Object();
    pSerieFactura.IdSucursal = $(".divFormaAgregarSerieFactura").attr("IdSucursal");
    pSerieFactura.SerieFactura = $("#txtSerieFactura").val();
    if ($("#chkTimbrado").is(':checked')) {
        pSerieFactura.Timbrado = 1;
    }
    else {
        pSerieFactura.Timbrado = 0;
    }

    if ($("#chkParcial").is(':checked')) {
        pSerieFactura.EsParcialidad = 1;
    }
    else {
        pSerieFactura.EsParcialidad = 0;
    }
    
    if ($("#chkEsVenta").is(':checked')) {
        pSerieFactura.EsVenta = 1;
    }
    else {
        pSerieFactura.EsVenta = 0;
    }
    
    var validacion = ValidaSerieFactura(pSerieFactura);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSerieFactura = pSerieFactura;
    SetAgregarSerieFactura(JSON.stringify(oRequest));
}

function AgregarSerieNotaCredito() {
    var pSerieNotaCredito = new Object();
    pSerieNotaCredito.IdSucursal = $(".divFormaAgregarSerieNotaCredito").attr("IdSucursal");
    pSerieNotaCredito.SerieNotaCredito = $("#txtSerieNotaCredito").val();
    var validacion = ValidaSerieNotaCredito(pSerieNotaCredito);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSerieNotaCredito = pSerieNotaCredito;
    SetAgregarSerieNotaCredito(JSON.stringify(oRequest));
}

function AgregarSeriePago() {
    var pSeriePago = new Object();
    pSeriePago.IdSucursal = $(".divFormaAgregarSeriePago").attr("IdSucursal");
    pSeriePago.SeriePago = $("#txtSeriePago").val();
    var validacion = ValidaSeriePago(pSeriePago);
    if (validacion != "") { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSeriePago = pSeriePago;
    SetAgregarSeriePago(JSON.stringify(oRequest));
}

function AgregarRutaCFDI() {
    var pRutaCFDI = new Object();
    pRutaCFDI.IdSucursal = $(".divFormaAgregarRutaCFDI").attr("IdSucursal");
    pRutaCFDI.RutaCFDI = $("#txtRutaCFDI").val();
    pRutaCFDI.TipoRuta = $("#cmbTipoRuta").val();    
    var validacion = ValidaRutaCFDI(pRutaCFDI);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pRutaCFDI = pRutaCFDI;
    SetAgregarRutaCFDI(JSON.stringify(oRequest));
}

function SetAgregarCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarCuentaBancaria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentaBancaria").trigger("reloadGrid");
                $("#dialogAgregarCuentaBancaria").dialog("close")
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

function SetAgregarSerieFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarSerieFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSerieFactura").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogAgregarSerieFactura").dialog("close")
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

function SetAgregarSerieNotaCredito(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarSerieNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSerieNotaCredito").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogAgregarSerieNotaCredito").dialog("close")
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

function SetAgregarSeriePago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarSeriePago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSeriePago").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogAgregarSeriePago").dialog("close")
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function SetAgregarRutaCFDI(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AgregarRutaCFDI",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdRutaCFDI").trigger("reloadGrid");
                $("#grdSucursal").trigger("reloadGrid");
                $("#dialogAgregarRutaCFDI").dialog("close")
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

function AsignarConexionContpaq() {
    var pAsignarConexionContpaq = new Object();
    pAsignarConexionContpaq.IdSucursal = $(".divFormaAsignarConexionContpaq").attr("IdSucursal");
    pAsignarConexionContpaq.BaseDatos = $("#txtBaseDatos").val();
    pAsignarConexionContpaq.Usuario = $("#txtUsuarioContpaq").val();
    pAsignarConexionContpaq.Contrasena = $("#txtContrasenaContpaq").val();
    var oRequest = new Object();
    oRequest.pAsignarConexionContpaq = pAsignarConexionContpaq;
    SetAsignarConexionContpaq(JSON.stringify(oRequest));
}

function SetAsignarConexionContpaq(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Sucursal.aspx/AsignarConexionContpaq",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogAsignarConexionContpaq").dialog("close")
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

function ObtenerListaBanco() {
    $("#cmbBanco").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Sucursal.aspx/ObtenerListaBancos"
    });
}

function ObtenerFormaDivisionAsignada(pRequest) {
    $("#dialogConsultarDivisionAsignada").obtenerVista({
        nombreTemplate: "tmplDivisionAsignadaSucursal.html",
        url: "Sucursal.aspx/ObtenerFormaDivisionAsignada",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarDivisionAsignada").dialog("open");
        }
    });
}

function ObtenerFormaCuentaBancariaAsignada(pRequest) {
    $("#dialogConsultarCuentaBancariaAsignada").obtenerVista({
        nombreTemplate: "tmplCuentaBancariaAsignadaSucursal.html",
        url: "Sucursal.aspx/ObtenerFormaCuentaBancariaAsignada",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarCuentaBancariaAsignada").dialog("open");
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarSucursal() {
    $("#dialogAgregarSucursal").obtenerVista({
        nombreTemplate: "tmplAgregarSucursal.html",
        url: "Sucursal.aspx/ObtenerFormaAgregarSucursal",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarSucursal").dialog("open");
        }
    });
}

function ObtenerFormaConsultarSucursal (pSucursal){
    $("#dialogConsultarSucursal").obtenerVista({
        nombreTemplate: "tmplConsultarSucursal.html",
        url: "Sucursal.aspx/ObtenerFormaConsultarSucursal",
        parametros: pSucursal,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarSucursal == 1) {
                $("#dialogConsultarSucursal").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Sucursal = new Object();
                        Sucursal.pIdSucursal = parseInt($("#divFormaConsultarSucursal").attr("IdSucursal"));
                        ObtenerFormaEditarSucursal(JSON.stringify(Sucursal))
                    }
                });
                $("#dialogConsultarSucursal").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarSucursal").dialog("option", "buttons", {});
                $("#dialogConsultarSucursal").dialog("option", "height", "445");
            }
            $("#dialogConsultarSucursal").dialog("open");
        }
    });
}

function ObtenerFormaEditarSucursal(pSucursal) {
    $("#dialogEditarSucursal").obtenerVista({
        nombreTemplate: "tmplEditarSucursal.html",
        url: "Sucursal.aspx/ObtenerFormaEditarSucursal",
        parametros: pSucursal,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarSucursal").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCuentaBancaria(pIdSucursal) {   
    $("#divFormaConsultarCuentaBancaria").obtenerVista({
        nombreTemplate: "tmplConsultarCuentaBancaria.html",
        url: "Sucursal.aspx/ObtenerFormaConsultarCuentaBancaria",
        parametros: pIdSucursal,
        despuesDeCompilar: function(pRespuesta) {
            FiltroCuentaBancaria();
            $("#dialogConsultarCuentaBancaria").dialog("open"); 
            
            $('#grdCuentaBancaria').one('click', '.div_grdCuentaBancaria_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancaria_AI']").children().attr("baja")
                var idCuentaBancaria = $(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusCuentaBancaria(idCuentaBancaria, baja);
            });                              
        }
    });
}

function ObtenerFormaConsultarSerieFactura(pIdSucursal) {
    $("#divFormaConsultarSerieFactura").obtenerVista({
        nombreTemplate: "tmplConsultarSerieFactura.html",
        url: "Sucursal.aspx/ObtenerFormaConsultarSerieFactura",
        parametros: pIdSucursal,
        despuesDeCompilar: function(pRespuesta) {
            FiltroSerieFactura();
            $("#dialogConsultarSerieFactura").dialog("open");

            $('#grdSerieFactura').one('click', '.div_grdSerieFactura_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSerieFactura_AI']").children().attr("baja")
                var idSerieFactura = $(registro).children("td[aria-describedby='grdSerieFactura_IdSerieFactura']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusSerieFactura(idSerieFactura, baja);
            });        
            
            $("#grdSerieFactura").on("click", ".ConsultarSerieFactura", function() {
                var registro = $(this).parents("tr");
                var Sucursal = new Object();
                Sucursal.pIdSerieFactura = parseInt($(registro).children("td[aria-describedby='grdSerieFactura_IdSerieFactura']").html());
                ObtenerFormaConsultarSerieFacturaConsultar(JSON.stringify(Sucursal));
            });
        }
    });
}

function ObtenerFormaConsultarSerieNotaCredito(pIdSucursal) {
    $("#divFormaConsultarSerieNotaCredito").obtenerVista({
        nombreTemplate: "tmplConsultarSerieNotaCredito.html",
        url: "Sucursal.aspx/ObtenerFormaConsultarSerieNotaCredito",
        parametros: pIdSucursal,
        despuesDeCompilar: function(pRespuesta) {
            FiltroSerieNotaCredito();
            $("#dialogConsultarSerieNotaCredito").dialog("open");

            $('#grdSerieNotaCredito').one('click', '.div_grdSerieNotaCredito_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSerieNotaCredito_AI']").children().attr("baja")
                var idSerieNotaCredito = $(registro).children("td[aria-describedby='grdSerieNotaCredito_IdSerieNotaCredito']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusSerieNotaCredito(idSerieNotaCredito, baja);
            });

            $("#grdSerieNotaCredito").on("click", ".ConsultarSerieNotaCredito", function() {
                var registro = $(this).parents("tr");
                var Sucursal = new Object();
                Sucursal.pIdSerieNotaCredito = parseInt($(registro).children("td[aria-describedby='grdSerieNotaCredito_IdSerieNotaCredito']").html());
                ObtenerFormaConsultarSerieNotaCreditoConsultar(JSON.stringify(Sucursal));
            });

        }
    });
}

function ObtenerFormaConsultarSeriePago(pIdSucursal) {
    $("#divFormaConsultarSeriePago").obtenerVista({
        nombreTemplate: "tmplConsultarSeriePago.html",
        url: "Sucursal.aspx/ObtenerFormaConsultarSeriePago",
        parametros: pIdSucursal,
        despuesDeCompilar: function (pRespuesta) {
            FiltroSerieNotaCredito();
            $("#dialogConsultarSeriePago").dialog("open");
            $("#grdSeriePago").trigger("reloadGrid");
            $('#grdSeriePago').one('click', '.div_grdSeriePago_AI', function (event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSeriePago_AI']").children().attr("baja")
                var idSeriePago = $(registro).children("td[aria-describedby='grdSeriePago_IdSeriePago']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusSeriePago(idSeriePago, baja);
            });

            $("#grdSeriePago").on("click", ".ConsultarSeriePago", function () {
                var registro = $(this).parents("tr");
                var Sucursal = new Object();
                Sucursal.pIdSeriePago = parseInt($(registro).children("td[aria-describedby='grdSeriePago_IdSeriePago']").html());
                console.log(Sucursal);
                ObtenerFormaConsultarSeriePagoConsultar(JSON.stringify(Sucursal));
            });

        }
    });
}

function ObtenerFormaConsultarRutaCFDI(pIdSucursal) {
    $("#divFormaConsultarRutaCFDI").obtenerVista({
        nombreTemplate: "tmplConsultarRutaCFDI.html",
        url: "Sucursal.aspx/ObtenerFormaConsultarRutaCFDI",
        parametros: pIdSucursal,
        despuesDeCompilar: function(pRespuesta) {
            FiltroRutaCFDI();
            $("#dialogConsultarRutaCFDI").dialog("open");

            $('#grdRutaCFDI').one('click', '.div_grdRutaCFDI_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdRutaCFDI_AI']").children().attr("baja")
                var idRutaCFDI = $(registro).children("td[aria-describedby='grdRutaCFDI_IdRutaCFDI']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusRutaCFDI(idRutaCFDI, baja);
            });

            $("#grdRutaCFDI").on("click", ".ConsultarRutaCFDI", function() {
                var registro = $(this).parents("tr");
                var Sucursal = new Object();
                Sucursal.pIdRutaCFDI = parseInt($(registro).children("td[aria-describedby='grdRutaCFDI_IdRutaCFDI']").html());
                ObtenerFormaConsultarRutaCFDIConsultar(JSON.stringify(Sucursal));
            });

        }
    });
}

function ObtenerFormaConsultarSerieFacturaConsultar(pIdSerieFactura) {
    $("#dialogConsultarSerieFacturaConsultar").obtenerVista({
        nombreTemplate: "tmplConsultarSerieFacturaConsultar.html",
        url: "Sucursal.aspx/ObtenerFormaSerieFacturaConsultar",
        parametros: pIdSerieFactura,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarSerieFactura == 1) {
                $("#dialogConsultarSerieFacturaConsultar").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var SerieFactura = new Object();
                        SerieFactura.IdSerieFactura = parseInt($("#divFormaConsultarSerieFacturaConsultar").attr("IdSerieFactura"));
                        ObtenerFormaEditarSerieFactura(JSON.stringify(SerieFactura))

                    }
                });
                $("#dialogConsultarSerieFacturaConsultar").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarSerieFacturaConsultar").dialog("option", "buttons", {});
                $("#dialogConsultarSerieFacturaConsultar").dialog("option", "height", "300");
            }
            $("#dialogConsultarSerieFacturaConsultar").dialog("open");
        }
    });
}

function ObtenerFormaConsultarSerieNotaCreditoConsultar(pIdSerieNotaCredito) {
    $("#dialogConsultarSerieNotaCreditoConsultar").obtenerVista({
        nombreTemplate: "tmplConsultarSerieNotaCreditoConsultar.html",
        url: "Sucursal.aspx/ObtenerFormaSerieNotaCreditoConsultar",
        parametros: pIdSerieNotaCredito,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarSerieNotaCredito == 1) {
                $("#dialogConsultarSerieNotaCreditoConsultar").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var SerieNotaCredito = new Object();
                        SerieNotaCredito.IdSerieNotaCredito = parseInt($("#divFormaConsultarSerieNotaCreditoConsultar").attr("IdSerieNotaCredito"));
                        ObtenerFormaEditarSerieNotaCredito(JSON.stringify(SerieNotaCredito))

                    }
                });
                $("#dialogConsultarSerieNotaCreditoConsultar").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarSerieNotaCreditoConsultar").dialog("option", "buttons", {});
                $("#dialogConsultarSerieNotaCreditoConsultar").dialog("option", "height", "300");
            }
            $("#dialogConsultarSerieNotaCreditoConsultar").dialog("open");
        }
    });
}

function ObtenerFormaConsultarSeriePagoConsultar(pIdSeriePago) {
    $("#dialogConsultarSeriePagoConsultar").obtenerVista({
        nombreTemplate: "tmplConsultarSeriePagoConsultar.html",
        url: "Sucursal.aspx/ObtenerFormaSeriePagoConsultar",
        parametros: pIdSeriePago,
        despuesDeCompilar: function (pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarSeriePago == 1) {
                $("#dialogConsultarSeriePagoConsultar").dialog("option", "buttons", {
                    "Editar": function () {
                        $(this).dialog("close");
                        var SeriePago = new Object();
                        SeriePago.IdSeriePago = parseInt($("#divFormaConsultarSeriePagoConsultar").attr("IdSeriePago"));
                        ObtenerFormaEditarSerieNotaCredito(JSON.stringify(SeriePago))

                    }
                });
                $("#dialogConsultarSeriePagoConsultar").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarSeriePagoConsultar").dialog("option", "buttons", {});
                $("#dialogConsultarSeriePagoConsultar").dialog("option", "height", "300");
            }
            $("#dialogConsultarSeriePagoConsultar").dialog("open");
        }
    });
}

function ObtenerFormaConsultarRutaCFDIConsultar(pIdRutaCFDI) {
    $("#dialogConsultarRutaCFDIConsultar").obtenerVista({
        nombreTemplate: "tmplConsultarRutaCFDIConsultar.html",
        url: "Sucursal.aspx/ObtenerFormaRutaCFDIConsultar",
        parametros: pIdRutaCFDI,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarRutaCFDI == 1) {
                $("#dialogConsultarRutaCFDIConsultar").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var RutaCFDI = new Object();
                        RutaCFDI.IdRutaCFDI = parseInt($("#divFormaConsultarRutaCFDIConsultar").attr("IdRutaCFDI"));
                        ObtenerFormaEditarRutaCFDI(JSON.stringify(RutaCFDI))

                    }
                });
                $("#dialogConsultarRutaCFDIConsultar").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarRutaCFDIConsultar").dialog("option", "buttons", {});
                $("#dialogConsultarRutaCFDIConsultar").dialog("option", "height", "300");
            }
            $("#dialogConsultarRutaCFDIConsultar").dialog("open");
        }
    });
}

function ObtenerFormaEditarSerieFactura(IdSerieFactura) {
    $("#dialogEditarSerieFactura").obtenerVista({
        nombreTemplate: "tmplEditarSerieFactura.html",
        url: "Sucursal.aspx/ObtenerFormaEditarSerieFactura",
        parametros: IdSerieFactura,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarSerieFactura").dialog("open");
        }
    });
}

function ObtenerFormaEditarSerieNotaCredito(IdSerieNotaCredito) {
    $("#dialogEditarSerieNotaCredito").obtenerVista({
        nombreTemplate: "tmplEditarSerieNotaCredito.html",
        url: "Sucursal.aspx/ObtenerFormaEditarSerieNotaCredito",
        parametros: IdSerieNotaCredito,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarSerieNotaCredito").dialog("open");
        }
    });
}

function ObtenerFormaEditarSeriePago(IdSeriePago) {
    $("#dialogEditarSeriePago").obtenerVista({
        nombreTemplate: "tmplEditarSeriePago.html",
        url: "Sucursal.aspx/ObtenerFormaEditarSeriePago",
        parametros: IdSeriePago,
        despuesDeCompilar: function (pRespuesta) {
            $("#dialogEditarSeriePago").dialog("open");
        }
    });
}

function ObtenerFormaEditarRutaCFDI(IdRutaCFDI) {
    $("#dialogEditarRutaCFDI").obtenerVista({
        nombreTemplate: "tmplEditarRutaCFDI.html",
        url: "Sucursal.aspx/ObtenerFormaEditarRutaCFDI",
        parametros: IdRutaCFDI,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarRutaCFDI").dialog("open");
        }
    });
}

function ObtenerFormaAgregarCuentaBancaria(pRequest) {
    $("#dialogAgregarCuentaBancaria").obtenerVista({
        nombreTemplate: "tmplAgregarCuentaBancaria.html",
        url: "Sucursal.aspx/ObtenerFormaAgregarCuentaBancaria",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCuentaBancaria").dialog("open");
        }
    });
}

function ObtenerFormaAgregarSerieFactura(pRequest) {
    $("#dialogAgregarSerieFactura").obtenerVista({
        nombreTemplate: "tmplAgregarSerieFactura.html",
        url: "Sucursal.aspx/ObtenerFormaAgregarSerieFactura",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarSerieFactura").dialog("open");
        }
    });
}

function ObtenerFormaAgregarSerieNotaCredito(pRequest) {
    $("#dialogAgregarSerieNotaCredito").obtenerVista({
        nombreTemplate: "tmplAgregarSerieNotaCredito.html",
        url: "Sucursal.aspx/ObtenerFormaAgregarSerieNotaCredito",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarSerieNotaCredito").dialog("open");
        }
    });
}

function ObtenerFormaAgregarSeriePago(pRequest) {
    $("#dialogAgregarSeriePago").obtenerVista({
        nombreTemplate: "tmplAgregarSeriePago.html",
        url: "Sucursal.aspx/ObtenerFormaAgregarSeriePago",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
            $("#dialogAgregarSeriePago").dialog("open");
        }
    });
}

function ObtenerFormaAgregarRutaCFDI(pRequest) {
    $("#dialogAgregarRutaCFDI").obtenerVista({
        nombreTemplate: "tmplAgregarRutaCFDI.html",
        url: "Sucursal.aspx/ObtenerFormaAgregarRutaCFDI",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarRutaCFDI").dialog("open");
        }
    });
}

function ObtenerListaEstados(pRequest){
    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Sucursal.aspx/ObtenerListaEstados",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaMunicipios(pRequest){
    $("#cmbMunicipio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Sucursal.aspx/ObtenerListaMunicipios",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaLocalidades(pRequest){
    $("#cmbLocalidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Sucursal.aspx/ObtenerListaLocalidades",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerFormaAsignarConexionContpaq(pRequest) {
    $("#dialogAsignarConexionContpaq").obtenerVista({
        nombreTemplate: "tmplAsignarConexionContpaq.html",
        url: "Sucursal.aspx/ObtenerFormaAsignarConexionContpaq",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAsignarConexionContpaq").dialog("open");
        }
    });
}

//----------Validaciones----------//
//--------------------------//
function ValidaSucursal(pSucursal) {
    var errores = "";

    if (pSucursal.Sucursal == "")
    { errores = errores + "<span>*</span> El nombre de la sucursal esta vacía, favor de capturarla.<br />"; }

    if (pSucursal.IdEmpresa == "0")
    { errores = errores + "<span>*</span> No se indicó la empresa, favor de seleccionarla.<br />"; }

    if (pSucursal.IdTipoMoneda == "0")
    { errores = errores + "<span>*</span> No se indicó el tipo de moneda, favor de seleccionarlo.<br />"; }

    if (pSucursal.IdIVA == "0")
    { errores = errores + "<span>*</span> No se indicó el IVA, favor de seleccionarlo.<br />"; }
    
    if(pSucursal.DireccionFiscal == 0)
    {
        if (pSucursal.Calle == "")
        { errores = errores + "<span>*</span> La calle está vacía, favor de capturarla.<br />"; }
        
        if (pSucursal.NumeroExterior == "")
        { errores = errores + "<span>*</span> El número externo está vacío, favor de capturarlo.<br />"; }
        
        if (pSucursal.Colonia == "")
        { errores = errores + "<span>*</span> La colonia está vacía, favor de capturarla.<br />"; }
        
        if (pSucursal.CodigoPostal == "")
        { errores = errores + "<span>*</span> El código postal está vacía, favor de capturarla.<br />"; }
    }
    
    if (pSucursal.Telefono == "")
    { errores = errores + "<span>*</span> El teléfono está vacío, favor de capturarlo.<br />"; }
    
    if (pSucursal.Correo != "")
    {
        if (ValidarCorreo(pSucursal.Correo))
        { errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
    }

    if (pSucursal.IVA == "")
    { errores = errores + "<span>*</span> El IVA está vacío, favor de capturarlo.<br />"; }
    
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaCuentaBancaria(pCuentaBancaria) {
    var errores = "";

    if (pCuentaBancaria.CuentaBancaria == "")
    { errores = errores + "<span>*</span> La cuenta bancaria esta vacía, favor de capturarla.<br />"; }
    
    if (pCuentaBancaria.IdBanco == 0)
    { errores = errores + "<span>*</span> El banco esta vacio, favor de capturarlo.<br />"; }
    
    if (pCuentaBancaria.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> La moneda esta vacia, favor de capturarla.<br />"; }
    
    if (pCuentaBancaria.Descripcion == "")
    { errores = errores + "<span>*</span> La descripción esta vacia, favor de capturarla.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaSerieFactura(pSerieFactura) {
    var errores = "";

    if (pSerieFactura.SerieFactura == "")
    { errores = errores + "<span>*</span> La serie para la factura esta vacía, favor de capturarla.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaSerieNotaCredito(pSerieNotaCredito) {
    var errores = "";

    if (pSerieNotaCredito.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> La serie para las notas de credito esta vacía, favor de capturarla.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaSeriePago(pSeriePago) {
    var errores = "";

    if (pSeriePago.SeriePago == "") { errores = errores + "<span>*</span> La serie para Complemento de Pago esta vacía, favor de capturarla.<br />"; }

    if (errores != "") { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaRutaCFDI(pRutaCFDI) {
    var errores = "";

    if (pRutaCFDI.RutaCFDI == "")
    { errores = errores + "<span>*</span> La ruta esta vacía, favor de capturarla.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}