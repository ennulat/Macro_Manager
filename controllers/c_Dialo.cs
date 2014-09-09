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
    class c_Dialo: BusinessDirectory
    {

        public c_Dialo(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Dialo";
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
                case "entry": base.finalSuccessStatus = "Vielen Dank für Ihren Eintrag."; break;
                case "update": base.finalSuccessStatus = "XXXXXXXXXXXXXXXXX"; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "entry": setiiPlayListEntry(); break;
                case "update": Console.WriteLine("noch nicht implementiert, mit Firefox direkt ausführen."); Console.ReadLine(); break;

            }

        }
       

        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Dialo\\Dialo_entry1.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");
         
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }

            base.macroParts.Add(Config.macroPath + "Dialo\\Dialo_entry2.iim");
            processStepsViewList.Add("Abschluss ... ");
           
            console_menu.SetProcessStepsList(processStepsViewList);
        }



        protected override void setiiOpeningHoursCodePlayEntry()
        {
            //'PROMPT 1.)wenn<SP>öffnungszeiten<SP>gegeben<SP>auf<SP>Annahmen<SP>prüfen<SP>/korrigieren!<BR>2.)macro<SP>fortführen..
            //'PAUSE
            //'Montag - Donnerstag: 12:00-13:00 Uhr
            //'<br></br>
            //'Freitag - Samstag: 10:00-12:00 Uhr, 13:30-18:00 Uh…
            //'<br></br>
            //'Sonntag: geschlossen
            //TAG POS=1 TYPE=TEXTAREA FORM=ID:companyToConfirmForm ATTR=ID:openingHoursAjax CONTENT={{COL22}}


            string iiohPlaycodestr = "";
            //KeyValuePair<string,string>

            Dictionary<string,string> weekdays2time = new Dictionary<string,string>();

            //first run
            string currentTimedetail = "";
            string tmp = "";
            string tmpEQdays = "";
            string currentCompareDay = "mon";
            bool sameWeektimes = false;
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                currentTimedetail = base.weekdays2timedetails[currentCompareDay].ToString();
                string day = weekday.Key.ToString();
                string mode = weekday.Value.ToString().Split('|')[0];
                string times = weekday.Value.ToString().Split('|')[1];

                //define times
                switch (mode) {
                    case "closed": times = "geschlossen"; break;
                    case "24h": times = "00:00-00:00 Uhr"; break;
                    case "split": times = times.Split('~')[0] + " Uhr" + ", " + times.Split('~')[1] + " Uhr"; break;
                    case "open": times = times + " Uhr"; break;
                }


                if (currentTimedetail == weekday.Value.ToString())
                {
                    sameWeektimes = true;
                    if (day.Equals(currentCompareDay))
                        tmpEQdays = currentCompareDay + ": " + times + "<br></br>";
                    else
                        tmpEQdays = currentCompareDay + "- " + day + ": " + times + "<br></br>";
                }
                else
                {
                    if (sameWeektimes) { //set last eqdays
                        if(tmp != "" && (tmp.Substring(0,2).Equals(tmpEQdays.Substring(0,2)))){
                            iiohPlaycodestr = iiohPlaycodestr.Replace(tmp,"");
                        }
                        iiohPlaycodestr += tmpEQdays;
                        tmpEQdays = "";
                        sameWeektimes = false;
                    }
                    
                    currentCompareDay = day;
                    tmp = day + ": " + times + "<br></br>";
                    iiohPlaycodestr += tmp;
                }

                if(day.Equals("sun") && tmpEQdays != ""){
                    if (tmp != "" && (tmp.Substring(0, 2).Equals(tmpEQdays.Substring(0, 2))))
                    {
                        iiohPlaycodestr = iiohPlaycodestr.Replace(tmp, "");
                    }
                    iiohPlaycodestr += tmpEQdays;
                }
                

            }


            iiohPlaycodestr = iiohPlaycodestr.Replace("mon", "Montag")
                                             .Replace("tue", "Dienstag")
                                             .Replace("wen", "Mittwoch")
                                             .Replace("thu", "Donnerstag")
                                             .Replace("fri", "Freitag")
                                             .Replace("sat", "Samstag")
                                             .Replace("sun", "Sonntag");

            iiohPlaycodestr = "CODE: TAG POS=1 TYPE=TEXTAREA FORM=ID:companyToConfirmForm ATTR=ID:openingHoursAjax CONTENT=" + iiohPlaycodestr.Replace(" ", "<SP>") + "\n" +
                              "PROMPT 1.)Prüfen<SP>ob<SP>Öffnungszeichen<SP>korrekt<SP>sind.<SP>ÖZ<SP>bestägigen<SP>('FOCUS'<SP>in<SP>ÖZ-TEXTFELD<SP>setzen<SP>&<SP>mit<SP>'ENTER'<SP>bestätigen!<BR>2.)Macro<SP>fortführen...\nPAUSE\n";
            
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE={{COL4}}<SP>({{COL7}})<SP>-<SP>Dialo_entry<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png\n";
            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

       

    }
}
