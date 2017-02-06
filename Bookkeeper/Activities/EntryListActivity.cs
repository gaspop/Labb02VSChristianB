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

using Bookkeeper.Model;

namespace Bookkeeper
{
	[Activity(Label = "@string/activityLabelEntryList")]
	public class EntryListActivity : Activity
	{
        ListView lvEntries;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.EntryListActivity);

			SetupListView();
		}

		protected override void OnResume()
		{
			base.OnResume();
			lvEntries.Adapter = new EntryAdapter(this);
		}

		private void SetupListView()
		{
			EntryAdapter adapter = new EntryAdapter(this);
			lvEntries = FindViewById<ListView>(Resource.Id.lvEntries);
			lvEntries.Adapter = adapter;
			lvEntries.Clickable = true;

			lvEntries.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
			{
				Entry entry = (Entry)lvEntries.GetItemAtPosition(e.Position);
				Intent intent = new Intent(this, typeof(EntryActivity));
				intent.PutExtra("EXTRA_EDIT", true);
				intent.PutExtra("EXTRA_ID", entry.Id);
				StartActivity(intent);
			};
		}

	}
}