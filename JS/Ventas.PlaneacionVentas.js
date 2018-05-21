/**/

var ver = true;

$(function () {

    MantenerSesion();

    setInterval(MantenerSesion, 1000 * 60 * 1.5);

    $("#sinPlaneacion").change(FiltroPlanVentas);
    $("#planeacionMes1").change(FiltroPlanVentas);

    $("#gbox_grdPlanVentas").livequery(function () {// MODIFICAR
        $("#grdPlanVentas").jqGrid('navButtonAdd', '#pagPlanVentas', {// MODIFICAR
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function () {
                var idoportunidad = '';
                if ($('#gbox_grdPlanVentas #gs_IdOportunidad').val() != null) {
                    idoportunidad = $('#gs_IdOportunidad').val();
                }
                var oportunidad = '';
                if ($('#gbox_grdPlanVentas #gs_Oportunidad').val() != null) {
                    oportunidad = $('#gs_Oportunidad').val();
                }
                var cliente = '';
                if ($('#gbox_grdPlanVentas #gs_Cliente').val() != null) {
                    cliente = $('#gs_Cliente').val();
                }
                var sucursal = -1;
                if ($('#gbox_grdPlanVentas #gs_Sucursal').val() != null) {
                    sucursal = $('#gbox_grdPlanVentas #gs_Sucursal').val();
                }
                var agente = '';
                if ($('#gbox_grdPlanVentas #gs_Agente').val() != null) {
                    agente = $('#gs_Agente').val();
                }
                var nivelinteres = -1;
                if ($('#gbox_grdPlanVentas #gs_NivelInteres').val() != null) {
                    nivelinteres = $('#gbox_grdPlanVentas #gs_NivelInteres').val();
                }
                var preventadetenido = -1;
                if ($('#gbox_grdPlanVentas #gs_PreventaDetenido').val() != null) {
                    preventadetenido = $('#gbox_grdPlanVentas #gs_PreventaDetenido').val();
                }
                var ventasdetenido = -1;
                if ($('#gbox_grdPlanVentas #gs_VentasDetenido').val() != null) {
                    ventasdetenido = $('#gbox_grdPlanVentas #gs_VentasDetenido').val();
                }
                var comprasdetenido = -1;
                if ($('#gbox_grdPlanVentas #gs_ComprasDetenido').val() != null) {
                    comprasdetenido = $('#gbox_grdPlanVentas #gs_ComprasDetenido').val();
                }
                var proyectosdetenido = -1;
                if ($('#gbox_grdPlanVentas #gs_ProyectosDetenido').val() != null) {
                    proyectosdetenido = $('#gbox_grdPlanVentas #gs_ProyectosDetenido').val();
                }
                var finzanzasdetenido = -1;
                if ($('#gbox_grdPlanVentas #gs_FinzanzasDetenido').val() != null) {
                    finzanzasdetenido = $('#gbox_grdPlanVentas #gs_FinzanzasDetenido').val();
                }
                var division = (($("#gs_Division").val() != null) ? parseInt($("#gs_Division").val()) : -1);
                var sinPlaneacion = 0;
                sinPlaneacion = ($("#sinPlaneacion").is(":checked")) ? 1 : 0;
                var planeacionMes1 = 0;
                planeacionMes1 = ($("#planeacionMes1").is(":checked")) ? 1 : 0;

                var EsProyecto = $('#gbox_grdPlanVentas #gs_EsProyecto').val();
                var Autorizado = $('#gbox_grdPlanVentas #gs_Autorizado').val();

                $.UnifiedExportFile({
                    action: '../ExportacionesExcel/ExportarExcelPlanVentas.aspx',
                    data: {
                        'pColumnaOrden': $('#grdPlanVentas').getGridParam('sortname'),
                        'pTipoOrden': $('#grdPlanVentas').getGridParam('sortorder'),
                        'pIdOportunidad': idoportunidad,
                        'pOportunidad': oportunidad,
                        'pCliente': cliente,
                        'pSucursal': sucursal,
                        'pAgente': agente,
                        'pNivelInteres': nivelinteres,
                        'pPreventaDetenido': preventadetenido,
                        'pVentasDetenido': ventasdetenido,
                        'pComprasDetenido': comprasdetenido,
                        'pProyectosDetenido': proyectosdetenido,
                        'pFinzanzasDetenido': finzanzasdetenido,
                        'pSinPlaneacion': sinPlaneacion,
                        'planeacionMes1': planeacionMes1,
                        'pDivision': division,
                        'pEsProyecto': EsProyecto,
                        'pAutorizado':Autorizado
                    },
                    downloadType: 'Normal'
                });

            }
        });
    });

    $("#btnAgregarOportunidad").click(ObtenerFormaAgregarOportunidad);

    $("#btnReporteCompras").click(function () {
    	if ($(this).hasClass('listo')) {
    		ReporteOrdenesCompras();
    	}
    }).mouseover(function () {
    	$(this).addClass('listo');
    }).mouseout(function () {
    	$(this).removeClass('listo');
    });

    $("#btnSabanaAutorizados").click(function () {
    	var ventana = $("<div></div>");
    	$(ventana).dialog({
    		modal: true,
			autoOpen: false,
    		draggable: false,
    		resizable: false,
			width: "auto",
    		close: function () {
    			$(ventana).remove();
    		},
    		buttons: {
    			"Cerrar": function () { $(this).dialog("close");}
    		}
    	});
    	$(ventana).obtenerVista({
    		url: "PlaneacionVentas.aspx/SabanaAutorizado",
    		nombreTemplate: "tmplSabanaAutorizados.html",
    		despuesDeCompilar: function () {
				$(ventana).dialog("open")
    		}
    	});
    });
    
	 $("#dialogAgregarOportunidad").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaAgregarOportunidad").remove();
        },
        buttons: {
            "Agregar": function () {
                AgregarOportunidad();
            },
            "Cancelar": function () {
                $(this).dialog("close")
            }
        }
	 });

	 $("#btnReporteAutorizadoVendedores").click(function () {
	 	var Ventana = $("<div></div>");
	 	$(Ventana).dialog({
	 		modal: true,
	 		autoOpen: false,
			width: "auto",
	 		draggable: false,
	 		resizable: false,
	 		close: function () {
	 			$(this).remove();
	 		},
	 		buttons: {
	 			"Cerrar": function () {
	 				$(this).dialog("close");
	 			}
	 		}
	 	});
	 	$(Ventana).obtenerVista({
	 		url: "PlaneacionVentas.aspx/ObtenerAutorizadosVendedores",
	 		nombreTemplate: "tmplReporteAutorizados.html",
	 		despuesDeCompilar: function () {
	 			$(Ventana).dialog("open");
	 		}
	 	});
	 });

});

