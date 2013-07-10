using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
    public partial class mappingK : System.Web.UI.Page
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
                loadNewProjects();
            }
        }

        private void loadNewProjects()
        {
            DataSet dsData = new DataSet();
            string sqlSelect = " SELECT PROJECTID FROM PROJECTS WHERE PROJECTID NOT IN (SELECT DISTINCT PROJECTID FROM ODS_PROJECT_KOPELLEN) ";
            string sqlInsert = " INSERT INTO ODS_PROJECT_KOPELLEN (PROJECTID, KOPPELEN) VALUES ([PROJECTID], 0) ";

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
                    myOracleCommand.CommandText = sqlInsert.Replace("[PROJECTID]", drData["PROJECTID"].ToString());
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

            GridView1.DataBind();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Session["edited_row_index"] = e.NewEditIndex.ToString();
            Session["confirmation_message_shown"] = "0";
            Session["old_value_koppelen"] = GridView1.Rows[e.NewEditIndex].Cells[3].Text.Trim();
            Session["new_value_koppelen"] = "";
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Session["edited_row_index"] = e.RowIndex.ToString();

            if (e.NewValues[0] != null)
            {
                Session["new_value_koppelen"] = e.NewValues[0].ToString();
            }
        }

        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            Session["edited_row_index"] = "";
            Session["confirmation_message_shown"] = "0";
            Session["old_value_koppelen"] = "";
            Session["new_value_koppelen"] = "";

            ShowUserInfo("clear", "");
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