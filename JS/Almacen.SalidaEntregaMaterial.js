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
    
    $('#dialogEditarSolicitudEntregaMaterial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaEditarSolicitudEntregaMaterial").remove();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogConsultarSolicitudEntregaMaterial").on("click", "#divImprimir", function () {
        var IdSolicitudMaterial = $("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial");
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
            if (pRespuesta.modelo.Permisos.puedeEditarSalidaEntregaMaterial == 1 && pRespuesta.modelo.Confirmado == 0) {
                
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
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("open");
            }
            else {
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "buttons", {});
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "height", "auto");
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("open");
            }
            
        }
    });
}

function ObtenerFormaEditarSolicitudMaterial(pIdSolicitudMaterial) {
    $("#dialogEditarSolicitudEntregaMaterial").obtenerVista({
        nombreTemplate: "tmplEditarSolicitudMaterial.html",
        url: "SalidaEntregaMaterial.aspx/ObtenerFormaEditarSolicitudEntregaMaterial",
        parametros: pIdSolicitudMaterial,
        despuesDeCompilar: function (pRespuesta) {
            console.log("Forma Edita Solicitud")
            console.log(pRespuesta.modelo);
            Inicializar_grdPartidasSolicitudMaterialEditar();
            if (pRespuesta.modelo.Permisos.puedeEditarSalidaEntregaMaterial == 1) {

                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "buttons", {
                    "Guardar": function () {
                        $(this).dialog("close");
                        var SolicitudMaterial = new Object();
                        SolicitudMaterial.IdSolicitudMaterial = parseInt($("#divFormaEditarSolicitudEntregaMaterial").attr("IdSolicitudMaterial"));
                        EditarSolicitudMaterial(SolicitudMaterial);
                    },
                    "Salir": function () {
                        $(this).dialog("close");
                    }
                });

                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "height", "auto");
            }
            else {
                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "buttons", {});
                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "height", "100");
            }
            $("#dialogEditarSolicitudEntregaMaterial").dialog("open");
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
    console.log(parseInt($("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial")));
    request.pIdSolicitudMaterial = 0;
    if ($("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != null && $("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != "") {
        request.pIdSolicitudMaterial = $("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial");
    }
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerSolicitudEntregaMaterialConceptosConsultar',
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

function FiltroPartidasSolicitudMaterialEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPartidasSolicitudMaterialEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPartidasSolicitudMaterialEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdPartidasSolicitudMaterialEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdPartidasSolicitudMaterialEditar').getGridParam('sortorder');
    console.log(parseInt($("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial")));
    request.pIdSolicitudMaterial = 0;
    if ($("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != null && $("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != "") {
        request.pIdSolicitudMaterial = $("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial");
    }
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerSolicitudEntregaMaterialConceptosEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdPartidasSolicitudMaterialEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
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

function EditarSolicitudMaterial(IdSolicitudMaterial) {
    var pSolicitudMaterial = new Object();

    pSolicitudMaterial.IdSolicitudMaterial = IdSolicitudMaterial.IdSolicitudMaterial;

    if ($("#chkConfirmado").is(':checked')) {
        pSolicitudMaterial.Aprobar = 1;
    }
    else {
        pSolicitudMaterial.Aprobar = 0;
    }

    pSolicitudMaterial.Comentarios = $("textarea#txtComentarios").val();
    
    var validacion = ValidaInventario();
    if (validacion != "") { MostrarMensajeError(validacion); return false; }
    
    SetEditarSolicitudMaterial(JSON.stringify(pSolicitudMaterial));
    
}

function SetEditarSolicitudMaterial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SalidaEntregaMaterial.aspx/EditarSolicitudEntregaMaterial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError("Se ha editado con exito!.");
                $("#grdEntregaMaterial").trigger("reloadGrid");
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

function ValidaInventario() {

    var errores = "";
    
    var ids = $('#grdPartidasSolicitudMaterialEditar').jqGrid('getDataIDs');
    console.log(ids);
    for (var i = 0; i < ids.length; i++) {

        var producto = $('#grdPartidasSolicitudMaterialEditar #' + ids[i] + ' td[aria-describedby="grdPartidasSolicitudMaterialEditar_NumeroParte"]').html();
        var disponible = $('#grdPartidasSolicitudMaterialEditar #' + ids[i] + ' td[aria-describedby="grdPartidasSolicitudMaterialEditar_DisponibleInventario"]').html();
        var cantidad = $('#grdPartidasSolicitudMaterialEditar #' + ids[i] + ' td[aria-describedby="grdPartidasSolicitudMaterialEditar_Cantidad"]').html();
        console.log(producto);
        console.log(cantidad);
        console.log(disponible);
        if (disponible == 0 || cantidad > disponible) {

            errores = errores + "<span>*</span> No hay en el inventario la cantidad suficiente para el producto: '" + producto + "'";
        }
        
    }

    if (errores != "") { errores = "<p>Favor de revisar lo siguiente:</p>" + errores; }

    return errores;
}