function FiltroPlanVentas() {
	MostrarBloqueo();
    var idoportunidad = '';
    if ($('#gbox_grdPlanVentas #gs_IdOportunidad').val() != null) {
        idoportunidad = $('#gs_IdOportunidad').val();
    }
    var oportunidad = '';
    if ($('#gbox_grdPlanVentas #gs_Oportunidad').val() != null) {
        oportunidad = $('#gs_Oportunidad').val();
    }
    var cliente = '';
    if ($('#gbox_grdPlanVentas #gs_Cliente').val() != null) {
        cliente = $('#gs_Cliente').val();
    }
    var condicionPago = -1;
    if ($("#gs_CondicionPago").val() != null) {
    	condicionPago = $("#gs_CondicionPago").val();
    }
    condicionPago = (condicionPago == "") ? -1 : condicionPago;
    var sucursal = -1;
    if ($('#gbox_grdPlanVentas #gs_Sucursal').val() != null) {
        sucursal = $('#gbox_grdPlanVentas #gs_Sucursal').val();
    }
    var agente = '';
    if ($('#gbox_grdPlanVentas #gs_Agente').val() != null) {
        agente = $('#gs_Agente').val();
    }
    var nivelinteres = -1;
    if ($('#gbox_grdPlanVentas #gs_NivelInteres').val() != null) {
        nivelinteres = $('#gbox_grdPlanVentas #gs_NivelInteres').val();
    }
    var preventadetenido = -1;
    if ($('#gbox_grdPlanVentas #gs_PreventaDetenido').val() != null) {
        preventadetenido = $('#gbox_grdPlanVentas #gs_PreventaDetenido').val();
    }
    var ventasdetenido = -1;
    if ($('#gbox_grdPlanVentas #gs_VentasDetenido').val() != null) {
        ventasdetenido = $('#gbox_grdPlanVentas #gs_VentasDetenido').val();
    }
    var comprasdetenido = -1;
    if ($('#gbox_grdPlanVentas #gs_ComprasDetenido').val() != null) {
        comprasdetenido = $('#gbox_grdPlanVentas #gs_ComprasDetenido').val();
    }
    var proyectosdetenido = -1;
    if ($('#gbox_grdPlanVentas #gs_ProyectosDetenido').val() != null) {
        proyectosdetenido = $('#gbox_grdPlanVentas #gs_ProyectosDetenido').val();
    }
    var finzanzasdetenido = -1;
    if ($('#gbox_grdPlanVentas #gs_FinzanzasDetenido').val() != null) {
        finzanzasdetenido = $('#gbox_grdPlanVentas #gs_FinzanzasDetenido').val();
    }
    var EsProyecto = -1;
    if ($('#gs_EsProyecto').val() != null) { EsProyecto = parseInt($('#gs_EsProyecto').val()) }
    var Autorizado = -1;
    if ($('#gs_Autorizado').val() != null) { Autorizado = parseInt($('#gs_Autorizado').val()) }
    var division = (($("#gs_Division").val() != null) ? parseInt($("#gs_Division").val()) : -1);
    var sinPlaneacion = 0;
    sinPlaneacion = ($("#sinPlaneacion").is(":checked")) ? 1 : 0;
    var planeacionMes1 = 0;
    planeacionMes1 = ($("#planeacionMes1").is(":checked")) ? 1 : 0;
    $.ajax({
        url: 'PlaneacionVentas.aspx/ObtenerPlanVentas',
        data: "{'pTamanoPaginacion':" + $('#grdPlanVentas').getGridParam('rowNum') + ",'pPaginaActual':" + $('#grdPlanVentas').getGridParam('page') + ",'pColumnaOrden':'" + $('#grdPlanVentas').getGridParam('sortname') + "','pTipoOrden':'" + $('#grdPlanVentas').getGridParam('sortorder') + "','pIdOportunidad':'" + idoportunidad + "','pOportunidad':'" + oportunidad + "','pCliente':'" + cliente + "','pSucursal':" + sucursal + ",'pAgente':'" + agente + "','pNivelInteres':" + nivelinteres + ",'pPreventaDetenido':" + preventadetenido + ",'pVentasDetenido':" + ventasdetenido + ",'pComprasDetenido':" + comprasdetenido + ",'pProyectosDetenido':" + proyectosdetenido + ",'pFinzanzasDetenido':" + finzanzasdetenido + ",'pSinPlaneacion':" + sinPlaneacion + ",'planeacionMes1':" + planeacionMes1 + ",'pDivision':" + division + ",'pEsProyecto':" + EsProyecto + ",'pAutorizado':" + Autorizado + ",'pIdCondicionPago': " + condicionPago + "}",
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') $('#grdPlanVentas')[0].addJSONData(JSON.parse(jsondata.responseText).d);
            else console.log(JSON.parse(jsondata.responseText).Message); 
        }
    });
}

