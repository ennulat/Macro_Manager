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
    class c_Meinestadt : BusinessDirectory
    {

        public c_Meinestadt(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Meinestadt";
        }

        public override void Main()
        {
            
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            BusinessDirectory.macroTypeList.Add("registration");
            BusinessDirectory.macroTypeList.Add("update");
            base.Main();
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "-fx";
            switch (macroType)
            {
                case "entry": browsertype = "-fx"; break;
                case "registration": browsertype = "-fx"; break;
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
                case "entry": base.finalSuccessStatus = "Vielen Dank für die Schaltung Ihres Gratis-Profils"; break;
                case "registration": base.finalSuccessStatus = "XXXXXXX"; break;
                case "update": base.finalSuccessStatus = "XXXXXXX"; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
                case "update": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
                case "register": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
                case "search": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
            }

        }
       

        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Meinestadt\\MeineStadt_companydata.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");
           
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }
           
            base.macroParts.Add(Config.macroPath + "Meinestadt\\MeineStadt_contactdata.iim");
            processStepsViewList.Add("Eintrag Kontaktdaten und Abschluss ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }

       


        protected override void setiiOpeningHoursCodePlayEntry()
        {
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:moVonV CONTENT=09:30
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:moBisV CONTENT=11:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:moVonN CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:moBisN CONTENT=14:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:diVonV CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:diBisV CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:diVonN CONTENT=13:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:diBisN CONTENT=17:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:miVonV CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:miBisV CONTENT=13:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:miVonN CONTENT=14:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:miBisN CONTENT=17:00
            //TAG POS=1 TYPE=DIV ATTR=TXT:Freitag<SP>von<SP>bis<SP>von<SP>bis
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:doVonV CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:doBisV CONTENT=13:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:doVonN CONTENT=14:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:doBisN CONTENT=17:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:frVonV CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:frBisV CONTENT=13:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:frVonN CONTENT=14:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:frBisN CONTENT=17:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:saVonV CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:saBisV CONTENT=13:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:saVonN CONTENT=15:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:saBisN CONTENT=18:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:soVonV CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:soBisV CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:soVonN CONTENT=14:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:soBisN CONTENT=17:00
            
            
            
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
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE={{COL4}}<SP>({{COL7}})<SP>-<SP>Meinestadt<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png";
            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times)
        {
            string iiohPlaycodestr = "TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:#weekday#VonV CONTENT=#timefrom#\n" +
                                     "TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:#weekday#BisV CONTENT=#timeto#\n";
            string weekday = "";
            switch (day)
            {
                case "mon": weekday = "mo";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday) ; break;
                case "tue": weekday = "di";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday); break;
                case "wen": weekday = "mi";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday); break;
                case "thu": weekday = "do";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday); break;
                case "fri": weekday = "fr";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday); break;
                case "sat": weekday = "sa";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday); break;
                case "sun": weekday = "so";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday); break;
            }

            switch (mode)
            {
                case "closed": iiohPlaycodestr = ""; break;
                case "24h":
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#timefrom#", "00:00")
                                                     .Replace("#timeto#", "00:00");
                    break;
                case "open":
                    string[] openingtimes = times.Split('-');
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#timefrom#", openingtimes[0])
                                                     .Replace("#timeto#", openingtimes[1]);
                    break;
                case "split":
                    string fntimeFrom = times.Split('~')[0].Split('-')[0];
                    string fntimeTo = times.Split('~')[0].Split('-')[1];
                    string antimeFrom = times.Split('~')[1].Split('-')[0];
                    string antimeTo = times.Split('~')[1].Split('-')[1];
         
                    iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:#weekday#VonN CONTENT=#timefrom2#\n"+
                                       "TAG POS=1 TYPE=INPUT:TEXT FORM=ID:myform ATTR=ID:#weekday#BisN CONTENT=#timeto2#\n";
                    iiohPlaycodestr =  iiohPlaycodestr.Replace("#weekday#", weekday)
                                                      .Replace("#timefrom#", fntimeFrom)
                                                      .Replace("#timeto#", fntimeTo)
                                                      .Replace("#timefrom2#", antimeFrom)
                                                      .Replace("#timeto2#", antimeTo);
                    break;
            }

            return iiohPlaycodestr;

        }

    }
}
