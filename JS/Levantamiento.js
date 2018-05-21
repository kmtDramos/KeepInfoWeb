//----------DHTMLX----------//
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function () {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    ObtenerTotalesEstatusLevantamiento();
    ObtenerFormaFiltrosLevantamiento();

    //////funcion del grid//////
    $("#gbox_grdLevantamiento").livequery(function () {
        $("#grdCotizacion").jqGrid('navButtonAdd', '#pagCotizacion', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function () {

                var pRazonSocial = "";
                var pFolio = "";
                var pIdEstatusCotizacion = -1;
                var pAI = 0;

                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;

                var idestatuscotizacion = $("#tblCotizacionTotalesEstatus").attr("idEstatusCotizacionSeleccionado");
                pIdEstatusCotizacion = validaNumero(idestatuscotizacion) ? idestatuscotizacion : -1;

                if ($('#gs_Folio').val() != null) { pFolio = $("#gs_Folio").val(); }

                if ($('#gs_RazonSocial').val() != null) { pRazonSocial = $("#gs_RazonSocial").val(); }

                if ($('#gs_AI').val() != null) { pAI = $("#gs_AI").val(); }

                if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

                    if ($("#chkPorFecha").is(':checked')) {
                        pPorFecha = 1;
                    }
                    else {
                        pPorFecha = 0;
                    }

                    pFechaInicial = $("#txtFechaInicial").val();
                    pFechaInicial = ConvertirFecha(pFechaInicial, 'aaaammdd');
                }
                if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                    pFechaFinal = $("#txtFechaFinal").val();
                    pFechaFinal = ConvertirFecha(pFechaFinal, 'aaaammdd');
                }

                $.UnifiedExportFile({
                    action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                        IsExportExcel: true,
                        pRazonSocial: pRazonSocial,
                        pFolio: pFolio,
                        pIdEstatusCotizacion: pIdEstatusCotizacion,
                        pAI: pAI,
                        pFechaInicial: pFechaInicial,
                        pFechaFinal: pFechaFinal,
                        pPorFecha: pPorFecha

                    }, downloadType: 'Normal'
                });

            }
        });
    });

    $('#grdLevantamiento').one('click', '.div_grdLevantamiento_AI', function (event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdLevantamiento_AI']").children().attr("baja")
        var idLevantamiento = $(registro).children("td[aria-describedby='grdLevantamiento_IdLevantamiento']").html();
        var baja = "false";
        idEstatusLevantamiento = 1;
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
            idEstatusLevantamiento = 4;
        }

        alert("cambiar");
        SetCambiarEstatus(idLevantamiento, baja, idEstatusLevantamiento);
    });

    $("#grdLevantamiento").on("click", ".imgFormaConsultarLevantamiento", function () {
        var registro = $(this).parents("tr");
        var Levantamiento = new Object();
        Levantamiento.pIdLevantamiento = parseInt($(registro).children("td[aria-describedby='grdLevantamiento_IdLevantamiento']").html());
        ObtenerFormaConsultarLevantamiento(JSON.stringify(Levantamiento));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarLevantamiento", function () {
        ObtenerFormaAgregarLevantamiento();
    });

    $('#dialogAgregarLevantamiento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function () {

           
        },
        close: function () {
            $("#divFormaAgregarLevantamiento").remove();
        },
        buttons: {
            "Guardar": function () {
                AgregarLevantamiento();
            },
            "Salir": function () {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarLevantamiento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            $("#divFormaConsultarLevantamiento").remove();
        }
    });

    $('#dialogEditarLevantamiento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function () {
            
        },
        close: function () {
            //$("#dialogEditarLevantamiento").remove();
        }
    });

    $(".spanFiltroTotal").click(function () {
        var idEstatusLevantamiento = $(this).attr("idEstatusLevantamiento");
        $("#tblLevantamientoTotalesEstatus").attr("idEstatusLevantamientoSeleccionado", idEstatusLevantamiento);
        FiltroLevantamiento();
    });
    
});


//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarLevantamiento() {
    $("#dialogAgregarLevantamiento").obtenerVista({
        nombreTemplate: "tmplAgregarLevantamiento.html",
        url: "Levantamiento.aspx/ObtenerFormaAgregarLevantamiento",
        despuesDeCompilar: function (pRespuesta) {
            Modelo = pRespuesta.modelo;

            $("#txtValidoHasta").datepicker();
            autocompletarCliente();
            $("#tabChecklist").tabs();

            $("#dialogAgregarLevantamiento").dialog("open");
 
            
            $("#cmbOportunidad").change(function (e) {
                var oRequest = new Object();
                oRequest.IdOportunidad = $(this).val();
                console.log(oRequest);
                ObtenerDivisionOportunidad(JSON.stringify(oRequest));
            });

        }
    });

}

