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
    class c_GoYellow : BusinessDirectory
    {

        public c_GoYellow(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "GoYellow";
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
                case "entry": base.finalSuccessStatus = "Vielen Dank für Ihr Vertrauen in GoYellow"; break;
                case "update": base.finalSuccessStatus = "Der Eintrag wurde gespeichert"; break; 
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

            base.macroParts.Add(Config.macroPath + "GoYellow\\GoYellow_updatepart1.iim");
            processStepsViewList.Add("Update Firmenstammdaten ... ");
           
            if (base.hasLogo)
            {
                this.setiiLogoCodePlayEntry();
                processStepsViewList.Add("Eintrag Firmenlogo ... ");
            }

            base.macroParts.Add(Config.macroPath + "GoYellow\\GoYellow_updatepart2.iim");
            processStepsViewList.Add("Update Firmenstammdaten ... ");
            
            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }

            if (base.hasGallerypics)
            {
                this.setiiGallerypicsCodePlayEntry();
                processStepsViewList.Add("Eintrag Galleriefoto (Photo_1) unter Registerkarte Leistung");
            }

            base.macroParts.Add(Config.macroPath + "GoYellow\\GoYellow_updatepart3.iim");
            processStepsViewList.Add("Kategoriebezeichnung & Kontaktdaten > Abschluss ... ");

            console_menu.SetProcessStepsList(processStepsViewList);
        }


        protected override void setiiPlayListEntry()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "GoYellow\\GoYellow_entrypart1.iim");
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

            //if (base.hasGallerypics) {
            //    this.setiiGallerypicsCodePlayEntry();
            //    processStepsViewList.Add("Eintrag Galleriefoto > Registerkarte Leistung");
            //}

            base.macroParts.Add(Config.macroPath + "GoYellow\\GoYellow_entrypart2.iim");
            processStepsViewList.Add("Eintragung Unternehmesbeschreibung ... ");

            base.macroParts.Add(Config.macroPath + "GoYellow\\GoYellow_entrypart3.iim");
            processStepsViewList.Add("Eintragung Kategoriebezeichnung & Kontaktdaten > Abschluss ... ");

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

            string iiLogoPlaycodestr = "CODE: TAG POS=1 TYPE=A ATTR=ID:link_uploadLogo1\n";
            //<input id="fileSelectLogo" class="fileSelection" type="file" lang="de" name="fileSelectLogo" accesskey="b" size="20" tabindex="120" title="Wählen Sie ein Bild von Ihrem Rechner"></input>
            iiLogoPlaycodestr += "TAG POS=1 TYPE=INPUT:FILE ATTR=ID:fileSelectLogo CONTENT=" + Config.picturePath + base.currentCompany.Replace(" ", "_") + "\\" + base.csvVars["logo"].ToString() + "\n";
            //iiLogoPlaycodestr += "TAG POS=1 TYPE=A ATTR=CLASS:btnLink\n";
            iiLogoPlaycodestr += "PROMPT 1.)Logo<SP>hochladen<SP>anklicken!<BR>2.)Macro<SP>fortführen...\nPAUSE\n";
            base.macroParts.Add(iiLogoPlaycodestr);
        }

        protected override void  setiiGallerypicsCodePlayEntry()
        {

            //Detailseiten/Leistungen
            //<a id="subpanel_standard2" class="edit"
            //<a id="link_pageMediaFile5" class="mediaFileSelectionOpener" tabindex="603" title="Klicken, um das Bild zu bearbeiten" href="#"
            //string iigallerypcisplaycodestr = "CODE: SET !SINGLESTEP YES\nTAG POS=1 TYPE=A ATTR=ID:link_pageMediaFile5\n"; <= Registerkarte Überuns
            string iigallerypcisplaycodestr = "CODE: TAG POS=1 TYPE=A ATTR=ID:link_pageMediaFile2\n";
            iigallerypcisplaycodestr += "TAG POS=1 TYPE=INPUT:FILE ATTR=ID:fileSelectImage CONTENT=" + Config.picturePath + base.currentCompany.Replace(" ", "_") + "\\" + base.galleryPics[0].ToString() + "\n";
            iigallerypcisplaycodestr += "PROMPT 1.)(photo_1)vorausgewählt,<SP>Bild<SP>hochladen<SP>anklicken<BR>ODER<BR>bevorzugtes<SP>Gallerybild<SP>manuell<SP>selektieren.<BR>2.)Macro<SP>fortführen...\nPAUSE\n";
            iigallerypcisplaycodestr += "TAG POS=7 TYPE=IMG ATTR=SRC:https://kundenportal.goyellow.de/images/common/btn_close.png\n";
            base.macroParts.Add(iigallerypcisplaycodestr);

            //base.setiiGallerypicsCodePlayEntry();
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
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE={{COL4}}<SP>({{COL7}})<SP>-<SP>GoYellow_entry_OH<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png";
            base.macroParts.Add(iiohPlaycodestr);

            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        protected override string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times)
        {
            
            
            //URL GOTO=https://kundenportal.goyellow.de/edit-listing.yp?product=richcontent&displayAdviceBox=Y
            //TAG POS=1 TYPE=A ATTR=ID:subpanel_companyprofile1
            //TAG POS=1 TYPE=INPUT:TEXT ATTR=ID:amfrom0 CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amto0 CONTENT=14:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmfrom0 CONTENT=15:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmto0 CONTENT=18:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amfrom1 CONTENT=09:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amto1 CONTENT=15:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmfrom1 CONTENT=16:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmto1 CONTENT=19:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amfrom2 CONTENT=08:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amto2 CONTENT=13:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmfrom2 CONTENT=14:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmto2 CONTENT=18:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amfrom3 CONTENT=19:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amto3 CONTENT=14:00
            //TAG POS=8 TYPE=TD ATTR=TXT:von
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmfrom3 CONTENT=15:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmto3 CONTENT=19:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amfrom4 CONTENT=08:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amto4 CONTENT=15:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmfrom4 CONTENT=16:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:pmto4 CONTENT=19+:0
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amfrom5 CONTENT=12:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amto5 CONTENT=15:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amfrom6 CONTENT=11:00
            //TAG POS=1 TYPE=INPUT:TEXT FORM=ID:frmEntry ATTR=ID:amto6 CONTENT=14:00
            //TAG POS=1 TYPE=TEXTAREA FORM=ID:frmEntry ATTR=ID:openinghourtext CONTENT=oh<SP>desc


            string iiohPlaycodestr = "TAG POS=1 TYPE=INPUT:TEXT ATTR=ID:amfrom#weekdayindex# CONTENT=#amtimefrom#\n" +
                                     "TAG POS=1 TYPE=INPUT:TEXT ATTR=ID:amto#weekdayindex# CONTENT=#amtimeto#\n";
            string weekdayindex = "";
            switch (day)
            {
                case "mon": weekdayindex = "0";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex); break;
                case "tue": weekdayindex = "1";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex); break;
                case "wen": weekdayindex = "2";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex); break;
                case "thu": weekdayindex = "3";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex); break;
                case "fri": weekdayindex = "4";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex); break;
                case "sat": weekdayindex = "5";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex); break;
                case "sun": weekdayindex = "6";
                            iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex); break;
            }

            switch (mode)
            {
                case "closed": iiohPlaycodestr = ""; break;
                case "24h":
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#amtimefrom#", "00:00").Replace("#amtimeto#", "00:00");
                    break;
                case "open":
                    string[] openingtimes = times.Split('-');
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#amtimefrom#", openingtimes[0]).Replace("#amtimeto#", openingtimes[1]);
                    break;
                case "split":
                    string amtimeFrom = times.Split('~')[0].Split('-')[0];
                    string amtimeTo = times.Split('~')[0].Split('-')[1];
                    string pmtimeFrom = times.Split('~')[1].Split('-')[0];
                    string pmtimeTo = times.Split('~')[1].Split('-')[1];
                    iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:TEXT ATTR=ID:pmfrom#weekdayindex# CONTENT=#pmtimefrom#\n" +
                                       "TAG POS=1 TYPE=INPUT:TEXT ATTR=ID:pmto#weekdayindex# CONTENT=#pmtimeto#\n";

                    iiohPlaycodestr = iiohPlaycodestr.Replace("#weekdayindex#", weekdayindex)
                                                     .Replace("#amtimefrom#", amtimeFrom)
                                                     .Replace("#amtimeto#", amtimeTo)
                                                     .Replace("#pmtimefrom#", pmtimeFrom)
                                                     .Replace("#pmtimeto#", pmtimeTo);
                    break;
            }

            iiohPlaycodestr += "TAG POS=1 TYPE=A ATTR=ID:subpanel_companyprofile1\n" +
                               "TAG POS=1 TYPE=TEXTAREA FORM=ID:frmEntry ATTR=ID:openinghourtext CONTENT=" + base.csvVars["COL22"].ToString().Replace(" ", "<SP>") + "\n";

            return iiohPlaycodestr;

        }

    }
}