function Termino_grdPlanVentas() {

    $("td[aria-describedby=grdPlanVentas_IdOportunidad]").click(function () {
        var Oportunidad = new Object();
        Oportunidad.pIdOportunidad = parseInt($(this).text());
        var Request = JSON.stringify(Oportunidad);
        ObtenerFormaEditarOportunidad(Request)
    });

    $("td[aria-describedby=grdPlanVentas_Autorizado]").each(function (index, element) {
        var checkbox = $('<input class="autorizado" type="checkbox" />')
        var autorizado = ($(element).text() == 'True');
        $(element).html('').append(checkbox);
        $(checkbox).prop('checked', autorizado).click(function (event) {
            event.preventDefault();
            AutorizarOportunidad(element);
        });
    });

    $("td[aria-describedby=grdPlanVentas_Baja]").each(function (index, element) {
        var IdOportunidad = $(element).parent("tr").children("td[aria-describedby=grdPlanVentas_IdOportunidad]").text()
        $(element).html('<img src="../Images/on.png" onclick="EliminarOportunidad(' + IdOportunidad + ');" style="cursor:pointer;"/>');
    });

    //
    $("td[aria-describedby=grdPlanVentas_Mes1]").each(function (index, element) {
        var input = $('<input type="text" class="Mes1 monto" style="width:96%;"/>').val($(element).text());
        $(input).change(ActualizarPlanVentas).css("background-color", "#EEE");
        InitCampo(input);
        $(element).html(input);
    });

    $("td[aria-describedby=grdPlanVentas_Mes2]").each(function (index, element) {
        var input = $('<input type="text" class="Mes2 monto" style="width:96%;"/>').val($(element).text());
        $(input).change(ActualizarPlanVentas).css("background-color", "#DDD");
        InitCampo(input);
        $(element).html(input);
    });

    $("td[aria-describedby=grdPlanVentas_Mes3]").each(function (index, element) {
        var input = $('<input type="text" class="Mes3 monto" style="width:96%;"/>').val($(element).text());
        $(input).change(ActualizarPlanVentas).css("background-color", "#CCC");
        InitCampo(input);
        $(element).html(input);
    });

    // Campos de fecha
    $("td[aria-describedby=grdPlanVentas_PreventaDetenido]").each(function (index, element) {
        var marcado = ($(element).text() == "True") ? "underline" : "";
        var fecha = $("td[aria-describedby=grdPlanVentas_CompromisoPreventa]", $(element).parent("tr")).text();
        fecha = (fecha == '01/01/1900') ? "" : fecha;
        var input = $('<input type="text" class="Preventa" value="' + fecha + '" style="width:50px;"/>');
        var Oportunidad = new Object();
        Oportunidad.IdOportunidad = parseInt($("td[aria-describedby=grdPlanVentas_IdOportunidad]", $(element).parent("tr")).text());
        Oportunidad.Fecha = 1;
        $(element).attr("title", fecha);
        $(element).html(input);
        $(input).click(function () { MostrarFecha(JSON.stringify(Oportunidad)); }).css({ "background-color": (marcado) ? "#DDD" : "none" });
    });

    $("td[aria-describedby=grdPlanVentas_VentasDetenido]").each(function (index, element) {
        var marcado = ($(element).text() == "True") ? "underline" : "";
        var fecha = $("td[aria-describedby=grdPlanVentas_CompromisoVenta]", $(element).parent("tr")).text();
        fecha = (fecha == '01/01/1900') ? "" : fecha;
        var input = $('<input type="text" class="Ventas" value="' + fecha + '" style="width:50px;"/>');
        var Oportunidad = new Object();
        Oportunidad.IdOportunidad = parseInt($("td[aria-describedby=grdPlanVentas_IdOportunidad]", $(element).parent("tr")).text());
        Oportunidad.Fecha = 2;
        $(element).html(input);
        $(input).click(function () { MostrarFecha(JSON.stringify(Oportunidad)); }).css({ "background-color": (marcado) ? "#DDD" : "none" });
    });

    $("td[aria-describedby=grdPlanVentas_ComprasDetenido]").each(function (index, element) {
        var marcado = ($(element).text() == "True") ? "underline" : "";
        var fecha = $("td[aria-describedby=grdPlanVentas_CompromisoCompras]", $(element).parent("tr")).text();
        fecha = (fecha == '01/01/1900') ? "" : fecha;
        var input = $('<input type="text" class="Compras" value="' + fecha + '" style="width:50px;"/>');
        var Oportunidad = new Object();
        Oportunidad.IdOportunidad = parseInt($("td[aria-describedby=grdPlanVentas_IdOportunidad]", $(element).parent("tr")).text());
        Oportunidad.Fecha = 3;
        $(element).html(input);
        $(input).click(function () { MostrarFecha(JSON.stringify(Oportunidad)); }).css({ "background-color": (marcado) ? "#DDD" : "none" });
    });

    $("td[aria-describedby=grdPlanVentas_ProyectosDetenido]").each(function (index, element) {
        var marcado = ($(element).text() == "True") ? "underline" : "";
        var fecha = $("td[aria-describedby=grdPlanVentas_CompromisoProyectos]", $(element).parent("tr")).text();
        fecha = (fecha == '01/01/1900') ? "" : fecha;
        var input = $('<input type="text" class="Proyectos" value="' + fecha + '" style="width:50px;"/>');
        var Oportunidad = new Object();
        Oportunidad.IdOportunidad = parseInt($("td[aria-describedby=grdPlanVentas_IdOportunidad]", $(element).parent("tr")).text());
        Oportunidad.Fecha = 4;
        $(element).html(input);
        $(input).click(function () { MostrarFecha(JSON.stringify(Oportunidad)); }).css({ "background-color": (marcado) ? "#DDD" : "none" });
    });

    $("td[aria-describedby=grdPlanVentas_FinzanzasDetenido]").each(function (index, element) {
        var marcado = ($(element).text() == "True");
        var fecha = $("td[aria-describedby=grdPlanVentas_CompromisoFinanzas]", $(element).parent("tr")).text();
        fecha = (fecha == '01/01/1900') ? "" : fecha;
        var input = $('<input type="text" class="Finanzas" value="' + fecha + '" style="width:50px;"/>');
        var Oportunidad = new Object();
        Oportunidad.IdOportunidad = parseInt($("td[aria-describedby=grdPlanVentas_IdOportunidad]", $(element).parent("tr")).text());
        Oportunidad.Fecha = 5;
        $(element).html(input);
        $(input).click(function () { MostrarFecha(JSON.stringify(Oportunidad)); }).css({"background-color":(marcado)?"#DDD":"none"});
    });

    $("tr", "#grdPlanVentas tbody").each(function (index, element) {
        var Mes1 = parseFloat(QuitaFormatoMoneda($(".Mes1", element).val()));
        var Mes2 = parseFloat(QuitaFormatoMoneda($(".Mes2", element).val()));
        var Mes3 = parseFloat(QuitaFormatoMoneda($(".Mes3", element).val()));
        if ((Mes1 + Mes2 + Mes3) == 0) {
            $(element).css("color", "red");
        }
    });

    $("td[aria-describedby=grdPlanVentas_FechaEntrega]").each(function (index, element) {
    	var input = $("<input class='FechaEntrega'/>");
    	var fecha = $(element).text();
    	$(input).datepicker();
    	$(input).val('hola');
    	$(element).html(input);
    });

    TotalesPlanVentas();
    TotalesPlanVentasDepartamento();
    TotalesPlanVentasSucursal();
    ProyectoPedidoAutorizados();
}

