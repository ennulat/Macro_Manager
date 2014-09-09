using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macro_Manager.models;
using Macro_Manager.helper;
using Macro_Manager.views;
using iMacros;
using System.Collections;
using System.IO;

namespace Macro_Manager.controllers
{
    class c_Foursquare : BusinessDirectory
    {

        public c_Foursquare(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Foursquare";
        }

        public override void Main()
        {
            
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("registration");
            BusinessDirectory.macroTypeList.Add("profil");
            BusinessDirectory.macroTypeList.Add("update");
            base.Main();
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "-fx";
            switch (macroType)
            {
                case "registration": browsertype = "-fx"; break;
                case "profil": browsertype = "-fx"; break;
                case "update": browsertype = "-fx"; break;
                default: browsertype = ""; break;
            }

            m_app.iimOpen(browsertype, true, 300);

        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {

                case "registration": base.finalSuccessStatus = "Willkommen bei Foursquare"; break;
                case "profil": base.finalSuccessStatus = base.csvVars["COL4"].ToString(); break;
                case "update": base.finalSuccessStatus = "XXXXXXXXXXXXX"; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {

                case "registration": setiiPlayListRegistration(); break;
                case "profil": setiiPlayListProfil(); break;
                case "update": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
            }

        }
       

        protected override void setiiPlayListRegistration()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Foursquare\\Foursquare_registration.iim");
            processStepsViewList.Add("Eintrag Registrationsdaten ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }
        
        protected override void setiiPlayListProfil()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Foursquare\\Foursquare_profil.iim");
            processStepsViewList.Add("Eintrag Firmenprofil ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }



       

    }
}
