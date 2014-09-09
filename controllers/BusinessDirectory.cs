using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macro_Manager.models;
using Macro_Manager.helper;
using Macro_Manager.views;
using iMacros;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace Macro_Manager.controllers
{
    class BusinessDirectory
    {

        protected Customer customer;
        protected List<string[]> data;
        //public static string[] companyNames;
        public static List<string> companyNames;
        protected List<string> macroParts;
        protected App m_app;
        protected Status m_status;
        protected string iiOpenMode;
        protected Dictionary<string, string> csvVars;
        protected string companyPicFolder;
        public static string directoryname;
        protected string extract;
        protected string currentCompany;
        protected bool hasOpeningHours;
        protected bool hasLogo;
        protected bool hasGallerypics;
        protected List<string> galleryPics;
        protected SortedList weekDaysSplitTimes;
        protected SortedList weekDaysOpeningTimes;
        protected SortedList weekDays24HourTimes;
        protected SortedList weekDaysClosed;
        protected SortedList workDaysClosed;
        protected SortedList workDays24h;
        protected SortedList workDaysOpen;
        protected SortedList workDaysSplit;
        public static List<string> macroTypeList;
        protected Dictionary<string,string> weekdays2timedetails;
        protected string[] ohWorkdays = new string[] { "mon", "tue", "wen", "thu", "fri" };
        protected string[] ohWeekdays = new string[] { "mon", "tue", "wen", "thu", "fri", "sat", "sun" };
        protected string[] ohModes = new string[] { "closed", "24h", "open", "split" };
        protected SortedList returnCodes;
        protected string finalSuccessStatus;
        protected int macro_type_selection;


        public BusinessDirectory(Customer customer) {
            //this.customer = customer;
            //this.customer = new Customer();
            this.customer = customer;
            this.customer.readCSV();
            data = customer.GetData();
            
            companyNames = customer.GetCompanyNames();
            m_app = new App();
            m_status = new Status();
            setReturnCodes();

            macroParts = new List<string>();

            this.hasOpeningHours = false;
            this.hasLogo = false;
            this.hasGallerypics = false;
        }

        private void setReturnCodes(){
            returnCodes = new SortedList();
            returnCodes.Add("-101", "Fehler (-101): Aborted: User pressed the Stop button in the iMacros sidebar. Typically, you can check this value to see if the user wants to exit the application.");
            returnCodes.Add("-102", "Fehler (-102): Browser Closed: User pressed the Window Close button in the browser.");
            returnCodes.Add("-802", "Fehler (-802): Timeout error (failed to load web page)");
            returnCodes.Add("-910", "Fehler (-910): Syntax error");
            returnCodes.Add("-911", "Fehler (-911): Bad parameter");
            returnCodes.Add("-912", "Fehler (-912): Unsupported command (e. g. DS)");
            returnCodes.Add("-920", "Fehler (-920): Element for specified x/y coordinates was not found ");
            returnCodes.Add("-921", "Fehler (-921): Element specified by TAG command was not found ");
            returnCodes.Add("-924", "Fehler (-924): Number of options in select box element has been exceeded\nAchtung: Die Zeitangaben der Öffnungszeiten werden nicht unterstützt!\nWenn möglich ändern.");
            returnCodes.Add("-925", "Fehler (-925): Select box has no specified options  ");
            returnCodes.Add("-930", "Fehler (-930): File not found (e.g. for imacros://run?m=non_existent.iim or URL GOTO=file://...) ");
            returnCodes.Add("-931", "Fehler (-931): Could not access file");
            returnCodes.Add("-933", "Fehler (-933): Network error while file or page loading");
            returnCodes.Add("-1001", "Fehler (-1001): Unknown error ");
            returnCodes.Add("-1100", "Fehler (-1100): Failed to load the macro (syntax or I/O error) (Found wrong macro command while loading file)..");
            returnCodes.Add("-1300", "Fehler (-1300): Html Element Not Found: Cannot find HTML element.");
            returnCodes.Add("-1800", "Fehler (-1800): Image Not Found: Could not find image with given confidence in browser screen. ");
            returnCodes.Add("-1810", "Fehler (-1810): Image Recognition Internal Error: Internal Error in Image Recognition Plugin (Is it properly installed?). ");
            returnCodes.Add("-1820", "Fehler (-1820): Cannot Load Image: Image file could not be loaded. ");
            returnCodes.Add("-1830", "Fehler (-1830): Illegal Imagesearch Subregion: ImageSearch subregion is either not completely contained in the source image, or smaller than search image, or not a rectangle. ");
            returnCodes.Add("-1840", "Fehler (-1840): ImageSearch Not Enough Memory: ImageSearch failed to allocate enough memory to open image file.  ");
        }

        protected void setCsvVars(int i)
        {
            string[] c_data = new string[data[i].Length];
            data[i].CopyTo(c_data, 0); 
            csvVars = new Dictionary<string,string>();
            string openinghoursdesc = "";
            string openinghours = "";
            string allpics = "";


            //change index of opening hours and oh description
            //oh from csv comes on index 21 and oh description on 22
            //string tmp = c_data[21];
            //c_data[21] = c_data[22];
            //c_data[22] = tmp;
           
            for (int z = 0; z < c_data.Length; z++)
            {
                string name = "";
                string value = "";
                switch (z)
                {
                    case 21: openinghoursdesc = c_data[z]; break;
                    case 22: openinghours = c_data[z]; break;
                    case 23: allpics = c_data[z]; break;
                    default: 
                        break;
                }

                name = "COL" + (z + 1).ToString();
                value = c_data[z].Trim();
                m_app.iimSet(name, value);
                csvVars.Add(name, value);
               
                
            }

            hasOpeningHours = false;
            weekdays2timedetails.Clear();
            if (openinghours != "")
            {
                this.hasOpeningHours = true;
                setOpeninHoursLists(openinghours);
            }


            hasLogo = false;
            hasGallerypics = false;
            if(allpics != ""){
                if (Directory.Exists(Config.picturePath + this.csvVars["COL4"].ToString().Replace(' ', '_')))
                    this.companyPicFolder = this.csvVars["COL4"].ToString().Replace(' ', '_');
                else if (Directory.Exists(Config.picturePath + this.csvVars["COL4"].ToString().Replace(' ', '_') + this.csvVars["COL4"].ToString().GetHashCode()))
                    this.companyPicFolder = this.csvVars["COL4"].ToString().Replace(' ', '_') + this.csvVars["COL4"].ToString().GetHashCode();
                else
                    throw new Exception("Bilderverzeichnis => " + this.csvVars["COL4"].ToString().Replace(' ', '_') + this.csvVars["COL4"].ToString().GetHashCode() + "nicht gefunden!");
                     

                if(allpics.Contains("logo")){
                    this.hasLogo = true;
                }

                if(allpics.Contains("photo")){
                    this.hasGallerypics = true;
                    this.galleryPics = new List<string>();
                }


                string[] pics = allpics.Split('|');
                foreach (string pic in pics) {
                    if (pic.Contains("logo")) {
                        this.csvVars.Add("logo", pic);
                    }else if(pic.Contains("photo")){
                        this.galleryPics.Add(pic);
                    }
                } 
               
            }


        }

        public virtual void Main(){


            bool loopMacroMenu = true;
            bool loopCustomerMenu = true;
            bool loopCompany = true;
            int customer_selection;
            int macro_step = 0;
            macro_type_selection = 0;
            string logStatus = "";
            string logMessage = "";
                                                


            do{
                
                try
                {
                    //pass: macro type menu
                    Console.Clear();
                    loopMacroMenu = true;
                    macro_type_selection = 0;
                    if (BusinessDirectory.macroTypeList.Count > 1) {
                        console_menu.macroMenu(ref macro_type_selection, ref loopMacroMenu);
                        if (!loopMacroMenu)
                            return;
                    }


                    do
                    {
                            
                        //pass: customer menu
                        Console.Clear();
                        loopCustomerMenu = true;
                        console_menu.customerMenu();
                        string strmenuselection = "";
                        int imenuselection = 0;  
                        strmenuselection = Console.ReadLine();
                        string pattern = @"(\d)";
                        Regex _regex = new Regex(pattern);
                        Match match;
                        match = _regex.Match(strmenuselection);
                        if (match.Length > 0)//type eq int
                        {
                            imenuselection = int.Parse(strmenuselection);
                            if (imenuselection >= companyNames.Count)//out of customer range ?
                                continue;
                        }
                        else {
                            if (strmenuselection.Equals("a"))//update customer data
                            {
                                customer.readCSV();
                                companyNames = customer.GetCompanyNames();
                                data = customer.GetData();
                                MainController.customer = customer;
                                continue;
                            }
                            else if (strmenuselection.Equals("b"))//redirect to macro type menu
                                break;
                            else if (strmenuselection.Equals("c"))//redirect to directory menu
                                return;
                            else
                                continue;
                        }
      
                        customer_selection = imenuselection;
                        currentCompany = BusinessDirectory.companyNames[customer_selection].ToString();
                        console_menu.displayMacroProcessHeader(customer_selection);

                        loopCompany = true;
                        macro_step = 0;
                        console_menu.ClearProcessStepsList();
                        macroParts = new List<string>();        
                        do
                        {//pass: loop each macro chunk with selected customer
                            try
                            {

                                iiOpenImacroApp(macroTypeList[macro_type_selection]);
                                setCsvVars(customer_selection);
                                if (macroParts.Count == 0)
                                {
                                    this.setiiPlayList(macroTypeList[macro_type_selection]);
                                    this.setFinalMacroSuccessStatus(macroTypeList[macro_type_selection]);
                                }

                                console_menu.displayMacroProcessStep(macro_step);
                                m_status = m_app.iimPlay(macroParts[macro_step]);

                                if (m_status.ToString() == "sOk")
                                {
                                    if (macro_step == macroParts.Count - 1)//end of macro reached?
                                    {
                                        this.extract = m_app.iimGetExtract();
                                        if (this.extract.Contains(finalSuccessStatus))//macro has finalized successfull?
                                        {
                                            writeLogfile("success", "OK");
                                            console_menu.displayMacroProcessFooter(ref macro_step, ref customer_selection, ref loopMacroMenu, ref loopCustomerMenu, ref loopCompany);
                                            currentCompany = BusinessDirectory.companyNames[customer_selection].ToString();
                                            
                                            macroParts.Clear();
                                            weekdays2timedetails.Clear();
                                            hasLogo = false;
                                            hasOpeningHours = false;
                                            hasGallerypics = false;



                                        }
                                        else
                                        {
                                                    
                                            logStatus = "error";
                                            logMessage = "Fehler, der finalSuccessStatus (" + finalSuccessStatus + ") ist abweichend!\n" +
                                                            "ggf. wurde die Firma: " + BusinessDirectory.companyNames[customer_selection]+ " bereits eingetragen?";
                                            writeLogfile(logStatus, logMessage );
                                            
                                        }
                                 
                                    }
                                    else
                                    {
                                        macro_step++; continue;
                                    }
                                }
                                else
                                {
                                    logStatus = "error";
                                    logMessage = "Ein unbekannter Fehler ist aufgetreten.";
                                    if (returnCodes[m_status.ToString()] != null)
                                    {
                                                
                                        logMessage = returnCodes[m_status.ToString()].ToString();
                                    }
                                            
                                    writeLogfile(logStatus, logMessage);

                                    this.iitakeScreenshot();
                                }

                            }
                            catch (Exception e)
                            {
                                loopCompany = false;
                                logStatus = "error";
                                logMessage = "Message: " + e.Message + "\n" + "Stacktrace:" + e.StackTrace;
                                writeLogfile(logStatus, logMessage);
                                
                            }

                            if (logStatus.Equals("error"))
                            {
                                console_menu.displayErrorMessageAndMenuOptions(ref macro_step, ref loopMacroMenu, ref loopCustomerMenu, ref loopCompany, customer_selection, logMessage);
                                logStatus = "";
                                logMessage = "";
                                macroParts.Clear();
                            }

                        } while (loopCompany);


                    } while (loopCustomerMenu);
                }
                catch (FormatException fex)
                {
                    //do nothing ..
                }
            
            }while(loopMacroMenu);

            m_app.iimExit();
        }

        protected void iitakeScreenshot(){

                string date = DateTime.Now.Date.ToString(new CultureInfo("de-DE")).Replace(" 00:00:00", "").Replace('.', '-');
                string time = DateTime.Now.TimeOfDay.ToString();
                string path = Config.logPath + date + "\\ErrorScreenshots";

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }   
              
                path = path + "\\Directory_" + directoryname + "_MacroType_" + macroTypeList[macro_type_selection] + "_company_" + currentCompany +"_Time_"+DateTime.Now.ToString("dd'-'mm'-'yyyy'--'H'-'mm'-'ss") +".png";
                iMacros.Status status = m_app.iimTakeBrowserScreenshot(path, 0);
        
        }
        private void setOpeninHoursLists(string openinghours)
        {

            weekDaysSplitTimes = new SortedList();
            weekDaysOpeningTimes = new SortedList();
            weekDays24HourTimes = new SortedList();
            weekDaysClosed = new SortedList();
            weekdays2timedetails = new Dictionary<string,string>();

            string[] rawdays = openinghours.Split('|');
            string rawtimes = "";
            string[] aDay;

            foreach (string rawday in rawdays)
            {
                aDay = rawday.Split('#');
                string weekday = aDay[0];
                string mode = aDay[1];
                rawtimes = aDay[2];

                //add reference weekday with mode and time
                string[] timedetails = new string[2];
                weekdays2timedetails.Add(weekday, mode + "|" + rawtimes);

                //add weektimes by mode
                switch (mode)
                {

                    case "split":
                        //"00:00-00:00~00:00-00:00"
                        weekDaysSplitTimes.Add(weekday, rawtimes);
                        break;

                    case "open":
                        //"00:00-00:00"
                        weekDaysOpeningTimes.Add(weekday, rawtimes);
                        break;
                    case "24h":
                        weekDays24HourTimes.Add(weekday, "24h");
                        break;
                    case "closed":
                        weekDaysClosed.Add(weekday, "closed");
                        break;
                }

            }


            //build workday lists
            foreach (string mode in this.ohModes)
            {

                try
                {
                    switch (mode)
                    {
                        case "closed":
                            workDaysClosed = new SortedList(weekDaysClosed);
                            if(workDaysClosed.ContainsKey("sat")){
                                workDaysClosed.Remove("sat");
                            }

                            if(workDaysClosed.ContainsKey("sun")){
                                workDaysClosed.Remove("sun");
                            }
                            break;
                        case "24h":
                            workDays24h = new SortedList(weekDays24HourTimes);
                            if (workDays24h.ContainsKey("sat")) {
                                workDays24h.Remove("sat");
                            }

                            if (workDays24h.ContainsKey("sun")) {
                                workDays24h.Remove("sun");
                            }
                            
                            
                            break;
                        case "open":
                            workDaysOpen = new SortedList(weekDaysOpeningTimes);
                            if(workDaysOpen.ContainsKey("sat")){
                                workDaysOpen.Remove("sat");
                            }

                            if(workDaysOpen.ContainsKey("sun")){
                                workDaysOpen.Remove("sun");
                            }
                            
                            
                            break;
                        case "split":
                            workDaysSplit = new SortedList(weekDaysSplitTimes);
                            if(workDaysSplit.ContainsKey("sat")){
                                workDaysSplit.Remove("sat");
                            }

                            if(workDaysSplit.ContainsKey("sun")){
                                workDaysSplit.Remove("sun");
                            }
                            
                            
                            break;

                    }
                }
                catch (ArgumentNullException nullex) { }

            }



        }

        protected void writeLogfile(string status, string message) {

          
            Logger.LogInfos = new Dictionary<string, string>();
            Logger.LogInfos.Add("status", status);
            Logger.LogInfos.Add("directory", BusinessDirectory.directoryname + " / Macro Type: " + macroTypeList[macro_type_selection]);
            Logger.LogInfos.Add("user", User.CurrentUser);
            Logger.LogInfos.Add("customer", this.currentCompany);
            Logger.LogInfos.Add("message", message);

            helper.Logger.writeLogfile();
        }


        /*##### dynamic controller part ###########*/
        protected virtual void iiOpenImacroApp(string macroType)
        {

            string browsertype = "-fx";
            m_app.iimOpen(browsertype, true, 300);

        }
        protected virtual void setiiPlayList(string typeselection) { }

        //entry build lists
        protected virtual void setiiPlayListEntry() { }
        protected virtual void setiiLogoCodePlayEntry() { }
        protected virtual string getiiOpeningHoursWeekdayStringEntry(string day, string mode, string times) { return ""; }
        protected virtual void setiiOpeningHoursCodePlayEntry() {

            if(Logger.LogOpeningHours){
                string message ="Öffnungszeiten: \n";
                foreach (KeyValuePair<string, string> entry in weekdays2timedetails) {
                    message += entry.Key.ToString() + ": " + entry.Value.ToString() + "\n";
                }
                message += "\n";
                message += "MacroCode: " + Logger.Iiplaycode;
            
                this.writeLogfile("debug", message); 
            }
        }
        protected virtual void setiiOpeningHoursCodePlayEntry(ref List<string> processStepsViewList) { }
        protected virtual void setiiGallerypicsCodePlayEntry() { }

        //update build lists
        protected virtual void setiiPlayListUpdate() { }

        //registration build lists
        protected virtual void setiiPlayListRegistration() { }

        //set company profil
        protected virtual void setiiPlayListProfil() { }

        //search build lists
        protected virtual void setiiPlayListSearch() { }

        //macro success status
        protected virtual void setFinalMacroSuccessStatus(string macroType) { }
        /*##### dynamic controller part ###########*/

       

    }
}
