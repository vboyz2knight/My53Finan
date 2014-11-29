using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace MyWeb.CSharp.My53Finan
{
    public partial class ShowData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Db"] != null)
            {
                if (Session["Db"] == "yup")
                {
                    //get our data from database
                    string connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

                    SqlConnection SqlConnection = new SqlConnection(connectionString) ;

                    SqlCommand command = new SqlCommand();
                    //command.CommandText = "select
                }
            }
            else if( Session["myTranSactions"] != null)
            {
            }
            else
            {
                // No data found
            }
        }
    }
}