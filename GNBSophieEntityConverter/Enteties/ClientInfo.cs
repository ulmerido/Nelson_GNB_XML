using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace GNBSophieEntityConverter.Enteties
{
    public class ClientInfo
    {
        public PersonalDetails PersonalDetails { get; set; }
        public List<Transaction> Transactions { get; set; }

        public ClientInfo() { }
        public ClientInfo(string path)
        {
            Transactions = new List<Transaction>();
            if (!String.IsNullOrEmpty(path)) ParseFromFile(path);
        }

        void ParseFromFile(string path)
        {
            List<String> allLines;
            if (File.Exists(path))
            {
                allLines = new List<String>(File.ReadLines(path));

                if (!allLines[0].Contains("Client Details:")) throw new Exception("Unknown Format under `Client Details`");

                PersonalDetails = new PersonalDetails();
                PersonalDetails.ParseFromSophieLine(allLines[1]);
                for (int i = 5; i < allLines.Count; i++)
                {
                    var x = new Transaction(allLines[i]);
                    Transactions.Add(x);
                }
            }
        }

        public string ToXML(XmlWriterSettings settings = null)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(ClientInfo));
            StringBuilder stringbuildXml = new StringBuilder();
            var xml = "";
            if(settings == null)
            {
                settings = new XmlWriterSettings();
                settings.Encoding = Encoding.GetEncoding("WINDOWS-1252");
            }
            using (XmlWriter writer = XmlWriter.Create("out.xml", settings))
            {
                xsSubmit.Serialize(writer, this);
                xml = stringbuildXml.ToString(); // Your XML
            }

            return xml;
        }

    }
}
