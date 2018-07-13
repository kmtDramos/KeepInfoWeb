//----------DHTMLX----------//
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function () {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    
    ObtenerFormaFiltrosSolicitudLevantamiento();

    $("#grdSolicitudLevantamiento").on("click", ".imgFormaConsultarSolicitudLevantamiento", function () {
        var registro = $(this).parents("tr");
        var SolLevantamiento = new Object();
        SolLevantamiento.pIdSolicitudLevantamiento = parseInt($(registro).children("td[aria-describedby='grdSolicitudLevantamiento_IdSolicitudLevantamiento']").html());
        ObtenerFormaConsultarSolicitudLevantamiento(JSON.stringify(SolLevantamiento));
    });


    $('#dialogConsultarSolicitudLevantamiento').dialog({
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
            $("#divFormaConsultarSolicitudLevantamiento").remove();
        }
    });

    $(".spanFiltroTotal").click(function () {
        FiltroSolicitudLevantamiento();
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

});



function ObtenerFormaFiltrosSolicitudLevantamiento() {
    $("#divFiltrosSolicitudLevantamiento").obtenerVista({
        nombreTemplate: "tmplFiltrosSolicitudLevantamiento.html",
        url: "SolicitudLevantamiento.aspx/ObtenerFormaFiltroSolicitudLevantamiento",
        despuesDeCompilar: function (pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function () {
                        FiltroSolicitudLevantamiento();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function () {
                        FiltroSolicitudLevantamiento();
                    }
                });
            }

            $('#divFiltrosSolicitudLevantamiento').on('click', '#chkPorFecha', function (event) {
                FiltroSolicitudLevantamiento();
            });

            $('#divFiltrosSolicitudLevantamiento').on('change', '#cmbEstatusSolicitudLevantamiento', function (event) {
                FiltroSolicitudLevantamiento();
            });
        }
    });
}

function FiltroSolicitudLevantamiento() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdSolicitudLevantamiento').getGridParam('rowNum');
    request.pPaginaActual = $('#grdSolicitudLevantamiento').getGridParam('page');
    request.pColumnaOrden = $('#grdSolicitudLevantamiento').getGridParam('sortname');
    request.pTipoOrden = $('#grdSolicitudLevantamiento').getGridParam('sortorder');
    request.pAI = 0;
    request.pRazonSocial = "";
    request.pFolio = "";
    request.pIdOportunidad = "";
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pConfirmado = -1;
    request.pPorFecha = 0;

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

    request.pConfirmado = ($("#cmbEstatusSolicitudLevantamiento").val() != null) ? parseInt($("#cmbEstatusSolicitudLevantamiento").val()) : -1; console.log(request)

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'SolicitudLevantamiento.aspx/ObtenerSolicitudLevantamiento',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') {
                $('#grdSolicitudLevantamiento')[0].addJSONData(JSON.parse(jsondata.responseText).d);
               
            }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaConsultarSolicitudLevantamiento(pIdSolicitudLevantamiento) {
    $("#dialogConsultarSolicitudLevantamiento").obtenerVista({
        nombreTemplate: "tmplConsultarSolicitudLevantamiento.html",
        url: "SolicitudLevantamiento.aspx/ObtenerFormaSoliciudLevantamiento",
        parametros: pIdSolicitudLevantamiento,
        despuesDeCompilar: function (pRespuesta) {
            Modelo = pRespuesta.modelo;
            $("#tabChecklist").tabs();

            $("#dialogConsultarSolicitudLevantamiento").dialog("option", "buttons", {
                "Crear Levantamiento": function () {
                    if ($("#chkConfirmarSolicitud").is(':checked')) {
                        var idsolicitudLev = $("#idSolicitud").text();
                        ObtenerFormaAgregarLevantamiento(idsolicitudLev);
                    } else {
                        MostrarMensajeError("La solicitud no se encuentra confirmada.");
                    }
                },
                "Salir": function () {
                    $(this).dialog("close");
                }
            });

            $("#dialogConsultarSolicitudLevantamiento").dialog("open");

        }
    });
}

