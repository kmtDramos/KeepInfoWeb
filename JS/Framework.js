//---------Framework---------//
//--------------------------//
var jqTemplates = new Array();

$(document).ready(function () {

	$("form").submit(function (event) {
		event.preventDefault();
	});

	$("a.ui-dialog-titlebar-close").click(function (evento) {
		evento.preventDefault();
	});

    $("#ctl00_ulMenuSeguridad").kendoMenu();
    $(".divAreaBotonesDialogCatalogos").livequery(function() {
        $(".divAreaBotonesDialogCatalogos").on("click", ".obtenerGestor", function() {
            window.open($(this).attr("gestor") + ".aspx?AccesoDirecto=true", "_blank", "toolbar=no,scrollbars=yes,height=650,width=980");
        });
    });
    
    if($("#divPaginaActual").attr("pagina") != "InicioSesion.aspx") {
        ObtenerFormaPanelUbicacion();
		$("#divExtensionesDeTelefono").show();
    }

    $('#dialogMensajeError').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMensajeAviso').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogDatosUsuario').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Editar": function() {
                EditarDatosUsuario();
            }
        }
    });

    $('#dialogCambiarContrasena').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Guardar cambios": function() {
                CambiarContrasena();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogCambiarSucursal').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Aceptar": function() {
                var request = new Object();
                request.pIdSucursal = $("#cmbSucursalesAsignadas").val();
                CambiarSucursal(JSON.stringify(request));
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $("#spanUsuario").livequery("click", function() {
        ObtenerFormaConsultarDatosUsuario();
    });

    $("#btnCambiarContrasena").livequery("click", function(event) {
        event.preventDefault();
        $("#dialogCambiarContrasena").dialog("open");
    });

    $(".tblDatosUsuario").livequery(function() {
        if ($("#txtDatosFechaNacimiento").length)
        { $("#txtDatosFechaNacimiento").datepicker(); }

        if ($("#btnCambiarContrasena").length) {
            $("#btnCambiarContrasena").button({
                icons: { primary: "ui-icon-locked" }
            });
        }
    });
    
    $("#divContenedorPanelUbicacion").on("click", "#imgCambiarSucursal", function(){
        ObtenerFormaCambiarSucursal();
    });

    $("#divContenedorPanelUbicacion").on("click", "#imgActualizarTipoCambio", function() {
        ActualizarTipoCambio();
    });    
    
    $("#dialogCambiarSucursal").on("change", "#cmbEmpresasAsignadas", function(){
        var request = new Object();
        request.pIdEmpresa = $("#cmbEmpresasAsignadas").val();
        ObtenerSucursalesAsignadas(JSON.stringify(request));
    });
});

//-------DatosUsuario-------//
//--------------------------//
function ObtenerFormaPanelUbicacion(){
    var TipoCambio = new Object();
    TipoCambio.IdTipoMonedaOrigen = parseInt(2);
    TipoCambio.IdTipoMonedaDestino = parseInt(1);        
    $("#divContenedorPanelUbicacion").obtenerVista({
        url: "../InicioSesion.aspx/ObtenerPanelUbicacion",
        nombreTemplate: "tmplPanelUbicacion.html",
        parametros: JSON.stringify(TipoCambio),
        despuesDeCompilar: function(pRespuesta) {
            if(pRespuesta.modelo.Sucursal == "") {
                $("#divContenedorPanelUbicacion").empty();
            }
        }
    });
}

function EditarDatosUsuario() {
    var nombre = $("#txtDatosNombre").val();
    var apellidoPaterno = $("#txtDatosApellidoPaterno").val();
    var apellidoMaterno = $("#txtDatosApellidoMaterno").val();
    var usuario = $("#txtDatosUsuario").val();
    var fechaNacimiento = $("#txtDatosFechaNacimiento").val();
    var correo = $("#txtDatosCorreo").val();
    var validacion = ValidaDatosUsuario(nombre, apellidoPaterno, apellidoMaterno, usuario, fechaNacimiento, correo);

    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    SetEditarDatosUsuario(nombre, apellidoPaterno, apellidoMaterno, usuario, fechaNacimiento, correo);
}

function CambiarContrasena() {
    var actual = $("#txtDatosContrasenaActual").val();
    var nueva = $("#txtDatosContrasena").val();
    var confirmar = $("#txtDatosConfirmarContrasena").val();
    var validacion = ValidaDatosContrasena(actual, nueva, confirmar);

    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    actual = CryptoJS.MD5(actual);
    nueva = CryptoJS.MD5(nueva);
    confirmar = CryptoJS.MD5(confirmar);
    SetCambiarContrasena(actual, nueva, confirmar);
}

