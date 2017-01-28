
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
	[Activity(Label = "@string/activityLabelTaxReport")]
	public class TaxReportActivity : Activity
	{
		TextView tvReport;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.TaxReportActivity);

			tvReport = FindViewById<TextView>(Resource.Id.tvReport);
			tvReport.Text = BookkeeperManager.Instance.GetTaxReport();
		}
	}
}
