using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Macro_Manager.models;
using Macro_Manager.controllers;
using System.Collections;

namespace Macro_Manager.views
{
    public static class console_menu
    {
        static public void setConsoleProperties() {
            Console.SetWindowSize(Console.WindowWidth - 20, Console.WindowHeight * 2);
            Console.CursorVisible = true;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
        
        }
        
        static public void userAuthentification() {
          
            bool authenticated = false;
            do
            {

                Console.Clear();
                Console.WriteLine("============= Benutzeranmeldung =============");
                Console.WriteLine();
                Console.Write("Eingabe Benutzername: ... ");
                string username = Console.ReadLine();
                Console.Write("\nEingabe Passwort: ... ");
                string password = Console.ReadLine();

                foreach (DictionaryEntry user in User.UserList)
                {

                    authenticated = false;
                    if (username == user.Key.ToString())
                    {
                        if (password == user.Value.ToString())
                        {
                            authenticated = true;
                            User.CurrentUser = username;
                            break;
                        }

                    }

                }
            } while (!authenticated);
        
        }

        static public void chooseDirectory()
        {
            Console.Clear();
            Console.WriteLine("============= Verzeichnisauswahl =============");
            Console.WriteLine();
            int i = 0;
            for (i = 0; i < Directories.DirectoyList.Count; i++)
            {
                Console.WriteLine("[" + i + "]  " + Directories.DirectoyList[i].ToString());
            }
            Console.WriteLine("[" + i + "]   Exit");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Eingabe ... ");
        }

        static public void customerMenu() {
            Console.Clear();
            Console.WriteLine("============= Kundenauswahl (" + BusinessDirectory.directoryname + ") =============");
            Console.WriteLine();
            int i;
            for (i = 0; i < BusinessDirectory.companyNames.Count; i++)
            {
                Console.WriteLine("[" + i + "] " + BusinessDirectory.companyNames[i].ToString());
            }

            Console.WriteLine("[a] csv erneut einlesen");

            if (BusinessDirectory.macroTypeList.Count > 1)
            {
                //Console.WriteLine("[" + i + "] zur Macro Typ Auswahl");
                Console.WriteLine("[b] zur Macro Typ Auswahl");
                //i++;
            }

            //Console.WriteLine("[" + i + "] zur Verzeichnisauswahl");
            //Console.WriteLine("[" + i + "] zur Verzeichnisauswahl");
            Console.WriteLine("[c] zur Verzeichnisauswahl");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Eingabe ... ");
      
        
        }

        static public void macroMenu(ref int macro_type_selection, ref bool loopMacroMenu)
        {
           
            Console.Clear();
            Console.WriteLine("============= Macro Typ Auswahl (" + BusinessDirectory.directoryname + ") =============");
            Console.WriteLine();
            Console.WriteLine("Typ:");
            Console.WriteLine();

            int i;
            for (i = 0; i < BusinessDirectory.macroTypeList.Count; i++)
            {
                Console.WriteLine("[" + i + "] " + BusinessDirectory.macroTypeList[i].ToString());
            }
            Console.WriteLine("[" + i + "] zur Verzeichnisauswahl");

            macro_type_selection = int.Parse(Console.ReadLine());
            if (macro_type_selection >= BusinessDirectory.macroTypeList.Count)
                loopMacroMenu = false;
        }



        static private List<string> processSteps;
        static public List<string> GetProcessStepsList()
        {
            if(processSteps == null){
                processSteps = new List<string>();
            }
            return processSteps;
        }

        static public void SetProcessStepsList(List<string> list) {
            processSteps = new List<string>(list);
        
        }

        static public void ClearProcessStepsList()
        {
            if (processSteps == null)
            {
                processSteps = new List<string>();
            }
            else
                processSteps.Clear();
            
        }

        static public void displayMacroProcessHeader(int selection) {
            Console.Clear();
            Console.Write("Starte Macro mit Firma:" + BusinessDirectory.companyNames[selection] + "\n(beliebige Taste drücken) ... ");
            Console.ReadKey(true);
            Console.Write("\n\n");
        }

        static public void displayMacroProcessStep(int step) {

            if (processSteps[step] != null) {
                Console.WriteLine("-passiere: " + processSteps[step].ToString());
            }
        
        }

