using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mapping;
using mapping.Account;

using System.Xml;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Devart.Data;
using Devart.Data.Oracle;

namespace mapping
{
    public partial class mappingGT : System.Web.UI.Page
    {
        String connectionString = ConfigurationManager.ConnectionStrings["cloxxsDB"].ConnectionString;
        int numberOfRows = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            // check if login is valid
            if (Session["cloxxsLoginValid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                if (Session["cloxxsLoginValid"].ToString().Length == 0)
                {
                    Response.Redirect("login.aspx");
                }
                if (Session["cloxxsLoginValid"].ToString().Trim() != "1")
                {
                    Response.Redirect("login.aspx");
                }
            }

            if (!IsPostBack)
            {
                loadSheets();
            }
        }

        private void loadSheets()
        {
            loadNewSheets("budgets");
            loadNewSheets("funds");
        }

        private void loadNewSheets(string type)
        {
            DataSet dsData = new DataSet();
            string sqlSelectBudgets = "SELECT BUDGETID AS SELECTED_ID FROM BUDGETS WHERE BUDGETID NOT IN (SELECT SHEETID FROM KOPPELINGEN WHERE SHEETTYPE = 'BU') ";
            string sqlSelectFunds = "SELECT FUNDID AS SELECTED_ID FROM FUNDS WHERE FUNDID NOT IN (SELECT SHEETID FROM KOPPELINGEN WHERE SHEETTYPE = 'FI') ";
            string sqlInsertBudget = "INSERT INTO KOPPELINGEN (SHEETID, SHEETTYPE, GROOTBOEKREKENING, TAAKNUMMER) VALUES ([SHEETID], 'BU', '', '') ";
            string sqlInsertFund = "INSERT INTO KOPPELINGEN (SHEETID, SHEETTYPE, GROOTBOEKREKENING, TAAKNUMMER) VALUES ([SHEETID], 'FI', '', '') ";
            string sqlSelect = "";
            string sqlInsert = "";

            switch (type)
            {
                case "budgets":
                    sqlSelect = sqlSelectBudgets;
                    sqlInsert = sqlInsertBudget;
                    break;
                case "funds":
                    sqlSelect = sqlSelectFunds;
                    sqlInsert = sqlInsertFund;
                    break;
            }

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlSelect;

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "CloxxsData");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;

            if (numberOfRows > 0)
            {
                myOracleCommand = myOracleConnection.CreateCommand();

                foreach (DataRow drData in dsData.Tables[0].Rows)
                {
                    myOracleCommand.CommandText = sqlInsert.Replace("[SHEETID]", drData["SELECTED_ID"].ToString());
                    numberOfRows = myOracleCommand.ExecuteNonQuery();
                }

                myOracleCommand.Dispose();
                myOracleCommand = null;
            }

            myOracleConnection.Dispose();
            myOracleConnection = null;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            lblRecordCount.Text = "";

            loadSheets();
            GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Session["edited_row_index"] = e.NewEditIndex.ToString();
            Session["accounts_present"] = GridView1.Rows[e.NewEditIndex].Cells[7].Text.Trim();

            string koppelingID = GridView1.Rows[Convert.ToInt32(Session["edited_row_index"].ToString())].Cells[1].Text.Trim();
            if (koppelingID.Length > 0)
            {
                DataSet dsCurrentValues = getDataCurrentValues(koppelingID);
                if (dsCurrentValues.Tables.Count > 0)
                {
                    if (dsCurrentValues.Tables[0].Rows.Count > 0)
                    {
                        if (dsCurrentValues.Tables[0].Rows[0]["GROOTBOEKREKENING"] != null)
                        {
                            Session["old_value_grootboeknummer"] = dsCurrentValues.Tables[0].Rows[0]["GROOTBOEKREKENING"].ToString().Trim();
                            Session["old_value_taaknummer"] = dsCurrentValues.Tables[0].Rows[0]["TAAKNUMMER"].ToString().Trim();

                            Session["new_value_grootboeknummer"] = Session["old_value_grootboeknummer"];
                            Session["new_value_taaknummer"] = Session["old_value_taaknummer"];
                        }
                    }
                }
            }
            else
            {
                Session["old_value_grootboeknummer"] = "";
                Session["old_value_taaknummer"] = "";
                Session["new_value_grootboeknummer"] = "";
                Session["new_value_taaknummer"] = "";
            }

