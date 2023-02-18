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
            const string CommandCreateDirection = "1";
            const string CommandSellTickets = "2";
            const string CommandCreateTrain = "3";
            const string CommandSentTrain = "4";
            const string CommandExit = "5";

            bool isWork = true;
            Train train = new Train();
            Dispatcher dispatcher = new Dispatcher();

            while (isWork)
            {
                Console.WriteLine($"Добро пожаловать в конфигуратор пассажирских поездов!\n");
                dispatcher.ShowInfo();
                train.ShowInfo();
                Console.WriteLine($"\nВведите {CommandCreateDirection} чтобы создать направление");
                Console.WriteLine($"Введите {CommandSellTickets} чтобы продать билеты");
                Console.WriteLine($"Введите {CommandCreateTrain} чтобы сформировать поезд");
                Console.WriteLine($"Введите {CommandSentTrain} чтобы отправить поезд");
                Console.WriteLine($"Введите {CommandExit} чтобы завершить работу");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandCreateDirection:
                        dispatcher.CreateDirection();
                        break;

                    case CommandSellTickets:
                        dispatcher.SellTickets();
                        break;

                    case CommandCreateTrain:
                        train.Create(dispatcher);
                        break;

                    case CommandSentTrain:
                        train.Sent(dispatcher);
                        break;

                    case CommandExit:
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
        protected Direction _direction;
        protected Tickets _tickets;

        public void ShowInfo()
        {
            if (_direction != null)
            {
                Console.WriteLine($"Текущее направление - {_direction.Name}.");
            }

            if (_tickets != null)
            {
                Console.WriteLine($"Колиство пассажиров не размещённых в поезде - {_tickets.Quantity}");
            }

            if (_train.Count > 0)
            {
                Console.WriteLine($"Количество присоединённых вагонов - {_train.Count}.");
            }
        }

        public void Create(Dispatcher dispatcher)
        {
            Random random = new Random();

            while (dispatcher.Tiskets.Quantity > 0)
            {
                dispatcher.ShowInfo();
                ShowInfo();
                Wagon newWagon = new Wagon(random);
                Console.WriteLine($"Перед вам вагон вместительностью {newWagon.Capacity} мест. Нажмите любую клавишу, чтобы присоединить его к поезду:");
                Console.ReadKey();
                TakePlaces(newWagon, dispatcher.Tiskets);
                _train.Add(newWagon);
                Console.WriteLine("Вагон успешно присоединён к поезду");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public void Sent(Dispatcher dispatcher)
        {
            Console.WriteLine($"Поезд успешно отправлен! Можете создать новое направление.");
            _train.Clear();
            dispatcher.SentTrain();
        }

        private void TakePlaces(Wagon wagon, Tickets passengers)
        {
            int passengersQuantity = passengers.Quantity;

            if(passengersQuantity > 0)
            {
                if(passengersQuantity > wagon.Capacity)
                {
                    passengersQuantity -= wagon.Capacity;
                    passengers.FixQuantity(passengersQuantity);
                }
                else
                {
                    passengers.ZeroizeQuantity();
                }
            }
            else
            {
                passengers.ZeroizeQuantity();
            }
        }
    }

    class Wagon
    {
        public Wagon(Random random)
        {
            int minCapacity = 10;
            int maxCapacity = 20;
            Capacity = random.Next(minCapacity, maxCapacity);
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

    class Tickets
    {
        public Tickets(Random passengersCount)
        {
            int minPassengersCount = 20;
            int maxPassengersCount = 100;
            Quantity = passengersCount.Next(minPassengersCount, maxPassengersCount);
        }

        public int Quantity { get; private set; }

        public void FixQuantity(int newQuantity)
        {
            Quantity= newQuantity;
        }

        public void ZeroizeQuantity()
        {
            Quantity= 0;
        }
    }

    class Dispatcher
    {
        private Direction _direction;

        public Tickets Tiskets { get; private set; }

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
            if (Tiskets == null)
            {
                Random random = new Random();
                Tiskets = new Tickets(random);
                Console.WriteLine($"Билеты успешно проданы. Всего пассажиров, купивших билеты - {Tiskets.Quantity}");
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

            if (Tiskets != null)
            {
                Console.WriteLine($"Колиство пассажиров не размещённых в поезде - {Tiskets.Quantity}");
            }
        }

        public void SentTrain()
        {
            _direction = null;
            Tiskets = null;
        }
    }
}
