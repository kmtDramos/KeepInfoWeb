<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaAltaGenerador.aspx.cs" Inherits="FormaAltaGenerador" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Clase
            <div class="divTituloFormularioAcciones">
            </div>
        </div>
        <div class="divAreaCamposLargo">
            <div class="divRenglon-Columna-4">
                <div class="divColumna-4-label"><asp:Label ID="lblClase" runat="server" CssClass="lblEtiqueta">Nombre de la Clase:</asp:Label></div>
                <div class="divColumna-4-input"><asp:TextBox ID="txtClase" runat="server" CssClass="txtCajaTexto"></asp:TextBox></div>
            </div>
        </div>
        <div class="divTituloFormulario divMargenTituloFormulario">
            Propiedades
            <div class="divTituloFormularioAcciones">
            </div>
        </div>
        <div class="divRenglon-Columna-4">
            <input type="checkbox" id="chkBaja" class="etiquetaOculta" /><button for="chkBaja" id="lblBaja">Baja</button>
            <button id="btnInteger" class="btnJQuery" tipoAtributo="I">Integer</button>
            <button id="btnString" class="btnJQuery" tipoAtributo="S">String</button>
            <button id="btnDecimal" class="btnJQuery" tipoAtributo="D">Decimal</button>
            <button id="btnDateTime" class="btnJQuery" tipoAtributo="DT">DateTime</button>
            <button id="btnBoolean" class="btnJQuery" tipoAtributo="B">Boolean</button>
        </div>
        <div id="divAtributos" class="divGeneradorAbributos">
            <div class="divFondoPermisos">Atributos</div>
            <ul id="ulListaAtributos" class="ulListaAtributos"></ul>
        </div>
        <div class="divAreaBotones">
            <input type="button" id="btnGuardar" value="Generar clase" class="button" onclick="javascript:AgregarGenerador();" />
        </div>
    </div>
</form>
