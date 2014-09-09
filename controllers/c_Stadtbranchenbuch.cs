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
    class c_Stadtbranchenbuch : BusinessDirectory
    {

        public c_Stadtbranchenbuch(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Stadtbranchenbuch";
        }

        public override void Main() {

            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            base.Main();
        
        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "entry": base.finalSuccessStatus = "Vielen Dank"; break;
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


        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times)
        {
            string iiohPlaycodestr = "";
            try
            {

                string ohType = "open";
                if (Logger.Iiplaycode != null && Logger.Iiplaycode.Contains("twovalbtn CONTENT=YES"))//split times available?
                {
                    ohType = "split";
                }

                string weekday = "";
                switch (ohType)
                {
                    /****OPEN*****/
                    case "open":
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status1#weekday# CONTENT=%#status#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:#dayfrom# CONTENT=%#timefrom#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:#dayto# CONTENT=%#timeto#\n";


                        switch (day)
                        {
                            case "mon": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "mo")
                                                                         .Replace("#dayfrom#", "von1mo")
                                                                         .Replace("#dayto#", "bis1mo");
                                weekday = "mo";
                                break;
                            case "tue": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "di")
                                                                         .Replace("#dayfrom#", "von1di")
                                                                         .Replace("#dayto#", "bis1di");
                                weekday = "di";
                                break;
                            case "wen": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "mi")
                                                                         .Replace("#dayfrom#", "von1mi")
                                                                         .Replace("#dayto#", "bis1mi");
                                weekday = "mi";
                                break;
                            case "thu": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "do")
                                                                         .Replace("#dayfrom#", "von1do")
                                                                         .Replace("#dayto#", "bis1do");
                                weekday = "do";
                                break;
                            case "fri": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "fr")
                                                                         .Replace("#dayfrom#", "von1fr")
                                                                         .Replace("#dayto#", "bis1fr");
                                weekday = "fr";
                                break;
                            case "sat": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "sa")
                                                                         .Replace("#dayfrom#", "von1sa")
                                                                         .Replace("#dayto#", "bis1sa");
                                weekday = "sa";
                                break;
                            case "sun": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "so")
                                                                         .Replace("#dayfrom#", "von1so")
                                                                         .Replace("#dayto#", "bis1so");
                                weekday = "so";
                                break;
                        }

                        switch (mode)
                        {
                            case "closed":
                                iiohPlaycodestr = "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status1#weekday# CONTENT=%#status#\n";
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday)
                                                                 .Replace("#status#", "geschlossen");
                                break;
                            case "24h":
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday)
                                                                 .Replace("#timefrom#", "00:00")
                                                                 .Replace("#timeto#", "00:00")
                                                                 .Replace("#status#", "offen");
                                break;
                            case "open":
                                string[] openingtimes = times.Split('-');
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday)
                                                                 .Replace("#timefrom#", openingtimes[0])
                                                                 .Replace("#timeto#", openingtimes[1])
                                                                 .Replace("#status#", "offen");
                                break;
                        }
                        break;

                    /****SPLIT*****/
                    case "split":
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status1#weekday# CONTENT=%#status#\n";
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status2#weekday# CONTENT=%#status#\n";
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:#dayfrom1# CONTENT=%#timefrom1#\n";
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:#dayto1# CONTENT=%#timeto1#\n";
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:#dayfrom2# CONTENT=%#timefrom2#\n";
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:#dayto2# CONTENT=%#timeto2#\n";

                        switch (day)
                        {
                            case "mon": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "mo")
                                                                         .Replace("#dayfrom1#", "von1mo")
                                                                         .Replace("#dayto1#", "bis1mo")
                                                                         .Replace("#dayfrom2#", "von2mo")
                                                                         .Replace("#dayto2#", "bis2mo");
                                weekday = "mo";
                                break;
                            case "tue": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "di")
                                                                         .Replace("#dayfrom1#", "von1di")
                                                                         .Replace("#dayto1#", "bis1di")
                                                                         .Replace("#dayfrom2#", "von2di")
                                                                         .Replace("#dayto2#", "bis2di");
                                weekday = "di";
                                break;
                            case "wen": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "mi")
                                                                         .Replace("#dayfrom1#", "von1mi")
                                                                         .Replace("#dayto1#", "bis1mi")
                                                                         .Replace("#dayfrom2#", "von2mi")
                                                                         .Replace("#dayto2#", "bis2mi");
                                weekday = "mi";
                                break;
                            case "thu": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "do")
                                                                         .Replace("#dayfrom1#", "von1do")
                                                                         .Replace("#dayto1#", "bis1do")
                                                                         .Replace("#dayfrom2#", "von2do")
                                                                         .Replace("#dayto2#", "bis2do");
                                weekday = "do";
                                break;
                            case "fri": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "fr")
                                                                         .Replace("#dayfrom1#", "von1fr")
                                                                         .Replace("#dayto1#", "bis1fr")
                                                                         .Replace("#dayfrom2#", "von2fr")
                                                                         .Replace("#dayto2#", "bis2fr");
                                weekday = "fr";
                                break;
                            case "sat": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "sa")
                                                                         .Replace("#dayfrom1#", "von1sa")
                                                                         .Replace("#dayto1#", "bis1sa")
                                                                         .Replace("#dayfrom2#", "von2sa")
                                                                         .Replace("#dayto2#", "bis2sa");
                                weekday = "sa";
                                break;
                            case "sun": iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", "so")
                                                                         .Replace("#dayfrom1#", "von1so")
                                                                         .Replace("#dayto1#", "bis1so")
                                                                         .Replace("#dayfrom2#", "von2so")
                                                                         .Replace("#dayto2#", "bis2so");
                                weekday = "so";
                                break;
                        }

                        switch (mode)
                        {
                            case "closed":
                                iiohPlaycodestr = "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status1#weekday# CONTENT=%#status#\n" +
                                                   "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status2#weekday# CONTENT=%#status#\n";
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday)
                                                                 .Replace("#status#", "geschlossen");
                                break;
                            case "24h":
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#timefrom1#", "00:00")
                                                                 .Replace("#timeto1#", "12:00")
                                                                 .Replace("#timefrom2#", "12:00")
                                                                 .Replace("#timeto2#", "00:00")
                                                                 .Replace("#status#", "offen");
                                break;
                            case "open": 
                                string[] openingtimes = times.Split('-');
                                iiohPlaycodestr = "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status1#weekday# CONTENT=%offen\n"
                                                + "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:status2#weekday# CONTENT=%geschlossen\n"
                                                + "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:von1#weekday# CONTENT=%#timefrom1#\n"
                                                + "TAG POS=1 TYPE=SELECT FORM=ID:step1 ATTR=ID:bis1#weekday# CONTENT=%#timeto1#\n";
                                iiohPlaycodestr = iiohPlaycodestr .Replace("#weekday#", weekday)
                                                                  .Replace("#timefrom1#", openingtimes[0])
                                                                  .Replace("#timeto1#", openingtimes[1]);
                                break;
                            case "split":
                                string[] splittimes = times.Split('~');
                                string fntimeFrom = times.Split('~')[0].Split('-')[0];
                                string fntimeTo = times.Split('~')[0].Split('-')[1];
                                string antimeFrom = times.Split('~')[1].Split('-')[0];
                                string antimeTo = times.Split('~')[1].Split('-')[1];
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#timefrom1#", fntimeFrom)
                                                                 .Replace("#timeto1#", fntimeTo)
                                                                 .Replace("#timefrom2#", antimeFrom)
                                                                 .Replace("#timeto2#", antimeTo)
                                                                 .Replace("#status#", "offen");
                                break;
                        }

                        break;


                }
            }
            catch (Exception e) {
                iiohPlaycodestr = "except*" + day + " (" + times + ")<BR>";
            }
            
            return iiohPlaycodestr;

        }

      

       

        protected override void setiiOpeningHoursCodePlayEntry(ref List<string> processStepsViewList)
        {
            processStepsViewList.Add("Eintrag Öffnungszeiten");
            string except = "";
            string tmp = "";
            string iiohPlaycodestr = "CODE: TAG POS=1 TYPE=INPUT:RADIO FORM=ID:step1 ATTR=ID:noOhB\n";

            //check if splittimes available
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                if (weekday.Value.ToString().Contains("~")) {
                    iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:step1 ATTR=ID:twovalbtn CONTENT=YES\n";
                    Logger.Iiplaycode = iiohPlaycodestr;
                    break;
                }
            }
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                tmp = "";
                string day = weekday.Key.ToString();
                string mode = weekday.Value.ToString().Split('|')[0];
                string times = weekday.Value.ToString().Split('|')[1];

               
                tmp = getiiOpeningHoursWeekdayStringEntry(day, mode, times);
                if (tmp.Contains("except*"))
                {
                    except += tmp.Split('*')[1];
                    tmp = "";
                }


                iiohPlaycodestr += tmp;

            }

            if (except != "")
            {
                iiohPlaycodestr += "PROMPT 1.)folgende<SP>Zeiten<SP>manuell<SP>setzen:<BR>" + except.Replace(" ", "<SP>") + "<BR>" +
                                    "2.)Macro<SP>fortführen\nPAUSE\n";
            }
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE=" + csvVars["COL4"].ToString().Replace(" ", "<SP>") + "<SP>(" + csvVars["COL7"].ToString().Replace(" ", "<SP>") + ")<SP>-<SP>Stadtbranchenbuch<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png";

            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
                case "update": Console.WriteLine("noch nicht implementiert."); Console.ReadLine(); break;
                case "register": Console.WriteLine("noch nicht implementiert."); Console.ReadLine(); break;
                case "search": Console.WriteLine("noch nicht implementiert."); Console.ReadLine(); break;
            }

        }

        protected override void setiiPlayListEntry() {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Stadtbranchenbuch\\Stadtbranchenbuch_companydata.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");

            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry(ref processStepsViewList);
            }
            else {
                processStepsViewList.Add("Eintrag Öffnungszeiten wird übersprungen");
                base.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:RADIO FORM=ID:step1 ATTR=ID:noOhA");
            }

           
            base.macroParts.Add(Config.macroPath + "Stadtbranchenbuch\\Stadtbranchenbuch_submition.iim");
            processStepsViewList.Add("Eintragung abschließen ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }


    }
}
