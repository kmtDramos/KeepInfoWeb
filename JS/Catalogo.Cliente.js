//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("Cliente");
    });

    $("#ckbVerTodosClinetes").change(function(e) {
        FiltroCliente();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCliente", function() {
        var pIdOrganizacion = 0;
        var Organizacion = new Object();
        Organizacion.pIdOrganizacion = pIdOrganizacion;
        ObtenerFormaAgregarCliente(JSON.stringify(Organizacion));
    });

    $("#grdCliente").on("click", "td[aria-describedby=grdCliente_Actividades]", function(event) {
        var registro = $(this).parents("tr");
        var pIdCliente = $(registro).children("td[aria-describedby='grdCliente_IdCliente']").html();
        $("#dialog_grdActividadesClienteOportunidad").attr("idCliente", pIdCliente);
        $("#dialog_grdActividadesClienteOportunidad").dialog("option", "title", "Actividades")
        $("#dialog_grdActividadesClienteOportunidad").dialog("open");
    });

    $("#grdCliente").on("click", "td[aria-describedby=grdCliente_Oportunidad]", function(event) {
        var registro = $(this).parents("tr");
        var pIdCliente = $(registro).children("td[aria-describedby='grdCliente_IdCliente']").html();

        var Cliente = new Object();
        Cliente.IdCliente = pIdCliente;
        ObtenerGridOportunidadesCliente(Cliente);
    });

    $("#grdCliente").on("click", ".imgFormaDirecciones", function() {
        var registro = $(this).parents("tr");
        var Cliente = new Object();
        Cliente.pIdCliente = parseInt($(registro).children("td[aria-describedby='grdCliente_IdCliente']").html());
        ObtenerFormaDirecciones(JSON.stringify(Cliente));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarDireccion", function() {
        ObtenerFormaAgregarDireccion();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCuentaBancariaCliente", function() {
        var Cliente = new Object();
        Cliente.pIdCliente = $("#divFormaCuentasBancariasCliente").attr("IdCliente");
        ObtenerFormaAgregarCuentaBancariaCliente(JSON.stringify(Cliente));
    });

    $('#dialogAgregarCliente, #dialogEditarCliente,#dialogAgregarDireccion, #dialogEditarDireccion').on('change', '#cmbPais', function(event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstadosFiltro(JSON.stringify(request));

        request.pIdEstado = $(this).val();
        ObtenerListaMunicipiosFiltro(JSON.stringify(request));

        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidadesFiltro(JSON.stringify(request));

    });

    $("#grdCliente").on("click", ".imgFormaConsultarCliente", function() {
        var registro = $(this).parents("tr");
        var Cliente = new Object();
        Cliente.pIdCliente = parseInt($(registro).children("td[aria-describedby='grdCliente_IdCliente']").html());
        ObtenerFormaConsultarCliente(JSON.stringify(Cliente));
    });

    $('#dialogAgregarCliente, #dialogEditarCliente, #dialogAgregarDireccion, #dialogEditarDireccion').on('change', '#cmbEstado', function(event) {
        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipiosFiltro(JSON.stringify(request));
    });

    $('#dialogAgregarCliente, #dialogEditarCliente, #dialogAgregarDireccion, #dialogEditarDireccion').on('change', '#cmbMunicipio', function(event) {
        var request = new Object();
        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidadesFiltro(JSON.stringify(request));
    });

    $("#grdDirecciones").on("click", ".imgFormaConsultarDireccion", function() {
        var registro = $(this).parents("tr");
        var Direccion = new Object();
        Direccion.pIdDireccionOrganizacion = parseInt($(registro).children("td[aria-describedby='grdDirecciones_IdDireccionOrganizacion']").html());
        ObtenerFormaConsultarDireccion(JSON.stringify(Direccion));
    });

    $('#grdCliente').one('click', '.div_grdCliente_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCliente_AI']").children().attr("baja")
        var idCliente = $(registro).children("td[aria-describedby='grdCliente_IdCliente']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus_Cliente(idCliente, baja);
    });

    //Se agrego para descuento
    //    $("#grdCliente").on("click", ".imgFormaConsultarDescuentoCliente", function() {
    //        var registro = $(this).parents("tr");
    //        var Cliente = new Object();

    //        Cliente.pIdCliente = parseInt($(registro).children("td[aria-describedby='grdCliente_IdCliente']").html());
    //        $("#dialogConsultarDescuentoCliente").attr("idCliente", Cliente.pIdCliente);
    //        FiltroDescuentoCliente();
    //        $("#dialogConsultarDescuentoCliente").dialog("open");
    //    });

    $("#grdCliente").on("click", ".imgFormaConsultarDescuentoCliente", function() {
        var registro = $(this).parents("tr");
        var Cliente = new Object();
        Cliente.pIdCliente = parseInt($(registro).children("td[aria-describedby='grdCliente_IdCliente']").html());
        ObtenerFormaListaDescuentosCliente(JSON.stringify(Cliente));
    });

    $("#dialogConsultarDescuentoCliente").on("click", "#btnAgregarDescuentoCliente", function() {
        ObtenerFormaAgregarDescuentoCliente();
    });


    $('#dialogAgregarDescuentoCliente').on('focusin', '#txtDescuentoCliente', function(event) {
        $(this).quitarValorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarDescuentoCliente').on('focusout', '#txtDescuentoCliente', function(event) {
        $(this).valorPredeterminado("Porcentaje");
        if (parseInt($(this).val(), 10) > 100) {
            $(this).val("100");
        }
    });

    $('#dialogAgregarDescuentoCliente').on('keypress', '#txtDescuentoCliente', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
        else {
            if (LimitarPorcentajeNumero(event, $(this).val(), 100)) {
                return false;
            }
        }
    });

    $('#grdDescuentoCliente').one('click', '.div_grdDescuentoCliente_AI_DescuentoCliente', function(event) {

        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdDescuentoCliente_AI_DescuentoCliente']").children().attr("baja")
        var idDescuentoCliente = $(registro).children("td[aria-describedby='grdDescuentoCliente_IdDescuentoCliente']").html();

        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatusDescuentoCliente(idDescuentoCliente, baja);
    });

    $('#grdDirecciones').one('click', '.div_grdDirecciones_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdDirecciones_AI']").children().attr("baja")
        var idDireccionOrganizacion = $(registro).children("td[aria-describedby='grdDirecciones_IdDireccionOrganizacion']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus_Direccion(idDireccionOrganizacion, baja);
    });

    $('#grdContactoOrganizacion').one('click', '.div_grdContactoOrganizacion_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdContactoOrganizacion_AI']").children().attr("baja")
        var idContactoOrganizacion = $(registro).children("td[aria-describedby='grdContactoOrganizacion_IdContactoOrganizacion']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus_ContactoOrganizacion(idContactoOrganizacion, baja);
    });

    $("#grdCliente").on("click", ".imgFormaContactos", function() {
        var registro = $(this).parents("tr");
        var Cliente = new Object();
        Cliente.pIdCliente = parseInt($(registro).children("td[aria-describedby='grdCliente_IdCliente']").html());
        ObtenerFormaContactos(JSON.stringify(Cliente));
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarContactoOrganizacion", function() {
        ObtenerFormaAgregarContacto();
    });

    $("#dialogAgregarContacto").on("click", "#divAgregarTelefono", function() {
        $("#dialogAgregarTelefono").obtenerVista({
            nombreTemplate: "tmplAgregarTelefono_Cliente.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarTelefono").dialog("open");
            }
        });
    });

    $("#dialogAgregarContacto").on("click", ".divEliminarTelefono", function() {
        var registro = $(this).parents("li");
        $(registro).slideUp('slow', function() {
            $(registro).remove();
        });
    });

    $("#dialogAgregarContacto").on("click", "#divAgregarCorreo", function() {
        $("#dialogAgregarCorreo").obtenerVista({
            nombreTemplate: "tmplAgregarCorreo_Cliente.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarCorreo").dialog("open");
                $("#txtCorreo").addClass('txtCorreo');
            }
        });
    });

    $("#dialogAgregarContacto").on("click", ".divEliminarCorreo", function() {
        var registro = $(this).parents("li");
        $(registro).slideUp('slow', function() {
            $(registro).remove();
        });
    });

    $("#grdContactoOrganizacion").on("click", ".imgFormaConsultarContacto", function() {
        var registro = $(this).parents("tr");
        var Contacto = new Object();
        Contacto.pIdContactoOrganizacion = parseInt($(registro).children("td[aria-describedby='grdContactoOrganizacion_IdContactoOrganizacion']").html());
        ObtenerFormaConsultarContacto(JSON.stringify(Contacto));
    });

    $("#dialogEditarContacto").on("click", "#divAgregarTelefono", function() {
        $("#dialogAgregarTelefonoEditar").obtenerVista({
            nombreTemplate: "tmplAgregarTelefono_Cliente.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarTelefonoEditar").dialog("open");
            }
        });
    });

    $("#dialogEditarContacto").on("click", "#divAgregarCorreo", function() {
        $("#dialogAgregarCorreoEditar").obtenerVista({
            nombreTemplate: "tmplAgregarCorreo_Cliente.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarCorreoEditar").dialog("open");

            }
        });
    });

    $("#dialogEditarContacto").on("click", ".divEliminarEdicionTelefono", function() {
        var pCliente = new Object();
        pCliente.pIdTelefonoContactoOrganizacion = $(this).attr("IdTelefonoContactoOrganizacion");
        EliminaTelefonoContactoOrganizacion(JSON.stringify(pCliente));

        var registro = $(this).parents("li");
        $(registro).slideUp('slow', function() {
            $(registro).remove();
        });

    });

    $("#dialogEditarContacto").on("click", ".divEliminarEdicionCorreo", function() {
        var pCliente = new Object();
        pCliente.pIdCorreoContactoOrganizacion = $(this).attr("IdCorreoContactoOrganizacion");
        EliminaCorreoContactoOrganizacion(JSON.stringify(pCliente));

        var registro = $(this).parents("li");
        $(registro).slideUp('slow', function() {
            $(registro).remove();
        });
    });

    $('#dialogAgregarCliente, #dialogEditarCliente').on('focusin', '#txtIVAActual', function(event) {
        $(this).quitarValorPredeterminado("decimal");
    });

    $('#dialogAgregarCliente, #dialogEditarCliente').on('focusout', '#txtIVAActual', function(event) {
        $(this).valorPredeterminado("decimal");
    });

    $('#dialogAgregarCliente, #dialogEditarCliente').on('focusin', '#txtLimiteCredito', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });

    $('#dialogAgregarCliente, #dialogEditarCliente').on('focusout', '#txtLimiteCredito', function(event) {
        $(this).valorPredeterminado("Moneda");
    });

    $('#dialogAgregarCliente, #dialogEditarCliente').on('keypress', '#txtLimiteCredito', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarCliente, #dialogEditarCliente').on('keypress', '#txtIVAActual', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
    });

    $("#grdCliente").on("click", ".imgFormaCuentaBancariaCliente", function() {
        var registro = $(this).parents("tr");
        var Cliente = new Object();
        Cliente.pIdCliente = parseInt($(registro).children("td[aria-describedby='grdCliente_IdCliente']").html());
        ObtenerFormaCuentaBancariaCliente(JSON.stringify(Cliente));
    });

    $('#grdCuentaBancariaCliente').one('click', '.div_grdCuentaBancariaCliente_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancariaCliente_AI']").children().attr("baja")
        var idCuentaBancariaCliente = $(registro).children("td[aria-describedby='grdCuentaBancariaCliente_IdCuentaBancariaCliente']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatusCuentaBancariaCliente(idCuentaBancariaCliente, baja);
    });

    $('#dialogAgregarDireccion').on('click', '#chkDireccionFiscal', function(event) {
        if ($(this).is(':checked')) {
            var idCliente = $("#divFormaDireccionesOrganizacion").attr("idCliente");
            if (idCliente == 0) {
                MostrarMensajeError("Es necesario seleccionar al proveedor para obtener la dirección fiscal.");
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', 'checked');
                var request = new Object();
                request.pIdCliente = idCliente;
                var pRequest = JSON.stringify(request);
                $("#tblFormaAgregarDireccion").obtenerVista({
                    nombreTemplate: "tmplConsultarCliente-DireccionFiscal.html",
                    url: "Cliente.aspx/ObtenerDireccionFiscalCliente",
                    parametros: pRequest,
                    despuesDeCompilar: function(pRespuesta) {
                    }
                });
            }
        }
        else {
            $(this).attr('checked', false);
            $("#tblFormaAgregarDireccion").obtenerVista({
                nombreTemplate: "tmplConsultarProveedor-DireccionFiscal.html",
                url: "Proveedor.aspx/ObtenerFormaAgregarDireccion",
                despuesDeCompilar: function(pRespuesta) {
                }
            });
        }
    });

    $('#dialogAgregarCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCliente").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCliente();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarClienteAEnrolar').dialog({
        autoOpen: false,
        height: 'auto',
        width: '625',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarClienteAEnrolar").remove();
            $("#txtRFC").val('');
            $("#txtRazonSocial").val('');
        },
        buttons: {
            "Enrolar": function() {
                EnrolarCliente();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: '625',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCliente").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCliente").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCliente();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarProveedorAEnrolar').dialog({
        autoOpen: false,
        height: 'auto',
        width: '625',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarProveedorAEnrolar").remove();
            $("#txtRFC").val('');
            $("#txtRazonSocial").val('');
        },
        buttons: {
            "Enrolar": function() {

                EnrolarProveedor();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogDirecciones').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarDirecciones").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarDireccion').dialog({
        autoOpen: false,
        height: 'auto',
        width: '600',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarDireccion").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarDireccion();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarCuentaBancariaCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCuentaBancariaCliente").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCuentaBancariaCliente();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarDireccion').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarDireccion").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarDireccion').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarDireccion").remove();
        },
        buttons: {
            "Editar": function() {
                EditarDireccion();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogContactos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarContactos").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarContacto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarContacto").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarContactoOrganizacion();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#dialogAgregarTelefono").dialog({
        autoOpen: false,
        height: "auto",
        width: "270",
        modal: true,
        draggable: false,
        resizable: false,
        show: "fade",
        hide: "fade",
        close: function(event, ui) {
            $(this).empty();
        },
        buttons: {
            "Agregar": function() {
                AgregarTelefono();
            }
        }
    });

    $("#dialogAgregarCorreo").dialog({
        autoOpen: false,
        height: "auto",
        width: "auto",
        modal: true,
        draggable: false,
        resizable: false,
        show: "fade",
        hide: "fade",
        close: function(event, ui) {
            $(this).empty();
        },
        buttons: {
            "Agregar": function() {
                AgregarCorreo();
            }
        }
    });

    $('#dialogConsultarContacto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarContacto").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarContacto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarContacto").remove();
        },
        buttons: {
            "Editar": function() {
                EditarContacto();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#dialogAgregarTelefonoEditar").dialog({
        autoOpen: false,
        height: "auto",
        width: "auto",
        modal: true,
        draggable: false,
        resizable: false,
        show: "fade",
        hide: "fade",
        close: function(event, ui) {
            $(this).empty();
        },
        buttons: {
            "Agregar": function() {
                AgregarTelefonoEditar();
            }
        }
    });

    $("#dialogAgregarCorreoEditar").dialog({
        autoOpen: false,
        height: "auto",
        width: "auto",
        modal: true,
        draggable: false,
        resizable: false,
        show: "fade",
        hide: "fade",
        close: function(event, ui) {
            $(this).empty();
        },
        buttons: {
            "Agregar": function() {
                AgregarCorreoEditar();
            }
        }
    });

    $('#dialogCuentaBancariaCliente').dialog({
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
    }
});

