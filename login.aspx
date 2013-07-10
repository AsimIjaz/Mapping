<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="mapping.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #E6E6E6;">
    <form id="form1" runat="server">
    
        <div align="center">
            
            <table background="./Images/login_box.jpg" height="282" width="424" valign="center" style="margin-top: 20%">
                <tr height="70">
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr height="20" valign="bottom">
                    <td width="30">&nbsp;</td>
                    <td align="left" colspan="2"><asp:Label ID="labelUserMessage" runat="server" Text="" Width="250px" CssClass="styleLabel"></asp:Label></td>
                    <td>&nbsp;</td>
                </tr>
                <tr height="20" valign="bottom">
                    <td width="30">&nbsp;</td>
                    <td align="left"><asp:Label ID="labelUsername" runat="server" Text="Gebruikersnaam:" Width="100px" CssClass="styleLabel"></asp:Label></td>
                    <td><asp:TextBox ID="textboxUsername" runat="server" CssClass="styleLoginControls" Width="150px" ></asp:TextBox></td>
                    <td>&nbsp;</td>
                </tr>
                <tr height="20" valign="top">
                    <td>&nbsp;</td>
                    <td align="left"><asp:Label ID="labelPassword" runat="server" Text="Wachtwoord:" Width="100px" CssClass="styleLabel"></asp:Label></td>
                    <td><asp:TextBox ID="textboxPassword" runat="server" CssClass="styleLoginControls" Width="150px" TextMode="Password"></asp:TextBox></td>
                    <td>&nbsp;</td>
                </tr>
                <tr height="20">
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td valign="top"><asp:Button ID="buttonCheckCredentials" runat="server" Text="Inloggen" onclick="buttonCheckCredentials_Click" />&nbsp;&nbsp;&nbsp;</td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
