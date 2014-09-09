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
    class c_3Klicks : BusinessDirectory
    {

        public c_3Klicks(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "3Klicks";
        }

        public override void Main()
        {
            
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            base.Main();
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "-fx";
            switch (macroType)
            {
                case "entry": browsertype = "-fx"; break;
                default: browsertype = "-fx"; break;
            }

            m_app.iimOpen(browsertype, true, 300);

        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "entry": base.finalSuccessStatus = "anmeldung10"; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
            }

        }
       

        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "3Klicks\\3Klicks_companydata.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");
           
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }
           
            //base.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:4\n");
            //processStepsViewList.Add("Transition >  Eintrag Kontaktdaten und Abschluss ... ");
            base.macroParts.Add(Config.macroPath + "3Klicks\\3Klicks_submition.iim");
            processStepsViewList.Add("Eintrag Kontaktdaten und Abschluss ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }

       

   
      


        protected override void setiiOpeningHoursCodePlayEntry()
        {

            string tmp = "";
            string iiohPlaycodestr = "CODE: ";
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                tmp = "";
                string day = weekday.Key.ToString();
                string mode = weekday.Value.ToString().Split('|')[0];
                string times = weekday.Value.ToString().Split('|')[1];
                tmp = getiiOpeningHoursWeekdayStringEntry(day, mode, times);
                iiohPlaycodestr += tmp;

            }

            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE={{COL4}}<SP>({{COL7}})<SP>-<SP>3Klicks<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png";
            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times)
        {
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:mo_from CONTENT=9.00-12:00,<SP>13:30-19:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:tu_from CONTENT=test
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:we_from CONTENT=tset
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:th_from CONTENT=9:00-18:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:fr_from CONTENT=24h
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:sa_from CONTENT=geschlossen
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:su_from CONTENT=geschlossen
            //TAG POS=1 TYPE=TABLE ATTR=TXT:Eingabe<SP>Ihrer<SP>Anmeldedaten<SP>Bitte<SP>füllen<SP>Sie*
            //TAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:anmeldung ATTR=NAME:Abschicken

            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:mo_from CONTENT=9.00-12:00,<SP>13:30-19:00
            string iiohPlaycodestr = "TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:anmeldung ATTR=NAME:#weekday#_from CONTENT=#timespan#\n";
            string weekday = "";
            switch (day)
            {
                case "mon": weekday = "mo"; break;
                case "tue": weekday = "tu"; break;
                case "wen": weekday = "we"; break;
                case "thu": weekday = "th"; break;
                case "fri": weekday = "fr"; break;
                case "sat": weekday = "sa"; break;
                case "sun": weekday = "su"; break;
            }

            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday);

            switch (mode)
            {
                case "closed": iiohPlaycodestr = iiohPlaycodestr.Replace("#timespan#", "geschlossen"); break;
                case "24h": iiohPlaycodestr = iiohPlaycodestr.Replace("#timespan#", "24<SP>Stunden<SP>geöffnet"); break;
                case "open": 
                    string[] openingtimes = times.Split('-');
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#timespan#", "von<SP>" + openingtimes[0] + "<SP>bis<SP>" + openingtimes[1]);
                    break;
                case "split":
                    string fntimeFrom = times.Split('~')[0].Split('-')[0];
                    string fntimeTo = times.Split('~')[0].Split('-')[1];
                    string antimeFrom = times.Split('~')[1].Split('-')[0];
                    string antimeTo = times.Split('~')[1].Split('-')[1];
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#timespan#", "Vormittags<SP>von<SP>" + fntimeFrom + "<SP>bis<SP>" + fntimeTo + ",<SP>Nachmittags<SP>von<SP>" + antimeFrom + "<SP>bis<SP>" + antimeTo);
                    break;
            }

            return iiohPlaycodestr;

        }

    }
}
