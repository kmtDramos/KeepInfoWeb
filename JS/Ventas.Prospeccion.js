/**/


$(function () {

    MantenerSesion();

    setTimeout(MantenerSesion, 1000 * 60);

    $("#txtFechaInicial").datepicker();

    $("#txtFechaFinal").datepicker();

    ObtenerTablaProspeccion();

    $("#btnAgregarFilaProspeccion").click(ObtenerAgregarFilaProspeccion);

    ObtenerUsuarios();

    $("#btnActualizarProspeccion").click(function (event) {
        ObtenerProspeccionPorUsuario();
    });

});

function ObtenerProspeccionPorUsuario() {
    MostrarBloqueo();

    var Usuario = new Object();
    Usuario.IdUsuario = parseInt($("#cmbUsuario").val());
    Usuario.FechaInicio = $("#txtFechaInicial").val();
    Usuario.FechaFin = $("#txtFechaFinal").val();

    var Request = JSON.stringify(Usuario);

    $("#divTablaProspeccion").obtenerVista({
        url: "Prospeccion.aspx/ObtenerTablaProspeccionPorUsuario",
        parametros: Request,
        nombreTemplate: "tmplTablaProspeccion.html",
        despuesDeCompilar: function () {
            $("input", "#tblProspeccion").change(function () {
                var fila = $(this).parent("td").parent("tr");
                GuardarFila(fila);
            });
            $('input[type=text]', "#tblProspeccion").each(function (index, element) {
                $(element).autocomplete({
                    source: function (request, response) {
                        var Cliente = new Object();
                        Cliente.pCliente = $(element).val();

                        var Request = JSON.stringify(Cliente);
                        $.ajax({
                            url: 'Prospeccion.aspx/BuscarCliente',
                            type: 'POST',
                            data: Request,
                            dataType: 'json',
                            contentType: 'application/json; charset=utf-8',
                            success: function (pRespuesta) {
                                var json = jQuery.parseJSON(pRespuesta.d);
                                response($.map(json.Table, function (item) {
                                    return { label: item.Cliente, value: item.Cliente, id: item.IdCliente }
                                }));
                            }
                        });
                    },
                    minLength: 2,
                    select: function (event, ui) {
                    },
                    focus: function (event, ui) {
                    },
                    change: function (event, ui) { },
                    open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
                    close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
                });
            });

            Totales();
            OcultarBloqueo();
        }
    });
}

