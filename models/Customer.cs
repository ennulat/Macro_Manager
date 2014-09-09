using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Macro_Manager.models;

namespace Macro_Manager.models
{

    class Customer
    {
        private List<string> companyNames;
        private List<string[]> data;
        public Customer() {
            readCSV();
        }

        public void readCSV(){
            string[] csvLines = File.ReadAllLines(Config.csvPath);
            List<string> listCsvLines = new List<string>();
            foreach (string l in csvLines)//set only data lines
            {
                if (l.Split(',').Length > 1)
                {
                    listCsvLines.Add(l);
                }
            }


            companyNames = new List<string>();
            data = new List<string[]>();
            for (int i = 0; i < listCsvLines.Count; i++)
            {
                //string[] line = listCsvLines[i].Split(';');
                string[] line = listCsvLines[i].Split(new string[] { "\"," }, StringSplitOptions.None);
                for (int c = 0; c < line.Length; c++)
                {
                    line[c] = line[c].Replace("\"", "");
                }

                data.Add(line);
                companyNames.Add(data[i][3]);
            }   

            
        }

        public List<string[]> GetData()
        {
            return data;
        }

        public List<string> GetCompanyNames()
        {
            return companyNames;
        }
    }
}

   

  