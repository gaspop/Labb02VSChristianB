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

using Bookkeeper.Model;

namespace Bookkeeper
{
	[Activity(Label = "@string/activityLabelAccountReport")]
	public class AccountReportActivity : Activity
	{

		ListView lvAccounts;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.AccountReportActivity);

			AccountAdapter adapter = new AccountAdapter(this);
			lvAccounts = FindViewById<ListView>(Resource.Id.lvAccounts);
			lvAccounts.Adapter = adapter;
		}

	}
}
