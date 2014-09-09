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
using System.Text.RegularExpressions;

namespace Macro_Manager.controllers
{
    class c_Quicker : BusinessDirectory
    {
        
        public c_Quicker(Customer customer): base(customer)//second param iiOpenMode
        {
            BusinessDirectory.directoryname = "Quicker";

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
                case "entry": base.finalSuccessStatus = "bei Quicker angemeldet"; break;
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

       


        protected override void setiiLogoCodePlayEntry()
        {
            //TAG POS=1 TYPE=INPUT:FILE ATTR=NAME:picture CONTENT=C:\Users\Public\Pictures\Sample<SP>Pictures\Jellyfish.jpg
            //string iiLogoPlaycodestr = "CODE: TAG POS=1 TYPE=INPUT:FILE ATTR=NAME:picture CONTENT=" + Config.picturePath  + this.companyPicFolder + "\\" + base.csvVars["logo"] + "\n";  
            string iiLogoPlaycodestr = "CODE: PROMPT 1.)ggf.<SP>1<SP>Logo/Galleriebild<SP>hochladen...<BR>Macro<SP>fortführen...\nPAUSE";
            //SIZE X=1579 Y=884
            //DS CMD=CLICK X=710 Y=325 CONTENT=
            //SIZE X=1579 Y=884
            //SIZE X=1579 Y=884
            //string iiLogoPlaycodestr = "CODE: SET !SINGLESTEP YES\nSIZE X=1579 Y=884\nDS CMD=CLICK X=710 Y=325 CONTENT=" + Config.picturePath  + "pic_folder" + "\\" + base.csvVars["logo"]+"\nPROMPT check\nPAUSE";           
            base.macroParts.Add(iiLogoPlaycodestr);
        }

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times) {


            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[1] CONTENT=10:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[1] CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[2] CONTENT=10:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[2] CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[3] CONTENT=10:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[3] CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[4] CONTENT=10:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[4] CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[5] CONTENT=10:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[5] CONTENT=12:00

            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[6] CONTENT=YES
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[7] CONTENT=YES
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[6] CONTENT=10:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[6] CONTENT=1200
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[7] CONTENT=10:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[7] CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[1] CONTENT=NO
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[2] CONTENT=NO
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[3] CONTENT=NO
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[4] CONTENT=NO
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[5] CONTENT=NO



            string iiohPlaycodestr = "";
            switch(day){
                case "mon": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[1] CONTENT=#status#\n"; break;
                case "tue": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[2] CONTENT=#status#\n"; break;
                case "wen": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[3] CONTENT=#status#\n"; break;
                case "thu": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[4] CONTENT=#status#\n"; break;
                case "fri": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[5] CONTENT=#status#\n"; break;
                case "sat": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[6] CONTENT=#status#\n"; break;
                case "sun": iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=ID:anlegenform ATTR=NAME:day[7] CONTENT=#status#\n"; break;
            }

            string key = "";
            string pattern = @"day\[(\d)\]";
            string timefrom = "TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:begin[#key#] CONTENT=#from#\n";
            string timeto = "TAG POS=1 TYPE=INPUT:TEXT FORM=ID:anlegenform ATTR=NAME:end[#key#] CONTENT=#to#\n";
            Regex _regex = new Regex(pattern);
            Match match;
            switch (mode) {
                case "closed": iiohPlaycodestr = iiohPlaycodestr.Replace("#status#", "NO"); break;
                case "24h": iiohPlaycodestr = iiohPlaycodestr.Replace("#status#", "YES");
                            match = _regex.Match(iiohPlaycodestr);
                            key = match.Groups[1].Value;
                            iiohPlaycodestr += timefrom.Replace("#key#", key).Replace("#from#", "00:00");
                            iiohPlaycodestr += timeto.Replace("#key#", key).Replace("#to#", "00:00");
                            break;
                case "open": iiohPlaycodestr = iiohPlaycodestr.Replace("#status#", "YES"); 
                             string[] openingtimes = times.Split('-');
                             match = _regex.Match(iiohPlaycodestr);
                             key = match.Groups[1].Value;
                             iiohPlaycodestr += timefrom.Replace("#key#", key).Replace("#from#", openingtimes[0]);
                             iiohPlaycodestr += timeto.Replace("#key#", key).Replace("#to#", openingtimes[1]);
                             break;                       
                case "split": iiohPlaycodestr = iiohPlaycodestr.Replace("#status#", "YES");
                              string fntimeFrom = times.Split('~')[0].Split('-')[0];
                              string fntimeTo = times.Split('~')[0].Split('-')[1];
                              string antimeFrom = times.Split('~')[1].Split('-')[0];
                              string antimeTo = times.Split('~')[1].Split('-')[1];

                              if (fntimeTo.Equals(antimeFrom))
                              {
                                  match = _regex.Match(iiohPlaycodestr);
                                  key = match.Groups[1].Value;
                                  iiohPlaycodestr += timefrom.Replace("#key#", key).Replace("#from#", fntimeFrom);
                                  iiohPlaycodestr += timeto.Replace("#key#", key).Replace("#to#", antimeTo);
                              }
                              else {
                                  iiohPlaycodestr = "except*" + day + " (" + times + ")<BR>";
                              
                              }
                              break;                       
            }
            return iiohPlaycodestr;
        
        }

        protected override void setiiOpeningHoursCodePlayEntry() {

            string except = "";
            string tmp = "";
            string iiohPlaycodestr = "CODE: ";
            //iiohPlaycodestr += "SET !SINGLESTEP YES\n";


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
            else
            {

                iiohPlaycodestr += "PROMPT 1.)Öffnungszeiten<SP>prüfen<BR>2.)Macro<SP>fortführen...\nPAUSE\n";
            }
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE={{COL4}}<SP>({{COL7}})<SP>-<SP>Quicker<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png";
            base.macroParts.Add(iiohPlaycodestr);
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

            base.macroParts.Add(Config.macroPath + "Quicker\\Quicker_companydata_contactdata.iim");
            processStepsViewList.Add("Eintrag Firmendaten & Kontaktdaten... ");
            if (base.hasLogo || base.hasGallerypics)
            {
                this.setiiLogoCodePlayEntry();
                processStepsViewList.Add("Eintrag Firmenlogo/Gallerybild ... ");
            }
           
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }

            base.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:NoFormName ATTR=VALUE:Weiter\n"+
                                "TAG POS=1 TYPE=TEXTAREA ATTR=ID:sMessage EXTRACT=TXT ");
            processStepsViewList.Add("Transition > Abschluss");
          
            //base.macroParts.Add(Config.macroPath + "Quicker\\Quicker_contactdata.iim");
            //processStepsViewList.Add("Eintrag Kontaktdaten und Abschluss ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }

    }
}
