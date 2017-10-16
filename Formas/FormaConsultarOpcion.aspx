<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaConsultarOpcion.aspx.cs" Inherits="FormaConsultarOpcion" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Consultar Opcion
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones"><span onclick="javascript:EliminarOpcion();">Eliminar</span></div>
            </div>
        </div>
        <table class="tblFormaOpcion">
            <tr>
                <td class="tblFormaOpcion-Etiqueta"><asp:Label ID="lblOpcion" runat="server" CssClass="lblEtiqueta">Opcion:</asp:Label></td>
                <td class="tblFormaOpcion-Control"><asp:Label ID="lblOpcionConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
                <td class="tblFormaOpcion-Etiqueta"><asp:Label ID="lblComando" runat="server" CssClass="lblEtiqueta">Comando:</asp:Label></td>
                <td class="tblFormaOpcion-Control"><asp:Label ID="lblComandoConsulta" runat="server" CssClass="lblEtiquetaConsulta"></asp:Label></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnRegresar" value="Regresar" class="button" onclick="javascript:SetFormaAltaOpcion();"/>
            <input type="button" id="btnEditar" value="Editar" class="button" runat="server" />
        </div>
    </div>
</form>