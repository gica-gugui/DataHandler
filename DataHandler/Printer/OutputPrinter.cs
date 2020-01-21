using DataHandler.Data.Models;
using DataHandler.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DataHandler.Printer
{
    public static class OutputPrinter
    {
        public static void Print(Output output)
        {
            PrintHeaders(output.Headers);

            PrintLines(output);
        }

        private static void PrintHeaders(Dictionary<string, Header> headers)
        {
            var descriptions = headers.Select(h => h.Value.Description.Trim());
            var headersOutput = string.Join(Constants.SEPARATOR, descriptions);

            Console.WriteLine(headersOutput);
        }

        private static void PrintLines(Output output)
        {
            foreach (var line in output.Lines)
            {
                var properties = line.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .OrderBy(p => output.Headers[p.Name].Index);

                var descriptions = properties.Select(p =>
                {
                    var result = p.GetValue(line);
                    if (result == null)
                    {
                        return string.Empty;
                    }

                    if (p.PropertyType == typeof(DateTime))
                    {
                        result = ((DateTime)result).ToString(Constants.FORMAT);
                    }

                    return result;
                });

                var linesOutput = string.Join(Constants.SEPARATOR, descriptions);

                Console.WriteLine(linesOutput);
            }
        }
    }
}
