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

        public int VAT { get; }

        public TaxRate (int vat)
        {
            VAT = vat;
        }

        public override string ToString()
        {
            return VAT + "%";
        }

    }
}