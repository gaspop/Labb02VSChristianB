using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;

namespace Bookkeeper
{
    [Activity(MainLauncher = true, Icon = "@drawable/bookkeeperIcon")]
    public class MainActivity : Activity
    {

        Button btnNewEvent;
		Button btnShowAllEvents;
		Button btnCreateReports;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView (Resource.Layout.MainActivity);
            SetupActivity();
        }

        private void SetupActivity()
		{
            SetTitle(Resource.String.activityLabelMain);
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