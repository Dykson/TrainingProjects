using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MvcProject.View;
using MvcProject.Service;

namespace MvcProject.Controller
{
    class AppController
    {
        private static AppController instance = null;
        public static AppController Instantiate()
        {
            if (AppController.instance == null)
            {
                AppController.instance = new AppController();
            }

            return AppController.instance;
        }

        private static string[] commands = new string[] 
        { 
            "show full name", "show all info", "update", "delete",
            "exit", "quit", "print", "select", "insert", "help"
        };

        private static bool Verification(string userInput)
        {
            for (int i = 0; i < AppController.commands.Length; i++)
			{
                if (userInput.StartsWith(AppController.commands[i]))
	            {
		            return true;
	            }
			}
            return false;
        }

        private string[] ConvertToCommand(string userInput)
        {
            if (userInput.StartsWith("print "))
            {
                if (userInput.Length >= 6) return new string[2] { "print", userInput.Substring(6) };
                return new string[1] { "print" };
            }
            if (AppController.Verification(userInput))
            {
                string[] separator = new string[] { " /", "=", "," };
                return userInput.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            }
            return null;
        }
        static void HelpCommand()
        {
            ViaConsole.WriteResponse(
@"
show full name \id=2          - показать полное имя пользователя с id=2
show all info \id=2          - показать всю информацию о пользователе с id=2
exit - выход без подтверждения
quit - выход с подтверждением
print \Hello world - напечатать текст Hello world
enable echo - включить эхо сервис
disable echo - выключить эхо сервис
insert - добавить новую запись в таблицу Users
help - список команд
", false);
        }
        void ShowUsers(string[] command, bool allInfo)
        {
            UserService userService = new UserService();

            while (true)
            {
                if (command.Length == 3)
                {
                    if (allInfo)
                    {
                    ViaConsole.WriteResponse(userService.GetAllInfo(userService.SelectUsersByField(command)), false);                        
                    }
                    else ViaConsole.WriteResponse(userService.GetFullName(userService.SelectUsersByField(command)), false);                        
                    return;
                }
                if (command.Length == 1)
                {
                    ViaConsole.WriteResponse("Введите ключ в формате field=value", false);
                    string key = ViaConsole.ReadLine();
                    string userInput = "show full name" + @" /" + key;
                    command = this.ConvertToCommand(userInput);
                    continue;
                }
                else
                {
                    ViaConsole.WriteResponse("Некорректный ключ!", false);
                    command = new string[1];
                    continue;
                }
            }
        }
        void Print(string[] command)
        {
            while (true)
            {
                if (command.Length == 2)
                {
                    ViaConsole.WriteResponse(command[1], false);
                    return;
                }
                if (command.Length == 1)
                {
                    ViaConsole.WriteResponse("Введите текст, который необходимо напечатать:", false);
                    string text = ViaConsole.ReadLine();
                    ViaConsole.WriteResponse(text, false);
                    return;
                }
                else
                {
                    ViaConsole.WriteResponse("Некорректный ключ!", false);
                    command = new string[1];
                    continue;
                }
            }
        }


        void UpdateField(string[] command)
        {
            UserService userService = new UserService();
            while (true)
            {
                if (command.Length == 5)
                {
                    ViaConsole.WriteResponse(userService.UpdateFieldByField(command), false);
                    return;
                }
                if (command.Length == 1)
                {
                    ViaConsole.WriteResponse("Введите ключ в формате field=value,field=newValue", false);
                    string key = ViaConsole.ReadLine();
                    string userInput = "update password" + @" /" + key;
                    command = this.ConvertToCommand(userInput);
                    continue;
                }
                else
                {
                    ViaConsole.WriteResponse("Некорректный ключ!", false);
                    command = new string[1];
                    continue;
                }
            }
        }

        public int Loop()
        {
            bool isEnableEcho = false;
            string userInput;

            while (true)
            {
                userInput = ViaConsole.ReadLine();

                #region  Echo service
                if (userInput == "enable echo" & !isEnableEcho)
                {
                    isEnableEcho = true;
                    ViaConsole.WriteResponse("Эхо сервис включен", false);
                    continue;
                }
                if (userInput == "disable echo" & isEnableEcho)
                {
                    isEnableEcho = false;
                    ViaConsole.WriteResponse("Эхо сервис выключен", false);
                    continue;
                }
                if (isEnableEcho)
                {
                    ViaConsole.WriteResponse(userInput, false);
                    continue;
                }
                #endregion

                string[] command = this.ConvertToCommand(userInput);

                #region UncorrectInput
                if (command == null)
                {
                    ViaConsole.WriteResponse("Такой команды не существует", false);
                    AppController.HelpCommand();
                    continue;
                }
                #endregion

                switch (command[0])
                {
                    case "show full name": this.ShowUsers(command, false); break;
                    case "show all info": this.ShowUsers(command, true); break;
                    case "update": this.UpdateField(command); break;
                    case "delete":  break;
                    #region Переделать insert
                    //case "insert": 
                    //    {
                    //            ViaConsole.WriteResponse("Введите логин:", true);
                    //            string login = ViaConsole.ReadCommand().ToLower();
                    //            ViaConsole.WriteResponse("Введите пароль:", true);
                    //            string password = ViaConsole.ReadCommand().ToLower();
                    //            ViaConsole.WriteResponse("Введите имя:", true);
                    //            string firstname = ViaConsole.ReadCommand().ToLower();
                    //            ViaConsole.WriteResponse("Введите фамилию:", true);
                    //            string lastname = ViaConsole.ReadCommand().ToLower();
                    //            ViaConsole.WriteResponse("Введите страну:", true);
                    //            string position = ViaConsole.ReadCommand().ToLower();
                    //            ViaConsole.WriteResponse(SqliteDBService.DBrequest(string
                    //                .Format("INSERT INTO users ('login', 'password', 'firstname', 'lastname', 'position')" + 
                    //                "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');", login, password, firstname, lastname, position)), false);
                    //            ViaConsole.WriteResponse("Запись успешно добавилась в базу данных", false);
                    //            break;
                    //    } 
                    #endregion
                    case "print": this.Print(command); break;
                    case "help": AppController.HelpCommand(); break;
                    case "exit": return 0;
                    case "quit":
                        {
                            #region QuitCommand
                            ViaConsole.WriteResponse("Выйти из программы? Y - да; N - нет:", true);
                            userInput = ViaConsole.ReadLine();

                            if (userInput.ToUpper() == "Y")
                            {
                                return 0;
                            }
                            else if (userInput.ToUpper() == "N")
                            {
                                break;
                            }
                            else 
                            {
                                ViaConsole.WriteResponse("Некорректный ввод.", false);
                                goto case "quit";
                            }
                            #endregion
                        }
                }
            }
        }
    }
}
