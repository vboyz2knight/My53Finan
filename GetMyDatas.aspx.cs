using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MyWeb.CSharp.My53Finan
{
    public partial class Choice : System.Web.UI.Page
    {
        string logFile = "\\CSharp\\My53Finan\\53Errors.txt";
        TranSactionS myMainTranSactionS = null;
        String UploadDir = "";
        MyLogger errLog = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            errLog = new MyLogger(logFile);

            UploadCSVPanel.Visible = false;
            IntialValue();
        }

        protected void UploadCSV_Click(object sender, EventArgs e)
        {
            ChoicePanel.Visible = false;
            UploadCSVPanel.Visible = true;
        }

        protected void UseDemo_Click(object sender, EventArgs e)
        {
            ChoicePanel.Visible = false;

            //string connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            //using(SqlConnection SqlConnection = new SqlConnection(connectionString));

            //SqlCommand command = new SqlCommand;
            //command.CommandText = "select
            Session["Db"] = "yup";
            Server.Transfer("ShowData.aspx");
        }

        private void IntialValue()
        {
            //// initialize our UploadDir          
            if (Session["UploadDir"] != null)
            {
                UploadDir = Session["UploadDir"].ToString();
            }
            else
            {
                System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
                if (rootWebConfig1.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting = rootWebConfig1.AppSettings.Settings["53UploadDir"];
                    if (customSetting != null)
                    {
                        UploadDir = HttpContext.Current.Server.MapPath(customSetting.Value);
                    }
                }
                else
                {
                    errLog.LogError("No Upload directory loaded, setting to default directory.", "Warning");
                    UploadDir = HttpContext.Current.Server.MapPath("/CSharp/My53Finan/Uploads/");
                }

                //saving it to session
                Session["UploadDir"] = UploadDir;
            }
        }

        protected void bttnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    //to set max upload size: web.config.comments file, find a node called <httpRuntime> ->set the maxRequestLength
                    //http://msdn.microsoft.com/en-us/library/aa479405.aspx


                    FileUpload1.SaveAs(UploadDir + FileUpload1.FileName);
                    lblError.Text = "File name: " + FileUpload1.PostedFile.FileName + "<br>" + FileUpload1.PostedFile.ContentLength + " kb<br>" +
                                    "Content type: " + FileUpload1.PostedFile.ContentType;

                    //Our category transactions are first loaded with description
                    //Build of filter to figure out what category each transaction

                    //Read the csv file
                    String FileName = UploadDir + FileUpload1.FileName;


                    ReadCSV myCSV = new ReadCSV(FileName);
                    myMainTranSactionS = myCSV.ReadMyCSV();

                    if (myMainTranSactionS.Count < 1)
                    {
                        errLog.LogError("No transaction found.", "Error");
                    }
                    else
                    {

                        //We need to put modified the category depend on each transaction

                        Session["myTranSactions"] = myMainTranSactionS;
                        Server.Transfer("CategorizeMe.aspx");                        

                    }

                    if (Session["myTranSactions"] == null)
                    {
                        Session["myTranSactions"] = myMainTranSactionS;
                    }

                    Server.Transfer("ShowData.aspx");

                }
                catch (Exception ex)
                {
                    errLog.LogError(ex.Message.ToString(), "Error");
                }
            }
            else
            {
                lblError.Text += "You have not specified a file.";
            }
        }
    }
}