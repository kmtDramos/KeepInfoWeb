/**/

$(function () {

    MantenerSesion();

    setInterval(MantenerSesion, 1000 * 60 * 1.5);

    $("#grdEntregaMaterial").on("click", ".imgFormaConsultarSolicitudMaterial", function () {
        console.log("Click");
        var registro = $(this).parents("tr");
        var SolicitudMaterial = new Object();
        SolicitudMaterial.pIdSolicitudMaterial = parseInt($(registro).children("td[aria-describedby='grdEntregaMaterial_IdSolicitudMaterial']").html());
        ObtenerFormaConsultarSolicitudMaterial(JSON.stringify(SolicitudMaterial));
    });

    $('#dialogConsultarSolicitudEntregaMaterial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaConsultarSolicitudEntregaMaterial").remove();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogConsultarSolicitudEntregaMaterial").on("click", "#divImprimir", function () {
        var IdSolicitudMaterial = $("#dialogConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial");
        Imprimir(IdSolicitudMaterial);
    });
});

function ObtenerFormaConsultarSolicitudMaterial(pIdSolicitudMaterial) {
    $("#dialogConsultarSolicitudEntregaMaterial").obtenerVista({
        nombreTemplate: "tmplConsultarSolicitudEntregaMaterial.html",
        url: "SalidaEntregaMaterial.aspx/ObtenerFormaSolicitudEntregaMaterial",
        parametros: pIdSolicitudMaterial,
        despuesDeCompilar: function (pRespuesta) {
            console.log("Forma Consulta Solicitud")
            console.log(pRespuesta.modelo);
            Inicializar_grdPartidasSolicitudMaterialConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarSalidaEntregaMaterial == 1) {
                
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "buttons", {
                    "Editar": function () {
                        $(this).dialog("close");
                        var SolicitudMaterial = new Object();
                        SolicitudMaterial.IdSolicitudMaterial = parseInt($("#divFormaConsultarSolicitudEntregaMaterial").attr("IdSolicitudMaterial"));
                        ObtenerFormaEditarSolicitudMaterial(JSON.stringify(SolicitudMaterial))
                    },
                    "Salir": function () {
                        $(this).dialog("close");
                    }
                });
                
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "buttons", {});
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "height", "100");
            }
            $("#dialogConsultarSolicitudEntregaMaterial").dialog("open");
        }
    });
}

function FiltroEntregaMaterial() {
    var SolicitudMaterial = new Object();
    SolicitudMaterial.pTamanoPaginacion = $('#grdEntregaMaterial').getGridParam('rowNum');
    SolicitudMaterial.pPaginaActual = $('#grdEntregaMaterial').getGridParam('page');
    SolicitudMaterial.pColumnaOrden = $('#grdEntregaMaterial').getGridParam('sortname');
    SolicitudMaterial.pTipoOrden = $('#grdEntregaMaterial').getGridParam('sortorder');
    SolicitudMaterial.pIdSolicitudMaterial = ($("#gs_IdSolicitudMaterial").val() == null) ? "" : $("#gs_IdSolicitudMaterial").val();

    var Request = JSON.stringify(SolicitudMaterial);
    $.ajax({
        url: "SalidaEntregaMaterial.aspx/ObtenerSolicitudEntregaMaterial",
        data: Request,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdEntregaMaterial')[0].addJSONData(JSON.parse(jsondata.responseText).d); OcultarBloqueo(); }
            else { console.log(JSON.parse(jsondata.responseText).Message); }
            TerminoInventario();
        }
    });
}

function FiltroPartidasSolicitudMaterialConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('sortorder');
    request.pIdNotaCredito = 0;
    //if ($("#divFormaConsultarSolicitudEntregaMaterial").attr("IdSolicitudMaterial") != null) {
        request.pIdSolicitudMaterial = "1";//; $("divFormaConsultarSolicitudEntregaMaterial").attr("IdSolicitudMaterial");
    //}
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerSolicitudEntregaMaterialConceptos',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdPartidasSolicitudMaterialConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function TerminoInventario() {
    $("td[aria-describedby=grdEntregaMaterial_Existencia]", "#grdEntregaMaterial tbody").each(function (index, element) {
        var IdExperienciaReal = $(element).parent("tr").children("td[aria-describedby=grdEntregaMaterial_IdExperienciaReal]").text();
        var input = $('<input type="text" value="' + $(element).text() + '" style="width:96%;text-align:right;"/>');
        //$(element).html(input);
        //$(input).change(function () { ActualizarExistenciaProducto(IdExperienciaReal, $(this).val()); });
    });
}

function Imprimir(pIdSolicitudMaterial) {
    MostrarBloqueo();

    var SolicitudMaterial = new Object();
    SolicitudMaterial.IdSolicitudMaterial = pIdSolicitudMaterial;

    var Request = JSON.stringify(SolicitudMaterial);

    var contenedor = $("<div></div>");

    $(contenedor).obtenerVista({
        url: "SalidaEntregaMaterial.aspx/Imprimir",
        parametros: Request,
        nombreTemplate: "tmplImprimirSalidaMaterial.html",
        despuesDeCompilar: function (Respuesta) {
            var plantilla = $(contenedor).html();
            var Impresion = window.open("", "");
            Impresion.document.write(plantilla);
            Impresion.print();
            Impresion.close();
        }
    });

}

/*
function ActualizarExistenciaProducto(IdExperienciaReal, Existencia) {
    var ExistenciaReal = new Object();
    ExistenciaReal.IdExperienciaReal = parseInt(IdExperienciaReal);
    ExistenciaReal.Existencia = parseInt(Existencia);
    var Request = JSON.stringify(ExistenciaReal);
    $.ajax({
        url: "InventarioReal.aspx/ActualizarExistenciaProducto",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (Respuesta) {
            FiltroInventario();
        }
    });
}

*/