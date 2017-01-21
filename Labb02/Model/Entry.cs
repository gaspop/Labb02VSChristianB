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

    public enum EventType { Income, Expense }

    public class Entry
    {
        public EventType type { get; set; }
        public int Sum { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public float VAT { get; set; }
    }

}