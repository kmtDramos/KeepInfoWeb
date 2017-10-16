//----------DHTMLX----------//
//--------------------------//
function tonclick(id) {
    var idMenu = tree.getSelectedItemId();
    if (idMenu != "Menu") 
    {
        SetFormaConsultarMenu(idMenu);
    }
    else 
    {
        ObtenerMenus();
    }
};

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerJsonArbolMenu();
    SetFormaAltaMenu();
    $("#ulMenus").sortable({ axis: "y" });
    $("#ulMenus").disableSelection();
    $("#ulSubmenus").sortable({axis: "y"});
    $("#ulSubmenus").disableSelection();

    //Dialogo Generico de Mensaje de Error
    $('#dialogOrdenarMenus').dialog({
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
                OrdenarMenus();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    //Dialogo Generico de Mensaje de Error
    $('#dialogOrdenarSubmenus').dialog({
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
                OrdenarSubmenus();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
});

//---------Funciones--------//
//--------------------------//
function AgregarMenu()
{
    var menu = $("#txtMenu").val();
    var idProyectoSistema = $("#cmbProyectoSistema").val();
    var validacion = ValidaMenu(menu,idProyectoSistema);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetAgregarMenu(menu,idProyectoSistema);
}

function EditarMenu()
{
    var idMenu = $("#divFormulario").attr("idMenu");
    var menu = $("#txtMenu").val();
    var idProyectoSistema = $("#cmbProyectoSistema").val();
    var validacion = ValidaMenu(menu,idProyectoSistema);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetEditarMenu(idMenu,menu,idProyectoSistema);
}

function EliminarMenu()
{
    var menu = $("#divFormulario").attr("menu");
    MostrarMensajeEliminar("Esta seguro de eliminar la menu: "+menu);
}

function ObtenerMenus() {
    pRequest = "{}";
    $.ajax({
        type: "POST",
        url: "Menu.aspx/ObtenerMenus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var OJson = $.parseJSON(pRespuesta.d)
            if (OJson.Success) {
                if (OJson.ListaMenus.length <= 1) {
                    MostrarMensajeAviso("No hay suficientes menus.");
                }
                else {
                    var liListado = "";
                    $.each(OJson.ListaMenus, function(id, objeto) {
                        liListado = liListado + "<li idMenu='" + objeto.IdMenu + "'>" + objeto.Menu + "</li>";
                    });
                    $("#ulMenus").html(liListado);
                    $("#dialogOrdenarMenus").dialog("open");
                }
            }
            else {
                MostrarMensajeError(OJson.Mensaje);
            }
        }
    });
}

function ObtenerSubmenus() 
{
    var pRequest = "{'pIdMenu':" + $("#divFormulario").attr("idMenu") + "}";
    $.ajax({
        type: "POST",
        url: "Menu.aspx/ObtenerSubmenus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var OJson = $.parseJSON(pRespuesta.d)
            if (OJson.Success) 
            {
                if (OJson.ListaSubmenus.length <= 1) {
                    MostrarMensajeAviso("No hay suficientes submenus.");
                }
                else {
                    var liListado = "";
                    $.each(OJson.ListaSubmenus, function(id, objeto) {
                        liListado = liListado + "<li idPagina='" + objeto.IdPagina + "'>" + objeto.Submenu + "</li>";
                    });
                    $("#ulSubmenus").html(liListado);
                    $("#dialogOrdenarSubmenus").dialog("open");
                }
            }
            else 
            {
                MostrarMensajeError(OJson.Mensaje);
            }
        }
    });
}

function OrdenarSubmenus() 
{
    var Submenu;
    var contador = 0;
    var OJson = new Object();
    OJson.pObjetoJSON = new Array();
    
    $("#ulSubmenus li").each(function() {
        contador = contador + 1;
        Submenu = new Object();
        Submenu.IdPagina = $(this).attr("IdPagina");
        Submenu.Orden = contador;
        OJson.pObjetoJSON.push(Submenu);
    });
    SetOrdenarSubmenus(JSON.stringify(OJson));
}

function OrdenarMenus() 
{
    var Submenu;
    var contador = 0;
    var OJson = new Object();
    OJson.pObjetoJSON = new Array();

    $("#ulMenus li").each(function() {
        contador = contador + 1;
        Menu = new Object();
        Menu.IdMenu = $(this).attr("IdMenu");
        Menu.Orden = contador;
        OJson.pObjetoJSON.push(Menu);
    });
    SetOrdenarMenus(JSON.stringify(OJson));
}

