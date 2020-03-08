using System;
using BankLibrary;

namespace CSBankAplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Bank<Account> bank = new Bank<Account>("UkrBank");
            bool alive = true;
            while (alive)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen; // выводим список команд зеленым цветом
                Console.WriteLine("1. Відкрити рахунок \t 2. Зняти гроші  \t 3. Поповнити рахунок");
                Console.WriteLine("4. Закрити рахунок \t 5. Показати наявні рахунки \t 6. Пропустити день");
                Console.WriteLine("7. Закрити програму");
                Console.WriteLine("Виберіть дію:");
                Console.ForegroundColor = color;
                try
                {
                    int command = Convert.ToInt32(Console.ReadLine());

                    switch (command)
                    {
                        case 1:
                            OpenAccount(bank);
                            break;
                        case 2:
                            Withdraw(bank);
                            break;
                        case 3:
                            Put(bank);
                            break;
                        case 4:
                            CloseAccount(bank);
                            break;
                        case 5:
                            PrintAllAccounts(bank);
                            break;
                        case 6:
                            break;
                        case 7:
                            alive = false;
                            continue;
                    }
                    bank.CalculatePercentage();
                }
                catch (Exception ex)
                {
                    // виводимо повідомлення про помилку червоним кольором
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
        }

        private static void OpenAccount(Bank<Account> bank)
        {
            Console.WriteLine("Вкажіть суму для створення рахунку:");

            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Виберіть тип рахунку: 1. Звичайний 2. Депозит");
            AccountType accountType;

            int type = Convert.ToInt32(Console.ReadLine());

            if (type == 2)
                accountType = AccountType.Deposit;
            else
                accountType = AccountType.Ordinary;

            bank.Open(
                accountType,
                sum,
                AddSumHandler,  // обробник додавання грошей на рахунок
                WithdrawSumHandler, // обробник зняття грошей
                (o, e) => Console.WriteLine(e.Message), // обробник зарахування відсотків у вигляді лямбда-виразів
                CloseAccountHandler, // обработчик закрытия счета
                PrintAllAccountsHandler, // обробник виведення інформації
                OpenAccountHandler // обработчик открытия счета
            ); 
        }

        private static void Withdraw(Bank<Account> bank)
        {
            Console.WriteLine("Вкажіть суму для зняття з рахунку:");

            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введіть id рахунку:");
            int id = Convert.ToInt32(Console.ReadLine());

            bank.Withdraw(sum, id);
        }

        private static void Put(Bank<Account> bank)
        {
            Console.WriteLine("Вкажіть суму, яку необхіднго покласти на рахунок:");
            decimal sum = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Введіть Id рахунку:");
            int id = Convert.ToInt32(Console.ReadLine());
            bank.Put(sum, id);
        }

        private static void CloseAccount(Bank<Account> bank)
        {
            Console.WriteLine("Введіть id рахунку, який потрібно закрити:");
            int id = Convert.ToInt32(Console.ReadLine());

            bank.Close(id);
        }

        private static void PrintAllAccounts(Bank<Account> bank)
        {
            Console.WriteLine("У банку зараз відкриті наступні рахунки:");
            bank.Print(out bool isEmpty);

            if (isEmpty) {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("На даний час немає відкритих рахунків.");
                Console.ForegroundColor = color;
            }
        }


        // Обробники події класу Account

        // обробник відкриття рахунку
        private static void OpenAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        // обробник додавання грошей на рахунок
        private static void AddSumHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        // обробник зняття грошей
        private static void WithdrawSumHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.Sum > 0)
                Console.WriteLine("Ідемо тратити гроші");
        }
        // обробник показування наявних акаунтів
        private static void PrintAllAccountsHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        // обробник закритя рахунку
        private static void CloseAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
