//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $("#grdAsignacionCliente").on("click", ".imgFormaAsignadacionCliente", function() {
        var registro = $(this).parents("tr");
        var idUsuario = $(registro).children("td[aria-describedby='grdAsignacionCliente_IdUsuario']").html();
        $("#dialogConsultarAsignacionCliente").attr("idUsuario", idUsuario);
        FiltroCliente();
        $("#dialogConsultarAsignacionCliente").dialog("open");
    });

    $("#grdCliente").on("click", ".imgFormaConsultarCliente", function() {
        var registro = $(this).parents("tr");
        var idCliente = $(registro).children("td[aria-describedby='grdCliente_IdCliente']").html();
        var pRequest = new Object();
        pRequest.pIdCliente = idCliente;
        ObtenerFormaConsultarCliente(JSON.stringify(pRequest));
    });

    $("#dialogConsultarAsignacionCliente").on("click", "#btnAgregarCliente", function() {
        var registro = $(this).parents("tr");
        var idUsuario = $("#dialogConsultarAsignacionCliente").attr("idUsuario");
        var pRequest = new Object();
        pRequest.pIdUsuario = idUsuario;
        ObtenerFormaAsignacionCliente(JSON.stringify(pRequest));
    });

    $('#dialogConsultarAsignacionCliente').dialog({
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
            "Salir": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogAsignarCliente').dialog({
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
                AsignarCliente();
            }
        }
    });

    $('#dialogConsultarClienteAsignado').dialog({
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
            "Salir": function() {
                $(this).dialog("close")
            }
        }
    });


    $('#grdCliente').one('click', '.div_grdCliente_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCliente_AI']").children().attr("baja")
        var idAsignacion = $(registro).children("td[aria-describedby='grdCliente_IdAsignacionCliente']").html();
        
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idAsignacion, baja);
    });

});

function AsignarCliente() {
    var pCliente = new Object();
    pCliente.IdUsuario = $("#divFormaAsignarClienteUsuario").attr("idUsuario");
    pCliente.IdCliente = $(".tblFormaCliente").attr("idCliente");

    var pRequest = new Object();
    pRequest.pCliente = pCliente;
    SetAsignarCliente(JSON.stringify(pRequest));
}

function SetAsignarCliente(pRequest) {
    $.ajax({
        type: "POST",
        url: "AsignacionCliente.aspx/AsignarCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta == 0) {
                $("#grdCliente").trigger("reloadGrid");
                $("#grdAsignacionCliente").trigger("reloadGrid");
                $("#dialogAsignarCliente").dialog("close");
            }
            else if (respuesta == 1) {
                MostrarMensajeError("Cliente ya asignado!");
            }
            else {
                MostrarMensajeError("Fallo la conexión");
            }
        },
        complete: function() {

        }
    });
}

function SetCambiarEstatus(pIdAsignacion, pBaja) {
    var pRequest = "{'pIdAsignacion':" + pIdAsignacion + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "AsignacionCliente.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCliente").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCliente').one('click', '.div_grdCliente_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCliente_AI']").children().attr("baja")
                var idAsignacion = $(registro).children("td[aria-describedby='grdCliente_IdAsignacionCliente']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idAsignacion, baja);
            });
        }
    });
}


function ObtenerFormaAsignacionCliente(pRequest)
{
    $("#dialogAsignarCliente").obtenerVista({
        nombreTemplate: "tmplAsignarCliente.html",
        url: "AsignacionCliente.aspx/ObtenerFormaAsignacionCliente",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            CrearAutocompletar();
            $("#dialogAsignarCliente").dialog("open");
        }
    });
}

function obtenerCliente(pRequest) {
    $.ajax({
        url: 'AsignacionCliente.aspx/ObtenerCliente',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogAsignarCliente").obtenerVista({
                    nombreTemplate: "tmplAsignarCliente.html",
                    modelo: respuesta.Modelo.Cliente,
                    despuesDeCompilar: function(pRespuesta) {
                        CrearAutocompletar();
                        $("#dialogAsignarCliente").dialog("open");
                    }
                });
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

function ObtenerFormaConsultarCliente(pRequest) {
    $("#dialogConsultarClienteAsignado").obtenerVista({
        nombreTemplate: "tmplConsultarClienteAsignado.html",
        url: "AsignacionCliente.aspx/ObtenerFormaConsultarCliente",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarClienteAsignado").dialog("open");
        }
    });
}

function FiltroCliente() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCliente').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCliente').getGridParam('page');
    request.pColumnaOrden = $('#grdCliente').getGridParam('sortname');
    request.pTipoOrden = $('#grdCliente').getGridParam('sortorder');
    request.pRazonSocial = "";
    request.pIdUsuario = 0;
    if ($("#gs_RazonSocial").existe()) {
        request.pRazonSocial = $("#gs_RazonSocial").val();
    }
    if ($("#dialogConsultarAsignacionCliente").attr("idUsuario") != null) {
        request.pIdUsuario = $("#dialogConsultarAsignacionCliente").attr("idUsuario");
    }
    request.pAI = 0
    if ($("#gs_AI").existe()) {
        request.pAI = $("#gs_AI").val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'AsignacionCliente.aspx/ObtenerClientes',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdCliente')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function CrearAutocompletar() {
    $('#txtAutocompletarCliente').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtAutocompletarCliente').val();
            $.ajax({
                type: 'POST',
                url: 'AsignacionCliente.aspx/BuscarCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAsignarClienteUsuario").attr("idCliente", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
        
            var pIdCliente = ui.item.id;
            $("#divFormaAsignarClienteUsuario").attr("idCliente", pIdCliente);
            var Cliente = new Object();
            Cliente.pIdCliente = pIdCliente;
            Cliente.pIdUsuario = $("#divFormaAsignarClienteUsuario").attr("idUsuario");
            obtenerCliente(JSON.stringify(Cliente));
             
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    }); 
}

//function CrearAutocompletar() {
//    $('#txtAutocompletarCliente').autocomplete({
//        source: function(request, response) {
//            var pCliente = new Object();
//            pCliente.Cliente = $("#txtAutocompletarCliente").val();

//            var oRequest = new Object();
//            oRequest.pCliente = pCliente;

//            $.ajax({
//                type: 'POST',
//                url: 'AsignacionCliente.aspx/BuscarCliente',
//                data: JSON.stringify(oRequest),
//                dataType: 'json',
//                contentType: 'application/json; charset=utf-8',
//                success: function(pRespuesta) {
//                    var json = jQuery.parseJSON(pRespuesta.d);
//                    response($.map(json, function(item) {
//                        return { label: item.Cliente, value: item.Cliente, id: item.IdCliente }
//                    }));
//                }
//            });
//        },
//        minLength: 2,
//        select: function(event, ui) {
//            var pIdCliente = ui.item.id;
//            var Cliente = new Object();
//            Cliente.pIdCliente = pIdCliente;
//            Cliente.pIdUsuario = $("#divFormaAsignarClienteUsuario").attr("idUsuario"); ;
//            obtenerCliente(JSON.stringify(Cliente));
//        },
//        change: function(event, ui) { },
//        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
//        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
//    });
//}