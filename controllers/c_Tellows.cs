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
    class c_Tellows : BusinessDirectory
    {
        
        public c_Tellows(Customer customer): base(customer)//second param iiOpenMode
        {
            BusinessDirectory.directoryname = "Tellows";
            
        }

        public override void Main()
        {
           
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            base.Main();
            
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
            }

        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "entry": base.finalSuccessStatus = "Vielen Dank! Deine Eingabe wurde aufgenommen."; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "-fx";
            switch (macroType)
            {
                case "entry": browsertype = "-fx"; break;
                default: browsertype = ""; break;
            }

            m_app.iimOpen(browsertype, true, 300);

        }

        protected override void setiiPlayListEntry() {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Tellows\\Tellows.iim");
            processStepsViewList.Add("Eintrag Firmendaten & Kontaktdaten ... ");
            console_menu.SetProcessStepsList(processStepsViewList);
        }

    }
}
