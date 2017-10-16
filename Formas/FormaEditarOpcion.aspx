<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaEditarOpcion.aspx.cs" Inherits="FormaEditarOpcion" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Editar Opcion
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones"><span onclick="javascript:EliminarOpcion();">Eliminar</span></div>
            </div>
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
            <input type="button" id="btnCancelar" value="Cancelar" class="button" onclick="javascript:SetFormaAltaOpcion();" tabindex="5"/>
            <input type="button" id="btnGuardar" value="Guardar" class="button" onclick="javascript:EditarOpcion();" tabindex="4"/>
        </div>
    </div>
</form>
