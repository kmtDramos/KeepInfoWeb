<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormaConsultarDatosUsuario.aspx.cs" Inherits="FormaConsultarDatosUsuario" %>
<form id="frmDatosUsuario" runat="server">
    <div id="divDatosUsuario">
        <table class="tblDatosUsuario">
            <tr>
                <td class="tblDatosUsuario-Etiqueta"><asp:Label ID="lblDatosNombre" runat="server" CssClass="lblEtiqueta">Nombre:</asp:Label></td>
                <td class="tblDatosUsuario-Control"><asp:TextBox ID="txtDatosNombre" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblDatosUsuario-Etiqueta"><asp:Label ID="lblDatosApellidoPaterno" runat="server" CssClass="lblEtiqueta">A. Paterno:</asp:Label></td>
                <td class="tblDatosUsuario-Control"><asp:TextBox ID="txtDatosApellidoPaterno" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblDatosUsuario-Etiqueta"><asp:Label ID="lblDatosApellidoMaterno" runat="server" CssClass="lblEtiqueta">A. Materno:</asp:Label></td>
                <td class="tblDatosUsuario-Control"><asp:TextBox ID="txtDatosApellidoMaterno" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblDatosUsuario-Etiqueta"><asp:Label ID="lblDatosUsuario" runat="server" CssClass="lblEtiqueta">Usuario:</asp:Label></td>
                <td class="tblDatosUsuario-Control"><asp:TextBox ID="txtDatosUsuario" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblDatosUsuario-Etiqueta"><asp:Label ID="lblDatosFechaNacimiento" runat="server" CssClass="lblEtiqueta">Fecha de Nacimiento:</asp:Label></td>
                <td class="tblDatosUsuario-Control"><asp:TextBox ID="txtDatosFechaNacimiento" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblDatosUsuario-Etiqueta"><asp:Label ID="lblDatosCorreo" runat="server" CssClass="lblEtiqueta">Correo:</asp:Label></td>
                <td class="tblDatosUsuario-Control"><asp:TextBox ID="txtDatosCorreo" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tblDatosUsuario-Control" colspan="2"><button id="btnCambiarContrasena">Cambiar contraseña</button></td>
            </tr>
        </table>
    </div>
</form>