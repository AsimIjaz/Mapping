using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using Devart.Data.Oracle;
using System.Configuration;
using System.IO;

namespace mapping
{
    public partial class ReadExcelFile : System.Web.UI.Page
    {

        private DataTable data;
       // private string connectionStringExcel;
       // private String connectionString;`

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.AutoGenerateColumns = true;
            data = new DataTable();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            String connectionString = ConfigurationManager.ConnectionStrings["cloxxsDB"].ConnectionString;

            try
            {
                // Clear the DataGridView and the connection string TextBox
                data.Dispose();
                data = new DataTable();
 
                string connectionStringExcel = (@"Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + Server.MapPath("~") + "\\excelsavemap\\CLOXXS_CONVERSIE." + "xlsx" + ";" + "Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"");
 
                // Fill the DataGridView and connection string TextBox
                using (OleDbConnection connection = new OleDbConnection(connectionStringExcel))
                {
                    connection.Open();


                    using (OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Conversie_Boekingen$]", connection))
                    {
                        adapter.Fill(data);

                        GridView1.DataSource = data;
                        GridView1.Visible = true;
                        GridView1.DataBind();
                        Label1.Text = data.Rows.Count.ToString();


                        // Create new DataSet
                        DataSet ds = new DataSet();

                        ds.Tables.Add(data);
                        // Loop through the rows of the only table in the DataSet
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            try
                            {
                                // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                                OracleConnection myConn1 = new OracleConnection(connectionString);
                                myConn1.Open();

                                String insertQuery = "";

                                insertQuery += " INSERT INTO CONVERSIE_BOEKINGEN(   ";
                                insertQuery += " PROJECT_ID, PROJECT_NAAM, RAPPORTDATUM,  BEDRIJF,  ACTIVUM,  GROOTBOEKNR,  OMSCHRIJVING_GROOTBOEKNUMMER,  TAAK,  OMSCHRIJVING_TAAK,  KOSTENSOORT,  OMSCHRIJVING_KOSTENSOORT,       ";
                                insertQuery += " DOCUMENT,  OMSCHRIJVING_BOEKING_VERP,  OMSCHRIJVING_FACTUUR,  VERPLICHTING,  DEBITEUR,  DEBITEUR_NAAM,  CREDITEUR,  CREDITEUR_NAAM,  PER_GB,  DIENSTJAAR,  FACTUURDATUM,       ";
                                insertQuery += " BOEKINGSDATUM,  BETAALDATUM,  DEBET,  CREDIT,  BTW,  BTW_CODE,  OPVOERING,  TOTAAL_OPVOERING,  AANTAL_DEBET,  AANTAL_CREDIT,  SOORT_BOEKING,  VOLGNUMMER,  KREDIETBEHEERDER,       ";
                                insertQuery += " OMSCHRIJVING_KREDIETBEHEERDER,  CONV_GROOTBOEKNUMMER,  CONV_TAAK,  CONV_VERPLICHTINGNUMMER)     ";

                                insertQuery += " VALUES (:p1, :p2, TO_TIMESTAMP(:p3, 'DD-MM-YY HH24:MI:SSXFF'), :p4, :p5, :p6, :p7, :p8, :p9, :p10, :p11, :p12, :p13, :p14, :p15, :p16, :p17, :p18, :p19, :p20, :p21, TO_DATE(:p22, 'DD-MM-YY'), TO_DATE(:p23, 'DD-MM-YY'), TO_DATE(:p24, 'DD-MM-YY'), :p25, :p26, :p27, :p28, :p29, :p30, :p31, :p32, :p33, :p34, :p35, :p36, :p37, :p38, :p39) ";

                                // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                                OracleCommand myCommand = new OracleCommand(insertQuery, myConn1);
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

                            }
                            catch(Exception excInsert){

                                //throw new Exception(excInsert.StackTrace);

                                string LogUrl = MapPath("~/excelsavemap/LogConversieBoekingen.txt");
                                StreamWriter SW;
                                SW = File.CreateText(LogUrl);
                                SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                                SW.WriteLine(excInsert.Source + ", " + excInsert.Message + ". " + excInsert.StackTrace);
                                SW.Close();
                            }




                            try
                            {
                                // delete all null values
                                // ConnectieString uit de Web.Config ophalen om regels toe te voegen  in de tabel Conversie_Boekingen
                                OracleConnection myConnDelete = new OracleConnection(connectionString);
                                myConnDelete.Open();

                                // Nu moet de data van de excel weggeschreven worden naar oracle tabel Conversie_Boekingen dr = de ExcelSheet
                                String DeleteNullQuery = " DELETE FROM CONVERSIE_VERPLICHTINGEN WHERE PROJECT_ID IS NULL ";

                                OracleCommand myCommandDelete = new OracleCommand(DeleteNullQuery, myConnDelete);

                                myCommandDelete.ExecuteNonQuery();
                                myCommandDelete.Dispose();
                                myConnDelete.Close();
                            }
                            catch(Exception excInsert2)
                            {
                                string LogUrl = MapPath("~/excelsavemap/LogConversieBoekingen.txt");
                                StreamWriter SW;
                                SW = File.CreateText(LogUrl);
                                SW.WriteLine("Datum:[" + System.DateTime.Now.ToLongDateString() + "]" + "   Tijd:[" + System.DateTime.Now.ToLongTimeString() + "]");
                                SW.WriteLine(excInsert2.Source + ", " + excInsert2.Message + ". " + excInsert2.StackTrace);
                                SW.Close();
                            }



                            //Label1.Text += dataRow[0].ToString();
                            //Response.Write("Integer Value : " + dataRow["IntegerValue"].ToString() + "<br>");
                            //Response.Write("String Value : " + dataRow["StringValue"].ToString() + "<br>");
                        }


                    }
                }


                // Label1.Text = data.Columns["GBKNR"].ToString();

            }
            catch (Exception ex)
            {
                // Display any errors
                Label1.Text = ("[" + ex.GetType().Name + "] " + ex.Message + ex.StackTrace);
            }

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        








    

    }
}