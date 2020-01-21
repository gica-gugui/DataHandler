using System.Collections.Generic;

namespace DataHandler.Data.Models
{
    public class Output
    {
        public Dictionary<string, Header> Headers { get; set; }

        public List<Input> Lines { get; set; }


        
    }
}