function ValidaDatosUsuario(pNombre, pApellidoPaterno, pApellidoMaterno, pFechaNacimiento, pUsuario, pCorreo) {
    var errores = "";

    if (pNombre == "")
    { errores = errores + "<span>*</span> El campo nombre esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pApellidoPaterno == "")
    { errores = errores + "<span>*</span> El campo apellido paterno esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pApellidoMaterno == "")
    { errores = errores + "<span>*</span> El campo apellido materno esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pFechaNacimiento == "")
    { errores = errores + "<span>*</span> El campo fecha de nacimiento esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pUsuario == "")
    { errores = errores + "<span>*</span> El campo usuario esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pCorreo == "")
    { errores = errores + "<span>*</span> El campo correo esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pCorreo != "") {
        if (ValidarCorreo(pCorreo))
        { errores = errores + "<span>*</span> El campo correo no es valido, favor de capturar un correo valido.<br />"; }
    }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaDatosContrasena(pActual, pNueva, pConfirmar) {
    var errores = "";

    if (pActual == "")
    { errores = errores + "<span>*</span> El campo contraseña actual esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pNueva == "")
    { errores = errores + "<span>*</span> El campo contraseña nueva esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pConfirmar == "")
    { errores = errores + "<span>*</span> El campo confirmar contraseña esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pNueva != "" && pConfirmar != "") {
        if (pNueva != pConfirmar) {
            errores = errores + "<span>*</span> Los campos contraseña nueva y confirmar no coinciden, favor de capturarlos nuevamente.<br />";
        }
    }

    $("#txtDatosContrasena").val("");
    $("#txtDatosConfirmarContrasena").val("");

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ObtenerFormaConsultarDatosUsuario() {
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarDatosUsuario.aspx",
        data: {},
        dataType: "html",
        success: function(pRespuesta) {
            $("#dialogDatosUsuario").html(pRespuesta);
            $("#dialogDatosUsuario").dialog("open");
        }
    });
}

function SetEditarDatosUsuario(pNombre, pApellidoPaterno, pApellidoMaterno, pUsuario, pFechaNacimiento, pCorreo) {
    var pRequest = "{'pNombre':'" + pNombre + "','pApellidoPaterno':'" + pApellidoPaterno + "','pApellidoMaterno':'" + pApellidoMaterno + "','pUsuario':'" + pUsuario + "','pFechaNacimiento':'" + pFechaNacimiento + "','pCorreo':'" + pCorreo + "'}";
    $.ajax({
        type: "POST",
        url: "Usuario.aspx/EditarDatosUsuario",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = pRespuesta.d.split("|");
            if (respuesta[0] == 0) {
                $("#spanUsuario").html(respuesta[1] + "<img id='imgEditarDatosUsuario' src='../images/editar.png' style='width:12px;'/>");
                $("#dialogDatosUsuario").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta[1]);
            }
        }
    });
}

function SetCambiarContrasena(pActual, pNueva, pConfirmar) {
    var pRequest = "{'pActual':'" + pActual + "','pNueva':'" + pNueva + "','pConfirmar':'" + pConfirmar + "'}";
    $.ajax({
        type: "POST",
        url: "Usuario.aspx/CambiarContrasena",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = pRespuesta.d.split("|");
            if (respuesta[0] == 0) {
                $("#dialogCambiarContrasena").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta[1]);
            }
        }
    });
}

function ObtenerFormaCambiarSucursal() {
    $("#dialogCambiarSucursal").obtenerVista({
        nombreTemplate: "tmplCambiarSucursal.html",
        url: "../InicioSesion.aspx/ObtenerFormaCambiarSucursal",
        despuesDeCompilar: function() {
            $("#dialogCambiarSucursal").dialog("open");
        }
    });
}

function CambiarSucursal(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "../InicioSesion.aspx/CambiarSucursal",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                location.reload();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                OcultarBloqueo();
                $("#dialogCambiarSucursal").dialog("close");
            }
        },
        complete: function() {
        }
    });
}

function ActualizarTipoCambio() {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "../InicioSesion.aspx/ActualizaTipoCambioWS",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                ObtenerFormaPanelUbicacion();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                OcultarBloqueo();                
            }
        },
        error: function() {
            OcultarBloqueo();
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerSucursalesAsignadas(pRequest) {
    $("#cmbSucursalesAsignadas").obtenerVista({
        parametros: pRequest,
        url: "../InicioSesion.aspx/ObtenerSucursalesAsignadas",
        nombreTemplate: "tmplComboGenerico.html"
    });
}

//Fecha Actual//
function FechaActual() {
    var fechaActual = new Date();
    var dd = fechaActual.getDate();
    var MM = fechaActual.getMonth() + 1;
    var yyyy = fechaActual.getFullYear();

    if (parseInt(dd) < 10) {
        dd = "0" + dd; 
    };
    if (parseInt(MM) < 10) {
        MM = "0" + MM; 
    };
    fechaActual = dd + "/" + MM + "/" + yyyy;

    return fechaActual;
}

function FechaCampo() {
    var fechaActual = new Date();
    var dd = fechaActual.getDate();
    var MM = fechaActual.getMonth() + 1;
    var yyyy = fechaActual.getFullYear();

    if (parseInt(dd) < 10) {
        dd = "0" + dd; 
    };
    if (parseInt(MM) < 10) {
        MM = "0" + MM; 
    };
    fechaActual = yyyy + "/" + MM + "/" + dd;

    return fechaActual;
}

//-----Mantener Session-----//
//--------------------------//
function MantenerSesion(pRaiz)
{
    if(pRaiz == "" || pRaiz == null)
    { var url = "../InicioSesion.aspx/MantenerSesion"; }
    else
    { var url = "InicioSesion.aspx/MantenerSesion"; }
	$.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            //MostrarMensajeError(pRespuesta.d);
        }
    });
}

//---------Funciones--------//
//--------------------------//
function ObtenerMenuPredeterminado() 
{
    var paginaActual = $("#divPaginaActual").attr("pagina");
    if (paginaActual != "InicioSesion.aspx") {
        var request = "{'pPaginaActual':'" + paginaActual + "'}"
        $("#ctl00_ulMenuSeguridad").obtenerVista({
            nombreTemplate: "tmplMenuPredeterminado.html",
            url: "../InicioSesion.aspx/ObtenerMenuPredeterminado",
            parametros: request,
            despuesDeCompilar: function() {
                $("#ctl00_ulMenuSeguridad").kendoMenu();
            }
        });
    }
}
function AjustarEtiquetas(pSelector,pAnchoMaximo)
{
    var max = 0;
    $(pSelector).each(function(){
        if ($(this).width() > max)
            max = $(this).width();
            if (max < pAnchoMaximo)
            {
                max = pAnchoMaximo;
                return false;
            }
    });
    $(pSelector).width(max+2);
}

function FormatoNumero(num,prefix)
{
    num = Math.round(parseFloat(num)*Math.pow(10,2))/Math.pow(10,2)
    prefix = prefix || '';
    num += '';
    var splitStr = num.split('.');
    var splitLeft = splitStr[0];
    var splitRight = splitStr.length > 1 ? '.' + splitStr[1] : '.00';
    splitRight = splitRight + '00';
    splitRight = splitRight.substr(0,3);
    var regx = /(\d+)(\d{3})/;
    while (regx.test(splitLeft))
    {
        splitLeft = splitLeft.replace(regx, '$1' + ',' + '$2');
    }
    return prefix + splitLeft + splitRight;
}