        static public void displayMacroProcessFooter(ref int macro_step, ref int customer_selection, ref bool loopMacroMenu, ref bool loopCustomerMenu, ref bool loopCompany) {
            //Console.Write("Eintrag erfolgreich!\n\nweiter mit return ... ");
            Console.Write("\nMacro erfolgreich abgeschlossen!\n");
            Console.Write("weiter (beliebige Taste drücken) ... ");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("============= Abschluss Menu (" + BusinessDirectory.directoryname + ") =============\n\n");
            if (customer_selection < (BusinessDirectory.companyNames.Count) -1)
            {
                Console.WriteLine("[a] nächsten Kunden eintragen (" + BusinessDirectory.companyNames[customer_selection + 1] + ")");
            }
            Console.WriteLine("[b] zurück zur Kundenauswahl");
            if (BusinessDirectory.macroTypeList.Count > 1) {
                Console.WriteLine("[c] zurück zur Macro Typ Auswahl");
            }
            
            Console.WriteLine("[d] zurück zur Verzeichnisauswahl");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Eingabe ... "); 


            switch (Console.ReadLine())
            {
                case "a": if (customer_selection < (BusinessDirectory.companyNames.Count) - 1)
                          {
                                macro_step = 0;    
                                customer_selection++;
                                Console.Clear();
                                console_menu.displayMacroProcessHeader(customer_selection);
                                
                          }
                          break;
                case "b": loopCompany = false; Console.Clear(); break;
                case "c": loopCustomerMenu = false; loopCompany = false; Console.Clear(); break;
                case "d": loopCompany = false; loopCustomerMenu = false; loopMacroMenu = false; Console.Clear(); break;
                default: loopCompany = false; Console.Clear(); break;
            }

            
            
        }

        static public void displayErrorMessageAndMenuOptions(ref int macro_step, ref bool loopMacroMenu, ref bool loopCustomerMenu, ref bool loopCompany, int customer_selection, string logMessage)
        { 
            Console.Write("\n\n");
            if(logMessage.Contains("Stacktrace")){
                Console.WriteLine("Error durch Exception Handling, Details siehe Logfile:\n " + Config.logPath + "\n\n");
            }else{
                Console.WriteLine(logMessage + "\n\n");
            }
            Console.Write("weiter (beliebige Taste drücken) ... ");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("============= Fehler Menu (" + BusinessDirectory.directoryname + ") =============\n\n");


            Console.WriteLine("[a] Macro für Kunden: " + BusinessDirectory.companyNames[customer_selection] + " erneut abspielen");
            Console.WriteLine("[b] zurück zur Kundenauswahl");
            if (BusinessDirectory.macroTypeList.Count > 1)
            {
                Console.WriteLine("[c] zurück zur Macro Typ Auswahl");
            }
            Console.WriteLine("[d] zurück zur Verzeichnisauswahl");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Eingabe ... ");


            switch (Console.ReadLine())
            {
                case "a": macro_step = 0; break;
                case "b": loopCompany = false; break;
                case "c": loopCustomerMenu = false; loopCompany = false; break;
                case "d": loopCustomerMenu = false; loopMacroMenu = false; loopCompany = false; break;
                default: loopCompany = false; break;
            }

            Console.Clear();
        }

        
        static public void displayErrorMessageAndMenuOptions(ref bool loop, string logMessage)
        {
            Console.Write("\n\n");
            if (logMessage.Contains("Stacktrace"))
            {
                Console.WriteLine("Error durch Exception Handling, Details siehe Logfile:\n " + Config.logPath + "\n\n");
            }
            else
            {
                Console.WriteLine(logMessage + "\n\n");
            }

            Console.Write("weiter (beliebige Taste drücken) ... ");
            Console.ReadKey(true);
            Console.Clear();
            Console.WriteLine("============= Fehler Menu =============\n\n");
            Console.WriteLine("[a] zurück zur Verzeichnisauswahl");
            Console.WriteLine("[b] Programm beenden");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Eingabe ... ");

            switch (Console.ReadLine())
            {
                case "a": break;
                case "b": loop = false; break;
                default: break;
            }

            Console.Clear();
        }

        static public void displayErrorMessage(string logMessage) {
            Console.Clear();
            Console.WriteLine("Error:");
            Console.WriteLine();
            Console.WriteLine(logMessage);
            Console.WriteLine();
            Console.WriteLine("Errorlog wurde erstellt unter: " + Config.logPath + "\n\n");
            Console.Read();
            Environment.Exit(0);
        }
    }
}
