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

using SQLite;

namespace Bookkeeper.Model
{
	public enum AccountType
	{
		Income = 1,
		Expense = 2,
		Money = 3,
	}

    public class Account : Java.Lang.Object
    {
		[PrimaryKey, Column("_id")]
		public int Number { get; set; }
		public string Name { get; set; }
		public AccountType Type { get; set; }

        public override string ToString()
        {
            return Number + ": " + Name;
        }

    }
}