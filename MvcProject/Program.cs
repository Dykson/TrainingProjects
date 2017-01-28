using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcProject.Controller;
using MvcProject.View;

namespace MvcProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ViaConsole.WriteResponse("Добро пожаловать. Эхо сервис выключен", false);
            AppController appController = AppController.Instantiate();
            appController.Loop();
        }
    }
}