var formato = {
    simbol: "$",
    separador: ",", // separador para los miles
    sepDecimal: ".", // separador para los decimales
    formatear: function(num) {
        num += '';
        num = parseFloat(num).toFixed(2);
        num = num.toString();
        var splitStr = num.split('.');
        var splitLeft = splitStr[0];
        var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '.00';
        var regx = /(\d+)(\d{3})/;
        while (regx.test(splitLeft)) {
            splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
        }
        if (splitLeft.length == 0) {
            splitLeft = "0,000";
        }
        return this.simbol + splitLeft + splitRight;
    },
    moneda: function(num, simbol) {
        this.simbol = simbol || '';
        return this.formatear(num);
    }
}

function MostrarBloqueo() 
{
    var ancho = ($(document).width()-128) / 2;
    var altura = ($(document).height()-128) / 2;
    var $overlay = $('<div id="divBloqueo" class="ui-widget-overlay-bloqueo"><img id="imgLoader" src="../images/ajax-loader-2.gif" style="margin:'+altura+'px 0 0 '+ancho+'px;" /></div>').hide().appendTo('body');
    $($overlay).width($(document).width());
    $($overlay).height($(document).height());
    $('#divBloqueo').fadeIn();
}

function MostrarBloqueoRaiz() 
{
    var ancho = ($(document).width()-128) / 2;
    var altura = ($(document).height()-128) / 2;
    var $overlay = $('<div id="divBloqueo" class="ui-widget-overlay"><img id="imgLoader" src="images/ajax-loader-2.gif" style="margin:'+altura+'px 0 0 '+ancho+'px;" /></div>').hide().appendTo('body');
    $($overlay).width($(document).width());
    $($overlay).height($(document).height());
    $('#divBloqueo').fadeIn();
}

function OcultarBloqueo()
{
    $('#divBloqueo').fadeOut();
    $('#divBloqueo').remove();
}

function ConvertirFecha(pFecha,pFormato)
{
    if(pFecha != '' && pFecha != null && pFecha != 'undefined')
    {
	    var dia = pFecha.substring(0,2);
	    var mes = pFecha.substring(3,5);
	    var anio = pFecha.substring(6,10);
	    switch (pFormato)
	    {
		    case "aaaa/mm/dd":
		      pFecha = anio + '/' + mes + '/' + dia;
		      break;
		    case "mm/dd/aaaa":
		      pFecha = mes + '/' + dia + '/' + anio;
		      break;
		    case "dd/mm/aaaa":
		      pFecha = dia + '/' + mes + '/' + anio;
		      break;
		    case "aaaa-mm-dd":
		      pFecha = anio + '-' + mes + '-' + dia;
		      break;
		     case "aaaammdd":
			     pFecha = anio + mes + dia;
			     break;
		    default:
	    }
	}
	else
	{
	    pFecha = "";
	}
	return pFecha;
}

function TruncateTo(stringToTruncate, thisLength) {
    truncatedString = stringToTruncate;
    if (stringToTruncate.length > thisLength) {
        truncatedString = stringToTruncate.substring(0, thisLength) + "..";
    }
    return truncatedString;
}

//--------Validaciones-------//
//--------------------------//
function ValidarCorreo(pCorreo)
{
    var invalido = 0;
    var caracteresInvalidos = ["|", "°", "!", "¡", "#", "{", "}", "[", "]", "^", "`", "´", "¨", "+", "*", "~", "¿", "?", "(", ")", "/", "\\", ",", "=", "&", "<", ">", "%", "\"", "$", ";", ",", ":", "ñ"];
    for(i in caracteresInvalidos) 
    {
        invalido = pCorreo.indexOf(caracteresInvalidos[i]);
        if(invalido != -1)
        {break;}
    }
    if(invalido == -1)
    {
        //Ejemplo de expresion regular: 'correo.12-@correo.com.mx'
        var patronCorreo = /^([a-z0-9_\.-]+\@[\da-z\.-]+\.[a-z\.]{2,6})$/gm;
        var obtieneSobrante = pCorreo.replace(patronCorreo,"");
        if(obtieneSobrante.length > 0)
        {
            //Ejemplo de expresion regular: 'correo.12-@correo.com'
            var patronCorreo2 = /^([a-z0-9_\.-]+\@[\da-z\.-]+\.[a-z\.]{2,6})$/gm;
            if(!pCorreo.match(patronCorreo2))
            {return true}
            else
            {return false}
        }
        else
        {return false;}
    }
    else
    {return true;}
}

function RFCValido(pRFC) {
    var invalido = 0;
    var caracteresInvalidos = ["|", "°", "!", "¡", "#", "{", "}", "[", "]", "^", "`", "´", "¨", "+", "*", "~", "¿", "?", "(", ")", "/", "\\", ",", "=", "&", "<", ">", "%", "\"", "$", ";", ",", ":", "ñ"];
    for (i in caracteresInvalidos) {
        invalido = pRFC.indexOf(caracteresInvalidos[i]);
        if (invalido != -1)
        { break; }
    }
    if (invalido == -1) {
        //Ejemplo de expresion regular: 'ABCD123456A1B, ABC123456A1B, ABC123456, ABCD123456'
        var patronRFC = /^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$/;
        if (!pRFC.match(patronRFC))
        { return false; }
        else
        { return true; }
    }
    else
    { return true; }
}

function RFCValidoEmpresarial(pRFC) {
    var invalido = 0;
    var caracteresInvalidos = ["|", "°", "!", "¡", "#", "{", "}", "[", "]", "^", "`", "´", "¨", "+", "*", "~", "¿", "?", "(", ")", "/", "\\", ",", "=", "&", "<", ">", "%", "\"", "$", ";", ",", ":", "ñ"];
    for (i in caracteresInvalidos) {
        invalido = pRFC.indexOf(caracteresInvalidos[i]);
        if (invalido != -1)
        { break; }
    }
    if (invalido == -1) {
        //Ejemplo de expresion regular: 'ABCD123456A1B, ABC123456A1B, ABC123456, ABCD123456'
        var patronRFC = /^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})$/;
        if (!pRFC.match(patronRFC))
        { return false; }
        else
        { return true; }
    }
    else
    { return true; }
}

