using System;

namespace DataHandler.Attributes
{
    public class ImportNameAttribute: Attribute
    {
        public string Value { get; }

        public ImportNameAttribute(string value)
        {
            Value = value;
        }
    }
}
