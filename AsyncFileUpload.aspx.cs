using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Data;

namespace mapping
{
    public partial class AsyncFileUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AsyncFileUpload1.UploadedComplete += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedComplete);
            AsyncFileUpload1.UploadedFileError += new EventHandler<AsyncFileUploadEventArgs>(AsyncFileUpload1_UploadedFileError);


            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OleDb.4.0;Data Source=C:\MappingAppV3\2011_05_04_cloxxs_mapping\source\excelsavemap\asimtestInvoice.xls;Extended Properties=Excel 8.0");
            OleDbCommand command = new OleDbCommand("SELECT * FROM [Invoices$]", connection);
            OleDbDataReader dr;

            connection.Open();
            dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable excelData = new DataTable("ExcelData");
            excelData.Load(dr);

            //GridView1.DataSource = excelData; 
        }


        void AsyncFileUpload1_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "size", "top.$get(\"" + uploadResult.ClientID + "\").innerHTML = 'Uploaded size: " + AsyncFileUpload1.FileBytes.Length.ToString() + "';", true);

            // Uncomment to save to AsyncFileUpload\Uploads folder.
            // ASP.NET must have the necessary permissions to write to the file system.

            //string savePath = MapPath("~/excelsavemap/" + Path.GetFileName(e.filename));
            // string savePath = MapPath("~/AsyncFileUpload/Uploads/" + Path.GetFileName(e.filename));
            // AsyncFileUpload1.SaveAs(savePath);
        }

        void AsyncFileUpload1_UploadedFileError(object sender, AsyncFileUploadEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "top.$get(\"" + uploadResult.ClientID + "\").innerHTML = 'Error: " + "" + "';", true);
        }


    }
}