            Session["confirmation_message_shown"] = "0";
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Session["edited_row_index"] = e.RowIndex.ToString();
            Session["accounts_present"] = GridView1.Rows[e.RowIndex].Cells[7].Text.Trim();

            e.NewValues.Clear();

            if (Session["new_value_grootboeknummer"] != null)
            {
                e.NewValues.Add("GROOTBOEKREKENING", Session["new_value_grootboeknummer"].ToString());
            }
            else
            {
                e.NewValues.Add("GROOTBOEKREKENING", null);
            }

            if (Session["new_value_taaknummer"] != null)
            {
                e.NewValues.Add("TAAKNUMMER", Session["new_value_taaknummer"].ToString());
            }
            else
            {
                e.NewValues.Add("TAAKNUMMER", null);
            }

            if ((Session["old_value_grootboeknummer"] != Session["new_value_grootboeknummer"]) || (Session["old_value_taaknummer"] != Session["new_value_taaknummer"]))
            {
                if (Session["accounts_present"].ToString() == "1")
                {
                    if (Session["confirmation_message_shown"].ToString() == "0")
                    {
                        e.Cancel = true;
                        string message = "Er zijn prognose- en/of meerwerkbedragen ingevuld op onderliggende contracten. \r\nAls u doorgaat dan worden deze bedragen ook verplaatst naar de nieuwe bestemming. \r\nWeet u zeker dat u de wijziging wilt doorvoeren ?";
                        ShowUserInfo("amounts_present_confirm", message);
                        Session["confirmation_message_shown"] = "1";
                    }
                }
            }
            else
            {
                e.Cancel = true;
                GridView1.EditIndex = -1;
            }
        }

        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            Session["edited_row_index"] = "";
            Session["accounts_present"] = "";
            Session["old_value_grootboeknummer"] = "";
            Session["old_value_taaknummer"] = "";
            Session["new_value_grootboeknummer"] = "";
            Session["new_value_taaknummer"] = "";
            Session["confirmation_message_shown"] = "0";
            Session["selectedGrootboek"] = "";
            Session["selectedTaaknummer"] = "";
            Session["selectedRowIndex"] = "";

