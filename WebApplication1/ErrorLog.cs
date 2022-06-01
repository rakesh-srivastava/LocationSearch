using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjLocationSearch
{
    public class ErrorLog
    {
        public static bool WriteLog(string strMessage)
        {
            try
            {
                string strFileName = "";
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", Path.GetTempPath(), strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(DateTime.Now.ToString()+ " \t ErrorMessage: "+ strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
