<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaAltaOpcion.aspx.cs" Inherits="FormaAltaOpcion" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Alta de Opciones
            <div class="divTituloFormularioAcciones"></div>
        </div>
        <table class="tblFormaOpcion">
            <tr>
                <td class="tblFormaOpcion-Etiqueta"><asp:Label ID="lblOpcion" runat="server" CssClass="lblEtiqueta">Opcion:</asp:Label></td>
                <td class="tblFormaOpcion-Control"><asp:TextBox ID="txtOpcion" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
                <td class="tblFormaOpcion-Etiqueta"><asp:Label ID="lblComando" runat="server" CssClass="lblEtiqueta">Comando:</asp:Label></td>
                <td class="tblFormaOpcion-Control"><asp:TextBox ID="txtComando" runat="server" CssClass="txtCajaTexto"></asp:TextBox></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:AgregarOpcion();"/>
        </div>
    </div>
</form>
