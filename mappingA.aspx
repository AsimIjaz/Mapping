﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mappingA.aspx.cs" Inherits="mapping.mappingA" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>CLOXXS - ACTIVA</title>
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
            <asp:Label ID="labelTitle" runat="server" Text="ACTIVA" Font-Bold="True" 
                Font-Names="Arial" Font-Size="12px"></asp:Label>
            <div style="height: 5px; width: 1150px; background-color: #ffffff;"></div>
            <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
            <div style="width: 1150px; float:none; background-color: #ffffff;">
                <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" 
                    SelectMethod="ProjectenSelectByProgramID">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownListPrograms" Name="ProgramID" 
                            PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" SelectMethod="ProgrammasSelectAll">
                </asp:ObjectDataSource>
                
                <asp:Label ID="lblSelectProgramma" runat="server" Text="Selecteer programma" Width="175px"></asp:Label>
                <asp:DropDownList ID="DropDownListPrograms" runat="server" AutoPostBack="True" 
                    DataSourceID="ObjectDataSource3" DataTextField="ProgramName" 
                    DataValueField="ProgramID" Width="250px" Font-Names="Arial" 
                    Font-Size="12px">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblSelectProject" runat="server" Text="Selecteer project" Width="175px"></asp:Label>
                <asp:DropDownList ID="DropDownListProjects" runat="server" AutoPostBack="True" 
                    DataSourceID="ObjectDataSource2" DataTextField="ProjectName" 
                    DataValueField="ProjectID" Width="250px" Font-Names="Arial" 
                    Font-Size="12px">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblVernieuwen" runat="server" Text=" " Width="175px"></asp:Label>
                <asp:Button ID="btnRefresh" runat="server" Text="Vernieuwen" width="250px" Font-Names="Arial" Font-Size="12px" onclick="btnRefresh_Click"/><br />
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
            <asp:Button ID="btnAdd" runat="server" Text="Toevoegen" width="250px" Font-Names="Arial" Font-Size="12px" onclick="btnAdd_Click"/>&nbsp;&nbsp;
            <asp:TextBox ID="textboxNewActivaNummer" width="250px" Font-Names="Arial" Font-Size="12px" runat="server"></asp:TextBox>
            <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
            <div style="width: 1150px; float:none; background-color: #ffffff;">
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" SelectMethod="ProjectActivaSelectAll" 
                    UpdateMethod="ProjectActivaUpdate" InsertMethod="ProjectActivaInsert" DeleteMethod="ProjectActivaDelete">
                    <SelectParameters>
	                    <asp:ControlParameter ControlID="DropDownListProjects" Name="filterProject" PropertyName="SelectedValue" Type="String" 	/>
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="PROJECTACTIVAID" Type="Int32" />
                        <asp:Parameter Name="ACTIVANUMMER" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="PROJECTID" Type="Int32" />
                        <asp:Parameter Name="ACTIVANUMMER" Type="String" />
                    </InsertParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="PROJECTACTIVAID" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
                <asp:Label ID="lblRecordCount" runat="server" Text="&nbsp;"></asp:Label>
                <asp:GridView ID="GridView1" runat="server" DataSourceID="ObjectDataSource1" 
                    AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    Font-Names="Arial" Font-Size="12px" DataKeyNames="PROJECTACTIVAID"  
                    AllowPaging="True" 
                    onrowediting="GridView1_RowEditing" ondatabound="GridView1_DataBound">
                    <Columns>
                        <asp:CommandField ButtonType="Button" CancelText="Annuleren" 
                            DeleteText="Verwijderen" EditText="Wijzigen" InsertText="Toevoegen" 
                            NewText="Nieuw" SelectText="Selecteren" ShowDeleteButton="True" 
                            ShowEditButton="True" UpdateText="Opslaan" />

                        <asp:BoundField DataField="PROJECTACTIVAID" HeaderText="PROJECTACTIVAID" ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="100px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="PROJECTNAAM" HeaderText="PROJECTNAAM" ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="150px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="ACTIVANUMMER" HeaderText="ACTIVANUMMER" ReadOnly="False">
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