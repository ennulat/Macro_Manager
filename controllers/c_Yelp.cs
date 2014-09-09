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
    class c_Yelp : BusinessDirectory
    {

        public c_Yelp(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Yelp";
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
                case "entry": base.finalSuccessStatus = "Bitte rufen Sie Ihre E-Mails ab um den Vorgang abzuschließen"; break;
                case "registration": base.finalSuccessStatus = "XXXXXXXXXXX"; break;
                case "update": base.finalSuccessStatus = "XXXXXXXXXXXXX"; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
                case "update": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
                case "registration": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
            }

        }
       

        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Yelp\\Yelp_companydata.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");
           
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }
            this.macroParts.Add(Config.macroPath + "Yelp\\Yelp_submition.iim");
            processStepsViewList.Add("Abschluss ... ");
           

            console_menu.SetProcessStepsList(processStepsViewList);
        }



        protected override void setiiOpeningHoursCodePlayEntry()
        {
            string except = "";
            string tmp = "";
            string iiohPlaycodestr = "CODE: ";
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

            if (except != "") {
                iiohPlaycodestr +="PROMPT 1.)folgende<SP>Öffnungszeiten<SP>haben<SP>keine<SP>gemeinsame<SP>Schnittmenge(Splittzeiten)<BR>"
                                + "oder<SP>besitzen<SP>ein<SP>nicht<SP>kompatibles<SP>Format:<BR>"
                                + except.Replace(" ", "<SP>") + "(Nach<SP>Absprache<SP>auslassen<SP>oder<SP>korrigieren)<BR><BR>"+
                                  "2.)Macro<SP>fortführen\nPAUSE\n";
            }
            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times)
        {


            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:Mo.<SP>Di.<SP>Mi.<SP>Do.<SP>Fr.<SP>Sa.<SP>So. CONTENT=%1
            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:00:00<SP>(Mitternacht)<SP>00:30<SP>01:00<SP>01:30<SP>02:00* CONTENT=%9
            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:09:30<SP>10:00<SP>10:30<SP>11:00<SP>11:30<SP>12:00<SP>(Mitta* CONTENT=%13
            //TAG POS=1 TYPE=BUTTON FORM=ID:biz-info-form ATTR=TXT:Zeiten<SP>hinzufügen
            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:Mo.<SP>Di.<SP>Mi.<SP>Do.<SP>Fr.<SP>Sa.<SP>So. CONTENT=%1
            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:00:00<SP>(Mitternacht)<SP>00:30<SP>01:00<SP>01:30<SP>02:00* CONTENT=%14
            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:14:30<SP>15:00<SP>15:30<SP>16:00<SP>16:30<SP>17:00<SP>17:30* CONTENT=%18
            //TAG POS=1 TYPE=BUTTON FORM=ID:biz-info-form ATTR=TXT:Zeiten<SP>hinzufügen

            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:Mo.<SP>Di.<SP>Mi.<SP>Do.<SP>Fr.<SP>Sa.<SP>So. CONTENT=%2
            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:00:00<SP>(Mitternacht)<SP>00:30<SP>01:00<SP>01:30<SP>02:00* CONTENT=%8.5
            //TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:09:00<SP>09:30<SP>10:00<SP>10:30<SP>11:00<SP>11:30<SP>12:00* CONTENT=%12.5
            //TAG POS=1 TYPE=BUTTON FORM=ID:biz-info-form ATTR=TXT:Zeiten<SP>hinzufügen


            string iiohPlaycodestr = "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:Mo.<SP>Di.<SP>Mi.<SP>Do.<SP>Fr.<SP>Sa.<SP>So. CONTENT=%#day#\n" +
                                     "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:00:00<SP>(Mitternacht)<SP>00:30<SP>01:00<SP>01:30<SP>02:00* CONTENT=%#timefrom#\n";

           
            string weekday = "";
            switch (day)
            {
                case "mon": weekday = "0"; break;
                case "tue": weekday = "1"; break;
                case "wen": weekday = "2"; break;
                case "thu": weekday = "3"; break;
                case "fri": weekday = "4"; break;
                case "sat": weekday = "5"; break;
                case "sun": weekday = "6"; break;
            }

            iiohPlaycodestr = iiohPlaycodestr.Replace("#day#", weekday);
            string tmp = "";
            switch (mode)
            {
                case "closed": iiohPlaycodestr = ""; break;
                case "24h":
                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:06:00<SP>06:30<SP>07:00<SP>07:30<SP>08:00<SP>08:30<SP>09:00* CONTENT=%#timeto#\n" +
                                       "TAG POS=1 TYPE=BUTTON FORM=ID:biz-info-form ATTR=TXT:Zeiten<SP>hinzufügen\n";
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#timefrom#", "0").Replace("#timeto#", "0");

                    break;
                case "open":
                    string[] openingtimes = times.Split('-');
                    string[] timespanfrom = openingtimes[0].Split(':');
                    string[] timespanto = openingtimes[1].Split(':');

                    if ((timespanfrom[1].Equals("00") || timespanfrom[1].Equals("30")) && (timespanto[1].Equals("00") || timespanto[1].Equals("30")))
                    {
                        tmp = "";
                        switch (timespanfrom[1]) {
                            case "00": tmp = timespanfrom[0] + ":30"; break;
                            case "30": if (timespanfrom[0].StartsWith("0"))
                                {
                                    int itmp = int.Parse(timespanfrom[0].Substring(1, 1));
                                    itmp = itmp += 1;
                                    tmp = (itmp.ToString().Length > 1) ? itmp.ToString() + ":00" : "0" + itmp.ToString() + ":00";
                                }
                                else {
                                    int itmp = int.Parse(timespanfrom[0]);
                                    itmp = itmp += 1;
                                    tmp = itmp.ToString() + ":00";
                                }
                                break;   
                        }

                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:" + tmp + "* CONTENT=%#timeto#\n" +
                                           "TAG POS=1 TYPE=BUTTON FORM=ID:biz-info-form ATTR=TXT:Zeiten<SP>hinzufügen\n";
                        
                        timespanfrom[0] = (timespanfrom[0].StartsWith("0")) ? timespanfrom[0].Substring(1, 1) : timespanfrom[0];
                        timespanfrom[1] = (timespanfrom[1].StartsWith("0")) ? "" : ".5";
                        timespanto[0] = (timespanto[0].StartsWith("0")) ? timespanto[0].Substring(1, 1) : timespanto[0];
                        timespanto[1] = (timespanto[1].StartsWith("0")) ? "" : ".5";

                        iiohPlaycodestr = iiohPlaycodestr.Replace("#timefrom#", timespanfrom[0] + timespanfrom[1]).Replace("#timeto#", timespanto[0] + timespanto[1]);
                    }
                    else {
                        iiohPlaycodestr = "except*" + day + " (" + times + ")<BR>";
                    }

                    
                    break;
                case "split":

                    string[] splittimes = times.Split('~');
                    string[] fntimefrom = splittimes[0].Split('-')[0].Split(':');
                    string[] fntimeto = splittimes[0].Split('-')[1].Split(':');
                    string[] antimefrom = splittimes[1].Split('-')[0].Split(':');
                    string[] antimeto = splittimes[1].Split('-')[1].Split(':');

                    //check if minutes part in all timespans in right format (00 | 30)
                    if ((fntimefrom[1].Equals("00") || fntimefrom[1].Equals("30")) && 
                        (fntimeto[1].Equals("00") || fntimeto[1].Equals("30")) &&
                        (antimefrom[1].Equals("00") || antimefrom[1].Equals("30")) &&
                        (antimeto[1].Equals("00") || antimeto[1].Equals("30")))
                    {
                        tmp = "";
                        switch (fntimefrom[1])
                        {
                            case "00": tmp = fntimefrom[0] + ":30"; break;
                            case "30": if (fntimefrom[0].StartsWith("0"))
                                {
                                    int itmp = int.Parse(fntimefrom[0].Substring(1, 1));
                                    itmp = itmp += 1;
                                    tmp = (itmp.ToString().Length > 1) ? itmp.ToString() + ":00" : "0" + itmp.ToString() + ":00";
                                }
                                else
                                {
                                    int itmp = int.Parse(fntimefrom[0]);
                                    itmp = itmp += 1;
                                    tmp = itmp.ToString() + ":00";
                                }
                                break;
                        }

                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:" + tmp + "* CONTENT=%#timeto#\n" +
                                           "TAG POS=1 TYPE=BUTTON FORM=ID:biz-info-form ATTR=TXT:Zeiten<SP>hinzufügen\n";


                        tmp = "";
                        switch (antimefrom[1])
                        {
                            case "00": tmp = antimefrom[0] + ":30"; break;
                            case "30": if (antimefrom[0].StartsWith("0"))
                                {
                                    int itmp = int.Parse(antimefrom[0].Substring(1, 1));
                                    itmp = itmp += 1;
                                    tmp = (itmp.ToString().Length > 1) ? itmp.ToString() + ":00" : "0" + itmp.ToString() + ":00";
                                }
                                else
                                {
                                    int itmp = int.Parse(antimefrom[0]);
                                    itmp = itmp += 1;
                                    tmp = itmp.ToString() + ":00";
                                }
                                break;
                        }

                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:Mo.<SP>Di.<SP>Mi.<SP>Do.<SP>Fr.<SP>Sa.<SP>So. CONTENT=%#day#\n" +
                                   "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:00:00<SP>(Mitternacht)<SP>00:30<SP>01:00<SP>01:30<SP>02:00* CONTENT=%#timefrom2#\n" +
                                   "TAG POS=1 TYPE=SELECT FORM=ID:biz-info-form ATTR=TXT:"+ tmp + "* CONTENT=%#timeto2#\n" +
                                   "TAG POS=1 TYPE=BUTTON FORM=ID:biz-info-form ATTR=TXT:Zeiten<SP>hinzufügen\n";


                        
                        
                        //timespans mapping
                        fntimefrom[0] = (fntimefrom[0].StartsWith("0")) ? fntimefrom[0].Substring(1, 1) : fntimefrom[0];
                        fntimefrom[1] = (fntimefrom[1].StartsWith("0")) ? "" : ".5";

                        fntimeto[0] = (fntimeto[0].StartsWith("0")) ? fntimeto[0].Substring(1, 1) : fntimeto[0];
                        fntimeto[1] = (fntimeto[1].StartsWith("0")) ? "" : ".5";

                        antimefrom[0] = (antimefrom[0].StartsWith("0")) ? antimefrom[0].Substring(1, 1) : antimefrom[0];
                        antimefrom[1] = (antimefrom[1].StartsWith("0")) ? "" : ".5";

                        antimeto[0] = (antimeto[0].StartsWith("0")) ? antimeto[0].Substring(1, 1) : antimeto[0];
                        antimeto[1] = (antimeto[1].StartsWith("0")) ? "" : ".5";


                        iiohPlaycodestr = iiohPlaycodestr.Replace("#day#", weekday)
                                                         .Replace("#timefrom#", string.Join("", fntimefrom))
                                                         .Replace("#timeto#", string.Join("", fntimeto))
                                                         .Replace("#timefrom2#", string.Join("", antimefrom))
                                                         .Replace("#timeto2#", string.Join("", antimeto));

                           
                    }
                    else
                    {
                        iiohPlaycodestr = "except*" + day + " (" + times + ")<BR>";
                    }
  
                    
                    break;
            }

            return iiohPlaycodestr;

        }

    }
}
