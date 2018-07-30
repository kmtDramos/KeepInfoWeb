//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

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
                $(".qq-upload-list").html("<li><span class='qq-upload-file'>Favor de subir una imagen.</span></li>");
                $("#divImagenProducto").html("<img class='imagenNoDisponible' src='../images/NoImage.png'/>");
                $("#divImagenProducto").attr("archivo", "");
                $(this).dialog("close");
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarProducto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarProducto").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarProducto();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarProducto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarProducto").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            },
            "Cerrar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogEditarProducto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarProducto").remove();
        },
        buttons: {
            "Editar": function() {
                EditarProducto();
            },
            "Cerrar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogAgregarCaracteristica').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCaracteristica").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCaracteristicaProducto();
            },
            "Cerrar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarDescuentoProducto').dialog({
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

    $('#dialogAgregarDescuentoProducto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarDescuentoProducto").remove();
        },
        buttons: {
            "Aceptar": function() {
                AgregarDescuentoProducto();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarProducto", function() {
        ObtenerFormaAgregarProducto();
    });

    $("#grdProducto").on("click", ".imgFormaConsultarProducto", function() {
        var registro = $(this).parents("tr");
        var Producto = new Object();
        Producto.pIdProducto = parseInt($(registro).children("td[aria-describedby='grdProducto_IdProducto']").html());
        ObtenerFormaConsultarProducto(JSON.stringify(Producto));
    });

    $("#divSubirImagenProducto").livequery(function() {
        var ctrlSubirLogo = new qq.FileUploader({
            element: document.getElementById('divSubirImagenProducto'),
            action: '../ControladoresSubirArchivos/SubirImagenProducto.ashx',
            allowedExtensions: ["png", "jpg", "jpeg"],
            template: '<div class="qq-uploader">' +
                '<div class="qq-upload-drop-area"></div>' +
                '<div class="qq-upload-container-list"><ul class="qq-upload-list"><li><span class="qq-upload-file">Favor de subir una imagen.</span></li></ul></div>' +
                '<div class="qq-upload-container-buttons"><div id="divEliminarImagenProducto" class="qq-upload-button">- Borrar</div><div class="qq-upload-button qq-divBotonSubir">+ Agregar</div></div>' +
                '</div>',
            onSubmit: function(id, fileName) {
                $(".qq-upload-list").empty();
            },
            onComplete: function(id, file, responseJSON) {
                $("#divImagenProducto").html("<img src='../Archivos/ImagenProducto/" + responseJSON.name + "' title='Producto' onload='javascript:ImagenProductoCargado();'/>");
                $("#divImagenProducto").attr("archivo", responseJSON.name);
                OcultarBloqueo();
            }
        });
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('click', '#divEliminarImagenProducto', function(event) {
        if ($("#divImagenProducto").attr("archivo") != "") {
            MostrarMensajeEliminar("¿Esta seguro de eliminar la imagen del producto?");
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('focusin', '#txtCosto', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('focusout', '#txtCosto', function(event) {
        $(this).valorPredeterminado("Moneda");
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('keypress', '#txtCosto', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('focusin', '#txtValorMedida', function(event) {
        $(this).quitarValorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('focusout', '#txtValorMedida', function(event) {
        $(this).valorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('keypress', '#txtValorMedida', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('focusin', '#txtMargenUtilidad', function(event) {
        $(this).quitarValorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('focusout', '#txtMargenUtilidad', function(event) {
        $(this).valorPredeterminado("Porcentaje");
        if (parseInt($(this).val(), 10) > 100) {
            $(this).val("100");
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('keypress', '#txtMargenUtilidad', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
        else {
            if (LimitarPorcentaje(event, $(this).val())) {
                return false;
            }
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('keypress', '#txtCodigoBarra', function(event) {
        if (!ValidarLetraNumero(event, $(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbUnidadCompraVenta', function(event) {
        if ($(this).val() != 0) {
            $("#txtValorMedida").removeAttr("readonly");
            $("#spanMedida").text($('#cmbUnidadCompraVenta option:selected').text() + ":");
        }
        else {
            $("#txtValorMedida").attr("readonly", "readonly");
            $("#spanMedida").text("");
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbTipoMoneda', function(event) {
        $(".spanTipoMoneda").text($('#cmbTipoMoneda option:selected').text());
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('keyup', "#txtCosto, #txtMargenUtilidad", function(event) {
        var precio = CalcularPrecio();
        $("#spanPrecio").text(formato.moneda(precio, "$"));
        if ($("#cmbTipoIVA").val() == "1") {
            var precioIVA = (precio * parseFloat('1.' + $("#divFormaAgregarProducto, #divFormaEditarProducto").attr("IVA")));
            $("#spanPrecioIVA").text(formato.moneda(precioIVA, "$"));
        }
        else {
            $("#spanPrecioIVA").text(formato.moneda(precio, "$"));
        }
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbTipoIVA', function(event) {
        if ($(this).val() == "1") {
            var precio = CalcularPrecio();
            var precioIVA = (precio * parseFloat('1.' + $("#divFormaAgregarProducto, #divFormaEditarProducto").attr("IVA")));
            $("#spanPrecioIVA").text(formato.moneda(precioIVA, "$"));
        }
        else {
            var precio = CalcularPrecio();
            $("#spanPrecioIVA").text(formato.moneda(precio, "$"));
        }
    });

    $('#grdProducto').one('click', '.div_grdProducto_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdProducto_AI']").children().attr("baja")
        var idProducto = $(registro).children("td[aria-describedby='grdProducto_IdProducto']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idProducto, baja);
    });

    $('#dialogAgregarProducto').on('click', '#divAgregarCaracteristica', function(event) {
        ObtenerFormaAgregarCaracteristicaProducto("AgregarProducto");
    });

    $('#dialogEditarProducto').on('click', '#divAgregarCaracteristica', function(event) {
        ObtenerFormaAgregarCaracteristicaProducto("EditarProducto");
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('click', '.divEliminarCaracteristica', function(event) {
        var CaracteristicaProducto = new Object();
        CaracteristicaProducto.pIdCaracteristicaProducto = $(this).parents(".liCaracteristica").attr("idCaracteristicaProducto");
        if (CaracteristicaProducto.pIdCaracteristicaProducto != "0") {
            EliminarCaracteristicaProducto(JSON.stringify(CaracteristicaProducto));
        }
        $(this).parents(".liCaracteristica").remove();
    });

    $("#grdProducto").on("click", ".imgFormaConsultarDescuentoProducto", function() {
        var registro = $(this).parents("tr");
        var Producto = new Object();
        Producto.pIdProducto = parseInt($(registro).children("td[aria-describedby='grdProducto_IdProducto']").html());
        $("#dialogConsultarDescuentoProducto").attr("idProducto", Producto.pIdProducto);
        FiltroDescuentoProducto();
        $("#dialogConsultarDescuentoProducto").dialog("open");
    });

    $("#dialogConsultarDescuentoProducto").on("click", "#btnAgregarDescuentoProducto", function() {
        ObtenerFormaAgregarDescuentoProducto();
    });

    $('#dialogAgregarDescuentoProducto').on('focusin', '#txtDescuentoProducto', function(event) {
        $(this).quitarValorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarDescuentoProducto').on('focusout', '#txtDescuentoProducto', function(event) {
        $(this).valorPredeterminado("Porcentaje");
        if (parseInt($(this).val(), 10) > 100) {
            $(this).val("100");
        }
    });

    $('#dialogAgregarDescuentoProducto').on('keypress', '#txtDescuentoProducto', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
        else {
            if (LimitarPorcentajeNumero(event, $(this).val(), 100)) {
                return false;
            }
        }
    });

    $('#grdDescuentoProducto').one('click', '.div_grdDescuentoProducto_AI_DescuentoProducto', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdDescuentoProducto_AI_DescuentoProducto']").children().attr("baja")
        var idDescuentoProducto = $(registro).children("td[aria-describedby='grdDescuentoProducto_IdDescuentoProducto']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatusDescuentoProducto(idDescuentoProducto, baja);
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbGrupo', function(event) {
        var request = new Object();
        request.pIdGrupo = $(this).val();
        ObtenerListaCategorias(JSON.stringify(request));
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbLinea', function (event) {
        var request = new Object();
        request.pIdLinea = $(this).val();
        ObtenerListaEstantes(JSON.stringify(request));

    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbRack', function (event) {
        var request = new Object();
        request.pIdEstante = $(this).val(); console.log(request);
        ObtenerListaRepisas(JSON.stringify(request));
    });

    $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbCategoria', function(event) {
        var request = new Object();
        request.pIdCategoria = $(this).val();
        ObtenerListaSubCategorias(JSON.stringify(request));

    });
    $(window).load(function() {
        $("#gs_Grupo").change(function() {
            var Grupo = new Object();
            Grupo.pIdGrupo = $(this).val();
            $.ajax({
                type: "POST",
                url: "Producto.aspx/ObtenerFiltroCategorias",
                data: JSON.stringify(Grupo),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function(answer) {
                    var json = JSON.parse(answer.d);
                    var firstOption = $($("option:eq(0)", "#gs_Categoria"));
                    $("#gs_Categoria").html('');
                    var opciones = json.Modelo.Opciones;
                    $("#gs_Categoria").append(firstOption);
                    for (i in opciones) {
                        var opcion = opciones[i];
                        var valor = opcion.Valor;
                        var descripcion = opcion.Descripcion;
                        $("#gs_Categoria").append($('<option value="' + valor + '">' + descripcion + '</option>'));
                    }
                }
            });
        });
        $("#gs_Categoria").change(function() {
            var Categoria = new Object();
            Categoria.pIdCategoria = $(this).val();
            $.ajax({
                type: "POST",
                url: "Producto.aspx/ObtenerFiltroSubGrupos",
                data: JSON.stringify(Categoria),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function(answer) {
                    var json = JSON.parse(answer.d);
                    var firstOption = $($("option:eq(0)", "#gs_SubGrupo"));
                    $("#gs_SubGrupo").html('');
                    var opciones = json.Modelo.Grupos;
                    $("#gs_SubGrupo").append(firstOption);
                    for (i in opciones) {
                        var opcion = opciones[i];
                        var valor = opcion.Valor;
                        var descripcion = opcion.Descripcion;
                        $("#gs_SubGrupo").append($('<option value="' + valor + '">' + descripcion + '</option>'));
                    }
                }
            });
        });
    });

});

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarProducto() {
    var pProducto = new Object();
    pProducto.ClaveProdServ = $("#txtClaveProdServ").val();
    pProducto.Producto = $("#txtProducto").val();
    pProducto.Clave = $("#txtClave").val();
    pProducto.NumeroParte = $("#txtNumeroParte").val();
    pProducto.Modelo = $("#txtModelo").val();
    pProducto.CodigoBarra = $("#txtCodigoBarra").val();
    pProducto.IdMarca = $("#cmbMarca").val();
    pProducto.IdGrupo = $("#cmbGrupo").val();
    pProducto.IdCategoria = $("#cmbCategoria").val();
    pProducto.Descripcion = $("#txtDescripcion").val();
    pProducto.Costo = $("#txtCosto").val();
    pProducto.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pProducto.MargenUtilidad = $("#txtMargenUtilidad").val();
    pProducto.IdTipoVenta = $("#cmbTipoVenta").val();
    pProducto.IdTipoIVA = $("#cmbTipoIVA").val();
    pProducto.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pProducto.ValorMedida = $("#txtValorMedida").val();
    pProducto.Imagen = $("#divImagenProducto").attr("archivo");
    pProducto.IdSubCategoria = $("#cmbSubCategoria").val();
    pProducto.IdDivision = parseInt($("#cmbDivision").val());
    pProducto.CodigoInterno = $("#txtCodigoInterno").val();

    pProducto.IdLinea = ($("#cmbLinea").val() != undefined || !isNull($("#cmbLinea").val())) ? $("#cmbLinea").val() : 0;
    pProducto.IdEstante = ($("#cmbRack").val() != undefined || !isNull($("#cmbRack").val())) ? $("#cmbRack").val() : 0;
    pProducto.IdRepisa = ($("#cmbRepisa").val() != undefined || !isNull($("#cmbRepisa").val())) ? $("#cmbRepisa").val() : 0;

    console.log(pProducto);
    pProducto.Costo = pProducto.Costo.replace("$","");
    pProducto.Costo = pProducto.Costo.split(",").join("");
    pProducto.Costo = parseFloat(pProducto.Costo);
    pProducto.MargenUtilidad = parseFloat(pProducto.MargenUtilidad, 10);
    if(pProducto.ValorMedida == "")
    { pProducto.ValorMedida = 0; }
    pProducto.ValorMedida = parseInt(pProducto.ValorMedida, 10);
    
    var aCaracteristicas = new Array();
    $(".liCaracteristica").each(function(i){
        var caracteristicaProducto = new Object();
        caracteristicaProducto.IdCaracteristica = $(this).find(".divCaracteristica").attr("idCaracteristica");
        caracteristicaProducto.Valor = $(this).find(".divValor").attr("valor");
        caracteristicaProducto.IdUnidadCaracteristica = $(this).find(".divUnidadCaracteristica").attr("IdUnidadCaracteristica");
        aCaracteristicas.push(caracteristicaProducto);
    });
    pProducto.CaracteristicasProducto = aCaracteristicas;
        
    var validacion = ValidarProducto(pProducto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    
    var oRequest = new Object();
    oRequest.pProducto = pProducto;
    
    SetAgregarProducto(JSON.stringify(oRequest));
}

function SetAgregarProducto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Producto.aspx/AgregarProducto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProducto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarProducto").dialog("close");
        }
    });
}

function EditarProducto() {
    var pProducto = new Object();

    pProducto.IdProducto = $("#divFormaEditarProducto").attr("idProducto");
    pProducto.Producto = $("#txtProducto").val();
    pProducto.ClaveProdServ = $("#txtClaveProdServ").val();
    pProducto.Clave = $("#txtClave").val();
    pProducto.NumeroParte = $("#txtNumeroParte").val();
    pProducto.Modelo = $("#txtModelo").val();
    pProducto.CodigoBarra = $("#txtCodigoBarra").val();
    pProducto.IdMarca = $("#cmbMarca").val();
    pProducto.IdGrupo = $("#cmbGrupo").val();
    pProducto.IdCategoria = $("#cmbCategoria").val();
    pProducto.Descripcion = $("#txtDescripcion").val();
    pProducto.Costo = $("#txtCosto").val();
    pProducto.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pProducto.MargenUtilidad = $("#txtMargenUtilidad").val();
    pProducto.IdTipoVenta = $("#cmbTipoVenta").val();
    pProducto.IdTipoIVA = $("#cmbTipoIVA").val();
    pProducto.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pProducto.ValorMedida = $("#txtValorMedida").val();
    pProducto.Imagen = $("#divImagenProducto").attr("archivo");
    pProducto.IdSubCategoria = $("#cmbSubCategoria").val();
    pProducto.IdDivision = parseInt($("#cmbDivision").val());
    pProducto.CodigoInterno = $("#txtCodigoInterno").val();

    pProducto.IdLinea = ($("#cmbLinea").val() != undefined || !isNull($("#cmbLinea").val())) ? $("#cmbLinea").val() : 0;
    pProducto.IdEstante = ($("#cmbRack").val() != undefined || !isNull($("#cmbRack").val())) ? $("#cmbRack").val() : 0;
    pProducto.IdRepisa = ($("#cmbRepisa").val() != undefined || !isNull($("#cmbRepisa").val())) ? $("#cmbRepisa").val() : 0;
    
    pProducto.Costo = pProducto.Costo.replace("$","");
    pProducto.Costo = pProducto.Costo.split(",").join("");
    pProducto.Costo = parseFloat(pProducto.Costo);
    pProducto.MargenUtilidad = parseFloat(pProducto.MargenUtilidad, 10);
    if(pProducto.ValorMedida == "")
    
    { pProducto.ValorMedida = 0; }
    pProducto.ValorMedida = parseInt(pProducto.ValorMedida, 10);
    
    var validacion = ValidarProducto(pProducto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    
    var oRequest = new Object();
    oRequest.pProducto = pProducto;
    SetEditarProducto(JSON.stringify(oRequest));
}

function SetEditarProducto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Producto.aspx/EditarProducto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProducto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarProducto").dialog("close");
        }
    });
}

function CalcularPrecio() {
    var costo = $("#txtCosto").val();
    if(costo == "")
    { costo = "0";}
    costo = costo.replace("$","");
    costo = costo.split(",").join("");
    costo = parseFloat(costo);
    
    var margenUtilidad = $("#txtMargenUtilidad").val();
    if(margenUtilidad == "")
    { margenUtilidad = "0";}
    var precio = (costo * 100) / (100 - parseFloat(margenUtilidad));
    return precio;
}

function AgregarCaracteristicaProducto() {
    if($("#dialogAgregarCaracteristica").attr("forma") == "AgregarProducto")
    {
        var pCaracteristicaProducto = new Object();
        pCaracteristicaProducto.IdCaracteristicaProducto = "0";
        pCaracteristicaProducto.IdCaracteristica = $("#cmbCaracteristica").val();
        pCaracteristicaProducto.Caracteristica = $("#cmbCaracteristica option:selected").text();
        pCaracteristicaProducto.Valor = $("#txtValor").val();
        pCaracteristicaProducto.IdUnidadCaracteristica = $('#cmbUnidadCaracteristica').val();
        pCaracteristicaProducto.UnidadCaracteristica = $('#cmbUnidadCaracteristica option:selected').text();
    }
    else
    {
        var pCaracteristicaProducto = new Object();
        pCaracteristicaProducto.pIdCaracteristica = $("#cmbCaracteristica").val();
        pCaracteristicaProducto.pCaracteristica = $("#cmbCaracteristica option:selected").text();
        pCaracteristicaProducto.pValor = $("#txtValor").val();
        pCaracteristicaProducto.pIdUnidadCaracteristica = $('#cmbUnidadCaracteristica').val();
        pCaracteristicaProducto.pUnidadCaracteristica = $('#cmbUnidadCaracteristica option:selected').text();
        pCaracteristicaProducto.pIdProducto = $("#divFormaEditarProducto").attr("idProducto");
    }
    
    var validacion = ValidarCaracteristicaProducto(pCaracteristicaProducto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    SetAgregarCaracteristicaProducto(pCaracteristicaProducto);
}

function SetAgregarCaracteristicaProducto(pRequest) {
    if($("#dialogAgregarCaracteristica").attr("forma") == "AgregarProducto")
    {
        $("#ulListadoCaracteristicas").obtenerVista({
            modelo: pRequest,
            remplazarVista: false,
            nombreTemplate: "tmplConsultarCaracteristicaProducto.html",
            despuesDeCompilar: function(pRespuesta) {
                $("#dialogAgregarCaracteristica").dialog("close");
            }
        });        
    }
    else
    {
        MostrarBloqueo();
        $.ajax({
            type: "POST",
            url: "Producto.aspx/AgregarCaracteristicaProducto",
            data: JSON.stringify(pRequest),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error == 0) {
                    $("#ulListadoCaracteristicas").obtenerVista({
                        modelo: respuesta.Modelo,
                        remplazarVista: false,
                        nombreTemplate: "tmplConsultarCaracteristicaProducto.html",
                        despuesDeCompilar: function(pRespuesta) {
                            $("#dialogAgregarCaracteristica").dialog("close");
                        }
                    });
                }
                else {
                    MostrarMensajeError(respuesta.Descripcion);
                }
            },
            complete: function() {
                OcultarBloqueo();
            }
        });
    }
}

function EliminarCaracteristicaProducto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Producto.aspx/EliminarCaracteristicaProducto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function AgregarDescuentoProducto() {
    var pDescuentoProducto = new Object();
    pDescuentoProducto.IdProducto = $("#dialogConsultarDescuentoProducto").attr("idProducto");
    pDescuentoProducto.Descuento = $("#txtDescuentoProducto").val();
    pDescuentoProducto.DescripcionDescuento = $("#txtDescripcion").val();
        
    var validacion = ValidarDescuentoProducto(pDescuentoProducto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    
    var oRequest = new Object();
    oRequest.pDescuentoProducto = pDescuentoProducto;
    SetAgregarDescuentoProducto(JSON.stringify(oRequest));
}

function SetAgregarDescuentoProducto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Producto.aspx/AgregarDescuentoProducto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDescuentoProducto").trigger("reloadGrid");
                $("#dialogAgregarDescuentoProducto").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarProducto()
{
    $("#dialogAgregarProducto").obtenerVista({
        url: "Producto.aspx/ObtenerFormaAgregarProducto",
        nombreTemplate: "tmplAgregarProducto.html",
        despuesDeCompilar: function(pRespuesta) {
            var respuesta = pRespuesta.modelo;
            $("#cmbTipoVenta option[value=4]").attr("selected", true);
            $("#cmbTipoMoneda option[value=1]").attr("selected", true); 
            $("#dialogAgregarProducto").dialog("open");
        }
    });
}

function ObtenerFormaConsultarProducto(pRequest) {
    $("#dialogConsultarProducto").obtenerVista({
        nombreTemplate: "tmplConsultarProducto.html",
        url: "Producto.aspx/ObtenerFormaConsultarProducto",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarProducto == 1) {
                $("#dialogConsultarProducto").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Producto = new Object();
                        Producto.pIdProducto = parseInt($("#divFormaConsultarProducto").attr("IdProducto"));
                        ObtenerFormaEditarProducto(JSON.stringify(Producto))
                    },
                    "Cerrar": function() {
                        $(this).dialog("close");
                    }
                });
                $("#dialogConsultarProducto").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarProducto").dialog("option", "buttons", {});
                $("#dialogConsultarProducto").dialog("option", "height", "100");
            }
            $("#dialogConsultarProducto").dialog("open");
            
        }
    });
}

function ObtenerFormaEditarProducto(pRequest) {
    $("#dialogEditarProducto").obtenerVista({
        nombreTemplate: "tmplEditarProducto.html",
        url: "Producto.aspx/ObtenerFormaEditarProducto",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarProducto").dialog("open");
        }
    });
}

function ObtenerFormaAgregarCaracteristicaProducto(pForma)
{
    $("#dialogAgregarCaracteristica").attr("Forma", pForma);
    $("#dialogAgregarCaracteristica").obtenerVista({
        url: "Producto.aspx/ObtenerFormaAgregarCaracteristica",
        nombreTemplate: "tmplAgregarCaracteristica.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCaracteristica").dialog("open");
        }
    });
}

function ImagenProductoCargado(){
    var marginLeft = (250 - parseInt($("#divImagenProducto img").css("width").replace("px",""),10)) / 2;
    $("#divImagenProducto img").css("margin-left", marginLeft);
}

function SetCambiarEstatus(pIdProducto, pBaja) {
    var pRequest = "{'pIdProducto':" + pIdProducto + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Producto.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdProducto").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdProducto').one('click', '.div_grdProducto_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdProducto_AI']").children().attr("baja")
                var idProducto = $(registro).children("td[aria-describedby='grdProducto_IdProducto']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idProducto, baja);
            });
        }
    });
}

function SetCambiarEstatusDescuentoProducto(pIdDescuentoProducto, pBaja) {
    var pRequest = "{'pIdDescuentoProducto':" + pIdDescuentoProducto + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Producto.aspx/CambiarEstatusDescuentoProducto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdDescuentoProducto").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdDescuentoProducto').one('click', '.div_grdDescuentoProducto_AI_DescuentoProducto', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdDescuentoProducto_AI_DescuentoProducto']").children().attr("baja")
                var idDescuentoProducto = $(registro).children("td[aria-describedby='grdDescuentoProducto_IdDescuentoProducto']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusDescuentoProducto(idDescuentoProducto, baja);
            });
        }
    });
}

