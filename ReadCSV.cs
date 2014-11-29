using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MyWeb.CSharp.My53Finan
{
    public class ReadCSV
    {
        string fileName="";
        MyLogger myLogging = null;

        public ReadCSV(string fileLoc)
        {
            //set file you be using
            if (fileLoc.Length > 0)
            {
                fileName = fileLoc;
            }
            else
            {
                //no csv file location
                
            }

            myLogging = new MyLogger();
        }

        public TranSactionS ReadMyCSV()
        {
            TranSactionS myTransactions = new TranSactionS();

            //file exist?
            FileStream aFile = null;
            StreamReader sreader = null;
            try
            {
                aFile = new FileStream(fileName, FileMode.Open);
                sreader = new StreamReader(aFile);

                string line;
                line = sreader.ReadLine();
                //read each line
                //Date,Description,"Check Number",Amount format 
                //Note in Description, there might be multiple commas
                //So we parse for Date, Amount, Check #, and assume the rest is Description

                DateTime tranDate;
                decimal tranAmnt;
                string tranCategory;
                string tranType;
                string tranCheck;
                string tranFilter = "Empty";
                string tmp;
                int pos = 0;

                while (line != null)
                {
                    //get the string to the first position of ,
                    pos = line.IndexOf(',');

                    tmp = line.Remove(pos);

                    // try to put it in DateTime, else it not a transaction line
                    if (DateTime.TryParse(tmp, out tranDate))
                    {
                        //remove the previous DateTime
                        line = line.Substring(pos + 2);

                        //get the string last position of ,
                        pos = line.LastIndexOf(',');
                        tmp = line.Substring(pos++);
                        tmp = tmp.Replace(",", "");
                      
                        if (decimal.TryParse(tmp, out tranAmnt))
                        {
                            if (tranAmnt < 0)
                            {
                                tranType = "Withraw";
                            }
                            else
                            {
                                tranType = "Deposit";
                            }

                            //remove the last string and to try find the check description if there any
                            line = line.Substring(0, pos - 1);
                            pos = line.LastIndexOf(',');
                            tmp = line.Substring(pos);
                            tmp = tmp.Replace(",", "");
                            tranCheck = tmp;

                            //whatever left in string should be category because of the way data are
                            //filter it as HouseBill,Restaurant,Grocery,CarMaintenance????
                            line = line.Substring(0, pos - 1);
                            tranCategory = line;

                            //put it in our transaction
                            TranSaction transaction;

                            transaction = new TranSaction(tranDate, tranCategory, tranCheck, tranAmnt, tranType, tranCategory,tranFilter);
                            
                            //put it in our transactionS
                            myTransactions.Add(transaction);

                        }
                        else
                        {
                            //Unable to parse for line amount.
                            myLogging.LogError("Unable to parse for amount."+line, "Error");
                        }
                    }
                    else
                    {
                        //Not a transaction line or invalid format. Unable to parse for date.
                        myLogging.LogError("Not a transaction line or invalid format. Unable to parse for date."+line, "Warning");
                    }


                    line = sreader.ReadLine();
                }


            }
            catch (IOException e)
            {
                myLogging.LogError(e.ToString(),"Error");
            }
            finally
            {
                //destroy file thingy?
                if (sreader != null)
                {
                    sreader.Dispose();
                }

                if (aFile != null)
                {
                    aFile.Dispose();
                }
            }
            return (myTransactions);
        }
    }
}