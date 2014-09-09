using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Macro_Manager.models;
using System.Globalization;

namespace Macro_Manager.helper
{
    public static class Logger
    {
        private static string iiplaycode;
        private static bool logOpeningHours;
        private static Dictionary<string, string> logInfos;

        public static Dictionary<string, string> LogInfos
        {
            get { return Logger.logInfos; }
            set { Logger.logInfos = value; }
        }

        public static bool LogOpeningHours
        {
            get { return Logger.logOpeningHours; }
            set { Logger.logOpeningHours = value; }
        }

        public static string Iiplaycode
        {
            get { return Logger.iiplaycode; }
            set { Logger.iiplaycode = value; }
        }

        public static void initLogger(){
            logInfos = new Dictionary<string, string>();
        }

        //public static void writeLogfile() {

        //    try
        //    {
        //        //if (logInfos == null) {
        //        //    logInfos = new Dictionary<string, string>();
        //        //}


        //        string date = DateTime.Now.Date.ToString(new CultureInfo("de-DE")).Replace(" 00:00:00", "").Replace('.', '-');
        //        string time = DateTime.Now.TimeOfDay.ToString();
        //        logInfos.Add("time", time);
        //        string path = Config.logPath + date;

        //        string message = logInfos["message"].ToString();

        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
                
        //        //debugoutput: Console.WriteLine(logInfos["status"].ToString()); Console.Read();
        //        switch (logInfos["status"].ToString())
        //        {
        //            case "error": path += "\\errorlog.txt"; break;
        //        }
 
        //        using (StreamWriter sw = File.AppendText(path))
        //        {
        //            switch (logInfos["status"].ToString())
        //            {
        //                //case "debug": sw.Write("Zeitpunkt: {0}\nKunde: {1}\nVerzeichnis: {2}\nBenutzer: {3}\nMacro Code: {4}Csv Data\n", time, customer, directory, user, message);
        //                default: sw.Write("Zeitpunkt: {0}\nMeldung: {1}\n", time, message);
        //                    //debugoutput: Console.WriteLine("write logfile: " + message);Console.Read();
        //                    break;
        //            }

                    
        //            sw.Write("\n**********************************************************************************************\n\n");
        //        }
        //    }
        //    catch (Exception e) {
        //        Console.Write(e.Message + "\n" + e.StackTrace + "\nweiter mit return ..");
        //        Console.Read();
            
        //    }

        //}


        public static void writeLogfile()
        {

            try
            {
                //if (logInfos == null) {
                //    logInfos = new Dictionary<string, string>();
                //}


                string date = DateTime.Now.Date.ToString(new CultureInfo("de-DE")).Replace(" 00:00:00", "").Replace('.', '-');
                string time = DateTime.Now.TimeOfDay.ToString();
                logInfos.Add("time", time);
                string path = Config.logPath + date;
                string customer = "", directory = "", user = "", message ="";
                message = logInfos["message"].ToString();
                try
                {
                    customer = logInfos["customer"].ToString();
                    directory = logInfos["directory"].ToString();
                    user = logInfos["user"].ToString();
                    
                }
                catch (KeyNotFoundException knfEx) { 
                  //do nothing ..
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //debugoutput: Console.WriteLine(logInfos["status"].ToString()); Console.Read();
                switch (logInfos["status"].ToString())
                {

                    case "success": path += "\\successlog.txt"; break;
                    case "error": path += "\\errorlog.txt"; break;
                    case "init_error": path += "\\errorlog.txt"; break;
                    case "debug": path += "\\debuglog.txt"; break;
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    switch (logInfos["status"].ToString())
                    {
                        //case "debug": sw.Write("Zeitpunkt: {0}\nKunde: {1}\nVerzeichnis: {2}\nBenutzer: {3}\nMacro Code: {4}Csv Data\n", time, customer, directory, user, message);
                        case "init_error": sw.Write("Zeitpunkt: {0}\nMeldung: {1}\n", time, message); break;
                        default: sw.Write("Zeitpunkt: {0}\nKunde: {1}\nVerzeichnis: {2}\nBenutzer: {3}\nMeldung: {4}\n", time, customer, directory, user, message);
                            //debugoutput: Console.WriteLine("write logfile: " + message);Console.Read();
                            break;
                    }


                    sw.Write("\n**********************************************************************************************\n\n");
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message + "\n" + e.StackTrace + "\nweiter mit return ..");
                Console.Read();

            }

        }
    }
}
