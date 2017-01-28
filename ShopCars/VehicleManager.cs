using System;
using System.IO;

namespace ShopCars
{
    class VehicleManager
    {
        private string path;
        public void SaveCar(Vehicle vehicle)
        {
                string tempState = vehicle.Serialize();

                if (File.ReadAllText(path).IndexOf(tempState) == -1)
                {
                    using (StreamWriter file = new StreamWriter(this.path, true))
                    {
                        file.Write(tempState + "\r");
                    }
                    Console.WriteLine("Состояние успешно сохранено");
                }
                else
                {
                    Console.WriteLine("Данное состояние средства передвижения было ранее сохранено");
                }
        }

        public Vehicle LoadCarByName(string name)
        {
            string file = File.ReadAllText(path);
            string[] unit = file.Split(new char[]{'\r'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in unit)   
            {

                if (line.IndexOf("Name=" + name) != -1)
                {
                    if (line.IndexOf("ShopCars.Automobile") == 1)
                    {
                        return null;//new VehicleFactory(
                            //lines[i].Substring(4), 10,
                            //Convert.ToInt32(lines[i].Substring(4)),
                            //Convert.ToInt32(substring[8])).CreateAutomobile(
                            //    name,
                            //    substring[2],
                            //    Convert.ToSingle(substring[12]),
                            //    Convert.ToInt32(substring[4]));
                    }
                    if (line.IndexOf("ShopCars.Bicycle") == 1)
                    {
                        return null; // new VehicleFactory(name, Convert.ToInt32(substring[10]), Convert.ToInt32(substring[4]), Convert.ToInt32(substring[6])).CreateBicycle(name, Convert.ToInt32(substring[10]), Convert.ToInt32(substring[2]));
                    }
                }
            }
            Console.WriteLine("Средства передвижения с данным именем не существует.");

            return null;
        }

        public VehicleManager(string path)
        {
            this.path = path;
        }
    }
}
