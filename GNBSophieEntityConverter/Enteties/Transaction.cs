using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GNBSophieEntityConverter.Enteties
{
    public class Transaction
    {
        private const string k_ErrorMsg = "Error in parsing Transaction";

        public string ID { get; set; }

        public string Type { get; set; }

        public string amount { get; set; }

        public string currency { get; set; }

        public string Balance { get; set; }

        public Transaction()
        {
        }

        public Transaction(string line = null)
        {
            if (!String.IsNullOrEmpty(line)) ParseFromSophieLine(line);
        }

        public void ParseFromSophieLine(string line)
        {
            var splitedLine = new List<String>(line.Split(','));
            
            if (splitedLine[0].ToLower().Contains("id: "))
            {
                ID = Regex.Replace(splitedLine[0], "ID: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `id: `");
            }

            if (splitedLine[1].ToLower().Contains("type: "))
            {
                Type = Regex.Replace(splitedLine[1], " Type: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `Type: `");
            }

            if (splitedLine[2].ToLower().Contains("amount: "))
            {
                amount = Regex.Replace(splitedLine[2], " Amount: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `amount: `");
            }

            if (splitedLine[3].ToLower().Contains("currency: "))
            {
                currency = Regex.Replace(splitedLine[3], " Currency: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `currency: `");
            }

            if (splitedLine[4].ToLower().Contains("balance: "))
            {
                Balance = Regex.Replace(splitedLine[4], " Balance: ", String.Empty, RegexOptions.IgnoreCase);
            }
            else
            {
                throw new Exception(k_ErrorMsg + " no match for `balance: `");
            }
        }

    }
}