function ObtenerDivisionOportunidad(pRequest) {
    console.log(pRequest);
    $("#cmbDivision").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        parametros: pRequest,
        url: "Levantamiento.aspx/ObtenerDivisionOportunidad",
        despuesDeCompilar: function (pRespuesta) {
            console.log(pRespuesta);
        }
    });
}
//-----------AJAX-----------//
//-Funciones de acciones-//
function autocompletarCliente() {
    $('#txtRazonSocial').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $("#txtRazonSocial").val();
            $.ajax({
                type: 'POST',
                url: 'Levantamiento.aspx/BuscarRazonSocial',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idCliente", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idCliente", pIdCliente);

            var request = new Object();
            request.pIdCliente = pIdCliente;
            request.pIdOportunidad = 0;
            ObtenerListaOportunidades(JSON.stringify(request));
           
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function ObtenerListaOportunidades(pRequest) {
    $("#cmbOportunidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        parametros: pRequest,
        url: "Levantamiento.aspx/ObtenerListaOportunidad",
        despuesDeCompilar: function (pRespuesta) {

            
            console.log(pRespuesta);
        }
    });
}

function ObtenerFormaFiltrosLevantamiento() {
    $("#divFiltrosLevantamiento").obtenerVista({
        nombreTemplate: "tmplFiltrosLevantamiento.html",
        url: "Levantamiento.aspx/ObtenerFormaFiltroLevantamiento",
        despuesDeCompilar: function (pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function () {
                        FiltroLevantamiento();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function () {
                        FiltroLevantamiento();
                    }
                });
            }

            $("#cmbEstatusLevantamiento").change(function () {
                FiltroLevantamiento();
            });

            //            $('#divFiltrosCotizacion').on('click', '#chkPorFecha', function(event) {
            //                FiltroCotizacion();
            //            });

        }
    });
}

function ObtenerTotalesEstatusLevantamiento() {

    var request = new Object();
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    request.pFolio = 0;
    request.pRazonSocial = "";
    request.pAI = 0;

    var idestatuslevantamiento = $("#tblLevantamientoTotalesEstatus").attr("idEstatusLevantamientoSeleccionado");
    request.pIdEstatusLevantamiento = validaNumero(idestatuslevantamiento) ? idestatuslevantamiento : -1;

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

        if ($("#chkPorFecha").is(':checked')) {
            request.pPorFecha = 1;
        }
        else {
            request.pPorFecha = 0;
        }

        request.pFechaInicial = $("#txtFechaInicial").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        request.pFechaFinal = $("#txtFechaFinal").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }

    if ($('#gs_Folio').val() != null && $('#gs_Folio').val() != "") {
        request.pFolio = $("#gs_Folio").val();
    }

    if ($('#gs_RazonSocial').val() != null && $('#gs_RazonSocial').val() != "") {
        request.pRazonSocial = $("#gs_RazonSocial").val();
    }

    if ($('#gs_AI').val() != null && $('#gs_AI').val() != "") {
        request.pAI = $("#gs_AI").val();
    }

    var pRequest = JSON.stringify(request);

    $.ajax({
        url: 'Levantamiento.aspx/ObtenerTotalesEstatusLevantamiento',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $.each(respuesta.Modelo.TotalesEstatusLevantamiento, function (index, oEstatusLevantamiento) {
                    $('#span-E' + oEstatusLevantamiento.IdEstatusLevantamiento).text(oEstatusLevantamiento.Total);
                });
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        }
    });
}

function SetCambiarEstatus(pIdLevantamiento, pBaja, pIdEstatusLevantamiento) {
    var pRequest = "{'pIdLevantamiento':" + pIdLevantamiento + ", 'pBaja':" + pBaja + ", 'pIdEstatusLevantamiento':" + pIdEstatusLevantamiento + "}";
    $.ajax({
        type: "POST",
        url: "Levantamiento.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            Modelo = respuesta.Modelo;
            if (respuesta.Error == 0) {
                $("#grdLevantamiento").trigger("reloadGrid");
                ObtenerTotalesEstatusLevantamiento();
            } else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            $('#grdLevantamiento').one('click', '.div_grdLevantamiento_AI', function (event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdLevantamiento_AI']").children().attr("baja")
                var idLevantamiento = $(registro).children("td[aria-describedby='grdLevantamiento_IdLevantamiento']").html();
                var idEstatusLevantamiento = 1;

                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                    idEstatusLevantamiento = 4;
                }

                SetCambiarEstatus(idLevantamiento, baja, idEstatusLevantamiento);
            });
        }
    });
}