function ObtenerFormaAgregarDescuentoProducto() {
    $("#dialogAgregarDescuentoProducto").obtenerVista({
        nombreTemplate: "tmplAgregarDescuentoProducto.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarDescuentoProducto").dialog("open");
        }
    });
}

function ObtenerListaSubCategorias(pRequest) {
    $("#cmbSubCategoria").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaSubCategorias",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            
        }
    });
}

function ObtenerListaRepisas(pRequest) {
    $("#cmbRepisa").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaRepisas",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
            console.log("pprraaaa");
        }
    });
}

//----------Validaciones----------//
//--------------------------------//
function ValidarProducto(pProducto) {
    var errores = "";

    if (pProducto.Producto == "")
    { errores = errores + "<span>*</span> El campo nombre del producto esta vacío, favor de capturarlo.<br />"; }
    /*if (pProducto.Clave == "")
    { errores = errores + "<span>*</span> El campo clave esta vacío, favor de capturarlo.<br />"; }*/
    if (pProducto.NoParte == "")
    { errores = errores + "<span>*</span> El campo número de parte esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.Modelo == "")
    { errores = errores + "<span>*</span> El campo modelo esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.IdMarca == 0)
    { errores = errores + "<span>*</span> El campo marca esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.IdGrupo == 0)
    { errores = errores + "<span>*</span> El campo grupo esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.IdCategoria == 0)
    { errores = errores + "<span>*</span> El campo categoría esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacío, favor de capturarlo.<br />"; }
    /*if (pProducto.Costo == 0)
    { errores = errores + "<span>*</span> El campo costo esta vacío, favor de capturarlo.<br />"; }*/
    /*if (parseFloat(pProducto.MargenUtilidad) == 0)
    { errores = errores + "<span>*</span> El campo margen de utilidad esta vacío, favor de capturarlo.<br />"; }*/
    if (pProducto.IdTipoVenta == 0)
    { errores = errores + "<span>*</span> El campo tipo venta esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.IdUnidadCompraVenta == 0)
    { errores = errores + "<span>*</span> El campo unidad compra venta esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.ClaveProdServ == "")
    { errores = errores + "<span>*</span> El campo Clave Prod / Serv (SAT) esta vacío, favor de capturarlo.<br />"; }
    if (pProducto.ClaveProdServ.length != 8 )
    { errores = errores + "<span>*</span> El campo Clave Prod / Serv (SAT) debe ser 8 digitos, favor de corregir.<br />"; }
    if(pProducto.IdUnidadCompraVenta != 0)
    {
        /*if (pProducto.ValorMedida == 0 || pProducto.ValorMedida == "")
        { errores = errores + "<span>*</span> El campo " + $('#cmbUnidadCompraVenta option:selected').text().toLowerCase() +  " esta vacío, favor de capturarlo.<br />"; }*/
    }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarDescuentoProducto(pDescuentoProducto) {
    var errores = "";

    if (parseFloat(pDescuentoProducto.Descuento) == 0)
    { errores = errores + "<span>*</span> El campo descuento esta vacío, favor de capturarlo.<br />"; }
    if (pDescuentoProducto.DescripcionDescuento == "")
    { errores = errores + "<span>*</span> El campo descripción del descuento esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarCaracteristicaProducto(pCaracteristicaProducto) {
    var errores = "";

    if($("#dialogAgregarCaracteristica").attr("forma") == "AgregarProducto") {
        if (pCaracteristicaProducto.pIdCaracteristica == 0)
        { errores = errores + "<span>*</span> El campo caracteristica del producto esta vacío, favor de capturarlo.<br />"; }
        if (pCaracteristicaProducto.pValor == "")
        { errores = errores + "<span>*</span> El campo valor esta vacío, favor de capturarlo.<br />"; }
    }
    else {
        if (pCaracteristicaProducto.IdCaracteristica == 0)
        { errores = errores + "<span>*</span> El campo caracteristica del producto esta vacío, favor de capturarlo.<br />"; }
        if (pCaracteristicaProducto.Valor == "")
        { errores = errores + "<span>*</span> El campo valor esta vacío, favor de capturarlo.<br />"; }
    }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ObtenerListaMarca() {
    var request = new Object();
    request.pIdMarca = $("#cmbMarca").val();
    $("#cmbMarca").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaMarcas",
        parametros: JSON.stringify(request)
    });
}

function ObtenerListaGrupos() {
    var request = new Object();
    request.pIdGrupo = $("#cmbGrupo").val();
    $("#cmbGrupo").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaGrupos",
        parametros: JSON.stringify(request)
    });
}

function ObtenerListaCategorias(pRequest) {
    $("#cmbCategoria").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaCategorias",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            var request = new Object();
            request.pIdCategoria = $("#cmbCategoria").val();
            ObtenerListaSubCategorias(JSON.stringify(request));   
        }
    });
}

function ObtenerListaEstantes(pRequest) {
    $("#cmbRack").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaEstantes",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
            /*
            $('#dialogAgregarProducto, #dialogEditarProducto').on('change', '#cmbRack', function (event) {
                var request = new Object();
                request.pIdEstante = $(this).val(); console.log(request);
                ObtenerListaRepisas(JSON.stringify(request));
            });
            */
        }
    });
}

