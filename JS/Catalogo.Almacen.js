//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarAlmacen", function() {
        ObtenerFormaAgregarAlmacen();
    });

    $('#grdAlmacen').one('click', '.div_grdAlmacen_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdAlmacen_AI']").children().attr("baja")
        var idAlmacen = $(registro).children("td[aria-describedby='grdAlmacen_IdAlmacen']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idAlmacen, baja);
    });

    $("#grdAlmacen").on("click", ".imgFormaConsultarAlmacen", function() {
        var registro = $(this).parents("tr");
        var Almacen = new Object();
        Almacen.pIdAlmacen = parseInt($(registro).children("td[aria-describedby='grdAlmacen_IdAlmacen']").html());
        ObtenerFormaConsultarAlmacen(JSON.stringify(Almacen))
    });

    $("#grdAlmacen").on("click", ".imgFormaSucursalAsignada", function() {
        var registro = $(this).parents("tr");
        var idAlmacen = $(registro).children("td[aria-describedby='grdAlmacen_IdAlmacen']").html();
        var oRequest = new Object();
        oRequest.pIdAlmacen = idAlmacen;
        ObtenerFormaSucursalAsignada(JSON.stringify(oRequest));
    });

    $("#dialogConsultarSucursalAsignada").on("click", "#ulSucursalesDisponibles li", function() {
        var registro = $(this);
        var Sucursal = new Object();
        Sucursal.IdSucursal = $(registro).attr("idSucursal");
        Sucursal.Sucursal = $(registro).attr("sucursal");
        AgregarEnrolamiento(registro, Sucursal);
    });

    $("#dialogConsultarSucursalAsignada").on("click", ".Eliminar", function() {
        var registro = $(this).parents("li");
        var Sucursal = new Object();
        Sucursal.IdSucursal = $(registro).attr("idSucursal");
        Sucursal.Sucursal = $(registro).attr("sucursal");
        EliminarEnrolamiento(registro, Sucursal);
    });

    $("#dialogConsultarSucursalAsignada").on("click", "#divSucursalesDisponibles", function() {
        TodasSucursalesAAsignadas();
    });

    $("#dialogConsultarSucursalAsignada").on("click", "#divSucursalesAsignadas", function() {
        TodasSucursalesADisponibles();
    });

    $('#dialogAgregarAlmacen').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarAlmacen").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarAlmacen();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarAlmacen').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarAlmacen").remove();
        },
        buttons: {
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogEditarAlmacen').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarAlmacen").remove();
        },
        buttons: {
            "Editar": function() {
                EditarAlmacen();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarSucursalAsignada').dialog({
        autoOpen: false,
        height: '600',
        width: '832',
        modal: true,
        draggable: true,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function(event, ui) {
        },
        buttons: {
            "Guardar cambios": function() {
                    AgregarSucursalAsignadaAlAlmacen();
            }
        }
    });


    $('#dialogAgregarAlmacen, #dialogEditarAlmacen').on('change', '#cmbPais', function(event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstados(JSON.stringify(request));

        var request = new Object();
        request.pIdEstado = 0;
        ObtenerListaMunicipios(JSON.stringify(request));
    });

    $('#dialogAgregarAlmacen, #dialogEditarAlmacen').on('change', '#cmbEstado', function(event) {
        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipios(JSON.stringify(request));
    });
});


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarAlmacen() {
    var pAlmacen = new Object();
    pAlmacen.Almacen = $("#txtAlmacen").val();
    pAlmacen.IdEmpresa = $("#cmbEmpresa").val();
    pAlmacen.Telefono = $("#txtTelefono").val();
    pAlmacen.Correo = $("#txtCorreo").val();
    pAlmacen.Calle = $("#txtCalle").val();
    pAlmacen.NumeroExterior = $("#txtNumeroExterior").val();
    pAlmacen.NumeroInterior = $("#txtNumeroInterior").val();
    pAlmacen.IdPais = $("#cmbPais").val();
    pAlmacen.IdEstado = $("#cmbEstado").val();
    pAlmacen.Colonia = $("#txtColonia").val();
    pAlmacen.IdMunicipio = $("#cmbMunicipio").val();
    pAlmacen.CodigoPostal = $("#txtCodigoPostal").val();
    pAlmacen.IdTipoAlmacen = $("#cmbTipoAlmacen").val();
    pAlmacen.DisponibleVenta = $("#chkDisponibleVenta").is(':checked');
    
    var validacion = ValidaAlmacen(pAlmacen);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAlmacen = pAlmacen;
    SetAgregarAlmacen(JSON.stringify(oRequest));
}

function SetAgregarAlmacen(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Almacen.aspx/AgregarAlmacen",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAlmacen").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarAlmacen").dialog("close");
        }
    });
}

