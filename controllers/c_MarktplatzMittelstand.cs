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
    class c_MarktplatzMittelstand : BusinessDirectory
    {

        public c_MarktplatzMittelstand(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Marktplatz Mittelstand";
        }

        public override void Main()
        {
            
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            BusinessDirectory.macroTypeList.Add("update");
            base.Main();
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

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "entry": base.finalSuccessStatus = "Ihr Firmenprofil ist noch nicht aktiviert."; break;
                //case "entry": base.finalSuccessStatus = "Bitte bestätigen Sie Ihre E-Mail-Adresse"; break;
                case "update": base.finalSuccessStatus = "mein.marktplatz-mittelstand.de/Logout"; break; 
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
                case "update": setiiPlayListUpdate(); break;
            }

        }
       
        protected override void setiiPlayListUpdate(){
            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "MarktplatzMittelstand\\MarktplatzMittelstand_update1.iim");
            processStepsViewList.Add("Update Firmenstammdaten ... ");

            if (base.hasLogo) {
                setiiLogoCodePlayEntry();
                processStepsViewList.Add("Logo hinzufügen/aktualisieren ... ");
            }

            if (base.hasOpeningHours) {
                base.macroParts.Add("CODE: SET !CLIPBOARD " +csvVars["COL22"].ToString().Replace(" ","<SP>") + "\n"+
                                    "TAG POS=14 TYPE=DIV ATTR=TXT:Bearbeiten\n"+
                                    "TAG POS=1 TYPE=TEXTAREA ATTR=ID:pp_t_opening_times CONTENT="+csvVars["COL22"].ToString().Replace(" ","<SP>")+ "\n"+
                                    "TAG POS=13 TYPE=A ATTR=ID:save\n");
                processStepsViewList.Add("Öffnungszeiten hinzufügen/aktualisieren ... ");
            }

            base.macroParts.Add(Config.macroPath + "MarktplatzMittelstand\\MarktplatzMittelstand_update2.iim");
            processStepsViewList.Add("Update Beenden > Logout ... ");

        }


        protected override void setiiLogoCodePlayEntry()
        {
            string iiplaycodestring = "CODE: ";
            //iiplaycodestring += "PROMPT 1.)Logo<SP>manuell<SP>hinzufügen/speichern<BR>2.)Macro<SP>fortführen...\nPAUSE\n";
            iiplaycodestring += "TAG POS=12 TYPE=DIV ATTR=TXT:Bearbeiten\nFRAME F=5\n";
            iiplaycodestring += "TAG POS=1 TYPE=INPUT:FILE ATTR=NAME:pp_t_logo CONTENT=" + Config.picturePath + companyPicFolder + "\\" + csvVars["logo"].ToString() + "\n";
          
            iiplaycodestring += "TAG POS=1 TYPE=INPUT:SUBMIT FORM=ID:frameMaster ATTR=ID:save\n";
            base.macroParts.Add(iiplaycodestring);
            
            base.setiiLogoCodePlayEntry();
        }


        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "MarktplatzMittelstand\\MarktplatzMittelstand_eintragung.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");
            //if (base.hasLogo)
            //{
            //    this.setiiLogoCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Firmenlogo ... ");
            //}
            //this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:6\n");
            //processStepsViewList.Add("Transition > Eintrag Öffnungszeiten ... ");
            //if (base.hasOpeningHours)
            //{
            //    this.setiiOpeningHoursCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            //}
            //this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:32\n");
            //processStepsViewList.Add("Transition > Eintrag Gallerybilder ... ");
            //if (base.hasGallerypics)
            //{
            //    this.setiiGallerypicsCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Gallerybilder ... ");
            //}
            //base.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:4\n");
            //processStepsViewList.Add("Transition >  Eintrag Kontaktdaten und Abschluss ... ");
            //base.macroParts.Add(Config.macroPath + "DasTelefonbuch\\DasTelefonbuch_desc_contactdata.iim");
            //processStepsViewList.Add("Eintrag Kontaktdaten und Abschluss ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }

  
    }
}
