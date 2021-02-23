using System;
using System.IO;

namespace EimiBot2
{
    // Logger class
    class Logger
    {
        public String GetTimestamp() 
        {
            return DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss:fff] ");
        }

        public String GetDate()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        public void Normal(string lines)
        {
            try {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + GetDate() + "-logs.txt", true))
                {
                    string toWrite = GetTimestamp() + lines;
                    Console.WriteLine(toWrite);
                    file.WriteLine(toWrite);
                }
            }
            catch (IOException)
            {

            }
}

        public void Info(string lines)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + GetDate() + "-logs.txt", true))
                {
                    string toWrite = GetTimestamp() + "[INFO] " + lines;
                    Console.WriteLine(toWrite);
                    file.WriteLine(toWrite);
                }
            }
            catch (IOException)
            {

            }
        }

        public void Debug(string lines)
        {
            try 
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + GetDate() + "-logs.txt", true))
                {
                    string toWrite = GetTimestamp() + "[DEBUG] " + lines;
                    Console.WriteLine(toWrite);
                    file.WriteLine(toWrite);
                }
            }
            catch (IOException)
            {

            }
}

        public void Warning(string lines)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + GetDate() + "-logs.txt", true))
                {
                    string toWrite = GetTimestamp() + "[WARN] " + lines;
                    Console.WriteLine(toWrite);
                    file.WriteLine(toWrite);
                }
            }
            catch (IOException)
            {

            }
}

        public void Error(string lines)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + GetDate() + "-logs.txt", true))
                {
                    string toWrite = GetTimestamp() + "[ERROR] " + lines;
                    Console.WriteLine(toWrite);
                    file.WriteLine(toWrite);
                }
            }
            catch (IOException)
            {

            }
}

        public void Fatal(string lines)
        {
            try 
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\logs\" + GetDate() + "-logs.txt", true))
                {
                    string toWrite = GetTimestamp() + "[FATAL] " + lines;
                    Console.WriteLine(toWrite);
                    file.WriteLine(toWrite);
                }
            }
            catch (IOException)
            {

            }
}
    }
}