function ValidarNumero(evt, txt){ 
    var key = evt.which || evt.keyCode;
    if ((key == 13 || key == 127 || key == 8) || (key >= 48 && key <=57)){
        return true;
    } 
    return false;
}

function ValidarNumeroPunto(evt, txt){ 
    var key = (document.all) ? evt.keyCode : evt.which;
    if ((key == 13 || key == 127 || key == 46 || key == 8 || key == 0 || key == 45) || (key >= 48 && key <=57)){
        if (key == 46 && txt.indexOf('.')!=-1)
        {return false;}
        return true;
    } 
    return false;
}

function ValidarFlotante(evt, txt){
    var key = (document.all)? evt.keyCode : evt.which;
    var valido = true;
    if(ValidarTeclasNumericas(key) || (key >= 48 && key <=57)){
        if(key == 46 && txt.indexOf(".") != -1){
            valido = false;
        }else{
            valido = true;
        }
    }
    return valido;
}

function ValidarTeclasNumericas(key){
    switch(key){
        case 0:
            return true;
            break;
        case 8:
            return true;
            break;
        case 13:
            return true;
            break;
        case 46:
            return true;
            break;
        case 127:
            return true;
            break;
        default:
            return false;
            break;
    }
}

function LimitarCaracteres(pNoCaracteres) {
    if(pNoCaracteres.length > 2) {
        return true;
    }
    else {
        return false;
    }
}

function LimitarPorcentaje(pEvento, pPorcentaje) {
    var key = pEvento.which || pEvento.keyCode;
    if(key != 46 && pPorcentaje.indexOf('.')==-1){
        var enteros = pPorcentaje.split('.')[0];
        if(enteros.length > 1) {
            return true;
        }
        else {
            return false;
        }
    }
}

function LimitarPorcentajeNumero(pEvento, pPorcentaje, pLimite) {
    var key = pEvento.which || pEvento.keyCode;
    numero = pPorcentaje + String.fromCharCode(key);
    if(key != 46 && pPorcentaje.indexOf('.') == -1){
        if(parseInt(numero) > pLimite) {
            return true;
        }
        else {
            
            return false;
        }
    }
    else if(key != 46 && pPorcentaje.indexOf('.') > 0) {
        if(parseFloat(numero) > parseFloat(pLimite)) {
            return true;
        }
        else {
            
            return false;
        }
    }
}

function ValidarLetraNumero(evt, txt) {
    var key = evt.which || evt.keyCode;
    if ((key == 13 || key == 127) || (key >= 48 && key <= 57) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122)) {
        return true;
    }
    return false;
}

function DesactivarEnter(evt, txt) {
    var key = evt.which || evt.keyCode;
    if (key != 13) {
        return true;
    }
    return false;
}

function DesactivarEnvio(evt, txt){ 
    var key = evt.which || evt.keyCode;
    if ((key == 13)){
        return false; 
    }
}

function ValorPredeterminado(pTextBox, pValor)
{
    if($(pTextBox).val() == "")
    { $(pTextBox).val(pValor); }
}

function QuitarValorPredeterminado(pTextBox,pValorPredeterminado)
{
    if($(pTextBox).val() == pValorPredeterminado)
    { $(pTextBox).val(""); }
}

function adjustSpinnerTimeRelation(thisInitialHour, thisInitialMinute, thisFinalHour, thisFinalMinute, thisFrom) {

    var horaInicialValue = $(thisInitialHour).val();
    var minutoInicialValue = $(thisInitialMinute).val();
    var horaFinalValue = $(thisFinalHour).val();
    var minutoFinalValue = $(thisFinalMinute).val();

    var horaInicial = parseInt(horaInicialValue + minutoInicialValue, 10);
    var horaFinal = parseInt(horaFinalValue + minutoFinalValue, 10);
    if (horaInicial > horaFinal) {
        if (thisFrom === "initial") {
            $(thisFinalHour).val(horaInicialValue)
            $(thisFinalMinute).val(minutoInicialValue)
        }
        if (thisFrom === "final") {
            $(thisInitialHour).val(horaFinalValue)
            $(thisInitialMinute).val(minutoFinalValue)
        }
    }
}

//------MENSAJES JQUERY-----//
//--------------------------//
function MostrarMensajeError(pMensaje)
{
    if ($("#Mensaje").get(0))
    {$('#Mensaje').remove();}
    $("#dialogMensajeError").append('<div id="Mensaje" class="divMensaje"><span class="ui-icon ui-icon-alert iconJQuery"></span>'+pMensaje+'</div>');
    $("#dialogMensajeError").dialog("open");
}

function MostrarMensajeEliminar(pMensaje)
{
    if ($("#MensajeEliminar").get(0))
    {$('#MensajeEliminar').remove();}
    $("#dialogMensajeEliminar").append('<div id="MensajeEliminar" class="divMensaje"><span class="ui-icon ui-icon-trash iconJQuery"></span>'+pMensaje+'</div>');
    $("#dialogMensajeEliminar").dialog("open");
}

function MostrarMensajeAviso(pMensaje)
{
    if ($("#MensajeAviso").get(0))
    {$('#MensajeAviso').remove();}
    $("#dialogMensajeAviso").append('<div id="MensajeAviso" class="divMensaje"><span class="ui-icon ui-icon-alert iconJQuery"></span>'+pMensaje+'</div>');
    $("#dialogMensajeAviso").dialog("open");
}

