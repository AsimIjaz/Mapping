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
//Rapportdatum
//Bedrijf
//Activum
//Grootboeknr
//Omschrijving_grootboeknummer
//Taak
//Omschrijving_Taak
//Kostensoort
//Omschrijving_kostensoort
//Document
//Omschrijving_boeking_verp
//Omschrijving_factuur
//Verplichting
//Debiteur
//Debiteur_naam
//Crediteur
//Crediteur_naam
//Per_gb
//Dienstjaar
//Factuurdatum
//Boekingsdatum
//Betaaldatum
//Debet
//Credit
//BTW
//BTW_code
//Opvoering
//Totaal_opvoering
//Aantal_debet
//Aantal_credit
//Soort_boeking
//Volgnummer
//Kredietbeheerder
//Omschrijving_kredietbeheerder
//CONV_Grootboeknummer
//CONV_Taak
//CONV_Verplichtingnummer

using cloxxs_general;

namespace mapping
{
    public partial class mappingF : System.Web.UI.Page
    {
        Boolean verpBoxStatus { get; set; }
        Boolean boekBoxStatus { get; set; }
        public static string LogUrl = "";

        public void showError()
        {
            TextBoxLog.Visible = true;
            TextBoxLog.ForeColor = System.Drawing.Color.Red;
            TextBoxLog.Font.Bold = true;

            string FileName = null;
            StreamReader fp = null;
            FileName = "excelsavemap/LogConversieBoekingen.txt";

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

            LogUrl = MapPath("~/excelsavemap/LogConversieBoekingen.txt");
            // debug test start
            //LogManager lm = new LogManager();
            //lm.LogMessage("testsessionID", "Mapping", "mappingF", "Page_Load", "this is a test message " + DateTime.Now.ToString());
            //lm = null;
            // debug test end

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

            AsyncFileUpload1.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedComplete);
            AsyncFileUpload1.UploadedFileError += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedFileError);

        }

        void AsyncFileUpload1_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "", false);
            string savePath = MapPath("~/excelsavemap/" + Path.GetFileName(e.FileName));
            AsyncFileUpload1.SaveAs(savePath);
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

        protected void Button1_Click1(object sender, EventArgs e)
        {
            

            long processedRows = 0;
            File.WriteAllText(MapPath("~/excelsavemap/LogConversieBoekingen.txt"), String.Empty); 


            DataTable data = new DataTable();
            DataTable data2 = new DataTable();
            DataTable data3 = new DataTable();

            string fileExtension = Session["UPLOAD_FILE_EXTENSION"].ToString();
            String connectionString = ConfigurationManager.ConnectionStrings["cloxxsDB"].ConnectionString;
            string connectionStringExcel = (@"Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Server.MapPath("~") + "\\excelsavemap\\CLOXXS_CONVERSIE." + fileExtension + ";" + "Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"");
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
                // Fill the DataGridView and connection string TextBox
              

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Conversie_Boekingen$]", connection))
                    {
                        try
                        {

                            //data = null;
                            adapter.Fill(data);

                            //GridView1.DataSource = data;
                            //GridView1.Visible = true;
                            //GridView1.DataBind();
                            lblUitvoerInlezen.Text = data.Rows.Count.ToString();


                            // Create new DataSet
                            DataSet ds = new DataSet();
                            ds.Tables.Add(data);
                            // Loop through the rows of the only table in the DataSet
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {


                                // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                                OracleConnection myConn = new OracleConnection(connectionString);
                                myConn.Open();

                                // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                                String DeleteQuery = "DELETE FROM CONVERSIE_BOEKINGEN WHERE PROJECT_ID = :p1 ";

                                OracleCommand myCommand = new OracleCommand(DeleteQuery, myConn);
                                myCommand.ParameterCheck = true;
                                myCommand.Prepare();
                                myCommand.Parameters[0].Value = dr["PROJECT_ID"];

                                myCommand.ExecuteNonQuery();

                                myConn.Close();

                            }
                        }
                        catch (Exception exceptionDelete)
                        {
                            connection.Close();
                            string LogUrl = MapPath("~/excelsavemap/LogConversieBoekingen.txt");
                            StreamWriter SW;
                            SW = File.CreateText(LogUrl);
                            SW.WriteLine("[Exception tijdens het verwijderen van de null regels (stap1)]");
                            SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                            SW.WriteLine(exceptionDelete.Source + ", " + exceptionDelete.Message + ". " + exceptionDelete.StackTrace);
                            SW.Close();
                        }

                    }
                    #endregion stap1
                    //string LogUrl = MapPath("~/excelsavemap/LogConversieBoekingen.txt");

               

                #region stap2
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Conversie_Boekingen$]", connection))
                    {
                        string LogUrl = MapPath("~/excelsavemap/LogConversieBoekingen.txt");
                        StreamWriter SW;
                        SW = File.CreateText(LogUrl);

                        adapter.Fill(data2);
                        OracleConnection myConn1 = new OracleConnection();
                        OracleCommand myCommand = new OracleCommand();

                        // Create new DataSet
                        DataSet ds = new DataSet();
                        ds.Tables.Add(data2);

                        // number of rows to go
                        SW.WriteLine("[rows to go:" + ds.Tables[0].Rows.Count.ToString() + "]");

                        // Loop through the rows of the only table in the DataSet
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            try
                            {
                                //teller/counter +1 for rows
                                processedRows = processedRows + 1;

                                // start processing
                                SW.WriteLine("[start: processing row:" + processedRows.ToString() + "]");

                                // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                                myConn1 = new OracleConnection(connectionString);
                                myConn1.Open();

                                String insertQuery = "";

                                insertQuery += " INSERT INTO CONVERSIE_BOEKINGEN(   ";
                                insertQuery += " PROJECT_ID, PROJECT_NAAM, RAPPORTDATUM,  BEDRIJF,  ACTIVUM,  GROOTBOEKNR,  OMSCHRIJVING_GROOTBOEKNUMMER,  TAAK,  OMSCHRIJVING_TAAK,  KOSTENSOORT,  OMSCHRIJVING_KOSTENSOORT,       ";
                                insertQuery += " DOCUMENT,  OMSCHRIJVING_BOEKING_VERP,  OMSCHRIJVING_FACTUUR,  VERPLICHTING,  DEBITEUR,  DEBITEUR_NAAM,  CREDITEUR,  CREDITEUR_NAAM,  PER_GB,  DIENSTJAAR,  FACTUURDATUM,       ";
                                insertQuery += " BOEKINGSDATUM,  BETAALDATUM,  DEBET,  CREDIT,  BTW,  BTW_CODE,  OPVOERING,  TOTAAL_OPVOERING,  AANTAL_DEBET,  AANTAL_CREDIT,  SOORT_BOEKING,  VOLGNUMMER,  KREDIETBEHEERDER,       ";
                                insertQuery += " OMSCHRIJVING_KREDIETBEHEERDER,  CONV_GROOTBOEKNUMMER,  CONV_TAAK,  CONV_VERPLICHTINGNUMMER)     ";

                                insertQuery += " VALUES (:p1, :p2, TO_TIMESTAMP(:p3, 'DD-MM-YY HH24:MI:SSXFF'), :p4, :p5, :p6, :p7, :p8, :p9, :p10, :p11, :p12, :p13, :p14, :p15, :p16, :p17, :p18, :p19, :p20, :p21, TO_DATE(:p22, 'DD-MM-YY'), TO_DATE(:p23, 'DD-MM-YY'), TO_DATE(:p24, 'DD-MM-YY'), :p25, :p26, :p27, :p28, :p29, :p30, :p31, :p32, :p33, :p34, :p35, :p36, :p37, :p38, :p39) ";

                                // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                                myCommand = new OracleCommand(insertQuery, myConn1);
                                myCommand.ParameterCheck = true;
                                myCommand.Prepare();

                                // p1-p38 waardes
                                myCommand.Parameters[0].Value = dr["PROJECT_ID"];
                                myCommand.Parameters[1].Value = dr["PROJECT_NAAM"];
                                myCommand.Parameters[2].Value = dr["RAPPORTDATUM"];
                                myCommand.Parameters[3].Value = dr["BEDRIJF"];
                                myCommand.Parameters[4].Value = dr["ACTIVUM"];
                                myCommand.Parameters[5].Value = dr["GROOTBOEKNR"];
                                myCommand.Parameters[6].Value = dr["OMSCHRIJVING_GROOTBOEKNUMMER"];
                                myCommand.Parameters[7].Value = dr["TAAK"];
                                myCommand.Parameters[8].Value = dr["OMSCHRIJVING_TAAK"];
                                myCommand.Parameters[9].Value = dr["KOSTENSOORT"];
                                myCommand.Parameters[10].Value = dr["OMSCHRIJVING_KOSTENSOORT"];
                                myCommand.Parameters[11].Value = dr["DOCUMENT"];
                                myCommand.Parameters[12].Value = dr["OMSCHRIJVING_BOEKING_VERP"];
                                myCommand.Parameters[13].Value = dr["OMSCHRIJVING_FACTUUR"];
                                myCommand.Parameters[14].Value = dr["VERPLICHTING"];
                                myCommand.Parameters[15].Value = dr["DEBITEUR"];
                                myCommand.Parameters[16].Value = dr["DEBITEUR_NAAM"];
                                myCommand.Parameters[17].Value = dr["CREDITEUR"];
                                myCommand.Parameters[18].Value = dr["CREDITEUR_NAAM"];
                                myCommand.Parameters[19].Value = dr["PER_GB"];
                                myCommand.Parameters[20].Value = dr["DIENSTJAAR"];
                                myCommand.Parameters[21].Value = dr["FACTUURDATUM"];
                                myCommand.Parameters[22].Value = dr["BOEKINGSDATUM"];
                                myCommand.Parameters[23].Value = dr["BETAALDATUM"];
                                myCommand.Parameters[24].Value = dr["DEBET"];
                                myCommand.Parameters[25].Value = dr["CREDIT"];
                                myCommand.Parameters[26].Value = dr["BTW"];
                                myCommand.Parameters[27].Value = dr["BTW_CODE"];
                                myCommand.Parameters[28].Value = dr["OPVOERING"];
                                myCommand.Parameters[29].Value = dr["TOTAAL_OPVOERING"];
                                myCommand.Parameters[30].Value = dr["AANTAL_DEBET"];
                                myCommand.Parameters[31].Value = dr["AANTAL_CREDIT"];
                                myCommand.Parameters[32].Value = dr["SOORT_BOEKING"];
                                myCommand.Parameters[33].Value = dr["VOLGNUMMER"];
                                myCommand.Parameters[34].Value = dr["KREDIETBEHEERDER"];
                                myCommand.Parameters[35].Value = dr["OMSCHRIJVING_KREDIETBEHEERDER"];
                                myCommand.Parameters[36].Value = dr["CONV_GROOTBOEKNUMMER"];
                                myCommand.Parameters[37].Value = dr["CONV_TAAK"];
                                myCommand.Parameters[38].Value = dr["CONV_VERPLICHTINGNUMMER"];


                                myCommand.ExecuteNonQuery();
 
                                myConn1.Close();
                                myConn1.Dispose();

                                // start processing
                                SW.WriteLine("[end  : processing row:" + processedRows.ToString() + "]");
                            }
                            catch (Exception excInsert)
                            {

                                myCommand.Dispose();
                                myConn1.Close();
                                myConn1.Dispose();

                                connection.Close();
                                connection.Dispose();

                                //SW = File.CreateText(LogUrl);
                                SW.WriteLine("[Exception tijdens het invoeren van regels(stap2)] ERROR REGEL[(" + processedRows + ")]");
                                SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                                SW.WriteLine(excInsert.Source + ", " + excInsert.Message + ". " + excInsert.StackTrace);
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
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Conversie_Boekingen$]", connection))
                    {
                            try
                            {
                                // delete all null values
                                // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                                myConnDelete = new OracleConnection(connectionString);
                                myConnDelete.Open();

                                // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                                String DeleteNullQuery = " DELETE FROM CONVERSIE_BOEKINGEN WHERE PROJECT_ID IS NULL ";

                                myCommandDelete = new OracleCommand(DeleteNullQuery, myConnDelete);

                                myCommandDelete.ExecuteNonQuery();
                                myCommandDelete.Dispose();
                                myConnDelete.Close();
                            }
                            catch (Exception excDelete2)
                            {
                                connection.Close();
                                connection.Dispose();
                                string LogUrl = MapPath("~/excelsavemap/LogConversieBoekingen.txt");
                            StreamWriter SW;
                            SW = File.CreateText(LogUrl);
                                SW.WriteLine("[Exception tijdens het verwijderen van de null regels(stap3)]");
                                SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                                SW.WriteLine(excDelete2.Source + ", " + excDelete2.Message + ". " + excDelete2.StackTrace);
                                SW.Close();

                            }
                        finally{
                               connection.Close();
                            connection.Dispose();
                            }

                        } 
                        #endregion stap3

                    }

                     
                }


                // Label1.Text = data.Columns["GBKNR"].ToString();
             catch (Exception ex)
            {
                // Display any errors
                lblUitvoerInlezen.Text = ("[" + ex.GetType().Name + "] " + ex.Message + ex.StackTrace);
               
            }


            GridView1.DataBind();
            showError(); 
        }


    }
}