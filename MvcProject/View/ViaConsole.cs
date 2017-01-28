using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcProject.Controller;

namespace MvcProject.View
{
    static class ViaConsole
    {
        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static void WriteResponse(string response, bool enableLine)
        {
            if (enableLine)
            {
                Console.Write(response);
            }
            else Console.WriteLine(response);
        }
        
    }
}