//--FUNCIONES PARA CONTROLES JQUERY--//
//-----------------------------------//
function ComboJquery(pNombreCombo)
{
	$.widget( "ui." + pNombreCombo, {
		_create: function() {
			var self = this,
				select = this.element.hide(),
				selected = select.children( ":selected" ),
				value = selected.val() ? selected.text() : "";
			var input = this.input = $( "<input>" )
				.insertAfter( select )
				.val( value )
				.attr("id","txtJq" + pNombreCombo)
				.autocomplete({
					delay: 0,
					minLength: 0,
					source: function( request, response ) {
						var matcher = new RegExp( $.ui.autocomplete.escapeRegex(request.term), "i" );
						response( select.children( "option" ).map(function() {
							var text = $( this ).text();
							if ( this.value && ( !request.term || matcher.test(text) ) )
								return {
									label: text.replace(
										new RegExp(
											"(?![^&;]+;)(?!<[^<>]*)(" +
											$.ui.autocomplete.escapeRegex(request.term) +
											")(?![^<>]*>)(?![^&;]+;)", "gi"
										), "<strong>$1</strong>" ),
									value: text,
									option: this
								};
						}) );
					},
					
					select: function( event, ui ) {
						ui.item.option.selected = true;
						self._trigger( "selected", event, {
							item: ui.item.option
						});
					},
					
					change: function( event, ui ) {
						if ( !ui.item ) {
							var matcher = new RegExp( "^" + $.ui.autocomplete.escapeRegex( $(this).val() ) + "$", "i" ),
								valid = false;
							select.children( "option" ).each(function() {
								if ( $( this ).text().match( matcher ) ) {
									this.selected = valid = true;
									return false;
								}
							});
							if ( !valid ) {
								// remove invalid value, as it didn't match anything
								$( this ).val( "" );
								select.val( "" );
								input.data( "autocomplete" ).term = "";
								return false;
							}
						}
					}					
				})
				.addClass( "ui-widget ui-widget-content ui-corner-left" );

			input.data( "autocomplete" )._renderItem = function( ul, item ) {
				return $( "<li></li>" )
					.data( "item.autocomplete", item )
					.append( "<a>" + item.label + "</a>" )
					.appendTo( ul );
			};

			this.button = $( "<button type='button'>&nbsp;</button>" )
				.attr( "tabIndex", -1 )
				.attr( "title", "" )
				.attr( "id","btn"+pNombreCombo)
				.insertAfter( input )
				.button({
					icons: {
						primary: "ui-icon-triangle-1-s"
					},
					text: false
				})
				.removeClass( "ui-corner-all" )
				.addClass( "ui-corner-right ui-button-icon" )
				.click(function() {
					// close if already visible
					if ( input.autocomplete( "widget" ).is( ":visible" ) ) {
						input.autocomplete( "close" );
						return;
					}

					// work around a bug (likely same cause as #5265)
					$( this ).blur();

					// pass empty string as value to search for, displaying all results
					input.autocomplete( "search", "" );
					input.focus();
				});
		},

		destroy: function() {
			this.input.remove();
			this.button.remove();
			this.element.show();
			$.Widget.prototype.destroy.call( this );
		}
	});
	eval('$("#"+pNombreCombo).'+pNombreCombo+'();');
}

$.widget("ui.spinnerTiempo", $.ui.spinner, {
    options: {
        min: 0
    },
    _create: function() {
        //Celso se puso alegre y lo cargo en el minimo que es cero para toda funcion tiempo
        this.element.val(this.options.min)
        // handle string values that need to be parsed
        this._setOption("max", this.options.max);
        this._setOption("min", this.options.min);
        this._setOption("step", this.options.step);

        // format the value, but don't constrain
        this._value(this.element.val(), true);

        this._draw();
        this._on(this._events);
        this._refresh();

        // turning off autocomplete prevents the browser from remembering the
        // value when navigating through history, so we re-enable autocomplete
        // if the page is unloaded before the widget is destroyed. #7790
        this._on(this.window, {
            beforeunload: function() {
                this.element.removeAttr("autocomplete");
            }
        });
    },
    _format: function(value) {
        return Globalize.format(value, "d2");
    }
});
$.widget("ui.spinnerMinutos", $.ui.spinnerTiempo, {
    options: {
        max: 59
    }
});
$.widget("ui.spinnerHoras", $.ui.spinnerTiempo, {
    options: {
        max: 23
    }
});

//--FUNCIONES PARA CONTROLES DHTMLX--//
//-----------------------------------//
function IniciarArbol(pJSON)
{
    //Efecto Jquery
    if (!$("#treeboxbox_tree").get(0)){
        $("#col-1").append("<div id='treeboxbox_tree' class='etiquetaOculta'></div>");
		SetIniciarArbol(pJSON);
        $("#treeboxbox_tree").fadeOut('slow');
	}
	else{
	    //Ocultar Arbol
        $('#treeboxbox_tree').fadeOut('slow', function() {
            $("#treeboxbox_tree").remove();
            $("#col-1").append("<div id='treeboxbox_tree' class='etiquetaOculta'></div>");
            SetIniciarArbol(pJSON);
            //Mostrar Arbol
            $("#treeboxbox_tree").fadeIn('slow');
        });
    }
}

function SetIniciarArbol(pJSON)
{
    tree = new dhtmlXTreeObject("treeboxbox_tree", "100%", "100%", 0);
    tree.setSkin('dhx_skyblue');
    tree.setImagePath("../js/dhtmlx/dhtmlxTree/codebase/imgs/csh_dhx_skyblue/");
    tree.enableCheckBoxes(0);
    tree.enableDragAndDrop(0);
    tree.setOnClickHandler(tonclick);
    tree.loadJSONObject(pJSON);
}

