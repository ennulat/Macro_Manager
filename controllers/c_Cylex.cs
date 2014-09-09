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
    class c_Cylex : BusinessDirectory
    {

        public c_Cylex(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Cylex";
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
                case "entry": browsertype = ""; break;
                case "update": browsertype = ""; break;
                default: browsertype = ""; break;
            }

            m_app.iimOpen(browsertype, true, 300);

        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                case "entry": base.finalSuccessStatus = "Auftrag erhalten"; break;
                case "update": base.finalSuccessStatus = "XXXXXXXX"; break;
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
       

        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Cylex\\Cylex.iim");
            processStepsViewList.Add("Eintrag Firmen & Kontaktdaten ... ");

            //if (base.hasLogo)
            //{
            //    this.setiiLogoCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Firmenlogo ... ");
            //}
            //this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:6\n");
            //processStepsViewList.Add("Transition > Eintrag Öffnungszeiten ... ");
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }
            //this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:32\n");
            //processStepsViewList.Add("Transition > Eintrag Gallerybilder ... ");
            if (base.hasGallerypics)
            {
                this.setiiGallerypicsCodePlayEntry();
                processStepsViewList.Add("Eintrag Gallerybilder ... ");
            }
            //base.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:4\n");
            //processStepsViewList.Add("Transition >  Eintrag Kontaktdaten und Abschluss ... ");
            //base.macroParts.Add(Config.macroPath + "DasTelefonbuch\\DasTelefonbuch_desc_contactdata.iim");
            //processStepsViewList.Add("Eintrag Kontaktdaten und Abschluss ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }

        protected override void setiiLogoCodePlayEntry()
        {

            //string iiLogoPlaycodestr = "";
            //base.macroParts.Add(iiLogoPlaycodestr);
        }

        protected override void setiiGallerypicsCodePlayEntry()
        {
            string iiGallerypcisPlaycodestr = "CODE: ";

            //TAG POS=1 TYPE=SPAN FORM=NAME:aspnetForm ATTR=TXT:Firmenprofil<SP>teilweise<SP>ausgefüllt
            //TAG POS=1 TYPE=A FORM=NAME:aspnetForm ATTR=TXT:Bilder<SP>hochladen<SP>Laden<SP>Sie<SP>Bilder<SP>und<SP>Fotos<SP>Ihres<SP>Unternehmens,<SP>Produkte<SP>oder<SP>D*
            //TAG POS=1 TYPE=A FORM=NAME:aspnetForm ATTR=TXT:Firmenfotos<SP>hochladen
            //TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$multimedia$tb_title CONTENT=Galeriebilder(Titel)
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$multimedia$FileUpload1 CONTENT=c:\Users\Omnea0001\Documents\iMacros\Datasources\Bilder\test_company_x\gallery2_Jellyfish.jpg
            //TAG POS=1 TYPE=INPUT:BUTTON FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$multimedia$Upload_Image
            


            iiGallerypcisPlaycodestr = "TAG POS=1 TYPE=SPAN FORM=NAME:aspnetForm ATTR=TXT:Firmenprofil<SP>teilweise<SP>ausgefüllt\n" +
            "TAG POS=1 TYPE=A FORM=NAME:aspnetForm ATTR=TXT:Bilder<SP>hochladen<SP>Laden<SP>Sie<SP>Bilder<SP>und<SP>Fotos<SP>Ihres<SP>Unternehmens,<SP>Produkte<SP>oder<SP>D*\n" +
            "TAG POS=1 TYPE=A FORM=NAME:aspnetForm ATTR=TXT:Firmenfotos<SP>hochladen\n"+
            "PROMPT 1.)ggf.<SP>manuell<SP>in<SP>das<SP>Bilder<SP>Menu<SP>navigieren<SP>(Bilder<SP>hochladen/Firmenfotos<SP>hochladen<SP>)<BR>2.)Macro<SP>fortführen...\n";
      

            int i = 7;
            foreach (string pic in base.galleryPics)
            {
                iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$multimedia$tb_title CONTENT=" + pic + "\n" +
                                            "TAG POS=1 TYPE=INPUT:FILE FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$multimedia$FileUpload1 CONTENT=" + Config.picturePath + base.companyPicFolder + "\\" + pic + "\n" +
                                            "TAG POS=1 TYPE=INPUT:BUTTON FORM=ID:aspnetForm ATTR=ID:ctl00_ContentPlaceHolder1_multimedia_Upload_Image\n";
                i++;
            }

            iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=A FORM=NAME:aspnetForm ATTR=TXT:Dashboard\n";
            base.macroParts.Add(iiGallerypcisPlaycodestr);

        }


        protected override void setiiOpeningHoursCodePlayEntry()
        {
            string except = "";
            string tmp = "";
           
            //string iiohPlaycodestr = "CODE: VERSION BUILD=9052613\nTAB T=1\nTAB CLOSEALLOTHERS\nURL GOTO=http://admin.cylex.de/firma_page.aspx?action=startup&d=cylex.de";
            //string iiohPlaycodestr = "CODE: PROMPT 1.)Klick<SP>Eintrag<SP>Öffnungszeiten<SP>über<SP>Kopfmenu:<BR>Firmendaten<SP>Bearbeiten/Öffnungszeiten<BR>2.)Macro<SP>fortführen...\nPAUSE\n";
            string iiohPlaycodestr = "CODE: " +
                                     "DS CMD=CLICK X=359 Y=103 CONTENT=\n" +
                                     "DS CMD=CLICK X=353 Y=213 CONTENT=\n" +
                                     "PROMPT 1.)ggf.<SP>manuell<SP>in<SP>das<SP>Menu<SP>Öffnungszeiten<SP>navigieren<SP>(Bearbeiten/Öffnungszeiten)<BR>2.)Macro<SP>fortführen...\n";
           
                          
            //'NonStop
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$CheckBoxNonStop CONTENT=YES


            //'Öffnungszeiten am Vor- und Nachmittag

            //'Anfang Checkbox für Split-time
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$CheckBoxDouble CONTENT=YES

            //check if splittimes available
            string timecomparestring = base.weekdays2timedetails["mon"].ToString();
            bool ohNonstop = true;
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                if (!weekday.Value.ToString().Equals("24h"))
                    ohNonstop = false;
                
                if (weekday.Value.ToString().Contains("~"))
                {
                    iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$CheckBoxDouble CONTENT=YES\n";
                    Logger.Iiplaycode = iiohPlaycodestr;
                    break;
                }
            }

            if (ohNonstop)
            {
                
                iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$CheckBoxNonStop CONTENT=YES\n";
            }
            else
            {
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
            }
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE={{COL4}}<SP>({{COL7}})<SP>-<SP>Cylex<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png\n";

            iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:SUBMIT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$ButtonSave\n";
            iiohPlaycodestr += "TAG POS=1 TYPE=A FORM=NAME:aspnetForm ATTR=TXT:Dashboard\n";

            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times)
        {
            //open
            //'Montag
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayHour CONTENT=%08
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayMinutes CONTENT=%15
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayHour2 CONTENT=%15
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayMinutes2 CONTENT=%25

            //split
            //'Montag
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$CheckBox CONTENT=NO
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayHour CONTENT=%03
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayMinutes CONTENT=%20
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayHour2 CONTENT=%18
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayMinutes2 CONTENT=%30
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayHour11 CONTENT=%20
            //TAG POS=3 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayMinutes* CONTENT=%30
            //TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayHour12 CONTENT=%11
            //TAG POS=4 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl01$DropDownListCurrentDayMinutes* CONTENT=%30

            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl02$CheckBox CONTENT=NO
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl03$CheckBox CONTENT=NO
            string weekday = "";
            switch (day)
            {
                case "mon": weekday = "1";
                    break;
                case "tue": weekday = "2";
                    break;
                case "wen": weekday = "3";
                    break;
                case "thu": weekday = "4";
                    break;
                case "fri": weekday = "5";
                    break;
                case "sat": weekday = "6";
                    break;
                case "sun": weekday = "7";
                    break;
            }

            string iiohPlaycodestr = "";
            try
            {

                string ohType = "open";
                if (Logger.Iiplaycode != null && Logger.Iiplaycode.Contains("CheckBoxDouble CONTENT=YES"))//split times available?
                {
                    ohType = "split";
                }
                               
                iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$CheckBox CONTENT=NO\n";
                iiohPlaycodestr.Replace("#weekday#", weekday);
               
                switch (ohType)
                {
                    /****OPEN*****/
                    case "open":
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayHour CONTENT=%#timefromHour#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayMinutes CONTENT=%#timefromMinutes#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayHour2 CONTENT=%#timetoHour#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayMinutes2 CONTENT=%#timetoMinutes#\n";

                        iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday);

                        switch (mode)
                        {
                            case "closed": iiohPlaycodestr = "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$CheckBox CONTENT=YES\n";
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday);
                                break;
                            case "24h":
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday)
                                                                 .Replace("#timefromHour#", "00")
                                                                 .Replace("#timefromMinutes#", "00")
                                                                 .Replace("#timetoHour#", "00")
                                                                 .Replace("#timetoMinutes#", "00");
                                                                 
                                break;
                            case "open":
                                string[] openingtimes = times.Split('-');
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday)
                                                                 .Replace("#timefromHour#", openingtimes[0].Split(':')[0])
                                                                 .Replace("#timefromMinutes#", openingtimes[0].Split(':')[1])
                                                                 .Replace("#timetoHour#", openingtimes[1].Split(':')[0])
                                                                 .Replace("#timetoMinutes#", openingtimes[1].Split(':')[1]);
                                break;
                        }
                        break;

                    /****SPLIT*****/
                    case "split":
                        iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayHour CONTENT=%#timefromHour#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayMinutes CONTENT=%#timefromMinutes#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayHour2 CONTENT=%#timetoHour#\n" +
                                           "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayMinutes2 CONTENT=%#timetoMinutes#\n";
                                         
                        iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday);
                       
                        switch (mode)
                        {
                            case "closed":
                                iiohPlaycodestr = "";
                                break;
                            case "24h":
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#timefromHour#", "00")
                                                                 .Replace("#timefromMinutes#", "00")
                                                                 .Replace("#timetoHour#", "00")
                                                                 .Replace("#timetoMinutes#", "00");
                                                         
                                break;
                            case "open":
                                string[] openingtimes = times.Split('-');
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#timefromHour#", openingtimes[0].Split(':')[0])
                                                                 .Replace("#timefromMinutes#", openingtimes[0].Split(':')[1])
                                                                 .Replace("#timetoHour#", openingtimes[1].Split(':')[0])
                                                                 .Replace("#timetoMinutes#", openingtimes[1].Split(':')[1]);
                                break;
                            case "split":
                                iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayHour11 CONTENT=%#timefromHour2#\n" +
                                                   "TAG POS=3 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayMinutes* CONTENT=%#timefromMinutes2#\n" +
                                                   "TAG POS=1 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayHour12 CONTENT=%#timetoHour2#\n" +
                                                   "TAG POS=4 TYPE=SELECT FORM=NAME:aspnetForm ATTR=NAME:ctl00$ContentPlaceHolder1$Opening$Repeater1$ctl0#weekday#$DropDownListCurrentDayMinutes* CONTENT=%#timetoMinutes2#\n";
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#weekday#", weekday);

                                string[] splittimes = times.Split('~');
                                string fntimeFrom = times.Split('~')[0].Split('-')[0];
                                string fntimeTo = times.Split('~')[0].Split('-')[1];
                                string antimeFrom = times.Split('~')[1].Split('-')[0];
                                string antimeTo = times.Split('~')[1].Split('-')[1];
                                iiohPlaycodestr = iiohPlaycodestr.Replace("#timefromHour#", fntimeFrom.Split(':')[0])
                                                                 .Replace("#timefromMinutes#", fntimeFrom.Split(':')[1])
                                                                 .Replace("#timetoHour#", fntimeTo.Split(':')[0])
                                                                 .Replace("#timetoMinutes#", fntimeTo.Split(':')[1])
                                                                 .Replace("#timefromHour2#", antimeFrom.Split(':')[0])
                                                                 .Replace("#timefromMinutes2#", antimeFrom.Split(':')[1])
                                                                 .Replace("#timetoHour2#", antimeTo.Split(':')[0])
                                                                 .Replace("#timetoMinutes2#", antimeTo.Split(':')[1]);
                                                              
                                break;
                        }

                        break;


                }
            }
            catch (Exception e)
            {
                iiohPlaycodestr = "except*" + day + " (" + times + ")<BR>";
            }

            return iiohPlaycodestr;

        }


        protected override void setiiPlayListUpdate()
        {
            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Cylex\\Cylex_update.iim");
            processStepsViewList.Add("Login Firmenprofil ... ");

            //if (base.hasLogo)
            //{
            //    this.setiiLogoCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Firmenlogo ... ");
            //}
            //this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:6\n");
            //processStepsViewList.Add("Transition > Eintrag Öffnungszeiten ... ");
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }
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
            
            
            base.setiiPlayListUpdate();
        }


    }
}
