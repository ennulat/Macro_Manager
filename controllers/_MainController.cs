using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Macro_Manager.models;
using Macro_Manager.controllers;
using Macro_Manager.views;
using Macro_Manager.helper;
using System.Collections;
using iMacros;
using System.Text.RegularExpressions;
using System.Diagnostics;



using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Logging;
using Google.Apis.Services;
using Google.Apis.Upload;


namespace Macro_Manager
{
    class MainController
    {

        public static Customer customer;
        public MainController(){
            Config config = new Config();
            customer = new Customer();
            Directories.LoadDirectoryList();
            Macro_Manager.models.User.loadUserList();
        }

        public void Main(){


            test();
            //User.CurrentUser = "Johannes";
            console_menu.setConsoleProperties();
            console_menu.userAuthentification();

            int selection = 0;
            bool loop = true;
            do{
                try
                {
                    console_menu.chooseDirectory();
                    selection = int.Parse(Console.ReadLine());
                    if (selection == Directories.DirectoyList.Count)
                        loop = false;

                    switch (selection)
                    {
                        
                        case 0: Console.WriteLine("nicht implementiert\n(weiter mit beliebiger Taste)");
                            Console.ReadKey(true); break; //Gelbeseiten
                        case 1: c_DasOertliche dasOertliche = new c_DasOertliche(customer);
                            dasOertliche.Main();
                            break;
                        case 2: c_Foursquare foursquare = new c_Foursquare(customer);
                            foursquare.Main();
                            break;
                        case 3: c_Yelp yelp = new c_Yelp(customer);
                            yelp.Main();
                            break;
                        case 4: c_DasTelefonbuch dasTelefonbuch = new c_DasTelefonbuch(customer);
                            dasTelefonbuch.Main();
                            break;
                        case 5: c_Meinestadt meinestadt = new c_Meinestadt(customer);
                            meinestadt.Main();
                            break;
                        case 6: c_11880 elf880 = new c_11880(customer);
                            elf880.Main();
                            break;
                        case 7: c_KlickTel klicktel = new c_KlickTel(customer);
                            klicktel.Main();
                            break;
                        case 8: c_Golocal golocal = new c_Golocal(customer);
                            golocal.Main();
                            break;
                        case 9: c_GewusstWo gewusstWo = new c_GewusstWo(customer);
                            gewusstWo.Main();
                            break;
                        case 10: c_Hotfrog hotfrog = new c_Hotfrog(customer);
                            hotfrog.Main();
                            break;
                        case 11: c_Dialo dialo = new c_Dialo(customer);
                            dialo.Main();
                            break;
                        case 12: c_Quicker quicker = new c_Quicker(customer);
                            quicker.Main();
                            break;
                        case 13: c_Pointoo pointoo = new c_Pointoo(customer);
                            pointoo.Main();
                            break;
                        case 14: c_BranchenbuchSuche branchenbuchsuche = new c_BranchenbuchSuche(customer);
                            branchenbuchsuche.Main();
                            break;
                        case 15: c_Tellows tellows = new c_Tellows(customer);
                            tellows.Main();
                            break;
                        case 16: c_Stadtbranchenbuch dasStadtBranchenbuch = new c_Stadtbranchenbuch(customer);
                            dasStadtBranchenbuch.Main();
                            break;
                        case 17: c_BranchenbuchDeutschland branchenbuchDeutschland = new c_BranchenbuchDeutschland(customer);
                            branchenbuchDeutschland.Main();
                            break;
                        case 18: c_Cylex cylex = new c_Cylex(customer);
                            cylex.Main();
                            break;
                        case 19: c_GoYellow goyellow = new c_GoYellow(customer);
                            goyellow.Main();
                            break;
                        case 20: c_MarktplatzMittelstand marktplatzMittelstand = new c_MarktplatzMittelstand(customer);
                            marktplatzMittelstand.Main();
                            break;
                        case 21: c_YellowMap yellowmap = new c_YellowMap(customer);
                            yellowmap.Main();
                            break;
                        case 22: c_Nahklick nahklick = new c_Nahklick(customer);
                            nahklick.Main();
                            break;
                        case 23: c_3Klicks dreiklicks = new c_3Klicks(customer);
                            dreiklicks.Main();
                            break;
                        
                        default: break;
                        //case 18: Console.WriteLine("nicht implementiert\n(weiter mit beliebiger Taste)");
                        //    Console.ReadKey(true); break;
                        

                    }


                }
                catch (FormatException fex) { 
                    //do nothing ..
                }
                catch (Exception e)
                {
                    string message = "Message: " + e.Message + "\n" + "Stacktrace:" + e.StackTrace;
                    Logger.initLogger();
                    Logger.LogInfos.Add("message", message);
                    Logger.LogInfos.Add("status", "error");
                    Logger.LogInfos.Add("user", User.CurrentUser);
                    Logger.LogInfos.Add("customer", "not selected");
                    Logger.LogInfos.Add("directory", "not selected");
                    Logger.writeLogfile();
                    console_menu.displayErrorMessageAndMenuOptions(ref loop, message);
                }

            }while(loop);

            if (!loop)
                Environment.Exit(0);
   
        }

        

        private void test() {


            // Register the authenticator and create the service
            var provider = new Google.Apis.Auth.OAuth2.ClientSecrets();
            provider.ClientId = "397721446430-lgdd1qe1b9jan8t4533haapa117vjodh.apps.googleusercontent.com";
            provider.ClientSecret = "LLCABCCGPpMdenb4ME-CijDz";

          
         

   
            //regex check
            //do
            //{
            //    string pattern = @"(\d)";
            //    string tmp = Console.ReadLine();

            //    Regex _regex = new Regex(pattern);

            //    Match match;
            //    match = _regex.Match(tmp);
            //    if (match.Length > 0) {
            //        Console.WriteLine(match.Value);
            //    }
            //} while (true);

            //config path check
            //Console.WriteLine();
            //Console.WriteLine("config path check:");
            //Console.WriteLine("csv path: " + Config.csvPath);
            //Console.WriteLine("picture path: " + Config.picturePath);
            //Console.WriteLine("macro path: " + Config.macroPath);
            //Console.WriteLine("log path: " + Config.logPath);
            //Console.WriteLine();
            //Console.Write("(irgendeine Taste drücken ... )");
            //Console.ReadKey(true);
        
        }
        
    }
}