            ShowUserInfo("clear", "");
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells.Count > 5)
                {
                    DropDownList ddlGB = (DropDownList)e.Row.Cells[5].FindControl("ddlGROOTBOEKREKENING");
                    if (ddlGB != null)
                    {
                        Session["selectedRowIndex"] = e.Row.RowIndex.ToString();
                    }
                }
            }
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {
            string selectedGrootboek = "";

            if (Session["selectedRowIndex"] != null)
            {
                if (Session["selectedRowIndex"].ToString().Trim().Length > 0)
                {
                    selectedGrootboek = GridView1.Rows.Count.ToString();
                    DropDownList ddlGB = (DropDownList)GridView1.Rows[Convert.ToInt32(Session["selectedRowIndex"].ToString())].Cells[5].FindControl("ddlGROOTBOEKREKENING");
                    if (ddlGB != null)
                    {
                        selectedGrootboek = ddlGB.SelectedValue.ToString();
                        if (selectedGrootboek.Trim().Length > 0)
                        {
                            DropDownList ddlTN = (DropDownList)GridView1.Rows[Convert.ToInt32(Session["selectedRowIndex"].ToString())].Cells[6].FindControl("ddlTAAKNUMMER");
                            if (ddlTN != null)
                            {
                                ddlTN.Items.Clear();
                                ddlTN.Items.Add(new ListItem("LEEG", "0"));

                                DataSet dsData = getDataTaaknummersSelectByGrootboekrekening(selectedGrootboek);
                                if (dsData.Tables.Count > 0)
                                {
                                    if (dsData.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow drData in dsData.Tables[0].Rows)
                                        {
                                            if (drData["TAAKNUMMER"] != null)
                                            {
                                                ddlTN.Items.Add(new ListItem(drData["TAAKNUMMER"].ToString().Trim(), drData["TAAKNUMMER"].ToString().Trim()));
                                            }
                                        }
                                    }
                                }
                                dsData.Dispose();
                                dsData = null;

                                if (Session["selectedTaaknummer"] != null)
                                {
                                    if (Session["selectedTaaknummer"].ToString().Trim().Length > 0)
                                    {
                                        ddlTN.SelectedValue = Session["selectedTaaknummer"].ToString().Trim();
                                    }
                                }

                                if (Session["old_value_taaknummer"] != null)
                                {
                                    //if (Session["old_value_taaknummer"].ToString().Trim().Length > 0)
                                    //{
                                    //    ddlTN.SelectedValue = Session["old_value_taaknummer"].ToString().Trim();
                                    //}

                                    try
                                    {
                                        ddlTN.SelectedValue = Session["old_value_taaknummer"].ToString().Trim();
                                    }
                                    catch (Exception exc)
                                    {
                                        lblErrorMsg1.ForeColor = System.Drawing.Color.Red;
                                        Session["old_value_taaknummer"] = "";
                                        lblErrorMsg1.Text = "Fout bij het ophalen taaknummers voor (GROOTBOEKNUMMER:" + ddlGB.SelectedValue.ToString() + ")";
 

                                    }

                                }

                                if (Session["new_value_taaknummer"] != null)
                                {
                                    if (Session["new_value_taaknummer"].ToString().Trim().Length > 0)
                                    {
                                        ddlTN.SelectedValue = Session["new_value_taaknummer"].ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void ddlGROOTBOEKREKENING_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            getCurrentValues();

            // clean taaknummer
            Session["new_value_taaknummer"] = "";
            Session["selectedTaaknummer"] = "";
        }

        protected void ddlTAAKNUMMER_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCurrentValues();
        }

        private void getCurrentValues()
        {
            DropDownList ddlGB = (DropDownList)GridView1.Rows[Convert.ToInt32(Session["selectedRowIndex"].ToString())].Cells[5].FindControl("ddlGROOTBOEKREKENING");
            if (ddlGB != null)
            {
                Session["new_value_grootboeknummer"] = ddlGB.SelectedValue.ToString();
            }

            DropDownList ddlTK = (DropDownList)GridView1.Rows[Convert.ToInt32(Session["selectedRowIndex"].ToString())].Cells[6].FindControl("ddlTAAKNUMMER");
            if (ddlTK != null)
            {
                Session["new_value_taaknummer"] = ddlTK.SelectedValue.ToString();
            }
        }

        private DataSet getDataTaaknummersSelectByGrootboekrekening(string filterGrootboekrekening)
        {
            DataSet dsData = new DataSet();
            StringBuilder sbSelectQuery = new StringBuilder();

            if (filterGrootboekrekening.Trim().Length > 0)
            {
                sbSelectQuery.AppendLine(" SELECT TAAK AS TAAKNUMMER FROM stg_ods_project_fcl_lijst WHERE FCL = " + filterGrootboekrekening + " ");
            }
            else
            {
                sbSelectQuery.AppendLine(" SELECT '' AS TAAKNUMMER FROM DUAL ");
            }

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sbSelectQuery.ToString();

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Projects");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }

        private DataSet getDataCurrentValues(string koppelingID)
        {
            DataSet dsData = new DataSet();

            StringBuilder sbSelectQuery = new StringBuilder();
            sbSelectQuery.AppendLine(" SELECT GROOTBOEKREKENING, TAAKNUMMER FROM KOPPELINGEN WHERE KOPPELINGID = " + koppelingID + " ");

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sbSelectQuery.ToString();

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Projects");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }

        private void ShowUserInfo(string action, string contentText)
        {
            Label lblSessionInfo;
            panelUserInteration.Controls.Clear();

            switch (action)
            {
                case "clear":
                    break;
                case "amounts_present_confirm":
                    lblSessionInfo = new Label();
                    lblSessionInfo.ID = "lblSessionInfo";
                    lblSessionInfo.Text = contentText;
                    lblSessionInfo.ForeColor = System.Drawing.Color.Red;
                    panelUserInteration.Controls.Add(lblSessionInfo);
                    break;
                case "[ORA-00001: unique constraint]":
                    lblSessionInfo = new Label();
                    lblSessionInfo.ID = "lblSessionInfo";
                    lblSessionInfo.Text = contentText;
                    lblSessionInfo.ForeColor = System.Drawing.Color.Red;
                    panelUserInteration.Controls.Add(lblSessionInfo);
                    break;
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            lblRecordCount.Text = "Aantal regels: " + GridView1.Rows.Count.ToString();
        }
    }
}