//--------Validaciones-------//
//--------------------------//
function ValidaMenu(pMenu,pIdProyectoSistema)
{
    var errores = "";
    if(pMenu == "")
    {errores=errores+"<span>*</span> El campo men&uacute; esta vac&iacute;o, favor de capturarlo.<br />";}
    
    if(pIdProyectoSistema == "0")
    {errores=errores+"<span>*</span> El campo ProyectoSistema no se ha seleccionado, favor de capturarlo.<br />";}
    
    if(errores != "")
    {errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores;}
    
    return errores;
}

//-----------AJAX-----------//
//--------------------------//
function ObtenerJsonArbolMenu() 
{   
    $.ajax({
        type: "POST",
        url: "Menu.aspx/ObtenerJsonArbolMenu",
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
function SetAgregarMenu(pMenu,pIdProyectoSistema)
{   
    var pRequest = "{'pMenu':'"+pMenu+"','pIdProyectoSistema':"+pIdProyectoSistema+"}";
    $.ajax({
        type: "POST",
        url: "Menu.aspx/AgregarMenu",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtMenu").val("");
                $("cmbProyectoSistema").val("0");
                IniciarArbol(respuesta.Modelo);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
            ObtenerMenuPredeterminado();
        }
    });
}

function SetEditarMenu(pIdMenu,pMenu,pIdProyectoSistema)
{
    var pRequest = "{'pIdMenu':"+pIdMenu+",'pMenu':'"+pMenu+"','pIdProyectoSistema':"+pIdProyectoSistema+"}";
    $.ajax({
        type: "POST",
        url: "Menu.aspx/EditarMenu",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = pRespuesta.d.split("|");
            if (respuesta[0] == 0) {
                var json = jQuery.parseJSON(respuesta[1]);
                IniciarArbol(json.Modelo);
                SetFormaAltaMenu();
            }
            else {
                MostrarMensajeError(respuesta[1]);
            }
            ObtenerMenuPredeterminado();
        }
    });
}

function SetEliminarMenu(pIdMenu)
{
    var pRequest = "{'pIdMenu':"+pIdMenu+"}";
    $.ajax({
        type: "POST",
        url: "Menu.aspx/EliminarMenu",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            ObtenerJsonArbolMenu();
            respuesta = pRespuesta.d.split("|");
            IniciarArbol(respuesta[1]);
            SetFormaAltaMenu();
            $("#dialogMensajeEliminar").dialog("close");
            ObtenerMenuPredeterminado();
        }
    });
}

function SetOrdenarSubmenus(pRequest) 
{
    $.ajax({
        type: "POST",
        url: "Menu.aspx/OrdenarSubmenus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var OJson = $.parseJSON(pRespuesta.d);
            if (OJson.Error) {
                $("#dialogOrdenarSubmenus").dialog("close");
                MostrarMensajeError(OJson.Mensaje);
            }
            else {
                $("#dialogOrdenarSubmenus").dialog("close");
            }
            ObtenerMenuPredeterminado();
        }
    });
}

function SetOrdenarMenus(pRequest) {
    $.ajax({
        type: "POST",
        url: "Menu.aspx/OrdenarMenus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var OJson = $.parseJSON(pRespuesta.d);
            if (OJson.Error) {
                $("#dialogOrdenarMenus").dialog("close");
                MostrarMensajeError(OJson.Mensaje);
            }
            else {
                $("#dialogOrdenarMenus").dialog("close");
            }
            ObtenerMenuPredeterminado();
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function SetFormaAltaMenu() 
{
    $.ajax({
        type: "POST",
        url: "../Formas/FormaAltaMenu.aspx",
        data: {},
        dataType: "html",
        success: function(pRespuesta){
            if (!$("#divFormulario").get(0)){
				$('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
			}
			else{
                $('#divFormulario').slideUp('slow', function() {
                    $('#formPrincipal').remove();
                    $('#col-2').append(pRespuesta);
                    $("#divFormulario").slideDown('slow');
                });
            }
            AjustarEtiquetas(".divColumna-4-label",150);
        }
    });
}

function SetFormaConsultarMenu(pIdMenu)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarMenu.aspx",
        data: {
            IdMenu: pIdMenu
        },
        dataType: "html",
        success: function(pRespuesta){
            $('#divFormulario').slideUp('slow', function() {
                $('#formPrincipal').remove();
                $('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
            });
        }
    });
}

function SetFormaEditarMenu(pIdMenu)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaEditarMenu.aspx",
        data: {
            IdMenu: pIdMenu
        },
        dataType: "html",
        success: function(pRespuesta){
            $('#divFormulario').slideUp('slow', function() {
                $('#formPrincipal').remove();
                $('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
            });
        }
    });
}