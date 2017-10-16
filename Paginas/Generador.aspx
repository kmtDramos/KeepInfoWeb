<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Generador.aspx.cs" Inherits="Generador" Title="Generador" %>
<asp:Content ID="headSeguridadMenu" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Desarrollo.Generador.js"></script>
</asp:Content>
<asp:Content ID="bodySeguridadMenu" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divPrueba"></div>
    <div id="divContenido">
        <div id="divVistaFormas" class="renglon-top"></div>
        <div id="divGridCGeneradas" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID CLASES GENERADAS-->
                <table id="grdClasesGeneradas"></table>
                <div id="pagClasesGeneradas"></div>
                <!--FIN DE GRID CLASES GENERADAS-->
            </div>
        </div>
        <!-------------LISTADO TIPOS DE DATOS------------->
        <div id='divString' class='divDisplayNone'>
            <div class="divTipoAtributo" title='String'>S</div>
            <label>Nombre del Atributo:</label>
            <input type='text' class='txtCajaTexto' tipo="nombreAtributo" />
            <label class='lblMargenIzquierdo'>Longitud:</label>
            <input type='text' class='txtNumericoChico' tipo="longitud" value="0" onkeypress='javascript:return ValidarNumero(event, this.value);' onblur='javascript:return ValorPredeterminado(this,"0");' onfocus='javascript:return QuitarValorPredeterminado(this,"0");' />
            <div class='divEliminar' title='Eliminar' onclick='javascript:EliminarAtributo(this);'>X</div>
        </div>
        <div id="divInteger" class='divDisplayNone'>
            <div class="divTipoAtributo" title='Integer'>I</div>
            <label>Nombre del Atributo:</label>
            <input type='text' class='txtCajaTexto' tipo="nombreAtributo" />
            <div class='divEliminar' title='Eliminar' onclick='javascript:EliminarAtributo(this);'>X</div>
        </div>
        <div id='divDecimal' class='divDisplayNone'>
            <div class="divTipoAtributo" title='Decimal'>D</div>
            <label>Nombre del Atributo:</label>
            <input type='text' class='txtCajaTexto' tipo="nombreAtributo" />
            <label class='lblMargenIzquierdo'>Longitud:</label>
            <input type='text' class='txtNumericoChico' tipo="longitud" value='0' onkeypress='javascript:return ValidarNumero(event, this.value);' onblur='javascript:return ValorPredeterminado(this,"0");' onfocus='javascript:return QuitarValorPredeterminado(this,"0");' />
            <label class='lblMargenIzquierdo'>Número de Decimales:</label>
            <input type='text' class='txtNumericoChico' tipo="numeroDecimales" value='0' onkeypress='javascript:return ValidarNumero(event, this.value);' onblur='javascript:return ValorPredeterminado(this,"0");' onfocus='javascript:return QuitarValorPredeterminado(this,"0");' />
            <div class='divEliminar' title='Eliminar' onclick='javascript:EliminarAtributo(this);'>X</div>
        </div>
        <div id='divDateTime' class='divDisplayNone'>
            <div class="divTipoAtributo" title='DateTime'>DT</div>
            <label>Nombre del Atributo:</label>
            <input type='text' class='txtCajaTexto' tipo="nombreAtributo" />
            <div class='divEliminar' title='Eliminar' onclick='javascript:EliminarAtributo(this);'>X</div>
        </div>
        <div id='divBoolean' class='divDisplayNone'>
            <div class="divTipoAtributo" title='Boolean'>B</div>
            <label>Nombre del Atributo:</label>
            <input type='text' class='txtCajaTexto' tipo="nombreAtributo"/>
            <div class='divEliminar' title='Eliminar' onclick='javascript:EliminarAtributo(this);'>X</div>
        </div>
        <div id="divBajaBoolean" class="divDisplayNone">
            <div class="divTipoAtributo" title='Boolean'>B</div>
            <div class="divInformacionAtributo" style="padding-top:5px;">
                <label>Nombre del Atributo:</label>
                <span class="spanInfo">Baja</span>
            </div>
        </div>
        <!--------------------------------------------------->
    </div>
</asp:Content>
