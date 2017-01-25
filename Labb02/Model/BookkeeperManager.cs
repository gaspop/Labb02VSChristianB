using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Labb02.Model
{
    public class BookkeeperManager
    {
        private static BookkeeperManager instance;

        public static BookkeeperManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookkeeperManager();
                }
                return instance;
            }
        }

        private List<Entry> entries;
        private List<Account> incomeAccounts;
        private List<Account> expenseAccounts;
        private List<Account> moneyAccounts;
        private List<TaxRate> taxRates;

        private BookkeeperManager()
        {
            entries = new List<Entry>();
            incomeAccounts = new List<Account>();
            expenseAccounts = new List<Account>();
            moneyAccounts = new List<Account>();
            taxRates = new List<TaxRate>();

            incomeAccounts.Add(new Account("Försäljning", 1));
            incomeAccounts.Add(new Account("Skumaffärer", 2));

            expenseAccounts.Add(new Account("Inköp", 1));
            moneyAccounts.Add(new Account("Kassa", 1));

            taxRates.Add(new TaxRate(6));
			taxRates.Add(new TaxRate(4));
        }

        public List<Entry> Entries
        {
            get { return entries; }
        }

        public List<Account> IncomeAccounts
        {
            get { return incomeAccounts; }
        }

        public List<Account> ExpenseAccounts
        {
            get { return expenseAccounts; }
        }

        public List<Account> MoneyAccounts
        {
            get { return moneyAccounts; }
        }

        public List<TaxRate> TaxRates
        {
            get { return taxRates; }
        }

        public void AddEntry(Entry entry)
        {
            entries.Add(entry);
        }

		public string EntryToString()
		{
			return string.Join("\n", Entries);
		}

    }
}