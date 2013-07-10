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
    public partial class checklistBudgets : System.Web.UI.Page
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
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            GridView1.DataBind();
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
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            lblRecordCount.Text = "Aantal regels: " + GridView1.Rows.Count.ToString();
        }

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            lblRecordCount.Text = "";
        }
    }
}