$('#dialogConsultarCuentaBancariaCliente').dialog({
    autoOpen: false,
    height: 'auto',
    width: 'auto',
    modal: true,
    draggable: false,
    resizable: false,
    show: 'fade',
    hide: 'fade',
    close: function() {
        $("#divFormaConsultarCuentaBancariaCliente").remove();
    },
    buttons: {
        "Aceptar": function() {
            $(this).dialog("close");
        }
    }
});

$('#dialogEditarCuentaBancariaCliente').dialog({
    autoOpen: false,
    height: 'auto',
    width: 'auto',
    modal: true,
    draggable: false,
    resizable: false,
    show: 'fade',
    hide: 'fade',
    close: function() {
        $("#divFormaEditarCuentaBancariaCliente").remove();
    },
    buttons: {
        "Editar": function() {
            EditarCuentaBancariaCliente();
        },
        "Cancelar": function() {
            $(this).dialog("close")
        }
    }
});

$("#grdCuentaBancariaCliente").on("click", ".imgFormaConsultarCuentaBancariaCliente", function() {
    var registro = $(this).parents("tr");
    var CuentaBancariaCliente = new Object();
    CuentaBancariaCliente.pIdCuentaBancariaCliente = parseInt($(registro).children("td[aria-describedby='grdCuentaBancariaCliente_IdCuentaBancariaCliente']").html());
    ObtenerFormaConsultarCuentaBancariaCliente(JSON.stringify(CuentaBancariaCliente));
});

$('#dialogAgregarCliente, #dialogEditarCliente').on('keypress', '#txtRazonSocial', function(event) {
    $("#txtRazonSocial").addClass('uppercase');

});

$('#dialogConsultarDescuentoCliente').dialog({
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
        "Cerrar": function() {
            $(this).dialog("close");
        }
    }
});

$('#dialogAgregarDescuentoCliente').dialog({
    autoOpen: false,
    height: 'auto',
    width: 'auto',
    modal: true,
    draggable: false,
    resizable: false,
    show: 'fade',
    hide: 'fade',
    close: function() {
        $("#divFormaAgregarDescuentoCliente").remove();
    },
    buttons: {
        "Aceptar": function() {
            AgregarDescuentoCliente();
        },
        "Cancelar": function() {
            $(this).dialog("close");
        }
    }
});


//##############################################################################
//////funcion del grid//////
$("#gbox_grdCliente").livequery(function() {// MODIFICAR
    $("#grdCliente").jqGrid('navButtonAdd', '#pagCliente', {// MODIFICAR
        caption: "Exportar",
        title: "Exportar",
        buttonicon: 'ui-icon-newwin',
        onClickButton: function() {

            //var oRequest = new Object();
            var pIdTipoCliente = -1;
            var pIdTipoIndustria = -1;
            var pNombreCliente = "";
            var pRazonSocial = "";
            var pRFC = "";
            var pAgente = "";
            var pCliente = -1;
            var pAI = -1;

            if ($('#gs_IdTipoCliente').val() != -1)
            { pIdTipoCliente = $("#gs_IdTipoCliente").val(); }

            if ($('#gs_TipoIndustria').val() != -1)
            { pIdTipoIndustria = $("#gs_TipoIndustria").val(); }

            if ($('#gs_NombreComercial').val() != null && $('#gs_NombreComercial').val() != "")
            { pNombreCliente = $("#gs_NombreComercial").val(); }

            if ($('#gs_RazonSocial').val() != null && $('#gs_RazonSocial').val() != "")
            { pRazonSocial = $("#gs_RazonSocial").val(); }

            if ($('#gs_RFC').val() != null && $('#gs_RFC').val() != "")
            { pRFC = $("#gs_RFC").val(); }

            if ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "")
            { pAgente = $("#gs_Agente").val(); }

            if ($('#gs_EsCliente').val() != -1)
            { pEsCliente = $("#gs_EsCliente").val(); }

            if ($('#gs_AI').val() != -1)
            { pAI = $("#gs_AI").val(); }

            $.UnifiedExportFile({
                action: '../ExportacionesExcel/ExportarExcel.aspx',
                data: {
                    IsExportExcel: true,
                    pIdTipoCliente: pIdTipoCliente,
                    pIdTipoIndustria: pIdTipoIndustria,
                    pNombreCliente: pNombreCliente,
                    pRazonSocial: pRazonSocial,
                    pRFC: pRFC,
                    pAgente: pAgente,
                    pCliente: pCliente,
                    pAI: pAI
                },
                downloadType: 'Normal'
            });

        }
    });
});
//#################################################################################


$('#dialogAgregarCliente, #dialogEditarCliente').on('change', '#txtRFC', function(event) {
    RevisaExisteRFC();
});



