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
	
    public class TaxRate : Java.Lang.Object
    {
		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; private set; }
		public float Rate { get; set; }

        public override string ToString()
        {
			return (Rate * 100) + "%";
        }

    }
}