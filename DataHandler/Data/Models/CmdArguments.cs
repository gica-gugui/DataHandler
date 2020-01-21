using DataHandler.Attributes;

namespace DataHandler.Data.Models
{
    public class CmdArguments
    {
        [Flag("-File")]
        public string FilePath { get; set; }

        [Flag("-SortByStartDate", IsRequired = false)]
        public bool SortByDate { get; set; }

        [Flag("-Project", IsRequired = false)]
        public int? Project { get; set; }
    }
}
