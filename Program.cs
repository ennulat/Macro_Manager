using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using iMacros;
using System.Collections;
using Macro_Manager.controllers;
using Macro_Manager.helper;
using Macro_Manager.views;
using Macro_Manager.models;

namespace Macro_Manager
{
    class Program
    {

        static void Main(string[] args)
        {
            try{
                MainController mainController = new MainController();
                mainController.Main();
            }
            catch (Exception e)
            {
                string message = "Message: " + e.Message + "\n" + "Stacktrace:" + e.StackTrace;
                Logger.initLogger();
                Logger.LogInfos.Add("message", message);
                Logger.LogInfos.Add("status", "init_error");
                Logger.writeLogfile();
                console_menu.displayErrorMessage(message);
            }
        }

   
    }
}
