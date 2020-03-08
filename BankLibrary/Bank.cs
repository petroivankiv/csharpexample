using System;
namespace BankLibrary
{
    // тип рахунку
    public enum AccountType
    {
        Ordinary,
        Deposit
    }

    public class Bank<T> where T : Account
    {
        T[] accounts;

        public string Name { get; private set; }

        public Bank(string name)
        {
            Name = name;
        }

        // метод створення рахунку
        public void Open(AccountType accountType, decimal sum,
            AccountStateHandler addSumHandler, AccountStateHandler withdrawSumHandler,
            AccountStateHandler calculationHandler, AccountStateHandler closeAccountHandler,
            AccountStateHandler printAccountsHandler, AccountStateHandler openAccountHandler)
        {
            T newAccount = null;

            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAccount(sum, 1) as T;
                    break;
                case AccountType.Deposit:
                    newAccount = new DepositAccount(sum, 40) as T;
                    break;
            }

            if (newAccount == null)
                throw new Exception("Помилка створення рахунку");
            // додаємо новий рахунок у масив рахунків      
            if (accounts == null)
                accounts = new T[] { newAccount };
            else
            {
                T[] tempAccounts = new T[accounts.Length + 1];

                for (int i = 0; i < accounts.Length; i++)
                    tempAccounts[i] = accounts[i];
                tempAccounts[tempAccounts.Length - 1] = newAccount;
                accounts = tempAccounts;
            }
            // встановлення обробників подій рахунків
            newAccount.Added += addSumHandler;
            newAccount.Withdrawed += withdrawSumHandler;
            newAccount.Closed += closeAccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Calculated += calculationHandler;
            newAccount.Printed += printAccountsHandler;

            newAccount.Open();
        }

        // додавання грошей до рахунку
        public void Put(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Рахунок не знайдено");
            account.Put(sum);
        }

        // зняття грошей
        public void Withdraw(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Рахунок не знайдено");
            account.Withdraw(sum);
        }

        // закриття рахунку
        public void Close(int id)
        {
            int index;
            T account = FindAccount(id, out index);
            if (account == null)
                throw new Exception("Рахунок не знайдено");

            account.Close();

            if (accounts.Length <= 1)
                accounts = null;
            else
            {
                // зменшуємо масив рахунків, видаляючи з нього закритий рахунок
                T[] tempAccounts = new T[accounts.Length - 1];
                for (int i = 0, j = 0; i < accounts.Length; i++)
                {
                    if (i != index)
                        tempAccounts[j++] = accounts[i];
                }
                accounts = tempAccounts;
            }
        }

        // виведення інформації
        public void Print(out bool isEmpty)
        {
            if (accounts == null)
            {
                isEmpty = true;
                return;
            }

            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].Print();
            }

            isEmpty = false;
        }

        // зараховуємо відсотки для рахунків
        public void CalculatePercentage()
        {
            if (accounts == null)
                return;
            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].IncrementDays();
                accounts[i].Calculate();
            }
        }

        // пошук рахунку по id
        public T FindAccount(int id)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                    return accounts[i];
            }
            return null;
        }
        // перевантажена версія пошуку рахунку
        public T FindAccount(int id, out int index)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                {
                    index = i;
                    return accounts[i];
                }
            }
            index = -1;
            return null;
        }
    }
}
