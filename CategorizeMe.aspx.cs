using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;

namespace MyWeb.CSharp.My53Finan
{
    public partial class CategorizeMe : System.Web.UI.Page
    {
        TranSactionS myTransactions = null;
        TranSactionS modifiedTransactions = null;
        TranSaction currentTransaction = null;
        String XMLFileLoc = "";
        String myLogLoc = "";
        List<KeyValuePair<string, string>> MyFilterXML = null;
        MyLogger myLogging = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetXMLFileLoc();

            if (Session["myTranSactions"] != null)
            {
                myTransactions = (TranSactionS)Session["myTranSactions"];
                                
                if (Session["modifiedTransactions"] != null)
                {
                    modifiedTransactions = (TranSactionS)Session["modifiedTransactions"];
                }

                GetXMLFilter();

                
                FilterPanel.Visible = true;
                PopulateCategories(myTransactions);
                
            }
            else
            {
                FilterPanel.Visible = false;
                lblMessage.Text = "Error:  No data found.";
            }
        }

        public bool GetXMLFilter()
        {
            bool bReturn = false;

            if (Session["XMLFilter"] == null)
            {
                if (!BuildMyFilter())
                {
                    myLogging.LogError("Unable to find filter. ", "Error");

                    MyFilterXML = new List<KeyValuePair<string, string>>();

                    Session["XMLFilter"] = MyFilterXML;
                    bReturn = true;
                }
                else
                {
                    Session["XMLFilter"] = MyFilterXML;
                }
            }
            else
            {
                MyFilterXML = (List<KeyValuePair<string, string>>)Session["XMLFilter"];
                bReturn = true;
            }

            return bReturn;
        }

        public bool GetXMLFileLoc()
        {
            bool bReturn = false;

            if (Session["XMLFileLoc"] != null)
            {
                XMLFileLoc = Session["XMLFileLoc"].ToString();

            }
            else
            {
                System.Configuration.Configuration rootWebConfig1 = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/");
                if (rootWebConfig1.AppSettings.Settings.Count > 0)
                {
                    System.Configuration.KeyValueConfigurationElement customSetting2 = rootWebConfig1.AppSettings.Settings["53FilterXMLFile"];
                    if (customSetting2 != null)
                    {
                        XMLFileLoc = HttpContext.Current.Server.MapPath(customSetting2.Value);
                        bReturn = true;
                    }
                }
                else
                {
                    myLogging.LogError("No Filter file loaded, setting to default filter file.", "Warning");
                    XMLFileLoc = HttpContext.Current.Server.MapPath("/CSharp/My53Finan/Uploads/Filter.xml");
                }

                //saving it to session
                Session["XMLFileLoc"] = XMLFileLoc;
            }

            return bReturn;
        }

