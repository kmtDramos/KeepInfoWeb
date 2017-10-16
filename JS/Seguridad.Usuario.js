//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaAltaUsuario();

    //Dialogo Eliminar
    $('#dialogMensajeEliminar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Eliminar": function() {
                var idUsuario = $("#divFormulario").attr("idUsuario");
                SetEliminarUsuario(idUsuario);
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarContrasena').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function(event, ui) {
            $("#txtContrasenaAdministrador").val("");
            $("#txtContrasenaNueva").val("");
        },
        buttons: {
            "Guardar cambios": function() {
                EditarContrasena();
            },
            "Cancelar": function() {
                $(this).dialog("close");
                $("#txtContrasenaAdministrador").val("");
                $("#txtContrasenaNueva").val("");
            }
        }
    });

    $('#grdUsuarios').one('click', '.div_grdUsuarios_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdUsuarios_AI']").children().attr("baja")
        var idUsuario = $(registro).children("td[aria-describedby='grdUsuarios_IdUsuario']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idUsuario, baja);
    });

    $("#divFormulario").livequery(function() {
        if ($("#txtFechaNacimiento").length)
        { $("#txtFechaNacimiento").datepicker(); }
    });

    //-----Eventos------------------------------------------------------
    $("#divVistaFormas").on("click", "#btnAgregarUsuario", function() {
        AgregarUsuario();
    });

    $("#divVistaFormas").on("click", "#btnFormaAltaUsuario", function() {
        ObtenerFormaAltaUsuario();
    });

    $("#divVistaFormas").on("click", "#btnFormaEditarUsuario", function() {
        var idUsuario = $("#divFormulario").attr("idUsuario");
        ObtenerFormaEditarUsuario(idUsuario);
    });

    $("#divVistaFormas").on("click", "#btnEditarUsuario", function() {
        EditarUsuario();
    });

    $("#divVistaFormas").on("click", "#imgEditarContrasena", function() {
        $("#dialogEditarContrasena").dialog("open");
    });

    $("#grdUsuarios").on("click", ".imgFormaConsultarUsuario", function() {
        var registro = $(this).parents("tr");
        var idUsuario = $(registro).children("td[aria-describedby='grdUsuarios_IdUsuario']").html();
        ObtenerFormaConsultarUsuario(idUsuario);
    });
    
    $("#grdUsuarios").on("click", ".imgFormaSucursalAsignada", function() {
        var registro = $(this).parents("tr");
        var idUsuario = $(registro).children("td[aria-describedby='grdUsuarios_IdUsuario']").html();
        var oRequest = new Object();
        oRequest.pIdUsuario = idUsuario;
        ObtenerFormaSucursalAsignada (JSON.stringify(oRequest));
    });
    
    $('#dialogConsultarSucursalAsignada').dialog({
        autoOpen: false,
        height: '660',
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
                AgregarSucursalAsignadaAUsuario();
            }
        }
    });
    
    $("#dialogConsultarSucursalAsignada").on("click", ".Eliminar", function() {
        var registro = $(this).parents("li");
        var Sucursal = new Object();
        Sucursal.IdSucursal = $(registro).attr("idSucursal");
        Sucursal.Sucursal = $(registro).attr("sucursal");
        EliminarEnrolamiento(registro, Sucursal);
    }); 
    
    $("#dialogConsultarSucursalAsignada").on("click", "#ulSucursalesDisponibles li", function() {
        var Sucursal = new Object();
        Sucursal.pIdSucursal = $(this).attr("idSucursal");
        Sucursal.pSucursal = $(this).attr("sucursal");
        Sucursal.pIdUsuario = $("#divFormaSucursalAsignadaAlUsuario").attr("idUsuario");
        $(this).slideUp('slow', function() {
            $(this).remove();
        });
       ObtenerFormaEditarEnrolamiento(JSON.stringify(Sucursal));
    }); 
    
    
    $("#dialogConsultarSucursalAsignada").on("click", "#divSucursalesDisponibles", function() {
        TodasSucursalesAAsignadas();
    });
    
    $("#dialogConsultarSucursalAsignada").on("click", "#divSucursalesAsignadas", function() {
        TodasSucursalesADisponibles();
    });
     
    
});

