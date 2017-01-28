﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;

namespace ShopCars
{
    interface ISerializable // возможность сериализовывать, рассказывать своё текущее состояние одной строкой
    {
        string Serialize();
    }

    interface IDrivable // возможность двигаться к определённой локации
    {
        void DriveTo(ILocation location);
    }

    interface ILocation // Локация - собирательное понятие из точки на карте,
    {                   // имеющую координаты х и у, и названия точки
        Coordinates MyCoordinates { get; set; } // Точка на карте
        string Name { get; set; } // Имя точки
    }

    struct Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Coordinates(int x, int y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }
    }
    class Location : ILocation
    {
        public Coordinates MyCoordinates { get; set; }
        public string Name { get; set; }
        public Location(string name, int x, int y)
        {
            this.Name = name;
            this.MyCoordinates = new Coordinates(x, y);
        }
    }

    abstract class Vehicle : IDrivable, ISerializable  // класс Средство передвижения
    {
        public string Name { get; set; }
        public int Cost { get; protected set; }

        public ILocation MyLocation { get; set; }

        private bool IsValidCoordinates(ILocation location)
        {
            return this.MyLocation.MyCoordinates.X == location.MyCoordinates.X &&
                   this.MyLocation.MyCoordinates.Y == location.MyCoordinates.Y;
        }
        public void DriveTo(ILocation location)
        {
            if (IsValidCoordinates(location))
            {
                Console.WriteLine("Мы уже тут");
            }
            else
            {
                this.MyLocation = location;
                Console.WriteLine("Приехали. Наша новая локация {0}", this.MyLocation.Name);
            }
        }

        public abstract void PrintAllCharacteristics();

        public abstract string Serialize();
    }

    class Automobile : Vehicle // класс Автомобиль наследуется от Средства передвижения
    {
        public static class Corpstype //Тип кузова, корпуса
        {
            private static readonly string[] corpsTypesDefault;

            static Corpstype()
            {
                corpsTypesDefault = File.ReadAllLines("corpsTypesDefault.txt");
            }

            public static bool CheckCorpsTypesUser(string corpsTypeUser)
            {
                foreach (string corpsType in corpsTypesDefault)
                {
                    if (corpsType.Equals(corpsTypeUser))
                    {
                        return true;
                    }
                }
                return false;
            }

            public static void ShowAllCorpsAuto()
            {
                foreach (string corpsType in corpsTypesDefault)
                {
                    Console.WriteLine(corpsType);
                }
            }
        }

        public string CorpsType { get; private set; }
        public float VolumeEngine { get; private set; } // Объём двигателя

        public Automobile(string name, string corpsType, float volumeEngine, int cost, ILocation myLocation)
        {
            this.Name = name;
            this.CorpsType = corpsType;
            this.VolumeEngine = volumeEngine;
            this.Cost = cost;
            this.MyLocation = myLocation;
        }

        public override void PrintAllCharacteristics()
        {
            Console.WriteLine("Характеристики:\nТип кузова - {0}\nОбъём двигателя - {1}\nЦена - {2}", this.CorpsType, this.VolumeEngine, this.Cost);
        }

        public override string Serialize()
        {
            ListDictionary members = new ListDictionary();
            members["Name"] = this.Name;
            members["CorpsType"] = this.CorpsType;
            members["VolumeEngine"] = this.VolumeEngine;
            members["Cost"] = this.Cost;
            members["MyLocation"] = this.MyLocation.Name + "," + this.MyLocation.MyCoordinates.X +
               "," + this.MyLocation.MyCoordinates.Y;

            ICollection keys = members.Keys;

            string shape = "";
            string vehicleId;
            string location = "";
            int locationId = 0;

            foreach (string key in keys)
            {
                if (key != "MyLocation")
                {
                    shape += string.Format("{0}={1}\n", key, members[key]);
                }
                else 
                {
                    location = string.Format("{0}={1}", key, members[key]);
                    locationId = location.GetHashCode();
                } 
            }
            vehicleId = "[" + this.ToString() + "=" + shape.GetHashCode() + "]\n<" + locationId +">\n";

            return vehicleId + shape + location;
        }
    }

    class Bicycle : Vehicle // класс Велосипед наследуется от Средства передвижения
    {
        public int NumberGears { get; private set; }  // Кол-во передач

        public Bicycle(string name, int numberGears, int cost, ILocation myLocation)
        {
            this.Name = name;
            this.NumberGears = numberGears;
            this.Cost = cost;
            this.MyLocation = myLocation;
        }

        public override void PrintAllCharacteristics()
        {
            Console.WriteLine("Характеристики:\nКоличество передач - {0}\nЦена - {1}", this.NumberGears, this.Cost);
        }

        public override string Serialize()
        {
            ListDictionary members = new ListDictionary();
            members["Name"] = this.Name;
            members["NumberGears"] = this.NumberGears;
            members["Cost"] = this.Cost;
            members["MyLocation"] = this.MyLocation.Name + "," + this.MyLocation.MyCoordinates.X +
               "," + this.MyLocation.MyCoordinates.Y;

            ICollection keys = members.Keys;

            string shape = "";
            string vehicleId = "";

            foreach (string key in keys)
            {
                shape += string.Format("{0}={1}\n", key, members[key]);
            }
            vehicleId = "[" + this.ToString() + "=" + shape.GetHashCode() + "]\n";

            return vehicleId + shape;
        }
    }

    class VehicleFactory
    {
        public readonly int maxNumberGears;
        public ILocation MyLocation { get; set; }

        public VehicleFactory() {}
        public VehicleFactory(string name, int maxNumberGears, int x, int y)
        {
            this.maxNumberGears = maxNumberGears;
            this.MyLocation = new Location(name, x, y);
        }
        public void ShowAllCorpsAuto() // Показать все возможные кузова автомобилей
        {
            Automobile.Corpstype.ShowAllCorpsAuto();
        }

        public Vehicle CreateAutomobile(string name, string corpsType, float volumeEngine, int cost)
        {
            if (corpsType == "")
            {
                Console.WriteLine("Автомобиль без кузова фабрика не будет изготавливать.");
                return null;
            }

            if (Automobile.Corpstype.CheckCorpsTypesUser(corpsType))
            {
                Console.WriteLine("{0} успешно изготовлен", name);
                return new Automobile(name, corpsType, volumeEngine, cost, this.MyLocation);
            }
            Console.WriteLine("Автомобиль с данным типом кузова фабрика не может изготовить.");

            return null;
        }
        public Vehicle CreateBicycle(string name, int numberGears, int cost)
        {
            if (numberGears > 0 && numberGears <= this.maxNumberGears)
            {
                Console.WriteLine("{0} успешно изготовлен", name);
                return new Bicycle(name, numberGears, cost, this.MyLocation);
            }
            Console.WriteLine("С таким количеством передач не смогу собрать велосипед");

            return null;
        }
    }
}
