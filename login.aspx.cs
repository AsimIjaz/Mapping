using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using Devart.Data.Oracle;

namespace mapping
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            labelUserMessage.Text = "";
        }

        protected void buttonCheckCredentials_Click(object sender, EventArgs e)
        {
            string checkUser = "";

            checkUser = checkUserValid(textboxUsername.Text.Trim(), textboxPassword.Text.Trim());

            //if (checkUser == "OK")
            if (checkUser.Length > 0)
            {
                if (!checkUser.Contains("Log-in fout"))
                {
                    // put in session
                    Session["cloxxsLoginValid"] = "";
                    Session["cloxxsLoginValid"] = "1";

                    Session["cloxxs_user_id"] = "";
                    Session["cloxxs_user_id"] = checkUser;

                    // redirect to report page
                    Response.Redirect("mappinggt.aspx");
                }
            }

            if (checkUser.Length > 0)
            {
                if (checkUser.Contains("Log-in fout"))
                {
                    labelUserMessage.Text = checkUser;
                }
            }
        }

        private string checkUserValid(string login, string password)
        {
            string result = "";
            string inputLogin = "";
            string inputPassword = "";
            string loopLogin = "";
            string loopPassword = "";
            string userID = "";
            string isAdmin = "0";

            char[] checkArray = { ' ' };

            inputLogin = login.TrimEnd(checkArray).TrimStart(checkArray);
            inputPassword = password.TrimEnd(checkArray).TrimStart(checkArray);

            // get details of all user
            DataSet dsUsers = GetUserDetails();

            if (dsUsers.Tables.Count > 0)
            {
                if (dsUsers.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drUser in dsUsers.Tables[0].Rows)
                    {
                        if (drUser["Login"] != null && drUser["Password"] != null)
                        {
                            loopLogin = drUser["Login"].ToString().TrimEnd(checkArray).TrimStart(checkArray);
                            loopPassword = drUser["Password"].ToString().TrimEnd(checkArray).TrimStart(checkArray);

                            if (loopLogin == inputLogin && loopPassword == inputPassword)
                            {
                                //return "OK";
                                userID = drUser["userid"].ToString().TrimEnd(checkArray).TrimStart(checkArray);
                                result = userID;
                                isAdmin = drUser["isadmin"].ToString().TrimEnd(checkArray).TrimStart(checkArray);
                                // put isadmin in session
                                Session["cloxxs_user_isadmin"] = isAdmin;
                                // check if is admin
                                if (isAdmin != "1")
                                {
                                    result = "Log-in fout: Gebruikers is geen Administrator. Toegang niet toegestaan.";
                                }
                                return result;
                            }
                        }
                    }
                    result = "Log-in fout: De gegevens zijn niet correct.";
                }
                else
                {
                    result = "Log-in fout: Gebruikerstabel is leeg.";
                }
            }
            else
            {
                result = "Log-in fout: Gebruikerstabel niet gevonden.";
            }



            return result;
        }

        private DataSet GetUserDetails()
        {
            string strSql = " SELECT * FROM vw_GetUsers ";

            DataSet test = new DataSet();

            return GetDataSet(strSql);
        }

        private DataSet GetDataSet(string sqlQuery)
        {
            string connString = "";
            DataSet dsData = new DataSet();
            string dbType = "";

            connString = ConfigurationManager.ConnectionStrings["cloxxsDB"].ConnectionString;
            dbType = ConfigurationManager.AppSettings["databaseType"].ToString();

            switch (dbType)
            {
                case "MSS05":
                case "MSS08":
                    SqlConnection sqlconn = new SqlConnection(connString);
                    SqlDataAdapter sqlda = new SqlDataAdapter(sqlQuery, sqlconn);
                    dsData = new DataSet();
                    sqlda.Fill(dsData);
                    sqlda = null;
                    sqlconn.Close();
                    sqlconn = null;
                    break;
                case "ORA10":
                case "ORA11":
                    OracleConnection oraconn = new OracleConnection(connString);
                    OracleDataAdapter orada = new OracleDataAdapter(sqlQuery, oraconn);
                    dsData = new DataSet();
                    orada.Fill(dsData);
                    orada = null;
                    oraconn.Close();
                    oraconn = null;
                    break;
            }

            return dsData;
        }
    }
}