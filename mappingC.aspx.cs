using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Devart.Common;
using Devart.Data;
using Devart.Data.Oracle;
using System.Configuration;
using System.Globalization;
using System.Text;

// ALL FIELDS
//Project_ID
//Project_naam
//K_Activum
//K_Omschrijving_activum
//K_Verplichting
//K_Omschrijving_verplichting
//K_Crediteur
//K_Omschrijving_crediteur
//K_Opdracht
//K_Zachte_verplichting
//K_Opdrachtcode
//K_Gbknr
//K_Omschrijving_Grootboeknummer
//K_Taak
//K_Omschrijving_taak
//K_Kstnsrt
//K_Omschrijving_kostensoort
//K_Afgehandeld
//K_Saldo_2009
//K_Saldo_2010
//K_Saldo_2011
//K_Saldo_2012
//K_Saldo_2013
//K_Saldo_2014
//K_Saldo_2015
//K_Totaal_Verplichting
//K_Totaal_Afboeking
//K_Openstaand
//Verplichting
//Omschrijving_verplichting
//Crediteur
//Omschrijving_crediteur
//Gbknr
//Omschrijving_Grootboeknummer
//Taaknummer
//Omschrijving_taak
//Opdracht_afgerond
//Saldo_2009
//Saldo_2010
//Saldo_2011
//Saldo_2012
//Saldo_2013
//Saldo_2014
//Saldo_2015
//Saldo_Totaal
//Inlezen

namespace mapping
{
    public partial class mappingC : System.Web.UI.Page
    {
        bool flagDebug = false;

        public static string LogUrl = "";

        public void showError()
        {
            TextBoxLog.Visible = true;
            TextBoxLog.ForeColor = System.Drawing.Color.Red;
            TextBoxLog.Font.Bold = true;

            string FileName = null;
            StreamReader fp = null;
            FileName = "excelsavemap/LogConversieVerplichtingen.txt";

            try
            {
                fp = File.OpenText(Server.MapPath(FileName));
                TextBoxLog.Text = fp.ReadToEnd();
                fp.Close();
            }
            catch (Exception err)
            {
                TextBoxLog.Text = "Log ophalen mislukt " + err.ToString();
            }
            finally
            {

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            LogUrl = MapPath("~/excelsavemap/LogConversieVerplichtingen.txt");

     

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

            AsyncFileUploadVerplichtingen.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedComplete);
            AsyncFileUploadVerplichtingen.UploadedFileError += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedFileError);
        }