function ObtenerListaTipoVenta() {
    var request = new Object();
    request.pIdTipoVenta = $("#cmbTipoVenta").val();
    $("#cmbTipoVenta").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaTiposVenta",
        parametros: JSON.stringify(request)
    });
}

function ObtenerListaUnidadCompraVenta() {
    var request = new Object();
    request.pIdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    $("#cmbUnidadCompraVenta").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Producto.aspx/ObtenerListaUnidadesCompraVenta",
        parametros: JSON.stringify(request)
    });
}

function FiltroDescuentoProducto() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDescuentoProducto').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDescuentoProducto').getGridParam('page');
    request.pColumnaOrden = $('#grdDescuentoProducto').getGridParam('sortname');
    request.pTipoOrden = $('#grdDescuentoProducto').getGridParam('sortorder');
    request.pIdProducto = 0;
    if ($("#dialogConsultarDescuentoProducto").attr("IdProducto") != null) {
        request.pIdProducto = $("#dialogConsultarDescuentoProducto").attr("IdProducto");
    }
    request.pBaja = -1
    if ($("#gs_AI_DescuentoProducto").existe()) {
        request.pBaja = $("#gs_AI_DescuentoProducto").val();
    }
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Producto.aspx/ObtenerDescuentoProducto',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDescuentoProducto')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}