﻿using System;
namespace BankLibrary
{
    public class DemandAccount : Account
    {
        public DemandAccount(decimal sum, int percentage) : base(sum, percentage)
        {
        }

        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Відкритий новий звичайний рахунок! Id рахунку: {this.Id}", this.Sum));
        }
    }
}
