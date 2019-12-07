
namespace GBNEntiyConvertConsoleProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new ConvertConsoleProgram();
            foreach (var str in args) System.Console.WriteLine(str);
            try
            {
                program.Run(args);

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.ReadLine();
        }
    }
}