function FiltroLevantamiento() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdLevantamiento').getGridParam('rowNum');
    request.pPaginaActual = $('#grdLevantamiento').getGridParam('page');
    request.pColumnaOrden = $('#grdLevantamiento').getGridParam('sortname');
    request.pTipoOrden = $('#grdLevantamiento').getGridParam('sortorder');
    request.pAI = 0;
    request.pRazonSocial = "";
    request.pFolio = "";
    request.pIdOportunidad = "";
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    request.pIdEstatusLevantamiento = -1;

    var idestatuslevantamiento = $("#tblLevantamientoTotalesEstatus").attr("idEstatusLevantamientoSeleccionado");
    request.pIdEstatusLevantamiento = validaNumero(idestatuslevantamiento) ? idestatuslevantamiento : -1;

    if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }

    if ($('#gs_IdOportunidad').val() != null) { request.pIdOportunidad = $("#gs_IdOportunidad").val(); }

    if ($('#gs_AI').val() != null) { request.pAI = $("#gs_AI").val(); }

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

        if ($("#chkPorFecha").is(':checked')) {
            request.pPorFecha = 1;
        }
        else {
            request.pPorFecha = 0;
        }

        request.pFechaInicial = $("#txtFechaInicial").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }

    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        request.pFechaFinal = $("#txtFechaFinal").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }

    if (request.pIdEstatusLevantamiento != "") {
        console.log(request);
        $("#cmbEstatusLevantamiento").val(request.pIdEstatusLevantamiento);

        if (request.pIdEstatusLevantamiento == 3) {
            $('#gs_AI').val(1);
            request.pAI = $("#gs_AI").val();
        }
    }

    if ($("#cmbEstatusLevantamiento").val() != null) {
        request.pIdEstatusLevantamiento = parseInt($("#cmbEstatusLevantamiento").val());
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Levantamiento.aspx/ObtenerLevantamiento',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') {
                ObtenerTotalesEstatusLevantamiento();
                $('#grdLevantamiento')[0].addJSONData(JSON.parse(jsondata.responseText).d);
                var btnDeclinar = '<img src="../images/false.png" height="20" style="cursor:pointer;" onclick="DeclinarCotizacion(this);" />';
                $('td[aria-describedby="grdCotizacion_Declinar"]', '#grdCotizacion').html(btnDeclinar);
                $('td[aria-describedby="grdCotizacion_Utilidad"]', '#grdCotizacion').each(function (index, elemento) {
                    var utilidad = parseFloat($(elemento).text().replace("$", "").replace(",", ""));
                    if (utilidad < 0)
                        $(elemento).css({ "color": "#F00" });
                });
            }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}


function AgregarLevantamiento() {
    var pLevantamiento = new Object();

    var pidLevantamiento = $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idLevantamiento");
    pLevantamiento.IdLevantamiento = validaNumero(pidLevantamiento) ? pidLevantamiento : 0;

    var idcliente = $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idCliente");
    pLevantamiento.IdCliente = validaNumero(idcliente) ? idcliente : 0;


    pLevantamiento.Nota = $("#txtLevantamiento").val();
    pLevantamiento.ValidoHasta = $("#txtValidoHasta").val();
    pLevantamiento.IdDivision = $("#cmbDivision").val();
    pLevantamiento.IdOportunidad = $("#cmbOportunidad").val();
    pLevantamiento.IdEstatusLevantamiento = 1;

    var validacion = ValidaLevantamiento(pLevantamiento);
    if (validacion != "") { MostrarMensajeError(validacion); return false; }
    
    SetAgregarLevantamiento(JSON.stringify(pLevantamiento));
}

function SetAgregarLevantamiento(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Levantamiento.aspx/AgregarLevantamiento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdLevantamiento").trigger("reloadGrid");
                ObtenerTotalesEstatusLevantamiento();
                MostrarMensajeError("Información guardada");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogAgregarLevantamiento").dialog("close");
        }
    });
}

function ObtenerFormaConsultarLevantamiento(pIdLevantamiento) {
    $("#dialogConsultarLevantamiento").obtenerVista({
        nombreTemplate: "tmplConsultarLevantamiento.html",
        url: "Levantamiento.aspx/ObtenerFormaLevantamiento",
        parametros: pIdLevantamiento,
        despuesDeCompilar: function (pRespuesta) {
            Modelo = pRespuesta.modelo;

            if (Modelo.IdEstatusLevantamiento == 1) {
                $("#dialogConsultarLevantamiento").dialog("option", "buttons", {
                    "Editar": function () {
                        var Levantamiento = new Object();
                        Levantamiento.IdLevantamiento = parseInt($("#divFormaConsultarLevantamiento").attr("idLevantamiento"));
                        ObtenerFormaEditarLevantamiento(JSON.stringify(Levantamiento))
                        $(this).dialog("close");
                    }
                });
            }
            else {
                $("#dialogConsultarLevantamiento").dialog("option", "buttons", {
                    "Salir": function () {
                        $(this).dialog("close");
                    }
                });
            }
            $("#dialogConsultarLevantamiento").dialog("open");

        }
    });
}

