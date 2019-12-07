using System;
using System.IO;
using GNBSophieEntityConverter;

namespace GBNEntiyConvertConsoleProgram
{
    class ConvertConsoleProgram
    {
        private Converter mConverter = new Converter();

        public ConvertConsoleProgram()
        {

        }

        public void Run(string[] args)
        {
            if (args.Length > 4) throw new Exception("Wrong number of arguments");
            if(args.Length<1)
            {
                runWithLoop();
            }
            else
            {
                var converter = new Converter();
                string inputPath = (args.Length >= 1) ? args[0] : null;
                string outputPath = (args.Length >= 2) ? args[1] : null;
                string outFileName = (args.Length >= 3) ? args[2] : null;
                string encoding = (args.Length >= 4) ? args[3] : "WINDOWS-1252";

                if (outputPath != null && !outputPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    outputPath += Path.DirectorySeparatorChar ;

                if (!File.Exists(inputPath)) throw new Exception("Cant find input file");

                converter.Convert(inputPath,outputPath,outFileName,encoding);
            }

        }

        private void runWithLoop()
        {
            bool continueScript = true;
            var converter = new Converter();

            while (continueScript)
            {
                Console.WriteLine(@"
Please choose:
    1. Enter '1' to insert a single file to convert.
    2. Enter '2' to insert list of files to convert from antoher file.
    E. Enter 'E' to Exit");

                string option = Console.ReadLine();
                Console.WriteLine();

                if (option.ToLower().Equals("e"))
                {
                    Console.WriteLine(Environment.NewLine+"BY!"+ Environment.NewLine);
                    break;
                }

                if (option.ToLower().Equals("1"))
                {
                    inputFromConsole();
                }

                if (option.ToLower().Equals("2"))
                {
                    inputFromFile();
                }

              
            }
        }

        private string getFilePath()
        {
            Console.WriteLine("Enter input file path");
            string inputPath = Console.ReadLine();

            if (!File.Exists(inputPath))
            {
                Console.WriteLine("\tCant find input file");
                Console.WriteLine("\tCheck if input path is vaild and try again");
                return null;
            }

            return inputPath;
        }

        private void inputFromConsole()
        {
            string inputPath = getFilePath();
            if (inputPath == null) return;
            mConverter.Convert(inputPath);
        }

        private void inputFromFile()
        {
            string inputPath = getFilePath();
            if (inputPath == null) return;
            var allLines = File.ReadAllLines(inputPath);
            foreach(String line in allLines)
            {
                var args = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (args.Length > 0)
                {
                    Console.WriteLine(Environment.NewLine + "Converting...");
                    Console.WriteLine("Source     \t[" + args[0] + "]");
                    if (args.Length > 1)
                        Console.WriteLine("Destination\t[" + args[1] + "]");
                    else
                        Console.WriteLine("Destination\t[" + Directory.GetCurrentDirectory() +"]");

                    Run(args);
                    Console.WriteLine("Done!");

                }
            }
        }
    }
}
