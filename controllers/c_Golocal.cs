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
    class c_Golocal : BusinessDirectory
    {

        public c_Golocal(Customer customer): base(customer)//second param iiOpenMode
        {
            BusinessDirectory.directoryname = "Golocal";
        }

        public override void Main()
        {
           
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            BusinessDirectory.macroTypeList.Add("update");
            base.Main();
            
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
                case "update": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
            }

        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "entry": base.finalSuccessStatus = "Vielen Dank"; break;
                case "update": base.finalSuccessStatus = "XXXXXXXXXXXXX"; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "-fx";
            switch (macroType)
            {
                case "entry": browsertype = "-fx"; break;
                case "update": browsertype = "-fx"; break;
                default: browsertype = ""; break;
            }

            m_app.iimOpen(browsertype, true, 300);

        }

        protected override void setiiPlayListEntry() {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "GoLocal\\GoLocal.iim");
            processStepsViewList.Add("Eintrag Firmendaten & Kontaktdaten ... ");
            console_menu.SetProcessStepsList(processStepsViewList);
        }

    }
}
