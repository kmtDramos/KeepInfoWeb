//----------DHTMLX----------//
//--------------------------//
function tonclick(id) {
    var idPagina = tree.getSelectedItemId();
    if (idPagina != "Paginas")
    {
        SetFormaConsultarPagina(idPagina);
    }
};

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerJsonArbolPaginas();
    SetFormaAltaPagina();

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
                var idPagina = $("#divFormulario").attr("idPagina");
                SetEliminarPagina(idOpcion);
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
});

//---------Funciones--------//
//--------------------------//
function AgregarPagina()
{
    var pagina = $("#txtPagina").val();
    var titulo = $("#txtTitulo").val();
    var nombreMenu = $("#txtNombreMenu").val();
    var idMenu = $("#cmbMenu").val();
    if($("#chkValidarSucursal").is(':checked'))
	{var validarSucursal = "true";}
	else
	{var validarSucursal = "false";}
    var validacion = ValidaPagina(pagina,titulo,nombreMenu,idMenu);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetAgregarPagina(pagina,titulo,nombreMenu,idMenu,validarSucursal);
}

function EditarPagina()
{
    var idPagina = $("#divFormulario").attr("idPagina");
    var pagina = $("#txtPagina").val();
    var titulo = $("#txtTitulo").val();
    var nombreMenu = $("#txtNombreMenu").val();
    var idMenu = $("#cmbMenu").val();
    if($("#chkValidarSucursal").is(':checked'))
	{var validarSucursal = "true";}
	else
	{var validarSucursal = "false";}
    var validacion = ValidaPagina(pagina,titulo,nombreMenu,idMenu);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetEditarPagina(idPagina,pagina,titulo,nombreMenu,idMenu,validarSucursal);
}

function EliminarPagina()
{
    var pagina = $("#divFormulario").attr("pagina");
    MostrarMensajeEliminar("Esta seguro de eliminar la pagina: "+pagina);
}

//--------Validaciones-------//
//--------------------------//
function ValidaPagina(pPagina,pTitulo,pNombreMenu,pIdMenu)
{
    var errores = "";
    if(pPagina == "")
    {errores=errores+"<span>*</span> El campo pagina esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pTitulo == "")
    {errores=errores+"<span>*</span> El campo titulo esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pNombreMenu == "")
    {errores=errores+"<span>*</span> El campo nombre menu esta vac&iacute;o, favor de capturarlo.<br />";}
    
    if(errores != "")
    {errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores;}
    
    return errores;
}

//-----------AJAX-----------//
//--------------------------//
function ObtenerJsonArbolPaginas() 
{   
    $.ajax({
        type: "POST",
        url: "Paginas.aspx/ObtenerJsonArbolPaginas",
        data: "",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            var respuesta = jQuery.parseJSON(pRespuesta.d);
            IniciarArbol(respuesta.Modelo);
        }
    });
}

//-----------AJAX------------//
//----Funciones de Accion----//
function SetAgregarPagina(pPagina,pTitulo,pNombreMenu,pIdMenu,pValidarSucursal)
{   
    var pRequest = "{'pPagina':'"+pPagina+"','pTitulo':'"+pTitulo+"','pNombreMenu':'"+pNombreMenu+"','pIdMenu':"+pIdMenu+",'pValidarSucursal':"+pValidarSucursal+"}";
    $.ajax({
        type: "POST",
        url: "Paginas.aspx/AgregarPagina",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            $("#txtPagina").val("");
            $("#txtTitulo").val("");
            $("#txtNombreMenu").val("");
            $("#cmbMenu").val("0");
            var respuesta = pRespuesta.d.split("|");
            var json = jQuery.parseJSON(respuesta[1]);
            IniciarArbol(json.Modelo);
        }
    });
}

function SetEditarPagina(pIdPagina,pPagina,pTitulo,pNombreMenu,pIdMenu,pValidarSucursal)
{
    var pRequest = "{'pIdPagina':"+pIdPagina+",'pPagina':'"+pPagina+"','pTitulo':'"+pTitulo+"','pNombreMenu':'"+pNombreMenu+"','pIdMenu':"+pIdMenu+",'pValidarSucursal':"+pValidarSucursal+"}";
    $.ajax({
        type: "POST",
        url: "Paginas.aspx/EditarPagina",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            respuesta = pRespuesta.d.split("|");
            if(respuesta[0] == 0)
            {
                var json = jQuery.parseJSON(respuesta[1]);
                IniciarArbol(json.Modelo);
                SetFormaAltaPagina();
            }
            else
            {
                MostrarMensajeError(respuesta[1]);
            }
            ObtenerMenuPredeterminado();
        }
    });
}

function SetEliminarPagina(pIdPagina)
{
    var pRequest = "{'pIdPagina':"+pIdPagina+"}";
    $.ajax({
        type: "POST",
        url: "Paginas.aspx/EliminarPagina",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            ObtenerJsonArbolPaginas();
            respuesta = pRespuesta.d.split("|");
            IniciarArbol(respuesta[1]);
            SetFormaAltaPagina();
            $("#dialogMensajeEliminar").dialog("close");
            ObtenerMenuPredeterminado();
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function SetFormaAltaPagina()
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaAltaPagina.aspx",
        data: {},
        dataType: "html",
        success: function(pRespuesta){
            if (!$("#divFormulario").get(0)){
				$('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
                AjustarEtiquetas(".divColumna-4-label",150);
			}
			else{
                $('#divFormulario').slideUp('slow', function() {
                    $('#formPrincipal').remove();
                    $('#col-2').append(pRespuesta);
                    $("#divFormulario").slideDown('slow');
                    AjustarEtiquetas(".divColumna-4-label",150);
                });
            }
        }
    });
}

function SetFormaConsultarPagina(pIdPagina)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarPagina.aspx",
        data: {
            IdPagina: pIdPagina
        },
        dataType: "html",
        success: function(pRespuesta){
            $('#divFormulario').slideUp('slow', function() {
                $('#formPrincipal').remove();
                $('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
                AjustarEtiquetas(".divColumna-4-label",150);
            });
        }
    });
}

function SetFormaEditarPagina(pIdPagina)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaEditarPagina.aspx",
        data: {
            IdPagina: pIdPagina
        },
        dataType: "html",
        success: function(pRespuesta){
            $('#divFormulario').slideUp('slow', function() {
                $('#formPrincipal').remove();
                $('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
                AjustarEtiquetas(".divColumna-4-label",150);
            });
        }
    });
}