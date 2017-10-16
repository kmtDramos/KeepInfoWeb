//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Proveedor");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarProveedor", function() {
        ObtenerFormaAgregarProveedor();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarDireccion", function() {
        ObtenerFormaAgregarDireccion();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarContacto", function() {
        ObtenerFormaAgregarContacto();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarOrganizacionIVA", function() {
        ObtenerFormaAgregarOrganizacionIVA();
    });

    $("#dialogAgregarContacto").on("click", "#divAgregarTelefono", function() {
        $("#dialogAgregarTelefono").obtenerVista({
            nombreTemplate: "tmplAgregarTelefono.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarTelefono").dialog("open");
            }
        });
    });

    $("#dialogAgregarContacto").on("click", "#divAgregarCorreo", function() {
        $("#dialogAgregarCorreo").obtenerVista({
            nombreTemplate: "tmplAgregarCorreo.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarCorreo").dialog("open");
            }
        });
    });

    $("#dialogEditarContacto").on("click", "#divAgregarTelefono", function() {
        $("#dialogAgregarTelefonoEditar").obtenerVista({
            nombreTemplate: "tmplAgregarTelefono.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarTelefonoEditar").dialog("open");
            }
        });
    });

    $("#dialogEditarContacto").on("click", "#divAgregarCorreo", function() {
        $("#dialogAgregarCorreoEditar").obtenerVista({
            nombreTemplate: "tmplAgregarCorreo.html",
            despuesDeCompilar: function() {
                $("#dialogAgregarCorreoEditar").dialog("open");
            }
        });
    });

    $("#dialogAgregarContacto").on("click", ".divEliminar", function() {
        $(this).parents('.liTelefono').slideUp('slow', function() {
            $(this).parents('.liTelefono').remove();
        });
    });

    $("#dialogAgregarContacto").on("click", ".divEliminar", function() {
        $(this).parents('.liCorreo').slideUp('slow', function() {
            $(this).parents('.liCorreo').remove();
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


    $("#grdProveedor").on("click", ".imgFormaConsultarProveedor", function() {
        var registro = $(this).parents("tr");
        var Proveedor = new Object();
        Proveedor.pIdProveedor = parseInt($(registro).children("td[aria-describedby='grdProveedor_IdProveedor']").html());
        ObtenerFormaConsultarProveedor(JSON.stringify(Proveedor));
    });

    $("#grdDirecciones").on("click", ".imgFormaConsultarDireccion", function() {
        var registro = $(this).parents("tr");
        var Direccion = new Object();
        Direccion.pIdDireccionOrganizacion = parseInt($(registro).children("td[aria-describedby='grdDirecciones_IdDireccionOrganizacion']").html());
        ObtenerFormaConsultarDireccion(JSON.stringify(Direccion));
    });

    $("#grdContactos").on("click", ".imgFormaConsultarContacto", function() {
        var registro = $(this).parents("tr");
        var Contacto = new Object();
        Contacto.pIdContactoOrganizacion = parseInt($(registro).children("td[aria-describedby='grdContactos_IdContactoOrganizacion']").html());
        ObtenerFormaConsultarContacto(JSON.stringify(Contacto));
    });

    $("#grdOrganizacionIVA").on("click", ".imgFormaConsultarIVA", function() {
        var registro = $(this).parents("tr");
        var OrganizacionIVA = new Object();
        OrganizacionIVA.pIdOrganizacionIVA = parseInt($(registro).children("td[aria-describedby='grdOrganizacionIVA_IdOrganizacionIVA']").html());
        ObtenerFormaConsultarOrganizacionIVA(JSON.stringify(OrganizacionIVA));
    });

    $("#grdProveedor").on("click", ".imgFormaDirecciones", function() {
        var registro = $(this).parents("tr");
        var Proveedor = new Object();
        Proveedor.pIdProveedor = parseInt($(registro).children("td[aria-describedby='grdProveedor_IdProveedor']").html());
        ObtenerFormaDirecciones(JSON.stringify(Proveedor));
    });

    $("#grdProveedor").on("click", ".imgFormaContactos", function() {
        var registro = $(this).parents("tr");
        var Proveedor = new Object();
        Proveedor.pIdProveedor = parseInt($(registro).children("td[aria-describedby='grdProveedor_IdProveedor']").html());
        ObtenerFormaContactos(JSON.stringify(Proveedor));
    });

    $("#grdProveedor").on("click", ".imgFormaOrganizacionIVA", function() {
        var registro = $(this).parents("tr");
        var Proveedor = new Object();
        Proveedor.pIdProveedor = parseInt($(registro).children("td[aria-describedby='grdProveedor_IdProveedor']").html());
        ObtenerFormaOrganizacionIVA(JSON.stringify(Proveedor));
    });

    $('#grdProveedor').one('click', '.div_grdProveedor_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdProveedor_AI']").children().attr("baja")
        var idProveedor = $(registro).children("td[aria-describedby='grdProveedor_IdProveedor']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idProveedor, baja);
    });

    $('#grdDirecciones').one('click', '.div_grdDirecciones_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdDirecciones_AI']").children().attr("baja")
        var IdDireccionOrganizacion = $(registro).children("td[aria-describedby='grdDirecciones_IdDireccionOrganizacion']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatusDireccion(IdDireccionOrganizacion, baja);
    });

    $('#grdContactos').one('click', '.div_grdContactos_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdContactos_AI']").children().attr("baja")
        var IdContactoOrganizacion = $(registro).children("td[aria-describedby='grdContactos_IdContactoOrganizacion']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatusContacto(IdContactoOrganizacion, baja);
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor, #dialogAgregarDireccion, #dialogEditarDireccion').on('change', '#cmbPais', function(event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstadosFiltro(JSON.stringify(request));

        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipiosFiltro(JSON.stringify(request));

        var request = new Object();
        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidadesFiltro(JSON.stringify(request));
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor, #dialogAgregarDireccion, #dialogEditarDireccion').on('change', '#cmbEstado', function(event) {
        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipiosFiltro(JSON.stringify(request));
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor, #dialogAgregarDireccion, #dialogEditarDireccion').on('change', '#cmbMunicipio', function(event) {
        var request = new Object();
        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidadesFiltro(JSON.stringify(request));
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor').on('focusin', '#txtIVAActual', function(event) {
        $(this).quitarValorPredeterminado("decimal");
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor').on('focusout', '#txtIVAActual', function(event) {
        $(this).valorPredeterminado("decimal");
    });

    $("#dialogAgregarContacto").on("click", ".divEliminarTelefono", function() {
        var registro = $(this).parents("li");
        $(registro).slideUp('slow', function() {
            $(registro).remove();
        });
    });

    $("#dialogAgregarContacto").on("click", ".divEliminarCorreo", function() {
        var registro = $(this).parents("li");
        $(registro).slideUp('slow', function() {
            $(registro).remove();
        });
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor').on('keypress', '#txtRFC', function(event) {
        if (!ValidarLetraNumero(event, $(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor').on('change', '#txtRFC', function(event) {
        RevisaExisteRFC();
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor').on('focusin', '#txtLimiteCredito', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });

    $('#dialogAgregarProveedor, #dialogEditarProveedor').on('focusout', '#txtLimiteCredito', function(event) {
        $(this).valorPredeterminado("Moneda");
    });

    $('#dialogAgregarDireccion').on('click', '#chkDireccionFiscal', function(event) {
        if ($(this).is(':checked')) {
            var idProveedor = $("#divFormaDireccionesOrganizacion").attr("idProveedor");
            if (idProveedor == 0) {
                MostrarMensajeError("Es necesario seleccionar al proveedor para obtener la dirección fiscal.");
                $(this).attr('checked', false);
            }
            else {
                $(this).attr('checked', 'checked');
                var request = new Object();
                request.pIdProveedor = idProveedor;
                var pRequest = JSON.stringify(request);
                $("#tblFormaAgregarDireccion").obtenerVista({
                    nombreTemplate: "tmplConsultarProveedor-DireccionFiscal.html",
                    url: "Proveedor.aspx/ObtenerDireccionFiscalProveedor",
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

    $('#dialogAgregarProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: '865',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
        },
        close: function() {
            //$("#divFormaAgregarProveedor").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarProveedor();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogAgregarDireccion').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
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
                $(this).dialog("close")
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

    $('#dialogAgregarOrganizacionIVA').dialog({
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
                AgregarOrganizacionIVA();
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

    $("#dialogAgregarTelefonoEditar").dialog({
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
                AgregarTelefonoEditar();
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
    $("#dialogAgregarCorreoEditar").dialog({
        autoOpen: false,
        height: "auto",
        width: "400",
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


    $('#dialogConsultarProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: '630',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarProveedor").remove();
        },
        buttons: {
            "Aceptar": function() {
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

    $('#dialogConsultarOrganizacionIVA').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarOrganizacionIVA").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogConsultarProveedorAEnrolar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarProveedorAEnrolar").remove();
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

    $('#dialogOrganizacionIVA').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaOrganizacionIVA").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogEditarProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: '865',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
        },
        close: function() {
            $("#divFormaEditarProveedor").remove();
            $("#divFormaAgregarProveedor").remove();
        },
        buttons: {
            "Editar": function() {
                EditarProveedor();
            },
            "Cancelar": function() {
                $(this).dialog("close")
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

    $('#dialogEditarOrganizacionIVA').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarOrganizacionIVA").remove();
        },
        buttons: {
            "Editar": function() {
                EditarOrganizacionIVA();
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
                EnrolarClienteAProveedor();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
});

function ObtenerListaEstadosFiltro(pRequest) {
    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaEstados",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaMunicipiosFiltro(pRequest) {
    $("#cmbMunicipio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaMunicipios",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaLocalidadesFiltro(pRequest) {
    $("#cmbLocalidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaLocalidades",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaEstado() {
    var request = new Object();
    request.pIdPais = $("#cmbPais").val();

    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaEstados",
        parametros: JSON.stringify(request),
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaMunicipio() {
    var request = new Object();
    request.pIdEstado = $("#cmbEstado").val();

    $("#cmbMunicipio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaMunicipios",
        parametros: JSON.stringify(request),
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaLocalidad() {
    var request = new Object();
    request.pIdMunicipio = $("#cmbMunicipio").val();

    $("#cmbLocalidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaLocalidades",
        parametros: JSON.stringify(request),
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}


//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarProveedor() {
    $("#dialogAgregarProveedor").obtenerVista({
        nombreTemplate: "tmplAgregarProveedor.html",
        url: "Proveedor.aspx/ObtenerFormaAgregarProveedor",
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarRazonSocial();
            //AutocompletarCuentaContable();
            $("#dialogAgregarProveedor").dialog("open");
            if (pRespuesta.modelo.Permisos.puedeAgregarProveedor == 0) {              
                BloquearClases("table#permiso tr");
            }
            
            if (pRespuesta.modelo.Permisos.puedeAgregarLimiteCreditoProveedor == 0) {              
                BloquearClases("table#permiso2 tr");
            }
        }
    });
}

function ObtenerFormaAgregarDireccion() {
    $("#dialogAgregarDireccion").obtenerVista({
        nombreTemplate: "tmplAgregarDireccion.html",
        url: "Proveedor.aspx/ObtenerFormaAgregarDireccion",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarDireccion").dialog("open");
        }
    });
}

function ObtenerFormaAgregarContacto() {
    $("#dialogAgregarContacto").obtenerVista({
        nombreTemplate: "tmplAgregarContacto.html",
        url: "Proveedor.aspx/ObtenerFormaAgregarContacto",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarContacto").dialog("open");
            $("#txtFechaCumpleanio").datepicker();
        }
    });
}

function ObtenerFormaAgregarOrganizacionIVA() {
    $("#dialogAgregarOrganizacionIVA").obtenerVista({
        nombreTemplate: "tmplAgregarOrganizacionIVA.html",
        url: "Proveedor.aspx/ObtenerFormaAgregarOrganizacionIVA",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarOrganizacionIVA").dialog("open");
        }
    });
}

function ObtenerFormaConsultarProveedor(pIdProveedor) {
    $("#dialogConsultarProveedor").obtenerVista({
        nombreTemplate: "tmplConsultarProveedor.html",
        url: "Proveedor.aspx/ObtenerFormaProveedor",
        parametros: pIdProveedor,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarProveedor == 1) {
                $("#dialogConsultarProveedor").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Proveedor = new Object();
                        Proveedor.IdProveedor = parseInt($("#divFormaConsultarProveedor").attr("IdProveedor"));
                        ObtenerFormaEditarProveedor(JSON.stringify(Proveedor))
                    }
                });
                $("#dialogConsultarProveedor").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarProveedor").dialog("option", "buttons", {});
                $("#dialogConsultarProveedor").dialog("option", "height", "500");
            }
            $("#dialogConsultarProveedor").dialog("open");
        }
    });
}

function ObtenerFormaConsultarDireccion(pIdDireccionOrganizacion) {
    $("#dialogConsultarDireccion").obtenerVista({
        nombreTemplate: "tmplConsultarDireccion.html",
        url: "Proveedor.aspx/ObtenerFormaDireccionOrganizacion",
        parametros: pIdDireccionOrganizacion,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarDireccion == 1) {
                $("#dialogConsultarDireccion").dialog("option", "buttons", {
                    "Editar": function() {
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

function ObtenerFormaConsultarContacto(pIdContactoOrganizacion) {
    $("#dialogConsultarContacto").obtenerVista({
        nombreTemplate: "tmplConsultarContacto.html",
        url: "Proveedor.aspx/ObtenerFormaContactoOrganizacion",
        parametros: pIdContactoOrganizacion,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarContactoProveedor == 1) {
                $("#dialogConsultarContacto").dialog("option", "buttons", {
                    "Editar": function() {
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

function ObtenerFormaConsultarOrganizacionIVA(pIdOrganizacionIVA) {
    $("#dialogConsultarOrganizacionIVA").obtenerVista({
        nombreTemplate: "tmplConsultarOrganizacionIVA.html",
        url: "Proveedor.aspx/ObtenerFormaIVA",
        parametros: pIdOrganizacionIVA,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarOrganizacionIVA == 1) {
                $("#dialogConsultarOrganizacionIVA").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var OrganizacionIVA = new Object();
                        OrganizacionIVA.pIdOrganizacionIVA = parseInt($("#divFormaConsultarOrganizacionIVA").attr("IdOrganizacionIVA"));
                        ObtenerFormaEditarOrganizacionIVA(JSON.stringify(OrganizacionIVA))
                    }
                });
                $("#dialogConsultarOrganizacionIVA").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarOrganizacionIVA").dialog("option", "buttons", {});
                $("#dialogConsultarOrganizacionIVA").dialog("option", "height", "300");
            }
            $("#dialogConsultarOrganizacionIVA").dialog("open");
        }
    });
}

function ObtenerFormaDirecciones(pIdProveedor) {
    $("#divFormaDirecciones").obtenerVista({
        nombreTemplate: "tmplDirecciones.html",
        url: "Proveedor.aspx/ObtenerFormaDirecciones",
        parametros: pIdProveedor,
        despuesDeCompilar: function(pRespuesta) {
            FiltroDirecciones();
            $("#dialogDirecciones").dialog("option", "buttons", {});
            $("#dialogDirecciones").dialog("open");
        }
    });
}

function ObtenerFormaContactos(pIdProveedor) {
    $("#divFormaContactos").obtenerVista({
        nombreTemplate: "tmplContactos.html",
        url: "Proveedor.aspx/ObtenerFormaContactos",
        parametros: pIdProveedor,
        despuesDeCompilar: function(pRespuesta) {
            FiltroContactos();
            $("#dialogContactos").dialog("option", "buttons", {});
            $("#dialogContactos").dialog("open");
        }
    });
}

function ObtenerFormaOrganizacionIVA(pIdProveedor) {
    $("#divFormaOrganizacionIVA").obtenerVista({
        nombreTemplate: "tmplOrganizacionIVA.html",
        url: "Proveedor.aspx/ObtenerFormaOrganizacionIVA",
        parametros: pIdProveedor,
        despuesDeCompilar: function(pRespuesta) {
            FiltroOrganizacionIVA();
            $("#dialogOrganizacionIVA").dialog("option", "buttons", {});
            $("#dialogOrganizacionIVA").dialog("open");
        }
    });
}

function ObtenerFormaEditarProveedor(IdProveedor) {
    //alert(IdProveedor);
    $("#dialogEditarProveedor").obtenerVista({
        nombreTemplate: "tmplEditarProveedor.html",
        url: "Proveedor.aspx/ObtenerFormaEditarProveedor",
        parametros: IdProveedor,
        despuesDeCompilar: function(pRespuesta) {
            //AutocompletarCuentaContable();
            $("#divFormaAgregarProveedor").remove();
            $("#dialogEditarProveedor").dialog("open");

//            alert(respuesta.modelo.Permisos.puedeEditarDatosOrganizacion);
//            alert(respuesta.modelo.IdUsuarioSucursalActual);
//            alert(respuesta.modelo.IdUsuarioSucursalDioDeAlta);
//            alert(respuesta.modelo.Permisos.puedeEditarDatosLimiteCredito);
//            alert(respuesta.modelo.Permisos.puedeEditarDatosFiscales);
//            
            if (respuesta.modelo.Permisos.puedeEditarDatosOrganizacion == 0) {
                BloquearClases("table#DatosOrganizacion tr");
            } else {
                if (respuesta.IdUsuarioSucursalActual != respuesta.IdUsuarioSucursalDioDeAlta) {
                    BloquearClases("table#DatosOrganizacion tr");
                }
            }

            if (respuesta.modelo.Permisos.puedeEditarDatosFiscales == 0) {
                BloquearClases("table#DatosFiscales tr");
            } else {
                if (respuesta.IdUsuarioSucursalActual != respuesta.IdUsuarioSucursalDioDeAlta) {
                    BloquearClases("table#DatosFiscales tr");
                }
            }

            if (respuesta.modelo.Permisos.puedeEditarDatosLimiteCredito == 0) {
                BloquearClases("table#DatosLimiteCredito tr");
            } else {
                if (respuesta.IdUsuarioSucursalActual != respuesta.IdUsuarioSucursalDioDeAlta) {
                    BloquearClases("table#DatosLimiteCredito tr");
                }
            }

            //            if (respuesta.IdSucursalAlta != respuesta.IdSucursalActual) {
            //                $("#datosOrganizacion input, #datosOrganizacion select, #datosOrganizacion textarea").each(function() {
            //                    $(this).attr("disabled", "disabled");
            //                });
            //            }


        }
    });
}

function ObtenerFormaEditarDireccion(IdDireccionOrganizacion) {
    $("#dialogEditarDireccion").obtenerVista({
        nombreTemplate: "tmplEditarDireccion.html",
        url: "Proveedor.aspx/ObtenerFormaEditarDireccion",
        parametros: IdDireccionOrganizacion,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarDireccion").dialog("open");
        }
    });
}

function ObtenerFormaEditarContacto(IdContactoOrganizacion) {
    $("#dialogEditarContacto").obtenerVista({
        nombreTemplate: "tmplEditarContacto.html",
        url: "Proveedor.aspx/ObtenerFormaEditarContacto",
        parametros: IdContactoOrganizacion,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarContacto").dialog("open");
            $("#txtFechaCumpleanio").datepicker();
        }
    });
}

function ObtenerFormaEditarOrganizacionIVA(pIdOrganizacionIVA) {
    $("#dialogEditarOrganizacionIVA").obtenerVista({
        nombreTemplate: "tmplEditarOrganizacionIVA.html",
        url: "Proveedor.aspx/ObtenerFormaIVA",
        parametros: pIdOrganizacionIVA,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarOrganizacionIVA").dialog("open");
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
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
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
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarProveedor() {
    var pProveedor = new Object();
    pProveedor.RazonSocial = $("#txtRazonSocial").val();
    pProveedor.NombreComercial = $("#txtNombreComercial").val();
    pProveedor.RFC = $("#txtRFC").val();
    pProveedor.Notas = $("#txtNotas").val();
    pProveedor.Dominio = $("#txtDominio").val();
    pProveedor.IdTipoIndustria = $("#cmbTipoIndustria").val();
    pProveedor.Calle = $("#txtCalle").val();
    pProveedor.NumeroExterior = $("#txtNumeroExterior").val();
    pProveedor.NumeroInterior = $("#txtNumeroInterior").val();
    pProveedor.Colonia = $("#txtColonia").val();
    pProveedor.CodigoPostal = $("#txtCodigoPostal").val();
    pProveedor.Conmutador = $("#txtConmutador").val();
    pProveedor.IdPais = $("#cmbPais").val();
    pProveedor.IdEstado = $("#cmbEstado").val();
    pProveedor.IdMunicipio = $("#cmbMunicipio").val();
    pProveedor.IdLocalidad = $("#cmbLocalidad").val();   
    pProveedor.Referencia = $("#txtReferencia").val();
    pProveedor.IdCondicionPago = $("#cmbCondicionPago").val();
    pProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pProveedor.IdGrupoEmpresarial = $("#cmbGrupoEmpresarial").val();
    pProveedor.IVAActual = $("#txtIVAActual").val();
    pProveedor.CuentaContable = $("#txtCuentaContable").val();
    pProveedor.CuentaContableDolares = $("#txtCuentaContableDolares").val();
    pProveedor.Correo = $("#txtCorreo").val();
    pProveedor.IdTipoGarantia = $("#cmbTipoGarantia").val();
    pProveedor.LimiteCredito = $("#txtLimiteCredito").val();
    
    var validacion = ValidaProveedor(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetAgregarProveedor(JSON.stringify(oRequest));
}

function AgregarDireccion() {
    var pProveedor = new Object();
    pProveedor.IdProveedor = $("#divFormaDireccionesOrganizacion").attr("idProveedor");
    pProveedor.DescripcionDireccion = $("#txtDescripcionDireccion").val();
    pProveedor.IdTipoDireccion = $("#cmbTipoDireccion").val();
    pProveedor.Calle = $("#txtCalle").val();
    pProveedor.NumeroExterior = $("#txtNumeroExterior").val();
    pProveedor.NumeroInterior = $("#txtNumeroInterior").val();
    pProveedor.Colonia = $("#txtColonia").val();
    pProveedor.CodigoPostal = $("#txtCodigoPostal").val();
    pProveedor.Conmutador = $("#txtConmutador").val();
    pProveedor.IdPais = $("#cmbPais").val();
    pProveedor.IdEstado= $("#cmbEstado").val();
    pProveedor.IdMunicipio = $("#cmbMunicipio").val();
    pProveedor.Referencia = $("#txtReferencia").val();
    pProveedor.IdLocalidad = $("#cmbLocalidad").val();
    var validacion = ValidaDireccion(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetAgregarDireccion(JSON.stringify(oRequest));
}

function AgregarContactoOrganizacion() {
    var pProveedor = new Object();
    pProveedor.IdProveedor = $("#divFormaContactosOrganizacion").attr("idProveedor");
    pProveedor.Nombre = $("#txtNombre").val();
    pProveedor.Puesto = $("#txtPuesto").val();
    pProveedor.Notas = $("#txtNotas").val();
    pProveedor.FechaCumpleanio = $("#txtFechaCumpleanio").val();

    pProveedor.Telefonos = new Array();
    $("#ulTelefonos li").each(function() {
        var pTelefono = new Object();
        pTelefono.Telefono = $(this).attr("telefono");
        pTelefono.Descripcion = $(this).attr("descripcion");
        pProveedor.Telefonos.push(pTelefono);
    });

    pProveedor.Correos = new Array();
    $("#ulCorreos li").each(function() {
        pProveedor.Correos.push($(this).attr("correo"));
    });

    var validacion = ValidaContacto(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetAgregarContacto(JSON.stringify(oRequest));
}

function AgregarOrganizacionIVA() {
    var pProveedor = new Object();
    pProveedor.IdProveedor = $("#divFormaOrganizacionesIVA").attr("idProveedor");
    pProveedor.IVA = $("#txtIVA").val();

    if ($("#chkEsPrincipal").is(':checked')) {
        pProveedor.EsPrincipal = 1;
    }
    else {
        pProveedor.EsPrincipal = 0;
    }
    
    var validacion = ValidaOrganizacionIVA(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetAgregarOrganizacionIVA(JSON.stringify(oRequest));
}

function SetAgregarProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/AgregarProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProveedor").trigger("reloadGrid");
                $("#dialogAgregarProveedor").dialog("close");
            }
            else if (respuesta.Descripcion == "valida") {
                ObtenerFormaConsultarProveedorAEnrolar(JSON.stringify(respuesta.Modelo));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetAgregarDireccion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/AgregarDireccion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
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
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetAgregarContacto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/AgregarContacto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdContactos").trigger("reloadGrid");
                $("#dialogAgregarContacto").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetAgregarOrganizacionIVA(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/AgregarOrganizacionIVA",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdOrganizacionIVA").trigger("reloadGrid");
                $("#dialogAgregarOrganizacionIVA").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
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

function AgregarTelefonoEditar() {
    var pTelefono = new Object();
    pTelefono.Telefono = $("#txtTelefono").val();
    pTelefono.Descripcion = $("#txtDescripcion").val();
    var validacion = ValidarTelefono(pTelefono);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var html = "<li class='liTelefono' telefono='" + pTelefono.Telefono + "' descripcion='" + pTelefono.Descripcion + "'>";
    html = html + "<table cellpadding='0' cellspacing='0'>";
    html = html + "<tr>";
    html = html + "<td style='width:370px'>";
    html = html + "<div style='overflow:auto;'>";
    html = html + "<div class='divDescripcion'>" + pTelefono.Telefono + " (" + pTelefono.Descripcion + ") </div>";
    html = html + "<div class='divEliminarEdicionTelefono' IdTelefonoContactoOrganizacion='${IdTelefonoContactoOrganizacion}'>X</div>";
    html = html + "</div>";
    html = html + "</td>";
    html = html + "</tr>";
    html = html + "</table>";
    html = html + "</li>";
    $("#ulListadoTelefonos").append(html);
    
    var pProveedor = new Object();
    pProveedor.IdContactoOrganizacion = $("#divFormaEditarContacto").attr("idContactoOrganizacion");
    pProveedor.Telefono = pTelefono.Telefono;
    pProveedor.Descripcion = pTelefono.Descripcion;
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetAgregarTelefonoEditar(JSON.stringify(oRequest));

    var Telefono = new Object();
    Telefono.IdContactoOrganizacion = parseInt($("#divFormaEditarContacto").attr("idContactoOrganizacion"));
    ObtenerFormaTelefonos(JSON.stringify(Telefono));
    $("#dialogAgregarTelefonoEditar").dialog("close");
}

function ObtenerFormaTelefonos(IdContactoOrganizacion) {
    $("#divTelefonosCorreos").obtenerVista({
        nombreTemplate: "tmplEditarTelefonosCorreos.html",
        url: "Proveedor.aspx/ObtenerFormaTelefonosCorreos",
        parametros: IdContactoOrganizacion,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function SetAgregarTelefonoEditar(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/AgregarTelefonoEditar",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogAgregarTelefonoEditar").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
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

function AgregarCorreoEditar() {
    var correo = $("#txtCorreo").val();
    var validacion = ValidarCorreoContacto(correo);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var html = "<li class='liCorreo' correo='" + correo + "'>";
    html = html + "<table cellpadding='0' cellspacing='0'>";
    html = html + "<tr>";
    html = html + "<td style='width:400px'>";
    html = html + "<div style='overflow:auto;'>";
    html = html + "<div class='divDescripcion'>" + correo + "</div>";
    html = html + "<div class='divEliminarEdicionCorreo'>X</div>";
    html = html + "</div>";
    html = html + "</td>";
    html = html + "</tr>";
    html = html + "</table>";
    html = html + "</li>";
    $("#ulCorreos").append(html);
    var pProveedor = new Object();
    pProveedor.IdContactoOrganizacion = $("#divFormaEditarContacto").attr("idContactoOrganizacion");
    pProveedor.Correo = correo;
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetAgregarCorreoEditar(JSON.stringify(oRequest));

    var Telefono = new Object();
    Telefono.IdContactoOrganizacion = parseInt($("#divFormaEditarContacto").attr("idContactoOrganizacion"));
    ObtenerFormaTelefonos(JSON.stringify(Telefono));
}

function SetAgregarCorreoEditar(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/AgregarCorreoEditar",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogAgregarCorreoEditar").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetCambiarEstatus(pIdProveedor, pBaja) {
    var pRequest = "{'pIdProveedor':" + pIdProveedor + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdProveedor").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdProveedor').one('click', '.div_grdProveedor_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdProveedor_AI']").children().attr("baja")
                var idProveedor = $(registro).children("td[aria-describedby='grdProveedor_IdProveedor']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idProveedor, baja);
            });
        }
    });
}

function SetCambiarEstatusDireccion(pIdDireccionOrganizacion, pBaja) {
    var pRequest = "{'pIdDireccionOrganizacion':" + pIdDireccionOrganizacion + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/CambiarEstatusDireccion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdDirecciones").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdDirecciones').one('click', '.div_grdDirecciones_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdDirecciones_AI']").children().attr("baja")
                var idDireccionOrganizacion = $(registro).children("td[aria-describedby='grdDirecciones_IdDireccionOrganizacion']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusDireccion(idDireccionOrganizacion, baja);
            });
        }
    });
}

function SetCambiarEstatusContacto(pIdContactoOrganizacion, pBaja) {
    var pRequest = "{'pIdContactoOrganizacion':" + pIdContactoOrganizacion + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/CambiarEstatusContacto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdContactos").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdContactos').one('click', '.div_grdContactos_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdContactos_AI']").children().attr("baja")
                var idContactoOrganizacion = $(registro).children("td[aria-describedby='grdContactos_IdContactoOrganizacion']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusContacto(idContactoOrganizacion, baja);
            });
        }
    });
}

function EditarProveedor() {
    var pProveedor = new Object();
    pProveedor.IdProveedor = $("#divFormaEditarProveedor").attr("idProveedor");
    pProveedor.RazonSocial = $("#txtRazonSocial").val();
    pProveedor.NombreComercial = $("#txtNombreComercial").val();
    pProveedor.RFC = $("#txtRFC").val();
    pProveedor.Notas = $("#txtNotas").val();
    pProveedor.Dominio = $("#txtDominio").val();
    pProveedor.IdTipoIndustria = $("#cmbTipoIndustria").val();
    pProveedor.Calle = $("#txtCalle").val();
    pProveedor.NumeroExterior = $("#txtNumeroExterior").val();
    pProveedor.NumeroInterior = $("#txtNumeroInterior").val();
    pProveedor.Colonia = $("#txtColonia").val();
    pProveedor.CodigoPostal = $("#txtCodigoPostal").val();
    pProveedor.Conmutador = $("#txtConmutador").val();
    pProveedor.IdPais = $("#cmbPais").val();
    pProveedor.IdEstado = $("#cmbEstado").val();
    pProveedor.IdMunicipio = $("#cmbMunicipio").val();
    pProveedor.IdLocalidad = $("#cmbLocalidad").val();
    pProveedor.Referencia = $("#txtReferencia").val();
    pProveedor.IdCondicionPago = $("#cmbCondicionPago").val();
    pProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pProveedor.IVAActual = $("#txtIVAActual").val();
    pProveedor.IdGrupoEmpresarial = $("#cmbGrupoEmpresarial").val();
    pProveedor.CuentaContable = $("#txtCuentaContable").val();
    pProveedor.CuentaContableDolares = $("#txtCuentaContableDolares").val();
    pProveedor.Correo = $("#txtCorreo").val();
    pProveedor.IdTipoGarantia = $("#cmbTipoGarantia").val();
    pProveedor.LimiteCredito = $("#txtLimiteCredito").val();
    
    var validacion = ValidaProveedor(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetEditarProveedor(JSON.stringify(oRequest));
}
function SetEditarProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/EditarProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProveedor").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarProveedor").dialog("close");
        }
    });
}

function EditarDireccion() {
    var pProveedor = new Object();
    pProveedor.IdDireccionOrganizacion = $("#divFormaEditarDireccion").attr("idDireccionOrganizacion");
    pProveedor.IdTipoDireccion = $("#cmbTipoDireccion").val();
    pProveedor.DescripcionDireccion = $("#txtDescripcionDireccion").val();
    pProveedor.Calle = $("#txtCalle").val();
    pProveedor.NumeroExterior = $("#txtNumeroExterior").val();
    pProveedor.NumeroInterior = $("#txtNumeroInterior").val();
    pProveedor.Colonia = $("#txtColonia").val();
    pProveedor.CodigoPostal = $("#txtCodigoPostal").val();
    pProveedor.Conmutador = $("#txtConmutador").val();
    pProveedor.IdPais = $("#cmbPais").val();
    pProveedor.IdEstado = $("#cmbEstado").val();
    pProveedor.IdMunicipio = $("#cmbMunicipio").val();
    pProveedor.Referencia = $("#txtReferencia").val();
    pProveedor.IdLocalidad = $("#cmbLocalidad").val();

    var validacion = ValidaDireccion(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetEditarDireccion(JSON.stringify(oRequest));
}
function SetEditarDireccion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/EditarDireccion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDirecciones").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarDireccion").dialog("close");
        }
    });
}

function EditarContacto() {
    var pProveedor = new Object();
    pProveedor.IdContactoOrganizacion = $("#divFormaEditarContacto").attr("idContactoOrganizacion");
    pProveedor.Nombre = $("#txtNombre").val();
    pProveedor.Puesto = $("#txtPuesto").val();
    pProveedor.Celular = $("#txtCelular").val();
    pProveedor.Fax = $("#txtFax").val();
    pProveedor.Notas = $("#txtNotas").val();
    pProveedor.FechaCumpleanio = $("#txtFechaCumpleanio").val();

    pProveedor.Telefonos = new Array();
    $("#ulTelefonos li").each(function() {
        var pTelefono = new Object();
        pTelefono.Telefono = $(this).attr("telefono");
        pTelefono.Descripcion = $(this).attr("descripcion");
        pProveedor.Telefonos.push(pTelefono);
    });

    pProveedor.Correos = new Array();
    $("#ulCorreos li").each(function() {
        pProveedor.Correos.push($(this).attr("correo"));
    });

    var validacion = ValidaContacto(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetEditarContacto(JSON.stringify(oRequest));
}

function EditarOrganizacionIVA() {

    var pProveedor = new Object();
    pProveedor.IdOrganizacionIVA = $("#divFormaEditarOrganizacionIVA").attr("idOrganizacionIVA");
    pProveedor.IdProveedor = $("#divFormaOrganizacionesIVA").attr("idProveedor");
    pProveedor.IVA = $("#txtIVA").val();

    if ($("#chkEsPrincipal").is(':checked')) {
        pProveedor.EsPrincipal = 1;
    }
    else {
        pProveedor.EsPrincipal = 0;
    }

    var validacion = ValidaOrganizacionIVA(pProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProveedor = pProveedor;
    SetEditarOrganizacionIVA(JSON.stringify(oRequest));
}

function SetEditarContacto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/EditarContacto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdContactos").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarContacto").dialog("close");
        }
    });
}

function SetEditarOrganizacionIVA(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/EditarOrganizacionIVA",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdOrganizacionIVA").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarOrganizacionIVA").dialog("close");
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
        url: "Proveedor.aspx/EnrolarProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProveedor").trigger("reloadGrid");
                $("#dialogAgregarProveedor, #dialogConsultarPorveedor").dialog("close");
                var Proveedor = new Object();
                Proveedor.pIdProveedor = respuesta.IdProveedor;
                ObtenerFormaConsultarProveedor(JSON.stringify(Proveedor));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarProveedorAEnrolar").dialog("close");
        }
    });
}

function FiltroDirecciones() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDirecciones').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDirecciones').getGridParam('page');
    request.pColumnaOrden = $('#grdDirecciones').getGridParam('sortname');
    request.pTipoOrden = $('#grdDirecciones').getGridParam('sortorder');
    request.pIdProveedor = 0;
    request.pAI = -1;
    
    if ($("#divFormaDireccionesOrganizacion").attr("IdProveedor") != null) {
        request.pIdProveedor = $("#divFormaDireccionesOrganizacion").attr("IdProveedor");
    }

    if ($('#divContGridDireccion').find(gs_AI).existe()) {
        request.pAI = $('#divContGridDireccion').find(gs_AI).val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Proveedor.aspx/ObtenerDirecciones',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDirecciones')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroContactos() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdContactos').getGridParam('rowNum');
    request.pPaginaActual = $('#grdContactos').getGridParam('page');
    request.pColumnaOrden = $('#grdContactos').getGridParam('sortname');
    request.pTipoOrden = $('#grdContactos').getGridParam('sortorder');
    request.pIdProveedor = 0;
    request.pAI = -1;
    
    if ($("#divFormaContactosOrganizacion").attr("IdProveedor") != null) {
        request.pIdProveedor = $("#divFormaContactosOrganizacion").attr("IdProveedor");
    }

    if ($('#divContGridContacto').find(gs_AI).existe()) {
        request.pAI = $('#divContGridContacto').find(gs_AI).val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Proveedor.aspx/ObtenerContactos',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdContactos')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroOrganizacionIVA() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOrganizacionIVA').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOrganizacionIVA').getGridParam('page');
    request.pColumnaOrden = $('#grdOrganizacionIVA').getGridParam('sortname');
    request.pTipoOrden = $('#grdOrganizacionIVA').getGridParam('sortorder');
    request.pIdProveedor = 0;
    request.pAI = -1;

    if ($("#divFormaOrganizacionIVA").attr("IdProveedor") != null) {
        request.pIdProveedor = $("#divFormaOrganizacionIVA").attr("IdProveedor");
    }

    if ($('#divContGridOrganizacionIVA').find(gs_AI).existe()) {
        request.pAI = $('#divContGridOrganizacionIVA').find(gs_AI).val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Proveedor.aspx/ObtenerOrganizacionIVA',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdOrganizacionIVA')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function AutocompletarCuentaContable() {

    $('#txtCuentaContable').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pCuentaContable = $('#txtCuentaContable').val();
            $.ajax({
                type: 'POST',
                url: 'Proveedor.aspx/BuscarCuentaContable',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarProveedor, #divFormaEditarProveedor").attr("idCuentaContable", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.CuentaContable, value: item.CuentaContable, id: item.IdCuentaContable}
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdCuentaContable = ui.item.id;
            $("#divFormaAgregarProveedor, #divFormaEditarProveedor").attr("idCuentaContable", pIdCuentaContable);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarRazonSocial(){
    $('#txtRazonSocial').autocomplete({
	    source: function( request, response ) 
	    {
	        var pRequest = new Object();
		    pRequest.pRazonSocial = $("#txtRazonSocial").val();
		    $.ajax({
			    type: 'POST',
			    //url: 'Proveedor.aspx/BuscarRazonSocial',
			    url: 'Proveedor.aspx/RevisaExisteRazonSocial',
			    data: JSON.stringify(pRequest),
			    dataType: 'json',
			    contentType: 'application/json; charset=utf-8',
			    success: function(pRespuesta){
				    var json = jQuery.parseJSON(pRespuesta.d);
				    response($.map(json.Table, function(item){
					    return {label:item.RazonSocial, value: item.RazonSocial, id: item.IdOrganizacion}
				    }));
			    }
		    });
	    },
	    minLength: 2,
	    select: function(event, ui) {
		    var Organizacion = new Object();
            Organizacion.pIdOrganizacion = ui.item.id;
            //ObtenerProveedorExistente(JSON.stringify(Organizacion));
            RevisaExisteOrganizacion(Organizacion);//AGREGADO POR MIKE
	    },
	    change: function(event, ui) {},
	    open: function() {$(this).removeClass("ui-corner-all").addClass("ui-corner-top");},
	    close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all");}
    });
}

function RevisaExisteOrganizacion(pRequest) {
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/RevisaExisteOrganizacion",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var Proveedor = new Object();
            var Organizacion = new Object();
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) { //Ya existe en mi sucursal, solo imprimir datos
                Proveedor.IdProveedor = parseInt(respuesta.Modelo.IdProveedor);
                ObtenerFormaEditarProveedor(JSON.stringify(Proveedor));
                $("#dialogEditarProveedor, #dialogAgregarProveedor").dialog("close");
            }
            else if (respuesta.Modelo == "enrolar") {  //Ya existe como proveedor, solo enrolar
                Proveedor.pIdProveedor = parseInt(respuesta.IdProveedor);
                ObtenerFormaConsultarProveedorAEnrolar(JSON.stringify(Proveedor));
            }
            else if (respuesta.Modelo == "agregarProveedor") {  //Ya existe como cliente, solo enrolar
                Organizacion.IdOrganizacion = parseInt(respuesta.IdOrganizacion);
                ObtenerFormaConsultarClienteAEnrolar(JSON.stringify(Organizacion));
            }
            else { //No Existe como proveedor ni como cliente
                Organizacion.pIdOrganizacion = parseInt(respuesta.IdOrganizacion);
                ObtenerFormaAgregarProveedor(JSON.stringify(Organizacion));

                if (respuesta.IdSucursalAlta != respuesta.IdSucursalActual) {
                    $("#datosOrganizacion input, #datosOrganizacion select, #datosOrganizacion textarea").each(function() {
                        $(this).attr("disabled", "disabled");
                    });
                }
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}


function ObtenerProveedorExistente(pRequest){
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/ObtenerProveedorExistente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var Proveedor = new Object();
            var Organizacion = new Object();
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                Proveedor.IdProveedor = parseInt(respuesta.Modelo.IdProveedor);
                ObtenerFormaEditarProveedor(JSON.stringify(Proveedor));
                $("#dialogEditarProveedor, #dialogAgregarProveedor").dialog("close");
            }
            else if (respuesta.Modelo == "enrolar") {
                var Proveedor = new Object();
                Proveedor.IdProveedor = parseInt(respuesta.IdProveedor);
                ObtenerFormaConsultarProveedorAEnrolar(JSON.stringify(Proveedor));
            }
            else {
                Organizacion.pIdOrganizacion = parseInt(respuesta.IdOrganizacion);
                ObtenerFormaAgregarProveedor(JSON.stringify(Organizacion));
                
                if(respuesta.IdSucursalAlta != respuesta.IdSucursalActual ){
                    $("#datosOrganizacion input, #datosOrganizacion select, #datosOrganizacion textarea").each(function(){
                        $(this).attr("disabled","disabled");
                    });
                }
            }
        },    
        complete: function() {
            OcultarBloqueo();
        }
    });   
}

