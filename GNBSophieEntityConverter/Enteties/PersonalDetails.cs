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

        public PersonalDetails()
        {

        }

        public void ParseFromSophieFile(string path)
        {
            List<String> allLines;
            if (File.Exists(path))
            {

                allLines = new List<String>(File.ReadLines(path));

                if (!allLines[0].Contains("Client Details:")) throw new Exception("Unknown Format under `Client Details`");

                var splitedLine = new List<String>(allLines[1].Split(','));

                if (splitedLine.Count != 3) throw new Exception("Unknown Format under `Client Details`");

                if (splitedLine[0].ToLower().Contains("first name:"))
                {
                    FirstName = Regex.Replace(splitedLine[0], "First Name: ", String.Empty, RegexOptions.IgnoreCase);
                }

                if (splitedLine[1].ToLower().Contains("last name: "))
                {
                    LastName = Regex.Replace(splitedLine[1], " last Name: ", String.Empty, RegexOptions.IgnoreCase);
                }

                if (splitedLine[2].ToLower().Contains("id: "))
                {
                    ID = Regex.Replace(splitedLine[2], " ID: ", String.Empty, RegexOptions.IgnoreCase);
                }

            }
            else
            {
                throw new Exception("File not found");
            }
        }

        public void ParseFromSophieLine(string line)
        {
            var splitedLine = new List<String>(line.Split(','));

            if (splitedLine.Count != 3) throw new Exception("Unknown Format under `Client Details`");

            if (splitedLine[0].ToLower().Contains("first name:"))
            {
                FirstName = Regex.Replace(splitedLine[0], "First Name: ", String.Empty, RegexOptions.IgnoreCase);
            }

            if (splitedLine[1].ToLower().Contains("last name: "))
            {
                LastName = Regex.Replace(splitedLine[1], " last Name: ", String.Empty, RegexOptions.IgnoreCase);
            }

            if (splitedLine[2].ToLower().Contains("id: "))
            {
                ID = Regex.Replace(splitedLine[2], " ID: ", String.Empty, RegexOptions.IgnoreCase);
            }

        }

    }

}
