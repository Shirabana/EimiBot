using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EimiBot2
{
    public class Messages
    {
        public string Guild { get; set; }
        public string Channel { get; set; }
        public int UserId { get; set; }

        public string Author { get; set; }
        public string Timestamp { get; set; }
        public string MessageContent { get; set; }
        public string[] ImageLocations { get; set; }

        public Messages()
        {
            // Empty constructor
        }

        public Messages(string Author, string Timestamp, string MessageContent)
        {
            this.Author = Author;
            this.Timestamp = Timestamp;
            this.MessageContent = MessageContent;
        }
    }
}