function MostrarFecha(Oportunidad) {
	var FechaComentario = JSON.parse(Oportunidad);
    var ventana = $("<div></div>");
    $(ventana).dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        draggable: false,
        close: function () { $(this).remove() },
        buttons: {
        	"Guardar": function () {
        		FechaComentario.FechaCompromiso = $("#txtFechaCompromiso", ventana).val();
        		FechaComentario.FechaTerminado = $("#txtFechaTermino", ventana).val();
        		FechaComentario.Detenido = $("#chkActivo", ventana).is(":checked");
        		FechaComentario.Fecha = parseInt($("#divFormaFechas", ventana).attr("TipoFecha"));
        		FechaComentario.Comentario = $("#txtComentarioCompromiso").val();
                
                var error = "";
                
                if ($("#txtFechaCompromiso").val() != $("#txtFechaCompromiso").attr("FechaOriginal") && $("#txtComentarioCompromiso").val() == "") {
                	error += "<li>La fecha de fecha de compromiso ha cambiado.</li>";
                	console.log(1);
                }

                if ($("#txtFechaTermino").val() != $("#txtFechaTermino").attr("FechaOriginal") && $("#txtComentarioCompromiso").val() == "") {
                	error += "<li>La fecha de fecha de terminación ha cambiado.</li>";
                	console.log(2);
                }

                if (error == "") {
                	GuardarFechasOportunidad(JSON.stringify(FechaComentario));
                	$(this).dialog("close");
                }
                else
                {
                	error = "Se requiere agregar un comentario:<ul>" + error + "</ul>";
                	MostrarMensajeError(error);
                }
            },
            "Cancelar": function () { $(this).dialog("close"); }
        }
    });
    $(ventana).obtenerVista({
        url: "PlaneacionVentas.aspx/ObtenerFechaCumplimiento",
        parametros: Oportunidad,
        nombreTemplate: "tmplControlFechaPlanVentas.html",
        despuesDeCompilar: function (Respuesta) {
            $(ventana).dialog("open");
            $("#txtFechaCompromiso").datepicker();
            $("#txtFechaTermino").datepicker();
        }
    });
}

function GuardarFechasOportunidad(Oportunidad) {
    $.ajax({
        url: "PlaneacionVentas.aspx/GuardarFechasOportunidad",
        type: "post",
        data: Oportunidad,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                FiltroPlanVentas();
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
            
        }
    });
}

function InitCampo(input) {
    $(input).dblclick(function () {
        var fila = $(input).parent("td").parent("tr");
        var Diferencia = QuitaFormatoMoneda($("td[aria-describedby=grdPlanVentas_Diferencia]", fila).text());
        $(this).val(Diferencia).change();
    }).focus(function () {
        $(this).val(QuitaFormatoMoneda($(this).val())).select()
    }).blur(function () {
        $(this).val(formato.moneda($(this).val(), '$'));
    }).css("color", "inherit");
}

function ActualizarPlanVentas() {
    var Oportunidad = new Object();
    var fila = $(this).parent("td").parent("tr");
    Oportunidad.IdOportunidad = parseInt($("td[aria-describedby=grdPlanVentas_IdOportunidad]", fila).text());
    Oportunidad.Mes1 = parseFloat(QuitaFormatoMoneda($(".Mes1", fila).val()));
    Oportunidad.Mes2 = parseFloat(QuitaFormatoMoneda($(".Mes2", fila).val()));
    Oportunidad.Mes3 = parseFloat(QuitaFormatoMoneda($(".Mes3", fila).val()));
    Oportunidad.Preventa = $(".Preventa", fila).hasClass("underline");
    Oportunidad.Venta = $(".Ventas", fila).hasClass("underline");
    Oportunidad.Compras = $(".Compras", fila).hasClass("underline");
    Oportunidad.Proyectos = $(".Proyectos", fila).hasClass("underline");
    Oportunidad.Finanzas = $(".Finanzas", fila).hasClass("underline");
    Oportunidad.CompromisoPreventa = $(".Preventa", fila).val().trim();
    Oportunidad.CompromisoVentas = $(".Ventas", fila).val().trim();
    Oportunidad.CompromisoCompras = $(".Compras", fila).val().trim();
    Oportunidad.CompromisoProyectos = $(".Proyectos", fila).val().trim();
    Oportunidad.CompromisoFinanzas = $(".Finanzas", fila).val().trim();

    var Request = JSON.stringify(Oportunidad);

    $.ajax({
        url: "PlaneacionVentas.aspx/GuardarPlanVentas",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {

            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
            FiltroPlanVentas();
        }
    });
}

