using DataHandler.Attributes;
using DataHandler.Conversion;
using DataHandler.Data.Models;
using DataHandler.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DataHandler.Parser
{
    public class InputParser
    {
        public static Output Parse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception("No file was found at the provided input file path.");
            }

            using (var sr = new StreamReader(filePath))
            {
                var headers = ParseHeaders(sr);

                if (headers == null)
                {
                    throw new Exception("The input file is empty.");
                }

                var lines = ParseInput(sr, headers);

                return new Output { Headers = headers, Lines = lines };
            }
        }

        private static Dictionary<string, Header> ParseHeaders(StreamReader sr)
        {
            while (sr.Peek() > -1)
            {
                var line = sr.ReadLine();

                if (IsEmpty(line))
                {
                    continue;
                }

                return ParseHeadersLine(line);
            }

            return null;
        }

        private static Dictionary<string, Header> ParseHeadersLine(string line)
        {
            var headers = new Dictionary<string, Header>();

            var properties = typeof(Input)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ImportNameAttribute>();
                var headerDescription = attribute != null ? attribute.Value : property.Name;

                var index = line.IndexOf(headerDescription);

                if (index < 0)
                {
                    throw new Exception($"The header must have a column named {headerDescription}.");
                }

                if (index != line.LastIndexOf(headerDescription))
                {
                    throw new Exception($"The header has a duplicate column named {headerDescription}.");
                }

                headers.Add(property.Name, new Header { Index = index, Description = headerDescription });
            }

            return headers
                .OrderBy(c => c.Value.Index)
                .Select((c, index) => new KeyValuePair<string, Header>(c.Key, new Header() { Index = index, Description = c.Value.Description }))
                .ToDictionary(c => c.Key, c => c.Value);
        }

        private static List<Input> ParseInput(StreamReader sr, Dictionary<string, Header> headers)
        {
            var lines = new List<Input>();

            while (sr.Peek() > -1)
            {
                var line = sr.ReadLine();

                if (IsEmpty(line))
                {
                    continue;
                }

                lines.Add(ParseInputLine(line, headers));
            }

            return lines;
        }

        private static Input ParseInputLine(string line, Dictionary<string, Header> headers)
        {
            var fragments = line.Split(Constants.SEPARATOR)
                .Select(f => {
                    if (f == Constants.NULL)
                    {
                        return null;
                    }

                    return f;
                })
                .ToArray();

            if (fragments.Length != headers.Count)
            {
                throw new Exception("The input line has a different number of values than the header.");
            }

            var input = new Input();

            var properties = typeof(Input)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                var headerIndex = headers[property.Name];

                try
                {
                    Convertor.ConvertValue(input, property, fragments[headerIndex.Index]);
                }
                catch
                {
                    throw new Exception($"The value {fragments[headerIndex.Index]} is not valid for column {headerIndex.Description}.");
                }
            }

            return input;
        }

        private static bool IsEmpty(string line)
        {
            return string.IsNullOrWhiteSpace(line) || line.StartsWith('#');
        }
    }
}
