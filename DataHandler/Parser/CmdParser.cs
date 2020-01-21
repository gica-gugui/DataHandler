using DataHandler.Attributes;
using DataHandler.Conversion;
using DataHandler.Data.Models;
using System;
using System.Linq;
using System.Reflection;

namespace DataHandler.Parser
{
    public static class CmdParser
    {
        public static CmdArguments Parse(string[] args)
        {
            var cmdArguments = new CmdArguments();

            var properties = typeof(CmdArguments)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FlagAttribute>();
                var flag = args
                    .Select((arg, index) => new { arg, index })
                    .FirstOrDefault(f => f.arg == attribute.Flag);

                string value = null;

                if (flag == null)
                {
                    if (attribute.IsRequired)
                    {
                        throw new Exception($"The flag {attribute.Flag} is required.");
                    }

                    continue;
                }

                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(cmdArguments, true);

                    continue;
                }

                value = args.ElementAtOrDefault(flag.index + 1);

                if (value == null)
                {
                    throw new Exception($"No value was provided for the argument flag {attribute.Flag}.");
                }

                try
                {
                    Convertor.ConvertValue(cmdArguments, property, value);
                }
                catch
                {
                    throw new Exception($"The value {value} is not valid for flag {attribute.Flag}.");
                }
            }

            return cmdArguments;
        }
    }
}
