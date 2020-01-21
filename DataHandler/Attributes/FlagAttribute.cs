using System;

namespace DataHandler.Attributes
{
    public class FlagAttribute: Attribute
    {
        public string Flag { get; }

        public bool IsRequired { get; set; }

        public FlagAttribute(string flag, bool isRequired = true)
        {
            Flag = flag;
            IsRequired = isRequired;
        }
    }
}
