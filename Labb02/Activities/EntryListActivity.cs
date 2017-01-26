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

using Labb02.Model;

namespace Labb02
{
	[Activity(Label = "EntryListActivity")]
	public class EntryListActivity : Activity
	{
        ListView lvEntries;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.EntryListActivity);
            EntryAdapter adapter = new EntryAdapter(this);
            lvEntries = FindViewById<ListView>(Resource.Id.lvEntries);
            lvEntries.Adapter = adapter;
		}
	}
}