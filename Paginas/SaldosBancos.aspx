<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="SaldosBancos.aspx.cs" Inherits="Paginas_SaldosBancos" Title="Saldos bancos" %>
<asp:Content ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/Contabilidad.SaldosBancos.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div style="padding:8px;">
        <div>
            <table>
                <tr>
                    <td>Banco:</td>
                    <td>
                        <select id="cmbBanco" style="width:173px;">
                            <option value="-1">-Todos-</option
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>Cuenta bancaria:</td>
                    <td>
                        <select id="cmbCuentaBancaria" style="width:173px;">
                            <option value="-1">-Todas-</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>Fecha inicio:</td>
                    <td>
                        <input type="text" id="txtFechaInicio" value="<%=DateTime.Today.ToShortDateString() %>" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <button type="button" class="buttonLTR" id="btnBuscarSaldos">Buscar saldos</button>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <div id="SaldosBancos"></div>
    </div>
</asp:Content>