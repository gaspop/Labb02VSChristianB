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
	public class Entry : Java.Lang.Object
    {
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; private set; }
        public EntryType Type { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int TypeAccountNumber { get; set; }
        public int MoneyAccountNumber { get; set; }
        public int SumTotal { get; set; }
        public float Rate { get; set; }
    }


    public enum EntryType
    {
        Income = 1,
        Expense = 2,
    }

}