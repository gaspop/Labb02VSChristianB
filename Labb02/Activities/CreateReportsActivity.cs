﻿
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
	[Activity(Label = "CreateReportsActivity")]
	public class CreateReportsActivity : Activity
	{

		Button btnAccountReport;
		Button btnVATReport;
		TextView tvReport;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.CreateReportsActivity);

			tvReport = FindViewById<TextView>(Resource.Id.tvReport);
			SetupButtons();
		}

		private void SetupButtons()
		{
			btnAccountReport = FindViewById<Button>(Resource.Id.btnAccountReport);
			btnAccountReport.Click += delegate
			{
			};

			btnVATReport = FindViewById<Button>(Resource.Id.btnVATReport);
			btnVATReport.Click += delegate
			{
				tvReport.Text = BookkeeperManager.Instance.GetTaxReport();
			};
		}
	}
}
