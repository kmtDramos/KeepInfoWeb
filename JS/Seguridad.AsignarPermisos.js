//----------DHTMLX----------//
//--------------------------//
function tonclick(id) {
    var idSeleccionado = tree.getSelectedItemId();
    var arrOpcion = idSeleccionado.split("|");
    if (arrOpcion.length > 1)
    {
        switch (arrOpcion[0])
        {
            case "Perfil":
                SetFormaConsultaAsignarPermisosPerfil(arrOpcion[1]);
                break;
            case "Pagina":
                SetFormaConsultaAsignarPermisosPagina(arrOpcion[1]);
                break;
        }
    }
};

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    SetXMLArbolPermisos();
    SetFormaInicioAsignarPermisos("Elige una opcion del arbol para configurar los permisos de perfiles y paginas.");
    $(document).tooltip({
        position: {
            my: "right top",
            at: "left top"
        }
    });
});

//---------Funciones--------//
//--------------------------//
function AgregarPermisosPerfil()
{
    var idPerfil = $("#divFormulario").attr("idPerfil");
	var opciones = "";
	$('#ulPermisosAsignados').children('li').each(function() {
	  opciones=opciones+$(this).attr("opcion")+"|"
	});
	var tamano = opciones.length;
	opciones = opciones.substring(0,tamano-1);
	SetAgregarPermisosPerfil(opciones,idPerfil);
}

function AgregarPermisosPagina()
{
    var idPagina = $("#divFormulario").attr("idPagina");
	var opciones = ""
	$('#ulPermisosAsignados').children('li').each(function() {
        opciones=opciones+$(this).attr("opcion")+"|";
	});
	var tamano = opciones.length;
	opciones = opciones.substring(0,tamano-1);
	SetAgregarPermisosPagina(opciones,idPagina);
}

//--------Validaciones-------//
//--------------------------//
function ValidaPagina(pPagina,pTitulo,pNombreMenu)
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
function SetXMLArbolPermisos() 
{
    $.ajax({
        type: "POST",
        url: "AsignarPermisos.aspx/JsonArbolPermisos",
        data: "",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = jQuery.parseJSON(pRespuesta.d);
            IniciarArbol(respuesta.Modelo);
        }
    });
}

//-----------AJAX------------//
//----Funciones de Accion----//
function SetAgregarPermisosPerfil(pOpciones,pIdPerfil)
{   
    var pRequest = "{'pOpciones':'"+pOpciones+"','pIdPerfil':"+pIdPerfil+"}";
    $.ajax({
        type: "POST",
        url: "AsignarPermisos.aspx/AgregarPermisosPerfil",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            SetFormaInicioAsignarPermisos("Se ha guardo con exito la asignación.");
        }
    });
}

function SetAgregarPermisosPagina(pOpciones,pPagina)
{   
    var pRequest = "{'pOpciones':'"+pOpciones+"','pPagina':"+pPagina+"}";
    $.ajax({
        type: "POST",
        url: "AsignarPermisos.aspx/AgregarPermisosPagina",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            SetFormaInicioAsignarPermisos("Se ha guardo con exito la asignación.");
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function SetFormaInicioAsignarPermisos(pMensaje)
{
    $.ajax({
        type: "POST",
        url: "../Formas/FormaInicioAsignarPermisos.aspx",
        data: {
            Mensaje: pMensaje
        },
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
        }
    });
}

function SetFormaConsultaAsignarPermisosPagina(pIdPagina)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarAsignarPermisosPagina.aspx",
        data: {
            IdPagina: pIdPagina
        },
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
        }
    });
}

function SetFormaConsultaAsignarPermisosPerfil(pIdPerfil)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarAsignarPermisosPerfil.aspx",
        data: {
            IdPerfil: pIdPerfil
        },
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
        }
    });
}

function SetFormaAltaAsignarPermisosPerfil(pIdPerfil)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaAltaAsignarPermisosPerfil.aspx",
        data: {
            IdPerfil: pIdPerfil
        },
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
            $("ul").livequery(function(){
                $(function() {
                    $( "ul.droptrue" ).sortable({
					    connectWith: "ul",
					    dropOnEmpty: true
				    });
                    $( "#ulPermisosAsignados, #ulPermisosDisponibles").disableSelection();
	            });
	        });
        }
    });
}

function SetFormaAltaAsignarPermisosPagina(pIdPagina)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaAltaAsignarPermisosPagina.aspx",
        data: {
            IdPagina: pIdPagina
        },
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
            $("ul").livequery(function(){
                $(function() {
                    $( "ul.droptrue" ).sortable({
					    connectWith: "ul",
					    dropOnEmpty: true
				    });
                    $( "#ulPermisosAsignados, #ulPermisosDisponibles").disableSelection();
	            });
	        });
        }
    });
}