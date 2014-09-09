using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Macro_Manager.helper;
using System.IO;
using System.Reflection;
using Macro_Manager.views;

namespace Macro_Manager.models
{

    public  class Config
    {

        public static string csvPath = "";
        public static string macroPath = "";
        public static string picturePath = "";
        public static string logPath = "";
        
        public Config() {


            IniFile iniFile = new IniFile(GetApplicationsPath().Replace("Debug", "").Replace("Release", "")+ "config.ini");
            if(!File.Exists(iniFile.Path)){
                Console.WriteLine("Konfigurationsdatei (config.ini) fehlt.\nBitte in Anwendungsverzeichnist (" + GetApplicationsPath() + ") einspielen");
                Console.ReadLine();
                Environment.Exit(0);
            }
            csvPath =iniFile.IniReadValue("pathes", "csvpath");
            macroPath =iniFile.IniReadValue("pathes", "macropath");
            picturePath =iniFile.IniReadValue("pathes", "picturepath");
            logPath =iniFile.IniReadValue("pathes", "logpath");
            
            Macro_Manager.helper.Logger.LogOpeningHours = true;

        }

        

        public static string GetApplicationsPath()
        {
            FileInfo fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            return fi.DirectoryName;
        } 


    }
}