//-----Plugin Obtener Vistas
jQuery.fn.obtenerVista = function(pOpciones) {
    MostrarBloqueo();
    var contenedor = $(this);
    var vista = {
        nombreTemplate: "",
        url: "",
        parametros: {},
        esDialog: false,
        remplazarVista: true,
        modelo:{},
        antesDeCompilar: function(pVista) { },
        despuesDeCompilar: function(pVista) { },
    }
    $.extend(vista, pOpciones);
    if (vista.nombreTemplate == "") {
        MostrarMensajeError("<p>Favor de completar los siguientes requisitos:</p><span>*</span> No se definió el nombre del template.");
    }
    else if (vista.url != "" && vista.nombreTemplate != "") {
        $.ajax({
            type: "POST",
            url: vista.url,
            data: vista.parametros,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                var respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error)
                { MostrarMensajeError(respuesta.Descripcion); OcultarBloqueo(); }
                else {
                    vista.modelo = respuesta.Modelo;
                    var existeTemplate = false;
                    $.each(jqTemplates, function(pIndice, pTemplate) {
                        if (pTemplate.nombre == vista.nombreTemplate) {
                            vista.template = pTemplate.template;
                            existeTemplate = true;
                        }
                        return (pTemplate.nombre != vista.nombre);
                    });

                    if (!existeTemplate) {
                        $.ajax({
                            type: 'Get',
                            url: "../Templates/" + vista.nombreTemplate,
                            cache: false,
                            success: function(data) {
                                var oTemplate = new Object();
                                oTemplate.nombre = vista.nombreTemplate;
                                oTemplate.template = data;
                                //jqTemplates.push(oTemplate);
                                vista.template = oTemplate.template;
                                vista.antesDeCompilar(vista);
                                if (vista.esDialog) {
                                    contenedor.dialog('option', "title", respuesta.Titulo);
                                    contenedor.dialog('option', "width", respuesta.Ancho);
                                    contenedor.dialog('option', "height", respuesta.Alto);
                                }
                                ProcesarTemplate(vista, contenedor);
                                vista.despuesDeCompilar(vista);
                            }
                        });
                    }
                    else {
                        vista.antesDeCompilar(vista);
                        if (vista.esDialog) {
                            contenedor.dialog('option', "title", respuesta.Titulo);
                            contenedor.dialog('option', "width", respuesta.Ancho);
                            contenedor.dialog('option', "height", respuesta.Alto);
                        }
                        ProcesarTemplate(vista, contenedor);
                        vista.despuesDeCompilar(vista);
                    }
                }
            },
            error: function() {
                OcultarBloqueo();
            },
            complete: function() {
            }
        });
    }
    else if (vista.url == "" && vista.nombreTemplate != "") {
        var existeTemplate = false;
        $.each(jqTemplates, function(pIndice, pTemplate) {
            if (pTemplate.nombre == vista.nombreTemplate) {
                vista.template = pTemplate.template;
                existeTemplate = true;
            }
            return (pTemplate.nombre == vista.nombre);
        });

        if (!existeTemplate) {
            $.ajax({
                type: 'Get',
                url: "../Templates/" + vista.nombreTemplate,
                cache: false,
                success: function(data) {
                    var oTemplate = new Object();
                    oTemplate.nombre = vista.nombreTemplate;
                    oTemplate.template = data;
                    //jqTemplates.push(oTemplate);
                    vista.template = oTemplate.template;
                    vista.antesDeCompilar(vista);
                    ProcesarTemplate(vista, contenedor)
                    vista.despuesDeCompilar(vista);
                },
                error: function() {
                    OcultarBloqueo();
                }
            });
        }
        else {
            vista.antesDeCompilar(vista);
            ProcesarTemplate(vista, contenedor);
            vista.despuesDeCompilar(vista);
        }
    }
}

function ProcesarTemplate(pVista, pContenedor) {
    if (pVista.remplazarVista == true) {
        switch (pVista.efecto) {
            case "slide":
                if (pContenedor.html().length) {
                    pContenedor.children().slideUp('slow', function() {
                        pContenedor.empty();
                        $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                        pContenedor.children().slideDown('slow');
                    });
                }
                else {
                    pContenedor.empty();
                    $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                    pContenedor.children().slideDown('slow');
                }
                break;
            case "fade":
                if (pContenedor.html().length) {
                    pContenedor.children().fadeOut('slow', function() {
                        pContenedor.empty();
                        $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                        pContenedor.children().fadeIn('slow');
                    });
                }
                else {
                    pContenedor.empty();
                    $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                    pContenedor.children().fadeIn('slow');
                }
                break;
            default:
                pContenedor.empty();
                $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                break;
        }
    }
    else {
        switch (pVista.efecto) {
            case "slide":
                    $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                    pContenedor.children().slideDown('slow');
                break;
            case "fade":
                    $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                    pContenedor.children().fadeIn('slow');
                break;
            default:
                $.tmpl(pVista.template, pVista.modelo).appendTo(pContenedor);
                break;
        }
    }
    OcultarBloqueo();
}

//-----Establecer predeterminados
$.datepicker.setDefaults({
    dateFormat: 'dd/mm/yy',
    changeMonth: true,
    changeYear: true,
    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
});

function CrearFiltroMultiChecks() {
    $("ul.topsel li").click(function() {
        if ($(this).find("ul.rulingWidth").attr("expectedWidth") == "0") {
            originalWidth = $(this).find("ul.rulingWidth").css("width");
            originalWidthSansPX = originalWidth.replace("px", "");
            newWidthSansPX = parseInt(originalWidthSansPX, 10) + 20;
            newWidth = newWidthSansPX + "px";
            $(this).find("ul.rulingWidth").attr("expectedWidth", newWidth)
            $(this).find("ul.rulingWidth").css("width", $(this).find("ul.rulingWidth").attr("expectedWidth"));
        }
        $(this).find("ul.subsel").show();
        $(this).hover(function() {
        }, function() {
            $(this).find("ul.subsel").slideUp(1);
        });
    }).hover(function() {
        $(this).addClass("subhover");
    }, function() {
        $(this).removeClass("subhover");
    });
}

function addAllChecks(thisModifier) {
    $('.' + thisModifier + '').each(function(i, e) {
        $(this).attr('checked', 'checked')
    });
}

function removeAllChecks(thisModifier, unprotect) {
    $('.' + thisModifier + '').each(function(i, e) {
        if ($(this).val() != 0 || unprotect) {
            $(this).attr('checked', false);
        }
    });
}

jQuery.fn.existe = function(){
    return jQuery(this).length > 0;
};

jQuery.fn.valorPredeterminado = function(pTipoValidacion){
    pTipoValidacion = pTipoValidacion.toUpperCase();
    switch(pTipoValidacion)
    {
        case "MONEDA":
            var valor = $(this).val();
            valor = valor.replace("$","");
            if(valor == "")
            { valor = 0; }
            $(this).val(formato.moneda(valor, "$"));
        break;
        case "PORCENTAJE":
            var valor = $(this).val();
            if(valor == "")
            { valor = 0; }
            $(this).val(valor);
        break;
        case "DECIMAL":
            var valor = $(this).val();
            if(valor == "")
            { valor = "0.00"; }
            $(this).val(parseFloat(valor).toFixed(2));
        break;
    }
}

