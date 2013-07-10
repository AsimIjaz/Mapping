using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Devart.Data;
using Devart.Data.Oracle;

namespace mapping
{
    public class BusinessObjectDATA
    {
        String connectionString = ConfigurationManager.ConnectionStrings["cloxxsDB"].ConnectionString;
        int numberOfRows = 0;
        string oldActivaNummer = "";

        public DataSet KoppelingenSelectAll(String filterProject, String filterSheetType, String filterSortType, String filterSortDirection)
        {
            DataSet dsData = new DataSet();
            String sqlQuery = "";

            sqlQuery = sqlQuery + "SELECT    KOPPELINGID,  ";
            sqlQuery = sqlQuery + "          CASE  ";
            sqlQuery = sqlQuery + "            WHEN SHEETTYPE = 'BU' THEN 'Budgetsheet'  ";
            sqlQuery = sqlQuery + "            WHEN SHEETTYPE = 'FI' THEN 'Financieringsheet'  ";
            sqlQuery = sqlQuery + "          END AS SHEETTYPE,  ";
            sqlQuery = sqlQuery + "          CODE,  ";
            sqlQuery = sqlQuery + "          NAAM,  ";
            sqlQuery = sqlQuery + "          GROOTBOEKREKENING,   ";
            sqlQuery = sqlQuery + "          TAAKNUMMER,  ";
            sqlQuery = sqlQuery + "          AMOUNTSPRESENT  ";
            sqlQuery = sqlQuery + "FROM      VW_KOPPELINGEN ";
            sqlQuery = sqlQuery + "WHERE	 PROJECTID = " + filterProject + " ";
            sqlQuery = sqlQuery + "AND	     SHEETTYPE = '" + filterSheetType + "' ";
            sqlQuery = sqlQuery + "ORDER BY  " + filterSortType + " " + filterSortDirection + " ";

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlQuery;

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Koppelingen");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }

        public DataSet ProjectActivaSelectAll(String filterProject)
        {
            DataSet dsData = new DataSet();
            String sqlQuery = "";

            sqlQuery = sqlQuery + "SELECT    PA.PROJECTACTIVAID,  ";
            sqlQuery = sqlQuery + "          PJ.NAME AS PROJECTNAAM,  ";
            sqlQuery = sqlQuery + "          PA.ACTIVANUMMER   ";
            sqlQuery = sqlQuery + "FROM      PROJECTS PJ, ODS_PROJECT_ACTIVA PA ";
            sqlQuery = sqlQuery + "WHERE	 PJ.PROJECTID = PA.PROJECTID ";
            sqlQuery = sqlQuery + "AND	     PJ.PROJECTID = " + filterProject + " ";
            sqlQuery = sqlQuery + "ORDER BY  PROJECTNAAM ASC ";

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlQuery;

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Koppelingen");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }

        public void KoppelingUpdate(int KOPPELINGID, string GROOTBOEKREKENING, string TAAKNUMMER)
        {
            try
            {
                DataSet dsData = new DataSet();
                String updateCommandNew = "";
                String strGROOTBOEKREKENING = GROOTBOEKREKENING;
                String strTAAKNUMMER = TAAKNUMMER;

                // corrections on input values
                if (TAAKNUMMER == null)
                {
                    strTAAKNUMMER = "";
                }
                else
                {
                    if (TAAKNUMMER.Trim() == "0")
                    {
                        strTAAKNUMMER = "";
                    }
                }

                OracleConnection myOracleConnection = new OracleConnection(connectionString);
                OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
                myOracleCommand.CommandText = " SELECT KOPPELINGID FROM KOPPELINGEN WHERE KOPPELINGID = " + KOPPELINGID + " ";

                OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
                myOracleDataAdapter.SelectCommand = myOracleCommand;

                myOracleConnection.Open();
                numberOfRows = myOracleDataAdapter.Fill(dsData, "CloxxsODSData");

                myOracleCommand.Dispose();
                myOracleCommand = null;
                myOracleDataAdapter.Dispose();
                myOracleDataAdapter = null;

                if (numberOfRows > 0)
                {
                    updateCommandNew = " UPDATE KOPPELINGEN SET GROOTBOEKREKENING = '" + strGROOTBOEKREKENING + "', TAAKNUMMER = '" + strTAAKNUMMER + "' WHERE KOPPELINGID = " + KOPPELINGID + " ";

                    myOracleCommand = myOracleConnection.CreateCommand();
                    myOracleCommand.CommandText = updateCommandNew;
                    numberOfRows = myOracleCommand.ExecuteNonQuery();

                    myOracleCommand.Dispose();
                    myOracleCommand = null;
                }

                myOracleConnection.Dispose();
                myOracleConnection = null;
            }
            catch (Exception currentException)
            {
                string myErrorString = currentException.Message + ", " + currentException.Source + ", " + currentException.StackTrace;
                if (myErrorString.Contains("ORA-00001: unique constraint"))
                {
                    throw new System.Exception("Deze combinatie van Grootboekrekening en Taaknummer bestaat al.");
                }
            }
            finally
            {

            }
        }

        public void ProjectActivaUpdate(int PROJECTACTIVAID, string ACTIVANUMMER)
        {
            DataSet dsData = new DataSet();
            String updateCommandNew = "";

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = " SELECT PROJECTACTIVAID FROM ODS_PROJECT_ACTIVA WHERE PROJECTACTIVAID = " + PROJECTACTIVAID + " ";

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "CloxxsODSData");

            myOracleCommand.Dispose();
            myOracleCommand = null;
            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;

            if (numberOfRows > 0)
            {
                updateCommandNew = " UPDATE ODS_PROJECT_ACTIVA SET ACTIVANUMMER = '" + ACTIVANUMMER + "' WHERE PROJECTACTIVAID = " + PROJECTACTIVAID + " ";

                myOracleCommand = myOracleConnection.CreateCommand();
                myOracleCommand.CommandText = updateCommandNew;
                numberOfRows = myOracleCommand.ExecuteNonQuery();

                myOracleCommand.Dispose();
                myOracleCommand = null;
            }

            myOracleConnection.Dispose();
            myOracleConnection = null;
        }

        public void ProjectActivaDelete(int PROJECTACTIVAID)
        {
            DataSet dsData = new DataSet();
            String sqlCommandNew = "";

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            sqlCommandNew = " DELETE FROM ODS_PROJECT_ACTIVA WHERE PROJECTACTIVAID = " + PROJECTACTIVAID + " ";

            myOracleConnection.Open();
            myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlCommandNew;
            numberOfRows = myOracleCommand.ExecuteNonQuery();

            myOracleCommand.Dispose();
            myOracleCommand = null;


            myOracleConnection.Dispose();
            myOracleConnection = null;
        }

        public DataSet ProjectenSelectAll()
        {
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = " SELECT PROJECTID, NAME AS PROJECTNAME FROM PROJECTS ";

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

        public DataSet ProjectenSelectByProgramID(string ProgramID)
        {
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = " SELECT PROJECTID, NAME AS PROJECTNAME FROM PROJECTS WHERE PROGRAMID = " + ProgramID + " ORDER BY PROJECTNAME ";

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

        public DataSet ProgrammasSelectAll()
        {
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = " SELECT PROGRAMID, NAME AS PROGRAMNAME FROM PROGRAMS ORDER BY PROGRAMNAME ";

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

        public DataSet GrootboekrekeningenSelectAll()
        {
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = " SELECT '' AS GROOTBOEKREKENING FROM DUAL UNION SELECT TO_CHAR(FCL) AS GROOTBOEKREKENING FROM STG_ODS_OVER_TE_HALEN_FCL ORDER BY GROOTBOEKREKENING ";

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

        public DataSet GrootboekrekeningenSelectByProjectID(string ProjectID)
        {
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            //myOracleCommand.CommandText = " SELECT '' AS GROOTBOEKREKENING FROM DUAL UNION SELECT TO_CHAR(FCL) AS GROOTBOEKREKENING FROM STG_ODS_OVER_TE_HALEN_FCL ORDER BY GROOTBOEKREKENING ";
            myOracleCommand.CommandText = " SELECT '' AS GROOTBOEKREKENING FROM DUAL UNION select TO_CHAR(fcl) as GROOTBOEKREKENING from stg_ods_project_fcl_lijst where projectid = " + ProjectID + " order by GROOTBOEKREKENING ";
            //select fcl from stg_ods_project_fcl_lijst where projectid = 163 order by fcl

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

        public DataSet TaaknummerSelectAll()
        {
            DataSet dsData = new DataSet();
            StringBuilder sbSelectQuery = new StringBuilder();

            sbSelectQuery.AppendLine("SELECT        TAAKNUMMERID, ");
            sbSelectQuery.AppendLine("              TAAKNUMMER ");
            sbSelectQuery.AppendLine("FROM          TAAKNUMMERS ");
            sbSelectQuery.AppendLine("ORDER BY TAAKNUMMER ");

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

        public DataSet TaaknummerSelectByGrootboekrekening(string Grootboekrekening)
        {
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            if (Grootboekrekening != null)
            {
                //sbSelectQuery.AppendLine(" SELECT TAAK AS TAAKNUMMER FROM STG_ODS_VERP WHERE FCL = '" + filterGrootboekrekening + "' ");
                myOracleCommand.CommandText = " SELECT TAAK AS TAAKNUMMER FROM stg_ods_project_fcl_lijst WHERE FCL = " + Grootboekrekening + " ";
            }
            else
            {
                myOracleCommand.CommandText = " SELECT '' AS TAAKNUMMER FROM DUAL ";
            }

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

        public DataSet ProjKoppDataSelectByProgramID(string filterProgram)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            sb.AppendLine(" SELECT    PJ.PROJECTID, ");
            sb.AppendLine("           PJ.NAME AS PROJECTNAME, ");
            sb.AppendLine("           OD.KOPPELEN, ");
            sb.AppendLine("           OD.DATUM ");
            sb.AppendLine(" FROM      ODS_PROJECT_KOPELLEN OD, ");
            sb.AppendLine("           PROJECTS PJ ");
            sb.AppendLine(" WHERE     OD.PROJECTID = PJ.PROJECTID ");
            sb.AppendLine(" AND       PJ.PROGRAMID = " + filterProgram + " ");
            sb.AppendLine(" ORDER BY PJ.NAME ");

            myOracleCommand.CommandText = sb.ToString();

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

        public DataSet SelectBudgetsNotMapped()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            sb.AppendLine(" SELECT PROGRAMS.NAME AS PROGRAMNAME, PROJECTS.NAME AS PROJECTNAME, SUBPROJECTS.NAME AS SUBPROJECTNAME, BUDGETS.NAME AS BUDGETNAME ");
            sb.AppendLine(" FROM PROGRAMS, PROJECTS, SUBPROJECTS, BUDGETS  ");
            sb.AppendLine(" WHERE BUDGETS.SUBPROJECTID = SUBPROJECTS.SUBPROJECTID ");
            sb.AppendLine(" AND   SUBPROJECTS.PROJECTID = PROJECTS.PROJECTID ");
            sb.AppendLine(" AND   PROJECTS.PROGRAMID = PROGRAMS.PROGRAMID ");
            sb.AppendLine(" AND BUDGETID NOT IN  ");
            sb.AppendLine(" ( ");
            sb.AppendLine("   SELECT SHEETID FROM KOPPELINGEN WHERE SHEETTYPE = 'BU' AND NOT GROOTBOEKREKENING IS NULL OR GROOTBOEKREKENING <> '' ");
            sb.AppendLine(" ) ");
            sb.AppendLine(" ORDER BY PROGRAMS.NAME, PROJECTS.NAME, SUBPROJECTS.NAME, BUDGETS.NAME ");

            myOracleCommand.CommandText = sb.ToString();

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

        public DataSet SelectAccountsNotMapped()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            sb.AppendLine(" SELECT FCL AS GROOTBOEKREKENING, TAAK AS TAAKNUMMER FROM STG_ODS_PROJECT_FCL_LIJST ST ");
            sb.AppendLine(" LEFT JOIN KOPPELINGEN KP ON KP.GROOTBOEKREKENING = ST.FCL AND KP.TAAKNUMMER = ST.TAAK ");
            sb.AppendLine(" WHERE KP.GROOTBOEKREKENING IS NULL ");

            myOracleCommand.CommandText = sb.ToString();

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

        public void ProjKoppDataUpdate(int PROJECTID, int KOPPELEN, string datum)
        {
            DataSet dsData = new DataSet();
            String updateCommandNew = "";

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = " SELECT PROJECTID FROM ODS_PROJECT_KOPELLEN WHERE PROJECTID = " + PROJECTID + " ";

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "CloxxsODSData");

            myOracleCommand.Dispose();
            myOracleCommand = null;
            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;

            if (numberOfRows > 0)
            {
                updateCommandNew = " UPDATE ODS_PROJECT_KOPELLEN SET KOPPELEN = " + KOPPELEN + ", DATUM = TO_DATE('" + datum + "', 'DD-MM-YYYY')" + " WHERE PROJECTID = " + PROJECTID + " ";

                myOracleCommand = myOracleConnection.CreateCommand();
                myOracleCommand.CommandText = updateCommandNew;
                numberOfRows = myOracleCommand.ExecuteNonQuery();

                myOracleCommand.Dispose();
                myOracleCommand = null;
            }

            myOracleConnection.Dispose();
            myOracleConnection = null;
        }


        //asim facturen ods aanpassen select

        public DataSet SelectInvoicesHistory()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            sb.AppendLine("   SELECT bookingdate, description, amount, code FROM INVOICES_HISTORY");

            myOracleCommand.CommandText = sb.ToString();

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Facturen_ODS");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }


        // filter invoices
        public DataSet FilterInvoices(string projectnummer)
        {
            DataSet dsData = new DataSet();
            String sqlQuery = "";

            sqlQuery = sqlQuery + "SELECT i.invoiceid, i.code FROM INVOICES I INNER JOIN CONTRACTS C ON I.code = C.code  INNER JOIN BUDGETS B ON C.budgetid = b.budgetid INNER JOIN SUBPROJECTS SB ON B.subprojectid = sb.subprojectid INNER JOIN PROJECTS P ON sb.projectid = P.projectid ";
            sqlQuery = sqlQuery + "WHERE p.projectid =" + projectnummer;


            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlQuery;

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Facturen_Filter");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }



        // update invoices
        public DataSet UpdateInvoices(int invoiceid, string code)
        {
            DataSet dsData = new DataSet();
            String sqlQuery = "";

            sqlQuery += "UPDATE INVOICES SET CODE = " + code + " WHERE INVOICEID = " + invoiceid;



            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlQuery;

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            //numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Facturen_Update");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }


        //asim ophalen FactuurLijst
        public DataSet FacturenLijst()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsData = new DataSet();

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();

            sb.AppendLine(" SELECT INVOICES.invoiceid , INVOICES.invoicenumber as DocumentNummer_1, to_char(INVOICES.invoicedate, 'dd-mm-yyyy') as BoekingsDatum , to_char(INVOICES.bookingdate, 'dd-mm-yyyy')as DienstJaar , to_char(INVOICES.fullfillmentdate, 'dd-mm-yyyy') as ValutaDatum ,(SUBSTR(INVOICES.description, 1, 25 ))as Omschrijving_Boeking, INVOICES.amount as Credit_Debit, INVOICES.companyid as CrediteurenNr, SUBSTR(INVOICES.Code, 0, 8) as FCL,  SUBSTR(INVOICES.Code, 10, 5) as TAAK, SUBSTR(INVOICES.Code, 16, 5) as VerplNr, '' as N_FCL, '' as N_Taak, '' as N_VerplNr  ");
            sb.AppendLine(" FROM INVOICES  INNER JOIN CONTRACTS C ON INVOICES.code = C.code INNER JOIN BUDGETS B ON C.budgetid = b.budgetid INNER JOIN SUBPROJECTS SB ON B.subprojectid = sb.subprojectid INNER JOIN PROJECTS P ON sb.projectid = P.projectid   ");
            sb.AppendLine(" WHERE ROWNUM < 3");

            myOracleCommand.CommandText = sb.ToString();

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_Lijst_facturen");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }


        // Mappingf.aspx = Conversie_Boekingen
        // Select Methode met een Project_ID Parameter om te filteren
        public DataSet SelectBoekingenProject(int PROJECT_ID)
        {
            DataSet dsData = new DataSet();
            String sqlQuery = "";

            sqlQuery += "SELECT * FROM CONVERSIE_BOEKINGEN WHERE PROJECT_ID =" + PROJECT_ID;
            //  sqlQuery += "SELECT * FROM CONVERSIE_BOEKINGEN";

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlQuery;

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_CONVERSIE_BOEKINGEN");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }



        // MappingC.aspx = Conversie_Verplichtingen
        // Select Methode met een Project_ID Parameter om te filteren
        public DataSet SelectVerplichtingenProject(int PROJECT_ID)
        {
            DataSet dsData = new DataSet();
            String sqlQuery = "";

            sqlQuery += "SELECT * FROM CONVERSIE_VERPLICHTINGEN WHERE PROJECT_ID =" + PROJECT_ID;
            //  sqlQuery += "SELECT * FROM CONVERSIE_BOEKINGEN";

            OracleConnection myOracleConnection = new OracleConnection(connectionString);
            OracleCommand myOracleCommand = myOracleConnection.CreateCommand();
            myOracleCommand.CommandText = sqlQuery;

            OracleDataAdapter myOracleDataAdapter = new OracleDataAdapter();
            myOracleDataAdapter.SelectCommand = myOracleCommand;

            myOracleConnection.Open();
            numberOfRows = myOracleDataAdapter.Fill(dsData, "Cloxxs_CONVERSIE_VERPLICHTINGEN");

            myOracleDataAdapter.Dispose();
            myOracleDataAdapter = null;
            myOracleConnection.Dispose();
            myOracleConnection = null;

            return dsData;
        }



    }
}