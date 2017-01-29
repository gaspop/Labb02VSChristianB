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

namespace Labb02.Model
{
    class EntryAdapter: BaseAdapter<Entry>
    {
        Activity activity;
        List<Entry> list;

        public EntryAdapter(Activity activity)
        {
            this.activity = activity;
			list = BookkeeperManager.Instance.Entries.OrderBy(e => e.Date).ToList();
        }

		public override Entry this[int position]
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
            View view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.EntryListItem, parent, false);
           
            TextView tvDate = view.FindViewById<TextView>(Resource.Id.entryItemDate);
            TextView tvDescription = view.FindViewById<TextView>(Resource.Id.entryItemDescription);
            TextView tvSum = view.FindViewById<TextView>(Resource.Id.entryItemSum);

            Entry entry = list[position];
            tvDate.Text = entry.Date.ToString("yyyy-MM-dd");
            tvDescription.Text = entry.Description;
            tvSum.Text = entry.SumTotal + " kr";

			if (entry.Type == EntryType.Income)
				tvSum.SetTextColor(new Color(76, 175, 80));
			else
				tvSum.SetTextColor(new Color(211, 47, 47));

            return view;
		}

    }
}
