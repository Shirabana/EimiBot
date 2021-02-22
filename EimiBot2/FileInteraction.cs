using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EimiBot2
{
    class FileInteraction
    {
        public String GetTimestamp()
        {
            return DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss:fff] ");
        }

        public String GetDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        public void Logged(string lines)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + GetDate() + "-deletionlogs.txt", true))
            {
                string toWrite = lines;
                Console.WriteLine(toWrite);
                file.WriteLine(toWrite);
            }
        }
    }
}
