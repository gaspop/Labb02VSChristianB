
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
using Android.Util;

using Labb02.Model;

namespace Labb02
{
	[Activity(Label = "EventActivity")]
	public class EventActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Event);
            // Create your application here

            Log.Debug("Event", string.Join("\n", BookkeeperManager.Instance.MoneyAccounts));

        }

        private void AddEntry(Entry entry)
        {

        }

    }
}
