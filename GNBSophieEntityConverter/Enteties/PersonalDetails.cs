using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GNBSophieEntityConverter.Enteties
{
    public class PersonalDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }

        private const string k_ErrorMsg = "Error in parsing PersonalDetails";

        public PersonalDetails()
        {

        }

        public void ParseFromSophieFile(string path)
        {
            List<String> allLines;
            if (File.Exists(path))
            {

                allLines = new List<String>(File.ReadLines(path));

                if (!allLines[0].Contains("Client Details:")) throw new Exception(k_ErrorMsg + "Unknown Format under `Client Details`");
                ParseFromSophieLine(allLines[1]);
            }
            else
            {
                throw new Exception(k_ErrorMsg + "File not found");
            }
        }

        public void ParseFromSophieLine(string line)
        {
            var splitedLine = new List<String>(line.Split(','));

            if (splitedLine.Count != 3) throw new Exception(k_ErrorMsg + " Wrong number of attribuets");

            if (splitedLine[0].ToLower().Contains("first name:"))
            {
                FirstName = Regex.Replace(splitedLine[0], "First Name: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `first name:`");
            }

            if (splitedLine[1].ToLower().Contains("last name: "))
            {
                LastName = Regex.Replace(splitedLine[1], " last Name: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `last name: `");
            }

            if (splitedLine[2].ToLower().Contains("id: "))
            {
                ID = Regex.Replace(splitedLine[2], " ID: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `id: `");
            }

        }

    }

}