$("#dialogPolizasCliente").dialog({
    autoOpen: false,
    modal: true,
    draggable: false,
    resizable: false,
    width: 'auto',
    height: 'auto',
    close: function() {
        $("#divPolizasCliente", this).remove();
    },
    buttons: {
        "Cerrar": function() {
            $(this).dialog("close");
        }
    }
});

$("#grdCliente").on("click", ".divImagenPolizas", function() {
    var Cliente = new Object();
    Cliente.IdCliente = parseInt($(this).parent("td").parent("tr").children("td[aria-describedby=grdCliente_IdCliente]").html());
    ObtenerPolizasCliente(JSON.stringify(Cliente));
});


});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCliente(pRequest) {
	$("#dialogAgregarCliente").obtenerVista({
		url: "Cliente.aspx/ObtenerFormaAgregarCliente",
		parametros: pRequest,
		dataType: "json",
		nombreTemplate: "tmplAgregarCliente.html",
		despuesDeCompilar: function (pRespuesta) {
			respuesta = pRespuesta;
			$("#dialogAgregarCliente").dialog("open");
			autocompletar();
			$("#txtCorreo").addClass('lowercase');

			if (respuesta.modelo.IdOrganizacion != '') {
				if (respuesta.modelo.Permisos.puedeEditarDatosOrganizacion == 0) {
					if (respuesta.modelo.IdSucursalActual != respuesta.modelo.IdSucursalAlta) {
						BloquearClases("table#DatosOrganizacion tr");
					}
				}

				if (respuesta.modelo.Permisos.puedeEditarDatosFiscales == 0) {
					if (respuesta.modelo.IdSucursalActual != respuesta.modelo.IdSucursalAlta) {
						BloquearClases("table#DatosFiscales tr");
					}
				}

				if (respuesta.modelo.Permisos.puedeEditarLimiteCredito == 0) { BloquearClases("table#DatosLineaCredito tr"); }
			}
		}
	});
}

function ObtenerGridOportunidadesCliente(Cliente) {
	var ventanaOportunidad = $('<div title="Oportunidades"></div>').dialog({
		modal: true,
		draggable: false,
		resizable: false,
		width: "auto",
		height: "auto",
		autoOpen: false,
		close: function () { $("#divMotrarOportunidadesCliente").remove(); }
	});

	var Request = JSON.stringify(Cliente);

	$(ventanaOportunidad).obtenerVista({
		url: "Cliente.aspx/ObtenerFormaOportunidadesCliente",
		parametros: Request,
		nombreTemplate: "tmplMostrarOportunidadesCliente.html",
		despuesDeCompilar: function (Respuesta) {
			initGridOportunidadesCliente();
			var json = Respuesta.modelo;
			$(ventanaOportunidad).dialog("open");
		}
	});
}

function ObtenerListaEstadosFiltro(pRequest) {
	$("#cmbEstado").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaEstados",
		parametros: pRequest,
		despuesDeCompilar: function (pRespuesta) {
		}
	});
}

function ObtenerListaMunicipiosFiltro(pRequest) {
	$("#cmbMunicipio").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaMunicipios",
		parametros: pRequest,
		despuesDeCompilar: function (pRespuesta) {
		}
	});
}

function ObtenerListaLocalidadesFiltro(pRequest) {
	$("#cmbLocalidad").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaLocalidades",
		parametros: pRequest,
		despuesDeCompilar: function (pRespuesta) {
		}
	});
}

function ObtenerListaTipoIndustria() {
	$("#cmbTipoIndustria").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaTiposIndustrias"
	});
}

function ObtenerListaEstado() {
	var request = new Object();
	request.pIdPais = $("#cmbPais").val();

	$("#cmbEstado").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaEstados",
		parametros: JSON.stringify(request),
		despuesDeCompilar: function (pRespuesta) {
		}
	});
}

function ObtenerListaMunicipio() {
	var request = new Object();
	request.pIdEstado = $("#cmbEstado").val();

	$("#cmbMunicipio").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaMunicipios",
		parametros: JSON.stringify(request),
		despuesDeCompilar: function (pRespuesta) {
		}
	});
}

function ObtenerListaLocalidad() {
	var request = new Object();
	request.pIdMunicipio = $("#cmbMunicipio").val();

	$("#cmbLocalidad").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaLocalidades",
		parametros: JSON.stringify(request),
		despuesDeCompilar: function (pRespuesta) {
		}
	});
}

function ObtenerListaTipoIndustria() {
	$("#cmbTipoIndustria").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaTiposIndustrias"
	});
}

function ObtenerListaCampana() {
	$("#cmbCampana").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaCampanas"
	});
}

function ObtenerListaTipoGarantia() {
	$("#cmbTipoGarantia").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaTiposGarantia"
	});
}

function ObtenerListaTipoCliente() {
	$("#cmbTipoCliente").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaTiposClientes"
	});
}

function ObtenerListaCondicionPago() {
	$("#cmbCondicionPago").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cliente.aspx/ObtenerListaCondicionesPago"
	});
}



function ObtenerFormaAgregarDireccion() {
	$("#dialogAgregarDireccion").obtenerVista({
		nombreTemplate: "tmplAgregarDireccion_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaAgregarDireccion",
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogAgregarDireccion").dialog("open");
		}
	});
}

function ObtenerFormaAgregarCuentaBancariaCliente(pIdCliente) {
	$("#dialogAgregarCuentaBancariaCliente").obtenerVista({
		nombreTemplate: "tmplAgregarCuentaBancariaCliente.html",
		url: "Cliente.aspx/ObtenerFormaAgregarCuentaBancariaCliente",
		parametros: pIdCliente,
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogAgregarCuentaBancariaCliente").dialog("open");
		}
	});
}

function ObtenerFormaConsultarDireccion(pIdDireccionOrganizacion) {
	$("#dialogConsultarDireccion").obtenerVista({
		nombreTemplate: "tmplConsultarDireccion_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaDireccionOrganizacion",
		parametros: pIdDireccionOrganizacion,
		despuesDeCompilar: function (pRespuesta) {
			if (pRespuesta.modelo.Permisos.puedeEditarDireccion == 1) {
				$("#dialogConsultarDireccion").dialog("option", "buttons", {
					"Editar": function () {
						$(this).dialog("close");
						var Direccion = new Object();
						Direccion.IdDireccionOrganizacion = parseInt($("#divFormaConsultarDireccion").attr("IdDireccionOrganizacion"));
						ObtenerFormaEditarDireccion(JSON.stringify(Direccion))
					}
				});
				$("#dialogConsultarDireccion").dialog("option", "height", "auto");
			}
			else {
				$("#dialogConsultarDireccion").dialog("option", "buttons", {});
				$("#dialogConsultarDireccion").dialog("option", "height", "300");
			}
			$("#dialogConsultarDireccion").dialog("open");
		}
	});
}

function ObtenerFormaEditarDireccion(IdDireccionOrganizacion) {
	$("#dialogEditarDireccion").obtenerVista({
		nombreTemplate: "tmplEditarDireccion_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaEditarDireccion",
		parametros: IdDireccionOrganizacion,
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogEditarDireccion").dialog("open");
		}
	});
}

function ObtenerFormaContactos(pIdCliente) {
	$("#divFormaContactos").obtenerVista({
		nombreTemplate: "tmplConsultarContactoOrganizacion_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaConsultarContactoOrganizacion",
		parametros: pIdCliente,
		despuesDeCompilar: function (pRespuesta) {
			FiltroContactoOrganizacion();
			$("#dialogContactos").dialog("option", "buttons", {});
			$("#dialogContactos").dialog("open");
		}
	});
}

function ObtenerFormaAgregarContacto() {
	$("#dialogAgregarContacto").obtenerVista({
		nombreTemplate: "tmplAgregarContactoOrganizacion_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaAgregarContactoOrganizacion",
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogAgregarContacto").dialog("open");
			$("#txtFechaCumpleanio").datepicker();
		}
	});
}

function ObtenerFormaConsultarContacto(pIdContactoOrganizacion) {
	$("#dialogConsultarContacto").obtenerVista({
		nombreTemplate: "tmplConsultarContacto_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaContactoOrganizacion",
		parametros: pIdContactoOrganizacion,
		despuesDeCompilar: function (pRespuesta) {
			if (pRespuesta.modelo.Permisos.puedeEditarContacto == 1) {
				$("#dialogConsultarContacto").dialog("option", "buttons", {
					"Editar": function () {
						$(this).dialog("close");
						var Contacto = new Object();
						Contacto.IdContactoOrganizacion = parseInt($("#divFormaConsultarContacto").attr("IdContactoOrganizacion"));
						ObtenerFormaEditarContacto(JSON.stringify(Contacto))

					}
				});
				$("#dialogConsultarContacto").dialog("option", "height", "auto");
			}
			else {
				$("#dialogConsultarContacto").dialog("option", "buttons", {});
				$("#dialogConsultarContacto").dialog("option", "height", "auto");
			}
			$("#dialogConsultarContacto").dialog("open");
		}
	});
}

function ObtenerFormaEditarContacto(IdContactoOrganizacion) {
	$("#dialogEditarContacto").obtenerVista({
		nombreTemplate: "tmplEditarContacto_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaEditarContacto",
		parametros: IdContactoOrganizacion,
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogEditarContacto").dialog("open");
			$("#txtFechaCumpleanio").datepicker();
		}
	});
}

function ObtenerFormaConsultarCuentaBancariaCliente(pIdCuentaBancariaCliente) {
	$("#dialogConsultarCuentaBancariaCliente").obtenerVista({
		nombreTemplate: "tmplConsultarCuentaBancariaCliente.html",
		url: "Cliente.aspx/ObtenerFormaConsultarCuentaBancariaCliente",
		parametros: pIdCuentaBancariaCliente,
		despuesDeCompilar: function (pRespuesta) {
			if (pRespuesta.modelo.Permisos.puedeConsultarCuentaBancariaCliente == 1) {
				$("#dialogConsultarCuentaBancariaCliente").dialog("option", "buttons", {
					"Editar": function () {
						$(this).dialog("close");
						var CuentaBancariaCliente = new Object();
						CuentaBancariaCliente.IdCuentaBancariaCliente = parseInt($("#divFormaConsultarCuentaBancariaCliente").attr("IdCuentaBancariaCliente"));
						ObtenerFormaEditarCuentaBancariaCliente(JSON.stringify(CuentaBancariaCliente))
					}
				});
				$("#dialogConsultarCuentaBancariaCliente").dialog("option", "height", "auto");
			}
			else {
				$("#dialogConsultarCuentaBancariaCliente").dialog("option", "buttons", {});
				$("#dialogConsultarCuentaBancariaCliente").dialog("option", "height", "300");
			}
			$("#dialogConsultarCuentaBancariaCliente").dialog("open");
		}
	});
}

