//----------DHTMLX----------//
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function () {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    
    $("#grdSolicitudLevantamiento").on("click", ".imgFormaConsultarSolicitudLevantamiento", function () {
        var registro = $(this).parents("tr");
        var SolLevantamiento = new Object();
        SolLevantamiento.pIdSolicitudLevantamiento = parseInt($(registro).children("td[aria-describedby='grdSolicitudLevantamiento_IdSolicitudLevantamiento']").html());
        //ObtenerFormaConsultarSolicitudLevantamiento(JSON.stringify(SolLevantamiento));
    });

    $("#btnActualizarReporteLevantamiento").click(FiltroReporteLevantamiento);

    $("#txtFechaInicial").datepicker();
    $("#txtFechaFinal").datepicker();

    LlenaSucursales();
    LlenaDivisiones();
    LlenaEstatus();

    $("#txtAgente").autocomplete({
        source: function (request, response) {
            var Usuario = new Object();
            Usuario.Usuario = request.term;
            var Request = JSON.stringify(Usuario);
            $.ajax({
                url: "ReporteLevantamiento.aspx/ObtenerUsuario",
                type: "post",
                data: Request,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (Respuesta) {
                    $("#divFiltrosReporteLevantamiento").attr("idAgente", "");
                    var json = JSON.parse(Respuesta.d);
                    var Usuarios = json.Modelo.Usuarios;
                    response($.map(Usuarios, function (item) {
                        return { label: item.Agente, value: item.Agente }
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            var pIdAgente = ui.item.id;
            $("#divFiltrosReporteLevantamiento").attr("idAgente", pIdAgente);
        }
    });

    $('#txtCliente').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $("#txtCliente").val();
            pRequest.pIdSucursal = $("#cmbSucursal").val();
            $.ajax({
                type: 'POST',
                url: 'ReporteLevantamiento.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFiltrosReporteLevantamiento").attr("idCliente", "");
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
            $("#divFiltrosReporteLevantamiento").attr("idCliente", pIdCliente);
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });

    $("#txtAsigando").autocomplete({
        source: function (request, response) {
            var Usuario = new Object();
            Usuario.Usuario = request.term;
            var Request = JSON.stringify(Usuario);
            $.ajax({
                url: "ReporteLevantamiento.aspx/ObtenerUsuario",
                type: "post",
                data: Request,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (Respuesta) {
                    $("#divFiltrosReporteLevantamiento").attr("idAsignado", "");
                    var json = JSON.parse(Respuesta.d);
                    var Usuarios = json.Modelo.Usuarios;
                    response($.map(Usuarios, function (item) {
                        return { label: item.Agente, value: item.Agente }
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            var pIdAgente = ui.item.id;
            $("#divFiltrosReporteLevantamiento").attr("IdAgente", pIdAgente);
        }
    });

});

function FiltroReporteLevantamiento() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdReporteLevantamiento').getGridParam('rowNum');
    request.pPaginaActual = $('#grdReporteLevantamiento').getGridParam('page');
    request.pColumnaOrden = $('#grdReporteLevantamiento').getGridParam('sortname');
    request.pTipoOrden = $('#grdReporteLevantamiento').getGridParam('sortorder');
    request.pAI = 0;
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pIdSucursal = -1;
    request.pIdAgente = -1;
    request.pIdDivision = -1;
    request.pIdEstatusLevantamiento = -1;
    request.pIdCliente = -1;
    request.pIdOportunidad = -1;
    request.pIdAsignado = -1;

    //get data
    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        request.pFechaInicial = $("#txtFechaInicial").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }

    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        request.pFechaFinal = $("#txtFechaFinal").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }

    if (parseInt($("#cmbSucursal").val()) != 0 && $("#cmbSucursal").val() != null){
        request.pIdSucursal = parseInt($("#cmbSucursal").val());
    }

    if ($("#divFiltrosReporteLevantamiento").attr("idAgente") != "" && !isNaN($("#divFiltrosReporteLevantamiento").attr("idAgente"))){
        request.pIdAgente = parseInt($("#divFiltrosReporteLevantamiento").attr("idAgente"))
    }

    if (parseInt($("#cmbDivision").val()) != 0 && $("#cmbDivision").val() != null) {
        request.pIdDivision = parseInt($("#cmbDivision").val());
    }

    if (parseInt($("#cmbEstatus").val()) != 0 && $("#cmbEstatus").val() != null) {
        request.pIdEstatus = parseInt($("#cmbEstatus").val());
    }

    if ($("#divFiltrosReporteLevantamiento").attr("idCliente") != "" && !isNaN($("#divFiltrosReporteLevantamiento").attr("idCliente"))) {
        request.pIdCliente = parseInt($("#divFiltrosReporteLevantamiento").attr("idCliente"));
    }

    if ($("#txtIdOportunidad").val() != "" && $("#txtIdOportunidad").val() != null) {
        request.pIdOportunidad = parseInt($("#txtIdOportunidad").val());
    }

    if ($("#divFiltrosReporteLevantamiento").attr("idAsignado") != "" && !isNaN($("#divFiltrosReporteLevantamiento").attr("idAsignado"))) {
        request.pIdAsignado = parseInt($("#divFiltrosReporteLevantamiento").attr("idAsignado"));
    }
    
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'ReporteLevantamiento.aspx/ObtenerReporteLevantamiento',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') {
                $('#grdReporteLevantamiento')[0].addJSONData(JSON.parse(jsondata.responseText).d);

            }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}


function LlenaSucursales() {
    $.ajax({
        url: "ReporteLevantamiento.aspx/ObtenerSucursales",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                var Modelo = json.Modelo;
                var Sucursales = Modelo.Sucursales;
                //sucursales.clear();
                var select = document.getElementById('cmbSucursal');
                for (x in Sucursales) {
                    //console.log(Sucursales[x]);
                    var opt = document.createElement('option');
                    opt.value = Sucursales[x].Valor;
                    opt.innerHTML = Sucursales[x].Descripcion;
                    select.appendChild(opt);
                }
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function LlenaDivisiones() {
    $.ajax({
        url: "ReporteLevantamiento.aspx/ObtenerDivisiones",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                var Modelo = json.Modelo;
                var Divisiones = Modelo.Divisiones;
                //sucursales.clear();
                var select = document.getElementById('cmbDivision');
                for (x in Divisiones) {
                    //console.log(Divisiones[x]);
                    var opt = document.createElement('option');
                    opt.value = Divisiones[x].Valor;
                    opt.innerHTML = Divisiones[x].Descripcion;
                    select.appendChild(opt);
                }
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function LlenaEstatus() {
    $.ajax({
        url: "ReporteLevantamiento.aspx/ObtenerEstatus",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                var Modelo = json.Modelo;
                var Estatus = Modelo.Estatus;
                //sucursales.clear();
                var select = document.getElementById('cmbEstatus');
                for (x in Estatus) {
                    //console.log(Divisiones[x]);
                    var opt = document.createElement('option');
                    opt.value = Estatus[x].Valor;
                    opt.innerHTML = Estatus[x].Descripcion;
                    select.appendChild(opt);
                }
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}