function EditarAlmacen() {
    var pAlmacen = new Object();
    pAlmacen.IdAlmacen = $("#divFormaEditarAlmacen").attr("IdAlmacen");
    pAlmacen.Almacen = $("#txtAlmacen").val();
    pAlmacen.IdEmpresa = $("#cmbEmpresa").val();
    pAlmacen.Telefono = $("#txtTelefono").val();
    pAlmacen.Correo = $("#txtCorreo").val();
    pAlmacen.Calle = $("#txtCalle").val();
    pAlmacen.NumeroExterior = $("#txtNumeroExterior").val();
    pAlmacen.NumeroInterior = $("#txtNumeroInterior").val();
    pAlmacen.IdPais = $("#cmbPais").val();
    pAlmacen.IdEstado = $("#cmbEstado").val();
    pAlmacen.Colonia = $("#txtColonia").val();
    pAlmacen.IdMunicipio = $("#cmbMunicipio").val();
    pAlmacen.CodigoPostal = $("#txtCodigoPostal").val();
    pAlmacen.IdTipoAlmacen = $("#cmbTipoAlmacen").val();
    pAlmacen.DisponibleVenta = $("#chkDisponibleVentaEdicion").is(':checked');
    
    var validacion = ValidaAlmacen(pAlmacen);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAlmacen = pAlmacen;
    SetEditarAlmacen(JSON.stringify(oRequest));
}

function SetEditarAlmacen(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Almacen.aspx/EditarAlmacen",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAlmacen").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarAlmacen").dialog("close");
        }
    });
}

function AgregarSucursalAsignadaAlAlmacen() {
    var pSucursal = new Object();
    var Sucursales = [];
    var Contador = 0;

    pSucursal.IdAlmacen = $("#divFormaAsignarSucursalesAlAlmacen").attr("idAlmacen");

    $("#ulSucursalesAsignadas li").each(function(i, e) {
        Contador += 1;
        var Sucursal = new Object();
        var registro = $(this);
        Sucursal.IdSucursal = $(registro).attr("idSucursal");
        Sucursales.push(Sucursal);
    });
    var oRequest = new Object();
    pSucursal.Sucursales = Sucursales;
    oRequest.pSucursal = pSucursal;
    SetAgregarSucursalAlmacen(JSON.stringify(oRequest))
}


function SetAgregarSucursalAlmacen(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Almacen.aspx/AgregarSucursalAlmacen",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogConsultarSucursalAsignada").dialog("close");    
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarSucursalAsignada").dialog("close");
        }
    });
}


function SetCambiarEstatus(pIdAlmacen, pBaja) {
    var pRequest = "{'pIdAlmacen':" + pIdAlmacen + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Almacen.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdAlmacen").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdAlmacen').one('click', '.div_grdAlmacen_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdAlmacen_AI']").children().attr("baja")
                var idAlmacen = $(registro).children("td[aria-describedby='grdAlmacen_IdAlmacen']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idAlmacen, baja);
            });
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarAlmacen() {
    $("#dialogAgregarAlmacen").obtenerVista({
        nombreTemplate: "tmplAgregarAlmacen.html",
        url: "Almacen.aspx/ObtenerFormaAgregarAlmacen",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarAlmacen").dialog("open");
        }
    });
}

function ObtenerFormaEditarAlmacen(pAlmacen) {
    $("#dialogEditarAlmacen").obtenerVista({
        nombreTemplate: "tmplEditarAlmacen.html",
        url: "Almacen.aspx/ObtenerFormaEditarAlmacen",
        parametros: pAlmacen,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarAlmacen").dialog("open");
        }
    });
}

function ObtenerListaEstados(pRequest) {
    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Almacen.aspx/ObtenerListaEstados",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerListaMunicipios(pRequest) {
    $("#cmbMunicipio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Almacen.aspx/ObtenerListaMunicipios",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerFormaConsultarAlmacen(pAlmacen) {
    $("#dialogConsultarAlmacen").obtenerVista({
        nombreTemplate: "tmplConsultarAlmacen.html",
        url: "Almacen.aspx/ObtenerFormaConsultarAlmacen",
        parametros: pAlmacen,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarAlmacen == 1) {
                $("#dialogConsultarAlmacen").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Almacen = new Object();
                        Almacen.pIdAlmacen = parseInt($("#divFormaConsultarAlmacen").attr("IdAlmacen"));
                        ObtenerFormaEditarAlmacen(JSON.stringify(Almacen))
                    }
                });
                $("#dialogConsultarAlmacen").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarAlmacen").dialog("option", "buttons", {});
                $("#dialogConsultarAlmacen").dialog("option", "height", "445");
            }
            $("#dialogConsultarAlmacen").dialog("open");
        }
    });
}

function ObtenerFormaEditarAlmacen(pAlmacen) {
    $("#dialogEditarAlmacen").obtenerVista({
        nombreTemplate: "tmplEditarAlmacen.html",
        url: "Almacen.aspx/ObtenerFormaEditarAlmacen",
        parametros: pAlmacen,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarAlmacen").dialog("open");
        }
    });
}