function ObtenerFormaEditarCuentaBancariaCliente(IdCuentaBancariaCliente) {
	$("#dialogEditarCuentaBancariaCliente").obtenerVista({
		nombreTemplate: "tmplEditarCuentaBancariaCliente.html",
		url: "Cliente.aspx/ObtenerFormaEditarCuentaBancariaCliente",
		parametros: IdCuentaBancariaCliente,
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogEditarCuentaBancariaCliente").dialog("open");
		}
	});
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCliente() {
	var pCliente = new Object();
	//Datos organizacion
	pCliente.RazonSocial = $("#txtRazonSocial").val();
	pCliente.NombreComercial = $("#txtNombreComercial").val();
	pCliente.RFC = $("#txtRFC").val();
	pCliente.Notas = $("#txtNotas").val();
	pCliente.Dominio = $("#txtDominio").val();
	pCliente.IdTipoIndustria = $("#cmbTipoIndustria").val();
	pCliente.IdGrupoEmpresarial = $("#cmbGrupoEmpresarial").val();
	pCliente.CuentaContable = $("#txtCuentaContable").val();
	pCliente.CuentaContableDolares = $("#txtCuentaContableDolares").val();
	pCliente.IdCampana = $("#cmbCampana").val();

	//Datos direccion de organizacion
	pCliente.Calle = $("#txtCalle").val();
	pCliente.NumeroExterior = $("#txtNumeroExterior").val();
	pCliente.NumeroInterior = $("#txtNumeroInterior").val();
	pCliente.Colonia = $("#txtColonia").val();
	pCliente.CodigoPostal = $("#txtCodigoPostal").val();
	pCliente.Conmutador = $("#txtConmutador").val();
	pCliente.IdMunicipio = $("#cmbMunicipio").val();
	pCliente.Referencia = $("#txtReferencia").val();
	pCliente.IdLocalidad = $("#cmbLocalidad").val();

	//Datos cliente 
	pCliente.LimiteDeCredito = QuitarFormatoNumero($("#txtLimiteCredito").val());
	pCliente.Correo = $("#txtCorreo").val();
	pCliente.IdTipoCliente = $("#cmbTipoCliente").val();
	pCliente.IdCondicionPago = $("#cmbCondicionPago").val();
	pCliente.IdFormaContacto = $("#cmbFormaContacto").val();
	pCliente.IVAActual = $("#txtIVAActual").val();
	pCliente.IdSegmentoMercado = $("#cmbSegmentoMercado").val();
	pCliente.IdTipoGarantia = $("#cmbTipoGarantia").val();
	pCliente.IdUsuarioAgente = $("#cmbUsuarioAgente").val();

	var validacion = ValidaCliente(pCliente);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetAgregarCliente(JSON.stringify(oRequest));
}

function SetAgregarCliente(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/AgregarCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				if (respuesta.Descripcion == "enrolar") {
					ObtenerFormaConsultarClienteAEnrolar(JSON.stringify(respuesta.Modelo));
					$("#dialogAgregarCliente").dialog("close");
				}
				else if (respuesta.Descripcion == "existeProveedor") {
					var Organizacion = new Object();
					Organizacion.pIdOrganizacion = parseInt(respuesta.IdOrganizacion);
					ObtenerFormaAgregarCliente(JSON.stringify(Organizacion))
				}
				$("#grdCliente").trigger("reloadGrid");
				$("#dialogAgregarCliente").dialog("close");
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

function ObtenerFormaConsultarClienteAEnrolar(pIdCliente) {
	$("#dialogConsultarClienteAEnrolar").obtenerVista({
		nombreTemplate: "tmplConsultarClienteAEnrolar.html",
		url: "Cliente.aspx/ObtenerFormaClienteAEnrolar",
		parametros: pIdCliente,
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogConsultarClienteAEnrolar").dialog("open");
		}
	});
}

function ObtenerFormaConsultarProveedorAEnrolar(pIdOrganizacion) {
	$("#dialogConsultarProveedorAEnrolar").obtenerVista({
		nombreTemplate: "tmplConsultarProveedorAEnrolar.html",
		url: "Cliente.aspx/ObtenerFormaProveedorAEnrolar",
		parametros: pIdOrganizacion,
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogConsultarProveedorAEnrolar").dialog("open");
		}
	});
}

function EnrolarCliente() {
	var pCliente = new Object();
	pCliente.IdCliente = $("#divFormaConsultarClienteAEnrolar").attr("idCliente");
	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetEnrolarCliente(JSON.stringify(oRequest));
}

function SetEnrolarCliente(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EnrolarCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCliente").trigger("reloadGrid");
				$("#dialogAgregarCliente, #dialogConsultarCliente").dialog("close");
				var Cliente = new Object();
				Cliente.pIdCliente = respuesta.IdCliente;
				ObtenerFormaConsultarCliente(JSON.stringify(Cliente));
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogConsultarClienteAEnrolar").dialog("close");
		}
	});
}

function EnrolarProveedor() {
	var pProveedor = new Object();
	pProveedor.IdProveedor = $("#divFormaConsultarProveedorAEnrolar").attr("idProveedor");
	var oRequest = new Object();
	oRequest.pProveedor = pProveedor;
	SetEnrolarProveedor(JSON.stringify(oRequest));
}

function SetEnrolarProveedor(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EnrolarProveedor",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCliente").trigger("reloadGrid");
				$("#dialogAgregarCliente, #dialogConsultarCliente, #dialogConsultarProveedorAEnrolar").dialog("close");
				var Cliente = new Object();
				Cliente.pIdCliente = respuesta.IdCliente;
				ObtenerFormaConsultarCliente(JSON.stringify(Cliente));
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogConsultarClienteAEnrolar").dialog("close");
		}
	});
}

function ObtenerFormaConsultarCliente(pIdCliente) {
	$("#dialogConsultarCliente").obtenerVista({
		nombreTemplate: "tmplConsultarCliente.html",
		url: "Cliente.aspx/ObtenerFormaConsultarCliente",
		parametros: pIdCliente,
		despuesDeCompilar: function (pRespuesta) {
			respuesta = pRespuesta
			if (respuesta.modelo.Permisos.puedeEditarCliente == 1 && respuesta.modelo.Permisos.diferenteSucursal == 0) {
				$("#dialogConsultarCliente").dialog("option", "buttons", {
					"Editar": function () {
						var Cliente = new Object();
						Cliente.IdCliente = parseInt($("#divFormaConsultarCliente").attr("IdCliente"));
						$(this).dialog("close");
						ObtenerFormaEditarCliente(JSON.stringify(Cliente))
					}
				});
				$("#dialogConsultarCliente").dialog("option", "height", "auto");
			}
			else {
				$("#dialogConsultarCliente").dialog("option", "buttons", {});
				$("#dialogConsultarCliente").dialog("option", "height", "570");
			}
			$("#dialogConsultarCliente").dialog("open");
		}
	});
}

function ObtenerFormaEditarCliente(IdCliente) {
	$("#dialogEditarCliente").obtenerVista({
		nombreTemplate: "tmplEditarCliente.html",
		url: "Cliente.aspx/ObtenerFormaEditarCliente",
		parametros: IdCliente,
		despuesDeCompilar: function (pRespuesta) {
			respuesta = pRespuesta;
			$("#dialogEditarCliente").dialog("open");
			autocompletar();
			$("#txtLimiteCredito").valorPredeterminado("Moneda");
			$("#txtIVAActual").valorPredeterminado("decimal");

			if (respuesta.modelo.Permisos.puedeEditarDatosOrganizacion == 0) {
				if (respuesta.modelo.IdSucursalActual != respuesta.modelo.IdSucursalAlta) {
					BloquearClases("table#DatosOrganizacion tr");
				}
			}

			if (respuesta.modelo.Permisos.puedeEditarDatosFiscales == 0) {
				if (respuesta.modelo.IdSucursalActual != respuesta.modelo.IdSucursalAlta) {
					BloquearClases("table#DatosFiscales tr");
				}
			}

			if (respuesta.modelo.Permisos.puedeEditarLimiteCredito == 0) { BloquearClases("table#DatosLineaCredito tr"); }
		}
	});
}

function ObtenerFormaDirecciones(pIdCliente) {
	$("#divFormaDirecciones").obtenerVista({
		nombreTemplate: "tmplDirecciones_Cliente.html",
		url: "Cliente.aspx/ObtenerFormaDirecciones",
		parametros: pIdCliente,
		despuesDeCompilar: function (pRespuesta) {
			FiltroDirecciones();
			$("#dialogDirecciones").dialog("option", "buttons", {});
			$("#dialogDirecciones").dialog("open");
		}
	});
}

function ObtenerFormaCuentaBancariaCliente(pIdCliente) {
	$("#divFormaCuentaBancariaCliente").obtenerVista({
		nombreTemplate: "tmplCuentaBancariaCliente.html",
		url: "Cliente.aspx/ObtenerFormaCuentaBancariaCliente",
		parametros: pIdCliente,
		despuesDeCompilar: function (pRespuesta) {
			Inicializar_grdCuentaBancariaCliente();
			FiltroCuentaBancariaCliente();
			$("#dialogCuentaBancariaCliente").dialog("open");

			$('#grdCuentaBancariaCliente').one('click', '.div_grdCuentaBancariaCliente_AI', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancariaCliente_AI']").children().attr("baja")
				var idCuentaBancariaCliente = $(registro).children("td[aria-describedby='grdCuentaBancariaCliente_IdCuentaBancariaCliente']").html();
				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
				}
				SetCambiarEstatusCuentaBancariaCliente(idCuentaBancariaCliente, baja);
			});
		}
	});
}