function Termino_grdUsuarios ()
{
	$("td[aria-describedby=grdUsuarios_Metas]", $("#grdUsuarios")).each(function (index, element) {
		$(element).html('<img src="../images/true.png" width="16"/>').click(function () {
			var Usuario = new Object();
			Usuario.IdUsuario = parseInt($(element).parent("tr").children("td[aria-describedby=grdUsuarios_IdUsuario]").text());
			var Request = JSON.stringify(Usuario);
			ObtenerMetasUsuario(Request);
		});
	});
}

function ObtenerMetasUsuario (Usuario)
{
	var ventana = $('<div id="dialogMetasUsuario"></div>');
	$(ventana).dialog({
		modal: true,
		autoOpen: false,
		resizable: false,
		draggable: false,
		close: function () {
			$(ventana).remove();
		},
		buttons: {
			"Guardar": function () {
				var Usuario = new Object();
				Usuario.IdUsuario = parseInt($("#divMetasUsuario", ventana).attr("IdUsuario"));
				var Metas = [];
				$(".txtMeta", ventana).each(function (index, element) {
					var Meta = new Object();
					Meta.IdDivision = $(element).attr("IdDivision");
					Meta.Meta = parseFloat($(element).val())
					Metas.push(Meta);
				});
				Usuario.Metas = { Metas: Metas };
				var Request = JSON.stringify(Usuario);
				GuardarMetasUsuario(Request)
			},
			"Cerrar": function () {
				$(ventana).dialog("close");
			}
		}
	});
	$(ventana).obtenerVista({
		url: "Usuario.aspx/ObtenerMetasUsuario",
		parametros: Usuario,
		nombreTemplate: "tmplMetasUsuario.html",
		despuesDeCompilar: function () {
			$(ventana).dialog("open");
		}
	});
}

