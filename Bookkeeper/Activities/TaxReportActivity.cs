
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
	[Activity(Label = "@string/activityLabelTaxReport")]
	public class TaxReportActivity : Activity
	{
        BookkeeperManager manager;
        TableLayout tableReport;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TaxReportActivity);

            tableReport = FindViewById<TableLayout>(Resource.Id.tableReport);
            manager = BookkeeperManager.Instance;

            OrganizeTaxReport();
        }

        private void OrganizeTaxReport()
        {
			int totalSum = 0;
            string[] splitter = new string[] { "\n\n" };
            string[] report = manager.GetTaxReport().Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < report.Length; i++)
            {
                string[] reportEvent = report[i].Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
				totalSum += Convert.ToInt32(reportEvent[2]);
                TableRow tr = (TableRow)View.Inflate(this, Resource.Layout.ReportTableRow, null);
                TextView tvRowDate = tr.FindViewById<TextView>(Resource.Id.rowDate);
                TextView tvRowDescription = tr.FindViewById<TextView>(Resource.Id.rowDescription);
                TextView tvRowSum = tr.FindViewById<TextView>(Resource.Id.rowSum);
                tvRowDate.Text = reportEvent[0];
                tvRowDescription.Text = reportEvent[1];
                tvRowSum.Text = reportEvent[2];

                tableReport.AddView(tr);
				if (i == report.Length -1)
					DisplaySum(totalSum);
            }
        }

		private void DisplaySum(int sum)
		{
			TableRow tr = (TableRow)View.Inflate(this, Resource.Layout.ReportTableRow, null);
			TextView tvRowDescription = tr.FindViewById<TextView>(Resource.Id.rowDescription);
			TextView tvRowSum = tr.FindViewById<TextView>(Resource.Id.rowSum);
			tvRowDescription.Text = GetString(Resource.String.taxReportTotalVAT);
			tvRowSum.Text = string.Format("{0}", sum);
			tr.SetPadding(0, 32, 0, 0);
			tableReport.AddView(tr);
		}
    }
}
