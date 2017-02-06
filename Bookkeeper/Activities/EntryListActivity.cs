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
    
        BookkeeperManager manager;
        ListView lvEntries;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.EntryListActivity);
            SetupActivity();
        }

		protected override void OnResume()
		{
			base.OnResume();
            if (manager.Entries.Count > 0)
                lvEntries.Adapter = new EntryAdapter(this);
		}

        private void SetupActivity()
        {
            manager = BookkeeperManager.Instance;
            if (manager.Entries.Count > 0)
            {
                SetupListView();
            }
            else
            {
                TextView tvMessage = FindViewById<TextView>(Resource.Id.tvMessage);
                tvMessage.Text = GetString(Resource.String.entryListNoEvents);
            }
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