using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace MyWeb.CSharp.My53Finan
{
    public class MyLogger
    {
        private string logFileLoc = @"/CSharp/My53Finan/53Logs.txt";  

        public bool LogError(string logError, string logType)
        {
            bool bReturn = false;
            FileStream aFile = null;
            StreamWriter sWriter = null;

            try
            {
                aFile = new FileStream(logFileLoc, FileMode.Append);
                sWriter = new StreamWriter(aFile);

                DateTime logDate = DateTime.Now;

                sWriter.WriteLine(logDate.ToString("{0:d/M/yyyy HH:mm:ss}     ") + logError);
            }
            catch (IOException e)
            {
                //
            }
            finally
            {
                if (aFile != null)
                {
                    aFile.Close();
                }

                if (sWriter != null)
                {
                    sWriter.Close();
                }
            }


            return bReturn;
        }
    }
}