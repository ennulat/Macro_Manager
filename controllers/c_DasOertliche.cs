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
    class c_DasOertliche : BusinessDirectory
    {

        public c_DasOertliche(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "DasÖrtliche";
        }

        public override void Main()
        {
            
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("entry");
            base.Main();
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "";
            switch (macroType)
            {
                case "entry": browsertype = ""; break;
                default: browsertype = ""; break;
            }

            m_app.iimOpen(browsertype, true, 300);
        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {
                //case "entry": base.finalSuccessStatus = "Vielen Dank!"; break;
                case "entry": base.finalSuccessStatus = "services.dasoertliche.de/services/schnupperpaket/011_set.php"; break;
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

            base.macroParts.Add(Config.macroPath + "DasOertliche\\DasOertliche_entry1.iim");
            processStepsViewList.Add("Eintrag Firmendaten ... ");

            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:keys ATTR=NAME:SubmitForward\n"+                                
            //transition > Beschreibungstext > Öffnungszeiten
            this.macroParts.Add("CODE: TAG POS=1 TYPE=TEXTAREA FORM=NAME:sloganandlogo ATTR=NAME:freetext CONTENT=" + base.csvVars["COL21"].Replace(" ", "<SP>") + "\n");
            processStepsViewList.Add("Transition > Logoupload ... ");

            if (base.hasLogo)
            {
                this.setiiLogoCodePlayEntry();
                processStepsViewList.Add("Eintrag Firmenlogo ... ");
            }            

            //Transition > Öffnungszeiten
            //FORM=NAME:sloganandlogo
            this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:IMAGE  ATTR=NAME:SubmitForward\n");
            processStepsViewList.Add("Transition > Eintrag Öffnungszeiten ... ");

            if (base.hasOpeningHours)
            {
                this.setiiOpeningHoursCodePlayEntry();
                processStepsViewList.Add("Eintrag Öffnungszeiten ... ");
            }
            //this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:SUBMIT  ATTR=TABINDEX:32\n");

            this.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:SubmitForward\n");
            processStepsViewList.Add("Transition > Eintrag Gallerybilder ... ");
            if (base.hasGallerypics)
            {
                this.setiiGallerypicsCodePlayEntry();
                processStepsViewList.Add("Eintrag Gallerybilder ... ");
            }

            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:previewandback ATTR=NAME:SubmitForward
            //base.macroParts.Add("CODE: TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:SubmitForward\n");
            //processStepsViewList.Add("Transition >  Eintrag Kontaktdaten und Abschluss ... ");

            //base.macroParts.Add(Config.macroPath + "DasTelefonbuch\\DasTelefonbuch_desc_contactdata.iim");
            base.macroParts.Add(Config.macroPath + "DasOertliche\\DasOertliche_entry2.iim");
            processStepsViewList.Add("Stammdaten Preview, Eintrag Kontaktdaten und Abschluss ... ");


            console_menu.SetProcessStepsList(processStepsViewList);
        }

       

        protected override void setiiLogoCodePlayEntry()
        {
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:sloganandlogo ATTR=NAME:imgfile CONTENT=C:\fakepath\_Chrysanthemum.jpg
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:sloganandlogo ATTR=NAME:imgupload
            string iiLogoPlaycodestr = "CODE: TAG POS=1 TYPE=INPUT:FILE FORM=NAME:sloganandlogo ATTR=NAME:imgfile CONTENT=" + Config.picturePath + this.companyPicFolder + "\\" + base.csvVars["logo"] + "\n" +
                                       "TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:sloganandlogo ATTR=NAME:imgupload\n";
            base.macroParts.Add(iiLogoPlaycodestr);
        }

        protected override void setiiGallerypicsCodePlayEntry()
        {
            
            //URL GOTO=https://services.dasoertliche.de/services/schnupperpaket/007_set.php
            //TAG POS=2 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/007_set.php#
            //TAG POS=1 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/007_set.php#
            //TAG POS=2 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/007_set.php#
            //TAG POS=2 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/007_set.php#
            //TAG POS=1 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/007_set.php#
            //TAG POS=2 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/007_set.php#
            //TAG POS=1 TYPE=IMG FORM=NAME:gallery ATTR=SRC:https://services.dasoertliche.de/services/schnupperpaket/img/dmy_galerie1.gif
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:gallery ATTR=NAME:imgfile CONTENT=C:\fakepath\logo.jpg
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:imgupload
            //TAG POS=1 TYPE=IMG FORM=NAME:gallery ATTR=SRC:https://services.dasoertliche.de/services/schnupperpaket/img/dmy_galerie2.gif
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:gallery ATTR=NAME:imgfile CONTENT=C:\fakepath\photo_1.jpg
            //TAG POS=1 TYPE=IMG FORM=NAME:gallery ATTR=SRC:https://services.dasoertliche.de/services/schnupperpaket/img/dmy_galerie3.gif
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:gallery ATTR=NAME:imgfile CONTENT=C:\fakepath\photo_2.jpg
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:imgupload
            //TAG POS=1 TYPE=IMG FORM=NAME:gallery ATTR=SRC:https://services.dasoertliche.de/services/schnupperpaket/img/dmy_galerie3.gif
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:gallery ATTR=NAME:imgfile CONTENT=C:\fakepath\photo_2.jpg
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:imgupload
            //TAG POS=1 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/008_set.php#
            //TAG POS=1 TYPE=IMG FORM=NAME:gallery ATTR=SRC:https://services.dasoertliche.de/services/schnupperpaket/img/dmy_galerie4.gif
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:gallery ATTR=NAME:imgfile CONTENT=C:\fakepath\photo_1.jpg
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:imgupload
            //TAG POS=1 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/008_set.php#
            //TAG POS=4 TYPE=IMG FORM=NAME:gallery ATTR=SRC:https://services.dasoertliche.de/services/schnupperpaket/sendimage.php?p1=e8ba1*
            //TAG POS=1 TYPE=INPUT:FILE FORM=NAME:gallery ATTR=NAME:imgfile CONTENT=C:\fakepath\photo_2.jpg
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:imgupload
            //TAG POS=1 TYPE=DIV FORM=NAME:gallery ATTR=CLASS:detailview
            //TAG POS=1 TYPE=A FORM=NAME:gallery ATTR=HREF:https://services.dasoertliche.de/services/schnupperpaket/008_set.php#
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:SubmitForward
            string iiGallerypcisPlaycodestr = "CODE: PROMPT 1.)Manuell<SP>bis<SP>zu<SP>5<SP>Gallerybilder<SP>hochladen<BR>2.)Macro<SP>fortführen...\nPAUSE\n";

            //int i = 7;
            //foreach (string pic in base.galleryPics)
            //{
            //    iiGallerypcisPlaycodestr += "TYPE=INPUT:FILE ATTR=NAME:imgfile CONTENT=" + Config.picturePath + base.companyPicFolder + "\\" + pic + "\n";
            //    iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:gallery ATTR=NAME:imgupload\n";
            //    i++;
            //}

            base.macroParts.Add(iiGallerypcisPlaycodestr);

        }


        protected override void setiiOpeningHoursCodePlayEntry()
        {
            //string except = "";
            string tmp = "";
            string iiohPlaycodestr = "CODE: ";
            int ohAddIndex = 0;
            foreach (KeyValuePair<string, string> weekday in base.weekdays2timedetails)
            {
                tmp = "";
                string day = weekday.Key.ToString();
                string mode = weekday.Value.ToString().Split('|')[0];
                string times = weekday.Value.ToString().Split('|')[1];

                tmp = getiiOpeningHoursWeekdayStringEntry_dasOertliche(day, mode, times, ohAddIndex);

                if (!mode.Equals("closed"))
                {
                    ohAddIndex++;
                }

                iiohPlaycodestr += tmp;
            }

     
            iiohPlaycodestr += "TAG POS=1 TYPE=TEXTAREA FORM=NAME:oh ATTR=NAME:addohtext CONTENT=" + csvVars["COL22"].ToString().Replace(" ", "<SP>") + "\n" +
                               "PROMPT 1.)Öffnungszeiten<SP>überprüfen!<BR>2.)Macro<SP>fortführen...\nPAUSE\n";
                                
            iiohPlaycodestr += "SAVEAS TYPE=PNG FOLDER=* FILE="+ csvVars["COL4"].ToString().Replace(" ", "<SP>") +
                               "<SP>" +
                               "("+ csvVars["COL7"].ToString().Replace(" ", "<SP>") +")" + "<SP>-<SP>DasOertliche<SP>-<SP>OH<SP>-{{!NOW:dd.mm.yyyy,<SP>hh.nn.ss}}<SP>Uhr.png\n";
            

            
            base.macroParts.Add(iiohPlaycodestr);



            Logger.Iiplaycode = iiohPlaycodestr;
            base.setiiOpeningHoursCodePlayEntry();
        }

        private string getiiOpeningHoursWeekdayStringEntry_dasOertliche(string day, string mode, string times, int ohAddIndex)
        {
            //ohaddindex
            //MO > 0
            //Di > 1  
            //Mi > 2 
            //Do > 3 
            //Fr > 4
            //Sa > 5
            //So > 6

            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_from1 CONTENT=%20
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_to1 CONTENT=%27
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh1_from1 CONTENT=%21
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh1_to1 CONTENT=%26
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh1_Tu CONTENT=YES
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh3_from1 CONTENT=%26
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh3_to1 CONTENT=%29
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh3_We CONTENT=YES


            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_from1 CONTENT=%20
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_to1 CONTENT=%29
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_from2 CONTENT=%31
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_to2 CONTENT=%39
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh0_Mo CONTENT=YES
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh1_from1 CONTENT=%18
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh1_to1 CONTENT=%35
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh1_Tu CONTENT=YES
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh2_from1 CONTENT=%18
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh2_to1 CONTENT=%28
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh2_from2 CONTENT=%32
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh2_to2 CONTENT=%37
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh2_We CONTENT=YES
            //TAG POS=1 TYPE=DIV FORM=NAME:oh ATTR=TXT:Ihre<SP>Öffnungszeiten<SP>Mo<SP>Di<SP>Mi<SP>Do<SP>Fr<SP>Sa<SP>So<SP>00:0000:3001:0001:3002:0002:3003:0003:*
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh3_from1 CONTENT=%20
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh3_to1 CONTENT=%25
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh3_Th CONTENT=YES
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh4_from1 CONTENT=%18
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh4_to1 CONTENT=%24
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh4_from2 CONTENT=%26
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh4_to2 CONTENT=%34
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh4_Fr CONTENT=YES
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=21 TYPE=TD FORM=NAME:oh ATTR=TXT:00:0000:3001:0001:3002:0002:3003:0003:3004:0004:3005:0005:3006:0006:3007:0007:3*
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh5_from1 CONTENT=%31
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh5_to1 CONTENT=%37
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh5_Sa CONTENT=YES
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh6_from1 CONTENT=%21
            //TAG POS=26 TYPE=TD FORM=NAME:oh ATTR=TXT:00:0000:3001:0001:3002:0002:3003:0003:3004:0004:3005:0005:3006:0006:3007:0007:3*
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh6_to1 CONTENT=%30
            //TAG POS=27 TYPE=TD FORM=NAME:oh ATTR=TXT:00:0000:3001:0001:3002:0002:3003:0003:3004:0004:3005:0005:3006:0006:3007:0007:3*
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh6_from2 CONTENT=%32
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh6_to2 CONTENT=%36
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh6_Su CONTENT=YES
            //TAG POS=1 TYPE=TEXTAREA FORM=NAME:oh ATTR=NAME:addohtext CONTENT=öz<SP>beschr
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:SubmitForward



            //URL GOTO=https://services.dasoertliche.de/services/schnupperpaket/006_set.php
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_from1 CONTENT=%21
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_to1 CONTENT=%29
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh0_from2 CONTENT=%-1
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh0_Mo CONTENT=YES
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh1_We CONTENT=YES
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh1_from1 CONTENT=%35
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh1_to1 CONTENT=%39
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=3 TYPE=LABEL FORM=NAME:oh ATTR=TXT:Fr
            //TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh2_Fr CONTENT=YES
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh2_from1 CONTENT=%28
            //TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh2_to1 CONTENT=%29
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=DIV FORM=NAME:oh ATTR=TXT:Ihre<SP>Öffnungszeiten<SP>Mo<SP>Di<SP>Mi<SP>Do<SP>Fr<SP>Sa<SP>So<SP>00:0000:3001:0001:3002:0002:3003:0003:*
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement
            //TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement


            string iiohPlaycodestr = "TAG POS=1 TYPE=INPUT:IMAGE FORM=NAME:oh ATTR=NAME:addohelement\n";
            string weekday = "";
            switch (day)
            {
                case "mon": weekday = "Mo"; break;
                case "tue": weekday = "Tu"; break;
                case "wen": weekday = "We"; break;
                case "thu": weekday = "Th"; break;
                case "fri": weekday = "Fr"; break;
                case "sat": weekday = "Sa"; break;
                case "sun": weekday = "Su"; break;
            }

            iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh#ohaddindex#_from1 CONTENT=%#fromtime1#\n" +
                               "TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh#ohaddindex#_to1 CONTENT=%#totime1#\n".Replace("#ohaddindex#", ohAddIndex.ToString());

            int ifromtmp1, ifromtmp2;
            int itotmp1, itotmp2;
            string strfromtmp1, strfromtmp2;
            string strtotmp1, strtotmp2;

            switch (mode)
            {
                case "closed": iiohPlaycodestr = ""; break;
                case "24h":
                    iiohPlaycodestr = iiohPlaycodestr.Replace("#fromtime1#", "00:00")
                                                     .Replace("#totime1#", "00:00");
                    break;
                case "open":
                    string[] openingtimes = times.Split('-');
                    ifromtmp1 = int.Parse(openingtimes[0].Split(':')[0]) * 2;
                    if(openingtimes[0].Split(':')[1] != "00"){//set multiplikator 2?
                        ifromtmp1++;
                    }
                    strfromtmp1 = (ifromtmp1.ToString().Length.Equals(1)) ? "0" + ifromtmp1.ToString() : ifromtmp1.ToString();


                    itotmp1 = int.Parse(openingtimes[1].Split(':')[0]) * 2;
                    if(openingtimes[1].Split(':')[1] != "00"){//set multiplikator 2?
                        itotmp1++;
                    }
                    strtotmp1 = (itotmp1.ToString().Length.Equals(1)) ? "0" + itotmp1.ToString() : itotmp1.ToString();


                    iiohPlaycodestr = iiohPlaycodestr.Replace("#fromtime1#", strfromtmp1)
                                                     .Replace("#totime1#", strtotmp1);
                    break;
                case "split":
                    string fntimeFrom = times.Split('~')[0].Split('-')[0];
                    string fntimeTo = times.Split('~')[0].Split('-')[1];
                    string antimeFrom = times.Split('~')[1].Split('-')[0];
                    string antimeTo = times.Split('~')[1].Split('-')[1];

                    //vormittagszeiten
                    ifromtmp1 = int.Parse(fntimeFrom.Split(':')[0]) * 2;
                    if (fntimeFrom.Split(':')[1] != "00")
                    {//set multiplikator 2?
                        ifromtmp1++;
                    }
                    strfromtmp1 = (ifromtmp1.ToString().Length.Equals(1)) ? "0" + ifromtmp1.ToString() : ifromtmp1.ToString();


                    itotmp1 = int.Parse(fntimeTo.Split(':')[0]) * 2;
                    if (fntimeTo.Split(':')[1] != "00")
                    {//set multiplikator 2?
                        itotmp1++;
                    }
                    strtotmp1 = (itotmp1.ToString().Length.Equals(1)) ? "0" + itotmp1.ToString() : itotmp1.ToString();

                    //nachmittagszeiten
                    ifromtmp2 = int.Parse(antimeFrom.Split(':')[0]) * 2;
                    if (antimeFrom.Split(':')[1] != "00")
                    {//set multiplikator 2?
                        ifromtmp2++;
                    }
                    strfromtmp2 = (ifromtmp2.ToString().Length.Equals(1)) ? "0" + ifromtmp2.ToString() : ifromtmp2.ToString();


                    itotmp2 = int.Parse(antimeTo.Split(':')[0]) * 2;
                    if (antimeTo.Split(':')[1] != "00")
                    {//set multiplikator 2?
                        itotmp2++;
                    }
                    strtotmp2 = (itotmp2.ToString().Length.Equals(1)) ? "0" + itotmp2.ToString() : itotmp2.ToString();

                    iiohPlaycodestr += "TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh#ohaddindex#_from2 CONTENT=%#fromtime2#\n" +
                                       "TAG POS=1 TYPE=SELECT FORM=NAME:oh ATTR=NAME:oh#ohaddindex#_to2 CONTENT=%#totime2#\n";

                    iiohPlaycodestr = iiohPlaycodestr.Replace("#fromtime1#", strfromtmp1)
                                                     .Replace("#totime1#", strtotmp1)
                                                     .Replace("#fromtime2#", strfromtmp2)
                                                     .Replace("#totime2#", strtotmp2);
                    break;

            }

            if (!mode.Equals("closed")) {
                iiohPlaycodestr = iiohPlaycodestr.Replace("#ohaddindex#", ohAddIndex.ToString());
                iiohPlaycodestr += "TAG POS=1 TYPE=INPUT:CHECKBOX FORM=NAME:oh ATTR=NAME:oh" + ohAddIndex + "_" + weekday + " CONTENT=YES\n";
            }



            return iiohPlaycodestr;

        }

    }
}
