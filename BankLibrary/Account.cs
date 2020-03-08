using System;
namespace BankLibrary
{
    public abstract class Account : IAccount
    {
        // Подія зняття грошей
        protected internal event AccountStateHandler Withdrawed;
        // Подія поповнення рахунку
        protected internal event AccountStateHandler Added;
        // Подія відкривання рахунку
        protected internal event AccountStateHandler Opened;
        // Подія закриття рахунку
        protected internal event AccountStateHandler Closed;
        // Подія нарахування відсотків 
        protected internal event AccountStateHandler Calculated;
        // Подія виведення інформації про рахунок 
        protected internal event AccountStateHandler Printed;

        static int counter = 0;
        protected int _days = 0; // час з моменту відкриття рахунку

        public Account(decimal sum, int percentage)
        {
            Sum = sum;
            Percentage = percentage;
            Id = ++counter;
        }

        // Поточна сума рахунку
        public decimal Sum { get; private set; }
        // Відсоток нарахування
        public int Percentage { get; private set; }
        // Ідентифікатор рахунку
        public int Id { get; private set; }
        // Виклик подій
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
                handler?.Invoke(this, e);
        }
        // Для кожної події визначається свій віртуальний метод
        protected virtual void OnOpened(AccountEventArgs e)
        {
            CallEvent(e, Opened);
        }
        protected virtual void OnWithdrawed(AccountEventArgs e)
        {
            CallEvent(e, Withdrawed);
        }
        protected virtual void OnAdded(AccountEventArgs e)
        {
            CallEvent(e, Added);
        }
        protected virtual void OnClosed(AccountEventArgs e)
        {
            CallEvent(e, Closed);
        }
        protected virtual void OnCalculated(AccountEventArgs e)
        {
            CallEvent(e, Calculated);
        }
        protected virtual void OnPrinted(AccountEventArgs e)
        {
            CallEvent(e, Printed);
        }

        public virtual void Put(decimal sum)
        {
            Sum += sum;
            OnAdded(new AccountEventArgs("На рахунок зараховано " + sum, sum));
        }
        // метод зняття з рахунку, повертає суму зняття з рахунку
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (Sum >= sum)
            {
                Sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"Суму {sum} знято з рахунку {Id}", sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостатньо грошей на рахунку {Id}", 0));
            }
            return result;
        }
        // відкриття рахунку
        protected internal virtual void Open()
        {
            OnOpened(new AccountEventArgs($"Відкритий новий рахунок! Id рахунку: {Id}", Sum));
        }
        // закриття рахунку
        protected internal virtual void Close()
        {
            OnClosed(new AccountEventArgs($"Рахунок {Id} закритий.  Баланс: {Sum}", Sum));
        }

        // виведення інформації про рахунок
        protected internal virtual void Print()
        {
            OnPrinted(new AccountEventArgs($"Рахунок ІД: {Id}. Баланс: {Sum}. Відкрито днів тому: {_days}", Sum));
        }

        protected internal void IncrementDays()
        {
            _days++;
        }
        // нарахування відсотків
        protected internal virtual void Calculate()
        {
            decimal increment = Sum * Percentage / 100;
            Sum = Sum + increment;
            OnCalculated(new AccountEventArgs($"Нараховані відсотки в розмірі: {increment}", increment));
        }
    }
}
