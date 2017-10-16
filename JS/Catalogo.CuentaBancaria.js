//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("CuentaBancaria");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCuentaBancaria", function() {
        ObtenerFormaAgregarCuentaBancaria();
    });

    $("#grdCuentaBancaria").on("click", ".imgFormaConsultarCuentaBancaria", function() {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerFormaConsultarCuentaBancaria(JSON.stringify(CuentaBancaria));
    });
    
    $("#grdCuentaBancaria").on("click", ".imgFormaCuentaBancariaAsignada", function() {
        var registro = $(this).parents("tr");
        var idCuentaBancaria = $(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html();
        var oRequest = new Object();
        oRequest.pIdCuentaBancaria = idCuentaBancaria;
        ObtenerFormaCuentaBancariaAsignadaUsuario(JSON.stringify(oRequest));
    });
    
    $("#dialogConsultarCuentaBancariaAsignadaUsuario").on("click", "#ulUsuariosDisponibles li", function() {
        var registro = $(this);
        var Usuario = new Object();
        Usuario.IdUsuario = $(registro).attr("idUsuario");
        Usuario.Usuario = $(registro).attr("Usuario");
        Usuario.pIdCuentaBancaria = $("#divFormaAsignarCuentaBancariasAUsuario").attr("idCuentabancaria");
        AgregarEnrolamientoUsuario(registro, JSON.stringify(Usuario));
    });
    
    $("#dialogConsultarCuentaBancariaAsignadaUsuario").on("click", ".Eliminar", function() {
        var registro = $(this).parents("li");
        var Usuario = new Object();
        Usuario.IdUsuario = $(registro).attr("idUsuario");
        Usuario.Usuario = $(registro).attr("Usuario");
        EliminarEnrolamientoUsuario(registro, Usuario);
    });
    
    $("#dialogConsultarCuentaBancariaAsignadaUsuario").on("click", "#divUsuariosDisponibles", function() {
        TodosUsuariosAsignados();
    });

    $("#dialogConsultarCuentaBancariaAsignadaUsuario").on("click", "#divUsuariosAsignados", function() {
        TodosUsuariosDisponibles();
    });

    $('#grdCuentaBancaria').on('click', '.div_grdCuentaBancaria_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancaria_AI']").children().attr("baja")
        var idCuentaBancaria = $(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCuentaBancaria, baja);
    });

    $('#dialogAgregarCuentaBancaria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCuentaBancaria").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCuentaBancaria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarCuentaBancariaAsignadaUsuario').dialog({
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
                AgregarCuentaBancariaAsignadaAUsuario();
            }
        }
    });

    $('#dialogConsultarCuentaBancaria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCuentaBancaria").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCuentaBancaria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCuentaBancaria").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCuentaBancaria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function ObtenerFormaCuentaBancariaAsignadaUsuario(pRequest) {
    $("#dialogConsultarCuentaBancariaAsignadaUsuario").obtenerVista({
        nombreTemplate: "tmplCuentaBancariaAsignadaUsuario.html",
        url: "CuentaBancaria.aspx/ObtenerFormaCuentaBancariaAsignadaUsuario",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarCuentaBancariaAsignadaUsuario").dialog("open");
        }
    });
}

function ObtenerFormaAgregarCuentaBancaria() {
    $("#dialogAgregarCuentaBancaria").obtenerVista({
        nombreTemplate: "tmplAgregarCuentaBancariaBanco.html",
        url: "CuentaBancaria.aspx/ObtenerFormaAgregarCuentaBancaria",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCuentaBancaria").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCuentaBancaria(pIdCuentaBancaria) {
    $("#dialogConsultarCuentaBancaria").obtenerVista({
        nombreTemplate: "tmplConsultarCuentaBancariaBanco.html",
        url: "CuentaBancaria.aspx/ObtenerFormaConsultarCuentaBancaria",
        parametros: pIdCuentaBancaria,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCuentaBancaria == 1) {
                $("#dialogConsultarCuentaBancaria").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var CuentaBancaria = new Object();
                        CuentaBancaria.IdCuentaBancaria = parseInt($("#divFormaConsultarCuentaBancaria").attr("IdCuentaBancaria"));
                        ObtenerFormaEditarCuentaBancaria(JSON.stringify(CuentaBancaria))
                    }
                });
                $("#dialogConsultarCuentaBancaria").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCuentaBancaria").dialog("option", "buttons", {});
                $("#dialogConsultarCuentaBancaria").dialog("option", "height", "100");
            }
            $("#dialogConsultarCuentaBancaria").dialog("open");
        }
    });
}