function ObtenerFormaListaDescuentosCliente(pIdCliente) {
	$("#divFormaCuentaBancariaCliente").obtenerVista({
		nombreTemplate: "tmplFormaDescuentoCliente.html",
		url: "Cliente.aspx/ObtenerFormaListaDescuentosCliente",
		parametros: pIdCliente,
		despuesDeCompilar: function (pRespuesta) {
			FiltroDescuentoCliente();
			$("#dialogConsultarDescuentoCliente").dialog("open");

			$('#grdDescuentoCliente').one('click', '.div_grdDescuentoCliente_AI', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdDescuentoCliente_AI']").children().attr("baja")
				var idDescuentoCliente = $(registro).children("td[aria-describedby='grdDescuentoCliente_IdDescuentoCliente']").html();
				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
				}
				SetCambiarEstatusDescuentoCliente(idDescuentoCliente, baja);
			});
		}
	});
}


function EditarCliente() {
	var pCliente = new Object();
	pCliente.IdCliente = $("#divFormaEditarCliente").attr("idCliente");

	//Datos organizacion
	pCliente.RazonSocial = $("#txtRazonSocial").val();
	pCliente.NombreComercial = $("#txtNombreComercial").val();
	pCliente.RFC = $("#txtRFC").val();
	pCliente.Notas = $("#txtNotas").val();
	pCliente.Dominio = $("#txtDominio").val();
	pCliente.IdTipoIndustria = $("#cmbTipoIndustria").val();
	pCliente.IdGrupoEmpresarial = $("#cmbGrupoEmpresarial").val();
	pCliente.CuentaContable = $("#txtCuentaContable").val();
	pCliente.CuentaContableDolares = $("#txtCuentaContableDolares").val();
	pCliente.IdCampana = $("#cmbCampana").val();

	//Datos direccion de organizacion
	pCliente.Calle = $("#txtCalle").val();
	pCliente.NumeroExterior = $("#txtNumeroExterior").val();
	pCliente.NumeroInterior = $("#txtNumeroInterior").val();
	pCliente.Colonia = $("#txtColonia").val();
	pCliente.CodigoPostal = $("#txtCodigoPostal").val();
	pCliente.Conmutador = $("#txtConmutador").val();
	pCliente.IdMunicipio = $("#cmbMunicipio").val();
	pCliente.Referencia = $("#txtReferencia").val();
	pCliente.IdLocalidad = $("#cmbLocalidad").val();

	//Datos cliente 
	pCliente.LimiteDeCredito = $("#txtLimiteCredito").val();
	pCliente.Correo = $("#txtCorreo").val();
	pCliente.IdTipoCliente = $("#cmbTipoCliente").val();
	pCliente.IdCondicionPago = $("#cmbCondicionPago").val();
	pCliente.IdFormaContacto = $("#cmbFormaContacto").val();
	pCliente.IVAActual = $("#txtIVAActual").val();
	pCliente.IdSegmentoMercado = $("#cmbSegmentoMercado").val();
	pCliente.IdTipoGarantia = $("#cmbTipoGarantia").val();
	pCliente.IdUsuarioAgente = $("#cmbUsuarioAgente").val();

	var validacion = ValidaCliente(pCliente);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }
	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetEditarCliente(JSON.stringify(oRequest));
}
function SetEditarCliente(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EditarCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCliente").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogEditarCliente").dialog("close");
		}
	});
}

function AgregarDireccion() {
	var pCliente = new Object();
	pCliente.IdCliente = $("#divFormaDireccionesOrganizacion").attr("idCliente");
	pCliente.IdTipoDireccion = $("#cmbTipoDireccion").val();
	pCliente.Calle = $("#txtCalle").val();
	pCliente.NumeroExterior = $("#txtNumeroExterior").val();
	pCliente.NumeroInterior = $("#txtNumeroInterior").val();
	pCliente.Colonia = $("#txtColonia").val();
	pCliente.CodigoPostal = $("#txtCodigoPostal").val();
	pCliente.Conmutador = $("#txtConmutador").val();
	pCliente.IdMunicipio = $("#cmbMunicipio").val();
	pCliente.Referencia = $("#txtReferencia").val();
	pCliente.IdLocalidad = $("#cmbLocalidad").val();
	pCliente.Descripcion = $("#txtDescripcionDireccion").val();
	//    var validacion = ValidaDireccion(pCliente);
	//    if (validacion != "")
	//    { MostrarMensajeError(validacion); return false; }
	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetAgregarDireccion(JSON.stringify(oRequest));
}
function SetAgregarDireccion(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/AgregarDireccion",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdDirecciones").trigger("reloadGrid");
				$("#dialogAgregarDireccion").dialog("close");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function FiltroDirecciones() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdDirecciones').getGridParam('rowNum');
	request.pPaginaActual = $('#grdDirecciones').getGridParam('page');
	request.pColumnaOrden = $('#grdDirecciones').getGridParam('sortname');
	request.pTipoOrden = $('#grdDirecciones').getGridParam('sortorder');
	request.pIdCliente = 0;
	request.pAI = 0;
	request.pEsCliente = -1;

	if ($("#divFormaDireccionesOrganizacion").attr("IdCliente") != null) {
		request.pIdCliente = $("#divFormaDireccionesOrganizacion").attr("IdCliente");
	}

	if ($('#divContGridDireccion').find(gs_AI).existe()) {
		request.pAI = $('#divContGridDireccion').find(gs_AI).val();
	}

	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cliente.aspx/ObtenerDirecciones',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success')
			{ $('#grdDirecciones')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
			else
			{ alert(JSON.parse(jsondata.responseText).Message); }
		}
	});
}

function EditarDireccion() {
	var pCliente = new Object();
	pCliente.IdDireccionOrganizacion = $("#divFormaEditarDireccion").attr("idDireccionOrganizacion");
	pCliente.IdTipoDireccion = $("#cmbTipoDireccion").val();
	pCliente.Calle = $("#txtCalle").val();
	pCliente.NumeroExterior = $("#txtNumeroExterior").val();
	pCliente.NumeroInterior = $("#txtNumeroInterior").val();
	pCliente.Colonia = $("#txtColonia").val();
	pCliente.CodigoPostal = $("#txtCodigoPostal").val();
	pCliente.Conmutador = $("#txtConmutador").val();
	pCliente.IdMunicipio = $("#cmbMunicipio").val();
	pCliente.Referencia = $("#txtReferencia").val();
	pCliente.IdLocalidad = $("#cmbLocalidad").val();
	pCliente.Descripcion = $("#txtDescripcionDireccion").val();

	//    var validacion = ValidaDireccion(pCliente);
	//    if (validacion != "")
	//    { MostrarMensajeError(validacion); return false; }
	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetEditarDireccion(JSON.stringify(oRequest));
}
function SetEditarDireccion(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EditarDireccion",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdDirecciones").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogEditarDireccion").dialog("close");
		}
	});
}

function SetCambiarEstatus_Cliente(pIdCliente, pBaja) {
	var pRequest = "{'pIdCliente':" + pIdCliente + ", 'pBaja':" + pBaja + "}";
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/CambiarEstatus_Cliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCliente").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			$('#grdCliente').one('click', '.div_grdCliente_AI', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdCliente_AI']").children().attr("baja")
				var idCliente = $(registro).children("td[aria-describedby='grdCliente_IdCliente']").html();
				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
				}
				SetCambiarEstatus_Cliente(idCliente, baja);
			});
		}
	});
}

function SetCambiarEstatus_Direccion(pIdDireccionOrganizacion, pBaja) {
	var pRequest = "{'pIdDireccionOrganizacion':" + pIdDireccionOrganizacion + ", 'pBaja':" + pBaja + "}";
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/CambiarEstatus_Direccion",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {

			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdDirecciones").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			$('#grdDirecciones').one('click', '.div_grdDirecciones_AI', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdDirecciones_AI']").children().attr("baja")
				var idDireccionOrganizacion = $(registro).children("td[aria-describedby='grdDirecciones_IdDireccionOrganizacion']").html();
				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
				}
				SetCambiarEstatus_Direccion(idDireccionOrganizacion, baja);
			});
		}
	});
}

function SetCambiarEstatus_ContactoOrganizacion(pIdContactoOrganizacion, pBaja) {
	var pRequest = "{'pIdContactoOrganizacion':" + pIdContactoOrganizacion + ", 'pBaja':" + pBaja + "}";
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/CambiarEstatus_ContactoOrganizacion",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {

			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdContactoOrganizacion").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			$('#grdContactoOrganizacion').one('click', '.div_grdContactoOrganizacion_AI', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdContactoOrganizacion_AI']").children().attr("baja")
				var idContactoOrganizacion = $(registro).children("td[aria-describedby='grdContactoOrganizacion_IdContactoOrganizacion']").html();
				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
				}
				SetCambiarEstatus_ContactoOrganizacion(idContactoOrganizacion, baja);
			});
		}
	});
}

function FiltroContactoOrganizacion() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdContactoOrganizacion').getGridParam('rowNum');
	request.pPaginaActual = $('#grdContactoOrganizacion').getGridParam('page');
	request.pColumnaOrden = $('#grdContactoOrganizacion').getGridParam('sortname');
	request.pTipoOrden = $('#grdContactoOrganizacion').getGridParam('sortorder');
	request.pIdCliente = 0;
	request.pAI = 0;
	request.pEsCliente = -1;

	if ($("#divFormaContactosOrganizacion").attr("IdCliente") != null) {
		request.pIdCliente = $("#divFormaContactosOrganizacion").attr("IdCliente");
	}

	if ($('#divGridContactoOrganizacion').find(gs_AI).existe()) {
		request.pAI = $('#divGridContactoOrganizacion').find(gs_AI).val();
	}

	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cliente.aspx/ObtenerContactoOrganizacion',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success')
			{ $('#grdContactoOrganizacion')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
			else
			{ alert(JSON.parse(jsondata.responseText).Message); }
		}
	});
}

function AgregarTelefono() {
	var pRequest = new Object();
	pRequest.Telefono = $("#txtTelefono").val();
	pRequest.Descripcion = $("#txtDescripcion").val();

	var validacion = ValidarTelefono(pRequest);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	$("#ulTelefonos").obtenerVista({
		nombreTemplate: "tmplAgregarTelefonoAEnrolar_Cliente.html",
		modelo: pRequest,
		remplazarVista: false,
		efecto: "slide"
	});

	$("#dialogAgregarTelefono").dialog("close");
}

