using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EimiBot2
{
    public class Messages
    {
        private string Guild { get; set; }
        private string Channel { get; set; }
        private int UserId { get; set; }

        private string Author { get; set; }
        private string Timestamp { get; set; }
        private string MessageContent { get; set; }
        private string[] ImageLocations { get; set; }

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