function ObtenerFormaEditarCuentaBancaria(IdCuentaBancaria) {
    $("#dialogEditarCuentaBancaria").obtenerVista({
        nombreTemplate: "tmplEditarCuentaBancariaBanco.html",
        url: "CuentaBancaria.aspx/ObtenerFormaEditarCuentaBancaria",
        parametros: IdCuentaBancaria,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCuentaBancaria").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCuentaBancaria() {
    var pCuentaBancaria = new Object();
    pCuentaBancaria.Saldo = $("#txtSaldo").val();
    pCuentaBancaria.IdBanco = $("#cmbBanco").val();
    pCuentaBancaria.CuentaBancaria = $("#txtCuentaBancaria").val();
    pCuentaBancaria.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCuentaBancaria.Descripcion = $("#txtDescripcion").val();
    pCuentaBancaria.CuentaContable = $("#txtCuentaContable").val();
    pCuentaBancaria.CuentaContableComplemento = $("#txtCuentaContableComplemento").val();

    var validacion = ValidaCuentaBancaria(pCuentaBancaria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentaBancaria = pCuentaBancaria;
    SetAgregarCuentaBancaria(JSON.stringify(oRequest));
}

function SetAgregarCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentaBancaria.aspx/AgregarCuentaBancaria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentaBancaria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCuentaBancaria").dialog("close");
        }
    });
}

function AgregarCuentaBancariaAsignadaAUsuario() {
    var pUsuario = new Object();
    var Usuarios = [];
    var Contador = 0;

    pUsuario.IdCuentaBancaria = $("#divFormaAsignarCuentaBancariasAUsuario").attr("idCuentabancaria");

    $("#ulUsuariosAsignados li").each(function(i, e) {
        Contador += 1;
        var Usuario = new Object();
        var registro = $(this);
        Usuario.IdUsuario = $(registro).attr("idUsuario");
        
        if ($(this).find("#chkPuedeVerSaldo").is(':checked')) {
            Usuario.PuedeVerSaldo = 1;
        }
        else {
            Usuario.PuedeVerSaldo = 0;
        }
        
        Usuarios.push(Usuario);
    });
    var oRequest = new Object();
    pUsuario.Usuarios = Usuarios;
    oRequest.pUsuario = pUsuario;
    SetAgregarCuentaBancariaUsuario(JSON.stringify(oRequest))
}

function SetAgregarCuentaBancariaUsuario(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentaBancaria.aspx/AgregarCuentaBancariaUsuario",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {                
                $("#dialogConsultarCuentaBancariaAsignadaUsuario").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarCuentaBancariaAsignadaUsuario").dialog("close");
        }
    });
}

function AgregarEnrolamientoUsuario(pElemento, pUsuario) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });
    $("#ulUsuariosAsignados").obtenerVista({
        nombreTemplate: "tmplAgregarCuentaBancariaAEnrolarUsuario.html",
        remplazarVista: false,
        parametros: pUsuario,
        url: "CuentaBancaria.aspx/ObtenerFormaCuentaBancariaAsignadaUsuarioCuentaContable",
        efecto: "slide",
        despuesDeCompilar: function() {
        }   
    });
}


function TodosUsuariosAsignados() {

    TodosUsuariosDisponibles();

    var UsuariosEnrolar = new Object();
    var Usuarios = [];
    var Contador = 0;

    UsuariosEnrolar.IdCuentaBancaria = $("#divFormaAsignarCuentaBancariasAUsuario").attr("idCuentaBancaria");
    
    $("#ulUsuariosDisponibles li").each(function(i, e) {
        Contador += 1;
        var Usuario = new Object();
        Usuario.IdUsuario = $(this).attr("idUsuario");                
        Usuarios.push(Usuario);
    });
    
    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay usuarios para asignar.<br />");return false;
    }
    
    UsuariosEnrolar.Usuarios = Usuarios;
    var oRequest = new Object();
    oRequest.pUsuariosEnrolar = UsuariosEnrolar;
    ObtenerFormaAgregarTodosUsuarios(JSON.stringify(oRequest));
}