function ObtenerFormaEditarLevantamiento(pRequest) {
    $("#dialogEditarLevantamiento").obtenerVista({
        nombreTemplate: "tmplEditarLevantamiento.html",
        url: "Levantamiento.aspx/ObtenerFormaEditarLevantamiento",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
            Modelo = pRespuesta.modelo;

            autocompletarCliente();

            var oRequest = new Object();
            oRequest.IdOportunidad = Modelo.IdOportunidad;
            ObtenerDivisionOportunidad(JSON.stringify(oRequest));

            var request = new Object();
            request.pIdCliente = Modelo.IdCliente;
            request.pIdOportunidad = Modelo.IdOportunidad;
            $("#txtValidoHasta").datepicker();
            $("#tabChecklist").tabs();
            //ObtenerListaOportunidades(JSON.stringify(request));

            //$("#cmbOportunidad").val("" + Modelo.IdOportunidad + "");

            $("#cmbOportunidad").change(function (e) {
                var oRequest = new Object();
                oRequest.IdOportunidad = $(this).val();
                ObtenerDivisionOportunidad(JSON.stringify(oRequest));
            });
            
            console.log(Modelo);
            if (Modelo.IdEstatusLevantamiento == 1) {
                $("#dialogEditarLevantamiento").dialog("option", "buttons", {
                    "Guardar": function () {
                        EditarLevantamiento();
                    },
                    "Salir": function () {
                        $(this).dialog("close")
                    }
                });
                $("#dialogConsultarLevantamiento").dialog("option", "height", "auto");
            }
            else {// ya no se edita
                $("#dialogEditarLevantamiento").dialog("option", "buttons", {
                    "Salir": function () {
                        $(this).dialog("close")
                    }
                });
                $("#dialogConsultarLevantamiento").dialog("option", "height", "auto");

            }
            $("#dialogEditarLevantamiento").dialog("open");
        }
    });
}

function EditarLevantamiento() {
    var pLevantamiento = new Object();

    var pidLevantamiento = $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idLevantamiento");
    pLevantamiento.IdLevantamiento = validaNumero(pidLevantamiento) ? pidLevantamiento : 0;

    var idcliente = $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idCliente");
    pLevantamiento.IdCliente = validaNumero(idcliente) ? idcliente : 0;

    pLevantamiento.Nota = $("#txtLevantamiento").val();
    pLevantamiento.ValidoHasta = $("#txtValidoHasta").val();
    pLevantamiento.IdDivision = $("#cmbDivision").val();
    pLevantamiento.IdOportunidad = $("#cmbOportunidad").val();

    var validacion = ValidaLevantamiento(pLevantamiento);
    if (validacion != "") { MostrarMensajeError(validacion); return false; }

    SetEditarLevantamiento(JSON.stringify(pLevantamiento));
}

function SetEditarLevantamiento(pRequest) {
    console.log(pRequest);
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Levantamiento.aspx/EditarLevantamiento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            console.log(respuesta);
            if (respuesta.Error == 0) {
                $("#grdLevantamiento").trigger("reloadGrid");
                ObtenerTotalesEstatusLevantamiento();
                MostrarMensajeError("Datos editados");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogEditarLevantamiento").dialog("close");
        }
    });
}


//----------Validaciones----------//
//--------------------------//
function ValidaLevantamiento(pLevantamiento) {
    var errores = "";

    if (pLevantamiento.IdCliente == 0) { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pLevantamiento.IdDivision == 0) { errores = errores + "<span>*</span> No hay división asociada, favor de seleccionarla.<br />"; }

    if (pLevantamiento.Nota == 0) { errores = errores + "<span>*</span> No hay Descripción / Referencias, favor de escribir.<br />"; }

    if (pLevantamiento.IdOportunidad == 0) { errores = errores + "<span>*</span> No hay Oportunidad asociada, favor de seleccionarla.<br />"; }

    if (pLevantamiento.ValidoHasta == 0) { errores = errores + "<span>*</span> No hay Fecha de Vigencia, favor de seleccionarla.<br />"; }

    if (errores != "") { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
