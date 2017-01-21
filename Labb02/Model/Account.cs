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
    public class Account
    {
        public string Name { get; }
        public int Number { get; }

        public Account (string name, int number)
        {
            Name = name;
            Number = number;
        }

        public override string ToString()
        {
            return "Name: " + Name + "\nNumber: " + Number;
        }

    }
}