function AgregarCorreo() {
	var pRequest = new Object();
	pRequest.Correo = $("#txtCorreo").val();
	var validacion = ValidarCorreoContacto(pRequest.Correo);

	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	$("#ulCorreos").obtenerVista({
		nombreTemplate: "tmplAgregarCorreoAEnrolar_Cliente.html",
		modelo: pRequest,
		remplazarVista: false,
		efecto: "slide"
	});

	$("#dialogAgregarCorreo").dialog("close");
}

function AgregarContactoOrganizacion() {
	var pCliente = new Object();
	pCliente.IdCliente = $("#divFormaContactosOrganizacion").attr("idCliente");
	pCliente.Nombre = $("#txtNombre").val();
	pCliente.Puesto = $("#txtPuesto").val();
	pCliente.Notas = $("#txtNotas").val();
	pCliente.FechaCumpleanio = $("#txtFechaCumpleanio").val();

	pCliente.Telefonos = new Array();
	$("#ulTelefonos li").each(function () {
		var pTelefono = new Object();
		pTelefono.Telefono = $(this).attr("telefono");
		pTelefono.Descripcion = $(this).attr("descripcion");
		pCliente.Telefonos.push(pTelefono);
	});

	pCliente.Correos = new Array();
	$("#ulCorreos li").each(function () {
		pCliente.Correos.push($(this).attr("correo"));
	});

	var validacion = ValidaContacto(pCliente);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }
	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetAgregarContacto(JSON.stringify(oRequest));
}

function SetAgregarContacto(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/AgregarContacto",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdContactoOrganizacion").trigger("reloadGrid");
				$("#dialogAgregarContacto").dialog("close");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function EditarContacto() {
	var pCliente = new Object();
	pCliente.IdContactoOrganizacion = $("#divFormaEditarContacto").attr("idContactoOrganizacion");
	pCliente.Nombre = $("#txtNombre").val();
	pCliente.Puesto = $("#txtPuesto").val();
	pCliente.Celular = $("#txtCelular").val();
	pCliente.Fax = $("#txtFax").val();
	pCliente.Notas = $("#txtNotas").val();
	pCliente.FechaCumpleanio = $("#txtFechaCumpleanio").val();


	pCliente.Telefonos = new Array();
	$("#ulTelefonos li").each(function () {
		pCliente.Telefonos.push($(this).attr("telefono"));
	});


	pCliente.Correos = new Array();
	$("#ulCorreos li").each(function () {
		pCliente.Correos.push($(this).attr("correo"));
	});


	var validacion = ValidaContacto(pCliente);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }
	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetEditarContacto(JSON.stringify(oRequest));
}

function SetEditarContacto(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EditarContacto",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdContactoOrganizacion").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogEditarContacto").dialog("close");
		}
	});
}

function AgregarTelefonoEditar() {
	var modelo = new Object();
	modelo.Telefono = $("#txtTelefono").val();
	modelo.Descripcion = $("#txtDescripcion").val();

	var validacion = ValidarTelefono(modelo);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	$("#ulTelefonos").obtenerVista({
		nombreTemplate: "tmplAgregarTelefonoAEnrolar_Cliente.html",
		modelo: modelo,
		remplazarVista: false,
		efecto: "slide"
	});

	var pRequest = new Object();
	pRequest.Telefonos = new Array();
	$("#ulTelefonos li").each(function () {
		pRequest.Telefonos.push($(this).attr("telefono"));
	});

	pRequest.IdContactoOrganizacion = $("#divFormaEditarContacto").attr("idContactoOrganizacion");
	pRequest.Telefono = $("#txtTelefono").val();
	pRequest.Descripcion = $("#txtDescripcion").val();

	var oRequest = new Object();
	oRequest.pCliente = pRequest;
	SetAgregarTelefonoEditar(JSON.stringify(oRequest));


}

function SetAgregarTelefonoEditar(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/AgregarTelefonoEditar",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#dialogAgregarTelefonoEditar").dialog("close");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function AgregarCorreoEditar() {
	var correo = $("#txtCorreo").val();
	var validacion = ValidarCorreoContacto(correo);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	var pRequest = new Object();
	pRequest.Correo = correo;
	$("#ulCorreos").obtenerVista({
		nombreTemplate: "tmplAgregarCorreoAEnrolar_Cliente.html",
		modelo: pRequest,
		remplazarVista: false,
		efecto: "slide"
	});

	var pCliente = new Object();
	pCliente.Correos = new Array();
	$("#ulCorreos li").each(function () {
		pCliente.Correos.push($(this).attr("correo"));
	});

	pCliente.IdContactoOrganizacion = $("#divFormaEditarContacto").attr("idContactoOrganizacion");
	pCliente.Correo = correo;
	var oRequest = new Object();
	oRequest.pCliente = pCliente;
	SetAgregarCorreoEditar(JSON.stringify(oRequest));

	var Telefono = new Object();
	Telefono.IdContactoOrganizacion = parseInt($("#divFormaEditarContacto").attr("idContactoOrganizacion"));
	//ObtenerFormaTelefonos(JSON.stringify(Telefono));
}

function SetAgregarCorreoEditar(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/AgregarCorreoEditar",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#dialogAgregarCorreoEditar").dialog("close");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function EliminaTelefonoContactoOrganizacion(pCliente) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EliminaTelefonoContactoOrganizacion",
		data: pCliente,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function EliminaCorreoContactoOrganizacion(pCliente) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EliminaCorreoContactoOrganizacion",
		data: pCliente,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function autocompletar() {
	$('#txtRazonSocial').autocomplete({
		source: function (request, response) {
			var pRequest = new Object();
			pRequest.pRazonSocial = $("#txtRazonSocial").val();
			$.ajax({
				type: 'POST',
				url: 'Cliente.aspx/RevisaExisteRazonSocial',
				data: JSON.stringify(pRequest),
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Table, function (item) {
						return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdOrganizacion }
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			var pIdOrganizacion = ui.item.id;
			var Organizacion = new Object();
			Organizacion.pIdOrganizacion = pIdOrganizacion;
			RevisaExisteOrganizacion(Organizacion);
		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});
}

function RevisaExisteOrganizacion(pRequest) {
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/RevisaExisteOrganizacion",
		data: JSON.stringify(pRequest),
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			var Cliente = new Object();
			var Organizacion = new Object();
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) { //Ya existe en mi sucursal, solo imprimir datos
				Cliente.IdCliente = parseInt(respuesta.Modelo.IdCliente);
				ObtenerFormaEditarCliente(JSON.stringify(Cliente));
				$("#dialogEditarCliente, #dialogAgregarCliente").dialog("close");
			}
			else if (respuesta.Modelo == "enrolar") {  //Ya existe como cliente, solo enrolar               
				Cliente.pIdCliente = parseInt(respuesta.IdCliente);
				ObtenerFormaConsultarClienteAEnrolar(JSON.stringify(Cliente));
			}
			else if (respuesta.Modelo == "agregarCliente") {  //Ya existe como proveedor, solo enrolar               
				Organizacion.pIdOrganizacion = parseInt(respuesta.IdOrganizacion);
				ObtenerFormaConsultarProveedorAEnrolar(JSON.stringify(Organizacion));
			}
			else { //No existe ni como proveedor ni como cliente                
				//Organizacion.pIdOrganizacion = parseInt(respuesta.IdOrganizacion);
				//ObtenerFormaAgregarCliente(JSON.stringify(Organizacion));         
			}

			if (respuesta.IdSucursalAlta != respuesta.IdSucursalActual) {
				$("#datosOrganizacion input, #datosOrganizacion select, #datosOrganizacion textarea").each(function () {
					$(this).attr("disabled", "disabled");
				});
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}



function RevisaExisteRFC() {
	var pRFC = $("#txtRFC").addClass('uppercase');
	pRFC = $("#txtRFC").val();
	if (pRFC != "") {
		var Organizacion = new Object();
		Organizacion.pRFC = pRFC;
		$.ajax({
			type: "POST",
			url: "Cliente.aspx/RevisaClienteRFC",
			data: JSON.stringify(Organizacion),
			dataType: "json",
			contentType: "application/json; charset=utf-8",
			success: function (pRespuesta) {
				var Cliente = new Object();
				var Organizacion = new Object();
				respuesta = jQuery.parseJSON(pRespuesta.d);

				if (respuesta.Error == 0) { //Ya existe en mi sucursal, solo imprimir datos
					Cliente.IdCliente = parseInt(respuesta.Modelo.IdCliente);
					ObtenerFormaEditarCliente(JSON.stringify(Cliente));
					$("#dialogEditarCliente, #dialogAgregarCliente").dialog("close");
				}
				else if (respuesta.Modelo == "enrolar") {  //Ya existe como cliente, solo enrolar               
					Cliente.pIdCliente = parseInt(respuesta.IdCliente);
					ObtenerFormaConsultarClienteAEnrolar(JSON.stringify(Cliente));
				}
				else if (respuesta.Modelo == "agregarCliente") {  //Ya existe como proveedor, solo enrolar               
					Organizacion.pIdOrganizacion = parseInt(respuesta.IdOrganizacion);
					ObtenerFormaConsultarProveedorAEnrolar(JSON.stringify(Organizacion));
				}
				else { //No existe ni como proveedor ni como cliente
					//Organizacion.pIdOrganizacion = parseInt(respuesta.IdOrganizacion);
					//ObtenerFormaAgregarCliente(JSON.stringify(Organizacion));
				}
				if (respuesta.IdSucursalAlta != respuesta.IdSucursalActual) {
					$("#datosOrganizacion input, #datosOrganizacion select, #datosOrganizacion textarea").each(function () {
						$(this).attr("disabled", "disabled");
					});
				}
			},
			complete: function () {
				OcultarBloqueo();
			}
		});
	}
}

function FiltroCuentaBancariaCliente() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdCuentaBancariaCliente').getGridParam('rowNum');
	request.pPaginaActual = $('#grdCuentaBancariaCliente').getGridParam('page');
	request.pColumnaOrden = $('#grdCuentaBancariaCliente').getGridParam('sortname');
	request.pTipoOrden = $('#grdCuentaBancariaCliente').getGridParam('sortorder');
	request.pIdCliente = 0;
	request.pIdBanco = 0;
	request.pIdTipoMoneda = 0;
	request.pAI = 0;
	request.pEsCliente = -1;

	if ($("#divFormaCuentasBancariasCliente").attr("IdCliente") != null) {
		request.pIdCliente = $("#divFormaCuentasBancariasCliente").attr("IdCliente");
	}

	if ($('#divContGridCuentaBancariaCliente').find(gs_AI).existe()) {
		request.pAI = $('#divContGridCuentaBancariaCliente').find(gs_AI).val();
	}

	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cliente.aspx/ObtenerCuentaBancariaCliente',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success')
			{ $('#grdCuentaBancariaCliente')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
			else
			{ alert(JSON.parse(jsondata.responseText).Message); }
		}
	});
}