function ObtenerUsuarios() {
    $.ajax({
        url: "Prospeccion.aspx/ObtenerUsuarios",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                var Usuarios = json.Modelo.Usuarios;

                for (x in Usuarios) {
                    $("#cmbUsuario").append($("<option value='" + Usuarios[x].Valor + "'>" + Usuarios[x].Nombre + "</option>"));
                }
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function openMail(value) {
    var fila = $(value).parent("td").parent("tr");
    var email = $("input[name=Correo]", fila).val();
   
    if (email != "") {
        if (validateEmail(email)) {
            var mailto_link = 'mailto:' + email;
            win = window.open(mailto_link, 'emailWindow');
            if (win && win.open && !win.closed) win.close();
        } else {
            MostrarMensajeError("El Correo no es Válido");
        }
    }
}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function ObtenerTablaProspeccion() {
    
    $("#divTablaProspeccion").obtenerVista({
        url: "Prospeccion.aspx/ObtenerTablaProspeccion",
        nombreTemplate: "tmplTablaProspeccion.html",
        despuesDeCompilar: function () {
            $("input", "#tblProspeccion").change(function () {
                var fila = $(this).parent("td").parent("tr");
                GuardarFila(fila);
            });
            $('input[type=text]', "#tblProspeccion").each(function (index, element) {
                $(element).autocomplete({
                    source: function (request, response) {
                        var Cliente = new Object();
                        Cliente.pCliente = $(element).val();

                        var Request = JSON.stringify(Cliente);
                        $.ajax({
                            url: 'Prospeccion.aspx/BuscarCliente',
                            type: 'POST',
                            data: Request,
                            dataType: 'json',
                            contentType: 'application/json; charset=utf-8',
                            success: function (pRespuesta) {
                                var json = jQuery.parseJSON(pRespuesta.d);
                                response($.map(json.Table, function (item) {
                                    return { label: item.Cliente, value: item.Cliente, id: item.IdCliente }
                                }));
                            }
                        });
                    },
                    minLength: 2,
                    select: function (event, ui) {
                    },
                    focus: function (event, ui) {
                    },
                    change: function (event, ui) { },
                    open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
                    close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
                });
            });
            
            Totales();
        }
    });
}

function ObtenerAgregarFilaProspeccion() {
    var tr = $('<tr class="fila" IdProspeccion="0"></tr>');
    $(tr).obtenerVista({
        url: "Prospeccion.aspx/ObtenerAgregarFilaProspeccion",
        nombreTemplate: "tmplFilaProspeccion.html",
        despuesDeCompilar: function () {
            $("tbody", "#tblProspeccion").append(tr);
            $("input", tr).change(function () {
                GuardarFila(tr);
            });

            $('input[type=text]', tr).autocomplete({
                source: function (request, response) {
                    var Cliente = new Object();
                    Cliente.pCliente = $('input[type=text]', tr).val();

                    var Request = JSON.stringify(Cliente);
                    $.ajax({
                        url: 'Prospeccion.aspx/BuscarCliente',
                        type: 'POST',
                        data: Request,
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (pRespuesta) {
                            var json = jQuery.parseJSON(pRespuesta.d);
                            response($.map(json.Table, function (item) {
                                return { label: item.Cliente, value: item.Cliente, id: item.IdCliente }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                },
                focus: function (event, ui) {
                },
                change: function (event, ui) { },
                open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
                close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
            });
        }
    });
}

function GuardarFila(fila) {
    var Prospeccion = new Object();
    Prospeccion.IdProspeccion = parseInt($(fila).attr("IdProspeccion"));
    Prospeccion.Cliente = $("input[name=Cliente]", fila).val();
    Prospeccion.Correo = $("input[name=Correo]", fila).val();
    Prospeccion.Nombre = $("input[name=Nombre]", fila).val();
    Prospeccion.Telefono = $("input[name=Telefono]", fila).val();

    var EstatusProspeccion = [];
    $("input[type=checkbox]", fila).each(function (index, element) {
        var Estatus = new Object();
        Estatus.IdEstatusProspeccion = parseInt($(element).val());
        Estatus.Baja = !$(element).is(":checked");
        EstatusProspeccion.push(Estatus);
    });

    if (EstatusProspeccion[9].Baja == false && EstatusProspeccion[10].Baja == false) {
        // MostrarMensajeError("Solo puede ser Cerrada Ganada o Cerrada Perdida.");

    }
    Prospeccion.EstatusProspeccion = EstatusProspeccion;

    var Request = JSON.stringify(Prospeccion);

    $.ajax({
        url: "Prospeccion.aspx/GuardarProspeccion",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                $(fila).attr("IdProspeccion", json.Modelo.IdProspeccion);
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}

function EliminarFila(element) {
    var ventana = $('<div><p>¿Deseas eliminar la prospección?</p></div>');
    $(ventana).dialog({
        modal: true,
        draggable: false,
        resizable: false,
        close: function () {
            $(ventana).remove();
        },
        buttons: {
            "Eliminar": function () {
                var Prospeccion = new Object();
                var fila = $(element).parent("td").parent("tr");

                Prospeccion.IdProspeccion = parseInt($(fila).attr("IdProspeccion"));

                var Request = JSON.stringify(Prospeccion);

                $.ajax({
                    url: "Prospeccion.aspx/EliminarProspeccion",
                    type: "post",
                    data: Request,
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (Respuesta) {
                        var json = JSON.parse(Respuesta.d);
                        if (json.Error == 0) {
                            $(fila).remove();
                        }
                        else {
                            MostrarMensajeError(json.Descripcion);
                        }
                    }
                });
                $(ventana).dialog("close");
            },
            "Cancelar": function () {
                $(ventana).dialog("close");
            }
        }
    });
}

function Totales() {

    var Usuario = new Object();
    Usuario.IdUsuario = parseInt($("#cmbUsuario").val());
    Usuario.FechaInicial = $("#txtFechaInicial").val();
    Usuario.FechaFinal = $("#txtFechaFinal").val();
    var Request = JSON.stringify(Usuario);

    $.ajax({
        url: "Prospeccion.aspx/Totales",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                if (json.Modelo.Totales.length != 0) {
                    $("#totalProspectos").text(json.Modelo.Totales[0].TotalProspectos);
                    $("#diasPromedio").text(json.Modelo.Totales[0].DiasPromedio);
                    $("#ganadas").text(json.Modelo.Totales[0].Ganados);
                    $("#perdidas").text(json.Modelo.Totales[0].Perdidos);
                } else {
                    $("#totalProspectos").text("0");
                    $("#diasPromedio").text("0");
                    $("#ganadas").text("0");
                    $("#perdidas").text("0");
                }
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });
}