function ObtenerFormaSucursalAsignada(pRequest) {
    $("#dialogConsultarSucursalAsignada").obtenerVista({
        nombreTemplate: "tmplSucursalAsignadaAlmacen.html",
        url: "Almacen.aspx/ObtenerFormaSucursalAsignada",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarSucursalAsignada").dialog("open");
        }
    });
}

function ObtenerFormaAgregarTodasSucursales(pSucursal) {
    $("#ulSucursalesAsignadas").obtenerVista({
        nombreTemplate: "tmplAgregarTodasSucursales.html",
        remplazarVista: false,
        parametros: pSucursal,
        url: "Usuario.aspx/ObtenerFormaAgregarTodasSucursales",
        efecto: "slide",
        despuesDeCompilar: function() {
            $('#ulSucursalesDisponibles').slideUp('fast', function() {
                $(this).children().remove();
                $(this).css("display", "")
            });
        }
    });
}

function TodasSucursalesAAsignadas() {
    var SucursalesEnrolar = new Object();
    var Sucursales = [];
    var Contador = 0;

    SucursalesEnrolar.IdAlmacen = $("#divFormaAsignarSucursalesAlAlmacen").attr("idAlmacen");

    $("#ulSucursalesDisponibles li").each(function(i, e) {
        Contador += 1;
        var Sucursal = new Object();
        var registro = $(this);
        Sucursal.IdSucursal = $(registro).attr("idSucursal");
        Sucursal.Sucursal = $(registro).attr("Sucursal");
        AgregarEnrolamiento(registro, Sucursal);
    });

    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay sucursales para asignar.<br />"); return false;
    }
}


function TodasSucursalesADisponibles() {
    var SucursalesEnrolar = new Object();
    var Sucursales = [];
    var Contador = 0;

    SucursalesEnrolar.IdAlmacen = $("#divFormaAsignarSucursalesAlAlmacen").attr("idAlmacen");

    $("#ulSucursalesAsignadas li").each(function(i, e) {
        Contador += 1;
        var Sucursal = new Object();
        var registro = $(this);
        Sucursal.IdSucursal = $(registro).attr("idSucursal");
        Sucursal.Sucursal = $(registro).attr("Sucursal");
        EliminarEnrolamiento(registro, Sucursal);
    });

    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay sucursales para eliminar.<br />"); return false;
    }
}


function EliminarEnrolamiento(pElemento, pSucursal) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulSucursalesDisponibles").obtenerVista({
        nombreTemplate: "tmplEliminarSucursalAlmacen.html",
        modelo: pSucursal,
        remplazarVista: false,
        efecto: "slide"
    });
}

function AgregarEnrolamiento(pElemento, pSucursal) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulSucursalesAsignadas").obtenerVista({
        nombreTemplate: "tmplAgregarSucursalAEnrolarAlmacen.html",
        modelo: pSucursal,
        remplazarVista: false,
        efecto: "slide"
    });
}

//----------Validaciones----------//
//--------------------------//
function ValidaAlmacen(pAlmacen) {
    var errores = "";

    if (pAlmacen.Almacen == "")
    { errores = errores + "<span>*</span> El nombre del almacén esta vacía, favor de capturarlo.<br />"; }

    if (pAlmacen.IdEmpresa == "0")
    { errores = errores + "<span>*</span> No se indicó la empresa, favor de seleccionarla.<br />"; }

    if (pAlmacen.Telefono == "")
    { errores = errores + "<span>*</span> El teléfono está vacío, favor de capturarlo.<br />"; }

    if (pAlmacen.Correo == "")
    { errores = errores + "<span>*</span> El correo está vacío, favor de capturarlo.<br />"; }

    if (pAlmacen.Calle == "")
    { errores = errores + "<span>*</span> La calle está vacía, favor de capturarla.<br />"; }

    if (pAlmacen.NumeroExterior == "")
    { errores = errores + "<span>*</span> El número externo está vacío, favor de capturarlo.<br />"; }

    if (pAlmacen.NumeroInterior == "")
    { errores = errores + "<span>*</span> El número interior está vacío, favor de capturarlo.<br />"; }

    if (pAlmacen.Colonia == "")
    { errores = errores + "<span>*</span> La colonia está vacía, favor de capturarla.<br />"; }

    if (pAlmacen.CodigoPostal == "")
    { errores = errores + "<span>*</span> El código postal está vacía, favor de capturarlo.<br />"; }

    if (pAlmacen.IdPais == 0)
    { errores = errores + "<span>*</span> El país está vacío, favor de capturarlo.<br />"; }

    if (pAlmacen.IdEstado == 0)
    { errores = errores + "<span>*</span> El estado está vacío, favor de capturarlo.<br />"; }

    if (pAlmacen.IdMunicipio == 0)
    { errores = errores + "<span>*</span> El municipio está vacío, favor de capturarlo.<br />"; }

    if (pAlmacen.IdTipoAlmacen == 0)
    { errores = errores + "<span>*</span> El tipo de almacen está vacío, favor de seleccionarlo.<br />"; }
        
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
