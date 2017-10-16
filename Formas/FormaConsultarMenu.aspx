<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaConsultarMenu.aspx.cs" Inherits="FormaConsultarMenu" %>
<form id="formPrincipal" runat="server">
    <div id="divFormulario" class="etiquetaOculta" runat="server">
        <div class="divTituloFormulario">
            Consultar Menú
            <div class="divTituloFormularioAcciones">
                <div class="divAcciones">
                    <span onclick="javascript:ObtenerSubmenus();">Ordenar Submenús</span> | 
                    <span onclick="javascript:EliminarMenu();">Eliminar</span>
                </div>
            </div>
        </div>
        <table class="tblFormaMenu">
            <tr>
                <td class="tblFormaMenu-Etiqueta"><asp:Label ID="lblMenu" runat="server" CssClass="lblEtiqueta">Menu:</asp:Label></td>
                <td class="tblFormaMenu-Control"><asp:Label ID="lblMenuConsulta" runat="server"></asp:Label></td>
                <td class="tblFormaMenu-Etiqueta"><asp:Label ID="lblProyectoSistema" runat="server" CssClass="lblEtiqueta">ProyectoSistema:</asp:Label></td>
                <td class="tblFormaMenu-Control"><asp:Label ID="lblProyectoSistemaConsulta" runat="server"></asp:Label></td>
            </tr>
        </table>
        <div class="divAreaBotones">
            <input type="button" id="btnRegresar" value="Regresar" class="button" onclick="javascript:SetFormaAltaMenu();"/>
            <input type="button" id="btnEditar" value="Editar" class="button" runat="server" />
        </div>
    </div>
</form>