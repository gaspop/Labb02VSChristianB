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
    public class TaxRate
    {

        public float VAT { get; }

        public TaxRate (float vat)
        {
            VAT = vat;
        }

    }
}