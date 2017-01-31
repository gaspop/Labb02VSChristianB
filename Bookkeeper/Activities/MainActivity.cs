using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;

using Bookkeeper.Model;

namespace Bookkeeper
{
    [Activity(MainLauncher = true, Icon = "@drawable/bookkeeperIcon")]
    public class MainActivity : Activity
    {

		public static readonly string TAG = "MainActivity";

        Button btnNewEvent;
		Button btnShowAllEvents;
		Button btnCreateReports;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView (Resource.Layout.MainActivity);

			SetTitle(Resource.String.activityLabelMain);
			SetupButtons();

        }

        private void SetupButtons()
		{
			btnNewEvent = FindViewById<Button>(Resource.Id.btnNewEvent);
			btnShowAllEvents = FindViewById<Button>(Resource.Id.btnShowAllEvents);
			btnCreateReports = FindViewById<Button>(Resource.Id.btnCreateReports);

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
            
			btnCreateReports.Click += delegate
			{
				Intent i = new Intent(this, typeof(CreateReportsActivity));
				StartActivity(i);
			};
		}
    }
}