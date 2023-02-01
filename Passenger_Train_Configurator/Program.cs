using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passenger_Train_Configurator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isWork = true;
            Train newTrain = new Train();

            while (isWork)
            {
                Console.WriteLine("Добро пожаловать в конфигуратор пассажирских поездов!\n");
                newTrain.ShowInfo();
                Console.WriteLine("\nВведите 1 чтобы создать направление");
                Console.WriteLine("Введите 2 чтобы продать билеты");
                Console.WriteLine("Введите 3 чтобы сформировать поезд");
                Console.WriteLine("Введите 4 чтобы отправить поезд");
                Console.WriteLine("Введите 5 чтобы завершить работу");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        newTrain.CreateDirection();
                        break;

                    case "2":
                        newTrain.SellTickets();
                        break;

                    case "3":
                        newTrain.Create();
                        break;

                    case "4":
                        newTrain.Sent();
                        break;

                    case "5":
                        Console.WriteLine("Завершение работы!");
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Такого пункта в меню нет!");
                        break;
                }

                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    class Train
    {
        private List<Wagon> _train = new List<Wagon>();
        private Direction _direction;
        private Passenger _passengers;

        public void CreateDirection()
        {
            if (_direction == null)
            {
                Console.WriteLine("Введите направление для нового поезда:");
                _direction = new Direction(Console.ReadLine());
            }
            else
            {
                Console.WriteLine("Направление уже создано, можете продавать билеты!");
            }
        }

        public void SellTickets()
        {
            if (_passengers == null)
            {
                Random random = new Random();
                _passengers = new Passenger(random);
                Console.WriteLine($"Билеты успешно проданы. Всего пассажиров, купивших билеты - {_passengers.Quantity}");
            }
            else
            {
                Console.WriteLine($"Вы уже продали билеты на направление {_direction.Name} . Сформируйте поезд!");
            }
        }

        public void ShowInfo()
        {
            if (_direction != null)
            {
                Console.WriteLine($"Текущее направление - {_direction.Name}.");
            }

            if (_passengers != null)
            {
                Console.WriteLine($"Колиство пассажиров не размещённых в поезде - {_passengers.Quantity}");
            }

            if (_train.Count > 0)
            {
                Console.WriteLine($"Количество присоединённых вагонов - {_train.Count}.");
            }
        }

        public void Create()
        {
            Random random = new Random();

            while (_passengers.Quantity > 0)
            {
                ShowInfo();
                Wagon newWagon = new Wagon(random);
                Console.WriteLine($"Перед вам вагон вместительностью {newWagon.Capacity} мест. Нажмите любую клавишу, чтобы присоединить его к поезду:");
                Console.ReadKey();
                _passengers.TakeThePlaces(newWagon);
                _train.Add(newWagon);
                Console.WriteLine("Вагон успешно присоединён к поезду");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public void Sent()
        {
            Console.WriteLine($"Поезд по направлению {_direction.Name} успешно отправлен! Можете создать новое направление.");
            _train.Clear();
            _direction= null;
            _passengers= null;
        }
    }

    class Wagon
    {
        public Wagon(Random random)
        {
            Capacity = random.Next(10, 20);
        }

        public int Capacity { get; private set; }
    }

    class Direction
    {
        public Direction(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }

    class Passenger
    {
        public Passenger(Random passengersCount)
        {
            Quantity = passengersCount.Next(20, 100);
        }

        public int Quantity { get; private set; }

        public int TakeThePlaces(Wagon wagon)
        {
            if (Quantity > 0)
            {
                if (Quantity > wagon.Capacity)
                {
                    return Quantity -= wagon.Capacity;

                }
                else
                {
                    return Quantity = 0;
                }
            }
            else
            {
                return Quantity = 0;
            }
        }
    }
}
