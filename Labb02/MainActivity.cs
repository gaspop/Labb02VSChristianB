using Android.App;
using Android.Widget;
using Android.OS;

namespace Labb02
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView tv;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            tv = FindViewById<TextView>(Resource.Id.tv1);
            tv.Text = "Hej hej!";

        }
    }
}