function AgregarCuentaBancariaCliente() {
	var pCuentaBancariaCliente = new Object();
	pCuentaBancariaCliente.IdCliente = $("#divFormaAgregarCuentaBancariaCliente").attr("IdCliente");
	pCuentaBancariaCliente.IdBanco = $("#cmbBanco").val();
	pCuentaBancariaCliente.CuentaBancariaCliente = $("#txtCuentaBancariaCliente").val();
	pCuentaBancariaCliente.IdTipoMoneda = $("#cmbTipoMoneda").val();
	pCuentaBancariaCliente.Descripcion = $("#txtDescripcion").val();
	pCuentaBancariaCliente.IdMetodoPago = $("#cmbMetodoPago").val();

	var validacion = ValidaCuentaBancariaCliente(pCuentaBancariaCliente);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }
	var oRequest = new Object();
	oRequest.pCuentaBancariaCliente = pCuentaBancariaCliente;
	SetAgregarCuentaBancariaCliente(JSON.stringify(oRequest));
}

function SetAgregarCuentaBancariaCliente(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/AgregarCuentaBancariaCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCuentaBancariaCliente").trigger("reloadGrid");
				$("#dialogAgregarCuentaBancariaCliente").dialog("close")
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

function SetCambiarEstatusCuentaBancariaCliente(pIdCuentaBancariaCliente, pBaja) {
	var pRequest = "{'pIdCuentaBancariaCliente':" + pIdCuentaBancariaCliente + ", 'pBaja':" + pBaja + "}";
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/CambiarEstatusCuentaBancariaCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			$("#grdCuentaBancariaCliente").trigger("reloadGrid");
		},
		complete: function () {
			$('#grdCuentaBancariaCliente').one('click', '.div_grdCuentaBancariaCliente_AI', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancariaCliente_AI']").children().attr("baja")
				var idCuentaBancariaCliente = $(registro).children("td[aria-describedby='grdCuentaBancariaCliente_IdCuentaBancariaCliente']").html();
				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
				}
				SetCambiarEstatusCuentaBancariaCliente(idCuentaBancariaCliente, baja);
			});
		}
	});
}

function EditarCuentaBancariaCliente() {
	var pCuentaBancariaCliente = new Object();
	pCuentaBancariaCliente.IdCuentaBancariaCliente = $("#divFormaEditarCuentaBancariaCliente").attr("idCuentaBancariaCliente");
	pCuentaBancariaCliente.IdBanco = $("#cmbBanco").val();
	pCuentaBancariaCliente.IdTipoMoneda = $("#cmbTipoMoneda").val();
	pCuentaBancariaCliente.Descripcion = $("#txtDescripcion").val();
	pCuentaBancariaCliente.CuentaBancariaCliente = $("#txtCuentaBancariaCliente").val();
	pCuentaBancariaCliente.IdMetodoPago = $("#cmbMetodoPago").val();

	var validacion = ValidaCuentaBancariaCliente(pCuentaBancariaCliente);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }
	var oRequest = new Object();
	oRequest.pCuentaBancariaCliente = pCuentaBancariaCliente;
	SetEditarCuentaBancariaCliente(JSON.stringify(oRequest));
}

function SetEditarCuentaBancariaCliente(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/EditarCuentaBancariaCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCuentaBancariaCliente").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogEditarCuentaBancariaCliente").dialog("close");
		}
	});
}

function AgregarDescuentoCliente() {
	var pDescuentoCliente = new Object();
	pDescuentoCliente.IdCliente = $("#dialogConsultarDescuentoCliente").attr("idCliente");
	pDescuentoCliente.Descuento = $("#txtDescuentoCliente").val();
	pDescuentoCliente.DescripcionDescuento = $("#txtDescripcion").val();

	var validacion = ValidarDescuentoCliente(pDescuentoCliente);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	var oRequest = new Object();
	oRequest.pDescuentoCliente = pDescuentoCliente;
	SetAgregarDescuentoCliente(JSON.stringify(oRequest));
}

function SetAgregarDescuentoCliente(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/AgregarDescuentoCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdDescuentoCliente").trigger("reloadGrid");
				$("#dialogAgregarDescuentoCliente").dialog("close");
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

function FiltroDescuentoCliente() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdDescuentoCliente').getGridParam('rowNum');
	request.pPaginaActual = $('#grdDescuentoCliente').getGridParam('page');
	request.pColumnaOrden = $('#grdDescuentoCliente').getGridParam('sortname');
	request.pTipoOrden = $('#grdDescuentoCliente').getGridParam('sortorder');
	request.pIdCliente = 0;
	if ($("#divFormaDescuentosCliente").attr("IdCliente") != null) {
		request.pIdCliente = $("#divFormaDescuentosCliente").attr("IdCliente");
	}
	request.pBaja = -1
	if ($("#gs_AI_DescuentoCliente").existe()) {
		request.pBaja = $("#gs_AI_DescuentoCliente").val();
	}
	request.pEsCliente = -1;

	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cliente.aspx/ObtenerDescuentoCliente',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success')
			{ $('#grdDescuentoCliente')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
			else
			{ alert(JSON.parse(jsondata.responseText).Message); }
		}
	});
}

function ObtenerFormaAgregarDescuentoCliente() {
	$("#dialogAgregarDescuentoCliente").obtenerVista({
		nombreTemplate: "tmplAgregarDescuentoCliente.html",
		despuesDeCompilar: function (pRespuesta) {
			$("#dialogAgregarDescuentoCliente").dialog("open");
		}
	});
}

function SetCambiarEstatusDescuentoCliente(pIdDescuentoCliente, pBaja) {
	var pRequest = "{'pIdDescuentoCliente':" + pIdDescuentoCliente + ", 'pBaja':" + pBaja + "}";
	$.ajax({
		type: "POST",
		url: "Cliente.aspx/CambiarEstatusDescuentoCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdDescuentoCliente").trigger("reloadGrid");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
			}
		},
		complete: function () {
			$('#grdDescuentoCliente').one('click', '.div_grdDescuentoCliente_AI_DescuentoCliente', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdDescuentoCliente_AI_DescuentoCliente']").children().attr("baja")
				var idDescuentoCliente = $(registro).children("td[aria-describedby='grdDescuentoCliente_IdDescuentoCliente']").html();
				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
				}
				SetCambiarEstatusDescuentoCliente(idDescuentoCliente, baja);
			});
		}
	});
}

//########################################################################################
// Funcion Clientes con oportunidades
//########################################################################################

function ObtenerClientesOportunidades() {
	$.ajax({
		url: "Cliente.aspx/ObtenerClientesOportunidades",
		type: "post",
		data: ObtenerFiltros(),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			var pResult = JSON.parse(result.d);
			$("#ConOportunidades").text(pResult.Modelo.ConOportunidades);
			$("#SinOportunidades").text(pResult.Modelo.SinOportunidades);
		}
	});
}

function ObtenerFiltros() {
	var request = new Object();

	request.pRazonSocial = ($("#gs_RazonSocial").val() != null) ? $("#gs_RazonSocial").val() : "";
	request.pNombreComercial = ($("#gs_NombreComercial").val() != null) ? $("#gs_NombreComercial").val() : "";
	request.pRFC = ($("#gs_RFC").val() != null) ? $("#gs_RFC").val() : "";
	request.pEsCliente = ($("#gs_EsCliente").val() != null) ? parseInt($("#gs_EsCliente").val()) : -1;
	request.pAgente = ($("#gs_Agente").val() != null) ? $("#gs_Agente").val() : "";
	request.pIdTipoCliente = ($("#gs_IdTipoCliente").val() != null) ? parseInt($("#gs_IdTipoCliente").val()) : -1;
	request.pAI = ($("#gs_AI").val() != null) ? parseInt($("#gs_AI").val()) : -1;

	var pRequest = JSON.stringify(request);
	return pRequest;
}


