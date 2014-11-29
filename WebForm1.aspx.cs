using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyWeb.CSharp.My53Finan
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1_Click1(null, new EventArgs());
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string[] arr1 = new string[] { "one", "two", "three" };

            for(int i=0;i<arr1.Length;i++)
            {
                //TextBox1.Text = arr1[i].ToString();

                Button1_Click(null,new EventArgs());
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Label.Text = "Good";
            ModalPopupExtender1.Show();
        }
    }
}