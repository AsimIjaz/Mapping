<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checklistBudgets.aspx.cs" Inherits="mapping.checklistBudgets" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>CLOXXS - BUDGETS ZONDER MAPPING</title>
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
            <asp:Label ID="labelTitle" runat="server" 
                Text="BUDGETS ZONDER MAPPING" Font-Bold="True" Font-Names="Arial" 
                Font-Size="12px"></asp:Label>
            <div style="height: 5px; width: 1150px; background-color: #ffffff;"></div>
            <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
            <div style="width: 1150px; float:none; background-color: #ffffff;">

                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" SelectMethod="SelectBudgetsNotMapped">
                </asp:ObjectDataSource>

                <asp:Label ID="lblVernieuwen" runat="server" Text=" " Width="175px"></asp:Label>
                <asp:Button ID="btnRefresh" runat="server" Text="Vernieuwen" width="250px" 
                    Font-Names="Arial" Font-Size="12px" onclick="btnRefresh_Click"/>
                <br />
            </div>
            <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
            <div style="width: 1150px; float:none; background-color: #ffffff;">
                <asp:ScriptManager ID="ScriptManagerMain" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanelUserMessage" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="panelUserInteration" runat="server" Width="1150px" Height="50px">
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
            <div style="width: 1150px; float:none; background-color: #ffffff;">

            <asp:Label ID="lblRecordCount" runat="server" Text="&nbsp;"></asp:Label>

                <asp:GridView ID="GridView1" runat="server" DataSourceID="ObjectDataSource1" 
                    AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    Font-Names="Arial" Font-Size="12px" DataKeyNames="PROGRAMNAME" 
                    ondatabound="GridView1_DataBound" ondatabinding="GridView1_DataBinding">
                    <Columns>

                        <asp:BoundField DataField="PROGRAMNAME" HeaderText="PROGRAMMANAAM" 
                            ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="100px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="PROJECTNAME" HeaderText="PROJECTNAAM" 
                            ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="250px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SUBPROJECTNAME" HeaderText="DEELPROJECTNAAM" 
                            ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="250px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="BUDGETNAME" HeaderText="BUDGETNAAM" 
                            ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="250px" Wrap="True" />
                        </asp:BoundField>

                    </Columns>
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
            </div>
            <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
            <div style="height: 5px; width: 1150px; background-color: #ffffff;"></div>

        </form>
    </body>
</html>