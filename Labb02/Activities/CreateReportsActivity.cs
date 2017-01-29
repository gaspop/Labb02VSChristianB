
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
	[Activity(Label = "@string/activityLabelCreateReport")]
	public class CreateReportsActivity : Activity
	{

		Button btnAccountReport;
		Button btnVATReport;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.CreateReportsActivity);
			SetupButtons();
		}

		private void SetupButtons()
		{
			btnAccountReport = FindViewById<Button>(Resource.Id.btnAccountReport);
			btnAccountReport.Click += delegate
			{
				Intent i = new Intent(this, typeof(AccountReportActivity));
				StartActivity(i);
			};

			btnVATReport = FindViewById<Button>(Resource.Id.btnVATReport);
			btnVATReport.Click += delegate
			{
				Intent i = new Intent(this, typeof(TaxReportActivity));
				StartActivity(i);
				                     
			};
		}
	}
}