function GuardarMetasUsuario (Usuario)
{
	$.ajax({
		url: "Usuario.aspx/GuardarMetasUsuario",
		type: "post",
		data: Usuario,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Resultado) {
			var json = JSON.parse(Resultado.d);
			if (json.Error == 0)
			{
				$("#dialogMetasUsuario").dialog("close");
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

//---------Funciones--------//
//--------------------------//
function AgregarUsuario() 
{
    var nombre = $("#txtNombre").val();
    var apellidoPaterno = $("#txtApellidoPaterno").val();
    var apellidoMaterno = $("#txtApellidoMaterno").val();
    var fechaNacimiento = $("#txtFechaNacimiento").val();
    var usuario = $("#txtUsuario").val();
    var contrasena = $("#txtContrasena").val();
    var correo = $("#txtCorreo").val();
    var idPerfil = $("#cmbPerfil").val();
    var esAgente = ($("#chkEsAgente").is(':checked')) ? "true" : "false";
    var esVendedor = ($("#chkEsVendedor").is(':checked')) ? "true" : "false";
	var Alcance1 = $("#txtAlcance1").val();
	var Alcance2 = $("#txtAlcance2").val();
	var Meta = $("#txtMeta").val();
	var ClientesNuevos = $("#txtClientesNuevos").val();
    var validacion = ValidaUsuario(nombre, apellidoPaterno, apellidoMaterno, fechaNacimiento, usuario, contrasena, correo, idPerfil);
    
    if(validacion != "")
    { MostrarMensajeError(validacion); return false; }
    contrasena = CryptoJS.MD5(contrasena);
    SetAgregarUsuario(nombre,apellidoPaterno,apellidoMaterno,fechaNacimiento,usuario,contrasena,correo,idPerfil, esAgente, esVendedor, Alcance1, Alcance2, Meta, ClientesNuevos);
}

function EditarUsuario()
{
    var idUsuario = $("#divFormulario").attr("idUsuario");
    var nombre = $("#txtNombre").val();
    var apellidoPaterno = $("#txtApellidoPaterno").val();
    var apellidoMaterno = $("#txtApellidoMaterno").val();
    var fechaNacimiento = $("#txtFechaNacimiento").val();
    var usuario = $("#txtUsuario").val();
    var contrasena = $("#txtContrasena").val();
    var correo = $("#txtCorreo").val();
    var idPerfil = $("#cmbPerfil").val();
    var esAgente = ($("#chkEsAgente").is(':checked')) ? "true" : "false";
    var esVendedor = ($("#chkEsVendedor").is(':checked')) ? "true" : "false";
	var Alcance1 = $("#txtAlcance1").val();
	var Alcance2 = $("#txtAlcance2").val();
	var Meta = $("#txtMeta").val();
	var ClientesNuevos = $("#txtClientesNuevos").val();
	
    var validacion = ValidaUsuario(nombre,apellidoPaterno,apellidoMaterno,fechaNacimiento,usuario,contrasena,correo,idPerfil);
    if(validacion != "")
    { MostrarMensajeError(validacion); return false; }
    contrasena = CryptoJS.MD5(contrasena);
    SetEditarUsuario(idUsuario, nombre, apellidoPaterno, apellidoMaterno, fechaNacimiento, usuario, contrasena, correo, idPerfil, esAgente, esVendedor, Alcance1, Alcance2, Meta, ClientesNuevos);
}

function EliminarUsuario()
{
    var usuario = $("#divFormulario").attr("usuario");
    MostrarMensajeEliminar("Esta seguro de eliminar al usuario: "+usuario);
}

function EditarContrasena() 
{
    var idUsuario = $("#divFormulario").attr("idUsuario");
    var contrasenaAdministrador = $("#txtContrasenaAdministrador").val();
    var contrasenaNueva = $("#txtContrasenaNueva").val();

    var validacion = ValidaEditarContrasena(contrasenaAdministrador, contrasenaNueva);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    contrasenaAdministrador = CryptoJS.MD5(contrasenaAdministrador);
    contrasenaNueva = CryptoJS.MD5(contrasenaNueva);
    SetEditarContrasena(idUsuario, contrasenaAdministrador, contrasenaNueva);
}

//--------Validaciones-------//
//--------------------------//
function ValidaUsuario(pNombre,pApellidoPaterno,pApellidoMaterno,pFechaNacimiento,pUsuario,pContrasena,pCorreo,pIdPerfil)
{
    var errores = "";
    
    if(pNombre == "")
    {errores=errores+"<span>*</span> El campo nombre esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pApellidoPaterno == "")
    {errores=errores+"<span>*</span> El campo apellido paterno esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pApellidoMaterno == "")
    {errores=errores+"<span>*</span> El campo apellido materno esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pFechaNacimiento == "")
    {errores=errores+"<span>*</span> El campo fecha de nacimiento esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pUsuario == "")
    {errores=errores+"<span>*</span> El campo usuario esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pContrasena == "")
    {errores=errores+"<span>*</span> El campo contraseña esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pCorreo == "")
    {errores=errores+"<span>*</span> El campo correo esta vac&iacute;o, favor de capturarlo.<br />";}
    
    if(pCorreo != "")
    {
        if(ValidarCorreo(pCorreo))
        {errores=errores+"<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />";}
    }
    
    if(errores != "")
    {errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores;}
    
    return errores;
}

function ValidaEditarContrasena(pContrasenaAdministrador, pContrasenaNueva) {
    var errores = "";

    if (pContrasenaAdministrador == "")
    { errores = errores + "<span>*</span> El campo contraseña administrador esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pContrasenaNueva == "")
    { errores = errores + "<span>*</span> El campo contraseña nueva esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaSucursal(pSucursal) {
    var errores = "";
    
    if (pSucursal.IdPerfil == 0)
    { errores = errores + "<span>*</span> No se indicó el perfil, favor de seleccionarlo.<br />"; }   
     
    return errores;
}

//-----------AJAX-----------//
//----LlamadasWebMethods----//
function SetAgregarUsuario(pNombre, pApellidoPaterno, pApellidoMaterno, pFechaNacimiento, pUsuario, pContrasena, pCorreo, pIdPerfil, pEsAgente, pEsVendedor, Alcance1, Alcance2, Meta, ClientesNuevos) {
	var pRequest = "{'pNombre':'" + pNombre + "','pApellidoPaterno':'" + pApellidoPaterno + "','pApellidoMaterno':'" + pApellidoMaterno + "','pFechaNacimiento':'" + pFechaNacimiento + "','pUsuario':'" + pUsuario + "','pContrasena':'" + pContrasena + "','pCorreo':'" + pCorreo + "','pIdPerfil':" + pIdPerfil + ", 'pEsAgente':" + pEsAgente + ", 'pEsVendedor':" + pEsVendedor + ", 'pAlcance1':" + Alcance1 + ", 'pAlcance2':" + Alcance2 + ", 'pMeta':" + Meta + ",'pClientesNuevos':" + ClientesNuevos + "}";
    $.ajax({
        type: "POST",
        url: "Usuario.aspx/AgregarUsuario",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            var respuesta = pRespuesta.d.split("|");
            if(respuesta[0]=="1")
            {MostrarMensajeError(respuesta[1]);}
            else
            {
                $("#txtNombre").val("");
                $("#txtApellidoPaterno").val("");
                $("#txtApellidoMaterno").val("");
                $("#txtFechaNacimiento").val("");
                $("#txtUsuario").val("");
                $("#txtContrasena").val("");
                $("#txtCorreo").val("");
                $("#cmbPerfil").val("0");
                $("#grdUsuarios").trigger("reloadGrid");
            }
        }
    });
}

function SetEditarUsuario(pIdUsuario, pNombre, pApellidoPaterno, pApellidoMaterno, pFechaNacimiento, pUsuario, pContrasena, pCorreo, pIdPerfil, pEsAgente, pEsVendedor, Alcance1, Alcance2, Meta, ClientesNuevos) {
	var pRequest = "{'pIdUsuario':" + pIdUsuario + ",'pNombre':'" + pNombre + "','pApellidoPaterno':'" + pApellidoPaterno + "','pApellidoMaterno':'" + pApellidoMaterno + "','pFechaNacimiento':'" + pFechaNacimiento + "','pUsuario':'" + pUsuario + "','pContrasena':'" + pContrasena + "','pCorreo':'" + pCorreo + "','pIdPerfil':" + pIdPerfil + ",'pEsAgente':" + pEsAgente + ", 'pEsVendedor':" + pEsVendedor + ", 'pAlcance1':" + Alcance1 + ", 'pAlcance2':" + Alcance2 + ", 'pMeta':" + Meta + ",'pClientesNuevos':" + ClientesNuevos + "}";
    $.ajax({
        type: "POST",
        url: "Usuario.aspx/EditarUsuario",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = pRespuesta.d.split("|");
            if (respuesta[0] == 0) {
                ObtenerFormaAltaUsuario();
                $("#grdUsuarios").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta[1]);
            }
        }
    });
}

function SetCambiarEstatus(pIdUsuario, pBaja) {
    var pRequest = "{'pIdUsuario':" + pIdUsuario + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Usuario.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdUsuarios").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdUsuarios').one('click', '.div_grdUsuarios_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdUsuarios_AI']").children().attr("baja");
                var idUsuario = $(registro).children("td[aria-describedby='grdUsuarios_IdUsuario']").html();
                var idEstatus = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    idEstatus = "true";
                }
                SetCambiarEstatus(idUsuario, idEstatus);
            });
        }
    });
}

function SetEditarContrasena(pIdUsuario, pContrasenaAdministrador, pContrasenaNueva) 
{
    var pRequest = "{'pIdUsuario':" + pIdUsuario + ", 'pContrasenaAdministrador':'" + pContrasenaAdministrador + "','pContrasenaNueva':'" + pContrasenaNueva + "'}";
    $.ajax({
        type: "POST",
        url: "Usuario.aspx/EditarContrasena",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = pRespuesta.d.split("|");
            if (respuesta[0] == 0) {
                MostrarMensajeAviso("El cambio de contraseña se guardó correctamente.");
                $("#dialogEditarContrasena").dialog("close");
                $("#txtContrasenaAdministrador").val("");
                $("#txtContrasenaNueva").val("");
            }
            else {
                MostrarMensajeError(respuesta[1]);
                $("#txtContrasenaAdministrador").val("");
            }
        },
        complete: function() {
            $("#dialogEditarContrasena").dialog("close");
        }
    });    
}

function ActivarSucursal(){
    $('#dialogConsultarSucursalAsignada').on('click', '.div_SucursalAsignada', function(event) {                
        var registro = $('.div_SucursalAsignada').parents("tr");          
        $(registro).children("td[aria-describedby]").attr( "aria-describedby", "inactivo" );
        $(registro).children("td[aria-describedby]").children().html('<img src="../images/off.png">');
        $(this).parent().attr( "aria-describedby", "activo" );
        $(this).html('<img src="../images/on.png">');  
    }); 
 }

function AgregarSucursalAsignadaAUsuario() {
    var SucursalesEnrolar = new Object();
    var Sucursales = [];
    var Contador = 0;
    var cuentaActivos = 0;
    var validacion = "";
    var Predeterminada = 0;

    SucursalesEnrolar.IdUsuario = $("#divFormaSucursalAsignadaAlUsuario").attr("idUsuario");

    $(".liEnrolamientoAsignado").each(function(i) {
        Contador += 1;
        var Sucursal = new Object();
        Sucursal.IdUsuario = $("#divFormaSucursalAsignadaAlUsuario").attr("idUsuario");
        Sucursal.IdSucursal = $(this).attr("idSucursal");        
        Sucursal.IdPerfil = $(this).find("#cmbPerfil").val();                
        estatus = $(this).attr( "aria-describedby", "activo" ).children("table").find("td[aria-describedby]").attr('aria-describedby');  

        if (estatus == 'activo'){
            cuentaActivos = cuentaActivos + 1;
            Predeterminada = $(this).attr("idSucursal"); 
        }
         
        Sucursal.Sucursal = $(this).attr("Sucursal");

        validacion = ValidaSucursal(Sucursal)
        if (validacion != "")
        { return false; }
        Sucursales.push(Sucursal);
    });
    if (Predeterminada !=0){    
        SucursalesEnrolar.IdSucursalPredeterminada = Predeterminada;
    }
    
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
      

    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No se ha seleccionado ninguna sucursal.<br />");return false;
    }
    
    if (cuentaActivos > 1 | cuentaActivos < 1 ) {
        MostrarMensajeError("<span>*</span> Debe de tener solo una sucursal como predeterminada, favor de verificar<br />");return false;
    }
    
    SucursalesEnrolar.Sucursales = Sucursales;
    var oRequest = new Object();
    oRequest.pSucursalesEnrolar = SucursalesEnrolar;
    SetEditarSucursalEnrolada(JSON.stringify(oRequest));
}

