<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mappingC.aspx.cs" Inherits="mapping.mappingC" %>
 
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CLOXXS - CONVERSIE VERPLICHTINGEN</title>
</head>
 <body style="font-family: Arial; font-size: 12px;">
    <form id="formMain" runat="server">

    <div style="height: 5px; width: 1150px; background-color: #ffffff;"></div>
    <div style="height: 20px; width: 1150px; background-color: #ffffff;">
        <asp:HyperLink ID="hyperlinkGrootboekrekeningen" runat="server" NavigateUrl="~/mappingGT.aspx">GRB-NR's & TAAK-NR's</asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink ID="hyperlinkActiva" runat="server" NavigateUrl="~/mappingA.aspx">ACTIVA</asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink ID="hyperlinkFCL" runat="server" NavigateUrl="~/mappingK.aspx">PROJECTEN KOPPELEN</asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink ID="hyperlinkChecklistBudgets" runat="server" NavigateUrl="~/checklistBudgets.aspx">BUDGETS ZONDER MAPPING</asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink ID="hyperlinkChecklistAccounts" runat="server" NavigateUrl="~/checklistAccounts.aspx">GRB-NR's ZONDER MAPPING</asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink ID="hyperlinkMappingF" runat="server" NavigateUrl="~/mappingF.aspx">CONVERSIE-BOEKINGEN</asp:HyperLink>&nbsp;&nbsp;
        <asp:HyperLink ID="hyperlink1" runat="server" NavigateUrl="~/mappingC.aspx">CONVERSIE-VERPLICHTINGEN</asp:HyperLink>&nbsp;&nbsp;
    </div>
    <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
    <div style="height: 5px; width: 1150px; background-color: #ffffff;"></div>
    <asp:Label ID="labelTitle" runat="server" Text="CONVERSIE VERPLICHTINGEN" Font-Bold="True" Font-Names="Arial" Font-Size="12px"></asp:Label>
    <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
    <div style="width: 1150px; float:none; background-color: #ffffff;"></div>
          
    <div style="width: 1150px; float:none; background-color: #ffffff;" />

    <asp:Label ID="lblSelectProgram" runat="server" Text="Selecteer programma" Width="175px"></asp:Label>
    <asp:DropDownList ID="DropDownListPrograms" runat="server" AutoPostBack="True" 
        DataSourceID="ObjectDataSource4" DataTextField="ProgramName" 
        DataValueField="ProgramID" Width="250px" Font-Names="Arial" 
        Font-Size="12px">
    </asp:DropDownList>
    <br />
    <asp:Label ID="lblSelectProject" runat="server" Text="Selecteer project" Width="175px"></asp:Label>
    <asp:DropDownList ID="DropDownListProjects" runat="server" AutoPostBack="True" 
        DataSourceID="ObjectDataSource2" DataTextField="ProjectName" 
        DataValueField="ProjectID" Width="350px" Font-Names="Arial" 
        Font-Size="12px" ondatabound="DropDownListProjects_DataBound">
    </asp:DropDownList>

    <br />
    <br />      
           <asp:Button ID="ButtonLog" runat="server" Font-Size="Smaller" 
            onclick="ButtonLog_Click" Text="Laat Log zien" />     
    <ajaxToolkit:AsyncFileUpload
        OnClientUploadError="uploadError" OnClientUploadComplete="uploadComplete" 
        runat="server" ID="AsyncFileUploadVerplichtingen" Width="250px" UploaderStyle="Traditional" 
        UploadingBackColor="#CCFFFF" ThrobberID="myThrobber"/>
           <asp:Button ID="Button1" runat="server" Text="Excel Inlezen" 
            Font-Size="Smaller" onclick="Button1_Click" />   <asp:Label runat="server" Text="&nbsp;" ID="uploadResult" /> 
            <asp:Label ID="InlezenExcelLabel" runat="server" ></asp:Label>
      
        &nbsp;
    <asp:Label runat="server" ID="myThrobber" style="display:none;" ></asp:Label>
       

    <br />

    <br />
    <asp:TextBox ID="textboxProcessingMessage" runat="server" Height="40px" Width="400px"></asp:TextBox>
  <br />
        <asp:TextBox ID="TextBoxLog" runat="server" Height="213px" TextMode="MultiLine" 
            Width="450px" Visible="false"></asp:TextBox>
  
    <table style="margin-top:9px; border-collapse: collapse; border-left: solid 1px #aaaaff; border-top: solid 1px #aaaaff;" runat="server" cellpadding="3" id="clientSide" />

    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
            
    <div style="height: 3px; width: 1150px; background-color: #000000;">

    <br />

    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
        TypeName="mapping.BusinessObjectDATA" 
        SelectMethod="ProjectenSelectByProgramID">
        <SelectParameters>
        <asp:ControlParameter ControlID="DropDownListPrograms" DefaultValue="" 
        Name="ProgramID" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" TypeName="mapping.BusinessObjectDATA" SelectMethod="ProgrammasSelectAll"></asp:ObjectDataSource>

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="Server" EnablePartialRendering="true" EnablePageMethods="true" /></div>

    <script type="text/javascript">

        function fillCell(row, cellNumber, text) {
            var cell = row.insertCell(cellNumber);
            cell.innerHTML = text;
            cell.style.borderBottom = cell.style.borderRight = "solid 1px #aaaaff";
        }

        function addToClientTable(name, text) {
            var table = document.getElementById("<%= clientSide.ClientID %>");
            var row = table.insertRow(0);
            fillCell(row, 0, "Bestand: " + "<span style='color:green;font-size: medium;'>" + name + "</span> Succesvol Opgeslagen");
            fillCell(row, 1, "Bestand Type:" + text);
        }

        function uploadError(sender, args) {
            addToClientTable(args.get_fileName(), "<span style='color:red;'>" + args.get_errorMessage() + "</span>");
        }

        function uploadComplete(sender, args) {
            //PageMethods.ImportVerplichtingenKnopKlaar();
            //location.reload(false);
        }

    </script>

    <asp:Label ID="lblRecordCount" runat="server" Text="&nbsp;"></asp:Label>

    <asp:GridView ID="GridView1" runat="server" 
        DataSourceID="ObjectVerplichtingenConversie" BackColor="White" 
        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            ondatabound="GridView1_DataBound">
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />
    </asp:GridView>

    <asp:ObjectDataSource ID="ObjectVerplichtingenConversie" runat="server" 
        SelectMethod="SelectVerplichtingenProject" TypeName="mapping.BusinessObjectDATA">
        <SelectParameters>
            <asp:ControlParameter ControlID="DropDownListProjects" Name="PROJECT_ID" 
                PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</form>
</body>
</html>