function ObtenerFormaAgregarLevantamiento(idSolicitudLev) {
    var SolLevantamiento = new Object();
    SolLevantamiento.pIdSolicitudLevantamiento = idSolicitudLev;

    $("#dialogAgregarLevantamiento").obtenerVista({
        nombreTemplate: "tmplAgregarLevantamiento.html",
        url: "SolicitudLevantamiento.aspx/ObtenerFormaAgregarLevantamiento",
        parametros: JSON.stringify(SolLevantamiento),
        despuesDeCompilar: function (pRespuesta) {
            console.log(pRespuesta);
            Modelo = pRespuesta.modelo;

            $("#txtSolLevantamiento").val(idSolicitudLev);
            $("#divFormaAgregarLevantamiento").attr("IdCliente", Modelo.IdCliente);
            $("#divFormaAgregarLevantamiento").attr("idsollevantamiento", idSolicitudLev);
            $("#txtRazonSocial").val(Modelo.RazonSocial);
            
            var Divisiones = Modelo.Divisiones;
            var select = document.getElementById('cmbDivision');
            select.innerHTML = "";
            for (x in Divisiones) {
                //console.log(Divisiones[x]);
                var optD = document.createElement('option');
                optD.value = Divisiones[x].Valor;
                optD.innerHTML = Divisiones[x].Descripcion;

                if (Divisiones[x].Valor == Modelo.IdDivision) {
                    optD.setAttribute("selected", "selected");

                }

                select.appendChild(optD);
            }

            var Oportunidades = Modelo.Oportunidades;
            var select = document.getElementById('cmbOportunidad');
            select.innerHTML = "";
            for (x in Oportunidades) {
                //console.log(Divisiones[x]);
                var opt = document.createElement('option');
                opt.value = Oportunidades[x].Valor;
                opt.innerHTML = Oportunidades[x].Valor + " - " + Oportunidades[x].Descripcion;

                if (Oportunidades[x].Selected == 1) {
                    opt.setAttribute("selected", "selected");

                }

                select.appendChild(opt);
            }

            $("#txtValidoHasta").datepicker();
            autocompletarCliente();
            autocompletarSolicitud();
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

function AgregarLevantamiento() {
    var pLevantamiento = new Object();

    var pidLevantamiento = $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idLevantamiento");
    pLevantamiento.IdLevantamiento = validaNumero(pidLevantamiento) ? pidLevantamiento : 0;

    var idcliente = $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idCliente");
    pLevantamiento.IdCliente = parseInt(validaNumero(idcliente) ? idcliente : 0);

    pLevantamiento.IdSolLevantamiento = parseInt($("#txtSolLevantamiento").val());

    pLevantamiento.Nota = $("#txtLevantamiento").val();
    pLevantamiento.ValidoHasta = $("#txtValidoHasta").val();
    pLevantamiento.IdDivision = parseInt($("#cmbDivision").val());
    pLevantamiento.IdOportunidad = parseInt($("#cmbOportunidad").val());
    pLevantamiento.IdEstatusLevantamiento = 1;

    pLevantamiento.Checks = obtenerChecks();

    console.log(pLevantamiento);
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
                MostrarMensajeError("Información guardada");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogAgregarLevantamiento").dialog("close");
        }
    });
}

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

function autocompletarSolicitud() {
    $('#txtSolLevantamiento').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pIdSolicitud = $("#txtSolLevantamiento").val();
            $.ajax({
                type: 'POST',
                url: 'Levantamiento.aspx/BuscarSolLevantamiento',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idSolLevantamiento", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        console.log(item);
                        return { label: item.IdSolicitudLevantamiento, value: item.IdSolicitudLevantamiento, id: item.IdSolicitudLevantamiento } /////
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            console.log(ui);
            var pIdSolLevantamiento = ui.item.id;
            $("#divFormaAgregarLevantamiento, #divFormaEditarLevantamiento").attr("idSolLevantamiento", pIdSolLevantamiento);
            var req = new Object();
            req.pIdSolLevantamiento = pIdSolLevantamiento;
            autocompletarDatos(req);
        },
        change: function (event, ui) { console.log("cambio"); },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function autocompletarDatos(pIdSolLevantamiento) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SolicitudLevantamiento.aspx/ObtenerDatos",
        data: JSON.stringify(pIdSolLevantamiento),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            console.log(respuesta);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarLevantamiento").attr("IdCliente", respuesta.Modelo.IdCliente);
                $("#divFormaAgregarLevantamiento").attr("idSolLevantamiento", respuesta.Modelo.idSolLevantamiento);
                $("#txtRazonSocial").val(respuesta.Modelo.RazonSocial);

                var Modelo = respuesta.Modelo;
                var Divisiones = Modelo.Divisiones;
                var select = document.getElementById('cmbDivision');
                select.innerHTML = "";
                for (x in Divisiones) {
                    //console.log(Divisiones[x]);
                    var optD = document.createElement('option');
                    optD.value = Divisiones[x].Valor;
                    optD.innerHTML = Divisiones[x].Descripcion;

                    if (Divisiones[x].Valor == Modelo.IdDivision) {
                        optD.setAttribute("selected", "selected");
                        
                    }

                    select.appendChild(optD);
                }

                var Oportunidades = Modelo.Oportunidades;
                var select = document.getElementById('cmbOportunidad');
                select.innerHTML = "";
                for (x in Oportunidades) {
                    //console.log(Divisiones[x]);
                    var opt = document.createElement('option');
                    opt.value = Oportunidades[x].Valor;
                    opt.innerHTML = Oportunidades[x].Valor + " - " +Oportunidades[x].Descripcion;

                    if (Oportunidades[x].Selected == 1) {
                        opt.setAttribute("selected", "selected");

                    }
                       
                    select.appendChild(opt);
                }

               // MostrarMensajeError("Información guardada");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            //$("#dialogAgregarLevantamiento").dialog("close");
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

function ObtenerListaOportunidades(pRequest) {
    $("#cmbOportunidad").obtenerVista({
        nombreTemplate: "tmplComboOportunidad.html",
        parametros: pRequest,
        url: "Levantamiento.aspx/ObtenerListaOportunidad",
        despuesDeCompilar: function (pRespuesta) {

            
            console.log(pRespuesta);
        }
    });
}

function obtenerChecks() {
    var pChecks = new Object();

    for (i = 1; i <= 170; i++) {
        pChecks["sino" + i] = ($("#chk" + i).prop("checked")) ? 1 : 0;
        pChecks["cantidad" + i] = parseInt($("#txtCantidad" + i).val());
        pChecks["Observacion" + i] = $("#txtObservacion" + i).val();
    }

    console.log(pChecks);

    return pChecks;
}

function ValidaLevantamiento(pLevantamiento) {
    var errores = "";

    if (pLevantamiento.IdSolLevantamiento == 0) { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pLevantamiento.IdCliente == 0) { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pLevantamiento.IdDivision == 0) { errores = errores + "<span>*</span> No hay división asociada, favor de seleccionarla.<br />"; }

    if (pLevantamiento.Nota == 0) { errores = errores + "<span>*</span> No hay Descripción / Referencias, favor de escribir.<br />"; }

    if (pLevantamiento.IdOportunidad == 0) { errores = errores + "<span>*</span> No hay Oportunidad asociada, favor de seleccionarla.<br />"; }

    if (pLevantamiento.ValidoHasta == 0) { errores = errores + "<span>*</span> No hay Fecha de Vigencia, favor de seleccionarla.<br />"; }

    if (errores != "") { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}


function Imprimir(IdSolLevantamiento) {
    MostrarBloqueo();

    var SolicitudLevantamiento = new Object();
    SolicitudLevantamiento.IdSolLevantamiento = IdSolLevantamiento;

    var Request = JSON.stringify(SolicitudLevantamiento);

    var formato = $("<div></div>");

    $(formato).obtenerVista({
        url: "SolicitudLevantamiento.aspx/ImprimirSolLevantamiento",
        parametros: Request,
        nombreTemplate: "tmplImprimirSolLevantamiento.html",
        despuesDeCompilar: function (Respuesta) {
            var impresion = window.open("", "_blank");
            impresion.document.write($(formato).html());
            impresion.print();
            impresion.close();
        }
    });

}

function ImprimirSolLevantamiento() {
    console.log("imprimir levantamiento");
    var IdSolLevantamiento = $("#idSolicitud").text();
    console.log(IdSolLevantamiento);
    Imprimir(IdSolLevantamiento);
};
