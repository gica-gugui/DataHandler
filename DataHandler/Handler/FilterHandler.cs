using DataHandler.Data.Models;
using System.Linq;

namespace DataHandler.Handler
{
    public class FilterHandler
    {
        public static Output Filter(Output output, CmdArguments args)
        {
            var result = new Output
            {
                Headers = output.Headers
            };

            var lines = output.Lines;

            if (args.Project != null)
            {
                lines = lines.Where(l => l.Project == args.Project.Value)
                    .ToList();
            }

            if (args.SortByDate)
            {
                lines = lines.OrderBy(l => l.StartDate)
                    .ToList();
            }

            result.Lines = lines;

            return result;
        }
    }
}
