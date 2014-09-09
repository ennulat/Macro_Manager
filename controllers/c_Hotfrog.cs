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
    class c_Hotfrog : BusinessDirectory
    {

        public c_Hotfrog(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Hotfrog";
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
                case "entry": base.finalSuccessStatus = "Vielen Dank für Ihre Anmeldung auf Hotfrog."; break;
                case "update": base.finalSuccessStatus = "hotfrog.de/Login.aspx"; break;
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

        protected override void setiiPlayListUpdate()
        {
            List<string> processStepsViewList = console_menu.GetProcessStepsList();
            base.macroParts.Add(Config.macroPath + "Hotfrog\\Hotfrog_update.iim");
            processStepsViewList.Add("Firmendatenaktualisierung & Nachtrag Öffnungszeiten/Bilder ... ");

            //if (base.hasOpeningHours)
            //{
            //    this.setiiOpeningHoursCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            //}

            console_menu.SetProcessStepsList(processStepsViewList);
        }


        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Hotfrog\\Hotfrog.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");
            //if (base.hasOpeningHours)
            //{
            //    this.setiiOpeningHoursCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            //}

            //if (base.hasLogo)
            //{
            //    this.setiiLogoCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Firmenlogo ... ");
            //}
            //this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:6\n");
            //processStepsViewList.Add("Transition > Eintrag Öffnungszeiten ... ");
            
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

       

        protected override void setiiLogoCodePlayEntry()
        {

            string iiLogoPlaycodestr = "CODE: TAG POS=1 TYPE=INPUT:FILE FORM=ACTION:/entry/entryWizard?execution=e1s4 ATTR=ID:logo CONTENT=" + Config.picturePath + this.companyPicFolder + "\\" + base.csvVars["logo"] + "\n" +
                                       "TAG POS=1 TYPE=INPUT:SUBMIT FORM=ACTION:/entry/entryWizard?execution=e1s4 ATTR=ID:_eventId_logoUpload\n";
            base.macroParts.Add(iiLogoPlaycodestr);
        }

        protected override void setiiGallerypicsCodePlayEntry()
        {
            string iiGallerypcisPlaycodestr = "CODE: ";

            int i = 7;
            foreach (string pic in base.galleryPics)
            {
                iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=INPUT:FILE FORM=ACTION:/entry/entryWizard?execution=e1s" + i + " ATTR=ID:gallery CONTENT=" + Config.picturePath + base.companyPicFolder + "\\" + pic + "\n";
                iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=INPUT:SUBMIT FORM=ACTION:/entry/entryWizard?execution=e1s" + i + " ATTR=ID:_eventId_galleryUpload\n";
                i++;
            }

            base.macroParts.Add(iiGallerypcisPlaycodestr);

        }


        protected override void setiiOpeningHoursCodePlayEntry()
        {
            //string except = "";
            string tmp = "";
            string iiohPlaycodestr = "CODE: ";
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                tmp = "";
                string day = weekday.Key.ToString();
                string mode = weekday.Value.ToString().Split('|')[0];
                string times = weekday.Value.ToString().Split('|')[1];
                tmp = getiiOpeningHoursWeekdayStringEntry(day, mode, times);
                //if (tmp.Contains("except*"))
                //{
                //    except += tmp.Split('*')[1];
                //    tmp = "";
                //}


                iiohPlaycodestr += tmp;

            }

            //if (except != "")
            //{
            //    iiohPlaycodestr += "PROMPT 1.)folgende<SP>Zeiten<SP>manuell<SP>setzen:<BR>" + except.Replace(" ", "<SP>") + "<BR>" +
            //                        "2.)Macro<SP>fortführen\nPAUSE\n";
            //}
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE={{COL4}}<SP>({{COL7}})<SP>-<SP>DasTelefonbuch<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png";
            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times)
        {
            string iiohPlaycodestr = "";
            switch (day)
            {
                case "mon": iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:Montag\n"; break;
                case "tue": iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:Dienstag\n"; break;
                case "wen": iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:Mittwoch\n"; break;
                case "thu": iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:Donnerstag\n"; break;
                case "fri": iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:Freitag\n"; break;
                case "sat": iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:Samstag\n"; break;
                case "sun": iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:Sonntag\n"; break;
            }

            switch (mode)
            {
                case "closed": break;
                case "24h":
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_from_1 CONTENT=%00:00\n";
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_to_1 CONTENT=%00:00\n";
                    break;
                case "open":
                    string[] openingtimes = times.Split('-');
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_from_1 CONTENT=%" + openingtimes[0] + "\n";
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_to_1 CONTENT=%" + openingtimes[1] + "\n";
                    break;
                case "split":
                    string fntimeFrom = times.Split('~')[0].Split('-')[0];
                    string fntimeTo = times.Split('~')[0].Split('-')[1];
                    string antimeFrom = times.Split('~')[1].Split('-')[0];
                    string antimeTo = times.Split('~')[1].Split('-')[1];
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_from_1 CONTENT=%" + fntimeFrom + "\n";
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_to_1 CONTENT=%" + fntimeTo + "\n";
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_from_2 CONTENT=%" + antimeFrom + "\n";
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ACTION:/entry/entryWizard?execution=e1s6 ATTR=ID:timeselect_to_2 CONTENT=%" + antimeTo + "\n";
                    break;
            }

            return iiohPlaycodestr;

        }

    }
}
