using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LivePerformance2
{
    public static class Log
    {
        
        public static void Error(string error)
        {
            Write("!!!");
            Write("Error:: " + error);
            Write("!!!");
        }

        public static void Warning(string warning)
        {
            Write("=!=");
            Write("Warning:: " + warning);
            Write("=!=");
        }

        public static void Information(string information)
        {
            Write("===");
            Write("Info:: " + information);
            Write("===");
        }

        private static void Write(string text)
        {
            System.Diagnostics.Debug.WriteLine(text); //could be better.. but theyre flipping standards here
            //every few months. I'd rather stay neutral and retain the option to use whatever they settle on.
        }
    }
}
