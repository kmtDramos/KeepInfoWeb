<%@ Page Language="C#" MasterPageFile="MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="InicioSesion.aspx.cs" Inherits="InicioSesion" Title="Inicio Sesi&oacute;n" %>
<asp:Content ID="headSeguridadInicioSesion" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript"  src="js/InicioSesion.js"></script>
</asp:Content>
<asp:Content ID="bodySeguridadInicioSesion" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divContenido">
        <div id="divCajaInicioSesion">
            <div id="divTituloInicioSesion">Iniciar Sesión</div>
            <div id="divInicioSesion" idUsuario="0">
                <table id="tableInicioSesion">
                    <tr>
                        <td><asp:Label ID="lblUsuario" runat="server" CssClass="lblInicioSesion">Usuario:</asp:Label></td>
                        <td><asp:TextBox ID="txtUsuario" runat="server" CssClass="txtInicioSesion"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblContrasena" runat="server" CssClass="lblInicioSesion">Contraseña:</asp:Label></td>
                        <td><asp:TextBox ID="txtContrasena" TextMode="Password" runat="server" CssClass="txtInicioSesion"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <input type="button" id="btnEntrar" value="Entrar" class="button" onclick="javascript:IniciarSesion();"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