        private bool SaveFilter()
        {
            bool bReturn = false;
            FileStream fs = null;

            try
            {
                //Load XML Is it valid?
                fs = new FileStream(XMLFileLoc, FileMode.Open, FileAccess.ReadWrite);
                XmlDocument document = new XmlDocument();
                document.Load(fs);

                //close stream
                if (fs != null)
                {
                    fs.Close();
                }

                XmlElement root = document.DocumentElement;

                //select all nodes categories
                XmlNodeList searchNodeList = document.SelectNodes("Categories");
                //insert out new category description in xml
                //search all child node of categories


                foreach (XmlNode searchNode in searchNodeList)
                {
                    XmlNodeList searchNode2List = searchNode.ChildNodes;
                    foreach (XmlNode searchNode3 in searchNode2List)
                    {
                        foreach (KeyValuePair<string, string> element in MyFilterXML)
                        {
                            //we match the category
                            if (searchNode3.Name == element.Key)
                            {
                                bool bDuplicate = false;

                                if (searchNode3.ChildNodes.Count > 0)
                                {    
                                    //not duplicate value?
                                    XmlNodeList searchNode4List = searchNode3.ChildNodes;
                                    foreach (XmlNode searchNode5 in searchNode4List)
                                    {
                                        string sInner = searchNode5.InnerText.ToLower();
                                        string sValue = element.Value.ToLower();
                                        if (sInner.Equals(sValue))
                                        {
                                            bDuplicate = true;
                                            break;
                                        }

                                    }

                                    if (!bDuplicate)
                                    {
                                        XmlElement newElement = document.CreateElement(searchNode3.Name + "X");
                                        newElement.InnerText = element.Value;
                                        root[searchNode3.Name].AppendChild(newElement);

                                    }
                                }
                                else
                                {
                                    XmlElement newElement = document.CreateElement(searchNode3.Name + "X");
                                    newElement.InnerText = element.Value;
                                    root[searchNode3.Name].AppendChild(newElement);

                                }

                            }
                        }
                    }

                }

                fs = new FileStream(XMLFileLoc, FileMode.Create, FileAccess.ReadWrite);

                document.Save(fs);
            }
            catch (Exception e)
            {
                //"Error: Unable to update filter."
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return bReturn;
        }

        protected void bttnSubmitFilter_Click(object sender, EventArgs e)
        {
            if (Session["currentTransaction"] != null)
            {
                currentTransaction = (TranSaction) Session["currentTransaction"];

                if (lblTransactionDescription.Text.IndexOf(txtFilter.Text) != -1)
                {
                    //look at our updated MyFilterXML to see if there an existing filter
                    if(!CategoryMe(currentTransaction) )
                    {
                        MyFilterXML.Add(new KeyValuePair<string, string>(RadioButtonList1.SelectedItem.Text, txtFilter.Text.Trim()));
                        Session["XMLFilter"] = MyFilterXML;

                        currentTransaction.myCategory = RadioButtonList1.SelectedValue.ToString();
                        currentTransaction.myFilter = txtFilter.Text.Trim();

                        if (modifiedTransactions != null)
                        {
                            modifiedTransactions.Add(currentTransaction);                        
                        }
                        else
                        {
                            modifiedTransactions = new TranSactionS();
                            modifiedTransactions.Add(currentTransaction);
                        }

                        myTransactions.Remove(currentTransaction);

                        if (myTransactions.Count > 0)
                        {
                            Session["modifiedTransactions"] = modifiedTransactions;
                            Server.Transfer("CategorizeMe.aspx");
                        }
                    }
                }
                else
                {
                    lblFilterError.Text = "Please choose your filter words part of the description.";
                }
            }
            else
            {
                lblFilterError.Text = "Something has horrible gone wrong.";
            }
        }

        private void PopulateCategories(TranSactionS mTranSactionS)
        {
                foreach (TranSaction s in mTranSactionS.ToList())
                {
                    currentTransaction = s;

                    if (!CategoryMe(currentTransaction))
                    {
                        //can't get the category by comparing what the filter xml have
                        //have to ask for user to figure out what it is

                        lblTransactionDescription.Text = s.Description.ToString();
                        Session["currentTransaction"] = currentTransaction;

                        break;

                    }
                }

                if (myTransactions.Count < 1)
                {
                    //if our filter is complete display our result in new page
                    SaveFilter();
                    Session.Remove("myTranSactions");
                    //Session.Remove("XMLFilter");
                    //Session.Remove("XMLFileLoc");
                    Session["modifiedTransactions"] = modifiedTransactions;
                    Server.Transfer("MyFinan53.aspx");
                }
        }

        //this method build List KeyValuePair of <String Category, String Value> from an XML file
        protected Boolean BuildMyFilter()
        {
            Boolean bReturn = true;
            MyFilterXML = new List<KeyValuePair<string, string>>();
            FileStream fs = null;


            //do we have a filter xml?
            //open or create XML file
            if (String.IsNullOrEmpty(XMLFileLoc))
            {
                //use default file.
                myLogging.LogError("Using default XML filter file.", "Warning");
                XMLFileLoc = @"\filters.xml";
            }

            //file exist?
            if (!File.Exists(XMLFileLoc))
            {

                //create XML file
                try
                {
                    fs = new FileStream(XMLFileLoc, FileMode.Create, FileAccess.ReadWrite);
                    XmlDocument document = new XmlDocument();

                    //create xml declaration and append it
                    XmlDeclaration dec = document.CreateXmlDeclaration("1.0", null, null);
                    document.AppendChild(dec);

                    //create root element
                    XmlElement root = document.CreateElement("Categories");
                    document.AppendChild(root);

                    //create the new nodes
                    short counter = 0;
                    foreach (TranSaction.enCategory myCat in Enum.GetValues(typeof(TranSaction.enCategory)))
                    {
                        XmlElement newElement = document.CreateElement(myCat.ToString());
                        newElement.InnerText = "";
                        root.InsertAfter(newElement, root.FirstChild);
                        counter++;
                    }

                    document.Save(fs);

                }
                catch (IOException e)
                {
                    //MessageBox.Show(e.ToString());
                    bReturn = false;
                }
                finally
                {
                    //close fstream
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            ////////////////////////////////////////
            try
            {
                //Load XML Is it valid?
                fs = new FileStream(XMLFileLoc, FileMode.Open, FileAccess.ReadWrite);
                XmlDocument document = new XmlDocument();
                document.Load(fs);

                //close stream
                if (fs != null)
                {
                    fs.Close();
                }

                string tmpString = "";
                String key = "";
                String value = "";

                XmlElement root = document.DocumentElement;

                //select all nodes categories
                XmlNodeList searchNodeS = document.SelectNodes("Categories");



                foreach (XmlNode searchNode in searchNodeS)
                {
                    tmpString = searchNode.Name;

                    //search all child node of categories                            
                    XmlNodeList searchNodeS2 = searchNode.ChildNodes;

                    tmpString = searchNodeS2.ToString();
                    foreach (XmlNode searchNode2 in searchNodeS2)
                    {
                        tmpString = searchNode2.Name;

                        XmlNodeList searchNodeS2a = searchNode2.ChildNodes;
                        foreach (XmlNode searchNode2a in searchNodeS2a)
                        {
                            tmpString = searchNode2a.InnerText;
                            if (searchNode2a.InnerText.Length > 1)
                            {
                                key = searchNode2.Name;
                                value = searchNode2a.InnerText;
                                MyFilterXML.Add(new KeyValuePair<string, string>(key, value));
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                myLogging.LogError(e.ToString(), "Error");
                bReturn = false;
            }
            finally
            {
                //close stream
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return bReturn;
        }

        protected bool CategoryMe(TranSaction s)
        {
            bool bReturn = false;

            if (s != null)
            {
                foreach (var element in MyFilterXML)
                {
                    if ((s.Description.ToLower().IndexOf(element.Value.ToLower()) != -1) && s.Description.Length > 0)
                    {
                        s.myFilter = element.Value;
                        s.myCategory =  element.Key;
                        if (modifiedTransactions != null)
                        {
                            modifiedTransactions.Add(s);
                            myTransactions.Remove(s);
                        }
                        else
                        {
                            modifiedTransactions = new TranSactionS();
                            modifiedTransactions.Add(s);
                            myTransactions.Remove(s);
                        }
                        bReturn = true;
                    }
                }
            }
            else
            {
                myLogging.LogError("Transaction is NULL.", "Error");
                bReturn = false;
            }

            return bReturn;
        }
    }
}