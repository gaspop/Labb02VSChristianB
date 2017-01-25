using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;

using Labb02.Model;

namespace Labb02
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

		public static readonly string TAG = "MainActivity";

        Button btnNewEvent;
		Button btnShowAllEvents;
		//Button btnCreateReports;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.MainActivity);
			setupButtons();

        }

		protected override void OnResume()
		{
			base.OnResume();
			Log.Debug(TAG, BookkeeperManager.Instance.EntryToString());
		}

        private void setupButtons()
		{
			btnNewEvent = FindViewById<Button>(Resource.Id.btnNewEvent);
			btnShowAllEvents = FindViewById<Button>(Resource.Id.btnShowAllEvents);
			//btnCreateReports = FindViewById<Button>(Resource.Id.btnCreateReports);

			btnNewEvent.Click += delegate
			{
				Intent i = new Intent(this, typeof(EntryActivity));
				StartActivity(i);
			};

			btnShowAllEvents.Click += delegate
			{
				Intent i = new Intent(this, typeof(EntryListActivity));
				StartActivity(i);
			};
            /*
			btnCreateReports.Click += delegate
			{
				//Intent i = new Intent(this, typeof(EntryActivity));
				//StartActivity(i);
			};
            */

		}

    }
}