        void AsyncFileUpload1_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "", false);
            string savePath = MapPath("~/excelsavemap/" + Path.GetFileName(e.FileName));
            AsyncFileUploadVerplichtingen.SaveAs(savePath);
            GetExcelFileExtension(savePath);
        }

        void AsyncFileUpload1_UploadedFileError(object sender, AsyncFileUploadEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "", false);
        }



        protected void DropDownListProjects_DataBound(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        private void GetExcelFileExtension(string path)
        {
            string extension = "";

            if (path.EndsWith(".xlsx"))
            {
                extension = "xlsx";
            }
            if (path.EndsWith(".xls"))
            {
                extension = "xls";
            }

            Session["UPLOAD_FILE_EXTENSION"] = extension;
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            lblRecordCount.Text = "Aantal regels: " + GridView1.Rows.Count.ToString();
        }

        protected void ButtonLog_Click(object sender, EventArgs e)
        {
            showError();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            long processedRows = 0;


            File.WriteAllText(MapPath("~/excelsavemap/LogConversieVerplichtingen.txt"), String.Empty); 

            DataTable data = new DataTable();
            DataTable data2 = new DataTable();
            DataTable data3 = new DataTable();

            string fileExtension = Session["UPLOAD_FILE_EXTENSION"].ToString();
            String connectionString = ConfigurationManager.ConnectionStrings["cloxxsDB"].ConnectionString;
            string connectionStringExcel = (@"Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + MapPath("~") + "\\excelsavemap\\CLOXXS_CONVERSIE." + fileExtension + ";" + "Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"");

            try
            {
                // Clear the DataGridView and the connection string TextBox
                data.Dispose();
                data = new DataTable();
                data2.Dispose();
                data2 = new DataTable();
                data3.Dispose();
                data3 = new DataTable(); 

                using (OleDbConnection connection = new OleDbConnection(connectionStringExcel))
                {
                    connection.Open();
                    #region stap1
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Conversie_Verplichtingen$]", connection))
                    {
                        adapter.Fill(data);
                        InlezenExcelLabel.Text = data.Rows.Count.ToString();

                        DataSet ds = new DataSet();
                        ds.Tables.Add(data);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            try
                            {
                                // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                                OracleConnection myConn = new OracleConnection(connectionString);
                                myConn.Open();

                                // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                                String DeleteQuery = "DELETE FROM CONVERSIE_VERPLICHTINGEN WHERE PROJECT_ID = :p1 ";

                                OracleCommand myCommand = new OracleCommand(DeleteQuery, myConn);
                                myCommand.ParameterCheck = true;
                                myCommand.Prepare();
                                myCommand.Parameters[0].Value = dr["PROJECT_ID"];

                                myCommand.ExecuteNonQuery();

                                myConn.Close();


                            }
                            catch (Exception excDeleteOldId)
                            {

                                connection.Close();
                                connection.Dispose();
                                string LogUrl = MapPath("~/excelsavemap/LogConversieVerplichtingen.txt");
                                StreamWriter SW;
                                SW = File.CreateText(LogUrl);
                                SW.WriteLine("[Exception tijdens het verwijderen van de null regels (stap1)]");
                                SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                                SW.WriteLine(excDeleteOldId.Source + ", " + excDeleteOldId.Message + ". " + excDeleteOldId.StackTrace);
                                SW.Close();

                            }
                        }
                    }
                    #endregion stap1

                    #region stap2
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Conversie_Verplichtingen$]", connection))
                    {
                        string LogUrlDebug = MapPath("~/excelsavemap/LogConversieVerplichtingen.txt");

                        StreamWriter SW;
                        SW = File.CreateText(LogUrlDebug);

                        adapter.Fill(data2);

                        OracleConnection myConn1 = new OracleConnection();
                        OracleCommand myCommand = new OracleCommand();

                        // Create new DataSet
                        DataSet ds = new DataSet();
                        ds.Tables.Add(data2);

                        // number of rows to go
                        if (flagDebug) SW.WriteLine("[rows to go:" + ds.Tables[0].Rows.Count.ToString() + "]");

                        // processed rows


                        // Loop through the rows of the only table in the DataSet
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            try
                            {
                                //teller/counter +1 for rows
                                processedRows = processedRows + 1;

                                // start processing
                                if (flagDebug) SW.WriteLine("[start: processing row:" + processedRows.ToString() + "]");

                                // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                                myConn1 = new OracleConnection(connectionString);
                                myConn1.Open();

                                String insertQuery = "";

                                //Project_ID,Project_naam,K_Activum,K_Omschrijving_activum,K_Verplichting,K_Omschrijving_verplichting,K_Crediteur,K_Omschrijving_crediteur,K_Gbknr,K_Omschrijving_Grootboeknummer,K_Taak,K_Omschrijving_taak,K_Saldo_2000,K_Saldo_2001,K_Saldo_2002,K_Saldo_2003,K_Saldo_2004,K_Saldo_2005,K_Saldo_2006,K_Saldo_2007,K_Saldo_2008,K_Saldo_2009,K_Saldo_2010,K_Saldo_2011,K_Saldo_2012,K_Saldo_2013,K_Saldo_2014,K_Saldo_2015,K_Saldo_2016,K_Saldo_2017,K_Saldo_2018,K_Saldo_2019,K_Saldo_2020,K_Totaal_Verplichting,K_Totaal_Afboeking,K_Openstaand,Verplichting,Omschrijving_verplichting,Crediteur,Omschrijving_crediteur,Gbknr,Omschrijving_Grootboeknummer,Taaknummer,Omschrijving_taak,Saldo_2000,Saldo_2001,Saldo_2002,Saldo_2003,Saldo_2004,Saldo_2005,Saldo_2006,Saldo_2007,Saldo_2008,Saldo_2009,Saldo_2010,Saldo_2011,Saldo_2012,Saldo_2013,Saldo_2014,Saldo_2015,Saldo_2016,Saldo_2017,Saldo_2018,Saldo_2019,Saldo_2020,Saldo_Totaal,Inlezen

                                insertQuery += " INSERT INTO CONVERSIE_VERPLICHTINGEN( ";
                                //insertQuery += " Project_ID, Project_naam, K_Activum, K_Omschrijving_activum, K_Verplichting, K_Omschrijving_verplichting, K_Crediteur, K_Omschrijving_crediteur, K_Gbknr, K_Omschrijving_Grootboeknummer, K_Taak, K_Omschrijving_taak, K_Saldo_2000, K_Saldo_2001, K_Saldo_2002, K_Saldo_2003, K_Saldo_2004, K_Saldo_2005, K_Saldo_2006, K_Saldo_2007, K_Saldo_2008, K_Saldo_2009, K_Saldo_2010, K_Saldo_2011, K_Saldo_2012, K_Saldo_2013, K_Saldo_2014, K_Saldo_2015,K_Saldo_2016, K_Saldo_2017, K_Saldo_2018, K_Saldo_2019, K_Saldo_2020, K_Totaal_Verplichting, K_Totaal_Afboeking, K_Openstaand, Verplichting, Omschrijving_verplichting, Crediteur, Omschrijving_crediteur, Gbknr, Omschrijving_Grootboeknummer, Taaknummer, Omschrijving_taak, Opdracht_afgerond, Saldo_2001, Saldo_2002, Saldo_2003, Saldo_2004, Saldo_2005, Saldo_2006, Saldo_2007,Saldo_2008,Saldo_2009, Saldo_2010, Saldo_2011, Saldo_2012, Saldo_2013, Saldo_2014, Saldo_2015, Saldo_2016, Saldo_2017, Saldo_2018, Saldo_2019, Saldo_2020, Saldo_Totaal, Inlezen) ";
                                insertQuery += " Project_ID,Project_naam,K_Activum,K_Omschrijving_activum,K_Verplichting,K_Omschrijving_verplichting,K_Crediteur,K_Omschrijving_crediteur,K_Gbknr,K_Omschrijving_Grootboeknummer,K_Taak,K_Omschrijving_taak,K_Saldo_2000,K_Saldo_2001,K_Saldo_2002,K_Saldo_2003,K_Saldo_2004,K_Saldo_2005,K_Saldo_2006,K_Saldo_2007,K_Saldo_2008,K_Saldo_2009,K_Saldo_2010,K_Saldo_2011,K_Saldo_2012,K_Saldo_2013,K_Saldo_2014,K_Saldo_2015,K_Saldo_2016,K_Saldo_2017,K_Saldo_2018,K_Saldo_2019,K_Saldo_2020,K_Totaal_Verplichting,K_Totaal_Afboeking,K_Openstaand,Verplichting,Omschrijving_verplichting,Crediteur,Omschrijving_crediteur,Gbknr,Omschrijving_Grootboeknummer,Taaknummer,Omschrijving_taak,Saldo_2000,Saldo_2001,Saldo_2002,Saldo_2003,Saldo_2004,Saldo_2005,Saldo_2006,Saldo_2007,Saldo_2008,Saldo_2009,Saldo_2010,Saldo_2011,Saldo_2012,Saldo_2013,Saldo_2014,Saldo_2015,Saldo_2016,Saldo_2017,Saldo_2018,Saldo_2019,Saldo_2020,Saldo_Totaal,Inlezen) ";

                                insertQuery += " VALUES (:p1, :p2, :p3, :p4, :p5, :p6, :p7, :p8, :p9, :p10, :p11, :p12, :p13, :p14, :p15, :p16, :p17, :p18, :p19, :p20, :p21, :p22, :p23, :p24, :p25, :p26, :p27, :p28, :p29, :p30, :p31, :p32, :p33, :p34, :p35, :p36, :p37, :p38, :p39, :p40, :p41, :p42, :p43, :p44, :p45, :p46, :p47, :p48, :p49, :p50, :p51, :p52, :p53, :p54, :p55, :p56, :p57, :p58, :p59, :p60, :p61, :p62, :p63, :p64, :p65, :p66, :p67 ) ";

                                // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                                myCommand = new OracleCommand(insertQuery, myConn1);
                                myCommand.ParameterCheck = true;
                                myCommand.Prepare();

                                myCommand.Parameters[0].Value = dr["Project_ID"];
                                myCommand.Parameters[1].Value = dr["Project_naam"];
                                myCommand.Parameters[2].Value = dr["K_Activum"];
                                myCommand.Parameters[3].Value = dr["K_Omschrijving_activum"];
                                myCommand.Parameters[4].Value = dr["K_Verplichting"];
                                myCommand.Parameters[5].Value = dr["K_Omschrijving_verplichting"];
                                myCommand.Parameters[6].Value = dr["K_Crediteur"];
                                myCommand.Parameters[7].Value = dr["K_Omschrijving_crediteur"];
                                myCommand.Parameters[8].Value = dr["K_Gbknr"];
                                myCommand.Parameters[9].Value = dr["K_Omschrijving_Grootboeknummer"];
                                myCommand.Parameters[10].Value = dr["K_Taak"];

                                myCommand.Parameters[11].Value = dr["K_Omschrijving_taak"];
                                myCommand.Parameters[12].Value = dr["K_Saldo_2000"];
                                myCommand.Parameters[13].Value = dr["K_Saldo_2001"];
                                myCommand.Parameters[14].Value = dr["K_Saldo_2002"];
                                myCommand.Parameters[15].Value = dr["K_Saldo_2003"];
                                myCommand.Parameters[16].Value = dr["K_Saldo_2004"];
                                myCommand.Parameters[17].Value = dr["K_Saldo_2005"];
                                myCommand.Parameters[18].Value = dr["K_Saldo_2006"];
                                myCommand.Parameters[19].Value = dr["K_Saldo_2007"];
                                myCommand.Parameters[20].Value = dr["K_Saldo_2008"];

                                myCommand.Parameters[21].Value = dr["K_Saldo_2009"];
                                myCommand.Parameters[22].Value = dr["K_Saldo_2010"];
                                myCommand.Parameters[23].Value = dr["K_Saldo_2011"];
                                myCommand.Parameters[24].Value = dr["K_Saldo_2012"];
                                myCommand.Parameters[25].Value = dr["K_Saldo_2013"];
                                myCommand.Parameters[26].Value = dr["K_Saldo_2014"];
                                myCommand.Parameters[27].Value = dr["K_Saldo_2015"];
                                myCommand.Parameters[28].Value = dr["K_Saldo_2016"];
                                myCommand.Parameters[29].Value = dr["K_Saldo_2017"];
                                myCommand.Parameters[30].Value = dr["K_Saldo_2018"];

                                myCommand.Parameters[31].Value = dr["K_Saldo_2019"];
                                myCommand.Parameters[32].Value = dr["K_Saldo_2020"];
                                myCommand.Parameters[33].Value = dr["K_Totaal_Verplichting"];
                                myCommand.Parameters[34].Value = dr["K_Totaal_Afboeking"];
                                myCommand.Parameters[35].Value = dr["K_Openstaand"];
                                myCommand.Parameters[36].Value = dr["Verplichting"];
                                myCommand.Parameters[37].Value = dr["Omschrijving_verplichting"];
                                myCommand.Parameters[38].Value = dr["Crediteur"];
                                myCommand.Parameters[39].Value = dr["Omschrijving_crediteur"];
                                myCommand.Parameters[40].Value = dr["Gbknr"];

                                myCommand.Parameters[41].Value = dr["Omschrijving_Grootboeknummer"];
                                myCommand.Parameters[42].Value = dr["Taaknummer"];
                                myCommand.Parameters[43].Value = dr["Omschrijving_taak"];
                                myCommand.Parameters[44].Value = dr["Saldo_2000"];
                                myCommand.Parameters[45].Value = dr["Saldo_2001"];
                                myCommand.Parameters[46].Value = dr["Saldo_2002"];
                                myCommand.Parameters[47].Value = dr["Saldo_2003"];
                                myCommand.Parameters[48].Value = dr["Saldo_2004"];
                                myCommand.Parameters[49].Value = dr["Saldo_2005"];
                                myCommand.Parameters[50].Value = dr["Saldo_2006"];

                                myCommand.Parameters[51].Value = dr["Saldo_2007"];
                                myCommand.Parameters[52].Value = dr["Saldo_2008"];
                                myCommand.Parameters[53].Value = dr["Saldo_2009"];
                                myCommand.Parameters[54].Value = dr["Saldo_2010"];
                                myCommand.Parameters[55].Value = dr["Saldo_2011"];
                                myCommand.Parameters[56].Value = dr["Saldo_2012"];
                                myCommand.Parameters[57].Value = dr["Saldo_2013"];
                                myCommand.Parameters[58].Value = dr["Saldo_2014"];
                                myCommand.Parameters[59].Value = dr["Saldo_2015"];
                                myCommand.Parameters[60].Value = dr["Saldo_2016"];

                                myCommand.Parameters[61].Value = dr["Saldo_2017"];
                                myCommand.Parameters[62].Value = dr["Saldo_2018"];
                                myCommand.Parameters[63].Value = dr["Saldo_2019"];
                                myCommand.Parameters[64].Value = dr["Saldo_2020"];
                                myCommand.Parameters[65].Value = dr["Saldo_Totaal"];
                                myCommand.Parameters[66].Value = dr["Inlezen"];

                                myCommand.ExecuteNonQuery();

                                myCommand.Dispose();
                                myConn1.Close();
                                myConn1.Dispose();

                                // start processing

                                if (flagDebug) SW.WriteLine("[end  : processing row:" + processedRows.ToString() + "]");
                            }
                            catch (Exception exceptionInvoer)
                            {
                                myCommand.Dispose();
                                myConn1.Close();
                                myConn1.Dispose();

                                connection.Close();
                                connection.Dispose();


                                SW.WriteLine("[Exception tijdens het invoeren regels (stap2) ] ERROR REGEL(" + processedRows + ")");
                                SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                                SW.WriteLine(exceptionInvoer.Source + ", " + exceptionInvoer.Message + ". " + exceptionInvoer.StackTrace);


                            }
                            finally
                            {
                                myCommand.Dispose();
                                myConn1.Close();
                                myConn1.Dispose();

                            }
                        }
                        SW.Close();
                        SW.Dispose();

                    }
                    #endregion stap2

                    OracleConnection myConnDelete = new OracleConnection();
                    OracleCommand myCommandDelete = new OracleCommand();

                    #region stap3
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Conversie_Verplichtingen$]", connection))
                    {
                        try
                        {

                            // delete all null values
                            // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                            myConnDelete = new OracleConnection(connectionString);
                            myConnDelete.Open();

                            // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                            String DeleteNullQuery = " DELETE FROM CONVERSIE_VERPLICHTINGEN WHERE PROJECT_ID IS NULL ";

                             myCommandDelete = new OracleCommand(DeleteNullQuery, myConnDelete);

                            myCommandDelete.ExecuteNonQuery();
                            myCommandDelete.Dispose();
                            myConnDelete.Close();
                        }
                        catch (Exception excDelete2)
                        {
                            connection.Close();
                            connection.Dispose();
                            string LogUrl = MapPath("~/excelsavemap/LogConversieVerplichtingen.txt");
                            StreamWriter SW;
                            SW = File.CreateText(LogUrl);
                            SW.WriteLine("[Exception tijdens het verwijderen van de null regels (stap3)]");
                            SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                            SW.WriteLine(excDelete2.Source + ", " + excDelete2.Message + ". " + excDelete2.StackTrace);
                            SW.Close();
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();

                        }

                    }
                    #endregion stap3

                }


            }
            catch (Exception ex)
            {
                // Display any errors
                InlezenExcelLabel.Text = ("[" + ex.GetType().Name + "] " + ex.Message + ex.StackTrace);

            }

            GridView1.DataBind();
            showError(); 

        }





    }
}