//-----Validaciones------------------------------------------------------
function ValidaCliente(pCliente) {
	var errores = "";

	if (pCliente.RazonSocial == "")
	{ errores = errores + "<span>*</span> La razón social esta vacía, favor de capturarla.<br />"; }

	if (ValidarRazonSocial(pCliente.RazonSocial))
	{ errores = errores + "<span>*</span> La razón tiene caracteres invalidos, favor de revisarlo.<br />"; }

	if (pCliente.NombreComercial == "")
	{ errores = errores + "<span>*</span> El nombre comercial del cliente esta vacío, favor de capturarlo.<br />"; }

	if (pCliente.RFC == "")
	{ errores = errores + "<span>*</span> El RFC esta vacío, favor de capturarlo.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ValidaDireccion(pCliente) {
	var errores = "";

	if (pCliente.Descripcion == "")
	{ errores = errores + "<span>*</span> El campo descripción breve esta vacío, favor de capturarlo.<br />"; }

	if (pCliente.IdTipoDireccion == 0)
	{ errores = errores + "<span>*</span> El campo tipo de dirección esta vacío, favor de seleccionarlo.<br />"; }

	if (pCliente.Calle == "")
	{ errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

	if (pCliente.NumeroExterior == "")
	{ errores = errores + "<span>*</span> El campo número exterior esta vacío, favor de capturarlo.<br />"; }

	if (pCliente.Colonia == "")
	{ errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }


	if (pCliente.CodigoPostal == "")
	{ errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }

	if (pCliente.CodigoPostal != "") {
		if (ValidarCodigoPostal(pCliente.CodigoPostal))
		{ errores = errores + "<span>*</span> El código postal debe de tener 5 números<br />"; }
	}

	if (pCliente.IdPais == 0)
	{ errores = errores + "<span>*</span> El campo país esta vacío, favor de seleccionarlo.<br />"; }

	if (pCliente.IdEstado == 0)
	{ errores = errores + "<span>*</span> El campo estado esta vacío, favor de seleccionarlo.<br />"; }

	if (pCliente.IdMunicipio == 0)
	{ errores = errores + "<span>*</span> El campo municipio esta vacío, favor de seleccionarlo.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ValidarTelefono(pRequest) {
	var errores = "";

	if (pRequest.Descripcion == "")
	{ errores = errores + "<span>*</span> La descripción está vacia, favor de capturarla.<br />"; }

	if (pRequest.Telefono == "")
	{ errores = errores + "<span>*</span> El teléfono esta vacío, favor de capturarlo.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ValidarCorreoContacto(pCorreo) {
	var errores = "";

	if (pCorreo == "")
	{ errores = errores + "<span>*</span> El correo esta vacío, favor de capturarlo.<br />"; }

	if (pCorreo != "") {
		if (ValidarCorreo(pCorreo))
		{ errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
	}

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ValidaContacto(pCliente) {
	var errores = "";

	if (pCliente.Nombre == "")
	{ errores = errores + "<span>*</span> El campo nombre esta vacío, favor de capturarlo.<br />"; }

	if (pCliente.Puesto == "")
	{ errores = errores + "<span>*</span> El campo puesto esta vacío, favor de capturarlo.<br />"; }

	if (pCliente.Telefonos.length === 0 && pCliente.Correos.length === 0)
	{ errores = errores + "<span>*</span> Debe ingresar un teléfono o un correo, favor de capturarlo.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ValidaCuentaBancariaCliente(pCuentaBancariaCliente) {
	var errores = "";

	if (pCuentaBancariaCliente.CuentaBancariaCliente != "") {
		if (pCuentaBancariaCliente.CuentaBancariaCliente.length < 4) {
			errores = errores + "<span>*</span> La cuenta bancaria debe ser mayor a 4 digitos, favor de corregir.<br />";
		}
	}

	if (pCuentaBancariaCliente.IdBanco == 0)
	{ errores = errores + "<span>*</span> El banco esta vacio, favor de capturarlo.<br />"; }

	if (pCuentaBancariaCliente.IdTipoMoneda == 0)
	{ errores = errores + "<span>*</span> La moneda esta vacia, favor de capturarla.<br />"; }

	if (pCuentaBancariaCliente.Descripcion == "")
	{ errores = errores + "<span>*</span> La descripción esta vacia, favor de capturarla.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ValidarDescuentoCliente(pDescuentoCliente) {
	var errores = "";

	if (parseFloat(pDescuentoCliente.Descuento) == 0)
	{ errores = errores + "<span>*</span> El campo descuento esta vacío, favor de capturarlo.<br />"; }
	if (pDescuentoCliente.DescripcionDescuento == "")
	{ errores = errores + "<span>*</span> El campo descripción del descuento esta vacío, favor de capturarlo.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}
function RevisarPermisoRFC(evt, txt) {

	$.ajax({
		type: "POST",
		url: "Cliente.aspx/RevisaPermisoRFC",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {

			respuesta = jQuery.parseJSON(pRespuesta.d);
			alert(respuesta);
			if (respuesta.Modelo.Permisos.puedeInsertarCaracteresRaros == 0) {
				if (!ValidarLetraNumero(evt, txt)) {
					return false;
				}

			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function Termino_grdCliente() {
	ObtenerClientesOportunidades();
}

/**/
function FiltroCliente() {
	var idtipocliente = -1;
	if ($('#gbox_grdCliente #gs_IdTipoCliente').val() != null) {
		idtipocliente = $('#gbox_grdCliente #gs_IdTipoCliente').val();
	}
	var idtipoindustria = -1;
	if ($('#gbox_grdCliente #gs_TipoIndustria').val() != null) {
		idtipoindustria = $('#gbox_grdCliente #gs_TipoIndustria').val();
	}
	var nombrecomercial = '';
	if ($('#gbox_grdCliente #gs_NombreComercial').val() != null) {
		nombrecomercial = $('#gs_NombreComercial').val();
	}
	var razonsocial = '';
	if ($('#gbox_grdCliente #gs_RazonSocial').val() != null) {
		razonsocial = $('#gs_RazonSocial').val();
	}
	var rfc = '';
	if ($('#gbox_grdCliente #gs_RFC').val() != null) {
		rfc = $('#gs_RFC').val();
	}
	var agente = '';
	if ($('#gbox_grdCliente #gs_Agente').val() != null) {
		agente = $('#gs_Agente').val();
	}
	var escliente = -1;
	if ($('#gbox_grdCliente #gs_EsCliente').val() != null) {
		escliente = $('#gbox_grdCliente #gs_EsCliente').val();
	}
	var ai = 0;
	if ($('#gbox_grdCliente #gs_AI').val() != null) {
		ai = $('#gbox_grdCliente #gs_AI').val();
	}

	var todos = 0;
	if ($("#ckbVerTodosClinetes").prop("checked")) {
		todos = 1;
	}

	var Cliente = new Object();
	Cliente.pTamanoPaginacion = $('#grdCliente').getGridParam('rowNum');
	Cliente.pPaginaActual = $('#grdCliente').getGridParam('page');
	Cliente.pColumnaOrden = $('#grdCliente').getGridParam('sortname');
	Cliente.pTipoOrden = $('#grdCliente').getGridParam('sortorder');
	Cliente.pIdTipoCliente = idtipocliente;
	Cliente.pIdTipoIndustria = idtipoindustria;
	Cliente.pNombreComercial = nombrecomercial;
	Cliente.pRazonSocial = razonsocial;
	Cliente.pRFC = rfc;
	Cliente.pAgente = agente;
	Cliente.pEsCliente = escliente;
	Cliente.pAI = ai;
	Cliente.pVerTodos = todos;

	var Request = JSON.stringify(Cliente);

	$.ajax({
		url: 'Cliente.aspx/ObtenerCliente',
		data: Request,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') $('#grdCliente')[0].addJSONData(JSON.parse(jsondata.responseText).d);
			else alert(JSON.parse(jsondata.responseText).Message);
			$("tr", "#grdCliente").each(function (index, element) {
				var color = "#FFFFFF";
				var opt = $("td[aria-describedby=grdCliente_Oportunidad]", element).html();
				var act = $("td[aria-describedby=grdCliente_Actividades]", element).html();
				if (opt == 'SI' && act == 'SI') {
					color = "#99FF99";
				} else if (opt == 'SI' || act == 'SI') {
					color = "#FFFF99";
				}
				$("td", element).css("background-color", color);
			});
		}
	});
}



function FiltroOportunidadCliente() {
	var OportunidadesCliente = new Object();
	OportunidadesCliente.pTamanoPaginacion = $('#grdOportunidadCliente').getGridParam('rowNum');
	OportunidadesCliente.pPaginaActual = $('#grdOportunidadCliente').getGridParam('page');
	OportunidadesCliente.pColumnaOrden = $('#grdOportunidadCliente').getGridParam('sortname');
	OportunidadesCliente.pTipoOrden = $('#grdOportunidadCliente').getGridParam('sortorder');
	OportunidadesCliente.pIdCliente = parseInt($("#divMotrarOportunidadesCliente").attr("IdCliente"));
	OportunidadesCliente.pIdOportunidad = 0;
	OportunidadesCliente.pAgente = "";
	var Request = JSON.stringify(OportunidadesCliente);
	$.ajax({
		url: 'Cliente.aspx/ObtenerOportunidadesCliente',
		data: Request,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') $('#grdOportunidadCliente')[0].addJSONData(JSON.parse(jsondata.responseText).d);
			else alert(JSON.parse(jsondata.responseText).Message);
		}
	});
}

function initGridOportunidadesCliente() {
	$('#grdOportunidadCliente').jqGrid({
		datatype: function () {
			FiltroOportunidadCliente();
		},
		jsonReader: {
			root: 'Elementos',
			page: 'PaginaActual',
			total: 'NoPaginas',
			records: 'NoRegistros',
			repeatitems: true,
			cell: 'Row',
			id: 'IdOportunidad'
		},
		colModel: [{
			name: 'IdOportunidad',
			width: '80px',
			align: 'Center',
			label: '#',
			hidden: false,
			sortable: true,
			search: false
		}, {
			name: 'Oportunidaad',
			width: '200px',
			align: 'Center',
			label: 'Oportunidad',
			hidden: false,
			sortable: true,
			search: false
		}, {
			name: 'Agente',
			width: '200px',
			align: 'Center',
			label: 'Agente',
			hidden: false,
			sortable: true,
			search: false
		}, {
			name: 'IdNivelInteres',
			width: '80px',
			align: 'Center',
			label: 'Nivel Interes',
			hidden: false,
			sortable: true,
			search: false
		}, {
			name: 'Dias',
			width: '80px',
			align: 'Center',
			label: 'Dias',
			hidden: false,
			sortable: true,
			search: false
		}, {
			name: 'Monto',
			width: '120px',
			align: 'Center',
			label: 'Monto',
			hidden: false,
			sortable: true,
			search: false
		}],
		pager: '#pagOportunidadCliente',
		loadtext: 'Cargando datos...',
		recordtext: '{0} - {1} de {2} elementos',
		emptyrecords: 'No hay resultados',
		pgtext: 'Pág: {0} de {1}',
		rowNum: '15',
		rowList: [15, 30, 60],
		viewrecords: true,
		multiselect: false,
		sortname: 'IdOportunidad',
		sortorder: 'DESC',
		width: '800',
		height: '300',
		caption: 'Oportunidades de cliente',
		hiddengrid: false,
		gridComplete: function () {
			var ids = $('#grdOportunidadCliente').jqGrid('getDataIDs');
			for (var i = 0; i < ids.length; i++) { }
		}
	}).navGrid('#pagOportunidadCliente', {
		edit: false,
		add: false,
		search: false,
		del: false
	});
	$('#grdOportunidadCliente').jqGrid('filterToolbar', {
		searchOnEnter: true
	});
}


function ObtenerPolizasCliente(Request) {
	MostrarBloqueo();
	$("#dialogPolizasCliente").obtenerVista({
		url: "Cliente.aspx/ObtenerPolizasCliente",
		parametros: Request,
		nombreTemplate: "tmplPolizasCliente.html",
		despuesDeCompilar: function () {

			$("#tabsPolizasCliente", "#divPolizasCliente").tabs();

			$("#dialogPolizasCliente").dialog("open");

		}
	});
}



