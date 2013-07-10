<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReadExcelFile.aspx.cs" Inherits="mapping.ReadExcelFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
    <div>
    <asp:label ID="Label1" runat="server" text="Label"></asp:label>
        <br />
 
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
            Text="Execute.." />
       
   
    </div>
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="Server" EnablePartialRendering="true" EnablePageMethods="true" />
    </form>
</body>
</html>
