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
    class c_Pointoo : BusinessDirectory
    {

        public c_Pointoo(Customer customer): base(customer)
        {
            BusinessDirectory.directoryname = "Pointoo";
        }

        public override void Main()
        {
            
            BusinessDirectory.macroTypeList = new List<string>();
            BusinessDirectory.macroTypeList.Add("registration");
            BusinessDirectory.macroTypeList.Add("profil");
            base.Main();
        }

        protected override void iiOpenImacroApp(string macroType)
        {
            string browsertype = "-fx";
            switch (macroType)
            {
                case "registration": browsertype = "-fx"; break;
                case "profil": browsertype = "-fx"; break;
                default: browsertype = ""; break;
            }

            m_app.iimOpen(browsertype, true, 300);

        }

        protected override void setFinalMacroSuccessStatus(string macroType)
        {
            //set final macro status for success validation
            switch (macroType)
            {

                case "registration": base.finalSuccessStatus = "Bitte E-Mail-Adresse bestätigen"; break;
                case "profil": base.finalSuccessStatus = "http://www.pointoo.de/"; break;
                default: base.finalSuccessStatus = ""; break;
            }
        }

        protected override void setiiPlayList(string typeselection)
        {
            switch (typeselection)
            {
                case "registration": setiiPlayListRegistration(); break;
                case "profil": setiiPlayListProfil(); break;
            }

        }
       

        protected override void setiiPlayListRegistration()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Pointoo\\Pointoo_registration.iim");
            processStepsViewList.Add("Eintrag Registrationsdaten ... ");


            console_menu.SetProcessStepsList(processStepsViewList);
        }

        protected override void setiiGallerypicsCodePlayEntry()
        {
            
            
            //C:\Users\Public\Pictures\Sample<SP>Pictures\_Chrysanthemum.jpg
            //
            //TAG POS=2 TYPE=SPAN ATTR=TXT:Eigene<SP>Fotos<SP>hochladen
            //TAG POS=1 TYPE=INPUT:FILE FORM=ID:new_assets_image ATTR=ID:assets_image_file CONTENT=C:\Users\Public\Pictures\Sample<SP>Pictures\_Desert.jpg
            //TAG POS=1 TYPE=BUTTON FORM=ID:new_assets_image ATTR=ID:assets_image_submit
            //TAG POS=2 TYPE=SPAN ATTR=TXT:Eigene<SP>Fotos<SP>hochladen
            //TAG POS=1 TYPE=INPUT:FILE FORM=ID:new_assets_image ATTR=ID:assets_image_file CONTENT=C:\Users\Public\Pictures\Sample<SP>Pictures\_Hydrangeas.jpg
            //TAG POS=1 TYPE=BUTTON FORM=ID:new_assets_image ATTR=ID:assets_image_submit


            string iiGallerypcisPlaycodestr = "CODE: ";
            iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=SPAN ATTR=TXT:Fotos<SP>hochladen\n";
            int i = 7;
            foreach (string pic in base.galleryPics)
            {
                iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=INPUT:FILE FORM=ID:new_assets_image ATTR=ID:assets_image_file CONTENT=" + Config.picturePath + base.companyPicFolder + "\\" + pic + "\n";
                iiGallerypcisPlaycodestr += "TAG POS=1 TYPE=BUTTON FORM=ID:new_assets_image ATTR=ID:assets_image_submit\n";
                i++;
            }

            base.macroParts.Add(iiGallerypcisPlaycodestr);

        }

        protected override void setiiPlayListProfil()
        {

            List<string> processStepsViewList = console_menu.GetProcessStepsList();

            base.macroParts.Add(Config.macroPath + "Pointoo\\Pointoo_profil.iim");
            processStepsViewList.Add("Eintrag Firmenprofil ... ");

            if (base.hasLogo || base.hasGallerypics) {
                this.setiiGallerypicsCodePlayEntry();
                processStepsViewList.Add("Upload Logo/Galleriebilder ... ");
            }

            base.macroParts.Add("CODE: TAG POS=2 TYPE=SPAN ATTR=TXT:abmelden");
            processStepsViewList.Add("Abmeldung .... ");
            console_menu.SetProcessStepsList(processStepsViewList);
        }



       

    }
}
