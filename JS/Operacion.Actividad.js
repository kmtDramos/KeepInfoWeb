
$(function() {

    var dialogActividad = $("<div id='dialogActividad' title='Agregar Actividad'></div>");

    var dialogEditarActividad = $("<div id='dialogEditarActividad' title='Editar Actividad'></div>");

    var contenido = "<div id='dialogAgenda' title='Agenda Actividades'>" +
                        "<select id='cmbIdUsuarioAgenda'><option value='-1'>[]</option></select>" +
                        " <input type='button' value='Agregar actividad' class='buttonLTR btnAbrirFormaAgregarActividad'/>" +
                        "<hr />" +
                        "<div id='agendaActividades'></div>" +
                    "</div>";
    var dialogAgenda = $(contenido);

    $("body").append(dialogActividad);
    $("body").append(dialogAgenda);

    $(dialogActividad).dialog({
        modal: true,
        width: "auto",
        draggable: false,
        resizable: false,
        autoOpen: false,
        close: function() { $("#divAgregarActividad").remove(); },
        buttons: {
            "Agregar": function() { AgregarActividad(); },
            "Cancelar": function() { $(this).dialog("close"); }
        }
    });

    $(dialogEditarActividad).dialog({
        modal: true,
        width: "auto",
        draggable: false,
        resizable: false,
        autoOpen: false,
        close: function() { $("#divEditarActividad").remove(); },
        open: function() { initObjetos(this) },
        buttons: {
            "Editar": function() { EditarActividad(); },
            "Cancelar": function() { $(this).dialog("close"); }
        }
    });

    $(dialogAgenda).dialog({
        modal: true,
        width: 1000,
        height: 600,
        draggable: false,
        resizable: false,
        autoOpen: false,
        open: function() {
            ObtenerUsuarios();
            CrearAgenda($("#agendaActividades"));
            $("#cmbIdUsuarioAgenda").change(function() {
                $("#agendaActividades").fullCalendar('refetchEvents');
            });
        },
        close: function() { $("#agendaActividades").fullCalendar('destroy'); },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $("body").one("click",".btnAbrirFormaAgregarActividad",function() {
        ObtenerFormaAgregarActividad(0, 0);
    });

    $(".btnAbrirAgendaActividades").click(function() {
        $(dialogAgenda).dialog("open");
    });

    //
    var contenedor = $('<div id="dialog_grdActividadesClienteOportunidad" class="divContGrid renglon-bottom" idCliente="0" idOportunidad="0" idTipoActividad="0">' +
                            '<input type="button" value="Agregar actividad" class="buttonLTR btnAbrirFormaAgregarActividad"/>' +
                            '<hr/>' +
                            '<div id="divContGrid">' +
                                '<table id="grdActividadesClienteOportunidad"></table>' +
                                '<div id="pagActividadesClienteOportunidad"></div>' +
                            '</div>' +
                        '</div>');
    $("body").append(contenedor);
    $(contenedor).dialog({
        modal: true,
        width: 730,
        draggable: false,
        resizable: false,
        autoOpen: false,
        open: function() {
            var formaActividad = this;
            FiltroActividadesClienteOportunidad();

            $(".btnAbrirFormaAgregarActividad").unbind("click").click(function() {
                var IdCliente = parseInt($("#dialog_grdActividadesClienteOportunidad").attr("idCliente"));
                var IdOportunidad = parseInt($("#dialog_grdActividadesClienteOportunidad").attr("idOportunidad"));
                ObtenerFormaAgregarActividad(IdCliente, IdOportunidad);
            });
        }
    });

    $("#grdActividadesClienteOportunidad").on("click", "td[aria-describedby=grdActividadesClienteOportunidad_IdOportunidad]", function() {
        var IdOportunidad = parseInt($(this).html());
        var IdCliente = parseInt($("#dialog_grdActividadesClienteOportunidad").attr("idCliente"));
        ObtenerFormaAgregarActividad(IdCliente, IdOportunidad);
    });

});

function CrearAgenda(agenda) {
	if (!$(agenda).hasClass("fc")) {
		$(agenda).fullCalendar({
			header: { left: 'prev,next today', center: 'title', right: 'agendaWeek,month,agendaDay' },
			defaultView: 'agendaWeek',
			height: 450,
			hiddenDays: [0],
			businessHours: { start: '08:30', end: '18:30', dow: [1, 2, 3, 4, 5, 6] },
			allDaySlot: true,
			slotDuration: '00:15:00',
			lang: 'es',
			minTime: '07:00',
			events: { url: "Agenda.aspx", data: function() { return { IdUsuario: parseInt($('#cmbIdUsuarioAgenda').val()) }; } },
			eventRender: function(event, element) {
				$(element).click(function(evt) {
					AbrirActividad(event);
				}).css("cursor", "pointer");
			},
			dayClick: function() {
				var date = arguments[0]._d;
				fecha = new Date(date);
				fecha = new Date(fecha.valueOf() + (1000 * 60 * 60 * 5));
				console.log(fecha);
				ObtenerFormaAgregarActividadFecha(fecha);
			}
		});
	} else {
		$(agenda).fullCalendar("refetchEvents");
	}
}

function AbrirActividad(evento) {
    var actividad = JSON.stringify(evento);
    $("<div class='dialogActividadDescripcion' data-actividad='" + actividad + "'>" + evento.description + "</div>").dialog({
    	title: evento.dialogTitle,
    	modal: true,
    	resizable: false,
    	draggable: false,
    	close: function() {
        	$("#agendaActividades").fullCalendar('refetchEvents');
    	},
    	buttons: {
    		"Realizar": function() { // Luis Lauro
    			RealizarActividad(evento.id);
    		},
    		"Editar": function() {
    			ObtenerFormaEditarActividad(evento.id);
    			$(this).dialog("close");
    		},
    		"Cerrar": function() {
    			$(this).dialog("close");
    		}
    	}
    });
}

function RealizarActividad(IdActividad) {
    
}

function ObtenerFormaEditarActividad(IdActividad) {
    var Actividad = new Object();
    Actividad.IdActividad = IdActividad;
    
    var Request = JSON.stringify(Actividad);

    $("#dialogEditarActividad").obtenerVista({
        url: "Actividad.aspx/ObtenerFormaEditarActividad",
        parametros: Request,
        nombreTemplate: "tmplEditarActividad.html",
        despuesDeCompilar: function(Respuesta) {
            $("#dialogEditarActividad").dialog("open");
        }
    });
}

function EditarActividad() {
    var Actividad = ObtenerDatosEditarActividad();
    Actividad.IdActividad = parseInt($("#divEditarActividad").attr("idActividad"));

    var validar = ValidarActividad(Actividad);

    if (validar == "") {
        $.ajax({
            url: "Actividad.aspx/EditarActividad",
            type: "post",
            data: JSON.stringify(Actividad),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(Respuesta) {
                var json = JSON.parse(Respuesta.d);
                if (json.Error == 0) {
                    $("#dialogEditarActividad").dialog("close");
                    $("#agendaActividades").fullCalendar("refetchEvents");
                    FiltroActividadesClienteOportunidad();
                    FiltroCliente();
                    FiltroOportunidad();
                } else {
                    MostrarMensajeError(json.Descripcion);
                }
            }
        });
    } else {
        MostrarMensajeError(validar);
    }
    
}

function RealizarActividad(IdActividad) {
    $.ajax({
        url:""
    });
}

function AutocompletarClienteActividad() {
    $('#txtClienteActividad').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pCliente = $("#txtClienteActividad").val();
            $.ajax({
                type: 'POST',
                url: 'Actividad.aspx/BuscarCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) { return { label: item.Cliente, value: item.Cliente, id: item.IdCliente} }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) { var pIdCliente = ui.item.id; $(this).attr("idCliente", pIdCliente); SeleccionarClienteActividad(); },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function ObtenerFormaAgregarActividad() {
    var Actividad = new Object();
    Actividad.IdCliente = (arguments[0] != "undefined") ? arguments[0] : 0;
    Actividad.IdOportunidad = (arguments[1] != "undefined") ? arguments[1] : 0;

    $("#dialogActividad").obtenerVista({
        url: "Actividad.aspx/ObtenerFormaAgregarActividad",
        parametros: JSON.stringify(Actividad),
        nombreTemplate: "tmplAgregarActividad.html",
        despuesDeCompilar: function(Modelo) {
            initObjetos($("#dialogActividad"));
            $("#dialogActividad").dialog("open");
        }
    });
}

function ObtenerFormaAgregarActividadFecha(date) {
    var Actividad = new Object();
    Actividad.IdCliente = 0;
    Actividad.IdOportunidad = 0;

    $("#dialogActividad").obtenerVista({
        url: "Actividad.aspx/ObtenerFormaAgregarActividad",
        parametros: JSON.stringify(Actividad),
        nombreTemplate: "tmplAgregarActividad.html",
        despuesDeCompilar: function(Modelo) {
            initObjetos($("#dialogActividad"));
            $("#txtFechaInicioActividad", "#dialogActividad").val(date.FechaHora()).change();
            $("#txtFechaInicioActividad", "#dialogActividad").val(date.FechaHora()).change();
            $("#dialogActividad").dialog("open");
        }
    });
}

function initObjetos(formaActividad) {

    $("#txtFechaInicioActividad", formaActividad).datetimepicker({
        dateFormat: "dd/mm/yy",
        timeFormat: "hh:mm tt"
    }).change(function() {
        var fecha = $(this).datetimepicker("getDate");
        fecha = new Date(fecha.valueOf() + (1000 * 60 * 15));
        $("#txtFechaFinActividad", formaActividad).datetimepicker("setDate", fecha).change();
    });

    $("#txtFechaFinActividad", formaActividad).datetimepicker({
        dateFormat: "dd/mm/yy",
        timeFormat: "hh:mm tt"
    });
    
    AutocompletarClienteActividad();
}

function SeleccionarClienteActividad() {
    ObtenerOportunidadesClienteActividad();
}

function ObtenerOportunidadesClienteActividad() {
    var Cliente = new Object();
    Cliente.IdCliente = parseInt($("#txtClienteActividad").attr("idCliente"));
    var Request = JSON.stringify(Cliente);
    $.ajax({
        url: "Actividad.aspx/ObtenerOportunidadesClienteActividad",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                CargarOportunidadesClienteActividad(json.Modelo.Oportunidades);
            } else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function CargarOportunidadesClienteActividad(Oportunidades){
    var comboOportunidades = $("#cmbIdOportunidadActividad");
    $(comboOportunidades).html('');
    var opcionInicial = $('<option value="">Elegir una opción...</option>');
    $(comboOportunidades).append(opcionInicial);
    for(x in Oportunidades){
        var Id = Oportunidades[x].Valor;
        var Descripcion = Oportunidades[x].Descripcion
        var optHTML = '<option value="' + Id + '">' + Descripcion + ' (' + Id + ')' + '</option>';
        $(comboOportunidades).append($(optHTML));
    }
}

function ObtenerDatosActividad() {
    var Actividad = new Object();
    Actividad.IdTipoActividad = ($("#cmbTipoActividad", "#dialogActividad").val() != "") ? parseInt($("#cmbTipoActividad", "#dialogActividad").val()) : 0;
    Actividad.FechaActividad = ($("#txtFechaInicioActividad", "#dialogActividad").val() != "") ? $("#txtFechaInicioActividad", "#dialogActividad").val() : "";
    Actividad.FechaFin = ($("#txtFechaFinActividad", "#dialogActividad").val() != "") ? $("#txtFechaFinActividad", "#dialogActividad").val() : "";
    Actividad.IdCliente = ($("#txtClienteActividad", "#dialogActividad").attr("idCliente") != "0") ? parseInt($("#txtClienteActividad", "#dialogActividad").attr("idCliente")) : 0;
    Actividad.Cliente = ($("#txtClienteActividad", "#dialogActividad").val() != "") ? $("#txtClienteActividad", "#dialogActividad").val() : "";
    Actividad.IdOportunidad = ($("#cmbIdOportunidadActividad", "#dialogActividad").val() != "") ? parseInt($("#cmbIdOportunidadActividad", "#dialogActividad").val()) : 0;
    Actividad.Actividad = ($("#txtActividad", "#dialogActividad").val() != "") ? $("#txtActividad", "#dialogActividad").val() : "";
    Actividad.Ubicacion = ($("#txtUbicacion","#dialogActividad").val() != null)? $("#txtUbicacion","#dialogActividad").val() : "";
    console.log(Actividad);
    return Actividad;
}

function ObtenerDatosEditarActividad() {
    var Actividad = new Object();
    Actividad.IdTipoActividad = ($("#cmbTipoActividad", "#dialogEditarActividad").val() != "") ? parseInt($("#cmbTipoActividad", "#dialogEditarActividad").val()) : 0;
    Actividad.FechaActividad = ($("#txtFechaInicioActividad", "#dialogEditarActividad").val() != "") ? $("#txtFechaInicioActividad", "#dialogEditarActividad").val() : "";
    Actividad.FechaFin = ($("#txtFechaFinActividad", "#dialogEditarActividad").val() != "") ? $("#txtFechaFinActividad", "#dialogEditarActividad").val() : "";
    Actividad.IdCliente = ($("#txtClienteActividad", "#dialogEditarActividad").attr("idCliente") != "0") ? parseInt($("#txtClienteActividad", "#dialogEditarActividad").attr("idCliente")) : 0;
    Actividad.Cliente = ($("#txtClienteActividad", "#dialogEditarActividad").val() != "") ? $("#txtClienteActividad", "#dialogEditarActividad").val() : "";
    Actividad.IdOportunidad = ($("#cmbIdOportunidadActividad", "#dialogEditarActividad").val() != "") ? parseInt($("#cmbIdOportunidadActividad", "#dialogEditarActividad").val()) : 0;
    Actividad.Actividad = ($("#txtActividad", "#dialogEditarActividad").val() != "") ? $("#txtActividad", "#dialogEditarActividad").val() : "";
    console.log(Actividad);
    return Actividad;
}

function AgregarActividad() {
    var Actividad = ObtenerDatosActividad();
    var Valido = ValidarActividad(Actividad);
    if (Valido == "") {
        SetAgregarActividad(Actividad);
        $("#agendaActividades").fullCalendar("refetchEvents");
    } else {
        MostrarMensajeError(Valido);
    }
}

function SetAgregarActividad(Actividad) {
    var Request = JSON.stringify(Actividad);
    $.ajax({
        url: "Actividad.aspx/AgregarActividad",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                $("#dialogActividad").dialog("close");
                FiltroActividadesClienteOportunidad();
                FiltroCliente();
                FiltroOportunidad();
            } else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function ObtenerUsuarios() {
    $.ajax({
        url: "Actividad.aspx/ObtenerListaUsuarios",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                CargarUsuariosAgenda(json.Modelo.Usuarios);
            } else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function CargarUsuariosAgenda(Usuarios) {
    $("#cmbIdUsuarioAgenda").html('');
    for (x in Usuarios) {
        var opcion = document.createElement("option");
        opcion.innerHTML = Usuarios[x].Descripcion;
        opcion.value = Usuarios[x].Valor;
        $("#cmbIdUsuarioAgenda").append(opcion);
    }
}

function ValidarActividad(Actividad) {
    var valido = "";
    
    if (Actividad.IdTipoActividad == 0)
    { valido += "<li>Seleccionar el tipo de actividad.</li>"; }

    if (Actividad.FechaActividad == "")
    { valido += "<li>Seleccionar la fecha deinicio la actividad.</li>"; }

    if (Actividad.FechaFin == "")
    { valido += "<li>Seleccionar la fecha fin de la actividad.</li>"; }
    
    if (Actividad.Cliente == "")
    { valido += "<li>Ingresar el cliente o prospecto.</li>"; }
    
    if (Actividad.Actividad == "")
    { valido += "<li>Ingresar la actividad a realizar.</li>"; }
    
    if (valido != "")
    { valido = "Favor de completar los siguientes campos:<ul>" + valido + "</ul>"; }
    
    return valido;
}

function MostrarAvisos() {
    $.ajax({
        url: "Actividad.aspx/ActividadesProximas",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: ActividadesProximas
    });
}

function CompletarActividad(IdActividad) {
    var Actividad = new Object();
    Actividad.IdActividad = Actividad;
    $.ajax({
        url: "",
        type: "post",
        data: JSON.stringify(Actividad),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function() {
            
        }
    });
}

function ActividadesProximas(actividades) {
    if (actividades.length > 0) {
        $().dialog();
    }
}

function MiniAviso(actividad) {

}

//
function FiltroActividadesClienteOportunidad() {
    var Actividad = new Object();
    Actividad.pIdCliente = parseInt($("#dialog_grdActividadesClienteOportunidad").attr("idCliente"));
    Actividad.pIdOportunidad = parseInt($("#dialog_grdActividadesClienteOportunidad").attr("idOportunidad"));
    Actividad.pIdTipoActividad = parseInt($("#dialog_grdActividadesClienteOportunidad").attr("idTipoActividad"));
    Actividad.pIdTipoActividad = ($("#gs_TipoActividad").val() != null) ? parseInt($("#gs_TipoActividad").val()) : Actividad.pIdTipoActividad;
    Actividad.pTamanoPaginacion = parseInt($('#grdActividadesClienteOportunidad').getGridParam('rowNum'));
    Actividad.pPaginaActual = parseInt($('#grdActividadesClienteOportunidad').getGridParam('page'));
    Actividad.pColumnaOrden = $('#grdActividadesClienteOportunidad').getGridParam('sortname');
    Actividad.pTipoOrden = $('#grdActividadesClienteOportunidad').getGridParam('sortorder');
    
    $.ajax({
        url: 'Actividad.aspx/ObtenerActividadesClienteOportunidad',
        data: JSON.stringify(Actividad),
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdActividadesClienteOportunidad')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

Date.prototype.FechaHora = function() {
    var D = this.getDate().DD();
    var M = (this.getMonth()+1).DD();
    var Y = this.getFullYear();
    var h = (this.getHours() > 12) ? (this.getHours() - 12).DD() : this.getHours().DD();
    var m = this.getMinutes().DD();
    var g = (this.getHours() > 12) ? "pm" : "am";
    var fecha = D + "/" + M + "/" + Y + " " + h + ":" + m + " " + g;
    return fecha;
}

Number.prototype.DD = function() {
    return (this < 10) ? "0"+this: this;
}