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

    public enum EntryType { Income = 1 , Expense = 2 }

    public class Entry
    {
        public EntryType Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Account AccountType { get; set; }
        public Account AccountTarget { get; set; }
        public int SumTotal { get; set; }
        public TaxRate VAT { get; set; }

		public override string ToString()
		{
			string s = "Type: " + Type;
			s += "\n" +"Date: " + Date.ToLongDateString();
			s += "\n" + "Description: " + Description;
			s += "\n" + "Account Type: " + AccountType;
			s += "\n" + "Account Target: " + AccountTarget;
			s += "\n" + "Sum Total: " + SumTotal;
			s += "\n" + "Tax Rate: " + VAT;

			return s;
		}
    }

}