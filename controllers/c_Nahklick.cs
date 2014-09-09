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
    class c_Nahklick : BusinessDirectory
    {

        public c_Nahklick(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Nahklick";
        }

        public override void Main()
        {
            
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("registration");
            BusinessDirectory.macroTypeList.Add("profil");
            base.Main();
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "-fx";
            switch (macroType)
            {
                case "registration": browsertype = "-fx"; break;
                case "profil": browsertype = "-fx"; break;
                default: browsertype = ""; break;
            }

            m_app.iimOpen(browsertype, true, 300);

        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "registration": base.finalSuccessStatus = "Aktivierung erforderlich!"; break;
                case "profil": base.finalSuccessStatus = "Du hast Dich erfolgreich ausgeloggt."; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "registration": setiiPlayListRegistration(); break;
                case "profil": setiiPlayListProfil(); break;

            }

        }

        protected override void setiiPlayListRegistration() {
            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Nahklick\\Nahklick_registration.iim");
            processStepsViewList.Add("Account Registration ... ");
        
        }


        protected override void setiiPlayListProfil()
        {
            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Nahklick\\Nahklick_profil.iim");
            processStepsViewList.Add("Profil Eintrag ... ");

        }

       

    }
}