function SetEditarSucursalEnrolada(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Usuario.aspx/EditarSucursalEnrolada",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogConsultarSucursalAsignada").dialog("close");
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function TodasSucursalesAAsignadas() {
    var SucursalesEnrolar = new Object();
    var Sucursales = [];
    var Contador = 0;

    SucursalesEnrolar.IdUsuario = $("#divFormaSucursalAsignadaAlUsuario").attr("idUsuario");
    
    $("#ulSucursalesDisponibles li").each(function(i, e) {
        Contador += 1;
        var Sucursal = new Object();
        Sucursal.IdSucursal = $(this).attr("idSucursal");                
                
        Sucursales.push(Sucursal);
    });
    
    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay sucursales para asignar.<br />");return false;
    }

    
    SucursalesEnrolar.Sucursales = Sucursales;
    var oRequest = new Object();
    oRequest.pSucursalesEnrolar = SucursalesEnrolar;
    ObtenerFormaAgregarTodasSucursales(JSON.stringify(oRequest));
}

function TodasSucursalesADisponibles() {
    var SucursalesEnrolar = new Object();
    var Sucursales = [];
    var Contador = 0;

    SucursalesEnrolar.IdUsuario = $("#divFormaSucursalAsignadaAlUsuario").attr("idUsuario");
    
    $("#ulSucursalesAsignadas li").each(function(i, e) {
        Contador += 1;
        var Sucursal = new Object();
        Sucursal.IdSucursal = $(this).attr("idSucursal");                
                
        Sucursales.push(Sucursal);
    });
    
    if (Contador == 0) {
        MostrarMensajeError("<span>*</span> No hay sucursales para desasignar.<br />");return false;
    }

    
    SucursalesEnrolar.Sucursales = Sucursales;
    var oRequest = new Object();
    oRequest.pSucursalesEnrolar = SucursalesEnrolar;
    ObtenerFormaEliminarTodasSucursales(JSON.stringify(oRequest));
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAltaUsuario()
{
    $.ajax({
        type: "POST",
        url: "../Formas/FormaAltaUsuario.aspx",
        data: {},
        dataType: "html",
        success: function(pRespuesta) {
            if ($("#divFormulario").length) {
                $('#divFormulario').slideUp('slow', function() {
                    $('#divVistaFormas').html(pRespuesta);
                    $("#divFormulario").slideDown('slow');
                });
            }
            else {
                $('#divVistaFormas').html(pRespuesta);
                $("#divFormulario").slideDown('slow');
            }
        }
    });
}

function ObtenerFormaConsultarUsuario(pIdUsuario)
{
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarUsuario.aspx",
        data: {
            IdUsuario: pIdUsuario
        },
        dataType: "html",
        success: function(pRespuesta) {
            if ($("#divFormulario").length) {
                $('#divFormulario').fadeOut('slow', function() {
                    $('#divVistaFormas').html(pRespuesta);
                    $("#divFormulario").fadeIn('slow');
                });
            }
            else {
                $('#divVistaFormas').html(pRespuesta);
                $("#divFormulario").fadeIn('slow');
            }
        }
    });
}