Number.prototype.currency = function(){
    var amount = this.toString().split('.');
    var hole = amount[0];
    var decimals = (amount[1]!=undefined)?amount[1]:"00";
    return "$" + hole.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,") + '.' + decimals.substring(0, 2);
};

jQuery.fn.quitarValorPredeterminado = function(pTipoValidacion){
    pTipoValidacion = pTipoValidacion.toUpperCase();
    switch(pTipoValidacion)
    {
        case "MONEDA":
            var valor = $(this).val();
            valor = valor.replace("$","");
            if(parseFloat(valor) == parseFloat("0"))
            { $(this).val(""); }
            else
            {
                var valor = $(this).val();
                valor = valor.replace("$","");
                valor = valor.replace(",","");
                valor = valor.split(",").join("");
                $(this).val(valor);
            }
        break;
        case "PORCENTAJE":
            var valor = $(this).val();
            if(parseFloat(valor) == parseFloat("0"))
            { $(this).val(""); }
            else
            {
                var valor = $(this).val();
                $(this).val(valor);
            }
        break;
        case "DECIMAL":
            var valor = $(this).val();
            if(parseFloat(valor) == parseFloat("0"))
            { $(this).val(""); }
            else
            {
                var valor = $(this).val();
                $(this).val(valor);
            }
        break;
    }
};

function ActualizarPanelControles(pControl) {
    if(window.opener != null) {
        var panelControles = window.opener.document.getElementById("divPanelControles");
        $(panelControles).html("<div id='divRefrescarControl' control='" + pControl + "'></div>");
        window.opener.ObtenerCombo();
    }
}

function ObtenerCombo(){
    eval("ObtenerLista" + $("#divRefrescarControl").attr("control") + "();");
}

function QuitarFormatoNumero(num) {
    num = num.replace(",", "");
    num = num.replace(",", "");
    num = num.replace(",", "");
    num = num.replace("$", "");
    num = num.replace(" ", "");
    num = num.replace(" ", "");
    return num;
}

///FUNCION QUE CONVIERTE LA CANTIDAD A LETRA

function mod(dividendo, divisor) {
    resDiv = dividendo / divisor;
    parteEnt = Math.floor(resDiv);
    parteFrac = resDiv - parteEnt;
    modulo = Math.round(parteFrac * divisor);
    return modulo;
}

function ObtenerParteEntDiv(dividendo, divisor) {
    resDiv = dividendo / divisor;
    parteEntDiv = Math.floor(resDiv);
    return parteEntDiv;
}

function fraction_part(dividendo, divisor) {
    resDiv = dividendo / divisor;
    f_part = Math.floor(resDiv);
    return f_part;
}

function string_literal_conversion(number) {

    centenas = ObtenerParteEntDiv(number, 100);

    number = mod(number, 100);

    decenas = ObtenerParteEntDiv(number, 10);
    number = mod(number, 10);

    unidades = ObtenerParteEntDiv(number, 1);
    number = mod(number, 1);
    string_hundreds = "";
    string_tens = "";
    string_units = "";

    if (centenas == 1) {
        string_hundreds = "ciento ";
    }


    if (centenas == 2) {
        string_hundreds = "doscientos ";
    }

    if (centenas == 3) {
        string_hundreds = "trescientos ";
    }

    if (centenas == 4) {
        string_hundreds = "cuatrocientos ";
    }

    if (centenas == 5) {
        string_hundreds = "quinientos ";
    }

    if (centenas == 6) {
        string_hundreds = "seiscientos ";
    }

    if (centenas == 7) {
        string_hundreds = "setecientos ";
    }

    if (centenas == 8) {
        string_hundreds = "ochocientos ";
    }

    if (centenas == 9) {
        string_hundreds = "novecientos ";
    }

    if (decenas == 1) {
        if (unidades == 1) {
            string_tens = "once";
        }

        if (unidades == 2) {
            string_tens = "doce";
        }

        if (unidades == 3) {
            string_tens = "trece";
        }

        if (unidades == 4) {
            string_tens = "catorce";
        }

        if (unidades == 5) {
            string_tens = "quince";
        }

        if (unidades == 6) {
            string_tens = "dieciseis";
        }

        if (unidades == 7) {
            string_tens = "diecisiete";
        }

        if (unidades == 8) {
            string_tens = "dieciocho";
        }

        if (unidades == 9) {
            string_tens = "diecinueve";
        }
    }

    if (decenas == 2) {
        string_tens = "veinti";
    }
    if (decenas == 3) {
        string_tens = "treinta";
    }
    if (decenas == 4) {
        string_tens = "cuarenta";
    }
    if (decenas == 5) {
        string_tens = "cincuenta";
    }
    if (decenas == 6) {
        string_tens = "sesenta";
    }
    if (decenas == 7) {
        string_tens = "setenta";
    }
    if (decenas == 8) {
        string_tens = "ochenta";
    }
    if (decenas == 9) {
        string_tens = "noventa";
    }


    if (decenas == 1) {
        string_units = ""; 
    }
    else {
        if (unidades == 1) {
            string_units = "un";
        }
        if (unidades == 2) {
            string_units = "dos";
        }
        if (unidades == 3) {
            string_units = "tres";
        }
        if (unidades == 4) {
            string_units = "cuatro";
        }
        if (unidades == 5) {
            string_units = "cinco";
        }
        if (unidades == 6) {
            string_units = "seis";
        }
        if (unidades == 7) {
            string_units = "siete";
        }
        if (unidades == 8) {
            string_units = "ocho";
        }
        if (unidades == 9) {
            string_units = "nueve";
        }
    }

    if (centenas == 1 && decenas == 0 && unidades == 0) {
        string_hundreds = "cien ";
    }

    if (decenas == 1 && unidades == 0) {
        string_tens = "diez ";
    }

    if (decenas == 2 && unidades == 0) {
        string_tens = "veinte ";
    }

    if (decenas >= 3 && unidades >= 1) {
        string_tens = string_tens + " y ";
    }

    final_string = string_hundreds + string_tens + string_units;
    return final_string;
}

