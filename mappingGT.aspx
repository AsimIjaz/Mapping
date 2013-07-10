<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mappingGT.aspx.cs" Inherits="mapping.mappingGT" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>CLOXXS - GROOTBOEKREKENINGEN EN TAAKNUMMERS</title>
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
                Text="GROOTBOEKREKENINGEN EN TAAKNUMMERS" Font-Bold="True" Font-Names="Arial" 
                Font-Size="12px"></asp:Label>
            <div style="height: 5px; width: 1150px; background-color: #ffffff;"></div>
            <div style="height: 3px; width: 1150px; background-color: #000000;"></div>
            <div style="width: 1150px; float:none; background-color: #ffffff;">
                <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" 
                    SelectMethod="ProjectenSelectByProgramID">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownListPrograms" DefaultValue="" 
                            Name="ProgramID" PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" 
                    SelectMethod="GrootboekrekeningenSelectByProjectID" >
                    <SelectParameters>
                        <asp:ControlParameter ControlID="DropDownListProjects" DefaultValue="" 
                            Name="ProjectID" PropertyName="SelectedValue" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" SelectMethod="ProgrammasSelectAll">
                </asp:ObjectDataSource>

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
                    DataValueField="ProjectID" Width="250px" Font-Names="Arial" 
                    Font-Size="12px">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblSelectSheetType" runat="server" Text="Selecteer sheet type" Width="175px"></asp:Label>
                <asp:DropDownList ID="DropDownListSheetType" runat="server" AutoPostBack="True" 
                    Width="250px" Font-Names="Arial" Font-Size="12px">
                    <asp:ListItem Value="BU">Budgetsheets</asp:ListItem>
                    <asp:ListItem Value="FI">Financieringssheets</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblSortType" runat="server" Text="Sorteer op" Width="175px"></asp:Label>
                <asp:DropDownList
                    ID="DropDownListSortType" runat="server" AutoPostBack="True" Width="250px" 
                    Font-Names="Arial" Font-Size="12px">
                    <asp:ListItem Value="KoppelingID">KoppelingID</asp:ListItem>
                    <asp:ListItem Value="Code">Code</asp:ListItem>
                    <asp:ListItem Value="Naam">Naam</asp:ListItem>
                    <asp:ListItem Value="Grootboekrekening">Grootboekrekening</asp:ListItem>
                    <asp:ListItem Value="Taaknummer">Taaknummer</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblSortDirection" runat="server" Text="Sorteerrichting" Width="175px"></asp:Label>
                <asp:DropDownList
                    ID="DropDownListSortDirection" runat="server" AutoPostBack="True" 
                    Width="250px" Font-Names="Arial" Font-Size="12px">
                    <asp:ListItem Value="ASC">Oplopend</asp:ListItem>
                    <asp:ListItem Value="DESC">Aflopend</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblVernieuwen" runat="server" Text=" " Width="175px"></asp:Label>
                <asp:Button ID="btnRefresh" runat="server" Text="Vernieuwen" width="250px" 
                    Font-Names="Arial" Font-Size="12px" onclick="btnRefresh_Click"/>
                <br />
            </div>
            <div style="height: 3px; width: 1150px; background-color: #000000;">
             <br />
                <asp:Label ID="lblErrorMsg1" runat="server" Text=""></asp:Label>
            </div>
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
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
                    TypeName="mapping.BusinessObjectDATA" SelectMethod="KoppelingenSelectAll" 
                    UpdateMethod="KoppelingUpdate">
                    <SelectParameters>
	                    <asp:ControlParameter ControlID="DropDownListProjects" 		    Name="filterProject" 		PropertyName="SelectedValue" Type="String" 	/>
	                    <asp:ControlParameter ControlID="DropDownListSheetType" 	    Name="filterSheetType" 		PropertyName="SelectedValue" Type="String"	/>
	                    <asp:ControlParameter ControlID="DropDownListSortType" 		    Name="filterSortType" 		PropertyName="SelectedValue" Type="String"	/>
	                    <asp:ControlParameter ControlID="DropDownListSortDirection" 	Name="filterSortDirection" 	PropertyName="SelectedValue" Type="String"	/>
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="KOPPELINGID" Type="Int32" />
                        <asp:Parameter Name="GROOTBOEKREKENING" Type="String" />
                        <asp:Parameter Name="TAAKNUMMER" Type="String" />
                    </UpdateParameters>
                </asp:ObjectDataSource>

                <asp:Label ID="lblRecordCount" runat="server" Text="&nbsp;"></asp:Label>

                <asp:GridView ID="GridView1" runat="server" DataSourceID="ObjectDataSource1" 
                    AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    Font-Names="Arial" Font-Size="12px" DataKeyNames="KOPPELINGID" 
                    onrowediting="GridView1_RowEditing" 
                    onrowupdating="GridView1_RowUpdating" onrowupdated="GridView1_RowUpdated" 
                    AllowPaging="True" onrowcreated="GridView1_RowCreated" 
                    onprerender="GridView1_PreRender" ondatabound="GridView1_DataBound">
                    <Columns>
                        <asp:CommandField CancelText="Annuleren" EditText="Wijzig" 
                            ShowEditButton="True" UpdateText="Opslaan" ButtonType="Button">
                        <ControlStyle Font-Names="Arial" Font-Size="12px" Width="75px" />
                        <FooterStyle Wrap="False" />
                        <HeaderStyle Wrap="False" Width="175px" />
                        <ItemStyle Wrap="False" Font-Bold="True" Font-Names="Arial" 
                            Font-Size="12px" />
                        </asp:CommandField>

                        <asp:BoundField DataField="KOPPELINGID" HeaderText="KOPPELING ID" 
                            ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="100px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SHEETTYPE" HeaderText="SHEET TYPE" ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="125px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="CODE" HeaderText="CODE" ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="75px" Wrap="True" />
                        </asp:BoundField>

                        <asp:BoundField DataField="NAAM" HeaderText="NAAM" ReadOnly="True">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="250px" Wrap="True" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="GROOTBOEKREKENING">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlGROOTBOEKREKENING" AutoPostBack="true"
                                DataTextField="GROOTBOEKREKENING" DataValueField="GROOTBOEKREKENING"
                                DataSourceID="ObjectDataSource3" runat="server" AppendDataBoundItems="true"
                                SelectedValue='<%# Bind("GROOTBOEKREKENING") %>' 
                                onselectedindexchanged="ddlGROOTBOEKREKENING_SelectedIndexChanged" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("GROOTBOEKREKENING") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="200px" Wrap="True" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="TAAKNUMMER">
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlTAAKNUMMER" AutoPostBack="True" runat="server" 
                                DataTextField="TAAKNUMMER" DataValueField="TAAKNUMMER" 
                                onselectedindexchanged="ddlTAAKNUMMER_SelectedIndexChanged" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("TAAKNUMMER") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="200px" Wrap="True" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="AMOUNTSPRESENT" 
                            ReadOnly="True" InsertVisible="False" ShowHeader="False">
                        <ControlStyle Width="0px" />
                        <FooterStyle Width="0px" />
                        <HeaderStyle Width="0px" />
                        <ItemStyle Width="0px" ForeColor="White" />
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