function TotalesPlanVentas() {
    var PlanVentas = new Object();
    PlanVentas.pIdOportunidad = $("#gs_IdOportunidad").val();
    PlanVentas.pOportunidad = $("#gs_Oportunidad").val();
    PlanVentas.pCliente = $("#gs_Cliente").val();
    PlanVentas.pIdSucursal = parseInt($("#gs_Sucursal").val());
    PlanVentas.pAgente = $("#gs_Agente").val();
    PlanVentas.pNivelInteres = parseInt($("#gs_NivelInteres").val());
    PlanVentas.pPreventaDetenido = parseInt($("#gs_PreventaDetenido").val());
    PlanVentas.pVentasDetenido = parseInt($("#gs_VentasDetenido").val());
    PlanVentas.pComprasDetenido = parseInt($("#gs_ComprasDetenido").val());
    PlanVentas.pProyectosDetenido = parseInt($("#gs_ProyectosDetenido").val());
    PlanVentas.pFinzanzasDetenido = parseInt($("#gs_FinzanzasDetenido").val());

    var Request = JSON.stringify(PlanVentas);

    $.ajax({
        url: "PlaneacionVentas.aspx/ObtenerTotalMeses",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                $("#facturado").text(formato.moneda(json.Modelo.Facturado, '$'));
                $("#Mes1").text(formato.moneda(json.Modelo.Mes1, '$'));
                $("#plan").text(formato.moneda(json.Modelo.PlanCierre, '$'));
                $("#Mes2").text(formato.moneda(json.Modelo.Mes2, '$'));
                $("#Mes3").text(formato.moneda(json.Modelo.Mes3, '$'));
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function TotalesPlanVentasSucursal() {
    var PlanVentas = new Object();
    PlanVentas.pIdOportunidad = $("#gs_IdOportunidad").val();
    PlanVentas.pOportunidad = $("#gs_Oportunidad").val();
    PlanVentas.pCliente = $("#gs_Cliente").val();
    PlanVentas.pIdSucursal = parseInt($("#gs_Sucursal").val());
    PlanVentas.pAgente = $("#gs_Agente").val();
    PlanVentas.pNivelInteres = parseInt($("#gs_NivelInteres").val());
    PlanVentas.pPreventaDetenido = parseInt($("#gs_PreventaDetenido").val());
    PlanVentas.pVentasDetenido = parseInt($("#gs_VentasDetenido").val());
    PlanVentas.pComprasDetenido = parseInt($("#gs_ComprasDetenido").val());
    PlanVentas.pProyectosDetenido = parseInt($("#gs_ProyectosDetenido").val());
    PlanVentas.pFinzanzasDetenido = parseInt($("#gs_FinzanzasDetenido").val());

    var Request = JSON.stringify(PlanVentas);

    $.ajax({
        url: "PlaneacionVentas.aspx/ObtenerTotalesSucursal",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
        	OcultarBloqueo();
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                $("#mty").text(formato.moneda(json.Modelo.Monterrey, '$'));
                $("#mex").text(formato.moneda(json.Modelo.Mexico, '$'));
                $("#gdl").text(formato.moneda(json.Modelo.Guadalajara, '$'));
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function TotalesPlanVentasDepartamento() {
    var PlanVentas = new Object();
    PlanVentas.pIdOportunidad = $("#gs_IdOportunidad").val();
    PlanVentas.pOportunidad = $("#gs_Oportunidad").val();
    PlanVentas.pCliente = $("#gs_Cliente").val();
    PlanVentas.pIdSucursal = parseInt($("#gs_Sucursal").val());
    PlanVentas.pAgente = $("#gs_Agente").val();
    PlanVentas.pNivelInteres = parseInt($("#gs_NivelInteres").val());
    PlanVentas.pPreventaDetenido = parseInt($("#gs_PreventaDetenido").val());
    PlanVentas.pVentasDetenido = parseInt($("#gs_VentasDetenido").val());
    PlanVentas.pComprasDetenido = parseInt($("#gs_ComprasDetenido").val());
    PlanVentas.pProyectosDetenido = parseInt($("#gs_ProyectosDetenido").val());
    PlanVentas.pFinzanzasDetenido = parseInt($("#gs_FinzanzasDetenido").val());

    var Request = JSON.stringify(PlanVentas);

    $.ajax({
        url: "PlaneacionVentas.aspx/ObtenerTotalDepartamentos",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
            	$("#preventa").text(formato.moneda(json.Modelo.Preventa, '$'));
            	$("#TotalPreventa").text(json.Modelo.TotalPreventa);
            	$("#venta").text(formato.moneda(json.Modelo.Ventas, '$'));
            	$("#TotalVenta").text(json.Modelo.TotalVentas);
            	$("#compras").text(formato.moneda(json.Modelo.Compras, '$'));
            	$("#TotalCompras").text(json.Modelo.TotalCompras);
            	$("#proyectos").text(formato.moneda(json.Modelo.Proyectos, '$'));
            	$("#TotalProyectos").text(json.Modelo.TotalProyectos);
            	$("#finanzas").text(formato.moneda(json.Modelo.Finanzas, '$'));
            	$("#TotalFinanzas").text(json.Modelo.TotalFinanzas);
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

//
function EliminarOportunidad(IdOportunidad) {

    validaOportunidad(IdOportunidad, function (resp) {

        if (!resp) {
            var ventana = $('<div><p>Motivo cancelación:</p><textarea id="txtMotivoCancelacion" style="width:280px;height:80px;" maxlength="500" placeholder="Motivo"></textarea></div>');
            $(ventana).dialog({
                modal: true,
                draggable: false,
                resizable: false,
                width: "auto",
                close: function () { $(this).remove(); },
                buttons: {
                    "Inactivar": function () {
                        var Oportunidad = new Object();
                        Oportunidad.IdOportunidad = IdOportunidad;
                        Oportunidad.MotivoCancelacion = $("#txtMotivoCancelacion", ventana).val();
                        var Request = JSON.stringify(Oportunidad);
                        $.ajax({
                            url: "PlaneacionVentas.aspx/EliminarOportunidad",
                            type: "post",
                            data: Request,
                            dataType: "json",
                            contentType: "application/json; charset=utf-8",
                            success: function () {
                                FiltroPlanVentas();
                            }
                        });
                    },
                    "Cancelar": function () { $(ventana).dialog("close"); }
                }
            });
        } else {
            MostrarMensajeError("No se puede Elimiar la Oportunidad por que ya cuenta con algun movimiento.");
        }
    });
}

//VALIDA OPORTUNIDAD
function validaOportunidad(IdOportunidad, callback) {
    var request = new Object();
    request.IdOportunidad = IdOportunidad;
    var resp;
    $.ajax({
        url: "PlaneacionVentas.aspx/validaOportunidad",
        type: "post",
        data: JSON.stringify(request),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            console.log(Respuesta);
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                resp = json.Respuesta;
                console.log(resp);
                callback(resp);
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

// Agregar Oportunidad
function ObtenerFormaAgregarOportunidad() {
    $("#dialogAgregarOportunidad").obtenerVista({
        nombreTemplate: "tmplAgregarOportunidad.html",
        url: "Oportunidad.aspx/ObtenerFormaAgregarOportunidad",
        despuesDeCompilar: function (pRespuesta) {
            AutocompletarClienteOportunidad();
            $("#dialogAgregarOportunidad").dialog("open");
            $("#txtProveedores").on("keypress keyup keydown", function () {
                var lb = $(this).val().split("\n").length;
                var l = $(this).val().length
                var r = Math.floor(l / 43) + lb;
                $(this).attr("rows", r);
            });
            $("#txtFechaCierre").datepicker({
                dateFormat: "dd/mm/yy",
                minDate: new Date()
            });
        }
    });
}

function AgregarOportunidad() {
    var pOportunidad = new Object();
    pOportunidad.pOportunidad = $("#txtOportunidad").val();
    pOportunidad.pIdCliente = $("#divFormaAgregarOportunidad").attr("idCliente");
    pOportunidad.pMonto = $("#txtMontoOportunidad").val().replace("$", "").replace(",", "");
    pOportunidad.pFechaCierre = $("#txtFechaCierre").val();
    pOportunidad.IdNivelInteresOportunidad = parseInt($("#cmbNivelInteresOportunidad").val());
    pOportunidad.pIdDivision = parseInt($("#cmbDivisionOportunidad").val());
    pOportunidad.pEsProyecto = parseInt($("#cmbEsProyecto").val());
    pOportunidad.pUrgente = parseInt($("#cmbUrgente").val());
    pOportunidad.pIdCampana = parseInt($("#cmbCampana").val());
    pOportunidad.pProveedores = $("#txtProveedores").val();
    pOportunidad.pUtilidad = parseInt($("#txtMargen").val());
    pOportunidad.pCosto = parseFloat($("#txtCosto").val().replace("$", "").replace(",", ""));
    var validacion = ValidarOportunidad(pOportunidad);
    if (validacion != "") {
        MostrarMensajeError(validacion);
        return false;
    }
    var oRequest = new Object();
    oRequest.pOportunidad = pOportunidad;
    SetAgregarOportunidad(JSON.stringify(oRequest));
}

function SetAgregarOportunidad(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/AgregarOportunidad",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdOportunidad").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogAgregarOportunidad").dialog("close");
        }
    });
}

// Editar Oportunidad
function ObtenerFormaEditarOportunidad(request) {
	var Oportunidad = JSON.parse(request);
	var ventana = $('<div id="dialogEditarOportunidad" Title="Oportunidad ' + Oportunidad.pIdOportunidad + '"></div>');
    $(ventana).dialog({
        modal: true,
        autoOpen: false,
        resizable: false,
        width: "auto",
        draggable: false,
        cloase: function () { $(this).remove(); },
        buttons: {
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogEditarOportunidad").obtenerVista({
        nombreTemplate: "tmplEditarOportunidad.html",
        url: "Oportunidad.aspx/ObtenerFormaEditarOportunidad",
        parametros: request,
        despuesDeCompilar: function (pRespuesta) {

        	AutocompletarUsuario();

            AutocompletarClienteOportunidad();

            $("#tabOportunidad").tabs();

            $("#dialogEditarOportunidad").dialog("open");

            $("#txtProveedores").on("keypress keyup keydown", function () {
                var lb = $(this).val().split("\n").length;
                var ll = $(this).val().split("\n")[$(this).val().split("\n").length - 1];
                var r = lb + Math.floor(ll.length / 43);
                $(this).attr("rows", r);
            }).keypress();

            $("#txtFechaCierre").datepicker({
                dateFormat: "dd/mm/yy",
                minDate: new Date()
            });

            $("#tblContactoCliente", "#dialogEditarOportunidad").DataTable({
                "oLanguage": { "sUrl": "../JS/Spanish.json" }
            });

            $("#tblProyectos", "#dialogEditarOportunidad").DataTable({
            	"oLanguage": { "sUrl": "../JS/Spanish.json" },
				"scrollCollapse": false
            });

            $("#tblFacturas", "#dialogEditarOportunidad").DataTable({
                "oLanguage": { "sUrl": "../JS/Spanish.json" },
                "scrollCollapse": false
            });

            $("#tblCompras", "#dialogEditarOportunidad").DataTable({
                "oLanguage": { "sUrl": "../JS/Spanish.json" },
                "scrollCollapse": false
            });

            $("#cmbDivisionOportunidad").change(function () {
                var Division = new Object();
                Division.IdDivision = parseInt($(this).val());
                var Request = JSON.stringify(Division);
                $("#iDivisionDescripcion").attr("title", '');
                ObtenerDescripcionDivision(Request);
            }).change();

            $('#tabOportunidad').bind('tabsshow', function (event, ui) {
                switch (ui.index) {
                    case 1:
                        $("#commit").scrollTop($("#commit")[0].scrollHeight);
                        break;
                }
            });
            
            costoUpDown();
        }
    });
}

///////////////////////////////////

/* NUEVA MANERA DE CALCULAR UTILIDAD */
var oldValueMonto = parseFloat($('input#txtMontoOportunidad').val().replace("$", "").replace(/,/g, ""));
var oldValueMargen = parseFloat($("#txtMargen").val());
var oldValueCosto = parseFloat($("#txtCosto").val().replace("$", "").replace(",", ""));
function calculoMontoMargenCosto(event) {
    var Monto = parseFloat($("#txtMontoOportunidad").val().replace("$", "").replace(/,/g, ""));
    var Margen = parseFloat($("#txtMargen").val());
    var Costo = parseFloat($("#txtCosto").val().replace("$", "").replace(/,/g, ""));

    Monto = (!isNaN(Monto) && isFinite(Monto)) ? Monto : 0;
    Margen = (!isNaN(Margen) && isFinite(Margen)) ? Margen : 0;
    Costo = (!isNaN(Costo) && isFinite(Costo)) ? Costo : 0;

    console.log(Monto);
    console.log(Margen);
    console.log(Costo);

    if (oldValueMonto != Monto && event.id == "txtMontoOportunidad" && Margen > 0) {
        Costo = (Monto > 0 && Margen > 0) ? Monto * ((100 - Margen) / 100) : Costo;
        $("#txtCosto").val(formato.moneda(Costo, '$'));
        console.log(Costo);
    } else if (oldValueMonto != Monto && event.id == "txtMontoOportunidad" && Costo > 0) {
        Margen = (Costo > 0 && Monto > 0) ? Math.round((Monto - Costo) * 100 / Monto) : Margen;
        $("#txtMargen").val(Margen);
        console.log(Margen);
    }

    if (oldValueMargen != Margen && event.id == "txtMargen" && Monto > 0) {
        Costo = (Monto > 0 && Margen > 0) ? Monto * ((100 - Margen) / 100) : Costo;
        $("#txtCosto").val(formato.moneda(Costo, '$'));
        console.log(Costo);
    } else if (oldValueMargen != Margen && event.id == "txtMargen" && Costo > 0) {
        Monto = (Margen > 0 && Costo > 0) ? Costo / ((100 - Margen) / 100) : Monto;
        $("#txtMontoOportunidad").val(formato.moneda(Monto, '$'));
        console.log(Monto);
    }

    if (oldValueCosto != Costo && event.id == "txtCosto" && Monto > 0) {
        Margen = (Costo > 0 && Monto > 0) ? Math.round((Monto - Costo) * 100 / Monto) : Margen;
        $("#txtMargen").val(Margen);
        console.log(Margen);
    } else if (oldValueCosto != Costo && event.id == "txtCosto" && Margen > 0) {
        Monto = (Margen > 0 && Costo > 0) ? Costo / ((100 - Margen) / 100) : Monto;
        $("#txtMontoOportunidad").val(formato.moneda(Monto, '$'));
        console.log(Monto);
    }

    costoUpDown();
}

/* FUNCION PARA IGUALAR MONTOS */
function igualarMonto() {
    var Monto = parseFloat($("#montoReal").val().replace("$", "").replace(/,/g, ""));
    $("#txtMontoOportunidad").val(formato.moneda(Monto, '$'));
    
    var Margen = parseFloat($("#margenReal").val());
    $("#txtMargen").val(Margen);

    var Costo = parseFloat($("#costoReal").val().replace("$", "").replace(/,/g, ""));
    $("#txtCosto").val(formato.moneda(Costo, '$'));
}

/* VALIDA SI LOS MONTOS SON IGUALES PARA COLOCAR ESTATUS CERRADA */
function validaMontos() {

    if ($("#cmbCerradaOportunidad").val() == 1) {
        var Monto = parseFloat($("#txtMontoOportunidad").val().replace("$", "").replace(/,/g, ""));
        var MontoReal = parseFloat($("#montoReal").val().replace("$", "").replace(/,/g, ""));

        if (Monto != MontoReal) {
            MostrarMensajeError("Los Montos deben de ser iguales.");
            $("#cmbCerradaOportunidad").val("0");
        }
    }

    costoUpDown();
}

/* VERIFICA SI EL COSTO ES MAYOR O MENOR AL COSTO REAL */
function costoUpDown() {
    var Costo = parseFloat($("#txtCosto").val().replace("$", "").replace(/,/g, ""));
    var CostoReal = parseFloat($("#costoReal").val().replace("$", "").replace(/,/g, ""));
    if (CostoReal > Costo) {
        $("#imgUpDownCosto").attr('src', '../Images/Red_Arrow_Up.png');
    } else {
        $("#imgUpDownCosto").attr('src', '../Images/Green_Arrow_Down.png');
    }
}
/////////////////////////////////////

function ObtenerDescripcionDivision(Division) {
    $.ajax({
        url: "Oportunidad.aspx/ObtenerDescripcionDivision",
        type: "post",
        data: Division,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                $("#iDivisionDescripcion").attr("title", json.Modelo.Descripcion);
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function AgregarOportunidad() {
    var pOportunidad = new Object();
    pOportunidad.pOportunidad = $("#txtOportunidad").val();
    pOportunidad.pIdCliente = $("#divFormaAgregarOportunidad").attr("idCliente");
    pOportunidad.pMonto = $("#txtMontoOportunidad").val().replace("$", "").replace(",", "");
    pOportunidad.pFechaCierre = $("#txtFechaCierre").val();
    pOportunidad.IdNivelInteresOportunidad = parseInt($("#cmbNivelInteresOportunidad").val());
    pOportunidad.pIdDivision = parseInt($("#cmbDivisionOportunidad").val());
    pOportunidad.pEsProyecto = parseInt($("#cmbEsProyecto").val());
    pOportunidad.pUrgente = parseInt($("#cmbUrgente").val());
    pOportunidad.pIdCampana = parseInt($("#cmbCampana").val());
    pOportunidad.pProveedores = $("#txtProveedores").val();
    pOportunidad.pUtilidad = parseInt($("#txtMargen").val());
    pOportunidad.pCosto = parseFloat($("#txtCosto").val().replace("$", "").replace(",", ""));
    var validacion = ValidarOportunidad(pOportunidad);
    if (validacion != "") {
        MostrarMensajeError(validacion);
        return false;
    }
    var oRequest = new Object();
    oRequest.pOportunidad = pOportunidad;
    SetAgregarOportunidad(JSON.stringify(oRequest));
}

function SetAgregarOportunidad(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/AgregarOportunidad",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdOportunidad").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogAgregarOportunidad").dialog("close");
        }
    });
}

function EditarOportunidad() {
    var pOportunidad = new Object();
    pOportunidad.pIdOportunidad = $("#divFormaEditarOportunidad").attr("idOportunidad");
    pOportunidad.pOportunidad = $("#txtOportunidad").val();
    pOportunidad.pIdCliente = $("#divFormaEditarOportunidad").attr("idCliente");
    pOportunidad.pIdUsuario = $("#divFormaEditarOportunidad").attr("idUsuario");
    pOportunidad.pMonto = QuitaFormatoMoneda($("#txtMontoOportunidad").val().replace("$", "").replace(",", ""));
    pOportunidad.pFechaCierre = $("#txtFechaCierre").val();
    pOportunidad.IdNivelInteresOportunidad = $("#cmbNivelInteresOportunidad").val();
    pOportunidad.pClasificacion = parseInt($("#cmbClasificacionOportunidad").val());
    pOportunidad.pDivision = parseInt($("#cmbDivisionOportunidad").val());
    pOportunidad.pCampana = parseInt($("#cmbCampana").val());
    pOportunidad.pCerrada = parseInt($("#cmbCerradaOportunidad").val());
    pOportunidad.pEsProyecto = parseInt($("#cmbEsProyectoOportunidad").val());
    pOportunidad.pUrgente = parseInt($("#cmbUrgenteOportunidad").val());
    pOportunidad.pProveedores = $("#txtProveedores").val();
    pOportunidad.pUtilidad = parseInt(QuitaFormatoMoneda($("#txtMargen").val()));
    pOportunidad.pCosto = parseFloat(QuitaFormatoMoneda($("#txtCosto").val().replace("$", "").replace(",", "")));
    var validacion = ValidarOportunidad(pOportunidad);
    if (validacion != "") {
        MostrarMensajeError(validacion);
        return false;
    }
    var oRequest = new Object();
    oRequest.pOportunidad = pOportunidad;
    SetEditarOportunidad(JSON.stringify(oRequest));
    //$("#dialogEditarOportunidad").dialog("close");
}

function SetEditarOportunidad(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/EditarOportunidad",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdPlanVentas").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            MostrarMensajeError("Se ha guardado con éxito.");
            //$("#dialogEditarOportunidad").dialog("close"); 
        }
    });
}

function ObtenerReporteAutorizadosVendedor() {

}

// Add and Read Commit
function GuardarComentario() {

    if ($("#addComentario").val() == ""){ 
        MostrarMensajeError("Favor de poner comentario previamente.");
    } else {
        MostrarBloqueo();
        var pComentario = new Object();
        pComentario.pComentario = $("#addComentario").val();
        pComentario.pIdOportunidad = parseInt($("#divFormaEditarOportunidad").attr("idOportunidad"));
        var pRequest = JSON.stringify(pComentario);

        $.ajax({
            type: "POST",
            url: "Oportunidad.aspx/AgregarComentarioOportunidad",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (pRespuesta) {
                var json = JSON.parse(pRespuesta.d);
                if (json.Error == 0) {
                    $("#commit").empty();
                    $("#addComentario").val("");
                    var str = '';
                    $.each(json, function (k, v) {
                        if (k == 'Modelo')
                            $.each(v, function (a, b) {
                                $.each(b, function (x, y) {
                                    str += '<div class="container">'
                                        + '   <h3>' + b[x].Usuario + ' - ' + b[x].Area + '</h3>'
                                        + '   <p>' + b[x].Comentario + '</p>'
                                        + '   <span class="time-right">' + b[x].Fecha + '</span>'
                                        + ' </div>';
                                });
                            });
                    });
                    $("#commit").append(str);
                    $("#commit").scrollTop($("#commit")[0].scrollHeight);
                }
                else {
                    MostrarMensajeError(json.Descripcion);
                }
            },
            complete: function () {
                OcultarBloqueo();
            }
        });
    }
}

function AutocompletarUsuario() {
    $("#txtUsuarioOportunidad").autocomplete({
        source: function (request, response) {
            var Usuario = new Object();
            Usuario.pUsuario = request.term;
            var pRequest = JSON.stringify(Usuario);
            $.ajax({
                type: "POST",
                url: "Oportunidad.aspx/ObtenerUsuariosAsignar",
                data: pRequest,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (oRespuesta) {
                    var json = jQuery.parseJSON(oRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.Usuario, value: item.Usuario, id: item.IdUsuario }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var IdUsuario = ui.item.id;
            $("#divFormaEditarOportunidad").attr("idUsuario", IdUsuario);
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarClienteOportunidad() {
    $('#txtClienteOportunidad').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pCliente = $("#txtClienteOportunidad").val();
            $.ajax({
                type: 'POST',
                url: 'Oportunidad.aspx/BuscarCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.Cliente, value: item.Cliente, id: item.IdCliente, Saldo: item.Saldo, CondicionPago: item.CondicionPago }
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            var pIdCliente = ui.item.id;
            var Saldo = ui.item.Saldo;
            var CondicionPago = ui.item.CondicionPago;
            $("#divFormaAgregarOportunidad, #divFormaEditarOportunidad").attr("idCliente", pIdCliente);
            $("#lvlSaldo").text(formato.moneda(Saldo, '$'));
            $("#txtCondicionPago").text(CondicionPago);
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    }).change(function () { $("#lvlSaldo").text('');  $("#txtCondicionPago").text('');});
}

function CalculoUtilidad() {
    var Monto = parseFloat($("#txtMontoOportunidad").val().replace("$", "").replace(/,/g, ""));
    var Margen = parseInt($("#txtMargen").val());
    var Costo = parseFloat($("#txtCosto").val().replace("$", "").replace(/,/g, ""));

    Monto = (!isNaN(Monto) && isFinite(Monto)) ? Monto : 0;
    Margen = (!isNaN(Margen) && isFinite(Margen)) ? Margen : 0;
    Costo = (!isNaN(Costo) && isFinite(Costo)) ? Costo : 0;

    Monto = (Monto == 0 && Margen > 0 && Costo > 0) ? Costo / ((100 - Margen) / 100) : Monto;
    Margen = (Margen == 0 && Costo > 0 && Monto > 0) ? Math.round((Monto - Costo) * 100 / Monto) : Margen;
    Costo = (Costo == 0 && Monto > 0 && Margen > 0) ? Monto * ((100 - Margen) / 100) : Costo;

    $("#txtMontoOportunidad").val(formato.moneda(Monto, '$'));
    $("#txtMargen").val(Margen);
    $("#txtCosto").val(formato.moneda(Costo, '$'));
}

function ValidarOportunidad(pOportunidad) {

    var error = "";
    if (pOportunidad.pOportunidad == "") {
        error += '<span>*</span> El campo de oportunidad esta vacio, favor de completarlo.<br/>';
    }
    if (pOportunidad.pIdCliente == "" || pOportunidad.pIdCliente == 0 || pOportunidad.pIdCliente == null || pOportunidad.pIdCliente == undefined) {
        error += '<span>*</span> Favor de selecionar el cliente de la oportunidad.<br/>';
    }
    if (pOportunidad.pMonto == "") {
        error += '<span>*</span> El campo del monto de la oprotunidad esta vacio, favor de completarlo.<br/>';
    }
    if (isNaN(pOportunidad.pMonto)) {
        error += '<span>*</span> El monto debe ser numerico y no puede llevar signos ni comas.<br/>';
    }
    if (pOportunidad.pIdNivelInteresOportunidad == "") {
        error += '<span>*</span> Favor de selecionar el nivel de interés de la oportunidad.<br/>';
    }
    if (pOportunidad.pUrgente == 1 && pOportunidad.IdNivelInteresOportunidad != 1) {
        error += '<span>*</span> Únicamente las oportunidades con nivel de interes alto pueden ser urgentes.<br/>';
    }
    if (error != "") {
        error = '<p>Favor de completar los siguientes requisitos:</p>' + error;
    }
    return error;
}

function AutorizarOportunidad(check) {
    var Oportunidad = new Object();
    Oportunidad.pIdOportunidad = parseInt($(check).parent('tr').children('td[aria-describedby=grdPlanVentas_IdOportunidad]').text())
    Oportunidad.pAutorizado = ($('.autorizado',check).prop("checked"))?1:0;
    var pRequest = JSON.stringify(Oportunidad);
    console.log(pRequest);
    $.ajax({
        url: "PlaneacionVentas.aspx/AutorizarOportunidad",
        type: "POST",
        data: pRequest,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                $("#grdPlanVentas").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
    
}

function ProyectoPedidoAutorizados() {
	var Oportunidad = new Object();
	Oportunidad.Agente = $("#gs_Agente").val();
	Oportunidad.IdSucursal = $("#gs_Sucursal").val();
	var Request = JSON.stringify(Oportunidad);
	$.ajax({
		type: "post",
		url: "PlaneacionVentas.aspx/ProyectosPedidosAutorizados",
		data: Request,
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				$("#proyectosAutorizados").text(json.Modelo.ProyectosAutorizados);
				$("#proyectosSinAutorizar").text(json.Modelo.ProyectosNoAutorizados);
				$("#pedidosAutorizado").text(json.Modelo.PedidosAutorizados);
				$("#pedidosSinAutorizar").text(json.Modelo.PedidosNoAutorizados);
				$("#totalAutorizado").text(json.Modelo.TotalAutorizados);
                $("#totaSinAutorizar").text(json.Modelo.TotalNoAutorizados);

                var facturado = parseFloat($("#facturado").text().replace("$", "").replace(/,/g, ""));
                var totalAutorizado = parseFloat($("#totalAutorizado").text().replace("$", "").replace(/,/g, ""));
                var totalSinAutorizado = parseFloat($("#totaSinAutorizar").text().replace("$", "").replace(/,/g, ""));

                var paraCerrar = facturado + totalAutorizado;
                var planCierre = paraCerrar + totalSinAutorizado;

                paraCerrar = formato.moneda(paraCerrar, '$');
                planCierre = formato.moneda(planCierre, '$');
                
                $("#paraCierre").text(paraCerrar);
                $("#planCierre").text(planCierre);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ReporteOrdenesCompras() {
	window.location = "../ExportacionesExcel/ExportarExcelReporteComprasOportunidad.aspx";
}