function ObtenerFormaEditarUsuario(pIdUsuario)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaEditarUsuario.aspx",
        data: {
            IdUsuario: pIdUsuario
        },
        dataType: "html",
        success: function(pRespuesta){
            $('#divFormulario').fadeOut('slow', function() {
                $('#divVistaFormas').html(pRespuesta);
                $("#divFormulario").fadeIn('slow');
            });
        }
    });
}

function ObtenerFormaSucursalAsignada(pRequest) {
    $("#dialogConsultarSucursalAsignada").obtenerVista({
        nombreTemplate: "tmplSucursalAsignada.html",
        url: "Usuario.aspx/ObtenerFormaSucursalAsignada",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarSucursalAsignada").dialog("open");
            ActivarSucursal();
        }
    });
}

function EliminarEnrolamiento(pElemento, pSucursal) {
    $(pElemento).slideUp('slow', function() {
        $(pElemento).remove();
    });

    $("#ulSucursalesDisponibles").obtenerVista({
        nombreTemplate: "tmplAgregarSucursalAEnrolar.html",
        modelo: pSucursal,
        remplazarVista: false,
        efecto: "slide"
    });
}

function ObtenerFormaEditarEnrolamiento(pSucursal) {
    $("#ulSucursalesAsignadas").obtenerVista({
        nombreTemplate: "tmplAgregarEnrolamientoSucursal.html",
        remplazarVista: false,
        parametros: pSucursal,
        url: "Usuario.aspx/ObtenerFormaAgregarEnrolamientoSucursal",
        efecto: "slide",
        despuesDeCompilar: function() {
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

function ObtenerFormaEliminarTodasSucursales(pSucursal) {
    $("#ulSucursalesDisponibles").obtenerVista({
        nombreTemplate: "tmplEliminarTodasSucursales.html",
        remplazarVista: false,
        parametros: pSucursal,
        url: "Usuario.aspx/ObtenerFormaEliminarTodasSucursales",
        efecto: "slide",
        despuesDeCompilar: function() {
            $('#ulSucursalesAsignadas').slideUp('fast', function() {
                $(this).children().remove();
                $(this).css("display", "")
            });            
        }
    });
}
