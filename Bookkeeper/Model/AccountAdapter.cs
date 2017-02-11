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
using Android.Graphics;
using Android.Util;

namespace Bookkeeper.Model
{
	class AccountAdapter : BaseAdapter<Account>
	{
		BookkeeperManager manager;
		Activity activity;
		List<Account> list;

		public AccountAdapter(Activity activity)
		{
			manager = BookkeeperManager.Instance;
			this.activity = activity;
			list = manager.Accounts.OrderBy(a => a.Type).ToList();
		}

		public override Account this[int position]
		{
			get { return list[position]; }
		}

		public override int Count
		{
			get
			{
				return list.Count;
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.AccountReportItem, parent, false);

			TextView tvName = view.FindViewById<TextView>(Resource.Id.reportAccount);
			TableLayout tlEntries = view.FindViewById<TableLayout>(Resource.Id.reportTableEntries);
			LinearLayout llResult = view.FindViewById<LinearLayout>(Resource.Id.reportResult);
			TextView tvSum = view.FindViewById<TextView>(Resource.Id.reportTotalSum);

			Account account = list[position];
			List<Entry> entries = manager.Entries.OrderBy(e => e.Date)
						.Where(e => e.TypeAccountNumber == account.Number
	             	             || e.MoneyAccountNumber == account.Number).ToList();
			Log.Debug("AccountAdapter", account.Name + ": " + entries.Count);

			tvName.Text = account.Name + " (" + account.Number + ")";
			tlEntries.RemoveAllViews();
			if (entries.Count > 0)
			{
				int TotalSum = 0;
				for (int i = 0; i < entries.Count; i++)
				{
					entries[i].TotalSum *= (entries[i].Type == EntryType.Income) ? 1 : (-1);
					TotalSum += entries[i].TotalSum;
					tlEntries.AddView(NewTableRow(entries[i]));
				}
				tvSum.Text = TotalSum + " kr";
				tlEntries.Visibility = ViewStates.Visible;
				llResult.Visibility = ViewStates.Visible;
			}
			else
			{
				tlEntries.Visibility = ViewStates.Gone;
				llResult.Visibility = ViewStates.Gone;
			}

			view.SetBackgroundColor
			  (position % 2 == 0 ? new Color(56, 56, 56) : view.DrawingCacheBackgroundColor);
			return view;
				
		}

		private TableRow NewTableRow(Entry e)
		{
			TableRow tr = (TableRow)View.Inflate(activity, Resource.Layout.ReportTableRow, null);
			TextView tvRowDate = tr.FindViewById<TextView>(Resource.Id.rowDate);
			TextView tvRowDescription = tr.FindViewById<TextView>(Resource.Id.rowDescription);
			TextView tvRowSum = tr.FindViewById<TextView>(Resource.Id.rowSum);

			tvRowDate.Text = e.Date.ToString("yyyy-MM-dd");
			tvRowDescription.Text = e.Description;
			tvRowSum.Text = string.Format("{0}", e.TotalSum);
			tvRowSum.SetTextColor
			        (e.Type == EntryType.Income ? Color.ForestGreen : Color.OrangeRed);

			return tr;
		}
	}
}