function covertirNumLetras(number, pMoneda) {
    number1 = number; 	
    cent = number1.split('.');
    centavos = cent[1];
    number = cent[0];

    millions_final_string = "";thousands_final_string = "";centenas_final_string = "";
    
    if (centavos == 0 || centavos == undefined) {
        centavos = "00";
    }

    if (number == 0 || number == "") { 						
        centenas_final_string = " cero "; 						
    }
    else {
        millions = ObtenerParteEntDiv(number, 1000000); 
        number = mod(number, 1000000);       

        if (millions != 0) {                      				
            if (millions == 1) { descriptor = " millon "; }                                                                                                                                 
            else { descriptor = " millones "; }  
        }
        else {
            descriptor = " "; 
        }

        millions_final_string = string_literal_conversion(millions) + descriptor;
        thousands = ObtenerParteEntDiv(number, 1000); 
        number = mod(number, 1000); 
        if (thousands != 1) { 
            thousands_final_string = string_literal_conversion(thousands) + " mil ";
        }
        if (thousands == 1) {
            thousands_final_string = " mil ";
        }
        if (thousands < 1) {
            thousands_final_string = " ";
        }

        centenas = number;
        centenas_final_string = string_literal_conversion(centenas);

    }
    cad = millions_final_string + thousands_final_string + centenas_final_string;
    cad = cad.toUpperCase();

    if (centavos.length > 2) {
        if (centavos.substring(2, 3) >= 5) {
            centavos = centavos.substring(0, 1) + (parseInt(centavos.substring(1, 2)) + 1).toString();
        }
        else {
            centavos = centavos.substring(0, 2);
        }
    }

    if (centavos.length == 1) {
        centavos = centavos + "0";
    }
    centavos = centavos + "/100";
    if (number == 1) {
        moneda = " " + pMoneda.toString().toUpperCase() + " ";
    }
    else {
        moneda = " " + validaPluralMoneda(pMoneda == undefined ? "" : pMoneda) + " ";
    }

    var mn = pMoneda == "Pesos" ? " M.N. " : " ";

    cValorLetras = cad + moneda + centavos + mn;
    cValorLetras = cValorLetras.replace(/(^\s*)|(\s*$)/g, "");
    return cValorLetras;
}
////////////////////////////////////////////


function validaPluralMoneda(moneda) {
    var vocales = "aeiou";
    var resultado = "";
    var monedaMinusculas = moneda.toString().toLowerCase();

    if (vocales.indexOf(monedaMinusculas.charAt(monedaMinusculas.length-1), 0) != -1) {
        resultado = moneda + "s";
        return resultado.toUpperCase();
    }

    resultado = moneda;  //+ "es";
    return resultado.toUpperCase();
}

function validaNumero(number) {
    return $.isNumeric(number);
}

//Funcion que quita caracteres moneda a un valor numérico o de cadena.
function QuitaFormatoMoneda(pImporte) {
    if (pImporte != null && pImporte.toString() != 'NaN') {
        if (pImporte.toString().indexOf(",") != -1) {
            var str = pImporte;

            str = str.split(",").join("");
            str = str.replace("$", "");

            return parseFloat(str);
        } else {
            if (pImporte.toString().indexOf("$") != -1) {
                var str = pImporte;
                str = str.replace("$", "");
                return parseFloat(str);
            } else {
                return parseFloat(pImporte);
            }
        }
    }
    else {
        return '0.00';
    }
}

function ValidarCodigoPostal(campo) {
    var ExpCP = '^([0-9]{2}|[0-9][0-9]|[0-9][0-9])[0-9]{3}$';
    if ((!campo.match(ExpCP)) && (!campo!='')) 
    { return true;} 
    else { return false;}
}

function ValidarRazonSocial(pRazonSocial) {
    var invalido = 0;
    var caracteresInvalidos = [";", ",", ".", ":", "á", "é", "í", "ó", "ú", "Á", "É", "Í", "Ó", "Ú"];
    for (i in caracteresInvalidos) {
        invalido = pRazonSocial.indexOf(caracteresInvalidos[i]);
        if (invalido != -1)
        { break; }
    }
    if (invalido == -1) {
        //Ejemplo de expresion regular: 'GRUPO ASERCOM SA DE CV'
        var patronRazonSocial = /^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$/;
        if (!pRazonSocial.match(patronRazonSocial))
        { return false; }
        else
        { return true; }
    }
    else
    { return true; }
}

function BloquearClases(div) {
   $(div).children('td').children('input, textarea, select').attr('disabled', true);

}
function ValidaCodigoPostalFiscal(pCodigoPostal) {
    if (isNaN(pCodigoPostal) == true) {
        return true;
    }
    else {
        if (pCodigoPostal.length != 5) {
            return true;
        }
        else {
            return false;
        }
    }
}

jQuery.fn.generarBotonesPaginador = function(){
    $("#" + this.attr("id") +" .btnPrimeraPagina").button({
        icons: {
            primary: "ui-icon-seek-first"
        },
        text: false
    });
    $("#" + this.attr("id") +" .btnPaginaAnterior").button({
        icons: {
            primary: "ui-icon-seek-prev"
        },
        text: false
    });
    $("#" + this.attr("id") +" .btnSiguientePagina").button({
        icons: {
            primary: "ui-icon-seek-next"
        },
        text: false
    });
    $("#" + this.attr("id") +" .btnUltimaPagina").button({
        icons: {
            primary: "ui-icon-seek-end"
        },
        text: false
    });
}



function ExportarExcel(Store, Parametros, Archivo) {
	window.open("../ExportacionesExcel/ExportarExcelStore.aspx?Store=" + Store + "&Parametros=" + Parametros + "&Archivo=" + Archivo);
}

function PasarAVentana(element) {
	var ventana = $("<div></div>");
	$(ventana).html($(element).html());
	$(ventana).dialog({
		modal: false,
		width: 'auto',
		draggable: false,
		resizable: false,
		close: function () {
			$(ventana).remove();
		}
	});
}