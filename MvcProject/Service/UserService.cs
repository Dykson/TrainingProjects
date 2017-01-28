using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using MvcProject.Entity;

namespace MvcProject.Service
{

    class UserService
    {
        public String GetFullName(User[] users)
        {
            if (users != null)
            {
                string result = "\n";

                for (int i = 0; i < users.Length; i++)
                {
                    result += users[i].Firstname + " " + users[i].Lastname + "\n";
                }
                return result;
            }
            return "Пользователь не найден";
        }

        public String GetAllInfo(User[] users)
        {
            if (users != null)
            {
                string result = "\n";

                for (int i = 0; i < users.Length; i++)
                {
                    result += string.Join(" | ", users[i].Dehydrator()) + "\n";
                }
                return result;
            }
            return "Пользователь не найден";
        }

        public User[] SelectUsersByField(string[] rowKey)
        {
            SqliteDBService sqliteDBService = SqliteDBService.GetInstance();

            Hashtable hashtable;
            hashtable = sqliteDBService.DbRequestToGiveData(string
                .Format("SELECT * FROM users WHERE {0} = '{1}'", rowKey[1], rowKey[2])); // Возможна SQL-инъекция

            if (hashtable != null)
            {
                User[] users = new User[hashtable.Count];

                for (int i = 0; i < hashtable.Count; i++)
                {
                    User user = new User();
                    string[] arrayString = (string[])hashtable[i];
                    user.Hydrator(arrayString);
                    users[i] = user;
                }
                return users;
            }
            return null;
        }

        public String DeleteUsersByField(string[] rowKey) // расширить
        {
            SqliteDBService sqliteDBService = SqliteDBService.GetInstance();
            int result = sqliteDBService.DbRequestWithoutGiveData(string.Format("DELETE FROM users WHERE {0} = '{1}'", rowKey[1], rowKey[2]));

            if (result >= 0)
            {
                return string.Format("Удалено пользователей: {0}", result);
            }

            return "Пользователи не найдены";
        }

        public String UpdateFieldByField(string[] rowKey) {return null;}// Расширить
        //{
        //    IDataReader reader = this.service.DbRequestToGiveData(string
        //        .Format("UPDATE users SET {0}='{1}' WHERE {2} = '{3}'", rowKey[3], rowKey[4], rowKey[1], rowKey[2]));

        //    if (reader != null)
        //    {
        //        SqliteDBService.CloseConnection();
        //        return "Поле изменено";
        //    }

        //    return "Ошибка, поле не было изменено";
        //}

        //public UserService select(string tableName){};

        //public UserService where(string key, string value);
        //public UserService andWhere(string key, string value);

        //public String getResult();

        //UserService.select('users').where("id", '4');
    }
}
