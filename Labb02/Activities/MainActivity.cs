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
        BookkeeperManager manager;

        Button btnNewEvent;
		Button btnShowAllEvents;
		Button btnCreateReports;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
			setupButtons();
            Log.Debug("Main", string.Join("\n", BookkeeperManager.Instance.MoneyAccounts));

        }

        private void setupButtons()
		{
			btnNewEvent = FindViewById<Button>(Resource.Id.btnNewEvent);
			btnShowAllEvents = FindViewById<Button>(Resource.Id.btnShowAllEvents);
			btnCreateReports = FindViewById<Button>(Resource.Id.btnCreateReports);

			btnNewEvent.Click += delegate
			{
				Intent i = new Intent(this, typeof(EventActivity));
				StartActivity(i);
			};
		}

    }
}