function ObtenerFormaAgregarTodosUsuarios(pUsuario) {
    $("#ulUsuariosAsignados").obtenerVista({
        nombreTemplate: "tmplAgregarTodosUsuarios.html",
        remplazarVista: false,
        parametros: pUsuario,
        url: "CuentaBancaria.aspx/ObtenerFormaAgregarTodosUsuarios",
        efecto: "slide",
        despuesDeCompilar: function() {
            $('#ulUsuariosDisponibles').slideUp('fast', function() {
                $(this).children().remove();
                $(this).css("display", "")
            });            
        }
    });
}


function TodosUsuariosDisponibles() {
    var UsuariosEnrolar = new Object();
    var Usuarios = [];
    var Contador = 0;

     UsuariosEnrolar.IdCuentaBancaria = $("#divFormaAsignarCuentaBancariasAUsuario").attr("idCuentaBancaria");

    $("#ulUsuariosAsignados li").each(function(i, e) {
        Contador += 1;
        var Usuario = new Object();
        var registro = $(this);
        Usuario.IdUsuario = $(registro).attr("idUsuario");
        Usuario.Usuario = $(registro).attr("Usuario");
        EliminarEnrolamientoUsuario(registro, Usuario);
    });
}

function EliminarEnrolamientoUsuario(pElemento, pUsuario) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulUsuariosDisponibles").obtenerVista({
        nombreTemplate: "tmplEliminarCuentaBancariaUsuario.html",
        modelo: pUsuario,
        remplazarVista: false,
        efecto: "slide"
    });
}

function SetCambiarEstatus(pIdCuentaBancaria, pBaja) {
    var pRequest = "{'pIdCuentaBancaria':" + pIdCuentaBancaria + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "CuentaBancaria.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCuentaBancaria").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCuentaBancaria').one('click', '.div_grdCuentaBancaria_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaBancaria_AI']").children().attr("baja")
                var idCuentaBancaria = $(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                //SetCambiarEstatus(idCuentaBancaria, baja);
            });
        }
    });
}

function EditarCuentaBancaria() {
    var pCuentaBancaria = new Object();
    pCuentaBancaria.IdCuentaBancaria = $("#divFormaEditarCuentaBancaria").attr("idCuentaBancaria");
    pCuentaBancaria.Saldo = $("#txtSaldo").val();
    pCuentaBancaria.IdBanco = $("#cmbBanco").val();
    pCuentaBancaria.CuentaBancaria = $("#txtCuentaBancaria").val();
    pCuentaBancaria.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCuentaBancaria.Descripcion = $("#txtDescripcion").val();
    pCuentaBancaria.CuentaContable = $("#txtCuentaContable").val();
    pCuentaBancaria.CuentaContableComplemento = $("#txtCuentaContableComplemento").val();
    
    var validacion = ValidaCuentaBancaria(pCuentaBancaria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentaBancaria = pCuentaBancaria;
    SetEditarCuentaBancaria(JSON.stringify(oRequest));
}
function SetEditarCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentaBancaria.aspx/EditarCuentaBancaria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentaBancaria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarCuentaBancaria").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaCuentaBancaria(pCuentaBancaria) {
    var errores = "";

    if (pCuentaBancaria.Saldo == "")
    { errores = errores + "<span>*</span> El esta vacía, favor de capturarla.<br />"; }
    
    if (pCuentaBancaria.CuentaBancaria == "")
    { errores = errores + "<span>*</span> La cuenta bancaria esta vacía, favor de capturarla.<br />"; }

    if (pCuentaBancaria.IdBanco == 0)
    { errores = errores + "<span>*</span> El banco esta vacio, favor de capturarlo.<br />"; }

    if (pCuentaBancaria.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> La moneda esta vacia, favor de capturarla.<br />"; }

    if (pCuentaBancaria.Descripcion == "")
    { errores = errores + "<span>*</span> La descripción esta vacia, favor de capturarla.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
