using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Devart.Data;
using Devart.Data.Oracle;
using mapping;

namespace mapping
{
    public partial class mappingA : System.Web.UI.Page
    {
        String connectionString = ConfigurationManager.ConnectionStrings["cloxxsDB"].ConnectionString;
        int numberOfRows = 0;
        string selectedActivaNummer = "";
        string selectedProjectID = "";

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
                loadNewProjects("activa");
            }
        }

        private void loadNewProjects(string type)
        {
            DataSet dsData = new DataSet();
            string sqlSelect = "SELECT PROJECTS.PROJECTID FROM PROJECTS WHERE PROJECTID NOT IN (SELECT DISTINCT PROJECTID FROM ODS_PROJECT_ACTIVA) ";
            string sqlInsert = "INSERT INTO ODS_PROJECT_ACTIVA (PROJECTID, ACTIVANUMMER) VALUES ([PROJECTID], '') ";

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

        private void ShowUserInfo(string action, string contentText)
        {
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (textboxNewActivaNummer.Text.Trim().Length > 0)
            {
                string projectid = DropDownListProjects.SelectedValue;
                string newActivaNummer = textboxNewActivaNummer.Text.Trim();

                DataSet dsData = new DataSet();
                String insertCommandNew = "";

                OracleConnection myOracleConnection = new OracleConnection(connectionString);
                OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

                insertCommandNew = " INSERT INTO ODS_PROJECT_ACTIVA (PROJECTID, ACTIVANUMMER) VALUES (" + projectid + ",'" + newActivaNummer + "') ";

                myOracleConnection.Open();
                myOracleCommand = myOracleConnection.CreateCommand();
                myOracleCommand.CommandText = insertCommandNew;
                numberOfRows = myOracleCommand.ExecuteNonQuery();

                myOracleCommand.Dispose();
                myOracleCommand = null;

                myOracleConnection.Close();
                myOracleConnection.Dispose();
                myOracleConnection = null;

                // REFRESH
                textboxNewActivaNummer.Text = "";
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            selectedActivaNummer = GridView1.Rows[e.NewEditIndex].Cells[3].Text.Trim();
            selectedProjectID = GridView1.Rows[e.NewEditIndex].Cells[1].Text.Trim();
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            lblRecordCount.Text = "Aantal regels: " + GridView1.Rows.Count.ToString();
        }

    }
}