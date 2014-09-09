using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Macro_Manager.models
{
    public static class Directories
    {
        private static List<string> directoyList;
        public static List<string> DirectoyList
        {
            get { return Directories.directoyList; }
            set { Directories.directoyList = value; }
        }

        public static void  LoadDirectoryList()
        {
            if (DirectoyList == null) {
                directoyList = new List<string>();
            }


            //directoyList.Add("_Branchebuchsuche");
            //directoyList.Add("_Tellows");
            //directoyList.Add("Stadtbranchenbuch");
            //directoyList.Add("DasOertliche");
            //directoyList.Add("Foursquare");
            //directoyList.Add("Yelp");
            //directoyList.Add("_11880");
            //directoyList.Add("KlickTel");
            //directoyList.Add("Gewusst-Wo");
            //directoyList.Add("_GoLocal");
            //directoyList.Add("DasTelefonbuch");
            //directoyList.Add("MeineStadt");
            //directoyList.Add("3Klicks");
            //directoyList.Add("_Hotfrog"); //Eintrag => OK, UPDATE => ausstehend
            //directoyList.Add("_Dialo");
            //directoyList.Add("_Pointoo");
            //directoyList.Add("Gelbeseiten");
            //directoyList.Add("Quicker");
            //directoyList.Add("AroundMe");
            //directoyList.Add("NahKlick");
            //directoyList.Add("_BranchenbuchDeutschland");
            //directoyList.Add("Marktplatz-Mittelstand");
            //directoyList.Add("Cylex");
            //directoyList.Add("YellowMap");
            //directoyList.Add("GoYellow");
            //directoyList.Add("Test Controller");

            directoyList.Add("Gelbeseiten");
            directoyList.Add("DasOertliche");
            directoyList.Add("Foursquare");
            directoyList.Add("Yelp");
            directoyList.Add("DasTelefonbuch");
            directoyList.Add("MeineStadt");
            directoyList.Add("_11880");
            directoyList.Add("KlickTel");
            directoyList.Add("_GoLocal");
            directoyList.Add("Gewusst-Wo");
            directoyList.Add("_Hotfrog"); //Eintrag => OK, UPDATE => ausstehend
            directoyList.Add("_Dialo");
            directoyList.Add("Quicker");
            directoyList.Add("_Pointoo");
            directoyList.Add("_Branchebuchsuche");
            directoyList.Add("_Tellows");
            directoyList.Add("Stadtbranchenbuch");
            directoyList.Add("_BranchenbuchDeutschland");
            directoyList.Add("Cylex");
            directoyList.Add("GoYellow");
            directoyList.Add("Marktplatz-Mittelstand");
            directoyList.Add("YellowMap");
            directoyList.Add("NahKlick");
            directoyList.Add("3Klicks"); 
            directoyList.Add("Wer Liefert Was");         
            directoyList.Add("Test Controller");

            
        }
    }
}
