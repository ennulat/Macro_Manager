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
    class c_GewusstWo : BusinessDirectory
    {

        public c_GewusstWo(Customer customer): base(customer)//second param iiOpenMode
        {
            BusinessDirectory.directoryname = "GewusstWo";
        }

        public override void Main()
        {
       
   
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            BusinessDirectory.macroTypeList.Add("update");
            base.Main();
        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "entry": base.finalSuccessStatus = "Ihre Nachricht wird weitergeleitet"; break;
                case "update": base.finalSuccessStatus = "XXXXXXXXXXXXXXXXX"; break;
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

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times) {

            //TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_mo_fr_from CONTENT={{_ot_mo_fr_from}}
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_mo_fr_to CONTENT={{_ot_mo_fr_to}}
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_sa_from CONTENT={{_ot_sa_from}}
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_sa_to CONTENT={{_ot_sa_to}}
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_so_from CONTENT={{_ot_so_from}}
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_so_to CONTENT={{_ot_so_to}}


            string iiohPlaycodestr = "";

            string timefrom = "", timeto = "";
            switch (mode) {
                case "closed": break;
                case "24h": 
                    timefrom = "00:00";
                    timeto = "00:00";
                    break;
                case "open":
                    string[] openingtimes = times.Split('-');
                    timefrom = openingtimes[0];
                    timeto = openingtimes[1];
                    break;
                case "split":
                    string fntimeFrom = times.Split('~')[0].Split('-')[0];
                    timefrom = fntimeFrom;
                    string fntimeTo = times.Split('~')[0].Split('-')[1];
                    string antimeFrom = times.Split('~')[1].Split('-')[0];
                    string antimeTo = times.Split('~')[1].Split('-')[1];

                    if (fntimeTo.Equals(antimeFrom))
                        timeto = antimeTo;
                    else {
                        return "except*" + day + ":<SP>" + times+"<BR>";
                    }
                    break;
            }

            switch (day)
            {
                case "mon": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_mo_fr_from CONTENT="+timefrom+"\n";
                            iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_mo_fr_to CONTENT="+timeto+"\n";
                            break;
                case "sat": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_sa_from CONTENT=" + timefrom + "\n";
                            iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_sa_to CONTENT=" + timeto + "\n";
                            break;
                case "sun": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_so_from CONTENT=" + timefrom + "\n";
                            iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=ACTION:/ ATTR=NAME:_ot_so_to CONTENT=" + timeto + "\n";
                            break;
            }

            return iiohPlaycodestr;
        
        }

        protected override void setiiOpeningHoursCodePlayEntry(ref List<string> processStepsViewList)
        {

            string timecomparestr = ""; //represent mo-fr times
            string iiohPlaycodestr;
            string ohtimesmessage="";
            string except = "";
            string day, mode, times, timedetails, tmp;
            bool workdaysAllEqual = true;

            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                if(timecomparestr.Equals("") && weekday.Key.ToString().Equals("mon"))
                    timecomparestr += weekday.Value.ToString().Split('|')[1];
                ohtimesmessage += weekday.Key.ToString() + ":<SP>" + weekday.Value.ToString() + "<BR>";
            }

            iiohPlaycodestr = "CODE: ";
            int i=1;
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                day = weekday.Key.ToString();
                mode = weekday.Value.ToString().Split('|')[0];
                times = weekday.Value.ToString().Split('|')[1];
                timedetails = weekday.Value.ToString();
                tmp = "";

                if (i < 6)//pass workdays
                {
                    if (!timecomparestr.Equals(times) && except.Equals(""))
                    {
                        except += (except.Equals("")) ? "PROMPT " : "";
                        except += "WERKTAGS(Mo-Fr)<SP>Öffnungszeiten<SP>sind<SP>abweichend!<BR>manuell<SP>prüfen:<BR>" + ohtimesmessage + "<BR>" +
                                  "ACHTUNG:<SP>WOCHEND<SP>ÖZ<SP>Angaben<SP>sind<SP>nur<SP>zusammen<SP>mit<SP>gegebenen<SP>WERKTAGS<SP>ÖZ<SP>gültig!<BR><BR>";
                                
                        except = except.Replace("|", "<SP>");
                        workdaysAllEqual = false;
                    }

                    
                    if(i.Equals(5) && workdaysAllEqual)//set timespan mo-fr
                        tmp = getiiOpeningHoursWeekdayStringEntry("mon", mode, timecomparestr);


                }
                else//pass: weekenddays
                    tmp= getiiOpeningHoursWeekdayStringEntry(day, mode, times);

                if (tmp.Contains("except"))
                {
                    except += (except.Equals("")) ? "PROMPT " : "";
                    except += tmp.Split('*')[1] + "Splitzeit<SP>hat<SP>keine<SP>gemeinsame<SP>Schnittmenge!<BR><BR>";
                }
                else
                    iiohPlaycodestr += tmp;
        
                i++;
            }

            if (except != "") {
                
                except += "2.)Macro<SP>fortführen...\nPAUSE\n";
                iiohPlaycodestr += except;
            }

            base.macroParts.Add(iiohPlaycodestr);
            processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            
            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
                case "update": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;
            }

        }

        protected override void setiiPlayListEntry() {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "GewusstWo\\GewusstWo_companydata.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry(ref processStepsViewList);
            }

            base.macroParts.Add(Config.macroPath + "GewusstWo\\GewusstWo_submition.iim");
            processStepsViewList.Add("Eintragung abschließen ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }

    }
}