//-----Validaciones------------------------------------------------------
function ValidaProveedor(pProveedor) {
    var errores = "";

    if (pProveedor.RazonSocial == "")
    { errores = errores + "<span>*</span> La razón social esta vacía, favor de capturarla.<br />"; }
    
    if (pProveedor.NombreComercial == "")
    { errores = errores + "<span>*</span> El nombre comercial del proveedor esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.RFC == "")
    { errores = errores + "<span>*</span> El RFC esta vacío, favor de capturarlo.<br />"; }
    
    if (pProveedor.IdTipoIndustria == 0)
    { errores = errores + "<span>*</span> El campo tipo de industria esta vacío, favor de seleccionarlo.<br />"; }
    
    if (pProveedor.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacío, favor de seleccionarlo.<br />"; }

    if (pProveedor.Calle == "")
    { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.NumeroExterior == "")
    { errores = errores + "<span>*</span> El campo número exterior esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.Colonia == "")
    { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.CodigoPostal == "")
    { errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.IdPais == 0)
    { errores = errores + "<span>*</span> El campo país esta vacío, favor de seleccionarlo.<br />"; }
    
    if (pProveedor.IdEstado == 0)
    { errores = errores + "<span>*</span> El campo estado esta vacío, favor de seleccionarlo.<br />"; }
    
    if (pProveedor.IdMunicipio == 0)
    { errores = errores + "<span>*</span> El campo municipio esta vacío, favor de seleccionarlo.<br />"; }
    
    if (pProveedor.IdLocalidad == 0)
    { errores = errores + "<span>*</span> El campo localidad esta vacío, favor de seleccionarlo.<br />"; }

    if (pProveedor.RFC != "") {
        if (RFCValidoEmpresarial(pProveedor.RFC) == false)
        { errores = errores + "<span>*</span> El formato del RFC no es valido, favor de capturar un RFC valido.<br />"; }
    }
   
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }
  
    return errores;
}


function ValidaDireccion(pProveedor) {
    var errores = "";

    if (pProveedor.IdTipoDireccion == 0)
    { errores = errores + "<span>*</span> El campo tipo de dirección esta vacío, favor de seleccionarlo.<br />"; }
    
    if (pProveedor.DescripcionDireccion == "")
    { errores = errores + "<span>*</span> El campo descripción breve esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.Calle == "")
    { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.NumeroExterior == "")
    { errores = errores + "<span>*</span> El campo número exterior esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.Colonia == "")
    { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.CodigoPostal == "")
    { errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }
    
    if (pProveedor.IdMunicipio == 0)
    { errores = errores + "<span>*</span> El campo municipio esta vacío, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}


function ValidaContacto(pProveedor) {
    var errores = "";

    if (pProveedor.Nombre == "")
    { errores = errores + "<span>*</span> El campo nombre esta vacío, favor de capturarlo.<br />"; }
    
    if (pProveedor.Puesto == "")
    { errores = errores + "<span>*</span> El campo puesto esta vacío, favor de capturarlo.<br />"; }

    if (pProveedor.Telefonos.length === 0 && pProveedor.Correos.length === 0)
    { errores = errores + "<span>*</span> Debe ingresar un teléfono o un correo, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaOrganizacionIVA(pProveedor) {
    var errores = "";

    if (pProveedor.IVA == 0)
    { errores = errores + "<span>*</span> El campo IVA esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarTelefono(pTelefono) {
    var errores = "";

    if (pTelefono.Descripcion == "")
    { errores = errores + "<span>*</span> El teléfono esta vac&iacute;o, favor de capturarlo.<br />"; }
    
    if (pTelefono.Telefono == "")
    { errores = errores + "<span>*</span> El teléfono esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarCorreoContacto(pCorreo) {
    var errores = "";

    if (pCorreo == "")
    { errores = errores + "<span>*</span> El correo esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pCorreo != "") {
        if (ValidarCorreo(pCorreo))
        { errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
    }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ObtenerListaTipoIndustria() {
    $("#cmbTipoIndustria").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaTiposIndustrias"
    });
}

function ObtenerListaCondicionPago() {
    $("#cmbCondicionPago").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaCondicionesPago"
    });
}

function ObtenerGrupoEmpresarial() {
    $("#cmbGrupoEmpresarial").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proveedor.aspx/ObtenerListaGrupoEmpresarial"
    });
}

function EnrolarClienteAProveedor() {
    var pCliente = new Object();
    pCliente.IdCliente = $("#divFormaConsultarClienteAEnrolar").attr("idCliente");
    var oRequest = new Object();
    oRequest.pCliente = pCliente;
    SetEnrolarClienteAProveedor(JSON.stringify(oRequest));
}

function SetEnrolarClienteAProveedor(oRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proveedor.aspx/EnrolarClienteAProveedor",
        data: oRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProveedor").trigger("reloadGrid");
                $("#dialogAgregarProveedor, #dialogConsultarProveedor").dialog("close");
                var Proveedor = new Object();
                Proveedor.pIdProveedor = respuesta.IdProveedor;
                ObtenerFormaConsultarProveedor(JSON.stringify(Proveedor));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarClienteAEnrolar").dialog("close");
        }
    });
}

function ObtenerFormaConsultarCliente(pIdCliente) {
    $("#dialogConsultarCliente").obtenerVista({
        nombreTemplate: "tmplConsultarCliente.html",
        url: "Proveedor.aspx/ObtenerFormaConsultarCliente",
        parametros: pIdCliente,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCliente == 1 && pRespuesta.modelo.Permisos.diferenteSucursal == 0) {
                $("#dialogConsultarCliente").dialog("option", "buttons", {
                    "Editar": function() {
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


function ObtenerFormaConsultarProveedorAEnrolar(pIdProveedor) {
    $("#dialogConsultarProveedorAEnrolar").obtenerVista({
        nombreTemplate: "tmplConsultarProveedorAEnrolar.html",
        url: "Proveedor.aspx/ObtenerFormaProveedorAEnrolar",
        parametros: pIdProveedor,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarProveedorAEnrolar").dialog("open");
        }
    });
}



function ObtenerFormaConsultarClienteAEnrolar(IdOrganizacion) {
    $("#dialogConsultarClienteAEnrolar").obtenerVista({
        nombreTemplate: "tmplConsultarClienteAEnrolar.html",
        url: "Proveedor.aspx/ObtenerFormaClienteAEnrolar",
        parametros: IdOrganizacion,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarClienteAEnrolar").dialog("open");
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
            url: "Proveedor.aspx/RevisaProveedorRFC",
            data: JSON.stringify(Organizacion),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                var Proveedor = new Object();
                var Organizacion = new Object();
                respuesta = jQuery.parseJSON(pRespuesta.d);

                if (respuesta.Error == 0) { //Ya existe en mi sucursal, solo imprimir datos
                    Proveedor.IdProveedor = parseInt(respuesta.Modelo.IdProveedor);
                    ObtenerFormaEditarProveedor(JSON.stringify(Proveedor));
                    $("#dialogEditarProveedor, #dialogAgregarProveedor").dialog("close");
                }
                else if (respuesta.Modelo == "enrolar") {  //Ya existe como proveedor, solo enrolar
                    Proveedor.pIdProveedor = parseInt(respuesta.IdProveedor);
                    ObtenerFormaConsultarProveedorAEnrolar(JSON.stringify(Proveedor));
                }
                else if (respuesta.Modelo == "agregarProveedor") {  //Ya existe como cliente, solo enrolar
                    Organizacion.IdOrganizacion = parseInt(respuesta.IdOrganizacion);
                    ObtenerFormaConsultarClienteAEnrolar(JSON.stringify(Organizacion));
                }
                else { //No Existe como proveedor ni como cliente


                }
                
                if (respuesta.IdSucursalAlta != respuesta.IdSucursalActual) {
                    $("#datosOrganizacion input, #datosOrganizacion select, #datosOrganizacion textarea").each(function() {
                        $(this).attr("disabled", "disabled");
                    });
                }
            },
            complete: function() {
                OcultarBloqueo();
            }
        });
    }
}
