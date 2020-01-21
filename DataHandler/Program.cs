using System;
using DataHandler.Handler;
using DataHandler.Parser;
using DataHandler.Printer;

namespace DataHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var arguments = CmdParser.Parse(args);

                var output = InputParser.Parse(arguments.FilePath);

                output = FilterHandler.Filter(output, arguments);

                OutputPrinter.Print(output);

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }        
    }
}
