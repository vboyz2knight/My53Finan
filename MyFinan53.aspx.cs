using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWeb.CSharp.My53Finan;
using System.Web.Configuration;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace MyWeb.CSharp.My53Finan
{
    public partial class MyFinan53 : System.Web.UI.Page
    {
        TranSactionS myMainTranSactionS = null;
        TranSactionS selectTranSactionS = null;
        List<KeyValuePair<string, string>> MyFilterXML = null;
        Lookup<string, decimal> barGraphLookUp = null;
        String XMLFileLoc = "";
        String UploadDir = "";
        String myLogLoc = "";
        MyLogger myLogging = null;

        //endDate, startdate = DateTime.MinValue -> Last Latest month.
        DateTime endDate = DateTime.MinValue;
        DateTime startDate = DateTime.MinValue;
        //DateTime latestTransactionDate = DateTime.MinValue;
        
               
        protected void Page_Load(object sender, EventArgs e)
        {
            IntialValue();

            if (Session["modifiedTransactions"] != null)
            {
                IntroductionPanel.Visible = false;
                ViewPanel.Visible = true;

                myMainTranSactionS = (TranSactionS)Session["modifiedTransactions"];

                if (Session["endDate"] != null)
                {
                    endDate = (DateTime) Session["endDate"];
                }

                if (Session["startDate"] != null)
                {
                    startDate = (DateTime) Session["startDate"];
                }

                DrawMyBarChart(myMainTranSactionS);
                DrawMyPieChart(myMainTranSactionS, startDate, endDate);
            }
            else
            {
                IntroductionPanel.Visible = true;
                ViewPanel.Visible = false;
            }
          

            if (IsPostBack)
            {
               
                
            }
            else if (!IsPostBack)
            {
                
            }
        }

        private void IntialValue()
        {
            myLogging = new MyLogger();
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
                    myLogging.LogError("No Upload directory loaded, setting to default directory.", "Warning");
                    UploadDir = HttpContext.Current.Server.MapPath("/CSharp/My53Finan/Uploads/");
                }

                //saving it to session
                Session["UploadDir"] = UploadDir;
            }
        }


        private bool DrawMyBarChart(TranSactionS TransactionS)
        {
            bool bReturn = false;

            if (TransactionS != null)
            {
                TranSactionS deposit = new TranSactionS();
                TranSactionS withraw = new TranSactionS();

                Legend leg = new Legend();
                Chart3.Legends.Add(leg);

                Chart3.Series.Add("Spending");
                Chart3.Series.Add("Deposit");

                //Chart3.Series["Spending"].PostBackValue = "#INDEX";
                //Chart3.Series["Spending"].LegendPostBackValue = "#INDEX";

                //Chart3.Series["Deposit"].PostBackValue = "#INDEX";
                //Chart3.Series["Deposit"].LegendPostBackValue = "#INDEX";

                withraw.AddRange(TransactionS);

                foreach (TranSaction s in withraw.ToList())
                {
                    if (s.myCategory=="Bank")
                    {
                        deposit.Add(s);
                        withraw.Remove(s);
                    }
                }

                barGraphLookUp = (Lookup<string, decimal>)withraw.ToLookup(p => p.myDate.ToString("y"), p => p.Amt);

                foreach (IGrouping<string, decimal> TransactionsGroup in barGraphLookUp)
                {
                    decimal amount = 0.00m;
                    
                    foreach(decimal s in TransactionsGroup)
                    {
                        amount += Math.Abs(s);
                    }

                    Chart3.Series["Spending"].Points.AddXY(TransactionsGroup.Key,amount );
                }

                barGraphLookUp = (Lookup<string, decimal>)deposit.ToLookup(p => p.myDate.ToString("y"), p => p.Amt);

                foreach (IGrouping<string, decimal> TransactionsGroup in barGraphLookUp)
                {
                    decimal amount = 0.00m;

                    foreach (decimal s in TransactionsGroup)
                    {
                        amount += Math.Abs(s);
                    }

                    Chart3.Series["Deposit"].Points.AddXY(TransactionsGroup.Key, amount);
                }

                // Set series visual attributes
                Chart3.Series["Spending"].ChartType = SeriesChartType.Column;
                Chart3.Series["Spending"].ShadowOffset = 2;
                Chart3.Series["Spending"].Color = Color.Red;


                Chart3.Series["Deposit"].ChartType = SeriesChartType.Column;
                Chart3.Series["Deposit"].ShadowOffset = 2;
                Chart3.Series["Deposit"].Color = Color.Green;

                Chart3.ChartAreas[0].AxisY.Title = "Value in $$";
                Chart3.Titles[0].Text = "Overall Spendings";

                
                bReturn = true;
            }

            return bReturn;
        }

        private void ShowMyTransactions(TranSactionS transactionS,string desiredCategory)
        {
            TranSactionS DesiredCategoryTranSactionS = new TranSactionS();

            foreach (TranSaction s in transactionS)
            {
                if (s.myCategory == desiredCategory.ToString())
                {
                    DesiredCategoryTranSactionS.Add(s);
                }
            }

            ListView1.DataSource = DesiredCategoryTranSactionS;
            ListView1.DataBind();

            DrawMyPieChart(DesiredCategoryTranSactionS,desiredCategory);
        }
        private DateTime GetLatestTransactionDate(TranSactionS Transactions)
        {
            Transactions.Sort();
            //our sort should sort highest to lowest
            //so we can pull the first transaction out and it should have the last latest month
            return (Transactions.First().myDate);
        }
        private DateTime GetOldestTransactionDate(TranSactionS Transactions)
        {
            Transactions.Sort();
            //our sort should sort highest to lowest
            //so we can pull the first transaction out and it should have the last latest month
            return (Transactions.Last().myDate);
        }
        //no date params, assume to draw pie chart from only the latest last month.
        private void DrawMyPieChart(TranSactionS myDrawTransactions,DateTime startDate,DateTime endDate)
        {
            DateTime monthYearOnly=DateTime.MinValue;
            DateTime latestTransactionDate = GetLatestTransactionDate(myDrawTransactions);

            if (selectTranSactionS == null)
            {
                selectTranSactionS = new TranSactionS();
            }
            else
            {
                selectTranSactionS.Clear();
            }

            if (myDrawTransactions != null)
            {             
                //show last latest month transactions
                if (startDate.Equals(DateTime.MinValue) && endDate.Equals(DateTime.MinValue))
                {                    
                    foreach (TranSaction s in myDrawTransactions)
                    {
                        //since our list is sorted, from highest
                        if ((s.myDate.Year.Equals(latestTransactionDate.Year) && s.myDate.Month.Equals(latestTransactionDate.Month)))
                        {
                           
                            selectTranSactionS.Add(s);
                        }
                        else
                        {
                            //we only interested in the last latest month transactions
                            break;
                        }
                    }

                }
                else if( endDate > startDate )
                {
                    foreach (TranSaction s in myDrawTransactions)
                    {
                        if ( (s.myDate >= startDate) && (s.myDate <= endDate) )
                        {
                            selectTranSactionS.Add(s);
                        }
                    }
                }
                else if (endDate.Equals(startDate) )
                {
                    foreach (TranSaction s in myDrawTransactions)
                    {
                        if ((s.myDate.Equals(endDate)) )
                        {
                            selectTranSactionS.Add(s);
                        }
                    }
                }

                if (selectTranSactionS != null)
                {
                    MyGraph graph1 = new MyGraph(selectTranSactionS);
                    Legend leg = new Legend();
                    Chart1.Legends.Add(leg);

                    // Set series and legend tooltips
                    Chart1.Series["Series1"].ToolTip = "#VALX: #VAL{C}";
                    Chart1.Series["Series1"].LegendToolTip = "#PERCENT";
                    Chart1.Series["Series1"].PostBackValue = "#INDEX";
                    Chart1.Series["Series1"].LegendPostBackValue = "#INDEX";

                    Chart1.Series["Series1"].Points.DataBindXY(graph1.GraphDatas, "Category", graph1.GraphDatas, "Amount");

                    // Set series visual attributes
                    Chart1.Series["Series1"].ChartType = SeriesChartType.Pie;
                    Chart1.Series["Series1"].ShadowOffset = 2;
                    Chart1.Series["Series1"].BorderColor = Color.DarkGray;
                    Chart1.Series["Series1"]["PieLabelStyle"] = "inside";

                    ListView1.DataSource = selectTranSactionS;
                    ListView1.DataBind();
                }


                Session["endDate"] = endDate;
                Session["startDate"] = startDate;
            }
            else
            {
                //Error, Null datas
            }
        }


        private void DrawMyPieChart(TranSactionS selectTranSactionS, string desiredCategory)
        {
            if (selectTranSactionS != null)
            {
                MyGraph graph1 = new MyGraph(selectTranSactionS, desiredCategory);
                foreach (TranSaction s in selectTranSactionS)
                {
                    if (s.myCategory == desiredCategory)
                    {
                    }
                }

                Legend leg = new Legend();

                Chart1.Legends.Add(leg);
                Chart1.Series["Series1"].Points.DataBindXY(graph1.GraphDatas, "Category", graph1.GraphDatas, "Amount");

            }
            else
            {
                //no data selected
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
                        myLogging.LogError("No transaction found.","Error");
                    }
                    else
                    {

                        //We need to put modified the category depend on each transaction

                        Session["myTranSactions"] = myMainTranSactionS;
                        Server.Transfer("CategorizeMe.aspx");                        

                    }

                }
                catch (Exception ex)
                {
                    myLogging.LogError(ex.Message.ToString(),"Error");
                }
            }
            else
            {
                lblError.Text += "You have not specified a file.";
            }
        }

        protected void Chart1_Click(object sender, ImageMapEventArgs e)
        {
            int pointIndex = int.Parse(e.PostBackValue);
            Series series = Chart1.Series["Series1"];
            if (pointIndex >= 0 && pointIndex <= series.Points.Count)
            {
                series.Points[pointIndex].CustomProperties += "Exploded=true";
                if (selectTranSactionS != null)
                {
                    BindSubCategory(series.Points[pointIndex].AxisLabel, selectTranSactionS);
                }
            }
   
        }

        private void BindSubCategory(string category, TranSactionS selectTranSactionS)
        {
            if (category.Length > 0)
            {
                category = category.Replace("Total", "");
                FilterTransactionsWithThisCategory(category, selectTranSactionS);
            }
        }

        private void FilterTransactionsWithThisCategory(string category, TranSactionS selectTranSactionS)
        {
            TranSactionS subTransactionS = new TranSactionS();
            Dictionary<string, decimal> displayTransactions = new Dictionary<string, decimal>();
            bool bExist = false;

            //find all our category
            foreach (TranSaction s in selectTranSactionS)
            {
                if (category.ToLower().IndexOf(s.myCategory.ToString().ToLower()) != -1)
                {
                    subTransactionS.Add(s);
                }
            }

            //group all the same filter into one
            foreach (TranSaction s in subTransactionS)
            {
                bExist = false;

                foreach(KeyValuePair<string, decimal> p in displayTransactions)
                {
                    //filter already exist, update the amount
                    if (s.myFilter.ToLower().IndexOf(p.Key.ToLower()) != -1)
                    {
                        bExist = true;
                        string mFilter = p.Key;
                        decimal mAmount = p.Value + Math.Abs(s.Amt);

                        displayTransactions.Remove(p.Key);
                        displayTransactions.Add(mFilter, mAmount);

                        break;
                    }
                }

                if (!bExist)
                {
                    displayTransactions.Add(s.myFilter, Math.Abs(s.Amt));
                }
            }

            if (displayTransactions != null)
            {
                ListView1.DataSource = subTransactionS;
                ListView1.DataBind();

                //MyGraph graph1 = new MyGraph(subTransactionS, p);
                Legend leg = new Legend();
                Chart2.Legends.Add(leg);

                //Chart1.Series["Series1"]["Exploded"] = "true";
                // Set series and legend tooltips
                Chart2.Series["Series2"].ToolTip = "#VALX: #VAL{C}";
                Chart2.Series["Series2"].LegendToolTip = "#PERCENT";
                //Chart2.Series["Series2"].PostBackValue = "#INDEX";
               //Chart2.Series["Series2"].LegendPostBackValue = "#INDEX";

                Chart2.Series["Series2"].Points.DataBindXY(displayTransactions.Keys, "Filter", displayTransactions.Values, "Amount");

                // Set series visual attributes
                Chart2.Series["Series2"].ChartType = SeriesChartType.Pie;
                Chart2.Series["Series2"].ShadowOffset = 2;
                Chart2.Series["Series2"].BorderColor = Color.DarkGray;
                //Chart2.Series["Series2"]["PieLabelStyle"] = "outside";
            }
        }
        /*
        protected void Button1_Click(object sender, EventArgs e)
        {
            ShowMyTransactions(myTranSactionS);
        }
        */
        
        protected void RecentMonth_Click(object sender, EventArgs e)
        {
            DrawMyPieChart(myMainTranSactionS, DateTime.MinValue, DateTime.MinValue);
        }

        protected void Latest3Month_Click(object sender, EventArgs e)
        {
            DateTime latestTransactionDate = GetLatestTransactionDate(myMainTranSactionS);

            DateTime endDate = latestTransactionDate;
            //we need to back 3 months later from latest Transaction, only want the months different not the date
            // latest transaction is in May, we want startDate to be in March.
            DateTime startDate = latestTransactionDate.AddMonths(-2);

            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            DrawMyPieChart(myMainTranSactionS, startDate, endDate);
        }

        protected void Latest6Month_Click(object sender, EventArgs e)
        {
            DateTime latestTransactionDate = GetLatestTransactionDate(myMainTranSactionS);

            DateTime endDate = latestTransactionDate;
            //we need to back 3 months later from latest Transaction, only want the months different not the date
            // latest transaction is in May, we want startDate to be in March.
            DateTime startDate = latestTransactionDate.AddMonths(-5);

            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            DrawMyPieChart(myMainTranSactionS, startDate, endDate);
        }

        protected void Latest12Month_Click(object sender, EventArgs e)
        {
            DateTime latestTransactionDate = GetLatestTransactionDate(myMainTranSactionS);

            DateTime endDate = latestTransactionDate;
            //we need to back 3 months later from latest Transaction, only want the months different not the date
            // latest transaction is in May, we want startDate to be in March.
            DateTime startDate = latestTransactionDate.AddMonths(-11);

            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            DrawMyPieChart(myMainTranSactionS, startDate, endDate);
        }
        
        protected void bSelectDates_Click(object sender, EventArgs e)
        {
            if ((txtEndDate.Text.Length > 0) && (txtStartDate.Text.Length > 0))
            {
                if (myMainTranSactionS != null)
                {
                    DateTime startDate = DateTime.Now;
                    DateTime endDate = DateTime.Now;

                    try
                    {
                        DateTime.TryParse(txtStartDate.Text, out startDate);
                        DateTime.TryParse(txtEndDate.Text, out endDate);

                        if ((startDate < GetOldestTransactionDate(myMainTranSactionS)) ||
                        (endDate > GetLatestTransactionDate(myMainTranSactionS)))
                        {
                            //your start date need to be
                            lblErrorGraph.Text = "Please choose dates between " + GetOldestTransactionDate(myMainTranSactionS).ToShortDateString() + " and " + GetLatestTransactionDate(myMainTranSactionS).ToShortDateString();
                        }
                        else
                        {
                            DrawMyPieChart(myMainTranSactionS, startDate, endDate);
                        }
                    }
                    catch
                    {
                        //unable to parse our inputs
                        lblErrorGraph.Text = "Inputs need to be in format of mm/dd/yyyy";
                    }
                    finally
                    {
                    }

                    
                }
                else
                {
                    //error no data transaction data found
                    lblErrorGraph.Text = "No data transaction data found.";
                }
            }
            else
            {
                //inputs are needed
                lblErrorGraph.Text = "Inputs StartDate and EndDate are needed.";
            }

        }
    }
}