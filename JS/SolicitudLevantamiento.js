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
                "Salir": function () {
                    $(this).dialog("close");
                }
            });

            $("#dialogConsultarSolicitudLevantamiento").dialog("open");

        }
    });
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
